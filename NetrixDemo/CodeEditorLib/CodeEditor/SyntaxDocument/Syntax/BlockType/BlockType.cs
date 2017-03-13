using System.Collections;
using System.Drawing;

namespace GuruComponents.CodeEditor.CodeEditor.Syntax
{
	/// <summary>
	/// BlockType class
	/// </summary>
	/// <remarks>
	/// The BlockType class represents a specific code/text element<br/>
	/// such as a string , comment or the code itself.<br/>
	/// <br/>
	/// a BlockType  can contain keywords , operators , scopes and child BlockTypes.<br/>
	/// <br/>
	/// <br/>
	/// For example , if we where to describe the language C#<br/>
	/// we would have the following blocks:<br/>
	/// <br/>
	/// Code block						- the BlockType containing all the keywords and operators.<br/>
	/// Singleline comment block		- a BlockType that starts on // terminates at the end of a line.<br/>
	/// Multiline comment block			- a BlockType that starts on /* can span multiple rows and terminates on */.<br/>
	/// String block					- a BlockType that starts on " terminates on " or at the end of a line.<br/>
	/// Char block						- a BlockType that starts on ' terminates on ' or at the end of a line.<br/>
	/// <br/>
	/// <b>CHILD BLOCKS:</b><br/>
	/// The code block would have all the other blocks as childblocks , since they can only appear inside the<br/>
	/// code block . A string can for example never exist inside a comment in C#.<br/>
	/// a BlockType can also have itself as a child block.<br/>
	/// For example , the C# Code block can have itself as a childblock and use the scope patterns "{" and "}"<br/>
	/// this way we can accomplish FOLDING since the parser will know where a new scope starts and ends.<br/>
	/// <br/>
	/// <b>SCOPES:</b><br/>
	/// Scopes describe what patterns starts and what patterns end a specific BlockType.<br/>
	/// For example , the C# Multiline Comment have the scope patterns /* and */<br/>
	/// <br/>
	/// <b>KEYWORDS:</b><br/>
	/// A Keyword is a pattern that can only exist between separator chars.<br/>
	/// For example the keyword "for" in c# is valid if it is contained in this string " for ("<br/>
	/// but it is not valid if the containing string is " MyFormat "<br/>
	/// <br/>
	/// <b>OPERATORS:</b><br/>
	/// Operators is the same thing as keywords but are valid even if there are no separator chars around it.<br/>
	/// In most cases operators are only one or two chars such as ":" or "->"<br/>
	/// operators in this context should not be mixed up with code operators such as "and" or "xor" in VB6<br/>
	/// in this context they are keywords.<br/>
	///<br/>
	/// <br/>
	///</remarks>
	public class BlockType
	{
		/// <summary>
		/// A list of keyword groups.
		/// For example , one keyword group could be "keywords" and another could be "datatypes"
		/// theese groups could have different color shemes assigned to them.
		/// </summary>
		public PatternListList KeywordsList = null; //new PatternListList (this);
		/// <summary>
		/// A list of operator groups.
		/// Each operator group can contain its own operator patterns and its own color shemes.
		/// </summary>
		public PatternListList OperatorsList = null; //new PatternListList (this);	
		/// <summary>
		/// A list of scopes , most block only contain one scope , eg a scope with start and end patterns "/*" and "*/"
		/// for multiline comments, but in some cases you will need more scopes , eg. PHP uses both "&lt;?" , "?&gt;" and "&lt;?PHP" , "PHP?&gt;"
		/// </summary>
		public ScopeCollection ScopePatterns = null;

		/// <summary>
		/// A list containing which BlockTypes are valid child blocks in a specific block.
		/// eg. strings and comments are child blocks for a code block
		/// </summary>
		public BlockTypeCollection ChildBlocks = new BlockTypeCollection();

		/// <summary>
		/// The style to use when colorizing the content of a block,
		/// meaning everything in this block except keywords , operators and childblocks.
		/// </summary>
		public TextStyle Style;

