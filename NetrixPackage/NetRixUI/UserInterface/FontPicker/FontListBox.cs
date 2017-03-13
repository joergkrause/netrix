using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;

namespace GuruComponents.Netrix.UserInterface.FontPicker
{
	/// <summary>
	///  Enhances the font listbox to show currently selected fonts.
	/// </summary>
	/// <remarks>
	/// This class provides the elementary list which is able to shows the fontnames 
	/// in there appropriate font.
	/// </remarks>
	[ToolboxItem(false)]
	public class FontListBox : System.Windows.Forms.ListBox
	{

        /// <summary>
        /// Used as left space between border and the beginning of font. In pixel.
        /// </summary>
        private const int imageWidth = 16;
        /// <summary>
        /// Current font.
        /// </summary>
        private Font nfont;
        /// <summary>
        /// Type of listbox.
        /// </summary>
        private ListBoxType _listtype = ListBoxType.FontNameAndSample;
        /// <summary>
        /// Max width, unlimited in 0.
        /// </summary>
        private int maxwid = 0;
        /// <summary>
        /// Sample string to display fonts.
        /// </summary>
        private string samplestr = " - NET.RIX";
        /// <summary>
        /// Default font list shown.
        /// </summary>
        private FontFamilyType type = FontFamilyType.System;
        /// <summary>
        /// Array of user fonts.
        /// </summary>
        private string[] _userfonts = null;


        /// <summary>
        /// The contructor is used to instantiate the control.
        /// </summary>
        /// <remarks>
        /// Some base values are set:
        /// <list type="bullet">
        /// <item><term>IntegralHeight</term><description>false</description></item>
        /// <item><term>Sorted</term><description>false</description></item>
        /// <item><term>DrawMode</term><description>DrawMode.OwnerDrawVariable</description></item>
        /// <item><term>FamilyType</term><description>FontFamilyType.Web</description></item>
        /// </list>
        /// The list of selected fonts is always clear.
        /// </remarks>
		public FontListBox() : base()
		{
            IntegralHeight = false;
            Sorted = false;
            DrawMode = DrawMode.OwnerDrawVariable;	
            this.SelectionMode = SelectionMode.MultiExtended;
            this.FamilyType = FontFamilyType.Web;
			this.Items.Clear();
		}
    
        /// <summary>
        /// Select a specific entry.
        /// </summary>
        /// <param name="index"></param>
		private void Select (int index)
		{
			this.SelectedIndex = index;
//			string s = this.Items[index].ToString();
//			Rectangle rect = GetItemRectangle(index);			
		}

        /// <summary>
        /// Set the highlighter ones down in the list.
        /// </summary>
        /// <remarks>
        /// This method can be used if the list is used as separate control to
        /// set the highlighted entry depending on an external event.
        /// </remarks>
		public void HighlightNext()
		{
			this.SelectedIndex++;
		}
		
        /// <summary>
        /// Set the highlighter ones up in the list.
        /// </summary>
        /// <remarks>
        /// This method can be used if the list is used as separate control to
        /// set the highlighted entry depending on an external event.
        /// </remarks>
        public void HighlightPrevious()
		{
			this.SelectedIndex--;
		}

		/// <summary>
		/// Number of elements in the listbox.
		/// </summary>
		/// <remarks>
		/// This returns simply the <c>Count</c> property of the base list.
		/// </remarks>
        public int Count
		{
			get
			{
				return Items.Count;
			}
		}

