

using System;
using GuruComponents.CodeEditor.CodeEditor.Syntax;
using System.ComponentModel;

namespace GuruComponents.CodeEditor.CodeEditor
{
	/// <summary>
	/// Selection class used by the SyntaxBoxControl
	/// </summary>
   
	public class Selection
	{
		/// <summary>
		/// Event fired when the selection has changed.
		/// </summary>
		public event EventHandler Change = null;

		#region Instance constructors

		/// <summary>
		/// Selection Constructor.
		/// </summary>
		/// <param name="control">Control that will use this selection</param>
		public Selection(EditViewControl control)
		{
			Control = control;
			this.Bounds = new TextRange();
		}

		#endregion Instance constructors

		#region Public instance properties

		/// <summary>
		/// Gets the text of the active selection
		/// </summary>
		public String Text
		{
			get
			{
				if (!this.IsValid)
				{
					return "";
				}
				else
				{
					return Control.Document.GetRange(this.LogicalBounds);
				}
			}
			set
			{
				if (this.Text == value) return;

				//selection text bug fix 
				//
				//selection gets too short if \n is used instead of newline
				string tmp = value.Replace(Environment.NewLine, "\n");
				tmp = tmp.Replace("\n", Environment.NewLine);
				value = tmp;
				//---


				TextPoint oCaretPos = Control.Caret.Position;
				int nCaretX = oCaretPos.X;
				int nCaretY = oCaretPos.Y;
				this.Control.Document.StartUndoCapture();
				this.DeleteSelection();
				this.Control.Document.InsertText(value, oCaretPos.X, oCaretPos.Y);
				this.SelLength = value.Length;
				if (nCaretX != oCaretPos.X || nCaretY != oCaretPos.Y)

				{
					this.Control.Caret.Position = new TextPoint(this.Bounds.LastColumn, this.Bounds.LastRow);
				}

				this.Control.Document.EndUndoCapture();
				this.Control.Document.InvokeChange();
			}
		}

		/// <summary>
		/// Returns the normalized positions of the selection.
		/// Swapping start and end values if the selection is reversed.
		/// </summary>
		public TextRange LogicalBounds
		{
			get
			{
				TextRange r = new TextRange();
				if (this.Bounds.FirstRow < this.Bounds.LastRow)
				{
					return this.Bounds;
				}
				else if (this.Bounds.FirstRow == this.Bounds.LastRow && this.Bounds.FirstColumn < this.Bounds.LastColumn)
				{
					return this.Bounds;
				}
				else
				{
					r.FirstColumn = this.Bounds.LastColumn;
					r.FirstRow = this.Bounds.LastRow;
					r.LastColumn = this.Bounds.FirstColumn;
					r.LastRow = this.Bounds.FirstRow;
					return r;
				}
			}
		}

		/// <summary>
		/// Returns true if the selection contains One or more chars
		/// </summary>
		public bool IsValid
		{
			get
			{
				return (this.LogicalBounds.FirstColumn != this.LogicalBounds.LastColumn ||
					this.LogicalBounds.FirstRow != this.LogicalBounds.LastRow);
			}
		}

		/// <summary>
		/// gets or sets the length of the selection in chars
		/// </summary>
		public int SelLength
		{
			get
			{
				TextPoint p1 = new TextPoint(this.Bounds.FirstColumn, this.Bounds.FirstRow);
				TextPoint p2 = new TextPoint(this.Bounds.LastColumn, this.Bounds.LastRow);
				int i1 = this.Control.Document.PointToIntPos(p1);
				int i2 = this.Control.Document.PointToIntPos(p2);
				return i2 - i1;
			}
			set { this.SelEnd = this.SelStart + value; }
		}

		/// <summary>
		/// Gets or Sets the Selection end as an index in the document text.
		/// </summary>
		public int SelEnd
		{
			get
			{
				TextPoint p = new TextPoint(this.Bounds.LastColumn, this.Bounds.LastRow);
				return this.Control.Document.PointToIntPos(p);
			}
			set
			{
				TextPoint p = this.Control.Document.IntPosToPoint(value);
				this.Bounds.LastColumn = p.X;
				this.Bounds.LastRow = p.Y;
			}
		}


