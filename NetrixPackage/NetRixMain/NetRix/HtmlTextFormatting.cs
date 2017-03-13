using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.WebEditing.Elements;
using GuruComponents.Netrix.WebEditing.Styles;
using System.Web.UI.WebControls;
using GuruComponents.Netrix.WebEditing.UndoRedo;

# pragma warning disable 0618

namespace GuruComponents.Netrix
{

    /// <summary>
    /// HtmlTextFormatting provides methods to format text selections or text blocks.
    /// </summary>
    /// <remarks>
    /// This class should never called directly from user code. It is necessary to use the 
    /// <see cref="GuruComponents.Netrix.HtmlEditor.TextFormatting"/> property to request a singleton instance
    /// of the class to access the various properties and methods. This assures that all requests are assigned
    /// to the current document only.
    /// <para>
    /// For dealing with an instance of the class it is recommended to use the <see cref="GuruComponents.Netrix.ITextFormatting"/>
    /// interface as type.
    /// </para>
    /// </remarks>
    public class HtmlTextFormatting : ITextFormatting, IDisposable
    {

        protected HtmlEditor editor;

        /// <summary>
        /// Constructor which simply takes an HtmlEditor to interface with MSHTML.
        /// </summary>
        /// <remarks>
        /// This constructor supports the
        /// NetRix infrastructure and should never called directly from user code.
        /// </remarks>
        /// <param name="editor"></param>
        internal HtmlTextFormatting(IHtmlEditor editor)
        {
            this.editor = (HtmlEditor)editor;
        }

        /// <summary>
        /// Gets the block format command strings from MSHTML.
        /// </summary>
        /// <remarks>
        /// The block format command parameters
        /// depend on the MSHTML version and language. For example: The english version accepts "Header1"
        /// whereas the german version needs "Überschrift1", in MSHTML 6.0. It is strongly recommended
        /// to gather the block formats before sending any block formatting command to the editor component.
        /// </remarks>
        /// <returns>Array of format command strings</returns>
        private string[] GetBlockFormats()
        {
            Interop.OLEVARIANT oleVar = new Interop.OLEVARIANT();
            object o;
            string[] fmts;
            this.editor.ExecResult(Interop.IDM.GETBLOCKFMTS, ref oleVar);
            oleVar.vt = Convert.ToInt16(System.Runtime.InteropServices.VarEnum.VT_ARRAY | System.Runtime.InteropServices.VarEnum.VT_BSTR);
            o = oleVar.ToNativeObject();
            oleVar.Clear();
            fmts = (string[])o;
            return fmts;
        }

        /// <summary>
        /// Translate the language dependent block formatting commands into the internal
        /// used enumaration
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        internal string GetFormatString(HtmlFormat format)
        {
            string[] blockFormats = GetBlockFormats();
            return blockFormats[(int)format];
        }

        # region Public Textformatting Properties

        /// <summary>
        /// Selects the whole text in the control
        /// </summary>
        /// <remarks>
        /// If key strokes for hot keys are hardwired this methods is linked with the Ctrl-A key.
        /// </remarks>
        public void SelectAll()
        {
            this.editor.Exec(Interop.IDM.SELECTALL);
        }

        /// <summary>
        /// Removes the selection the user has made. 
        /// </summary>
        /// <remarks>
        /// This method does not delete the content currently selected.
        /// </remarks>
        public void ClearSelection()
        {
            this.editor.Exec(Interop.IDM.CLEARSELECTION);
        }

        /// <summary>
        /// Removes the current inline formatting with &lt;font&gt; tag.
        /// </summary>
        /// <example>
        /// The following code assumes menu item to RemoveInlineFormat and its click event 
        /// is handle like this:
        /// <code>
        /// private void mnuRemoveInlineFormat_Click(object sender, System.EventArgs e)
        /// {
        ///		htmlEditor2.TextFormatting.RemoveInlineFormat();
        /// }
        /// </code>
        /// </example>
        /// <seealso cref="RemoveParagraphFormat"/>
        /// <seealso cref="SetHtmlFormat"/>
        public virtual void RemoveInlineFormat()
        {
            this.editor.Exec(Interop.IDM.REMOVEFORMAT);
        }

        /// <summary>
        /// Removes the current paragraph formatting by any kind of block element.
        /// </summary>
        /// <remarks>
        /// This is a shortcut for <c>SetHtmlFormat(HtmlFormat.Normal)</c>.
        /// </remarks>
        /// <example>
        /// The following code assumes menu item to RemoveInlineFormat and its click event 
        /// is handle like this:
        /// <code>
        /// private void mnuRemoveParagraphFormat_Click(object sender, System.EventArgs e)
        /// {
        ///		htmlEditor2.TextFormatting.RemoveParagraphFormat();
        /// }
        /// </code>
        /// </example>
        /// <seealso cref="RemoveInlineFormat"/>
        /// <seealso cref="SetHtmlFormat"/>
        public void RemoveParagraphFormat()
        {
            this.SetHtmlFormat(HtmlFormat.Normal);
        }

        /// <summary>
        /// Indicates if the current text can be indented.
        /// </summary>
        /// <remarks>
        /// It is recommended to use this property to update the user interface, e.g. disabling or enabling toolbar buttons or menu entries.
        /// </remarks>
        public virtual bool CanIndent
        {
            get
            {
                return editor.IsCommandEnabled(Interop.IDM.INDENT);
            }
        }

        /// <summary>
        /// Indicates if the background color can be set
        /// </summary>
        /// <remarks>
        /// It is recommended to use this property to update the user interface, e.g. disabling or enabling toolbar buttons or menu entries.
        /// </remarks>
        public virtual bool CanSetBackColor
        {
            get
            {
                return editor.IsCommandEnabled(Interop.IDM.BACKCOLOR);
            }
        }

        /// <summary>
        /// Indicates if the font face can be set.
        /// </summary>
        /// <remarks>
        /// It is recommended to use this property to update the user interface, e.g. disabling or enabling toolbar buttons or menu entries.
        /// </remarks>
        public virtual bool CanSetFontName
        {
            get
            {
                return editor.IsCommandEnabled(Interop.IDM.FONTNAME);
            }
        }

        /// <summary>
        /// Indicates if the font size can get set.
        /// </summary>
        /// <remarks>
        /// It is recommended to use this property to update the user interface, e.g. disabling or enabling toolbar buttons or menu entries.
        /// </remarks>
        public virtual bool CanSetFontSize
        {
            get
            {
                return editor.IsCommandEnabled(Interop.IDM.FONTSIZE);
            }
        }

        /// <summary>
        /// Indicates if the foreground color can be set.
        /// </summary>
        /// <remarks>
        /// It is recommended to use this property to update the user interface, e.g. disabling or enabling toolbar buttons or menu entries.
        /// </remarks>
        public virtual bool CanSetForeColor
        {
            get
            {
                return editor.IsCommandEnabled(Interop.IDM.FORECOLOR);
            }
        }

