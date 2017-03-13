using System.ComponentModel;
using GuruComponents.Netrix.ComInterop;
using Comzept.Genesis.NetRix.VgxDraw;
using GuruComponents.Netrix.VmlDesigner.DataTypes;

namespace GuruComponents.Netrix.VmlDesigner.Elements
{
	/// <summary>
	/// This sub-element may appear inside a shape or a shapetype to define the path that makes up the shape.
	/// </summary>
	/// <remarks>
	/// This is done through a string that contains a rich set of pen movement commands.  
	/// This sub-element also describes the limo-stretch point, inscribed textbox rectangle locations, and connection 
	/// site locations.  The limo-stretch definition and the formulas element (described below) allow greater 
	/// designer control of how the path scales.   They allow, for example, definition of a true rounded corner 
	/// rectangle where the corners remain circular even though the rectangle is scaled anisotropically.
	/// <para>
    /// Edit behavior extensions: VML does not mandate a user interface for editing applications. It attempts to convey information 
    /// about the object which is being edited - this may imply the behavior of an editor.  One common operation implied 
    /// by VML is the need to edit the points in a path. The edit behavior extensions attempt to identify some common 
    /// behavior of v objects so that applications behave consistently however the information encoded is very low level.  
    /// Consequently these extensions may be ignored completely by a conforming application and any conforming application 
    /// is free to remove or rewrite the edit information in the path. The extensions define the behavior of all 
    /// following points under editing operations which move the points or the associated line segments. Nine different 
    /// behaviors are identified for the vertices in the path attribute (the end points, not the control points) 
    /// depending on whether the associated line segment is a line or curve.
	/// </para>
    /// command Name parameters Description 
    ///    vertex behavior line segment 
    ///    ha AutoLine 0 auto line 
    ///    hb AutoCurve 0 auto curve 
    ///    hc CornerLine 0 corner line 
    ///    hd CornerCurve 0 corner curve 
    ///    he SmoothLine 0 smooth line 
    ///    hf SmoothCurve 0 smooth curve 
    ///    hg SymmetricLine 0 symmetric line 
    ///    hh SymmetricCurve 0 symmetric curve 
    ///    hi Freeform 0 auto any 
    ///<para>
    ///The line segment type defines whether the behavior applies to points which are adjacent to lines or whether 
    ///it applies to points adjacent to curves. The vertex behavior specifies how the two line segments either side 
    ///of a point are expected to behave as the point is moved.
    ///</para>
    ///    Vertex behavior Are (curve) control points calculated automatically? Are control points either side of the 
    ///    vertex equidistant? Are control points co-linear with the vertex? Are control points visible to the user? 
    ///    Auto yes - - no  
    ///    Symmetric no yes yes yes 
    ///    Smooth no no yes yes 
    ///    Corner no no no yes 
    ///    Freeform no no no yes 
    ///
    ///    The auto behavior implements some application-defined algorithm to guess the correct control points when 
    ///    a point is moved. This is, effectively, the default - it implies that the application should use other 
    ///    information to determine the control point behavior. The symmetric, smooth and corner behaviors determine how 
    ///    one control point behaves when the other at that vertex is moved. The freeform behavior does not recalculate 
    ///    control point position as vertices are moved.
    ///    <para>
    ///    Lexical format of the v attribute value
    ///    This value consists of commands followed by zero or more parameters.  The number of parameters is given in the table above.  In this table, the suffix "*" indicates that the parameter set may be repeated (but the number of parameters must be a multiple of the given number).  The quadratic bézier must have more than two pairs of parameters. 
    ///    </para>
    ///    Either commas or spaces may be used to delimit parameters for each command.  E.g. "m 0,0" and "m0 0" are both acceptable. 
    ///    Parameters that are zero may be omitted using commas with no parameter.  E.g. "c 10,10,0,0,25,13" and "c 10,10,,,25,13" are equivalent. 
    ///    Parameterized paths are also allowed. In this case, the shape must also have a formula element with a list of formulas that may be substituted into the path using the @ symbol followed by the number of the formula. The adj property of the shape contains the input parameters for these formulas.  E.g. "moveto @1@4".   The evaluations of the formulas are substituted into the appropriate positions.   Note that @ also serves as a delimiter. 
    ///In the event that a path is malformed VML requires the following behavior if the page is displayed. 
    ///<para>
    ///Missing parameter values must be supplied as 0. 
    ///Unrecognized commands must be skipped - they should be treated as though they are space characters. 
    ///An application is also permitted to fail to display the page (with a diagnostic) or to alert the user that some content is malformed.
    ///</para>
	/// </remarks>
	[ToolboxItem(false)]
    public sealed class PathElement : VmlBaseElement
	{

