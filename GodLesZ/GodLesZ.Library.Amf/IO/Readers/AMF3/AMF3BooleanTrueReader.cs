
namespace GodLesZ.Library.Amf.IO.Readers {
	/// <summary>
	/// This type supports the infrastructure and is not intended to be used directly from your code.
	/// </summary>
	class AMF3BooleanTrueReader : IAMFReader {
		public AMF3BooleanTrueReader() {
		}

		#region IAMFReader Members

		public object ReadData(AMFReader reader) {
			return true;
		}

		#endregion
	}
}
