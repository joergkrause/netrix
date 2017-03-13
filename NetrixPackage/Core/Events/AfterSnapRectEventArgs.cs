using System;
using System.Drawing;
using System.Web.UI;
using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.WebEditing.Behaviors;

namespace GuruComponents.Netrix.Events
{

	/// <summary>
	/// Event arguments for event AfterSnapRectEvent.
	/// </summary>
    public class AfterSnapRectEventArgs : EventArgs
    {

        Control element;
		Rectangle rectangle;
		SnapZone zone;

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
		public AfterSnapRectEventArgs(IHtmlEditor editor, Interop.IHTMLElement el, Interop.RECT rect, Interop.ELEMENT_CORNER pVar)
		{
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
        /// TODO: Add comment.
        /// </summary>
        public Rectangle Rectangle
		{
			get
			{
				return this.rectangle;
			}
		}

        /// <summary>
        /// TODO: Add comment.
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