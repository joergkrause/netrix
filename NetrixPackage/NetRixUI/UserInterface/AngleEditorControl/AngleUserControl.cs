using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using GuruComponents.Netrix.UserInterface.TypeEditors;
using GuruComponents.Netrix.UserInterface.TypeConverters;
using System.Drawing.Design;

namespace GuruComponents.Netrix.UserInterface
{

    /*
     * TODO:
     * 
     *  LabelPosition
     *  NumPosition
     *  ShapedBackground
     *  Runtime Resize
     * 
     * */


    /// <summary>
    /// A control to set an angle.
    /// </summary>
    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(GuruComponents.Netrix.UserInterface.ResourceManager), "Resources.ToolBox.AngleEditor.ico")]
    [DefaultEvent("AngleChanged")]
    public partial class AngleEditor : UserControl
    {

        private Pen penCircle;
        private Pen penSticks;
        private SolidBrush brushPoint;
        private int ps;
        private bool dontRefire;
        private bool drawSticks;

        /// <summary>
        /// A user control to enter angle values.
        /// </summary>
        public AngleEditor()
        {
            InitializeComponent();
            penCircle = new Pen(new SolidBrush(Color.Black), 2F);
            penSticks = new Pen(new SolidBrush(Color.Blue), 2F);
            brushPoint = new SolidBrush(Color.Red);
            ps = 8;
            dontRefire = false;
            drawSticks = true;
            labelDesc.Visible = false;
        }

        /// <summary>
        /// Overridden to paint the content.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics gr = e.Graphics;
            gr.CompositingQuality = CompositingQuality.HighQuality;
            gr.SmoothingMode = SmoothingMode.AntiAlias;
            int w = Width-(ps*2);
            int h = Height-(ps*2);
            int ph = (ps/2);
            if (drawSticks)
            {
                gr.DrawLine(penSticks, Width / 2, ph, Width / 2, ps * 2 - ph);
                gr.DrawLine(penSticks, ph, Height / 2, ps * 2 - ph, Height / 2);
                gr.DrawLine(penSticks, Width / 2, Height - ph, Width / 2, Height - ps * 2 + ph);
                gr.DrawLine(penSticks, Width - ph, Height / 2, Width - ps * 2 + ph, Height / 2);
            }
            gr.DrawEllipse(penCircle, ps, ps, w, h);
            double angle = (double)numericUpDown1.Value * Math.PI / 180;
            double a1 = Math.Cos(angle);
            double a2 = Math.Sin(angle);            
            gr.FillEllipse(brushPoint, 
                ph + Convert.ToInt32(w -  ((w/2) - ((w/2) * a1))),
                ph + Convert.ToInt32(h - ((h/2) + ((h/2) * a2))),
                ps,
                ps);
            gr.Dispose();
            base.OnPaint(e);
        }

        /// <summary>
        /// Invalidates on resize.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Invalidate();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            OnAngleChanged((int)numericUpDown1.Value);
            Invalidate();
        }

        private void CalculateNum(MouseEventArgs e) {
            double dx = e.X - (this.Width / 2);
            double dy = e.Y - (this.Height / 2);
            if (dy != 0) {
                if (dx >= 0 && dy >= 0) numericUpDown1.Value = 270 + Convert.ToInt16(Math.Abs(Convert.ToDecimal(180 * Math.Atan(dx / dy) / Math.PI)));
                if (dx <  0 && dy >= 0) numericUpDown1.Value = 270 - Convert.ToInt16(Math.Abs(Convert.ToDecimal(180 * Math.Atan(dx / dy) / Math.PI)));
                if (dx <  0 && dy <  0) numericUpDown1.Value =  90 + Convert.ToInt16(Math.Abs(Convert.ToDecimal(180 * Math.Atan(dx / dy) / Math.PI)));
                if (dx >= 0 && dy <  0) numericUpDown1.Value =  90 - Convert.ToInt16(Math.Abs(Convert.ToDecimal(180 * Math.Atan(dx / dy) / Math.PI)));
                //System.Diagnostics.Debug.WriteLine("dx/dy=" + (dx / dy));
            }
        }

        /// <summary>
        /// Control mouse to move the spot.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseMove(MouseEventArgs e) {
            if (e.Button == MouseButtons.Left) {
                CalculateNum(e);
            }
        }

        /// <summary>
        /// Control mouse to get the spot.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseClick(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) {
                CalculateNum(e);
            }
        }
        
        /// <summary>
        /// Fired if angle has been changed.
        /// </summary>
        [Category("NetRix")]
        public event AngleChangedEventHandler AngleChanged;

        /// <summary>
        /// Called if angle has been changed.
        /// </summary>
        /// <param name="angle"></param>
        protected void OnAngleChanged(int angle)
        {
            if (dontRefire) return;
            if (AngleChanged != null)
            {
                AngleChangedEventArgs e = new AngleChangedEventArgs(angle);
                dontRefire = true;
                AngleChanged(this, e);
                Angle = e.Angle;
                dontRefire = false;
            }
        }

        /// <summary>
        /// Current Angle set by user.
        /// </summary>
        [Category("Angle Editor")]
        [DefaultValue(0)]
        public decimal Angle
        {
            get { return numericUpDown1.Value; }
            set { numericUpDown1.Value = value; }
        }

        /// <summary>
        /// Color or 'pointer' (spot for mouse).
        /// </summary>
        [Category("Angle Editor")]
        [DefaultValue(typeof(Color), "Red")]
        [TypeConverterAttribute(typeof(UITypeConverterColor))]
        [EditorAttribute(
             typeof(UITypeEditorColor),
             typeof(UITypeEditor))]
        public Color PointerColor
        {
            get { return brushPoint.Color; }
            set { brushPoint.Color = value; Invalidate();}
        }

        /// <summary>
        /// Color of circle around the angle area.
        /// </summary>
        [Category("Angle Editor")]
        [DefaultValue(typeof(Color), "Black")]
        [TypeConverterAttribute(typeof(UITypeConverterColor))]
        [EditorAttribute(
             typeof(UITypeEditorColor),
             typeof(UITypeEditor))]
        public Color CircleColor
        {
            get { return penCircle.Color; }
            set { penCircle.Color = value; Invalidate(); }
        }

        /// <summary>
        /// Color of sticks used to tag 0°, 90°, 180, and 270° positions.
        /// </summary>
        /// <seealso cref="DrawSticks"/>
        [Category("Angle Editor")]
        [DefaultValue(typeof(Color), "Blue")]
        [TypeConverterAttribute(typeof(UITypeConverterColor))]
        [EditorAttribute(
             typeof(UITypeEditorColor),
             typeof(UITypeEditor))]
        public Color StickColor
        {
            get { return penSticks.Color; }
            set { penSticks.Color = value; Invalidate(); }
        }

        /// <summary>
        /// Draw the sticks at 0°, 90°, 180, and 270° positions.
        /// </summary>
        /// <seealso cref="StickColor"/>
        [Category("Angle Editor")]
        [DefaultValue(true)]
        public bool DrawSticks
        {
            get { return drawSticks; }
            set { drawSticks = value; Invalidate(); }
        }

        /// <summary>
        /// Show an additional label.
        /// </summary>
        /// <seealso cref="LabelProperties"/>
        [Category("Angle Editor")]
        [DefaultValue(false)]
        public bool ShowLabel
        {
            get { return labelDesc.Visible; }
            set { labelDesc.Visible = value; Invalidate(); }
        }

        /// <summary>
        /// Size of the pointer (mouse spot). Default is 8 (pixel).
        /// </summary>
        [Category("Angle Editor")]
        [DefaultValue(8)]
        public int PointerSize
        {
            get { return ps; }
            set { ps = value; Invalidate(); }
        }

        /// <summary>
        /// Access to the NumericUpDown control.
        /// </summary>
        [Category("Angle Editor")]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public NumericUpDown NumUpDownProperties
        {
            get { return numericUpDown1; }
        }

        /// <summary>
        /// Access to the label control.
        /// </summary>
        /// <seealso cref="ShowLabel"/>
        [Category("Angle Editor")]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public Label LabelProperties
        {
            get { return labelDesc; }
        }

        private void AngleEditor_MouseMove(object sender, MouseEventArgs e)
        {
            Rectangle r = new Rectangle(numericUpDown1.Location, numericUpDown1.Size);
            if (r.Contains(e.X, e.Y))
            {
                Cursor = Cursors.Default;
            }
        }


    }
}
