using System;

namespace GuruComponents.Netrix.WebEditing.Styles
{
	/// <summary>
	/// Defines which part of a positioned object is visible.</summary>
	/// <remarks>
	/// Top, right, bottom, and left specify length values, any of which can be replaced by auto, leaving that side not clipped. The value of top specifies that everything above this value on the Y axis (with 0 at the top) is clipped. The value of right specifies that everything above this value on the X axis (with 0 at the left) is clipped. The value of bottom specifies that everything below this value on the Y axis (with 0 at the top) is clipped. The value of left specifies that everything to the left of this value on the X axis (with 0 at the left) is clipped.
	/// </remarks>
	public class ClipStyle : IClipStyle
	{

        private System.Web.UI.WebControls.Unit left;
        private System.Web.UI.WebControls.Unit right;
        private System.Web.UI.WebControls.Unit top;
        private System.Web.UI.WebControls.Unit bottom;

        /// <summary>
        /// Ctor
        /// </summary>
        public ClipStyle()
        {
            left = System.Web.UI.WebControls.Unit.Empty;
            right = System.Web.UI.WebControls.Unit.Empty;
            top = System.Web.UI.WebControls.Unit.Empty;
            bottom = System.Web.UI.WebControls.Unit.Empty;
            clipType = ClipFormat.Auto;
        }

        /// <summary>
        /// The left value of the clip area.
        /// </summary>
        public System.Web.UI.WebControls.Unit Left
        {
            get
            {
                return left;
            }
            set
            {
                left = value;
            }
        }
        /// <summary>
        /// The right value of the clip area.
        /// </summary>
        public System.Web.UI.WebControls.Unit Right
        {
            get
            {
                return right;
            }
            set
            {
                right = value;
            }
        }
        /// <summary>
        /// The top value of the clip area.
        /// </summary>
        public System.Web.UI.WebControls.Unit Top
        {
            get
            {
                return top;
            }
            set
            {
                top = value;
            }
        }
        /// <summary>
        /// The bottom value of the clip area.
        /// </summary>
        public System.Web.UI.WebControls.Unit Bottom
        {
            get
            {
                return bottom;
            }
            set
            {
                bottom = value;
            }
        }

        private ClipFormat clipType;

        /// <summary>
        /// The clip type.
        /// </summary>
        public ClipFormat ClipType
        {
            get
            {
                return clipType;
            }
            set
            {
                clipType = value;
            }
        }

        /// <summary>
        /// Gets or sets the string value representation of the clip area.
        /// </summary>
        /// <remarks>
        /// The clip format is "auto" or "rect(top right bottom left)" where the values are given in
        /// typical style sheet measures (px, pt, ...).
        /// </remarks>
        public string ClipString
        {
            get
            {
                switch (clipType)
                {
                    case ClipFormat.Auto:
                        return "auto";
                    case ClipFormat.Rectangle:
                        return String.Format("rect({0} {1} {2} {3})", Top, Right, Bottom, Left);
                }
                return String.Empty;
            }
            set
            {
                if (value.StartsWith("auto"))
                {
                    clipType = ClipFormat.Auto;
                    return;
                }
                if (value.StartsWith("rect"))
                {
                    string[] arrVals = value.Split(' ');
                    if (arrVals.Length == 4)
                    {
                        clipType = ClipFormat.Rectangle;
                        Top = System.Web.UI.WebControls.Unit.Parse(arrVals[0]);
                        Right = System.Web.UI.WebControls.Unit.Parse(arrVals[1]);
                        Bottom = System.Web.UI.WebControls.Unit.Parse(arrVals[2]);
                        Left = System.Web.UI.WebControls.Unit.Parse(arrVals[3]);
                    } 
                    else 
                    {
                        clipType = ClipFormat.Auto;
                    }
                }
            }
        }

	}
}
