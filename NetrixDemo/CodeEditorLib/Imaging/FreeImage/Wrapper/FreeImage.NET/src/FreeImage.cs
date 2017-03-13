// ==========================================================
// FreeImage.NET 3
//
// Design and implementation by
// - David Boland (davidboland@vodafone.ie)
//
// Contributors:
// - Andrew S. Townley
// - Hervé Drolon
//
// This file is part of FreeImage 3
//
// COVERED CODE IS PROVIDED UNDER THIS LICENSE ON AN "AS IS" BASIS,
// WITHOUT WARRANTY OF ANY KIND, EITHER EXPRESSED OR IMPLIED,
// INCLUDING, WITHOUT LIMITATION, WARRANTIES THAT THE COVERED CODE IS
// FREE OF DEFECTS, MERCHANTABLE, FIT FOR A PARTICULAR PURPOSE OR
// NON-INFRINGING. THE ENTIRE RISK AS TO THE QUALITY AND PERFORMANCE
// OF THE COVERED CODE IS WITH YOU. SHOULD ANY COVERED CODE PROVE
// DEFECTIVE IN ANY RESPECT, YOU (NOT THE INITIAL DEVELOPER OR ANY
// OTHER CONTRIBUTOR) ASSUME THE COST OF ANY NECESSARY SERVICING,
// REPAIR OR CORRECTION. THIS DISCLAIMER OF WARRANTY CONSTITUTES AN
// ESSENTIAL PART OF THIS LICENSE. NO USE OF ANY COVERED CODE IS
// AUTHORIZED HEREUNDER EXCEPT UNDER THIS DISCLAIMER.
//
// Use at your own risk!
//
// ==========================================================

using System;
using System.IO;
using System.Runtime.InteropServices;

namespace FreeImageAPI
{
	/**
	Handle to a bitmap
	*/
	using FIBITMAP = UInt32;
	/**
	Handle to a multipage file
	*/
	using FIMULTIBITMAP = UInt32;
	/**
	Handle to a memory I/O stream
	*/
	using FIMEMORY = UInt32;

	/*	[StructLayout(LayoutKind.Sequential)]
		public class FreeImageIO
		{
			public FI_ReadProc readProc;
			public FI_WriteProc writeProc;
			public FI_SeekProc seekProc;
			public FI_TellProc tellProc;
		}
	
		[StructLayout(LayoutKind.Sequential)]
		public class FI_Handle
		{
			public FileStream stream;
		}
		public delegate void FI_ReadProc(IntPtr buffer, uint size, uint count, IntPtr handle);
		public delegate void FI_WriteProc(IntPtr buffer, uint size, uint count, IntPtr handle);
		public delegate int FI_SeekProc(IntPtr handle, long offset, int origin);
		public delegate int FI_TellProc(IntPtr handle);
		*/

	// Types used in the library (directly copied from Windows) -----------------

	[StructLayout(LayoutKind.Sequential)]
	public class RGBQUAD { 
		public byte rgbBlue;
		public byte rgbGreen;
		public byte rgbRed;
		public byte rgbReserved;
	}
	
	[StructLayout(LayoutKind.Sequential)]
	public class RGBTRIPLE { 
		public byte rgbtBlue;
		public byte rgbtGreen;
		public byte rgbtRed;
	}

	[StructLayout(LayoutKind.Sequential)]
	public class BITMAPINFOHEADER {
		public uint size;
		public int width; 
		public int height; 
		public ushort biPlanes; 
		public ushort biBitCount;
		public uint biCompression; 
		public uint biSizeImage; 
		public int biXPelsPerMeter; 
		public int biYPelsPerMeter; 
		public uint biClrUsed; 
		public uint biClrImportant;
	}

	[StructLayout(LayoutKind.Sequential)]
	public class BITMAPINFO {
	  public BITMAPINFOHEADER bmiHeader; 
	  public RGBQUAD bmiColors;
	}

	// Types used in the library (specific to FreeImage) ------------------------
	/** 48-bit RGB 
	*/
	[StructLayout(LayoutKind.Sequential)]
	public class FIRGB16 {
		public ushort red;
		public ushort green;
		public ushort blue;
	}

	/** 64-bit RGBA
	*/
	[StructLayout(LayoutKind.Sequential)]
	public class FIRGBA16 {
		public ushort red;
		public ushort green;
		public ushort blue;
		public ushort alpha;
	}

	/** 96-bit RGB Float
	*/
	[StructLayout(LayoutKind.Sequential)]
	public class FIRGBF {
		public float red;
		public float green;
		public float blue;
	}

	/** 128-bit RGBA Float
	*/
	[StructLayout(LayoutKind.Sequential)]
	public class FIRGBAF {
		public float red;
		public float green;
		public float blue;
		public float alpha;
	}

	/** Data structure for COMPLEX type (complex number)
	*/
	[StructLayout(LayoutKind.Sequential)]
	public class FICOMPLEX {
		// real part
		public double r;
		// imaginary part
		public double i;
	}

	// Important enums ----------------------------------------------------------

