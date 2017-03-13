using System;
using System.Collections;
using GuruComponents.Netrix.ComInterop;

namespace GuruComponents.Netrix.WebEditing.Elements.Html5
{
    /// <summary>
    /// Stores a collection of SOURCE elements. (HTML 5)
    /// </summary>
    /// <remarks>
    /// This class stores SOURCE elements in any combination for AUDIO and VIDEO elements.
    /// </remarks>
    public class SourceElementsCollection : System.Collections.CollectionBase
    {
        /// <summary>
        /// Provide one possible type to the propertygrid's collection editor to help them building elements
        /// </summary>
        public static readonly Type SourceElementType = typeof(GuruComponents.Netrix.WebEditing.Elements.Html5.SourceElement);

        private Interop.IHTMLElement selectElement;

        internal SourceElementsCollection(Interop.IHTMLElement s, IHtmlEditor editor)
            : base()
        {
            selectElement = (Interop.IHTMLElement)s;
            Interop.IHTMLElementCollection options = (Interop.IHTMLElementCollection)selectElement.GetChildren();
            for (int i = 0; i < options.GetLength(); i++)
            {
                Interop.IHTMLElement el = (Interop.IHTMLElement)options.Item(i, i);
                object oe = new SourceElement(el, (HtmlEditor)editor);
                InnerList.Add(oe);
            }
        }

        public new void Add(object o)
        {
            if (o == null) return;
            base.InnerList.Add(o);
            if (OnInsertHandler != null)
            {
                OnInsertHandler(base.InnerList.Count, o);
            }
        }

        public new void InsertAt(int index, object o)
        {
            if (o == null) return;
            base.InnerList.Add(o);
            if (OnInsertHandler != null)
            {
                OnInsertHandler(index, o);
            }
        }

        public new bool Contains(object o)
        {
            if (o == null) return false;
            return InnerList.Contains(o);
        }

        public new void RemoveAt(int index)
        {
            if (index >= base.InnerList.Count || index < 0) return;
            base.InnerList.RemoveAt(index);
            Interop.IHTMLElementCollection options = (Interop.IHTMLElementCollection)selectElement.GetChildren();
            for (int i = 0; i < options.GetLength(); i++)
            {
                Interop.IHTMLElement el = (Interop.IHTMLElement)options.Item(i, i);
                if (index == i)
                {
                    Interop.IHTMLDOMNode no = (Interop.IHTMLDOMNode)el;
                    no.removeNode(true);
                }
            }
        }

        public new SourceElement this[int index]
        {
            get
            {
                object o = base.InnerList[index];
                return o as SourceElement;
            }
            set
            {
                base.InnerList[index] = value;
            }
        }

        /// <summary>
        /// Fired if the collection editor inserts a new element.
        /// </summary>
        public event CollectionInsertHandler OnInsertHandler;
        /// <summary>
        /// Fired if the collection editor starts a new sequence.
        /// </summary>
        public event CollectionClearHandler OnClearHandler;

        protected override void OnInsertComplete(int index, object value)
        {
            base.OnInsertComplete(index, value);
            // prevent from fail during first collection fill process, because the handler is temporary removed
            if (OnInsertHandler != null)
            {
                OnInsertHandler(index, value);
            }
        }

        protected override void OnClear()
        {
            base.InnerList.Clear(); //.OnClear ();
            // prevent from fail during first collection fill process, because the handler is temporary removed
            if (OnClearHandler != null)
            {
                OnClearHandler();
            }
        }
    }
}