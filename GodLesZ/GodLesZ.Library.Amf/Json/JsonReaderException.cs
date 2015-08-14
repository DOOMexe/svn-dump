using System;

namespace GodLesZ.Library.Amf.Json {
	/// <summary>
	/// The exception thrown when an error occurs while reading Json text.
	/// </summary>
	public class JsonReaderException : Exception {
		/// <summary>
		/// Initializes a new instance of the <see cref="JsonReaderException"/> class.
		/// </summary>
		public JsonReaderException() {
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="JsonReaderException"/> class
		/// with a specified error message.
		/// </summary>
		/// <param name="message">The error message that explains the reason for the exception.</param>
		public JsonReaderException(string message)
			: base(message) {
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="JsonReaderException"/> class
		/// with a specified error message and a reference to the inner exception that is the cause of this exception.
		/// </summary>
		/// <param name="message">The error message that explains the reason for the exception.</param>
		/// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
		public JsonReaderException(string message, Exception innerException)
			: base(message, innerException) {
		}
	}
}
