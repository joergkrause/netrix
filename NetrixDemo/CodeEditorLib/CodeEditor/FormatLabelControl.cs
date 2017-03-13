
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Web;
using System.Windows.Forms;
using GuruComponents.CodeEditor.Library.Drawing.GDI;

namespace GuruComponents.CodeEditor.Forms
{
	/// <summary>
	/// 
	/// </summary>
	public class FormatLabelControl : Widget
	{
		private string _Text = "format <b>label</b>";
		private FormatLabelElement[] _Elements = null;
		private ArrayList _Rows = null;
		private Hashtable _Fonts = new Hashtable();
		private Hashtable _Images = new Hashtable();
		private bool _WordWrap = true;
		private PictureBox Filler;
		private VScrollBar vScroll;
		private HScrollBar hScroll;
		private bool _AutoSizeHorizontal = false;
		private bool _AutoSizeVertical = false;
		private ScrollBars _ScrollBars = 0;
		private FormatLabelElement _ActiveElement = null;
		private Color _Link_Color = Color.Blue;
		private Color _Link_Color_Hover = Color.Blue;
		private bool _Link_UnderLine = false;
		private bool _Link_UnderLine_Hover = true;
		private bool _HasImageError = false;

		private ImageList _ImageList = null;

		public event ClickLinkEventHandler ClickLink = null;

		protected void OnClickLink(string Link)
		{
			if (ClickLink != null)
				ClickLink(this, new ClickLinkEventArgs(Link));
		}

		#region PUBLIC PROPERTY MARGIN

		private int _Margin = 0;

		public int LabelMargin
		{
			get { return _Margin; }
			set
			{
				_Margin = value;
				CreateRows();
				this.Invalidate();
			}
		}

		#endregion

		public ImageList ImageList
		{
			get { return _ImageList; }
			set
			{
				_ImageList = value;
				this.Invalidate();
				//this.Text = this.Text;
			}
		}

		public Color Link_Color
		{
			get { return _Link_Color; }
			set
			{
				_Link_Color = value;
				this.Invalidate();
			}
		}

        protected override void OnInvalidated(InvalidateEventArgs e)
        {
            if (this.IsHandleCreated)
                RedrawBuffer();
            base.OnInvalidated(e);
        }



		public Color Link_Color_Hover
		{
			get { return _Link_Color_Hover; }
			set
			{
				_Link_Color_Hover = value;
				this.Invalidate();
			}
		}

		public bool Link_UnderLine
		{
			get { return _Link_UnderLine; }
			set
			{
				_Link_UnderLine = value;
				this.Invalidate();
			}
		}

		public bool Link_UnderLine_Hover
		{
			get { return _Link_UnderLine_Hover; }
			set
			{
				_Link_UnderLine_Hover = value;
				this.Invalidate();
			}
		}


		public bool AutoSizeHorizontal
		{
			get { return _AutoSizeHorizontal; }
			set { _AutoSizeHorizontal = value; }

		}

		public bool AutoSizeVertical
		{
			get { return _AutoSizeVertical; }
			set { _AutoSizeVertical = value; }

		}

		#region Defaults

		private Container components = null;

		public FormatLabelControl()
		{
            _Rows = new ArrayList();

			InitializeComponent();

            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.Opaque, true);

			this.Text = this.Text;
            InitScrollbars(); 
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				foreach (GDIObject o in this._Fonts.Values)
					o.Dispose();

