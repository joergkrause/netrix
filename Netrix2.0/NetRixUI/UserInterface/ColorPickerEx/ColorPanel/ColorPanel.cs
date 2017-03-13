using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Diagnostics;
using System.Collections.Generic;

namespace GuruComponents.Netrix.UserInterface.ColorPicker
{

	/// <summary>
	/// A control that allows the selection of a color from a fixed color palette.
	/// </summary>
	/// <remarks>
	/// The color panel displays a grid of colors.  These colors are either derived from the
	/// System.Drawing.KnownColor enumeration, or supplied by the user using the <c>CustomColor</c> property.
	/// </remarks>
	[ToolboxItem(true)]
    [ToolboxBitmap(typeof(GuruComponents.Netrix.UserInterface.ResourceManager), "ToolBox.ColorEditor.ico")]
    [DesignerAttribute(typeof(GuruComponents.Netrix.UserInterface.NetrixUIDesigner))]
    [DefaultEvent("ColorChanged")]
    public class ColorPanel : System.Windows.Forms.UserControl
	{
		// defaults
		internal const ColorSortOrder    defaultColorSortOrder   = GuruComponents.Netrix.UserInterface.ColorPicker.ColorSortOrder.Unsorted;
		internal const ColorSet          defaultColorSet         = GuruComponents.Netrix.UserInterface.ColorPicker.ColorSet.Web;
		internal const BorderStyle       defaultBorderStyle      = BorderStyle.FixedSingle;
		internal const int               defaultPreferredColumns = 18;
		internal static readonly Size    defaultColorWellSize    = new Size(10,10);
		internal static readonly Color   defaultColor            = Color.Empty;

		private System.Windows.Forms.ToolTip     toolTip;
		private System.ComponentModel.IContainer components;

		private System.Windows.Forms.BorderStyle borderStyle      = defaultBorderStyle;
		private Size                             borderSize       = new Size(0, 0);
		private Size                             colorWellSize    = defaultColorWellSize;
		private ColorWellInfo[]                  colorWells       = null;
		private ColorWellInfo                    pickColor        = null;
		private ColorWellInfo                    currentColor     = null;
		private ColorSet                         colorSet         = defaultColorSet;
		private ColorSortOrder                   colorSortOrder   = defaultColorSortOrder;
		private int                              preferredColumns = defaultPreferredColumns;
        private List<Color>                      customColors     = null;

		private int   columns;
		private int   rows;
		private Point lastMousePosition;
        /// <summary>
		/// Initializes a new instance of the ColorPanel class.
		/// </summary>
		/// <remarks>
		/// The default constructor initializes all fields to their default values.
		/// </remarks>
		public ColorPanel()
        {
            colorWells = ColorWellHelper.GetColorWells(colorSet, colorSortOrder);
			ResetCustomColors();
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
			UpdateBorderSize();
			AutoSizePanel();
		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.toolTip = new System.Windows.Forms.ToolTip(this.components);
			// 
			// toolTip
			// 
			this.toolTip.AutomaticDelay = 0;
			// 
			// ColorPanel
			// 
			this.BackColor = System.Drawing.SystemColors.ControlDark;
			this.Name = "ColorPanel";
			this.Size = new System.Drawing.Size(152, 184);
			this.toolTip.SetToolTip(this, "color");

		}
		#endregion

        /// <summary>
		/// Layout the color wells in the available space.
		/// </summary>
		private void LayoutColorWells()
		{
			int x = borderSize.Width;
			int y = borderSize.Height;

			foreach (ColorWellInfo c in colorWells)
			{
				c.ColorPosition = new Rectangle(x, y, colorWellSize.Width, colorWellSize.Height);
				x += colorWellSize.Width;
				if( x + colorWellSize.Width > ClientRectangle.Width )
				{
					y += colorWellSize.Height;
					x = borderSize.Width;
				}
			}
		}

		/// <summary>
		/// The ColorChangedEvent event handler.
		/// </summary>
		[Browsable(true), Category("ColorPanel")]
		public event ColorChangedEventHandler ColorChanged;
		/// <summary>
		/// 
		/// </summary>
		private void FireColorChanged()
		{
			if (null != pickColor)
			{
				OnColorChanged(new ColorChangedEventArgs(pickColor.Color));
			}
		}

