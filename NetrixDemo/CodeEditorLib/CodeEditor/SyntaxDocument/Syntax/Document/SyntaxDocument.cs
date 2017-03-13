using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using GuruComponents.CodeEditor.CodeEditor.Syntax.SyntaxDocumentParsers;

namespace GuruComponents.CodeEditor.CodeEditor.Syntax
{

	/// <summary>
	/// The SyntaxDocument is a component that is responsible for Parsing , Folding , Undo / Redo actions and various text actions.
	/// </summary>
	public class SyntaxDocument : Component, IEnumerable
	{
		#region General declarations

		/// <summary>
		/// List of rows that should be parsed
		/// </summary>
		public RowCollection ParseQueue = new RowCollection();

		/// <summary>
		/// 
		/// </summary>
		public RowCollection KeywordQueue = new RowCollection();

		public FormatRangeCollection FormatRanges = null;

		/// <summary>
		/// List of rows that is not hidden by folding
		/// </summary>
		public RowCollection VisibleRows = new RowCollection();

		private RowCollection mDocument = new RowCollection();

		/// <summary>
		/// Buffer containing undo actions
		/// </summary>
		public UndoBuffer UndoBuffer = new UndoBuffer();

		/// <summary>
		/// The active parser of the document
		/// </summary>
		public IParser Parser = new DefaultParser();

		/// <summary>
		/// For public use only
		/// </summary>
		private bool mIsParsed = true;

		private string mSyntaxFile = "";

		#region PUBLIC PROPERTY UNDOSTEP

		private int _UndoStep;

		public int UndoStep
		{
			get
			{
				if (_UndoStep > this.UndoBuffer.Count)
					_UndoStep = this.UndoBuffer.Count;

				return _UndoStep;
			}
			set { _UndoStep = value; }
		}

		#endregion

		/// <summary>
		/// Tag property , lets the user store custom data in the row.
		/// </summary>
		public object Tag = null;


		/// <summary>
		/// Gets or Sets if folding needs to be recalculated
		/// </summary>
		public bool NeedResetRows = false;

		private bool mModified = false;
		private bool mFolding = true;
		private Container components = null;
		private bool mCaptureMode = false;
		private UndoBlockCollection mCaptureBlock = null;

		/// <summary>
		/// Event that is raised when there is no more rows to parse
		/// </summary>
		public event EventHandler ParsingCompleted;

		public event EventHandler UndoBufferChanged = null;

		/// <summary>
		/// Raised when the parser is active
		/// </summary>
		public event EventHandler Parsing;

		/// <summary>
		/// Raised when the document content is changed
		/// </summary>
		public event EventHandler Change;

		public event RowEventHandler BreakPointAdded;
		public event RowEventHandler BreakPointRemoved;

		public event RowEventHandler BookmarkAdded;
		public event RowEventHandler BookmarkRemoved;

		protected virtual void OnBreakPointAdded(Row r)
		{
			if (BreakPointAdded != null)
				BreakPointAdded(this, new RowEventArgs(r));
		}

		protected virtual void OnBreakPointRemoved(Row r)
		{
			if (BreakPointRemoved != null)
				BreakPointRemoved(this, new RowEventArgs(r));
		}

		protected virtual void OnBookmarkAdded(Row r)
		{
			if (BookmarkAdded != null)
				BookmarkAdded(this, new RowEventArgs(r));
		}

		protected virtual void OnBookmarkRemoved(Row r)
		{
			if (BookmarkRemoved != null)
				BookmarkRemoved(this, new RowEventArgs(r));
		}

		protected virtual void OnUndoBufferChanged()
		{
			if (UndoBufferChanged != null)
				UndoBufferChanged(this, EventArgs.Empty);
		}


		public virtual void InvokeBreakPointAdded(Row r)
		{
			OnBreakPointAdded(r);
		}

		public virtual void InvokeBreakPointRemoved(Row r)
		{
			OnBreakPointRemoved(r);
		}

		public virtual void InvokeBookmarkAdded(Row r)
		{
			OnBookmarkAdded(r);
		}

		public virtual void InvokeBookmarkRemoved(Row r)
		{
			OnBookmarkRemoved(r);
		}


		//public event System.EventHandler CreateParser;

		/// <summary>
		/// Raised when the modified flag has changed
		/// </summary>
		public event EventHandler ModifiedChanged;

		//----------------------------------------------

		/// <summary>
		/// Raised when a row have been parsed
		/// </summary>
		public event ParserEventHandler RowParsed;

		//	public event ParserEventHandler	 RowAdded;
		/// <summary>
		/// Raised when a row have been deleted
		/// </summary>
		public event ParserEventHandler RowDeleted;

		#endregion

		#region PUBLIC PROPERTY MAXUNDOBUFFERSIZE

		/// <summary>
		/// Gets or Sets the Maximum number of entries in the undobuffer
		/// </summary>
		public int MaxUndoBufferSize
		{
			get { return UndoBuffer.MaxSize; }
			set { UndoBuffer.MaxSize = value; }
		}

		#endregion

		#region PUBLIC PROPERTY VERSION

		private long _Version = long.MinValue;

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public long Version
		{
			get { return _Version; }
			set { _Version = value; }
		}

		#endregion

		/// <summary>
		/// For internal use only
		/// </summary>
		public void ChangeVersion()
		{
			this.Version ++;
			if (this.Version > long.MaxValue - 10)
				this.Version = long.MinValue;
		}

		/// <summary>
		/// Starts an Undo Capture.
		/// This method can be called if you with to collect multiple text operations into one undo action
		/// </summary>
		public void StartUndoCapture()
		{
			mCaptureMode = true;
			mCaptureBlock = new UndoBlockCollection();
		}

