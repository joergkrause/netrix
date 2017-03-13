using System;
using System.IO;
using System.Runtime.InteropServices;
using Comzept.Library.Drawing;

namespace Comzept.Library.Drawing.Internal
{
	using PVOID = IntPtr;
	using FIBITMAP = Int32;
	using FIMULTIBITMAP = Int32;
	

	
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
	

	internal delegate void FreeImage_OutputMessageFunction(FreeImage.FreeImageFormat format, string msg);

	internal class FreeImageApi
	{
		// Init/Error routines ----------------------------------------
		[DllImport("FreeImage.dll", EntryPoint="FreeImage_Initialise")]
		public static extern void Initialise(bool loadLocalPluginsOnly);
		
		// alias for Americans :)
		[DllImport("FreeImage.dll", EntryPoint="FreeImage_Initialise")]
		public static extern void Initialize(bool loadLocalPluginsOnly);
		
		
		[DllImport("FreeImage.dll", EntryPoint="FreeImage_DeInitialise")]
		public static extern void DeInitialise();
		
		// alias for Americians :)
		[DllImport("FreeImage.dll", EntryPoint="FreeImage_DeInitialise")]
		public static extern void DeInitialize();

		[DllImport("FreeImage.dll", EntryPoint = "FreeImage_CloseMemory")]
		public static extern void CloseMemory(IntPtr stream);

		// Version routines -------------------------------------------
		[DllImport("FreeImage.dll", EntryPoint="FreeImage_GetVersion")]
		public static extern string GetVersion();
		
		[DllImport("FreeImage.dll", EntryPoint="FreeImage_GetCopyrightMessage")]
		public static extern string GetCopyrightMessage();
	

		
		// Message Output routines ------------------------------------
		// missing void FreeImage_OutputMessageProc(int fif, 
		// 				const char *fmt, ...);
		
		[DllImport("FreeImage.dll", EntryPoint="FreeImage_SetOutputMessage")]
		public static extern void SetOutputMessage(FreeImage_OutputMessageFunction omf);
		
		
		
		// Allocate/Clone/Unload routines -----------------------------
		[DllImport("FreeImage.dll", EntryPoint="FreeImage_Allocate")]
		public static extern FIBITMAP Allocate(int width, int height, 
				int bpp, uint red_mask, uint green_mask, uint blue_mask);
		
		[DllImport("FreeImage.dll", EntryPoint="FreeImage_AllocateT")]
		public static extern FIBITMAP AllocateT(FreeImage.FreeImageType ftype, int width, 
				int height, int bpp, uint red_mask, uint green_mask, 
				uint blue_mask);
		
		[DllImport("FreeImage.dll", EntryPoint="FreeImage_Clone")]
		public static extern FIBITMAP Clone(FIBITMAP dib);

		[DllImport("FreeImage.dll", EntryPoint="FreeImage_Unload")]
		public static extern void Unload(FIBITMAP dib);
		


		// Load/Save routines -----------------------------------------
		[DllImport("FreeImage.dll", EntryPoint="FreeImage_Load")]
		public static extern FIBITMAP Load(FreeImage.FreeImageFormat format, string filename, int flags);
	
		// missing FIBITMAP FreeImage_LoadFromHandle(FreeImage.FreeImageFormat fif,
		// 				FreeImageIO *io, fi_handle handle, int flags);

		[DllImport("FreeImage.dll", EntryPoint="FreeImage_Save")]
		public static extern bool Save(FreeImage.FreeImageFormat format, FIBITMAP dib, string filename, int flags);
		
		// missing BOOL FreeImage_SaveToHandle(FreeImage.FreeImageFormat fif, FIBITMAP *dib,
		// 				FreeImageIO *io, fi_handle handle, int flags);
		

		// Plugin interface -------------------------------------------
		// missing FreeImage.FreeImageFormat FreeImage_RegisterLocalPlugin(FI_InitProc proc_address, 
		// 				const char *format, const char *description, 
		// 				const char *extension, const char *regexpr);
		//
		// missing FreeImage.FreeImageFormat FreeImage_RegisterExternalPlugin(const char *path,
		// 				const char *format, const char *description,
		// 				const char *extension, const char *regexpr);
		
