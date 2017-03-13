using System;

namespace GuruComponents.Netrix.VmlDesigner.DataTypes
{
	/// <summary>
	/// Enumeration used for values that can be one of three states.
	/// </summary>
    public enum TriState
    {
        /// <summary>
        /// True
        /// </summary>
        True   = Comzept.Genesis.NetRix.VgxDraw.VgTriState.vgTriStateTrue,
        /// <summary>
        /// False
        /// </summary>
        False  = Comzept.Genesis.NetRix.VgxDraw.VgTriState.vgTriStateFalse,
        /// <summary>
        /// Inherited mixed
        /// </summary>
        Mixed  = Comzept.Genesis.NetRix.VgxDraw.VgTriState.vgTriStateMixed,
        /// <summary>
        /// Toggle from previous state
        /// </summary>
        Toggle = Comzept.Genesis.NetRix.VgxDraw.VgTriState.vgTriStateToggle,
    }
}
