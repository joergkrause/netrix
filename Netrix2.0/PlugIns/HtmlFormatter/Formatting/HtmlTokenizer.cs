using System;

namespace GuruComponents.Netrix.HtmlFormatting
{
    /// <summary>
    /// This class creates a stream of tokens from the input string. The tokens control how the
    /// formatter will place the elements in line or on a new line and how to indent.
    /// </summary>
    sealed class HtmlTokenizer
    {

        /// <summary>
        /// The Tokenizer codes to set the state of the currently found or checked character state.
        /// </summary>
		internal class HtmlTokenizerStates
		{
			public const int BeginCommentTag1 = 100;
			public const int BeginCommentTag2 = 101;
			public const int BeginDoubleQuote = 19;
			public const int BeginSingleQuote = 20;
			public const int EndCommentTag1 = 103;
			public const int EndCommentTag2 = 104;
			public const int EndDoubleQuote = 11;
			public const int EndSingleQuote = 13;
			public const int EndTag = 17;
			public const int EqualsChar = 18;
			public const int Error = 16;
			public const int ExpAttr = 6;
			public const int ExpAttrVal = 9;
			public const int ExpEquals = 8;
			public const int ExpTag = 2;
			public const int ExpTagAfterSlash = 4;
			public const int ForwardSlash = 3;
			public const int InAttr = 7;
			public const int InAttrVal = 14;
			public const int InCommentTag = 102;
			public const int InDoubleQuoteAttrVal = 10;
			public const int InSingleQuoteAttrVal = 12;
			public const int InTagName = 5;
			public const int RunAtServerState = 2048;
			public const int RunAtState = 1024;
			public const int Script = 40;
			public const int ScriptState = 256;
			public const int SelfTerminating = 15;
			public const int ServerSideScript = 30;
			public const int StartTag = 1;
			public const int Style = 50;
			public const int StyleState = 512;
			public const int Text = 0;
			public const int XmlDirective = 60;

			public HtmlTokenizerStates ()
			{
				return;
			} 
		}

        /// <summary>
        /// Start search for the first token with the current content. The content comes in 
        /// array of char, which is build in the <see cref="HtmlFormatter"/> class.
        /// </summary>
        /// <param name="chars"></param>
        /// <returns></returns>
        public static Token GetFirstToken(char[] chars)
        {
            if (chars == null)
            {
                throw new ArgumentNullException("chars");
            }
            else
            {
                return GetNextToken(chars, chars.Length, 0, 0);
            }
        }

        /// <summary>
        /// Start with the first token at a specific position.
        /// </summary>
        /// <param name="chars">Content array</param>
        /// <param name="length">Start position</param>
        /// <param name="initialState">Current State from which the token is searched.</param>
        /// <returns></returns>
        public static Token GetFirstToken(char[] chars, int length, int initialState)
        {
            return GetNextToken(chars, length, 0, initialState);
        }

        /// <summary>
        /// The next token in the stream
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static Token GetNextToken(Token token)
        {
            if (token == null)
            {
                throw new ArgumentNullException("token");
            }
            else
            {
                return GetNextToken(token.Chars, token.CharsLength, token.EndIndex, token.EndState);
            }
        }

