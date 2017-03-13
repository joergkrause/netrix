using System;
using System.Collections;

namespace GuruComponents.Netrix.Events
{
    /// <summary>
    /// Delegate of the 
    /// <see cref="ISelection.SelectionChanged">SelectionChanged</see> event, 
    /// defined in the <see cref="ISelection">ISelection</see> interface and deriving class.
    /// </summary>
    public delegate void SelectionChangedEventHandler(object sender, SelectionChangedEventArgs e);

    /// <summary>
    /// Contains the element which was last selected
    /// </summary>
    public class SelectionChangedEventArgs : EventArgs
    {
        private WebEditing.Elements.ElementCollection el;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="elements"></param>
        public SelectionChangedEventArgs(WebEditing.Elements.ElementCollection elements)
        {
            el = elements;
        }

        /// <summary>
        /// Gets a collection of elements containing in the last selection.
        /// </summary>
        /// <remarks>
        /// After insert
        /// operations the collection is just one element - the last inserted. 
        /// </remarks>
        public WebEditing.Elements.ElementCollection SelectedElements
        {
            get
            {
                return el;
            }
        }

        /// <summary>
        /// Return the first or the onw and only element in the collection of
        /// selected element. 
        /// </summary>
        /// <remarks>This is the prefered method to retrieve the current element.</remarks>
        public WebEditing.Elements.IElement SelectedElement
        {
            get
            {
                if (el.Count > 0)
                {
                    return (el as ArrayList)[0] as WebEditing.Elements.IElement;
                } 
                else 
                {
                    return null;
                }
            }
        }

    }

}
