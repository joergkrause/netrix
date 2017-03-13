using System;
using GuruComponents.Netrix.WebEditing.Elements;

namespace GuruComponents.Netrix.WebEditing.Documents
{

    /// <summary>
    /// This class provides support for the script element editor.
    /// </summary>
    /// <remarks>
    /// The programmatic access to the script element collection can be used to control any script block
    /// in the head section of the document. New script elements are alwas inserted in the head section.
    /// To control the order the list must be removed completely and the elements must be add in the requested 
    /// order. 
    /// </remarks>
    /// <example>
    /// The following example shows how to insert a JavaScript section if the user changes the value of any
    /// script event in the propertygrid. To have the current element in the propertygrid, attach the
    /// HtmlElementChanged event of the base class to the appropriate handler and set the current element
    /// to the SelectedObject property. Then attach the PropertyValueChanged event to an handler. The following 
    /// code shows that handler and the code needed to insert an new script element on any change.
    /// <code>
/// private void propertyGrid1_PropertyValueChanged(object s, System.Windows.Forms.PropertyValueChangedEventArgs e)
/// {
///     if (e.ChangedItem.Label.StartsWith("Script"))
///   {
///     ScriptElement script = new ScriptElement();
///     StringBuilder sb = new StringBuilder();
///     sb.AppendFormat("function {0}()", e.ChangedItem.Value.ToString());
///     sb.Append(Environment.NewLine);
///     sb.Append("{" + Environment.NewLine);
///     sb.Append("}" + Environment.NewLine);
///     script.ScriptContent = sb.ToString();
///     this.htmlEditor3.DocumentStructure.ScriptBlocks.Add(script);
///   }
/// }
    /// </code>
    /// In reality this method must be enhanced to support changes and removing. It is recommended to investigate
    /// the event argument <c>e</c> to see how to gather more information about the current changed property.
    /// </example>
    public class ScriptElementCollection : System.Collections.CollectionBase, ICollectionBase
    {
        /// <summary>
        /// Adds a script element to the collection. This member supports the NetRix infrastructure and is used to 
        /// support the <see cref="System.Windows.Forms.PropertyGrid"/>.
        /// </summary>
        /// <param name="o"></param>
        public new void Add(ScriptElement o) 
        {
            base.InnerList.Add(o);
            if (OnInsertHandler != null)
            {
                OnInsertHandler(base.InnerList.Count, o);
            }
        }

        /// <summary>
        /// Checks if the element is part of the collection. This member supports the NetRix infrastructure and is used to 
        /// support the <see cref="System.Windows.Forms.PropertyGrid"/>.
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public new bool Contains(ScriptElement o) 
        {
            return InnerList.Contains(o);
        }

        /// <summary>
        /// Removes the element from the collection. This member supports the NetRix infrastructure and is used to 
        /// support the <see cref="System.Windows.Forms.PropertyGrid"/>.
        /// </summary>
        /// <param name="o"></param>
        public new void Remove(ScriptElement o) 
        {
            base.InnerList.Remove(o);
        }

        protected override void OnRemove(int index, object value)
        {
            base.OnRemove(index, value);
            if (OnRemoveHandler != null)
            {
                OnRemoveHandler(index, value);
            }
        }

        public event CollectionRemoveHandler OnRemoveHandler;

        /// <summary>
        /// Clears the list and calles the <see cref="OnClearHandler"/> event.
        /// </summary>
        /// <seealso cref="OnClearHandler"/>
        public new void Clear()
        {
            base.InnerList.Clear();
            if (OnClearHandler != null)
            {
                OnClearHandler();
            }
        }

        /// <summary>
        /// Gets or sets a script element in the collection using an index. This member supports the NetRix infrastructure and is used to 
        /// support the <see cref="System.Windows.Forms.PropertyGrid"/>.
        /// </summary>
        public ScriptElement this[int index] 
        {
            get 
            {
                return (ScriptElement)base.InnerList[index];
            }
            set
            {
                base.InnerList[index] = value;
            }
        }

        /// <summary>
        /// Fired if the collection editor inserts a new element. 
        /// </summary>
        /// <remarks>
        /// This member supports the NetRix infrastructure and is used to 
        /// support the <see cref="System.Windows.Forms.PropertyGrid"/>.
        /// </remarks>
        public event CollectionInsertHandler OnInsertHandler;

        /// <summary>
        /// Fired if the colection editor starts a new sequence.
        /// </summary>
        /// <remarks>
        /// This member supports the NetRix infrastructure and is used to 
        /// support the <see cref="System.Windows.Forms.PropertyGrid"/>.
        /// </remarks>
        public event CollectionClearHandler OnClearHandler;

        protected override void OnInsertComplete(int index, object value)
        {
            base.OnInsert (index, value);
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
			if (obj is ScriptElement)
			{
				this.Add(obj as ScriptElement);
			}
		}

		#endregion
	}
}