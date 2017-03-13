using System;
using System.IO;
using System.Text;

namespace GuruComponents.Netrix.HtmlFormatting.Elements
{
    sealed class FormattedTextWriter : TextWriter
    {
        private TextWriter baseWriter;

        private string indentString;

        private int currentColumn;

        private int indentLevel;

        private bool indentPending;

        private bool onNewLine;


        public override Encoding Encoding
        {
            get
            {
                return baseWriter.Encoding;
            }
        }

        public override string NewLine
        {
            get
            {
                return baseWriter.NewLine;
            }

            set
            {
                baseWriter.NewLine = value;
            }
        }

        public int Indent
        {
            get
            {
                return indentLevel;
            }

            set
            {
                if (value < 0)
                {
                    value = 0;
                }
                indentLevel = value;
            }
        }

        public FormattedTextWriter(TextWriter writer, string indentString)
        {
            baseWriter = writer;
            this.indentString = indentString;
            onNewLine = true;
            currentColumn = 0;
        }

        public override void Close()
        {
            baseWriter.Close();
        }

        public override void Flush()
        {
            baseWriter.Flush();
        }

        public static bool HasBackWhiteSpace(string s)
        {
            if (s == null || s.Length == 0)
            {
                return false;
            }
            else
            {
                return Char.IsWhiteSpace(s[s.Length - 1]);
            }
        }

        public static bool HasFrontWhiteSpace(string s)
        {
            if (s == null || s.Length == 0)
            {
                return false;
            }
            else
            {
                return Char.IsWhiteSpace(s[0]);
            }
        }

        public static bool IsWhiteSpace(string s)
        {
            for (int i = 0; i < s.Length; i++)
            {
                if (!Char.IsWhiteSpace(s[i]))
                {
                    return false;
                }
            }
            return true;
        }

        private string MakeSingleLine(string s)
        {
            StringBuilder stringBuilder = new StringBuilder();
            int i = 0;
            while (i < s.Length)
            {
                char ch = s[i];
                if (Char.IsWhiteSpace(ch))
                {
                    stringBuilder.Append(' ');
                    for (; i < s.Length && Char.IsWhiteSpace(s[i]); i++)
                    {
                    }
                }
                else
                {
                    stringBuilder.Append(ch);
                    i++;
                }
            }
            return stringBuilder.ToString();
        }

        public static string Trim(string text, bool frontWhiteSpace)
        {
            if (text.Length == 0)
            {
                return String.Empty;
            }
            if (IsWhiteSpace(text))
            {
                if (frontWhiteSpace)
                {
                    return " ";
                }
                else
                {
                    return String.Empty;
                }
            }
            string str = text.Trim();
            if (frontWhiteSpace && HasFrontWhiteSpace(text))
            {
                str = String.Concat(" ", str);
            }
            if (HasBackWhiteSpace(text))
            {
                str = String.Concat(str, " ");
            }
            return str;
        }

        private void OutputIndent()
        {
            if (indentPending)
            {
                for (int i = 0; i < indentLevel; i++)
                {
                    baseWriter.Write(indentString);
                }
                indentPending = false;
            }
        }

        public void WriteLiteral(string s)
        {
            if (s.Length != 0)
            {
                StringReader stringReader = new StringReader(s);
                string str1 = stringReader.ReadLine();
                string str2 = stringReader.ReadLine();
                while (str1 != null)
                {
                    Write(str1);
                    str1 = str2;
                    str2 = stringReader.ReadLine();
                    if (str1 != null)
                    {
                        WriteLine();
                    }
                    if (str2 != null)
                    {
                        str1 = str1.Trim();
                    }
                    else if (str1 != null)
                    {
                        str1 = Trim(str1, false);
                    }
                }
            }
        }

        public void WriteLiteralWrapped(string s, int maxLength)
        {
            if (s.Length != 0)
            {
                string[] strs = MakeSingleLine(s).Split(null);
                if (HasFrontWhiteSpace(s))
                {
                    Write(' ');
                }
                for (int i = 0; i < (int)strs.Length; i++)
                {
                    if (strs[i].Length > 0)
                    {
                        Write(strs[i]);
                        if (i < (int)strs.Length - 1 && strs[i + 1].Length > 0)
                        {
                            if (currentColumn > maxLength)
                            {
                                WriteLine();
                            }
                            else
                            {
                                Write(' ');
                            }
                        }
                    }
                }
                if (HasBackWhiteSpace(s) && !IsWhiteSpace(s))
                {
                    Write(' ');
                }
            }
        }

        public void WriteLineIfNotOnNewLine()
        {
            if (!onNewLine)
            {
                baseWriter.WriteLine();
                onNewLine = true;
                currentColumn = 0;
                indentPending = true;
            }
        }

        public override void Write(string s)
        {
            OutputIndent();
            baseWriter.Write(s);
            onNewLine = false;
            currentColumn += s.Length;
        }