		/// <summary>
		/// Raises the ColorChanged event.
		/// </summary>
		/// <param name="e">A ColorChangedEventArgs contains the event data.</param>
		protected virtual void OnColorChanged(ColorChangedEventArgs e)
		{
			if( null != ColorChanged )
			{
				ColorChanged(this, e);
			}
		}

		/// <summary>
		/// Get the color well at the specified point.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		private ColorWellInfo ColorWellFromPoint( int x, int y )
		{
			int w = ClientRectangle.Width;
			int h = ClientRectangle.Height;

			// could be optimized
			foreach( ColorWellInfo c in colorWells )
			{
				if( c.ColorPosition.Contains(x,y) )
				{
					return c;
				}
			}

			return null;
		}

		/// <summary>
		/// Get the first color well with the specified color.
		/// There may be multiple color wells with the same color for custom color palettes.
		/// Note that Color.White != Color.Window even when Color.Window is white!
		/// </summary>
		/// <param name="col"></param>
		/// <returns></returns>
		private ColorWellInfo ColorWellFromColor(Color col)
		{
			if (col == Color.Empty)
			{
				ColorWellInfo cwi = new ColorWellInfo(Color.Empty, 0);
			} 
			else 
			{
				foreach (ColorWellInfo c in colorWells)
				{
					if (c.Color == col)
					{
						return c;
					}
				}
			}
			return null;
		}

		/// <summary>
		/// Get the sorted index of the color well (not the original index).
		/// </summary>
		/// <param name="col"></param>
		/// <returns></returns>
		private int IndexFromColorWell( ColorWellInfo col )
		{
			int num_colorWells = colorWells.Length;

			int index = -1;

			for( int i=0; i<num_colorWells; i++ )
			{
				if( colorWells[i] == col )
				{
					index = i;
				}
			}

			return index;
		}

		/// <summary>
		/// Overrides the OnClick event in order to detect user color selection.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnClick(System.EventArgs e)
		{
			base.OnClick(e);
			if (null != currentColor)
			{
				// invalidate previous pick color
				if (null != pickColor)
				{
					Invalidate(pickColor.ColorPosition);
				}
				// set new pick color
				pickColor = currentColor;
				// invalidate new pick color
				Invalidate(pickColor.ColorPosition);
				FireColorChanged();
			}
		}

		/// <summary>
		/// Change the color currently selected. Does not cause a ColorChanged event.
		/// </summary>
		/// <param name="newColor"></param>
		private void ChangeColor(ColorWellInfo newColor)
		{
			if (newColor != currentColor)
			{
				if (null != currentColor)
				{
					Invalidate(currentColor.ColorPosition);
				}
				currentColor = newColor;
				if (null != currentColor)
				{
					Invalidate(currentColor.ColorPosition);
					toolTip.SetToolTip(this, currentColor.ColorName);
				}
				else
				{
					toolTip.SetToolTip(this, "");
				}
				Update();
			}
		}

		/// <summary>
		/// Overrides the OnMouseMove event in order to track mouse movement.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseMove(System.Windows.Forms.MouseEventArgs e)
		{
			base.OnMouseMove(e);
			if( !Enabled )
				return;
			Point mousePosition = new Point(e.X,e.Y);
			// Invalidation causes an OnMouseMove event - filter it out so it doesn't
			// interfere with keyboard control
			if( ClientRectangle.Contains(mousePosition) && (lastMousePosition != mousePosition) )
			{
				lastMousePosition = mousePosition;
				ColorWellInfo newColor = ColorWellFromPoint(e.X,e.Y);
				ChangeColor(newColor);
			}
		}

		/// <summary>
		/// Overrides OnMouseLeave in order to detect when the mouse
		/// has left the control.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseLeave(System.EventArgs e)
		{
			base.OnMouseLeave(e);
			if (!Enabled) return;
			ColorWellInfo invalidColor = currentColor;
			//currentColor = null;
			if (null != invalidColor)
			{
				Invalidate( invalidColor.ColorPosition );
				Update();
			}
		}

		/// <summary>
		/// Overrides OnGotFocus so the control can be redrawn with the focus.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnGotFocus(System.EventArgs e)
		{
			base.OnGotFocus(e);

			Refresh();
		}

		/// <summary>
		/// Overrides OnLostFocus so the control can be redrawn without the focus.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnLostFocus(System.EventArgs e)
		{
			base.OnLostFocus(e);

			Refresh();
		}