	public enum LoadSaveFlags {
		BMP_DEFAULT			= 0,
		BMP_SAVE_RLE		= 1,
		CUT_DEFAULT			= 0,
		DDS_DEFAULT			= 0,
		FAXG3_DEFAULT		= 0,
		GIF_DEFAULT			= 0,
		GIF_LOAD256			= 1,		// Load	the	image as a 256 color image with	ununsed	palette	entries, if	it's 16	or 2 color
		GIF_PLAYBACK		= 2,		// 'Play' the GIF to generate each frame (as 32bpp)	instead	of returning raw frame data	when loading
		HDR_DEFAULT			= 0,
		ICO_DEFAULT			= 0,
		ICO_MAKEALPHA		= 1,		// convert to 32bpp	and	create an alpha	channel	from the AND-mask when loading
		IFF_DEFAULT			= 0,
		JPEG_DEFAULT		= 0,
		JPEG_FAST			= 1,
		JPEG_ACCURATE		= 2,
		JPEG_QUALITYSUPERB	= 0x80,
		JPEG_QUALITYGOOD	= 0x100,
		JPEG_QUALITYNORMAL	= 0x200,
		JPEG_QUALITYAVERAGE	= 0x400,
		JPEG_QUALITYBAD		= 0x800,
		JPEG_CMYK			= 0x1000,	// load	separated CMYK "as is" (use	| to combine with other	flags)
		KOALA_DEFAULT		= 0,
		LBM_DEFAULT			= 0,
		MNG_DEFAULT			= 0,
		PCD_DEFAULT			= 0,
		PCD_BASE			= 1,		// load	the	bitmap sized 768 x 512
		PCD_BASEDIV4		= 2,		// load	the	bitmap sized 384 x 256
		PCD_BASEDIV16		= 3,		// load	the	bitmap sized 192 x 128
		PCX_DEFAULT			= 0,
		PNG_DEFAULT			= 0,
		PNG_IGNOREGAMMA		= 1,		// avoid gamma correction
		PNM_DEFAULT			= 0,
		PNM_SAVE_RAW		= 0,		//	If set the writer saves	in RAW format (i.e.	P4,	P5 or P6)
		PNM_SAVE_ASCII		= 1,		// If	set	the	writer saves in	ASCII format (i.e. P1, P2 or P3)
		PSD_DEFAULT			= 0,
		RAS_DEFAULT			= 0,
		TARGA_DEFAULT		= 0,
		TARGA_LOAD_RGB888	= 1,		//	If set the loader converts RGB555 and ARGB8888 -> RGB888.
		TIFF_DEFAULT		= 0,
		TIFF_CMYK			= 0x0001,	// reads/stores	tags for separated CMYK	(use | to combine with compression flags)
		TIFF_PACKBITS		= 0x0100,	// save using	PACKBITS compression
		TIFF_DEFLATE		= 0x0200,	// save using	DEFLATE	compression	(a.k.a.	ZLIB compression)
		TIFF_ADOBE_DEFLATE	= 0x0400,	// save using	ADOBE DEFLATE compression
		TIFF_NONE			= 0x0800,	// save without any compression
		TIFF_CCITTFAX3		= 0x1000,	//	save using CCITT Group 3 fax encoding
		TIFF_CCITTFAX4		= 0x2000,	//	save using CCITT Group 4 fax encoding
		TIFF_LZW			= 0x4000,	// save	using LZW compression
		TIFF_JPEG			= 0x8000,	// save	using JPEG compression
		WBMP_DEFAULT		= 0,
		XBM_DEFAULT			= 0,
		XPM_DEFAULT			= 0,
	}

	/** I/O image format identifiers.
	*/
	public enum FREE_IMAGE_FORMAT {
		FIF_UNKNOWN = -1,
		FIF_BMP		= 0,
		FIF_ICO		= 1,
		FIF_JPEG	= 2,
		FIF_JNG		= 3,
		FIF_KOALA	= 4,
		FIF_LBM		= 5,
		FIF_IFF = FIF_LBM,
		FIF_MNG		= 6,
		FIF_PBM		= 7,
		FIF_PBMRAW	= 8,
		FIF_PCD		= 9,
		FIF_PCX		= 10,
		FIF_PGM		= 11,
		FIF_PGMRAW	= 12,
		FIF_PNG		= 13,
		FIF_PPM		= 14,
		FIF_PPMRAW	= 15,
		FIF_RAS		= 16,
		FIF_TARGA	= 17,
		FIF_TIFF	= 18,
		FIF_WBMP	= 19,
		FIF_PSD		= 20,
		FIF_CUT		= 21,
		FIF_XBM		= 22,
		FIF_XPM		= 23,
		FIF_DDS     = 24,
		FIF_GIF     = 25,
		FIF_HDR		= 26,
		FIF_FAXG3	= 27
	}

