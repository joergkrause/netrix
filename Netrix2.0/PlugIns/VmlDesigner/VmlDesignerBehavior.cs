using System;
using System.Collections;
using System.Drawing;
using System.Globalization;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using GuruComponents.Netrix.ComInterop;
using Comzept.Genesis.NetRix.VgxDraw;
using GuruComponents.Netrix.VmlDesigner.Behaviors;
using GuruComponents.Netrix.VmlDesigner.DataTypes;

namespace GuruComponents.Netrix.VmlDesigner
{

	public class VmlDesignerBehavior : Interop.IHTMLEditDesigner
	{

		/// <summary>
		/// Descripes the usuable contactpoints at the choosen element
		/// </summary>
		private enum eContactPoint{
			none ,              // no ContactPoint
			top,
			left,
			bottom,
			right,
			topleft,
			topright,
			bottomleft,
			bottomrigth,
			all,
			from,
			to,
            nth,                // used by polylines the nth-point is selected
            rotate,             // used by all rotatable the rorator is selected
            ctrl1,              // used by curves and arcs control1 is selected
            ctrl2               // used by curves and arcs control2 is selected
		}

        /// <summary>
        /// Stores the artibutes of an element
        /// </summary>
        private class Properties
        {

            public Point topleft;
            public Point topright;
            public Point topmiddle;
            public Point bottommiddle;
            public Point bottomleft;
            public Point bottomright;
            public Point middleleft;
            public Point middleright;
            public Point center;
            public Point rotator;
            public Point ctrl1;
            public Point ctrl2;
            public double angle;
            public double startAngle;
            public double endAngle;
            public int top;
            public int left;
            public int width;
            public int height;
            public int right;
            public int bottom;
        }

		
		const int CATCHZONE = 4;        // Zone to catch the mouse pointer on the lines

		private double x;
		private double y;
		private Interop.IHTMLElement2 body2;
		private VmlDesigner extender;
		private IHtmlEditor _htmlEditor;
		private bool _active;
        private bool _insertMode;
		private bool _snapenabled;      // snaps handles to grid
		private int _snapgrid;          // snap handles grid measure
		private bool _elementEvents;

		private Cursor prevCursor;
		
		// Documents world in Pixel
		private Point _Mouse = new Point (-1,-1) ;
		private Point _PrevMouse = new Point (-1,-1) ;
        private Point _TransMouse = new Point (-1,-1) ;
        private Point _PrevTransMouse = new Point (-1,-1) ;
        private Properties _origStyleProperties = new Properties() ;
		
		// for all shapes (use an enum)        
        private eContactPoint movedContactPoint = eContactPoint.none ;  

        private bool isMouseDown = false;       // indicates that a mouse-button is pressed
		private bool isMouseMove = false ;      // indicates that the mouse is curently moving

        private bool isMouseOutSide = false ;   // if the mouse is outside

   	
        internal static CultureInfo DefaultCulture;
        private static float dpiX;
        private static float dpiY;
        private Cursor RotateCursor;

        private bool _canRotate;

		/// <summary>
		/// This constructor builds a new VmlDesignerBehavior
		/// </summary>
		/// <param name="host"></param>
		/// <param name="vmlDesigner"></param>
		/// <param name="Extender"></param>
		internal VmlDesignerBehavior(IHtmlEditor host, VmlDesignerProperties vmlDesigner, VmlDesigner Extender)
		{
			_htmlEditor = host;
			this.extender = Extender;
			_active = vmlDesigner.Active;
            _insertMode = false;
            _snapenabled = vmlDesigner.SnapEnabled;
			_elementEvents = vmlDesigner.ElementEvents;			
            _snapgrid = vmlDesigner.SnapGrid;
            _canRotate = vmlDesigner.CanRotate;
            prevCursor = Cursors.Default;
            DefaultCulture = new CultureInfo("en-US");  // used to recognize single/double
            this._htmlEditor.ReadyStateComplete += new EventHandler(_htmlEditor_ReadyStateComplete);
            ((Control)this._htmlEditor).MouseLeave +=new EventHandler(VmlDesignerBehavior_MouseLeave);
            ((Control)this._htmlEditor).MouseEnter +=new EventHandler(VmlDesignerBehavior_MouseEnter);

            Graphics gr = Graphics.FromHwnd(((Control) this._htmlEditor).Handle);
            dpiX = gr.DpiX;
            dpiY = gr.DpiY;                                  
            RotateCursor = new Cursor(this.GetType().Assembly.GetManifestResourceStream("GuruComponents.Netrix.VmlDesigner.Resources.Rotate.cur"));
		}

		public bool Active{
			get{ return _active; }
			set{ _active = value; }
		}

        public bool InsertMode
        {
            get{ return _insertMode; }
            set{ _insertMode = value; }
        }

		/// <summary>
		/// Gets or sets the distance in pixels between the grid.
		/// </summary>
		/// <remarks>
		/// The grid is used to snap the helpline to fixed points.
		/// </remarks>
		public int SnapGrid
        {
			get{ return _snapgrid; }
			set{ _snapgrid = value; }
		}

		/// <summary>
		/// Gets or sets the snap function.
		/// </summary>
		/// <remarks>
		/// If the Helpline snap feature is turned on the helpline jumps 
		/// from point to point within a grid. The grid is defined with the
		/// <see cref="GuruComponents.Netrix.WebEditing.Behaviors.HelpLine.HelpLine.SnapGrid">SnapGrid</see> property.
		/// </remarks>
		public bool SnapEnabled{
			get{ return _snapenabled; }
			set{ _snapenabled = value; }
		}

		/// <summary>
		/// Gets or sets the current X coordinate.
		/// </summary>
		/// <remarks>
		/// Setting the point outside the area make the helpline invisible. The user cannot
		/// move the helpline anymore. The host application should provide a technique to 
		/// replace the helpline to a visible point.
		/// </remarks>
		private double RealX{
			get{ return x; }
			set{ x = value; }
		}

		/// <summary>
		/// Gets or sets the current Y coordinate.
		/// </summary>
		/// <remarks>
		/// Setting the point outside the area make the helpline invisible. The user cannot
		/// move the helpline anymore. The host application should provide a technique to 
		/// replace the helpline to a visible point.
		/// </remarks>
		private double RealY{
			get{ return y;}
			set{ y = value;}
		}


		private void ResetCurrentPointer()
		{
			Cursor.Current = this.prevCursor;
			this._htmlEditor.SetMousePointer(false);
		}

