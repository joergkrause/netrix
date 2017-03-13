using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Collections;

namespace GuruComponents.Netrix.UserInterface.ToolBox
{
    class ToolboxTab
    {
        public const int ButtonHeight = 21;
        private Border3DStyle _itemHoverBorder = Border3DStyle.Raised;
        private Border3DStyle _itemSelectionBorder = Border3DStyle.SunkenInner;
        private bool _allowSelection = false;
        private SolidBrush _backgroundBrush = null;
        private SolidBrush _selectionBrush = null;
        private SolidBrush _itemHooverBrush = null;
        private SolidBrush _textBrush = null;
        private int _vOffset = 2;
        private int _hOffset = 0;
        private ArrayList _items = new ArrayList();
        private ItemPanel _panel = new ItemPanel();
        private InertButton _button = new InertButton();
        private ImageList _imageList = null;
        private String _caption = "";
        public ToolTip ToolTip = null;
        private Control _parent = null;
        public bool SuspendMouseEvents = false;
        public ContextMenu ContextMenu
        {
            get { return _panel.ContextMenu; }
            set { _panel.ContextMenu = value; }
        }

        public int ItemsCount
        {
            get { return _items.Count; }
        }

        public ToolboxItem[] Items
        {
            get { return (ToolboxItem[])_items.ToArray(typeof(ToolboxItem)); }
        }

        public string Caption
        {
            get { return _caption; }
        }

        public int Top
        {
            get { return _button.Top; }
        }

        public Border3DStyle ItemHoverBorder
        {
            set
            {
                _itemHoverBorder = value;
                foreach (ToolboxItem i in _items)
                {
                    i.ItemHoverBorder = value;
                }
            }
        }

        public Border3DStyle ItemSelectionBorder
        {
            set
            {
                _itemSelectionBorder = value;
                foreach (ToolboxItem i in _items)
                {
                    i.ItemSelectionBorder = value;
                }
            }
        }

        public Color ButtonColor
        {
            get { return _button.BackColor; }
            set { _button.BackColor = value; }
        }

        public Color OpenTabBackColor
        {
            set { _button.BackColor = value; }
        }

        public Color OpenTabForeColor
        {
            set { _button.ForeColor = value; }
        }

        public SolidBrush BackgroundBrush
        {
            set
            {
                _backgroundBrush = value;
                _panel.BackColor = value.Color;
                foreach (ToolboxItem item in _items)
                {
                    item.BackgroundBrush = value;
                }
            }
        }

        public SolidBrush SelectionBrush
        {
            set
            {
                _selectionBrush = value;
                foreach (ToolboxItem item in _items)
                {
                    item.SelectionBrush = value;
                }
            }
        }

        public SolidBrush ItemHooverBrush
        {
            set
            {
                _itemHooverBrush = value;
                foreach (ToolboxItem item in _items)
                {
                    item.ItemHooverBrush = value;
                }
            }
        }

        public SolidBrush TextBrush
        {
            set
            {
                _textBrush = value;
                _button.ForeColor = _textBrush.Color;
                foreach (ToolboxItem item in _items)
                {
                    item.TextBrush = value;
                }
            }
        }

        public bool AllowSelection
        {
            get { return _allowSelection; }
            set
            {
                SelectItem(null);
                _allowSelection = value;
            }
        }

        public string SelectedItemCaption
        {
            get
            {
                foreach (ToolboxItem item in _items)
                {
                    if (item.Selected)
                    {
                        return item.Caption;
                    }
                }
                return "";
            }
        }

        public string SelectedItemId
        {
            get
            {
                foreach (ToolboxItem item in _items)
                {
                    if (item.Selected)
                    {
                        return item.Id;
                    }
                }
                return "";
            }
        }

        public object SelectedItemTag
        {
            get
            {
                foreach (ToolboxItem item in _items)
                {
                    if (item.Selected)
                    {
                        return item.Tag;
                    }
                }
                return null;
            }
        }

        public ImageList ImageList
        {
            get { return _imageList; }
            set
            {
                _imageList = value;
                foreach (ToolboxItem item in _items)
                {
                    if (value != null && item.ImageIndex >= 0 && item.ImageIndex < _imageList.Images.Count)
                    {
                        item.Image = _imageList.Images[item.ImageIndex];
                    }
                    else
                    {
                        item.Image = null;
                    }
                }
            }
        }


