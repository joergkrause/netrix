using System;

namespace GuruComponents.Netrix.WebEditing.Styles
{

    /// <summary>
    /// Interface for clip style attribute.
    /// </summary>
    public interface IClipStyle
    {

            /// <summary>
            /// The left value of the clip area.
            /// </summary>
            System.Web.UI.WebControls.Unit Left
            {
                get; set;
            }
            /// <summary>
            /// The right value of the clip area.
            /// </summary>
            System.Web.UI.WebControls.Unit Right
            {
                get; set;
            }
            /// <summary>
            /// The top value of the clip area.
            /// </summary>
            System.Web.UI.WebControls.Unit Top
            {
                get; set;
            }
            /// <summary>
            /// The bottom value of the clip area.
            /// </summary>
            System.Web.UI.WebControls.Unit Bottom
            {
                get; set;
            }

            /// <summary>
            /// The clip type.
            /// </summary>
            ClipFormat ClipType
            {
                get;
                set;
            }

            /// <summary>
            /// Gets or sets the string value representation of the clip area.
            /// </summary>
            /// <remarks>
            /// The clip format is "auto" or "rect(top right bottom left)" where the values are given in
            /// typical style sheet measures (px, pt, ...).
            /// </remarks>
            string ClipString
            {
                get;
                set;
            }

    }


}
