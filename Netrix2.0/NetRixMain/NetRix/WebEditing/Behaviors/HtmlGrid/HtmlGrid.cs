using System;
using System.ComponentModel;
using System.Drawing;
using GuruComponents.Netrix;
using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.WebEditing.Elements;

namespace GuruComponents.Netrix.WebEditing.Behaviors
{
	/// <summary>
	/// Displays a grid of pixels to help positioning elements in absolute position mode.
	/// </summary>
	/// <remarks>
	/// Optionally implements SnapRect to snap new elements to grid borders. Gridsize defaults to 8.
	/// <para>The grid does not make much sense in flow mode. Consider using <see cref="GuruComponents.Netrix.HtmlEditor.AbsolutePositioningEnabled">AbsolutePositioningEnabled</see>.</para>
	/// </remarks> 
	/// <seealso cref="SnapElement"/>
	/// <seealso cref="GuruComponents.Netrix.HtmlEditor.AbsolutePositioningEnabled">AbsolutePositioningEnabled</seealso>
	[Serializable()]
	public sealed class HtmlGrid : IHtmlGrid
	{
		private bool snapEnabled;
		private Size gridSize;
		private bool gridVisible;
		private Color gridColor;
		private GridType gridType;
		private GridLineType gridLineType;
		private int gridLineWidth;
		private Interop.IElementBehaviorSite _behaviorSite;
		private Interop.IHTMLPaintSite _paintsite;
		private int snapZone;
		private bool snapOnResize;
		private IHtmlEditor htmlEditor;
		private BodyElement body;

		# region Public Properties

		/// <summary>
		/// Number of pixel the grid will snap.
		/// </summary>
		/// <remarks>
		/// If the grid is every 20 pixels and the zone is 5, the snap feature will work from pixel 15 to 25 on first column.
		/// The pixel between 25 and 35 on second column will not snap. If the zone is equal or greater, then grid elements
		/// will always snap.
		/// <para>
		/// The default value is 4. If the default grid size is 8 x 8, the snap feature will snap anywhere, if it is active.
		/// </para>
		/// </remarks>
		[DefaultValue(4), Browsable(true), Category("Netrix Component"), Description("Number of pixel the grid will snap.")]        
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public int SnapZone 
		{
			get
			{
				return snapZone;
			}
			set
			{
				snapZone = value;
			}
		}

		/// <summary>
		/// Snap will also work on element resize.
		/// </summary>
		/// <remarks>
		/// The <see cref="SnapZone"/> property applies, too.
		/// </remarks>
		[DefaultValue(true), Browsable(true), Category("Netrix Component"), Description("Snap will also work on element resize.")]        
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public bool SnapOnResize 
		{
			get
			{
				return snapOnResize;
			}
			set
			{
				snapOnResize = value;
			}
		}


		/// <summary>
		/// The visual appearance of lines, if used instead of points.
		/// </summary>
		[DefaultValue(GridLineType.Solid), Browsable(true), Category("Netrix Component"), Description("The visual appearance of lines, if used instead of points.")]        
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public GridLineType GridLineVisualisation 
		{ 
			get 
			{
				return gridLineType; 
			}
			set
			{
				gridLineType = value;
				if (_paintsite != null && GridVisible)
				{
					_paintsite.InvalidateRect(IntPtr.Zero);
				} 			
			}
		}

		/// <summary>
		/// The width of lines if the grid is shown as lines or the width of crosses in case cross view.
		/// </summary>
		[DefaultValue(1), Browsable(true), Category("Netrix Component"), Description("The width of lines if the grid is shown as lines or the width of crosses in case cross view.")]        
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public int GridLineWidth 
		{ 
			get
			{
				return gridLineWidth;
			}
			set
			{
				gridLineWidth = value;
				if (_paintsite != null && GridVisible)
				{
					_paintsite.InvalidateRect(IntPtr.Zero);
				} 			
			}
		}

