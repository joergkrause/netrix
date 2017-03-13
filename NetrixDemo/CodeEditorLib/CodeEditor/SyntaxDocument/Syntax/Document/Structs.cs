using System;

namespace GuruComponents.CodeEditor.CodeEditor.Syntax
{
	/// <summary>
	/// Class representing a point in a text.
	/// where x is the column and y is the row.
	/// </summary>
	public class TextPoint
	{
		private int x = 0;
		private int y = 0;

		/// <summary>
		/// Event fired when the X or Y property has changed.
		/// </summary>
		public event EventHandler Change = null;

		private void OnChange()
		{
			if (Change != null)
				Change(this, new EventArgs());
		}

		/// <summary>
		/// 
		/// </summary>
		public TextPoint()
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="X"></param>
		/// <param name="Y"></param>
		public TextPoint(int X, int Y)
		{
			this.X = X;
			this.Y = Y;
		}

		/// <summary>
		/// 
		/// </summary>
		public int X
		{
			get { return x; }
			set
			{
				x = value;
				OnChange();
			}

		}

		/// <summary>
		/// 
		/// </summary>
		public int Y
		{
			get { return y; }
			set
			{
				y = value;
				OnChange();
			}
		}


	}

}