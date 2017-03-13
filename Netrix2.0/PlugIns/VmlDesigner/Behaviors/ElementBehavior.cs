using System;
using System.Globalization;
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;

using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix;
using GuruComponents.Netrix.VmlDesigner.DataTypes;
using GuruComponents.Netrix.VmlDesigner;
using GuruComponents.Netrix.VmlDesigner.Util;
using GuruComponents.Netrix.VmlDesigner.Elements;
using Comzept.Genesis.NetRix.VgxDraw;
using System.Web.UI;

namespace GuruComponents.Netrix.VmlDesigner.Behaviors {
    
    /// <summary>
    /// Holds the LineHandle behavior definitions.
    /// </summary>
    public class ShapeBehavior : GuruComponents.Netrix.WebEditing.Behaviors.IBaseBehavior {

		private readonly CultureInfo ci = new CultureInfo("en-us");
        internal static Hashtable BehaviorInstances;
        private bool drawHandles;
        private int cookie;
        private Interop.IHTMLElement2 body;
		private EventSink eventSink;
		private static ShapeBehavior eb;

        private bool hasRotateHandle;

        /// <summary>
        /// Get or set whether the shape has a rotater handle.
        /// </summary>
        public bool HasRotateHandle
        {
            get { return hasRotateHandle; }
            set { hasRotateHandle = value; }
        }
        private Color handleBorder;

        /// <summary>
        /// Get or set which color is used for the handle border.
        /// </summary>
        public Color HandleBorder
        {
            get { return handleBorder; }
            set { handleBorder = value; }
        }
        private Color handleFill;

        /// <summary>
        /// Get or set which color is used for the handles space.
        /// </summary>
        public Color HandleFill
        {
            get { return handleFill; }
            set { handleFill = value; }
        }
        private Color rotatorHandleFill;

        /// <summary>
        /// Get or set the color of the rotator handle.
        /// </summary>
        public Color RotatorHandleFill
        {
            get { return rotatorHandleFill; }
            set { rotatorHandleFill = value; }
        }
        private Color curveHandleFill;

        /// <summary>
        /// Get or set the color of the curve handle.
        /// </summary>
        public Color CurveHandleFill
        {
            get { return curveHandleFill; }
            set { curveHandleFill = value; }
        }
        private Color formHandleFill;

        /// <summary>
        /// Get or set the color of the form handle.
        /// </summary>
        public Color FormHandleFill
        {
            get { return formHandleFill; }
            set { formHandleFill = value; }
        }

        static ShapeBehavior() {
            BehaviorInstances = new Hashtable();
        }
        
        private ShapeBehavior() {
            handleBorder = Color.Black;
            handleFill = Color.White;
            rotatorHandleFill = Color.LightGreen;
            curveHandleFill = Color.LightBlue;
            formHandleFill = Color.Yellow;
            hasRotateHandle = true;
            this.p = new Pen(new SolidBrush(handleBorder));
            this.b = new SolidBrush(handleFill);
            this.b1 = new SolidBrush(rotatorHandleFill);
            this.b2 = new SolidBrush(curveHandleFill);
            this.b3 = new SolidBrush(formHandleFill);		
        }

        private string elementName;
        private Interop.IHTMLElement element;
        private Pen p;
        private Brush b;
        private Brush b1;
        private Brush b2;
        private Brush b3;
        private Interop.IHTMLPaintSite _paintsite;

        internal static ShapeBehavior InitElementBehavior(Interop.IHTMLElement element) {
			//System.Diagnostics.Debug.WriteLine(element.GetHashCode(), element.GetTagName());
			if (BehaviorInstances[element] == null)
			{
				BehaviorInstances[element] = new ShapeBehavior();
			} 
			ShapeBehavior.eb = (ShapeBehavior) BehaviorInstances[element];
            ShapeBehavior.eb.element = element;
            ShapeBehavior.eb.body = ((Interop.IHTMLDocument2)((Interop.IHTMLElement) element).GetDocument()).GetBody() as Interop.IHTMLElement2;
            ShapeBehavior.eb.elementName = element.GetTagName();
			//ElementBehavior.eb.eventSink = new EventSink(element);
            return ShapeBehavior.eb;
        }

		internal static ShapeBehavior Instance
		{
			get
			{
				return eb;
			}
		}

        /// <summary>
        /// Name of this behavior. ReadOnly.
        /// </summary>
        public string Name {
            get {
                return "VectorDesignerShapeHandles#" + elementName;
            }
        }

        #region Helper Methods

        /// <summary>
        /// Set style attribute for this element.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="styleName"></param>
        /// <param name="styleText"></param>
        public void SetStyleAttribute(Interop.IHTMLElement element, string styleName, string styleText) {
            Interop.IHTMLStyle style = (Interop.IHTMLStyle) element.GetStyle();
            style.SetAttribute(styleName.Replace("-", String.Empty), styleText, 0);
        }

        /// <summary>
        /// Return style attribute for element.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="styleName"></param>
        /// <returns></returns>
        public string GetStyleAttribute(Interop.IHTMLElement element, string styleName) {    
            Interop.IHTMLStyle style = (Interop.IHTMLStyle) element.GetStyle();
            object o = style.GetAttribute(styleName.Replace("-", String.Empty), 0);
            if (o == null) {
                return String.Empty;
            }
            else {
                string styleText = o.ToString();
                return styleText;
            }
        }
		
        public object GetAttribute(string attribute) {
            object local2;
            try {
                object[] locals = new object[1] { null };
                element.GetAttribute(attribute, 0, locals);
                object local1 = locals[0];
                if (local1 is DBNull) {
                    local1 = null;
                }
                local2 = local1;
            }
            catch {
                local2 = null;
            }
            return local2;
        }

        public bool DrawHandles {
            get {
                return drawHandles;
            }
            set {
                drawHandles = value;
                object oFactory = (object) this;                
                if (drawHandles) {                    
                    cookie = ((Interop.IHTMLElement2) element).AddBehavior(Name, ref oFactory);
                } 
                else {
                    ((Interop.IHTMLElement2) element).RemoveBehavior(cookie);
                }
            }
        }

