using System;
using System.Collections.Generic;
using System.Text;
using Comzept.Library.Drawing.Internal;

namespace Comzept.Library.Drawing
{
    public class FreeImageConstrast : FreeImagePixelTransformation
    {
        private double _constrast = 0;

        private bool _completed;

        public bool Completed
        {
            get { return _completed; }
        }

        public double Constrast
        {
            get { return _constrast; }
            set { _constrast = value; }
        }

        public FreeImageConstrast(double constrast)
        {
            _constrast = constrast;
        }

        internal override void Run(FreeImage image)
        {
            _completed = FreeImageApi.AdjustContrast(image.GetFreeImageHwnd(), _constrast);
        }
    }
}
