using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using GuruComponents.Netrix.Events;
using GuruComponents.Netrix.WebEditing.Elements;

namespace GuruComponents.Netrix.UserInterface.Ruler
{

	/// <summary>
	/// Creates a ruler, which can be placed either vertical or horizontal.
	/// RulerControl creates a simple but powerful ruler.
	/// </summary>
    /// <remarks>
    /// The ruler has several features especially for designing HTML pages:
    /// <list type="bullet">
    ///     <item>Show measures in pixel and point.</item>
    ///     <item>Define beginning of scale to respect scroll positions.</item>
    ///     <item>Show margins.</item>
    ///     <item>Show segments, for instance, show table columns/rows or selected object's boundaries.</item>
    ///     <item>Free setting of font, colors, and scales.</item>
    ///     <item>Screen and show mouse position.</item>
    /// </list>
    /// Using various properties the ruler can be highly customized and used to present measures and
    /// boundaries of objects, set them interactively and fire events to change objects directly from ruler
    /// operations using the mouse.
    /// </remarks>
    /// <remarks>
    /// The ruler can be attache either vertically or horizontally. For usage with NetRix component it's recommended
    /// to use the scaling in pixel and attach the scroll event to set appropriate start value.
    /// <example>
    /// The following example assumes you have two ruler, one at the top and one at the left of the editor control:
    /// <code>
    /// // Attach the scroll event to an event handler
    /// this.htmlEditor1.Window.Scroll += new GuruComponents.Netrix.Events.DocumentEventHandler(Window_Scroll);
    /// 
    /// // in the handler manipulate the ruler to follow the scrollbar
    /// void Window_Scroll(object sender, GuruComponents.Netrix.Events.DocumentEventArgs e)
    /// {
    ///    rulerControl1.StartValue = htmlEditor1.HorizontalScrollPosition;
    ///    rulerControl2.StartValue = htmlEditor1.VerticalScrollPosition;
    /// }
    /// </code>
    /// You can use VS.NET designer to set several properties of the ruler to customize according your application's conditions.
    /// </example>
    /// </remarks>
	[ToolboxItem(true)]
    [ToolboxBitmap(typeof(ResourceManager), "Resources.Toolbox.Ruler.ico")]
    public class RulerControl : Control, IMessageFilter
    {
        /// <summary>
        /// Controls the way margin areas are drawn.
        /// </summary>
        public enum Margins
        {
            /// <summary>
            /// No margin area.
            /// </summary>
            None,
            /// <summary>
            /// Left (on horizontal) or top (on vertical).
            /// </summary>
            LeftOrTop,
            /// <summary>
            /// Right (on horizontal) or bottom (on vertical).
            /// </summary>
            RightOrBottom,
            /// <summary>
            /// Show both margins.
            /// </summary>
            Both
        }

        /// <summary>
        /// Defines a segment on the ruler to represent object dimensions.
        /// </summary>
        [Serializable()]
        public class Segment
        {
            private int start, end;
            [NonSerialized()]
            private IElement element;
            private Orientation orientation;
            [NonSerialized()]
            private RulerControl parentControl;

            /// <summary>
            /// Creates a segment.
            /// </summary>
            /// <param name="parentControl">Rulercontrol this segment belongs to.</param>
            public Segment(RulerControl parentControl)
            {
                this.parentControl = parentControl;
                this.orientation = parentControl.Orientation;
                this.start = 0;
                this.end = 0;
                this.element = null;
            }

            /// <summary>
            /// Creates a segment with parameters.
            /// </summary>
            /// <param name="parentControl">Rulercontrol this segment belongs to.</param>
            /// <param name="start">Start position</param>
            /// <param name="end">End position</param>
            public Segment(RulerControl parentControl, int start, int end)
            {
                this.start = start;
                this.end = end;
                this.element = null;
                this.orientation = parentControl.Orientation;
                this.parentControl = parentControl;
            }

            //public static bool operator ==(Segment s1, Segment s2)
            //{
            //    if (s1 == null && s2 == null) return true;
            //    if (s1 != null || s2 != null) return false;
            //    return (s1.Element == s2.Element && s1.Start == s2.Start && s1.End == s2.End);
            //}

            //public static bool operator !=(Segment s1, Segment s2)
            //{
            //    if ((s1 == null || s2 == null) && s1 != s2) return true;
            //    if ((s1 == null || s2 == null) && s1 == s2) return false;
            //    return (s1.Element != s2.Element || s1.Start != s2.Start || s1.End != s2.End);
            //}

            /// <summary>
            /// Start value for object, e.g. left or top boundary of a DIV.
            /// </summary>
            public int Start
            {
                get { return start; }
                set { start = value; }
            }

            /// <summary>
            /// End value for object, e.g. right or bottom boundary of a DIV.
            /// </summary>
            public int End
            {
                get { return end; }
                set { end = value; }
            }

            /// <summary>
            /// Orientation of the ruler, either vertically or horizontally.
            /// </summary>
            public Orientation Orientation
            {
                get { return orientation; }
                set { orientation = value; }
            }
            /// <summary>
            /// Element attached to this segment.
            /// </summary>
            public IElement Element
            {
                get { return element; }
                set 
                { 
                    element = value;
                    element.Resize += new DocumentEventHandler(element_Resize);
                    element.Move += new DocumentEventHandler(element_Move);
                }
            }

            void element_Move(object sender, DocumentEventArgs e)
            {
                SetDimensions();
            }

            void element_Resize(object sender, DocumentEventArgs e)
            {
                SetDimensions();
            }

