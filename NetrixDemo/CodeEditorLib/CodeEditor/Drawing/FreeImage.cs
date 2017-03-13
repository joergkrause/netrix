using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.Text;
using GuruComponents.CodeEditor.Library.Drawing.Internal;
using System.Runtime.InteropServices;
using System.Threading;

namespace GuruComponents.CodeEditor.Library.Drawing
{

	/// <summary>
    /// Main class provides access to freeimage library.
    /// </summary>
    /// <remarks>
	/// For use this class you need to have on your system the native library of 
	/// FreeImage you can grab it from http://freeimage.sourceforge.net/
	/// 
	/// This class is a Managed Wrapper for FreeImage library with the praticity of
	/// Classes.
	/// </remarks>
	public class FreeImage:IDisposable,ICloneable
	{
        ~FreeImage()
        {
            Dispose();
        }

        static FreeImage()
        {
            FreeImageApi.Initialise(false);
            AppDomain.CurrentDomain.DomainUnload += new EventHandler(CurrentDomain_DomainUnload);
        }

        static void CurrentDomain_DomainUnload(object sender, EventArgs e)
        {
            FreeImageApi.DeInitialise();
        }


        public event EventHandler TransformationCompleted;

		#region API Declarations

		[DllImport("gdi32.dll")]
		private static extern int SetDIBitsToDevice(IntPtr hdc, int x, int y, int dx, int dy, int SrcX, int SrcY, int Scan, int NumScans, IntPtr Bits, IntPtr BitsInfo, int wUsage);

		#endregion

		#region Enumerations

		public enum FreeImageFormat
		{
			Unknown = -1,
			Bmp = 0,
			Ico = 1,
			Jpg = 2,
			Jng = 3,
			Koala = 4,
			Lbm = 5,
			Iff = Lbm,
			Mng = 6,
			Pbm = 7,
			PbmRaw = 8,
			Pcd = 9,
			Pcx = 10,
			Pgm = 11,
			PgmRaw = 12,
			Png = 13,
			Ppm = 14,
			PpmRaw = 15,
			Ras = 16,
			Tga = 17,
			Tif = 18,
			Wbmp = 19,
			Psd = 20,
			Cut = 21,
			Xbm = 22,
			Xpm = 23,
			Dds = 24,
			Gif = 25
		}
		public enum FreeImageQuantize
		{
			WUQuant = 0,
			NNQuant = 1
		}
		public enum FreeImageDither
		{
			FS = 0,
			BAYER4x4 = 1,
			BAYER8x8 = 2,
			CLUSTER6x6 = 3,
			CLUSTER8x8 = 4,
			CLUSTER16x16 = 5
		}
		public enum FreeImageFilter
		{
			BOX = 0,
			BICUBIC = 1,
			BILINEAR = 2,
			BSPLINE = 3,
			CATMULLROM = 4,
			LANCZOS3 = 5
		}
		public enum FreeImageColorChannel
		{
			RGB = 0,
			RED = 1,
			GREEN = 2,
			BLUE = 3,
			ALPHA = 4,
			BLACK = 5
		}
		public enum FreeImageType
		{
			UNKNOWN = 0,
			BITMAP = 1,
			UINT16 = 2,
			INT16 = 3,
			UINT32 = 4,
			INT32 = 5,
			FLOAT = 6,
			DOUBLE = 7,
			COMPLEX = 8
		}
		public enum FreeImageColorType
		{
			MINISBLACK = 0,
			MINISWHITE = 1,
			PALETTE = 2,
			RGB = 3,
			RGBALPHA = 4,
			CMYK = 5
		}

		#endregion

		#region Static Members

		public static string GetVersion()
		{
			return FreeImageApi.GetVersion();
		}
		public static string GetCopyright()
		{
			return FreeImageApi.GetCopyrightMessage();
		}

		#endregion

		#region Private Members

		private int m_Handle = 0;
		private IntPtr m_MemPtr = IntPtr.Zero;

		private FreeImageFormat ImageFormatToFIF(ImageFormat imageFormat)
		{
			string fmt = imageFormat.ToString().ToLower();
			if (fmt == "bmp")
			{
				return FreeImageFormat.Bmp;
			}
			if (fmt == "jpg")
			{
				return FreeImageFormat.Jpg;
			}
			return FreeImageFormat.Unknown;
		}

		#endregion

		#region Constructors

