using System;
using System.Drawing;
using System.Collections;
using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.WebEditing.Elements;

namespace GuruComponents.Netrix.TableDesigner
{
	/// <summary>
	/// Purpose of this class is create instance of highlighted cell for the table. It stores
	/// the background color and the color of highlight border.
	/// </summary>
	internal class HighLightCellFactory
	{
		private Color _HighLightColor = Color.Pink;
        private Color _HighLightBorderColor = Color.Black;
        IHtmlEditor htmlEditor;

		internal HighLightCellFactory(Color HighLightColor, Color HighLightBorderColor, IHtmlEditor htmlEditor)
		{
			_HighLightColor = HighLightColor;
            _HighLightBorderColor = HighLightBorderColor;
            this.htmlEditor = htmlEditor;
		}

		internal BaseHighLightCell CreateHighLightCell(Interop.IHTMLTableCell Cell)
		{
			return new HighLightCellColor(_HighLightColor, _HighLightBorderColor, Cell, htmlEditor);
		}
	}

}
