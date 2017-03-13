using System;
using System.ComponentModel;
using System.Web.UI.WebControls;

using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.Designer;
using GuruComponents.Netrix.Events;
using GuruComponents.Netrix.WebEditing.Elements;
using GuruComponents.Netrix.WebEditing.Styles;
using System.Collections.Specialized;
using System.Collections;
using System.ComponentModel.Design;
using System.Drawing;
using System.Xml;
# if EDX
using GuruComponents.Netrix.XmlDesigner.Edx;
# endif
#pragma warning disable 1591
namespace GuruComponents.Netrix.XmlDesigner
{
	/// <summary>
	/// The base XML Control class.
	/// </summary>
	/// <remarks>
	/// The purpose of this class is the definition of the design time behavior of XML elements.
	/// Basically it follows the strategy of ASP.NET webcontrols, but it neither depends on ASP.NET nor
	/// it needs any part of the ASP.NET infrastructure. Basically, any complete implementation consists
	/// of two parts: 
	/// <list type="list">
	///		<item>1. The element's class, inherited from XmlControl</item>
	///		<item>2. The element's designer class, inherited from <see cref="System.Web.UI.Design.ControlDesigner"/>.</item>
	/// </list>
	/// Whearas the element's class gives programmatic access to the elements properties, the designer creates
	/// the HTML which replaces the XML at design time. Both, base class and designer, can interact with the content
	/// using common events.
	/// <para>
	/// In a standard scenario the base class implements each attribute the XML supports as a property. This simplifies
	/// the usage of the PropertyGrid. But you're free to implement the attribute access in any way, though. XmlControl
	/// provides the methods <see cref="GetAttribute"/> and <see cref="SetAttribute"/> for easy access. It provides also
	/// the methods <see cref="GetStyleAttribute"/> and <see cref="SetStyleAttribute"/> as well, for easy access of the
	/// common <i>style</i> attribute. The class provides also public access to all (theoretically) supported events.
	/// Remember, that not all elements and there HTML representation can fire any kind of event the control provides.
	/// However, the common events like Click, DblCkick, Move, MouseXXX etc. are always supported.
	/// </para>
	/// <para>
	/// Implementing the designer is the main part of the work on the element's class. The conjunction between class and 
	/// designer is made by .NET attributes and custom attributes defined in XmlDesigner assembly:
	/// <code>
	/// using System.ComponentModel;
    /// // other usings go here
	/// 
	/// [DesignerAttribute(typeof(TracklinkDesigner), typeof(IDesigner))]        // Designer attached
    /// [Formatting(FormattingFlags.Xml)]                                        // Controls formatting in source
    /// [XmlElement("rn", "tracklink")]                                          // Defines appearance in source
    /// public class TracklinkControl : XmlControl
	/// {
	///		// your implementation goes here
    /// 
    ///     [DesignOnly(true)]                                                   // does not make this property persistent
    ///     [Browsable(false)]                                                   // property does not appear in PropertyGrid
    ///     public override string ID
    ///     {
    ///         get { return ""; }
    ///         set { }
    ///     }
	/// }
	/// </code>
	/// In that example the class <i>TracklinkDesigner</i> is the element specific design time behavior of the element's
	/// class <i>TracklickControl</i>. The designer needs to override at least one method <see cref="GetDesignTimeHtml"/>.
	/// This method is called by the XmlDesigner extender provider every time the control needs to render the XML. It's
	/// a quite good idea to make this method performaning as good as possible. The second important method to override
	/// is <see cref="Initialize"/>, which is called after the instantiation of the designer. The parameter contains a
	/// reference to the underlying element object (of type TracklinkControl in the example). This is the way the designer
	/// can control the properties of the base class. The designer can also turn on the verbs (or commands) area of the
	/// PropertyGrid. To activate it, override the <see cref="Verbs"/> property. The verbs contain delegates to methods
	/// within the designer. When they are fired, they can set properties of the underlying element object. The 
	/// <see cref="GetDesignTimeHtml"/> method should be always implemented interactive, to represent permantly the 
	/// current state of the element object. See example below for a very simple implementation of a designer.
	/// </para>
    /// <para>
    /// Using attributes one can control the way the element is written back to sources. With <c>[DesignOnly(true)]</c>
    /// decorated properties are not written back. To avoid defined properties from being written, just inherit from
    /// this class and override the particular property, add the attribute and the property is not being written as
    /// element's attribute.
    /// </para>
	/// </remarks>
	/// <example>
/// public class TracklinkDesigner : System.Web.UI.Design.ControlDesigner
///{        
///
///	protected TracklinkControl component;
///
///	public TracklinkDesigner() : base()
/// {
/// }
///
///	public override void Initialize(IComponent component)
/// {
///	 base.Initialize (component);
///	 this.component = (TracklinkControl) component;                        
/// }        
///
///	[Browsable(false)]
/// public new DataBindingCollection DataBindings
/// {
///	 get
///  {
///	   return null;
///  }
/// }
///
///	public override string GetDesignTimeHtml()
/// {            
///	  if (component.Visible)
///   {
///       return String.Format(@"&lt;div style=""{0}"" class=""{1}""&gt;{2}&lt;/div&gt;",
///       component.Style,
///       component.Class,
///       component.InnerHtml
///	       );
///    } 
///	   else 
///    {
///       return String.Empty;
///    }
/// }   
///    
///	public override System.ComponentModel.Design.DesignerVerbCollection Verbs
/// {
///	  get
///   {
///	     return base.Verbs;
///   }
/// }
///
///}
	/// </example>
	[Bindable(false), ToolboxItem(false)]
    public abstract class XmlControl : System.Web.UI.Control, IElement, ICustomTypeDescriptor
	{

        private Interop.IHTMLElement element;
        private Interop.IHTMLElementDefaults viewElementDefaults;

        [Browsable(false), DesignOnly(true)]
        internal Interop.IHTMLElementDefaults ViewElementDefaults
        {
            get { return viewElementDefaults; }
            set 
            { 
                viewElementDefaults = value;
            //    ContentEditable = true;
            }
        }

        private IHtmlEditor htmlEditor;
                
        private HybridDictionary behaviorCookie = new HybridDictionary(2);
        private bool ignoreCase = true;
        private bool designTimeOnly = false;
        private ElementBehavior eb;

        internal protected XmlControl(Interop.IHTMLElement peer, IHtmlEditor editor)
        {
            htmlEditor = editor;
            element = peer;
        }

        public virtual bool IsSelectable()
        {
            return false;
        }

        public Interop.IHTMLElement GetBaseElement()
        {
            return element;            
        }
      
        public string UniqueName
        {
            get { return Site.Name; }
        }

        [Browsable(false)]
        public virtual XmlDocument TransformXml
        {
            get
            {
                return null;
            }
        }

		/// <summary>
		/// Inserts an element at the specified location.
		/// </summary>
		/// <param name="method">Specifies where to insert the HTML element.</param>
		/// <param name="element">Element to be inserted adjacent to the object.</param>
		public void InsertAdjacentElement(InsertWhere method, IElement element)
		{
            InsertAdjacentHtml(method, element.OuterHtml);
		}

		/// <summary>
		/// Inserts the given HTML text into the element at the location.
		/// </summary>
		/// <param name="method"></param>
		/// <param name="html"></param>
		public void InsertAdjacentHtml(InsertWhere method, string html)
		{
            object[] pVars = new object[1] { null };
            element.GetAttribute(XmlElementDesigner.VIEWLINK_ATTRIB, 0, pVars);
            if (pVars[0] is ViewLink)
            {
                Interop.IHTMLElement el = (Interop.IHTMLElement)((ViewLink)pVars[0]).DesignTimeElement;
                el.InsertAdjacentHTML(method.ToString(), html);
            }
            else
            {
                element.InsertAdjacentHTML(method.ToString(), html);
            }
		}

# region ViewLink Defaults 