		/// <summary>
		/// The name of this block.
		/// names are not required for block but can be a good help when interacting with the parser.
		/// </summary>
		public string Name = "";

		/// <summary>
		/// The background color of a block.
		/// </summary>
		public Color BackColor = Color.Transparent;

		/// <summary>
		/// Gets or Sets if the BlockType can span multiple lines or if it should terminate at the end of a line.
		/// </summary>
		public bool MultiLine = false;

		/// <summary>
		/// Gets or Sets if the parser should terminate any child block when it finds an end scope pattern for this block.
		/// for example %&gt; in asp terminates any asp block even if it appears inside an asp string.
		/// </summary>
		public bool TerminateChildren = false;


		/// <summary>
		/// Returns false if any color has been assigned to the backcolor property
		/// </summary>
		public bool Transparent
		{
			get { return (BackColor.A == 0); }
		}

		/// <summary>
		/// Default BlockType constructor
		/// </summary>
		public BlockType(Language parent) : this()
		{
			this.Parent = parent;
			this.Parent.ChangeVersion();
		}

		public BlockType()
		{
			this.KeywordsList = new PatternListList(this);
			this.OperatorsList = new PatternListList(this);

			Style = new TextStyle();
			KeywordsList.Parent = this;
			KeywordsList.IsKeyword = true;
			OperatorsList.Parent = this;
			OperatorsList.IsOperator = true;
			ScopePatterns = new ScopeCollection(this);
		}

		#region PUBLIC PROPERTY PARENT

		private Language _Parent;

		public Language Parent
		{
			get { return _Parent; }
			set { _Parent = value; }
		}

		#endregion

		public Hashtable LookupTable = new Hashtable();
		private ArrayList tmpSimplePatterns = new ArrayList();
		public PatternCollection ComplexPatterns = new PatternCollection();

		public void ResetLookupTable()
		{
			this.LookupTable.Clear();
			this.tmpSimplePatterns.Clear();
			this.ComplexPatterns.Clear();

		}

		public void AddToLookupTable(Pattern pattern)
		{
			if (pattern.IsComplex)
			{
				ComplexPatterns.Add(pattern);
				return;
			}
			else
			{
				this.tmpSimplePatterns.Add(pattern);
			}

		}

		public void BuildLookupTable()
		{
			IComparer comparer = new PatternComparer();
			this.tmpSimplePatterns.Sort(comparer);
			foreach (Pattern p in tmpSimplePatterns)
			{
				if (p.StringPattern.Length <= 2)
				{
					char c = p.StringPattern[0];

					if (!p.Parent.CaseSensitive)
					{
						char c1 = char.ToLower(c);
						if (LookupTable[c1] == null)
							LookupTable[c1] = new PatternCollection();

						PatternCollection patterns = LookupTable[c1] as PatternCollection;
						if (!patterns.Contains(p))
							patterns.Add(p);

						char c2 = char.ToUpper(c);
						if (LookupTable[c2] == null)
							LookupTable[c2] = new PatternCollection();

						patterns = LookupTable[c2] as PatternCollection;
						if (!patterns.Contains(p))
							patterns.Add(p);
					}
					else
					{
						if (LookupTable[c] == null)
							LookupTable[c] = new PatternCollection();

						PatternCollection patterns = LookupTable[c] as PatternCollection;
						if (!patterns.Contains(p))
							patterns.Add(p);
					}
				}
				else
				{
					string c = p.StringPattern.Substring(0, 3).ToLower();

					if (LookupTable[c] == null)
						LookupTable[c] = new PatternCollection();

					PatternCollection patterns = LookupTable[c] as PatternCollection;
					if (!patterns.Contains(p))
						patterns.Add(p);
				}
			}
		}
	}

	public class PatternComparer : IComparer
	{
		#region Implementation of IComparer

		public int Compare(object x, object y)
		{
			Pattern xx = x as Pattern;
			Pattern yy = y as Pattern;
			return yy.StringPattern.Length.CompareTo(xx.StringPattern.Length);
		}

		#endregion
	}
}