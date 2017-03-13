using GuruComponents.CodeEditor.Library.Drawing.GDI;
using GuruComponents.CodeEditor.CodeEditor.TextDraw;

namespace GuruComponents.CodeEditor.CodeEditor
{
	/// <summary>
	/// Represents which split view is currently active in the syntaxbox.
	/// </summary>
	public enum ActiveView
	{
		TopLeft,
		TopRight,
		BottomLeft,
		BottomRight,
	}

	/// <summary>
	/// Indent styles used by the control
	/// </summary>
	public enum IndentStyle
	{
		/// <summary>
		/// Caret is always confined to the first column when a new line is inserted.
		/// </summary>
		None = 0,
		/// <summary>
		/// New lines inherit the same indention as the previous row.
		/// </summary>
		LastRow = 1,
		/// <summary>
		/// New lines get their indention from the scoping level.
		/// <seealso cref="GuruComponents.CodeEditor.CodeEditor.Syntax.Scope.CauseIndent">CauseIndent</seealso>
		/// </summary>
		Scope = 2,
		/// <summary>
		/// New lines get thir indention from the scoping level or from the previous row
		/// depending on which is most indented.
		/// <seealso cref="GuruComponents.CodeEditor.CodeEditor.Syntax.Scope.CauseIndent">CauseIndent</seealso>
		/// </summary>
		Smart = 3,
	}
}

namespace GuruComponents.CodeEditor.CodeEditor.TextDraw
{
	/// <summary>
	/// To be implemented
	/// </summary>
	public enum TextDrawType
	{
		/// <summary>
		/// For public use only
		/// </summary>
		StarBorder = 0,
		/// <summary>
		/// For public use only
		/// </summary>
		MinusBorder = 1,
		/// <summary>
		/// For public use only
		/// </summary>
		DoubleBorder = 2,
		/// <summary>
		/// For public use only
		/// </summary>
		SingleBorder = 3
	}

	/// <summary>
	/// For public use only
	/// </summary>
	public enum TextDrawDirectionType
	{
		/// <summary>
		/// For public use only
		/// </summary>
		Right = 1,
		/// <summary>
		/// For public use only
		/// </summary>
		Left = 2,
		/// <summary>
		/// For public use only
		/// </summary>
		Up = 4,
		/// <summary>
		/// For public use only
		/// </summary>
		Down = 8
	}


	/// <summary>
	/// For public use only
	/// </summary>
	public enum TextBorderChars
	{
		/// <summary>
		/// For public use only
		/// </summary>
		DownRight = 0,
		/// <summary>
		/// For public use only
		/// </summary>
		RightLeft = 1,
		/// <summary>
		/// For public use only
		/// </summary>
		DownRightLeft = 2,
		/// <summary>
		/// For public use only
		/// </summary>
		DownLeft = 4,
		/// <summary>
		/// For public use only
		/// </summary>
		DownUp = 5,
		/// <summary>
		/// For public use only
		/// </summary>
		DownUpRight = 10,
		/// <summary>
		/// For public use only
		/// </summary>
		DownUpRightLeft = 11,
		/// <summary>
		/// For public use only
		/// </summary>
		DownUpLeft = 12,
		/// <summary>
		/// For public use only
		/// </summary>
		UpRight = 20,
		/// <summary>
		/// For public use only
		/// </summary>
		UpRightLeft = 21,
		/// <summary>
		/// For public use only
		/// </summary>
		UpLeft = 22,
		/// <summary>
		/// For public use only
		/// </summary>
		Blank = 6
	}

	/// <summary>
	/// Text actions that can be performed by the SyntaxBoxControl
	/// </summary>
	public enum XTextAction
	{
		/// <summary>
		/// The control is not performing any action
		/// </summary>
		xtNone = 0,
		/// <summary>
		/// The control is in Drag Drop mode
		/// </summary>
		xtDragText = 1,
		/// <summary>
		/// The control is selecting text
		/// </summary>
		xtSelect = 2
	}
}

