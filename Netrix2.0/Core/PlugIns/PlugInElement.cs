using System.Drawing;
using GuruComponents.Netrix.WebEditing.Behaviors;
using GuruComponents.Netrix.WebEditing.Elements;

namespace GuruComponents.Netrix.PlugIns
{
    /// <summary>
    /// This interface defines a public way to insert new element definition within the NetRix
    /// environment from within a Plug-In.
    /// </summary>
    public interface IElementEngine
    {
        /// <summary>
        /// The command, used to identify a drag operation.
        /// </summary>
        int DragDropCommand
        {
            get;
            set;
        }
        /// <summary>
        /// The icon shown as mouse pointer during valid drag operation.
        /// </summary>
        Icon DragIcon
        {
            get;
            set;
        }

        /// <summary>
        /// The HTML produced on drop event.
        /// </summary>
        string InnerHtml
        {
            get;
            set;
        }

        /// <summary>
        /// The element which is build from this drag 'n drop operation
        /// </summary>
        IElement Element
        {
            get;
            set;
        }

        /// <summary>
        /// The namespace in which this element exists. This results in saving the
        /// content as NS:TagName
        /// </summary>
        string Namespace
        {
            get;
            set;
        }

        /// <summary>
        /// This content will be used to display the element at design time and it will never
        /// be written at save. The save process will use <seealso cref="InnerHtml"/> in any case.
        /// </summary>
        string DesignTimeHtml
        {
            get;
            set;
        }

        /// <summary>
        /// The behavior attached to this element.
        /// </summary>
        IBaseBehavior ElementBehavior
        {
            get;
            set;
        }

//        /// <summary>
//        /// Informs the formatter module how to format the element.
//        /// </summary>
//        GuruComponents.Netrix.HtmlFormatting.Elements.TagInfo FormattingInfo
//        {
//            get;
//            set;
//        }

    }
}
