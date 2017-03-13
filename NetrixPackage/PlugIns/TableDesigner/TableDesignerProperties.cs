using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace GuruComponents.Netrix.TableDesigner
{

    /// <summary>
    /// This class builds a property object to control the overall behavior of the table designer.
    /// </summary>
    /// <remarks>
    /// The constructor sets some defaults:
    /// <list type="table">
    ///     <listheader><item>Property</item><item>Value</item></listheader>
    ///     <item><term>SliderActivated</term><description>true</description></item>
    ///     <item><term>SliderAddMode</term><description>false</description></item>
    ///     <item><term>SliderLine</term><description>brown, dashed, 1px</description></item>
    ///     <item><term>WithCellSelection</term><description>true</description></item>
    ///     <item><term>CellSelectionColor</term><description>silver</description></item>
    ///     <item><term>CellBorderBehavior</term><description>black, dotted, 1px</description></item>
    ///     <item><term>TableBorderBehavior</term><description>black, solid, 1px</description></item>
    /// </list>
    /// </remarks>
    [Serializable()]
    public class TableDesignerProperties : INotifyPropertyChanged
    {
        private bool active;

        private bool sliderActivated;
        private bool sliderAddMode;

        private bool withCellSelection;
        private Color cellSelectionColor;
        private Color cellSelectionBorderColor;
        private Color tableBackground;

        private bool processTABKey;
        private bool fastResizeMode;
        private bool oldSliderMode;
        private bool oldSliderWasSet;

        private bool staticBehavior;
        private bool advancedParameters;

        /// <summary>
        /// This constructor sets the designer properties. 
        /// </summary>
        /// <remarks>
        /// It sets various default values for the table:
        /// All values can be changed using the various properties this class provides.
        /// </remarks>
        public TableDesignerProperties()
        {
            active = false;
            advancedParameters = false;
            sliderAddMode = false;
            sliderLine = new SliderLineProperty(Color.Brown, 2, DashStyle.Solid);
            tableBorder = new TableBorderBehavior(Color.Black, 1, DashStyle.Solid);
            cellBorderBehavior = new CellBorderBehavior(Color.Black, 1, DashStyle.Dot);
            withCellSelection = true;
            cellSelectionColor = Color.Silver;
            CellSelectionBorderColor = Color.Black;
            tableBackground = Color.White;
            SliderActivated = true;
            processTABKey = false;
            StaticBehavior = false;
            FormatterProperties = new TableFormatterProperties();
        }

        private TableFormatterProperties _FormatterProperties;
        /// <summary>
        /// Changes the default Table Formatter properties.
        /// </summary>
        [Browsable(true)]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        [Category("NetRix Table Designer")]
        [Description("Changes the default Table Formatter properties.")]
        public TableFormatterProperties FormatterProperties
        {
            set
            {
                _FormatterProperties = value;
                OnPropertyChanged("FormatterProperties");
            }
            get
            {
                return _FormatterProperties;
            }
        }

        /// <summary>
        /// Activate Table Designer and Behaviors.
        /// </summary>
        [Browsable(true), Category("NetRix Table Designer")]
        [Description("Activate Table Designer and Behaviors.")]
        [DefaultValue(false)]
        public bool Active
        {
            set
            {
                active = value;
                OnPropertyChanged("Active");
            }
            get
            {
                return active;
                
            }
        }

        /// <summary>
        /// Activates / Deactivates the slider mode.
        /// </summary>
        /// <remarks>
        /// It is recommended to turn it off on slow systems (500 MHz and less).
        /// </remarks>
        [Browsable(true), Category("NetRix Table Designer")]
        [Description("Activates / Deactivates the slider mode.")]
        [DefaultValue(true)]
        public bool SliderActivated
        {
            get
            {
                return this.sliderActivated;
            }
            set
            {
                this.sliderActivated = value;
                OnPropertyChanged("SliderActivated");
            }
        }

        /// <summary>
        /// If true, all tables on page will render as table mode, otherwise only the active (current) table.
        /// </summary>
        [Browsable(true), Category("NetRix Table Designer")]
        [Description("If true, all tables on page will render as table mode, otherwise only the active (current) table.")]
        [DefaultValue(false)]
        public bool StaticBehavior
        {
            get
            {
                return this.staticBehavior;
            }
            set
            {
                this.staticBehavior = value;
                OnPropertyChanged("StaticBehavior");
            }
        }

        /// <summary>
        /// Shows advanced information during cell resize like Visual Studio or Dreamweaver.
        /// </summary>
        [Browsable(true), Category("NetRix Table Designer")]
        [Description("Shows advanced information during cell resize like Visual Studio or Dreamweaver.")]
        [DefaultValue(false)]
        public bool AdvancedParameters
        {
            get { return advancedParameters; }
            set
            {
                advancedParameters = value;
                OnPropertyChanged("AdvancedParameters");
            }
        }

        /// <summary>
        /// Activates / Deactivates the fast resize mode.
        /// </summary>
        /// <remarks>
        /// Normally the resize mode works in a WYSIWYG manner and shows the result of the next step 
        /// immediately. With large tables this can be a huge performance flaw. The host application
        /// should enable fast resize mode when a table has more than 10 rows or columns and complex
        /// content or embedded tables. In that case the slider is automatically turned on and shows
        /// the final cell position. The table is redrawn only once after the user releases the mouse 
        /// button. 
        /// </remarks>
        [Browsable(true), Category("NetRix Table Designer")]
        [Description("Activates / Deactivates the fast resize mode.")]
        [DefaultValue(false), RefreshProperties(RefreshProperties.Repaint)]
        public bool FastResizeMode
        {
            get
            {
                return this.fastResizeMode;
            }
            set
            {
                this.fastResizeMode = value;
                // Set the slider mode, but preserve the previously set state
                if (value)
                {
                    this.oldSliderMode = this.SliderActivated;
                    this.oldSliderWasSet = true;
                    this.SliderActivated = true;
                }
                else
                {
                    if (oldSliderWasSet)
                    {
                        this.oldSliderWasSet = false;
                        this.SliderActivated = this.oldSliderMode;
                    }
                }
                OnPropertyChanged("FastResizeMode");
            }
        }
        /// <summary>
        /// If true the TAB key will move the caret inside the table.
        /// </summary>
        /// <remarks>
        /// This will prevent the TAB key
        /// from beeing moved around through the controls collection of the form. To get the normal
        /// behavior of the TAB key this option must be set to false.
        /// <para>
        /// If the table designer is not activated the TAB key has always there normal behavior.
        /// </para>
        /// </remarks>
        [Browsable(true), Category("NetRix Table Designer")]
        [Description("If true the TAB key will move the caret inside the table.")]
        [DefaultValue(false)]
        public bool ProcessTABKey
        {
            get
            {
                return processTABKey;
            }
            set
            {
                processTABKey = value;
                OnPropertyChanged("ProcessTABKey");
            }
        }

        /// <summary>
        /// The <see cref="System.Boolean"/> used to modify the slider behavior.
        /// </summary>
        /// <remarks>
        /// If true the current
        /// column or row will grow/shrink and the previous column or row will not change their widht
        /// or height, respectively. This results in a growing/shrinking table.
        /// </remarks>
        [Browsable(true), Category("NetRix Table Designer")]
        [Description("If true changing cell width will grow table.")]
        [DefaultValue(false)]
        public bool SliderAddMode
        {
            set
            {
                sliderAddMode = value;
                OnPropertyChanged("SliderAddMode");
            }
            get
            {
                return sliderAddMode;
            }
        }

        # region SliderLine

        SliderLineProperty sliderLine;

        /// <summary>
        /// Class which is used to collect properties of the slider
        /// </summary>
        [Serializable()]
        public class SliderLineProperty
        {

            private Color sliderLineColor;
            private int sliderLineWidth;
            private DashStyle sliderLineDashStyle;

            /// <summary>
            /// Sets default properties fpor slider.
            /// </summary>
            /// <param name="color"></param>
            /// <param name="width"></param>
            /// <param name="style"></param>
            public SliderLineProperty(Color color, int width, DashStyle style)
            {
                this.sliderLineColor = color;
                this.sliderLineWidth = width;
                this.sliderLineDashStyle = style;
            }

            /// <summary>
            /// The <see cref="System.Drawing.Pen"/> used to draw the slider line.
            /// </summary>
            [Browsable(true), Category("NetRix Table Designer")]
            [Description("Color of slider.")]
            [DefaultValue(typeof(Color), "Brown")]
            public Color SliderLineColor
            {
                set
                {
                    sliderLineColor = value;
                }
                get
                {
                    return sliderLineColor;
                }
            }

            /// <summary>
            /// Width of slider.
            /// </summary>
            [Browsable(true), Category("NetRix Table Designer")]
            [Description("Width of slider.")]
            [DefaultValue(2)]
            public int SliderLineWidth
            {
                set
                {
                    sliderLineWidth = value;
                }
                get
                {
                    return sliderLineWidth;
                }
            }

            /// <summary>
            /// Line style of slider.
            /// </summary>
            [Browsable(true), Category("NetRix Table Designer")]
            [Description("Style of slider.")]
            [DefaultValue(DashStyle.Solid)]
            public DashStyle SliderLineDashStyle
            {
                set
                {
                    sliderLineDashStyle = value;
                }
                get
                {
                    return sliderLineDashStyle;
                }
            }

            /// <summary>
            /// Designer support.
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return "Open to show properties";
            }

        }


        /// <summary>
        /// Properties of the slider.
        /// </summary>
        [Browsable(true), Category("NetRix Table Designer")]
        [Description("Behavior of slider.")]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public SliderLineProperty SliderLine
        {
            set
            {
                if (sliderLine == null)
                {
                    sliderLine = new SliderLineProperty(Color.Brown, 2, DashStyle.Solid);
                }
                sliderLine = value;
            }
            get
            {
                return sliderLine;
            }
        }

        # endregion

        /// <summary>
        /// The value used to activate cell selection (highlighting).
        /// </summary>
        [Browsable(true), Category("NetRix Table Designer")]
        [Description("Makes cells selectable.")]
        [DefaultValue(true)]
        public bool WithCellSelection
        {
            set
            {
                withCellSelection = value;
                OnPropertyChanged("WithCellSelection");
            }
            get
            {
                return withCellSelection;
            }
        }

        /// <summary>
        /// The background color of the whole table used during design mode.
        /// </summary>
        /// <remarks>
        /// This property defaults to white. It's recommended to set the value
        /// accordingly to the <see cref="GuruComponents.Netrix.WebEditing.Elements.Element.EffectiveStyle">EffectiveStyle</see> 
        /// background color property of the table to avoid text visible problems, if the text is also white.
        /// </remarks>
        [Browsable(true), Category("NetRix Table Designer")]
        [Description("Background Color of table.")]
        [DefaultValue(typeof(Color), "White")]
        public Color TableBackground
        {
            get
            {
                return tableBackground;
            }
            set
            {
                tableBackground = value;
                OnPropertyChanged("TableBackground");
            }
        }

        /// <summary>
        /// The <see cref="System.Drawing.Color">Color</see> used to highlight cell border during selection.
        /// </summary>            
        [Browsable(true), Category("NetRix Table Designer")]
        [Description("Border Color of selected cells.")]
        [DefaultValue(typeof(Color), "Black")]
        public Color CellSelectionBorderColor
        {
            set
            {
                cellSelectionBorderColor = value;
                OnPropertyChanged("CellSelectionBorderColor");
            }
            get
            {
                return cellSelectionBorderColor;
            }
        }

        /// <summary>
        /// The <see cref="System.Drawing.Color"/> used to highlight cell background during selection.
        /// </summary>            
        [Browsable(true), Category("NetRix Table Designer")]
        [Description("Background Color of selected cells.")]
        [DefaultValue(typeof(Color), "Silver")]
        public Color CellSelectionColor
        {
            set
            {
                cellSelectionColor = value;
                OnPropertyChanged("CellSelectionColor");
            }
            get
            {
                return cellSelectionColor;
            }
        }

        CellBorderBehavior cellBorderBehavior;

        /// <summary>
        /// Covers several options for cell border appearance.
        /// </summary>
        [Serializable()]
        [DefaultValue("Open to show properties")]
        public class CellBorderBehavior : INotifyPropertyChanged
        {

            private Color cellBorderColor;
            private int cellBorderWidth;
            private DashStyle cellBorderDashStyle;

            /// <summary>
            /// Creates a new appearance object with the given properties.
            /// </summary>
            /// <param name="color">Default border color.</param>
            /// <param name="width">Default border width.</param>
            /// <param name="style">Default border style.</param>
            public CellBorderBehavior(Color color, int width, DashStyle style)
            {
                this.cellBorderColor = color;
                this.cellBorderWidth = width;
                this.cellBorderDashStyle = style;
            }


            /// <summary>
            /// The <see cref="System.Drawing.Pen"/> used to draw the slider line.
            /// </summary>
            [Browsable(true), Category("NetRix Table Designer")]
            [Description("Color of cells.")]
            [DefaultValue(typeof(Color), "Black")]
            public Color CellBorderColor
            {
                set
                {
                    cellBorderColor = value;
                    OnPropertyChanged("CellBorderColor");
                }
                get
                {
                    return cellBorderColor;
                }
            }

            /// <summary>
            /// The border width of the cell.
            /// </summary>
            [Browsable(true), Category("NetRix Table Designer")]
            [Description("Border width of cells.")]
            [DefaultValue(1)]
            public int CellBorderWidth
            {
                set
                {
                    cellBorderWidth = value;
                    OnPropertyChanged("CellBorderWidth");
                }
                get
                {
                    return cellBorderWidth;
                }
            }

            /// <summary>
            /// The dash style of the cell.
            /// </summary>
            [Browsable(true), Category("NetRix Table Designer")]
            [Description("Style of cell border.")]
            [DefaultValue(DashStyle.Dot)]
            public DashStyle CellBorderDashStyle
            {
                set
                {
                    cellBorderDashStyle = value;
                    OnPropertyChanged("CellBorderDashStyle");
                }
                get
                {
                    return cellBorderDashStyle;
                }
            }

            /// <summary>
            /// Overwritten to support property browser.
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return "Open to show properties";
            }
            /// <summary>
            /// Called internally for the PropertyChanged event.
            /// </summary>
            /// <param name="info"></param>
            protected virtual void OnPropertyChanged(string info)
            {
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(info));
                }
            }
            /// <summary>
            /// Fired if a property has been changed.
            /// </summary>
            public event PropertyChangedEventHandler PropertyChanged;
        }

        /// <summary>
        /// Sets the border appearance.
        /// </summary>
        [Browsable(true), Category("NetRix Table Designer")]
        [Description("Behavior of cells.")]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public CellBorderBehavior CellBorder
        {
            set
            {
                if (cellBorderBehavior == null)
                {
                    cellBorderBehavior = new CellBorderBehavior(Color.Black, 1, DashStyle.Dot);
                     //cellBorderBehavior.PropertyChanged += (o, e) => OnPropertyChanged(e.PropertyName);
                    //Because not working in 2005 above line
                    cellBorderBehavior.PropertyChanged += new PropertyChangedEventHandler(cellBorderBehavior_PropertyChanged);
                }
                cellBorderBehavior = value;
            }
            get
            {
                return cellBorderBehavior;
            }
        }

        void cellBorderBehavior_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(e.PropertyName);
        }

        /// <summary>
        /// Class which containts information about table border layout.
        /// </summary>
        [Serializable()]
        public class TableBorderBehavior : INotifyPropertyChanged
        {

            private Color tableBorderColor;
            private int tableBorderWidth;
            private DashStyle tableBorderDashStyle;

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="color"></param>
            /// <param name="width"></param>
            /// <param name="style"></param>
            public TableBorderBehavior(Color color, int width, DashStyle style)
            {
                this.tableBorderColor = color;
                this.tableBorderWidth = width;
                this.tableBorderDashStyle = style;
            }

            /// <summary>
            /// Color of table border.
            /// </summary>
            [Browsable(true), Category("NetRix Table Designer")]
            [Description("Color of table border.")]
            [DefaultValue(typeof(Color), "Black")]
            public Color TableBorderColor
            {
                set
                {
                    tableBorderColor = value;
                    OnPropertyChanged("TableBorderColor");
                }
                get
                {
                    return tableBorderColor;
                }
            }

            /// <summary>
            /// Width of table border.
            /// </summary>
            [Browsable(true), Category("NetRix Table Designer")]
            [Description("Width of table border.")]
            [DefaultValue(1)]
            public int TableBorderWidth
            {
                set
                {
                    tableBorderWidth = value;
                    OnPropertyChanged("TableBorderWidth");
                }
                get
                {
                    return tableBorderWidth;
                }
            }

            /// <summary>
            /// Style of table border.
            /// </summary>
            [Browsable(true), Category("NetRix Table Designer")]
            [Description("Style of table border.")]
            [DefaultValue(DashStyle.Solid)]
            [Bindable(true)]
            public DashStyle TableBorderDashStyle
            {
                set
                {
                    tableBorderDashStyle = value;
                    OnPropertyChanged("TableBorderDashStyle");
                }
                get
                {
                    return tableBorderDashStyle;
                }
            }

            /// <summary>
            /// Overwritten to support property browser.
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return "Open to show properties";
            }
            /// <summary>
            /// Called internally for the PropertyChanged event.
            /// </summary>
            /// <param name="info"></param>
            protected virtual void OnPropertyChanged(string info)
            {
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(info));
                }
            }
            /// <summary>
            /// Fired if a property has been changed.
            /// </summary>
            public event PropertyChangedEventHandler PropertyChanged;
        }

        TableBorderBehavior tableBorder;

        /// <summary>
        /// Layout of table border.
        /// </summary>
        [Browsable(true), Category("NetRix Table Designer")]
        [Description("Layout of table border.")]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public TableBorderBehavior TableBorder
        {
            set
            {
                if (tableBorder == null)
                {
                    tableBorder = new TableBorderBehavior(Color.Black, 1, DashStyle.Dash);
                   // tableBorder.PropertyChanged += (o, e) => OnPropertyChanged(e.PropertyName);
                   //Because not working in 2005 above line
                    tableBorder.PropertyChanged += new PropertyChangedEventHandler(tableBorder_PropertyChanged);
                   
                }
                tableBorder = value;
            }
            get
            {
                return tableBorder;
            }
        }

        void tableBorder_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(e.PropertyName);
        }
      

        /// <summary>
        /// Supports the propertygrid element.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            int changed = 0;
            changed += (active != false) ? 1 : 0;
            changed += (sliderAddMode != false) ? 1 : 0;

            changed += (sliderLine.SliderLineColor != Color.Brown) ? 1 : 0;
            changed += (sliderLine.SliderLineWidth != 2) ? 1 : 0;
            changed += (sliderLine.SliderLineDashStyle != DashStyle.Solid) ? 1 : 0;

            changed += (tableBorder.TableBorderColor != Color.Black) ? 1 : 0;
            changed += (tableBorder.TableBorderWidth != 1) ? 1 : 0;
            changed += (tableBorder.TableBorderDashStyle != DashStyle.Solid) ? 1 : 0;

            changed += (cellBorderBehavior.CellBorderColor != Color.Black) ? 1 : 0;
            changed += (cellBorderBehavior.CellBorderWidth != 1) ? 1 : 0;
            changed += (cellBorderBehavior.CellBorderDashStyle != DashStyle.Dot) ? 1 : 0;

            changed += (withCellSelection != true) ? 1 : 0;
            changed += (cellSelectionColor != Color.Silver) ? 1 : 0;
            changed += (CellSelectionBorderColor != Color.Black) ? 1 : 0;
            changed += (tableBackground != Color.White) ? 1 : 0;
            changed += (SliderActivated != true) ? 1 : 0;
            changed += (processTABKey != false) ? 1 : 0;
            changed += (StaticBehavior != false) ? 1 : 0;
            changed += (advancedParameters != false) ? 1 : 0;

            return String.Format("{0} propert{1} changed", changed, (changed == 1) ? "y" : "ies");
        }
        /// <summary>
        /// Called internally for the PropertyChanged event.
        /// </summary>
        /// <param name="info"></param>
        protected virtual void OnPropertyChanged(string info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
        /// <summary>
        /// Fired if a property has been changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
