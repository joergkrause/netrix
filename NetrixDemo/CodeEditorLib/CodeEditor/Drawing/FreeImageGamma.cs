using System;
using System.Collections.Generic;
using System.Text;
using GuruComponents.CodeEditor.Library.Drawing.Internal;

namespace GuruComponents.CodeEditor.Library.Drawing
{
    public class FreeImageGamma : FreeImagePixelTransformation
    {
        private double _gamma;

        public double Gamma
        {
            get { return _gamma; }
            set { _gamma = value; }
        }

        private bool _completed;

        public bool Completed
        {
            get { return _completed; }
        }

        public FreeImageGamma(double gamma)
        {
            _gamma = gamma;
        }

        internal override void Run(FreeImage image)
        {
            _completed = FreeImageApi.AdjustGamma(image.GetFreeImageHwnd(), _gamma);
        }
    }
}
