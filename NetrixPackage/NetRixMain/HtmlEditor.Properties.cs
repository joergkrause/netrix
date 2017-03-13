using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.WebEditing;
using GuruComponents.Netrix.WebEditing.Behaviors;
using GuruComponents.Netrix.WebEditing.Documents;
using GuruComponents.Netrix.WebEditing.Elements;
using GuruComponents.Netrix.HtmlFormatting;
using GuruComponents.Netrix.WebEditing.Glyphs;
using GuruComponents.Netrix.WebEditing.HighLighting;
using GuruComponents.Netrix.WebEditing.UndoRedo;
using HtmlWindow = GuruComponents.Netrix.WebEditing.Documents.HtmlWindow;

# pragma warning disable 0618

namespace GuruComponents.Netrix
{

    public partial class HtmlEditor
    {

        # region Private storage for User Properties
        private SnapElement _snapElement = null;
        private bool _scrollBarsEnabled = true;
        private bool _flatScrollBars = false;
        private bool _xptheming = true;
        private bool _scriptEnabled = true;
        private bool _allowInPlaceNavigation = true;
        private bool _noTextSelection = false;
        private bool _imeReconversion = false;
        private bool _fullDocumentMode = true;
        private bool _internalShortcutKeys = true;
        private bool _allowDrop = false;
        private BlockDefaultType _blockDefaultType = BlockDefaultType.P;
        private bool _stopFocusOnLoad = false;
        private bool _transposeEnterBehavior = false;

        private bool _absolutePositioningEnabled = false;
        private bool _absolutePositioningDesired = false;
        private bool _absolutePositioningDesiredValue = false;

        private bool _autoUrlModeEnabled = false;
        private bool _autoUrlModeDesired = false;
        private bool _autoUrlModeDesiredValue = false;

        private bool _linkedStylesheetsEnabled = true;
        private bool _linkedStylesheetsDesired = true;
        private bool _linkedStylesheetsDesiredValue = true;

        private bool _bordersVisible = false;
        private bool _bordersDesired = false;
        private bool _bordersDesiredValue = false;

        private bool _designModeEnabled = true;
        private bool _designModeDesired = false;
        private bool _designModeDesiredValue = true;

        private bool _stringLoadDesired = true;
        private string _stringLoadContent = String.Empty;

        private bool _glyphsDesired = false;
        private BuildGlyphs _glyphTable = null;

        private bool _multipleSelectionEnabled = false;
        private bool _multipleSelectionDesired = false;
        private bool _multipleSelectionDesiredValue = false;

        private bool _liveResizeEnabled = false;
        private bool _liveResizeDesired = false;
        private bool _liveResizeDesiredValue = false;

        private BatchedUndoUnit _undoUnit = null;

        private bool _atomicEnabled = false;
        private bool _atomicDesired = false;
        private bool _atomicDesiredValue = false;

        private HtmlTextFormatting _textFormatting = null;
        private HtmlDocument _document = null;
        private HtmlSelection _selection = null;
        private bool _isFileBasedDocument = false;
        private bool _complexContent = true;
        private bool _keepSelectionEnabled = false;
        private bool _keepSelectionDesiredValue = false;
        private bool _keepSelectionDesired = false;
        private HtmlGrid _grid = null;
        private bool _gridVisibleDesired = false;
        private bool _gridVisibleDesiredValue = false;

        private bool _respectVisiblity = false;
        private bool _respectVisiblityDesired = false;
        private bool _respectVisiblityDesiredValue = false;

        private HtmlDocumentStructure _documentStructure = null;
        private HtmlWindow _htmlWindow = null;
        private IFrameSet _htmlframeset = null;
        private TextSelector _textSelector = null;
        private Encoding _encoding = Encoding.GetEncoding("iso-8859-1");

        private bool _loadUrlDesired;
        private string _desiredUrl;

        private ArrayList _editDesigners = new ArrayList(2);

        private HtmlFormatter _htmlFormatter;

        internal static CultureInfo Culture = null;
        private string _currentCulture = String.Empty;

        private ContextMenu _contextMenu;
        private bool buildMht = false;

        private ContextMenuStrip _contextMenuStrip;
        # endregion

        # region Implementation of RichTextBox Elements

        /// <summary>
        /// If set to true all loaded data with textual base type will be treated as HTML.
        /// </summary>
        [Browsable(true), DefaultValue(false), Category("Netrix Component"), Description("If set to true all loaded data with textual base type will be treated as HTML.")]
        public bool ForceMimeType
        {
            get
            {
                return _forceMimeType;
            }
            set
            {
                _forceMimeType = value;
            }
        }
        private bool _forceMimeType = false;

        /// <summary>
        /// Processes the TAB key if possible instead of moving focus to next control.
        /// </summary>
        [Browsable(true), DefaultValue(false), Category("Netrix Component"), Description("Processes the TAB key if possible instead of moving focus to next control.")]
        public bool AcceptsTab
        {
            get { return _AcceptsTab; }
            set { _AcceptsTab = value; }
        }
        private bool _AcceptsTab;

        /// <summary>
        /// Whether select a complete word on double click.
        /// </summary>
        [Browsable(true), DefaultValue(false), Category("Netrix Component"), Description("Whether select a complete word on double click.")]
        public bool AutoWordSelection
        {
            get { return _AutoWordSelection; }
            set { _AutoWordSelection = value; }
        }
        private bool _AutoWordSelection;

        /// <summary>
        /// Sets or gest the current font.
        /// </summary>
        /// <remarks>
        /// This property is a shortcut to <see cref="ITextFormatting.FontSize"/>, <see cref="ITextFormatting.FontName"/>
        /// and several toggle methods. In case the color
        /// is not set the property returns Times New Roman 12pt.</remarks>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new Font Font
        {
            get
            {
                if (IsReady && this.TextFormatting != null)
                {
                    FontUnit unit = this.TextFormatting.FontSize;
                    FontStyle style =
                        ((this.TextFormatting.GetBoldInfo() == HtmlCommandInfo.Both ? FontStyle.Bold : FontStyle.Regular)
                        |
                        (this.TextFormatting.GetItalicsInfo() == HtmlCommandInfo.Both ? FontStyle.Italic : FontStyle.Regular)
                        |
                        (this.TextFormatting.GetStrikethroughInfo() == HtmlCommandInfo.Both ? FontStyle.Strikeout : FontStyle.Regular)
                        |
                        (this.TextFormatting.GetUnderlineInfo() == HtmlCommandInfo.Both ? FontStyle.Underline : FontStyle.Regular));
                    Font font = new Font(this.TextFormatting.FontName, HtmlTextFormatting.ConvertUnitToPoint(unit), style);
                    return font;
                }
                else
                {
                    return new Font("Times New Roman", 12.0F);
                }
            }
            set
            {
                if (IsReady && this.TextFormatting != null)
                {
                    this.TextFormatting.FontName = value.Name;
                    if ((value.Bold && this.TextFormatting.GetBoldInfo() != HtmlCommandInfo.Both) || (!value.Bold && this.TextFormatting.GetBoldInfo() == HtmlCommandInfo.Both))
                    {
                        this.TextFormatting.ToggleBold();
                    }
                    if ((value.Italic && this.TextFormatting.GetItalicsInfo() != HtmlCommandInfo.Both) || (!value.Italic && this.TextFormatting.GetItalicsInfo() == HtmlCommandInfo.Both))
                    {
                        this.TextFormatting.ToggleItalics();
                    }
                    if ((value.Strikeout && this.TextFormatting.GetStrikethroughInfo() != HtmlCommandInfo.Both) || (!value.Strikeout && this.TextFormatting.GetStrikethroughInfo() == HtmlCommandInfo.Both))
                    {
                        this.TextFormatting.ToggleStrikethrough();
                    }
                    if ((value.Underline && this.TextFormatting.GetUnderlineInfo() != HtmlCommandInfo.Both) || (!value.Underline && this.TextFormatting.GetUnderlineInfo() == HtmlCommandInfo.Both))
                    {
                        this.TextFormatting.ToggleUnderline();
                    }
                    this.TextFormatting.FontSize = HtmlTextFormatting.ConvertSizeToUnit(value.SizeInPoints);
                }
            }
        }

        /// <summary>
        /// Sets or gets the writing direction.
        /// </summary>
        /// <remarks>
        /// </remarks>      
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new bool RightToLeft
        {
            get
            {
                return (this.GetCommandInfo(Interop.IDM.DIRRTL) == HtmlCommandInfo.Checked);
            }
            set
            {
                if (this.TextFormatting != null)
                {
                    this.TextFormatting.DirectionRtlDocument(value);
                }
            }
        }

