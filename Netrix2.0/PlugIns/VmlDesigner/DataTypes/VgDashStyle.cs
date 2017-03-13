using System;  
using System.ComponentModel;  

using GuruComponents.Netrix.ComInterop;
using Comzept.Genesis.NetRix.VgxDraw;
using GuruComponents.Netrix.VmlDesigner.Elements;

namespace GuruComponents.Netrix.VmlDesigner.DataTypes
{
	/// <summary>
	/// The DashStyle attribute allows the user to specify a custom-defined as well as use predefined dash patterns. 
	/// </summary>
	/// <remarks>
	/// Custom Patterns:
	/// This is done using a series of numbers. Dash styles are defined in terms of the length of the dash 
	/// (the drawn part of the stroke) and the length of the space between the dashes. The lengths are relative to the 
	/// line width; a length of "1" is equal to the line width. The EndCap style is applied to each dash, arrow styles are not. 
	/// The string first defines the length of the dash then the length of the space. This may be repeated to form complex dash 
	/// styles. The string should always contain a pair of numbers; if it contains an odd number of numbers the last may be 
	/// disregarded. The following table lists some typical values and a description of the intended effect. 
	/// "0" implies a dot that should be fourfold symmetrical (with round endcaps it should be a circle). 
	/// If the line endcap is Flat, a viewer should choose a built-in operating system dash where possible (i.e., something 
	/// that is fast to draw). The following shows some examples.
	/// <list type="bullet">
	/// <item>“2 2” 
	/// short-dashes (each dash and the space in between is twice the width of the line) 
	/// </item>
	/// <item>
	/// “1 2” 
	/// dot (each dash is the width of the line while each space is twice the width of the line) 
	/// </item>
	/// <item>
	/// “4 2” 
	/// dash (each dash is four times the width of the line while each space is twice the width of the line) 
	/// </item>
	/// <item>
	/// “8 2” 
	/// long-dash 
	/// </item>
	/// <item>
	/// “4 2 1 2” 
	/// dash dot 
	/// </item>
	/// <item>
	/// “8 2 1 2” 
	/// long-dash dot 
	/// </item>
	/// <item>
	/// “8 2 1 2 1 2” 
	/// long-dash dot dot
	/// </item>
	/// </list>
	/// </remarks>
	public sealed class VgDashStyle
	{

		private IVgLineDashStyle native;
             
		internal VgDashStyle(IVgLineDashStyle native)
		{
			this.native = native;
		}

		#region IVgLineDashStyle Members

        /// <summary>
        /// Creator pointer
        /// </summary>
		[Browsable(false)]
		public int Creator
		{
			get
			{
				return native.Creator;
			}
		}

        /// <summary>
        /// Parent
        /// </summary>
		[Browsable(false)]
		public object ParentShape
		{
			get
			{
				return native.parentShape;
			}
		}

        /// <summary>
        /// Custom dash style
        /// </summary>
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public VgDashStyleArray Array
		{
			get
			{
				try
				{
					IVgDashStyleArray nativeArray = native.array as IVgDashStyleArray;
					return new VgDashStyleArray(native.array);
				}
				catch
				{
					return null;
				}
			}
		}

        /// <summary>
        /// Application
        /// </summary>
		[Browsable(false)]
		public object Application
		{
			get
			{
				return native.Application;
			}
		}

        /// <summary>
        /// Base style
        /// </summary>
		[RefreshProperties(RefreshProperties.Repaint)]
		public LineDashStyle PresetStyle
		{
			get
			{
				return (LineDashStyle) (int) native.presetStyle;
			}
			set
			{
				native.presetStyle = (Comzept.Genesis.NetRix.VgxDraw.VgLineDashStyle) (int) value;
			}
		}

		[RefreshProperties(RefreshProperties.Repaint)]
		public string Value
		{
			get
			{
				return native.value;
			}
			set
			{
				native.value = value;
			}
		}

		#endregion

        /// <summary>
        /// Design time support
        /// </summary>
        /// <returns></returns>
		public override string ToString()
		{
			return "Dashstyle Properties";
		}


	}
}
