

using System;
using System.Diagnostics;
using GuruComponents.CodeEditor.CodeEditor.Syntax;

namespace GuruComponents.CodeEditor.CodeEditor
{
	/// <summary>
	/// Caret class used by the SyntaxBoxControl
	/// </summary>
	public sealed class Caret
	{
		/// <summary>
		/// Event fired when the carets position has changed.
		/// </summary>
		public event EventHandler Change = null;

		#region General Declarations

		// X Position of the caret (in logical units (eg. 1 tab = 5 chars)
		private int OldLogicalXPos = 0;

		/// <summary>
		/// The Position of the caret in Chars (Column and Row index)
		/// </summary>
		private TextPoint _Position;

		/// <summary>
		/// Used by the painter to determine if the caret should be rendered or not
		/// </summary>
		public bool Blink;

		// to what control does the caret belong??
		private EditViewControl Control;

		#endregion

		#region Constructor(s)

		/// <summary>
		/// Caret constructor
		/// </summary>
		/// <param name="control">The control that will use the caret</param>
		public Caret(EditViewControl control)
		{
			this.Position = new TextPoint(0, 0);
			Control = control;
		}

		#endregion

		#region Helpers

		private void RememberXPos()
		{
			OldLogicalXPos = LogicalPosition.X;
		}

		/// <summary>
		/// Confines the caret to a valid position within the active document
		/// </summary>
		public void CropPosition()
		{
			if (this.Position.X < 0)
				this.Position.X = 0;

			if (this.Position.Y >= Control.Document.Count)
				this.Position.Y = Control.Document.Count - 1;

			if (this.Position.Y < 0)
				this.Position.Y = 0;

			Row xtr = CurrentRow;

			if (this.Position.X > xtr.Text.Length && !Control.VirtualWhitespace)
				this.Position.X = xtr.Text.Length;
		}

		#endregion

		#region Movement Methods

		/// <summary>
		/// Moves the caret right one step.
		/// if the caret is placed at the last column of a row the caret will move down one row and be placed at the first column of that row.
		/// </summary>
		/// <param name="Select">True if a selection should be created from the current caret pos to the new pos</param>
		public void MoveRight(bool Select)
		{
			this.CropPosition();
			this.Position.X ++;

			if (this.CurrentRow.IsCollapsed)
			{
				if (this.Position.X > this.CurrentRow.Expansion_EndChar)
				{
					this.Position.Y = this.CurrentRow.Expansion_EndRow.Index;
					this.Position.X = this.CurrentRow.Expansion_EndRow.Expansion_StartChar;
					this.CropPosition();
				}
				RememberXPos();
				CaretMoved(Select);

			}
			else
			{
				Row xtr = CurrentRow;
				if (this.Position.X > xtr.Text.Length && !Control.VirtualWhitespace)
				{
					if (this.Position.Y < Control.Document.Count - 1)
					{
						this.MoveDown(Select);
						this.Position.X = 0;
						//this.Position.Y ++;
						this.CropPosition();
					}
					else
						this.CropPosition();
				}
				RememberXPos();
				CaretMoved(Select);
			}
		}

		/// <summary>
		/// Moves the caret up one row.
		/// </summary>
		/// <param name="Select">True if a selection should be created from the current caret pos to the new pos</param>
		public void MoveUp(bool Select)
		{
			this.CropPosition();
			int x = OldLogicalXPos;
			//error here
			
				if (this.CurrentRow != null && this.CurrentRow.PrevVisibleRow != null)
				{
					this.Position.Y = this.CurrentRow.PrevVisibleRow.Index;
					if (this.CurrentRow.IsCollapsed)
					{
						x = 0;
					}
				}
	
			 
				this.CropPosition();
				this.LogicalPosition = new TextPoint(x, this.Position.Y);
				this.CropPosition();
				CaretMoved(Select);
			 
		}

		/// <summary>
		/// Moves the caret up x rows
		/// </summary>
		/// <param name="rows">Number of rows the caret should be moved up</param>
		/// <param name="Select">True if a selection should be created from the current caret pos to the new pos</param>
		public void MoveUp(int rows, bool Select)
		{
			this.CropPosition();
			int x = OldLogicalXPos;
			try
			{
				int pos = this.CurrentRow.VisibleIndex;
				pos -= rows;
				if (pos < 0)
					pos = 0;
				Row r = this.Control.Document.VisibleRows[pos];
				pos = r.Index;


				this.Position.Y = pos;

//				for (int i=0;i<rows;i++)
//				{
//					this.Position.Y =  this.CurrentRow.PrevVisibleRow.Index;
//				}
				if (this.CurrentRow.IsCollapsed)
				{
					x = 0;
				}
			}
			catch
			{
			}
			this.CropPosition();
			this.LogicalPosition = new TextPoint(x, this.Position.Y);
			this.CropPosition();
			CaretMoved(Select);
		}