				if (components != null)
					components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.Filler = new System.Windows.Forms.PictureBox();
			this.vScroll = new System.Windows.Forms.VScrollBar();
			this.hScroll = new System.Windows.Forms.HScrollBar();
			this.SuspendLayout();
			// 
			// Filler
			// 
			this.Filler.BackColor = System.Drawing.SystemColors.Control;
			this.Filler.Cursor = System.Windows.Forms.Cursors.Default;
			this.Filler.Location = new System.Drawing.Point(136, 112);
			this.Filler.Name = "Filler";
			this.Filler.Size = new System.Drawing.Size(16, 16);
			this.Filler.TabIndex = 5;
			this.Filler.TabStop = false;
			// 
			// vScroll
			// 
			this.vScroll.Cursor = System.Windows.Forms.Cursors.Default;
			this.vScroll.LargeChange = 2;
			this.vScroll.Location = new System.Drawing.Point(136, -8);
			this.vScroll.Name = "vScroll";
			this.vScroll.Size = new System.Drawing.Size(16, 112);
			this.vScroll.TabIndex = 4;
			this.vScroll.Scroll += new System.Windows.Forms.ScrollEventHandler(this.vScroll_Scroll);
			// 
			// hScroll
			// 
			this.hScroll.Cursor = System.Windows.Forms.Cursors.Default;
			this.hScroll.LargeChange = 1;
			this.hScroll.Location = new System.Drawing.Point(0, 112);
			this.hScroll.Maximum = 600;
			this.hScroll.Name = "hScroll";
			this.hScroll.Size = new System.Drawing.Size(128, 16);
			this.hScroll.TabIndex = 3;
			// 
			// FormatLabelControl
			// 
			this.BackColor = System.Drawing.SystemColors.Window;
			this.Controls.AddRange(new System.Windows.Forms.Control[]
				{
					this.Filler,
					this.vScroll,
					this.hScroll
				});
			this.Name = "FormatLabelControl";
			this.Size = new System.Drawing.Size(160, 136);
			this.ResumeLayout(false);

		}

		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public override string Text
		{
			get { return _Text; }
			set
			{
				try
				{
					//Text=value;
					_Text = value;

					CreateAll();
					this.Invalidate();
				}
				catch (Exception x)
				{
					Console.WriteLine(x.Message);
					System.Diagnostics.Debugger.Break();
				}

                RedrawBuffer();
			}

		}

		private void CreateAll()
		{
			_Elements = CreateElements();
			ClearFonts();


			ApplyFormat(_Elements);
			CreateWords(_Elements);
			CreateRows();
			SetAutoSize();
		}

		private void ClearFonts()
		{
			foreach (GDIFont gf in _Fonts.Values)
			{
				gf.Dispose();
			}
			_Fonts.Clear();
		}

		#endregion

		#endregion

		private void SetAutoSize()
		{
			if (this.AutoSizeHorizontal)
				this.Width = this.GetWidth();

			if (this.AutoSizeVertical)
				this.Height = this.GetHeight();

		}

		public bool WordWrap
		{
			get { return _WordWrap; }
			set
			{
				_WordWrap = value;
				CreateRows();
				this.Invalidate();
			}


		}


		[Browsable(true), EditorBrowsable(EditorBrowsableState.Always), Obsolete("", false)]
		public override Image BackgroundImage
		{
			get { return base.BackgroundImage; }
			set { base.BackgroundImage = value; }
		}

        GDISurface _bufferSurface = null;

        private void RedrawBuffer()
        {
            _bufferSurface = new GDISurface(this.Width, this.Height, this, true);

            using (Graphics gfx = Graphics.FromHdc(_bufferSurface.hDC))
            {
                //try
                //{
                _bufferSurface.FontTransparent = true;

                if (this.BackgroundImage != null)
                {
                    gfx.DrawImage(this.BackgroundImage, 0, 0, this.Width, this.Height);
                }
                else
                {
                    _bufferSurface.Clear(this.BackColor);
                }
                int x = LabelMargin;
                int y = LabelMargin;
                for (int i = vScroll.Value; i < _Rows.Count; i++)
                {
                    FormatLabelRow r = (FormatLabelRow)_Rows[i];
                    x = LabelMargin;
                    r.Visible = true;
                    r.Top = y;
                    if (r.RenderSeparator)
                    {
                        Color c1 = Color.FromArgb(120, 0, 0, 0);
                        Brush b1 = new SolidBrush(c1);
                        gfx.FillRectangle(b1, 0, y, this.Width, 1);

                        Color c2 = Color.FromArgb(120, 255, 255, 255);
                        Brush b2 = new SolidBrush(c2);
                        gfx.FillRectangle(b2, 0, y + 1, this.Width, 1);

                        b1.Dispose();
                        b2.Dispose();


                        //bbuff.DrawLine (this.ForeColor,new Point (0,y),new Point (this.Width,y));
                    }

                    foreach (FormatLabelWord w in r.Words)
                    {
                        int ypos = r.Height - w.Height + y;

                        if (w.Image != null)
                        {
                            gfx.DrawImage(w.Image, x, y);
                            //bbuff.FillRect (Color.Red ,x,ypos,w.Width ,w.Height);
                        }
                        else
                        {
                            GDIFont gf = null;
                            if (w.Element.Link != null)
                            {
                                Font f = null;

                                FontStyle fs = w.Element.Font.Style;
                                if (w.Element.Link == _ActiveElement)
                                {
                                    if (_Link_UnderLine_Hover)
                                        fs |= FontStyle.Underline;

                                    f = new Font(w.Element.Font, fs);
                                }
                                else
                                {
                                    if (_Link_UnderLine)
                                        fs |= FontStyle.Underline;

                                    f = new Font(w.Element.Font, fs);
                                }

                                gf = GetFont(f);
                            }
                            else
                            {
                                gf = GetFont(w.Element.Font);
                            }

                            _bufferSurface.Font = gf;
                            if (w.Element.Effect != TextEffect.None)
                            {
                                _bufferSurface.TextForeColor = w.Element.EffectColor;

                                if (w.Element.Effect == TextEffect.Outline)
                                {
                                    for (int xx = -1; xx <= 1; xx++)
                                        for (int yy = -1; yy <= 1; yy++)
                                            _bufferSurface.DrawTabbedString(w.Text, x + xx, ypos + yy, 0, 0);


                                }
                                else if (w.Element.Effect != TextEffect.None)
                                {
                                    _bufferSurface.DrawTabbedString(w.Text, x + 1, ypos + 1, 0, 0);
                                }
                            }


                            if (w.Element.Link != null)
                            {
                                if (w.Element.Link == _ActiveElement)
                                {
                                    _bufferSurface.TextForeColor = Link_Color_Hover;
                                }
                                else
                                {
                                    _bufferSurface.TextForeColor = Link_Color;
                                }
                            }
                            else
                                _bufferSurface.TextForeColor = w.Element.ForeColor;

                            _bufferSurface.TextBackColor = w.Element.BackColor;
                            _bufferSurface.DrawTabbedString(w.Text, x, ypos, 0, 0);
                        }

                        w.ScreenArea.X = x;
                        w.ScreenArea.Y = ypos;
                        x += w.Width;
                    }

                    y += r.Height + r.BottomPadd;
                    if (y > this.Height)
                        break;
                }

                //}
                //catch (Exception x)
                //{
                //    Console.WriteLine(x.Message);
                //}
            }
        }