        private Point Offset {
            get {
                return new Point(body.GetScrollLeft(), body.GetScrollTop());
            }
        }

        /// <summary>
        /// Rotate the Point anti-clockwards around the center Point cent
        /// </summary>
        /// <param name="X">point x coordinate</param>
        /// <param name="Y">point y coordinate</param>
        /// <param name="cent">rotations-center</param>
        /// <param name="deg">degrees to rotate</param>
        /// <returns>the new rotated Point</returns>
        /// <remarks>returns only a new point if the angle is zero</remarks>
        public static System.Drawing.Point CalculateContactPoint(int X, int Y, System.Drawing.Point cent, double deg) {
            if (deg == 0){
                return new System.Drawing.Point(X,Y);
            }
            deg = deg * Math.PI / 180; // need to convert DEG into RAD first !
            return new System.Drawing.Point( 
                Convert.ToInt32(((X - cent.X) * Math.Cos(deg)) - ((Y - cent.Y) * Math.Sin(deg)) + cent.X),
                Convert.ToInt32(((X - cent.X) * Math.Sin(deg)) + ((Y - cent.Y) * Math.Cos(deg)) + cent.Y));
        }

        # endregion

        # region IHTMLPainter Member

        void Interop.IHTMLPainter.Draw(int leftBounds, int topBounds, int rightBounds, int bottomBounds, int leftUpdate, int topUpdate, int rightUpdate, int bottomUpdate, int lDrawFlags, IntPtr hdc, IntPtr pvDrawObject) {
            if (!drawHandles) {
                this._paintsite.InvalidateRect(IntPtr.Zero);
            } 
            else {

                System.Drawing.Point center  ;
                System.Drawing.Point topleft ;
                System.Drawing.Point topright ;
                System.Drawing.Point topmiddle ;
                System.Drawing.Point bottommiddle ;
                System.Drawing.Point bottomleft ;
                System.Drawing.Point bottomright ;
                System.Drawing.Point middleleft ;
                System.Drawing.Point middleright ;
                System.Drawing.Point rotator ;

                Graphics graphics = Graphics.FromHdc(hdc);

                int scrollLeft = this.body.GetScrollLeft();
                int scrollTop = this.body.GetScrollTop();

                // Get Style-Attributes which are valid for all elements
                //int left = (Convert.ToInt32((GetStyleAttribute(element, "left").Equals(String.Empty)) ? 0 : System.Web.UI.WebControls.Unit.Parse(GetStyleAttribute(element, "left")).Value)) - scrollLeft ;
                //int top = (Convert.ToInt32((GetStyleAttribute(element, "top").Equals(String.Empty)) ? 0 : System.Web.UI.WebControls.Unit.Parse(GetStyleAttribute(element, "top")).Value)) - scrollTop ;
                //int width = Convert.ToInt32((GetStyleAttribute(element, "width").Equals(String.Empty)) ? 0 : System.Web.UI.WebControls.Unit.Parse(GetStyleAttribute(element, "width")).Value);
                //int height = Convert.ToInt32((GetStyleAttribute(element, "height").Equals(String.Empty)) ? 0 : System.Web.UI.WebControls.Unit.Parse(GetStyleAttribute(element, "height")).Value);
                int left = element.GetStyle().GetPixelLeft() - scrollLeft;
                int top = element.GetStyle().GetPixelTop() - scrollTop;
                int width = element.GetStyle().GetPixelWidth();
                int height = element.GetStyle().GetPixelHeight();
                bool flipx = ((GetStyleAttribute(element, "flip")).ToLower().IndexOf("x") != -1) ;
                bool flipy = ((GetStyleAttribute(element, "flip")).ToLower().IndexOf("y") != -1) ;
                double angle = Convert.ToDouble((GetStyleAttribute(element, "rotation").Equals(String.Empty)) ? 0 : System.Web.UI.WebControls.Unit.Parse(GetStyleAttribute(element, "rotation")).Value);
               
                // calcumate special attributes for rect/curve/arc
                int right = left + width ;
                int bottom = top + height ;

                //                // get some body properties
                //                string tmp = (string)((Interop.IHtmlBodyElement)this.body).leftMargin;
                //                int leftMargin = Convert.ToInt32((tmp.Equals(String.Empty)) ? 0 : System.Web.UI.WebControls.Unit.Parse(tmp).Value);
                //                tmp = (string)((Interop.IHtmlBodyElement)this.body).topMargin;
                //                int topMargin = Convert.ToInt32((tmp.Equals(String.Empty)) ? 0 : System.Web.UI.WebControls.Unit.Parse(tmp).Value);

                
                int magicOffset = 10 ;
                
                // note: the style attributes (top,left) has to be added to the other attributes(ftom,to,...)
                //       the style attributes are indipendent of the body-border, the other not
                
                   
                // Make Global Konfigurable (Show Border around Rect Surface)
                switch (elementName) {
                    case "curve":
                        VgVector2D f1 = new VgVector2D((IVgVector2D) GetAttribute("from"));
                        VgVector2D t1 = new VgVector2D((IVgVector2D) GetAttribute("to"));
               
                        VgVector2D c1 = new VgVector2D((IVgVector2D) GetAttribute("control1"));
                        VgVector2D c2 = new VgVector2D((IVgVector2D) GetAttribute("control2"));		                
                
                        graphics.FillEllipse(b, left + f1.Pixel.X - 3 , top + f1.Pixel.Y - 3 , 6, 6);
                        graphics.FillEllipse(b, left + t1.Pixel.X - 3 , top + t1.Pixel.Y - 3 , 6, 6);
                        graphics.DrawEllipse(p, left + f1.Pixel.X - 3 , top + f1.Pixel.Y - 3 , 6, 6);
                        graphics.DrawEllipse(p, left + t1.Pixel.X - 3 , top + t1.Pixel.Y - 3 , 6, 6);

                        graphics.FillEllipse(b2, left + c1.Pixel.X - 3 , top + c1.Pixel.Y - 3 , 6, 6);
                        graphics.FillEllipse(b2, left + c2.Pixel.X - 3 , top + c2.Pixel.Y - 3 , 6, 6);
                        graphics.DrawEllipse(p, left + c1.Pixel.X - 3 , top + c1.Pixel.Y - 3 , 6, 6);
                        graphics.DrawEllipse(p, left + c2.Pixel.X - 3 , top + c2.Pixel.Y - 3 , 6, 6);
                        break;
                    case "line":
                        VgVector2D f = new VgVector2D((IVgVector2D) GetAttribute("from"));
                        VgVector2D t = new VgVector2D((IVgVector2D) GetAttribute("to"));

                        graphics.FillEllipse(b, left + f.Pixel.X - 3 , top + f.Pixel.Y - 3 , 6, 6);
                        graphics.FillEllipse(b, left + t.Pixel.X - 3 , top + t.Pixel.Y - 3 , 6, 6);
                        graphics.DrawEllipse(p, left + f.Pixel.X - 3 , top + f.Pixel.Y - 3 , 6, 6);
                        graphics.DrawEllipse(p, left + t.Pixel.X - 3 , top + t.Pixel.Y - 3 , 6, 6);
                        break;
                    case "polyline":
                        VgPoints points = new VgPoints((IVgPoints) GetAttribute("points"));
                        for (int i=0; i < points.length; i++){
                            VgVector2D pt = new VgVector2D(points[i]);
                            graphics.FillEllipse(b, left+pt.Pixel.X - 3 , top+pt.Pixel.Y - 3 + magicOffset, 6, 6);
                            graphics.DrawEllipse(p, left+pt.Pixel.X - 3 , top+pt.Pixel.Y - 3 + magicOffset, 6, 6);
                        }
                        break;
                    case "arc":
                        // calulate contact points
                        center = new System.Drawing.Point((left+right)/2,(top+bottom)/2);
                        topleft = CalculateContactPoint(left,top,center,angle);
                        topright = CalculateContactPoint(right,top,center,angle);
                        topmiddle = CalculateContactPoint((left + right)/2,top,center,angle);
                        bottommiddle = CalculateContactPoint((left + right)/2,bottom,center,angle);
                        bottomleft = CalculateContactPoint(left,bottom,center,angle);
                        bottomright = CalculateContactPoint(right,bottom,center,angle);
                        middleleft = CalculateContactPoint(left,(top + bottom)/2,center,angle);
                        middleright = CalculateContactPoint(right,(top + bottom)/2,center,angle);

                        rotator = CalculateContactPoint((left + right)/2,top-15,center,angle);

                        // two additional contact points for the start and end angle
                        double startAngle = Math.PI / 180 *(Convert.ToInt32((GetAttribute( "startangle").Equals(String.Empty)) ? "0" : GetAttribute( "startangle")));
                        double endAngle = Math.PI / 180 *(Convert.ToInt32((GetAttribute( "endangle").Equals(String.Empty)) ? "0" : GetAttribute( "endangle"))) ;
                
                        System.Drawing.Point ctrl1 = CalculateContactPoint((int)(center.X+(width/2 * Math.Sin(startAngle))) ,(int)(center.Y-(height/2 * Math.Cos(startAngle))) ,center,angle); 
                        System.Drawing.Point ctrl2 = CalculateContactPoint((int)(center.X+(width/2 * Math.Sin(endAngle))) ,(int)(center.Y-(height/2 * Math.Cos(endAngle))) ,center,angle); 
                        
                        // draw points
                        graphics.DrawLine(p,topmiddle.X , topmiddle.Y ,rotator.X ,rotator.Y );

                        graphics.FillEllipse(b, topleft.X - 3, topleft.Y - 3, 6, 6);
                        graphics.FillEllipse(b, topright.X - 3, topright.Y - 3 , 6, 6);
                        graphics.FillEllipse(b, topmiddle.X - 3, topmiddle.Y - 3, 6, 6);
                        graphics.FillEllipse(b, bottommiddle.X -3 , bottommiddle.Y - 3 , 6, 6);
                        graphics.FillEllipse(b, bottomleft.X - 3, bottomleft.Y - 3 , 6, 6);
                        graphics.FillEllipse(b, bottomright.X - 3, bottomright.Y -3 , 6, 6);
                        graphics.FillEllipse(b, middleleft.X - 3,middleleft.Y - 3, 6, 6);
                        graphics.FillEllipse(b, middleright.X -3,middleright.Y - 3 , 6, 6);
                        if (HasRotateHandle)
                        {
                            graphics.FillEllipse(b1, rotator.X - 3, rotator.Y - 3, 6, 6);
                        }
                        graphics.FillEllipse(b3, ctrl1.X -3,ctrl1.Y - 3 , 6, 6);
                        graphics.FillEllipse(b3, ctrl2.X -3,ctrl2.Y - 3 , 6, 6);
                        
                        graphics.DrawEllipse(p, topleft.X - 3, topleft.Y - 3, 6, 6);
                        graphics.DrawEllipse(p, topright.X - 3, topright.Y - 3 , 6, 6);
                        graphics.DrawEllipse(p, topmiddle.X - 3, topmiddle.Y - 3, 6, 6);
                        graphics.DrawEllipse(p, bottommiddle.X -3 , bottommiddle.Y - 3 , 6, 6);
                        graphics.DrawEllipse(p, bottomleft.X - 3, bottomleft.Y - 3 , 6, 6);
                        graphics.DrawEllipse(p, bottomright.X - 3, bottomright.Y -3 , 6, 6);
                        graphics.DrawEllipse(p, middleleft.X - 3,middleleft.Y - 3, 6, 6);
                        graphics.DrawEllipse(p, middleright.X -3,middleright.Y - 3 , 6, 6);
                        if (HasRotateHandle)
                        {
                            graphics.DrawEllipse(p, rotator.X - 3, rotator.Y - 3, 6, 6);
                        }
                        graphics.DrawEllipse(p, ctrl1.X -3,ctrl1.Y - 3 , 6, 6);
                        graphics.DrawEllipse(p, ctrl2.X -3,ctrl2.Y - 3 , 6, 6);

                        break;
                    case "oval":
                    case "roundrect":
                    case "rect":
                    case "shape":
                        // calulate contact points
                        center = new System.Drawing.Point((left+right)/2,(top+bottom)/2);
                        topleft = CalculateContactPoint(left,top,center,angle);
                        topright = CalculateContactPoint(right,top,center,angle);
                        topmiddle = CalculateContactPoint((left + right)/2,top,center,angle);
                        bottommiddle = CalculateContactPoint((left + right)/2,bottom,center,angle);
                        bottomleft = CalculateContactPoint(left,bottom,center,angle);
                        bottomright = CalculateContactPoint(right,bottom,center,angle);
                        middleleft = CalculateContactPoint(left,(top + bottom)/2,center,angle);
                        middleright = CalculateContactPoint(right,(top + bottom)/2,center,angle);
                        if (HasRotateHandle)
                        {
                            rotator = CalculateContactPoint((left + right) / 2, top - 15, center, angle);
                            graphics.DrawLine(p,topmiddle.X , topmiddle.Y ,rotator.X ,rotator.Y );
                            graphics.FillEllipse(b1, rotator.X - 3, rotator.Y - 3, 6, 6);
                            graphics.DrawEllipse(p, rotator.X - 3, rotator.Y - 3, 6, 6);
                        }
                        // draw points

                        graphics.FillEllipse(b, topleft.X - 3, topleft.Y - 3, 6, 6);
                        graphics.FillEllipse(b, topright.X - 3, topright.Y - 3 , 6, 6);
                        graphics.FillEllipse(b, topmiddle.X - 3, topmiddle.Y - 3, 6, 6);
                        graphics.FillEllipse(b, bottommiddle.X -3 , bottommiddle.Y - 3 , 6, 6);
                        graphics.FillEllipse(b, bottomleft.X - 3, bottomleft.Y - 3 , 6, 6);
                        graphics.FillEllipse(b, bottomright.X - 3, bottomright.Y -3 , 6, 6);
                        graphics.FillEllipse(b, middleleft.X - 3,middleleft.Y - 3, 6, 6);
                        graphics.FillEllipse(b, middleright.X -3,middleright.Y - 3 , 6, 6);

                        graphics.DrawEllipse(p, topleft.X - 3, topleft.Y - 3, 6, 6);
                        graphics.DrawEllipse(p, topright.X - 3, topright.Y - 3 , 6, 6);
                        graphics.DrawEllipse(p, topmiddle.X - 3, topmiddle.Y - 3, 6, 6);
                        graphics.DrawEllipse(p, bottommiddle.X -3 , bottommiddle.Y - 3 , 6, 6);
                        graphics.DrawEllipse(p, bottomleft.X - 3, bottomleft.Y - 3 , 6, 6);
                        graphics.DrawEllipse(p, bottomright.X - 3, bottomright.Y -3 , 6, 6);
                        graphics.DrawEllipse(p, middleleft.X - 3,middleleft.Y - 3, 6, 6);
                        graphics.DrawEllipse(p, middleright.X -3,middleright.Y - 3 , 6, 6);

                        break;
                }
                graphics.Dispose();
            }
        }

