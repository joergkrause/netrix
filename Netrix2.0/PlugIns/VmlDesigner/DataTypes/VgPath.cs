using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Text.RegularExpressions;
using Comzept.Genesis.NetRix.VgxDraw;

namespace GuruComponents.Netrix.VmlDesigner.DataTypes
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
	public sealed class VgPath	
	{

		private IVgPath nativePath;

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
        ///         <item>m</item><item>moveto</item><item>2</item><item>Start a new subpath at the given x,y-coordinate.</item>
        ///     </term>
        ///     <term>
        ///         <item>l</item><item>lineto</item><item>2*</item><item>Draw a line from the current point to the given x,y-coordinate, which becomes the new current point. Additional coordinate pairs may be specified to form a polyline, e.g., "l 10,13,45,27,89,-12". </item>
        ///     </term>
        ///     <term>
        ///         <item>c</item><item>curveto</item><item>6*</item><item>Draw a cubic bezier curve from the current point to the coordinate given by the final two parameters, the control points given by the first four parameters. The current point becomes the endpoint of the bezier.</item>
        ///     </term>
        ///     <term>
        ///         <item>x</item><item>close</item><item>0</item><item>Close the current subpath by drawing a straight line from the current point to the original moveto point.</item>
        ///     </term>
        ///     <term>
        ///         <item>e</item><item>end</item><item>0</item><item>End the current set of subpaths. A given set of subpaths (as delimited by end) are filled using eofill. Subsequent sets of subpaths are filled independently and superimposed on existing ones.</item>
        ///     </term>
        ///     <term>
        ///         <item>t</item><item>rmoveto</item><item>2*</item><item>Start a new subpath at the relative coordinates (cpx + x, cpy + y) where cpx, cpy is the current position.</item>
        ///     </term>
        ///     <term>
        ///         <item>r</item><item>rlineto</item><item>2*</item><item>Draw a line from the current point to the relative coordinate (cpx + x, cpy + y). If additional coordinate pairs are given, each new point is computed relative to the last one.</item>
        ///     </term>
        ///     <term>
        ///         <item>v</item><item>rcurveto</item><item>6*</item><item>Cubic bezier curve using the given coordinates relative to the current point. All the points are computed relative to the same starting point. </item>
        ///     </term>
        ///     <term>
        ///         <item>nf</item><item>nofill</item><item>0</item><item>The current set of subpaths (delimited by end) will not be filled.item>
        ///     </term>
        ///     <term>
        ///         <item>ns</item><item>nostroke</item><item>0</item><item>The current set of subpaths (delimited by end) will not be stroked. </item>
        ///     </term>
        ///     <term>
        ///         <item>ae</item><item>angleellipseto</item><item>6*</item><item>Draw a segment of an ellipse. A straight line is drawn from the current point to the start point of the segment.</item>
        ///     </term>
        ///     <term>
        ///         <item>al</item><item>angleellipse</item><item>6*</item><item>Same as ae except that there is an implied m to the starting point of the segment.</item>
        ///     </term>
        ///     <term>
        ///         <item>at</item><item>arcto</item><item>8*</item><item>The first four values define the bounding box of an ellipse. The last four define two radial vectors. A segment of the ellipse is drawn that starts at the angle defined by the start radius vector and ends at the angle defined by the end vector. A straight line is drawn from the current point to the start of the arc. The arc is always drawn in a counterclockwise direction.</item>
        ///     </term>
        ///     <term>
        ///         <item>ar</item><item>arc</item><item>8*</item><item>Same as at. However, a new subpath is started by an implied m to the start point of the arc.</item>
        ///     </term>
        ///     <term>
        ///         <item>wa</item><item>clockwisearcto</item><item>8*</item><item>Same as at but the arc is drawn in a clockwise direction.</item>
        ///     </term>
        ///     <term>
        ///         <item>wr</item><item>clockwisearc</item><item>8*</item><item>Same as ar but is drawn in a clockwise direction.</item>
        ///     </term>
        ///     <term>
        ///         <item>qx</item><item>ellipticalqaudrantx</item><item>2*</item><item>A quarter ellipse is drawn from the current point to the given endpoint. The elliptical segment is initially tangential to a line parallel to the x-axis; i.e., the segment starts out horizontal.</item>
        ///     </term>
        ///     <term>
        ///         <item>qy</item><item>ellipticalquadranty</item><item>2*</item><item>Same as qx except that the elliptical segment is initially tangential to a line parallel to the y-axis; i.e., the segment starts out vertical.</item>
        ///     </term>
        ///     <term>
        ///         <item>qb</item><item>quadraticbezier</item><item>2*</item><item>Defines one or more quadratic bezier curves by means of control points and an endpoint. Intermediate (on-curve) points are obtained by interpolation between successive control points similar to TrueType fonts. The subpath need not be a start, in which case the subpath is closed and the last point defines the start point.</item>
        ///     </term>
        /// </list>
        /// </remarks>
        [Browsable(true), Category("Element Layout"), DefaultValue("")]
        public string V
        {
            get
            {
				try
				{
					return nativePath.v;
				}
				catch
				{
					return "No Path defined";
				}
            }
            set
            {
                nativePath.v = value;
            }
        }

        private readonly Regex rx = new Regex(@"[ml,](-?\d+)[,](-?\d+)", RegexOptions.Compiled);

        /// <summary>
        /// Gets the Path information as point array.
        /// </summary>
        /// <remarks>
        /// This property returns the absolute path information, like the <see cref="System.Drawing.FillPath">FillPath</see>
        /// method requires. The path information is recalculated dynamically according the current <see cref="V"/> and
        /// <see cref="Coords"/> attributes.
        /// <para>
        /// The property does not appear in PropertyGrid.
        /// </para>
        /// <example>
        /// You can use this property to create special drawings based on the path. The following example assumes you have
        /// a shape, which has a textbox subelement. A binary behavior can be attached and within the draw method the behavior
        /// is supposed to draw a filled shadow, which follows the path. The "element" field has to be provided be the calling code.
        /// In a binary behavior you can get this by calling the method GetElement that way:
        /// <code>
        /// IElement element = base.GetElement(editor);
        /// </code>
        /// Now we need to assure we have some usings:
        /// <code>
        /// using System.Drawing;
        /// using System.Drawing.Drawing2D;
        /// using GuruComponents.Netrix;
        /// using GuruComponents.Netrix.WebEditing.Elements;
        /// using GuruComponents.Netrix.ComInterop.Interop;
        /// using GuruComponents.Netrix.VmlDesigner.Elements;
        /// </code>
        /// This part appears in the Draw method the binary behavior provides:
        /// <code>
        /// // Create a GDI graphic path (choose different fillmode if required)
        /// GraphicsPath path = new GraphicsPath(FillMode.Winding);
        /// // Get the shape element the behavior is attached to
        /// ShapeElement shape = (ShapeElement) element;
        /// // Get the path
        /// Point[] f1 = shape.Path.AbsoluteVPath;
        /// // copy the path to the GDI path 
        /// path.AddLines(shape.Path.AbsoluteVPath);                    
        /// // Get the bounding rectangle of the inner textbox element IHTMLRect and IHTMLElement2 are from Interop class
        /// IHTMLRect rr = ((IHTMLElement2)shape.GetChild(0).GetBaseElement()).GetBoundingClientRect();
        /// // Create a matrix to transform the coordinates to the inner rectangle, assuming the coordinates of the 
        /// // shape are defined as 200,200
        /// Matrix matrix = new Matrix(
        ///     new Rectangle(0, 0, 200, 200),
        ///     new Point[] {
        ///         new Point(rr.left, rr.top),
        ///         new Point(rr.right, rr.top),
        ///         new Point(rr.left, rr.bottom)
        ///     }
        /// );
        /// // transform the output
        /// gr.Transform = matrix;
        /// // Fill the path, so the area we draw fits exactly the shape
        /// gr.FillPath(FillBrush, path);
        /// </code>
        /// If subsequent drawings being made the matrix should be removed from the Graphics object.
        /// <para>
        /// Instead of using the Interop class directly the element classes provide the necessary information too. However,
        /// in time critical environments this could have some performance pitfalls. Therefore, passing the wrapper and accessing 
        /// the data directly is sometimes a good idea.
        /// </para>
        /// </example>
        /// </remarks>
        [Browsable(false)]
        public Point[] AbsoluteVPath
        {
            get
            {
                string v = nativePath.v;
                MatchCollection m = rx.Matches(v);
                ArrayList a = new ArrayList(m.Count);
                foreach (Match match in m)
                {
                    if (match.Groups.Count != 3) continue; // strip out invalid points
                    a.Add(new Point(Convert.ToInt32(match.Groups[1].Value), Convert.ToInt32(match.Groups[2].Value)));
                }
                Point[] points = new Point[a.Count];
                a.CopyTo(points);
                return points;
            }
        }

        /// <summary>
        /// A point along the x and y dimensions of a shape where the shape will limo stretch.
        /// </summary>
        /// <remarks>
        /// Defines the point where the shape is stretched; e.g., for a giraffe shape, the limo point would be on the neck so when the shape is resized, the neck will stretch and the rest of the shape will maintain its dimensions.
        /// </remarks>
        [Browsable(true), Category("Element Layout")]
        [TypeConverter(typeof(ExpandableObjectConverter))]
		public VgVector2D Limo
        {
            get
            {
                return new VgVector2D(nativePath.limo);
            }
        }

        /// <summary>
        /// If set the path may be filled, if unset any fill specification on the path should be ignored.
        /// </summary>
        [Browsable(true), Category("Element Layout"), DefaultValue("")]
        public TriState FillOk
        {
            get
            {
                return (TriState)nativePath.fillok;
            }
            set
            {
                nativePath.fillok = (VgTriState)(((int)value == 1) ? -1 : (int)value);;
            }
        }

        /// <summary>
        /// If set the path may be stroked, if unset any stroke specification on the path should be ignored.
        /// </summary>
        [Browsable(true), Category("Element Layout"), DefaultValue("")]
        public TriState StrokeOk
        {
            get
            {
                return (TriState) nativePath.strokeok;
            }
            set
            {
                nativePath.strokeok = (VgTriState)(((int)value == 1) ? -1 : (int)value);;
            }
        }
        /// <summary>
        /// If set a shadow path may be created from the path, if unset any shadow specification should be ignored.
        /// </summary>
        [Browsable(true), Category("Element Layout"), DefaultValue("")]
        public TriState ShadowOk
        {
            get
            {
                return (TriState) nativePath.shadowok;
            }
            set
            {
                nativePath.shadowok = (VgTriState)(((int)value == 1) ? -1 : (int)value);;
            }
        }


        /// <summary>
        /// If set arrowheads may be added to the ends of the path, if unset any arrowheads specified in the stroke element should be ignored.
        /// </summary>
        [Browsable(true), Category("Element Layout"), DefaultValue("")]
        public TriState ArrowOk
        {
            get
            {
                return (TriState) nativePath.arrowok;
            }
            set
            {
                nativePath.arrowok = (VgTriState)(((int)value == 1) ? -1 : (int)value);;
            }
        }

        /// <summary>
        /// If set a gradient fill can be produced by repeated drawing of scaled versions of the path - this must only be set if it is possible to scale the path in such a way that a fill is always contained in the original path.  This controls the interpretation of the fill element type="gradientradial" attribute setting.
        /// </summary>
        [Browsable(true), Category("Element Layout"), DefaultValue("")]
        public TriState GradientshapeOk
        {
            get
            {
                return (TriState) nativePath.gradientshapeok;
            }
            set
            {
                nativePath.gradientshapeok = (VgTriState)(((int)value == 1) ? -1 : (int)value);;
            }
        }

        /// <summary>
        /// If set this indicates that the path is an appropriate warping path for the textpath element.  If not set the textpath element must be ignored.  Normally textpath paths are not useful unless they are associated with a textpath element.
        /// </summary>
        [Browsable(true), Category("Element Layout"), DefaultValue("")]
        public TriState TextpathOk
        {
            get
            {
                return (TriState) nativePath.textpathok;
            }
            set
            {
                nativePath.textpathok = (VgTriState)(((int)value == 1) ? -1 : (int)value);;
            }
        }

        /// <summary>
        /// A string of the form "L1,T1,R1,B1; L2,T2,R2,B2;…" If the string is null, then the textbox is set equal to the geometry box. In practice 1, 2, 3 or 6 text rectangles may be specified. Detail on how more than one rect is used, is specified elsewhere. The left, top, right, or bottom values can be a reference to a formula in the form @number where number is the formula’s ordinal number.  The default is the same as the containing block.
        /// </summary>
        [Browsable(true), Category("Element Layout"), DefaultValue("")]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		public VgFixedRectangleArray TextboxRect
        {
            get
            {
                return new VgFixedRectangleArray(nativePath.textboxrect);
            }
        }

        
		internal VgPath(IVgPath path)
		{
			nativePath = path;
		}

        internal VgPath()
		{
			nativePath = null;
		}

		public override string ToString()
		{
			return "Path Properties";
		}

	
	}
}