        public event EventHandler TabSelected;
        public event EventHandler ItemSelected;
        public event EventHandler ItemActivate;
        public event EventHandler ItemDrag;

        public ToolboxTab(string caption, ImageList images, Control parent)
        {
            _parent = parent;
            _caption = caption;
            _imageList = images;
            _button.Height = ButtonHeight;
            _button.FlatStyle = FlatStyle.Popup;
            _button.TextAlign = ContentAlignment.MiddleLeft;
            _button.Text = _caption;
            _button.TabIndex = 0;
            _button.Tag = this;
            _button.Click += new EventHandler(OnClick);
            _panel.BackColor = SystemColors.Control;
            _panel.BorderStyle = System.Windows.Forms.BorderStyle.None;
            _panel.AutoScroll = false;
            _panel.MouseLeave += new EventHandler(OnMouseLeave);
            _parent.Controls.Add(_button);
            _parent.Controls.Add(_panel);
        }

        public int AddItem(string caption, string id, string descr, int imageIndex, object tag, ContextMenu menu)
        {
            ToolboxItem item = new ToolboxItem(caption, id, descr, imageIndex, _imageList, tag, _panel);
            int top = _vOffset;
            if (_items.Count > 0)
            {
                top += Items[_items.Count - 1].Bottom;
            }
            item.ItemHooverBrush = _itemHooverBrush;
            item.ItemHoverBorder = _itemHoverBorder;
            item.Left = _hOffset;
            item.Top = top;
            item.Width = _panel.ClientRectangle.Width - 2 * _hOffset;
            item.ItemDrag += new EventHandler(OnItemDrag);
            item.ItemClick += new EventHandler(OnItemClick);
            item.ItemDoubleClick += new EventHandler(OnItemDoubleClick);
            item.ItemHighlighted += new EventHandler(OnItemHighlighted);
            item.ItemUnHighlighted += new EventHandler(OnItemUnHighlighted);
            item.BackgroundBrush = _backgroundBrush;
            item.SelectionBrush = _selectionBrush;
            item.ItemSelectionBorder = _itemSelectionBorder;
            item.TextBrush = _textBrush;
            item.ContextMenu = menu;
            item.SetToolTip(ToolTip);
            int idx = _items.Add(item);
            item.Index = idx;
            return idx;
        }

        public void Location(int left, int top)
        {
            _button.Left = left;
            _button.Top = top;
            _panel.Left = left;
            _panel.Top = top + _button.Height;
        }

        public void Size(int width, int height, int buttRightOffset)
        {
            _button.Width = width - buttRightOffset;
            _panel.Width = width;
            _panel.Height = height;
            foreach (ToolboxItem item in _items)
            {
                item.Width = _panel.ClientRectangle.Width;
            }
        }

        public void Shrink(int delta)
        {
            _panel.Height -= delta;
        }

        public void Growth(int delta)
        {
            _panel.Height += delta;
        }

        public void MoveUp(int delta)
        {
            _button.Top -= delta;
            _panel.Top -= delta;
        }

        public void MoveDown(int delta)
        {
            _button.Top += delta;
            _panel.Top += delta;
        }

        public bool ScrollUpItems()
        {
            if (GetFirstVisibleItem() > 0)
            {
                foreach (ToolboxItem item in _items)
                {
                    item.ScrollUp(_vOffset);
                }
                _panel.Invalidate();
                return true;
            }
            return false;
        }

        public bool ScrollDownItems()
        {
            if (GetLastVisibleItem() < _items.Count - 1)
            {
                foreach (ToolboxItem item in _items)
                {
                    item.ScrollDown(_vOffset);
                }
                _panel.Invalidate();
                return true;
            }
            return false;
        }

        public void LayoutItems()
        {
            ToolboxItem[] list = Items;
            int top = _vOffset;
            for (int i = 0; i < list.Length; i++)
            {
                list[i].Top = top;
                top += list[i].Height + _vOffset;
            }
        }

        public void Remove()
        {
            _parent.Controls.Remove(_button);
            _parent.Controls.Remove(_panel);
        }

