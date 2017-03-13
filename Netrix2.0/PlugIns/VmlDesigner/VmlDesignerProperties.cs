using System;
using System.ComponentModel;

namespace GuruComponents.Netrix.VmlDesigner
{
	/// <summary>
	/// Properties for VmlDesigner Extender Provider
	/// </summary>
    [Serializable()]
	public class VmlDesignerProperties 
	{

        private bool active;
        private bool snapEnabled;
        private int snapgrid;
		private bool elementEvents;

        private bool canRotate;

        /// <summary>
        /// Indicates whether the ability to rotate objects is globally available.
        /// </summary>
        [Browsable(true), DefaultValue(true), Category("Behavior"), Description("Indicates whether the ability to rotate objects is globally available.")]
        public bool CanRotate
        {
            get { return canRotate; }
            set { canRotate = value; }
        }

		public VmlDesignerProperties()
		{
            active = false;
            snapEnabled = true;
            snapgrid = 16;
			elementEvents = false;
            canRotate = true;
        }
        
        /// <summary>
        /// Turns the advanced editing features on or off.
        /// </summary>
        [Category("Behavior"), DefaultValue(false), Description("Turns the advanced editing features on or off.")]
        public bool Active
        {
            get
            { 
                return active;
            }
            set
            {
                active = value;
            }
        }

        /// <summary>
        /// If active the elements can fire private events to which the host can hook on. 
        /// </summary>
        /// <remarks>
        /// In case the events aren't used it's recommonded to set this property to <c>False</c> to increase performance.
        /// </remarks>
		[Category("Behavior"), DefaultValue(false), Description("If active the elements can fire private events to which the host can hook on.")]
		public bool ElementEvents
		{
			get
			{ 
				return elementEvents;
			}
			set
			{
				elementEvents = value;
			}
		}

        /// <summary>
        /// Enable or disable snap to grid.
        /// </summary>
        /// <remarks>
        /// <seealso cref="SnapGrid"/>
        /// </remarks>
        [Category("Snap"), DefaultValue(true), Description("Enable or disable snap to grid.")]
        public bool SnapEnabled
        {
            get
            {
                return snapEnabled;
            }

            set
            {
                snapEnabled = value;
            }
        }

        /// <summary>
        /// Gets or sets the distance in pixels between the grid.
        /// </summary>
        /// <remarks>
        /// The grid is used to snap the helpline to fixed points.
        /// </remarks>
        [Category("Snap"), DefaultValue(16), Description("Gets or sets the distance in pixels between the grid.")]
        public int SnapGrid
        {
            get
            {
                return snapgrid;
            }

            set
            {
                snapgrid = value;
            }
        }

        public override string ToString()
        {
            int i = 0;
            if (Active) i++;
            if (SnapGrid != 16) i++;
            if (SnapEnabled != true) i++;
			if (ElementEvents) i++;
            if (CanRotate != true) i++;
            return String.Format("{1} Propert{0} modified", (i == 0) ? "y" : "ies", i);
        }
    }
}
