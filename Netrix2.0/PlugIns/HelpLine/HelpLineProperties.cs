using System;
using System.ComponentModel;
using System.Drawing;

namespace GuruComponents.Netrix.HelpLine
{
	/// <summary>
	/// Properties for HelpLine Extender Provider.
	/// </summary>
    /// <remarks>
    /// In code, you can set the properties that way:
    /// <code>
    /// // Prepare Properties
    /// hp1.Active = true;
    /// hp1.LineColor = Color.Blue;
    /// hp1.CrossEnabled = false;
    /// hp1.LineYEnabled = false;
    /// hp1.LineXEnabled = false;
    /// hp1.SnapElements = false;
    /// hp1.SnapOnResize = false;
    /// hp1.SnapEnabled = false;
    /// hp1.X = (int)(210/25.4)*DPI;
    /// // Create extender
    /// this.helpLineA4 = new HelpLine(this.components);
    /// // hook events (optional)
    /// this.helpLineA4.HelpLineMoving += new HelpLineMoving(this.helpLineA4_HelpLineMoving);
    /// this.helpLineA4.HelpLineMoved += new HelpLineMoved(this.helpLineA4_HelpLineMoved);
    /// // Assign properties and editor
    /// this.helpLineA4.SetHelpLine(this.htmlEditor, hp1);
    /// // ... some more code
    /// </code>
    /// <para>
    /// This way you have to create one <see cref="HelpLine"/> object per line (in fact, it's a pair of line). This can be assigned
    /// to one or more editor controls. This means, if you need four editors, each with two lines, you need to create exactly two
    /// <see cref="HelpLine"/> extenders, assigned to each of the four editors. The extender can manage the properties for such an
    /// multiple assignment.
    /// </para>
    /// <para>
    /// Finally, you can change the properties on the fly. This needs access to the assigned properties. Therefore, you cannot
    /// access the (in the example above) <c>h1</c> variable. Instead, you must retrieve the assigned version:
    /// <code>
    /// helpLineA4.GetHelpLine(htmlEditor).LineXEnabled = true;
    /// </code>
    /// You may also 'refresh' your local property object from extender:
    /// <code>
    /// h1 = helpLineA4.GetHelpLine(htmlEditor);
    /// h1.LineColor = Color.Red;
    /// // ... more properties
    /// </code>
    /// </para>
    /// </remarks>
    [Serializable()]
	public class HelpLineProperties 
	{

        private bool active;
        private bool crossEnabled;
        private int snapZone;
        private bool snapEnabled;
        private bool snapOnResize;
        private int snapgrid;
        private bool snapelements;
        private bool lineXEnabled;
        private bool lineYEnabled;
        private int x;
        private int y;
        private Color color;
        private int width;
        private System.Drawing.Drawing2D.DashStyle dash;
        private bool lineVisible;
        [NonSerialized()]
        private HelpLineBehavior behavior;

        /// <summary>
        /// Ctor for properties.
        /// </summary>
		public HelpLineProperties()
		{
            active = true;
            
            crossEnabled = true;
            snapOnResize = true;
            snapEnabled = true;
            snapZone = 12;
            snapgrid = 16;
            snapelements = true;
            lineXEnabled = true;
            lineYEnabled = true;
            x = 100;
            y = 100;
            color = Color.Blue;
            lineVisible = true;
            width = 1;
            dash = System.Drawing.Drawing2D.DashStyle.Solid;
        }

        /// <summary>
        /// Supports propertygrid
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            int props = 0;
            props += (active == false) ? 0 : 1;
            props += (crossEnabled == true) ? 0 : 1;
            props += (snapOnResize == true) ? 0 : 1;
            props += (snapEnabled == true) ? 0 : 1;
            props += (snapZone == 12) ? 0 : 1;
            props += (snapgrid == 16) ? 0 : 1;
            props += (snapelements == true) ? 0 : 1;
            props += (lineXEnabled == true) ? 0 : 1;
            props += (lineYEnabled == true) ? 0 : 1;
            props += (x == 100) ? 0 : 1;
            props += (y == 100) ? 0 : 1;
            props += (color == Color.Blue) ? 0 : 1;
            props += (lineVisible == true) ? 0 : 1;
            props += (width == 1) ? 0 : 1;
            props += (dash == System.Drawing.Drawing2D.DashStyle.Solid) ? 0 : 1;
            return String.Format("{0} {1} changed", props, (props == 1) ? "property" : "properties");
        }

