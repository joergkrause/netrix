using System;

namespace GuruComponents.Netrix.UserInterface.StyleParser
{
	/// <summary>
	/// Contains a pair of a value and a unit like "px" or "pt".
	/// </summary>
	/// <remarks>
	/// This class supports the StyleParser.
	/// </remarks>
    [Serializable()]
    public class StyleUnit
	{
		private float _value;
		private string _unit;

        /// <summary>
        /// Constructor, sets the default values.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="unit">The unit for the value (px or pt or similiar).</param>
		public StyleUnit(float value, string unit)
		{
			_value = value;
			_unit = unit;
		}
        /// <summary>
        /// The value. The application should assure to avoid combination that doesn't make sense, like 1.5 with unit "px".
        /// </summary>
		public float Value
		{
			get
			{
				return _value;
			}
			set
			{
				_value = value;
			}
		}
        /// <summary>
        /// The unit.
        /// </summary>
		public string Unit
		{
			get
			{
				return _unit;
			}
			set
			{
				_unit = value;
			}
		}

        /// <summary>
        /// String value of unit.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return String.Format("{0}{1}", Value, Unit);
        }

	}
}
