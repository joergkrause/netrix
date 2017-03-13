using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Xml;

namespace GuruComponents.CodeEditor.CodeEditor.Syntax
{
	/// <summary>
	/// File type class.
	/// </summary>
	public class FileType
	{
		/// <summary>
		/// The file type extension
		/// </summary>
		public string Extension="";
		/// <summary>
		/// The name of the file type
		/// </summary>
		public string Name="";

	}
	/// <summary>
	/// The Language class describes a language.<br/>
	/// It consists of a MainBlock , which is the start BlockType of the Language<br/>
	/// It also have a list of filetypes that is valid for this language<br/>
	/// </summary>
	/// <example>
	/// <b>Apply a Syntax to a SyntaxBox</b>
	/// <code>
	/// SyntaxBoxControl1.Document.SyntaxFile="C#.syn";
	/// </code>
	/// </example>
	public class Language
	{
		#region PUBLIC PROPERTY SEPARATORS
		private string _Separators=".,:;{}()[]+-*/\\ \t=&%$#@!|&";
		public string Separators
		{
			get
			{
				return _Separators;
			}
			set
			{
				_Separators = value;
			}
		}
		#endregion

        public string FileDialogFilters
        {
            get
            {
                FileType current = null;
                StringBuilder sb = new StringBuilder();

                sb.Append(this.Name);
                sb.Append(" |");

                for (int i = 0; i < FileTypes.Count; i++)
                {
                    current = (FileType)FileTypes[i];

                    sb.Append("*");
                    sb.Append(current.Extension);
                    if (i < FileTypes.Count - 1)
                        sb.Append(";");
                }

                return sb.ToString();
            }
        }

            #region PUBLIC PROPERTY VERSION
            private long _Version = long.MinValue;
		public long Version
		{
			get
			{
				return _Version;
			}
			set
			{
				_Version = value;
			}
		}
		#endregion

		public void UpdateLists()
		{
			BlockType[] blocks =this.Blocks;
			foreach (BlockType block in blocks)
			{
				block.Parent = this;
				block.ResetLookupTable ();				

				block.KeywordsList.Parent=block;
				foreach (PatternList patterns in block.KeywordsList)
				{
					patterns.Parent = block.KeywordsList;

					foreach (Pattern pattern in patterns)
					{
						block.AddToLookupTable (pattern);
					}
				}

				block.OperatorsList.Parent=block;
				foreach (PatternList patterns in block.OperatorsList)
				{
					patterns.Parent = block.OperatorsList;

					foreach (Pattern pattern in patterns)
					{
						block.AddToLookupTable (pattern);
					}
				}
				block.BuildLookupTable ();
			}
		}
		
		public void ChangeVersion()
		{
			this.Version ++;
			if (this.Version > long.MaxValue-10)
				this.Version = long.MinValue;
		}

		private Hashtable			_Blocks=new Hashtable ();
		private Hashtable			_Styles=new Hashtable ();

		/// <summary>
		/// Name of the Language
		/// </summary>
		public string Name="";	
		/// <summary>
		/// The start BlockType for this language
		/// </summary>
		public BlockType MainBlock=null;

		/// <summary>
		/// ArrayList containing the valid filetypes for this language
		/// </summary>
		public ArrayList FileTypes=new ArrayList ();

		/// <summary>
		/// 
		/// </summary>
		public Language()
		{
		}

        //public TextStyle[] TextStyles
        //{
        //    get
        //    {
        //        TextStyle[] sts = new TextStyle[_Styles.Count];

        //        _Styles.CopyTo(sts,0);

        //        return sts;
        //    }
        //    set
        //    {
        //        _Styles.Clear();

        //        TextStyle[] sts = value;

        //        foreach (TextStyle style in sts)
        //        {
        //            _Styles.Add(style.Name, style);
        //        }
        //    }
        //}

		public static Language FromSyntaxFile(string filename)
		{
			SyntaxLoader sl=new SyntaxLoader();
			return sl.Load (filename);
		}

        public static Language FromSyntaxFile(Stream stream)
        {
            SyntaxLoader sl = new SyntaxLoader();
            return sl.Load(stream);
        }