		protected override void OnPaint(PaintEventArgs e)
		{
			this.SetAutoSize();
			//base.OnPaint (e);

			if (_HasImageError)
				CreateAll();

            if (_bufferSurface != null)
                _bufferSurface.RenderToControl(0, 0);

            //GDISurface bbuff = new GDISurface(this.Width, this.Height, this, true);

            //Graphics g = Graphics.FromHdc(bbuff.hDC);
            //try
            //{
            //    bbuff.FontTransparent = true;

            //    if (this.BackgroundImage != null)
            //    {
            //        g.DrawImage(this.BackgroundImage, 0, 0, this.Width, this.Height);
            //    }
            //    else
            //    {
            //        bbuff.Clear(this.BackColor);
            //    }
            //    int x = LabelMargin;
            //    int y = LabelMargin;
            //    for (int i = vScroll.Value; i < _Rows.Count; i++)
            //    {
            //        FormatLabelRow r = (FormatLabelRow) _Rows[i];
            //        x = LabelMargin;
            //        r.Visible = true;
            //        r.Top = y;
            //        if (r.RenderSeparator)
            //        {
            //            Color c1 = Color.FromArgb(120, 0, 0, 0);
            //            Brush b1 = new SolidBrush(c1);
            //            g.FillRectangle(b1, 0, y, this.Width, 1);

            //            Color c2 = Color.FromArgb(120, 255, 255, 255);
            //            Brush b2 = new SolidBrush(c2);
            //            g.FillRectangle(b2, 0, y + 1, this.Width, 1);

            //            b1.Dispose();
            //            b2.Dispose();


            //            //bbuff.DrawLine (this.ForeColor,new Point (0,y),new Point (this.Width,y));
            //        }

            //        foreach (FormatLabelWord w in r.Words)
            //        {
            //            int ypos = r.Height - w.Height + y;

            //            if (w.Image != null)
            //            {
            //                g.DrawImage(w.Image, x, y);
            //                //bbuff.FillRect (Color.Red ,x,ypos,w.Width ,w.Height);
            //            }
            //            else
            //            {
            //                GDIFont gf = null;
            //                if (w.Element.Link != null)
            //                {
            //                    Font f = null;

            //                    FontStyle fs = w.Element.Font.Style;
            //                    if (w.Element.Link == _ActiveElement)
            //                    {
            //                        if (_Link_UnderLine_Hover)
            //                            fs |= FontStyle.Underline;

            //                        f = new Font(w.Element.Font, fs);
            //                    }
            //                    else
            //                    {
            //                        if (_Link_UnderLine)
            //                            fs |= FontStyle.Underline;

            //                        f = new Font(w.Element.Font, fs);
            //                    }

            //                    gf = GetFont(f);
            //                }
            //                else
            //                {
            //                    gf = GetFont(w.Element.Font);
            //                }

            //                bbuff.Font = gf;
            //                if (w.Element.Effect != TextEffect.None)
            //                {
            //                    bbuff.TextForeColor = w.Element.EffectColor;

            //                    if (w.Element.Effect == TextEffect.Outline)
            //                    {
            //                        for (int xx = -1; xx <= 1; xx++)
            //                            for (int yy = -1; yy <= 1; yy++)
            //                                bbuff.DrawTabbedString(w.Text, x + xx, ypos + yy, 0, 0);


            //                    }
            //                    else if (w.Element.Effect != TextEffect.None)
            //                    {
            //                        bbuff.DrawTabbedString(w.Text, x + 1, ypos + 1, 0, 0);
            //                    }
            //                }


            //                if (w.Element.Link != null)
            //                {
            //                    if (w.Element.Link == _ActiveElement)
            //                    {
            //                        bbuff.TextForeColor = Link_Color_Hover;
            //                    }
            //                    else
            //                    {
            //                        bbuff.TextForeColor = Link_Color;
            //                    }
            //                }
            //                else
            //                    bbuff.TextForeColor = w.Element.ForeColor;

            //                bbuff.TextBackColor = w.Element.BackColor;
            //                bbuff.DrawTabbedString(w.Text, x, ypos, 0, 0);
            //            }

            //            w.ScreenArea.X = x;
            //            w.ScreenArea.Y = ypos;
            //            x += w.Width;
            //        }

            //        y += r.Height + r.BottomPadd;
            //        if (y > this.Height)
            //            break;
            //    }

            //}
            //catch (Exception x)
            //{
            //    Console.WriteLine(x.Message);
            //}
            //bbuff.RenderToControl(0, 0);
            //bbuff.Dispose();
            //g.Dispose();

		}