        public override void Write(bool value)
        {
            OutputIndent();
            baseWriter.Write(value);
            onNewLine = false;
            currentColumn += value.ToString().Length;
        }

        public override void Write(char value)
        {
            OutputIndent();
            baseWriter.Write(value);
            onNewLine = false;
            currentColumn++;
        }

        public override void Write(char[] buffer)
        {
            OutputIndent();
            baseWriter.Write(buffer);
            onNewLine = false;
            currentColumn += (int)buffer.Length;
        }

        public override void Write(char[] buffer, int index, int count)
        {
            OutputIndent();
            baseWriter.Write(buffer, index, count);
            onNewLine = false;
            currentColumn += count;
        }

        public override void Write(double value)
        {
            OutputIndent();
            baseWriter.Write(value);
            onNewLine = false;
            currentColumn += value.ToString().Length;
        }

        public override void Write(float value)
        {
            OutputIndent();
            baseWriter.Write(value);
            onNewLine = false;
            currentColumn += value.ToString().Length;
        }

        public override void Write(int value)
        {
            OutputIndent();
            baseWriter.Write(value);
            onNewLine = false;
            currentColumn += value.ToString().Length;
        }

        public override void Write(long value)
        {
            OutputIndent();
            baseWriter.Write(value);
            onNewLine = false;
            currentColumn += value.ToString().Length;
        }

        public override void Write(object value)
        {
            OutputIndent();
            baseWriter.Write(value);
            onNewLine = false;
            currentColumn += value.ToString().Length;
        }

        public override void Write(string format, object arg0)
        {
            OutputIndent();
            string str = String.Format(format, arg0);
            baseWriter.Write(str);
            onNewLine = false;
            currentColumn += str.Length;
        }

        public override void Write(string format, object arg0, object arg1)
        {
            OutputIndent();
            string str = String.Format(format, arg0, arg1);
            baseWriter.Write(str);
            onNewLine = false;
            currentColumn += str.Length;
        }

        public override void Write(string format, params object[] arg)
        {
            OutputIndent();
            string str = String.Format(format, arg);
            baseWriter.Write(str);
            onNewLine = false;
            currentColumn += str.Length;
        }

        public override void WriteLine(string s)
        {
            OutputIndent();
            baseWriter.WriteLine(s);
            indentPending = true;
            onNewLine = true;
            currentColumn = 0;
        }

        public override void WriteLine()
        {
            OutputIndent();
            baseWriter.WriteLine();
            indentPending = true;
            onNewLine = true;
            currentColumn = 0;
        }

        public override void WriteLine(bool value)
        {
            OutputIndent();
            baseWriter.WriteLine(value);
            indentPending = true;
            onNewLine = true;
            currentColumn = 0;
        }

        public override void WriteLine(char value)
        {
            OutputIndent();
            baseWriter.WriteLine(value);
            indentPending = true;
            onNewLine = true;
            currentColumn = 0;
        }

        public override void WriteLine(char[] buffer)
        {
            OutputIndent();
            baseWriter.WriteLine(buffer);
            indentPending = true;
            onNewLine = true;
            currentColumn = 0;
        }

        public override void WriteLine(char[] buffer, int index, int count)
        {
            OutputIndent();
            baseWriter.WriteLine(buffer, index, count);
            indentPending = true;
            onNewLine = true;
            currentColumn = 0;
        }

        public override void WriteLine(double value)
        {
            OutputIndent();
            baseWriter.WriteLine(value);
            indentPending = true;
            onNewLine = true;
            currentColumn = 0;
        }

        public override void WriteLine(float value)
        {
            OutputIndent();
            baseWriter.WriteLine(value);
            indentPending = true;
            onNewLine = true;
            currentColumn = 0;
        }

        public override void WriteLine(int value)
        {
            OutputIndent();
            baseWriter.WriteLine(value);
            indentPending = true;
            onNewLine = true;
            currentColumn = 0;
        }

        public override void WriteLine(long value)
        {
            OutputIndent();
            baseWriter.WriteLine(value);
            indentPending = true;
            onNewLine = true;
            currentColumn = 0;
        }

        public override void WriteLine(object value)
        {
            OutputIndent();
            baseWriter.WriteLine(value);
            indentPending = true;
            onNewLine = true;
            currentColumn = 0;
        }

        public override void WriteLine(string format, object arg0)
        {
            OutputIndent();
            baseWriter.WriteLine(format, arg0);
            indentPending = true;
            onNewLine = true;
            currentColumn = 0;
        }

        public override void WriteLine(string format, object arg0, object arg1)
        {
            OutputIndent();
            baseWriter.WriteLine(format, arg0, arg1);
            indentPending = true;
            onNewLine = true;
            currentColumn = 0;
        }

        public override void WriteLine(string format, params object[] arg)
        {
            OutputIndent();
            baseWriter.WriteLine(format, arg);
            indentPending = true;
            currentColumn = 0;
            onNewLine = true;
        }
    }

}