		/// <summary>
		/// Gets or Sets the Selection start as an index in the document text.
		/// </summary>
		public int SelStart
		{
			get
			{
				TextPoint p = new TextPoint(this.Bounds.FirstColumn, this.Bounds.FirstRow);
				return this.Control.Document.PointToIntPos(p);
			}
			set
			{
				TextPoint p = this.Control.Document.IntPosToPoint(value);
				this.Bounds.FirstColumn = p.X;
				this.Bounds.FirstRow = p.Y;
			}
		}

		/// <summary>
		/// Gets or Sets the logical Selection start as an index in the document text.
		/// </summary>
		public int LogicalSelStart
		{
			get
			{
				TextPoint p = new TextPoint(this.LogicalBounds.FirstColumn, this.LogicalBounds.FirstRow);
				return this.Control.Document.PointToIntPos(p);
			}
			set
			{
				TextPoint p = this.Control.Document.IntPosToPoint(value);
				this.Bounds.FirstColumn = p.X;
				this.Bounds.FirstRow = p.Y;
			}
		}

		#endregion Public instance properties

		#region Public instance methods

		/// <summary>
		/// Indent the active selection one step.
		/// </summary>
		public void Indent()
		{
			if (!this.IsValid)
				return;

			Row xtr = null;
			UndoBlockCollection ActionGroup = new UndoBlockCollection();
			for (int i = this.LogicalBounds.FirstRow; i <= this.LogicalBounds.LastRow; i++)
			{
				xtr = Control.Document[i];
				xtr.Text = "\t" + xtr.Text;
				UndoBlock b = new UndoBlock();
				b.Action = UndoAction.InsertRange;
				b.Text = "\t";
				b.Position.X = 0;
				b.Position.Y = i;
				ActionGroup.Add(b);
			}
			if (ActionGroup.Count > 0)
				Control.Document.AddToUndoList(ActionGroup);
			this.Bounds = this.LogicalBounds;
			this.Bounds.FirstColumn = 0;
			this.Bounds.LastColumn = xtr.Text.Length;
			Control.Caret.Position.X = this.LogicalBounds.LastColumn;
			Control.Caret.Position.Y = this.LogicalBounds.LastRow;
		}

		/// <summary>
		/// Outdent the active selection one step
		/// </summary>
		public void Outdent()
		{
			if (!this.IsValid)
				return;

			Row xtr = null;
			UndoBlockCollection ActionGroup = new UndoBlockCollection();
			for (int i = this.LogicalBounds.FirstRow; i <= this.LogicalBounds.LastRow; i++)
			{
				xtr = Control.Document[i];
				UndoBlock b = new UndoBlock();
				b.Action = UndoAction.DeleteRange;
				b.Position.X = 0;
				b.Position.Y = i;
				ActionGroup.Add(b);
				string s = xtr.Text;
				if (s.StartsWith("\t"))
				{
					b.Text = s.Substring(0, 1);
					s = s.Substring(1);
				}
				if (s.StartsWith("    "))
				{
					b.Text = s.Substring(0, 4);
					s = s.Substring(4);
				}
				xtr.Text = s;
			}
			if (ActionGroup.Count > 0)
				Control.Document.AddToUndoList(ActionGroup);
			this.Bounds = this.LogicalBounds;
			this.Bounds.FirstColumn = 0;
			this.Bounds.LastColumn = xtr.Text.Length;
			Control.Caret.Position.X = this.LogicalBounds.LastColumn;
			Control.Caret.Position.Y = this.LogicalBounds.LastRow;
		}


		public void Indent(string Pattern)
		{
			if (!this.IsValid)
				return;

			Row xtr = null;
			UndoBlockCollection ActionGroup = new UndoBlockCollection();
			for (int i = this.LogicalBounds.FirstRow; i <= this.LogicalBounds.LastRow; i++)
			{
				xtr = Control.Document[i];
				xtr.Text = Pattern + xtr.Text;
				UndoBlock b = new UndoBlock();
				b.Action = UndoAction.InsertRange;
				b.Text = Pattern;
				b.Position.X = 0;
				b.Position.Y = i;
				ActionGroup.Add(b);
			}
			if (ActionGroup.Count > 0)
				Control.Document.AddToUndoList(ActionGroup);
			this.Bounds = this.LogicalBounds;
			this.Bounds.FirstColumn = 0;
			this.Bounds.LastColumn = xtr.Text.Length;
			Control.Caret.Position.X = this.LogicalBounds.LastColumn;
			Control.Caret.Position.Y = this.LogicalBounds.LastRow;
		}