	/** Image type used in FreeImage.
	*/
	public enum FREE_IMAGE_TYPE {
		FIT_UNKNOWN = 0,	// unknown type
		FIT_BITMAP  = 1,	// standard image			: 1-, 4-, 8-, 16-, 24-, 32-bit
		FIT_UINT16	= 2,	// array of unsigned short	: unsigned 16-bit
		FIT_INT16	= 3,	// array of short			: signed 16-bit
		FIT_UINT32	= 4,	// array of unsigned long	: unsigned 32-bit
		FIT_INT32	= 5,	// array of long			: signed 32-bit
		FIT_FLOAT	= 6,	// array of float			: 32-bit IEEE floating point
		FIT_DOUBLE	= 7,	// array of double			: 64-bit IEEE floating point
		FIT_COMPLEX	= 8,	// array of FICOMPLEX		: 2 x 64-bit IEEE floating point
		FIT_RGB16	= 9,	// 48-bit RGB image			: 3 x 16-bit
		FIT_RGBA16	= 10,	// 64-bit RGBA image		: 4 x 16-bit
		FIT_RGBF	= 11,	// 96-bit RGB float image	: 3 x 32-bit IEEE floating point
		FIT_RGBAF	= 12	// 128-bit RGBA float image	: 4 x 32-bit IEEE floating point
	}

	/** Image color type used in FreeImage.
	*/
	public enum	FREE_IMAGE_COLOR_TYPE {
		FIC_MINISWHITE = 0,		// min value is white
		FIC_MINISBLACK = 1,		// min value is black
		FIC_RGB        = 2,		// RGB color model
		FIC_PALETTE    = 3,		// color map indexed
		FIC_RGBALPHA   = 4,		// RGB color model with alpha channel
		FIC_CMYK       = 5		// CMYK color model
	};

	/** Color quantization algorithms.
	Constants used in FreeImage_ColorQuantize.
	*/
	public enum FREE_IMAGE_QUANTIZE	{
		FIQ_WUQUANT = 0,		// Xiaolin Wu color quantization algorithm
		FIQ_NNQUANT = 1			// NeuQuant neural-net quantization algorithm by Anthony Dekker
	}

	/** Dithering algorithms.
	Constants used in FreeImage_Dither.
	*/	
	public enum FREE_IMAGE_DITHER {
		FID_FS			= 0,	// Floyd & Steinberg error diffusion
		FID_BAYER4x4	= 1,	// Bayer ordered dispersed dot dithering (order 2 dithering matrix)
		FID_BAYER8x8	= 2,	// Bayer ordered dispersed dot dithering (order 3 dithering matrix)
		FID_CLUSTER6x6	= 3,	// Ordered clustered dot dithering (order 3 - 6x6 matrix)
		FID_CLUSTER8x8	= 4,	// Ordered clustered dot dithering (order 4 - 8x8 matrix)
		FID_CLUSTER16x16= 5		// Ordered clustered dot dithering (order 8 - 16x16 matrix)
	}
	/** Lossless JPEG transformations
	Constants used in FreeImage_JPEGTransform
	*/
	public enum FREE_IMAGE_JPEG_OPERATION {
		FIJPEG_OP_NONE			= 0,	// no transformation
		FIJPEG_OP_FLIP_H		= 1,	// horizontal flip
		FIJPEG_OP_FLIP_V		= 2,	// vertical flip
		FIJPEG_OP_TRANSPOSE		= 3,	// transpose across UL-to-LR axis
		FIJPEG_OP_TRANSVERSE	= 4,	// transpose across UR-to-LL axis
		FIJPEG_OP_ROTATE_90		= 5,	// 90-degree clockwise rotation
		FIJPEG_OP_ROTATE_180	= 6,	// 180-degree rotation
		FIJPEG_OP_ROTATE_270	= 7		// 270-degree clockwise (or 90 ccw)
	};	

	/** Tone mapping operators.
	Constants used in FreeImage_ToneMapping.
	*/
	public enum FREE_IMAGE_TMO {
		FITMO_DRAGO03	 = 0,	// Adaptive logarithmic mapping (F. Drago, 2003)
		FITMO_REINHARD05 = 1,	// Dynamic range reduction inspired by photoreceptor physiology (E. Reinhard, 2005)
	};

	/** Upsampling / downsampling filters. 
	Constants used in FreeImage_Rescale.
	*/
	public enum FREE_IMAGE_FILTER {
		FILTER_BOX		  = 0,	// Box, pulse, Fourier window, 1st order (constant) b-spline
		FILTER_BICUBIC	  = 1,	// Mitchell & Netravali's two-param cubic filter
		FILTER_BILINEAR   = 2,	// Bilinear filter
		FILTER_BSPLINE	  = 3,	// 4th order (cubic) b-spline
		FILTER_CATMULLROM = 4,	// Catmull-Rom spline, Overhauser spline
		FILTER_LANCZOS3	  = 5	// Lanczos3 filter
	}

	/** Color channels.
	Constants used in color manipulation routines.
	*/
	public enum FREE_IMAGE_COLOR_CHANNEL {
		FICC_RGB	= 0,	// Use red, green and blue channels
		FICC_RED	= 1,	// Use red channel
		FICC_GREEN	= 2,	// Use green channel
		FICC_BLUE	= 3,	// Use blue channel
		FICC_ALPHA	= 4,	// Use alpha channel
		FICC_BLACK	= 5,	// Use black channel
		FICC_REAL	= 6,	// Complex images: use real part
		FICC_IMAG	= 7,	// Complex images: use imaginary part
		FICC_MAG	= 8,	// Complex images: use magnitude
		FICC_PHASE	= 9		// Complex images: use phase
	}


