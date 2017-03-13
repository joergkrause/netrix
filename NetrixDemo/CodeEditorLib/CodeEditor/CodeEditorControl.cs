#region using...

using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Design;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Windows.Forms;
using GuruComponents.CodeEditor.Library.Drawing.GDI;
using GuruComponents.CodeEditor.CodeEditor.Syntax;
using GuruComponents.CodeEditor.Forms;
using GuruComponents.CodeEditor.CodeEditor;
using GuruComponents.CodeEditor.CodeEditor.TextDraw;
using GuruComponents.CodeEditor.Library.Timers;
using GuruComponents.CodeEditor.Library.Win32;

#endregion

namespace GuruComponents.CodeEditor.CodeEditor
{
	/// <summary>
	/// Syntaxbox control that can be used as a pure text editor or as a code editor when a syntaxfile is used.
	/// </summary>
    [Designer(typeof (CodeEditorDesigner), typeof (IDesigner))]
	public class CodeEditorControl : Widget
	{

		protected internal bool DisableIntelliMouse = false;
		protected internal bool DisableFindForm = false;
		protected internal bool DisableAutoList = false;
		protected internal bool DisableInfoTip = false;
		protected internal bool DisableSplitView = false;
		protected internal bool DisableScrollBars = false;

        public event EventHandler FileNameChanged;
        public event EventHandler FileSavedChanged;

		#region General Declarations

		private IContainer components;
		private ArrayList _Views = null;
		private SyntaxDocument _Document = null;
		private int _TooltipDelay = 240;
		private int _TabSize = 4;
		private int _GutterMarginWidth = 19;
		private int _SmoothScrollSpeed = 2;
		private int _RowPadding = 0;
		private long _ticks = 0; //splitter doubleclick timer
		private bool _ShowWhitespace = false;
		private bool _ShowTabGuides = false;
		private bool _ShowLineNumbers = true;
		private bool _ShowGutterMargin = true;
		private bool _ReadOnly = false;
		private bool _HighLightActiveLine = false;
		private bool _VirtualWhitespace = false;
		private bool _BracketMatching = true;
		private bool _OverWrite = false;
		private bool _ParseOnPaste = false;
		private bool _SmoothScroll = false;
		private bool _AllowBreakPoints = true;
		private bool _LockCursorUpdate = false;
		private Color _BracketBorderColor = Color.DarkBlue;
		private Color _TabGuideColor = ControlPaint.Light(SystemColors.ControlLight);
		private Color _OutlineColor = SystemColors.ControlDark;
		private Color _WhitespaceColor = SystemColors.ControlDark;
		private Color _SeparatorColor = SystemColors.Control;
		private Color _SelectionBackColor = SystemColors.Highlight;
		private Color _SelectionForeColor = SystemColors.HighlightText;
		private Color _InactiveSelectionBackColor = SystemColors.ControlDark;
		private Color _InactiveSelectionForeColor = SystemColors.ControlLight;
		private Color _BreakPointBackColor = Color.DarkRed;
		private Color _BreakPointForeColor = Color.White;
		private Color _BackColor = Color.White;
		private Color _HighLightedLineColor = Color.LightYellow;
		private Color _GutterMarginColor = SystemColors.Control;
		private Color _LineNumberBackColor = SystemColors.Window;
		private Color _LineNumberForeColor = Color.Teal;
		private Color _GutterMarginBorderColor = SystemColors.ControlDark;
		private Color _LineNumberBorderColor = Color.Teal;
		private Color _BracketForeColor = Color.Black;
		private Color _BracketBackColor = Color.LightSteelBlue;
		private Color _ScopeBackColor = Color.Transparent;
		private Color _ScopeIndicatorColor = Color.Transparent;
		private TextDrawType _TextDrawStyle = 0;
		private IndentStyle _Indent = IndentStyle.LastRow;
		private string _FontName = "Courier New";
		private float _FontSize = 10f;
		private EditViewControl _ActiveView = null;
		private KeyboardActionList _KeyboardActions = new KeyboardActionList();

		private string[] TextBorderStyles = new string[]
			{
				"****** * ******* * ******",
				"+---+| | |+-+-+| | |+---+",
				"+---+¦ ¦ ¦¦-+-¦¦ ¦ ¦+---+",
				"+---+¦ ¦ ¦+-+-¦¦ ¦ ¦+---+"
			};

		#endregion

		#region Internal Components/Controls

		private SplitViewWidget splitView1;
		private EditViewControl UpperLeft;
		private EditViewControl UpperRight;
		private EditViewControl LowerLeft;
		private EditViewControl LowerRight;
		private ImageList _GutterIcons;
		private WeakTimer ParseTimer;
		private ImageList _AutoListIcons;

		#endregion

		#region Public Events

		/// <summary>
		/// An event that is fired when the cursor hovers a pattern;
		/// </summary>
		public event WordMouseHandler WordMouseHover = null;

		/// <summary>
		/// An event that is fired when the cursor hovers a pattern;
		/// </summary>
		public event WordMouseHandler WordMouseDown = null;

		/// <summary>
		/// An event that is fired when the control has updated the clipboard
		/// </summary>
		public event CopyHandler ClipboardUpdated = null;

		/// <summary>
		/// Event fired when the caret of the active view have moved.
		/// </summary>
		public event EventHandler CaretChange = null;

		/// <summary>
		/// 
		/// </summary>
		public event EventHandler SelectionChange = null;

		/// <summary>
		/// Event fired when the user presses the up or the down button on the infotip.
		/// </summary>
		public event EventHandler InfoTipSelectedIndexChanged = null;

		/// <summary>
		/// Event fired when a row is rendered.
		/// </summary>
		public event RowPaintHandler RenderRow = null;

		/// <summary>
		/// An event that is fired when mouse down occurs on a row
		/// </summary>
		public event RowMouseHandler RowMouseDown = null;

		/// <summary>
		/// An event that is fired when mouse move occurs on a row
		/// </summary>
		public event RowMouseHandler RowMouseMove = null;

		/// <summary>
		/// An event that is fired when mouse up occurs on a row
		/// </summary>
		public event RowMouseHandler RowMouseUp = null;

		/// <summary>
		/// An event that is fired when a click occurs on a row
		/// </summary>
		public event RowMouseHandler RowClick = null;

		/// <summary>
		/// An event that is fired when a double click occurs on a row
		/// </summary>
		public event RowMouseHandler RowDoubleClick = null;

		#endregion //END PUBLIC EGENTS

		#region Public Properties

		#region PUBLIC PROPERTY SHOWEOLMARKER

		private bool _ShowEOLMarker = false;

		[Category("Appearance"),
			Description("Determines if a ¶ should be displayed at the end of a line")]
		[DefaultValue(false)]
		public bool ShowEOLMarker
		{
			get { return _ShowEOLMarker; }
			set
			{
				_ShowEOLMarker = value;
				this.Redraw();
			}
		}

		#endregion

		#region PUBLIC PROPERTY EOLMARKERCOLOR

		private Color _EOLMarkerColor = Color.Red;

		[Category("Appearance"),
			Description("The color of the EOL marker")]
		[DefaultValue(typeof (Color), "Red")]
		public Color EOLMarkerColor
		{
			get { return _EOLMarkerColor; }
			set
			{
				_EOLMarkerColor = value;
				this.Redraw();
			}
		}

		#endregion

		#region PUBLIC PROPERTY AUTOLISTAUTOSELECT

		private bool _AutoListAutoSelect = true;

		[DefaultValue(true)]
		public bool AutoListAutoSelect
		{
			get { return _AutoListAutoSelect; }
			set { _AutoListAutoSelect = value; }
		}

		#endregion

		#region PUBLIC PROPERTY COPYASRTF

		private bool _CopyAsRTF = false;

		[Category("Behavior - Clipboard"),
			Description("determines if the copy actions should be stored as RTF")]
		[DefaultValue(typeof (Color), "false")]
		public bool CopyAsRTF
		{
			get { return _CopyAsRTF; }
			set { _CopyAsRTF = value; }
		}

		#endregion

		[Category("Appearance - Scopes"),
			Description("The color of the active scope")]
		[DefaultValue(typeof (Color), "Transparent")]
		public Color ScopeBackColor
		{
			get { return _ScopeBackColor; }
			set
			{
				_ScopeBackColor = value;
				this.Redraw();
			}
		}

		[Category("Appearance - Scopes"),
			Description("The color of the scope indicator")]
		[DefaultValue(typeof (Color), "Transparent")]
		public Color ScopeIndicatorColor
		{
			get { return _ScopeIndicatorColor; }
			set
			{
				_ScopeIndicatorColor = value;
				this.Redraw();
			}
		}

        #region PUBLIC PROPERTY SHOWSCOPEINDICATOR

        private bool _ShowScopeIndicator;

        [Category("Appearance - Scopes"), Description(
            "Determines if the scope indicator should be shown")
            ]
        [DefaultValue(true)]
        public bool ShowScopeIndicator
        {
            get { return _ShowScopeIndicator; }
            set
            {
                _ShowScopeIndicator = value;
                this.Redraw();
            }
        }

        #endregion

		/// <summary>
		/// Positions the AutoList
		/// </summary>
		[Category("Behavior")]
		[Browsable(false)]
        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public TextPoint AutoListPosition
		{
			get { return _ActiveView.AutoListPosition; }
			set
			{
				if (_ActiveView == null)
					return;

				_ActiveView.AutoListPosition = value;
			}
		}

