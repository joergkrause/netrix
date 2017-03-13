using System.ComponentModel;
using System.Drawing;
using System.Web.UI;
using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.WebEditing.Behaviors;

namespace GuruComponents.Netrix.Events
{

	/// <summary>
	/// Event arguments for event BeforeSnapRectEvent.
	/// </summary>
    public class BeforeSnapRectEventArgs : CancelEventArgs
    {
        Control element;
		Rectangle rectangle;
		SnapZone zone;
        IHtmlEditor editor;

        /// <summary>
        /// Constructor for event arguments.
        /// </summary>
        /// <remarks>
        /// Used internally to support NetRix infrastructure. There is no need to call this constructor directly.
        /// </remarks>
        /// <param name="editor"></param>
        /// <param name="el"></param>
        /// <param name="pVar"></param>
        /// <param name="rect"></param>
		public BeforeSnapRectEventArgs(IHtmlEditor editor, Interop.IHTMLElement el, Interop.RECT rect, Interop.ELEMENT_CORNER pVar)
		{
            this.editor = editor;
			element = editor.GenericElementFactory.CreateElement(el);
			rectangle = new Rectangle(rect.left, rect.top, rect.right - rect.left, rect.bottom - rect.top);

			zone = (SnapZone) (int) pVar;
		}

        /// <summary>
        /// The element which the event belongs to.
        /// </summary>
        public Control Element
        {
            get
            {
                return this.element;
            }
        }			

        /// <summary>
        /// Returns the current scroll position.
        /// </summary>
        /// <remarks>
        /// You can use this value to determine the position of the element against other absolute positioned objects, that might
        /// not handle the scroll position on its own.
        /// </remarks>
        public Point ScrollPos
        {
            get
            {
                Interop.IHTMLElement2 body2 = (editor.GetBodyElement().GetBaseElement()) as Interop.IHTMLElement2;
                Point scrollPos = new Point(body2.GetScrollLeft(), body2.GetScrollTop());
                return scrollPos;
            }
        }

        /// <summary>
        /// Area of snaping element.
        /// </summary>
		public Rectangle Rectangle
		{
			get
			{
				return this.rectangle;
			}
			set
			{
				this.rectangle = value;
			}
		}			

        /// <summary>
        /// Zone which has to snap.
        /// </summary>
		public SnapZone SnapZone
		{
			get
			{
				return this.zone;
			}
		}			

    }

}