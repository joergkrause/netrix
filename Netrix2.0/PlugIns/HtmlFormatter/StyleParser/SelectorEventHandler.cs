namespace GuruComponents.Netrix.UserInterface.StyleParser
{
    /// <summary>
    /// Used to define a event which is fired on each selector recognized.
    /// </summary>
    /// <remarks>
    /// This event handler is used internally by the style parser class. It may be used
    /// to enhance the behavior by an external handler.
    /// </remarks>
	public delegate void SelectorEventHandler(object sender, SelectorEventArgs e);
}
