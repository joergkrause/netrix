using System;
using System.Drawing;
using GuruComponents.Netrix;
using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.WebEditing.Behaviors;

namespace GuruComponents.Netrix.HelpLine
{
	/// <summary>
	/// Edit designer for HelpLine support.
	/// </summary>
	/// <remarks>
	/// Checks for mouse over cross, moves and draws the
	/// helpline. This designer may work if it is attached as behavior to body and as edit designer
	/// to mshtml site. The host application may switch on/off the behavior but never removes the designer. 
	/// <para>
	/// Additional features available for helplines:
	/// <list type="bullet">
	/// <item>
	/// <term>Snap helpline to grid (default: On)</term>
	/// <description>You can define a (invisible) grid which the helpline snaps into. The grids default distance is 16 pixels.</description>
	/// <term>Snap Elements to helpline (Default: On)</term>
	/// <description>If the control is in 2D (absolute) position mode the elements can be snapped to the line. The magnetic zone is 4 pixels.</description>
	/// <term>Change the Color and Width of the Pen (Default: Blue, width 1 pixel)</term>
	/// <description>You can use a <see cref="System.Drawing.Pen">Pen</see> object to change the style of the lines.</description>
	/// </item>
	/// </list>
	/// The helpline can be moved using the mouse either on the cross (changes x and y coordinates the same time) or on each
	/// line (moves only x or y, respectively). During the move with the cross the mouse pointer becomes a hand and the Ctrl-Key
	/// can be used to modify the behavior.
	/// </para>
	/// <para>
	/// <b>Usage instructions:</b>
	/// </para>
	/// <para>
	/// To use the helpline you must retrieve an instance of that class using the property
    /// <see cref="GuruComponents.Netrix.HelpLine.HelpLine">HelpLine</see>. The returned object can be changed
	/// in any way. After changing you must use the 
    /// <see cref="GuruComponents.Netrix.HelpLine.HelplineCommands.Activate">Activate</see> command to make the lines visible.
    /// The behavior can changed at any time. The object returned from <see cref="GuruComponents.Netrix.HelpLine.HelpLine">HelpLine</see>
	/// is always the same (singleton).
	/// </para>
	/// </remarks>
    public class HelpLineBehavior : BaseBehavior, Interop.IHTMLEditDesigner
    {

        const int CATCHZONE = 2;        // Zone to catch the mouse pointer on the lines

        private static int instanceCounter = 0;
        private int instance;

        private int x, y;               // Position of Helpline
        private bool _lineVisible;      // makes the lines visible
        private bool _lineXEnabled;
        private bool _lineYEnabled;
        private bool _crossEnabled;
        private bool _snapHelpLineX;    // true during helpline x move
        private bool _snapHelpLineY;    // true during helpline y move
        private bool _snapToGrid;      // snaps help line to grid
        private bool _snapelements;     // snaps elements to the help line
        private int _snapgrid;          // snaps helpline to grid measure
        private int snapZone = 12;
        private bool snapOnResize = true;
        private IHtmlEditor _host;
        private System.Windows.Forms.Cursor _oldCursor;
        private Pen _style;

        private Interop.IHTMLElement2 body2;
        private HelpLine extender;

        /// <summary>
        /// This constructor builds a new helpline object with given coordinates.
        /// </summary>
        /// <param name="host">The related control</param>
        /// <param name="helpLine">Properties</param>
        /// <param name="extender">Reference to editor component.</param>
        internal HelpLineBehavior(IHtmlEditor host, HelpLineProperties helpLine, HelpLine extender) : base(host)
	    {
            _host = host;
            _host.ReadyStateComplete += new EventHandler(_host_ReadyStateComplete);
            // store the instances to help the behavior to work on multiple extenders
            instance = instanceCounter++;
            this.extender = extender;
            _snapToGrid = helpLine.SnapToGrid;            
            _snapgrid = helpLine.SnapGrid;
            // assure a valid value, avoid Div by zero
            if (_snapToGrid && _snapgrid == 0) _snapgrid = 16;
            snapZone = helpLine.SnapZone;
            base.HtmlPaintFlag = HtmlPainter.Transparent;
            base.HtmlZOrderFlag = HtmlZOrder.AboveContent;
            _style = new Pen(helpLine.LineColor, (float)helpLine.LineWidth);
            _lineVisible = helpLine.LineVisible;            
            _snapelements = helpLine.SnapElements;
            _lineXEnabled = helpLine.LineXEnabled;
            _lineYEnabled = helpLine.LineYEnabled;
            _crossEnabled = helpLine.CrossEnabled;
            this.x = helpLine.X;
            this.y = helpLine.Y;
        }