	// Message output function --------------------------------------------------

	public delegate void FreeImage_OutputMessageFunction(FREE_IMAGE_FORMAT format, string msg);
	
	/**
	  FreeImage API
	  See the FreeImage PDF documentation for a definition of each function. 
	*/
	public class FreeImage {
#if (DEBUG)
		private const string dllName = "FreeImaged.dll";
#else
		private const string dllName = "FreeImage.dll";
#endif

		// Init/Error routines ----------------------------------------

		[DllImport(dllName, EntryPoint="FreeImage_Initialise")]
		public static extern void Initialise(bool loadLocalPluginsOnly);
		
		// alias for Americans :)		
		[DllImport(dllName, EntryPoint="FreeImage_Initialise")]
		public static extern void Initialize(bool loadLocalPluginsOnly);
		
		[DllImport(dllName, EntryPoint="FreeImage_DeInitialise")]
		public static extern void DeInitialise();
		
		// alias for Americians :)
		[DllImport(dllName, EntryPoint="FreeImage_DeInitialise")]
		public static extern void DeInitialize();
		
		// Version routines -------------------------------------------

		[DllImport(dllName, EntryPoint="FreeImage_GetVersion")]
		public static extern string GetVersion();
		
		[DllImport(dllName, EntryPoint="FreeImage_GetCopyrightMessage")]
		public static extern string GetCopyrightMessage();
	
		// Message Output routines ------------------------------------
		
		[DllImport(dllName, EntryPoint="FreeImage_SetOutputMessage")]
		public static extern void SetOutputMessage(FreeImage_OutputMessageFunction omf);
		
		// Allocate / Clone / Unload routines ---------------------------------------

		[DllImport(dllName, EntryPoint="FreeImage_Allocate")]
		public static extern FIBITMAP Allocate(int width, int height, 
				int bpp, uint red_mask, uint green_mask, uint blue_mask);
		
		[DllImport(dllName, EntryPoint="FreeImage_AllocateT")]
		public static extern FIBITMAP AllocateT(FREE_IMAGE_TYPE ftype, int width, 
				int height, int bpp, uint red_mask, uint green_mask, uint blue_mask);
		
		[DllImport(dllName, EntryPoint="FreeImage_Clone")]
		public static extern FIBITMAP Clone(FIBITMAP dib);
		
		[DllImport(dllName, EntryPoint="FreeImage_Unload")]
		public static extern void Unload(FIBITMAP dib);
		
		// Load / Save routines -----------------------------------------------------

		[DllImport(dllName, EntryPoint="FreeImage_Load")]
		public static extern FIBITMAP Load(FREE_IMAGE_FORMAT format, string filename, int flags);
	
		// missing FIBITMAP FreeImage_LoadFromHandle(FREE_IMAGE_FORMAT fif,
		// 				FreeImageIO *io, fi_handle handle, int flags);

		[DllImport(dllName, EntryPoint="FreeImage_Save")]
		public static extern bool Save(FREE_IMAGE_FORMAT format, FIBITMAP dib, string filename, int flags);
		
		// missing BOOL FreeImage_SaveToHandle(FREE_IMAGE_FORMAT fif, FIBITMAP *dib,
		// 				FreeImageIO *io, fi_handle handle, int flags);
		
		// Memory I/O stream routines -----------------------------------------------

		[DllImport(dllName, EntryPoint="FreeImage_OpenMemory")] 
		public static extern FIMEMORY OpenMemory(IntPtr bits, Int32 size_in_bytes);  
 
		[DllImport(dllName, EntryPoint="FreeImage_CloseMemory")] 
		public static extern void CloseMemory(FIMEMORY stream); 
 
		[DllImport(dllName, EntryPoint="FreeImage_LoadFromMemory")] 
		public static extern FIBITMAP LoadFromMemory(FREE_IMAGE_FORMAT fif, FIMEMORY stream, int flags); 
 
		[DllImport(dllName, EntryPoint="FreeImage_SaveToMemory")] 
		public static extern bool SaveToMemory(FREE_IMAGE_FORMAT fif, FIBITMAP dib, FIMEMORY stream, int flags); 
 
		[DllImport(dllName, EntryPoint="FreeImage_TellMemory")] 
		public static extern long TellMemory(FIMEMORY stream, int flags); 
 
		[DllImport(dllName, EntryPoint="FreeImage_SeekMemory")] 
		public static extern bool SeekMemory(FIMEMORY stream, long offset, int origin); 
 
		[DllImport(dllName, EntryPoint="FreeImage_AcquireMemory")] 
		public static extern long AcquireMemory(FIMEMORY stream, ref IntPtr data, ref int size_in_bytes); 
 

		// Plugin interface -------------------------------------------