        /// <summary>
        /// Returns the currently selected text.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string SelectedText
        {
            get
            {
                if (this.Selection != null)
                {
                    return this.Selection.Text;
                }
                else
                {
                    return String.Empty;
                }
            }
        }

        [Browsable(false), DefaultValue(Alignment.Left)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Alignment SelectionAlignment
        {
            get
            {
                if (this.TextFormatting != null)
                {
                    return this.TextFormatting.GetAlignment();
                }
                else
                {
                    return Alignment.None;
                }
            }
            set
            {
                if (this.TextFormatting != null)
                {
                    this.TextFormatting.SetAlignment(value);
                }
            }
        }

        /// <summary>
        /// Lines the document represents, that means, text divided by line feeds.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string[] Lines
        {
            get
            {
                if (IsReady)
                {
                    ArrayList lines = new ArrayList();
                    lines.AddRange(this.InnerText.Split('\n'));
                    string[] lString = new string[lines.Count];
                    lines.CopyTo(lString);
                    return lString;
                }
                else
                {
                    return new string[] { };
                }
            }
            set
            {
                this.InnerHtml = String.Join("<br>\n\r", value);
            }
        }

        /// <summary>
        /// Length of current selection.
        /// </summary>		
        [Browsable(false), DefaultValue(0)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int SelectionLength
        {
            get
            {
                if (DesignMode) return 0;
                if (this.Selection == null) return 0;
                if (this.Selection.Text == null) return 0;
                return this.Selection.Text.Length;
            }
            set
            {
                ITextSelector selector = this.TextSelector;
                if (selector == null) return;
                //selector.MovePointersToRange();
                int currentLength = selector.GetTextBetweenPointers().Length;
                if (currentLength > value)
                {
                    //shrink selection by moving the second pointer
                    for (int i = 0; i < currentLength - value; i++)
                    {
                        selector.MoveSecondPointer(MoveTextPointer.PrevChar);
                    }
                }
                else
                {
                    for (int i = 0; i < value - currentLength; i++)
                    {
                        selector.MoveSecondPointer(MoveTextPointer.NextChar);
                    }
                }
            }
        }

        /// <summary>
        /// Length of text content, with HTML parts stripped out.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int TextLength
        {
            get
            {
                return this.InnerText.Length;
            }
        }

        /// <summary>
        /// Appends text at the current caret position and treats the text as HTML.
        /// </summary>
        /// <remarks>In case the current caret cannot be recognized the content is appended to the end.</remarks>
        /// <param name="text">Text or HTML being written.</param>
        public void AppendText(string text)
        {
            this.Document.InsertHtml(text);
        }

        /// <summary>
        /// Clears the document but let the connected objects intact.
        /// </summary>
        public void Clear()
        {
            this.GetActiveDocument(false).Clear();
            ResetDesiredProperties(false);
        }

        /// <summary>
        /// Returns the character nearby the given client position.
        /// </summary>
        /// <param name="pt">Point in client coordinates.</param>
        /// <returns>Character nearby the coordinates. In case it cannot be recognized it will return <see cref="Char.MinValue"/>.</returns>
        public char GetCharFromPosition(Point pt)
        {
            try
            {
                Interop.IMarkupServices ms;
                Interop.IDisplayServices ds;
                ms = (Interop.IMarkupServices)this.GetActiveDocument(false);
                Interop.IMarkupPointer mp;
                ms.CreateMarkupPointer(out mp);
                Interop.IHTMLElement el = ((Element)this.Selection.Element).GetBaseElement();
                ds = (Interop.IDisplayServices)this.GetActiveDocument(false);
                Interop.IDisplayPointer dp;
                ds.CreateDisplayPointer(out dp);
                // Move display pointer to markup
                uint result;
                Interop.POINT ptPoint = new Interop.POINT();
                ptPoint.x = pt.X;
                ptPoint.y = pt.Y;
                dp.MoveToPoint(ptPoint, Interop.COORD_SYSTEM.COORD_SYSTEM_CONTAINER, el, (uint)0, out result);
                Interop.IHTMLCaret cr;
                ds.GetCaret(out cr);
                // set the caret at the beginning of the new selection
                cr.MoveCaretToPointer(dp, true, Interop.CARET_DIRECTION.CARET_DIRECTION_SAME);
                this.TextSelector.MoveSecondPointer(MoveTextPointer.NextChar);
                string text = this.TextSelector.GetTextBetweenPointers();
                return (text != null && text.Length > 0) ? text[0] : Char.MinValue;
            }
            catch
            {
                return Char.MinValue;
            }
        }

        # endregion

        private IHtmlFormatterOptions htmlFormatterOptions;

        /// <summary>
        /// Default formatter used if no external formatter is being provided.
        /// </summary>
        /// <remarks>
        /// To provide your own formatter, implement <see cref="IHtmlFormatterOptions"/> interface and
        /// set to this property. Then the internal object gets overwritten.
        /// </remarks>
        /// <seealso cref="IHtmlFormatterOptions"/>
        [Browsable(true), Description("Default formatter used if no external formatter is being provided.")]
        [Category("Netrix Component")]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public IHtmlFormatterOptions HtmlFormatterOptions
        {
            get
            {
                if (htmlFormatterOptions == null)
                    htmlFormatterOptions = new HtmlFormatterOptions();
                return (HtmlFormatterOptions)htmlFormatterOptions;
            }
            set
            {
                htmlFormatterOptions = value;
            }
        }

        /// <summary>
        /// Forces the file loader to prepare the content as MHT which could be save later.
        /// </summary>
        /// <remarks>
        /// This option is probably time consuming on each save operation.
        /// </remarks>
        /// <seealso cref="SaveMht()"/>
        /// <seealso cref="CanSaveMht"/>
        [Browsable(true), Description("Forces the file loader to prepare the content as MHT which could be save later.")]
        [Category("Netrix Component"), DefaultValue(false)]
        public bool CanBuildMht
        {
            get
            {
                return buildMht;
            }
            set
            {
                buildMht = value;
            }
        }

        /// <summary>
        /// Checks whether or not we have MHT loaded and available for save to string or file.
        /// </summary>
        /// <seealso cref="SaveMht()"/>
        [Browsable(false)]
        public bool CanSaveMht
        {
            get { return (SaveMht() != null); }
        }

        /// <summary>
        /// Textual representation of content.
        /// </summary>
        /// <remarks>
        /// The inner text is all content without any tags. This may garbage the text because spaces between
        /// words made by elements disappear. Setting the inner text will remove all tags from document. 
        /// </remarks>
        /// <seealso cref="InnerHtml"/>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false), DefaultValue("")]
        public override
            string Text
        {
            get
            {
                return this.InnerText;
            }
            set
            {
                this.InnerText = value;
            }
        }

        /// <summary>
        /// Controls Visibility of whole container.
        /// </summary>
        public new bool Visible
        {
            get { return base.Visible; }
            set
            {
                base.Visible = value;
                panelEditContainer.Visible = base.Visible;
            }
        }

        /// <summary>
        /// The current Assembly version.
        /// </summary>                   
        /// <remarks>
        /// This property returns the current assembly version to inform during design session about the assembly loaded.
        /// </remarks>
        [Browsable(true), Category("Netrix Component"), Description("Current Version. ReadOnly.")]
        public string Version
        {
            get
            {
                return this.GetType().Assembly.GetName().Version.ToString();
            }
        }

        /// <summary>
        /// Returns the current serial number prefix the control was licensed with.
        /// </summary>
        /// <remarks>
        /// Note: The prefix does not contain secret information about the key or your serial number.
        /// If an user retrieves the prefix from the control he cannot activate or retrieve your license.
        /// </remarks>
        [Browsable(false)]
        public string SerialNumberPrefix
        {
            get
            {
# if  LICENSED
				return this._license.LicenseKey.Substring(0, this._license.LicenseKey.IndexOf("-"));
# else
                return "BETA";
# endif

            }
        }

        /// <summary>
        /// Get or sets the control's enabled state and sets nested controls as well.
        /// </summary>
        public new bool Enabled
        {
            get { return base.Enabled; }
            set
            {
                base.Enabled = value;
                toolStripContainer1.Enabled = value;
                rulerControlV.Enabled = value;
                rulerControlH.Enabled = value;
                statusStrip1.Enabled = value;

            }

        }

        # region Culture Management

        /// <summary>
        /// Sets or gets the current Culture.
        /// </summary>
        /// <remarks>
        /// Setting a different culture will load the language from
        /// NetRix.Localization.dll satellite DLL, which should contain the strings and images for that
        /// culture. If the culture does not exists there the default culture "en-US" will load.
        /// Once the resource is loaded, we store it in a static field to make it accessible to all
        /// classes which does not derive from HtmlEditor.
        /// </remarks>
        [Browsable(false), DefaultValue("en-US")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string CurrentCulture
        {
            get
            {
                if (_currentCulture.Equals(String.Empty))
                {
                    _currentCulture = Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;
                }
                return _currentCulture;
            }
            set
            {
                if (this.DesignMode) return;
                try
                {
                    if (value.Length >= 5 && value.IndexOf("-") != -1)
                    {
                        Culture = new CultureInfo(value, true);
                    }
                    else
                    {
                        Culture = Thread.CurrentThread.CurrentCulture;
                    }
                }
                catch
                {
                    // don't change current culture on error
                    Culture = Thread.CurrentThread.CurrentCulture;
                }
                Thread.CurrentThread.CurrentCulture = Culture;
                Thread.CurrentThread.CurrentUICulture = Culture;
                // Set the culture for the resource assembly
                _currentCulture = value;
            }
        }

        # endregion

        /// <summary>
        /// Get an instance of the <see cref="HtmlFormatter"/> class.
        /// </summary>
        private HtmlFormatter Formatter
        {
            get
            {
                if (_htmlFormatter == null)
                {
                    _htmlFormatter = new HtmlFormatter();
                }
                return _htmlFormatter;
            }
        }

        /// <summary>
        /// Access to the glyph module.
        /// </summary>
        /// <remarks>
        /// This property gives access to the advanced customizable glyph module.
        /// <para>Glyphs are small icons representing tags in design mode. It helps users to understand where the tags are and how they build
        /// the content. There are several glyphs available as embedded resources.</para>
        /// </remarks>
        /// <seealso cref="GlyphVariant"/>
        /// <seealso cref="HtmlGlyphsKind"/>
        [TypeConverter(typeof(ExpandableObjectConverter))]
        [Browsable(true), Category("Netrix Component"), Description("Access to the glyph module.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public BuildGlyphs Glyphs
        {
            get
            {
                if (_glyphTable == null)
                {
                    _glyphTable = new BuildGlyphs(this);
                }
                if (!IsReady)
                {
                    _glyphsDesired = true;
                }
                return _glyphTable;
            }
        }

        #region +++++ Block: Common Control Properties

        # region Highlighting, SpellChecker and Word selection

        /// <summary>
        /// Return the currently used TextSelector.
        /// </summary>
        /// <remarks>
        /// The <see cref="GuruComponents.Netrix.WebEditing.HighLighting.TextSelector">TextSelector</see> class handles selection 
        /// specifically for text, whereas HtmlSelection provides element based methods.
        /// </remarks>
        [Browsable(false)]
        public ITextSelector TextSelector
        {
            get
            {
                if (!ThrowDocumentNotReadyException()) return null;
                if (_textSelector == null)
                {
                    _textSelector = new TextSelector(this);
                }
                return (ITextSelector)_textSelector;
            }
        }

        /// <summary>
        /// This property returns the number of words in the document.
        /// </summary>
        /// <remarks>
        /// If the document is not in design mode the property will return 0. The method is time consuming.
        /// It is not recommended to call this property frequently, e.g. in mouse move or key press handlers.
        /// </remarks>
        /// <remarks>
        /// The method works in design mode only.
        /// </remarks>
        /// <example>
        /// Assuming MenuItem4 is a way the user can reach this method, the application can display the number of words
        /// using this method:<code>
        ///Private Sub MenuItem4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem4.Click
        ///  MessageBox.Show(Me, "This document contains approximatly " + Me.HtmlEditor1.WordCount.ToString() + " Words", 
        ///                      "Word Count", 
        ///                      MessageBoxButtons.OK, 
        ///                      MessageBoxIcon.Information)
        ///End Sub</code>
        /// </example>
        [Browsable(false)]
        public int WordCount
        {
            get
            {
                if (!ThrowDocumentNotReadyException()) return 0;
                // Marker
                Interop.IMarkupServices ims = GetActiveDocument(false) as Interop.IMarkupServices;
                Interop.IHTMLTxtRange trg = GetBodyThreadSafe(false).createTextRange();
                Interop.IMarkupPointer pStart, pEnd, pCursor1, pCursor2;
                ims.CreateMarkupPointer(out pStart);
                ims.CreateMarkupPointer(out pEnd);
                ims.CreateMarkupPointer(out pCursor1);
                ims.CreateMarkupPointer(out pCursor2);
                ims.MovePointersToRange(trg, pStart, pEnd);
                // set start location for pointers
                pCursor1.MoveToPointer(pStart);
                pCursor2.MoveToPointer(pStart);
                int bDone = 0;
                int wordCount = 0;
                while (bDone == 0)
                {
                    pCursor2.IsEqualTo(pEnd, out bDone);
                    if (bDone == 0)
                    {
                        pCursor2.IsLeftOfOrEqualTo(pCursor1, out bDone);

                        if (bDone != 0)
                        {
                            //pCursor1.MoveUnit(Interop.MOVEUNIT_ACTION.MOVEUNIT_NEXTCLUSTERBEGIN);
                            if (wordCount > 0)
                            {
                                pCursor1.MoveUnit(Interop.MOVEUNIT_ACTION.MOVEUNIT_NEXTWORDBEGIN);
                            }
                            pCursor2.MoveToPointer(pCursor1);
                            pCursor2.MoveUnit(Interop.MOVEUNIT_ACTION.MOVEUNIT_NEXTWORDEND);
                            pCursor2.IsEqualTo(pCursor1, out bDone);
                        }
                    }
                    ims.MoveRangeToPointers(pCursor1, pCursor2, trg);
                    if (trg.GetText() != null)
                    {
                        string txt = trg.GetText().Trim();
                        bool valid = true;
                        for (int i = 0; i < txt.Length; i++)
                        {
                            if (Char.IsPunctuation(txt, i))
                            {
                                valid = false;
                                break;
                            }
                        }
                        if (valid && txt.Length > 0)
                        {
                            wordCount++;
                        }
                    }
                    pCursor1.MoveUnit(Interop.MOVEUNIT_ACTION.MOVEUNIT_NEXTWORDBEGIN);
                    pCursor2.MoveUnit(Interop.MOVEUNIT_ACTION.MOVEUNIT_NEXTWORDEND);
                }
                return wordCount;
            }
        }

        # endregion

        #region Grid

        [Browsable(true), Category("Netrix Component"), Description("The grid properties, including snap features.")]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public IHtmlGrid Grid
        {
            get
            {
                if (_grid == null)
                {
                    _grid = new HtmlGrid(this);
                }
                return _grid;
            }
        }
        # endregion

        # region Styles and Behavior

        private bool _disableEditFocus = false;
        private bool _disableEditFocusDesired = false;

        /// <summary>
        /// Turns off the hatched border and handles around a site selectable element when the element has "edit focus" in design mode; that is, when the text or contents of the element can be edited.
        /// </summary>
        /// <value><c>True</c> to active the property. Defaults to <c>False</c>.</value>
        [DefaultValue(false), Browsable(true), Category("Netrix Component"), Description("Turns off the hatched border and handles around a site selectable element when the element has 'edit focus' in design mode; that is, when the text or contents of the element can be edited.")]
        public bool DisableEditFocus
        {
            get
            {
                return _disableEditFocus;
            }
            set
            {
                _disableEditFocus = value;
                if (IsReady)
                {
                    this.Exec(Interop.IDM.DISABLE_EDITFOCUS_UI, _disableEditFocus);
                }
                else
                {
                    _disableEditFocusDesired = true;
                }
            }
        }

        /// <summary>
        /// When activated, the control will respect the visiblity property set to hidden and the
        /// display property set to none.
        /// </summary>
        /// <value><c>True</c> to active the property. Defaults to <c>False</c>.</value>
        [DefaultValue(false), Browsable(true), Category("Netrix Component"), Description("When activated, the control will respect the visiblity styles applicable to browse mode in design mode too.")]
        public bool RespectVisibility
        {
            get
            {
                return _respectVisiblity;
            }
            set
            {
                _respectVisiblity = value;
                if (IsReady)
                {
                    this.GetActiveDocument(false).ExecCommand("RespectVisibilityIndesign", false, _respectVisiblity ? "true" : "false");
                }
                else
                {
                    _respectVisiblityDesired = true;
                    _respectVisiblityDesiredValue = value;
                }
            }
        }

        /// <summary>
        /// Show borders around elements if they don't have own borders.
        /// </summary>
        /// <remarks>
        /// This makes invisible tables and blind images visible at design time. 
        /// If the table designer plugin is being used it's recommended to disable this option.
        /// </remarks>
        [DefaultValue(false), Browsable(true), Category("Netrix Component"), Description("Show borders around elements if they have no own borders.")]
        public bool BordersVisible
        {
            get
            {
                return _bordersVisible;
            }
            set
            {
                _bordersVisible = value;
                if (!IsReady)
                {
                    _bordersDesired = true;
                    _bordersDesiredValue = value;
                    return;
                }
                else
                {
                    Exec(Interop.IDM.SHOWZEROBORDERATDESIGNTIME, _bordersVisible);
                }
            }
        }

        /// <summary>
        /// This property disables linked stylesheets temporarily in design view.
        /// </summary>
        /// <remarks>
        /// It does not remove the styles or link tags nor it has any effect on the final document.
        /// </remarks>
        /// <example>
        /// Use the following code to switch the style sheet on and off:
        /// <code>
        /// this.htmlEditor1.LinkedStylesheetsEnabled = e.Button.Pushed;
        /// </code>
        /// </example>
        [DefaultValue(true), Browsable(true), Category("Netrix Component"), Description("This property disables linked stylesheets temporarily in design view.")]
        public bool LinkedStylesheetsEnabled
        {
            get
            {
                return _linkedStylesheetsEnabled;
            }
            set
            {
                _linkedStylesheetsEnabled = value;
                if (!IsReady)
                {
                    _linkedStylesheetsDesired = true;
                    _linkedStylesheetsDesiredValue = value;
                    return;
                }
                else
                {
                    // take the collection of linked styles to disable or enable them
                    Interop.IHTMLStyleSheetsCollection ss = this.GetActiveDocument(true).GetStyleSheets();
                    if (ss.Length == 0) return;
                    for (int i = 0; i < ss.Length; i++)
                    {
                        Interop.IHTMLStyleSheet sRef = (Interop.IHTMLStyleSheet)ss.Item(i);
                        sRef.SetDisabled(!_linkedStylesheetsEnabled);
                    }
                }
            }
        }

        /// <summary>
        /// Enables or disables the automatic detection of URL and eMail addresses during text editing.
        /// </summary>
        [DefaultValue(false), Browsable(true), Category("Netrix Component"), Description("Enables or disables the automatic detection of URL and eMail addresses during text editing.")]
        public bool AutoUrlModeEnabled
        {
            get
            {
                return _autoUrlModeEnabled;
            }
            set
            {
                //If the control isn't ready to be put into abs pos mode,
                //set a flag and put it in abs pos mode when it is ready
                _autoUrlModeEnabled = value;
                if (!IsReady)
                {
                    _autoUrlModeDesired = true;
                    _absolutePositioningDesiredValue = value;
                    return;
                }
                else
                {
                    Exec(Interop.IDM.AUTOURLDETECT_MODE, value);
                }
            }
        }
        # endregion

        # region 2D Positioning

        /// <summary>
        /// Causes the editor to update an element's appearance continuously during a resizing or moving operation, rather than updating only at the completion of the move or resize.
        /// </summary>
        /// <remarks>
        /// When this feature is off, an element's new position or size is indicated by a dashed rectangle until the mouse button is released.
        /// </remarks>
        [DefaultValue(false), Browsable(true), Category("Netrix Component"), Description("Causes the editor to update an element's appearance continuously during a resizing or moving operation, rather than updating only at the completion of the move or resize.")]
        public bool LiveResize
        {
            get
            {
                return _liveResizeEnabled;
            }
            set
            {
                _liveResizeEnabled = value;
                if (!IsReady)
                {
                    _liveResizeDesiredValue = value;
                    _liveResizeDesired = true;
                }
                else
                {
                    this.Exec(Interop.IDM.LIVERESIZE, _liveResizeEnabled);
                }

            }
        }

        /// <summary>
        /// Any element that has the <see cref="IElement.AtomicSelection"/> attribute set will be selectable only as a unit.
        /// </summary>
        /// <remarks>
        /// In design mode, the editor's default behavior allows you to locate the insertion point in the text of any element, 
        /// including elements with an ATOMICSELECTION attribute.
        /// </remarks>
        [Browsable(true), Category("Netrix Component"), Description("Any element that has the AtomicSelection attribute set will be selectable only as a unit.")]
        [DefaultValue(false)]
        public bool AtomicSelection
        {
            get
            {
                return _atomicEnabled;
            }
            set
            {
                _atomicEnabled = value;
                if (!IsReady)
                {
                    _atomicDesiredValue = value;
                    _atomicDesired = true;
                }
                else
                {
                    this.Exec(Interop.IDM.ATOMICSELECTION, _atomicEnabled);
                }

            }
        }

        /// <summary>
        /// Indicates if multiple selection is enabled in the editor.</summary>
        /// <remarks>
        /// Implicitly places MSHTML into multiple selection mode if set to <c>true</c>.
        /// This selection mode applies only if the control is in absolute positioning mode. To enter this mode
        /// just set the property 
        /// <see cref="GuruComponents.Netrix.HtmlEditor.AbsolutePositioningEnabled">AbsolutePositioningEnabled</see>.
        /// Internally the control uses style sheets to enable HTML beeing absolute positioned.
        /// <seealso cref="HtmlGrid.SnapEnabled">SnapEnabled</seealso>
        /// <seealso cref="GuruComponents.Netrix.HtmlEditor.AbsolutePositioningEnabled">AbsolutePositioningEnabled</seealso>
        /// <seealso cref="HtmlGrid.GridVisible">GridVisible</seealso>
        /// <seealso cref="HtmlEditor.Grid">Grid</seealso>
        /// </remarks>
        [DefaultValue(false), Browsable(true), Category("Netrix Component"), Description("Indicates if multiple selection is enabled in the editor.")]
        public bool MultipleSelectionEnabled
        {
            get
            {
                return _multipleSelectionEnabled;
            }
            set
            {
                //If the control isn't ready yet, postpone setting multiple selection
                _multipleSelectionEnabled = value;
                if (!IsReady)
                {
                    _multipleSelectionDesiredValue = value;
                    _multipleSelectionDesired = true;
                }
                else
                {
                    this.Exec(Interop.IDM.MULTIPLESELECTION, _multipleSelectionEnabled);
                }
            }
        }

        /// <summary>
        /// Enables or disables absolute position for the entire editor.
        /// </summary>
        /// <remarks>
        /// Internally the control uses style sheets to enable HTML beeing absolute positioned. After moving
        /// positionable elements (IMG, TABLE, DIV, ...) the style "position:absolute" will be applied. If a 
        /// position could be retrieved the "top" and "left" style attributes are set. Switching this property off
        /// will not remove the position information nor move the elements. After an element is first time moved
        /// with the mouse with AbsolutePositioningEnabled is off, the style "position:absolute" wil be removed.
        /// The position information will still left in the style attribute. If the property will be activated later,
        /// the style "position:absolute" is added again and the elements jump back to there previous position after
        /// the mouse touches them.
        /// <seealso cref="HtmlGrid.SnapEnabled">SnapEnabled</seealso>
        /// <seealso cref="GuruComponents.Netrix.HtmlEditor.MultipleSelectionEnabled">MultipleSelectionEnabled</seealso>
        /// <seealso cref="HtmlGrid.GridVisible">GridVisible</seealso>
        /// <seealso cref="HtmlEditor.Grid">Grid</seealso>
        /// </remarks>
        [DefaultValue(false), Browsable(true), Category("Netrix Component"), Description("Enables or disables absolute position for the entire editor")]
        public bool AbsolutePositioningEnabled
        {
            get
            {
                return _absolutePositioningEnabled;
            }
            set
            {
                //If the control isn't ready to be put into abs pos mode,
                //set a flag and put it in abs pos mode when it is ready
                _absolutePositioningEnabled = value;
                if (!IsReady)
                {
                    _absolutePositioningDesired = true;
                    _absolutePositioningDesiredValue = value;
                    return;
                }
                else
                {
                    //Turn abs pos mode on or off depending on the new value
                    this.SetMousePointer(value);
                    if (value)
                    {
                        Cursor.Current = Cursors.Arrow;
                    }
                    Exec(Interop.IDM.ABSOLUTE_POSITION, _absolutePositioningEnabled);
                    Exec(Interop.IDM.TWOD_POSITION, _absolutePositioningEnabled);
                }
            }
        }

        # endregion

        # region General Document Related Commands and Properties

        /// <summary>
        /// The current selection in the editor.
        /// </summary> 
        /// <remarks>
        /// See <see cref="GuruComponents.Netrix.HtmlSelection"/> for more information.
        /// </remarks>
        [Browsable(false)]
        public ISelection Selection
        {
            get
            {
                if (_selection == null)
                {
                    _selection = new HtmlSelection(this);
                }
                return _selection;
            }
        }

        /// <summary>
        /// Maintains a selection even when the control loses focus.
        /// </summary>
        /// <remarks>
        /// This property is useful for applications that require multiple control instances to interact with each other. 
        /// One example is an e-mail application that allows users to select from a list of e-mails in one control instance and 
        /// to see the actual e-mail in a second instance.
        /// </remarks>        
        [Browsable(true), Category("Netrix Component"), Description("Maintains a selection in a control instance when the control loses focus.")]
        [DefaultValue(false)]
        public bool KeepSelection
        {
            get
            {
                return _keepSelectionEnabled;
            }
            set
            {
                _keepSelectionEnabled = value;
                if (!IsReady)
                {
                    _keepSelectionDesiredValue = value;
                    _keepSelectionDesired = true;
                }
                else
                {
                    this.Exec(Interop.IDM.KEEPSELECTION, _keepSelectionEnabled);
                }

            }
        }

        /// <summary>
        /// The text formatting element of the editor.
        /// </summary>
        /// <remarks>
        /// See <see cref="GuruComponents.Netrix.HtmlTextFormatting"/> for more information.
        /// </remarks>
        [Browsable(false)]
        public ITextFormatting TextFormatting
        {
            get
            {
                if (_textFormatting == null)
                {
                    _textFormatting = new HtmlTextFormatting(this);
                }
                return _textFormatting;
            }
        }

        /// <summary>
        /// The text formatting element of the editor. This class primarily supports CSS formatting.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This property is new in the Netrix 2.0 Spring 2011 edition.
        /// </para>
        /// See <see cref="GuruComponents.Netrix.CssTextFormatting"/> for more information.
        /// </remarks>
        [Browsable(false)]
        public ITextFormatting CssTextFormatting
        {
            get
            {
                if (_textFormatting == null)
                {
                    _textFormatting = new CssTextFormatting(this);
                }
                return _textFormatting;
            }
        }

        /// <summary>
        /// Indicates whether the editor is in design mode.</summary>
        /// <remarks>
        /// Also places MSHTML into design mode if set to <c>true</c>.
        /// </remarks> 
        [DefaultValue(true), Browsable(true), Category("Netrix Component"), Description("Indicates whether the editor is in design mode. Also places MSHTML into design mode if set to true.")]
        public bool DesignModeEnabled
        {
            get
            {
                return _designModeEnabled;
            }
            set
            {
                _designModeEnabled = value;
                _designModeDesiredValue = value;
                // Prepare string loading in case we have no file
                if (IsFileBasedDocument)
                {
                    _loadUrlDesired = true;
                    _desiredUrl = Url;
                }
                else
                {
                    _stringLoadDesired = true;
                    _stringLoadContent = GetRawHtml(false);
                }
                //If the control isn't ready to be put into design mode,
                //set a flag and put it in design mode when it is ready
                if (!IsReady)
                {
                    _designModeDesired = true;
                }
                else
                {
                    _designModeDesired = false;
                    //Turn design mode on or off depending on the new value
                    this.Exec(_designModeEnabled ? Interop.IDM.EDITMODE : Interop.IDM.BROWSEMODE);
                    if (_stringLoadDesired)
                    {
                        StringLoader(_stringLoadContent);
                        return;
                    }
                    if (_loadUrlDesired)
                    {
                        if (IsFileBasedDocument)
                            LoadFile(_desiredUrl);
                        else
                            LoadUrl(_desiredUrl);
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// On creation process look for desired design state and set internally after the
        /// component was succesfully created. Called from HtmlEditor::OnCreate method.
        /// </summary>
        internal bool DesignModeDesired
        {
            get
            {
                return this._designModeDesired;
            }
        }

        /// <summary>
        /// Returns the document object for doing insertions.
        /// </summary>
        /// <remarks>
        /// If the property was called the first time and there was no document loaded the control
        /// creates a new empty document with default properties. 
        /// </remarks>
        [Browsable(false)]
        public IDocument Document
        {
            get
            {
                if (!IsReady)
                {
                    _document = null;
                }
                if (_document == null)
                {
                    _document = new HtmlDocument(this);
                }
                return _document;
            }
        }

        /// <summary>
        /// The structure of the document.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        [Browsable(false)]
        public IDocumentStructure DocumentStructure
        {
            get
            {
                if (_documentStructure == null)
                {
                    _documentStructure = new HtmlDocumentStructure(this);
                }
                return (IDocumentStructure)_documentStructure;
            }
        }

        /// <summary>
        /// Gets access to common window related functions and global helper methods.
        /// </summary>
        /// <remarks>
        /// Wrapper information: This class implements IHTMLWindow2, IHTMLWindow3, and IHTMLWindow4.
        /// </remarks>
        [Browsable(false)]
        public IHtmlWindow Window
        {
            get
            {
                if (_htmlWindow == null)
                {
                    Interop.IHTMLWindow2 w = ((Interop.IHTMLDocument2)GetActiveDocument(true)).GetParentWindow();
                    _htmlWindow = new HtmlWindow(w, this);
                }
                return _htmlWindow;
            }
        }

        HtmlTimer _htmlTimer;

        /// <summary>
        /// Gets access to timer features for smooth internal timed operations.
        /// </summary>
        /// <remarks>
        /// Wrapper information: This class partly implements IHTMLWindow2, IHTMLWindow3, and IHTMLWindow4.
        /// </remarks>
        [Browsable(false)]
        public HtmlTimer Timer
        {
            get
            {
                if (_htmlTimer == null)
                {
                    Interop.IHTMLWindow2 w = ((Interop.IHTMLDocument2)GetActiveDocument(true)).GetParentWindow();
                    _htmlTimer = new HtmlTimer(w, this);
                }
                return _htmlTimer;
            }
        }

        # endregion

        # region Frames

        /// <summary>
        /// Gets <c>true</c> if the current document is a frame document.
        /// </summary>
        /// <remarks>
        /// The purpose of this property is to detect the frame state of the document and switch 
        /// appropriate UI elements on or off. Internally the property simply checks the number 
        /// of frames. Therefore the property may return <c>false</c>, even if the document is
        /// a frame document with just zero frames. Developery of application which use frames
        /// extensivly may double check the content using <see cref="HtmlEditor.GetXmlDocument()"/> method and
        /// XPath to see the real content of the base document.
        /// </remarks>
        [Browsable(false)]
        public bool IsFrameDocument
        {
            get
            {
                if (!ThrowDocumentNotReadyException()) return false;
                return (this.DocumentStructure.NumberOfFrames > 0);
            }
        }

        /// <summary>
        /// Returns an object containing all Frame information.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Works only if the base document of a frameset.
        /// If the document contains a body this getter is still active and usable, but the framecollection is null.
        /// After retrieving successfully the frame object it can used to manipulate or create framesets.
        /// If the current document is a standard document and a new frame structure is build, the content will
        /// depraved and replaced by a new empty frameset.
        /// </para>
        /// <para>
        /// The documentation of <see cref="GuruComponents.Netrix.WebEditing.Documents.HtmlFrameSet"/> for more information about manipulation framesets.
        /// </para>
        /// </remarks>
        [Browsable(false)]
        public IFrameSet HtmlFrames
        {
            get
            {
                if (!this.DesignModeEnabled || this.DocumentStructure.NumberOfFrames == 0)
                {
                    // destroy previous definition
                    _htmlframeset = null;
                    // document does not have frames or is not editable
                    return null;
                }
                else
                {
                    if (_htmlframeset == null)
                    {
                        _htmlframeset = new HtmlFrameSet(this, this.GetActiveDocument(true));
                        //base.CleanUpControl();
                    }
                    return _htmlframeset;
                }
            }
        }

        # endregion

        # region Contextmenu

        /// <summary>
        /// ContextMenu for the whole designer surface.
        /// </summary>
        [DefaultValue(null)]
        [Browsable(true), Category("Netrix Component"), Description("ContextMenu for the whole designer surface.")]
#if !DOTNET20
# pragma warning disable 0809
        [Obsolete("Use ContextMenuStrip instead.")]
#endif
        public override ContextMenu ContextMenu
        {
            get
            {
                return _contextMenu;
            }
            set
            {
                _contextMenu = value;
            }
        }

#if !DOTNET20
        /// <summary>
        /// ContextMenu for the whole designer surface.
        /// </summary>
        [DefaultValue(null)]
        [Browsable(true), Category("Netrix Component"), Description("ContextMenuStrip for the whole designer surface.")]
        public override ContextMenuStrip ContextMenuStrip
        {
            get
            {
                return _contextMenuStrip;
            }
            set
            {
                _contextMenuStrip = value;
            }
        }

#endif


        # endregion

        /// <summary>
        /// Gets the vertical scroll position.
        /// </summary>
        /// <remarks>
        /// The vertical scroll position is the distance from the document top
        /// to the current visible top border, measured in pixel. If the scrollbar is
        /// at the top position, this value is 0.
        /// </remarks>
        [Browsable(false)]
        public int VerticalScrollPosition
        {
            get
            {
                Interop.IHtmlBodyElement body = GetBodyThreadSafe(false);
                if (body != null)
                {
                    return ((Interop.IHTMLElement2)body).GetScrollTop();
                }
                return 0;
            }
        }

        /// <summary>
        /// Gets the horizontal scroll position. 
        /// </summary>
        /// <remarks>
        /// The horizontal scroll position is the distance from the document left
        /// to the current visible left border, measured in pixel. If the scrollbar is
        /// at the left position, this value is 0.
        /// </remarks>
        [Browsable(false)]
        public int HorizontalScrollPosition
        {
            get
            {
                Interop.IHtmlBodyElement body = GetBodyThreadSafe(false);
                if (body != null)
                {
                    return ((Interop.IHTMLElement2)body).GetScrollLeft();
                }
                return 0;
            }
        }

        /// <summary>
        /// Allow to use the NetRix control as a drop target.
        /// </summary>
        /// <remarks>
        /// Without this property the control will not recognize drop messages. 
        /// <seealso cref="GuruComponents.Netrix.WebEditing.DragDrop.DragDropCommands"/>
        /// </remarks>
        [DefaultValue(false), Browsable(true), Description("Allow to use the NetRix control as a drop target."), Category("Netrix Component")]
        public override bool AllowDrop
        {
            get
            {
                return _allowDrop;
            }
            set
            {
                _allowDrop = value;
                base.AllowDrop = _allowDrop;
            }
        }

        /// <summary>
        /// Gets/Sets if the control is active before receiving the focus.
        /// </summary>
        /// <remarks>
        /// The purpose of the property is to temporarily hook off the control from the
        /// TAB list of controls. This results in removing the cursor (insertion point) from surface,
        /// until the user explicitly clicks onto the surface. After this forced activation
        /// the control behaves like any other input control the property is no longer used.
        /// </remarks>
        [Category("Netrix Component"),
        Description("Gets/Sets if the control is active before receiving the focus."), DefaultValue(true)]
        public bool ActivationEnabled
        {
            get
            {
                return _allowActivation;
            }
            set
            {
                _allowActivation = value;
            }
        }

        /// <summary>
        /// Indicates if the control is ready for use.
        /// </summary>
        /// <remarks>
        /// <para>
        /// If the document was a frameset, the complete handling is done in HtmlFrameSet class. This
        /// class can set the IsReady property exclusivly to true, if the frame document is ready.
        /// This assures that subsequent calls / events used from the base classes, like <see cref="GuruComponents.Netrix.HtmlEditor.Selection"/>
        /// work properly as they normally throw exceptions if methods are called for an unready document.
        /// </para>
        /// </remarks>
        [Browsable(false)]
        public bool IsReady
        {
            get
            {
                // These checks help the VS.NET designer mode to avoid crashs
                if (MshtmlSite == null || DesignMode) return false;
                if (MshtmlSite.MSHTMLDocument == null) return false;
                if (MshtmlSite.MSHTMLDocument.GetBody() is Interop.IHtmlBodyElement && MshtmlSite.MSHTMLDocument.GetReadyState() == "complete")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Indicates if the contents of the editor have been modified.
        /// </summary>
        /// <remarks>
        /// This flag is set to <c>false</c> after a document or document fragment wss loaded. It becomes
        /// <c>true</c> after the first change in the documents structure or content. Moving the caret or
        /// clicking objects without moving them wil not set the flag to dirty state.
        /// <para>
        /// The flag will be reset on each save operation. This is done by calling the RawHtml content by
        /// any of the methods 
        /// <see cref="GuruComponents.Netrix.HtmlEditor.SaveHtml(Stream)">SaveHtml</see> or 
        /// <see cref="GuruComponents.Netrix.HtmlEditor.SaveFormattedHtml(Stream)">SaveFormattedHtml</see>. Whereas the
        /// SaveXXX methods really write content into a stream the 
        /// <see cref="GuruComponents.Netrix.HtmlEditor.GetRawHtml()">GetRawHtml</see> or
        /// <see cref="GuruComponents.Netrix.HtmlEditor.GetFormattedHtml()">GetFormattedHtml</see>, 
        /// method just returns the content to the caller. Both method are overloaded and provide a second
        /// parameter to force resetting the dirty flag. If this is done the host application should assure that the
        /// content is really saved, to keep the flag and the UI synchronized.        
        /// </para>
        /// </remarks>
        [Browsable(false)]
        public virtual bool IsDirty
        {
            get
            {
                if (this.DesignModeEnabled && IsReady)
                {
                    return ((Interop.IPersistMoniker)MshtmlSite.MSHTMLDocument).IsDirty() == Interop.S_OK;
                }
                return false;
            }
        }

        /// <summary>
        /// Activates or deactivates the internal shortcut keys.
        /// </summary>
        /// <remarks>
        /// If the property is on (default), the following keys are static assigned:
        /// <list type="table">
        /// <listheader>
        /// <item>Key</item><item>Description</item><item>Design Mode Only</item>
        /// </listheader>
        /// <item>Ctrl-A</item><item>Select all</item><item>-</item>
        /// <item>Ctrl-C</item><item>Copy selected text</item><item>-</item>
        /// <item>Ctrl-P</item><item>Print immediately using internal dialogs.</item><item>-</item>
        /// <item>Ctrl-U</item><item>Underline</item><item>+</item>
        /// <item>Ctrl-B</item><item>Bold</item><item>+</item>
        /// <item>Ctrl-I</item><item>Italic</item><item>+</item>
        /// <item>Ctrl-L</item><item>Align Left (Block only)</item><item>+</item>
        /// <item>Ctrl-R</item><item>Align Right (Block only)</item><item>+</item>
        /// <item>Ctrl-J</item><item>Justify (Multiline Block only)</item><item>+</item>
        /// <item>Ctrl-V</item><item>Paste</item><item>+</item>
        /// <item>Ctrl-X</item><item>Cut</item><item>+</item>
        /// <item>Ctrl-Y</item><item>Redo</item><item>+</item>
        /// <item>Ctrl-Z</item><item>Undo</item><item>+</item>
        /// </list>
        /// The property can be turned on and off during run time and will become active
        /// immediately. 
        /// <para>
        /// To strip out specific keys you can handle the 
        /// <see cref="GuruComponents.Netrix.HtmlEditor.BeforeShortcut">BeforeShortcut</see> event
        /// and set the <c>Cancel</c> property to true to disable the internal key handling. The same
        /// event can be used to assign more Shortcut keys in your application, because the event
        /// is fired on every kex in combination with the Ctrl (Control) key, even if the key stroke
        /// is not handled elsewhere.
        /// </para>
        /// </remarks>
        [DefaultValue(true), Browsable(true), Category("Netrix Component"), Description("Activates or deactivates the internal shortcut keys.")]
        public bool InternalShortcutKeys
        {
            get
            {
                return _internalShortcutKeys;
            }
            set
            {
                _internalShortcutKeys = value;
            }
        }

        /// <summary>
        /// The encoding of the document.
        /// </summary>
        /// <remarks>
        /// The encoding can be changed during design time to change the document encoding with next save operation.
        /// </remarks>
        /// <example>
        /// Changing the encoding is very straight. The following line will set the encoding for all following save operations to UTF-8:
        /// <code>
        /// this.htmlEditor1.Encoding = System.Text.Encoding.UTF8;
        /// </code>
        /// If you load a text from a string with non-Ascii characters the automatic encoding recognition needs
        /// to knwo which culture should be used. Therefore it is important to set the CurrentCulture property of
        /// the current thread correctly. Even if the most systems run correctly it is recommended to set the 
        /// current culture in all environments to a specific value.
        /// <seealso cref="LoadHtml(string)"/>
        /// <seealso cref="LoadUrl"/>
        /// <seealso cref="LoadFile"/>
        /// <seealso cref="GetRawHtml()"/>
        /// </example>
        [Browsable(false), ReadOnly(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Encoding Encoding
        {
            get
            {
                Encoding innerEncoding = DocumentStructure.Encoding;
                return (_encoding == null) ? innerEncoding : _encoding;
            }
            set
            {
                _encoding = value;
            }
        }


        /// <summary>
        /// Does not execute any script until fully activated. Browse Mode only.
        /// </summary>
        /// <remarks>
        /// <para>NetRix does not execute any script until fully activated. This flag is used to postpone 
        /// script execution until the host is active and, therefore, ready for script to be executed.
        /// </para>
        /// In design mode the scripting is always disabled and cannot be enabled by user code. When switching to browse
        /// mode or design mode to make scripting available remember to wait for <see cref="ReadyStateComplete"/> event before 
        /// issue any action or send commands.
        /// <para>Prior Version 1.6 the description was wrong, but the implemention was as intended.</para>
        /// <seealso cref="DesignModeEnabled"/>
        /// </remarks>
        [DefaultValue(true), Browsable(true), Category("Netrix Component"), Description("Does not execute any script until fully activated.")]
        public bool ScriptEnabled
        {
            get
            {
                return _scriptEnabled;
            }
            set
            {
                _scriptEnabled = value;
            }
        }

        /// <summary>
        /// Sets various styles regarding the scrollbars (runtime property only).
        /// </summary>
        /// <remarks>
        /// This option sets the current body style to specific scrollbar options. To make this property persistent, the host
        /// application should either save the complete document, including the body, or save the body style "overflow" property
        /// separatly. The following table maps the various options against the CSS:
        /// <list type="table">
        ///		<listheader>
        ///			<item>Property</item><item>CSS Style</item>
        ///		</listheader>
        ///		<term>
        ///			<item>None</item><item>hidden</item>
        ///		</term>
        ///		<term>
        ///			<item>ForcedBoth</item><item>scroll</item>
        ///		</term>
        ///		<term>
        ///			<item>Both</item><item>auto</item>
        ///		</term>
        ///		<term>
        ///			<item>Vertical, Horizontal, ForcedVertical, ForcedHorizontal</item><item>Not supported.</item>
        ///		</term>
        /// </list>
        /// <para>
        /// This property can set only after <see cref="ReadyStateComplete"/> event has been fired.
        /// </para>
        /// <para>
        /// This property is not accessible at design time, it can be set at runtime only.
        /// </para>
        /// <para>
        /// The get accessor will return RichTextBoxScrollBars.Both in case of non recognizable value or non-existing attribute, which
        /// is the controls default behavior.
        /// </para>
        /// <seealso cref="ScrollBarsEnabled"/>
        /// </remarks>
        /// <exception cref="NotSupportedException">Throws if the value cannot be transformed into corresponding CSS.</exception>
        [DefaultValue(RichTextBoxScrollBars.Both), Browsable(false), Category("Netrix Component"), Description("Various scrollbar options.")]
        public RichTextBoxScrollBars ScrollBars
        {
            get
            {
                if (!ThrowDocumentNotReadyException()) return RichTextBoxScrollBars.Both;
                object overflow = ((Interop.IHTMLElement2)GetBodyElement().GetBaseElement()).GetRuntimeStyle().GetAttribute("overflow", 0);
                if (overflow != null)
                {
                    switch (overflow.ToString())
                    {
                        case "hidden":
                            return RichTextBoxScrollBars.None;
                        case "auto":
                            return RichTextBoxScrollBars.Both;
                        case "scroll":
                            return RichTextBoxScrollBars.ForcedBoth;
                        default:
                            return RichTextBoxScrollBars.Both;
                    }
                }
                return RichTextBoxScrollBars.Both;
            }
            set
            {
                if (!ThrowDocumentNotReadyException()) return;
                Interop.IHTMLStyle style = ((Interop.IHTMLElement2)GetBodyElement()).GetRuntimeStyle();
                switch (value)
                {
                    case RichTextBoxScrollBars.None:
                        style.SetAttribute("overflow", "hidden", 0);
                        break;
                    case RichTextBoxScrollBars.Both:
                        style.SetAttribute("overflow", "auto", 0);
                        break;
                    case RichTextBoxScrollBars.ForcedBoth:
                        style.SetAttribute("overflow", "scroll", 0);
                        break;
                    default:
                        throw new NotSupportedException("This property value is not supported in underlying CSS.");
                }
            }
        }

        /// <summary>
        /// Enables or disables scrollbars.
        /// </summary>
        /// <remarks>
        /// This property works globally and turns scrollbars on or off for the next document being loaded.
        /// For more scrollbar options see <see cref="ScrollBars"/> property.
        /// <para>
        /// This property can be set at both, design time and run time.
        /// </para>
        /// <seealso cref="ScrollBars"/>
        /// </remarks>
        [DefaultValue(true), Browsable(true), Category("Netrix Component"), Description("Enables or disables scrollbars.")]
        public bool ScrollBarsEnabled
        {
            get
            {
                return _scrollBarsEnabled;
            }
            set
            {
                _scrollBarsEnabled = value;
            }
        }

        /// <summary>
        /// Allows access to the temp file if any exists, used for path resolvation.
        /// </summary>
        /// <remarks>
        /// The property may return <c>null</c> (<c>Nothing</c> in Visual Basic) in case of non existent temporary path.
        /// </remarks>
        [Browsable(false)]
        public string TempFile
        {
            get
            {
                return this.tmpFile;
            }
            set
            {
                this.tmpFile = value;
            }
        }

        /// <summary>
        /// Allows access to the base path if any, used for path resolvation.
        /// </summary>
        /// <remarks>
        /// The property may return <c>null</c> (<c>Nothing</c> in Visual Basic) in case of non existent temporary path.
        /// </remarks>
        internal string TempPath
        {
            get
            {
                return (this.tmpPath.StartsWith("file://")) ? this.tmpPath : String.Concat("file://", this.tmpPath);
            }
            set
            {
                this.tmpPath = (value.EndsWith("\\") || value.EndsWith("/")) ? value : value + Path.DirectorySeparatorChar;
            }
        }

        private bool _antiquirks = false;

        /// <summary>
        /// Sets the control into W3C compliant rendering mode.
        /// </summary>
        /// <remarks>
        /// This property changes the behavior of the document by setting the DOCTYPE tag on top of the document
        /// during creation. Existing documents will not change their behavior.
        /// </remarks>
        [Browsable(true), Category("Netrix Component"), Description("Sets the control into W3C compliant rendering mode.")]
        [DefaultValue(false)]
        public bool AntiQuirks
        {
            get
            {
                return _antiquirks;
            }
            set
            {
                _antiquirks = value;
            }
        }

        /// <summary>
        /// Gets the url of the document contained in the control.
        /// </summary>
        /// <remarks>
        /// If there is no path defined, the property returns <c>null</c>.
        /// </remarks>
        [Browsable(false), DefaultValue(null)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual string Url
        {
            get { return _url; }
            set { _url = value; }
        }
        private string _url;

        # region Control Design and Behavior


        /// <summary>
        /// Elements which could be influenced by TAB key should get TAB sequences in design mode.
        /// </summary>
        /// <remarks>
        /// This applies to:
        /// <list type="bullet">
        ///     <item>LI</item>
        /// </list>
        /// </remarks>
        [DefaultValue(false), Browsable(false), Category("Netrix Component"), Description("Whether or elements which could be influenced by TAB key should get TAB sequences in design mode.")]
        public bool HandleTAB
        {
            get { return _HandleTAB; }
            set { _HandleTAB = value; }
        }
        private bool _HandleTAB;

        /// <summary>
        /// Show component borders in 3D style.
        /// </summary>
        /// <remarks>
        /// The default is true (3D style used). Changing this property requieres re-load.
        /// </remarks>
        [DefaultValue(false), Browsable(false), Category("Netrix Component"), Description("Show component borders in 3D style. The default is true (3D style used).")]
        public bool Border3d
        {
            get { return _Border3d; }
            set { _Border3d = value; }
        }
        private bool _Border3d;

        /// <summary>
        /// Controls the way the component renders themeable controls.
        /// </summary>
        /// <remarks>
        /// Buttons, Select boxes and other form elements can be rendered during
        /// design time using XP themes, if the application runs on XP. If the designer
        /// is used to design pages which run later on other operating systems,
        /// it is recommended to switch theming support of.
        /// <para>
        /// The default value is <c>true</c> (Theming is on).
        /// </para>
        /// </remarks>
        [Browsable(true), DefaultValue(true)]
        [Category("Netrix Component"), Description("Controls the way the component renders themeable controls.")]
        public bool XPTheming
        {
            get
            {
                return _xptheming;
            }
            set
            {
                _xptheming = value;
            }
        }
        /// <summary>
        /// Sets how paragraphs will be inserted, DIV or P tags.
        /// </summary>
        /// <remarks>
        /// <seealso cref="GuruComponents.Netrix.BlockDefaultType"/>
        /// </remarks>
        [Browsable(true), DefaultValue(BlockDefaultType.P)]
        [Category("Netrix Component"), Description("Sets how paragraphs will be inserted, DIV or P tags.")]
        public BlockDefaultType BlockDefault
        {
            get
            {
                return _blockDefaultType;
            }
            set
            {
                _blockDefaultType = value;
            }
        }

        /// <summary>
        /// Set the behavior of links in browse mode.
        /// </summary>
        /// <remarks>
        /// If this property is set to <c>true</c> an the user clicks a link in browse mode the new page
        /// will load internally and replace the current page. If set to <c>false</c> the link will start
        /// a new instance of Internet Explorer (or the systems default browser) and opens the link 
        /// externally. Any further navigation operations are no longer under the control of NetRix.
        /// <para>
        /// This option is not recognized in design mode.
        /// </para>
        /// </remarks>
        [DefaultValue(true), Browsable(true), Category("Netrix Component"), Description("Allow to navigate in design mode without calling external browser.")]
        public bool AllowInPlaceNavigation
        {
            get
            {
                return _allowInPlaceNavigation;
            }
            set
            {
                _allowInPlaceNavigation = value;
            }
        }

        /// <summary>
        /// Disables the ability to select text if control is in browse mode.
        /// </summary>
        /// <remarks>
        /// This applies only if the content is loaded after setting the property.
        /// <para>
        /// Note: This features is available as of NetRix Pro 1.6. It's not available in Advanced or Standard.
        /// </para>
        /// </remarks>
        [DefaultValue(false), Browsable(true), Category("Netrix Component"), Description("Disables the ability to select text if control is in browse mode.")]
        public bool NoTextSelection
        {
            get
            {
                return _noTextSelection;
            }
            set
            {
                _noTextSelection = value;
            }
        }

        /// <summary>
        /// Allows computer users to employ IME reconversion while browsing Web pages.
        /// </summary>
        /// <remarks>
        /// During initialization, the host can set this flag to enable Input Method Editor (IME) reconversion, allowing computer users to employ IME reconversion while browsing Web pages. An input method editor is a program that allows users to enter complex characters and symbols, such as Japanese Kanji characters, using a standard keyboard.
        /// <para>
        /// Note: This features is available as of NetRix Pro 1.6.
        /// </para>
        /// </remarks>
        [DefaultValue(false), Browsable(true), Category("Netrix Component"), Description("Allows computer users to employ IME reconversion while browsing Web pages.")]
        public bool ImeReconversion
        {
            get
            {
                return _imeReconversion;
            }
            set
            {
                _imeReconversion = value;
            }
        }


        /// <summary>
        /// Gets or sets a value to determine full document mode.
        /// </summary>
        /// <remarks>
        /// The full document mode forces the basic structure of the loaded HTML document to 
        /// this one:
        /// <code>
        /// &lt;html&gt;
        /// &lt;body&gt;
        ///   &lt;!-- your HTML fragment goes here --&gt;
        /// &lt;/body&gt;
        /// &lt;/html&gt;
        /// </code>
        /// To avoid this behavior just set this property to <c>false</c> and reload the document.
        /// <para>
        /// Changing this property during an editing session will not be recognized.
        /// </para>
        /// </remarks>
        [Browsable(false), DefaultValue(true)]
        public bool IsFullDocumentMode
        {
            get
            {
                return _fullDocumentMode;
            }
            set
            {
                _fullDocumentMode = value;
            }
        }

        # endregion

        # region Editor Commands

        /// <summary>
        /// Indicates if the current selection can be copied.
        /// </summary>
        [Browsable(false)]
        public bool CanCopy
        {
            get
            {
                return IsCommandEnabled(Interop.IDM.COPY);
            }
        }

        /// <summary>
        /// Indicates if the current selection can be cut.
        /// </summary>
        [Browsable(false)]
        public bool CanCut
        {
            get
            {
                return IsCommandEnabled(Interop.IDM.CUT);
            }
        }

        /// <summary>
        /// Indicates if the current selection can be pasted to.
        /// </summary>
        [Browsable(false)]
        public bool CanPaste
        {
            get
            {
                return IsCommandEnabled(Interop.IDM.PASTE);
            }
        }

        /// <summary>
        /// Indicates if the current selection can be deleted.
        /// </summary>
        [Browsable(false)]
        public bool CanDelete
        {
            get
            {
                return IsCommandEnabled(Interop.IDM.DELETE);
            }
        }


        /// <summary>
        /// Indicates if the editor can redo.
        /// </summary>
        [Browsable(false)]
        public bool CanRedo
        {
            get
            {
                return IsCommandEnabled(Interop.IDM.REDO);
            }
        }

        /// <summary>
        /// Indicates if the editor can undo.
        /// </summary>
        [Browsable(false)]
        public bool CanUndo
        {
            get
            {
                return IsCommandEnabled(Interop.IDM.UNDO);
            }
        }


        /// <summary>
        /// Copy the current selection.
        /// </summary>
        /// <remarks>
        /// The command does nothing if the <see cref="CanCopy"/> property returns false.
        /// </remarks>
        public void Copy()
        {
            if (CanCopy)
            {
                Exec(Interop.IDM.COPY);
            }
        }

        /// <summary>
        /// Cut the current selection
        /// </summary>
        /// <remarks>
        /// The command does nothing if the <see cref="CanCut"/> property returns false.
        /// </remarks>
        public void Cut()
        {
            if (CanCut)
            {
                Exec(Interop.IDM.CUT);
            }
        }

        /// <summary>
        /// Cut the current selection.
        /// </summary>
        public void Paste()
        {
            if (CanPaste)
            {
                Exec(Interop.IDM.PASTE);
            }
        }

        /// <summary>
        /// Delete the current selection.
        /// </summary>
        public void Delete()
        {
            if (CanDelete)
            {
                Exec(Interop.IDM.DELETE);
            }
        }

        /// <summary>
        /// Shows flat or normal themed scrollbars
        /// </summary>
        [Browsable(true), DefaultValue(false)]
        [Category("Netrix Component"), Description("Shows flat or normal themed scrollbars")]
        public bool FlatScrollBars
        {
            get
            {
                return _flatScrollBars;
            }
            set
            {
                _flatScrollBars = value;
            }
        }

        private bool threadSafety = false;

        /// <summary>
        /// Turns on thread safe access to properties and methods.
        /// </summary>
        /// <remarks>
        /// By default the component isn't thread safe. However, the burdensome Invoke technique required to invoke
        /// cross thread calls is implemented internally and can be turned on or off using this property.
        /// The drawback is that permanent access in a threadsafe manner could result in the performance flaw. In
        /// case nobody needs cross thread access it's recommended to let this property set to <c>false</c>.
        /// </remarks>
        [DefaultValue(false), Browsable(true), Description("Turns on thread safe access to properties and methods."), Category("Netrix Component")]
        public bool ThreadSafety
        {
            get { return threadSafety; }
            set { threadSafety = value; }
        }

        # endregion

        #endregion

    }
}