        /// <summary>
        /// Unique ID.
        /// </summary>
        [Browsable(true), Category("Element Layout"), DefaultValue("")]
        public string Id
        {
            get
            {
                return base.GetStringAttribute("id");
            }
            set
            {
                base.SetStringAttribute("id", value);
            }
        }

        /// <summary>
        /// A string containing the commands that define the path. (See below for definition of the command set).
        /// </summary>
        /// <remarks>
        /// The v attribute string (or the path property of shape) is made up of a rich set of commands as summarized in the following table:
        /// <list type="table">
        ///     <listheader>
        ///         <item>Command</item><item>Name</item><item>Parameter</item><item>Description</item>
        ///     </listheader>
        ///     <term>
        ///         <item>m</item><item>moveto</item><item>2</item><item>Description</item>
        ///     </term>
        ///     <term>
        ///         <item>l</item><item>lineto</item><item>2*</item><item>Description</item>
        ///     </term>
        ///     <term>
        ///         <item>c</item><item>curveto</item><item>6*</item><item>Description</item>
        ///     </term>
        ///     <term>
        ///         <item>x</item><item>close</item><item>0</item><item>Description</item>
        ///     </term>
        ///     <term>
        ///         <item>e</item><item>end</item><item>0</item><item>Description</item>
        ///     </term>
        ///     <term>
        ///         <item>t</item><item>rmoveto</item><item>2*</item><item>Description</item>
        ///     </term>
        ///     <term>
        ///         <item>r</item><item>rlineto</item><item>2*</item><item>Description</item>
        ///     </term>
        ///     <term>
        ///         <item>v</item><item>rcurveto</item><item>6*</item><item>Description</item>
        ///     </term>
        ///     <term>
        ///         <item>nf</item><item>nofill</item><item>0</item><item>Description</item>
        ///     </term>
        ///     <term>
        ///         <item>ns</item><item>nostroke</item><item>0</item><item>Description</item>
        ///     </term>
        ///     <term>
        ///         <item>ae</item><item>angleellipseto</item><item>6*</item><item>Description</item>
        ///     </term>
        ///     <term>
        ///         <item>al</item><item>angleellipse</item><item>6*</item><item>Description</item>
        ///     </term>
        ///     <term>
        ///         <item>at</item><item>arcto</item><item>8*</item><item>Description</item>
        ///     </term>
        ///     <term>
        ///         <item>ar</item><item>arc</item><item>8*</item><item>Description</item>
        ///     </term>
        ///     <term>
        ///         <item>wa</item><item>clockwisearcto</item><item>8*</item><item>Description</item>
        ///     </term>
        ///     <term>
        ///         <item>wr</item><item>clockwisearc</item><item>8*</item><item>Description</item>
        ///     </term>
        ///     <term>
        ///         <item>qx</item><item>ellipticalqaudrantx</item><item>2*</item><item>Description</item>
        ///     </term>
        ///     <term>
        ///         <item>qy</item><item>ellipticalquadranty</item><item>2*</item><item>Description</item>
        ///     </term>
        ///     <term>
        ///         <item>qb</item><item>quadraticbezier</item><item>2*</item><item>Description</item>
        ///     </term>
        /// </list>
        /// </remarks>
        [Browsable(true), Category("Element Layout"), DefaultValue("")]
        public string V
        {
            get
            {
                return base.GetStringAttribute("v");
            }
            set
            {
                base.SetStringAttribute("v", value);
            }
        }