        private bool contectEditable = true;
        [Browsable(false), DesignOnly(true)]
        public bool ContentEditable
        {
            get
            {
                return contectEditable; // (viewElementDefaults.GetContentEditable().ToLower().Equals("true")) ? true : false;
            }
            set
            {
                contectEditable = value;
                //viewElementDefaults.SetContentEditable((value) ? "true" : "false");
            }
        }



# endregion

        /// <summary>
		/// Inserts the given text into the element at the specified location.
		/// </summary>
		/// <param name="method">Specifies where to insert the text.</param>
		/// <param name="text"></param>
		public void InsertAdjacentText(InsertWhere method, string text)
		{
			GetBaseElement().InsertAdjacentText(method.ToString(), text);
		}

        public System.Drawing.Rectangle GetAbsoluteArea()
        {
            throw new NotImplementedException();
        }

        private IExtendedProperties extendedProperties;

        /// <summary>
        /// Gives access to extended properties which do not have any relation to attributes.
        /// </summary>
        [Browsable(false)]
        public IExtendedProperties ExtendedProperties
        {
            get
            {
                if (extendedProperties == null)
                {
                    extendedProperties = new ExtendedProperties(element);
                }
                return extendedProperties;
            }
        }

        /// <summary>
        /// Makes the object visible or unvisible.
        /// </summary>
        /// <remarks>
        /// To use this property at design time one must set the global property 
        /// <see cref="GuruComponents.Netrix.HtmlEditor.RespectVisibility">RespectVisibility</see> to <c>true</c>,
        /// otherwise the element will still stay visible.
        /// </remarks>
        [Browsable(false)]
        public override bool Visible
        {
            get
            {
                return base.Visible; 
            }
            set
            {
                base.Visible = value;
                RuntimeStyle.visibility = (value) ? "visible" : "hidden";
            }
        }

        [Browsable(false), System.Web.UI.PersistenceModeAttribute(System.Web.UI.PersistenceMode.InnerProperty)]
        public override bool EnableViewState
        {
            get
            {
                return false;
            }
            set
            {
                throw new NotSupportedException();
            }
        }

        [Browsable(true), DesignOnly(true)]
        public override string ID
        {
            get
            {
                if (this.GetAttribute("id") != null)
                    return (string)this.GetAttribute("id");
                else
                    return String.Empty;
            }
        }

        /// <summary>
        /// Access to the HtmlEditor containing the element.
        /// </summary>
        [Browsable(false), DesignOnlyAttribute(true)]
        public GuruComponents.Netrix.IHtmlEditor HtmlEditor
        {
            get
            {                
                return htmlEditor; 
            }
            set
            {
                htmlEditor = value;
            }
        }

        #region IElement Member

        [Browsable(false), System.Web.UI.PersistenceModeAttribute(System.Web.UI.PersistenceMode.InnerProperty)]
        public string InnerXml
        {
            get
            {
                string xml = "";
                try
                {
                    xml = element.GetInnerHTML(); // TODO: Check valid content
                }
                catch
                {
                }
                return xml;
            }
            set
            {
                try
                {
                    element.SetInnerHTML(value);
                }
                catch
                {
                }
            }
        }


        [Browsable(false), System.Web.UI.PersistenceModeAttribute(System.Web.UI.PersistenceMode.InnerProperty)]
        public string InnerHtml
        {
            get
            {
                string html = "";
                try
                {
                    html = element.GetInnerHTML();
                }
                catch
                {
                }
                return html;
            }
            set
            {
                try
                {
                    //element.GetStyle().SetWidth("200px");
                    //element.GetStyle().SetHeight("200px");
                    //element.SetInnerHTML("");
                    //element.InsertAdjacentHTML("beforeEnd", value);
                    element.SetInnerHTML(value);
                }
                catch
                {
                }
            }
        }

        [Browsable(false), System.Web.UI.PersistenceModeAttribute(System.Web.UI.PersistenceMode.InnerProperty)]
        public string InnerText
        {
            get
            {
                return element.GetInnerText();
            }
            set
            {
                element.SetInnerText(value == null ? "" : value);
            }
        }

        IElementDom elementDom;
        
        [Browsable(false), System.Web.UI.PersistenceModeAttribute(System.Web.UI.PersistenceMode.InnerProperty)]
        public IElementDom ElementDom
        {
            get
            {
                if (elementDom == null)
                {
                    elementDom = new ElementDom(element as Interop.IHTMLDOMNode, htmlEditor);
                }
                return elementDom;
            }
        }

        /// <summary>
        /// Overridden to return tagname.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return TagName;
        }

        public ElementCollection GetChildren()
        {
            Interop.IHTMLElementCollection ec = element.GetChildren() as Interop.IHTMLElementCollection;
            ElementCollection newEc = new ElementCollection();
            if (ec != null)
            {
                for (int i = 0; i < ec.GetLength(); i++)
                {
                    Interop.IHTMLElement el = (Interop.IHTMLElement) ec.Item(i, i);                   
                    newEc.Add(htmlEditor.GenericElementFactory.CreateElement(el));
                }
            }
            return newEc;
        }

        /// <summary>
        /// Replace the complete style attribute's content with the fiven string.
        /// </summary>
        /// <param name="CssText"></param>
        public void SetStyle(string CssText)
        {
            element.GetStyle().SetCssText(CssText);
        }

        /// <summary>
        /// Returns itself.
        /// </summary>
        [Browsable(false), DesignOnlyAttribute(true)]
        public IElement TagElement
        {
            get
            {
                return this;
            }
        }

        /// <summary>
        /// Access the parent element within the document hierarchie.
        /// </summary>
        /// <returns></returns>
        virtual public System.Web.UI.Control GetParent()
        {
            Interop.IHTMLElement parent = element.GetParentElement();
            if (parent != null)
            {
                return htmlEditor.GenericElementFactory.CreateElement(parent);
            } 
            else 
            {
                return null;
            }
        }

        virtual public string GetStyle()
        {
            string css = element.GetStyle().GetCssText();
            return css;
        }

        virtual public System.Web.UI.Control GetChild(string name)
        {
            try
            {
                Interop.IHTMLElement el = (Interop.IHTMLElement)((Interop.IHTMLElementCollection)element.GetChildren()).Item(name, null);
                return htmlEditor.GenericElementFactory.CreateElement(el);
            }
            catch
            {
                return null;
            }
        }

        System.Web.UI.Control GuruComponents.Netrix.WebEditing.Elements.IElement.GetChild(int index)
        {
            try
            {
                Interop.IHTMLElement el = (Interop.IHTMLElement)((Interop.IHTMLElementCollection)element.GetChildren()).Item(null, index);
                return htmlEditor.GenericElementFactory.CreateElement(el);
            }
            catch
            {
                return null;
            }
        }
        
        /// <summary>
        /// Returns the HTML of element including the tag definition.
        /// </summary>
        [Browsable(false), DesignOnlyAttribute(true)]
        virtual public string OuterHtml
        {
            get
            {
                return element.GetOuterHTML();
            }
            set
            {
                element.SetOuterHTML(value);
            }
        }

        private CssEffectiveStyle effectiveStyle = null;
        private ElementStyle runtimeStyle = null;
        private ElementStyle currentStyle = null;

        /// <summary>
        /// Gets the current STYLE definition.
        /// </summary>
        /// <remarks>
        /// This property shows the effective style if this element as a cascade of the global
        /// and inline styles defined elsewhere. Readonly.
        /// <para>
        /// The property returns <c>null</c> (<c>Nothing</c> in VB.NET) if the effective style can not be retrieved.
        /// </para>
        /// </remarks>
        [Browsable(true)]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        [Category("Element Style")]
        [Description("The effective style as a cascaded combination of global, embedded and inline styles. Readonly.")]        
        [DisplayName("Effective Style")]
        public IEffectiveStyle EffectiveStyle
        {
            get
            {                   
                if (effectiveStyle == null) 
                {   
                    Interop.IHTMLCurrentStyle cs = ((Interop.IHTMLElement2)this.element).GetCurrentStyle() as Interop.IHTMLCurrentStyle;
                    effectiveStyle = new CssEffectiveStyle(cs);
                }
                return effectiveStyle;
            }
        }

