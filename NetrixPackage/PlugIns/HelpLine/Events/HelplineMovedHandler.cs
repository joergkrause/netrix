using System;

namespace GuruComponents.Netrix.HelpLine.Events
{
    /// <summary>
    /// This event is fired after the helpline was moved. It can be used to update a position display.
    /// </summary>
	public delegate void HelpLineMoved(object sender, HelplineMovedEventArgs e);
}