		/// <summary>
		/// Positions the InfoTip
		/// </summary>
		[Category("Behavior")]
		[Browsable(false)]
        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public TextPoint InfoTipPosition
		{
			get { return _ActiveView.InfoTipPosition; }
			set
			{
				if (_ActiveView == null)
					return;

				_ActiveView.InfoTipPosition = value;
			}
		}

		[Browsable(false)]
		public int SplitviewV
		{
			get { return this.splitView1.SplitviewV; }
			set
			{
				if (this.splitView1 == null)
					return;

				this.splitView1.SplitviewV = value;
			}
		}

		[Browsable(false)]
		public int SplitviewH
		{
			get { return this.splitView1.SplitviewH; }
			set
			{
				if (this.splitView1 == null)
					return;
				this.splitView1.SplitviewH = value;
			}
		}


		/// <summary>
		/// Gets or Sets the active view
		/// </summary>
		[Browsable(false)]
       // [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ActiveView ActiveView
		{
			get
			{
				if (_ActiveView == UpperLeft)
					return ActiveView.TopLeft;

				if (_ActiveView == UpperRight)
					return ActiveView.TopRight;

				if (_ActiveView == LowerLeft)
					return ActiveView.BottomLeft;

				if (_ActiveView == LowerRight)
					return ActiveView.BottomRight;

				return (ActiveView) 0;
			}
			set
			{
				if (value != ActiveView.BottomRight)
				{
					ActivateSplits();
				}


				if (value == ActiveView.TopLeft)
					_ActiveView = UpperLeft;

				if (value == ActiveView.TopRight)
					_ActiveView = UpperRight;

				if (value == ActiveView.BottomLeft)
					_ActiveView = LowerLeft;

				if (value == ActiveView.BottomRight)
					_ActiveView = LowerRight;

			}

		}

		/// <summary>
		/// Prevents the control from changing the cursor.
		/// </summary>
		[Description("Prevents the control from changing the cursor.")]
		[Category("Appearance")]
		[Browsable(false)]
		public bool LockCursorUpdate
		{
			get { return _LockCursorUpdate; }
			set { _LockCursorUpdate = value; }
		}

		/// <summary>
		/// The row padding in pixels.
		/// </summary>
		[Category("Appearance"),
			Description("The number of pixels to add between rows")]
		[DefaultValue(0)]
		public int RowPadding
		{
			get { return _RowPadding; }
			set { _RowPadding = value; }
		}


		/// <summary>
		/// The selected index in the infotip.
		/// </summary>
		[Category("Appearance - Infotip"),
			Description("The currently active selection in the infotip")]
		[Browsable(false)]
		public int InfoTipSelectedIndex
		{
			get { return _ActiveView.InfoTip.SelectedIndex; }
			set
			{
				if (_ActiveView == null || _ActiveView.InfoTip == null)
					return;

				_ActiveView.InfoTip.SelectedIndex = value;
			}
		}

		/// <summary>
		/// Gets or Sets the image used in the infotip.
		/// </summary>
		[Category("Appearance - InfoTip"),
			Description("An image to show in the infotip")]
		[DefaultValue(null)]
       // [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Image InfoTipImage
		{
			get { return _ActiveView.InfoTip.Image; }
			set
			{
				if (_ActiveView == null || _ActiveView.InfoTip == null)
					return;


				_ActiveView.InfoTip.Image = value;
			}
		}

		/// <summary>
		/// Get or Sets the number of choices that could be made in the infotip.
		/// </summary>
		[Category("Appearance"),
			Description("Get or Sets the number of choices that could be made in the infotip")]
		[Browsable(false)]
		public int InfoTipCount
		{
			get { return _ActiveView.InfoTip.Count; }
			set
			{
				if (_ActiveView == null || _ActiveView.InfoTip == null)
					return;

				_ActiveView.InfoTip.Count = value;
				_ActiveView.InfoTip.Init();
			}
		}

		/// <summary>
		/// The text in the Infotip.
		/// </summary>
		/// <remarks><br/>
		/// The text uses a HTML like syntax.<br/>
		/// <br/>
		/// Supported tags are:<br/>
		/// <br/>
		/// &lt;Font Size="Size in Pixels" Face="Font Name" Color="Named color" &gt;&lt;/Font&gt; Set Font size,color and fontname.<br/>
		/// &lt;HR&gt; : Inserts a horizontal separator line.<br/>
		/// &lt;BR&gt; : Line break.<br/>
		/// &lt;B&gt;&lt;/B&gt; : Activate/Deactivate Bold style.<br/>
		/// &lt;I&gt;&lt;/I&gt; : Activate/Deactivate Italic style.<br/>
		/// &lt;U&gt;&lt;/U&gt; : Activate/Deactivate Underline style.	<br/>			
		/// </remarks>	
		/// <example >
		/// <code>
		/// MySyntaxBox.InfoTipText="public void MyMethod ( &lt;b&gt; string text &lt;/b&gt; );"; 		
		/// </code>
		/// </example>	
		[Category("Appearance - InfoTip"),
			Description("The infotip text")]
		[DefaultValue("")]
		public string InfoTipText
		{
			get { return _ActiveView.InfoTip.Data; }
			set
			{
				if (_ActiveView == null || _ActiveView.InfoTip == null)
					return;

				_ActiveView.InfoTip.Data = value;
			}
		}

		/// <summary>
		/// Gets the Selection object from the active view.
		/// </summary>
		[Browsable(false)]
     //   [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Selection Selection
		{
			get
			{
				if (_ActiveView != null)
				{
					return _ActiveView.Selection;
				}
				return null;
			}
		}

		/// <summary>
		/// Collection of KeyboardActions that is used by the control.
		/// Keyboard actions to add shortcut key combinations to certain tasks.
		/// </summary>
		[Browsable(false)]
        [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden)]
		public KeyboardActionList KeyboardActions
		{
			get { return _KeyboardActions; }
			set { _KeyboardActions = value; }
		}

		/// <summary>
		/// Gets or Sets if the AutoList is visible in the active view.
		/// </summary>
		[Category("Appearance"),
			Description("Gets or Sets if the AutoList is visible in the active view.")]
		[Browsable(false)]
		public bool AutoListVisible
		{
			get
			{
				if (_ActiveView != null)
					return _ActiveView.AutoListVisible;
				else
					return false;
			}
			set
			{
				if (_ActiveView != null)
					_ActiveView.AutoListVisible = value;
			}
		}

		/// <summary>
		/// Gets or Sets if the InfoTip is visible in the active view.
		/// </summary>
		[Category("Appearance"),
			Description("Gets or Sets if the InfoTip is visible in the active view.")]
		[Browsable(false)]
		public bool InfoTipVisible
		{
			get
			{
				if (_ActiveView != null)
					return _ActiveView.InfoTipVisible;
				else
					return false;
			}
			set
			{
				if (_ActiveView != null)
					_ActiveView.InfoTipVisible = value;
			}
		}

		/// <summary>
		/// Gets if the control can perform a Copy action.
		/// </summary>
		[Browsable(false)]
		public bool CanCopy
		{
			get { return _ActiveView.CanCopy; }
		}

		/// <summary>
		/// Gets if the control can perform a Paste action.
		/// (if the clipboard contains a valid text).
		/// </summary>
		[Browsable(false)]
		public bool CanPaste
		{
			get { return _ActiveView.CanPaste; }
		}


		/// <summary>
		/// Gets if the control can perform a ReDo action.
		/// </summary>
		[Browsable(false)]
		public bool CanRedo
		{
			get { return _ActiveView.CanRedo; }
		}

		/// <summary>
		/// Gets if the control can perform an Undo action.
		/// </summary>
		[Browsable(false)]
		public bool CanUndo
		{
			get { return _ActiveView.CanUndo; }
		}

		/// <summary>
		/// Gets or Sets the imagelist to use in the gutter margin.
		/// </summary>
		/// <remarks>
		/// Image Index 0 is used to display the Breakpoint icon.
		/// Image Index 1 is used to display the Bookmark icon.
		/// </remarks>		
		[Category("Appearance - Gutter Margin"),
			Description("Gets or Sets the imagelist to use in the gutter margin.")]
		public ImageList GutterIcons
		{
			get { return _GutterIcons; }
			set
			{
				_GutterIcons = value;
				this.Redraw();
			}

		}

		/// <summary>
		/// Gets or Sets the imagelist to use in the autolist.
		/// </summary>
		[Category("Appearance"),
			Description("Gets or Sets the imagelist to use in the autolist.")]
		[DefaultValue(null)]
		public ImageList AutoListIcons
		{
			get { return _AutoListIcons; }
			set
			{
				_AutoListIcons = value;


				foreach (EditViewControl ev in Views)
				{
					if (ev != null && ev.AutoList != null)
						ev.AutoList.Images = value;
				}
				this.Redraw();
			}

		}

		/// <summary>
		/// Gets or Sets the border styles of the split views.
		/// </summary>
		[Category("Appearance - Borders")]
		[Description("Gets or Sets the border styles of the split views.")]
        [DefaultValue(ControlBorderStyle.FixedSingle)]
		public ControlBorderStyle ChildBorderStyle
		{
			get { return ((EditViewControl) Views[0]).BorderStyle; }
			set
			{
				foreach (EditViewControl ev in this.Views)
				{
					ev.BorderStyle = value;
				}
			}
		}