		private FormatLabelElement[] CreateElements()
		{
			string text = this.Text.Replace("\n", "");
			text = text.Replace("\r", "");
			string[] parts = text.Split('<');
			ArrayList Elements = new ArrayList();
			int i = 0;
			foreach (string part in parts)
			{
				FormatLabelElement cmd = new FormatLabelElement();

				if (i == 0)
				{
					cmd.Text = part;
				}
				else
				{
					string[] TagTextPair = part.Split('>');
					cmd.Tag = TagTextPair[0].ToLower();
					if (cmd.Tag.IndexOfAny(" \t".ToCharArray()) >= 0)
					{
						int ws = cmd.Tag.IndexOfAny(" \t".ToCharArray());
						string s1 = TagTextPair[0].Substring(0, ws).ToLower();
						string s2 = TagTextPair[0].Substring(ws + 1);
						cmd.Tag = s1 + " " + s2;
					}


					cmd.Text = TagTextPair[1];


					if (cmd.TagName == "img")
					{
						FormatLabelElement img = new FormatLabelElement();
						img.Tag = cmd.Tag;
						Elements.Add(img);
						cmd.Tag = "";
						//	Elements.Add (cmd);					
					}
//
//					if (cmd.TagName == "hr")
//					{
//						Element hr=new Element();
//						hr.Tag = cmd.Tag;					
//						Elements.Add (hr);
//						cmd.Tag ="";
//						cmd.Text ="a";
//						//	Elements.Add (cmd);					
//					}

					cmd.Text = cmd.Text.Replace("\t", "     ");
					cmd.Text = cmd.Text.Replace("&#145;", "'");
					cmd.Text = cmd.Text.Replace("&#146;", "'");


					cmd.Text = cmd.Text.Replace(" ", ((char) 1).ToString());
					cmd.Text = HttpUtility.HtmlDecode(cmd.Text);
					//	cmd.Text =cmd.Text.Replace (" ","*");
					cmd.Text = cmd.Text.Replace(((char) 1).ToString(), " ");


				}


				Elements.Add(cmd);
				i++;
			}

			FormatLabelElement[] res = new FormatLabelElement[Elements.Count];
			Elements.CopyTo(res);
			return res;
		}

