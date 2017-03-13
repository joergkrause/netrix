using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.WebEditing.Elements;
using GuruComponents.Netrix.WebEditing.Styles;
using System.Web.UI.WebControls;
using GuruComponents.Netrix.WebEditing.UndoRedo;

namespace GuruComponents.Netrix
{

    /// <summary>
    /// CssTextFormatting provides methods to format text selections or text blocks using CSS inline styles.
    /// </summary>
    /// <remarks>
    /// This class should never called directly from user code. It is necessary to use the 
    /// <see cref="GuruComponents.Netrix.HtmlEditor.CssTextFormatting"/> property to request a singleton instance
    /// of the class to access the various properties and methods. This assures that all requests are assigned
    /// to the current document only.
    /// <para>
    /// For dealing with an instance of the class it is recommended to use the <see cref="GuruComponents.Netrix.ITextFormatting"/>
    /// interface as type.
    /// </para>
    /// <para>
    /// This class was introduced with Netrix 2 in Spring 2011 release to avoid conflicts with the classic formatting behavior, as exposed
    /// by the <see cref="HtmlTextFormatting"/> class.
    /// </para>
    /// </remarks>
    /// <seealso cref="GuruComponents.Netrix.HtmlTextFormatting">HtmlTextFormatting</seealso>
    /// <seealso cref="GuruComponents.Netrix.ITextFormatting">ITextFormatting</seealso>
    public class CssTextFormatting : HtmlTextFormatting
    {

        /// <summary>
        /// Constructor which simply takes an HtmlEditor to interface with MSHTML.
        /// </summary>
        /// <remarks>
        /// This constructor supports the
        /// NetRix infrastructure and should never called directly from user code.
        /// </remarks>
        /// <param name="editor"></param>
        internal CssTextFormatting(IHtmlEditor editor) : base(editor)
        {
        }

        # region Public Textformatting Properties

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
        public override void RemoveInlineFormat()
        {
            this.editor.Exec(Interop.IDM.REMOVEFORMAT);
        }

