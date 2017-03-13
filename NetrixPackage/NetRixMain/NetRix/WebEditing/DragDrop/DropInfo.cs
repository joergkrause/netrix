using System;
using System.Collections.Generic;
using System.Text;
using GuruComponents.Netrix.WebEditing.DragDrop;

namespace GuruComponents.Netrix.WebEditing.DragDrop
{
    /// <summary>
    /// Exposes information about drag 'n drop procedure for better control.
    /// </summary>
    public class DropInfo
    {
        private IDataObjectConverter _converter;

        /// <summary>
        /// 
        /// </summary>
        public IDataObjectConverter Converter
        {
            get { return _converter; }
            set { _converter = value; }
        }
        private DataObjectConverterInfo _converterInfo;

        /// <summary>
        /// 
        /// </summary>
        public DataObjectConverterInfo ConverterInfo
        {
            get { return _converterInfo; }
            set { _converterInfo = value; }
        }

    }
}
