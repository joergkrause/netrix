using System;
using System.IO;
using System.Collections;

namespace GuruComponents.CodeEditor.CodeEditor.Syntax
{
	/// <summary>
	/// Language list class
	/// </summary>
	public class LanguageList
	{

		ArrayList mLanguages=null;


		/// <summary>
		/// 
		/// </summary>
		public LanguageList()
		{
			mLanguages=new ArrayList ();
			
			string[] files=Directory.GetFiles(".","*.syn");
			foreach (string file in files)
			{
				//try
				//{
					SyntaxLoader l=new SyntaxLoader ();
					mLanguages.Add (l.Load (file));
				//}
				//catch(System.Exception x)
				//{
				//	Console.WriteLine (x.Message);
				//}
			}

		}


        public void Add(Language lang)
        {
            mLanguages.Add(lang);
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public Language GetLanguageFromFile(string path)
		{
			string extension= System.IO.Path.GetExtension (path);
			foreach (GuruComponents.CodeEditor.CodeEditor.Syntax.Language lang in mLanguages)
			{
				foreach (GuruComponents.CodeEditor.CodeEditor.Syntax.FileType ft in lang.FileTypes)
				{
					if (extension.ToLower () == ft.Extension.ToLower ())
					{
						return lang;
					}
				}
			}
			return null;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public ArrayList ListLanguages()
		{
			return mLanguages ;
		}
	}
}