        /// <summary>
        /// Indicates if the Html format (eg ordered lists, paragraph, heading) can be set.
        /// </summary>
        /// <remarks>
        /// It is recommended to use this property to update the user interface, e.g. disabling or enabling toolbar buttons or menu entries.
        /// </remarks>
        public virtual bool CanSetHtmlFormat
        {
            get
            {
                return editor.IsCommandEnabled(Interop.IDM.BLOCKFMT);
            }
        }

        /// <summary>
        /// Indicates if the current text can be unindented.
        /// </summary>
        /// <remarks>
        /// It is recommended to use this property to update the user interface, e.g. disabling or enabling toolbar buttons or menu entries.
        /// </remarks>
        public virtual bool CanUnindent
        {
            get
            {
                return editor.IsCommandEnabled(Interop.IDM.OUTDENT);
            }
        }

        /// <summary>
        /// Gets and sets the font name of the current text.
        /// </summary>
        /// <remarks>
        /// Any call to the set path will remove empty attributes, if the current element is a font element and
        /// the attribute is empty. If no more attributes left the font tag is removed.  
        /// <para>
        /// The intention if this property is NOT to change the font face of any font attribute which is valid in
        /// the parent hierarchy. This means, that only an active selection with one or more characters will
        /// encapsulated with the font tag. To get the current element just use <c>HtmlEditor.GetCurrentElement</c>
        /// or get the current element form HtmlElementChanged event. If the current element is the font
        /// element the property <c>face</c> can be used to set or retrieve the current font face value.
        /// </para>
        /// </remarks>
        public virtual string FontName
        {
            get
            {
                IElement e = editor.GetCurrentElement();
                if (e != null)
                {
                    return e.EffectiveStyle.font.FontFamily;
                }
                else
                {
                    return String.Empty;
                }
            }
            set
            {
                editor.Exec(Interop.IDM.FONTNAME, value);
                ResetFontTag();
            }
        }

        /// <summary>
        /// Gets and sets the font size of the current text.
        /// </summary>
        /// <remarks>
        /// Any call to the set path will remove empty attributes, if the current element is a font element and
        /// the attribute is empty. If no more attributes left the font tag is removed.  
        /// <para>
        /// The intention if this property is NOT to change the font size of any font attribute which is valid in
        /// the parent hierarchy. This means, that only an active selection with one or more characters will
        /// encapsulated with the font tag. To get the current element just use <c>HtmlEditor.GetCurrentElement</c>
        /// or get the current element form HtmlElementChanged event. If the current element is the font
        /// element the property <c>size</c> can be used to set or retrieve the current font size value.
        /// </para>
        /// </remarks>
        public GuruComponents.Netrix.WebEditing.FontUnit FontSize
        {
            get 
            {
                IElement e = editor.GetCurrentElement();
                if (e != null)
                {
                    int fs = Convert.ToInt32(e.EffectiveStyle.font.FontSize.Value);
                    if (fs > 0 && fs < 8)
                    {
                        return (GuruComponents.Netrix.WebEditing.FontUnit)fs;
                    }
                    else 
                    {
                        // CSS Style values have to be transformed
                        return ConvertSizeToUnit(fs);
                    }
                } 
                else 
                {
                    return GuruComponents.Netrix.WebEditing.FontUnit.Empty;
                }
            }
            set 
            {
                if (value != GuruComponents.Netrix.WebEditing.FontUnit.Empty)
				{
                    int val = ConvertUnitToSize(value);
					editor.Exec(Interop.IDM.FONTSIZE, val);
					ResetFontTag();
				}
            }
        }

        public virtual System.Web.UI.WebControls.Unit CssFontSize
        {
            get { throw new NotImplementedException("Use CssTextFormatting class to use this function."); }
            set { throw new NotImplementedException("Use CssTextFormatting class to use this function."); }
        }

