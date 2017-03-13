using System;
using System.Drawing;
//using Puzzle.Collections;
using System.Collections;

namespace GuruComponents.CodeEditor.CodeEditor.Syntax
{
    public class FormatRangeCollection : BaseCollection
	{
		#region PUBLIC PROPERTY DOCUMENT

		private SyntaxDocument _Document;

		internal SyntaxDocument Document
		{
			get { return _Document; }
		}

		#endregion

		public FormatRange this[int index]
		{
			get { return this.List[index] as FormatRange; }
		}

		public FormatRangeCollection(SyntaxDocument document)
		{
			_Document = document;
		}

		public int Add(FormatRange item)
		{
			item.Document = this.Document;
			item.Apply();
			return this.List.Add(item);
		}

		public void Remove(FormatRange item)
		{
			this.List.Remove(item);
		}

		public bool RowContainsFormats(Row row)
		{
			foreach (FormatRange frang in this)
			{
				if (frang.Bounds.FirstRow <= row.Index && frang.Bounds.LastRow >= row.Index)
					return true;
			}

			return false;
		}

		public FormatRange MergeFormats(int x, int y)
		{
			FormatRange fr = null;
			foreach (FormatRange frang in this)
			{
				if (frang.Contains(x, y) != 0)
					continue;

				if (frang.BackColor != Color.Empty)
				{
					if (fr == null)
						fr = new FormatRange();

					fr.BackColor = frang.BackColor;
				}

				if (frang.ForeColor != Color.Empty)
				{
					if (fr == null)
						fr = new FormatRange();

					fr.ForeColor = frang.ForeColor;
				}

				if (frang.WaveColor != Color.Empty)
				{
					if (fr == null)
						fr = new FormatRange();

					fr.WaveColor = frang.WaveColor;
				}

				if (frang.InfoTip.Length > 0)
				{
					if (fr == null)
						fr = new FormatRange();

					if (fr.InfoTip.Length > 0)
					{
						fr.InfoTip += Environment.NewLine + Environment.NewLine + frang.InfoTip;
					}
					else
					{
						fr.InfoTip = frang.InfoTip;
					}
				}
			}

			return fr;
		}


		public void Expand(int xPos, int yPos, string Text)
		{
			if (this.RowContainsFormats(this.Document[yPos]))
			{
				string tmp = Text.Replace(Environment.NewLine, "\n");
				int l = tmp.Length;


				string[] tmplines = tmp.Split('\n');

				foreach (FormatRange fr in this)
				{
					int res = fr.Contains2(xPos, yPos);
					if (res == 0)
					{
						fr.Bounds.LastRow += tmplines.Length - 1;
						if (fr.Bounds.LastRow == yPos + tmplines.Length - 1)
							fr.Bounds.LastColumn += tmplines[tmplines.Length - 1].Length;
					}
					if (res == -1)
					{
						fr.Bounds.FirstRow += tmplines.Length - 1;
						fr.Bounds.LastRow += tmplines.Length - 1;
						if (fr.Bounds.FirstRow == yPos + tmplines.Length - 1)
						{
							if (tmplines.Length > 1)
								fr.Bounds.FirstColumn -= xPos;

							fr.Bounds.FirstColumn += tmplines[tmplines.Length - 1].Length;
						}
					}
					if (res == 1)
					{
						//ignore
					}

				}
			}
		}

		public void Shrink(TextRange Range)
		{
			string Text = this.Document.GetRange(Range);
			string tmp = Text.Replace(Environment.NewLine, "\n");
			int l = tmp.Length;
			string[] tmplines = tmp.Split('\n');

			foreach (FormatRange fr in this)
			{
				int res = fr.Contains(Range.FirstColumn, Range.FirstRow);
				int res2 = fr.Contains(Range.LastColumn, Range.LastRow);

				int rows = Range.LastRow - Range.FirstRow;
				if (res == -1 && res2 == -1)
				{
					fr.Bounds.FirstRow -= rows;
					fr.Bounds.LastRow -= rows;
					if (fr.Bounds.FirstRow == Range.FirstRow + tmplines.Length - 1)
					{
						if (tmplines.Length > 1)
							fr.Bounds.FirstColumn -= Range.FirstColumn;

						fr.Bounds.FirstColumn -= tmplines[tmplines.Length - 1].Length;
					}
					else if (fr.Bounds.FirstRow == Range.FirstRow)
					{
						if (tmplines.Length > 1)
							fr.Bounds.FirstColumn += Range.FirstColumn;

						fr.Bounds.FirstColumn -= tmplines[tmplines.Length - 1].Length;
					}
				}
				if (res == -1 && res2 == 0)
				{
					fr.Bounds.FirstRow -= rows;
					fr.Bounds.LastRow -= rows;
					fr.Bounds.FirstColumn = Range.FirstColumn;
				}
				if (res == 0 && res2 == 0)
				{
					fr.Bounds.LastRow -= rows;
					if (fr.Bounds.LastRow == Range.FirstRow)
					{
						if (rows == 0)
							fr.Bounds.LastColumn -= tmplines[tmplines.Length - 1].Length;
						else
							fr.Bounds.LastColumn = Range.FirstColumn + fr.Bounds.LastColumn;
					}
				}
				if (res == 0 && res2 == 1)
				{
					//delete end of range
					fr.Bounds.LastColumn = Range.FirstColumn - 1;
					fr.Bounds.LastRow = Range.FirstRow;
				}

				if (res == -1 && res2 == 1)
				{
					//delete this range
					fr.Bounds.SetBounds(-1, -1, -1, -1);
				}


				if (res == 1)
				{
					//ignore
				}
			}
		}
	}
}