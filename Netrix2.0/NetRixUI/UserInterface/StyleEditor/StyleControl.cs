using System;
using System.Text;
using System.Globalization;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using GuruComponents.Netrix.UserInterface.StyleParser;
using CPU = GuruComponents.Netrix.UserInterface.ColorPicker;
using FPU = GuruComponents.Netrix.UserInterface.FontPicker;
using SPP = GuruComponents.Netrix.UserInterface.StyleParser;
using System.Reflection;
using System.Collections.Generic;

namespace GuruComponents.Netrix.UserInterface.StyleEditor
{
    /// <summary>
    /// This is the style editor control, which build the complete graphical user interface
    /// </summary>
    /// <remarks>
    /// It is used for editing of one style definition. It accepts and returns a string. It makes use
    /// of the style parser to built an object tree from the definitions. The control can be embedded
    /// as an standalone user control.
    /// </remarks>
    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(GuruComponents.Netrix.UserInterface.ResourceManager), "ToolBox.StyleEditor.ico")]
    [Designer(typeof(GuruComponents.Netrix.UserInterface.NetrixUIDesigner))]
    public class StyleControl : System.Windows.Forms.UserControl
    {
        # region Control Elements
        private CssParser parser;
        private SPP.StyleObject _so;
        private NumberFormatInfo CultureStyle;
        private System.Windows.Forms.RadioButton radioButtonPositionRemove;
        private System.Windows.Forms.RadioButton radioButtonDirectionRtl;
        private System.Windows.Forms.RadioButton radioButtonDirectionLtr;
        private System.Windows.Forms.RadioButton radioButtonDirectionRemove;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.RadioButton radioButtonListBulletImage;
        private System.Windows.Forms.RadioButton radioButtonListBulletStyle;
        private System.Windows.Forms.RadioButton radioButtonListBulletNone;
        private Control foundControl;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBoxTextTransform;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBoxFontWeight;
        private System.Windows.Forms.CheckBox checkBoxFontVariantSmallCaps;
        private System.Windows.Forms.CheckBox checkBoxFontStyleItalic;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox comboBoxFontSizeAbsolute;
        private System.Windows.Forms.ComboBox comboBoxFontSizeUnit;
        private System.Windows.Forms.RadioButton radioButtonFontSizeAbsolute;
        private System.Windows.Forms.RadioButton radioButtonFontSizeUnit;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label50;
        private System.Windows.Forms.Label label49;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label label52;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox comboBoxBackgroundPosition;
        private System.Windows.Forms.ComboBox comboBoxBackgroundPositionY;
        private System.Windows.Forms.ComboBox comboBoxBackgroundPositionX;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox comboBoxBackgroundAttachment;
        private System.Windows.Forms.ComboBox comboBoxBackgroundRepeat;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.CheckBox checkBox6;
        private System.Windows.Forms.GroupBox groupBox18;
        private System.Windows.Forms.Label label56;
        private System.Windows.Forms.Label label55;
        private System.Windows.Forms.Label label54;
        private System.Windows.Forms.Label label53;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.ComboBox comboBoxPaddingTopUnit;
        private System.Windows.Forms.ComboBox comboBoxMarginTopUnit;
        private System.Windows.Forms.ComboBox comboBoxMarginLeftUnit;
        private System.Windows.Forms.ComboBox comboBoxPaddingLeftUnit;
        private System.Windows.Forms.ComboBox comboBoxMarginBottomUnit;
        private System.Windows.Forms.ComboBox comboBoxPaddingBottomUnit;
        private System.Windows.Forms.ComboBox comboBoxMarginRightUnit;
        private System.Windows.Forms.ComboBox comboBoxPaddingRightUnit;
        private System.Windows.Forms.ComboBox comboBoxBorderTopStyle;
        private System.Windows.Forms.ComboBox comboBoxBorderTopWidthUnit;
        private System.Windows.Forms.ComboBox comboBoxBorderLeftStyle;
        private System.Windows.Forms.ComboBox comboBoxBorderLeftWidthUnit;
        private System.Windows.Forms.ComboBox comboBoxBorderRightStyle;
        private System.Windows.Forms.ComboBox comboBoxBorderRightWidthUnit;
        private System.Windows.Forms.ComboBox comboBoxBorderBottomWidthUnit;
        private System.Windows.Forms.ComboBox comboBoxBorderBottomStyle;
        private System.Windows.Forms.GroupBox groupBox21;
        private System.Windows.Forms.ComboBox comboBoxTextDecoration;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.Label label51;
        private System.Windows.Forms.ComboBox comboBoxTextIdentUnit;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.Label label37;
        private System.Windows.Forms.ComboBox comboBoxWordSpacingUnit;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.ComboBox comboBoxLineHeightUnit;
        private System.Windows.Forms.ComboBox comboBoxLetterSpacingUnit;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.GroupBox groupBox20;
        private System.Windows.Forms.ComboBox comboBoxStyleVerticalAlignment;
        private System.Windows.Forms.GroupBox groupBox19;
        private System.Windows.Forms.RadioButton radioButtonTextAlignDisable;
        private System.Windows.Forms.RadioButton radioButtonTextAlignJustify;
        private System.Windows.Forms.RadioButton radioButtonTextAlignCenter;
        private System.Windows.Forms.RadioButton radioButtonTextAlignRight;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.RadioButton radioButtonTextAlignLeft;
        private System.Windows.Forms.GroupBox groupBox10;
        private System.Windows.Forms.ComboBox comboBoxStyleLineBreak;
        private System.Windows.Forms.Label label48;
        private System.Windows.Forms.ComboBox comboBoxStyleOverFlow;
        private System.Windows.Forms.Label label47;
        private System.Windows.Forms.ComboBox comboBoxDisplay;
        private System.Windows.Forms.Label label45;
        private System.Windows.Forms.GroupBox groupBox9;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ComboBox comboBoxWidthUnit;
        private System.Windows.Forms.ComboBox comboBoxHeightUnit;
        private System.Windows.Forms.ComboBox comboBoxLeftUnit;
        private System.Windows.Forms.ComboBox comboBoxTopUnit;
        private System.Windows.Forms.RadioButton radioButtonPositionAbsolute;
        private System.Windows.Forms.RadioButton radioButtonPositionRelative;
        private System.Windows.Forms.RadioButton radioButtonPositionFixed;
        private System.Windows.Forms.RadioButton radioButtonPositionNone;
        private System.Windows.Forms.GroupBox groupBox14;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.ComboBox comboBoxListStylePosition;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.ComboBox comboBoxListStyleType;
        private System.Windows.Forms.GroupBox groupBox23;
        private System.Windows.Forms.ComboBox comboBoxCommonRuleWidth;
        private System.Windows.Forms.Label label42;
        private System.Windows.Forms.Label label41;
        private System.Windows.Forms.Label label40;
        private System.Windows.Forms.ComboBox comboBoxColumnRuleStyle;
        private System.Windows.Forms.Label label39;
        private System.Windows.Forms.GroupBox groupBox16;
        private System.Windows.Forms.Label label44;
        private System.Windows.Forms.Label label43;
        private System.Windows.Forms.Label label38;
        private System.Windows.Forms.ComboBox comboBoxCaptionSide;
        private System.Windows.Forms.Label label65;
        private System.Windows.Forms.Label label66;
        private System.Windows.Forms.Label label67;
        private System.Windows.Forms.Label label68;
        private System.Windows.Forms.Label label69;
        private System.Windows.Forms.Label label71;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.TabControl tabControlEditor;
        private System.Windows.Forms.TabPage tabPageFonts;
        private System.Windows.Forms.TabPage tabPageBackground;
        private System.Windows.Forms.TabPage tabPageBorders;
        private System.Windows.Forms.TabPage tabPageLayout;
        private System.Windows.Forms.TabPage tabPagePosition;
        private System.Windows.Forms.TabPage tabPageLists;
        private System.Windows.Forms.TabPage tabPageMisc;
        private System.Windows.Forms.TabPage tabPageTables;
        private System.Windows.Forms.GroupBox groupBox22;
        private System.Windows.Forms.RadioButton radioButtonVisiblityRemove;
        private System.Windows.Forms.RadioButton radioButtonVisibilityNone;
        private System.Windows.Forms.RadioButton radioButtonVisibilityHidden;
        private System.Windows.Forms.RadioButton radioButtonVisibilityVisible;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.NumericUpDown numericUpDownTextIndent;
        private System.Windows.Forms.GroupBox groupBox15;
        private System.Windows.Forms.Label label70;
        private System.Windows.Forms.PictureBox pictureBox9;
        private System.Windows.Forms.Label label64;
        private System.Windows.Forms.Label label63;
        private System.Windows.Forms.Label label62;
        private System.Windows.Forms.Label label61;
        private System.Windows.Forms.Label label60;
        private System.Windows.Forms.Label label59;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.ComboBox comboBoxCursor;
        private System.Windows.Forms.Label label58;
        private System.Windows.Forms.NumericUpDown numericUpDownLineHeight;
        private System.Windows.Forms.NumericUpDown numericUpDownLetterSpacing;
        private System.Windows.Forms.NumericUpDown numericUpDownWordSpacing;
        private System.Windows.Forms.Label label36;
        private System.Windows.Forms.NumericUpDown numericUpDownFontSize;
        private System.Windows.Forms.NumericUpDown numericUpDownBackgroundPositionY;
        private System.Windows.Forms.NumericUpDown numericUpDownBackgroundPositionX;
        private System.Windows.Forms.NumericUpDown numericUpDownMarginLeft;
        private System.Windows.Forms.NumericUpDown numericUpDownPaddingLeft;
        private System.Windows.Forms.NumericUpDown numericUpDownBorderLeftWidth;
        private System.Windows.Forms.NumericUpDown numericUpDownPaddingBottom;
        private System.Windows.Forms.NumericUpDown numericUpDownMarginBottom;
        private System.Windows.Forms.NumericUpDown numericUpDownPaddingRight;
        private System.Windows.Forms.NumericUpDown numericUpDownBorderRightWidth;
        private System.Windows.Forms.NumericUpDown numericUpDownMarginRight;
        private System.Windows.Forms.NumericUpDown numericUpDownPaddingTop;
        private System.Windows.Forms.NumericUpDown numericUpDownBorderTopWidth;
        private System.Windows.Forms.NumericUpDown numericUpDownMarginTop;
        private System.Windows.Forms.NumericUpDown numericUpDownLeft;
        private System.Windows.Forms.NumericUpDown numericUpDownTop;
        private System.Windows.Forms.NumericUpDown numericUpDownWidth;
        private System.Windows.Forms.NumericUpDown numericUpDownHeight;
        private System.Windows.Forms.NumericUpDown numericUpDownRuleWidth;
        private System.Windows.Forms.NumericUpDown numericUpDownColumnSpan;
        private System.Windows.Forms.NumericUpDown numericUpDownRowSpan;
        private CPU.ColorPickerUserControl colorPickerColor;
        private CPU.ColorPickerUserControl colorPickerBackgroundColor;
        private CPU.ColorPickerUserControl colorPickerBorderBottom;
        private CPU.ColorPickerUserControl colorPickerBorderRight;
        private CPU.ColorPickerUserControl colorPickerBorderLeft;
        private CPU.ColorPickerUserControl colorPickerBorderTop;
        private CPU.ColorPickerUserControl colorPickerColumn;
        private CPU.ColorPickerUserControl colorPickerScrollTrack;
        private CPU.ColorPickerUserControl colorPicker3Dlight;
        private CPU.ColorPickerUserControl colorPickerScrollHighlight;
        private CPU.ColorPickerUserControl colorPickerScrollDarkShadow;
        private CPU.ColorPickerUserControl colorPickerScrollShadow;
        private CPU.ColorPickerUserControl colorPickerScrollBase;
        private CPU.ColorPickerUserControl colorPickerScrollArrow;
        private CPU.ColorPickerUserControl colorPickerScrollFace;
        private System.Windows.Forms.Button buttonBackgroundImage;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.TextBox textBoxBackgroundImage;
        private System.Windows.Forms.Button buttonListStyleImage;
        private System.Windows.Forms.TextBox textBoxListStyleImage;
        private FPU.FontPickerUserControl fontPickerFont;
        private System.Windows.Forms.NumericUpDown numericUpDownColumns;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.NumericUpDown numericUpDownBorderBottomWidth;
        private System.Windows.Forms.PictureBox pictureBox5;
        private System.Windows.Forms.PictureBox pictureBox6;
        private System.Windows.Forms.PictureBox pictureBox7;
        private System.Windows.Forms.PictureBox pictureBox8;
        private System.Windows.Forms.PictureBox pictureBox10;
        private System.Windows.Forms.PictureBox pictureBox11;
        private System.Windows.Forms.PictureBox pictureBox12;
        private System.Windows.Forms.PictureBox pictureBox13;
        private System.Windows.Forms.PictureBox pictureBox14;
        private System.Windows.Forms.NumericUpDown numericUpDownColumnGap;
        private System.Windows.Forms.ComboBox comboBoxColumnGap;
        private System.Windows.Forms.Label labelColumnGap;
        private System.Windows.Forms.PictureBox pictureBox15;
        private System.Windows.Forms.TabControl tabControlBorders;
        private System.Windows.Forms.TabPage tabPageAll;
        private System.Windows.Forms.TabPage tabPageLeft;
        private System.Windows.Forms.TabPage tabPageRight;
        private System.Windows.Forms.TabPage tabPageTop;
        private System.Windows.Forms.TabPage tabPageBottom;
        private System.Windows.Forms.GroupBox groupBox12;
        private System.Windows.Forms.GroupBox groupBox13;
        private System.Windows.Forms.GroupBox groupBox17;
        private System.Windows.Forms.GroupBox groupBox24;
        private System.Windows.Forms.GroupBox groupBox25;
        private System.Windows.Forms.NumericUpDown numericUpDownMarginAll;
        private System.Windows.Forms.ImageList imageListBorderTabs;
        private System.Windows.Forms.NumericUpDown numericUpDownBorderWidthAll;
        private GuruComponents.Netrix.UserInterface.ColorPicker.ColorPickerUserControl colorPickerBorderColorAll;
        private System.Windows.Forms.ComboBox comboBoxBorderStyleAll;
        private System.Windows.Forms.ComboBox comboBoxBorderWidthAll;
        private System.Windows.Forms.NumericUpDown numericUpDownPaddingAll;
        private System.Windows.Forms.ComboBox comboBoxMarginAll;
        private System.Windows.Forms.ComboBox comboBoxPaddingAll;
        private System.ComponentModel.IContainer components;

        private bool _supressEvents;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.Label label33;
        private System.Windows.Forms.Label label34;
        private System.Windows.Forms.Label label35;
        private System.Windows.Forms.Label label46;
        private System.Windows.Forms.Label label57;
        private System.Windows.Forms.Label label72;
        private System.Windows.Forms.Label label73;
        private System.Windows.Forms.Label label74;
        private System.Windows.Forms.Label label75;
        private System.Windows.Forms.Label label76;
        # endregion
        # region StyleProperties

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.PictureBox pictureBox4;
        private System.Windows.Forms.GroupBox groupBox11;

        # endregion

        private System.Windows.Forms.Panel Border;

        /// <summary>
        /// Helps the num up down controls to set the caret correctly after internal value change.
        /// </summary>
        private int Caret = 0;

        /// <summary>
        /// The constructor, builds the interface and setup localizing.
        /// </summary>
        public StyleControl()
        {
            this.colorPickerColor = new GuruComponents.Netrix.UserInterface.ColorPicker.ColorPickerUserControl(true);
            this.colorPickerBackgroundColor = new GuruComponents.Netrix.UserInterface.ColorPicker.ColorPickerUserControl(true);
            this.colorPickerBorderBottom = new GuruComponents.Netrix.UserInterface.ColorPicker.ColorPickerUserControl(true);
            this.colorPickerBorderRight = new GuruComponents.Netrix.UserInterface.ColorPicker.ColorPickerUserControl(true);
            this.colorPickerBorderLeft = new GuruComponents.Netrix.UserInterface.ColorPicker.ColorPickerUserControl(true);
            this.colorPickerBorderTop = new GuruComponents.Netrix.UserInterface.ColorPicker.ColorPickerUserControl(true);
            this.colorPickerColumn = new GuruComponents.Netrix.UserInterface.ColorPicker.ColorPickerUserControl(true);
            this.colorPickerScrollTrack = new GuruComponents.Netrix.UserInterface.ColorPicker.ColorPickerUserControl(true);
            this.colorPicker3Dlight = new GuruComponents.Netrix.UserInterface.ColorPicker.ColorPickerUserControl(true);
            this.colorPickerScrollHighlight = new GuruComponents.Netrix.UserInterface.ColorPicker.ColorPickerUserControl(true);
            this.colorPickerScrollDarkShadow = new GuruComponents.Netrix.UserInterface.ColorPicker.ColorPickerUserControl(true);
            this.colorPickerScrollShadow = new GuruComponents.Netrix.UserInterface.ColorPicker.ColorPickerUserControl(true);
            this.colorPickerScrollBase = new GuruComponents.Netrix.UserInterface.ColorPicker.ColorPickerUserControl(true);
            this.colorPickerScrollArrow = new GuruComponents.Netrix.UserInterface.ColorPicker.ColorPickerUserControl(true);
            this.colorPickerScrollFace = new GuruComponents.Netrix.UserInterface.ColorPicker.ColorPickerUserControl(true);
            this.colorPickerBorderColorAll = new GuruComponents.Netrix.UserInterface.ColorPicker.ColorPickerUserControl(true);
            this.fontPickerFont = new GuruComponents.Netrix.UserInterface.FontPicker.FontPickerUserControl(true);
            
            InitializeComponent();
            SetColorControls();
            SetUp();

            parser = new CssParser();
            parser.SelectorReady += new SelectorEventHandler(this.SelectorHandler);
            this.tabControlEditor.SelectedIndex = 0;
        }

        internal StyleControl(bool @internal)
        {

            this.colorPickerColor = new GuruComponents.Netrix.UserInterface.ColorPicker.ColorPickerUserControl(true);
            this.colorPickerBackgroundColor = new GuruComponents.Netrix.UserInterface.ColorPicker.ColorPickerUserControl(true);
            this.colorPickerBorderBottom = new GuruComponents.Netrix.UserInterface.ColorPicker.ColorPickerUserControl(true);
            this.colorPickerBorderRight = new GuruComponents.Netrix.UserInterface.ColorPicker.ColorPickerUserControl(true);
            this.colorPickerBorderLeft = new GuruComponents.Netrix.UserInterface.ColorPicker.ColorPickerUserControl(true);
            this.colorPickerBorderTop = new GuruComponents.Netrix.UserInterface.ColorPicker.ColorPickerUserControl(true);
            this.colorPickerColumn = new GuruComponents.Netrix.UserInterface.ColorPicker.ColorPickerUserControl(true);
            this.colorPickerScrollTrack = new GuruComponents.Netrix.UserInterface.ColorPicker.ColorPickerUserControl(true);
            this.colorPicker3Dlight = new GuruComponents.Netrix.UserInterface.ColorPicker.ColorPickerUserControl(true);
            this.colorPickerScrollHighlight = new GuruComponents.Netrix.UserInterface.ColorPicker.ColorPickerUserControl(true);
            this.colorPickerScrollDarkShadow = new GuruComponents.Netrix.UserInterface.ColorPicker.ColorPickerUserControl(true);
            this.colorPickerScrollShadow = new GuruComponents.Netrix.UserInterface.ColorPicker.ColorPickerUserControl(true);
            this.colorPickerScrollBase = new GuruComponents.Netrix.UserInterface.ColorPicker.ColorPickerUserControl(true);
            this.colorPickerScrollArrow = new GuruComponents.Netrix.UserInterface.ColorPicker.ColorPickerUserControl(true);
            this.colorPickerScrollFace = new GuruComponents.Netrix.UserInterface.ColorPicker.ColorPickerUserControl(true);
            this.colorPickerBorderColorAll = new GuruComponents.Netrix.UserInterface.ColorPicker.ColorPickerUserControl(true);
            this.fontPickerFont = new GuruComponents.Netrix.UserInterface.FontPicker.FontPickerUserControl(true);

            InitializeComponent();
            SetColorControls();
            SetUp();

            parser = new CssParser();
            parser.SelectorReady += new SelectorEventHandler(this.SelectorHandler);
            this.tabControlEditor.SelectedIndex = 0;
        }

        private void SetColorControls()
        {
            // 
            // fontPickerFont
            // 
            this.fontPickerFont.AllowDrop = true;
            this.fontPickerFont.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));
            this.fontPickerFont.Border = System.Windows.Forms.BorderStyle.None;
            this.fontPickerFont.BoxCaption = "Font Selection";
            this.fontPickerFont.ForeColor = System.Drawing.SystemColors.ControlText;
            this.fontPickerFont.Location = new System.Drawing.Point(121, 16);
            this.fontPickerFont.Name = "fontPickerFont";
            this.fontPickerFont.PopulateStringList = "";
            this.fontPickerFont.SampleString = " - NET.RIX";
            this.fontPickerFont.Size = new System.Drawing.Size(289, 125);
            this.fontPickerFont.TabIndex = 0;
            this.fontPickerFont.TabStop = false;
            this.fontPickerFont.Tag = "font-family";
            this.fontPickerFont.Title = "Fonts";
            this.fontPickerFont.ContentChanged += new GuruComponents.Netrix.UserInterface.FontPicker.ContentChangedEventHandler(this.fontPickerFont_ContentChanged);
            // 
            // colorPickerBorderColorAll
            // 
            this.colorPickerBorderColorAll.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));
            this.colorPickerBorderColorAll.BackColor = System.Drawing.SystemColors.Control;
            this.colorPickerBorderColorAll.CurrentColor = System.Drawing.Color.Empty;
            this.colorPickerBorderColorAll.Location = new System.Drawing.Point(16, 24);
            this.colorPickerBorderColorAll.Name = "colorPickerBorderColorAll";
            this.colorPickerBorderColorAll.Size = new System.Drawing.Size(208, 23);
            this.colorPickerBorderColorAll.TabIndex = 0;
            this.colorPickerBorderColorAll.Tag = "border-color";            // 
            // colorPickerColor
            // 
            this.colorPickerColor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));
            this.colorPickerColor.BackColor = System.Drawing.SystemColors.Control;
            this.colorPickerColor.CurrentColor = System.Drawing.Color.Empty;
            this.colorPickerColor.Location = new System.Drawing.Point(8, 40);
            this.colorPickerColor.Name = "colorPickerColor";
            this.colorPickerColor.Size = new System.Drawing.Size(184, 23);
            this.colorPickerColor.TabIndex = 0;
            this.colorPickerColor.Tag = "color";
            this.colorPickerColor.ColorChanged += new GuruComponents.Netrix.UserInterface.ColorPicker.ColorChangedEventHandler(this.genesisColor_ColorChanged);
            // 
            // colorPickerBackgroundColor
            // 
            this.colorPickerBackgroundColor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));
            this.colorPickerBackgroundColor.BackColor = System.Drawing.SystemColors.Control;
            this.colorPickerBackgroundColor.CurrentColor = System.Drawing.Color.Empty;
            this.colorPickerBackgroundColor.Location = new System.Drawing.Point(112, 24);
            this.colorPickerBackgroundColor.Name = "colorPickerBackgroundColor";
            this.colorPickerBackgroundColor.Size = new System.Drawing.Size(296, 23);
            this.colorPickerBackgroundColor.TabIndex = 0;
            this.colorPickerBackgroundColor.Tag = "background-color";
            this.colorPickerBackgroundColor.ColorChanged += new GuruComponents.Netrix.UserInterface.ColorPicker.ColorChangedEventHandler(this.genesisColor_ColorChanged);
            // 
            // colorPickerBorderBottom
            // 
            this.colorPickerBorderBottom.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));
            this.colorPickerBorderBottom.BackColor = System.Drawing.SystemColors.Control;
            this.colorPickerBorderBottom.CurrentColor = System.Drawing.Color.Empty;
            this.colorPickerBorderBottom.Location = new System.Drawing.Point(16, 24);
            this.colorPickerBorderBottom.Name = "colorPickerBorderBottom";
            this.colorPickerBorderBottom.Size = new System.Drawing.Size(208, 23);
            this.colorPickerBorderBottom.TabIndex = 0;
            this.colorPickerBorderBottom.Tag = "border-bottom-color";
            this.colorPickerBorderBottom.ColorChanged += new GuruComponents.Netrix.UserInterface.ColorPicker.ColorChangedEventHandler(this.genesisColor_ColorChanged);
            // 
            // colorPickerBorderRight
            // 
            this.colorPickerBorderRight.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));
            this.colorPickerBorderRight.BackColor = System.Drawing.SystemColors.Control;
            this.colorPickerBorderRight.CurrentColor = System.Drawing.Color.Empty;
            this.colorPickerBorderRight.Location = new System.Drawing.Point(16, 24);
            this.colorPickerBorderRight.Name = "colorPickerBorderRight";
            this.colorPickerBorderRight.Size = new System.Drawing.Size(208, 23);
            this.colorPickerBorderRight.TabIndex = 0;
            this.colorPickerBorderRight.Tag = "border-right-color";
            this.colorPickerBorderRight.ColorChanged += new GuruComponents.Netrix.UserInterface.ColorPicker.ColorChangedEventHandler(this.genesisColor_ColorChanged);
            // 
            // colorPickerBorderLeft
            // 
            this.colorPickerBorderLeft.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));
            this.colorPickerBorderLeft.BackColor = System.Drawing.SystemColors.Control;
            this.colorPickerBorderLeft.CurrentColor = System.Drawing.Color.Empty;
            this.colorPickerBorderLeft.Location = new System.Drawing.Point(16, 24);
            this.colorPickerBorderLeft.Name = "colorPickerBorderLeft";
            this.colorPickerBorderLeft.Size = new System.Drawing.Size(208, 23);
            this.colorPickerBorderLeft.TabIndex = 0;
            this.colorPickerBorderLeft.Tag = "border-left-color";
            this.colorPickerBorderLeft.ColorChanged += new GuruComponents.Netrix.UserInterface.ColorPicker.ColorChangedEventHandler(this.genesisColor_ColorChanged);
            // 
            // colorPickerBorderTop
            // 
            this.colorPickerBorderTop.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));
            this.colorPickerBorderTop.BackColor = System.Drawing.SystemColors.Control;
            this.colorPickerBorderTop.CurrentColor = System.Drawing.Color.Empty;
            this.colorPickerBorderTop.Location = new System.Drawing.Point(16, 24);
            this.colorPickerBorderTop.Name = "colorPickerBorderTop";
            this.colorPickerBorderTop.Size = new System.Drawing.Size(216, 23);
            this.colorPickerBorderTop.TabIndex = 0;
            this.colorPickerBorderTop.Tag = "border-top-color";
            this.colorPickerBorderTop.ColorChanged += new GuruComponents.Netrix.UserInterface.ColorPicker.ColorChangedEventHandler(this.genesisColor_ColorChanged);
            // 
            // colorPickerColumn
            // 
            this.colorPickerColumn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));
            this.colorPickerColumn.BackColor = System.Drawing.SystemColors.Control;
            this.colorPickerColumn.CurrentColor = System.Drawing.Color.Empty;
            this.colorPickerColumn.Location = new System.Drawing.Point(140, 48);
            this.colorPickerColumn.Name = "colorPickerColumn";
            this.colorPickerColumn.Size = new System.Drawing.Size(128, 23);
            this.colorPickerColumn.TabIndex = 1;
            this.colorPickerColumn.Tag = "column-color";
            this.colorPickerColumn.ColorChanged += new GuruComponents.Netrix.UserInterface.ColorPicker.ColorChangedEventHandler(this.genesisColor_ColorChanged);
            // 
            // colorPickerScrollTrack
            // 
            this.colorPickerScrollTrack.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));
            this.colorPickerScrollTrack.BackColor = System.Drawing.SystemColors.Control;
            this.colorPickerScrollTrack.CurrentColor = System.Drawing.Color.Empty;
            this.colorPickerScrollTrack.Location = new System.Drawing.Point(114, 198);
            this.colorPickerScrollTrack.Name = "colorPickerScrollTrack";
            this.colorPickerScrollTrack.Size = new System.Drawing.Size(120, 23);
            this.colorPickerScrollTrack.TabIndex = 7;
            this.colorPickerScrollTrack.Tag = "scrollbar-track-color";
            this.colorPickerScrollTrack.ColorChanged += new GuruComponents.Netrix.UserInterface.ColorPicker.ColorChangedEventHandler(this.genesisColor_ColorChanged);
            // 
            // colorPicker3Dlight
            // 
            this.colorPicker3Dlight.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));
            this.colorPicker3Dlight.BackColor = System.Drawing.SystemColors.Control;
            this.colorPicker3Dlight.CurrentColor = System.Drawing.Color.Empty;
            this.colorPicker3Dlight.Location = new System.Drawing.Point(114, 174);
            this.colorPicker3Dlight.Name = "colorPicker3Dlight";
            this.colorPicker3Dlight.Size = new System.Drawing.Size(120, 23);
            this.colorPicker3Dlight.TabIndex = 6;
            this.colorPicker3Dlight.Tag = "scrollbar-3dlight-color";
            this.colorPicker3Dlight.ColorChanged += new GuruComponents.Netrix.UserInterface.ColorPicker.ColorChangedEventHandler(this.genesisColor_ColorChanged);
            // 
            // colorPickerScrollHighlight
            // 
            this.colorPickerScrollHighlight.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));
            this.colorPickerScrollHighlight.BackColor = System.Drawing.SystemColors.Control;
            this.colorPickerScrollHighlight.CurrentColor = System.Drawing.Color.Empty;
            this.colorPickerScrollHighlight.Location = new System.Drawing.Point(114, 151);
            this.colorPickerScrollHighlight.Name = "colorPickerScrollHighlight";
            this.colorPickerScrollHighlight.Size = new System.Drawing.Size(120, 23);
            this.colorPickerScrollHighlight.TabIndex = 5;
            this.colorPickerScrollHighlight.Tag = "scrollbar-highlight-color";
            this.colorPickerScrollHighlight.ColorChanged += new GuruComponents.Netrix.UserInterface.ColorPicker.ColorChangedEventHandler(this.genesisColor_ColorChanged);
            // 
            // colorPickerScrollDarkShadow
            // 
            this.colorPickerScrollDarkShadow.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));
            this.colorPickerScrollDarkShadow.BackColor = System.Drawing.SystemColors.Control;
            this.colorPickerScrollDarkShadow.CurrentColor = System.Drawing.Color.Empty;
            this.colorPickerScrollDarkShadow.Location = new System.Drawing.Point(114, 128);
            this.colorPickerScrollDarkShadow.Name = "colorPickerScrollDarkShadow";
            this.colorPickerScrollDarkShadow.Size = new System.Drawing.Size(120, 23);
            this.colorPickerScrollDarkShadow.TabIndex = 4;
            this.colorPickerScrollDarkShadow.Tag = "scrollbar-darkshadow-color";
            this.colorPickerScrollDarkShadow.ColorChanged += new GuruComponents.Netrix.UserInterface.ColorPicker.ColorChangedEventHandler(this.genesisColor_ColorChanged);
            // 
            // colorPickerScrollShadow
            // 
            this.colorPickerScrollShadow.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));
            this.colorPickerScrollShadow.BackColor = System.Drawing.SystemColors.Control;
            this.colorPickerScrollShadow.CurrentColor = System.Drawing.Color.Empty;
            this.colorPickerScrollShadow.Location = new System.Drawing.Point(114, 105);
            this.colorPickerScrollShadow.Name = "colorPickerScrollShadow";
            this.colorPickerScrollShadow.Size = new System.Drawing.Size(120, 23);
            this.colorPickerScrollShadow.TabIndex = 3;
            this.colorPickerScrollShadow.Tag = "scrollbar-shadow-color";
            this.colorPickerScrollShadow.ColorChanged += new GuruComponents.Netrix.UserInterface.ColorPicker.ColorChangedEventHandler(this.genesisColor_ColorChanged);
            // 
            // colorPickerScrollBase
            // 
            this.colorPickerScrollBase.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));
            this.colorPickerScrollBase.BackColor = System.Drawing.SystemColors.Control;
            this.colorPickerScrollBase.CurrentColor = System.Drawing.Color.Empty;
            this.colorPickerScrollBase.Location = new System.Drawing.Point(114, 81);
            this.colorPickerScrollBase.Name = "colorPickerScrollBase";
            this.colorPickerScrollBase.Size = new System.Drawing.Size(120, 23);
            this.colorPickerScrollBase.TabIndex = 2;
            this.colorPickerScrollBase.Tag = "scrollbar-base-color";
            this.colorPickerScrollBase.ColorChanged += new GuruComponents.Netrix.UserInterface.ColorPicker.ColorChangedEventHandler(this.genesisColor_ColorChanged);
            // 
            // colorPickerScrollArrow
            // 
            this.colorPickerScrollArrow.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));
            this.colorPickerScrollArrow.BackColor = System.Drawing.SystemColors.Control;
            this.colorPickerScrollArrow.CurrentColor = System.Drawing.Color.Empty;
            this.colorPickerScrollArrow.Location = new System.Drawing.Point(114, 57);
            this.colorPickerScrollArrow.Name = "colorPickerScrollArrow";
            this.colorPickerScrollArrow.Size = new System.Drawing.Size(120, 23);
            this.colorPickerScrollArrow.TabIndex = 1;
            this.colorPickerScrollArrow.Tag = "scrollbar-arrow-color";
            this.colorPickerScrollArrow.ColorChanged += new GuruComponents.Netrix.UserInterface.ColorPicker.ColorChangedEventHandler(this.genesisColor_ColorChanged);
            // 
            // colorPickerScrollFace
            // 
            this.colorPickerScrollFace.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));
            this.colorPickerScrollFace.BackColor = System.Drawing.SystemColors.Control;
            this.colorPickerScrollFace.CurrentColor = System.Drawing.Color.Empty;
            this.colorPickerScrollFace.Location = new System.Drawing.Point(114, 33);
            this.colorPickerScrollFace.Name = "colorPickerScrollFace";
            this.colorPickerScrollFace.Size = new System.Drawing.Size(120, 23);
            this.colorPickerScrollFace.TabIndex = 0;
            this.colorPickerScrollFace.Tag = "scrollbar-face-color";
            this.colorPickerScrollFace.ColorChanged += new GuruComponents.Netrix.UserInterface.ColorPicker.ColorChangedEventHandler(this.genesisColor_ColorChanged);


            this.groupBox2.Controls.Add(this.colorPickerColor);
            this.groupBox1.Controls.Add(this.fontPickerFont);
            this.groupBox4.Controls.Add(this.colorPickerBackgroundColor);
            this.groupBox25.Controls.Add(this.colorPickerBorderColorAll);
            this.groupBox24.Controls.Add(this.colorPickerBorderLeft);
            this.groupBox17.Controls.Add(this.colorPickerBorderRight);
            this.groupBox13.Controls.Add(this.colorPickerBorderTop);
            this.groupBox12.Controls.Add(this.colorPickerBorderBottom);
            this.groupBox23.Controls.Add(this.colorPickerColumn);
            this.groupBox15.Controls.Add(this.colorPickerScrollTrack);
            this.groupBox15.Controls.Add(this.colorPicker3Dlight);
            this.groupBox15.Controls.Add(this.colorPickerScrollHighlight);
            this.groupBox15.Controls.Add(this.colorPickerScrollDarkShadow);
            this.groupBox15.Controls.Add(this.colorPickerScrollShadow);
            this.groupBox15.Controls.Add(this.colorPickerScrollBase);
            this.groupBox15.Controls.Add(this.colorPickerScrollArrow);
            this.groupBox15.Controls.Add(this.colorPickerScrollFace);

        }

        /// <summary>
        /// Controls the custom color tab of all color picker controls.
        /// </summary>
        [Browsable(false)]
        public List<Color> CustomColors
        {
            set
            {
                colorPickerColor.ResetControl(value);
                colorPickerBackgroundColor.ResetControl(value);
                colorPickerBorderBottom.ResetControl(value);
                colorPickerBorderRight.ResetControl(value);
                colorPickerBorderLeft.ResetControl(value);
                colorPickerBorderTop.ResetControl(value);
                colorPickerColumn.ResetControl(value);
                colorPickerScrollTrack.ResetControl(value);
                colorPicker3Dlight.ResetControl(value);
                colorPickerScrollHighlight.ResetControl(value);
                colorPickerScrollDarkShadow.ResetControl(value);
                colorPickerScrollShadow.ResetControl(value);
                colorPickerScrollBase.ResetControl(value);
                colorPickerScrollArrow.ResetControl(value);
                colorPickerScrollFace.ResetControl(value);
            }
        }

        /// <summary>
        /// Sets the visiblity of the specified TAB.
        /// </summary>
        /// <remarks>
        /// By default all TabPages are visible. The collection of names can be retrieved using the
        /// <see cref="GetTabNames"/> method.
        /// </remarks>
        /// <exception cref="System.ArgumentException">Fired if the given TabName does not exist within the collection.</exception>
        /// <param name="tabName">The Tab which should change their visiblity.</param>
        public void HideTab(string tabName)
        {
            this.tabControlEditor.SuspendLayout();
            foreach (TabPage page in this.tabControlEditor.Controls)
            {
                // add all pages, except the one we want to hide
                if (tabName.Equals(page.Name))
                {
                    this.tabControlEditor.Controls.Remove(page);
                }
            }
            // if position page is hidden, disable the linklabel on table page
            linkLabel1.Enabled = tabName.Equals("tabPagePosition");
            this.tabControlEditor.ResumeLayout(true);
        }

        /// <summary>
        /// Selects the tabpage which has the given index.
        /// </summary>
        /// <param name="index">The index of the TabPage to select. Zero based.</param>
        public void SelectTab(int index)
        {
            this.tabControlEditor.SelectedIndex = index;
        }

        /// <summary>
        /// Returns the list of TabNames to control the Tabs individually.
        /// </summary>
        /// <remarks>
        /// The list returns by default the following names:
        /// <list type="bullet">
        /// <item>tabPageFonts</item>
        /// <item>tabPageLayout</item>
        /// <item>tabPageBackground</item>
        /// <item>tabPagePosition</item>
        /// <item>tabPageBorders</item>
        /// <item>tabPageLists</item>
        /// <item>tabPageTables</item>
        /// <item>tabPageMisc</item>
        /// </list>
        /// <seealso cref="HideTab"/>
        /// </remarks>
        /// <returns></returns>
        public IList GetTabNames()
        {
            ArrayList tabNames = new ArrayList();
            foreach (TabPage tp in this.tabControlEditor.TabPages)
            {
                tabNames.Add(tp.Name);
            }
            return tabNames;
        }

        /// <summary>
        /// Sets all base values, language resources and clears the state of comboboxes. 
        /// </summary>
        /// <remarks>
        /// Should be called after each change of the culture to update the UI.
        /// This method calls implicitily <see cref="ResetFields"/>. After that the current style string must 
        /// be set again to fill the fields with the proper values.
        /// </remarks>
        public void SetUp()
        {
            this.tabControlEditor.Controls.Clear();
            this.tabControlEditor.Controls.Add(this.tabPageFonts);
            this.tabControlEditor.Controls.Add(this.tabPageLayout);
            this.tabControlEditor.Controls.Add(this.tabPageBackground);
            this.tabControlEditor.Controls.Add(this.tabPagePosition);
            this.tabControlEditor.Controls.Add(this.tabPageBorders);
            this.tabControlEditor.Controls.Add(this.tabPageLists);
            this.tabControlEditor.Controls.Add(this.tabPageTables);
            this.tabControlEditor.Controls.Add(this.tabPageMisc);

            object[] styleBackgroundAttachment = new object[]
            {
                new UnitPair("",        GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.NoValue")),
                new UnitPair("scroll",  GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.Scroll")),
                new UnitPair("fixed",   GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.Fixed")),
            };
            object[] styleBorderStyle = new object[] 
            {
                new UnitPair("",        GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.NoValue")),
                new UnitPair("none",    GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.None")),
                new UnitPair("solid",   GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.Solid")),
                new UnitPair("double",  GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.Double")),
                new UnitPair("dotted",  GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.Dotted")),
                new UnitPair("dashed",  GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.Dashed")),
                new UnitPair("outset",  GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.OutSet")),
                new UnitPair("groove",  GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.Groove")),
                new UnitPair("inset",   GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.InSet")),
                new UnitPair("ridge",   GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.Ridge")),
                new UnitPair("hidden",  GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.Hidden")),
                new UnitPair("inherit", GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.Inherit"))
            };
            object[] styleUnit = new object[]
            {
                new UnitPair("",        GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.NoValue")),
                new UnitPair("px",      GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.Pixel")),
                new UnitPair("pt",      GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.Point")),
                new UnitPair("%",       GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.Percentage")),
                new UnitPair("pc",      GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.Pica")),
                new UnitPair("mm",      GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.Millimeter")),
                new UnitPair("cm",      GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.Centimeter")),
                new UnitPair("em",      GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.m-Size")),
                new UnitPair("ex",      GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.x-Height"))
            };
            object[] styleFontWeight = new object[] 
            {
                new UnitPair("",        GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.NoValue")),
                new UnitPair("normal",  GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.Normal")),
                new UnitPair("bold",    GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.Bold")),
                new UnitPair("bolder",  GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.Bolder")),
                new UnitPair("lighter", GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.Lighter")),
                new UnitPair("100", "100"),
                new UnitPair("200", "200"),
                new UnitPair("300", "300"),
                new UnitPair("400", "400"),
                new UnitPair("500", "500"),
                new UnitPair("600", "600"),
                new UnitPair("700", "700"),
                new UnitPair("800", "800"),
                new UnitPair("900", "900"),
                new UnitPair("inherit", GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.Inherit"))
            };
            object[] styleTextTransform = new object[] 
            {
                new UnitPair("",           GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.NoValue")),
                new UnitPair("none",       GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.None")),
                new UnitPair("capitalize", GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.CapitalLetter")),
                new UnitPair("uppercase",  GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.UpperCase")),
                new UnitPair("lowercase",  GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.LowerCase"))
            };
            object[] stylePosition = new object[] 
            {
                new UnitPair("none",    GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.None")),
                new UnitPair("top",     GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.Top")),
                new UnitPair("center",  GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.Center")),
                new UnitPair("bottom",  GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.Bottom")),
                new UnitPair("absolute",GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.Absolute"))
            };
            object[] styleDisplay = new object[] 
            {
                new UnitPair("",        GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.NoValue")),
                new UnitPair("block",   GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.AsBlock")),
                new UnitPair("inline",  GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.InlineText")),
                new UnitPair("list-item", GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.ListItem")),
                new UnitPair("none",    GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.None"))
            };
            object[] styleVerticalAlign = new Object[]
            {
                new UnitPair("",            GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.NoValue")),
                new UnitPair("top",         GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.Top")),
                new UnitPair("middle",      GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.Middle")),
                new UnitPair("bottom",      GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.Bottom")),
                new UnitPair("baseline",    GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.Baseline")),
                new UnitPair("sub",         GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.Sub")),
                new UnitPair("super",       GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.Super")),
                new UnitPair("text-top",    GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.TopOfText")),
                new UnitPair("text-bottom", GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.BottomOfText"))
            };
            object[] styleTextDecoration = new object[]
            {
                new UnitPair("",            GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.NoValue")),
                new UnitPair("none",        GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.None")),
                new UnitPair("underline",   GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.Underline")),
                new UnitPair("overline",    GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.StrikeOver")),
                new UnitPair("line-through",GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.StrikeOut")),
                new UnitPair("blink",       GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.BlinkText"))
            };

            object[] styleLineBreak = new object[]
            {
                new UnitPair("",            GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.NoValue")),
                new UnitPair("normal",      GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.Standard")),
                new UnitPair("pre",         GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.Pre")),
                new UnitPair("nowrap",      GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.NoWrap")),
            };
            object[] styleOverFlow = new object[]
            {
                new UnitPair("",            GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.NoValue")),
                new UnitPair("visible",     GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.Visible")),
                new UnitPair("hidden",      GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.NotVisible")),
                new UnitPair("scroll",      GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.ScrollableArea")),
                new UnitPair("auto",        GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.Automatic"))
            };
            object[] styleCaptionSide = new object[]
            {
                new UnitPair("",            GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.NoValue")),
                new UnitPair("top",         GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.Top")),
                new UnitPair("topleft",     GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.TopAndLeft")),
                new UnitPair("topright",    GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.TopAndRight")),
                new UnitPair("bottom",      GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.Bottom")),
                new UnitPair("bottomleft",  GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.BottomAndLeft")),
                new UnitPair("bottomright", GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.BottomAndRight"))
            };
            object[] styleBackground = new object[]
            {
                new UnitPair("",            GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.NoValue")),
                new UnitPair("top",         GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.Top")),
                new UnitPair("middle",      GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.Middle")),
                new UnitPair("left",        GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.Left")),
                new UnitPair("right",       GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.Right")),
                new UnitPair("center",      GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.Center")),
                new UnitPair("bottom",      GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.Bottom")),
                new UnitPair("inherit",     GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.Inherit"))
            };
            object[] styleBackgroundRepeat = new object[]
            {
                new UnitPair("",            GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.NoValue")),
                new UnitPair("repeat",      GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.Repeat")),
                new UnitPair("repeat-x",    GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.RepeatRows")),
                new UnitPair("repeat-y",    GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.RepeatColumns")),
                new UnitPair("no-repeat",   GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.NoRepeat"))
            };
            object[] styleAbsoluteSize = new object[]
            {
                new UnitPair("",            GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.NoValue")),
                new UnitPair("normal",      GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.Standard")),
                new UnitPair("xx-small",    GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.ExtremSmall")),
                new UnitPair("x-small",     GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.VerySmall")),
                new UnitPair("small",       GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.small")),
                new UnitPair("medium",      GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.Medium")),
                new UnitPair("large",       GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.Large")),
                new UnitPair("x-large",     GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.ExtraLarge")),
                new UnitPair("xx-large",    GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.XXL")),
                new UnitPair("inherit",     GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.Inherit"))
            };
            object[] styleListStylePosition = new object[]
            {
                new UnitPair("",            GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.NoValue")),
                new UnitPair("inside",      GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.Inside List")),
                new UnitPair("outside",     GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.Outside List")),
                new UnitPair("none",        GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.None"))
            };
            object[] styleListStyleType = new object[]
            {
                new UnitPair("",            GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.NoValue")),
                new UnitPair("square",      GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.Square")),
                new UnitPair("disc",        GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.Disc")),
                new UnitPair("circle",      GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.Circle")),
                new UnitPair("lower-roman", GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.LowerRoman")),
                new UnitPair("upper-roman", GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.UpperRoman")),
                new UnitPair("lower-alpha", GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.LowerAlpha")),
                new UnitPair("upper-alpha", GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.UpperAlpha")),
                new UnitPair("decimal-leading-zero", 
                GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.DecimalZero")),
                new UnitPair("decimal",     GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.Decimal")),
                new UnitPair("inherit",     GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.Inherit"))
            };
            object[] styleBorderWidth = new object[]
            {
                new UnitPair("",            GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.NoValue")),
                new UnitPair("thin",        GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.Thin")),
                new UnitPair("medium",      GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.Medium")),
                new UnitPair("thick",       GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.Thick"))
            };
            object[] styleCursor = new object[]
            {
                new UnitPair("",            GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.NoValue")),
                new UnitPair("default",     GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.Standard")),
                new UnitPair("auto",        GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.Auto")), 
                new UnitPair("crosshair",   GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.Cross")),
                new UnitPair("pointer",     GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.Hand")),
                new UnitPair("hand",        GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.HandIE5Only")),
                new UnitPair("move",        GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.MoveObject")),
                new UnitPair("e-resize",    GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.ResizeRightBorder")),     
                new UnitPair("ne-resize",   GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.ResizeUpperRightCorner")), 
                new UnitPair("nw-resize",   GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.ResizeUpperLeftCorner")), 
                new UnitPair("n-resize",    GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.ResizeTopBorder")), 
                new UnitPair("se-resize",   GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.ResizeLowerRightCorner")),
                new UnitPair("sw-resize",   GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.ResizeLowerLeftCorner")),
                new UnitPair("s-resize",    GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.ResizeBottomBorder")),
                new UnitPair("w-resize",    GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.ResizeLeftBorder")),
                new UnitPair("text",        GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.InsideTextbox")),
                new UnitPair("wait",        GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.Wait(Hourglass)")),
                new UnitPair("help",        GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.Combos.Help"))
            };

            // Clear lists to reset overall state after recall
            this.comboBoxListStylePosition.Items.Clear();
            this.comboBoxBorderLeftStyle.Items.Clear();
            this.comboBoxBorderRightStyle.Items.Clear();
            this.comboBoxBorderTopStyle.Items.Clear();
            this.comboBoxBorderBottomStyle.Items.Clear();
            this.comboBoxBorderStyleAll.Items.Clear();

            this.comboBoxDisplay.Items.Clear();
            this.comboBoxStyleOverFlow.Items.Clear();
            this.comboBoxStyleVerticalAlignment.Items.Clear();
            this.comboBoxBackgroundRepeat.Items.Clear();
            this.comboBoxCaptionSide.Items.Clear();
            this.comboBoxBackgroundAttachment.Items.Clear();

            this.comboBoxCommonRuleWidth.Items.Clear();
            this.comboBoxColumnRuleStyle.Items.Clear();
            this.comboBoxColumnGap.Items.Clear();

            this.comboBoxStyleLineBreak.Items.Clear();

            this.comboBoxTextDecoration.Items.Clear();
            this.comboBoxFontWeight.Items.Clear();

            this.comboBoxBackgroundPositionX.Items.Clear();
            this.comboBoxBackgroundPositionY.Items.Clear();
            this.comboBoxBackgroundPosition.Items.Clear();

            this.comboBoxBorderBottomWidthUnit.Items.Clear();
            this.comboBoxBorderLeftWidthUnit.Items.Clear();
            this.comboBoxBorderRightWidthUnit.Items.Clear();
            this.comboBoxBorderTopWidthUnit.Items.Clear();
            this.comboBoxBorderWidthAll.Items.Clear();

            this.comboBoxFontSizeUnit.Items.Clear();
            this.comboBoxHeightUnit.Items.Clear();
            this.comboBoxLeftUnit.Items.Clear();
            this.comboBoxLetterSpacingUnit.Items.Clear();
            this.comboBoxLineHeightUnit.Items.Clear();
            this.comboBoxMarginBottomUnit.Items.Clear();
            this.comboBoxMarginTopUnit.Items.Clear();
            this.comboBoxMarginLeftUnit.Items.Clear();
            this.comboBoxMarginRightUnit.Items.Clear();
            this.comboBoxMarginAll.Items.Clear();

            this.comboBoxPaddingBottomUnit.Items.Clear();
            this.comboBoxPaddingTopUnit.Items.Clear();
            this.comboBoxPaddingLeftUnit.Items.Clear();
            this.comboBoxPaddingRightUnit.Items.Clear();
            this.comboBoxPaddingAll.Items.Clear();

            this.comboBoxTextIdentUnit.Items.Clear();
            this.comboBoxTopUnit.Items.Clear();
            this.comboBoxWidthUnit.Items.Clear();
            this.comboBoxWordSpacingUnit.Items.Clear();
            this.comboBoxTextTransform.Items.Clear();
            this.comboBoxFontSizeAbsolute.Items.Clear();
            this.comboBoxListStylePosition.Items.Clear();
            this.comboBoxListStyleType.Items.Clear();
            this.comboBoxCursor.Items.Clear();
            // Fill lists
            this.comboBoxBorderLeftStyle.Items.AddRange(styleBorderStyle);
            this.comboBoxBorderRightStyle.Items.AddRange(styleBorderStyle);
            this.comboBoxBorderTopStyle.Items.AddRange(styleBorderStyle);
            this.comboBoxBorderBottomStyle.Items.AddRange(styleBorderStyle);
            this.comboBoxBorderStyleAll.Items.AddRange(styleBorderStyle);

            this.comboBoxDisplay.Items.AddRange(styleDisplay);
            this.comboBoxStyleOverFlow.Items.AddRange(styleOverFlow);
            this.comboBoxStyleVerticalAlignment.Items.AddRange(styleVerticalAlign);
            this.comboBoxBackgroundRepeat.Items.AddRange(styleBackgroundRepeat);
            this.comboBoxCaptionSide.Items.AddRange(styleCaptionSide);
            this.comboBoxBackgroundAttachment.Items.AddRange(styleBackgroundAttachment);

            this.comboBoxCommonRuleWidth.Items.AddRange(styleUnit);
            this.comboBoxColumnRuleStyle.Items.AddRange(styleBorderStyle);
            this.comboBoxColumnGap.Items.AddRange(styleUnit);

            this.comboBoxStyleLineBreak.Items.AddRange(styleLineBreak);

            this.comboBoxTextDecoration.Items.AddRange(styleTextDecoration);
            this.comboBoxFontWeight.Items.AddRange(styleFontWeight);

            this.comboBoxBackgroundPositionX.Items.AddRange(styleUnit);
            this.comboBoxBackgroundPositionY.Items.AddRange(styleUnit);
            this.comboBoxBackgroundPosition.Items.AddRange(styleBackground);

            this.comboBoxBorderBottomWidthUnit.Items.AddRange(styleUnit);
            this.comboBoxBorderLeftWidthUnit.Items.AddRange(styleUnit);
            this.comboBoxBorderRightWidthUnit.Items.AddRange(styleUnit);
            this.comboBoxBorderTopWidthUnit.Items.AddRange(styleUnit);
            this.comboBoxBorderWidthAll.Items.AddRange(styleUnit);

            this.comboBoxFontSizeUnit.Items.AddRange(styleUnit);
            this.comboBoxHeightUnit.Items.AddRange(styleUnit);
            this.comboBoxLeftUnit.Items.AddRange(styleUnit);
            this.comboBoxLetterSpacingUnit.Items.AddRange(styleUnit);
            this.comboBoxLineHeightUnit.Items.AddRange(styleUnit);
            this.comboBoxMarginBottomUnit.Items.AddRange(styleUnit);
            this.comboBoxMarginTopUnit.Items.AddRange(styleUnit);
            this.comboBoxMarginLeftUnit.Items.AddRange(styleUnit);
            this.comboBoxMarginRightUnit.Items.AddRange(styleUnit);
            this.comboBoxMarginAll.Items.AddRange(styleUnit);

            this.comboBoxPaddingBottomUnit.Items.AddRange(styleUnit);
            this.comboBoxPaddingTopUnit.Items.AddRange(styleUnit);
            this.comboBoxPaddingLeftUnit.Items.AddRange(styleUnit);
            this.comboBoxPaddingRightUnit.Items.AddRange(styleUnit);
            this.comboBoxPaddingAll.Items.AddRange(styleUnit);

            this.comboBoxTextIdentUnit.Items.AddRange(styleUnit);
            this.comboBoxTopUnit.Items.AddRange(styleUnit);
            this.comboBoxWidthUnit.Items.AddRange(styleUnit);
            this.comboBoxWordSpacingUnit.Items.AddRange(styleUnit);
            this.comboBoxTextTransform.Items.AddRange(styleTextTransform);
            this.comboBoxFontSizeAbsolute.Items.AddRange(styleAbsoluteSize);
            this.comboBoxListStylePosition.Items.AddRange(styleListStylePosition);
            this.comboBoxListStyleType.Items.AddRange(styleListStyleType);
            this.comboBoxCursor.Items.AddRange(styleCursor);
            //
            this.textBoxBackgroundImage.Clear();
            this.textBoxListStyleImage.Clear();
            //
            this.radioButtonDirectionRemove.Checked = true;
            this.radioButtonTextAlignDisable.Checked = true;
            this.radioButtonPositionRemove.Checked = true;
            //
            this.checkBoxFontStyleItalic.Checked = false;
            this.checkBoxFontVariantSmallCaps.Checked = false;
            // set number culture to english format
            CultureStyle = new NumberFormatInfo();
            CultureStyle.NumberDecimalDigits = 2;
            CultureStyle.NumberDecimalSeparator = ".";
            CultureStyle.NumberGroupSeparator = "";
            CultureStyle.PercentDecimalDigits = 0;
            CultureStyle.PercentDecimalSeparator = ".";
            CultureStyle.PercentGroupSeparator = "";
            CultureStyle.NegativeSign = "-";
            // let event handler work (disable events to avoid interferences with internal field settings)
            SupressEvents = false;
            // Set resource text's            
            this.groupBox3.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.groupBox3.Text");
            this.label3.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.label3.Text");
            this.label2.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.label2.Text");
            this.checkBoxFontVariantSmallCaps.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.checkBoxFontVariantSmallCaps.Text");
            this.checkBoxFontStyleItalic.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.checkBoxFontStyleItalic.Text");
            this.groupBox2.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.groupBox2.Text");
            this.label36.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.label36.Text");
            this.radioButtonFontSizeAbsolute.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.radioButtonFontSizeAbsolute.Text");
            this.radioButtonFontSizeUnit.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.radioButtonFontSizeUnit.Text");
            this.label1.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.label1.Text");
            this.groupBox1.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.groupBox1.Text");
            this.label50.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.label50.Text");
            this.label49.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.label49.Text");
            this.groupBox5.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.groupBox5.Text");
            this.label52.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.label52.Text");
            this.label9.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.label9.Text"); //"How to position the image?";
            this.label8.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.label8.Text"); //"Vertical:";
            this.label7.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.label7.Text"); //"Horizontal:";
            this.label6.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.label6.Text"); //"Fixing";
            this.label5.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.label5.Text"); //"Tile";
            this.label4.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.label4.Text"); //"Select Image from disk:";
            this.buttonBackgroundImage.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.buttonBackgroundImage.Text"); //"...";
            this.groupBox4.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.groupBox4.Text"); //"Background Color";
            this.label27.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.label27.Text"); //"Background Color:";
            this.checkBox6.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.checkBox6.Text"); //"Transparency";
            this.groupBox18.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.groupBox18.Text"); //"Margin, Padding, and Border Style";
            this.label56.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.label56.Text"); //"Padding";
            this.label55.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.label55.Text"); //"Border Width";
            this.label54.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.label54.Text"); //"Border Style";
            this.label53.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.label53.Text"); //"Border Color";
            this.label17.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.label17.Text"); //"Margin";
            this.groupBox21.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.groupBox21.Text"); //"Text Decoration";
            this.label67.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.label67.Text"); //"Determines how text is displayed in addition to font characteristics.";
            this.groupBox8.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.groupBox8.Text"); //"Direction and Indentation";
            this.label68.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.label68.Text"); //"Determine how text inside a block is aligned";
            this.label29.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.label29.Text"); //"Direction:";
            this.radioButtonDirectionRemove.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.radioButtonDirectionRemove.Text"); //"Remove";
            this.label51.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.label51.Text"); //"Block Indentation:";
            this.radioButtonDirectionRtl.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.radioButtonDirectionRtl.Text"); //"right to left";
            this.radioButtonDirectionLtr.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.radioButtonDirectionLtr.Text"); //"left to right";
            this.groupBox7.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.groupBox7.Text"); //"Line and Character Space";
            this.label66.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.label66.Text"); //"These options determine how floating text is displayed. The result depends on the coosen font.";
            this.label37.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.label37.Text"); //"Words:";
            this.label11.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.label11.Text"); //"Line Height:";
            this.label12.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.label12.Text"); //"Characters:";
            this.groupBox6.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.groupBox6.Text"); //"Alignment";
            this.groupBox19.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.groupBox19.Text"); //"Horizontal Alignment";
            this.radioButtonTextAlignDisable.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.radioButtonTextAlignDisable.Text"); //"Remove Option from Sheet";
            this.radioButtonTextAlignJustify.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.radioButtonTextAlignJustify.Text"); //"Justify";
            this.radioButtonTextAlignCenter.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.radioButtonTextAlignCenter.Text"); //"";
            this.radioButtonTextAlignRight.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.radioButtonTextAlignRight.Text"); //"Right";
            this.label10.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.label10.Text"); //"Center";
            this.radioButtonTextAlignLeft.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.radioButtonTextAlignLeft.Text"); //"Left";
            this.groupBox20.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.groupBox20.Text"); //"Vertical Alignment";
            this.label65.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.label65.Text"); //"Determine how the element is align vertically. This is commonly used in tables and boxes.";
            this.groupBox10.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.groupBox10.Text"); //"Text Floating Options";
            this.groupBox11.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.groupBox11.Text"); //"Caption"
            this.label48.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.label48.Text"); //"Linebreak Handling";
            this.label47.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.label47.Text"); //"Overflow";
            this.label45.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.label45.Text"); //"Display";
            this.groupBox9.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.groupBox9.Text"); //"Position Mode of an Object";
            this.radioButtonPositionRemove.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.radioButtonPositionRemove.Text"); //"Remove option from sheet";
            this.label16.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.label16.Text"); //"From left border:";
            this.label15.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.label15.Text"); //"From top border";
            this.label14.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.label14.Text"); //"Width";
            this.label13.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.label13.Text"); //"Height";
            this.radioButtonPositionAbsolute.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.radioButtonPositionAbsolute.Text"); //"Absolute";
            this.radioButtonPositionRelative.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.radioButtonPositionRelative.Text"); //"Text Floating";
            this.radioButtonPositionFixed.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.radioButtonPositionFixed.Text"); //"Relative non floating";
            this.radioButtonPositionNone.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.radioButtonPositionNone.Text"); //"None";
            this.groupBox14.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.groupBox14.Text"); //"Define List Item Appearance";
            this.label69.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.label69.Text"); //"These options determine the appearance of unordered lists.  The final result depends also from written text, font and spacing characteristics.";
            this.buttonListStyleImage.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.buttonListStyleImage.Text"); //"...";
            this.label22.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.label22.Text"); //"Position:";
            this.label23.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.label23.Text"); //"Select Image from disk:";
            this.radioButtonListBulletImage.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.radioButtonListBulletImage.Text"); //"With user specific bullet (choose image):";
            this.label21.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.label21.Text"); //"Style";
            this.radioButtonListBulletStyle.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.radioButtonListBulletStyle.Text"); //"With standard bullets (choose style and position):";
            this.radioButtonListBulletNone.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.radioButtonListBulletNone.Text"); //"Without bullets";
            this.groupBox23.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.groupBox23.Text"); //"Columns";
            this.label42.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.label42.Text"); //"Rule Width:";
            this.label41.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.label41.Text"); //"Rule Style";
            this.label40.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.label40.Text"); //"Rule Color";
            this.label39.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.label39.Text"); //"Columns";
            this.groupBox16.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.groupBox16.Text"); //"Tables";
            this.label44.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.label44.Text"); //"Row Span";
            this.label43.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.label43.Text"); //"Column Span";
            this.label38.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.label38.Text"); //"Caption Position";
            this.linkLabel1.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.linkLabel1.Text"); //"Click here to set";
            this.label71.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.label71.Text"); //"Height and Width is choosen on Position Tab:";
            this.tabPageFonts.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.tabPageFonts.Text"); //"Font";
            this.tabPageBackground.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.tabPageBackground.Text"); //"Background";
            this.tabPageBorders.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.tabPageBorders.Text"); //"Borders";
            this.tabPageLayout.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.tabPageLayout.Text"); //"Layout";
            this.tabPagePosition.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.tabPagePosition.Text"); //"Position";
            this.tabPageLists.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.tabPageLists.Text"); //"Lists";
            this.tabPageTables.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.tabPageTables.Text"); //"Tables";
            this.tabPageMisc.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.tabPageMisc.Text"); //"Miscellenous";
            this.groupBox15.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.groupBox15.Text"); //"User Interface";

            this.groupBox25.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.groupBox25.Text"); //"All B";
            this.groupBox24.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.groupBox24.Text"); //"L B";
            this.groupBox17.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.groupBox17.Text"); //"R B";
            this.groupBox13.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.groupBox13.Text"); //"T B";
            this.groupBox12.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.groupBox12.Text"); //"B B";

            this.tabPageAll.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.tabPageAll.Text"); //"Font";
            this.tabPageLeft.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.tabPageLeft.Text"); //"Left";
            this.tabPageRight.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.tabPageRight.Text"); //"Right";
            this.tabPageTop.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.tabPageTop.Text"); //"Top";
            this.tabPageBottom.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.tabPageBottom.Text"); //"Bottom";

            this.label70.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.label70.Text"); //"Cursor and Scrollbar options are currently not available for browser other then Internet Explorer.";
            this.label64.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.label64.Text"); //"Track";
            this.label63.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.label63.Text"); //"3D light";
            this.label62.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.label62.Text"); //"Highlight";
            this.label61.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.label61.Text"); //"Darkshadow";
            this.label60.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.label60.Text"); //"Shadow";
            this.label59.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.label59.Text"); //"Base";
            this.label26.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.label26.Text"); //"Face";
            this.label25.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.label25.Text"); //"Colors of Scrollbar elements:";
            this.label24.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.label24.Text"); //"Cursor:";
            this.label58.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.label58.Text"); //"Arrow";
            this.groupBox22.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.groupBox22.Text"); //"Visibility";
            this.radioButtonVisiblityRemove.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.radioButtonVisiblityRemove.Text"); //"Remove option from sheet";
            this.radioButtonVisibilityNone.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.radioButtonVisibilityNone.Text"); //"None";
            this.radioButtonVisibilityHidden.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.radioButtonVisibilityHidden.Text"); //"Hidden";
            this.radioButtonVisibilityVisible.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.radioButtonVisibilityVisible.Text"); //"Visible";
            this.label31.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.label31.Text"); //"If element set as unvisible (Visiblity = Hidden) it\'s necessary to change the behavior with scripting to show it later. If no scripting capabilites used set to 'None' or 'Remove'.";
            this.button1.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.button1.Text"); //"Synchronize left, right, and bottom values with top. ";            
            this.labelColumnGap.Text = GuruComponents.Netrix.UserInterface.ResourceManager.GetString("StyleEditor.labelColumnGap.Text"); //"Column Gap";            
            // Custom Colors and SetUp for ColorPicker
            this.CustomColors = ResourceManager.CustomColors;
            // Reset Fontpicker
            this.fontPickerFont.ResetUI();

            // add images to picturebox, not currently localized
            Assembly thisAssembly = typeof(StyleControl).Assembly;
            this.pictureBox1.Image = Image.FromStream(thisAssembly.GetManifestResourceStream("GuruComponents.Netrix.UserInterface.Resources.Resx.UserInterface.StyleEditor_Position_Left.bmp"));
            this.pictureBox2.Image = Image.FromStream(thisAssembly.GetManifestResourceStream("GuruComponents.Netrix.UserInterface.Resources.Resx.UserInterface.StyleEditor_Position_Center.bmp"));
            this.pictureBox3.Image = Image.FromStream(thisAssembly.GetManifestResourceStream("GuruComponents.Netrix.UserInterface.Resources.Resx.UserInterface.StyleEditor_Position_Right.bmp"));
            this.pictureBox4.Image = Image.FromStream(thisAssembly.GetManifestResourceStream("GuruComponents.Netrix.UserInterface.Resources.Resx.UserInterface.StyleEditor_Position_Justify.bmp"));
            this.pictureBox5.Image = Image.FromStream(thisAssembly.GetManifestResourceStream("GuruComponents.Netrix.UserInterface.Resources.Resx.UserInterface.StyleEditor_Layout_DirectionLtr.bmp"));
            this.pictureBox6.Image = Image.FromStream(thisAssembly.GetManifestResourceStream("GuruComponents.Netrix.UserInterface.Resources.Resx.UserInterface.StyleEditor_Layout_DirectionRtl.bmp"));
            this.pictureBox7.Image = Image.FromStream(thisAssembly.GetManifestResourceStream("GuruComponents.Netrix.UserInterface.Resources.Resx.UserInterface.StyleEditor_Lists_Style.bmp"));
            this.pictureBox8.Image = Image.FromStream(thisAssembly.GetManifestResourceStream("GuruComponents.Netrix.UserInterface.Resources.Resx.UserInterface.StyleEditor_Lists_Position.bmp"));
            this.pictureBox9.Image = Image.FromStream(thisAssembly.GetManifestResourceStream("GuruComponents.Netrix.UserInterface.Resources.Resx.UserInterface.StyleEditor_Misc_Arrows.bmp"));
            this.pictureBox10.Image = Image.FromStream(thisAssembly.GetManifestResourceStream("GuruComponents.Netrix.UserInterface.Resources.Resx.UserInterface.StyleEditor_Lists_Without.bmp"));
            this.pictureBox11.Image = Image.FromStream(thisAssembly.GetManifestResourceStream("GuruComponents.Netrix.UserInterface.Resources.Resx.UserInterface.StyleEditor_Tables_CaptionPos.bmp"));
            this.pictureBox12.Image = Image.FromStream(thisAssembly.GetManifestResourceStream("GuruComponents.Netrix.UserInterface.Resources.Resx.UserInterface.StyleEditor_Tables_ColSpan.bmp"));
            this.pictureBox13.Image = Image.FromStream(thisAssembly.GetManifestResourceStream("GuruComponents.Netrix.UserInterface.Resources.Resx.UserInterface.StyleEditor_Tables_RowSpan.bmp"));
            this.pictureBox14.Image = Image.FromStream(thisAssembly.GetManifestResourceStream("GuruComponents.Netrix.UserInterface.Resources.Resx.UserInterface.StyleEditor_Tables_RuleStyle.bmp"));
            this.pictureBox15.Image = Image.FromStream(thisAssembly.GetManifestResourceStream("GuruComponents.Netrix.UserInterface.Resources.Resx.UserInterface.StyleEditor_PaddingMargin.bmp"));
            
            this.tabControlEditor.ImageList = imageListBorderTabs;
            this.tabPageFonts.ImageIndex = 0;
            // clear fields
            this.ResetFields();
        }


        #region Component Designer generated code

        /// <summary> 
        /// Die verwendeten Ressourcen bereinigen.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        /// <summary> 
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StyleControl));
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBoxTextTransform = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBoxFontWeight = new System.Windows.Forms.ComboBox();
            this.checkBoxFontVariantSmallCaps = new System.Windows.Forms.CheckBox();
            this.checkBoxFontStyleItalic = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label36 = new System.Windows.Forms.Label();
            this.numericUpDownFontSize = new System.Windows.Forms.NumericUpDown();
            this.comboBoxFontSizeAbsolute = new System.Windows.Forms.ComboBox();
            this.comboBoxFontSizeUnit = new System.Windows.Forms.ComboBox();
            this.radioButtonFontSizeAbsolute = new System.Windows.Forms.RadioButton();
            this.radioButtonFontSizeUnit = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label50 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label49 = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.numericUpDownBackgroundPositionX = new System.Windows.Forms.NumericUpDown();
            this.label52 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.comboBoxBackgroundPosition = new System.Windows.Forms.ComboBox();
            this.comboBoxBackgroundPositionY = new System.Windows.Forms.ComboBox();
            this.comboBoxBackgroundPositionX = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.comboBoxBackgroundAttachment = new System.Windows.Forms.ComboBox();
            this.comboBoxBackgroundRepeat = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.buttonBackgroundImage = new System.Windows.Forms.Button();
            this.textBoxBackgroundImage = new System.Windows.Forms.TextBox();
            this.numericUpDownBackgroundPositionY = new System.Windows.Forms.NumericUpDown();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label27 = new System.Windows.Forms.Label();
            this.checkBox6 = new System.Windows.Forms.CheckBox();
            this.groupBox18 = new System.Windows.Forms.GroupBox();
            this.pictureBox15 = new System.Windows.Forms.PictureBox();
            this.label32 = new System.Windows.Forms.Label();
            this.tabControlBorders = new System.Windows.Forms.TabControl();
            this.tabPageAll = new System.Windows.Forms.TabPage();
            this.groupBox25 = new System.Windows.Forms.GroupBox();
            this.numericUpDownBorderWidthAll = new System.Windows.Forms.NumericUpDown();
            this.comboBoxBorderStyleAll = new System.Windows.Forms.ComboBox();
            this.comboBoxBorderWidthAll = new System.Windows.Forms.ComboBox();
            this.numericUpDownMarginAll = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownPaddingAll = new System.Windows.Forms.NumericUpDown();
            this.comboBoxMarginAll = new System.Windows.Forms.ComboBox();
            this.comboBoxPaddingAll = new System.Windows.Forms.ComboBox();
            this.tabPageLeft = new System.Windows.Forms.TabPage();
            this.groupBox24 = new System.Windows.Forms.GroupBox();
            this.numericUpDownBorderLeftWidth = new System.Windows.Forms.NumericUpDown();
            this.comboBoxBorderLeftStyle = new System.Windows.Forms.ComboBox();
            this.comboBoxBorderLeftWidthUnit = new System.Windows.Forms.ComboBox();
            this.numericUpDownMarginLeft = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownPaddingLeft = new System.Windows.Forms.NumericUpDown();
            this.comboBoxMarginLeftUnit = new System.Windows.Forms.ComboBox();
            this.comboBoxPaddingLeftUnit = new System.Windows.Forms.ComboBox();
            this.tabPageRight = new System.Windows.Forms.TabPage();
            this.groupBox17 = new System.Windows.Forms.GroupBox();
            this.comboBoxBorderRightWidthUnit = new System.Windows.Forms.ComboBox();
            this.numericUpDownBorderRightWidth = new System.Windows.Forms.NumericUpDown();
            this.comboBoxBorderRightStyle = new System.Windows.Forms.ComboBox();
            this.numericUpDownPaddingRight = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownMarginRight = new System.Windows.Forms.NumericUpDown();
            this.comboBoxMarginRightUnit = new System.Windows.Forms.ComboBox();
            this.comboBoxPaddingRightUnit = new System.Windows.Forms.ComboBox();
            this.tabPageTop = new System.Windows.Forms.TabPage();
            this.groupBox13 = new System.Windows.Forms.GroupBox();
            this.numericUpDownBorderTopWidth = new System.Windows.Forms.NumericUpDown();
            this.comboBoxBorderTopStyle = new System.Windows.Forms.ComboBox();
            this.comboBoxBorderTopWidthUnit = new System.Windows.Forms.ComboBox();
            this.comboBoxPaddingTopUnit = new System.Windows.Forms.ComboBox();
            this.comboBoxMarginTopUnit = new System.Windows.Forms.ComboBox();
            this.numericUpDownPaddingTop = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownMarginTop = new System.Windows.Forms.NumericUpDown();
            this.tabPageBottom = new System.Windows.Forms.TabPage();
            this.groupBox12 = new System.Windows.Forms.GroupBox();
            this.comboBoxBorderBottomWidthUnit = new System.Windows.Forms.ComboBox();
            this.comboBoxBorderBottomStyle = new System.Windows.Forms.ComboBox();
            this.numericUpDownBorderBottomWidth = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownPaddingBottom = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownMarginBottom = new System.Windows.Forms.NumericUpDown();
            this.comboBoxMarginBottomUnit = new System.Windows.Forms.ComboBox();
            this.comboBoxPaddingBottomUnit = new System.Windows.Forms.ComboBox();
            this.imageListBorderTabs = new System.Windows.Forms.ImageList(this.components);
            this.label56 = new System.Windows.Forms.Label();
            this.label55 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.label54 = new System.Windows.Forms.Label();
            this.label53 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.groupBox21 = new System.Windows.Forms.GroupBox();
            this.label67 = new System.Windows.Forms.Label();
            this.label28 = new System.Windows.Forms.Label();
            this.comboBoxTextDecoration = new System.Windows.Forms.ComboBox();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.pictureBox6 = new System.Windows.Forms.PictureBox();
            this.pictureBox5 = new System.Windows.Forms.PictureBox();
            this.label29 = new System.Windows.Forms.Label();
            this.radioButtonDirectionRemove = new System.Windows.Forms.RadioButton();
            this.radioButtonDirectionRtl = new System.Windows.Forms.RadioButton();
            this.radioButtonDirectionLtr = new System.Windows.Forms.RadioButton();
            this.label33 = new System.Windows.Forms.Label();
            this.label34 = new System.Windows.Forms.Label();
            this.numericUpDownTextIndent = new System.Windows.Forms.NumericUpDown();
            this.label68 = new System.Windows.Forms.Label();
            this.label51 = new System.Windows.Forms.Label();
            this.comboBoxTextIdentUnit = new System.Windows.Forms.ComboBox();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.label66 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.numericUpDownWordSpacing = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownLetterSpacing = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownLineHeight = new System.Windows.Forms.NumericUpDown();
            this.label37 = new System.Windows.Forms.Label();
            this.comboBoxWordSpacingUnit = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.comboBoxLineHeightUnit = new System.Windows.Forms.ComboBox();
            this.comboBoxLetterSpacingUnit = new System.Windows.Forms.ComboBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.groupBox19 = new System.Windows.Forms.GroupBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.radioButtonTextAlignDisable = new System.Windows.Forms.RadioButton();
            this.radioButtonTextAlignJustify = new System.Windows.Forms.RadioButton();
            this.radioButtonTextAlignCenter = new System.Windows.Forms.RadioButton();
            this.radioButtonTextAlignRight = new System.Windows.Forms.RadioButton();
            this.label10 = new System.Windows.Forms.Label();
            this.radioButtonTextAlignLeft = new System.Windows.Forms.RadioButton();
            this.groupBox20 = new System.Windows.Forms.GroupBox();
            this.label65 = new System.Windows.Forms.Label();
            this.label30 = new System.Windows.Forms.Label();
            this.comboBoxStyleVerticalAlignment = new System.Windows.Forms.ComboBox();
            this.groupBox10 = new System.Windows.Forms.GroupBox();
            this.label19 = new System.Windows.Forms.Label();
            this.comboBoxStyleLineBreak = new System.Windows.Forms.ComboBox();
            this.label48 = new System.Windows.Forms.Label();
            this.comboBoxStyleOverFlow = new System.Windows.Forms.ComboBox();
            this.label47 = new System.Windows.Forms.Label();
            this.comboBoxDisplay = new System.Windows.Forms.ComboBox();
            this.label45 = new System.Windows.Forms.Label();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.numericUpDownLeft = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownTop = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownWidth = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownHeight = new System.Windows.Forms.NumericUpDown();
            this.radioButtonPositionRemove = new System.Windows.Forms.RadioButton();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.comboBoxWidthUnit = new System.Windows.Forms.ComboBox();
            this.comboBoxHeightUnit = new System.Windows.Forms.ComboBox();
            this.comboBoxLeftUnit = new System.Windows.Forms.ComboBox();
            this.comboBoxTopUnit = new System.Windows.Forms.ComboBox();
            this.radioButtonPositionAbsolute = new System.Windows.Forms.RadioButton();
            this.radioButtonPositionRelative = new System.Windows.Forms.RadioButton();
            this.radioButtonPositionFixed = new System.Windows.Forms.RadioButton();
            this.radioButtonPositionNone = new System.Windows.Forms.RadioButton();
            this.groupBox14 = new System.Windows.Forms.GroupBox();
            this.pictureBox10 = new System.Windows.Forms.PictureBox();
            this.pictureBox8 = new System.Windows.Forms.PictureBox();
            this.pictureBox7 = new System.Windows.Forms.PictureBox();
            this.label69 = new System.Windows.Forms.Label();
            this.buttonListStyleImage = new System.Windows.Forms.Button();
            this.label22 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.radioButtonListBulletImage = new System.Windows.Forms.RadioButton();
            this.textBoxListStyleImage = new System.Windows.Forms.TextBox();
            this.comboBoxListStylePosition = new System.Windows.Forms.ComboBox();
            this.label21 = new System.Windows.Forms.Label();
            this.comboBoxListStyleType = new System.Windows.Forms.ComboBox();
            this.radioButtonListBulletStyle = new System.Windows.Forms.RadioButton();
            this.radioButtonListBulletNone = new System.Windows.Forms.RadioButton();
            this.label35 = new System.Windows.Forms.Label();
            this.label46 = new System.Windows.Forms.Label();
            this.label57 = new System.Windows.Forms.Label();
            this.groupBox23 = new System.Windows.Forms.GroupBox();
            this.numericUpDownColumnGap = new System.Windows.Forms.NumericUpDown();
            this.comboBoxColumnGap = new System.Windows.Forms.ComboBox();
            this.labelColumnGap = new System.Windows.Forms.Label();
            this.pictureBox14 = new System.Windows.Forms.PictureBox();
            this.numericUpDownRuleWidth = new System.Windows.Forms.NumericUpDown();
            this.comboBoxCommonRuleWidth = new System.Windows.Forms.ComboBox();
            this.label42 = new System.Windows.Forms.Label();
            this.label41 = new System.Windows.Forms.Label();
            this.label40 = new System.Windows.Forms.Label();
            this.comboBoxColumnRuleStyle = new System.Windows.Forms.ComboBox();
            this.label39 = new System.Windows.Forms.Label();
            this.numericUpDownColumns = new System.Windows.Forms.NumericUpDown();
            this.label72 = new System.Windows.Forms.Label();
            this.groupBox16 = new System.Windows.Forms.GroupBox();
            this.pictureBox13 = new System.Windows.Forms.PictureBox();
            this.pictureBox12 = new System.Windows.Forms.PictureBox();
            this.numericUpDownRowSpan = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownColumnSpan = new System.Windows.Forms.NumericUpDown();
            this.label44 = new System.Windows.Forms.Label();
            this.label43 = new System.Windows.Forms.Label();
            this.label74 = new System.Windows.Forms.Label();
            this.label38 = new System.Windows.Forms.Label();
            this.comboBoxCaptionSide = new System.Windows.Forms.ComboBox();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.label71 = new System.Windows.Forms.Label();
            this.tabControlEditor = new System.Windows.Forms.TabControl();
            this.tabPageFonts = new System.Windows.Forms.TabPage();
            this.tabPageLayout = new System.Windows.Forms.TabPage();
            this.tabPageBackground = new System.Windows.Forms.TabPage();
            this.tabPagePosition = new System.Windows.Forms.TabPage();
            this.tabPageBorders = new System.Windows.Forms.TabPage();
            this.tabPageLists = new System.Windows.Forms.TabPage();
            this.tabPageTables = new System.Windows.Forms.TabPage();
            this.groupBox11 = new System.Windows.Forms.GroupBox();
            this.pictureBox11 = new System.Windows.Forms.PictureBox();
            this.label73 = new System.Windows.Forms.Label();
            this.tabPageMisc = new System.Windows.Forms.TabPage();
            this.groupBox15 = new System.Windows.Forms.GroupBox();
            this.label70 = new System.Windows.Forms.Label();
            this.pictureBox9 = new System.Windows.Forms.PictureBox();
            this.label64 = new System.Windows.Forms.Label();
            this.label63 = new System.Windows.Forms.Label();
            this.label62 = new System.Windows.Forms.Label();
            this.label61 = new System.Windows.Forms.Label();
            this.label60 = new System.Windows.Forms.Label();
            this.label59 = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.comboBoxCursor = new System.Windows.Forms.ComboBox();
            this.label58 = new System.Windows.Forms.Label();
            this.label76 = new System.Windows.Forms.Label();
            this.groupBox22 = new System.Windows.Forms.GroupBox();
            this.radioButtonVisiblityRemove = new System.Windows.Forms.RadioButton();
            this.radioButtonVisibilityNone = new System.Windows.Forms.RadioButton();
            this.radioButtonVisibilityHidden = new System.Windows.Forms.RadioButton();
            this.radioButtonVisibilityVisible = new System.Windows.Forms.RadioButton();
            this.label31 = new System.Windows.Forms.Label();
            this.label75 = new System.Windows.Forms.Label();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.Border = new System.Windows.Forms.Panel();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownFontSize)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownBackgroundPositionX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownBackgroundPositionY)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.groupBox18.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox15)).BeginInit();
            this.tabControlBorders.SuspendLayout();
            this.tabPageAll.SuspendLayout();
            this.groupBox25.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownBorderWidthAll)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMarginAll)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPaddingAll)).BeginInit();
            this.tabPageLeft.SuspendLayout();
            this.groupBox24.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownBorderLeftWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMarginLeft)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPaddingLeft)).BeginInit();
            this.tabPageRight.SuspendLayout();
            this.groupBox17.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownBorderRightWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPaddingRight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMarginRight)).BeginInit();
            this.tabPageTop.SuspendLayout();
            this.groupBox13.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownBorderTopWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPaddingTop)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMarginTop)).BeginInit();
            this.tabPageBottom.SuspendLayout();
            this.groupBox12.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownBorderBottomWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPaddingBottom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMarginBottom)).BeginInit();
            this.groupBox21.SuspendLayout();
            this.groupBox8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTextIndent)).BeginInit();
            this.groupBox7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownWordSpacing)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLetterSpacing)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLineHeight)).BeginInit();
            this.groupBox6.SuspendLayout();
            this.groupBox19.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.groupBox20.SuspendLayout();
            this.groupBox10.SuspendLayout();
            this.groupBox9.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLeft)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTop)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownHeight)).BeginInit();
            this.groupBox14.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox10)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox7)).BeginInit();
            this.groupBox23.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownColumnGap)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox14)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRuleWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownColumns)).BeginInit();
            this.groupBox16.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox13)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox12)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRowSpan)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownColumnSpan)).BeginInit();
            this.tabControlEditor.SuspendLayout();
            this.tabPageFonts.SuspendLayout();
            this.tabPageLayout.SuspendLayout();
            this.tabPageBackground.SuspendLayout();
            this.tabPagePosition.SuspendLayout();
            this.tabPageBorders.SuspendLayout();
            this.tabPageLists.SuspendLayout();
            this.tabPageTables.SuspendLayout();
            this.groupBox11.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox11)).BeginInit();
            this.tabPageMisc.SuspendLayout();
            this.groupBox15.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox9)).BeginInit();
            this.groupBox22.SuspendLayout();
            this.Border.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.comboBoxTextTransform);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.comboBoxFontWeight);
            this.groupBox3.Controls.Add(this.checkBoxFontVariantSmallCaps);
            this.groupBox3.Controls.Add(this.checkBoxFontStyleItalic);
            this.groupBox3.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox3.Location = new System.Drawing.Point(220, 177);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(204, 218);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Font Styles";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(5, 120);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(184, 16);
            this.label3.TabIndex = 4;
            this.label3.Tag = "";
            this.label3.Text = "Letter Transformation";
            // 
            // comboBoxTextTransform
            // 
            this.comboBoxTextTransform.DisplayMember = "Member";
            this.comboBoxTextTransform.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxTextTransform.Location = new System.Drawing.Point(8, 136);
            this.comboBoxTextTransform.Name = "comboBoxTextTransform";
            this.comboBoxTextTransform.Size = new System.Drawing.Size(168, 21);
            this.comboBoxTextTransform.TabIndex = 2;
            this.comboBoxTextTransform.Tag = "text-transform";
            this.comboBoxTextTransform.ValueMember = "Value";
            this.comboBoxTextTransform.SelectionChangeCommitted += new System.EventHandler(this.comboBox_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(5, 72);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(184, 16);
            this.label2.TabIndex = 0;
            this.label2.Text = "Thickness";
            // 
            // comboBoxFontWeight
            // 
            this.comboBoxFontWeight.DisplayMember = "Member";
            this.comboBoxFontWeight.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxFontWeight.Location = new System.Drawing.Point(8, 88);
            this.comboBoxFontWeight.Name = "comboBoxFontWeight";
            this.comboBoxFontWeight.Size = new System.Drawing.Size(168, 21);
            this.comboBoxFontWeight.TabIndex = 0;
            this.comboBoxFontWeight.Tag = "font-weight";
            this.comboBoxFontWeight.ValueMember = "Value";
            this.comboBoxFontWeight.SelectionChangeCommitted += new System.EventHandler(this.comboBox_SelectedIndexChanged);
            // 
            // checkBoxFontVariantSmallCaps
            // 
            this.checkBoxFontVariantSmallCaps.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.checkBoxFontVariantSmallCaps.Location = new System.Drawing.Point(8, 40);
            this.checkBoxFontVariantSmallCaps.Name = "checkBoxFontVariantSmallCaps";
            this.checkBoxFontVariantSmallCaps.Size = new System.Drawing.Size(184, 24);
            this.checkBoxFontVariantSmallCaps.TabIndex = 19;
            this.checkBoxFontVariantSmallCaps.Tag = "font-variant|small-caps";
            this.checkBoxFontVariantSmallCaps.Text = "Small Caps";
            this.checkBoxFontVariantSmallCaps.CheckedChanged += new System.EventHandler(this.checkBox_CheckedChanged);
            // 
            // checkBoxFontStyleItalic
            // 
            this.checkBoxFontStyleItalic.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.checkBoxFontStyleItalic.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxFontStyleItalic.Location = new System.Drawing.Point(8, 16);
            this.checkBoxFontStyleItalic.Name = "checkBoxFontStyleItalic";
            this.checkBoxFontStyleItalic.Size = new System.Drawing.Size(184, 24);
            this.checkBoxFontStyleItalic.TabIndex = 18;
            this.checkBoxFontStyleItalic.Tag = "font-style|italic";
            this.checkBoxFontStyleItalic.Text = "Italic";
            this.checkBoxFontStyleItalic.CheckedChanged += new System.EventHandler(this.checkBox_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.label36);
            this.groupBox2.Controls.Add(this.numericUpDownFontSize);
            this.groupBox2.Controls.Add(this.comboBoxFontSizeAbsolute);
            this.groupBox2.Controls.Add(this.comboBoxFontSizeUnit);
            this.groupBox2.Controls.Add(this.radioButtonFontSizeAbsolute);
            this.groupBox2.Controls.Add(this.radioButtonFontSizeUnit);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox2.Location = new System.Drawing.Point(8, 177);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(204, 218);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Font Attributes";
            // 
            // label36
            // 
            this.label36.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label36.Location = new System.Drawing.Point(8, 72);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(192, 32);
            this.label36.TabIndex = 1;
            this.label36.Text = "Specific or generic font size:";
            this.label36.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // numericUpDownFontSize
            // 
            this.numericUpDownFontSize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.numericUpDownFontSize.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numericUpDownFontSize.Enabled = false;
            this.numericUpDownFontSize.Location = new System.Drawing.Point(24, 128);
            this.numericUpDownFontSize.Name = "numericUpDownFontSize";
            this.numericUpDownFontSize.Size = new System.Drawing.Size(64, 20);
            this.numericUpDownFontSize.TabIndex = 2;
            this.numericUpDownFontSize.Tag = "font-size|value";
            this.numericUpDownFontSize.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownFontSize.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            this.numericUpDownFontSize.KeyDown += new System.Windows.Forms.KeyEventHandler(this.numericUpDown_KeyDown);
            // 
            // comboBoxFontSizeAbsolute
            // 
            this.comboBoxFontSizeAbsolute.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxFontSizeAbsolute.DisplayMember = "Member";
            this.comboBoxFontSizeAbsolute.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxFontSizeAbsolute.Enabled = false;
            this.comboBoxFontSizeAbsolute.Location = new System.Drawing.Point(24, 176);
            this.comboBoxFontSizeAbsolute.Name = "comboBoxFontSizeAbsolute";
            this.comboBoxFontSizeAbsolute.Size = new System.Drawing.Size(164, 21);
            this.comboBoxFontSizeAbsolute.TabIndex = 3;
            this.comboBoxFontSizeAbsolute.Tag = "font-size|absolute";
            this.comboBoxFontSizeAbsolute.ValueMember = "Value";
            this.comboBoxFontSizeAbsolute.SelectionChangeCommitted += new System.EventHandler(this.comboBox_SelectedIndexChanged);
            // 
            // comboBoxFontSizeUnit
            // 
            this.comboBoxFontSizeUnit.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxFontSizeUnit.DisplayMember = "Member";
            this.comboBoxFontSizeUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxFontSizeUnit.Location = new System.Drawing.Point(88, 128);
            this.comboBoxFontSizeUnit.Name = "comboBoxFontSizeUnit";
            this.comboBoxFontSizeUnit.Size = new System.Drawing.Size(100, 21);
            this.comboBoxFontSizeUnit.TabIndex = 1;
            this.comboBoxFontSizeUnit.Tag = "font-size|unit";
            this.comboBoxFontSizeUnit.ValueMember = "Value";
            this.comboBoxFontSizeUnit.SelectionChangeCommitted += new System.EventHandler(this.comboBox_SelectedIndexChanged);
            // 
            // radioButtonFontSizeAbsolute
            // 
            this.radioButtonFontSizeAbsolute.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.radioButtonFontSizeAbsolute.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.radioButtonFontSizeAbsolute.Location = new System.Drawing.Point(8, 160);
            this.radioButtonFontSizeAbsolute.Name = "radioButtonFontSizeAbsolute";
            this.radioButtonFontSizeAbsolute.Size = new System.Drawing.Size(184, 16);
            this.radioButtonFontSizeAbsolute.TabIndex = 3;
            this.radioButtonFontSizeAbsolute.Text = "Size Name:";
            // 
            // radioButtonFontSizeUnit
            // 
            this.radioButtonFontSizeUnit.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.radioButtonFontSizeUnit.Checked = true;
            this.radioButtonFontSizeUnit.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.radioButtonFontSizeUnit.Location = new System.Drawing.Point(8, 112);
            this.radioButtonFontSizeUnit.Name = "radioButtonFontSizeUnit";
            this.radioButtonFontSizeUnit.Size = new System.Drawing.Size(188, 16);
            this.radioButtonFontSizeUnit.TabIndex = 2;
            this.radioButtonFontSizeUnit.TabStop = true;
            this.radioButtonFontSizeUnit.Text = "Specific size:";
            this.radioButtonFontSizeUnit.CheckedChanged += new System.EventHandler(this.radioButtonFontSizeUnit_CheckedChanged);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(8, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(192, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Text Color";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.label50);
            this.groupBox1.Controls.Add(this.label18);
            this.groupBox1.Controls.Add(this.label49);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox1.Location = new System.Drawing.Point(8, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(416, 159);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Font Family";
            // 
            // label50
            // 
            this.label50.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.label50.BackColor = System.Drawing.SystemColors.Info;
            this.label50.Cursor = System.Windows.Forms.Cursors.Help;
            this.label50.ForeColor = System.Drawing.SystemColors.InfoText;
            this.label50.Location = new System.Drawing.Point(5, 15);
            this.label50.Name = "label50";
            this.label50.Size = new System.Drawing.Size(112, 133);
            this.label50.TabIndex = 0;
            this.label50.Text = "Can be any number of fonts or generic fonts. The browser uses the first local ava" +
                "ilable font from the list.";
            // 
            // label18
            // 
            this.label18.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.label18.BackColor = System.Drawing.SystemColors.GrayText;
            this.label18.Location = new System.Drawing.Point(8, 18);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(112, 133);
            this.label18.TabIndex = 2;
            // 
            // label49
            // 
            this.label49.Location = new System.Drawing.Point(123, 22);
            this.label49.Name = "label49";
            this.label49.Size = new System.Drawing.Size(112, 16);
            this.label49.TabIndex = 0;
            this.label49.Text = "Font Family List";
            // 
            // groupBox5
            // 
            this.groupBox5.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox5.Controls.Add(this.numericUpDownBackgroundPositionX);
            this.groupBox5.Controls.Add(this.label52);
            this.groupBox5.Controls.Add(this.label9);
            this.groupBox5.Controls.Add(this.label8);
            this.groupBox5.Controls.Add(this.label7);
            this.groupBox5.Controls.Add(this.comboBoxBackgroundPosition);
            this.groupBox5.Controls.Add(this.comboBoxBackgroundPositionY);
            this.groupBox5.Controls.Add(this.comboBoxBackgroundPositionX);
            this.groupBox5.Controls.Add(this.label6);
            this.groupBox5.Controls.Add(this.label5);
            this.groupBox5.Controls.Add(this.comboBoxBackgroundAttachment);
            this.groupBox5.Controls.Add(this.comboBoxBackgroundRepeat);
            this.groupBox5.Controls.Add(this.label4);
            this.groupBox5.Controls.Add(this.buttonBackgroundImage);
            this.groupBox5.Controls.Add(this.textBoxBackgroundImage);
            this.groupBox5.Controls.Add(this.numericUpDownBackgroundPositionY);
            this.groupBox5.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox5.Location = new System.Drawing.Point(8, 96);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(416, 298);
            this.groupBox5.TabIndex = 1;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Background Image";
            // 
            // numericUpDownBackgroundPositionX
            // 
            this.numericUpDownBackgroundPositionX.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numericUpDownBackgroundPositionX.Enabled = false;
            this.numericUpDownBackgroundPositionX.Location = new System.Drawing.Point(151, 202);
            this.numericUpDownBackgroundPositionX.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.numericUpDownBackgroundPositionX.Minimum = new decimal(new int[] {
            9999,
            0,
            0,
            -2147483648});
            this.numericUpDownBackgroundPositionX.Name = "numericUpDownBackgroundPositionX";
            this.numericUpDownBackgroundPositionX.Size = new System.Drawing.Size(48, 20);
            this.numericUpDownBackgroundPositionX.TabIndex = 6;
            this.numericUpDownBackgroundPositionX.Tag = "background-position-x|value";
            this.numericUpDownBackgroundPositionX.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            this.numericUpDownBackgroundPositionX.KeyDown += new System.Windows.Forms.KeyEventHandler(this.numericUpDown_KeyDown);
            // 
            // label52
            // 
            this.label52.Location = new System.Drawing.Point(7, 162);
            this.label52.Name = "label52";
            this.label52.Size = new System.Drawing.Size(136, 40);
            this.label52.TabIndex = 17;
            this.label52.Text = "In Association with the container element";
            this.label52.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label9
            // 
            this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label9.Location = new System.Drawing.Point(16, 137);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(392, 14);
            this.label9.TabIndex = 16;
            this.label9.Text = "How to position the image?";
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(7, 226);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(144, 16);
            this.label8.TabIndex = 13;
            this.label8.Text = "Vertical:";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(7, 202);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(144, 16);
            this.label7.TabIndex = 12;
            this.label7.Text = "Horizontal:";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboBoxBackgroundPosition
            // 
            this.comboBoxBackgroundPosition.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxBackgroundPosition.DisplayMember = "Member";
            this.comboBoxBackgroundPosition.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxBackgroundPosition.Location = new System.Drawing.Point(151, 162);
            this.comboBoxBackgroundPosition.Name = "comboBoxBackgroundPosition";
            this.comboBoxBackgroundPosition.Size = new System.Drawing.Size(144, 21);
            this.comboBoxBackgroundPosition.TabIndex = 4;
            this.comboBoxBackgroundPosition.Tag = "background-position";
            this.comboBoxBackgroundPosition.ValueMember = "Value";
            this.comboBoxBackgroundPosition.SelectionChangeCommitted += new System.EventHandler(this.comboBox_SelectedIndexChanged);
            // 
            // comboBoxBackgroundPositionY
            // 
            this.comboBoxBackgroundPositionY.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxBackgroundPositionY.DisplayMember = "Member";
            this.comboBoxBackgroundPositionY.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxBackgroundPositionY.Location = new System.Drawing.Point(199, 226);
            this.comboBoxBackgroundPositionY.Name = "comboBoxBackgroundPositionY";
            this.comboBoxBackgroundPositionY.Size = new System.Drawing.Size(96, 21);
            this.comboBoxBackgroundPositionY.TabIndex = 7;
            this.comboBoxBackgroundPositionY.Tag = "background-position-y|unit";
            this.comboBoxBackgroundPositionY.ValueMember = "Value";
            this.comboBoxBackgroundPositionY.SelectionChangeCommitted += new System.EventHandler(this.comboBox_SelectedIndexChanged);
            // 
            // comboBoxBackgroundPositionX
            // 
            this.comboBoxBackgroundPositionX.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxBackgroundPositionX.DisplayMember = "Member";
            this.comboBoxBackgroundPositionX.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxBackgroundPositionX.Location = new System.Drawing.Point(199, 202);
            this.comboBoxBackgroundPositionX.Name = "comboBoxBackgroundPositionX";
            this.comboBoxBackgroundPositionX.Size = new System.Drawing.Size(96, 21);
            this.comboBoxBackgroundPositionX.TabIndex = 5;
            this.comboBoxBackgroundPositionX.Tag = "background-position-x|unit";
            this.comboBoxBackgroundPositionX.ValueMember = "Value";
            this.comboBoxBackgroundPositionX.SelectionChangeCommitted += new System.EventHandler(this.comboBox_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(31, 105);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(120, 23);
            this.label6.TabIndex = 7;
            this.label6.Text = "Fixing";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(31, 85);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(120, 16);
            this.label5.TabIndex = 6;
            this.label5.Text = "Tile";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboBoxBackgroundAttachment
            // 
            this.comboBoxBackgroundAttachment.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxBackgroundAttachment.DisplayMember = "Member";
            this.comboBoxBackgroundAttachment.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxBackgroundAttachment.Location = new System.Drawing.Point(151, 106);
            this.comboBoxBackgroundAttachment.Name = "comboBoxBackgroundAttachment";
            this.comboBoxBackgroundAttachment.Size = new System.Drawing.Size(144, 21);
            this.comboBoxBackgroundAttachment.TabIndex = 3;
            this.comboBoxBackgroundAttachment.Tag = "background-attachment";
            this.comboBoxBackgroundAttachment.ValueMember = "Value";
            this.comboBoxBackgroundAttachment.SelectedIndexChanged += new System.EventHandler(this.comboBox_SelectedIndexChanged);
            // 
            // comboBoxBackgroundRepeat
            // 
            this.comboBoxBackgroundRepeat.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxBackgroundRepeat.DisplayMember = "Member";
            this.comboBoxBackgroundRepeat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxBackgroundRepeat.Location = new System.Drawing.Point(151, 82);
            this.comboBoxBackgroundRepeat.Name = "comboBoxBackgroundRepeat";
            this.comboBoxBackgroundRepeat.Size = new System.Drawing.Size(144, 21);
            this.comboBoxBackgroundRepeat.TabIndex = 2;
            this.comboBoxBackgroundRepeat.Tag = "background-repeat";
            this.comboBoxBackgroundRepeat.ValueMember = "Value";
            this.comboBoxBackgroundRepeat.SelectedIndexChanged += new System.EventHandler(this.comboBox_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(10, 25);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(389, 21);
            this.label4.TabIndex = 3;
            this.label4.Text = "Select Image from disk:";
            // 
            // buttonBackgroundImage
            // 
            this.buttonBackgroundImage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonBackgroundImage.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.buttonBackgroundImage.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonBackgroundImage.Location = new System.Drawing.Point(301, 51);
            this.buttonBackgroundImage.Name = "buttonBackgroundImage";
            this.buttonBackgroundImage.Size = new System.Drawing.Size(107, 20);
            this.buttonBackgroundImage.TabIndex = 1;
            this.buttonBackgroundImage.Text = "...";
            this.buttonBackgroundImage.Click += new System.EventHandler(this.buttonBackgroundImage_Click);
            // 
            // textBoxBackgroundImage
            // 
            this.textBoxBackgroundImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxBackgroundImage.Location = new System.Drawing.Point(13, 51);
            this.textBoxBackgroundImage.Name = "textBoxBackgroundImage";
            this.textBoxBackgroundImage.Size = new System.Drawing.Size(281, 20);
            this.textBoxBackgroundImage.TabIndex = 0;
            this.textBoxBackgroundImage.Tag = "background-image";
            this.textBoxBackgroundImage.Leave += new System.EventHandler(this.textBox_TextChanged);
            // 
            // numericUpDownBackgroundPositionY
            // 
            this.numericUpDownBackgroundPositionY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numericUpDownBackgroundPositionY.Enabled = false;
            this.numericUpDownBackgroundPositionY.Location = new System.Drawing.Point(151, 226);
            this.numericUpDownBackgroundPositionY.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.numericUpDownBackgroundPositionY.Minimum = new decimal(new int[] {
            9999,
            0,
            0,
            -2147483648});
            this.numericUpDownBackgroundPositionY.Name = "numericUpDownBackgroundPositionY";
            this.numericUpDownBackgroundPositionY.Size = new System.Drawing.Size(48, 20);
            this.numericUpDownBackgroundPositionY.TabIndex = 8;
            this.numericUpDownBackgroundPositionY.Tag = "background-position-y|value";
            this.numericUpDownBackgroundPositionY.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            this.numericUpDownBackgroundPositionY.KeyDown += new System.Windows.Forms.KeyEventHandler(this.numericUpDown_KeyDown);
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.Controls.Add(this.label27);
            this.groupBox4.Controls.Add(this.checkBox6);
            this.groupBox4.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox4.Location = new System.Drawing.Point(8, 8);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(416, 80);
            this.groupBox4.TabIndex = 0;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Background Color";
            // 
            // label27
            // 
            this.label27.Location = new System.Drawing.Point(10, 25);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(100, 42);
            this.label27.TabIndex = 3;
            this.label27.Text = "Background Color:";
            this.label27.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // checkBox6
            // 
            this.checkBox6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox6.Location = new System.Drawing.Point(112, 56);
            this.checkBox6.Name = "checkBox6";
            this.checkBox6.Size = new System.Drawing.Size(280, 16);
            this.checkBox6.TabIndex = 1;
            this.checkBox6.Tag = "transparency|on";
            this.checkBox6.Text = "Transparency";
            this.checkBox6.Visible = false;
            this.checkBox6.CheckedChanged += new System.EventHandler(this.checkBox_CheckedChanged);
            // 
            // groupBox18
            // 
            this.groupBox18.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox18.Controls.Add(this.pictureBox15);
            this.groupBox18.Controls.Add(this.label32);
            this.groupBox18.Controls.Add(this.tabControlBorders);
            this.groupBox18.Controls.Add(this.label56);
            this.groupBox18.Controls.Add(this.label55);
            this.groupBox18.Controls.Add(this.button1);
            this.groupBox18.Controls.Add(this.label54);
            this.groupBox18.Controls.Add(this.label53);
            this.groupBox18.Controls.Add(this.label17);
            this.groupBox18.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox18.Location = new System.Drawing.Point(8, 0);
            this.groupBox18.Name = "groupBox18";
            this.groupBox18.Size = new System.Drawing.Size(416, 395);
            this.groupBox18.TabIndex = 0;
            this.groupBox18.TabStop = false;
            this.groupBox18.Text = "Margin, Padding, and Border Style";
            // 
            // pictureBox15
            // 
            this.pictureBox15.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pictureBox15.Cursor = System.Windows.Forms.Cursors.Help;
            this.pictureBox15.Location = new System.Drawing.Point(8, 283);
            this.pictureBox15.Name = "pictureBox15";
            this.pictureBox15.Size = new System.Drawing.Size(120, 96);
            this.pictureBox15.TabIndex = 112;
            this.pictureBox15.TabStop = false;
            // 
            // label32
            // 
            this.label32.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label32.BackColor = System.Drawing.SystemColors.GrayText;
            this.label32.Location = new System.Drawing.Point(15, 285);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(116, 97);
            this.label32.TabIndex = 113;
            // 
            // tabControlBorders
            // 
            this.tabControlBorders.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.tabControlBorders.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControlBorders.Controls.Add(this.tabPageAll);
            this.tabControlBorders.Controls.Add(this.tabPageLeft);
            this.tabControlBorders.Controls.Add(this.tabPageRight);
            this.tabControlBorders.Controls.Add(this.tabPageTop);
            this.tabControlBorders.Controls.Add(this.tabPageBottom);
            this.tabControlBorders.ImageList = this.imageListBorderTabs;
            this.tabControlBorders.ItemSize = new System.Drawing.Size(48, 21);
            this.tabControlBorders.Location = new System.Drawing.Point(125, 16);
            this.tabControlBorders.Name = "tabControlBorders";
            this.tabControlBorders.SelectedIndex = 0;
            this.tabControlBorders.Size = new System.Drawing.Size(283, 251);
            this.tabControlBorders.TabIndex = 5;
            // 
            // tabPageAll
            // 
            this.tabPageAll.Controls.Add(this.groupBox25);
            this.tabPageAll.Controls.Add(this.numericUpDownMarginAll);
            this.tabPageAll.Controls.Add(this.numericUpDownPaddingAll);
            this.tabPageAll.Controls.Add(this.comboBoxMarginAll);
            this.tabPageAll.Controls.Add(this.comboBoxPaddingAll);
            this.tabPageAll.ImageIndex = 0;
            this.tabPageAll.Location = new System.Drawing.Point(4, 4);
            this.tabPageAll.Name = "tabPageAll";
            this.tabPageAll.Size = new System.Drawing.Size(275, 222);
            this.tabPageAll.TabIndex = 0;
            this.tabPageAll.Text = "All";
            // 
            // groupBox25
            // 
            this.groupBox25.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox25.Controls.Add(this.numericUpDownBorderWidthAll);
            this.groupBox25.Controls.Add(this.comboBoxBorderStyleAll);
            this.groupBox25.Controls.Add(this.comboBoxBorderWidthAll);
            this.groupBox25.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox25.Location = new System.Drawing.Point(8, 40);
            this.groupBox25.Name = "groupBox25";
            this.groupBox25.Size = new System.Drawing.Size(245, 112);
            this.groupBox25.TabIndex = 112;
            this.groupBox25.TabStop = false;
            this.groupBox25.Text = "All Borders";
            // 
            // numericUpDownBorderWidthAll
            // 
            this.numericUpDownBorderWidthAll.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numericUpDownBorderWidthAll.Enabled = false;
            this.numericUpDownBorderWidthAll.Location = new System.Drawing.Point(16, 72);
            this.numericUpDownBorderWidthAll.Name = "numericUpDownBorderWidthAll";
            this.numericUpDownBorderWidthAll.Size = new System.Drawing.Size(64, 20);
            this.numericUpDownBorderWidthAll.TabIndex = 3;
            this.numericUpDownBorderWidthAll.Tag = "border-width|value";
            // 
            // comboBoxBorderStyleAll
            // 
            this.comboBoxBorderStyleAll.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxBorderStyleAll.DisplayMember = "Member";
            this.comboBoxBorderStyleAll.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxBorderStyleAll.Location = new System.Drawing.Point(16, 48);
            this.comboBoxBorderStyleAll.Name = "comboBoxBorderStyleAll";
            this.comboBoxBorderStyleAll.Size = new System.Drawing.Size(205, 21);
            this.comboBoxBorderStyleAll.TabIndex = 1;
            this.comboBoxBorderStyleAll.Tag = "border-style";
            this.comboBoxBorderStyleAll.ValueMember = "Value";
            this.comboBoxBorderStyleAll.SelectionChangeCommitted += new System.EventHandler(this.comboBox_SelectedIndexChanged);
            // 
            // comboBoxBorderWidthAll
            // 
            this.comboBoxBorderWidthAll.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxBorderWidthAll.DisplayMember = "Member";
            this.comboBoxBorderWidthAll.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxBorderWidthAll.Location = new System.Drawing.Point(80, 72);
            this.comboBoxBorderWidthAll.Name = "comboBoxBorderWidthAll";
            this.comboBoxBorderWidthAll.Size = new System.Drawing.Size(141, 21);
            this.comboBoxBorderWidthAll.TabIndex = 2;
            this.comboBoxBorderWidthAll.Tag = "border-width|unit";
            this.comboBoxBorderWidthAll.ValueMember = "Value";
            this.comboBoxBorderWidthAll.SelectionChangeCommitted += new System.EventHandler(this.comboBox_SelectedIndexChanged);
            // 
            // numericUpDownMarginAll
            // 
            this.numericUpDownMarginAll.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numericUpDownMarginAll.Enabled = false;
            this.numericUpDownMarginAll.Location = new System.Drawing.Point(8, 8);
            this.numericUpDownMarginAll.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.numericUpDownMarginAll.Minimum = new decimal(new int[] {
            9999,
            0,
            0,
            -2147483648});
            this.numericUpDownMarginAll.Name = "numericUpDownMarginAll";
            this.numericUpDownMarginAll.Size = new System.Drawing.Size(64, 20);
            this.numericUpDownMarginAll.TabIndex = 1;
            this.numericUpDownMarginAll.Tag = "margin|value";
            this.numericUpDownMarginAll.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            this.numericUpDownMarginAll.KeyDown += new System.Windows.Forms.KeyEventHandler(this.numericUpDown_KeyDown);
            // 
            // numericUpDownPaddingAll
            // 
            this.numericUpDownPaddingAll.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numericUpDownPaddingAll.Enabled = false;
            this.numericUpDownPaddingAll.Location = new System.Drawing.Point(8, 160);
            this.numericUpDownPaddingAll.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.numericUpDownPaddingAll.Minimum = new decimal(new int[] {
            9999,
            0,
            0,
            -2147483648});
            this.numericUpDownPaddingAll.Name = "numericUpDownPaddingAll";
            this.numericUpDownPaddingAll.Size = new System.Drawing.Size(64, 20);
            this.numericUpDownPaddingAll.TabIndex = 3;
            this.numericUpDownPaddingAll.Tag = "padding|value";
            this.numericUpDownPaddingAll.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            this.numericUpDownPaddingAll.KeyDown += new System.Windows.Forms.KeyEventHandler(this.numericUpDown_KeyDown);
            // 
            // comboBoxMarginAll
            // 
            this.comboBoxMarginAll.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxMarginAll.DisplayMember = "Member";
            this.comboBoxMarginAll.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxMarginAll.Location = new System.Drawing.Point(72, 8);
            this.comboBoxMarginAll.Name = "comboBoxMarginAll";
            this.comboBoxMarginAll.Size = new System.Drawing.Size(109, 21);
            this.comboBoxMarginAll.TabIndex = 0;
            this.comboBoxMarginAll.Tag = "margin|unit";
            this.comboBoxMarginAll.ValueMember = "Value";
            this.comboBoxMarginAll.SelectionChangeCommitted += new System.EventHandler(this.comboBox_SelectedIndexChanged);
            // 
            // comboBoxPaddingAll
            // 
            this.comboBoxPaddingAll.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxPaddingAll.DisplayMember = "Member";
            this.comboBoxPaddingAll.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxPaddingAll.Location = new System.Drawing.Point(72, 160);
            this.comboBoxPaddingAll.Name = "comboBoxPaddingAll";
            this.comboBoxPaddingAll.Size = new System.Drawing.Size(109, 21);
            this.comboBoxPaddingAll.TabIndex = 2;
            this.comboBoxPaddingAll.Tag = "padding|unit";
            this.comboBoxPaddingAll.ValueMember = "Value";
            this.comboBoxPaddingAll.SelectionChangeCommitted += new System.EventHandler(this.comboBox_SelectedIndexChanged);
            // 
            // tabPageLeft
            // 
            this.tabPageLeft.Controls.Add(this.groupBox24);
            this.tabPageLeft.Controls.Add(this.numericUpDownMarginLeft);
            this.tabPageLeft.Controls.Add(this.numericUpDownPaddingLeft);
            this.tabPageLeft.Controls.Add(this.comboBoxMarginLeftUnit);
            this.tabPageLeft.Controls.Add(this.comboBoxPaddingLeftUnit);
            this.tabPageLeft.ImageIndex = 1;
            this.tabPageLeft.Location = new System.Drawing.Point(4, 4);
            this.tabPageLeft.Name = "tabPageLeft";
            this.tabPageLeft.Size = new System.Drawing.Size(275, 222);
            this.tabPageLeft.TabIndex = 1;
            this.tabPageLeft.Text = "Left";
            // 
            // groupBox24
            // 
            this.groupBox24.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox24.Controls.Add(this.numericUpDownBorderLeftWidth);
            this.groupBox24.Controls.Add(this.comboBoxBorderLeftStyle);
            this.groupBox24.Controls.Add(this.comboBoxBorderLeftWidthUnit);
            this.groupBox24.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox24.Location = new System.Drawing.Point(8, 40);
            this.groupBox24.Name = "groupBox24";
            this.groupBox24.Size = new System.Drawing.Size(245, 112);
            this.groupBox24.TabIndex = 107;
            this.groupBox24.TabStop = false;
            this.groupBox24.Text = "Left Border";
            // 
            // numericUpDownBorderLeftWidth
            // 
            this.numericUpDownBorderLeftWidth.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numericUpDownBorderLeftWidth.Enabled = false;
            this.numericUpDownBorderLeftWidth.Location = new System.Drawing.Point(16, 72);
            this.numericUpDownBorderLeftWidth.Name = "numericUpDownBorderLeftWidth";
            this.numericUpDownBorderLeftWidth.Size = new System.Drawing.Size(64, 20);
            this.numericUpDownBorderLeftWidth.TabIndex = 3;
            this.numericUpDownBorderLeftWidth.Tag = "border-left-width|value";
            this.numericUpDownBorderLeftWidth.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            this.numericUpDownBorderLeftWidth.KeyDown += new System.Windows.Forms.KeyEventHandler(this.numericUpDown_KeyDown);
            // 
            // comboBoxBorderLeftStyle
            // 
            this.comboBoxBorderLeftStyle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxBorderLeftStyle.DisplayMember = "Member";
            this.comboBoxBorderLeftStyle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxBorderLeftStyle.Location = new System.Drawing.Point(16, 48);
            this.comboBoxBorderLeftStyle.Name = "comboBoxBorderLeftStyle";
            this.comboBoxBorderLeftStyle.Size = new System.Drawing.Size(205, 21);
            this.comboBoxBorderLeftStyle.TabIndex = 1;
            this.comboBoxBorderLeftStyle.Tag = "border-left-style";
            this.comboBoxBorderLeftStyle.ValueMember = "Value";
            this.comboBoxBorderLeftStyle.SelectionChangeCommitted += new System.EventHandler(this.comboBox_SelectedIndexChanged);
            // 
            // comboBoxBorderLeftWidthUnit
            // 
            this.comboBoxBorderLeftWidthUnit.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxBorderLeftWidthUnit.DisplayMember = "Member";
            this.comboBoxBorderLeftWidthUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxBorderLeftWidthUnit.Location = new System.Drawing.Point(80, 72);
            this.comboBoxBorderLeftWidthUnit.Name = "comboBoxBorderLeftWidthUnit";
            this.comboBoxBorderLeftWidthUnit.Size = new System.Drawing.Size(141, 21);
            this.comboBoxBorderLeftWidthUnit.TabIndex = 2;
            this.comboBoxBorderLeftWidthUnit.Tag = "border-left-width|unit";
            this.comboBoxBorderLeftWidthUnit.ValueMember = "Value";
            this.comboBoxBorderLeftWidthUnit.SelectionChangeCommitted += new System.EventHandler(this.comboBox_SelectedIndexChanged);
            // 
            // numericUpDownMarginLeft
            // 
            this.numericUpDownMarginLeft.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numericUpDownMarginLeft.Enabled = false;
            this.numericUpDownMarginLeft.Location = new System.Drawing.Point(8, 8);
            this.numericUpDownMarginLeft.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.numericUpDownMarginLeft.Minimum = new decimal(new int[] {
            9999,
            0,
            0,
            -2147483648});
            this.numericUpDownMarginLeft.Name = "numericUpDownMarginLeft";
            this.numericUpDownMarginLeft.Size = new System.Drawing.Size(64, 20);
            this.numericUpDownMarginLeft.TabIndex = 1;
            this.numericUpDownMarginLeft.Tag = "margin-left|value";
            this.numericUpDownMarginLeft.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            this.numericUpDownMarginLeft.KeyDown += new System.Windows.Forms.KeyEventHandler(this.numericUpDown_KeyDown);
            // 
            // numericUpDownPaddingLeft
            // 
            this.numericUpDownPaddingLeft.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numericUpDownPaddingLeft.Enabled = false;
            this.numericUpDownPaddingLeft.Location = new System.Drawing.Point(8, 160);
            this.numericUpDownPaddingLeft.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.numericUpDownPaddingLeft.Minimum = new decimal(new int[] {
            9999,
            0,
            0,
            -2147483648});
            this.numericUpDownPaddingLeft.Name = "numericUpDownPaddingLeft";
            this.numericUpDownPaddingLeft.Size = new System.Drawing.Size(64, 20);
            this.numericUpDownPaddingLeft.TabIndex = 3;
            this.numericUpDownPaddingLeft.Tag = "padding-left|value";
            this.numericUpDownPaddingLeft.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            this.numericUpDownPaddingLeft.KeyDown += new System.Windows.Forms.KeyEventHandler(this.numericUpDown_KeyDown);
            // 
            // comboBoxMarginLeftUnit
            // 
            this.comboBoxMarginLeftUnit.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxMarginLeftUnit.DisplayMember = "Member";
            this.comboBoxMarginLeftUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxMarginLeftUnit.Location = new System.Drawing.Point(72, 8);
            this.comboBoxMarginLeftUnit.Name = "comboBoxMarginLeftUnit";
            this.comboBoxMarginLeftUnit.Size = new System.Drawing.Size(109, 21);
            this.comboBoxMarginLeftUnit.TabIndex = 0;
            this.comboBoxMarginLeftUnit.Tag = "margin-left|unit";
            this.comboBoxMarginLeftUnit.ValueMember = "Value";
            this.comboBoxMarginLeftUnit.SelectionChangeCommitted += new System.EventHandler(this.comboBox_SelectedIndexChanged);
            // 
            // comboBoxPaddingLeftUnit
            // 
            this.comboBoxPaddingLeftUnit.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxPaddingLeftUnit.DisplayMember = "Member";
            this.comboBoxPaddingLeftUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxPaddingLeftUnit.Location = new System.Drawing.Point(72, 160);
            this.comboBoxPaddingLeftUnit.Name = "comboBoxPaddingLeftUnit";
            this.comboBoxPaddingLeftUnit.Size = new System.Drawing.Size(109, 21);
            this.comboBoxPaddingLeftUnit.TabIndex = 2;
            this.comboBoxPaddingLeftUnit.Tag = "padding-left|unit";
            this.comboBoxPaddingLeftUnit.ValueMember = "Value";
            this.comboBoxPaddingLeftUnit.SelectionChangeCommitted += new System.EventHandler(this.comboBox_SelectedIndexChanged);
            // 
            // tabPageRight
            // 
            this.tabPageRight.Controls.Add(this.groupBox17);
            this.tabPageRight.Controls.Add(this.numericUpDownPaddingRight);
            this.tabPageRight.Controls.Add(this.numericUpDownMarginRight);
            this.tabPageRight.Controls.Add(this.comboBoxMarginRightUnit);
            this.tabPageRight.Controls.Add(this.comboBoxPaddingRightUnit);
            this.tabPageRight.ImageIndex = 2;
            this.tabPageRight.Location = new System.Drawing.Point(4, 4);
            this.tabPageRight.Name = "tabPageRight";
            this.tabPageRight.Size = new System.Drawing.Size(275, 222);
            this.tabPageRight.TabIndex = 2;
            this.tabPageRight.Text = "Right";
            // 
            // groupBox17
            // 
            this.groupBox17.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox17.Controls.Add(this.comboBoxBorderRightWidthUnit);
            this.groupBox17.Controls.Add(this.numericUpDownBorderRightWidth);
            this.groupBox17.Controls.Add(this.comboBoxBorderRightStyle);
            this.groupBox17.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox17.Location = new System.Drawing.Point(8, 40);
            this.groupBox17.Name = "groupBox17";
            this.groupBox17.Size = new System.Drawing.Size(245, 112);
            this.groupBox17.TabIndex = 108;
            this.groupBox17.TabStop = false;
            this.groupBox17.Text = "Right Border";
            // 
            // comboBoxBorderRightWidthUnit
            // 
            this.comboBoxBorderRightWidthUnit.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxBorderRightWidthUnit.DisplayMember = "Member";
            this.comboBoxBorderRightWidthUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxBorderRightWidthUnit.Location = new System.Drawing.Point(80, 72);
            this.comboBoxBorderRightWidthUnit.Name = "comboBoxBorderRightWidthUnit";
            this.comboBoxBorderRightWidthUnit.Size = new System.Drawing.Size(141, 21);
            this.comboBoxBorderRightWidthUnit.TabIndex = 2;
            this.comboBoxBorderRightWidthUnit.Tag = "border-right-width|unit";
            this.comboBoxBorderRightWidthUnit.ValueMember = "Value";
            this.comboBoxBorderRightWidthUnit.SelectionChangeCommitted += new System.EventHandler(this.comboBox_SelectedIndexChanged);
            // 
            // numericUpDownBorderRightWidth
            // 
            this.numericUpDownBorderRightWidth.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numericUpDownBorderRightWidth.Enabled = false;
            this.numericUpDownBorderRightWidth.Location = new System.Drawing.Point(16, 72);
            this.numericUpDownBorderRightWidth.Name = "numericUpDownBorderRightWidth";
            this.numericUpDownBorderRightWidth.Size = new System.Drawing.Size(64, 20);
            this.numericUpDownBorderRightWidth.TabIndex = 3;
            this.numericUpDownBorderRightWidth.Tag = "border-right-width|value";
            this.numericUpDownBorderRightWidth.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            this.numericUpDownBorderRightWidth.KeyDown += new System.Windows.Forms.KeyEventHandler(this.numericUpDown_KeyDown);
            // 
            // comboBoxBorderRightStyle
            // 
            this.comboBoxBorderRightStyle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxBorderRightStyle.DisplayMember = "Member";
            this.comboBoxBorderRightStyle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxBorderRightStyle.Location = new System.Drawing.Point(16, 48);
            this.comboBoxBorderRightStyle.Name = "comboBoxBorderRightStyle";
            this.comboBoxBorderRightStyle.Size = new System.Drawing.Size(205, 21);
            this.comboBoxBorderRightStyle.TabIndex = 1;
            this.comboBoxBorderRightStyle.Tag = "border-right-style";
            this.comboBoxBorderRightStyle.ValueMember = "Value";
            this.comboBoxBorderRightStyle.SelectionChangeCommitted += new System.EventHandler(this.comboBox_SelectedIndexChanged);
            // 
            // numericUpDownPaddingRight
            // 
            this.numericUpDownPaddingRight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numericUpDownPaddingRight.Enabled = false;
            this.numericUpDownPaddingRight.Location = new System.Drawing.Point(8, 160);
            this.numericUpDownPaddingRight.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.numericUpDownPaddingRight.Minimum = new decimal(new int[] {
            9999,
            0,
            0,
            -2147483648});
            this.numericUpDownPaddingRight.Name = "numericUpDownPaddingRight";
            this.numericUpDownPaddingRight.Size = new System.Drawing.Size(64, 20);
            this.numericUpDownPaddingRight.TabIndex = 3;
            this.numericUpDownPaddingRight.Tag = "padding-right|value";
            this.numericUpDownPaddingRight.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            this.numericUpDownPaddingRight.KeyDown += new System.Windows.Forms.KeyEventHandler(this.numericUpDown_KeyDown);
            // 
            // numericUpDownMarginRight
            // 
            this.numericUpDownMarginRight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numericUpDownMarginRight.Enabled = false;
            this.numericUpDownMarginRight.Location = new System.Drawing.Point(8, 8);
            this.numericUpDownMarginRight.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.numericUpDownMarginRight.Minimum = new decimal(new int[] {
            9999,
            0,
            0,
            -2147483648});
            this.numericUpDownMarginRight.Name = "numericUpDownMarginRight";
            this.numericUpDownMarginRight.Size = new System.Drawing.Size(64, 20);
            this.numericUpDownMarginRight.TabIndex = 1;
            this.numericUpDownMarginRight.Tag = "margin-right|value";
            this.numericUpDownMarginRight.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            this.numericUpDownMarginRight.KeyDown += new System.Windows.Forms.KeyEventHandler(this.numericUpDown_KeyDown);
            // 
            // comboBoxMarginRightUnit
            // 
            this.comboBoxMarginRightUnit.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxMarginRightUnit.DisplayMember = "Member";
            this.comboBoxMarginRightUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxMarginRightUnit.Location = new System.Drawing.Point(72, 8);
            this.comboBoxMarginRightUnit.Name = "comboBoxMarginRightUnit";
            this.comboBoxMarginRightUnit.Size = new System.Drawing.Size(109, 21);
            this.comboBoxMarginRightUnit.TabIndex = 0;
            this.comboBoxMarginRightUnit.Tag = "margin-right|unit";
            this.comboBoxMarginRightUnit.ValueMember = "Value";
            this.comboBoxMarginRightUnit.SelectionChangeCommitted += new System.EventHandler(this.comboBox_SelectedIndexChanged);
            // 
            // comboBoxPaddingRightUnit
            // 
            this.comboBoxPaddingRightUnit.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxPaddingRightUnit.DisplayMember = "Member";
            this.comboBoxPaddingRightUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxPaddingRightUnit.Location = new System.Drawing.Point(72, 160);
            this.comboBoxPaddingRightUnit.Name = "comboBoxPaddingRightUnit";
            this.comboBoxPaddingRightUnit.Size = new System.Drawing.Size(109, 21);
            this.comboBoxPaddingRightUnit.TabIndex = 2;
            this.comboBoxPaddingRightUnit.Tag = "padding-right|unit";
            this.comboBoxPaddingRightUnit.ValueMember = "Value";
            this.comboBoxPaddingRightUnit.SelectionChangeCommitted += new System.EventHandler(this.comboBox_SelectedIndexChanged);
            // 
            // tabPageTop
            // 
            this.tabPageTop.Controls.Add(this.groupBox13);
            this.tabPageTop.Controls.Add(this.comboBoxPaddingTopUnit);
            this.tabPageTop.Controls.Add(this.comboBoxMarginTopUnit);
            this.tabPageTop.Controls.Add(this.numericUpDownPaddingTop);
            this.tabPageTop.Controls.Add(this.numericUpDownMarginTop);
            this.tabPageTop.ImageIndex = 3;
            this.tabPageTop.Location = new System.Drawing.Point(4, 4);
            this.tabPageTop.Name = "tabPageTop";
            this.tabPageTop.Size = new System.Drawing.Size(275, 222);
            this.tabPageTop.TabIndex = 3;
            this.tabPageTop.Text = "Top";
            // 
            // groupBox13
            // 
            this.groupBox13.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox13.Controls.Add(this.numericUpDownBorderTopWidth);
            this.groupBox13.Controls.Add(this.comboBoxBorderTopStyle);
            this.groupBox13.Controls.Add(this.comboBoxBorderTopWidthUnit);
            this.groupBox13.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox13.Location = new System.Drawing.Point(8, 40);
            this.groupBox13.Name = "groupBox13";
            this.groupBox13.Size = new System.Drawing.Size(245, 112);
            this.groupBox13.TabIndex = 106;
            this.groupBox13.TabStop = false;
            this.groupBox13.Text = "Top Border";
            // 
            // numericUpDownBorderTopWidth
            // 
            this.numericUpDownBorderTopWidth.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numericUpDownBorderTopWidth.Enabled = false;
            this.numericUpDownBorderTopWidth.Location = new System.Drawing.Point(16, 72);
            this.numericUpDownBorderTopWidth.Name = "numericUpDownBorderTopWidth";
            this.numericUpDownBorderTopWidth.Size = new System.Drawing.Size(64, 20);
            this.numericUpDownBorderTopWidth.TabIndex = 3;
            this.numericUpDownBorderTopWidth.Tag = "border-top-width|value";
            this.numericUpDownBorderTopWidth.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            this.numericUpDownBorderTopWidth.KeyDown += new System.Windows.Forms.KeyEventHandler(this.numericUpDown_KeyDown);
            // 
            // comboBoxBorderTopStyle
            // 
            this.comboBoxBorderTopStyle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxBorderTopStyle.DisplayMember = "Member";
            this.comboBoxBorderTopStyle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxBorderTopStyle.Location = new System.Drawing.Point(16, 48);
            this.comboBoxBorderTopStyle.Name = "comboBoxBorderTopStyle";
            this.comboBoxBorderTopStyle.Size = new System.Drawing.Size(205, 21);
            this.comboBoxBorderTopStyle.TabIndex = 1;
            this.comboBoxBorderTopStyle.Tag = "border-top-style";
            this.comboBoxBorderTopStyle.ValueMember = "Value";
            this.comboBoxBorderTopStyle.SelectionChangeCommitted += new System.EventHandler(this.comboBox_SelectedIndexChanged);
            // 
            // comboBoxBorderTopWidthUnit
            // 
            this.comboBoxBorderTopWidthUnit.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxBorderTopWidthUnit.DisplayMember = "Member";
            this.comboBoxBorderTopWidthUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxBorderTopWidthUnit.Location = new System.Drawing.Point(80, 72);
            this.comboBoxBorderTopWidthUnit.Name = "comboBoxBorderTopWidthUnit";
            this.comboBoxBorderTopWidthUnit.Size = new System.Drawing.Size(141, 21);
            this.comboBoxBorderTopWidthUnit.TabIndex = 2;
            this.comboBoxBorderTopWidthUnit.Tag = "border-top-width|unit";
            this.comboBoxBorderTopWidthUnit.ValueMember = "Value";
            this.comboBoxBorderTopWidthUnit.SelectionChangeCommitted += new System.EventHandler(this.comboBox_SelectedIndexChanged);
            // 
            // comboBoxPaddingTopUnit
            // 
            this.comboBoxPaddingTopUnit.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxPaddingTopUnit.DisplayMember = "Member";
            this.comboBoxPaddingTopUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxPaddingTopUnit.Location = new System.Drawing.Point(72, 160);
            this.comboBoxPaddingTopUnit.Name = "comboBoxPaddingTopUnit";
            this.comboBoxPaddingTopUnit.Size = new System.Drawing.Size(109, 21);
            this.comboBoxPaddingTopUnit.TabIndex = 2;
            this.comboBoxPaddingTopUnit.Tag = "padding-top|unit";
            this.comboBoxPaddingTopUnit.ValueMember = "Value";
            this.comboBoxPaddingTopUnit.SelectionChangeCommitted += new System.EventHandler(this.comboBox_SelectedIndexChanged);
            // 
            // comboBoxMarginTopUnit
            // 
            this.comboBoxMarginTopUnit.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxMarginTopUnit.DisplayMember = "Member";
            this.comboBoxMarginTopUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxMarginTopUnit.Location = new System.Drawing.Point(72, 8);
            this.comboBoxMarginTopUnit.Name = "comboBoxMarginTopUnit";
            this.comboBoxMarginTopUnit.Size = new System.Drawing.Size(109, 21);
            this.comboBoxMarginTopUnit.TabIndex = 0;
            this.comboBoxMarginTopUnit.Tag = "margin-top|unit";
            this.comboBoxMarginTopUnit.ValueMember = "Value";
            this.comboBoxMarginTopUnit.SelectionChangeCommitted += new System.EventHandler(this.comboBox_SelectedIndexChanged);
            // 
            // numericUpDownPaddingTop
            // 
            this.numericUpDownPaddingTop.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numericUpDownPaddingTop.Enabled = false;
            this.numericUpDownPaddingTop.Location = new System.Drawing.Point(8, 160);
            this.numericUpDownPaddingTop.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.numericUpDownPaddingTop.Minimum = new decimal(new int[] {
            9999,
            0,
            0,
            -2147483648});
            this.numericUpDownPaddingTop.Name = "numericUpDownPaddingTop";
            this.numericUpDownPaddingTop.Size = new System.Drawing.Size(64, 20);
            this.numericUpDownPaddingTop.TabIndex = 3;
            this.numericUpDownPaddingTop.Tag = "padding-top|value";
            this.numericUpDownPaddingTop.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            this.numericUpDownPaddingTop.KeyDown += new System.Windows.Forms.KeyEventHandler(this.numericUpDown_KeyDown);
            // 
            // numericUpDownMarginTop
            // 
            this.numericUpDownMarginTop.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numericUpDownMarginTop.Enabled = false;
            this.numericUpDownMarginTop.Location = new System.Drawing.Point(8, 8);
            this.numericUpDownMarginTop.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.numericUpDownMarginTop.Minimum = new decimal(new int[] {
            9999,
            0,
            0,
            -2147483648});
            this.numericUpDownMarginTop.Name = "numericUpDownMarginTop";
            this.numericUpDownMarginTop.Size = new System.Drawing.Size(64, 20);
            this.numericUpDownMarginTop.TabIndex = 1;
            this.numericUpDownMarginTop.Tag = "margin-top|value";
            this.numericUpDownMarginTop.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            this.numericUpDownMarginTop.KeyDown += new System.Windows.Forms.KeyEventHandler(this.numericUpDown_KeyDown);
            // 
            // tabPageBottom
            // 
            this.tabPageBottom.Controls.Add(this.groupBox12);
            this.tabPageBottom.Controls.Add(this.numericUpDownPaddingBottom);
            this.tabPageBottom.Controls.Add(this.numericUpDownMarginBottom);
            this.tabPageBottom.Controls.Add(this.comboBoxMarginBottomUnit);
            this.tabPageBottom.Controls.Add(this.comboBoxPaddingBottomUnit);
            this.tabPageBottom.ImageIndex = 4;
            this.tabPageBottom.Location = new System.Drawing.Point(4, 4);
            this.tabPageBottom.Name = "tabPageBottom";
            this.tabPageBottom.Size = new System.Drawing.Size(275, 222);
            this.tabPageBottom.TabIndex = 4;
            this.tabPageBottom.Text = "Bottom";
            // 
            // groupBox12
            // 
            this.groupBox12.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox12.Controls.Add(this.comboBoxBorderBottomWidthUnit);
            this.groupBox12.Controls.Add(this.comboBoxBorderBottomStyle);
            this.groupBox12.Controls.Add(this.numericUpDownBorderBottomWidth);
            this.groupBox12.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox12.Location = new System.Drawing.Point(8, 40);
            this.groupBox12.Name = "groupBox12";
            this.groupBox12.Size = new System.Drawing.Size(245, 112);
            this.groupBox12.TabIndex = 109;
            this.groupBox12.TabStop = false;
            this.groupBox12.Text = "Bottom Border";
            // 
            // comboBoxBorderBottomWidthUnit
            // 
            this.comboBoxBorderBottomWidthUnit.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxBorderBottomWidthUnit.DisplayMember = "Member";
            this.comboBoxBorderBottomWidthUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxBorderBottomWidthUnit.Location = new System.Drawing.Point(80, 72);
            this.comboBoxBorderBottomWidthUnit.Name = "comboBoxBorderBottomWidthUnit";
            this.comboBoxBorderBottomWidthUnit.Size = new System.Drawing.Size(141, 21);
            this.comboBoxBorderBottomWidthUnit.TabIndex = 2;
            this.comboBoxBorderBottomWidthUnit.Tag = "border-bottom-width|unit";
            this.comboBoxBorderBottomWidthUnit.ValueMember = "Value";
            this.comboBoxBorderBottomWidthUnit.SelectionChangeCommitted += new System.EventHandler(this.comboBox_SelectedIndexChanged);
            // 
            // comboBoxBorderBottomStyle
            // 
            this.comboBoxBorderBottomStyle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxBorderBottomStyle.DisplayMember = "Member";
            this.comboBoxBorderBottomStyle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxBorderBottomStyle.Location = new System.Drawing.Point(16, 48);
            this.comboBoxBorderBottomStyle.Name = "comboBoxBorderBottomStyle";
            this.comboBoxBorderBottomStyle.Size = new System.Drawing.Size(205, 21);
            this.comboBoxBorderBottomStyle.TabIndex = 1;
            this.comboBoxBorderBottomStyle.Tag = "border-bottom-style";
            this.comboBoxBorderBottomStyle.ValueMember = "Value";
            this.comboBoxBorderBottomStyle.SelectionChangeCommitted += new System.EventHandler(this.comboBox_SelectedIndexChanged);
            // 
            // numericUpDownBorderBottomWidth
            // 
            this.numericUpDownBorderBottomWidth.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numericUpDownBorderBottomWidth.Enabled = false;
            this.numericUpDownBorderBottomWidth.Location = new System.Drawing.Point(16, 72);
            this.numericUpDownBorderBottomWidth.Name = "numericUpDownBorderBottomWidth";
            this.numericUpDownBorderBottomWidth.Size = new System.Drawing.Size(64, 20);
            this.numericUpDownBorderBottomWidth.TabIndex = 3;
            this.numericUpDownBorderBottomWidth.Tag = "border-bottom-width|value";
            this.numericUpDownBorderBottomWidth.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            this.numericUpDownBorderBottomWidth.KeyDown += new System.Windows.Forms.KeyEventHandler(this.numericUpDown_KeyDown);
            // 
            // numericUpDownPaddingBottom
            // 
            this.numericUpDownPaddingBottom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numericUpDownPaddingBottom.Enabled = false;
            this.numericUpDownPaddingBottom.Location = new System.Drawing.Point(8, 160);
            this.numericUpDownPaddingBottom.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.numericUpDownPaddingBottom.Minimum = new decimal(new int[] {
            9999,
            0,
            0,
            -2147483648});
            this.numericUpDownPaddingBottom.Name = "numericUpDownPaddingBottom";
            this.numericUpDownPaddingBottom.Size = new System.Drawing.Size(64, 20);
            this.numericUpDownPaddingBottom.TabIndex = 3;
            this.numericUpDownPaddingBottom.Tag = "padding-bottom|value";
            this.numericUpDownPaddingBottom.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            this.numericUpDownPaddingBottom.KeyDown += new System.Windows.Forms.KeyEventHandler(this.numericUpDown_KeyDown);
            // 
            // numericUpDownMarginBottom
            // 
            this.numericUpDownMarginBottom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numericUpDownMarginBottom.Enabled = false;
            this.numericUpDownMarginBottom.Location = new System.Drawing.Point(8, 8);
            this.numericUpDownMarginBottom.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.numericUpDownMarginBottom.Minimum = new decimal(new int[] {
            9999,
            0,
            0,
            -2147483648});
            this.numericUpDownMarginBottom.Name = "numericUpDownMarginBottom";
            this.numericUpDownMarginBottom.Size = new System.Drawing.Size(64, 20);
            this.numericUpDownMarginBottom.TabIndex = 1;
            this.numericUpDownMarginBottom.Tag = "margin-bottom|value";
            this.numericUpDownMarginBottom.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            this.numericUpDownMarginBottom.KeyDown += new System.Windows.Forms.KeyEventHandler(this.numericUpDown_KeyDown);
            // 
            // comboBoxMarginBottomUnit
            // 
            this.comboBoxMarginBottomUnit.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxMarginBottomUnit.DisplayMember = "Member";
            this.comboBoxMarginBottomUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxMarginBottomUnit.Location = new System.Drawing.Point(72, 8);
            this.comboBoxMarginBottomUnit.Name = "comboBoxMarginBottomUnit";
            this.comboBoxMarginBottomUnit.Size = new System.Drawing.Size(109, 21);
            this.comboBoxMarginBottomUnit.TabIndex = 0;
            this.comboBoxMarginBottomUnit.Tag = "margin-bottom|unit";
            this.comboBoxMarginBottomUnit.ValueMember = "Value";
            this.comboBoxMarginBottomUnit.SelectionChangeCommitted += new System.EventHandler(this.comboBox_SelectedIndexChanged);
            // 
            // comboBoxPaddingBottomUnit
            // 
            this.comboBoxPaddingBottomUnit.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxPaddingBottomUnit.DisplayMember = "Member";
            this.comboBoxPaddingBottomUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxPaddingBottomUnit.Location = new System.Drawing.Point(72, 160);
            this.comboBoxPaddingBottomUnit.Name = "comboBoxPaddingBottomUnit";
            this.comboBoxPaddingBottomUnit.Size = new System.Drawing.Size(109, 21);
            this.comboBoxPaddingBottomUnit.TabIndex = 2;
            this.comboBoxPaddingBottomUnit.Tag = "padding-bottom|unit";
            this.comboBoxPaddingBottomUnit.ValueMember = "Value";
            this.comboBoxPaddingBottomUnit.SelectionChangeCommitted += new System.EventHandler(this.comboBox_SelectedIndexChanged);
            // 
            // imageListBorderTabs
            // 
            this.imageListBorderTabs.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListBorderTabs.ImageStream")));
            this.imageListBorderTabs.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListBorderTabs.Images.SetKeyName(0, "");
            this.imageListBorderTabs.Images.SetKeyName(1, "");
            this.imageListBorderTabs.Images.SetKeyName(2, "");
            this.imageListBorderTabs.Images.SetKeyName(3, "");
            this.imageListBorderTabs.Images.SetKeyName(4, "");
            this.imageListBorderTabs.Images.SetKeyName(5, "");
            this.imageListBorderTabs.Images.SetKeyName(6, "");
            this.imageListBorderTabs.Images.SetKeyName(7, "");
            this.imageListBorderTabs.Images.SetKeyName(8, "");
            this.imageListBorderTabs.Images.SetKeyName(9, "");
            this.imageListBorderTabs.Images.SetKeyName(10, "");
            this.imageListBorderTabs.Images.SetKeyName(11, "");
            // 
            // label56
            // 
            this.label56.Location = new System.Drawing.Point(5, 182);
            this.label56.Name = "label56";
            this.label56.Size = new System.Drawing.Size(119, 16);
            this.label56.TabIndex = 83;
            this.label56.Text = "Padding";
            this.label56.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label55
            // 
            this.label55.Location = new System.Drawing.Point(5, 134);
            this.label55.Name = "label55";
            this.label55.Size = new System.Drawing.Size(119, 16);
            this.label55.TabIndex = 82;
            this.label55.Text = "Border Width";
            this.label55.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.BackColor = System.Drawing.SystemColors.Info;
            this.button1.Location = new System.Drawing.Point(134, 278);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(270, 24);
            this.button1.TabIndex = 4;
            this.button1.Text = "Synchronize left, right, and bottom values with top. ";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label54
            // 
            this.label54.Location = new System.Drawing.Point(5, 111);
            this.label54.Name = "label54";
            this.label54.Size = new System.Drawing.Size(119, 16);
            this.label54.TabIndex = 81;
            this.label54.Text = "Border Style";
            this.label54.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label53
            // 
            this.label53.Location = new System.Drawing.Point(3, 86);
            this.label53.Name = "label53";
            this.label53.Size = new System.Drawing.Size(121, 16);
            this.label53.TabIndex = 80;
            this.label53.Text = "Border Color";
            this.label53.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label17
            // 
            this.label17.Location = new System.Drawing.Point(5, 29);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(119, 16);
            this.label17.TabIndex = 45;
            this.label17.Text = "Margin";
            this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // groupBox21
            // 
            this.groupBox21.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox21.Controls.Add(this.label67);
            this.groupBox21.Controls.Add(this.label28);
            this.groupBox21.Controls.Add(this.comboBoxTextDecoration);
            this.groupBox21.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox21.Location = new System.Drawing.Point(8, 267);
            this.groupBox21.Name = "groupBox21";
            this.groupBox21.Size = new System.Drawing.Size(202, 126);
            this.groupBox21.TabIndex = 0;
            this.groupBox21.TabStop = false;
            this.groupBox21.Text = "Text Decoration";
            // 
            // label67
            // 
            this.label67.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label67.BackColor = System.Drawing.SystemColors.Info;
            this.label67.Cursor = System.Windows.Forms.Cursors.Help;
            this.label67.ForeColor = System.Drawing.SystemColors.InfoText;
            this.label67.Location = new System.Drawing.Point(14, 53);
            this.label67.Name = "label67";
            this.label67.Size = new System.Drawing.Size(170, 56);
            this.label67.TabIndex = 1;
            this.label67.Text = "Determines how text is displayed in addition to font characteristics.";
            // 
            // label28
            // 
            this.label28.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label28.BackColor = System.Drawing.SystemColors.GrayText;
            this.label28.Location = new System.Drawing.Point(17, 56);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(170, 56);
            this.label28.TabIndex = 13;
            // 
            // comboBoxTextDecoration
            // 
            this.comboBoxTextDecoration.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxTextDecoration.DisplayMember = "Member";
            this.comboBoxTextDecoration.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxTextDecoration.Location = new System.Drawing.Point(16, 29);
            this.comboBoxTextDecoration.Name = "comboBoxTextDecoration";
            this.comboBoxTextDecoration.Size = new System.Drawing.Size(170, 21);
            this.comboBoxTextDecoration.TabIndex = 0;
            this.comboBoxTextDecoration.Tag = "text-decoration";
            this.comboBoxTextDecoration.ValueMember = "Value";
            this.comboBoxTextDecoration.SelectionChangeCommitted += new System.EventHandler(this.comboBox_SelectedIndexChanged);
            // 
            // groupBox8
            // 
            this.groupBox8.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox8.Controls.Add(this.pictureBox6);
            this.groupBox8.Controls.Add(this.pictureBox5);
            this.groupBox8.Controls.Add(this.label29);
            this.groupBox8.Controls.Add(this.radioButtonDirectionRemove);
            this.groupBox8.Controls.Add(this.radioButtonDirectionRtl);
            this.groupBox8.Controls.Add(this.radioButtonDirectionLtr);
            this.groupBox8.Controls.Add(this.label33);
            this.groupBox8.Controls.Add(this.label34);
            this.groupBox8.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox8.Location = new System.Drawing.Point(222, 267);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(202, 126);
            this.groupBox8.TabIndex = 0;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "Direction";
            // 
            // pictureBox6
            // 
            this.pictureBox6.Cursor = System.Windows.Forms.Cursors.Help;
            this.pictureBox6.Location = new System.Drawing.Point(124, 96);
            this.pictureBox6.Name = "pictureBox6";
            this.pictureBox6.Size = new System.Drawing.Size(48, 12);
            this.pictureBox6.TabIndex = 33;
            this.pictureBox6.TabStop = false;
            // 
            // pictureBox5
            // 
            this.pictureBox5.Cursor = System.Windows.Forms.Cursors.Help;
            this.pictureBox5.Location = new System.Drawing.Point(124, 72);
            this.pictureBox5.Name = "pictureBox5";
            this.pictureBox5.Size = new System.Drawing.Size(48, 12);
            this.pictureBox5.TabIndex = 32;
            this.pictureBox5.TabStop = false;
            // 
            // label29
            // 
            this.label29.Location = new System.Drawing.Point(8, 28);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(185, 17);
            this.label29.TabIndex = 0;
            this.label29.Text = "Direction:";
            // 
            // radioButtonDirectionRemove
            // 
            this.radioButtonDirectionRemove.Checked = true;
            this.radioButtonDirectionRemove.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.radioButtonDirectionRemove.Location = new System.Drawing.Point(8, 48);
            this.radioButtonDirectionRemove.Name = "radioButtonDirectionRemove";
            this.radioButtonDirectionRemove.Size = new System.Drawing.Size(116, 18);
            this.radioButtonDirectionRemove.TabIndex = 0;
            this.radioButtonDirectionRemove.TabStop = true;
            this.radioButtonDirectionRemove.Tag = "direction|remove";
            this.radioButtonDirectionRemove.Text = "Remove";
            this.radioButtonDirectionRemove.CheckedChanged += new System.EventHandler(this.checkBox_CheckedChanged);
            // 
            // radioButtonDirectionRtl
            // 
            this.radioButtonDirectionRtl.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.radioButtonDirectionRtl.Location = new System.Drawing.Point(8, 96);
            this.radioButtonDirectionRtl.Name = "radioButtonDirectionRtl";
            this.radioButtonDirectionRtl.Size = new System.Drawing.Size(116, 16);
            this.radioButtonDirectionRtl.TabIndex = 2;
            this.radioButtonDirectionRtl.Tag = "direction|param|rtl";
            this.radioButtonDirectionRtl.Text = "right to left";
            this.radioButtonDirectionRtl.CheckedChanged += new System.EventHandler(this.checkBox_CheckedChanged);
            // 
            // radioButtonDirectionLtr
            // 
            this.radioButtonDirectionLtr.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.radioButtonDirectionLtr.Location = new System.Drawing.Point(8, 72);
            this.radioButtonDirectionLtr.Name = "radioButtonDirectionLtr";
            this.radioButtonDirectionLtr.Size = new System.Drawing.Size(116, 16);
            this.radioButtonDirectionLtr.TabIndex = 1;
            this.radioButtonDirectionLtr.Tag = "direction|param|ltr";
            this.radioButtonDirectionLtr.Text = "left to right";
            this.radioButtonDirectionLtr.CheckedChanged += new System.EventHandler(this.checkBox_CheckedChanged);
            // 
            // label33
            // 
            this.label33.BackColor = System.Drawing.SystemColors.GrayText;
            this.label33.Location = new System.Drawing.Point(126, 74);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(49, 13);
            this.label33.TabIndex = 2;
            // 
            // label34
            // 
            this.label34.BackColor = System.Drawing.SystemColors.GrayText;
            this.label34.Location = new System.Drawing.Point(126, 98);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(50, 13);
            this.label34.TabIndex = 4;
            // 
            // numericUpDownTextIndent
            // 
            this.numericUpDownTextIndent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numericUpDownTextIndent.Enabled = false;
            this.numericUpDownTextIndent.Location = new System.Drawing.Point(254, 33);
            this.numericUpDownTextIndent.Maximum = new decimal(new int[] {
            1024,
            0,
            0,
            0});
            this.numericUpDownTextIndent.Minimum = new decimal(new int[] {
            1024,
            0,
            0,
            -2147483648});
            this.numericUpDownTextIndent.Name = "numericUpDownTextIndent";
            this.numericUpDownTextIndent.Size = new System.Drawing.Size(58, 20);
            this.numericUpDownTextIndent.TabIndex = 4;
            this.numericUpDownTextIndent.Tag = "text-indent|value";
            this.numericUpDownTextIndent.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            this.numericUpDownTextIndent.KeyDown += new System.Windows.Forms.KeyEventHandler(this.numericUpDown_KeyDown);
            // 
            // label68
            // 
            this.label68.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label68.BackColor = System.Drawing.SystemColors.Info;
            this.label68.Cursor = System.Windows.Forms.Cursors.Help;
            this.label68.ForeColor = System.Drawing.SystemColors.InfoText;
            this.label68.Location = new System.Drawing.Point(254, 62);
            this.label68.Name = "label68";
            this.label68.Size = new System.Drawing.Size(153, 57);
            this.label68.TabIndex = 32;
            this.label68.Text = "Determine how text inside a block is aligned";
            // 
            // label51
            // 
            this.label51.Location = new System.Drawing.Point(251, 16);
            this.label51.Name = "label51";
            this.label51.Size = new System.Drawing.Size(144, 16);
            this.label51.TabIndex = 29;
            this.label51.Text = "Block Indentation:";
            // 
            // comboBoxTextIdentUnit
            // 
            this.comboBoxTextIdentUnit.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxTextIdentUnit.DisplayMember = "Member";
            this.comboBoxTextIdentUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxTextIdentUnit.Location = new System.Drawing.Point(312, 33);
            this.comboBoxTextIdentUnit.Name = "comboBoxTextIdentUnit";
            this.comboBoxTextIdentUnit.Size = new System.Drawing.Size(96, 21);
            this.comboBoxTextIdentUnit.TabIndex = 3;
            this.comboBoxTextIdentUnit.Tag = "text-indent|unit";
            this.comboBoxTextIdentUnit.ValueMember = "Value";
            this.comboBoxTextIdentUnit.SelectionChangeCommitted += new System.EventHandler(this.comboBox_SelectedIndexChanged);
            // 
            // groupBox7
            // 
            this.groupBox7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox7.Controls.Add(this.label66);
            this.groupBox7.Controls.Add(this.label20);
            this.groupBox7.Controls.Add(this.numericUpDownWordSpacing);
            this.groupBox7.Controls.Add(this.numericUpDownLetterSpacing);
            this.groupBox7.Controls.Add(this.numericUpDownLineHeight);
            this.groupBox7.Controls.Add(this.label37);
            this.groupBox7.Controls.Add(this.comboBoxWordSpacingUnit);
            this.groupBox7.Controls.Add(this.label11);
            this.groupBox7.Controls.Add(this.label12);
            this.groupBox7.Controls.Add(this.comboBoxLineHeightUnit);
            this.groupBox7.Controls.Add(this.comboBoxLetterSpacingUnit);
            this.groupBox7.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox7.Location = new System.Drawing.Point(8, 145);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(416, 112);
            this.groupBox7.TabIndex = 0;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Line and Character Space";
            // 
            // label66
            // 
            this.label66.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label66.BackColor = System.Drawing.SystemColors.Info;
            this.label66.Cursor = System.Windows.Forms.Cursors.Help;
            this.label66.ForeColor = System.Drawing.SystemColors.InfoText;
            this.label66.Location = new System.Drawing.Point(255, 22);
            this.label66.Name = "label66";
            this.label66.Size = new System.Drawing.Size(152, 80);
            this.label66.TabIndex = 30;
            this.label66.Text = "These options determine how floating text is displayed. The result depends on the" +
                " coosen font.";
            // 
            // label20
            // 
            this.label20.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label20.BackColor = System.Drawing.SystemColors.GrayText;
            this.label20.Location = new System.Drawing.Point(258, 25);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(152, 80);
            this.label20.TabIndex = 31;
            // 
            // numericUpDownWordSpacing
            // 
            this.numericUpDownWordSpacing.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numericUpDownWordSpacing.Enabled = false;
            this.numericUpDownWordSpacing.Location = new System.Drawing.Point(120, 24);
            this.numericUpDownWordSpacing.Name = "numericUpDownWordSpacing";
            this.numericUpDownWordSpacing.Size = new System.Drawing.Size(56, 20);
            this.numericUpDownWordSpacing.TabIndex = 5;
            this.numericUpDownWordSpacing.Tag = "word-spacing|value";
            this.numericUpDownWordSpacing.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            this.numericUpDownWordSpacing.KeyDown += new System.Windows.Forms.KeyEventHandler(this.numericUpDown_KeyDown);
            // 
            // numericUpDownLetterSpacing
            // 
            this.numericUpDownLetterSpacing.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numericUpDownLetterSpacing.Enabled = false;
            this.numericUpDownLetterSpacing.Location = new System.Drawing.Point(120, 48);
            this.numericUpDownLetterSpacing.Name = "numericUpDownLetterSpacing";
            this.numericUpDownLetterSpacing.Size = new System.Drawing.Size(56, 20);
            this.numericUpDownLetterSpacing.TabIndex = 3;
            this.numericUpDownLetterSpacing.Tag = "letter-spacing|value";
            this.numericUpDownLetterSpacing.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            this.numericUpDownLetterSpacing.KeyDown += new System.Windows.Forms.KeyEventHandler(this.numericUpDown_KeyDown);
            // 
            // numericUpDownLineHeight
            // 
            this.numericUpDownLineHeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numericUpDownLineHeight.Enabled = false;
            this.numericUpDownLineHeight.Location = new System.Drawing.Point(120, 72);
            this.numericUpDownLineHeight.Name = "numericUpDownLineHeight";
            this.numericUpDownLineHeight.Size = new System.Drawing.Size(56, 20);
            this.numericUpDownLineHeight.TabIndex = 1;
            this.numericUpDownLineHeight.Tag = "line-height|value";
            this.numericUpDownLineHeight.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            this.numericUpDownLineHeight.KeyDown += new System.Windows.Forms.KeyEventHandler(this.numericUpDown_KeyDown);
            // 
            // label37
            // 
            this.label37.Location = new System.Drawing.Point(8, 24);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(112, 16);
            this.label37.TabIndex = 26;
            this.label37.Text = "Words:";
            this.label37.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboBoxWordSpacingUnit
            // 
            this.comboBoxWordSpacingUnit.DisplayMember = "Member";
            this.comboBoxWordSpacingUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxWordSpacingUnit.Location = new System.Drawing.Point(176, 24);
            this.comboBoxWordSpacingUnit.Name = "comboBoxWordSpacingUnit";
            this.comboBoxWordSpacingUnit.Size = new System.Drawing.Size(72, 21);
            this.comboBoxWordSpacingUnit.TabIndex = 0;
            this.comboBoxWordSpacingUnit.Tag = "word-spacing|unit";
            this.comboBoxWordSpacingUnit.ValueMember = "Value";
            this.comboBoxWordSpacingUnit.SelectionChangeCommitted += new System.EventHandler(this.comboBox_SelectedIndexChanged);
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(8, 72);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(112, 16);
            this.label11.TabIndex = 21;
            this.label11.Text = "Line Height:";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label12
            // 
            this.label12.Location = new System.Drawing.Point(8, 48);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(112, 16);
            this.label12.TabIndex = 20;
            this.label12.Text = "Characters:";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboBoxLineHeightUnit
            // 
            this.comboBoxLineHeightUnit.DisplayMember = "Member";
            this.comboBoxLineHeightUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxLineHeightUnit.Location = new System.Drawing.Point(176, 72);
            this.comboBoxLineHeightUnit.Name = "comboBoxLineHeightUnit";
            this.comboBoxLineHeightUnit.Size = new System.Drawing.Size(72, 21);
            this.comboBoxLineHeightUnit.TabIndex = 4;
            this.comboBoxLineHeightUnit.Tag = "line-height|unit";
            this.comboBoxLineHeightUnit.ValueMember = "Value";
            this.comboBoxLineHeightUnit.SelectionChangeCommitted += new System.EventHandler(this.comboBox_SelectedIndexChanged);
            // 
            // comboBoxLetterSpacingUnit
            // 
            this.comboBoxLetterSpacingUnit.DisplayMember = "Member";
            this.comboBoxLetterSpacingUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxLetterSpacingUnit.Location = new System.Drawing.Point(176, 48);
            this.comboBoxLetterSpacingUnit.Name = "comboBoxLetterSpacingUnit";
            this.comboBoxLetterSpacingUnit.Size = new System.Drawing.Size(72, 21);
            this.comboBoxLetterSpacingUnit.TabIndex = 2;
            this.comboBoxLetterSpacingUnit.Tag = "letter-spacing|unit";
            this.comboBoxLetterSpacingUnit.ValueMember = "Value";
            this.comboBoxLetterSpacingUnit.SelectionChangeCommitted += new System.EventHandler(this.comboBox_SelectedIndexChanged);
            // 
            // groupBox6
            // 
            this.groupBox6.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox6.Controls.Add(this.groupBox19);
            this.groupBox6.Controls.Add(this.groupBox20);
            this.groupBox6.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox6.Location = new System.Drawing.Point(8, 8);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(416, 177);
            this.groupBox6.TabIndex = 0;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Alignment";
            // 
            // groupBox19
            // 
            this.groupBox19.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox19.Controls.Add(this.pictureBox1);
            this.groupBox19.Controls.Add(this.pictureBox4);
            this.groupBox19.Controls.Add(this.pictureBox3);
            this.groupBox19.Controls.Add(this.pictureBox2);
            this.groupBox19.Controls.Add(this.radioButtonTextAlignDisable);
            this.groupBox19.Controls.Add(this.radioButtonTextAlignJustify);
            this.groupBox19.Controls.Add(this.radioButtonTextAlignCenter);
            this.groupBox19.Controls.Add(this.radioButtonTextAlignRight);
            this.groupBox19.Controls.Add(this.label10);
            this.groupBox19.Controls.Add(this.radioButtonTextAlignLeft);
            this.groupBox19.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox19.Location = new System.Drawing.Point(208, 16);
            this.groupBox19.Name = "groupBox19";
            this.groupBox19.Size = new System.Drawing.Size(200, 145);
            this.groupBox19.TabIndex = 0;
            this.groupBox19.TabStop = false;
            this.groupBox19.Text = "Horizontal Alignment";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(51, 72);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(16, 16);
            this.pictureBox1.TabIndex = 12;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox4
            // 
            this.pictureBox4.Location = new System.Drawing.Point(51, 104);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(16, 16);
            this.pictureBox4.TabIndex = 15;
            this.pictureBox4.TabStop = false;
            // 
            // pictureBox3
            // 
            this.pictureBox3.Location = new System.Drawing.Point(127, 72);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(16, 16);
            this.pictureBox3.TabIndex = 14;
            this.pictureBox3.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Location = new System.Drawing.Point(89, 72);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(16, 16);
            this.pictureBox2.TabIndex = 13;
            this.pictureBox2.TabStop = false;
            // 
            // radioButtonTextAlignDisable
            // 
            this.radioButtonTextAlignDisable.Checked = true;
            this.radioButtonTextAlignDisable.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.radioButtonTextAlignDisable.Location = new System.Drawing.Point(24, 16);
            this.radioButtonTextAlignDisable.Name = "radioButtonTextAlignDisable";
            this.radioButtonTextAlignDisable.Size = new System.Drawing.Size(160, 16);
            this.radioButtonTextAlignDisable.TabIndex = 2;
            this.radioButtonTextAlignDisable.TabStop = true;
            this.radioButtonTextAlignDisable.Tag = "text-align|remove";
            this.radioButtonTextAlignDisable.Text = "Remove Option from Sheet";
            this.radioButtonTextAlignDisable.CheckedChanged += new System.EventHandler(this.checkBox_CheckedChanged);
            // 
            // radioButtonTextAlignJustify
            // 
            this.radioButtonTextAlignJustify.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.radioButtonTextAlignJustify.Location = new System.Drawing.Point(76, 104);
            this.radioButtonTextAlignJustify.Name = "radioButtonTextAlignJustify";
            this.radioButtonTextAlignJustify.Size = new System.Drawing.Size(104, 16);
            this.radioButtonTextAlignJustify.TabIndex = 3;
            this.radioButtonTextAlignJustify.Tag = "text-align|param|justify";
            this.radioButtonTextAlignJustify.Text = "Justify";
            this.radioButtonTextAlignJustify.CheckedChanged += new System.EventHandler(this.checkBox_CheckedChanged);
            // 
            // radioButtonTextAlignCenter
            // 
            this.radioButtonTextAlignCenter.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.radioButtonTextAlignCenter.Location = new System.Drawing.Point(91, 55);
            this.radioButtonTextAlignCenter.Name = "radioButtonTextAlignCenter";
            this.radioButtonTextAlignCenter.Size = new System.Drawing.Size(16, 14);
            this.radioButtonTextAlignCenter.TabIndex = 1;
            this.radioButtonTextAlignCenter.Tag = "text-align|param|center";
            this.radioButtonTextAlignCenter.CheckedChanged += new System.EventHandler(this.checkBox_CheckedChanged);
            // 
            // radioButtonTextAlignRight
            // 
            this.radioButtonTextAlignRight.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.radioButtonTextAlignRight.Location = new System.Drawing.Point(128, 55);
            this.radioButtonTextAlignRight.Name = "radioButtonTextAlignRight";
            this.radioButtonTextAlignRight.Size = new System.Drawing.Size(64, 16);
            this.radioButtonTextAlignRight.TabIndex = 2;
            this.radioButtonTextAlignRight.Tag = "text-align|param|right";
            this.radioButtonTextAlignRight.Text = "Right";
            this.radioButtonTextAlignRight.CheckedChanged += new System.EventHandler(this.checkBox_CheckedChanged);
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(80, 40);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(48, 16);
            this.label10.TabIndex = 0;
            this.label10.Text = "Center";
            // 
            // radioButtonTextAlignLeft
            // 
            this.radioButtonTextAlignLeft.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.radioButtonTextAlignLeft.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.radioButtonTextAlignLeft.Location = new System.Drawing.Point(8, 54);
            this.radioButtonTextAlignLeft.Name = "radioButtonTextAlignLeft";
            this.radioButtonTextAlignLeft.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.radioButtonTextAlignLeft.Size = new System.Drawing.Size(56, 43);
            this.radioButtonTextAlignLeft.TabIndex = 0;
            this.radioButtonTextAlignLeft.Tag = "text-align|param|left";
            this.radioButtonTextAlignLeft.Text = "Left";
            this.radioButtonTextAlignLeft.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.radioButtonTextAlignLeft.CheckedChanged += new System.EventHandler(this.checkBox_CheckedChanged);
            // 
            // groupBox20
            // 
            this.groupBox20.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox20.Controls.Add(this.label65);
            this.groupBox20.Controls.Add(this.label30);
            this.groupBox20.Controls.Add(this.comboBoxStyleVerticalAlignment);
            this.groupBox20.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox20.Location = new System.Drawing.Point(8, 16);
            this.groupBox20.Name = "groupBox20";
            this.groupBox20.Size = new System.Drawing.Size(192, 145);
            this.groupBox20.TabIndex = 0;
            this.groupBox20.TabStop = false;
            this.groupBox20.Text = "Vertical Alignment";
            // 
            // label65
            // 
            this.label65.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label65.BackColor = System.Drawing.SystemColors.Info;
            this.label65.Cursor = System.Windows.Forms.Cursors.Help;
            this.label65.ForeColor = System.Drawing.SystemColors.InfoText;
            this.label65.Location = new System.Drawing.Point(16, 55);
            this.label65.Name = "label65";
            this.label65.Size = new System.Drawing.Size(168, 81);
            this.label65.TabIndex = 1;
            this.label65.Text = "Determine how the element is align vertically. This is commonly used in tables an" +
                "d boxes.";
            // 
            // label30
            // 
            this.label30.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label30.BackColor = System.Drawing.SystemColors.GrayText;
            this.label30.Location = new System.Drawing.Point(19, 58);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(168, 81);
            this.label30.TabIndex = 2;
            // 
            // comboBoxStyleVerticalAlignment
            // 
            this.comboBoxStyleVerticalAlignment.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxStyleVerticalAlignment.DisplayMember = "Member";
            this.comboBoxStyleVerticalAlignment.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxStyleVerticalAlignment.Location = new System.Drawing.Point(16, 24);
            this.comboBoxStyleVerticalAlignment.Name = "comboBoxStyleVerticalAlignment";
            this.comboBoxStyleVerticalAlignment.Size = new System.Drawing.Size(168, 21);
            this.comboBoxStyleVerticalAlignment.TabIndex = 0;
            this.comboBoxStyleVerticalAlignment.Tag = "vertical-align";
            this.comboBoxStyleVerticalAlignment.ValueMember = "Value";
            this.comboBoxStyleVerticalAlignment.SelectionChangeCommitted += new System.EventHandler(this.comboBox_SelectedIndexChanged);
            // 
            // groupBox10
            // 
            this.groupBox10.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox10.Controls.Add(this.label68);
            this.groupBox10.Controls.Add(this.label19);
            this.groupBox10.Controls.Add(this.comboBoxStyleLineBreak);
            this.groupBox10.Controls.Add(this.label48);
            this.groupBox10.Controls.Add(this.comboBoxStyleOverFlow);
            this.groupBox10.Controls.Add(this.label47);
            this.groupBox10.Controls.Add(this.comboBoxDisplay);
            this.groupBox10.Controls.Add(this.label45);
            this.groupBox10.Controls.Add(this.label51);
            this.groupBox10.Controls.Add(this.comboBoxTextIdentUnit);
            this.groupBox10.Controls.Add(this.numericUpDownTextIndent);
            this.groupBox10.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox10.Location = new System.Drawing.Point(8, 8);
            this.groupBox10.Name = "groupBox10";
            this.groupBox10.Size = new System.Drawing.Size(416, 128);
            this.groupBox10.TabIndex = 0;
            this.groupBox10.TabStop = false;
            this.groupBox10.Text = "Text Floating Options";
            // 
            // label19
            // 
            this.label19.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label19.BackColor = System.Drawing.SystemColors.GrayText;
            this.label19.Location = new System.Drawing.Point(258, 65);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(152, 57);
            this.label19.TabIndex = 33;
            // 
            // comboBoxStyleLineBreak
            // 
            this.comboBoxStyleLineBreak.DisplayMember = "Member";
            this.comboBoxStyleLineBreak.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxStyleLineBreak.Location = new System.Drawing.Point(120, 93);
            this.comboBoxStyleLineBreak.Name = "comboBoxStyleLineBreak";
            this.comboBoxStyleLineBreak.Size = new System.Drawing.Size(121, 21);
            this.comboBoxStyleLineBreak.TabIndex = 2;
            this.comboBoxStyleLineBreak.Tag = "white-space";
            this.comboBoxStyleLineBreak.ValueMember = "Value";
            this.comboBoxStyleLineBreak.SelectionChangeCommitted += new System.EventHandler(this.comboBox_SelectedIndexChanged);
            // 
            // label48
            // 
            this.label48.Location = new System.Drawing.Point(10, 94);
            this.label48.Name = "label48";
            this.label48.Size = new System.Drawing.Size(112, 28);
            this.label48.TabIndex = 8;
            this.label48.Text = "Linebreak Handling";
            this.label48.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // comboBoxStyleOverFlow
            // 
            this.comboBoxStyleOverFlow.DisplayMember = "Member";
            this.comboBoxStyleOverFlow.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxStyleOverFlow.Location = new System.Drawing.Point(120, 61);
            this.comboBoxStyleOverFlow.Name = "comboBoxStyleOverFlow";
            this.comboBoxStyleOverFlow.Size = new System.Drawing.Size(121, 21);
            this.comboBoxStyleOverFlow.TabIndex = 1;
            this.comboBoxStyleOverFlow.Tag = "overflow";
            this.comboBoxStyleOverFlow.ValueMember = "Value";
            this.comboBoxStyleOverFlow.SelectionChangeCommitted += new System.EventHandler(this.comboBox_SelectedIndexChanged);
            // 
            // label47
            // 
            this.label47.Location = new System.Drawing.Point(8, 64);
            this.label47.Name = "label47";
            this.label47.Size = new System.Drawing.Size(112, 29);
            this.label47.TabIndex = 6;
            this.label47.Text = "Overflow";
            this.label47.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // comboBoxDisplay
            // 
            this.comboBoxDisplay.DisplayMember = "Member";
            this.comboBoxDisplay.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxDisplay.Location = new System.Drawing.Point(120, 29);
            this.comboBoxDisplay.Name = "comboBoxDisplay";
            this.comboBoxDisplay.Size = new System.Drawing.Size(121, 21);
            this.comboBoxDisplay.TabIndex = 0;
            this.comboBoxDisplay.Tag = "display";
            this.comboBoxDisplay.ValueMember = "Value";
            this.comboBoxDisplay.SelectionChangeCommitted += new System.EventHandler(this.comboBox_SelectedIndexChanged);
            // 
            // label45
            // 
            this.label45.Location = new System.Drawing.Point(8, 31);
            this.label45.Name = "label45";
            this.label45.Size = new System.Drawing.Size(112, 29);
            this.label45.TabIndex = 2;
            this.label45.Text = "Display";
            this.label45.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // groupBox9
            // 
            this.groupBox9.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox9.Controls.Add(this.numericUpDownLeft);
            this.groupBox9.Controls.Add(this.numericUpDownTop);
            this.groupBox9.Controls.Add(this.numericUpDownWidth);
            this.groupBox9.Controls.Add(this.numericUpDownHeight);
            this.groupBox9.Controls.Add(this.radioButtonPositionRemove);
            this.groupBox9.Controls.Add(this.label16);
            this.groupBox9.Controls.Add(this.label15);
            this.groupBox9.Controls.Add(this.label14);
            this.groupBox9.Controls.Add(this.label13);
            this.groupBox9.Controls.Add(this.comboBoxWidthUnit);
            this.groupBox9.Controls.Add(this.comboBoxHeightUnit);
            this.groupBox9.Controls.Add(this.comboBoxLeftUnit);
            this.groupBox9.Controls.Add(this.comboBoxTopUnit);
            this.groupBox9.Controls.Add(this.radioButtonPositionAbsolute);
            this.groupBox9.Controls.Add(this.radioButtonPositionRelative);
            this.groupBox9.Controls.Add(this.radioButtonPositionFixed);
            this.groupBox9.Controls.Add(this.radioButtonPositionNone);
            this.groupBox9.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox9.Location = new System.Drawing.Point(8, 201);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(416, 192);
            this.groupBox9.TabIndex = 0;
            this.groupBox9.TabStop = false;
            this.groupBox9.Text = "Position Mode of an Object";
            // 
            // numericUpDownLeft
            // 
            this.numericUpDownLeft.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numericUpDownLeft.Enabled = false;
            this.numericUpDownLeft.Location = new System.Drawing.Point(248, 144);
            this.numericUpDownLeft.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.numericUpDownLeft.Minimum = new decimal(new int[] {
            9999,
            0,
            0,
            -2147483648});
            this.numericUpDownLeft.Name = "numericUpDownLeft";
            this.numericUpDownLeft.Size = new System.Drawing.Size(64, 20);
            this.numericUpDownLeft.TabIndex = 0;
            this.numericUpDownLeft.Tag = "left|value";
            this.numericUpDownLeft.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            this.numericUpDownLeft.KeyDown += new System.Windows.Forms.KeyEventHandler(this.numericUpDown_KeyDown);
            // 
            // numericUpDownTop
            // 
            this.numericUpDownTop.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numericUpDownTop.Enabled = false;
            this.numericUpDownTop.Location = new System.Drawing.Point(248, 112);
            this.numericUpDownTop.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.numericUpDownTop.Minimum = new decimal(new int[] {
            9999,
            0,
            0,
            -2147483648});
            this.numericUpDownTop.Name = "numericUpDownTop";
            this.numericUpDownTop.Size = new System.Drawing.Size(64, 20);
            this.numericUpDownTop.TabIndex = 15;
            this.numericUpDownTop.Tag = "top|value";
            this.numericUpDownTop.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            this.numericUpDownTop.KeyDown += new System.Windows.Forms.KeyEventHandler(this.numericUpDown_KeyDown);
            // 
            // numericUpDownWidth
            // 
            this.numericUpDownWidth.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numericUpDownWidth.Enabled = false;
            this.numericUpDownWidth.Location = new System.Drawing.Point(248, 64);
            this.numericUpDownWidth.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.numericUpDownWidth.Name = "numericUpDownWidth";
            this.numericUpDownWidth.Size = new System.Drawing.Size(64, 20);
            this.numericUpDownWidth.TabIndex = 13;
            this.numericUpDownWidth.Tag = "width|value";
            this.numericUpDownWidth.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            this.numericUpDownWidth.KeyDown += new System.Windows.Forms.KeyEventHandler(this.numericUpDown_KeyDown);
            // 
            // numericUpDownHeight
            // 
            this.numericUpDownHeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numericUpDownHeight.Enabled = false;
            this.numericUpDownHeight.Location = new System.Drawing.Point(248, 32);
            this.numericUpDownHeight.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.numericUpDownHeight.Name = "numericUpDownHeight";
            this.numericUpDownHeight.Size = new System.Drawing.Size(64, 20);
            this.numericUpDownHeight.TabIndex = 11;
            this.numericUpDownHeight.Tag = "height|value";
            this.numericUpDownHeight.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            this.numericUpDownHeight.KeyDown += new System.Windows.Forms.KeyEventHandler(this.numericUpDown_KeyDown);
            // 
            // radioButtonPositionRemove
            // 
            this.radioButtonPositionRemove.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.radioButtonPositionRemove.Checked = true;
            this.radioButtonPositionRemove.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.radioButtonPositionRemove.Location = new System.Drawing.Point(16, 24);
            this.radioButtonPositionRemove.Name = "radioButtonPositionRemove";
            this.radioButtonPositionRemove.Size = new System.Drawing.Size(176, 32);
            this.radioButtonPositionRemove.TabIndex = 5;
            this.radioButtonPositionRemove.TabStop = true;
            this.radioButtonPositionRemove.Tag = "position|remove";
            this.radioButtonPositionRemove.Text = "Remove option ";
            this.radioButtonPositionRemove.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.radioButtonPositionRemove.CheckedChanged += new System.EventHandler(this.checkBox_CheckedChanged);
            // 
            // label16
            // 
            this.label16.Location = new System.Drawing.Point(152, 145);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(96, 44);
            this.label16.TabIndex = 0;
            this.label16.Text = "From left border:";
            this.label16.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label15
            // 
            this.label15.Location = new System.Drawing.Point(152, 113);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(96, 31);
            this.label15.TabIndex = 0;
            this.label15.Text = "From top border";
            this.label15.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label14
            // 
            this.label14.Location = new System.Drawing.Point(180, 64);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(66, 16);
            this.label14.TabIndex = 0;
            this.label14.Text = "Width";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label13
            // 
            this.label13.Location = new System.Drawing.Point(182, 32);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(66, 16);
            this.label13.TabIndex = 0;
            this.label13.Text = "Height";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboBoxWidthUnit
            // 
            this.comboBoxWidthUnit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxWidthUnit.DisplayMember = "Member";
            this.comboBoxWidthUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxWidthUnit.Location = new System.Drawing.Point(312, 64);
            this.comboBoxWidthUnit.Name = "comboBoxWidthUnit";
            this.comboBoxWidthUnit.Size = new System.Drawing.Size(96, 21);
            this.comboBoxWidthUnit.TabIndex = 12;
            this.comboBoxWidthUnit.Tag = "width|unit";
            this.comboBoxWidthUnit.ValueMember = "Value";
            this.comboBoxWidthUnit.SelectionChangeCommitted += new System.EventHandler(this.comboBox_SelectedIndexChanged);
            // 
            // comboBoxHeightUnit
            // 
            this.comboBoxHeightUnit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxHeightUnit.DisplayMember = "Member";
            this.comboBoxHeightUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxHeightUnit.Location = new System.Drawing.Point(312, 32);
            this.comboBoxHeightUnit.Name = "comboBoxHeightUnit";
            this.comboBoxHeightUnit.Size = new System.Drawing.Size(96, 21);
            this.comboBoxHeightUnit.TabIndex = 10;
            this.comboBoxHeightUnit.Tag = "height|unit";
            this.comboBoxHeightUnit.ValueMember = "Value";
            this.comboBoxHeightUnit.SelectionChangeCommitted += new System.EventHandler(this.comboBox_SelectedIndexChanged);
            // 
            // comboBoxLeftUnit
            // 
            this.comboBoxLeftUnit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxLeftUnit.DisplayMember = "Member";
            this.comboBoxLeftUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxLeftUnit.Location = new System.Drawing.Point(312, 144);
            this.comboBoxLeftUnit.Name = "comboBoxLeftUnit";
            this.comboBoxLeftUnit.Size = new System.Drawing.Size(96, 21);
            this.comboBoxLeftUnit.TabIndex = 16;
            this.comboBoxLeftUnit.Tag = "left|unit";
            this.comboBoxLeftUnit.ValueMember = "Value";
            this.comboBoxLeftUnit.SelectionChangeCommitted += new System.EventHandler(this.comboBox_SelectedIndexChanged);
            // 
            // comboBoxTopUnit
            // 
            this.comboBoxTopUnit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxTopUnit.DisplayMember = "Member";
            this.comboBoxTopUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxTopUnit.Location = new System.Drawing.Point(312, 112);
            this.comboBoxTopUnit.Name = "comboBoxTopUnit";
            this.comboBoxTopUnit.Size = new System.Drawing.Size(96, 21);
            this.comboBoxTopUnit.TabIndex = 14;
            this.comboBoxTopUnit.Tag = "top|unit";
            this.comboBoxTopUnit.ValueMember = "Value";
            this.comboBoxTopUnit.SelectionChangeCommitted += new System.EventHandler(this.comboBox_SelectedIndexChanged);
            // 
            // radioButtonPositionAbsolute
            // 
            this.radioButtonPositionAbsolute.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.radioButtonPositionAbsolute.Location = new System.Drawing.Point(16, 128);
            this.radioButtonPositionAbsolute.Name = "radioButtonPositionAbsolute";
            this.radioButtonPositionAbsolute.Size = new System.Drawing.Size(136, 24);
            this.radioButtonPositionAbsolute.TabIndex = 9;
            this.radioButtonPositionAbsolute.Tag = "position|param|absolute";
            this.radioButtonPositionAbsolute.Text = "Absolute";
            this.radioButtonPositionAbsolute.CheckedChanged += new System.EventHandler(this.checkBox_CheckedChanged);
            // 
            // radioButtonPositionRelative
            // 
            this.radioButtonPositionRelative.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.radioButtonPositionRelative.Location = new System.Drawing.Point(16, 104);
            this.radioButtonPositionRelative.Name = "radioButtonPositionRelative";
            this.radioButtonPositionRelative.Size = new System.Drawing.Size(136, 24);
            this.radioButtonPositionRelative.TabIndex = 8;
            this.radioButtonPositionRelative.Tag = "position|param|relative";
            this.radioButtonPositionRelative.Text = "Text Floating";
            this.radioButtonPositionRelative.CheckedChanged += new System.EventHandler(this.checkBox_CheckedChanged);
            // 
            // radioButtonPositionFixed
            // 
            this.radioButtonPositionFixed.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.radioButtonPositionFixed.Location = new System.Drawing.Point(16, 80);
            this.radioButtonPositionFixed.Name = "radioButtonPositionFixed";
            this.radioButtonPositionFixed.Size = new System.Drawing.Size(136, 24);
            this.radioButtonPositionFixed.TabIndex = 7;
            this.radioButtonPositionFixed.Tag = "position|param|fixed";
            this.radioButtonPositionFixed.Text = "Relative non floating";
            this.radioButtonPositionFixed.CheckedChanged += new System.EventHandler(this.checkBox_CheckedChanged);
            // 
            // radioButtonPositionNone
            // 
            this.radioButtonPositionNone.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.radioButtonPositionNone.Location = new System.Drawing.Point(16, 56);
            this.radioButtonPositionNone.Name = "radioButtonPositionNone";
            this.radioButtonPositionNone.Size = new System.Drawing.Size(136, 24);
            this.radioButtonPositionNone.TabIndex = 6;
            this.radioButtonPositionNone.Tag = "position|param|none";
            this.radioButtonPositionNone.Text = "None";
            this.radioButtonPositionNone.CheckedChanged += new System.EventHandler(this.checkBox_CheckedChanged);
            // 
            // groupBox14
            // 
            this.groupBox14.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox14.Controls.Add(this.pictureBox10);
            this.groupBox14.Controls.Add(this.pictureBox8);
            this.groupBox14.Controls.Add(this.pictureBox7);
            this.groupBox14.Controls.Add(this.label69);
            this.groupBox14.Controls.Add(this.buttonListStyleImage);
            this.groupBox14.Controls.Add(this.label22);
            this.groupBox14.Controls.Add(this.label23);
            this.groupBox14.Controls.Add(this.radioButtonListBulletImage);
            this.groupBox14.Controls.Add(this.textBoxListStyleImage);
            this.groupBox14.Controls.Add(this.comboBoxListStylePosition);
            this.groupBox14.Controls.Add(this.label21);
            this.groupBox14.Controls.Add(this.comboBoxListStyleType);
            this.groupBox14.Controls.Add(this.radioButtonListBulletStyle);
            this.groupBox14.Controls.Add(this.radioButtonListBulletNone);
            this.groupBox14.Controls.Add(this.label35);
            this.groupBox14.Controls.Add(this.label46);
            this.groupBox14.Controls.Add(this.label57);
            this.groupBox14.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox14.Location = new System.Drawing.Point(8, 8);
            this.groupBox14.Name = "groupBox14";
            this.groupBox14.Size = new System.Drawing.Size(416, 387);
            this.groupBox14.TabIndex = 0;
            this.groupBox14.TabStop = false;
            this.groupBox14.Text = "Define List Item Appearance";
            // 
            // pictureBox10
            // 
            this.pictureBox10.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox10.Cursor = System.Windows.Forms.Cursors.Help;
            this.pictureBox10.Location = new System.Drawing.Point(248, 24);
            this.pictureBox10.Name = "pictureBox10";
            this.pictureBox10.Size = new System.Drawing.Size(32, 16);
            this.pictureBox10.TabIndex = 13;
            this.pictureBox10.TabStop = false;
            // 
            // pictureBox8
            // 
            this.pictureBox8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox8.Cursor = System.Windows.Forms.Cursors.Help;
            this.pictureBox8.Location = new System.Drawing.Point(248, 115);
            this.pictureBox8.Name = "pictureBox8";
            this.pictureBox8.Size = new System.Drawing.Size(48, 16);
            this.pictureBox8.TabIndex = 12;
            this.pictureBox8.TabStop = false;
            // 
            // pictureBox7
            // 
            this.pictureBox7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox7.Cursor = System.Windows.Forms.Cursors.Help;
            this.pictureBox7.Location = new System.Drawing.Point(248, 82);
            this.pictureBox7.Name = "pictureBox7";
            this.pictureBox7.Size = new System.Drawing.Size(148, 16);
            this.pictureBox7.TabIndex = 11;
            this.pictureBox7.TabStop = false;
            // 
            // label69
            // 
            this.label69.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label69.Location = new System.Drawing.Point(32, 224);
            this.label69.Name = "label69";
            this.label69.Size = new System.Drawing.Size(360, 64);
            this.label69.TabIndex = 10;
            this.label69.Text = "These options determine the appearance of unordered lists.  The final result depe" +
                "nds also from written text, font and spacing characteristics.";
            // 
            // buttonListStyleImage
            // 
            this.buttonListStyleImage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonListStyleImage.Enabled = false;
            this.buttonListStyleImage.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.buttonListStyleImage.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonListStyleImage.Location = new System.Drawing.Point(248, 184);
            this.buttonListStyleImage.Name = "buttonListStyleImage";
            this.buttonListStyleImage.Size = new System.Drawing.Size(55, 20);
            this.buttonListStyleImage.TabIndex = 6;
            this.buttonListStyleImage.Text = "...";
            this.buttonListStyleImage.Click += new System.EventHandler(this.buttonListStyleImage_Click);
            // 
            // label22
            // 
            this.label22.Location = new System.Drawing.Point(8, 112);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(112, 23);
            this.label22.TabIndex = 5;
            this.label22.Text = "Position:";
            this.label22.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label23
            // 
            this.label23.Location = new System.Drawing.Point(32, 168);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(368, 16);
            this.label23.TabIndex = 9;
            this.label23.Text = "Select Image from disk:";
            // 
            // radioButtonListBulletImage
            // 
            this.radioButtonListBulletImage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.radioButtonListBulletImage.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.radioButtonListBulletImage.Location = new System.Drawing.Point(8, 144);
            this.radioButtonListBulletImage.Name = "radioButtonListBulletImage";
            this.radioButtonListBulletImage.Size = new System.Drawing.Size(384, 16);
            this.radioButtonListBulletImage.TabIndex = 4;
            this.radioButtonListBulletImage.Text = "With user specific bullet (choose image):";
            this.radioButtonListBulletImage.CheckedChanged += new System.EventHandler(this.radioButtonListBulletImage_CheckedChanged);
            // 
            // textBoxListStyleImage
            // 
            this.textBoxListStyleImage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxListStyleImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxListStyleImage.Enabled = false;
            this.textBoxListStyleImage.Location = new System.Drawing.Point(32, 184);
            this.textBoxListStyleImage.Name = "textBoxListStyleImage";
            this.textBoxListStyleImage.Size = new System.Drawing.Size(208, 20);
            this.textBoxListStyleImage.TabIndex = 5;
            this.textBoxListStyleImage.Tag = "list-style-image";
            this.textBoxListStyleImage.Leave += new System.EventHandler(this.textBox_TextChanged);
            // 
            // comboBoxListStylePosition
            // 
            this.comboBoxListStylePosition.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxListStylePosition.DisplayMember = "Member";
            this.comboBoxListStylePosition.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxListStylePosition.Location = new System.Drawing.Point(120, 112);
            this.comboBoxListStylePosition.Name = "comboBoxListStylePosition";
            this.comboBoxListStylePosition.Size = new System.Drawing.Size(120, 21);
            this.comboBoxListStylePosition.TabIndex = 3;
            this.comboBoxListStylePosition.Tag = "list-style-position";
            this.comboBoxListStylePosition.ValueMember = "Value";
            this.comboBoxListStylePosition.SelectionChangeCommitted += new System.EventHandler(this.comboBox_SelectedIndexChanged);
            // 
            // label21
            // 
            this.label21.Location = new System.Drawing.Point(8, 80);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(112, 23);
            this.label21.TabIndex = 3;
            this.label21.Text = "Style";
            this.label21.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboBoxListStyleType
            // 
            this.comboBoxListStyleType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxListStyleType.DisplayMember = "Member";
            this.comboBoxListStyleType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxListStyleType.Location = new System.Drawing.Point(120, 80);
            this.comboBoxListStyleType.Name = "comboBoxListStyleType";
            this.comboBoxListStyleType.Size = new System.Drawing.Size(120, 21);
            this.comboBoxListStyleType.TabIndex = 2;
            this.comboBoxListStyleType.Tag = "list-style-type";
            this.comboBoxListStyleType.ValueMember = "Value";
            this.comboBoxListStyleType.SelectionChangeCommitted += new System.EventHandler(this.comboBox_SelectedIndexChanged);
            // 
            // radioButtonListBulletStyle
            // 
            this.radioButtonListBulletStyle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.radioButtonListBulletStyle.Checked = true;
            this.radioButtonListBulletStyle.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.radioButtonListBulletStyle.Location = new System.Drawing.Point(8, 48);
            this.radioButtonListBulletStyle.Name = "radioButtonListBulletStyle";
            this.radioButtonListBulletStyle.Size = new System.Drawing.Size(392, 24);
            this.radioButtonListBulletStyle.TabIndex = 1;
            this.radioButtonListBulletStyle.TabStop = true;
            this.radioButtonListBulletStyle.Text = "With standard bullets (choose style and position):";
            this.radioButtonListBulletStyle.CheckedChanged += new System.EventHandler(this.radioButtonListBulletStyle_CheckedChanged);
            // 
            // radioButtonListBulletNone
            // 
            this.radioButtonListBulletNone.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.radioButtonListBulletNone.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.radioButtonListBulletNone.Location = new System.Drawing.Point(8, 24);
            this.radioButtonListBulletNone.Name = "radioButtonListBulletNone";
            this.radioButtonListBulletNone.Size = new System.Drawing.Size(232, 24);
            this.radioButtonListBulletNone.TabIndex = 0;
            this.radioButtonListBulletNone.Text = "Without bullets";
            this.radioButtonListBulletNone.CheckedChanged += new System.EventHandler(this.radioButtonListBulletNone_CheckedChanged);
            // 
            // label35
            // 
            this.label35.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label35.BackColor = System.Drawing.SystemColors.GrayText;
            this.label35.Location = new System.Drawing.Point(253, 27);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(30, 16);
            this.label35.TabIndex = 115;
            // 
            // label46
            // 
            this.label46.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label46.BackColor = System.Drawing.SystemColors.GrayText;
            this.label46.Location = new System.Drawing.Point(251, 88);
            this.label46.Name = "label46";
            this.label46.Size = new System.Drawing.Size(148, 13);
            this.label46.TabIndex = 116;
            // 
            // label57
            // 
            this.label57.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label57.BackColor = System.Drawing.SystemColors.GrayText;
            this.label57.Location = new System.Drawing.Point(251, 121);
            this.label57.Name = "label57";
            this.label57.Size = new System.Drawing.Size(48, 13);
            this.label57.TabIndex = 117;
            // 
            // groupBox23
            // 
            this.groupBox23.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox23.Controls.Add(this.numericUpDownColumnGap);
            this.groupBox23.Controls.Add(this.comboBoxColumnGap);
            this.groupBox23.Controls.Add(this.labelColumnGap);
            this.groupBox23.Controls.Add(this.pictureBox14);
            this.groupBox23.Controls.Add(this.numericUpDownRuleWidth);
            this.groupBox23.Controls.Add(this.comboBoxCommonRuleWidth);
            this.groupBox23.Controls.Add(this.label42);
            this.groupBox23.Controls.Add(this.label41);
            this.groupBox23.Controls.Add(this.label40);
            this.groupBox23.Controls.Add(this.comboBoxColumnRuleStyle);
            this.groupBox23.Controls.Add(this.label39);
            this.groupBox23.Controls.Add(this.numericUpDownColumns);
            this.groupBox23.Controls.Add(this.label72);
            this.groupBox23.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox23.Location = new System.Drawing.Point(8, 208);
            this.groupBox23.Name = "groupBox23";
            this.groupBox23.Size = new System.Drawing.Size(416, 189);
            this.groupBox23.TabIndex = 0;
            this.groupBox23.TabStop = false;
            this.groupBox23.Text = "Text floating in Columns";
            // 
            // numericUpDownColumnGap
            // 
            this.numericUpDownColumnGap.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numericUpDownColumnGap.Enabled = false;
            this.numericUpDownColumnGap.Location = new System.Drawing.Point(140, 144);
            this.numericUpDownColumnGap.Maximum = new decimal(new int[] {
            1024,
            0,
            0,
            0});
            this.numericUpDownColumnGap.Name = "numericUpDownColumnGap";
            this.numericUpDownColumnGap.Size = new System.Drawing.Size(48, 20);
            this.numericUpDownColumnGap.TabIndex = 6;
            this.numericUpDownColumnGap.Tag = "column-gap|value";
            // 
            // comboBoxColumnGap
            // 
            this.comboBoxColumnGap.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxColumnGap.DisplayMember = "Member";
            this.comboBoxColumnGap.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxColumnGap.Location = new System.Drawing.Point(188, 144);
            this.comboBoxColumnGap.Name = "comboBoxColumnGap";
            this.comboBoxColumnGap.Size = new System.Drawing.Size(80, 21);
            this.comboBoxColumnGap.TabIndex = 5;
            this.comboBoxColumnGap.Tag = "column-gap|unit";
            this.comboBoxColumnGap.ValueMember = "Value";
            this.comboBoxColumnGap.SelectionChangeCommitted += new System.EventHandler(this.comboBox_SelectedIndexChanged);
            // 
            // labelColumnGap
            // 
            this.labelColumnGap.Location = new System.Drawing.Point(12, 145);
            this.labelColumnGap.Name = "labelColumnGap";
            this.labelColumnGap.Size = new System.Drawing.Size(128, 32);
            this.labelColumnGap.TabIndex = 108;
            this.labelColumnGap.Text = "Column Gap:";
            this.labelColumnGap.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // pictureBox14
            // 
            this.pictureBox14.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox14.Cursor = System.Windows.Forms.Cursors.Help;
            this.pictureBox14.Location = new System.Drawing.Point(280, 48);
            this.pictureBox14.Name = "pictureBox14";
            this.pictureBox14.Size = new System.Drawing.Size(128, 120);
            this.pictureBox14.TabIndex = 107;
            this.pictureBox14.TabStop = false;
            // 
            // numericUpDownRuleWidth
            // 
            this.numericUpDownRuleWidth.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numericUpDownRuleWidth.Enabled = false;
            this.numericUpDownRuleWidth.Location = new System.Drawing.Point(140, 80);
            this.numericUpDownRuleWidth.Name = "numericUpDownRuleWidth";
            this.numericUpDownRuleWidth.Size = new System.Drawing.Size(48, 20);
            this.numericUpDownRuleWidth.TabIndex = 3;
            this.numericUpDownRuleWidth.Tag = "column-rule-width|value";
            this.numericUpDownRuleWidth.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            this.numericUpDownRuleWidth.KeyDown += new System.Windows.Forms.KeyEventHandler(this.numericUpDown_KeyDown);
            // 
            // comboBoxCommonRuleWidth
            // 
            this.comboBoxCommonRuleWidth.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxCommonRuleWidth.DisplayMember = "Member";
            this.comboBoxCommonRuleWidth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxCommonRuleWidth.Location = new System.Drawing.Point(188, 80);
            this.comboBoxCommonRuleWidth.Name = "comboBoxCommonRuleWidth";
            this.comboBoxCommonRuleWidth.Size = new System.Drawing.Size(80, 21);
            this.comboBoxCommonRuleWidth.TabIndex = 2;
            this.comboBoxCommonRuleWidth.Tag = "column-rule-width|unit";
            this.comboBoxCommonRuleWidth.ValueMember = "Value";
            this.comboBoxCommonRuleWidth.SelectionChangeCommitted += new System.EventHandler(this.comboBox_SelectedIndexChanged);
            // 
            // label42
            // 
            this.label42.Location = new System.Drawing.Point(12, 81);
            this.label42.Name = "label42";
            this.label42.Size = new System.Drawing.Size(128, 28);
            this.label42.TabIndex = 6;
            this.label42.Text = "Rule Width:";
            this.label42.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label41
            // 
            this.label41.Location = new System.Drawing.Point(12, 114);
            this.label41.Name = "label41";
            this.label41.Size = new System.Drawing.Size(128, 28);
            this.label41.TabIndex = 5;
            this.label41.Text = "Rule Style";
            this.label41.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label40
            // 
            this.label40.Location = new System.Drawing.Point(12, 51);
            this.label40.Name = "label40";
            this.label40.Size = new System.Drawing.Size(128, 30);
            this.label40.TabIndex = 4;
            this.label40.Text = "Rule Color";
            this.label40.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // comboBoxColumnRuleStyle
            // 
            this.comboBoxColumnRuleStyle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxColumnRuleStyle.DisplayMember = "Member";
            this.comboBoxColumnRuleStyle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxColumnRuleStyle.Location = new System.Drawing.Point(140, 112);
            this.comboBoxColumnRuleStyle.Name = "comboBoxColumnRuleStyle";
            this.comboBoxColumnRuleStyle.Size = new System.Drawing.Size(128, 21);
            this.comboBoxColumnRuleStyle.TabIndex = 4;
            this.comboBoxColumnRuleStyle.Tag = "column-rule-style";
            this.comboBoxColumnRuleStyle.ValueMember = "Value";
            this.comboBoxColumnRuleStyle.SelectedIndexChanged += new System.EventHandler(this.comboBox_SelectedIndexChanged);
            // 
            // label39
            // 
            this.label39.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label39.Location = new System.Drawing.Point(81, 21);
            this.label39.Name = "label39";
            this.label39.Size = new System.Drawing.Size(327, 16);
            this.label39.TabIndex = 1;
            this.label39.Text = "Columns";
            // 
            // numericUpDownColumns
            // 
            this.numericUpDownColumns.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numericUpDownColumns.Location = new System.Drawing.Point(8, 20);
            this.numericUpDownColumns.Name = "numericUpDownColumns";
            this.numericUpDownColumns.Size = new System.Drawing.Size(68, 20);
            this.numericUpDownColumns.TabIndex = 0;
            this.numericUpDownColumns.Tag = "columns";
            this.numericUpDownColumns.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            this.numericUpDownColumns.KeyDown += new System.Windows.Forms.KeyEventHandler(this.numericUpDown_KeyDown);
            // 
            // label72
            // 
            this.label72.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label72.BackColor = System.Drawing.SystemColors.GrayText;
            this.label72.Location = new System.Drawing.Point(287, 51);
            this.label72.Name = "label72";
            this.label72.Size = new System.Drawing.Size(124, 120);
            this.label72.TabIndex = 115;
            // 
            // groupBox16
            // 
            this.groupBox16.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox16.Controls.Add(this.pictureBox13);
            this.groupBox16.Controls.Add(this.pictureBox12);
            this.groupBox16.Controls.Add(this.numericUpDownRowSpan);
            this.groupBox16.Controls.Add(this.numericUpDownColumnSpan);
            this.groupBox16.Controls.Add(this.label44);
            this.groupBox16.Controls.Add(this.label43);
            this.groupBox16.Controls.Add(this.label74);
            this.groupBox16.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox16.Location = new System.Drawing.Point(8, 48);
            this.groupBox16.Name = "groupBox16";
            this.groupBox16.Size = new System.Drawing.Size(416, 72);
            this.groupBox16.TabIndex = 0;
            this.groupBox16.TabStop = false;
            this.groupBox16.Text = "Tables";
            // 
            // pictureBox13
            // 
            this.pictureBox13.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox13.Cursor = System.Windows.Forms.Cursors.Help;
            this.pictureBox13.Location = new System.Drawing.Point(261, 34);
            this.pictureBox13.Name = "pictureBox13";
            this.pictureBox13.Size = new System.Drawing.Size(148, 32);
            this.pictureBox13.TabIndex = 7;
            this.pictureBox13.TabStop = false;
            // 
            // pictureBox12
            // 
            this.pictureBox12.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox12.Cursor = System.Windows.Forms.Cursors.Help;
            this.pictureBox12.Location = new System.Drawing.Point(261, 9);
            this.pictureBox12.Name = "pictureBox12";
            this.pictureBox12.Size = new System.Drawing.Size(148, 28);
            this.pictureBox12.TabIndex = 6;
            this.pictureBox12.TabStop = false;
            // 
            // numericUpDownRowSpan
            // 
            this.numericUpDownRowSpan.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.numericUpDownRowSpan.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numericUpDownRowSpan.Location = new System.Drawing.Point(160, 40);
            this.numericUpDownRowSpan.Name = "numericUpDownRowSpan";
            this.numericUpDownRowSpan.Size = new System.Drawing.Size(72, 20);
            this.numericUpDownRowSpan.TabIndex = 1;
            this.numericUpDownRowSpan.Tag = "row-span";
            this.numericUpDownRowSpan.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            this.numericUpDownRowSpan.KeyDown += new System.Windows.Forms.KeyEventHandler(this.numericUpDown_KeyDown);
            // 
            // numericUpDownColumnSpan
            // 
            this.numericUpDownColumnSpan.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.numericUpDownColumnSpan.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numericUpDownColumnSpan.Location = new System.Drawing.Point(160, 16);
            this.numericUpDownColumnSpan.Name = "numericUpDownColumnSpan";
            this.numericUpDownColumnSpan.Size = new System.Drawing.Size(72, 20);
            this.numericUpDownColumnSpan.TabIndex = 0;
            this.numericUpDownColumnSpan.Tag = "column-span";
            this.numericUpDownColumnSpan.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            this.numericUpDownColumnSpan.KeyDown += new System.Windows.Forms.KeyEventHandler(this.numericUpDown_KeyDown);
            // 
            // label44
            // 
            this.label44.Location = new System.Drawing.Point(7, 40);
            this.label44.Name = "label44";
            this.label44.Size = new System.Drawing.Size(152, 16);
            this.label44.TabIndex = 3;
            this.label44.Text = "Row Span";
            this.label44.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label43
            // 
            this.label43.Location = new System.Drawing.Point(8, 16);
            this.label43.Name = "label43";
            this.label43.Size = new System.Drawing.Size(152, 16);
            this.label43.TabIndex = 2;
            this.label43.Text = "Column Span";
            this.label43.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label74
            // 
            this.label74.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label74.BackColor = System.Drawing.SystemColors.GrayText;
            this.label74.Location = new System.Drawing.Point(265, 13);
            this.label74.Name = "label74";
            this.label74.Size = new System.Drawing.Size(147, 56);
            this.label74.TabIndex = 115;
            // 
            // label38
            // 
            this.label38.Location = new System.Drawing.Point(16, 22);
            this.label38.Name = "label38";
            this.label38.Size = new System.Drawing.Size(176, 16);
            this.label38.TabIndex = 1;
            this.label38.Text = "Caption Position";
            // 
            // comboBoxCaptionSide
            // 
            this.comboBoxCaptionSide.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxCaptionSide.DisplayMember = "Member";
            this.comboBoxCaptionSide.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxCaptionSide.Location = new System.Drawing.Point(16, 40);
            this.comboBoxCaptionSide.Name = "comboBoxCaptionSide";
            this.comboBoxCaptionSide.Size = new System.Drawing.Size(176, 21);
            this.comboBoxCaptionSide.TabIndex = 0;
            this.comboBoxCaptionSide.Tag = "caption-side";
            this.comboBoxCaptionSide.ValueMember = "Value";
            this.comboBoxCaptionSide.SelectedIndexChanged += new System.EventHandler(this.comboBox_SelectedIndexChanged);
            // 
            // linkLabel1
            // 
            this.linkLabel1.Location = new System.Drawing.Point(285, 8);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(145, 37);
            this.linkLabel1.TabIndex = 0;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "Click here to set";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // label71
            // 
            this.label71.Location = new System.Drawing.Point(8, 8);
            this.label71.Name = "label71";
            this.label71.Size = new System.Drawing.Size(278, 32);
            this.label71.TabIndex = 6;
            this.label71.Text = "Height and Width is choosen on Position Tab:";
            this.label71.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // tabControlEditor
            // 
            this.tabControlEditor.Controls.Add(this.tabPageFonts);
            this.tabControlEditor.Controls.Add(this.tabPageLayout);
            this.tabControlEditor.Controls.Add(this.tabPageBackground);
            this.tabControlEditor.Controls.Add(this.tabPagePosition);
            this.tabControlEditor.Controls.Add(this.tabPageBorders);
            this.tabControlEditor.Controls.Add(this.tabPageLists);
            this.tabControlEditor.Controls.Add(this.tabPageTables);
            this.tabControlEditor.Controls.Add(this.tabPageMisc);
            this.tabControlEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlEditor.ImageList = this.imageListBorderTabs;
            this.tabControlEditor.Location = new System.Drawing.Point(0, 0);
            this.tabControlEditor.Multiline = true;
            this.tabControlEditor.Name = "tabControlEditor";
            this.tabControlEditor.SelectedIndex = 2;
            this.tabControlEditor.Size = new System.Drawing.Size(440, 448);
            this.tabControlEditor.TabIndex = 0;
            this.tabControlEditor.SelectedIndexChanged += new System.EventHandler(this.tabControlStyleDialogs_SelectedIndexChanged);
            // 
            // tabPageFonts
            // 
            this.tabPageFonts.Controls.Add(this.groupBox1);
            this.tabPageFonts.Controls.Add(this.groupBox2);
            this.tabPageFonts.Controls.Add(this.groupBox3);
            this.tabPageFonts.ImageIndex = 5;
            this.tabPageFonts.Location = new System.Drawing.Point(4, 42);
            this.tabPageFonts.Name = "tabPageFonts";
            this.tabPageFonts.Size = new System.Drawing.Size(432, 402);
            this.tabPageFonts.TabIndex = 1;
            this.tabPageFonts.Text = "Font";
            // 
            // tabPageLayout
            // 
            this.tabPageLayout.Controls.Add(this.groupBox10);
            this.tabPageLayout.Controls.Add(this.groupBox21);
            this.tabPageLayout.Controls.Add(this.groupBox8);
            this.tabPageLayout.Controls.Add(this.groupBox7);
            this.tabPageLayout.ImageIndex = 10;
            this.tabPageLayout.Location = new System.Drawing.Point(4, 42);
            this.tabPageLayout.Name = "tabPageLayout";
            this.tabPageLayout.Size = new System.Drawing.Size(432, 402);
            this.tabPageLayout.TabIndex = 4;
            this.tabPageLayout.Text = "Layout";
            // 
            // tabPageBackground
            // 
            this.tabPageBackground.Controls.Add(this.groupBox4);
            this.tabPageBackground.Controls.Add(this.groupBox5);
            this.tabPageBackground.ImageIndex = 6;
            this.tabPageBackground.Location = new System.Drawing.Point(4, 42);
            this.tabPageBackground.Name = "tabPageBackground";
            this.tabPageBackground.Size = new System.Drawing.Size(432, 402);
            this.tabPageBackground.TabIndex = 2;
            this.tabPageBackground.Text = "Background";
            // 
            // tabPagePosition
            // 
            this.tabPagePosition.Controls.Add(this.groupBox6);
            this.tabPagePosition.Controls.Add(this.groupBox9);
            this.tabPagePosition.ImageIndex = 9;
            this.tabPagePosition.Location = new System.Drawing.Point(4, 42);
            this.tabPagePosition.Name = "tabPagePosition";
            this.tabPagePosition.Size = new System.Drawing.Size(432, 402);
            this.tabPagePosition.TabIndex = 5;
            this.tabPagePosition.Text = "Position";
            // 
            // tabPageBorders
            // 
            this.tabPageBorders.Controls.Add(this.groupBox18);
            this.tabPageBorders.ImageIndex = 0;
            this.tabPageBorders.Location = new System.Drawing.Point(4, 42);
            this.tabPageBorders.Name = "tabPageBorders";
            this.tabPageBorders.Size = new System.Drawing.Size(432, 402);
            this.tabPageBorders.TabIndex = 3;
            this.tabPageBorders.Text = "Borders";
            // 
            // tabPageLists
            // 
            this.tabPageLists.Controls.Add(this.groupBox14);
            this.tabPageLists.ImageIndex = 7;
            this.tabPageLists.Location = new System.Drawing.Point(4, 42);
            this.tabPageLists.Name = "tabPageLists";
            this.tabPageLists.Size = new System.Drawing.Size(432, 402);
            this.tabPageLists.TabIndex = 6;
            this.tabPageLists.Text = "Lists";
            // 
            // tabPageTables
            // 
            this.tabPageTables.Controls.Add(this.groupBox11);
            this.tabPageTables.Controls.Add(this.groupBox16);
            this.tabPageTables.Controls.Add(this.groupBox23);
            this.tabPageTables.Controls.Add(this.label71);
            this.tabPageTables.Controls.Add(this.linkLabel1);
            this.tabPageTables.ImageIndex = 11;
            this.tabPageTables.Location = new System.Drawing.Point(4, 42);
            this.tabPageTables.Name = "tabPageTables";
            this.tabPageTables.Size = new System.Drawing.Size(432, 402);
            this.tabPageTables.TabIndex = 7;
            this.tabPageTables.Text = "Tables";
            // 
            // groupBox11
            // 
            this.groupBox11.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox11.Controls.Add(this.pictureBox11);
            this.groupBox11.Controls.Add(this.label38);
            this.groupBox11.Controls.Add(this.comboBoxCaptionSide);
            this.groupBox11.Controls.Add(this.label73);
            this.groupBox11.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox11.Location = new System.Drawing.Point(8, 128);
            this.groupBox11.Name = "groupBox11";
            this.groupBox11.Size = new System.Drawing.Size(416, 72);
            this.groupBox11.TabIndex = 0;
            this.groupBox11.TabStop = false;
            this.groupBox11.Text = "Caption";
            // 
            // pictureBox11
            // 
            this.pictureBox11.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox11.Cursor = System.Windows.Forms.Cursors.Help;
            this.pictureBox11.Location = new System.Drawing.Point(213, 17);
            this.pictureBox11.Name = "pictureBox11";
            this.pictureBox11.Size = new System.Drawing.Size(196, 48);
            this.pictureBox11.TabIndex = 2;
            this.pictureBox11.TabStop = false;
            // 
            // label73
            // 
            this.label73.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label73.BackColor = System.Drawing.SystemColors.GrayText;
            this.label73.Location = new System.Drawing.Point(220, 21);
            this.label73.Name = "label73";
            this.label73.Size = new System.Drawing.Size(192, 47);
            this.label73.TabIndex = 115;
            // 
            // tabPageMisc
            // 
            this.tabPageMisc.Controls.Add(this.groupBox15);
            this.tabPageMisc.Controls.Add(this.groupBox22);
            this.tabPageMisc.ImageIndex = 8;
            this.tabPageMisc.Location = new System.Drawing.Point(4, 42);
            this.tabPageMisc.Name = "tabPageMisc";
            this.tabPageMisc.Size = new System.Drawing.Size(432, 402);
            this.tabPageMisc.TabIndex = 8;
            this.tabPageMisc.Text = "Miscellenous";
            // 
            // groupBox15
            // 
            this.groupBox15.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox15.Controls.Add(this.label70);
            this.groupBox15.Controls.Add(this.pictureBox9);
            this.groupBox15.Controls.Add(this.label64);
            this.groupBox15.Controls.Add(this.label63);
            this.groupBox15.Controls.Add(this.label62);
            this.groupBox15.Controls.Add(this.label61);
            this.groupBox15.Controls.Add(this.label60);
            this.groupBox15.Controls.Add(this.label59);
            this.groupBox15.Controls.Add(this.label26);
            this.groupBox15.Controls.Add(this.label25);
            this.groupBox15.Controls.Add(this.label24);
            this.groupBox15.Controls.Add(this.comboBoxCursor);
            this.groupBox15.Controls.Add(this.label58);
            this.groupBox15.Controls.Add(this.label76);
            this.groupBox15.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox15.Location = new System.Drawing.Point(8, 8);
            this.groupBox15.Name = "groupBox15";
            this.groupBox15.Size = new System.Drawing.Size(416, 232);
            this.groupBox15.TabIndex = 0;
            this.groupBox15.TabStop = false;
            this.groupBox15.Text = "User Interface";
            // 
            // label70
            // 
            this.label70.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label70.BackColor = System.Drawing.SystemColors.Info;
            this.label70.Cursor = System.Windows.Forms.Cursors.Help;
            this.label70.ForeColor = System.Drawing.SystemColors.InfoText;
            this.label70.Location = new System.Drawing.Point(255, 60);
            this.label70.Name = "label70";
            this.label70.Size = new System.Drawing.Size(152, 75);
            this.label70.TabIndex = 21;
            this.label70.Text = "Cursor and Scrollbar options are currently not available for browser other then I" +
                "nternet Explorer.";
            // 
            // pictureBox9
            // 
            this.pictureBox9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox9.BackColor = System.Drawing.SystemColors.ControlDark;
            this.pictureBox9.Cursor = System.Windows.Forms.Cursors.Help;
            this.pictureBox9.Location = new System.Drawing.Point(256, 140);
            this.pictureBox9.Name = "pictureBox9";
            this.pictureBox9.Size = new System.Drawing.Size(154, 82);
            this.pictureBox9.TabIndex = 20;
            this.pictureBox9.TabStop = false;
            // 
            // label64
            // 
            this.label64.Location = new System.Drawing.Point(8, 204);
            this.label64.Name = "label64";
            this.label64.Size = new System.Drawing.Size(108, 16);
            this.label64.TabIndex = 19;
            this.label64.Text = "Track";
            this.label64.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label63
            // 
            this.label63.Location = new System.Drawing.Point(8, 180);
            this.label63.Name = "label63";
            this.label63.Size = new System.Drawing.Size(108, 16);
            this.label63.TabIndex = 18;
            this.label63.Text = "3D light";
            this.label63.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label62
            // 
            this.label62.Location = new System.Drawing.Point(8, 156);
            this.label62.Name = "label62";
            this.label62.Size = new System.Drawing.Size(108, 16);
            this.label62.TabIndex = 17;
            this.label62.Text = "Highlight";
            this.label62.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label61
            // 
            this.label61.Location = new System.Drawing.Point(8, 132);
            this.label61.Name = "label61";
            this.label61.Size = new System.Drawing.Size(110, 16);
            this.label61.TabIndex = 16;
            this.label61.Text = "Darkshadow";
            this.label61.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label60
            // 
            this.label60.Location = new System.Drawing.Point(8, 108);
            this.label60.Name = "label60";
            this.label60.Size = new System.Drawing.Size(108, 16);
            this.label60.TabIndex = 15;
            this.label60.Text = "Shadow";
            this.label60.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label59
            // 
            this.label59.Location = new System.Drawing.Point(8, 84);
            this.label59.Name = "label59";
            this.label59.Size = new System.Drawing.Size(108, 16);
            this.label59.TabIndex = 14;
            this.label59.Text = "Base";
            this.label59.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label26
            // 
            this.label26.Location = new System.Drawing.Point(8, 36);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(108, 16);
            this.label26.TabIndex = 12;
            this.label26.Text = "Face";
            this.label26.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label25
            // 
            this.label25.Location = new System.Drawing.Point(8, 16);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(242, 16);
            this.label25.TabIndex = 3;
            this.label25.Text = "Colors of Scrollbar elements:";
            // 
            // label24
            // 
            this.label24.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label24.Location = new System.Drawing.Point(251, 16);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(152, 16);
            this.label24.TabIndex = 2;
            this.label24.Text = "Cursor:";
            // 
            // comboBoxCursor
            // 
            this.comboBoxCursor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxCursor.DisplayMember = "Member";
            this.comboBoxCursor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxCursor.Location = new System.Drawing.Point(255, 33);
            this.comboBoxCursor.Name = "comboBoxCursor";
            this.comboBoxCursor.Size = new System.Drawing.Size(152, 21);
            this.comboBoxCursor.TabIndex = 8;
            this.comboBoxCursor.Tag = "cursor";
            this.comboBoxCursor.ValueMember = "Value";
            this.comboBoxCursor.SelectionChangeCommitted += new System.EventHandler(this.comboBox_SelectedIndexChanged);
            // 
            // label58
            // 
            this.label58.Location = new System.Drawing.Point(8, 60);
            this.label58.Name = "label58";
            this.label58.Size = new System.Drawing.Size(108, 16);
            this.label58.TabIndex = 13;
            this.label58.Text = "Arrow";
            this.label58.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label76
            // 
            this.label76.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label76.BackColor = System.Drawing.SystemColors.GrayText;
            this.label76.ForeColor = System.Drawing.SystemColors.InfoText;
            this.label76.Location = new System.Drawing.Point(258, 63);
            this.label76.Name = "label76";
            this.label76.Size = new System.Drawing.Size(152, 75);
            this.label76.TabIndex = 22;
            // 
            // groupBox22
            // 
            this.groupBox22.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox22.Controls.Add(this.radioButtonVisiblityRemove);
            this.groupBox22.Controls.Add(this.radioButtonVisibilityNone);
            this.groupBox22.Controls.Add(this.radioButtonVisibilityHidden);
            this.groupBox22.Controls.Add(this.radioButtonVisibilityVisible);
            this.groupBox22.Controls.Add(this.label31);
            this.groupBox22.Controls.Add(this.label75);
            this.groupBox22.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox22.Location = new System.Drawing.Point(8, 248);
            this.groupBox22.Name = "groupBox22";
            this.groupBox22.Size = new System.Drawing.Size(416, 146);
            this.groupBox22.TabIndex = 0;
            this.groupBox22.TabStop = false;
            this.groupBox22.Text = "Visibility";
            // 
            // radioButtonVisiblityRemove
            // 
            this.radioButtonVisiblityRemove.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.radioButtonVisiblityRemove.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.radioButtonVisiblityRemove.Checked = true;
            this.radioButtonVisiblityRemove.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.radioButtonVisiblityRemove.Location = new System.Drawing.Point(8, 17);
            this.radioButtonVisiblityRemove.Name = "radioButtonVisiblityRemove";
            this.radioButtonVisiblityRemove.Size = new System.Drawing.Size(160, 31);
            this.radioButtonVisiblityRemove.TabIndex = 0;
            this.radioButtonVisiblityRemove.TabStop = true;
            this.radioButtonVisiblityRemove.Tag = "visibility|remove";
            this.radioButtonVisiblityRemove.Text = "Remove option from sheet";
            this.radioButtonVisiblityRemove.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.radioButtonVisiblityRemove.CheckedChanged += new System.EventHandler(this.checkBox_CheckedChanged);
            // 
            // radioButtonVisibilityNone
            // 
            this.radioButtonVisibilityNone.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.radioButtonVisibilityNone.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.radioButtonVisibilityNone.Location = new System.Drawing.Point(8, 47);
            this.radioButtonVisibilityNone.Name = "radioButtonVisibilityNone";
            this.radioButtonVisibilityNone.Size = new System.Drawing.Size(152, 16);
            this.radioButtonVisibilityNone.TabIndex = 1;
            this.radioButtonVisibilityNone.Tag = "visibility|param|none";
            this.radioButtonVisibilityNone.Text = "None";
            this.radioButtonVisibilityNone.CheckedChanged += new System.EventHandler(this.checkBox_CheckedChanged);
            // 
            // radioButtonVisibilityHidden
            // 
            this.radioButtonVisibilityHidden.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.radioButtonVisibilityHidden.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.radioButtonVisibilityHidden.Location = new System.Drawing.Point(8, 88);
            this.radioButtonVisibilityHidden.Name = "radioButtonVisibilityHidden";
            this.radioButtonVisibilityHidden.Size = new System.Drawing.Size(152, 16);
            this.radioButtonVisibilityHidden.TabIndex = 3;
            this.radioButtonVisibilityHidden.Tag = "visibility|param|hidden";
            this.radioButtonVisibilityHidden.Text = "Hidden";
            this.radioButtonVisibilityHidden.CheckedChanged += new System.EventHandler(this.checkBox_CheckedChanged);
            // 
            // radioButtonVisibilityVisible
            // 
            this.radioButtonVisibilityVisible.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.radioButtonVisibilityVisible.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.radioButtonVisibilityVisible.Location = new System.Drawing.Point(8, 68);
            this.radioButtonVisibilityVisible.Name = "radioButtonVisibilityVisible";
            this.radioButtonVisibilityVisible.Size = new System.Drawing.Size(152, 15);
            this.radioButtonVisibilityVisible.TabIndex = 2;
            this.radioButtonVisibilityVisible.Tag = "visibility|param|visible";
            this.radioButtonVisibilityVisible.Text = "Visible";
            this.radioButtonVisibilityVisible.CheckedChanged += new System.EventHandler(this.checkBox_CheckedChanged);
            // 
            // label31
            // 
            this.label31.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label31.BackColor = System.Drawing.SystemColors.Info;
            this.label31.Cursor = System.Windows.Forms.Cursors.Help;
            this.label31.ForeColor = System.Drawing.SystemColors.InfoText;
            this.label31.Location = new System.Drawing.Point(172, 15);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(236, 90);
            this.label31.TabIndex = 6;
            this.label31.Tag = "";
            this.label31.Text = "If element set as unvisible (Visiblity = Hidden) it\'s necessary to change the beh" +
                "avior with scripting to show it later. If no scripting capabilites used set to \'" +
                "None\' or \'Remove\'.";
            // 
            // label75
            // 
            this.label75.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label75.BackColor = System.Drawing.SystemColors.GrayText;
            this.label75.ForeColor = System.Drawing.SystemColors.InfoText;
            this.label75.Location = new System.Drawing.Point(175, 18);
            this.label75.Name = "label75";
            this.label75.Size = new System.Drawing.Size(236, 90);
            this.label75.TabIndex = 14;
            this.label75.Tag = "";
            // 
            // Border
            // 
            this.Border.Controls.Add(this.tabControlEditor);
            this.Border.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Border.Location = new System.Drawing.Point(0, 0);
            this.Border.Name = "Border";
            this.Border.Size = new System.Drawing.Size(440, 448);
            this.Border.TabIndex = 1;
            // 
            // StyleControl
            // 
            this.Controls.Add(this.Border);
            this.Name = "StyleControl";
            this.Size = new System.Drawing.Size(440, 448);
            this.Resize += new System.EventHandler(this.StyleUserControl_Resize);
            this.groupBox3.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownFontSize)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownBackgroundPositionX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownBackgroundPositionY)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox18.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox15)).EndInit();
            this.tabControlBorders.ResumeLayout(false);
            this.tabPageAll.ResumeLayout(false);
            this.groupBox25.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownBorderWidthAll)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMarginAll)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPaddingAll)).EndInit();
            this.tabPageLeft.ResumeLayout(false);
            this.groupBox24.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownBorderLeftWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMarginLeft)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPaddingLeft)).EndInit();
            this.tabPageRight.ResumeLayout(false);
            this.groupBox17.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownBorderRightWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPaddingRight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMarginRight)).EndInit();
            this.tabPageTop.ResumeLayout(false);
            this.groupBox13.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownBorderTopWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPaddingTop)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMarginTop)).EndInit();
            this.tabPageBottom.ResumeLayout(false);
            this.groupBox12.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownBorderBottomWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPaddingBottom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMarginBottom)).EndInit();
            this.groupBox21.ResumeLayout(false);
            this.groupBox8.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTextIndent)).EndInit();
            this.groupBox7.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownWordSpacing)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLetterSpacing)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLineHeight)).EndInit();
            this.groupBox6.ResumeLayout(false);
            this.groupBox19.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.groupBox20.ResumeLayout(false);
            this.groupBox10.ResumeLayout(false);
            this.groupBox9.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLeft)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTop)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownHeight)).EndInit();
            this.groupBox14.ResumeLayout(false);
            this.groupBox14.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox10)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox7)).EndInit();
            this.groupBox23.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownColumnGap)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox14)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRuleWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownColumns)).EndInit();
            this.groupBox16.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox13)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox12)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRowSpan)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownColumnSpan)).EndInit();
            this.tabControlEditor.ResumeLayout(false);
            this.tabPageFonts.ResumeLayout(false);
            this.tabPageLayout.ResumeLayout(false);
            this.tabPageBackground.ResumeLayout(false);
            this.tabPagePosition.ResumeLayout(false);
            this.tabPageBorders.ResumeLayout(false);
            this.tabPageLists.ResumeLayout(false);
            this.tabPageTables.ResumeLayout(false);
            this.groupBox11.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox11)).EndInit();
            this.tabPageMisc.ResumeLayout(false);
            this.groupBox15.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox9)).EndInit();
            this.groupBox22.ResumeLayout(false);
            this.Border.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        # region External Event Handler

        /// <summary>
        /// Adds or removes the event handler for the event fired if any of the content is changed, e.g.
        /// one of the fields has had a interaction with the user.
        /// </summary>
        [Description("Adds or removes the event handler for the event fired if any of the content is changed")]
        [Category("Netrix Component")]
        public event EventHandler ContentChanged;

        /// <summary>
        /// Setting this property to true avoids recalling of the event during the parsing of style parts.
        /// </summary>
        private bool SupressEvents
        {
            get { return _supressEvents; }
            set { _supressEvents = value; }
        }

        /// <summary>
        /// Fires the OnContentChanged event after each field change operation
        /// </summary>
        /// <remarks>
        /// Fires the OnContentChanged event after each field change operation regardless whether the
        /// new value is valid or allowed. Invalid values may be removed later during parsing and the
        /// event is fired again with the correct values. This means for the event handler that any action
        /// can fire many times, but the last one contains always correct values.
        /// </remarks>
        /// <param name="sender">The control that has fired the event. Not used.</param>
        protected void OnContentChanged(object sender)
        {
            if (!SupressEvents)
            {
                SupressEvents = true;
                FieldsToSelector();
                if (ContentChanged != null)
                {
                    // add your own args here to give the handler information about the style changed
                    ContentChanged(sender, EventArgs.Empty);
                }
                SupressEvents = false;
            }
            ((Control)sender).Parent.Refresh();
            //Application.DoEvents();
        }

        /// <summary>
        /// Fired if a drag enter event occurs.
        /// </summary>
        /// <remarks>
        /// This simply calls the base event handler and does not check the content beeing dragged.
        /// </remarks>
        /// <param name="drgevent">The event arguments. If overridden in derived class the method should perform some actions on the data.</param>
        protected override void OnDragEnter(DragEventArgs drgevent)
        {
            if (this.AllowDrop)
            {
                base.OnDragEnter(drgevent);
            }
        }

        /// <summary>
        /// Fired if the control receives a drop event.
        /// </summary>
        /// <remarks>
        /// This method is called during a drop event. It gathers the data and tries to deserialize a text string
        /// from dragged data. If this is possible the <see cref="GuruComponents.Netrix.UserInterface.StyleEditor.StyleControl.StyleString">StyleString</see>
        /// fill be filled with the data without further investigation. The next change in the control may reduce the
        /// text to the recognizable parts.
        /// </remarks>
        /// <param name="drgevent">The event arguments. If overridden in derived class the method should perform some actions on the data.</param>
        protected override void OnDragDrop(DragEventArgs drgevent)
        {
            if (this.AllowDrop)
            {
                if (drgevent.Data.GetDataPresent(DataFormats.Text))
                {
                    string content = drgevent.Data.GetData(DataFormats.Text).ToString();
                    this.StyleString = content;
                }
            }
        }

        #endregion

        # region Private Helper Functions
        /// <summary>
        /// Transforms the whole style object into one string. 
        /// </summary>
        /// <param name="so">StyleObject</param>
        /// <param name="Formatted">If <code>true</code> the output is formatted with Tabs and Newlines.</param>
        /// <returns></returns>
        private string SelectorToString(SPP.StyleObject so, bool Formatted)
        {
            StringBuilder sb = new StringBuilder();
            StyleUnit su;
            StyleColor sc;
            StyleProperty sp;
            StyleList sl;
            StyleGroup sg;
            string[] arr = null;
            string StyleValue, StyleElement;
            foreach (DictionaryEntry d in so.Styles)
            {
                // before returning the style switch stylenames into upper case, this is the default behavior of MSHTML
                StyleElement = d.Key.ToString().ToUpper();
                StyleValue = String.Empty;
                switch (d.Value.GetType().Name)
                {
                    case "StyleUnit":
                        su = (StyleUnit)d.Value;
                        StyleValue = String.Format(CultureStyle, "{0}{1}", su.Value, su.Unit);
                        break;
                    case "StyleColor":
                        sc = (StyleColor)d.Value;
                        StyleValue = String.Format("#{0:X2}{1:X2}{2:X2}", sc.R, sc.G, sc.B);
                        break;
                    case "StyleProperty":
                        sp = (StyleProperty)d.Value;
                        // add url(...) to style values which contain urls for images
                        if (StyleElement.ToLower().IndexOf("image") != -1)
                        {
                            StyleValue = String.Concat("url(", sp.Value, ")");
                        }
                        else
                        {
                            StyleValue = sp.Value;
                        }
                        break;
                    case "StyleList":
                        sl = (StyleList)d.Value;
                        arr = new string[sl.Count];
                        sl.CopyTo(arr, 0);
                        StyleValue = String.Join(",", arr);
                        break;
                    case "StyleGroup":
                        sg = (StyleGroup)d.Value;
                        su = (StyleUnit)sg.Unit;
                        sp = (StyleProperty)sg.Property;
                        sc = (StyleColor)sg.Color;
                        sl = (StyleList)sg.List;
                        if (sc != null)
                        {
                            StyleValue = String.Format("#{0:X2}{1:X2}{2:X2}", sc.R, sc.G, sc.B);
                        }
                        if (sp != null)
                        {
                            StyleValue = sp.Value;
                        }
                        if (su != null)
                        {
                            StyleValue = String.Format(CultureStyle, "{0}{1}", su.Value, su.Unit);
                        }
                        if (sl != null)
                        {
                            arr = new string[sl.Count];
                            sl.CopyTo(arr, 0);
                            StyleValue = String.Join(",", arr);
                        }
                        break;
                }
                if (Formatted)
                    sb.AppendFormat("\t\t{0}:{1};{2}", StyleElement, StyleValue, System.Environment.NewLine);
                else
                    sb.AppendFormat("{0}:{1}; ", StyleElement, StyleValue);
            }
            return sb.ToString();
        }


        private void _ResetFields(Control c)
        {
            foreach (Control cc in c.Controls)
            {
                if (cc.Tag != null)
                {
                    // TODO: Define more robust "not defined" values to have better control on saving!
                    switch (cc.GetType().Name)
                    {
                        case "TextBox":
                            TextBox tb = (TextBox)cc;
                            tb.Text = String.Empty;
                            break;
                        case "ColorPickerUserControl":
                            CPU.ColorPickerUserControl gc = (CPU.ColorPickerUserControl)cc;
                            gc.ResetControl();
                            break;
                        case "ComboBox":
                            System.Windows.Forms.ComboBox cb = (System.Windows.Forms.ComboBox)cc;
                            if (cb.Items.Count > 0)
                                cb.SelectedIndex = -1;
                            break;
                        case "ListBox":
                            System.Windows.Forms.ListBox lb = (System.Windows.Forms.ListBox)cc;
                            lb.Items.Clear();
                            break;
                        case "FontPickerUserControl":
                            FPU.FontPickerUserControl glb = (FPU.FontPickerUserControl)cc;
                            glb.Items.Clear();
                            break;
                        case "NumericUpDown":
                            System.Windows.Forms.NumericUpDown num = (System.Windows.Forms.NumericUpDown)cc;
                            num.Value = (decimal)0;
                            break;
                        case "CheckBox":
                            System.Windows.Forms.CheckBox ch = (System.Windows.Forms.CheckBox)cc;
                            ch.Checked = false;
                            break;
                        case "RadioButton":
                            System.Windows.Forms.RadioButton rb = (System.Windows.Forms.RadioButton)cc;
                            if (((string)rb.Tag).IndexOf("|remove") != -1)
                            {
                                rb.Checked = true;
                            }
                            else
                            {
                                rb.Checked = false;
                            }
                            break;
                    }
                }
                if (cc.HasChildren)
                {
                    _ResetFields(cc);
                }
            }
        }


        // color box
        private void FillColorFields(string StyleName, int R, int G, int B)
        {
            GetStyleControl(this, StyleName);
            if (this.foundControl != null)
            {
                if (this.foundControl.GetType().Name == "ColorPickerUserControl")
                {
                    CPU.ColorPickerUserControl gc = (CPU.ColorPickerUserControl)this.foundControl;
                    gc.CurrentColor = System.Drawing.Color.FromArgb(R, G, B);
                }
            }
        }
        // list with multiple values
        private void FillListFields(string StyleName, string[] ListValue)
        {
            GetStyleControl(this, String.Format("{0}", StyleName));
            if (this.foundControl == null) return;
            if (this.foundControl.GetType().Name == "ComboBox")
            {
                ComboBox cb = (ComboBox)this.foundControl;
                cb.Items.Clear();
                cb.Items.AddRange(ListValue);
                return;
            }
            if (this.foundControl.GetType().Name == "ListBox")
            {
                ListBox lb = (ListBox)this.foundControl;
                lb.Items.Clear();
                lb.Items.AddRange(ListValue);
                return;
            }
            if (this.foundControl.GetType().Name == "FontPickerUserControl")
            {
                FPU.FontPickerUserControl gb = (FPU.FontPickerUserControl)this.foundControl;
                gb.Items.Clear();
                gb.Items.AddRange(ListValue);
                return;
            }
        }

        private void FillStringFields(string StyleName, string Value)
        {
            GetStyleControl(this, StyleName);
            if (this.foundControl == null)
            {
                // some fields can contain unit/val pairs or fixed values, so we try to get them all
                GetStyleControl(this, String.Format("{0}|absolute", StyleName));
                if (this.foundControl != null && this.foundControl.GetType().Name == "ComboBox")
                {
                    ComboBox cb = (ComboBox)this.foundControl;
                    // TODO: chek why FindString not works with border width styles
                    cb.SelectedIndex = cb.FindString(GetStylePropertyName(cb, Value));
                    return;
                }
                // special for checkboxes
                GetStyleControl(this, String.Format("{0}|{1}", StyleName, Value));
                if (this.foundControl != null && this.foundControl.GetType().Name == "CheckBox")
                {
                    CheckBox ch = (CheckBox)this.foundControl;
                    ch.Checked = true;
                    return;
                }
                // special for radiobuttons
                GetStyleControl(this, String.Format("{0}|param|{1}", StyleName, Value));
                if (this.foundControl != null && this.foundControl.GetType().Name == "RadioButton")
                {
                    RadioButton rb = (RadioButton)this.foundControl;
                    rb.Checked = true;
                    return;
                }
            }
            else
            {
                switch (this.foundControl.GetType().Name)
                {
                    case "NumericUpDown":
                        NumericUpDown nud = (NumericUpDown)this.foundControl;
                        try
                        {
                            nud.Value = Convert.ToDecimal(Value);
                        }
                        catch
                        {
                            // catch exception thrown by unallowed values to prevent closing the whole editor
                            nud.Value = 0;
                        }
                        break;
                    case "TextBox":
                        TextBox tb = (TextBox)this.foundControl;
                        tb.Text = Value;
                        break;
                    case "ComboBox":
                        ComboBox cb = (ComboBox)this.foundControl;
                        cb.SelectedIndex = cb.FindString(GetStylePropertyName(cb, Value));
                        break;
                    case "FontPickerUserControl":
                        FPU.FontPickerUserControl gb = (FPU.FontPickerUserControl)this.foundControl;
                        gb.Items.Clear();
                        gb.Items.Add(Value);
                        break;
                }
            }
        }

        private void FillUnitFields(string StyleName, float Value, string Unit)
        {
            GetStyleControl(this, String.Format("{0}|unit", StyleName));
            if (this.foundControl == null) return;
            if (this.foundControl.GetType().Name == "ComboBox")
            {
                ComboBox cb = (ComboBox)this.foundControl;
                cb.SelectedIndex = cb.FindString(GetStylePropertyName(cb, Unit));
            }
            this.foundControl = null;
            GetStyleControl(this, String.Format("{0}|value", StyleName));
            if (this.foundControl == null) return;
            if (this.foundControl.GetType().Name == "NumericUpDown")
            {
                System.Windows.Forms.NumericUpDown n = (System.Windows.Forms.NumericUpDown)this.foundControl;
                n.Enabled = true;
                try
                {
                    n.Value = (decimal)Value;
                }
                catch
                {
                    n.Value = 0;
                }
            }
        }
        // one value from list (like "Arial")
        private void FillSelectFields(string StyleName, string ListValue)
        {
            GetStyleControl(this, String.Format("{0}", StyleName));
            if (this.foundControl == null) return;
            if (this.foundControl.GetType().Name == "ComboBox")
            {
                ComboBox cb = (ComboBox)this.foundControl;
                cb.SelectedIndex = cb.FindString(ListValue);
            }
        }


        /// <summary>
        /// Searches for the appropritiate control in the controls collections of the tab pages.
        /// To start c must be null. Returns the control.
        /// </summary>
        /// <param name="c"></param>
        /// <param name="StyleName"></param>
        private void GetStyleControl(Control c, string StyleName)
        {
            if (c == null)
            {
                c = this;
                this.foundControl = null;
            }
            if (this.foundControl != null) return;
            foreach (Control cc in c.Controls)
            {
                if (cc.Tag != null && ((string)cc.Tag).ToLower().Equals(StyleName.ToLower()))
                {
                    this.foundControl = cc;
                    break;
                }
                if (cc.HasChildren)
                {
                    GetStyleControl(cc, StyleName);
                }
            }
        }		// simple text field or one value form a list

        /// <summary>
        /// Looks for a internal value in a given list based on the display name.
        /// </summary>
        /// <param name="cb"></param>
        /// <param name="Value"></param>
        /// <returns><code>String.Empty</code> if no element found, the element member else.</returns>
        private string GetStylePropertyName(ComboBox cb, string Value)
        {
            foreach (UnitPair up in cb.Items)
            {
                if (up.Value == Value)
                    return up.Member;
            }
            return String.Empty;
        }


        /// <summary>
        /// Run through all controls and create a hashtable with the changed/setted styles.
        /// </summary>
        /// <param name="c">Control where the operation starts.</param>
        /// <param name="ListOfStyles">The Hashtable with the current styles.</param>
        /// <returns></returns>
        private void FieldsToObjects(Control c, ref Hashtable ListOfStyles)
        {
            // loop through the controls to gather changes
            StyleUnit su;
            StyleColor sc;
            StyleProperty sp;
            StyleList sl;
            // Genesis.StyleParser.StyleGroup sg;
            string StyleBaseElement;
            foreach (Control cc in c.Controls)
            {
                if ((string)cc.Tag != String.Empty)
                {
                    string StyleElement = (string)cc.Tag;
                    if (StyleElement != null)
                    {
                        #region switch
                        switch (cc.GetType().Name)
                        {
                            case "CheckBox":
                                // as defined: style|property
                                CheckBox ch = (CheckBox)cc;
                                string[] se = StyleElement.Split(new char[] { '|' }, 2);
                                StyleBaseElement = se[0];
                                if (ch.Checked)
                                {
                                    sp = new StyleProperty(se[1]);
                                    ListOfStyles[StyleBaseElement] = sp;
                                }
                                else
                                {
                                    ListOfStyles.Remove(StyleBaseElement);
                                }
                                break;
                            case "TextBox":
                                TextBox tb = (TextBox)cc;
                                if (tb.Text != String.Empty)
                                {
                                    sp = new StyleProperty(tb.Text);
                                    ListOfStyles[StyleElement] = sp;
                                }
                                else
                                {
                                    ListOfStyles.Remove(StyleElement);
                                }
                                break;
                            case "ColorPickerUserControl":
                                CPU.ColorPickerUserControl gc = (CPU.ColorPickerUserControl)cc;
                                if (gc.CurrentColor != Color.Empty)
                                {
                                    sc = new StyleColor();
                                    sc.R = gc.CurrentColor.R;
                                    sc.G = gc.CurrentColor.G;
                                    sc.B = gc.CurrentColor.B;
                                    ListOfStyles[StyleElement] = sc;
                                }
                                else
                                {
                                    ListOfStyles.Remove(StyleElement);
                                }
                                break;
                            case "ComboBox":
                                System.Windows.Forms.ComboBox cb = (System.Windows.Forms.ComboBox)cc;
                                if (cb.Items.Count > 0 && cb.SelectedIndex != -1 && cb.SelectedItem.ToString().Length > 0)
                                {
                                    string cbValue = ((UnitPair)cb.SelectedItem).Value;
                                    // handle *|unit fields with the numUpDown control, any other here
                                    if (StyleElement.IndexOf("|absolute") != -1)
                                    {
                                        StyleBaseElement = StyleElement.Substring(0, StyleElement.IndexOf("|absolute"));
                                        if (cbValue.Equals(String.Empty))
                                        {
                                            ListOfStyles.Remove(StyleBaseElement);
                                        }
                                        else
                                        {
                                            sp = new StyleProperty(cbValue);
                                            ListOfStyles[StyleBaseElement] = sp;
                                        }
                                    }
                                    else if (StyleElement.IndexOf("|unit") == -1)
                                    {
                                        if (cbValue.Equals(String.Empty))
                                        {
                                            ListOfStyles.Remove(StyleElement);
                                        }
                                        else
                                        {
                                            sp = new StyleProperty(cbValue);
                                            ListOfStyles[StyleElement] = sp;
                                        }
                                    }
                                }
                                else
                                {
                                    ListOfStyles.Remove(StyleElement);
                                }
                                break;
                            case "FontPickerUserControl":
                                FPU.FontPickerUserControl gl = (FPU.FontPickerUserControl)cc;
                                if (gl.Items.Count > 0)
                                {
                                    sl = new StyleList();
                                    foreach (string s in gl.Items)
                                    {
                                        sl.Add(s);
                                    }
                                    ListOfStyles[StyleElement] = sl;
                                }
                                else
                                {
                                    ListOfStyles.Remove(StyleElement);
                                }
                                break;
                            case "ListBox":
                                System.Windows.Forms.ListBox lb = (System.Windows.Forms.ListBox)cc;
                                if (lb.Items.Count > 0)
                                {
                                    sl = new StyleList();
                                    foreach (string s in lb.Items)
                                    {
                                        sl.Add(s);
                                    }
                                    ListOfStyles[StyleElement] = sl;
                                }
                                else
                                {
                                    ListOfStyles.Remove(StyleElement);
                                }
                                break;
                            case "NumericUpDown":
                                System.Windows.Forms.NumericUpDown num = (System.Windows.Forms.NumericUpDown)cc;
                                // do not handle *|value fields here
                                if (StyleElement.IndexOf("|value") == -1)
                                {
                                    if (num.Value == 0)
                                    {
                                        ListOfStyles.Remove(StyleElement);
                                    }
                                    else
                                    {
                                        sp = new StyleProperty(num.Value.ToString());
                                        ListOfStyles[StyleElement] = sp;
                                    }
                                }
                                else
                                {
                                    // found a |value field
                                    StyleBaseElement = StyleElement.Substring(0, StyleElement.IndexOf("|value"));
                                    su = new StyleUnit((float)num.Value, String.Empty);
                                    // look for the unit field in the same parent control
                                    foreach (Control ccc in cc.Parent.Controls)
                                    {
                                        if ((string)ccc.Tag != String.Empty && ccc.GetType().Name == "ComboBox")
                                        {
                                            ComboBox cbc = (ComboBox)ccc;
                                            if (((string)cbc.Tag).IndexOf(String.Format("{0}|unit", StyleBaseElement)) != -1)
                                            {
                                                if (cbc.SelectedIndex != -1)
                                                {
                                                    su.Unit = ((UnitPair)cbc.SelectedItem).Value;
                                                }
                                                break;
                                            }
                                        }
                                    }
                                    if (su.Unit.Equals(String.Empty))
                                    {
                                        ListOfStyles.Remove(StyleBaseElement);
                                    }
                                    else
                                    {
                                        ListOfStyles[StyleBaseElement] = su;
                                    }
                                }
                                break;
                            case "RadioButton":
                                RadioButton rb = (RadioButton)cc;
                                if (rb.Checked)
                                {
                                    string[] parts = StyleElement.Split(new char[] { '|' });
                                    if (parts.Length == 3 && parts[1] == "param")
                                    {
                                        sp = new StyleProperty(parts[2]);
                                        ListOfStyles[parts[0]] = sp;
                                    }
                                    if (parts.Length == 2 && parts[1] == "remove")
                                    {
                                        ListOfStyles.Remove(parts[0]);
                                    }
                                }
                                break;
                        }
                        #endregion
                    }
                    // set form as dirty to inform outer controls ??
                }
                if (cc.HasChildren)
                {
                    FieldsToObjects(cc, ref ListOfStyles);
                }
            }
        }


        #endregion

        # region Public Accessor Methods

        /// <summary>
        /// Takes a given StyleObject and fill fields from data. 
        /// </summary>
        /// <remarks>The method does nothing if StyleObject is null.</remarks>
        public void SelectorToFields()
        {
            if (this.StyleObject == null) return;
            SPP.StyleObject so = this.StyleObject;
            Hashtable ht = so.Styles;
            if (ht == null) return;
            SupressEvents = true;
            this.ResetFields();
            StyleUnit su;
            StyleColor sc;
            StyleProperty sp;
            StyleList sl;
            StyleGroup sg;
            string StyleElement;
            foreach (DictionaryEntry d in ht)
            {
                this.foundControl = null;						// force search for every entry
                StyleElement = d.Key.ToString();
                switch (d.Value.GetType().Name)
                {
                    case "StyleUnit":
                        su = (StyleUnit)d.Value;
                        FillUnitFields(StyleElement, su.Value, su.Unit);
                        break;
                    case "StyleColor":
                        sc = (StyleColor)d.Value;
                        FillColorFields(StyleElement, sc.R, sc.G, sc.B);
                        break;
                    case "StyleProperty":
                        sp = (StyleProperty)d.Value;
                        FillStringFields(StyleElement, sp.Value);
                        break;
                    case "StyleList":
                        sl = (StyleList)d.Value;
                        FillListFields(StyleElement, sl.ToArray());
                        break;
                    case "StyleGroup":
                        sg = (StyleGroup)d.Value;
                        su = (StyleUnit)sg.Unit;
                        sp = (StyleProperty)sg.Property;
                        sc = (StyleColor)sg.Color;
                        sl = (StyleList)sg.List;
                        if (sc != null)
                        {
                            if (StyleElement.StartsWith("border", StringComparison.OrdinalIgnoreCase))
                            {
                                FillColorFields(String.Format("{0}-color", StyleElement), sc.R, sc.G, sc.B);
                            }
                            else
                            {
                                FillColorFields(StyleElement, sc.R, sc.G, sc.B);
                            }
                        }
                        if (sp != null)
                        {
                            if (StyleElement.StartsWith("border", StringComparison.OrdinalIgnoreCase))
                            {
                                FillStringFields(String.Format("{0}-style", StyleElement), sp.Value);
                            }
                            else
                            {
                                FillStringFields(StyleElement, sp.Value);
                            }
                        }
                        if (su != null)
                        {
                            if (StyleElement.StartsWith("border", StringComparison.OrdinalIgnoreCase))
                            {
                                FillUnitFields(String.Format("{0}-width", StyleElement), su.Value, su.Unit);
                            }
                            else
                            {
                                FillUnitFields(StyleElement, su.Value, su.Unit);
                            }
                        }
                        if (sl != null)
                        {
                            FillListFields(StyleElement, sl.ToArray());
                        }
                        break;
                }
            }
            SupressEvents = false;
        }


        /// <summary>
        /// Refreshes a StyleObject object with the current state of the fields.
        /// </summary>
        /// <returns>Modified StyleObject</returns>
        public SPP.StyleObject FieldsToSelector()
        {
            SPP.StyleObject so = this.StyleObject;
            if (so != null)
            {
                Hashtable ListOfStyles = System.Collections.Specialized.CollectionsUtil.CreateCaseInsensitiveHashtable(5);
                ListOfStyles = so.Styles;
                this.FieldsToObjects(this.tabControlEditor, ref ListOfStyles);
                so.Styles = ListOfStyles;
            }
            return so;
        }


        /// <summary>
        /// Sets all controls to an "undefined" state.
        /// </summary>
        public void ResetFields()
        {
            _ResetFields(this.tabControlEditor);
        }

        /// <summary>
        /// This method converts any StyleObject object into the appropreciated string representation.
        /// </summary>
        /// <param name="so">The StyleObjetc to convert.</param>
        /// <param name="Formatted">If <code>true</code> provides newlines and breaks.</param>
        /// <returns></returns>
        public string StyleObjectToString(SPP.StyleObject so, bool Formatted)
        {
            if (so == null) return String.Empty;
            return SelectorToString(so, Formatted);
        }

        /// <summary>
        /// Gets or sets the current StyleObject, containing the Hashtable with all style objects.
        /// </summary>
        [Browsable(false)]
        public SPP.StyleObject StyleObject
        {
            get
            {
                if (_so == null)
                {
                    _so = new StyleObject();
                }
                return _so;
            }
            set
            {
                _so = value;
            }
        }


        /// <summary>
        /// Gets or sets the string representation of current StyleObject object. 
        /// </summary>
        /// <remarks>If the string is set it will
        /// parsed by the style parser. Internally a virtual selector 'b' is being created.</remarks>
        [Description("Gets or sets the string representation of current StyleObject object. If the string is set it will parsed by the style parser.")]
        [Category("NetRix")]
        public string StyleString
        {
            get
            {
                string result = String.Empty;
                if (StyleObject != null)
                {
                    result = this.SelectorToString(StyleObject, false);
                }
                return result;
            }
            set
            {
                // parse one style to create StyleObject
                this.ResetFields();
                parser.StyleSheet = "b {" + value + "}";
                parser.Parse();             
            }
        }

        /// <summary>
        /// Gets the name of currently selected tab.
        /// </summary>
        [Browsable(false)]
        public string SelectedTabName
        {
            get
            {
                return this.tabControlEditor.SelectedTab.Name;
            }
        }


        #endregion

        # region External Events

        /// <summary>
        /// Fired if the parser used internally is done and all fields are populated.
        /// </summary>
        [Description("Fired if the parser used internally is done and all fields are populated.")]
        [Category("Netrix Component")]
        public event SelectorEventHandler ParserReady;

        /// <summary>
        /// Invokes the event that is fired if the parser used internally is done and all fields are populated.
        /// </summary>
        /// <param name="e">The parameters</param>
        /// <param name="sender">The control instance</param>
        protected void OnParserReady(object sender, SelectorEventArgs e)
        {
            if (ParserReady != null)
            {
                ParserReady(sender, e);
            }
        }

        # endregion

        # region Internal Event Handler

        private void SelectorHandler(object sender, SelectorEventArgs args)
        {
            if (args.Name == null) return;
            this.StyleObject = args.Selector;
            SelectorToFields();
            OnParserReady(this, args);
        }

        private void tabControlStyleDialogs_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            this.Invalidate();
            Application.DoEvents();
        }

        private void comboBox_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            ComboBox cb = (ComboBox)sender;
            string tag = (cb.Tag == null) ? String.Empty : cb.Tag.ToString();
            if (tag != null && tag.EndsWith("|unit"))
            {
                // this part enables or disables the related num control accordingly to the selected value
                string leftPart = String.Concat(tag.Substring(0, tag.IndexOf("|") + 1), "value");
                this.GetStyleControl(null, leftPart);
                Control c = this.foundControl;
                if (c != null && c is NumericUpDown)
                {
                    System.Windows.Forms.NumericUpDown n = (System.Windows.Forms.NumericUpDown)c;
                    n.TextChanged -= new System.EventHandler(this.textBox_TextChanged);
                    if (cb.SelectedItem == null || ((UnitPair)cb.SelectedItem).Value.Equals(String.Empty))
                    {
                        n.Enabled = false;
                    }
                    else
                    {
                        n.Enabled = true;
                        switch (((UnitPair)cb.SelectedItem).Value)
                        {
                            case "px":
                            case "em":
                            case "ex":
                                n.DecimalPlaces = 0;
                                break;
                            case "pt":
                            case "%":
                            case "pc":
                            case "mm":
                            case "cm":
                                n.DecimalPlaces = 2;
                                break;
                        }
                    }
                    n.TextChanged += new System.EventHandler(this.textBox_TextChanged);
                }
            }
            this.OnContentChanged(sender);
        }
        // used for radiobuttons, too
        private void checkBox_CheckedChanged(object sender, System.EventArgs e)
        {
            this.OnContentChanged(sender);
        }

        private void genesisColor_ColorChanged(object sender, GuruComponents.Netrix.UserInterface.ColorPicker.ColorChangedEventArgs e)
        {
            this.OnContentChanged(sender);
        }

        private int currentCaretPosition = 0;

        private void textBox_TextChanged(object sender, System.EventArgs e)
        {
            switch (sender.GetType().Name)
            {
                case "TextBox":
                    TextBox tb = (TextBox)sender;
                    currentCaretPosition = tb.SelectionStart;
                    this.OnContentChanged(sender);
                    tb.SelectionStart = currentCaretPosition;
                    break;
                case "NumericUpDown":
                    NumericUpDown nb = (NumericUpDown)sender;
                    if (nb.Value.ToString().Equals("0"))
                    {
                        nb.Select(0, 1);
                        Caret = 1;
                    }
                    else
                    {
                        nb.Select(Caret, 0);
                    }
                    this.OnContentChanged(sender);
                    break;
            }

        }

        private void comboBoxFontSizeUnit_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            this.radioButtonFontSizeUnit.Checked = true;
            this.radioButtonFontSizeAbsolute.Checked = false;
            this.comboBoxFontSizeAbsolute.SelectedIndex = -1;
        }

        private void comboBoxFontSizeAbsolute_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            this.radioButtonFontSizeUnit.Checked = false;
            this.radioButtonFontSizeAbsolute.Checked = true;
            this.numericUpDownFontSize.Value = 0.0m;
            this.comboBoxFontSizeUnit.SelectedIndex = -1;
        }

        private void genesisListBoxFontFamily_ContentChanged(object sender, System.EventArgs e)
        {
            this.OnContentChanged(sender);
        }

        private void linkLabel1_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
        {
            this.tabControlEditor.SelectedIndex = this.tabControlEditor.TabPages.IndexOf(this.tabPagePosition);
        }

        private void radioButtonFontSizeUnit_CheckedChanged(object sender, System.EventArgs e)
        {
            if (radioButtonFontSizeUnit.Checked)
            {
                this.numericUpDownFontSize.Enabled = true;
                this.comboBoxFontSizeUnit.Enabled = true;
                this.comboBoxFontSizeAbsolute.Enabled = false;
                this.comboBoxFontSizeAbsolute.SelectedIndex = -1;
            }
            else
            {
                this.numericUpDownFontSize.Enabled = false;
                this.comboBoxFontSizeUnit.Enabled = false;
                this.comboBoxFontSizeAbsolute.Enabled = true;
                this.comboBoxFontSizeUnit.SelectedIndex = -1;
            }
        }

        private void buttonBackgroundImage_Click(object sender, System.EventArgs e)
        {
            openFileDialog.Reset();
            openFileDialog.Title = ResourceManager.GetString("StyleEditor.OpenFileDialog.BackgroundImage.Title");
            openFileDialog.Filter = ResourceManager.GetString("StyleEditor.OpenFileDialog.BackgroundImage.Filter");
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                this.textBoxBackgroundImage.Text = System.IO.Path.GetFileName(openFileDialog.FileName);
                // force change after new image from disc
                this.textBox_TextChanged(sender, e);
            }
        }

        private void buttonListStyleImage_Click(object sender, System.EventArgs e)
        {
            openFileDialog.Reset();
            openFileDialog.Title = ResourceManager.GetString("StyleEditor.OpenFileDialog.ListImage.Title");
            openFileDialog.Filter = ResourceManager.GetString("StyleEditor.OpenFileDialog.ListImage.Filter");
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                this.textBoxListStyleImage.Text = System.IO.Path.GetFileName(openFileDialog.FileName);
                // force change after new image from disc
                this.textBox_TextChanged(sender, e);
            }
        }

        private void fontPickerFont_ContentChanged(object sender, System.EventArgs e)
        {
            this.OnContentChanged(sender);
        }

        private void radioButtonListBulletNone_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonListBulletNone.Checked)
            {
                this.comboBoxListStylePosition.SelectedIndex = -1;
                this.comboBoxListStyleType.SelectedIndex = -1;
            }
            OnContentChanged(sender);
        }

        private void radioButtonListBulletStyle_CheckedChanged(object sender, System.EventArgs e)
        {
            if (radioButtonListBulletStyle.Checked)
            {
                this.comboBoxListStylePosition.Enabled = true;
                this.comboBoxListStyleType.Enabled = true;
            }
            else
            {
                this.comboBoxListStylePosition.Enabled = false;
                this.comboBoxListStyleType.Enabled = false;
            }
        }

        private void radioButtonListBulletImage_CheckedChanged(object sender, System.EventArgs e)
        {
            if (radioButtonListBulletImage.Checked)
            {
                this.textBoxListStyleImage.Enabled = true;
                this.buttonListStyleImage.Enabled = true;
            }
            else
            {
                this.textBoxListStyleImage.Clear();
                this.textBox_TextChanged(sender, e);
                this.textBoxListStyleImage.Enabled = false;
                this.buttonListStyleImage.Enabled = false;
            }
        }

        /// <summary>
        /// Synchronizes the four border-style attributes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, System.EventArgs e)
        {
            decimal d1 = this.numericUpDownMarginTop.Value;
            int c1 = this.comboBoxMarginTopUnit.SelectedIndex;
            Color c = this.colorPickerBorderTop.CurrentColor;
            int c2 = this.comboBoxBorderTopStyle.SelectedIndex;
            decimal d2 = this.numericUpDownBorderTopWidth.Value;
            int c3 = this.comboBoxBorderTopWidthUnit.SelectedIndex;
            decimal d3 = this.numericUpDownPaddingTop.Value;
            int c4 = this.comboBoxPaddingTopUnit.SelectedIndex;

            this.colorPickerBorderLeft.CurrentColor = c;
            this.comboBoxMarginLeftUnit.SelectedIndex = c1;
            this.comboBoxBorderLeftStyle.SelectedIndex = c2;
            this.comboBoxBorderLeftWidthUnit.SelectedIndex = c3;
            this.comboBoxPaddingLeftUnit.SelectedIndex = c4;
            this.numericUpDownBorderLeftWidth.Value = d2;
            this.numericUpDownMarginLeft.Value = d1;
            this.numericUpDownPaddingLeft.Value = d3;

            this.colorPickerBorderRight.CurrentColor = c;
            this.comboBoxMarginRightUnit.SelectedIndex = c1;
            this.comboBoxBorderRightStyle.SelectedIndex = c2;
            this.comboBoxBorderRightWidthUnit.SelectedIndex = c3;
            this.comboBoxPaddingRightUnit.SelectedIndex = c4;
            this.numericUpDownMarginRight.Value = d1;
            this.numericUpDownBorderRightWidth.Value = d2;
            this.numericUpDownPaddingRight.Value = d3;

            this.colorPickerBorderBottom.CurrentColor = c;
            this.comboBoxMarginBottomUnit.SelectedIndex = c1;
            this.comboBoxBorderBottomStyle.SelectedIndex = c2;
            this.comboBoxBorderBottomWidthUnit.SelectedIndex = c3;
            this.comboBoxPaddingBottomUnit.SelectedIndex = c4;
            this.numericUpDownMarginBottom.Value = d1;
            this.numericUpDownBorderBottomWidth.Value = d2;
            this.numericUpDownPaddingBottom.Value = d3;

            this.OnContentChanged(sender);
        }

        /// <summary>
        /// This method sets the caret within the numericUpDown fields to simulate the TextBox like
        /// behavior if the user enters the values directly. 
        /// </summary>
        /// <param name="sender">The NumericUpDown control receiving the key event.</param>
        /// <param name="e">The Key pressed.</param>
        private void numericUpDown_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            NumericUpDown nb = sender as System.Windows.Forms.NumericUpDown;
            if (nb == null) return;
            switch (e.KeyCode)
            {
                case Keys.Back:
                    if (nb.Value.ToString().Length == 1)
                    {
                        nb.Value = nb.Minimum;
                        Caret = 0;
                    }
                    else
                    {
                        Caret--;
                    }
                    e.Handled = true;
                    break;
                case Keys.End:
                    Caret = nb.Value.ToString().Length;
                    break;
                case Keys.Home:
                    Caret = 0;
                    break;
                case Keys.Left:
                    Caret = (Caret > 0) ? --Caret : Caret;
                    break;
                case Keys.Right:
                    Caret = (Caret <= nb.Value.ToString().Length) ? ++Caret : Caret;
                    break;
                case Keys.NumPad0:
                case Keys.D0:
                case Keys.NumPad1:
                case Keys.D1:
                case Keys.NumPad2:
                case Keys.D2:
                case Keys.NumPad3:
                case Keys.D3:
                case Keys.NumPad4:
                case Keys.D4:
                case Keys.NumPad5:
                case Keys.D5:
                case Keys.NumPad6:
                case Keys.D6:
                case Keys.NumPad7:
                case Keys.D7:
                case Keys.NumPad8:
                case Keys.D8:
                case Keys.NumPad9:
                case Keys.D9:
                    Caret = (Caret > nb.Maximum.ToString().Length) ? nb.Maximum.ToString().Length : ++Caret;
                    break;
                case Keys.OemPeriod:
                    if (nb.DecimalPlaces > 0)
                    {
                        Caret = (Caret > nb.Maximum.ToString().Length) ? nb.Maximum.ToString().Length : ++Caret;
                    }
                    break;
            }
        }

        /// <summary>
        /// This handler suppresses any resizing of the control during design time.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StyleUserControl_Resize(object sender, System.EventArgs e)
        {
            this.Size = new Size(Math.Max(440, this.Width), Math.Max(448, this.Height));
        }

        #endregion

        # region Public Design Time Properties

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
        /// The style of the border around the control
        /// </summary>
        [Category("NetRix"), Description("The style of the border around the control")]
        [DefaultValue("None")]
        public BorderStyle BorderDesign
        {
            set
            {
                this.Border.BorderStyle = value; 
            }
            get
            {
                return this.Border.BorderStyle;
            }
        }

        /// <summary>
        /// Sets the appereance of the buttons and other controls.
        /// </summary>
        public FlatStyle ButtonAppearance
        {
            set
            {
                this.radioButtonDirectionLtr.FlatStyle = value;
                this.radioButtonDirectionRemove.FlatStyle = value;
                this.radioButtonDirectionRtl.FlatStyle = value;
                this.radioButtonFontSizeAbsolute.FlatStyle = value;
                this.radioButtonFontSizeUnit.FlatStyle = value;
                this.radioButtonListBulletImage.FlatStyle = value;
                this.radioButtonListBulletNone.FlatStyle = value;
                this.radioButtonListBulletStyle.FlatStyle = value;
                this.radioButtonPositionAbsolute.FlatStyle = value;
                this.radioButtonPositionFixed.FlatStyle = value;
                this.radioButtonPositionNone.FlatStyle = value;
                this.radioButtonPositionRelative.FlatStyle = value;
                this.radioButtonPositionRemove.FlatStyle = value;
                this.radioButtonTextAlignCenter.FlatStyle = value;
                this.radioButtonTextAlignDisable.FlatStyle = value;
                this.radioButtonTextAlignJustify.FlatStyle = value;
                this.radioButtonTextAlignLeft.FlatStyle = value;
                this.radioButtonTextAlignRight.FlatStyle = value;
                this.radioButtonVisibilityHidden.FlatStyle = value;
                this.radioButtonVisibilityNone.FlatStyle = value;
                this.radioButtonVisibilityVisible.FlatStyle = value;
                this.radioButtonVisiblityRemove.FlatStyle = value;
                this.button1.FlatStyle = value;
                this.buttonBackgroundImage.FlatStyle = value;
                this.buttonListStyleImage.FlatStyle = value;
                this.checkBox6.FlatStyle = value;
                this.checkBoxFontStyleItalic.FlatStyle = value;
                this.checkBoxFontVariantSmallCaps.FlatStyle = value;
                this.groupBox1.FlatStyle = value;
                this.groupBox2.FlatStyle = value;
                this.groupBox3.FlatStyle = value;
                this.groupBox4.FlatStyle = value;
                this.groupBox5.FlatStyle = value;
                this.groupBox6.FlatStyle = value;
                this.groupBox7.FlatStyle = value;
                this.groupBox8.FlatStyle = value;
                this.groupBox9.FlatStyle = value;
                this.groupBox10.FlatStyle = value;
                this.groupBox11.FlatStyle = value;
                this.groupBox12.FlatStyle = value;
                this.groupBox13.FlatStyle = value;
                this.groupBox14.FlatStyle = value;
                this.groupBox15.FlatStyle = value;
                this.groupBox16.FlatStyle = value;
                this.groupBox17.FlatStyle = value;
                this.groupBox18.FlatStyle = value;
                this.groupBox19.FlatStyle = value;
                this.groupBox20.FlatStyle = value;
                this.groupBox21.FlatStyle = value;
                this.groupBox22.FlatStyle = value;
                this.groupBox23.FlatStyle = value;
                this.groupBox24.FlatStyle = value;
                this.groupBox25.FlatStyle = value;
            }
        }

        /// <summary>
        /// The design of the button controls
        /// </summary>
        [Category("NetRix"), Description("The design of the button controls")]
        public System.Windows.Forms.TabAppearance TabAppearance
        {
            set
            {
                this.tabControlEditor.Appearance = value;
            }
            get
            {
                return this.tabControlEditor.Appearance;
            }
        }

        /// <summary>
        /// Sample String for Font Picker List Box.
        /// </summary>        
        [Browsable(true), Category("NetRix"), Description("Sample String for Font Picker List Box.")]
        public string FontPickerSampleString
        {
            get
            {
                return this.fontPickerFont.SampleString;
            }
            set
            {
                this.fontPickerFont.SampleString = value;
            }
        }

        # endregion

    }
}