        void Interop.IHTMLPainter.OnResize(int cx, int cy) {
        }

        void Interop.IHTMLPainter.GetPainterInfo(Interop.HTML_PAINTER_INFO htmlPainterInfo) {
            htmlPainterInfo.lFlags = 0x2;
            htmlPainterInfo.lZOrder = 7;
            htmlPainterInfo.iidDrawObject = Guid.Empty;

            // Get Style-Attributes which are valid for all elements
            //int left = Convert.ToInt32((GetStyleAttribute(element, "left").Equals(String.Empty)) ? 0 : System.Web.UI.WebControls.Unit.Parse(GetStyleAttribute(element, "left"), ci).Value);
            //int top = Convert.ToInt32((GetStyleAttribute(element, "top").Equals(String.Empty)) ? 0 : System.Web.UI.WebControls.Unit.Parse(GetStyleAttribute(element, "top"), ci).Value) ;
            //int width = Convert.ToInt32((GetStyleAttribute(element, "width").Equals(String.Empty)) ? 0 : System.Web.UI.WebControls.Unit.Parse(GetStyleAttribute(element, "width"), ci).Value);
            //int height = Convert.ToInt32((GetStyleAttribute(element, "height").Equals(String.Empty)) ? 0 : System.Web.UI.WebControls.Unit.Parse(GetStyleAttribute(element, "height"), ci).Value);
            int left = element.GetStyle().GetPixelLeft();
            int top = element.GetStyle().GetPixelTop();
            int width = element.GetStyle().GetPixelWidth();
            int height = element.GetStyle().GetPixelHeight();
            double angle = Convert.ToDouble((GetStyleAttribute(element, "rotation").Equals(String.Empty)) ? 0 : System.Web.UI.WebControls.Unit.Parse(GetStyleAttribute(element, "rotation"), ci).Value);

            switch (elementName){
                case "oval":
                    // calulate the real edges
                    // TODO: should calculate the needed bounds here 
                    int a = (width > height) ? width : height ;
                    int b = (width > height) ? height : width ;
                    int dx = Convert.ToInt32(a - (Math.Sqrt(Math.Pow(a,2)-Math.Pow(b,2)))) ;
                    int dy = Convert.ToInt32(b - (Math.Pow(b,2)/a)) ;
                    dx = (dy > dx) ? dy + 10: dx + 10 ;
                    htmlPainterInfo.rcBounds = new Interop.RECT(dx, dx, dx, dx);
                    break;
                case "shape":                    
					htmlPainterInfo.rcBounds = new Interop.RECT(30, 30, 30, 30);
					break;
                case "roundrect":
                case "rect":
                    // TODO: should calculate the needed bounds here 
                    htmlPainterInfo.rcBounds = new Interop.RECT(30, 30, 30, 30);
                    break;
                case "arc":
                    // TODO: should calculate the needed bounds here 
                    htmlPainterInfo.rcBounds = new Interop.RECT(100, 100, 100, 100);
                    break;
                default:
                    // assuming the default overlap is less then 3 px
                    htmlPainterInfo.rcBounds = new Interop.RECT(6, 6, 6, 6);
                    break;
            }
        }

