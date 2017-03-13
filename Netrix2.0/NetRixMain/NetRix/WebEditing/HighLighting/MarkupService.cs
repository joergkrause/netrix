using System;
using System.Collections.Generic;
using System.Text;
using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.WebEditing.Elements;
using System.Web.UI;

namespace GuruComponents.Netrix.NetRix.WebEditing.HighLighting
{
    public class MarkupService
    {

        private Interop.IMarkupServices ms;
        private Interop.IMarkupServices2 ms2;
        private IHtmlEditor editor;

        internal MarkupService(IHtmlEditor editor)
        {
            this.editor = editor;
            ms = (Interop.IMarkupServices)editor.GetActiveDocument(false);
            ms2 = (Interop.IMarkupServices2)editor.GetActiveDocument(false);
        }

        internal Interop.IMarkupServices Native { get { return ms; } } 

        #region IMarkupServices Members

        public MarkupPointer CreateMarkupPointer()
        {
            Interop.IMarkupPointer ppPointer;
            ms.CreateMarkupPointer(out ppPointer);
            return new MarkupPointer(ppPointer, editor);
        }

        public MarkupContainer CreateMarkupContainer()
        {
            Interop.IMarkupContainer ppMarkupContainer;
            ms.CreateMarkupContainer(out ppMarkupContainer);
            return new MarkupContainer(ppMarkupContainer, editor);
        }

        public Control CreateElement(Interop.ELEMENT_TAG_ID tagID, string pchAttributes)
        {
            Interop.IHTMLElement ppElement;
            ms.CreateElement(tagID, pchAttributes, out ppElement);
            return editor.GenericElementFactory.CreateElement(ppElement);
        }

        public Control CloneElement(Interop.IHTMLElement pElemCloneThis)
        {
            Interop.IHTMLElement ppElementTheClone;
            ms.CloneElement(pElemCloneThis, out ppElementTheClone);
            return editor.GenericElementFactory.CreateElement(ppElementTheClone);
        }

        public void InsertElement(Interop.IHTMLElement pElementInsert, MarkupPointer pointerStart, MarkupPointer pointerFinish)
        {
            Interop.IMarkupPointer pPointerStart = pointerStart.Native;
            Interop.IMarkupPointer pPointerFinish = pointerFinish.Native;
            ms.InsertElement(pElementInsert, pPointerStart, pPointerFinish);
        }

        public void RemoveElement(IElement elementRemove)
        {
            Interop.IHTMLElement pElementRemove = elementRemove.GetBaseElement();
            ms.RemoveElement(pElementRemove);
        }

        public void Remove(MarkupPointer pointerStart, MarkupPointer pointerFinish)
        {
            Interop.IMarkupPointer pPointerStart = pointerStart.Native;
            Interop.IMarkupPointer pPointerFinish = pointerFinish.Native;
            ms.Remove(pPointerStart, pPointerFinish);
        }

        public void Copy(MarkupPointer pointerSourceStart, MarkupPointer pointerSourceFinish, MarkupPointer pointerTarget)
        {
            Interop.IMarkupPointer pPointerSourceStart = pointerSourceStart.Native;
            Interop.IMarkupPointer pPointerSourceFinish = pointerSourceFinish.Native;
            Interop.IMarkupPointer pPointerTarget = pointerTarget.Native;
            ms.Copy(pPointerSourceStart, pPointerSourceFinish, pPointerTarget);
        }

        public void Move(MarkupPointer pointerSourceStart, MarkupPointer pointerSourceFinish, MarkupPointer pointerTarget)
        {
            Interop.IMarkupPointer pPointerSourceStart = pointerSourceStart.Native;
            Interop.IMarkupPointer pPointerSourceFinish = pointerSourceFinish.Native;
            Interop.IMarkupPointer pPointerTarget = pointerTarget.Native;
            ms.Move(pPointerSourceStart, pPointerSourceFinish, pPointerTarget);
        }

        public void InsertText(string pchText, MarkupPointer pointerTarget)
        {
            InsertText(pchText, pchText.Length, pointerTarget);
        }

        public void InsertText(string pchText, int cch, MarkupPointer pointerTarget)
        {
            Interop.IMarkupPointer pPointerTarget = pointerTarget.Native;
            ms.InsertText(pchText, cch, pPointerTarget);
        }

        public MarkupContainer ParseString(string pchHTML, uint dwFlags, MarkupPointer pointerStart, MarkupPointer pointerFinish)
        {
            Interop.IMarkupPointer ppPointerStart = pointerStart.Native;
            Interop.IMarkupPointer ppPointerFinish = pointerFinish.Native;
            Interop.IMarkupContainer ppContainerResult;
            ms.ParseString(ref pchHTML, dwFlags, out ppContainerResult, ppPointerStart, ppPointerFinish);
            return new MarkupContainer(ppContainerResult, editor);
        }

