using System;  
using GuruComponents.Netrix.ComInterop;

using GuruComponents.Netrix.VmlDesigner.DataTypes;
using Comzept.Genesis.NetRix.VgxDraw;
using System.Drawing.Design;
using System.ComponentModel;
using GuruComponents.Netrix;

namespace GuruComponents.Netrix.VmlDesigner.Elements
{
	/// <summary>
	/// This element is used to draw a cubic bézier curve.
	/// </summary>
    /// <remarks>Predefined curve shape.
    /// </remarks>
    /// <example>
    /// The following is the minimum code needed to produce an curve.
    /// <code>
    /// &lt;v:curve 
    /// from="10pt,10pt" to="100pt,10pt"
    /// control1="40pt,30pt" control2="70pt,30pt"&gt;
    /// &lt;/v:curve&gt;
    /// </code>
    /// </example>
	[ToolboxItem(false)]
    public class CurveElement : LineElement
	{

        /// <summary>
        /// Gets or sets the first control point of a bezier curve.
        /// </summary>
        /// <remarks>
        /// <seealso cref="To"/> <seealso cref="From"/>
        /// </remarks>
        public VgVector2D Control1
        {
            get
            {
                return new VgVector2D((IVgVector2D) GetAttribute("control1"));
            }
            set
            {
                SetAttribute("control1", value.NativeVector);
            }
        }

        /// <summary>
        /// Gets or sets the second control point of a bezier curve.
        /// </summary>
        /// <remarks>
        /// <seealso cref="To"/> <seealso cref="From"/>
        /// </remarks>
        public VgVector2D Control2
        {
            get
            {
                return new VgVector2D((IVgVector2D) GetAttribute("control2"));
            }
            set
            {
                SetAttribute("control2", value.NativeVector);
            }
        }


		public CurveElement(IHtmlEditor editor) : base("v:curve", editor)
		{
		}

        internal CurveElement(Interop.IHTMLElement peer, IHtmlEditor editor) : base(peer, editor)
        {
        }

	}
}