        void Interop.IHTMLPainter.HitTestPoint(int ptx, int pty, out bool hitTest, out int plPartID) {
            //System.Diagnostics.Debug.WriteLine(ptx.ToString() + " " + pty.ToString(), "Hittest");
            hitTest = false;
            plPartID = 0;
        }

        void Interop.IElementBehavior.Init(Interop.IElementBehaviorSite behaviorSite) {
            System.Diagnostics.Debug.WriteLine("Init");
            _paintsite = (Interop.IHTMLPaintSite) behaviorSite;
        }

        void Interop.IElementBehavior.Notify(int eventId, IntPtr pVar) {
            System.Diagnostics.Debug.WriteLine("Notify");
			if (eventId == 0)
			{
				if (element != null && eventSink == null)
				{
					eventSink = new EventSink(element);
					eventSink.Connect();
				}				
			}
        }

        void Interop.IElementBehavior.Detach() {
            System.Diagnostics.Debug.WriteLine("Detach");
//			eventSink.Disconnect();
//			eventSink = null;
        }


        # endregion

		# region Event Management
		/// <summary>
		/// The purpose of this class is to deal with the events a control will
		/// fire at design time.
		/// </summary>
		private sealed class EventSink
		{
                      
			private ConnectionPointCookie _eventSinkCookie;
			private Interop.IHTMLElement _baseElement;

