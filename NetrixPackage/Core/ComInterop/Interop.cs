using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Security;
using BIND_OPTS = System.Runtime.InteropServices.ComTypes.BIND_OPTS;
using FILETIME = System.Runtime.InteropServices.ComTypes.FILETIME;
#pragma warning disable 1591
namespace GuruComponents.Netrix.ComInterop
{
	/// <exclude/>
	/// <summary>
	/// This class is a wrapper to MSHTML.
	/// </summary>
	/// <remarks>
	/// This class contains all COM related definitions, like interfaces, enumerations,
	/// Win32 API calls, structures and classes to help the interfaces to get implemented
	/// and all GUIDs to find the right entry points in the MSHTML.DLL.
	/// <para>
	/// The interface definitions does not contain the whole MSHTML but the ones we need
	/// to use the NetRix functions.
	/// </para>
	/// </remarks>
	[SuppressUnmanagedCodeSecurity()]
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public sealed class Interop
	{

		# region Enumerations

		public struct PROTOCOLDATA
		{
			public uint grfFlags;
			public uint dwState;
			public IntPtr pData;
			public uint cbData;
		}

		public struct LARGE_INTEGER
		{
			public long QuadPart;
		}

		public struct ULARGE_INTEGER
		{
			public ulong QuadPart;
		}
		public enum QUERYOPTION
		{
			QUERY_EXPIRATION_DATE = 1,
			QUERY_TIME_OF_LAST_CHANGE,
			QUERY_CONTENT_ENCODING,
			QUERY_CONTENT_TYPE,
			QUERY_REFRESH,
			QUERY_RECOMBINE,
			QUERY_CAN_NAVIGATE,
			QUERY_USES_NETWORK,
			QUERY_IS_CACHED,
			QUERY_IS_INSTALLEDENTRY,
			QUERY_IS_CACHED_OR_MAPPED,
			QUERY_USES_CACHE,
			QUERY_IS_SECURE,
			QUERY_IS_SAFE
		}

		public enum SELECTION_TYPE
		{
			None = 0,
			Caret = 1,
			Text = 2,
			Control = 3,
			Max = 2147483647
		}

		/// <summary>
		/// IDM Commands used internally 
		/// </summary>
		public enum IDM
		{
			UNKNOWN = 0,
			ALIGNBOTTOM = 1,
			ALIGNHORIZONTALCENTERS = 2,
			ALIGNLEFT = 3,
			ALIGNRIGHT = 4,
			ALIGNTOGRID = 5,
			ALIGNTOP = 6,
			ALIGNVERTICALCENTERS = 7,
			ARRANGEBOTTOM = 8,
			ARRANGERIGHT = 9,
			BRINGFORWARD = 10,
			BRINGTOFRONT = 11,
			CENTERHORIZONTALLY = 12,
			CENTERVERTICALLY = 13,
			CODE = 14,
			DELETE = 17,
			FONTNAME = 18,
			FONTSIZE = 19,
			GROUP = 20,
			HORIZSPACECONCATENATE = 21,
			HORIZSPACEDECREASE = 22,
			HORIZSPACEINCREASE = 23,
			HORIZSPACEMAKEEQUAL = 24,
			INSERTOBJECT = 25,
			MULTILEVELREDO = 30,
			SENDBACKWARD = 32,
			SENDTOBACK = 33,
			SHOWTABLE = 34,
			SIZETOCONTROL = 35,
			SIZETOCONTROLHEIGHT = 36,
			SIZETOCONTROLWIDTH = 37,
			SIZETOFIT = 38,
			SIZETOGRID = 39,
			SNAPTOGRID = 40,
			TABORDER = 41,
			TOOLBOX = 42,
			MULTILEVELUNDO = 44,
			UNGROUP = 45,
			VERTSPACECONCATENATE = 46,
			VERTSPACEDECREASE = 47,
			VERTSPACEINCREASE = 48,
			VERTSPACEMAKEEQUAL = 49,
			JUSTIFYFULL = 50,
			BACKCOLOR = 51,
			BOLD = 52,
			BORDERCOLOR = 53,
			FLAT = 54,
			FORECOLOR = 55,
			ITALIC = 56,
			JUSTIFYCENTER = 57,
			JUSTIFYGENERAL = 58,
			JUSTIFYLEFT = 59,
			JUSTIFYRIGHT = 60,
			RAISED = 61,
			SUNKEN = 62,
			UNDERLINE = 63,
			CHISELED = 64,
			ETCHED = 65,
			SHADOWED = 66,
			FIND = 67,
			SHOWGRID = 69,
			OBJECTVERBLIST0 = 72,
			OBJECTVERBLIST1 = 73,
			OBJECTVERBLIST2 = 74,
			OBJECTVERBLIST3 = 75,
			OBJECTVERBLIST4 = 76,
			OBJECTVERBLIST5 = 77,
			OBJECTVERBLIST6 = 78,
			OBJECTVERBLIST7 = 79,
			OBJECTVERBLIST8 = 80,
			OBJECTVERBLIST9 = 81,
			CONVERTOBJECT = 82,
			CUSTOMCONTROL = 83,
			CUSTOMIZEITEM = 84,
			RENAME = 85,
			IMPORT = 86,
			NEWPAGE = 87,
			MOVE = 88,
			CANCEL = 89,
			FONT = 90,
			STRIKETHROUGH = 91,
			DELETEWORD = 92,
			EXECPRINT = 93,
			JUSTIFYNONE = 94,
			TRISTATEBOLD = 95,
			TRISTATEITALIC = 96,
			TRISTATEUNDERLINE = 97,

			FOLLOW_ANCHOR = 2008,

			INSINPUTIMAGE = 2114,
			INSINPUTBUTTON = 2115,
			INSINPUTRESET = 2116,
			INSINPUTSUBMIT = 2117,
			INSINPUTUPLOAD = 2118,
			INSFIELDSET = 2119,

			PASTEINSERT = 2120,
			REPLACE = 2121,
			EDITSOURCE = 2122,
			BOOKMARK = 2123,
			HYPERLINK = 2124,
			UNLINK = 2125,
			BROWSEMODE = 2126,
			EDITMODE = 2127,
			UNBOOKMARK = 2128,

			TOOLBARS = 2130,
			STATUSBAR = 2131,
			FORMATMARK = 2132,
			TEXTONLY = 2133,
			OPTIONS = 2135,
			FOLLOWLINKC = 2136,
			FOLLOWLINKN = 2137,
			VIEWSOURCE = 2139,
			ZOOMPOPUP = 2140,

			// IDM_BASELINEFONT1, IDM_BASELINEFONT2, IDM_BASELINEFONT3, IDM_BASELINEFONT4,
			// and IDM_BASELINEFONT5 should be consecutive integers,
			//
			BASELINEFONT1 = 2141,
			BASELINEFONT2 = 2142,
			BASELINEFONT3 = 2143,
			BASELINEFONT4 = 2144,
			BASELINEFONT5 = 2145,

			HORIZONTALLINE = 2150,
			LINEBREAKNORMAL = 2151,
			LINEBREAKLEFT = 2152,
			LINEBREAKRIGHT = 2153,
			LINEBREAKBOTH = 2154,
			NONBREAK = 2155,
			SPECIALCHAR = 2156,
			HTMLSOURCE = 2157,
			IFRAME = 2158,
			HTMLCONTAIN = 2159,
			TEXTBOX = 2161,
			TEXTAREA = 2162,
			CHECKBOX = 2163,
			RADIOBUTTON = 2164,
			DROPDOWNBOX = 2165,
			LISTBOX = 2166,
			BUTTON = 2167,
			IMAGE = 2168,
			OBJECT = 2169,
			ONED = 2170,
			IMAGEMAP = 2171,
			FILE = 2172,
			COMMENT = 2173,
			SCRIPT = 2174,
			JAVAAPPLET = 2175,
			PLUGIN = 2176,
			PAGEBREAK = 2177,
			HTMLAREA = 2178,

			PARAGRAPH = 2180,
			FORM = 2181,
			MARQUEE = 2182,
			LIST = 2183,
			ORDERLIST = 2184,
			UNORDERLIST = 2185,
			INDENT = 2186,
			OUTDENT = 2187,
			PREFORMATTED = 2188,
			ADDRESS = 2189,
			BLINK = 2190,
			DIV = 2191,

			TABLEINSERT = 2200,
			RCINSERT = 2201,
			CELLINSERT = 2202,
			CAPTIONINSERT = 2203,
			CELLMERGE = 2204,
			CELLSPLIT = 2205,
			CELLSELECT = 2206,
			ROWSELECT = 2207,
			COLUMNSELECT = 2208,
			TABLESELECT = 2209,
			TABLEPROPERTIES = 2210,
			CELLPROPERTIES = 2211,
			ROWINSERT = 2212,
			COLUMNINSERT = 2213,

			HELP_CONTENT = 2220,
			HELP_ABOUT = 2221,
			HELP_README = 2222,

			REMOVEFORMAT = 2230,
			PAGEINFO = 2231,
			TELETYPE = 2232,
			GETBLOCKFMTS = 2233,
			BLOCKFMT = 2234,
			SHOWHIDE_CODE = 2235,
			TABLE = 2236,

			COPYFORMAT = 2237,
			PASTEFORMAT = 2238,
			GOTO = 2239,

			CHANGEFONT = 2240,
			CHANGEFONTSIZE = 2241,
			CHANGECASE = 2246,
			SHOWSPECIALCHAR = 2249,

			SUBSCRIPT = 2247,
			SUPERSCRIPT = 2248,

			CENTERALIGNPARA = 2250,
			LEFTALIGNPARA = 2251,
			RIGHTALIGNPARA = 2252,
			REMOVEPARAFORMAT = 2253,
			APPLYNORMAL = 2254,
			APPLYHEADING1 = 2255,
			APPLYHEADING2 = 2256,
			APPLYHEADING3 = 2257,

			DOCPROPERTIES = 2260,
			ADDFAVORITES = 2261,
			COPYSHORTCUT = 2262,
			SAVEBACKGROUND = 2263,
			SETWALLPAPER = 2264,
			COPYBACKGROUND = 2265,
			CREATESHORTCUT = 2266,
			PAGE = 2267,
			SAVETARGET = 2268,
			SHOWPICTURE = 2269,
			SAVEPICTURE = 2270,
			DYNSRCPLAY = 2271,
			DYNSRCSTOP = 2272,
			PRINTTARGET = 2273,
			IMGARTPLAY = 2274,
			IMGARTSTOP = 2275,
			IMGARTREWIND = 2276,
			PRINTQUERYJOBSPENDING = 2277,
			SETDESKTOPITEM = 2278,
			CONTEXTMENU = 2280,
			GOBACKWARD = 2282,
			GOFORWARD = 2283,
			PRESTOP = 2284,

			MP_MYPICS = 2287,
			MP_EMAILPICTURE = 2288,
			MP_PRINTPICTURE = 2289,

			CREATELINK = 2290,
			COPYCONTENT = 2291,

			LANGUAGE = 2292,

			GETPRINTTEMPLATE = 2295,
			SETPRINTTEMPLATE = 2296,
			TEMPLATE_PAGESETUP = 2298,

			REFRESH = 2300,
			STOPDOWNLOAD = 2301,

			ENABLE_INTERACTION = 2302,

			LAUNCHDEBUGGER = 2310,
			BREAKATNEXT = 2311,

			INSINPUTHIDDEN = 2312,
			INSINPUTPASSWORD = 2313,

			OVERWRITE = 2314,

			PARSECOMPLETE = 2315,

			HTMLEDITMODE = 2316,

			REGISTRYREFRESH = 2317,
			COMPOSESETTINGS = 2318,

			SHOWALLTAGS = 2327,
			SHOWALIGNEDSITETAGS = 2321,
			SHOWSCRIPTTAGS = 2322,
			SHOWSTYLETAGS = 2323,
			SHOWCOMMENTTAGS = 2324,
			SHOWAREATAGS = 2325,
			SHOWUNKNOWNTAGS = 2326,
			SHOWMISCTAGS = 2320,
			SHOWZEROBORDERATDESIGNTIME = 2328,

			AUTODETECT = 2329,

			SCRIPTDEBUGGER = 2330,

			GETBYTESDOWNLOADED = 2331,

			NOACTIVATENORMALOLECONTROLS = 2332,
			NOACTIVATEDESIGNTIMECONTROLS = 2333,
			NOACTIVATEJAVAAPPLETS = 2334,
			NOFIXUPURLSONPASTE = 2335,

			EMPTYGLYPHTABLE = 2336,
			ADDTOGLYPHTABLE = 2337,
			REMOVEFROMGLYPHTABLE = 2338,
			REPLACEGLYPHCONTENTS = 2339,

			SHOWWBRTAGS = 2340,

			PERSISTSTREAMSYNC = 2341,
			SETDIRTY = 2342,

			RUNURLSCRIPT = 2343,


			ZOOMRATIO = 2344,
			GETZOOMNUMERATOR = 2345,
			GETZOOMDENOMINATOR = 2346,

			// COMMANDS FOR COMPLEX TEXT
			DIRLTR = 2350,
			DIRRTL = 2351,
			BLOCKDIRLTR = 2352,
			BLOCKDIRRTL = 2353,
			INLINEDIRLTR = 2354,
			INLINEDIRRTL = 2355,

			// SHDOCVW
			ISTRUSTEDDLG = 2356,

			// MSHTMLED
			INSERTSPAN = 2357,
			LOCALIZEEDITOR = 2358,

			// XML MIMEVIEWER
			SAVEPRETRANSFORMSOURCE = 2370,
			VIEWPRETRANSFORMSOURCE = 2371,

			// Scrollbar context menu
			SCROLL_HERE = 2380,
			SCROLL_TOP = 2381,
			SCROLL_BOTTOM = 2382,
			SCROLL_PAGEUP = 2383,
			SCROLL_PAGEDOWN = 2384,
			SCROLL_UP = 2385,
			SCROLL_DOWN = 2386,
			SCROLL_LEFTEDGE = 2387,
			SCROLL_RIGHTEDGE = 2388,
			SCROLL_PAGELEFT = 2389,
			SCROLL_PAGERIGHT = 2390,
			SCROLL_LEFT = 2391,
			SCROLL_RIGHT = 2392,

			// IE 6 Form Editing Commands
			MULTIPLESELECTION = 2393,
			TWOD_POSITION = 2394,
			TWOD_ELEMENT = 2395,
			ONED_ELEMENT = 2396,
			ABSOLUTE_POSITION = 2397,
			LIVERESIZE = 2398,
			ATOMICSELECTION = 2399,

			// Auto URL detection mode
			AUTOURLDETECT_MODE = 2400,

			// Legacy IE50 compatible paste
			IE50_PASTE = 2401,

			// ie50 paste mode
			IE50_PASTE_MODE = 2402,

			//,begin_internal
			GETIPRINT = 2403,
			//,end_internal

			// for disabling selection handles
			DISABLE_EDITFOCUS_UI = 2404,

			// for visibility/display in design
			RESPECTVISIBILITY_INDESIGN = 2405,

			// set css mode
			CSSEDITING_LEVEL = 2406,

			// New outdent
			UI_OUTDENT = 2407,

			// Printing Status
			UPDATEPAGESTATUS = 2408,

			// IME Reconversion 
			IME_ENABLE_RECONVERSION = 2409,
			KEEPSELECTION = 2410,
			UNLOADDOCUMENT = 2411,
			OVERRIDE_CURSOR = 2420,
			PEERHITTESTSAMEINEDIT = 2423,
			TRUSTAPPCACHE = 2425,
			BACKGROUNDIMAGECACHE = 2430,
			DEFAULTBLOCK = 6046,
			MIMECSET__FIRST__ = 3609,
			MIMECSET__LAST__ = 3699,
			MENUEXT_FIRST__ = 3700,
			MENUEXT_LAST__ = 3732,
			MENUEXT_COUNT = 3733,

			// Commands mapped from the standard set.  We should
			// consider deleting them from public header files.

			OPEN = 2000,
			NEW = 2001,
			SAVE = 70,
			SAVEAS = 71,
			SAVECOPYAS = 2002,
			PRINTPREVIEW = 2003,
			SHOWPRINT = 2010,
			SHOWPAGESETUP = 2011,
			PRINT = 27,
			PAGESETUP = 2004,
			SPELL = 2005,
			PASTESPECIAL = 2006,
			CLEARSELECTION = 2007,
			PROPERTIES = 28,
			REDO = 29,
			UNDO = 43,
			SELECTALL = 31,
			ZOOMPERCENT = 50,
			GETZOOM = 68,
			STOP = 2138,
			COPY = 15,
			CUT = 16,
			PASTE = 26,
			PERSISTDEFAULTVALUES = 7100,
			PROTECTMETATAGS = 7101,
			PRESERVEUNDOALWAYS = 6049,
		}

		public enum STGM
		{
			STGM_READ = 0x00000000,
			STGM_WRITE = 0x00000001,
			STGM_READWRITE = 0x00000002,
			STGM_SHARE_DENY_NONE = 0x00000040,
			STGM_SHARE_DENY_READ = 0x00000030,
			STGM_SHARE_DENY_WRITE = 0x00000020,
			STGM_SHARE_EXCLUSIVE = 0x00000010,
			STGM_PRIORITY = 0x00040000,
			STGM_CREATE = 0x00001000,
			STGM_CONVERT = 0x00020000,
			STGM_FAILIFTHERE = 0x00000000,
			STGM_DIRECT = 0x00000000,
			STGM_TRANSACTED = 0x00010000,
			STGM_NOSCRATCH = 0x00100000,
			STGM_NOSNAPSHOT = 0x00200000,
			STGM_SIMPLE = 0x08000000,
			STGM_DIRECT_SWMR = 0x00400000,
			STGM_DELETEONRELEASE = 0x04000000
		}

		public enum PARSEACTION
		{
			PARSE_CANONICALIZE = 1,
			PARSE_FRIENDLY,
			PARSE_SECURITY_URL,
			PARSE_ROOTDOCUMENT,
			PARSE_DOCUMENT,
			PARSE_ANCHOR,
			PARSE_ENCODE,
			PARSE_DECODE,
			PARSE_PATH_FROM_URL,
			PARSE_URL_FROM_PATH,
			PARSE_MIME,
			PARSE_SERVER,
			PARSE_SCHEMA,
			PARSE_SITE,
			PARSE_DOMAIN,
			PARSE_LOCATION,
			PARSE_SECURITY_DOMAIN
		}

		public enum OLECONTF
		{
			OLECONTF_EMBEDDINGS = 1,
			OLECONTF_LINKS = 2,
			OLECONTF_OTHERS = 4,
			OLECONTF_ONLYUSER = 8,
			OLECONTF_ONLYIFRUNNING = 16
		}

		public enum MBID
		{
			Abort = 3,     // Beenden wurde gewählt
			Cancel = 2,     // Abbrechen wurde gewählt
			Ignore = 5,     // Ignorieren wurde gewählt
			No = 7,     // Nein wurde gewählt
			OK = 1,     // OK wurde gewählt
			Retry = 4,     // Wiederholen wurde gewählt
			Yes = 6     // Ja wurde gewählt

		}

		/// <summary>
		/// Messagebox style constants, used to re-create suppressed Messageboxes.
		/// </summary>
		[Flags()]
		public enum MB
		{
			AbortRetryIgnore = 0x2, // Abbrechen, Wiederholen, Weiter
			Help = 0x4000, // Hilfe: wird nur in Verbindung mit einem anderen Button angezeigt
			OK = 0x0, // OK
			OKCancel = 0x1, // OK, Abbrechen
			RetryCancel = 0x5, // Wiederholen, Abbrechen
			YesNo = 0x4, // Ja, Nein
			YesNoCancel = 0x3, // Ja, Nein, Abbrechen
			IconError = 0x10, // fehler symbol
			IconExclamation = 0x30, // ausrufezeichen symbol
			IconInformation = 0x40, // informations symbol
			IconQuestion = 0x20, // fragezeichen symbol
			DefButton1 = 0x0, // standardbutton ist button 1
			DefButton2 = 0x100, // standardbutton ist button 2
			DefButton3 = 0x200, // standardbutton ist button 3
			DefButton4 = 0x300, // standardbutton ist button 4
			ApplModal = 0x0, // messagebox modal zum programm anzeigen
			SystemModal = 0x1000, // messagebox modal zum system anzeigen
			TaskModal = 0x2000, // messagebox modal zum thread anzeigen
			DefaultDesktopOnly = 0x20000, //(winnt/2000) die dialogbox wird// nur auf dem standard desktop// angezeigt
			Right = 0x80000, // der text wird rechts ausgerichtet
			RtlReading = 0x100000, // richtet die schrift der dialogbox von, // rechts nach links, falls dies der, // systemstandard ist
			SetForeground = 0x10000, // die messagebox wird in den, // vordergrund gebracht.
			UserIcon = 0x80, // (msgboxparams) legt fest, dass ein, // benutzerdefiniertes icon angezeigt, // werden soll
			TopMost = 0x40000       // MsgBox wird als 1. in der Z-Order-// Reihenfolge angezeigt

		}

		/// <summary>
		/// Provides a layout behavior with information about the current state of the layout engine.
		/// </summary>
		[Flags]
		public enum BEHAVIOR_LAYOUT_MODE : long
		{
			/// <summary>
			/// The layout engine is requesting the natural size of the element. Both width and height must be returned.
			/// </summary>
			BEHAVIORLAYOUTMODE_NATURAL = 0x0001,
			/// <summary>
			/// The layout engine is requesting the element's minimum width requirement.
			/// </summary>
			BEHAVIORLAYOUTMODE_MINWIDTH = 0x0002,
			/// <summary>
			/// The layout engine is requesting the element's maximum width requirement.
			/// </summary>
			BEHAVIORLAYOUTMODE_MAXWIDTH = 0x0004,
			/// <summary>
			/// The layout engine is requesting the element's media resolution.
			/// </summary>
			BEHAVIORLAYOUTMODE_MEDIA_RESOLUTION = 0x4000,
			/// <summary>
			/// This value is used when the layout engine is running through a final pass to layout percent-sized parent elements. This pass happens when a sized-to-content element is forced to change size and has percent-sized child elements that may need to resize themselves to adjust.
			/// </summary>
			BEHAVIORLAYOUTMODE_FINAL_PERCENT = 0x8000
		}

		/// <summary>
		/// Specifies the type of layout control the layout behavior exhibits.
		/// </summary>
		[Flags]
		public enum BEHAVIOR_LAYOUT_INFO : int
		{
			/// <summary>
			/// The layout behavior completely controls sizing.
			/// </summary>
			BEHAVIORLAYOUTINFO_FULLDELEGATION = 1,
			/// <summary>
			/// The layout behavior modifies the natural content size.
			/// </summary>
			BEHAVIORLAYOUTINFO_MODIFYNATURAL = 2,
			/// <summary>
			/// The layout behavior maps the content size.
			/// </summary>
			BEHAVIORLAYOUTINFO_MAPSIZE = 4
		}

		public enum CARET_DIRECTION : int
		{
			CARET_DIRECTION_INDETERMINATE = 0,
			CARET_DIRECTION_SAME = 1,
			CARET_DIRECTION_BACKWARD = 2,
			CARET_DIRECTION_FORWARD = 3,
			CARET_DIRECTION_Max = 2147483647
		};
		public enum COORD_SYSTEM : int
		{
			COORD_SYSTEM_GLOBAL = 0,
			COORD_SYSTEM_PARENT = 1,
			COORD_SYSTEM_CONTAINER = 2,
			COORD_SYSTEM_CONTENT = 3,
			COORD_SYSTEM_FRAME = 4,
			COORD_SYSTEM_Max = 2147483647
		}

		public enum DROPEFFECTS : int
		{
			DROPEFFECT_NONE = 0,
			DROPEFFECT_COPY = 1,
			DROPEFFECT_MOVE = 2,
			DROPEFFECT_LINK = 4,
			DROPEFFECT_SCROLL = int.MinValue,
		}
		public enum POINTER_GRAVITY : int
		{
			POINTER_GRAVITY_Left = 0,
			POINTER_GRAVITY_Right = 1,
			POINTER_GRAVITY_Max = 2147483647
		};
		public enum ELEMENT_ADJACENCY : int
		{
			ELEM_ADJ_BeforeBegin = 0,
			ELEM_ADJ_AfterBegin = 1,
			ELEM_ADJ_BeforeEnd = 2,
			ELEM_ADJ_AfterEnd = 3,
			ELEMENT_ADJACENCY_Max = 2147483647
		};
		public enum MARKUP_CONTEXT_TYPE : int
		{
			CONTEXT_TYPE_None = 0,
			CONTEXT_TYPE_Text = 1,
			CONTEXT_TYPE_EnterScope = 2,
			CONTEXT_TYPE_ExitScope = 3,
			CONTEXT_TYPE_NoScope = 4,
			MARKUP_CONTEXT_TYPE_Max = 2147483647
		};
		public enum MOVEUNIT_ACTION : int
		{
			MOVEUNIT_PREVCHAR = 0,
			MOVEUNIT_NEXTCHAR = 1,
			MOVEUNIT_PREVCLUSTERBEGIN = 2,
			MOVEUNIT_NEXTCLUSTERBEGIN = 3,
			MOVEUNIT_PREVCLUSTEREND = 4,
			MOVEUNIT_NEXTCLUSTEREND = 5,
			MOVEUNIT_PREVWORDBEGIN = 6,
			MOVEUNIT_NEXTWORDBEGIN = 7,
			MOVEUNIT_PREVWORDEND = 8,
			MOVEUNIT_NEXTWORDEND = 9,
			MOVEUNIT_PREVPROOFWORD = 10,
			MOVEUNIT_NEXTPROOFWORD = 11,
			MOVEUNIT_NEXTURLBEGIN = 12,
			MOVEUNIT_PREVURLBEGIN = 13,
			MOVEUNIT_NEXTURLEND = 14,
			MOVEUNIT_PREVURLEND = 15,
			MOVEUNIT_PREVSENTENCE = 16,
			MOVEUNIT_NEXTSENTENCE = 17,
			MOVEUNIT_PREVBLOCK = 18,
			MOVEUNIT_NEXTBLOCK = 19,
			MOVEUNIT_ACTION_Max = 2147483647
		};
		public enum DISPLAY_MOVEUNIT : int
		{
			DISPLAY_MOVEUNIT_PreviousLine = 1,
			DISPLAY_MOVEUNIT_NextLine = 2,
			DISPLAY_MOVEUNIT_CurrentLineStart = 3,
			DISPLAY_MOVEUNIT_CurrentLineEnd = 4,
			DISPLAY_MOVEUNIT_TopOfWindow = 5,
			DISPLAY_MOVEUNIT_BottomOfWindow = 6,
			DISPLAY_MOVEUNIT_Max = 2147483647
		};
		public enum DISPLAY_GRAVITY : int
		{
			DISPLAY_GRAVITY_PreviousLine = 1,
			DISPLAY_GRAVITY_NextLine = 2,
			DISPLAY_GRAVITY_Max = 2147483647
		}

		public enum ELEMENT_TAG_ID : int
		{
			NULL = 0,
			UNKNOWN = 1,
			A = 2,
			ACRONYM = 3,
			ADDRESS = 4,
			APPLET = 5,
			AREA = 6,
			B = 7,
			BASE = 8,
			BASEFONT = 9,
			BDO = 10,
			BGSOUND = 11,
			BIG = 12,
			BLINK = 13,
			BLOCKQUOTE = 14,
			BODY = 15,
			BR = 16,
			BUTTON = 17,
			CAPTION = 18,
			CENTER = 19,
			CITE = 20,
			CODE = 21,
			COL = 22,
			COLGROUP = 23,
			COMMENT = 24,
			COMMENT_RAW = 25,
			DD = 26,
			DEL = 27,
			DFN = 28,
			DIR = 29,
			DIV = 30,
			DL = 31,
			DT = 32,
			EM = 33,
			EMBED = 34,
			FIELDSET = 35,
			FONT = 36,
			FORM = 37,
			FRAME = 38,
			FRAMESET = 39,
			GENERIC = 40,
			H1 = 41,
			H2 = 42,
			H3 = 43,
			H4 = 44,
			H5 = 45,
			H6 = 46,
			HEAD = 47,
			HR = 48,
			HTML = 49,
			I = 50,
			IFRAME = 51,
			IMG = 52,
			INPUT = 53,
			INS = 54,
			KBD = 55,
			LABEL = 56,
			LEGEND = 57,
			LI = 58,
			LINK = 59,
			LISTING = 60,
			MAP = 61,
			MARQUEE = 62,
			MENU = 63,
			META = 64,
			NEXTID = 65,
			NOBR = 66,
			NOEMBED = 67,
			NOFRAMES = 68,
			NOSCRIPT = 69,
			OBJECT = 70,
			OL = 71,
			OPTION = 72,
			P = 73,
			PARAM = 74,
			PLAINTEXT = 75,
			PRE = 76,
			Q = 77,
			RP = 78,
			RT = 79,
			RUBY = 80,
			S = 81,
			SAMP = 82,
			SCRIPT = 83,
			SELECT = 84,
			SMALL = 85,
			SPAN = 86,
			STRIKE = 87,
			STRONG = 88,
			STYLE = 89,
			SUB = 90,
			SUP = 91,
			TABLE = 92,
			TBODY = 93,
			TC = 94,
			TD = 95,
			TEXTAREA = 96,
			TFOOT = 97,
			TH = 98,
			THEAD = 99,
			TITLE = 100,
			TR = 101,
			TT = 102,
			U = 103,
			UL = 104,
			VAR = 105,
			WBR = 106,
			XMP = 107,
			ROOT = 108,
			OPTGROUP = 109,
			ABBR = 110,
			COUNT = 111,
			LAST_PREDEFINED = 10000
		}

		public enum ELEMENT_CORNER : int
		{
			ELEMENT_CORNER_NONE = 0,
			ELEMENT_CORNER_TOP = 1,
			ELEMENT_CORNER_LEFT = 2,
			ELEMENT_CORNER_BOTTOM = 3,
			ELEMENT_CORNER_RIGHT = 4,
			ELEMENT_CORNER_TOPLEFT = 5,
			ELEMENT_CORNER_TOPRIGHT = 6,
			ELEMENT_CORNER_BOTTOMLEFT = 7,
			ELEMENT_CORNER_BOTTOMRIGHT = 8,
			ELEMENT_CORNER_Max = 2147483647
		}

		public enum OLECMDTEXTF
		{
			OLECMDTEXTF_NONE = 0,
			OLECMDTEXTF_NAME = 1,
			OLECMDTEXTF_STATUS = 2
		}

		public enum DOCHOSTUIDBLCLICK
		{
			DEFAULT = 0x0,
			SHOWCODE = 0x2,
			SHOWPROPERTIES = 0x1
		}

		public enum BEHAVIOR_EVENT
		{
			CONTENTREADY = 0x0,
			DOCUMENTREADY = 0x1,
			APPLYSTYLE = 0x2,
			DOCUMENTCONTEXTCHANGE = 0x3,
			CONTENTSAVE = 0x4
		}

		public enum DOCHOSTUIFLAG
		{
			/// <summary>
			/// MSHTML does not enable selection of the text in the form.
			/// </summary>
			DIALOG = 0x00000001,
			/// <summary>
			/// Not supported in NetRix
			/// </summary>
			DISABLE_HELP_MENU = 0x00000002,
			/// <summary>
			/// MSHTML does not use 3-D borders on any frames or framesets. To turn the border off on only the outer frameset use DOCHOSTUIFLAG_NO3DOUTERBORDER
			/// </summary>
			NO3DBORDER = 0x00000004,
			/// <summary>
			/// MSHTML does not have scroll bars. 
			/// </summary>
			SCROLL_NO = 0x00000008,
			/// <summary>
			/// MSHTML does not execute any script until fully activated.
			/// </summary>
			DISABLE_SCRIPT_INACTIVE = 0x00000010,
			/// <summary>
			/// Not supported in NetRix
			/// </summary>
			OPENNEWWIN = 0x00000020,
			/// <summary>
			/// Not supported in NetRix
			/// </summary>
			DISABLE_OFFSCREEN = 0x00000040,
			/// <summary>
			/// Uses flat scroll bars for any UI it displays.
			/// </summary>
			FLAT_SCROLLBAR = 0x00000080,
			/// <summary>
			/// Inserts the div tag if a return is entered in edit mode. Without this flag, MSHTML will use the p tag. 
			/// </summary>
			DIV_BLOCKDEFAULT = 0x00000100,
			/// <summary>
			/// Becomes UI active if the mouse is clicked in the client area of the window. It does not become UI active if the mouse is clicked on a non-client area, such as a scroll bar.
			/// </summary>
			ACTIVATE_CLIENTHIT_ONLY = 0x00000200,
			OVERRIDEBEHAVIORFACTORY = 0x00000400,
			CODEPAGELINKEDFONTS = 0x00000800,
			URL_ENCODING_DISABLE_UTF8 = 0x00001000,
			URL_ENCODING_ENABLE_UTF8 = 0x00002000,
			ENABLE_FORMS_AUTOCOMPLETE = 0x00004000,
			ENABLE_INPLACE_NAVIGATION = 0x00010000,
			IME_ENABLE_RECONVERSION = 0x00020000,
			THEME = 0x00040000,
			NOTHEME = 0x00080000,
			NOPICS = 0x00100000,
			NO3DOUTERBORDER = 0x00200000,
			DISABLE_EDIT_NS_FIXUP = 0x00400000,
			LOCAL_MACHINE_ACCESS_CHECK = 0x00800000,
			DISABLE_UNTRUSTEDPROTOCOL = 0x01000000,
			HOST_NAVIGATES = 0x02000000,
			ENABLE_REDIRECT_NOTIFICATION = 0x04000000,
			/// <summary>
			/// Causes MSHTML to use the Document Object Model (DOM) to create native "windowless" select controls that can be visually layered under other elements.
			/// </summary>
			/// <remarks>This is ON by default and cannot be changed in Netrix' object model.</remarks>
			USE_WINDOWLESS_SELECTCONTROL = 0x08000000,
			/// <summary>
			/// Causes MSHTML to create standard Microsoft Win32 "windowed" select and drop-down controls. 
			/// </summary>
			/// <remarks>This is OFF by default and cannot be changed in Netrix' object model.</remarks>
			USE_WINDOWED_SELECTCONTROL = 0x10000000,
			ENABLE_ACTIVEX_INACTIVATE_MODE = 0x20000000,
			/// <summary>
			/// Causes layout engine to calculate document pixels as 96 dots per inch (dpi). Normally, a document pixel is the same size as a screen pixel. This flag is equivalent to setting the FEATURE_96DPI_PIXEL feature control key on a per-host basis. 
			/// </summary>
			DPI_AWARE = 0x40000000
		}

		public enum OLE
		{
			OLEIVERB_DISCARDUNDOSTATE = -6,
			OLEIVERB_HIDE = -3,
			OLEIVERB_INPLACEACTIVATE = -5,
			OLECLOSE_NOSAVE = 1,
			OLEIVERB_OPEN = -2,
			OLEIVERB_PRIMARY = 0,
			OLEIVERB_PROPERTIES = -7,
			OLEIVERB_SHOW = -1,
			OLEIVERB_UIACTIVATE = -4
		}

		// InternetProtocol Data notification flags
		[Flags]
		public enum BSCF
		{
			BSCF_FIRSTDATANOTIFICATION = 0x00000001,
			BSCF_INTERMEDIATEDATANOTIFICATION = 0x00000002,
			BSCF_LASTDATANOTIFICATION = 0x00000004,
			BSCF_DATAFULLYAVAILABLE = 0x00000008,
			BSCF_AVAILABLEDATASIZEUNKNOWN = 0x00000010
		}

		public enum BITMAPINFO
		{
			MAX_COLORSIZE = 256
		}

		// Winmm.dll Imports
		public enum WM
		{
			WM_QUERYUISTATE = 297,
			WM_KEYFIRST = 0x0100,
			WM_KEYLAST = 0x0108,
			WM_KEYDOWN = 0x0100,
			WM_KEYUP = 0x0101,
			WM_SETFOCUS = 0x7,
			WM_MOUSEACTIVATE = 0x21,
			WM_PARENTNOTIFY = 0x210,
			WM_ACTIVATE = 0x6,
			WM_KILLFOCUS = 0x8,
			WM_CLOSE = 0x10,
			WM_DESTROY = 0x2,
			WM_LBUTTONDOWN = 0x0201,
			WM_LBUTTONUP = 0x0202,
			WM_LBUTTONDBLCLK = 0x0203,
			WM_RBUTTONDOWN = 0x0204,
			WM_RBUTTONUP = 0x0205,
			WM_RBUTTONDBLCLK = 0x0206,
			WM_MBUTTONDOWN = 0x0207,
			WM_MBUTTONUP = 0x0208,
			WM_MBUTTONDBLCLK = 0x0209,
			WM_XBUTTONDOWN = 0x020B,
			WM_XBUTTONUP = 0x020C,
			WM_MOUSEMOVE = 0x0200,
			WM_MOUSELEAVE = 0x02A3,
			WM_MOUSEHOVER = 0x02A1
		}

		public enum MK
		{
			MK_LBUTTON = 0x0001,
			MK_RBUTTON = 0x0002,
			MK_SHIFT = 0x0004,
			MK_CONTROL = 0x0008,
			MK_MBUTTON = 0x0010,
			MK_XBUTTON1 = 0x0020
		}

		# endregion

		#region Imported constants

		#region Static constants
		public static Guid ElementBehaviorFactory = new Guid("3050f429-98b5-11cf-bb82-00aa00bdce0b");
		public static Guid Guid_MSHTML = new Guid("DE4BA900-59CA-11CF-9592-444553540000");
		public static Guid IID_IUnknown = new Guid("{00000000-0000-0000-C000-000000000046}");
		public static IntPtr NullIntPtr = ((IntPtr)((int)(0)));
		#endregion

		public const int

			E_ABORT = unchecked((int)0x80004004),
			E_ACCESSDENIED = unchecked((int)0x80070005),
			E_FAIL = unchecked((int)0x80004005),
			E_HANDLE = unchecked((int)0x80070006),
			E_INVALIDARG = unchecked((int)0x80070057),
			E_POINTER = unchecked((int)0x80004003),
			E_NOTIMPL = unchecked((int)0x80004001),
			E_NOINTERFACE = unchecked((int)0x80004002),
			E_OUTOFMEMORY = unchecked((int)0x8007000E),
			E_UNEXPECTED = unchecked((int)0x8000FFFF),
			E_PENDING = unchecked((int)0x8000000A),
			E_DEFAULTACTION = unchecked((int)0x800C0011),

			ELEMENTDESCRIPTOR_FLAGS_LITERAL = 1,
			ELEMENTDESCRIPTOR_FLAGS_NESTED_LITERAL = 2,

			ELEMENTNAMESPACE_FLAGS_ALLOWANYTAG = 1,
			ELEMENTNAMESPACE_FLAGS_QUERYFORUNKNOWNTAGS = 2,

			INET_E_DEFAULT_ACTION = (int)INET.INET_E_USE_DEFAULT_PROTOCOLHANDLER,

			S_FALSE = 1,
			S_OK = 0;

		public enum INET : long
		{
			INET_E_AUTHENTICATION_REQUIRED = -2146697207,
			INET_E_CANNOT_CONNECT = -2146697212,
			INET_E_CANNOT_INSTANTIATE_OBJECT = -2146697200,
			INET_E_CANNOT_LOAD_DATA = -2146697201,
			INET_E_CANNOT_LOCK_REQUEST = -2146697194,
			INET_E_CANNOT_REPLACE_SFP_FILE = -2146696448,
			INET_E_CODE_DOWNLOAD_DECLINED = -2146696960,
			INET_E_CONNECTION_TIMEOUT = -2146697205,
			INET_E_DATA_NOT_AVAILABLE = -2146697209,
			INET_E_DEFAULT_ACTION = -2146697199,
			INET_E_DOWNLOAD_FAILURE = -2146697208,
			INET_E_ERROR_FIRST = -2146697214,
			INET_E_ERROR_LAST = -2146697193,
			INET_E_INVALID_REQUEST = -2146697204,
			INET_E_INVALID_URL = -2146697214,
			INET_E_NO_SESSION = -2146697213,
			INET_E_NO_VALID_MEDIA = -2146697206,
			INET_E_OBJECT_NOT_FOUND = -2146697210,
			INET_E_QUERYOPTION_UNKNOWN = -2146697197,
			INET_E_REDIRECT_FAILED = -2146697196,
			INET_E_REDIRECT_TO_DIR = -2146697195,
			INET_E_REDIRECTING = -2146697196,
			INET_E_RESOURCE_NOT_FOUND = -2146697211,
			INET_E_RESULT_DISPATCHED = -2146696704,
			INET_E_SECURITY_PROBLEM = -2146697202,
			INET_E_UNKNOWN_PROTOCOL = -2146697203,
			INET_E_USE_DEFAULT_PROTOCOLHANDLER = -2146697199,
			INET_E_USE_DEFAULT_SETTING = -2146697198,
			INET_E_USE_EXTEND_BINDING = -2146697193
		}


		[ComVisible(false)]
		public enum OLECMDEXECOPT
		{
			OLECMDEXECOPT_DODEFAULT = 0,
			OLECMDEXECOPT_PROMPTUSER = 1,
			OLECMDEXECOPT_DONTPROMPTUSER = 2,
			OLECMDEXECOPT_SHOWHELP = 3
		}

		[ComVisible(false)]
		public enum OLECMDF
		{
			OLECMDF_SUPPORTED = 1,
			OLECMDF_ENABLED = 2,
			OLECMDF_LATCHED = 4,
			OLECMDF_NINCHED = 8
		}
		[ComVisible(false)]
		public enum StreamConsts
		{
			LOCK_WRITE = 0x1,
			LOCK_EXCLUSIVE = 0x2,
			LOCK_ONLYONCE = 0x4,
			STATFLAG_DEFAULT = 0x0,
			STATFLAG_NONAME = 0x1,
			STATFLAG_NOOPEN = 0x2,
			STGC_DEFAULT = 0x0,
			STGC_OVERWRITE = 0x1,
			STGC_ONLYIFCURRENT = 0x2,
			STGC_DANGEROUSLYCOMMITMERELYTODISKCACHE = 0x4,
			STREAM_SEEK_SET = 0x0,
			STREAM_SEEK_CUR = 0x1,
			STREAM_SEEK_END = 0x2
		}

		[ComVisible(false), StructLayout(LayoutKind.Sequential)]
		public sealed class tagLOGPALETTE
		{
			[MarshalAs(UnmanagedType.U2)/*leftover(offset=0, palVersion)*/]
			public short palVersion;

			[MarshalAs(UnmanagedType.U2)/*leftover(offset=2, palNumEntries)*/]
			public short palNumEntries;

			// UNMAPPABLE: palPalEntry: Cannot be used as a structure field.
			//   /** @com.structmap(UNMAPPABLE palPalEntry) */
			//  public UNMAPPABLE palPalEntry;

		}

		[ComVisible(false), StructLayout(LayoutKind.Sequential)]
		public sealed class tagOIFI
		{
			[MarshalAs(UnmanagedType.U4)]
			public int cb;

			[MarshalAs(UnmanagedType.I4)]
			public int fMDIApp;

			public IntPtr hwndFrame;

			public IntPtr hAccel;

			[MarshalAs(UnmanagedType.U4)]
			public int cAccelEntries;

		}


		[ComVisible(false), StructLayout(LayoutKind.Sequential)]
		public sealed class tagOleMenuGroupWidths
		{
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
			public int[] widths = new int[6];
		}

		[ComVisible(false), StructLayout(LayoutKind.Sequential)]
		public sealed class tagOLEVERB
		{
			[MarshalAs(UnmanagedType.I4)]
			public int lVerb;

			[MarshalAs(UnmanagedType.LPWStr)]
			public String lpszVerbName;

			[MarshalAs(UnmanagedType.U4)]
			public int fuFlags;

			[MarshalAs(UnmanagedType.U4)]
			public int grfAttribs;

		}

		#endregion

		#region Structs and common Classes

		[StructLayout(LayoutKind.Sequential)]
		public struct OLECMD
		{
			[MarshalAs(UnmanagedType.U4)]
			public int cmdID;
			[MarshalAs(UnmanagedType.U4)]
			public int cmdf;
		}

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		public class OLECMDTEXT
		{
			public OLECMDTEXTF cmdtextf;
			public int cwActual;
			public readonly int cwBuf = 256;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
			public string text;
		}

		[StructLayout(LayoutKind.Explicit)]
		public class OLEVARIANT
		{
			[FieldOffset(0)]
			public Int16 vt;
			[FieldOffset(2)]
			public Int16 wReserved1;
			[FieldOffset(4)]
			public Int16 wReserved2;
			[FieldOffset(6)]
			public Int16 wReserved3;
			[FieldOffset(8)]
			public int lVal;
			[FieldOffset(8)]
			public short iVal;
			//            [FieldOffset(8)]
			//            public double dblVal;
			[FieldOffset(8)]
			public IntPtr bstrVal;
			[FieldOffset(8)]
			public IntPtr pUnkVal;
			[FieldOffset(8)]
			public IntPtr pArray;
			[FieldOffset(8)]
			public IntPtr pvRecord;
			[FieldOffset(12)]
			public IntPtr pRecInfo;

			public void Clear()
			{
				VariantClear(this);
			}

			public void LoadString(string val)
			{
				this.vt = Convert.ToInt16((byte)this.vt | (byte)VarEnum.VT_BSTR);
				this.bstrVal = Marshal.StringToBSTR(val);
			}

			public void LoadInteger(int val)
			{
				this.vt = Convert.ToInt16(VarEnum.VT_I4);
				this.lVal = val;
			}

			public void LoadBoolean(bool val)
			{
				this.vt = Convert.ToInt16(VarEnum.VT_BOOL);
				this.iVal = (short)(val ? 1 : 0);
			}

			public object ToNativeObject()
			{
				IntPtr p = IntPtr.Zero;
				try
				{
					p = Marshal.AllocCoTaskMem(Marshal.SizeOf(this.GetType()));
					Marshal.StructureToPtr(this, p, false);
					return Marshal.GetObjectForNativeVariant(p);
				}
				finally
				{
					Marshal.FreeCoTaskMem(p);
				}
			}

			[DllImport("Oleaut32.dll", PreserveSig = false)]
			private static extern void VariantClear(OLEVARIANT var);

		}

		/// <exclude />
		[ComVisible(false), StructLayout(LayoutKind.Sequential)]
		public class COMMSG
		{
			/// <exclude />
			public IntPtr hwnd;
			/// <exclude />
			public int message;
			/// <exclude />
			public IntPtr wParam;
			/// <exclude />
			public IntPtr lParam;
			/// <exclude />
			public int time;
			/// <exclude />
			public int pt_x;
			/// <exclude />
			public int pt_y;
		}

		/// <exclude />
		[ComVisible(false), StructLayout(LayoutKind.Sequential)]
		public sealed class DISPPARAMS
		{
			/// <exclude />
			public IntPtr rgvarg;
			/// <exclude />
			public IntPtr rgdispidNamedArgs;
			/// <exclude />
			[MarshalAs(UnmanagedType.U4)]
			public int cArgs;
			/// <exclude />
			[MarshalAs(UnmanagedType.U4)]
			public int cNamedArgs;
		}

		/// <exclude />
		[ComVisible(false), StructLayout(LayoutKind.Sequential)]
		public sealed class EXCEPINFO
		{

			/// <exclude />
			[MarshalAs(UnmanagedType.U2)]
			public short wCode;
			/// <exclude />
			[MarshalAs(UnmanagedType.U2)]
			public short wReserved;
			/// <exclude />
			[MarshalAs(UnmanagedType.BStr)]
			public string bstrSource;
			/// <exclude />
			[MarshalAs(UnmanagedType.BStr)]
			public string bstrDescription;
			/// <exclude />
			[MarshalAs(UnmanagedType.BStr)]
			public string bstrHelpFile;
			/// <exclude />
			[MarshalAs(UnmanagedType.U4)]
			public int dwHelpContext;
			/// <exclude />
			public IntPtr dwReserved;
			/// <exclude />
			public IntPtr dwFillIn;
			/// <exclude />
			[MarshalAs(UnmanagedType.I4)]
			public int scode;
		}

		/// <exclude />
		[StructLayout(LayoutKind.Sequential)]
		public class DOCHOSTUIINFO
		{
			/// <exclude />
			[MarshalAs(UnmanagedType.U4)]
			public int cbSize;

			/// <exclude />
			[MarshalAs(UnmanagedType.I4)]
			public int dwFlags;

			/// <exclude />
			[MarshalAs(UnmanagedType.I4)]
			public int dwDoubleClick;

			/// <exclude />
			[MarshalAs(UnmanagedType.I4)]
			public int dwReserved1;

			/// <exclude />
			[MarshalAs(UnmanagedType.I4)]
			public int dwReserved2;
		}

		/// <summary>
		/// Specifies the desired data or view aspect of the object when drawing or getting data.
		/// </summary>
		[Flags]
		public enum DVASPECT
		{
			/// <summary>
			///     A representation of an object that lets that object be displayed as an embedded
			///    object inside a container. This value is typically specified for compound
			///     document objects. The presentation can be provided for the screen or printer.
			///     </summary>
			DVASPECT_CONTENT = 1,
			///<summary>
			///     A thumbnail representation of an object that lets that object be displayed
			///     in a browsing tool. The thumbnail is approximately a 120 by 120 pixel, 16-color
			///     (recommended), device-independent bitmap potentially wrapped in a metafile.
			///     </summary>
			DVASPECT_THUMBNAIL = 2,
			///<summary>
			///     An iconic representation of an object.
			///     </summary>
			DVASPECT_ICON = 4,
			///<summary>
			///     A representation of an object on the screen as though it were printed to
			///     a printer using the Print command from the File menu. The described data
			///     may represent a sequence of pages.
			///     </summary>
			DVASPECT_DOCPRINT = 8,
		}

		/// <summary>
		/// Provides the managed definition of the TYMED structure.
		/// </summary>
		[Flags]
		public enum TYMED
		{
			// Summary:
			//     No data is being passed.
			TYMED_NULL = 0,
			//
			// Summary:
			//     The storage medium is a global memory handle (HGLOBAL). Allocate the global
			//     handle with the GMEM_SHARE flag. If the System.Runtime.InteropServices.ComTypes.STGMEDIUMSystem.Runtime.InteropServices.ComTypes.STGMEDIUM.pUnkForRelease
			//     member is null, the destination process should use GlobalFree to release
			//     the memory.
			TYMED_HGLOBAL = 1,
			//
			// Summary:
			//     The storage medium is a disk file identified by a path. If the STGMEDIUMSystem.Runtime.InteropServices.ComTypes.STGMEDIUM.pUnkForRelease
			//     member is null, the destination process should use OpenFile to delete the
			//     file.
			TYMED_FILE = 2,
			//
			// Summary:
			//     The storage medium is a stream object identified by an IStream pointer. Use
			//     ISequentialStream::Read to read the data. If the System.Runtime.InteropServices.ComTypes.STGMEDIUMSystem.Runtime.InteropServices.ComTypes.STGMEDIUM.pUnkForRelease
			//     member is not null, the destination process should use IStream::Release to
			//     release the stream component.
			TYMED_ISTREAM = 4,
			//
			// Summary:
			//     The storage medium is a storage component identified by an IStorage pointer.
			//     The data is in the streams and storages contained by this IStorage instance.
			//     If the System.Runtime.InteropServices.ComTypes.STGMEDIUMSystem.Runtime.InteropServices.ComTypes.STGMEDIUM.pUnkForRelease
			//     member is not null, the destination process should use IStorage::Release
			//     to release the storage component.
			TYMED_ISTORAGE = 8,
			//
			// Summary:
			//     The storage medium is a Graphics Device Interface (GDI) component (HBITMAP).
			//     If the System.Runtime.InteropServices.ComTypes.STGMEDIUMSystem.Runtime.InteropServices.ComTypes.STGMEDIUM.pUnkForRelease
			//     member is null, the destination process should use DeleteObject to delete
			//     the bitmap.
			TYMED_GDI = 16,
			//
			// Summary:
			//     The storage medium is a metafile (HMETAFILE). Use the Windows or WIN32 functions
			//     to access the metafile's data. If the System.Runtime.InteropServices.ComTypes.STGMEDIUMSystem.Runtime.InteropServices.ComTypes.STGMEDIUM.pUnkForRelease
			//     member is null, the destination process should use DeleteMetaFile to delete
			//     the bitmap.
			TYMED_MFPICT = 32,
			//
			// Summary:
			//     The storage medium is an enhanced metafile. If the System.Runtime.InteropServices.ComTypes.STGMEDIUMSystem.Runtime.InteropServices.ComTypes.STGMEDIUM.pUnkForRelease
			//     member is null, the destination process should use DeleteEnhMetaFile to delete
			//     the bitmap.
			TYMED_ENHMF = 64,
		}

		/// <exclude />
		public enum CLIPFORMAT
		{
			/// <exclude />
			CF_TEXT = 1,
			/// <exclude />
			CF_BITMAP = 2,
			/// <exclude />
			CF_METAFILEPICT = 3,
			/// <exclude />
			CF_SYLK = 4,
			/// <exclude />
			CF_DIF = 5,
			/// <exclude />
			CF_TIFF = 6,
			/// <exclude />
			CF_OEMTEXT = 7,
			/// <exclude />
			CF_DIB = 8,
			/// <exclude />
			CF_PALETTE = 9,
			/// <exclude />
			CF_PENDATA = 10,
			/// <exclude />
			CF_RIFF = 11,
			/// <exclude />
			CF_WAVE = 12,
			/// <exclude />
			CF_UNICODETEXT = 13,
			/// <exclude />
			CF_ENHMETAFILE = 14,
			/// <exclude />
			CF_HDROP = 15,
			/// <exclude />
			CF_LOCALE = 16,
			/// <exclude />
			CF_DIBV5 = 17
		}

		/// <exclude />
		[StructLayout(LayoutKind.Sequential)]
		public struct FORMATETC
		{
			/// <exclude />
			public short cfFormat;
			/// <exclude />
			public IntPtr ptd;
			/// <exclude />
			[MarshalAs(UnmanagedType.U4)]
			public DVASPECT dwAspect;
			/// <exclude />
			public int lindex;
			/// <exclude />
			[MarshalAs(UnmanagedType.U4)]
			public TYMED tymed;
		};

		[StructLayout(LayoutKind.Sequential)]
		public struct HTML_PAINT_XFORM
		{
			public Single eM11;
			public Single eM12;
			public Single eM21;
			public Single eM22;
			public Single eDx;
			public Single eDy;
		};

		[StructLayout(LayoutKind.Sequential)]
		public struct HTML_PAINT_DRAW_INFO
		{
			public RECT rcViewport;
			public IntPtr hrgnUpdate;
			public HTML_PAINT_XFORM xform;
		};

		/// <exclude />
		[StructLayout(LayoutKind.Sequential)]
		public class HTML_PAINTER_INFO
		{
			/// <exclude />            
			[MarshalAs(UnmanagedType.I4)]
			public int lFlags;

			/// <exclude />
			[MarshalAs(UnmanagedType.I4)]
			public int lZOrder;

			/// <exclude />
			[MarshalAs(UnmanagedType.Struct)]
			public Guid iidDrawObject;

			/// <exclude />
			[MarshalAs(UnmanagedType.Struct)]
			public RECT rcBounds;
		}

		/// <exclude />
		[ComVisible(false), StructLayout(LayoutKind.Sequential)]
		public class NMHDR
		{
			/// <exclude />
			public IntPtr hwndFrom;
			/// <exclude />
			public int idFrom;
			/// <exclude />
			public int code;
		}

		/// <exclude />
		[ComVisible(false), StructLayout(LayoutKind.Sequential)]
		public class NMCUSTOMDRAW
		{
			/// <exclude />
			public NMHDR nmcd;
			/// <exclude />
			public int dwDrawStage;
			/// <exclude />
			public IntPtr hdc;
			/// <exclude />
			public RECT rc;
			/// <exclude />
			public int dwItemSpec;
			/// <exclude />
			public int uItemState;
			/// <exclude />
			public IntPtr lItemlParam;
		}

		/// <exclude />
		[StructLayout(LayoutKind.Sequential)]
		public struct POINT
		{
			/// <exclude />
			public int x;
			/// <exclude />
			public int y;

		}

		/// <exclude />
		[StructLayout(LayoutKind.Sequential)]
		public struct POINTL
		{
			/// <exclude />
			public long x;
			/// <exclude />
			public long y;

		}


		/// <exclude />
		[StructLayout(LayoutKind.Sequential)]
		public class RECT
		{
			/// <exclude />
			public int left;
			/// <exclude />
			public int top;
			/// <exclude />
			public int right;
			/// <exclude />
			public int bottom;

			/// <exclude />
			public RECT()
			{
			}

			/// <exclude />
			public RECT(int left, int top, int right, int bottom)
			{
				this.left = left;
				this.top = top;
				this.right = right;
				this.bottom = bottom;
			}

			/// <exclude />
			public static RECT FromXYWH(int x, int y, int width, int height)
			{
				return new RECT(x, y, x + width, y + height);
			}
		}

		/// <exclude />
		[ComVisible(false), StructLayout(LayoutKind.Sequential)]
		public sealed class STATDATA
		{

			/// <exclude />
			[MarshalAs(UnmanagedType.U4)]
			public int advf;
			/// <exclude />
			[MarshalAs(UnmanagedType.U4)]
			public int dwConnection;

		}

		/// <exclude />
		[StructLayout(LayoutKind.Sequential)]
		public struct STGMEDIUM
		{
			/// <exclude />
			[MarshalAs(UnmanagedType.U4)]
			public int tymed;
			/// <exclude />
			public IntPtr data;
			/// <exclude />
			[MarshalAs(UnmanagedType.IUnknown)]
			public object pUnkForRelease;
		}

		/// <exclude />
		[ComVisible(false), StructLayout(LayoutKind.Sequential)]
		public class STATSTG
		{

			/// <exclude />
			[MarshalAs(UnmanagedType.I4)]
			public int pwcsName;
			/// <exclude />
			[MarshalAs(UnmanagedType.I4)]
			public int type;
			/// <exclude />
			[MarshalAs(UnmanagedType.I8)]
			public long cbSize;
			/// <exclude />
			[MarshalAs(UnmanagedType.I8)]
			public long mtime;
			/// <exclude />
			[MarshalAs(UnmanagedType.I8)]
			public long ctime;
			/// <exclude />
			[MarshalAs(UnmanagedType.I8)]
			public long atime;
			/// <exclude />
			[MarshalAs(UnmanagedType.I8)]
			public long grfMode;
			/// <exclude />
			[MarshalAs(UnmanagedType.I8)]
			public long grfLocksSupported;
			/// <exclude />
			[MarshalAs(UnmanagedType.I4)]
			public int clsid_data1;
			/// <exclude />
			[MarshalAs(UnmanagedType.I2)]
			public short clsid_data2;
			/// <exclude />
			[MarshalAs(UnmanagedType.I2)]
			public short clsid_data3;
			/// <exclude />
			[MarshalAs(UnmanagedType.U1)]
			public byte clsid_b0;
			/// <exclude />
			[MarshalAs(UnmanagedType.U1)]
			public byte clsid_b1;
			/// <exclude />
			[MarshalAs(UnmanagedType.U1)]
			public byte clsid_b2;
			/// <exclude />
			[MarshalAs(UnmanagedType.U1)]
			public byte clsid_b3;
			/// <exclude />
			[MarshalAs(UnmanagedType.U1)]
			public byte clsid_b4;
			/// <exclude />
			[MarshalAs(UnmanagedType.U1)]
			public byte clsid_b5;
			/// <exclude />
			[MarshalAs(UnmanagedType.U1)]
			public byte clsid_b6;
			/// <exclude />
			[MarshalAs(UnmanagedType.U1)]
			public byte clsid_b7;
			/// <exclude />
			[MarshalAs(UnmanagedType.I8)]
			public long grfStateBits;
			/// <exclude />
			[MarshalAs(UnmanagedType.I8)]
			public long reserved;
		}

		/// <exclude />
		[ComVisible(false), StructLayout(LayoutKind.Sequential)]
		public sealed class tagSIZE
		{
			/// <exclude />
			[MarshalAs(UnmanagedType.I4)]
			public int cx;

			/// <exclude />
			[MarshalAs(UnmanagedType.I4)]
			public int cy;

		}

		/// <exclude />
		[StructLayout(LayoutKind.Sequential)]
		public sealed class tagSIZEL
		{
			/// <exclude />
			[MarshalAs(UnmanagedType.I4)]
			public int cx;

			/// <exclude />
			[MarshalAs(UnmanagedType.I4)]
			public int cy;

		}

		#endregion

		# region UrlMon Interfaces and Enumerations

		[Flags]
		public enum BINDINFO
		{
			BINDF_ASYNCHRONOUS = 0x1,
			BINDF_ASYNCSTORAGE = 0x2,
			BINDF_NOPROGRESSIVERENDERING = 0x4,
			BINDF_OFFLINEOPERATION = 0x8,
			BINDF_GETNEWESTVERSION = 0x10,
			BINDF_NOWRITECACHE = 0x20,
			BINDF_NEEDFILE = 0x40,
			BINDF_PULLDATA = 0x80,
			BINDF_IGNORESECURITYPROBLEM = 0x100,
			BINDF_RESYNCHRONIZE = 0x200,
			BINDF_HYPERLINK = 0x400,
			BINDF_NO_UI = 0x800,
			BINDF_SILENTOPERATION = 0x1000,
			BINDF_PRAGMA_NO_CACHE = 0x2000,
			BINDF_GETCLASSOBJECT = 0x4000,
			BINDF_RESERVED_1 = 0x8000,
			BINDF_FREE_THREADED = 0x10000,
			BINDF_DIRECT_READ = 0x20000,
			BINDF_FORMS_SUBMIT = 0x40000,
			BINDF_GETFROMCACHE_IF_NET_FAIL = 0x80000,
			BINDF_FROMURLMON = 0x100000,
			BINDF_FWD_BACK = 0x200000,
			BINDF_PREFERDEFAULTHANDLER = 0x400000,
			BINDF_ENFORCERESTRICTED = 0x800000
		}

		public enum BINDF
		{
			BINDF_ASYNCHRONOUS = 0x00000001,
			BINDF_ASYNCSTORAGE = 0x00000002,
			BINDF_NOPROGRESSIVERENDERING = 0x00000004,
			BINDF_OFFLINEOPERATION = 0x00000008,
			BINDF_GETNEWESTVERSION = 0x00000010,
			BINDF_NOWRITECACHE = 0x00000020,
			BINDF_NEEDFILE = 0x00000040,
			BINDF_PULLDATA = 0x00000080,
			BINDF_IGNORESECURITYPROBLEM = 0x00000100,
			BINDF_RESYNCHRONIZE = 0x00000200,
			BINDF_HYPERLINK = 0x00000400,
			BINDF_NO_UI = 0x00000800,
			BINDF_SILENTOPERATION = 0x00001000,
			BINDF_PRAGMA_NO_CACHE = 0x00002000,
			BINDF_GETCLASSOBJECT = 0x00004000,
			BINDF_RESERVED_1 = 0x00008000,
			BINDF_FREE_THREADED = 0x00010000,
			BINDF_DIRECT_READ = 0x00020000,
			BINDF_FORMS_SUBMIT = 0x00040000,
			BINDF_GETFROMCACHE_IF_NET_FAIL = 0x00080000,
			BINDF_FROMURLMON = 0x00100000,
			BINDF_FWD_BACK = 0x00200000,
			BINDF_PREFERDEFAULTHANDLER = 0x00400000,
			BINDF_RESERVED_3 = 0x00800000
		}

		public enum BINDSTATUS
		{
			BINDSTATUS_FINDINGRESOURCE,
			BINDSTATUS_CONNECTING,
			BINDSTATUS_REDIRECTING,
			BINDSTATUS_BEGINDOWNLOADDATA,
			BINDSTATUS_DOWNLOADINGDATA,
			BINDSTATUS_ENDDOWNLOADDATA,
			BINDSTATUS_BEGINDOWNLOADCOMPONENTS,
			BINDSTATUS_INSTALLINGCOMPONENTS,
			BINDSTATUS_ENDDOWNLOADCOMPONENTS,
			BINDSTATUS_USINGCACHEDCOPY,
			BINDSTATUS_SENDINGREQUEST,
			BINDSTATUS_CLASSIDAVAILABLE,
			BINDSTATUS_MIMETYPEAVAILABLE,
			BINDSTATUS_CACHEFILENAMEAVAILABLE,
			BINDSTATUS_BEGINSYNCOPERATION,
			BINDSTATUS_ENDSYNCOPERATION,
			BINDSTATUS_BEGINUPLOADDATA,
			BINDSTATUS_UPLOADINGDATA,
			BINDSTATUS_ENDUPLOADINGDATA,
			BINDSTATUS_PROTOCOLCLASSID,
			BINDSTATUS_ENCODING,
			BINDSTATUS_VERFIEDMIMETYPEAVAILABLE,
			BINDSTATUS_CLASSINSTALLLOCATION,
			BINDSTATUS_DECODING,
			BINDSTATUS_LOADINGMIMEHANDLER,
			BINDSTATUS_CONTENTDISPOSITIONATTACH,
			BINDSTATUS_FILTERREPORTMIMETYPE,
			BINDSTATUS_CLSIDCANINSTANTIATE,
			BINDSTATUS_IUNKNOWNAVAILABLE,
			BINDSTATUS_DIRECTBIND,
			BINDSTATUS_RAWMIMETYPE,
			BINDSTATUS_PROXYDETECTING,
			BINDSTATUS_ACCEPTRANGES,
			BINDSTATUS_COOKIE_SENT,
			BINDSTATUS_COMPACT_POLICY_RECEIVED,
			BINDSTATUS_COOKIE_SUPPRESSED,
			BINDSTATUS_COOKIE_STATE_UNKNOWN,
			BINDSTATUS_COOKIE_STATE_ACCEPT,
			BINDSTATUS_COOKIE_STATE_REJECT,
			BINDSTATUS_COOKIE_STATE_PROMPT,
			BINDSTATUS_COOKIE_STATE_LEASH,
			BINDSTATUS_COOKIE_STATE_DOWNGRADE,
			BINDSTATUS_POLICY_HREF,
			BINDSTATUS_P3P_HEADER,
			BINDSTATUS_SESSION_COOKIE_RECEIVED,
			BINDSTATUS_PERSISTENT_COOKIE_RECEIVED,
			BINDSTATUS_SESSION_COOKIES_ALLOWED,
			BINDSTATUS_CACHECONTROL,
			BINDSTATUS_CONTENTDISPOSITIONFILENAME,
			BINDSTATUS_MIMETEXTPLAINMISMATCH,
			BINDSTATUS_PUBLISHERAVAILABLE,
			BINDSTATUS_DISPLAYNAMEAVAILABLE
		}

		public enum BINDINFOF : int
		{
			BINDINFOF_URLENCODESTGMEDDATA,
			BINDINFOF_URLENCODEDEXTRAINFO
		}

		public enum BINDVERB : int
		{
			BINDVERB_GET,
			BINDVERB_POST,
			BINDVERB_PUT,
			BINDVERB_CUSTOM
		}

		public struct BindInfo
		{
			public uint cbSize;
			public string szExtraInfo;
			public STGMEDIUM stgmedData;
			public BINDINFOF grfBindInfoF;
			public BINDVERB dwBindVerb;
			public string szCustomVerb;
			public long cbstgmedData;
			public long dwOptions;
			public long dwOptionsFlags;
			public long dwCodePage;
			public int securityAttributes;
			public Guid iid;
			public IUnknown pUnk;
			public long dwReserved;
		}

		[ComVisible(false)]
		[Guid("79eac9c1-baf9-11ce-8c82-00aa004ba90b")]
		[InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IBindStatusCallback
		{
			void OnStartBinding([In] int dwReserved, [In] IBinding pib);

			void GetPriority([Out] long pnPriority);

			void OnLowResource([In] int reserved);

			void OnProgress([In] uint ulProgress,
				[In] uint ulProgressMax,
				[In] BINDSTATUS ulStatusCode,
				[In, MarshalAs(UnmanagedType.LPStr)] string szStatusText);

			void OnStopBinding([In] int hresult,
				[In] string szError);

			void GetBindInfo([Out] out BINDF grfBINDF, [Out][In] ref BindInfo pbindinfo);

			void OnDataAvailable([In] BSCF grfBSCF,
				[In] long dwSize,
				[In] FORMATETC pformatetc,
				[In] STGMEDIUM pstgmed);

			void OnObjectAvailable([In] Guid riid, [In] IUnknown punk);

		}

		[ComVisible(false)]
		[Guid("79eac9c0-baf9-11ce-8c82-00aa004ba90b")]
		[InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IBinding
		{
			void Abort();

			void Suspend();

			void Resume();

			void SetPriority([In] int nPriority);

			void GetPriority([Out] int pnPriority);

			void GetBindResult([Out] Guid pclsidProtocol,
				[Out] long pdwResult,
				[Out] string pszResult,
				[Out][In] long pdwReserved);
		}

		# endregion

		#region Interfaces and classes

		/// <exclude />
		[Guid("0000010F-0000-0000-C000-000000000046"), InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IAdviseSink
		{

			//C#r: UNDONE (Field in interface) public static readonly    Guid iid;
			void OnDataChange(
				[In]
				FORMATETC pFormatetc,
				[In]
				STGMEDIUM pStgmed);

			void OnViewChange(
				[In, MarshalAs(UnmanagedType.U4)]
				int dwAspect,
				[In, MarshalAs(UnmanagedType.I4)]
				int lindex);

			void OnRename(
				[In, MarshalAs(UnmanagedType.Interface)]
				object pmk);

			void OnSave();


			void OnClose();
		}

		/// <exclude />
		[Guid("0000000e-0000-0000-C000-000000000046"), InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IBindCtx
		{
			void EnumObjectParam(out IEnumString ppenum);
			void GetBindOptions(ref BIND_OPTS pbindopts);
			void GetObjectParam(string pszKey, [Out, MarshalAs(UnmanagedType.IUnknown)] out object ppunk);
			void GetRunningObjectTable([Out, MarshalAs(UnmanagedType.Interface)] out object pprot);
			void RegisterObjectBound([MarshalAs(UnmanagedType.IUnknown)] object punk);
			void RegisterObjectParam(string pszKey, [MarshalAs(UnmanagedType.IUnknown)] object punk);
			void ReleaseBoundObjects();
			void RevokeObjectBound([MarshalAs(UnmanagedType.IUnknown)] object punk);
			int RevokeObjectParam(string pszKey);
			void SetBindOptions(ref BIND_OPTS pbindopts);
		}

		[
			ComImport(),
			InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
			GuidAttribute("00000001-0000-0000-C000-000000000046"),
			]
		public interface IClassFactory
		{
			void CreateInstance(
				IntPtr pUnkOuter,
				ref Guid riid,
				out IntPtr ppvObject);
			void LockServer(bool fLock);
		}

		[ComImport(),
			Guid("C4D244B0-D43E-11CF-893B-00AA00BDCE1A"),
			InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IDocHostShowUI
		{
			[return: MarshalAs(UnmanagedType.I4)]
			[PreserveSig]
			int ShowMessage(
				[In] IntPtr hwnd, [In][MarshalAs(UnmanagedType.LPWStr)] String lpStrText,
				 [In][MarshalAs(UnmanagedType.LPWStr)] String lpstrCaption,
				 [In] uint dwType, [In][MarshalAs(UnmanagedType.LPWStr)] String
				  lpStrHelpFile, [In] uint dwHelpContext,
				   [Out] out uint lpresult);

			[return: MarshalAs(UnmanagedType.I4)]
			[PreserveSig]
			int ShowHelp(
				[In] IntPtr hwnd,
				[In][MarshalAs(UnmanagedType.LPWStr)] String lpHelpFile,
				[In] uint uCommand,
				[In] uint dwData,
				[In] POINT ptMouse,
				[Out][MarshalAs(UnmanagedType.IDispatch)] Object pDispatchObjectHit
				);
		}

		[ComImport(), Guid("BD3F23C0-D43E-11CF-893B-00AA00BDCE1A"), InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IDocHostUIHandler
		{

			[PreserveSig]
			int ShowContextMenu(
				[In, MarshalAs(UnmanagedType.U4)]        int dwID,
				[In] ref POINT pt,
				[In, MarshalAs(UnmanagedType.Interface)] object pcmdtReserved,
				[In, MarshalAs(UnmanagedType.Interface)] object pdispReserved);

			[PreserveSig]
			int GetHostInfo(
				[In, Out] DOCHOSTUIINFO info);

			[PreserveSig]
			int ShowUI(
				[In, MarshalAs(UnmanagedType.I4)]        int dwID,
				[In, MarshalAs(UnmanagedType.Interface)] IOleInPlaceActiveObject activeObject,
				[In, MarshalAs(UnmanagedType.Interface)] IOleCommandTarget commandTarget,
				[In, MarshalAs(UnmanagedType.Interface)] IOleInPlaceFrame frame,
				[In, MarshalAs(UnmanagedType.Interface)] IOleInPlaceUIWindow doc);

			[PreserveSig]
			int HideUI();

			[PreserveSig]
			int UpdateUI();

			[PreserveSig]
			int EnableModeless(
				[In, MarshalAs(UnmanagedType.Bool)] bool fEnable);

			[PreserveSig]
			int OnDocWindowActivate(
				[In, MarshalAs(UnmanagedType.Bool)] bool fActivate);

			[PreserveSig]
			int OnFrameWindowActivate(
				[In, MarshalAs(UnmanagedType.Bool)] bool fActivate);

			[PreserveSig]
			int ResizeBorder(
				[In] RECT rect,
				[In] IOleInPlaceUIWindow doc,
				[In] bool fFrameWindow);

			[PreserveSig]
			int TranslateAccelerator(
				[In]                               COMMSG msg,
				[In]                               ref Guid group,
				[In, MarshalAs(UnmanagedType.I4)] int nCmdID);

			[PreserveSig]
			int GetOptionKeyPath(
				[Out, MarshalAs(UnmanagedType.LPArray)] String[] pbstrKey,
				[In, MarshalAs(UnmanagedType.U4)]        int dw);

			[PreserveSig]
			int GetDropTarget(
				[In, MarshalAs(UnmanagedType.Interface)]   IOleDropTarget pDropTarget,
				[Out, MarshalAs(UnmanagedType.Interface)] out IOleDropTarget ppDropTarget);

			[PreserveSig]
			int GetExternal(
				[Out, MarshalAs(UnmanagedType.Interface)] out object ppDispatch);

			[PreserveSig]
			int TranslateUrl(
				[In, MarshalAs(UnmanagedType.U4)]       int dwTranslate,
				[In, MarshalAs(UnmanagedType.LPWStr)]   string strURLIn,
				[Out, MarshalAs(UnmanagedType.LPWStr)] out string pstrURLOut);

			[PreserveSig]
			int FilterDataObject(
				[In, MarshalAs(UnmanagedType.Interface)]   IOleDataObject pDO,
				[Out, MarshalAs(UnmanagedType.Interface)] out IOleDataObject ppDORet);
		}

		[Guid("3050F425-98B5-11CF-BB82-00AA00BDCE0B"), InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IElementBehavior
		{

			void Init(
				[In, MarshalAs(UnmanagedType.Interface)]
				IElementBehaviorSite pBehaviorSite);

			void Notify(
				[In, MarshalAs(UnmanagedType.U4)]
				int dwEvent,
				[In]
				IntPtr pVar);

			void Detach();
		}

		[ComImport()]
		[Guid("3050f6ba-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IElementBehaviorLayout
		{
			void GetSize([In] BEHAVIOR_LAYOUT_MODE dwFlags, [In] tagSIZE sizeContent, [In, Out] ref POINT pptTranslateB, [In, Out] ref POINT pptTopLeft, [In, Out] tagSIZE psizeProposed);

			[return: MarshalAs(UnmanagedType.I4)]
			int GetLayoutInfo();

			void GetPosition([In] BEHAVIOR_LAYOUT_MODE lFlags, [In, Out] ref POINT pptTopLeft);

			void MapSize([In, Out] tagSIZE pSizeIn, [Out] RECT prcOut);


			//void GetSize([In] long dwFlags, [In] tagSIZE sizeContent , [in, out] tagPOINT * pptTranslateBy , [in, out] tagPOINT * pptTopLeft , [in, out] tagSIZE * psizeProposed  );
			//void GetLayoutInfo([out, retval] long * plLayoutInfo );
			//void GetPosition([In] long lFlags, [in, out] tagPOINT * pptTopLeft  );
			//void MapSize([In] tagSIZE * psizeIn , [out] tagRECT * prcOut  );

		}


		[Guid("3050F429-98B5-11CF-BB82-00AA00BDCE0B"), InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IElementBehaviorFactory
		{

			[return: MarshalAs(UnmanagedType.Interface)]
			IElementBehavior FindBehavior(
				[In, MarshalAs(UnmanagedType.BStr)]
				string bstrBehavior,
				[In, MarshalAs(UnmanagedType.BStr)]
				string bstrBehaviorUrl,
				[In, Out, MarshalAs(UnmanagedType.Interface)]
				IElementBehaviorSite pSite);
		}

		[Guid("3050F427-98B5-11CF-BB82-00AA00BDCE0B"), InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IElementBehaviorSite
		{

			[return: MarshalAs(UnmanagedType.Interface)]
			IHTMLElement GetElement();


			void RegisterNotification(
				[In, MarshalAs(UnmanagedType.I4)]
				int lEvent);
		}

		// OM == 3050F489-98B5-11CF-BB82-00AA00BDCE0B
		[Guid("3050F659-98B5-11CF-BB82-00AA00BDCE0B"), InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IElementBehaviorSiteOM2
		{

			[return: MarshalAs(UnmanagedType.I4)]
			int RegisterEvent(
				[In, MarshalAs(UnmanagedType.BStr)]
				string pchEvent,
				[In, MarshalAs(UnmanagedType.I4)]
				int lFlags);

			[return: MarshalAs(UnmanagedType.I4)]
			int GetEventCookie(
				[In, MarshalAs(UnmanagedType.BStr)]
				string pchEvent);


			void FireEvent(
				[In, MarshalAs(UnmanagedType.I4)]
				int lCookie,
				[In, MarshalAs(UnmanagedType.Interface)]
				IHTMLEventObj pEventObject);

			[return: MarshalAs(UnmanagedType.Interface)]
			IHTMLEventObj CreateEventObject();

			void RegisterName(
				[In, MarshalAs(UnmanagedType.BStr)]
				string pchName);


			void RegisterUrn(
				[In, MarshalAs(UnmanagedType.BStr)]
				string pchUrn);
			//}

			//[Guid("3050F659-98B5-11CF-BB82-00AA00BDCE0B"), InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
			//public interface IElementBehaviorSiteOM2
			//{
			[return: MarshalAs(UnmanagedType.Interface)]
			IHTMLElementDefaults GetDefaults();
		}

		[Guid("3050F671-98B5-11CF-BB82-00AA00BDCE0B"), InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IElementNamespace
		{

			void AddTag(
				[In, MarshalAs(UnmanagedType.BStr)]
				string tagName,
				[In, MarshalAs(UnmanagedType.I4)]
				int lFlags);
		}

		[Guid("3050F672-98B5-11CF-BB82-00AA00BDCE0B"), InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IElementNamespaceFactory
		{

			void Create(
				[In, MarshalAs(UnmanagedType.Interface)]
				IElementNamespace pNamespace);
		}

		[Guid("3050F7FD-98B5-11CF-BB82-00AA00BDCE0B"), InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IElementNamespaceFactoryCallback
		{

			void Resolve(
				[In, MarshalAs(UnmanagedType.BStr)]
				string nameSpace,
				[In, MarshalAs(UnmanagedType.BStr)]
				string tagName,
				[In, MarshalAs(UnmanagedType.BStr)]
				string attributes,
				[In, MarshalAs(UnmanagedType.Interface)]
				IElementNamespace pNamespace);
		}

		[Guid("3050F670-98B5-11CF-BB82-00AA00BDCE0B"), InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IElementNamespaceTable
		{

			void AddNamespace(
				[In, MarshalAs(UnmanagedType.BStr)]
				string nameSpace,
				[In, MarshalAs(UnmanagedType.BStr)]
				string urn,
				[In, MarshalAs(UnmanagedType.I4)]
				int lFlags,
				[In]
				ref Object factory);
		}

		[ComImport(), Guid("00000103-0000-0000-C000-000000000046"), InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IEnumFORMATETC
		{

			[return: MarshalAs(UnmanagedType.I4)]
			[PreserveSig]
			int Next(
				[In, MarshalAs(UnmanagedType.U4)]
				int celt,
				[Out]
				FORMATETC rgelt,
				[In, Out, MarshalAs(UnmanagedType.LPArray)]
				int[] pceltFetched);

			[return: MarshalAs(UnmanagedType.I4)]
			[PreserveSig]
			int Skip(
				[In, MarshalAs(UnmanagedType.U4)]
				int celt);

			[return: MarshalAs(UnmanagedType.I4)]
			[PreserveSig]
			int Reset();

			[return: MarshalAs(UnmanagedType.I4)]
			[PreserveSig]
			int Clone(
				[Out, MarshalAs(UnmanagedType.LPArray)]
				IEnumFORMATETC[] ppenum);
		}

		/// <exclude/>
		[Guid("B3E7C340-EF97-11CE-9BC9-00AA00608E01"), InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IEnumOleUndoUnits
		{

			//[PreserveSig]
			void Next(
				[In, MarshalAs(UnmanagedType.U4)] int numDesired,
				[Out, MarshalAs(UnmanagedType.Interface)] out IOleUndoUnit unit,
				[Out, MarshalAs(UnmanagedType.U4)] out int numReceived);

			//void Bogus();

			//[PreserveSig]
			void Skip(
				[In, MarshalAs(UnmanagedType.I4)] int numToSkip);

			//[PreserveSig]
			void Reset();

			//[PreserveSig]
			void Clone(
				[Out, MarshalAs(UnmanagedType.Interface)] IEnumOleUndoUnits enumerator);
		};

		[Guid("00000000-0000-0000-C000-000000000046"),

			InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IUnknown
		{
		};

		[Guid("00000100-0000-0000-C000-000000000046"), InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IEnumUnknown
		{
			void Next(
				[In, MarshalAs(UnmanagedType.U4)] int celt,
				//[Out, MarshalAs(UnmanagedType.Interface)] out IUnknown pUnk,
				[Out] out IUnknown pUnk,
				[Out] out uint Dummy);
			void Skip([In, MarshalAs(UnmanagedType.U4)] int celt);
			void Reset();
			void Clone([Out] IEnumUnknown ppenum);
		}


		[ComImport(), Guid("00000104-0000-0000-C000-000000000046"), InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IEnumOLEVERB
		{
			[return: MarshalAs(UnmanagedType.I4)]
			[PreserveSig]
			int Next(
				[MarshalAs(UnmanagedType.U4)] int celt,
				[Out] tagOLEVERB rgelt,
				[Out, MarshalAs(UnmanagedType.LPArray)] int[] pceltFetched);

			[return: MarshalAs(UnmanagedType.I4)]
			[PreserveSig]
			int Skip([In, MarshalAs(UnmanagedType.U4)] int celt);
			void Reset();
			void Clone(out IEnumOLEVERB ppenum);
		}

		[Guid("00000105-0000-0000-C000-000000000046"), InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IEnumSTATDATA
		{
			void Next(
				[In, MarshalAs(UnmanagedType.U4)] int celt,
				[Out] STATDATA rgelt,
				[Out, MarshalAs(UnmanagedType.LPArray)] int[] pceltFetched);
			void Skip([In, MarshalAs(UnmanagedType.U4)] int celt);
			void Reset();
			void Clone([Out, MarshalAs(UnmanagedType.LPArray)] IEnumSTATDATA[] ppenum);
		}

		[ComImport()]
		[Guid("3050f1d8-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		public interface IHtmlBodyElement
		{
			[DispId(DispId.IHTMLBODYELEMENT_CREATETEXTRANGE)]
			IHTMLTxtRange createTextRange();

			String background
			{
				[DispId(DispId.IHTMLBODYELEMENT_BACKGROUND)]
				get;
				[DispId(DispId.IHTMLBODYELEMENT_BACKGROUND)]
				set;
			}
			Object leftMargin
			{
				[DispId(DispId.IHTMLBODYELEMENT_LEFTMARGIN)]
				get;
				[DispId(DispId.IHTMLBODYELEMENT_LEFTMARGIN)]
				set;
			}
			String bgProperties
			{
				[DispId(DispId.IHTMLBODYELEMENT_BGPROPERTIES)]
				get;
				[DispId(DispId.IHTMLBODYELEMENT_BGPROPERTIES)]
				set;
			}
			Object onload
			{
				[DispId(DispId.IHTMLBODYELEMENT_ONLOAD)]
				get;
				[DispId(DispId.IHTMLBODYELEMENT_ONLOAD)]
				set;
			}
			String scroll
			{
				[DispId(DispId.IHTMLBODYELEMENT_SCROLL)]
				get;
				[DispId(DispId.IHTMLBODYELEMENT_SCROLL)]
				set;
			}
			Object onunload
			{
				[DispId(DispId.IHTMLBODYELEMENT_ONUNLOAD)]
				get;
				[DispId(DispId.IHTMLBODYELEMENT_ONUNLOAD)]
				set;
			}
			Boolean noWrap
			{
				[DispId(DispId.IHTMLBODYELEMENT_NOWRAP)]
				get;
				[DispId(DispId.IHTMLBODYELEMENT_NOWRAP)]
				set;
			}
			Object text
			{
				[DispId(DispId.IHTMLBODYELEMENT_TEXT)]
				get;
				[DispId(DispId.IHTMLBODYELEMENT_TEXT)]
				set;
			}
			Object rightMargin
			{
				[DispId(DispId.IHTMLBODYELEMENT_RIGHTMARGIN)]
				get;
				[DispId(DispId.IHTMLBODYELEMENT_RIGHTMARGIN)]
				set;
			}
			Object vLink
			{
				[DispId(DispId.IHTMLBODYELEMENT_VLINK)]
				get;
				[DispId(DispId.IHTMLBODYELEMENT_VLINK)]
				set;
			}
			Object onselect
			{
				[DispId(DispId.IHTMLBODYELEMENT_ONSELECT)]
				get;
				[DispId(DispId.IHTMLBODYELEMENT_ONSELECT)]
				set;
			}
			Object onbeforeunload
			{
				[DispId(DispId.IHTMLBODYELEMENT_ONBEFOREUNLOAD)]
				get;
				[DispId(DispId.IHTMLBODYELEMENT_ONBEFOREUNLOAD)]
				set;
			}
			Object aLink
			{
				[DispId(DispId.IHTMLBODYELEMENT_ALINK)]
				get;
				[DispId(DispId.IHTMLBODYELEMENT_ALINK)]
				set;
			}
			Object link
			{
				[DispId(DispId.IHTMLBODYELEMENT_LINK)]
				get;
				[DispId(DispId.IHTMLBODYELEMENT_LINK)]
				set;
			}
			Object topMargin
			{
				[DispId(DispId.IHTMLBODYELEMENT_TOPMARGIN)]
				get;
				[DispId(DispId.IHTMLBODYELEMENT_TOPMARGIN)]
				set;
			}
			Object bottomMargin
			{
				[DispId(DispId.IHTMLBODYELEMENT_BOTTOMMARGIN)]
				get;
				[DispId(DispId.IHTMLBODYELEMENT_BOTTOMMARGIN)]
				set;
			}
			Object bgColor
			{
				[DispId(DispId.IHTMLBODYELEMENT_BGCOLOR)]
				get;
				[DispId(DispId.IHTMLBODYELEMENT_BGCOLOR)]
				set;
			}
		}


		[ComImport()]
		[Guid("3050f3db-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		public interface IHTMLCurrentStyle
		{
			[DispId(DispId.IHTMLCURRENTSTYLE_GETATTRIBUTE)]
			Object getAttribute(String strAttributeName, Int32 lFlags);

			String wordBreak
			{
				[DispId(DispId.IHTMLCURRENTSTYLE_WORDBREAK)]
				get;
			}
			Object borderTopWidth
			{
				[DispId(DispId.IHTMLCURRENTSTYLE_BORDERTOPWIDTH)]
				get;
			}
			Object backgroundPositionY
			{
				[DispId(DispId.IHTMLCURRENTSTYLE_BACKGROUNDPOSITIONY)]
				get;
			}
			String position
			{
				[DispId(DispId.IHTMLCURRENTSTYLE_POSITION)]
				get;
			}
			Object borderTopColor
			{
				[DispId(DispId.IHTMLCURRENTSTYLE_BORDERTOPCOLOR)]
				get;
			}
			String styleFloat
			{
				[DispId(DispId.IHTMLCURRENTSTYLE_STYLEFLOAT)]
				get;
			}
			String rubyPosition
			{
				[DispId(DispId.IHTMLCURRENTSTYLE_RUBYPOSITION)]
				get;
			}
			String layoutGridType
			{
				[DispId(DispId.IHTMLCURRENTSTYLE_LAYOUTGRIDTYPE)]
				get;
			}
			String borderTopStyle
			{
				[DispId(DispId.IHTMLCURRENTSTYLE_BORDERTOPSTYLE)]
				get;
			}
			Object layoutGridLine
			{
				[DispId(DispId.IHTMLCURRENTSTYLE_LAYOUTGRIDLINE)]
				get;
			}
			String backgroundImage
			{
				[DispId(DispId.IHTMLCURRENTSTYLE_BACKGROUNDIMAGE)]
				get;
			}
			Object top
			{
				[DispId(DispId.IHTMLCURRENTSTYLE_TOP)]
				get;
			}
			String backgroundRepeat
			{
				[DispId(DispId.IHTMLCURRENTSTYLE_BACKGROUNDREPEAT)]
				get;
			}
			Object fontWeight
			{
				[DispId(DispId.IHTMLCURRENTSTYLE_FONTWEIGHT)]
				get;
			}
			String listStyleImage
			{
				[DispId(DispId.IHTMLCURRENTSTYLE_LISTSTYLEIMAGE)]
				get;
			}
			Object paddingTop
			{
				[DispId(DispId.IHTMLCURRENTSTYLE_PADDINGTOP)]
				get;
			}
			Object paddingBottom
			{
				[DispId(DispId.IHTMLCURRENTSTYLE_PADDINGBOTTOM)]
				get;
			}
			Object borderRightColor
			{
				[DispId(DispId.IHTMLCURRENTSTYLE_BORDERRIGHTCOLOR)]
				get;
			}
			Object zIndex
			{
				[DispId(DispId.IHTMLCURRENTSTYLE_ZINDEX)]
				get;
			}
			Object letterSpacing
			{
				[DispId(DispId.IHTMLCURRENTSTYLE_LETTERSPACING)]
				get;
			}
			String cursor
			{
				[DispId(DispId.IHTMLCURRENTSTYLE_CURSOR)]
				get;
			}
			Object borderBottomColor
			{
				[DispId(DispId.IHTMLCURRENTSTYLE_BORDERBOTTOMCOLOR)]
				get;
			}
			Object lineHeight
			{
				[DispId(DispId.IHTMLCURRENTSTYLE_LINEHEIGHT)]
				get;
			}
			Object clipTop
			{
				[DispId(DispId.IHTMLCURRENTSTYLE_CLIPTOP)]
				get;
			}
			String textAlign
			{
				[DispId(DispId.IHTMLCURRENTSTYLE_TEXTALIGN)]
				get;
			}
			String textTransform
			{
				[DispId(DispId.IHTMLCURRENTSTYLE_TEXTTRANSFORM)]
				get;
			}
			String borderCollapse
			{
				[DispId(DispId.IHTMLCURRENTSTYLE_BORDERCOLLAPSE)]
				get;
			}
			Object clipLeft
			{
				[DispId(DispId.IHTMLCURRENTSTYLE_CLIPLEFT)]
				get;
			}
			String direction
			{
				[DispId(DispId.IHTMLCURRENTSTYLE_DIRECTION)]
				get;
			}
			Object textIndent
			{
				[DispId(DispId.IHTMLCURRENTSTYLE_TEXTINDENT)]
				get;
			}
			Object borderLeftWidth
			{
				[DispId(DispId.IHTMLCURRENTSTYLE_BORDERLEFTWIDTH)]
				get;
			}
			Object borderRightWidth
			{
				[DispId(DispId.IHTMLCURRENTSTYLE_BORDERRIGHTWIDTH)]
				get;
			}
			Object fontSize
			{
				[DispId(DispId.IHTMLCURRENTSTYLE_FONTSIZE)]
				get;
			}
			String fontFamily
			{
				[DispId(DispId.IHTMLCURRENTSTYLE_FONTFAMILY)]
				get;
			}
			String rubyAlign
			{
				[DispId(DispId.IHTMLCURRENTSTYLE_RUBYALIGN)]
				get;
			}
			Object clipRight
			{
				[DispId(DispId.IHTMLCURRENTSTYLE_CLIPRIGHT)]
				get;
			}
			String layoutGridMode
			{
				[DispId(DispId.IHTMLCURRENTSTYLE_LAYOUTGRIDMODE)]
				get;
			}
			Object textKashida
			{
				[DispId(DispId.IHTMLCURRENTSTYLE_TEXTKASHIDA)]
				get;
			}
			String display
			{
				[DispId(DispId.IHTMLCURRENTSTYLE_DISPLAY)]
				get;
			}
			String listStylePosition
			{
				[DispId(DispId.IHTMLCURRENTSTYLE_LISTSTYLEPOSITION)]
				get;
			}
			String textDecoration
			{
				[DispId(DispId.IHTMLCURRENTSTYLE_TEXTDECORATION)]
				get;
			}
			Object borderBottomWidth
			{
				[DispId(DispId.IHTMLCURRENTSTYLE_BORDERBOTTOMWIDTH)]
				get;
			}
			String borderBottomStyle
			{
				[DispId(DispId.IHTMLCURRENTSTYLE_BORDERBOTTOMSTYLE)]
				get;
			}
			String overflowY
			{
				[DispId(DispId.IHTMLCURRENTSTYLE_OVERFLOWY)]
				get;
			}
			String borderColor
			{
				[DispId(DispId.IHTMLCURRENTSTYLE_BORDERCOLOR)]
				get;
			}
			String lineBreak
			{
				[DispId(DispId.IHTMLCURRENTSTYLE_LINEBREAK)]
				get;
			}
			String borderWidth
			{
				[DispId(DispId.IHTMLCURRENTSTYLE_BORDERWIDTH)]
				get;
			}
			Object marginBottom
			{
				[DispId(DispId.IHTMLCURRENTSTYLE_MARGINBOTTOM)]
				get;
			}
			Object height
			{
				[DispId(DispId.IHTMLCURRENTSTYLE_HEIGHT)]
				get;
			}
			String behavior
			{
				[DispId(DispId.IHTMLCURRENTSTYLE_BEHAVIOR)]
				get;
			}
			String pageBreakBefore
			{
				[DispId(DispId.IHTMLCURRENTSTYLE_PAGEBREAKBEFORE)]
				get;
			}
			String clear
			{
				[DispId(DispId.IHTMLCURRENTSTYLE_CLEAR)]
				get;
			}
			String fontStyle
			{
				[DispId(DispId.IHTMLCURRENTSTYLE_FONTSTYLE)]
				get;
			}
			String overflowX
			{
				[DispId(DispId.IHTMLCURRENTSTYLE_OVERFLOWX)]
				get;
			}
			String borderLeftStyle
			{
				[DispId(DispId.IHTMLCURRENTSTYLE_BORDERLEFTSTYLE)]
				get;
			}
			Object borderLeftColor
			{
				[DispId(DispId.IHTMLCURRENTSTYLE_BORDERLEFTCOLOR)]
				get;
			}
			Object width
			{
				[DispId(DispId.IHTMLCURRENTSTYLE_WIDTH)]
				get;
			}
			String margin
			{
				[DispId(DispId.IHTMLCURRENTSTYLE_MARGIN)]
				get;
			}
			Object bottom
			{
				[DispId(DispId.IHTMLCURRENTSTYLE_BOTTOM)]
				get;
			}
			String imeMode
			{
				[DispId(DispId.IHTMLCURRENTSTYLE_IMEMODE)]
				get;
			}
			String tableLayout
			{
				[DispId(DispId.IHTMLCURRENTSTYLE_TABLELAYOUT)]
				get;
			}
			Object backgroundColor
			{
				[DispId(DispId.IHTMLCURRENTSTYLE_BACKGROUNDCOLOR)]
				get;
			}
			String borderStyle
			{
				[DispId(DispId.IHTMLCURRENTSTYLE_BORDERSTYLE)]
				get;
			}
			String borderRightStyle
			{
				[DispId(DispId.IHTMLCURRENTSTYLE_BORDERRIGHTSTYLE)]
				get;
			}
			String backgroundAttachment
			{
				[DispId(DispId.IHTMLCURRENTSTYLE_BACKGROUNDATTACHMENT)]
				get;
			}
			Object marginTop
			{
				[DispId(DispId.IHTMLCURRENTSTYLE_MARGINTOP)]
				get;
			}
			Object paddingLeft
			{
				[DispId(DispId.IHTMLCURRENTSTYLE_PADDINGLEFT)]
				get;
			}
			Object marginRight
			{
				[DispId(DispId.IHTMLCURRENTSTYLE_MARGINRIGHT)]
				get;
			}
			Object backgroundPositionX
			{
				[DispId(DispId.IHTMLCURRENTSTYLE_BACKGROUNDPOSITIONX)]
				get;
			}
			String visibility
			{
				[DispId(DispId.IHTMLCURRENTSTYLE_VISIBILITY)]
				get;
			}
			Object verticalAlign
			{
				[DispId(DispId.IHTMLCURRENTSTYLE_VERTICALALIGN)]
				get;
			}
			String padding
			{
				[DispId(DispId.IHTMLCURRENTSTYLE_PADDING)]
				get;
			}
			Object color
			{
				[DispId(DispId.IHTMLCURRENTSTYLE_COLOR)]
				get;
			}
			Object paddingRight
			{
				[DispId(DispId.IHTMLCURRENTSTYLE_PADDINGRIGHT)]
				get;
			}
			String textAutospace
			{
				[DispId(DispId.IHTMLCURRENTSTYLE_TEXTAUTOSPACE)]
				get;
			}
			String rubyOverhang
			{
				[DispId(DispId.IHTMLCURRENTSTYLE_RUBYOVERHANG)]
				get;
			}
			Object layoutGridChar
			{
				[DispId(DispId.IHTMLCURRENTSTYLE_LAYOUTGRIDCHAR)]
				get;
			}
			String blockDirection
			{
				[DispId(DispId.IHTMLCURRENTSTYLE_BLOCKDIRECTION)]
				get;
			}
			String textJustifyTrim
			{
				[DispId(DispId.IHTMLCURRENTSTYLE_TEXTJUSTIFYTRIM)]
				get;
			}
			String fontVariant
			{
				[DispId(DispId.IHTMLCURRENTSTYLE_FONTVARIANT)]
				get;
			}
			String pageBreakAfter
			{
				[DispId(DispId.IHTMLCURRENTSTYLE_PAGEBREAKAFTER)]
				get;
			}
			String textJustify
			{
				[DispId(DispId.IHTMLCURRENTSTYLE_TEXTJUSTIFY)]
				get;
			}
			Object clipBottom
			{
				[DispId(DispId.IHTMLCURRENTSTYLE_CLIPBOTTOM)]
				get;
			}
			String overflow
			{
				[DispId(DispId.IHTMLCURRENTSTYLE_OVERFLOW)]
				get;
			}
			Object marginLeft
			{
				[DispId(DispId.IHTMLCURRENTSTYLE_MARGINLEFT)]
				get;
			}
			Object left
			{
				[DispId(DispId.IHTMLCURRENTSTYLE_LEFT)]
				get;
			}
			String unicodeBidi
			{
				[DispId(DispId.IHTMLCURRENTSTYLE_UNICODEBIDI)]
				get;
			}
			Object right
			{
				[DispId(DispId.IHTMLCURRENTSTYLE_RIGHT)]
				get;
			}
			String listStyleType
			{
				[DispId(DispId.IHTMLCURRENTSTYLE_LISTSTYLETYPE)]
				get;
			}
			String accelerator
			{
				[DispId(DispId.IHTMLCURRENTSTYLE_ACCELERATOR)]
				get;
			}
		}

		#region HTMLDocument interfaces and classes
		[ComImport(), Guid("25336920-03F9-11CF-8FD0-00AA00686F13")]
		public class HTMLDocument
		{
		}


		[Guid("626FC520-A41E-11CF-A731-00A0C9082637"), InterfaceTypeAttribute(ComInterfaceType.InterfaceIsDual)]
		public interface IHTMLDocument
		{

			[return: MarshalAs(UnmanagedType.Interface)]
			object GetScript();
		}

		[Guid("332C4425-26CB-11D0-B483-00C04FD90119"), InterfaceTypeAttribute(ComInterfaceType.InterfaceIsDual)]
		public interface IHTMLDocument2
		{

			[return: MarshalAs(UnmanagedType.IDispatch)]
			object GetScript();

			[return: MarshalAs(UnmanagedType.Interface)]
			IHTMLElementCollection GetAll();

			[return: MarshalAs(UnmanagedType.Interface)]
			IHTMLElement GetBody();

			[return: MarshalAs(UnmanagedType.Interface)]
			IHTMLElement GetActiveElement();

			[return: MarshalAs(UnmanagedType.Interface)]
			IHTMLElementCollection GetImages();

			[return: MarshalAs(UnmanagedType.Interface)]
			IHTMLElementCollection GetApplets();

			[return: MarshalAs(UnmanagedType.Interface)]
			IHTMLElementCollection GetLinks();

			[return: MarshalAs(UnmanagedType.Interface)]
			IHTMLElementCollection GetForms();

			[return: MarshalAs(UnmanagedType.Interface)]
			IHTMLElementCollection GetAnchors();


			void SetTitle(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetTitle();

			[return: MarshalAs(UnmanagedType.Interface)]
			IHTMLElementCollection GetScripts();


			void SetDesignMode(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetDesignMode();

			[return: MarshalAs(UnmanagedType.Interface)]
			IHTMLSelectionObject GetSelection();

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetReadyState();

			[return: MarshalAs(UnmanagedType.Interface)]
			object GetFrames();

			[return: MarshalAs(UnmanagedType.Interface)]
			IHTMLElementCollection GetEmbeds();

			[return: MarshalAs(UnmanagedType.Interface)]
			IHTMLElementCollection GetPlugins();

			void SetAlinkColor(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetAlinkColor();

			void SetBgColor(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetBgColor();

			void SetFgColor(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetFgColor();

			void SetLinkColor(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetLinkColor();

			void SetVlinkColor(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetVlinkColor();

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetReferrer();

			[return: MarshalAs(UnmanagedType.Interface)]
			object GetLocation();

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetLastModified();

			void SetURL(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetURL();

			void SetDomain(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetDomain();

			void SetCookie(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetCookie();

			void SetExpando(
				[In, MarshalAs(UnmanagedType.Bool)]
				bool p);

			[return: MarshalAs(UnmanagedType.Bool)]
			bool GetExpando();

			void SetCharset(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetCharset();

			void SetDefaultCharset(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetDefaultCharset();

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetMimeType();

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetFileSize();

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetFileCreatedDate();

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetFileModifiedDate();

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetFileUpdatedDate();

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetSecurity();

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetProtocol();

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetNameProp();

			void Write(
				[In, MarshalAs(UnmanagedType.BStr)]
				string psarray);

			void Writeln(
				[In, MarshalAs(UnmanagedType.BStr)]
				string psarray);

			[return: MarshalAs(UnmanagedType.Interface)]
			object Open(
				[In, MarshalAs(UnmanagedType.BStr)]
				string URL,
				[In, MarshalAs(UnmanagedType.Struct)]
				Object name,
				[In, MarshalAs(UnmanagedType.Struct)]
				Object features,
				[In, MarshalAs(UnmanagedType.Struct)]
				Object replace);

			void Close();

			void Clear();

			[return: MarshalAs(UnmanagedType.Bool)]
			bool QueryCommandSupported(
				[In, MarshalAs(UnmanagedType.BStr)]
				string cmdID);

			[return: MarshalAs(UnmanagedType.Bool)]
			bool QueryCommandEnabled(
				[In, MarshalAs(UnmanagedType.BStr)]
				string cmdID);

			[return: MarshalAs(UnmanagedType.Bool)]
			bool QueryCommandState(
				[In, MarshalAs(UnmanagedType.BStr)]
				string cmdID);

			[return: MarshalAs(UnmanagedType.Bool)]
			bool QueryCommandIndeterm(
				[In, MarshalAs(UnmanagedType.BStr)]
				string cmdID);

			[return: MarshalAs(UnmanagedType.BStr)]
			string QueryCommandText(
				[In, MarshalAs(UnmanagedType.BStr)]
				string cmdID);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object QueryCommandValue(
				[In, MarshalAs(UnmanagedType.BStr)]
				string cmdID);

			[return: MarshalAs(UnmanagedType.Bool)]
			bool ExecCommand(
				[In, MarshalAs(UnmanagedType.BStr)]
				string cmdID,
				[In, MarshalAs(UnmanagedType.Bool)]
				bool showUI,
				[In, MarshalAs(UnmanagedType.Struct)]
				Object value);

			[return: MarshalAs(UnmanagedType.Bool)]
			bool ExecCommandShowHelp(
				[In, MarshalAs(UnmanagedType.BStr)]
				string cmdID);

			[return: MarshalAs(UnmanagedType.Interface)]
			IHTMLElement CreateElement(
				[In, MarshalAs(UnmanagedType.BStr)]
				string eTag);

			void SetOnhelp(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetOnhelp();

			void SetOnclick(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetOnclick();

			void SetOndblclick(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetOndblclick();


			void SetOnkeyup(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetOnkeyup();

			void SetOnkeydown(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetOnkeydown();

			void SetOnkeypress(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetOnkeypress();

			void SetOnmouseup(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetOnmouseup();

			void SetOnmousedown(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetOnmousedown();

			void SetOnmousemove(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetOnmousemove();

			void SetOnmouseout(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetOnmouseout();

			void SetOnmouseover(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetOnmouseover();

			void SetOnreadystatechange(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetOnreadystatechange();

			void SetOnafterupdate(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetOnafterupdate();

			void SetOnrowexit(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetOnrowexit();

			void SetOnrowenter(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetOnrowenter();

			int SetOndragstart(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetOndragstart();

			void SetOnselectstart(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetOnselectstart();

			[return: MarshalAs(UnmanagedType.Interface)]
			IHTMLElement ElementFromPoint(
				[In, MarshalAs(UnmanagedType.I4)]
				int x,
				[In, MarshalAs(UnmanagedType.I4)]
				int y);

			[return: MarshalAs(UnmanagedType.Interface)]
			IHTMLWindow2 GetParentWindow();

			[return: MarshalAs(UnmanagedType.Interface)]
			IHTMLStyleSheetsCollection GetStyleSheets();

			void SetOnbeforeupdate(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetOnbeforeupdate();

			void SetOnerrorupdate(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetOnerrorupdate();

			[return: MarshalAs(UnmanagedType.BStr)]
			string toString();

			[return: MarshalAs(UnmanagedType.Interface)]
			IHTMLStyleSheet CreateStyleSheet(
				[In, MarshalAs(UnmanagedType.BStr)]
				string bstrHref,
				[In, MarshalAs(UnmanagedType.I4)]
				int lIndex);
		}

		[ComImport()]
		[Guid("3050f485-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		public interface IHTMLDocument3
		{
			[DispId(DispId.IHTMLDOCUMENT3_RELEASECAPTURE)]
			void ReleaseCapture();

			[DispId(DispId.IHTMLDOCUMENT3_RECALC)]
			void Recalc(Boolean fForce);

			[DispId(DispId.IHTMLDOCUMENT3_CREATETEXTNODE)]
			IHTMLDOMNode CreateTextNode(String text);

			[DispId(DispId.IHTMLDOCUMENT3_ATTACHEVENT)]
			Boolean AttachEvent(String @event, Object pDisp);

			[DispId(DispId.IHTMLDOCUMENT3_DETACHEVENT)]
			void DetachEvent(String @event, Object pDisp);

			[DispId(DispId.IHTMLDOCUMENT3_CREATEDOCUMENTFRAGMENT)]
			IHTMLDocument2 CreateDocumentFragment();

			[DispId(DispId.IHTMLDOCUMENT3_GETELEMENTSBYNAME)]
			IHTMLElementCollection GetElementsByName(String name);

			[DispId(DispId.IHTMLDOCUMENT3_GETELEMENTBYID)]
			IHTMLElement GetElementById(String v);

			[DispId(DispId.IHTMLDOCUMENT3_GETELEMENTSBYTAGNAME)]
			IHTMLElementCollection GetElementsByTagName(String tagName);

			Object onrowsinserted
			{
				[DispId(DispId.IHTMLDOCUMENT3_ONROWSINSERTED)]
				get;
				[DispId(DispId.IHTMLDOCUMENT3_ONROWSINSERTED)]
				set;
			}
			Object ondatasetchanged
			{
				[DispId(DispId.IHTMLDOCUMENT3_ONDATASETCHANGED)]
				get;
				[DispId(DispId.IHTMLDOCUMENT3_ONDATASETCHANGED)]
				set;
			}
			Object ondataavailable
			{
				[DispId(DispId.IHTMLDOCUMENT3_ONDATAAVAILABLE)]
				get;
				[DispId(DispId.IHTMLDOCUMENT3_ONDATAAVAILABLE)]
				set;
			}
			Object ondatasetcomplete
			{
				[DispId(DispId.IHTMLDOCUMENT3_ONDATASETCOMPLETE)]
				get;
				[DispId(DispId.IHTMLDOCUMENT3_ONDATASETCOMPLETE)]
				set;
			}
			Object onpropertychange
			{
				[DispId(DispId.IHTMLDOCUMENT3_ONPROPERTYCHANGE)]
				get;
				[DispId(DispId.IHTMLDOCUMENT3_ONPROPERTYCHANGE)]
				set;
			}

			[DispId(DispId.IHTMLDOCUMENT3_CHILDNODES)]
			Object GetChildNodes();

			String dir
			{
				[DispId(DispId.IHTMLDOCUMENT3_DIR)]
				get;
				[DispId(DispId.IHTMLDOCUMENT3_DIR)]
				set;
			}
			Boolean inheritStyleSheets
			{
				[DispId(DispId.IHTMLDOCUMENT3_INHERITSTYLESHEETS)]
				get;
				[DispId(DispId.IHTMLDOCUMENT3_INHERITSTYLESHEETS)]
				set;
			}
			Object onrowsdelete
			{
				[DispId(DispId.IHTMLDOCUMENT3_ONROWSDELETE)]
				get;
				[DispId(DispId.IHTMLDOCUMENT3_ONROWSDELETE)]
				set;
			}
			Boolean enableDownload
			{
				[DispId(DispId.IHTMLDOCUMENT3_ENABLEDOWNLOAD)]
				get;
				[DispId(DispId.IHTMLDOCUMENT3_ENABLEDOWNLOAD)]
				set;
			}
			Object onstop
			{
				[DispId(DispId.IHTMLDOCUMENT3_ONSTOP)]
				get;
				[DispId(DispId.IHTMLDOCUMENT3_ONSTOP)]
				set;
			}
			IHTMLDocument2 parentDocument
			{
				[DispId(DispId.IHTMLDOCUMENT3_PARENTDOCUMENT)]
				get;
			}
			Object oncellchange
			{
				[DispId(DispId.IHTMLDOCUMENT3_ONCELLCHANGE)]
				get;
				[DispId(DispId.IHTMLDOCUMENT3_ONCELLCHANGE)]
				set;
			}
			Object onbeforeeditfocus
			{
				[DispId(DispId.IHTMLDOCUMENT3_ONBEFOREEDITFOCUS)]
				get;
				[DispId(DispId.IHTMLDOCUMENT3_ONBEFOREEDITFOCUS)]
				set;
			}

			[DispId(DispId.IHTMLDOCUMENT3_DOCUMENTELEMENT)]
			IHTMLElement GetDocumentElement();

			[DispId(DispId.IHTMLDOCUMENT3_UNIQUEID)]
			String GetUniqueID();

			Object oncontextmenu
			{
				[DispId(DispId.IHTMLDOCUMENT3_ONCONTEXTMENU)]
				get;
				[DispId(DispId.IHTMLDOCUMENT3_ONCONTEXTMENU)]
				set;
			}
			[DispId(DispId.IHTMLDOCUMENT3_BASEURL)]
			String GetBaseUrl();
			[DispId(DispId.IHTMLDOCUMENT3_BASEURL)]
			void SetBaseUrl(string baseUrl);

		}

		[ComImport()]
		[Guid("3050f69a-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		public interface IHTMLDocument4
		{
			[DispId(DispId.IHTMLDOCUMENT4_FOCUS)]
			void focus();

			[DispId(DispId.IHTMLDOCUMENT4_HASFOCUS)]
			Boolean hasFocus();

			[DispId(DispId.IHTMLDOCUMENT4_CREATEDOCUMENTFROMURL)]
			IHTMLDocument2 createDocumentFromUrl(String bstrUrl, String bstrOptions);

			[DispId(DispId.IHTMLDOCUMENT4_CREATEEVENTOBJECT)]
			IHTMLEventObj createEventObject(Object pvarEventObject);

			[DispId(DispId.IHTMLDOCUMENT4_FIREEVENT)]
			Boolean fireEvent(String bstrEventName, Object pvarEventObject);

			[DispId(DispId.IHTMLDOCUMENT4_CREATERENDERSTYLE)]
			IHTMLRenderStyle createRenderStyle(String v);

			String media
			{
				[DispId(DispId.IHTMLDOCUMENT4_MEDIA)]
				get;
				[DispId(DispId.IHTMLDOCUMENT4_MEDIA)]
				set;
			}
			String URLUnencoded
			{
				[DispId(DispId.IHTMLDOCUMENT4_URLUNENCODED)]
				get;
			}
			Object namespaces
			{
				[DispId(DispId.IHTMLDOCUMENT4_NAMESPACES)]
				get;
			}
			Object onselectionchange
			{
				[DispId(DispId.IHTMLDOCUMENT4_ONSELECTIONCHANGE)]
				get;
				[DispId(DispId.IHTMLDOCUMENT4_ONSELECTIONCHANGE)]
				set;
			}
			Object oncontrolselect
			{
				[DispId(DispId.IHTMLDOCUMENT4_ONCONTROLSELECT)]
				get;
				[DispId(DispId.IHTMLDOCUMENT4_ONCONTROLSELECT)]
				set;
			}
		}

		#endregion

		[Guid("3050f662-98b5-11cf-bb82-00aa00bdce0b"), InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IHTMLEditDesigner
		{
			[return: MarshalAs(UnmanagedType.I4)]
			[PreserveSig]
			int PreHandleEvent(
				[In] int dispId,
				[In, MarshalAs(UnmanagedType.Interface)]
				IHTMLEventObj eventObj
				);

			[return: MarshalAs(UnmanagedType.I4)]
			[PreserveSig]
			int PostHandleEvent(
				[In] int dispId,
				[In, MarshalAs(UnmanagedType.Interface)]
				IHTMLEventObj eventObj
				);

			[return: MarshalAs(UnmanagedType.I4)]
			[PreserveSig]
			int TranslateAccelerator(
				[In] int dispId,
				[In, MarshalAs(UnmanagedType.Interface)]
				IHTMLEventObj eventObj
				);

			[return: MarshalAs(UnmanagedType.I4)]
			[PreserveSig]
			int PostEditorEventNotify(
				[In] int dispId,
				[In, MarshalAs(UnmanagedType.Interface)]
				IHTMLEventObj eventObj
				);

		}

		[Guid("3050f6a0-98b5-11cf-bb82-00aa00bdce0b"), InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IHTMLEditHost
		{

			void SnapRect(
				[In, MarshalAs(UnmanagedType.Interface)]
				IHTMLElement pElement,
				[In, Out]
				RECT rcNew,
				[In, MarshalAs(UnmanagedType.I4)]
				int nHandle);
		}

		[Guid("3050f663-98b5-11cf-bb82-00aa00bdce0b"), InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IHTMLEditServices
		{

			[return: MarshalAs(UnmanagedType.I4)]
			int AddDesigner(
				[In, MarshalAs(UnmanagedType.Interface)]
				IHTMLEditDesigner designer);

			[return: MarshalAs(UnmanagedType.Interface)]
			int GetSelectionServices(
			   [In, MarshalAs(UnmanagedType.Interface)]
				IMarkupContainer markupContainer,
			   [Out, MarshalAs(UnmanagedType.Interface)]
				out IntPtr ss);

			[return: MarshalAs(UnmanagedType.I4)]
			int MoveToSelectionAnchor(
				[In, MarshalAs(UnmanagedType.Interface)]
				object markupPointer);

			[return: MarshalAs(UnmanagedType.I4)]
			int MoveToSelectionEnd(
				[In, MarshalAs(UnmanagedType.Interface)]
				object markupPointer);

			[return: MarshalAs(UnmanagedType.I4)]
			int RemoveDesigner(
				[In, MarshalAs(UnmanagedType.Interface)]
				IHTMLEditDesigner designer);
		}

		[ComImport()]
		[Guid("3050f684-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		public interface ISelectionServices
		{
			void SetSelectionType(SELECTION_TYPE eType, ISelectionServicesListener pIListener);

			void GetMarkupContainer(out IMarkupContainer ppIContainer);

			void AddSegment(IMarkupPointer pIStart, IMarkupPointer pIEnd, out ISegment ppISegmentAdded);

			void AddElementSegment(IHTMLElement pIElement, out IElementSegment ppISegmentAdded);

			void RemoveSegment(ISegment pISegment);

			void GetSelectionServicesListener(out ISelectionServicesListener ppISelectionServicesListener);

		}

		[ComImport()]
		[Guid("3050f68f-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IElementSegment
		{
			void GetElement(out IHTMLElement ppIElement);

			void SetPrimary(Int32 fPrimary);

			void IsPrimary(out Int32 pfPrimary);

		}

		[ComImport()]
		[Guid("3050f699-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		public interface ISelectionServicesListener
		{
			void BeginSelectionUndo();

			void EndSelectionUndo();

			void OnSelectedElementExit(IMarkupPointer pIElementStart, IMarkupPointer pIElementEnd, IMarkupPointer pIElementContentStart, IMarkupPointer pIElementContentEnd);

			void OnChangeType(SELECTION_TYPE eType, ISelectionServicesListener pIListener);

			void GetTypeDetail(out String pTypeDetail);

		}

		[ComImport()]
		[Guid("3050f683-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		public interface ISegment
		{
			void GetPointers(IMarkupPointer pIStart, IMarkupPointer pIEnd);

		}

		[Guid("3050F605-98B5-11CF-BB82-00AA00BDCE0B")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		public interface ISegmentList
		{
			void CreateIterator([Out] out ISegmentListIterator ppIIter);
			void GetType([Out] SELECTION_TYPE peType);
			void IsEmpty([Out] out int pfEmpty);
		}

		[Guid("3050F692-98B5-11CF-BB82-00AA00BDCE0B")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		public interface ISegmentListIterator
		{
			void Current([Out] out ISegment ppISegment);
			void First();
			void IsDone();
			void Advance();
		}

		#region HTMLElement interfaces

		[ComImport()]
		[Guid("3050f20c-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		public interface IHTMLCommentElement
		{
			String text
			{
				[DispId(DispId.IHTMLCOMMENTELEMENT_TEXT)]
				get;
				[DispId(DispId.IHTMLCOMMENTELEMENT_TEXT)]
				set;
			}
			Int32 atomic
			{
				[DispId(DispId.IHTMLCOMMENTELEMENT_ATOMIC)]
				get;
				[DispId(DispId.IHTMLCOMMENTELEMENT_ATOMIC)]
				set;
			}
		}

		[ComImport()]
		[Guid("3050f28b-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		public interface IHTMLScriptElement
		{
			Boolean defer
			{
				[DispId(DispId.IHTMLSCRIPTELEMENT_DEFER)]
				get;
				[DispId(DispId.IHTMLSCRIPTELEMENT_DEFER)]
				set;
			}
			String htmlFor
			{
				[DispId(DispId.IHTMLSCRIPTELEMENT_HTMLFOR)]
				get;
				[DispId(DispId.IHTMLSCRIPTELEMENT_HTMLFOR)]
				set;
			}
			String type
			{
				[DispId(DispId.IHTMLSCRIPTELEMENT_TYPE)]
				get;
				[DispId(DispId.IHTMLSCRIPTELEMENT_TYPE)]
				set;
			}
			String readyState
			{
				[DispId(DispId.IHTMLSCRIPTELEMENT_READYSTATE)]
				get;
			}
			String text
			{
				[DispId(DispId.IHTMLSCRIPTELEMENT_TEXT)]
				get;
				[DispId(DispId.IHTMLSCRIPTELEMENT_TEXT)]
				set;
			}
			String @event
			{
				[DispId(DispId.IHTMLSCRIPTELEMENT_EVENT)]
				get;
				[DispId(DispId.IHTMLSCRIPTELEMENT_EVENT)]
				set;
			}
			Object onerror
			{
				[DispId(DispId.IHTMLSCRIPTELEMENT_ONERROR)]
				get;
				[DispId(DispId.IHTMLSCRIPTELEMENT_ONERROR)]
				set;
			}
			String src
			{
				[DispId(DispId.IHTMLSCRIPTELEMENT_SRC)]
				get;
				[DispId(DispId.IHTMLSCRIPTELEMENT_SRC)]
				set;
			}
		}

		[Guid("3050F669-98B5-11CF-BB82-00AA00BDCE0B"), InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IHTMLElementRender
		{
			void DrawToDC([In] IntPtr hdc);
			void SetDocumentPrinter([In, MarshalAs(UnmanagedType.BStr)] string bstrPrinterName, [In, Out] IntPtr hdc);
		}

		[Guid("3050F4D0-98B5-11CF-BB82-00AA00BDCE0B"), InterfaceTypeAttribute(ComInterfaceType.InterfaceIsDual)]
		public interface IHTMLUniqueName
		{
			[DispId(DispId.IHTMLUNIQUENAME_UNIQUENUMBER)]
			void uniqueNumber([Out] out int p);

			[DispId(DispId.IHTMLUNIQUENAME_UNIQUEID)]
			void uniqueID([Out] out string p);
		}

		[Guid("3050F1FF-98B5-11CF-BB82-00AA00BDCE0B"), InterfaceTypeAttribute(ComInterfaceType.InterfaceIsDual)]
		public interface IHTMLElement
		{


			void SetAttribute(
				[In, MarshalAs(UnmanagedType.BStr)]
				string strAttributeName,
				[In, MarshalAs(UnmanagedType.Struct)]
				Object AttributeValue,
				[In, MarshalAs(UnmanagedType.I4)]
				int lFlags);


			void GetAttribute(
				[In, MarshalAs(UnmanagedType.BStr)]
				string strAttributeName,
				[In, MarshalAs(UnmanagedType.I4)]
				int lFlags,
				[Out, MarshalAs(UnmanagedType.LPArray)]
				Object[] pvars);

			[return: MarshalAs(UnmanagedType.Bool)]
			bool RemoveAttribute(
				[In, MarshalAs(UnmanagedType.BStr)]
				string strAttributeName,
				[In, MarshalAs(UnmanagedType.I4)]
				int lFlags);


			void SetClassName(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetClassName();

			void SetId(
			[In, MarshalAs(UnmanagedType.BStr)]
			string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetId();

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetTagName();

			[return: MarshalAs(UnmanagedType.Interface)]
			IHTMLElement GetParentElement();

			[return: MarshalAs(UnmanagedType.Interface)]
			IHTMLStyle GetStyle();


			void SetOnhelp(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetOnhelp();


			void SetOnclick(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetOnclick();


			void SetOndblclick(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetOndblclick();


			void SetOnkeydown(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetOnkeydown();


			void SetOnkeyup(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetOnkeyup();


			void SetOnkeypress(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetOnkeypress();


			void SetOnmouseout(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetOnmouseout();


			void SetOnmouseover(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetOnmouseover();


			void SetOnmousemove(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetOnmousemove();


			void SetOnmousedown(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetOnmousedown();


			void SetOnmouseup(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetOnmouseup();

			[return: MarshalAs(UnmanagedType.Interface)]
			object GetDocument();


			void SetTitle(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetTitle();


			void SetLanguage(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetLanguage();


			void SetOnselectstart(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetOnselectstart();


			void ScrollIntoView(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object varargStart);

			[return: MarshalAs(UnmanagedType.Bool)]
			bool Contains(
				[In, MarshalAs(UnmanagedType.Interface)]
				IHTMLElement pChild);

			[return: MarshalAs(UnmanagedType.I4)]
			int GetSourceIndex();

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetRecordNumber();


			void SetLang(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetLang();

			[return: MarshalAs(UnmanagedType.I4)]
			int GetOffsetLeft();

			[return: MarshalAs(UnmanagedType.I4)]
			int GetOffsetTop();

			[return: MarshalAs(UnmanagedType.I4)]
			int GetOffsetWidth();

			[return: MarshalAs(UnmanagedType.I4)]
			int GetOffsetHeight();

			[return: MarshalAs(UnmanagedType.Interface)]
			IHTMLElement GetOffsetParent();


			void SetInnerHTML(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetInnerHTML();


			void SetInnerText(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetInnerText();


			void SetOuterHTML(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetOuterHTML();


			void SetOuterText(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetOuterText();


			void InsertAdjacentHTML(
				[In, MarshalAs(UnmanagedType.BStr)]
				string where,
				[In, MarshalAs(UnmanagedType.BStr)]
				string html);


			void InsertAdjacentText(
				[In, MarshalAs(UnmanagedType.BStr)]
				string where,
				[In, MarshalAs(UnmanagedType.BStr)]
				string text);

			[return: MarshalAs(UnmanagedType.Interface)]
			IHTMLElement GetParentTextEdit();

			[return: MarshalAs(UnmanagedType.Bool)]
			bool GetIsTextEdit();


			void Click();

			[return: MarshalAs(UnmanagedType.Interface)]
			object GetFilters();


			void SetOndragstart(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetOndragstart();

			[return: MarshalAs(UnmanagedType.BStr)]
			string toString();


			void SetOnbeforeupdate(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetOnbeforeupdate();


			void SetOnafterupdate(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetOnafterupdate();


			void SetOnerrorupdate(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetOnerrorupdate();


			void SetOnrowexit(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetOnrowexit();


			void SetOnrowenter(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetOnrowenter();


			void SetOndatasetchanged(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetOndatasetchanged();


			void SetOndataavailable(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetOndataavailable();


			void SetOndatasetcomplete(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetOndatasetcomplete();


			void SetOnfilterchange(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetOnfilterchange();

			[return: MarshalAs(UnmanagedType.Interface)]
			object GetChildren();

			[return: MarshalAs(UnmanagedType.Interface)]
			object GetAll();

		}

		[Guid("3050F434-98B5-11CF-BB82-00AA00BDCE0B"), InterfaceTypeAttribute(ComInterfaceType.InterfaceIsDual)]
		public interface IHTMLElement2
		{

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetScopeName();


			void SetCapture(
				[In, MarshalAs(UnmanagedType.Bool)]
				bool containerCapture);


			void ReleaseCapture();


			void SetOnlosecapture(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetOnlosecapture();

			[return: MarshalAs(UnmanagedType.BStr)]
			string ComponentFromPoint(
				[In, MarshalAs(UnmanagedType.I4)]
				int x,
				[In, MarshalAs(UnmanagedType.I4)]
				int y);


			void DoScroll(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object component);


			void SetOnscroll(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetOnscroll();


			void SetOndrag(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetOndrag();


			void SetOndragend(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetOndragend();


			void SetOndragenter(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetOndragenter();


			void SetOndragover(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetOndragover();


			void SetOndragleave(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetOndragleave();


			void SetOndrop(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetOndrop();


			void SetOnbeforecut(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetOnbeforecut();


			void SetOncut(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetOncut();


			void SetOnbeforecopy(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetOnbeforecopy();


			void SetOncopy(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetOncopy();


			void SetOnbeforepaste(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetOnbeforepaste();


			void SetOnpaste(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetOnpaste();

			[return: MarshalAs(UnmanagedType.Interface)]
			IHTMLCurrentStyle GetCurrentStyle();


			void SetOnpropertychange(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetOnpropertychange();

			[return: MarshalAs(UnmanagedType.Interface)]
			object GetClientRects();

			[return: MarshalAs(UnmanagedType.Interface)]
			IHTMLRect GetBoundingClientRect();


			void SetExpression(
				[In, MarshalAs(UnmanagedType.BStr)]
				string propname,
				[In, MarshalAs(UnmanagedType.BStr)]
				string expression,
				[In, MarshalAs(UnmanagedType.BStr)]
				string language);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetExpression(
				[In, MarshalAs(UnmanagedType.BStr)]
				Object propname);

			[return: MarshalAs(UnmanagedType.Bool)]
			bool RemoveExpression(
				[In, MarshalAs(UnmanagedType.BStr)]
				string propname);


			void SetTabIndex(
				[In, MarshalAs(UnmanagedType.I2)]
				short p);

			[return: MarshalAs(UnmanagedType.I2)]
			short GetTabIndex();


			void Focus();


			void SetAccessKey(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetAccessKey();


			void SetOnblur(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetOnblur();


			void SetOnfocus(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetOnfocus();


			void SetOnresize(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetOnresize();


			void Blur();


			void AddFilter(
				[In, MarshalAs(UnmanagedType.Interface)]
				object pUnk);


			void RemoveFilter(
				[In, MarshalAs(UnmanagedType.Interface)]
				object pUnk);

			[return: MarshalAs(UnmanagedType.I4)]
			int GetClientHeight();

			[return: MarshalAs(UnmanagedType.I4)]
			int GetClientWidth();

			[return: MarshalAs(UnmanagedType.I4)]
			int GetClientTop();

			[return: MarshalAs(UnmanagedType.I4)]
			int GetClientLeft();

			[return: MarshalAs(UnmanagedType.Bool)]
			bool AttachEvent(
				[In, MarshalAs(UnmanagedType.BStr)]
				string ev,
				[In, MarshalAs(UnmanagedType.Interface)]
				object pdisp);


			void DetachEvent(
				[In, MarshalAs(UnmanagedType.BStr)]
				string ev,
				[In, MarshalAs(UnmanagedType.Interface)]
				object pdisp);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetReadyState();


			void SetOnreadystatechange(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetOnreadystatechange();


			void SetOnrowsdelete(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetOnrowsdelete();


			void SetOnrowsinserted(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetOnrowsinserted();


			void SetOncellchange(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetOncellchange();


			void SetDir(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetDir();

			[return: MarshalAs(UnmanagedType.Interface)]
			object CreateControlRange();

			[return: MarshalAs(UnmanagedType.I4)]
			int GetScrollHeight();

			[return: MarshalAs(UnmanagedType.I4)]
			int GetScrollWidth();


			void SetScrollTop(
				[In, MarshalAs(UnmanagedType.I4)]
				int p);

			[return: MarshalAs(UnmanagedType.I4)]
			int GetScrollTop();


			void SetScrollLeft(
				[In, MarshalAs(UnmanagedType.I4)]
				int p);

			[return: MarshalAs(UnmanagedType.I4)]
			int GetScrollLeft();


			void ClearAttributes();


			void MergeAttributes(
				[In, MarshalAs(UnmanagedType.Interface)]
				IHTMLElement mergeThis);


			void SetOncontextmenu(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetOncontextmenu();

			[return: MarshalAs(UnmanagedType.Interface)]
			IHTMLElement InsertAdjacentElement(
				[In, MarshalAs(UnmanagedType.BStr)]
				string where,
				[In, MarshalAs(UnmanagedType.Interface)]
				IHTMLElement insertedElement);

			[return: MarshalAs(UnmanagedType.Interface)]
			IHTMLElement ApplyElement(
				[In, MarshalAs(UnmanagedType.Interface)]
				IHTMLElement apply,
				[In, MarshalAs(UnmanagedType.BStr)]
				string where);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetAdjacentText(
				[In, MarshalAs(UnmanagedType.BStr)]
				string where);

			[return: MarshalAs(UnmanagedType.BStr)]
			string ReplaceAdjacentText(
				[In, MarshalAs(UnmanagedType.BStr)]
				string where,
				[In, MarshalAs(UnmanagedType.BStr)]
				string newText);

			[return: MarshalAs(UnmanagedType.Bool)]
			bool GetCanHaveChildren();

			[return: MarshalAs(UnmanagedType.I4)]
			int AddBehavior(
				[In, MarshalAs(UnmanagedType.BStr)]
				string bstrUrl,
				[In]
				ref Object pvarFactory);

			[return: MarshalAs(UnmanagedType.Bool)]
			bool RemoveBehavior(
				[In, MarshalAs(UnmanagedType.I4)]
				int cookie);

			[return: MarshalAs(UnmanagedType.Interface)]
			IHTMLStyle GetRuntimeStyle();

			[return: MarshalAs(UnmanagedType.Interface)]
			object GetBehaviorUrns();


			void SetTagUrn(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetTagUrn();


			void SetOnbeforeeditfocus(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetOnbeforeeditfocus();

			[return: MarshalAs(UnmanagedType.I4)]
			int GetReadyStateValue();

			[return: MarshalAs(UnmanagedType.Interface)]
			IHTMLElementCollection GetElementsByTagName(
				[In, MarshalAs(UnmanagedType.BStr)]
				string v);

			[return: MarshalAs(UnmanagedType.Interface)]
			IHTMLStyle GetBaseStyle();

			[return: MarshalAs(UnmanagedType.Interface)]
			IHTMLCurrentStyle GetBaseCurrentStyle();

			[return: MarshalAs(UnmanagedType.Interface)]
			IHTMLStyle GetBaseRuntimeStyle();


			void SetOnmousehover(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetOnmousehover();


			void SetOnkeydownpreview(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetOnkeydownpreview();

			[return: MarshalAs(UnmanagedType.Interface)]
			object GetBehavior(
				[In, MarshalAs(UnmanagedType.BStr)]
				string bstrName,
				[In, MarshalAs(UnmanagedType.BStr)]
				string bstrUrn);
		}

		[ComImport()]
		[Guid("3050f673-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		public interface IHTMLElement3
		{
			[DispId(DispId.IHTMLELEMENT3_MERGEATTRIBUTES)]
			void mergeAttributes(IHTMLElement mergeThis, Object pvarFlags);

			[DispId(DispId.IHTMLELEMENT3_SETACTIVE)]
			void setActive();

			[DispId(DispId.IHTMLELEMENT3_FIREEVENT)]
			bool fireEvent(string bstrEventName, [In, Out, MarshalAs(UnmanagedType.Interface)] ref Interop.IHTMLEventObj pvarEventObject);

			[DispId(DispId.IHTMLELEMENT3_DRAGDROP)]
			bool dragDrop();

			String contentEditable
			{
				[DispId(DispId.IHTMLELEMENT3_CONTENTEDITABLE)]
				get;
				[DispId(DispId.IHTMLELEMENT3_CONTENTEDITABLE)]
				set;
			}
			Object ondeactivate
			{
				[DispId(DispId.IHTMLELEMENT3_ONDEACTIVATE)]
				get;
				[DispId(DispId.IHTMLELEMENT3_ONDEACTIVATE)]
				set;
			}
			Object onactivate
			{
				[DispId(DispId.IHTMLELEMENT3_ONACTIVATE)]
				get;
				[DispId(DispId.IHTMLELEMENT3_ONACTIVATE)]
				set;
			}
			Boolean isMultiLine
			{
				[DispId(DispId.IHTMLELEMENT3_ISMULTILINE)]
				get;
			}
			Object onresizestart
			{
				[DispId(DispId.IHTMLELEMENT3_ONRESIZESTART)]
				get;
				[DispId(DispId.IHTMLELEMENT3_ONRESIZESTART)]
				set;
			}
			Object onlayoutcomplete
			{
				[DispId(DispId.IHTMLELEMENT3_ONLAYOUTCOMPLETE)]
				get;
				[DispId(DispId.IHTMLELEMENT3_ONLAYOUTCOMPLETE)]
				set;
			}
			Boolean inflateBlock
			{
				[DispId(DispId.IHTMLELEMENT3_INFLATEBLOCK)]
				get;
				[DispId(DispId.IHTMLELEMENT3_INFLATEBLOCK)]
				set;
			}
			Boolean isDisabled
			{
				[DispId(DispId.IHTMLELEMENT3_ISDISABLED)]
				get;
			}
			Object onmouseleave
			{
				[DispId(DispId.IHTMLELEMENT3_ONMOUSELEAVE)]
				get;
				[DispId(DispId.IHTMLELEMENT3_ONMOUSELEAVE)]
				set;
			}
			Object onmove
			{
				[DispId(DispId.IHTMLELEMENT3_ONMOVE)]
				get;
				[DispId(DispId.IHTMLELEMENT3_ONMOVE)]
				set;
			}
			Object onmoveend
			{
				[DispId(DispId.IHTMLELEMENT3_ONMOVEEND)]
				get;
				[DispId(DispId.IHTMLELEMENT3_ONMOVEEND)]
				set;
			}
			Object oncontrolselect
			{
				[DispId(DispId.IHTMLELEMENT3_ONCONTROLSELECT)]
				get;
				[DispId(DispId.IHTMLELEMENT3_ONCONTROLSELECT)]
				set;
			}
			Object onbeforedeactivate
			{
				[DispId(DispId.IHTMLELEMENT3_ONBEFOREDEACTIVATE)]
				get;
				[DispId(DispId.IHTMLELEMENT3_ONBEFOREDEACTIVATE)]
				set;
			}
			Object onpage
			{
				[DispId(DispId.IHTMLELEMENT3_ONPAGE)]
				get;
				[DispId(DispId.IHTMLELEMENT3_ONPAGE)]
				set;
			}
			Object onresizeend
			{
				[DispId(DispId.IHTMLELEMENT3_ONRESIZEEND)]
				get;
				[DispId(DispId.IHTMLELEMENT3_ONRESIZEEND)]
				set;
			}
			Object onmouseenter
			{
				[DispId(DispId.IHTMLELEMENT3_ONMOUSEENTER)]
				get;
				[DispId(DispId.IHTMLELEMENT3_ONMOUSEENTER)]
				set;
			}
			Boolean canHaveHTML
			{
				[DispId(DispId.IHTMLELEMENT3_CANHAVEHTML)]
				get;
			}
			Boolean disabled
			{
				[DispId(DispId.IHTMLELEMENT3_DISABLED)]
				get;
				[DispId(DispId.IHTMLELEMENT3_DISABLED)]
				set;
			}
			Boolean hideFocus
			{
				[DispId(DispId.IHTMLELEMENT3_HIDEFOCUS)]
				get;
				[DispId(DispId.IHTMLELEMENT3_HIDEFOCUS)]
				set;
			}
			Object onmovestart
			{
				[DispId(DispId.IHTMLELEMENT3_ONMOVESTART)]
				get;
				[DispId(DispId.IHTMLELEMENT3_ONMOVESTART)]
				set;
			}
			Boolean isContentEditable
			{
				[DispId(DispId.IHTMLELEMENT3_ISCONTENTEDITABLE)]
				get;
			}
			Int32 glyphMode
			{
				[DispId(DispId.IHTMLELEMENT3_GLYPHMODE)]
				get;
			}
		}

		[ComImport()]
		[Guid("3050f80f-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		public interface IHTMLElement4
		{
			[DispId(DispId.IHTMLELEMENT4_NORMALIZE)]
			void normalize();

			[DispId(DispId.IHTMLELEMENT4_GETATTRIBUTENODE)]
			IHTMLDOMAttribute getAttributeNode(String bstrname);

			[DispId(DispId.IHTMLELEMENT4_SETATTRIBUTENODE)]
			IHTMLDOMAttribute setAttributeNode(IHTMLDOMAttribute pattr);

			[DispId(DispId.IHTMLELEMENT4_REMOVEATTRIBUTENODE)]
			IHTMLDOMAttribute removeAttributeNode(IHTMLDOMAttribute pattr);

			Object onmousewheel
			{
				[DispId(DispId.IHTMLELEMENT4_ONMOUSEWHEEL)]
				get;
				[DispId(DispId.IHTMLELEMENT4_ONMOUSEWHEEL)]
				set;
			}
			Object onfocusin
			{
				[DispId(DispId.IHTMLELEMENT4_ONFOCUSIN)]
				get;
				[DispId(DispId.IHTMLELEMENT4_ONFOCUSIN)]
				set;
			}
			Object onbeforeactivate
			{
				[DispId(DispId.IHTMLELEMENT4_ONBEFOREACTIVATE)]
				get;
				[DispId(DispId.IHTMLELEMENT4_ONBEFOREACTIVATE)]
				set;
			}
			Object onfocusout
			{
				[DispId(DispId.IHTMLELEMENT4_ONFOCUSOUT)]
				get;
				[DispId(DispId.IHTMLELEMENT4_ONFOCUSOUT)]
				set;
			}
		}

		[ComImport()]
		[Guid("3050f205-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		public interface IHTMLLinkElement
		{
			IHTMLStyleSheet styleSheet
			{
				[DispId(DispId.IHTMLLINKELEMENT_STYLESHEET)]
				get;
			}
			String readyState
			{
				[DispId(DispId.IHTMLLINKELEMENT_READYSTATE)]
				get;
			}
			Object onload
			{
				[DispId(DispId.IHTMLLINKELEMENT_ONLOAD)]
				get;
				[DispId(DispId.IHTMLLINKELEMENT_ONLOAD)]
				set;
			}
			String rev
			{
				[DispId(DispId.IHTMLLINKELEMENT_REV)]
				get;
				[DispId(DispId.IHTMLLINKELEMENT_REV)]
				set;
			}
			String media
			{
				[DispId(DispId.IHTMLLINKELEMENT_MEDIA)]
				get;
				[DispId(DispId.IHTMLLINKELEMENT_MEDIA)]
				set;
			}
			Object onerror
			{
				[DispId(DispId.IHTMLLINKELEMENT_ONERROR)]
				get;
				[DispId(DispId.IHTMLLINKELEMENT_ONERROR)]
				set;
			}
			String href
			{
				[DispId(DispId.IHTMLLINKELEMENT_HREF)]
				get;
				[DispId(DispId.IHTMLLINKELEMENT_HREF)]
				set;
			}
			String type
			{
				[DispId(DispId.IHTMLLINKELEMENT_TYPE)]
				get;
				[DispId(DispId.IHTMLLINKELEMENT_TYPE)]
				set;
			}
			Object onreadystatechange
			{
				[DispId(DispId.IHTMLLINKELEMENT_ONREADYSTATECHANGE)]
				get;
				[DispId(DispId.IHTMLLINKELEMENT_ONREADYSTATECHANGE)]
				set;
			}
			Boolean disabled
			{
				[DispId(DispId.IHTMLLINKELEMENT_DISABLED)]
				get;
				[DispId(DispId.IHTMLLINKELEMENT_DISABLED)]
				set;
			}
			String rel
			{
				[DispId(DispId.IHTMLLINKELEMENT_REL)]
				get;
				[DispId(DispId.IHTMLLINKELEMENT_REL)]
				set;
			}
		}


		[ComImport()]
		[Guid("3050f266-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		public interface IHTMLMapElement
		{
			String name
			{
				[DispId(DispId.IHTMLMAPELEMENT_NAME)]
				get;
				[DispId(DispId.IHTMLMAPELEMENT_NAME)]
				set;
			}
			IHTMLAreasCollection areas
			{
				[DispId(DispId.IHTMLMAPELEMENT_AREAS)]
				get;
			}
		}

		[ComImport()]
		[Guid("3050f203-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		public interface IHTMLMetaElement
		{
			String httpEquiv
			{
				[DispId(DispId.IHTMLMETAELEMENT_HTTPEQUIV)]
				get;
				[DispId(DispId.IHTMLMETAELEMENT_HTTPEQUIV)]
				set;
			}
			String content
			{
				[DispId(DispId.IHTMLMETAELEMENT_CONTENT)]
				get;
				[DispId(DispId.IHTMLMETAELEMENT_CONTENT)]
				set;
			}
			String url
			{
				[DispId(DispId.IHTMLMETAELEMENT_URL)]
				get;
				[DispId(DispId.IHTMLMETAELEMENT_URL)]
				set;
			}
			String name
			{
				[DispId(DispId.IHTMLMETAELEMENT_NAME)]
				get;
				[DispId(DispId.IHTMLMETAELEMENT_NAME)]
				set;
			}
			String charset
			{
				[DispId(DispId.IHTMLMETAELEMENT_CHARSET)]
				get;
				[DispId(DispId.IHTMLMETAELEMENT_CHARSET)]
				set;
			}
		}

		[ComImport()]
		[Guid("3050f383-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		public interface IHTMLAreasCollection
		{
			[DispId(DispId.IHTMLAREASCOLLECTION_ITEM)]
			Object item(Object name, Object index);

			[DispId(DispId.IHTMLAREASCOLLECTION_TAGS)]
			Object tags(Object tagName);

			[DispId(DispId.IHTMLAREASCOLLECTION_ADD)]
			void add(IHTMLElement element, Object before);

			[DispId(DispId.IHTMLAREASCOLLECTION_REMOVE)]
			void remove(Int32 index);

			Int32 length
			{
				[DispId(DispId.IHTMLAREASCOLLECTION_LENGTH)]
				get;
				[DispId(DispId.IHTMLAREASCOLLECTION_LENGTH)]
				set;
			}
			Object _newEnum
			{
				[DispId(DispId.IHTMLAREASCOLLECTION__NEWENUM)]
				get;
			}
		}

		# region Disp Interfaces

		[ComImport()]
		[Guid("3050f55f-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		public interface DispHTMLDocument
		{
			[DispId(DispId.IHTMLDOCUMENT2_WRITE)]
			void write(Object[] psarray);

			[DispId(DispId.IHTMLDOCUMENT2_WRITELN)]
			void writeln(Object[] psarray);

			[DispId(DispId.IHTMLDOCUMENT2_OPEN)]
			Object open(String url, Object name, Object features, Object replace);

			[DispId(DispId.IHTMLDOCUMENT2_CLOSE)]
			void close();

			[DispId(DispId.IHTMLDOCUMENT2_CLEAR)]
			void clear();

			[DispId(DispId.IHTMLDOCUMENT2_QUERYCOMMANDSUPPORTED)]
			Boolean queryCommandSupported(String cmdID);

			[DispId(DispId.IHTMLDOCUMENT2_QUERYCOMMANDENABLED)]
			Boolean queryCommandEnabled(String cmdID);

			[DispId(DispId.IHTMLDOCUMENT2_QUERYCOMMANDSTATE)]
			Boolean queryCommandState(String cmdID);

			[DispId(DispId.IHTMLDOCUMENT2_QUERYCOMMANDINDETERM)]
			Boolean queryCommandIndeterm(String cmdID);

			[DispId(DispId.IHTMLDOCUMENT2_QUERYCOMMANDTEXT)]
			String queryCommandText(String cmdID);

			[DispId(DispId.IHTMLDOCUMENT2_QUERYCOMMANDVALUE)]
			Object queryCommandValue(String cmdID);

			[DispId(DispId.IHTMLDOCUMENT2_EXECCOMMAND)]
			Boolean execCommand(String cmdID, Boolean showUI, Object value);

			[DispId(DispId.IHTMLDOCUMENT2_EXECCOMMANDSHOWHELP)]
			Boolean execCommandShowHelp(String cmdID);

			[DispId(DispId.IHTMLDOCUMENT2_CREATEELEMENT)]
			IHTMLElement createElement(String eTag);

			[DispId(DispId.IHTMLDOCUMENT2_ELEMENTFROMPOINT)]
			IHTMLElement elementFromPoint(Int32 x, Int32 y);

			[DispId(DispId.IHTMLDOCUMENT2_TOSTRING)]
			String toString();

			[DispId(DispId.IHTMLDOCUMENT2_CREATESTYLESHEET)]
			IHTMLStyleSheet createStyleSheet(String bstrHref, Int32 lIndex);

			[DispId(DispId.IHTMLDOCUMENT3_RELEASECAPTURE)]
			void releaseCapture();

			[DispId(DispId.IHTMLDOCUMENT3_RECALC)]
			void recalc(Boolean fForce);

			[DispId(DispId.IHTMLDOCUMENT3_CREATETEXTNODE)]
			IHTMLDOMNode createTextNode(String text);

			[DispId(DispId.IHTMLDOCUMENT3_ATTACHEVENT)]
			Boolean attachEvent(String @event, Object pDisp);

			[DispId(DispId.IHTMLDOCUMENT3_DETACHEVENT)]
			void detachEvent(String @event, Object pDisp);

			[DispId(DispId.IHTMLDOCUMENT3_CREATEDOCUMENTFRAGMENT)]
			IHTMLDocument2 createDocumentFragment();

			[DispId(DispId.IHTMLDOCUMENT3_GETELEMENTSBYNAME)]
			IHTMLElementCollection getElementsByName(String v);

			[DispId(DispId.IHTMLDOCUMENT3_GETELEMENTBYID)]
			IHTMLElement getElementById(String v);

			[DispId(DispId.IHTMLDOCUMENT3_GETELEMENTSBYTAGNAME)]
			IHTMLElementCollection getElementsByTagName(String v);

			[DispId(DispId.IHTMLDOCUMENT4_FOCUS)]
			void focus();

			[DispId(DispId.IHTMLDOCUMENT4_HASFOCUS)]
			Boolean hasFocus();

			[DispId(DispId.IHTMLDOCUMENT4_CREATEDOCUMENTFROMURL)]
			IHTMLDocument2 createDocumentFromUrl(String bstrUrl, String bstrOptions);

			[DispId(DispId.IHTMLDOCUMENT4_CREATEEVENTOBJECT)]
			IHTMLEventObj createEventObject(Object pvarEventObject);

			[DispId(DispId.IHTMLDOCUMENT4_FIREEVENT)]
			Boolean fireEvent(String bstrEventName, Object pvarEventObject);

			[DispId(DispId.IHTMLDOCUMENT4_CREATERENDERSTYLE)]
			IHTMLRenderStyle createRenderStyle(String v);

			[DispId(DispId.IHTMLDOCUMENT5_CREATEATTRIBUTE)]
			IHTMLDOMAttribute createAttribute(String bstrattrName);

			[DispId(DispId.IHTMLDOCUMENT5_CREATECOMMENT)]
			IHTMLDOMNode createComment(String bstrdata);

			[DispId(DispId.IHTMLDOMNODE_HASCHILDNODES)]
			Boolean hasChildNodes();

			[DispId(DispId.IHTMLDOMNODE_INSERTBEFORE)]
			IHTMLDOMNode insertBefore(IHTMLDOMNode newChild, Object refChild);

			[DispId(DispId.IHTMLDOMNODE_REMOVECHILD)]
			IHTMLDOMNode removeChild(IHTMLDOMNode oldChild);

			[DispId(DispId.IHTMLDOMNODE_REPLACECHILD)]
			IHTMLDOMNode replaceChild(IHTMLDOMNode newChild, IHTMLDOMNode oldChild);

			[DispId(DispId.IHTMLDOMNODE_CLONENODE)]
			IHTMLDOMNode cloneNode(Boolean fDeep);

			[DispId(DispId.IHTMLDOMNODE_REMOVENODE)]
			IHTMLDOMNode removeNode(Boolean fDeep);

			[DispId(DispId.IHTMLDOMNODE_SWAPNODE)]
			IHTMLDOMNode swapNode(IHTMLDOMNode otherNode);

			[DispId(DispId.IHTMLDOMNODE_REPLACENODE)]
			IHTMLDOMNode replaceNode(IHTMLDOMNode replacement);

			[DispId(DispId.IHTMLDOMNODE_APPENDCHILD)]
			IHTMLDOMNode appendChild(IHTMLDOMNode newChild);

			Object onstop
			{
				[DispId(DispId.IHTMLDOCUMENT3_ONSTOP)]
				get;
				[DispId(DispId.IHTMLDOCUMENT3_ONSTOP)]
				set;
			}
			String defaultCharset
			{
				[DispId(DispId.IHTMLDOCUMENT2_DEFAULTCHARSET)]
				get;
				[DispId(DispId.IHTMLDOCUMENT2_DEFAULTCHARSET)]
				set;
			}
			Object onselectstart
			{
				[DispId(DispId.IHTMLDOCUMENT2_ONSELECTSTART)]
				get;
				[DispId(DispId.IHTMLDOCUMENT2_ONSELECTSTART)]
				set;
			}
			String URL
			{
				[DispId(DispId.IHTMLDOCUMENT2_URL)]
				get;
				[DispId(DispId.IHTMLDOCUMENT2_URL)]
				set;
			}
			Object namespaces
			{
				[DispId(DispId.IHTMLDOCUMENT4_NAMESPACES)]
				get;
			}
			Object onrowenter
			{
				[DispId(DispId.IHTMLDOCUMENT2_ONROWENTER)]
				get;
				[DispId(DispId.IHTMLDOCUMENT2_ONROWENTER)]
				set;
			}
			IHTMLElement activeElement
			{
				[DispId(DispId.IHTMLDOCUMENT2_ACTIVEELEMENT)]
				get;
			}
			Object oncontextmenu
			{
				[DispId(DispId.IHTMLDOCUMENT3_ONCONTEXTMENU)]
				get;
				[DispId(DispId.IHTMLDOCUMENT3_ONCONTEXTMENU)]
				set;
			}
			IHTMLDocument2 parentDocument
			{
				[DispId(DispId.IHTMLDOCUMENT3_PARENTDOCUMENT)]
				get;
			}
			IHTMLDOMNode doctype
			{
				[DispId(DispId.IHTMLDOCUMENT5_DOCTYPE)]
				get;
			}
			Object onmousewheel
			{
				[DispId(DispId.IHTMLDOCUMENT5_ONMOUSEWHEEL)]
				get;
				[DispId(DispId.IHTMLDOCUMENT5_ONMOUSEWHEEL)]
				set;
			}
			Object linkColor
			{
				[DispId(DispId.IHTMLDOCUMENT2_LINKCOLOR)]
				get;
				[DispId(DispId.IHTMLDOCUMENT2_LINKCOLOR)]
				set;
			}
			Object alinkColor
			{
				[DispId(DispId.IHTMLDOCUMENT2_ALINKCOLOR)]
				get;
				[DispId(DispId.IHTMLDOCUMENT2_ALINKCOLOR)]
				set;
			}
			Object onrowsinserted
			{
				[DispId(DispId.IHTMLDOCUMENT3_ONROWSINSERTED)]
				get;
				[DispId(DispId.IHTMLDOCUMENT3_ONROWSINSERTED)]
				set;
			}
			Object Script
			{
				[DispId(DispId.IHTMLDOCUMENT_SCRIPT)]
				get;
			}
			Object onmousedown
			{
				[DispId(DispId.IHTMLDOCUMENT2_ONMOUSEDOWN)]
				get;
				[DispId(DispId.IHTMLDOCUMENT2_ONMOUSEDOWN)]
				set;
			}
			IHTMLFramesCollection2 frames
			{
				[DispId(DispId.IHTMLDOCUMENT2_FRAMES)]
				get;
			}
			Object ondblclick
			{
				[DispId(DispId.IHTMLDOCUMENT2_ONDBLCLICK)]
				get;
				[DispId(DispId.IHTMLDOCUMENT2_ONDBLCLICK)]
				set;
			}
			IHTMLDOMNode nextSibling
			{
				[DispId(DispId.IHTMLDOMNODE_NEXTSIBLING)]
				get;
			}
			IHTMLDOMNode firstChild
			{
				[DispId(DispId.IHTMLDOMNODE_FIRSTCHILD)]
				get;
			}
			Object onbeforeupdate
			{
				[DispId(DispId.IHTMLDOCUMENT2_ONBEFOREUPDATE)]
				get;
				[DispId(DispId.IHTMLDOCUMENT2_ONBEFOREUPDATE)]
				set;
			}
			IHTMLElementCollection images
			{
				[DispId(DispId.IHTMLDOCUMENT2_IMAGES)]
				get;
			}
			String domain
			{
				[DispId(DispId.IHTMLDOCUMENT2_DOMAIN)]
				get;
				[DispId(DispId.IHTMLDOCUMENT2_DOMAIN)]
				set;
			}
			Object onbeforeactivate
			{
				[DispId(DispId.IHTMLDOCUMENT5_ONBEFOREACTIVATE)]
				get;
				[DispId(DispId.IHTMLDOCUMENT5_ONBEFOREACTIVATE)]
				set;
			}
			String compatMode
			{
				[DispId(DispId.IHTMLDOCUMENT5_COMPATMODE)]
				get;
			}
			Object nodeValue
			{
				[DispId(DispId.IHTMLDOMNODE_NODEVALUE)]
				get;
				[DispId(DispId.IHTMLDOMNODE_NODEVALUE)]
				set;
			}
			Object onmouseup
			{
				[DispId(DispId.IHTMLDOCUMENT2_ONMOUSEUP)]
				get;
				[DispId(DispId.IHTMLDOCUMENT2_ONMOUSEUP)]
				set;
			}
			Object onhelp
			{
				[DispId(DispId.IHTMLDOCUMENT2_ONHELP)]
				get;
				[DispId(DispId.IHTMLDOCUMENT2_ONHELP)]
				set;
			}
			Object onbeforedeactivate
			{
				[DispId(DispId.IHTMLDOCUMENT5_ONBEFOREDEACTIVATE)]
				get;
				[DispId(DispId.IHTMLDOCUMENT5_ONBEFOREDEACTIVATE)]
				set;
			}
			Object onmouseout
			{
				[DispId(DispId.IHTMLDOCUMENT2_ONMOUSEOUT)]
				get;
				[DispId(DispId.IHTMLDOCUMENT2_ONMOUSEOUT)]
				set;
			}
			Object onrowexit
			{
				[DispId(DispId.IHTMLDOCUMENT2_ONROWEXIT)]
				get;
				[DispId(DispId.IHTMLDOCUMENT2_ONROWEXIT)]
				set;
			}
			IHTMLSelectionObject selection
			{
				[DispId(DispId.IHTMLDOCUMENT2_SELECTION)]
				get;
			}
			Object vlinkColor
			{
				[DispId(DispId.IHTMLDOCUMENT2_VLINKCOLOR)]
				get;
				[DispId(DispId.IHTMLDOCUMENT2_VLINKCOLOR)]
				set;
			}
			Object onfocusout
			{
				[DispId(DispId.IHTMLDOCUMENT5_ONFOCUSOUT)]
				get;
				[DispId(DispId.IHTMLDOCUMENT5_ONFOCUSOUT)]
				set;
			}
			Object onfocusin
			{
				[DispId(DispId.IHTMLDOCUMENT5_ONFOCUSIN)]
				get;
				[DispId(DispId.IHTMLDOCUMENT5_ONFOCUSIN)]
				set;
			}
			Object childNodes
			{
				[DispId(DispId.IHTMLDOMNODE_CHILDNODES)]
				get;
			}
			Object onclick
			{
				[DispId(DispId.IHTMLDOCUMENT2_ONCLICK)]
				get;
				[DispId(DispId.IHTMLDOCUMENT2_ONCLICK)]
				set;
			}
			Object onpropertychange
			{
				[DispId(DispId.IHTMLDOCUMENT3_ONPROPERTYCHANGE)]
				get;
				[DispId(DispId.IHTMLDOCUMENT3_ONPROPERTYCHANGE)]
				set;
			}
			Object attributes
			{
				[DispId(DispId.IHTMLDOMNODE_ATTRIBUTES)]
				get;
			}
			String charset
			{
				[DispId(DispId.IHTMLDOCUMENT2_CHARSET)]
				get;
				[DispId(DispId.IHTMLDOCUMENT2_CHARSET)]
				set;
			}
			String mimeType
			{
				[DispId(DispId.IHTMLDOCUMENT2_MIMETYPE)]
				get;
			}
			String readyState
			{
				[DispId(DispId.IHTMLDOCUMENT2_READYSTATE)]
				get;
			}
			Object onafterupdate
			{
				[DispId(DispId.IHTMLDOCUMENT2_ONAFTERUPDATE)]
				get;
				[DispId(DispId.IHTMLDOCUMENT2_ONAFTERUPDATE)]
				set;
			}
			IHTMLDOMNode parentNode
			{
				[DispId(DispId.IHTMLDOMNODE_PARENTNODE)]
				get;
			}
			String uniqueID
			{
				[DispId(DispId.IHTMLDOCUMENT3_UNIQUEID)]
				get;
			}
			IHTMLElement body
			{
				[DispId(DispId.IHTMLDOCUMENT2_BODY)]
				get;
			}
			Boolean expando
			{
				[DispId(DispId.IHTMLDOCUMENT2_EXPANDO)]
				get;
				[DispId(DispId.IHTMLDOCUMENT2_EXPANDO)]
				set;
			}
			Object bgColor
			{
				[DispId(DispId.IHTMLDOCUMENT2_BGCOLOR)]
				get;
				[DispId(DispId.IHTMLDOCUMENT2_BGCOLOR)]
				set;
			}
			String fileUpdatedDate
			{
				[DispId(DispId.IHTMLDOCUMENT2_FILEUPDATEDDATE)]
				get;
			}
			String designMode
			{
				[DispId(DispId.IHTMLDOCUMENT2_DESIGNMODE)]
				get;
				[DispId(DispId.IHTMLDOCUMENT2_DESIGNMODE)]
				set;
			}
			Object onmousemove
			{
				[DispId(DispId.IHTMLDOCUMENT2_ONMOUSEMOVE)]
				get;
				[DispId(DispId.IHTMLDOCUMENT2_ONMOUSEMOVE)]
				set;
			}
			Object onbeforeeditfocus
			{
				[DispId(DispId.IHTMLDOCUMENT3_ONBEFOREEDITFOCUS)]
				get;
				[DispId(DispId.IHTMLDOCUMENT3_ONBEFOREEDITFOCUS)]
				set;
			}
			Object onkeydown
			{
				[DispId(DispId.IHTMLDOCUMENT2_ONKEYDOWN)]
				get;
				[DispId(DispId.IHTMLDOCUMENT2_ONKEYDOWN)]
				set;
			}
			Boolean enableDownload
			{
				[DispId(DispId.IHTMLDOCUMENT3_ENABLEDOWNLOAD)]
				get;
				[DispId(DispId.IHTMLDOCUMENT3_ENABLEDOWNLOAD)]
				set;
			}
			IHTMLDOMImplementation implementation
			{
				[DispId(DispId.IHTMLDOCUMENT5_IMPLEMENTATION)]
				get;
			}
			String fileCreatedDate
			{
				[DispId(DispId.IHTMLDOCUMENT2_FILECREATEDDATE)]
				get;
			}
			IHTMLElementCollection all
			{
				[DispId(DispId.IHTMLDOCUMENT2_ALL)]
				get;
			}
			String cookie
			{
				[DispId(DispId.IHTMLDOCUMENT2_COOKIE)]
				get;
				[DispId(DispId.IHTMLDOCUMENT2_COOKIE)]
				set;
			}
			Object onactivate
			{
				[DispId(DispId.IHTMLDOCUMENT5_ONACTIVATE)]
				get;
				[DispId(DispId.IHTMLDOCUMENT5_ONACTIVATE)]
				set;
			}
			Object onselectionchange
			{
				[DispId(DispId.IHTMLDOCUMENT4_ONSELECTIONCHANGE)]
				get;
				[DispId(DispId.IHTMLDOCUMENT4_ONSELECTIONCHANGE)]
				set;
			}
			String referrer
			{
				[DispId(DispId.IHTMLDOCUMENT2_REFERRER)]
				get;
			}
			IHTMLLocation location
			{
				[DispId(DispId.IHTMLDOCUMENT2_LOCATION)]
				get;
			}
			Object ondeactivate
			{
				[DispId(DispId.IHTMLDOCUMENT5_ONDEACTIVATE)]
				get;
				[DispId(DispId.IHTMLDOCUMENT5_ONDEACTIVATE)]
				set;
			}
			String title
			{
				[DispId(DispId.IHTMLDOCUMENT2_TITLE)]
				get;
				[DispId(DispId.IHTMLDOCUMENT2_TITLE)]
				set;
			}
			Object onerrorupdate
			{
				[DispId(DispId.IHTMLDOCUMENT2_ONERRORUPDATE)]
				get;
				[DispId(DispId.IHTMLDOCUMENT2_ONERRORUPDATE)]
				set;
			}
			String dir
			{
				[DispId(DispId.IHTMLDOCUMENT3_DIR)]
				get;
				[DispId(DispId.IHTMLDOCUMENT3_DIR)]
				set;
			}
			String baseUrl
			{
				[DispId(DispId.IHTMLDOCUMENT3_BASEURL)]
				get;
				[DispId(DispId.IHTMLDOCUMENT3_BASEURL)]
				set;
			}
			Object onreadystatechange
			{
				[DispId(DispId.IHTMLDOCUMENT2_ONREADYSTATECHANGE)]
				get;
				[DispId(DispId.IHTMLDOCUMENT2_ONREADYSTATECHANGE)]
				set;
			}
			Object oncontrolselect
			{
				[DispId(DispId.IHTMLDOCUMENT4_ONCONTROLSELECT)]
				get;
				[DispId(DispId.IHTMLDOCUMENT4_ONCONTROLSELECT)]
				set;
			}
			String fileModifiedDate
			{
				[DispId(DispId.IHTMLDOCUMENT2_FILEMODIFIEDDATE)]
				get;
			}
			Object ondataavailable
			{
				[DispId(DispId.IHTMLDOCUMENT3_ONDATAAVAILABLE)]
				get;
				[DispId(DispId.IHTMLDOCUMENT3_ONDATAAVAILABLE)]
				set;
			}
			IHTMLElementCollection plugins
			{
				[DispId(DispId.IHTMLDOCUMENT2_PLUGINS)]
				get;
			}
			String nameProp
			{
				[DispId(DispId.IHTMLDOCUMENT2_NAMEPROP)]
				get;
			}
			IHTMLWindow2 parentWindow
			{
				[DispId(DispId.IHTMLDOCUMENT2_PARENTWINDOW)]
				get;
			}
			IHTMLElementCollection scripts
			{
				[DispId(DispId.IHTMLDOCUMENT2_SCRIPTS)]
				get;
			}
			String protocol
			{
				[DispId(DispId.IHTMLDOCUMENT2_PROTOCOL)]
				get;
			}
			Int32 nodeType
			{
				[DispId(DispId.IHTMLDOMNODE_NODETYPE)]
				get;
			}
			Object ownerDocument
			{
				[DispId(DispId.IHTMLDOMNODE2_OWNERDOCUMENT)]
				get;
			}
			String media
			{
				[DispId(DispId.IHTMLDOCUMENT4_MEDIA)]
				get;
				[DispId(DispId.IHTMLDOCUMENT4_MEDIA)]
				set;
			}
			IHTMLElementCollection applets
			{
				[DispId(DispId.IHTMLDOCUMENT2_APPLETS)]
				get;
			}
			String nodeName
			{
				[DispId(DispId.IHTMLDOMNODE_NODENAME)]
				get;
			}
			Object onkeypress
			{
				[DispId(DispId.IHTMLDOCUMENT2_ONKEYPRESS)]
				get;
				[DispId(DispId.IHTMLDOCUMENT2_ONKEYPRESS)]
				set;
			}
			Object oncellchange
			{
				[DispId(DispId.IHTMLDOCUMENT3_ONCELLCHANGE)]
				get;
				[DispId(DispId.IHTMLDOCUMENT3_ONCELLCHANGE)]
				set;
			}
			Object ondatasetcomplete
			{
				[DispId(DispId.IHTMLDOCUMENT3_ONDATASETCOMPLETE)]
				get;
				[DispId(DispId.IHTMLDOCUMENT3_ONDATASETCOMPLETE)]
				set;
			}
			Object ondragstart
			{
				[DispId(DispId.IHTMLDOCUMENT2_ONDRAGSTART)]
				get;
				[DispId(DispId.IHTMLDOCUMENT2_ONDRAGSTART)]
				set;
			}
			Object onmouseover
			{
				[DispId(DispId.IHTMLDOCUMENT2_ONMOUSEOVER)]
				get;
				[DispId(DispId.IHTMLDOCUMENT2_ONMOUSEOVER)]
				set;
			}
			IHTMLElementCollection embeds
			{
				[DispId(DispId.IHTMLDOCUMENT2_EMBEDS)]
				get;
			}
			IHTMLElementCollection anchors
			{
				[DispId(DispId.IHTMLDOCUMENT2_ANCHORS)]
				get;
			}
			IHTMLDOMNode lastChild
			{
				[DispId(DispId.IHTMLDOMNODE_LASTCHILD)]
				get;
			}
			Object onrowsdelete
			{
				[DispId(DispId.IHTMLDOCUMENT3_ONROWSDELETE)]
				get;
				[DispId(DispId.IHTMLDOCUMENT3_ONROWSDELETE)]
				set;
			}
			Boolean inheritStyleSheets
			{
				[DispId(DispId.IHTMLDOCUMENT3_INHERITSTYLESHEETS)]
				get;
				[DispId(DispId.IHTMLDOCUMENT3_INHERITSTYLESHEETS)]
				set;
			}
			Object onkeyup
			{
				[DispId(DispId.IHTMLDOCUMENT2_ONKEYUP)]
				get;
				[DispId(DispId.IHTMLDOCUMENT2_ONKEYUP)]
				set;
			}
			Object fgColor
			{
				[DispId(DispId.IHTMLDOCUMENT2_FGCOLOR)]
				get;
				[DispId(DispId.IHTMLDOCUMENT2_FGCOLOR)]
				set;
			}
			String URLUnencoded
			{
				[DispId(DispId.IHTMLDOCUMENT4_URLUNENCODED)]
				get;
			}
			Object ondatasetchanged
			{
				[DispId(DispId.IHTMLDOCUMENT3_ONDATASETCHANGED)]
				get;
				[DispId(DispId.IHTMLDOCUMENT3_ONDATASETCHANGED)]
				set;
			}
			String security
			{
				[DispId(DispId.IHTMLDOCUMENT2_SECURITY)]
				get;
			}
			IHTMLDOMNode previousSibling
			{
				[DispId(DispId.IHTMLDOMNODE_PREVIOUSSIBLING)]
				get;
			}
			IHTMLElementCollection links
			{
				[DispId(DispId.IHTMLDOCUMENT2_LINKS)]
				get;
			}
			IHTMLStyleSheetsCollection styleSheets
			{
				[DispId(DispId.IHTMLDOCUMENT2_STYLESHEETS)]
				get;
			}
			IHTMLElement documentElement
			{
				[DispId(DispId.IHTMLDOCUMENT3_DOCUMENTELEMENT)]
				get;
			}
			String lastModified
			{
				[DispId(DispId.IHTMLDOCUMENT2_LASTMODIFIED)]
				get;
			}
			IHTMLElementCollection forms
			{
				[DispId(DispId.IHTMLDOCUMENT2_FORMS)]
				get;
			}
			String fileSize
			{
				[DispId(DispId.IHTMLDOCUMENT2_FILESIZE)]
				get;
			}
		}


		[ComImport()]
		[Guid("3050f4b0-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		public interface IHTMLDOMAttribute
		{
			Object nodeValue
			{
				[DispId(DispId.IHTMLDOMATTRIBUTE_NODEVALUE)]
				get;
				[DispId(DispId.IHTMLDOMATTRIBUTE_NODEVALUE)]
				set;
			}
			String nodeName
			{
				[DispId(DispId.IHTMLDOMATTRIBUTE_NODENAME)]
				get;
			}
			Boolean specified
			{
				[DispId(DispId.IHTMLDOMATTRIBUTE_SPECIFIED)]
				get;
			}
		}

		[ComImport()]
		[Guid("3050f4c3-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		public interface IHTMLAttributeCollection
		{
			[DispId(DispId.IHTMLATTRIBUTECOLLECTION_ITEM)]
			Object item(Object name);

			Object _newEnum
			{
				[DispId(DispId.IHTMLATTRIBUTECOLLECTION__NEWENUM)]
				get;
			}
			Int32 length
			{
				[DispId(DispId.IHTMLATTRIBUTECOLLECTION_LENGTH)]
				get;
			}
		}

		[ComImport()]
		[Guid("3050f80a-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		public interface IHTMLAttributeCollection2
		{
			[DispId(DispId.IHTMLATTRIBUTECOLLECTION2_GETNAMEDITEM)]
			IHTMLDOMAttribute getNamedItem(String bstrName);

			[DispId(DispId.IHTMLATTRIBUTECOLLECTION2_SETNAMEDITEM)]
			IHTMLDOMAttribute setNamedItem(IHTMLDOMAttribute ppNode);

			[DispId(DispId.IHTMLATTRIBUTECOLLECTION2_REMOVENAMEDITEM)]
			IHTMLDOMAttribute removeNamedItem(String bstrName);

		}

		[ComImport()]
		[Guid("163BB1E0-6E00-11cf-837A-48DC04C10000")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		public interface IHTMLLocation
		{
			[DispId(DispId.IHTMLLOCATION_RELOAD)]
			void reload(Boolean flag);

			[DispId(DispId.IHTMLLOCATION_REPLACE)]
			void replace(String bstr);

			[DispId(DispId.IHTMLLOCATION_ASSIGN)]
			void assign(String bstr);

			[DispId(DispId.IHTMLLOCATION_TOSTRING)]
			String toString();

			String pathname
			{
				[DispId(DispId.IHTMLLOCATION_PATHNAME)]
				get;
				[DispId(DispId.IHTMLLOCATION_PATHNAME)]
				set;
			}
			String port
			{
				[DispId(DispId.IHTMLLOCATION_PORT)]
				get;
				[DispId(DispId.IHTMLLOCATION_PORT)]
				set;
			}
			String hash
			{
				[DispId(DispId.IHTMLLOCATION_HASH)]
				get;
				[DispId(DispId.IHTMLLOCATION_HASH)]
				set;
			}
			String hostname
			{
				[DispId(DispId.IHTMLLOCATION_HOSTNAME)]
				get;
				[DispId(DispId.IHTMLLOCATION_HOSTNAME)]
				set;
			}
			String href
			{
				[DispId(DispId.IHTMLLOCATION_HREF)]
				get;
				[DispId(DispId.IHTMLLOCATION_HREF)]
				set;
			}
			String host
			{
				[DispId(DispId.IHTMLLOCATION_HOST)]
				get;
				[DispId(DispId.IHTMLLOCATION_HOST)]
				set;
			}
			String search
			{
				[DispId(DispId.IHTMLLOCATION_SEARCH)]
				get;
				[DispId(DispId.IHTMLLOCATION_SEARCH)]
				set;
			}
			String protocol
			{
				[DispId(DispId.IHTMLLOCATION_PROTOCOL)]
				get;
				[DispId(DispId.IHTMLLOCATION_PROTOCOL)]
				set;
			}
		}

		[ComImport()]
		[Guid("3050f80d-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		public interface IHTMLDOMImplementation
		{
			[DispId(DispId.IHTMLDOMIMPLEMENTATION_HASFEATURE)]
			Boolean hasFeature(String bstrfeature, Object version);

		}

		# endregion

		[ComImport()]
		[Guid("3050f265-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		public interface IHTMLAreaElement
		{
			[DispId(DispId.IHTMLAREAELEMENT_FOCUS)]
			void focus();

			[DispId(DispId.IHTMLAREAELEMENT_BLUR)]
			void blur();

			String hash
			{
				[DispId(DispId.IHTMLAREAELEMENT_HASH)]
				get;
				[DispId(DispId.IHTMLAREAELEMENT_HASH)]
				set;
			}
			String shape
			{
				[DispId(DispId.IHTMLAREAELEMENT_SHAPE)]
				get;
				[DispId(DispId.IHTMLAREAELEMENT_SHAPE)]
				set;
			}
			String hostname
			{
				[DispId(DispId.IHTMLAREAELEMENT_HOSTNAME)]
				get;
				[DispId(DispId.IHTMLAREAELEMENT_HOSTNAME)]
				set;
			}
			String alt
			{
				[DispId(DispId.IHTMLAREAELEMENT_ALT)]
				get;
				[DispId(DispId.IHTMLAREAELEMENT_ALT)]
				set;
			}
			Int16 tabIndex
			{
				[DispId(DispId.IHTMLAREAELEMENT_TABINDEX)]
				get;
				[DispId(DispId.IHTMLAREAELEMENT_TABINDEX)]
				set;
			}
			String target
			{
				[DispId(DispId.IHTMLAREAELEMENT_TARGET)]
				get;
				[DispId(DispId.IHTMLAREAELEMENT_TARGET)]
				set;
			}
			String href
			{
				[DispId(DispId.IHTMLAREAELEMENT_HREF)]
				get;
				[DispId(DispId.IHTMLAREAELEMENT_HREF)]
				set;
			}
			Object onblur
			{
				[DispId(DispId.IHTMLAREAELEMENT_ONBLUR)]
				get;
				[DispId(DispId.IHTMLAREAELEMENT_ONBLUR)]
				set;
			}
			String pathname
			{
				[DispId(DispId.IHTMLAREAELEMENT_PATHNAME)]
				get;
				[DispId(DispId.IHTMLAREAELEMENT_PATHNAME)]
				set;
			}
			String protocol
			{
				[DispId(DispId.IHTMLAREAELEMENT_PROTOCOL)]
				get;
				[DispId(DispId.IHTMLAREAELEMENT_PROTOCOL)]
				set;
			}
			String host
			{
				[DispId(DispId.IHTMLAREAELEMENT_HOST)]
				get;
				[DispId(DispId.IHTMLAREAELEMENT_HOST)]
				set;
			}
			String port
			{
				[DispId(DispId.IHTMLAREAELEMENT_PORT)]
				get;
				[DispId(DispId.IHTMLAREAELEMENT_PORT)]
				set;
			}
			Object onfocus
			{
				[DispId(DispId.IHTMLAREAELEMENT_ONFOCUS)]
				get;
				[DispId(DispId.IHTMLAREAELEMENT_ONFOCUS)]
				set;
			}
			String search
			{
				[DispId(DispId.IHTMLAREAELEMENT_SEARCH)]
				get;
				[DispId(DispId.IHTMLAREAELEMENT_SEARCH)]
				set;
			}
			Boolean noHref
			{
				[DispId(DispId.IHTMLAREAELEMENT_NOHREF)]
				get;
				[DispId(DispId.IHTMLAREAELEMENT_NOHREF)]
				set;
			}
			String coords
			{
				[DispId(DispId.IHTMLAREAELEMENT_COORDS)]
				get;
				[DispId(DispId.IHTMLAREAELEMENT_COORDS)]
				set;
			}
		}

		#endregion

		[ComImport()]
		[Guid("3050f311-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		public interface IHTMLFrameBase
		{
			String frameBorder
			{
				[DispId(DispId.IHTMLFRAMEBASE_FRAMEBORDER)]
				get;
				[DispId(DispId.IHTMLFRAMEBASE_FRAMEBORDER)]
				set;
			}
			String name
			{
				[DispId(DispId.IHTMLFRAMEBASE_NAME)]
				get;
				[DispId(DispId.IHTMLFRAMEBASE_NAME)]
				set;
			}
			Boolean noResize
			{
				[DispId(DispId.IHTMLFRAMEBASE_NORESIZE)]
				get;
				[DispId(DispId.IHTMLFRAMEBASE_NORESIZE)]
				set;
			}
			String src
			{
				[DispId(DispId.IHTMLFRAMEBASE_SRC)]
				get;
				[DispId(DispId.IHTMLFRAMEBASE_SRC)]
				set;
			}
			Object border
			{
				[DispId(DispId.IHTMLFRAMEBASE_BORDER)]
				get;
				[DispId(DispId.IHTMLFRAMEBASE_BORDER)]
				set;
			}
			Object frameSpacing
			{
				[DispId(DispId.IHTMLFRAMEBASE_FRAMESPACING)]
				get;
				[DispId(DispId.IHTMLFRAMEBASE_FRAMESPACING)]
				set;
			}
			Object marginWidth
			{
				[DispId(DispId.IHTMLFRAMEBASE_MARGINWIDTH)]
				get;
				[DispId(DispId.IHTMLFRAMEBASE_MARGINWIDTH)]
				set;
			}
			String scrolling
			{
				[DispId(DispId.IHTMLFRAMEBASE_SCROLLING)]
				get;
				[DispId(DispId.IHTMLFRAMEBASE_SCROLLING)]
				set;
			}
			Object marginHeight
			{
				[DispId(DispId.IHTMLFRAMEBASE_MARGINHEIGHT)]
				get;
				[DispId(DispId.IHTMLFRAMEBASE_MARGINHEIGHT)]
				set;
			}
		}

		[ComImport()]
		[Guid("3050f6db-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		public interface IHTMLFrameBase2
		{
			Object onreadystatechange
			{
				[DispId(DispId.IHTMLFRAMEBASE2_ONREADYSTATECHANGE)]
				get;
				[DispId(DispId.IHTMLFRAMEBASE2_ONREADYSTATECHANGE)]
				set;
			}
			Object onload
			{
				[DispId(DispId.IHTMLFRAMEBASE2_ONLOAD)]
				get;
				[DispId(DispId.IHTMLFRAMEBASE2_ONLOAD)]
				set;
			}
			String readyState
			{
				[DispId(DispId.IHTMLFRAMEBASE2_READYSTATE)]
				get;
			}
			IHTMLWindow2 contentWindow
			{
				[DispId(DispId.IHTMLFRAMEBASE2_CONTENTWINDOW)]
				get;
			}
			Boolean allowTransparency
			{
				[DispId(DispId.IHTMLFRAMEBASE2_ALLOWTRANSPARENCY)]
				get;
				[DispId(DispId.IHTMLFRAMEBASE2_ALLOWTRANSPARENCY)]
				set;
			}
		}


		[ComImport()]
		[Guid("3050f7f5-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		public interface IHTMLFrameElement2
		{
			Object width
			{
				[DispId(DispId.IHTMLFRAMEELEMENT2_WIDTH)]
				get;
				[DispId(DispId.IHTMLFRAMEELEMENT2_WIDTH)]
				set;
			}
			Object height
			{
				[DispId(DispId.IHTMLFRAMEELEMENT2_HEIGHT)]
				get;
				[DispId(DispId.IHTMLFRAMEELEMENT2_HEIGHT)]
				set;
			}
		}


		[ComImport()]
		[Guid("332c4426-26cb-11d0-b483-00c04fd90119")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		public interface IHTMLFramesCollection2
		{
			[DispId(DispId.IHTMLFRAMESCOLLECTION2_ITEM)]
			Object item(Object pvarIndex);

			[DispId(DispId.IHTMLFRAMESCOLLECTION2_LENGTH)]
			Int32 length();
		}

		[Guid("3050F21F-98B5-11CF-BB82-00AA00BDCE0B"), InterfaceTypeAttribute(ComInterfaceType.InterfaceIsDual)]
		public interface IHTMLElementCollection
		{

			[return: MarshalAs(UnmanagedType.BStr)]
			string toString();


			void SetLength(
				[In, MarshalAs(UnmanagedType.I4)]
				int p);

			[return: MarshalAs(UnmanagedType.I4)]
			int GetLength();

			[return: MarshalAs(UnmanagedType.Interface)]
			object Get_newEnum();

			[return: MarshalAs(UnmanagedType.Interface)]
			object Item(
				[In, MarshalAs(UnmanagedType.Struct)]
				object name,
				[In, MarshalAs(UnmanagedType.Struct)]
				object index);

			[return: MarshalAs(UnmanagedType.Interface)]
			object Tags(
				[In, MarshalAs(UnmanagedType.Struct)]
				object tagName);
		}

		[Guid("3050F6C9-98B5-11CF-BB82-00AA00BDCE0B"), InterfaceTypeAttribute(ComInterfaceType.InterfaceIsDual)]
		public interface IHTMLElementDefaults
		{

			[return: MarshalAs(UnmanagedType.Interface)]
			IHTMLStyle GetStyle();

			void SetTabStop(
				[In, MarshalAs(UnmanagedType.Bool)]
				bool v);

			[return: MarshalAs(UnmanagedType.Bool)]
			bool GetTabStop();


			void SetViewInheritStyle(
				[In, MarshalAs(UnmanagedType.Bool)]
				bool v);

			[return: MarshalAs(UnmanagedType.Bool)]
			bool GetViewInheritStyle();


			void SetViewMasterTab(
				[In, MarshalAs(UnmanagedType.Bool)]
				bool v);

			[return: MarshalAs(UnmanagedType.Bool)]
			bool GetViewMasterTab();


			void SetScrollSegmentX(
				[In, MarshalAs(UnmanagedType.I4)]
				int v);

			[return: MarshalAs(UnmanagedType.I4)]
			int GetScrollSegmentX();


			void SetScrollSegmentY(
				[In, MarshalAs(UnmanagedType.I4)]
				object p);

			[return: MarshalAs(UnmanagedType.I4)]
			int GetScrollSegmentY();


			void SetIsMultiLine(
				[In, MarshalAs(UnmanagedType.Bool)]
				bool v);

			[return: MarshalAs(UnmanagedType.Bool)]
			bool GetIsMultiLine();


			void SetContentEditable(
				[In, MarshalAs(UnmanagedType.BStr)]
				string v);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetContentEditable();


			void SetCanHaveHTML(
				[In, MarshalAs(UnmanagedType.Bool)]
				bool v);

			[return: MarshalAs(UnmanagedType.Bool)]
			bool GetCanHaveHTML();


			void SetViewLink(
				[In, MarshalAs(UnmanagedType.Interface)]
				IHTMLDocument viewLink);

			[return: MarshalAs(UnmanagedType.Interface)]
			IHTMLDocument GetViewLink();

			void SetFrozen(
				[In, MarshalAs(UnmanagedType.Bool)]
				bool v);

			[return: MarshalAs(UnmanagedType.Bool)]
			bool GetFrozen();
		}

		//        [Guid("3050F33C-98B5-11CF-BB82-00AA00BDCE0B"), InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
		//            public interface IHTMLElementEvents 
		//        {
		//            void Bogus1();
		//            void Bogus2();
		//            void Bogus3();
		//            void Invoke(
		//                [In, MarshalAs(UnmanagedType.U4)] int id,
		//                [In] ref Guid g,
		//                [In, MarshalAs(UnmanagedType.U4)] int lcid,
		//                [In, MarshalAs(UnmanagedType.U4)] int dwFlags,
		//                [In] DISPPARAMS pdp,
		//                [Out, MarshalAs(UnmanagedType.LPArray)]
		//                Object[] pvarRes,
		//                [Out]
		//                EXCEPINFO pei,
		//                [Out, MarshalAs(UnmanagedType.LPArray)]
		//                int[] nArgError);
		//        }


		[ComImport()]
		[Guid("3050f32d-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		public interface IHTMLEventObj
		{
			String qualifier
			{
				[DispId(DispId.IHTMLEVENTOBJ_QUALIFIER)]
				get;
			}
			Boolean ctrlKey
			{
				[DispId(DispId.IHTMLEVENTOBJ_CTRLKEY)]
				get;
			}
			Object srcFilter
			{
				[DispId(DispId.IHTMLEVENTOBJ_SRCFILTER)]
				get;
			}
			Int32 screenX
			{
				[DispId(DispId.IHTMLEVENTOBJ_SCREENX)]
				get;
			}
			Boolean altKey
			{
				[DispId(DispId.IHTMLEVENTOBJ_ALTKEY)]
				get;
			}
			Int32 x
			{
				[DispId(DispId.IHTMLEVENTOBJ_X)]
				get;
			}
			Int32 offsetY
			{
				[DispId(DispId.IHTMLEVENTOBJ_OFFSETY)]
				get;
			}
			Int32 reason
			{
				[DispId(DispId.IHTMLEVENTOBJ_REASON)]
				get;
			}
			Int32 clientX
			{
				[DispId(DispId.IHTMLEVENTOBJ_CLIENTX)]
				get;
			}
			Boolean cancelBubble
			{
				[DispId(DispId.IHTMLEVENTOBJ_CANCELBUBBLE)]
				get;
				[DispId(DispId.IHTMLEVENTOBJ_CANCELBUBBLE)]
				set;
			}
			Int32 clientY
			{
				[DispId(DispId.IHTMLEVENTOBJ_CLIENTY)]
				get;
			}
			Int32 offsetX
			{
				[DispId(DispId.IHTMLEVENTOBJ_OFFSETX)]
				get;
			}
			Int32 screenY
			{
				[DispId(DispId.IHTMLEVENTOBJ_SCREENY)]
				get;
			}
			Object returnValue
			{
				[DispId(DispId.IHTMLEVENTOBJ_RETURNVALUE)]
				get;
				[DispId(DispId.IHTMLEVENTOBJ_RETURNVALUE)]
				set;
			}
			IHTMLElement toElement
			{
				[DispId(DispId.IHTMLEVENTOBJ_TOELEMENT)]
				get;
			}
			String type
			{
				[DispId(DispId.IHTMLEVENTOBJ_TYPE)]
				get;
			}
			IHTMLElement fromElement
			{
				[DispId(DispId.IHTMLEVENTOBJ_FROMELEMENT)]
				get;
			}
			Int32 y
			{
				[DispId(DispId.IHTMLEVENTOBJ_Y)]
				get;
			}
			Boolean shiftKey
			{
				[DispId(DispId.IHTMLEVENTOBJ_SHIFTKEY)]
				get;
			}

			Int32 button
			{
				[DispId(DispId.IHTMLEVENTOBJ_BUTTON)]
				get;
			}
			Int32 keyCode
			{
				[DispId(DispId.IHTMLEVENTOBJ_KEYCODE)]
				get;
				[DispId(DispId.IHTMLEVENTOBJ_KEYCODE)]
				set;
			}
			IHTMLElement srcElement
			{
				[DispId(DispId.IHTMLEVENTOBJ_SRCELEMENT)]
				get;
			}
		}

		[ComImport()]
		[Guid("3050f48B-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		public interface IHTMLEventObj2
		{
			[DispId(DispId.IHTMLEVENTOBJ2_SETATTRIBUTE)]
			void setAttribute(String strAttributeName, Object AttributeValue, Int32 lFlags);

			[DispId(DispId.IHTMLEVENTOBJ2_GETATTRIBUTE)]
			Object getAttribute(String strAttributeName, Int32 lFlags);

			[DispId(DispId.IHTMLEVENTOBJ2_REMOVEATTRIBUTE)]
			Boolean removeAttribute(String strAttributeName, Int32 lFlags);

			Int32 y
			{
				[DispId(DispId.IHTMLEVENTOBJ2_Y)]
				get;
				[DispId(DispId.IHTMLEVENTOBJ2_Y)]
				set;
			}
			Int32 clientX
			{
				[DispId(DispId.IHTMLEVENTOBJ2_CLIENTX)]
				get;
				[DispId(DispId.IHTMLEVENTOBJ2_CLIENTX)]
				set;
			}
			String dataFld
			{
				[DispId(DispId.IHTMLEVENTOBJ2_DATAFLD)]
				get;
				[DispId(DispId.IHTMLEVENTOBJ2_DATAFLD)]
				set;
			}
			IHTMLBookmarkCollection bookmarks
			{
				[DispId(DispId.IHTMLEVENTOBJ2_BOOKMARKS)]
				get;
				[DispId(DispId.IHTMLEVENTOBJ2_BOOKMARKS)]
				set;
			}
			IHTMLDataTransfer dataTransfer
			{
				[DispId(DispId.IHTMLEVENTOBJ2_DATATRANSFER)]
				get;
			}
			IHTMLElementCollection boundElements
			{
				[DispId(DispId.IHTMLEVENTOBJ2_BOUNDELEMENTS)]
				get;
				[DispId(DispId.IHTMLEVENTOBJ2_BOUNDELEMENTS)]
				set;
			}
			Int32 reason
			{
				[DispId(DispId.IHTMLEVENTOBJ2_REASON)]
				get;
				[DispId(DispId.IHTMLEVENTOBJ2_REASON)]
				set;
			}
			Int32 button
			{
				[DispId(DispId.IHTMLEVENTOBJ2_BUTTON)]
				get;
				[DispId(DispId.IHTMLEVENTOBJ2_BUTTON)]
				set;
			}
			Int32 clientY
			{
				[DispId(DispId.IHTMLEVENTOBJ2_CLIENTY)]
				get;
				[DispId(DispId.IHTMLEVENTOBJ2_CLIENTY)]
				set;
			}
			String qualifier
			{
				[DispId(DispId.IHTMLEVENTOBJ2_QUALIFIER)]
				get;
				[DispId(DispId.IHTMLEVENTOBJ2_QUALIFIER)]
				set;
			}
			Int32 x
			{
				[DispId(DispId.IHTMLEVENTOBJ2_X)]
				get;
				[DispId(DispId.IHTMLEVENTOBJ2_X)]
				set;
			}
			Object recordset
			{
				[DispId(DispId.IHTMLEVENTOBJ2_RECORDSET)]
				get;
				[DispId(DispId.IHTMLEVENTOBJ2_RECORDSET)]
				set;
			}
			Boolean ctrlKey
			{
				[DispId(DispId.IHTMLEVENTOBJ2_CTRLKEY)]
				get;
				[DispId(DispId.IHTMLEVENTOBJ2_CTRLKEY)]
				set;
			}
			Boolean shiftKey
			{
				[DispId(DispId.IHTMLEVENTOBJ2_SHIFTKEY)]
				get;
				[DispId(DispId.IHTMLEVENTOBJ2_SHIFTKEY)]
				set;
			}
			Boolean altKey
			{
				[DispId(DispId.IHTMLEVENTOBJ2_ALTKEY)]
				get;
				[DispId(DispId.IHTMLEVENTOBJ2_ALTKEY)]
				set;
			}
			Object srcFilter
			{
				[DispId(DispId.IHTMLEVENTOBJ2_SRCFILTER)]
				get;
				[DispId(DispId.IHTMLEVENTOBJ2_SRCFILTER)]
				set;
			}
			String srcUrn
			{
				[DispId(DispId.IHTMLEVENTOBJ2_SRCURN)]
				get;
				[DispId(DispId.IHTMLEVENTOBJ2_SRCURN)]
				set;
			}
			String type
			{
				[DispId(DispId.IHTMLEVENTOBJ2_TYPE)]
				get;
				[DispId(DispId.IHTMLEVENTOBJ2_TYPE)]
				set;
			}
			Int32 offsetY
			{
				[DispId(DispId.IHTMLEVENTOBJ2_OFFSETY)]
				get;
				[DispId(DispId.IHTMLEVENTOBJ2_OFFSETY)]
				set;
			}
			Int32 offsetX
			{
				[DispId(DispId.IHTMLEVENTOBJ2_OFFSETX)]
				get;
				[DispId(DispId.IHTMLEVENTOBJ2_OFFSETX)]
				set;
			}
			IHTMLElement fromElement
			{
				[DispId(DispId.IHTMLEVENTOBJ2_FROMELEMENT)]
				get;
				[DispId(DispId.IHTMLEVENTOBJ2_FROMELEMENT)]
				set;
			}
			IHTMLElement toElement
			{
				[DispId(DispId.IHTMLEVENTOBJ2_TOELEMENT)]
				get;
				[DispId(DispId.IHTMLEVENTOBJ2_TOELEMENT)]
				set;
			}
			Boolean repeat
			{
				[DispId(DispId.IHTMLEVENTOBJ2_REPEAT)]
				get;
				[DispId(DispId.IHTMLEVENTOBJ2_REPEAT)]
				set;
			}
			Int32 screenY
			{
				[DispId(DispId.IHTMLEVENTOBJ2_SCREENY)]
				get;
				[DispId(DispId.IHTMLEVENTOBJ2_SCREENY)]
				set;
			}
			IHTMLElement srcElement
			{
				[DispId(DispId.IHTMLEVENTOBJ2_SRCELEMENT)]
				get;
				[DispId(DispId.IHTMLEVENTOBJ2_SRCELEMENT)]
				set;
			}
			String propertyName
			{
				[DispId(DispId.IHTMLEVENTOBJ2_PROPERTYNAME)]
				get;
				[DispId(DispId.IHTMLEVENTOBJ2_PROPERTYNAME)]
				set;
			}
			Int32 screenX
			{
				[DispId(DispId.IHTMLEVENTOBJ2_SCREENX)]
				get;
				[DispId(DispId.IHTMLEVENTOBJ2_SCREENX)]
				set;
			}
		}

		[ComImport()]
		[Guid("3050f680-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		public interface IHTMLEventObj3
		{
			Boolean ctrlLeft
			{
				[DispId(DispId.IHTMLEVENTOBJ3_CTRLLEFT)]
				get;
				[DispId(DispId.IHTMLEVENTOBJ3_CTRLLEFT)]
				set;
			}
			String nextPage
			{
				[DispId(DispId.IHTMLEVENTOBJ3_NEXTPAGE)]
				get;
			}
			IntPtr imeRequest
			{
				[DispId(DispId.IHTMLEVENTOBJ3_IMEREQUEST)]
				get;
			}
			Int32 behaviorPart
			{
				[DispId(DispId.IHTMLEVENTOBJ3_BEHAVIORPART)]
				get;
			}
			Boolean contentOverflow
			{
				[DispId(DispId.IHTMLEVENTOBJ3_CONTENTOVERFLOW)]
				get;
			}
			IntPtr imeRequestData
			{
				[DispId(DispId.IHTMLEVENTOBJ3_IMEREQUESTDATA)]
				get;
			}
			Boolean shiftLeft
			{
				[DispId(DispId.IHTMLEVENTOBJ3_SHIFTLEFT)]
				get;
				[DispId(DispId.IHTMLEVENTOBJ3_SHIFTLEFT)]
				set;
			}
			IntPtr imeCompositionChange
			{
				[DispId(DispId.IHTMLEVENTOBJ3_IMECOMPOSITIONCHANGE)]
				get;
			}
			IntPtr imeNotifyData
			{
				[DispId(DispId.IHTMLEVENTOBJ3_IMENOTIFYDATA)]
				get;
			}
			IntPtr keyboardLayout
			{
				[DispId(DispId.IHTMLEVENTOBJ3_KEYBOARDLAYOUT)]
				get;
			}
			IntPtr imeNotifyCommand
			{
				[DispId(DispId.IHTMLEVENTOBJ3_IMENOTIFYCOMMAND)]
				get;
			}
			Int32 behaviorCookie
			{
				[DispId(DispId.IHTMLEVENTOBJ3_BEHAVIORCOOKIE)]
				get;
			}
			Boolean altLeft
			{
				[DispId(DispId.IHTMLEVENTOBJ3_ALTLEFT)]
				get;
				[DispId(DispId.IHTMLEVENTOBJ3_ALTLEFT)]
				set;
			}
		}

		[ComImport()]
		[Guid("3050f814-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		public interface IHTMLEventObj4
		{
			Int32 wheelDelta
			{
				[DispId(DispId.IHTMLEVENTOBJ4_WHEELDELTA)]
				get;
			}
		}

		[ComImport()]
		[Guid("3050f4ce-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		public interface IHTMLBookmarkCollection
		{
			[DispId(DispId.IHTMLBOOKMARKCOLLECTION_ITEM)]
			Object item(Int32 index);

			Object _newEnum
			{
				[DispId(DispId.IHTMLBOOKMARKCOLLECTION__NEWENUM)]
				get;
			}
			Int32 length
			{
				[DispId(DispId.IHTMLBOOKMARKCOLLECTION_LENGTH)]
				get;
			}
		}

		[Guid("3050F6A6-98B5-11CF-BB82-00AA00BDCE0B"), InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IHTMLPainter
		{
			void Draw(
				[In, MarshalAs(UnmanagedType.I4)]
				int leftBounds,
				[In, MarshalAs(UnmanagedType.I4)]
				int topBounds,
				[In, MarshalAs(UnmanagedType.I4)]
				int rightBounds,
				[In, MarshalAs(UnmanagedType.I4)]
				int bottomBounds,
				[In, MarshalAs(UnmanagedType.I4)]
				int leftUpdate,
				[In, MarshalAs(UnmanagedType.I4)]
				int topUpdate,
				[In, MarshalAs(UnmanagedType.I4)]
				int rightUpdate,
				[In, MarshalAs(UnmanagedType.I4)]
				int bottomUpdate,
				[In, MarshalAs(UnmanagedType.U4)]
				int lDrawFlags,
				[In]
				IntPtr hdc,
				[In]
				IntPtr pvDrawObject);

			void OnResize(
				[In, MarshalAs(UnmanagedType.I4)]
				int cx,
				[In, MarshalAs(UnmanagedType.I4)]
				int cy);

			void GetPainterInfo(
				[Out]
				HTML_PAINTER_INFO htmlPainterInfo);

			void HitTestPoint(
				[In, MarshalAs(UnmanagedType.I4)]
				int ptx,
				[In, MarshalAs(UnmanagedType.I4)]
				int pty,
				[Out, MarshalAs(UnmanagedType.Bool)]
				out bool pbHit,
				[Out, MarshalAs(UnmanagedType.I4)]
				out int plPartID);
		}

		[Guid("3050F6DF-98B5-11CF-BB82-00AA00BDCE0B")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IHTMLPainterEventInfo
		{
			void GetEventInfoFlags([Out] out int plEventInfoFlags);
			void GetEventTarget([In] IHTMLElement ppElement);
			void SetCursor([In] int lPartID);
			void StringFromPartID([In] int lPartID, [Out, MarshalAs(UnmanagedType.BStr)] out string pbstrPart);
		}


		[ComImport()]
		[Guid("3050f24f-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		public interface IHTMLObjectElement
		{
			IHTMLFormElement form
			{
				[DispId(DispId.IHTMLOBJECTELEMENT_FORM)]
				get;
			}
			Object recordset
			{
				[DispId(DispId.IHTMLOBJECTELEMENT_RECORDSET)]
				get;
				[DispId(DispId.IHTMLOBJECTELEMENT_RECORDSET)]
				set;
			}
			Int32 hspace
			{
				[DispId(DispId.IHTMLOBJECTELEMENT_HSPACE)]
				get;
				[DispId(DispId.IHTMLOBJECTELEMENT_HSPACE)]
				set;
			}
			Object onreadystatechange
			{
				[DispId(DispId.IHTMLOBJECTELEMENT_ONREADYSTATECHANGE)]
				get;
				[DispId(DispId.IHTMLOBJECTELEMENT_ONREADYSTATECHANGE)]
				set;
			}
			Object width
			{
				[DispId(DispId.IHTMLOBJECTELEMENT_WIDTH)]
				get;
				[DispId(DispId.IHTMLOBJECTELEMENT_WIDTH)]
				set;
			}
			String type
			{
				[DispId(DispId.IHTMLOBJECTELEMENT_TYPE)]
				get;
				[DispId(DispId.IHTMLOBJECTELEMENT_TYPE)]
				set;
			}
			String BaseHref
			{
				[DispId(DispId.IHTMLOBJECTELEMENT_BASEHREF)]
				get;
			}
			String codeBase
			{
				[DispId(DispId.IHTMLOBJECTELEMENT_CODEBASE)]
				get;
				[DispId(DispId.IHTMLOBJECTELEMENT_CODEBASE)]
				set;
			}
			Object @object
			{
				[DispId(DispId.IHTMLOBJECTELEMENT_OBJECT)]
				get;
			}
			String altHtml
			{
				[DispId(DispId.IHTMLOBJECTELEMENT_ALTHTML)]
				get;
				[DispId(DispId.IHTMLOBJECTELEMENT_ALTHTML)]
				set;
			}
			Object onerror
			{
				[DispId(DispId.IHTMLOBJECTELEMENT_ONERROR)]
				get;
				[DispId(DispId.IHTMLOBJECTELEMENT_ONERROR)]
				set;
			}
			String classid
			{
				[DispId(DispId.IHTMLOBJECTELEMENT_CLASSID)]
				get;
			}
			String codeType
			{
				[DispId(DispId.IHTMLOBJECTELEMENT_CODETYPE)]
				get;
				[DispId(DispId.IHTMLOBJECTELEMENT_CODETYPE)]
				set;
			}
			Int32 vspace
			{
				[DispId(DispId.IHTMLOBJECTELEMENT_VSPACE)]
				get;
				[DispId(DispId.IHTMLOBJECTELEMENT_VSPACE)]
				set;
			}
			Int32 readyState
			{
				[DispId(DispId.IHTMLOBJECTELEMENT_READYSTATE)]
				get;
			}
			Object height
			{
				[DispId(DispId.IHTMLOBJECTELEMENT_HEIGHT)]
				get;
				[DispId(DispId.IHTMLOBJECTELEMENT_HEIGHT)]
				set;
			}
			String data
			{
				[DispId(DispId.IHTMLOBJECTELEMENT_DATA)]
				get;
			}
			String name
			{
				[DispId(DispId.IHTMLOBJECTELEMENT_NAME)]
				get;
				[DispId(DispId.IHTMLOBJECTELEMENT_NAME)]
				set;
			}
			String code
			{
				[DispId(DispId.IHTMLOBJECTELEMENT_CODE)]
				get;
				[DispId(DispId.IHTMLOBJECTELEMENT_CODE)]
				set;
			}
			String align
			{
				[DispId(DispId.IHTMLOBJECTELEMENT_ALIGN)]
				get;
				[DispId(DispId.IHTMLOBJECTELEMENT_ALIGN)]
				set;
			}
		}


		[ComImport()]
		[Guid("3050f4cd-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		public interface IHTMLObjectElement2
		{
			[DispId(DispId.IHTMLOBJECTELEMENT2_NAMEDRECORDSET)]
			Object namedRecordset(String dataMember, Object hierarchy);

			String classid
			{
				[DispId(DispId.IHTMLOBJECTELEMENT2_CLASSID)]
				get;
				[DispId(DispId.IHTMLOBJECTELEMENT2_CLASSID)]
				set;
			}
			String data
			{
				[DispId(DispId.IHTMLOBJECTELEMENT2_DATA)]
				get;
				[DispId(DispId.IHTMLOBJECTELEMENT2_DATA)]
				set;
			}
		}


		[ComImport()]
		[Guid("3050f827-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		public interface IHTMLObjectElement3
		{
			Boolean declare
			{
				[DispId(DispId.IHTMLOBJECTELEMENT3_DECLARE)]
				get;
				[DispId(DispId.IHTMLOBJECTELEMENT3_DECLARE)]
				set;
			}
			String useMap
			{
				[DispId(DispId.IHTMLOBJECTELEMENT3_USEMAP)]
				get;
				[DispId(DispId.IHTMLOBJECTELEMENT3_USEMAP)]
				set;
			}
			Object border
			{
				[DispId(DispId.IHTMLOBJECTELEMENT3_BORDER)]
				get;
				[DispId(DispId.IHTMLOBJECTELEMENT3_BORDER)]
				set;
			}
			String standby
			{
				[DispId(DispId.IHTMLOBJECTELEMENT3_STANDBY)]
				get;
				[DispId(DispId.IHTMLOBJECTELEMENT3_STANDBY)]
				set;
			}
			String alt
			{
				[DispId(DispId.IHTMLOBJECTELEMENT3_ALT)]
				get;
				[DispId(DispId.IHTMLOBJECTELEMENT3_ALT)]
				set;
			}
			String archive
			{
				[DispId(DispId.IHTMLOBJECTELEMENT3_ARCHIVE)]
				get;
				[DispId(DispId.IHTMLOBJECTELEMENT3_ARCHIVE)]
				set;
			}
		}

		[ComImport()]
		[Guid("3050f6a7-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IHTMLPaintSite
		{
			void InvalidatePainterInfo();

			void InvalidateRect(IntPtr prcInvalid);

			void InvalidateRegion(IntPtr rgnInvalid);

			void GetDrawInfo(Int32 lFlags, out HTML_PAINT_DRAW_INFO pDrawInfo);

			void TransformGlobalToLocal(POINT ptGlobal, out POINT pptLocal);

			void TransformLocalToGlobal(POINT ptLocal, out POINT pptGlobal);

			void GetHitTestCookie(out Int32 plCookie);

		}

		[ComImport()]
		[Guid("3050f4a4-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		public interface IHTMLRectCollection
		{
			[DispId(DispId.IHTMLRECTCOLLECTION_ITEM)]
			Object item(Object pvarIndex);

			Int32 length
			{
				[DispId(DispId.IHTMLRECTCOLLECTION_LENGTH)]
				get;
			}
			Object _newEnum
			{
				[DispId(DispId.IHTMLRECTCOLLECTION__NEWENUM)]
				get;
			}
		}

		[ComImport()]
		[Guid("3050f4a3-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		public interface IHTMLRect
		{
			Int32 bottom
			{
				[DispId(DispId.IHTMLRECT_BOTTOM)]
				get;
				[DispId(DispId.IHTMLRECT_BOTTOM)]
				set;
			}
			Int32 right
			{
				[DispId(DispId.IHTMLRECT_RIGHT)]
				get;
				[DispId(DispId.IHTMLRECT_RIGHT)]
				set;
			}
			Int32 top
			{
				[DispId(DispId.IHTMLRECT_TOP)]
				get;
				[DispId(DispId.IHTMLRECT_TOP)]
				set;
			}
			Int32 left
			{
				[DispId(DispId.IHTMLRECT_LEFT)]
				get;
				[DispId(DispId.IHTMLRECT_LEFT)]
				set;
			}
		}

		[ComImport()]
		[Guid("3050f3cf-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		public interface IHTMLRuleStyle
		{
			[DispId(DispId.IHTMLRULESTYLE_SETATTRIBUTE)]
			void setAttribute(String strAttributeName, Object AttributeValue, Int32 lFlags);

			[DispId(DispId.IHTMLRULESTYLE_GETATTRIBUTE)]
			Object getAttribute(String strAttributeName, Int32 lFlags);

			[DispId(DispId.IHTMLRULESTYLE_REMOVEATTRIBUTE)]
			Boolean removeAttribute(String strAttributeName, Int32 lFlags);

			String backgroundPosition
			{
				[DispId(DispId.IHTMLRULESTYLE_BACKGROUNDPOSITION)]
				get;
				[DispId(DispId.IHTMLRULESTYLE_BACKGROUNDPOSITION)]
				set;
			}
			String padding
			{
				[DispId(DispId.IHTMLRULESTYLE_PADDING)]
				get;
				[DispId(DispId.IHTMLRULESTYLE_PADDING)]
				set;
			}
			Boolean textDecorationOverline
			{
				[DispId(DispId.IHTMLRULESTYLE_TEXTDECORATIONOVERLINE)]
				get;
				[DispId(DispId.IHTMLRULESTYLE_TEXTDECORATIONOVERLINE)]
				set;
			}
			Object borderTopColor
			{
				[DispId(DispId.IHTMLRULESTYLE_BORDERTOPCOLOR)]
				get;
				[DispId(DispId.IHTMLRULESTYLE_BORDERTOPCOLOR)]
				set;
			}
			String border
			{
				[DispId(DispId.IHTMLRULESTYLE_BORDER)]
				get;
				[DispId(DispId.IHTMLRULESTYLE_BORDER)]
				set;
			}
			Object paddingRight
			{
				[DispId(DispId.IHTMLRULESTYLE_PADDINGRIGHT)]
				get;
				[DispId(DispId.IHTMLRULESTYLE_PADDINGRIGHT)]
				set;
			}
			String backgroundAttachment
			{
				[DispId(DispId.IHTMLRULESTYLE_BACKGROUNDATTACHMENT)]
				get;
				[DispId(DispId.IHTMLRULESTYLE_BACKGROUNDATTACHMENT)]
				set;
			}
			String borderWidth
			{
				[DispId(DispId.IHTMLRULESTYLE_BORDERWIDTH)]
				get;
				[DispId(DispId.IHTMLRULESTYLE_BORDERWIDTH)]
				set;
			}
			Object borderBottomWidth
			{
				[DispId(DispId.IHTMLRULESTYLE_BORDERBOTTOMWIDTH)]
				get;
				[DispId(DispId.IHTMLRULESTYLE_BORDERBOTTOMWIDTH)]
				set;
			}
			String pageBreakBefore
			{
				[DispId(DispId.IHTMLRULESTYLE_PAGEBREAKBEFORE)]
				get;
				[DispId(DispId.IHTMLRULESTYLE_PAGEBREAKBEFORE)]
				set;
			}
			Object paddingTop
			{
				[DispId(DispId.IHTMLRULESTYLE_PADDINGTOP)]
				get;
				[DispId(DispId.IHTMLRULESTYLE_PADDINGTOP)]
				set;
			}
			String position
			{
				[DispId(DispId.IHTMLRULESTYLE_POSITION)]
				get;
			}
			String textDecoration
			{
				[DispId(DispId.IHTMLRULESTYLE_TEXTDECORATION)]
				get;
				[DispId(DispId.IHTMLRULESTYLE_TEXTDECORATION)]
				set;
			}
			String borderLeftStyle
			{
				[DispId(DispId.IHTMLRULESTYLE_BORDERLEFTSTYLE)]
				get;
				[DispId(DispId.IHTMLRULESTYLE_BORDERLEFTSTYLE)]
				set;
			}
			String backgroundRepeat
			{
				[DispId(DispId.IHTMLRULESTYLE_BACKGROUNDREPEAT)]
				get;
				[DispId(DispId.IHTMLRULESTYLE_BACKGROUNDREPEAT)]
				set;
			}
			String borderRightStyle
			{
				[DispId(DispId.IHTMLRULESTYLE_BORDERRIGHTSTYLE)]
				get;
				[DispId(DispId.IHTMLRULESTYLE_BORDERRIGHTSTYLE)]
				set;
			}
			String textAlign
			{
				[DispId(DispId.IHTMLRULESTYLE_TEXTALIGN)]
				get;
				[DispId(DispId.IHTMLRULESTYLE_TEXTALIGN)]
				set;
			}
			String clip
			{
				[DispId(DispId.IHTMLRULESTYLE_CLIP)]
				get;
				[DispId(DispId.IHTMLRULESTYLE_CLIP)]
				set;
			}
			String listStyleImage
			{
				[DispId(DispId.IHTMLRULESTYLE_LISTSTYLEIMAGE)]
				get;
				[DispId(DispId.IHTMLRULESTYLE_LISTSTYLEIMAGE)]
				set;
			}
			Boolean textDecorationUnderline
			{
				[DispId(DispId.IHTMLRULESTYLE_TEXTDECORATIONUNDERLINE)]
				get;
				[DispId(DispId.IHTMLRULESTYLE_TEXTDECORATIONUNDERLINE)]
				set;
			}
			Object verticalAlign
			{
				[DispId(DispId.IHTMLRULESTYLE_VERTICALALIGN)]
				get;
				[DispId(DispId.IHTMLRULESTYLE_VERTICALALIGN)]
				set;
			}
			Object borderBottomColor
			{
				[DispId(DispId.IHTMLRULESTYLE_BORDERBOTTOMCOLOR)]
				get;
				[DispId(DispId.IHTMLRULESTYLE_BORDERBOTTOMCOLOR)]
				set;
			}
			String fontWeight
			{
				[DispId(DispId.IHTMLRULESTYLE_FONTWEIGHT)]
				get;
				[DispId(DispId.IHTMLRULESTYLE_FONTWEIGHT)]
				set;
			}
			Object fontSize
			{
				[DispId(DispId.IHTMLRULESTYLE_FONTSIZE)]
				get;
				[DispId(DispId.IHTMLRULESTYLE_FONTSIZE)]
				set;
			}
			Object backgroundColor
			{
				[DispId(DispId.IHTMLRULESTYLE_BACKGROUNDCOLOR)]
				get;
				[DispId(DispId.IHTMLRULESTYLE_BACKGROUNDCOLOR)]
				set;
			}
			Object borderRightWidth
			{
				[DispId(DispId.IHTMLRULESTYLE_BORDERRIGHTWIDTH)]
				get;
				[DispId(DispId.IHTMLRULESTYLE_BORDERRIGHTWIDTH)]
				set;
			}
			Object height
			{
				[DispId(DispId.IHTMLRULESTYLE_HEIGHT)]
				get;
				[DispId(DispId.IHTMLRULESTYLE_HEIGHT)]
				set;
			}
			Object backgroundPositionX
			{
				[DispId(DispId.IHTMLRULESTYLE_BACKGROUNDPOSITIONX)]
				get;
				[DispId(DispId.IHTMLRULESTYLE_BACKGROUNDPOSITIONX)]
				set;
			}
			String font
			{
				[DispId(DispId.IHTMLRULESTYLE_FONT)]
				get;
				[DispId(DispId.IHTMLRULESTYLE_FONT)]
				set;
			}
			String background
			{
				[DispId(DispId.IHTMLRULESTYLE_BACKGROUND)]
				get;
				[DispId(DispId.IHTMLRULESTYLE_BACKGROUND)]
				set;
			}
			String listStylePosition
			{
				[DispId(DispId.IHTMLRULESTYLE_LISTSTYLEPOSITION)]
				get;
				[DispId(DispId.IHTMLRULESTYLE_LISTSTYLEPOSITION)]
				set;
			}
			Object lineHeight
			{
				[DispId(DispId.IHTMLRULESTYLE_LINEHEIGHT)]
				get;
				[DispId(DispId.IHTMLRULESTYLE_LINEHEIGHT)]
				set;
			}
			Object color
			{
				[DispId(DispId.IHTMLRULESTYLE_COLOR)]
				get;
				[DispId(DispId.IHTMLRULESTYLE_COLOR)]
				set;
			}
			Object borderLeftWidth
			{
				[DispId(DispId.IHTMLRULESTYLE_BORDERLEFTWIDTH)]
				get;
				[DispId(DispId.IHTMLRULESTYLE_BORDERLEFTWIDTH)]
				set;
			}
			Object paddingLeft
			{
				[DispId(DispId.IHTMLRULESTYLE_PADDINGLEFT)]
				get;
				[DispId(DispId.IHTMLRULESTYLE_PADDINGLEFT)]
				set;
			}
			String borderColor
			{
				[DispId(DispId.IHTMLRULESTYLE_BORDERCOLOR)]
				get;
				[DispId(DispId.IHTMLRULESTYLE_BORDERCOLOR)]
				set;
			}
			String fontStyle
			{
				[DispId(DispId.IHTMLRULESTYLE_FONTSTYLE)]
				get;
				[DispId(DispId.IHTMLRULESTYLE_FONTSTYLE)]
				set;
			}
			Object borderLeftColor
			{
				[DispId(DispId.IHTMLRULESTYLE_BORDERLEFTCOLOR)]
				get;
				[DispId(DispId.IHTMLRULESTYLE_BORDERLEFTCOLOR)]
				set;
			}
			String overflow
			{
				[DispId(DispId.IHTMLRULESTYLE_OVERFLOW)]
				get;
				[DispId(DispId.IHTMLRULESTYLE_OVERFLOW)]
				set;
			}
			String borderRight
			{
				[DispId(DispId.IHTMLRULESTYLE_BORDERRIGHT)]
				get;
				[DispId(DispId.IHTMLRULESTYLE_BORDERRIGHT)]
				set;
			}
			String borderTopStyle
			{
				[DispId(DispId.IHTMLRULESTYLE_BORDERTOPSTYLE)]
				get;
				[DispId(DispId.IHTMLRULESTYLE_BORDERTOPSTYLE)]
				set;
			}
			Object letterSpacing
			{
				[DispId(DispId.IHTMLRULESTYLE_LETTERSPACING)]
				get;
				[DispId(DispId.IHTMLRULESTYLE_LETTERSPACING)]
				set;
			}
			String cursor
			{
				[DispId(DispId.IHTMLRULESTYLE_CURSOR)]
				get;
				[DispId(DispId.IHTMLRULESTYLE_CURSOR)]
				set;
			}
			Boolean textDecorationBlink
			{
				[DispId(DispId.IHTMLRULESTYLE_TEXTDECORATIONBLINK)]
				get;
				[DispId(DispId.IHTMLRULESTYLE_TEXTDECORATIONBLINK)]
				set;
			}
			Object backgroundPositionY
			{
				[DispId(DispId.IHTMLRULESTYLE_BACKGROUNDPOSITIONY)]
				get;
				[DispId(DispId.IHTMLRULESTYLE_BACKGROUNDPOSITIONY)]
				set;
			}
			Boolean textDecorationLineThrough
			{
				[DispId(DispId.IHTMLRULESTYLE_TEXTDECORATIONLINETHROUGH)]
				get;
				[DispId(DispId.IHTMLRULESTYLE_TEXTDECORATIONLINETHROUGH)]
				set;
			}
			String borderLeft
			{
				[DispId(DispId.IHTMLRULESTYLE_BORDERLEFT)]
				get;
				[DispId(DispId.IHTMLRULESTYLE_BORDERLEFT)]
				set;
			}
			String fontFamily
			{
				[DispId(DispId.IHTMLRULESTYLE_FONTFAMILY)]
				get;
				[DispId(DispId.IHTMLRULESTYLE_FONTFAMILY)]
				set;
			}
			Object textIndent
			{
				[DispId(DispId.IHTMLRULESTYLE_TEXTINDENT)]
				get;
				[DispId(DispId.IHTMLRULESTYLE_TEXTINDENT)]
				set;
			}
			Object width
			{
				[DispId(DispId.IHTMLRULESTYLE_WIDTH)]
				get;
				[DispId(DispId.IHTMLRULESTYLE_WIDTH)]
				set;
			}
			String borderStyle
			{
				[DispId(DispId.IHTMLRULESTYLE_BORDERSTYLE)]
				get;
				[DispId(DispId.IHTMLRULESTYLE_BORDERSTYLE)]
				set;
			}
			String filter
			{
				[DispId(DispId.IHTMLRULESTYLE_FILTER)]
				get;
				[DispId(DispId.IHTMLRULESTYLE_FILTER)]
				set;
			}
			Object wordSpacing
			{
				[DispId(DispId.IHTMLRULESTYLE_WORDSPACING)]
				get;
				[DispId(DispId.IHTMLRULESTYLE_WORDSPACING)]
				set;
			}
			Object marginTop
			{
				[DispId(DispId.IHTMLRULESTYLE_MARGINTOP)]
				get;
				[DispId(DispId.IHTMLRULESTYLE_MARGINTOP)]
				set;
			}
			String visibility
			{
				[DispId(DispId.IHTMLRULESTYLE_VISIBILITY)]
				get;
				[DispId(DispId.IHTMLRULESTYLE_VISIBILITY)]
				set;
			}
			String backgroundImage
			{
				[DispId(DispId.IHTMLRULESTYLE_BACKGROUNDIMAGE)]
				get;
				[DispId(DispId.IHTMLRULESTYLE_BACKGROUNDIMAGE)]
				set;
			}
			String pageBreakAfter
			{
				[DispId(DispId.IHTMLRULESTYLE_PAGEBREAKAFTER)]
				get;
				[DispId(DispId.IHTMLRULESTYLE_PAGEBREAKAFTER)]
				set;
			}
			String fontVariant
			{
				[DispId(DispId.IHTMLRULESTYLE_FONTVARIANT)]
				get;
				[DispId(DispId.IHTMLRULESTYLE_FONTVARIANT)]
				set;
			}
			Object borderRightColor
			{
				[DispId(DispId.IHTMLRULESTYLE_BORDERRIGHTCOLOR)]
				get;
				[DispId(DispId.IHTMLRULESTYLE_BORDERRIGHTCOLOR)]
				set;
			}
			String clear
			{
				[DispId(DispId.IHTMLRULESTYLE_CLEAR)]
				get;
				[DispId(DispId.IHTMLRULESTYLE_CLEAR)]
				set;
			}
			Object paddingBottom
			{
				[DispId(DispId.IHTMLRULESTYLE_PADDINGBOTTOM)]
				get;
				[DispId(DispId.IHTMLRULESTYLE_PADDINGBOTTOM)]
				set;
			}
			String listStyle
			{
				[DispId(DispId.IHTMLRULESTYLE_LISTSTYLE)]
				get;
				[DispId(DispId.IHTMLRULESTYLE_LISTSTYLE)]
				set;
			}
			Object borderTopWidth
			{
				[DispId(DispId.IHTMLRULESTYLE_BORDERTOPWIDTH)]
				get;
				[DispId(DispId.IHTMLRULESTYLE_BORDERTOPWIDTH)]
				set;
			}
			Object marginRight
			{
				[DispId(DispId.IHTMLRULESTYLE_MARGINRIGHT)]
				get;
				[DispId(DispId.IHTMLRULESTYLE_MARGINRIGHT)]
				set;
			}
			String margin
			{
				[DispId(DispId.IHTMLRULESTYLE_MARGIN)]
				get;
				[DispId(DispId.IHTMLRULESTYLE_MARGIN)]
				set;
			}
			String textTransform
			{
				[DispId(DispId.IHTMLRULESTYLE_TEXTTRANSFORM)]
				get;
				[DispId(DispId.IHTMLRULESTYLE_TEXTTRANSFORM)]
				set;
			}
			String borderBottom
			{
				[DispId(DispId.IHTMLRULESTYLE_BORDERBOTTOM)]
				get;
				[DispId(DispId.IHTMLRULESTYLE_BORDERBOTTOM)]
				set;
			}
			Boolean textDecorationNone
			{
				[DispId(DispId.IHTMLRULESTYLE_TEXTDECORATIONNONE)]
				get;
				[DispId(DispId.IHTMLRULESTYLE_TEXTDECORATIONNONE)]
				set;
			}
			String cssText
			{
				[DispId(DispId.IHTMLRULESTYLE_CSSTEXT)]
				get;
				[DispId(DispId.IHTMLRULESTYLE_CSSTEXT)]
				set;
			}
			String borderTop
			{
				[DispId(DispId.IHTMLRULESTYLE_BORDERTOP)]
				get;
				[DispId(DispId.IHTMLRULESTYLE_BORDERTOP)]
				set;
			}
			Object top
			{
				[DispId(DispId.IHTMLRULESTYLE_TOP)]
				get;
				[DispId(DispId.IHTMLRULESTYLE_TOP)]
				set;
			}
			String whiteSpace
			{
				[DispId(DispId.IHTMLRULESTYLE_WHITESPACE)]
				get;
				[DispId(DispId.IHTMLRULESTYLE_WHITESPACE)]
				set;
			}
			String display
			{
				[DispId(DispId.IHTMLRULESTYLE_DISPLAY)]
				get;
				[DispId(DispId.IHTMLRULESTYLE_DISPLAY)]
				set;
			}
			String borderBottomStyle
			{
				[DispId(DispId.IHTMLRULESTYLE_BORDERBOTTOMSTYLE)]
				get;
				[DispId(DispId.IHTMLRULESTYLE_BORDERBOTTOMSTYLE)]
				set;
			}
			Object marginLeft
			{
				[DispId(DispId.IHTMLRULESTYLE_MARGINLEFT)]
				get;
				[DispId(DispId.IHTMLRULESTYLE_MARGINLEFT)]
				set;
			}
			String styleFloat
			{
				[DispId(DispId.IHTMLRULESTYLE_STYLEFLOAT)]
				get;
				[DispId(DispId.IHTMLRULESTYLE_STYLEFLOAT)]
				set;
			}
			Object left
			{
				[DispId(DispId.IHTMLRULESTYLE_LEFT)]
				get;
				[DispId(DispId.IHTMLRULESTYLE_LEFT)]
				set;
			}
			Object zIndex
			{
				[DispId(DispId.IHTMLRULESTYLE_ZINDEX)]
				get;
				[DispId(DispId.IHTMLRULESTYLE_ZINDEX)]
				set;
			}
			Object marginBottom
			{
				[DispId(DispId.IHTMLRULESTYLE_MARGINBOTTOM)]
				get;
				[DispId(DispId.IHTMLRULESTYLE_MARGINBOTTOM)]
				set;
			}
			String listStyleType
			{
				[DispId(DispId.IHTMLRULESTYLE_LISTSTYLETYPE)]
				get;
				[DispId(DispId.IHTMLRULESTYLE_LISTSTYLETYPE)]
				set;
			}
		}

		[ComImport()]
		[Guid("3050f4ac-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		public interface IHTMLRuleStyle2
		{
			String lineBreak
			{
				[DispId(DispId.IHTMLRULESTYLE2_LINEBREAK)]
				get;
				[DispId(DispId.IHTMLRULESTYLE2_LINEBREAK)]
				set;
			}
			String unicodeBidi
			{
				[DispId(DispId.IHTMLRULESTYLE2_UNICODEBIDI)]
				get;
				[DispId(DispId.IHTMLRULESTYLE2_UNICODEBIDI)]
				set;
			}
			String imeMode
			{
				[DispId(DispId.IHTMLRULESTYLE2_IMEMODE)]
				get;
				[DispId(DispId.IHTMLRULESTYLE2_IMEMODE)]
				set;
			}
			String textJustify
			{
				[DispId(DispId.IHTMLRULESTYLE2_TEXTJUSTIFY)]
				get;
				[DispId(DispId.IHTMLRULESTYLE2_TEXTJUSTIFY)]
				set;
			}
			Object layoutGridChar
			{
				[DispId(DispId.IHTMLRULESTYLE2_LAYOUTGRIDCHAR)]
				get;
				[DispId(DispId.IHTMLRULESTYLE2_LAYOUTGRIDCHAR)]
				set;
			}
			String layoutGridType
			{
				[DispId(DispId.IHTMLRULESTYLE2_LAYOUTGRIDTYPE)]
				get;
				[DispId(DispId.IHTMLRULESTYLE2_LAYOUTGRIDTYPE)]
				set;
			}
			Object textKashida
			{
				[DispId(DispId.IHTMLRULESTYLE2_TEXTKASHIDA)]
				get;
				[DispId(DispId.IHTMLRULESTYLE2_TEXTKASHIDA)]
				set;
			}
			String borderCollapse
			{
				[DispId(DispId.IHTMLRULESTYLE2_BORDERCOLLAPSE)]
				get;
				[DispId(DispId.IHTMLRULESTYLE2_BORDERCOLLAPSE)]
				set;
			}
			Int32 pixelRight
			{
				[DispId(DispId.IHTMLRULESTYLE2_PIXELRIGHT)]
				get;
				[DispId(DispId.IHTMLRULESTYLE2_PIXELRIGHT)]
				set;
			}
			Object bottom
			{
				[DispId(DispId.IHTMLRULESTYLE2_BOTTOM)]
				get;
				[DispId(DispId.IHTMLRULESTYLE2_BOTTOM)]
				set;
			}
			String textAutospace
			{
				[DispId(DispId.IHTMLRULESTYLE2_TEXTAUTOSPACE)]
				get;
				[DispId(DispId.IHTMLRULESTYLE2_TEXTAUTOSPACE)]
				set;
			}
			String textJustifyTrim
			{
				[DispId(DispId.IHTMLRULESTYLE2_TEXTJUSTIFYTRIM)]
				get;
				[DispId(DispId.IHTMLRULESTYLE2_TEXTJUSTIFYTRIM)]
				set;
			}
			String layoutGridMode
			{
				[DispId(DispId.IHTMLRULESTYLE2_LAYOUTGRIDMODE)]
				get;
				[DispId(DispId.IHTMLRULESTYLE2_LAYOUTGRIDMODE)]
				set;
			}
			Object layoutGridLine
			{
				[DispId(DispId.IHTMLRULESTYLE2_LAYOUTGRIDLINE)]
				get;
				[DispId(DispId.IHTMLRULESTYLE2_LAYOUTGRIDLINE)]
				set;
			}
			Single posRight
			{
				[DispId(DispId.IHTMLRULESTYLE2_POSRIGHT)]
				get;
				[DispId(DispId.IHTMLRULESTYLE2_POSRIGHT)]
				set;
			}
			String accelerator
			{
				[DispId(DispId.IHTMLRULESTYLE2_ACCELERATOR)]
				get;
				[DispId(DispId.IHTMLRULESTYLE2_ACCELERATOR)]
				set;
			}
			String wordBreak
			{
				[DispId(DispId.IHTMLRULESTYLE2_WORDBREAK)]
				get;
				[DispId(DispId.IHTMLRULESTYLE2_WORDBREAK)]
				set;
			}
			String rubyOverhang
			{
				[DispId(DispId.IHTMLRULESTYLE2_RUBYOVERHANG)]
				get;
				[DispId(DispId.IHTMLRULESTYLE2_RUBYOVERHANG)]
				set;
			}
			String tableLayout
			{
				[DispId(DispId.IHTMLRULESTYLE2_TABLELAYOUT)]
				get;
				[DispId(DispId.IHTMLRULESTYLE2_TABLELAYOUT)]
				set;
			}
			String position
			{
				[DispId(DispId.IHTMLRULESTYLE2_POSITION)]
				get;
				[DispId(DispId.IHTMLRULESTYLE2_POSITION)]
				set;
			}
			String direction
			{
				[DispId(DispId.IHTMLRULESTYLE2_DIRECTION)]
				get;
				[DispId(DispId.IHTMLRULESTYLE2_DIRECTION)]
				set;
			}
			String behavior
			{
				[DispId(DispId.IHTMLRULESTYLE2_BEHAVIOR)]
				get;
				[DispId(DispId.IHTMLRULESTYLE2_BEHAVIOR)]
				set;
			}
			String layoutGrid
			{
				[DispId(DispId.IHTMLRULESTYLE2_LAYOUTGRID)]
				get;
				[DispId(DispId.IHTMLRULESTYLE2_LAYOUTGRID)]
				set;
			}
			String overflowY
			{
				[DispId(DispId.IHTMLRULESTYLE2_OVERFLOWY)]
				get;
				[DispId(DispId.IHTMLRULESTYLE2_OVERFLOWY)]
				set;
			}
			Single posBottom
			{
				[DispId(DispId.IHTMLRULESTYLE2_POSBOTTOM)]
				get;
				[DispId(DispId.IHTMLRULESTYLE2_POSBOTTOM)]
				set;
			}
			String rubyAlign
			{
				[DispId(DispId.IHTMLRULESTYLE2_RUBYALIGN)]
				get;
				[DispId(DispId.IHTMLRULESTYLE2_RUBYALIGN)]
				set;
			}
			Object right
			{
				[DispId(DispId.IHTMLRULESTYLE2_RIGHT)]
				get;
				[DispId(DispId.IHTMLRULESTYLE2_RIGHT)]
				set;
			}
			Int32 pixelBottom
			{
				[DispId(DispId.IHTMLRULESTYLE2_PIXELBOTTOM)]
				get;
				[DispId(DispId.IHTMLRULESTYLE2_PIXELBOTTOM)]
				set;
			}
			String rubyPosition
			{
				[DispId(DispId.IHTMLRULESTYLE2_RUBYPOSITION)]
				get;
				[DispId(DispId.IHTMLRULESTYLE2_RUBYPOSITION)]
				set;
			}
			String overflowX
			{
				[DispId(DispId.IHTMLRULESTYLE2_OVERFLOWX)]
				get;
				[DispId(DispId.IHTMLRULESTYLE2_OVERFLOWX)]
				set;
			}
		}

		[ComImport()]
		[Guid("3050f817-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		public interface IHTMLRuleStyle4
		{
			String textOverflow
			{
				[DispId(DispId.IHTMLRULESTYLE4_TEXTOVERFLOW)]
				get;
				[DispId(DispId.IHTMLRULESTYLE4_TEXTOVERFLOW)]
				set;
			}
			Object minHeight
			{
				[DispId(DispId.IHTMLRULESTYLE4_MINHEIGHT)]
				get;
				[DispId(DispId.IHTMLRULESTYLE4_MINHEIGHT)]
				set;
			}
		}

		[ComImport()]
		[Guid("3050f657-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		public interface IHTMLRuleStyle3
		{
			String wordWrap
			{
				[DispId(DispId.IHTMLRULESTYLE3_WORDWRAP)]
				get;
				[DispId(DispId.IHTMLRULESTYLE3_WORDWRAP)]
				set;
			}
			String writingMode
			{
				[DispId(DispId.IHTMLRULESTYLE3_WRITINGMODE)]
				get;
				[DispId(DispId.IHTMLRULESTYLE3_WRITINGMODE)]
				set;
			}
			Object scrollbar3dLightColor
			{
				[DispId(DispId.IHTMLRULESTYLE3_SCROLLBAR3DLIGHTCOLOR)]
				get;
				[DispId(DispId.IHTMLRULESTYLE3_SCROLLBAR3DLIGHTCOLOR)]
				set;
			}
			Object scrollbarTrackColor
			{
				[DispId(DispId.IHTMLRULESTYLE3_SCROLLBARTRACKCOLOR)]
				get;
				[DispId(DispId.IHTMLRULESTYLE3_SCROLLBARTRACKCOLOR)]
				set;
			}
			Object scrollbarFaceColor
			{
				[DispId(DispId.IHTMLRULESTYLE3_SCROLLBARFACECOLOR)]
				get;
				[DispId(DispId.IHTMLRULESTYLE3_SCROLLBARFACECOLOR)]
				set;
			}
			String textAlignLast
			{
				[DispId(DispId.IHTMLRULESTYLE3_TEXTALIGNLAST)]
				get;
				[DispId(DispId.IHTMLRULESTYLE3_TEXTALIGNLAST)]
				set;
			}
			Object scrollbarArrowColor
			{
				[DispId(DispId.IHTMLRULESTYLE3_SCROLLBARARROWCOLOR)]
				get;
				[DispId(DispId.IHTMLRULESTYLE3_SCROLLBARARROWCOLOR)]
				set;
			}
			Object zoom
			{
				[DispId(DispId.IHTMLRULESTYLE3_ZOOM)]
				get;
				[DispId(DispId.IHTMLRULESTYLE3_ZOOM)]
				set;
			}
			Object scrollbarBaseColor
			{
				[DispId(DispId.IHTMLRULESTYLE3_SCROLLBARBASECOLOR)]
				get;
				[DispId(DispId.IHTMLRULESTYLE3_SCROLLBARBASECOLOR)]
				set;
			}
			String layoutFlow
			{
				[DispId(DispId.IHTMLRULESTYLE3_LAYOUTFLOW)]
				get;
				[DispId(DispId.IHTMLRULESTYLE3_LAYOUTFLOW)]
				set;
			}
			Object scrollbarHighlightColor
			{
				[DispId(DispId.IHTMLRULESTYLE3_SCROLLBARHIGHLIGHTCOLOR)]
				get;
				[DispId(DispId.IHTMLRULESTYLE3_SCROLLBARHIGHLIGHTCOLOR)]
				set;
			}
			Object scrollbarShadowColor
			{
				[DispId(DispId.IHTMLRULESTYLE3_SCROLLBARSHADOWCOLOR)]
				get;
				[DispId(DispId.IHTMLRULESTYLE3_SCROLLBARSHADOWCOLOR)]
				set;
			}
			String textUnderlinePosition
			{
				[DispId(DispId.IHTMLRULESTYLE3_TEXTUNDERLINEPOSITION)]
				get;
				[DispId(DispId.IHTMLRULESTYLE3_TEXTUNDERLINEPOSITION)]
				set;
			}
			Object scrollbarDarkShadowColor
			{
				[DispId(DispId.IHTMLRULESTYLE3_SCROLLBARDARKSHADOWCOLOR)]
				get;
				[DispId(DispId.IHTMLRULESTYLE3_SCROLLBARDARKSHADOWCOLOR)]
				set;
			}
			Object textKashidaSpace
			{
				[DispId(DispId.IHTMLRULESTYLE3_TEXTKASHIDASPACE)]
				get;
				[DispId(DispId.IHTMLRULESTYLE3_TEXTKASHIDASPACE)]
				set;
			}
		}


		[Guid("3050F25A-98B5-11CF-BB82-00AA00BDCE0B"), InterfaceTypeAttribute(ComInterfaceType.InterfaceIsDual)]
		public interface IHTMLSelectionObject
		{

			[return: MarshalAs(UnmanagedType.Interface)]
			object CreateRange();


			void Empty();


			void Clear();

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetSelectionType();

		}

		[Guid("3050f230-98b5-11cf-bb82-00aa00bdce0b"), InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIDispatch)]
		public interface IHTMLTextContainer
		{
			[return: MarshalAs(UnmanagedType.IDispatch)]
			object createControlRange();
			int get_ScrollHeight();
			int get_ScrollWidth();
			int get_ScrollTop();
			int get_ScrollLeft();
			void put_ScrollHeight(int i);
			void put_ScrollWidth(int i);
			void put_ScrollTop(int i);
			void put_ScrollLeft(int i);
		}

		[Guid("3050F220-98B5-11CF-BB82-00AA00BDCE0B"), InterfaceTypeAttribute(ComInterfaceType.InterfaceIsDual)]
		public interface IHTMLTxtRange
		{

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetHtmlText();

			/// <summary>
			/// Sets the text contained within the range. 
			/// </summary>
			void SetText([In, MarshalAs(UnmanagedType.BStr)] string p);
			/// <summary>
			/// Retrieves the text contained within the range. 
			/// </summary>
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetText();

			//			string Text
			//			{
			//				[DispId(DispId.IHTMLTXTRANGE_TEXT)]
			//				[return: MarshalAs(UnmanagedType.BStr)]
			//				get;
			//				[DispId(DispId.IHTMLTXTRANGE_TEXT)]
			//				set;
			//			}

			[return: MarshalAs(UnmanagedType.Interface)]
			IHTMLElement ParentElement();

			[return: MarshalAs(UnmanagedType.Interface)]
			IHTMLTxtRange Duplicate();

			[return: MarshalAs(UnmanagedType.Bool)]
			bool InRange(
				[In, MarshalAs(UnmanagedType.Interface)]
				IHTMLTxtRange range);

			[return: MarshalAs(UnmanagedType.Bool)]
			bool IsEqual(
				[In, MarshalAs(UnmanagedType.Interface)]
				IHTMLTxtRange range);


			void ScrollIntoView(
				[In, MarshalAs(UnmanagedType.Bool)]
				bool fStart);


			void Collapse(
				[In, MarshalAs(UnmanagedType.Bool)]
				bool Start);

			[return: MarshalAs(UnmanagedType.Bool)]
			bool Expand(
				[In, MarshalAs(UnmanagedType.BStr)]
				string Unit);

			[return: MarshalAs(UnmanagedType.I4)]
			int Move(
				[In, MarshalAs(UnmanagedType.BStr)]
				string Unit,
				[In, MarshalAs(UnmanagedType.I4)]
				int Count);

			[return: MarshalAs(UnmanagedType.I4)]
			int MoveStart(
				[In, MarshalAs(UnmanagedType.BStr)]
				string Unit,
				[In, MarshalAs(UnmanagedType.I4)]
				int Count);

			[return: MarshalAs(UnmanagedType.I4)]
			int MoveEnd(
				[In, MarshalAs(UnmanagedType.BStr)]
				string Unit,
				[In, MarshalAs(UnmanagedType.I4)]
				int Count);


			void Select();


			void PasteHTML(
				[In, MarshalAs(UnmanagedType.BStr)]
				string html);


			void MoveToElementText(
				[In, MarshalAs(UnmanagedType.Interface)]
				IHTMLElement element);


			void SetEndPoint(
				[In, MarshalAs(UnmanagedType.BStr)]
				string how,
				[In, MarshalAs(UnmanagedType.Interface)]
				IHTMLTxtRange SourceRange);

			[return: MarshalAs(UnmanagedType.I4)]
			int CompareEndPoints(
				[In, MarshalAs(UnmanagedType.BStr)]
				string how,
				[In, MarshalAs(UnmanagedType.Interface)]
				IHTMLTxtRange SourceRange);

			[return: MarshalAs(UnmanagedType.Bool)]
			bool FindText(
				[In, MarshalAs(UnmanagedType.BStr)]
				string String,
				[In, MarshalAs(UnmanagedType.I4)]
				int Count,
				[In, MarshalAs(UnmanagedType.I4)]
				int Flags);


			void MoveToPoint(
				[In, MarshalAs(UnmanagedType.I4)]
				int x,
				[In, MarshalAs(UnmanagedType.I4)]
				int y);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetBookmark();

			[return: MarshalAs(UnmanagedType.Bool)]
			bool MoveToBookmark(
				[In, MarshalAs(UnmanagedType.BStr)]
				string Bookmark);

			[return: MarshalAs(UnmanagedType.Bool)]
			bool QueryCommandSupported(
				[In, MarshalAs(UnmanagedType.BStr)]
				string cmdID);

			[return: MarshalAs(UnmanagedType.Bool)]
			bool QueryCommandEnabled(
				[In, MarshalAs(UnmanagedType.BStr)]
				string cmdID);

			[return: MarshalAs(UnmanagedType.Bool)]
			bool QueryCommandState(
				[In, MarshalAs(UnmanagedType.BStr)]
				string cmdID);

			[return: MarshalAs(UnmanagedType.Bool)]
			bool QueryCommandIndeterm(
				[In, MarshalAs(UnmanagedType.BStr)]
				string cmdID);

			[return: MarshalAs(UnmanagedType.BStr)]
			string QueryCommandText(
				[In, MarshalAs(UnmanagedType.BStr)]
				string cmdID);

			[return: MarshalAs(UnmanagedType.Struct)]
			object QueryCommandValue(
				[In, MarshalAs(UnmanagedType.BStr)]
				string cmdID);

			[return: MarshalAs(UnmanagedType.Bool)]
			bool ExecCommand(
				[In, MarshalAs(UnmanagedType.BStr)]
				string cmdID,
				[In, MarshalAs(UnmanagedType.Bool)]
				bool showUI,
				[In, MarshalAs(UnmanagedType.Struct)]
				object value);

			[return: MarshalAs(UnmanagedType.Bool)]
			bool ExecCommandShowHelp(
				[In, MarshalAs(UnmanagedType.BStr)]
				string cmdID);
		}

		#region HTMLStyle and HTMLStyleSheet interfaces
		[Guid("3050F25E-98B5-11CF-BB82-00AA00BDCE0B"), InterfaceTypeAttribute(ComInterfaceType.InterfaceIsDual)]
		public interface IHTMLStyle
		{


			void SetFontFamily(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetFontFamily();


			void SetFontStyle(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetFontStyle();


			void SetFontObject(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetFontObject();


			void SetFontWeight(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetFontWeight();


			void SetFontSize(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetFontSize();


			void SetFont(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetFont();


			void SetColor(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetColor();


			void SetBackground(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetBackground();


			void SetBackgroundColor(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetBackgroundColor();


			void SetBackgroundImage(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetBackgroundImage();


			void SetBackgroundRepeat(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetBackgroundRepeat();


			void SetBackgroundAttachment(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetBackgroundAttachment();


			void SetBackgroundPosition(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetBackgroundPosition();


			void SetBackgroundPositionX(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetBackgroundPositionX();


			void SetBackgroundPositionY(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetBackgroundPositionY();


			void SetWordSpacing(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetWordSpacing();


			void SetLetterSpacing(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetLetterSpacing();


			void SetTextDecoration(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetTextDecoration();


			void SetTextDecorationNone(
				[In, MarshalAs(UnmanagedType.Bool)]
				bool p);

			[return: MarshalAs(UnmanagedType.Bool)]
			bool GetTextDecorationNone();


			void SetTextDecorationUnderline(
				[In, MarshalAs(UnmanagedType.Bool)]
				bool p);

			[return: MarshalAs(UnmanagedType.Bool)]
			bool GetTextDecorationUnderline();


			void SetTextDecorationOverline(
				[In, MarshalAs(UnmanagedType.Bool)]
				bool p);

			[return: MarshalAs(UnmanagedType.Bool)]
			bool GetTextDecorationOverline();


			void SetTextDecorationLineThrough(
				[In, MarshalAs(UnmanagedType.Bool)]
				bool p);

			[return: MarshalAs(UnmanagedType.Bool)]
			bool GetTextDecorationLineThrough();


			void SetTextDecorationBlink(
				[In, MarshalAs(UnmanagedType.Bool)]
				bool p);

			[return: MarshalAs(UnmanagedType.Bool)]
			bool GetTextDecorationBlink();


			void SetVerticalAlign(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetVerticalAlign();


			void SetTextTransform(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetTextTransform();


			void SetTextAlign(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetTextAlign();


			void SetTextIndent(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetTextIndent();


			void SetLineHeight(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetLineHeight();


			void SetMarginTop(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetMarginTop();


			void SetMarginRight(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetMarginRight();


			void SetMarginBottom(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetMarginBottom();


			void SetMarginLeft(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetMarginLeft();


			void SetMargin(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetMargin();


			void SetPaddingTop(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetPaddingTop();


			void SetPaddingRight(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetPaddingRight();


			void SetPaddingBottom(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetPaddingBottom();


			void SetPaddingLeft(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetPaddingLeft();


			void SetPadding(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetPadding();


			void SetBorder(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetBorder();


			void SetBorderTop(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetBorderTop();


			void SetBorderRight(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetBorderRight();


			void SetBorderBottom(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetBorderBottom();


			void SetBorderLeft(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetBorderLeft();


			void SetBorderColor(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetBorderColor();


			void SetBorderTopColor(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetBorderTopColor();


			void SetBorderRightColor(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetBorderRightColor();


			void SetBorderBottomColor(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetBorderBottomColor();


			void SetBorderLeftColor(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetBorderLeftColor();


			void SetBorderWidth(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetBorderWidth();


			void SetBorderTopWidth(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetBorderTopWidth();


			void SetBorderRightWidth(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetBorderRightWidth();


			void SetBorderBottomWidth(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetBorderBottomWidth();


			void SetBorderLeftWidth(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetBorderLeftWidth();


			void SetBorderStyle(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetBorderStyle();


			void SetBorderTopStyle(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetBorderTopStyle();


			void SetBorderRightStyle(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetBorderRightStyle();


			void SetBorderBottomStyle(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetBorderBottomStyle();


			void SetBorderLeftStyle(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetBorderLeftStyle();


			void SetWidth(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetWidth();


			void SetHeight(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetHeight();


			void SetStyleFloat(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetStyleFloat();


			void SetClear(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetClear();


			void SetDisplay(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetDisplay();


			void SetVisibility(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetVisibility();


			void SetListStyleType(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetListStyleType();


			void SetListStylePosition(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetListStylePosition();


			void SetListStyleImage(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetListStyleImage();


			void SetListStyle(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetListStyle();


			void SetWhiteSpace(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetWhiteSpace();


			void SetTop(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetTop();


			void SetLeft(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetLeft();

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetPosition();


			void SetZIndex(
				[In, MarshalAs(UnmanagedType.Struct)]
				Object p);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetZIndex();


			void SetOverflow(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetOverflow();


			void SetPageBreakBefore(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetPageBreakBefore();


			void SetPageBreakAfter(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetPageBreakAfter();


			void SetCssText(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetCssText();


			void SetPixelTop(
				[In, MarshalAs(UnmanagedType.I4)]
				int p);

			[return: MarshalAs(UnmanagedType.I4)]
			int GetPixelTop();


			void SetPixelLeft(
				[In, MarshalAs(UnmanagedType.I4)]
				int p);

			[return: MarshalAs(UnmanagedType.I4)]
			int GetPixelLeft();


			void SetPixelWidth(
				[In, MarshalAs(UnmanagedType.I4)]
				int p);

			[return: MarshalAs(UnmanagedType.I4)]
			int GetPixelWidth();


			void SetPixelHeight(
				[In, MarshalAs(UnmanagedType.I4)]
				int p);

			[return: MarshalAs(UnmanagedType.I4)]
			int GetPixelHeight();


			void SetPosTop(
				[In, MarshalAs(UnmanagedType.R4)]
				float p);

			[return: MarshalAs(UnmanagedType.R4)]
			float GetPosTop();


			void SetPosLeft(
				[In, MarshalAs(UnmanagedType.R4)]
				float p);

			[return: MarshalAs(UnmanagedType.R4)]
			float GetPosLeft();


			void SetPosWidth(
				[In, MarshalAs(UnmanagedType.R4)]
				float p);

			[return: MarshalAs(UnmanagedType.R4)]
			float GetPosWidth();


			void SetPosHeight(
				[In, MarshalAs(UnmanagedType.R4)]
				float p);

			[return: MarshalAs(UnmanagedType.R4)]
			float GetPosHeight();


			void SetCursor(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetCursor();


			void SetClip(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetClip();


			void SetFilter(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetFilter();


			void SetAttribute(
				[In, MarshalAs(UnmanagedType.BStr)]
				string strAttributeName,
				[In, MarshalAs(UnmanagedType.Struct)]
				Object AttributeValue,
				[In, MarshalAs(UnmanagedType.I4)]
				int lFlags);

			[return: MarshalAs(UnmanagedType.Struct)]
			Object GetAttribute(
				[In, MarshalAs(UnmanagedType.BStr)]
				string strAttributeName,
				[In, MarshalAs(UnmanagedType.I4)]
				int lFlags);

			[return: MarshalAs(UnmanagedType.Bool)]
			bool RemoveAttribute(
				[In, MarshalAs(UnmanagedType.BStr)]
				string strAttributeName,
				[In, MarshalAs(UnmanagedType.I4)]
				int lFlags);

		}

		[Guid("3050F4A2-98B5-11CF-BB82-00AA00BDCE0B"), InterfaceTypeAttribute(ComInterfaceType.InterfaceIsDual)]
		public interface IHTMLStyle2
		{
			[DispId(DispId.IHTMLSTYLE2_TABLELAYOUT)]
			void SettableLayout([In, MarshalAs(UnmanagedType.BStr)] string p);

			[DispId(DispId.IHTMLSTYLE2_TABLELAYOUT)]
			[return: MarshalAs(UnmanagedType.BStr)]
			string GettableLayout();

			[DispId(DispId.IHTMLSTYLE2_BORDERCOLLAPSE)]
			void SetBorderCollapse([In, MarshalAs(UnmanagedType.BStr)] string p);

			[DispId(DispId.IHTMLSTYLE2_BORDERCOLLAPSE)]
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetBorderCollapse();

			[DispId(DispId.IHTMLSTYLE2_DIRECTION)]
			void SetDirection([In, MarshalAs(UnmanagedType.BStr)] string p);

			[DispId(DispId.IHTMLSTYLE2_DIRECTION)]
			string GetDirection();

			[DispId(DispId.IHTMLSTYLE2_DIRECTION)]
			void SetBehavior([In, MarshalAs(UnmanagedType.BStr)] string p);

			[DispId(DispId.IHTMLSTYLE2_DIRECTION)]
			string GetBehavior();

			[DispId(DispId.IHTMLSTYLE2_SETEXPRESSION)]
			void SetExpression(
				[In, MarshalAs(UnmanagedType.BStr)] string propname,
				[In, MarshalAs(UnmanagedType.BStr)] string expression,
				[In, MarshalAs(UnmanagedType.BStr)] string language);

			[DispId(DispId.IHTMLSTYLE2_GETEXPRESSION)]
			string getExpression([In, MarshalAs(UnmanagedType.BStr)] string propname);

			[DispId(DispId.IHTMLSTYLE2_GETEXPRESSION)]
			bool RemoveExpression([In, MarshalAs(UnmanagedType.BStr)] string propname);

			[DispId(DispId.IHTMLSTYLE2_POSITION)]
			void SetPosition([In, MarshalAs(UnmanagedType.BStr)] string p);

			[DispId(DispId.IHTMLSTYLE2_POSITION)]
			string GetPosition();

			[DispId(DispId.IHTMLSTYLE2_UNICODEBIDI)]
			void SetUnicodeBidi([In, MarshalAs(UnmanagedType.BStr)] string p);

			[DispId(DispId.IHTMLSTYLE2_UNICODEBIDI)]
			string unicodeBidi();

			[DispId(DispId.IHTMLSTYLE2_BOTTOM)]
			void SetBottom([In] object p);

			[DispId(DispId.IHTMLSTYLE2_BOTTOM)]
			object bottom();

			[DispId(DispId.IHTMLSTYLE2_RIGHT)]
			void SetRight([In] object p);

			[DispId(DispId.IHTMLSTYLE2_RIGHT)]
			object GetRight();

			[DispId(DispId.IHTMLSTYLE2_PIXELBOTTOM)]
			void SetPixelBottom([In] int p);
			[DispId(DispId.IHTMLSTYLE2_PIXELBOTTOM)]
			int GetPixelBottom();
			[DispId(DispId.IHTMLSTYLE2_PIXELRIGHT)]
			void SetPixelRight([In] int p);
			[DispId(DispId.IHTMLSTYLE2_PIXELRIGHT)]
			int GetPixelRight();
			[DispId(DispId.IHTMLSTYLE2_POSBOTTOM)]
			void SetPosBottom([In] float p);
			[DispId(DispId.IHTMLSTYLE2_POSBOTTOM)]
			float GetPosBottom();
			[DispId(DispId.IHTMLSTYLE2_IMEMODE)]
			void SetPosRight([In] float p);
			[DispId(DispId.IHTMLSTYLE2_IMEMODE)]
			float GetPosRight();
			[DispId(DispId.IHTMLSTYLE2_IMEMODE)]
			void SetImeMode([In, MarshalAs(UnmanagedType.BStr)] string p);
			[DispId(DispId.IHTMLSTYLE2_IMEMODE)]
			string GetImeMode();
			[DispId(DispId.IHTMLSTYLE2_RUBYALIGN)]
			void SetRubyAlign([In, MarshalAs(UnmanagedType.BStr)] string p);
			[DispId(DispId.IHTMLSTYLE2_RUBYALIGN)]
			string GetRubyAlign();
			[DispId(DispId.IHTMLSTYLE2_RUBYPOSITION)]
			void SetRubyPosition([In, MarshalAs(UnmanagedType.BStr)] string p);
			[DispId(DispId.IHTMLSTYLE2_RUBYPOSITION)]
			string GetRubyPosition();
			[DispId(DispId.IHTMLSTYLE2_RUBYOVERHANG)]
			void SetRubyOverhang([In, MarshalAs(UnmanagedType.BStr)] string p);
			[DispId(DispId.IHTMLSTYLE2_RUBYOVERHANG)]
			string GetRubyOverhang();
			[DispId(DispId.IHTMLSTYLE2_LAYOUTGRIDCHAR)]
			void SetLayoutGridChar([In] object p);
			[DispId(DispId.IHTMLSTYLE2_LAYOUTGRIDCHAR)]
			object GetLayoutGridChar();
			[DispId(DispId.IHTMLSTYLE2_LAYOUTGRIDLINE)]
			void SetLayoutGridLine([In] object p);
			[DispId(DispId.IHTMLSTYLE2_LAYOUTGRIDLINE)]
			object GetLayoutGridLine();
			[DispId(DispId.IHTMLSTYLE2_LAYOUTGRIDMODE)]
			void SetLayoutGridMode([In, MarshalAs(UnmanagedType.BStr)] string p);
			[DispId(DispId.IHTMLSTYLE2_LAYOUTGRIDMODE)]
			void GetLayoutGridMode();
			[DispId(DispId.IHTMLSTYLE2_LAYOUTGRIDTYPE)]
			void SetLayoutGridType([In, MarshalAs(UnmanagedType.BStr)] string p);
			[DispId(DispId.IHTMLSTYLE2_LAYOUTGRIDTYPE)]
			string GetLayoutGridType();
			[DispId(DispId.IHTMLSTYLE2_LAYOUTGRID)]
			void SetLayoutGrid([In, MarshalAs(UnmanagedType.BStr)] string p);
			[DispId(DispId.IHTMLSTYLE2_LAYOUTGRID)]
			string GetLayoutGrid();
			[DispId(DispId.IHTMLSTYLE2_WORDBREAK)]
			void SetWordBreak([In, MarshalAs(UnmanagedType.BStr)] string p);
			[DispId(DispId.IHTMLSTYLE2_WORDBREAK)]
			string GetWordBreak();
			[DispId(DispId.IHTMLSTYLE2_LINEBREAK)]
			void SetLineBreak([In, MarshalAs(UnmanagedType.BStr)] string p);
			[DispId(DispId.IHTMLSTYLE2_LINEBREAK)]
			string GetLineBreak();
			[DispId(DispId.IHTMLSTYLE2_TEXTJUSTIFY)]
			void SetTextJustify([In, MarshalAs(UnmanagedType.BStr)] string p);
			[DispId(DispId.IHTMLSTYLE2_TEXTJUSTIFY)]
			string GetTextJustify();
			[DispId(DispId.IHTMLSTYLE2_TEXTJUSTIFYTRIM)]
			void SetTextJustifyTrim([In, MarshalAs(UnmanagedType.BStr)] string p);
			[DispId(DispId.IHTMLSTYLE2_TEXTJUSTIFYTRIM)]
			string GetTextJustifyTrim();
			[DispId(DispId.IHTMLSTYLE2_TEXTKASHIDA)]
			void SetTextKashida([In] object p);
			[DispId(DispId.IHTMLSTYLE2_TEXTKASHIDA)]
			string GetTextKashida();
			[DispId(DispId.IHTMLSTYLE2_TEXTAUTOSPACE)]
			void SetTextAutospace([In, MarshalAs(UnmanagedType.BStr)] string p);
			[DispId(DispId.IHTMLSTYLE2_TEXTAUTOSPACE)]
			string GetTextAutospace();
			[DispId(DispId.IHTMLSTYLE2_OVERFLOWX)]
			void SetOverflowX([In, MarshalAs(UnmanagedType.BStr)] string p);
			[DispId(DispId.IHTMLSTYLE2_OVERFLOWX)]
			string GetOverflowX();
			[DispId(DispId.IHTMLSTYLE2_OVERFLOWY)]
			void SetOverflowY([In, MarshalAs(UnmanagedType.BStr)] string p);
			[DispId(DispId.IHTMLSTYLE2_OVERFLOWY)]
			string GetOverflowY();
			[DispId(DispId.IHTMLSTYLE2_ACCELERATOR)]
			void SetAccelerator([In, MarshalAs(UnmanagedType.BStr)] string p);
			[DispId(DispId.IHTMLSTYLE2_ACCELERATOR)]
			string GetAccelerator();
		}

		[Guid("3050F656-98B5-11CF-BB82-00AA00BDCE0B"), InterfaceTypeAttribute(ComInterfaceType.InterfaceIsDual)]
		public interface IHTMLStyle3
		{
			[DispId(DispId.IHTMLSTYLE3_LAYOUTFLOW)]
			void layoutFlow([In, MarshalAs(UnmanagedType.BStr)] string p);
			[DispId(DispId.IHTMLSTYLE3_LAYOUTFLOW)]
			string layoutFlow();
			[DispId(DispId.IHTMLSTYLE3_ZOOM)]
			void zoom([In] object p);
			[DispId(DispId.IHTMLSTYLE3_ZOOM)]
			object zoom();
			[DispId(DispId.IHTMLSTYLE3_WORDWRAP)]
			void SetWordWrap([In, MarshalAs(UnmanagedType.BStr)] string p);
			[DispId(DispId.IHTMLSTYLE3_WORDWRAP)]
			string GetWordWrap();
			[DispId(DispId.IHTMLSTYLE3_TEXTUNDERLINEPOSITION)]
			void SetTextUnderlinePosition([In, MarshalAs(UnmanagedType.BStr)] string p);
			[DispId(DispId.IHTMLSTYLE3_TEXTUNDERLINEPOSITION)]
			string GetTextUnderlinePosition();
			[DispId(DispId.IHTMLSTYLE3_SCROLLBARBASECOLOR)]
			void SetScrollbarBaseColor([In] object p);
			[DispId(DispId.IHTMLSTYLE3_SCROLLBARBASECOLOR)]
			object GetScrollbarBaseColor();
			[DispId(DispId.IHTMLSTYLE3_SCROLLBARFACECOLOR)]
			void SetScrollbarFaceColor([In] object p);
			[DispId(DispId.IHTMLSTYLE3_SCROLLBARFACECOLOR)]
			object GetScrollbarFaceColor();
			[DispId(DispId.IHTMLSTYLE3_SCROLLBAR3DLIGHTCOLOR)]
			void SetScrollbar3dLightColor([In] object p);
			[DispId(DispId.IHTMLSTYLE3_SCROLLBAR3DLIGHTCOLOR)]
			object GetScrollbar3dLightColor();
			[DispId(DispId.IHTMLSTYLE3_SCROLLBARSHADOWCOLOR)]
			void SetScrollbarShadowColor([In] object p);
			[DispId(DispId.IHTMLSTYLE3_SCROLLBARSHADOWCOLOR)]
			object GetScrollbarShadowColor();
			[DispId(DispId.IHTMLSTYLE3_SCROLLBARHIGHLIGHTCOLOR)]
			void SetScrollbarHighlightColor([In] object p);
			[DispId(DispId.IHTMLSTYLE3_SCROLLBARHIGHLIGHTCOLOR)]
			object GetScrollbarHighlightColor();
			[DispId(DispId.IHTMLSTYLE3_SCROLLBARDARKSHADOWCOLOR)]
			void SetScrollbarDarkShadowColor([In] object p);
			[DispId(DispId.IHTMLSTYLE3_SCROLLBARDARKSHADOWCOLOR)]
			object GetScrollbarDarkShadowColor();
			[DispId(DispId.IHTMLSTYLE3_SCROLLBARARROWCOLOR)]
			void SetScrollbarArrowColor([In] object p);
			[DispId(DispId.IHTMLSTYLE3_SCROLLBARARROWCOLOR)]
			object GetScrollbarArrowColor();
			[DispId(DispId.IHTMLSTYLE3_SCROLLBARTRACKCOLOR)]
			void SetScrollbarTrackColor([In] object p);
			[DispId(DispId.IHTMLSTYLE3_SCROLLBARTRACKCOLOR)]
			object GetScrollbarTrackColor();
			[DispId(DispId.IHTMLSTYLE3_WRITINGMODE)]
			void SetWritingMode([In, MarshalAs(UnmanagedType.BStr)] string p);
			[DispId(DispId.IHTMLSTYLE3_WRITINGMODE)]
			string GetWritingMode();
			[DispId(DispId.IHTMLSTYLE3_TEXTALIGNLAST)]
			void SetTextAlignLast([In, MarshalAs(UnmanagedType.BStr)] string p);
			[DispId(DispId.IHTMLSTYLE3_TEXTALIGNLAST)]
			string GetTextAlignLast();
			[DispId(DispId.IHTMLSTYLE3_TEXTKASHIDASPACE)]
			void SetTextKashidaSpace([In] object p);
			[DispId(DispId.IHTMLSTYLE3_TEXTKASHIDASPACE)]
			object GetTextKashidaSpace();
		}

		[Guid("3050F816-98B5-11CF-BB82-00AA00BDCE0B"), InterfaceTypeAttribute(ComInterfaceType.InterfaceIsDual)]
		public interface IHTMLStyle4 : IDispatch
		{
			[DispId(DispId.IHTMLSTYLE4_TEXTOVERFLOW)]
			void SetTextOverflow([In, MarshalAs(UnmanagedType.BStr)] string p);
			[DispId(DispId.IHTMLSTYLE4_TEXTOVERFLOW)]
			string GettextOverflow();
			[DispId(DispId.IHTMLSTYLE4_MINHEIGHT)]
			void SetMinHeight([In] object p);
			[DispId(DispId.IHTMLSTYLE4_MINHEIGHT)]
			object GetmMinHeight();
		}



		[ComImport()]
		[Guid("3050f375-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		public interface IHTMLStyleElement
		{
			String readyState
			{
				[DispId(DispId.IHTMLSTYLEELEMENT_READYSTATE)]
				get;
			}
			Object onerror
			{
				[DispId(DispId.IHTMLSTYLEELEMENT_ONERROR)]
				get;
				[DispId(DispId.IHTMLSTYLEELEMENT_ONERROR)]
				set;
			}
			Boolean disabled
			{
				[DispId(DispId.IHTMLSTYLEELEMENT_DISABLED)]
				get;
				[DispId(DispId.IHTMLSTYLEELEMENT_DISABLED)]
				set;
			}
			String type
			{
				[DispId(DispId.IHTMLSTYLEELEMENT_TYPE)]
				get;
				[DispId(DispId.IHTMLSTYLEELEMENT_TYPE)]
				set;
			}
			Object onload
			{
				[DispId(DispId.IHTMLSTYLEELEMENT_ONLOAD)]
				get;
				[DispId(DispId.IHTMLSTYLEELEMENT_ONLOAD)]
				set;
			}
			String media
			{
				[DispId(DispId.IHTMLSTYLEELEMENT_MEDIA)]
				get;
				[DispId(DispId.IHTMLSTYLEELEMENT_MEDIA)]
				set;
			}
			Object onreadystatechange
			{
				[DispId(DispId.IHTMLSTYLEELEMENT_ONREADYSTATECHANGE)]
				get;
				[DispId(DispId.IHTMLSTYLEELEMENT_ONREADYSTATECHANGE)]
				set;
			}
			IHTMLStyleSheet styleSheet
			{
				[DispId(DispId.IHTMLSTYLEELEMENT_STYLESHEET)]
				get;
			}
		}


		[Guid("3050F2E3-98B5-11CF-BB82-00AA00BDCE0B"), InterfaceTypeAttribute(ComInterfaceType.InterfaceIsDual)]
		public interface IHTMLStyleSheet
		{


			void SetTitle(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetTitle();

			[return: MarshalAs(UnmanagedType.Interface)]
			IHTMLStyleSheet GetParentStyleSheet();

			[return: MarshalAs(UnmanagedType.Interface)]
			IHTMLElement GetOwningElement();


			void SetDisabled(
				[In, MarshalAs(UnmanagedType.Bool)]
				bool p);

			[return: MarshalAs(UnmanagedType.Bool)]
			bool GetDisabled();

			[return: MarshalAs(UnmanagedType.Bool)]
			bool GetReadOnly();

			[return: MarshalAs(UnmanagedType.Interface)]
			IHTMLStyleSheetsCollection GetImports();


			void SetHref(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetHref();

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetStyleSheetType();

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetId();

			[return: MarshalAs(UnmanagedType.I4)]
			int AddImport(
				[In, MarshalAs(UnmanagedType.BStr)]
				string bstrURL,
				[In, MarshalAs(UnmanagedType.I4)]
				int lIndex);

			[return: MarshalAs(UnmanagedType.I4)]
			int AddRule(
				[In, MarshalAs(UnmanagedType.BStr)]
				string bstrSelector,
				[In, MarshalAs(UnmanagedType.BStr)]
				string bstrStyle,
				[In, MarshalAs(UnmanagedType.I4)]
				int lIndex);


			void RemoveImport(
				[In, MarshalAs(UnmanagedType.I4)]
				int lIndex);


			void RemoveRule(
				[In, MarshalAs(UnmanagedType.I4)]
				int lIndex);


			void SetMedia(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetMedia();


			void SetCssText(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetCssText();

			[return: MarshalAs(UnmanagedType.Interface)]
			IHTMLStyleSheetRulesCollection GetRules();

		}

		[ComImport()]
		[Guid("3050f37e-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		public interface IHTMLStyleSheetsCollection
		{
			[DispId(DispId.IHTMLSTYLESHEETSCOLLECTION_ITEM)]
			Object Item(Object pvarIndex);

			Object _newEnum
			{
				[return: MarshalAs(UnmanagedType.Interface)]
				[DispId(DispId.IHTMLSTYLESHEETSCOLLECTION__NEWENUM)]
				get;
			}

			Int32 Length
			{
				[DispId(DispId.IHTMLSTYLESHEETSCOLLECTION_LENGTH)]
				get;
			}
		}

		[Guid("3050F357-98B5-11CF-BB82-00AA00BDCE0B"), InterfaceTypeAttribute(ComInterfaceType.InterfaceIsDual)]
		public interface IHTMLStyleSheetRule
		{


			void SetSelectorText(
				[In, MarshalAs(UnmanagedType.BStr)]
				string p);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetSelectorText();

			[return: MarshalAs(UnmanagedType.Interface)]
			IHTMLRuleStyle GetStyle();

			[return: MarshalAs(UnmanagedType.Bool)]
			bool GetReadOnly();

		}


		[Guid("3050F2E5-98B5-11CF-BB82-00AA00BDCE0B"), InterfaceTypeAttribute(ComInterfaceType.InterfaceIsDual)]
		public interface IHTMLStyleSheetRulesCollection
		{

			[return: MarshalAs(UnmanagedType.I4)]
			int GetLength();

			[return: MarshalAs(UnmanagedType.Interface)]
			IHTMLStyleSheetRule Item(
				[In, MarshalAs(UnmanagedType.I4)]
				int index);

		}

		#endregion

		[ComImport()]
		[Guid("332c4427-26cb-11d0-b483-00c04fd90119")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		public interface IHTMLWindow2
		{

			[DispId(DispId.IHTMLWINDOW2_CLEARTIMEOUT)]
			void clearTimeout(Int32 timerID);

			[DispId(DispId.IHTMLWINDOW2_ALERT)]
			void alert(String message);

			[DispId(DispId.IHTMLWINDOW2_CONFIRM)]
			Boolean confirm(String message);

			[DispId(DispId.IHTMLWINDOW2_PROMPT)]
			[return: MarshalAs(UnmanagedType.Struct)]
			object prompt(
				[In, MarshalAs(UnmanagedType.BStr)]
				string message,
				[In, MarshalAs(UnmanagedType.BStr)]
				string defstr);

			[DispId(DispId.IHTMLWINDOW2_CLOSE)]
			void close();

			[DispId(DispId.IHTMLWINDOW2_OPEN)]
			IHTMLWindow2 open(String url, String name, String features, Boolean replace);


			[DispId(DispId.IHTMLWINDOW2_SHOWMODALDIALOG)]
			Object showModalDialog(String dialog, Object varArgIn, Object varOptions);

			[DispId(DispId.IHTMLWINDOW2_SHOWHELP)]
			void showHelp(String helpURL, Object helpArg, String features);

			[DispId(DispId.IHTMLWINDOW2_FOCUS)]
			void focus();


			[DispId(DispId.IHTMLWINDOW2_SCROLL)]
			void scroll(Int32 x, Int32 y);


			[DispId(DispId.IHTMLWINDOW2_CLEARINTERVAL)]
			void clearInterval(Int32 timerID);

			[DispId(DispId.IHTMLWINDOW2_EXECSCRIPT)]
			Object execScript([In, MarshalAs(UnmanagedType.BStr)] String code, [In, MarshalAs(UnmanagedType.BStr), Optional] String language);

			[DispId(DispId.IHTMLWINDOW2_TOSTRING)]
			String toString();

			[DispId(DispId.IHTMLWINDOW2_SCROLLBY)]
			void scrollBy(Int32 x, Int32 y);

			[DispId(DispId.IHTMLWINDOW2_SCROLLTO)]
			void scrollTo(Int32 x, Int32 y);

			[DispId(DispId.IHTMLWINDOW2_MOVETO)]
			void moveTo(Int32 x, Int32 y);


			[DispId(DispId.IHTMLWINDOW2_RESIZETO)]


			Object onerror
			{
				[DispId(DispId.IHTMLWINDOW2_ONERROR)]
				get;
				[DispId(DispId.IHTMLWINDOW2_ONERROR)]
				set;
			}
			Object onfocus
			{
				[DispId(DispId.IHTMLWINDOW2_ONFOCUS)]
				get;
				[DispId(DispId.IHTMLWINDOW2_ONFOCUS)]
				set;
			}
			Boolean closed
			{
				[DispId(DispId.IHTMLWINDOW2_CLOSED)]
				get;
			}
			IOmNavigator clientInformation
			{
				[DispId(DispId.IHTMLWINDOW2_CLIENTINFORMATION)]
				get;
			}
			IHTMLOptionElementFactory Option
			{
				[DispId(DispId.IHTMLWINDOW2_OPTION)]
				get;
			}
			IHTMLWindow2 self
			{
				[DispId(DispId.IHTMLWINDOW2_SELF)]
				get;
			}
			Object onbeforeunload
			{
				[DispId(DispId.IHTMLWINDOW2_ONBEFOREUNLOAD)]
				get;
				[DispId(DispId.IHTMLWINDOW2_ONBEFOREUNLOAD)]
				set;
			}
			String defaultStatus
			{
				[DispId(DispId.IHTMLWINDOW2_DEFAULTSTATUS)]
				get;
				[DispId(DispId.IHTMLWINDOW2_DEFAULTSTATUS)]
				set;
			}
			IOmHistory history
			{
				[DispId(DispId.IHTMLWINDOW2_HISTORY)]
				get;
			}
			String status
			{
				[DispId(DispId.IHTMLWINDOW2_STATUS)]
				get;
				[DispId(DispId.IHTMLWINDOW2_STATUS)]
				set;
			}
			Object offscreenBuffering
			{
				[DispId(DispId.IHTMLWINDOW2_OFFSCREENBUFFERING)]
				get;
				[DispId(DispId.IHTMLWINDOW2_OFFSCREENBUFFERING)]
				set;
			}
			Object external
			{
				[DispId(DispId.IHTMLWINDOW2_EXTERNAL)]
				get;
			}
			Object onunload
			{
				[DispId(DispId.IHTMLWINDOW2_ONUNLOAD)]
				get;
				[DispId(DispId.IHTMLWINDOW2_ONUNLOAD)]
				set;
			}
			String name
			{
				[DispId(DispId.IHTMLWINDOW2_NAME)]
				get;
				[DispId(DispId.IHTMLWINDOW2_NAME)]
				set;
			}
			IHTMLWindow2 parent
			{
				[DispId(DispId.IHTMLWINDOW2_PARENT)]
				get;
			}
			Object onresize
			{
				[DispId(DispId.IHTMLWINDOW2_ONRESIZE)]
				get;
				[DispId(DispId.IHTMLWINDOW2_ONRESIZE)]
				set;
			}
			IOmNavigator navigator
			{
				[DispId(DispId.IHTMLWINDOW2_NAVIGATOR)]
				get;
			}
			IHTMLLocation location
			{
				[DispId(DispId.IHTMLWINDOW2_LOCATION)]
				get;
			}
			Object _newEnum
			{
				[DispId(DispId.IHTMLWINDOW2__NEWENUM)]
				get;
			}
			Object opener
			{
				[DispId(DispId.IHTMLWINDOW2_OPENER)]
				get;
				[DispId(DispId.IHTMLWINDOW2_OPENER)]
				set;
			}
			IHTMLDocument2 document
			{
				[DispId(DispId.IHTMLWINDOW2_DOCUMENT)]
				get;
			}
			IHTMLFramesCollection2 frames
			{
				[DispId(DispId.IHTMLWINDOW2_FRAMES)]
				get;
			}
			Object onscroll
			{
				[DispId(DispId.IHTMLWINDOW2_ONSCROLL)]
				get;
				[DispId(DispId.IHTMLWINDOW2_ONSCROLL)]
				set;
			}
			IHTMLWindow2 top
			{
				[DispId(DispId.IHTMLWINDOW2_TOP)]
				get;
			}
			IHTMLImageElementFactory Image
			{
				[DispId(DispId.IHTMLWINDOW2_IMAGE)]
				get;
			}
			Object onhelp
			{
				[DispId(DispId.IHTMLWINDOW2_ONHELP)]
				get;
				[DispId(DispId.IHTMLWINDOW2_ONHELP)]
				set;
			}
			Object onblur
			{
				[DispId(DispId.IHTMLWINDOW2_ONBLUR)]
				get;
				[DispId(DispId.IHTMLWINDOW2_ONBLUR)]
				set;
			}
			IHTMLEventObj @event
			{
				[DispId(DispId.IHTMLWINDOW2_EVENT)]
				get;
			}
			Object onload
			{
				[DispId(DispId.IHTMLWINDOW2_ONLOAD)]
				get;
				[DispId(DispId.IHTMLWINDOW2_ONLOAD)]
				set;
			}
			IHTMLScreen screen
			{
				[DispId(DispId.IHTMLWINDOW2_SCREEN)]
				get;
			}
			IHTMLWindow2 window
			{
				[DispId(DispId.IHTMLWINDOW2_WINDOW)]
				get;
			}
		}

		[ComImport()]
		[Guid("3050f4ae-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		public interface IHTMLWindow3
		{
			[DispId(DispId.IHTMLWINDOW3_ATTACHEVENT)]
			Boolean attachEvent(String @event, Object pDisp);

			[DispId(DispId.IHTMLWINDOW3_DETACHEVENT)]
			void detachEvent(String @event, Object pDisp);

			[DispId(DispId.IHTMLWINDOW3_SETTIMEOUT)]
			Int32 setTimeout(object expression, Int32 msec, Object language);
			// [MarshalAs(UnmanagedType.FunctionPtr)] IntPtr

			[DispId(DispId.IHTMLWINDOW3_SETINTERVAL)]
			Int32 setInterval(Object expression, Int32 msec, Object language);

			[DispId(DispId.IHTMLWINDOW3_PRINT)]
			void print();

			[DispId(DispId.IHTMLWINDOW3_SHOWMODELESSDIALOG)]
			IHTMLWindow2 showModelessDialog(String url, Object varArgIn, Object options);

			Int32 screenTop
			{
				[DispId(DispId.IHTMLWINDOW3_SCREENTOP)]
				get;
			}
			IHTMLDataTransfer clipboardData
			{
				[DispId(DispId.IHTMLWINDOW3_CLIPBOARDDATA)]
				get;
			}
			Object onbeforeprint
			{
				[DispId(DispId.IHTMLWINDOW3_ONBEFOREPRINT)]
				get;
				[DispId(DispId.IHTMLWINDOW3_ONBEFOREPRINT)]
				set;
			}
			Object onafterprint
			{
				[DispId(DispId.IHTMLWINDOW3_ONAFTERPRINT)]
				get;
				[DispId(DispId.IHTMLWINDOW3_ONAFTERPRINT)]
				set;
			}
			Int32 screenLeft
			{
				[DispId(DispId.IHTMLWINDOW3_SCREENLEFT)]
				get;
			}
		}

		/// <exclude />
		[ComImport()]
		[Guid("FECEAAA5-8405-11cf-8BA1-00AA00476DA6")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		public interface IOmNavigator
		{
			/// <exclude />
			[DispId(DispId.IOMNAVIGATOR_JAVAENABLED)]
			Boolean javaEnabled();

			/// <exclude />
			[DispId(DispId.IOMNAVIGATOR_TAINTENABLED)]
			Boolean taintEnabled();

			/// <exclude />
			[DispId(DispId.IOMNAVIGATOR_TOSTRING)]
			String toString();

			/// <exclude />
			String systemLanguage
			{
				[DispId(DispId.IOMNAVIGATOR_SYSTEMLANGUAGE)]
				get;
			}
			/// <exclude />
			String cpuClass
			{
				[DispId(DispId.IOMNAVIGATOR_CPUCLASS)]
				get;
			}
			/// <exclude />
			IHTMLOpsProfile userProfile
			{
				[DispId(DispId.IOMNAVIGATOR_USERPROFILE)]
				get;
			}
			/// <exclude />
			Boolean onLine
			{
				[DispId(DispId.IOMNAVIGATOR_ONLINE)]
				get;
			}
			/// <exclude />
			Int32 connectionSpeed
			{
				[DispId(DispId.IOMNAVIGATOR_CONNECTIONSPEED)]
				get;
			}
			/// <exclude />
			String appName
			{
				[DispId(DispId.IOMNAVIGATOR_APPNAME)]
				get;
			}
			/// <exclude />
			String appMinorVersion
			{
				[DispId(DispId.IOMNAVIGATOR_APPMINORVERSION)]
				get;
			}
			/// <exclude />
			String userLanguage
			{
				[DispId(DispId.IOMNAVIGATOR_USERLANGUAGE)]
				get;
			}
			/// <exclude />
			IHTMLOpsProfile opsProfile
			{
				[DispId(DispId.IOMNAVIGATOR_OPSPROFILE)]
				get;
			}
			/// <exclude />
			IHTMLMimeTypesCollection mimeTypes
			{
				[DispId(DispId.IOMNAVIGATOR_MIMETYPES)]
				get;
			}
			/// <exclude />
			String appVersion
			{
				[DispId(DispId.IOMNAVIGATOR_APPVERSION)]
				get;
			}
			/// <exclude />
			String appCodeName
			{
				[DispId(DispId.IOMNAVIGATOR_APPCODENAME)]
				get;
			}
			/// <exclude />
			String platform
			{
				[DispId(DispId.IOMNAVIGATOR_PLATFORM)]
				get;
			}
			/// <exclude />
			String browserLanguage
			{
				[DispId(DispId.IOMNAVIGATOR_BROWSERLANGUAGE)]
				get;
			}
			/// <exclude />
			String userAgent
			{
				[DispId(DispId.IOMNAVIGATOR_USERAGENT)]
				get;
			}
			/// <exclude />
			Boolean cookieEnabled
			{
				[DispId(DispId.IOMNAVIGATOR_COOKIEENABLED)]
				get;
			}
			/// <exclude />
			IHTMLPluginsCollection plugins
			{
				[DispId(DispId.IOMNAVIGATOR_PLUGINS)]
				get;
			}
		}


		/// <exclude />
		[ComImport()]
		[Guid("3050f6cf-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		public interface IHTMLWindow4
		{
			/// <exclude />
			[DispId(DispId.IHTMLWINDOW4_CREATEPOPUP)]
			Object createPopup(Object varArgIn);

			/// <exclude />
			IHTMLFrameBase frameElement
			{
				[DispId(DispId.IHTMLWINDOW4_FRAMEELEMENT)]
				get;
			}
		}

		/// <exclude />
		[ComImport()]
		[Guid("3050f38c-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		public interface IHTMLOptionElementFactory
		{
			/// <exclude />
			[DispId(DispId.IHTMLOPTIONELEMENTFACTORY_CREATE)]
			IHTMLOptionElement create(Object text, Object value, Object defaultselected, Object selected);

		}

		/// <exclude />
		[ComImport()]
		[Guid("FECEAAA2-8405-11cf-8BA1-00AA00476DA6")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		public interface IOmHistory
		{
			/// <exclude />
			[DispId(DispId.IOMHISTORY_BACK)]
			void back(Object pvargdistance);

			/// <exclude />
			[DispId(DispId.IOMHISTORY_FORWARD)]
			void forward(Object pvargdistance);

			/// <exclude />
			[DispId(DispId.IOMHISTORY_GO)]
			void go(Object pvargdistance);

			/// <exclude />
			Int16 length
			{
				[DispId(DispId.IOMHISTORY_LENGTH)]
				get;
			}
		}

		/// <exclude />
		[ComImport()]
		[Guid("3050f38e-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		public interface IHTMLImageElementFactory
		{
			/// <exclude />
			[DispId(DispId.IHTMLIMAGEELEMENTFACTORY_CREATE)]
			IHTMLImgElement create(Object width, Object height);

		}

		/// <exclude />
		[ComImport()]
		[Guid("3050f240-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		public interface IHTMLImgElement
		{
			/// <exclude />
			Object onabort
			{
				[DispId(DispId.IHTMLIMGELEMENT_ONABORT)]
				get;
				[DispId(DispId.IHTMLIMGELEMENT_ONABORT)]
				set;
			}
			/// <exclude />
			String lowsrc
			{
				[DispId(DispId.IHTMLIMGELEMENT_LOWSRC)]
				get;
				[DispId(DispId.IHTMLIMGELEMENT_LOWSRC)]
				set;
			}
			/// <exclude />
			String useMap
			{
				[DispId(DispId.IHTMLIMGELEMENT_USEMAP)]
				get;
				[DispId(DispId.IHTMLIMGELEMENT_USEMAP)]
				set;
			}
			String vrml
			{
				[DispId(DispId.IHTMLIMGELEMENT_VRML)]
				get;
				[DispId(DispId.IHTMLIMGELEMENT_VRML)]
				set;
			}
			/// <exclude />
			String alt
			{
				[DispId(DispId.IHTMLIMGELEMENT_ALT)]
				get;
				[DispId(DispId.IHTMLIMGELEMENT_ALT)]
				set;
			}
			/// <exclude />
			String readyState
			{
				[DispId(DispId.IHTMLIMGELEMENT_READYSTATE)]
				get;
			}
			/// <exclude />
			Boolean complete
			{
				[DispId(DispId.IHTMLIMGELEMENT_COMPLETE)]
				get;
			}
			/// <exclude />
			String name
			{
				[DispId(DispId.IHTMLIMGELEMENT_NAME)]
				get;
				[DispId(DispId.IHTMLIMGELEMENT_NAME)]
				set;
			}
			/// <exclude />
			String fileUpdatedDate
			{
				[DispId(DispId.IHTMLIMGELEMENT_FILEUPDATEDDATE)]
				get;
			}
			/// <exclude />
			Int32 width
			{
				[DispId(DispId.IHTMLIMGELEMENT_WIDTH)]
				get;
				[DispId(DispId.IHTMLIMGELEMENT_WIDTH)]
				set;
			}
			/// <exclude />
			String start
			{
				[DispId(DispId.IHTMLIMGELEMENT_START)]
				get;
				[DispId(DispId.IHTMLIMGELEMENT_START)]
				set;
			}
			/// <exclude />
			String nameProp
			{
				[DispId(DispId.IHTMLIMGELEMENT_NAMEPROP)]
				get;
			}
			/// <exclude />
			String src
			{
				[DispId(DispId.IHTMLIMGELEMENT_SRC)]
				get;
				[DispId(DispId.IHTMLIMGELEMENT_SRC)]
				set;
			}
			/// <exclude />
			String fileModifiedDate
			{
				[DispId(DispId.IHTMLIMGELEMENT_FILEMODIFIEDDATE)]
				get;
			}
			/// <exclude />
			Object loop
			{
				[DispId(DispId.IHTMLIMGELEMENT_LOOP)]
				get;
				[DispId(DispId.IHTMLIMGELEMENT_LOOP)]
				set;
			}
			/// <exclude />
			Int32 vspace
			{
				[DispId(DispId.IHTMLIMGELEMENT_VSPACE)]
				get;
				[DispId(DispId.IHTMLIMGELEMENT_VSPACE)]
				set;
			}
			/// <exclude />
			String fileSize
			{
				[DispId(DispId.IHTMLIMGELEMENT_FILESIZE)]
				get;
			}
			/// <exclude />
			String align
			{
				[DispId(DispId.IHTMLIMGELEMENT_ALIGN)]
				get;
				[DispId(DispId.IHTMLIMGELEMENT_ALIGN)]
				set;
			}
			/// <exclude />
			String dynsrc
			{
				[DispId(DispId.IHTMLIMGELEMENT_DYNSRC)]
				get;
				[DispId(DispId.IHTMLIMGELEMENT_DYNSRC)]
				set;
			}
			/// <exclude />
			String fileCreatedDate
			{
				[DispId(DispId.IHTMLIMGELEMENT_FILECREATEDDATE)]
				get;
			}
			/// <exclude />
			Object onerror
			{
				[DispId(DispId.IHTMLIMGELEMENT_ONERROR)]
				get;
				[DispId(DispId.IHTMLIMGELEMENT_ONERROR)]
				set;
			}
			/// <exclude />
			Boolean isMap
			{
				[DispId(DispId.IHTMLIMGELEMENT_ISMAP)]
				get;
				[DispId(DispId.IHTMLIMGELEMENT_ISMAP)]
				set;
			}
			/// <exclude />
			Object onload
			{
				[DispId(DispId.IHTMLIMGELEMENT_ONLOAD)]
				get;
				[DispId(DispId.IHTMLIMGELEMENT_ONLOAD)]
				set;
			}
			/// <exclude />
			Int32 height
			{
				[DispId(DispId.IHTMLIMGELEMENT_HEIGHT)]
				get;
				[DispId(DispId.IHTMLIMGELEMENT_HEIGHT)]
				set;
			}
			/// <exclude />
			String protocol
			{
				[DispId(DispId.IHTMLIMGELEMENT_PROTOCOL)]
				get;
			}
			/// <exclude />
			String href
			{
				[DispId(DispId.IHTMLIMGELEMENT_HREF)]
				get;
			}
			/// <exclude />
			String mimeType
			{
				[DispId(DispId.IHTMLIMGELEMENT_MIMETYPE)]
				get;
			}
			/// <exclude />
			Int32 hspace
			{
				[DispId(DispId.IHTMLIMGELEMENT_HSPACE)]
				get;
				[DispId(DispId.IHTMLIMGELEMENT_HSPACE)]
				set;
			}
			/// <exclude />
			Object border
			{
				[DispId(DispId.IHTMLIMGELEMENT_BORDER)]
				get;
				[DispId(DispId.IHTMLIMGELEMENT_BORDER)]
				set;
			}
		}

		/// <exclude />
		[ComImport()]
		[Guid("3050f211-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		public interface IHTMLOptionElement
		{
			/// <exclude />
			String text
			{
				[DispId(DispId.IHTMLOPTIONELEMENT_TEXT)]
				get;
				[DispId(DispId.IHTMLOPTIONELEMENT_TEXT)]
				set;
			}
			/// <exclude />
			String value
			{
				[DispId(DispId.IHTMLOPTIONELEMENT_VALUE)]
				get;
				[DispId(DispId.IHTMLOPTIONELEMENT_VALUE)]
				set;
			}
			/// <exclude />
			IHTMLFormElement form
			{
				[DispId(DispId.IHTMLOPTIONELEMENT_FORM)]
				get;
			}
			/// <exclude />
			Boolean defaultSelected
			{
				[DispId(DispId.IHTMLOPTIONELEMENT_DEFAULTSELECTED)]
				get;
				[DispId(DispId.IHTMLOPTIONELEMENT_DEFAULTSELECTED)]
				set;
			}
			/// <exclude />
			Int32 index
			{
				[DispId(DispId.IHTMLOPTIONELEMENT_INDEX)]
				get;
				[DispId(DispId.IHTMLOPTIONELEMENT_INDEX)]
				set;
			}
			/// <exclude />
			Boolean selected
			{
				[DispId(DispId.IHTMLOPTIONELEMENT_SELECTED)]
				get;
				[DispId(DispId.IHTMLOPTIONELEMENT_SELECTED)]
				set;
			}
		}

		/// <exclude />
		[ComImport()]
		[Guid("3050f3fc-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		public interface IHTMLMimeTypesCollection
		{
			/// <exclude />
			Int32 length
			{
				[DispId(DispId.IHTMLMIMETYPESCOLLECTION_LENGTH)]
				get;
			}
		}

		/// <exclude />
		[ComImport()]
		[Guid("3050f401-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		public interface IHTMLOpsProfile
		{
			/// <exclude />
			[DispId(DispId.IHTMLOPSPROFILE_ADDREQUEST)]
			Boolean addRequest(String name, Object reserved);

			/// <exclude />
			[DispId(DispId.IHTMLOPSPROFILE_CLEARREQUEST)]
			void clearRequest();

			/// <exclude />
			[DispId(DispId.IHTMLOPSPROFILE_DOREQUEST)]
			void doRequest(Object usage, Object fname, Object domain, Object path, Object expire, Object reserved);

			/// <exclude />
			[DispId(DispId.IHTMLOPSPROFILE_GETATTRIBUTE)]
			String getAttribute(String name);

			/// <exclude />
			[DispId(DispId.IHTMLOPSPROFILE_SETATTRIBUTE)]
			Boolean setAttribute(String name, String value, Object prefs);

			/// <exclude />
			[DispId(DispId.IHTMLOPSPROFILE_COMMITCHANGES)]
			Boolean commitChanges();

			/// <exclude />
			[DispId(DispId.IHTMLOPSPROFILE_ADDREADREQUEST)]
			Boolean addReadRequest(String name, Object reserved);

			/// <exclude />
			[DispId(DispId.IHTMLOPSPROFILE_DOREADREQUEST)]
			void doReadRequest(Object usage, Object fname, Object domain, Object path, Object expire, Object reserved);

			/// <exclude />
			[DispId(DispId.IHTMLOPSPROFILE_DOWRITEREQUEST)]
			Boolean doWriteRequest();

		}

		/// <exclude />
		[ComImport()]
		[Guid("3050f3fd-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		public interface IHTMLPluginsCollection
		{
			/// <exclude />
			[DispId(DispId.IHTMLPLUGINSCOLLECTION_REFRESH)]
			void refresh(Boolean reload);

			/// <exclude />
			Int32 length
			{
				[DispId(DispId.IHTMLPLUGINSCOLLECTION_LENGTH)]
				get;
			}
		}
		/// <exclude />
		[ComImport()]
		[Guid("3050f35c-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		public interface IHTMLScreen
		{
			/// <exclude />
			Int32 updateInterval
			{
				[DispId(DispId.IHTMLSCREEN_UPDATEINTERVAL)]
				get;
				[DispId(DispId.IHTMLSCREEN_UPDATEINTERVAL)]
				set;
			}
			/// <exclude />
			Int32 width
			{
				[DispId(DispId.IHTMLSCREEN_WIDTH)]
				get;
			}
			/// <exclude />
			Int32 bufferDepth
			{
				[DispId(DispId.IHTMLSCREEN_BUFFERDEPTH)]
				get;
				[DispId(DispId.IHTMLSCREEN_BUFFERDEPTH)]
				set;
			}
			/// <exclude />
			Int32 colorDepth
			{
				[DispId(DispId.IHTMLSCREEN_COLORDEPTH)]
				get;
			}
			/// <exclude />
			Boolean fontSmoothingEnabled
			{
				[DispId(DispId.IHTMLSCREEN_FONTSMOOTHINGENABLED)]
				get;
			}
			/// <exclude />
			Int32 availWidth
			{
				[DispId(DispId.IHTMLSCREEN_AVAILWIDTH)]
				get;
			}
			/// <exclude />
			Int32 availHeight
			{
				[DispId(DispId.IHTMLSCREEN_AVAILHEIGHT)]
				get;
			}
			/// <exclude />
			Int32 height
			{
				[DispId(DispId.IHTMLSCREEN_HEIGHT)]
				get;
			}
		}



		/// <exclude />
		[ComImport()]
		[Guid("3050f4b3-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		public interface IHTMLDataTransfer
		{
			/// <exclude />
			[DispId(DispId.IHTMLDATATRANSFER_SETDATA)]
			Boolean setData(String format, Object data);
			/// <exclude />
			[DispId(DispId.IHTMLDATATRANSFER_GETDATA)]
			Object getData(String format);
			/// <exclude />
			[DispId(DispId.IHTMLDATATRANSFER_CLEARDATA)]
			Boolean clearData(String format);
			/// <exclude />
			String dropEffect
			{
				[DispId(DispId.IHTMLDATATRANSFER_DROPEFFECT)]
				get;
				[DispId(DispId.IHTMLDATATRANSFER_DROPEFFECT)]
				set;
			}
			/// <exclude />
			String effectAllowed
			{
				[DispId(DispId.IHTMLDATATRANSFER_EFFECTALLOWED)]
				get;
				[DispId(DispId.IHTMLDATATRANSFER_EFFECTALLOWED)]
				set;
			}
		}
		/// <exclude />
		[ComImport(),
			GuidAttribute("79EAC9E1-BAF9-11CE-8C82-00AA004BA90B"),
			InterfaceType(ComInterfaceType.InterfaceIsIUnknown)
			]
		public interface IInternetBindInfo
		{
			/// <exclude />
			void GetBindInfo(out uint grfBINDF, ref BINDINFO
				pbindinfo);
			/// <exclude />
			void GetBindString(uint ulStringType, ref string ppwzStr, uint
				cEl, ref
				uint pcElFetched);
		}

		/// <exclude />
		[ComImport(),
			GuidAttribute("79EAC9E4-BAF9-11CE-8C82-00AA004BA90B"),
			InterfaceType(ComInterfaceType.InterfaceIsIUnknown)
			]
		public interface IInternetProtocol
		{
			#region "IInternetProtocolRoot Methods"

			[PreserveSigAttribute]
			int Start([MarshalAs(UnmanagedType.LPWStr)]string szUrl, IInternetProtocolSink pOIProtSink,
			   IInternetBindInfo pOIBindInfo, uint grfPI, uint
			   dwReserved);
			void Continue([MarshalAs(UnmanagedType.I4)]ref PROTOCOLDATA pProtocolData);
			void Abort(int hrReason, uint dwOptions);
			void Terminate(uint dwOptions);
			void Suspend();
			void Resume();
			#endregion

			#region "IInternetProtocol Methods"
			[PreserveSigAttribute]
			int Read(IntPtr pv, uint cb, out uint pcbRead);
			void Seek(LARGE_INTEGER dlibMove, uint dwOrigin, out
				ULARGE_INTEGER
				plibNewPosition);
			void LockRequest(uint dwOptions);
			void UnlockRequest();
			#endregion
		}

		/// <exclude />
		[ComImport(),
		 InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
		 Guid("79eac9ec-baf9-11ce-8c82-00aa004ba90b")]
		public interface IInternetProtocolInfo
		{
			void ParseUrl([MarshalAs(UnmanagedType.LPWStr)]string pwzUrl, [MarshalAs(UnmanagedType.U4)] PARSEACTION
				parseAction, UInt32 dwParseFlags,
				IntPtr pwzResult, UInt32 cchResult, out UInt32 pcchResult, UInt32
				dwReserved);

			void CombineUrl([MarshalAs(UnmanagedType.LPWStr)]string pwzBaseUrl, [MarshalAs(UnmanagedType.LPWStr)]string pwzRelativeUrl, UInt32
				dwCombineFlags,
				IntPtr pwzResult, UInt32 cchResult, out UInt32 pcchResult, UInt32
				dwReserved);

			void CompareUrl([MarshalAs(UnmanagedType.LPWStr)]string pwzUrl1, [MarshalAs(UnmanagedType.LPWStr)]string pwzUrl2, UInt32 dwCompareFlags);

			void QueryInfo([MarshalAs(UnmanagedType.LPWStr)]string pwzUrl, [MarshalAs(UnmanagedType.U4)] QUERYOPTION
				queryOption, UInt32 dwQueryFlags,
				IntPtr pBuffer, UInt32 cbBuffer, ref UInt32 cbBuf, UInt32
				dwReserved);
		}

		[
			ComImport(),
			GuidAttribute("79EAC9E5-BAF9-11CE-8C82-00AA004BA90B"),
			InterfaceType(ComInterfaceType.InterfaceIsIUnknown)
			]
		public interface IInternetProtocolSink
		{
			void Switch(ref PROTOCOLDATA pProtocolData);
			void ReportProgress([MarshalAs(UnmanagedType.I4)]BINDSTATUS ulStatusCode, string szStatusText);
			void ReportData([MarshalAs(UnmanagedType.I4)]BSCF grfBSCF, uint ulProgress, uint
				ulProgressMax);
			void ReportResult(int hrResult, uint dwError, string
				szResult);
		}

		[Guid("79eac9e7-baf9-11ce-8c82-00aa004ba90b"), InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IInternetSession
		{
			[PreserveSig]
			int RegisterNameSpace(
				[In] IClassFactory classFactory,
				[In] ref Guid rclsid,
				[In, MarshalAs(UnmanagedType.LPWStr)]
				string pwzProtocol,
				[In] int cPatterns,
				[In, MarshalAs(UnmanagedType.LPWStr)]
				string ppwzPatterns,
				[In] int dwReserved);

			[PreserveSig]
			int UnregisterNameSpace(
				[In] IClassFactory classFactory,
				[In, MarshalAs(UnmanagedType.LPWStr)]
				string pszProtocol);

			[PreserveSig]
			int RegisterMimeFilter(
							[In] IClassFactory pCF,
							[In] ref Guid rclsid,
							[In] string pwzType);

			[PreserveSig]
			int UnregisterMimeFilter(
							[In] IClassFactory pCF,
							[In] string pwzType);

			[PreserveSig]
			int CreateBinding(
							[In] IntPtr pBC,
							[In] string szUrl,
							[In] IUnknown pUnkOuter,
							[Out] IUnknown ppUnk,
							[Out] IInternetProtocol ppOInetProt,
							[In] int dwOption);

			[PreserveSig]
			int SetSessionOption(
							[In] int dwOption,
							[In] object pBuffer,
							[In] int dwBufferLength,
							[In] int dwReserved);

			[PreserveSig]
			int GetSessionOption(
							[In] int dwOption,
							[Out][In] object pBuffer,
							[Out][In] int pdwBufferLength,
							[In] int dwReserved);

		}

		[Guid("0000000F-0000-0000-C000-000000000046"), InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IMoniker
		{

			// IPersistStream methods
			[return: MarshalAs(UnmanagedType.I4)]
			[PreserveSig]
			int IsDirty();

			void Load(
				[In, MarshalAs(UnmanagedType.Interface)]
				IStream pstm);

			void Save(
				[In, MarshalAs(UnmanagedType.Interface)]
				IStream pstm,
				[In, MarshalAs(UnmanagedType.Bool)]
				bool fClearDirty);

			[return: MarshalAs(UnmanagedType.I8)]
			long GetSizeMax();
			// End IPersistStream methods

			[return: MarshalAs(UnmanagedType.Interface)]
			object BindToObject(
				[In, MarshalAs(UnmanagedType.Interface)]
				object pbc,
				[In, MarshalAs(UnmanagedType.Interface)]
				IMoniker pMkToLeft,
				[In]
				ref Guid riidResult);

			[return: MarshalAs(UnmanagedType.Interface)]
			object BindToStorage(
				[In, MarshalAs(UnmanagedType.Interface)]
				object pbc,
				[In, MarshalAs(UnmanagedType.Interface)]
				IMoniker pMkToLeft,
				[In]
				ref Guid riidResult);

			[return: MarshalAs(UnmanagedType.Interface)]
			IMoniker Reduce(
				[In, MarshalAs(UnmanagedType.Interface)]
				object pbc,
				[In, MarshalAs(UnmanagedType.I4)]
				int dwReduceHowFar,
				[In, Out, MarshalAs(UnmanagedType.Interface)]
				IMoniker pMkToLeft);

			[return: MarshalAs(UnmanagedType.Interface)]
			IMoniker Reduce(
				[In, MarshalAs(UnmanagedType.Interface)]
				IMoniker pMkRight,
				[In, MarshalAs(UnmanagedType.Bool)]
				bool fOnlyIfNotGeneric);

			[return: MarshalAs(UnmanagedType.Interface)]
			object Reduce(
				[In, MarshalAs(UnmanagedType.Bool)]
				bool fForward);


			void IsEqual(
				[In, MarshalAs(UnmanagedType.Interface)]
				IMoniker pOtherMoniker);

			[return: MarshalAs(UnmanagedType.I4)]
			[PreserveSig]
			int Hash();


			void IsRunning(
				[In, MarshalAs(UnmanagedType.Interface)]
				object pbc,
				[In, MarshalAs(UnmanagedType.Interface)]
				IMoniker pMkToLeft,
				[In, MarshalAs(UnmanagedType.Interface)]
				IMoniker pMkNewlyRunning);

			[return: MarshalAs(UnmanagedType.LPStruct)]
			FILETIME GetTimeOfLastChange(
				[In, MarshalAs(UnmanagedType.Interface)]
				object pbc,
				[In, MarshalAs(UnmanagedType.Interface)]
				IMoniker pMkToLeft);

			[return: MarshalAs(UnmanagedType.Interface)]
			IMoniker Inverse();

			[return: MarshalAs(UnmanagedType.Interface)]
			IMoniker CommonPrefixWith(
				[In, MarshalAs(UnmanagedType.Interface)]
				IMoniker pMkOther);

			[return: MarshalAs(UnmanagedType.Interface)]
			IMoniker RelativePathTo(
				[In, MarshalAs(UnmanagedType.Interface)]
				IMoniker pMkOther);

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetDisplayName(
				[In, MarshalAs(UnmanagedType.Interface)]
				object pbc,
				[In, MarshalAs(UnmanagedType.Interface)]
				IMoniker pMkOther);

			[return: MarshalAs(UnmanagedType.Interface)]
			IMoniker ParseDisplayName(
				[In, MarshalAs(UnmanagedType.Interface)]
				object pbc,
				[In, MarshalAs(UnmanagedType.Interface)]
				IMoniker pMkToLeft,
				[In, MarshalAs(UnmanagedType.BStr)]
				string pszDisplayName,
				[Out, MarshalAs(UnmanagedType.LPArray)]
				int[] pchEaten);

			[return: MarshalAs(UnmanagedType.I4)]
			[PreserveSig]
			int IsSystemMoniker();
		}

		#region OLE interfaces
		[Guid("00000118-0000-0000-C000-000000000046"), InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IOleClientSite
		{

			[return: MarshalAs(UnmanagedType.I4)]
			[PreserveSig]
			int SaveObject();

			[return: MarshalAs(UnmanagedType.I4)]
			[PreserveSig]
			int GetMoniker(
				[In, MarshalAs(UnmanagedType.U4)]          int dwAssign,
				[In, MarshalAs(UnmanagedType.U4)]          int dwWhichMoniker,
				[Out, MarshalAs(UnmanagedType.Interface)] out object ppmk);

			[return: MarshalAs(UnmanagedType.I4)]
			[PreserveSig]
			int GetContainer(
				[Out]
				out IOleContainer ppContainer);

			[return: MarshalAs(UnmanagedType.I4)]
			[PreserveSig]
			int ShowObject();

			[return: MarshalAs(UnmanagedType.I4)]
			[PreserveSig]
			int OnShowWindow(
				[In, MarshalAs(UnmanagedType.I4)] int fShow);

			[return: MarshalAs(UnmanagedType.I4)]
			[PreserveSig]
			int RequestNewObjectLayout();
		}

		[ComImport(), Guid("B722BCCB-4E68-101B-A2BC-00AA00404770"),
			InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IOleCommandTarget
		{

			[return: MarshalAs(UnmanagedType.I4)]
			[PreserveSig]
			int QueryStatus(
				ref Guid pguidCmdGroup,
				int cCmds,
				[In, Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] OLECMD[] prgCmds,
				[In, Out] OLECMDTEXT pCmdText);

			[return: MarshalAs(UnmanagedType.I4)]
			[PreserveSig]
			int Exec(
				ref Guid pguidCmdGroup,
				int nCmdID,
				int nCmdexecopt,
				[In, MarshalAs(UnmanagedType.LPStruct)]  OLEVARIANT pvaIn,
				[Out, MarshalAs(UnmanagedType.LPStruct)] OLEVARIANT pvaOut);
		}


		[Guid("0000011B-0000-0000-C000-000000000046"), InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IOleContainer
		{


			void ParseDisplayName(
				[In, MarshalAs(UnmanagedType.Interface)] object pbc,
				[In, MarshalAs(UnmanagedType.BStr)]      string pszDisplayName,
				[Out, MarshalAs(UnmanagedType.LPArray)] int[] pchEaten,
				[Out, MarshalAs(UnmanagedType.LPArray)] object[] ppmkOut);


			void EnumObjects(
				[In, MarshalAs(UnmanagedType.U4)]        int grfFlags,
				//[Out, MarshalAs(UnmanagedType.LPArray)] object[] ppenum);
				//[Out, MarshalAs(UnmanagedType.LPStruct)] OLEVARIANT ppenum);
				[Out] out IEnumUnknown ppenum);

			void LockContainer(
				[In, MarshalAs(UnmanagedType.I4)] int fLock);
		}

		[ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("0000010E-0000-0000-C000-000000000046")]
		public interface IOleDataObject
		{
			void GetData([In] ref FORMATETC format, [Out] out STGMEDIUM medium);
			void GetDataHere([In] ref FORMATETC format, ref STGMEDIUM medium);
			[PreserveSig]
			int QueryGetData([In] ref FORMATETC format);
			[PreserveSig]
			int GetCanonicalFormatEtc([In] ref FORMATETC formatIn, out FORMATETC formatOut);
			void SetData([In] ref FORMATETC formatIn, [In] ref STGMEDIUM medium, [MarshalAs(UnmanagedType.Bool)] bool release);
			IEnumFORMATETC EnumFormatEtc(DATADIR direction);
			[PreserveSig]
			int DAdvise([In] ref FORMATETC pFormatetc, ADVF advf, IAdviseSink adviseSink, out int connection);
			void DUnadvise(int connection);
			[PreserveSig]
			int EnumDAdvise(out IEnumSTATDATA enumAdvise);
		}

		[ComImport(), Guid("B722BCC7-4E68-101B-A2BC-00AA00404770"), InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IOleDocumentSite
		{

			[return: MarshalAs(UnmanagedType.I4)]
			[PreserveSig]
			int ActivateMe(
				[In, MarshalAs(UnmanagedType.Interface)]
				IOleDocumentView pViewToActivate);


		}

		[Guid("B722BCC6-4E68-101B-A2BC-00AA00404770"), InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IOleDocumentView
		{


			void SetInPlaceSite(
				[In, MarshalAs(UnmanagedType.Interface)]
				IOleInPlaceSite pIPSite);

			[return: MarshalAs(UnmanagedType.Interface)]
			IOleInPlaceSite GetInPlaceSite();

			[return: MarshalAs(UnmanagedType.Interface)]
			object GetDocument();


			void SetRect(
				[In]
				RECT prcView);


			void GetRect(
				[Out]
				RECT prcView);


			void SetRectComplex(
				[In]
				RECT prcView,
				[In]
				RECT prcHScroll,
				[In]
				RECT prcVScroll,
				[In]
				RECT prcSizeBox);


			void Show(
				[In, MarshalAs(UnmanagedType.I4)]
				int fShow);


			void UIActivate(
				[In, MarshalAs(UnmanagedType.I4)]
				int fUIActivate);


			void Open();


			void Close(
				[In, MarshalAs(UnmanagedType.I8)]
				long dwReserved);


			void SaveViewState(
				[In, MarshalAs(UnmanagedType.Interface)]
				IStream pstm);


			void ApplyViewState(
				[In, MarshalAs(UnmanagedType.Interface)]
				IStream pstm);


			void Clone(
				[In, MarshalAs(UnmanagedType.Interface)]
				IOleInPlaceSite pIPSiteNew,
				[Out, MarshalAs(UnmanagedType.LPArray)]
				IOleDocumentView[] ppViewNew);


		}

		[ComImport(), Guid("00000121-0000-0000-C000-000000000046"), InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IOleDropSource
		{

			[return: MarshalAs(UnmanagedType.I4)]
			[PreserveSig]
			int QueryContinueDrag(
				[In, MarshalAs(UnmanagedType.I4)]
				int fEscapePressed,
				[In, MarshalAs(UnmanagedType.U4)]
				int grfKeyState);

			[return: MarshalAs(UnmanagedType.I4)]
			[PreserveSig]
			int GiveFeedback(
				[In, MarshalAs(UnmanagedType.U4)]
				int dwEffect);
		}

		[ComImport(), Guid("00000122-0000-0000-C000-000000000046"), InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IOleDropTarget
		{

			[PreserveSig]
			int OleDragEnter(
				//[In, MarshalAs(UnmanagedType.Interface)]
				IntPtr pDataObj,
				[In, MarshalAs(UnmanagedType.U4)]
				int grfKeyState,
				[In]
				long pt,
				[In, Out]
				ref int pdwEffect);

			[PreserveSig]
			int OleDragOver(
				[In, MarshalAs(UnmanagedType.U4)]
				int grfKeyState,
				[In]
				long pt,
				[In, Out]
				ref int pdwEffect);

			[PreserveSig]
			int OleDragLeave();

			[PreserveSig]
			int OleDrop(
				//[In, MarshalAs(UnmanagedType.Interface)]
				IntPtr pDataObj,
				[In, MarshalAs(UnmanagedType.U4)]
				int grfKeyState,
				[In]
				long pt,
				[In, Out]
				ref int pdwEffect);
		}

		[ComImport(), Guid("00000117-0000-0000-C000-000000000046"), InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IOleInPlaceActiveObject
		{

			[return: MarshalAs(UnmanagedType.I4)]
			[PreserveSig]
			int GetWindow(out IntPtr hwnd);


			void ContextSensitiveHelp(
				[In, MarshalAs(UnmanagedType.I4)]
				int fEnterMode);

			[return: MarshalAs(UnmanagedType.I4)]
			[PreserveSig]
			int TranslateAccelerator(
				[In, MarshalAs(UnmanagedType.LPStruct)] COMMSG lpmsg);


			void OnFrameWindowActivate(
				[In, MarshalAs(UnmanagedType.I4)]
				int fActivate);


			void OnDocWindowActivate(
				[In, MarshalAs(UnmanagedType.I4)]
				int fActivate);


			void ResizeBorder(
				[In]
				RECT prcBorder,
				[In]
				IOleInPlaceUIWindow pUIWindow,
				[In, MarshalAs(UnmanagedType.I4)]
				int fFrameWindow);


			void EnableModeless(
				[In, MarshalAs(UnmanagedType.I4)]
				int fEnable);


		}

		[ComImport(), Guid("00000116-0000-0000-C000-000000000046"), InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IOleInPlaceFrame
		{

			IntPtr GetWindow();

			void ContextSensitiveHelp(
				[In, MarshalAs(UnmanagedType.I4)]
				int fEnterMode);


			void GetBorder(
				[Out]
				RECT lprectBorder);


			void RequestBorderSpace(
				[In]
				RECT pborderwidths);


			void SetBorderSpace(
				[In]
				RECT pborderwidths);


			void SetActiveObject(
				[In, MarshalAs(UnmanagedType.Interface)]
				IOleInPlaceActiveObject pActiveObject,
				[In, MarshalAs(UnmanagedType.LPWStr)]
				string pszObjName);


			void InsertMenus(
				[In]
				IntPtr hmenuShared,
				[In, Out]
				tagOleMenuGroupWidths lpMenuWidths);


			void SetMenu(
				[In]
				IntPtr hmenuShared,
				[In]
				IntPtr holemenu,
				[In]
				IntPtr hwndActiveObject);


			void RemoveMenus(
				[In]
				IntPtr hmenuShared);


			void SetStatusText(
				[In, MarshalAs(UnmanagedType.BStr)]
				string pszStatusText);


			void EnableModeless(
				[In, MarshalAs(UnmanagedType.I4)]
				int fEnable);

			[return: MarshalAs(UnmanagedType.I4)]
			[PreserveSig]
			int TranslateAccelerator(
				[In, MarshalAs(UnmanagedType.LPStruct)]
				COMMSG lpmsg,
				[In, MarshalAs(UnmanagedType.U2)]
				short wID);


		}

		[ComImport(), Guid("00000113-0000-0000-C000-000000000046"), InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IOleInPlaceObject
		{

			[return: MarshalAs(UnmanagedType.I4)]
			[PreserveSig]
			int GetWindow([Out]out IntPtr hwnd);


			void ContextSensitiveHelp(
				[In, MarshalAs(UnmanagedType.I4)]
				int fEnterMode);


			void InPlaceDeactivate();

			[return: MarshalAs(UnmanagedType.I4)]
			[PreserveSig]
			int UIDeactivate();


			void SetObjectRects(
				[In]
				RECT lprcPosRect,
				[In]
				RECT lprcClipRect);


			void ReactivateAndUndo();


		}

		[ComImport(),
			Guid("9C2CAD80-3424-11CF-B670-00AA004CD6D8"),
			InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IOleInPlaceSiteEx
		{
			//IOleWindow
			[return: MarshalAs(UnmanagedType.I4)]
			[PreserveSig]
			int GetWindow([In, Out] ref IntPtr phwnd);
			[return: MarshalAs(UnmanagedType.I4)]
			[PreserveSig]
			int ContextSensitiveHelp([In, MarshalAs(UnmanagedType.Bool)] bool
				fEnterMode);

			[return: MarshalAs(UnmanagedType.I4)]
			[PreserveSig]
			int CanInPlaceActivate();
			[return: MarshalAs(UnmanagedType.I4)]
			[PreserveSig]
			int OnInPlaceActivate();
			[return: MarshalAs(UnmanagedType.I4)]
			[PreserveSig]
			int OnUIActivate();
			[return: MarshalAs(UnmanagedType.I4)]
			[PreserveSig]
			int GetWindowContext([Out, MarshalAs(UnmanagedType.Interface)] out IOleInPlaceFrame ppFrame,
				[Out, MarshalAs(UnmanagedType.Interface)] out IOleInPlaceUIWindow
				ppDoc, [Out] RECT lprcPosRect, [Out] RECT lprcClipRect, [In, Out] tagOIFI
				lpFrameInfo);
			[return: MarshalAs(UnmanagedType.I4)]
			[PreserveSig]
			int Scroll([In, MarshalAs(UnmanagedType.Struct)] tagSIZE scrollExtent); //tagSIZE
			[return: MarshalAs(UnmanagedType.I4)]
			[PreserveSig]
			int OnUIDeactivate([In, MarshalAs(UnmanagedType.I4)] int fUndoable);
			[return: MarshalAs(UnmanagedType.I4)]
			[PreserveSig]
			int OnInPlaceDeactivate();
			[return: MarshalAs(UnmanagedType.I4)]
			[PreserveSig]
			int DiscardUndoState();
			[return: MarshalAs(UnmanagedType.I4)]
			[PreserveSig]
			int DeactivateAndUndo();
			[return: MarshalAs(UnmanagedType.I4)]
			[PreserveSig]
			int OnPosRectChange([In] ref RECT lprcPosRect);


			//IOleInPlaceSiteEx
			[return: MarshalAs(UnmanagedType.I4)]
			[PreserveSig]
			int OnInPlaceActivateEx(
				[Out, MarshalAs(UnmanagedType.Bool)] out bool pfNoRedraw,
				[In, MarshalAs(UnmanagedType.I4)]  int dwFlags
				);

			[return: MarshalAs(UnmanagedType.I4)]
			[PreserveSig]
			int OnInPlaceDeactivateEx(
				[In, MarshalAs(UnmanagedType.Bool)] bool fNoRedraw
				);

			[return: MarshalAs(UnmanagedType.I4)]
			[PreserveSig]
			int RequestUIActivate();
		}


		[ComImport(), Guid("00000119-0000-0000-C000-000000000046"), InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IOleInPlaceSite
		{

			IntPtr GetWindow();

			[return: MarshalAs(UnmanagedType.I4)]
			[PreserveSig]
			int ContextSensitiveHelp(
				[In, MarshalAs(UnmanagedType.I4)]
				int fEnterMode);

			[return: MarshalAs(UnmanagedType.I4)]
			[PreserveSig]
			int CanInPlaceActivate();

			[return: MarshalAs(UnmanagedType.I4)]
			[PreserveSig]
			int OnInPlaceActivate();

			[return: MarshalAs(UnmanagedType.I4)]
			[PreserveSig]
			int OnUIActivate();

			[return: MarshalAs(UnmanagedType.I4)]
			[PreserveSig]
			int GetWindowContext(
				[Out]
				out IOleInPlaceFrame ppFrame,
				[Out]
				out IOleInPlaceUIWindow ppDoc,
				[Out]
				RECT lprcPosRect,
				[Out]
				RECT lprcClipRect,
				[In, Out]
				tagOIFI lpFrameInfo);

			[return: MarshalAs(UnmanagedType.I4)]
			[PreserveSig]
			int Scroll(
				[In, MarshalAs(UnmanagedType.Struct)]
				tagSIZE scrollExtent);

			[return: MarshalAs(UnmanagedType.I4)]
			int OnUIDeactivate(
				[In, MarshalAs(UnmanagedType.I4)]
				int fUndoable);

			[return: MarshalAs(UnmanagedType.I4)]
			[PreserveSig]
			int OnInPlaceDeactivate();

			[return: MarshalAs(UnmanagedType.I4)]
			[PreserveSig]
			int DiscardUndoState();

			[return: MarshalAs(UnmanagedType.I4)]
			[PreserveSig]
			int DeactivateAndUndo();

			[return: MarshalAs(UnmanagedType.I4)]
			[PreserveSig]
			int OnPosRectChange(
				[In]
				RECT lprcPosRect);
		}

		[Guid("00000115-0000-0000-C000-000000000046"), InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IOleInPlaceUIWindow
		{

			//C#r: UNDONE (Field in interface) public static readonly    Guid iid;

			IntPtr GetWindow();


			void ContextSensitiveHelp(
				[In, MarshalAs(UnmanagedType.I4)]
				int fEnterMode);


			void GetBorder(
				[Out]
				RECT lprectBorder);


			void RequestBorderSpace(
				[In]
				RECT pborderwidths);


			void SetBorderSpace(
				[In]
				RECT pborderwidths);


			void SetActiveObject(
				[In, MarshalAs(UnmanagedType.Interface)]
				IOleInPlaceActiveObject pActiveObject,
				[In, MarshalAs(UnmanagedType.LPWStr)]
				string pszObjName);


		}

		[ComImport(), Guid("00000112-0000-0000-C000-000000000046"), InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IOleObject
		{

			[return: MarshalAs(UnmanagedType.I4)]
			[PreserveSig]
			int SetClientSite(
				[In, MarshalAs(UnmanagedType.Interface)]
				IOleClientSite pClientSite);

			[return: MarshalAs(UnmanagedType.I4)]
			[PreserveSig]
			int GetClientSite(out IOleClientSite site);

			[return: MarshalAs(UnmanagedType.I4)]
			[PreserveSig]
			int SetHostNames(
				[In, MarshalAs(UnmanagedType.LPWStr)]
				string szContainerApp,
				[In, MarshalAs(UnmanagedType.LPWStr)]
				string szContainerObj);

			[return: MarshalAs(UnmanagedType.I4)]
			[PreserveSig]
			int Close(
				[In, MarshalAs(UnmanagedType.I4)]
				int dwSaveOption);

			[return: MarshalAs(UnmanagedType.I4)]
			[PreserveSig]
			int SetMoniker(
				[In, MarshalAs(UnmanagedType.U4)]
				int dwWhichMoniker,
				[In, MarshalAs(UnmanagedType.Interface)]
				object pmk);

			[return: MarshalAs(UnmanagedType.I4)]
			[PreserveSig]
			int GetMoniker(
				[In, MarshalAs(UnmanagedType.U4)]
				int dwAssign,
				[In, MarshalAs(UnmanagedType.U4)]
				int dwWhichMoniker,
				out object moniker);

			[return: MarshalAs(UnmanagedType.I4)]
			[PreserveSig]
			int InitFromData(
				[In, MarshalAs(UnmanagedType.Interface)]
				IOleDataObject pDataObject,
				[In, MarshalAs(UnmanagedType.I4)]
				int fCreation,
				[In, MarshalAs(UnmanagedType.U4)]
				int dwReserved);

			int GetClipboardData(
				[In, MarshalAs(UnmanagedType.U4)]
				int dwReserved,
				out IOleDataObject data);

			[return: MarshalAs(UnmanagedType.I4)]
			[PreserveSig]
			int DoVerb(
				[In, MarshalAs(UnmanagedType.I4)]
				int iVerb,
				[In]
				IntPtr lpmsg,
				[In, MarshalAs(UnmanagedType.Interface)]
				IOleClientSite pActiveSite,
				[In, MarshalAs(UnmanagedType.I4)]
				int lindex,
				[In]
				IntPtr hwndParent,
				[In]
				RECT lprcPosRect);

			[return: MarshalAs(UnmanagedType.I4)]
			[PreserveSig]
			int EnumVerbs(out IEnumOLEVERB e);

			[return: MarshalAs(UnmanagedType.I4)]
			[PreserveSig]
			int OleUpdate();

			[return: MarshalAs(UnmanagedType.I4)]
			[PreserveSig]
			int IsUpToDate();

			[return: MarshalAs(UnmanagedType.I4)]
			[PreserveSig]
			int GetUserClassID(
				[In, Out]
				ref Guid pClsid);

			[return: MarshalAs(UnmanagedType.I4)]
			[PreserveSig]
			int GetUserType(
				[In, MarshalAs(UnmanagedType.U4)]
				int dwFormOfType,
				[Out, MarshalAs(UnmanagedType.LPWStr)]
				out string userType);

			[return: MarshalAs(UnmanagedType.I4)]
			[PreserveSig]
			int SetExtent(
				[In, MarshalAs(UnmanagedType.U4)]
				int dwDrawAspect,
				[In]
				tagSIZEL pSizel);

			[return: MarshalAs(UnmanagedType.I4)]
			[PreserveSig]
			int GetExtent(
				[In, MarshalAs(UnmanagedType.U4)]
				int dwDrawAspect,
				[Out]
				tagSIZEL pSizel);

			[return: MarshalAs(UnmanagedType.I4)]
			[PreserveSig]
			int Advise(
				[In, MarshalAs(UnmanagedType.Interface)]
				IAdviseSink pAdvSink,
				out int cookie);

			[return: MarshalAs(UnmanagedType.I4)]
			[PreserveSig]
			int Unadvise(
				[In, MarshalAs(UnmanagedType.U4)]
				int dwConnection);

			[return: MarshalAs(UnmanagedType.I4)]
			[PreserveSig]
			int EnumAdvise(out IEnumSTATDATA e);

			[return: MarshalAs(UnmanagedType.I4)]
			[PreserveSig]
			int GetMiscStatus(
				[In, MarshalAs(UnmanagedType.U4)]
				int dwAspect,
				out int misc);

			[return: MarshalAs(UnmanagedType.I4)]
			[PreserveSig]
			int SetColorScheme(
				[In]
				tagLOGPALETTE pLogpal);


		}

		[Guid("A1FAF330-EF97-11CE-9BC9-00AA00608E01"), InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IOleParentUndoUnit //: IOleUndoUnit 
		{
			# region IOleUndoUnit
			[return: MarshalAs(UnmanagedType.I4)]
			[PreserveSig]
			int Do(
				[In, MarshalAs(UnmanagedType.Interface)]
				IOleUndoManager undoManager);

			[return: MarshalAs(UnmanagedType.I4)]
			[PreserveSig]
			int GetDescription(
				[Out, MarshalAs(UnmanagedType.BStr)]
				out string bStr);

			[return: MarshalAs(UnmanagedType.I4)]
			[PreserveSig]
			int GetUnitType(
				[Out, MarshalAs(UnmanagedType.I4)]
				out int clsid,
				[Out, MarshalAs(UnmanagedType.I4)]
				out int plID);

			[return: MarshalAs(UnmanagedType.I4)]
			[PreserveSig]
			int OnNextAdd();
			# endregion
			[return: MarshalAs(UnmanagedType.I4)]
			[PreserveSig]
			int Open(
				[In, MarshalAs(UnmanagedType.Interface)]
				IOleParentUndoUnit parentUnit);

			[return: MarshalAs(UnmanagedType.I4)]
			[PreserveSig]
			int Close(
				[In, MarshalAs(UnmanagedType.Interface)]
				IOleParentUndoUnit parentUnit,
				[In, MarshalAs(UnmanagedType.Bool)]
				bool fCommit);

			[return: MarshalAs(UnmanagedType.I4)]
			[PreserveSig]
			int Add(
				[In, MarshalAs(UnmanagedType.Interface)]
				IOleUndoUnit undoUnit);

			[return: MarshalAs(UnmanagedType.I4)]
			[PreserveSig]
			int FindUnit(
				[In, MarshalAs(UnmanagedType.Interface)]
				IOleUndoUnit undoUnit);

			[return: MarshalAs(UnmanagedType.I4)]
			[PreserveSig]
			int GetParentState(
				[Out, MarshalAs(UnmanagedType.I8)]
				out long state
				);
		}
		[ComImport(), Guid("6D5140C1-7436-11CE-8034-00AA006009FA"),
			InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IOleServiceProvider
		{
			[return: MarshalAs(UnmanagedType.I4)]
			[PreserveSig]
			int QueryService(
				[In] ref Guid guidService,
				[In] ref Guid riid,
				out IntPtr ppvObject);
		}

		[Guid("D001F200-EF97-11CE-9BC9-00AA00608E01"), InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IOleUndoManager
		{
			void Open(
				[In, MarshalAs(UnmanagedType.Interface)]
				IOleParentUndoUnit parentUndo);

			void Close(
				[In, MarshalAs(UnmanagedType.Interface)]
				IOleParentUndoUnit parentUndo,
				[In, MarshalAs(UnmanagedType.Bool)]
				bool fCommit);

			void Add(
				[In, MarshalAs(UnmanagedType.Interface)]
				IOleUndoUnit undoUnit);

			[return: MarshalAs(UnmanagedType.I8)]
			long GetOpenParentState();

			void DiscardFrom(
				[In, MarshalAs(UnmanagedType.Interface)]
				IOleUndoUnit undoUnit);

			void UndoTo(
				[In, MarshalAs(UnmanagedType.Interface)]
				IOleUndoUnit undoUnit);

			void RedoTo(
				[In, MarshalAs(UnmanagedType.Interface)]
				IOleUndoUnit undoUnit);

			[return: MarshalAs(UnmanagedType.Interface)]
			IEnumOleUndoUnits EnumUndoable();

			[return: MarshalAs(UnmanagedType.Interface)]
			IEnumOleUndoUnits EnumRedoable();

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetLastUndoDescription();

			[return: MarshalAs(UnmanagedType.BStr)]
			string GetLastRedoDescription();

			void Enable(
				[In, MarshalAs(UnmanagedType.Bool)]
				bool fEnable);

		}

		[Guid("894AD3B0-EF97-11CE-9BC9-00AA00608E01"), InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IOleUndoUnit
		{
			[return: MarshalAs(UnmanagedType.I4)]
			[PreserveSig]
			int Do(
				[In, MarshalAs(UnmanagedType.Interface)]
				IOleUndoManager undoManager);

			[return: MarshalAs(UnmanagedType.I4)]
			[PreserveSig]
			int GetDescription(
				[Out, MarshalAs(UnmanagedType.BStr)]
				out string bStr);

			[return: MarshalAs(UnmanagedType.I4)]
			[PreserveSig]
			int GetUnitType(
				[Out, MarshalAs(UnmanagedType.I4)]
				out int clsid,
				[Out, MarshalAs(UnmanagedType.I4)]
				out int plID);

			[return: MarshalAs(UnmanagedType.I4)]
			[PreserveSig]
			int OnNextAdd();
		}
		#endregion
		[Guid("79eac9c9-baf9-11ce-8c82-00aa004ba90b"), InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IPersistMoniker
		{


			void GetClassID(
				[In, Out]
				ref Guid pClassID);

			[return: MarshalAs(UnmanagedType.I4)]
			[PreserveSig]
			int IsDirty();


			void Load(
				[In]
				int fFullyAvailable,
				[In, MarshalAs(UnmanagedType.Interface)]
				IMoniker pmk,
				[In, MarshalAs(UnmanagedType.Interface)]
				object pbc,
				[In]
				int grfMode);


			void Save(
				[In, MarshalAs(UnmanagedType.Interface)]
				IMoniker pimkName,
				[In, MarshalAs(UnmanagedType.Interface)]
				object pbc,
				[In, MarshalAs(UnmanagedType.Bool)]
				bool fRemember);

			void SaveCompleted(
				[In, MarshalAs(UnmanagedType.Interface)]
				IMoniker pmk,
				[In, MarshalAs(UnmanagedType.Interface)]
				object pbc);

			[return: MarshalAs(UnmanagedType.Interface)]
			IMoniker GetCurMoniker();
		}

		[ComImport(), Guid("7FD52380-4E07-101B-AE2D-08002B2EC713"), InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IPersistStreamInit
		{

			void GetClassID(
				[In, Out]
				ref Guid pClassID);

			[return: MarshalAs(UnmanagedType.I4)]
			[PreserveSig]
			int IsDirty();

			void Load(
				[In, MarshalAs(UnmanagedType.Interface)]
				IStream pstm);

			void Save(
				[In, MarshalAs(UnmanagedType.Interface)]
				IStream pstm,
				[In, MarshalAs(UnmanagedType.I4)]
				int fClearDirty);

			void GetSizeMax(
				[Out, MarshalAs(UnmanagedType.LPArray)]
				long pcbSize);

			void InitNew();

		}

		[Guid("9BFBBC02-EFF1-101A-84ED-00AA00341D07"), InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IPropertyNotifySink
		{
			[return: MarshalAs(UnmanagedType.I4)]
			[PreserveSig]
			int OnChanged(
				[In, MarshalAs(UnmanagedType.I4)] int DispId
				);

			[return: MarshalAs(UnmanagedType.I4)]
			[PreserveSig]
			int OnRequestEdit(
				[In, MarshalAs(UnmanagedType.I4)] int DispId
				);
		}

		[Guid("6D5140C1-7436-11CE-8034-00AA006009FA"), InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IServiceProvider
		{
			[return: MarshalAs(UnmanagedType.I4)]
			int QueryService(
				[In] ref Guid sid,
				[In] ref Guid iid,
				out IntPtr service
				);
		}

		//        [Guid("0000000C-0000-0000-C000-000000000046"), InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
		//            public interface IStream 
		//        {
		//
		//            [return: MarshalAs(UnmanagedType.I4)]
		//            [PreserveSig]
		//            int Read(
		//                [In]
		//                IntPtr buf,
		//                [In, MarshalAs(UnmanagedType.I4)]
		//                int len);
		//
		//            [return: MarshalAs(UnmanagedType.I4)]
		//            [PreserveSig]
		//            int Write(
		//                [In]
		//                IntPtr buf,
		//                [In, MarshalAs(UnmanagedType.I4)]
		//                int len);
		//
		//            [return: MarshalAs(UnmanagedType.I8)]
		//            long Seek(
		//                [In, MarshalAs(UnmanagedType.I8)]
		//                long dlibMove,
		//                [In, MarshalAs(UnmanagedType.I4)]
		//                int dwOrigin);
		//
		//
		//            void SetSize(
		//                [In, MarshalAs(UnmanagedType.I8)]
		//                long libNewSize);
		//
		//            [return: MarshalAs(UnmanagedType.I8)]
		//            long CopyTo(
		//                [In, MarshalAs(UnmanagedType.Interface)]
		//                IStream pstm,
		//                [In, MarshalAs(UnmanagedType.I8)]
		//                long cb,
		//                [Out, MarshalAs(UnmanagedType.LPArray)]
		//                long[] pcbRead);
		//
		//
		//            void Commit(
		//                [In, MarshalAs(UnmanagedType.I4)]
		//                int grfCommitFlags);
		//
		//
		//            void Revert();
		//
		//
		//            void LockRegion(
		//                [In, MarshalAs(UnmanagedType.I8)]
		//                long libOffset,
		//                [In, MarshalAs(UnmanagedType.I8)]
		//                long cb,
		//                [In, MarshalAs(UnmanagedType.I4)]
		//                int dwLockType);
		//
		//
		//            void UnlockRegion(
		//                [In, MarshalAs(UnmanagedType.I8)]
		//                long libOffset,
		//                [In, MarshalAs(UnmanagedType.I8)]
		//                long cb,
		//                [In, MarshalAs(UnmanagedType.I4)]
		//                int dwLockType);
		//
		//
		//            void Stat(
		//                [Out] out
		//                STATSTG pstatstg,
		//                [In, MarshalAs(UnmanagedType.I4)]
		//                int grfStatFlag);
		//
		//            [return: MarshalAs(UnmanagedType.Interface)]
		//            IStream Clone();
		//
		//
		//        }
		//
		[ComImport()]
		[Guid("3050f690-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IHighlightSegment
		{
		}

		[ComImport()]
		[Guid("3050f606-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IHighlightRenderingServices
		{
			void AddSegment(IDisplayPointer pDispPointerStart, IDisplayPointer pDispPointerEnd, IHTMLRenderStyle pIRenderStyle, out IHighlightSegment ppISegment);

			void MoveSegmentToPointers(IHighlightSegment pISegment, IDisplayPointer pDispPointerStart, IDisplayPointer pDispPointerEnd);

			void RemoveSegment(IHighlightSegment pISegment);

		}

		[ComImport()]
		[Guid("3050f69e-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IDisplayPointer
		{
			void MoveToPoint(POINT ptPoint, COORD_SYSTEM eCoordSystem, IHTMLElement pElementContext, UInt32 dwHitTestOptions, out UInt32 pdwHitTestResults);

			void MoveUnit(DISPLAY_MOVEUNIT eMoveUnit, Int32 lXPos);

			void PositionMarkupPointer(IMarkupPointer pMarkupPointer);

			void MoveToPointer(IDisplayPointer pDispPointer);

			void SetPointerGravity(POINTER_GRAVITY eGravity);

			void GetPointerGravity(out POINTER_GRAVITY peGravity);

			void SetDisplayGravity(DISPLAY_GRAVITY eGravity);

			void GetDisplayGravity(out DISPLAY_GRAVITY peGravity);

			void IsPositioned(out Int32 pfPositioned);

			void Unposition();

			void IsEqualTo(IDisplayPointer pDispPointer, out Int32 pfIsEqual);

			void IsLeftOf(IDisplayPointer pDispPointer, out Int32 pfIsLeftOf);

			void IsRightOf(IDisplayPointer pDispPointer, out Int32 pfIsRightOf);

			void IsAtBOL(out Int32 pfBOL);

			void MoveToMarkupPointer(IMarkupPointer pPointer, IDisplayPointer pDispLineContext);

			void ScrollIntoView();

			void GetLineInfo(out ILineInfo ppLineInfo);

			void GetFlowElement(out IHTMLElement ppLayoutElement);

			void QueryBreaks(out UInt32 pdwBreaks);

		}

		[ComImport()]
		[Guid("3050f69d-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IDisplayServices
		{
			void CreateDisplayPointer(out IDisplayPointer ppDispPointer);

			void TransformRect(ref RECT pRect, COORD_SYSTEM eSource, COORD_SYSTEM eDestination, IHTMLElement pIElement);

			void TransformPoint(ref POINT pPoint, COORD_SYSTEM eSource, COORD_SYSTEM eDestination, IHTMLElement pIElement);

			void GetCaret(out IHTMLCaret ppCaret);

			void GetComputedStyle(IMarkupPointer pPointer, out IHTMLComputedStyle ppComputedStyle);

			void ScrollRectIntoView(IHTMLElement pIElement, RECT rect);

			void HasFlowLayout(IHTMLElement pIElement, out Int32 pfHasFlowLayout);

		}

		[ComImport()]
		[Guid("3050f6c3-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IHTMLComputedStyle
		{
			void IsEqual(IHTMLComputedStyle pComputedStyle, out Boolean pfEqual);

			Boolean underline
			{
				[DispId(DispId.IHTMLCOMPUTEDSTYLE_UNDERLINE)]
				get;
			}
			Boolean bold
			{
				[DispId(DispId.IHTMLCOMPUTEDSTYLE_BOLD)]
				get;
			}
			Boolean blockDirection
			{
				[DispId(DispId.IHTMLCOMPUTEDSTYLE_BLOCKDIRECTION)]
				get;
			}
			Boolean overline
			{
				[DispId(DispId.IHTMLCOMPUTEDSTYLE_OVERLINE)]
				get;
			}
			Boolean OL
			{
				[DispId(DispId.IHTMLCOMPUTEDSTYLE_OL)]
				get;
			}
			Int32 fontSize
			{
				[DispId(DispId.IHTMLCOMPUTEDSTYLE_FONTSIZE)]
				get;
			}
			UInt32 backgroundColor
			{
				[DispId(DispId.IHTMLCOMPUTEDSTYLE_BACKGROUNDCOLOR)]
				get;
			}
			Boolean direction
			{
				[DispId(DispId.IHTMLCOMPUTEDSTYLE_DIRECTION)]
				get;
			}
			Boolean hasBgColor
			{
				[DispId(DispId.IHTMLCOMPUTEDSTYLE_HASBGCOLOR)]
				get;
			}
			Boolean explicitFace
			{
				[DispId(DispId.IHTMLCOMPUTEDSTYLE_EXPLICITFACE)]
				get;
			}
			Boolean italic
			{
				[DispId(DispId.IHTMLCOMPUTEDSTYLE_ITALIC)]
				get;
			}
			Boolean superScript
			{
				[DispId(DispId.IHTMLCOMPUTEDSTYLE_SUPERSCRIPT)]
				get;
			}
			UInt32 textColor
			{
				[DispId(DispId.IHTMLCOMPUTEDSTYLE_TEXTCOLOR)]
				get;
			}
			Boolean subScript
			{
				[DispId(DispId.IHTMLCOMPUTEDSTYLE_SUBSCRIPT)]
				get;
			}
			Int32 fontWeight
			{
				[DispId(DispId.IHTMLCOMPUTEDSTYLE_FONTWEIGHT)]
				get;
			}
			String fontName
			{
				[DispId(DispId.IHTMLCOMPUTEDSTYLE_FONTNAME)]
				get;
			}
			Boolean strikeOut
			{
				[DispId(DispId.IHTMLCOMPUTEDSTYLE_STRIKEOUT)]
				get;
			}
			Boolean preFormatted
			{
				[DispId(DispId.IHTMLCOMPUTEDSTYLE_PREFORMATTED)]
				get;
			}
		}

		[ComImport()]
		[Guid("3050f49f-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IMarkupPointer
		{
			void OwningDoc(out IHTMLDocument2 ppDoc);
			void Gravity(out POINTER_GRAVITY pGravity);
			void SetGravity(POINTER_GRAVITY Gravity);
			void Cling(out Int32 pfCling);
			void SetCling(Int32 fCLing);
			void Unposition();
			void IsPositioned(out Int32 pfPositioned);
			void GetContainer(out IMarkupContainer ppContainer);
			void MoveAdjacentToElement(IHTMLElement pElement, ELEMENT_ADJACENCY eAdj);
			void MoveToPointer(IMarkupPointer pPointer);
			void MoveToContainer(IMarkupContainer pContainer, Int32 fAtStart);
			void Left(Int32 fMove, out MARKUP_CONTEXT_TYPE pContext, out IHTMLElement ppElement, ref Int32 pcch, out String pchText);
			void Right(Int32 fMove, out MARKUP_CONTEXT_TYPE pContext, out IHTMLElement ppElement, ref Int32 pcch, out String pchText);
			void CurrentScope(out IHTMLElement ppElemCurrent);
			void IsLeftOf(IMarkupPointer pPointerThat, out Int32 pfResult);
			void IsLeftOfOrEqualTo(IMarkupPointer pPointerThat, out Int32 pfResult);
			void IsRightOf(IMarkupPointer pPointerThat, out Int32 pfResult);
			void IsRightOfOrEqualTo(IMarkupPointer pPointerThat, out Int32 pfResult);
			void IsEqualTo(IMarkupPointer pPointerThat, out Int32 pfAreEqual);
			void MoveUnit(MOVEUNIT_ACTION muAction);
			void FindText(ref String pchFindText, UInt32 dwFlags, IMarkupPointer pIEndMatch, IMarkupPointer pIEndSearch);
		}

		[ComImport()]
		[Guid("3050f675-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IMarkupPointer2
		{
			void IsAtWordBreak(out Int32 pfAtBreak);
			void GetMarkupPosition(out Int32 plMP);
			void MoveToMarkupPosition(IMarkupContainer pContainer, Int32 lMP);
			void MoveUnitBounded(MOVEUNIT_ACTION muAction, IMarkupPointer pIBoundary);
			void IsInsideURL(IMarkupPointer pRight, out Int32 pfResult);
			void MoveToContent(IHTMLElement pIElement, Int32 fAtStart);
		}

		[ComImport()]
		[Guid("3050f4a0-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IMarkupServices
		{
			void CreateMarkupPointer(out IMarkupPointer ppPointer);

			void CreateMarkupContainer(out IMarkupContainer ppMarkupContainer);

			void CreateElement(ELEMENT_TAG_ID tagID, [In, Out] String pchAttributes, [Out, MarshalAs(UnmanagedType.Interface)] out IHTMLElement ppElement);

			void CloneElement(IHTMLElement pElemCloneThis, out IHTMLElement ppElementTheClone);

			void InsertElement(
				[In, MarshalAs(UnmanagedType.Interface)] IHTMLElement pElementInsert,
				[In, MarshalAs(UnmanagedType.Interface)] IMarkupPointer pPointerStart,
				[In, MarshalAs(UnmanagedType.Interface)] IMarkupPointer pPointerFinish);

			void RemoveElement(IHTMLElement pElementRemove);

			void Remove(IMarkupPointer pPointerStart, IMarkupPointer pPointerFinish);

			void Copy(IMarkupPointer pPointerSourceStart, IMarkupPointer pPointerSourceFinish, IMarkupPointer pPointerTarget);

			void Move(IMarkupPointer pPointerSourceStart, IMarkupPointer pPointerSourceFinish, IMarkupPointer pPointerTarget);

			void InsertText(/*ref*/[In, Out] String pchText, Int32 cch, IMarkupPointer pPointerTarget);

			void ParseString(ref String pchHTML, UInt32 dwFlags, out IMarkupContainer ppContainerResult, IMarkupPointer ppPointerStart, IMarkupPointer ppPointerFinish);

			void ParseGlobal(UserHGLOBAL hglobalHTML, UInt32 dwFlags, out IMarkupContainer ppContainerResult, IMarkupPointer pPointerStart, IMarkupPointer pPointerFinish);

			void IsScopedElement(IHTMLElement pElement, out Int32 pfScoped);

			void GetElementTagId(IHTMLElement pElement, out ELEMENT_TAG_ID ptagId);

			void GetTagIDForName(String bstrName, out ELEMENT_TAG_ID ptagId);

			void GetNameForTagID(ELEMENT_TAG_ID tagId, out String pbstrName);

			void MovePointersToRange(IHTMLTxtRange pIRange, IMarkupPointer pPointerStart, IMarkupPointer pPointerFinish);

			void MoveRangeToPointers(IMarkupPointer pPointerStart, IMarkupPointer pPointerFinish, IHTMLTxtRange pIRange);

			void BeginUndoUnit(ref String pchTitle);

			void EndUndoUnit();

		}

		public struct UserHGLOBAL
		{
			public int fContext;
			public IntPtr u;
		}


		[Guid("3050F682-98B5-11CF-BB82-00AA00BDCE0B")]
		public interface IMarkupServices2
		{

			void ParseGlobalEx(
				[In] UserHGLOBAL hglobalHTML,
				[In] uint dwFlags,
				[In] IMarkupContainer pContext,
				[Out] out IMarkupContainer ppContainerResult,
				[In] IMarkupPointer pPointerStart,
				[In] IMarkupPointer pPointerFinish);

			void ValidateElements(
				[In] IMarkupPointer pPointerStart,
				[In] IMarkupPointer pPointerFinish,
				[In] IMarkupPointer pPointerTarget,
				[In, Out] ref IMarkupPointer pPointerStatus,
				[Out] out IHTMLElement ppElemFailBottom,
				[Out] out IHTMLElement ppElemFailTop);

			void SaveSegmentsToClipboard(
				[In] ISegmentList pSegmentList,
				[In] uint dwFlags);
		}

		[ComImport()]
		[Guid("3050f6ae-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		public interface IHTMLRenderStyle
		{
			String textUnderlineStyle
			{
				[DispId(DispId.IHTMLRENDERSTYLE_TEXTUNDERLINESTYLE)]
				get;
				[DispId(DispId.IHTMLRENDERSTYLE_TEXTUNDERLINESTYLE)]
				set;
			}
			String defaultTextSelection
			{
				[DispId(DispId.IHTMLRENDERSTYLE_DEFAULTTEXTSELECTION)]
				get;
				[DispId(DispId.IHTMLRENDERSTYLE_DEFAULTTEXTSELECTION)]
				set;
			}
			Int32 renderingPriority
			{
				[DispId(DispId.IHTMLRENDERSTYLE_RENDERINGPRIORITY)]
				get;
				[DispId(DispId.IHTMLRENDERSTYLE_RENDERINGPRIORITY)]
				set;
			}
			String textDecoration
			{
				[DispId(DispId.IHTMLRENDERSTYLE_TEXTDECORATION)]
				get;
				[DispId(DispId.IHTMLRENDERSTYLE_TEXTDECORATION)]
				set;
			}
			Object textColor
			{
				[DispId(DispId.IHTMLRENDERSTYLE_TEXTCOLOR)]
				get;
				[DispId(DispId.IHTMLRENDERSTYLE_TEXTCOLOR)]
				set;
			}
			String textEffect
			{
				[DispId(DispId.IHTMLRENDERSTYLE_TEXTEFFECT)]
				get;
				[DispId(DispId.IHTMLRENDERSTYLE_TEXTEFFECT)]
				set;
			}
			String textLineThroughStyle
			{
				[DispId(DispId.IHTMLRENDERSTYLE_TEXTLINETHROUGHSTYLE)]
				get;
				[DispId(DispId.IHTMLRENDERSTYLE_TEXTLINETHROUGHSTYLE)]
				set;
			}
			Object textBackgroundColor
			{
				[DispId(DispId.IHTMLRENDERSTYLE_TEXTBACKGROUNDCOLOR)]
				get;
				[DispId(DispId.IHTMLRENDERSTYLE_TEXTBACKGROUNDCOLOR)]
				set;
			}
			Object textDecorationColor
			{
				[DispId(DispId.IHTMLRENDERSTYLE_TEXTDECORATIONCOLOR)]
				get;
				[DispId(DispId.IHTMLRENDERSTYLE_TEXTDECORATIONCOLOR)]
				set;
			}
		}

		[ComImport()]
		[Guid("3050f5f9-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IMarkupContainer
		{
			void OwningDoc(out IHTMLDocument2 ppDoc);

		}

		[ComImport()]
		[Guid("3050f244-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		public interface IHTMLSelectElement
		{
			[DispId(DispId.IHTMLSELECTELEMENT_ADD)]
			void add(IHTMLElement element, Object before);

			[DispId(DispId.IHTMLSELECTELEMENT_REMOVE)]
			void remove(Int32 index);

			[DispId(DispId.IHTMLSELECTELEMENT_ITEM)]
			Object item(Object name, Object index);

			[DispId(DispId.IHTMLSELECTELEMENT_TAGS)]
			Object tags(Object tagName);

			String name
			{
				[DispId(DispId.IHTMLSELECTELEMENT_NAME)]
				get;
				[DispId(DispId.IHTMLSELECTELEMENT_NAME)]
				set;
			}
			Object onchange
			{
				[DispId(DispId.IHTMLSELECTELEMENT_ONCHANGE)]
				get;
				[DispId(DispId.IHTMLSELECTELEMENT_ONCHANGE)]
				set;
			}
			Int32 length
			{
				[DispId(DispId.IHTMLSELECTELEMENT_LENGTH)]
				get;
				[DispId(DispId.IHTMLSELECTELEMENT_LENGTH)]
				set;
			}
			Object options
			{
				[DispId(DispId.IHTMLSELECTELEMENT_OPTIONS)]
				get;
			}
			String type
			{
				[DispId(DispId.IHTMLSELECTELEMENT_TYPE)]
				get;
			}
			Boolean multiple
			{
				[DispId(DispId.IHTMLSELECTELEMENT_MULTIPLE)]
				get;
				[DispId(DispId.IHTMLSELECTELEMENT_MULTIPLE)]
				set;
			}
			String value
			{
				[DispId(DispId.IHTMLSELECTELEMENT_VALUE)]
				get;
				[DispId(DispId.IHTMLSELECTELEMENT_VALUE)]
				set;
			}
			Int32 size
			{
				[DispId(DispId.IHTMLSELECTELEMENT_SIZE)]
				get;
				[DispId(DispId.IHTMLSELECTELEMENT_SIZE)]
				set;
			}
			IHTMLFormElement form
			{
				[DispId(DispId.IHTMLSELECTELEMENT_FORM)]
				get;
			}
			Int32 selectedIndex
			{
				[DispId(DispId.IHTMLSELECTELEMENT_SELECTEDINDEX)]
				get;
				[DispId(DispId.IHTMLSELECTELEMENT_SELECTEDINDEX)]
				set;
			}
			Boolean disabled
			{
				[DispId(DispId.IHTMLSELECTELEMENT_DISABLED)]
				get;
				[DispId(DispId.IHTMLSELECTELEMENT_DISABLED)]
				set;
			}
			Object _newEnum
			{
				[DispId(DispId.IHTMLSELECTELEMENT__NEWENUM)]
				get;
			}
		}

		[ComImport(),
			Guid("CB5BDC81-93C1-11cf-8F20-00805F2CD064"),
			InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IObjectSafety
		{
			[return: MarshalAs(UnmanagedType.I4)]
			[PreserveSig]
			Int32 GetInterfaceSafetyOptions(
				[In]  ref UInt32 riid,					// Interface that we want options for
				[Out] out UInt32 pdwSupportedOptions,	// Options meaningful on this interface
				[Out] out UInt32 pdwEnabledOptions);		// current option values on this interface

			[return: MarshalAs(UnmanagedType.I4)]
			[PreserveSig]
			Int32 SetInterfaceSafetyOptions(
				[In] ref UInt32 riid,				// Interface to set options for
				[In] UInt32 dwOptionSetMask,			// Options to change
				[In] Int32 dwEnabledOptions);		// New option values
		}

		[ComImport()]
		[Guid("3050f204-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		public interface IHTMLBaseElement
		{
			String href
			{
				[DispId(DispId.IHTMLBASEELEMENT_HREF)]
				get;
				[DispId(DispId.IHTMLBASEELEMENT_HREF)]
				set;
			}
			String target
			{
				[DispId(DispId.IHTMLBASEELEMENT_TARGET)]
				get;
				[DispId(DispId.IHTMLBASEELEMENT_TARGET)]
				set;
			}
		}

		/// <exclude/>
		[Guid("3050f604-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IHTMLCaret
		{
			void MoveCaretToPointer(
				[In, MarshalAs(UnmanagedType.Interface)] IDisplayPointer pDispPointer,
				[In, MarshalAs(UnmanagedType.Bool)] bool fScrollIntoView,
				[In] CARET_DIRECTION eDir);

			void MoveCaretToPointerEx(
				[In, MarshalAs(UnmanagedType.Interface)] IDisplayPointer pDispPointer,
				[In, MarshalAs(UnmanagedType.Bool)] bool fVisible,
				[In, MarshalAs(UnmanagedType.Bool)] bool fScrollIntoView,
				[In] CARET_DIRECTION eDir);

			void MoveMarkupPointerToCaret(
				[In, MarshalAs(UnmanagedType.Interface)] IMarkupPointer pIMarkupPointer);

			void MoveDisplayPointerToCaret(
				[MarshalAs(UnmanagedType.Interface)] IDisplayPointer pDispPointer);

			void IsVisible(
				[MarshalAs(UnmanagedType.Bool), Out] out bool pIsVisible);

			void Show(
				[MarshalAs(UnmanagedType.Bool)] bool fScrollIntoView);

			void Hide();

			void InsertText(ref String pText, Int32 lLen);

			void ScrollIntoView();

			void GetLocation([In, Out, MarshalAs(UnmanagedType.Struct)] ref POINT pPoint, [MarshalAs(UnmanagedType.Bool)] bool fTranslate);

			void GetCaretDirection(out CARET_DIRECTION peDir);

			void SetCaretDirection(CARET_DIRECTION eDir);

		}

		[ComImport()]
		[Guid("3050f1f7-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		public interface IHTMLFormElement
		{
			[DispId(DispId.IHTMLFORMELEMENT_SUBMIT)]
			void submit();

			[DispId(DispId.IHTMLFORMELEMENT_RESET)]
			void reset();

			[DispId(DispId.IHTMLFORMELEMENT_ITEM)]
			Object item(Object name, Object index);

			[DispId(DispId.IHTMLFORMELEMENT_TAGS)]
			Object tags(Object tagName);

			String encoding
			{
				[DispId(DispId.IHTMLFORMELEMENT_ENCODING)]
				get;
				[DispId(DispId.IHTMLFORMELEMENT_ENCODING)]
				set;
			}
			String action
			{
				[DispId(DispId.IHTMLFORMELEMENT_ACTION)]
				get;
				[DispId(DispId.IHTMLFORMELEMENT_ACTION)]
				set;
			}
			Object _newEnum
			{
				[DispId(DispId.IHTMLFORMELEMENT__NEWENUM)]
				get;
			}
			String target
			{
				[DispId(DispId.IHTMLFORMELEMENT_TARGET)]
				get;
				[DispId(DispId.IHTMLFORMELEMENT_TARGET)]
				set;
			}
			String name
			{
				[DispId(DispId.IHTMLFORMELEMENT_NAME)]
				get;
				[DispId(DispId.IHTMLFORMELEMENT_NAME)]
				set;
			}
			String method
			{
				[DispId(DispId.IHTMLFORMELEMENT_METHOD)]
				get;
				[DispId(DispId.IHTMLFORMELEMENT_METHOD)]
				set;
			}
			String dir
			{
				[DispId(DispId.IHTMLFORMELEMENT_DIR)]
				get;
				[DispId(DispId.IHTMLFORMELEMENT_DIR)]
				set;
			}
			Object elements
			{
				[DispId(DispId.IHTMLFORMELEMENT_ELEMENTS)]
				get;
			}
			Int32 length
			{
				[DispId(DispId.IHTMLFORMELEMENT_LENGTH)]
				get;
				[DispId(DispId.IHTMLFORMELEMENT_LENGTH)]
				set;
			}
			Object onsubmit
			{
				[DispId(DispId.IHTMLFORMELEMENT_ONSUBMIT)]
				get;
				[DispId(DispId.IHTMLFORMELEMENT_ONSUBMIT)]
				set;
			}
			Object onreset
			{
				[DispId(DispId.IHTMLFORMELEMENT_ONRESET)]
				get;
				[DispId(DispId.IHTMLFORMELEMENT_ONRESET)]
				set;
			}
		}

		[ComImport()]
		[Guid("3050f7e2-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		public interface ILineInfo
		{
			Int32 lineDirection
			{
				[DispId(DispId.ILINEINFO_LINEDIRECTION)]
				get;
			}
			Int32 textHeight
			{
				[DispId(DispId.ILINEINFO_TEXTHEIGHT)]
				get;
			}
			Int32 x
			{
				[DispId(DispId.ILINEINFO_X)]
				get;
			}
			Int32 textDescent
			{
				[DispId(DispId.ILINEINFO_TEXTDESCENT)]
				get;
			}
			Int32 baseLine
			{
				[DispId(DispId.ILINEINFO_BASELINE)]
				get;
			}
		}
		[ComImport()]
		[Guid("3050f3ee-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		public interface IHTMLFiltersCollection
		{
			[DispId(DispId.IHTMLFILTERSCOLLECTION_ITEM)]
			Object item(Object pvarIndex);

			Int32 length
			{
				[DispId(DispId.IHTMLFILTERSCOLLECTION_LENGTH)]
				get;
			}
			Object _newEnum
			{
				[DispId(DispId.IHTMLFILTERSCOLLECTION__NEWENUM)]
				get;
			}
		}

		[ComImport()]
		[Guid("3050f21e-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		public interface IHTMLTable
		{
			[DispId(DispId.IHTMLTABLE_REFRESH)]
			void refresh();

			[DispId(DispId.IHTMLTABLE_NEXTPAGE)]
			void nextPage();

			[DispId(DispId.IHTMLTABLE_PREVIOUSPAGE)]
			void previousPage();

			[DispId(DispId.IHTMLTABLE_CREATETHEAD)]
			Object createTHead();

			[DispId(DispId.IHTMLTABLE_DELETETHEAD)]
			void deleteTHead();

			[DispId(DispId.IHTMLTABLE_CREATETFOOT)]
			Object createTFoot();

			[DispId(DispId.IHTMLTABLE_DELETETFOOT)]
			void deleteTFoot();

			[DispId(DispId.IHTMLTABLE_CREATECAPTION)]
			IHTMLTableCaption createCaption();

			[DispId(DispId.IHTMLTABLE_DELETECAPTION)]
			void deleteCaption();

			[DispId(DispId.IHTMLTABLE_INSERTROW)]
			Object insertRow(Int32 index);

			[DispId(DispId.IHTMLTABLE_DELETEROW)]
			void deleteRow(Int32 index);

			Object cellPadding
			{
				[DispId(DispId.IHTMLTABLE_CELLPADDING)]
				get;
				[DispId(DispId.IHTMLTABLE_CELLPADDING)]
				set;
			}
			Int32 dataPageSize
			{
				[DispId(DispId.IHTMLTABLE_DATAPAGESIZE)]
				get;
				[DispId(DispId.IHTMLTABLE_DATAPAGESIZE)]
				set;
			}
			IHTMLTableCaption caption
			{
				[DispId(DispId.IHTMLTABLE_CAPTION)]
				get;
			}
			IHTMLElementCollection rows
			{
				[DispId(DispId.IHTMLTABLE_ROWS)]
				get;
			}
			IHTMLTableSection tFoot
			{
				[DispId(DispId.IHTMLTABLE_TFOOT)]
				get;
			}
			String rules
			{
				[DispId(DispId.IHTMLTABLE_RULES)]
				get;
				[DispId(DispId.IHTMLTABLE_RULES)]
				set;
			}
			Object height
			{
				[DispId(DispId.IHTMLTABLE_HEIGHT)]
				get;
				[DispId(DispId.IHTMLTABLE_HEIGHT)]
				set;
			}
			Object onreadystatechange
			{
				[DispId(DispId.IHTMLTABLE_ONREADYSTATECHANGE)]
				get;
				[DispId(DispId.IHTMLTABLE_ONREADYSTATECHANGE)]
				set;
			}
			Object border
			{
				[DispId(DispId.IHTMLTABLE_BORDER)]
				get;
				[DispId(DispId.IHTMLTABLE_BORDER)]
				set;
			}
			IHTMLTableSection tHead
			{
				[DispId(DispId.IHTMLTABLE_THEAD)]
				get;
			}
			String align
			{
				[DispId(DispId.IHTMLTABLE_ALIGN)]
				get;
				[DispId(DispId.IHTMLTABLE_ALIGN)]
				set;
			}
			Object bgColor
			{
				[DispId(DispId.IHTMLTABLE_BGCOLOR)]
				get;
				[DispId(DispId.IHTMLTABLE_BGCOLOR)]
				set;
			}
			Object borderColorLight
			{
				[DispId(DispId.IHTMLTABLE_BORDERCOLORLIGHT)]
				get;
				[DispId(DispId.IHTMLTABLE_BORDERCOLORLIGHT)]
				set;
			}
			IHTMLElementCollection tBodies
			{
				[DispId(DispId.IHTMLTABLE_TBODIES)]
				get;
			}
			String readyState
			{
				[DispId(DispId.IHTMLTABLE_READYSTATE)]
				get;
			}
			String background
			{
				[DispId(DispId.IHTMLTABLE_BACKGROUND)]
				get;
				[DispId(DispId.IHTMLTABLE_BACKGROUND)]
				set;
			}
			Object borderColorDark
			{
				[DispId(DispId.IHTMLTABLE_BORDERCOLORDARK)]
				get;
				[DispId(DispId.IHTMLTABLE_BORDERCOLORDARK)]
				set;
			}
			Object width
			{
				[DispId(DispId.IHTMLTABLE_WIDTH)]
				get;
				[DispId(DispId.IHTMLTABLE_WIDTH)]
				set;
			}
			String frame
			{
				[DispId(DispId.IHTMLTABLE_FRAME)]
				get;
				[DispId(DispId.IHTMLTABLE_FRAME)]
				set;
			}
			Int32 cols
			{
				[DispId(DispId.IHTMLTABLE_COLS)]
				get;
				[DispId(DispId.IHTMLTABLE_COLS)]
				set;
			}
			Object cellSpacing
			{
				[DispId(DispId.IHTMLTABLE_CELLSPACING)]
				get;
				[DispId(DispId.IHTMLTABLE_CELLSPACING)]
				set;
			}
			Object borderColor
			{
				[DispId(DispId.IHTMLTABLE_BORDERCOLOR)]
				get;
				[DispId(DispId.IHTMLTABLE_BORDERCOLOR)]
				set;
			}
		}

		[ComImport()]
		[Guid("3050f2eb-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		public interface IHTMLTableCaption
		{
			String align
			{
				[DispId(DispId.IHTMLTABLECAPTION_ALIGN)]
				get;
				[DispId(DispId.IHTMLTABLECAPTION_ALIGN)]
				set;
			}
			String vAlign
			{
				[DispId(DispId.IHTMLTABLECAPTION_VALIGN)]
				get;
				[DispId(DispId.IHTMLTABLECAPTION_VALIGN)]
				set;
			}
		}

		[ComImport()]
		[Guid("3050f23b-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		public interface IHTMLTableSection
		{
			[DispId(DispId.IHTMLTABLESECTION_INSERTROW)]
			Object insertRow(Int32 index);

			[DispId(DispId.IHTMLTABLESECTION_DELETEROW)]
			void deleteRow(Int32 index);

			String vAlign
			{
				[DispId(DispId.IHTMLTABLESECTION_VALIGN)]
				get;
				[DispId(DispId.IHTMLTABLESECTION_VALIGN)]
				set;
			}
			Object bgColor
			{
				[DispId(DispId.IHTMLTABLESECTION_BGCOLOR)]
				get;
				[DispId(DispId.IHTMLTABLESECTION_BGCOLOR)]
				set;
			}
			String align
			{
				[DispId(DispId.IHTMLTABLESECTION_ALIGN)]
				get;
				[DispId(DispId.IHTMLTABLESECTION_ALIGN)]
				set;
			}
			IHTMLElementCollection rows
			{
				[DispId(DispId.IHTMLTABLESECTION_ROWS)]
				get;
			}
		}

		[ComImport()]
		[Guid("3050f23d-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		public interface IHTMLTableCell
		{
			String vAlign
			{
				[DispId(DispId.IHTMLTABLECELL_VALIGN)]
				get;
				[DispId(DispId.IHTMLTABLECELL_VALIGN)]
				set;
			}
			String background
			{
				[DispId(DispId.IHTMLTABLECELL_BACKGROUND)]
				get;
				[DispId(DispId.IHTMLTABLECELL_BACKGROUND)]
				set;
			}
			Int32 colSpan
			{
				[DispId(DispId.IHTMLTABLECELL_COLSPAN)]
				get;
				[DispId(DispId.IHTMLTABLECELL_COLSPAN)]
				set;
			}
			Int32 cellIndex
			{
				[DispId(DispId.IHTMLTABLECELL_CELLINDEX)]
				get;
			}
			Int32 rowSpan
			{
				[DispId(DispId.IHTMLTABLECELL_ROWSPAN)]
				get;
				[DispId(DispId.IHTMLTABLECELL_ROWSPAN)]
				set;
			}
			Boolean noWrap
			{
				[DispId(DispId.IHTMLTABLECELL_NOWRAP)]
				get;
				[DispId(DispId.IHTMLTABLECELL_NOWRAP)]
				set;
			}
			Object bgColor
			{
				[DispId(DispId.IHTMLTABLECELL_BGCOLOR)]
				get;
				[DispId(DispId.IHTMLTABLECELL_BGCOLOR)]
				set;
			}
			Object borderColor
			{
				[DispId(DispId.IHTMLTABLECELL_BORDERCOLOR)]
				get;
				[DispId(DispId.IHTMLTABLECELL_BORDERCOLOR)]
				set;
			}
			Object height
			{
				[DispId(DispId.IHTMLTABLECELL_HEIGHT)]
				get;
				[DispId(DispId.IHTMLTABLECELL_HEIGHT)]
				set;
			}
			Object borderColorLight
			{
				[DispId(DispId.IHTMLTABLECELL_BORDERCOLORLIGHT)]
				get;
				[DispId(DispId.IHTMLTABLECELL_BORDERCOLORLIGHT)]
				set;
			}
			Object width
			{
				[DispId(DispId.IHTMLTABLECELL_WIDTH)]
				get;
				[DispId(DispId.IHTMLTABLECELL_WIDTH)]
				set;
			}
			String align
			{
				[DispId(DispId.IHTMLTABLECELL_ALIGN)]
				get;
				[DispId(DispId.IHTMLTABLECELL_ALIGN)]
				set;
			}
			Object borderColorDark
			{
				[DispId(DispId.IHTMLTABLECELL_BORDERCOLORDARK)]
				get;
				[DispId(DispId.IHTMLTABLECELL_BORDERCOLORDARK)]
				set;
			}
		}

		[ComImport()]
		[Guid("3050f23c-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		public interface IHTMLTableRow
		{
			[DispId(DispId.IHTMLTABLEROW_INSERTCELL)]
			Object insertCell(Int32 index);

			[DispId(DispId.IHTMLTABLEROW_DELETECELL)]
			void deleteCell(Int32 index);

			String vAlign
			{
				[DispId(DispId.IHTMLTABLEROW_VALIGN)]
				get;
				[DispId(DispId.IHTMLTABLEROW_VALIGN)]
				set;
			}
			Object borderColorDark
			{
				[DispId(DispId.IHTMLTABLEROW_BORDERCOLORDARK)]
				get;
				[DispId(DispId.IHTMLTABLEROW_BORDERCOLORDARK)]
				set;
			}
			Object borderColor
			{
				[DispId(DispId.IHTMLTABLEROW_BORDERCOLOR)]
				get;
				[DispId(DispId.IHTMLTABLEROW_BORDERCOLOR)]
				set;
			}
			Int32 rowIndex
			{
				[DispId(DispId.IHTMLTABLEROW_ROWINDEX)]
				get;
			}
			IHTMLElementCollection cells
			{
				[DispId(DispId.IHTMLTABLEROW_CELLS)]
				get;
			}
			String align
			{
				[DispId(DispId.IHTMLTABLEROW_ALIGN)]
				get;
				[DispId(DispId.IHTMLTABLEROW_ALIGN)]
				set;
			}
			Object borderColorLight
			{
				[DispId(DispId.IHTMLTABLEROW_BORDERCOLORLIGHT)]
				get;
				[DispId(DispId.IHTMLTABLEROW_BORDERCOLORLIGHT)]
				set;
			}
			Int32 sectionRowIndex
			{
				[DispId(DispId.IHTMLTABLEROW_SECTIONROWINDEX)]
				get;
			}
			Object bgColor
			{
				[DispId(DispId.IHTMLTABLEROW_BGCOLOR)]
				get;
				[DispId(DispId.IHTMLTABLEROW_BGCOLOR)]
				set;
			}
		}

		[ComImport()]
		[Guid("3050f29c-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		public interface IHTMLControlRange
		{
			[DispId(DispId.IHTMLCONTROLRANGE_SELECT)]
			void select();

			[DispId(DispId.IHTMLCONTROLRANGE_ADD)]
			void add(IHTMLControlElement item);

			[DispId(DispId.IHTMLCONTROLRANGE_REMOVE)]
			void remove(Int32 index);

			[DispId(DispId.IHTMLCONTROLRANGE_ITEM)]
			IHTMLElement item(Int32 index);

			[DispId(DispId.IHTMLCONTROLRANGE_SCROLLINTOVIEW)]
			void scrollIntoView(Object varargStart);

			[DispId(DispId.IHTMLCONTROLRANGE_QUERYCOMMANDSUPPORTED)]
			Boolean queryCommandSupported(String cmdID);

			[DispId(DispId.IHTMLCONTROLRANGE_QUERYCOMMANDENABLED)]
			Boolean queryCommandEnabled(String cmdID);

			[DispId(DispId.IHTMLCONTROLRANGE_QUERYCOMMANDSTATE)]
			Boolean queryCommandState(String cmdID);

			[DispId(DispId.IHTMLCONTROLRANGE_QUERYCOMMANDINDETERM)]
			Boolean queryCommandIndeterm(String cmdID);

			[DispId(DispId.IHTMLCONTROLRANGE_QUERYCOMMANDTEXT)]
			String queryCommandText(String cmdID);

			[DispId(DispId.IHTMLCONTROLRANGE_QUERYCOMMANDVALUE)]
			Object queryCommandValue(String cmdID);

			[DispId(DispId.IHTMLCONTROLRANGE_EXECCOMMAND)]
			Boolean execCommand(String cmdID, Boolean showUI, Object value);

			[DispId(DispId.IHTMLCONTROLRANGE_EXECCOMMANDSHOWHELP)]
			Boolean execCommandShowHelp(String cmdID);

			[DispId(DispId.IHTMLCONTROLRANGE_COMMONPARENTELEMENT)]
			IHTMLElement commonParentElement();

			Int32 length
			{
				[DispId(DispId.IHTMLCONTROLRANGE_LENGTH)]
				get;
			}
		}

		[Guid("3050f65e-98b5-11cf-bb82-00aa00bdce0b"), InterfaceTypeAttribute(ComInterfaceType.InterfaceIsDual)]
		public interface IHTMLControlRange2
		{
			[return: MarshalAs(UnmanagedType.I4)]
			[PreserveSig]
			int addElement(
				[In, MarshalAs(UnmanagedType.Interface)]
				IHTMLElement element);
		}

		[ComImport()]
		[Guid("3050f4e9-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		public interface IHTMLControlElement
		{
			[DispId(DispId.IHTMLCONTROLELEMENT_FOCUS)]
			void focus();

			[DispId(DispId.IHTMLCONTROLELEMENT_BLUR)]
			void blur();

			[DispId(DispId.IHTMLCONTROLELEMENT_ADDFILTER)]
			void addFilter(Object pUnk);

			[DispId(DispId.IHTMLCONTROLELEMENT_REMOVEFILTER)]
			void removeFilter(Object pUnk);

			Object onblur
			{
				[DispId(DispId.IHTMLCONTROLELEMENT_ONBLUR)]
				get;
				[DispId(DispId.IHTMLCONTROLELEMENT_ONBLUR)]
				set;
			}
			Int32 clientWidth
			{
				[DispId(DispId.IHTMLCONTROLELEMENT_CLIENTWIDTH)]
				get;
			}
			Int16 tabIndex
			{
				[DispId(DispId.IHTMLCONTROLELEMENT_TABINDEX)]
				get;
				[DispId(DispId.IHTMLCONTROLELEMENT_TABINDEX)]
				set;
			}
			Int32 clientHeight
			{
				[DispId(DispId.IHTMLCONTROLELEMENT_CLIENTHEIGHT)]
				get;
			}
			Object onfocus
			{
				[DispId(DispId.IHTMLCONTROLELEMENT_ONFOCUS)]
				get;
				[DispId(DispId.IHTMLCONTROLELEMENT_ONFOCUS)]
				set;
			}
			String accessKey
			{
				[DispId(DispId.IHTMLCONTROLELEMENT_ACCESSKEY)]
				get;
				[DispId(DispId.IHTMLCONTROLELEMENT_ACCESSKEY)]
				set;
			}
			Object onresize
			{
				[DispId(DispId.IHTMLCONTROLELEMENT_ONRESIZE)]
				get;
				[DispId(DispId.IHTMLCONTROLELEMENT_ONRESIZE)]
				set;
			}
			Int32 clientTop
			{
				[DispId(DispId.IHTMLCONTROLELEMENT_CLIENTTOP)]
				get;
			}
			Int32 clientLeft
			{
				[DispId(DispId.IHTMLCONTROLELEMENT_CLIENTLEFT)]
				get;
			}
		}

		# region DOM Interfaces

		[ComImport()]
		[Guid("3050f5ab-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		public interface IHTMLDOMChildrenCollection
		{
			[DispId(DispId.IHTMLDOMCHILDRENCOLLECTION_ITEM)]
			Object item(Int32 index);

			Int32 length
			{
				[DispId(DispId.IHTMLDOMCHILDRENCOLLECTION_LENGTH)]
				get;
			}
			Object _newEnum
			{
				[DispId(DispId.IHTMLDOMCHILDRENCOLLECTION__NEWENUM)]
				get;
			}
		}

		[ComImport()]
		[Guid("3050f5da-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		public interface IHTMLDOMNode
		{
			[DispId(DispId.IHTMLDOMNODE_HASCHILDNODES)]
			Boolean hasChildNodes();

			[DispId(DispId.IHTMLDOMNODE_INSERTBEFORE)]
			IHTMLDOMNode insertBefore(IHTMLDOMNode newChild, Object refChild);

			[DispId(DispId.IHTMLDOMNODE_REMOVECHILD)]
			IHTMLDOMNode removeChild(IHTMLDOMNode oldChild);

			[DispId(DispId.IHTMLDOMNODE_REPLACECHILD)]
			IHTMLDOMNode replaceChild(IHTMLDOMNode newChild, IHTMLDOMNode oldChild);

			[DispId(DispId.IHTMLDOMNODE_CLONENODE)]
			IHTMLDOMNode cloneNode(Boolean fDeep);

			[DispId(DispId.IHTMLDOMNODE_REMOVENODE)]
			IHTMLDOMNode removeNode(Boolean fDeep);

			[DispId(DispId.IHTMLDOMNODE_SWAPNODE)]
			IHTMLDOMNode swapNode(IHTMLDOMNode otherNode);

			[DispId(DispId.IHTMLDOMNODE_REPLACENODE)]
			IHTMLDOMNode replaceNode(IHTMLDOMNode replacement);

			[DispId(DispId.IHTMLDOMNODE_APPENDCHILD)]
			IHTMLDOMNode appendChild(IHTMLDOMNode newChild);

			Object nodeValue
			{
				[DispId(DispId.IHTMLDOMNODE_NODEVALUE)]
				get;
				[DispId(DispId.IHTMLDOMNODE_NODEVALUE)]
				set;
			}
			IHTMLDOMNode previousSibling
			{
				[DispId(DispId.IHTMLDOMNODE_PREVIOUSSIBLING)]
				get;
			}
			Int32 nodeType
			{
				[DispId(DispId.IHTMLDOMNODE_NODETYPE)]
				get;
			}
			IHTMLDOMNode lastChild
			{
				[DispId(DispId.IHTMLDOMNODE_LASTCHILD)]
				get;
			}
			String nodeName
			{
				[DispId(DispId.IHTMLDOMNODE_NODENAME)]
				get;
			}
			IHTMLDOMNode firstChild
			{
				[DispId(DispId.IHTMLDOMNODE_FIRSTCHILD)]
				get;
			}
			IHTMLDOMNode parentNode
			{
				[DispId(DispId.IHTMLDOMNODE_PARENTNODE)]
				get;
			}
			Object childNodes
			{
				[DispId(DispId.IHTMLDOMNODE_CHILDNODES)]
				get;
			}
			Object attributes
			{
				[DispId(DispId.IHTMLDOMNODE_ATTRIBUTES)]
				get;
			}
			IHTMLDOMNode nextSibling
			{
				[DispId(DispId.IHTMLDOMNODE_NEXTSIBLING)]
				get;
			}
		}

		# endregion

		# region Markup and Change

		/// <exclude/>
		[ComImport()]
		[Guid("3050f648-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IMarkupContainer2
		{

			void OwningDoc(out IHTMLDocument2 ppDoc);

			void CreateChangeLog(
				[MarshalAs(UnmanagedType.Interface)]
				IHTMLChangeSink pChangeSink,
				[Out, MarshalAs(UnmanagedType.Interface)]
				out IHTMLChangeLog ppChangeLog,
				[In, MarshalAs(UnmanagedType.I4)] int fForward,
				[In, MarshalAs(UnmanagedType.I4)] int fBackward);

			void RegisterForDirtyRange(
				[MarshalAs(UnmanagedType.Interface)]
				IHTMLChangeSink pChangeSink,
				[Out] out UInt32 pdwCookie);

			void UnRegisterForDirtyRange([In] UInt32 dwCookie);

			void GetAndClearDirtyRange(
				UInt32 dwCookie,
				[MarshalAs(UnmanagedType.Interface)] IMarkupPointer pIPointerBegin,
				[MarshalAs(UnmanagedType.Interface)] IMarkupPointer pIPointerEnd);

			Int32 GetVersionNumber();

			void GetMasterElement(
				[MarshalAs(UnmanagedType.Interface), Out] out IHTMLElement ppElementMaster);

		}

		[ComImport()]
		[Guid("3050f64a-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IHTMLChangeSink
		{
			void Notify();

		}

		[ComImport()]
		[Guid("3050f6e0-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IHTMLChangePlayback
		{
			void ExecChange(ref Byte pbRecord, Int32 fForward);

		}

		[ComImport()]
		[Guid("3050f649-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IHTMLChangeLog
		{
			void GetNextChange([MarshalAs(UnmanagedType.LPWStr)] string pbBuffer, [In] Int32 nBufferSize, [Out] out Int32 pnRecordLength);
		}

		# endregion

		# region Security, Authentication

		[ComImport, GuidAttribute("79EAC9D0-BAF9-11CE-8C82-00AA004BA90B"),
			InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IAuthenticate
		{
			void Authenticate(IntPtr phwnd, ref string pszUsername, ref string pszPassword);
		}

		[ComImport, GuidAttribute("79eac9d7-bafa-11ce-8c82-00aa004ba90b"),
			InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IHttpSecurity
		{
			// Inherited from IWindowForBindingUI
			[return: MarshalAs(UnmanagedType.I4)]
			[PreserveSig]
			int GetWindow(ref Guid refGuid, ref IntPtr phWnd);

			[return: MarshalAs(UnmanagedType.I4)]
			[PreserveSig]
			int OnSecurityProblem(int dwProblem);
		}

		public interface IInternetSecurityMgrSite
		{
			void EnableModeless(bool fEnable);
			IntPtr GetWindow();
		}

		public interface IEnumString
		{
			IEnumString Clone();
			bool Next(uint celt, uint rgelt, uint pCeltFetched);
			void Reset();
			void Skip(uint celt);
		}

		[ComImport,
			GuidAttribute("79eac9ee-baf9-11ce-8c82-00aa004ba90b"),
			InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown),
			ComVisible(false)
			]
		public interface IInternetSecurityManager
		{

			[return: MarshalAs(UnmanagedType.I4)]
			[PreserveSig]
			int SetSecuritySite([In, MarshalAs(UnmanagedType.Interface)] IInternetSecurityMgrSite pSite);

			[return: MarshalAs(UnmanagedType.I4)]
			[PreserveSig]
			int GetSecuritySite([Out] out IInternetSecurityMgrSite pSite);

			[return: MarshalAs(UnmanagedType.I4)]
			[PreserveSig]
			int MapUrlToZone([In, MarshalAs(UnmanagedType.LPWStr)] string pwszUrl,
			   out URLZONE pdwZone, UInt32 dwFlags);

			[return: MarshalAs(UnmanagedType.I4)]
			[PreserveSig]
			int GetSecurityId([MarshalAs(UnmanagedType.LPWStr)] string pwszUrl,
					  [MarshalAs(UnmanagedType.LPArray)] byte[] pbSecurityId,
					  ref UInt32 pcbSecurityId, uint dwReserved);

			[return: MarshalAs(UnmanagedType.I4)]
			[PreserveSig]
			int ProcessUrlAction([In, MarshalAs(UnmanagedType.LPWStr)] string pwszUrl,
					 UInt32 dwAction, ref byte pPolicy, UInt32 cbPolicy,
					 byte pContext, UInt32 cbContext, UInt32 dwFlags,
					 UInt32 dwReserved);

			[return: MarshalAs(UnmanagedType.I4)]
			[PreserveSig]
			int QueryCustomPolicy([In, MarshalAs(UnmanagedType.LPWStr)] string pwszUrl,
					  ref Guid guidKey, ref byte ppPolicy, ref URLPOLICY pcbPolicy,
					  ref byte pContext, UInt32 cbContext, UInt32 dwReserved);

			[return: MarshalAs(UnmanagedType.I4)]
			[PreserveSig]
			int SetZoneMapping(UInt32 dwZone,
					   [In, MarshalAs(UnmanagedType.LPWStr)] string lpszPattern,
					  SZM_FLAG dwFlags);

			[return: MarshalAs(UnmanagedType.I4)]
			[PreserveSig]
			int GetZoneMappings(URLZONE dwZone, out UCOMIEnumString ppenumString,
				UInt32 dwFlags);
		}

		public enum PUAF
		{
			DEFAULT = 0x0000000,
			NOUI = 0x00000001,
			ISFILE = 0x00000002,
			WARN_IF_DENIED = 0x00000004,
			FORCEUI_FOREGROUND = 0x00000008,
			CHECK_TIFS = 0x00000010,
			DONTCHECKBOXINDIALOG = 0x00000020,
			TRUSTED = 0x00000040,
			ACCEPT_WILDCARD_SCHEME = 0x00000080,
			ENFORCERESTRICTED = 0x00000100,
			NOSAVEDFILECHECK = 0x00000200,
			REQUIRESAVEDFILECHECK = 0x00000400,
			LMZ_UNLOCKED = 0x00010000,
			LMZ_LOCKED = 0x00020000,
			DEFAULTZONEPOL = 0x00040000,
			NPL_USE_LOCKED_IF_RESTRICTED = 0x00080000,
			NOUIIFLOCKED = 0x00100000
		}

		public enum URLZONE : int
		{
			PREDEFINED_MIN = 0,
			LOCAL_MACHINE = 0,
			INTRANET,
			TRUSTED,
			INTERNET,
			UNTRUSTED,
			PREDEFINED_MAX = 999,
			USER_MIN = 1000,
			USER_MAX = 10000
		}

		public enum URLACTIONS : long
		{
			ACTIVEX_CONFIRM_NOOBJECTSAFETY = 0x00001204,
			ACTIVEX_CURR_MAX = 0x00001206,
			ACTIVEX_MAX = 0x000013ff,
			ACTIVEX_MIN = 0x00001200,
			ACTIVEX_OVERRIDE_DATA_SAFETY = 0x00001202,
			ACTIVEX_OVERRIDE_OBJECT_SAFETY = 0x00001201,
			ACTIVEX_OVERRIDE_SCRIPT_SAFETY = 0x00001203,
			ACTIVEX_RUN = 0x00001200,
			ACTIVEX_TREATASUNTRUSTED = 0x00001205,
			ALLOW_RESTRICTEDPROTOCOLS = 0x00002300,
			AUTHENTICATE_CLIENT = 0x00001A01,
			AUTOMATIC_ACTIVEX_UI = 0x00002201,
			AUTOMATIC_DOWNLOAD_UI = 0x00002200,
			AUTOMATIC_DOWNLOAD_UI_MIN = 0x00002200,
			BEHAVIOR_MIN = 0x00002000,
			BEHAVIOR_RUN = 0x00002000,
			CHANNEL_SOFTDIST_MAX = 0x00001Eff,
			CHANNEL_SOFTDIST_MIN = 0x00001E00,
			CHANNEL_SOFTDIST_PERMISSIONS = 0x00001E05,
			CLIENT_CERT_PROMPT = 0x00001A04,
			COOKIES = 0x00001A02,
			COOKIES_ENABLED = 0x00001A10,
			COOKIES_SESSION = 0x00001A03,
			COOKIES_SESSION_THIRD_PARTY = 0x00001A06,
			COOKIES_THIRD_PARTY = 0x00001A05,
			CREDENTIALS_USE = 0x00001A00,
			CROSS_DOMAIN_DATA = 0x00001406,
			DOWNLOAD_CURR_MAX = 0x00001004,
			DOWNLOAD_MAX = 0x000011FF,
			DOWNLOAD_MIN = 0x00001000,
			DOWNLOAD_SIGNED_ACTIVEX = 0x00001001,
			DOWNLOAD_UNSIGNED_ACTIVEX = 0x00001004,
			FEATURE_MIME_SNIFFING = 0x00002100,
			FEATURE_MIN = 0x00002100,
			FEATURE_ZONE_ELEVATION = 0x00002101,
			FEATURE_WINDOW_RESTRICTIONS = 0x00002102,
			HTML_CURR_MAX = 0x00001609,
			HTML_FONT_DOWNLOAD = 0x00001604,
			HTML_JAVA_RUN = 0x00001605,
			HTML_MAX = 0x000017ff,
			HTML_MIN = 0x00001600,
			HTML_MIXED_CONTENT = 0x00001609,
			HTML_SUBFRAME_NAVIGATE = 0x00001607,
			HTML_SUBMIT_FORMS = 0x00001601,
			HTML_SUBMIT_FORMS_FROM = 0x00001602,
			HTML_SUBMIT_FORMS_TO = 0x00001603,
			HTML_USERDATA_SAVE = 0x00001606,
			INFODELIVERY_CURR_MAX = 0x00001D06,
			INFODELIVERY_MAX = 0x00001Dff,
			INFODELIVERY_MIN = 0x00001D00,
			INFODELIVERY_NO_ADDING_CHANNELS = 0x00001D00,
			INFODELIVERY_NO_ADDING_SUBSCRIPTIONS = 0x00001D03,
			INFODELIVERY_NO_CHANNEL_LOGGING = 0x00001D06,
			INFODELIVERY_NO_EDITING_CHANNELS = 0x00001D01,
			INFODELIVERY_NO_EDITING_SUBSCRIPTIONS = 0x00001D04,
			INFODELIVERY_NO_REMOVING_CHANNELS = 0x00001D02,
			INFODELIVERY_NO_REMOVING_SUBSCRIPTIONS = 0x00001D05,
			JAVA_CURR_MAX = 0x00001C00,
			JAVA_MAX = 0x00001Cff,
			JAVA_MIN = 0x00001C00,
			JAVA_PERMISSIONS = 0x00001C00,
			NETWORK_CURR_MAX = 0x00001A10,
			NETWORK_MAX = 0x00001Bff,
			NETWORK_MIN = 0x00001A00,
			SCRIPT_CURR_MAX = 0x00001407,
			SCRIPT_JAVA_USE = 0x00001402,
			SCRIPT_MAX = 0x000015ff,
			SCRIPT_MIN = 0x00001400,
			SCRIPT_OVERRIDE_SAFETY = 0x00001401,
			SCRIPT_PASTE = 0x00001407,
			SCRIPT_RUN = 0x00001400,
			SCRIPT_SAFE_ACTIVEX = 0x00001405,
			SHELL_CURR_MAX = 0x00001809,
			SHELL_SHELLEXECUTE = 0x00001806,
			SHELL_EXECUTE_HIGHRISK = 0x00001806,
			SHELL_EXECUTE_LOWRISK = 0x00001808,
			SHELL_EXECUTE_MODRISK = 0x00001807,
			SHELL_FILE_DOWNLOAD = 0x00001803,
			SHELL_INSTALL_DTITEMS = 0x00001800,
			SHELL_MAX = 0x000019ff,
			SHELL_MIN = 0x00001800,
			SHELL_MOVE_OR_COPY = 0x00001802,
			SHELL_POPUPMGR = 0x00001809,
			SHELL_VERB = 0x00001804,
			SHELL_WEBVIEW_VERB = 0x00001805
		}

		public enum SZM_FLAG : int
		{
			SZM_CREATE = 0x00000000,
			SZM_DELETE = 0x00000001
		}

		public enum URLPOLICY : int
		{
			ACTIVEX_CHECK_LIST = 0x00010000,
			ALLOW = 0x00,
			AUTHENTICATE_CHALLENGE_RESPONSE = 0x00010000,
			AUTHENTICATE_CLEARTEXT_OK = 0x00000000,
			AUTHENTICATE_MUTUAL_ONLY = 0x00030000,
			BEHAVIOR_CHECK_LIST = 0x00010000,
			CHANNEL_SOFTDIST_AUTOINSTALL = 0x00030000,
			CHANNEL_SOFTDIST_PRECACHE = 0x00020000,
			CHANNEL_SOFTDIST_PROHIBIT = 0x00010000,
			CREDENTIALS_ANONYMOUS_ONLY = 0x00030000,
			CREDENTIALS_CONDITIONAL_PROMPT = 0x00020000,
			CREDENTIALS_MUST_PROMPT_USER = 0x00010000,
			CREDENTIALS_SILENT_LOGON_OK = 0x00000000,
			DISALLOW = 0x03,
			JAVA_CUSTOM = 0x00800000,
			JAVA_HIGH = 0x00010000,
			JAVA_LOW = 0x00030000,
			JAVA_MEDIUM = 0x00020000,
			JAVA_PROHIBIT = 0x00000000,
			MASK_PERMISSIONS = 0x0f,
			QUERY = 0x01,
		}

		# endregion

		[ComImport()]
		[Guid("3050f4b7-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		public interface IHTMLGenericElement
		{
			[DispId(DispId.IHTMLGENERICELEMENT_NAMEDRECORDSET)]
			Object namedRecordset(String dataMember, Object hierarchy);

			Object recordset
			{
				[DispId(DispId.IHTMLGENERICELEMENT_RECORDSET)]
				get;
			}
		}

		# region Events

		[Guid("3050F33C-98B5-11CF-BB82-00AA00BDCE0B"), InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IHTMLGenericEvents
		{
			void Bogus1();
			void Bogus2();
			void Bogus3();
			void Invoke(
				[In, MarshalAs(UnmanagedType.U4)] int id,
				[In] ref Guid g,
				[In, MarshalAs(UnmanagedType.U4)] int lcid,
				[In, MarshalAs(UnmanagedType.U4)] int dwFlags,
				[In] DISPPARAMS pdp,
				[Out, MarshalAs(UnmanagedType.LPArray)]
				Object[] pvarRes,
				[Out]
				EXCEPINFO pei,
				[Out, MarshalAs(UnmanagedType.LPArray)]
				int[] nArgError);
		}

		[ComImport()]
		[Guid("3050f60f-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		public interface IHTMLElementEvents2
		{
			[DispId(DispId.HTMLELEMENTEVENTS2_ONHELP)]
			Boolean onhelp(IHTMLEventObj pEvtObj);

			[DispId(DispId.HTMLELEMENTEVENTS2_ONCLICK)]
			Boolean onclick(IHTMLEventObj pEvtObj);

			[DispId(DispId.HTMLELEMENTEVENTS2_ONDBLCLICK)]
			Boolean ondblclick(IHTMLEventObj pEvtObj);

			[DispId(DispId.HTMLELEMENTEVENTS2_ONKEYPRESS)]
			Boolean onkeypress(IHTMLEventObj pEvtObj);

			[DispId(DispId.HTMLELEMENTEVENTS2_ONKEYDOWN)]
			void onkeydown(IHTMLEventObj pEvtObj);

			[DispId(DispId.HTMLELEMENTEVENTS2_ONKEYUP)]
			void onkeyup(IHTMLEventObj pEvtObj);

			[DispId(DispId.HTMLELEMENTEVENTS2_ONMOUSEOUT)]
			void onmouseout(IHTMLEventObj pEvtObj);

			[DispId(DispId.HTMLELEMENTEVENTS2_ONMOUSEOVER)]
			void onmouseover(IHTMLEventObj pEvtObj);

			[DispId(DispId.HTMLELEMENTEVENTS2_ONMOUSEMOVE)]
			void onmousemove(IHTMLEventObj pEvtObj);

			[DispId(DispId.HTMLELEMENTEVENTS2_ONMOUSEDOWN)]
			void onmousedown(IHTMLEventObj pEvtObj);

			[DispId(DispId.HTMLELEMENTEVENTS2_ONMOUSEUP)]
			void onmouseup(IHTMLEventObj pEvtObj);

			[DispId(DispId.HTMLELEMENTEVENTS2_ONSELECTSTART)]
			Boolean onselectstart(IHTMLEventObj pEvtObj);

			[DispId(DispId.HTMLELEMENTEVENTS2_ONFILTERCHANGE)]
			void onfilterchange(IHTMLEventObj pEvtObj);

			[DispId(DispId.HTMLELEMENTEVENTS2_ONDRAGSTART)]
			Boolean ondragstart(IHTMLEventObj pEvtObj);

			[DispId(DispId.HTMLELEMENTEVENTS2_ONBEFOREUPDATE)]
			Boolean onbeforeupdate(IHTMLEventObj pEvtObj);

			[DispId(DispId.HTMLELEMENTEVENTS2_ONAFTERUPDATE)]
			void onafterupdate(IHTMLEventObj pEvtObj);

			[DispId(DispId.HTMLELEMENTEVENTS2_ONERRORUPDATE)]
			Boolean onerrorupdate(IHTMLEventObj pEvtObj);

			[DispId(DispId.HTMLELEMENTEVENTS2_ONROWEXIT)]
			Boolean onrowexit(IHTMLEventObj pEvtObj);

			[DispId(DispId.HTMLELEMENTEVENTS2_ONROWENTER)]
			void onrowenter(IHTMLEventObj pEvtObj);

			[DispId(DispId.HTMLELEMENTEVENTS2_ONDATASETCHANGED)]
			void ondatasetchanged(IHTMLEventObj pEvtObj);

			[DispId(DispId.HTMLELEMENTEVENTS2_ONDATAAVAILABLE)]
			void ondataavailable(IHTMLEventObj pEvtObj);

			[DispId(DispId.HTMLELEMENTEVENTS2_ONDATASETCOMPLETE)]
			void ondatasetcomplete(IHTMLEventObj pEvtObj);

			[DispId(DispId.HTMLELEMENTEVENTS2_ONLOSECAPTURE)]
			void onlosecapture(IHTMLEventObj pEvtObj);

			[DispId(DispId.HTMLELEMENTEVENTS2_ONPROPERTYCHANGE)]
			void onpropertychange(IHTMLEventObj pEvtObj);

			[DispId(DispId.HTMLELEMENTEVENTS2_ONSCROLL)]
			void onscroll(IHTMLEventObj pEvtObj);

			[DispId(DispId.HTMLELEMENTEVENTS2_ONFOCUS)]
			void onfocus(IHTMLEventObj pEvtObj);

			[DispId(DispId.HTMLELEMENTEVENTS2_ONBLUR)]
			void onblur(IHTMLEventObj pEvtObj);

			[DispId(DispId.HTMLELEMENTEVENTS2_ONRESIZE)]
			void onresize(IHTMLEventObj pEvtObj);

			[DispId(DispId.HTMLELEMENTEVENTS2_ONDRAG)]
			Boolean ondrag(IHTMLEventObj pEvtObj);

			[DispId(DispId.HTMLELEMENTEVENTS2_ONDRAGEND)]
			void ondragend(IHTMLEventObj pEvtObj);

			[DispId(DispId.HTMLELEMENTEVENTS2_ONDRAGENTER)]
			Boolean ondragenter(IHTMLEventObj pEvtObj);

			[DispId(DispId.HTMLELEMENTEVENTS2_ONDRAGOVER)]
			Boolean ondragover(IHTMLEventObj pEvtObj);

			[DispId(DispId.HTMLELEMENTEVENTS2_ONDRAGLEAVE)]
			void ondragleave(IHTMLEventObj pEvtObj);

			[DispId(DispId.HTMLELEMENTEVENTS2_ONDROP)]
			Boolean ondrop(IHTMLEventObj pEvtObj);

			[DispId(DispId.HTMLELEMENTEVENTS2_ONBEFORECUT)]
			Boolean onbeforecut(IHTMLEventObj pEvtObj);

			[DispId(DispId.HTMLELEMENTEVENTS2_ONCUT)]
			Boolean oncut(IHTMLEventObj pEvtObj);

			[DispId(DispId.HTMLELEMENTEVENTS2_ONBEFORECOPY)]
			Boolean onbeforecopy(IHTMLEventObj pEvtObj);

			[DispId(DispId.HTMLELEMENTEVENTS2_ONCOPY)]
			Boolean oncopy(IHTMLEventObj pEvtObj);

			[DispId(DispId.HTMLELEMENTEVENTS2_ONBEFOREPASTE)]
			Boolean onbeforepaste(IHTMLEventObj pEvtObj);

			[DispId(DispId.HTMLELEMENTEVENTS2_ONPASTE)]
			Boolean onpaste(IHTMLEventObj pEvtObj);

			[DispId(DispId.HTMLELEMENTEVENTS2_ONCONTEXTMENU)]
			Boolean oncontextmenu(IHTMLEventObj pEvtObj);

			[DispId(DispId.HTMLELEMENTEVENTS2_ONROWSDELETE)]
			void onrowsdelete(IHTMLEventObj pEvtObj);

			[DispId(DispId.HTMLELEMENTEVENTS2_ONROWSINSERTED)]
			void onrowsinserted(IHTMLEventObj pEvtObj);

			[DispId(DispId.HTMLELEMENTEVENTS2_ONCELLCHANGE)]
			void oncellchange(IHTMLEventObj pEvtObj);

			[DispId(DispId.HTMLELEMENTEVENTS2_ONREADYSTATECHANGE)]
			void onreadystatechange(IHTMLEventObj pEvtObj);

			[DispId(DispId.HTMLELEMENTEVENTS2_ONLAYOUTCOMPLETE)]
			void onlayoutcomplete(IHTMLEventObj pEvtObj);

			[DispId(DispId.HTMLELEMENTEVENTS2_ONPAGE)]
			void onpage(IHTMLEventObj pEvtObj);

			[DispId(DispId.HTMLELEMENTEVENTS2_ONMOUSEENTER)]
			void onmouseenter(IHTMLEventObj pEvtObj);

			[DispId(DispId.HTMLELEMENTEVENTS2_ONMOUSELEAVE)]
			void onmouseleave(IHTMLEventObj pEvtObj);

			[DispId(DispId.HTMLELEMENTEVENTS2_ONACTIVATE)]
			void onactivate(IHTMLEventObj pEvtObj);

			[DispId(DispId.HTMLELEMENTEVENTS2_ONDEACTIVATE)]
			void ondeactivate(IHTMLEventObj pEvtObj);

			[DispId(DispId.HTMLELEMENTEVENTS2_ONBEFOREDEACTIVATE)]
			Boolean onbeforedeactivate(IHTMLEventObj pEvtObj);

			[DispId(DispId.HTMLELEMENTEVENTS2_ONBEFOREACTIVATE)]
			Boolean onbeforeactivate(IHTMLEventObj pEvtObj);

			[DispId(DispId.HTMLELEMENTEVENTS2_ONFOCUSIN)]
			void onfocusin(IHTMLEventObj pEvtObj);

			[DispId(DispId.HTMLELEMENTEVENTS2_ONFOCUSOUT)]
			void onfocusout(IHTMLEventObj pEvtObj);

			[DispId(DispId.HTMLELEMENTEVENTS2_ONMOVE)]
			void onmove(IHTMLEventObj pEvtObj);

			[DispId(DispId.HTMLELEMENTEVENTS2_ONCONTROLSELECT)]
			Boolean oncontrolselect(IHTMLEventObj pEvtObj);

			[DispId(DispId.HTMLELEMENTEVENTS2_ONMOVESTART)]
			Boolean onmovestart(IHTMLEventObj pEvtObj);

			[DispId(DispId.HTMLELEMENTEVENTS2_ONMOVEEND)]
			void onmoveend(IHTMLEventObj pEvtObj);

			[DispId(DispId.HTMLELEMENTEVENTS2_ONRESIZESTART)]
			Boolean onresizestart(IHTMLEventObj pEvtObj);

			[DispId(DispId.HTMLELEMENTEVENTS2_ONRESIZEEND)]
			void onresizeend(IHTMLEventObj pEvtObj);

			[DispId(DispId.HTMLELEMENTEVENTS2_ONMOUSEWHEEL)]
			Boolean onmousewheel(IHTMLEventObj pEvtObj);

		}

		[ComImport()]
		[Guid("3050f33c-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		public interface IHTMLElementEvents
		{
			[DispId(DispId.HTMLELEMENTEVENTS_ONHELP)]
			Boolean onhelp();

			[DispId(DispId.HTMLELEMENTEVENTS_ONCLICK)]
			Boolean onclick();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDBLCLICK)]
			Boolean ondblclick();

			[DispId(DispId.HTMLELEMENTEVENTS_ONKEYPRESS)]
			Boolean onkeypress();

			[DispId(DispId.HTMLELEMENTEVENTS_ONKEYDOWN)]
			void onkeydown();

			[DispId(DispId.HTMLELEMENTEVENTS_ONKEYUP)]
			void onkeyup();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEOUT)]
			void onmouseout();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEOVER)]
			void onmouseover();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEMOVE)]
			void onmousemove();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEDOWN)]
			void onmousedown();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEUP)]
			void onmouseup();

			[DispId(DispId.HTMLELEMENTEVENTS_ONSELECTSTART)]
			Boolean onselectstart();

			[DispId(DispId.HTMLELEMENTEVENTS_ONFILTERCHANGE)]
			void onfilterchange();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDRAGSTART)]
			Boolean ondragstart();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFOREUPDATE)]
			Boolean onbeforeupdate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONAFTERUPDATE)]
			void onafterupdate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONERRORUPDATE)]
			Boolean onerrorupdate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONROWEXIT)]
			Boolean onrowexit();

			[DispId(DispId.HTMLELEMENTEVENTS_ONROWENTER)]
			void onrowenter();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDATASETCHANGED)]
			void ondatasetchanged();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDATAAVAILABLE)]
			void ondataavailable();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDATASETCOMPLETE)]
			void ondatasetcomplete();

			[DispId(DispId.HTMLELEMENTEVENTS_ONLOSECAPTURE)]
			void onlosecapture();

			[DispId(DispId.HTMLELEMENTEVENTS_ONPROPERTYCHANGE)]
			void onpropertychange();

			[DispId(DispId.HTMLELEMENTEVENTS_ONSCROLL)]
			void onscroll();

			[DispId(DispId.HTMLELEMENTEVENTS_ONFOCUS)]
			void onfocus();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBLUR)]
			void onblur();

			[DispId(DispId.HTMLELEMENTEVENTS_ONRESIZE)]
			void onresize();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDRAG)]
			Boolean ondrag();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDRAGEND)]
			void ondragend();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDRAGENTER)]
			Boolean ondragenter();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDRAGOVER)]
			Boolean ondragover();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDRAGLEAVE)]
			void ondragleave();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDROP)]
			Boolean ondrop();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFORECUT)]
			Boolean onbeforecut();

			[DispId(DispId.HTMLELEMENTEVENTS_ONCUT)]
			Boolean oncut();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFORECOPY)]
			Boolean onbeforecopy();

			[DispId(DispId.HTMLELEMENTEVENTS_ONCOPY)]
			Boolean oncopy();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFOREPASTE)]
			Boolean onbeforepaste();

			[DispId(DispId.HTMLELEMENTEVENTS_ONPASTE)]
			Boolean onpaste();

			[DispId(DispId.HTMLELEMENTEVENTS_ONCONTEXTMENU)]
			Boolean oncontextmenu();

			[DispId(DispId.HTMLELEMENTEVENTS_ONROWSDELETE)]
			void onrowsdelete();

			[DispId(DispId.HTMLELEMENTEVENTS_ONROWSINSERTED)]
			void onrowsinserted();

			[DispId(DispId.HTMLELEMENTEVENTS_ONCELLCHANGE)]
			void oncellchange();

			[DispId(DispId.HTMLELEMENTEVENTS_ONREADYSTATECHANGE)]
			void onreadystatechange();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFOREEDITFOCUS)]
			void onbeforeeditfocus();

			[DispId(DispId.HTMLELEMENTEVENTS_ONLAYOUTCOMPLETE)]
			void onlayoutcomplete();

			[DispId(DispId.HTMLELEMENTEVENTS_ONPAGE)]
			void onpage();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFOREDEACTIVATE)]
			Boolean onbeforedeactivate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFOREACTIVATE)]
			Boolean onbeforeactivate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOVE)]
			void onmove();

			[DispId(DispId.HTMLELEMENTEVENTS_ONCONTROLSELECT)]
			Boolean oncontrolselect();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOVESTART)]
			Boolean onmovestart();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOVEEND)]
			void onmoveend();

			[DispId(DispId.HTMLELEMENTEVENTS_ONRESIZESTART)]
			Boolean onresizestart();

			[DispId(DispId.HTMLELEMENTEVENTS_ONRESIZEEND)]
			void onresizeend();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEENTER)]
			void onmouseenter();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSELEAVE)]
			void onmouseleave();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEWHEEL)]
			Boolean onmousewheel();

			[DispId(DispId.HTMLELEMENTEVENTS_ONACTIVATE)]
			void onactivate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDEACTIVATE)]
			void ondeactivate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONFOCUSIN)]
			void onfocusin();

			[DispId(DispId.HTMLELEMENTEVENTS_ONFOCUSOUT)]
			void onfocusout();

		}


		[ComImport()]
		[Guid("3050f29d-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		public interface IHTMLAnchorEvents
		{
			[DispId(DispId.HTMLELEMENTEVENTS_ONHELP)]
			Boolean onhelp();

			[DispId(DispId.HTMLELEMENTEVENTS_ONCLICK)]
			Boolean onclick();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDBLCLICK)]
			Boolean ondblclick();

			[DispId(DispId.HTMLELEMENTEVENTS_ONKEYPRESS)]
			Boolean onkeypress();

			[DispId(DispId.HTMLELEMENTEVENTS_ONKEYDOWN)]
			void onkeydown();

			[DispId(DispId.HTMLELEMENTEVENTS_ONKEYUP)]
			void onkeyup();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEOUT)]
			void onmouseout();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEOVER)]
			void onmouseover();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEMOVE)]
			void onmousemove();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEDOWN)]
			void onmousedown();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEUP)]
			void onmouseup();

			[DispId(DispId.HTMLELEMENTEVENTS_ONSELECTSTART)]
			Boolean onselectstart();

			[DispId(DispId.HTMLELEMENTEVENTS_ONFILTERCHANGE)]
			void onfilterchange();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDRAGSTART)]
			Boolean ondragstart();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFOREUPDATE)]
			Boolean onbeforeupdate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONAFTERUPDATE)]
			void onafterupdate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONERRORUPDATE)]
			Boolean onerrorupdate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONROWEXIT)]
			Boolean onrowexit();

			[DispId(DispId.HTMLELEMENTEVENTS_ONROWENTER)]
			void onrowenter();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDATASETCHANGED)]
			void ondatasetchanged();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDATAAVAILABLE)]
			void ondataavailable();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDATASETCOMPLETE)]
			void ondatasetcomplete();

			[DispId(DispId.HTMLELEMENTEVENTS_ONLOSECAPTURE)]
			void onlosecapture();

			[DispId(DispId.HTMLELEMENTEVENTS_ONPROPERTYCHANGE)]
			void onpropertychange();

			[DispId(DispId.HTMLELEMENTEVENTS_ONSCROLL)]
			void onscroll();

			[DispId(DispId.HTMLELEMENTEVENTS_ONFOCUS)]
			void onfocus();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBLUR)]
			void onblur();

			[DispId(DispId.HTMLELEMENTEVENTS_ONRESIZE)]
			void onresize();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDRAG)]
			Boolean ondrag();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDRAGEND)]
			void ondragend();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDRAGENTER)]
			Boolean ondragenter();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDRAGOVER)]
			Boolean ondragover();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDRAGLEAVE)]
			void ondragleave();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDROP)]
			Boolean ondrop();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFORECUT)]
			Boolean onbeforecut();

			[DispId(DispId.HTMLELEMENTEVENTS_ONCUT)]
			Boolean oncut();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFORECOPY)]
			Boolean onbeforecopy();

			[DispId(DispId.HTMLELEMENTEVENTS_ONCOPY)]
			Boolean oncopy();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFOREPASTE)]
			Boolean onbeforepaste();

			[DispId(DispId.HTMLELEMENTEVENTS_ONPASTE)]
			Boolean onpaste();

			[DispId(DispId.HTMLELEMENTEVENTS_ONCONTEXTMENU)]
			Boolean oncontextmenu();

			[DispId(DispId.HTMLELEMENTEVENTS_ONROWSDELETE)]
			void onrowsdelete();

			[DispId(DispId.HTMLELEMENTEVENTS_ONROWSINSERTED)]
			void onrowsinserted();

			[DispId(DispId.HTMLELEMENTEVENTS_ONCELLCHANGE)]
			void oncellchange();

			[DispId(DispId.HTMLELEMENTEVENTS_ONREADYSTATECHANGE)]
			void onreadystatechange();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFOREEDITFOCUS)]
			void onbeforeeditfocus();

			[DispId(DispId.HTMLELEMENTEVENTS_ONLAYOUTCOMPLETE)]
			void onlayoutcomplete();

			[DispId(DispId.HTMLELEMENTEVENTS_ONPAGE)]
			void onpage();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFOREDEACTIVATE)]
			Boolean onbeforedeactivate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFOREACTIVATE)]
			Boolean onbeforeactivate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOVE)]
			void onmove();

			[DispId(DispId.HTMLELEMENTEVENTS_ONCONTROLSELECT)]
			Boolean oncontrolselect();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOVESTART)]
			Boolean onmovestart();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOVEEND)]
			void onmoveend();

			[DispId(DispId.HTMLELEMENTEVENTS_ONRESIZESTART)]
			Boolean onresizestart();

			[DispId(DispId.HTMLELEMENTEVENTS_ONRESIZEEND)]
			void onresizeend();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEENTER)]
			void onmouseenter();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSELEAVE)]
			void onmouseleave();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEWHEEL)]
			Boolean onmousewheel();

			[DispId(DispId.HTMLELEMENTEVENTS_ONACTIVATE)]
			void onactivate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDEACTIVATE)]
			void ondeactivate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONFOCUSIN)]
			void onfocusin();

			[DispId(DispId.HTMLELEMENTEVENTS_ONFOCUSOUT)]
			void onfocusout();

		}

		[ComImport()]
		[Guid("3050f25b-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		public interface IHTMLImgEvents
		{
			[DispId(DispId.HTMLELEMENTEVENTS_ONHELP)]
			Boolean onhelp();

			[DispId(DispId.HTMLELEMENTEVENTS_ONCLICK)]
			Boolean onclick();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDBLCLICK)]
			Boolean ondblclick();

			[DispId(DispId.HTMLELEMENTEVENTS_ONKEYPRESS)]
			Boolean onkeypress();

			[DispId(DispId.HTMLELEMENTEVENTS_ONKEYDOWN)]
			void onkeydown();

			[DispId(DispId.HTMLELEMENTEVENTS_ONKEYUP)]
			void onkeyup();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEOUT)]
			void onmouseout();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEOVER)]
			void onmouseover();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEMOVE)]
			void onmousemove();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEDOWN)]
			void onmousedown();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEUP)]
			void onmouseup();

			[DispId(DispId.HTMLELEMENTEVENTS_ONSELECTSTART)]
			Boolean onselectstart();

			[DispId(DispId.HTMLELEMENTEVENTS_ONFILTERCHANGE)]
			void onfilterchange();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDRAGSTART)]
			Boolean ondragstart();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFOREUPDATE)]
			Boolean onbeforeupdate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONAFTERUPDATE)]
			void onafterupdate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONERRORUPDATE)]
			Boolean onerrorupdate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONROWEXIT)]
			Boolean onrowexit();

			[DispId(DispId.HTMLELEMENTEVENTS_ONROWENTER)]
			void onrowenter();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDATASETCHANGED)]
			void ondatasetchanged();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDATAAVAILABLE)]
			void ondataavailable();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDATASETCOMPLETE)]
			void ondatasetcomplete();

			[DispId(DispId.HTMLELEMENTEVENTS_ONLOSECAPTURE)]
			void onlosecapture();

			[DispId(DispId.HTMLELEMENTEVENTS_ONPROPERTYCHANGE)]
			void onpropertychange();

			[DispId(DispId.HTMLELEMENTEVENTS_ONSCROLL)]
			void onscroll();

			[DispId(DispId.HTMLELEMENTEVENTS_ONFOCUS)]
			void onfocus();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBLUR)]
			void onblur();

			[DispId(DispId.HTMLELEMENTEVENTS_ONRESIZE)]
			void onresize();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDRAG)]
			Boolean ondrag();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDRAGEND)]
			void ondragend();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDRAGENTER)]
			Boolean ondragenter();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDRAGOVER)]
			Boolean ondragover();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDRAGLEAVE)]
			void ondragleave();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDROP)]
			Boolean ondrop();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFORECUT)]
			Boolean onbeforecut();

			[DispId(DispId.HTMLELEMENTEVENTS_ONCUT)]
			Boolean oncut();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFORECOPY)]
			Boolean onbeforecopy();

			[DispId(DispId.HTMLELEMENTEVENTS_ONCOPY)]
			Boolean oncopy();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFOREPASTE)]
			Boolean onbeforepaste();

			[DispId(DispId.HTMLELEMENTEVENTS_ONPASTE)]
			Boolean onpaste();

			[DispId(DispId.HTMLELEMENTEVENTS_ONCONTEXTMENU)]
			Boolean oncontextmenu();

			[DispId(DispId.HTMLELEMENTEVENTS_ONROWSDELETE)]
			void onrowsdelete();

			[DispId(DispId.HTMLELEMENTEVENTS_ONROWSINSERTED)]
			void onrowsinserted();

			[DispId(DispId.HTMLELEMENTEVENTS_ONCELLCHANGE)]
			void oncellchange();

			[DispId(DispId.HTMLELEMENTEVENTS_ONREADYSTATECHANGE)]
			void onreadystatechange();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFOREEDITFOCUS)]
			void onbeforeeditfocus();

			[DispId(DispId.HTMLELEMENTEVENTS_ONLAYOUTCOMPLETE)]
			void onlayoutcomplete();

			[DispId(DispId.HTMLELEMENTEVENTS_ONPAGE)]
			void onpage();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFOREDEACTIVATE)]
			Boolean onbeforedeactivate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFOREACTIVATE)]
			Boolean onbeforeactivate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOVE)]
			void onmove();

			[DispId(DispId.HTMLELEMENTEVENTS_ONCONTROLSELECT)]
			Boolean oncontrolselect();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOVESTART)]
			Boolean onmovestart();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOVEEND)]
			void onmoveend();

			[DispId(DispId.HTMLELEMENTEVENTS_ONRESIZESTART)]
			Boolean onresizestart();

			[DispId(DispId.HTMLELEMENTEVENTS_ONRESIZEEND)]
			void onresizeend();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEENTER)]
			void onmouseenter();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSELEAVE)]
			void onmouseleave();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEWHEEL)]
			Boolean onmousewheel();

			[DispId(DispId.HTMLELEMENTEVENTS_ONACTIVATE)]
			void onactivate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDEACTIVATE)]
			void ondeactivate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONFOCUSIN)]
			void onfocusin();

			[DispId(DispId.HTMLELEMENTEVENTS_ONFOCUSOUT)]
			void onfocusout();

			[DispId(DispId.HTMLIMGEVENTS_ONLOAD)]
			void onload();

			[DispId(DispId.HTMLIMGEVENTS_ONERROR)]
			void onerror();

			[DispId(DispId.HTMLIMGEVENTS_ONABORT)]
			void onabort();

		}

		[ComImport()]
		[Guid("3050f366-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		public interface IHTMLAreaEvents
		{
			[DispId(DispId.HTMLELEMENTEVENTS_ONHELP)]
			Boolean onhelp();

			[DispId(DispId.HTMLELEMENTEVENTS_ONCLICK)]
			Boolean onclick();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDBLCLICK)]
			Boolean ondblclick();

			[DispId(DispId.HTMLELEMENTEVENTS_ONKEYPRESS)]
			Boolean onkeypress();

			[DispId(DispId.HTMLELEMENTEVENTS_ONKEYDOWN)]
			void onkeydown();

			[DispId(DispId.HTMLELEMENTEVENTS_ONKEYUP)]
			void onkeyup();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEOUT)]
			void onmouseout();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEOVER)]
			void onmouseover();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEMOVE)]
			void onmousemove();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEDOWN)]
			void onmousedown();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEUP)]
			void onmouseup();

			[DispId(DispId.HTMLELEMENTEVENTS_ONSELECTSTART)]
			Boolean onselectstart();

			[DispId(DispId.HTMLELEMENTEVENTS_ONFILTERCHANGE)]
			void onfilterchange();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDRAGSTART)]
			Boolean ondragstart();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFOREUPDATE)]
			Boolean onbeforeupdate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONAFTERUPDATE)]
			void onafterupdate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONERRORUPDATE)]
			Boolean onerrorupdate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONROWEXIT)]
			Boolean onrowexit();

			[DispId(DispId.HTMLELEMENTEVENTS_ONROWENTER)]
			void onrowenter();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDATASETCHANGED)]
			void ondatasetchanged();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDATAAVAILABLE)]
			void ondataavailable();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDATASETCOMPLETE)]
			void ondatasetcomplete();

			[DispId(DispId.HTMLELEMENTEVENTS_ONLOSECAPTURE)]
			void onlosecapture();

			[DispId(DispId.HTMLELEMENTEVENTS_ONPROPERTYCHANGE)]
			void onpropertychange();

			[DispId(DispId.HTMLELEMENTEVENTS_ONSCROLL)]
			void onscroll();

			[DispId(DispId.HTMLELEMENTEVENTS_ONFOCUS)]
			void onfocus();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBLUR)]
			void onblur();

			[DispId(DispId.HTMLELEMENTEVENTS_ONRESIZE)]
			void onresize();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDRAG)]
			Boolean ondrag();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDRAGEND)]
			void ondragend();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDRAGENTER)]
			Boolean ondragenter();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDRAGOVER)]
			Boolean ondragover();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDRAGLEAVE)]
			void ondragleave();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDROP)]
			Boolean ondrop();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFORECUT)]
			Boolean onbeforecut();

			[DispId(DispId.HTMLELEMENTEVENTS_ONCUT)]
			Boolean oncut();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFORECOPY)]
			Boolean onbeforecopy();

			[DispId(DispId.HTMLELEMENTEVENTS_ONCOPY)]
			Boolean oncopy();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFOREPASTE)]
			Boolean onbeforepaste();

			[DispId(DispId.HTMLELEMENTEVENTS_ONPASTE)]
			Boolean onpaste();

			[DispId(DispId.HTMLELEMENTEVENTS_ONCONTEXTMENU)]
			Boolean oncontextmenu();

			[DispId(DispId.HTMLELEMENTEVENTS_ONROWSDELETE)]
			void onrowsdelete();

			[DispId(DispId.HTMLELEMENTEVENTS_ONROWSINSERTED)]
			void onrowsinserted();

			[DispId(DispId.HTMLELEMENTEVENTS_ONCELLCHANGE)]
			void oncellchange();

			[DispId(DispId.HTMLELEMENTEVENTS_ONREADYSTATECHANGE)]
			void onreadystatechange();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFOREEDITFOCUS)]
			void onbeforeeditfocus();

			[DispId(DispId.HTMLELEMENTEVENTS_ONLAYOUTCOMPLETE)]
			void onlayoutcomplete();

			[DispId(DispId.HTMLELEMENTEVENTS_ONPAGE)]
			void onpage();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFOREDEACTIVATE)]
			Boolean onbeforedeactivate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFOREACTIVATE)]
			Boolean onbeforeactivate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOVE)]
			void onmove();

			[DispId(DispId.HTMLELEMENTEVENTS_ONCONTROLSELECT)]
			Boolean oncontrolselect();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOVESTART)]
			Boolean onmovestart();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOVEEND)]
			void onmoveend();

			[DispId(DispId.HTMLELEMENTEVENTS_ONRESIZESTART)]
			Boolean onresizestart();

			[DispId(DispId.HTMLELEMENTEVENTS_ONRESIZEEND)]
			void onresizeend();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEENTER)]
			void onmouseenter();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSELEAVE)]
			void onmouseleave();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEWHEEL)]
			Boolean onmousewheel();

			[DispId(DispId.HTMLELEMENTEVENTS_ONACTIVATE)]
			void onactivate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDEACTIVATE)]
			void ondeactivate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONFOCUSIN)]
			void onfocusin();

			[DispId(DispId.HTMLELEMENTEVENTS_ONFOCUSOUT)]
			void onfocusout();

		}
		[ComImport()]
		[Guid("3050f2b3-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		public interface IHTMLButtonElementEvents
		{
			[DispId(DispId.HTMLELEMENTEVENTS_ONHELP)]
			Boolean onhelp();

			[DispId(DispId.HTMLELEMENTEVENTS_ONCLICK)]
			Boolean onclick();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDBLCLICK)]
			Boolean ondblclick();

			[DispId(DispId.HTMLELEMENTEVENTS_ONKEYPRESS)]
			Boolean onkeypress();

			[DispId(DispId.HTMLELEMENTEVENTS_ONKEYDOWN)]
			void onkeydown();

			[DispId(DispId.HTMLELEMENTEVENTS_ONKEYUP)]
			void onkeyup();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEOUT)]
			void onmouseout();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEOVER)]
			void onmouseover();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEMOVE)]
			void onmousemove();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEDOWN)]
			void onmousedown();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEUP)]
			void onmouseup();

			[DispId(DispId.HTMLELEMENTEVENTS_ONSELECTSTART)]
			Boolean onselectstart();

			[DispId(DispId.HTMLELEMENTEVENTS_ONFILTERCHANGE)]
			void onfilterchange();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDRAGSTART)]
			Boolean ondragstart();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFOREUPDATE)]
			Boolean onbeforeupdate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONAFTERUPDATE)]
			void onafterupdate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONERRORUPDATE)]
			Boolean onerrorupdate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONROWEXIT)]
			Boolean onrowexit();

			[DispId(DispId.HTMLELEMENTEVENTS_ONROWENTER)]
			void onrowenter();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDATASETCHANGED)]
			void ondatasetchanged();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDATAAVAILABLE)]
			void ondataavailable();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDATASETCOMPLETE)]
			void ondatasetcomplete();

			[DispId(DispId.HTMLELEMENTEVENTS_ONLOSECAPTURE)]
			void onlosecapture();

			[DispId(DispId.HTMLELEMENTEVENTS_ONPROPERTYCHANGE)]
			void onpropertychange();

			[DispId(DispId.HTMLELEMENTEVENTS_ONSCROLL)]
			void onscroll();

			[DispId(DispId.HTMLELEMENTEVENTS_ONFOCUS)]
			void onfocus();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBLUR)]
			void onblur();

			[DispId(DispId.HTMLELEMENTEVENTS_ONRESIZE)]
			void onresize();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDRAG)]
			Boolean ondrag();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDRAGEND)]
			void ondragend();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDRAGENTER)]
			Boolean ondragenter();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDRAGOVER)]
			Boolean ondragover();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDRAGLEAVE)]
			void ondragleave();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDROP)]
			Boolean ondrop();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFORECUT)]
			Boolean onbeforecut();

			[DispId(DispId.HTMLELEMENTEVENTS_ONCUT)]
			Boolean oncut();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFORECOPY)]
			Boolean onbeforecopy();

			[DispId(DispId.HTMLELEMENTEVENTS_ONCOPY)]
			Boolean oncopy();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFOREPASTE)]
			Boolean onbeforepaste();

			[DispId(DispId.HTMLELEMENTEVENTS_ONPASTE)]
			Boolean onpaste();

			[DispId(DispId.HTMLELEMENTEVENTS_ONCONTEXTMENU)]
			Boolean oncontextmenu();

			[DispId(DispId.HTMLELEMENTEVENTS_ONROWSDELETE)]
			void onrowsdelete();

			[DispId(DispId.HTMLELEMENTEVENTS_ONROWSINSERTED)]
			void onrowsinserted();

			[DispId(DispId.HTMLELEMENTEVENTS_ONCELLCHANGE)]
			void oncellchange();

			[DispId(DispId.HTMLELEMENTEVENTS_ONREADYSTATECHANGE)]
			void onreadystatechange();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFOREEDITFOCUS)]
			void onbeforeeditfocus();

			[DispId(DispId.HTMLELEMENTEVENTS_ONLAYOUTCOMPLETE)]
			void onlayoutcomplete();

			[DispId(DispId.HTMLELEMENTEVENTS_ONPAGE)]
			void onpage();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFOREDEACTIVATE)]
			Boolean onbeforedeactivate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFOREACTIVATE)]
			Boolean onbeforeactivate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOVE)]
			void onmove();

			[DispId(DispId.HTMLELEMENTEVENTS_ONCONTROLSELECT)]
			Boolean oncontrolselect();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOVESTART)]
			Boolean onmovestart();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOVEEND)]
			void onmoveend();

			[DispId(DispId.HTMLELEMENTEVENTS_ONRESIZESTART)]
			Boolean onresizestart();

			[DispId(DispId.HTMLELEMENTEVENTS_ONRESIZEEND)]
			void onresizeend();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEENTER)]
			void onmouseenter();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSELEAVE)]
			void onmouseleave();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEWHEEL)]
			Boolean onmousewheel();

			[DispId(DispId.HTMLELEMENTEVENTS_ONACTIVATE)]
			void onactivate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDEACTIVATE)]
			void ondeactivate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONFOCUSIN)]
			void onfocusin();

			[DispId(DispId.HTMLELEMENTEVENTS_ONFOCUSOUT)]
			void onfocusout();

		}

		[ComImport()]
		[Guid("3050f4ea-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		public interface IHTMLControlElementEvents
		{
			[DispId(DispId.HTMLELEMENTEVENTS_ONHELP)]
			Boolean onhelp();

			[DispId(DispId.HTMLELEMENTEVENTS_ONCLICK)]
			Boolean onclick();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDBLCLICK)]
			Boolean ondblclick();

			[DispId(DispId.HTMLELEMENTEVENTS_ONKEYPRESS)]
			Boolean onkeypress();

			[DispId(DispId.HTMLELEMENTEVENTS_ONKEYDOWN)]
			void onkeydown();

			[DispId(DispId.HTMLELEMENTEVENTS_ONKEYUP)]
			void onkeyup();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEOUT)]
			void onmouseout();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEOVER)]
			void onmouseover();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEMOVE)]
			void onmousemove();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEDOWN)]
			void onmousedown();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEUP)]
			void onmouseup();

			[DispId(DispId.HTMLELEMENTEVENTS_ONSELECTSTART)]
			Boolean onselectstart();

			[DispId(DispId.HTMLELEMENTEVENTS_ONFILTERCHANGE)]
			void onfilterchange();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDRAGSTART)]
			Boolean ondragstart();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFOREUPDATE)]
			Boolean onbeforeupdate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONAFTERUPDATE)]
			void onafterupdate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONERRORUPDATE)]
			Boolean onerrorupdate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONROWEXIT)]
			Boolean onrowexit();

			[DispId(DispId.HTMLELEMENTEVENTS_ONROWENTER)]
			void onrowenter();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDATASETCHANGED)]
			void ondatasetchanged();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDATAAVAILABLE)]
			void ondataavailable();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDATASETCOMPLETE)]
			void ondatasetcomplete();

			[DispId(DispId.HTMLELEMENTEVENTS_ONLOSECAPTURE)]
			void onlosecapture();

			[DispId(DispId.HTMLELEMENTEVENTS_ONPROPERTYCHANGE)]
			void onpropertychange();

			[DispId(DispId.HTMLELEMENTEVENTS_ONSCROLL)]
			void onscroll();

			[DispId(DispId.HTMLELEMENTEVENTS_ONFOCUS)]
			void onfocus();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBLUR)]
			void onblur();

			[DispId(DispId.HTMLELEMENTEVENTS_ONRESIZE)]
			void onresize();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDRAG)]
			Boolean ondrag();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDRAGEND)]
			void ondragend();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDRAGENTER)]
			Boolean ondragenter();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDRAGOVER)]
			Boolean ondragover();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDRAGLEAVE)]
			void ondragleave();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDROP)]
			Boolean ondrop();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFORECUT)]
			Boolean onbeforecut();

			[DispId(DispId.HTMLELEMENTEVENTS_ONCUT)]
			Boolean oncut();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFORECOPY)]
			Boolean onbeforecopy();

			[DispId(DispId.HTMLELEMENTEVENTS_ONCOPY)]
			Boolean oncopy();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFOREPASTE)]
			Boolean onbeforepaste();

			[DispId(DispId.HTMLELEMENTEVENTS_ONPASTE)]
			Boolean onpaste();

			[DispId(DispId.HTMLELEMENTEVENTS_ONCONTEXTMENU)]
			Boolean oncontextmenu();

			[DispId(DispId.HTMLELEMENTEVENTS_ONROWSDELETE)]
			void onrowsdelete();

			[DispId(DispId.HTMLELEMENTEVENTS_ONROWSINSERTED)]
			void onrowsinserted();

			[DispId(DispId.HTMLELEMENTEVENTS_ONCELLCHANGE)]
			void oncellchange();

			[DispId(DispId.HTMLELEMENTEVENTS_ONREADYSTATECHANGE)]
			void onreadystatechange();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFOREEDITFOCUS)]
			void onbeforeeditfocus();

			[DispId(DispId.HTMLELEMENTEVENTS_ONLAYOUTCOMPLETE)]
			void onlayoutcomplete();

			[DispId(DispId.HTMLELEMENTEVENTS_ONPAGE)]
			void onpage();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFOREDEACTIVATE)]
			Boolean onbeforedeactivate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFOREACTIVATE)]
			Boolean onbeforeactivate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOVE)]
			void onmove();

			[DispId(DispId.HTMLELEMENTEVENTS_ONCONTROLSELECT)]
			Boolean oncontrolselect();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOVESTART)]
			Boolean onmovestart();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOVEEND)]
			void onmoveend();

			[DispId(DispId.HTMLELEMENTEVENTS_ONRESIZESTART)]
			Boolean onresizestart();

			[DispId(DispId.HTMLELEMENTEVENTS_ONRESIZEEND)]
			void onresizeend();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEENTER)]
			void onmouseenter();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSELEAVE)]
			void onmouseleave();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEWHEEL)]
			Boolean onmousewheel();

			[DispId(DispId.HTMLELEMENTEVENTS_ONACTIVATE)]
			void onactivate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDEACTIVATE)]
			void ondeactivate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONFOCUSIN)]
			void onfocusin();

			[DispId(DispId.HTMLELEMENTEVENTS_ONFOCUSOUT)]
			void onfocusout();

		}

		[ComImport()]
		[Guid("3050f260-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		public interface IHTMLDocumentEvents
		{
			[DispId(DispId.HTMLDOCUMENTEVENTS_ONHELP)]
			Boolean onhelp();

			[DispId(DispId.HTMLDOCUMENTEVENTS_ONCLICK)]
			Boolean onclick();

			[DispId(DispId.HTMLDOCUMENTEVENTS_ONDBLCLICK)]
			Boolean ondblclick();

			[DispId(DispId.HTMLDOCUMENTEVENTS_ONKEYDOWN)]
			void onkeydown();

			[DispId(DispId.HTMLDOCUMENTEVENTS_ONKEYUP)]
			void onkeyup();

			[DispId(DispId.HTMLDOCUMENTEVENTS_ONKEYPRESS)]
			Boolean onkeypress();

			[DispId(DispId.HTMLDOCUMENTEVENTS_ONMOUSEDOWN)]
			void onmousedown();

			[DispId(DispId.HTMLDOCUMENTEVENTS_ONMOUSEMOVE)]
			void onmousemove();

			[DispId(DispId.HTMLDOCUMENTEVENTS_ONMOUSEUP)]
			void onmouseup();

			[DispId(DispId.HTMLDOCUMENTEVENTS_ONMOUSEOUT)]
			void onmouseout();

			[DispId(DispId.HTMLDOCUMENTEVENTS_ONMOUSEOVER)]
			void onmouseover();

			[DispId(DispId.HTMLDOCUMENTEVENTS_ONREADYSTATECHANGE)]
			void onreadystatechange();

			[DispId(DispId.HTMLDOCUMENTEVENTS_ONBEFOREUPDATE)]
			Boolean onbeforeupdate();

			[DispId(DispId.HTMLDOCUMENTEVENTS_ONAFTERUPDATE)]
			void onafterupdate();

			[DispId(DispId.HTMLDOCUMENTEVENTS_ONROWEXIT)]
			Boolean onrowexit();

			[DispId(DispId.HTMLDOCUMENTEVENTS_ONROWENTER)]
			void onrowenter();

			[DispId(DispId.HTMLDOCUMENTEVENTS_ONDRAGSTART)]
			Boolean ondragstart();

			[DispId(DispId.HTMLDOCUMENTEVENTS_ONSELECTSTART)]
			Boolean onselectstart();

			[DispId(DispId.HTMLDOCUMENTEVENTS_ONERRORUPDATE)]
			Boolean onerrorupdate();

			[DispId(DispId.HTMLDOCUMENTEVENTS_ONCONTEXTMENU)]
			Boolean oncontextmenu();

			[DispId(DispId.HTMLDOCUMENTEVENTS_ONSTOP)]
			Boolean onstop();

			[DispId(DispId.HTMLDOCUMENTEVENTS_ONROWSDELETE)]
			void onrowsdelete();

			[DispId(DispId.HTMLDOCUMENTEVENTS_ONROWSINSERTED)]
			void onrowsinserted();

			[DispId(DispId.HTMLDOCUMENTEVENTS_ONCELLCHANGE)]
			void oncellchange();

			[DispId(DispId.HTMLDOCUMENTEVENTS_ONPROPERTYCHANGE)]
			void onpropertychange();

			[DispId(DispId.HTMLDOCUMENTEVENTS_ONDATASETCHANGED)]
			void ondatasetchanged();

			[DispId(DispId.HTMLDOCUMENTEVENTS_ONDATAAVAILABLE)]
			void ondataavailable();

			[DispId(DispId.HTMLDOCUMENTEVENTS_ONDATASETCOMPLETE)]
			void ondatasetcomplete();

			[DispId(DispId.HTMLDOCUMENTEVENTS_ONBEFOREEDITFOCUS)]
			void onbeforeeditfocus();

			[DispId(DispId.HTMLDOCUMENTEVENTS_ONSELECTIONCHANGE)]
			void onselectionchange();

			[DispId(DispId.HTMLDOCUMENTEVENTS_ONCONTROLSELECT)]
			Boolean oncontrolselect();

			[DispId(DispId.HTMLDOCUMENTEVENTS_ONMOUSEWHEEL)]
			Boolean onmousewheel();

			[DispId(DispId.HTMLDOCUMENTEVENTS_ONFOCUSIN)]
			void onfocusin();

			[DispId(DispId.HTMLDOCUMENTEVENTS_ONFOCUSOUT)]
			void onfocusout();

			[DispId(DispId.HTMLDOCUMENTEVENTS_ONACTIVATE)]
			void onactivate();

			[DispId(DispId.HTMLDOCUMENTEVENTS_ONDEACTIVATE)]
			void ondeactivate();

			[DispId(DispId.HTMLDOCUMENTEVENTS_ONBEFOREACTIVATE)]
			Boolean onbeforeactivate();

			[DispId(DispId.HTMLDOCUMENTEVENTS_ONBEFOREDEACTIVATE)]
			Boolean onbeforedeactivate();

		}

		[ComImport()]
		[Guid("3050f364-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		public interface IHTMLFormElementEvents
		{
			[DispId(DispId.HTMLELEMENTEVENTS_ONHELP)]
			Boolean onhelp();

			[DispId(DispId.HTMLELEMENTEVENTS_ONCLICK)]
			Boolean onclick();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDBLCLICK)]
			Boolean ondblclick();

			[DispId(DispId.HTMLELEMENTEVENTS_ONKEYPRESS)]
			Boolean onkeypress();

			[DispId(DispId.HTMLELEMENTEVENTS_ONKEYDOWN)]
			void onkeydown();

			[DispId(DispId.HTMLELEMENTEVENTS_ONKEYUP)]
			void onkeyup();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEOUT)]
			void onmouseout();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEOVER)]
			void onmouseover();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEMOVE)]
			void onmousemove();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEDOWN)]
			void onmousedown();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEUP)]
			void onmouseup();

			[DispId(DispId.HTMLELEMENTEVENTS_ONSELECTSTART)]
			Boolean onselectstart();

			[DispId(DispId.HTMLELEMENTEVENTS_ONFILTERCHANGE)]
			void onfilterchange();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDRAGSTART)]
			Boolean ondragstart();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFOREUPDATE)]
			Boolean onbeforeupdate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONAFTERUPDATE)]
			void onafterupdate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONERRORUPDATE)]
			Boolean onerrorupdate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONROWEXIT)]
			Boolean onrowexit();

			[DispId(DispId.HTMLELEMENTEVENTS_ONROWENTER)]
			void onrowenter();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDATASETCHANGED)]
			void ondatasetchanged();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDATAAVAILABLE)]
			void ondataavailable();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDATASETCOMPLETE)]
			void ondatasetcomplete();

			[DispId(DispId.HTMLELEMENTEVENTS_ONLOSECAPTURE)]
			void onlosecapture();

			[DispId(DispId.HTMLELEMENTEVENTS_ONPROPERTYCHANGE)]
			void onpropertychange();

			[DispId(DispId.HTMLELEMENTEVENTS_ONSCROLL)]
			void onscroll();

			[DispId(DispId.HTMLELEMENTEVENTS_ONFOCUS)]
			void onfocus();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBLUR)]
			void onblur();

			[DispId(DispId.HTMLELEMENTEVENTS_ONRESIZE)]
			void onresize();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDRAG)]
			Boolean ondrag();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDRAGEND)]
			void ondragend();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDRAGENTER)]
			Boolean ondragenter();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDRAGOVER)]
			Boolean ondragover();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDRAGLEAVE)]
			void ondragleave();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDROP)]
			Boolean ondrop();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFORECUT)]
			Boolean onbeforecut();

			[DispId(DispId.HTMLELEMENTEVENTS_ONCUT)]
			Boolean oncut();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFORECOPY)]
			Boolean onbeforecopy();

			[DispId(DispId.HTMLELEMENTEVENTS_ONCOPY)]
			Boolean oncopy();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFOREPASTE)]
			Boolean onbeforepaste();

			[DispId(DispId.HTMLELEMENTEVENTS_ONPASTE)]
			Boolean onpaste();

			[DispId(DispId.HTMLELEMENTEVENTS_ONCONTEXTMENU)]
			Boolean oncontextmenu();

			[DispId(DispId.HTMLELEMENTEVENTS_ONROWSDELETE)]
			void onrowsdelete();

			[DispId(DispId.HTMLELEMENTEVENTS_ONROWSINSERTED)]
			void onrowsinserted();

			[DispId(DispId.HTMLELEMENTEVENTS_ONCELLCHANGE)]
			void oncellchange();

			[DispId(DispId.HTMLELEMENTEVENTS_ONREADYSTATECHANGE)]
			void onreadystatechange();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFOREEDITFOCUS)]
			void onbeforeeditfocus();

			[DispId(DispId.HTMLELEMENTEVENTS_ONLAYOUTCOMPLETE)]
			void onlayoutcomplete();

			[DispId(DispId.HTMLELEMENTEVENTS_ONPAGE)]
			void onpage();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFOREDEACTIVATE)]
			Boolean onbeforedeactivate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFOREACTIVATE)]
			Boolean onbeforeactivate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOVE)]
			void onmove();

			[DispId(DispId.HTMLELEMENTEVENTS_ONCONTROLSELECT)]
			Boolean oncontrolselect();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOVESTART)]
			Boolean onmovestart();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOVEEND)]
			void onmoveend();

			[DispId(DispId.HTMLELEMENTEVENTS_ONRESIZESTART)]
			Boolean onresizestart();

			[DispId(DispId.HTMLELEMENTEVENTS_ONRESIZEEND)]
			void onresizeend();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEENTER)]
			void onmouseenter();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSELEAVE)]
			void onmouseleave();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEWHEEL)]
			Boolean onmousewheel();

			[DispId(DispId.HTMLELEMENTEVENTS_ONACTIVATE)]
			void onactivate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDEACTIVATE)]
			void ondeactivate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONFOCUSIN)]
			void onfocusin();

			[DispId(DispId.HTMLELEMENTEVENTS_ONFOCUSOUT)]
			void onfocusout();

			[DispId(DispId.HTMLFORMELEMENTEVENTS_ONSUBMIT)]
			Boolean onsubmit();

			[DispId(DispId.HTMLFORMELEMENTEVENTS_ONRESET)]
			Boolean onreset();

		}

		[ComImport()]
		[Guid("3050f800-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		public interface IHTMLFrameSiteEvents
		{
			[DispId(DispId.HTMLELEMENTEVENTS_ONHELP)]
			Boolean onhelp();

			[DispId(DispId.HTMLELEMENTEVENTS_ONCLICK)]
			Boolean onclick();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDBLCLICK)]
			Boolean ondblclick();

			[DispId(DispId.HTMLELEMENTEVENTS_ONKEYPRESS)]
			Boolean onkeypress();

			[DispId(DispId.HTMLELEMENTEVENTS_ONKEYDOWN)]
			void onkeydown();

			[DispId(DispId.HTMLELEMENTEVENTS_ONKEYUP)]
			void onkeyup();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEOUT)]
			void onmouseout();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEOVER)]
			void onmouseover();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEMOVE)]
			void onmousemove();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEDOWN)]
			void onmousedown();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEUP)]
			void onmouseup();

			[DispId(DispId.HTMLELEMENTEVENTS_ONSELECTSTART)]
			Boolean onselectstart();

			[DispId(DispId.HTMLELEMENTEVENTS_ONFILTERCHANGE)]
			void onfilterchange();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDRAGSTART)]
			Boolean ondragstart();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFOREUPDATE)]
			Boolean onbeforeupdate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONAFTERUPDATE)]
			void onafterupdate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONERRORUPDATE)]
			Boolean onerrorupdate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONROWEXIT)]
			Boolean onrowexit();

			[DispId(DispId.HTMLELEMENTEVENTS_ONROWENTER)]
			void onrowenter();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDATASETCHANGED)]
			void ondatasetchanged();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDATAAVAILABLE)]
			void ondataavailable();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDATASETCOMPLETE)]
			void ondatasetcomplete();

			[DispId(DispId.HTMLELEMENTEVENTS_ONLOSECAPTURE)]
			void onlosecapture();

			[DispId(DispId.HTMLELEMENTEVENTS_ONPROPERTYCHANGE)]
			void onpropertychange();

			[DispId(DispId.HTMLELEMENTEVENTS_ONSCROLL)]
			void onscroll();

			[DispId(DispId.HTMLELEMENTEVENTS_ONFOCUS)]
			void onfocus();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBLUR)]
			void onblur();

			[DispId(DispId.HTMLELEMENTEVENTS_ONRESIZE)]
			void onresize();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDRAG)]
			Boolean ondrag();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDRAGEND)]
			void ondragend();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDRAGENTER)]
			Boolean ondragenter();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDRAGOVER)]
			Boolean ondragover();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDRAGLEAVE)]
			void ondragleave();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDROP)]
			Boolean ondrop();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFORECUT)]
			Boolean onbeforecut();

			[DispId(DispId.HTMLELEMENTEVENTS_ONCUT)]
			Boolean oncut();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFORECOPY)]
			Boolean onbeforecopy();

			[DispId(DispId.HTMLELEMENTEVENTS_ONCOPY)]
			Boolean oncopy();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFOREPASTE)]
			Boolean onbeforepaste();

			[DispId(DispId.HTMLELEMENTEVENTS_ONPASTE)]
			Boolean onpaste();

			[DispId(DispId.HTMLELEMENTEVENTS_ONCONTEXTMENU)]
			Boolean oncontextmenu();

			[DispId(DispId.HTMLELEMENTEVENTS_ONROWSDELETE)]
			void onrowsdelete();

			[DispId(DispId.HTMLELEMENTEVENTS_ONROWSINSERTED)]
			void onrowsinserted();

			[DispId(DispId.HTMLELEMENTEVENTS_ONCELLCHANGE)]
			void oncellchange();

			[DispId(DispId.HTMLELEMENTEVENTS_ONREADYSTATECHANGE)]
			void onreadystatechange();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFOREEDITFOCUS)]
			void onbeforeeditfocus();

			[DispId(DispId.HTMLELEMENTEVENTS_ONLAYOUTCOMPLETE)]
			void onlayoutcomplete();

			[DispId(DispId.HTMLELEMENTEVENTS_ONPAGE)]
			void onpage();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFOREDEACTIVATE)]
			Boolean onbeforedeactivate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFOREACTIVATE)]
			Boolean onbeforeactivate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOVE)]
			void onmove();

			[DispId(DispId.HTMLELEMENTEVENTS_ONCONTROLSELECT)]
			Boolean oncontrolselect();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOVESTART)]
			Boolean onmovestart();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOVEEND)]
			void onmoveend();

			[DispId(DispId.HTMLELEMENTEVENTS_ONRESIZESTART)]
			Boolean onresizestart();

			[DispId(DispId.HTMLELEMENTEVENTS_ONRESIZEEND)]
			void onresizeend();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEENTER)]
			void onmouseenter();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSELEAVE)]
			void onmouseleave();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEWHEEL)]
			Boolean onmousewheel();

			[DispId(DispId.HTMLELEMENTEVENTS_ONACTIVATE)]
			void onactivate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDEACTIVATE)]
			void ondeactivate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONFOCUSIN)]
			void onfocusin();

			[DispId(DispId.HTMLELEMENTEVENTS_ONFOCUSOUT)]
			void onfocusout();

			[DispId(DispId.HTMLFRAMESITEEVENTS_ONLOAD)]
			void onload();

		}

		[ComImport()]
		[Guid("3050f2af-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		public interface IHTMLInputFileElementEvents
		{
			[DispId(DispId.HTMLELEMENTEVENTS_ONHELP)]
			Boolean onhelp();

			[DispId(DispId.HTMLELEMENTEVENTS_ONCLICK)]
			Boolean onclick();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDBLCLICK)]
			Boolean ondblclick();

			[DispId(DispId.HTMLELEMENTEVENTS_ONKEYPRESS)]
			Boolean onkeypress();

			[DispId(DispId.HTMLELEMENTEVENTS_ONKEYDOWN)]
			void onkeydown();

			[DispId(DispId.HTMLELEMENTEVENTS_ONKEYUP)]
			void onkeyup();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEOUT)]
			void onmouseout();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEOVER)]
			void onmouseover();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEMOVE)]
			void onmousemove();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEDOWN)]
			void onmousedown();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEUP)]
			void onmouseup();

			[DispId(DispId.HTMLELEMENTEVENTS_ONSELECTSTART)]
			Boolean onselectstart();

			[DispId(DispId.HTMLELEMENTEVENTS_ONFILTERCHANGE)]
			void onfilterchange();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDRAGSTART)]
			Boolean ondragstart();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFOREUPDATE)]
			Boolean onbeforeupdate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONAFTERUPDATE)]
			void onafterupdate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONERRORUPDATE)]
			Boolean onerrorupdate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONROWEXIT)]
			Boolean onrowexit();

			[DispId(DispId.HTMLELEMENTEVENTS_ONROWENTER)]
			void onrowenter();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDATASETCHANGED)]
			void ondatasetchanged();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDATAAVAILABLE)]
			void ondataavailable();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDATASETCOMPLETE)]
			void ondatasetcomplete();

			[DispId(DispId.HTMLELEMENTEVENTS_ONLOSECAPTURE)]
			void onlosecapture();

			[DispId(DispId.HTMLELEMENTEVENTS_ONPROPERTYCHANGE)]
			void onpropertychange();

			[DispId(DispId.HTMLELEMENTEVENTS_ONSCROLL)]
			void onscroll();

			[DispId(DispId.HTMLELEMENTEVENTS_ONFOCUS)]
			void onfocus();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBLUR)]
			void onblur();

			[DispId(DispId.HTMLELEMENTEVENTS_ONRESIZE)]
			void onresize();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDRAG)]
			Boolean ondrag();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDRAGEND)]
			void ondragend();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDRAGENTER)]
			Boolean ondragenter();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDRAGOVER)]
			Boolean ondragover();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDRAGLEAVE)]
			void ondragleave();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDROP)]
			Boolean ondrop();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFORECUT)]
			Boolean onbeforecut();

			[DispId(DispId.HTMLELEMENTEVENTS_ONCUT)]
			Boolean oncut();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFORECOPY)]
			Boolean onbeforecopy();

			[DispId(DispId.HTMLELEMENTEVENTS_ONCOPY)]
			Boolean oncopy();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFOREPASTE)]
			Boolean onbeforepaste();

			[DispId(DispId.HTMLELEMENTEVENTS_ONPASTE)]
			Boolean onpaste();

			[DispId(DispId.HTMLELEMENTEVENTS_ONCONTEXTMENU)]
			Boolean oncontextmenu();

			[DispId(DispId.HTMLELEMENTEVENTS_ONROWSDELETE)]
			void onrowsdelete();

			[DispId(DispId.HTMLELEMENTEVENTS_ONROWSINSERTED)]
			void onrowsinserted();

			[DispId(DispId.HTMLELEMENTEVENTS_ONCELLCHANGE)]
			void oncellchange();

			[DispId(DispId.HTMLELEMENTEVENTS_ONREADYSTATECHANGE)]
			void onreadystatechange();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFOREEDITFOCUS)]
			void onbeforeeditfocus();

			[DispId(DispId.HTMLELEMENTEVENTS_ONLAYOUTCOMPLETE)]
			void onlayoutcomplete();

			[DispId(DispId.HTMLELEMENTEVENTS_ONPAGE)]
			void onpage();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFOREDEACTIVATE)]
			Boolean onbeforedeactivate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFOREACTIVATE)]
			Boolean onbeforeactivate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOVE)]
			void onmove();

			[DispId(DispId.HTMLELEMENTEVENTS_ONCONTROLSELECT)]
			Boolean oncontrolselect();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOVESTART)]
			Boolean onmovestart();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOVEEND)]
			void onmoveend();

			[DispId(DispId.HTMLELEMENTEVENTS_ONRESIZESTART)]
			Boolean onresizestart();

			[DispId(DispId.HTMLELEMENTEVENTS_ONRESIZEEND)]
			void onresizeend();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEENTER)]
			void onmouseenter();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSELEAVE)]
			void onmouseleave();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEWHEEL)]
			Boolean onmousewheel();

			[DispId(DispId.HTMLELEMENTEVENTS_ONACTIVATE)]
			void onactivate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDEACTIVATE)]
			void ondeactivate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONFOCUSIN)]
			void onfocusin();

			[DispId(DispId.HTMLELEMENTEVENTS_ONFOCUSOUT)]
			void onfocusout();

			[DispId(DispId.HTMLINPUTTEXTELEMENTEVENTS_ONCHANGE)]
			Boolean onchange();

			[DispId(DispId.HTMLINPUTTEXTELEMENTEVENTS_ONSELECT)]
			void onselect();

			[DispId(DispId.HTMLINPUTTEXTELEMENTEVENTS_ONLOAD)]
			void onload();

			[DispId(DispId.HTMLINPUTTEXTELEMENTEVENTS_ONERROR)]
			void onerror();

			[DispId(DispId.HTMLINPUTTEXTELEMENTEVENTS_ONABORT)]
			void onabort();

		}

		[ComImport()]
		[Guid("3050f2a7-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		public interface IHTMLInputTextElementEvents
		{
			[DispId(DispId.HTMLELEMENTEVENTS_ONHELP)]
			Boolean onhelp();

			[DispId(DispId.HTMLELEMENTEVENTS_ONCLICK)]
			Boolean onclick();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDBLCLICK)]
			Boolean ondblclick();

			[DispId(DispId.HTMLELEMENTEVENTS_ONKEYPRESS)]
			Boolean onkeypress();

			[DispId(DispId.HTMLELEMENTEVENTS_ONKEYDOWN)]
			void onkeydown();

			[DispId(DispId.HTMLELEMENTEVENTS_ONKEYUP)]
			void onkeyup();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEOUT)]
			void onmouseout();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEOVER)]
			void onmouseover();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEMOVE)]
			void onmousemove();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEDOWN)]
			void onmousedown();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEUP)]
			void onmouseup();

			[DispId(DispId.HTMLELEMENTEVENTS_ONSELECTSTART)]
			Boolean onselectstart();

			[DispId(DispId.HTMLELEMENTEVENTS_ONFILTERCHANGE)]
			void onfilterchange();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDRAGSTART)]
			Boolean ondragstart();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFOREUPDATE)]
			Boolean onbeforeupdate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONAFTERUPDATE)]
			void onafterupdate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONERRORUPDATE)]
			Boolean onerrorupdate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONROWEXIT)]
			Boolean onrowexit();

			[DispId(DispId.HTMLELEMENTEVENTS_ONROWENTER)]
			void onrowenter();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDATASETCHANGED)]
			void ondatasetchanged();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDATAAVAILABLE)]
			void ondataavailable();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDATASETCOMPLETE)]
			void ondatasetcomplete();

			[DispId(DispId.HTMLELEMENTEVENTS_ONLOSECAPTURE)]
			void onlosecapture();

			[DispId(DispId.HTMLELEMENTEVENTS_ONPROPERTYCHANGE)]
			void onpropertychange();

			[DispId(DispId.HTMLELEMENTEVENTS_ONSCROLL)]
			void onscroll();

			[DispId(DispId.HTMLELEMENTEVENTS_ONFOCUS)]
			void onfocus();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBLUR)]
			void onblur();

			[DispId(DispId.HTMLELEMENTEVENTS_ONRESIZE)]
			void onresize();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDRAG)]
			Boolean ondrag();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDRAGEND)]
			void ondragend();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDRAGENTER)]
			Boolean ondragenter();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDRAGOVER)]
			Boolean ondragover();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDRAGLEAVE)]
			void ondragleave();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDROP)]
			Boolean ondrop();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFORECUT)]
			Boolean onbeforecut();

			[DispId(DispId.HTMLELEMENTEVENTS_ONCUT)]
			Boolean oncut();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFORECOPY)]
			Boolean onbeforecopy();

			[DispId(DispId.HTMLELEMENTEVENTS_ONCOPY)]
			Boolean oncopy();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFOREPASTE)]
			Boolean onbeforepaste();

			[DispId(DispId.HTMLELEMENTEVENTS_ONPASTE)]
			Boolean onpaste();

			[DispId(DispId.HTMLELEMENTEVENTS_ONCONTEXTMENU)]
			Boolean oncontextmenu();

			[DispId(DispId.HTMLELEMENTEVENTS_ONROWSDELETE)]
			void onrowsdelete();

			[DispId(DispId.HTMLELEMENTEVENTS_ONROWSINSERTED)]
			void onrowsinserted();

			[DispId(DispId.HTMLELEMENTEVENTS_ONCELLCHANGE)]
			void oncellchange();

			[DispId(DispId.HTMLELEMENTEVENTS_ONREADYSTATECHANGE)]
			void onreadystatechange();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFOREEDITFOCUS)]
			void onbeforeeditfocus();

			[DispId(DispId.HTMLELEMENTEVENTS_ONLAYOUTCOMPLETE)]
			void onlayoutcomplete();

			[DispId(DispId.HTMLELEMENTEVENTS_ONPAGE)]
			void onpage();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFOREDEACTIVATE)]
			Boolean onbeforedeactivate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFOREACTIVATE)]
			Boolean onbeforeactivate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOVE)]
			void onmove();

			[DispId(DispId.HTMLELEMENTEVENTS_ONCONTROLSELECT)]
			Boolean oncontrolselect();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOVESTART)]
			Boolean onmovestart();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOVEEND)]
			void onmoveend();

			[DispId(DispId.HTMLELEMENTEVENTS_ONRESIZESTART)]
			Boolean onresizestart();

			[DispId(DispId.HTMLELEMENTEVENTS_ONRESIZEEND)]
			void onresizeend();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEENTER)]
			void onmouseenter();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSELEAVE)]
			void onmouseleave();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEWHEEL)]
			Boolean onmousewheel();

			[DispId(DispId.HTMLELEMENTEVENTS_ONACTIVATE)]
			void onactivate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDEACTIVATE)]
			void ondeactivate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONFOCUSIN)]
			void onfocusin();

			[DispId(DispId.HTMLELEMENTEVENTS_ONFOCUSOUT)]
			void onfocusout();

			[DispId(DispId.HTMLINPUTTEXTELEMENTEVENTS_ONCHANGE)]
			Boolean onchange();

			[DispId(DispId.HTMLINPUTTEXTELEMENTEVENTS_ONSELECT)]
			void onselect();

			[DispId(DispId.HTMLINPUTTEXTELEMENTEVENTS_ONLOAD)]
			void onload();

			[DispId(DispId.HTMLINPUTTEXTELEMENTEVENTS_ONERROR)]
			void onerror();

			[DispId(DispId.HTMLINPUTTEXTELEMENTEVENTS_ONABORT)]
			void onabort();

		}

		[ComImport()]
		[Guid("3050f2c3-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		public interface IHTMLInputImageEvents
		{
			[DispId(DispId.HTMLELEMENTEVENTS_ONHELP)]
			Boolean onhelp();

			[DispId(DispId.HTMLELEMENTEVENTS_ONCLICK)]
			Boolean onclick();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDBLCLICK)]
			Boolean ondblclick();

			[DispId(DispId.HTMLELEMENTEVENTS_ONKEYPRESS)]
			Boolean onkeypress();

			[DispId(DispId.HTMLELEMENTEVENTS_ONKEYDOWN)]
			void onkeydown();

			[DispId(DispId.HTMLELEMENTEVENTS_ONKEYUP)]
			void onkeyup();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEOUT)]
			void onmouseout();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEOVER)]
			void onmouseover();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEMOVE)]
			void onmousemove();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEDOWN)]
			void onmousedown();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEUP)]
			void onmouseup();

			[DispId(DispId.HTMLELEMENTEVENTS_ONSELECTSTART)]
			Boolean onselectstart();

			[DispId(DispId.HTMLELEMENTEVENTS_ONFILTERCHANGE)]
			void onfilterchange();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDRAGSTART)]
			Boolean ondragstart();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFOREUPDATE)]
			Boolean onbeforeupdate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONAFTERUPDATE)]
			void onafterupdate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONERRORUPDATE)]
			Boolean onerrorupdate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONROWEXIT)]
			Boolean onrowexit();

			[DispId(DispId.HTMLELEMENTEVENTS_ONROWENTER)]
			void onrowenter();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDATASETCHANGED)]
			void ondatasetchanged();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDATAAVAILABLE)]
			void ondataavailable();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDATASETCOMPLETE)]
			void ondatasetcomplete();

			[DispId(DispId.HTMLELEMENTEVENTS_ONLOSECAPTURE)]
			void onlosecapture();

			[DispId(DispId.HTMLELEMENTEVENTS_ONPROPERTYCHANGE)]
			void onpropertychange();

			[DispId(DispId.HTMLELEMENTEVENTS_ONSCROLL)]
			void onscroll();

			[DispId(DispId.HTMLELEMENTEVENTS_ONFOCUS)]
			void onfocus();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBLUR)]
			void onblur();

			[DispId(DispId.HTMLELEMENTEVENTS_ONRESIZE)]
			void onresize();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDRAG)]
			Boolean ondrag();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDRAGEND)]
			void ondragend();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDRAGENTER)]
			Boolean ondragenter();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDRAGOVER)]
			Boolean ondragover();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDRAGLEAVE)]
			void ondragleave();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDROP)]
			Boolean ondrop();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFORECUT)]
			Boolean onbeforecut();

			[DispId(DispId.HTMLELEMENTEVENTS_ONCUT)]
			Boolean oncut();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFORECOPY)]
			Boolean onbeforecopy();

			[DispId(DispId.HTMLELEMENTEVENTS_ONCOPY)]
			Boolean oncopy();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFOREPASTE)]
			Boolean onbeforepaste();

			[DispId(DispId.HTMLELEMENTEVENTS_ONPASTE)]
			Boolean onpaste();

			[DispId(DispId.HTMLELEMENTEVENTS_ONCONTEXTMENU)]
			Boolean oncontextmenu();

			[DispId(DispId.HTMLELEMENTEVENTS_ONROWSDELETE)]
			void onrowsdelete();

			[DispId(DispId.HTMLELEMENTEVENTS_ONROWSINSERTED)]
			void onrowsinserted();

			[DispId(DispId.HTMLELEMENTEVENTS_ONCELLCHANGE)]
			void oncellchange();

			[DispId(DispId.HTMLELEMENTEVENTS_ONREADYSTATECHANGE)]
			void onreadystatechange();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFOREEDITFOCUS)]
			void onbeforeeditfocus();

			[DispId(DispId.HTMLELEMENTEVENTS_ONLAYOUTCOMPLETE)]
			void onlayoutcomplete();

			[DispId(DispId.HTMLELEMENTEVENTS_ONPAGE)]
			void onpage();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFOREDEACTIVATE)]
			Boolean onbeforedeactivate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFOREACTIVATE)]
			Boolean onbeforeactivate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOVE)]
			void onmove();

			[DispId(DispId.HTMLELEMENTEVENTS_ONCONTROLSELECT)]
			Boolean oncontrolselect();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOVESTART)]
			Boolean onmovestart();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOVEEND)]
			void onmoveend();

			[DispId(DispId.HTMLELEMENTEVENTS_ONRESIZESTART)]
			Boolean onresizestart();

			[DispId(DispId.HTMLELEMENTEVENTS_ONRESIZEEND)]
			void onresizeend();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEENTER)]
			void onmouseenter();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSELEAVE)]
			void onmouseleave();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEWHEEL)]
			Boolean onmousewheel();

			[DispId(DispId.HTMLELEMENTEVENTS_ONACTIVATE)]
			void onactivate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDEACTIVATE)]
			void ondeactivate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONFOCUSIN)]
			void onfocusin();

			[DispId(DispId.HTMLELEMENTEVENTS_ONFOCUSOUT)]
			void onfocusout();

			[DispId(DispId.HTMLINPUTIMAGEEVENTS_ONLOAD)]
			void onload();

			[DispId(DispId.HTMLINPUTIMAGEEVENTS_ONERROR)]
			void onerror();

			[DispId(DispId.HTMLINPUTIMAGEEVENTS_ONABORT)]
			void onabort();

		}

		[ComImport()]
		[Guid("3050f329-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		public interface IHTMLLabelEvents
		{
			[DispId(DispId.HTMLELEMENTEVENTS_ONHELP)]
			Boolean onhelp();

			[DispId(DispId.HTMLELEMENTEVENTS_ONCLICK)]
			Boolean onclick();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDBLCLICK)]
			Boolean ondblclick();

			[DispId(DispId.HTMLELEMENTEVENTS_ONKEYPRESS)]
			Boolean onkeypress();

			[DispId(DispId.HTMLELEMENTEVENTS_ONKEYDOWN)]
			void onkeydown();

			[DispId(DispId.HTMLELEMENTEVENTS_ONKEYUP)]
			void onkeyup();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEOUT)]
			void onmouseout();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEOVER)]
			void onmouseover();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEMOVE)]
			void onmousemove();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEDOWN)]
			void onmousedown();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEUP)]
			void onmouseup();

			[DispId(DispId.HTMLELEMENTEVENTS_ONSELECTSTART)]
			Boolean onselectstart();

			[DispId(DispId.HTMLELEMENTEVENTS_ONFILTERCHANGE)]
			void onfilterchange();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDRAGSTART)]
			Boolean ondragstart();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFOREUPDATE)]
			Boolean onbeforeupdate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONAFTERUPDATE)]
			void onafterupdate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONERRORUPDATE)]
			Boolean onerrorupdate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONROWEXIT)]
			Boolean onrowexit();

			[DispId(DispId.HTMLELEMENTEVENTS_ONROWENTER)]
			void onrowenter();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDATASETCHANGED)]
			void ondatasetchanged();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDATAAVAILABLE)]
			void ondataavailable();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDATASETCOMPLETE)]
			void ondatasetcomplete();

			[DispId(DispId.HTMLELEMENTEVENTS_ONLOSECAPTURE)]
			void onlosecapture();

			[DispId(DispId.HTMLELEMENTEVENTS_ONPROPERTYCHANGE)]
			void onpropertychange();

			[DispId(DispId.HTMLELEMENTEVENTS_ONSCROLL)]
			void onscroll();

			[DispId(DispId.HTMLELEMENTEVENTS_ONFOCUS)]
			void onfocus();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBLUR)]
			void onblur();

			[DispId(DispId.HTMLELEMENTEVENTS_ONRESIZE)]
			void onresize();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDRAG)]
			Boolean ondrag();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDRAGEND)]
			void ondragend();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDRAGENTER)]
			Boolean ondragenter();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDRAGOVER)]
			Boolean ondragover();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDRAGLEAVE)]
			void ondragleave();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDROP)]
			Boolean ondrop();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFORECUT)]
			Boolean onbeforecut();

			[DispId(DispId.HTMLELEMENTEVENTS_ONCUT)]
			Boolean oncut();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFORECOPY)]
			Boolean onbeforecopy();

			[DispId(DispId.HTMLELEMENTEVENTS_ONCOPY)]
			Boolean oncopy();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFOREPASTE)]
			Boolean onbeforepaste();

			[DispId(DispId.HTMLELEMENTEVENTS_ONPASTE)]
			Boolean onpaste();

			[DispId(DispId.HTMLELEMENTEVENTS_ONCONTEXTMENU)]
			Boolean oncontextmenu();

			[DispId(DispId.HTMLELEMENTEVENTS_ONROWSDELETE)]
			void onrowsdelete();

			[DispId(DispId.HTMLELEMENTEVENTS_ONROWSINSERTED)]
			void onrowsinserted();

			[DispId(DispId.HTMLELEMENTEVENTS_ONCELLCHANGE)]
			void oncellchange();

			[DispId(DispId.HTMLELEMENTEVENTS_ONREADYSTATECHANGE)]
			void onreadystatechange();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFOREEDITFOCUS)]
			void onbeforeeditfocus();

			[DispId(DispId.HTMLELEMENTEVENTS_ONLAYOUTCOMPLETE)]
			void onlayoutcomplete();

			[DispId(DispId.HTMLELEMENTEVENTS_ONPAGE)]
			void onpage();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFOREDEACTIVATE)]
			Boolean onbeforedeactivate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFOREACTIVATE)]
			Boolean onbeforeactivate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOVE)]
			void onmove();

			[DispId(DispId.HTMLELEMENTEVENTS_ONCONTROLSELECT)]
			Boolean oncontrolselect();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOVESTART)]
			Boolean onmovestart();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOVEEND)]
			void onmoveend();

			[DispId(DispId.HTMLELEMENTEVENTS_ONRESIZESTART)]
			Boolean onresizestart();

			[DispId(DispId.HTMLELEMENTEVENTS_ONRESIZEEND)]
			void onresizeend();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEENTER)]
			void onmouseenter();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSELEAVE)]
			void onmouseleave();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEWHEEL)]
			Boolean onmousewheel();

			[DispId(DispId.HTMLELEMENTEVENTS_ONACTIVATE)]
			void onactivate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDEACTIVATE)]
			void ondeactivate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONFOCUSIN)]
			void onfocusin();

			[DispId(DispId.HTMLELEMENTEVENTS_ONFOCUSOUT)]
			void onfocusout();

		}

		[ComImport()]
		[Guid("3050f3cc-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		public interface IHTMLLinkElementEvents
		{
			[DispId(DispId.HTMLELEMENTEVENTS_ONHELP)]
			Boolean onhelp();

			[DispId(DispId.HTMLELEMENTEVENTS_ONCLICK)]
			Boolean onclick();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDBLCLICK)]
			Boolean ondblclick();

			[DispId(DispId.HTMLELEMENTEVENTS_ONKEYPRESS)]
			Boolean onkeypress();

			[DispId(DispId.HTMLELEMENTEVENTS_ONKEYDOWN)]
			void onkeydown();

			[DispId(DispId.HTMLELEMENTEVENTS_ONKEYUP)]
			void onkeyup();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEOUT)]
			void onmouseout();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEOVER)]
			void onmouseover();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEMOVE)]
			void onmousemove();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEDOWN)]
			void onmousedown();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEUP)]
			void onmouseup();

			[DispId(DispId.HTMLELEMENTEVENTS_ONSELECTSTART)]
			Boolean onselectstart();

			[DispId(DispId.HTMLELEMENTEVENTS_ONFILTERCHANGE)]
			void onfilterchange();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDRAGSTART)]
			Boolean ondragstart();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFOREUPDATE)]
			Boolean onbeforeupdate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONAFTERUPDATE)]
			void onafterupdate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONERRORUPDATE)]
			Boolean onerrorupdate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONROWEXIT)]
			Boolean onrowexit();

			[DispId(DispId.HTMLELEMENTEVENTS_ONROWENTER)]
			void onrowenter();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDATASETCHANGED)]
			void ondatasetchanged();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDATAAVAILABLE)]
			void ondataavailable();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDATASETCOMPLETE)]
			void ondatasetcomplete();

			[DispId(DispId.HTMLELEMENTEVENTS_ONLOSECAPTURE)]
			void onlosecapture();

			[DispId(DispId.HTMLELEMENTEVENTS_ONPROPERTYCHANGE)]
			void onpropertychange();

			[DispId(DispId.HTMLELEMENTEVENTS_ONSCROLL)]
			void onscroll();

			[DispId(DispId.HTMLELEMENTEVENTS_ONFOCUS)]
			void onfocus();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBLUR)]
			void onblur();

			[DispId(DispId.HTMLELEMENTEVENTS_ONRESIZE)]
			void onresize();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDRAG)]
			Boolean ondrag();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDRAGEND)]
			void ondragend();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDRAGENTER)]
			Boolean ondragenter();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDRAGOVER)]
			Boolean ondragover();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDRAGLEAVE)]
			void ondragleave();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDROP)]
			Boolean ondrop();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFORECUT)]
			Boolean onbeforecut();

			[DispId(DispId.HTMLELEMENTEVENTS_ONCUT)]
			Boolean oncut();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFORECOPY)]
			Boolean onbeforecopy();

			[DispId(DispId.HTMLELEMENTEVENTS_ONCOPY)]
			Boolean oncopy();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFOREPASTE)]
			Boolean onbeforepaste();

			[DispId(DispId.HTMLELEMENTEVENTS_ONPASTE)]
			Boolean onpaste();

			[DispId(DispId.HTMLELEMENTEVENTS_ONCONTEXTMENU)]
			Boolean oncontextmenu();

			[DispId(DispId.HTMLELEMENTEVENTS_ONROWSDELETE)]
			void onrowsdelete();

			[DispId(DispId.HTMLELEMENTEVENTS_ONROWSINSERTED)]
			void onrowsinserted();

			[DispId(DispId.HTMLELEMENTEVENTS_ONCELLCHANGE)]
			void oncellchange();

			[DispId(DispId.HTMLELEMENTEVENTS_ONREADYSTATECHANGE)]
			void onreadystatechange();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFOREEDITFOCUS)]
			void onbeforeeditfocus();

			[DispId(DispId.HTMLELEMENTEVENTS_ONLAYOUTCOMPLETE)]
			void onlayoutcomplete();

			[DispId(DispId.HTMLELEMENTEVENTS_ONPAGE)]
			void onpage();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFOREDEACTIVATE)]
			Boolean onbeforedeactivate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFOREACTIVATE)]
			Boolean onbeforeactivate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOVE)]
			void onmove();

			[DispId(DispId.HTMLELEMENTEVENTS_ONCONTROLSELECT)]
			Boolean oncontrolselect();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOVESTART)]
			Boolean onmovestart();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOVEEND)]
			void onmoveend();

			[DispId(DispId.HTMLELEMENTEVENTS_ONRESIZESTART)]
			Boolean onresizestart();

			[DispId(DispId.HTMLELEMENTEVENTS_ONRESIZEEND)]
			void onresizeend();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEENTER)]
			void onmouseenter();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSELEAVE)]
			void onmouseleave();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEWHEEL)]
			Boolean onmousewheel();

			[DispId(DispId.HTMLELEMENTEVENTS_ONACTIVATE)]
			void onactivate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDEACTIVATE)]
			void ondeactivate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONFOCUSIN)]
			void onfocusin();

			[DispId(DispId.HTMLELEMENTEVENTS_ONFOCUSOUT)]
			void onfocusout();

			[DispId(DispId.HTMLLINKELEMENTEVENTS_ONLOAD)]
			void onload();

			[DispId(DispId.HTMLLINKELEMENTEVENTS_ONERROR)]
			void onerror();

		}
		[ComImport()]
		[Guid("3050f3ba-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		public interface IHTMLMapEvents
		{
			[DispId(DispId.HTMLELEMENTEVENTS_ONHELP)]
			Boolean onhelp();

			[DispId(DispId.HTMLELEMENTEVENTS_ONCLICK)]
			Boolean onclick();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDBLCLICK)]
			Boolean ondblclick();

			[DispId(DispId.HTMLELEMENTEVENTS_ONKEYPRESS)]
			Boolean onkeypress();

			[DispId(DispId.HTMLELEMENTEVENTS_ONKEYDOWN)]
			void onkeydown();

			[DispId(DispId.HTMLELEMENTEVENTS_ONKEYUP)]
			void onkeyup();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEOUT)]
			void onmouseout();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEOVER)]
			void onmouseover();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEMOVE)]
			void onmousemove();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEDOWN)]
			void onmousedown();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEUP)]
			void onmouseup();

			[DispId(DispId.HTMLELEMENTEVENTS_ONSELECTSTART)]
			Boolean onselectstart();

			[DispId(DispId.HTMLELEMENTEVENTS_ONFILTERCHANGE)]
			void onfilterchange();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDRAGSTART)]
			Boolean ondragstart();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFOREUPDATE)]
			Boolean onbeforeupdate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONAFTERUPDATE)]
			void onafterupdate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONERRORUPDATE)]
			Boolean onerrorupdate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONROWEXIT)]
			Boolean onrowexit();

			[DispId(DispId.HTMLELEMENTEVENTS_ONROWENTER)]
			void onrowenter();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDATASETCHANGED)]
			void ondatasetchanged();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDATAAVAILABLE)]
			void ondataavailable();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDATASETCOMPLETE)]
			void ondatasetcomplete();

			[DispId(DispId.HTMLELEMENTEVENTS_ONLOSECAPTURE)]
			void onlosecapture();

			[DispId(DispId.HTMLELEMENTEVENTS_ONPROPERTYCHANGE)]
			void onpropertychange();

			[DispId(DispId.HTMLELEMENTEVENTS_ONSCROLL)]
			void onscroll();

			[DispId(DispId.HTMLELEMENTEVENTS_ONFOCUS)]
			void onfocus();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBLUR)]
			void onblur();

			[DispId(DispId.HTMLELEMENTEVENTS_ONRESIZE)]
			void onresize();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDRAG)]
			Boolean ondrag();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDRAGEND)]
			void ondragend();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDRAGENTER)]
			Boolean ondragenter();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDRAGOVER)]
			Boolean ondragover();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDRAGLEAVE)]
			void ondragleave();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDROP)]
			Boolean ondrop();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFORECUT)]
			Boolean onbeforecut();

			[DispId(DispId.HTMLELEMENTEVENTS_ONCUT)]
			Boolean oncut();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFORECOPY)]
			Boolean onbeforecopy();

			[DispId(DispId.HTMLELEMENTEVENTS_ONCOPY)]
			Boolean oncopy();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFOREPASTE)]
			Boolean onbeforepaste();

			[DispId(DispId.HTMLELEMENTEVENTS_ONPASTE)]
			Boolean onpaste();

			[DispId(DispId.HTMLELEMENTEVENTS_ONCONTEXTMENU)]
			Boolean oncontextmenu();

			[DispId(DispId.HTMLELEMENTEVENTS_ONROWSDELETE)]
			void onrowsdelete();

			[DispId(DispId.HTMLELEMENTEVENTS_ONROWSINSERTED)]
			void onrowsinserted();

			[DispId(DispId.HTMLELEMENTEVENTS_ONCELLCHANGE)]
			void oncellchange();

			[DispId(DispId.HTMLELEMENTEVENTS_ONREADYSTATECHANGE)]
			void onreadystatechange();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFOREEDITFOCUS)]
			void onbeforeeditfocus();

			[DispId(DispId.HTMLELEMENTEVENTS_ONLAYOUTCOMPLETE)]
			void onlayoutcomplete();

			[DispId(DispId.HTMLELEMENTEVENTS_ONPAGE)]
			void onpage();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFOREDEACTIVATE)]
			Boolean onbeforedeactivate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFOREACTIVATE)]
			Boolean onbeforeactivate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOVE)]
			void onmove();

			[DispId(DispId.HTMLELEMENTEVENTS_ONCONTROLSELECT)]
			Boolean oncontrolselect();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOVESTART)]
			Boolean onmovestart();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOVEEND)]
			void onmoveend();

			[DispId(DispId.HTMLELEMENTEVENTS_ONRESIZESTART)]
			Boolean onresizestart();

			[DispId(DispId.HTMLELEMENTEVENTS_ONRESIZEEND)]
			void onresizeend();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEENTER)]
			void onmouseenter();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSELEAVE)]
			void onmouseleave();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEWHEEL)]
			Boolean onmousewheel();

			[DispId(DispId.HTMLELEMENTEVENTS_ONACTIVATE)]
			void onactivate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDEACTIVATE)]
			void ondeactivate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONFOCUSIN)]
			void onfocusin();

			[DispId(DispId.HTMLELEMENTEVENTS_ONFOCUSOUT)]
			void onfocusout();

		}

		[ComImport()]
		[Guid("3050f2b8-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		public interface IHTMLMarqueeElementEvents
		{
			[DispId(DispId.HTMLELEMENTEVENTS_ONHELP)]
			Boolean onhelp();

			[DispId(DispId.HTMLELEMENTEVENTS_ONCLICK)]
			Boolean onclick();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDBLCLICK)]
			Boolean ondblclick();

			[DispId(DispId.HTMLELEMENTEVENTS_ONKEYPRESS)]
			Boolean onkeypress();

			[DispId(DispId.HTMLELEMENTEVENTS_ONKEYDOWN)]
			void onkeydown();

			[DispId(DispId.HTMLELEMENTEVENTS_ONKEYUP)]
			void onkeyup();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEOUT)]
			void onmouseout();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEOVER)]
			void onmouseover();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEMOVE)]
			void onmousemove();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEDOWN)]
			void onmousedown();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEUP)]
			void onmouseup();

			[DispId(DispId.HTMLELEMENTEVENTS_ONSELECTSTART)]
			Boolean onselectstart();

			[DispId(DispId.HTMLELEMENTEVENTS_ONFILTERCHANGE)]
			void onfilterchange();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDRAGSTART)]
			Boolean ondragstart();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFOREUPDATE)]
			Boolean onbeforeupdate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONAFTERUPDATE)]
			void onafterupdate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONERRORUPDATE)]
			Boolean onerrorupdate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONROWEXIT)]
			Boolean onrowexit();

			[DispId(DispId.HTMLELEMENTEVENTS_ONROWENTER)]
			void onrowenter();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDATASETCHANGED)]
			void ondatasetchanged();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDATAAVAILABLE)]
			void ondataavailable();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDATASETCOMPLETE)]
			void ondatasetcomplete();

			[DispId(DispId.HTMLELEMENTEVENTS_ONLOSECAPTURE)]
			void onlosecapture();

			[DispId(DispId.HTMLELEMENTEVENTS_ONPROPERTYCHANGE)]
			void onpropertychange();

			[DispId(DispId.HTMLELEMENTEVENTS_ONSCROLL)]
			void onscroll();

			[DispId(DispId.HTMLELEMENTEVENTS_ONFOCUS)]
			void onfocus();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBLUR)]
			void onblur();

			[DispId(DispId.HTMLELEMENTEVENTS_ONRESIZE)]
			void onresize();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDRAG)]
			Boolean ondrag();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDRAGEND)]
			void ondragend();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDRAGENTER)]
			Boolean ondragenter();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDRAGOVER)]
			Boolean ondragover();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDRAGLEAVE)]
			void ondragleave();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDROP)]
			Boolean ondrop();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFORECUT)]
			Boolean onbeforecut();

			[DispId(DispId.HTMLELEMENTEVENTS_ONCUT)]
			Boolean oncut();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFORECOPY)]
			Boolean onbeforecopy();

			[DispId(DispId.HTMLELEMENTEVENTS_ONCOPY)]
			Boolean oncopy();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFOREPASTE)]
			Boolean onbeforepaste();

			[DispId(DispId.HTMLELEMENTEVENTS_ONPASTE)]
			Boolean onpaste();

			[DispId(DispId.HTMLELEMENTEVENTS_ONCONTEXTMENU)]
			Boolean oncontextmenu();

			[DispId(DispId.HTMLELEMENTEVENTS_ONROWSDELETE)]
			void onrowsdelete();

			[DispId(DispId.HTMLELEMENTEVENTS_ONROWSINSERTED)]
			void onrowsinserted();

			[DispId(DispId.HTMLELEMENTEVENTS_ONCELLCHANGE)]
			void oncellchange();

			[DispId(DispId.HTMLELEMENTEVENTS_ONREADYSTATECHANGE)]
			void onreadystatechange();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFOREEDITFOCUS)]
			void onbeforeeditfocus();

			[DispId(DispId.HTMLELEMENTEVENTS_ONLAYOUTCOMPLETE)]
			void onlayoutcomplete();

			[DispId(DispId.HTMLELEMENTEVENTS_ONPAGE)]
			void onpage();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFOREDEACTIVATE)]
			Boolean onbeforedeactivate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFOREACTIVATE)]
			Boolean onbeforeactivate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOVE)]
			void onmove();

			[DispId(DispId.HTMLELEMENTEVENTS_ONCONTROLSELECT)]
			Boolean oncontrolselect();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOVESTART)]
			Boolean onmovestart();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOVEEND)]
			void onmoveend();

			[DispId(DispId.HTMLELEMENTEVENTS_ONRESIZESTART)]
			Boolean onresizestart();

			[DispId(DispId.HTMLELEMENTEVENTS_ONRESIZEEND)]
			void onresizeend();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEENTER)]
			void onmouseenter();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSELEAVE)]
			void onmouseleave();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEWHEEL)]
			Boolean onmousewheel();

			[DispId(DispId.HTMLELEMENTEVENTS_ONACTIVATE)]
			void onactivate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDEACTIVATE)]
			void ondeactivate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONFOCUSIN)]
			void onfocusin();

			[DispId(DispId.HTMLELEMENTEVENTS_ONFOCUSOUT)]
			void onfocusout();

			[DispId(DispId.HTMLTEXTCONTAINEREVENTS_ONCHANGE)]
			void onchange();

			[DispId(DispId.HTMLTEXTCONTAINEREVENTS_ONSELECT)]
			void onselect();

			[DispId(DispId.HTMLMARQUEEELEMENTEVENTS_ONBOUNCE)]
			void onbounce();

			[DispId(DispId.HTMLMARQUEEELEMENTEVENTS_ONFINISH)]
			void onfinish();

			[DispId(DispId.HTMLMARQUEEELEMENTEVENTS_ONSTART)]
			void onstart();

		}

		[ComImport()]
		[Guid("3050f3c4-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		public interface IHTMLObjectElementEvents
		{
			[DispId(DispId.HTMLOBJECTELEMENTEVENTS_ONBEFOREUPDATE)]
			Boolean onbeforeupdate();

			[DispId(DispId.HTMLOBJECTELEMENTEVENTS_ONAFTERUPDATE)]
			void onafterupdate();

			[DispId(DispId.HTMLOBJECTELEMENTEVENTS_ONERRORUPDATE)]
			Boolean onerrorupdate();

			[DispId(DispId.HTMLOBJECTELEMENTEVENTS_ONROWEXIT)]
			Boolean onrowexit();

			[DispId(DispId.HTMLOBJECTELEMENTEVENTS_ONROWENTER)]
			void onrowenter();

			[DispId(DispId.HTMLOBJECTELEMENTEVENTS_ONDATASETCHANGED)]
			void ondatasetchanged();

			[DispId(DispId.HTMLOBJECTELEMENTEVENTS_ONDATAAVAILABLE)]
			void ondataavailable();

			[DispId(DispId.HTMLOBJECTELEMENTEVENTS_ONDATASETCOMPLETE)]
			void ondatasetcomplete();

			[DispId(DispId.HTMLOBJECTELEMENTEVENTS_ONERROR)]
			Boolean onerror();

			[DispId(DispId.HTMLOBJECTELEMENTEVENTS_ONROWSDELETE)]
			void onrowsdelete();

			[DispId(DispId.HTMLOBJECTELEMENTEVENTS_ONROWSINSERTED)]
			void onrowsinserted();

			[DispId(DispId.HTMLOBJECTELEMENTEVENTS_ONCELLCHANGE)]
			void oncellchange();

			[DispId(DispId.HTMLOBJECTELEMENTEVENTS_ONREADYSTATECHANGE)]
			void onreadystatechange();

		}

		[ComImport()]
		[Guid("3050f2bd-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		public interface IHTMLOptionButtonElementEvents
		{
			[DispId(DispId.HTMLELEMENTEVENTS_ONHELP)]
			Boolean onhelp();

			[DispId(DispId.HTMLELEMENTEVENTS_ONCLICK)]
			Boolean onclick();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDBLCLICK)]
			Boolean ondblclick();

			[DispId(DispId.HTMLELEMENTEVENTS_ONKEYPRESS)]
			Boolean onkeypress();

			[DispId(DispId.HTMLELEMENTEVENTS_ONKEYDOWN)]
			void onkeydown();

			[DispId(DispId.HTMLELEMENTEVENTS_ONKEYUP)]
			void onkeyup();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEOUT)]
			void onmouseout();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEOVER)]
			void onmouseover();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEMOVE)]
			void onmousemove();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEDOWN)]
			void onmousedown();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEUP)]
			void onmouseup();

			[DispId(DispId.HTMLELEMENTEVENTS_ONSELECTSTART)]
			Boolean onselectstart();

			[DispId(DispId.HTMLELEMENTEVENTS_ONFILTERCHANGE)]
			void onfilterchange();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDRAGSTART)]
			Boolean ondragstart();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFOREUPDATE)]
			Boolean onbeforeupdate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONAFTERUPDATE)]
			void onafterupdate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONERRORUPDATE)]
			Boolean onerrorupdate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONROWEXIT)]
			Boolean onrowexit();

			[DispId(DispId.HTMLELEMENTEVENTS_ONROWENTER)]
			void onrowenter();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDATASETCHANGED)]
			void ondatasetchanged();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDATAAVAILABLE)]
			void ondataavailable();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDATASETCOMPLETE)]
			void ondatasetcomplete();

			[DispId(DispId.HTMLELEMENTEVENTS_ONLOSECAPTURE)]
			void onlosecapture();

			[DispId(DispId.HTMLELEMENTEVENTS_ONPROPERTYCHANGE)]
			void onpropertychange();

			[DispId(DispId.HTMLELEMENTEVENTS_ONSCROLL)]
			void onscroll();

			[DispId(DispId.HTMLELEMENTEVENTS_ONFOCUS)]
			void onfocus();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBLUR)]
			void onblur();

			[DispId(DispId.HTMLELEMENTEVENTS_ONRESIZE)]
			void onresize();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDRAG)]
			Boolean ondrag();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDRAGEND)]
			void ondragend();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDRAGENTER)]
			Boolean ondragenter();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDRAGOVER)]
			Boolean ondragover();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDRAGLEAVE)]
			void ondragleave();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDROP)]
			Boolean ondrop();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFORECUT)]
			Boolean onbeforecut();

			[DispId(DispId.HTMLELEMENTEVENTS_ONCUT)]
			Boolean oncut();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFORECOPY)]
			Boolean onbeforecopy();

			[DispId(DispId.HTMLELEMENTEVENTS_ONCOPY)]
			Boolean oncopy();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFOREPASTE)]
			Boolean onbeforepaste();

			[DispId(DispId.HTMLELEMENTEVENTS_ONPASTE)]
			Boolean onpaste();

			[DispId(DispId.HTMLELEMENTEVENTS_ONCONTEXTMENU)]
			Boolean oncontextmenu();

			[DispId(DispId.HTMLELEMENTEVENTS_ONROWSDELETE)]
			void onrowsdelete();

			[DispId(DispId.HTMLELEMENTEVENTS_ONROWSINSERTED)]
			void onrowsinserted();

			[DispId(DispId.HTMLELEMENTEVENTS_ONCELLCHANGE)]
			void oncellchange();

			[DispId(DispId.HTMLELEMENTEVENTS_ONREADYSTATECHANGE)]
			void onreadystatechange();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFOREEDITFOCUS)]
			void onbeforeeditfocus();

			[DispId(DispId.HTMLELEMENTEVENTS_ONLAYOUTCOMPLETE)]
			void onlayoutcomplete();

			[DispId(DispId.HTMLELEMENTEVENTS_ONPAGE)]
			void onpage();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFOREDEACTIVATE)]
			Boolean onbeforedeactivate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFOREACTIVATE)]
			Boolean onbeforeactivate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOVE)]
			void onmove();

			[DispId(DispId.HTMLELEMENTEVENTS_ONCONTROLSELECT)]
			Boolean oncontrolselect();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOVESTART)]
			Boolean onmovestart();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOVEEND)]
			void onmoveend();

			[DispId(DispId.HTMLELEMENTEVENTS_ONRESIZESTART)]
			Boolean onresizestart();

			[DispId(DispId.HTMLELEMENTEVENTS_ONRESIZEEND)]
			void onresizeend();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEENTER)]
			void onmouseenter();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSELEAVE)]
			void onmouseleave();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEWHEEL)]
			Boolean onmousewheel();

			[DispId(DispId.HTMLELEMENTEVENTS_ONACTIVATE)]
			void onactivate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDEACTIVATE)]
			void ondeactivate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONFOCUSIN)]
			void onfocusin();

			[DispId(DispId.HTMLELEMENTEVENTS_ONFOCUSOUT)]
			void onfocusout();

			[DispId(DispId.HTMLINPUTTEXTELEMENTEVENTS_ONCHANGE)]
			Boolean onchange();

			[DispId(DispId.HTMLINPUTTEXTELEMENTEVENTS_ONSELECT)]
			void onselect();

			[DispId(DispId.HTMLINPUTTEXTELEMENTEVENTS_ONLOAD)]
			void onload();

			[DispId(DispId.HTMLINPUTTEXTELEMENTEVENTS_ONERROR)]
			void onerror();

			[DispId(DispId.HTMLINPUTTEXTELEMENTEVENTS_ONABORT)]
			void onabort();

		}

		[ComImport()]
		[Guid("3050f3e2-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		public interface IHTMLScriptEvents
		{
			[DispId(DispId.HTMLELEMENTEVENTS_ONHELP)]
			Boolean onhelp();

			[DispId(DispId.HTMLELEMENTEVENTS_ONCLICK)]
			Boolean onclick();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDBLCLICK)]
			Boolean ondblclick();

			[DispId(DispId.HTMLELEMENTEVENTS_ONKEYPRESS)]
			Boolean onkeypress();

			[DispId(DispId.HTMLELEMENTEVENTS_ONKEYDOWN)]
			void onkeydown();

			[DispId(DispId.HTMLELEMENTEVENTS_ONKEYUP)]
			void onkeyup();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEOUT)]
			void onmouseout();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEOVER)]
			void onmouseover();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEMOVE)]
			void onmousemove();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEDOWN)]
			void onmousedown();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEUP)]
			void onmouseup();

			[DispId(DispId.HTMLELEMENTEVENTS_ONSELECTSTART)]
			Boolean onselectstart();

			[DispId(DispId.HTMLELEMENTEVENTS_ONFILTERCHANGE)]
			void onfilterchange();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDRAGSTART)]
			Boolean ondragstart();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFOREUPDATE)]
			Boolean onbeforeupdate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONAFTERUPDATE)]
			void onafterupdate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONERRORUPDATE)]
			Boolean onerrorupdate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONROWEXIT)]
			Boolean onrowexit();

			[DispId(DispId.HTMLELEMENTEVENTS_ONROWENTER)]
			void onrowenter();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDATASETCHANGED)]
			void ondatasetchanged();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDATAAVAILABLE)]
			void ondataavailable();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDATASETCOMPLETE)]
			void ondatasetcomplete();

			[DispId(DispId.HTMLELEMENTEVENTS_ONLOSECAPTURE)]
			void onlosecapture();

			[DispId(DispId.HTMLELEMENTEVENTS_ONPROPERTYCHANGE)]
			void onpropertychange();

			[DispId(DispId.HTMLELEMENTEVENTS_ONSCROLL)]
			void onscroll();

			[DispId(DispId.HTMLELEMENTEVENTS_ONFOCUS)]
			void onfocus();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBLUR)]
			void onblur();

			[DispId(DispId.HTMLELEMENTEVENTS_ONRESIZE)]
			void onresize();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDRAG)]
			Boolean ondrag();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDRAGEND)]
			void ondragend();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDRAGENTER)]
			Boolean ondragenter();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDRAGOVER)]
			Boolean ondragover();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDRAGLEAVE)]
			void ondragleave();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDROP)]
			Boolean ondrop();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFORECUT)]
			Boolean onbeforecut();

			[DispId(DispId.HTMLELEMENTEVENTS_ONCUT)]
			Boolean oncut();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFORECOPY)]
			Boolean onbeforecopy();

			[DispId(DispId.HTMLELEMENTEVENTS_ONCOPY)]
			Boolean oncopy();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFOREPASTE)]
			Boolean onbeforepaste();

			[DispId(DispId.HTMLELEMENTEVENTS_ONPASTE)]
			Boolean onpaste();

			[DispId(DispId.HTMLELEMENTEVENTS_ONCONTEXTMENU)]
			Boolean oncontextmenu();

			[DispId(DispId.HTMLELEMENTEVENTS_ONROWSDELETE)]
			void onrowsdelete();

			[DispId(DispId.HTMLELEMENTEVENTS_ONROWSINSERTED)]
			void onrowsinserted();

			[DispId(DispId.HTMLELEMENTEVENTS_ONCELLCHANGE)]
			void oncellchange();

			[DispId(DispId.HTMLELEMENTEVENTS_ONREADYSTATECHANGE)]
			void onreadystatechange();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFOREEDITFOCUS)]
			void onbeforeeditfocus();

			[DispId(DispId.HTMLELEMENTEVENTS_ONLAYOUTCOMPLETE)]
			void onlayoutcomplete();

			[DispId(DispId.HTMLELEMENTEVENTS_ONPAGE)]
			void onpage();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFOREDEACTIVATE)]
			Boolean onbeforedeactivate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFOREACTIVATE)]
			Boolean onbeforeactivate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOVE)]
			void onmove();

			[DispId(DispId.HTMLELEMENTEVENTS_ONCONTROLSELECT)]
			Boolean oncontrolselect();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOVESTART)]
			Boolean onmovestart();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOVEEND)]
			void onmoveend();

			[DispId(DispId.HTMLELEMENTEVENTS_ONRESIZESTART)]
			Boolean onresizestart();

			[DispId(DispId.HTMLELEMENTEVENTS_ONRESIZEEND)]
			void onresizeend();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEENTER)]
			void onmouseenter();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSELEAVE)]
			void onmouseleave();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEWHEEL)]
			Boolean onmousewheel();

			[DispId(DispId.HTMLELEMENTEVENTS_ONACTIVATE)]
			void onactivate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDEACTIVATE)]
			void ondeactivate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONFOCUSIN)]
			void onfocusin();

			[DispId(DispId.HTMLELEMENTEVENTS_ONFOCUSOUT)]
			void onfocusout();

			[DispId(DispId.HTMLSCRIPTEVENTS_ONERROR)]
			void onerror();

		}

		[ComImport()]
		[Guid("3050f302-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		public interface IHTMLSelectElementEvents
		{
			[DispId(DispId.HTMLELEMENTEVENTS_ONHELP)]
			Boolean onhelp();

			[DispId(DispId.HTMLELEMENTEVENTS_ONCLICK)]
			Boolean onclick();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDBLCLICK)]
			Boolean ondblclick();

			[DispId(DispId.HTMLELEMENTEVENTS_ONKEYPRESS)]
			Boolean onkeypress();

			[DispId(DispId.HTMLELEMENTEVENTS_ONKEYDOWN)]
			void onkeydown();

			[DispId(DispId.HTMLELEMENTEVENTS_ONKEYUP)]
			void onkeyup();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEOUT)]
			void onmouseout();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEOVER)]
			void onmouseover();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEMOVE)]
			void onmousemove();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEDOWN)]
			void onmousedown();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEUP)]
			void onmouseup();

			[DispId(DispId.HTMLELEMENTEVENTS_ONSELECTSTART)]
			Boolean onselectstart();

			[DispId(DispId.HTMLELEMENTEVENTS_ONFILTERCHANGE)]
			void onfilterchange();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDRAGSTART)]
			Boolean ondragstart();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFOREUPDATE)]
			Boolean onbeforeupdate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONAFTERUPDATE)]
			void onafterupdate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONERRORUPDATE)]
			Boolean onerrorupdate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONROWEXIT)]
			Boolean onrowexit();

			[DispId(DispId.HTMLELEMENTEVENTS_ONROWENTER)]
			void onrowenter();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDATASETCHANGED)]
			void ondatasetchanged();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDATAAVAILABLE)]
			void ondataavailable();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDATASETCOMPLETE)]
			void ondatasetcomplete();

			[DispId(DispId.HTMLELEMENTEVENTS_ONLOSECAPTURE)]
			void onlosecapture();

			[DispId(DispId.HTMLELEMENTEVENTS_ONPROPERTYCHANGE)]
			void onpropertychange();

			[DispId(DispId.HTMLELEMENTEVENTS_ONSCROLL)]
			void onscroll();

			[DispId(DispId.HTMLELEMENTEVENTS_ONFOCUS)]
			void onfocus();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBLUR)]
			void onblur();

			[DispId(DispId.HTMLELEMENTEVENTS_ONRESIZE)]
			void onresize();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDRAG)]
			Boolean ondrag();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDRAGEND)]
			void ondragend();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDRAGENTER)]
			Boolean ondragenter();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDRAGOVER)]
			Boolean ondragover();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDRAGLEAVE)]
			void ondragleave();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDROP)]
			Boolean ondrop();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFORECUT)]
			Boolean onbeforecut();

			[DispId(DispId.HTMLELEMENTEVENTS_ONCUT)]
			Boolean oncut();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFORECOPY)]
			Boolean onbeforecopy();

			[DispId(DispId.HTMLELEMENTEVENTS_ONCOPY)]
			Boolean oncopy();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFOREPASTE)]
			Boolean onbeforepaste();

			[DispId(DispId.HTMLELEMENTEVENTS_ONPASTE)]
			Boolean onpaste();

			[DispId(DispId.HTMLELEMENTEVENTS_ONCONTEXTMENU)]
			Boolean oncontextmenu();

			[DispId(DispId.HTMLELEMENTEVENTS_ONROWSDELETE)]
			void onrowsdelete();

			[DispId(DispId.HTMLELEMENTEVENTS_ONROWSINSERTED)]
			void onrowsinserted();

			[DispId(DispId.HTMLELEMENTEVENTS_ONCELLCHANGE)]
			void oncellchange();

			[DispId(DispId.HTMLELEMENTEVENTS_ONREADYSTATECHANGE)]
			void onreadystatechange();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFOREEDITFOCUS)]
			void onbeforeeditfocus();

			[DispId(DispId.HTMLELEMENTEVENTS_ONLAYOUTCOMPLETE)]
			void onlayoutcomplete();

			[DispId(DispId.HTMLELEMENTEVENTS_ONPAGE)]
			void onpage();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFOREDEACTIVATE)]
			Boolean onbeforedeactivate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFOREACTIVATE)]
			Boolean onbeforeactivate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOVE)]
			void onmove();

			[DispId(DispId.HTMLELEMENTEVENTS_ONCONTROLSELECT)]
			Boolean oncontrolselect();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOVESTART)]
			Boolean onmovestart();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOVEEND)]
			void onmoveend();

			[DispId(DispId.HTMLELEMENTEVENTS_ONRESIZESTART)]
			Boolean onresizestart();

			[DispId(DispId.HTMLELEMENTEVENTS_ONRESIZEEND)]
			void onresizeend();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEENTER)]
			void onmouseenter();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSELEAVE)]
			void onmouseleave();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEWHEEL)]
			Boolean onmousewheel();

			[DispId(DispId.HTMLELEMENTEVENTS_ONACTIVATE)]
			void onactivate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDEACTIVATE)]
			void ondeactivate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONFOCUSIN)]
			void onfocusin();

			[DispId(DispId.HTMLELEMENTEVENTS_ONFOCUSOUT)]
			void onfocusout();

			[DispId(DispId.HTMLSELECTELEMENTEVENTS_ONCHANGE)]
			void onchange();

		}

		[ComImport()]
		[Guid("3050f3cb-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		public interface IHTMLStyleElementEvents
		{
			[DispId(DispId.HTMLELEMENTEVENTS_ONHELP)]
			Boolean onhelp();

			[DispId(DispId.HTMLELEMENTEVENTS_ONCLICK)]
			Boolean onclick();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDBLCLICK)]
			Boolean ondblclick();

			[DispId(DispId.HTMLELEMENTEVENTS_ONKEYPRESS)]
			Boolean onkeypress();

			[DispId(DispId.HTMLELEMENTEVENTS_ONKEYDOWN)]
			void onkeydown();

			[DispId(DispId.HTMLELEMENTEVENTS_ONKEYUP)]
			void onkeyup();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEOUT)]
			void onmouseout();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEOVER)]
			void onmouseover();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEMOVE)]
			void onmousemove();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEDOWN)]
			void onmousedown();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEUP)]
			void onmouseup();

			[DispId(DispId.HTMLELEMENTEVENTS_ONSELECTSTART)]
			Boolean onselectstart();

			[DispId(DispId.HTMLELEMENTEVENTS_ONFILTERCHANGE)]
			void onfilterchange();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDRAGSTART)]
			Boolean ondragstart();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFOREUPDATE)]
			Boolean onbeforeupdate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONAFTERUPDATE)]
			void onafterupdate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONERRORUPDATE)]
			Boolean onerrorupdate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONROWEXIT)]
			Boolean onrowexit();

			[DispId(DispId.HTMLELEMENTEVENTS_ONROWENTER)]
			void onrowenter();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDATASETCHANGED)]
			void ondatasetchanged();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDATAAVAILABLE)]
			void ondataavailable();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDATASETCOMPLETE)]
			void ondatasetcomplete();

			[DispId(DispId.HTMLELEMENTEVENTS_ONLOSECAPTURE)]
			void onlosecapture();

			[DispId(DispId.HTMLELEMENTEVENTS_ONPROPERTYCHANGE)]
			void onpropertychange();

			[DispId(DispId.HTMLELEMENTEVENTS_ONSCROLL)]
			void onscroll();

			[DispId(DispId.HTMLELEMENTEVENTS_ONFOCUS)]
			void onfocus();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBLUR)]
			void onblur();

			[DispId(DispId.HTMLELEMENTEVENTS_ONRESIZE)]
			void onresize();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDRAG)]
			Boolean ondrag();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDRAGEND)]
			void ondragend();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDRAGENTER)]
			Boolean ondragenter();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDRAGOVER)]
			Boolean ondragover();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDRAGLEAVE)]
			void ondragleave();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDROP)]
			Boolean ondrop();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFORECUT)]
			Boolean onbeforecut();

			[DispId(DispId.HTMLELEMENTEVENTS_ONCUT)]
			Boolean oncut();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFORECOPY)]
			Boolean onbeforecopy();

			[DispId(DispId.HTMLELEMENTEVENTS_ONCOPY)]
			Boolean oncopy();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFOREPASTE)]
			Boolean onbeforepaste();

			[DispId(DispId.HTMLELEMENTEVENTS_ONPASTE)]
			Boolean onpaste();

			[DispId(DispId.HTMLELEMENTEVENTS_ONCONTEXTMENU)]
			Boolean oncontextmenu();

			[DispId(DispId.HTMLELEMENTEVENTS_ONROWSDELETE)]
			void onrowsdelete();

			[DispId(DispId.HTMLELEMENTEVENTS_ONROWSINSERTED)]
			void onrowsinserted();

			[DispId(DispId.HTMLELEMENTEVENTS_ONCELLCHANGE)]
			void oncellchange();

			[DispId(DispId.HTMLELEMENTEVENTS_ONREADYSTATECHANGE)]
			void onreadystatechange();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFOREEDITFOCUS)]
			void onbeforeeditfocus();

			[DispId(DispId.HTMLELEMENTEVENTS_ONLAYOUTCOMPLETE)]
			void onlayoutcomplete();

			[DispId(DispId.HTMLELEMENTEVENTS_ONPAGE)]
			void onpage();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFOREDEACTIVATE)]
			Boolean onbeforedeactivate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFOREACTIVATE)]
			Boolean onbeforeactivate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOVE)]
			void onmove();

			[DispId(DispId.HTMLELEMENTEVENTS_ONCONTROLSELECT)]
			Boolean oncontrolselect();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOVESTART)]
			Boolean onmovestart();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOVEEND)]
			void onmoveend();

			[DispId(DispId.HTMLELEMENTEVENTS_ONRESIZESTART)]
			Boolean onresizestart();

			[DispId(DispId.HTMLELEMENTEVENTS_ONRESIZEEND)]
			void onresizeend();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEENTER)]
			void onmouseenter();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSELEAVE)]
			void onmouseleave();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEWHEEL)]
			Boolean onmousewheel();

			[DispId(DispId.HTMLELEMENTEVENTS_ONACTIVATE)]
			void onactivate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDEACTIVATE)]
			void ondeactivate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONFOCUSIN)]
			void onfocusin();

			[DispId(DispId.HTMLELEMENTEVENTS_ONFOCUSOUT)]
			void onfocusout();

			[DispId(DispId.HTMLSTYLEELEMENTEVENTS_ONLOAD)]
			void onload();

			[DispId(DispId.HTMLSTYLEELEMENTEVENTS_ONERROR)]
			void onerror();

		}

		[ComImport()]
		[Guid("3050f407-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		public interface IHTMLTableEvents
		{
			[DispId(DispId.HTMLELEMENTEVENTS_ONHELP)]
			Boolean onhelp();

			[DispId(DispId.HTMLELEMENTEVENTS_ONCLICK)]
			Boolean onclick();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDBLCLICK)]
			Boolean ondblclick();

			[DispId(DispId.HTMLELEMENTEVENTS_ONKEYPRESS)]
			Boolean onkeypress();

			[DispId(DispId.HTMLELEMENTEVENTS_ONKEYDOWN)]
			void onkeydown();

			[DispId(DispId.HTMLELEMENTEVENTS_ONKEYUP)]
			void onkeyup();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEOUT)]
			void onmouseout();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEOVER)]
			void onmouseover();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEMOVE)]
			void onmousemove();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEDOWN)]
			void onmousedown();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEUP)]
			void onmouseup();

			[DispId(DispId.HTMLELEMENTEVENTS_ONSELECTSTART)]
			Boolean onselectstart();

			[DispId(DispId.HTMLELEMENTEVENTS_ONFILTERCHANGE)]
			void onfilterchange();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDRAGSTART)]
			Boolean ondragstart();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFOREUPDATE)]
			Boolean onbeforeupdate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONAFTERUPDATE)]
			void onafterupdate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONERRORUPDATE)]
			Boolean onerrorupdate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONROWEXIT)]
			Boolean onrowexit();

			[DispId(DispId.HTMLELEMENTEVENTS_ONROWENTER)]
			void onrowenter();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDATASETCHANGED)]
			void ondatasetchanged();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDATAAVAILABLE)]
			void ondataavailable();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDATASETCOMPLETE)]
			void ondatasetcomplete();

			[DispId(DispId.HTMLELEMENTEVENTS_ONLOSECAPTURE)]
			void onlosecapture();

			[DispId(DispId.HTMLELEMENTEVENTS_ONPROPERTYCHANGE)]
			void onpropertychange();

			[DispId(DispId.HTMLELEMENTEVENTS_ONSCROLL)]
			void onscroll();

			[DispId(DispId.HTMLELEMENTEVENTS_ONFOCUS)]
			void onfocus();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBLUR)]
			void onblur();

			[DispId(DispId.HTMLELEMENTEVENTS_ONRESIZE)]
			void onresize();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDRAG)]
			Boolean ondrag();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDRAGEND)]
			void ondragend();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDRAGENTER)]
			Boolean ondragenter();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDRAGOVER)]
			Boolean ondragover();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDRAGLEAVE)]
			void ondragleave();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDROP)]
			Boolean ondrop();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFORECUT)]
			Boolean onbeforecut();

			[DispId(DispId.HTMLELEMENTEVENTS_ONCUT)]
			Boolean oncut();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFORECOPY)]
			Boolean onbeforecopy();

			[DispId(DispId.HTMLELEMENTEVENTS_ONCOPY)]
			Boolean oncopy();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFOREPASTE)]
			Boolean onbeforepaste();

			[DispId(DispId.HTMLELEMENTEVENTS_ONPASTE)]
			Boolean onpaste();

			[DispId(DispId.HTMLELEMENTEVENTS_ONCONTEXTMENU)]
			Boolean oncontextmenu();

			[DispId(DispId.HTMLELEMENTEVENTS_ONROWSDELETE)]
			void onrowsdelete();

			[DispId(DispId.HTMLELEMENTEVENTS_ONROWSINSERTED)]
			void onrowsinserted();

			[DispId(DispId.HTMLELEMENTEVENTS_ONCELLCHANGE)]
			void oncellchange();

			[DispId(DispId.HTMLELEMENTEVENTS_ONREADYSTATECHANGE)]
			void onreadystatechange();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFOREEDITFOCUS)]
			void onbeforeeditfocus();

			[DispId(DispId.HTMLELEMENTEVENTS_ONLAYOUTCOMPLETE)]
			void onlayoutcomplete();

			[DispId(DispId.HTMLELEMENTEVENTS_ONPAGE)]
			void onpage();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFOREDEACTIVATE)]
			Boolean onbeforedeactivate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFOREACTIVATE)]
			Boolean onbeforeactivate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOVE)]
			void onmove();

			[DispId(DispId.HTMLELEMENTEVENTS_ONCONTROLSELECT)]
			Boolean oncontrolselect();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOVESTART)]
			Boolean onmovestart();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOVEEND)]
			void onmoveend();

			[DispId(DispId.HTMLELEMENTEVENTS_ONRESIZESTART)]
			Boolean onresizestart();

			[DispId(DispId.HTMLELEMENTEVENTS_ONRESIZEEND)]
			void onresizeend();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEENTER)]
			void onmouseenter();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSELEAVE)]
			void onmouseleave();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEWHEEL)]
			Boolean onmousewheel();

			[DispId(DispId.HTMLELEMENTEVENTS_ONACTIVATE)]
			void onactivate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDEACTIVATE)]
			void ondeactivate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONFOCUSIN)]
			void onfocusin();

			[DispId(DispId.HTMLELEMENTEVENTS_ONFOCUSOUT)]
			void onfocusout();

		}

		[ComImport()]
		[Guid("3050f624-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		public interface IHTMLTextContainerEvents2
		{
			[DispId(DispId.HTMLELEMENTEVENTS2_ONHELP)]
			Boolean onhelp(IHTMLEventObj pEvtObj);

			[DispId(DispId.HTMLELEMENTEVENTS2_ONCLICK)]
			Boolean onclick(IHTMLEventObj pEvtObj);

			[DispId(DispId.HTMLELEMENTEVENTS2_ONDBLCLICK)]
			Boolean ondblclick(IHTMLEventObj pEvtObj);

			[DispId(DispId.HTMLELEMENTEVENTS2_ONKEYPRESS)]
			Boolean onkeypress(IHTMLEventObj pEvtObj);

			[DispId(DispId.HTMLELEMENTEVENTS2_ONKEYDOWN)]
			void onkeydown(IHTMLEventObj pEvtObj);

			[DispId(DispId.HTMLELEMENTEVENTS2_ONKEYUP)]
			void onkeyup(IHTMLEventObj pEvtObj);

			[DispId(DispId.HTMLELEMENTEVENTS2_ONMOUSEOUT)]
			void onmouseout(IHTMLEventObj pEvtObj);

			[DispId(DispId.HTMLELEMENTEVENTS2_ONMOUSEOVER)]
			void onmouseover(IHTMLEventObj pEvtObj);

			[DispId(DispId.HTMLELEMENTEVENTS2_ONMOUSEMOVE)]
			void onmousemove(IHTMLEventObj pEvtObj);

			[DispId(DispId.HTMLELEMENTEVENTS2_ONMOUSEDOWN)]
			void onmousedown(IHTMLEventObj pEvtObj);

			[DispId(DispId.HTMLELEMENTEVENTS2_ONMOUSEUP)]
			void onmouseup(IHTMLEventObj pEvtObj);

			[DispId(DispId.HTMLELEMENTEVENTS2_ONSELECTSTART)]
			Boolean onselectstart(IHTMLEventObj pEvtObj);

			[DispId(DispId.HTMLELEMENTEVENTS2_ONFILTERCHANGE)]
			void onfilterchange(IHTMLEventObj pEvtObj);

			[DispId(DispId.HTMLELEMENTEVENTS2_ONDRAGSTART)]
			Boolean ondragstart(IHTMLEventObj pEvtObj);

			[DispId(DispId.HTMLELEMENTEVENTS2_ONBEFOREUPDATE)]
			Boolean onbeforeupdate(IHTMLEventObj pEvtObj);

			[DispId(DispId.HTMLELEMENTEVENTS2_ONAFTERUPDATE)]
			void onafterupdate(IHTMLEventObj pEvtObj);

			[DispId(DispId.HTMLELEMENTEVENTS2_ONERRORUPDATE)]
			Boolean onerrorupdate(IHTMLEventObj pEvtObj);

			[DispId(DispId.HTMLELEMENTEVENTS2_ONROWEXIT)]
			Boolean onrowexit(IHTMLEventObj pEvtObj);

			[DispId(DispId.HTMLELEMENTEVENTS2_ONROWENTER)]
			void onrowenter(IHTMLEventObj pEvtObj);

			[DispId(DispId.HTMLELEMENTEVENTS2_ONDATASETCHANGED)]
			void ondatasetchanged(IHTMLEventObj pEvtObj);

			[DispId(DispId.HTMLELEMENTEVENTS2_ONDATAAVAILABLE)]
			void ondataavailable(IHTMLEventObj pEvtObj);

			[DispId(DispId.HTMLELEMENTEVENTS2_ONDATASETCOMPLETE)]
			void ondatasetcomplete(IHTMLEventObj pEvtObj);

			[DispId(DispId.HTMLELEMENTEVENTS2_ONLOSECAPTURE)]
			void onlosecapture(IHTMLEventObj pEvtObj);

			[DispId(DispId.HTMLELEMENTEVENTS2_ONPROPERTYCHANGE)]
			void onpropertychange(IHTMLEventObj pEvtObj);

			[DispId(DispId.HTMLELEMENTEVENTS2_ONSCROLL)]
			void onscroll(IHTMLEventObj pEvtObj);

			[DispId(DispId.HTMLELEMENTEVENTS2_ONFOCUS)]
			void onfocus(IHTMLEventObj pEvtObj);

			[DispId(DispId.HTMLELEMENTEVENTS2_ONBLUR)]
			void onblur(IHTMLEventObj pEvtObj);

			[DispId(DispId.HTMLELEMENTEVENTS2_ONRESIZE)]
			void onresize(IHTMLEventObj pEvtObj);

			[DispId(DispId.HTMLELEMENTEVENTS2_ONDRAG)]
			Boolean ondrag(IHTMLEventObj pEvtObj);

			[DispId(DispId.HTMLELEMENTEVENTS2_ONDRAGEND)]
			void ondragend(IHTMLEventObj pEvtObj);

			[DispId(DispId.HTMLELEMENTEVENTS2_ONDRAGENTER)]
			Boolean ondragenter(IHTMLEventObj pEvtObj);

			[DispId(DispId.HTMLELEMENTEVENTS2_ONDRAGOVER)]
			Boolean ondragover(IHTMLEventObj pEvtObj);

			[DispId(DispId.HTMLELEMENTEVENTS2_ONDRAGLEAVE)]
			void ondragleave(IHTMLEventObj pEvtObj);

			[DispId(DispId.HTMLELEMENTEVENTS2_ONDROP)]
			Boolean ondrop(IHTMLEventObj pEvtObj);

			[DispId(DispId.HTMLELEMENTEVENTS2_ONBEFORECUT)]
			Boolean onbeforecut(IHTMLEventObj pEvtObj);

			[DispId(DispId.HTMLELEMENTEVENTS2_ONCUT)]
			Boolean oncut(IHTMLEventObj pEvtObj);

			[DispId(DispId.HTMLELEMENTEVENTS2_ONBEFORECOPY)]
			Boolean onbeforecopy(IHTMLEventObj pEvtObj);

			[DispId(DispId.HTMLELEMENTEVENTS2_ONCOPY)]
			Boolean oncopy(IHTMLEventObj pEvtObj);

			[DispId(DispId.HTMLELEMENTEVENTS2_ONBEFOREPASTE)]
			Boolean onbeforepaste(IHTMLEventObj pEvtObj);

			[DispId(DispId.HTMLELEMENTEVENTS2_ONPASTE)]
			Boolean onpaste(IHTMLEventObj pEvtObj);

			[DispId(DispId.HTMLELEMENTEVENTS2_ONCONTEXTMENU)]
			Boolean oncontextmenu(IHTMLEventObj pEvtObj);

			[DispId(DispId.HTMLELEMENTEVENTS2_ONROWSDELETE)]
			void onrowsdelete(IHTMLEventObj pEvtObj);

			[DispId(DispId.HTMLELEMENTEVENTS2_ONROWSINSERTED)]
			void onrowsinserted(IHTMLEventObj pEvtObj);

			[DispId(DispId.HTMLELEMENTEVENTS2_ONCELLCHANGE)]
			void oncellchange(IHTMLEventObj pEvtObj);

			[DispId(DispId.HTMLELEMENTEVENTS2_ONREADYSTATECHANGE)]
			void onreadystatechange(IHTMLEventObj pEvtObj);

			[DispId(DispId.HTMLELEMENTEVENTS2_ONLAYOUTCOMPLETE)]
			void onlayoutcomplete(IHTMLEventObj pEvtObj);

			[DispId(DispId.HTMLELEMENTEVENTS2_ONPAGE)]
			void onpage(IHTMLEventObj pEvtObj);

			[DispId(DispId.HTMLELEMENTEVENTS2_ONMOUSEENTER)]
			void onmouseenter(IHTMLEventObj pEvtObj);

			[DispId(DispId.HTMLELEMENTEVENTS2_ONMOUSELEAVE)]
			void onmouseleave(IHTMLEventObj pEvtObj);

			[DispId(DispId.HTMLELEMENTEVENTS2_ONACTIVATE)]
			void onactivate(IHTMLEventObj pEvtObj);

			[DispId(DispId.HTMLELEMENTEVENTS2_ONDEACTIVATE)]
			void ondeactivate(IHTMLEventObj pEvtObj);

			[DispId(DispId.HTMLELEMENTEVENTS2_ONBEFOREDEACTIVATE)]
			Boolean onbeforedeactivate(IHTMLEventObj pEvtObj);

			[DispId(DispId.HTMLELEMENTEVENTS2_ONBEFOREACTIVATE)]
			Boolean onbeforeactivate(IHTMLEventObj pEvtObj);

			[DispId(DispId.HTMLELEMENTEVENTS2_ONFOCUSIN)]
			void onfocusin(IHTMLEventObj pEvtObj);

			[DispId(DispId.HTMLELEMENTEVENTS2_ONFOCUSOUT)]
			void onfocusout(IHTMLEventObj pEvtObj);

			[DispId(DispId.HTMLELEMENTEVENTS2_ONMOVE)]
			void onmove(IHTMLEventObj pEvtObj);

			[DispId(DispId.HTMLELEMENTEVENTS2_ONCONTROLSELECT)]
			Boolean oncontrolselect(IHTMLEventObj pEvtObj);

			[DispId(DispId.HTMLELEMENTEVENTS2_ONMOVESTART)]
			Boolean onmovestart(IHTMLEventObj pEvtObj);

			[DispId(DispId.HTMLELEMENTEVENTS2_ONMOVEEND)]
			void onmoveend(IHTMLEventObj pEvtObj);

			[DispId(DispId.HTMLELEMENTEVENTS2_ONRESIZESTART)]
			Boolean onresizestart(IHTMLEventObj pEvtObj);

			[DispId(DispId.HTMLELEMENTEVENTS2_ONRESIZEEND)]
			void onresizeend(IHTMLEventObj pEvtObj);

			[DispId(DispId.HTMLELEMENTEVENTS2_ONMOUSEWHEEL)]
			Boolean onmousewheel(IHTMLEventObj pEvtObj);

			[DispId(DispId.HTMLTEXTCONTAINEREVENTS2_ONCHANGE)]
			void onchange(IHTMLEventObj pEvtObj);

			[DispId(DispId.HTMLTEXTCONTAINEREVENTS2_ONSELECT)]
			void onselect(IHTMLEventObj pEvtObj);

		}

		[ComImport()]
		[Guid("1FF6AA72-5842-11cf-A707-00AA00C0098D")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		public interface IHTMLTextContainerEvents
		{
			[DispId(DispId.HTMLELEMENTEVENTS_ONHELP)]
			Boolean onhelp();

			[DispId(DispId.HTMLELEMENTEVENTS_ONCLICK)]
			Boolean onclick();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDBLCLICK)]
			Boolean ondblclick();

			[DispId(DispId.HTMLELEMENTEVENTS_ONKEYPRESS)]
			Boolean onkeypress();

			[DispId(DispId.HTMLELEMENTEVENTS_ONKEYDOWN)]
			void onkeydown();

			[DispId(DispId.HTMLELEMENTEVENTS_ONKEYUP)]
			void onkeyup();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEOUT)]
			void onmouseout();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEOVER)]
			void onmouseover();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEMOVE)]
			void onmousemove();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEDOWN)]
			void onmousedown();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEUP)]
			void onmouseup();

			[DispId(DispId.HTMLELEMENTEVENTS_ONSELECTSTART)]
			Boolean onselectstart();

			[DispId(DispId.HTMLELEMENTEVENTS_ONFILTERCHANGE)]
			void onfilterchange();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDRAGSTART)]
			Boolean ondragstart();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFOREUPDATE)]
			Boolean onbeforeupdate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONAFTERUPDATE)]
			void onafterupdate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONERRORUPDATE)]
			Boolean onerrorupdate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONROWEXIT)]
			Boolean onrowexit();

			[DispId(DispId.HTMLELEMENTEVENTS_ONROWENTER)]
			void onrowenter();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDATASETCHANGED)]
			void ondatasetchanged();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDATAAVAILABLE)]
			void ondataavailable();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDATASETCOMPLETE)]
			void ondatasetcomplete();

			[DispId(DispId.HTMLELEMENTEVENTS_ONLOSECAPTURE)]
			void onlosecapture();

			[DispId(DispId.HTMLELEMENTEVENTS_ONPROPERTYCHANGE)]
			void onpropertychange();

			[DispId(DispId.HTMLELEMENTEVENTS_ONSCROLL)]
			void onscroll();

			[DispId(DispId.HTMLELEMENTEVENTS_ONFOCUS)]
			void onfocus();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBLUR)]
			void onblur();

			[DispId(DispId.HTMLELEMENTEVENTS_ONRESIZE)]
			void onresize();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDRAG)]
			Boolean ondrag();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDRAGEND)]
			void ondragend();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDRAGENTER)]
			Boolean ondragenter();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDRAGOVER)]
			Boolean ondragover();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDRAGLEAVE)]
			void ondragleave();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDROP)]
			Boolean ondrop();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFORECUT)]
			Boolean onbeforecut();

			[DispId(DispId.HTMLELEMENTEVENTS_ONCUT)]
			Boolean oncut();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFORECOPY)]
			Boolean onbeforecopy();

			[DispId(DispId.HTMLELEMENTEVENTS_ONCOPY)]
			Boolean oncopy();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFOREPASTE)]
			Boolean onbeforepaste();

			[DispId(DispId.HTMLELEMENTEVENTS_ONPASTE)]
			Boolean onpaste();

			[DispId(DispId.HTMLELEMENTEVENTS_ONCONTEXTMENU)]
			Boolean oncontextmenu();

			[DispId(DispId.HTMLELEMENTEVENTS_ONROWSDELETE)]
			void onrowsdelete();

			[DispId(DispId.HTMLELEMENTEVENTS_ONROWSINSERTED)]
			void onrowsinserted();

			[DispId(DispId.HTMLELEMENTEVENTS_ONCELLCHANGE)]
			void oncellchange();

			[DispId(DispId.HTMLELEMENTEVENTS_ONREADYSTATECHANGE)]
			void onreadystatechange();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFOREEDITFOCUS)]
			void onbeforeeditfocus();

			[DispId(DispId.HTMLELEMENTEVENTS_ONLAYOUTCOMPLETE)]
			void onlayoutcomplete();

			[DispId(DispId.HTMLELEMENTEVENTS_ONPAGE)]
			void onpage();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFOREDEACTIVATE)]
			Boolean onbeforedeactivate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONBEFOREACTIVATE)]
			Boolean onbeforeactivate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOVE)]
			void onmove();

			[DispId(DispId.HTMLELEMENTEVENTS_ONCONTROLSELECT)]
			Boolean oncontrolselect();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOVESTART)]
			Boolean onmovestart();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOVEEND)]
			void onmoveend();

			[DispId(DispId.HTMLELEMENTEVENTS_ONRESIZESTART)]
			Boolean onresizestart();

			[DispId(DispId.HTMLELEMENTEVENTS_ONRESIZEEND)]
			void onresizeend();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEENTER)]
			void onmouseenter();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSELEAVE)]
			void onmouseleave();

			[DispId(DispId.HTMLELEMENTEVENTS_ONMOUSEWHEEL)]
			Boolean onmousewheel();

			[DispId(DispId.HTMLELEMENTEVENTS_ONACTIVATE)]
			void onactivate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONDEACTIVATE)]
			void ondeactivate();

			[DispId(DispId.HTMLELEMENTEVENTS_ONFOCUSIN)]
			void onfocusin();

			[DispId(DispId.HTMLELEMENTEVENTS_ONFOCUSOUT)]
			void onfocusout();

			[DispId(DispId.HTMLTEXTCONTAINEREVENTS_ONCHANGE)]
			void onchange();

			[DispId(DispId.HTMLTEXTCONTAINEREVENTS_ONSELECT)]
			void onselect();

		}

		[ComImport()]
		[Guid("96A0A4E0-D062-11cf-94B6-00AA0060275C")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		public interface IHTMLWindowEvents
		{
			[DispId(DispId.HTMLWINDOWEVENTS_ONLOAD)]
			void onload();

			[DispId(DispId.HTMLWINDOWEVENTS_ONUNLOAD)]
			void onunload();

			[DispId(DispId.HTMLWINDOWEVENTS_ONHELP)]
			Boolean onhelp();

			[DispId(DispId.HTMLWINDOWEVENTS_ONFOCUS)]
			void onfocus();

			[DispId(DispId.HTMLWINDOWEVENTS_ONBLUR)]
			void onblur();

			[DispId(DispId.HTMLWINDOWEVENTS_ONERROR)]
			void onerror(String description, String url, Int32 line);

			[DispId(DispId.HTMLWINDOWEVENTS_ONRESIZE)]
			void onresize();

			[DispId(DispId.HTMLWINDOWEVENTS_ONSCROLL)]
			void onscroll();

			[DispId(DispId.HTMLWINDOWEVENTS_ONBEFOREUNLOAD)]
			void onbeforeunload();

			[DispId(DispId.HTMLWINDOWEVENTS_ONBEFOREPRINT)]
			void onbeforeprint();

			[DispId(DispId.HTMLWINDOWEVENTS_ONAFTERPRINT)]
			void onafterprint();

		}


		# endregion

		#endregion

		# region DataBinding

		[ComImport()]
		[Guid("3050f3f2-98b5-11cf-bb82-00aa00bdce0b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
		public interface IHTMLDatabinding
		{
			String dataSrc
			{
				[DispId(DispId.IHTMLDATABINDING_DATASRC)]
				get;
				[DispId(DispId.IHTMLDATABINDING_DATASRC)]
				set;
			}
			String dataFormatAs
			{
				[DispId(DispId.IHTMLDATABINDING_DATAFORMATAS)]
				get;
				[DispId(DispId.IHTMLDATABINDING_DATAFORMATAS)]
				set;
			}
			String dataFld
			{
				[DispId(DispId.IHTMLDATABINDING_DATAFLD)]
				get;
				[DispId(DispId.IHTMLDATABINDING_DATAFLD)]
				set;
			}
		}

		# endregion

		[Guid("00020400-0000-0000-c000-000000000046"),
		InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IDispatch
		{
			int GetTypeInfoCount([Out] out int count);


			int GetTypeInfo([In] uint mustBeZero,
				[In] int localeId,
				[Out, MarshalAs(UnmanagedType.Interface)] 
				out ITypeInfo typeInfo);


			int GetIDsOfNames(
				ref Guid refIidDummy,
				ref String[] names,
				uint namesCount,
				int localeId,
				// DISPID[] ids); 
				int ids);


			//int Invoke(//DISPID member, 
			//    Object member,
			//    ref Guid refIidDummy,
			//    uint localeId,
			//    uint flags,
			//    DISPPARAMS dispParams,
			//    out Object[] result,
			//    Object exception,
			//    int errorInd);

			void Invoke(
				[In, MarshalAs(UnmanagedType.U4)] int id,
				[In] ref Guid g,
				[In, MarshalAs(UnmanagedType.U4)] int lcid,
				[In, MarshalAs(UnmanagedType.U4)] int dwFlags,
				[In] DISPPARAMS pdp,
				[Out, MarshalAs(UnmanagedType.LPArray)]
							Object[] pvarRes,
				[Out]
							EXCEPINFO pei,
				[Out, MarshalAs(UnmanagedType.LPArray)]
				int[] nArgError);
		}

		public static bool Succeeded(int hr)
		{
			return (hr >= 0);
		}

		private static int VariantSize = (int)Marshal.OffsetOf(typeof(FindSizeOfVariant), "b");

		public static unsafe IntPtr ArrayToVARIANTVector(object[] args)
		{
			int length = args.Length;
			IntPtr ptr = Marshal.AllocCoTaskMem(length * VariantSize);
			byte* numPtr = (byte*)ptr;
			for (int i = 0; i < length; i++)
			{
				Marshal.GetNativeVariantForObject(args[i], (IntPtr)(numPtr + (VariantSize * i)));
			}
			return ptr;
		}

		public static unsafe void FreeVARIANTVector(IntPtr mem, int len)
		{
			byte* numPtr = (byte*)mem;
			for (int i = 0; i < len; i++)
			{
				Win32.VariantClear(new HandleRef(null, (IntPtr)(numPtr + (VariantSize * i))));
			}
			Marshal.FreeCoTaskMem(mem);
		}

		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		private struct FindSizeOfVariant
		{
			[MarshalAs(UnmanagedType.Struct)]
			public object var;
			public byte b;
		}

	}
}
