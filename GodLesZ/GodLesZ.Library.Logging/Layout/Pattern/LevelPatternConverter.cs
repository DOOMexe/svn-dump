using System;
using System.Text;
using System.IO;

using GodLesZ.Library.Logging.Core;

namespace GodLesZ.Library.Logging.Layout.Pattern {
	/// <summary>
	/// Write the event level to the output
	/// </summary>
	/// <remarks>
	/// <para>
	/// Writes the display name of the event <see cref="LoggingEvent.Level"/>
	/// to the writer.
	/// </para>
	/// </remarks>
	/// <author>Nicko Cadell</author>
	internal sealed class LevelPatternConverter : PatternLayoutConverter {
		/// <summary>
		/// Write the event level to the output
		/// </summary>
		/// <param name="writer"><see cref="TextWriter" /> that will receive the formatted result.</param>
		/// <param name="loggingEvent">the event being logged</param>
		/// <remarks>
		/// <para>
		/// Writes the <see cref="Level.DisplayName"/> of the <paramref name="loggingEvent"/> <see cref="LoggingEvent.Level"/>
		/// to the <paramref name="writer"/>.
		/// </para>
		/// </remarks>
		override protected void Convert(TextWriter writer, LoggingEvent loggingEvent) {
			writer.Write(loggingEvent.Level.DisplayName);
		}
	}
}