			private class HtmlEvents
			{

				protected Interop.IHTMLEventObj _eventobj;
				protected Interop.IHTMLElement _baseElement;
				private Interop.IHTMLWindow2 window;
				protected Interop.IHTMLDocument2 document;

				protected HtmlEvents(Interop.IHTMLElement _element)
				{
					this._baseElement = _element; 
					document = (Interop.IHTMLDocument2) this._baseElement.GetDocument();
					window = document.GetParentWindow();
				}

				protected bool GetEventObject()
				{                     
					try
					{        
						// native event object				
						_eventobj = window.@event;
						if (_eventobj != null && _eventobj.srcElement != null)
						{
							return true;
						}
					}
					catch (Exception ex)
					{
						System.Diagnostics.Debug.WriteLine(ex.Message, "GetEvent::Exception");
					}
					return false;
				}

			}

			/// <summary>
			/// Generic event invocation, curently not used.
			/// </summary>
			private class HtmlGenericEvent : HtmlEvents, Interop.IHTMLGenericEvents
			{

				private static bool ForcedSelection = false;

				public HtmlGenericEvent(Interop.IHTMLElement e) : base(e)
				{
					//ToggleSelection = false;
				}

				#region IHTMLGenericEvents Member

				public void Bogus1()
				{
				}

				public void Bogus2()
				{
				}

				public void Bogus3()
				{
				}

