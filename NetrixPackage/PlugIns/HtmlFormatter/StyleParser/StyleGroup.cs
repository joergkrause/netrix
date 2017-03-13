using System;

namespace GuruComponents.Netrix.UserInterface.StyleParser
{
	/// <summary>
	/// A StyleGroup is a combination of a simple style property, a style unit and a style color. 
	/// </summary>
    [Serializable()]
    public class StyleGroup
	{
		StyleUnit _unit;
		StyleColor _color;
		StyleProperty _property;
		StyleList _list;

        /// <summary>
        /// The unit value.
        /// </summary>
		public StyleUnit Unit
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
        /// The color value.
        /// </summary>
		public StyleColor Color
		{
			get
			{
				return _color;
			}
			set
			{
				_color = value;
			}
		}
        /// <summary>
        /// The property value.
        /// </summary>
		public StyleProperty Property
		{
			get
			{
				return _property;
			}
			set
			{
				_property = value;
			}
		}
        /// <summary>
        /// The list value.
        /// </summary>
		public StyleList List
		{
			get
			{
				return _list;
			}
			set
			{
				_list = value;
			}
		}

        /// <summary>
        /// Override for design time support.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return String.Format("{0} {1} {2}", _property, _color, _unit);
        }

	}
}