        /// <summary>
        /// The foreground color of the current text.
        /// </summary>
        /// <remarks>
        /// Any call to the set path will remove empty attributes, if the current element is a font element and
        /// the attribute is empty. If no more attributes left the font tag is removed.  
        /// <para>
        /// The intention if this property is NOT to change the color of any font attribute which is valid in
        /// the parent hierarchy. This means, that only an active selection with one or more characters will
        /// encapsulated with the font tag. To get the current element just use <c>HtmlEditor.GetCurrentElement</c>
        /// or get the current element form HtmlElementChanged event. If the current element is the font
        /// element the property color can be used to set or retrieve the current color value.
        /// </para>
        /// <seealso cref="GuruComponents.Netrix.HtmlTextFormatting.BackColor">BackColor</seealso>
        /// </remarks>
        /// <example>
        /// Assuming you have a dialog <c>ColorWizard</c> that provides both, ForeColor and BackColor settings,
        /// the following code will change the current selection to the selected colors (C#):
        /// <code>
        ///ColorWizardDlg = new ColorWizard();
        ///ColorWizardDlg.FontForeColor = this.htmlEditor1.TextFormatting.ForeColor;
        ///ColorWizardDlg.FontBackColor = this.htmlEditor1.TextFormatting.BackColor;
        ///if (ColorWizardDlg.ShowDialog() == DialogResult.OK)
        ///{
        ///   this.htmlEditor1.TextFormatting.ForeColor = ColorWizardDlg.FontForeColor;
        ///   this.htmlEditor1.TextFormatting.BackColor = ColorWizardDlg.FontBackColor;
        ///}
        /// </code>
        /// The dialog is best build using the ColorPicker user control from NetRix UI package. The following code
        /// is a good example of how to do this (C#):
        /// <code>
        ///using System;
        ///using System.Drawing;
        ///using System.Collections;
        ///using System.ComponentModel;
        ///using System.Windows.Forms;
        ///
        ///using GuruComponents.Netrix.WebEditing.UserInterface.ColorPicker;
        ///
        ///namespace Comzept.DemoApplication
        ///{
        ///public class ColorWizard : System.Windows.Forms.Form
        ///{
        ///    private ColorPickerUserControl colorPickerUserControlFore;
        ///    private ColorPickerUserControl colorPickerUserControlBack;
        ///    private System.Windows.Forms.Button buttonOK;
        ///    private System.Windows.Forms.Button buttonCancel;
        ///    private System.Windows.Forms.Label labelHeader;
        ///    private System.Windows.Forms.Label labelForeground;
        ///    private System.Windows.Forms.Label labelBackground;
        ///    private System.Windows.Forms.Label labelSample;
        ///    private System.ComponentModel.Container components = null;
        ///
        ///    public ColorWizard()
        ///    {
        ///        InitializeComponent();
        ///    }
        ///
        ///    protected override void Dispose( bool disposing )
        ///    {
        ///        if( disposing )
        ///        {
        ///            if(components != null)
        ///            {
        ///                components.Dispose();
        ///            }
        ///        }
        ///        base.Dispose( disposing );
        ///    }
        ///
        ///    private void InitializeComponent()
        ///    {
        ///        this.labelHeader = new System.Windows.Forms.Label();
        ///        this.colorPickerUserControlFore = new GuruComponents.Netrix.WebEditing.UserInterface.ColorPicker.ColorPickerUserControl();
        ///        this.colorPickerUserControlBack = new GuruComponents.Netrix.WebEditing.UserInterface.ColorPicker.ColorPickerUserControl();
        ///        this.labelForeground = new System.Windows.Forms.Label();
        ///        this.labelBackground = new System.Windows.Forms.Label();
        ///        this.buttonOK = new System.Windows.Forms.Button();
        ///        this.buttonCancel = new System.Windows.Forms.Button();
        ///        this.labelSample = new System.Windows.Forms.Label();
        ///        this.SuspendLayout();
        ///        /// 
        ///        /// labelHeader
        ///        /// 
        ///        this.labelHeader.FlatStyle = System.Windows.Forms.FlatStyle.System;
        ///        this.labelHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
        ///        this.labelHeader.Location = new System.Drawing.Point(8, 8);
        ///        this.labelHeader.Name = "labelHeader";
        ///        this.labelHeader.Size = new System.Drawing.Size(272, 24);
        ///        this.labelHeader.TabIndex = 0;
        ///        this.labelHeader.Text = "Change the currently selected color:";
        ///        /// 
        ///        /// colorPickerUserControlFore
        ///        /// 
        ///        this.colorPickerUserControlFore.CurrentColor = System.Drawing.Color.Empty;
        ///        this.colorPickerUserControlFore.Location = new System.Drawing.Point(8, 48);
        ///        this.colorPickerUserControlFore.Name = "colorPickerUserControlFore";
        ///        this.colorPickerUserControlFore.Size = new System.Drawing.Size(272, 23);
        ///        this.colorPickerUserControlFore.TabIndex = 1;
        ///        this.colorPickerUserControlFore.ColorChanged += new GuruComponents.Netrix.WebEditing.UserInterface.ColorPicker.ColorChangedEventHandler(this.colorPickerUserControlFore_ColorChanged);
        ///        /// 
        ///        /// colorPickerUserControlBack
        ///        /// 
        ///        this.colorPickerUserControlBack.CurrentColor = System.Drawing.Color.Empty;
        ///        this.colorPickerUserControlBack.Location = new System.Drawing.Point(8, 96);
        ///        this.colorPickerUserControlBack.Name = "colorPickerUserControlBack";
        ///        this.colorPickerUserControlBack.Size = new System.Drawing.Size(272, 23);
        ///        this.colorPickerUserControlBack.TabIndex = 2;
        ///        this.colorPickerUserControlBack.ColorChanged += new GuruComponents.Netrix.WebEditing.UserInterface.ColorPicker.ColorChangedEventHandler(this.colorPickerUserControlBack_ColorChanged);
        ///        /// 
        ///        /// labelForeground
        ///        /// 
        ///        this.labelForeground.FlatStyle = System.Windows.Forms.FlatStyle.System;
        ///        this.labelForeground.Location = new System.Drawing.Point(8, 32);
        ///        this.labelForeground.Name = "labelForeground";
        ///        this.labelForeground.Size = new System.Drawing.Size(100, 16);
        ///        this.labelForeground.TabIndex = 3;
        ///        this.labelForeground.Text = "Foreground:";
        ///        /// 
        ///        /// labelBackground
        ///        /// 
        ///        this.labelBackground.FlatStyle = System.Windows.Forms.FlatStyle.System;
        ///        this.labelBackground.Location = new System.Drawing.Point(8, 80);
        ///        this.labelBackground.Name = "labelBackground";
        ///        this.labelBackground.Size = new System.Drawing.Size(100, 16);
        ///        this.labelBackground.TabIndex = 4;
        ///        this.labelBackground.Text = "Background";
        ///        /// 
        ///        /// buttonOK
        ///        /// 
        ///        this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
        ///        this.buttonOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
        ///        this.buttonOK.Location = new System.Drawing.Point(208, 176);
        ///        this.buttonOK.Name = "buttonOK";
        ///        this.buttonOK.TabIndex = 5;
        ///        this.buttonOK.Text = "OK";
        ///        /// 
        ///        /// buttonCancel
        ///        /// 
        ///        this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        ///        this.buttonCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
        ///        this.buttonCancel.Location = new System.Drawing.Point(128, 176);
        ///        this.buttonCancel.Name = "buttonCancel";
        ///        this.buttonCancel.TabIndex = 6;
        ///        this.buttonCancel.Text = "Cancel";
        ///        /// 
        ///        /// labelSample
        ///        /// 
        ///        this.labelSample.BackColor = System.Drawing.SystemColors.ControlLightLight;
        ///        this.labelSample.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
        ///        this.labelSample.Font = new System.Drawing.Font("Verdana", 14F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
        ///        this.labelSample.Location = new System.Drawing.Point(8, 128);
        ///        this.labelSample.Name = "labelSample";
        ///        this.labelSample.Size = new System.Drawing.Size(272, 40);
        ///        this.labelSample.TabIndex = 7;
        ///        this.labelSample.Text = "Worldsoft Color Sample";
        ///        this.labelSample.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        ///        /// 
        ///        /// ColorWizard
        ///        /// 
        ///        this.AcceptButton = this.buttonOK;
        ///        this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
        ///        this.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
        ///        this.CancelButton = this.buttonCancel;
        ///        this.ClientSize = new System.Drawing.Size(290, 208);
        ///        this.Controls.Add(this.labelSample);
        ///        this.Controls.Add(this.buttonCancel);
        ///        this.Controls.Add(this.buttonOK);
        ///        this.Controls.Add(this.labelBackground);
        ///        this.Controls.Add(this.labelForeground);
        ///        this.Controls.Add(this.colorPickerUserControlBack);
        ///        this.Controls.Add(this.colorPickerUserControlFore);
        ///        this.Controls.Add(this.labelHeader);
        ///        this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
        ///        this.MaximizeBox = false;
        ///        this.MinimizeBox = false;
        ///        this.Name = "ColorWizard";
        ///        this.ShowInTaskbar = false;
        ///        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
        ///        this.Text = "Color Wizard";
        ///        this.ResumeLayout(false);
        ///
        ///    }
        ///
        ///    private void colorPickerUserControlFore_ColorChanged(object sender, ColorChangedEventArgs e)
        ///    {
        ///        this.labelSample.ForeColor = e.Color;
        ///    }
        ///
        ///    private void colorPickerUserControlBack_ColorChanged(object sender, ColorChangedEventArgs e)
        ///    {
        ///        this.labelSample.BackColor = e.Color;
        ///    }
        ///
        ///    public Color FontBackColor
        ///    {
        ///        set
        ///        {
        ///            this.colorPickerUserControlBack.CurrentColor = value;
        ///            this.labelSample.BackColor = value;
        ///        }
        ///        get
        ///        {
        ///            return this.colorPickerUserControlBack.CurrentColor;
        ///        }
        ///    }
        ///
        ///    public Color FontForeColor
        ///    {
        ///        set
        ///        {
        ///            this.colorPickerUserControlFore.CurrentColor = value;
        ///            this.labelSample.ForeColor = value;
        ///        }
        ///        get
        ///        {
        ///            return this.colorPickerUserControlFore.CurrentColor;
        ///        }
        ///    }
        ///}
        ///}        
        /// </code>
        /// </example>
        public virtual Color ForeColor
        {
            get
            {
                IElement e = editor.GetCurrentElement();
                if (e != null)
                {
                    return e.EffectiveStyle.color;
                }
                else
                {
                    return Color.Empty;
                }
            }
            set
            {
                //Translate the color and execute the command
                string color = ColorTranslator.ToHtml(value);
                editor.Exec(Interop.IDM.FORECOLOR, color);
                ResetFontTag();
            }
        }

