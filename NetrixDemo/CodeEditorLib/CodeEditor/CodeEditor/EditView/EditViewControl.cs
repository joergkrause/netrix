

#region using ...

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Windows.Forms;
using GuruComponents.CodeEditor.CodeEditor.Syntax;
using GuruComponents.CodeEditor.Forms;
using GuruComponents.CodeEditor.CodeEditor.Painter;
using GuruComponents.CodeEditor.CodeEditor.TextDraw;
using ScrollEventArgs = GuruComponents.CodeEditor.Forms.IntelliMouse.ScrollEventArgs;
using ScrollEventHandler = GuruComponents.CodeEditor.Forms.IntelliMouse.ScrollEventHandler;
using GuruComponents.CodeEditor.Library.Timers;
using GuruComponents.CodeEditor.Library.Win32;
using System.IO;
using GuruComponents.CodeEditor.Library.Globalization;

#endregion

namespace GuruComponents.CodeEditor.CodeEditor
{
	[ToolboxItem(false)]
	public sealed class EditViewControl : SplitViewChildWidget
	{

		#region General Declarations

		private int MouseX = 0;
		private int MouseY = 0;
		private MouseButtons MouseButton = 0;

		/// <summary>
		/// The Point in the text where the Autolist was activated.
		/// </summary>
		public TextPoint AutoListStartPos = null;

		private Selection _Selection;
		private Caret _Caret;
		private double _IntelliScrollPos = 0;

		/// <summary>
		/// The Point in the text where the InfoTip was activated.
		/// </summary>		
		public TextPoint InfoTipStartPos = null;

		private bool _AutoListVisible = false;
		private bool _InfoTipVisible = false;
		private bool _OverWrite = false;
		private bool _KeyDownHandled = false;
		public ViewPoint View = new ViewPoint();
		public IPainter Painter;
		private TextDrawType _TextDrawStyle = 0;

		private string[] TextBorderStyles = new string[]
			{
				"****** * ******* * ******",
				"+---+| | |+-+-+| | |+---+",
				"+---+¦ ¦ ¦¦-+-¦¦ ¦ ¦+---+",
				"+---+¦ ¦ ¦+-+-¦¦ ¦ ¦+---+"
			};

		#endregion

		#region Internal controls

		private WeakTimer CaretTimer;
		private PictureBox Filler;
		private ToolTip tooltip;
		private IContainer components;

		#region PUBLIC PROPERTY FINDREPLACEDIALOG 

		private FindReplaceForm _FindReplaceDialog;

		public FindReplaceForm FindReplaceDialog
		{
			get
			{
				CreateFindForm();


				return _FindReplaceDialog;
			}
			set { _FindReplaceDialog = value; }
		}

		#endregion

		private IntelliMouseControl IntelliMouse;

		#region PUBLIC PROPERTY AUTOLIST

		private AutoListForm _AutoList;

		public AutoListForm AutoList
		{
			get
			{
				CreateAutoList();

				return _AutoList;
			}
			set { _AutoList = value; }
		}

		#endregion

		#region PUBLIC PROPERTY INFOTIP

		private InfoTipForm _InfoTip;

		public InfoTipForm InfoTip
		{
			get
			{
				CreateInfoTip();

				return _InfoTip;
			}
			set { _InfoTip = value; }
		}

		#endregion

		public bool HasAutoList
		{
			get { return this._AutoList != null; }
		}

		public bool HasInfoTip
		{
			get { return this._InfoTip != null; }
		}


		private WeakReference _Control = null;

		public CodeEditorControl _CodeEditor
		{
			get
			{
				try
				{
					if (_Control != null && _Control.IsAlive)
						return (CodeEditorControl) _Control.Target;
					else
						return null;
				}
				catch
				{
					return null;
				}
			}
			set { _Control = new WeakReference(value); }
		}

		#endregion

		#region Public events

		/// <summary>
		/// An event that is fired when the caret has moved.
		/// </summary>
		public event EventHandler CaretChange = null;

		/// <summary>
		/// An event that is fired when the selection has changed.
		/// </summary>
		public event EventHandler SelectionChange = null;

		/// <summary>
		/// An event that is fired when mouse down occurs on a row
		/// </summary>
		public event RowMouseHandler RowMouseDown = null;

		/// <summary>
		/// An event that is fired when mouse move occurs on a row
		/// </summary>
		public event RowMouseHandler RowMouseMove = null;

		/// <summary>
		/// An event that is fired when mouse up occurs on a row
		/// </summary>
		public event RowMouseHandler RowMouseUp = null;

		/// <summary>
		/// An event that is fired when a click occurs on a row
		/// </summary>
		public event RowMouseHandler RowClick = null;

		/// <summary>
		/// An event that is fired when a double click occurs on a row
		/// </summary>
		public event RowMouseHandler RowDoubleClick = null;

		/// <summary>
		/// An event that is fired when the control has updated the clipboard
		/// </summary>
		public event CopyHandler ClipboardUpdated = null;

		#endregion


        public CodeEditorControl CodeEditor
        {
            get
            {
                try
                {
                    if (_Control != null && _Control.IsAlive)
                        return (CodeEditorControl)_Control.Target;
                    else
                        return null;
                }
                catch
                {
                    return null;
                }
            }
            set { _Control = new WeakReference(value); }
        }

        #region PUBLIC PROPERTY IMEWINDOW

        private IMEWindow _IMEWindow;

        public IMEWindow IMEWindow
        {
            get { return _IMEWindow; }
            set { _IMEWindow = value; }
        }

        #endregion

		private void CreateAutoList()
		{
			if (this._CodeEditor != null && !this._CodeEditor.DisableAutoList && this._AutoList == null)
			{
				//Debug.WriteLine("Creating Autolist");

				this.AutoList = new AutoListForm(this);
				NativeMethods.SetWindowLong(this.AutoList.Handle,
				                            NativeMethods.GWL_STYLE,
				                            (int)NativeMethods.WS_CHILD);

				this.AutoList.SendToBack();
				this.AutoList.Visible = false;
				//this.Controls.Add (this.AutoList);
				this.AutoList.DoubleClick += new EventHandler(this.AutoListDoubleClick);

				this.AutoList.Images = this._CodeEditor.AutoListIcons;
				this.AutoList.Add("a123", "a123", "Some tooltip for this item 1", 1);
				this.AutoList.Add("b456", "b456", "Some tooltip for this item 2", 2);
				this.AutoList.Add("c789", "c789", "Some tooltip for this item 3", 2);
				this.AutoList.Add("d012", "d012", "Some tooltip for this item 4", 3);
				this.AutoList.Add("e345", "e345", "Some tooltip for this item 5", 4);
			}
		}

		private void CreateFindForm()
		{
			if (!this._CodeEditor.DisableFindForm && this._FindReplaceDialog == null)
			{
				Debug.WriteLine("Creating Findform");
				FindReplaceDialog = new FindReplaceForm(this);
			}
		}

		private void CreateInfoTip()
		{
			if (this._CodeEditor != null && !this._CodeEditor.DisableInfoTip && this._InfoTip == null)
			{
				Debug.WriteLine("Creating Infotip");

				this.InfoTip = new InfoTipForm(this);
				NativeMethods.SetWindowLong(this.InfoTip.Handle,
				                            NativeMethods.GWL_STYLE,
				                            (int)NativeMethods.WS_CHILD);

				this.InfoTip.SendToBack();
				this.InfoTip.Visible = false;
			}
		}

		#region Constructor

		/// <summary>
		/// Default constructor for the SyntaxBoxControl
		/// </summary>
		public EditViewControl(CodeEditorControl Parent) : base()
		{
			_CodeEditor = Parent;


			Painter = new Painter_GDI(this);
			_Selection = new Selection(this);
			_Caret = new Caret(this);

			_Caret.Change += new EventHandler(this.CaretChanged);
			_Selection.Change += new EventHandler(this.SelectionChanged);


			//	this.AttachDocument (_SyntaxBox.Document);


			InitializeComponent();


			CreateAutoList();
			//CreateFindForm ();
			CreateInfoTip();

            this.SetStyle(ControlStyles.AllPaintingInWmPaint, false);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, false);
			this.SetStyle(ControlStyles.Selectable, true);
			this.SetStyle(ControlStyles.ResizeRedraw, true);
			this.SetStyle(ControlStyles.Opaque, true);
			this.SetStyle(ControlStyles.UserPaint, true);
           // this.SetStyle(ControlStyles.UserMouse, true);
		}

		#endregion

		#region DISPOSE()

		/// <summary>
		/// </summary>
		/// <param name="disposing"></param>
		protected override void Dispose(bool disposing)
		{
			RemoveFocus();
#if DEBUG
			try
			{
				Console.WriteLine("disposing editview");
			}
			catch
			{
			}
#endif

			if (disposing)
			{
				if (components != null)
					components.Dispose();

				try
				{
					if (this.Painter != null)
						this.Painter.Dispose();
				}
				catch
				{
				}
			}
			base.Dispose(disposing);
		}

		#endregion

		~EditViewControl()
		{
#if DEBUG
			try
			{
				Console.WriteLine("finalizing editview");
			}
			catch
			{
			}
#endif
		}

		#region Private/Protected/public Properties

		public int PixelTabSize
		{
			get { return _CodeEditor.TabSize*View.CharWidth; }
		}

		#endregion

		#region Private/Protected/Internal Methods

		private void DoResize()
		{
			if (this.Visible && this.Width > 0 && this.Height > 0 && this.IsHandleCreated)
			{
				try
				{
					if (Filler == null)
						return;

					TopThumb.Width = SystemInformation.VerticalScrollBarWidth;
					LeftThumb.Height = SystemInformation.HorizontalScrollBarHeight;
					vScroll.Width = SystemInformation.VerticalScrollBarWidth;
					hScroll.Height = SystemInformation.HorizontalScrollBarHeight;

					if (TopThumbVisible)
					{
						vScroll.Top = TopThumb.Height;
						if (hScroll.Visible)
							vScroll.Height = this.ClientHeight - hScroll.Height - TopThumb.Height;
						else
							vScroll.Height = this.ClientHeight - TopThumb.Height;

					}
					else
					{
						if (hScroll.Visible)
							vScroll.Height = this.ClientHeight - hScroll.Height;
						else
							vScroll.Height = this.ClientHeight;

						vScroll.Top = 0;
					}

					if (LeftThumbVisible)
					{
						hScroll.Left = LeftThumb.Width;
						if (vScroll.Visible)
							hScroll.Width = this.ClientWidth - vScroll.Width - LeftThumb.Width;
						else
							hScroll.Width = this.ClientWidth - LeftThumb.Width;


					}
					else
					{
						if (vScroll.Visible)
							hScroll.Width = this.ClientWidth - vScroll.Width;
						else
							hScroll.Width = this.ClientWidth;

						hScroll.Left = 0;
					}


					if (this.Width != OldWidth && this.Width > 0)
					{
						OldWidth = this.Width;
						if (Painter != null)
							Painter.Resize();
					}


					vScroll.Left = this.ClientWidth - vScroll.Width;
					hScroll.Top = this.ClientHeight - hScroll.Height;

					LeftThumb.Left = 0;
					LeftThumb.Top = hScroll.Top;

					TopThumb.Left = vScroll.Left;
					;
					TopThumb.Top = 0;


					Filler.Left = vScroll.Left;
					Filler.Top = hScroll.Top;
					Filler.Width = vScroll.Width;
					Filler.Height = hScroll.Height;

				}
				catch
				{
				}

			}
		}

		private void InsertText(string text)
		{
			Caret.CropPosition();
			if (Selection.IsValid)
			{
				Selection.DeleteSelection();
				InsertText(text);
			}
			else
			{
				if (!_OverWrite || text.Length > 1)
				{
					Row xtr = Caret.CurrentRow;
					TextPoint p = Document.InsertText(text, Caret.Position.X, Caret.Position.Y);
					Caret.CurrentRow.Parse(true);
					if (text.Length == 1)
					{
						Caret.SetPos(p);
						Caret.CaretMoved(false);

					}
					else
					{
						//Document.i = true;

						Document.ResetVisibleRows();
						Caret.SetPos(p);
						Caret.CaretMoved(false);
					}
				}
				else
				{
					TextRange r = new TextRange();
					r.FirstColumn = Caret.Position.X;
					r.FirstRow = Caret.Position.Y;
					r.LastColumn = Caret.Position.X + 1;
					r.LastRow = Caret.Position.Y;
					UndoBlockCollection ag = new UndoBlockCollection();
					UndoBlock b;
					b = new UndoBlock();
					b.Action = UndoAction.DeleteRange;
					b.Text = Document.GetRange(r);
					b.Position = Caret.Position;
					ag.Add(b);
					Document.DeleteRange(r, false);
					b = new UndoBlock();
					b.Action = UndoAction.InsertRange;
					string NewChar = text;
					b.Text = NewChar;
					b.Position = Caret.Position;
					ag.Add(b);
					Document.AddToUndoList(ag);
					Document.InsertText(NewChar, Caret.Position.X, Caret.Position.Y, false);
					Caret.CurrentRow.Parse(true);

					Caret.MoveRight(false);
				}
			}
			//	this.ScrollIntoView ();

		}

