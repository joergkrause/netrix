using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;

namespace GuruComponents.Netrix.SpellChecker
{

	/// <summary>
	/// The arguments used with <see cref="Speller.WordOnContext"/> event, which is fired after right click on word.
	/// </summary>
    public class WordOnContextEventArgs : EventArgs
    {

        private string word;
        private string html;
        private Point pos;
        private List<string> suggestions;

        /// <summary>
        /// A list of suggestions for the word.
        /// </summary>
        public List<string> Suggestions
        {
            get { return suggestions; }
            set { suggestions = value; }
        }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="word"></param>
        /// <param name="html"></param>
        /// <param name="pos"></param>
        public WordOnContextEventArgs(string word, string html, Point pos)
        {
            this.word = word;
            this.html = html;
            this.pos  = pos;
        }

        /// <summary>
        /// The word (text) currently recognized as under pointer.
        /// </summary>
        public string Word
        {
            get
            {
                return word;
            }
        }

        /// <summary>
        /// The word (with HTML formatting) recognized as under pointer.
        /// </summary>
        public string Html
        {
            get
            {
                return html;
            }
        }

        /// <summary>
        /// The location of the contextmenu following the current mouse spot.
        /// </summary>
        public Point Position
        {
            get
            {
                return pos;
            }
        }





    }

}
