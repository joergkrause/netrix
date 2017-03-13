using System;

namespace GuruComponents.Netrix.UserInterface.StyleEditor
{
	/// <summary>
	/// Used for Value/Pair Lists.
	/// </summary>
	/// <remarks>
	/// This structure stores attribute/parameter pairs 
	/// </remarks>
	public struct UnitPair
	{
		private string _val;
		private string _mem;
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="Value">Set the value.</param>
        /// <param name="Member">Set the name the value belongs to.</param>
		public UnitPair(string Value, string Member)
		{
			_val = Value;
			_mem = Member;
		}
        /// <summary>
        /// Gets or sets the value.
        /// </summary>
		public string Value
		{
			get
			{
				return _val;
			}
			set
			{
				_val = value;
			}
		}
        /// <summary>
        /// Gets or sets the member the value belongs to.
        /// </summary>
		public string Member
		{
			get
			{
				return _mem;
			}
			set
			{
				_mem = value;
			}
		}
	}

}
