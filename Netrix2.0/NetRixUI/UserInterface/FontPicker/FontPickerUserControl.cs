using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Data;
using System.Windows.Forms;
using System.Diagnostics;
using FPU = GuruComponents.Netrix.UserInterface.FontPicker;

namespace GuruComponents.Netrix.UserInterface.FontPicker
{
    /// <summary>
    /// This class realises the FontPicker UserControl
    /// </summary>
    /// <remarks>
    /// The user control shows a font selection box and allows
    /// the user to selected multiple fonts from different lists. Used for the style sheet editor and
    /// in the PropertyGrid.
    /// <para>
    /// To protect the user interface the width of the control is limited to 168 pixel.
    /// </para>
    /// </remarks>
    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(GuruComponents.Netrix.UserInterface.ResourceManager), "ToolBox.FontPicker.ico")]
    [Designer(typeof(GuruComponents.Netrix.UserInterface.NetrixUIDesigner))]
    public class FontPickerUserControl : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button btnAddEdit;
		private System.Windows.Forms.Button btnRemove;
		private System.Windows.Forms.Button btnUp;
		private System.Windows.Forms.Button btnDown;
		private FPU.FontListBox fontListBox;
		private FPU.FontPickerBox ff;
		private System.Windows.Forms.Label labelInnerTitle;
        private ContentChangedEventHandler contentChangedHandler;
		private System.Windows.Forms.Panel panelControl;
        private ImageList imageListFF;
        private IContainer components;

		#region Component Designer generated code


        /// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if( components != null )
					components.Dispose();
			}
			base.Dispose( disposing );
        }

		/// <summary>
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FontPickerUserControl));
            this.btnAddEdit = new System.Windows.Forms.Button();
            this.imageListFF = new System.Windows.Forms.ImageList(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnUp = new System.Windows.Forms.Button();
            this.btnDown = new System.Windows.Forms.Button();
            this.labelInnerTitle = new System.Windows.Forms.Label();
            this.panelControl = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.panelControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnAddEdit
            // 
            this.btnAddEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddEdit.BackColor = System.Drawing.Color.White;
            this.btnAddEdit.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnAddEdit.ImageIndex = 4;
            this.btnAddEdit.ImageList = this.imageListFF;
            this.btnAddEdit.Location = new System.Drawing.Point(160, 0);
            this.btnAddEdit.Name = "btnAddEdit";
            this.btnAddEdit.Size = new System.Drawing.Size(32, 23);
            this.btnAddEdit.TabIndex = 1;
            this.btnAddEdit.UseVisualStyleBackColor = false;
            this.btnAddEdit.Click += new System.EventHandler(this.btnAddEdit_Click);
            // 
            // imageListFF
            // 
            this.imageListFF.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListFF.ImageStream")));
            this.imageListFF.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListFF.Images.SetKeyName(0, "AddFont.ico");
            this.imageListFF.Images.SetKeyName(1, "RemoveFont.ico");
            this.imageListFF.Images.SetKeyName(2, "FontUp.ico");
            this.imageListFF.Images.SetKeyName(3, "FontDown.ico");
            this.imageListFF.Images.SetKeyName(4, "AddFont.ico");
            this.imageListFF.Images.SetKeyName(5, "RemoveFont.ico");
            this.imageListFF.Images.SetKeyName(6, "FontUp.ico");
            this.imageListFF.Images.SetKeyName(7, "FontDown.ico");
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            this.panel1.Controls.Add(this.btnAddEdit);
            this.panel1.Controls.Add(this.btnRemove);
            this.panel1.Controls.Add(this.btnUp);
            this.panel1.Controls.Add(this.btnDown);
            this.panel1.Controls.Add(this.labelInnerTitle);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(288, 23);
            this.panel1.TabIndex = 0;
            // 
            // btnRemove
            // 
            this.btnRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRemove.BackColor = System.Drawing.Color.White;
            this.btnRemove.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnRemove.ImageIndex = 5;
            this.btnRemove.ImageList = this.imageListFF;
            this.btnRemove.Location = new System.Drawing.Point(192, 0);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(32, 23);
            this.btnRemove.TabIndex = 2;
            this.btnRemove.UseVisualStyleBackColor = false;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnUp
            // 
            this.btnUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUp.BackColor = System.Drawing.Color.White;
            this.btnUp.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnUp.ImageIndex = 6;
            this.btnUp.ImageList = this.imageListFF;
            this.btnUp.Location = new System.Drawing.Point(224, 0);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(32, 23);
            this.btnUp.TabIndex = 3;
            this.btnUp.UseVisualStyleBackColor = false;
            this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
            // 
            // btnDown
            // 
            this.btnDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDown.BackColor = System.Drawing.Color.White;
            this.btnDown.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnDown.ImageIndex = 7;
            this.btnDown.ImageList = this.imageListFF;
            this.btnDown.Location = new System.Drawing.Point(256, 0);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(32, 23);
            this.btnDown.TabIndex = 4;
            this.btnDown.UseVisualStyleBackColor = false;
            this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
            // 
            // labelInnerTitle
            // 
            this.labelInnerTitle.Location = new System.Drawing.Point(0, 5);
            this.labelInnerTitle.Name = "labelInnerTitle";
            this.labelInnerTitle.Size = new System.Drawing.Size(192, 16);
            this.labelInnerTitle.TabIndex = 1;
            this.labelInnerTitle.Text = "Font";
            // 
            // panelControl
            // 
            this.panelControl.AllowDrop = true;
            this.panelControl.Controls.Add(this.panel1);
            this.panelControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl.Location = new System.Drawing.Point(0, 0);
            this.panelControl.Name = "panelControl";
            this.panelControl.Size = new System.Drawing.Size(288, 144);
            this.panelControl.TabIndex = 2;
            // 
            // FontPickerUserControl
            // 
            this.AllowDrop = true;
            this.Controls.Add(this.panelControl);
            this.Name = "FontPickerUserControl";
            this.Size = new System.Drawing.Size(288, 144);
            this.panel1.ResumeLayout(false);
            this.panelControl.ResumeLayout(false);
            this.ResumeLayout(false);

        }
		#endregion

        /// <summary>
        /// The constructor is used to instantiate the control.
        /// </summary>
        /// <remarks>
        /// This method sets the basix values and the localization. The list of selected fonts
        /// is clear.
        /// </remarks>
		public FontPickerUserControl()
		{			
            this.fontListBox = new GuruComponents.Netrix.UserInterface.FontPicker.FontListBox();
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
            InitializeListbox();
            SetUp();
		}

        internal FontPickerUserControl(bool @internal)
        {
            this.fontListBox = new GuruComponents.Netrix.UserInterface.FontPicker.FontListBox();
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();
            InitializeListbox();
            SetUp();
        }

        private void InitializeListbox()
        {
            //// 
            //// fontListBox
            //// 
            this.fontListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));
            this.fontListBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.fontListBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.fontListBox.FamilyType = GuruComponents.Netrix.UserInterface.FontPicker.FontFamilyType.Web;
            this.fontListBox.IntegralHeight = false;
            this.fontListBox.ListType = GuruComponents.Netrix.UserInterface.FontPicker.ListBoxType.FontNameAndSample;
            this.fontListBox.Location = new System.Drawing.Point(0, 24);
            this.fontListBox.Name = "fontListBox";
            this.fontListBox.SampleString = " - NET.RIX";
            this.fontListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.fontListBox.Size = new System.Drawing.Size(288, 120);
            this.fontListBox.TabIndex = 1;
            this.fontListBox.UserFonts = null;
            //
            this.panelControl.Controls.Add(this.fontListBox);
        }

        private void SetUp()
        {
            this.ff = new FPU.FontPickerBox();
            this.ff.Name = "ff";
            this.ff.Opacity = 1;
            this.ff.ShowInTaskbar = false;
            this.ff.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("FontPickerUserControl.ff.Text"); //"Font Selection Box";
            this.ff.Visible = false;
            this.ff.StartPosition = FormStartPosition.Manual;
            this.ff.Location = this.PointToScreen(new System.Drawing.Point(0, 0));

            btnAddEdit.GotFocus	+= new EventHandler( ButtonGotFocus );
            btnRemove.GotFocus	+= new EventHandler( ButtonGotFocus );
            btnUp.GotFocus		+= new EventHandler( ButtonGotFocus );
            btnDown.GotFocus	+= new EventHandler( ButtonGotFocus );

            fontListBox.Items.Clear();
            this.fontListBox.DragDrop += new DragEventHandler(OnDragDrophandler);
            this.fontListBox.DragEnter += new DragEventHandler(OnDragEnterHandler);

        }

        /// <summary>
        /// The current Assembly version.
        /// </summary>                   
        /// <remarks>
        /// This property returns the current assembly version to inform during design session about the assembly loaded.
        /// </remarks>
        [Browsable(true), Category("NetRix"), Description("Current Version. ReadOnly.")]
        public string Version
        {
            get
            {
                return this.GetType().Assembly.GetName().Version.ToString();
            }
        }

		private void OnDragEnterHandler(object sender, DragEventArgs e)
		{
			e.Effect = DragDropEffects.Copy;
		}

		private void OnDragDrophandler(object sender, DragEventArgs e)
		{
			object o = e.Data.GetData(DataFormats.Text);
			if (o != null)
			{
				this.fontListBox.NewItem(o.ToString());
			} 
			else 
			{
				System.Drawing.Text.PrivateFontCollection pfc = new System.Drawing.Text.PrivateFontCollection();
				try
				{					
					string[] FontName = (string[]) e.Data.GetData(DataFormats.FileDrop, true);
					for (int f = 0; f < FontName.Length; f++)
					{
						pfc.AddFontFile(FontName[f]);
					}
					if (pfc.Families.Length > 0) 
					{
						for (int i = 0; i < pfc.Families.Length; i++)
						{
							this.fontListBox.NewItem(((FontFamily)pfc.Families[i]).Name);
						}
					}					
				}
				finally
				{
					pfc.Dispose();
				}
			}
		}


        /// <summary>
        /// Reset the UI.
        /// </summary>
        /// <remarks>
        /// The call will reset the localizable strings for the current culture.
        /// </remarks>
        public void ResetUI()
        {
            this.ff.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("FontPickerUserControl.ff.Text"); //"Font Selection Box";
            this.ff.ResetUI();
        }

		/// <summary>
		/// Caption of the window.
		/// </summary>
		/// <remarks>
		/// This sets the text of the font box. It is not necessary to set this option because the
		/// value is loaded from the satellite assemblies. Setting of this property is only required
		/// if a different text is necessary.
		/// </remarks>
        [Browsable(true)]
        [Description("Caption of the window.")]
        [Category("NetRix")]
        public string BoxCaption
		{
			set
			{
				this.ff.Text = value;
			}
			get
			{
				return this.ff.Text;
			}
		}

        /// <summary>
        /// The sample string used in the font list.
        /// </summary>
        /// <remarks>
        /// This presets the sample string shown in the font list.
        /// </remarks>
        [Browsable(true)]
        [Description("Sample string, shown after each font entry.")]
        [Category("NetRix")]
        public string SampleString
		{
			set
			{
				this.fontListBox.SampleString = value;
				this.ff.SampleString = value;
			}
			get
			{
				return this.fontListBox.SampleString;
			}
		}

		/// <summary>
		/// Adds or removes the event handler for the event fired if any of the content is changed
		/// </summary>
		/// <remarks>
		/// The event is fired if one of the fields has had a interaction with the user.
		/// </remarks>
        [Browsable(true)]
        [Description("Event handler for the event fired if any of the content is changed.")]
        [Category("NetRix")]
        public event ContentChangedEventHandler ContentChanged
		{
			add
			{
				contentChangedHandler += value;
			}
			remove
			{
				contentChangedHandler -= value;
			}
		}

		/// <summary>
		/// Fired if the content of the list has changed.
		/// </summary>
		/// <param name="sender"></param>
        private void OnContentChanged(object sender)
		{
            if (contentChangedHandler != null) 
            {
                contentChangedHandler(sender, new EventArgs());
            }
		}

        /// <summary>
        /// Gets or sets the list of fonts using a comma separated string.
        /// </summary>
        /// <remarks>
        /// This property transform the string collection used internally to populate a 
        /// comma separated list. This is for direct usage in the face or font-family attributes
        /// which requires such a list.
        /// </remarks>
        [Browsable(true)]
        [Description("Gets or sets the list of fonts using a comma separated string.")]
        [Category("NetRix")]
        public string PopulateStringList
		{
			get
			{
				string[] list = new string[fontListBox.Items.Count];
				fontListBox.Items.CopyTo(list, 0);
				return String.Join(",", list);
			}
            set
            {
                fontListBox.Items.Clear();
                if (!value.Equals(String.Empty))
                {
                    fontListBox.Items.AddRange(value.ToString().Split(','));
                }
            }
		}

        /// <summary>
        /// Helps to protect the minimum measures of the control to more than 168 pixel.
        /// </summary>
        /// <remarks>
        /// This is necessary to show the buttons and description text properly.
        /// </remarks>
        /// <param name="e"></param>
		protected override void OnResize( EventArgs e )
		{
			if( this.Width < 168 )
			{
				this.Width = 168;
			}
			base.OnResize( e );
		}

		/// <summary>
		/// Populates the inner list to transparent access.
		/// </summary>
		/// <remarks>
		/// If the final application needs other types of font lists this property allows
		/// the access to the base class list. This property is read only.
		/// </remarks>
        [Browsable(false)]
        public System.Windows.Forms.ListBox.ObjectCollection Items
		{
			get
			{
				return this.fontListBox.Items;
			}
		}

		/// <summary>
		/// Gets or sets the border style around the control.
		/// </summary>
		[Browsable(true)]
		[Description("Gets or sets the border style around the control.")]
		[Category("NetRix")]
		public BorderStyle Border
		{
			get
			{
				return this.panelControl.BorderStyle;
			}
			set
			{
				this.panelControl.BorderStyle = value;
			}
		}

		/// <summary>
		/// Allows the user to drop fonts onto the control.
		/// </summary>
		/// <remarks>
		/// If dropping is allowed the listbox (not the entire control) will accept
		/// fontnames (if strings are the data) or font objects from which the string is
		/// extracted.
		/// <para>
		/// The control does not allow adding fonts more than once. If a font is added as string the whole string
		/// is treated as one name. To allow users to add multiple fonts at the same time use the data type 
		/// <see cref="System.Windows.Forms.DataFormats.FileDrop">FileDrop</see>. The control expects that the path points
		/// to a TrueType or OpenType font which exists on the local computer. The name will than be added to the
		/// list. Font variants (Arial, Arial Bold, Arial Italics) are treated as one font and added only once, 
		/// as long as the fonts have the same base font.
		/// </para>
		/// <para>
		/// Note: The control cannot be used as a drop source in the current version.
		/// </para>
		/// </remarks>
		/// <example>
		/// Assuming you have a button on your form and the AllowDrop property set to <c>true</c>. The button has
		/// a property <c>Tag</c> set to "Arial". The following code will create a drag drop solution for adding
		/// fonts to the list:
		/// <code>
		/// private void button_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		/// {
		///	   button1.DoDragDrop(((Button) sender).Tag.ToString(), DragDropEffects.Copy);
		///	}
		/// </code>
		/// </example>
		[Browsable(true)]
		[Description("Allows the user to drop fonts onto the control.")]
		[Category("NetRix")]
		public new bool AllowDrop
		{
			get
			{				
				return this.fontListBox.AllowDrop;
			}
			set
			{
				this.fontListBox.AllowDrop = value;
			}
		}

        /// <summary>
        /// Called if the button got the focus.
        /// </summary>
        /// <remarks>
        /// Used to set the focus to the list instead to the button after the button was clicked.
        /// </remarks>
        /// <param name="e">The event arguments to put through to the handler.</param>
		protected override void OnGotFocus( EventArgs e )
		{
			ButtonGotFocus( null, EventArgs.Empty );
		}
		
        private void ButtonGotFocus( object sender, EventArgs e )
		{
			fontListBox.Focus();
		}
		
        /// <summary>
        /// Called if the Add (+) button has clicked. Shows the fontpicker list form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddEdit_Click( object sender, EventArgs e )
		{
			this.ff.Location = this.PointToScreen(new System.Drawing.Point(0, 0));
			if (ff.ShowDialog() == DialogResult.OK)
			{
                fontListBox.BeginUpdate();
				foreach (string s in ff.SelectedFont)
				{					
					fontListBox.NewItem(s);
					fontListBox.SelectedIndex = -1;					
				}
                fontListBox.EndUpdate();
			}
			this.OnContentChanged(sender);
		}

		/// <summary>
		/// Gets or sets the Title of the Control, displayed left hand above the list.
		/// </summary>
		/// <remarks>
		/// The title should only be set if the localized string is not appropriate.
		/// </remarks>
		[Browsable(true)]
        [Description("Gets or sets the Title of the Control, displayed left hand above the list.")]
        [Category("NetRix")]
		public string Title
		{
			get
			{
				return labelInnerTitle.Text;
			}
			set
			{
				labelInnerTitle.Text = value;
			}
		}

		/// <summary>
		/// Called if the Remove (-) button was clicked.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
        private void btnRemove_Click(object sender, System.EventArgs e)
		{
			fontListBox.RemoveSelected();
			this.OnContentChanged(sender);
		}

		/// <summary>
		/// Called if the Up button was clicked.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
        private void btnUp_Click(object sender, System.EventArgs e)
		{
			fontListBox.MoveSelectedUp();
			this.OnContentChanged(sender);
		}

		/// <summary>
		/// Called if the down button was clicked
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
        private void btnDown_Click(object sender, System.EventArgs e)
		{
			fontListBox.MoveSelectedDown();
			this.OnContentChanged(sender);
		}

	}
}
