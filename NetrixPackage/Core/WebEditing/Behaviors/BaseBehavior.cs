using System;
using System.Drawing;
using System.Web.UI;
using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.WebEditing.Elements;

namespace GuruComponents.Netrix.WebEditing.Behaviors
{

    /// <summary>
    /// Baseclass to build binary behaviors.
    /// </summary>
    /// <remarks>
    /// All binary behaviors should derive from this class and 
    /// overwrite the methods where a different functionality is needed. Overridable methods are
    /// marked as virtual. It is not necessary to overwrite or implement any methods if one don't need
    /// any additional functionality except the 
    /// <seealso cref="Draw(int, int, int, int, Graphics)">Draw</seealso> method, which is
    /// needed to provide at least any functionality to the behavior.
    /// <para>
    /// Binary behaviors add additional functionality to the element the behavior is attached. To implement
    /// a binary behavior, just derive from this class and overwrite the methods which are required to get
    /// the requested behavior.
    /// Then, after an instance of the behavior is being created, attach the behavior to the element:
    /// <code>
    /// // Get the behavior
    /// MyBehavior behavior = new MayBehavior();
    /// // Get the element
    /// IElement someElement = GetElementMethod();
    /// someElement.ElementBehaviors.Add(behavior);
    /// </code>
    /// If the behavior is no longer needed, you can detach from element:
    /// <code>
    /// someElement.ElementBehaviors.Remove(behavior);
    /// </code>
    /// The name property is used internally to get the behavior from element back and remove it.
    /// </para>
    /// <seealso cref="IElement"/>
    /// <seealso cref="IElementBehavior"/>
    /// </remarks>
    public abstract class BaseBehavior : IBaseBehavior
    {
        /// <summary>
        /// Access to underlying site object.
        /// </summary>
        internal protected Interop.IElementBehaviorSite behaviorSite;
        /// <summary>
        /// Paint site.
        /// </summary>
        internal protected Interop.IHTMLPaintSite paintsite;

        private IHtmlEditor _host;
        /// <summary>
        /// The base behavior's ctor.
        /// </summary>
        /// <param name="host"></param>
        public BaseBehavior(IHtmlEditor host)
        {
            _host = host;
        }

        private Pen borderPenStyle;
        private Rectangle borderMargin;
        private HtmlZOrder zorder = HtmlZOrder.BelowContent;
        private HtmlPainter painter = HtmlPainter.Transparent;
        /// <summary>
        /// Unique name
        /// </summary>
        protected internal readonly static string url = "GuruComponents.Netrix";
        /// <summary>
        /// Invalidate and repaint immediately.
        /// </summary>
        public void Invalidate()
        {
            if (this.paintsite != null)
            {
                try
                {
                    this.paintsite.InvalidateRect(IntPtr.Zero);
                }
                catch (System.Runtime.InteropServices.COMException)
                {
                }
            }
        }

        /// <summary>
        /// Get the name of the bahavior to avoid double attach to same element.
        /// </summary>
        /// <remarks>
        /// The final behavior implementation must override this property.
        /// </remarks>
        public abstract string Name { get; }

        /// <summary>
        /// Flag to control how the binary behavior will drawn.
        /// </summary>
        public virtual HtmlPainter HtmlPaintFlag
        {
            set
            {
                painter = value;
            }
            get
            {
                return painter;
            }
        }

        /// <summary>
        /// Flag to control the z order in which the behavior will drawn against the element, surface and window.
        /// </summary>
        public virtual HtmlZOrder HtmlZOrderFlag
        {
            set
            {
                zorder = value;
            }
            get
            {
                return zorder;
            }
        }

        /// <summary>
        /// Sets the Pen used to display a rectangle around the assigned object.
        /// </summary>
        public virtual Pen BorderPenStyle
        {
            set
            {
                borderPenStyle = value;
            }
            get
            {
                return borderPenStyle;
            }
        }

        /// <summary>
        /// Sets the margin which is build outside the element and let grow the space the element
        /// takes on the designer surface.
        /// </summary><remarks>
        /// This property defaults to zero margins. It is useful to
        /// overwrite if an element has a border which overwrites the drawn line. 
        /// </remarks>
        public virtual Rectangle BorderMargin
        {
            set
            {
                borderMargin = value;
            }
            get
            {
                return borderMargin;
            }
        }

		# region Behavior Events

		/// <summary>
		/// Notifies the behavior about the progress of parsing the document and the element to which the behavior is attached.
		/// </summary>
		public event BehaviorNotifyEventHandler Notify;
		/// <summary>
		/// Notifies the behavior that it is being detached from an element.
		/// </summary>
		public event EventHandler Detach;
		/// <summary>
		/// Notifies the behavior that it has been instantiated.
		/// </summary>
		public event EventHandler Init;

