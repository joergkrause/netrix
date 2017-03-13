using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace GuruComponents.Netrix.UserInterface.ColorPicker
{
	/// <summary>
	/// A Color picker control specialized for HTML colors.
	/// </summary>
	/// <remarks>
	/// This is the basic color control. It provides a button to pop-up a color selection box,
	/// a textbox for entering colors as names or RGB values and a context menu for a few simple
	/// configuration options.
	/// <para>
	/// The control has two selection modes:
	/// <list type="bullet">
	/// <item><see cref="GuruComponents.Netrix.UserInterface.ColorPicker.ColorPanel">ColorPanel</see></item>
	/// <item><see cref="GuruComponents.Netrix.UserInterface.ColorPicker.ColorSlider">ColorSlider</see></item>
	/// </list>
	/// Whereas the <see cref="GuruComponents.Netrix.UserInterface.ColorPicker.ColorSlider">ColorSlider</see> supports
	/// the whole RGB space the <see cref="GuruComponents.Netrix.UserInterface.ColorPicker.ColorPanel">ColorPanel</see> has
	/// three specialized panes:
	/// <list type="bullet">
	/// <item><term>Web</term><description>Contains the 216 web safe colors as a grid.</description></item>
	/// <item><term>Custom</term><description>Allows the user to add his own color collection.</description></item>
	/// <item><term>System</term><description>The standard system colors used by Windows.</description></item>
	/// </list>
	/// </para>
	/// </remarks>
    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(GuruComponents.Netrix.UserInterface.ResourceManager), "ToolBox.ColorEditor.ico")]
    [Designer(typeof(NetrixUIDesigner))]
    public class ColorPickerUserControl : UserControl
	{
		private ContextMenu contextMenuColorPicker;
		private TextBox tbColor;
		private Button buttonContextMenu;
		//
		const bool			defaultDisplayColor		= true;
		const bool			defaultDisplayColorName	= false;

	    private ColorSliderForm sliderPopUp			= null;
		private bool		bDisplayColor			= defaultDisplayColor;
		private bool		bDisplayColorName		= defaultDisplayColorName;
		private MenuItem	menuItemColorDisplayColorGrid;
		private MenuItem	menuItemColorDisplayRgbSlider;
		private ToolTip	toolTipColorName;
		private MenuItem	menuItemDisableColor;
		private IContainer components;

	    private Color								panelColor;
		private ColorPanelForm colorPanelPopUp;		// external form as popup
		private Button buttonSelectColor;
        private MenuItem menuItem4;
        /// <summary>
        /// The constructor, used to instantiate the control.
        /// </summary>
        /// <remarks>
        /// This constructor sets various default values. The list of custom colors is empty.
        /// </remarks>
		public ColorPickerUserControl()
		{   
            InitializeComponent();
            panelColor = ColorPanel.defaultColor;
            this.colorPanelPopUp = new ColorPanelForm();
            this.sliderPopUp = new ColorSliderForm();
            this.colorPanelPopUp.Columns = 18;
            ResetControl();
            Paint();
		}

        internal ColorPickerUserControl(bool @internal) 
        {
            InitializeComponent();
            panelColor            = ColorPanel.defaultColor;
            this.colorPanelPopUp  = new ColorPanelForm();
            this.sliderPopUp = new ColorSliderForm();
            this.colorPanelPopUp.Columns = 18;
            ResetControl();
            Paint();
        }

		/// <summary>
		/// Sets the control in a undefined state.
		/// </summary>
		/// <remarks>
		/// This resets the current color and removes the list of custom colors.
		/// </remarks>
		public void ResetControl()
		{
			ResetControl(new List<Color>());
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

		/// <summary>
		/// Sets the control in an undefined state.
		/// </summary>
		/// <remarks>
		/// This is used to inform other controls
		/// to not rewrite the value in the control. Additionally the custom color table 
		/// is filled with a array of colors.
		/// </remarks>
		/// <param name="customiser">Array of colors to customize the palette.</param>
        public void ResetControl(List<Color> customiser)
		{
			this.tbColor.TextChanged -= new EventHandler(this.tbColor_TextChanged);
			this.toolTipColorName.SetToolTip(this.tbColor, "No value assigned");
			colorPanelPopUp.CustomColors = customiser;
			this.tbColor.Text = String.Empty;
			this.panelColor = Color.Empty;
			this.buttonSelectColor.BackColor = Color.FromKnownColor(KnownColor.Control);
            this.colorPanelPopUp.SetUp();
			this.Refresh();
			this.tbColor.TextChanged += new EventHandler(this.tbColor_TextChanged);
            Paint();
		}

        /// <summary>
        /// Sets the caption of the sub windows of both controls.
        /// </summary>
        /// <remarks>
        /// This option can be used to localize or to customize the control. The value appears as the
        /// text property of both pop-up forms, 
        /// <see cref="GuruComponents.Netrix.UserInterface.ColorPicker.ColorPanel">ColorPanel</see> and <see cref="GuruComponents.Netrix.UserInterface.ColorPicker.ColorSlider">ColorSlider</see>.
        /// </remarks>
        public string SetPanelCaption
        {
            set
            {
                this.colorPanelPopUp.Text = value;
                this.sliderPopUp.Text = value;
            }
        }

        /// <summary>
        /// Set the menu item names (caption) of the context menu.
        /// </summary>
        /// <remarks>
        /// This option can be used to localize or to customize the control.
        /// </remarks>
        /// <param name="disableMenuText">Menu entry 'Reset color'.</param>
        /// <param name="selectGridMenuText">Menu entry 'Show Grid'.</param>
        /// <param name="selectSliderMenuText">Menu entry 'Show Slider'.</param>
        public void SetMenuNames(string disableMenuText, string selectGridMenuText, string selectSliderMenuText)
        {
            this.menuItemColorDisplayColorGrid.Text = selectGridMenuText;
            this.menuItemColorDisplayRgbSlider.Text = selectSliderMenuText;
            this.menuItemDisableColor.Text = disableMenuText;
        }

		/// <summary>
		/// Die verwendeten Ressourcen bereinigen.
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

		#region Component Designer generated code
		/// <summary>
		/// Erforderliche Methode für die Designerunterstützung. 
		/// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ColorPickerUserControl));
            this.contextMenuColorPicker = new System.Windows.Forms.ContextMenu();
            this.menuItemDisableColor = new System.Windows.Forms.MenuItem();
            this.menuItem4 = new System.Windows.Forms.MenuItem();
            this.menuItemColorDisplayColorGrid = new System.Windows.Forms.MenuItem();
            this.menuItemColorDisplayRgbSlider = new System.Windows.Forms.MenuItem();
            this.buttonContextMenu = new System.Windows.Forms.Button();
            this.tbColor = new System.Windows.Forms.TextBox();
            this.toolTipColorName = new System.Windows.Forms.ToolTip(this.components);
            this.buttonSelectColor = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // contextMenuColorPicker
            // 
            this.contextMenuColorPicker.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemDisableColor,
            this.menuItem4,
            this.menuItemColorDisplayColorGrid,
            this.menuItemColorDisplayRgbSlider});
            // 
            // menuItemDisableColor
            // 
            this.menuItemDisableColor.Index = 0;
            this.menuItemDisableColor.Text = "&Reset Color";
            this.menuItemDisableColor.Click += new System.EventHandler(this.menuItemDisableColor_Click);
            // 
            // menuItem4
            // 
            this.menuItem4.Index = 1;
            this.menuItem4.Text = "-";
            // 
            // menuItemColorDisplayColorGrid
            // 
            this.menuItemColorDisplayColorGrid.Checked = true;
            this.menuItemColorDisplayColorGrid.Index = 2;
            this.menuItemColorDisplayColorGrid.RadioCheck = true;
            this.menuItemColorDisplayColorGrid.Text = "Use Color Grid";
            this.menuItemColorDisplayColorGrid.Click += new System.EventHandler(this.menuItemColorDisplayColorGrid_Click);
            // 
            // menuItemColorDisplayRgbSlider
            // 
            this.menuItemColorDisplayRgbSlider.Index = 3;
            this.menuItemColorDisplayRgbSlider.RadioCheck = true;
            this.menuItemColorDisplayRgbSlider.Text = "Use RGB &Slider";
            this.menuItemColorDisplayRgbSlider.Click += new System.EventHandler(this.menuItemColorDisplayRgbSlider_Click);
            // 
            // buttonContextMenu
            // 
            this.buttonContextMenu.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonContextMenu.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonContextMenu.Image = ((System.Drawing.Image)(resources.GetObject("buttonContextMenu.Image")));
            this.buttonContextMenu.Location = new System.Drawing.Point(115, 1);
            this.buttonContextMenu.Name = "buttonContextMenu";
            this.buttonContextMenu.Size = new System.Drawing.Size(21, 20);
            this.buttonContextMenu.TabIndex = 0;
            this.buttonContextMenu.Click += new System.EventHandler(this.buttonContextMenu_Click);
            this.buttonContextMenu.MouseHover += new System.EventHandler(this.buttonContextMenu_Click);
            // 
            // tbColor
            // 
            this.tbColor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbColor.Location = new System.Drawing.Point(61, 1);
            this.tbColor.Name = "tbColor";
            this.tbColor.Size = new System.Drawing.Size(52, 20);
            this.tbColor.TabIndex = 1;
            this.tbColor.Text = "#000000";
            this.toolTipColorName.SetToolTip(this.tbColor, "No Color Name");
            this.tbColor.Leave += new System.EventHandler(this.tbColor_Leave);
            // 
            // toolTipColorName
            // 
            this.toolTipColorName.AutomaticDelay = 200;
            // 
            // buttonSelectColor
            // 
            this.buttonSelectColor.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSelectColor.Location = new System.Drawing.Point(1, 1);
            this.buttonSelectColor.Name = "buttonSelectColor";
            this.buttonSelectColor.Size = new System.Drawing.Size(57, 20);
            this.buttonSelectColor.TabIndex = 4;
            this.buttonSelectColor.Click += new System.EventHandler(this.buttonSelectColor_Click);
            // 
            // ColorPickerUserControl
            // 
            this.Controls.Add(this.buttonSelectColor);
            this.Controls.Add(this.tbColor);
            this.Controls.Add(this.buttonContextMenu);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.MaximumSize = new System.Drawing.Size(1000, 21);
            this.MinimumSize = new System.Drawing.Size(140, 21);
            this.Name = "ColorPickerUserControl";
            this.Size = new System.Drawing.Size(140, 21);
            this.Resize += new System.EventHandler(this.GenesisColor_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
		#endregion

		private void buttonContextMenu_Click(object sender, EventArgs e)
		{
			this.contextMenuColorPicker.Show(this.buttonContextMenu, new Point(5, 5));
		}

        /// <summary>
        /// Refreshes the control if the focus is gotten.
        /// </summary>
        /// <remarks>
        /// This refreshes the textbox and the panel colors if the control got the focus.
        /// </remarks>
        /// <param name="e">The event arguments send from the caller.</param>
        protected override void OnGotFocus(EventArgs e)
		{
			base.OnGotFocus(e);
			this.Refresh();
		}

        /// <summary>
        /// Refreshes the control if the focus is lost.
        /// </summary>
        /// <remarks>
        /// This refreshes the textbox and the panel colors if the control lost the focus.
        /// </remarks>
        /// <param name="e">The event arguments send from the caller.</param>
		protected override void OnLostFocus(EventArgs e)
		{
			base.OnLostFocus(e);
			this.Refresh();
		}

        /// <summary>
        /// Overrides the OnPaint event to control the button painting.
        /// </summary>
        /// <remarks>
        /// This is used to display the color name and the colored button background.
        /// </remarks>
        protected new void Paint()
        {
            if (this.Enabled && !DesignMode)
            {
                this.SuspendLayout();
                if (this.panelColor == Color.Empty)
                {
                    this.buttonSelectColor.BackColor = Color.FromKnownColor(KnownColor.Control);
                    this.tbColor.Text = "";
                    this.toolTipColorName.SetToolTip(this.tbColor, "No Color Defined");
                }
                else
                {
                    Color c = Color.FromArgb(panelColor.R, panelColor.G, panelColor.B);
                    this.buttonSelectColor.BackColor = this.panelColor;
                    if (this.bDisplayColor)
                    {
                        this.tbColor.Text = ColorName(c); // String.Format("#{0:X2}{1:X2}{2:X2}", c.R, c.G, c.B);
                    }
                    else
                    {
                        this.tbColor.Text = String.Empty;
                    }
                    if (this.bDisplayColorName)
                    {
                        this.tbColor.Text += String.Format(" ({0})", ColorTranslator.ToHtml(c));
                    }
                    this.toolTipColorName.SetToolTip(this.tbColor, ColorTranslator.ToHtml(c));
                }
                this.ResumeLayout();
            }
        }

        private static string ColorName(Color c)
        {
            if (c.Name == "0") return "";
            if (c.IsNamedColor) return c.Name;
            foreach (KnownColor kcc in Enum.GetValues(typeof(KnownColor)))
            {
                if (kcc.ToString().StartsWith("Active") || kcc.ToString().StartsWith("Control") || kcc.ToString().StartsWith("Highlight") || kcc.ToString().StartsWith("Info") || kcc.ToString().StartsWith("Menu") || kcc.ToString().StartsWith("Window") || kcc.ToString().StartsWith("Transparent")) continue;
                Color cc = Color.FromKnownColor(kcc);
                if (cc.R == c.R && cc.G == c.G && cc.B == c.B) return kcc.ToString();
            }
            return ColorTranslator.ToHtml(c).ToUpper();
        }

		/// <summary>
		/// Opens popup then Down arrow or Down arrow with Alt key is hit.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		protected override bool IsInputKey(Keys key)
		{
			bool IsDropKey = true;
			if (key == Keys.Down || (key == (Keys.Down | Keys.Alt)))
			{
				ShowPopUp();
			} 
			else 
			{
				IsDropKey = base.IsInputKey(key);
			}
			return IsDropKey;
		}
		/// <summary>
		/// Captures the mouse and show context menu.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			if (e.Button == MouseButtons.Right)
			{
				// show context menu exactly at mouse position
				this.contextMenuColorPicker.Show(this, new Point(e.X, e.Y));
			}
		}

        /// <summary>
        /// Shows the selected pop-up window.
        /// </summary>
        /// <remarks>
        /// This method shows the pop-up, whatever was selected.
        /// </remarks>
		public void ShowPopUp()
		{
			if (this.menuItemColorDisplayRgbSlider.Checked)
			{
				ShowRgbSlider();
			} 
			else if (this.menuItemColorDisplayColorGrid.Checked)
			{
				ShowColorGrid();
			}
		}

		private void ShowColorGrid()
		{
			Point q = this.PointToScreen(Point.Empty);
			this.colorPanelPopUp.StartPosition = FormStartPosition.Manual;
			this.colorPanelPopUp.Top = q.Y;
			this.colorPanelPopUp.Left = q.X;
			this.colorPanelPopUp.Color = panelColor;
			if(DialogResult.OK == this.colorPanelPopUp.ShowDialog())	
			{
				panelColor = this.colorPanelPopUp.Color;
                Paint();
				// raise event to inform the outer space
				OnColorChanged(new ColorChangedEventArgs(panelColor));
			} 
            this.Refresh();		// to force Tooltip and Textbox display
		}

        //private void colorPanelControl_ColorChanged(object sender, ColorChangedEventArgs e)
        //{
        //    this.panelColor = e.Color;
        //    Paint();
        //    OnColorChanged(e);
        //}

		private void ShowRgbSlider()
		{
			if (null == this.sliderPopUp)	// first time create slider, then let changes untouched
			{
                this.sliderPopUp = new ColorSliderForm();
			}
            Point p = new Point( this.Left, this.Bottom );
            Point q = this.PointToScreen(p);
            this.sliderPopUp.Top = q.Y - 150;
            this.sliderPopUp.Left = q.X - 50;
            this.sliderPopUp.Color = panelColor;
            if(DialogResult.OK == this.sliderPopUp.ShowDialog())
			{
				this.panelColor = this.sliderPopUp.Color;
                Paint();
				OnColorChanged(new ColorChangedEventArgs(panelColor));
				this.Refresh();		// to force Tooltip and Textbox display
			}			
		}

		/// <summary>
		/// Fired if the color has changed or resetted.
		/// </summary>
		public event ColorChangedEventHandler ColorChanged;

        /// <summary>
        /// Called if the color has changed.
        /// </summary>
        /// <remarks>
        /// used to fire the external color change event.
        /// </remarks>
        /// <param name="e">The event args contain the color.</param>
		protected virtual void OnColorChanged(ColorChangedEventArgs e)
		{
			if (ColorChanged != null)
			{
				ColorChanged(this, e);
			}
		}

        /// <summary>
        /// Reorganize the buttons on resize.
        /// </summary>
        /// <remarks>
        /// The definition for the control is simple: The textbox will take two third of the control and 
        /// the colored pop-up button will take one third. The context menu button will always set to a
        /// constant width. If the control is stretched in the VS.NET designer the both controls are 
        /// stretched relatively.
        /// </remarks>
        /// <param name="sender">This control.</param>
        /// <param name="e">Event arguments.</param>
		public virtual void GenesisColor_Resize(object sender, EventArgs e)
		{
			int iY = 1; // always on top of control
			this.buttonContextMenu.Location = new Point(this.Width - this.buttonContextMenu.Width - 1, iY);
			this.tbColor.Location = new Point((int)Math.Floor((double)(this.Width - this.buttonContextMenu.Width) * 1/3), iY);
			this.tbColor.Width = (int) Math.Floor((double)(this.Width - this.buttonContextMenu.Width) * 2/3) - 1;
			this.buttonSelectColor.Width = (int) Math.Floor((double)(this.Width - this.buttonContextMenu.Width) * 1/3) - 1;
			if (this.Width < 100)
			{
				this.Width = 100;
			}
			Invalidate(true);
		}

		private void menuItemColorDisplayColorGrid_Click(object sender, EventArgs e)
		{
			this.menuItemColorDisplayColorGrid.Checked = true;
			this.menuItemColorDisplayRgbSlider.Checked = false;
		}

		private void menuItemColorDisplayRgbSlider_Click(object sender, EventArgs e)
		{
			this.menuItemColorDisplayColorGrid.Checked = false;
			this.menuItemColorDisplayRgbSlider.Checked = true;
		}

		private void menuItemDisableColor_Click(object sender, EventArgs e)
		{
			ResetControl();
			// fire the event in the case of disabled color, too
			OnColorChanged(new ColorChangedEventArgs(Color.Empty));
		}

		private void buttonSelectColor_Click(object sender, EventArgs e)
		{
			ShowPopUp();
		}

		private void tbColor_TextChanged(object sender, EventArgs e)
		{
			try
			{
				if (this.tbColor.Text.Length == 7 && this.tbColor.Text[0] == '#')
				{
                    CurrentColor = ColorTranslator.FromHtml(this.tbColor.Text);
				} 
				this.buttonSelectColor.BackColor = CurrentColor;
			}
			catch
			{
				CurrentColor = Color.Empty;
				this.buttonSelectColor.BackColor = Color.FromKnownColor(KnownColor.Control);
			}
		}

		private void tbColor_Leave(object sender, EventArgs e)
		{
			try
			{
                if (this.tbColor.Text.Length == 0)
                {
                    this.ResetControl();
                    return;
                }
				if (this.tbColor.Text.Length == 7 && this.tbColor.Text[0] == '#')
				{
					CurrentColor = ColorTranslator.FromHtml(this.tbColor.Text);
				} 
				else 
				{
					CurrentColor = Color.FromName(this.tbColor.Text);
					this.buttonSelectColor.BackColor = CurrentColor;
				}
				this.OnColorChanged(new ColorChangedEventArgs(CurrentColor));
			}
			catch
			{
				ResetControl();
			}
		}



	    /// <summary>
	    /// Sets or gets the current color.
	    /// </summary>
	    /// <remarks>
	    /// Setting the value forces a refresh to show the value immediataly.
	    /// </remarks>
		public Color CurrentColor
		{
			get
			{
				return this.panelColor;
			}
			set
			{
				this.panelColor = value;
                Paint();
				this.Refresh();
			}
		}
	}
}