        /// <summary>
        /// Sets the background of the current text selection to the given color.
        /// </summary>
        /// <remarks>
        /// This property cannot be 
        /// applied if no text is selected. 
        /// Any call to the set path will remove empty attributes, if the current element is a font element and
        /// the attribute is empty. If no more attributes left the font tag is removed.  
        /// <para>
        /// The intention if this property is NOT to change the font face of any font attribute which is valid in
        /// the parent hierarchy. This means, that only an active selection with one or more characters will
        /// encapsulated with the font tag. To get the current element just use <code>HtmlEditor.GetCurrentElement</code>
        /// or get the current element form HtmlElementChanged event. If the current element is the font
        /// element the style attribute <code>background-color</code> can be used.
        /// </para>
        /// <para>
        /// For an example see <see cref="GuruComponents.Netrix.HtmlTextFormatting.ForeColor">ForeColor</see> property.
        /// </para>
        /// <seealso cref="GuruComponents.Netrix.HtmlTextFormatting.ForeColor">ForeColor</seealso>
        /// </remarks>
        public virtual Color BackColor
        {
            get
            {
                //Query MSHTML, convert the result, and return the color
                //int i = editor.ExecResultInt(Interop.IDM.BACKCOLOR);
                //return ConvertMSHTMLColor(i);
                IElement e = editor.GetCurrentElement();
                return e.EffectiveStyle.background.Color;
            }
            set
            {
                //Translate the color and execute the command
                string color = ColorTranslator.ToHtml(value);
                editor.Exec(Interop.IDM.BACKCOLOR, color);
                ResetFontTag();
            }
        }

        /// <summary>
        /// Gets the current state of the bold command (enabled and/or checked).
        /// </summary>
        /// <remarks>
        /// The state depends on the current caret position and may vary if the caret moves. It refers to 
        /// the allowed commands and does not check if issuing the command make sense. Therefore setting
        /// bold inside a tag that is already bold, doesn't make any sense. But it will result in valid
        /// HTML and therefore issuing the command wil allowed. It is up the host application to build a 
        /// more clever user interface by checking the parent/child hierachies.
        /// </remarks>
        /// <returns>Returns a value of type <see cref="GuruComponents.Netrix.HtmlCommandInfo">HtmlCommandInfo</see>.</returns>
        public virtual HtmlCommandInfo GetBoldInfo()
        {
            return editor.GetCommandInfo(Interop.IDM.BOLD);
        }

        /// <summary>
        /// Retrieves the current block format</summary>
        /// <remarks>
        /// The block format can be "Header 1", "Unordered List", etc. and the method returns
        /// it in the internal enum format to stay language independent.
        /// <seealso cref="GuruComponents.Netrix.HtmlFormat">HtmlFormat</seealso>
        /// </remarks>
        /// <returns><see cref="GuruComponents.Netrix.HtmlFormat">HtmlFormat</see> enumeration</returns>
        public virtual HtmlFormat GetHtmlFormat()
        {
            Interop.OLEVARIANT oleVar = new Interop.OLEVARIANT();
            editor.CommandTarget.Exec(ref Interop.Guid_MSHTML, (int)Interop.IDM.BLOCKFMT, (int)Interop.OLECMDEXECOPT.OLECMDEXECOPT_DONTPROMPTUSER, null, oleVar);
            oleVar.vt = Convert.ToInt16(System.Runtime.InteropServices.VarEnum.VT_BSTR);
            object o = oleVar.ToNativeObject();
            oleVar.Clear();
            if (o is string)
            {
                string formatString = o.ToString();
                string[] formats = GetBlockFormats();
                for (int i = 0; i < formats.Length; i++)
                {
                    if (formatString.Equals(formats[i]))
                    {
                        return (HtmlFormat)i;
                    }
                }
            }
            return HtmlFormat.Normal;
        }

        /// <summary>
        /// Gets the current state of the italics command (enabled and/or checked).
        /// </summary>
        /// <remarks>
        /// The state depends on the current caret position and may vary if the caret moves. It refers to 
        /// the allowed commands and does not check if issuing the command make sense. Therefore setting
        /// bold inside a tag that is already bold, doesn't make any sense. But it will result in valid
        /// HTML and therefore issuing the command wil allowed. It is up the host application to build a 
        /// more clever user interface by checking the parent/child hierachies.
        /// </remarks>
        /// <returns>Returns a value of type <see cref="GuruComponents.Netrix.HtmlCommandInfo">HtmlCommandInfo</see>.</returns>
        public virtual HtmlCommandInfo GetItalicsInfo()
        {
            return editor.GetCommandInfo(Interop.IDM.ITALIC);
        }

        /// <summary>
        /// Gets the current state of the strikethrough command (enabled and/or checked).
        /// </summary>
        /// <remarks>
        /// The state depends on the current caret position and may vary if the caret moves. It refers to 
        /// the allowed commands and does not check if issuing the command make sense. Therefore setting
        /// bold inside a tag that is already bold, doesn't make any sense. But it will result in valid
        /// HTML and therefore issuing the command wil allowed. It is up the host application to build a 
        /// more clever user interface by checking the parent/child hierachies.
        /// </remarks>
        /// <returns>Returns a value of type <see cref="GuruComponents.Netrix.HtmlCommandInfo">HtmlCommandInfo</see>.</returns>
        public virtual HtmlCommandInfo GetStrikethroughInfo()
        {
            return editor.GetCommandInfo(Interop.IDM.STRIKETHROUGH);
        }

        /// <summary>
        /// Gets the current state of the Subscript command (enabled and/or checked). This method can only
        /// detect the state if some text is selected. 
        /// </summary>
        /// <remarks>
        /// The state depends on the current caret position and may vary if the caret moves. It refers to 
        /// the allowed commands and does not check if issuing the command make sense. Therefore setting
        /// bold inside a tag that is already bold, doesn't make any sense. But it will result in valid
        /// HTML and therefore issuing the command wil allowed. It is up the host application to build a 
        /// more clever user interface by checking the parent/child hierachies.
        /// </remarks>
        /// <returns>Returns a value of type <see cref="GuruComponents.Netrix.HtmlCommandInfo">HtmlCommandInfo</see>.</returns>
        public virtual HtmlCommandInfo GetSubscriptInfo()
        {
            return editor.GetCommandInfo(Interop.IDM.SUBSCRIPT);
        }

