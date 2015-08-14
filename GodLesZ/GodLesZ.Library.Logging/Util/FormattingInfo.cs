using System;
using GodLesZ.Library.Logging.Util;

namespace GodLesZ.Library.Logging.Util {
	/// <summary>
	/// Contain the information obtained when parsing formatting modifiers 
	/// in conversion modifiers.
	/// </summary>
	/// <remarks>
	/// <para>
	/// Holds the formatting information extracted from the format string by
	/// the <see cref="PatternParser"/>. This is used by the <see cref="PatternConverter"/>
	/// objects when rendering the output.
	/// </para>
	/// </remarks>
	/// <author>Nicko Cadell</author>
	/// <author>Gert Driesen</author>
	public class FormattingInfo {
		#region Public Instance Constructors

		/// <summary>
		/// Defaut Constructor
		/// </summary>
		/// <remarks>
		/// <para>
		/// Initializes a new instance of the <see cref="FormattingInfo" /> class.
		/// </para>
		/// </remarks>
		public FormattingInfo() {
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <remarks>
		/// <para>
		/// Initializes a new instance of the <see cref="FormattingInfo" /> class
		/// with the specified parameters.
		/// </para>
		/// </remarks>
		public FormattingInfo(int min, int max, bool leftAlign) {
			m_min = min;
			m_max = max;
			m_leftAlign = leftAlign;
		}

		#endregion Public Instance Constructors

		#region Public Instance Properties

		/// <summary>
		/// Gets or sets the minimum value.
		/// </summary>
		/// <value>
		/// The minimum value.
		/// </value>
		/// <remarks>
		/// <para>
		/// Gets or sets the minimum value.
		/// </para>
		/// </remarks>
		public int Min {
			get { return m_min; }
			set { m_min = value; }
		}

		/// <summary>
		/// Gets or sets the maximum value.
		/// </summary>
		/// <value>
		/// The maximum value.
		/// </value>
		/// <remarks>
		/// <para>
		/// Gets or sets the maximum value.
		/// </para>
		/// </remarks>
		public int Max {
			get { return m_max; }
			set { m_max = value; }
		}

		/// <summary>
		/// Gets or sets a flag indicating whether left align is enabled
		/// or not.
		/// </summary>
		/// <value>
		/// A flag indicating whether left align is enabled or not.
		/// </value>
		/// <remarks>
		/// <para>
		/// Gets or sets a flag indicating whether left align is enabled or not.
		/// </para>
		/// </remarks>
		public bool LeftAlign {
			get { return m_leftAlign; }
			set { m_leftAlign = value; }
		}

		#endregion Public Instance Properties

		#region Private Instance Fields

		private int m_min = -1;
		private int m_max = int.MaxValue;
		private bool m_leftAlign = false;

		#endregion Private Instance Fields
	}
}
