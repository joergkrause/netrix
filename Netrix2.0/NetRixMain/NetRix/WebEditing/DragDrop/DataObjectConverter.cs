using System;
using System.Windows.Forms;
using GuruComponents.Netrix.ComInterop;

namespace GuruComponents.Netrix.WebEditing.DragDrop
{
    /// <summary>
    /// This class converts data for dragged objects.
    /// </summary>
    internal class DataObjectConverter : GuruComponents.Netrix.WebEditing.DragDrop.IDataObjectConverter
    {

        /// <summary>
        /// Checks out if we can convert the dropsource into HTML.
        /// </summary>
        /// <param name="dataObject"></param>
        /// <returns></returns>
        public virtual DataObjectConverterInfo CanConvertToHtml(DataObject dataObject)
        {
            foreach (string format in dataObject.GetFormats())
            {
                if (format.Equals("FileDrop") || format.Equals("System.Drawing.Design.ToolboxItem"))
                {
                    return DataObjectConverterInfo.Externally;
                }
                if (format.Equals("GuruComponents.Netrix.WebEditing.Elements.IElement"))
                {
                    GuruComponents.Netrix.WebEditing.Elements.IElement el = dataObject.GetData(format) as GuruComponents.Netrix.WebEditing.Elements.IElement;
                    if (el != null)
                    {
                        return DataObjectConverterInfo.Native;
                    }
                }
                if (format.Equals("GuruComponents.Netrix.WebEditing.DragDrop.DragDropCommands"))
                {
                    return DataObjectConverterInfo.CanConvert;
                }
            }
            object data = dataObject.GetData(DataFormats.Serializable);
            // can be null if internal/external dragdrop operation, so let MSHTML handle it
            if (data == null)
            {
                try
                {
                    data = dataObject.GetData(DataFormats.Text);
                    if (data != null)
                    {
                        return DataObjectConverterInfo.Text;
                    }
                    data = dataObject.GetData(DataFormats.Html);
                    if (data != null)
                    {
                        return DataObjectConverterInfo.Text;
                    }
                }
                catch
                {
                }
            }
            if (data is DragDropCommands)
            {
                return DataObjectConverterInfo.CanConvert;
            }
            try
            {
                DragDropCommands command = (DragDropCommands)Enum.Parse(typeof(DragDropCommands), data.ToString(), false);
                return DataObjectConverterInfo.CanConvert;
            }
            catch
            {
            }
            return DataObjectConverterInfo.Unhandled;
        }

        /// <summary>
        /// Determines a dropped data source and build HTML strings that can be inserted into the document.
        /// </summary>
        /// <param name="originalDataObject"></param>
        /// <param name="IsInAbsolutePositionMode"></param>
        /// <param name="absolutePosition"></param>
        /// <param name="newDataObject"></param>
        /// <returns>Returns <c>false</c> if the convertion fails.</returns>
        public virtual bool ConvertToHtml(DataObject originalDataObject, bool IsInAbsolutePositionMode, System.Drawing.Point absolutePosition, ref DataObject newDataObject)
        {
            object data = originalDataObject.GetData(DataFormats.Serializable);
            string styleAbsPosition = String.Empty;
            if (IsInAbsolutePositionMode)
            {
                // prepare position 
                styleAbsPosition = String.Format(@"style=""position:absolute;left:{0}px;top:{1}px""", absolutePosition.X, absolutePosition.Y);
            }
            try
            {
                newDataObject = new DataObject();
                DragDropCommands dragData = (DragDropCommands)data;
                switch (dragData)
                {
                    case DragDropCommands.Anchor:
                        newDataObject.SetData(DataFormats.Html, @"<a " + styleAbsPosition + @" href=""new.html"">New Hyperlink</a>");
                        break;
                    case DragDropCommands.Break:
                        newDataObject.SetData(DataFormats.Html, @"<br/>");
                        break;
                    case DragDropCommands.Button:
                        newDataObject.SetData(DataFormats.Html, @"<input type=""button"" value=""Button""/>");
                        break;
                    case DragDropCommands.Checkbox:
                        newDataObject.SetData(DataFormats.Html, @"<input type=""checkbox"" />");
                        break;
                    case DragDropCommands.Div:
                        newDataObject.SetData(DataFormats.Html, @"<div " + styleAbsPosition + @">New Division</div>");
                        break;
                    case DragDropCommands.DropDown:
                        newDataObject.SetData(DataFormats.Html, @"<select size=""1""></select>");
                        break;
                    case DragDropCommands.FileButton:
                        newDataObject.SetData(DataFormats.Html, @"<input type=""file"" />");
                        break;
                    case DragDropCommands.Form:
                        newDataObject.SetData(DataFormats.Html, @"<form method=""post""></form>");
                        break;
                    case DragDropCommands.HorizontalRule:
                        newDataObject.SetData(DataFormats.Html, @"<hr/>");
                        break;
                    case DragDropCommands.ImageButton:
                        newDataObject.SetData(DataFormats.Html, @"<input type=""image""/>");
                        break;
                    case DragDropCommands.ListBox:
                        newDataObject.SetData(DataFormats.Html, @"<select size=""3""></select>");
                        break;
                    case DragDropCommands.Paragraph:
                        newDataObject.SetData(DataFormats.Html, @"<p>New Paragraph</p>");
                        break;
                    case DragDropCommands.Password:
                        newDataObject.SetData(DataFormats.Html, @"<input type=""password""/>");
                        break;
                    case DragDropCommands.RadioButton:
                        newDataObject.SetData(DataFormats.Html, @"<input type=""radio"">");
                        break;
                    case DragDropCommands.ResetButton:
                        newDataObject.SetData(DataFormats.Html, @"<input type=""reset"" value=""Reset"">");
                        break;
                    case DragDropCommands.HiddenField:
                        newDataObject.SetData(DataFormats.Html, @"<input type=""hidden"" >");
                        break;
                    case DragDropCommands.SubmitButton:
                        newDataObject.SetData(DataFormats.Html, @"<input type=""submit"" value=""Submit"">");
                        break;
                    case DragDropCommands.TextArea:
                        newDataObject.SetData(DataFormats.Html, @"<textarea rows=""3"" cols=""40""></textarea>");
                        break;
                    case DragDropCommands.Textbox:
                        newDataObject.SetData(DataFormats.Html, @"<input type=""text"">");
                        break;
                    case DragDropCommands.Image:
                        newDataObject.SetData(DataFormats.Html, @"<img/>");
                        break;
                    case DragDropCommands.Table:
                        newDataObject.SetData(DataFormats.Html, @"<table><tr><td></td><td></td><td></td></tr><tr><td></td><td></td><td></td></tr><tr><td></td><td></td><td></td></tr></table>");
                        break;
                    case DragDropCommands.Span:
                        newDataObject.SetData(DataFormats.Html, @"<span>New Span</span>");
                        break;
                    default:
                        // TODO: Access plug-in here
                        return false;
                }
            }
            catch
            {
                return false;
            }
            return true;
        }
    }

}