				public void Invoke(int id, ref Guid g, int lcid, int dwFlags, GuruComponents.Netrix.ComInterop.Interop.DISPPARAMS pdp, Object[] pvarRes, GuruComponents.Netrix.ComInterop.Interop.EXCEPINFO pei, int[] nArgError)
				{						
					GetEventObject();
					{
						//System.Diagnostics.Debug.WriteLine(_eventobj.type, "Generic Type On: " + _eventobj.srcElement.GetTagName());
					}
					if (_eventobj == null) return;
                    switch (_eventobj.type)
					{
							# region Unused
//						case "help":
//						{
//							//((VmlBaseElement) _element).InvokeOnHelp(this, new DocumentEventArgs(, _element));
//							break;
//						}
//
//						case "click":
//						{
//							if (((VmlBaseElement) _element).OnClick != null && GetEventObject())
//							{
//								((VmlBaseElement) _element).InvokeClick(_eventobj, _element);
//							}
//							break;
//						}
//
//						case "dblclick":
//						{
//							if (((VmlBaseElement) _element).OnDblClick != null && GetEventObject())
//							{
//								((VmlBaseElement) _element).InvokeDblClick(_eventobj, _element);
//							}
//							break;
//						}
//
//						case "keypress":
//						{
//							if (GetEventObject())
//							{
//								((VmlBaseElement) _element).InvokeKeyDown(_eventobj, _element);                    
//								((VmlBaseElement) _element).InvokeKeyUp(_eventobj, _element);
//								((VmlBaseElement) _element).InvokeKeyPress(_eventobj, _element);
//							}
//							break;
//						}
//
//						case "keydown":
//						{
//							// see onkeypress":
//							break;
//						}
//
//						case "keyup":
//						{
//							// see onkeypress":
//							break;
//						}
//
//						case "mouseout":
//						{                    
//							((VmlBaseElement) _element).InvokeMouseOut(_eventobj, _element);
//							break;
//						}
//
//						case "mouseover":
//						{                    
//							((VmlBaseElement) _element).InvokeMouseOver(_eventobj, _element);
//							break;
//						}
//
//						case "mousemove":
//						{   
//							if (((VmlBaseElement) _element).OnMouseMove != null)
//							{
//								((VmlBaseElement) _element).InvokeMouseMove(_eventobj, _element);
//							}
//							break;
//						}
//
//						case "mousedown":
//						{    
//							if (((VmlBaseElement) _element).OnMouseDown != null && GetEventObject())
//							{
//								((VmlBaseElement) _element).InvokeMouseDown(_eventobj, _element);
//							}
//							break;
//						}
//
//						case "mouseup":
//						{                    
//							if (((VmlBaseElement) _element).OnMouseUp != null && GetEventObject())
//							{
//								((VmlBaseElement) _element).InvokeMouseUp(_eventobj, _element);
//							}
//							break;
//						}
//
//						case "selectstart":
//						{                    
//							if (((VmlBaseElement) _element).OnSelectStart != null && GetEventObject())
//							{
//								((VmlBaseElement) _element).InvokeSelectStart(_eventobj, _element);
//							}
//							break;
//						}
//
//						case "filterchange":
//						{         
//							if (((VmlBaseElement) _element).OnFilterChange != null && GetEventObject())
//							{
//								((VmlBaseElement) _element).InvokeFilterChange(_eventobj, _element);
//							}
//							break;
//						}
//
//						case "dragstart":
//						{    
//							if (((VmlBaseElement) _element).OnDragStart != null && GetEventObject())
//							{
//								((VmlBaseElement) _element).InvokeDragStart(_eventobj, _element);
//							}
//							break;
//						}
//
//						case "beforeupdate":
//						{                    
//							//((VmlBaseElement) _element).InvokeBeforeUpdate(this, new DocumentEventArgs(, _element));
//							break;
//						}
//
//						case "afterupdate":
//						{
//							//((VmlBaseElement) _element).InvokeAfterUpdate(this, new DocumentEventArgs(, _element));
//							break;
//						}
//
//						case "errorupdate":
//						{
//							//((VmlBaseElement) _element).InvokeErrorUpdate(this, new DocumentEventArgs(, _element));
//							break;
//						}
//
//						case "rowexit":
//						{
//							//((VmlBaseElement) _element).InvokeRowExit(this, new DocumentEventArgs(, _element));
//							break;
//						}
//
//						case "rowenter":
//						{
//							//((VmlBaseElement) _element).InvokeRowEnter(this, new DocumentEventArgs(, _element));
//							break;
//						}
//
//						case "datasetchanged":
//						{
//							//((VmlBaseElement) _element).InvokeDatasetChanged(this, new DocumentEventArgs(, _element));
//							break;
//						}
//
//						case "dataavailable":
//						{
//							//((VmlBaseElement) _element).InvokeDataAvailable(this, new DocumentEventArgs(, _element));
//							break;
//						}
//
//						case "datasetcomplete":
//						{
//							//((VmlBaseElement) _element).InvokeDatasetComplete(this, new DocumentEventArgs(, _element));
//							break;
//						}
//
//						case "losecapture":
//						{         
//							if (((VmlBaseElement) _element).OnLoseCapture != null && GetEventObject())
//							{
//								((VmlBaseElement) _element).InvokeLoseCapture(_eventobj, _element);
//							}
//							break;
//						}
//
//						case "propertychange":
//						{                   
//							if (((VmlBaseElement) _element).OnPropertyChange != null && GetEventObject())
//							{
//								((VmlBaseElement) _element).InvokePropertyChange(_eventobj, _element);
//							}
//							break;
//						}
//
//						case "scroll":
//						{    
//							if (((VmlBaseElement) _element).OnScroll != null && GetEventObject())
//							{
//								((VmlBaseElement) _element).InvokeScroll(_eventobj, _element);
//							}
//							break;
//						}
//
//						case "focus":
//						{                    
//							if (((VmlBaseElement) _element).OnFocus != null && GetEventObject())
//							{
//								((VmlBaseElement) _element).InvokeFocus(_eventobj, _element);
//							}
//							break;
//						}
//
//						case "blur":
//						{                    
//							if (((VmlBaseElement) _element).OnBlur != null && GetEventObject())
//							{
//								((VmlBaseElement) _element).InvokeBlur(_eventobj, _element);
//							}
//							break;
//						}
//
//						case "resize":
//						{           
//							if (((VmlBaseElement) _element).OnResize != null && GetEventObject())
//							{
//								((VmlBaseElement) _element).InvokeResize(_eventobj, _element);
//							}
//							break;
//						}
//
//						case "drag":
//						{
//							if (((VmlBaseElement) _element).OnDrag != null && GetEventObject())
//							{
//								((VmlBaseElement) _element).InvokeDrag(_eventobj, _element);
//							}
//							break;
//						}
//
//						case "dragend":
//						{                    
//							if (((VmlBaseElement) _element).OnDragEnd != null && GetEventObject())
//							{
//								((VmlBaseElement) _element).InvokeDragEnd(_eventobj, _element);
//							}
//							break;
//						}
//
//						case "dragenter":
//						{
//							if (((VmlBaseElement) _element).OnDragEnter != null && GetEventObject())
//							{
//								((VmlBaseElement) _element).InvokeDragEnter(_eventobj, _element);
//							}
//							break;
//						}
//
//						case "dragover":
//						{                    
//							if (((VmlBaseElement) _element).OnDragOver != null && GetEventObject())
//							{
//								((VmlBaseElement) _element).InvokeDragOver(_eventobj, _element);
//							}
//							break;
//						}
//
//						case "dragleave":
//						{                    
//							if (((VmlBaseElement) _element).OnDragLeave != null && GetEventObject())
//							{
//								((VmlBaseElement) _element).InvokeDragLeave(_eventobj, _element);
//							}
//							break;
//						}
//
//						case "drop":
//						{                    
//							if (((VmlBaseElement) _element).OnDrop != null && GetEventObject())
//							{
//								((VmlBaseElement) _element).InvokeDrop(_eventobj, _element);
//							}
//							break;
//						}
//
//						case "beforecut":
//						{                    
//							if (((VmlBaseElement) _element).OnBeforeCut != null && GetEventObject())
//							{
//								((VmlBaseElement) _element).InvokeBeforeCut(_eventobj, _element);
//							}
//							break;
//						}
//
//						case "cut":
//						{                    
//							if (((VmlBaseElement) _element).OnCut != null && GetEventObject())
//							{
//								((VmlBaseElement) _element).InvokeCut(_eventobj, _element);
//							}
//							break;
//						}
//
//						case "beforecopy":
//						{                    
//							if (((VmlBaseElement) _element).OnBeforeCopy != null && GetEventObject())
//							{
//								((VmlBaseElement) _element).InvokeBeforeCopy(_eventobj, _element);
//							}
//							break;
//						}
//
//						case "copy":
//						{                    
//							if (((VmlBaseElement) _element).OnCopy != null && GetEventObject())
//							{
//								((VmlBaseElement) _element).InvokeCopy(_eventobj, _element);
//							}
//							break;
//						}
//
//						case "beforepaste":
//						{
//							if (((VmlBaseElement) _element).OnBeforePaste != null && GetEventObject())
//							{
//								((VmlBaseElement) _element).InvokeBeforePaste(_eventobj, _element);
//							}
//							break;
//						}
//
//						case "paste":
//						{                    
//							if (((VmlBaseElement) _element).OnPaste != null && GetEventObject())
//							{
//								((VmlBaseElement) _element).InvokePaste(_eventobj, _element);
//							}
//							break;
//						}
//
//						case "contextmenu":
//						{                  
//							if (((VmlBaseElement) _element).OnContextMenu != null && GetEventObject())
//							{
//								((VmlBaseElement) _element).InvokeContextMenu(_eventobj, _element);
//							}
//							break;
//						}
//
//						case "rowsdelete":
//						{
//							//((VmlBaseElement) _element).InvokeRowsDelete(this, new DocumentEventArgs(, _element));
//							break;
//						}
//
//						case "rowsinserted":
//						{
//							//((VmlBaseElement) _element).InvokeRowsInserted(this, new DocumentEventArgs(, _element));
//							break;
//						}
//
//						case "cellchange":
//						{
//							//((VmlBaseElement) _element).InvokeCellChange(this, new DocumentEventArgs(, _element));
//							break;
//						}
//
//						case "readystatechange":
//						{
//							//((VmlBaseElement) _element).InvokeReadyStateChange(this, new DocumentEventArgs(, _element));
//							break;
//						}
//
//						case "beforeeditfocus":
//						{                    
//							if (((VmlBaseElement) _element).OnEditFocus != null && GetEventObject())
//							{
//								((VmlBaseElement) _element).InvokeEditFocus(_eventobj, _element);
//							}
//							break;
//						}
//
//						case "layoutcomplete":
//						{                    
//							if (((VmlBaseElement) _element).OnLayoutComplete != null && GetEventObject())
//							{
//								((VmlBaseElement) _element).InvokeLayoutComplete(_eventobj, _element);
//							}
//							break;
//						}
//
//						case "page":
//						{   
//							if (((VmlBaseElement) _element).OnPage != null && GetEventObject())
//							{
//								((VmlBaseElement) _element).InvokePage(_eventobj, _element);
//							}
//							break;
//						}
//
//						case "beforedeactivate":
//						{                    
//							if (((VmlBaseElement) _element).OnBeforeDeactivate != null && GetEventObject())
//							{
//								((VmlBaseElement) _element).InvokeBeforeDeactivate(_eventobj, _element);
//							}
//							break;
//						}
//
//						case "beforeactivate":
//						{
//							if (((VmlBaseElement) _element).OnBeforeActivate != null && GetEventObject())
//							{
//								((VmlBaseElement) _element).InvokeBeforeActivate(_eventobj, _element);
//							}
//							break;
//						}
//
//						case "move":
//						{    
//							if (((VmlBaseElement) _element).OnMove != null && GetEventObject())
//							{
//								((VmlBaseElement) _element).InvokeMove(_eventobj, _element);
//							}
//							break;
//						}
//
//
//						case "movestart":
//						{                    
//							if (((VmlBaseElement) _element).OnMoveStart != null && GetEventObject())
//							{
//								((VmlBaseElement) _element).InvokeMoveStart(_eventobj, _element);
//							}
//							break;
//						}
//
//						case "moveend":
//						{                    
//							if (((VmlBaseElement) _element).OnMoveEnd != null && GetEventObject())
//							{
//								((VmlBaseElement) _element).InvokeMoveEnd(_eventobj, _element);
//							}
//							break;
//						}
//
//						case "resizestart":
//						{
//							if (((VmlBaseElement) _element).OnResizeStart != null && GetEventObject())
//							{
//								((VmlBaseElement) _element).InvokeResizeStart(_eventobj, _element);
//							}
//							break;
//						}
//
//						case "resizeend":
//						{
//							if (((VmlBaseElement) _element).OnResizeEnd != null && GetEventObject())
//							{
//								((VmlBaseElement) _element).InvokeResizeEnd(_eventobj, _element);
//							}
//							break;
//						}
//
//						case "mouseenter":
//						{   
//							if (((VmlBaseElement) _element).OnMouseEnter != null && GetEventObject())
//							{
//								((VmlBaseElement) _element).InvokeMouseEnter(_eventobj, _element);
//							}
//							break;
//						}
//
//						case "mouseleave":
//						{
//							if (((VmlBaseElement) _element).OnMouseLeave != null && GetEventObject())
//							{
//								((VmlBaseElement) _element).InvokeMouseLeave(_eventobj, _element);
//							}
//							break;
//						}
//
//						case "mousewheel":
//						{
//							if (((VmlBaseElement) _element).OnMouseWheel != null && GetEventObject())
//							{
//								((VmlBaseElement) _element).InvokeMouseWheel(_eventobj, _element);
//							}
//							break;
//						}
//
//						case "activate":
//						{
//							if (((VmlBaseElement) _element).OnActivate != null && GetEventObject())
//							{
//								((VmlBaseElement) _element).InvokeActivate(_eventobj, _element);
//							}
//							break;
//						}
//
//						case "deactivate":
//						{
//							if (((VmlBaseElement) _element).OnDeactivate != null && GetEventObject())
//							{
//								((VmlBaseElement) _element).InvokeDeactivate(_eventobj, _element);
//							}
//							break;
//						}
//
//						case "focusin":
//						{
//							if (((VmlBaseElement) _element).OnFocusIn != null && GetEventObject())
//							{
//								((VmlBaseElement) _element).InvokeFocusIn(_eventobj, _element);
//							}
//							break;
//						}
//
//						case "focusout":
//						{
//							if (((VmlBaseElement) _element).OnFocusOut != null && GetEventObject())
//							{
//								((VmlBaseElement) _element).InvokeFocusOut(_eventobj, _element);
//							}
//							break;
//						}
//
//						case "load":
//						{
//							if (((VmlBaseElement) _element).OnLoad != null && GetEventObject())
//							{
//								((VmlBaseElement) _element).InvokeLoad(_eventobj, _element);
//							}
//							break;
//						}
//
//						case "error":
//						{
//							if (((VmlBaseElement) _element).OnError != null && GetEventObject())
//							{
//								((VmlBaseElement) _element).InvokeError(_eventobj, _element);
//							}
//							break;
//						}
//
//						case "change":
//						{
//							if (((VmlBaseElement) _element).OnChange != null && GetEventObject())
//							{
//								((VmlBaseElement) _element).InvokeChange(_eventobj, _element);
//							}
//							break;
//						}
//
//
//						case "abort":
//						{                    
//							if (((VmlBaseElement) _element).OnAbort != null && GetEventObject())
//							{
//								((VmlBaseElement) _element).InvokeAbort(_eventobj, _element);
//							}
//							break;
//						}
//
//
//						case "select":
//						{
//							if (((VmlBaseElement) _element).OnSelect != null && GetEventObject())
//							{
//								((VmlBaseElement) _element).InvokeSelect(_eventobj, _element);
//							}
//							break;
//						}
//
//						case "selectionchange":
//						{
//							if (((VmlBaseElement) _element).OnSelectionChange != null && GetEventObject())
//							{
//								((VmlBaseElement) _element).InvokeSelectionChange(_eventobj, _element);
//							}
//							break;
//						}
//
//						case "stop":
//						{
//							if (((VmlBaseElement) _element).OnStop != null && GetEventObject())
//							{
//								((VmlBaseElement) _element).InvokeStop(_eventobj, _element);
//							}
//							break;
//						}
//
//						case "reset":
//						{
//							break;
//						}
//
//						case "submit":
//						{
//							break;
//						}
						# endregion
						case "dblclick":							
							ForcedSelection = true;
							Interop.IHTMLTextContainer container = document.GetBody() as Interop.IHTMLTextContainer;
							object controlRange = container.createControlRange();
							Interop.IHTMLControlRange htmlControlRange = controlRange as Interop.IHTMLControlRange;
							Interop.IHTMLControlRange2 htmlControlRange2 = controlRange as Interop.IHTMLControlRange2;
							int hr = htmlControlRange2.addElement(_eventobj.srcElement);							
							foreach (ShapeBehavior eb in ShapeBehavior.BehaviorInstances.Values)
							{
								eb.DrawHandles = false;
							}
							htmlControlRange.select();							
							ForcedSelection = false;
							break;
						case "controlselect":
						{    
							if (!ForcedSelection)
							{								
								//Interop.IHTMLSelectionObject selectionobj = document.GetSelection();
								try 
								{
									//selectionobj.empty();									
									_eventobj.returnValue = false;
								}
								finally{}
							} 
//							else 
//							{
//								ElementBehavior.Instance.DrawHandles = false;
//							}
							break;
						}
					}
				}

