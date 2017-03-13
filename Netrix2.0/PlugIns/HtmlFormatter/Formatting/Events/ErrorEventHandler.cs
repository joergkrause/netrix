using System;
using System.IO;

namespace GuruComponents.Netrix.HtmlFormatting
{

    /// <summary>
    /// Used for error event, called if the formatted cannot resolve the content.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void ErrorEventHandler(object sender, ErrorEventArgs e);

    /// <summary>
    /// The Event Arguments for the Error Event.
    /// </summary>
    /// <remarks>
    /// The Error event is fired than the given code can not satisfy the output tree.
    /// </remarks>
    public class ErrorEventArgs
    {

        private string t1Text, t2Text;
        private int t1Length, t2Length;

        internal ErrorEventArgs(Token t1, Token t2)
        {
            t1Text = t1.Text;
            t2Text = t2.Text;
            t1Length = t1.Length;
            t2Length = t2.Length;
        }

        /// <summary>
        /// The text of the token before the error
        /// </summary>
        public string TokenBeforeText
        {
            get
            {
                return t1Text;
            }
        }

        /// <summary>
        /// The text of the token after the error
        /// </summary>
        public string TokenAfterText
        {
            get
            {
                return t2Text;
            }
        }
    
        /// <summary>
        /// The length of the token before the error
        /// </summary>
        public int TokenBeforeLength
        {
            get
            {
                return t1Length;
            }
        }

        /// <summary>
        /// The length of the token after the error
        /// </summary>
        public int TokenAfterLength
        {
            get
            {
                return t2Length;
            }
        }

    }
}
