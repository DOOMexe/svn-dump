// .NET Compact Framework 1.0 has no support for ASP.NET
// SSCLI 1.0 has no support for ASP.NET
#if !NETCF && !SSCLI && !CLIENT_PROFILE

using System.IO;
using System.Web;
using GodLesZ.Library.Logging.Core;
using GodLesZ.Library.Logging.Util;

namespace GodLesZ.Library.Logging.Layout.Pattern {
	/// <summary>
	/// Abstract class that provides access to the current HttpContext (<see cref="HttpContext.Current" />) that 
	/// derived classes need.
	/// </summary>
	/// <remarks>
	/// This class handles the case when HttpContext.Current is null by writing
	/// <see cref="SystemInfo.NotAvailableText" /> to the writer.
	/// </remarks>
	/// <author>Ron Grabowski</author>
	internal abstract class AspNetPatternLayoutConverter : PatternLayoutConverter {
		protected override void Convert(TextWriter writer, LoggingEvent loggingEvent) {
			if (HttpContext.Current == null) {
				writer.Write(SystemInfo.NotAvailableText);
			} else {
				Convert(writer, loggingEvent, HttpContext.Current);
			}
		}

		/// <summary>
		/// Derived pattern converters must override this method in order to
		/// convert conversion specifiers in the correct way.
		/// </summary>
		/// <param name="writer"><see cref="TextWriter" /> that will receive the formatted result.</param>
		/// <param name="loggingEvent">The <see cref="LoggingEvent" /> on which the pattern converter should be executed.</param>
		/// <param name="httpContext">The <see cref="HttpContext" /> under which the ASP.Net request is running.</param>
		protected abstract void Convert(TextWriter writer, LoggingEvent loggingEvent, HttpContext httpContext);
	}
}

#endif