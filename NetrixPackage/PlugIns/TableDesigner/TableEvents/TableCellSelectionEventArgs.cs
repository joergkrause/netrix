using System;
using System.Collections;
using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.WebEditing.Elements;

namespace GuruComponents.Netrix.TableDesigner
{
	/// <summary>
	/// Event argument used for the TableCellSelection event.
	/// </summary>
	public class TableCellSelectionEventArgs : EventArgs
	{

        private Interop.IHTMLTable _t;
        private HighLightCellList _cellStack;
        private IElement[] temp = null;
        private IHtmlEditor htmlEditor;

		internal TableCellSelectionEventArgs(Interop.IHTMLTable t, HighLightCellList cellStack, IHtmlEditor htmlEditor)
		{
            _t = t;
            _cellStack = cellStack;
            this.htmlEditor = htmlEditor;
		}

        /// <summary>
        /// This property gives access to the table which the cells collection comes from.
        /// </summary>
        public IElement Table
        {
            get
            {
                return htmlEditor.GenericElementFactory.CreateElement((Interop.IHTMLElement) this._t) as IElement;
            }
        }

        /// <summary>
        /// This property gives access to the currently highlighted cells.
        /// </summary>
        /// <remarks>
        /// It takes the Interop cell
        /// objects and creates or retrieves the native <see cref="TableCellElement"/> objects for
        /// usage in the host application.
        /// <para>
        /// <b>Callers:</b> The collection becomes invalid if the table structure has changed after getting
        /// these values. Invalid cells are zombies, they have no conjunction with the underlying 
        /// document anymore. Manipulating them can result in unpredictable effects.
        /// </para>
        /// </remarks>
        public IElement[] TableCellCollection
        {
            get
            {
                if (temp == null)
                {
                    temp = new IElement[this._cellStack.Count];                    
                    for (int i = 0; i < this._cellStack.Count; i++)
                    {   
                        temp.SetValue(htmlEditor.GenericElementFactory.CreateElement((Interop.IHTMLElement) ((BaseHighLightCell)this._cellStack[i]).TableCell), i);
                    }
                }
                return temp;
            }
        }

        /// <summary>
        /// Number of cells in the collection.
        /// </summary>
        public int CellCount
        {
            get
            {
                return this._cellStack.Count;
            }
        }

	}
}