        /// <summary>
        /// TODO: Add comment.
        /// </summary>
        protected void OnInit()
		{
			if (Init != null)
			{
				Init(this, EventArgs.Empty);
			}
		}

        /// <summary>
        /// TODO: Add comment.
        /// </summary>
        protected void OnNotify(BehaviorNotifyEventArgs e)
		{
			if (Notify != null)
			{
				Notify(this, e);
			}
		}

        /// <summary>
        /// TODO: Add comment.
        /// </summary>
        protected void OnDetach()
		{
			if (Detach != null)
			{
				Detach(this, EventArgs.Empty);
			}
		}

		# endregion

        #region IHTMLPainter Member

        void Interop.IHTMLPainter.Draw(int leftBounds, int topBounds, int rightBounds, int bottomBounds, int leftUpdate, int topUpdate, int rightUpdate, int bottomUpdate, int lDrawFlags, IntPtr hdc, IntPtr pvDrawObject)
        {
            try
            {
                if (!_host.IsReady || hdc == IntPtr.Zero || !_host.DesignModeEnabled || hdc.ToInt64() == 1) return;
                using (Graphics gr = Graphics.FromHdc(hdc))
                {
                    Draw(leftBounds, topBounds, rightBounds, bottomBounds, gr);
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// Overwrite with method to implement own binary behaviors. The method can contain any GDI+
        /// or GDI calls to paint on the designer surface within the element the bahavior is attached.
        /// The following code gives an idea how to implement this:
        /// <code>
        /// using System.Drawing;
        /// // other using statements may be needed
        /// </code>
        /// <code>
        /// Graphics gr = Graphics.FromHdc(hdc);
        /// gr.PageUnit = GraphicsUnit.Pixel;
        /// gr.DrawRectangle(BorderPenStyle, leftBounds, topBounds, rightBounds - leftBounds, bottomBounds - topBounds);
        /// </code>
        /// This behavior draws a rectangle around the element taking the default styles.
        /// </summary>
        /// <param name="leftBounds">left side of element</param>
        /// <param name="topBounds">top side of element</param>
        /// <param name="rightBounds">right side of element</param>
        /// <param name="bottomBounds">bottom side of element</param>
        /// <param name="hdc">Pointer to graphic device. See example of how to use it.</param>
        [Obsolete("This overload is obsolete, please use the method which directly provides Graphics object.")]
        protected virtual void Draw(int leftBounds, int topBounds, int rightBounds, int bottomBounds, IntPtr hdc)
        {

        }

        /// <summary>
        /// Overwrite with method to implement own binary behaviors. The method can contain any GDI+
        /// or GDI calls to paint on the designer surface within the element the bahavior is attached.
        /// The following code gives an idea how to implement this:
        /// <code>
        /// using System.Drawing;
        /// // other using statements may be needed
        /// </code>
        /// <code>
        /// Graphics gr = Graphics.FromHdc(hdc);
        /// gr.PageUnit = GraphicsUnit.Pixel;
        /// gr.DrawRectangle(BorderPenStyle, leftBounds, topBounds, rightBounds - leftBounds, bottomBounds - topBounds);
        /// </code>
        /// This behavior draws a rectangle around the element taking the default styles.
        /// </summary>
        /// <param name="leftBounds">left side of element</param>
        /// <param name="topBounds">top side of element</param>
        /// <param name="rightBounds">right side of element</param>
        /// <param name="bottomBounds">bottom side of element</param>
        /// <param name="gr">Pointer to graphic device. See example of how to use it.</param>
        protected virtual void Draw(int leftBounds, int topBounds, int rightBounds, int bottomBounds, Graphics gr)
        {

        }


         /// <summary>
        /// Called when an element containing a rendering behavior is resized.
        /// </summary>
        /// <param name="cx">Width after resize</param>
        /// <param name="cy">Height after resize</param>
        void Interop.IHTMLPainter.OnResize(int cx, int cy)
        {
            OnResize(cx, cy);
        }
        /// <summary>
        /// TODO: Add comments.
        /// </summary>
        /// <param name="cx"></param>
        /// <param name="cy"></param>
        protected virtual void OnResize(int cx, int cy)
        {
        }

        /// <summary>
        /// This methode creates a PainterInfo object and returns it. Normally one shouldnot overwrite this
        /// method and use the properties <see cref="HtmlPaintFlag">HtmlPaintFlag</see> 
        /// and <see cref="HtmlZOrderFlag">HtmlZOrderFlag</see> instead.
        /// </summary>
        /// <param name="pInfo"></param>
        void Interop.IHTMLPainter.GetPainterInfo(Interop.HTML_PAINTER_INFO pInfo)
        {
            pInfo.lFlags = (int) this.HtmlPaintFlag;
            pInfo.lZOrder = (int) this.HtmlZOrderFlag;
            pInfo.iidDrawObject = Guid.Empty;
            pInfo.rcBounds = new Interop.RECT(borderMargin.Left, borderMargin.Top, borderMargin.Right, borderMargin.Bottom);
        }

        /// <summary>
        /// The current test hit can checked against the element coordinates.
        /// </summary>
        /// <param name="ptx">X coordinate</param>
        /// <param name="pty">Y coordinate</param>
        /// <param name="pbHit"></param>
        /// <param name="plPartID"></param>
        /// <returns></returns>
        void Interop.IHTMLPainter.HitTestPoint(int ptx, int pty, out bool pbHit, out int plPartID)
        {
            pbHit = HitTestPoint(ptx, pty, out plPartID);
        }

        /// <summary>
        /// Called by the renderer to retrieve a value that specifies whether a point is contained in a rendering behavior.
        /// </summary>
        /// <remarks>
        /// The renderer calls this method when calls are made to such methods as elementFromPoint or componentFromPoint that 
        /// need to determine how a point relates to the elements in a document's object tree. The behavior then can determine 
        /// how the element to which it is attached handles events. In particular, the plPartID parameter enables a rendering 
        /// behavior to assign identification numbers for different parts of the behavior. The value of this parameter is stored 
        /// as an event object property. For example, a resizing behavior could return a unique value in plPartID for each 
        /// of its sizing handles. The <see cref="Draw(int,int,int,int,IntPtr)"/> method could then query the event object for this value to determine 
        /// in which direction to resize an element as the user drags the sizing handle.
        /// When the behavior is rendered below the flow layer of an element, the plPartID returned by this method is not passed 
        /// to the event object. Instead, the event object contains information provided by the objects in the flow layer, above 
        /// the behavior. The lZOrder member of the <see cref="HtmlPainter"/> structure specified by GetPainterInfo 
        /// determines whether the behavior is above or below the flow layer.
        /// </remarks>
        /// <param name="ptx">x of point clicked relative to the top-left corner of the element to which the behavior is attached.</param>
        /// <param name="pty">y of point clicked relative to the top-left corner of the element to which the behavior is attached.</param>
        /// <param name="plPartID">Receives a number identifying which part of the behavior has been hit</param>
        /// <returns>TRUE if the point is contained in the element to which the rendering behavior is attached, or FALSE otherwise.</returns>
        protected virtual bool HitTestPoint(int ptx, int pty, out int plPartID)
        {
            plPartID = 0;
            return true;
        }

        #endregion

        #region IElementBehavior Member

        void Interop.IElementBehavior.Init(Interop.IElementBehaviorSite pBehaviorSite)
        {
            behaviorSite = pBehaviorSite;			
            paintsite = (Interop.IHTMLPaintSite) behaviorSite;
			behaviorSite.RegisterNotification(0);
			behaviorSite.RegisterNotification(1);
			behaviorSite.RegisterNotification(2);
			behaviorSite.RegisterNotification(3);
			behaviorSite.RegisterNotification(4);
			OnInit();
        }

        void Interop.IElementBehavior.Notify(int dwEvent, IntPtr pVar)
        {
			BehaviorNotifyType t;
			switch (dwEvent)
			{
				case 0:
					t = BehaviorNotifyType.ContentReady;
					OnNotify(new BehaviorNotifyEventArgs(t, behaviorSite));
					break;
				case 1:
					t = BehaviorNotifyType.DocumentReady;
					OnNotify(new BehaviorNotifyEventArgs(t, behaviorSite));
					break;
				case 2:
					return;
				case 3:
					t = BehaviorNotifyType.DocumentContextChange;
					OnNotify(new BehaviorNotifyEventArgs(t, behaviorSite));
					break;
				case 4:
					t = BehaviorNotifyType.ContentSave;
					OnNotify(new BehaviorNotifyEventArgs(t, behaviorSite));
					break;
			}			
        }

        void Interop.IElementBehavior.Detach()
        {
			OnDetach();
        }

        #endregion

		/// <summary>
		/// Return the attached element.
		/// </summary>
		/// <param name="editor"></param>
		/// <returns></returns>
        public Control GetElement(IHtmlEditor editor)
        {            
            return editor.GenericElementFactory.CreateElement(this.behaviorSite.GetElement());
        }

        /// <summary>
        /// Returns the current instance. 
        /// </summary>
        /// <remarks>
        /// Both parameters are for backward compatibility and does not affect the result. They might be set <c>null</c> safely.
        /// </remarks>
        /// <param name="editor">Editor reference. Not used.</param>
        /// <param name="element">Alternate element. Nou used.</param>
        public Interop.IElementBehavior GetBehavior(IHtmlEditor editor, Interop.IHTMLElement element)
		{            
			return this;
		}

    }
}
