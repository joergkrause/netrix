using System.ComponentModel;
using System.Drawing;
using System.Drawing.Printing;

namespace GuruComponents.CodeEditor.CodeEditor.Syntax
{
	/// <summary>
	/// Printer document class.
	/// </summary>
	/// <example >
	/// 
	/// 
	/// <b>Print the content of a SyntaxDocument:</b>
	/// <code>
	/// SourceCodePrintDocument PrintDoc=new SourceCodePrintDocument(MySyntaxDocument);
	///
	///	PrintDialog1.Document =PrintDoc;
	///	if (PrintDialog1.ShowDialog ()==DialogResult.OK)
	///		PrintDoc.Print ();
	/// </code>
	/// <hr/>
	/// <b>Print Preview the content of a SyntaxDocument</b>
	/// <code>
	/// SourceCodePrintDocument PrintDoc=new SourceCodePrintDocument(MySyntaxDocument);
	/// PrintPreviewDialog1.Document = PrintDoc
	/// PrintPreviewDialog1.ShowDialog ();
	/// </code>
	/// </example>
	[ToolboxItem(true)]
	public class SourceCodePrintDocument : PrintDocument
	{
		private Font fontNormal = null;
		private Font fontBreak = null;
		private int RowIndex = 0;


		private SyntaxDocument _Document = null;
		private RowCollection rc = null;

		public SyntaxDocument Document
		{
			get { return _Document; }
			set { _Document = value; }

		}

		public SourceCodePrintDocument() : base()
		{
		}

		public SourceCodePrintDocument(SyntaxDocument document) : base()
		{
			this.Document = document;
		}

		//Override OnBeginPrint to set up the font we are going to use
		protected override void OnBeginPrint(PrintEventArgs ev)
		{
			base.OnBeginPrint(ev);
			fontNormal = new Font("Courier new", 8, FontStyle.Regular);
			fontBreak = new Font("Symbol", 8, FontStyle.Bold);
//			fontBold						= new Font("Arial", 10,FontStyle.Bold);
//			fontItalic						= new Font("Arial", 10,FontStyle.Italic);
//			fontBoldItalic					= new Font("Arial", 10,FontStyle.Bold | FontStyle.Italic);
//			fontUnderline					= new Font("Arial", 10,FontStyle.Underline);
//			fontBoldUnderline				= new Font("Arial", 10,FontStyle.Bold | FontStyle.Underline);
//			fontItalicUnderline				= new Font("Arial", 10,FontStyle.Italic | FontStyle.Underline);
//			fontBoldItalicUnderline			= new Font("Arial", 10,FontStyle.Bold | FontStyle.Italic | FontStyle.Underline);
			RowIndex = 0;


		}

		//Override the OnPrintPage to provide the printing logic for the document
		protected override void OnPrintPage(PrintPageEventArgs ev)
		{
			float lpp = 0;
			float yPos = 0;
			int count = 0;
			float leftMargin = ev.MarginBounds.Left;
			float rightMargin = ev.MarginBounds.Right;
			float topMargin = ev.MarginBounds.Top;
			//ev.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;

			if (rc == null)
			{
				Document.ParseAll();
				Document.ParseAll(true);


				rc = new RowCollection();
				foreach (Row r in Document)
				{
					bool hasbreak = false;
					float x = leftMargin;
					Row newRow = new Row();
					rc.Add(newRow);
					foreach (Word w in r)
					{
						Font f = fontNormal;
						if (w.Style != null)
						{
							FontStyle fs = 0;

							if (w.Style.Bold)
								fs |= FontStyle.Bold;

							if (w.Style.Italic)
								fs |= FontStyle.Italic;

							if (w.Style.Underline)
								fs |= FontStyle.Underline;

							f = new Font("Courier new", 8, fs);
						}
						SizeF sf = ev.Graphics.MeasureString(w.Text, f);
						if (x + sf.Width > rightMargin)
						{
							char chr = (char) 0xbf;
							Word br = new Word();
							br.Text = chr + "";
							br.InfoTip = "break char";
							newRow.Add(br);
							hasbreak = true;


							newRow = new Row();
							rc.Add(newRow);
							x = leftMargin;
						}
						x += sf.Width;
						newRow.Add(w);


					}
					if (hasbreak)
					{
						rc.Add(new Row());
					}
				}
			}
			//------------------------------------------------------

			base.OnPrintPage(ev);


			lpp = ev.MarginBounds.Height/fontNormal.GetHeight(ev.Graphics);


			while (count < lpp && (RowIndex < rc.Count))
			{
				float x = leftMargin;
				yPos = topMargin + (count*fontNormal.GetHeight(ev.Graphics));

				Row r = rc[RowIndex];

				foreach (Word w in r)
				{
					if (w.InfoTip != null && w.InfoTip == "break char")
					{
						ev.Graphics.DrawString(w.Text, fontBreak, Brushes.Black, x, yPos, new StringFormat());
					}
					else
					{
						SizeF sf = ev.Graphics.MeasureString(w.Text, fontNormal);

						if (w.Text != null && (".,:;".IndexOf(w.Text) >= 0))
						{
							sf.Width = 6;
							x -= 4;
						}
						if (w.Text == "\t")
						{
							sf.Width = ev.Graphics.MeasureString("...", fontNormal).Width;
						}


						Color c = Color.Black;
						Font f = fontNormal;
						if (w.Style != null)
						{
							c = w.Style.ForeColor;
							FontStyle fs = 0;

							if (w.Style.Bold)
								fs |= FontStyle.Bold;

							if (w.Style.Italic)
								fs |= FontStyle.Italic;

							if (w.Style.Underline)
								fs |= FontStyle.Underline;

							f = new Font("Courier new", 8, fs);

							if (!w.Style.Transparent)
							{
								Color bg = w.Style.BackColor;
								ev.Graphics.FillRectangle(new SolidBrush(bg), x, yPos, sf.Width, fontNormal.GetHeight(ev.Graphics));


							}

						}

						c = Color.FromArgb(c.R, c.G, c.B);


						ev.Graphics.DrawString(w.Text, f, new SolidBrush(c), x, yPos, new StringFormat());
						x += sf.Width;
					}
				}

				count++;
				RowIndex++;
			}

			//If we have more lines then print another page
			if (RowIndex < rc.Count)
				ev.HasMorePages = true;
			else
				ev.HasMorePages = false;
		}

	}

}