        public void RemoveItem(int index)
        {
            if (index >= 0 && index < _items.Count)
            {
                ToolboxItem item = Items[index];
                item.Remove(ToolTip);
                _items.Remove(item);
                RenumberItems();
                LayoutItems();
            }
        }

        public void ReDraw()
        {
            _panel.Invalidate();
        }

        public void Clear()
        {
            _panel.Visible = false;
            while (_items.Count > 0)
            {
                RemoveItem(0);
            }
            _panel.Visible = true;
        }

        private void RenumberItems()
        {
            for (int i = 0; i < _items.Count; i++)
            {
                ToolboxItem item = _items[i] as ToolboxItem;
                item.Index = i;
            }
        }

        private ToolboxItem GetItemAt(int y)
        {
            int index = -1;
            for (int i = 0; i < _items.Count; i++)
            {
                ToolboxItem item = _items[i] as ToolboxItem;
                if (item.Top < y && item.Bottom > y)
                {
                    index = i;
                }
            }
            if (index >= 0)
            {
                return _items[index] as ToolboxItem;
            }
            else
            {
                return null;
            }
        }

        private int GetFirstVisibleItem()
        {
            int ret = -1;
            ToolboxItem[] list = Items;
            Rectangle rect = _panel.ClientRectangle;
            for (int i = 0; i < list.Length; i++)
            {
                if (rect.Contains(list[i].Bounds))
                {
                    ret = i;
                    break;
                }
            }
            return ret;
        }

        private int GetLastVisibleItem()
        {
            int ret = -1;
            ToolboxItem[] list = Items;
            Rectangle rect = _panel.ClientRectangle;
            for (int i = list.Length - 1; i >= 0; i--)
            {
                if (rect.Contains(list[i].Bounds))
                {
                    ret = i;
                    break;
                }
            }
            return ret;
        }

        private void HighLightItem(ToolboxItem item)
        {
            foreach (ToolboxItem i in _items)
            {
                if (item == i)
                {
                    i.Highlighted = true;
                    _panel.Invalidate(i.Bounds);
                }
                else if (i.Highlighted)
                {
                    i.Highlighted = false;
                    _panel.Invalidate(i.Bounds);
                }
            }
        }

        private void SelectItem(ToolboxItem item)
        {
            if (_allowSelection)
            {
                foreach (ToolboxItem i in _items)
                {
                    if (item == i)
                    {
                        i.Selected = true;
                        _panel.Invalidate(i.Bounds);
                    }
                    else if (i.Selected)
                    {
                        i.Selected = false;
                        _panel.Invalidate(i.Bounds);
                    }
                }
                if (item != null && ItemSelected != null)
                {
                    ItemSelected(item, EventArgs.Empty);
                }
            }
        }

        private void OnClick(object sender, EventArgs e)
        {
            if (TabSelected != null)
            {
                TabSelected(this, EventArgs.Empty);
            }
        }

        private void OnItemClick(object sender, EventArgs e)
        {
            ToolboxItem item = sender as ToolboxItem;
            if (item != null)
            {
                SelectItem(item);
            }
        }

        private void OnItemDoubleClick(object sender, EventArgs e)
        {
            if (ItemActivate != null)
            {
                ToolboxItem item = sender as ToolboxItem;
                ItemActivate(item, EventArgs.Empty);
            }
        }

        private void OnItemDrag(object sender, EventArgs e)
        {
            ToolboxItem item = sender as ToolboxItem;
            if (item != null && ItemDrag != null)
            {
                SelectItem(item);
                ItemDrag(sender, EventArgs.Empty);
            }
        }

        private void OnItemHighlighted(object sender, EventArgs e)
        {
            if (!SuspendMouseEvents)
            {
                ToolboxItem item = sender as ToolboxItem;
                if (item != null)
                {
                    HighLightItem(item);
                }
            }
        }

        private void OnItemUnHighlighted(object sender, EventArgs e)
        {
            if (!SuspendMouseEvents)
            {
                HighLightItem(null);
            }
        }

        private void OnMouseLeave(object sender, EventArgs e)
        {
            HighLightItem(null);
        }
    }


}