		private string GetAttrib(string attrib, string tag)
		{
			try
			{
				if (tag.IndexOf(attrib) < 0)
					return "";

				//tag=tag.Replace("\"","");
				tag = tag.Replace("\t", " ");

				int start = tag.IndexOf(attrib);
				int end = start + attrib.Length;
				int valuestart = tag.IndexOf("=", end);
				if (valuestart < 0)
					return "";
				valuestart++;


				string value = tag.Substring(valuestart);

				while (value.StartsWith(" "))
					value = value.Substring(1);

				//int pos=0;

				if (value.StartsWith("\""))
				{
					// = "value"
					value = value.Substring(1);
					int valueend = value.IndexOf("\"");
					value = value.Substring(0, valueend);
					return value;


				}
				else
				{
					// = value
					int valueend = value.IndexOf(" ");
					if (valueend < 0)
						valueend = value.Length;
					value = value.Substring(0, valueend);
					return value;
				}
				//return "";
			}
			catch
			{
				return "";
			}
		}

		private void ApplyFormat(FormatLabelElement[] Elements)
		{
			Stack bold = new Stack();
			Stack italic = new Stack();
			Stack underline = new Stack();
			Stack forecolor = new Stack();
			Stack backcolor = new Stack();
			Stack fontsize = new Stack();
			Stack fontname = new Stack();
			Stack link = new Stack();
			Stack effectcolor = new Stack();
			Stack effect = new Stack();

			bold.Push(this.Font.Bold);
			italic.Push(this.Font.Italic);
			underline.Push(this.Font.Underline);
			forecolor.Push(this.ForeColor);
			backcolor.Push(Color.Transparent);
			fontsize.Push((int) (this.Font.Size*1.3));
			fontname.Push(this.Font.Name);
			effect.Push(TextEffect.None);
			effectcolor.Push(Color.Black);
			link.Push(null);


			foreach (FormatLabelElement Element in Elements)
			{
				switch (Element.TagName)
				{
					case "b":
						{
							bold.Push(true);
							break;
						}
					case "a":
						{
							//underline.Push (true);
							//forecolor.Push (_l);
							link.Push(Element);
							break;
						}
					case "i":
					case "em":
						{
							italic.Push(true);
							break;
						}
					case "u":
						{
							underline.Push(true);
							break;
						}
					case "font":
						{
							string _fontname = GetAttrib("face", Element.Tag);
							string _size = GetAttrib("size", Element.Tag);
							string _color = GetAttrib("color", Element.Tag);
							string _effectcolor = GetAttrib("effectcolor", Element.Tag);
							string _effect = GetAttrib("effect", Element.Tag);


							if (_size == "")
								fontsize.Push(fontsize.Peek());
							else
								fontsize.Push(int.Parse(_size));

							if (_fontname == "")
								fontname.Push(fontname.Peek());
							else
								fontname.Push(_fontname);

							if (_color == "")
								forecolor.Push(forecolor.Peek());
							else
								forecolor.Push(Color.FromName(_color));

							if (_effectcolor == "")
								effectcolor.Push(effectcolor.Peek());
							else
								effectcolor.Push(Color.FromName(_effectcolor));

							if (_effect == "")
								effect.Push(effect.Peek());
							else
								effect.Push(Enum.Parse(typeof (TextEffect), _effect, true));

							break;

						}
					case "br":
						{
							Element.NewLine = true;
							break;
						}
					case "hr":
						{
							Element.NewLine = true;
							break;
						}
					case "h3":
						{
							fontsize.Push((int) (this.Font.Size*1.4));
							bold.Push(true);
							Element.NewLine = true;
							break;
						}
					case "h4":
						{
							fontsize.Push((int) (this.Font.Size*1.2));
							bold.Push(true);
							Element.NewLine = true;
							break;
						}
					case "/b":
						{
							bold.Pop();
							break;
						}
					case "/a":
						{
							//underline.Pop ();
							//forecolor.Pop ();
							link.Pop();
							break;
						}
					case "/i":
					case "/em":
						{
							italic.Pop();
							break;
						}
					case "/u":
						{
							underline.Pop();
							break;
						}
					case "/font":
						{
							fontname.Pop();
							fontsize.Pop();
							forecolor.Pop();
							effect.Pop();
							effectcolor.Pop();
							break;
						}
					case "/h3":
						{
							fontsize.Pop();
							bold.Pop();
							Element.NewLine = true;
							break;
						}
					case "/h4":
						{
							fontsize.Pop();
							bold.Pop();
							Element.NewLine = true;
							break;
						}

					default:
						{
							break;
						}
				}


				//---------------------------------------------------------------------
				bool Bold = (bool) bold.Peek();
				bool Italic = (bool) italic.Peek();
				bool Underline = (bool) underline.Peek();
				FormatLabelElement Link = (FormatLabelElement) link.Peek();
				string FontName = (string) fontname.Peek();
				int FontSize = (int) fontsize.Peek();
				Color BackColor = (Color) backcolor.Peek();
				Color ForeColor = (Color) forecolor.Peek();
				TextEffect Effect = (TextEffect) effect.Peek();
				Color EffectColor = (Color) effectcolor.Peek();

				FontStyle fs = 0;
				if (Bold) fs |= FontStyle.Bold;
				if (Italic) fs |= FontStyle.Italic;
				if (Underline) fs |= FontStyle.Underline;

				Font font = new Font(FontName, FontSize, fs);
				Element.Font = font;
				Element.BackColor = BackColor;
				Element.ForeColor = ForeColor;
				Element.Link = Link;
				Element.Effect = Effect;
				Element.EffectColor = EffectColor;
			}
		}

