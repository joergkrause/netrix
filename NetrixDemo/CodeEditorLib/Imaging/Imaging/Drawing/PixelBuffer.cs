using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace Comzept.Library.Drawing
{
    public class PixelBuffer : IDisposable
    {
        public struct PixelInfo
        {
            public ColorHLS Color;
            public int X, Y;
            public PixelInfo(int x, int y, ColorHLS color)
            {
                X = x;
                Y = y;
                Color = color;
            }
            public PixelInfo(int x, int y)
                : this(x, y, new ColorHLS(0,0, 0, 0))
            {
            }
        }

        private int[] _nullXArray;
        private ColorHLS[] _nullColorArray;

        public static PixelBuffer FromBitmap(Bitmap bmp)
        {
            PixelBuffer pixBuffer = new PixelBuffer(bmp.Width, bmp.Height);
            pixBuffer.CopyBitmapDataToBuffer(bmp);

            return pixBuffer;
        }

        private byte[] _buffer;
        private Bitmap _bmp;
        private int _byteCount;
        private int _width, _height;
        private int _bOffset;
        private int _bWidth;

        public PixelBuffer(int width, int height)
        {
            Initialize(width, height);
        }
        public PixelBuffer(int width, int height, ColorHLS backColor)
            : this(width, height)
        {
            ClearBuffer(backColor);
        }
        ~PixelBuffer()
        {
            this.Dispose();
        }

        public void Dispose()
        {
            _bmp.Dispose();
        }
        public unsafe void ClearBuffer(ColorHLS color)
        {
            fixed (byte* buffer = _buffer)
            {
                ClearBuffer(buffer, color);
            }
        }
        public unsafe void GetBufferData(out byte[] buffer)
        {
            buffer = new byte[_buffer.Length];
            fixed (byte* destBuffer = buffer)
            {
                fixed (byte* srcBuffer = _buffer)
                {
                    CopyBuffer(destBuffer, srcBuffer, _buffer.Length);
                }
            }
        }
        public unsafe void SetBufferData(byte[] buffer)
        {
            buffer = new byte[_buffer.Length];
            fixed (byte* destBuffer = _buffer)
            {
                fixed (byte* srcBuffer = buffer)
                {
                    CopyBuffer(destBuffer, srcBuffer, _buffer.Length);
                }
            }
        }
        public unsafe void GetPixel(int x, int y, out ColorHLS color)
        {
            fixed (byte* buff = _buffer)
            {
                byte* p = buff;
                for (int i = 0; i < y; ++i)
                {
                    p += _bWidth;
                    p += _bOffset;
                }
                p += (x * 3);

                byte b = (byte)(*p);
                p++;
                byte g = (byte)(*p);
                p++;
                byte r = (byte)(*p);

                color = new ColorHLS(255, r, g, b);
            }

        }
        public void GetPixel(int x, int y, out PixelInfo pixelInfo)
        {
            ColorHLS color;
            GetPixel(x, y, out color);
            pixelInfo = new PixelInfo(x, y, color);
        }
        public void GetPixel(ref PixelInfo pixelInfo)
        {
            ColorHLS color;
            GetPixel(pixelInfo.X, pixelInfo.Y, out color);
            pixelInfo.Color = color;
        }
        public void SetPixel(int x, int y, ColorHLS color, out PixelInfo oldPixelInfo)
        {
            ColorHLS oldColor;
            SetPixel(x, y, color, out oldColor);
            oldPixelInfo = new PixelInfo(x, y, oldColor);
        }
        public void SetPixel(PixelInfo pixelInfo, out PixelInfo oldPixelInfo)
        {
            ColorHLS oldColor;
            SetPixel(pixelInfo.X, pixelInfo.Y, pixelInfo.Color, out oldColor);
            oldPixelInfo = new PixelInfo(pixelInfo.X, pixelInfo.Y, oldColor);
        }
        public unsafe void SetPixel(int x, int y, ColorHLS color, out ColorHLS oldColor)
        {
            fixed (byte* buff = _buffer)
            {
                byte* p = buff;
                for (int i = 0; i < y; ++i)
                {
                    p += _bWidth;
                    p += _bOffset;
                }
                p += (x * 3);

                oldColor = new ColorHLS(255, 0, 0, 0);

                oldColor.Blue = (byte)p[0];
                p[0] = color.Blue;
                p++;

                oldColor.Green = (byte)p[0];
                p[0] = color.Green;
                p++;

                oldColor.Red = (byte)p[0];
                p[0] = color.Red;
            }
        }
        public void SetPixel(PixelInfo pixelInfo, out ColorHLS oldColor)
        {
            SetPixel(pixelInfo.X, pixelInfo.Y, pixelInfo.Color, out oldColor);
        }
        public void SetPixel(int x, int y, ColorHLS color)
        {
            ColorHLS oldColor;
            SetPixel(x, y, color, out oldColor);
        }
        public void SetPixel(PixelInfo pixelInfo)
        {
            ColorHLS oldColor;
            SetPixel(pixelInfo.X, pixelInfo.Y, pixelInfo.Color, out oldColor);
        }
        public void Render(Graphics gfx, Rectangle rectangle)
        {
            Render(gfx, (float)rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
        }
        public void Render(Graphics gfx, RectangleF rectangle)
        {
            Render(gfx, rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
        }
        public void Render(Graphics gfx, Point location)
        {
            Render(gfx, (float)location.X, location.Y, _width, _height);
        }
        public void Render(Graphics gfx, PointF location)
        {
            Render(gfx, location.X, location.Y, _width, _height);
        }
        public void Render(Graphics gfx, int x, int y)
        {
            Render(gfx, (float)x, y, _width, _height);
        }
        public void Render(Graphics gfx, int x, int y, int width, int height)
        {
            Render(gfx, (float)x, y, width, height);
        }
        public void Render(Graphics gfx, float x, float y)
        {
            Render(gfx, x, y, _width, _height);
        }
        public void Render(Graphics gfx, float x, float y, float width, float height)
        {
            CopyBufferToBitmapData();
            gfx.DrawImage(_bmp, x, y, width, height);
        }
        public Bitmap ToBitmap()
        {
            CopyBufferToBitmapData();
            return (Bitmap)_bmp.Clone();
        }

        public delegate ColorHLS PixelFillingDelegate(int x, int y,int count,int total, bool setPixel);

        public void DrawLine(int x1, int y1, int x2, int y2, ColorHLS color)
        {
            ColorHLS pixCol = color.Clone();
            PixelFillingDelegate pfd = delegate(int x, int y, int count, int total, bool setPixel)
            {

                if(setPixel)
                    SetPixel(x, y, pixCol);

                return pixCol;
            };

            DrawLine(x1, y1, x2, y2, pfd);
        }
        public void DrawLine(int x1, int y1, int x2, int y2, ColorHLS color1,ColorHLS color2)
        {
            bool created = false;
            ColorHLS color = new ColorHLS(color1);
            float[,] steps = new float[0, 0];
            ColorHLS[] colors=new ColorHLS[0];

            PixelFillingDelegate pfd = delegate(int x, int y, int count, int total, bool setPixel)
            {
                if (!created)
                {
                    steps = ColorUtils.GetGradientColorSteps(color1, color2,total);
                    created = true;
                }

                color.SetRGB(
                    (byte)steps[count, 0],
                    (byte)steps[count, 1],
                    (byte)steps[count, 2]
                    );

                if (setPixel)
                    SetPixel(x, y, color);

                return color;
            };

            DrawLine(x1, y1, x2, y2, pfd);
        }
        public void DrawLine(int x1, int y1, int x2, int y2, PixelFillingDelegate pixFillingDelegate)
        {
            int deltax, deltay;

            deltax = (x2 - x1);
            deltay = (y2 - y1);

            if (deltax == 0)
            {
                if (deltay == 0)
                {
                    return;
                }
                DrawVerticalLine(x1, y1, deltay, pixFillingDelegate);
                return;
            }
            else if (deltay == 0)
            {
                DrawHorizontalLine(x1, y1, deltax, pixFillingDelegate);
                return;
            }

            /* Calcoliamo il deltax ed il deltay della linea, ovvero il numero di pixel presenti a livello
                orizzontale e verticale. Utilizziamo la funzione abs() poichè a noi interessa il loro 
                valore assoluto. */
            deltax = Math.Abs(x2 - x1);
            deltay = Math.Abs(y2 - y1);

            int i, numpixels,
                d, dinc1, dinc2,
                x, xinc1, xinc2,
                y, yinc1, yinc2;

            /* Adesso controlliamo se la linea è più "orizzontale" o "verticale", ed inizializziamo
               in maniera appropriate le variabili di comodo. */
            if (deltax >= deltay)
            {
                /* La linea risulta maggiormente schiacciata sull' ascissa */
                numpixels = deltax + 1;

                /* La nostra variabile decisionale, basata sulla x della linea */
                d = (2 * deltay) - deltax;

                /* Settiamo gli incrementi */
                dinc1 = deltay * 2;
                dinc2 = (deltay - deltax) * 2;
                xinc1 = xinc2 = 1;
                yinc1 = 0;
                yinc2 = 1;
            }
            else
            {
                /* La linea risulta maggiormente schiacciata sull' ordinata*/
                numpixels = deltay + 1;

                /* La nostra variabile decisionale, basata sulla y della linea */
                d = (2 * deltax) - deltay;

                /* Settiamo gli incrementi */
                dinc1 = deltax * 2;
                dinc2 = (deltax - deltay) * 2;
                xinc1 = 0;
                xinc2 = 1;
                yinc1 = yinc2 = 1;
            }

            /* Eseguiamo un controllo per settare il corretto 
                andamento della linea */
            if (x1 > x2)
            {
                xinc1 = -xinc1;
                xinc2 = -xinc2;
            }
            if (y1 > y2)
            {
                yinc1 = -yinc1;
                yinc2 = -yinc2;
            }

            /* Settiamo le coordinate iniziali  */
            x = x1;
            y = y1;

            /* Stampiamo la linea */
            for (i = 1; i < numpixels; i++)
            {
                //SetPixel(x, y, color);
                pixFillingDelegate(x, y, i-1,numpixels-1,true);

                /* Scegliamo l' incremento del "passo" a seconda dellla
                    variabile decisionale */
                if (d < 0)
                {
                    d = d + dinc1;
                    x = x + xinc1;
                    y = y + yinc1;
                }
                else
                {
                    d = d + dinc2;
                    x = x + xinc2;
                    y = y + yinc2;
                }
            }
        }

        private void Initialize(int width, int height)
        {
            _width = width;
            _height = height;

            _bmp = new Bitmap(_width, _height, PixelFormat.Format24bppRgb);

            BitmapData bmpData = _bmp.LockBits(new Rectangle(0, 0, _width, _height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

            _bOffset = bmpData.Stride - (_width * 3);
            _bWidth = _width * 3;
            _byteCount = bmpData.Stride * _height;

            _buffer = new byte[_byteCount];

            _bmp.UnlockBits(bmpData);

            _nullXArray = new int[_height];
            _nullColorArray = new ColorHLS[_height];
            for (int i = 0; i < _height; i++)
            {
                _nullXArray[i] = -1;
                _nullColorArray[i] = new ColorHLS(0,0,0,0);
            }
        }
        private unsafe void CopyBuffer(byte* dest, byte* src, int lenght)
        {
            for (int i = 0; i < lenght / 4; i++)
            {
                *((int*)dest) = *((int*)src);
                dest += 4;
                src += 4;
            }
            for (int i = 0; i < lenght % 4; i++)
            {
                *dest = *src;
                dest++;
                src++;
            }
        }
        private unsafe void ClearBuffer(byte* buffer, ColorHLS color)
        {
            for (int y = 0; y < _height; ++y)
            {
                for (int x = 0; x < _bWidth; x += 3)
                {
                    buffer[0] = color.Blue;
                    ++buffer;
                    buffer[0] = color.Green;
                    ++buffer;
                    buffer[0] = color.Red;
                    ++buffer;
                }
                buffer += _bOffset;
            }
        }
        private unsafe void CopyBitmapDataToBuffer(Bitmap bmp)
        {
            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, _width, _height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            byte* src = (byte*)bmpData.Scan0;

            fixed (byte* dest = _buffer)
            {
                CopyBuffer(dest, src, _buffer.Length);
            }

            bmp.UnlockBits(bmpData);
        }
        private unsafe void CopyBufferToBitmapData()
        {
            BitmapData bmpData = _bmp.LockBits(new Rectangle(0, 0, _width, _height), ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);
            byte* dest = (byte*)(void*)bmpData.Scan0;

            fixed (byte* src = _buffer)
            {
                CopyBuffer(dest, src, _buffer.Length);
            }

            _bmp.UnlockBits(bmpData);
        }
        private unsafe void DrawHorizontalLine(int x, int y, int width, PixelFillingDelegate pixFillingDelegate)
        {
            if (width < 0)
            {
                // width è negativa
                x += width + 1;
            }

            width = Math.Abs(width);

            fixed (byte* buff = _buffer)
            {
                byte* p = buff;
                for (int i = 0; i < y; ++i)
                {
                    p += _bWidth;
                    p += _bOffset;
                }
                p += (x * 3);

                for (int i = 0; i < width; i++)
                {
                    ColorHLS color = pixFillingDelegate(x+i, y , i, width, false);
                    p[0] = color.Blue;
                    p++;
                    p[0] = color.Green;
                    p++;
                    p[0] = color.Red;
                    p++;
                }
            }
        }
        private unsafe void DrawVerticalLine(int x, int y, int height, PixelFillingDelegate pixFillingDelegate)
        {
            if (height < 0)
            {
                // height è negativa
                y += height + 1;
            }

            height = Math.Abs(height);

            fixed (byte* buff = _buffer)
            {
                byte* p = buff;
                for (int i = 0; i < y; ++i)
                {
                    p += _bWidth;
                    p += _bOffset;
                }
                p += (x * 3);

                for (int i = 0; i < height; i++)
                {
                    ColorHLS color = pixFillingDelegate(x, y + i, i, height, false);
                    p[0] = color.Blue;
                    p++;
                    p[0] = color.Green;
                    p++;
                    p[0] = color.Red;
                    p++;

                    p += (_bWidth - 3);
                    p += _bOffset;
                }
            }
        }

    }
}
