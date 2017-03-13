using System;
using System.Text;
using System.IO;

namespace GuruComponents.CodeEditor.Library.Text
{
	/// <summary>
	/// WordReader is a class for simplified reading of word from a string
	/// you can iterate on the string for word and get the current word you can
	/// set the a set of char as separator
	/// </summary>
	public class WordReader
	{
		string m_Text = null;

		public WordReader(string text)
		{
			m_Text=text;
		}

		public static WordReader FromFile(string filename)
		{
			string text = null;

			StreamReader sreader = new StreamReader(filename);

			text = sreader.ReadToEnd();

			sreader.Close();

			return new WordReader(text);
		}

		int m_Position = 0;
		string[] m_Separators = new string[]{" ","\r","\n", "\t" ,"."};
		string m_CurrentWord = string.Empty;
		string m_CurrentSeparator = string.Empty;
		string m_LastWord = String.Empty;
		string[] m_Ignores = new string[0];
		bool m_IsEnd = false;


		public bool Read(string[] separators)
		{
			m_Separators = separators;

			return Read();
		}

		public bool Read()
		{
			StringBuilder sbWord = new StringBuilder();

			if (m_Position == (m_Text.Length))
			{
				m_IsEnd = true;
				return false;
			}

			while(m_Position < m_Text.Length)
			{
				string cChar = m_Text[m_Position].ToString();

				if(Array.IndexOf(m_Ignores,cChar) > 0)
				{
					m_Position++;
					continue;
				}
				
				if(Array.IndexOf(m_Separators,cChar) == -1)
				{
					sbWord.Append(cChar);
					m_CurrentSeparator = null;
				}
				else
				{
					if(sbWord.Length == 0)
					{
						m_Position++;
						continue;
					}
					m_CurrentSeparator = cChar.ToString();
					m_Position++;
					break;
				}
				
				m_Position++;
			}
			m_LastWord = m_CurrentWord;
			m_CurrentWord = sbWord.ToString();

			if (m_Position == m_Text.Length)
			{
				m_IsEnd = true;
			}

			return true;
		}

		public bool IsEnd
		{
			get
			{
				return m_IsEnd;
			}
			set
			{
				m_IsEnd = value;
			}
		}
	
		public string CurrentWord
		{
			get
			{
				return m_CurrentWord;
			}
		}


		public string LastWord
		{
			get
			{
				return m_LastWord;
			}
		}

		public string[] Separator
		{
			get
			{
				return m_Separators;
			}
			set
			{
				m_Separators = value;
			}
		}
		public string[] Ignores
		{
			get
			{
				return m_Ignores;
			}
			set
			{
				m_Ignores = value;
			}
		}

		public string CurrentSeparator
		{
			get
			{
				return m_CurrentSeparator;
			}
		}


	}
}