        /// <summary>
        /// Gets the current state of the Superscript command (enabled and/or checked).
        /// </summary>
        /// <remarks>
        /// The state depends on the current caret position and may vary if the caret moves. It refers to 
        /// the allowed commands and does not check if issuing the command make sense. Therefore setting
        /// bold inside a tag that is already bold, doesn't make any sense. But it will result in valid
        /// HTML and therefore issuing the command wil allowed. It is up the host application to build a 
        /// more clever user interface by checking the parent/child hierachies.
        /// </remarks>
        /// <returns>Returns a value of type <see cref="GuruComponents.Netrix.HtmlCommandInfo">HtmlCommandInfo</see>.</returns>
        public virtual HtmlCommandInfo GetSuperscriptInfo()
        {
            return editor.GetCommandInfo(Interop.IDM.SUPERSCRIPT);
        }

        /// <summary>
        /// Gets the current state of the Underline command (enabled and/or checked).
        /// </summary>
        /// <remarks>
        /// The state depends on the current caret position and may vary if the caret moves. It refers to 
        /// the allowed commands and does not check if issuing the command make sense. Therefore setting
        /// bold inside a tag that is already bold, doesn't make any sense. But it will result in valid
        /// HTML and therefore issuing the command wil allowed. It is up the host application to build a 
        /// more clever user interface by checking the parent/child hierachies.
        /// </remarks>
        /// <returns>Returns a value of type <see cref="GuruComponents.Netrix.HtmlCommandInfo">HtmlCommandInfo</see>.</returns>
        public virtual HtmlCommandInfo GetUnderlineInfo()
        {
            return editor.GetCommandInfo(Interop.IDM.UNDERLINE);
        }

        /// <summary>
        /// Sets the HTML format (eg ordered list, paragraph, etc.) of the current text.
        /// </summary>
        /// <example>
        /// You may have a click handler for a tool bar. Then the following example shows how to use the
        /// Tag property of a ToolBarItem from the SyncFusion UI package:
        /// <code>
        ///private void barItemFormat_Click(object sender, System.EventArgs e)
        ///{
        ///    Syncfusion.Windows.Forms.Tools.XPMenus.BarItem barItem = (Syncfusion.Windows.Forms.Tools.XPMenus.BarItem) sender;
        ///    switch (barItem.Tag.ToString())
        ///    {
        ///        case "bold":
        ///            this.htmlEditor1.TextFormatting.ToggleBold();
        ///            break;
        ///        case "italic":
        ///            this.htmlEditor1.TextFormatting.ToggleItalics();
        ///            break;
        ///        case "underline":
        ///            this.htmlEditor1.TextFormatting.ToggleUnderline();
        ///            break;
        ///        case "superscript":
        ///            this.htmlEditor1.TextFormatting.ToggleSuperscript();
        ///            break;
        ///        case "subscript":
        ///            this.htmlEditor1.TextFormatting.ToggleSubscript();
        ///            break;
        ///        case "strikethrough":
        ///            this.htmlEditor1.TextFormatting.ToggleStrikethrough();
        ///            break;
        ///        case "indent":
        ///            this.htmlEditor1.TextFormatting.Indent();
        ///            break;
        ///        case "outdent":
        ///            this.htmlEditor1.TextFormatting.UnIndent();
        ///            break;
        ///        case "forecolor":
        ///            ColorWizardDlg = new ColorWizard();
        ///            ColorWizardDlg.FontForeColor = this.htmlEditor1.TextFormatting.ForeColor;
        ///            ColorWizardDlg.FontBackColor = this.htmlEditor1.TextFormatting.BackColor;
        ///            if (ColorWizardDlg.ShowDialog() == DialogResult.OK)
        ///            {
        ///                this.htmlEditor1.TextFormatting.ForeColor = ColorWizardDlg.FontForeColor;
        ///                this.htmlEditor1.TextFormatting.BackColor = ColorWizardDlg.FontBackColor;
        ///            }
        ///            break;
        ///        case "fontname":
        ///            FontWizardDlg = new FontWizard();
        ///            FontWizardDlg.FontName = this.htmlEditor1.TextFormatting.FontName;
        ///            FontWizardDlg.FontSize = this.htmlEditor1.TextFormatting.FontSize;
        ///            if (FontWizardDlg.ShowDialog() == DialogResult.OK)
        ///            {
        ///                this.htmlEditor1.TextFormatting.FontName = FontWizardDlg.FontName;
        ///                this.htmlEditor1.TextFormatting.FontSize = FontWizardDlg.FontSize;
        ///            }
        ///            break;
        ///        case "fontsize":
        ///            ///
        ///            break;
        ///        case "justifyleft":
        ///            this.htmlEditor1.TextFormatting.SetAlignment(Alignment.Left);
        ///            break;
        ///        case "justifycenter":
        ///            this.htmlEditor1.TextFormatting.SetAlignment(Alignment.Center);
        ///            break;
        ///        case "justifyright":
        ///            this.htmlEditor1.TextFormatting.SetAlignment(Alignment.Right);
        ///            break;
        ///        case "justifyfull":
        ///            this.htmlEditor1.TextFormatting.SetAlignment(Alignment.Full);
        ///            break;
        ///        case "orderlist":
        ///            this.htmlEditor1.Document.CreateOrderedList();
        ///            break;
        ///        case "unorderlist":
        ///            this.htmlEditor1.Document.CreateUnorderedList();
        ///            break;
        ///            /// TABLE
        ///        case "insertrow":
        ///            this.htmlEditor1.TableFormatter.InsertTableRow();
        ///            break;
        ///        case "insertcolumn":
        ///            this.htmlEditor1.TableFormatter.InsertTableColumn();
        ///            break;                    
        ///        case "deleterow":
        ///            this.htmlEditor1.TableFormatter.DeleteTableRow();
        ///            break;
        ///        case "deletecolumn":
        ///            this.htmlEditor1.TableFormatter.DeleteTableColumn();
        ///            break;                    
        ///        case "deletecell":
        ///            this.htmlEditor1.TableFormatter.DeleteCell();
        ///            break;
        ///        case "insertcell":
        ///            this.htmlEditor1.TableFormatter.InsertCell();
        ///            break;                    
        ///        case "mergecells":
        ///            this.htmlEditor1.TableFormatter.MergeCells();
        ///            break;
        ///        case "mergeleft":
        ///            this.htmlEditor1.TableFormatter.MergeLeft();
        ///            break;
        ///        case "mergeright":
        ///            this.htmlEditor1.TableFormatter.MergeRight();
        ///            break;
        ///        case "mergetop":
        ///            this.htmlEditor1.TableFormatter.MergeUp();
        ///            break;
        ///        case "mergebottom":
        ///            this.htmlEditor1.TableFormatter.MergeDown();
        ///            break;
        ///        case "splitcellvertical":
        ///            this.htmlEditor1.TableFormatter.SplitVertical();
        ///            break;
        ///        case "splitcellhorizontal":
        ///            this.htmlEditor1.TableFormatter.SplitHorizontal();
        ///            break;
        ///            /// PARA
        ///        case "h1":
        ///            this.htmlEditor1.TextFormatting.SetHtmlFormat(HtmlFormat.Heading1);
        ///            break;
        ///        case "h2":
        ///            this.htmlEditor1.TextFormatting.SetHtmlFormat(HtmlFormat.Heading2);
        ///            break;
        ///        case "h3":
        ///            this.htmlEditor1.TextFormatting.SetHtmlFormat(HtmlFormat.Heading3);
        ///            break;
        ///        case "h4":
        ///            this.htmlEditor1.TextFormatting.SetHtmlFormat(HtmlFormat.Heading4);
        ///            break;
        ///        case "h5":
        ///            this.htmlEditor1.TextFormatting.SetHtmlFormat(HtmlFormat.Heading5);
        ///            break;
        ///        case "h6":
        ///            this.htmlEditor1.TextFormatting.SetHtmlFormat(HtmlFormat.Heading6);
        ///            break;
        ///        case "paragraph":
        ///            this.htmlEditor1.TextFormatting.SetHtmlFormat(HtmlFormat.Paragraph);
        ///            break;
        ///        case "removeformat":
        ///            this.htmlEditor1.TextFormatting.SetHtmlFormat(HtmlFormat.Normal);
        ///            break;
        ///        case "preformatted":
        ///            this.htmlEditor1.TextFormatting.SetHtmlFormat(HtmlFormat.Formatted);
        ///            break;
        ///    }
        ///}
        /// </code>
        /// To use other packages like Infragistics just replace the first line of the method to get the Tag property
        /// of your favorite ToolBar.
        /// <para>
        /// To use the .NET standard menu items you can still use the toolbar, but you must set your own class, derive
        /// from the internal one and add the missing <c>Tag</c> property. Then write the values of the case branches
        /// in that property of each BarItem or ToolButton and attach the method as click handler.
        /// </para>
        /// </example>
        /// <param name="format">Formatting instruction from <see cref="GuruComponents.Netrix.HtmlFormat">HtmlFormat</see> enumeration.</param>
        public virtual void SetHtmlFormat(HtmlFormat format)
        {
            editor.Exec(Interop.IDM.BLOCKFMT, GetFormatString(format));
        }