		/// <summary>
		/// Gets or Sets the border color of the split views.
		/// </summary>
		[Category("Appearance - Borders")]
		[Description("Gets or Sets the border color of the split views.")]
		[DefaultValue(typeof (Color), "ControlDark")]
		public Color ChildBorderColor
		{
			get { return ((EditViewControl) Views[0]).BorderColor; }
			set
			{
				foreach (EditViewControl ev in this.Views)
				{
					if (ev != null)
					{
						ev.BorderColor = value;
					}
				}
			}
		}

		/// <summary>
		/// Gets or Sets the color to use when rendering Tab guides.
		/// </summary>
		[Category("Appearance - Tabs")]
		[Description("Gets or Sets the color to use when rendering Tab guides.")]
		[DefaultValue(typeof (Color), "Control")]
		public Color TabGuideColor
		{
			get { return _TabGuideColor; }
			set
			{
				_TabGuideColor = value;
				this.Redraw();
			}
		}

		/// <summary>
		/// Gets or Sets the color of the bracket match borders.
		/// </summary>
		/// <remarks>
		/// NOTE: use Color.Transparent to turn off the bracket match borders.
		/// </remarks>
		[Category("Appearance - Bracket Match")]
		[Description("Gets or Sets the color of the bracket match borders.")]
		[DefaultValue(typeof (Color), "DarkBlue")]
		public Color BracketBorderColor
		{
			get { return _BracketBorderColor; }
			set
			{
				_BracketBorderColor = value;
				this.Redraw();
			}
		}

		/// <summary>
		/// Gets or Sets if the control should render Tab guides.
		/// </summary>
		[Category("Appearance - Tabs")]
		[Description("Gets or Sets if the control should render Tab guides.")]
		[DefaultValue(false)]
		public bool ShowTabGuides
		{
			get { return _ShowTabGuides; }
			set
			{
				_ShowTabGuides = value;
				this.Redraw();
			}

		}

		/// <summary>
		/// Gets or Sets the color to use when rendering whitespace characters
		/// </summary>
		[Category("Appearance")]
		[Description("Gets or Sets the color to use when rendering whitespace characters.")]
		[DefaultValue(typeof (Color), "Control")]
		public Color WhitespaceColor
		{
			get { return _WhitespaceColor; }
			set
			{
				_WhitespaceColor = value;
				this.Redraw();
			}
		}

		/// <summary>
		/// Gets or Sets the color of the code Outlining (both folding lines and collapsed blocks).
		/// </summary>
		[Category("Appearance")]
		[Description("Gets or Sets the color of the code Outlining (both folding lines and collapsed blocks).")]
		[DefaultValue(typeof (Color), "ControlDark")]
		public Color OutlineColor
		{
			get { return _OutlineColor; }
			set
			{
				_OutlineColor = value;
				InitGraphics();
				this.Redraw();
			}
		}


		/// <summary>
		/// Determines if the control should use a smooth scroll when scrolling one row up or down.
		/// </summary>
		[Category("Behavior")]
		[Description("Determines if the control should use a smooth scroll when scrolling one row up or down.")]
		[DefaultValue(typeof (Color), "False")]
		public bool SmoothScroll
		{
			get { return _SmoothScroll; }
			set { _SmoothScroll = value; }
		}

		/// <summary>
		/// Gets or Sets the speed of the vertical scroll when SmoothScroll is activated
		/// </summary>
		[Category("Behavior")]
		[Description("Gets or Sets the speed of the vertical scroll when SmoothScroll is activated")]
		[DefaultValue(2)]
		public int SmoothScrollSpeed
		{
			get { return _SmoothScrollSpeed; }
			set
			{
				if (value <= 0)
				{
					throw(new Exception("Scrollsped may not be less than 1"));
				}
				else
					_SmoothScrollSpeed = value;
			}
		}

		/// <summary>
		/// Gets or Sets if the control can display breakpoints or not.
		/// </summary>
		[Category("Behavior")]
		[Description("Gets or Sets if the control can display breakpoints or not.")]
		[DefaultValue(true)]
		public bool AllowBreakPoints
		{
			get { return _AllowBreakPoints; }
			set { _AllowBreakPoints = value; }

		}

		/// <summary>
		/// Gets or Sets if the control should perform a full parse of the document when content is drag dropped or pasted into the control
		/// </summary>
		[Category("Behavior - Clipboard")]
		[Description("Gets or Sets if the control should perform a full parse of the document when content is drag dropped or pasted into the control")]
		[DefaultValue(false)]
		public bool ParseOnPaste
		{
			get { return _ParseOnPaste; }
			set
			{
				_ParseOnPaste = value;
				this.Redraw();
			}
		}

		/// <summary>
		/// Returns true if the control is in overwrite mode.
		/// </summary>
		[Browsable(false)]
		public bool OverWrite
		{
            get { return this._ActiveView.OverWrite; }
		}


		/// <summary>
		/// Gets or Sets the Size of the font.
		/// <seealso cref="FontName"/>
		/// </summary>
		[Category("Appearance - Font")]
		[Description("The size of the font")]
		[DefaultValue(10f)]
		public float FontSize
		{
			get { return _FontSize; }
			set
			{
				_FontSize = value;
				InitGraphics();
				this.Redraw();
			}
		}

		/// <summary>
		/// Determines what indentstyle to use on a new line.
		/// </summary>
		[Category("Behavior")]
		[Description("Determines how the the control indents a new line")]
		[DefaultValue(IndentStyle.LastRow)]
		public IndentStyle Indent
		{
			get { return _Indent; }
			set { _Indent = value; }
		}

		/// <summary>
		/// Gets or Sets the SyntaxDocument the control is currently attatched to.
		/// </summary>
		[Category("Content")]
		[Description("The SyntaxDocument that is attatched to the contro")]
		public SyntaxDocument Document
		{
			get { return _Document; }
			set { AttachDocument(value); }

		}

		/// <summary>
		/// Get or Set the delay before the tooltip is displayed over a collapsed block
		/// </summary>
		[Category("Behavior")]
		[Description("The delay before the tooltip is displayed over a collapsed block")]
		[DefaultValue(240)]
		public int TooltipDelay
		{
			get { return _TooltipDelay; }
			set { _TooltipDelay = value; }

		}

		/// <summary>
		/// Get or Set the delay before the tooltip is displayed over a collapsed block
		/// </summary>
		[Category("Behavior")]
		[Description("Determines if the control is readonly or not")]
		[DefaultValue(false)]
		public bool ReadOnly
		{
			get { return _ReadOnly; }
			set { _ReadOnly = value; }
		}

		/// <summary>
		/// Gets or Sets the name of the font.
		/// <seealso cref="FontSize"/>
		/// </summary>
		[Category("Appearance - Font")]
		[Description("The name of the font that is used to render the control")]
		[Editor(typeof (FontList), typeof (UITypeEditor))]
		[DefaultValue("Courier New")]
		public string FontName
		{
			get { return _FontName; }
			set
			{
				if (this.Views == null)
					return;

				_FontName = value;
				InitGraphics();
				foreach (EditViewControl evc in this.Views)
					evc.CalcMaxCharWidth();

				this.Redraw();
			}
		}

		/// <summary>
		/// Determines the style to use when painting with alt+arrow keys.
		/// </summary>
		[Category("Behavior")]
		[Description("Determines what type of chars to use when painting with ALT+arrow keys")]
		[DefaultValue(TextDrawType.StarBorder)]
		public TextDrawType TextDrawStyle
		{
			get { return _TextDrawStyle; }
			set { _TextDrawStyle = value; }
		}

		/// <summary>
		/// Gets or Sets if bracketmatching is active
		/// <seealso cref="BracketForeColor"/>
		/// <seealso cref="BracketBackColor"/>
		/// </summary>
		[Category("Appearance - Bracket Match")]
		[Description("Determines if the control should highlight scope patterns")]
		[DefaultValue(true)]
		public bool BracketMatching
		{
			get { return _BracketMatching; }
			set
			{
				_BracketMatching = value;
				this.Redraw();
			}
		}

		/// <summary>
		/// Gets or Sets if Virtual Whitespace is active.
		/// <seealso cref="ShowWhitespace"/>
		/// </summary>
		[Category("Behavior")]
		[Description("Determines if virtual Whitespace is active")]
		[DefaultValue(false)]
		public bool VirtualWhitespace
		{
			get { return _VirtualWhitespace; }
			set
			{
				_VirtualWhitespace = value;
				this.Redraw();
			}
		}

		/// <summary>
		/// Gets or Sets the separator Color.
		/// <seealso cref="BracketMatching"/>
		/// <seealso cref="BracketBackColor"/>
		/// </summary>
		[Category("Appearance")]
		[Description("The separator color")]
		[DefaultValue(typeof (Color), "Control")]
		public Color SeparatorColor
		{
			get { return _SeparatorColor; }
			set
			{
				_SeparatorColor = value;
				this.Redraw();
			}
		}