        /// <summary>
        /// Indicates if the background color can be set
        /// </summary>
        /// <remarks>
        /// It is recommended to use this property to update the user interface, e.g. disabling or enabling toolbar buttons or menu entries.
        /// </remarks>
        public override bool CanSetBackColor
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
        public override bool CanSetFontName
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
        public override bool CanSetFontSize
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
        public override bool CanSetForeColor
        {
            get
            {
                return editor.IsCommandEnabled(Interop.IDM.FORECOLOR);
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
        public override string FontName
        {
            get
            {
                IElement e = GetElementThatSpansSelection(false);
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
                BatchedUndoUnit unit = editor.OpenBatchUndo("FontName");
                IElement el = GetElementThatSpansSelection(true);
                if (el != null)
                {
                    el.CurrentStyle.FontStyle.fontFamily = value;
                    CleanUpChildElements(el, "font-family");
                }
                unit.Close();
            }
        }

        /// <summary>
        /// Gets or sets the font size using a span tag and local CSS style 'font-size'.
        /// </summary>
        /// <remarks>
        /// Use as a replacement for <see cref="FontSize"/>.
        /// </remarks>
        public override System.Web.UI.WebControls.Unit CssFontSize
        {
            get
            {
                IElement e = GetElementThatSpansSelection(false);
                if (e != null)
                {
                    return e.EffectiveStyle.font.FontSize;
                }
                else
                {
                    return System.Web.UI.WebControls.Unit.Empty;
                }
            }
            set
            {
                BatchedUndoUnit unit = editor.OpenBatchUndo("CssFontSize");
                IElement el = GetElementThatSpansSelection(true);
                if (el != null)
                {
                    el.CurrentStyle.FontStyle.fontSize = value;
                    CleanUpChildElements(el, "font-size");
                }
                unit.Close();
            }
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
        public override Color ForeColor
        {
            get
            {
                IElement e = GetElementThatSpansSelection(false);
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
                BatchedUndoUnit unit = editor.OpenBatchUndo("ForeColor");
                IElement el = GetElementThatSpansSelection(true);
                if (el != null)
                {
                    el.CurrentStyle.color = value;
                    CleanUpChildElements(el, "color");
                }
                unit.Close();
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
        public override Color BackColor
        {
            get
            {
                IElement e = GetElementThatSpansSelection(false);
                return e.EffectiveStyle.background.Color;
            }
            set
            {
                BatchedUndoUnit unit = editor.OpenBatchUndo("BackColor");
                IElement el = GetElementThatSpansSelection(true);
                if (el != null)
                {
                    el.CurrentStyle.BackgroundStyle.backgroundColor = value;
                    CleanUpChildElements(el, "background-color");
                }
                unit.Close();
            }
        }
      
        /// <summary>
        /// Align the current block selection or current block.
        /// </summary>
        /// <remarks>Full alignment justifies the text.</remarks>
        /// <param name="alignment"></param>
        public override void SetAlignment(Alignment alignment)
        {
            // first look whether we have an element where the style is already set
            //Interop.IDisplayServices ds = (Interop.IDisplayServices)editor.GetActiveDocument(false);
            //Interop.IDisplayPointer dp;
            //Interop.IHTMLCaret caret;
            //ds.CreateDisplayPointer(out dp);
            //ds.GetCaret(out caret);
            //caret.MoveDisplayPointerToCaret(dp);
            //Interop.POINT pp = new Interop.POINT();
            //caret.GetLocation(ref pp, false);
            IElement el = GetElementThatSpansSelection(false);
            if (el != null)
            {
                switch (alignment)
                {
                    case Alignment.Left:
                        el.CurrentStyle.textAlign = HorizontalAlign.Left;
                        break;
                    case Alignment.Right:
                        el.CurrentStyle.textAlign = HorizontalAlign.Right;
                        break;
                    case Alignment.Center:
                        el.CurrentStyle.textAlign = HorizontalAlign.Center;
                        break;
                    case Alignment.Full:
                        el.CurrentStyle.textAlign = HorizontalAlign.Justify;
                        break;
                    default:
                        el.CurrentStyle.textAlign = HorizontalAlign.NotSet;
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
        public override Alignment GetAlignment()
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

        # endregion

        # region CSS Compliant Formatting

        private IElement GetElementThatSpansSelection(bool createImmediately)
        {
            /* 1. Take the current selection
             * 2. Check whether the boundaries match a container element exactly
             * 3. If YES take the boundary element and return it. Caller can use styles on this.
             * 4. If NO wrap the selection into a SPAN element and return this.
             * */
            Interop.IHtmlBodyElement body = editor.GetBodyElement().GetBaseElement() as Interop.IHtmlBodyElement;
            Interop.IHTMLDocument2 doc = ((Interop.IHTMLElement)body).GetDocument() as Interop.IHTMLDocument2;
            Interop.IHTMLSelectionObject sel = (Interop.IHTMLSelectionObject) doc.GetSelection();
            Interop.IHTMLTxtRange tr = sel.CreateRange() as Interop.IHTMLTxtRange;
            Interop.IHTMLElement parent = null;
            if (tr.GetText() != null)
            {
                parent = tr.ParentElement();
            }
            Interop.IHTMLElement el = null;
            for (; ; )
            {
                if (parent != null && !parent.GetTagName().Equals("BODY"))
                {
                    string selection = tr.GetHtmlText();
                    string parenthtm = parent.GetOuterHTML();
                    selection = (!String.IsNullOrEmpty(selection)) ? selection.Trim().Replace(Environment.NewLine, "") : String.Empty;
                    parenthtm = (!String.IsNullOrEmpty(parenthtm)) ? parenthtm.Trim().Replace(Environment.NewLine, "") : String.Empty;
                    if (parenthtm.Contains(selection))
                    {
                        el = parent;
                        // found a valid element
                        break;
                    }
                }
                // come here if element was not found, does not match range, or an error occured
                if (createImmediately)
                {
                    // this is just to look whether the current formatting is already as expected
                    Interop.IMarkupServices ms = (Interop.IMarkupServices)doc;
                    Interop.IMarkupPointer m1, m2;
                    ms.CreateMarkupPointer(out m1);
                    ms.CreateMarkupPointer(out m2);
                    ms.CreateElement(Interop.ELEMENT_TAG_ID.SPAN, "", out el);
                    Interop.IDisplayServices ds;
                    Interop.IDisplayPointer dp;
                    Interop.IMarkupPointer mp;
                    Interop.IHTMLCaret cr;
                    ds = (Interop.IDisplayServices)doc;
                    ds.CreateDisplayPointer(out dp);
                    ds.GetCaret(out cr);
                    ms.CreateMarkupPointer(out mp);
                    if (parent != null)
                    {
                        ms.MovePointersToRange(tr, m1, m2);
                        Interop.MARKUP_CONTEXT_TYPE ct = Interop.MARKUP_CONTEXT_TYPE.CONTEXT_TYPE_None;
                        Interop.IHTMLElement scope;
                        int chars = 0;
                        string text = "";
                        m1.Left(-1, out ct, out scope, ref chars, out text);
                        m2.Right(parent.GetTagName().Equals("BODY") ? 1 : 0, out ct, out scope, ref chars, out text);
                        ms.InsertElement(el, m1, m2);
                        //... and place caret inside the element
                        mp.MoveAdjacentToElement(el, Interop.ELEMENT_ADJACENCY.ELEM_ADJ_AfterBegin);
                        dp.MoveToMarkupPointer(mp, null);
                        cr.MoveCaretToPointer(dp, true, Interop.CARET_DIRECTION.CARET_DIRECTION_SAME);
                    }
                    else
                    {
                        // assume their is no selection and we have to create an element with the caret inside
                        cr.MoveDisplayPointerToCaret(dp);
                        dp.PositionMarkupPointer(m1);
                        dp.PositionMarkupPointer(m2);
                        ms.InsertElement(el, m1, m2);
                    }
                }
                else
                {
                    el = parent;
                }

                break;
            }
            return editor.GenericElementFactory.CreateElement(el) as IElement;
        }

        private void CleanUpChildElements(IElement el, string styleToRemove)
        {
            // after outer formatting this we have to remove the attributes from all child element 
            Interop.IHTMLElementCollection ec = ((Interop.IHTMLElement2)el.GetBaseElement()).GetElementsByTagName("*");
            if (ec != null && ec.GetLength() > 0)
            {
                for (int i = ec.GetLength() - 1; i >= 0; i--)
                {
                    Interop.IHTMLElement element = (Interop.IHTMLElement)ec.Item(i, i);
                    element.GetStyle().RemoveAttribute(styleToRemove, 0);
                    if (element.GetTagName().ToUpper().Equals("SPAN"))
                    {
                        if (String.IsNullOrEmpty(element.GetStyle().GetCssText()))
                        {
                            // the element does not has any specific style
                            string innerHtml = element.GetInnerHTML();
                            //((Interop.IHTMLDOMNode) element)
                            Interop.IMarkupServices ms = editor.MshtmlSite.MSHTMLDocument as Interop.IMarkupServices;
                            Interop.IHTMLTxtRange tr = ((Interop.IHtmlBodyElement)((Interop.IHTMLDocument2)editor.MshtmlSite.MSHTMLDocument).GetBody()).createTextRange();
                            tr.MoveToElementText(element);
                            tr.PasteHTML(innerHtml);
                        }
                    }
                }
            }
        }

        # endregion

    }
}