		/// <summary>
		/// Overrides IsInputKey.<br></br><br></br>
		/// This allows the control to tell the base class that the keys
		/// Keys.Left, Keys.Right, Keys.Up and Keys.Down should cause the OnKeyDown event.
		/// </summary>
		/// <param name="keyData">One of the <c>System.Windows.Forms.Keys</c> values</param>
		/// <returns><B>true</B> if keyData is one of 
		/// Keys.Left, Keys.Right, Keys.Up and Keys.Down.  Otherwise <B>false</B>.</returns>
		protected override bool IsInputKey( System.Windows.Forms.Keys keyData )
		{
			bool bIsInputKey = true;

			switch( keyData )
			{
			case Keys.Left:
				break;
			case Keys.Right:
				break;
			case Keys.Down:
				break;
			case Keys.Up:
				break;
			default:
				bIsInputKey = base.IsInputKey(keyData);
				break;
			}

			return bIsInputKey;
		}

		private void MoveColumn( int index, bool bNext )
		{
			int numColors = colorWells.Length;

			int r = index/columns;
			int c = index - (r*columns);

			int nextIndex = 0;

			if( bNext )
			{
				c++;
				if( c >= columns )
				{
					c = 0;
				}

				nextIndex = r*columns + c;

				if( nextIndex >= numColors )
				{
					nextIndex = r*columns;
				}
			}
			else
			{
				c--;
				if( c < 0 )
				{
					c = columns - 1;
				}
				nextIndex = r*columns + c;
				if( nextIndex >= numColors )
				{
					nextIndex = numColors - 1;
				}
			}

			ChangeColor( colorWells[nextIndex] );
		}

		private void MoveRow( int index, bool bNext )
		{
			int numColors = colorWells.Length;

			int r = index/columns;
			int c = index - (r*columns);

			int nextIndex = 0;

			if( bNext )
			{
				r++;
				if( r >= rows )
				{
					r = 0;
				}

				nextIndex = r*columns + c;

				if( nextIndex >= numColors )
				{
					nextIndex = c;
				}
			}
			else
			{
				r--;
				if( r < 0 )
				{
					r = rows - 1;
				}
				nextIndex = r*columns + c;
				if( nextIndex >= numColors )
				{
					nextIndex = (r-1)*columns + c;
				}
			}

			ChangeColor( colorWells[nextIndex] );
		}

		/// <summary>
		/// Overrides OnKeyDown so that a color may be selected using the keyboard.<br></br><br></br>
		/// Use the keys - Left, Right, Up, Down and Enter.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnKeyDown(System.Windows.Forms.KeyEventArgs e)
		{
			base.OnKeyDown(e);

			if( !Enabled )
				return;

			int index = IndexFromColorWell( (null!=currentColor) ? (currentColor) : (pickColor) );

			switch( e.KeyCode )
			{
			case Keys.Enter:
				if( null != currentColor )
				{
					ColorWellInfo oldColor = pickColor;

					pickColor = currentColor;
					FireColorChanged();

					Invalidate( oldColor.ColorPosition );
					Invalidate( currentColor.ColorPosition );

					Update();
				}
				break;
			case Keys.Left:
				if( index < 0 )
				{
					// start at the last color
					ChangeColor(colorWells[colorWells.Length-1]);
				}
				else
				{
					MoveColumn( index, false );
				}
				break;
			case Keys.Right:
				if( index < 0 || index > (colorWells.Length-1) )
				{
					// start at the first color
					ChangeColor(colorWells[0]);
				}
				else
				{
					MoveColumn( index, true );
				}
				break;
			case Keys.Down:
				if( index < 0 )
				{
					// start at the first color
					ChangeColor(colorWells[0]);
				}
				else
				{
					MoveRow( index, true );
				}
				break;
			case Keys.Up:
				if( index < 0 )
				{
					// start at the last color
					ChangeColor(colorWells[colorWells.Length-1]);
				}
				else
				{
					MoveRow( index, false );
				}
				break;
			}
		}

		/// <summary>
		/// When the ColorPanel is being resized GetPreferredWidth is called to
		/// determine the preferred width.
		/// For ColorPanel the preferred width is the control's default Width, i.e the control may be resized.
		/// <br></br>
		/// Derived classes, such as ColorPanelWithCapture may override this.
		/// </summary>
		/// <returns></returns>
		protected virtual int GetPreferredWidth()
		{
			return Size.Width;
		}

