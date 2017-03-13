using System;
using System.Collections.Generic;
using System.Text;
using GuruComponents.Netrix.ComInterop;

namespace GuruComponents.Netrix.NetRix.WebEditing.HighLighting
{
    public class MarkupPointer : Interop.IMarkupPointer, Interop.IMarkupPointer2
    {

        Interop.IMarkupPointer mp;
        Interop.IMarkupPointer2 mp2;
        private IHtmlEditor editor;

        internal MarkupPointer(Interop.IMarkupPointer mp, IHtmlEditor editor)
        {
            this.editor = editor;
            this.mp = mp;
            this.mp2 = (Interop.IMarkupPointer2)mp;
        }

        internal Interop.IMarkupPointer Native
        {
            get { return mp; }
        }


        #region IMarkupPointer2 Members

        public void IsAtWordBreak(out int pfAtBreak)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void GetMarkupPosition(out int plMP)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void MoveToMarkupPosition(Interop.IMarkupContainer pContainer, int lMP)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void MoveUnitBounded(Interop.MOVEUNIT_ACTION muAction, Interop.IMarkupPointer pIBoundary)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void IsInsideURL(Interop.IMarkupPointer pRight, out int pfResult)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void MoveToContent(Interop.IHTMLElement pIElement, int fAtStart)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        #region IMarkupPointer Members

        public void OwningDoc(out Interop.IHTMLDocument2 ppDoc)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Gravity(out Interop.POINTER_GRAVITY pGravity)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetGravity(Interop.POINTER_GRAVITY Gravity)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Cling(out int pfCling)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetCling(int fCLing)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Unposition()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void IsPositioned(out int pfPositioned)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void GetContainer(out Interop.IMarkupContainer ppContainer)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void MoveAdjacentToElement(Interop.IHTMLElement pElement, Interop.ELEMENT_ADJACENCY eAdj)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void MoveToPointer(Interop.IMarkupPointer pPointer)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void MoveToContainer(Interop.IMarkupContainer pContainer, int fAtStart)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Left(int fMove, out Interop.MARKUP_CONTEXT_TYPE pContext, out Interop.IHTMLElement ppElement, ref int pcch, out string pchText)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Right(int fMove, out Interop.MARKUP_CONTEXT_TYPE pContext, out Interop.IHTMLElement ppElement, ref int pcch, out string pchText)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void CurrentScope(out Interop.IHTMLElement ppElemCurrent)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void IsLeftOf(Interop.IMarkupPointer pPointerThat, out int pfResult)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void IsLeftOfOrEqualTo(Interop.IMarkupPointer pPointerThat, out int pfResult)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void IsRightOf(Interop.IMarkupPointer pPointerThat, out int pfResult)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void IsRightOfOrEqualTo(Interop.IMarkupPointer pPointerThat, out int pfResult)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void IsEqualTo(Interop.IMarkupPointer pPointerThat, out int pfAreEqual)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void MoveUnit(Interop.MOVEUNIT_ACTION muAction)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void FindText(ref string pchFindText, uint dwFlags, Interop.IMarkupPointer pIEndMatch, Interop.IMarkupPointer pIEndSearch)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}