		// missing FREE_IMAGE_FORMAT FreeImage_RegisterLocalPlugin(FI_InitProc proc_address, 
		// 				const char *format, const char *description, 
		// 				const char *extension, const char *regexpr);
		//
		// missing FREE_IMAGE_FORMAT FreeImage_RegisterExternalPlugin(const char *path,
		// 				const char *format, const char *description,
		// 				const char *extension, const char *regexpr);
		
		[DllImport(dllName, EntryPoint="FreeImage_GetFIFCount")]
		public static extern int GetFIFCount();
		
		[DllImport(dllName, EntryPoint="FreeImage_SetPluginEnabled")]
		public static extern int SetPluginEnabled(FREE_IMAGE_FORMAT format, bool enabled);
		
		[DllImport(dllName, EntryPoint="FreeImage_IsPluginEnabled")]
		public static extern int IsPluginEnabled(FREE_IMAGE_FORMAT format);
		
		[DllImport(dllName, EntryPoint="FreeImage_GetFIFFromFormat")]
		public static extern FREE_IMAGE_FORMAT GetFIFFromFormat(string format);
		
		[DllImport(dllName, EntryPoint="FreeImage_GetFIFFromMime")]
		public static extern FREE_IMAGE_FORMAT GetFIFFromMime(string mime);
		
		[DllImport(dllName, EntryPoint="FreeImage_GetFormatFromFIF")]
		public static extern string GetFormatFromFIF(FREE_IMAGE_FORMAT format);
		
		[DllImport(dllName, EntryPoint="FreeImage_GetFIFExtensionList")]
		public static extern string GetFIFExtensionList(FREE_IMAGE_FORMAT format);
		
		[DllImport(dllName, EntryPoint="FreeImage_GetFIFDescription")]
		public static extern string GetFIFDescription(FREE_IMAGE_FORMAT format);
		
		[DllImport(dllName, EntryPoint="FreeImage_GetFIFRegExpr")]
		public static extern string GetFIFRegExpr(FREE_IMAGE_FORMAT format);
		
		[DllImport(dllName, EntryPoint="FreeImage_GetFIFFromFilename")]
		public static extern FREE_IMAGE_FORMAT GetFIFFromFilename(string filename);
		
		[DllImport(dllName, EntryPoint="FreeImage_FIFSupportsReading")]
		public static extern bool FIFSupportsReading(FREE_IMAGE_FORMAT format);
		
		[DllImport(dllName, EntryPoint="FreeImage_FIFSupportsWriting")]
		public static extern bool FIFSupportsWriting(FREE_IMAGE_FORMAT format);
		
		[DllImport(dllName, EntryPoint="FreeImage_FIFSupportsExportBPP")]
		public static extern bool FIFSupportsExportBPP(FREE_IMAGE_FORMAT format, int bpp);
		
		[DllImport(dllName, EntryPoint="FreeImage_FIFSupportsExportType")]
		public static extern bool FIFSupportsExportType(FREE_IMAGE_FORMAT format, FREE_IMAGE_TYPE ftype);
		
		[DllImport(dllName, EntryPoint="FreeImage_FIFSupportsICCProfiles")]
		public static extern bool FIFSupportsICCProfiles(FREE_IMAGE_FORMAT format, FREE_IMAGE_TYPE ftype);

		// Multipage interface ----------------------------------------

		[DllImport(dllName, EntryPoint="FreeImage_OpenMultiBitmap")]
		public static extern FIMULTIBITMAP OpenMultiBitmap(
			FREE_IMAGE_FORMAT format, string filename, bool createNew, bool readOnly, bool keepCacheInMemory, int flags);
		
		[DllImport(dllName, EntryPoint="FreeImage_CloseMultiBitmap")]
		public static extern long CloseMultiBitmap(FIMULTIBITMAP bitmap, int flags);
		
		[DllImport(dllName, EntryPoint="FreeImage_GetPageCount")]
		public static extern int GetPageCount(FIMULTIBITMAP bitmap);
		
		[DllImport(dllName, EntryPoint="FreeImage_AppendPage")]
		public static extern void AppendPage(FIMULTIBITMAP bitmap, FIBITMAP data);
		
		[DllImport(dllName, EntryPoint="FreeImage_InsertPage")]
		public static extern void InsertPage(FIMULTIBITMAP bitmap, int page, FIBITMAP data);
		
		[DllImport(dllName, EntryPoint="FreeImage_DeletePage")]
		public static extern void DeletePage(FIMULTIBITMAP bitmap, int page);
		
		[DllImport(dllName, EntryPoint="FreeImage_LockPage")]
		public static extern FIBITMAP LockPage(FIMULTIBITMAP bitmap, int page);
		
		[DllImport(dllName, EntryPoint="FreeImage_UnlockPage")]
		public static extern void UnlockPage(FIMULTIBITMAP bitmap, FIBITMAP data, bool changed);
		
		[DllImport(dllName, EntryPoint="FreeImage_MovePage")]
		public static extern bool MovePage(FIMULTIBITMAP bitmap, int target, int source);
		
		[DllImport(dllName, EntryPoint="FreeImage_GetLockedPageNumbers")]
		public static extern bool GetLockedPageNumbers(FIMULTIBITMAP bitmap, IntPtr pages, IntPtr count);
		
