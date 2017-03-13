using System.Text.RegularExpressions;

namespace GuruComponents.CodeEditor.CodeEditor.Syntax
{
	/// <summary>
	/// PatternScanResult struct is redurned by the Pattern class when an .IndexIn call has been performed.
	/// </summary>
	public struct PatternScanResult
	{
		/// <summary>
		/// The index on which the pattern was found in the source string
		/// </summary>
		public int Index;

		/// <summary>
		/// The string that was found , this is always the same as the pattern StringPattern property if the pattern is a simple pattern.
		/// if the pattern is complex this field will contain the string that was found by the scan.
		/// </summary>
		public string Token;
	}

	public enum BracketType
	{
		None,
		StartBracket,
		EndBracket,
	}

	/// <summary>
	/// A Pattern is a specific string or a RegEx pattern that is used by the parser.
	/// There are two types of patterns , Simple and Complex.
	/// 
	/// Simple Patterns are patterns that consists of a simple fixed string eg. "void" or "for".
	/// Complex Patterns are patterns that consists of RegEx patterns , eg hex numbers or urls can be described as regex patterns.
	/// </summary>
	public sealed class Pattern
	{
		#region PUBLIC PROPERTY SEPARATORS

		private string _Separators = ".,+-*^\\/()[]{}@:;'?£$#%& \t=<>";

		public string Separators
		{
			get { return _Separators; }
			set { _Separators = value; }
		}

		#endregion

		/// <summary>
		/// Category of the pattern
		/// Built in categories are:
		/// URL
		/// MAIL
		/// FILE
		/// </summary>
		public string Category = null;

		private string _StringPattern = "";

		/// <summary>
		/// For public use only
		/// </summary>
		public string LowerStringPattern = "";

		private Regex rx;
		private Regex rx2;
		private char[] PatternBuffer = null;

		/// <summary>
		/// Gets if the pattern is a simple string or a RegEx pattern
		/// </summary>
		public bool IsComplex = false;

		/// <summary>
		/// Gets or Sets if the pattern is a separator pattern .
		/// A separator pattern can be "End Sub" in VB6 , whenever that pattern is found , the SyntaxBoxControl will render a horizontal separator line.
		/// NOTE: this should not be mixed up with separator chars.
		/// </summary>
		public bool IsSeparator = false;

		/// <summary>
		/// Get or Sets if this pattern needs separator chars before and after it in order to be valid.
		/// </summary>
		public bool IsKeyword = false;

		/// <summary>
		/// The owning PatternList , eg a specific KeywordList or OperatorList
		/// </summary>
		public PatternList Parent = null;

		public BracketType BracketType = BracketType.None;
		public Pattern MatchingBracket = null;
		public bool IsMultiLineBracket = true;

