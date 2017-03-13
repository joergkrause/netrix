using System;
using System.Collections.Generic;
using System.Text;
using GuruComponents.Netrix.ComInterop;

namespace GuruComponents.Netrix.NetRix.WebEditing.HighLighting
{
    public class LineInfo
    {

        private Interop.ILineInfo li;

        internal LineInfo(Interop.ILineInfo li)
        {
            this.li = li;
        }

        internal Interop.ILineInfo Native { get { return li; } }

        #region ILineInfo Members

        public int LineDirection
        {
            get { return li.lineDirection; }
        }

        public int TextHeight
        {
            get { return li.textHeight; }
        }

        public int X
        {
            get { return li.x; }
        }

        public int TextDescent
        {
            get { return li.textDescent; }
        }

        public int BaseLine
        {
            get { return li.baseLine; }
        }

        #endregion
    }
}