		internal FreeImage(int handle)
			:this(handle,IntPtr.Zero)
		{
           // FreeImageApi.Initialise(false);
		}
		internal FreeImage(int handle,IntPtr memPtr)
		{
           // FreeImageApi.Initialise(true);

			m_Handle = handle;
			m_MemPtr = memPtr;
		}

		public FreeImage(string filename)
		{
            

			FreeImageFormat fif = FreeImageApi.GetFIFFromFilename(filename);
			if(fif == FreeImageFormat.Unknown) throw new Exception("Unknown file format");

			m_Handle = FreeImageApi.Load(fif, filename, 0);
			m_MemPtr=IntPtr.Zero;
		}

		public FreeImage(Bitmap bitmap, ImageFormat imageFormat)
		{
           
			FreeImageFormat fif = ImageFormatToFIF(imageFormat);
			if (fif == FreeImageFormat.Unknown)
			{
				throw new Exception("Image format \"" + imageFormat.ToString() + "\" is not supported");
			}

			MemoryStream ms = new MemoryStream();
			bitmap.Save(ms, imageFormat);
			ms.Flush();

			byte[] buffer = new byte[((int)(ms.Length - 1)) + 1];
			ms.Position = 0;
			ms.Read(buffer, 0, (int)ms.Length);
			ms.Close();

			IntPtr dataPtr = Marshal.AllocHGlobal(buffer.Length);
			Marshal.Copy(buffer, 0, dataPtr, buffer.Length);

			m_MemPtr = FreeImageApi.OpenMemory(dataPtr, buffer.Length);
			m_Handle = FreeImageApi.LoadFromMemory(fif, m_MemPtr, 0);

		}

		#endregion

		#region Public Properties

		public int Bpp
		{
			get
			{
				return (int)FreeImageApi.GetBPP(m_Handle);
			}
		}
		public int Pitch
		{
			get
			{
				return (int)FreeImageApi.GetPitch(m_Handle);
			}
		}
		public int DotsPerMeterX
		{
			get
			{
				return (int)FreeImageApi.GetDotsPerMeterX(m_Handle);
			}
		}
		public int DotsPerMeterY
		{
			get
			{
				return (int)FreeImageApi.GetDotsPerMeterY(m_Handle);
			}
		}
		public int Width
		{
			get
			{
				return (int)FreeImageApi.GetWidth(m_Handle);
			}
		}
		public int Height
		{
			get
			{
				return (int)FreeImageApi.GetHeight(m_Handle);
			}
		}
		public SizeF Size
		{
			get
			{
				return new SizeF(this.Width, this.Height);
			}
		}
		public int UsedColors
		{
			get
			{
				return (int)FreeImageApi.GetColorsUsed(m_Handle);
			}
		}
		public int TransparencyCount
		{
			get
			{
				return (int)FreeImageApi.GetTransparencyCount(m_Handle);
			}
		}
		public FreeImageColorType ColorType
		{
			get
			{
				return (FreeImageColorType)FreeImageApi.GetColorType(m_Handle);
			}
		}

        //public bool AdjustContrast(double percentage)
        //{
        //     return FreeImageApi.AdjustContrast(m_Handle, percentage);
        //}

        //public bool AdjustBrightness(double percentage)
        //{
        //    return FreeImageApi.AdjustBrightness(m_Handle, percentage);
        //}

        //public bool AdjustGamma(double percentage)
        //{
        //    return FreeImageApi.AdjustGamma(m_Handle, percentage);
        //}

		public FreeImageType ImageType
		{
			get
			{
				return FreeImageApi.GetImageType(m_Handle);
			}
		}

		#endregion

		#region Public Methods

		public Bitmap GetBitmap()
		{
			Bitmap bmp = new Bitmap(this.Width, this.Height);
			Graphics gfx = Graphics.FromImage(bmp);
			IntPtr ptrHDC = gfx.GetHdc();

			this.PaintToDevice(ptrHDC, 0, 0, bmp.Width, bmp.Height, 0, 0, 0, bmp.Height, 0);

			gfx.ReleaseHdc(ptrHDC);

			return bmp;
		}

        /// <summary>
        /// Get the native FreeImage Handle
        /// </summary>
        /// <returns></returns>
        public int GetFreeImageHwnd()
        {
        
            return m_Handle;
        }
		
