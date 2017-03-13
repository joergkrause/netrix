using System;
using System.Collections.Generic;
using System.Text;
using GuruComponents.Netrix.ComInterop;
using System.Drawing;
using System.Web.UI;
using GuruComponents.Netrix.WebEditing.Elements;

namespace GuruComponents.Netrix.NetRix.WebEditing.HighLighting
{
    public class DisplayService   
    {

        Interop.IDisplayServices ds;
        private IHtmlEditor editor;

        internal DisplayService(IHtmlEditor editor)
        {
            this.editor = editor;
            ds = (Interop.IDisplayServices)editor.GetActiveDocument(false);
        }

        #region IDisplayServices Members

        public DisplayPointer CreateDisplayPointer()
        {
            Interop.IDisplayPointer ppDispPointer;
            ds.CreateDisplayPointer(out ppDispPointer);
            DisplayPointer dp = new DisplayPointer(ppDispPointer, editor);
            return dp;
        }

        public Interop.RECT TransformRect(Interop.RECT pRect, Interop.COORD_SYSTEM eSource, Interop.COORD_SYSTEM eDestination, IElement element)
        {
            Interop.IHTMLElement pIElement = ((IElement)element).GetBaseElement();
            ds.TransformRect(ref pRect, eSource, eDestination, pIElement);
            return pRect;
        }

        public Interop.POINT TransformPoint(Interop.POINT pPoint, Interop.COORD_SYSTEM eSource, Interop.COORD_SYSTEM eDestination, IElement element)
        {
            Interop.IHTMLElement pIElement = element.GetBaseElement();
            ds.TransformPoint(ref pPoint, eSource, eDestination, pIElement);
            return pPoint;
        }

        HtmlCaret GetCaret()
        {
            Interop.IHTMLCaret caret;
            ds.GetCaret(out caret);
            return new HtmlCaret(caret);
        }

        Interop.IHTMLComputedStyle GetComputedStyle(MarkupPointer mp)
        {
            Interop.IHTMLComputedStyle ppComputedStyle;
            Interop.IMarkupPointer pPointer = mp.Native;
            ds.GetComputedStyle(pPointer, out ppComputedStyle);
            return ppComputedStyle;
        }

        public void ScrollRectIntoView(IElement element, Interop.RECT rect)
        {
            Interop.IHTMLElement pIElement = element.GetBaseElement();
            ds.ScrollRectIntoView(pIElement, rect);
        }

        public bool HasFlowLayout(IElement element)
        {
            int pfHasFlowLayout;
            Interop.IHTMLElement pIElement = element.GetBaseElement();
            ds.HasFlowLayout(pIElement, out pfHasFlowLayout);
            return (pfHasFlowLayout == 1);
        }

        #endregion
    }
}
