using System;
using System.IO;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Drawing;
using System.Globalization;
using System.Web.UI.Design;
using System.Web.UI.WebControls;

using GuruComponents.Netrix;
using GuruComponents.Netrix.Designer;
using GuruComponents.Netrix.Events;
using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.WebEditing.Elements;
using System.Collections.Generic;
using GuruComponents.Netrix.WebEditing.Behaviors;

namespace GuruComponents.Netrix.XmlDesigner
{

    internal delegate void ElementEventHandler(object sender, Interop.IHTMLEventObj e);
    /// <summary>
    /// This class creates the design time HTML for XML control, which are linked to the underlying HTML designer.
    /// </summary>
    public class DesignTimeBehavior : 
        IBaseBehavior, 
        Interop.IElementBehaviorLayout,
        Interop.IHTMLPainterEventInfo,
        IDisposable
    {
        private IHtmlEditor _editor;
        private ViewLink vl;
        private Interop.IElementBehaviorSite _behaviorSite;
        private bool vlActive, vlDrawIcon;
        private bool loading;
        private bool saving;
        private static Icon behaviorIcon;

        static DesignTimeBehavior()
        {
            behaviorIcon = new Icon(typeof(XmlDesigner).Assembly.GetManifestResourceStream("GuruComponents.Netrix.XmlDesigner.Resources.Behavior.ico"));
        }

        /// <summary>
        /// Creates new behavior instance
        /// </summary>
        /// <param name="editor"></param>
        public DesignTimeBehavior(IHtmlEditor editor)
        {
            _editor = editor;
            loading = false;
            saving = false;
            vlActive = XmlDesigner.EnsurePropertiesExists(editor).Active;
            vlDrawIcon = XmlDesigner.EnsurePropertiesExists(editor).Icon;
            _editor.Loaded += new LoadEventHandler(_editor_Loaded);
            _editor.Loading += new LoadEventHandler(_editor_Loading);
            _editor.Saving += new SaveEventHandler(_editor_Saving);
            _editor.Saved += new SaveEventHandler(_editor_Saved);
        }

        void _editor_Saved(object sender, SaveEventArgs e)
        {
            saving = false;
        }

        void _editor_Saving(object sender, SaveEventArgs e)
        {
            saving = true;
        }

        void _editor_Loading(object sender, LoadEventArgs e)
        {
            loading = true;
        }

        void _editor_Loaded(object sender, LoadEventArgs e)
        {
            loading = false;
        }

        internal bool Active
        {
            get { return vlActive; }
        }

        internal bool DrawIcon
        {
            get { return vlDrawIcon; }
        }

        /// <summary>
        /// Access the view element.
        /// </summary>
        public object ViewElement
        {
            get { return vl.DesignTimeElementView; }
        }

        # region IElementBehavior

        /// <summary>
        /// Callback gets called for every registered element type.
        /// </summary>
        /// <param name="behaviorSite"></param>
        void Interop.IElementBehavior.Init(Interop.IElementBehaviorSite behaviorSite)
        {
            if (behaviorSite == null || !Active) return;

            this._behaviorSite = behaviorSite;
            _behaviorSite.RegisterNotification(0);
            _behaviorSite.RegisterNotification(1);
            _behaviorSite.RegisterNotification(2);
            _behaviorSite.RegisterNotification(3);
            _behaviorSite.RegisterNotification(4);
            _behaviorSite.RegisterNotification(5);
 
        }

        /// <summary>
        /// Called for any element which has an designer attached to inform about particular operations.
        /// </summary>
        /// <param name="eventId">Type of event, see <see cref="Interop.BEHAVIOR_EVENT"/>.</param>
        /// <param name="pVar">Pointer th sender object. For internal use only.</param>
        void Interop.IElementBehavior.Notify(int eventId, IntPtr pVar)
        {
            //if (pVar != IntPtr.Zero && eventId != 0x4) return;
            if (_behaviorSite == null) return;
            if (((System.Windows.Forms.UserControl)_editor).IsDisposed) return;
            Interop.IHTMLElement el = _behaviorSite.GetElement();
            System.Diagnostics.Debug.WriteLine(eventId);
            switch ((Interop.BEHAVIOR_EVENT)eventId)
            {
                case Interop.BEHAVIOR_EVENT.CONTENTREADY:
                    if (saving) break;
                    vl = new ViewLink(_editor, this);
                    Interop.IHTMLElement element = _behaviorSite.GetElement();
                    element.SetAttribute(XmlElementDesigner.VIEWLINK_ATTRIB, vl, 0);                    
                    vl.IsReady = false;
                    vl.Active = vlActive;
                    vl.OnBehaviorInit(_behaviorSite);                    
                    vl.OnContentReady(element);
                    // in case a designer implements a binary behavior, we're now ready to accept it
                    ((Interop.IHTMLPaintSite)_behaviorSite).InvalidatePainterInfo();
                    vl.IsReady = true;
                    break;
                case Interop.BEHAVIOR_EVENT.DOCUMENTCONTEXTCHANGE:
                    if (vl != null)
                    {
                        vl.OnDocumentContextChanged();
                    }
                    break;
                case Interop.BEHAVIOR_EVENT.CONTENTSAVE:
                    if (!loading && vl != null && vl.IsReady)
                    {
                        vl.OnContentSave();
                    }
                    break;
                case Interop.BEHAVIOR_EVENT.DOCUMENTREADY:
                    if (vl != null)
                    {
                        vl.IsReady = true;
                    }
                    break;
                case Interop.BEHAVIOR_EVENT.APPLYSTYLE:
                    if (vl != null)
                    {
                        vl.InvalidateRegion();
                    }
                    break;

            } 
            
        }

        void Interop.IElementBehavior.Detach()
        {
            if (vl == null) return;
            vl.OnBehaviorDetach();
            vl.Dispose();
            vl = null;
        }

        # endregion

        #region IBaseBehavior Members

        /// <summary>
        /// Unique name.
        /// </summary>
        public string Name
        {
            get { return "XmlBaseBehavior#XML"; }
        }


        System.Web.UI.Control IBaseBehavior.GetElement(IHtmlEditor editor)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        # region IHTMLPainter Member

        void Interop.IHTMLPainter.Draw(int leftBounds, int topBounds, int rightBounds, int bottomBounds, int leftUpdate, int topUpdate, int rightUpdate, int bottomUpdate, int lDrawFlags, IntPtr hdc, IntPtr pvDrawObject)
        {
            if (vl != null)
            {
                using (Graphics graphics = Graphics.FromHdc(hdc))
                {
                    if (DrawIcon)
                    {
                        // draw icon slightly outside the element
                        graphics.DrawIcon(behaviorIcon, leftBounds - behaviorIcon.Width, topBounds - behaviorIcon.Height);
                    }
                    // call any custom drawing
                    vl.Draw(graphics, new Rectangle(leftBounds, topBounds, rightBounds - leftBounds, bottomBounds - topBounds));
                }
            }
        }

        void Interop.IHTMLPainter.OnResize(int cx, int cy)
        {
            if (vl != null)
            {
                vl.OnResize(cx, cy);
            }
        }

        void Interop.IHTMLPainter.GetPainterInfo(Interop.HTML_PAINTER_INFO htmlPainterInfo)
        {
            if (vl == null)
            {
                htmlPainterInfo.lFlags = (int)HtmlPainter.Transparent;
                htmlPainterInfo.lZOrder = (int)HtmlZOrder.AboveContent;
                htmlPainterInfo.rcBounds = new Interop.RECT(0, 0, 0, 0);
            }
            else
            {
                Interop.HTML_PAINTER_INFO hpi = vl.GetPainterInfo();
                if (hpi != null)
                {
                    htmlPainterInfo.lFlags = hpi.lFlags;
                    htmlPainterInfo.lZOrder = hpi.lZOrder;
                    htmlPainterInfo.rcBounds = hpi.rcBounds;
                }
            }
        }

        void Interop.IHTMLPainter.HitTestPoint(int ptx, int pty, out bool pbHit, out int plPartID)
        {
            if (vl != null)
            {
                vl.HitTestPoint(ptx, pty, out pbHit, out plPartID);
            }
            else
            {
                pbHit = false;
                plPartID = 0;
            }
        }

        # endregion


        #region IElementBehaviorLayout Members

        /// <summary>
        /// Enables a behavior to take part in the layout process.
        /// </summary>
        /// <param name="dwFlags">
        /// BEHAVIORLAYOUTMODE_NATURAL = The layout engine is requesting the natural size of the element. Both width and height must be returned.
        /// BEHAVIORLAYOUTMODE_MINWIDTH = The layout engine is requesting the element's minimum width requirement.
        /// BEHAVIORLAYOUTMODE_MAXWIDTH = The layout engine is requesting the element's maximum width requirement.
        /// BEHAVIORLAYOUTMODE_MEDIA_RESOLUTION = The layout engine is requesting the element's media resolution.
        /// BEHAVIORLAYOUTMODE_FINAL_PERCENT = This value is used when the layout engine is running through a final pass to layout percent-sized parent elements. This pass happens when a sized-to-content element is forced to change size and has percent-sized child elements that may need to resize themselves to adjust.
        /// </param>
        /// <param name="sizeContent">Dimensions of the content during rendering.</param>
        /// <param name="pptTranslateB">Specifies the amount to translate the rendered behavior.</param>
        /// <param name="pptTopLeft">Specifies the upper left corner of the rendered behavior.</param>
        /// <param name="psizeProposed">Specifies the proposed size of the behavior.</param>
        void Interop.IElementBehaviorLayout.GetSize(Interop.BEHAVIOR_LAYOUT_MODE dwFlags, Interop.tagSIZE sizeContent, ref Interop.POINT pptTranslateB, ref Interop.POINT pptTopLeft, Interop.tagSIZE psizeProposed)
        {
            // not implemented
            switch (dwFlags)
            {
                case Interop.BEHAVIOR_LAYOUT_MODE.BEHAVIORLAYOUTMODE_NATURAL:
                    break;
                case Interop.BEHAVIOR_LAYOUT_MODE.BEHAVIORLAYOUTMODE_MAXWIDTH:
                    break;
                case Interop.BEHAVIOR_LAYOUT_MODE.BEHAVIORLAYOUTMODE_MINWIDTH:
                    break;
                case Interop.BEHAVIOR_LAYOUT_MODE.BEHAVIORLAYOUTMODE_MEDIA_RESOLUTION:
                    break;
                case Interop.BEHAVIOR_LAYOUT_MODE.BEHAVIORLAYOUTMODE_FINAL_PERCENT:
                    break;
            }
        }

        /// <summary>
        /// A binary element behavior exposes an IElementBehaviorLayout interface in order to take part in the layout process. 
        /// The layout engine requests this interface. If the interface exists, the layout engine calls the 
        /// GetLayoutInfo method. The behavior returns a value that indicates whether it will register for layout 
        /// participation before or after natural sizing occurs.
        /// </summary>
        /// <returns></returns>
        //void Interop.IElementBehaviorLayout.GetLayoutInfo(out int pInfo) // Interop.BEHAVIOR_LAYOUT_INFO
        int Interop.IElementBehaviorLayout.GetLayoutInfo() // Interop.BEHAVIOR_LAYOUT_INFO
        {
            return (int) (Interop.BEHAVIOR_LAYOUT_INFO.BEHAVIORLAYOUTINFO_MAPSIZE | Interop.BEHAVIOR_LAYOUT_INFO.BEHAVIORLAYOUTINFO_MODIFYNATURAL); //: Modify slightly
        }

        void Interop.IElementBehaviorLayout.GetPosition(Interop.BEHAVIOR_LAYOUT_MODE lFlags, ref Interop.POINT pptTopLeft)
        {
            //
        }

        void Interop.IElementBehaviorLayout.MapSize(Interop.tagSIZE pSizeIn, Interop.RECT prcOut)
        {
            prcOut.top = 0;
            prcOut.left = 0;
            prcOut.bottom = pSizeIn.cy;
            prcOut.right = pSizeIn.cx;
            if (vl != null)
            {
                prcOut.top += vl.BorderZone.Top;
                prcOut.left += vl.BorderZone.Left;
                prcOut.bottom += vl.BorderZone.Height;
                prcOut.right += vl.BorderZone.Width;
            }
        }

        #endregion

        #region IHTMLPainterEventInfo Members

        void Interop.IHTMLPainterEventInfo.GetEventInfoFlags(out int plEventInfoFlags)
        {
            plEventInfoFlags = 0x2; // 0x2 = SetCursor, 0x1 = Call EventTarget (not supported!)
        }

        void Interop.IHTMLPainterEventInfo.GetEventTarget(Interop.IHTMLElement ppElement)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        void Interop.IHTMLPainterEventInfo.SetCursor(int lPartID)
        {
            vl.SetMousePointer(lPartID);
        }

        void Interop.IHTMLPainterEventInfo.StringFromPartID(int lPartID, out string pbstrPart)
        {
            pbstrPart = lPartID.ToString();
        }

        #endregion

        #region IDisposable Members

        /// <summary>
        /// Disposes the behavior.
        /// </summary>
        public void Dispose()
        {
            if (vl != null)
            {
                vl.Dispose();
            }
        }

        #endregion
    }

}

