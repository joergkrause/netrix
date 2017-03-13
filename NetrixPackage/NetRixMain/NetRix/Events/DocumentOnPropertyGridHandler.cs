using System;

namespace GuruComponents.Netrix.Events
{
    /// <summary>
    /// This delegate will fire an attached event handler if the user clicks on the property button
    /// on an external PropertyGrid. Without a PropertyGrid the underlying event does nothing.
    /// </summary>
	public delegate void DocumentOnPropertyGridHandler();
}
