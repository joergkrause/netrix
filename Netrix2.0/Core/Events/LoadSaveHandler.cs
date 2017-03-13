using System;

namespace GuruComponents.Netrix.Events
{
    /// <summary>
    /// Fired before the load process starts.
    /// </summary>
	public delegate void LoadEventHandler(object sender, LoadEventArgs e);

    /// <summary>
    /// Fired before a save operation starts.
    /// </summary>
    public delegate void SaveEventHandler(object sender, SaveEventArgs e);

    /// <summary>
    /// Fired if an new element was created.
    /// </summary>
	/// <remarks>This event does not fire if the element has been created by drag 'n drop or paste operations.</remarks>
    public delegate void CreatedEventHandler(object sender, EventArgs e);

}