		// File type request routines ---------------------------------

		[DllImport(dllName, EntryPoint="FreeImage_GetFileType")]
		public static extern FREE_IMAGE_FORMAT GetFileType(string filename, int size);

		// missing FREE_IMAGE_FORMAT FreeImage_GetFileTypeFromHandle(FreeImageIO *io,
		// 			fi_handle handle, int size);

		[DllImport(dllName, EntryPoint="FreeImage_GetFileTypeFromMemory")]
		public static extern FREE_IMAGE_FORMAT GetFileTypeFromMemory(FIMEMORY stream, int size);

		// Image type request routines --------------------------------

		[DllImport(dllName, EntryPoint="FreeImage_GetImageType")]
		public static extern FREE_IMAGE_TYPE GetImageType(FIBITMAP dib);
		
		// FreeImage helper routines ------------------------------------------------

		[DllImport(dllName, EntryPoint="FreeImage_IsLittleEndian")]
		public static extern bool IsLittleEndian();
		
		[DllImport(dllName, EntryPoint="FreeImage_LookupX11Color")]
		public static extern bool LookupX11Color(string szColor, ref int red, ref int green, ref int blue);

		[DllImport(dllName, EntryPoint="FreeImage_LookupSVGColor")]
		public static extern bool LookupSVGColor(string szColor, ref int red, ref int green, ref int blue);

		// Pixel access functions -------------------------------------

		[DllImport(dllName, EntryPoint="FreeImage_GetBits")]
		public static extern IntPtr GetBits(FIBITMAP dib);
		
		[DllImport(dllName, EntryPoint="FreeImage_GetScanLine")]
		public static extern IntPtr GetScanLine(FIBITMAP dib, int scanline);
		
		[DllImport(dllName, EntryPoint="FreeImage_GetPixelIndex")]
		public static extern bool GetPixelIndex(FIBITMAP dib, uint x, uint y, ref byte value);
		
		[DllImport(dllName, EntryPoint="FreeImage_GetPixelColor")]
		public static extern bool GetPixelColor(FIBITMAP dib, uint x, uint y, 
			[Out, MarshalAs(UnmanagedType.LPStruct)]RGBQUAD value);
		
		[DllImport(dllName, EntryPoint="FreeImage_SetPixelIndex")]
		public static extern bool SetPixelIndex(FIBITMAP dib, uint x, uint y, ref byte value);

		[DllImport(dllName, EntryPoint="FreeImage_SetPixelColor")]
		public static extern bool SetPixelColor(FIBITMAP dib, uint x, uint y, 
			[In, MarshalAs(UnmanagedType.LPStruct)] RGBQUAD value);

		// DIB info routines --------------------------------------------------------
		
		[DllImport(dllName, EntryPoint="FreeImage_GetColorsUsed")]
		public static extern uint GetColorsUsed(FIBITMAP dib);
		
		[DllImport(dllName, EntryPoint="FreeImage_GetBPP")]
		public static extern uint GetBPP(FIBITMAP dib);
		
		[DllImport(dllName, EntryPoint="FreeImage_GetWidth")]
		public static extern uint GetWidth(FIBITMAP dib);
		
		[DllImport(dllName, EntryPoint="FreeImage_GetHeight")]
		public static extern uint GetHeight(FIBITMAP dib);
		
		[DllImport(dllName, EntryPoint="FreeImage_GetLine")]
		public static extern uint GetLine(FIBITMAP dib);
		
		[DllImport(dllName, EntryPoint="FreeImage_GetPitch")]
		public static extern uint GetPitch(FIBITMAP dib);
		
		[DllImport(dllName, EntryPoint="FreeImage_GetDIBSize")]
		public static extern uint GetDIBSize(FIBITMAP dib);
		
		[DllImport(dllName, EntryPoint="FreeImage_GetPalette")]
		[return: MarshalAs(UnmanagedType.LPArray)]
		public static extern RGBQUAD GetPalette(FIBITMAP dib);
		
		[DllImport(dllName, EntryPoint="FreeImage_GetDotsPerMeterX")]
		public static extern uint GetDotsPerMeterX(FIBITMAP dib);
		
		[DllImport(dllName, EntryPoint="FreeImage_GetDotsPerMeterY")]
		public static extern uint GetDotsPerMeterY(FIBITMAP dib);
		
		[DllImport(dllName, EntryPoint="FreeImage_GetInfoHeader")]
		[return: MarshalAs(UnmanagedType.LPStruct)]
		public static extern BITMAPINFOHEADER GetInfoHeader(FIBITMAP dib);
		
		[DllImport(dllName, EntryPoint="FreeImage_GetInfo")]
		[return: MarshalAs(UnmanagedType.LPStruct)]
		public static extern BITMAPINFO GetInfo(FIBITMAP dib);
		
		[DllImport(dllName, EntryPoint="FreeImage_GetColorType")]
		public static extern FREE_IMAGE_COLOR_TYPE GetColorType(FIBITMAP dib);
		
		[DllImport(dllName, EntryPoint="FreeImage_GetRedMask")]
		public static extern uint GetRedMask(FIBITMAP dib);
		
