using System;
using GuruComponents.Netrix.WebEditing.Behaviors;
using System.Collections;

namespace GuruComponents.Netrix.WebEditing.Elements
{
    /// <summary>
    /// This interfaces handles attached binary behaviors.
    /// </summary>
    public interface IElementBehavior
    {
    
        # region Collection
        /// <summary>
        /// Number of behaviors attached
        /// </summary>
        int Count { get; }
        /// <summary>
        /// Get behaviors
        /// </summary>
        /// <returns></returns>
        IDictionaryEnumerator GetEnumerator();

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
        void AddBehavior(IBaseBehavior ElementBehavior, int width, int height);

		/// <summary>
		/// Adds a specific behavior to this element and stores the behavior cookie.
		/// </summary>
		/// <remarks>
		/// To use a binary behavior create a class which inherits directly from 
		/// <see cref="GuruComponents.Netrix.WebEditing.Behaviors.BaseBehavior"/> class. Overwrite the
		/// <see cref="GuruComponents.Netrix.WebEditing.Behaviors.BaseBehavior.Draw(int,int,int,int,IntPtr)">Draw</see> method to give
		/// the behavior a specific graphical context and 
        /// <see cref="GuruComponents.Netrix.WebEditing.Behaviors.BaseBehavior.Name">Name</see> property to have
        /// unique name if multiple behavior are attached the same time.
		/// </remarks>
		/// <param name="elementBehavior"></param>
		void AddBehavior(IBaseBehavior elementBehavior);

        /// <summary>
        /// Has at least one behavior attached.
        /// </summary>
        bool HasBehavior();

        /// <summary>
        /// The specified behavior is attached to this element.
        /// </summary>
        /// <param name="elementBehavior"></param>
        bool HasBehavior(IBaseBehavior elementBehavior);

        /// <summary>
        /// Returns the names of the instances of all attached behaviors.
        /// </summary>
        /// <returns>Collection of names of the attched behavior instances.</returns>
        /// <remarks>
        /// Behaviors are supposed to have unique names. This methods returns the 
        /// active instances with unique names. Behaviors which break the rule may not
        /// appear correctly.
        /// </remarks>
        ICollection GetBehaviorNames();

        /// <summary>
        /// Remove all behaviors from element.
        /// </summary>
        void RemoveBehavior();

        /// <summary>
        /// Remove the given behavior from element.
        /// </summary>
        /// <remarks>
        /// If the behavior has forced setting the width and/or height of the element to beeing displayed
        /// this method will remove these style attributes. If other parts of the app uses tese values to
        /// you must assure that the previously set values are reset.
        /// </remarks>
        /// <param name="ElementBehavior">The behavior that was previously attached.</param>
		void RemoveBehavior(IBaseBehavior ElementBehavior);

        # endregion

    }
}
