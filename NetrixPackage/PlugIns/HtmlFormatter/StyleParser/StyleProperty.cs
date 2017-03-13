using System;

namespace GuruComponents.Netrix.UserInterface.StyleParser
{
	/// <summary>
	/// Contains a simple text property like "solid".
	/// </summary>
    [Serializable()]
    public class StyleProperty
	{
		private string _value;

        /// <summary>
        /// The constructor sets the base value.
        /// </summary>
        /// <param name="value"></param>
		public StyleProperty(string value)
		{
			_value = value;
		}
		/// <summary>
		/// Gets or sets the value of the property.
		/// </summary>
		public string Value
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
        /// String form of value;
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return _value;
        }
	}
}
