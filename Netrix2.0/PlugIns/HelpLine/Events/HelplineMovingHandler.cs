using System;

namespace GuruComponents.Netrix.HelpLine.Events
{
    /// <summary>
    /// This event is fired during the helpline moving. It can be used to update a position display.
    /// </summary>
	public delegate void HelpLineMoving(object sender, HelplineMovedEventArgs e);
}