		[DllImport("FreeImage.dll", EntryPoint="FreeImage_GetFIFCount")]
		public static extern int GetFIFCount();
		
		[DllImport("FreeImage.dll", EntryPoint="FreeImage_SetPluginEnabled")]
		public static extern int SetPluginEnabled(FreeImage.FreeImageFormat format, bool enabled);
		
		[DllImport("FreeImage.dll", EntryPoint="FreeImage_IsPluginEnabled")]
		public static extern int IsPluginEnabled(FreeImage.FreeImageFormat format);
		
		[DllImport("FreeImage.dll", EntryPoint="FreeImage_GetFIFFromFormat")]
		public static extern FreeImage.FreeImageFormat GetFIFFromFormat(string format);
		
		[DllImport("FreeImage.dll", EntryPoint="FreeImage_GetFIFFromMime")]
		public static extern FreeImage.FreeImageFormat GetFIFFromMime(string mime);
		
		[DllImport("FreeImage.dll", EntryPoint="FreeImage_GetFormatFromFIF")]
		public static extern string GetFormatFromFIF(FreeImage.FreeImageFormat format);
		
		[DllImport("FreeImage.dll", EntryPoint="FreeImage_GetFIFExtensionList")]
		public static extern string GetFIFExtensionList(FreeImage.FreeImageFormat format);
		
		[DllImport("FreeImage.dll", EntryPoint="FreeImage_GetFIFDescription")]
		public static extern string GetFIFDescription(FreeImage.FreeImageFormat format);
		
		[DllImport("FreeImage.dll", EntryPoint="FreeImage_GetFIFRegExpr")]
		public static extern string GetFIFRegExpr(FreeImage.FreeImageFormat format);
		
		[DllImport("FreeImage.dll", EntryPoint="FreeImage_GetFIFFromFilename")]
		public static extern FreeImage.FreeImageFormat GetFIFFromFilename([MarshalAs( UnmanagedType.LPStr) ]string filename);
		
		[DllImport("FreeImage.dll", EntryPoint="FreeImage_FIFSupportsReading")]
		public static extern bool FIFSupportsReading(FreeImage.FreeImageFormat format);
		
		[DllImport("FreeImage.dll", EntryPoint="FreeImage_FIFSupportsWriting")]
		public static extern bool FIFSupportsWriting(FreeImage.FreeImageFormat format);
		
		[DllImport("FreeImage.dll", EntryPoint="FreeImage_FIFSupportsExportBPP")]
		public static extern bool FIFSupportsExportBPP(FreeImage.FreeImageFormat format, int bpp);
		
		[DllImport("FreeImage.dll", EntryPoint="FreeImage_FIFSupportsExportType")]
		public static extern bool FIFSupportsExportType(FreeImage.FreeImageFormat format, FreeImage.FreeImageType ftype);
		
		[DllImport("FreeImage.dll", EntryPoint="FreeImage_FIFSupportsICCProfiles")]
		public static extern bool FIFSupportsICCProfiles(FreeImage.FreeImageFormat format, FreeImage.FreeImageType ftype);



		// Multipage interface ----------------------------------------
		[DllImport("FreeImage.dll", EntryPoint="FreeImage_OpenMultiBitmap")]
		public static extern FIMULTIBITMAP OpenMultiBitmap(
			FreeImage.FreeImageFormat format, string filename, bool createNew, bool readOnly, bool keepCacheInMemory);
		
		[DllImport("FreeImage.dll", EntryPoint="FreeImage_CloseMultiBitmap")]
		public static extern long CloseMultiBitmap(FIMULTIBITMAP bitmap, int flags);
		
		[DllImport("FreeImage.dll", EntryPoint="FreeImage_GetPageCount")]
		public static extern int GetPageCount(FIMULTIBITMAP bitmap);
		