		/// <summary>
		/// Moves the caret down x rows.
		/// </summary>
		/// <param name="rows">The number of rows the caret should be moved down</param>
		/// <param name="Select">True if a selection should be created from the current caret pos to the new pos</param>
		public void MoveDown(int rows, bool Select)
		{
			int x = OldLogicalXPos;
			this.CropPosition();
			//this.Position.Y +=rows;
			try
			{
				int pos = this.CurrentRow.VisibleIndex;
				pos += rows;
				if (pos > this.Control.Document.VisibleRows.Count - 1)
					pos = this.Control.Document.VisibleRows.Count - 1;

				Row r = this.Control.Document.VisibleRows[pos];
				pos = r.Index;
				this.Position.Y = pos;

//				for (int i=0;i<rows;i++)
//				{
//					this.Position.Y =  this.CurrentRow.NextVisibleRow.Index;
//					
//				}
				if (this.CurrentRow.IsCollapsed)
				{
					x = 0;
				}
			}
			catch
			{
			}
			finally
			{
				this.CropPosition();
				this.LogicalPosition = new TextPoint(x, this.Position.Y);
				this.CropPosition();
				CaretMoved(Select);
			}
		}


		/// <summary>
		/// Moves the caret down one row.
		/// </summary>
		/// <param name="Select">True if a selection should be created from the current caret pos to the new pos</param>
        public void MoveDown(bool Select)
        {
            this.CropPosition();
            int x = OldLogicalXPos;
            //error here


            Row r = this.CurrentRow;
            Row r2 = r.NextVisibleRow;

            if (r2 != null)
            {
                this.Position.Y = r2.Index;
                if (this.CurrentRow.IsCollapsed)
                {
                    x = 0;
                }
            }

            this.CropPosition();
            this.LogicalPosition = new TextPoint(x, this.Position.Y);
            this.CropPosition();
            CaretMoved(Select);
        }

		/// <summary>
		/// Moves the caret left one step.
		/// if the caret is placed at the first column the caret will be moved up one line and placed at the last column of the row.
		/// </summary>
		/// <param name="Select">True if a selection should be created from the current caret pos to the new pos</param>
		public void MoveLeft(bool Select)
		{
			this.CropPosition();
			this.Position.X --;

			if (this.CurrentRow.IsCollapsedEndPart)
			{
				if (this.Position.X < this.CurrentRow.Expansion_StartChar)
				{
					if (this.CurrentRow.Expansion_StartRow.Index == -1)
						Debugger.Break();
					this.Position.Y = this.CurrentRow.Expansion_StartRow.Index;
					this.Position.X = this.CurrentRow.Expansion_StartRow.Expansion_EndChar;
					this.CropPosition();
				}
				RememberXPos();
				CaretMoved(Select);

			}
			else
			{
				if (this.Position.X < 0)
				{
					if (this.Position.Y > 0)
					{
						this.MoveUp(Select);
						this.CropPosition();
						Row xtr = CurrentRow;
						this.Position.X = xtr.Text.Length;
						if (this.CurrentRow.IsCollapsed)
						{
							this.Position.Y = this.CurrentRow.Expansion_EndRow.Index;
							this.Position.X = this.CurrentRow.Text.Length;
						}
					}
					else
						this.CropPosition();


				}
				RememberXPos();
				CaretMoved(Select);
			}
		}


		/// <summary>
		/// Moves the caret to the first non whitespace column at the active row
		/// </summary>
		/// <param name="Select">True if a selection should be created from the current caret pos to the new pos</param>
		public void MoveHome(bool Select)
		{
			this.CropPosition();
			if (this.CurrentRow.IsCollapsedEndPart)
			{
				this.Position.Y = this.CurrentRow.Expansion_StartRow.Index;
				this.MoveHome(Select);
			}
			else
			{
				int i = CurrentRow.GetLeadingWhitespace().Length;
				if (this.Position.X == i)
				{
					this.Position.X = 0;
				}
				else
				{
					this.Position.X = i;
				}
				RememberXPos();
				CaretMoved(Select);
			}
		}