		/// <summary>
		/// Gets or Sets the foreground Color to use when BracketMatching is activated.
		/// <seealso cref="BracketMatching"/>
		/// <seealso cref="BracketBackColor"/>
		/// </summary>
		[Category("Appearance - Bracket Match")]
		[Description("The foreground color to use when BracketMatching is activated")]
		[DefaultValue(typeof (Color), "Black")]
		public Color BracketForeColor
		{
			get { return _BracketForeColor; }
			set
			{
				_BracketForeColor = value;
				this.Redraw();
			}
		}

		/// <summary>
		/// Gets or Sets the background Color to use when BracketMatching is activated.
		/// <seealso cref="BracketMatching"/>
		/// <seealso cref="BracketForeColor"/>
		/// </summary>
		[Category("Appearance - Bracket Match")]
		[Description("The background color to use when BracketMatching is activated")]
		[DefaultValue(typeof (Color), "LightSteelBlue")]
		public Color BracketBackColor
		{
			get { return _BracketBackColor; }
			set
			{
				_BracketBackColor = value;
				this.Redraw();
			}
		}


		/// <summary>
		/// The inactive selection background color.
		/// </summary>
		[Category("Appearance - Selection")]
		[Description("The inactive selection background color.")]
		[DefaultValue(typeof (Color), "ControlDark")]
		public Color InactiveSelectionBackColor
		{
			get { return _InactiveSelectionBackColor; }
			set
			{
				_InactiveSelectionBackColor = value;
				this.Redraw();
			}
		}

		/// <summary>
		/// The inactive selection foreground color.
		/// </summary>
		[Category("Appearance - Selection")]
		[Description("The inactive selection foreground color.")]
		[DefaultValue(typeof (Color), "ControlLight")]
		public Color InactiveSelectionForeColor
		{
			get { return _InactiveSelectionForeColor; }
			set
			{
				_InactiveSelectionForeColor = value;
				this.Redraw();
			}
		}

		/// <summary>
		/// The selection background color.
		/// </summary>
		[Category("Appearance - Selection")]
		[Description("The selection background color.")]
		[DefaultValue(typeof (Color), "Highlight")]
		public Color SelectionBackColor
		{
			get { return _SelectionBackColor; }
			set
			{
				_SelectionBackColor = value;
				this.Redraw();
			}
		}

		/// <summary>
		/// The selection foreground color.
		/// </summary>
		[Category("Appearance - Selection")]
		[Description("The selection foreground color.")]
		[DefaultValue(typeof (Color), "HighlightText")]
		public Color SelectionForeColor
		{
			get { return _SelectionForeColor; }
			set
			{
				_SelectionForeColor = value;
				this.Redraw();
			}
		}

		/// <summary>
		/// Gets or Sets the border Color of the gutter margin.
		/// <seealso cref="GutterMarginColor"/>
		/// </summary>
		[Category("Appearance - Gutter Margin")]
		[Description("The border color of the gutter margin")]
		[DefaultValue(typeof (Color), "ControlDark")]
		public Color GutterMarginBorderColor
		{
			get { return _GutterMarginBorderColor; }
			set
			{
				_GutterMarginBorderColor = value;
				InitGraphics();
				this.Redraw();
			}
		}

		/// <summary>
		/// Gets or Sets the border Color of the line number margin
		/// <seealso cref="LineNumberForeColor"/>
		/// <seealso cref="LineNumberBackColor"/>
		/// </summary>
		[Category("Appearance - Line Numbers")]
		[Description("The border color of the line number margin")]
		[DefaultValue(typeof (Color), "Teal")]
		public Color LineNumberBorderColor
		{
			get { return _LineNumberBorderColor; }
			set
			{
				_LineNumberBorderColor = value;
				InitGraphics();
				this.Redraw();
			}
		}


		/// <summary>
		/// Gets or Sets the foreground Color of a Breakpoint.
		/// <seealso cref="BreakPointBackColor"/>
		/// </summary>
		[Category("Appearance - BreakPoints")]
		[Description("The foreground color of a Breakpoint")]
		[DefaultValue(typeof (Color), "White")]
		public Color BreakPointForeColor
		{
			get { return _BreakPointForeColor; }
			set
			{
				_BreakPointForeColor = value;
				this.Redraw();
			}
		}

		/// <summary>
		/// Gets or Sets the background Color to use for breakpoint rows.
		/// <seealso cref="BreakPointForeColor"/>
		/// </summary>
		[Category("Appearance - BreakPoints")]
		[Description("The background color to use when BracketMatching is activated")]
		[DefaultValue(typeof (Color), "DarkRed")]
		public Color BreakPointBackColor
		{
			get { return _BreakPointBackColor; }
			set
			{
				_BreakPointBackColor = value;
				this.Redraw();
			}
		}

		/// <summary>
		/// Gets or Sets the foreground Color of line numbers.
		/// <seealso cref="LineNumberBorderColor"/>
		/// <seealso cref="LineNumberBackColor"/>
		/// </summary>
		[Category("Appearance - Line Numbers")]
		[Description("The foreground color of line numbers")]
		[DefaultValue(typeof (Color), "Teal")]
		public Color LineNumberForeColor
		{
			get { return _LineNumberForeColor; }
			set
			{
				_LineNumberForeColor = value;
				InitGraphics();
				this.Redraw();
			}
		}

		/// <summary>
		/// Gets or Sets the background Color of line numbers.
		/// <seealso cref="LineNumberForeColor"/>
		/// <seealso cref="LineNumberBorderColor"/>
		/// </summary>
		[Category("Appearance - Line Numbers")]
		[Description("The background color of line numbers")]
		[DefaultValue(typeof (Color), "Window")]
		public Color LineNumberBackColor
		{
			get { return _LineNumberBackColor; }
			set
			{
				_LineNumberBackColor = value;
				InitGraphics();
				this.Redraw();
			}
		}

		/// <summary>
		/// Gets or Sets the Color of the gutter margin
		/// <seealso cref="GutterMarginBorderColor"/>
		/// </summary>
		[Category("Appearance - Gutter Margin")]
		[Description("The color of the gutter margin")]
		[DefaultValue(typeof (Color), "Control")]
		public Color GutterMarginColor
		{
			get { return _GutterMarginColor; }
			set
			{
				_GutterMarginColor = value;
				InitGraphics();
				this.Redraw();
			}
		}

		/// <summary>
		/// Gets or Sets the background Color of the client area.
		/// </summary>
		[Category("Appearance")]
		[Description("The background color of the client area")]
		[DefaultValue(typeof (Color), "Window")]
		new public Color BackColor
		{
			get { return _BackColor; }
			set
			{
				_BackColor = value;
				InitGraphics();
				this.Redraw();
			}
		}

		/// <summary>
		/// Gets or Sets the background Color of the active line.
		/// <seealso cref="HighLightActiveLine"/>
		/// </summary>
		[Category("Appearance - Active Line")]
		[Description("The background color of the active line")]
		[DefaultValue(typeof (Color), "LightYellow")]
		public Color HighLightedLineColor
		{
			get { return _HighLightedLineColor; }
			set
			{
				_HighLightedLineColor = value;
				InitGraphics();
				this.Redraw();
			}
		}

		/// <summary>
		/// Determines if the active line should be highlighted.
		/// </summary>
		[Category("Appearance - Active Line")]
		[Description("Determines if the active line should be highlighted")]
		[DefaultValue(false)]
		public bool HighLightActiveLine
		{
			get { return _HighLightActiveLine; }
			set
			{
				_HighLightActiveLine = value;
				this.Redraw();
			}
		}

		/// <summary>
		/// Determines if Whitespace should be rendered as symbols.
		/// </summary>
		[Category("Appearance")]
		[Description("Determines if Whitespace should be rendered as symbols")]
		[DefaultValue(false)]
		public bool ShowWhitespace
		{
			get { return _ShowWhitespace; }
			set
			{
				_ShowWhitespace = value;
				this.Redraw();
			}
		}

		/// <summary>
		/// Determines if the line number margin should be visible.
		/// </summary>
		[Category("Appearance - Line Numbers")]
		[Description("Determines if the line number margin should be visible")]
		[DefaultValue(true)]
		public bool ShowLineNumbers
		{
			get { return _ShowLineNumbers; }
			set
			{
				_ShowLineNumbers = value;
				this.Redraw();
			}
		}

		/// <summary>
		/// Determines if the gutter margin should be visible.
		/// </summary>
		[Category("Appearance - Gutter Margin")]
		[Description("Determines if the gutter margin should be visible")]
		[DefaultValue(true)]
		public bool ShowGutterMargin
		{
			get { return _ShowGutterMargin; }
			set
			{
				_ShowGutterMargin = value;
				this.Redraw();
			}
		}

		/// <summary>
		/// Gets or Sets the witdth of the gutter margin in pixels.
		/// </summary>
		[Category("Appearance - Gutter Margin")]
		[Description("Determines the witdth of the gutter margin in pixels")]
		[DefaultValue(19)]
		public int GutterMarginWidth
		{
			get { return _GutterMarginWidth; }
			set
			{
				_GutterMarginWidth = value;
				this.Redraw();
			}

		}

		/// <summary>
		/// Get or Sets the size of a TAB char in number of SPACES.
		/// </summary>
		[Category("Appearance - Tabs")]
		[Description("Determines the size of a TAB in number of SPACE chars")]
		[DefaultValue(4)]
		public int TabSize
		{
			get { return _TabSize; }
			set
			{
				_TabSize = value;
				this.Redraw();
			}
		}