		/// <summary>
		/// Ends an Undo capture and pushes the collected actions onto the undostack
		/// <seealso cref="StartUndoCapture"/>
		/// </summary>
		/// <returns></returns>
		public UndoBlockCollection EndUndoCapture()
		{
			mCaptureMode = false;
			AddToUndoList(mCaptureBlock);
			return mCaptureBlock;
		}

		/// <summary>
		/// ReParses the document
		/// </summary>
		public void ReParse()
		{
			this.Text = this.Text;
		}

		/// <summary>
		/// Get or Set the Modified flag
		/// </summary>
		public bool Modified
		{
			get { return mModified; }
			set
			{
				mModified = value;
				OnModifiedChanged();
			}
		}

		/// <summary>
		/// Removes all bookmarks in the document
		/// </summary>
		public void ClearBookmarks()
		{
			foreach (Row r in this)
			{
				r.Bookmarked = false;
			}
			InvokeChange();
		}

		/// <summary>
		/// Removes all breakpoints in the document.
		/// </summary>
		public void ClearBreakpoints()
		{
			foreach (Row r in this)
			{
				r.Breakpoint = false;
			}
			InvokeChange();
		}


		/// <summary>
		/// Get or Set the Name of the Syntaxfile to use
		/// </summary>
		[DefaultValue("")]
		public string SyntaxFile
		{
			get { return mSyntaxFile; }
			set
			{
				mSyntaxFile = value;
				//	this.Parser=new Parser_Default();
				this.Parser.Init(value);
				this.Text = this.Text;
			}
		}

		/// <summary>
		/// Call this method to ensure that a specific row is fully parsed
		/// </summary>
		/// <param name="Row"></param>
		public void EnsureParsed(Row Row)
		{
			this.ParseAll();
			Parser.ParseLine(Row.Index, true);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="container"></param>
		public SyntaxDocument(IContainer container) : this()
		{
			container.Add(this);
			InitializeComponent();
		}

		private void Init()
		{
			Language l = new Language();
			l.MainBlock = new BlockType(l);
			l.MainBlock.MultiLine = true;
			this.Parser.Init(l);
		}

		/// <summary>
		/// Call this method to make the SyntaxDocument raise the Changed event
		/// </summary>
		public void InvokeChange()
		{
			OnChange();
		}

		/// <summary>
		/// Gets or Sets if the document should use folding or not
		/// </summary>
		[DefaultValue(true)]
		public bool Folding
		{
			get { return mFolding; }
			set
			{
				mFolding = value;
				if (!value)
				{
					foreach (Row r in this)
					{
						r.Expanded = true;
					}

				}
				ResetVisibleRows();

				OnChange();
			}
		}

		#region Component Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
		}

		#endregion

		/// <summary>
		/// 
		/// </summary>
		public SyntaxDocument()
		{
			Parser.Document = this;
			Text = "";
			ResetVisibleRows();
			Init();
			this.FormatRanges = new FormatRangeCollection(this);
		}

		/// <summary>
		/// Performs a segment parse on all rows. No Keyword colorizing
		/// </summary>
		public void ParseAll()
		{
			while (ParseQueue.Count > 0)
				ParseSome();

			ParseQueue.Clear();
		}

		/// <summary>
		/// Parses all rows , either a segment parse or a full parse with keyword colorizing
		/// </summary>
		public void ParseAll(bool ParseKeywords)
		{
			this.ParseAll();
			if (ParseKeywords)
			{
				for (int i = 0; i < this.Count; i++)
				{
					if (this[i].RowState != RowState.AllParsed)
						Parser.ParseLine(i, true);
				}
				ParseQueue.Clear();
				KeywordQueue.Clear();
			}


		}

		/// <summary>
		/// Folds all foldable rows
		/// </summary>
		public void FoldAll()
		{
			this.ParseAll(false);
			foreach (Row r in this)
			{
				r.Expanded = false;
			}
			this.ResetVisibleRows();
			this.OnChange();
		}

		/// <summary>
		/// UnFolds all foldable rows
		/// </summary>
		public void UnFoldAll()
		{
			this.ParseAll(false);
			foreach (Row r in this)
			{
				r.Expanded = true;
			}
			this.ResetVisibleRows();
			this.OnChange();
		}


		/// <summary>
		/// Parses a chunk of 1000 rows , this is not thread safe
		/// </summary>
		public void ParseSome()
		{
			this.ParseSome(1000);
		}

		/// <summary>
		/// Parse a chunk of rows, this is not thread safe
		/// </summary>
		/// <param name="RowCount">The number of rows to parse</param>
		public void ParseSome(int RowCount)
		{
			if (this.ParseQueue.Count > 0)
			{
				mIsParsed = false;
				int i = 0;
				while (i < RowCount && this.ParseQueue.Count > 0)
				{
					Row row = this.ParseQueue[0];
					i += ParseRows(row);
				}

				if (this.NeedResetRows)
					this.ResetVisibleRows();

				if (Parsing != null)
					Parsing(this, new EventArgs());
			}
			else
			{
				if (!mIsParsed && !Modified)
				{
					mIsParsed = true;

					foreach (Row r in this)
					{
						if (r.Expansion_StartSegment != null && r.Expansion_EndRow != null)
						{
							if (r.Expansion_StartSegment.Scope.DefaultExpanded == false)
								r.Expanded = false;
						}
					}
					ResetVisibleRows();
					if (ParsingCompleted != null)
						ParsingCompleted(this, new EventArgs());
				}
			}

			if (this.ParseQueue.Count == 0 && this.KeywordQueue.Count > 0)
			{
//				Console.WriteLine (this.KeywordQueue.Count.ToString ());
				int i = 0;
				while (i < RowCount/20 && this.KeywordQueue.Count > 0)
				{
					Row row = this.KeywordQueue[0];
					i += ParseRows(row, true);
				}
			}
		}