		/// <summary>
		/// Outdent the active selection one step
		/// </summary>
		public void Outdent(string Pattern)
		{
			if (!this.IsValid)
				return;

			Row xtr = null;
			UndoBlockCollection ActionGroup = new UndoBlockCollection();
			for (int i = this.LogicalBounds.FirstRow; i <= this.LogicalBounds.LastRow; i++)
			{
				xtr = Control.Document[i];
				UndoBlock b = new UndoBlock();
				b.Action = UndoAction.DeleteRange;
				b.Position.X = 0;
				b.Position.Y = i;
				ActionGroup.Add(b);
				string s = xtr.Text;
				if (s.StartsWith(Pattern))
				{
					b.Text = s.Substring(0, Pattern.Length);
					s = s.Substring(Pattern.Length);
				}
				xtr.Text = s;
			}
			if (ActionGroup.Count > 0)
				Control.Document.AddToUndoList(ActionGroup);
			this.Bounds = this.LogicalBounds;
			this.Bounds.FirstColumn = 0;
			this.Bounds.LastColumn = xtr.Text.Length;
			Control.Caret.Position.X = this.LogicalBounds.LastColumn;
			Control.Caret.Position.Y = this.LogicalBounds.LastRow;
		}

		/// <summary>
		/// Delete the active selection.
		/// <seealso cref="ClearSelection"/>
		/// </summary>
		public void DeleteSelection()
		{
			TextRange r = this.LogicalBounds;

			int x = r.FirstColumn;
			int y = r.FirstRow;
			Control.Document.DeleteRange(r);
			Control.Caret.Position.X = x;
			Control.Caret.Position.Y = y;
			ClearSelection();
			Control.ScrollIntoView();
		}

		/// <summary>
		/// Clear the active selection
		/// <seealso cref="DeleteSelection"/>
		/// </summary>
		public void ClearSelection()
		{
			Bounds.FirstColumn = Control.Caret.Position.X;
			Bounds.FirstRow = Control.Caret.Position.Y;
			Bounds.LastColumn = Control.Caret.Position.X;
			Bounds.LastRow = Control.Caret.Position.Y;
		}

		/// <summary>
		/// Make a selection from the current selection start to the position of the caret
		/// </summary>
		public void MakeSelection()
		{
			Bounds.LastColumn = Control.Caret.Position.X;
			Bounds.LastRow = Control.Caret.Position.Y;
		}

		/// <summary>
		/// Select all text.
		/// </summary>
		public void SelectAll()
		{
			Bounds.FirstColumn = 0;
			Bounds.FirstRow = 0;
			Bounds.LastColumn = Control.Document[Control.Document.Count - 1].Text.Length;
			Bounds.LastRow = Control.Document.Count - 1;
			Control.Caret.Position.X = Bounds.LastColumn;
			Control.Caret.Position.Y = Bounds.LastRow;
			Control.ScrollIntoView();
		}

		#endregion Public instance methods

		#region Public instance fields

		/// <summary>
		/// The bounds of the selection
		/// </summary>
		/// 
		private TextRange _Bounds;

		public TextRange Bounds
		{
			get { return _Bounds; }
			set
			{
				if (_Bounds != null)
				{
					_Bounds.Change -= new EventHandler(this.Bounds_Change);
				}

				_Bounds = value;
				_Bounds.Change += new EventHandler(this.Bounds_Change);
				OnChange();
			}
		}

		private void Bounds_Change(object s, EventArgs e)
		{
			OnChange();
		}

		#endregion Public instance fields

		#region Protected instance fields

		private EditViewControl Control;

		#endregion Protected instance fields

		private void PositionChange(object s, EventArgs e)
		{
			OnChange();
		}

		private void OnChange()
		{
			if (Change != null)
				Change(this, null);

		}
	}
}