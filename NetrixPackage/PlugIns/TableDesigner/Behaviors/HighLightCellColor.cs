using System;
using System.Drawing;
using System.Collections.Specialized;
using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.WebEditing.Elements;


namespace GuruComponents.Netrix.TableDesigner
{
    /// <summary>
    /// Purpose of this class is provide highlight cell with specific brush and pen (for border).
    /// Making the border the same color as the background the border is not visible. 
    /// </summary>
    internal class HighLightCellColor : BaseHighLightCell
    {
        private Color _HighLightColor;
        private Color _HighLightBorderColor;
        private int _cookie;
        private static HighLightCellBehavior _behavior = null;

        /// <summary>
        /// Constructor, sets the base values for highlighting.
        /// </summary>
        /// <param name="HighLightColor">Background color</param>
        /// <param name="HighLightBorderColor">Border color</param>
        /// <param name="Cell">Cell to which the highlight should added</param>
        /// <param name="htmlEditor"></param>
        internal HighLightCellColor(Color HighLightColor, Color HighLightBorderColor, Interop.IHTMLTableCell Cell, IHtmlEditor htmlEditor) : base(Cell, htmlEditor)
        {
            _HighLightColor = HighLightColor;
            _HighLightBorderColor = HighLightBorderColor;            
            _cookie = -1;
            if (_behavior == null)
            {
                _behavior = new HighLightCellBehavior(htmlEditor);
            }
        }

        internal override void MakeHighLight()
        {
            _behavior.CellBrush = new System.Drawing.SolidBrush(_HighLightColor);
            _behavior.BorderPen = new Pen(_HighLightBorderColor, 2.0F);
            object behavior = _behavior;
            _cookie = ((Interop.IHTMLElement2)base.GenericCell).AddBehavior("HighLightCell", ref behavior);
        }

        internal override void ReleaseHighLight()
        {
            if (_cookie != -1)
            {
                ((Interop.IHTMLElement2) base.GenericCell).RemoveBehavior(_cookie);
            }
        }
    }
}
