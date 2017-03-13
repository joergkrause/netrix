using System;
using System.Collections;

namespace GuruComponents.CodeEditor.CodeEditor.Syntax
{
	/// <summary>
	/// A List containing patterns.
	/// this could be for example a list of keywords or operators
	/// </summary>
	public sealed class PatternList : IEnumerable
	{
		private PatternCollection mPatterns = new PatternCollection();

		/// <summary>
		/// for public use only
		/// </summary>
		public Hashtable SimplePatterns = new Hashtable();

		/// <summary>
		/// 
		/// </summary>
		public Hashtable SimplePatterns1Char = new Hashtable();

		/// <summary>
		/// For public use only
		/// </summary>
		public Hashtable SimplePatterns2Char = new Hashtable();

		/// <summary>
		/// For public use only
		/// </summary>
		public PatternCollection ComplexPatterns = new PatternCollection();

		/// <summary>
		/// Gets or Sets the TextStyle that should be assigned to patterns in this list
		/// </summary>
		public TextStyle Style = new TextStyle();

		/// <summary>
		/// Gets or Sets if this list contains case seinsitive patterns
		/// </summary>		
		public bool CaseSensitive = false;

		/// <summary>
		/// Gets or Sets if the patterns in this list should be case normalized
		/// </summary>
		public bool NormalizeCase = false;

		/// <summary>
		/// 
		/// </summary>
		public PatternListList Parent = null;

		/// <summary>
		/// The parent BlockType of this list
		/// </summary>
		public BlockType ParentBlock = null;

		/// <summary>
		/// The name of the pattern list
		/// </summary>
		public string Name = "";

		/// <summary>
		/// 
		/// </summary>
		public PatternList()
		{
			SimplePatterns = new Hashtable(CaseInsensitiveHashCodeProvider.Default, CaseInsensitiveComparer.Default);
		}

		/// <summary>
/// 
/// </summary>
/// <returns></returns>
		public IEnumerator GetEnumerator()
		{
			return mPatterns.GetEnumerator();
		}

		/// <summary>
/// 
/// </summary>
/// <param name="Pattern"></param>
/// <returns></returns>
		public Pattern Add(Pattern Pattern)
		{
			if (this.Parent != null && this.Parent.Parent != null && this.Parent.Parent.Parent != null)
			{
				Pattern.Separators = this.Parent.Parent.Parent.Separators;
				this.Parent.Parent.Parent.ChangeVersion();

			}

			if (!Pattern.IsComplex && !Pattern.ContainsSeparator)
			{
				//store pattern in lookuptable if it is a simple pattern
				string s = "";

				if (Pattern.StringPattern.Length >= 2)
					s = Pattern.StringPattern.Substring(0, 2);
				else
					s = Pattern.StringPattern.Substring(0, 1) + " ";

				s = s.ToLower();

				if (Pattern.StringPattern.Length == 1)
				{
					SimplePatterns1Char[Pattern.StringPattern] = Pattern;
				}
				else
				{
					if (SimplePatterns2Char[s] == null)
						SimplePatterns2Char[s] = new PatternCollection();
					PatternCollection ar = (PatternCollection) SimplePatterns2Char[s];
					ar.Add(Pattern);
				}

				if (this.CaseSensitive)
					SimplePatterns[Pattern.LowerStringPattern] = Pattern;
				else
					SimplePatterns[Pattern.StringPattern] = Pattern;

//				if (SimplePatterns[s]==null)
//					SimplePatterns.Add (s,new ArrayList ());
//				
//				ArrayList bb=(ArrayList) SimplePatterns[s];
//
//				bb.Add (Pattern);

			}
			else
			{
				ComplexPatterns.Add(Pattern);
			}

			mPatterns.Add(Pattern);
			if (Pattern.Parent == null)
				Pattern.Parent = this;
			else
			{
				throw(new Exception("Pattern already assigned to another PatternList"));
			}
			return Pattern;
		}

		/// <summary>
/// 
/// </summary>
		public void Clear()
		{
			mPatterns.Clear();
		}
	}
}