		private bool IsIndex(string SRC)
		{
			try
			{
				int i = int.Parse(SRC);
				return true;
			}
			catch
			{
				return false;
			}
		}

		private void CreateWords(FormatLabelElement[] Elements)
		{
			GDISurface bbuff = new GDISurface(1, 1, this, false);

			_HasImageError = false;
			foreach (FormatLabelElement Element in Elements)
			{
				if (Element.TagName == "img")
				{
					Element.words = new FormatLabelWord[1];

					Element.words[0] = new FormatLabelWord();

					Image img = null;

					try
					{
						string SRC = GetAttrib("img", Element.Tag).ToLower();
						if (IsIndex(SRC))
						{
							int index = int.Parse(SRC);
							img = this.ImageList.Images[index];
						}
						else if (SRC.StartsWith("http://")) //from url
						{
						}
						else if (SRC.StartsWith("file://")) // from file
						{
							img = Image.FromFile(SRC.Substring(7));
						}
						else //from file
						{
							img = Image.FromFile(SRC);
						}
					}
					catch
					{
						img = new Bitmap(20, 20);
						_HasImageError = true;
					}

					Element.words[0].Image = img;


					Element.words[0].Element = Element;


					if (img != null)
					{
						Element.words[0].Height = img.Height;
						Element.words[0].Width = img.Width;
						Element.words[0].ScreenArea.Width = img.Width;
						Element.words[0].ScreenArea.Height = img.Height;
					}
				}
				else
				{
					string[] words = Element.Text.Split(' ');
					Element.words = new FormatLabelWord[words.Length];
					int i = 0;
					foreach (string word in words)
					{
						Element.words[i] = new FormatLabelWord();
						string tmp = "";
						Element.words[i].Element = Element;
						if (i == words.Length - 1)
						{
							Element.words[i].Text = word;
							tmp = word;
						}
						else
						{
							Element.words[i].Text = word + " ";
							tmp = word + " "; //last space cant be measured , lets measure an "," instead
						}
						//SizeF size=g.MeasureString (tmp,Element.Font);
						bbuff.Font = GetFont(Element.Font);
						Size s = bbuff.MeasureTabbedString(tmp, 0);
						Element.words[i].Height = s.Height;
						Element.words[i].Width = s.Width - 0;
						Element.words[i].ScreenArea.Width = Element.words[i].Width;
						Element.words[i].ScreenArea.Height = Element.words[i].Height;
						//	Element.words[i].Link =Element.Link ;

						i++;
					}
				}
			}

			bbuff.Dispose();
		}

		private GDIFont GetFont(Font font)
		{
			GDIFont gf = (GDIFont) _Fonts[GetFontKey(font)];
			if (gf == null)
			{
				gf = new GDIFont(font.Name, font.Size, font.Bold, font.Italic, font.Underline, false);
				_Fonts[GetFontKey(font)] = gf;
			}
			return gf;
		}

		private string GetFontKey(Font font)
		{
			return font.Name + font.Bold.ToString() + font.Italic.ToString() + font.Underline.ToString() + font.Size.ToString();

		}