namespace GuruComponents.CodeEditor.CodeEditor.Painter
{
	/// <summary>
	/// View point struct used by the SyntaxBoxControl.
	/// The struct contains information about various rendering parameters that the IPainter needs.
	/// </summary>
	public class ViewPoint
	{
		/// <summary>
		/// Used for offsetting the screen in y axis.
		/// </summary>
		public int YOffset;

		/// <summary>
		/// Height of a row in pixels
		/// </summary>
		public int RowHeight;

		/// <summary>
		/// Width of a char (space) in pixels
		/// </summary>
		public int CharWidth;


		/// <summary>
		/// Index of the first visible row
		/// </summary>
		public int FirstVisibleRow;

		/// <summary>
		/// Number of rows that can be displayed in the current view
		/// </summary>
		public int VisibleRowCount;

		/// <summary>
		/// Index of the first visible column
		/// </summary>
		public int FirstVisibleColumn;

		/// <summary>
		/// Width of the client area in pixels
		/// </summary>
		public int ClientAreaWidth;

		/// <summary>
		/// Height of the client area in pixels
		/// </summary>
		public int ClientAreaStart;

		/// <summary>
		/// The action that the SyntaxBoxControl is currently performing
		/// </summary>
		public XTextAction Action;

		/// <summary>
		/// Width of the gutter margin in pixels
		/// </summary>
		public int GutterMarginWidth;

		/// <summary>
		/// Width of the Linenumber margin in pixels
		/// </summary>
		public int LineNumberMarginWidth;

		/// <summary>
		/// 
		/// </summary>
		public int TotalMarginWidth;

		/// <summary>
		/// Width of the text margin (sum of gutter + linenumber + folding margins)
		/// </summary>
		public int TextMargin;

		//document items
	}


	/// <summary>
	/// Struct used by the Painter_GDI class.
	/// </summary>
	public class RenderItems
	{
		/// <summary>
		/// For public use only
		/// </summary>
		public GDISurface BackBuffer; //backbuffer surface
		/// <summary>
		/// For public use only
		/// </summary>
		public GDISurface SelectionBuffer; //backbuffer surface
		/// <summary>
		/// For public use only
		/// </summary>
		public GDISurface StringBuffer; //backbuffer surface
		/// <summary>
		/// For public use only
		/// </summary>
		public GDIFont FontNormal; //Font , no decoration		
		/// <summary>
		/// For public use only
		/// </summary>
		public GDIFont FontBold; //Font , bold
		/// <summary>
		/// For public use only
		/// </summary>
		public GDIFont FontItalic; //Font , italic
		/// <summary>
		/// For public use only
		/// </summary>
		public GDIFont FontBoldItalic; //Font , bold & italic
		/// <summary>
		/// For public use only
		/// </summary>
		public GDIFont FontUnderline; //Font , no decoration		
		/// <summary>
		/// For public use only
		/// </summary>
		public GDIFont FontBoldUnderline; //Font , bold
		/// <summary>
		/// For public use only
		/// </summary>
		public GDIFont FontItalicUnderline; //Font , italic
		/// <summary>
		/// For public use only
		/// </summary>
		public GDIFont FontBoldItalicUnderline; //Font , bold & italic
		/// <summary>
		/// For public use only
		/// </summary>
		public GDIBrush GutterMarginBrush; //Gutter margin brush
		/// <summary>
		/// For public use only
		/// </summary>
		public GDIBrush GutterMarginBorderBrush; //Gutter margin brush
		/// <summary>
		/// For public use only
		/// </summary>
		public GDIBrush LineNumberMarginBrush; //linenumber margin brush
		/// <summary>
		/// For public use only
		/// </summary>
		public GDIBrush LineNumberMarginBorderBrush; //linenumber margin brush
		/// <summary>
		/// For public use only
		/// </summary>
		public GDIBrush BackgroundBrush; //background brush
		/// <summary>
		/// For public use only
		/// </summary>
		public GDIBrush HighLightLineBrush; //background brush
		/// <summary>
		/// For public use only
		/// </summary>
		public GDIBrush OutlineBrush; //background brush
	}
}