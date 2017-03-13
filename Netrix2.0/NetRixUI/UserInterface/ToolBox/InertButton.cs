using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace GuruComponents.Netrix.UserInterface.ToolBox
{
   
        class InertButton : Button
        {
            public InertButton()
                : base()
            {
                SetStyle(ControlStyles.StandardDoubleClick, false);
                SetStyle(ControlStyles.Selectable, false);
            }
        }
}