		/// <summary>
		/// The visual appearance of the grid.
		/// </summary>
		/// <remarks>
		/// The grid can appear as points (default), crosses and lines. Making crosses bigger than point distance will
		/// produce lines, too. 
		/// </remarks>
		[DefaultValue(GridType.Points), Browsable(true), Category("Netrix Component"), Description("Gets or sets the visual appearance of the grid.")]        
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public GridType GridVisualisation 
		{ 
			get
			{
				return gridType ;
			}
			set
			{
				gridType = value;
				if (_paintsite != null && GridVisible)
				{
					_paintsite.InvalidateRect(IntPtr.Zero);
				} 			
			}
		}

		/// <summary>
		/// Gets or sets the visibility of the grid.
		/// </summary>
		/// <remarks>
		/// The grid is a good base to build the VS.NET like GridLayout.
		/// <seealso cref="SnapEnabled">SnapEnabled</seealso>
		/// <seealso cref="GuruComponents.Netrix.HtmlEditor.AbsolutePositioningEnabled">AbsolutePositioningEnabled</seealso>
		/// <seealso cref="GuruComponents.Netrix.HtmlEditor.MultipleSelectionEnabled">MultipleSelectionEnabled</seealso>
		/// </remarks>
		/// <example>
		/// To simulate the VS.NET GridLayout just set the following properties in one step. This code assumes that
		/// you have set the boolean member variable <c>State</c> before calling the sequence:
		/// <code>
		///this.htmlEditor1.GridVisible = State;
		///this.htmlEditor1.SnapEnabled = State;
		///this.htmlEditor1.AbsolutePositioningEnabled = State;
		///this.htmlEditor1.MultipleSelectionEnabled = State;</code>
		/// </example>
		[DefaultValue(false), Browsable(true), Category("Netrix Component"), Description("Gets or sets the visibility of the grid.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public bool GridVisible
		{
			get
			{
				return gridVisible;
			}

			set
			{
				gridVisible = value;
				SetGrid();        
			}
		}

		/// <summary>
		/// Enables snap of elements to grid if absolute positioning is enabled.
		/// </summary>
		/// <remarks>
		/// In flow layout is this function useless, therefore this options activates grid mode, too.
		/// </remarks>
		[DefaultValue(false), Browsable(true), Category("Netrix Component"), Description("Enables snap of elements to grid if absolute positioning is enabled. In flow layout is this function useless, therefore this options activates grid mode, too.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public bool SnapEnabled
		{
			get
			{
				return snapEnabled;
			}

			set
			{
				snapEnabled = value;
			}
		}

		/// <summary>
		/// Gets or sets the gap between pixels shown as grid.
		/// </summary>
		/// <remarks>
		/// Defaults to 8 for both width and height, if no value is set. Setting values lower than 8 will cause performance
		/// problems on large surfaces because to many point are plotted with any screen refresh.
		/// </remarks>
		[DefaultValue(typeof(Size), "8,8")]
		[Browsable(true), Category("Netrix Component"), Description("Defines the grid size in pixel. Defaults to 8. The grid is made by plotting dots, crosses or lines.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public Size GridSize
		{
			get
			{
				return gridSize;
			}

			set
			{
				gridSize = value;
				if (_paintsite != null && GridVisible)
				{
					_paintsite.InvalidateRect(IntPtr.Zero);
				}            
			}
		}

		/// <summary>
		/// Gets or sets the color of the grid pixel.
		/// </summary>
		/// <remarks>
		/// Defaults to <c>Color.Black</c>.
		/// </remarks>
		[DefaultValue(typeof(Color), "Black")]
		[Browsable(true), Category("Netrix Component"), Description("Gets or sets the color of the grid pixel.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public Color GridColor
		{
			set
			{
				gridColor = value;
				if (_paintsite != null && GridVisible)
				{
					_paintsite.InvalidateRect(IntPtr.Zero);
				} 
			}
			get
			{
				return gridColor;
			}
		}

		private int Win32GridColor
		{
			get
			{
				return ColorTranslator.ToWin32(GridColor);
			}
		}


		# endregion
		
		/// <summary>
		/// Creates a grid object and sets the grid width to a default of 8.
		/// </summary>
		public HtmlGrid(IHtmlEditor editor)
		{
			htmlEditor = editor;
            htmlEditor.ReadyStateComplete -= new EventHandler(htmlEditor_ReadyStateComplete);
            htmlEditor.ReadyStateComplete += new EventHandler(htmlEditor_ReadyStateComplete);
			snapEnabled = false;
			gridSize = new Size(8, 8);
			gridVisible = false;
			gridColor = Color.Black;            
			gridType = GridType.Points;
			gridLineType = GridLineType.Solid;
			gridLineWidth = 1;
			snapZone = 4;
			snapOnResize = true;
			snapEnabled = false;
		}

		void htmlEditor_ReadyStateComplete(object sender, EventArgs e)
		{
            if (this.htmlEditor.DesignModeEnabled)
            {
                body = this.htmlEditor.GetBodyElement() as BodyElement;
                SetGrid();
            }
		}

		private void SetGrid()
		{
            if (body == null) return;
			_paintsite = null;
			if (GridVisible)
			{
				body.ElementBehaviors.AddBehavior(this);
			}
			else
			{
				body.ElementBehaviors.RemoveBehavior(this);
			}
		}

		/// <summary>
		/// Returns a descriptive information about the number of changed properties for usage in design time environments.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			int changed = 0;
			if (gridColor != Color.Black) changed++;
			if (gridVisible != false) changed++;
			if (snapEnabled != false) changed++;
			if (snapOnResize != true) changed++;
			if (snapZone != 4) changed++;
			if (!gridSize.Equals(new Size(8, 8))) changed++;
			if (gridType != GridType.Points) changed++;
			if (gridLineType != GridLineType.Solid) changed++;
			if (gridLineWidth != 1) changed++;
			return String.Format("{0} propert{1} changed", changed, (changed == 1) ? "y" : "ies");
		}

		/// <summary>
		/// The name of the behavior.
		/// </summary>
		/// <remarks>
		/// The name is used to remove the behavior later. Readonly.
		/// </remarks>
		[Browsable(false)]
		public string Name
		{
			get
			{
				return "HtmlGrid#Behavior#";
			}
		}

		/// <summary>
		/// The method used to draw the grid.
		/// </summary>
		/// <remarks>
		/// The method cannot be changed. Internally the API-function <c>SetPixel</c> is used to avoid performance
		/// flaw.
		/// </remarks>
		/// <param name="leftBounds"></param>
		/// <param name="topBounds"></param>
		/// <param name="rightBounds"></param>
		/// <param name="bottomBounds"></param>
		/// <param name="hdc"></param>
		public void Draw(int leftBounds, int topBounds, int rightBounds, int bottomBounds, IntPtr hdc)
		{
			if (!GridVisible)
			{
				if (_paintsite != null)
				{
					_paintsite.InvalidateRect(IntPtr.Zero);
				}
				return;
			}
			IntPtr pen;
			IntPtr pp = IntPtr.Zero;
			switch (GridVisualisation)
			{
				case GridType.Points:
					for (int i = leftBounds; i < rightBounds; i += gridSize.Width)
					{
						for (int j = topBounds; j < bottomBounds; j += gridSize.Height)
						{
							Win32.SetPixel(hdc, i, j, Win32GridColor);
						}
					}
					break;
				case GridType.Lines:
					pen = Win32.CreatePen((int)gridLineType, (int)gridLineWidth, Win32GridColor);
					Win32.SelectObject(hdc, pen);                    
					for (int i = leftBounds; i < rightBounds; i += gridSize.Width)
					{                        
						Win32.MoveToEx(hdc, i, 0, pp);
						Win32.LineTo(hdc, i, bottomBounds);
					}			        
					for (int j = topBounds; j < bottomBounds; j += gridSize.Height)
					{                        
						Win32.MoveToEx(hdc, 0, j, pp);
						Win32.LineTo(hdc, rightBounds, j);
					}
					Win32.DeleteObject(pen);
					break;
				case GridType.Cross:
					pen = Win32.CreatePen((int)gridLineType, 1, Win32GridColor);
					Win32.SelectObject(hdc, pen);                    
					for (int i = leftBounds; i < rightBounds; i += gridSize.Width)
					{
						for (int j = topBounds; j < bottomBounds; j += gridSize.Height)
						{
							Win32.MoveToEx(hdc, (int)(i-gridLineWidth), j, pp);
							Win32.LineTo(hdc, (int)(i+gridLineWidth+1), j);
							Win32.MoveToEx(hdc, i, (int)(j-gridLineWidth), pp);
							Win32.LineTo(hdc, i, (int)(j+gridLineWidth+1));
						}
					}
					Win32.DeleteObject(pen);
					break;
			}	
		}

		#region IHTMLPainter Member

		void Interop.IHTMLPainter.Draw(int leftBounds, int topBounds, int rightBounds, int bottomBounds, int leftUpdate, int topUpdate, int rightUpdate, int bottomUpdate, int lDrawFlags, IntPtr hdc, IntPtr pvDrawObject)
		{
			this.Draw(leftBounds, topBounds, rightBounds, bottomBounds, hdc);
		}

		/// <summary>
		/// Called when an element containing a rendering behavior is resized.
		/// </summary>
		/// <param name="cx">Width after resize</param>
		/// <param name="cy">Height after resize</param>
		void Interop.IHTMLPainter.OnResize(int cx, int cy)
		{
		}

		/// <summary>
		/// This methode creates a PainterInfo object and returns it. Normally one shouldnot overwrite this
		/// method and use the properties <see cref="BaseBehavior.HtmlPaintFlag">HtmlPaintFlag</see> 
		/// and <see cref="BaseBehavior.HtmlZOrderFlag">HtmlZOrderFlag</see> instead.
		/// </summary>
		/// <param name="pInfo"></param>
		void Interop.IHTMLPainter.GetPainterInfo(Interop.HTML_PAINTER_INFO pInfo)
		{            
			pInfo.lFlags = (int) HtmlPainter.Transparent;
			pInfo.lZOrder = (int) HtmlZOrder.BelowContent;
			pInfo.iidDrawObject = Guid.Empty;
			pInfo.rcBounds = new Interop.RECT(0, 0, 0, 0);
		}

		/// <summary>
		/// The current test hit can checked against the element coordinates.
		/// </summary>
		/// <param name="ptx">X coordinate</param>
		/// <param name="pty">Y coordinate</param>
		/// <param name="pbHit"></param>
		/// <param name="plPartID"></param>
		void Interop.IHTMLPainter.HitTestPoint(int ptx, int pty, out bool pbHit, out int plPartID)
		{
			plPartID = 0;
			pbHit = false;
		}

		#endregion

		#region IElementBehavior Member

		void Interop.IElementBehavior.Init(Interop.IElementBehaviorSite pBehaviorSite)
		{
			_behaviorSite = pBehaviorSite;
			_paintsite = (Interop.IHTMLPaintSite) _behaviorSite;
		}

		void Interop.IElementBehavior.Notify(int dwEvent, IntPtr pVar)
		{
		}

		void Interop.IElementBehavior.Detach()
		{
		}

		#endregion

		public Interop.IElementBehavior GetBehavior(IHtmlEditor editor, Interop.IHTMLElement element)
		{            
			return (Interop.IElementBehavior) this;
		}

		public System.Web.UI.Control GetElement(IHtmlEditor editor)
		{            
			return null;
		}
	}
}