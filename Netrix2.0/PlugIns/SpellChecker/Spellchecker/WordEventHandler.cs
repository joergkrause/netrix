using System;
using System.Collections.Generic;
using System.Text;
using GuruComponents.Netrix.SpellChecker.NetSpell;

namespace GuruComponents.Netrix.SpellChecker
{
    /// <summary>
    /// Delegate that supports the spell checker events.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void WordEventHandler(object sender, SpellingEventArgs e);
}
