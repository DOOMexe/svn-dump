using GodLesZ.Library.Amf.Messaging.Api.Stream;
using GodLesZ.Library.Amf.Util;
using log4net;

namespace GodLesZ.Library.Amf.Messaging.Rtmp.Stream.Codec {
	class AVCVideo : IVideoStreamCodec {
		private static ILog log = LogManager.GetLogger(typeof(AVCVideo));

		/// <summary>
		/// AVC video codec constant.
		/// </summary>
		static string CodecName = "AVC";
		/// <summary>
		/// Block of data (AVC DecoderConfigurationRecord).
		/// </summary>
		private byte[] _blockDataAVCDCR;
		/// <summary>
		/// Data block size (AVC DecoderConfigurationRecord).
		/// </summary>
		private int _blockSizeAVCDCR;
		/// <summary>
		/// Block of data (last KeyFrame).
		/// </summary>
		private byte[] _blockDataLKF;
		/// <summary>
		/// Data block size (last KeyFrame).
		/// </summary>
		private int _blockSizeLKF;
		/// <summary>
		/// Number of data blocks (last KeyFrame).
		/// </summary>
		private int _dataCountLKF;
		/// <summary>
		/// Number of data blocks (Decoder Configuration Record).
		/// </summary>
		private int _dataCountAVCDCR;

		public AVCVideo() {
			Reset();
		}

		#region IVideoStreamCodec Members

		public string Name {
			get { return AVCVideo.CodecName; }
		}

		public void Reset() {
			_blockDataLKF = null;
			_blockSizeLKF = 0;
			_blockSizeAVCDCR = 0;
			_blockDataAVCDCR = null;
			_dataCountLKF = 0;
			_dataCountAVCDCR = 0;
		}

		public bool CanDropFrames {
			get { return true; }
		}

		public bool CanHandleData(ByteBuffer data) {
			if (data.Limit == 0)
				return false;// Empty buffer
			byte first = data.Get();
			bool result = ((first & 0x0f) == VideoCodec.AVC.Id);
			data.Rewind();
			return result;
		}

		public bool AddData(ByteBuffer data) {
			if (data.Limit == 0)
				return true;
			if (!CanHandleData(data))
				return false;
			data.Rewind();
			byte frameType = data.Get();
			data.Rewind();
			if ((frameType & 0xf0) != VideoCodec.FLV_FRAME_KEY)
				return true;// Not a keyframe

			//If we don't have the AVCDecoderConfigurationRecord stored...
			if (_blockDataAVCDCR == null) {
				data.Get();//FT
				data.Get();//CODECID
				byte AVCPacketType = data.Get();
				//Sequence Header / here comes a AVCDecoderConfigurationRecord
				if (log.IsDebugEnabled)
					log.Debug(string.Format("AVCPacketType: {0}", AVCPacketType));
				if (AVCPacketType == 0) {
					data.Rewind();
					// Store AVCDecoderConfigurationRecord data
					_dataCountAVCDCR = data.Limit;
					if (_blockSizeAVCDCR < _dataCountAVCDCR) {
						_blockSizeAVCDCR = _dataCountAVCDCR;
						_blockDataAVCDCR = new byte[_blockSizeAVCDCR];
					}
					data.Read(_blockDataAVCDCR, 0, _dataCountAVCDCR);
				}
			}
			data.Rewind();
			// Store last keyframe
			_dataCountLKF = data.Limit;
			if (_blockSizeLKF < _dataCountLKF) {
				_blockSizeLKF = _dataCountLKF;
				_blockDataLKF = new byte[_blockSizeLKF];
			}
			data.Read(_blockDataLKF, 0, _dataCountLKF);
			data.Rewind();
			return true;
		}

		public ByteBuffer GetKeyframe() {
			if (_dataCountLKF == 0)
				return null;

			ByteBuffer result = ByteBuffer.Allocate(_dataCountLKF);
			result.Write(_blockDataLKF, 0, _dataCountLKF);
			result.Rewind();
			return result;
		}

		public ByteBuffer GetDecoderConfiguration() {
			if (_dataCountAVCDCR == 0)
				return null;

			ByteBuffer result = ByteBuffer.Allocate(_dataCountAVCDCR);
			result.Write(_blockDataAVCDCR, 0, _dataCountAVCDCR);
			result.Rewind();
			return result;
		}

		#endregion
	}
}
