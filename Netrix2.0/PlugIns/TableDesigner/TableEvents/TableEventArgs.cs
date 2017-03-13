using System;
using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.WebEditing.Elements;

namespace GuruComponents.Netrix.TableDesigner
{
	/// <summary>
	/// Sends the current table after OnFocus or OnBlur events to the event handler.
	/// </summary>
	public class TableEventArgs : System.EventArgs
	{

        private IElement _table;

		internal TableEventArgs(Interop.IHTMLElement table, IHtmlEditor htmlEditor)
		{
            if (table != null) 
            {
                _table = (IElement) htmlEditor.GenericElementFactory.CreateElement(table);
            } 
            else 
            {
                _table = null;
            }
		}

        /// <summary>
        /// Gets the table element which causes the event.
        /// </summary>
        public IElement TableElement
        {
            get
            {
                return _table;
            }
        }

	}
}