		private void CreateRows()
		{
			if (_Elements != null)
			{
				int x = 0;
				_Rows = new ArrayList();

				//build rows---------------------------------------------
				FormatLabelRow row = new FormatLabelRow();
				_Rows.Add(row);
				bool WhiteSpace = false;
				foreach (FormatLabelElement Element in _Elements)
				{
					if (Element.words == null)
						return;

					if (Element.NewLine)
					{
						//tag forces a new line
						x = 0;
						row = new FormatLabelRow();
						_Rows.Add(row);
						WhiteSpace = true;
					}
					if (Element.TagName == "hr")
					{
						row.RenderSeparator = true;
					}

					//else
					//{


					foreach (FormatLabelWord word in Element.words)
					{
						if (WordWrap)
						{
							int scrollwdh = 0;
							if (ScrollBars == ScrollBars.Both || ScrollBars == ScrollBars.Vertical)
								scrollwdh = vScroll.Width;

							if ((word.Width + x) > this.ClientWidth - LabelMargin - scrollwdh)
							{
								//new line due to wordwrap
								x = 0;
								row = new FormatLabelRow();
								_Rows.Add(row);
								WhiteSpace = true;
							}
						}

						if (word.Text.Replace(" ", "") != "" || word.Image != null)
							WhiteSpace = false;

						if (!WhiteSpace)
						{
							row.Words.Add(word);

							x += word.Width;
						}
					}
					//}
				}

				//apply width and height to all rows
				int index = 0;
				foreach (FormatLabelRow r in this._Rows)
				{
					int width = 0;
					int height = 0;
					int padd = 0;

					if (index > 0)
					{
						int previndex = index - 1;
						FormatLabelRow prev = (FormatLabelRow) _Rows[previndex];
						while (previndex >= 0 && prev.Words.Count == 0)
						{
							prev = (FormatLabelRow) _Rows[previndex];
							previndex--;
						}

						if (previndex >= 0)
						{
							prev = (FormatLabelRow) _Rows[previndex];
							if (prev.Words.Count > 0)
							{
								FormatLabelWord w = (FormatLabelWord) prev.Words[prev.Words.Count - 1];
								height = w.Height;
							}
						}

					}


					foreach (FormatLabelWord w in r.Words)
					{
						if (w.Height > height && (w.Text != ""))
							height = w.Height;

						width += w.Width;

					}
					r.Height = height;

					int MaxImageH = 0;
					foreach (FormatLabelWord w in r.Words)
					{
						if (w.Image != null)
						{
							if (w.Height > height)
								MaxImageH = w.Height;
						}
					}

					foreach (FormatLabelWord w in r.Words)
					{
						int imgH = 0;
						int imgPadd = 0;
						if (w.Image != null)
						{
							string valign = GetAttrib("valign", w.Element.Tag);
							switch (valign)
							{
								case "top":
									{
										imgH = r.Height;
										imgPadd = w.Height - imgH;
										break;
									}
								case "middle":
								case "center":
									{
										int tmp = 0;
										imgH = r.Height;
										tmp = (w.Height - imgH)/2;
										imgH += tmp;
										imgPadd = tmp;

										break;
									}
								case "bottom":
									{
										imgH = w.Height;
										imgPadd = 0;
										break;
									}
								default:
									{
										imgH = w.Height;
										imgPadd = 0;
										break;
									}

							}

							if (imgH > height)
								height = imgH;

							if (imgPadd > padd)
								padd = imgPadd;


							width += w.Width;
						}
					}
					r.Width = width;
					r.Height = height;
					r.BottomPadd = padd;
					index++;
				}

				this.vScroll.Maximum = this._Rows.Count;
			}
		}


		private void InitScrollbars()
		{
			if (vScroll == null || hScroll == null)
				return;

			if (this.ScrollBars == ScrollBars.Both)
			{
				vScroll.Left = this.ClientWidth - vScroll.Width;
				vScroll.Top = 0;
				vScroll.Height = this.ClientHeight - hScroll.Height;

				hScroll.Left = 0;
				hScroll.Top = this.ClientHeight - hScroll.Height;
				hScroll.Width = this.ClientWidth - vScroll.Width;

				Filler.Left = vScroll.Left;
				Filler.Top = hScroll.Top;

				Filler.Visible = true;
				vScroll.Visible = true;
				hScroll.Visible = true;
			}
			else if (this.ScrollBars == ScrollBars.Vertical)
			{
				vScroll.Left = this.ClientWidth - vScroll.Width;
				vScroll.Top = 0;
				vScroll.Height = this.ClientHeight;

				hScroll.Left = 0;
				hScroll.Top = this.ClientHeight - hScroll.Height;
				hScroll.Width = this.ClientWidth - vScroll.Width;

				Filler.Left = vScroll.Left;
				Filler.Top = hScroll.Top;

				Filler.Visible = false;
				vScroll.Visible = true;
				hScroll.Visible = false;

			}
			else if (this.ScrollBars == ScrollBars.Horizontal)
			{
				vScroll.Left = this.ClientWidth - vScroll.Width;
				vScroll.Top = 0;
				vScroll.Height = this.ClientHeight;

				hScroll.Left = 0;
				hScroll.Top = this.ClientHeight - hScroll.Height;
				hScroll.Width = this.ClientWidth;

				Filler.Left = vScroll.Left;
				Filler.Top = hScroll.Top;

				Filler.Visible = false;
				vScroll.Visible = false;
				hScroll.Visible = true;

			}
			else if (this.ScrollBars == ScrollBars.None)
			{
				vScroll.Left = this.ClientWidth - vScroll.Width;
				vScroll.Top = 0;
				vScroll.Height = this.ClientHeight;

				hScroll.Left = 0;
				hScroll.Top = this.ClientHeight - hScroll.Height;
				hScroll.Width = this.ClientWidth;

				Filler.Left = vScroll.Left;
				Filler.Top = hScroll.Top;

				Filler.Visible = false;
				vScroll.Visible = false;
				hScroll.Visible = false;

			}

		}