            private void SetDimensions()
            {
                Rectangle area = element.GetAbsoluteArea();
                int left = element.HtmlEditor.GetBodyElement().GetBaseElement().GetOffsetLeft();
                int top = element.HtmlEditor.GetBodyElement().GetBaseElement().GetOffsetTop();
                if (Orientation == Orientation.Horizontal)
                {
                    if (area.Left + left != Start || area.Right + left != End)
                    {
                        this.Start = area.Left + left;
                        this.End = area.Right + left;
                        parentControl._Bitmap = null;
                        parentControl.Invalidate();
                    }
                }
                else
                {
                    if (Start != area.Top + top || End != area.Bottom + top)
                    {
                        this.Start = area.Top + top;
                        this.End = area.Bottom + top;
                        parentControl._Bitmap = null;
                        parentControl.Invalidate();
                    }
                }
            }

            /// <summary>
            /// String representation of a segment.
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return String.Format("Segment {0}-{1}", Start, End);
            }
        }

        #region Internal Variables

        private Margins             _showMargin;
        private Border3DStyle       _marginStyle        = Border3DStyle.Raised;
        private int                 _leftTopMargin;
        private int                 _rightBottomMargin;
        private int                 _scrollbaroffset    = 16;
        private List<Segment> _segments;
        private Border3DSide        _segmentSides       = Border3DSide.All;
        private Border3DStyle       _segmentStyle       = Border3DStyle.Etched;
		private int					_Scale;
		private int					_ScaleStartValue;
		private bool				_bDrawLine			= false;
	    private int					_iMousePosition		= 1;
	    private Bitmap				_Bitmap				= null;
        #endregion

        #region Property variable

        private Orientation		_Orientation;
		private ScaleMode		_ScaleMode;
		private RulerAlignment	_RulerAlignment     = RulerAlignment.BottomOrRight;
		private Border3DStyle		_i3DBorderStyle     = Border3DStyle.Etched;
		private int					_iMajorInterval     = 100;
		private int					_iNumberOfDivisions = 10;
		private int					_DivisionMarkFactor = 5;
		private int					_MiddleMarkFactor	= 3;
		private double				_ZoomFactor         = 1;
		private double				_StartValue			= 0;
		private bool				_bMouseTrackingOn   = false;
		private bool				_VerticalNumbers	= true;

		#endregion

		#region Event Arguments

        /// <summary>
        /// Event arguments for the ScaleModeChanged event.
        /// </summary>
		public class ScaleModeChangedEventArgs : EventArgs
		{
			/// <summary>
			/// Returns the current scale mode.
			/// </summary>
            public ScaleMode Mode;

            /// <summary>
            /// Ctor for ScaleModeChangedEventArgs class.
            /// </summary>
            /// <param name="Mode">Mode the scaling is running in.</param>
			public ScaleModeChangedEventArgs(ScaleMode Mode)
			{
				this.Mode = Mode;
			}
		}

        /// <summary>
        /// Event arguments for the HooverValue event.
        /// </summary>
        public class HooverValueEventArgs : EventArgs
		{
            /// <summary>
            /// Return the hover value.
            /// </summary>
			public double Value;

            /// <summary>
            /// Ctor for HooverValueEventArgs class.
            /// </summary>
            /// <param name="Value">Current value</param>
			public HooverValueEventArgs(double Value)
			{
				this.Value = Value;
			}
		}


		#endregion

		#region Delegates

        /// <summary>
        /// Used to declare the ScaleModeChanged event.
        /// </summary>
        /// <param name="sender">Ruler object</param>
        /// <param name="e">Event arguments</param>
		public delegate void ScaleModeChangedEvent(object sender, ScaleModeChangedEventArgs e);
        /// <summary>
        /// Used to declare the HooverValue event.
        /// </summary>
        /// <param name="sender">Ruler object</param>
        /// <param name="e">Event arguments</param>
		public delegate void HooverValueEvent(object sender, HooverValueEventArgs e);
		// public delegate void ClickEvent(object sender, ClickEventArgs e);

		#endregion

		#region Events

        /// <summary>
        /// Fired if the scaling changes.
        /// </summary>
		public event ScaleModeChangedEvent ScaleModeChanged;
        /// <summary>
        /// Fired on mouse over.
        /// </summary>
		public event HooverValueEvent HooverValue;
        /// <summary>
        /// Fired for each mousemove step on a segments handle zone.
        /// </summary>
        /// <remarks>Not yet implemented. Reserved for future use.</remarks>
# pragma warning disable 0067
        public event EventHandler SegmentBorderMoved;
        /// <summary>
        /// Fored for each mousemove step on a margins handle zone.
        /// </summary>
        /// <remarks>Not yet implemented. Reserved for future use.</remarks>
# pragma warning disable 0067
        public event EventHandler MarginMoved;

        #endregion

        /// <summary> 
		/// Required designer variable.
		/// </summary>
		private Container components = null;


	    #region Constructors/Destructors

        /// <summary>
        /// Creates a new instance of ruler control.
        /// </summary>
        /// <remarks>
        /// The ctor sets various default values:
        /// <list type="bullet">
        /// <item>BackColor: White</item>
        /// <item>ForeColor: Black</item>
        /// <item>ScaleMode: Pixels</item>
        /// </list>
        /// </remarks>
        /// <seealso cref="ScaleMode"/>
        public RulerControl()
        {

            this._segments = new List<Segment>();

            base.BackColor = Color.White;
            base.ForeColor = Color.Black;

            _showMargin = Margins.Both;
            _leftTopMargin = 0;
            _rightBottomMargin = 0;

            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();

            ScaleMode = ScaleMode.Pixels;

            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
        }

		#endregion

		#region Methods