		/// <summary>
		/// This method is called internally to set the control's size.<br></br>
		/// If the Columns property is 0 then the control fixes it's width to the 
		/// nearest number of columns that fit into the value returned by GetPreferredWidth.<br></br>
		/// If the Columns property is greater than 0 then the control will display that many columns.
		/// </summary>
		protected void AutoSizePanel()
		{
			if( preferredColumns <= 0 )
			{
				int preferredWidth = GetPreferredWidth();
				int w    = preferredWidth - borderSize.Width*2;
				int remw = w % colorWellSize.Width;
				columns  = w/colorWellSize.Width;
				rows     = colorWells.Length/columns + ((colorWells.Length%columns != 0)?1:0);
				int h    = rows*colorWellSize.Height +  borderSize.Height*2;
				if( remw != 0 || h != Size.Height )
				{
					w = preferredWidth - remw;

					this.ClientSize = new Size(w,h);
				}
				LayoutColorWells();
				Refresh();
			}
			else
			{
				int preferred = preferredColumns;
				// if there are less color wells than the number of preferredColumns
				// then use the number of color wells
				if( colorWells.Length < preferredColumns )
				{
					preferred = colorWells.Length;
				}
				columns = preferred;
				int w = preferred * colorWellSize.Width + borderSize.Width * 2;
                if (colorWells.Length > 0)
                {
                    rows = colorWells.Length / columns + ((colorWells.Length % columns != 0) ? 1 : 0);
                }
                else
                {
                    rows = 0;
                }
				int h = rows*colorWellSize.Height +  borderSize.Height*2;
				this.ClientSize = new Size(w,h);
				LayoutColorWells();
				Refresh();
			}
		}

		/// <summary>
		/// Overrides OnResize so the control can be auto-sized.
		/// </summary>
		/// <remarks>
		/// The control auto-sizes.  It first fixes the width to the nearest
		/// whole multiple of the color well width, and then fixes the height to
		/// the nearest whole multiple of the color well height.
		/// </remarks>
		/// <param name="e"></param>
		protected override void OnResize(System.EventArgs e)
		{
			base.OnResize(e);
			AutoSizePanel();
		}

		/// <summary>
		/// Overrides OnEnabledChanged so the control can redraw itself 
		/// enabled/disabled.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnEnabledChanged(System.EventArgs e)
		{
			Refresh();
		}


		/// <summary>
		/// Overrides OnPaint so the control can be drawn.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
		{
			foreach( ColorWellInfo c in colorWells )
			{
				ColorWellHelper.DrawColorWell(e.Graphics, Enabled, c == currentColor, c == pickColor, c );
			}

			// draw a border (or not)
			switch( borderStyle )
			{
			case BorderStyle.Fixed3D:
				ControlPaint.DrawBorder3D( e.Graphics, ClientRectangle, Border3DStyle.Sunken );
				break;
			case BorderStyle.FixedSingle:
				ControlPaint.DrawBorder3D( e.Graphics, ClientRectangle, Border3DStyle.Flat );
				break;
			}

			if( Focused && Enabled )
			{
				Rectangle r= ClientRectangle;
				r.Inflate( -borderSize.Width+1, -borderSize.Height+1 );
				ControlPaint.DrawFocusRectangle(e.Graphics,r);
			}

			// call base.OnPaint last so clients can paint over the control if required
			base.OnPaint(e);
		}

		/// <summary>
		/// Override OnSystemColorsChanged to that the System color palette
		/// can be updated when a user modifies the system colors.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnSystemColorsChanged(System.EventArgs e)
		{
			base.OnSystemColorsChanged(e);

			if(colorSet == ColorSet.System)
			{
				// generate new set of system colors
				colorWells = ColorWellHelper.GetColorWells(colorSet, colorSortOrder);
				LayoutColorWells();

				UpdatePickColor();

				FireColorChanged();

				Refresh();
			}
		}

