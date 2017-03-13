using System;
using System.Collections;
using GuruComponents.Netrix.ComInterop;

namespace GuruComponents.Netrix.WebEditing.Elements
{
    /// <summary>
    /// Stores a collection of option elements.
    /// </summary>
    /// <remarks>
    /// This class stores OPTION and OPTGROUP elements in any combination.
    /// </remarks>
    public class OptionElementsCollection : System.Collections.CollectionBase
    {
        /// <summary>
        /// Provide one possible type to the propertygrid's collection editor to help them building elements
        /// </summary>
        public static readonly Type OptionElementType = typeof(GuruComponents.Netrix.WebEditing.Elements.OptionElement);
        /// <summary>
        /// Provide one possible type to the propertygrid's collection editor to help them building elements
        /// </summary>
        public static readonly Type OptGroupElementType = typeof(GuruComponents.Netrix.WebEditing.Elements.OptGroupElement);


        private Interop.IHTMLElement selectElement;

        internal OptionElementsCollection(Interop.IHTMLSelectElement s, IHtmlEditor editor) : base()
        {
            selectElement = (Interop.IHTMLElement) s;
            Interop.IHTMLElementCollection options = (Interop.IHTMLElementCollection) selectElement.GetChildren();
            for (int i = 0; i < options.GetLength(); i++)
            {	
                Interop.IHTMLElement el = (Interop.IHTMLElement) options.Item(i, i);
                object oe = null;
                switch (el.GetTagName())
                {
                    case "OPTION":
                        oe = new OptionElement(el, editor);
                        break;
                    case "OPTGROUP":
                        oe = new OptGroupElement(el, editor);
                        break;
                }
                if (oe != null)
                {
                    InnerList.Add(oe);
                }
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
            Interop.IHTMLElementCollection options = (Interop.IHTMLElementCollection) selectElement.GetChildren();
            for (int i = 0; i < options.GetLength(); i++)
            {	
                Interop.IHTMLElement el = (Interop.IHTMLElement) options.Item(i, i);
                if (index == i)
                {
                    Interop.IHTMLDOMNode no = (Interop.IHTMLDOMNode) el;
                    no.removeNode(true);
                }
            }
        }

        public new IOptionElement this[int index] 
        {
            get 
            {
                object o = base.InnerList[index];
                switch (o.GetType().Name)
                {
                    case "OptionElement":
                        return o as OptionElement;
                    case "OptGroupElement":
                        return o as OptGroupElement;
                }
                return null;
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
            base.OnInsertComplete (index, value);
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