		/// <summary>
		/// A point along the x and y dimensions of a shape where the shape will limo stretch.
		/// </summary>
		/// <remarks>
		/// Defines the point where the shape is stretched; e.g., for a giraffe shape, the limo point would be on the neck so when the shape is resized, the neck will stretch and the rest of the shape will maintain its dimensions.
		/// </remarks>
		[Browsable(true), Category("Element Layout")]
        public VgVector2D Limo
        {
            get
            {
                return new VgVector2D((IVgVector2D) base.GetAttribute("limo"));
            }
            set
            {
                base.SetAttribute("limo", value.NativeVector);
            }
        }

        /// <summary>
        /// If set the path may be filled, if unset any fill specification on the path should be ignored.
        /// </summary>
        [Browsable(true), Category("Element Layout"), DefaultValue("")]
        public bool FillOk
        {
            get
            {
                return base.GetBooleanAttribute("fillok");
            }
            set
            {
                base.SetBooleanAttribute("fillok", value);
            }
        }

        /// <summary>
        /// If set the path may be stroked, if unset any stroke specification on the path should be ignored.
        /// </summary>
        [Browsable(true), Category("Element Layout"), DefaultValue("")]
        public bool StrokeOk
        {
            get
            {
                return base.GetBooleanAttribute("strokeok");
            }
            set
            {
                base.SetBooleanAttribute("strokeok", value);
            }
        }
        /// <summary>
        /// If set a shadow path may be created from the path, if unset any shadow specification should be ignored.
        /// </summary>
        [Browsable(true), Category("Element Layout"), DefaultValue("")]
        public bool ShadowOk
        {
            get
            {
                return base.GetBooleanAttribute("shadowok");
            }
            set
            {
                base.SetBooleanAttribute("shadowok", value);
            }
        }


        /// <summary>
        /// If set arrowheads may be added to the ends of the path, if unset any arrowheads specified in the stroke element should be ignored.
        /// </summary>
        [Browsable(true), Category("Element Layout"), DefaultValue("")]
        public bool ArrowOk
        {
            get
            {
                return base.GetBooleanAttribute("arrowok");
            }
            set
            {
                base.SetBooleanAttribute("arrowok", value);
            }
        }

        /// <summary>
        /// If set a gradient fill can be produced by repeated drawing of scaled versions of the path - this must only be set if it is possible to scale the path in such a way that a fill is always contained in the original path.  This controls the interpretation of the fill element type="gradientradial" attribute setting.
        /// </summary>
        [Browsable(true), Category("Element Layout"), DefaultValue("")]
        public bool GradientshapeOk
        {
            get
            {
                return base.GetBooleanAttribute("gradientshapeok");
            }
            set
            {
                base.SetBooleanAttribute("gradientshapeok", value);
            }
        }

        /// <summary>
        /// If set this indicates that the path is an appropriate warping path for the textpath element.  If not set the textpath element must be ignored.  Normally textpath paths are not useful unless they are associated with a textpath element.
        /// </summary>
        [Browsable(true), Category("Element Layout"), DefaultValue("")]
        public bool TextpathOk
        {
            get
            {
                return base.GetBooleanAttribute("textpathok");
            }
            set
            {
                base.SetBooleanAttribute("textpathok", value);
            }
        }

        /// <summary>
        /// A string of the form "L1,T1,R1,B1; L2,T2,R2,B2;…" If the string is null, then the textbox is set equal to the geometry box. In practice 1, 2, 3 or 6 text rectangles may be specified. Detail on how more than one rect is used, is specified elsewhere. The left, top, right, or bottom values can be a reference to a formula in the form @number where number is the formula’s ordinal number.  The default is the same as the containing block.
        /// </summary>
        [Browsable(true), Category("Element Layout"), DefaultValue("")]
        public string TextboxRect
        {
            get
            {
                return base.GetStringAttribute("textboxrect");
            }
            set
            {
                base.SetStringAttribute("textboxrect", value);
            }
        }

        
		public PathElement(IHtmlEditor editor) : base("v:path", editor)
		{
		}

        internal PathElement(Interop.IHTMLGenericElement peer, IHtmlEditor editor) : base ((Interop.IHTMLElement)peer, editor)
        {
        }

	}
}