		/// <summary>
		/// Set/get the controls border style.
		/// </summary>
		[Browsable(true), CategoryAttribute("Runtime Appearance")]
		[DefaultValue(defaultBorderStyle), Description("Indicates the color panel's border style.")]
		public new System.Windows.Forms.BorderStyle BorderStyle
		{
			get
			{
				return borderStyle;
			}
			set
			{
				Utils.CheckValidEnumValue( "BorderStyle", value, typeof(System.Windows.Forms.BorderStyle) );

				if( borderStyle != value )
				{
					borderStyle = value;

					UpdateBorderSize();

					AutoSizePanel();
				}
			}
		}
		/// <summary>
		/// Update the border size values based on the current border style.
		/// </summary>
		private void UpdateBorderSize()
		{
			Size bs = new Size();

			switch( borderStyle )
			{
			case BorderStyle.Fixed3D:
				bs = SystemInformation.Border3DSize;
				break;
			case BorderStyle.FixedSingle:
				bs = SystemInformation.BorderSize;
				break;
			case BorderStyle.None:
				break;
			}
			
			// increase border size by 1,1 to accomodate focus rectangle
			bs.Width++;
			bs.Height++;

			borderSize = bs;
		}

		/// <summary>
		/// Set/get the pick Color.
		/// </summary>
		[Browsable(true), Category("ColorPanel"), Description("Get/set the pick color.")]
		public System.Drawing.Color Color
		{
			get
			{
				if( pickColor != null )
				{
					if (currentColor == null)
						return pickColor.Color;
					else
						return currentColor.Color;
				}
				else
				{
					return defaultColor;
				}
			}
			set
			{
				if( ((pickColor != null) && ( pickColor.Color != value )) || (pickColor == null) )
				{
					UpdatePickColor(value);
					Refresh();
				}
			}
		}

		/// <summary>
		/// Design time support to reset the Color property to it's default value.
		/// </summary>
		public void ResetColor()
		{
			Color = defaultColor;
		}

		/// <summary>
		/// Design time support to indicate whether the Color property should be serialized.
		/// </summary>
		/// <returns></returns>
		public bool ShouldSerializeColor()
		{
			if( pickColor != null )
			{
				return pickColor.Color != defaultColor;
			}
			else
			{
				return false;
			}
		}

		private void UpdatePickColor()
		{
			if( pickColor != null )
			{
				UpdatePickColor(pickColor.Color);
			}
			else
			{
				UpdatePickColor(defaultColor);
			}
		}

		private void UpdatePickColor(Color c)
		{
			pickColor = ColorWellFromColor(c);
			if (null == pickColor)
			{
				pickColor = ColorWellFromColor(defaultColor);
			}
		}

		/// <summary>
		/// Set/get the set of colors displayed by the control. Some colorsets are prepared to be used in typical web apps.
		/// See <see cref="GuruComponents.Netrix.UserInterface.ColorPicker.ColorSet">ColorSet</see>, too.
		/// </summary>
		[Browsable(true)]
		[Category("ColorPanel")]
		[DefaultValue(defaultColorSet)]
		[Description("Get/set the palette of colors to be displayed.")]
		public ColorSet ColorSet
		{
			get
			{
				return colorSet;
			}
			set
			{
				Utils.CheckValidEnumValue("ColorSet", value, typeof(ColorSet) );
				if (value != colorSet)
				{
					if (value == ColorSet.Custom)
					{
						colorWells = ColorWellHelper.GetCustomColorWells( customColors, colorSortOrder );
					}
					else
					{
						colorWells = ColorWellHelper.GetColorWells(value, colorSortOrder);
					}
					colorSet = value;
					UpdatePickColor();
					// FireColorChanged();
					AutoSizePanel();
				}
			}
		}

		/// <summary>
		/// Set/get the size of the color wells.
		/// </summary>
		[Browsable(true)]
		[Category("ColorPanel")]
		[Description("Set/get the size of the color wells displayed in the color panel.")]
		public System.Drawing.Size ColorWellSize
		{
			get
			{
				return this.colorWellSize;
			}
			set
			{
				if( value.Height > SystemInformation.Border3DSize.Height*2+2 &&
					value.Width > SystemInformation.Border3DSize.Width*2+2 )
				{
					if( value != this.ColorWellSize )
					{
						this.colorWellSize = value;
						AutoSizePanel();
					}
				}
				else
				{
					Size min = new Size(
						SystemInformation.Border3DSize.Height*2+2,
						SystemInformation.Border3DSize.Width*2+2 );

					string msg = string.Format( "The color well size must be at least {0}.", min );

					throw new ArgumentOutOfRangeException( "ColorWellSize", value, msg );
				}
			}
		}

		/// <summary>
		/// Design time support to reset the ColorWellSize property to it's default value.
		/// </summary>
		public void ResetColorWellSize()
		{
			ColorWellSize = defaultColorWellSize;
		}

