using System;
using System.ComponentModel;
using System.Drawing;
using GuruComponents.Netrix;
using GuruComponents.Netrix.ComInterop;

namespace GuruComponents.Netrix.WebEditing.HighLighting
{
	/// <summary>
	/// Defines a style for customized HighLighting.
	/// </summary>
	/// <remarks>
	/// This used as a parameter for the HighLight services, 
	/// which add specific styles to elements or text selections without changing the document or DOM
	/// during design time. The style type depends on the constructor used to instantiate the object.
	/// </remarks>
	[Serializable()]
	public class HighLightStyle : IHighLightStyle
	{
		private IHighlightColor textColor;
		private IHighlightColor textBackgroundColor;
        private IHighlightColor lineColor;
		private UnderlineStyle underlineStyle = UnderlineStyle.Wave;
		private LineThroughStyle lineThroughStyle = LineThroughStyle.Single;
		private LineType lineType = LineType.Underline;

		private bool IsDefault;
		private uint priority = 0;

		/// <summary>
		/// Instantiates a new render style object with no values and lowest priority.
		/// </summary>
		public HighLightStyle()
		{
			IsDefault = false;
			priority = 0;
			textColor = new HighlightColor(ColorType.Transparent);
			textBackgroundColor = new HighlightColor(ColorType.Transparent);
			lineColor = new HighlightColor(Color.Red, ColorType.Color);
		}

		[Obsolete("This constructor is depreciated. Please use HighLightColor to define color parameters.")] 
		public HighLightStyle(Color lineColor, UnderlineStyle underlineStyle) : this(HighlightColor.Color(lineColor), underlineStyle)
		{			
		}

		[Obsolete("This constructor is depreciated. Please use HighLightColor to define color parameters.")] 
		public HighLightStyle(Color lineColor, LineThroughStyle lineThroughStyle) : this(HighlightColor.Color(lineColor), lineThroughStyle)
		{			
		}

		[Obsolete("This constructor is depreciated. Please use HighLightColor to define color parameters.")] 
		public HighLightStyle(Color textColor, Color textBackgroundColor) : this(HighlightColor.Color(textColor), HighlightColor.Color(textBackgroundColor))
		{			
		}

		/// <summary>
		/// Instantiates a new render style object with a default underline style and lowest priority.
		/// </summary>
		/// <param name="lineColor">This parameter defines the line color used for this style.</param>
		/// <param name="underlineStyle">This parameter defines the underline style of this style.</param>
		public HighLightStyle(HighlightColor lineColor, UnderlineStyle underlineStyle) : this()
		{
			this.lineType = LineType.Underline;
			this.lineColor = lineColor;
			this.underlineStyle = underlineStyle;
			this.textColor = new HighlightColor(ColorType.Transparent);
			this.textBackgroundColor = new HighlightColor(ColorType.Transparent);
		}

		/// <summary>
		/// Instantiates a new render style object with a default line-through style and lowest priority.
		/// </summary>
		/// <param name="lineColor">This parameter defines the line color for this style.</param>
		/// <param name="lineThroughStyle">This parameter defines the the line through style.</param>
		public HighLightStyle(HighlightColor lineColor, LineThroughStyle lineThroughStyle) : this()
		{
			this.lineType = LineType.LineThrough;
			this.lineColor = lineColor;
			this.lineThroughStyle = lineThroughStyle;
			this.textColor = new HighlightColor(Color.Empty, ColorType.Inherit);
			this.textBackgroundColor = new HighlightColor(ColorType.Transparent);
		}

		/// <summary>
		/// Instantiates a new render style object with a default text coloring.
		/// </summary>
		/// <param name="textColor">This parameter defines the fore color used for this style.</param>
		/// <param name="textBackgroundColor">This parameter defines the back color used for this style.</param>
		public HighLightStyle(HighlightColor textColor, HighlightColor textBackgroundColor) : this()
		{
			this.lineType = LineType.None;
			this.textColor = textColor;
			this.textBackgroundColor = textBackgroundColor;
		}

