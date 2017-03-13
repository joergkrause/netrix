using System;
using System.Drawing;
using System.ComponentModel;

namespace GuruComponents.Netrix.WebEditing.HighLighting
{
	/// <summary>
	/// This class supports the color information settings for HighLightStyle.
	/// </summary>
	/// <remarks>
	/// Holds the color of the text, background, or line color in the object. The value can be set to 
	/// any valid HTML/CSS color value within the 216 colors base table or to the values "auto" or "inherit".
	/// <seealso cref="ColorType"/>
	/// </remarks>
	[TypeConverter(typeof(MyExpandableObjectConverter)), Serializable()]
	public class HighlightColor : IHighlightColor
	{

        /// <summary>
        /// Creates an object with the specified type.
        /// </summary>
        /// <param name="type">The type of color.</param>
		public HighlightColor(ColorType type)
		{
			ColorValue = System.Drawing.Color.Empty;
			Type = type;
		}

		/// <summary>
        /// Creates an object with the specified color and type.
        /// </summary>
        /// <param name="color">The color of the object.</param>
        /// <param name="type">The type of color.</param>
		public HighlightColor(System.Drawing.Color color, ColorType type)
		{
			Type = type;
			ColorValue = color;
		}

		/// <summary>
		/// Creates a new <see cref="HighlightColor"/> object with the specified color.
		/// </summary>
        /// <remarks>
        /// The object created is always of type <see cref="ColorType.Color"/>.
        /// </remarks>
		/// <param name="color">The color of the new object.</param>
		/// <returns>Instance of <see cref="HighlightColor"/>, preset to given color.</returns>
        public static HighlightColor Color(System.Drawing.Color color)
		{
			return new HighlightColor(color, ColorType.Color);
		}

		/// <summary>
		/// Returns an object which is set to "auto" value.
		/// </summary>
		/// <returns>Instance of <see cref="HighlightColor"/>, preset to "auto".</returns>
		public static HighlightColor Auto()
		{
			return new HighlightColor(ColorType.Auto);
		}

		/// <summary>
		/// Returns an object which is set to "inherit" value.
		/// </summary>
		/// <returns>Instance of <see cref="HighlightColor"/>, preset to "inherit".</returns>
		public static HighlightColor Inherit()
		{
			return new HighlightColor(ColorType.Inherit);
		}

		/// <summary>
		/// Returns an object which is set to "transparent" value.
		/// </summary>
		/// <returns>Instance of <see cref="HighlightColor"/>, preset to "transparent".</returns>
		public static HighlightColor Transparent()
		{
			return new HighlightColor(ColorType.Transparent);
		}


        private ColorType type = ColorType.Auto;
		/// <summary>
		/// Current type of Color in HighLightColor object.
		/// </summary>
		[Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), DefaultValue(typeof(ColorType), "Auto")]
        [Description("Current type of Color in HighLightColor object.")]
		public ColorType Type 
        {
            get
            {
                return type; 
            }
            set
            {
                type = value;
				if (type != ColorType.Color) 
				{
					ColorValue = System.Drawing.Color.Empty;
				}
            }		        
        }

		private System.Drawing.Color colorValue;
        /// <summary>
		/// Sets or retrieves the color of the object.
		/// </summary>
		[Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Description("Sets or retrieves the color of the object.")]
		[DefaultValueAttribute(typeof(Color), "Empty")]

		[TypeConverterAttribute(typeof(GuruComponents.Netrix.UserInterface.TypeConverters.UITypeConverterColor))]
		[EditorAttribute(
			 typeof(GuruComponents.Netrix.UserInterface.TypeEditors.UITypeEditorColor),
			 typeof(System.Drawing.Design.UITypeEditor))]
		[GuruComponents.Netrix.UserInterface.TypeEditors.DisplayName()]
		public System.Drawing.Color ColorValue
		{
		    get
		    {
		        return colorValue;        
		    }
            set
            {
                colorValue = value;
				if (colorValue != System.Drawing.Color.Empty)
				{
					Type = ColorType.Color;
				}
            }
		}

        /// <summary>
		/// Returns the color name in HTML format, or the word "auto" or "inherit", respectively.
		/// </summary>
		[Browsable(false)]
        public string ColorName
		{
			get
			{
				switch (Type)
				{
					case ColorType.Transparent:
						return "Transparent";
					case ColorType.Auto:
						return "Auto";
					case ColorType.Inherit:
						return "Inherit";
					case ColorType.Color:
						if (ColorValue != System.Drawing.Color.Empty)
						{
							return System.Drawing.ColorTranslator.ToHtml(ColorValue);
						} 
						else 
						{
							return "Empty";
						}
				}
				return "Auto";
			}
		}

        public override string ToString()
        {
            return ColorName;
        }


	}
}