        /// <summary>
        /// This methods maps the aligment enumeration used from host application to
        /// the internally used IDM commands.
        /// </summary>
        /// <param name="alignment">Aligment value. <see cref="GuruComponents.Netrix.Alignment"/></param>
        /// <returns>IDM command or -1 in case of unrecognized enum value</returns>
        protected virtual Interop.IDM MapAlignment(Alignment alignment)
        {
            switch (alignment)
            {
                case Alignment.Left:
                    return Interop.IDM.JUSTIFYLEFT;
                case Alignment.Right:
                    return Interop.IDM.JUSTIFYRIGHT;
                case Alignment.Center:
                    return Interop.IDM.JUSTIFYCENTER;
                case Alignment.Full:
                    return Interop.IDM.JUSTIFYFULL;
                case Alignment.None:
                    return Interop.IDM.JUSTIFYNONE;
                default:
                    return Interop.IDM.UNKNOWN;
            }
        }

        /// <summary>
        /// Set the whole document to LTR (left to right) or RTL (right to left) mode.
        /// </summary>
        /// <remarks>
        /// RTL is used for languages primarily running from right to left, like arabic,
        /// whereas LTR is used for western languages.
        /// </remarks>
        /// <param name="rtl">True set RTL, false LTR. Both cases set the attribute.</param>
        public void DirectionRtlDocument(bool rtl)
        {
            if (rtl)
            {
                this.editor.Exec(Interop.IDM.DIRRTL);
            }
            else
            {
                this.editor.Exec(Interop.IDM.DIRLTR);
            }
        }
        /// <summary>
        /// Set a block (paragraph, header) to LTR (left to right) or RTL (right to left) mode.
        /// </summary>
        /// <remarks>
        /// RTL is used for languages primarily running from right to left, like arabic,
        /// whereas LTR is used for western languages.
        /// </remarks>
        /// <param name="rtl">True set RTL, false LTR. Both cases set the attribute.</param>
        public void DirectionRtlBlock(bool rtl)
        {
            if (rtl)
            {
                this.editor.Exec(Interop.IDM.BLOCKDIRRTL);
            }
            else
            {
                this.editor.Exec(Interop.IDM.BLOCKDIRLTR);
            }
        }
        /// <summary>
        /// Set an inline element to LTR (left to right) or RTL (right to left) mode.
        /// </summary>
        /// <remarks>
        /// RTL is used for languages primarily running from right to left, like arabic,
        /// whereas LTR is used for western languages.
        /// </remarks>
        /// <param name="rtl">True set RTL, false LTR. Both cases set the attribute.</param>
        public void DirectionRtlInline(bool rtl)
        {
            if (rtl)
            {
                this.editor.Exec(Interop.IDM.INLINEDIRRTL);
            }
            else
            {
                this.editor.Exec(Interop.IDM.INLINEDIRLTR);
            }
        }

        /// <summary>
        /// Align the current block selection or current block.
        /// </summary>
        /// <remarks>Full alignment justifies the text.</remarks>
        /// <param name="alignment"></param>
        public virtual void SetAlignment(Alignment alignment)
        {
            // first look whether we have an element where the style is already set
            Interop.IDisplayServices ds = (Interop.IDisplayServices)editor.GetActiveDocument(false);
            Interop.IDisplayPointer dp;
            Interop.IHTMLCaret caret;
            ds.CreateDisplayPointer(out dp);
            ds.GetCaret(out caret);
            caret.MoveDisplayPointerToCaret(dp);
            Interop.POINT pp = new Interop.POINT();
            caret.GetLocation(ref pp, false);
            Interop.IHTMLElement el = editor.GetActiveDocument(false).ElementFromPoint(pp.x, pp.y);
            string ta = el.GetStyle().GetTextAlign();
            if (!String.IsNullOrEmpty(ta))
            {
                switch (alignment)
                {
                    case Alignment.Left:
                        el.GetStyle().SetTextAlign("left");
                        break;
                    case Alignment.Right:
                        el.GetStyle().SetTextAlign("right");
                        break;
                    case Alignment.Center:
                        el.GetStyle().SetTextAlign("center");
                        break;
                    case Alignment.Full:
                        el.GetStyle().SetTextAlign("fulle");
                        break;
                    default:
                        el.GetStyle().RemoveAttribute("text-align", 0);
                        break;
                }
            }
            else
            {
                editor.Exec(MapAlignment(alignment));
            }
        }

