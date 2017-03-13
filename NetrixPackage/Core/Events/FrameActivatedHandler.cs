namespace GuruComponents.Netrix.Events
{

    /// <summary>
    /// This delegate points to to a handler which is fired when the user clicks on 
    /// a different frame. 
    /// </summary>
    /// <remarks>The currently activated frame will fire the event and inform the 
    /// handler about frame name and addtional framedata.</remarks>
    /// <seealso cref="FrameEventArgs"/>
	public delegate void FrameActivatedEventHandler(object sender, FrameEventArgs e);

}