using System;
using System.Collections;
using GuruComponents.Netrix.ComInterop;

namespace GuruComponents.Netrix.WebEditing.Elements
{

    /// <summary>
    /// The class builds the collection of PARAM elements.
    /// </summary>
    /// <remarks>
    /// This class supports the elements OBJECT, EMBED and APPLET. It is used to manage the
    /// collection of parameters added to any of these elements. The class supports the integrated
    /// param collection editor (full version only) and the programmatic access to the parameter elements
    /// (both versions). 
    /// </remarks>
    public class ParameterElementsCollection : System.Collections.CollectionBase
    {
        /// <summary>
        /// Provide one possible type to the propertygrid's collection editor to help them building elements.
        /// </summary>
        public static readonly Type ParamElementType = typeof(GuruComponents.Netrix.WebEditing.Elements.ParamElement);

        private Interop.IHTMLElement objectElement;

        internal ParameterElementsCollection(Interop.IHTMLObjectElement s) : base()
        {
            objectElement = (Interop.IHTMLElement)s;
        }

        public new void Add(object o) 
        {
            if (o == null) return;
            base.InnerList.Add(o);
            OnInsertHandler(base.InnerList.Count, o);
        }

        public new bool Contains(object o) 
        {
            if (o == null) return false;
            return InnerList.Contains(o);
        }

        public new void RemoveAt(int index, object o) 
        {
            if (o == null) return;
            base.InnerList.Remove(o);
            Interop.IHTMLDOMChildrenCollection param = (Interop.IHTMLDOMChildrenCollection) ((Interop.IHTMLDOMNode) objectElement).childNodes;
            for (int i = 0; i < param.length; i++)
            {	
                Interop.IHTMLElement el = (Interop.IHTMLElement) param.item(0);
                if (index == i)
                {
                    Interop.IHTMLDOMNode no = (Interop.IHTMLDOMNode) el;
                    no.removeNode(true);
                }
            }
        }

        public new ParamElement this[int index] 
        {
            get 
            {
                object o = base.InnerList[index];
                return o as ParamElement;
            }
            set
            {
                base.InnerList[index] = value;
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