		/// <summary>
		/// Gets if the document is fully parsed
		/// </summary>
		[Browsable(false)]
		public bool IsParsed
		{
			get { return mIsParsed; }
		}

		/// <summary>
		/// Returns the row at the specified index
		/// </summary>
		public Row this[int index]
		{
			get
			{
				if (index < 0 || index >= mDocument.Count)
				{
					//	System.Diagnostics.Debugger.Break ();
					return null;
				}
				return mDocument[index];
			}

			set { mDocument[index] = value; }
		}

		/// <summary>
		/// Add a new row with the specified text to the bottom of the document
		/// </summary>
		/// <param name="Text">Text to add</param>
		/// <returns>The row that was added</returns>		
		public Row Add(string Text)
		{
			return Add(Text, true);
		}

		/// <summary>
		/// Add a new row with the specified text to the bottom of the document
		/// </summary>
		/// <param name="Text">Text to add</param>
		/// <param name="StoreUndo">true if and undo action should be added to the undo stack</param>
		/// <returns>The row that was added</returns>
		public Row Add(string Text, bool StoreUndo)
		{
			Row xtl = new Row();
			mDocument.Add(xtl);
			xtl.Document = this;
			xtl.Text = Text;
			return xtl;
		}

		/// <summary>
		/// Insert a text at the specified row index
		/// </summary>
		/// <param name="Text">Text to insert</param>
		/// <param name="index">Row index where the text should be inserted</param>
		/// <returns>The row that was inserted</returns>
		public Row Insert(string Text, int index)
		{
			return Insert(Text, index, true);
		}

		/// <summary>
		/// Insert a text at the specified row index
		/// </summary>
		/// <param name="Text">Text to insert</param>
		/// <param name="index">Row index where the text should be inserted</param>
		/// <param name="StoreUndo">true if and undo action should be added to the undo stack</param>
		/// <returns>The row that was inserted</returns>
		public Row Insert(string Text, int index, bool StoreUndo)
		{
			Row xtl = new Row();
			xtl.Document = this;
			mDocument.Insert(index, xtl);
			xtl.Text = Text;
			if (StoreUndo)
			{
				UndoBlock undo = new UndoBlock();
				undo.Text = Text;
				undo.Position.Y = this.IndexOf(xtl);
				AddToUndoList(undo);
			}

			//this.ResetVisibleRows ();
			return xtl;
		}


		/// <summary>
		/// Remove a row at specified row index
		/// </summary>
		/// <param name="index">index of the row that should be removed</param>
		public void Remove(int index)
		{
			Remove(index, true);

		}

		public void Remove(int index, bool StoreUndo)
		{
			this.Remove(index, StoreUndo, true);
		}

		/// <summary>
		/// Remove a row at specified row index
		/// </summary>
		/// <param name="index">index of the row that should be removed</param>
		/// <param name="StoreUndo">true if and undo action should be added to the undo stack</param>
		public void Remove(int index, bool StoreUndo, bool RaiseChanged)
		{
			Row r = this[index];

			if (StoreUndo)
			{
				TextRange ra = new TextRange();

				if (index != this.Count - 1)
				{
					ra.FirstColumn = 0;
					ra.FirstRow = index;
					ra.LastRow = index + 1;
					ra.LastColumn = 0;
				}
				else
				{
					ra.FirstColumn = r.PrevRow.Text.Length;
					ra.FirstRow = index - 1;
					ra.LastRow = index;
					ra.LastColumn = r.Text.Length;
				}
				PushUndoBlock(UndoAction.DeleteRange, GetRange(ra), ra.FirstColumn, ra.FirstRow);

			}


			mDocument.RemoveAt(index);
			if (r.InKeywordQueue)
				this.KeywordQueue.Remove(r);

			if (r.InQueue)
				this.ParseQueue.Remove(r);

			//this.ResetVisibleRows ();
			OnRowDeleted(r);
			if (RaiseChanged)
				OnChange();
		}

		/// <summary>
		/// Deletes a range of text
		/// </summary>
		/// <param name="Range">the range that should be deleted</param>
		public void DeleteRange(TextRange Range)
		{
			DeleteRange(Range, true);
		}

		private int ParseRows(Row r)
		{
			return ParseRows(r, false);
		}


		private int ParseRows(Row r, bool Keywords)
		{
			if (!Keywords)
			{
				int index = this.IndexOf(r);
				int count = 0;
                //try
                //{
					while (r.InQueue && count < 100)
					{
						if (index >= 0)
						{
							if (index > 0)
								if (this[index - 1].InQueue)
									ParseRow(this[index - 1]);

							Parser.ParseLine(index, false);
						}

						int i = this.ParseQueue.IndexOf(r);
						if (i >= 0)
							this.ParseQueue.RemoveAt(i);
						r.InQueue = false;
						index++;
						count++;
						r = this[index];

						if (r == null)
							break;
					}
                //}
                //catch
                //{
                //}

				return count;
			}
			else
			{
				int index = this.IndexOf(r);
				if (index == -1 || r.InKeywordQueue == false)
				{
					this.KeywordQueue.Remove(r);
					return 0;
				}
				int count = 0;
                //try
                //{
					while (r.InKeywordQueue && count < 100)
					{
						if (index >= 0)
						{
							if (index > 0)
								if (this[index - 1].InQueue)
									ParseRow(this[index - 1]);

							Parser.ParseLine(index, true);
						}
						index++;
						count++;
						r = this[index];

						if (r == null)
							break;
					}
                //}
                //catch
                //{
                //}

				return count;
			}
		}