				#endregion

			}
            			
			public EventSink(Interop.IHTMLElement element)
			{
				this._baseElement = element;
			}

			/// <summary>
			/// Connects the specified control and its underlying element to the event sink.
			/// </summary>
			/// <param name="element">Underlying element of control.</param>
			public void Connect()
			{
				//this._baseElement = ((VmlBaseElement) this._element).GetBaseElement();
				Guid guid = typeof(Interop.IHTMLGenericEvents).GUID;				
				HtmlGenericEvent genericEvents = new HtmlGenericEvent(_baseElement);
				this._eventSinkCookie = new ConnectionPointCookie(this._baseElement, genericEvents, guid, false);
			}

			public void Disconnect()
			{
				if (this._eventSinkCookie != null)
				{
					this._eventSinkCookie.Disconnect();
					this._eventSinkCookie = null;
				}
			}

		}

		# endregion

        # region IElementBehaviorFactory

        /// <summary>
        /// Adds a global behavior which applies to all v:xxx elements on the page immediately.
        /// </summary>
        /// <param name="bstrBehavior"></param>
        /// <param name="bstrBehaviorUrl"></param>
        /// <param name="pSite"></param>
        /// <returns></returns>
        public Interop.IElementBehavior FindBehavior(string bstrBehavior, string bstrBehaviorUrl, Interop.IElementBehaviorSite pSite) 
		{
            //            string tagPrefix = ((Interop.IHTMLElement2)pSite.GetElement()).GetScopeName();            
            //            // just the default, extend here to support usercontrols etc.
            //            if (tagPrefix.Equals("v"))
            //            {
            //                ElementBehavior bi = new ElementBehavior(pSite.GetElement());
            //                int i = pSite.GetElement().GetHashCode();
            //                System.Diagnostics.Debug.WriteLine(bstrBehavior + " " + i.ToString(), "Fabric");
            //                this.BehaviorInstances.Add(i, bi); 
            //                return (Interop.IElementBehavior)bi;
            //            }
            //            else
            //            {
            //                return null;
            //            }
            return null;
        }

        # endregion 

        /// <summary>
        /// Internally used access to the behavior.
        /// </summary>
        /// <param name="editor">The editor the element belongs to.</param>
        /// <param name="element">The element for which the behavior is required.</param>
        /// <returns></returns>
        public Interop.IElementBehavior GetBehavior(IHtmlEditor editor, Interop.IHTMLElement element) {            
            return (Interop.IElementBehavior) this;
        }

        /// <summary>
        /// Returns the element this object belongs to.
        /// </summary>
        /// <param name="editor"></param>
        /// <returns></returns>
        public Control GetElement(IHtmlEditor editor)
		{            
			return editor.GenericElementFactory.CreateElement(element);
		}


    }
}