using System;
using System.ComponentModel.Design;

namespace GuruComponents.Netrix.Designer
{
	/// <summary>
	/// Zusammenfassung für CommandWrapper.
	/// </summary>
	public class CommandWrapper : MenuCommand
	{

        private bool restoreSelection;
        private bool needMultiSelection;
        private IHtmlEditor targetEditor;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="id"></param>
        public CommandWrapper(EventHandler handler, CommandID id) : this(handler, id, false, false)
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="id"></param>
        /// <param name="restoreSelection"></param>
        /// <param name="needMultiSelection"></param>
		public CommandWrapper(EventHandler handler, CommandID id, bool restoreSelection, bool needMultiSelection) : base(handler, id)
		{
            this.restoreSelection = restoreSelection;
            this.needMultiSelection = needMultiSelection;
		}

        /// <summary>
        /// Editor reference
        /// </summary>
        public IHtmlEditor TargetEditor
        {
            get
            {
                return this.targetEditor;
            }
            set
            {
                this.targetEditor = value;
            }
        }
        /// <summary>
        /// Groups Guid.
        /// </summary>
        public Guid Guid
        {
            get
            {
                return base.CommandID.Guid;
            }
        }
        /// <summary>
        /// Comamnd ID
        /// </summary>
        public int ID
        {
            get
            {
                return base.CommandID.ID;
            }
        }
        /// <summary>
        /// Name
        /// </summary>
        public string Name
        {
            get
            {
                return base.CommandID.ToString();
            }
        }
        /// <summary>
        /// Whether Should keep seletion.
        /// </summary>
        public bool RestoreSelection
        {
            get
            {
                return restoreSelection;
            }
            set
            {
                restoreSelection = value;
            }
        }
        /// <summary>
        /// Need multiple selections
        /// </summary>
        public bool NeedMultiSelection
        {
            get
            {
                return needMultiSelection;
            }
            set
            {
                needMultiSelection = value;
            }
        }


	}
}
