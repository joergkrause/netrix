using System;  
using System.ComponentModel;  


using Comzept.Genesis.NetRix.VgxDraw;
using GuruComponents.Netrix.VmlDesigner;
using GuruComponents.Netrix.VmlDesigner.DataTypes;
using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix;


namespace GuruComponents.Netrix.VmlDesigner.Elements
{
	/// <summary>
	/// The polyline element is used to define shapes made up of connected line segments.
	/// </summary>
	[ToolboxItem(false)]
    public class ConectorElement : PolylineElement, IConnector
	{

        private bool autoConnect = true;
        private int connectRange = 8;

        /// <summary>
        /// Defines the connect range in pixel around the spot. Default is 8.
        /// </summary>
        /// <remarks>
        /// If the range is 10 pixel, and the current spot is 100;100, the range will
        /// be from 90;90 to 110;110, that means, 10 pixel in each direction around the spot.
        /// </remarks>
        [DefaultValue(8), Description("Defines the connect range in pixel around the spot. Default is 8.")]
        public int ConnectRange
        {
            get { return connectRange; }
            set { connectRange = value; }
        }

        /// <summary>
        /// Makes the element connecting to next spot within a defined range.
        /// </summary>
        [DefaultValue(true), Description("Makes the element connecting to next spot within a defined range.")]
        public bool AutoConnect
        {
            get { return autoConnect; }
            set { autoConnect = value; }
        }


        public ConectorElement(IHtmlEditor editor) : base(editor)
		{
		}

        internal ConectorElement(Interop.IHTMLElement peer, IHtmlEditor editor)
            : base(peer, editor)
        {
        }

    }
}