        void _host_ReadyStateComplete(object sender, EventArgs e)
        {
            if (_host.DesignModeEnabled)
            {
                body2 = ((Interop.IHTMLElement2)_host.GetBodyElement().GetBaseElement());
            }
        }

        /// <summary>
        /// The name of the behavior. This value is used to distinguesh between behaviors and to remove 
        /// a behavior later without touching other ones.
        /// </summary>
        public override string Name
        {
            get
            {
                return "HelpLine#" + instance.ToString();
            }
        }

        /// <summary>
        /// Pen used to draw both lines.
        /// </summary>
        public Pen PenStyle
        {
            get { return _style; }
            set { _style = value; }
        }

        /// <summary>
        /// Gets or sets the snap zone in which the helpline is magnetic. Defaults to 12 pixel.
        /// </summary>
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
        /// Gets or sets the snap on resize feature. If on the helpline will be magnetic during element resizing.
        /// </summary>
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
        /// Enables the X line. Default is On.
        /// </summary>
        public bool LineXEnabled
        {
            set
            {
                _lineXEnabled = value;
            }
        }

        /// <summary>
        /// Enables the Y line. Default is On.
        /// </summary>
        public bool LineYEnabled
        {
            set
            {
                _lineYEnabled = value;
            }
        }
        
        /// <summary>
        /// Enables the cross sign. Default is On.
        /// </summary>
        public bool CrossEnabled
        {
            set
            {
                _crossEnabled = value;
            }
        }

        /// <summary>
        /// The draw method used to draw to helpline.
        /// </summary>
        /// <remarks>
        /// This method supports the NetRix infrastructure and cannot be called from user code.
        /// </remarks>
        /// <param name="leftBounds"></param>
        /// <param name="topBounds"></param>
        /// <param name="rightBounds"></param>
        /// <param name="bottomBounds"></param>
        /// <param name="gr">The graphics object the line is drawn into.</param>
        protected override void Draw(int leftBounds, int topBounds, int rightBounds, int bottomBounds, Graphics gr)
        {
            if (!_lineVisible) return;
            //if (!_lineVisible)
            //{
            //    try
            //    {
            //        base.Invalidate();
            //    }
            //    catch
            //    {
            //    }
            //    return;
            //}
            gr.PageUnit = GraphicsUnit.Pixel;
            if (_lineXEnabled)
            {
                gr.DrawLine(_style, leftBounds + this.X, topBounds, leftBounds + this.X, bottomBounds);
            }
            if (_lineYEnabled)
            {
                gr.DrawLine(_style, leftBounds, topBounds + this.Y, rightBounds, topBounds + this.Y);
            }
            if (_crossEnabled)
            {
                gr.FillRectangle(new SolidBrush(Color.White), leftBounds + this.X - 2, topBounds + this.Y - 2, 5, 5);
                gr.DrawRectangle(_style, leftBounds + this.X - 3, topBounds + this.Y - 3, 6, 6);
            }
        }

        # region Public Properties

        /// <summary>
        /// Sets the style used to draw the helpline and the border of the cross.
        /// </summary>
        public Pen LineStyle
        {
            set
            {
                _style = value;
            }
            get
            {
                return _style;
            }
        }

        /// <summary>
        /// Make the helpline temporarily invisble.
        /// </summary>
        public bool LineVisible
        {
            set
            {
                _lineVisible = value;                
            }
        }

        /// <summary>
        /// Sets an existing helpline to specific point.
        /// </summary>
        /// <param name="origin"></param>
        public void SetXY(Point origin)
        {
            X = origin.X;
            Y = origin.Y;
        }

