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
    public class PolylineElement : LineElement
	{

        private VgPointsCollection points;

        /// <summary>
        /// Gets or sets the start point of the element.
        /// </summary>
        /// <remarks>
        /// <seealso cref="To"/>
        /// </remarks>
        public VgPointsCollection Points
        {
            get
            {
                if (points == null)
                {
                    points = new VgPointsCollection(base.GetBaseElement(), InternalPoints);
                    points.ClearCollection += new CollectionClearHandler(points_ClearCollection);
                    points.Insert += new CollectionInsertHandler(points_Insert);
                }
                return points;
            }
        }

        [Browsable(false)]
        public override VgVector2D To
        {
            get
            {
                return new VgVector2D(base.GetAttribute("to") as IVgVector2D);
            }
            set
            {
                base.SetAttribute("to", value.NativeVector);
            }
        }

        [Browsable(false)]
        public override VgVector2D From
        {
            get
            {
                return new VgVector2D(base.GetAttribute("from") as IVgVector2D);
            }
            set
            {
                base.SetAttribute("from", value.NativeVector);
            }
        }

        private IVgPoints InternalPoints
        {
            get
            {
                return (IVgPoints) base.GetAttribute("points");
            }
            set
            {
                base.SetAttribute("points", value);
            }
        }

        private void points_ClearCollection()
        {
            Points.RemoveAll();
        }

        private void points_Insert(int index, object @value)
        {
            if (@value is VgVector2D)
            {
                //
            } 
            else 
            {
                throw new ArgumentException("Expect parameter of type VgVector2D", "value");
            }
        }

        public PolylineElement(IHtmlEditor editor) : base("v:polyline", editor)
		{
		}

        internal PolylineElement(Interop.IHTMLElement peer, IHtmlEditor editor) : base(peer, editor)
        {
        }

    }
}
