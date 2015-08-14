

namespace GodLesZ.Library.Amf.IO.Readers {
	/// <summary>
	/// This type supports the infrastructure and is not intended to be used directly from your code.
	/// </summary>
	class AMF3ByteArrayReader : IAMFReader {
		public AMF3ByteArrayReader() {
		}

		#region IAMFReader Members

		public object ReadData(AMFReader reader) {
			return reader.ReadAMF3ByteArray();
		}

		#endregion
	}
}