		[DllImport("FreeImage.dll", EntryPoint="FreeImage_AppendPage")]
		public static extern void AppendPage(FIMULTIBITMAP bitmap, FIBITMAP data);
		
		[DllImport("FreeImage.dll", EntryPoint="FreeImage_InsertPage")]
		public static extern void InsertPage(FIMULTIBITMAP bitmap, int page, FIBITMAP data);
		
		[DllImport("FreeImage.dll", EntryPoint="FreeImage_DeletePage")]
		public static extern void DeletePage(FIMULTIBITMAP bitmap, int page);
		
		[DllImport("FreeImage.dll", EntryPoint="FreeImage_LockPage")]
		public static extern FIBITMAP LockPage(FIMULTIBITMAP bitmap, int page);
		
		[DllImport("FreeImage.dll", EntryPoint="FreeImage_UnlockPage")]
		public static extern void UnlockPage(FIMULTIBITMAP bitmap, int page, bool changed);
		
		[DllImport("FreeImage.dll", EntryPoint="FreeImage_MovePage")]
		public static extern bool MovePage(FIMULTIBITMAP bitmap, int target, int source);
		
		[DllImport("FreeImage.dll", EntryPoint="FreeImage_GetLockedPageNumbers")]
		public static extern bool GetLockedPageNumbers(FIMULTIBITMAP bitmap, IntPtr pages, IntPtr count);
		
		
		
		// File type request routines ---------------------------------
		[DllImport("FreeImage.dll", EntryPoint="FreeImage_GetFileType")]
		public static extern FreeImage.FreeImageFormat GetFileType(string filename, int size);

		// missing FreeImage.FreeImageFormat FreeImage_GetFileTypeFromHandle(FreeImageIO *io,
		// 			fi_handle handle, int size);

		// Image type request routines --------------------------------
		[DllImport("FreeImage.dll", EntryPoint="FreeImage_GetImageType")]
		public static extern FreeImage.FreeImageType GetImageType(FIBITMAP dib);

		
		
		// Info functions ---------------------------------------------
		[DllImport("FreeImage.dll", EntryPoint="FreeImage_IsLittleEndian")]
		public static extern bool IsLittleEndian();
		
		
		
		// Pixel access functions -------------------------------------
		[DllImport("FreeImage.dll", EntryPoint="FreeImage_GetBits")]
		public static extern IntPtr GetBits(FIBITMAP dib);
		
		[DllImport("FreeImage.dll", EntryPoint="FreeImage_GetScanLine")]
		public static extern IntPtr GetScanLine(FIBITMAP dib, int scanline);
		
		[DllImport("FreeImage.dll", EntryPoint="FreeImage_GetPixelIndex")]
		public static extern bool GetPixelIndex(FIBITMAP dib, uint x, uint y, byte value);
		
		
		
		[DllImport("FreeImage.dll", EntryPoint="FreeImage_GetColorsUsed")]
		public static extern uint GetColorsUsed(FIBITMAP dib);
		
		[DllImport("FreeImage.dll", EntryPoint="FreeImage_GetBPP")]
		public static extern uint GetBPP(FIBITMAP dib);
		
		[DllImport("FreeImage.dll", EntryPoint="FreeImage_GetWidth")]
		public static extern uint GetWidth(FIBITMAP dib);
		
		[DllImport("FreeImage.dll", EntryPoint="FreeImage_GetHeight")]
		public static extern uint GetHeight(FIBITMAP dib);
		
		[DllImport("FreeImage.dll", EntryPoint="FreeImage_GetLine")]
		public static extern uint GetLine(FIBITMAP dib);
		
		[DllImport("FreeImage.dll", EntryPoint="FreeImage_GetPitch")]
		public static extern uint GetPitch(FIBITMAP dib);
		
		[DllImport("FreeImage.dll", EntryPoint="FreeImage_GetDIBSize")]
		public static extern uint GetDIBSize(FIBITMAP dib);
		