		/// <summary>
		/// Forces a row to be parsed
		/// </summary>
		/// <param name="r">Row to parse</param>
		/// <param name="ParseKeywords">true if keywords and operators should be parsed</param>
		public void ParseRow(Row r, bool ParseKeywords)
		{
			int index = this.IndexOf(r);
			if (index >= 0)
			{
				if (index > 0)
					if (this[index - 1].InQueue)
						ParseRow(this[index - 1]);

				Parser.ParseLine(index, false);
				if (ParseKeywords)
					Parser.ParseLine(index, true);
			}

			int i = this.ParseQueue.IndexOf(r);
			if (i >= 0)
				this.ParseQueue.RemoveAt(i);

			r.InQueue = false;
		}


		/// <summary>
		/// Forces a row to be parsed
		/// </summary>
		/// <param name="r">Row to parse</param>
		public void ParseRow(Row r)
		{
			ParseRow(r, false);
		}

		/// <summary>
		/// Gets the row index of the next bookmarked row
		/// </summary>
		/// <param name="StartIndex">Start index</param>
		/// <returns>Index of the next bookmarked row</returns>
		public int GetNextBookmark(int StartIndex)
		{
			for (int i = StartIndex + 1; i < this.Count; i++)
			{
				Row r = this[i];
				if (r.Bookmarked)
					return i;
			}

			for (int i = 0; i < StartIndex; i++)
			{
				Row r = this[i];
				if (r.Bookmarked)
					return i;
			}

			return StartIndex;
		}

		/// <summary>
		/// Gets the row index of the previous bookmarked row
		/// </summary>
		/// <param name="StartIndex">Start index</param>
		/// <returns>Index of the previous bookmarked row</returns>
		public int GetPreviousBookmark(int StartIndex)
		{
			for (int i = StartIndex - 1; i >= 0; i--)
			{
				Row r = this[i];
				if (r.Bookmarked)
					return i;
			}

			for (int i = this.Count - 1; i >= StartIndex; i--)
			{
				Row r = this[i];
				if (r.Bookmarked)
					return i;
			}

			return StartIndex;
		}

		/// <summary>
		/// Deletes a range of text
		/// </summary>
		/// <param name="Range">Range to delete</param>
		/// <param name="StoreUndo">true if the actions should be pushed onto the undo stack</param>
		public void DeleteRange(TextRange Range, bool StoreUndo)
		{
			TextRange r = Range;
			Modified = true;
			if (StoreUndo)
			{
				string deltext = GetRange(Range);
				PushUndoBlock(UndoAction.DeleteRange, deltext, r.FirstColumn, r.FirstRow);
			}


			if (r.FirstRow == r.LastRow)
			{
				Row xtr = this[r.FirstRow];
				int max = Math.Min(r.FirstColumn, xtr.Text.Length);
				string left = xtr.Text.Substring(0, max);
				string right = "";
				if (xtr.Text.Length >= r.LastColumn)
					right = xtr.Text.Substring(r.LastColumn);
				xtr.Text = left + right;
			}
			else
			{
				if (r.LastRow > this.Count - 1)
					r.LastRow = this.Count - 1;

				string row1 = "";
				Row xtr = this[r.FirstRow];
				if (r.FirstColumn > xtr.Text.Length)
				{
					int diff = r.FirstColumn - xtr.Text.Length;
					string ws = new string(' ', diff);
					this.InsertText(ws, xtr.Text.Length, r.FirstRow, true);
					//return;
				}

				row1 = xtr.Text.Substring(0, r.FirstColumn);

				string row2 = "";
				Row xtr2 = this[r.LastRow];
				int Max = Math.Min(xtr2.Text.Length, r.LastColumn);
				row2 = xtr2.Text.Substring(Max);

				string tot = row1 + row2;
				//bool fold=this[r.LastRow].IsCollapsed | this[r.FirstRow].IsCollapsed ;

				int start = r.FirstRow;
				int end = r.LastRow;

				for (int i = end - 1; i >= start; i--)
				{
					this.Remove(i, false, false);
				}

				//todo: DeleteRange error						
				//this.Insert ( tot  ,r.FirstRow,false);


				Row row = this[start];
				bool f = row.IsCollapsed;
				row.Expanded = true;
				row.Text = tot;
				row.StartSegments.Clear();
				row.EndSegments.Clear();
				row.StartSegment = null;
				row.EndSegment = null;
				row.Parse();


				//	if (row.CanFold)
				//		row.Expansion_StartSegment.Expanded = !fold;

			}

			//ShirinkFormatRanges
			if (this.FormatRanges != null)
			{
				this.FormatRanges.Shrink(Range);
			}


			ResetVisibleRows();
			OnChange();


		}

		/// <summary>
		/// Get a range of text
		/// </summary>
		/// <param name="Range">The range to get</param>
		/// <returns>string containing the text inside the given range</returns>
		public string GetRange(TextRange Range)
		{
			if (Range.FirstRow >= this.Count)
				Range.FirstRow = this.Count;

			if (Range.LastRow >= this.Count)
				Range.LastRow = this.Count;

			if (Range.FirstRow != Range.LastRow)
			{
				//note:error has been tracked here
				Row r1 = this[Range.FirstRow];
				int mx = Math.Min(r1.Text.Length, Range.FirstColumn);
				string s1 = r1.Text.Substring(mx) + Environment.NewLine;

				//if (Range.LastRow >= this.Count)
				//	Range.LastRow=this.Count -1;

				Row r2 = this[Range.LastRow];
				if (r2 == null)
					return "";

				int Max = Math.Min(r2.Text.Length, Range.LastColumn);
				string s2 = r2.Text.Substring(0, Max);

				string s3 = "";
				StringBuilder sb = new StringBuilder();
				for (int i = Range.FirstRow + 1; i <= Range.LastRow - 1; i++)
				{
					Row r3 = this[i];

					sb.Append(r3.Text + Environment.NewLine);
				}

				s3 = sb.ToString();
				return s1 + s3 + s2;
			}
			else
			{
				Row r = this[Range.FirstRow];
				int Max = Math.Min(r.Text.Length, Range.LastColumn);
				int Length = Max - Range.FirstColumn;
				if (Length <= 0)
					return "";
				string s = r.Text.Substring(Range.FirstColumn, Max - Range.FirstColumn);
				return s;
			}

		}