		private void InsertEnter()
		{
			Caret.CropPosition();
			if (Selection.IsValid)
			{
				Selection.DeleteSelection();
				InsertEnter();
			}
			else
			{
				switch (this.Indent)
				{
					case IndentStyle.None:
						{
							Row xtr = Caret.CurrentRow;
							Document.InsertText("\n", Caret.Position.X, Caret.Position.Y);
							//depends on what sort of indention we are using....
							Caret.CurrentRow.Parse();
							Caret.MoveDown(false);
							Caret.CurrentRow.Parse(false);
							Caret.CurrentRow.Parse(true);

							Caret.Position.X = 0;
							Caret.SetPos(Caret.Position);
							break;
						}
					case IndentStyle.LastRow:
						{
							Row xtr = Caret.CurrentRow;
							string indent = xtr.GetLeadingWhitespace();
							int Max = Math.Min(indent.Length, Caret.Position.X);
							string split = "\n" + indent.Substring(0, Max);
							Document.InsertText(split, Caret.Position.X, Caret.Position.Y);
							Document.ResetVisibleRows();
							Caret.CurrentRow.Parse(false);
							Caret.CurrentRow.Parse(true);
							Caret.MoveDown(false);
							Caret.CurrentRow.Parse(false);
							Caret.CurrentRow.Parse(true);

							Caret.Position.X = indent.Length;
							Caret.SetPos(Caret.Position);
							xtr.Parse(false);
							xtr.Parse(true);
							xtr.NextRow.Parse(false);
							xtr.NextRow.Parse(true);

							break;
						}
					case IndentStyle.Scope:
						{
							Row xtr = Caret.CurrentRow;
							xtr.Parse(true);
							if (xtr.ShouldOutdent)
							{
								OutdentEndRow();
							}

							Document.InsertText("\n", Caret.Position.X, Caret.Position.Y);
							//depends on what sort of indention we are using....
							Caret.CurrentRow.Parse();
							Caret.MoveDown(false);
							Caret.CurrentRow.Parse(false);

							string indent = new String('\t', Caret.CurrentRow.Depth);
							Document.InsertText(indent, 0, Caret.Position.Y);

							Caret.CurrentRow.Parse(false);
							Caret.CurrentRow.Parse(true);

							Caret.Position.X = indent.Length;
							Caret.SetPos(Caret.Position);
							Caret.CropPosition();
							Selection.ClearSelection();

							xtr.Parse(false);
							xtr.Parse(true);
							xtr.NextRow.Parse(false);
							xtr.NextRow.Parse(true);

							break;

						}
					case IndentStyle.Smart:
						{
							Row xtr = Caret.CurrentRow;
							if (xtr.ShouldOutdent)
							{
								OutdentEndRow();
							}
							Document.InsertText("\n", Caret.Position.X, Caret.Position.Y);
							Caret.MoveDown(false);
							Caret.CurrentRow.Parse(false);
							Caret.CurrentRow.Parse(true);
							Caret.CurrentRow.StartSegment.StartRow.Parse(false);
							Caret.CurrentRow.StartSegment.StartRow.Parse(true);

							string prev = "\t" + Caret.CurrentRow.StartSegment.StartRow.GetVirtualLeadingWhitespace();

							string indent = Caret.CurrentRow.PrevRow.GetLeadingWhitespace();
							if (indent.Length < prev.Length)
								indent = prev;

							string ts = "\t" + new String(' ', this.TabSize);
							while (indent.IndexOf(ts) >= 0)
							{
								indent = indent.Replace(ts, "\t\t");
							}

							Document.InsertText(indent, 0, Caret.Position.Y);

							Caret.CurrentRow.Parse(false);
							Caret.CurrentRow.Parse(true);

							Caret.Position.X = indent.Length;
							Caret.SetPos(Caret.Position);

							Caret.CropPosition();
							Selection.ClearSelection();
							xtr.Parse(false);
							xtr.Parse(true);
							xtr.NextRow.Parse(false);
							xtr.NextRow.Parse(true);
							break;
						}
				}
				ScrollIntoView();
			}
		}

		private void OutdentEndRow()
		{
			try
			{
				if (this.Indent == IndentStyle.Scope)
				{
					Row xtr = Caret.CurrentRow;
					string ct = xtr.Text.Substring(0, xtr.GetLeadingWhitespace().Length);
					string indent1 = new String('\t', Caret.CurrentRow.Depth);
					TextRange tr = new TextRange();
					tr.FirstColumn = 0;
					tr.LastColumn = xtr.GetLeadingWhitespace().Length;
					tr.FirstRow = xtr.Index;
					tr.LastRow = xtr.Index;
					this.Document.DeleteRange(tr);
					this.Document.InsertText(indent1, 0, xtr.Index, true);

					int diff = indent1.Length - tr.LastColumn;
					Caret.Position.X += diff;
					Caret.SetPos(Caret.Position);
					Caret.CropPosition();
					Selection.ClearSelection();
					Caret.CurrentRow.Parse(false);
					Caret.CurrentRow.Parse(true);

				}
				else if (this.Indent == IndentStyle.Smart)
				{
					Row xtr = Caret.CurrentRow;

					if (xtr.FirstNonWsWord == xtr.Expansion_EndSegment.EndWord)
					{
						string ct = xtr.Text.Substring(0, xtr.GetLeadingWhitespace().Length);
						//int j=xtr.Expansion_StartRow.StartWordIndex;
						string indent1 = xtr.StartSegment.StartWord.Row.GetVirtualLeadingWhitespace();
						TextRange tr = new TextRange();
						tr.FirstColumn = 0;
						tr.LastColumn = xtr.GetLeadingWhitespace().Length;
						tr.FirstRow = xtr.Index;
						tr.LastRow = xtr.Index;
						this.Document.DeleteRange(tr);
						string ts = "\t" + new String(' ', this.TabSize);
						while (indent1.IndexOf(ts) >= 0)
						{
							indent1 = indent1.Replace(ts, "\t\t");
						}
						this.Document.InsertText(indent1, 0, xtr.Index, true);

						int diff = indent1.Length - tr.LastColumn;
						Caret.Position.X += diff;
						Caret.SetPos(Caret.Position);
						Caret.CropPosition();
						Selection.ClearSelection();
						Caret.CurrentRow.Parse(false);
						Caret.CurrentRow.Parse(true);
					}
				}
			}
			catch
			{
			}
		}

		private void DeleteForward()
		{
			Caret.CropPosition();
			if (Selection.IsValid)
				Selection.DeleteSelection();
			else
			{
				Row xtr = Caret.CurrentRow;
				if (Caret.Position.X == xtr.Text.Length)
				{
					if (Caret.Position.Y <= Document.Count - 2)
					{
						TextRange r = new TextRange();
						r.FirstColumn = Caret.Position.X;
						r.FirstRow = Caret.Position.Y;
						r.LastRow = r.FirstRow + 1;
						r.LastColumn = 0;

						Document.DeleteRange(r);
						Document.ResetVisibleRows();

					}
				}
				else
				{
					TextRange r = new TextRange();
					r.FirstColumn = Caret.Position.X;
					r.FirstRow = Caret.Position.Y;
					r.LastRow = r.FirstRow;
					r.LastColumn = r.FirstColumn + 1;
					Document.DeleteRange(r);
					Document.ResetVisibleRows();
					Caret.CurrentRow.Parse(false);
					Caret.CurrentRow.Parse(true);
				}
			}
		}

		private void DeleteBackwards()
		{
			Caret.CropPosition();
			if (Selection.IsValid)
				Selection.DeleteSelection();
			else
			{
				Row xtr = Caret.CurrentRow;
				if (Caret.Position.X == 0)
				{
					if (Caret.Position.Y > 0)
					{
						Caret.Position.Y --;
						Caret.MoveEnd(false);
						DeleteForward();
						//Caret.CurrentRow.Parse ();
						Document.ResetVisibleRows();
					}
				}
				else
				{
					if (Caret.Position.X >= xtr.Text.Length)
					{
						TextRange r = new TextRange();
						r.FirstColumn = Caret.Position.X - 1;
						r.FirstRow = Caret.Position.Y;
						r.LastRow = r.FirstRow;
						r.LastColumn = r.FirstColumn + 1;
						Document.DeleteRange(r);
						Document.ResetVisibleRows();
						Caret.MoveEnd(false);
						Caret.CurrentRow.Parse();
					}
					else
					{
						TextRange r = new TextRange();
						r.FirstColumn = Caret.Position.X - 1;
						r.FirstRow = Caret.Position.Y;
						r.LastRow = r.FirstRow;
						r.LastColumn = r.FirstColumn + 1;
						Document.DeleteRange(r);
						Document.ResetVisibleRows();
						Caret.MoveLeft(false);
						Caret.CurrentRow.Parse();
					}
				}
			}
		}

		private void ScrollScreen(int Amount)
		{
			ScrollScreen(Amount, 2);
		}

		private void ScrollScreen(int Amount, int speed)
		{
			try
			{
				this.tooltip.RemoveAll();

				int newval = vScroll.Value + Amount;

				newval = Math.Max(newval, vScroll.Minimum);
				newval = Math.Min(newval, vScroll.Maximum);

				if (newval >= vScroll.Maximum - 2)
					newval = vScroll.Maximum - 2;

				vScroll.Value = newval;
				this.Redraw();
			}
			catch
			{
			}
		}

		private void PasteText()
		{
			try
			{
				IDataObject iData = Clipboard.GetDataObject();

				if (iData.GetDataPresent(DataFormats.UnicodeText))
				{
					// Yes it is, so display it in a text box.
					string s = (string) iData.GetData(DataFormats.UnicodeText);

					InsertText(s);
					if (ParseOnPaste)
						this.Document.ParseAll(true);
				}
				else if (iData.GetDataPresent(DataFormats.Text))
				{
					// Yes it is, so display it in a text box.
					string s = (string) iData.GetData(DataFormats.Text);

					InsertText(s);
					if (ParseOnPaste)
						this.Document.ParseAll(true);
				}
			}
			catch
			{
				//ignore
			}
		}


		private void BeginDragDrop()
		{
			this.DoDragDrop(Selection.Text, DragDropEffects.All);

		}

		private void Redraw()
		{
			this.Invalidate();
		}


		private void RedrawCaret()
		{
            using (Graphics gfx = this.CreateGraphics())
            {
                Painter.RenderCaret(gfx);
            }
		}

		private void SetMouseCursor(int x, int y)
		{
			if (_CodeEditor.LockCursorUpdate)
			{
				this.Cursor = _CodeEditor.Cursor;
				return;
			}

			if (View.Action == XTextAction.xtDragText)
			{
				this.Cursor = Cursors.Hand;
				//Cursor.Current = Cursors.Hand;
			}
			else
			{
				if (x < View.TotalMarginWidth)
				{
					if (x < View.GutterMarginWidth)
					{
						this.Cursor = Cursors.Arrow;
					}
					else
					{
						Assembly assembly = GetType().Assembly;

                        Stream strm = assembly.GetManifestResourceStream("GuruComponents.CodeEditor.CodeEditor.Resources.FlippedCursor.cur");
                        if (strm != null)
                        {
                            this.Cursor = new Cursor(strm); 
                        }
					} 
				}
				else
				{
					if (x > View.TextMargin - 8)
					{
						if (IsOverSelection(x, y))
							this.Cursor = Cursors.Arrow;
						else
						{
							TextPoint tp = this.Painter.CharFromPixel(x, y);
							Word w = this.Document.GetWordFromPos(tp);
							if (w != null && w.Pattern != null && w.Pattern.Category != null)
							{
								WordMouseEventArgs e = new WordMouseEventArgs();
								e.Pattern = w.Pattern;
								e.Button = MouseButtons.None;
								e.Cursor = Cursors.Hand;
								e.Word = w;

								this._CodeEditor.OnWordMouseHover(ref e);

								this.Cursor = e.Cursor;
							}
							else
								this.Cursor = Cursors.IBeam;
						}
					}
					else
					{
						this.Cursor = Cursors.Arrow;
					}
				}
			}
		}

