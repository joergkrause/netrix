using System;
using System.Collections.Generic;
using System.Text;
using GuruComponents.Netrix.WebEditing.Behaviors;
using System.Collections.Specialized;
using System.Collections;
using GuruComponents.Netrix.ComInterop;

namespace GuruComponents.Netrix.WebEditing.Elements
{
    /// <summary>
    /// This class handles attached binary behaviors.
    /// </summary>
    /// <remarks>
    /// Dynamic HTML (DHTML) behaviors can be implemented in script as well as in compiled languages, such as C++. 
    /// NetRix supports the compiled version and fully supports usage with C#, VB.NET, and other .NET languages.
    /// </remarks>
    public class ElementBehavior : IElementBehavior
    {

        private HybridDictionary behaviorCookie = new HybridDictionary(2);
        private bool behaviorWidthHeightCustomized = false; // true, if widht/height was added
        private IElement element;        

        /// <summary>
        /// Creates an object which holds all behavior information for the specified element object.
        /// </summary>
        /// <remarks>
        /// This creates a 1:1 relation between element and its behaviors, whereas the ElementBehavior class
        /// can handle an unlimited number of behaviors for the element. This results in the following:
        /// 1:1:n (Element:ElementBehavior:Behaviors).
        /// </remarks>
        /// <param name="element"></param>
        public ElementBehavior(IElement element)
        {
            this.element = element;
        }

        # region Collection

        /// <summary>
        /// Number of behaviors attached.
        /// </summary>
        public int Count
        {
            get
            {
                return behaviorCookie.Count;
            }
        }

        /// <summary>
        /// Get collection of all attached behaviors.
        /// </summary>
        /// <returns></returns>
        public IDictionaryEnumerator GetEnumerator()
        {
            return behaviorCookie.GetEnumerator();
        }

        # endregion

        #region Behavior

        /// <summary>
        /// Adds a specific behavior to the element and give them a rectangle measures.
        /// </summary>
        /// <remarks>
        /// This is used because 
        /// binary behaviors can only get visible for elements having a rectangle format. This is
        /// done by adding style-width and style-height. Remark: Removing behavior will remove the
        /// style attributes, but doing not so will remain the attributes in the document after saving.
        /// If the element has already set the attributes, they will not change, even if they are
        /// set in a way that makes the element unvisible.
        /// </remarks>
        /// <param name="ElementBehavior">The behavior object</param>
        /// <param name="width">Width in Pixel</param>
        /// <param name="height">Height in Pixel</param>
        public void AddBehavior(IBaseBehavior ElementBehavior, int width, int height)
        {
            // checkout if it is an element without width/height measures, MSHTML does not show such elements,
            // therefore we attach this here and save this state to remove it later
            if (element.GetStyleAttribute("width").Equals(String.Empty) && element.GetStyleAttribute("height").Equals(String.Empty))
            {
                element.SetStyleAttribute("width", String.Format("{0}px", width));
                element.SetStyleAttribute("height", String.Format("{0}px", height));
                behaviorWidthHeightCustomized = true;
            } 
            else 
            {
                behaviorWidthHeightCustomized = false;
            }
            AddBehavior(ElementBehavior);
        }

		/// <summary>
		/// Adds a specific behavior to this element and stores the behavior cookie.
		/// </summary>
		/// <remarks>
		/// To use a binary behavior create a class which inherits directly from 
        /// <see cref="GuruComponents.Netrix.WebEditing.Behaviors.BaseBehavior">BaseBehavior</see> class. Overwrite the
		/// Draw method to give
		/// the behavior a specific graphical context and 
        /// <see cref="GuruComponents.Netrix.WebEditing.Behaviors.BaseBehavior.Name">Name</see> property to have
        /// unique name if multiple behavior are attached the same time.
		/// </remarks>
		/// <param name="elementBehavior"></param>
		public void AddBehavior(IBaseBehavior elementBehavior)
		{
			if (this.element.GetBaseElement() == null)
			{
				throw new ArgumentException("Cannot assign a behavior to not initialised element");
			} 
			else 
			{                
                object o = elementBehavior;
                string bName = elementBehavior.Name;
                //System.Diagnostics.Debug.WriteLine(bName, "Add");
                if (!behaviorCookie.Contains(bName)) // do not attach same behavior twice for this element
                {
                    behaviorCookie.Add(bName, ((Interop.IHTMLElement2) element.GetBaseElement()).AddBehavior(bName, ref o));
                }
			}
		}

        /// <summary>
        /// Has at least one behavior attached.
        /// </summary>
        public bool HasBehavior()
        {
            return behaviorCookie.Count > 0;
        }

        /// <summary>
        /// The specified behavior is attached to this element.
        /// </summary>
        /// <param name="elementBehavior"></param>
        public bool HasBehavior(IBaseBehavior elementBehavior)
        {
            return behaviorCookie.Contains(elementBehavior);
        }

        /// <summary>
        /// Returns the names of the instances of all attached behaviors.
        /// </summary>
        /// <returns>Collection of names of the attched behavior instances.</returns>
        /// <remarks>
        /// Behaviors are supposed to have unique names. This methods returns the 
        /// active instances with unique names. Behaviors which break the rule may not
        /// appear correctly.
        /// </remarks>
        public ICollection GetBehaviorNames()
        {
            return behaviorCookie.Keys;
        }

        /// <summary>
        /// Remove all behaviors from element.
        /// </summary>
        public void RemoveBehavior()
        {            
            if (this.behaviorCookie.Count > 0)
            {
                foreach (DictionaryEntry cookieEntry in this.behaviorCookie)
                {
                    ((Interop.IHTMLElement2) element.GetBaseElement()).RemoveBehavior((int) cookieEntry.Value);
                }
                this.behaviorCookie.Clear();
            }
        }

        /// <summary>
        /// Remove the given behavior from element.
        /// </summary>
        /// <remarks>
        /// If the behavior has forced setting the width and/or height of the element to beeing displayed
        /// this method will remove these style attributes. If other parts of the app uses tese values to
        /// you must assure that the previously set values are reset.
        /// </remarks>
        /// <param name="ElementBehavior">The behavior that was previously attached.</param>
		public void RemoveBehavior(IBaseBehavior ElementBehavior)
		{
            if (ElementBehavior == null)
            {
                RemoveBehavior();
                return;
            }
			if (element.GetBaseElement() == null)
			{
				throw new ArgumentException("Cannot remove a behavior cause element is uninitialized or behavior is not set");
			} 
			else 
			{
                if (behaviorWidthHeightCustomized)
                {
                    element.RemoveStyleAttribute("width");
                    element.RemoveStyleAttribute("height");
                    behaviorWidthHeightCustomized = false;
                }
                if (behaviorCookie.Contains(ElementBehavior.Name))
                {
                    ((Interop.IHTMLElement2) element.GetBaseElement()).RemoveBehavior((int)behaviorCookie[ElementBehavior.Name]);
                    behaviorCookie.Remove(ElementBehavior.Name);
                }
			}
		}

        # endregion

    }
}
