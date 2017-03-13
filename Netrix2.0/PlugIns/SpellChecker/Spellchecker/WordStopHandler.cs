using System;
using System.Collections;

namespace GuruComponents.Netrix.SpellChecker
{

	/// <summary>
	/// The handler should return true if the word checker must stop.
	/// </summary>
	public delegate bool WordCheckerStopHandler(object sender, EventArgs e);

}