		[DllImport(dllName, EntryPoint="FreeImage_GetGreenMask")]
		public static extern uint GetGreenMask(FIBITMAP dib);
		
		[DllImport(dllName, EntryPoint="FreeImage_GetBlueMask")]
		public static extern uint GetBlueMask(FIBITMAP dib);
		
		[DllImport(dllName, EntryPoint="FreeImage_GetTransparencyCount")]
		public static extern uint GetTransparencyCount(FIBITMAP dib);
		
		[DllImport(dllName, EntryPoint="FreeImage_GetTransparencyTable")]
		public static extern IntPtr GetTransparencyTable(FIBITMAP dib);
		
		[DllImport(dllName, EntryPoint="FreeImage_SetTransparent")]
		public static extern void SetTransparent(FIBITMAP dib, bool enabled);
		
		[DllImport(dllName, EntryPoint="FreeImage_SetTransparencyTable")]
		public static extern void SetTransparencyTable(FIBITMAP dib, IntPtr table, int count);

		[DllImport(dllName, EntryPoint="FreeImage_IsTransparent")]
		public static extern bool IsTransparent(FIBITMAP dib);
		
		[DllImport(dllName, EntryPoint="FreeImage_HasBackgroundColor")]
		public static extern bool HasBackgroundColor(FIBITMAP dib);

		[DllImport(dllName, EntryPoint="FreeImage_GetBackgroundColor")]
		public static extern bool GetBackgroundColor(FIBITMAP dib, 
			[Out, MarshalAs(UnmanagedType.LPStruct)]RGBQUAD bkcolor);

		[DllImport(dllName, EntryPoint="FreeImage_SetBackgroundColor")]
		public static extern bool SetBackgroundColor(FIBITMAP dib, 
			[In, MarshalAs(UnmanagedType.LPStruct)]RGBQUAD bkcolor);

		// Smart conversion routines ------------------------------------------------

		[DllImport(dllName, EntryPoint="FreeImage_ConvertTo4Bits")]
		public static extern FIBITMAP ConvertTo4Bits(FIBITMAP dib);

		[DllImport(dllName, EntryPoint="FreeImage_ConvertTo8Bits")]
		public static extern FIBITMAP ConvertTo8Bits(FIBITMAP dib);

		[DllImport(dllName, EntryPoint="FreeImage_ConvertTo16Bits555")]
		public static extern FIBITMAP ConvertTo16Bits555(FIBITMAP dib);

		[DllImport(dllName, EntryPoint="FreeImage_ConvertTo16Bits565")]
		public static extern FIBITMAP ConvertTo16Bits565(FIBITMAP dib);

		[DllImport(dllName, EntryPoint="FreeImage_ConvertTo24Bits")]
		public static extern FIBITMAP ConvertTo24Bits(FIBITMAP dib);

		[DllImport(dllName, EntryPoint="FreeImage_ConvertTo32Bits")]
		public static extern FIBITMAP ConvertTo32Bits(FIBITMAP dib);

		[DllImport(dllName, EntryPoint="FreeImage_ColorQuantize")]
		public static extern FIBITMAP ColorQuantize(FIBITMAP dib, FREE_IMAGE_QUANTIZE quantize);

		[DllImport(dllName, EntryPoint="FreeImage_Threshold")]
		public static extern FIBITMAP Threshold(FIBITMAP dib, uint T);

		[DllImport(dllName, EntryPoint="FreeImage_Dither")]
		public static extern FIBITMAP Dither(FIBITMAP dib, FREE_IMAGE_DITHER algorithm);
		
		[DllImport(dllName, EntryPoint="FreeImage_ConvertFromRawBits")]
		public static extern FIBITMAP ConvertFromRawBits(byte[] bits, int width, int height,
			int pitch, uint bpp, uint redMask, uint greenMask, uint blueMask, bool topDown);

		[DllImport(dllName, EntryPoint="FreeImage_ConvertToRawBits")]
		public static extern void ConvertToRawBits(IntPtr bits, FIBITMAP dib, int pitch,
			uint bpp, uint redMask, uint greenMask, uint blueMask, bool topDown);

		[DllImport(dllName, EntryPoint="FreeImage_ConvertToStandardType")]
		public static extern FIBITMAP ConvertToStandardType(FIBITMAP dib, FREE_IMAGE_TYPE dst_type, bool scale_linear);

		[DllImport(dllName, EntryPoint="FreeImage_ConvertToType")]
		public static extern FIBITMAP ConvertToType(FIBITMAP dib, FREE_IMAGE_TYPE dst_type, bool scale_linear);

		[DllImport(dllName, EntryPoint="FreeImage_ConvertToRGBF")]
		public static extern FIBITMAP ConvertToRGBF(FIBITMAP dib);

		[DllImport(dllName, EntryPoint="FreeImage_ToneMapping")]
		public static extern FIBITMAP ToneMapping(FIBITMAP dib, FREE_IMAGE_TMO tmo, double first_param, double second_param);

