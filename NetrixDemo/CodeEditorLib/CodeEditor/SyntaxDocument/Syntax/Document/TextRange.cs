using System;

namespace GuruComponents.CodeEditor.CodeEditor.Syntax
{
	/// <summary>
	/// A range of text
	/// </summary>
	public class TextRange
	{
		public event EventHandler Change = null;

		protected virtual void OnChange()
		{
			if (Change != null)
				Change(this, EventArgs.Empty);
		}

		/// <summary>
		/// The start row of the range
		/// </summary>

		#region PUBLIC PROPERTY FIRSTROW
		private int _FirstRow = 0;

		public int FirstRow
		{
			get { return _FirstRow; }
			set
			{
				_FirstRow = value;
				OnChange();
			}
		}

		#endregion		

		/// <summary>
		/// The start column of the range
		/// </summary>

		#region PUBLIC PROPERTY FIRSTCOLUMN
		private int _FirstColumn;

		public int FirstColumn
		{
			get { return _FirstColumn; }
			set
			{
				_FirstColumn = value;
				OnChange();
			}
		}

		#endregion

		/// <summary>
		/// The end row of the range
		/// </summary>

		#region PUBLIC PROPERTY LASTROW
		private int _LastRow = 0;

		public int LastRow
		{
			get { return _LastRow; }
			set
			{
				_LastRow = value;
				OnChange();
			}
		}

		#endregion

		/// <summary>
		/// The end column of the range
		/// </summary>

		#region PUBLIC PROPERTY LASTCOLUMN
		private int _LastColumn = 0;

		public int LastColumn
		{
			get { return _LastColumn; }
			set
			{
				_LastColumn = value;
				OnChange();
			}
		}

		#endregion

		public void SetBounds(int FirstColumn, int FirstRow, int LastColumn, int LastRow)
		{
			_FirstColumn = FirstColumn;
			_FirstRow = FirstRow;
			_LastColumn = LastColumn;
			_LastRow = LastRow;
			OnChange();
		}

		public TextRange()
		{
		}

		public TextRange(int FirstColumn, int FirstRow, int LastColumn, int LastRow)
		{
			_FirstColumn = FirstColumn;
			_FirstRow = FirstRow;
			_LastColumn = LastColumn;
			_LastRow = LastRow;
		}

	}
}