        /// <summary>
        /// Filter mouse events to move ruler barks.
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool PreFilterMessage(ref Message m)
		{
			if (!this._bMouseTrackingOn) return false;

			if (m.Msg == (int)Msg.WM_MOUSEMOVE)
			{
				int X = 0;
				int Y = 0;

				// The mouse coordinate are measured in screen coordinates because thats what 
				// Control.MousePosition returns.  The Message,LParam value is not used because
				// it returns the mouse position relative to the control the mouse is over. 
				// Chalk and cheese.

				Point pointScreen = MousePosition;

				// Get the origin of this control in screen coordinates so that later we can 
				// compare it against the mouse point to determine it we've hit this control.

				Point pointClientOrigin = new Point(X, Y);
				pointClientOrigin = PointToScreen(pointClientOrigin);

				_bDrawLine = false;

			    HooverValueEventArgs eHoover;

				// Work out whether the mouse is within the Y-axis bounds of a vertital ruler or 
				// within the X-axis bounds of a horizontal ruler

				if (this.Orientation == Orientation.Horizontal)
				{
					_bDrawLine = (pointScreen.X >= pointClientOrigin.X) && (pointScreen.X <= pointClientOrigin.X + this.Width);
				}
				else
				{
					_bDrawLine = (pointScreen.Y >= pointClientOrigin.Y) && (pointScreen.Y <= pointClientOrigin.Y + this.Height);
				}

				// If the mouse is in valid position...
				if (_bDrawLine)
				{
					// ...workout the position of the mouse relative to the 
					X = pointScreen.X-pointClientOrigin.X;
					Y = pointScreen.Y-pointClientOrigin.Y;

					// Determine whether the mouse is within the bounds of the control itself

				    // Make the relative mouse position available in pixel relative to this control's origin
					ChangeMousePosition((this.Orientation == Orientation.Horizontal) ? X : Y);
					eHoover = new HooverValueEventArgs(CalculateValue(_iMousePosition));

				} 
				else
				{
					ChangeMousePosition(-1);
					eHoover = new HooverValueEventArgs(_iMousePosition);
				}

				// Paint directly by calling the OnPaint() method.  This way the background is not 
				// hosed by the call to Invalidate() so paining occurs without the hint of a flicker
				PaintEventArgs e = null;
				try
				{
					e = new PaintEventArgs(this.CreateGraphics(), this.ClientRectangle);
					OnPaint(e);
				}
				finally
				{
                    if (e != null)
                    {
                        e.Graphics.Dispose();
                    }
				}

				OnHooverValue(eHoover);
			}

			if ((m.Msg == (int)Msg.WM_MOUSELEAVE) || 
				(m.Msg == (int)Msg.WM_NCMOUSELEAVE))
			{
				_bDrawLine = false;
				PaintEventArgs paintArgs = null;
				try
				{
					paintArgs = new PaintEventArgs(this.CreateGraphics(), this.ClientRectangle);
					this.OnPaint(paintArgs);
				}
				finally
				{
					if (paintArgs != null) paintArgs.Graphics.Dispose();
				}
			}

			return false;  // Whether or not the message is filtered
		}

        /// <summary>
        /// Scale value from pixel using the given offset.
        /// </summary>
        /// <param name="iOffset">The offset being used to calculate the value.</param>
        /// <returns></returns>
		public double PixelToScaleValue(int iOffset)
		{
			return this.CalculateValue(iOffset);
		}

        /// <summary>
        /// Calculate value to pixel using the given scale.
        /// </summary>
        /// <param name="nScaleValue">The scale being used to calculate the value.</param>
        /// <returns>Pixel value</returns>
        public int ScaleValueToPixel(double nScaleValue)
		{
			return CalculatePixel(nScaleValue);
		}

