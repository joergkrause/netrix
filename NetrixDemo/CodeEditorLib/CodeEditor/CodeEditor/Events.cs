

using System;
using System.Drawing;
using System.Windows.Forms;
using GuruComponents.CodeEditor.CodeEditor.Syntax;

namespace GuruComponents.CodeEditor.CodeEditor
{
	public delegate void RowPaintHandler(object sender, RowPaintEventArgs e);

	public delegate void RowMouseHandler(object sender, RowMouseEventArgs e);

	public delegate void CopyHandler(object sender, CopyEventArgs e);

	public delegate void WordMouseHandler(object sender, ref WordMouseEventArgs e);

	/// <summary>
	/// Event arg for Copy/Cut actions.
	/// </summary>
	public class CopyEventArgs
	{
		/// <summary>
		/// The text copied to the clipboard.
		/// </summary>
		public string Text = "";
	}

	/// <summary>
	/// Event args passed to word mouse events of the syntaxbox
	/// </summary>
	public class WordMouseEventArgs : EventArgs
	{
		/// <summary>
		/// The word where the event was fired
		/// </summary>
		public Word Word;

		/// <summary>
		/// The pattern that triggered the event
		/// </summary>
		public Pattern Pattern;

		/// <summary>
		/// The mouse buttons that was pressed when the event fired
		/// </summary>
		public MouseButtons Button;

		/// <summary>
		/// Reference to a mouse cursor , developers can assign new values here to display new cursors for a given pattern
		/// </summary>
		public Cursor Cursor;
	}

	/// <summary>
	/// Event args for mouse actions on the syntaxbox
	/// </summary>
	public class RowMouseEventArgs : EventArgs
	{
		/// <summary>
		/// The row where the event was fired
		/// </summary>
		public Row Row;

		/// <summary>
		/// The mousebuttons that was pressed when the event was fired
		/// </summary>
		public MouseButtons Button;

		/// <summary>
		/// The X position of the mouse cursor when the event was fired
		/// </summary>
		public int MouseX;

		/// <summary>
		/// The Y position of the mouse cursor when the event was fired
		/// </summary>
		public int MouseY;

		/// <summary>
		/// The area of the row where the event was fired
		/// </summary>
		public RowArea Area;
	}


	/// <summary>
	/// Describes in what area a mouse event occured on a row
	/// </summary>
	public enum RowArea
	{
		/// <summary>
		/// Represents the gutter margin
		/// </summary>
		GutterArea,
		/// <summary>
		/// Represents the LineNumber section
		/// </summary>
		LineNumberArea,
		/// <summary>
		/// Represents the area where the folding symbols are shown
		/// </summary>
		FoldingArea,
		/// <summary>
		/// Represents the actual text part of a row
		/// </summary>
		TextArea,
	}

	/// <summary>
	/// Event args passed to owner draw events of the syntaxbox
	/// </summary>
	public class RowPaintEventArgs : EventArgs
	{
		/// <summary>
		/// The row to draw
		/// </summary>
		public Row Row;

		/// <summary>
		/// The graphics surface to draw on
		/// </summary>
		public Graphics Graphics;

		/// <summary>
		/// the bounds of the row
		/// </summary>
		public Rectangle Bounds;
	}


}