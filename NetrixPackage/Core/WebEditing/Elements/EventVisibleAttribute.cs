using System;
using System.Collections.Generic;
using System.Text;

namespace GuruComponents.Netrix.WebEditing.Elements
{
    /// <summary>
    /// Used in a derived element class to tag an event to make the event visible within the PropertyGrid event tab.
    /// </summary>
    /// <remarks>
    /// This attribute supports a design environment and has no effect at runtime.
    /// </remarks>
    public class EventVisibleAttribute : Attribute
    {
    }

    /// <summary>
    /// Used to tag scripting events.
    /// </summary>
    public class ScriptingVisibleAttribute : Attribute
    {
    }
}
