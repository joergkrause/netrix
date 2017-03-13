using System;
using System.Collections;
using GuruComponents.Netrix.ComInterop;

namespace GuruComponents.Netrix.WebEditing.Elements
{
    /// <summary>
    /// This class provides a collection of options that are part of a option group.
    /// </summary>
    /// <remarks>
    /// The constructor
    /// will create the list based on the given element which is expected to be an OPTGROUP element.
    /// </remarks>
    public class OptionOnlyElementsCollection : System.Collections.CollectionBase
    {

        IHtmlEditor editor;

        internal OptionOnlyElementsCollection(Interop.IHTMLElement optGroup, IHtmlEditor editor) : base()
        {
            this.editor = editor;
            Interop.IHTMLElementCollection options = (Interop.IHTMLElementCollection) optGroup.GetChildren();
            for (int i = 0; i < options.GetLength(); i++)
            {	
                Interop.IHTMLElement el = (Interop.IHTMLElement) options.Item(i, i);
                OptionElement oe = null;
                oe = new OptionElement(el, editor);
                if (oe != null)
                {
                    InnerList.Add(oe);
                }
            }
        }

        public void Add(OptionElement o) 
        {
            if (o == null) return;
            base.InnerList.Add(o);
        }

        public bool Contains(OptionElement o) 
        {
            if (o == null) return false;
            return InnerList.Contains(o);
        }

        public void Remove(OptionElement o) 
        {
            if (o == null) return;
            base.InnerList.Remove(o);
        }

        public OptionElement this[int index] 
        {
            get 
            {
                return (OptionElement) base.InnerList[index];
            }
            set
            {
                base.InnerList[index] = (OptionElement) value;
            }
        }

        /// <summary>
        /// Fired if the collection editor inserts a new element
        /// </summary>
        public event CollectionInsertHandler OnInsertHandler;
        /// <summary>
        /// Fired if the collection editor starts a new sequence
        /// </summary>
        public event CollectionClearHandler OnClearHandler;

        protected override void OnInsertComplete(int index, object value)
        {
            base.OnInsertComplete (index, value);
            // prevent from fail during first collection fill process, because the handler is temporary removed
            if (OnInsertHandler != null)                                                             
            {
                OnInsertHandler(index, value);
            }
        }

        protected override void OnClear()
        {
            base.OnClear ();
            // prevent from fail during first collection fill process, because the handler is temporary removed
            if (OnClearHandler != null)
            {
                OnClearHandler();
            }
        }
    }
}