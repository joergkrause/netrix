using System;
using System.ComponentModel;
using System.Collections;
using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.WebEditing.Elements;
using System.Web.UI;

namespace GuruComponents.Netrix.WebEditing.Elements
{
    /// <summary>
    /// This class holds a collection of elements.
    /// </summary>
    /// <remarks>
    /// ElementCollection is a datasource implementation which holds an 
    /// array of <see cref="GuruComponents.Netrix.WebEditing.Elements.IElement">IElement</see> objects 
    /// to allow enumerable access from host application. The goal is to have a 
    /// element collection which can used as a datasource rather than an arraylist, which makes
    /// copies of elements. Copying elements is not the common way because the element objects are
    /// really huge.
    /// <seealso cref="GuruComponents.Netrix.WebEditing.Elements.IElement">IElement</seealso>
    /// </remarks>
    public class ElementCollection :  ArrayList 
    {
        /// <summary>
        /// Adds a new element to the collection.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public int Add(IElement value)
        {
            return base.Add (value);
        }

        /// <summary>
        /// Adds a control to the collection.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public int Add(Control value)
        {
            return base.Add(value);
        }

        /// <summary>
        /// Returns an element or a control which implements IElement.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
		public new IElement this[int index]
		{
			get
			{
				return base[index] as IElement;
			}
			set
			{ 
				base[index] = value;
			}
		}

        /// <summary>
        /// Get a control at the specified index, event if it's not implementing IElement.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Control GetControlAtIndex(int index)
        {
            return base[index] as Control;
        }

    }
}