		private void SnapToGrid()
		{
			if (_snapenabled)
			{
				x = Math.Max(0, Convert.ToInt32(Math.Ceiling(x / _snapgrid) - 1) * _snapgrid + (_snapgrid / 2));
				y = Math.Max(0, Convert.ToInt32(Math.Ceiling(y / _snapgrid) - 1) * _snapgrid + (_snapgrid / 2));
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

		private object GetAttribute(Interop.IHTMLElement element, string attribute, bool ignoreCase)
		{
			if (element == null)
			{
				return null;
			}
			object[] locals1 = new object[1];
			try
			{
				element.GetAttribute(attribute, !ignoreCase ? 1 : 0, locals1);
				object local = locals1[0];
				return local;
			}
			catch
			{
				return null;
			}
		}

		/// <summary>
		/// This method recognizes a VML element and sets all possible handle areas.
		/// </summary>
		/// <param name="e"></param>
		/// <returns></returns>
		private Interop.IHTMLElement GetVmlElement(Interop.IHTMLEventObj e)
		{
			if (e.srcElement != null && ((Interop.IHTMLElement2) e.srcElement).GetScopeName().Equals("v"))
			{
				return e.srcElement;
			}
			return null;
		}

		#region Helper Methods

        /// <summary>
        /// Checks if the point m is on the courve a to d controld by b and c 
        /// using the DeCasteljau Subdivision / DeCasteljau Algorithm
        /// </summary>
        /// <param name="m">mouse point</param>
        /// <param name="a">start</param>
        /// <param name="d">end</param>
        /// <param name="b">control1</param>
        /// <param name="c">control2</param>
        /// <returns></returns>
        private bool IsOnCurve(Point m,Point a,Point d,Point b,Point c){
            // TODO: should be improved 
            Point dest=new Point(0,0),ab,bc,cd,abbc,bccd ;
            for (float t=0;t<1;t=t+0.01f){
                ab = lerp (a,b,t);           // point between a and b (green)
                bc = lerp (b,c,t);           // point between b and c (green)
                cd = lerp (c,d,t);           // point between c and d (green)
                abbc = lerp (ab,bc,t);       // point between ab and bc (blue)
                bccd = lerp (bc,cd,t);       // point between bc and cd (blue)
                dest = lerp (abbc,bccd,t);   // point on the bezier-curve (black)
                if (dest.X - 0.01 <= m.X && dest.X + 0.01 >= m.X){
                    break ;
                }
            }
            return (dest.Y - 10 <= m.Y && dest.Y + 10 >= m.Y);
        }

        /// <summary>
        /// simple linear interpolation between two points
        /// </summary>
        /// <param name="a">first point</param>
        /// <param name="b">second point</param>
        /// <param name="t">position</param>
        /// <returns></returns>
        private Point lerp(Point a,Point b, float t){        
           return new Point (Convert.ToInt32(a.X + (b.X-a.X)*t),Convert.ToInt32(a.Y + (b.Y-a.Y)*t)) ;
        }

		/// <summary>
		/// Checks if the mouse pointer is on the line
		/// </summary>
		/// <param name="x">mouse x</param>
		/// <param name="y">mouse y</param>
		/// <param name="x1">line from x</param>
		/// <param name="y1">line from y</param>
		/// <param name="x2">line to x</param>
		/// <param name="y2">line to y</param>
		/// <returns></returns>
		private bool IsOnLine (double x, double y ,double x1, double y1, double x2, double y2)
		{
			// wird gebraucht, weil genau ein Pixel zu schwer zu treffen ist ;-)
			int treshold = 3;
			
			//System.Console.Out.WriteLine("IsOnLine({0} | {1} | {2} | {3} | {4} |{5})",x,y,x1,y1,x2,y2);
			
			// Liegt der Punkt im X Bereich
			// nicht unbedingt nötig, da IHTMLEdit das schon prüft !
			// diser Vergleicht ging bei senkrechten Linien schief, weil die X Komponente nicht auf 0 ist ?!
			// das gleiche gilt für den Y Bereich, wird auch von IHtmlEdit geprüft
			/*
			 * 
			if (!((x1-treshold < x && x < x2+treshold) || (x2-treshold < x && x < x1+treshold))){
				System.Console.Out.WriteLine("IsOnLine() -> false (1)");
				return false ;
			}*/
			 
			// Sonderfall --> Senkrecht ?
			if (x1 == x2){
				if ((y1-treshold < y && y < y2+treshold) || (y2-treshold < y && y < y1+treshold)){
					//System.Console.Out.WriteLine("IsOnLine() -> true ");
					return true ;
				}else{
					//System.Console.Out.WriteLine("IsOnLine() -> false ");
					return false ;
				}
			}
		    // Liegt der Punkt auf der Geraden
		    double stolpe = (y2-y1)/(x2-x1) ;
		    double dist = (Math.Abs(stolpe) <= 1) ? (stolpe)*(x-x1) - (y-y1) : (y-y1)/(stolpe) - (x-x1) ;
			if(-treshold < dist && dist < treshold){
				//System.Console.Out.WriteLine("IsOnLine() -> true {0}",dist);
				return true ;
			}
			//System.Console.Out.WriteLine("IsOnLine() -> false {0}",dist);
			return false ;
		}

        private bool IsOnRect(double x, double y ,double x1, double y1, double x2, double y2)
        {
            // TODO: Calculate the real drag area, like the line (not the rectangle around the line, etc.)
            double val = ( (y2-y1)*x + (y1*x2 - x1*y2) ) / (x2-x1) - y;
            if(val <= CATCHZONE || val >= -CATCHZONE)
                return true;
            else
                return false;
        }

        private bool IsOnRect(double x, double y, int left, int top, int width, int height) {
            int x1 = left;
            int y1 = top;
            int x2 = left + width;
            int y2 = top + height;
            // TODO: Calculate the real drag area, like the line (not the rectangle around the line, etc.)
            if (((y2 - y1) * x + (y1 * x2 - x1 * y2)) > 0) {
                int val = Convert.ToInt32(((y2 - y1) * x + (y1 * x2 - x1 * y2)) / (x2 - x1) - y);
                if (val <= CATCHZONE || val >= -CATCHZONE)
                    return true;
                else
                    return false;
            } else {
                return false;
            }
        }

        private bool IsInZone(double x, double y, double px, double py) {
            if((x+CATCHZONE >= px && x-CATCHZONE <= px) &&
                (y+CATCHZONE >= py && y-CATCHZONE <= py)) {
                return true;
            } 
            else {
                return false;
            }
        }

        private bool IsInZone(double x, double y) {
            return IsInZone(x,y,RealX,RealY);
        }

        /// <summary>
        /// Checks if the pointer is in the CATCHZONE of the pointer
        /// </summary>
        /// <param name="corner">the corner</param>
        /// <param name="pointer">the pointer</param>
        /// <returns></returns>
        private bool IsInZone(Point corner,Point pointer){
            return IsInZone(corner.X,corner.Y,pointer.X,pointer.Y);
        }
        
        private bool IsBetween(double check, int min, int max)
		{
			if (check <= max && check >= min)
				return true;
			else
				return false;
		}

		public object GetAttribute(Interop.IHTMLElement element, string attribute)
		{
			object local2;
			try
			{
				object[] locals = new object[1];
				locals[0] = null;
				element.GetAttribute(attribute, 0, locals);
				object local1 = locals[0];
				if (local1 is DBNull)
				{
					local1 = null;
				}
				local2 = local1;
			}
			catch
			{
				local2 = null;
			}
			return local2;
		}

		public void SetAttribute(Interop.IHTMLElement element, string attribute, object value)
		{
			try
			{
				element.SetAttribute(attribute, value, 0);
			}
			catch (Exception e)
			{
				Console.Out.WriteLine(e.Message);
			}
		}

        public void SetStyleAttribute(Interop.IHTMLElement element, string styleName, string styleText)
        {
            Interop.IHTMLStyle style = (Interop.IHTMLStyle) element.GetStyle();
            style.SetAttribute(styleName.Replace("-", String.Empty), styleText, 0);
        }

        public string GetStyleAttribute(Interop.IHTMLElement element, string styleName)
        {    
            Interop.IHTMLStyle style = (Interop.IHTMLStyle) element.GetStyle();
            object o = style.GetAttribute(styleName.Replace("-", String.Empty), 0);
            if (o == null)
            {
                return String.Empty;
            }
            else 
            {
                string styleText = o.ToString();
                return styleText;
            }
        }

		# endregion

        /// <summary>
        /// This var save the last edited element
        /// </summary>
        private Interop.IHTMLElement lastVmlElement = null ;
        private int whichContactPoint ;

        /// <summary>
        /// Loop recursivly through parent elements and collect offsets. Needed to operate with
        /// nested tables correctly. This property returns the left offset.
        /// </summary>
        private int GetParentOffsetLeft(Interop.IHTMLElement ce) {
            int po = 0;
            while (ce.GetOffsetParent() != null) {
                po += ((Interop.IHTMLElement) ce).GetOffsetParent().GetOffsetLeft();
                ce = ce.GetOffsetParent();
            }
            return po;
        }
            
        /// <summary>
        /// Loop recursivly through parent elements and collect offsets. Needed to operate with
        /// nested tables correctly. This property returns the top offset. 
        /// </summary>
        private int GetParentOffsetTop(Interop.IHTMLElement ce)
        {
            int po = 0;
            while (ce.GetOffsetParent() != null)
            {
                po += ((Interop.IHTMLElement) ce).GetOffsetParent().GetOffsetTop();
                ce = ce.GetOffsetParent();
            }
            return po;
        }

        /// <summary>
        /// Loop recursivly through parent elements and collect offsets. Needed to operate with
        /// nested tables correctly. This property returns the left offset.
        /// </summary>
        private int GetParentOffsetWidth(Interop.IHTMLElement ce) {
            int po = 0;
            while (ce.GetOffsetParent() != null) {
                po += ((Interop.IHTMLElement) ce).GetOffsetParent().GetOffsetWidth();
                ce = ce.GetOffsetParent();
            }
            return po;
        }
            
        /// <summary>
        /// Loop recursivly through parent elements and collect offsets. Needed to operate with
        /// nested tables correctly. This property returns the top offset. 
        /// </summary>
        private int GetParentOffsetHeight(Interop.IHTMLElement ce) {
            int po = 0;
            while (ce.GetOffsetParent() != null) {
                po += ((Interop.IHTMLElement) ce).GetOffsetParent().GetOffsetHeight();
                ce = ce.GetOffsetParent();
            }
            return po;
        }
        /// <summary>
        /// Left offset of table including any subsequent offset for nested tables
        /// </summary>
        private int GetLeftOffset(Interop.IHTMLElement el)
        {
            return el.GetOffsetLeft() + GetParentOffsetLeft(el);
        }

        /// <summary>
        /// Top offset of table including any subsequent offset for nested tables
        /// </summary>
        private int GetTopOffset(Interop.IHTMLElement el)
        {
            return el.GetOffsetTop() + GetParentOffsetTop(el);
        }

        /// <summary>
        /// Expects rotation values with the suffix "fd" (fractional degree), "deg" or "rad" as well as "°" sign.
        /// </summary>
        /// <param name="rot"></param>
        /// <returns></returns>
        internal static double RotationParse(string rot)
        {
            if (rot.EndsWith("fd"))
            {
                string fd = rot.Substring(0, rot.Length - "fd".Length);
                return 65536 / Int32.Parse(fd); // 1
            }
            if (rot.EndsWith("deg"))
            {
                string deg = rot.Substring(0, rot.Length - "deg".Length);
                return Int32.Parse(deg);        // 65536
            }
            if (rot.EndsWith("rad"))
            {
                string rad = rot.Substring(0, rot.Length - "rad".Length);
                return (Math.PI * 180) / Int32.Parse(rad); // 65536/180
            }
            if (rot.EndsWith("°"))
            {
                string grd = rot.Substring(0, rot.Length - "°".Length);
                return Int32.Parse(grd);        // 65536
            }
            return Unit.Parse(rot, DefaultCulture).Value;
        }

        /// <summary>
        /// Converts any possible string representation into pixel.
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        internal static int UnitParse(string attribute, bool x)
        {
            if (!attribute.Equals(String.Empty) && !attribute.Equals("auto"))
            {
                Unit unit = Unit.Parse(attribute, DefaultCulture);
                float dpi = (x) ? dpiX : dpiY;
                switch (unit.Type)
                {
                    case UnitType.Pixel:
                        return Convert.ToInt32(unit.Value);
                    case UnitType.Point:
                        return Convert.ToInt32(unit.Value);             // TODO:
                    case UnitType.Inch:
                        return Convert.ToInt32(unit.Value * dpi);
                    case UnitType.Pica:
                        return Convert.ToInt32(unit.Value);             // TODO:
                    case UnitType.Percentage:
                        return Convert.ToInt32(unit.Value);             // TODO:
                    case UnitType.Mm:
                        return Convert.ToInt32(unit.Value * dpi / 25.4);             // TODO:
                    case UnitType.Cm:
                        return Convert.ToInt32(unit.Value * dpi / 2.54);
                    case UnitType.Em:
                        return Convert.ToInt32(unit.Value);             // TODO:
                    case UnitType.Ex:
                        return Convert.ToInt32(unit.Value);             // TODO:
                }
            }
            return 0;
        }

        /// <summary>
        /// Width offset of table including any subsequent offset for nested tables
        /// </summary>
        private int GetWidthOffset(Interop.IHTMLElement el) 
        {
            return el.GetOffsetWidth() + GetParentOffsetWidth(el);
        }

        /// <summary>
        /// Height offset of table including any subsequent offset for nested tables
        /// </summary>
        private int GetHeightOffset(Interop.IHTMLElement el) {
            return el.GetOffsetHeight() + GetParentOffsetHeight(el);
        }

        int Interop.IHTMLEditDesigner.PreHandleEvent(int dispId, Interop.IHTMLEventObj eventObj)
        {
            Interop.IHTMLElement element;
            int result = Interop.S_FALSE;

            if (!this._htmlEditor.IsReady || !Active)
            {
                return result;
            }

            // für diese 2 lassen sich keine Konstanten finden. 
            // um ruckelfrei zu werden müssen beide entsprechend beachtet werden ...
            // einer kommt, wenn sich die offsets ändern weil die mouse den Bereich um das Object
            // unterschritten hat, die andere scheint auf eine allgemeine Änderung der Mouse außerhalb des
            // Bereichs zu reagieren, es handelt sich dabei warscheinlich um mouse hat object fokus verlassen
            if (dispId == -2147418104 || dispId == -2147418103 || this.isMouseOutSide)
            {
                return result;
            }

            if (InsertMode)
            {
                // if the user is inserting something,
                // we need to kill the events.
                return Interop.S_OK;
            }

            //System.Diagnostics.Debug.WriteLine(dispId.ToString() +"  "+this.GetVmlElement(eventObj));

            if (eventObj.srcElement == null)
            {
                return result;
            }
            else
            {
                body2 =
                    ((Interop.IHTMLDocument2) ((Interop.IHTMLElement) eventObj.srcElement).GetDocument()).GetBody() as
                    Interop.IHTMLElement2;
            }
            // evaluate what the mouse is currently doing
            if (eventObj.button == 1)
            {
                switch (dispId)
                {
                    case DispId.MOUSEMOVE:
                        this.isMouseMove = true;
                        break;
                    case DispId.MOUSEDOWN:
                        this.isMouseDown = true;
                        break;
                    case DispId.MOUSEUP:
                        // reset everything need for the move-detection
                        this.isMouseDown = false;
                        this.isMouseMove = false;
                        this.movedContactPoint = eContactPoint.none;
                        _PrevMouse.X = -1;
                        _PrevMouse.Y = -1;
                        _PrevTransMouse.X = -1;
                        _PrevTransMouse.Y = -1;
                        ResetCurrentPointer();
                        break;
                }
            }
            else
            {
                // the mouse is not pressed any more
                this.isMouseDown = false;
                this.movedContactPoint = eContactPoint.none;
                _PrevMouse.X = -1;
                _PrevMouse.Y = -1;
                _PrevTransMouse.X = -1;
                _PrevTransMouse.Y = -1;
                ResetCurrentPointer();
            }

            //System.Diagnostics.Debug.WriteLineIf(eventObj.srcElement != null, eventObj.type, eventObj.srcElement.GetTagName());
            // we're in size mode or one element is selected, but the pointer has left the element, 
            // so we need to access the last element, cause the emement has not changed
            if ((this.movedContactPoint != eContactPoint.none) || (this.lastVmlElement != null))
            {
                element = lastVmlElement;
            }
            else
            {
                element = this.GetVmlElement(eventObj);
            }

            if (element == null)
            {
                if (_active)
                {
                    this.ResetCurrentPointer();
                }
                return result;
            }


            //int bl = ((Interop.IHTMLElement)body2).GetOffsetLeft();
            //int bt = ((Interop.IHTMLElement)body2).GetOffsetTop();

            string tmp = (string) ((Interop.IHtmlBodyElement) body2).leftMargin;
            int leftMargin = Convert.ToInt32((tmp.Equals(String.Empty)) ? 0 : UnitParse(tmp, true));
            tmp = (string) ((Interop.IHtmlBodyElement) body2).topMargin;
            int topMargin = Convert.ToInt32((tmp.Equals(String.Empty)) ? 0 : UnitParse(tmp, false));

            int styleLeft = ((Interop.IHTMLStyle) ((Interop.IHTMLElement) element).GetStyle()).GetPixelLeft();
            int styleTop = ((Interop.IHTMLStyle) ((Interop.IHTMLElement) element).GetStyle()).GetPixelTop();

            int left = UnitParse(GetStyleAttribute(element, "left"), true);
            int top = UnitParse(GetStyleAttribute(element, "top"), false);
            int width = UnitParse(GetStyleAttribute(element, "width"), true);
            int height = UnitParse(GetStyleAttribute(element, "height"), false);
            int right = left + width;
            int bottom = top + height;
            bool flipx = ((GetStyleAttribute(element, "flip")).ToLower().IndexOf("x") != -1);
            bool flipy = ((GetStyleAttribute(element, "flip")).ToLower().IndexOf("y") != -1);
            double angle =
                Convert.ToDouble((GetStyleAttribute(element, "rotation").Equals(String.Empty))
                                     ? 0
                                     : RotationParse(GetStyleAttribute(element, "rotation")));

            //  int offsetL = element.GetOffsetLeft();
            //  int offsetT = element.GetOffsetTop();

            // has to be calulated by our self
            // TODO: Optimize calulation here ;-)
            int offsetL = left;
            int offsetT = top;
            int scrollLeft = body2.GetScrollLeft();
            int scrollTop = body2.GetScrollTop();

            // Calculate the real position of the pointer including scroll dependencies in the world of the 
            // elements coordinate system
            RealX = (eventObj.clientX - offsetL + scrollLeft);
            RealY = (eventObj.clientY - offsetT + scrollTop);

            //// Calculate the real mouse position in the documents world
            _Mouse.X = (eventObj.clientX + scrollLeft);
            _Mouse.Y = (eventObj.clientY + scrollTop);

            //                if (dispId == DispId.MOUSEDOWN || (dispId == DispId.MOUSEMOVE)) {
            //                    System.Console.Out.WriteLine("------------------------------------------------");
            //                    System.Console.Out.WriteLine(element.GetTagName());
            //                    System.Console.Out.WriteLine("ElementOffset LFT {0} TOP {1}",offsetL,offsetT);
            //                    System.Console.Out.WriteLine("Mouse         LFT {0} TOP {1}",RealX,RealY);
            //                    System.Console.Out.WriteLine("BodyOffset    LFT {0} TOP {1}",bl,bt);
            //                    System.Console.Out.WriteLine("ScrollOffset  LFT {0} TOP {1}",sl,st);
            //                    System.Console.Out.WriteLine("Cumulative    LFT {0} TOP {1}",this.GetLeftOffset(element),this.GetTopOffset(element));
            //                    System.Console.Out.WriteLine("Parents       LFT {0} TOP {1}",this.GetParentOffsetLeft(element),this.GetParentOffsetTop(element));
            //                    System.Console.Out.WriteLine("------------------------------------------------");
            //                }

            // decide what to do dependent on the vml-element
            switch (element.GetTagName().ToLower())
            {
                    #region line

                case "line":

                    VgVector2D f = new VgVector2D((IVgVector2D) GetAttribute(element, "from"));
                    VgVector2D t = new VgVector2D((IVgVector2D) GetAttribute(element, "to"));

                    if (IsInZone(f.Pixel.X, f.Pixel.Y) || (this.movedContactPoint == eContactPoint.from))
                    {
                        if (Cursor.Current != Cursors.Hand)
                        {
                            this.prevCursor = Cursor.Current;
                            Cursor.Current = Cursors.Hand;
                        }

                        if (this.isMouseDown == true && (this.movedContactPoint == eContactPoint.none)
                            /*&& this.isMouseMove == true*/) //this.isElementSelected == true && isMouseDrag == true)
                        {
                            this.movedContactPoint = eContactPoint.from;
                            lastVmlElement = element;
                            if (_PrevMouse.X == -1) _PrevMouse.X = _Mouse.X;
                            if (_PrevMouse.Y == -1) _PrevMouse.Y = _Mouse.Y;
                        }
                        result = Interop.S_OK;
                    }
                    else if (IsInZone(t.Pixel.X, t.Pixel.Y) || (this.movedContactPoint == eContactPoint.to))
                    {
                        if (Cursor.Current != Cursors.Hand)
                        {
                            this.prevCursor = Cursor.Current;
                            Cursor.Current = Cursors.Hand;
                        }
                        if (this.isMouseDown == true && (this.movedContactPoint == eContactPoint.none)
                            /*&& this.isMouseMove == true*/) //this.isElementSelected == true && isMouseDrag == true)
                        {
                            this.movedContactPoint = eContactPoint.to;
                            lastVmlElement = element;
                            if (_PrevMouse.X == -1) _PrevMouse.X = _Mouse.X;
                            if (_PrevMouse.Y == -1) _PrevMouse.Y = _Mouse.Y;
                        }
                        result = Interop.S_OK;
                    }
                    else if (IsOnLine(RealX, RealY, f.Pixel.X, f.Pixel.Y, t.Pixel.X, t.Pixel.Y) ||
                             (this.movedContactPoint == eContactPoint.all))
                    {
                        if (this.movedContactPoint != eContactPoint.all)
                        {
                            this.prevCursor = Cursor.Current;
                            Cursor.Current = Cursors.SizeAll;
                        }
                        if (this.isMouseDown == true /*&& this.isMouseMove == true*/)
                            //this.isElementSelected == true && isMouseDrag == true)
                        {
                            this.movedContactPoint = eContactPoint.all;
                            lastVmlElement = element;
                            if (_PrevMouse.X == -1) _PrevMouse.X = _Mouse.X;
                            if (_PrevMouse.Y == -1) _PrevMouse.Y = _Mouse.Y;
                        }
                        result = Interop.S_OK;
                    }
                    else if (IsOnRect(RealX, RealY, f.Pixel.X, f.Pixel.Y, t.Pixel.X, t.Pixel.Y))
                    {
                        this.ResetCurrentPointer();
                        result = Interop.S_OK;
                        eventObj.cancelBubble = true;
                        eventObj.returnValue = false;
                    }
                    else
                    {
                        if (Cursor.Current == Cursors.Hand)
                        {
                            this.ResetCurrentPointer();
                        }
                    }
                    if (this.movedContactPoint == eContactPoint.from)
                    {
                        SnapToGrid();
                        f.Pixel =
                            new Point(f.Pixel.X + (_Mouse.X - _PrevMouse.X), f.Pixel.Y + (_Mouse.Y - _PrevMouse.Y));
                        _PrevMouse.X = _Mouse.X;
                        _PrevMouse.Y = _Mouse.Y;
                        result = Interop.S_OK;
                        break;
                    }
                    else if (this.movedContactPoint == eContactPoint.to)
                    {
                        SnapToGrid();
                        t.Pixel =
                            new Point(t.Pixel.X + (_Mouse.X - _PrevMouse.X), t.Pixel.Y + (_Mouse.Y - _PrevMouse.Y));
                        _PrevMouse.X = _Mouse.X;
                        _PrevMouse.Y = _Mouse.Y;
                        result = Interop.S_OK;
                        break;
                    }
                    else if (this.movedContactPoint == eContactPoint.all && _PrevMouse.X != -1 && _PrevMouse.Y != -1)
                    {
                        SnapToGrid();
                        f.Pixel =
                            new Point(f.Pixel.X + (_Mouse.X - _PrevMouse.X), f.Pixel.Y + (_Mouse.Y - _PrevMouse.Y));
                        t.Pixel =
                            new Point(t.Pixel.X + (_Mouse.X - _PrevMouse.X), t.Pixel.Y + (_Mouse.Y - _PrevMouse.Y));
                        _PrevMouse.X = _Mouse.X;
                        _PrevMouse.Y = _Mouse.Y;
                        result = Interop.S_OK;
                    }
                    break;

                    #endregion

                    #region curve

                case "curve":
                    VgVector2D c1 = new VgVector2D((IVgVector2D) GetAttribute(element, "control1"));
                    VgVector2D c2 = new VgVector2D((IVgVector2D) GetAttribute(element, "control2"));
                    VgVector2D from = new VgVector2D((IVgVector2D) GetAttribute(element, "from"));
                    VgVector2D to = new VgVector2D((IVgVector2D) GetAttribute(element, "to"));

                    if (IsInZone(c1.Pixel.X, c1.Pixel.Y) || (this.movedContactPoint == eContactPoint.ctrl1))
                    {
                        if (Cursor.Current != Cursors.Hand)
                        {
                            this.prevCursor = Cursor.Current;
                            Cursor.Current = Cursors.Hand;
                        }

                        if (this.isMouseDown == true && (this.movedContactPoint == eContactPoint.none)
                            /*&& this.isMouseMove == true*/)
                        {
//this.isElementSelected == true && isMouseDrag == true)
                            this.movedContactPoint = eContactPoint.ctrl1;
                            lastVmlElement = element;
                            if (_PrevMouse.X == -1) _PrevMouse.X = _Mouse.X;
                            if (_PrevMouse.Y == -1) _PrevMouse.Y = _Mouse.Y;
                            result = Interop.S_OK;
                        }
                    }
                    else if (IsInZone(c2.Pixel.X, c2.Pixel.Y) || (this.movedContactPoint == eContactPoint.ctrl2))
                    {
                        if (Cursor.Current != Cursors.Hand)
                        {
                            this.prevCursor = Cursor.Current;
                            Cursor.Current = Cursors.Hand;
                        }
                        if (this.isMouseDown == true && (this.movedContactPoint == eContactPoint.none)
                            /*&& this.isMouseMove == true*/)
                        {
//this.isElementSelected == true && isMouseDrag == true)
                            this.movedContactPoint = eContactPoint.ctrl2;
                            lastVmlElement = element;
                            if (_PrevMouse.X == -1) _PrevMouse.X = _Mouse.X;
                            if (_PrevMouse.Y == -1) _PrevMouse.Y = _Mouse.Y;
                            result = Interop.S_OK;
                        }
                    }
                    else if (IsInZone(from.Pixel.X, from.Pixel.Y) || (this.movedContactPoint == eContactPoint.from))
                    {
                        if (Cursor.Current != Cursors.Hand)
                        {
                            this.prevCursor = Cursor.Current;
                            Cursor.Current = Cursors.Hand;
                        }

                        if (this.isMouseDown == true && (this.movedContactPoint == eContactPoint.none)
                            /*&& this.isMouseMove == true*/)
                        {
//this.isElementSelected == true && isMouseDrag == true)
                            this.movedContactPoint = eContactPoint.from;
                            lastVmlElement = element;
                            if (_PrevMouse.X == -1) _PrevMouse.X = _Mouse.X;
                            if (_PrevMouse.Y == -1) _PrevMouse.Y = _Mouse.Y;
                            result = Interop.S_OK;
                        }
                    }
                    else if (IsInZone(to.Pixel.X, to.Pixel.Y) || (this.movedContactPoint == eContactPoint.to))
                    {
                        if (Cursor.Current != Cursors.Hand)
                        {
                            this.prevCursor = Cursor.Current;
                            Cursor.Current = Cursors.Hand;
                        }
                        if (this.isMouseDown == true && (this.movedContactPoint == eContactPoint.none)
                            /*&& this.isMouseMove == true*/)
                        {
//this.isElementSelected == true && isMouseDrag == true)
                            this.movedContactPoint = eContactPoint.to;
                            lastVmlElement = element;
                            if (_PrevMouse.X == -1) _PrevMouse.X = _Mouse.X;
                            if (_PrevMouse.Y == -1) _PrevMouse.Y = _Mouse.Y;
                            result = Interop.S_OK;
                        }
                    }
                    else if (IsOnCurve(new Point((int) RealX, (int) RealY), from.Pixel, to.Pixel, c1.Pixel, c2.Pixel) ||
                             (this.movedContactPoint == eContactPoint.all))
                    {
                        if (this.movedContactPoint != eContactPoint.all)
                        {
                            this.prevCursor = Cursor.Current;
                            Cursor.Current = Cursors.SizeAll;
                        }
                        if (this.isMouseDown == true /*&& this.isMouseMove == true*/)
                        {
//this.isElementSelected == true && isMouseDrag == true)
                            this.movedContactPoint = eContactPoint.all;
                            lastVmlElement = element;
                            if (_PrevMouse.X == -1) _PrevMouse.X = _Mouse.X;
                            if (_PrevMouse.Y == -1) _PrevMouse.Y = _Mouse.Y;
                        }
                        result = Interop.S_OK;
                    }
                    else if (IsOnRect(RealX, RealY, from.Pixel.X, from.Pixel.Y, to.Pixel.X, to.Pixel.Y))
                    {
                        this.ResetCurrentPointer();
                        result = Interop.S_OK;
                        eventObj.cancelBubble = true;
                        eventObj.returnValue = false;

                        /*
                            // TODO: calulate if the pointer is on teh curve 
                            if(this.isMouseDown == true && (this.movedContactPoint == eContactPoint.none)) {
                                this.movedContactPoint = eContactPoint.all;
                                lastVmlElement = element; 
                                if (_PrevMouse.X == -1) _PrevMouse.X = _Mouse.X;
                                if (_PrevMouse.Y == -1) _PrevMouse.Y = _Mouse.Y;
                                result = Interop.S_OK;
                            }    
                            */
                    }
                    else
                    {
                        if (Cursor.Current == Cursors.Hand)
                        {
                            this.ResetCurrentPointer();
                        }
                    }

                    switch (this.movedContactPoint)
                    {
                        case eContactPoint.from:
                            // do the action 
                            SnapToGrid();
                            from.Pixel =
                                new Point(from.Pixel.X + (_Mouse.X - _PrevMouse.X),
                                          from.Pixel.Y + (_Mouse.Y - _PrevMouse.Y));
                            _PrevMouse.X = _Mouse.X;
                            _PrevMouse.Y = _Mouse.Y;
                            result = Interop.S_OK;
                            break;
                        case eContactPoint.to:
                            // do the action 
                            SnapToGrid();
                            to.Pixel =
                                new Point(to.Pixel.X + (_Mouse.X - _PrevMouse.X), to.Pixel.Y + (_Mouse.Y - _PrevMouse.Y));
                            _PrevMouse.X = _Mouse.X;
                            _PrevMouse.Y = _Mouse.Y;
                            result = Interop.S_OK;
                            break;
                        case eContactPoint.ctrl1:
                            // do the action 
                            SnapToGrid();
                            c1.Pixel =
                                new Point(c1.Pixel.X + (_Mouse.X - _PrevMouse.X), c1.Pixel.Y + (_Mouse.Y - _PrevMouse.Y));
                            _PrevMouse.X = _Mouse.X;
                            _PrevMouse.Y = _Mouse.Y;
                            result = Interop.S_OK;
                            break;
                        case eContactPoint.ctrl2:
                            // do the action 
                            SnapToGrid();
                            c2.Pixel =
                                new Point(c2.Pixel.X + (_Mouse.X - _PrevMouse.X), c2.Pixel.Y + (_Mouse.Y - _PrevMouse.Y));
                            _PrevMouse.X = _Mouse.X;
                            _PrevMouse.Y = _Mouse.Y;
                            result = Interop.S_OK;
                            break;
                        case eContactPoint.all:
                            SnapToGrid();
                            from.Pixel =
                                new Point(from.Pixel.X + (_Mouse.X - _PrevMouse.X),
                                          from.Pixel.Y + (_Mouse.Y - _PrevMouse.Y));
                            to.Pixel =
                                new Point(to.Pixel.X + (_Mouse.X - _PrevMouse.X), to.Pixel.Y + (_Mouse.Y - _PrevMouse.Y));
                            c1.Pixel =
                                new Point(c1.Pixel.X + (_Mouse.X - _PrevMouse.X), c1.Pixel.Y + (_Mouse.Y - _PrevMouse.Y));
                            c2.Pixel =
                                new Point(c2.Pixel.X + (_Mouse.X - _PrevMouse.X), c2.Pixel.Y + (_Mouse.Y - _PrevMouse.Y));
                            _PrevMouse.X = _Mouse.X;
                            _PrevMouse.Y = _Mouse.Y;
                            result = Interop.S_OK;
                            break;
                    }


                    break;

                    #endregion

                    #region polyline

                case "polyline":
                    int magicOffset = 10;
                    // first extract all Points
                    VgPoints p = new VgPoints((IVgPoints) GetAttribute(element, "points"));
                    VgVector2D[] points = new VgVector2D[p.length];
                    for (int i = 0; i < p.length; i++)
                    {
                        points[i] = new VgVector2D(p[i]);

                        // find the Point where the Mouse is on
                        if (IsInZone(points[i].Pixel.X, points[i].Pixel.Y + magicOffset) ||
                            (this.movedContactPoint == eContactPoint.nth && whichContactPoint == i))
                        {
                            if (Cursor.Current != Cursors.Hand)
                            {
                                this.prevCursor = Cursor.Current;
                                Cursor.Current = Cursors.Hand;
                            }

                            if (this.isMouseDown == true && (this.movedContactPoint == eContactPoint.none)
                                /*&& this.isMouseMove == true*/)
                            {
//this.isElementSelected == true && isMouseDrag == true)
                                this.movedContactPoint = eContactPoint.nth;
                                lastVmlElement = element;
                                whichContactPoint = i;
                                if (_PrevMouse.X == -1) _PrevMouse.X = _Mouse.X;
                                if (_PrevMouse.Y == -1) _PrevMouse.Y = _Mouse.Y;
                            }
                            result = Interop.S_OK;
                        }
                    }
                    if (result != Interop.S_OK)
                    {
                        for (int j = 0; j < points.Length - 1; j++)
                        {
                            // find the line where the mouse is on
                            if (
                                IsOnLine(RealX, RealY, points[j].Pixel.X, points[j].Pixel.Y, points[j + 1].Pixel.X,
                                         points[j + 1].Pixel.Y) || (this.movedContactPoint == eContactPoint.all))
                            {
                                if (this.movedContactPoint != eContactPoint.all)
                                {
                                    this.prevCursor = Cursor.Current;
                                    Cursor.Current = Cursors.SizeAll;
                                }
                                if (this.isMouseDown == true && (this.movedContactPoint == eContactPoint.none)
                                    /*&& this.isMouseMove == true*/)
                                {
//this.isElementSelected == true && isMouseDrag == true)
                                    this.movedContactPoint = eContactPoint.all;
                                    lastVmlElement = element;
                                    if (_PrevMouse.X == -1) _PrevMouse.X = _Mouse.X;
                                    if (_PrevMouse.Y == -1) _PrevMouse.Y = _Mouse.Y;
                                }
                                result = Interop.S_OK;
                            }
                        }
                    }
                    if (result != Interop.S_OK &&
                        IsOnRect(RealX, RealY, this.GetLeftOffset(element), this.GetTopOffset(element),
                                 this.GetLeftOffset(element) + this.GetWidthOffset(element),
                                 this.GetTopOffset(element) + this.GetHeightOffset(element)))
                    {
                        this.ResetCurrentPointer();
                        result = Interop.S_OK;
                        eventObj.cancelBubble = true;
                        eventObj.returnValue = false;
                    }

                    //
                    // begin to handle
                    //
                    if (this.movedContactPoint == eContactPoint.nth)
                    {
                        SnapToGrid();
                        points[whichContactPoint].Pixel =
                            new Point(points[whichContactPoint].Pixel.X + (_Mouse.X - _PrevMouse.X),
                                      points[whichContactPoint].Pixel.Y + (_Mouse.Y - _PrevMouse.Y));
                        _PrevMouse.X = _Mouse.X;
                        _PrevMouse.Y = _Mouse.Y;
                        result = Interop.S_OK;
                    }
                    else if (this.movedContactPoint == eContactPoint.all && _PrevMouse.X != -1 && _PrevMouse.Y != -1)
                    {
                        SnapToGrid();
                        /*
                            foreach (VgVector2D point in points){ 
                                point.Pixel = new Point (point.Pixel.X + (_Mouse.X - _PrevMouse.X),point.Pixel.Y + (_Mouse.Y - _PrevMouse.Y))  ;
                            }
                            */
                        this.SetStyleAttribute(element, "left", (left + _Mouse.X - _PrevMouse.X) + "px");
                        this.SetStyleAttribute(element, "top", (top + _Mouse.Y - _PrevMouse.Y) + "px");
                        _PrevMouse.X = _Mouse.X;
                        _PrevMouse.Y = _Mouse.Y;
                        result = Interop.S_OK;
                    }
                    break;

                    #endregion

                    #region arc

                case "arc":

                    if (this.movedContactPoint == eContactPoint.none)
                    {
                        // calulate contact points and attribtes and keep then
                        _origStyleProperties.center = new Point((left + right)/2, (top + bottom)/2);
                        _origStyleProperties.topleft =
                            ShapeBehavior.CalculateContactPoint(left, top, _origStyleProperties.center, 0);
                        _origStyleProperties.topright =
                            ShapeBehavior.CalculateContactPoint(right, top, _origStyleProperties.center, 0);
                        _origStyleProperties.topmiddle =
                            ShapeBehavior.CalculateContactPoint((left + right)/2, top, _origStyleProperties.center, 0);
                        _origStyleProperties.bottommiddle =
                            ShapeBehavior.CalculateContactPoint((left + right)/2, bottom, _origStyleProperties.center, 0);
                        _origStyleProperties.bottomleft =
                            ShapeBehavior.CalculateContactPoint(left, bottom, _origStyleProperties.center, 0);
                        _origStyleProperties.bottomright =
                            ShapeBehavior.CalculateContactPoint(right, bottom, _origStyleProperties.center, 0);
                        _origStyleProperties.middleleft =
                            ShapeBehavior.CalculateContactPoint(left, (top + bottom)/2, _origStyleProperties.center, 0);
                        _origStyleProperties.middleright =
                            ShapeBehavior.CalculateContactPoint(right, (top + bottom)/2, _origStyleProperties.center, 0);
                        _origStyleProperties.rotator =
                            ShapeBehavior.CalculateContactPoint((left + right)/2, top - 15, _origStyleProperties.center,
                                                                0);
                        _origStyleProperties.startAngle =
                            (Convert.ToInt32((this.GetAttribute(element, "startangle", true).Equals(String.Empty))
                                                 ? "0"
                                                 : GetAttribute(element, "startangle", true)));
                        _origStyleProperties.endAngle =
                            (Convert.ToInt32((GetAttribute(element, "endangle").Equals(String.Empty))
                                                 ? "0"
                                                 : GetAttribute(element, "endangle")));
                        _origStyleProperties.ctrl1 =
                            ShapeBehavior.CalculateContactPoint(
                                (int)
                                (_origStyleProperties.center.X +
                                 (width/2*Math.Sin(_origStyleProperties.startAngle*Math.PI/180))),
                                (int)
                                (_origStyleProperties.center.Y -
                                 (height/2*Math.Cos(_origStyleProperties.startAngle*Math.PI/180))),
                                _origStyleProperties.center, 0);
                        _origStyleProperties.ctrl2 =
                            ShapeBehavior.CalculateContactPoint(
                                (int)
                                (_origStyleProperties.center.X +
                                 (width/2*Math.Sin(_origStyleProperties.endAngle*Math.PI/180))),
                                (int)
                                (_origStyleProperties.center.Y -
                                 (height/2*Math.Cos(_origStyleProperties.endAngle*Math.PI/180))),
                                _origStyleProperties.center, 0);
                        _origStyleProperties.angle = angle;
                        _origStyleProperties.left = left;
                        _origStyleProperties.top = top;
                        _origStyleProperties.width = width;
                        _origStyleProperties.height = height;
                        _origStyleProperties.right = right;
                        _origStyleProperties.bottom = bottom;
                    }

                    _TransMouse =
                        ShapeBehavior.CalculateContactPoint(_Mouse.X, _Mouse.Y, _origStyleProperties.center,
                                                            -_origStyleProperties.angle);

                    // decide which action
                    if (IsInZone(_origStyleProperties.ctrl1, _TransMouse) ||
                        (this.movedContactPoint == eContactPoint.rotate))
                    {
                        if (Cursor.Current != RotateCursor)
                        {
                            this.prevCursor = Cursor.Current;
                            _htmlEditor.SetMousePointer(true);
                            Cursor.Current = RotateCursor;
                            Cursor.Show();
                        }

                        if (this.isMouseDown == true && (this.movedContactPoint == eContactPoint.none)
                            /*&& this.isMouseMove == true*/)
                        {
                            //this.isElementSelected == true && isMouseDrag == true)
                            this.movedContactPoint = eContactPoint.ctrl1;
                            lastVmlElement = element;
                            if (_PrevMouse.X == -1)
                            {
                                _PrevMouse.X = _Mouse.X;
                                _PrevMouse.Y = _Mouse.Y;
                            }
                        }
                        result = Interop.S_OK;
                    }
                    else if (IsInZone(_origStyleProperties.ctrl2, _TransMouse) ||
                             (this.movedContactPoint == eContactPoint.rotate))
                    {
                        if (Cursor.Current != RotateCursor)
                        {
                            this.prevCursor = Cursor.Current;
                            _htmlEditor.SetMousePointer(true);
                            Cursor.Current = RotateCursor;
                            Cursor.Show();
                        }

                        if (this.isMouseDown == true && (this.movedContactPoint == eContactPoint.none)
                            /*&& this.isMouseMove == true*/)
                        {
                            //this.isElementSelected == true && isMouseDrag == true)
                            this.movedContactPoint = eContactPoint.ctrl2;
                            lastVmlElement = element;
                            if (_PrevMouse.X == -1)
                            {
                                _PrevMouse.X = _Mouse.X;
                                _PrevMouse.Y = _Mouse.Y;
                            }
                        }
                        result = Interop.S_OK;
                    }
                    else if (IsInZone(_origStyleProperties.topleft, _TransMouse) ||
                             (this.movedContactPoint == eContactPoint.topleft))
                    {
                        if (Cursor.Current != Cursors.SizeNWSE)
                        {
                            this.prevCursor = Cursor.Current;
                            Cursor.Current = Cursors.SizeNWSE;
                        }

                        if (this.isMouseDown == true && (this.movedContactPoint == eContactPoint.none)
                            /*&& this.isMouseMove == true*/)
                        {
//this.isElementSelected == true && isMouseDrag == true)
                            this.movedContactPoint = eContactPoint.topleft;
                            lastVmlElement = element;
                            if (_PrevMouse.X == -1)
                            {
                                _PrevMouse.X = _Mouse.X;
                                _PrevMouse.Y = _Mouse.Y;
                            }
                        }
                        result = Interop.S_OK;
                    }
                    else if (IsInZone(_origStyleProperties.topright, _TransMouse) ||
                             (this.movedContactPoint == eContactPoint.topright))
                    {
                        if (Cursor.Current != Cursors.SizeNESW)
                        {
                            this.prevCursor = Cursor.Current;
                            Cursor.Current = Cursors.SizeNESW;
                        }

                        if (this.isMouseDown == true && (this.movedContactPoint == eContactPoint.none)
                            /*&& this.isMouseMove == true*/)
                        {
//this.isElementSelected == true && isMouseDrag == true)
                            this.movedContactPoint = eContactPoint.topright;
                            lastVmlElement = element;
                            if (_PrevMouse.X == -1)
                            {
                                _PrevMouse.X = _Mouse.X;
                                _PrevMouse.Y = _Mouse.Y;
                                _PrevTransMouse.X = _TransMouse.X;
                                _PrevTransMouse.Y = _TransMouse.Y;
                            }
                        }
                        result = Interop.S_OK;
                    }
                    else if (IsInZone(_origStyleProperties.topmiddle, _TransMouse) ||
                             (this.movedContactPoint == eContactPoint.top))
                    {
                        if (Cursor.Current != Cursors.SizeNS)
                        {
                            this.prevCursor = Cursor.Current;
                            Cursor.Current = Cursors.SizeNS;
                        }

                        if (this.isMouseDown == true && (this.movedContactPoint == eContactPoint.none)
                            /*&& this.isMouseMove == true*/)
                        {
//this.isElementSelected == true && isMouseDrag == true)
                            this.movedContactPoint = eContactPoint.top;
                            lastVmlElement = element;
                            if (_PrevMouse.X == -1)
                            {
                                _PrevMouse.X = _Mouse.X;
                                _PrevMouse.Y = _Mouse.Y;
                                _PrevTransMouse.X = _TransMouse.X;
                                _PrevTransMouse.Y = _TransMouse.Y;
                            }
                        }
                        result = Interop.S_OK;
                    }
                    else if (IsInZone(_origStyleProperties.bottomleft, _TransMouse) ||
                             (this.movedContactPoint == eContactPoint.bottomleft))
                    {
                        if (Cursor.Current != Cursors.SizeNESW)
                        {
                            this.prevCursor = Cursor.Current;
                            Cursor.Current = Cursors.SizeNESW;
                        }

                        if (this.isMouseDown == true &&
                            (this.movedContactPoint == eContactPoint.none)
                            /*&& this.isMouseMove == true*/)
                        {
//this.isElementSelected == true && isMouseDrag == true)
                            this.movedContactPoint = eContactPoint.bottomleft;
                            lastVmlElement = element;
                            if (_PrevMouse.X == -1)
                            {
                                _PrevMouse.X = _Mouse.X;
                                _PrevMouse.Y = _Mouse.Y;
                                _PrevTransMouse.X = _TransMouse.X;
                                _PrevTransMouse.Y = _TransMouse.Y;
                            }
                        }
                        result = Interop.S_OK;
                    }
                    else if (IsInZone(_origStyleProperties.bottomright, _TransMouse) ||
                             (this.movedContactPoint == eContactPoint.bottomrigth))
                    {
                        if (Cursor.Current != Cursors.SizeNWSE)
                        {
                            this.prevCursor = Cursor.Current;
                            Cursor.Current = Cursors.SizeNWSE;
                        }

                        if (this.isMouseDown == true &&
                            (this.movedContactPoint == eContactPoint.none)
                            /*&& this.isMouseMove == true*/)
                        {
//this.isElementSelected == true && isMouseDrag == true)
                            this.movedContactPoint = eContactPoint.bottomrigth;
                            lastVmlElement = element;
                            if (_PrevMouse.X == -1)
                            {
                                _PrevMouse.X = _Mouse.X;
                                _PrevMouse.Y = _Mouse.Y;
                                _PrevTransMouse.X = _TransMouse.X;
                                _PrevTransMouse.Y = _TransMouse.Y;
                            }
                        }
                        result = Interop.S_OK;
                    }
                    else if (IsInZone(_origStyleProperties.bottommiddle, _TransMouse) ||
                             (this.movedContactPoint == eContactPoint.bottom))
                    {
                        if (Cursor.Current != Cursors.SizeNS)
                        {
                            this.prevCursor = Cursor.Current;
                            Cursor.Current = Cursors.SizeNS;
                        }

                        if (this.isMouseDown == true &&
                            (this.movedContactPoint == eContactPoint.none)
                            /*&& this.isMouseMove == true*/)
                        {
//this.isElementSelected == true && isMouseDrag == true)
                            this.movedContactPoint = eContactPoint.bottom;
                            lastVmlElement = element;
                            if (_PrevMouse.X == -1)
                            {
                                _PrevMouse.X = _Mouse.X;
                                _PrevMouse.Y = _Mouse.Y;
                                _PrevTransMouse.X = _TransMouse.X;
                                _PrevTransMouse.Y = _TransMouse.Y;
                            }
                        }
                        result = Interop.S_OK;
                    }
                    else if (IsInZone(_origStyleProperties.middleleft, _TransMouse) ||
                             (this.movedContactPoint == eContactPoint.left))
                    {
                        if (Cursor.Current != Cursors.SizeWE)
                        {
                            this.prevCursor = Cursor.Current;
                            Cursor.Current = Cursors.SizeWE;
                        }

                        if (this.isMouseDown == true &&
                            (this.movedContactPoint == eContactPoint.none)
                            /*&& this.isMouseMove == true*/)
                        {
//this.isElementSelected == true && isMouseDrag == true)
                            this.movedContactPoint = eContactPoint.left;
                            lastVmlElement = element;
                            if (_PrevMouse.X == -1)
                            {
                                _PrevMouse.X = _Mouse.X;
                                _PrevMouse.Y = _Mouse.Y;
                                _PrevTransMouse.X = _TransMouse.X;
                                _PrevTransMouse.Y = _TransMouse.Y;
                            }
                        }
                        result = Interop.S_OK;
                    }
                    else if (IsInZone(_origStyleProperties.middleright, _TransMouse) ||
                             (this.movedContactPoint == eContactPoint.right))
                    {
                        if (Cursor.Current != Cursors.SizeWE)
                        {
                            this.prevCursor = Cursor.Current;
                            Cursor.Current = Cursors.SizeWE;
                        }

                        if (this.isMouseDown == true &&
                            (this.movedContactPoint == eContactPoint.none)
                            /*&& this.isMouseMove == true*/)
                        {
//this.isElementSelected == true && isMouseDrag == true)
                            this.movedContactPoint = eContactPoint.right;
                            lastVmlElement = element;
                            if (_PrevMouse.X == -1)
                            {
                                _PrevMouse.X = _Mouse.X;
                                _PrevMouse.Y = _Mouse.Y;
                                _PrevTransMouse.X = _TransMouse.X;
                                _PrevTransMouse.Y = _TransMouse.Y;
                            }
                        }
                        result = Interop.S_OK;
                    }
                    else if (IsInZone(_origStyleProperties.rotator, _TransMouse) ||
                             (this.movedContactPoint == eContactPoint.rotate))
                    {
                        if (Cursor.Current != RotateCursor)
                        {
                            this.prevCursor = Cursor.Current;
                            _htmlEditor.SetMousePointer(true);
                            Cursor.Current = RotateCursor;
                            Cursor.Show();
                        }

                        if (this.isMouseDown == true &&
                            (this.movedContactPoint == eContactPoint.none)
                            /*&& this.isMouseMove == true*/)
                        {
                            //this.isElementSelected == true && isMouseDrag == true)
                            this.movedContactPoint = eContactPoint.rotate;
                            lastVmlElement = element;
                            if (_PrevMouse.X == -1)
                            {
                                _PrevMouse.X = _Mouse.X;
                                _PrevMouse.Y = _Mouse.Y;
                            }
                        }
                        result = Interop.S_OK;
                    }
                    else if (
                        IsOnRect(_TransMouse.X, _TransMouse.Y, left, top,
                                 width, height) ||
                        (this.movedContactPoint == eContactPoint.all))
                    {
                        if (this.movedContactPoint != eContactPoint.all)
                        {
                            this.prevCursor = Cursor.Current;
                            Cursor.Current = Cursors.SizeAll;
                        }
                        if (this.isMouseDown == true &&
                            this.isMouseMove == true &&
                            (this.movedContactPoint == eContactPoint.none))
                        {
//this.isElementSelected == true && isMouseDrag == true)
                            this.movedContactPoint = eContactPoint.all;
                            lastVmlElement = element;
                            if (_PrevMouse.X == -1)
                            {
                                _PrevMouse.X = _Mouse.X;
                                _PrevMouse.Y = _Mouse.Y;
                            }
                        }
                        this.ResetCurrentPointer();
                        result = Interop.S_OK;
                        eventObj.cancelBubble = true;
                        eventObj.returnValue = false;
                    }

                    // perform the action
                    if (_PrevMouse.X != -1 && _PrevMouse.Y != -1)
                    {
                        SnapToGrid();

                        //System.Diagnostics.Debug.WriteLine(this.movedContactPoint);

                        int diffMouseX = Convert.ToInt32(_Mouse.X - _PrevMouse.X);
                        int diffMouseY = Convert.ToInt32(_Mouse.Y - _PrevMouse.Y);
                        int diffTransX = Convert.ToInt32(_TransMouse.X - _PrevTransMouse.X);
                        int diffTransY = Convert.ToInt32(_TransMouse.Y - _PrevTransMouse.Y);

                        Point unRotatedTopLeft, rotatedTopLeft, newCenter, newPoint;

                        switch (this.movedContactPoint)
                        {
                            case eContactPoint.all:
                                left = _origStyleProperties.left + diffMouseX;
                                top = _origStyleProperties.top + diffMouseY;
                                break;
                            case eContactPoint.topleft:
                                if (angle != 0)
                                {
                                    // for the calulation only the TopLeft and the Center is need
                                    rotatedTopLeft =
                                        ShapeBehavior.CalculateContactPoint(_origStyleProperties.topleft.X,
                                                                            _origStyleProperties.topleft.Y,
                                                                            _origStyleProperties.center,
                                                                            _origStyleProperties.angle);
                                    newPoint = new Point(rotatedTopLeft.X + diffMouseX, rotatedTopLeft.Y + diffMouseY);
                                    newCenter =
                                        new Point(_origStyleProperties.center.X + (diffMouseX/2),
                                                  _origStyleProperties.center.Y + (diffMouseY/2));
                                    unRotatedTopLeft =
                                        ShapeBehavior.CalculateContactPoint(newPoint.X, newPoint.Y, newCenter, -angle);
                                    left = unRotatedTopLeft.X;
                                    top = unRotatedTopLeft.Y;
                                    width = (newCenter.X - unRotatedTopLeft.X)*2;
                                    height = (newCenter.Y - unRotatedTopLeft.Y)*2;
                                }
                                else
                                {
                                    left = _origStyleProperties.left + diffMouseX;
                                    top = _origStyleProperties.top + diffMouseY;
                                    width = _origStyleProperties.width - diffMouseX;
                                    height = _origStyleProperties.height - diffMouseY;
                                }
                                break;
                            case eContactPoint.topright:
                                if (angle != 0)
                                {
                                    rotatedTopLeft =
                                        ShapeBehavior.CalculateContactPoint(_origStyleProperties.topleft.X,
                                                                            _origStyleProperties.topleft.Y + diffTransY,
                                                                            _origStyleProperties.center,
                                                                            _origStyleProperties.angle);
                                    newCenter =
                                        new Point(_origStyleProperties.center.X + (diffMouseX/2),
                                                  _origStyleProperties.center.Y + (diffMouseY/2));
                                    unRotatedTopLeft =
                                        ShapeBehavior.CalculateContactPoint(rotatedTopLeft.X, rotatedTopLeft.Y,
                                                                            newCenter, -angle);
                                    left = unRotatedTopLeft.X;
                                    top = unRotatedTopLeft.Y;
                                    width = (newCenter.X - unRotatedTopLeft.X)*2;
                                    height = (newCenter.Y - unRotatedTopLeft.Y)*2;
                                }
                                else
                                {
                                    width = _origStyleProperties.width + diffMouseX;
                                    top = _origStyleProperties.top + diffMouseY;
                                    height = _origStyleProperties.height - diffMouseY;
                                }
                                break;
                            case eContactPoint.top:
                                if (angle != 0)
                                {
                                    // it is only the Y-Part of the movment need in the unrotated world, so calulate it and rotate it back
                                    // to calumate the movment like on the other corners
                                    newPoint =
                                        ShapeBehavior.CalculateContactPoint(0, diffTransY, new Point(0, 0), angle);
                                    rotatedTopLeft =
                                        ShapeBehavior.CalculateContactPoint(_origStyleProperties.topleft.X,
                                                                            _origStyleProperties.topleft.Y,
                                                                            _origStyleProperties.center,
                                                                            _origStyleProperties.angle);
                                    newCenter =
                                        new Point(_origStyleProperties.center.X + (newPoint.X/2),
                                                  _origStyleProperties.center.Y + (newPoint.Y/2));
                                    unRotatedTopLeft =
                                        ShapeBehavior.CalculateContactPoint(rotatedTopLeft.X + newPoint.X,
                                                                            rotatedTopLeft.Y + newPoint.Y, newCenter,
                                                                            -angle);
                                    left = unRotatedTopLeft.X;
                                    top = unRotatedTopLeft.Y;
                                    width = (newCenter.X - unRotatedTopLeft.X)*2;
                                    height = (newCenter.Y - unRotatedTopLeft.Y)*2;
                                }
                                else
                                {
                                    top = _origStyleProperties.top + diffMouseY;
                                    height = _origStyleProperties.height - diffMouseY;
                                }
                                break;
                            case eContactPoint.bottomleft:
                                if (angle != 0)
                                {
                                    rotatedTopLeft =
                                        ShapeBehavior.CalculateContactPoint(
                                            _origStyleProperties.topleft.X + diffTransX, _origStyleProperties.topleft.Y,
                                            _origStyleProperties.center, _origStyleProperties.angle);
                                    newCenter =
                                        new Point(_origStyleProperties.center.X + (diffMouseX/2),
                                                  _origStyleProperties.center.Y + (diffMouseY/2));
                                    unRotatedTopLeft =
                                        ShapeBehavior.CalculateContactPoint(rotatedTopLeft.X, rotatedTopLeft.Y,
                                                                            newCenter, -angle);
                                    left = unRotatedTopLeft.X;
                                    top = unRotatedTopLeft.Y;
                                    width = (newCenter.X - unRotatedTopLeft.X)*2;
                                    height = (newCenter.Y - unRotatedTopLeft.Y)*2;
                                }
                                else
                                {
                                    left = _origStyleProperties.left + diffMouseX;
                                    width = _origStyleProperties.width - diffMouseX;
                                    height = _origStyleProperties.height + diffMouseY;
                                }
                                break;
                            case eContactPoint.bottomrigth:
                                if (angle != 0)
                                {
                                    rotatedTopLeft =
                                        ShapeBehavior.CalculateContactPoint(_origStyleProperties.topleft.X,
                                                                            _origStyleProperties.topleft.Y,
                                                                            _origStyleProperties.center,
                                                                            _origStyleProperties.angle);
                                    newCenter =
                                        new Point(_origStyleProperties.center.X + (diffMouseX/2),
                                                  _origStyleProperties.center.Y + (diffMouseY/2));
                                    unRotatedTopLeft =
                                        ShapeBehavior.CalculateContactPoint(rotatedTopLeft.X, rotatedTopLeft.Y,
                                                                            newCenter, -angle);
                                    left = unRotatedTopLeft.X;
                                    top = unRotatedTopLeft.Y;
                                    width = (newCenter.X - unRotatedTopLeft.X)*2;
                                    height = (newCenter.Y - unRotatedTopLeft.Y)*2;
                                }
                                else
                                {
                                    width = _origStyleProperties.width + diffMouseX;
                                    height = _origStyleProperties.height + diffMouseY;
                                }
                                break;
                            case eContactPoint.bottom:
                                if (angle != 0)
                                {
                                    // it is only the Y-Part of the movment need in the unrotated world, so calulate it and rotate it back
                                    // to calumate the movment like on the other corners
                                    newPoint =
                                        ShapeBehavior.CalculateContactPoint(0, diffTransY, new Point(0, 0), angle);
                                    rotatedTopLeft =
                                        ShapeBehavior.CalculateContactPoint(_origStyleProperties.topleft.X,
                                                                            _origStyleProperties.topleft.Y,
                                                                            _origStyleProperties.center,
                                                                            _origStyleProperties.angle);
                                    newCenter =
                                        new Point(_origStyleProperties.center.X + (newPoint.X/2),
                                                  _origStyleProperties.center.Y + (newPoint.Y/2));
                                    unRotatedTopLeft =
                                        ShapeBehavior.CalculateContactPoint(rotatedTopLeft.X, rotatedTopLeft.Y,
                                                                            newCenter, -angle);
                                    left = unRotatedTopLeft.X;
                                    top = unRotatedTopLeft.Y;
                                    width = (newCenter.X - unRotatedTopLeft.X)*2;
                                    height = (newCenter.Y - unRotatedTopLeft.Y)*2;
                                }
                                else
                                {
                                    height = _origStyleProperties.height + diffMouseY;
                                }
                                break;
                            case eContactPoint.left:
                                if (angle != 0)
                                {
                                    // it is only the Y-Part of the movment need in the unrotated world, so calulate it and rotate it back
                                    // to calumate the movment like on the other corners
                                    newPoint =
                                        ShapeBehavior.CalculateContactPoint(diffTransX, 0, new Point(0, 0), angle);
                                    rotatedTopLeft =
                                        ShapeBehavior.CalculateContactPoint(_origStyleProperties.topleft.X,
                                                                            _origStyleProperties.topleft.Y,
                                                                            _origStyleProperties.center,
                                                                            _origStyleProperties.angle);
                                    newCenter =
                                        new Point(_origStyleProperties.center.X + (newPoint.X/2),
                                                  _origStyleProperties.center.Y + (newPoint.Y/2));
                                    unRotatedTopLeft =
                                        ShapeBehavior.CalculateContactPoint(rotatedTopLeft.X + newPoint.X,
                                                                            rotatedTopLeft.Y + newPoint.Y, newCenter,
                                                                            -angle);
                                    left = unRotatedTopLeft.X;
                                    top = unRotatedTopLeft.Y;
                                    width = (newCenter.X - unRotatedTopLeft.X)*2;
                                    height = (newCenter.Y - unRotatedTopLeft.Y)*2;
                                }
                                else
                                {
                                    left = _origStyleProperties.left + diffMouseX;
                                    width = _origStyleProperties.width - diffMouseX;
                                }
                                break;
                            case eContactPoint.right:
                                if (angle != 0)
                                {
                                    // it is only the Y-Part of the movment need in the unrotated world, so calulate it and rotate it back
                                    // to calumate the movment like on the other corners
                                    newPoint =
                                        ShapeBehavior.CalculateContactPoint(diffTransX, 0, new Point(0, 0), angle);
                                    rotatedTopLeft =
                                        ShapeBehavior.CalculateContactPoint(_origStyleProperties.topleft.X,
                                                                            _origStyleProperties.topleft.Y,
                                                                            _origStyleProperties.center,
                                                                            _origStyleProperties.angle);
                                    newCenter =
                                        new Point(_origStyleProperties.center.X + (newPoint.X/2),
                                                  _origStyleProperties.center.Y + (newPoint.Y/2));
                                    unRotatedTopLeft =
                                        ShapeBehavior.CalculateContactPoint(rotatedTopLeft.X, rotatedTopLeft.Y,
                                                                            newCenter, -angle);
                                    left = unRotatedTopLeft.X;
                                    top = unRotatedTopLeft.Y;
                                    width = (newCenter.X - unRotatedTopLeft.X)*2;
                                    height = (newCenter.Y - unRotatedTopLeft.Y)*2;
                                }
                                else
                                {
                                    width = _origStyleProperties.width + diffMouseX;
                                }
                                break;
                            case eContactPoint.rotate:
                                angle = _origStyleProperties.angle + diffMouseX;
                                break;
                            case eContactPoint.ctrl1:
                                this.SetAttribute(element, "startangle", _origStyleProperties.startAngle + diffMouseX);
                                break;
                            case eContactPoint.ctrl2:
                                this.SetAttribute(element, "endangle", _origStyleProperties.endAngle + diffMouseX);
                                break;
                            default:
                                // do nothing ;-)
                                break;
                        }
                        //							
                        //							_PrevMouse.X = _Mouse.X;
                        //                          _PrevMouse.Y = _Mouse.Y;
                        //
                        //                          _PrevTransMouse.X = _TransMouse.X;
                        //                          _PrevTransMouse.Y = _TransMouse.Y;

                        if (height < 10)
                        {
                            height = 10;
                            flipy = !flipy;
                        }
                        if (width < 10)
                        {
                            width = 10;
                            flipx = !flipx;
                        }

                        this.SetStyleAttribute(element, "flip", ((flipx) ? "x " : "") + ((flipy) ? "y" : ""));
                        this.SetStyleAttribute(element, "left", left.ToString() + "px");
                        this.SetStyleAttribute(element, "top", top.ToString() + "px");
                        this.SetStyleAttribute(element, "width", width.ToString() + "px");
                        this.SetStyleAttribute(element, "height", height.ToString() + "px");
                        this.SetStyleAttribute(element, "rotation", angle.ToString());
                        result = Interop.S_OK;
                    }
                    break;

                    #endregion

                    #region shape

                case "oval":
                case "shape":
                case "roundrect":
                case "rect":

                    if (this.movedContactPoint == eContactPoint.none)
                    {
                        // calulate contact points and attribtes and keep then
                        _origStyleProperties.center = new Point((left + right)/2, (top + bottom)/2);
                        _origStyleProperties.topleft =
                            ShapeBehavior.CalculateContactPoint(left, top, _origStyleProperties.center, 0);
                        _origStyleProperties.topright =
                            ShapeBehavior.CalculateContactPoint(right, top, _origStyleProperties.center, 0);
                        _origStyleProperties.topmiddle =
                            ShapeBehavior.CalculateContactPoint((left + right)/2, top, _origStyleProperties.center, 0);
                        _origStyleProperties.bottommiddle =
                            ShapeBehavior.CalculateContactPoint((left + right)/2, bottom, _origStyleProperties.center, 0);
                        _origStyleProperties.bottomleft =
                            ShapeBehavior.CalculateContactPoint(left, bottom, _origStyleProperties.center, 0);
                        _origStyleProperties.bottomright =
                            ShapeBehavior.CalculateContactPoint(right, bottom, _origStyleProperties.center, 0);
                        _origStyleProperties.middleleft =
                            ShapeBehavior.CalculateContactPoint(left, (top + bottom)/2, _origStyleProperties.center, 0);
                        _origStyleProperties.middleright =
                            ShapeBehavior.CalculateContactPoint(right, (top + bottom)/2, _origStyleProperties.center, 0);
                        _origStyleProperties.rotator =
                            ShapeBehavior.CalculateContactPoint((left + right)/2, top - 15, _origStyleProperties.center,
                                                                0);
                        _origStyleProperties.angle = angle;
                        _origStyleProperties.left = left;
                        _origStyleProperties.top = top;
                        _origStyleProperties.width = width;
                        _origStyleProperties.height = height;
                        _origStyleProperties.right = right;
                        _origStyleProperties.bottom = bottom;
                    }

                    _TransMouse =
                        ShapeBehavior.CalculateContactPoint(_Mouse.X, _Mouse.Y, _origStyleProperties.center,
                                                            -_origStyleProperties.angle);

                    // decide which action 
                    if (IsInZone(_origStyleProperties.topleft, _TransMouse) ||
                        (this.movedContactPoint == eContactPoint.topleft))
                    {
                        if (Cursor.Current != Cursors.SizeNWSE)
                        {
                            this.prevCursor = Cursor.Current;
                            Cursor.Current = Cursors.SizeNWSE;
                        }

                        if (this.isMouseDown && (this.movedContactPoint == eContactPoint.none)
                            /*&& this.isMouseMove == true*/) //this.isElementSelected == true && isMouseDrag == true)
                        {
                            this.movedContactPoint = eContactPoint.topleft;
                            lastVmlElement = element;
                            if (_PrevMouse.X == -1)
                            {
                                _PrevMouse.X = _Mouse.X;
                                _PrevMouse.Y = _Mouse.Y;
                            }
                        }
                        result = Interop.S_OK;
                    }
                    else if (IsInZone(_origStyleProperties.topright, _TransMouse) ||
                             (this.movedContactPoint == eContactPoint.topright))
                    {
                        if (Cursor.Current != Cursors.SizeNESW)
                        {
                            this.prevCursor = Cursor.Current;
                            Cursor.Current = Cursors.SizeNESW;
                        }

                        if (this.isMouseDown && (this.movedContactPoint == eContactPoint.none)
                            /*&& this.isMouseMove == true*/)
                            //this.isElementSelected == true && isMouseDrag == true)
                        {
                            this.movedContactPoint = eContactPoint.topright;
                            lastVmlElement = element;
                            if (_PrevMouse.X == -1)
                            {
                                _PrevMouse.X = _Mouse.X;
                                _PrevMouse.Y = _Mouse.Y;
                                _PrevTransMouse.X = _TransMouse.X;
                                _PrevTransMouse.Y = _TransMouse.Y;
                            }
                        }
                        result = Interop.S_OK;
                    }
                    else if (IsInZone(_origStyleProperties.topmiddle, _TransMouse) ||
                             (this.movedContactPoint == eContactPoint.top))
                    {
                        if (Cursor.Current != Cursors.SizeNS)
                        {
                            this.prevCursor = Cursor.Current;
                            Cursor.Current = Cursors.SizeNS;
                        }

                        if (this.isMouseDown && (this.movedContactPoint == eContactPoint.none)
                            /*&& this.isMouseMove == true*/)
                            //this.isElementSelected == true && isMouseDrag == true)
                        {
                            this.movedContactPoint = eContactPoint.top;
                            lastVmlElement = element;
                            if (_PrevMouse.X == -1)
                            {
                                _PrevMouse.X = _Mouse.X;
                                _PrevMouse.Y = _Mouse.Y;
                                _PrevTransMouse.X = _TransMouse.X;
                                _PrevTransMouse.Y = _TransMouse.Y;
                            }
                        }
                        result = Interop.S_OK;
                    }
                    else if (IsInZone(_origStyleProperties.bottomleft, _TransMouse) ||
                             (this.movedContactPoint == eContactPoint.bottomleft))
                    {
                        if (Cursor.Current != Cursors.SizeNESW)
                        {
                            this.prevCursor = Cursor.Current;
                            Cursor.Current = Cursors.SizeNESW;
                        }

                        if (this.isMouseDown == true && (this.movedContactPoint == eContactPoint.none)
                            /*&& this.isMouseMove == true*/)
                            //this.isElementSelected == true && isMouseDrag == true)
                        {
                            this.movedContactPoint = eContactPoint.bottomleft;
                            lastVmlElement = element;
                            if (_PrevMouse.X == -1)
                            {
                                _PrevMouse.X = _Mouse.X;
                                _PrevMouse.Y = _Mouse.Y;
                                _PrevTransMouse.X = _TransMouse.X;
                                _PrevTransMouse.Y = _TransMouse.Y;
                            }
                        }
                        result = Interop.S_OK;
                    }
                    else if (IsInZone(_origStyleProperties.bottomright, _TransMouse) ||
                             (this.movedContactPoint == eContactPoint.bottomrigth))
                    {
                        if (Cursor.Current != Cursors.SizeNWSE)
                        {
                            this.prevCursor = Cursor.Current;
                            Cursor.Current = Cursors.SizeNWSE;
                        }

                        if (this.isMouseDown == true && (this.movedContactPoint == eContactPoint.none)
                            /*&& this.isMouseMove == true*/)
                            //this.isElementSelected == true && isMouseDrag == true)
                        {
                            this.movedContactPoint = eContactPoint.bottomrigth;
                            lastVmlElement = element;
                            if (_PrevMouse.X == -1)
                            {
                                _PrevMouse.X = _Mouse.X;
                                _PrevMouse.Y = _Mouse.Y;
                                _PrevTransMouse.X = _TransMouse.X;
                                _PrevTransMouse.Y = _TransMouse.Y;
                            }
                        }
                        result = Interop.S_OK;
                    }
                    else if (IsInZone(_origStyleProperties.bottommiddle, _TransMouse) ||
                             (this.movedContactPoint == eContactPoint.bottom))
                    {
                        if (Cursor.Current != Cursors.SizeNS)
                        {
                            this.prevCursor = Cursor.Current;
                            Cursor.Current = Cursors.SizeNS;
                        }

                        if (this.isMouseDown == true &&
                            (this.movedContactPoint == eContactPoint.none)
                            /*&& this.isMouseMove == true*/)
                            //this.isElementSelected == true && isMouseDrag == true)
                        {
                            this.movedContactPoint = eContactPoint.bottom;
                            lastVmlElement = element;
                            if (_PrevMouse.X == -1)
                            {
                                _PrevMouse.X = _Mouse.X;
                                _PrevMouse.Y = _Mouse.Y;
                                _PrevTransMouse.X = _TransMouse.X;
                                _PrevTransMouse.Y = _TransMouse.Y;
                            }
                        }
                        result = Interop.S_OK;
                    }
                    else if (IsInZone(_origStyleProperties.middleleft, _TransMouse) ||
                             (this.movedContactPoint == eContactPoint.left))
                    {
                        if (Cursor.Current != Cursors.SizeWE)
                        {
                            this.prevCursor = Cursor.Current;
                            Cursor.Current = Cursors.SizeWE;
                        }

                        if (this.isMouseDown == true &&
                            (this.movedContactPoint == eContactPoint.none)
                            /*&& this.isMouseMove == true*/)
                            //this.isElementSelected == true && isMouseDrag == true)
                        {
                            this.movedContactPoint = eContactPoint.left;
                            lastVmlElement = element;
                            if (_PrevMouse.X == -1)
                            {
                                _PrevMouse.X = _Mouse.X;
                                _PrevMouse.Y = _Mouse.Y;
                                _PrevTransMouse.X = _TransMouse.X;
                                _PrevTransMouse.Y = _TransMouse.Y;
                            }
                        }
                        result = Interop.S_OK;
                    }
                    else if (IsInZone(_origStyleProperties.middleright, _TransMouse) ||
                             (this.movedContactPoint == eContactPoint.right))
                    {
                        if (Cursor.Current != Cursors.SizeWE)
                        {
                            this.prevCursor = Cursor.Current;
                            Cursor.Current = Cursors.SizeWE;
                        }

                        if (this.isMouseDown == true &&
                            (this.movedContactPoint == eContactPoint.none)
                            /*&& this.isMouseMove == true*/)
                            //this.isElementSelected == true && isMouseDrag == true)
                        {
                            this.movedContactPoint = eContactPoint.right;
                            lastVmlElement = element;
                            if (_PrevMouse.X == -1)
                            {
                                _PrevMouse.X = _Mouse.X;
                                _PrevMouse.Y = _Mouse.Y;
                                _PrevTransMouse.X = _TransMouse.X;
                                _PrevTransMouse.Y = _TransMouse.Y;
                            }
                        }
                        result = Interop.S_OK;
                    }
                    else if (_canRotate)
                    {
                        if (IsInZone(_origStyleProperties.rotator, _TransMouse) ||
                            (this.movedContactPoint == eContactPoint.rotate))
                        {
                            if (Cursor.Current != RotateCursor)
                            {
                                this.prevCursor = Cursor.Current;
                                _htmlEditor.SetMousePointer(true);
                                Cursor.Current = RotateCursor;
                                Cursor.Show();
                            }
                            //                            if(System.Windows.Forms.Cursor.Current != System.Windows.Forms.Cursors.VSplit) 
                            //                            {
                            //                                this.prevCursor = System.Windows.Forms.Cursor.Current;
                            //                                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.VSplit;                                
                            //                            }

                            if (this.isMouseDown == true &&
                                (this.movedContactPoint == eContactPoint.none)
                                /*&& this.isMouseMove == true*/)
                            {
//this.isElementSelected == true && isMouseDrag == true)
                                this.movedContactPoint = eContactPoint.rotate;
                                lastVmlElement = element;
                                if (_PrevMouse.X == -1)
                                {
                                    _PrevMouse.X = _Mouse.X;
                                    _PrevMouse.Y = _Mouse.Y;
                                }
                            }
                            result = Interop.S_OK;
                        }
                    }
                    if (IsOnRect(_TransMouse.X, _TransMouse.Y, left, top, width, height) ||
                        (this.movedContactPoint == eContactPoint.all))
                    {
                        if (this.movedContactPoint != eContactPoint.all)
                        {
                            this.prevCursor = Cursor.Current;
                            Cursor.Current = Cursors.SizeAll;
                        }
                        if (this.isMouseDown == true && this.isMouseMove == true &&
                            (this.movedContactPoint == eContactPoint.none))
                            //this.isElementSelected == true && isMouseDrag == true)
                        {
                            this.movedContactPoint = eContactPoint.all;
                            lastVmlElement = element;
                            if (_PrevMouse.X == -1)
                            {
                                _PrevMouse.X = _Mouse.X;
                                _PrevMouse.Y = _Mouse.Y;
                            }
                        }
                        this.ResetCurrentPointer();
                        result = Interop.S_OK;
                        eventObj.cancelBubble = true;
                        eventObj.returnValue = false;
                    }

                    // perform the action
                    if (_PrevMouse.X != -1 && _PrevMouse.Y != -1)
                    {
                        SnapToGrid();

                        //System.Diagnostics.Debug.WriteLine(this.movedContactPoint);

                        int diffMouseX = Convert.ToInt32(_Mouse.X - _PrevMouse.X);
                        int diffMouseY = Convert.ToInt32(_Mouse.Y - _PrevMouse.Y);
                        int diffTransX = Convert.ToInt32(_TransMouse.X - _PrevTransMouse.X);
                        int diffTransY = Convert.ToInt32(_TransMouse.Y - _PrevTransMouse.Y);

                        Point unRotatedTopLeft, rotatedTopLeft, newCenter, newPoint;

                        switch (this.movedContactPoint)
                        {
                            case eContactPoint.all:
                                left = _origStyleProperties.left + diffMouseX;
                                top = _origStyleProperties.top + diffMouseY;
                                break;
                            case eContactPoint.topleft:
                                if (angle != 0)
                                {
                                    // for the calulation only the TopLeft and the Center is need
                                    rotatedTopLeft =
                                        ShapeBehavior.CalculateContactPoint(_origStyleProperties.topleft.X,
                                                                            _origStyleProperties.topleft.Y,
                                                                            _origStyleProperties.center,
                                                                            _origStyleProperties.angle);
                                    newPoint = new Point(rotatedTopLeft.X + diffMouseX, rotatedTopLeft.Y + diffMouseY);
                                    newCenter =
                                        new Point(_origStyleProperties.center.X + (diffMouseX/2),
                                                  _origStyleProperties.center.Y + (diffMouseY/2));
                                    unRotatedTopLeft =
                                        ShapeBehavior.CalculateContactPoint(newPoint.X, newPoint.Y, newCenter, -angle);
                                    left = unRotatedTopLeft.X;
                                    top = unRotatedTopLeft.Y;
                                    width = (newCenter.X - unRotatedTopLeft.X)*2;
                                    height = (newCenter.Y - unRotatedTopLeft.Y)*2;

                                    //                                        // correct the round-error
                                    //                                        if ((diffMouseX & 0x01) == 1){
                                    //                                            width++ ;
                                    //                                        }
                                    //                                        if ((diffMouseY & 0x01) == 1){
                                    //                                            height++ ;
                                    //                                        }
                                }
                                else
                                {
                                    left = _origStyleProperties.left + diffMouseX;
                                    top = _origStyleProperties.top + diffMouseY;
                                    width = _origStyleProperties.width - diffMouseX;
                                    height = _origStyleProperties.height - diffMouseY;
                                }
                                break;
                            case eContactPoint.topright:
                                if (angle != 0)
                                {
                                    rotatedTopLeft =
                                        ShapeBehavior.CalculateContactPoint(_origStyleProperties.topleft.X,
                                                                            _origStyleProperties.topleft.Y + diffTransY,
                                                                            _origStyleProperties.center,
                                                                            _origStyleProperties.angle);
                                    newCenter =
                                        new Point(_origStyleProperties.center.X + (diffMouseX/2),
                                                  _origStyleProperties.center.Y + (diffMouseY/2));
                                    unRotatedTopLeft =
                                        ShapeBehavior.CalculateContactPoint(rotatedTopLeft.X, rotatedTopLeft.Y,
                                                                            newCenter, -angle);
                                    left = unRotatedTopLeft.X;
                                    top = unRotatedTopLeft.Y;
                                    width = (newCenter.X - unRotatedTopLeft.X)*2;
                                    height = (newCenter.Y - unRotatedTopLeft.Y)*2;
                                }
                                else
                                {
                                    width = _origStyleProperties.width + diffMouseX;
                                    top = _origStyleProperties.top + diffMouseY;
                                    height = _origStyleProperties.height - diffMouseY;
                                }
                                break;
                            case eContactPoint.top:
                                if (angle != 0)
                                {
                                    // it is only the Y-Part of the movment need in the unrotated world, so calulate it and rotate it back
                                    // to calumate the movment like on the other corners
                                    newPoint =
                                        ShapeBehavior.CalculateContactPoint(0, diffTransY, new Point(0, 0), angle);
                                    rotatedTopLeft =
                                        ShapeBehavior.CalculateContactPoint(_origStyleProperties.topleft.X,
                                                                            _origStyleProperties.topleft.Y,
                                                                            _origStyleProperties.center,
                                                                            _origStyleProperties.angle);
                                    newCenter =
                                        new Point(_origStyleProperties.center.X + (newPoint.X/2),
                                                  _origStyleProperties.center.Y + (newPoint.Y/2));
                                    unRotatedTopLeft =
                                        ShapeBehavior.CalculateContactPoint(rotatedTopLeft.X + newPoint.X,
                                                                            rotatedTopLeft.Y + newPoint.Y, newCenter,
                                                                            -angle);
                                    left = unRotatedTopLeft.X;
                                    top = unRotatedTopLeft.Y;
                                    width = (newCenter.X - unRotatedTopLeft.X)*2;
                                    height = (newCenter.Y - unRotatedTopLeft.Y)*2;
                                }
                                else
                                {
                                    top = _origStyleProperties.top + diffMouseY;
                                    height = _origStyleProperties.height - diffMouseY;
                                }
                                break;
                            case eContactPoint.bottomleft:
                                if (angle != 0)
                                {
                                    rotatedTopLeft =
                                        ShapeBehavior.CalculateContactPoint(
                                            _origStyleProperties.topleft.X + diffTransX, _origStyleProperties.topleft.Y,
                                            _origStyleProperties.center, _origStyleProperties.angle);
                                    newCenter =
                                        new Point(_origStyleProperties.center.X + (diffMouseX/2),
                                                  _origStyleProperties.center.Y + (diffMouseY/2));
                                    unRotatedTopLeft =
                                        ShapeBehavior.CalculateContactPoint(rotatedTopLeft.X, rotatedTopLeft.Y,
                                                                            newCenter, -angle);
                                    left = unRotatedTopLeft.X;
                                    top = unRotatedTopLeft.Y;
                                    width = (newCenter.X - unRotatedTopLeft.X)*2;
                                    height = (newCenter.Y - unRotatedTopLeft.Y)*2;
                                }
                                else
                                {
                                    left = _origStyleProperties.left + diffMouseX;
                                    width = _origStyleProperties.width - diffMouseX;
                                    height = _origStyleProperties.height + diffMouseY;
                                }
                                break;
                            case eContactPoint.bottomrigth:
                                if (angle != 0)
                                {
                                    rotatedTopLeft =
                                        ShapeBehavior.CalculateContactPoint(_origStyleProperties.topleft.X,
                                                                            _origStyleProperties.topleft.Y,
                                                                            _origStyleProperties.center,
                                                                            _origStyleProperties.angle);
                                    newCenter =
                                        new Point(_origStyleProperties.center.X + (diffMouseX/2),
                                                  _origStyleProperties.center.Y + (diffMouseY/2));
                                    unRotatedTopLeft =
                                        ShapeBehavior.CalculateContactPoint(rotatedTopLeft.X, rotatedTopLeft.Y,
                                                                            newCenter, -angle);
                                    left = unRotatedTopLeft.X;
                                    top = unRotatedTopLeft.Y;
                                    width = (newCenter.X - unRotatedTopLeft.X)*2;
                                    height = (newCenter.Y - unRotatedTopLeft.Y)*2;
                                }
                                else
                                {
                                    width = _origStyleProperties.width + diffMouseX;
                                    height = _origStyleProperties.height + diffMouseY;
                                }
                                break;
                            case eContactPoint.bottom:
                                if (angle != 0)
                                {
                                    // it is only the Y-Part of the movment need in the unrotated world, so calulate it and rotate it back
                                    // to calumate the movment like on the other corners
                                    newPoint =
                                        ShapeBehavior.CalculateContactPoint(0, diffTransY, new Point(0, 0), angle);
                                    rotatedTopLeft =
                                        ShapeBehavior.CalculateContactPoint(_origStyleProperties.topleft.X,
                                                                            _origStyleProperties.topleft.Y,
                                                                            _origStyleProperties.center,
                                                                            _origStyleProperties.angle);
                                    newCenter =
                                        new Point(_origStyleProperties.center.X + (newPoint.X/2),
                                                  _origStyleProperties.center.Y + (newPoint.Y/2));
                                    unRotatedTopLeft =
                                        ShapeBehavior.CalculateContactPoint(rotatedTopLeft.X, rotatedTopLeft.Y,
                                                                            newCenter, -angle);
                                    left = unRotatedTopLeft.X;
                                    top = unRotatedTopLeft.Y;
                                    width = (newCenter.X - unRotatedTopLeft.X)*2;
                                    height = (newCenter.Y - unRotatedTopLeft.Y)*2;
                                }
                                else
                                {
                                    height = _origStyleProperties.height + diffMouseY;
                                }
                                break;
                            case eContactPoint.left:
                                if (angle != 0)
                                {
                                    // it is only the Y-Part of the movment need in the unrotated world, so calulate it and rotate it back
                                    // to calumate the movment like on the other corners
                                    newPoint =
                                        ShapeBehavior.CalculateContactPoint(diffTransX, 0, new Point(0, 0), angle);
                                    rotatedTopLeft =
                                        ShapeBehavior.CalculateContactPoint(_origStyleProperties.topleft.X,
                                                                            _origStyleProperties.topleft.Y,
                                                                            _origStyleProperties.center,
                                                                            _origStyleProperties.angle);
                                    newCenter =
                                        new Point(_origStyleProperties.center.X + (newPoint.X/2),
                                                  _origStyleProperties.center.Y + (newPoint.Y/2));
                                    unRotatedTopLeft =
                                        ShapeBehavior.CalculateContactPoint(rotatedTopLeft.X + newPoint.X,
                                                                            rotatedTopLeft.Y + newPoint.Y, newCenter,
                                                                            -angle);
                                    left = unRotatedTopLeft.X;
                                    top = unRotatedTopLeft.Y;
                                    width = (newCenter.X - unRotatedTopLeft.X)*2;
                                    height = (newCenter.Y - unRotatedTopLeft.Y)*2;
                                }
                                else
                                {
                                    left = _origStyleProperties.left + diffMouseX;
                                    width = _origStyleProperties.width - diffMouseX;
                                }
                                break;
                            case eContactPoint.right:
                                if (angle != 0)
                                {
                                    // it is only the Y-Part of the movment need in the unrotated world, so calulate it and rotate it back
                                    // to calumate the movment like on the other corners
                                    newPoint =
                                        ShapeBehavior.CalculateContactPoint(diffTransX, 0, new Point(0, 0), angle);
                                    rotatedTopLeft =
                                        ShapeBehavior.CalculateContactPoint(_origStyleProperties.topleft.X,
                                                                            _origStyleProperties.topleft.Y,
                                                                            _origStyleProperties.center,
                                                                            _origStyleProperties.angle);
                                    newCenter =
                                        new Point(_origStyleProperties.center.X + (newPoint.X/2),
                                                  _origStyleProperties.center.Y + (newPoint.Y/2));
                                    unRotatedTopLeft =
                                        ShapeBehavior.CalculateContactPoint(rotatedTopLeft.X, rotatedTopLeft.Y,
                                                                            newCenter, -angle);
                                    left = unRotatedTopLeft.X;
                                    top = unRotatedTopLeft.Y;
                                    width = (newCenter.X - unRotatedTopLeft.X)*2;
                                    height = (newCenter.Y - unRotatedTopLeft.Y)*2;
                                }
                                else
                                {
                                    width = _origStyleProperties.width + diffMouseX;
                                }
                                break;
                            case eContactPoint.rotate:
                                angle = _origStyleProperties.angle + diffMouseX;
                                break;
                            default:
                                // do nothing ;-)
                                break;
                        }
                        //							
                        //							_PrevMouse.X = _Mouse.X;
                        //                          _PrevMouse.Y = _Mouse.Y;
                        //
                        //                          _PrevTransMouse.X = _TransMouse.X;
                        //                          _PrevTransMouse.Y = _TransMouse.Y;

                        if (height < 10)
                        {
                            height = 10;
                            flipy = !flipy;
                        }
                        if (width < 10)
                        {
                            width = 10;
                            flipx = !flipx;
                        }

                        this.SetStyleAttribute(element, "flip", ((flipx) ? "x " : "") + ((flipy) ? "y" : ""));
                        this.SetStyleAttribute(element, "left", left.ToString() + "px");
                        this.SetStyleAttribute(element, "top", top.ToString() + "px");
                        this.SetStyleAttribute(element, "width", width.ToString() + "px");
                        this.SetStyleAttribute(element, "height", height.ToString() + "px");
                        this.SetStyleAttribute(element, "rotation", angle.ToString());
                        result = Interop.S_OK;
                    }
                    break;

                    #endregion

                default:
                    this.ResetCurrentPointer();
                    break;
            }

            // if nobody has handled the button (no contactpoint is moving) change the object to handle
            if ((this.movedContactPoint == eContactPoint.none) && this.isMouseDown)
            {
                this.lastVmlElement = this.GetVmlElement(eventObj);

                foreach (DictionaryEntry de in ShapeBehavior.BehaviorInstances)
                {
                    ((ShapeBehavior) de.Value).DrawHandles = false;
                }

                if (this.lastVmlElement != null)
                {
                    int i = lastVmlElement.GetHashCode();
                    if (ShapeBehavior.BehaviorInstances[i] == null)
                    {
                        ShapeBehavior.BehaviorInstances[i] = ShapeBehavior.InitElementBehavior(lastVmlElement);
                        // switch rotate handler on/off
                        ((ShapeBehavior) ShapeBehavior.BehaviorInstances[i]).HasRotateHandle = _canRotate;
                    }
                    ((ShapeBehavior) ShapeBehavior.BehaviorInstances[i]).DrawHandles = true;
                }

                this.ResetCurrentPointer();
            }

            if (_elementEvents)
            {
                result = Interop.S_FALSE;
            }
            return result;
        }

        int Interop.IHTMLEditDesigner.PostHandleEvent(int dispId, Interop.IHTMLEventObj eventObj) {
            return Interop.S_FALSE;
        }

        int Interop.IHTMLEditDesigner.TranslateAccelerator(int dispId, Interop.IHTMLEventObj eventObj) {
            return Interop.S_FALSE;
        }

        int Interop.IHTMLEditDesigner.PostEditorEventNotify(int dispId, Interop.IHTMLEventObj eventObj) {
            return Interop.S_FALSE;
        }

        #endregion

        private void _htmlEditor_ReadyStateComplete(object sender, EventArgs e) {
            VmlDesigner.EnsureVmlStyle(this._htmlEditor);
            
            //this.movedContactPoint = eContactPoint.none  ;
            //this.lastVmlElement = null ;
            //this.body2 = null;
            //bool style_exists = false;

            //GuruComponents.Netrix.WebEditing.Elements.HtmlElement html = (GuruComponents.Netrix.WebEditing.Elements.HtmlElement)_htmlEditor.GetElementsByTagName("html")[0];
            //html.SetAttribute("xmlns:v", "urn:schemas-microsoft-com:vml");
            //GuruComponents.Netrix.WebEditing.Documents.StyleElementCollection sc = (GuruComponents.Netrix.WebEditing.Documents.StyleElementCollection)_htmlEditor.DocumentStructure.EmbeddedStylesheets;
            //foreach (StyleElement s in sc) {
               
            //}
            //if (!style_exists) {
            //    StyleElement style = (StyleElement)this._htmlEditor.CreateElement("style");
            //    sc.Add(style);
            //    style.Content = @"v\:* { behavior: url(#default#VML); }";
            //}

        }
        private void VmlDesignerBehavior_MouseLeave(object sender, EventArgs e) {
            //// get the bounds of the client rect from the control, to check if the control is left
            //if (((System.Windows.Forms.Control)_htmlEditor).Parent != null) {
            //    System.Drawing.Point TL = ((System.Windows.Forms.Control)_htmlEditor).Parent.PointToScreen(new System.Drawing.Point(((System.Windows.Forms.Control)_htmlEditor).Left, ((System.Windows.Forms.Control)_htmlEditor).Top));
            //    System.Drawing.Point BR = ((System.Windows.Forms.Control)_htmlEditor).Parent.PointToScreen(new System.Drawing.Point(((System.Windows.Forms.Control)_htmlEditor).Left + ((System.Windows.Forms.Control)_htmlEditor).Width, ((System.Windows.Forms.Control)_htmlEditor).Top + ((System.Windows.Forms.Control)_htmlEditor).Height));

            //    // check if the control is left
            //    if (TL.Y >= System.Windows.Forms.Control.MousePosition.Y || BR.Y <= System.Windows.Forms.Control.MousePosition.Y ||
            //        TL.X >= System.Windows.Forms.Control.MousePosition.X || BR.X <= System.Windows.Forms.Control.MousePosition.X) {
            //        // Look the editor while the mouse is outside
            //        this.isMouseOutSide = true;
            //    }
            //}
        }

        private void VmlDesignerBehavior_MouseEnter(object sender, EventArgs e) {
            
        //    // check if the control is enterd
        //    if (this.isMouseOutSide){
        //        this.isMouseOutSide = false ;
        //        if (System.Windows.Forms.Control.MouseButtons == System.Windows.Forms.MouseButtons.Left){
        //            this.isMouseDown = true ;
        //        }else{
        //            // reset everything need for the move-detection
        //            this.isMouseDown = false;
        //            this.isMouseMove = false;
        //            this.movedContactPoint = eContactPoint.none ;
        //            _PrevMouse.X = -1;
        //            _PrevMouse.Y = -1;
        //            _PrevTransMouse.X = -1;
        //            _PrevTransMouse.Y = -1;
        //            ResetCurrentPointer();                                                    
        //            this.lastVmlElement = null ;
        //            this.isMouseOutSide = false ;
        //        }
        //    }
        }
    }
}