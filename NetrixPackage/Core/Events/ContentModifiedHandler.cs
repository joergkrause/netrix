using System;
using System.Windows.Forms;
using System.Drawing;

using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.WebEditing.Elements;

namespace GuruComponents.Netrix.Events
{

    /// <summary>
    /// Fired if the control becomes Dirty the first time.
    /// </summary>
    /// <remarks>
    /// The event is fired if the Dirty flag changes from "not dirty" to "dirty". If the
    /// dirty flag is reset after this and becomes dirty later the event is fired again.
    /// </remarks>
	public delegate void ContentModifiedHandler(object sender, ContentModifiedEventArgs e);

}