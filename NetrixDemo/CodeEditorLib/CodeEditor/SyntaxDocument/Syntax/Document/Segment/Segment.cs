namespace GuruComponents.CodeEditor.CodeEditor.Syntax
{
	/// <summary>
	/// 
	/// </summary>
	public class Segment
	{
		/// <summary>
		/// The owner BlockType
		/// </summary>
		public BlockType BlockType;

		/// <summary>
		/// The parent segment
		/// </summary>
		public Segment Parent;

		/// <summary>
		/// The depth of this segment in the segment hirarchy
		/// </summary>
		public int Depth = 0;

		/// <summary>
		/// The row on which the segment starts
		/// </summary>
		public Row StartRow;

		/// <summary>
		/// The word that starts this segment
		/// </summary>
		public Word StartWord;

		/// <summary>
		/// The row that the segment ends on
		/// </summary>
		public Row EndRow;

		/// <summary>
		/// The word that ends this segment
		/// </summary>
		public Word EndWord;

		/// <summary>
		/// Gets or Sets if this segment is expanded
		/// </summary>
		public bool Expanded = true;

		/// <summary>
		/// Gets or Sets what scope triggered this segment
		/// </summary>
		public Scope Scope = null;


		/// <summary>
		/// 
		/// </summary>
		/// <param name="startrow"></param>
		public Segment(Row startrow)
		{
			StartRow = startrow;
		}

		/// <summary>
		/// 
		/// </summary>
		public Segment()
		{
		}
	}
}