        internal void SetBehaviorReference(HelpLineBehavior behavior)
        {
            this.behavior = behavior;
        }
            
        /// <summary>
        /// Current status of the event management.
        /// </summary>
        /// <remarks>
        /// By default the helpline is active. If this property is set to <c>false</c> the control stops firing events and receiving mouse control.
        /// The user is no longer able to move the helpline around. The helpline remains visible.
        /// </remarks>
        /// <seealso cref="LineVisible"/>
        [Category("Behavior"), DefaultValue(true)]
        public bool Active
        {
            get
            { 
                return active;
            }
            set
            {
                active = value;                
            }
        }

        /// <summary>
        /// Current X position in pixel.
        /// </summary>
        [Category("Behavior"), DefaultValue(100), Description("Current X position in pixel.")]
        public int X
        {
            get
            {
                if (behavior != null)
                {
                    x = behavior.X;
                }
                return x;
            }
            set
            {
                x = value;
                if (behavior != null)
                {
                    behavior.SetX(x);
                    behavior.Invalidate();
                }
            }
        }

        /// <summary>
        /// Current Y position in pixel.
        /// </summary>
        [Category("Behavior"), DefaultValue(100), Description("Current Y position in pixel.")]
        public int Y
        {
            get
            {
                if (behavior != null)
                {
                    y = behavior.Y;
                }
                return y;
            }
            set
            {
                y = value;
                if (behavior != null)
                {
                    behavior.SetY(y);
                    behavior.Invalidate();
                }
            }
        }

        /// <summary>
        /// Color of help lines.
        /// </summary>
        [Category("Layout"), DefaultValue(typeof(Color), "Blue"), Description("Color of help lines.")]
        public System.Drawing.Color LineColor
        {
            get
            { 
                return color;
            }
            set
            {
                color = value;
                if (behavior != null)
                {
                    behavior.LineStyle.Color = color;
                    behavior.Invalidate();
                }
            }
        }

        /// <summary>
        /// Width of line in pixel.
        /// </summary>
        [Category("Layout"), DefaultValue(1), Description("Width of pen in pixel.")]
        public int LineWidth
        {
            get
            {
                return width;
            }
            set
            {
                width = value;
                if (behavior != null)
                {
                    behavior.LineStyle.Width = width;
                    behavior.Invalidate();
                }
            }
        }

        /// <summary>
        /// Dashstyle of line.
        /// </summary>
        [Category("Layout"), DefaultValue(typeof(System.Drawing.Drawing2D.DashStyle), "Solid"), Description("Dashstyle of pen.")]
        public System.Drawing.Drawing2D.DashStyle LineDash
        {
            get
            {
                return dash;
            }
            set
            {
                dash = value;
                if (behavior != null)
                {
                    behavior.LineStyle.DashStyle = value;
                    behavior.Invalidate();
                }
            }
        }

        /// <summary>
        /// Make the helpline temporarily invisible.
        /// </summary>
        [Category("Layout"), DefaultValue(true), Description("Make the helpline temporarily invisible.")]
        public bool LineVisible
        {
            get
            {
                return lineVisible;
            }
            set
            {
                lineVisible = value;
                if (behavior != null)
                {
                    behavior.LineVisible = lineVisible;
                    behavior.Invalidate();
                }
            }
        }

        /// <summary>
        /// Gets or sets the snap zone in which the helpline is magnetic. Defaults to 12 pixel.
        /// </summary>
        [Category("Snap"), DefaultValue(12), Description("Gets or sets the snap zone in which the helpline is magnetic. Defaults to 12 pixel.")]
        public int SnapZone
        {
            get
            {
                return snapZone;
            }
            set
            {
                snapZone = value;
                if (behavior != null)
                {
                    behavior.SnapZone = snapZone;
                    behavior.Invalidate();
                }
            }
        }

