using System;
using System.Collections.Generic;
using System.Text;
using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.WebEditing.UndoRedo;
using GuruComponents.Netrix.WebEditing.Elements;
using System.Web.UI;

namespace GuruComponents.Netrix.NetRix.WebEditing.HighLighting
{
    public class MarkupContainer
    {
        
        private Interop.IMarkupContainer2 mc;
        private uint pdwCookie;
        private IHtmlEditor editor;

        internal MarkupContainer(Interop.IMarkupContainer mc, IHtmlEditor editor)
        {
            this.editor = editor;
            this.mc = (Interop.IMarkupContainer2)mc;
        }

        internal Interop.IMarkupContainer2 Native
        {
            get { return mc; }
        }

        #region IMarkupContainer Members

        public Interop.IHTMLDocument2 OwningDoc()
        {
            Interop.IHTMLDocument2 ppDoc;
            ((Interop.IMarkupContainer)mc).OwningDoc(out ppDoc);
            return ppDoc;
        }

        #endregion

        #region IMarkupContainer2 Members


        public void CreateChangeLog(ChangeSink changeSink, bool fForward, bool fBackward)
        {
            Interop.IHTMLChangeSink pChangeSink = changeSink;
            Interop.IHTMLChangeLog ppChangeLog;
            mc.CreateChangeLog(pChangeSink, out ppChangeLog, (fForward ? 1 : 0), (fBackward ? 1 : 0));
        }

        public int RegisterForDirtyRange(ChangeSink changeSink)
        {
            Interop.IHTMLChangeSink pChangeSink = changeSink;
            mc.RegisterForDirtyRange(pChangeSink, out pdwCookie);
            return (int)pdwCookie;
        }

        public void UnRegisterForDirtyRange()
        {
            if (pdwCookie == 0) 
                throw new ArgumentException("Cookie not set. call RegisterForDirtyRange() first");
            mc.UnRegisterForDirtyRange(pdwCookie);
        }

        public void UnRegisterForDirtyRange(uint dwCookie)
        {
            mc.UnRegisterForDirtyRange(dwCookie);
        }

        public void GetAndClearDirtyRange(uint dwCookie, MarkupPointer pointerBegin, MarkupPointer pointerEnd)
        {
            Interop.IMarkupPointer pIPointerBegin = pointerBegin.Native;
            Interop.IMarkupPointer pIPointerEnd = pointerEnd.Native;
            mc.GetAndClearDirtyRange(dwCookie, pIPointerBegin, pIPointerEnd);
        }

        public int GetVersionNumber()
        {
            return mc.GetVersionNumber();
        }

        public Control GetMasterElement()
        {
            Interop.IHTMLElement ppElementMaster;
            mc.GetMasterElement(out ppElementMaster);
            return editor.GenericElementFactory.CreateElement(ppElementMaster);
        }

        #endregion
    }
}