		public void MergeByMainBlock(Language Target)
		{
			BlockType [] blocks = this.Blocks;
			foreach (BlockType bt in blocks)
			{
				bt.ChildBlocks.Insert (0,Target.MainBlock);
			}
		}

		public void MergeByChildBlocks(Language Target)
		{
			BlockType [] blocks = this.Blocks;
			foreach (BlockType bt in blocks)
			{
				for (int i=Target.MainBlock.ChildBlocks.Count-1 ;i>=0;i--)
				{
					BlockType child=Target.MainBlock.ChildBlocks[i];
					bt.ChildBlocks.Insert (0,child);
				}
			}
		}


		/// <summary>
		/// Gets all BlockTypes in a given language.
		/// </summary>
		public BlockType[] Blocks
		{
			get
			{
				_Blocks.Clear ();
				FillBlocks(this.MainBlock);
				BlockType[] blocks=new BlockType[_Blocks.Values.Count];
				int i=0;
				foreach (BlockType bt in _Blocks.Values)
				{
					blocks[i]=bt;
					i++;
				}
				

				return blocks;
			}	
		}

		public TextStyle[] Styles
		{
			get
			{
				_Styles.Clear ();
				BlockType[] blocks=this.Blocks ;
				foreach (BlockType bt in blocks)
				{
					_Styles[bt.Style]=bt.Style;
					
					foreach (Scope sc in bt.ScopePatterns)
					{

						if (sc.Style!= null)
							_Styles[sc.Style]=sc.Style;						
					}

					foreach (PatternList pl in bt.KeywordsList)
					{
						if (pl.Style!= null)
							_Styles[pl.Style]=pl.Style;												
					}

					foreach (PatternList pl in bt.OperatorsList)
					{
						if (pl.Style!= null)
							_Styles[pl.Style]=pl.Style;												
					}
				}

				TextStyle[] styles=new TextStyle[_Styles.Values.Count];
				int i=0;
				foreach (TextStyle st in _Styles.Values)
				{
					styles[i]=st;
					i++;
				}
				return styles;
			}
		}

		private void FillBlocks(BlockType bt)
		{
			if (bt==null)
				return;

			if (_Blocks[bt]!=null)
				return;

			_Blocks[bt]=bt;

			foreach (BlockType btc in bt.ChildBlocks)
			{
				FillBlocks(btc);
			}
			foreach (Scope sc in bt.ScopePatterns)
			{
				FillBlocks(sc.SpawnBlockOnEnd);
				FillBlocks(sc.SpawnBlockOnStart);
			}
		}


        /// <summary>
        /// Save user styles configuration on the directory set on UserCustomStyles
        /// if UserCustomStyles is null the function throw an exception.
        /// </summary>
        /// <returns></returns>
        public void SaveStyles()
        {
            if (SyntaxLoader.UserCustomStyles == null)
            {
                throw new IOException("Invalid user config dir.");
            }
            else
            {
                if (!Directory.Exists(SyntaxLoader.UserCustomStyles))
                {
                    throw new IOException("Invalid user config path name, or path don't exist.");
                }
            }

            string path = Path.Combine(SyntaxLoader.UserCustomStyles, this.Name + ".conf");

            XmlTextWriter xwr = new XmlTextWriter(path, Encoding.UTF8);

            xwr.Formatting = Formatting.Indented;

            xwr.WriteStartElement("styles");

            TextStyle[] styles = this.Styles;

            foreach(TextStyle style in styles)
            {
                xwr.WriteStartElement("Style");

                xwr.WriteAttributeString("Name", style.Name);
                xwr.WriteAttributeString("ForeColor", style.ForeColor.Name);
                xwr.WriteAttributeString("BackColor", style.BackColor.Name);
                xwr.WriteAttributeString("Bold", style.Bold.ToString());
                xwr.WriteAttributeString("Italic", style.Italic.ToString());
                xwr.WriteAttributeString("Underline", style.Italic.ToString());

                xwr.WriteEndElement();
            }

            xwr.WriteEndElement();

            xwr.Flush();

            xwr.Close();

        }

        public override string ToString()
        {
            return this.Name;
        }
//		/// <summary>
//		/// Serializes the language object into an xml string.
//		/// </summary>
//		/// <returns></returns>
//		public string ToXML()
//		{
//			return "";
//		}


	}
}