        /// <summary>
        /// Get the current Alignment of the paragraph where the caret resides.
        /// </summary>
        /// <returns>A value of the Alignment enumeration.</returns>
        public virtual Alignment GetAlignment()
        {
            if (this.editor.GetCommandInfo(Interop.IDM.JUSTIFYLEFT) == HtmlCommandInfo.Both
                ||
                this.editor.GetCommandInfo(Interop.IDM.JUSTIFYLEFT) == HtmlCommandInfo.Checked)
            {
                return Alignment.Left;
            }
            if (this.editor.GetCommandInfo(Interop.IDM.JUSTIFYCENTER) == HtmlCommandInfo.Both
                ||
                this.editor.GetCommandInfo(Interop.IDM.JUSTIFYCENTER) == HtmlCommandInfo.Checked)
            {
                return Alignment.Center;
            }
            if (this.editor.GetCommandInfo(Interop.IDM.JUSTIFYRIGHT) == HtmlCommandInfo.Both
                ||
                this.editor.GetCommandInfo(Interop.IDM.JUSTIFYRIGHT) == HtmlCommandInfo.Checked)
            {
                return Alignment.Right;
            }
            if (this.editor.GetCommandInfo(Interop.IDM.JUSTIFYFULL) == HtmlCommandInfo.Both
                ||
                this.editor.GetCommandInfo(Interop.IDM.JUSTIFYFULL) == HtmlCommandInfo.Checked)
            {
                return Alignment.Full;
            }
            // no answer, just investigate the current style form caret pos
            Interop.IDisplayServices ds = (Interop.IDisplayServices)editor.GetActiveDocument(false);
            Interop.IDisplayPointer dp;
            Interop.IHTMLCaret caret;
            ds.CreateDisplayPointer(out dp);
            ds.GetCaret(out caret);
            caret.MoveDisplayPointerToCaret(dp);
            Interop.POINT pp = new Interop.POINT();
            caret.GetLocation(ref pp, false);
            Interop.IHTMLElement el = editor.GetActiveDocument(false).ElementFromPoint(pp.x, pp.y);
            if (el != null)
            {
                string ta = ((Interop.IHTMLElement2)el).GetCurrentStyle().textAlign;
                switch (ta)
                {
                    case "left":
                        return Alignment.Left;
                    case "right":
                        return Alignment.Right;
                    case "center":
                        return Alignment.Center;
                    case "justify":
                        return Alignment.Full;
                    default:
                        return Alignment.None;
                }
            }
            else
            {
                return Alignment.None;
            }
        }

        /// <summary>
        /// Toggles the current selection with the bold formatting (STRONG tag).
        /// </summary>
        /// <remarks>
        /// This applies always to selected text only or inserts an empty tag.
        /// </remarks>
        /// <example>
        /// The following code assumes you have a toolbar and the <c>Tag</c> property set properly.
        /// The event handler simply checks the Tag property and calls the toggle methods accordingly:
        /// <code>
        /// private void toolBarFormatting_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
        /// {
        ///    ToolBarButton tb = e.Button;
        ///    switch (tb.Tag.ToString())
        ///    {
        ///        case "B":
        ///        this.htmlEditor1.TextFormatting.ToggleBold();
        ///        break;
        ///        case "U":
        ///        this.htmlEditor1.TextFormatting.ToggleUnderline();
        ///        break;
        ///        case "I":
        ///        this.htmlEditor1.TextFormatting.ToggleItalics();
        ///        break;
        ///    }
        /// }
        /// </code>
        /// </example>
        public virtual void ToggleBold()
        {
            editor.Exec(Interop.IDM.BOLD);
        }

        /// <summary>
        /// Toggles the current selection with the italic formatting (EM tag).
        /// </summary>
        /// <remarks>
        /// This applies always to selected text only or inserts an empty tag.
        /// </remarks>
        /// <example>
        /// The following code assumes you have a toolbar and the <c>Tag</c> property set properly.
        /// The event handler simply checks the Tag property and calls the toggle methods accordingly:
        /// <code>
        /// private void toolBarFormatting_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
        /// {
        ///    ToolBarButton tb = e.Button;
        ///    switch (tb.Tag.ToString())
        ///    {
        ///        case "B":
        ///        this.htmlEditor1.TextFormatting.ToggleBold();
        ///        break;
        ///        case "U":
        ///        this.htmlEditor1.TextFormatting.ToggleUnderline();
        ///        break;
        ///        case "I":
        ///        this.htmlEditor1.TextFormatting.ToggleItalics();
        ///        break;
        ///    }
        /// }
        /// </code>
        /// This makes the buttons work, but they still do not appear pushed-on, when the caret is inside a bold or italic tag.
        /// <para>
        /// To checkout the current position add an event handler for SelectionChanged events. If you have already done this using the PropertyGrid, it is not necessary to use an additional handler:
        /// </para>
        /// <code>
        /// this.htmlEditor1.Selection.SelectionChanged += new  GuruComponents.NetrixEvents.SelectionChangedEventHandler (Selection_SelectionChanged);
        /// </code>
        /// Using the current element will work under most circumstances, but is not yet perfected. If a complete paragraph is 
        /// bold and somewhere inside is an italic formatting, this method will recognize the italic element and “ignore” 
        /// the bold one. The toggle button for bold will then not appear pushed-on, but clicking it will remove the 
        /// current bold formatting, which is the exact opposite operation to the one that is expected.
        /// <para>
        /// To do it the right way, you need to traverse the parent tree beginning from the current element. This is easy, because the component does feature the correct methods for this operation.
        /// </para>
        /// <para>
        /// The event handler checks for some tags currently under the caret and sets the buttons after entering such element:
        /// </para>
        /// <code>
        /// private void Selection_SelectionChanged(object sender, GuruComponents.NetrixEvents.SelectionChangedEventArgs e)
        /// {
        ///   // some other stuff here
        ///   this.toolBarButtonB.Pushed = false;
        ///   this.toolBarButtonI.Pushed = false;
        ///   this.toolBarButtonU.Pushed = false;
        ///   // Build element collection from current element in
        ///   ElementCollection ec = this.htmlEditor1.Selection.GetParentHierarchy(element);
        ///   // Add current element to the first position
        ///   ec.Insert(0, element);
        ///   // traverse the hierarchy to check out which buttons should set
        ///   foreach (IElement el in ec)
        ///   {
        ///     switch (el.TagName)
        ///     {
        ///         case "STRONG":
        ///         case "B":
        ///             this.toolBarButtonB.Pushed = true;
        ///             break;
        ///         case "EM":
        ///         case "I":
        ///             this.toolBarButtonI.Pushed = true;
        ///             break;
        ///         case "U":
        ///             this.toolBarButtonU.Pushed = true;
        ///             break;
        ///     }
        ///   }
        /// }
        /// </code>
        /// The method works this way: First, it releases all buttons. Then it checks for the current parent hierarchy and 
        /// adds the current element to the first position of the hierarchy. Next, a simple loop goes through the collection 
        /// and if an element associated with a button is found, the button appearance changes to pushed-in. As you can see, 
        /// this method is somewhat fuzzy against the elements. Even if the ToggleBold method sets &lt;STRONG&gt; tags, it 
        /// can check out for &lt;B&gt;. Using the toggle method again will replace the &lt;B&gt; tag with &lt;STRONG&gt;. The same happens with &lt;I&gt; and &lt;EM&gt;. Internally &lt;EM&gt; is the preferred tag.
        /// </example>
        public virtual void ToggleItalics()
        {
            editor.Exec(Interop.IDM.ITALIC);
        }

        /// <summary>
        /// Toggles the current selection with the strikethrough formatting (STRIKE tag).
        /// </summary>
        /// <remarks>
        /// This applies always to selected text only or inserts
        /// an empty tag.
        /// <para>
        /// For an in-depth explanation of using the toggle commands see 
        /// <see cref="GuruComponents.Netrix.HtmlTextFormatting.ToggleBold">ToggleBold</see>.
        /// </para>
        /// </remarks>
        public virtual void ToggleStrikethrough()
        {
            editor.Exec(Interop.IDM.STRIKETHROUGH);
        }

