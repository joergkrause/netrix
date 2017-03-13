using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace GuruComponents.Netrix.UserInterface.ColorPicker
{
    /// <summary>
    /// A helper class for very fast bitmap operations.
    /// </summary>
    public unsafe class BitmapUnsafe
	{
		Bitmap bitmap;

			// three elements used for MakeGreyUnsafe
		int width;
		BitmapData bitmapData = null;
		Byte* pBase = null;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="bitmap">Bitmap the content is stored in.</param>
		public BitmapUnsafe(Bitmap bitmap)
		{
			this.bitmap = bitmap;
		}

        /// <summary>
        /// Save to bitmap.
        /// </summary>
        /// <param name="filename"></param>
		public void Save(string filename)
		{
			bitmap.Save(filename, ImageFormat.Jpeg);
		}

        /// <summary>
        /// Dispose bitmap.
        /// </summary>
		public void Dispose()
		{
			bitmap.Dispose();
		}

        /// <summary>
        /// Access to related bitmap.
        /// </summary>
		public Bitmap Bitmap
		{
			get
			{
				return(bitmap);
			}
		}

        /// <summary>
        /// Size of bitmap.
        /// </summary>
		public Point PixelSize
		{
			get
			{
				GraphicsUnit unit = GraphicsUnit.Pixel;
				RectangleF bounds = bitmap.GetBounds(ref unit);

				return new Point((int) bounds.Width, (int) bounds.Height);
			}
		}

        /// <summary>
        /// Convert all pixels to gray.
        /// </summary>
		public void MakeGrey()
		{
			Point size = PixelSize;

			LockBitmap();

			for (int y = 0; y < size.Y; y++)
			{
				PixelData* pPixel = PixelAt(0, y);
				for (int x = 0; x < size.X; x++)
				{
					byte value = (byte) ((pPixel->red + pPixel->green + pPixel->blue) / 3);
					pPixel->red =  value;
					pPixel->green = value;
					pPixel->blue = value;
					pPixel++;
				}
			}
			UnlockBitmap();
		}

        /// <summary>
        /// Make the red palette.
        /// </summary>
        /// <param name="redValue">The fixed value.</param>
   		public void MakeRedPalette(byte redValue)
		{
			LockBitmap();

			for (byte y = 0; y < 255; y++)
			{
				PixelData* pPixel = PixelAt(0, y);
				for (byte x = 0; x < 255; x++)
				{
					pPixel->red =  redValue;
					pPixel->green = x;
					pPixel->blue = y;
					pPixel++;
				}
			}
			UnlockBitmap();			
		}

        /// <summary>
        /// Make the blue palette.
        /// </summary>
        /// <param name="blueValue">The fixed value.</param>
        public void MakeBluePalette(byte blueValue)
		{
            LockBitmap();

			for (byte y = 0; y < 255; y++)
			{
				PixelData* pPixel = PixelAt(0, y);
				for (byte x = 0; x < 255; x++)
				{
					pPixel->red =  x;
					pPixel->green = y;
					pPixel->blue = blueValue;
					pPixel++;
				}
			}
			UnlockBitmap();	
		}

        /// <summary>
        /// Make the green palette.
        /// </summary>
        /// <param name="greenValue">The fixed value.</param>
        public void MakeGreenPalette(byte greenValue)
		{
            LockBitmap();

			for (byte y = 0; y < 255; y++)
			{
				PixelData* pPixel = PixelAt(0, y);
				for (byte x = 0; x < 255; x++)
				{
					pPixel->red =  x;
					pPixel->green = greenValue;
					pPixel->blue = y;
					pPixel++;
				}
			}
			UnlockBitmap();	
		}

        /// <summary>
        /// Lock the bitmap.
        /// </summary>
		public void LockBitmap()
		{
			GraphicsUnit unit = GraphicsUnit.Pixel;
			RectangleF boundsF = bitmap.GetBounds(ref unit);
			Rectangle bounds = new Rectangle((int) boundsF.X,
				(int) boundsF.Y,
				(int) boundsF.Width,
				(int) boundsF.Height);

			// Figure out the number of bytes in a row
			// This is rounded up to be a multiple of 4
			// bytes, since a scan line in an image must always be a multiple of 4 bytes
			// in length. 
			width = (int) boundsF.Width * sizeof(PixelData);
			if (width % 4 != 0)
			{
				width = 4 * (width / 4 + 1);
			}

			bitmapData = 
				bitmap.LockBits(bounds, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

			pBase = (Byte*) bitmapData.Scan0.ToPointer();
		}

        /// <summary>
        /// Retrieve a pixel at specified location.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
		public PixelData* PixelAt(int x, int y)
		{
			return (PixelData*) (pBase + y * width + x * sizeof(PixelData));
		}

        /// <summary>
        /// Unlock the bitmap.
        /// </summary>
		public void UnlockBitmap()
		{
			bitmap.UnlockBits(bitmapData);
			bitmapData = null;
			pBase = null;
		}
	}
}