        /// <summary>
        /// Recognizes the F2 (select), Insert (select and take), Del (remove), Enter (select),
        /// KeyDown/KeyUp (moves current font).
        /// </summary>
        /// <param name="e"></param>
		protected override void OnKeyDown(KeyEventArgs e)
		{
			if( e.KeyData == Keys.F2 )
			{
				int index = SelectedIndex;

				if( index == ListBox.NoMatches || index == 65535 )
				{
					if( Items.Count > 0 )
					{
						index = 0;
					}
				}
				if (index != ListBox.NoMatches && index != 65535)
				{
					this.Select(index);
				}
			}       
			else if (e.KeyData == Keys.Insert)
			{
                if (this.Items.Count > 0)
				    this.SelectedIndex = 0;
			}
			else if (e.KeyData == Keys.Delete)
			{
				this.RemoveSelected();
			}
			else if (e.KeyData == Keys.Enter)
			{
                if (this.Items.Count > 0)
				    this.SelectedIndex = 0;
			}
			else if( ( e.KeyCode == Keys.Down ) && e.Control )
			{
				this.MoveSelectedDown();
				if( this.SelectedIndex > 0 )
				{
					this.SelectedIndex--;
				}
			}
			else if( ( e.KeyCode == Keys.Up ) && e.Control )
			{
				this.MoveSelectedUp();
				if( this.SelectedIndex < ( this.Items.Count - 1 ) )
				{
					this.SelectedIndex++;
				}
			}

			base.OnKeyDown( e );
		}

		/// <summary>
		/// Called if a new item is inserted.
		/// </summary>
		/// <remarks>
		/// If called from outside the control the item will be added to the list and 
		/// control will refresh. If it has no focus the focus is moved there.
		/// The method checks for the given font name and will not insert the font more
		/// than once. The font is compared using the name, not the real font file.
		/// The method does nothing if the font already exists in the list.
		/// </remarks>
		/// <param name="FontName"></param>
        public void NewItem(string FontName)
		{
			if (!this.Items.Contains(FontName))
			{
				base.Items.Add(FontName);
				this.Select(base.Items.Count - 1);
                this.Focus();
			}
		}

        /// <summary>
        /// Called if the current font should removed.
        /// </summary>
        /// <remarks>
        /// If one or more lines highlighted within the list this method removes these
        /// fonts from the list.
        /// </remarks>
		public void RemoveSelected()
		{
			if( this.Items.Count > 0 )
			{
				this.BeginUpdate();
				int newItems = this.Items.Count - this.SelectedIndices.Count;
				object[] temp = new object[newItems];
				for (int i = 0, j = 0; i < this.Items.Count; i++)
				{
					if (!this.SelectedIndices.Contains(i))
					{
						temp[j++] = this.Items[i];
					}
				}
				this.Items.Clear();
				this.Items.AddRange(temp);
				if (this.Items.Count > 0)
				{
                    this.Select(0);
				}
				this.EndUpdate();
                this.Focus();
			}
		}

		/// <summary>
		/// Move one up.
		/// </summary>
		/// <remarks>
		/// The currently selected font will be moved upwards in the list.
		/// This method expects that exactly one (1) font is selected. If no font or
		/// more than one is selected the method ignores the call and does nothing.
		/// </remarks>
        public void MoveSelectedUp()
		{
			object tmp;
			int index;
			if(this.SelectedIndices.Count == 1 && this.SelectedIndex > 0)
			{
				tmp = this.SelectedItem;
				index = this.SelectedIndex;
				this.Items.RemoveAt( index );
				this.Items.Insert( index - 1, tmp );
                this.Select(index - 1);
                this.Focus();
                this.SelectedIndex = index - 1;
			}
		}

        /// <summary>
        /// Move one down.
        /// </summary>
        /// <remarks>
        /// The currently selected font will be moved downwards in the list.
        /// This method expects that exactly one (1) font is selected. If no font or
        /// more than one is selected the method ignores the call and does nothing.
        /// </remarks>
        public void MoveSelectedDown()
		{
			int index = this.SelectedIndex;
			if (this.SelectedIndices.Count == 1 && (index + 1) < this.Items.Count)
			{
				object tmp = this.SelectedItem;
				this.Items.RemoveAt(index);
				this.Items.Insert(index + 1, tmp);
                this.Select(index + 1);
                this.Focus();
                this.SelectedIndex = index + 1;
			}
		}
		