		public Interop.IHTMLRenderStyle GetRenderStyle(Interop.IHTMLDocument2 ActiveDocument)
		{
			Interop.IHTMLRenderStyle renderStyle = ((Interop.IHTMLDocument4) ActiveDocument).createRenderStyle(null);
			renderStyle.defaultTextSelection = IsDefault ? "true" : "false";
			renderStyle.renderingPriority = (int) this.priority;
		    switch (lineType)
			{
				default:
				case LineType.None:
					renderStyle.textDecoration = "none";
					renderStyle.textBackgroundColor = textBackgroundColor.ColorName;
					if (textColor.Type == ColorType.Auto || textColor.Type == ColorType.Inherit)
					{
						renderStyle.textColor = ((Interop.IHTMLCurrentStyle)((Interop.IHTMLElement2)ActiveDocument.GetBody()).GetCurrentStyle()).getAttribute("color", 1);
					} 
					else 
					{
						renderStyle.textColor = textColor.ColorName;					
					}					
                    break;
				case LineType.Overline:
					renderStyle.textDecoration = "overline";
					renderStyle.textDecorationColor = lineColor.ColorName;
					renderStyle.textBackgroundColor = textBackgroundColor.ColorName;
					if (textColor.Type == ColorType.Auto || textColor.Type == ColorType.Inherit)
					{
						renderStyle.textColor = ((Interop.IHTMLCurrentStyle)((Interop.IHTMLElement2)ActiveDocument.GetBody()).GetCurrentStyle()).getAttribute("color", 1);
					} 
					else 
					{
						renderStyle.textColor = textColor.ColorName;					
					}					
                    break;
				case LineType.Underline:
					renderStyle.textDecoration = "underline";
					renderStyle.textDecorationColor = lineColor.ColorName;
					renderStyle.textBackgroundColor = textBackgroundColor.ColorName;
					if (textColor.Type == ColorType.Auto || textColor.Type == ColorType.Inherit)
					{
						renderStyle.textColor = ((Interop.IHTMLCurrentStyle)((Interop.IHTMLElement2)ActiveDocument.GetBody()).GetCurrentStyle()).getAttribute("color", 1);
					} 
					else 
					{
						renderStyle.textColor = textColor.ColorName;					
					}
					break;
				case LineType.LineThrough:
					renderStyle.textDecoration = "line-through";
					renderStyle.textDecorationColor = lineColor.ColorName;
					renderStyle.textBackgroundColor = textBackgroundColor.ColorName;
					if (textColor.Type == ColorType.Auto || textColor.Type == ColorType.Inherit)
					{
						renderStyle.textColor = ((Interop.IHTMLCurrentStyle)((Interop.IHTMLElement2)ActiveDocument.GetBody()).GetCurrentStyle()).getAttribute("color", 1);
					} 
					else 
					{
						renderStyle.textColor = textColor.ColorName;					
					}
					break;
			}
			string u;
			switch (underlineStyle)
			{
				case UnderlineStyle.Single:
					u = "single";
					break;
				case UnderlineStyle.Double:
					u = "double";
					break;
				case UnderlineStyle.Words:
					u = "words";
					break; 
				case UnderlineStyle.Dotted:
					u = "dotted";
					break; 
				case UnderlineStyle.Thick:
					u = "thick";
					break; 
				case UnderlineStyle.Dash:
					u = "dash";
					break; 
				case UnderlineStyle.DotDash:
					u = "dot-dash";
					break; 
				case UnderlineStyle.DotDotDash:
					u = "dot-dot-dash";
					break; 
				case UnderlineStyle.Wave:
					u = "wave";
					break; 
				case UnderlineStyle.SingleAccounting:
					u = "single-accounting";
					break; 
				case UnderlineStyle.DoubleAccounting:
					u = "double-accounting";
					break; 
				case UnderlineStyle.ThickDash:
					u = "thick-dash";
					break;   
				default:
				case UnderlineStyle.Undefined:
					u = "undefined";
					break;
			}
			renderStyle.textUnderlineStyle = u;            
			string l;
			switch (lineThroughStyle)
			{
				default:
				case LineThroughStyle.Undefined:
					l = "undefined";
					break;				
				case LineThroughStyle.Single:
					l = "single";
					break;				
				case LineThroughStyle.Double:
					l = "double";
					break;
			}
			renderStyle.textLineThroughStyle = l;
			return renderStyle;
		}


