using System;
using System.Drawing;

using GuruComponents.Netrix;
using GuruComponents.Netrix.Events;
using GuruComponents.Netrix.ComInterop;

namespace GuruComponents.Netrix.WebEditing.Behaviors
{
	/// <summary>
	/// A helper class used internally that let elements snap to grid positions in absolute position mode.
	/// </summary>
	/// <remarks>
	/// This class intercepts the drag event to snap elements to grid borders. Gridsize defaults to 8. That means, if you
	/// move an element in absolute position mode around the element "jumps" from one grid point to the next one. This 
	/// function helps aligning elements properly. 
	/// <para>
	/// This class is usually not used from custom code. To activate use the Grid's properties, such as <see cref="GuruComponents.Netrix.WebEditing.Behaviors.HtmlGrid.SnapEnabled">SnapEnabled</see>
	/// </para>
	/// </remarks>
	/// <seealso cref="GuruComponents.Netrix.WebEditing.Behaviors.HtmlGrid">HtmlGrid</seealso>
	public sealed class SnapElement : Interop.IHTMLEditHost
	{

		private IHtmlEditor _editor;

		internal SnapElement(IHtmlEditor editor)
		{
			_editor = editor;
		}

		private bool SnapEnabled
		{
			get
			{
				return _editor.Grid.SnapEnabled;
			}
		}

		private int SnapZone
		{
			get
			{
				return _editor.Grid.SnapZone;
			}
		}

		private bool SnapOnResize
		{
			get
			{
				return _editor.Grid.SnapOnResize;
			}
		}

		private Size GridSize
		{
			get
			{
				return _editor.Grid.GridSize;
			}
		}

		private int GridSizeMidVert
		{
			get
			{
				return GridSize.Height/2;
			}
		}

		private int GridSizeMidHorz
		{
			get
			{
				return GridSize.Width/2;
			}
		}

		public event BeforeSnapRectEventHandler BeforeSnapRect;
		public event AfterSnapRectEventHandler AfterSnapRect;

		# region IHTMLEditHost Implementation

		void Interop.IHTMLEditHost.SnapRect(Interop.IHTMLElement element, Interop.RECT rect, int pVar)
		{
			bool cancel = false;
			if (BeforeSnapRect != null)
			{				
				BeforeSnapRectEventArgs b = new BeforeSnapRectEventArgs(_editor, element, rect, (Interop.ELEMENT_CORNER)pVar);
				BeforeSnapRect(_editor, b);
				// rewrite changed values
				rect.left   = b.Rectangle.Left;
				rect.right  = b.Rectangle.Right;
				rect.top    = b.Rectangle.Top;
				rect.bottom = b.Rectangle.Bottom;
				cancel = b.Cancel;
			}
			if (!cancel && SnapEnabled)
			{
				SnapRectToGrid(ref rect, pVar);
			}
			if (AfterSnapRect != null)
			{
				AfterSnapRect(_editor, new AfterSnapRectEventArgs(_editor, element, rect, (Interop.ELEMENT_CORNER)pVar));
			}
		}

		private void SnapRectToGrid(ref Interop.RECT rect, int pVar)
		{
			switch ((Interop.ELEMENT_CORNER)pVar)
			{
				case Interop.ELEMENT_CORNER.ELEMENT_CORNER_NONE:
					int i = rect.right - rect.left;
					int j = rect.bottom - rect.top;
					rect.top = (rect.top + GridSizeMidVert) / GridSize.Height * GridSize.Height;
					rect.left = (rect.left + GridSizeMidHorz) / GridSize.Width * GridSize.Width;
					rect.bottom = rect.top + j;
					rect.right = rect.left + i;
					return;

				case Interop.ELEMENT_CORNER.ELEMENT_CORNER_TOP:
					rect.top    = ((rect.top + GridSizeMidVert) / GridSize.Height * GridSize.Height) ;
					return;

				case Interop.ELEMENT_CORNER.ELEMENT_CORNER_LEFT:
					rect.left   = ((rect.left + GridSizeMidHorz) / GridSize.Width * GridSize.Width) ;
					return;

				case Interop.ELEMENT_CORNER.ELEMENT_CORNER_BOTTOM:
					rect.bottom = ((rect.bottom + GridSizeMidVert) / GridSize.Height * GridSize.Height) ;
					return;

				case Interop.ELEMENT_CORNER.ELEMENT_CORNER_RIGHT:
					rect.right  = ((rect.right + GridSizeMidHorz) / GridSize.Width * GridSize.Width) ;
					return;

				case Interop.ELEMENT_CORNER.ELEMENT_CORNER_TOPLEFT:
					rect.top    = ((rect.top + GridSizeMidVert) / GridSize.Height * GridSize.Height) ;
					rect.left   = ((rect.left + GridSizeMidHorz) / GridSize.Width * GridSize.Width) ;
					return;

				case Interop.ELEMENT_CORNER.ELEMENT_CORNER_TOPRIGHT:
					rect.top    = (rect.top + GridSizeMidVert) / GridSize.Height * GridSize.Height ;
					rect.right  = ((rect.right + GridSizeMidHorz) / GridSize.Width * GridSize.Width) ;
					return;

				case Interop.ELEMENT_CORNER.ELEMENT_CORNER_BOTTOMLEFT:
					rect.bottom = ((rect.bottom + GridSizeMidVert) / GridSize.Height * GridSize.Height) ;
					rect.left   = ((rect.left + GridSizeMidHorz) / GridSize.Width * GridSize.Width) ;
					return;

				case Interop.ELEMENT_CORNER.ELEMENT_CORNER_BOTTOMRIGHT:
					rect.bottom = ((rect.bottom + GridSizeMidVert) / GridSize.Height * GridSize.Height) ;
					rect.right  = ((rect.right + GridSizeMidHorz) / GridSize.Width * GridSize.Width) ;
					return;

				default:
					return;
			}
		}     

		# endregion


	 }
}