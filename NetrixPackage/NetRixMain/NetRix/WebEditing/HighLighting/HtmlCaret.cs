using System;
using System.Collections.Generic;
using System.Text;
using GuruComponents.Netrix.ComInterop;

namespace GuruComponents.Netrix.NetRix.WebEditing.HighLighting
{
    class HtmlCaret
    {

        private Interop.IHTMLCaret cr;

        internal HtmlCaret(Interop.IHTMLCaret cr)
        {
            this.cr = cr;
        }

        #region IHTMLCaret Members

        public void MoveCaretToPointer(DisplayPointer dispPointer, bool fScrollIntoView, Interop.CARET_DIRECTION eDir)
        {
            Interop.IDisplayPointer pDispPointer = dispPointer.Native;
            cr.MoveCaretToPointer(pDispPointer, fScrollIntoView, eDir);
        }

        public void MoveCaretToPointer(DisplayPointer dispPointer, bool fVisible, bool fScrollIntoView, Interop.CARET_DIRECTION eDir)
        {
            Interop.IDisplayPointer pDispPointer = dispPointer.Native;
            cr.MoveCaretToPointerEx(pDispPointer, fVisible, fScrollIntoView, eDir);
        }

        public void MoveMarkupPointerToCaret(MarkupPointer markupPointer)
        {
            Interop.IMarkupPointer pIMarkupPointer = markupPointer.Native;
            cr.MoveMarkupPointerToCaret(pIMarkupPointer);
        }

        public void MoveDisplayPointerToCaret(DisplayPointer dispPointer)
        {
            Interop.IDisplayPointer pDispPointer = dispPointer.Native;
            cr.MoveDisplayPointerToCaret(pDispPointer);
        }

        public bool IsVisible()
        {
            bool pIsVisible;
            cr.IsVisible(out pIsVisible);
            return pIsVisible;
        }

        public void Show()
        {
            Show(true);
        }

        public void Show(bool fScrollIntoView)
        {
            cr.Show(fScrollIntoView);
        }

        public void Hide()
        {
            cr.Hide();
        }

        public void InsertText(string pText, int lLen)
        {
            cr.InsertText(ref pText, lLen);
        }

        public void ScrollIntoView()
        {
            cr.ScrollIntoView();
        }

        public Interop.POINT GetLocation(bool fTranslate)
        {
            Interop.POINT pPoint = new Interop.POINT();
            cr.GetLocation(ref pPoint, fTranslate);
            return pPoint;
        }

        public Interop.CARET_DIRECTION GetCaretDirection()
        {
            Interop.CARET_DIRECTION peDir;
            cr.GetCaretDirection(out peDir);
            return peDir;
        }

        public void SetCaretDirection(Interop.CARET_DIRECTION eDir)
        {
            cr.SetCaretDirection(eDir);
        }

        #endregion
    }
}
