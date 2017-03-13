using System;
using System.Windows.Forms;
namespace GuruComponents.Netrix.WebEditing.DragDrop
{
    public interface IDataObjectConverter
    {
        bool ConvertToHtml(System.Windows.Forms.DataObject originalDataObject, bool IsInAbsolutePositionMode, System.Drawing.Point absolutePosition, ref System.Windows.Forms.DataObject newDataObject);

        DataObjectConverterInfo CanConvertToHtml(DataObject dataObject);
    }
}
