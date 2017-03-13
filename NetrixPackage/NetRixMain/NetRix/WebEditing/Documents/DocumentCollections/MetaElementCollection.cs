using System;
using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.WebEditing.Elements;

namespace GuruComponents.Netrix.WebEditing.Documents
{
    /// <summary>
    /// A collection of meta tags (&lt;meta name="" ... &gt;) or HTTP-EQUIV meta tags
    /// </summary>
    public class MetaElementCollection : System.Collections.CollectionBase, ICollectionBase
    {

        internal Interop.IHTMLDocument2 msHtmlDocument;
        private HtmlEditor htmlEditor;

        internal MetaElementCollection(HtmlEditor htmlEditor) : base()
        {
            this.htmlEditor = htmlEditor;
        }

        /// <summary>
        /// Adds a meta element to the collection. This member supports the NetRix infrastructure and is used to 
        /// support the <see cref="System.Windows.Forms.PropertyGrid"/>.
        /// </summary>
        /// <param name="metaElement"></param>
        public void Add(MetaElement metaElement) 
        {
            if (metaElement == null) return;
            base.List.Add(metaElement);
        }

        public IElement Create(MetaTagType metaTagType, string name, string content)
        {
            Interop.IHTMLMetaElement meta = this.msHtmlDocument.CreateElement("META") as Interop.IHTMLMetaElement;
            if (meta != null)
            {
                switch (metaTagType)
                {
                    case MetaTagType.HttpEquiv:
                        meta.httpEquiv = name;
                        break;
                    case MetaTagType.Named:
                        meta.name = name;
                        break;
                }
                meta.content = content;
                IElement el = htmlEditor.GenericElementFactory.CreateElement((Interop.IHTMLElement) meta) as IElement;
                this.OnInsertComplete(0, el);
                return el;
            }
            return null;
        }

        /// <summary>
        /// Checks if this meta element is part of the collection. This member supports the NetRix infrastructure and is used to 
        /// support the <see cref="System.Windows.Forms.PropertyGrid"/>.
        /// </summary>
        /// <param name="metaElement"></param>
        /// <returns></returns>
        public bool Contains(MetaElement metaElement) 
        {
            if (metaElement == null) return false;
            return List.Contains(metaElement);
        }

        /// <summary>
        /// Remove the meta element from the collection. This member supports the NetRix infrastructure and is used to 
        /// support the <see cref="System.Windows.Forms.PropertyGrid"/>.
        /// </summary>
        /// <param name="metaElement"></param>
        public void Remove(MetaElement metaElement) 
        {
            if (metaElement == null) return;
            base.List.Remove(metaElement);
        }

        /// <summary>
        /// Gets or sets a meta element in then collection using an index. This member supports the NetRix infrastructure and is used to 
        /// support the <see cref="System.Windows.Forms.PropertyGrid"/>.
        /// </summary>
        public MetaElement this[int index] 
        {
            get 
            {
                return (MetaElement)base.List[index];
            }
            set
            {
                base.List[index] = value;
            }
        }

        /// <summary>
        /// Fired if the collection editor inserts a new element. This member supports the NetRix infrastructure and is used to 
        /// support the <see cref="System.Windows.Forms.PropertyGrid"/>.
        /// </summary>
        public event CollectionInsertHandler OnInsertHandler;
        /// <summary>
        /// Fired if the collection editor starts a new sequence. This member supports the NetRix infrastructure and is used to 
        /// support the <see cref="System.Windows.Forms.PropertyGrid"/>.
        /// </summary>
        public event CollectionClearHandler OnClearHandler;

        /// <summary>
        /// Fired if an element is being removed.
        /// </summary>
        public event CollectionRemoveHandler OnRemoveHandler;

        protected override void OnRemove(int index, object value)
        {
            base.OnRemove(index, value);
            if (OnRemoveHandler != null)
            {
                OnRemoveHandler(index, value);
            }
        }

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
		#region ICollectionBase Members

		void GuruComponents.Netrix.WebEditing.Documents.ICollectionBase.Add(object obj)
		{
			if (obj is MetaElement)
			{
				this.Add(obj as MetaElement);
			}
		}

		#endregion
	}
}