        /// <summary>
        /// The next token within the stream from a given start position and state. The tokenizer loops through the
        /// chars and determines the next token based on the given state and an "expected" state (Exp = expected).
        /// If the expected state is recognized, the token is build, otherwise the search goes further until the 
        /// current state is completely resolved or an error is thrown.
        /// </summary>
        /// <param name="chars">Content array</param>
        /// <param name="length">Length, number of chars to beeing searched.</param>
        /// <param name="startIndex">Start index, point from which the search starts.</param>
        /// <param name="startState">The current state of the tokenizer.</param>
        /// <returns></returns>
		public static Token GetNextToken(char[] chars, int length, int startIndex, int startState)
		{
			Token token;

			if (chars == null)
			{
				throw new ArgumentNullException("chars");
			}
			if (startIndex >= length)
			{
				return null;
			}
			int currentState = startState;
			bool IsScriptState = (startState & HtmlTokenizerStates.ScriptState) == 0 == false;		    // ScriptState
			int scriptState = !IsScriptState ? 0 : HtmlTokenizerStates.ScriptState;
			bool IsStyleState = (startState & HtmlTokenizerStates.StyleState) == 0 == false;		        // StyleState
			int styleState = !IsStyleState ? 0 : HtmlTokenizerStates.StyleState;                             
			bool IsRunAtState = (startState & HtmlTokenizerStates.RunAtState) == 0 == false;		    // RunAtState
			int runAtState = !IsRunAtState ? 0 : HtmlTokenizerStates.RunAtState;
			bool IsRunAtServerState = (startState & HtmlTokenizerStates.RunAtServerState) == 0 == false;		// RunAtServerState
			int runAtServerState = !IsRunAtServerState ? 0 : HtmlTokenizerStates.RunAtServerState;
			int index1 = startIndex;
			int index2 = startIndex;
			int index3;
            for (token = null; token == null && index1 < length; index1++)
			{
				char ch = chars[index1];
                int currentBaseState = currentState & 255;
                if (currentBaseState <= HtmlTokenizerStates.Style)
				{
					switch (currentBaseState)
					{
						case HtmlTokenizerStates.Text:
							if (ch != '<')
							{
								continue;
							}
							currentState = HtmlTokenizerStates.StartTag;
							index3 = index1;
							token = new Token(TokenType.TextToken, currentState, index2, index3, chars, length);
							break;

						case HtmlTokenizerStates.StartTag:
                            if (ch != '<')
                            {
                                currentState = HtmlTokenizerStates.Error;
                            }
                            else if (
                                // Check for <% Tag (ASP/ASP.NET)
                                (index1 + 1 < length && chars[index1 + 1] == '%') 
                                ||
                                // Check for PHP short script tag
                                (index1 + 4 < length && (chars[index1 + 1] == '?' && chars[index1 + 2] == '='))
                                )	
                            {
                                currentState = HtmlTokenizerStates.ServerSideScript | scriptState | styleState;
                                index2 = index1;
                                // slip thru the end tag
                            }
                            else
                            {
                                currentState = HtmlTokenizerStates.ExpTag | scriptState | styleState;
                                index3 = index1 + 1;
                                token = new Token(TokenType.OpenBracket, currentState, index2, index3, chars, length);
                            }
							break;

						case HtmlTokenizerStates.ExpTag:
							if (ch == '/')
							{
								currentState = HtmlTokenizerStates.ForwardSlash | scriptState | styleState;
								index3 = index1;
								token = new Token(TokenType.Empty, currentState, index2, index3, chars, length);
							}
							else if (ch == '!')
							{
								currentState = HtmlTokenizerStates.BeginCommentTag1 | scriptState | styleState;
								index2 = index1;
							}
							else if (ch == '%')
							{
								currentState = HtmlTokenizerStates.ServerSideScript;
								index2 = index1;
							}
							else if (IsWordChar(ch))
							{
								currentState = HtmlTokenizerStates.InTagName | scriptState | styleState;
								index2 = index1;
							}
							else
							{
								currentState = HtmlTokenizerStates.Error;
							}
							break;

						case HtmlTokenizerStates.ServerSideScript:
							int indexAspClose = IndexOf(chars, index1, length, "%>");	// Detect end of <% %> section for ASP/ASP.NET
                            if (indexAspClose > -1)
                            {
                                currentState = HtmlTokenizerStates.Text;
                                index3 = indexAspClose + 2;
                                token = new Token(TokenType.InlineServerScript, currentState, index2, index3, chars, length);
                                break;
                            }
                            int indexPhpClose = IndexOf(chars, index1, length, "?>");	// Detect end of <? ?> section for PHP
                            if (indexPhpClose > -1)
                            {
                                currentState = HtmlTokenizerStates.Text;
                                index3 = indexPhpClose + 2;
                                token = new Token(TokenType.PhpScriptTag, currentState, index2, index3, chars, length);
                                break;
                            }

                            index1 = length - 1;
					        break;

						case HtmlTokenizerStates.ForwardSlash:
							if (ch == '/')
							{
								currentState = HtmlTokenizerStates.ExpTagAfterSlash | scriptState | styleState;
								index3 = index1 + 1;
								token = new Token(TokenType.ForwardSlash, currentState, index2, index3, chars, length);
							}
							else
							{
								currentState = HtmlTokenizerStates.Error;
							}
							break;

						case HtmlTokenizerStates.ExpTagAfterSlash:
							if (IsWordChar(ch))
							{
								currentState = HtmlTokenizerStates.InTagName | scriptState | styleState;
								index2 = index1;
							}
							else
							{
								currentState = HtmlTokenizerStates.Error;
							}
							break;

						case HtmlTokenizerStates.InTagName:
							if (IsWhitespace(ch))
							{
								currentState = HtmlTokenizerStates.ExpAttr;
								index3 = index1;
								string str1 = new String(chars, index2, index3 - index2);
								if (str1.ToLower().Equals("script"))
								{
									if (!IsScriptState)
									{
										currentState |= HtmlTokenizerStates.ScriptState;
									}
								}
								else if (str1.ToLower().Equals("style") && !IsStyleState)
								{
									currentState |= HtmlTokenizerStates.StyleState;
								}
								token = new Token(TokenType.TagName, currentState, index2, index3, chars, length);
							}
							else if (ch == '>')
							{
								currentState = HtmlTokenizerStates.EndTag;
								index3 = index1;
								string str2 = new String(chars, index2, index3 - index2);
								if (str2.ToLower().Equals("script"))
								{
									if (!IsScriptState)
									{
										currentState |= HtmlTokenizerStates.ScriptState;
									}
								}
								else if (str2.ToLower().Equals("style") && !IsStyleState)
								{
									currentState |= HtmlTokenizerStates.StyleState;
								}
								token = new Token(TokenType.TagName, currentState, index2, index3, chars, length);
							}
							else if (!IsWordChar(ch))
							{
								if (ch == '/')
								{
									currentState = HtmlTokenizerStates.SelfTerminating;
									index3 = index1;
									string str3 = new String(chars, index2, index3 - index2);
									if (str3.ToLower().Equals("script"))
									{
										if (!IsScriptState)
										{
											currentState |= HtmlTokenizerStates.ScriptState;
										}
									}
									else if (str3.ToLower().Equals("style") && !IsStyleState)
									{
										currentState |= HtmlTokenizerStates.StyleState;
									}
									token = new Token(TokenType.TagName, currentState, index2, index3, chars, length);
								}
								else
								{
									currentState = HtmlTokenizerStates.Error;
								}
							}
							break;

						case HtmlTokenizerStates.ExpAttr:
							if (IsWordChar(ch))
							{
								currentState = HtmlTokenizerStates.InAttr | scriptState | styleState | runAtServerState;
								index3 = index1;
								token = new Token(TokenType.Whitespace, currentState, index2, index3, chars, length);
							}
							else if (ch == '>')
							{
								currentState = HtmlTokenizerStates.EndTag | scriptState | styleState | runAtServerState;
								index3 = index1;
								token = new Token(TokenType.Whitespace, currentState, index2, index3, chars, length);
							}
							else if (ch == '/')
							{
								currentState = HtmlTokenizerStates.SelfTerminating | scriptState | styleState;
								index3 = index1;
								token = new Token(TokenType.Whitespace, currentState, index2, index3, chars, length);
							}
							else if (!IsWhitespace(ch))
							{
								currentState = HtmlTokenizerStates.Error;
							}
							break;

						case HtmlTokenizerStates.InAttr:
							if (IsWhitespace(ch))
							{
								currentState = HtmlTokenizerStates.ExpEquals | scriptState | styleState | runAtServerState;
								index3 = index1;
								if (IsScriptState && new String(chars, index2, index3 - index2).ToLower() == "runat")
								{
									currentState |= 1024;
								}
								token = new Token(TokenType.AttrName, currentState, index2, index3, chars, length);
							}
							else if (ch == '=')
							{
								currentState = HtmlTokenizerStates.ExpEquals | scriptState | styleState | runAtServerState;
								index3 = index1;
								if (IsScriptState && new String(chars, index2, index3 - index2).ToLower() == "runat")
								{
									currentState |= HtmlTokenizerStates.RunAtState;
								}
								token = new Token(TokenType.AttrName, currentState, index2, index3, chars, length);
							}
							else if (ch == '>')
							{
								currentState = HtmlTokenizerStates.EndTag | scriptState | styleState | runAtServerState;
								index3 = index1;
								token = new Token(TokenType.AttrName, currentState, index2, index3, chars, length);
							}
							else if (ch == '/')
							{
								currentState = HtmlTokenizerStates.SelfTerminating | scriptState | styleState;
								index3 = index1;
								token = new Token(TokenType.AttrName, currentState, index2, index3, chars, length);
							}
							else if (!IsWordChar(ch))
							{
								currentState = HtmlTokenizerStates.Error;
							}
							break;

						case HtmlTokenizerStates.ExpEquals:
							if (ch == '=')
							{
								currentState = HtmlTokenizerStates.ExpAttrVal | scriptState | styleState | runAtState | runAtServerState;
								index2 = index1;
								index3 = index1 + 1;
								token = new Token(TokenType.EqualsChar, currentState, index2, index3, chars, length);
							}
							else if (ch == '>')
							{
								currentState = HtmlTokenizerStates.EndTag | scriptState | styleState | runAtServerState;
								index3 = index1;
								token = new Token(TokenType.Whitespace, currentState, index2, index3, chars, length);
							}
							else if (ch == '/')
							{
								currentState = HtmlTokenizerStates.SelfTerminating;
								index3 = index1;
								token = new Token(TokenType.Whitespace, currentState, index2, index3, chars, length);
							}
							else if (IsWordChar(ch))
							{
								currentState = HtmlTokenizerStates.InAttr | scriptState | styleState | runAtServerState;
								index3 = index1;
								token = new Token(TokenType.Whitespace, currentState, index2, index3, chars, length);
							}
							else if (!IsWhitespace(ch))
							{
								currentState = HtmlTokenizerStates.Error;
							}
							break;

						case HtmlTokenizerStates.EqualsChar:
							if (ch == '=')
							{
								currentState = HtmlTokenizerStates.ExpAttrVal | scriptState | styleState | runAtState | runAtServerState;
								index3 = index1 + 1;
								token = new Token(TokenType.EqualsChar, currentState, index2, index3, chars, length);
							}
							else
							{
								currentState = HtmlTokenizerStates.Error;
							}
							break;

						case HtmlTokenizerStates.ExpAttrVal:
							if (ch == '\'')
							{
								currentState = HtmlTokenizerStates.BeginSingleQuote | scriptState | styleState | runAtState | runAtServerState;
								index3 = index1;
								token = new Token(TokenType.Whitespace, currentState, index2, index3, chars, length);
							}
							else if (ch == '\"')
							{
								currentState = HtmlTokenizerStates.BeginDoubleQuote | scriptState | styleState | runAtState | runAtServerState;
								index3 = index1;
								token = new Token(TokenType.Whitespace, currentState, index2, index3, chars, length);
							}
							else if (IsWordChar(ch) || ch == '/')   // here we need to recognize the forward slash, because the parameter can contain a unix path
							{
								currentState = HtmlTokenizerStates.InAttrVal | scriptState | styleState | runAtState | runAtServerState;
								index3 = index1;
								token = new Token(TokenType.Whitespace, currentState, index2, index3, chars, length);
							}
							else if (!IsWhitespace(ch))
							{
								currentState = HtmlTokenizerStates.Error;
							}
							break;

						case HtmlTokenizerStates.BeginDoubleQuote:
							if (ch == '\"')
							{
								currentState = HtmlTokenizerStates.InDoubleQuoteAttrVal | scriptState | styleState | runAtState | runAtServerState;
								index3 = index1 + 1;
								token = new Token(TokenType.DoubleQuote, currentState, index2, index3, chars, length);
							}
							else
							{
								currentState = HtmlTokenizerStates.Error;
                                System.Diagnostics.Debug.WriteLine("BeginDoubleQuote");
							}
							break;

						case HtmlTokenizerStates.InDoubleQuoteAttrVal:
							if (ch != '\"')
							{
								continue; // goto IL_0c11;
							}
							currentState = HtmlTokenizerStates.EndDoubleQuote | scriptState | styleState | runAtServerState;
							index3 = index1;
							if (IsRunAtState && new String(chars, index2, index3 - index2).ToLower() == "server")
							{
								currentState |= HtmlTokenizerStates.RunAtServerState;
							}
							token = new Token(TokenType.AttrVal, currentState, index2, index3, chars, length);
							break;

						case HtmlTokenizerStates.EndDoubleQuote:
							if (ch == '\"')
							{
								currentState = HtmlTokenizerStates.ExpAttr | scriptState | styleState | runAtServerState;
								index3 = index1 + 1;
								token = new Token(TokenType.DoubleQuote, currentState, index2, index3, chars, length);
							}
							else
							{
								currentState = HtmlTokenizerStates.Error;
							}
							break;

						case HtmlTokenizerStates.BeginSingleQuote:
							if (ch == '\'')
							{
								currentState = HtmlTokenizerStates.InSingleQuoteAttrVal | scriptState | styleState | runAtState | runAtServerState;
								index3 = index1 + 1;
								token = new Token(TokenType.SingleQuote, currentState, index2, index3, chars, length);
							}
							else
							{
								currentState = HtmlTokenizerStates.Error;
							}
							break;

						case HtmlTokenizerStates.InSingleQuoteAttrVal:
							if (ch != '\'')
							{
								continue;
							}
							currentState = HtmlTokenizerStates.EndSingleQuote | scriptState | styleState | runAtServerState;
							index3 = index1;
							if (IsRunAtState && new String(chars, index2, index3 - index2).ToLower() == "server")
							{
								currentState |= HtmlTokenizerStates.RunAtServerState;
							}
							token = new Token(TokenType.AttrVal, currentState, index2, index3, chars, length);
							break;

						case HtmlTokenizerStates.EndSingleQuote:
							if (ch == '\'')
							{
								currentState = HtmlTokenizerStates.ExpAttr | scriptState | styleState | runAtServerState;
								index3 = index1 + 1;
								token = new Token(TokenType.SingleQuote, currentState, index2, index3, chars, length);
							}
							else
							{
								currentState = HtmlTokenizerStates.Error;
							}
							break;

						case HtmlTokenizerStates.InAttrVal:
							if (IsWhitespace(ch))
							{
								currentState = HtmlTokenizerStates.ExpAttr | scriptState | styleState | runAtServerState;
								index3 = index1;
								if (IsRunAtState && new String(chars, index2, index3 - index2).ToLower() == "server")
								{
									currentState |= HtmlTokenizerStates.RunAtServerState;
								}
								token = new Token(TokenType.AttrVal, currentState, index2, index3, chars, length);
							}
							else if (ch == '>')
							{
								currentState = HtmlTokenizerStates.EndTag | scriptState | styleState | runAtServerState;
								index3 = index1;
								if (IsRunAtState && new String(chars, index2, index3 - index2).ToLower() == "server")
								{
									currentState |= HtmlTokenizerStates.RunAtServerState;
								}
								token = new Token(TokenType.AttrVal, currentState, index2, index3, chars, length);
							}
							else if (ch == '/' && index1 + 1 < length && chars[index1 + 1] == '>')
							{
								currentState = HtmlTokenizerStates.SelfTerminating | scriptState | styleState | runAtServerState;
								index3 = index1;
								if (IsRunAtState && new String(chars, index2, index3 - index2).ToLower() == "server")
								{
									currentState |= HtmlTokenizerStates.RunAtServerState;
								}
								token = new Token(TokenType.AttrVal, currentState, index2, index3, chars, length);
							}
							break;

						case HtmlTokenizerStates.SelfTerminating:
							if (ch == '/' && index1 + 1 < length && chars[index1 + 1] == '>')
							{
								currentState = HtmlTokenizerStates.Text;
								index3 = index1 + 2;
								token = new Token(TokenType.SelfTerminating, currentState, index2, index3, chars, length);
							}
							else
							{
								currentState = HtmlTokenizerStates.Error;
							}
							break;

						case HtmlTokenizerStates.EndTag:
							if (ch == '>')
							{
								if (IsScriptState)
								{
									currentState = HtmlTokenizerStates.Script | scriptState | styleState | runAtServerState;
								}
								else if (IsStyleState)
								{
									currentState = HtmlTokenizerStates.Style | scriptState | styleState;
								}
								else
								{
									currentState = HtmlTokenizerStates.Text;
								}
								index3 = index1 + 1;
								token = new Token(TokenType.CloseBracket, currentState, index2, index3, chars, length);
							}
							else
							{
								currentState = HtmlTokenizerStates.Error;
							}
							break;

						case HtmlTokenizerStates.Script:
							int endScriptIndex = IndexOf(chars, index1, length, "</script>");
							if (endScriptIndex > -1)
							{
								currentState = HtmlTokenizerStates.StartTag | scriptState | styleState | runAtServerState;
								index3 = endScriptIndex;
								if (IsRunAtServerState)
								{
									token = new Token(TokenType.ServerScriptBlock, currentState, index2, index3, chars, length);
								}
								else
								{
									token = new Token(TokenType.ClientScriptBlock, currentState, index2, index3, chars, length);
								}
							}
							else
							{
								index1 = length - 1;
							}
							break;

						case HtmlTokenizerStates.Error:
							if (ch != '>')
							{
								continue;
							}
							currentState = HtmlTokenizerStates.EndTag;
							index3 = index1;
							token = new Token(TokenType.Error, currentState, index2, index3, chars, length);
							break;
                        // some mixed states we ignore here
						case 21:
						case 22:
						case 23:
						case 24:
						case 25:
						case 26:
						case 27:
						case 28:
						case 29:
						case 31:
						case 32:
						case 33:
						case 34:
						case 35:
						case 36:
						case 37:
						case 38:
						case 39:
							break;

						default:
							if (currentBaseState != HtmlTokenizerStates.Style)
							{
								continue;
							}
							int endStyleIndex = IndexOf(chars, index1, length, "</style>");
							if (endStyleIndex > -1)
							{
								currentState = HtmlTokenizerStates.StartTag | scriptState | styleState;
								index3 = endStyleIndex;
								token = new Token(TokenType.Style, currentState, index2, index3, chars, length);
							}
							else
							{
								index1 = length - 1;
							}
							break;
					}
				}
				else if (currentBaseState != HtmlTokenizerStates.XmlDirective)
				{
					switch (currentBaseState)
					{
						case HtmlTokenizerStates.BeginCommentTag1:
							if (ch == '-')
							{
								currentState = HtmlTokenizerStates.BeginCommentTag2;
							}
							else if (IsWordChar(ch))
							{
								currentState = HtmlTokenizerStates.XmlDirective;
							}
							else
							{
								currentState = HtmlTokenizerStates.Error;
							}
							break;

						case HtmlTokenizerStates.BeginCommentTag2:
							if (ch == '-')
							{
								currentState = HtmlTokenizerStates.InCommentTag;
							}
							else
							{
								currentState = HtmlTokenizerStates.Error;
							}
							break;

						case HtmlTokenizerStates.InCommentTag:
							if (ch != '-')
							{
								continue;
							}
							currentState = HtmlTokenizerStates.EndCommentTag1;
							break;

						case HtmlTokenizerStates.EndCommentTag1:
							if (ch == '-')
							{
								currentState = HtmlTokenizerStates.EndCommentTag2;
							}
							else
							{
								currentState = HtmlTokenizerStates.InCommentTag;
							}
							break;

						case HtmlTokenizerStates.EndCommentTag2:
							if (Char.IsWhiteSpace(ch))
							{
								continue;
							}
							if (ch == '>')
							{
								currentState = HtmlTokenizerStates.EndTag;
								index3 = index1;
								token = new Token(TokenType.Comment, currentState, index2, index3, chars, length);
							}
							else
							{
								currentState = HtmlTokenizerStates.InCommentTag;
							}
							break;
					}
				}
				else if (ch == '>')
				{
					currentState = HtmlTokenizerStates.EndTag;
					index3 = index1;
					token = new Token(TokenType.XmlDirective, currentState, index2, index3, chars, length);
				}
			}
			if (index1 >= length && token == null)
			{
				TokenType tokenType;
				int currentBaseState = currentState & 255;
				if (currentBaseState <= 14)
				{
					if (currentBaseState != HtmlTokenizerStates.Text)
					{
						switch (currentBaseState)
						{
							case HtmlTokenizerStates.InAttr:
								tokenType = TokenType.AttrName;
								currentState = HtmlTokenizerStates.ExpAttr;
								break;

							case HtmlTokenizerStates.ExpAttr:
								tokenType = TokenType.Whitespace;
								break;

							default:
								if (currentBaseState != HtmlTokenizerStates.InAttrVal)
								{
									return token;
								}
								tokenType = TokenType.AttrVal;
								currentState = HtmlTokenizerStates.ExpAttr;
								break;
						}
					}
					else
					{
						tokenType = TokenType.TextToken;
					}
				}
				else if (currentBaseState <= HtmlTokenizerStates.Script)
				{
					if (currentBaseState != HtmlTokenizerStates.ServerSideScript)
					{
						if (currentBaseState != HtmlTokenizerStates.Script)
						{
							return token;
						}
						if (IsRunAtServerState)
						{
							tokenType = TokenType.ServerScriptBlock;
						}
						else
						{
							tokenType = TokenType.ClientScriptBlock;
						}
					}
					else
					{
						tokenType = TokenType.InlineServerScript;
					}
				}
				else if (currentBaseState != HtmlTokenizerStates.Style)
				{
					switch (currentBaseState)
					{
						case HtmlTokenizerStates.BeginCommentTag1:
						case HtmlTokenizerStates.BeginCommentTag2:
						case HtmlTokenizerStates.InCommentTag:
						case HtmlTokenizerStates.EndCommentTag1:
						case HtmlTokenizerStates.EndCommentTag2:
							tokenType = TokenType.Comment;
							break;
						default:
							tokenType = TokenType.Error;
							currentState = HtmlTokenizerStates.Error;
							break;
					}
				}
				else
				{
					tokenType = TokenType.Style;
				}
				index3 = index1;
				token = new Token(tokenType, currentState, index2, index3, chars, length);
			}
			return token;
		}

