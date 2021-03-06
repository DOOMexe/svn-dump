using GodLesZ.Library.Amf.Messaging.Rtmp.Event;
using GodLesZ.Library.Amf.Messaging.Rtmp.Stream.Messages;
using log4net;

namespace GodLesZ.Library.Amf.Messaging.Rtmp.Stream {
	/// <summary>
	/// State machine for video frame dropping in live streams.
	/// <p>
	/// We start sending all frame types. Disposable interframes can be dropped any
	/// time without affecting the current state. If a regular interframe is dropped,
	/// all future frames up to the next keyframes are dropped as well. Dropped
	/// keyframes result in only keyframes being sent. If two consecutive keyframes
	/// have been successfully sent, regular interframes will be sent in the next
	/// iteration as well. If these frames all went through, disposable interframes
	/// are sent again.
	/// </p>
	/// So from highest to lowest bandwidth and back, the states go as follows:
	/// <ul>
	/// <li>all frames</li>
	/// <li>keyframes and interframes</li>
	/// <li>keyframes</li>
	/// <li>keyframes and interframes</li>
	/// <li>all frames</li>
	/// </ul>
	/// </summary>
	class VideoFrameDropper : IFrameDropper {
		private static ILog log = LogManager.GetLogger(typeof(VideoFrameDropper));

		/// <summary>
		/// Current state.
		/// </summary>
		private FrameDropperState _state;

		public VideoFrameDropper() {
			Reset();
		}

		#region IFrameDropper Members

		public bool CanSendPacket(RtmpMessage message, long pending) {
			IRtmpEvent packet = message.body;
			if (!(packet is VideoData)) {
				// We currently only drop video packets.
				return true;
			}

			VideoData video = packet as VideoData;
			FrameType type = video.FrameType;
			bool result = false;
			switch (_state) {
				case FrameDropperState.SEND_ALL:
					// All packets will be sent.
					result = true;
					break;
				case FrameDropperState.SEND_INTERFRAMES:
					// Only keyframes and interframes will be sent.
					if (type == FrameType.Keyframe) {
						if (pending == 0) {
							// Send all frames from now on.
							_state = FrameDropperState.SEND_ALL;
						}
						result = true;
					} else if (type == FrameType.Interframe) {
						result = true;
					}
					break;
				case FrameDropperState.SEND_KEYFRAMES:
					// Only keyframes will be sent.
					result = (type == FrameType.Keyframe);
					if (result && pending == 0) {
						// Maybe switch back to SEND_INTERFRAMES after the next keyframe
						_state = FrameDropperState.SEND_KEYFRAMES_CHECK;
					}
					break;
				case FrameDropperState.SEND_KEYFRAMES_CHECK:
					// Only keyframes will be sent.
					result = (type == FrameType.Keyframe);
					if (result && pending == 0) {
						// Continue with sending interframes as well
						_state = FrameDropperState.SEND_INTERFRAMES;
					}
					break;
				default:
					break;
			}
			return result;
		}

		public void DropPacket(RtmpMessage message) {
			IRtmpEvent packet = message.body;
			if (!(packet is VideoData)) {
				// Only check video packets.
				return;
			}

			VideoData video = packet as VideoData;
			FrameType type = video.FrameType;

			switch (_state) {
				case FrameDropperState.SEND_ALL:
					if (type == FrameType.DisposableInterframe) {
						// Remain in state, packet is safe to drop.
						return;
					} else if (type == FrameType.Interframe) {
						// Drop all frames until the next keyframe.
						_state = FrameDropperState.SEND_KEYFRAMES;
						return;
					} else if (type == FrameType.Keyframe) {
						// Drop all frames until the next keyframe.
						_state = FrameDropperState.SEND_KEYFRAMES;
						return;
					}
					break;
				case FrameDropperState.SEND_INTERFRAMES:
					if (type == FrameType.Interframe) {
						// Drop all frames until the next keyframe.
						_state = FrameDropperState.SEND_KEYFRAMES_CHECK;
						return;
					} else if (type == FrameType.Keyframe) {
						// Drop all frames until the next keyframe.
						_state = FrameDropperState.SEND_KEYFRAMES;
						return;
					}
					break;
				case FrameDropperState.SEND_KEYFRAMES:
					// Remain in state.
					break;
				case FrameDropperState.SEND_KEYFRAMES_CHECK:
					if (type == FrameType.Keyframe) {
						// Switch back to sending keyframes, but don't move to SEND_INTERFRAMES afterwards.
						_state = FrameDropperState.SEND_KEYFRAMES;
						return;
					}
					break;
				default:
					break;
			}
		}

		public void SendPacket(RtmpMessage message) {
		}

		public void Reset() {
			Reset(FrameDropperState.SEND_ALL);
		}

		public void Reset(FrameDropperState state) {
			_state = state;
		}

		#endregion
	}
}