        /// <summary>
        /// Toggles the current selection with the subscript formatting (SUB tag).
        /// </summary>
        /// <remarks>
        /// This applies always to selected text only or inserts
        /// an empty tag.
        /// <para>
        /// For an in-depth explanation of using the toggle commands see 
        /// <see cref="GuruComponents.Netrix.HtmlTextFormatting.ToggleBold">ToggleBold</see>.
        /// </para>
        /// </remarks>
        public virtual void ToggleSubscript()
        {
            editor.Exec(Interop.IDM.SUBSCRIPT);
        }

        /// <summary>
        /// Toggles the current selection with the superscript formatting (SUP tag).
        /// </summary>
        /// <remarks>
        /// This applies always to selected text only or inserts
        /// an empty tag.
        /// <para>
        /// For an in-depth explanation of using the toggle commands see 
        /// <see cref="GuruComponents.Netrix.HtmlTextFormatting.ToggleBold">ToggleBold</see>.
        /// </para>
        /// </remarks>
        public virtual void ToggleSuperscript()
        {
            editor.Exec(Interop.IDM.SUPERSCRIPT);
        }

        /// <summary>
        /// Toggles the current selection with the underline formatting (U tag).
        /// </summary>
        /// <remarks>
        /// This applies always to selected text only or inserts
        /// an empty tag.
        /// <para>
        /// For an in-depth explanation of using the toggle commands see 
        /// <see cref="GuruComponents.Netrix.HtmlTextFormatting.ToggleBold">ToggleBold</see>.
        /// </para>
        /// </remarks>
        public virtual void ToggleUnderline()
        {
            editor.Exec(Interop.IDM.UNDERLINE);
        }

        /// <summary>
        /// Indents the current text.
        /// </summary>
        /// <remarks>
        /// This applies always to the whole paragraph, or if in an ordered or unorderlist, for the current bullet.
        /// </remarks>
        public virtual void Indent()
        {
            editor.Exec(Interop.IDM.INDENT);
        }

        /// <summary>
        /// Unindents the current text.
        /// </summary>
        /// <remarks>
        /// This applies always to the whole paragraph. If there is no outdent
        /// level is available the command will be ignored.
        /// </remarks>
        public virtual void UnIndent()
        {
            editor.Exec(Interop.IDM.OUTDENT);
        }

        # endregion

        # region Static Helpers

        /// <summary>
        /// Converts an MSHTML color to a Framework color.
        /// </summary>
        /// <remarks>
        ///  Internally it is COLORREF of type DWORD.
        /// </remarks>
        /// <param name="colorValue">object colorValue - The color value returned from MSHTML</param>
        /// <returns>Color which the parameter represents</returns>
        private static Color ConvertMSHTMLColor(object colorValue)
        {
            if (colorValue != null)
            {
                Type colorType = colorValue.GetType();
                if (colorType == typeof(int))
                {
                    //If the colorValue is an int, it's a Win32 color
                    return ColorTranslator.FromWin32((int)colorValue);
                }
                else if (colorType == typeof(string))
                {
                    //Otherwise, it's a string, so convert that
                    return ColorTranslator.FromHtml((string)colorValue);
                }
                Debug.WriteLine("Unexpected color type : " + colorType.FullName);
            }
            return Color.Empty;
        }

        public static GuruComponents.Netrix.WebEditing.FontUnit ConvertSizeToUnit(float size)
        {
            return ConvertSizeToUnit((int)size);
        }

        public static GuruComponents.Netrix.WebEditing.FontUnit ConvertSizeToUnit(int size)
        {
            switch (size)
            {
                case 8:
                case 9:
                    return GuruComponents.Netrix.WebEditing.FontUnit.XXSmall;
                case 10:
                case 11:
                    return GuruComponents.Netrix.WebEditing.FontUnit.XSmall;
                case 12:
                case 13:
                    return GuruComponents.Netrix.WebEditing.FontUnit.Small;
                case 14:
                case 15:
                case 16:
                    return GuruComponents.Netrix.WebEditing.FontUnit.Medium;
                case 17:
                case 18:
                case 19:
                    return GuruComponents.Netrix.WebEditing.FontUnit.Large;
                case 20:
                case 21:
                case 22:
                case 23:
                    return GuruComponents.Netrix.WebEditing.FontUnit.XLarge;
                case 24:
                    return GuruComponents.Netrix.WebEditing.FontUnit.XXLarge;
            }
            return GuruComponents.Netrix.WebEditing.FontUnit.Empty;
        }

        public static int ConvertUnitToPoint(GuruComponents.Netrix.WebEditing.FontUnit unit)
        {
            switch (unit)
            {
                case GuruComponents.Netrix.WebEditing.FontUnit.XXSmall:
                    return 8;
                case GuruComponents.Netrix.WebEditing.FontUnit.XSmall:
                    return 10;
                case GuruComponents.Netrix.WebEditing.FontUnit.Small:
                    return 12;
                case GuruComponents.Netrix.WebEditing.FontUnit.Medium:
                    return 14;
                case GuruComponents.Netrix.WebEditing.FontUnit.Large:
                    return 18;
                case GuruComponents.Netrix.WebEditing.FontUnit.XLarge:
                    return 20;
                case GuruComponents.Netrix.WebEditing.FontUnit.XXLarge:
                    return 24;
            }
            return 0;
        }

        public static int ConvertUnitToSize(GuruComponents.Netrix.WebEditing.FontUnit unit)
        {
            int val = 0;
            switch ((int)unit)
            {
                case 100:
                    val = -1;
                    break;
                case 101:
                    val = +1;
                    break;
                default:
                    val = (int)unit;
                    break;
            }
            return val;
        }

        protected void ResetFontTag()
        {
            IElement e = editor.GetCurrentElement();
            FontElement fontElement = e as GuruComponents.Netrix.WebEditing.Elements.FontElement;
            if (fontElement != null)
            {
                if (fontElement.color == Color.Empty
                    &&
                    fontElement.size == GuruComponents.Netrix.WebEditing.FontUnit.Empty
                    &&
                    fontElement.face.Equals(String.Empty)
                    &&
                    (fontElement.style != null && fontElement.style.IndexOf("BACKGROUND-COLOR") == -1)
                    )
                {
                    fontElement.ElementDom.RemoveElement(false);
                    //editor.Selection.RemoveCurrentElement(true);	
                }
                else
                {
                    if (fontElement.style != null && fontElement.style.IndexOf("BACKGROUND-COLOR") == -1)
                    {
                        fontElement.RemoveStyleAttribute("BACKGROUND-COLOR");
                    }
                    if (fontElement.color == Color.Empty)
                    {
                        fontElement.RemoveAttribute("color");
                    }
                    if (fontElement.size == GuruComponents.Netrix.WebEditing.FontUnit.Empty)
                    {
                        fontElement.RemoveAttribute("size");
                    }
                    if (fontElement.face.Equals(String.Empty))
                    {
                        fontElement.RemoveAttribute("face");
                    }
                }
            }
        }

        # endregion

        #region IDisposable Members

        public void Dispose()
        {

        }

        #endregion
    }
}