        /// <summary>
        /// Gets or sets the snap on resize feature. If on the helpline will be magnetic during element resizing.
        /// </summary>
        [Category("Snap"), DefaultValue(true), Description("Gets or sets the snap on resize feature. If on the helpline will be magnetic during element resizing.")]
        public bool SnapOnResize
        {
            get
            {
                return snapOnResize;
            }
            set
            {
                snapOnResize = value;
                if (behavior != null)
                {
                    behavior.SnapOnResize = snapOnResize;
                    behavior.Invalidate();
                }
            }
        }

        /// <summary>
        /// Gets or sets the snap to grid feature. If on the helpline will be magnetic against a virtual grid.
        /// </summary>
        [Category("Snap"), DefaultValue(true), Description("Gets or sets the snap to grid feature. If on the helpline will be magnetic against a virtual grid.")]
        public bool SnapToGrid
        {
            get
            {
                return snapEnabled;
            }
            set
            {
                snapEnabled = value;
                if (behavior != null)
                {
                    behavior.SnapEnabled = snapEnabled;
                    behavior.Invalidate();
                }
            }
        }

        /// <summary>
        /// Enables the X line. Default is On.
        /// </summary>
        [Category("Layout"), DefaultValue(true), Description("Enables the X line. Default is On.")]
        public bool LineXEnabled
        {
            get
            {
                return lineXEnabled;
            }
            set
            {
                lineXEnabled = value;
                if (behavior != null)
                {
                    behavior.LineXEnabled = lineXEnabled;
                    behavior.Invalidate();
                }
            }
        }

        /// <summary>
        /// Enables the Y line. Default is On.
        /// </summary>
        [Category("Layout"), DefaultValue(true), Description("Enables the Y line. Default is On.")]
        public bool LineYEnabled
        {
            get
            {
                return lineYEnabled;
            }
            set
            {
                lineYEnabled = value;
                if (behavior != null)
                {
                    behavior.LineYEnabled = lineYEnabled;
                    behavior.Invalidate();
                }
            }
        }
        
        /// <summary>
        /// Enables the cross sign. 
        /// </summary>
        /// <remarks>
        /// Default is On, if both lines are visible, otherwise it's not visible.
        /// </remarks>
        [Category("Layout"), DefaultValue(true), Description("Enables the cross sign. Default is On.")]
        public bool CrossEnabled
        {
            get
            {
                return crossEnabled;
            }
            set
            {
                crossEnabled = value;
                if (behavior != null)
                {
                    behavior.CrossEnabled = crossEnabled;
                    behavior.Invalidate();
                }
            }
        }

        /// <summary>
        /// Let elements snap to the helpline.
        /// </summary>
        [Category("Snap"), DefaultValue(true), Description("Let elements snap to the helpline.")]
        public bool SnapElements
        {
            get
            {
                return snapelements;
            }

            set
            {
                snapelements = value;
                if (behavior != null)
                {
                    behavior.SnapElements = snapelements;
                    behavior.Invalidate();
                }
            }
        }

        /// <summary>
        /// Gets or sets the distance in pixels between the grid.
        /// </summary>
        /// <remarks>
        /// The grid is used to snap the helpline to fixed points. It has no relation to the Grid property
        /// exposed by the NetRix base control. However, it's recommended to set both, the virtual helpine grid and
        /// the visible grid to the same distance for best user experience.
        /// </remarks>
        [Category("Snap"), DefaultValue(16), Description("Gets or sets the distance in pixels between the grid.")]
        public int SnapGrid
        {
            get
            {
                return snapgrid;
            }

            set
            {
                snapgrid = value;
                if (behavior != null)
                {
                    behavior.SnapGrid = snapgrid;
                    behavior.Invalidate();
                }
            }
        }
    }
}
