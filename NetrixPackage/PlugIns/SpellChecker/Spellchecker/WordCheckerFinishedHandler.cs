namespace GuruComponents.Netrix.SpellChecker
{
    /// <summary>
    /// Event, fired if the word checker is ready with whole document.
    /// </summary>
    /// <remarks>
    /// During background checking processes the event is fired after each run through the document.
    /// </remarks>
    /// <seealso cref="WordCheckerEventArgs"/>
    public delegate void WordCheckerFinishedHandler(object sender, WordCheckerEventArgs e);

}