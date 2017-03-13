using System;

namespace GuruComponents.Netrix.WebEditing.Elements
{
	/// <summary>
	/// Zusammenfassung für ElementComponent.
	/// </summary>
	public enum ElementComponent
	{
        /// <summary>
        /// Component is inside the client area of the object.
        /// </summary>
        ClientArea,
        /// <summary>
        /// Component is outside the bounds of the object.
        /// </summary>
        Outside,
        /// <summary>
        /// Down scroll arrow is at the specified location.
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
        /// Bottom sizing handle is at the specified location.
        /// </summary>
        HandleBottom,
        /// <summary>
        /// Lower-left sizing handle is at the specified location.
        /// </summary>
        HandleBottomLeft,
        /// <summary>
        /// Lower-right sizing handle is at the specified location.
        /// </summary>
        HandleBottomRight,
        /// <summary>
        /// Left sizing handle is at the specified location.
        /// </summary>
        HandleLeft,
        /// <summary>
        /// Right sizing handle is at the specified location.
        /// </summary>
        HandleRight,
        /// <summary>
        /// Top sizing handle is at the specified location.
        /// </summary>
        HandleTop,
        /// <summary>
        /// Upper-left sizing handle is at the specified location.
        /// </summary>
        HandleTopLeft,
        /// <summary>
        /// Upper-right sizing handle is at the specified location.
        /// </summary>
        HandleTopRight,
        /// <summary>
        /// Is on bottom border zone, but not on a handle.
        /// </summary>
        BottomBorder,
        /// <summary>
        /// Is on bottom border zone, but not on a handle.
        /// </summary>
        TopBorder,
        /// <summary>
        /// Is on top top zone, but not on a handle.
        /// </summary>
        LeftBorder,
        /// <summary>
        /// Is on bottom right zone, but not on a handle.
        /// </summary>
        RightBorder,
        /// <summary>
        /// Just indicate end of enum. Do not use.
        /// </summary>
        MaxValue
	}
}