		public void PaintToDevice(IntPtr destDC, int x, int y, int width, int height, int srcX, int srcY, int scan, int mumScans, int wUsage)
		{
			try
			{
				IntPtr ptrBits = FreeImageApi.GetBits(m_Handle);
				IntPtr ptrInfo = FreeImageApi.GetInfo(m_Handle);
				SetDIBitsToDevice(destDC, x, y, width, height, srcX, srcY, scan, mumScans, ptrBits, ptrInfo, wUsage);
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}
		
		public void PaintToBitmap(Bitmap bitmap, int x, int y, int width, int height, int srcX, int srcY)
		{
			Graphics gfx = Graphics.FromImage(bitmap);

			IntPtr ptrHDC = gfx.GetHdc();

			this.PaintToDevice(ptrHDC, x, y, width, height, 0, 0, 0, this.Height, 0);

			gfx.ReleaseHdc(ptrHDC);
		}
		
		public void PaintToGraphics(Graphics graphics, int x, int y, int width, int height, int srcX, int srcY)
		{

			IntPtr ptrHDC = graphics.GetHdc();

			this.PaintToDevice(ptrHDC, x, y, width, height, 0, 0, 0, this.Height, 0);

			graphics.ReleaseHdc(ptrHDC);
		}

        public FreeImage Rotate(double angle)
		{
			int i = FreeImageApi.RotateClassic(m_Handle, angle);

            return new FreeImage(i);
		}

        public FreeImage RotateExtended(double angle, double xShift, double yShift, double xOrigin, double yOrigin, bool mask)
		{
			int i = FreeImageApi.RotateEx(m_Handle, angle, xShift, yShift, xOrigin, yOrigin, mask);

            return new FreeImage(i);
		}

		public void RotateExtended(double angle, double xShift, double yShift, double xOrigin, double yOrigin)
		{
			RotateExtended(angle, xShift, yShift, xOrigin, yOrigin, false);
		}

		public FreeImage Rescale(int width, int height)
		{
			int newHandle = FreeImageApi.Rescale(m_Handle, width, height,FreeImageFilter.BICUBIC);
			return new FreeImage(newHandle);
		}


        public bool FlipVertical()
        {
            return FreeImageApi.FlipVertical(m_Handle);
        }

        public bool FlipHorizontal()
        {
            return FreeImageApi.FlipHorizontal(m_Handle);
        }

        /// <summary>
        /// Converts a bitmap to 1-bit monochrome bitmap using a threshold T between [0..255]. 
        /// The function first converts the bitmap to a 8-bit greyscale bitmap. Then, any brightness 
        /// level that is less than T is set to zero, otherwise to 1. For 1-bit input bitmaps, the 
        /// function clones the input bitmap and builds a monochrome palette.
        /// </summary>
        /// <param name="range"></param>
        /// <returns></returns>
        public FreeImage Threshold(byte range)
        {            
            int i = FreeImageApi.Threshold(m_Handle, range);

            return new FreeImage(i);
        }

        public bool Invert()
        {
            return FreeImageApi.Invert(m_Handle);
        }

		public bool Save(string filename)
		{
			if (File.Exists(filename))
				File.Delete(filename);

			FreeImageFormat fif = FreeImageApi.GetFIFFromFilename(filename);
			FreeImageApi.SetPluginEnabled(fif, true);

			return FreeImageApi.Save(fif, m_Handle, filename, 0);
		}

		public bool Save(string filename, FreeImageFormat type)
		{
			if (File.Exists(filename))
				File.Delete(filename);

			FreeImageApi.SetPluginEnabled(type, true);

			return FreeImageApi.Save(type, m_Handle, filename, 0);
		}

		#endregion

		#region IDisposable Members

        bool disposed = false;

		public void Dispose()
		{
            if (!disposed)
            {
                      FreeImageApi.Unload(m_Handle);
                if (m_MemPtr != IntPtr.Zero) FreeImageApi.CloseMemory(m_MemPtr);
            }
            disposed = true;
			//FreeImageApi.DeInitialize();
		}

		#endregion

		#region ICloneable Members

		public FreeImage Clone()
		{
			int clone = FreeImageApi.Clone(m_Handle);
			return new FreeImage(clone);
		}
		
		object ICloneable.Clone()
		{
			return this.Clone();
		}

		#endregion


        public void ApplyTransformation(FreeImageTransformation transform)
        {
            transform.Run(this);

            if (TransformationCompleted != null)
                TransformationCompleted(this, new EventArgs());
        }

        internal void DisposeAndSetHandle(int hwnd)
        {
            FreeImageApi.Unload(m_Handle);

            m_Handle = hwnd;
        }
	}
}