        /// <summary>
        /// Checks the char is a whitespace.
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        private static bool IsWhitespace(char c)
        {
            return Char.IsWhiteSpace(c);
        }

        /// <summary>
        /// Checks the char is a word character, which is a letter, a digit or one of the characters
        /// '_' (underline), ':' (colon), '#' (hash sign), '-' (minus sign), '*' (asterisk), '.' (dot), and '^'.
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        private static bool IsWordChar(char c)
        {
            if (!Char.IsLetterOrDigit(c) && c != '_' && c != ':' && c != '#' && c != '-' && c != '*' && c != '^' && c != ',')
            {
                return c == '.';
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Searches for a specific character or string within the chars array from a specific position
        /// and to a specific position.
        /// </summary>
        /// <param name="chars">Chars array</param>
        /// <param name="startIndex">Start position</param>
        /// <param name="endColumnNumber">End position</param>
        /// <param name="s">String to be searched for</param>
        /// <returns></returns>
        private static int IndexOf(char[] chars, int startIndex, int endColumnNumber, string s)
        {
            int currentState = s.Length;
            int j = endColumnNumber - currentState + 1;
            for (int k = startIndex; k < j; k++)
            {
                bool flag = true;
                for (int runAtState = 0; runAtState < currentState; runAtState++)
                {
                    if (Char.ToUpper(chars[k + runAtState]) != Char.ToUpper(s[runAtState]))
                    {
                        flag = false;
                        break;
                    }
                }
                if (flag)
                {
                    return k;
                }
            }
            return -1;
        }
    }

}