using System;

namespace GuruComponents.Netrix.HtmlFormatting
{

    /// <summary>
    /// This class represents a token, which defines a part of text with a specific meaning,
    /// like an attribute or a closing tag.
    /// </summary>
    sealed class Token
    {

        private TokenType _type;
        private char[] _chars;
        private int _charsLength;
        private string _text;
        private int _startIndex;
        private int _endIndex;
        private int _endState;

        internal char[] Chars
        {
            get
            {
                return _chars;
            }
        }

        internal int CharsLength
        {
            get
            {
                return _charsLength;
            }
        }

        public int EndIndex
        {
            get
            {
                return _endIndex;
            }
        }

        public int EndState
        {
            get
            {
                return _endState;
            }
        }

        public int Length
        {
            get
            {
                return _endIndex - _startIndex;
            }
        }

        public int StartIndex
        {
            get
            {
                return _startIndex;
            }
        }

        public string Text
        {
            get
            {
                if (_text == null)
                {
                    _text = new String(_chars, StartIndex, EndIndex - StartIndex);
                }
                return _text;
            }
        }

        public TokenType Type
        {
            get
            {
                return _type;
            }
        }

        public Token(TokenType type, int endState, int startIndex, int endIndex, char[] chars, int charsLength)
        {
            _type = type;
            _chars = chars;
            _charsLength = charsLength;
            _startIndex = startIndex;
            _endIndex = endIndex;
            _endState = endState;
        }
    }

}