		[DllImport(dllName, EntryPoint="FreeImage_TmoDrago03")]
		public static extern FIBITMAP TmoDrago03(FIBITMAP dib, double gamma, double exposure);

		[DllImport(dllName, EntryPoint="FreeImage_TmoReinhard05")]
		public static extern FIBITMAP TmoReinhard05(FIBITMAP dib, double intensity, double contrast);

		// ZLib interface -----------------------------------------------------------

		[DllImport(dllName, EntryPoint="FreeImage_ZLibCompress")]
		public static extern Int32 ZLibCompress(IntPtr target, Int32 target_size, IntPtr source, Int32 source_size);

		[DllImport(dllName, EntryPoint="FreeImage_ZLibUncompress")]
		public static extern Int32 ZLibUncompress(IntPtr target, Int32 target_size, IntPtr source, Int32 source_size);

		[DllImport(dllName, EntryPoint="FreeImage_ZLibGZip")]
		public static extern Int32 ZLibGZip(IntPtr target, Int32 target_size, IntPtr source, Int32 source_size);
		
		[DllImport(dllName, EntryPoint="FreeImage_ZLibGUnzip")]
		public static extern Int32 ZLibGUnzip(IntPtr target, Int32 target_size, IntPtr source, Int32 source_size);
		
		[DllImport(dllName, EntryPoint="FreeImage_ZLibCRC32")]
		public static extern Int32 ZLibCRC32(Int32 crc, IntPtr source, Int32 source_size);

		// --------------------------------------------------------------------------
		// Image manipulation toolkit -----------------------------------------------
		// --------------------------------------------------------------------------

		// rotation and flipping
		
		[DllImport(dllName, EntryPoint="FreeImage_RotateClassic")]
		public static extern FIBITMAP RotateClassic(FIBITMAP dib, double angle);
		
		[DllImport(dllName, EntryPoint="FreeImage_RotateEx")]
		public static extern FIBITMAP RotateEx(
			FIBITMAP dib, double angle, double xShift, double yShift, double xOrigin, double yOrigin, bool useMask);
		
		[DllImport(dllName, EntryPoint="FreeImage_FlipHorizontal")]
		public static extern bool FlipHorizontal(FIBITMAP dib);
		
		[DllImport(dllName, EntryPoint="FreeImage_FlipVertical")]
		public static extern bool FlipVertical(FIBITMAP dib);

		[DllImport(dllName, EntryPoint="FreeImage_JPEGTransform")]
		public static extern bool JPEGTransform(string src_file, string dst_file, FREE_IMAGE_JPEG_OPERATION operation, bool perfect);

		// upsampling / downsampling
		
		[DllImport(dllName, EntryPoint="FreeImage_Rescale")]
		public static extern FIBITMAP Rescale(FIBITMAP dib, int dst_width, int dst_height, FREE_IMAGE_FILTER filter);
		
		// color manipulation routines (point operations)		
		
		[DllImport(dllName, EntryPoint="FreeImage_AdjustCurve")]
		public static extern bool AdjustCurve(FIBITMAP dib, byte[] lut, FREE_IMAGE_COLOR_CHANNEL channel);
		
		[DllImport(dllName, EntryPoint="FreeImage_AdjustGamma")]
		public static extern bool AdjustGamma(FIBITMAP dib, double gamma);
		
		[DllImport(dllName, EntryPoint="FreeImage_AdjustBrightness")]
		public static extern bool AdjustBrightness(FIBITMAP dib, double percentage);
		
		[DllImport(dllName, EntryPoint="FreeImage_AdjustContrast")]
		public static extern bool AdjustContrast(FIBITMAP dib, double percentage);
		
		[DllImport(dllName, EntryPoint="FreeImage_Invert")]
		public static extern bool Invert(FIBITMAP dib);
		
		[DllImport(dllName, EntryPoint="FreeImage_GetHistogram")]
		public static extern bool GetHistogram(FIBITMAP dib, IntPtr histo, FREE_IMAGE_COLOR_CHANNEL channel);
		
		// channel processing routines

		[DllImport(dllName, EntryPoint="FreeImage_GetChannel")]
		public static extern FIBITMAP GetChannel(FIBITMAP dib, FREE_IMAGE_COLOR_CHANNEL channel);
		
		[DllImport(dllName, EntryPoint="FreeImage_SetChannel")]
		public static extern bool SetChannel(FIBITMAP dib, FIBITMAP dib8, FREE_IMAGE_COLOR_CHANNEL channel);
		
		// copy / paste / composite routines
		
		[DllImport(dllName, EntryPoint="FreeImage_Copy")]
		public static extern FIBITMAP Copy(FIBITMAP dib, int left, int top, int right, int bottom);
		
		[DllImport(dllName, EntryPoint="FreeImage_Paste")]
		public static extern bool Paste(FIBITMAP dst, FIBITMAP src, int left, int top, int alpha);

		[DllImport(dllName, EntryPoint="FreeImage_Composite")]
		public static extern FIBITMAP Composite(FIBITMAP fg, bool useFileBkg, 
			[In, MarshalAs(UnmanagedType.LPStruct)] RGBQUAD appBkColor, FIBITMAP bg);
	}
}
