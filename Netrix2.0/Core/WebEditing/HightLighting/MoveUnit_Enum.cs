using System;
          
namespace GuruComponents.Netrix.WebEditing.HighLighting
{
    /// <summary>
    /// To move a pointer a given number of units this enumeration indication the kind of unit.
    /// </summary>
    public enum MoveUnit
    {
        /// <summary>
        /// Move the give number of characters.
        /// </summary>
        Character,
        /// <summary>
        /// Move the give number of words.
        /// </summary>
        Word,
        /// <summary>
        /// Move the give number of sentences.
        /// </summary>
        Sentence,
        /// <summary>
        /// Move back to the original pointer locations.
        /// </summary>
        TextEdit
    }

}
