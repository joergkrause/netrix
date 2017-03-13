using System;
using System.Drawing;

namespace GuruComponents.CodeEditor.CodeEditor.Syntax
{
	/// <summary>
	/// Format ranges can be applied to a syntaxdocument to mark sections such as breakpoints or errors
	/// </summary>
	public class FormatRange
	{
		#region PUBLIC PROPERTY BACKCOLOR

		private Color _BackColor = Color.Empty;

		public Color BackColor
		{
			get { return _BackColor; }
			set
			{
				_BackColor = value;
				Apply();
			}
		}

		#endregion

		#region PUBLIC PROPERTY FORECOLOR

		private Color _ForeColor = Color.Empty;

		public Color ForeColor
		{
			get { return _ForeColor; }
			set
			{
				_ForeColor = value;
				Apply();
			}
		}

		#endregion

		#region PUBLIC PROPERTY WAVECOLOR

		private Color _WaveColor = Color.Empty;

		public Color WaveColor
		{
			get { return _WaveColor; }
			set
			{
				_WaveColor = value;
				Apply();
			}
		}

		#endregion

		#region PUBLIC PROPERTY INFOTIP

		private string _InfoTip = "";

		public string InfoTip
		{
			get { return _InfoTip; }
			set
			{
				_InfoTip = value;
				Apply();
			}
		}

		#endregion

		#region PUBLIC PROPERTY BOUNDS

		private TextRange _Bounds;
		private TextRange _OldBounds = new TextRange();

		public TextRange Bounds
		{
			get { return _Bounds; }
			set { _Bounds = value; }
		}

		#endregion

		#region PUBLIC PROPERTY TAG

		private object _Tag;

		public object Tag
		{
			get { return _Tag; }
			set { _Tag = value; }
		}

		#endregion

		#region PUBLIC PROPERTY DOCUMENT

		private SyntaxDocument _Document;

		internal SyntaxDocument Document
		{
			get { return _Document; }
			set { _Document = value; }
		}

		#endregion

		public FormatRange()
		{
			this.Bounds = new TextRange();
			this.Bounds.Change += new EventHandler(this.BoundsChanged);
		}

		public FormatRange(TextRange Bounds, Color ForeColor, Color BackColor)
		{
			this.BackColor = BackColor;
			this.ForeColor = ForeColor;
			this.Bounds = Bounds;
			this.Bounds.Change += new EventHandler(this.BoundsChanged);
		}

		public FormatRange(TextRange Bounds, Color ForeColor, Color BackColor, Color WaveColor)
		{
			this.BackColor = BackColor;
			this.ForeColor = ForeColor;
			this.WaveColor = WaveColor;
			this.Bounds = Bounds;
			this.Bounds.Change += new EventHandler(this.BoundsChanged);
		}

		public FormatRange(TextRange Bounds, Color WaveColor)
		{
			this.WaveColor = WaveColor;
			this.Bounds = Bounds;
			this.Bounds.Change += new EventHandler(this.BoundsChanged);
		}

		public int Contains(TextPoint tp)
		{
			return this.Contains(tp.X, tp.Y);
		}

		public int Contains(int x, int y)
		{
			if (y < this.Bounds.FirstRow)
				return -1;

			if (y > this.Bounds.LastRow)
				return 1;

			if (y == this.Bounds.FirstRow && x < this.Bounds.FirstColumn)
				return -1;

			if (y == this.Bounds.LastRow && x > this.Bounds.LastColumn)
				return 1;

			return 0;
		}

		public int Contains2(TextPoint tp)
		{
			return this.Contains2(tp.X, tp.Y);
		}

		public int Contains2(int x, int y)
		{
			if (y < this.Bounds.FirstRow)
				return -1;

			if (y > this.Bounds.LastRow)
				return 1;

			if (y == this.Bounds.FirstRow && x <= this.Bounds.FirstColumn)
				return -1;

			if (y == this.Bounds.LastRow && x > this.Bounds.LastColumn)
				return 1;

			return 0;
		}

		public void Apply()
		{
			if (this.Document == null)
				return;

			ApplyOld();
			for (int i = this.Bounds.FirstRow; i <= this.Bounds.LastRow; i++)
			{
				if (i < 0 || i >= this.Document.Count)
					return;

				Row r = this.Document[i];

				if (r == null)
					return;

				if (r.RowState == RowState.AllParsed)
				{
					r.AddToParseQueue();
				}
			}
		}

		public void ApplyOld()
		{
			for (int i = this._OldBounds.FirstRow; i <= this._OldBounds.LastRow; i++)
			{
				if (i < 0 || i >= this.Document.Count)
					return;

				Row r = this.Document[i];

				if (r == null)
					return;

				if (r.RowState == RowState.AllParsed)
				{
					r.AddToParseQueue();
				}
			}
		}

		protected virtual void BoundsChanged(object s, EventArgs e)
		{
			Apply();
			_OldBounds.FirstColumn = _Bounds.FirstColumn;
			_OldBounds.LastColumn = _Bounds.LastColumn;
			_OldBounds.FirstRow = _Bounds.FirstRow;
			_OldBounds.LastRow = _Bounds.LastRow;
		}
	}
}