		/// <summary>
		/// Defines the property of the style.
		/// </summary>
		/// <remarks>
		/// Properties with higher priority override properties with lower priority. The default value is 0, 
		/// and the higher the number, the higher the priority. The rendering priority has a limit of 9.
		/// <para>
		/// If two properties of the IHTMLRenderStyle interface are applied to the same text selection, 
		/// the property with higher rendering priority overrides the property with lower rendering priority. 
		/// If you allow two properties of the IHTMLRenderStyle interface to have the same rendering priority, 
		/// the control will not be able to determine which property will override the other.
		/// </para>
		/// </remarks>
        [Browsable(true), Category("Behavior"), DefaultValue(typeof(uint), "0")]
		public uint Priority
		{
			get
			{
				return priority;
			}
			set
			{
				priority = (uint)Math.Min(8, Math.Abs(value));
			}
		}
		/// <summary>
		/// Sets or retrieves the color of the text in the object.
		/// </summary>
		/// <remarks>
		/// The color allows the types "color", "auto" and "inherit" only.
		/// </remarks>
		[Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [TypeConverter(typeof(ExpandableObjectConverter)), Category("Color")]
        public IHighlightColor TextColor
		{
			get
			{
				return textColor;
			}
			set
			{
				textColor = value;
			}
		}
		/// <summary>
		/// Sets or retrieves the color behind the text in the object.
		/// </summary>
		/// <remarks>
		/// The color allows the types "color", "transparent", and "inherit" only.
		/// </remarks>
		[Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [TypeConverter(typeof(ExpandableObjectConverter)), Category("Color")]
        public IHighlightColor TextBackgroundColor
		{
			get
			{
				return textBackgroundColor;
			}
			set
			{
				textBackgroundColor = value;
			}
		}
		/// <summary>
		/// Sets or retrieves the appearance of the underline decoration of the object.
		/// </summary>
		/// <remarks>
		/// This property supports the thick-dash value as of Internet Explorer 6.
		/// <seealso cref="GuruComponents.Netrix.WebEditing.HighLighting.UnderlineStyle">UnderlineStyle</seealso>
		/// </remarks>
        [Browsable(true), Category("Style"), DefaultValue(typeof(UnderlineStyle), "Wave")]
		public GuruComponents.Netrix.WebEditing.HighLighting.UnderlineStyle UnderlineStyle
		{
			get
			{
				return underlineStyle;
			}
			set
			{
				underlineStyle = value;
			}
		}
		/// <summary>
		/// Sets or retrieves the appearance of the line-through decoration of the object.
		/// </summary>
		/// <remarks>
		/// <seealso cref="GuruComponents.Netrix.WebEditing.HighLighting.LineThroughStyle">LineThroughStyle</seealso>
		/// </remarks>
        [Browsable(true), Category("Style"), DefaultValue(typeof(LineThroughStyle), "Single")]
		public GuruComponents.Netrix.WebEditing.HighLighting.LineThroughStyle LineThroughStyle
		{
			get
			{
				return lineThroughStyle;
			}
			set
			{
				lineThroughStyle = value;
			}
		}
		/// <summary>
		/// Sets or retrieves the color of the line-through, overline, or underline decorations of the object.
		/// </summary>
		/// <remarks>
		/// The color allows the types "color" and "auto" only.
		/// </remarks>
		[Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [TypeConverter(typeof(ExpandableObjectConverter)), Category("Color")]
        public IHighlightColor LineColor
		{
			get
			{
				return lineColor;
			}
			set
			{
				lineColor = value;
			}
		}
		/// <summary>
		/// Sets or retrieves whether the text in the object renders with a line-through, overline, or underline decoration.
		/// </summary>
		/// <remarks>
		/// <seealso cref="GuruComponents.Netrix.WebEditing.HighLighting.LineType">LineType</seealso>
		/// </remarks>
		[Browsable(true), Category("Style"), DefaultValue(typeof(LineType), "Underline")]
		public GuruComponents.Netrix.WebEditing.HighLighting.LineType LineType
		{
			get
			{
				return lineType;
			}
			set
			{
				lineType = value;
			}
		}

        /// <summary>
        /// Overridden for design time support.
        /// </summary>
        /// <returns></returns>
		public override string ToString()
		{
			return "Click + to see options";
		}

	}
}