		private void CopyText()
		{
			//no freaking vs.net copy empty selection 
			if (!Selection.IsValid)
				return;

			if (this._CodeEditor.CopyAsRTF)
			{
				this.CopyAsRTF();


			}
			else
			{
                //try
                //{
                if (this.Selection != null)
                {
                    string t = Selection.Text;
                    Clipboard.SetDataObject(t, true);
                    CopyEventArgs ea = new CopyEventArgs();
                    ea.Text = t;
                    OnClipboardUpdated(ea);
                }
                //}
                //catch
                //{
                //    try
                //    {
                //        string t = Selection.Text;
                //        Clipboard.SetDataObject(t, true);
                //        CopyEventArgs ea = new CopyEventArgs();
                //        ea.Text = t;
                //        OnClipboardUpdated(ea);
                //    }
                //    catch
                //    {
                //    }
                //}
			}


		}

		/// <summary>
		/// For public use only
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		protected override bool IsInputKey(Keys key)
		{
			switch (key)
			{
				case Keys.Up:
				case Keys.Down:
				case Keys.Right:
				case Keys.Left:
				case Keys.Tab:
				case Keys.PageDown:
				case Keys.PageUp:
				case Keys.Enter:
					return true;
			}
			return true; //base.IsInputKey(key);			
		}

		protected override bool IsInputChar(char c)
		{
			return true;
		}

		private void TextDraw(TextDrawDirectionType Direction)
		{
			TextRange r = new TextRange();
			r.FirstColumn = Caret.Position.X;
			r.FirstRow = Caret.Position.Y;
			r.LastColumn = Caret.Position.X + 1;
			r.LastRow = Caret.Position.Y;

			int Style = (int) this.TextDrawStyle;
			string OldChar = Document.GetRange(r);
			string BorderString = TextBorderStyles[Style];
			//TextBorderChars OldCharType=0;

			if (OldChar == "")
				OldChar = " ";


			UndoBlockCollection ag = new UndoBlockCollection();
			UndoBlock b;
			b = new UndoBlock();
			b.Action = UndoAction.DeleteRange;
			b.Text = Document.GetRange(r);
			b.Position = Caret.Position;
			ag.Add(b);
			Document.DeleteRange(r, false);

			b = new UndoBlock();
			b.Action = UndoAction.InsertRange;


			string NewChar = "*";


			b.Text = NewChar;
			b.Position = Caret.Position;
			ag.Add(b);
			Document.AddToUndoList(ag);
			Document.InsertText(NewChar, Caret.Position.X, Caret.Position.Y, false);
		}


		public void RemoveFocus()
		{
			if (this.InfoTip == null || this.AutoList == null)
				return;

			if (!this.ContainsFocus && !InfoTip.ContainsFocus && !AutoList.ContainsFocus)
			{
				CaretTimer.Enabled = false;
				this.Caret.Blink = false;
				this._AutoListVisible = false;
				this._InfoTipVisible = false;
			}
			this.Redraw();
		}

		private void SelectCurrentWord()
		{
			Row xtr = Caret.CurrentRow;
			if (xtr.Text == "")
				return;

			if (Caret.Position.X >= xtr.Text.Length)
				return;

			string Char = xtr.Text.Substring(Caret.Position.X, 1);
			int Type = CharType(Char);

			int left = Caret.Position.X;
			int right = Caret.Position.X;

			while (left >= 0 && CharType(xtr.Text.Substring(left, 1)) == Type)
				left --;

			while (right <= xtr.Text.Length - 1 && CharType(xtr.Text.Substring(right, 1)) == Type)
				right++;

			Selection.Bounds.FirstRow = Selection.Bounds.LastRow = xtr.Index;
			Selection.Bounds.FirstColumn = left + 1;
			Selection.Bounds.LastColumn = right;
			Caret.Position.X = right;
			Caret.SetPos(Caret.Position);
			this.Redraw();
		}

		private int CharType(string s)
		{
			string g1 = " \t";
			string g2 = ".,-+'?´=)(/&%¤#!\"\\<>[]$£@*:;{}";

			if (g1.IndexOf(s) >= 0)
				return 1;

			if (g2.IndexOf(s) >= 0)
				return 2;

			return 3;
		}

		private void SelectPattern(int RowIndex, int Column, int Length)
		{
			this.Selection.Bounds.FirstColumn = Column;
			this.Selection.Bounds.FirstRow = RowIndex;
			this.Selection.Bounds.LastColumn = Column + Length;
			this.Selection.Bounds.LastRow = RowIndex;
			this.Caret.Position.X = Column + Length;
			this.Caret.Position.Y = RowIndex;
			this.Caret.CurrentRow.EnsureVisible();
			this.ScrollIntoView();
			this.Redraw();
		}

		public void InitVars()
		{
			//setup viewpoint data


			if (View.RowHeight == 0)
				View.RowHeight = 48;

			if (View.CharWidth == 0)
				View.CharWidth = 16;

			//View.RowHeight=16;
			//View.CharWidth=8;

			View.FirstVisibleColumn = hScroll.Value;
			View.FirstVisibleRow = vScroll.Value;
			//	View.yOffset =_yOffset;

			View.VisibleRowCount = 0;
			if (hScroll.Visible)
				View.VisibleRowCount = (this.Height - hScroll.Height)/View.RowHeight + 1;
			else
				View.VisibleRowCount = (this.Height - hScroll.Height)/View.RowHeight + 2;

			if (this.ShowGutterMargin)
				View.GutterMarginWidth = this.GutterMarginWidth;
			else
				View.GutterMarginWidth = 0;

			if (this.ShowLineNumbers)
			{
				int chars = (Document.Count).ToString().Length;
				string s = new String('9', chars);
				View.LineNumberMarginWidth = 10 + this.Painter.MeasureString(s).Width;
			}
			else
				View.LineNumberMarginWidth = 0;


			View.TotalMarginWidth = View.GutterMarginWidth + View.LineNumberMarginWidth;
			if (Document.Folding)
				View.TextMargin = View.TotalMarginWidth + 20;
			else
				View.TextMargin = View.TotalMarginWidth + 7;


			View.ClientAreaWidth = this.Width - vScroll.Width - View.TextMargin;
			View.ClientAreaStart = View.FirstVisibleColumn*View.CharWidth;
		}

		private int MaxCharWidth = 8;

		public void CalcMaxCharWidth()
		{
			MaxCharWidth = this.Painter.GetMaxCharWidth();
		}

		public void SetMaxHorizontalScroll()
		{
			CalcMaxCharWidth();
			int CharWidth = this.View.CharWidth;
			if (CharWidth == 0)
				CharWidth = 1;

			if (this.View.ClientAreaWidth/CharWidth < 0)
			{
				this.hScroll.Maximum = 1000;
				return;
			}

			this.hScroll.LargeChange = this.View.ClientAreaWidth/CharWidth;

			try
			{
				int max = 0;
				for (int i = this.View.FirstVisibleRow; i < this.Document.VisibleRows.Count; i++)
				{
					if (i >= this.View.VisibleRowCount + this.View.FirstVisibleRow)
						break;

					string l = "";

					if (this.Document.VisibleRows[i].IsCollapsed)
						l = this.Document.VisibleRows[i].VirtualCollapsedRow.Text;
					else
						l = this.Document.VisibleRows[i].Text;

					l = l.Replace("\t", new string(' ', this.TabSize));
					if (l.Length > max)
						max = l.Length;
				}

				int pixels = max*MaxCharWidth;
				int chars = pixels/CharWidth;


				if (this.hScroll.Value <= chars)
					this.hScroll.Maximum = chars;
			}
			catch
			{
				this.hScroll.Maximum = 1000;
			}
		}

		public void InitScrollbars()
		{
			if (Document.VisibleRows.Count > 0)
			{
				vScroll.Maximum = Document.VisibleRows.Count + 1; //+this.View.VisibleRowCount-2;// - View.VisibleRowCount  ;
				vScroll.LargeChange = this.View.VisibleRowCount;
				SetMaxHorizontalScroll();

			}
			else
				vScroll.Maximum = 1;
		}


		/// <summary>
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new Container();
			ResourceManager resources = new ResourceManager(typeof (EditViewControl));
			this.Filler = new PictureBox();
			this.CaretTimer = new WeakTimer(this.components);
			this.tooltip = new ToolTip(this.components);

			this.SuspendLayout();

			if (!_CodeEditor.DisableIntelliMouse)
			{
				this.IntelliMouse = new IntelliMouseControl();
				// 
				// IntelliMouse
				// 
				this.IntelliMouse.BackgroundImage = ((Bitmap) (resources.GetObject("IntelliMouse.BackgroundImage")));
				this.IntelliMouse.Image = ((Bitmap) (resources.GetObject("IntelliMouse.Image")));
				this.IntelliMouse.Location = new Point(197, 157);
				this.IntelliMouse.Name = "IntelliMouse";
				this.IntelliMouse.Size = new Size(28, 28);
				this.IntelliMouse.TabIndex = 4;
				this.IntelliMouse.TransparencyKey = Color.FromArgb(((Byte) (255)), ((Byte) (0)), ((Byte) (255)));
				this.IntelliMouse.Visible = false;
				this.IntelliMouse.EndScroll += new EventHandler(this.IntelliMouse_EndScroll);
				this.IntelliMouse.BeginScroll += new EventHandler(this.IntelliMouse_BeginScroll);
				this.IntelliMouse.Scroll += new ScrollEventHandler(this.IntelliMouse_Scroll);

			}