		/// <summary>
		/// Design time support to indicate whether the ColorWellSize property should be serialized.
		/// </summary>
		/// <returns></returns>
		public bool ShouldSerializeColorWellSize()
		{
			return colorWellSize != defaultColorWellSize;
		}

		/// <summary>
		/// Set/get the order in which colors in the palette should be sorted.<br></br><br></br>
		/// See <see cref="GuruComponents.Netrix.UserInterface.ColorPicker.ColorSortOrder">ColorSortOrder</see>.
		/// </summary>
		[Browsable(true), Category("ColorPanel"), DefaultValue(defaultColorSortOrder)]
		[Description("Get/set the order that the colors in the color palette are displayed.")]
		public ColorSortOrder ColorSortOrder
		{
			get
			{
				return colorSortOrder;
			}
			set
			{
				Utils.CheckValidEnumValue( "ColorSortOrder", value, typeof(ColorSortOrder) );

				if( value != colorSortOrder )
				{
					ColorWellHelper.SortColorWells(colorWells, value);
					LayoutColorWells();
					colorSortOrder = value;
					Refresh();
				}
			}
		}

		/// <summary>
		/// Set/get the number of preferred columns.<br></br><br></br>
		/// If you set this value less than or equal to 0, you may resize the control.<br></br>
		/// If you set this greater that 0 the control will have a fixed width
		/// of 'Columns' unless 
		/// there are fewer colors than 'Columns' in the palette in which case it will display all colors in
		/// a single row.
		/// </summary>
		[Browsable(true), Category("ColorPanel"), DefaultValue(defaultPreferredColumns)]
		[Description("Set/get the number of preferred columns.  If set to 0 the control can be manually resized.")]
		public int Columns
		{
			get
			{
				return preferredColumns;
			}
			set
			{
				if( value > 0 )
				{
					if( value <= colorWells.Length )
					{
						preferredColumns = value;
					}
					else
					{
						preferredColumns = colorWells.Length;
					}
				}
				else
				{
					preferredColumns = 0;
				}

				AutoSizePanel();
			}
		}

		/// <summary>
		/// Set/get the custom color palette to be displayed.
		/// </summary>
		[Browsable(true)]
		[Category("ColorPanel")]
		[Description("Set/get the custom color palette.")]
		public List<Color> CustomColors
		{
			get
			{
				return customColors;
			}
			set
			{

				customColors = value;

                if (colorSet == ColorSet.Custom && customColors != null && !DesignMode)
				{
					// apply custom colors
					colorWells = ColorWellHelper.GetCustomColorWells(customColors, colorSortOrder);

					UpdatePickColor();

					LayoutColorWells();
					AutoSizePanel();
				}
			}
		}

		/// <summary>
		/// Design time support to reset the CustomColors property to it's default value.
		/// </summary>
		public void ResetCustomColors()
		{
			CustomColors = ColorPanel.DefaultCustomColors();
		}

		/// <summary>
		/// Helper for ColorPicker/ColorPanel
		/// </summary>
		/// <returns></returns>
        internal static List<Color> DefaultCustomColors()
		{
            List<Color> colors = new List<Color>(16);
            colors.AddRange(new Color[] 
                {
                    Color.Black,
                    Color.Blue,
                    Color.DarkBlue,
                    Color.Red,

                    Color.Green,
                    Color.Olive,
                    Color.Violet,
                    Color.Yellow,

                    Color.Brown,
                    Color.DarkRed,
                    Color.Gray,
                    Color.Aquamarine,

                    Color.DarkBlue,
                    Color.Coral,
                    Color.Silver,
                    Color.White
                }
            );
			return colors;
		}

		/// <summary>
		/// Design time support to indicate whether the CustomColors property should be serialized.
		/// </summary>
		public bool ShouldSerializeCustomColors()
		{
			return ColorPanel.ShouldSerializeCustomColors(customColors);
		}

		/// <summary>
		/// Helper for ColorPicker/ColorPanel
		/// </summary>
		/// <returns></returns>
        internal static bool ShouldSerializeCustomColors(List<Color> customColors)
		{
			bool bShouldSerialize = (customColors.Count != 16);

			if( !bShouldSerialize )
			{
				foreach( Color c in customColors )
				{
					if( c != Color.White )
					{
						bShouldSerialize = true;
						break;
					}
				}
			}

			return bShouldSerialize;
		}
	}

    }
