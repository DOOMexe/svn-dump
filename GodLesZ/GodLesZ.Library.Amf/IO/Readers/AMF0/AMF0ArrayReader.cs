
namespace GodLesZ.Library.Amf.IO.Readers {
	/// <summary>
	/// This type supports the infrastructure and is not intended to be used directly from your code.
	/// </summary>
	class AMF0ArrayReader : IAMFReader {
		public AMF0ArrayReader() {
		}

		#region IAMFReader Members

		public object ReadData(AMFReader reader) {
			return reader.ReadArray();
		}

		#endregion
	}
}
