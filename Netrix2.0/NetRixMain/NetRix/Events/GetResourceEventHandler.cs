using System;

namespace GuruComponents.Netrix.Events
{
    /// <summary>
    /// Fired if the control requests a embedded resource, e.g. an image or object.
    /// </summary>
    public delegate void GetResourceEventHandler(Object s, GetResourceEventArgs e);

}
