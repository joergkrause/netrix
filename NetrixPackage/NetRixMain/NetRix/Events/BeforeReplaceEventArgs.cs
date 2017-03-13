using System;
using System.Windows.Forms;

namespace GuruComponents.Netrix.Events
{

    /// <summary>
    /// This class defines data for the <see cref="GuruComponents.Netrix.HtmlEditor.BeforeReplace">BeforeReplace</see> event.
    /// </summary>
    /// <remarks>
    /// This class allows cancellation of replacement by setting the <see cref="Result"/> property.
    /// </remarks>
    public class BeforeReplaceEventArgs : System.EventArgs
    {
        private string word;
        private int counter;
        private DialogResult result;

        internal BeforeReplaceEventArgs(string word, int counter)
        {
            this.word = word;
            this.counter = counter;
            this.result = DialogResult.OK;
        }

        /// <summary>
        /// Sets the result of the handler.
        /// </summary>
        /// <remarks>
        ///  The various fields of the <see cref="System.Windows.Forms.DialogResult">DialogResult</see> enumeration have the following meaning:
        /// <list type="bullet">
        /// <item><term>Yes</term><description>Replace</description></item>
        /// <item><term>OK</term><description>Replace</description></item>
        /// <item><term>None</term><description>End method and stop all replacements (e.g. Cancel)</description></item>
        /// <item><term>Cancel</term><description>End method and stop all replacements (e.g. Cancel)</description></item>
        /// <item><term>Abort</term><description>End method and stop all replacements (e.g. Cancel)</description></item>
        /// <item><term>No</term><description>Skip this (current) replacement, but continue.</description></item>
        /// <item><term>Ignore</term><description>Skip this (current) replacement, but continue.</description></item>
        /// <item><term>Retry</term><description>Start replacement at the beginning. (Helpful after some words were skipped)</description></item>
        /// </list>
        /// </remarks>
        public DialogResult Result
        {
            set
            {
                this.result = value;
            }
        }

        /// <summary>
        /// Returns the word counter result.
        /// </summary>                      
        /// <remarks>
        /// The number of words the <see cref="GuruComponents.Netrix.HtmlEditor.Replace(string,string,bool,bool,bool)">Replace</see> method has
        /// already replace or is about to replace, if the next action is allowed. The counter increases before the
        /// next word is replaced, but after the word is selected for replacement.
        /// </remarks>
        public int Counter
        {
            get
            {
                return this.counter;
            }
        }

        internal DialogResult GetResult()
        {
            return this.result;
        }


    }
}