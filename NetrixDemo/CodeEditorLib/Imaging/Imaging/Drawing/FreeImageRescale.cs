using System;
using System.Collections.Generic;
using System.Text;
using Comzept.Library.Drawing.Internal;

namespace Comzept.Library.Drawing
{
    public class FreeImageRescale : FreeImageGeometryTransformation
    {
        private int _height = 0;
        private int _width = 0;
        private FreeImage.FreeImageFilter _Filter;

        public FreeImage.FreeImageFilter Filter
        {
            get { return _Filter; }
            set { _Filter = value; }
        }

        public int Height
        {
            get { return _height; }
            set { _height = value; }
        }

        public int Width
        {
            get { return _width; }
            set { _width = value; }
        }

        internal override void Run(FreeImage image)
        {
            int hwnd = FreeImageApi.Rescale(image.GetFreeImageHwnd(), this.Width
            , this.Height, _Filter);

            image.DisposeAndSetHandle(hwnd);
        }

        public FreeImageRescale(int width, int height, 
            FreeImage.FreeImageFilter filter)
        {
            _width = width;
            _height = height;
        }

        public FreeImageRescale(int width, int height):
            this(width,height, FreeImage.FreeImageFilter.BICUBIC)
        {

        }
    }
}
