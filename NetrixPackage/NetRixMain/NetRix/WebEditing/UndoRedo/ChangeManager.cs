using System;
using System.Runtime.InteropServices;
using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.WebEditing.Elements;
using GuruComponents.Netrix.WebEditing.HighLighting;

namespace GuruComponents.Netrix.WebEditing.UndoRedo
{

    /// <summary>
    /// Handles information about changes in the document and let control the dirty flag and change notifications.
    /// </summary>
    public class ChangeMonitor : IDisposable
    {

        private Interop.IMarkupContainer2 mc2;
        private uint cookie;
        private ChangeSink sink;
        private Interop.IHTMLChangeLog clog = null; 
        private HtmlEditor editor;

        internal event EventHandler Change;
        
        internal ChangeMonitor(Interop.IMarkupContainer2 mc, HtmlEditor editor)
        {
            this.editor = editor;
            this.mc2 = mc;
            
            sink = new ChangeSink();
            
            this.RegisterDirtyRange();                    

            if (cookie != 0)
            {
                sink.Change += new EventHandler(sink_Change);
            }            
            
        }

        void sink_Change(object sender, EventArgs e)
        {
            if (Change != null)
            {
                Change(null, null);
            }            
        }

        /// <summary>
        /// Stops any further dirty range notification, corresponding to a given cookie, from the markup container.
        /// </summary>
        public void UnRegisterDirtyRange()
        {
            mc2.UnRegisterForDirtyRange(cookie);

        }

        /// <summary>
        /// Registers a given IHTMLChangeSink interface to receive dirty range notification from the markup container.
        /// </summary>
        /// <remarks>
        /// This method is called implicitly on each <see cref="GuruComponents.Netrix.HtmlEditor.ReadyStateComplete">ReadyStateComplete</see> event. You must call 
        /// <see cref="UnRegisterDirtyRange"/> to stop notifications in the <see cref="GuruComponents.Netrix.HtmlEditor.ReadyStateComplete">ReadyStateComplete</see> handler.
        /// </remarks>
        public void RegisterDirtyRange()
        {
            mc2.RegisterForDirtyRange(sink, out cookie);
        }

        /// <summary>
        /// Creates a change log for a markup container and registers the change log to receive notice of any changes that take place in the container.
        /// </summary>
        /// <remarks>
        /// <para>
        /// To prevent this method from returning an error, set either fForward or fBackward, or both, to TRUE. These two parameters determine the kind of information that is stored in the change log. Forward information lets you replay a change (using IHTMLChangePlayback::ExecChange). Backward information lets you reverse a change.
        /// </para>
        /// <para>
        /// To stop receiving change notifications, call <see cref="ReleaseLog"/>.
        /// </para>  
        /// Version Information: This method is not support in NetRix Standard.
        /// </remarks>
        /// <see cref="GetNextChange"/>
        /// <param name="forward">Request forward information in your change log.</param>
        /// <param name="backward">Request backward information in your change log.</param>
        public void CreateLog(bool forward, bool backward)
        {
            mc2.CreateChangeLog(sink, out clog, forward ? 1 :0, backward ? 1 : 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// Version Information: This method is not support in NetRix Standard.
        /// </remarks>
        public void ReleaseLog()
        {
            Marshal.ReleaseComObject(clog);
            clog = null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// Version Information: This method is not support in NetRix Standard.
        /// </remarks>
        /// <returns></returns>
        public string GetNextChange()
        {
            string buffer = "";
            int size = 0;
            int length;
            clog.GetNextChange(buffer, size, out length);
            return buffer;
        }

        /// <summary>
        /// Retrieves the master element when a markup container belongs to a document that is a child of another one (like a frame within a frameSet).
        /// </summary>
        /// <remarks>
        /// Version Information: This method is not support in NetRix Standard.
        /// </remarks>
        public IElement MasterElement
        {
            get
            {
                Interop.IHTMLElement el;
                mc2.GetMasterElement(out el);
                if (el != null)
                {
                    return editor.GenericElementFactory.CreateElement(el) as IElement;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Retrieves a version number that represents the number of changes in a markup container's content.
        /// </summary>
        /// <remarks>
        /// Version Information: This method is not support in NetRix Standard.
        /// </remarks>
        public int Versions
        {
            get { return mc2.GetVersionNumber(); }
        }

        /// <summary>
        /// Retrieves and resets the dirty range associated with the current registration.
        /// </summary>
        /// <remarks>
        /// Version Information: This method is not support in NetRix Standard.
        /// </remarks>
        /// <returns>Returns an object which allows flexible access to the range.</returns>
        public ITextSelector GetDirtyRange()
        {
            TextSelector ts = new TextSelector(editor);
            Interop.IMarkupServices ims = (Interop.IMarkupServices) editor.GetActiveDocument(false);
            Interop.IMarkupPointer pStart;
            Interop.IMarkupPointer pEnd;            
            ims.CreateMarkupPointer(out pStart);
            ims.CreateMarkupPointer(out pEnd);
            mc2.GetAndClearDirtyRange(cookie, pStart, pEnd);
            ts.MovePointers(pStart, pEnd);
            return ts;
        }

        #region IDisposable Members

        public void Dispose()
        {
            //sink.Dispose();
            //ReleaseLog();
            //sink = null;
            //clog = null;
        }

        #endregion
    }
}