        /// <summary>
        /// The runtime style provide access to additonal appearance information at runtime. Does not persist.
        /// </summary>
        /// <remarks>
        /// This property allows access to styles not being persistent within the document. They affect only at runtime
        /// and can change the current appearance of an object. One can use this to add specific effects during user
        /// operation of to customize elements in particular situations.
        /// <seealso cref="EffectiveStyle"/>
        /// <seealso cref="CurrentStyle"/>
        /// <seealso cref="SetStyleAttribute"/>
        /// <seealso cref="RemoveStyleAttribute"/>
        /// <seealso cref="GetStyleAttribute"/>
        /// </remarks>
        [Browsable(true)]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        [Category("Element Style")]
        [Description("The runtime style provide access to additonal appearance information at runtime. Does not persist.")]        
        [DisplayName("Runtime Style"), DesignOnly(true)]
        public IElementStyle RuntimeStyle
        {
            get
            {                
                if (runtimeStyle == null) 
                {   
                    Interop.IHTMLStyle cs = ((Interop.IHTMLElement2)this.GetBaseElement()).GetRuntimeStyle() as Interop.IHTMLStyle;
                    if (cs != null)
                    {
                        runtimeStyle = new ElementStyle(cs);
                    }
                }
                return runtimeStyle;
            }
        }

        /// <summary>
        /// Access to the style attribute in an object form.
        /// </summary>
        /// <seealso cref="EffectiveStyle"/>
        /// <seealso cref="RuntimeStyle"/>
        /// <seealso cref="SetStyleAttribute"/>
        /// <seealso cref="RemoveStyleAttribute"/>
        /// <seealso cref="GetStyleAttribute"/>
        [Browsable(true)]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        [Category("Element Style")]
        [Description("Access to the style attribute in an object form.")]        
        [DisplayName("CSS Style"), DesignOnly(true)]
        public IElementStyle CurrentStyle
        {
            get
            {            
                if (currentStyle == null) 
                {   
                    Interop.IHTMLStyle cs = ((Interop.IHTMLElement)this.GetBaseElement()).GetStyle() as Interop.IHTMLStyle;
                    if (cs != null)
                    {
                        currentStyle = new ElementStyle(cs);
                    }
                }
                return currentStyle;
            }
        }

        #endregion

        # region internal

        public object GetAttribute(string attribute)
        {
            if (element == null)
            {
                return null;
            }
            object[] locals1 = new object[1];
            try
            {
                element.GetAttribute(attribute, !ignoreCase ? 1 : 0, locals1);
                object local = locals1[0];
                return local;
            }
            catch
            {
                return null;
            }
        }

        public virtual Unit Width
        {
            get
            {
                object s = GetExactStyle().GetWidth();
                if (s == null || s.ToString().Equals("auto")) 
                    return Unit.Empty;
                else
                    return Unit.Parse(s.ToString());
            }
            set
            {
                SetStyleAttribute("width", value.ToString());
            }
        }


        public virtual Unit Height
        {
            get
            {
                object s = GetExactStyle().GetHeight();
                if (s == null || s.ToString().Equals("auto"))
                    return Unit.Empty;
                else
                    return Unit.Parse(s.ToString());
            }
            set
            {
                SetStyleAttribute("height", value.ToString());                
            }
        }

        public string GetStyleAttribute(string attribute)
        {
            if (element != null)
            {
                Interop.IHTMLStyle interop_IHTMLStyle = null;
                if (interop_IHTMLStyle != null)
                {
                    try
                    {
                        object local = interop_IHTMLStyle.GetAttribute(attribute, !ignoreCase ? 1 : 0);
                        if (local == null)
                            return "";
                        else
                            return local.ToString();
                    }
                    catch
                    {
                        return null;
                    }
                }
            }
            return null;
        }

        private Interop.IHTMLStyle GetExactStyle()
        {
            if (designTimeOnly)
            {
                return ((Interop.IHTMLElement2)element).GetRuntimeStyle();
            }
            else
            {
                return element.GetStyle();
            }

        }

        public void RemoveAttribute(string attribute)
        {
            if (element != null)
            {
                try
                {
                    element.RemoveAttribute(attribute, !ignoreCase ? 1 : 0);
                }
                catch
                {
                }
            }
        }

        public void RemoveStyleAttribute(string attribute)
        {
            if (element != null)
            {
                Interop.IHTMLStyle interop_IHTMLStyle = null;
                if (designTimeOnly)
                {
                    interop_IHTMLStyle = ((Interop.IHTMLElement2)element).GetRuntimeStyle();
                }
                else
                {
                    interop_IHTMLStyle = element.GetStyle();
                }
                if (interop_IHTMLStyle != null)
                {
                    try
                    {
                        interop_IHTMLStyle.RemoveAttribute(attribute, !ignoreCase ? 1 : 0);
                    }
                    catch
                    {
                    }
                }
            }
        }