		/// <summary>
		/// Gets the row count of the document
		/// </summary>
		[Browsable(false)]
		public int Count
		{
			get { return mDocument.Count; }
		}

		/// <summary>
		/// Returns the index of a given row
		/// </summary>
		/// <param name="xtr">row to find</param>
		/// <returns>Index of the given row</returns>
		public int IndexOf(Row xtr)
		{
			return mDocument.IndexOf(xtr);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public IEnumerator GetEnumerator()
		{
			return mDocument.GetEnumerator();
		}

		/// <summary>
		/// Clear all content in the document
		/// </summary>
		public void clear()
		{
			foreach (Row r in mDocument)
			{
				OnRowDeleted(r);
			}
			mDocument.Clear();
			//		this.FormatRanges.Clear ();
			ParseQueue.Clear();
			KeywordQueue.Clear();
			UndoBuffer.Clear();
			UndoStep = 0;
			//	this.Add ("");
			//	ResetVisibleRows();
			//	this.OnChange ();
		}

		/// <summary>
		/// Gets or Sets the text of the entire document
		/// </summary>		
		[Browsable(false)]
		//	[RefreshProperties (RefreshProperties.All)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string Text
		{
			get
			{
				int i = 0;
				StringBuilder sb = new StringBuilder();

				ParseAll(true);
				foreach (Row tr in mDocument)
				{
					if (i > 0)
						sb.Append(Environment.NewLine);
					tr.MatchCase();
					sb.Append(tr.Text);
					i++;
				}
				return sb.ToString();
			}

			set
			{
				this.clear();
				this.Add("");
				InsertText(value, 0, 0);
				UndoBuffer.Clear();
				UndoStep = 0;
				Modified = false;
				mIsParsed = false;
				//OnChange();
				InvokeChange();
			}
		}

		public void Clear()
		{
			this.Text = "";
		}

		/// <summary>
		/// Gets and string array containing the text of all rows.
		/// </summary>
		public string[] Lines
		{
			get
			{
				string s = this.Text.Replace(Environment.NewLine, "\n");
				return Text.Split("\n".ToCharArray());
			}
			set
			{
				string s = "";
				foreach (string sl in value)
					s += sl + "\n";
				Text = s.Substring(0, s.Length - 1);
			}
		}

		/// <summary>
		/// Inserts a text into the document at a given column,row.
		/// </summary>
		/// <param name="text">Text to insert</param>
		/// <param name="xPos">Column</param>
		/// <param name="yPos">Row index</param>
		/// <returns>TextPoint containing the end of the inserted text</returns>
		public TextPoint InsertText(string text, int xPos, int yPos)
		{
			return InsertText(text, xPos, yPos, true);
		}

		/// <summary>
		/// Inserts a text into the document at a given column,row.
		/// </summary>
		/// <param name="text">Text to insert</param>
		/// <param name="xPos">Column</param>
		/// <param name="yPos">Row index</param>
		/// <param name="StoreUndo">true if this action should be pushed onto the undo stack</param>
		/// <returns>TextPoint containing the end of the inserted text</returns>
		public TextPoint InsertText(string text, int xPos, int yPos, bool StoreUndo)
		{
			Modified = true;
			Row xtr = this[yPos];
			string lft, rgt;

			if (xPos > xtr.Text.Length)
			{
				//virtualwhitespace fix
				int Padd = xPos - xtr.Text.Length;
				string PaddStr = new string(' ', Padd);
				text = PaddStr + text;
				xPos -= Padd;
			}
			lft = xtr.Text.Substring(0, xPos);
			rgt = xtr.Text.Substring(xPos);
			string NewText = lft + text + rgt;


			string t = NewText.Replace(Environment.NewLine, "\n");
			string[] lines = t.Split('\n');
			xtr.Text = lines[0];

			Row lastrow = xtr;

			//this.Parser.ParsePreviewLine(xtr);	
			xtr.Parse();
			if (!xtr.InQueue)
				this.ParseQueue.Add(xtr);
			xtr.InQueue = true;

			int i = IndexOf(xtr);


			for (int j = 1; j < lines.Length; j++)
			{
				lastrow = Insert(lines[j], j + i, false);
			}

			if (StoreUndo)
				PushUndoBlock(UndoAction.InsertRange, text, xPos, yPos);


			//ExpandFormatRanges
			if (this.FormatRanges != null)
			{
				this.FormatRanges.Expand(xPos, yPos, text);

			}


			this.ResetVisibleRows();
			OnChange();


			return new TextPoint(lastrow.Text.Length - rgt.Length, IndexOf(lastrow));
		}

		private void OnModifiedChanged()
		{
			//System.Windows.Forms.MessageBox.Show ("on change");
			if (ModifiedChanged != null)
				ModifiedChanged(this, new EventArgs());
		}

		private void OnChange()
		{
			//System.Windows.Forms.MessageBox.Show ("on change");
			if (Change != null)
				Change(this, new EventArgs());
		}

		private void OnRowParsed(Row r)
		{
			if (RowParsed != null)
				RowParsed(this, new RowEventArgs(r));

			this.OnApplyFormatRanges(r);
		}

		//		private void OnRowAdded(Row r)
		//		{
		//			if (RowAdded != null)
		//				RowAdded(this,new RowEventArgs(r));
		//		}
		private void OnRowDeleted(Row r)
		{
			if (RowDeleted != null)
				RowDeleted(this, new RowEventArgs(r));
		}

		public void PushUndoBlock(UndoAction Action, string Text, int x, int y)
		{
			UndoBlock undo = new UndoBlock();
			undo.Action = Action;
			undo.Text = Text;
			undo.Position.Y = y;
			undo.Position.X = x;
			//AddToUndoList(undo);

			if (mCaptureMode)
			{
				mCaptureBlock.Add(undo);
			}
			else
			{
				AddToUndoList(undo);
			}
		}

		/// <summary>
		/// Gets a Range from a given text
		/// </summary>
		/// <param name="text"></param>
		/// <param name="xPos"></param>
		/// <param name="yPos"></param>
		/// <returns></returns>
		public TextRange GetRangeFromText(string text, int xPos, int yPos)
		{
			string t = text.Replace(Environment.NewLine, "\n");
			string[] lines = t.Split("\n".ToCharArray());
			TextRange r = new TextRange();
			r.FirstColumn = xPos;
			r.FirstRow = yPos;
			r.LastRow = lines.Length - 1 + yPos;
			r.LastColumn = lines[lines.Length - 1].Length;
			if (r.FirstRow == r.LastRow)
				r.LastColumn += r.FirstColumn;

			return r;
		}

		public void AddToUndoList(UndoBlock undo)
		{
			//store the undo action in a actiongroup
			UndoBlockCollection ActionGroup = new UndoBlockCollection();
			ActionGroup.Add(undo);

			AddToUndoList(ActionGroup);
		}

        /// <summary>
        /// Sets a breakpoint in the document at specified line.
        /// </summary>
        /// <param name="line">Line number where to set.</param>
        public void AddBreakpoint(int line)
        {
            foreach (Row r in this)
            {
                if (r.Index == line - 1)
                {
                    r.Breakpoint = true;
                    break;
                }
            }
            InvokeChange();
        }

	    /// <summary>
		/// Add an action to the undo stack
		/// </summary>
		/// <param name="ActionGroup">action to add</param>
		public void AddToUndoList(UndoBlockCollection ActionGroup)
		{
			//int count=UndoBuffer.Count-UndoStep;
//			if (count>0)
//			{
//				System.Windows.Forms.MessageBox.Show (UndoStep.ToString ());
//				System.Windows.Forms.MessageBox.Show (count.ToString ());
//			}

			UndoBuffer.ClearFrom(UndoStep);

			//	System.Windows.Forms.MessageBox.Show (UndoBuffer.Count.ToString());		
			UndoBuffer.Add(ActionGroup);
			UndoStep++;
			this.OnUndoBufferChanged();
		}

		/// <summary>
		/// Perform an undo action
		/// </summary>
		/// <returns>The position where the caret should be placed</returns>
		public TextPoint Undo()
		{
			if (UndoStep == 0)
				return new TextPoint(-1, -1);


			UndoBlockCollection ActionGroup = (UndoBlockCollection) this.UndoBuffer[UndoStep - 1];
			UndoBlock undo = (UndoBlock) ActionGroup[0];

			for (int i = ActionGroup.Count - 1; i >= 0; i--)
			{
				undo = (UndoBlock) ActionGroup[i];
				//TextPoint tp=new TextPoint (undo.Position.X,undo.Position.Y);
				switch (undo.Action)
				{
					case UndoAction.DeleteRange:
						InsertText(undo.Text, undo.Position.X, undo.Position.Y, false);
						break;
					case UndoAction.InsertRange:
						{
							TextRange r = GetRangeFromText(undo.Text, undo.Position.X, undo.Position.Y);
							DeleteRange(r, false);
						}
						break;
					default:
						break;
				}
			}

			UndoStep--;
			this.ResetVisibleRows();

			//no undo steps left , the document is not dirty
			if (UndoStep == 0)
				Modified = false;

			TextPoint tp = new TextPoint(undo.Position.X, undo.Position.Y);
			OnUndoBufferChanged();
			return tp;

		}

		public void AutoIndentSegment(Segment Segment)
		{
			if (Segment == null)
				Segment = this[0].StartSegment;

			Row start = Segment.StartRow;
			Row end = Segment.EndRow;
			if (start == null)
				start = this[0];

			if (end == null)
				end = this[this.Count - 1];


			for (int i = start.Index; i <= end.Index; i++)
			{
				Row r = this[i];
				int depth = r.Indent;
				string text = r.Text.Substring(r.GetLeadingWhitespace().Length);
				string indent = new string('\t', depth);
				r.Text = indent + text;
			}
			this.ResetVisibleRows();

		}

		//Returns the segment object at the given position
		/// <summary>
		/// Gets a Segment object form a given column , Row index
		/// (This only applies if the row is fully parsed)
		/// </summary>
		/// <param name="p">Column and Rowindex</param>
		/// <returns>Segment object at the given position</returns>
		public Segment GetSegmentFromPos(TextPoint p)
		{
			Row xtr = this[p.Y];
			int CharNo = 0;

			if (xtr.Count == 0)
				return xtr.StartSegment;

			Segment prev = xtr.StartSegment;
            Word w;
            //foreach (Word w in xtr)
            for (int i = 0; i < xtr.Count; i++)
            {
                w = xtr[i];

				if (w.Text.Length + CharNo > p.X)
				{
					if (CharNo == p.X)
						return prev;
					else
						return w.Segment;
				}
				else
				{
					CharNo += w.Text.Length;
					prev = w.Segment;
				}
			}

			return xtr.EndSegment;
		}

		//the specific word that contains the char in point p
		/// <summary>
		/// Gets a Word object form a given column , Row index
		/// (this only applies if the row is fully parsed)
		/// </summary>
		/// <param name="p">Column and Rowindex</param>
		/// <returns>Word object at the given position</returns>
		public Word GetWordFromPos(TextPoint p)
		{
			Row xtr = this[p.Y];
			int CharNo = 0;
			Word CorrectWord = null;

            Word w;
			//foreach (Word w in xtr)
            for(int i = 0; i< xtr.Count;i++)
			{
                w = xtr[i];

				if (CorrectWord != null)
				{
					if (w.Text == "")
						return w;
					else
						return CorrectWord;
				}

				if (w.Text.Length + CharNo > p.X || w == xtr[xtr.Count - 1])
				{
					//return w;
					CorrectWord = w;
				}
				else
				{
					CharNo += w.Text.Length;
				}
			}
			return CorrectWord;
		}

		//the specific word that contains the char in point p
		/// <summary>
		/// Gets a Word object form a given column , Row index
		/// (this only applies if the row is fully parsed)
		/// </summary>
		/// <param name="p">Column and Rowindex</param>
		/// <returns>Word object at the given position</returns>
		public Word GetFormatWordFromPos(TextPoint p)
		{
			Row xtr = this[p.Y];
			int CharNo = 0;
			Word CorrectWord = null;
			//foreach (Word w in xtr.FormattedWords)
            for(int i = 0; i< xtr.FormattedWords.Count;i++)
			{
                Word w = xtr.FormattedWords[i];

				if (CorrectWord != null)
				{
					if (w.Text == "")
						return w;
					else
						return CorrectWord;
				}

				if (w.Text.Length + CharNo > p.X || w == xtr[xtr.Count - 1])
				{
					//return w;
					CorrectWord = w;
				}
				else
				{
					CharNo += w.Text.Length;
				}
			}
			return CorrectWord;
		}

		/// <summary>
		/// Call this method to make the document raise the RowParsed event
		/// </summary>
		/// <param name="row"></param>
		public void InvokeRowParsed(Row row)
		{
			this.OnRowParsed(row);
		}


		/// <summary>
		/// Call this method to recalculate the visible rows
		/// </summary>
		public void ResetVisibleRows()
		{
			InternalResetVisibleRows();
		}

		private void InternalResetVisibleRows()
		{
//			if (System.DateTime.Now > new DateTime (2002,12,31))
//			{
//				
//				this.mDocument = new RowCollection ();
//				this.Add ("BETA VERSION EXPIRED");
//				VisibleRows = this.mDocument;
//				return;
//			}

			if (!this.mFolding)
			{
				VisibleRows = mDocument;
				this.NeedResetRows = false;
			}
			else
			{
				this.NeedResetRows = false;
				VisibleRows = new RowCollection(); //.Clear ();			
				int RealRow = 0;
				Row r = null;
				for (int i = 0; i < this.Count; i++)
				{
					r = this[RealRow];
					VisibleRows.Add(r);
					bool collapsed = false;
					if (r.CanFold)
						if (r.Expansion_StartSegment.Expanded == false)
						{
							if (r.Expansion_StartSegment.EndWord == null)
							{
							}
							else
							{
								r = r.Expansion_EndRow; // .Expansion_StartSegment.EndRow;
								collapsed = true;
							}
						}

					if (!collapsed)
						RealRow++;
					else
						RealRow = this.IndexOf(r) + 1;

					if (RealRow >= this.Count)
						break;
				}
			}
		}

		/// <summary>
		/// Converts a Column/Row index position into a char index
		/// </summary>
		/// <param name="pos">TextPoint where x is column and y is row index</param>
		/// <returns>Char index in the document text</returns>
		public int PointToIntPos(TextPoint pos)
		{
			int y = 0;
			int p = 0;
			foreach (Row r in this)
			{
				if (y == pos.Y)
					break;
				p += r.Text.Length + Environment.NewLine.Length;
				y++;
			}

			return p + Math.Min(pos.X, this[pos.Y].Text.Length);
		}

		/// <summary>
		/// Converts a char index into a Column/Row index
		/// </summary>
		/// <param name="pos">Char index to convert</param>
		/// <returns>Point where x is column and y is row index</returns>
		public TextPoint IntPosToPoint(int pos)
		{
			int p = 0;
			int y = 0;
			int x = 0;
			foreach (Row r in this)
			{
				p += r.Text.Length + Environment.NewLine.Length;
				if (p > pos)
				{
					p -= r.Text.Length + Environment.NewLine.Length;
					x = pos - p;
					return new TextPoint(x, y);
				}
				y++;
			}
			return new TextPoint(-1, -1);
		}

		/// <summary>
		/// Toggle expansion of a given row
		/// </summary>
		/// <param name="r"></param>
		public void ToggleRow(Row r)
		{
			if (!mFolding)
				return;

			if (r.Expansion_EndRow == null || r.Expansion_StartRow == null)
				return;


//			if (r.IsCollapsed)
//			{
//				r.Expansion_StartSegment.Expanded =	true;
//				ExpandRow(r);
//			}
//			else
//			{
//				r.Expansion_StartSegment.Expanded =	false;
//				CollapseRow(r);
//			}

			if (r.CanFold)
				r.Expanded = !r.Expanded;
			ResetVisibleRows();

			OnChange();
		}

		/// <summary>
		/// Collapse a given row
		/// </summary>
		/// <param name="r">Row to collapse</param>
		private void CollapseRow(Row r)
		{
			//remove rows from visible list
			Row start = r.Expansion_StartRow;
			Row end = r.Expansion_EndRow;
			int count = end.VisibleIndex - start.VisibleIndex - 1;
			int startIndex = start.VisibleIndex + 1;
			for (int i = 0; i <= count; i++)
			{
				this.VisibleRows.RemoveAt(startIndex);
			}
		}

		/// <summary>
		/// Expand a given row
		/// </summary>
		/// <param name="r">Row to expand</param>
		private void ExpandRow(Row r)
		{
			//add rows to visible list...
			Row start = r.Expansion_StartRow;
			Row end = r.Expansion_EndRow;
			int count = end.Index - start.Index - 1;
			int startIndex = start.Index + 1;

			int visIndex = start.VisibleIndex + 1;
			int i = 0;
			while (i <= count)
			{
				Row tmpRow = this[startIndex + i];
				this.VisibleRows.Insert(visIndex, tmpRow);
				if (tmpRow.Expansion_StartSegment != null)
					if (tmpRow.Expansion_StartSegment.Expanded == false)
					{
						tmpRow = tmpRow.Expansion_StartSegment.EndRow;
						i = tmpRow.Index - startIndex;
					}
				visIndex++;
				i++;
			}

			//ResetVisibleRows ();
		}

		/// <summary>
		/// Perform an redo action
		/// </summary>
		/// <returns>The position where the caret should be placed</returns>
		public TextPoint Redo()
		{
			if (UndoStep >= UndoBuffer.Count)
				return new TextPoint(-1, -1);

			UndoBlockCollection ActionGroup = (UndoBlockCollection) this.UndoBuffer[UndoStep];
			UndoBlock undo = (UndoBlock) ActionGroup[0];
			for (int i = 0; i < ActionGroup.Count; i++)
			{
				undo = (UndoBlock) ActionGroup[i];

				switch (undo.Action)
				{
					case UndoAction.InsertRange:
						{
							InsertText(undo.Text, undo.Position.X, undo.Position.Y, false);
						}
						break;
					case UndoAction.DeleteRange:
						{
							TextRange r = GetRangeFromText(undo.Text, undo.Position.X, undo.Position.Y);
							DeleteRange(r, false);
						}
						break;
					default:
						break;
				}
			}

			TextRange ran = GetRangeFromText(undo.Text, undo.Position.X, undo.Position.Y);
			UndoStep++;
			this.ResetVisibleRows();
			OnUndoBufferChanged();
			return new TextPoint(ran.LastColumn, ran.LastRow);

		}

		public Word GetStartBracketWord(Word Start, Pattern End, Segment FindIn)
		{
			if (Start == null || Start.Pattern == null || Start.Segment == null)
				return null;

			int CurrentRow = Start.Row.Index;
			int FirstRow = FindIn.StartRow.Index;
			int x = Start.Index;
			int count = 0;
			while (CurrentRow >= FirstRow)
			{
				for (int i = x; i >= 0; i--)
				{
					Word w = this[CurrentRow][i];
					if (w.Segment == FindIn && w.Type == WordType.xtWord)
					{
						if (w.Pattern == Start.Pattern)
							count++;
						if (w.Pattern == End)
							count--;

						if (count == 0)
							return w;
					}
				}

				if (!Start.Pattern.IsMultiLineBracket)
					break;

				CurrentRow--;
				if (CurrentRow >= 0)
					x = this[CurrentRow].Count - 1;
			}
			return null;
		}


		public Word GetEndBracketWord(Word Start, Pattern End, Segment FindIn)
		{
			if (Start == null || Start.Pattern == null || Start.Segment == null)
				return null;

			int CurrentRow = Start.Row.Index;

			int LastRow = this.Count - 1;
			if (FindIn.EndRow != null)
				LastRow = FindIn.EndRow.Index;


			int x = Start.Index;
			int count = 0;
			while (CurrentRow <= LastRow)
			{
				for (int i = x; i < this[CurrentRow].Count; i++)
				{
					Word w = this[CurrentRow][i];
					if (w.Segment == FindIn && w.Type == WordType.xtWord)
					{
						if (w.Pattern == Start.Pattern)
							count++;
						if (w.Pattern == End)
							count--;

						if (count == 0)
							return w;
					}
				}

				if (!Start.Pattern.IsMultiLineBracket)
					break;

				CurrentRow++;
				x = 0;
			}
			return null;
		}


		/// <summary>
		/// 
		/// </summary>
		~SyntaxDocument()
		{
		}

		protected internal void OnApplyFormatRanges(Row row)
		{
			if (this.FormatRanges == null)
				return;


			if (!this.FormatRanges.RowContainsFormats(row) || row.RowState != RowState.AllParsed)
			{
				row.FormattedWords = row.mWords;
			}
			else
			{
				row.FormattedWords = new WordCollection();
				int i = 0;
				int l = 0;
				int x = 0;
				int ri = row.Index;
				foreach (char c in row.Text)
				{
					Word w = row[i];
					FormatRange fr = FormatRanges.MergeFormats(x, ri);
					Word wn = new Word();
					wn.Text = c.ToString();


					if (fr == null)
					{
						wn.Style = w.Style;
					}
					else
					{
						wn.Style = new TextStyle();
						if (w.Style == null)
							w.Style = new TextStyle();

						wn.Style.BackColor = (fr.BackColor == Color.Empty) ? w.Style.BackColor : fr.BackColor;
						wn.Style.ForeColor = (fr.ForeColor == Color.Empty) ? w.Style.ForeColor : fr.ForeColor;
						wn.InfoTip = fr.InfoTip;
						/*wn.Style.Bold = false;
						wn.Style.Italic = true;
						wn.Style.Underline = false;*/
						if (fr.WaveColor != Color.Empty)
						{
							wn.HasError = true;
							wn.ErrorColor = fr.WaveColor;
						}
					}
					wn.Type = w.Type;
					wn.Segment = w.Segment;
					row.FormattedWords.Add(wn);


					l++;
					if (l == row[i].Text.Length)
					{
						i++;
						l = 0;
					}
					x++;
				}

			}
		}
	}
}