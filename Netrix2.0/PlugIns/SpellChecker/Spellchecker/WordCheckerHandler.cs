namespace GuruComponents.Netrix.SpellChecker
{
	/// <summary>
	/// The callback function attached here is called for any word found.
	/// </summary>
	/// <remarks>
	/// The handler
	/// should return true if the word is ok or false if it is wrong.
	/// </remarks>
	public delegate bool WordCheckerHandler(object sender, WordEventArgs e);

}