			// 
			// hScroll
			// 
			this.hScroll.Cursor = Cursors.Default;
			this.hScroll.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScroll_Scroll);
			// 
			// vScroll
			// 
			this.vScroll.Cursor = Cursors.Default;
			this.vScroll.Scroll += new System.Windows.Forms.ScrollEventHandler(this.vScroll_Scroll);

			// 
			// CaretTimer
			// 
			this.CaretTimer.Enabled = true;
			this.CaretTimer.Interval = 500;
			this.CaretTimer.Tick += new EventHandler(this.CaretTimer_Tick);
			// 
			// tooltip
			// 
			this.tooltip.AutoPopDelay = 50000;
			this.tooltip.InitialDelay = 0;
			this.tooltip.ReshowDelay = 1000;
			this.tooltip.ShowAlways = true;
			// 
			// TopThumb
			// 
			this.TopThumb.BackColor = SystemColors.Control;
			this.TopThumb.Cursor = Cursors.HSplit;
			this.TopThumb.Location = new Point(101, 17);
			this.TopThumb.Name = "TopThumb";
			this.TopThumb.Size = new Size(16, 8);
			this.TopThumb.TabIndex = 3;
			this.TopThumb.Visible = false;
			// 
			// LeftThumb
			// 
			this.LeftThumb.BackColor = SystemColors.Control;
			this.LeftThumb.Cursor = Cursors.VSplit;
			this.LeftThumb.Location = new Point(423, 17);
			this.LeftThumb.Name = "LeftThumb";
			this.LeftThumb.Size = new Size(8, 16);
			this.LeftThumb.TabIndex = 3;
			this.LeftThumb.Visible = false;
			// 
			// EditViewControl
			// 
			this.AllowDrop = true;
			this.Controls.AddRange(new Control[]
				{
					this.IntelliMouse
				});
			this.Size = new Size(240, 216);
			this.LostFocus += new EventHandler(this.EditViewControl_Leave);
			this.GotFocus += new EventHandler(this.EditViewControl_Enter);
			this.ResumeLayout(false);


		}


		public void InsertAutolistText()
		{
			TextRange tr = new TextRange();
			tr.FirstRow = Caret.Position.Y;
			tr.LastRow = Caret.Position.Y;
			tr.FirstColumn = AutoListStartPos.X;
			tr.LastColumn = Caret.Position.X;

			Document.DeleteRange(tr, true);
			Caret.Position.X = AutoListStartPos.X;
			this.InsertText(AutoList.SelectedText);
			SetFocus();
		}

		private void MoveCaretToNextWord(bool Select)
		{
			int x = Caret.Position.X;
			int y = Caret.Position.Y;
			string StartChar = "";
			int StartType = 0;
			bool found = false;
			if (x == Caret.CurrentRow.Text.Length)
			{
				StartType = 1;
			}
			else
			{
				StartChar = Document[y].Text.Substring(Caret.Position.X, 1);
				StartType = CharType(StartChar);
			}


			while (y < Document.Count)
			{
				while (x < Document[y].Text.Length)
				{
					string Char = Document[y].Text.Substring(x, 1);
					int Type = CharType(Char);
					if (Type != StartType)
					{
						if (Type == 1)
						{
							StartType = 1;
						}
						else
						{
							found = true;
							break;
						}
					}
					x++;
				}
				if (found)
					break;
				x = 0;
				y++;
			}

			if (y >= this.Document.Count - 1)
			{
				y = this.Document.Count - 1;

				if (x >= this.Document[y].Text.Length)
					x = this.Document[y].Text.Length - 1;

				if (x == -1)
					x = 0;
			}

			Caret.SetPos(new TextPoint(x, y));
			if (!Select)
				Selection.ClearSelection();
			if (Select)
			{
				Selection.MakeSelection();
			}


			ScrollIntoView();


		}

		public void InitGraphics()
		{
			this.Painter.InitGraphics();
		}


		private void MoveCaretToPrevWord(bool Select)
		{
			int x = Caret.Position.X;
			int y = Caret.Position.Y;
			string StartChar = "";
			int StartType = 0;
			bool found = false;
			if (x == Caret.CurrentRow.Text.Length)
			{
				StartType = 1;
				x = Caret.CurrentRow.Text.Length - 1;
			}
			else
			{
				StartChar = Document[y].Text.Substring(Caret.Position.X, 1);
				StartType = CharType(StartChar);
			}


			while (y >= 0)
			{
				while (x >= 0 && x < Document[y].Text.Length)
				{
					string Char = Document[y].Text.Substring(x, 1);
					int Type = CharType(Char);
					if (Type != StartType)
					{
						found = true;

						string Char2 = Document[y].Text.Substring(x, 1);
						int Type2 = Type;
						while (x > 0)
						{
							Char2 = Document[y].Text.Substring(x, 1);
							Type2 = CharType(Char2);
							if (Type2 != Type)
							{
								x++;
								break;
							}

							x--;
						}

						break;

					}
					x--;
				}
				if (found)
					break;

				if (y == 0)
				{
					x = 0;
					break;
				}
				y--;
				x = Document[y].Text.Length - 1;
			}


			Caret.SetPos(new TextPoint(x, y));
			if (!Select)
				Selection.ClearSelection();

			if (Select)
			{
				Selection.MakeSelection();
			}

			ScrollIntoView();


		}


		private void SetFocus()
		{
			this.Focus();
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Displays the GotoLine dialog.
		/// </summary>
		public void ShowGotoLine()
		{
			GotoLineForm go = new GotoLineForm(this, this.Document.Count);
//			if (this.TopLevelControl is Form)
//				go.Owner=(Form)this.TopLevelControl;

			go.ShowDialog(this.TopLevelControl);
		}

		/// <summary>
		/// -
		/// </summary>
		public void ShowSettings()
		{
			//	SettingsForm se=new SettingsForm (this);
			//	se.ShowDialog();
		}

		/// <summary>
		/// Places the caret on a specified line and scrolls the caret into view.
		/// </summary>
		/// <param name="RowIndex">the zero based index of the line to jump to</param>
		public void GotoLine(int RowIndex)
		{
			if (RowIndex >= this.Document.Count)
				RowIndex = this.Document.Count - 1;

			if (RowIndex < 0)
				RowIndex = 0;

			this.Caret.Position.Y = RowIndex;
			this.Caret.Position.X = 0;
			this.Caret.CurrentRow.EnsureVisible();
			ClearSelection();
			ScrollIntoView();
			this.Redraw();
		}


		/// <summary>
		/// Clears the active selection.
		/// </summary>
		public void ClearSelection()
		{
			this.Selection.ClearSelection();
			this.Redraw();
		}

		/// <summary>
		/// Returns if a specified pixel position is over the current selection.
		/// </summary>
		/// <param name="x">X Position in pixels</param>
		/// <param name="y">Y Position in pixels</param>
		/// <returns>true if over selection othewise false</returns>
		public bool IsOverSelection(int x, int y)
		{
			TextPoint p = Painter.CharFromPixel(x, y);

			if (p.Y >= this.Selection.LogicalBounds.FirstRow && p.Y <= this.Selection.LogicalBounds.LastRow && this.Selection.IsValid)
			{
				if (p.Y > this.Selection.LogicalBounds.FirstRow && p.Y < this.Selection.LogicalBounds.LastRow && this.Selection.IsValid)
					return true;
				else
				{
					if (p.Y == this.Selection.LogicalBounds.FirstRow && this.Selection.LogicalBounds.FirstRow == this.Selection.LogicalBounds.LastRow)
					{
						if (p.X >= this.Selection.LogicalBounds.FirstColumn && p.X <= this.Selection.LogicalBounds.LastColumn)
							return true;
						else
							return false;
					}
					else if (p.X >= this.Selection.LogicalBounds.FirstColumn && p.Y == this.Selection.LogicalBounds.FirstRow)
						return true;
					else if (p.X <= this.Selection.LogicalBounds.LastColumn && p.Y == this.Selection.LogicalBounds.LastRow)
						return true;
					else
						return false;
				}
			}
			else
				return false; //no chance we are over Selection.LogicalBounds
		}

		/// <summary>
		/// Scrolls a given position in the text into view.
		/// </summary>
		/// <param name="Pos">Position in text</param>
		public void ScrollIntoView(TextPoint Pos)
		{
			TextPoint tmp = this.Caret.Position;
			this.Caret.Position = Pos;
			this.Caret.CurrentRow.EnsureVisible();
			this.ScrollIntoView();
			this.Caret.Position = tmp;
			this.Invalidate();
		}

		public void ScrollIntoView(int RowIndex)
		{
			Row r = Document[RowIndex];
			r.EnsureVisible();
			this.vScroll.Value = r.VisibleIndex;
			this.Invalidate();
		}

		/// <summary>
		/// Scrolls the caret into view.
		/// </summary>
		public void ScrollIntoView()
		{
			InitScrollbars();

			Caret.CropPosition();
			try
			{
				Row xtr2 = Caret.CurrentRow;
				if (xtr2.VisibleIndex >= View.FirstVisibleRow + View.VisibleRowCount - 2)
				{
					int Diff = Caret.CurrentRow.VisibleIndex - (View.FirstVisibleRow + View.VisibleRowCount - 2) + View.FirstVisibleRow;
					if (Diff > Document.VisibleRows.Count - 1)
						Diff = Document.VisibleRows.Count - 1;

					Row r = Document.VisibleRows[Diff];
					int index = r.VisibleIndex;
					if (index != -1)
						vScroll.Value = index;
				}
			}
			catch
			{
			}


			if (Caret.CurrentRow.VisibleIndex < View.FirstVisibleRow)
			{
				Row r = Caret.CurrentRow;
				int index = r.VisibleIndex;
				if (index != -1)
					vScroll.Value = index;
			}

			Row xtr = Caret.CurrentRow;


			int x = 0;
			if (Caret.CurrentRow.IsCollapsedEndPart)
			{
				x = Painter.MeasureRow(xtr, Caret.Position.X).Width + Caret.CurrentRow.Expansion_PixelStart;
				x -= Painter.MeasureRow(xtr, xtr.Expansion_StartChar).Width;

				if (x >= View.ClientAreaWidth + View.ClientAreaStart)
					hScroll.Value = Math.Min(hScroll.Maximum, ((x - View.ClientAreaWidth)/View.CharWidth) + 15);

				if (x < View.ClientAreaStart + 10)
					hScroll.Value = Math.Max(hScroll.Minimum, ((x)/View.CharWidth) - 15);
			}
			else
			{
				x = Painter.MeasureRow(xtr, Caret.Position.X).Width;

				if (x >= View.ClientAreaWidth + View.ClientAreaStart)
					hScroll.Value = Math.Min(hScroll.Maximum, ((x - View.ClientAreaWidth)/View.CharWidth) + 15);

				if (x < View.ClientAreaStart)
					hScroll.Value = Math.Max(hScroll.Minimum, ((x)/View.CharWidth) - 15);
			}
		}

		/// <summary>
		/// Moves the caret to the next line that has a bookmark.
		/// </summary>
		public void GotoNextBookmark()
		{
			int index = 0;
			index = Document.GetNextBookmark(Caret.Position.Y);
			Caret.SetPos(new TextPoint(0, index));
			ScrollIntoView();
			this.Redraw();
		}


		/// <summary>
		/// Moves the caret to the previous line that has a bookmark.
		/// </summary>
		public void GotoPreviousBookmark()
		{
			int index = 0;
			index = Document.GetPreviousBookmark(Caret.Position.Y);
			Caret.SetPos(new TextPoint(0, index));
			ScrollIntoView();
			this.Redraw();
		}

		/// <summary>
		/// Selects next occurance of the given pattern.
		/// </summary>
		/// <param name="Pattern">Pattern to find</param>
		/// <param name="MatchCase">Case sensitive</param>
		/// <param name="WholeWords">Match whole words only</param>
		public bool SelectNext(string Pattern, bool MatchCase, bool WholeWords, bool UseRegEx)
		{
			string pattern = Pattern;
			for (int i = this.Caret.Position.Y; i < this.Document.Count; i++)
			{
				Row r = Document[i];
				int Col = -1;

				string s = "";
				string t = r.Text;
				if (WholeWords)
				{
					s = " " + r.Text + " ";
					t = "";
					pattern = " " + Pattern + " ";
					foreach (char c in s)
					{
						if (".,+-*^\\/()[]{}@:;'?£$#%& \t=<>".IndexOf(c) >= 0)
							t += " ";
						else
							t += c;
					}
				}

				if (!MatchCase)
				{
					t = t.ToLower();
					pattern = pattern.ToLower();
				}

				Col = t.IndexOf(pattern);

				int StartCol = this.Caret.Position.X;
				int StartRow = this.Caret.Position.Y;
				if ((Col >= StartCol) || (i > StartRow && Col >= 0))
				{
					SelectPattern(i, Col, Pattern.Length);
					return true;
				}
			}
			return false;
		}

		public bool ReplaceSelection(string text)
		{
			if (!Selection.IsValid)
				return false;

			int x = Selection.LogicalBounds.FirstColumn;
			int y = Selection.LogicalBounds.FirstRow;

			this.Selection.DeleteSelection();

			Caret.Position.X = x;
			Caret.Position.Y = y;

			InsertText(text);


			Selection.Bounds.FirstRow = y;
			Selection.Bounds.FirstColumn = x + text.Length;

			Selection.Bounds.LastRow = y;
			Selection.Bounds.LastColumn = x + text.Length;

			Caret.Position.X = x + text.Length;
			Caret.Position.Y = y;
			return true;
		}


		/// <summary>
		/// Toggles a bookmark on/off on the active row.
		/// </summary>
		public void ToggleBookmark()
		{
			Document[Caret.Position.Y].Bookmarked = !Document[Caret.Position.Y].Bookmarked;
			this.Redraw();
		}


		/// <summary>
		/// Deletes selected text if possible otherwise deletes forward. (delete key)
		/// </summary>
		public void Delete()
		{
			DeleteForward();
			this.Refresh();
		}

		/// <summary>
		/// Selects all text in the active document. (control + a)
		/// </summary>
		public void SelectAll()
		{
			Selection.SelectAll();
			this.Redraw();
		}


		/// <summary>
		/// Paste text from clipboard to current caret position. (control + v)
		/// </summary>
		public void Paste()
		{
			PasteText();
			this.Refresh();
		}

		/// <summary>
		/// Copies selected text to clipboard. (control + c)
		/// </summary>
		public void Copy()
		{
			CopyText();
		}

		/// <summary>
		/// Cuts selected text to clipboard. (control + x)
		/// </summary>
		public void Cut()
		{
			CopyText();
			Selection.DeleteSelection();
		}

		/// <summary>
		/// Removes the current row
		/// </summary>
		public void RemoveCurrentRow()
		{
			if (Caret.CurrentRow != null && this.Document.Count > 1)
			{
				this.Document.Remove(Caret.CurrentRow.Index, true);
				this.Document.ResetVisibleRows();
				this.Caret.CropPosition();
				this.Caret.CurrentRow.Text = this.Caret.CurrentRow.Text;
				this.Caret.CurrentRow.Parse(true);
				this.Document.ResetVisibleRows();
				this.ScrollIntoView();
				//this.Refresh ();

			}
		}

		public void CutClear()
		{
			if (Selection.IsValid)
				Cut();
			else
				RemoveCurrentRow();
		}

		/// <summary>
		/// Redo last undo action. (control + y)
		/// </summary>
		public void Redo()
		{
			TextPoint p = this.Document.Redo();
			if (p.X != -1 && p.Y != -1)
			{
				Caret.Position = p;
				Selection.ClearSelection();
				ScrollIntoView();
			}
		}

		/// <summary>
		/// Undo last edit action. (control + z)
		/// </summary>
		public void Undo()
		{
			TextPoint p = this.Document.Undo();
			if (p.X != -1 && p.Y != -1)
			{
				Caret.Position = p;
				Selection.ClearSelection();
				ScrollIntoView();
			}
		}

		/// <summary>
		/// Returns a point where x is the column and y is the row from a given pixel position.
		/// </summary>
		/// <param name="x">X Position in pixels</param>
		/// <param name="y">Y Position in pixels</param>
		/// <returns>Column and Rowindex</returns>
		public TextPoint CharFromPixel(int x, int y)
		{
			return this.Painter.CharFromPixel(x, y);
		}

		public void ShowFind()
		{
			if (FindReplaceDialog != null)
			{
				FindReplaceDialog.TopLevel = true;
				if (this.TopLevelControl is Form)
				{
					FindReplaceDialog.Owner = (Form) this.TopLevelControl;
				}
				FindReplaceDialog.ShowFind();
			}
		}

		public void ShowReplace()
		{
			if (FindReplaceDialog != null)
			{
				FindReplaceDialog.TopLevel = true;
				if (this.TopLevelControl is Form)
				{
					FindReplaceDialog.Owner = (Form) this.TopLevelControl;
				}
				FindReplaceDialog.ShowReplace();
			}
		}

		public void AutoListBeginLoad()
		{
			this.AutoList.BeginLoad();
		}

		public void AutoListEndLoad()
		{
			this.AutoList.EndLoad();
		}

		public void FindNext()
		{
			this.FindReplaceDialog.FindNext();
		}

		#endregion

		#region Public Properties

		/// <summary>
		/// Returns true if the control is in overwrite mode.
		/// </summary>
		[Browsable(false)]
		public bool OverWrite
		{
			get { return _OverWrite; }
		}

		/// <summary>
		/// Returns True if the control contains a selected text.
		/// </summary>
		[Browsable(false)]
		public bool CanCopy
		{
			get { return this.Selection.IsValid; }
		}

		/// <summary>
		/// Returns true if there is any valid text data inside the Clipboard.
		/// </summary>
		[Browsable(false)]
		public bool CanPaste
		{
			get
			{
				string s = "";
				try
				{
					IDataObject iData = Clipboard.GetDataObject();

					if (iData.GetDataPresent(DataFormats.Text))
					{
						// Yes it is, so display it in a text box.
						s = (String) iData.GetData(DataFormats.Text);
					}

					if (s != null)
						return true;
				}
				catch
				{
				}
				return false;
			}
		}

		/// <summary>
		/// Returns true if the undobuffer contains one or more undo actions.
		/// </summary>
		[Browsable(false)]
		public bool CanUndo
		{
			get { return (this.Document.UndoStep > 0); }
		}

		/// <summary>
		/// Returns true if the control can redo the last undo action/s
		/// </summary>
		[Browsable(false)]
		public bool CanRedo
		{
			get { return (this.Document.UndoStep < this.Document.UndoBuffer.Count - 1); }
		}


		/// <summary>
		/// Gets the size (in pixels) of the font to use when rendering the the content.
		/// The value is retrived from the owning Syntaxbox control.
		/// </summary>
		public float FontSize
		{
			get { return _CodeEditor.FontSize; }

		}


		/// <summary>
		/// Gets the indention style to use when inserting new lines.
		/// The value is retrived from the owning Syntaxbox control.
		/// </summary>
		public IndentStyle Indent
		{
			get { return _CodeEditor.Indent; }

		}

		/// <summary>
		/// Gets the SyntaxDocument the control is currently attatched to.
		/// The value is retrived from the owning Syntaxbox control.
		/// </summary>
		[Category("Content")]
		[Description("The SyntaxDocument that is attatched to the contro")]
		public SyntaxDocument Document
		{
			get { return _CodeEditor.Document; }

		}

		/// <summary>
		/// Gets the delay in MS before the tooltip is displayed when hovering a collapsed section.
		/// The value is retrived from the owning Syntaxbox control.
		/// </summary>
		public int TooltipDelay
		{
            get
            {
                return _CodeEditor.TooltipDelay;
            }
		}


		/// <summary>
		/// Gets if the control is readonly.
		/// The value is retrived from the owning Syntaxbox control.
		/// </summary>
		public bool ReadOnly
		{
			get { return _CodeEditor.ReadOnly; }
		}


		/// <summary>
		/// Gets the name of the font to use when rendering the control.
		/// The value is retrived from the owning Syntaxbox control.
		/// </summary>
		public string FontName
		{
			get { return _CodeEditor.FontName; }

		}

		/// <summary>
		/// Determines the style to use when painting with alt+arrow keys.
		/// </summary>
		[Category("Behavior")]
		[Description("Determines what type of chars to use when painting with ALT+arrow keys")]
		public TextDrawType TextDrawStyle
		{
			get { return _TextDrawStyle; }
			set { _TextDrawStyle = value; }
		}

		/// <summary>
		/// Gets if the control should render bracket matching.
		/// The value is retrived from the owning Syntaxbox control.
		/// </summary>
		public bool BracketMatching
		{
            get
            {
                return _CodeEditor.BracketMatching;
            }
		}


		/// <summary>
		/// Gets if the control should render whitespace chars.
		/// The value is retrived from the owning Syntaxbox control.
		/// </summary>
		public bool VirtualWhitespace
		{
			get { return _CodeEditor.VirtualWhitespace; }
		}


		/// <summary>
		/// Gets the Color of the horizontal separators (a'la visual basic 6).
		/// The value is retrived from the owning Syntaxbox control.
		/// </summary>
		public Color SeparatorColor
		{
			get { return _CodeEditor.SeparatorColor; }
		}


		/// <summary>
		/// Gets the text color to use when rendering bracket matching.
		/// The value is retrived from the owning Syntaxbox control.
		/// </summary>
		public Color BracketForeColor
		{
			get { return _CodeEditor.BracketForeColor; }
		}


		/// <summary>
		/// Gets the back color to use when rendering bracket matching.
		/// The value is retrived from the owning Syntaxbox control.
		/// </summary>
		public Color BracketBackColor
		{
			get { return _CodeEditor.BracketBackColor; }
		}


		/// <summary>
		/// Gets the back color to use when rendering the selected text.
		/// The value is retrived from the owning Syntaxbox control.
		/// </summary>
		public Color SelectionBackColor
		{
			get { return _CodeEditor.SelectionBackColor; }
		}


		/// <summary>
		/// Gets the text color to use when rendering the selected text.
		/// The value is retrived from the owning Syntaxbox control.
		/// </summary>
		public Color SelectionForeColor
		{
			get { return _CodeEditor.SelectionForeColor; }

		}

		/// <summary>
		/// Gets the back color to use when rendering the inactive selected text.
		/// The value is retrived from the owning Syntaxbox control.
		/// </summary>
		public Color InactiveSelectionBackColor
		{
			get { return _CodeEditor.InactiveSelectionBackColor; }
		}


		/// <summary>
		/// Gets the text color to use when rendering the inactive selected text.
		/// The value is retrived from the owning Syntaxbox control.
		/// </summary>
		public Color InactiveSelectionForeColor
		{
			get { return _CodeEditor.InactiveSelectionForeColor; }

		}


		/// <summary>
		/// Gets the color of the border between the gutter area and the line number area.
		/// The value is retrived from the owning Syntaxbox control.
		/// </summary>
		public Color GutterMarginBorderColor
		{
			get { return _CodeEditor.GutterMarginBorderColor; }

		}


		/// <summary>
		/// Gets the color of the border between the line number area and the folding area.
		/// The value is retrived from the owning Syntaxbox control.
		/// </summary>
		public Color LineNumberBorderColor
		{
			get { return _CodeEditor.LineNumberBorderColor; }

		}


		/// <summary>
		/// Gets the text color to use when rendering breakpoints.
		/// The value is retrived from the owning Syntaxbox control.
		/// </summary>
		public Color BreakPointForeColor
		{
			get { return _CodeEditor.BreakPointForeColor; }

		}

		/// <summary>
		/// Gets the back color to use when rendering breakpoints.
		/// The value is retrived from the owning Syntaxbox control.
		/// </summary>
		public Color BreakPointBackColor
		{
			get { return _CodeEditor.BreakPointBackColor; }

		}

		/// <summary>
		/// Gets the text color to use when rendering line numbers.
		/// The value is retrived from the owning Syntaxbox control.
		/// </summary>
		public Color LineNumberForeColor
		{
			get { return _CodeEditor.LineNumberForeColor; }

		}

		/// <summary>
		/// Gets the back color to use when rendering line number area.
		/// </summary>
		public Color LineNumberBackColor
		{
			get { return _CodeEditor.LineNumberBackColor; }

		}

		/// <summary>
		/// Gets the color of the gutter margin.
		/// The value is retrived from the owning Syntaxbox control.
		/// </summary>
		public Color GutterMarginColor
		{
			get { return _CodeEditor.GutterMarginColor; }
		}

		/// <summary>
		/// Gets or Sets the background Color of the client area.
		/// </summary>
		[Category("Appearance")]
		[Description("The background color of the client area")]
		new public Color BackColor
		{
			get { return _CodeEditor.BackColor; }
			set { _CodeEditor.BackColor = value; }
		}

		/// <summary>
		/// Gets the back color to use when rendering the active line.
		/// </summary>
		public Color HighLightedLineColor
		{
			get { return _CodeEditor.HighLightedLineColor; }
		}

		/// <summary>
		/// Get if the control should highlight the active line.
		/// The value is retrived from the owning Syntaxbox control.
		/// </summary>
		public bool HighLightActiveLine
		{
			get { return _CodeEditor.HighLightActiveLine; }
		}

		/// <summary>
		/// Get if the control should render whitespace chars.
		/// The value is retrived from the owning Syntaxbox control.
		/// </summary>
		public bool ShowWhitespace
		{
			get { return _CodeEditor.ShowWhitespace; }
		}


		/// <summary>
		/// Get if the line number margin is visible or not.
		/// The value is retrived from the owning Syntaxbox control.
		/// </summary>
		public bool ShowLineNumbers
		{
			get { return _CodeEditor.ShowLineNumbers; }
		}


		/// <summary>
		/// Get if the gutter margin is visible or not.
		/// The value is retrived from the owning Syntaxbox control.
		/// </summary>
		public bool ShowGutterMargin
		{
			get { return _CodeEditor.ShowGutterMargin; }
		}

		/// <summary>
		/// Get the Width of the gutter margin (in pixels)
		/// The value is retrived from the owning Syntaxbox control.
		/// </summary>
		public int GutterMarginWidth
		{
			get { return _CodeEditor.GutterMarginWidth; }
		}


		/// <summary>
		/// Get the numbers of space chars in a tab.
		/// The value is retrived from the owning Syntaxbox control.
		/// </summary>
		public int TabSize
		{
			get { return _CodeEditor.TabSize; }
		}

		/// <summary>
		/// Get if the control should render 'Tab guides'
		/// The value is retrived from the owning Syntaxbox control.
		/// </summary>
		public bool ShowTabGuides
		{
			get { return _CodeEditor.ShowTabGuides; }
		}

		/// <summary>
		/// Gets the color to use when rendering whitespace chars.
		/// The value is retrived from the owning Syntaxbox control.
		/// </summary>
		public Color WhitespaceColor
		{
			get { return _CodeEditor.WhitespaceColor; }
		}

		/// <summary>
		/// Gets the color to use when rendering tab guides.
		/// The value is retrived from the owning Syntaxbox control.
		/// </summary>
		public Color TabGuideColor
		{
			get { return _CodeEditor.TabGuideColor; }
		}

		/// <summary>
		/// Get the color to use when rendering bracket matching borders.
		/// The value is retrived from the owning Syntaxbox control.
		/// </summary>
		/// <remarks>
		/// NOTE: Use the Color.Transparent turn off the bracket match borders.
		/// </remarks>
		public Color BracketBorderColor
		{
			get { return _CodeEditor.BracketBorderColor; }
		}


		/// <summary>
		/// Get the color to use when rendering Outline symbols.
		/// The value is retrived from the owning Syntaxbox control.
		/// </summary>
		public Color OutlineColor
		{
			get { return _CodeEditor.OutlineColor; }

		}


		/// <summary>
		/// Positions the AutoList
		/// </summary>
		[Category("Behavior")]
		public TextPoint AutoListPosition
		{
			get { return AutoListStartPos; }
			set { AutoListStartPos = value; }
		}

		/// <summary>
		/// Positions the InfoTip
		/// </summary>
		[Category("Behavior")]
		public TextPoint InfoTipPosition
		{
			get { return InfoTipStartPos; }
			set { InfoTipStartPos = value; }
		}


		/// <summary>
		/// Gets or Sets if the intellisense list is visible.
		/// </summary>
		[Category("Behavior")]
		public bool AutoListVisible
		{
			set
			{
				CreateAutoList();
				if (this.AutoList == null)
					return;

				if (value)
				{
					AutoList.TopLevel = true;
					AutoList.BringToFront();
					InfoTip.BringToFront();
					if (this.TopLevelControl is Form)
					{
						AutoList.Owner = (Form) this.TopLevelControl;
					}
				}

				_AutoListVisible = value;
				InfoTip.BringToFront();

				this.Redraw();

			}
			get { return _AutoListVisible; }
		}

		/// <summary>
		/// Gets or Sets if the infotip is visible
		/// </summary>
		[Category("Behavior")]
		public bool InfoTipVisible
		{
			set
			{
				CreateInfoTip();
				if (this.InfoTip == null)
					return;

				if (value)
				{
					//	InfoTipStartPos=new TextPoint (Caret.Position.X,Caret.Position.Y);
					InfoTip.TopLevel = true;
					AutoList.BringToFront();
					if (this.TopLevelControl is Form)
					{
						InfoTip.Owner = (Form) this.TopLevelControl;
					}
				}
				//else
				//	InfoTipStartPos=new TextPoint (0,0);

				InfoTip.BringToFront();

				_InfoTipVisible = value;
				if (this.InfoTip != null && value == true)
				{
					this.InfoTip.Init();
				}
				this.Redraw();
			}
			get { return _InfoTipVisible; }
		}

		/// <summary>
		/// Get if the control should use smooth scrolling.
		/// The value is retrived from the owning Syntaxbox control.
		/// </summary>
		public bool SmoothScroll
		{
			get { return _CodeEditor.SmoothScroll; }
		}

		/// <summary>
		/// Get the number of pixels the screen should be scrolled per frame when using smooth scrolling.
		/// The value is retrived from the owning Syntaxbox control.
		/// </summary>
		public int SmoothScrollSpeed
		{
			get { return _CodeEditor.SmoothScrollSpeed; }
		}


		/// <summary>
		/// Get if the control should parse all text when text is pasted from the clipboard.
		/// The value is retrived from the owning Syntaxbox control.
		/// </summary>
		public bool ParseOnPaste
		{
			get { return _CodeEditor.ParseOnPaste; }
		}


		/// <summary>
		/// Gets the Caret object.
		/// </summary>
		public Caret Caret
		{
			get { return _Caret; }
		}

		/// <summary>
		/// Gets the Selection object.
		/// </summary>
		public Selection Selection
		{
			get { return _Selection; }
		}

		#endregion

		#region eventhandlers

		private void OnClipboardUpdated(CopyEventArgs e)
		{
			if (ClipboardUpdated != null)
				ClipboardUpdated(this, e);
		}


		private void OnRowMouseDown(RowMouseEventArgs e)
		{
			if (RowMouseDown != null)
				RowMouseDown(this, e);
		}

		private void OnRowMouseMove(RowMouseEventArgs e)
		{
			if (RowMouseMove != null)
				RowMouseMove(this, e);
		}

		private void OnRowMouseUp(RowMouseEventArgs e)
		{
			if (RowMouseUp != null)
				RowMouseUp(this, e);
		}

		private void OnRowClick(RowMouseEventArgs e)
		{
			if (RowClick != null)
				RowClick(this, e);
		}

		private void OnRowDoubleClick(RowMouseEventArgs e)
		{
			if (RowDoubleClick != null)
				RowDoubleClick(this, e);
		}


		protected override void OnLoad(EventArgs e)
		{
			this.DoResize();
			this.Refresh();
		}


		public void OnParse()
		{
			this.Redraw();
		}

		public void OnChange()
		{
			if (this.Caret.Position.Y > this.Document.Count - 1)
			{
				this.Caret.Position.Y = this.Document.Count - 1;
				//this.Caret.MoveAbsoluteHome (false);
				ScrollIntoView();
			}

			try
			{
				if (this.VirtualWhitespace == false && Caret.CurrentRow != null && Caret.Position.X > Caret.CurrentRow.Text.Length)
				{
					Caret.Position.X = Caret.CurrentRow.Text.Length;
					this.Redraw();
				}
			}
			catch
			{
			}


			if (!this.ContainsFocus)
			{
				this.Selection.ClearSelection();
			}


			if (this.Selection.LogicalBounds.FirstRow > this.Document.Count)
			{
				this.Selection.Bounds.FirstColumn = Caret.Position.X;
				this.Selection.Bounds.LastColumn = Caret.Position.X;
				this.Selection.Bounds.FirstRow = Caret.Position.Y;
				this.Selection.Bounds.LastRow = Caret.Position.Y;
			}

			if (this.Selection.LogicalBounds.LastRow > this.Document.Count)
			{
				this.Selection.Bounds.FirstColumn = Caret.Position.X;
				this.Selection.Bounds.LastColumn = Caret.Position.X;
				this.Selection.Bounds.FirstRow = Caret.Position.Y;
				this.Selection.Bounds.LastRow = Caret.Position.Y;
			}

			this.Redraw();


		}

		/// <summary>
		/// Overrides the default OnKeyDown
		/// </summary>
		/// <param name="e"></param>
		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);
			_KeyDownHandled = e.Handled;

			if (e.KeyCode == Keys.Escape && (InfoTipVisible || AutoListVisible))
			{
				InfoTipVisible = false;
				AutoListVisible = false;
				e.Handled = true;
				this.Redraw();
				return;
			}

			if (!e.Handled && InfoTipVisible && InfoTip.Count > 1)
			{
				//move infotip selection
				if (e.KeyCode == Keys.Up)
				{
					this._CodeEditor.InfoTipSelectedIndex ++;
					e.Handled = true;
					return;
				}

				if (e.KeyCode == Keys.Down)
				{
					this._CodeEditor.InfoTipSelectedIndex --;

					e.Handled = true;
					return;
				}


			}

			if (!e.Handled && AutoListVisible)
			{
				//move autolist selection
				if ((e.KeyCode == Keys.Up || e.KeyCode == Keys.Down || e.KeyCode == Keys.PageUp || e.KeyCode == Keys.PageDown))
				{
					AutoList.SendKey((int) e.KeyCode);
					e.Handled = true;
					return;
				}

				//inject inser text from autolist
				if (e.KeyCode == Keys.Space || e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab)
				{
					string s = AutoList.SelectedText;
					if (s != "")
						InsertAutolistText();
					AutoListVisible = false;
					e.Handled = true;
					this.Redraw();
					return;
				}
			}


			if (!e.Handled)
			{
				//do keyboard actions
				foreach (KeyboardAction ka in this._CodeEditor.KeyboardActions)
				{
					if (!this.ReadOnly || ka.AllowReadOnly)
					{
						if ((ka.Key == (Keys) (int) e.KeyCode)
							&& ka.Alt == e.Alt
							&& ka.Shift == e.Shift
							&& ka.Control == e.Control)
							ka.Action(); //if keys match , call action delegate
					}
				}


				//------------------------------------------------------------------------------------------------------------


				switch ((Keys) (int) e.KeyCode)
				{
					case Keys.ShiftKey:
					case Keys.ControlKey:
					case Keys.Alt:
						return;

					case Keys.Down:
						if (e.Control)
							ScrollScreen(1);
						else
						{
							if (e.Alt)
								TextDraw(TextDrawDirectionType.Down);
							Caret.MoveDown(e.Shift);
							this.Redraw();

						}
						break;
					case Keys.Up:
						if (e.Control)
							ScrollScreen(-1);
						else
						{
							if (e.Alt)
								TextDraw(TextDrawDirectionType.Up);
							Caret.MoveUp(e.Shift);
						}
						this.Redraw();
						break;
					case Keys.Left:
						{
							if (e.Control)
							{
								MoveCaretToPrevWord(e.Shift);
							}
							else
							{
								if (e.Alt)
									TextDraw(TextDrawDirectionType.Left);
								Caret.MoveLeft(e.Shift);
							}
						}
						this.Redraw();
						break;
					case Keys.Right:
						{
							if (e.Control)
							{
								MoveCaretToNextWord(e.Shift);
							}
							else
							{
								if (e.Alt)
									TextDraw(TextDrawDirectionType.Right);

								Caret.MoveRight(e.Shift);
							}
						}
						this.Redraw();
						break;
					case Keys.End:
						if (e.Control)
							Caret.MoveAbsoluteEnd(e.Shift);
						else
							Caret.MoveEnd(e.Shift);
						this.Redraw();
						break;
					case Keys.Home:
						if (e.Control)
							Caret.MoveAbsoluteHome(e.Shift);
						else
							Caret.MoveHome(e.Shift);
						this.Redraw();
						break;
					case Keys.PageDown:
						Caret.MoveDown(View.VisibleRowCount - 2, e.Shift);
						this.Redraw();
						break;
					case Keys.PageUp:
						Caret.MoveUp(View.VisibleRowCount - 2, e.Shift);
						this.Redraw();
						break;

					default:
						break;
				}


				//dont do if readonly
				if (!this.ReadOnly)
				{
					switch ((Keys) (int) e.KeyCode)
					{
						case Keys.Enter:
							{
								if (e.Control)
								{
									if (Caret.CurrentRow.CanFold)
									{
										Caret.MoveHome(false);
										Document.ToggleRow(Caret.CurrentRow);
										this.Redraw();
									}
								}
								else
									InsertEnter();
								break;
							}
						case Keys.Back:
							if (!e.Control)
								this.DeleteBackwards();
							else
							{
								if (this.Selection.IsValid)
									this.Selection.DeleteSelection();
								else
								{
									this.Selection.ClearSelection();
									MoveCaretToPrevWord(true);
									this.Selection.DeleteSelection();
								}
								this.Caret.CurrentRow.Parse(true);
							}

							break;
						case Keys.Delete:
							{
								if (!e.Control && !e.Alt && !e.Shift)
								{
									this.Delete();
								}
								else if (e.Control && !e.Alt && !e.Shift)
								{
									if (this.Selection.IsValid)
									{
										Cut();
									}
									else
									{
										this.Selection.ClearSelection();
										MoveCaretToNextWord(true);
										this.Selection.DeleteSelection();
									}
									this.Caret.CurrentRow.Parse(true);
								}
								break;
							}
						case Keys.Insert:
							{
								if (!e.Control && !e.Alt && !e.Shift)
								{
									_OverWrite = !_OverWrite;
								}
								break;
							}
						case Keys.Tab:
							{
								if (!Selection.IsValid)
									InsertText("\t");

								break;
							}
						default:
							{
								break;
							}
					}
				}
				Caret.Blink = true;
				//this.Redraw ();
			}
		}

		/// <summary>
		/// Overrides the default OnKeyPress
		/// </summary>
		/// <param name="e"></param>
		protected override void OnKeyPress(KeyPressEventArgs e)
		{
			base.OnKeyPress(e);


			if (!e.Handled && !_KeyDownHandled && e.KeyChar != (char) 127)
			{
				if (((int) e.KeyChar) < 32)
					return;

				if (!this.ReadOnly)
				{
					switch ((Keys) (int) e.KeyChar)
					{
						default:
							{
								InsertText(e.KeyChar.ToString());

								if (this.Indent == IndentStyle.Scope || this.Indent == IndentStyle.Smart)
								{
									if (Caret.CurrentRow.ShouldOutdent)
									{
										OutdentEndRow();
									}
								}
								break;
							}
					}
				}
			}

			if (AutoListVisible && !e.Handled && _CodeEditor.AutoListAutoSelect)
			{
				string s = Caret.CurrentRow.Text;
				try
				{
					if (Caret.Position.X - AutoListStartPos.X >= 0)
					{
						s = s.Substring(AutoListStartPos.X, Caret.Position.X - AutoListStartPos.X);
						AutoList.SelectItem(s);
					}
				}
				catch
				{
				}
			}
		}

		/// <summary>
		/// Overrides the default OnKeyUp
		/// </summary>
		/// <param name="e"></param>
		protected override void OnKeyUp(KeyEventArgs e)
		{
			base.OnKeyUp(e);
		}

		/// <summary>
		/// Overrides the default OnMouseDown
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseDown(MouseEventArgs e)
		{
           // this.Select();
			MouseX = e.X;
			MouseY = e.Y;
			MouseButton = e.Button;

			SetFocus();
			base.OnMouseDown(e);
			TextPoint pos = Painter.CharFromPixel(e.X, e.Y);
			Row row = null;
			if (pos.Y >= 0 && pos.Y < this.Document.Count)
				row = this.Document[pos.Y];

			#region RowEvent

			RowMouseEventArgs rea = new RowMouseEventArgs();
			rea.Row = row;
			rea.Button = e.Button;
			rea.MouseX = MouseX;
			rea.MouseY = MouseY;
			if (e.X >= this.View.TextMargin - 7)
			{
				rea.Area = RowArea.TextArea;
			}
			else if (e.X < this.View.GutterMarginWidth)
			{
				rea.Area = RowArea.GutterArea;
			}
			else if (e.X < this.View.LineNumberMarginWidth + this.View.GutterMarginWidth)
			{
				rea.Area = RowArea.LineNumberArea;
			}
			else if (e.X < this.View.TextMargin - 7)
			{
				rea.Area = RowArea.FoldingArea;
			}

			this.OnRowMouseDown(rea);

			#endregion

			try
			{
				Row r2 = Document[pos.Y];
				if (r2 != null)
				{
					if (e.X >= r2.Expansion_PixelEnd && r2.IsCollapsed)
					{
						if (r2.Expansion_StartSegment != null)
						{
							if (r2.Expansion_StartSegment.StartRow != null && r2.Expansion_StartSegment.EndRow != null && r2.Expansion_StartSegment.Expanded == false)
							{
								if (!IsOverSelection(e.X, e.Y))
								{
									this.Caret.Position.X = pos.X;
									this.Caret.Position.Y = pos.Y;
									this.Selection.ClearSelection();

									Row r3 = r2.Expansion_EndRow;
									int x3 = r3.Expansion_StartChar;

									this.Caret.Position.X = x3;
									this.Caret.Position.Y = r3.Index;
									this.Selection.MakeSelection();

									this.Redraw();
									View.Action = XTextAction.xtSelect;

									return;

								}
							}
						}
					}
				}
			}
			catch
			{
				//this is untested code...
			}

			bool shift = NativeMethods.IsKeyPressed(Keys.ShiftKey);

			if (e.X > View.TotalMarginWidth)
			{
				if (e.X > View.TextMargin - 8)
				{
					if (!IsOverSelection(e.X, e.Y))
					{
						//selecting
						if (e.Button == MouseButtons.Left)
						{
							if (!shift)
							{
								TextPoint tp = pos;
								Word w = this.Document.GetWordFromPos(tp);
								if (w != null && w.Pattern != null && w.Pattern.Category != null)
								{
									WordMouseEventArgs pe = new WordMouseEventArgs();
									pe.Pattern = w.Pattern;
									pe.Button = e.Button;
									pe.Cursor = Cursors.Hand;
									pe.Word = w;

									this._CodeEditor.OnWordMouseDown(ref pe);

									this.Cursor = pe.Cursor;
									return;
								}

								View.Action = XTextAction.xtSelect;
								Caret.SetPos(pos);
								Selection.ClearSelection();
								Caret.Blink = true;
								this.Redraw();
							}
							else
							{
								View.Action = XTextAction.xtSelect;
								Caret.SetPos(pos);
								Selection.MakeSelection();
								Caret.Blink = true;
								this.Redraw();
							}
						}
					}
				}
				else
				{
					if (row.Expansion_StartSegment != null)
					{
						Caret.SetPos(new TextPoint(0, pos.Y));
						Selection.ClearSelection();
						this.Document.ToggleRow(row);
						this.Redraw();
					}
				}
			}
			else
			{
				if (e.X < View.GutterMarginWidth)
				{
					if (_CodeEditor.AllowBreakPoints)
					{
						Row r = Document[Painter.CharFromPixel(e.X, e.Y).Y];
						r.Breakpoint = !r.Breakpoint;
						this.Redraw();
					}
					else
					{
						Row r = Document[Painter.CharFromPixel(e.X, e.Y).Y];
						r.Breakpoint = false;
						this.Redraw();
					}
				}
				else
				{
					View.Action = XTextAction.xtSelect;
					Caret.SetPos(Painter.CharFromPixel(0, e.Y));
					Selection.ClearSelection();
					Caret.MoveDown(true);

					this.Redraw();
				}
			}
			SetMouseCursor(e.X, e.Y);


		}

		/// <summary>
		/// Overrides the default OnMouseMove
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseMove(MouseEventArgs e)
		{
			MouseX = e.X;
			MouseY = e.Y;
			MouseButton = e.Button;

			TextPoint pos = Painter.CharFromPixel(e.X, e.Y);
			Row row = null;
			if (pos.Y >= 0 && pos.Y < this.Document.Count)
				row = this.Document[pos.Y];

			#region RowEvent

			RowMouseEventArgs rea = new RowMouseEventArgs();
			rea.Row = row;
			rea.Button = e.Button;
			rea.MouseX = MouseX;
			rea.MouseY = MouseY;
			if (e.X >= this.View.TextMargin - 7)
			{
				rea.Area = RowArea.TextArea;
			}
			else if (e.X < this.View.GutterMarginWidth)
			{
				rea.Area = RowArea.GutterArea;
			}
			else if (e.X < this.View.LineNumberMarginWidth + this.View.GutterMarginWidth)
			{
				rea.Area = RowArea.LineNumberArea;
			}
			else if (e.X < this.View.TextMargin - 7)
			{
				rea.Area = RowArea.FoldingArea;
			}

			this.OnRowMouseMove(rea);

			#endregion

			try
			{
				if (Document != null)
				{
					if (e.Button == MouseButtons.Left)
					{
						if (View.Action == XTextAction.xtSelect)
						{
							//Selection ACTIONS!!!!!!!!!!!!!!
							Caret.Blink = true;
							Caret.SetPos(pos);
							if (e.X <= View.TotalMarginWidth)
								Caret.MoveDown(true);
							Caret.CropPosition();
							Selection.MakeSelection();
							ScrollIntoView();
							this.Redraw();

						}
						else if (View.Action == XTextAction.xtNone)
						{
							if (IsOverSelection(e.X, e.Y))
							{
								BeginDragDrop();
							}
						}
						else if (View.Action == XTextAction.xtDragText)
						{
						}
					}
					else
					{
						TextPoint p = pos;
						Row r = Document[p.Y];
						bool DidShow = false;

						if (r != null)
						{
							if (e.X >= r.Expansion_PixelEnd && r.IsCollapsed)
							{
								if (r.Expansion_StartSegment != null)
								{
									if (r.Expansion_StartSegment.StartRow != null && r.Expansion_StartSegment.EndRow != null && r.Expansion_StartSegment.Expanded == false)
									{
										string t = "";
										int j = 0;
										for (int i = r.Expansion_StartSegment.StartRow.Index; i <= r.Expansion_StartSegment.EndRow.Index; i++)
										{
											if (j > 0)
												t += "\n";
											Row tmp = Document[i];
											string tmpstr = tmp.Text.Replace("\t", "    ");
											t += tmpstr;
											if (j > 20)
											{
												t += "...";
												break;
											}

											j++;
										}


										//tooltip.res
										tooltip.InitialDelay = this.TooltipDelay;
										if (tooltip.GetToolTip(this) != t)
											tooltip.SetToolTip(this, t);
										tooltip.Active = true;
										DidShow = true;
									}
								}
							}
							else
							{
								Word w = this.Document.GetFormatWordFromPos(p);
								if (w != null)
								{
									if (w.InfoTip != null)
									{
										tooltip.InitialDelay = this.TooltipDelay;
										if (tooltip.GetToolTip(this) != w.InfoTip)
											tooltip.SetToolTip(this, w.InfoTip);
										tooltip.Active = true;
										DidShow = true;
									}
								}
							}
						}

						if (this.tooltip != null)
						{
							if (!DidShow)
								tooltip.SetToolTip(this, "");
						}
					}

					SetMouseCursor(e.X, e.Y);
					base.OnMouseMove(e);
				}

			}
			catch
			{
			}
		}

		/// <summary>
		/// Overrides the default OnMouseUp
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseUp(MouseEventArgs e)
		{
			MouseX = e.X;
			MouseY = e.Y;
			MouseButton = e.Button;

			TextPoint pos = Painter.CharFromPixel(e.X, e.Y);
			Row row = null;
			if (pos.Y >= 0 && pos.Y < this.Document.Count)
				row = this.Document[pos.Y];

			#region RowEvent

			RowMouseEventArgs rea = new RowMouseEventArgs();
			rea.Row = row;
			rea.Button = e.Button;
			rea.MouseX = MouseX;
			rea.MouseY = MouseY;
			if (e.X >= this.View.TextMargin - 7)
			{
				rea.Area = RowArea.TextArea;
			}
			else if (e.X < this.View.GutterMarginWidth)
			{
				rea.Area = RowArea.GutterArea;
			}
			else if (e.X < this.View.LineNumberMarginWidth + this.View.GutterMarginWidth)
			{
				rea.Area = RowArea.LineNumberArea;
			}
			else if (e.X < this.View.TextMargin - 7)
			{
				rea.Area = RowArea.FoldingArea;
			}

			this.OnRowMouseUp(rea);

			#endregion

			if (View.Action == XTextAction.xtNone)
			{
				if (e.X > View.TotalMarginWidth)
				{
					if (IsOverSelection(e.X, e.Y) && e.Button == MouseButtons.Left)
					{
						View.Action = XTextAction.xtSelect;
						Caret.SetPos(Painter.CharFromPixel(e.X, e.Y));
						Selection.ClearSelection();
						this.Redraw();
					}
				}
			}

			View.Action = XTextAction.xtNone;
			base.OnMouseUp(e);
		}

		/// <summary>
		/// Overrides the default OnMouseWheel
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseWheel(MouseEventArgs e)
		{
			int l = SystemInformation.MouseWheelScrollLines;
			ScrollScreen(-(e.Delta/120)*l, 2);

			base.OnMouseWheel(e);
		}

		/// <summary>
		/// Overrides the default OnPaint
		/// </summary>
		/// <param name="e"></param>
		protected override void OnPaint(PaintEventArgs e)
		{
			if (Document != null && this.Width > 0 && this.Height > 0)
			{
				Painter.RenderAll(e.Graphics);
			}
		}

		/// <summary>
		/// Overrides the default OnHandleCreated
		/// </summary>
		/// <param name="e"></param>
		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);
		}


		private int OldWidth = 0;

		/// <summary>
		/// Overrides the default OnResize
		/// </summary>
		/// <param name="e"></param>
		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			DoResize();
		}

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
        }

		/// <summary>
		/// Overrides the default OnDragOver
		/// </summary>
		/// <param name="drgevent"></param>
		protected override void OnDragOver(DragEventArgs drgevent)
		{
			if (!this.ReadOnly)
			{
				if (Document != null)
				{
					View.Action = XTextAction.xtDragText;

					Point pt = this.PointToClient(new Point(drgevent.X, drgevent.Y));

					int x = pt.X;
					int y = pt.Y;

					//	drgevent.Effect = DragDropEffects.All  ;
					//Caret.Position = Painter.CharFromPixel(x,y);

					if ((drgevent.KeyState & 8) == 8)
					{
						drgevent.Effect = DragDropEffects.Copy;
					}
					else
					{
						drgevent.Effect = DragDropEffects.Move;
					}
					Caret.SetPos(Painter.CharFromPixel(x, y));
					this.Redraw();
				}
			}
			else
			{
				drgevent.Effect = DragDropEffects.None;
			}
		}

		/// <summary>
		/// Overrides the default OnDragDrop
		/// </summary>
		/// <param name="drgevent"></param>
		protected override void OnDragDrop(DragEventArgs drgevent)
		{
			if (!this.ReadOnly)
			{
				if (Document != null)
				{
					View.Action = XTextAction.xtNone;
					int SelStart = Selection.LogicalSelStart;
					int DropStart = Document.PointToIntPos(Caret.Position);


					string s = drgevent.Data.GetData(typeof (string)).ToString();
					//int SelLen=s.Replace ("\r\n","\n").Length ;
					int SelLen = s.Length;


					if (DropStart >= SelStart && DropStart <= SelStart + Math.Abs(Selection.SelLength))
						DropStart = SelStart;
					else if (DropStart >= SelStart + SelLen)
						DropStart -= SelLen;


					this.Document.StartUndoCapture();
					if ((drgevent.KeyState & 8) == 0)
					{
						this._CodeEditor.Selection.DeleteSelection();
						this.Caret.Position = Document.IntPosToPoint(DropStart);
					}

					TextPoint p = Document.InsertText(s, Caret.Position.X, Caret.Position.Y);
					this.Document.EndUndoCapture();

					Selection.SelStart = Document.PointToIntPos(Caret.Position);
					Selection.SelLength = SelLen;
					Document.ResetVisibleRows();
					ScrollIntoView();
					this.Redraw();
					drgevent.Effect = DragDropEffects.All;

					if (ParseOnPaste)
						this.Document.ParseAll(true);

					View.Action = XTextAction.xtNone;
				}
			}
		}


		/// <summary>
		///  Overrides the default OnDragEnter
		/// </summary>
		/// <param name="drgevent"></param>
		protected override void OnDragEnter(DragEventArgs drgevent)
		{
		}

		/// <summary>
		///  Overrides the default OnDragLeave
		/// </summary>
		/// <param name="e"></param>
		protected override void OnDragLeave(EventArgs e)
		{
			View.Action = XTextAction.xtNone;
		}

		/// <summary>
		///  Overrides the default OnDoubleClick
		/// </summary>
		/// <param name="e"></param>
		protected override void OnDoubleClick(EventArgs e)
		{
			TextPoint pos = Painter.CharFromPixel(MouseX, MouseY);
			Row row = null;
			if (pos.Y >= 0 && pos.Y < this.Document.Count)
				row = this.Document[pos.Y];

			#region RowEvent

			RowMouseEventArgs rea = new RowMouseEventArgs();
			rea.Row = row;
			rea.Button = MouseButtons.None;
			rea.MouseX = MouseX;
			rea.MouseY = MouseY;
			if (MouseX >= this.View.TextMargin - 7)
			{
				rea.Area = RowArea.TextArea;
			}
			else if (MouseX < this.View.GutterMarginWidth)
			{
				rea.Area = RowArea.GutterArea;
			}
			else if (MouseX < this.View.LineNumberMarginWidth + this.View.GutterMarginWidth)
			{
				rea.Area = RowArea.LineNumberArea;
			}
			else if (MouseX < this.View.TextMargin - 7)
			{
				rea.Area = RowArea.FoldingArea;
			}

			this.OnRowDoubleClick(rea);

			#endregion

			try
			{
				Row r2 = Document[pos.Y];
				if (r2 != null)
				{
					if (MouseX >= r2.Expansion_PixelEnd && r2.IsCollapsed)
					{
						if (r2.Expansion_StartSegment != null)
						{
							if (r2.Expansion_StartSegment.StartRow != null && r2.Expansion_StartSegment.EndRow != null && r2.Expansion_StartSegment.Expanded == false)
							{
								r2.Expanded = true;
								this.Document.ResetVisibleRows();
								this.Redraw();
								return;
							}
						}
					}
				}
			}
			catch
			{
				//this is untested code...
			}

			if (MouseX > this.View.TotalMarginWidth)
				SelectCurrentWord();


		}

		protected override void OnClick(EventArgs e)
		{
			TextPoint pos = Painter.CharFromPixel(MouseX, MouseY);
			Row row = null;
			if (pos.Y >= 0 && pos.Y < this.Document.Count)
				row = this.Document[pos.Y];

			#region RowEvent

			RowMouseEventArgs rea = new RowMouseEventArgs();
			rea.Row = row;
			rea.Button = MouseButtons.None;
			rea.MouseX = MouseX;
			rea.MouseY = MouseY;
			if (MouseX >= this.View.TextMargin - 7)
			{
				rea.Area = RowArea.TextArea;
			}
			else if (MouseX < this.View.GutterMarginWidth)
			{
				rea.Area = RowArea.GutterArea;
			}
			else if (MouseX < this.View.LineNumberMarginWidth + this.View.GutterMarginWidth)
			{
				rea.Area = RowArea.LineNumberArea;
			}
			else if (MouseX < this.View.TextMargin - 7)
			{
				rea.Area = RowArea.FoldingArea;
			}

			this.OnRowClick(rea);

			#endregion
		}

		private void vScroll_Scroll(object sender, System.Windows.Forms.ScrollEventArgs e)
		{
			SetMaxHorizontalScroll();
			_InfoTipVisible = false;
			_AutoListVisible = false;

			SetFocus();


			int diff = e.NewValue - vScroll.Value;
			if ((diff == -1 || diff == 1) && (e.Type == ScrollEventType.SmallDecrement || e.Type == ScrollEventType.SmallIncrement))
			{
				ScrollScreen(diff);
			}
			else
			{
				Invalidate();
			}
		}

		private void hScroll_Scroll(object sender, System.Windows.Forms.ScrollEventArgs e)
		{
			_InfoTipVisible = false;
			_AutoListVisible = false;

			SetFocus();
			Invalidate();
		}

		private void CaretTimer_Tick(object sender, EventArgs e)
		{
			Caret.Blink = !Caret.Blink;
			this.RedrawCaret();
		}


		private void AutoListDoubleClick(object sender, EventArgs e)
		{
			string s = AutoList.SelectedText;
			if (s != "")
				InsertAutolistText();
			AutoListVisible = false;
			this.Redraw();
		}


		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);
			if (tooltip != null)
				tooltip.RemoveAll();
		}

		private void CaretChanged(object s, EventArgs e)
		{
			OnCaretChange();
		}

		private void EditViewControl_Leave(object sender, EventArgs e)
		{
			RemoveFocus();
		}

		private void EditViewControl_Enter(object sender, EventArgs e)
		{
			CaretTimer.Enabled = true;
		}

		private void SelectionChanged(object s, EventArgs e)
		{
			OnSelectionChange();
		}

		private void OnCaretChange()
		{
			if (CaretChange != null)
				CaretChange(this, null);
		}

		private void OnSelectionChange()
		{
			if (SelectionChange != null)
				SelectionChange(this, null);
		}

		protected override void OnVisibleChanged(EventArgs e)
		{
			if (this.Visible == false)
				RemoveFocus();

			base.OnVisibleChanged(e);
			DoResize();
		}

		#endregion

		private void IntelliMouse_BeginScroll(object sender, EventArgs e)
		{
			_IntelliScrollPos = 0;
			this.View.YOffset = 0;
		}

		private void IntelliMouse_EndScroll(object sender, EventArgs e)
		{
			this.View.YOffset = 0;
			this.Redraw();
		}

		private void IntelliMouse_Scroll(object sender, ScrollEventArgs e)
		{
			if (e.DeltaY < 0 && vScroll.Value == 0)
			{
				this.View.YOffset = 0;
				this.Redraw();
				return;

			}

			if (e.DeltaY > 0 && vScroll.Value >= vScroll.Maximum - this.View.VisibleRowCount + 1)
			{
				this.View.YOffset = 0;
				this.Redraw();
				return;
			}

			_IntelliScrollPos += (double) e.DeltaY/(double) 8;

			int scrollrows = (int) (_IntelliScrollPos)/this.View.RowHeight;
			if (scrollrows != 0)
			{
				_IntelliScrollPos -= scrollrows*this.View.RowHeight;
			}
			this.View.YOffset = -(int) _IntelliScrollPos;
			this.ScrollScreen(scrollrows);

		}


		protected override void WndProc(ref Message m)
		{
			if (m.Msg == (int) WindowMessage.WM_DESTROY)
			{
				try
				{
					if (FindReplaceDialog != null)
						FindReplaceDialog.Close();

					if (AutoList != null)
						AutoList.Close();

					if (InfoTip != null)
						InfoTip.Close();
				}
				catch
				{
				}

			}

			base.WndProc(ref m);
		}

		private void CopyAsRTF()
		{
			TextStyle[] styles = this.Document.Parser.Language.Styles;
			this.Document.ParseAll(true);
			int r1 = Selection.LogicalBounds.FirstRow;
			int r2 = Selection.LogicalBounds.LastRow;
			int c1 = Selection.LogicalBounds.FirstColumn;
			int c2 = Selection.LogicalBounds.LastColumn;

			StringBuilder sb = new StringBuilder();
			sb.Append(@"{\rtf1\ansi\ansicpg1252\deff0\deflang1053{\fonttbl{\f0\fmodern\fprq1\fcharset0 " + this.FontName + @";}}");
			sb.Append(@"{\colortbl ;");

			foreach (TextStyle ts in styles)
			{
				sb.AppendFormat("\\red{0}\\green{1}\\blue{2};", ts.ForeColor.R, ts.ForeColor.G, ts.ForeColor.B);
				sb.AppendFormat("\\red{0}\\green{1}\\blue{2};", ts.BackColor.R, ts.BackColor.G, ts.BackColor.B);
			}

			sb.Append(@";}");
			sb.Append(@"\viewkind4\uc1\pard\f0\fs20");


			bool Done = false;
			for (int i = r1; i <= r2; i++)
			{
				Row row = this.Document[i];


				foreach (Word w in row)
				{
					if (i == r1 && w.Column + w.Text.Length < c1)
						continue;

					bool IsFirst = (i == r1 && w.Column <= c1 && w.Column + w.Text.Length > c1);
					bool IsLast = (i == r2 && w.Column < c2 && w.Column + w.Text.Length > c2);


					if (w.Type == WordType.xtWord && w.Style != null)
					{
						int clrindex = Array.IndexOf(styles, w.Style);
						clrindex *= 2;
						clrindex ++;

						sb.Append("{\\cf" + clrindex.ToString());
						if (!w.Style.Transparent)
						{
							sb.Append("\\highlight" + (clrindex + 1).ToString());
						}
						sb.Append(" ");
					}

					if (w.Style != null)
					{
						if (w.Style.Bold)
							sb.Append(@"\b ");
						if (w.Style.Underline)
							sb.Append(@"\ul ");
						if (w.Style.Italic)
							sb.Append(@"\i ");
					}
					string wordtext = w.Text;

					if (IsLast)
						wordtext = wordtext.Substring(0, c2 - w.Column);

					if (IsFirst)
						wordtext = wordtext.Substring(c1 - w.Column);


					wordtext = wordtext.Replace(@"\", @"\\").Replace(@"}", @"\}").Replace(@"{", @"\{");

					sb.Append(wordtext);

					if (w.Style != null)
					{
						if (w.Style.Bold)
							sb.Append(@"\b0 ");
						if (w.Style.Underline)
							sb.Append(@"\ul0 ");
						if (w.Style.Italic)
							sb.Append(@"\i0 ");
					}

					if (w.Type == WordType.xtWord && w.Style != null)
					{
						sb.Append("}");
					}

					if (IsLast)
					{
						Done = true;
						break;
					}
				}
				if (Done)
					break;

				sb.Append(@"\par");
			}


			DataObject da;


			da = new DataObject();
			da.SetData(DataFormats.Rtf, sb.ToString());
			string s = this.Selection.Text;
			da.SetData(DataFormats.Text, s);
			Clipboard.SetDataObject(da);

			CopyEventArgs ea = new CopyEventArgs();
			ea.Text = s;
			OnClipboardUpdated(ea);
		}


        /// <summary>
        /// Scrolls a given position in the text to the center of the view.
        /// </summary>
        /// <param name="Pos">Position in text</param>
        public void CenterInView(int rowIndex)
        {
            //this method is a contribution of Marcel Isler goodfun (at) goodfun (dot) org 
            if (this.Document == null && rowIndex > this.Document.Count
                && rowIndex < 0)
                return;

            Row row = this.Document[rowIndex];
            int topRow = 0;
            int visibleLines = 0;

            if (row != null)
            {
                InitScrollbars();
                row.EnsureVisible();

                visibleLines = View.VisibleRowCount / 2;
                if (row.VisibleIndex < visibleLines)
                {
                    topRow = 0;
                }
                else
                {
                    topRow = row.VisibleIndex - visibleLines;
                }
                this.vScroll.Value = topRow;
                this.Invalidate();
            }
        }

	}
}