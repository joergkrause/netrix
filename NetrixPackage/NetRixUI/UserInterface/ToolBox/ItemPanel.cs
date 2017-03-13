using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace GuruComponents.Netrix.UserInterface.ToolBox
{
        class ItemPanel : Panel
        {
            public ItemPanel()
                : base()
            {
                SetStyle(ControlStyles.ResizeRedraw, true);
                SetStyle(ControlStyles.UserPaint, true);
                SetStyle(ControlStyles.AllPaintingInWmPaint, true);
                SetStyle(ControlStyles.DoubleBuffer, true);
                SetStyle(ControlStyles.Selectable, true);
            }
        }
}