		#endregion

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			// 
			// RulerControl
			// 
			this.Name = "RulerControl";
			this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.RulerControl_MouseUp);
		}

		#endregion

		#region Overrides

        /// <summary>
        /// The ruler control class.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "RulerControl Properties";
        }

		/// <summary>
		/// Dispose the component.
		/// </summary>
		/// <param name="disposing"></param>
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

        /// <summary>
        /// Invalidate of resize.
        /// </summary>
        /// <remarks>
        /// Overwrite to prevent immediate refereshing.
        /// </remarks>
        /// <param name="e"></param>
		protected override void OnResize(EventArgs e)
		{
			base.OnResize (e);

			// Take private resize actions here
			_Bitmap = null;
			this.Invalidate();
		}

        /// <summary>
        /// Invalidate the whole region.
        /// </summary>
		public override void Refresh()
		{
			base.Refresh ();
			this.Invalidate();
		}

        /// <summary>
        /// Override to prevent immediae repainting.
        /// </summary>
        /// <param name="e"></param>
		[Description("Draws the ruler marks in the scale requested.")]
		protected override void OnPaint(PaintEventArgs e)
		{
			//base.OnPaint (e);
			DrawControl(e.Graphics);
		}

        /// <summary>
        /// Override to handle Message filter disposing yourself.
        /// </summary>
        /// <param name="e"></param>
		protected override void OnHandleDestroyed(EventArgs e)
		{
			base.OnHandleDestroyed (e);
			try
			{
				if (_bMouseTrackingOn) Application.RemoveMessageFilter(this);
			} 
			catch {}
		}

		#endregion

		#region Event Handlers

		private void RulerControl_MouseDown(object sender, MouseEventArgs e)
		{
//			if (e.Button.Equals(MouseButtons.Right)) 
		}

		private void RulerControl_MouseUp(object sender, MouseEventArgs e)
		{
            if ((MouseButtons & MouseButtons.Right) != 0 && this.ContextMenu != null) 
			{
				this.ContextMenu.Show(this, PointToClient(MousePosition));
			}
			else
			{
				EventArgs eClick = new EventArgs();
				this.OnClick(eClick);
			}
		}

        /// <summary>
        /// Get mouse move operations to deal with margin settings.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            // TODO: Implement Margin Changes here
        }

        /// <summary>
        /// Get mouse hover operations to deal with margin settings.
        /// </summary>
        /// <param name="e"></param>
        protected void OnHooverValue(HooverValueEventArgs e)
		{
			if (HooverValue != null) HooverValue(this, e);
		}

		/// <summary>
        /// Changed the scale mode, fires the <see cref="ScaleModeChanged"/> event.
		/// </summary>
		/// <param name="e"></param>
        protected void OnScaleModeChanged(ScaleModeChangedEventArgs e)
		{
			if (ScaleModeChanged != null) ScaleModeChanged(this, e);
		}

		/// <summary>
		/// Invalidates on enter.
		/// </summary>
		/// <param name="e"></param>
        protected override void OnEnter(EventArgs e)
		{
			base.OnEnter (e);
			_bDrawLine = false;
			Invalidate();
		}

        /// <summary>
        /// Invalidates on leave.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLeave(EventArgs e)
		{
			base.OnLeave (e);
			Invalidate();
		}

		#endregion

		#region Properties

        /// <summary>
        /// The border style use the Windows.Forms.Border3DStyle type.
        /// </summary>
		[
		DefaultValue(typeof(Border3DStyle),"Etched"),
		Description("The border style use the Windows.Forms.Border3DStyle type"),
		Category("Ruler"),
		]
		public Border3DStyle BorderStyle
		{
			get
			{
				return _i3DBorderStyle;
			}
			set
			{
				_i3DBorderStyle = value;
				_Bitmap = null;
				Invalidate();
			}
		}

        /// <summary>
        /// Horizontal or vertical layout.
        /// </summary>
		[Description("Horizontal or vertical layout.")]
		[Category("Ruler")]
		public Orientation Orientation
		{ 
			get { return _Orientation; }
			set 
			{
				_Orientation = value;
				_Bitmap = null;
				Invalidate();
			}
		}

        /// <summary>
        /// A value from which the ruler marking should be shown. Default is zero.
        /// </summary>
		[Description("A value from which the ruler marking should be shown. Default is zero.")]
		[Category("Ruler")]
		public double StartValue
		{
			get { return _StartValue; }
			set 
			{
				_StartValue = value;
				_ScaleStartValue = Convert.ToInt32(value * _Scale / _iMajorInterval);  // Convert value to pixels
				_Bitmap = null;
				Invalidate();
			}
		}

        /// <summary>
        /// The scale to use.
        /// </summary>
		[Description("The scale to use.")]
		[Category("Ruler")]
		public ScaleMode ScaleMode
		{
			get { return _ScaleMode; }
			set 
			{
				ScaleMode iOldScaleMode = _ScaleMode;
				_ScaleMode = value;    

				if (_iMajorInterval == DefaultMajorInterval(iOldScaleMode))
				{
					// Set the default Scale and MajorInterval value
					_Scale = DefaultScale(_ScaleMode);
					_iMajorInterval = DefaultMajorInterval(_ScaleMode);

				} 
				else
				{
					MajorInterval = _iMajorInterval;
				}

				// Use the current start value (if there is one)
				this.StartValue = this._StartValue;
				ScaleModeChangedEventArgs e = new ScaleModeChangedEventArgs(value);
				this.OnScaleModeChanged(e);
			}
		}

        /// <summary>
        /// The value of the major interval. 
        /// </summary>
        /// <remarks>
        /// If displaying inches, 1 is a typical value. If displaying Points, 36 or 72 might good values.
        /// </remarks>
		[Description("The value of the major interval. When displaying inches, 1 is a typical value.  When displaying Points, 36 or 72 might good values.")]
		[Category("Ruler")]
		public int MajorInterval
		{
			get { return _iMajorInterval; }
			set 
			{ 
				if (value <=0) throw new Exception("The major interval value cannot be less than one");
				_iMajorInterval = value;
				_Scale = DefaultScale(_ScaleMode) * _iMajorInterval / DefaultMajorInterval(_ScaleMode);
				_Bitmap = null;
				Invalidate();
			}
		}

        /// <summary>
        /// How many divisions should be shown between each major intervals.
        /// </summary>
		[Description("How many divisions should be shown between each major interval.")]
		[Category("Ruler")]
		public int Divisions
		{
			get { return _iNumberOfDivisions; }
			set 
			{
				if (value <=0) throw new Exception("The number of divisions cannot be less than one");
				_iNumberOfDivisions = value;
				_Bitmap = null;
				Invalidate();
			}
		}

        /// <summary>
        /// The height or width of this control multiplied by the reciprocal of this number will be used to compute the height of the non-middle division marks.
        /// </summary>
		[Description("The height or width of this control multiplied by the reciprocal of this number will be used to compute the height of the non-middle division marks.")]
		[Category("Ruler")]
		public int DivisionMarkFactor
		{
			get { return _DivisionMarkFactor; }
			set 
			{ 
				if (value <=0) throw new Exception("The Division Mark Factor cannot be less than one");
				_DivisionMarkFactor = value;
				_Bitmap = null;
				Invalidate();
			}
		}

        /// <summary>
        /// The height or width of this control multiplied by the reciprocal of this number will be used to compute the height of the middle division mark.
        /// </summary>
		[Description("The height or width of this control multiplied by the reciprocal of this number will be used to compute the height of the middle division mark.")]
		[Category("Ruler")]
		public int MiddleMarkFactor
		{
			get { return _MiddleMarkFactor; }
			set
			{
				if (value <=0) throw new Exception("The Middle Mark Factor cannot be less than one");
				_MiddleMarkFactor = value;
				_Bitmap = null;
				Invalidate();
			}
		}

        /// <summary>
        /// The value of the current mouse position expressed in unit of the scale set (centimetres, inches, etc).
        /// </summary>
		[Description("The value of the current mouse position expressed in unit of the scale set (centimetres, inches, etc).")]
		[Category("Ruler")]
		public double ScaleValue
		{
			get {return CalculateValue(_iMousePosition); }
		}

        /// <summary>
        /// True if a line is displayed to track the current position of the mouse and events are generated as the mouse moves.
        /// </summary>
		[Description("True if a line is displayed to track the current position of the mouse and events are generated as the mouse moves.")]
		[Category("Ruler")]
		public bool MouseTrackingOn
		{
			get { return _bMouseTrackingOn; }
			set 
			{ 
				if (value == _bMouseTrackingOn) return;
				
				if (value)
				{
					// Tracking is being enabled so add the message filter hook
					Application.AddMessageFilter(this);
				}
				else
				{
					// Tracking is being disabled so remove the message filter hook
					Application.RemoveMessageFilter(this);
					ChangeMousePosition(-1);
				}

				_bMouseTrackingOn = value;

				_Bitmap = null;
				Invalidate();
			}
		}

        /// <summary>
        /// The font used to display the division number.
        /// </summary>
		[Description("The font used to display the division number.")]
		public override Font Font
		{
			get
			{
				return base.Font;
			}
			set
			{
				base.Font = value;
				_Bitmap = null;
				Invalidate();
			}
		}

        /// <summary>
        /// Return the mouse position as number of pixels from the top or left of the control.
        /// </summary>
        /// <remarks>
        ///  -1 means that the mouse is positioned before or after the control.
        /// </remarks>
		[Description("Return the mouse position as number of pixels from the top or left of the control.  -1 means that the mouse is positioned before or after the control.")]
		[Category("Ruler")]
		public int MouseLocation
		{
			get { return _iMousePosition; }
		}

        /// <summary>
        /// The style used to draw additional segments.
        /// </summary>
        [DefaultValue(typeof(Border3DStyle), "Etched")]
        [Description("The style used to draw additional segments.")]
        [Category("Ruler")]
        public Border3DStyle SegmentStyle
        {
            get
            {
                return _segmentStyle;
            }
            set
            {
                _segmentStyle = value;
                _Bitmap = null;
                Invalidate();
            }
        }

        /// <summary>
        /// The sides used to draw the border zone of a segment.
        /// </summary>
        [DefaultValue(typeof(Border3DSide), "All")]
        [Description("The sides used to draw the border zone of a segment.")]
        [Category("Ruler")]
        public Border3DSide SegmentSides
        {
            get
            {
                return _segmentSides;
            }
            set
            {
                _segmentSides = value;
                _Bitmap = null;
                Invalidate();
            }
        }

        /// <summary>
        /// The list of segments displayed on the ruler.
        /// </summary>
        [DefaultValue(0)]
        [Description("The list of segments displayed on the ruler.")]
        [Browsable(false)]
        [Category("Ruler")]
        public List<Segment> Segments
        {
            get
            {
                return this._segments;
            }
            set
            {
                this._segments = value;
                _Bitmap = null;
                Invalidate();
            }
        }

        /// <summary>
        /// The color used to lines and numbers on the ruler.
        /// </summary>
		[DefaultValue(typeof(Color), "ControlDarkDark")]
		[Description("The color used to lines and numbers on the ruler.")]
        [Category("Ruler")]
        public override Color ForeColor
		{
			get
			{
				return base.ForeColor;
			}
			set
			{
				base.ForeColor = value;
				_Bitmap = null;
				Invalidate();
			}
		}
        
        /// <summary>
        /// The color used to paint the background of the ruler.
        /// </summary>
        /// <summary>
        /// The color used to paint the background of the ruler.
        /// </summary>
		[DefaultValue(typeof(Color), "White")]
		[Description("The color used to paint the background of the ruler.")]
        [Category("Ruler")]
        public override Color BackColor
		{
			get
			{
				return base.BackColor;
			}
			set
			{
				base.BackColor = value;
				_Bitmap = null;
				Invalidate();
			}
		}

        /// <summary>
        /// The style used to paint the background of the margin area.
        /// </summary>
        [DefaultValue(typeof(Border3DStyle), "Raised")]
        [Description("The style used to paint the background of the margin area.")]
        [Category("Ruler")]
        public Border3DStyle MarginStyle
        {
            get
            {
                return this._marginStyle;
            }
            set
            {
                this._marginStyle = value;
                _Bitmap = null;
                Invalidate();
            }
        }

        /// <summary>
        /// Whether it shows vertical numbers.
        /// </summary>
        [DefaultValue(typeof(Margins), "Both")]
        [Description("The color used to paint the background of the margin area.")]
        [Category("Ruler")]
        public Margins ShowMargin
        {
            get
            {
                return this._showMargin;
            }
            set
            {
                this._showMargin = value;
                _Bitmap = null;
                Invalidate();
            }
        }

        /// <summary>
        /// The left or top margin value.
        /// </summary>
        [DefaultValue(0)]
        [Description("The left or top margin value.")]
        [Category("Ruler")]
        public int LeftTopMargin
        {
            get
            {
                return this._leftTopMargin;
            }
            set
            {
                this._leftTopMargin = value;
                _Bitmap = null;
                Invalidate();
            }
        }

        /// <summary>
        /// The left or top margin value.
        /// </summary>
        [DefaultValue(0)]
        [Description("The right or bottom margin value.")]
        [Category("Ruler")]
        public int RightBottomMargin
        {
            get
            {
                return this._rightBottomMargin;
            }
            set
            {
                this._rightBottomMargin = value;
                _Bitmap = null;
                Invalidate();
            }
        }

        /// <summary>
        /// Offset which applies to margin and segment drawing if scrollbars inside the editor are visible.
        /// </summary>
        [Description("Offset which applies to margin and segment drawing if scrollbars inside the editor are visible.")]
        [Category("Ruler")]
        public int ScrollBarOffset
        {
            get { return _scrollbaroffset; }
            set
            {
                _scrollbaroffset = value;
                _Bitmap = null;
                Invalidate();
            }
        }

        /// <summary>
        /// Whether show numbers vertical.
        /// </summary>
		[Description("Whether show numbers vertical.")]
		[Category("Ruler")]
        public bool VerticalNumbers
		{
			get { return _VerticalNumbers; }
			set
			{
				_VerticalNumbers = value;
				_Bitmap = null;
				Invalidate();
			}
		}

        /// <summary>
        /// A factor between 0.1 and 10 by which the displayed scale will be zoomed.
        /// </summary>
		[Description("A factor between 0.1 and 10 by which the displayed scale will be zoomed.")]
		[Category("Ruler")]
		public double ZoomFactor
		{
			get { return _ZoomFactor; }
			set 
			{
				if ((value < 0.1) || (value > 10)) throw new Exception("Zoom factor can be between 10% and 1000%");
				if (_ZoomFactor == value) return;
				_ZoomFactor = value;
				this.ScaleMode = _ScaleMode;
				_Bitmap = null;
				Invalidate();
			}
		}

        /// <summary>
        /// Determines how the ruler markings are displayed.
        /// </summary>
		[Description("Determines how the ruler markings are displayed.")]
		[Category("Ruler")]
		public RulerAlignment RulerAlignment
		{
			get { return _RulerAlignment; }
			set 
			{
				if (_RulerAlignment == value) return;
				_RulerAlignment = value;
				_Bitmap = null;
				Invalidate();
			}
		}

		#endregion

        # region Public Service Methods

        /// <summary>
        /// Attaches an element to a new segment.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <returns>Returns the segment created to hold the element's relation.</returns>
        public Segment AttachElementToSegment(IElement element)
        {
            Segment s = new Segment(this);
            s.Element = element;
            if (_segments == null)
            {
                _segments = new List<Segment>();
            }
            Segments.Add(s);
            return s;
        }

        /// <summary>
        /// Removes the segment from segment list.
        /// </summary>
        /// <param name="s"></param>
        public void RemoveElementFromSegment(Segment s)
        {
            Segments.Remove(s);
            Invalidate();
        }

        # endregion Public Service Methods

        #region Private functions

        private double CalculateValue(int iOffset)
		{
			if (iOffset < 0) return 0;

			double nValue = ((double)iOffset-Start()) / _Scale * _iMajorInterval;
			return nValue + this._StartValue;
		}

		/// <summary>
        /// May not return zero even when a -ve scale number is given as the returned value needs to allow for the border thickness.
		/// </summary>
		/// <param name="nScaleValue"></param>
		/// <returns></returns>
		private int CalculatePixel(double nScaleValue)
		{

			double nValue = nScaleValue - this._StartValue;
			if (nValue < 0) return Start();  // Start is the offset to the actual display area to allow for the border (if any)

			int iOffset = Convert.ToInt32(nValue / _iMajorInterval * _Scale);

			return iOffset + Start();
		}

        /// <summary>
        /// Render the track line (on screen line of current mouse position).
        /// </summary>
        /// <param name="g"></param>
		public void RenderTrackLine(Graphics g)
		{
			if (_bMouseTrackingOn & _bDrawLine)
			{
				int iOffset = Offset();

				// Optionally render Mouse tracking line
				switch(Orientation)
				{
					case Orientation.Horizontal:
						Line(g, _iMousePosition, iOffset, _iMousePosition, Height - iOffset);
						break;
					case Orientation.Vertical:
						Line (g, iOffset, _iMousePosition, Width - iOffset, _iMousePosition);
						break;
				}
			}
		}

		private void DrawControl(Graphics graphics)
		{
			Graphics g;

			if (_Bitmap == null)
			{

				// Create a bitmap
				_Bitmap = new Bitmap(this.Width, this.Height);

				g = Graphics.FromImage(_Bitmap);

				try
				{
                    // prepare some values
                    int iScale = _Scale;
                    int iStart = Start();
                    int iOffset = Offset();
                    int iWidth = (this.Orientation == Orientation.Horizontal) ? Width - _scrollbaroffset : Width;
				    int iHeight = (this.Orientation == Orientation.Horizontal) ? Height : Height - _scrollbaroffset;
                    int iEnd = (this.Orientation == Orientation.Horizontal) ? iWidth : iHeight;
                    
                    // Wash the background with BackColor
                    g.FillRectangle(new SolidBrush(this.BackColor), iStart, iStart, _Bitmap.Width, _Bitmap.Height);
                    Rectangle areaRect;
					// draw the margins
                    if (ShowMargin != Margins.None)
                    {
                        // Add the margins, if required

                        if (this.Orientation == Orientation.Vertical)
                        {
                            if (ShowMargin == Margins.Both || ShowMargin == Margins.LeftOrTop)
                            {
                                areaRect = new Rectangle(iStart, iStart, iWidth / 2, LeftTopMargin);
                                ControlPaint.DrawBorder3D(g, areaRect, Border3DStyle.Raised, Border3DSide.All);
                            }
                            if (ShowMargin == Margins.Both || ShowMargin == Margins.RightOrBottom)
                            {
                                areaRect = new Rectangle(iStart, iHeight - RightBottomMargin, iWidth / 2, RightBottomMargin);
                                ControlPaint.DrawBorder3D(g, areaRect, Border3DStyle.Raised, Border3DSide.All);
                            }
                        }
                        else // Orientation.Horizontal
                        {
                            if (ShowMargin == Margins.Both || ShowMargin == Margins.LeftOrTop)
                            {
                                areaRect = new Rectangle(iStart, iStart, LeftTopMargin, iHeight / 2);
                                ControlPaint.DrawBorder3D(g, areaRect, Border3DStyle.Raised, Border3DSide.All);
                            }
                            if (ShowMargin == Margins.Both || ShowMargin == Margins.RightOrBottom)
                            {
                                areaRect = new Rectangle(iWidth - RightBottomMargin - iStart, iStart, RightBottomMargin, iHeight / 2);
                                ControlPaint.DrawBorder3D(g, areaRect, Border3DStyle.Raised, Border3DSide.All);
                            }
                        }
                    }

                    // draw the Segments (each has "[Border][SegmentZone][Border]")
                    if (Segments != null)
                    {
                        foreach (Segment s in Segments)
                        {                            
                            if (this.Orientation == Orientation.Vertical)
                            {
                                areaRect = new Rectangle(iStart, s.Start, iWidth - iOffset - iStart, s.End - s.Start);
                                ControlPaint.DrawBorder3D(g, areaRect, SegmentStyle, Border3DSide.All);
                            }
                            else // Orientation.Horizontal
                            {
                                areaRect = new Rectangle(s.Start, iStart, s.End - s.Start, iHeight - iStart - iOffset);
                                ControlPaint.DrawBorder3D(g, areaRect, SegmentStyle, Border3DSide.All);
                            }
                        }
                    }

                    // Paint the lines on the image
					for(int j = iStart; j <= iEnd; j += iScale)
					{
						int iLeft = _Scale;  // Make an assumption that we're starting at zero or on a major increment
						int jOffset = j+_ScaleStartValue;

						iScale = ((jOffset-iStart) % _Scale);  // Get the mod vale to see if this is "big line" opportunity

						// If it is, draw big line
						if (iScale == 0)
						{
							if (_RulerAlignment != RulerAlignment.Middle)
							{
								if (this.Orientation == Orientation.Horizontal)
									Line(g, j, 0, j, iHeight);
								else
									Line (g, 0, j, iWidth, j);
							}

							iLeft = _Scale;     // Set the for loop increment
						} 
						else
						{
							iLeft = _Scale - iScale;     // Set the for loop increment
						}

						iScale = iLeft;

						int iValue = (((jOffset-iStart)/_Scale)+1) * _iMajorInterval;
						DrawValue(g, iValue, j - iStart, iScale);

						int iUsed = 0;

						//Draw small lines
						for(int i = 0; i < _iNumberOfDivisions; i++)
						{
							int iX = Convert.ToInt32(Math.Round((double)(_Scale-iUsed)/(double)(_iNumberOfDivisions - i),0)); // Use a spreading algorithm rather that using expensive floating point numbers
							iUsed += iX;

							if (iUsed >= (_Scale-iLeft))
							{
								iX = iUsed+j-(_Scale-iLeft);

								// Is it an even number and, if so, is it the middle value?
								bool bMiddleMark = ((_iNumberOfDivisions & 0x1) == 0) & (i+1==_iNumberOfDivisions/2);
								bool bShowMiddleMark = bMiddleMark;
								bool bLastDivisionMark = (i+1 == _iNumberOfDivisions);
								bool bLastAlignMiddleDivisionMark =  bLastDivisionMark & (_RulerAlignment == RulerAlignment.Middle);
								bool bShowDivisionMark = !bMiddleMark & !bLastAlignMiddleDivisionMark;

								if (bShowMiddleMark)
								{
									DivisionMark(g, iX, _MiddleMarkFactor);  // Height or Width will be 1/3
								} 
								else if (bShowDivisionMark)
								{
									DivisionMark(g, iX, _DivisionMarkFactor);  // Height or Width will be 1/5
								}
							}
						}
					}
                    // scrollbar zone
                    if (ScrollBarOffset > 0)
                    {
                        if (this.Orientation == Orientation.Vertical)
                        {
                            ControlPaint.DrawScrollButton(g, iStart, Height - ScrollBarOffset, (Width - iOffset)/2, ScrollBarOffset, ScrollButton.Left, ButtonState.Normal);
                            ControlPaint.DrawScrollButton(g, iStart + Width/2, Height - ScrollBarOffset, (Width - iOffset)/2, ScrollBarOffset, ScrollButton.Right, ButtonState.Normal);
                        }
                        else
                        {
                            ControlPaint.DrawScrollButton(g, Width - ScrollBarOffset - iStart, iOffset, ScrollBarOffset, (Height - 2 * iOffset) / 2, ScrollButton.Up, ButtonState.Normal);
                            ControlPaint.DrawScrollButton(g, Width - ScrollBarOffset - iStart, iOffset + (Height / 2), ScrollBarOffset, (Height - 2 * iOffset) / 2, ScrollButton.Down, ButtonState.Normal);
                        }
                    }

				    // general border
                    if (_i3DBorderStyle != Border3DStyle.Flat)
                    {
                        ControlPaint.DrawBorder3D(g, this.ClientRectangle, this._i3DBorderStyle);
                    }
				}
				catch(Exception)
				{
				}
				finally 
				{
					g.Dispose();
				}
			}

			g = graphics;

			try
			{

				// Always draw the bitmap
				g.DrawImage(_Bitmap, this.ClientRectangle);

				RenderTrackLine(g);
			}
			catch(Exception)
			{
			}
			finally
			{
				GC.Collect();
			}

		}

		private void DivisionMark(Graphics g, int iPosition, int iProportion)
		{
			// This function is affected by the RulerAlignment setting

			int iMarkStart = 0, iMarkEnd = 0;

			if (this.Orientation == Orientation.Horizontal)
			{

				switch(_RulerAlignment)
				{
					case RulerAlignment.BottomOrRight:
					{
						iMarkStart = Height - Height/iProportion;
						iMarkEnd = Height;
						break;
					}
					case RulerAlignment.Middle:
					{
						iMarkStart = (Height - Height/iProportion)/2 -1;
						iMarkEnd = iMarkStart + Height/iProportion;
						break;
					}
					case RulerAlignment.TopOrLeft:
					{
						iMarkStart = 0;
						iMarkEnd = Height/iProportion;
						break;
					}
				}

				Line(g, iPosition, iMarkStart, iPosition, iMarkEnd);
			}
			else
			{

				switch(_RulerAlignment)
				{
					case RulerAlignment.BottomOrRight:
					{
						iMarkStart = Width - Width/iProportion;
						iMarkEnd = Width;
						break;
					}
					case RulerAlignment.Middle:
					{
						iMarkStart = (Width - Width/iProportion)/2 -1;
						iMarkEnd = iMarkStart + Width/iProportion;
						break;
					}
					case RulerAlignment.TopOrLeft:
					{
						iMarkStart = 0;
						iMarkEnd = Width/iProportion;
						break;
					}
				}

				Line(g, iMarkStart, iPosition, iMarkEnd, iPosition);
			}
		}

		private void DrawValue(Graphics g, int iValue, int iPosition, int iSpaceAvailable)
		{

			// The sizing operation is common to all options
			StringFormat format = new StringFormat(StringFormatFlags.MeasureTrailingSpaces);
			if (_VerticalNumbers)
				format.FormatFlags |= StringFormatFlags.DirectionVertical;
			
			SizeF size = g.MeasureString((iValue).ToString(), this.Font, iSpaceAvailable, format);

			Point drawingPoint;
			int iX = 0;
			int iY = 0;

			if (this.Orientation == Orientation.Horizontal)
			{
				switch(_RulerAlignment)
				{
					case RulerAlignment.BottomOrRight:
					{
						iX = iPosition + iSpaceAvailable - (int)size.Width - 2;
						iY = 2;
						break;
					}
					case RulerAlignment.Middle:
					{
						iX = iPosition + iSpaceAvailable - (int)size.Width/2;
						iY = (Height - (int)size.Height)/2 - 2;
						break;
					}
					case RulerAlignment.TopOrLeft:
					{
						iX = iPosition + iSpaceAvailable - (int)size.Width - 2;
						iY = Height - 2 - (int)size.Height;
						break;
					}
				}

				drawingPoint = new Point(iX, iY);
			}
			else
			{
				switch(_RulerAlignment)
				{
					case RulerAlignment.BottomOrRight:
					{
						iX = 2;
						iY = iPosition + iSpaceAvailable - (int)size.Height - 2;
						break;
					}
					case RulerAlignment.Middle:
					{
						iX = (Width - (int)size.Width)/2 - 1;
						iY = iPosition + iSpaceAvailable - (int)size.Height/2;
						break;
					}
					case RulerAlignment.TopOrLeft:
					{
						iX = Width - 2 - (int)size.Width;
						iY = iPosition + iSpaceAvailable - (int)size.Height - 2;
						break;
					}
				}

				drawingPoint = new Point(iX, iY);
			}

			// The drawstring function is common to all operations

			g.DrawString(iValue.ToString(), this.Font, new SolidBrush(this.ForeColor), drawingPoint, format);
		}

		private void Line(Graphics g, int x1, int y1, int x2, int y2)
		{
			g.DrawLine(new Pen(new SolidBrush(this.ForeColor)), x1, y1, x2, y2);
		}

		private int DefaultScale(ScaleMode iScaleMode)
		{
			int iScale = 100;

			// Set scaling
			switch(iScaleMode)
			{
					// Determines the *relative* proportions of each scale
				case ScaleMode.Points:
					iScale = 96;
					break;
				case ScaleMode.Pixels:
					iScale = 100;
					break;
				case ScaleMode.Centimetres:
					iScale = 38;
					break;
				case ScaleMode.Inches:
					iScale = 96;
					break;
			}

			return Convert.ToInt32(iScale * _ZoomFactor);
		}

		private static int DefaultMajorInterval(ScaleMode iScaleMode)
		{
			int iInterval = 10;

			// Set scaling
			switch(iScaleMode)
			{
					// Determines the *relative* proportions of each scale
				case ScaleMode.Points:
					iInterval = 72;
					break;
				case ScaleMode.Pixels:
					iInterval = 100;
					break;
				case ScaleMode.Centimetres:
					iInterval = 1;
					break;
				case ScaleMode.Inches:
					iInterval = 1;
					break;
			}

			return iInterval;
		}

		private int Offset()
		{
			int iOffset;

			switch(this._i3DBorderStyle)
			{
				case Border3DStyle.Flat: iOffset = 0; break;
				case Border3DStyle.Adjust: iOffset = 0; break;
				case Border3DStyle.Sunken: iOffset = 2; break;
				case Border3DStyle.Bump: iOffset = 2; break;
				case Border3DStyle.Etched: iOffset = 2; break;
				case Border3DStyle.Raised: iOffset = 2; break;
				case Border3DStyle.RaisedInner: iOffset = 1; break;
				case Border3DStyle.RaisedOuter: iOffset = 1; break;
				case Border3DStyle.SunkenInner: iOffset = 1; break;
				case Border3DStyle.SunkenOuter: iOffset = 1; break;
				default: iOffset = 0; break;
			}

			return iOffset;
		}

		private int Start()
		{
			int iStart;

			switch(this._i3DBorderStyle)
			{
				case Border3DStyle.Flat: iStart = 0; break;
				case Border3DStyle.Adjust: iStart = 0; break;
				case Border3DStyle.Sunken: iStart = 1; break;
				case Border3DStyle.Bump: iStart = 1; break;
				case Border3DStyle.Etched: iStart = 1; break;
				case Border3DStyle.Raised: iStart = 1; break;
				case Border3DStyle.RaisedInner: iStart = 0; break;
				case Border3DStyle.RaisedOuter: iStart = 0; break;
				case Border3DStyle.SunkenInner: iStart = 0; break;
				case Border3DStyle.SunkenOuter: iStart = 0; break;
				default: iStart = 0; break;
			}
			return iStart;
		}

		private void ChangeMousePosition(int iNewPosition)
		{
		    this._iMousePosition = iNewPosition;
		}

	}

	#endregion

}