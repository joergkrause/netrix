using System;

namespace GuruComponents.Netrix.WebEditing.Elements
{
	/// <summary>
	/// Defines an action that simulates a click on a scroll-bar component.
	/// </summary>
	public enum ScrollAction
	{
        /// <summary>
        /// Default. Down scroll arrow is at the specified location. 
        /// </summary>
        ScrollbarDown,
       
        /// <summary>
        /// Horizontal scroll thumb or box is at the specified location.
        /// </summary>
        ScrollbarHThumb,
    
        /// <summary>
        /// Left scroll arrow is at the specified location.
        /// </summary>
        ScrollbarLeft,
    
        /// <summary>
        /// Page-down scroll bar shaft is at the specified location.
        /// </summary>
        ScrollbarPageDown,
        
        /// <summary>
        /// Page-left scroll bar shaft is at the specified location.
        /// </summary>
        ScrollbarPageLeft,
        
        /// <summary>
        /// Page-right scroll bar shaft is at the specified location.
        /// </summary>
        ScrollbarPageRight,
        
        /// <summary>
        /// Page-up scroll bar shaft is at the specified location.
        /// </summary>
        ScrollbarPageUp,
        
        /// <summary>
        /// Right scroll arrow is at the specified location.
        /// </summary>
        ScrollbarRight,
        
        /// <summary>
        /// Up scroll arrow is at the specified location.
        /// </summary>
        ScrollbarUp,
        
        /// <summary>
        /// Vertical scroll thumb or box is at the specified location.
        /// </summary>
        ScrollbarVThumb,
        
        /// <summary>
        /// Composite reference to scrollbarDown.
        /// </summary>
        Down,
        
        /// <summary>
        /// Composite reference to scrollbarLeft.
        /// </summary>
        Left,
        
        /// <summary>
        /// Composite reference to scrollbarPageDown.
        /// </summary>
        PageDown,
        
        /// <summary>
        /// Composite reference to scrollbarPageLeft.
        /// </summary>
        PageLeft,
        
        /// <summary>
        /// Composite reference to scrollbarPageRight.
        /// </summary>
        PageRight,
        
        /// <summary>
        /// Composite reference to scrollbarPageUp.
        /// </summary>
        PageUp,
        
        /// <summary>
        /// Composite reference to scrollbarRight.
        /// </summary>
        Right,
        
        /// <summary>
        /// Composite reference to scrollbarUp.
        /// </summary>
        Up
	}
}