		[DllImport("FreeImage.dll", EntryPoint="FreeImage_GetPalette")]
		[return: MarshalAs(UnmanagedType.LPStruct)]
		public static extern RGBQUAD GetPalette(FIBITMAP dib);
		
		[DllImport("FreeImage.dll", EntryPoint="FreeImage_GetDotsPerMeterX")]
		public static extern uint GetDotsPerMeterX(FIBITMAP dib);
		
		[DllImport("FreeImage.dll", EntryPoint="FreeImage_GetDotsPerMeterY")]
		public static extern uint GetDotsPerMeterY(FIBITMAP dib);
		
		[DllImport("FreeImage.dll", EntryPoint="FreeImage_GetInfoHeader")]
		[return: MarshalAs(UnmanagedType.LPStruct)]
		public static extern BITMAPINFOHEADER GetInfoHeader(FIBITMAP dib);
		
		[DllImport("FreeImage.dll", EntryPoint="FreeImage_GetInfo")]
		public static extern IntPtr GetInfo(FIBITMAP dib);
		
		[DllImport("FreeImage.dll", EntryPoint="FreeImage_GetColorType")]
		public static extern int GetColorType(FIBITMAP dib);
		
		[DllImport("FreeImage.dll", EntryPoint="FreeImage_GetRedMask")]
		public static extern uint GetRedMask(FIBITMAP dib);
		
		[DllImport("FreeImage.dll", EntryPoint="FreeImage_GetGreenMask")]
		public static extern uint GetGreenMask(FIBITMAP dib);
		
		[DllImport("FreeImage.dll", EntryPoint="FreeImage_GetBlueMask")]
		public static extern uint GetBlueMask(FIBITMAP dib);
		
		[DllImport("FreeImage.dll", EntryPoint="FreeImage_GetTransparencyCount")]
		public static extern uint GetTransparencyCount(FIBITMAP dib);
		
		[DllImport("FreeImage.dll", EntryPoint="FreeImage_GetTransparencyTable")]
		public static extern IntPtr GetTransparencyTable(FIBITMAP dib);
		
		[DllImport("FreeImage.dll", EntryPoint="FreeImage_SetTransparent")]
		public static extern void SetTransparent(FIBITMAP dib, bool enabled);
		
		[DllImport("FreeImage.dll", EntryPoint="FreeImage_IsTransparent")]
		public static extern bool IsTransparent(FIBITMAP dib);

		[DllImport("FreeImage.dll", EntryPoint="FreeImage_ConvertTo8Bits")]
		public static extern FIBITMAP ConvertTo8Bits(FIBITMAP dib);
		
		[DllImport("FreeImage.dll", EntryPoint="FreeImage_ConvertTo16Bits555")]
		public static extern FIBITMAP ConvertTo16Bits555(FIBITMAP dib);
		
		[DllImport("FreeImage.dll", EntryPoint="FreeImage_ConvertTo16Bits565")]
		public static extern FIBITMAP ConvertTo16Bits565(FIBITMAP dib);
		
		[DllImport("FreeImage.dll", EntryPoint="FreeImage_ConvertTo24Bits")]
		public static extern FIBITMAP ConvertTo24Bits(FIBITMAP dib);
		
		[DllImport("FreeImage.dll", EntryPoint="FreeImage_ConvertTo32Bits")]
		public static extern FIBITMAP ConvertTo32Bits(FIBITMAP dib);
		
		[DllImport("FreeImage.dll", EntryPoint="ColorQuantize")]
		public static extern FIBITMAP ColorQuantize(FIBITMAP dib, FreeImage.FreeImageQuantize quantize);
		
		[DllImport("FreeImage.dll", EntryPoint="FreeImage_Threshold")]
		public static extern FIBITMAP Threshold(FIBITMAP dib, byte t);
		
		[DllImport("FreeImage.dll", EntryPoint="FreeImage_Dither")]
		public static extern FIBITMAP Dither(FIBITMAP dib, FreeImage.FreeImageDither algorithm);
		