		#region public property ScrollBars

		private ScrollBars _ScrollBars;

		[Category("Appearance"),
			Description("Determines what Scrollbars should be visible")]
		[DefaultValue(ScrollBars.Both)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ScrollBars ScrollBars
		{
			get { return _ScrollBars; }

			set
			{
				if (_Views == null)
					return;

				if (DisableScrollBars)
					value = ScrollBars.None;

				foreach (EditViewControl evc in _Views)
				{
					evc.ScrollBars = value;
				}
				_ScrollBars = value;
			}
		}

		#endregion 

		#region public property SplitView

		//member variable
		private bool _SplitView;

		[Category("Appearance"),
			Description("Determines if the controls should use splitviews")]        
		[DefaultValue(true)]
		public bool SplitView
		{
			get { return _SplitView; }

			set
			{
				_SplitView = value;

				if (this.splitView1 == null)
					return;

				if (!SplitView)
				{
					this.splitView1.Visible = false;
					this.Controls.Add(LowerRight);
					LowerRight.HideThumbs();
					LowerRight.Dock = DockStyle.Fill;
				}
				else
				{
					this.splitView1.Visible = true;
					this.splitView1.LowerRight = LowerRight;
					LowerRight.Dock = DockStyle.None;
					LowerRight.ShowThumbs();
				}
			}
		}

		#endregion //END PROPERTY SplitView

		#endregion // PUBLIC PROPERTIES

		#region Public Methods

		/// <summary>
		/// Resets the Splitview.
		/// </summary>
		public void ResetSplitview()
		{
			this.splitView1.ResetSplitview();
		}

		public void ScrollIntoView(int RowIndex)
		{
			_ActiveView.ScrollIntoView(RowIndex);
		}

		/// <summary>
		/// Disables painting while loading data into the Autolist
		/// </summary>
		/// <remarks>
		/// 
		/// </remarks>
		/// <example>
		/// <code>
		/// MySyntaxBox.AutoListClear();
		/// MySyntaxBox.AutoListBeginLoad();
		/// MySyntaxBox.AutoListAdd ("test",1);
		/// MySyntaxBox.AutoListAdd ("test",2);
		/// MySyntaxBox.AutoListAdd ("test",3);
		/// MySyntaxBox.AutoListAdd ("test",4);
		/// MySyntaxBox.AutoListEndLoad();
		/// </code>
		/// </example>
		public void AutoListBeginLoad()
		{
			this._ActiveView.AutoListBeginLoad();
		}

		/// <summary>
		/// Resumes painting and autosizes the Autolist.			
		/// </summary>		
		public void AutoListEndLoad()
		{
			this._ActiveView.AutoListEndLoad();
		}

		/// <summary>
		/// Clears the content in the autolist.
		/// </summary>
		public void AutoListClear()
		{
			this._ActiveView.AutoList.Clear();
		}

		/// <summary>
		/// Adds an item to the autolist control.
		/// </summary>
		/// <example>
		/// <code>
		/// MySyntaxBox.AutoListClear();
		/// MySyntaxBox.AutoListBeginLoad();
		/// MySyntaxBox.AutoListAdd ("test",1);
		/// MySyntaxBox.AutoListAdd ("test",2);
		/// MySyntaxBox.AutoListAdd ("test",3);
		/// MySyntaxBox.AutoListAdd ("test",4);
		/// MySyntaxBox.AutoListEndLoad();
		/// </code>
		/// </example>
		/// <param name="Text">The text to display in the autolist</param>
		/// <param name="ImageIndex">The image index in the AutoListIcons</param>
		public void AutoListAdd(string Text, int ImageIndex)
		{
			this._ActiveView.AutoList.Add(Text, ImageIndex);
		}

		/// <summary>
		/// Adds an item to the autolist control.
		/// </summary>
		/// <param name="Text">The text to display in the autolist</param>
		/// <param name="InsertText">The text to insert in the code</param>
		/// <param name="ImageIndex">The image index in the AutoListIcons</param>
		public void AutoListAdd(string Text, string InsertText, int ImageIndex)
		{
			this._ActiveView.AutoList.Add(Text, InsertText, ImageIndex);
		}

		/// <summary>
		/// Adds an item to the autolist control.
		/// </summary>
		/// <param name="Text">The text to display in the autolist</param>
		/// <param name="InsertText">The text to insert in the code</param>
		/// <param name="ToolTip"></param>
		/// <param name="ImageIndex">The image index in the AutoListIcons</param>
		public void AutoListAdd(string Text, string InsertText, string ToolTip, int ImageIndex)
		{
			this._ActiveView.AutoList.Add(Text, InsertText, ToolTip, ImageIndex);
		}

		/// <summary>
		/// Converts a Client pixel coordinate into a TextPoint (Column/Row)
		/// </summary>
		/// <param name="x">Pixel x position</param>
		/// <param name="y">Pixel y position</param>
		/// <returns>The row and column at the given pixel coordinate.</returns>
		public TextPoint CharFromPixel(int x, int y)
		{
			return _ActiveView.CharFromPixel(x, y);
		}

		/// <summary>
		/// Clears the selection in the active view.
		/// </summary>
		public void ClearSelection()
		{
			_ActiveView.ClearSelection();
		}

		/// <summary>
		/// Executes a Copy action on the selection in the active view.
		/// </summary>
		public void Copy()
		{
			_ActiveView.Copy();
            this.Saved = false;
		}

		/// <summary>
		/// Executes a Cut action on the selection in the active view.
		/// </summary>
		public void Cut()
		{
			_ActiveView.Cut();
            this.Saved = false;
		}

		/// <summary>
		/// Executes a Delete action on the selection in the active view.
		/// </summary>
		public void Delete()
		{
			_ActiveView.Delete();
		}

		/// <summary>
		/// Moves the caret of the active view to a specific row.
		/// </summary>
		/// <param name="RowIndex">the row to jump to</param>
		public void GotoLine(int RowIndex)
		{
			_ActiveView.GotoLine(RowIndex);
		}

		/// <summary>
		/// Moves the caret of the active view to the next bookmark.
		/// </summary>
		public void GotoNextBookmark()
		{
			_ActiveView.GotoNextBookmark();
		}

		/// <summary>
		/// Moves the caret of the active view to the previous bookmark.
		/// </summary>
		public void GotoPreviousBookmark()
		{
			_ActiveView.GotoPreviousBookmark();
		}


		/// <summary>
		/// Takes a pixel position and returns true if that position is inside the selected text.
		/// 
		/// </summary>
		/// <param name="x">Pixel x position.</param>
		/// <param name="y">Pixel y position</param>
		/// <returns>true if the position is inside the selection.</returns>
		public bool IsOverSelection(int x, int y)
		{
			return _ActiveView.IsOverSelection(x, y);
		}

		/// <summary>
		/// Execute a Paste action if possible.
		/// </summary>
		public void Paste()
		{
			_ActiveView.Paste();
            this.Saved = false;
		}

		/// <summary>
		/// Execute a ReDo action if possible.
		/// </summary>
		public void Redo()
		{
			_ActiveView.Redo();
            this.Saved = false;
		}

		/// <summary>
		/// Makes the caret in the active view visible on screen.
		/// </summary>
		public void ScrollIntoView()
		{
			_ActiveView.ScrollIntoView();
		}

		/// <summary>
		/// Scrolls the active view to a specific position.
		/// </summary>
		/// <param name="Pos"></param>
		public void ScrollIntoView(TextPoint Pos)
		{
			_ActiveView.ScrollIntoView(Pos);
		}

		/// <summary>
		/// Select all the text in the active view.
		/// </summary>
		public void SelectAll()
		{
			_ActiveView.SelectAll();
		}

		/// <summary>
		/// Selects the next word (from the current caret position) that matches the parameter criterias.
		/// </summary>
		/// <param name="Pattern">The pattern to find</param>
		/// <param name="MatchCase">Match case , true/false</param>
		/// <param name="WholeWords">Match whole words only , true/false</param>
		/// <param name="UseRegEx">To be implemented</param>
		public void FindNext(string Pattern, bool MatchCase, bool WholeWords, bool UseRegEx)
		{
			_ActiveView.SelectNext(Pattern, MatchCase, WholeWords, UseRegEx);
		}

		/// <summary>
		/// Finds the next occurance of the pattern in the find/replace dialog
		/// </summary>
		public void FindNext()
		{
			_ActiveView.FindNext();
		}

		/// <summary>
		/// Shows the default GotoLine dialog.
		/// </summary>
		/// <example>
		/// <code>
		/// //Display the Goto Line dialog
		/// MySyntaxBox.ShowGotoLine();
		/// </code>
		/// </example>
		public void ShowGotoLine()
		{
			_ActiveView.ShowGotoLine();
		}

		/// <summary>
		/// Not yet implemented
		/// </summary>
		public void ShowSettings()
		{
			_ActiveView.ShowSettings();
		}

		/// <summary>
		/// Toggles a bookmark on the active row of the active view.
		/// </summary>
		public void ToggleBookmark()
		{
			_ActiveView.ToggleBookmark();
		}

		/// <summary>
		/// Executes an undo action if possible.
		/// </summary>
		public void Undo()
		{
			_ActiveView.Undo();

            this.Saved = false;
		}


		/// <summary>
		/// Shows the Find dialog
		/// </summary>
		/// <example>
		/// <code>
		/// //Show FindReplace dialog
		/// MySyntaxBox.ShowFind();
		/// </code>
		/// </example>
		public void ShowFind()
		{
			_ActiveView.ShowFind();
		}

		/// <summary>
		/// Shows the Replace dialog
		/// </summary>
		/// <example>
		/// <code>
		/// //Show FindReplace dialog
		/// MySyntaxBox.ShowReplace();
		/// </code>
		/// </example>
		public void ShowReplace()
		{
			_ActiveView.ShowReplace();
		}


		/// <summary>
		/// Gets the Caret object from the active view.
		/// </summary>
		[Browsable(false)]
		public Caret Caret
		{
			get
			{
				if (_ActiveView != null)
				{
					return _ActiveView.Caret;
				}
				return null;
			}
		}

		#endregion //END Public Methods

        //protected virtual void OnCreate()
        //{
        //}

        private bool _Saved = false;

        public bool Saved
        {
            get
            {
                return _Saved;
            }
            set
            {
                _Saved = value;

                this.OnFileSavedChanged(EventArgs.Empty);
            }
        }



        private string _FileName;

        public string FileName
        {
            get
            {
                return _FileName;
            }
        }

        private void SetFileName(string filename)
        {
            _FileName = filename;
            this.OnFileNameChanged(new EventArgs());
        }


        protected virtual void OnFileNameChanged(EventArgs e)
        {
            if (FileNameChanged != null)
                FileNameChanged(null, e);
        }

        protected virtual void OnFileSavedChanged(EventArgs e)
        {
            if (FileSavedChanged != null)            
                FileSavedChanged(this, e);            
        }

        /// <overloads/>
        /// <summary>
        /// Save current content to the given file.
        /// </summary>
        public void Save(string filename)
        {
            string text = this.Document.Text;

            StreamWriter swr = new StreamWriter(filename);

            swr.Write(text);

            swr.Flush();

            swr.Close();

            SetFileName(filename);

            this.Saved = true;
        }

        /// <overloads/>
        /// <summary>
        /// Save current content to the file the content was loaded from.
        /// </summary>
        /// <exception cref="IOException">Thrown if content was not loaded from file.</exception>
        public void Save()
        {
            if (this.FileName == null)
                throw new IOException("Invalid Filename.");

            this.Save(this.FileName);
        }

        /// <summary>
        /// Open the given file and shows the content within the current document.
        /// </summary>
        /// <exception cref="NullReferenceException">Thrown if the current document is <c>null</c> (not initialized).</exception>
        /// <param name="filename">The full path to the file being opened.</param>
        public void Open(string filename)
        {
            if (this.Document == null)
                throw new NullReferenceException("CodeEditorControl.Document");

            StreamReader swr = new StreamReader(filename);

            this.Document.Text = swr.ReadToEnd();

            swr.Close();

            SetFileName(filename);



            this.Saved = true;
        }

        public void ReLoadFile()
        {
            if (this.FileName == null)
                throw new IOException("Invalid Filename.");

            this.Open(this.FileName);
        }


        public EditViewControl ActiveViewControl
        {
            get
            {
                return _ActiveView;
            }
        }

		#region Constructor

		/// <summary>
		/// Default constructor for the SyntaxBoxControl
		/// </summary>
		public CodeEditorControl() : base()
		{
			this.Document = new SyntaxDocument();
			//this.OnCreate();

			if (!DisableSplitView)
			{
				this.splitView1 = new SplitViewWidget();

				this.LowerRight = new EditViewControl(this);

			}
			else
			{
				this.LowerRight = new EditViewControl(this);
				this.Controls.Add(LowerRight);
				LowerRight.HideThumbs();
				LowerRight.Dock = DockStyle.Fill;
			}

//			this.UpperLeft.Visible = false;
//			this.UpperRight.Visible = false;
//			this.LowerLeft.Visible = false;
//			this.LowerRight.Visible = false;
//			this.splitView1.Visible = false;


			this.SuspendLayout();
			// 
			// splitView1
			// 

			if (!this.DisableSplitView)
			{
				this.splitView1.SuspendLayout();
				this.splitView1.BackColor = SystemColors.Control;
				this.splitView1.Controls.AddRange(new Control[]
					{
						this.LowerRight
					});
				this.splitView1.Dock = DockStyle.Fill;
				this.splitView1.Location = new Point(0, 0);
				this.splitView1.LowerRight = this.LowerRight;
				this.splitView1.Name = "splitView1";
				this.splitView1.Size = new Size(500, 364);
				this.splitView1.TabIndex = 0;


			}
			// 
			// LowerRight
			// 
			this.LowerRight.AllowDrop = true;
			this.LowerRight.BorderColor = Color.White;
            this.LowerRight.BorderStyle = ControlBorderStyle.None;
			this.LowerRight.Location = new Point(148, 124);
			this.LowerRight.Name = "LowerRight";
			this.LowerRight.Size = new Size(352, 240);
			this.LowerRight.TabIndex = 3;
			this.LowerRight.TextDrawStyle = TextDrawType.StarBorder;

			// 
			// SyntaxBoxControl
			// 

			if (!this.DisableSplitView)
				this.Controls.AddRange(new Control[]
					{
						this.splitView1
					});

			this.Name = "SyntaxBoxControl";
			this.Size = new Size(504, 368);
			if (!this.DisableSplitView)
			{
				this.splitView1.ResumeLayout(false);
				this.splitView1.Resizing += new EventHandler(this.SplitView_Resizing);
				this.splitView1.HideLeft += new EventHandler(this.SplitView_HideLeft);
				this.splitView1.HideTop += new EventHandler(this.SplitView_HideTop);
			}

			this.ResumeLayout(false);


			this.Views = new ArrayList();
			CreateViews();
			_ActiveView = LowerRight;


			InitializeComponent();
			this.SetStyle(ControlStyles.Selectable, true);

			//assign keys
			KeyboardActions.Add(new KeyboardAction(Keys.Z, false, true, false, false, new ActionDelegate(this.Undo)));
			KeyboardActions.Add(new KeyboardAction(Keys.Y, false, true, false, false, new ActionDelegate(this.Redo)));

			KeyboardActions.Add(new KeyboardAction(Keys.F3, false, false, false, true, new ActionDelegate(this.FindNext)));

			KeyboardActions.Add(new KeyboardAction(Keys.C, false, true, false, true, new ActionDelegate(this.Copy)));
			KeyboardActions.Add(new KeyboardAction(Keys.X, false, true, false, false, new ActionDelegate(this.CutClear)));
			KeyboardActions.Add(new KeyboardAction(Keys.V, false, true, false, false, new ActionDelegate(this.Paste)));

			KeyboardActions.Add(new KeyboardAction(Keys.Insert, false, true, false, true, new ActionDelegate(this.Copy)));
			KeyboardActions.Add(new KeyboardAction(Keys.Delete, true, false, false, false, new ActionDelegate(this.Cut)));
			KeyboardActions.Add(new KeyboardAction(Keys.Insert, true, false, false, false, new ActionDelegate(this.Paste)));

			KeyboardActions.Add(new KeyboardAction(Keys.A, false, true, false, true, new ActionDelegate(this.SelectAll)));

			KeyboardActions.Add(new KeyboardAction(Keys.F, false, true, false, false, new ActionDelegate(this.ShowFind)));
			KeyboardActions.Add(new KeyboardAction(Keys.H, false, true, false, false, new ActionDelegate(this.ShowReplace)));
			KeyboardActions.Add(new KeyboardAction(Keys.G, false, true, false, true, new ActionDelegate(this.ShowGotoLine)));
			KeyboardActions.Add(new KeyboardAction(Keys.T, false, true, false, false, new ActionDelegate(this.ShowSettings)));

			KeyboardActions.Add(new KeyboardAction(Keys.F2, false, true, false, true, new ActionDelegate(this.ToggleBookmark)));
			KeyboardActions.Add(new KeyboardAction(Keys.F2, false, false, false, true, new ActionDelegate(this.GotoNextBookmark)));
			KeyboardActions.Add(new KeyboardAction(Keys.F2, true, false, false, true, new ActionDelegate(this.GotoPreviousBookmark)));

			KeyboardActions.Add(new KeyboardAction(Keys.Escape, false, false, false, true, new ActionDelegate(this.ClearSelection)));

			KeyboardActions.Add(new KeyboardAction(Keys.Tab, false, false, false, false, new ActionDelegate(Selection.Indent)));
			KeyboardActions.Add(new KeyboardAction(Keys.Tab, true, false, false, false, new ActionDelegate(Selection.Outdent)));

			AutoListIcons = _AutoListIcons;
			this.SplitView = true;
			this.ScrollBars = ScrollBars.Both;
            this.BorderStyle = ControlBorderStyle.None;
			this.ChildBorderColor = SystemColors.ControlDark;
            this.ChildBorderStyle = ControlBorderStyle.FixedSingle;
			this.BackColor = SystemColors.Window;

//			this.UpperLeft.Visible = true;
//			this.UpperRight.Visible = true;
//			this.LowerLeft.Visible = true;
//			this.LowerRight.Visible = true;
//			this.splitView1.Visible = true;

		}

		#endregion //END Constructor

		private void ActivateSplits()
		{
			if (this.UpperLeft == null)
			{
				this.UpperLeft = new EditViewControl(this);
				this.UpperRight = new EditViewControl(this);
				this.LowerLeft = new EditViewControl(this);


				// 
				// UpperLeft
				// 
				this.UpperLeft.AllowDrop = true;
				this.UpperLeft.Name = "UpperLeft";
				this.UpperLeft.TabIndex = 6;
				// 
				// UpperRight
				// 
				this.UpperRight.AllowDrop = true;
				this.UpperRight.Name = "UpperRight";
				this.UpperRight.TabIndex = 4;
				// 
				// LowerLeft
				// 
				this.LowerLeft.AllowDrop = true;
				this.LowerLeft.Name = "LowerLeft";
				this.LowerLeft.TabIndex = 5;


				this.splitView1.Controls.AddRange(new Control[]
					{
						this.UpperLeft,
						this.LowerLeft,
						this.UpperRight
					});

				this.splitView1.UpperRight = this.LowerLeft;
				this.splitView1.UpperLeft = this.UpperLeft;
				this.splitView1.LowerLeft = this.UpperRight;

				CreateViews();

				this.AutoListIcons = this.AutoListIcons;
				this.InfoTipImage = this.InfoTipImage;
				this.ChildBorderStyle = this.ChildBorderStyle;
				this.ChildBorderColor = this.ChildBorderColor;
				this.BackColor = this.BackColor;
				this.Document = this.Document;
				this.Redraw();
			}
		}

		#region EventHandlers

		protected virtual void OnClipboardUpdated(CopyEventArgs e)
		{
			if (ClipboardUpdated != null)
				ClipboardUpdated(this, e);
		}

		protected virtual void OnRowMouseDown(RowMouseEventArgs e)
		{
			if (RowMouseDown != null)
				RowMouseDown(this, e);
		}

		protected virtual void OnRowMouseMove(RowMouseEventArgs e)
		{
			if (RowMouseMove != null)
				RowMouseMove(this, e);
		}

		protected virtual void OnRowMouseUp(RowMouseEventArgs e)
		{
			if (RowMouseUp != null)
				RowMouseUp(this, e);
		}

		protected virtual void OnRowClick(RowMouseEventArgs e)
		{
			if (RowClick != null)
				RowClick(this, e);
		}

		protected virtual void OnRowDoubleClick(RowMouseEventArgs e)
		{
			if (RowDoubleClick != null)
				RowDoubleClick(this, e);
		}


		private void ParseTimer_Tick(object sender, EventArgs e)
		{
			Document.ParseSome();
		}

		protected virtual void OnInfoTipSelectedIndexChanged()
		{
			if (InfoTipSelectedIndexChanged != null)
				InfoTipSelectedIndexChanged(null, null);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnEnter(EventArgs e)
		{
			base.OnEnter(e);
			if (_ActiveView != null)
			{
				_ActiveView.Focus();
			}
		}

		private void TopThumb_DoubleClick(object sender, EventArgs e)
		{
			//splitView1.Split5050h ();
		}

		private void LeftThumb_DoubleClick(object sender, EventArgs e)
		{
			//splitView1.Split5050v ();
		}

		private void TopThumb_MouseDown(object sender, MouseEventArgs e)
		{
			this.ActivateSplits();

			long t = DateTime.Now.Ticks - _ticks;
			_ticks = DateTime.Now.Ticks;


			if (t < 3000000)
			{
				splitView1.Split5050h();
			}
			else
			{
				splitView1.InvokeMouseDownh();
			}
		}

		private void LeftThumb_MouseDown(object sender, MouseEventArgs e)
		{
			this.ActivateSplits();

			long t = DateTime.Now.Ticks - _ticks;
			_ticks = DateTime.Now.Ticks;


			if (t < 3000000)
			{
				splitView1.Split5050v();
			}
			else
			{
				splitView1.InvokeMouseDownv();
			}
		}

		private void SplitView_Resizing(object sender, EventArgs e)
		{
			LowerRight.TopThumbVisible = false;
			LowerRight.LeftThumbVisible = false;
		}

		private void SplitView_HideTop(object sender, EventArgs e)
		{
			LowerRight.TopThumbVisible = true;
		}

		private void SplitView_HideLeft(object sender, EventArgs e)
		{
			LowerRight.LeftThumbVisible = true;
		}

		private void View_Enter(object sender, EventArgs e)
		{
			this._ActiveView = (EditViewControl) sender;
		}

		private void View_Leave(object sender, EventArgs e)
		{
			//	((EditViewControl)sender).RemoveFocus ();
		}

		private void View_RowClick(object sender, RowMouseEventArgs e)
		{
			OnRowClick(e);
		}

		private void View_RowDoubleClick(object sender, RowMouseEventArgs e)
		{
			OnRowDoubleClick(e);
		}

		private void View_RowMouseDown(object sender, RowMouseEventArgs e)
		{
			OnRowMouseDown(e);
		}

		private void View_RowMouseMove(object sender, RowMouseEventArgs e)
		{
			OnRowMouseMove(e);
		}

		private void View_RowMouseUp(object sender, RowMouseEventArgs e)
		{
			OnRowMouseUp(e);
		}

		private void View_ClipboardUpdated(object sender, CopyEventArgs e)
		{
			this.OnClipboardUpdated(e);
		}


		public void OnRenderRow(RowPaintEventArgs e)
		{
			if (RenderRow != null)
				RenderRow(this, e);
		}

		public void OnWordMouseHover(ref WordMouseEventArgs e)
		{
			if (WordMouseHover != null)
				WordMouseHover(this, ref e);
		}

		public void OnWordMouseDown(ref WordMouseEventArgs e)
		{
			if (WordMouseDown != null)
				WordMouseDown(this, ref e);
		}

		protected virtual void OnCaretChange(object sender)
		{
			if (CaretChange != null)
				CaretChange(this, null);
		}

		protected virtual void OnSelectionChange(object sender)
		{
			if (SelectionChange != null)
				SelectionChange(this, null);
		}

		private void View_CaretChanged(object s, EventArgs e)
		{
			OnCaretChange(s);
		}

		private void View_SelectionChanged(object s, EventArgs e)
		{
			OnSelectionChange(s);
		}

		private void View_DoubleClick(object sender, EventArgs e)
		{
			OnDoubleClick(e);
		}

		private void View_MouseUp(object sender, MouseEventArgs e)
		{
			EditViewControl ev = (EditViewControl) sender;
			MouseEventArgs ea = new MouseEventArgs(e.Button, e.Clicks, e.X + ev.Location.X + ev.BorderWidth, e.Y + ev.Location.Y + ev.BorderWidth, e.Delta);
			OnMouseUp(ea);
		}

		private void View_MouseMove(object sender, MouseEventArgs e)
		{
			EditViewControl ev = (EditViewControl) sender;
			MouseEventArgs ea = new MouseEventArgs(e.Button, e.Clicks, e.X + ev.Location.X + ev.BorderWidth, e.Y + ev.Location.Y + ev.BorderWidth, e.Delta);
			OnMouseMove(ea);
		}

		private void View_MouseLeave(object sender, EventArgs e)
		{
			OnMouseLeave(e);
		}

		private void View_MouseHover(object sender, EventArgs e)
		{
			OnMouseHover(e);
		}

		private void View_MouseEnter(object sender, EventArgs e)
		{
			OnMouseEnter(e);
		}

		private void View_MouseDown(object sender, MouseEventArgs e)
		{
			EditViewControl ev = (EditViewControl) sender;
			MouseEventArgs ea = new MouseEventArgs(e.Button, e.Clicks, e.X + ev.Location.X + ev.BorderWidth, e.Y + ev.Location.Y + ev.BorderWidth, e.Delta);
			OnMouseDown(ea);
		}

		private void View_KeyUp(object sender, KeyEventArgs e)
		{
			OnKeyUp(e);
            this.Saved = false;
		}

		private void View_KeyPress(object sender, KeyPressEventArgs e)
		{
			OnKeyPress(e);
		}

		private void View_KeyDown(object sender, KeyEventArgs e)
		{
			OnKeyDown(e);
		}

		private void View_Click(object sender, EventArgs e)
		{
			OnClick(e);
		}

		private void View_DragOver(object sender, DragEventArgs e)
		{
			OnDragOver(e);
		}

		private void View_DragLeave(object sender, EventArgs e)
		{
			OnDragLeave(e);
		}

		private void View_DragEnter(object sender, DragEventArgs e)
		{
			OnDragEnter(e);
		}

		private void View_DragDrop(object sender, DragEventArgs e)
		{
			OnDragDrop(e);
            this.Saved = false;
		}

		private void View_InfoTipSelectedIndexChanged(object sender, EventArgs e)
		{
			OnInfoTipSelectedIndexChanged();
		}

		#endregion

		#region Private Properties

		private ArrayList Views
		{
			get { return _Views; }
			set { _Views = value; }
		}

		#endregion

		#region DISPOSE()

		/// <summary>
		/// </summary>
		/// <param name="disposing"></param>
		protected override void Dispose(bool disposing)
		{
			//must destroy license
#if DEBUG
			try
			{
				Console.WriteLine("disposing syntaxbox");
			}
			catch
			{
			}
#endif


			if (disposing)
			{
				if (components != null)
					components.Dispose();
			}
			base.Dispose(disposing);
		}

		#endregion //END DISPOSE

		/// <summary>
		/// 
		/// </summary>
		~CodeEditorControl()
		{
#if DEBUG
			try
			{
				Console.WriteLine("finalizing syntaxbox");
			}
			catch
			{
			}
#endif
		}

		#region Private/Protected/Internal methods

		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CodeEditorControl));
            this._GutterIcons = new System.Windows.Forms.ImageList(this.components);
            this._AutoListIcons = new System.Windows.Forms.ImageList(this.components);
            this.ParseTimer = new GuruComponents.CodeEditor.Library.Timers.WeakTimer(this.components);
            this.SuspendLayout();
            // 
            // _GutterIcons
            // 
            this._GutterIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("_GutterIcons.ImageStream")));
            this._GutterIcons.TransparentColor = System.Drawing.Color.Transparent;
            this._GutterIcons.Images.SetKeyName(0, "break_point.png");
            this._GutterIcons.Images.SetKeyName(1, "");
            this._GutterIcons.Images.SetKeyName(2, "");
            this._GutterIcons.Images.SetKeyName(3, "");
            this._GutterIcons.Images.SetKeyName(4, "");
            this._GutterIcons.Images.SetKeyName(5, "");
            this._GutterIcons.Images.SetKeyName(6, "");
            this._GutterIcons.Images.SetKeyName(7, "");
            this._GutterIcons.Images.SetKeyName(8, "");
            this._GutterIcons.Images.SetKeyName(9, "");
            this._GutterIcons.Images.SetKeyName(10, "");
            this._GutterIcons.Images.SetKeyName(11, "");
            this._GutterIcons.Images.SetKeyName(12, "");
            // 
            // _AutoListIcons
            // 
            this._AutoListIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("_AutoListIcons.ImageStream")));
            this._AutoListIcons.TransparentColor = System.Drawing.Color.Transparent;
            this._AutoListIcons.Images.SetKeyName(0, "");
            this._AutoListIcons.Images.SetKeyName(1, "");
            this._AutoListIcons.Images.SetKeyName(2, "");
            this._AutoListIcons.Images.SetKeyName(3, "");
            this._AutoListIcons.Images.SetKeyName(4, "");
            this._AutoListIcons.Images.SetKeyName(5, "");
            this._AutoListIcons.Images.SetKeyName(6, "");
            this._AutoListIcons.Images.SetKeyName(7, "");
            this._AutoListIcons.Images.SetKeyName(8, "");
            // 
            // ParseTimer
            // 
            this.ParseTimer.Enabled = true;
            this.ParseTimer.Interval = 1;
            this.ParseTimer.Tick += new System.EventHandler(this.ParseTimer_Tick);
            this.ResumeLayout(false);

		}


		protected override void OnLoad(EventArgs e)
		{
			this.Refresh();
		}

		private void Redraw()
		{
			if (this.Views == null)
				return;

			foreach (EditViewControl ev in this.Views)
			{
				if (ev != null)
				{
					ev.Refresh();
				}
			}
		}

		private void InitGraphics()
		{
			if (this.Views == null || this.Parent == null)
				return;

			foreach (EditViewControl ev in this.Views)
			{
				ev.InitGraphics();
			}
		}


		private bool DoOnce = false;

		private void CreateViews()
		{
			if (UpperRight != null)
			{
				Views.Add(UpperRight);
				Views.Add(UpperLeft);
				Views.Add(LowerLeft);
			}

			if (!DoOnce)
			{
				Views.Add(LowerRight);
				LowerRight.TopThumbVisible = true;
				LowerRight.LeftThumbVisible = true;
				LowerRight.TopThumb.DoubleClick += new EventHandler(TopThumb_DoubleClick);
				LowerRight.LeftThumb.DoubleClick += new EventHandler(LeftThumb_DoubleClick);

				LowerRight.TopThumb.MouseDown += new MouseEventHandler(TopThumb_MouseDown);
				LowerRight.LeftThumb.MouseDown += new MouseEventHandler(LeftThumb_MouseDown);


			}


			foreach (EditViewControl ev in Views)
			{
				if (DoOnce && ev == this.LowerRight)
					continue;

				//attatch events to views
				ev.Enter += new EventHandler(this.View_Enter);
				ev.Leave += new EventHandler(this.View_Leave);
				ev.GotFocus += new EventHandler(this.View_Enter);
				ev.LostFocus += new EventHandler(this.View_Leave);
				ev.CaretChange += new EventHandler(this.View_CaretChanged);
				ev.SelectionChange += new EventHandler(this.View_SelectionChanged);
				ev.Click += new EventHandler(this.View_Click);
				ev.DoubleClick += new EventHandler(this.View_DoubleClick);
				ev.MouseDown += new MouseEventHandler(this.View_MouseDown);
				ev.MouseEnter += new EventHandler(this.View_MouseEnter);
				ev.MouseHover += new EventHandler(this.View_MouseHover);
				ev.MouseLeave += new EventHandler(this.View_MouseLeave);
				ev.MouseMove += new MouseEventHandler(this.View_MouseMove);
				ev.MouseUp += new MouseEventHandler(this.View_MouseUp);
				ev.KeyDown += new KeyEventHandler(this.View_KeyDown);
				ev.KeyPress += new KeyPressEventHandler(this.View_KeyPress);
				ev.KeyUp += new KeyEventHandler(this.View_KeyUp);
				ev.DragDrop += new DragEventHandler(this.View_DragDrop);
				ev.DragOver += new DragEventHandler(this.View_DragOver);
				ev.DragLeave += new EventHandler(this.View_DragLeave);
				ev.DragEnter += new DragEventHandler(this.View_DragEnter);

				if (ev.InfoTip != null)
				{
					ev.InfoTip.Data = "";
					ev.InfoTip.SelectedIndexChanged += new EventHandler(this.View_InfoTipSelectedIndexChanged);
				}

				ev.RowClick += new RowMouseHandler(this.View_RowClick);
				ev.RowDoubleClick += new RowMouseHandler(this.View_RowDoubleClick);

				ev.RowMouseDown += new RowMouseHandler(this.View_RowMouseDown);
				ev.RowMouseMove += new RowMouseHandler(this.View_RowMouseMove);
				ev.RowMouseUp += new RowMouseHandler(this.View_RowMouseUp);
				ev.ClipboardUpdated += new CopyHandler(this.View_ClipboardUpdated);

			}

			DoOnce = true;
			this.Redraw();

		}

		#endregion //END Private/Protected/Internal methods

		public void AttachDocument(SyntaxDocument document)
		{
			//_Document=document;

			if (_Document != null)
			{
				_Document.ParsingCompleted -= new EventHandler(this.OnParsingCompleted);
				_Document.Parsing -= new EventHandler(this.OnParse);
				_Document.Change -= new EventHandler(this.OnChange);
			}

			if (document == null)
				document = new SyntaxDocument();

			_Document = document;

			if (this._Document != null)
			{
				_Document.ParsingCompleted += new EventHandler(this.OnParsingCompleted);
				_Document.Parsing += new EventHandler(this.OnParse);
				_Document.Change += new EventHandler(this.OnChange);
			}

			this.Redraw();
		}

		protected virtual void OnParse(object Sender, EventArgs e)
		{
			foreach (EditViewControl ev in Views)
			{
				ev.OnParse();
			}
		}

		protected virtual void OnParsingCompleted(object Sender, EventArgs e)
		{
			foreach (EditViewControl ev in Views)
			{
				ev.Invalidate();
			}
		}

		protected virtual void OnChange(object Sender, EventArgs e)
		{
			if (Views == null)
				return;


			foreach (EditViewControl ev in Views)
			{
				ev.OnChange();
			}
			this.OnTextChanged(EventArgs.Empty);
		}

		public void RemoveCurrentRow()
		{
			_ActiveView.RemoveCurrentRow();
		}

		public void CutClear()
		{
			_ActiveView.CutClear();
		}


		[Browsable(false)]
		[Obsolete("Use .FontName and .FontSize", true)]
		public override Font Font
		{
			get { return base.Font; }
			set { base.Font = value; }
		}



//		[Browsable(true)]
//		[DesignerSerializationVisibility (DesignerSerializationVisibility.Hidden)]
//		[RefreshProperties (RefreshProperties.All)]
//		public override string Text
//		{
//			get
//			{
//				return this.Document.Text;
//			}
//			set
//			{
//				this.Document.Text=value;
//			}
//		}

		[Browsable(false)]
		[Obsolete("Apply a syntax instead", true)]
		public override Color ForeColor
		{
			get { return base.ForeColor; }
			set { base.ForeColor = value; }
		}

		public void AutoListInsertSelectedText()
		{
			_ActiveView.InsertAutolistText();
		}

		/// <summary>
		/// The currently highlighted text in the autolist.
		/// </summary>
		[Browsable(false)]
		public string AutoListSelectedText
		{
			get { return _ActiveView.AutoList.SelectedText; }
			set
			{
				if (_ActiveView == null || _ActiveView.AutoList == null)
					return;

				_ActiveView.AutoList.SelectItem(value);
			}
		}


		protected override void WndProc(ref Message m)
		{
			base.WndProc(ref m);
			if (m.Msg == (int) WindowMessage.WM_SETFOCUS)
			{
				if (_ActiveView != null)
					_ActiveView.Focus();
			}
		}




	}
}