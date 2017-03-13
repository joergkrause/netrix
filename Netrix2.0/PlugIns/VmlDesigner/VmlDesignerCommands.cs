using System;
using System.ComponentModel.Design;

namespace GuruComponents.Netrix.VmlDesigner
{

    internal enum VmlDesignerCommand
    {
        Activate    = 100,
        Deactivate  = 101,
        EnsureStyle = 102,
        InsertMode = 103,
        DesignMode = 104

    }

	/// <summary>
	/// VmlDesignerCommands contains a list of commands this extension provides.
	/// </summary>
	public class VmlDesignerCommands
	{

        private Guid vmlDesignerGuid;

        /// <summary>
        /// Constructor, creates and fills list.
        /// </summary>
		public VmlDesignerCommands()
		{
            vmlDesignerGuid = this.GetType().GUID;
            Activate        = new CommandID(vmlDesignerGuid, (int)VmlDesignerCommand.Activate);
            Deactivate      = new CommandID(vmlDesignerGuid, (int)VmlDesignerCommand.Deactivate);
            EnsureStyle     = new CommandID(vmlDesignerGuid, (int)VmlDesignerCommand.EnsureStyle);
            InsertMode      = new CommandID(vmlDesignerGuid, (int)VmlDesignerCommand.InsertMode);
            DesignMode      = new CommandID(vmlDesignerGuid, (int)VmlDesignerCommand.DesignMode);
		}

        public Guid CommandGroup
        {
            get
            {
                return this.vmlDesignerGuid;
            }
        }

        /// <summary>
        /// Activates the extension at runtime.
        /// </summary>
        public readonly CommandID Activate;
        /// <summary>
        /// Deactivates the extension at runtime.
        /// </summary>
        public readonly CommandID Deactivate;
        /// <summary>
        /// Checks that the VML Style instruction exists and creates one, if not.
        /// </summary>
        public readonly CommandID EnsureStyle;
        /// <summary>
        /// During Insert change event handling.
        /// </summary>
        public readonly CommandID InsertMode;
        /// <summary>
        /// Default mode during interactive design.
        /// </summary>
        public readonly CommandID DesignMode;
	}
}