		/// <summary>
		/// Moves the caret to the end of a row ignoring any whitespace characters at the end of the row
		/// </summary>
		/// <param name="Select">True if a selection should be created from the current caret pos to the new pos</param>
		public void MoveEnd(bool Select)
		{
			if (this.CurrentRow.IsCollapsed)
			{
				this.Position.Y = this.CurrentRow.Expansion_EndRow.Index;
				this.MoveEnd(Select);
			}
			else
			{
				this.CropPosition();
				Row xtr = CurrentRow;
				this.Position.X = xtr.Text.Length;
				RememberXPos();
				CaretMoved(Select);
			}
		}

		public void CaretMoved(bool Select)
		{
			Control.ScrollIntoView();
			if (!Select)
				Control.Selection.ClearSelection();
			else
				Control.Selection.MakeSelection();
		}

		/// <summary>
		/// Moves the caret to the first column of the active row
		/// </summary>
		/// <param name="Select">True if a selection should be created from the current caret pos to the new pos</param>
		public void MoveAbsoluteHome(bool Select)
		{
			this.Position.X = 0;
			this.Position.Y = 0;
			RememberXPos();
			CaretMoved(Select);
		}

		/// <summary>
		/// Moves the caret to the absolute end of the active row
		/// </summary>
		/// <param name="Select">True if a selection should be created from the current caret pos to the new pos</param>
		public void MoveAbsoluteEnd(bool Select)
		{
			this.Position.X = Control.Document[Control.Document.Count - 1].Text.Length;
			;
			this.Position.Y = Control.Document.Count - 1;
			RememberXPos();
			CaretMoved(Select);
		}

		#endregion

		#region Get Related info from Caret Position

		/// <summary>
		/// Gets the word that the caret is placed on.
		/// This only applies if the active row is fully parsed.
		/// </summary>
		/// <returns>a Word object from the active row</returns>
		public Word CurrentWord
		{
			get { return this.Control.Document.GetWordFromPos(this.Position); }
		}

		/// <summary>
		/// Gets the word that the caret is placed on.
		/// This only applies if the active row is fully parsed.
		/// </summary>
		/// <returns>a Word object from the active row</returns>
		public Segment CurrentSegment()
		{
			return this.Control.Document.GetSegmentFromPos(this.Position);
		}

		/// <summary>
		/// Returns the row that the caret is placed on
		/// </summary>
		/// <returns>a Row object from the active document</returns>
		public Row CurrentRow
		{
			get { return Control.Document[this.Position.Y]; }
		}

		#endregion

		#region Set Position Methods/Props

		/// <summary>
		/// Sets the position of the caret
		/// </summary>
		/// <param name="pos">Point containing the new x and y positions</param>
		public void SetPos(TextPoint pos)
		{
			this.Position = pos;
			RememberXPos();
		}

		/// <summary>
		/// Gets or Sets the Logical position of the caret.
		/// </summary>
		public TextPoint LogicalPosition
		{
			get
			{
				if (this.Position.X < 0)
					return new TextPoint(0, this.Position.Y);

				Row xtr = this.CurrentRow;
				int x = 0;
				if (xtr == null)
					return new TextPoint(0, 0);

				int max = xtr.Text.Length;

				int Padd = Math.Max(this.Position.X - xtr.Text.Length, 0);
				string PaddStr = new String(' ', Padd);
				string TotStr = xtr.Text + PaddStr;

				foreach (char c in TotStr.ToCharArray(0, this.Position.X))
				{
					if (c == '\t')
					{
						x += Control.TabSize - (x%Control.TabSize);
					}
					else
					{
						x++;
					}
				}
				return new TextPoint(x, this.Position.Y);
			}
			set
			{
				Row xtr = this.CurrentRow;
				int x = 0;
				int xx = 0;
				if (value.X > 0)
				{
					char[] chars = xtr.Text.ToCharArray();

					int i = 0;

					while (x < value.X)
					{
						char c;
						if (i < chars.Length)
							c = chars[i];
						else
							c = ' ';
						xx++;
						if (c == '\t')
						{
							x += Control.TabSize - (x%Control.TabSize);
						}
						else
						{
							x++;
						}
						i++;
					}
				}


				this.Position.Y = value.Y;
				this.Position.X = xx;
			}


		}

		#endregion

		/// <summary>
		/// Gets or Sets the position of the caret.
		/// </summary>
		public TextPoint Position
		{
			get { return _Position; }
			set
			{
				_Position = value;
				_Position.Change += new EventHandler(this.PositionChange);
				OnChange();
			}
		}

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