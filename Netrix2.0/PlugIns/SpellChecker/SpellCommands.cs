using System;
using System.ComponentModel.Design;

namespace GuruComponents.Netrix.SpellChecker
{
    internal enum SpellCommand : int
    {
        StartWordByWord    = 100,
        StopWordByWord     = 101,
        StartBackground    = 102,
        StopBackground     = 103,
        StartBlock         = 104,
        StopBlock          = 105,

        RemoveHighLight    = 110,
        ClearBuffer        = 111
    }

    internal class SpellCommandToolInvokeArgs : EventArgs
    {
        internal SpellCommandToolInvokeArgs(IHtmlEditor associatedEditor, SpellCommand invokedCommand)
        {
            AssociatedEditor = associatedEditor;
            InvokedCommand = invokedCommand;
        }
        private IHtmlEditor _associatededitor;
        internal IHtmlEditor AssociatedEditor
        {
            get { return _associatededitor;}
            set { _associatededitor =value;}
        }

        private SpellCommand _invokedcommand;
        internal SpellCommand InvokedCommand
        {
            get { return _invokedcommand; }
            set { _invokedcommand = value; }
        }

    }

	/// <summary>
	/// Speller Commands this extender provides.
	/// </summary>
    /// <remarks>
    /// You must invoke the commands using the <c>Invoke</c> method in the HtmlEditor base class.
    /// Commands can not have parameters. In fact, one must set the speller properties accordingly before issuing a command.
    /// </remarks>
	public class SpellCommands
	{

        private Guid hlDesignerGuid;

        internal SpellCommands()
        {
            hlDesignerGuid = Guid.NewGuid(); // assure that each instance has its own GUID!
            StartWordByWord = new CommandID(hlDesignerGuid, (int)SpellCommand.StartWordByWord);
            StopWordByWord = new CommandID(hlDesignerGuid, (int)SpellCommand.StopWordByWord);
            StartBackground = new CommandID(hlDesignerGuid, (int)SpellCommand.StartBackground);
            StopBackground = new CommandID(hlDesignerGuid, (int)SpellCommand.StopBackground);
            StartBlock = new CommandID(hlDesignerGuid, (int)SpellCommand.StartBlock);
            StopBlock = new CommandID(hlDesignerGuid, (int)SpellCommand.StopBlock);

            
            RemoveHighLight = new CommandID(hlDesignerGuid, (int)SpellCommand.RemoveHighLight);
            ClearBuffer = new CommandID(hlDesignerGuid, (int)SpellCommand.ClearBuffer);
        }

        /// <summary>
        /// The command group's GUID this plugin supports.
        /// </summary>
        public Guid CommandGroup
        {
            get
            {
                return this.hlDesignerGuid;
            }
        }

        /// <summary>
        /// Start spelling word by word.
        /// </summary>
        public readonly CommandID StartWordByWord    ;
        /// <summary>
        /// Stop spelling word by word.
        /// </summary>
        public readonly CommandID StopWordByWord     ;
        /// <summary>
        /// Start spelling as-you-type.
        /// </summary>
        public readonly CommandID StartBackground    ;
        /// <summary>
        /// Stop spelling as-you-type.
        /// </summary>
        public readonly CommandID StopBackground     ;
        /// <summary>
        /// Start spelling within the currently selected block of text.
        /// </summary>
        public readonly CommandID StartBlock    ;
        /// <summary>
        /// Stop spelling within the currently selected block of text.
        /// </summary>
        public readonly CommandID StopBlock     ;
        /// <summary>
        /// Remove all highlight segments.
        /// </summary>
        public readonly CommandID RemoveHighLight    ;
        /// <summary>
        /// Clear the word buffer.
        /// </summary>
        public readonly CommandID ClearBuffer        ;
	}
}
