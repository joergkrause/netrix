using System;
using System.Collections.Generic;
using System.Text;
using Comzept.Library.Drawing.Internal;

namespace Comzept.Library.Drawing
{
    public class FreeImageBrightness : FreeImagePixelTransformation
    {
        private double _brightness = 0;
        private bool _completed;

        public bool Completed
        {
            get { return _completed; }
        }

        public double Brightness
        {
            get { return _brightness; }
            set { _brightness = value; }
        }

        public FreeImageBrightness(double brightness)
        {
            //if (brightness > 255)
            //{
            //    brightness = 255;
            //}

            //if (brightness < 0)
            //    brightness = 0;

            _brightness = brightness;
        }

        internal override void Run(FreeImage image)
        {
            _completed = FreeImageApi.AdjustBrightness(image.GetFreeImageHwnd(), _brightness);
        }
    }
}