        /// <summary>
        /// Gets or sets the current X coordinate.
        /// </summary>
        /// <remarks>
        /// Setting the point outside the area make the helpline invisible. The user cannot
        /// move the helpline anymore. The host application should provide a technique to 
        /// replace the helpline to a visible point.
        /// </remarks>
        public int X
        {
            get
            {
                return x;
            }
            set
            {
                x = value;
                base.Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the current Y coordinate.
        /// </summary>
        /// <remarks>
        /// Setting the point outside the area make the helpline invisible. The user cannot
        /// move the helpline anymore. The host application should provide a technique to 
        /// replace the helpline to a visible point.
        /// </remarks>
        public int Y
        {
            get
            {
                return y;
            }
            set
            {
                y = value;
                base.Invalidate();
            }
        }

        internal void SetY(int y)
        {
            this.y = y;
        }

        internal void SetX(int x)
        {
            this.x = x;
        }

        /// <summary>
        /// Gets or sets the snap function.
        /// </summary>
        /// <remarks>
        /// If the Helpline snap feature is turned on the helpline jumps 
        /// from point to point within a grid. The grid is defined with the
        /// <see cref="SnapGrid">SnapGrid</see> property.
        /// </remarks>
        public bool SnapEnabled
        {
            get
            {
                return _snapToGrid;
            }

            set
            {
                _snapToGrid = value;
            }
        }

        /// <summary>
        /// Let elements snap to the current helpline during move or resize.
        /// </summary>
        public bool SnapElements
        {
            get
            {
                return _snapelements;
            }

            set
            {
                _snapelements = value;
            }
        }

        /// <summary>
        /// Gets or sets the distance in pixels between a virtual grid.
        /// </summary>
        /// <remarks>
        /// The grid is used to snap the helpline to fixed points. It should match
        /// the visible grid, but can be set independently if required.
        /// </remarks>
        public int SnapGrid
        {
            get
            {
                return _snapgrid;
            }

            set
            {
                _snapgrid = value;
            }
        }

        # endregion

        private bool IsInCross(int x, int y)
        {
            if (x > this.X - 4 
                &&
                x < this.X + 4
                &&
                y > this.Y - 4 
                && 
                y < this.Y + 4)
                return true;
            else
                return false;
        }

        private void ResetCurrentHelpPointer()
        {
            System.Windows.Forms.Cursor.Current = this._oldCursor;
            this._host.SetMousePointer(false);
            this._snapHelpLineX = _snapHelpLineY = false;
            this._oldCursor = null;
            extender.OnHelpLineMoved(this._host, new Point(this.x, this.y));
        }

        private void SnapToGrid()
        {
            if (!_snapToGrid)
            {
                return;
            } 
            else
            {
                if (_snapHelpLineX)
                    x = Convert.ToInt32(Math.Ceiling((double)x / _snapgrid)) * _snapgrid - (_snapgrid / 2);
                if (_snapHelpLineY)
                    y = Convert.ToInt32(Math.Ceiling((double)y / _snapgrid)) * _snapgrid - (_snapgrid / 2);
            }
        }

        /// <summary>
        /// Checks id a value is between to boundaries (including the boundary value).
        /// </summary>
        /// <param name="check">Value to check</param>
        /// <param name="min">Lower boundary</param>
        /// <param name="max">Upper boundary</param>
        /// <returns></returns>
        private bool IsBetween(int check, int min, int max)
        {
            if (check <= max && check >= min)
                return true;
            else
                return false;
        }

        #region IHTMLEditDesigner Member

        int Interop.IHTMLEditDesigner.PreHandleEvent(int dispId, GuruComponents.Netrix.ComInterop.Interop.IHTMLEventObj eventObj)
        {
            int result = Interop.S_FALSE;
            // Not ready end immediately 
            if (!this._host.IsReady || eventObj.srcElement == null) return result;
            // If not active drop any handling completely
            if (!this.extender.GetHelpLine(_host).Active) return result;
            //
            if (body2 == null) return result;
            int RealX = eventObj.clientX + body2.GetScrollLeft();
            int RealY = eventObj.clientY + body2.GetScrollTop();
            switch (dispId)
            {
                case DispId.MOUSEUP:
                    // release moving and restore cursor
                    if (this._snapHelpLineX || _snapHelpLineY)
                    {
                        ResetCurrentHelpPointer();
                        result = Interop.S_OK;
                    }
                    break;
                case DispId.MOUSEMOVE:
                    // move helpline with left button down
                    // 1 == left click, 2 == right click
                    if (this._lineVisible && (this._snapHelpLineX || _snapHelpLineY) && eventObj.button == 1)
                    {
                        // check for overlaying scrollbar areas
                        // body has vertical scrollbar; this reduces the active width
                        if (body2.GetClientWidth() > 0 && eventObj.clientX > body2.GetClientWidth() || eventObj.clientX <= 0)
                        {
                            ResetCurrentHelpPointer();
                        }
                        // body has horizontal scrollbar; this reduces the active height
                        if (body2.GetClientHeight() > 0 && eventObj.clientY > body2.GetClientHeight() || eventObj.clientY <= 0)
                        {
                            ResetCurrentHelpPointer();
                        }
                        bool fixX = false, fixY = false;
                        if (eventObj.ctrlKey)
                        {
                            // control key fixes the movement to only one direction, to determine the
                            // direction check the last move and fix this one
                            if ((this.x - RealX) < (this.y - RealY))
                            {
                                fixY = true;
                            } 
                            else 
                            {
                                fixX = true;
                            }
                        }
                        if (!fixX && _snapHelpLineX)
                            this.x = RealX;
                        if (!fixY && _snapHelpLineY)
                            this.y = RealY;
                        // Snap to grid if enabled and no ALT key pressed
                        if (!eventObj.altKey)
                        {
                            SnapToGrid();
                        }
                        // refresh mouse pointer
                        if (_snapHelpLineX && _snapHelpLineY)
                        {
                            this._host.SetMousePointer(true);
                            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Hand;
                        }
                        else
                        {
                            if (_snapHelpLineX)
                            {
                                this._host.SetMousePointer(true);
                                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.SizeWE;
                            }
                            if (_snapHelpLineY)
                            {
                                this._host.SetMousePointer(true);
                                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.SizeNS;
                            }
                        }
                        // fire event to inform host app that new coordinates reached
                        extender.OnHelpLineMoving(this._host, new Point(this.x, this.y));
                        base.Invalidate();
                        result = Interop.S_OK;
                    }                     
                    if (this._lineVisible && !this._snapHelpLineX && !_snapHelpLineY && eventObj.button == 0) 
                    {
                        // check for mouse over cross to inform user that snap is possible
                        if (IsInCross(RealX, RealY))
                        {
                            _oldCursor = System.Windows.Forms.Cursor.Current;
                            this._host.SetMousePointer(true);
                            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Hand;
                            result = Interop.S_OK;
                            break;
                        } 
                        // check for mouse over line to inform user that snap is possible
                        if (IsBetween(RealX, x-CATCHZONE, x+CATCHZONE))
                        {
                            _oldCursor = System.Windows.Forms.Cursor.Current;
                            this._host.SetMousePointer(true);
                            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.SizeWE;
                            result = Interop.S_OK;
                            break;
                        }
                        if (IsBetween(RealY, y-CATCHZONE, y+CATCHZONE))
                        {
                            _oldCursor = System.Windows.Forms.Cursor.Current;
                            this._host.SetMousePointer(true);
                            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.SizeNS;
                            result = Interop.S_OK;
                            break;
                        }
                        if (this._oldCursor != null)
                        {
                            System.Windows.Forms.Cursor.Current = this._oldCursor;
                            this._host.SetMousePointer(false);
                            this._oldCursor = null;
                            result = Interop.S_FALSE;
                        }
                    }
                    break;
                case DispId.MOUSEDOWN:
                    // mouse goes down, check if it is on helpline cross, then start moving
                    if (IsInCross(RealX, RealY) && !this._snapHelpLineX && !_snapHelpLineY && eventObj.button == 1)
                    {
                        this._snapHelpLineX = _snapHelpLineY = true;
                        result = Interop.S_OK;
                        break;
                    }
                    if (IsBetween(RealX, x-CATCHZONE, x+CATCHZONE))
                    {
                        this._snapHelpLineX = true;
                        result = Interop.S_OK;
                        break;
                    }
                    if (IsBetween(RealY, y-CATCHZONE, y+CATCHZONE))
                    {
                        this._snapHelpLineY = true;
                        result = Interop.S_OK;
                        break;
                    }
                    break;
            }
            return result;
        }

        int Interop.IHTMLEditDesigner.PostHandleEvent(int dispId, GuruComponents.Netrix.ComInterop.Interop.IHTMLEventObj eventObj)
        {
            return Interop.S_FALSE;
        }

        int Interop.IHTMLEditDesigner.TranslateAccelerator(int dispId, GuruComponents.Netrix.ComInterop.Interop.IHTMLEventObj eventObj)
        {
            return Interop.S_FALSE;
        }

        int Interop.IHTMLEditDesigner.PostEditorEventNotify(int dispId, GuruComponents.Netrix.ComInterop.Interop.IHTMLEventObj eventObj)
        {
            return Interop.S_FALSE;
        }

        #endregion

    }
}