		protected override void OnResize(EventArgs e)
		{
			try
			{
				InitScrollbars();
				SetAutoSize();

			}
			catch
			{
			}
			CreateRows();
			base.OnResize(e);
		}

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            if (this.IsHandleCreated)
                RedrawBuffer();
        }
		protected override void OnMouseUp(MouseEventArgs e)
		{
			int y = e.Y;
			int x = e.X;

			int index = 0;
			bool Link = false;
			//this.Cursor =Cursors.Arrow;
			_ActiveElement = null;
			if (this._Rows != null)
			{
				foreach (FormatLabelRow r in this._Rows)
				{
					if (y >= r.Top && y <= r.Top + r.Height)
					{
						foreach (FormatLabelWord w in r.Words)
						{
							if (y >= w.ScreenArea.Top && y <= w.ScreenArea.Bottom)
							{
								if (x >= w.ScreenArea.Left && x <= w.ScreenArea.Right)
								{
									//MessageBox.Show (w.Text);
									if (w.Element.Link != null)
									{
										Link = true;
										_ActiveElement = w.Element.Link;
										break;
									}
									//this.Cursor =Cursors.Hand;

								}
							}
						}
						break;
					}
					index++;
				}
			}
			if (Link)
			{
				this.Cursor = Cursors.Hand;
				this.Invalidate();
				OnClickLink(GetAttrib("href", _ActiveElement.Tag));
			}
			else
			{
				this.Cursor = Cursors.Arrow;
				this.Invalidate();
			}


			base.OnMouseUp(e);
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			int y = e.Y;
			int x = e.X;

			int index = 0;
			bool Link = false;
			//this.Cursor =Cursors.Arrow;
			_ActiveElement = null;
			if (this._Rows != null)
			{
				foreach (FormatLabelRow r in this._Rows)
				{
					if (y >= r.Top && y <= r.Top + r.Height)
					{
						foreach (FormatLabelWord w in r.Words)
						{
							if (y >= w.ScreenArea.Top && y <= w.ScreenArea.Bottom)
							{
								if (x >= w.ScreenArea.Left && x <= w.ScreenArea.Right)
								{
									//MessageBox.Show (w.Text);
									if (w.Element.Link != null)
									{
										Link = true;
										_ActiveElement = w.Element.Link;
										break;
									}
									//this.Cursor =Cursors.Hand;									
								}
							}
						}
						break;
					}
					index++;
				}
			}
			if (Link)
			{
				this.Cursor = Cursors.Hand;
				this.Invalidate();
			}
			else
			{
				this.Cursor = Cursors.Arrow;
				this.Invalidate();
			}
			base.OnMouseMove(e);
		}

		private void vScroll_Scroll(object sender, ScrollEventArgs e)
		{
			this.Invalidate();
		}


		public ScrollBars ScrollBars
		{
			get { return _ScrollBars; }
			set
			{
				_ScrollBars = value;
				InitScrollbars();
			}
		}

		public int GetWidth()
		{
			int max = 0;
			foreach (FormatLabelRow r in this._Rows)
			{
				if (r.Width > max)
					max = r.Width;
			}

			return max + LabelMargin*2 + this.BorderWidth*2;
		}

		public int GetHeight()
		{
			int max = 0;
			foreach (FormatLabelRow r in this._Rows)
			{
				max += r.Height;
			}

			return max + LabelMargin*2 + this.BorderWidth*2;
		}
	}
}