        # region Base

        /// <summary>
        /// Used to override the sample string.
        /// </summary>
        /// <remarks>
        /// Gets or sets the sample string.
        /// </remarks>
        public string SampleString
        {
            set
            {
                samplestr = value;
            }
            get
            {
                return samplestr;
            }
        }

        /// <summary>
        /// Used to set or get the font family type.
        /// </summary>
        /// <remarks>
        /// If the list is prefilled with fonts (which is an option) this property 
        /// determines the type of font list. If the list is used for the final font selection the
        /// user mades the type <see cref="FontFamilyType.User"/> is used.
        /// </remarks>
        public FontFamilyType FamilyType
        {
            set
            {
                type = value;                
            }
            get
            {
                return type;
            }
        }

        /// <summary>
        /// Used to get or set the user fonts.
        /// </summary>
        /// <remarks>
        /// If the list type is user and the prefilled user list is used this property allows to
        /// set the list of fonts as an string array.
        /// </remarks>
        public string[] UserFonts
        {
            set
            {
                _userfonts = value;
            }
            get
            {
                return _userfonts;
            }
        }

        /// <summary>
        /// Used to set or get the listbox type.
        /// </summary>
        /// <remarks>
        /// Related to the base list the control derived from.
        /// </remarks>
        public ListBoxType ListType
        {
            set
            {
                _listtype = value;
            }
            get
            {
                return _listtype;
            }
        }

        /// <summary>
        /// Used to fill the listbox with a predefined font list.
        /// </summary>
        /// <remarks>
        /// After the various property are set this method will fill the list with the
        /// selected fonts. After setting the font the selected index is set to 0.
        /// </remarks>
        public void Populate()
        {
            switch (type)
            {
                case FontFamilyType.Generic:
                    Items.Clear();
                    Items.AddRange(new string[] {"Sans-Serif", "Monospace", "Serif"});
                    break;
                case FontFamilyType.System:
                    FontFamilyTypeSystem:
                        Items.Clear();
                    foreach (FontFamily ff in FontFamily.Families)
                    {
                        if(ff.IsStyleAvailable(FontStyle.Regular))
                            Items.Add(ff.Name);												
                    }
                    break;
                case FontFamilyType.Web:
                    Items.Clear();
                    Items.AddRange(new string[] {"Verdana", "Tahoma", "Courier New", "Times"});
                    break;
                case FontFamilyType.User:
                    Items.Clear();
                    if (UserFonts != null)
                    {
                        Items.AddRange(UserFonts);
                    }
                    break;
                default:
                    goto FontFamilyTypeSystem;
            }			
            if(Items.Count > 0)
                SelectedIndex=0;
        }

        /// <summary>
        /// Measures one entry in the owner drawn listbox.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMeasureItem(System.Windows.Forms.MeasureItemEventArgs e)
        {	
            if(e.Index > -1 && Items.Count > 0)
            {
                int w = 0;
                string fontstring = Items[e.Index].ToString();						
                Graphics g = CreateGraphics();
                e.ItemHeight = (int)g.MeasureString(fontstring, new Font(fontstring,10)).Height;
                w = (int)g.MeasureString(fontstring, new Font(fontstring,10)).Width;
                if(ListType == ListBoxType.FontNameAndSample)
                {
                    int h1 = (int)g.MeasureString(samplestr, new Font(fontstring,10)).Height;
                    int h2 = (int)g.MeasureString(Items[e.Index].ToString(), new Font("Arial",10)).Height;
                    int w1 = (int)g.MeasureString(samplestr, new Font(fontstring,10)).Width;
                    int w2 = (int)g.MeasureString(Items[e.Index].ToString(), new Font("Arial",10)).Width;
                    if(h1 > h2 )
                        h2 = h1;
                    e.ItemHeight = h2;
                    w = w1 + w2;
                }
                w += imageWidth * 2;
                if(w > maxwid)
                    maxwid=w;
                if(e.ItemHeight > 20)
                    e.ItemHeight = 20;
            }						
            base.OnMeasureItem(e);
        }