        public void SetAttribute(string attribute, object val)
        {
            if (element != null)
            {
                try
                {
                    element.SetAttribute(attribute, val, !ignoreCase ? 1 : 0);
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs(attribute));
                    }
                }
                catch
                {
                }
                
            }
        }

        public void SetStyleAttribute(string attribute, string val)
        {
            if (element != null)
            {
                Interop.IHTMLStyle interop_IHTMLStyle = null;
                if (designTimeOnly)
                {
                    interop_IHTMLStyle = ((Interop.IHTMLElement2)element).GetRuntimeStyle();
                }
                else
                {
                    interop_IHTMLStyle = element.GetStyle();
                }
                if (interop_IHTMLStyle != null)
                {
                    try
                    {
                        interop_IHTMLStyle.SetAttribute(attribute, val, !ignoreCase ? 1 : 0);
                        if (PropertyChanged != null)
                        {
                            PropertyChanged(this, new PropertyChangedEventArgs("style"));
                        }
                    }
                    catch
                    {
                    }
                }
            }
        }

        # endregion

        # region Control Events

        public event PropertyChangedEventHandler PropertyChanged;

        # endregion

        # region Internal Events

        /// <summary>
        /// 
        /// </summary>
        virtual public event DocumentEventHandler LoseCapture;
        /// <summary>
        /// 
        /// </summary>
        internal protected void OnLoseCapture(DocumentEventArgs e) { if (LoseCapture != null) { LoseCapture(this, e); } }
        /// <summary>
        /// 
        /// </summary>
        virtual public event DocumentEventHandler Click;
        /// <summary>
        /// 
        /// </summary>
        internal protected void OnClick(DocumentEventArgs e) { if (Click != null) { Click(this, e); } }
        /// <summary>
        /// 
        /// </summary>
        virtual public event DocumentEventHandler DblClick;
        /// <summary>
        /// 
        /// </summary>
        internal protected void OnDblClick(DocumentEventArgs e) { if (DblClick != null) { DblClick(this, e); } }
        /// <summary>
        /// 
        /// </summary>
        virtual public event DocumentEventHandler DragStart;
        /// <summary>
        /// 
        /// </summary>
        internal protected void OnDragStart(DocumentEventArgs e) { if (DragStart != null) { DragStart(this, e); } }
        /// <summary>
        /// 
        /// </summary>
        virtual new public event DocumentEventHandler Focus;
        /// <summary>
        /// 
        /// </summary>
        internal protected void OnFocus(DocumentEventArgs e) { if (Focus != null) { Focus(this, e); } }
        /// <summary>
        /// 
        /// </summary>
        virtual public event DocumentEventHandler Drop;
        /// <summary>
        /// 
        /// </summary>
        internal protected void OnDrop(DocumentEventArgs e) { if (Drop != null) { Drop(this, e); } }
        /// <summary>
        /// 
        /// </summary>
        virtual public event DocumentEventHandler Blur;
        /// <summary>
        /// 
        /// </summary>
        internal protected void OnBlur(DocumentEventArgs e) { if (Blur != null) { Blur(this, e); } }
        /// <summary>
        /// 
        /// </summary>
        virtual public event DocumentEventHandler DragOver;
        /// <summary>
        /// 
        /// </summary>
        internal protected void OnDragOver(DocumentEventArgs e) { if (DragOver != null) { DragOver(this, e); } }
        /// <summary>
        /// 
        /// </summary>
        virtual public event DocumentEventHandler DragEnter;
        /// <summary>
        /// 
        /// </summary>
        internal protected void OnDragEnter(DocumentEventArgs e) { if (DragEnter != null) { DragEnter(this, e); } }
        /// <summary>
        /// 
        /// </summary>
        virtual public event DocumentEventHandler DragLeave;
        /// <summary>
        /// 
        /// </summary>
        internal protected void OnDragLeave(DocumentEventArgs e) { if (DragLeave != null) { DragLeave(this, e); } }
        /// <summary>
        /// 
        /// </summary>
        virtual public event DocumentEventHandler KeyDown;
        /// <summary>
        /// 
        /// </summary>
        internal protected void OnKeyDown(DocumentEventArgs e) { if (KeyDown != null) { KeyDown(this, e); } }
        /// <summary>
        /// 
        /// </summary>
        virtual public event DocumentEventHandler KeyPress;
        /// <summary>
        /// 
        /// </summary>
        internal protected void OnKeyPress(DocumentEventArgs e) { if (KeyPress != null) { KeyPress(this, e); } }
        /// <summary>
        /// 
        /// </summary>
        virtual public event DocumentEventHandler KeyUp;
        /// <summary>
        /// 
        /// </summary>
        internal protected void OnKeyUp(DocumentEventArgs e) { if (KeyUp != null) { KeyUp(this, e); } }
        /// <summary>
        /// 
        /// </summary>
        virtual public event DocumentEventHandler MouseDown;
        /// <summary>
        /// 
        /// </summary>
        internal protected void OnMouseDown(DocumentEventArgs e) { if (MouseDown != null) { MouseDown(this, e); } }
        /// <summary>
        /// 
        /// </summary>
        virtual public event DocumentEventHandler ResizeStart;
        /// <summary>
        /// 
        /// </summary>
        internal protected void OnResizeStart(DocumentEventArgs e) { if (ResizeStart != null) { ResizeStart(this, e); } }
        /// <summary>
        /// 
        /// </summary>
        virtual public event DocumentEventHandler ResizeEnd;
        /// <summary>
        /// 
        /// </summary>
        internal protected void OnResizeEnd(DocumentEventArgs e) { if (ResizeEnd != null) { ResizeEnd(this, e); } }
        /// <summary>
        /// 
        /// </summary>
        virtual public event DocumentEventHandler MouseEnter;
        /// <summary>
        /// 
        /// </summary>
        internal protected void OnMouseEnter(DocumentEventArgs e) { if (MouseEnter != null) { MouseEnter(this, e); } }
        /// <summary>
        /// 
        /// </summary>
        virtual public event DocumentEventHandler MouseLeave;
        /// <summary>
        /// 
        /// </summary>
        internal protected void OnMouseLeave(DocumentEventArgs e) { if (MouseLeave != null) { MouseLeave(this, e); } }
        /// <summary>
        /// 
        /// </summary>
        virtual public event DocumentEventHandler MouseMove;
        /// <summary>
        /// 
        /// </summary>
        internal protected void OnMouseMove(DocumentEventArgs e) { if (MouseMove != null) { MouseMove(this, e); } }
        /// <summary>
        /// 
        /// </summary>
        virtual public event DocumentEventHandler MouseOut;
        /// <summary>
        /// 
        /// </summary>
        internal protected void OnMouseOut(DocumentEventArgs e) { if (MouseOut != null) { MouseOut(this, e); } }
        /// <summary>
        /// 
        /// </summary>
        virtual public event DocumentEventHandler MouseOver;
        /// <summary>
        /// 
        /// </summary>
        internal protected void OnMouseOver(DocumentEventArgs e) { if (MouseOver != null) { MouseOver(this, e); } }
        /// <summary>
        /// 
        /// </summary>
        virtual public event DocumentEventHandler MouseUp;
        /// <summary>
        /// 
        /// </summary>
        internal protected void OnMouseUp(DocumentEventArgs e) { if (MouseUp != null) { MouseUp(this, e); } }
        /// <summary>
        /// 
        /// </summary>
        virtual public event DocumentEventHandler SelectStart;
        /// <summary>
        /// 
        /// </summary>
        internal protected void OnSelectStart(DocumentEventArgs e) { if (SelectStart != null) { SelectStart(this, e); } }
        /// <summary>
        /// 
        /// </summary>
        virtual public event DocumentEventHandler LayoutComplete;
        /// <summary>
        /// 
        /// </summary>
        internal protected void OnLayoutComplete(DocumentEventArgs e) { if (LayoutComplete != null) { LayoutComplete(this, e); } }
        /// <summary>
        /// 
        /// </summary>
        public new event DocumentEventHandler Load;
        /// <summary>
        /// 
        /// </summary>
        internal protected void OnLoad(DocumentEventArgs e) { if (Load != null) { Load(this, e); } }
        /// <summary>
        /// 
        /// </summary>
        virtual public event DocumentEventHandler MouseWheel;
        /// <summary>
        /// 
        /// </summary>
        internal protected void OnMouseWheel(DocumentEventArgs e) { if (MouseWheel != null) { MouseWheel(this, e); } }
        /// <summary>
        /// 
        /// </summary>
        virtual public event DocumentEventHandler MoveEnd;
        /// <summary>
        /// 
        /// </summary>
        internal protected void OnMoveEnd(DocumentEventArgs e) { if (MoveEnd != null) { MoveEnd(this, e); } }
        /// <summary>
        /// 
        /// </summary>
        virtual public event DocumentEventHandler MoveStart;
        /// <summary>
        /// 
        /// </summary>
        internal protected void OnMoveStart(DocumentEventArgs e) { if (MoveStart != null) { MoveStart(this, e); } }
        /// <summary>
        /// 
        /// </summary>
        virtual public event DocumentEventHandler Activate;
        /// <summary>
        /// 
        /// </summary>
        internal protected void OnActivate(DocumentEventArgs e) { if (Activate != null) { Activate(this, e); } }
        /// <summary>
        /// 
        /// </summary>
        virtual public event DocumentEventHandler BeforeActivate;
        /// <summary>
        /// 
        /// </summary>
        internal protected void OnBeforeActivate(DocumentEventArgs e) { if (BeforeActivate != null) { BeforeActivate(this, e); } }
        /// <summary>
        /// 
        /// </summary>
        virtual public event DocumentEventHandler BeforeCopy;
        /// <summary>
        /// 
        /// </summary>
        internal protected void OnBeforeCopy(DocumentEventArgs e) { if (BeforeCopy != null) { BeforeCopy(this, e); } }
        /// <summary>
        /// 
        /// </summary>
        virtual public event DocumentEventHandler BeforeCut;
        /// <summary>
        /// 
        /// </summary>
        internal protected void OnBeforeCut(DocumentEventArgs e) { if (BeforeCut != null) { BeforeCut(this, e); } }
        /// <summary>
        /// 
        /// </summary>
        virtual public event DocumentEventHandler BeforePaste;
        /// <summary>
        /// 
        /// </summary>
        internal protected void OnBeforePaste(DocumentEventArgs e) { if (BeforePaste != null) { BeforePaste(this, e); } }
        /// <summary>
        /// 
        /// </summary>
        virtual public event DocumentEventHandler ContextMenu;
        /// <summary>
        /// 
        /// </summary>
        internal protected void OnContextMenu(DocumentEventArgs e) { if (ContextMenu != null) { ContextMenu(this, e); } }
        /// <summary>
        /// 
        /// </summary>
        virtual public event DocumentEventHandler Copy;
        /// <summary>
        /// 
        /// </summary>
        internal protected void OnCopy(DocumentEventArgs e) { if (Copy != null) { Copy(this, e); } }
        /// <summary>
        /// 
        /// </summary>
        virtual public event DocumentEventHandler Cut;
        /// <summary>
        /// 
        /// </summary>
        internal protected void OnCut(DocumentEventArgs e) { if (Cut != null) { Cut(this, e); } }
        /// <summary>
        /// 
        /// </summary>
        virtual public event DocumentEventHandler Deactivate;
        /// <summary>
        /// 
        /// </summary>
        internal protected void OnDeactivate(DocumentEventArgs e) { if (Deactivate != null) { Deactivate(this, e); } }
        /// <summary>
        /// 
        /// </summary>
        virtual public event DocumentEventHandler Drag;
        /// <summary>
        /// 
        /// </summary>
        internal protected void OnDrag(DocumentEventArgs e) { if (Drag != null) { Drag(this, e); } }
        /// <summary>
        /// 
        /// </summary>
        virtual public event DocumentEventHandler DragEnd;
        /// <summary>
        /// 
        /// </summary>
        internal protected void OnDragEnd(DocumentEventArgs e) { if (DragEnd != null) { DragEnd(this, e); } }
        /// <summary>
        /// 
        /// </summary>
        virtual public event DocumentEventHandler FocusIn;
        /// <summary>
        /// 
        /// </summary>
        internal protected void OnFocusIn(DocumentEventArgs e) { if (FocusIn != null) { FocusIn(this, e); } }
        /// <summary>
        /// 
        /// </summary>
        virtual public event DocumentEventHandler FocusOut;
        /// <summary>
        /// 
        /// </summary>
        internal protected void OnFocusOut(DocumentEventArgs e) { if (FocusOut != null) { FocusOut(this, e); } }
        /// <summary>
        /// 
        /// </summary>
        virtual public event DocumentEventHandler FilterChange;
        /// <summary>
        /// 
        /// </summary>
        internal protected void OnFilterChange(DocumentEventArgs e) { if (FilterChange != null) { FilterChange(this, e); } }
        /// <summary>
        /// 
        /// </summary>
        virtual public event DocumentEventHandler Abort;
        /// <summary>
        /// 
        /// </summary>
        internal protected void OnAbort(DocumentEventArgs e) { if (Abort != null) { Abort(this, e); } }
        /// <summary>
        /// 
        /// </summary>
        virtual public event DocumentEventHandler Change;
        /// <summary>
        /// 
        /// </summary>
        internal protected void OnChange(DocumentEventArgs e) { if (Change != null) { Change(this, e); } }
        /// <summary>
        /// 
        /// </summary>
        virtual public event DocumentEventHandler Select;
        /// <summary>
        /// 
        /// </summary>
        internal protected void OnSelect(DocumentEventArgs e) { if (Select != null) { Select(this, e); } }
        /// <summary>
        /// 
        /// </summary>
        virtual public event DocumentEventHandler SelectionChange;
        /// <summary>
        /// 
        /// </summary>
        internal protected void OnSelectionChange(DocumentEventArgs e) { if (SelectionChange != null) { SelectionChange(this, e); } }
        /// <summary>
        /// 
        /// </summary>
        virtual public event DocumentEventHandler Stop;
        /// <summary>
        /// 
        /// </summary>
        internal protected void OnStop(DocumentEventArgs e) { if (Stop != null) { Stop(this, e); } }
        /// <summary>
        /// 
        /// </summary>
        virtual public event DocumentEventHandler BeforeDeactivate;
        /// <summary>
        /// 
        /// </summary>
        internal protected void OnBeforeDeactivate(DocumentEventArgs e) { if (BeforeDeactivate != null) { BeforeDeactivate(this, e); } }
        /// <summary>
        /// 
        /// </summary>
        virtual public event DocumentEventHandler ControlSelect;
        /// <summary>
        /// 
        /// </summary>
        internal protected void OnControlSelect(DocumentEventArgs e) { if (ControlSelect != null) { ControlSelect(this, e); } }
        /// <summary>
        /// 
        /// </summary>
        virtual public event DocumentEventHandler EditFocus;
        /// <summary>
        /// 
        /// </summary>
        internal protected void OnEditFocus(DocumentEventArgs e) { if (EditFocus != null) { EditFocus(this, e); } }
        /// <summary>
        /// 
        /// </summary>
        virtual public event DocumentEventHandler Error;
        /// <summary>
        /// 
        /// </summary>
        internal protected void OnError(DocumentEventArgs e) { if (Error != null) { Error(this, e); } }
        /// <summary>
        /// 
        /// </summary>
        virtual public event DocumentEventHandler Move;
        /// <summary>
        /// 
        /// </summary>
        internal protected void OnMove(DocumentEventArgs e) { if (Move != null) { Move(this, e); } }
        /// <summary>
        /// 
        /// </summary>
        virtual public event DocumentEventHandler Paste;
        /// <summary>
        /// 
        /// </summary>
        internal protected void OnPaste(DocumentEventArgs e) { if (Paste != null) { Paste(this, e); } }
        /// <summary>
        /// 
        /// </summary>
        virtual public event DocumentEventHandler PropertyChange;
        /// <summary>
        /// 
        /// </summary>
        internal protected void OnPropertyChange(DocumentEventArgs e) { if (PropertyChange != null) { PropertyChange(this, e); } }
        /// <summary>
        /// 
        /// </summary>
        virtual public event DocumentEventHandler Resize;
        /// <summary>
        /// 
        /// </summary>
        internal protected void OnResize(DocumentEventArgs e) { if (Resize != null) { Resize(this, e); } }
        /// <summary>
        /// 
        /// </summary>
        virtual public event DocumentEventHandler Scroll;
        /// <summary>
        /// 
        /// </summary>
        internal protected void OnScroll(DocumentEventArgs e) { if (Scroll != null) { Scroll(this, e); } }
        /// <summary>
        /// 
        /// </summary>
        virtual public event DocumentEventHandler Paged;
        /// <summary>
        /// 
        /// </summary>
        internal protected void OnPage(DocumentEventArgs e) { if (Paged != null) { Paged(this, e); } }

        internal void InvokeLoseCapture(Interop.IHTMLEventObj e) { if (LoseCapture != null) { OnLoseCapture(new  DocumentEventArgs(e, this)); } }
        internal void InvokeClick(Interop.IHTMLEventObj e) { if (Click != null) { OnClick(new  DocumentEventArgs(e, this)); } }
        internal void InvokeDblClick(Interop.IHTMLEventObj e) { if (DblClick != null) { OnDblClick(new  DocumentEventArgs(e, this)); } }
        internal void InvokeDragStart(Interop.IHTMLEventObj e) { if (DragStart != null) { OnDragStart(new  DocumentEventArgs(e, this)); } }
        internal void InvokeFocus(Interop.IHTMLEventObj e) { if (Focus != null) { OnFocus(new  DocumentEventArgs(e, this)); } }
        internal void InvokeDrop(Interop.IHTMLEventObj e) { if (Drop != null) { OnDrop(new  DocumentEventArgs(e, this)); } }
        internal void InvokeBlur(Interop.IHTMLEventObj e) { if (Blur != null) { OnBlur(new  DocumentEventArgs(e, this)); } }
        internal void InvokeDragOver(Interop.IHTMLEventObj e) { if (DragOver != null) { OnDragOver(new  DocumentEventArgs(e, this)); } }
        internal void InvokeDragEnter(Interop.IHTMLEventObj e) { if (DragEnter != null) { OnDragEnter(new  DocumentEventArgs(e, this)); } }
        internal void InvokeDragLeave(Interop.IHTMLEventObj e) { if (DragLeave != null) { OnDragLeave(new  DocumentEventArgs(e, this)); } }
        internal void InvokeKeyDown(Interop.IHTMLEventObj e) { if (KeyDown != null) { OnKeyDown(new  DocumentEventArgs(e, this)); } }
        internal void InvokeKeyPress(Interop.IHTMLEventObj e) { if (KeyPress != null) { OnKeyPress(new  DocumentEventArgs(e, this)); } }
        internal void InvokeKeyUp(Interop.IHTMLEventObj e) { if (KeyUp != null) { OnKeyUp(new  DocumentEventArgs(e, this)); } }
        internal void InvokeMouseDown(Interop.IHTMLEventObj e) { if (MouseDown != null) { OnMouseDown(new  DocumentEventArgs(e, this)); } }
        internal void InvokeResizeStart(Interop.IHTMLEventObj e) { if (ResizeStart != null) { OnResizeStart(new  DocumentEventArgs(e, this)); } }
        internal void InvokeResizeEnd(Interop.IHTMLEventObj e) { if (ResizeEnd != null) { OnResizeEnd(new  DocumentEventArgs(e, this)); } }
        internal void InvokeMouseEnter(Interop.IHTMLEventObj e) { if (MouseEnter != null) { OnMouseEnter(new  DocumentEventArgs(e, this)); } }
        internal void InvokeMouseLeave(Interop.IHTMLEventObj e) { if (MouseLeave != null) { OnMouseLeave(new  DocumentEventArgs(e, this)); } }
        internal void InvokeMouseMove(Interop.IHTMLEventObj e) { if (MouseMove != null) { OnMouseMove(new  DocumentEventArgs(e, this)); } }
        internal void InvokeMouseOut(Interop.IHTMLEventObj e) { if (MouseOut != null) { OnMouseOut(new  DocumentEventArgs(e, this)); } }
        internal void InvokeMouseOver(Interop.IHTMLEventObj e) { if (MouseOver != null) { OnMouseOver(new  DocumentEventArgs(e, this)); } }
        internal void InvokeMouseUp(Interop.IHTMLEventObj e) { if (MouseUp != null) { OnMouseUp(new  DocumentEventArgs(e, this)); } }
        internal void InvokeSelectStart(Interop.IHTMLEventObj e) { if (SelectStart != null) { OnSelectStart(new  DocumentEventArgs(e, this)); } }
        internal void InvokeLayoutComplete(Interop.IHTMLEventObj e) { if (LayoutComplete != null) { OnLayoutComplete(new  DocumentEventArgs(e, this)); } }
        internal void InvokeLoad(Interop.IHTMLEventObj e) { if (Load != null) { OnLoad(new  DocumentEventArgs(e, this)); } }
        internal void InvokeMouseWheel(Interop.IHTMLEventObj e) { if (MouseWheel != null) { OnMouseWheel(new  DocumentEventArgs(e, this)); } }
        internal void InvokeMoveEnd(Interop.IHTMLEventObj e) { if (MoveEnd != null) { OnMoveEnd(new  DocumentEventArgs(e, this)); } }
        internal void InvokeMoveStart(Interop.IHTMLEventObj e) { if (MoveStart != null) { OnMoveStart(new  DocumentEventArgs(e, this)); } }
        internal void InvokeActivate(Interop.IHTMLEventObj e) { if (Activate != null) { OnActivate(new  DocumentEventArgs(e, this)); } }
        internal void InvokeBeforeActivate(Interop.IHTMLEventObj e) { if (BeforeActivate != null) { OnBeforeActivate(new  DocumentEventArgs(e, this)); } }
        internal void InvokeBeforeCopy(Interop.IHTMLEventObj e) { if (BeforeCopy != null) { OnBeforeCopy(new  DocumentEventArgs(e, this)); } }
        internal void InvokeBeforeCut(Interop.IHTMLEventObj e) { if (BeforeCut != null) { OnBeforeCut(new  DocumentEventArgs(e, this)); } }
        internal void InvokeBeforePaste(Interop.IHTMLEventObj e) { if (BeforePaste != null) { OnBeforePaste(new  DocumentEventArgs(e, this)); } }
        internal void InvokeContextMenu(Interop.IHTMLEventObj e) { if (ContextMenu != null) { OnContextMenu(new  DocumentEventArgs(e, this)); } }
        internal void InvokeCopy(Interop.IHTMLEventObj e) { if (Copy != null) { OnCopy(new  DocumentEventArgs(e, this)); } }
        internal void InvokeCut(Interop.IHTMLEventObj e) { if (Cut != null) { OnCut(new  DocumentEventArgs(e, this)); } }
        internal void InvokeDeactivate(Interop.IHTMLEventObj e) { if (Deactivate != null) { OnDeactivate(new  DocumentEventArgs(e, this)); } }
        internal void InvokeDrag(Interop.IHTMLEventObj e) { if (Drag != null) { OnDrag(new  DocumentEventArgs(e, this)); } }
        internal void InvokeDragEnd(Interop.IHTMLEventObj e) { if (DragEnd != null) { OnDragEnd(new  DocumentEventArgs(e, this)); } }
        internal void InvokeFocusIn(Interop.IHTMLEventObj e) { if (FocusIn != null) { OnFocusIn(new  DocumentEventArgs(e, this)); } }
        internal void InvokeFocusOut(Interop.IHTMLEventObj e) { if (FocusOut != null) { OnFocusOut(new  DocumentEventArgs(e, this)); } }
        internal void InvokeFilterChange(Interop.IHTMLEventObj e) { if (FilterChange != null) { OnFilterChange(new  DocumentEventArgs(e, this)); } }
        internal void InvokeAbort(Interop.IHTMLEventObj e) { if (Abort != null) { OnAbort(new  DocumentEventArgs(e, this)); } }
        internal void InvokeChange(Interop.IHTMLEventObj e) { if (Change != null) { OnChange(new  DocumentEventArgs(e, this)); } }
        internal void InvokeSelect(Interop.IHTMLEventObj e) { if (Select != null) { OnSelect(new  DocumentEventArgs(e, this)); } }
        internal void InvokeSelectionChange(Interop.IHTMLEventObj e) { if (SelectionChange != null) { OnSelectionChange(new  DocumentEventArgs(e, this)); } }
        internal void InvokeStop(Interop.IHTMLEventObj e) { if (Stop != null) { OnStop(new  DocumentEventArgs(e, this)); } }
        internal void InvokeBeforeDeactivate(Interop.IHTMLEventObj e) { if (BeforeDeactivate != null) { OnBeforeDeactivate(new  DocumentEventArgs(e, this)); } }
        internal void InvokeControlSelect(Interop.IHTMLEventObj e) { if (ControlSelect != null) { OnControlSelect(new  DocumentEventArgs(e, this)); } }
        internal void InvokeEditFocus(Interop.IHTMLEventObj e) { if (EditFocus != null) { OnEditFocus(new  DocumentEventArgs(e, this)); } }
        internal void InvokeError(Interop.IHTMLEventObj e) { if (Error != null) { OnError(new  DocumentEventArgs(e, this)); } }
        internal void InvokeMove(Interop.IHTMLEventObj e) { if (Move != null) { OnMove(new  DocumentEventArgs(e, this)); } }
        internal void InvokePaste(Interop.IHTMLEventObj e) { if (Paste != null) { OnPaste(new  DocumentEventArgs(e, this)); } }
        internal void InvokePropertyChange(Interop.IHTMLEventObj e) { if (PropertyChange != null) { OnPropertyChange(new  DocumentEventArgs(e, this)); } }
        internal void InvokeResize(Interop.IHTMLEventObj e) { if (Resize != null) { OnResize(new  DocumentEventArgs(e, this)); } }
        internal void InvokeScroll(Interop.IHTMLEventObj e) { if (Scroll != null) { OnScroll(new  DocumentEventArgs(e, this)); } }
        internal void InvokePage(Interop.IHTMLEventObj e) { if (Paged != null) { OnPage(new  DocumentEventArgs(e, this)); } }


        # endregion

        [Browsable(false), DefaultValue(false)]
        bool IElement.AtomicSelection 
        {
            get
            {
                return false; //throw new NotSupportedException();
            }
            set
            { 
                throw new NotSupportedException();
            }
        }

        [Browsable(false), DefaultValue(false)]
        bool IElement.Unselectable 
        { 
            get
            {
                return false; //throw new NotSupportedException();
            } 
            set
            { 
                throw new NotSupportedException();
            }
        }

        [Browsable(false), DesignOnly(true)]
        public string Alias
        {
            get
            {
                return ((Interop.IHTMLElement2)element).GetScopeName();
            }
        }

        [Browsable(false), DesignOnly(true)]
        public string ElementName
        {
            get
            {
                return String.Format("{0}:{1}", Alias, TagName);
            }
        }

        [Browsable(false), DesignOnly(true)]
        public string TagName
        {
            get
            {
                return element.GetTagName();
            }
        }

        #region ICustomTypeDescriptor Member

        PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties(Attribute[] filter) 
        { 
            Type t = GetType();
            PropertyDescriptorCollection baseProps;
            try
            {
                baseProps = TypeDescriptor.GetProperties(t, filter); 
            }
            catch
            {
                baseProps = TypeDescriptor.GetProperties(t, filter); 
            }
            for (int i = 0; i < baseProps.Count; i++) 
            { 
                // TODO: Call OnPropertyFilterRequest here!
            } 
            return baseProps;
        }
 
        AttributeCollection ICustomTypeDescriptor.GetAttributes() 
        { 
            return TypeDescriptor.GetAttributes(this, true); 
        } 
 
        string ICustomTypeDescriptor.GetClassName() 
        { 
            return TypeDescriptor.GetClassName(this, true); 
        } 
 
        string ICustomTypeDescriptor.GetComponentName() 
        { 
            return TypeDescriptor.GetComponentName(this, true); 
        } 
 
        TypeConverter ICustomTypeDescriptor.GetConverter() 
        { 
            return TypeDescriptor.GetConverter(this, true); 
        } 
 
        EventDescriptor ICustomTypeDescriptor.GetDefaultEvent() 
        { 
            return TypeDescriptor.GetDefaultEvent(this, true); 
        } 
 
        EventDescriptorCollection ICustomTypeDescriptor.GetEvents(Attribute[] attributes) 
        { 
            return TypeDescriptor.GetEvents(this, attributes, true); 
        } 
 
        EventDescriptorCollection ICustomTypeDescriptor.GetEvents() 
        { 
            return TypeDescriptor.GetEvents(this, true); 
        } 
 
        PropertyDescriptor ICustomTypeDescriptor.GetDefaultProperty() 
        { 
            return TypeDescriptor.GetDefaultProperty(this, true); 
        } 
 
        PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties() 
        { 
            return TypeDescriptor.GetProperties(this, true); 
        } 
 
        object ICustomTypeDescriptor.GetEditor(Type editorBaseType) 
        { 
            return TypeDescriptor.GetEditor(this, editorBaseType, true); 
        } 
 
        object ICustomTypeDescriptor.GetPropertyOwner(PropertyDescriptor pd) 
        { 
            return this; 
        } 
 
        #endregion

        #region IElement Members

        [Browsable(false), System.Web.UI.PersistenceModeAttribute(System.Web.UI.PersistenceMode.InnerProperty)]
        public IElementBehavior ElementBehaviors
        {
            get
            {
                if (eb == null)
                {
                    eb = new ElementBehavior(this);
                }
                return eb;

            }
        }

        #endregion

        #region IElement Members

        [Browsable(false)]
        public bool IsAbsolutePositioned
        {
            get { return ("absolute".Equals(GetAttribute("position"))); }
        }

        [Browsable(false)]
        public bool IsTextEdit
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        #endregion

        System.Web.UI.CssStyleCollection IElement.Style
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        event DocumentEventHandler IElement.LoseCapture
        {
            add { throw new NotImplementedException(); }
            remove { throw new NotImplementedException(); }
        }

        event DocumentEventHandler IElement.Click
        {
            add { throw new NotImplementedException(); }
            remove { throw new NotImplementedException(); }
        }

        event DocumentEventHandler IElement.DblClick
        {
            add { throw new NotImplementedException(); }
            remove { throw new NotImplementedException(); }
        }

        event DocumentEventHandler IElement.DragStart
        {
            add { throw new NotImplementedException(); }
            remove { throw new NotImplementedException(); }
        }

        event DocumentEventHandler IElement.Focus
        {
            add { throw new NotImplementedException(); }
            remove { throw new NotImplementedException(); }
        }

        event DocumentEventHandler IElement.Drop
        {
            add { throw new NotImplementedException(); }
            remove { throw new NotImplementedException(); }
        }

        event DocumentEventHandler IElement.Blur
        {
            add { throw new NotImplementedException(); }
            remove { throw new NotImplementedException(); }
        }

        event DocumentEventHandler IElement.DragOver
        {
            add { throw new NotImplementedException(); }
            remove { throw new NotImplementedException(); }
        }

        event DocumentEventHandler IElement.DragEnter
        {
            add { throw new NotImplementedException(); }
            remove { throw new NotImplementedException(); }
        }

        event DocumentEventHandler IElement.DragLeave
        {
            add { throw new NotImplementedException(); }
            remove { throw new NotImplementedException(); }
        }

        event DocumentEventHandler IElement.KeyDown
        {
            add { throw new NotImplementedException(); }
            remove { throw new NotImplementedException(); }
        }

        event DocumentEventHandler IElement.KeyPress
        {
            add { throw new NotImplementedException(); }
            remove { throw new NotImplementedException(); }
        }

        event DocumentEventHandler IElement.KeyUp
        {
            add { throw new NotImplementedException(); }
            remove { throw new NotImplementedException(); }
        }

        event DocumentEventHandler IElement.MouseDown
        {
            add { throw new NotImplementedException(); }
            remove { throw new NotImplementedException(); }
        }

        event DocumentEventHandler IElement.ResizeStart
        {
            add { throw new NotImplementedException(); }
            remove { throw new NotImplementedException(); }
        }

        event DocumentEventHandler IElement.ResizeEnd
        {
            add { throw new NotImplementedException(); }
            remove { throw new NotImplementedException(); }
        }

        event DocumentEventHandler IElement.MouseEnter
        {
            add { throw new NotImplementedException(); }
            remove { throw new NotImplementedException(); }
        }

        event DocumentEventHandler IElement.MouseLeave
        {
            add { throw new NotImplementedException(); }
            remove { throw new NotImplementedException(); }
        }

        event DocumentEventHandler IElement.MouseMove
        {
            add { throw new NotImplementedException(); }
            remove { throw new NotImplementedException(); }
        }

        event DocumentEventHandler IElement.MouseOut
        {
            add { throw new NotImplementedException(); }
            remove { throw new NotImplementedException(); }
        }

        event DocumentEventHandler IElement.MouseOver
        {
            add { throw new NotImplementedException(); }
            remove { throw new NotImplementedException(); }
        }

        event DocumentEventHandler IElement.MouseUp
        {
            add { throw new NotImplementedException(); }
            remove { throw new NotImplementedException(); }
        }

        event DocumentEventHandler IElement.SelectStart
        {
            add { throw new NotImplementedException(); }
            remove { throw new NotImplementedException(); }
        }

        event DocumentEventHandler IElement.LayoutComplete
        {
            add { throw new NotImplementedException(); }
            remove { throw new NotImplementedException(); }
        }

        event DocumentEventHandler IElement.Load
        {
            add { throw new NotImplementedException(); }
            remove { throw new NotImplementedException(); }
        }

        event DocumentEventHandler IElement.MouseWheel
        {
            add { throw new NotImplementedException(); }
            remove { throw new NotImplementedException(); }
        }

        event DocumentEventHandler IElement.MoveEnd
        {
            add { throw new NotImplementedException(); }
            remove { throw new NotImplementedException(); }
        }

        event DocumentEventHandler IElement.MoveStart
        {
            add { throw new NotImplementedException(); }
            remove { throw new NotImplementedException(); }
        }

        event DocumentEventHandler IElement.Activate
        {
            add { throw new NotImplementedException(); }
            remove { throw new NotImplementedException(); }
        }

        event DocumentEventHandler IElement.BeforeActivate
        {
            add { throw new NotImplementedException(); }
            remove { throw new NotImplementedException(); }
        }

        event DocumentEventHandler IElement.BeforeCopy
        {
            add { throw new NotImplementedException(); }
            remove { throw new NotImplementedException(); }
        }

        event DocumentEventHandler IElement.BeforeCut
        {
            add { throw new NotImplementedException(); }
            remove { throw new NotImplementedException(); }
        }

        event DocumentEventHandler IElement.BeforePaste
        {
            add { throw new NotImplementedException(); }
            remove { throw new NotImplementedException(); }
        }

        event DocumentEventHandler IElement.ContextMenu
        {
            add { throw new NotImplementedException(); }
            remove { throw new NotImplementedException(); }
        }

        event DocumentEventHandler IElement.Copy
        {
            add { throw new NotImplementedException(); }
            remove { throw new NotImplementedException(); }
        }

        event DocumentEventHandler IElement.Cut
        {
            add { throw new NotImplementedException(); }
            remove { throw new NotImplementedException(); }
        }

        event DocumentEventHandler IElement.Deactivate
        {
            add { throw new NotImplementedException(); }
            remove { throw new NotImplementedException(); }
        }

        event DocumentEventHandler IElement.Drag
        {
            add { throw new NotImplementedException(); }
            remove { throw new NotImplementedException(); }
        }

        event DocumentEventHandler IElement.DragEnd
        {
            add { throw new NotImplementedException(); }
            remove { throw new NotImplementedException(); }
        }

        event DocumentEventHandler IElement.FocusIn
        {
            add { throw new NotImplementedException(); }
            remove { throw new NotImplementedException(); }
        }

        event DocumentEventHandler IElement.FocusOut
        {
            add { throw new NotImplementedException(); }
            remove { throw new NotImplementedException(); }
        }

        event DocumentEventHandler IElement.FilterChange
        {
            add { throw new NotImplementedException(); }
            remove { throw new NotImplementedException(); }
        }

        event DocumentEventHandler IElement.Abort
        {
            add { throw new NotImplementedException(); }
            remove { throw new NotImplementedException(); }
        }

        event DocumentEventHandler IElement.Change
        {
            add { throw new NotImplementedException(); }
            remove { throw new NotImplementedException(); }
        }

        event DocumentEventHandler IElement.Select
        {
            add { throw new NotImplementedException(); }
            remove { throw new NotImplementedException(); }
        }

        event DocumentEventHandler IElement.SelectionChange
        {
            add { throw new NotImplementedException(); }
            remove { throw new NotImplementedException(); }
        }

        event DocumentEventHandler IElement.Stop
        {
            add { throw new NotImplementedException(); }
            remove { throw new NotImplementedException(); }
        }

        event DocumentEventHandler IElement.BeforeDeactivate
        {
            add { throw new NotImplementedException(); }
            remove { throw new NotImplementedException(); }
        }

        event DocumentEventHandler IElement.ControlSelect
        {
            add { throw new NotImplementedException(); }
            remove { throw new NotImplementedException(); }
        }

        event DocumentEventHandler IElement.EditFocus
        {
            add { throw new NotImplementedException(); }
            remove { throw new NotImplementedException(); }
        }

        event DocumentEventHandler IElement.Error
        {
            add { throw new NotImplementedException(); }
            remove { throw new NotImplementedException(); }
        }

        event DocumentEventHandler IElement.Move
        {
            add { throw new NotImplementedException(); }
            remove { throw new NotImplementedException(); }
        }

        event DocumentEventHandler IElement.Paste
        {
            add { throw new NotImplementedException(); }
            remove { throw new NotImplementedException(); }
        }

        event DocumentEventHandler IElement.PropertyChange
        {
            add { throw new NotImplementedException(); }
            remove { throw new NotImplementedException(); }
        }

        event DocumentEventHandler IElement.Resize
        {
            add { throw new NotImplementedException(); }
            remove { throw new NotImplementedException(); }
        }

        event DocumentEventHandler IElement.Scroll
        {
            add { throw new NotImplementedException(); }
            remove { throw new NotImplementedException(); }
        }

        event DocumentEventHandler IElement.Paged
        {
            add { throw new NotImplementedException(); }
            remove { throw new NotImplementedException(); }
        }

        IExtendedProperties IElement.ExtendedProperties
        {
            get { throw new NotImplementedException(); }
        }

        string IElement.UniqueName
        {
            get { throw new NotImplementedException(); }
        }

        bool IElement.IsSelectable()
        {
            throw new NotImplementedException();
        }

        bool IElement.IsAbsolutePositioned
        {
            get { throw new NotImplementedException(); }
        }

        bool IElement.IsTextEdit
        {
            get { throw new NotImplementedException(); }
        }

        void IElement.InsertAdjacentElement(InsertWhere method, IElement element)
        {
            throw new NotImplementedException();
        }

        void IElement.InsertAdjacentHtml(InsertWhere method, string html)
        {
            throw new NotImplementedException();
        }

        void IElement.InsertAdjacentText(InsertWhere method, string text)
        {
            throw new NotImplementedException();
        }

        Rectangle IElement.GetAbsoluteArea()
        {
            throw new NotImplementedException();
        }

        Interop.IHTMLElement IElement.GetBaseElement()
        {
            throw new NotImplementedException();
        }

        IHtmlEditor IElement.HtmlEditor
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        bool IElement.ContentEditable
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        IElementBehavior IElement.ElementBehaviors
        {
            get { throw new NotImplementedException(); }
        }

        string IElement.InnerHtml
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        string IElement.OuterHtml
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        string IElement.InnerText
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        string IElement.TagName
        {
            get { throw new NotImplementedException(); }
        }

        IElement IElement.TagElement
        {
            get { throw new NotImplementedException(); }
        }

        string IElement.Alias
        {
            get { throw new NotImplementedException(); }
        }

        string IElement.ElementName
        {
            get { throw new NotImplementedException(); }
        }

        System.Web.UI.Control IElement.GetChild(string name)
        {
            throw new NotImplementedException();
        }

        ElementCollection IElement.GetChildren()
        {
            throw new NotImplementedException();
        }

        System.Web.UI.Control IElement.GetParent()
        {
            throw new NotImplementedException();
        }

        string IElement.ToString()
        {
            throw new NotImplementedException();
        }

        string IElement.GetStyle()
        {
            throw new NotImplementedException();
        }

        void IElement.SetStyle(string CssText)
        {
            throw new NotImplementedException();
        }

        string IElement.GetStyleAttribute(string styleName)
        {
            throw new NotImplementedException();
        }

        void IElement.SetStyleAttribute(string styleName, string styleText)
        {
            throw new NotImplementedException();
        }

        void IElement.RemoveStyleAttribute(string styleName)
        {
            throw new NotImplementedException();
        }

        void IElement.SetAttribute(string attribute, object value)
        {
            throw new NotImplementedException();
        }

        void IElement.RemoveAttribute(string attribute)
        {
            throw new NotImplementedException();
        }

        object IElement.GetAttribute(string attribute)
        {
            throw new NotImplementedException();
        }

        IElementDom IElement.ElementDom
        {
            get { throw new NotImplementedException(); }
        }

        IEffectiveStyle IElement.EffectiveStyle
        {
            get { throw new NotImplementedException(); }
        }

        IElementStyle IElement.RuntimeStyle
        {
            get { throw new NotImplementedException(); }
        }

        IElementStyle IElement.CurrentStyle
        {
            get { throw new NotImplementedException(); }
        }
    }
}