		[DllImport("FreeImage.dll", EntryPoint="FreeImage_ConvertFromRawBits")]
		public static extern FIBITMAP ConvertFromRawBits(byte[] bits, int width, int height,
			int pitch, uint bpp, uint redMask, uint greenMask, uint blueMask, bool topDown);
		
		[DllImport("FreeImage.dll", EntryPoint="FreeImage_ConvertToRawBits")]
		public static extern void ConvertToRawBits(IntPtr bits, FIBITMAP dib, int pitch,
			uint bpp, uint redMask, uint greenMask, uint blueMask, bool topDown);
		
		[DllImport("FreeImage.dll", EntryPoint="FreeImage_RotateClassic")]
		public static extern FIBITMAP RotateClassic(FIBITMAP dib, Double angle);
		
		[DllImport("FreeImage.dll", EntryPoint="FreeImage_RotateEx")]
		public static extern FIBITMAP RotateEx(
			FIBITMAP dib, Double angle, Double xShift, Double yShift, Double xOrigin, Double yOrigin, bool useMask);
		
		[DllImport("FreeImage.dll", EntryPoint="FreeImage_FlipHorizontal")]
		public static extern bool FlipHorizontal(FIBITMAP dib);
		
		[DllImport("FreeImage.dll", EntryPoint="FreeImage_FlipVertical")]
		public static extern bool FlipVertical(FIBITMAP dib);
		
		
		
		[DllImport("FreeImage.dll", EntryPoint="FreeImage_Rescale")]
		public static extern FIBITMAP Rescale(FIBITMAP dib, int dst_width, int dst_height, FreeImage.FreeImageFilter filter);
		
		
		
		[DllImport("FreeImage.dll", EntryPoint="FreeImage_AdjustCurve")]
		public static extern bool AdjustCurve(FIBITMAP dib, byte[] lut, FreeImage.FreeImageColorChannel channel);
		
		[DllImport("FreeImage.dll", EntryPoint="FreeImage_AdjustGamma")]
		public static extern bool AdjustGamma(FIBITMAP dib, Double gamma);
		
		[DllImport("FreeImage.dll", EntryPoint="FreeImage_AdjustBrightness")]
		public static extern bool AdjustBrightness(FIBITMAP dib, Double percentage);
		
		[DllImport("FreeImage.dll", EntryPoint="FreeImage_AdjustContrast")]
		public static extern bool AdjustContrast(FIBITMAP dib, Double percentage);
		
		[DllImport("FreeImage.dll", EntryPoint="FreeImage_Invert")]
		public static extern bool Invert(FIBITMAP dib);
		
		[DllImport("FreeImage.dll", EntryPoint="FreeImage_GetHistogram")]
		public static extern bool GetHistogram(FIBITMAP dib, int histo, FreeImage.FreeImageColorChannel channel);
		


		[DllImport("FreeImage.dll", EntryPoint="FreeImage_GetChannel")]
		public static extern bool GetChannel(FIBITMAP dib, FreeImage.FreeImageColorChannel channel);
		
		[DllImport("FreeImage.dll", EntryPoint="FreeImage_SetChannel")]
		public static extern bool SetChannel(FIBITMAP dib, FIBITMAP dib8, FreeImage.FreeImageColorChannel channel);
		
		
		
		[DllImport("FreeImage.dll", EntryPoint="FreeImage_Copy")]
		public static extern FIBITMAP Copy(FIBITMAP dib, int left, int top, int right, int bottom);
		
		[DllImport("FreeImage.dll", EntryPoint="FreeImage_Paste")]
		public static extern bool Paste(FIBITMAP dst, FIBITMAP src, int left, int top, int alpha);



		[DllImport("FreeImage.dll", EntryPoint="FreeImage_OpenMemory")]
		public static extern IntPtr OpenMemory(IntPtr data, int size);

		[DllImport("FreeImage.dll", EntryPoint = "FreeImage_LoadFromMemory")]
		public static extern int LoadFromMemory(FreeImage.FreeImageFormat format, IntPtr stream, int flags);


	}
}