        /// <summary>
        /// Creates on entry in the owner drawn listbox.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnDrawItem(System.Windows.Forms.DrawItemEventArgs e)
        {	
            if(e.Index > -1 && Items.Count > 0)
            {
                string fontstring = Items[e.Index].ToString();
                switch (type)
                {
                    case FontFamilyType.Generic:
                        switch (fontstring)
                        {
                            case "Sans-Serif":
                                nfont = new Font("Arial",10);
                                break;
                            case "Serif":
                                nfont = new Font("Times New Roman",10);
                                break;
                            case "Monospace":
                                nfont = new Font("Courier New",10);
                                break;
                        }
                        break;
                    case FontFamilyType.System:
                        nfont = new Font(fontstring, 10);
                        break;
                    case FontFamilyType.Web:
                        nfont = new Font(fontstring, 10);
                        break;
                    case FontFamilyType.User:
                        nfont = new Font(fontstring, 10);
                        break;
                }
                Font afont = new Font("Arial",10);
                if (nfont == null) return;
                switch (ListType)
                {
                    case ListBoxType.FontNameAndSample:
                        Graphics g = CreateGraphics();
                        int w = (int)g.MeasureString(fontstring, afont).Width;

                        if((e.State & DrawItemState.Selected)==0)
                        {
                            e.Graphics.FillRectangle(new SolidBrush(SystemColors.Window),
                                e.Bounds.X + imageWidth,e.Bounds.Y,e.Bounds.Width,e.Bounds.Height);
                            e.Graphics.DrawString(fontstring,afont,new SolidBrush(SystemColors.WindowText),
                                e.Bounds.X + imageWidth,e.Bounds.Y);	
                            e.Graphics.DrawString(samplestr,nfont,new SolidBrush(SystemColors.WindowText),
                                e.Bounds.X+w + imageWidth,e.Bounds.Y);	
                        }
                        else
                        {
                            e.Graphics.FillRectangle(new SolidBrush(SystemColors.Highlight),
                                e.Bounds.X + imageWidth,e.Bounds.Y,e.Bounds.Width,e.Bounds.Height);
                            e.Graphics.DrawString(fontstring,afont,new SolidBrush(SystemColors.HighlightText),
                                e.Bounds.X + imageWidth,e.Bounds.Y);
                            e.Graphics.DrawString(samplestr,nfont,new SolidBrush(SystemColors.HighlightText),
                                e.Bounds.X+w + imageWidth,e.Bounds.Y);
                        }	
                        break;
                    case ListBoxType.FontSample:
                        if((e.State & DrawItemState.Selected)==0)
                        {
                            e.Graphics.FillRectangle(new SolidBrush(SystemColors.Window),
                                e.Bounds.X + imageWidth,e.Bounds.Y,e.Bounds.Width,e.Bounds.Height);
                            e.Graphics.DrawString(fontstring,nfont,new SolidBrush(SystemColors.WindowText),
                                e.Bounds.X + imageWidth,e.Bounds.Y);
					
                        }
                        else
                        {
                            e.Graphics.FillRectangle(new SolidBrush(SystemColors.Highlight),
                                e.Bounds.X + imageWidth,e.Bounds.Y,e.Bounds.Width,e.Bounds.Height);
                            e.Graphics.DrawString(fontstring,nfont,new SolidBrush(SystemColors.HighlightText),
                                e.Bounds.X + imageWidth,e.Bounds.Y);
                        }
                        break;
                }
                //e.Graphics.DrawImage(image, new Point(e.Bounds.X, e.Bounds.Y)); 
            }
            base.OnDrawItem(e);
        }

        # endregion

	}
}
