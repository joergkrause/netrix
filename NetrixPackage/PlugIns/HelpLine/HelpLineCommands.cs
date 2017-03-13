using System;
using System.ComponentModel.Design;

namespace GuruComponents.Netrix.HelpLine
{

    internal enum HelplineCommand : int
    {
        Activate    = 100,
        Deactivate  = 101,
    }

	/// <summary>
	/// HelplineCommands this extender provides.
	/// </summary>
    public class HelplineCommands
    {

        private Guid hlDesignerGuid;

        /// <summary>
        /// Ctor for helpline commands class.
        /// </summary>
	    public HelplineCommands()
	    {
            hlDesignerGuid = Guid.NewGuid(); // assure that any instance has its own GUID!
            Activate   = new CommandID(hlDesignerGuid, (int)HelplineCommand.Activate);
            Deactivate = new CommandID(hlDesignerGuid, (int)HelplineCommand.Deactivate);
	    }

        /// <summary>
        /// The identifier for the command group built to support the helpline's commands.
        /// </summary>
        public Guid CommandGroup
        {
            get
            {
                return this.hlDesignerGuid;
            }
        }

        /// <summary>
        /// Command used to activate the helpline.
        /// </summary>
        public readonly CommandID Activate;
        /// <summary>
        /// Command used to deactivate the helpline.
        /// </summary>
        public readonly CommandID Deactivate;

    }
}