		/// <summary>
		/// Gets or Sets the the text of the pattern
		/// this only applies if the pattern is a simple pattern.
		/// </summary>
		public string StringPattern
		{
			get { return _StringPattern; }
			set
			{
				_StringPattern = value;
				LowerStringPattern = _StringPattern.ToLower();
			}


		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="pattern"></param>
		/// <param name="iscomplex"></param>
		public Pattern(string pattern, bool iscomplex)
		{
			StringPattern = pattern;
			if (iscomplex)
			{
				IsComplex = true;
				rx = new Regex(StringPattern, RegexOptions.Compiled);
				rx2 = new Regex(StringPattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
			}
			else
			{
				PatternBuffer = pattern.ToCharArray();
				IsComplex = false;
			}
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="pattern"></param>
		/// <param name="iscomplex"></param>
		/// <param name="separator"></param>
		/// <param name="keyword"></param>
		public Pattern(string pattern, bool iscomplex, bool separator, bool keyword)
		{
			Init(pattern, iscomplex, separator, keyword);
		}

		private void Init(string pattern, bool iscomplex, bool separator, bool keyword)
		{
			StringPattern = pattern;
			IsSeparator = separator;
			IsKeyword = keyword;
			if (iscomplex)
			{
				IsComplex = true;
				rx = new Regex(StringPattern, RegexOptions.Compiled);
				rx2 = new Regex(StringPattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
			}
			else
			{
				PatternBuffer = pattern.ToCharArray();
				IsComplex = false;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="pattern"></param>
		/// <param name="iscomplex"></param>
		/// <param name="separator"></param>
		/// <param name="keyword"></param>
		/// <param name="EscapeChar"></param>
		public Pattern(string pattern, bool iscomplex, bool separator, bool keyword, string EscapeChar)
		{
			EscapeChar = Regex.Escape(EscapeChar);
			string EscPattern = string.Format("(?<=((?<!{0})({0}{0})*))({1})", EscapeChar, pattern);
			Init(EscPattern, true, separator, keyword);
		}

		/// <summary>
		/// For public use only
		/// </summary>
		/// <param name="Text"></param>
		/// <param name="Position"></param>
		/// <returns></returns>
		public bool HasSeparators(string Text, int Position)
		{
			return (CharIsSeparator(Text, Position - 1) && CharIsSeparator(Text, Position + StringPattern.Length));
		}


		private bool CharIsSeparator(string Text, int Position)
		{
			if (Position < 0 || Position >= Text.Length)
				return true;

			string s = Text.Substring(Position, 1);
			if (Separators.IndexOf(s) >= 0)
				return true;
			else
				return false;
		}

		/// <summary>
		/// Returns the index of the pattern in a string
		/// </summary>
		/// <param name="Text">The string in which to find the pattern</param>
		/// <param name="StartPosition">Start index in the string</param>
		/// <param name="MatchCase">true if a case sensitive match should be performed</param>
		/// <returns>A PatternScanResult containing information on where the pattern was found and also the text of the pattern</returns>
		public PatternScanResult IndexIn(string Text, int StartPosition, bool MatchCase, string Separators)
		{
			if (Separators == null)
			{
			}
			else
			{
				this.Separators = Separators;
			}

			if (!IsComplex)
			{
				if (!this.IsKeyword)
					return SimpleFind(Text, StartPosition, MatchCase);
				else
					return SimpleFindKeyword(Text, StartPosition, MatchCase);
			}
			else
			{
				if (!this.IsKeyword)
					return ComplexFind(Text, StartPosition);
				else
					return ComplexFindKeyword(Text, StartPosition);
			}
		}


		private PatternScanResult SimpleFind(string Text, int StartPosition, bool MatchCase)
		{
			int Position = 0;
			if (MatchCase)
				Position = Text.IndexOf(StringPattern, StartPosition);
			else
				Position = Text.ToLower().IndexOf(this.LowerStringPattern, StartPosition);

			PatternScanResult Result;
			if (Position >= 0)
			{
				Result.Index = Position;
				Result.Token = Text.Substring(Position, StringPattern.Length);
			}
			else
			{
				Result.Index = 0;
				Result.Token = "";
			}

			return Result;
		}

		private PatternScanResult SimpleFindKeyword(string Text, int StartPosition, bool MatchCase)
		{
			PatternScanResult res;
			while (true)
			{
				res = SimpleFind(Text, StartPosition, MatchCase);
				if (res.Token == "")
					return res;

				if (CharIsSeparator(Text, res.Index - 1) && CharIsSeparator(Text, res.Index + res.Token.Length))
					return res;

				StartPosition = res.Index + 1;
				if (StartPosition >= Text.Length)
				{
					res.Token = "";
					res.Index = 0;
					return res;
				}
			}
		}


		private PatternScanResult ComplexFindKeyword(string Text, int StartPosition)
		{
			PatternScanResult res;
			while (true)
			{
				res = ComplexFind(Text, StartPosition);
				if (res.Token == "")
					return res;

				if (CharIsSeparator(Text, res.Index - 1) && CharIsSeparator(Text, res.Index + res.Token.Length))
					return res;

				StartPosition = res.Index + 1;
				if (StartPosition >= Text.Length)
				{
					res.Token = "";
					res.Index = 0;
					return res;
				}
			}
		}

		private PatternScanResult ComplexFind(string Text, int StartPosition)
		{
			MatchCollection mc = rx.Matches(Text);
			int pos = 0;
			string p = "";
			foreach (Match m in mc)
			{
				pos = m.Index;
				p = m.Value;
				if (pos >= StartPosition)
				{
					PatternScanResult t;
					t.Index = pos;
					t.Token = p;
					return t;
				}
			}
			PatternScanResult res;
			res.Index = 0;
			res.Token = "";
			return res;
		}

		/// <summary>
		/// Returns true if the pattern contains separator chars<br/>
		/// (This is used by the parser)
		/// </summary>
		public bool ContainsSeparator
		{
			get
			{
				//foreach (char c in this.StringPattern.ToCharArray())

                for(int i = 0; i < this.StringPattern.Length;i++)
				{
                    char c = this.StringPattern[i];
					if (Separators.IndexOf(c) >= 0)
						return true;
				}
				return false;

			}
		}

		#region old stuff

		//		private unsafe PatternScanResult SimpleFindFast(string Text,int StartPosition,bool MatchCase)
		//		{
		//			PatternScanResult Result;
		//			int Length=Text.Length - StartPosition;
		//			char[] Buff=Text.ToCharArray (StartPosition,Length);
		//			Length-=Pattern.Length;
		//
		//			if (StartPosition>=Text.Length-1 || Length<1)
		//			{
		//				Result.Index =0;
		//				Result.Token ="";
		//				return Result;
		//			}
		//			fixed(char* p=&PatternBuffer[0])
		//			{
		//				fixed(char* c=&Buff[0])
		//				{
		//					for (int i=0;i<=Length;i++)
		//					{					
		//						if (c[i]==PatternBuffer[0])
		//						{
		//							bool found=true;
		//							for (int j=0;j<Pattern.Length;j++)
		//							{
		//								if (c[i+j]!=p[j])
		//								{
		//									found=false;
		//									break;
		//								}
		//							}
		//							if (found)
		//							{
		//								Result.Index =i+StartPosition;
		//								Result.Token = Text.Substring(i+StartPosition,this.Pattern.Length);
		//								return Result;
		//							}							
		//						}
		//					}
		//				}
		//			}
		//			
		//			Result.Index =0;
		//			Result.Token ="";
		//			return Result;
		//		}


//		private PatternScanResult ComplexFind(string Text)
//		{
//			Match m= rx.Match (Text);
//			int pos=0;
//			string p="";
//			if (m.Success)
//			{
//				pos=m.Index;
//				p=m.Value;
//				PatternScanResult t;
//				t.Index =pos;
//				t.Token = p;
//				return t;
//			}
//			else
//			{
//				PatternScanResult res;
//				res.Index =0;
//				res.Token ="";
//				return res;
//			}
//		}

//		private PatternScanResult ComplexFind(string Text,ref char[] SeparatorPositions)
//		{
//		
//		}

//		private PatternScanResult ComplexFind(string Text,ref char[] SeparatorPositions)
//		{
//			MatchCollection mc= rx.Matches (Text);
//			int pos=0;
//			string p="";
//			foreach (Match m in mc)
//			{
//				pos=m.Index;
//				p=m.Value;
//				if (SeparatorPositions[pos]==(char)32 && SeparatorPositions[pos+p.Length+1]==(char)32 )
//				{
//					PatternScanResult t;
//					t.Index =pos;
//					t.Token = p;
//					return t;
//				}
//			}
//			PatternScanResult res;
//			res.Index =0;
//			res.Token ="";
//			return res;
//		}

		#endregion
	}
}