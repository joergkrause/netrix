using System;
using System.Collections;
using GuruComponents.CodeEditor.CodeEditor.Syntax;

namespace GuruComponents.CodeEditor.CodeEditor.Syntax.SyntaxDocumentParsers
{

	public sealed class ParseTools
	{
		public static char[] Parse(string text,string separators)
		{
			//string Result="";
			System.Text.StringBuilder Result=new System.Text.StringBuilder ();
			text= " " + text +" ";

            char c;

			for(int i = 0; i <text.Length;i++)
            {
                c = text[i];
				if (separators.IndexOf (c)>=0 )
					Result.Append (' ');
				else
					Result.Append ('.');
            }

			return Result.ToString().ToCharArray ();
		}



		public static void AddPatternString(string Text,Row Row,Pattern Pattern,TextStyle Style,Segment Segment,bool HasError)
		{
			Word x= Row.Add (Text);
			x.Style = Style;
			x.Pattern = Pattern;
			x.HasError =HasError;
			x.Segment =Segment;
		}

		public unsafe static void AddString(string Text,Row Row,TextStyle Style,Segment Segment)
		{
			if (Text=="")
				return;

			System.Text.StringBuilder CurrentWord=new System.Text.StringBuilder();

			char[] Buff=Text.ToCharArray();

			fixed (char* c = &Buff[0])
			{
				for (int i=0;i<Text.Length;i++)
				{
					if (c[i]==' ' || c[i]=='\t')
					{

						if (CurrentWord.Length != 0)
						{
							Word word = Row.Add (CurrentWord.ToString ());
							word.Style =Style;
							word.Segment =Segment;
							CurrentWord =new System.Text.StringBuilder();
						}

						Word ws=Row.Add (c[i].ToString ());
						if (c[i]==' ')
							ws.Type= WordType.xtSpace;
						else
							ws.Type= WordType.xtTab;
						ws.Style = Style ;
						ws.Segment = Segment;					
					}
					else
						CurrentWord.Append (c[i].ToString ());					
				}
				if (CurrentWord.Length  != 0)
				{
					Word word=Row.Add (CurrentWord.ToString ());
					word.Style =Style;
					word.Segment =Segment;				
				}	
			}
		}


		

		public static ArrayList GetWords(string text)
		{
			ArrayList words=new ArrayList ();
			System.Text.StringBuilder CurrentWord=new System.Text.StringBuilder();

            for(int i = 0; i < text.Length;i++)
            {
                char c = text[i];

				if (c==' ' || c=='\t')
				{
					if (CurrentWord.ToString ()!="")
					{
						words.Add (CurrentWord.ToString ());
						CurrentWord =new System.Text.StringBuilder();
					}

					words.Add (c.ToString ());
					
				}
				else
					CurrentWord.Append (c.ToString ());
			}
			if (CurrentWord.ToString ()!="")
			words.Add (CurrentWord.ToString ());
			return words;
		}

		public static PatternScanResult GetFirstWord(char[] TextBuffer,PatternCollection Patterns,int StartPosition)
		{
			PatternScanResult Result;
			Result.Index =0;
			Result.Token ="";

//			for (int i=StartPosition;i<TextBuffer.Length;i++)
//			{
//
//				//-----------------------------------------------
//				if (c[i]==PatternBuffer[0])
//				{
//					bool found=true;
//					for (int j=0;j<Pattern.Length;j++)
//					{
//						if (c[i+j]!=p[j])
//						{
//							found=false;
//							break;
//						}
//					}
//					if (found)
//					{
//						Result.Index =i+StartPosition;
//						Result.Token = Text.Substring(i+StartPosition,this.Pattern.Length);
//						return Result;
//					}							
//				}
//				//-----------------------------------------------
//			}



			return Result;
		}
	}
}
