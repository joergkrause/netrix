using System;
using System.Collections;
using System.IO;
using System.Text;

namespace GuruComponents.Netrix.HtmlFormatting
{
    /// <summary>
    /// This class formattes embedded script sections. It is currently not used.
    /// </summary>
    sealed class ScriptFormatter
    {

		private class ScriptLine
		{
			// FIELDS

			private bool _isBlank;
			private int _padding;
			private string _script;

			// PROPERTIES

			public bool IsBlank
			{
				get
				{
					return this._isBlank;
				} 
      
			}

			public int Length
			{
				get
				{
					if ( (this.IsBlank))
					{
						return 0;
					}
      
					return this.Padding  +  this.Script.Length;
				} 
      
			}
			
			public int Padding
			{
				get
				{
					if ( (this.IsBlank))
					{
						return 2147483647;
					}
      
					return this._padding;
				} 
      
				set
				{
					if (! (this.IsBlank))
					{
						this._padding = value;
					}
      
					return;
				} 
      
			}
			
			public string Script
			{
				get
				{
					return this._script;
				} 
      
			}
			// METHODS

			public ScriptLine (string  script)
			{
				int i = 0;
				int j = 0;
				if (script.Length  >  0)
				{
					char ch = script[i];
					while ( (char.IsWhiteSpace (ch)) && (i  <  script.Length))
					{
						if (ch  ==  9)
						{
							j += TabSize;
						}
						else 
						{
							j++;
						}
  
						ch = script[i];
						i++;
              
					};
				}
  
				this._padding = j;
				this._script = script.Trim ();
				if (this._script.Length  ==  0)
				{
					this._isBlank = true;
					this._padding = 0;
				}
  
				return;
			} 
			public override string ToString ()
			{
				if ( (this.IsBlank))
				{
					return "";
				}
  
				StringBuilder stringBuilder = new StringBuilder ((this.Padding  +  this.Script.Length));
				for (int i = 0; i  <  this.Padding; i++)
				{
					stringBuilder.Append(' ');
          
				};
				stringBuilder.Append (this.Script);
				return stringBuilder.ToString ();
			} 
		}
		
		public static int TabSize = 4;
        private ArrayList _lines;
        private int _minPadding;
        private int _length;

        public ScriptFormatter()
        {
            _minPadding = int.MaxValue;
        }

        public void AddScript(string script)
        {
            StringReader stringReader = new StringReader(script);
            for (string str = stringReader.ReadLine(); str != null; str = stringReader.ReadLine())
            {
                ScriptLine scriptLine = new ScriptLine(str);
                int i = scriptLine.Padding;
                if (i < _minPadding)
                {
                    _minPadding = i;
                }
                _length += scriptLine.Length + 2;
                if (_lines == null)
                {
                    _lines = new ArrayList();
                }
                _lines.Add(scriptLine);
            }
        }

        public override string ToString()
        {
            if (_lines == null)
            {
                return "";
            }
            StringBuilder stringBuilder = new StringBuilder(_length);
            IEnumerator iEnumerator = _lines.GetEnumerator();
            try
            {
                while (iEnumerator.MoveNext())
                {
                    ScriptLine scriptLine = (ScriptLine)iEnumerator.Current;
                    scriptLine.Padding = scriptLine.Padding - _minPadding;
                    stringBuilder.Append(scriptLine.ToString());
                    stringBuilder.Append(Environment.NewLine);
                }
            }
            finally
            {
                IDisposable iDisposable = iEnumerator as IDisposable;
                if (iDisposable != null)
                {
                    iDisposable.Dispose();
                }
            }
            return stringBuilder.ToString();
        }
    }

}
