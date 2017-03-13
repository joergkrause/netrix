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
	/// Adds properties to elements to implement auto connector feature.
	/// </summary>
    public interface IConnector
	{


        /// <summary>
        /// Defines the connect range in pixel around the spot. Default is 8.
        /// </summary>
        /// <remarks>
        /// If the range is 10 pixel, and the current spot is 100;100, the range will
        /// be from 90;90 to 110;110, that means, 10 pixel in each direction around the spot.
        /// </remarks>
        int ConnectRange
        {
            get;
            set;
        }

        /// <summary>
        /// Makes the element connecting to next spot within a defined range.
        /// </summary>
        bool AutoConnect
        {
            get;
            set;
        }

    }
}
