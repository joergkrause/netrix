using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace GuruComponents.Netrix.UserInterface.ToolBox
{
        class ToolboxItem
        {
            private const int _deltaDrag = 5;
            private int _itemHeight = 22;
            private static ContextMenu _menu = new ContextMenu();
            private static StringFormat _stringFormat = new StringFormat(StringFormatFlags.NoWrap);
            private static Font _font = new Font("Microsoft Sans Serif", 8, FontStyle.Regular);
            private Border3DStyle _itemHoverBorder = Border3DStyle.Raised;
            private Border3DStyle _itemSelectionBorder = Border3DStyle.SunkenInner;
            private SolidBrush _selectionBrush = null;
            private SolidBrush _itemHooverBrush = null;
            private SolidBrush _textBrush = null;
            private Point _startDragPos;
            private bool _startDrag = false;
            private int _imageIndex = -1;
            private Image _image = null;
            private String _caption = "";
            private String _id = "";
            private bool _selected = false;
            private bool _highlighted = false;
            private ItemPanel _panel = new ItemPanel();
            private Panel _parent = null;
            private bool _menuOpen = false;
            public string Description = "";
            public int Index = -1;
            public ContextMenu ContextMenu
            {
                get { return _panel.ContextMenu; }
                set { _panel.ContextMenu = value; }
            }

            public object Tag = null;
            public Border3DStyle ItemHoverBorder
            {
                set { _itemHoverBorder = value; }
            }

            public Border3DStyle ItemSelectionBorder
            {
                set { _itemSelectionBorder = value; }
            }

            public Rectangle ClientRectangle
            {
                get { return _panel.ClientRectangle; }
            }

            public Rectangle Bounds
            {
                get { return _panel.Bounds; }
            }

            public bool Selected
            {
                get { return _selected; }
                set
                {
                    _selected = value;
                    _panel.Invalidate();
                }
            }

            public bool Highlighted
            {
                get { return _highlighted; }
                set
                {
                    _highlighted = value;
                    _panel.Invalidate();
                }
            }

            public String Caption
            {
                get { return _caption; }
            }

            public String Id
            {
                get { return _id; }
            }

            public int ImageIndex
            {
                get { return _imageIndex; }
            }

            public int Top
            {
                get { return _panel.Top; }
                set { _panel.Top = value; }
            }

            public int Left
            {
                set { _panel.Left = value; }
            }

            public int Width
            {
                set { _panel.Width = value; _panel.Invalidate(); }
            }

            public int Height
            {
                get { return _panel.Height; }
            }

            public int Bottom
            {
                get { return _panel.Bottom; }
            }

            public SolidBrush BackgroundBrush
            {
                set { _panel.BackColor = value.Color; }
            }

            public SolidBrush SelectionBrush
            {
                set { _selectionBrush = value; }
            }

            public SolidBrush ItemHooverBrush
            {
                set { _itemHooverBrush = value; }
            }

            public SolidBrush TextBrush
            {
                set { _textBrush = value; }
            }

            public Image Image
            {
                set { _image = value; }
            }

            public event EventHandler ItemUnHighlighted;
            public event EventHandler ItemHighlighted;
            public event EventHandler ItemClick;
            public event EventHandler ItemDoubleClick;
            public event EventHandler ItemDrag;

            static ToolboxItem()
            {
                _stringFormat.Alignment = StringAlignment.Near;
                _stringFormat.LineAlignment = StringAlignment.Center;
                _stringFormat.Trimming = StringTrimming.EllipsisCharacter;
                _menu.MenuItems.Add("Delete");
            }

            public ToolboxItem(string caption, string id, string description,
                int imageIndex, ImageList imageList, object tag, Panel parent)
            {
                _imageIndex = imageIndex;
                _caption = caption;
                _id = id;
                Tag = tag;
                Description = description;
                _parent = parent;
                if (imageList != null && _imageIndex >= 0 && _imageIndex < imageList.Images.Count)
                {
                    _image = imageList.Images[_imageIndex];
                }
                _panel.Left = 0;
                _panel.Height = _itemHeight;
                _panel.Width = _parent.Width;
                _panel.MouseMove += new MouseEventHandler(OnMouseMove);
                _panel.MouseUp += new MouseEventHandler(OnMouseUp);
                _panel.MouseDown += new MouseEventHandler(OnMouseDown);
                _panel.MouseEnter += new EventHandler(OnMouseEnter);
                _panel.MouseLeave += new EventHandler(OnMouseLeave);
                _panel.Click += new EventHandler(OnMouseClick);
                _panel.DoubleClick += new EventHandler(OnMouseDoubleClick);
                _panel.Paint += new PaintEventHandler(OnPaint);
                _parent.Controls.Add(_panel);
            }

            /// <summary>
            /// Removes an item.
            /// </summary>
            /// <param name="tip"></param>
            public void Remove(ToolTip tip)
            {
                _parent.Controls.Remove(_panel);
                tip.SetToolTip(_panel, "");
            }

            public void ScrollUp(int offset)
            {
                _panel.Top += _panel.Height + offset;
            }

            public void ScrollDown(int offset)
            {
                _panel.Top -= _panel.Height + offset;
            }

            public void SetToolTip(ToolTip tip)
            {
                if (Description != "")
                {
                    tip.SetToolTip(_panel, Caption + "\n" + Description);
                }
                else
                {
                    tip.SetToolTip(_panel, Caption);
                }
            }

            private void OnMouseEnter(object sender, EventArgs e)
            {
                if (ItemHighlighted != null)
                {
                    ItemHighlighted(this, EventArgs.Empty);
                }
            }

            private void OnMouseLeave(object sender, EventArgs e)
            {
                if (!_menuOpen && ItemUnHighlighted != null)
                {
                    ItemUnHighlighted(this, EventArgs.Empty);
                }
            }

            private void OnMouseMove(object sender, MouseEventArgs e)
            {
                if (_startDrag)
                {
                    Point p = Control.MousePosition;
                    if (Math.Abs(p.X - _startDragPos.X) > _deltaDrag || Math.Abs(p.X - _startDragPos.X) > _deltaDrag)
                    {
                        _startDrag = false;
                        if (ItemDrag != null)
                        {
                            ItemDrag(this, EventArgs.Empty);
                        }
                    }
                }
            }

            private void OnMouseDown(object sender, MouseEventArgs e)
            {
                _startDrag = true;
                _startDragPos = Control.MousePosition;
                ItemClick(this, EventArgs.Empty);
            }

            private void OnMouseUp(object sender, MouseEventArgs e)
            {
                _startDrag = false;
            }
            private void OnMouseClick(object sender, EventArgs e) { }

            private void OnMouseDoubleClick(object sender, EventArgs e)
            {
                if (ItemDoubleClick != null)
                {
                    ItemDoubleClick(this, EventArgs.Empty);
                }
            }

            private void OnPaint(object sender, PaintEventArgs e)
            {
                Graphics g = e.Graphics;
                Rectangle rect = ClientRectangle;
                if (_selected)
                {
                    g.FillRectangle(_selectionBrush, e.ClipRectangle);
                    ControlPaint.DrawBorder3D(g, rect, _itemSelectionBorder);
                }
                if (!_selected && _highlighted)
                {
                    g.FillRectangle(_itemHooverBrush, e.ClipRectangle);
                    ControlPaint.DrawBorder3D(g, rect, _itemHoverBorder);
                }
                if (_image != null)
                {
                    g.DrawImage(_image, 4, (rect.Height - _image.Height) / 2);
                }
                rect.Width -= rect.Height + 6;
                rect.X += rect.Height + 4;
                g.DrawString(_caption, _font, _textBrush, rect, _stringFormat);
            }
        }

}
