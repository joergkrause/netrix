using System;
using System.ComponentModel;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using System.ComponentModel.Design;
using System.Drawing.Design;
          
namespace GuruComponents.Netrix.UserInterface.ToolBox
{

    /// <summary>
    /// Provides a simple toolbox implementation for editor purposes.
    /// </summary>
    /// <remarks>
    /// Content of toolbox can be created from XML document as well programmatically by calling
    /// methods. Several events support interaction.
    /// </remarks>
    [DesignerAttribute(typeof(GuruComponents.Netrix.UserInterface.ToolBox.Toolbox.WindowMessageDesigner), typeof(IDesigner))]
    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(GuruComponents.Netrix.UserInterface.ResourceManager), "Resources.ToolBox.Toolbox.ico")]
    public class Toolbox : System.Windows.Forms.UserControl
    {

        private IToolboxService toolboxService = null;
        private const int _hMargin = 2;
        private const int _vMargin = 2;
        /// <summary>
        /// Tab name for XML configuration.
        /// </summary>
        public const string XmlTabName = "ToolBoxTab";
        /// <summary>
        /// Item name for XML configuration.
        /// </summary>
        public const string XmlItemName = "ToolBoxItem";

        #region Component Designer generated code
        private System.Windows.Forms.Panel _panel;
        private System.Windows.Forms.Timer _timer;
        private System.Windows.Forms.ToolTip _toolTip;
        private System.ComponentModel.IContainer components;

        /// <summary> 
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this._panel = new System.Windows.Forms.Panel();
            this._timer = new System.Windows.Forms.Timer(this.components);
            this._toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // _panel
            // 
            this._panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._panel.Location = new System.Drawing.Point(0, 0);
            this._panel.Name = "_panel";
            this._panel.Size = new System.Drawing.Size(153, 328);
            this._panel.TabIndex = 0;
            // 
            // _timer
            // 
            this._timer.Interval = 200;
            this._timer.Tick += new System.EventHandler(this.OnTimerTick);
            // 
            // _toolTip
            // 
            this._toolTip.AutoPopDelay = 3000;
            this._toolTip.InitialDelay = 1000;
            this._toolTip.ReshowDelay = 100;
            // 
            // Toolbox
            // 
            this.Controls.Add(this._panel);
            this.Name = "Toolbox";
            this.Size = new System.Drawing.Size(153, 328);
            this.Load += new System.EventHandler(this.OnLoadControl);
            this.ResumeLayout(false);

        }
        #endregion

        /// <summary>
        /// Controls windows messages for flexible mouse managing.
        /// </summary>
        public class WindowMessageDesigner : System.Windows.Forms.Design.ControlDesigner
        {
            /// <summary>
            /// Ctor.
            /// </summary>
            public WindowMessageDesigner()
            {
            }

            /// <summary>
            /// Window procedure override passes events to control.
            /// </summary>
            /// <param name="m"></param>
            [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
            protected override void WndProc(ref System.Windows.Forms.Message m)
            {
                if (m.HWnd == this.Control.Handle)
                    base.WndProc(ref m);
                else
                    this.DefWndProc(ref m);
            }
        }

        /// <summary>
        /// Fired when user clicks on another tab.
        /// </summary>
        public event ToolboxEventHandler TabChanged = null;
        /// <summary>
        /// Fired when an item is selected by user action.
        /// </summary>
        public event ToolboxEventHandler ItemSelect = null;
        /// <summary>
        /// Fired when the is going into active state by either user of programmatic action.
        /// </summary>
        public event ToolboxEventHandler ItemActivate = null;

        private Color _buttonColor = SystemColors.Control;
        private Color _openTabBackColor = SystemColors.ControlLight;
        private Color _openTabForeColor = SystemColors.ControlText;
        private SolidBrush _backgroundBrush = new SolidBrush(SystemColors.Control);
        private SolidBrush _selectionBrush = new SolidBrush(SystemColors.ControlLight);
        private SolidBrush _textBrush = new SolidBrush(SystemColors.ControlText);
        private SolidBrush _itemHooverBrush = new SolidBrush(SystemColors.Control);
        private Border3DStyle _itemHoverBorder = Border3DStyle.Etched;
        private Border3DStyle _itemSelectionBorder = Border3DStyle.SunkenInner;
        private ImageList _defaultImageList = null;

        private int _moveStep = 16;
        private int _scrollButtonWidth = 21;
        private int _scrollButtonOffset = 2;
        private int _selectedTabIndex = -1;
        private bool _allowSelection = true;
        private bool _allowItemDrop = true;
        private int _scrollDelayInterval = 300;
        private int _scrollTickInterval = 50;
        private int _moveTickInterval = 10;
        private InertButton _scrollUpButton = new InertButton();
        private InertButton _scrollDownButton = new InertButton();
        private int _tabToOpen = -1;
        private ArrayList _tabs = new ArrayList();
        private bool _scrollUpButtonMouseDown = false;
        private bool _scrollDownButtonMouseDown = false;
        private bool _moveTabDown = false;
        private bool _moveTabUp = false;
        private ToolboxTab SelectedTab
        {
            get
            {
                if (SelectedTabIndex >= 0)
                {
                    return GetTab(SelectedTabIndex);
                }
                else
                {
                    return null;
                }
            }
        }

        private int TabWidth
        {
            get { return _panel.ClientRectangle.Width - 2 * _hMargin; }
        }

        private int VisibleListHeight
        {
            get { return _panel.ClientRectangle.Height - 2 * _vMargin - (_tabs.Count + 2) * _vMargin; }
        }

        private int ScrollButtonLeft
        {
            get { return _panel.ClientRectangle.Width - _hMargin - _scrollButtonWidth; }
        }

        private int SelectedTabHeight
        {
            get
            {
                int nt = (SelectedTabIndex < TabsCount - 1) ? TabsCount : TabsCount + 1;
                return _panel.ClientRectangle.Height - nt * (ToolboxTab.ButtonHeight + _vMargin);
            }
        }

        /// <summary>
        /// Style of border appearance.
        /// </summary>
        [CategoryAttribute("Appearance"), DefaultValue(BorderStyle.None)]
        public new BorderStyle BorderStyle {
            get {return _panel.BorderStyle;}
            set {_panel.BorderStyle = value;}
        }

        /// <summary>
        /// Allow selection of elements.
        /// </summary>
        [CategoryAttribute("Behavior"), DefaultValue(true)]
        [Description("Allow selection of elements.")]
        public bool AllowSelection
        {
            get { return _allowSelection; }
            set
            {
                _allowSelection = value;
                foreach (ToolboxTab tab in _tabs)
                {
                    tab.AllowSelection = value;
                }
            }
        }

        /// <summary>
        /// Allow dropping items onto the surface.
        /// </summary>
        [CategoryAttribute("Behavior"), DefaultValue(true)]
        [Description("Allow dropping items onto the surface.")]
        public bool AllowItemDrop
        {
            get
            {
                return _allowItemDrop;
            }
            set
            {
                _allowItemDrop = value;
            }
        }

        /// <summary>
        /// Show tooltips on mouse over.
        /// </summary>
        [CategoryAttribute("Behavior"), DefaultValue(true)]
        [Description("Show tooltips on mouse over.")]
        public bool ShowToolTip
        {
            get { return _toolTip.Active; }
            set { _toolTip.Active = value; }
        }

        /// <summary>
        /// Border style for item hover (mouse over item).
        /// </summary>
        [CategoryAttribute("Appearance"), DefaultValue(Border3DStyle.Etched)]
        [Description("Border style for item hover (mouse over item).")]
        public Border3DStyle ItemHoverBorder
        {
            get { return _itemHoverBorder; }
            set
            {
                _itemHoverBorder = value;
                foreach (ToolboxTab tab in _tabs)
                {
                    tab.ItemHoverBorder = value;
                }
                Invalidate(true);
            }
        }

        /// <summary>
        /// Border for selected items.
        /// </summary>
        [CategoryAttribute("Appearance"), DefaultValue(Border3DStyle.SunkenInner)]
        [Description("Border for selected items.")]
        public Border3DStyle ItemSelectionBorder
        {
            get { return _itemSelectionBorder; }
            set
            {
                _itemSelectionBorder = value;
                foreach (ToolboxTab tab in _tabs)
                {
                    tab.ItemSelectionBorder = value;
                }
                Invalidate(true);
            }
        }

        /// <summary>
        /// Color of selected items.
        /// </summary>
        [CategoryAttribute("Appearance"), DefaultValue(null)]
        [Description("Color of selected items.")]
        public Color SelectionColor
        {
            get { return _selectionBrush.Color; }
            set
            {
                _selectionBrush = new SolidBrush(value);
                foreach (ToolboxTab tab in _tabs)
                {
                    tab.SelectionBrush = _selectionBrush;
                }
                Invalidate(true);
            }
        }

        /// <summary>
        /// Color on mouse over.
        /// </summary>
        [CategoryAttribute("Appearance")]
        [Description("Color on mouse over.")]
        public Color ItemHooverColor
        {
            get { return _itemHooverBrush.Color; }
            set
            {
                _itemHooverBrush = new SolidBrush(value);
                foreach (ToolboxTab tab in _tabs)
                {
                    tab.ItemHooverBrush = _itemHooverBrush;
                }
                Invalidate(true);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Appearance")]
        [Description("Color of item buttons.")]
        public Color ButtonColor
        {
            get { return _buttonColor; }
            set
            {
                _buttonColor = value;
                _scrollDownButton.BackColor = value;
                _scrollUpButton.BackColor = value;
                foreach (ToolboxTab tab in _tabs)
                {
                    tab.ButtonColor = _buttonColor;
                }
                Invalidate(true);
            }
        }

        /// <summary>
        /// Back color of opened tabs.
        /// </summary>
        [CategoryAttribute("Appearance")]
        [Description("Back color of opened tabs.")]
        public Color OpenTabBackColor
        {
            get { return _openTabBackColor; }
            set
            {
                _openTabBackColor = value;
                foreach (ToolboxTab tab in _tabs)
                {
                    if (tab == SelectedTab)
                    {
                        tab.OpenTabBackColor = _openTabBackColor;
                    }
                    else
                    {
                        tab.OpenTabBackColor = _buttonColor;
                    }
                }
            }
        }

        /// <summary>
        /// Fore color of opened tabs.
        /// </summary>
        [CategoryAttribute("Appearance")]
        [Description("Fore color of opened tabs.")]
        public Color OpenTabForeColor
        {
            get { return _openTabForeColor; }
            set
            {
                _openTabForeColor = value;
                foreach (ToolboxTab tab in _tabs)
                {
                    if (tab == SelectedTab)
                    {
                        tab.OpenTabForeColor = _openTabForeColor;
                    }
                    else
                    {
                        tab.OpenTabForeColor = ForeColor;
                    }
                }
            }
        }

        /// <summary>
        /// Tick interval for movemenents.
        /// </summary>
        [CategoryAttribute("Behavior"), DefaultValue(10)]
        [Description("Tick interval for movemenents.")]
        public int MovementTickInterval
        {
            get { return _moveTickInterval; }
            set { _moveTickInterval = value; }
        }

        /// <summary>
        /// Images used to show beneath items.
        /// </summary>
        [CategoryAttribute("Appearance"), DefaultValue(null)]
        [Description("Images used to show beneath items.")]
        public ImageList ImageList
        {
            get { return _defaultImageList; }
            set { _defaultImageList = value; }
        }

        /// <summary>
        /// Number of tabs.
        /// </summary>
        [Browsable(false)]
        [Description("")]
        public int TabsCount
        {
            get { return _tabs.Count; }
        }

        /// <summary>
        /// Currently selected index of open tab.
        /// </summary>
        [Browsable(false)]
        [Description("Currently selected index of open tab.")]
        public int SelectedTabIndex
        {
            get { return _selectedTabIndex; }
            set
            {
                if (value >= -1 && value < _tabs.Count)
                {
                    _selectedTabIndex = value;
                    EndAllMovements();
                    LayoutTabs();
                }
            }
        }

        /// <summary>
        /// Currently selected caption of open tab.
        /// </summary>
        [Browsable(false)]
        public string SelectedTabCaption
        {
            get
            {
                int idx = SelectedTabIndex;
                if (idx >= 0)
                {
                    return GetTab(idx).Caption;
                }
                return "";
            }
        }

        /// <summary>
        /// Caption of currently selected item.
        /// </summary>
        [Browsable(false)]
        public string SelectedItemCaption
        {
            get
            {
                if (_allowSelection)
                {
                    ToolboxTab tab = SelectedTab;
                    if (tab != null)
                    {
                        return tab.SelectedItemCaption;
                    }
                }
                return "";
            }
        }

        /// <summary>
        /// Id of selected item.
        /// </summary>
        [Browsable(false)]
        public string SelectedItemId
        {
            get
            {
                if (_allowSelection)
                {
                    ToolboxTab tab = SelectedTab;
                    if (tab != null)
                    {
                        return tab.SelectedItemId;
                    }
                }
                return "";
            }
        }

        /// <summary>
        /// Tag of selected item.
        /// </summary>
        [Browsable(false)]
        public object SelectedItemTag
        {
            get
            {
                if (_allowSelection)
                {
                    ToolboxTab tab = SelectedTab;
                    if (tab != null)
                    {
                        return tab.SelectedItemTag;
                    }
                }
                return null;
            }
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public Toolbox()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();
            BackColorChanged += new EventHandler(OnChangeBackColor);
            ForeColorChanged += new EventHandler(OnChangeForeColor);
        }

        private ToolboxTab GetTab(int index)
        {
            return _tabs[index] as ToolboxTab;
        }

        private void LayoutTabs()
        {
            int w = TabWidth;
            int opentabheight = SelectedTabHeight - _vMargin;
            for (int i = 0; i < _tabs.Count; i++)
            {
                ToolboxTab tab = (ToolboxTab)_tabs[i];
                int offset = 0;
                int h = 0;
                if (i == SelectedTabIndex || i == SelectedTabIndex + 1)
                {
                    offset = _scrollButtonWidth + _scrollButtonOffset;
                }
                if (i == SelectedTabIndex)
                {
                    h = opentabheight;
                    tab.OpenTabBackColor = _openTabBackColor;
                    tab.OpenTabForeColor = _openTabForeColor;
                }
                else
                {
                    tab.OpenTabBackColor = _buttonColor;
                    tab.OpenTabForeColor = ForeColor;
                }
                if (i <= _selectedTabIndex)
                {
                    tab.Location(_hMargin, GetTabTop(i));
                }
                else
                {
                    tab.Location(_hMargin, GetTabTopFromBottom(i));
                }
                tab.Size(w, h, offset);
            }
            if (SelectedTabIndex >= 0)
            {
                _scrollUpButton.Visible = true;
                _scrollDownButton.Visible = true;
                _scrollUpButton.Left = ScrollButtonLeft;
                _scrollDownButton.Left = ScrollButtonLeft;
                _scrollUpButton.Top = SelectedTabIndex * (ToolboxTab.ButtonHeight + _vMargin) + _vMargin;
                _scrollDownButton.Top = _scrollUpButton.Top + SelectedTabHeight + ToolboxTab.ButtonHeight;
            }
            else
            {
                _scrollUpButton.Visible = false;
                _scrollDownButton.Visible = false;
            }
        }

        private void ScrollUpItems()
        {
            ToolboxTab tab = SelectedTab;
            if (tab != null && tab.ItemsCount > 0)
            {
                if (tab.ScrollUpItems())
                {
                    tab.ReDraw();
                }
                else
                {
                    EndAllMovements();
                }
            }
        }

        private void ScrollDownItems()
        {
            ToolboxTab tab = SelectedTab;
            if (tab != null && tab.ItemsCount > 0)
            {
                if (tab.ScrollDownItems())
                {
                    tab.ReDraw();
                }
                else
                {
                    EndAllMovements();
                }
            }
        }

        private int GetTabTop(int index)
        {
            return (ToolboxTab.ButtonHeight + _vMargin) * index + _vMargin;
        }

        private int GetTabTopFromBottom(int index)
        {
            return _panel.ClientRectangle.Height - (ToolboxTab.ButtonHeight + _vMargin) * (TabsCount - index);
        }

        private void MoveTabOneStepUp()
        {
            if (_tabToOpen >= 0)
            {
                ToolboxTab seltab = SelectedTab;
                ToolboxTab tab = GetTab(_tabToOpen);
                if (tab != null)
                {
                    seltab.Shrink(_moveStep);
                    for (int i = _selectedTabIndex + 1; i <= _tabToOpen; i++)
                    {
                        ToolboxTab t = GetTab(i);
                        t.MoveUp(_moveStep);
                    }
                    tab.Growth(_moveStep);
                    int endpoint = GetTabTop(_tabToOpen);
                    if (tab.Top <= endpoint)
                    {
                        EndAllMovements();
                        if (TabChanged != null)
                        {
                            ToolBoxEventArgs ea = new ToolBoxEventArgs(
                                SelectedTab.Caption, "", "",
                                SelectedTabIndex, -1, null);
                            TabChanged(this, ea);
                        }
                    }
                    //SelectedTab.ReDraw();
                    //tab.ReDraw();
                }
            }
        }

        private void MoveTabOneStepDown()
        {
            if (_tabToOpen >= 0)
            {
                ToolboxTab opentab = GetTab(_tabToOpen);
                ToolboxTab movetab = GetTab(_tabToOpen + 1);
                SelectedTab.Shrink(_moveStep);
                for (int i = _tabToOpen + 1; i <= _selectedTabIndex; i++)
                {
                    ToolboxTab t = GetTab(i);
                    t.MoveDown(_moveStep);
                }
                opentab.Growth(_moveStep);
                int endpoint = GetTabTopFromBottom(_tabToOpen + 1);
                if (movetab.Top >= endpoint)
                {
                    EndAllMovements();
                    if (TabChanged != null)
                    {
                        ToolBoxEventArgs ea = new ToolBoxEventArgs(
                            SelectedTab.Caption, "", "",
                            SelectedTabIndex, -1, null);
                        TabChanged(this, ea);
                    }
                    //                    opentab.ReDraw();
                    //                    SelectedTab.ReDraw();
                }
            }
        }

        private void EndAllMovements()
        {
            StopTimer();
            _scrollUpButton.Visible = true;
            _scrollDownButton.Visible = true;
            _scrollUpButtonMouseDown = false;
            _scrollDownButtonMouseDown = false;
            _moveTabDown = false;
            _moveTabUp = false;
            if (_tabToOpen >= 0 && _tabToOpen != _selectedTabIndex)
            {
                ((ToolboxTab)_tabs[_tabToOpen]).SuspendMouseEvents = false;
                _selectedTabIndex = _tabToOpen;
                LayoutTabs();
            }
            _tabToOpen = -1;
        }

        private void StartTimer(int interval)
        {
            _timer.Interval = interval;
            _timer.Start();
        }

        private void StopTimer()
        {
            _timer.Start();
        }

        private void OnLoadControl(object sender, System.EventArgs e)
        {
            Size sz = new Size(_scrollButtonWidth, ToolboxTab.ButtonHeight);
            _scrollUpButton.Size = sz;
            _scrollUpButton.FlatStyle = FlatStyle.Popup;
            _scrollUpButton.MouseUp += new MouseEventHandler(this.OnSBUpMouseUp);
            _scrollUpButton.MouseDown += new MouseEventHandler(this.OnSBUpMouseDown);
            _scrollUpButton.Paint += new PaintEventHandler(OnPaintScrollButton);
            _scrollUpButton.Visible = false;
            _panel.Controls.Add(_scrollUpButton);

            _scrollDownButton.Size = sz;
            _scrollDownButton.FlatStyle = FlatStyle.Popup;
            _scrollDownButton.MouseUp += new MouseEventHandler(this.OnSBDownMouseUp);
            _scrollDownButton.MouseDown += new MouseEventHandler(this.OnSBDownMouseDown);
            _scrollDownButton.Paint += new PaintEventHandler(OnPaintScrollButton);
            _scrollDownButton.Visible = false;
            _panel.Controls.Add(_scrollDownButton);

            if (_backgroundBrush.Color != BackColor)
            {
                _backgroundBrush = new SolidBrush(BackColor);
            }
            if (_textBrush.Color != ForeColor)
            {
                _textBrush = new SolidBrush(ForeColor);
            }
            Resize += new EventHandler(OnResize);
            if (DesignMode)
            {
                AddTab("Tab 1", null, null);
                AddItem("Item 11", "Item 1", "Item 11 Description", 0, null, null, 0);
                AddItem("Item 12", "Item 2", "Item 12 Description", 1, null, null, 0);
                AddItem("Item 13", "Item 2", "", 2, null, null, 0);
                AddItem("Item 14", "Item 2", "", 3, null, null, 0);
                AddItem("Item 15", "Item 2", "Item 15 Description", 4, null, null, 0);
                AddItem("Item 16", "Item 2", "", 5, null, null, 0);
                AddTab("Tab 2", null, null);
                AddItem("Item 21", "Item 1", "", 0, null, null, 1);
                AddItem("Item 22", "Item 2", "", 1, null, null, 1);
                AddItem("Item 23", "Item 2", "", 2, null, null, 1);
                AddItem("Item 24", "Item 2", "", 3, null, null, 1);
                AddItem("Item 25", "Item 2", "", 4, null, null, 1);
                AddItem("Item 26", "Item 2", "", 5, null, null, 1);
                SelectedTabIndex = 0;
            }
        }

        private void OnResize(object sender, EventArgs e)
        {
            LayoutTabs();

        }

        private void OnPaintScrollButton(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Point[] points = new Point[3];
            Control ctl = sender as Control;
            int cx = ctl.ClientRectangle.Width / 2;
            int cy = ctl.ClientRectangle.Height / 2;
            if (sender == _scrollUpButton)
            {
                cy -= 4;
                points[0] = new Point(cx, cy);
                points[1] = new Point(cx + 6, cy + 7);
                points[2] = new Point(cx - 6, cy + 7);
            }
            else
            {
                cy -= 1;
                points[0] = new Point(cx, cy + 6);
                points[1] = new Point(cx + 6, cy);
                points[2] = new Point(cx - 5, cy);
            }
            g.FillPolygon(_textBrush, points);
        }

        private void OnSelectTab(object sender, EventArgs e)
        {
            EndAllMovements();
            ToolboxTab tab = sender as ToolboxTab;
            tab.LayoutItems();
            _tabToOpen = _tabs.IndexOf(tab);
            tab.SuspendMouseEvents = true;
            if (_moveTickInterval <= 0)
            {
                EndAllMovements();
            }
            else
            {
                if (_tabToOpen < _selectedTabIndex)
                {
                    _scrollUpButton.Visible = false;
                    _scrollDownButton.Visible = false;
                    _moveTabDown = true;
                    StartTimer(_moveTickInterval);
                }
                else if (_tabToOpen > _selectedTabIndex)
                {
                    _scrollUpButton.Visible = false;
                    _scrollDownButton.Visible = false;
                    _moveTabUp = true;
                    StartTimer(_moveTickInterval);
                }
            }
        }

        private void OnSBUpMouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            _scrollUpButtonMouseDown = true;
            ScrollUpItems();
            StartTimer(_scrollDelayInterval);
        }

        private void OnSBUpMouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            EndAllMovements();
        }

        private void OnSBDownMouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            _scrollDownButtonMouseDown = true;
            ScrollDownItems();
            StartTimer(_scrollDelayInterval);
        }

        private void OnSBDownMouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            EndAllMovements();
        }

        private void OnItemActivate(object sender, EventArgs e)
        {
            ToolboxItem item = sender as ToolboxItem;
            if (item != null && ItemActivate != null)
            {
                ToolBoxEventArgs ea = new ToolBoxEventArgs(
                    SelectedTab.Caption, item.Caption, item.Id,
                    SelectedTabIndex, item.Index, item.Tag);
                ItemActivate(this, ea);
            }
        }

        private void OnItemDrag(object sender, EventArgs e)
        {
            if (_allowItemDrop)
            {
                ToolboxItem item = sender as ToolboxItem;
                if (item != null)
                {
                    ToolBoxEventArgs ea = new ToolBoxEventArgs(
                        SelectedTab.Caption, item.Caption, item.Id,
                        SelectedTabIndex, item.Index, item.Tag);
                    DoDragDrop(ea, DragDropEffects.Copy);
                }
            }
        }

        private void OnSelectItem(object sender, EventArgs e)
        {
            ToolboxItem item = sender as ToolboxItem;
            if (item != null && ItemSelect != null)
            {
                ToolBoxEventArgs ea = new ToolBoxEventArgs(
                    SelectedTab.Caption, item.Caption, item.Id,
                    SelectedTabIndex, item.Index, item.Tag);
                ItemSelect(this, ea);
            }
        }

        private void OnChangeBackColor(object sender, EventArgs e)
        {
            _panel.BackColor = BackColor;
            _backgroundBrush = new SolidBrush(BackColor);
            foreach (ToolboxTab tab in _tabs)
            {
                tab.BackgroundBrush = _backgroundBrush;
            }
            Invalidate(true);
        }

        private void OnChangeForeColor(object sender, EventArgs e)
        {
            _panel.ForeColor = ForeColor;
            _textBrush = new SolidBrush(ForeColor);
            foreach (ToolboxTab tab in _tabs)
            {
                tab.TextBrush = _textBrush;
            }
            Invalidate(true);
        }

        private void OnTimerTick(object sender, System.EventArgs e)
        {
            if (_scrollUpButtonMouseDown)
            {
                _timer.Interval = _scrollTickInterval;
                ScrollUpItems();
            }
            else if (_scrollDownButtonMouseDown)
            {
                _timer.Interval = _scrollTickInterval;
                ScrollDownItems();
            }
            else if (_moveTabUp)
            {
                MoveTabOneStepUp();
            }
            else if (_moveTabDown)
            {
                MoveTabOneStepDown();
            }
        }

        private void InitializeTab(ToolboxTab tab, ContextMenu menu)
        {
            tab.ItemHoverBorder = _itemHoverBorder;
            tab.ItemHooverBrush = _itemHooverBrush;
            tab.AllowSelection = _allowSelection;
            tab.TabSelected += new EventHandler(OnSelectTab);
            tab.ItemActivate += new EventHandler(OnItemActivate);
            tab.ItemSelected += new EventHandler(OnSelectItem);
            tab.ItemDrag += new EventHandler(OnItemDrag);
            tab.ButtonColor = _buttonColor;
            tab.BackgroundBrush = _backgroundBrush;
            tab.SelectionBrush = _selectionBrush;
            tab.ItemSelectionBorder = _itemSelectionBorder;
            tab.TextBrush = _textBrush;
            tab.ToolTip = _toolTip;
            tab.ContextMenu = menu;
            SelectedTabIndex = _tabs.Add(tab);
            LayoutTabs();
        }


        /// <summary>
        /// Remove all the tabs and items from the Toolbox
        /// </summary>
        public void Clear()
        {
            while (_tabs.Count > 0)
            {
                RemoveTab(0);
            }
            _scrollUpButton.Visible = false;
            _scrollDownButton.Visible = false;
            SelectedTabIndex = -1;
        }

        /// <summary>
        /// Add a new tab to the toolbox. The new tab will reference the specified ImageList.
        /// If this is null no ImageList will be used.
        /// </summary>
        /// <param name="caption">the caption of the new tab</param>
        /// <param name="imagelist">the ImageList to use for this tab</param>
        /// <param name="menu">the default context menu to use inside this tab</param>
        /// <returns>the index of the new tab inside the Toolbox</returns>
        public int AddTab(string caption, ImageList imagelist, ContextMenu menu)
        {
            ToolboxTab tab = new ToolboxTab(caption, imagelist, _panel);
            InitializeTab(tab, menu);
            return SelectedTabIndex;
        }

        /// <summary>
        /// Add a new tab to the toolbox. The new tab will reference the default ImageList
        /// </summary>
        /// <param name="caption">the caption of the new tab</param>
        /// <param name="menu">the default context menu to use inside this tab</param>
        /// <returns>the index of the new tab inside the Toolbox</returns>
        public int AddTab(string caption, ContextMenu menu)
        {
            ToolboxTab tab = new ToolboxTab(caption, _defaultImageList, _panel);
            InitializeTab(tab, menu);
            return SelectedTabIndex;
        }

        /// <summary>
        /// Add a new item to the Toolbox
        /// </summary>
        /// <param name="caption">the caption of the new item</param>
        /// <param name="id">the id to be used to identify the item</param>
        /// <param name="descr">the description to use in the tooltip</param>
        /// <param name="imageIndex">the index inside the tab's ImageList to use</param>
        /// <param name="tag">an optional object that has to be associated to this item</param>
        /// <param name="menu">the menu to associate to this item. It overrides the tab context menu</param>
        /// <param name="tabIndex">the index of the tab the item has to be added to</param>
        /// <returns></returns>
        public int AddItem(string caption, string id, string descr,
            int imageIndex, object tag, ContextMenu menu, int tabIndex)
        {
            ToolboxTab tab = GetTab(tabIndex);
            return tab.AddItem(caption, id, descr, imageIndex, tag, menu);
        }

        /// <summary>
        /// Remove a Tab
        /// </summary>
        /// <param name="index">the index of the tab to remove</param>
        public void RemoveTab(int index)
        {
            EndAllMovements();
            if (index >= 0 && index < TabsCount)
            {
                ToolboxTab tab = GetTab(index);
                tab.Remove();
                _tabs.Remove(tab);
                if (SelectedTabIndex >= TabsCount)
                {
                    SelectedTabIndex = TabsCount - 1;
                }
                else
                {
                    LayoutTabs();
                }
            }
        }

        /// <summary>
        /// Remove an item
        /// </summary>
        /// <param name="tabindex">the index of the tab the item to remove belongs to</param>
        /// <param name="itemindex">the index of the item to remove</param>
        public void RemoveItem(int tabindex, int itemindex)
        {
            if (tabindex >= 0 && tabindex < TabsCount)
            {
                ToolboxTab tab = GetTab(tabindex);
                tab.RemoveItem(itemindex);
            }
        }

        /// <summary>
        /// Returns the index of the tab with a specific caption
        /// </summary>
        /// <param name="caption">the caption of the tab to return</param>
        /// <returns>the index of the tab, -1 if the tab is not found</returns>
        public int GetTabIndex(string caption)
        {
            int index = -1;
            for (int i = 0; i < _tabs.Count; i++)
            {
                ToolboxTab tab = GetTab(i);
                if (tab != null && tab.Caption == caption)
                {
                    index = i;
                    break;
                }
            }
            return index;
        }

        /// <summary>
        /// Removes all the items from the tab with a specified index
        /// </summary>
        /// <param name="index">The index of the tab to clear</param>
        public void ClearTab(int index)
        {
            if (index >= 0 && index < _tabs.Count)
            {
                ToolboxTab tab = GetTab(index);
                if (tab != null)
                {
                    tab.Clear();
                }
            }
        }

        /// <summary>
        /// Save the Toolbox tabs and items layout in an Xml Document starting from the node passed.
        /// </summary>
        /// <param name="xml">the XmlElement to use to store the data</param>
        public void SaveTemplate(XmlElement xml)
        {
            foreach (ToolboxTab tab in _tabs)
            {
                XmlElement xmltab = AddXmlNode(XmlTabName, xml);
                SetXmlAttribute("caption", tab.Caption, xmltab);
                foreach (ToolboxItem item in tab.Items)
                {
                    XmlElement xmlitem = AddXmlNode(XmlItemName, xmltab);
                    SetXmlAttribute("caption", item.Caption, xmlitem);
                    SetXmlAttribute("class", item.Id, xmlitem);
                    SetXmlAttribute("description", item.Description, xmlitem);
                    SetXmlAttribute("image", item.ImageIndex.ToString(), xmlitem);
                }
            }
        }

        /// <summary>
        /// Replace the current Toolbox tabs and items layout with the one 
        /// stored in an Xml Document starting from the node passed.
        /// </summary>
        /// <param name="parentxml">the XmlElement to use to load the data</param>
        public void LoadTemplate(XmlElement parentxml)
        {
            Clear();
            AppendTemplate(parentxml);
            if (_tabs.Count > 0)
            {
                SelectedTabIndex = 0;
            }
        }

        /// <summary>
        /// Replace the current Toolbox tabs and items layout with the one 
        /// stored in an Xml Document starting from the node passed.
        /// </summary>
        /// <param name="parentxml">the XmlElement to use to load the data</param>
        /// <param name="imagelist">the ImageList to use for the new added tabs</param>
        public void LoadTemplate(XmlElement parentxml, ImageList imagelist)
        {
            Clear();
            AppendTemplate(parentxml, imagelist);
            if (_tabs.Count > 0)
            {
                SelectedTabIndex = 0;
            }
        }

        /// <summary>
        /// Append to the current Toolbox tabs and items layout the one 
        /// stored in an Xml Document starting from the node passed.
        /// </summary>
        /// <param name="parentxml">the XmlElement to use to load the data</param>
        public void AppendTemplate(XmlElement parentxml)
        {
            AppendTemplate(parentxml, _defaultImageList);
        }

        /// <summary>
        /// Append to the current Toolbox tabs and items layout the one 
        /// stored in an Xml Document starting from the node passed.
        /// </summary>
        /// <param name="parentxml">the XmlElement to use to load the data</param>
        /// <param name="imagelist">the ImageList to use for the new added tabs</param>
        public void AppendTemplate(XmlElement parentxml, ImageList imagelist)
        {
            XmlNodeList tablist = parentxml.SelectNodes(XmlTabName);
            foreach (XmlElement xmltab in tablist)
            {
                string tabcaption = GetXmlAttribute("caption", xmltab);
                if (tabcaption != "")
                {
                    int tabindex = AddTab(tabcaption, imagelist, null);
                    XmlNodeList itemlist = xmltab.SelectNodes(XmlItemName);
                    foreach (XmlElement xmlitem in itemlist)
                    {
                        string caption = GetXmlAttribute("caption", xmlitem);
                        string id = GetXmlAttribute("class", xmlitem);
                        string descr = GetXmlAttribute("description", xmlitem);
                        int index = -1;
                        try
                        {
                            index = Int32.Parse(GetXmlAttribute("image", xmlitem));
                        }
                        catch { }
                        AddItem(caption, id, descr, index, null, null, tabindex);
                    }
                }
            }
        }


        #region Static Misc Methods

        /// <summary>
        /// Add Node as XML.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="root"></param>
        /// <returns></returns>
        public static XmlElement AddXmlNode(string name, XmlNode root)
        {
            XmlDocument doc = root.OwnerDocument;
            if (doc == null)
            {
                doc = (XmlDocument)root;
            }
            return (XmlElement)root.AppendChild(doc.CreateElement(name));
        }

        /// <summary>
        /// Set XML Attribute.
        /// </summary>
        /// <param name="attr"></param>
        /// <param name="val"></param>
        /// <param name="root"></param>
        public static void SetXmlAttribute(string attr, string val, XmlNode root)
        {
            if (root.Attributes[attr] == null)
            {
                root.Attributes.Append(root.OwnerDocument.CreateAttribute(attr));
            }
            root.Attributes[attr].Value = val;
        }

        /// <summary>
        /// Get XML Attribute.
        /// </summary>
        /// <param name="attr"></param>
        /// <param name="root"></param>
        /// <returns></returns>
        public static string GetXmlAttribute(string attr, XmlNode root)
        {
            if (root != null && root.Attributes[attr] != null)
            {
                return root.Attributes[attr].Value;
            }
            return "";
        }
        #endregion

        /// <summary>
        /// Add toolboxservice if integrated in a design time environment.
        /// </summary>
        public override System.ComponentModel.ISite Site
        {
            get
            {
                return base.Site;
            }
            set
            {     
                base.Site = value;

                // If the component was sited, attempt to obtain 
                // an IToolboxService instance.
                if( base.Site != null )
                {
                    toolboxService = (IToolboxService)this.GetService(typeof(IToolboxService));
                    // If an IToolboxService was located, update the 
                    // category list.
                    if (toolboxService != null)
                    {
                        LayoutTabs();
                    }
                }
                else
                    toolboxService = null;
            }
        }

    }


}