        public MarkupContainer ParseGlobal(string html, uint dwFlags, MarkupPointer pointerStart, MarkupPointer pointerFinish)
        {
            Interop.UserHGLOBAL hglobalHTML = new Interop.UserHGLOBAL();
            hglobalHTML.u = System.Runtime.InteropServices.Marshal.StringToBSTR(html);
            Interop.IMarkupPointer ppPointerStart = pointerStart.Native;
            Interop.IMarkupPointer ppPointerFinish = pointerFinish.Native;
            Interop.IMarkupContainer ppContainerResult;
            ms.ParseGlobal(hglobalHTML, dwFlags, out ppContainerResult, ppPointerStart, ppPointerFinish);
            return new MarkupContainer(ppContainerResult, editor);
        }

        public MarkupContainer ParseGlobal(string html, uint dwFlags, MarkupContainer pContext, MarkupPointer pointerStart, MarkupPointer pointerFinish)
        {
            Interop.UserHGLOBAL hglobalHTML = new Interop.UserHGLOBAL();
            hglobalHTML.u = System.Runtime.InteropServices.Marshal.StringToBSTR(html);
            Interop.IMarkupContainer ppContainerResult;
            Interop.IMarkupPointer ppPointerStart = pointerStart.Native;
            Interop.IMarkupPointer ppPointerFinish = pointerFinish.Native;
            ms2.ParseGlobalEx(hglobalHTML, dwFlags, (Interop.IMarkupContainer)pContext.Native, out ppContainerResult, ppPointerStart, ppPointerFinish);
            return new MarkupContainer(ppContainerResult, editor);
        }

        public bool IsScopedElement(IElement element)
        {
            int pfScoped;
            Interop.IHTMLElement pIElement = element.GetBaseElement();
            ms.IsScopedElement(pIElement, out pfScoped);
            return (pfScoped == 1);
        }

        public Interop.ELEMENT_TAG_ID GetElementTagId(IElement element)
        {
            Interop.ELEMENT_TAG_ID ptagId;
            Interop.IHTMLElement pIElement = element.GetBaseElement();
            ms.GetElementTagId(pIElement, out ptagId);
            return ptagId;
        }

        public Interop.ELEMENT_TAG_ID GetTagIDForName(string bstrName)
        {
            Interop.ELEMENT_TAG_ID ptagId;
            ms.GetTagIDForName(bstrName, out ptagId);
            return ptagId;
        }

        public string GetNameForTagID(Interop.ELEMENT_TAG_ID ptagId)
        {
            string pbstrName;
            ms.GetNameForTagID(ptagId, out pbstrName);
            return pbstrName;
        }

        public void MovePointersToRange(HtmlTextRange range, MarkupPointer pointerStart, MarkupPointer pointerFinish)
        {
            Interop.IHTMLTxtRange pIRange = range.Native;
            Interop.IMarkupPointer pPointerStart = pointerStart;
            Interop.IMarkupPointer pPointerFinish = pointerFinish;
            ms.MovePointersToRange(pIRange, pPointerStart, pPointerFinish);
        }

        public void MoveRangeToPointers(MarkupPointer pointerStart, MarkupPointer pointerFinish, HtmlTextRange range)
        {
            Interop.IMarkupPointer pPointerStart = pointerStart.Native;
            Interop.IMarkupPointer pPointerFinish = pointerFinish.Native;
            Interop.IHTMLTxtRange pIRange = range.Native;
            ms.MoveRangeToPointers(pPointerStart, pPointerFinish, pIRange);
        }

        public void BeginUndoUnit(string pchTitle)
        {
            ms.BeginUndoUnit(ref pchTitle);
        }

        public void EndUndoUnit()
        {
            ms.EndUndoUnit();
        }

        #endregion

        #region IMarkupServices2 Members

        public void ValidateElements(MarkupPointer pointerStart, MarkupPointer pointerFinish, MarkupPointer pointerTarget, ref MarkupPointer pointerStatus, out Interop.IHTMLElement ppElemFailBottom, out Interop.IHTMLElement ppElemFailTop)
        {
            Interop.IMarkupPointer pPointerStart = pointerStart.Native;
            Interop.IMarkupPointer pPointerFinish = pointerFinish.Native;
            Interop.IMarkupPointer pPointerTarget = pointerTarget.Native;
            Interop.IMarkupPointer pPointerStatus = pointerStatus.Native;
            ms2.ValidateElements(pPointerStart, pPointerFinish, pPointerTarget, ref pPointerStatus, out ppElemFailBottom, out ppElemFailTop);
        }

        public void SaveSegmentsToClipboard(Interop.ISegmentList pSegmentList, uint dwFlags)
        {
            ms2.SaveSegmentsToClipboard(pSegmentList, dwFlags);
        }

        #endregion
    }
}
