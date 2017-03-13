using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.Events;
using Comzept.Genesis.NetRix.VgxDraw;
using GuruComponents.Netrix.WebEditing.Elements;
using GuruComponents.Netrix.WebEditing.Styles;
#pragma warning disable 1591
namespace GuruComponents.Netrix.VmlDesigner.Elements
{
	/// <summary>
	/// The base class used for all kinds of shapes.
	/// </summary>
	/// <remarks>
	/// This class provides access to elementary attributes, events, properties and methods
	/// any VML element has.
	/// </remarks>
    [ToolboxItem(false)]
	public class VmlBaseElement : Control, IElement, IComponent
	{
        private IHtmlEditor htmlEditor;
        private Interop.IHTMLElement element;
	    private IElementDom elementDom;
		private EventSink _eventSink;

		internal VmlBaseElement(Interop.IHTMLElement peer, IHtmlEditor editor)
		{
            this.htmlEditor = editor;
			Debug.Assert(peer != null, "ctor needs a peer");
			element = peer;
		    Connect();
		}

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="newTag"></param>
        /// <param name="editor"></param>
        protected VmlBaseElement(string newTag, IHtmlEditor editor)
        {
            this.htmlEditor = editor;
            VmlBaseElement vel = (VmlBaseElement) this.htmlEditor.CreateElement(newTag);
            element = vel.GetBaseElement();
			Connect();
        }

        private IExtendedProperties extendedProperties;
        /// <summary>
        /// Access to several additional properties.
        /// </summary>
        public IExtendedProperties ExtendedProperties
        {
            get
            {
                if (extendedProperties == null)
                {
                    extendedProperties = new ExtendedProperties(GetBaseElement());
                }
                return extendedProperties;
            }
        }

        /// <summary>
        /// Is selectable
        /// </summary>
        /// <returns></returns>
        public virtual bool IsSelectable()
        {
            return true;
        }

        [Browsable(false)]
        string IElement.UniqueName
        {
            get { return ""; }
        }

		internal void Connect()
		{
			_eventSink = new EventSink(this);
			_eventSink.Connect();
		}

		internal void DisConnect()
		{
			if (_eventSink != null)
			{
				_eventSink.Disconnect();
				_eventSink = null;
			}
		}

        /// <summary>
        /// Disconnect on finalize
        /// </summary>
		~VmlBaseElement()
		{
			DisConnect();
		}

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Interop.IHTMLElement GetBaseElement() 
        {
            return element;
        }

        /// <summary>
        /// Referenced editor.
        /// </summary>
        [Browsable(false)]
        public IHtmlEditor HtmlEditor
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

		/// <summary>
		/// Inserts an element at the specified location.
		/// </summary>
		/// <param name="method">Specifies where to insert the HTML element.</param>
		/// <param name="element">Element to be inserted adjacent to the object.</param>
		void IElement.InsertAdjacentElement(InsertWhere method, IElement element)
		{
			throw new NotImplementedException("");
		}

		/// <summary>
		/// Inserts the given HTML text into the element at the location.
		/// </summary>
		/// <param name="method"></param>
		/// <param name="html"></param>
		void IElement.InsertAdjacentHtml(InsertWhere method, string html)
		{
			throw new NotImplementedException("");
		}

		/// <summary>
		/// Inserts the given text into the element at the specified location.
		/// </summary>
		/// <param name="method">Specifies where to insert the text.</param>
		/// <param name="text"></param>
		void IElement.InsertAdjacentText(InsertWhere method, string text)
		{
			throw new NotImplementedException("");
		}

        # region Inner/Outer Access

        private delegate void SetInnerOuterDelegate(string text);

        private void InvokeSetInnerHtml(string text)
        {
            if (((System.Windows.Forms.Control)htmlEditor).InvokeRequired)
            {
                SetInnerOuterDelegate d = new SetInnerOuterDelegate(InvokeSetInnerHtml);
                ((System.Windows.Forms.Control)htmlEditor).Invoke(d, text);                
            }
            else
            {
                element.SetInnerHTML(text);
            }
        }

        private void InvokeSetInnerText(string text)
        {
            if (((System.Windows.Forms.Control)htmlEditor).InvokeRequired)
            {
                SetInnerOuterDelegate d = new SetInnerOuterDelegate(InvokeSetInnerText);
                ((System.Windows.Forms.Control)htmlEditor).Invoke(d, text);
            }
            else
            {
                element.SetInnerText(text);
            }
        }

        private void InvokeSetOuterHtml(string text)
        {
            if (((System.Windows.Forms.Control)htmlEditor).InvokeRequired)
            {
                SetInnerOuterDelegate d = new SetInnerOuterDelegate(InvokeSetOuterHtml);
                ((System.Windows.Forms.Control)htmlEditor).Invoke(d, text);
            }
            else
            {
                element.SetOuterHTML(text);
            }
        }

        /// <summary>
        /// Gets or sets inner html of the element.
        /// </summary>
        /// <remarks>
        /// The inner html is the complete content between the opening and the closing tag.
        /// If the element is not a container element the property will return <see cref="System.String.Empty">String.Empty</see>.
        /// Any HTML is returned "as is", with the braces and tag names.
        /// </remarks>
        [Browsable(false)]
        public string InnerHtml
        {
            get
            {
                string str;

                try
                {
                    str = element.GetInnerHTML();
                }
                catch
                {
                    str = String.Empty;
                }
                return str;
            }

            set
            {
                try
                {
                    if (htmlEditor.ThreadSafety)
                    {
                        InvokeSetInnerHtml(value);
                    }
                    else
                    {
                        element.SetInnerHTML(value);
                    }
                }
                catch
                {                    
                }
            }
        }

        /// <summary>
        /// Gets or sets the inner text of the element.
        /// </summary>
        /// <remarks>
        /// The inner text is the complete content between the opening and the closing tag, with any HTML tags
        /// stripped out.
        /// If the element is not a container element the property will return <see cref="System.String.Empty">String.Empty</see>.
        /// Any HTML is returned "as is", with the braces and tag names.
        /// </remarks>
        [Browsable(false)]
        public string InnerText
        {
            get
            {
                string str;

                try
                {
                    str = element.GetInnerText();
                }
                catch
                {
                    str = String.Empty;
                }
                return str;
            }

            set
            {
                try
                {
                    if (htmlEditor.ThreadSafety)
                    {
                        InvokeSetInnerText(value);
                    }
                    else
                    {
                        element.SetInnerText(value);
                    }
                }
                catch
                {                    
                }
            }
        }

        /// <summary>
        /// Gets or sets outer html of the element.
        /// </summary>
        /// <remarks>
        /// The outer html is the complete content between the opening and the closing tag and it includes the
        /// tags themselfes.
        /// If the element is not a container element the property will return <see cref="System.String.Empty">String.Empty</see>.
        /// Any HTML is returned "as is", with the braces and tag names.
        /// </remarks>
        [Browsable(false)]
        public string OuterHtml
        {
            get
            {
                string str;

                try
                {
                    str = element.GetOuterHTML();
                }
                catch
                {
                    str = String.Empty;
                }
                return str;
            }

            set
            {
                try
                {
                    if (htmlEditor.ThreadSafety)
                    {
                        InvokeSetOuterHtml(value);
                    }
                    else
                    {
                        element.SetOuterHTML(value);
                    }
                }
                catch
                {
                }
            }
        }

        # endregion Inner/Outer Access

        [Browsable(false)]
        public IElementDom ElementDom
        {
            get
            {
                if (elementDom == null)
                {
                    elementDom = new ElementDom((Interop.IHTMLDOMNode) element, this.HtmlEditor);
                }
                return elementDom;
            }
        }

        /// <summary>
        /// Tag form.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return String.Format("{0}:{1}", ((Interop.IHTMLElement2)element).GetScopeName(), element.GetTagName());
        }

        /// <summary>
        /// Get children of element
        /// </summary>
        /// <returns></returns>
        public ElementCollection GetChildren()
        {
            Interop.IHTMLElementCollection ec = this.GetBaseElement().GetChildren() as Interop.IHTMLElementCollection;
            ElementCollection newEc = new ElementCollection();
            if (ec != null)
            {
                for (int i = 0; i < ec.GetLength(); i++)
                {
                    newEc.Add(this.HtmlEditor.GenericElementFactory.CreateElement((Interop.IHTMLElement) ec.Item(i, i)));
                }
            }
            return newEc;
        }

        [Browsable(true), DefaultValue(true), Category("Element Behavior")]
        public bool ContentEditable
        {
            get
            {
                return ((Interop.IHTMLElement3) element).contentEditable.Equals("true");
            }
            set
            {
                ((Interop.IHTMLElement3) element).contentEditable = value ? "true" : "false";
            }
        }

        [Browsable(false)]
        public IElement TagElement
        {
            get
            {
                return this;
            }
        }

        /// <summary>
        /// Get parent element
        /// </summary>
        /// <returns></returns>
        public Control GetParent()
        {            
            Interop.IHTMLDOMNode node = ((Interop.IHTMLDOMNode) this.element).parentNode;
            Interop.IHTMLElement parent = node as Interop.IHTMLElement;	// may fail if there is no parent
            if (parent != null)
            {
                return this.HtmlEditor.GenericElementFactory.CreateElement(parent);
            } 
            else 
            {
                return null;
            }
        }

        #region GetAttr

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attribute"></param>
        /// <returns></returns>
        public virtual object GetAttribute(string attribute)
        {
            try
            {
                if (htmlEditor.ThreadSafety)
                {
                    return InvokeGetAttribute(attribute);
                }
                else
                {
                    return GetNativeAttribute(attribute);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return null;
            }
        }

        private object InvokeGetAttribute(string attr)
        {
            if (((System.Windows.Forms.Control)htmlEditor).InvokeRequired)
            {
                GetAttributeDelegate d = new GetAttributeDelegate(InvokeGetAttribute);
                return ((System.Windows.Forms.Control)htmlEditor).Invoke(d, new object[] { attr });
            }
            else
            {
                return GetNativeAttribute(attr);
            }
        }

        private delegate object GetAttributeDelegate(string attr);

                /// <summary>
        /// Universal access to any attribute.
        /// </summary>
        /// <remarks>
        /// The type returned may vary depended on the internal type.
        /// </remarks>
        /// <param name="attribute">The name of the attribute.</param>
        /// <returns>The object which is the value of the attribute.</returns>
        private object GetNativeAttribute(string attribute)
        {
            object local2;
            attribute = (attribute.Equals("http-equiv")) ? "httpequiv" : attribute;
            try
            {
                object[] locals = new object[1];
                locals[0] = null;
                element.GetAttribute(attribute, 0, locals);
                object local1 = locals[0];
                if (local1 is DBNull)
                {
                    local1 = null;
                }
                local2 = local1;
            }
            catch
            {
                local2 = null;
            }
            return local2;
        }

        #endregion GetAttr

        #region RemoveAttr

        /// <summary>
        /// Remove the give attribute from element.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This property is thread safe if <see cref="GuruComponents.Netrix.HtmlEditor.ThreadSafety">ThreadSafety</see>
        /// is turned on. This feature was added in Version 1.6 (Nov 2006) and is available in both, Standard and Advanced
        /// edition.
        /// </para>
        /// </remarks>
        /// <seealso cref="GuruComponents.Netrix.HtmlEditor.ThreadSafety">ThreadSafety</seealso>
        /// <param name="attribute">The name of the attribute which is about to be removed. Case insensitive.</param>
        public virtual void RemoveAttribute(string attribute)
        {
            try
            {
                if (htmlEditor.ThreadSafety)
                {
                    InvokeRemoveAttribute(attribute);
                }
                else
                {
                    if (GetAttribute(attribute) != null)
                    {
                        element.RemoveAttribute(attribute, 0);
                    }
                }
            }
            catch
            {
            }
        }

        private delegate void RemoveAttributeDelegate(string attr);

        private void InvokeRemoveAttribute(string attr)
        {
            if (((System.Windows.Forms.Control)htmlEditor).InvokeRequired)
            {
                RemoveAttributeDelegate d = new RemoveAttributeDelegate(InvokeRemoveAttribute);
                ((System.Windows.Forms.Control)htmlEditor).Invoke(d, new object[] { attr });
            }
            else
            {
                if (GetAttribute(attr) != null)
				{
					element.RemoveAttribute(attr, 0);
				}
            }
        }

        #endregion RemoveAttr

        #region SetAttribute

        /// <summary>
        /// Sets an attribute to a specific value.
        /// </summary>
        /// <remarks>
        /// The command may does nothing if the value does not correspond with the attribute. E.g. it
        /// is almost impossible to write a pixel value if the attribute expects a font name.
        /// <para>
        /// This property is thread safe if <see cref="GuruComponents.Netrix.HtmlEditor.ThreadSafety">ThreadSafety</see>
        /// is turned on. 
        /// </para>
        /// </remarks>
        /// <seealso cref="GuruComponents.Netrix.HtmlEditor.ThreadSafety">ThreadSafety</seealso>
        /// <param name="attribute">The name of the attribute.</param>
        /// <param name="value">The value being written.</param>
        public virtual void SetAttribute(string attribute, object value)
        {
            try
            {
                if (htmlEditor.ThreadSafety)
                {
                    //RemoveAttribute(attribute);
                    InvokeSetAttribute(attribute, value);
                }
                else
                {
                    //RemoveAttribute(attribute);
                    element.SetAttribute(attribute, value, 0);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        private delegate void SetAttributeDelegate(string attr, object value);

        private void InvokeSetAttribute(string attr, object value)
        {
            if (((System.Windows.Forms.Control)htmlEditor).InvokeRequired)
            {
                SetAttributeDelegate d = new SetAttributeDelegate(InvokeSetAttribute);
                ((System.Windows.Forms.Control)htmlEditor).Invoke(d, new object[] { attr, value });
            }
            else
            {
                element.SetAttribute(attr, value, 0);
            }
        }

        #endregion SetAttribute

        # region Get/Set Styles

        /// <summary>
        /// This method return the complete CSS inline definition, which the style attribute contains.
        /// </summary>
        /// <remarks>
        /// For easy access to specific styles it is recommended to use the 
        /// <see cref="GuruComponents.Netrix.WebEditing.Elements.IElement.GetStyleAttribute">GetStyleAttribute</see> and 
        /// <see cref="GuruComponents.Netrix.WebEditing.Elements.IElement.SetStyleAttribute">SetStyleAttribute</see>
        /// methods. This and both alternative methods will check the content and cannot assign values not
        /// processed by the Internet Explorer Engine. The final behavior may vary depending on the currently
        /// installed IE engine.
        /// </remarks>
        /// <returns>Returns the complete style text (all rules) as one string.</returns>
        public string GetStyle()
        {
            Interop.IHTMLStyle style = GetBaseElement().GetStyle();
            return style.GetCssText();
        }

        /// <summary>
        /// Set the current style by overwriting the complete content of the style attribute.
        /// </summary>
        /// <param name="CssText">The style text; should be CSS compliant.</param>
        public void SetStyle(string CssText)
        {
            if (htmlEditor.ThreadSafety)
            {
                InvokeSetStyle(CssText);
            }
            else
            {
                Interop.IHTMLStyle style = GetBaseElement().GetStyle();
                style.SetCssText(CssText);
            }
        }

        private delegate void SetStyleDelegate(string css);
        private delegate void SetStyleAttributeDelegate(string styleName, string styleText);

        private void InvokeSetStyle(string CssText)
        {
            if (((System.Windows.Forms.Control)htmlEditor).InvokeRequired)
            {
                SetStyleDelegate d = new SetStyleDelegate(InvokeSetStyle);
                ((System.Windows.Forms.Control)htmlEditor).Invoke(d, CssText);
            }
            else
            {
                element.GetStyle().SetCssText(CssText);
            }
        }

        private void InvokeSetStyleAttribute(string styleName, string styleText)
        {
            if (((System.Windows.Forms.Control)htmlEditor).InvokeRequired)
            {
                SetStyleAttributeDelegate d = new SetStyleAttributeDelegate(InvokeSetStyleAttribute);
                ((System.Windows.Forms.Control)htmlEditor).Invoke(d, styleName, styleText);
            }
            else
            {
                try
                {
                    element.GetStyle().SetAttribute(styleName.Replace("-", String.Empty), styleText, 0);
                }
                catch
                {
                    element.GetStyle().SetAttribute(styleName, styleText, 0);
                }
            }
        }

        /// <summary>
        /// Gets a specific part of an inline style.
        /// </summary>
        /// <param name="styleName">The style attribute to retrieve</param>
        /// <returns>The string representation of the style. Returns <see cref="String.Empty"/> if the 
        /// style does not exists.</returns>
        public string GetStyleAttribute(string styleName)
        {    
            Interop.IHTMLStyle style = GetBaseElement().GetStyle();
            object o = style.GetAttribute(styleName.Replace("-", String.Empty), 0);
            if (o == null)
            {
                return String.Empty;
            }
            else 
            {
                string styleText = o.ToString();
                return styleText;
            }
        }

        /// <summary>
        /// Sets a specific part of an inline style.
        /// </summary>
        /// <remarks>
        /// Setting to <seealso cref="System.String.Empty">String.Empty</seealso> does remove
        /// the style name. For a more intuitive way use <see cref="GuruComponents.Netrix.WebEditing.Elements.Element.RemoveStyleAttribute">RemoveStyleAttribute</see> instead.
        /// Setting impossible rule texts will either ignore the command or set unexpected values.
        /// </remarks>
        /// <param name="styleName">The name of the style rule beeing set, e.g. "font-family".</param>
        /// <param name="styleText">The rule content, like "Verdana,Arial" for the "font-family" rule.</param>
        public virtual void SetStyleAttribute(string styleName, string styleText)
        {
            if (htmlEditor.ThreadSafety)
            {
                InvokeSetStyleAttribute(styleName, styleText);
            }
            else
            {
                try
                {
                    element.GetStyle().SetAttribute(styleName.Replace("-", String.Empty), styleText, 0);
                }
                catch
                {
                    element.GetStyle().SetAttribute(styleName, styleText, 0);
                }
            }
        }

        /// <summary>
        /// Removes an style attribute and his content from the inline style string.
        /// </summary>
        /// <param name="styleName">Name of style to be removed</param>
        public virtual void RemoveStyleAttribute(string styleName)
        {
            if (styleName.IndexOf("-") != -1)
            {
                SetStyleAttribute(styleName, string.Empty);
            } 
            else 
            {
                Interop.IHTMLStyle style = GetBaseElement().GetStyle();
                style.RemoveAttribute(styleName, 0);
            }
        }

        # endregion

        # region Specialized Attribute Access Methods

        protected internal void SetBooleanAttribute(string attribute, bool value)
        {
            if (value)
            {
                SetAttribute(attribute, 1);
            } 
            else 
            {
                //RemoveAttribute(attribute);
                element.SetAttribute(attribute, String.Empty, 0);
            }
        }

        protected internal void SetColorAttribute(string attribute, Color value)
        {
            if (value.IsEmpty)
            {
                RemoveAttribute(attribute);
                return;
            }            
            IVgColor c = GetAttribute(attribute) as IVgColor;
			if (c != null)
			{
				c.R = value.R;
				c.G = value.G;
				c.B = value.B;			
				SetAttribute(attribute, c);
			} 
			else 
			{
				SetAttribute(attribute, ColorTranslator.ToHtml(value));
			}
        }

        protected internal void SetEnumAttribute(string attribute, Enum value, Enum defaultValue)
        {
            if (value.Equals(defaultValue))
            {
                RemoveAttribute(attribute);
                return;
            }
            SetAttribute(attribute, value.ToString());
        }

        protected internal void SetIntegerAttribute(string attribute, int value, int defaultValue)
        {
            if (value == defaultValue)
            {
                RemoveAttribute(attribute);
                return;
            }
            SetAttribute(attribute, value);
        }

        protected internal void SetStringAttribute(string attribute, string value)
        {
            SetStringAttribute(attribute, value, String.Empty);
        }

        protected internal void SetStringAttribute(string attribute, string value, string defaultValue)
        {
            if (value == null || value.Equals(defaultValue))
            {
                RemoveAttribute(attribute);
                return;
            }
            SetAttribute(attribute, value);
            // TODO: htmlEditor.FireContentModified(new GuruComponents.Netrix.EventHandler.ContentModifiedEventArgs(this, htmlEditor));
        }

        protected internal void SetUnitAttribute(string attribute, Unit value)
        {
            if (value.IsEmpty)
            {
                RemoveAttribute(attribute);
                return;
            }
            SetAttribute(attribute, value.ToString(CultureInfo.InvariantCulture));
        }


        protected internal bool GetBooleanAttribute(string attribute)
        {
            object local = GetAttribute(attribute);
            if (local == null)
            {
                return false;
            }
            if (local is Boolean)
            {
                return (bool)local;
            }
            else
            {
                return false;
            }
        }

        protected internal Color GetColorAttribute(string attribute)
        {
			if (GetAttribute(attribute) is IVgColor)
			{
				IVgColor str = GetAttribute(attribute) as IVgColor;
				if (str != null)
				{
					return ColorTranslator.FromHtml(str.value);
				}
				else
				{
					return Color.Empty;
				}
			} 
			if (GetAttribute(attribute) is string)			
			{
				return ColorTranslator.FromHtml(GetAttribute(attribute).ToString());
			}
			return Color.Empty;
        }

        protected internal Enum GetEnumAttribute(string attribute, Enum defaultValue)
        {
            Enum @enum;

            Type type = defaultValue.GetType();
            object local = GetAttribute(attribute);
            if (local == null)
            {
                return defaultValue;
            }
            string str = local as String;
            if (str == null || str.Length == 0)
            {
                return defaultValue;
            }
            try
            {
                @enum = (Enum)Enum.Parse(type, str, true);
            }
            catch
            {
                @enum = defaultValue;
            }
            return @enum;
        }

        protected internal int GetIntegerAttribute(string attribute, int defaultValue)
        {
            object local = GetAttribute(attribute);
            if (local == null)
            {
                return defaultValue;
            }
            if (local is Int32)
            {
                return (int)local;
            }
            if (local is Int16)
            {
                return (short)local;
            }
            if (local is String)
            {
                string str = (String)local;
                if (str.Length != 0 && Char.IsDigit(str[0]))
                {
                    try
                    {
                        int i = Int32.Parse((String)local);
                        return i;
                    }
                    catch
                    {
                        return defaultValue;
                    }
                }
            }
            return defaultValue;
        }

        protected internal string GetStringAttribute(string attribute)
        {
            return GetStringAttribute(attribute, String.Empty);
        }

        protected internal string GetStringAttribute(string attribute, string defaultValue)
        {
            object local = GetAttribute(attribute);
            if (local == null)
            {
                return defaultValue;
            }
            if (local is String)
            {
                return (String)local;
            }
            else
            {
                return defaultValue;
            }
        }

        protected internal Unit GetUnitAttribute(string attribute)
        {
            Unit unit;

            object local = GetAttribute(attribute);
            if (local == null)
            {
                return Unit.Empty;
            }
            if (local is Int32)
            {
                return new Unit(Convert.ToInt32(local), UnitType.Pixel);
            }
            if (local is Double)
            {
                return new Unit(Convert.ToDouble(local), UnitType.Point);
            }
            try
            {
                unit = new Unit((String)local, CultureInfo.InvariantCulture);
            }
            catch
            {
                unit = Unit.Empty;
            }
            return unit;
        }

        # endregion 

        private IElementBehavior elementBehavior;
        public IElementBehavior ElementBehaviors
        {
            get
            {
                if (elementBehavior == null)
                {
                    elementBehavior = new ElementBehavior(this);
                }
                return elementBehavior;
            }
        }

        public Control GetChild(string name)
        {
            try
            {
                Interop.IHTMLElement child = ((Interop.IHTMLElement)((Interop.IHTMLElementCollection)this.GetBaseElement().GetChildren()).Item(name, null));
                if (child != null)
                {
                    return this.HtmlEditor.GenericElementFactory.CreateElement(child);
                } 
                else 
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }

        public Control GetChild(int index)
        {
            try
            {
                Interop.IHTMLElement child = ((Interop.IHTMLElement)((Interop.IHTMLElementCollection)this.GetBaseElement().GetChildren()).Item(null, index));
                if (child != null)
                {
                    return this.HtmlEditor.GenericElementFactory.CreateElement(child);
                } 
                else 
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }

        public Control GetChild(string name, int index)
        {
            try {
                Interop.IHTMLElement child = ((Interop.IHTMLElement)((Interop.IHTMLElementCollection)this.GetBaseElement().GetChildren()).Item(name, index));
                if (child != null) {
                    return this.HtmlEditor.GenericElementFactory.CreateElement(child);
                } 
                else {
                    return null;
                }
            }
            catch {
                return null;
            }
        }
        
        [Browsable(false)]
        public string TagName
        {
            get
            {
                return String.Format("{0}:{1}", ((Interop.IHTMLElement2) GetBaseElement()).GetScopeName(), GetBaseElement().GetTagName());
            }
        }

		[Browsable(true)]
		[TypeConverter(typeof(ExpandableObjectConverter))]
        [Category("Element Style")]
        [Description("The effective style as a cascaded combination of global, embedded and inline styles. Readonly.")]        
		public IEffectiveStyle EffectiveStyle
        {
			get
			{   
				Interop.IHTMLCurrentStyle cs = ((Interop.IHTMLElement2)this.GetBaseElement()).GetCurrentStyle();
				CssEffectiveStyle effectiveStyle = null;
				if (cs != null) 
				{   
					effectiveStyle = new CssEffectiveStyle(cs);
				}
				return effectiveStyle;
			}
		}

        private IElementStyle elementStyle;
        private IElementStyle currentStyle;

        [Browsable(true)]
		[TypeConverter(typeof(ExpandableObjectConverter))]
        [Category("Element Style")]
        [Description("The runtime style as CSS inline styles.")]
		public IElementStyle RuntimeStyle
        {
            get
            {                
                if (elementStyle == null) 
                {   
                    Interop.IHTMLStyle cs = this.GetBaseElement().GetStyle();
                    if (cs != null)
                    {
                        elementStyle = new ElementStyle(cs);
                    }
                }
                return elementStyle;
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
                    Interop.IHTMLStyle cs = this.GetBaseElement().GetStyle();
                    if (cs != null)
                    {
                        currentStyle = new ElementStyle(cs);
                    }
                }
                return currentStyle;
            }
        }

		/// <summary>
		/// Returns the absolute coordinates of the element in pixel.
		/// </summary>
		/// <remarks>
		/// This method works even for non absolute positioned elements. Some elements, which have no rectangle
		/// dimensions, may fail returning any useful values.
		/// </remarks>
		/// <returns>A rectangle which describes the dimensions of the client area of the element.</returns>
		public Rectangle GetAbsoluteArea()
		{
			int x = element.GetOffsetLeft();
			int y = element.GetOffsetTop();
			int height= element.GetOffsetHeight();
			int width = element.GetOffsetWidth();
			Interop.IHTMLElement parent = element.GetParentElement();
			while (parent != null)
			{
				if (parent.GetTagName().Equals("TR")) 
				{
					parent = parent.GetParentElement();
					continue;
				}
				x += parent.GetOffsetLeft();
				y += parent.GetOffsetTop();                
				parent = parent.GetParentElement();
			} 
			return new Rectangle(x, y, width, height);
		}
		/// <summary>
		/// The element with this property set to TRUE will be selectable only as a unit.
		/// </summary>
		/// <remarks>
		/// This property is only available 
		/// </remarks>
		[Browsable(false)]
		public bool AtomicSelection
		{
			get
			{
				string sel = GetStringAttribute("ATOMICSELECTION");
				return (sel.ToLower().Equals("true"));
			} 
			set
			{
				SetStringAttribute("ATOMICSELECTION", value ? "true" : "false");
			}
		}

		/// <summary>
		/// Makes the element unselectable, so the user cannot activate it for resizing or moving.
		/// </summary>
		/// <remarks>
		/// The property is ignored if the element is already an unselectable element. Only block elements
		/// like DIV, TABLE, and IMG can be selectable.
		/// </remarks>
		[Browsable(false)]
		public bool Unselectable 
		{ 
			get
			{
				string sel = GetStringAttribute("UNSELECTABLE");
				return (sel.ToLower().Equals("on"));
			} 
			set
			{
				SetStringAttribute("UNSELECTABLE", value ? "on" : "off");
			}
		}
        #endregion

        # region External Events

        /// <summary>
        /// The purpose of this class is to deal with the events a control will
        /// fire at design time.
        /// </summary>
		private sealed class EventSink
		{
                      
			private IElement _element;
			private ConnectionPointCookie _eventSinkCookie;
			private Interop.IHTMLElement _baseElement;

			private class HtmlEvents
			{

				protected Interop.IHTMLEventObj _eventobj;
				protected Interop.IHTMLElement _baseElement;
				protected IElement _element;
				private Interop.IHTMLWindow2 window;

				protected HtmlEvents(IElement _element)
				{
					this._baseElement = ((VmlBaseElement)_element).GetBaseElement();
					this._element = _element;
					Interop.IHTMLDocument2 _document = (Interop.IHTMLDocument2) this._baseElement.GetDocument();
					window = _document.GetParentWindow();
				}

				protected bool GetEventObject()
				{                     
					try
					{        
						// native event object				
						_eventobj = window.@event;
						if (_eventobj != null && _eventobj.srcElement != null && _eventobj.srcElement == _baseElement)
						{
							// check suppressed events
							EventType type = (EventType) Enum.Parse(typeof(EventType), _eventobj.type, true);
							if (_element.HtmlEditor.EventManager.GetEnabled(type))
							{
								//System.Diagnostics.Debug.WriteLine(_eventobj.type, _element.TagName);
								return true;
							}
						}
					}
					catch (Exception ex)
					{
						Debug.WriteLine(ex.Message, "GetEvent::Exception");
					}
					return false;
				}

			}

			/// <summary>
			/// Generic event invocation, curently not used.
			/// </summary>
			private class HtmlGenericEvent : HtmlEvents, Interop.IHTMLGenericEvents
			{

				public HtmlGenericEvent(IElement e) : base(e)
				{
				}

				#region IHTMLGenericEvents Member

				public void Bogus1()
				{
				}

				public void Bogus2()
				{
				}

				public void Bogus3()
				{
				}

				public void Invoke(int id, ref Guid g, int lcid, int dwFlags, Interop.DISPPARAMS pdp, Object[] pvarRes, Interop.EXCEPINFO pei, int[] nArgError)
				{						
					if (!GetEventObject()) return;
					//System.Diagnostics.Debug.WriteLine(_eventobj.type, "------- VmlBaseClass Element: " + _eventobj.srcElement.GetTagName());
					switch (_eventobj.type)
					{
						case "help":
						{
							//((VmlBaseElement) _element).OnOnHelp(this, new DocumentEventArgs(, _element));
							break;
						}

						case "click":
						{
							if (((VmlBaseElement) _element).Click != null/*  && GetEventObject() */)
							{
								((VmlBaseElement) _element).OnClick(_eventobj, _element);
							}
							break;
						}

						case "dblclick":
						{
							if (((VmlBaseElement) _element).DblClick != null/*  && GetEventObject() */)
							{
								((VmlBaseElement) _element).OnDblClick(_eventobj, _element);
							}
							break;
						}

						case "keypress":
						{
							if (GetEventObject())
							{
								((VmlBaseElement) _element).OnKeyDown(_eventobj, _element);                    
								((VmlBaseElement) _element).OnKeyUp(_eventobj, _element);
								((VmlBaseElement) _element).OnKeyPress(_eventobj, _element);
							}
							break;
						}

						case "keydown":
						{
							// see onkeypress":
							break;
						}

						case "keyup":
						{
							// see onkeypress":
							break;
						}

						case "mouseout":
						{                    
							((VmlBaseElement) _element).OnMouseOut(_eventobj, _element);
							break;
						}

						case "mouseover":
						{                    
							((VmlBaseElement) _element).OnMouseOver(_eventobj, _element);
							break;
						}

						case "mousemove":
						{   
							if (((VmlBaseElement) _element).MouseMove != null)
							{
								((VmlBaseElement) _element).OnMouseMove(_eventobj, _element);
							}
							break;
						}

						case "mousedown":
						{    
							if (((VmlBaseElement) _element).MouseDown != null/*  && GetEventObject() */)
							{
								((VmlBaseElement) _element).OnMouseDown(_eventobj, _element);
							}
							break;
						}

						case "mouseup":
						{                    
							if (((VmlBaseElement) _element).MouseUp != null/*  && GetEventObject() */)
							{
								((VmlBaseElement) _element).OnMouseUp(_eventobj, _element);
							}
							break;
						}

						case "selectstart":
						{                    
							if (((VmlBaseElement) _element).SelectStart != null/*  && GetEventObject() */)
							{
								((VmlBaseElement) _element).OnSelectStart(_eventobj, _element);
							}
							break;
						}

						case "filterchange":
						{         
							if (((VmlBaseElement) _element).FilterChange != null/*  && GetEventObject() */)
							{
								((VmlBaseElement) _element).OnFilterChange(_eventobj, _element);
							}
							break;
						}

						case "dragstart":
						{    
							if (((VmlBaseElement) _element).DragStart != null/*  && GetEventObject() */)
							{
								((VmlBaseElement) _element).OnDragStart(_eventobj, _element);
							}
							break;
						}

						case "beforeupdate":
						{                    
							//((VmlBaseElement) _element).OnBeforeUpdate(this, new DocumentEventArgs(, _element));
							break;
						}

						case "afterupdate":
						{
							//((VmlBaseElement) _element).OnAfterUpdate(this, new DocumentEventArgs(, _element));
							break;
						}

						case "errorupdate":
						{
							//((VmlBaseElement) _element).OnErrorUpdate(this, new DocumentEventArgs(, _element));
							break;
						}

						case "rowexit":
						{
							//((VmlBaseElement) _element).OnRowExit(this, new DocumentEventArgs(, _element));
							break;
						}

						case "rowenter":
						{
							//((VmlBaseElement) _element).OnRowEnter(this, new DocumentEventArgs(, _element));
							break;
						}

						case "datasetchanged":
						{
							//((VmlBaseElement) _element).OnDatasetChanged(this, new DocumentEventArgs(, _element));
							break;
						}

						case "dataavailable":
						{
							//((VmlBaseElement) _element).OnDataAvailable(this, new DocumentEventArgs(, _element));
							break;
						}

						case "datasetcomplete":
						{
							//((VmlBaseElement) _element).OnDatasetComplete(this, new DocumentEventArgs(, _element));
							break;
						}

						case "losecapture":
						{         
							if (((VmlBaseElement) _element).LoseCapture != null/*  && GetEventObject() */)
							{
								((VmlBaseElement) _element).OnLoseCapture(_eventobj, _element);
							}
							break;
						}

						case "propertychange":
						{                   
							if (((VmlBaseElement) _element).PropertyChange != null/*  && GetEventObject() */)
							{
								((VmlBaseElement) _element).OnPropertyChange(_eventobj, _element);
							}
							break;
						}

						case "scroll":
						{    
							if (((VmlBaseElement) _element).Scroll != null/*  && GetEventObject() */)
							{
								((VmlBaseElement) _element).OnScroll(_eventobj, _element);
							}
							break;
						}

						case "focus":
						{                    
							if (((VmlBaseElement) _element).Focus != null/*  && GetEventObject() */)
							{
								((VmlBaseElement) _element).OnFocus(_eventobj, _element);
							}
							break;
						}

						case "blur":
						{                    
							if (((VmlBaseElement) _element).Blur != null/*  && GetEventObject() */)
							{
								((VmlBaseElement) _element).OnBlur(_eventobj, _element);
							}
							break;
						}

						case "resize":
						{           
							if (((VmlBaseElement) _element).Resize != null/*  && GetEventObject() */)
							{
								((VmlBaseElement) _element).OnResize(_eventobj, _element);
							}
							break;
						}

						case "drag":
						{
							if (((VmlBaseElement) _element).Drag != null/*  && GetEventObject() */)
							{
								((VmlBaseElement) _element).OnDrag(_eventobj, _element);
							}
							break;
						}

						case "dragend":
						{                    
							if (((VmlBaseElement) _element).DragEnd != null/*  && GetEventObject() */)
							{
								((VmlBaseElement) _element).OnDragEnd(_eventobj, _element);
							}
							break;
						}

						case "dragenter":
						{
							if (((VmlBaseElement) _element).DragEnter != null/*  && GetEventObject() */)
							{
								((VmlBaseElement) _element).OnDragEnter(_eventobj, _element);
							}
							break;
						}

						case "dragover":
						{                    
							if (((VmlBaseElement) _element).DragOver != null/*  && GetEventObject() */)
							{
								((VmlBaseElement) _element).OnDragOver(_eventobj, _element);
							}
							break;
						}

						case "dragleave":
						{                    
							if (((VmlBaseElement) _element).DragLeave != null/*  && GetEventObject() */)
							{
								((VmlBaseElement) _element).OnDragLeave(_eventobj, _element);
							}
							break;
						}

						case "drop":
						{                    
							if (((VmlBaseElement) _element).Drop != null/*  && GetEventObject() */)
							{
								((VmlBaseElement) _element).OnDrop(_eventobj, _element);
							}
							break;
						}

						case "beforecut":
						{                    
							if (((VmlBaseElement) _element).BeforeCut != null/*  && GetEventObject() */)
							{
								((VmlBaseElement) _element).OnBeforeCut(_eventobj, _element);
							}
							break;
						}

						case "cut":
						{                    
							if (((VmlBaseElement) _element).Cut != null/*  && GetEventObject() */)
							{
								((VmlBaseElement) _element).OnCut(_eventobj, _element);
							}
							break;
						}

						case "beforecopy":
						{                    
							if (((VmlBaseElement) _element).BeforeCopy != null/*  && GetEventObject() */)
							{
								((VmlBaseElement) _element).OnBeforeCopy(_eventobj, _element);
							}
							break;
						}

						case "copy":
						{                    
							if (((VmlBaseElement) _element).Copy != null/*  && GetEventObject() */)
							{
								((VmlBaseElement) _element).OnCopy(_eventobj, _element);
							}
							break;
						}

						case "beforepaste":
						{
							if (((VmlBaseElement) _element).BeforePaste != null/*  && GetEventObject() */)
							{
								((VmlBaseElement) _element).OnBeforePaste(_eventobj, _element);
							}
							break;
						}

						case "paste":
						{                    
							if (((VmlBaseElement) _element).Paste != null/*  && GetEventObject() */)
							{
								((VmlBaseElement) _element).OnPaste(_eventobj, _element);
							}
							break;
						}

						case "contextmenu":
						{                  
							if (((VmlBaseElement) _element).ContextMenu != null/*  && GetEventObject() */)
							{
								((VmlBaseElement) _element).OnContextMenu(_eventobj, _element);
							}
							break;
						}

						case "rowsdelete":
						{
							//((VmlBaseElement) _element).OnRowsDelete(this, new DocumentEventArgs(, _element));
							break;
						}

						case "rowsinserted":
						{
							//((VmlBaseElement) _element).OnRowsInserted(this, new DocumentEventArgs(, _element));
							break;
						}

						case "cellchange":
						{
							//((VmlBaseElement) _element).OnCellChange(this, new DocumentEventArgs(, _element));
							break;
						}

						case "readystatechange":
						{
							//((VmlBaseElement) _element).OnReadyStateChange(this, new DocumentEventArgs(, _element));
							break;
						}

						case "beforeeditfocus":
						{                    
							if (((VmlBaseElement) _element).EditFocus != null/*  && GetEventObject() */)
							{
								((VmlBaseElement) _element).OnEditFocus(_eventobj, _element);
							}
							break;
						}

						case "layoutcomplete":
						{                    
							if (((VmlBaseElement) _element).LayoutComplete != null/*  && GetEventObject() */)
							{
								((VmlBaseElement) _element).OnLayoutComplete(_eventobj, _element);
							}
							break;
						}

						case "page":
						{   
							if (((VmlBaseElement) _element).Paged != null/*  && GetEventObject() */)
							{
								((VmlBaseElement) _element).OnPage(_eventobj, _element);
							}
							break;
						}

						case "beforedeactivate":
						{                    
							if (((VmlBaseElement) _element).BeforeDeactivate != null/*  && GetEventObject() */)
							{
								((VmlBaseElement) _element).OnBeforeDeactivate(_eventobj, _element);
							}
							break;
						}

						case "beforeactivate":
						{
							if (((VmlBaseElement) _element).BeforeActivate != null/*  && GetEventObject() */)
							{
								((VmlBaseElement) _element).OnBeforeActivate(_eventobj, _element);
							}
							break;
						}

						case "move":
						{    
							if (((VmlBaseElement) _element).Move != null/*  && GetEventObject() */)
							{
								((VmlBaseElement) _element).OnMove(_eventobj, _element);
							}
							break;
						}

						case "controlselect":
						{    
							if (((VmlBaseElement) _element).ControlSelect != null/*  && GetEventObject() */)
							{
								((VmlBaseElement) _element).OnControlSelect(_eventobj, _element);							
							}
							break;
						}

						case "movestart":
						{                    
							if (((VmlBaseElement) _element).MoveStart != null/*  && GetEventObject() */)
							{
								((VmlBaseElement) _element).OnMoveStart(_eventobj, _element);
							}
							break;
						}

						case "moveend":
						{                    
							if (((VmlBaseElement) _element).MoveEnd != null/*  && GetEventObject() */)
							{
								((VmlBaseElement) _element).OnMoveEnd(_eventobj, _element);
							}
							break;
						}

						case "resizestart":
						{
							if (((VmlBaseElement) _element).ResizeStart != null/*  && GetEventObject() */)
							{
								((VmlBaseElement) _element).OnResizeStart(_eventobj, _element);
							}
							break;
						}

						case "resizeend":
						{
							if (((VmlBaseElement) _element).ResizeEnd != null/*  && GetEventObject() */)
							{
								((VmlBaseElement) _element).OnResizeEnd(_eventobj, _element);
							}
							break;
						}

						case "mouseenter":
						{   
							if (((VmlBaseElement) _element).MouseEnter != null/*  && GetEventObject() */)
							{
								((VmlBaseElement) _element).OnMouseEnter(_eventobj, _element);
							}
							break;
						}

						case "mouseleave":
						{
							if (((VmlBaseElement) _element).MouseLeave != null/*  && GetEventObject() */)
							{
								((VmlBaseElement) _element).OnMouseLeave(_eventobj, _element);
							}
							break;
						}

						case "mousewheel":
						{
							if (((VmlBaseElement) _element).MouseWheel != null/*  && GetEventObject() */)
							{
								((VmlBaseElement) _element).OnMouseWheel(_eventobj, _element);
							}
							break;
						}

						case "activate":
						{
							if (((VmlBaseElement) _element).Activate != null/*  && GetEventObject() */)
							{
								((VmlBaseElement) _element).OnActivate(_eventobj, _element);
							}
							break;
						}

						case "deactivate":
						{
							if (((VmlBaseElement) _element).Deactivate != null/*  && GetEventObject() */)
							{
								((VmlBaseElement) _element).OnDeactivate(_eventobj, _element);
							}
							break;
						}

						case "focusin":
						{
							if (((VmlBaseElement) _element).FocusIn != null/*  && GetEventObject() */)
							{
								((VmlBaseElement) _element).OnFocusIn(_eventobj, _element);
							}
							break;
						}

						case "focusout":
						{
							if (((VmlBaseElement) _element).FocusOut != null/*  && GetEventObject() */)
							{
								((VmlBaseElement) _element).OnFocusOut(_eventobj, _element);
							}
							break;
						}

						case "load":
						{
							if (((VmlBaseElement) _element).Load != null/*  && GetEventObject() */)
							{
								((VmlBaseElement) _element).OnLoad(_eventobj, _element);
							}
							break;
						}

						case "error":
						{
							if (((VmlBaseElement) _element).Error != null/*  && GetEventObject() */)
							{
								((VmlBaseElement) _element).OnError(_eventobj, _element);
							}
							break;
						}

						case "change":
						{
							if (((VmlBaseElement) _element).Change != null/*  && GetEventObject() */)
							{
								((VmlBaseElement) _element).OnChange(_eventobj, _element);
							}
							break;
						}


						case "abort":
						{                    
							if (((VmlBaseElement) _element).Abort != null/*  && GetEventObject() */)
							{
								((VmlBaseElement) _element).OnAbort(_eventobj, _element);
							}
							break;
						}


						case "select":
						{
							if (((VmlBaseElement) _element).Select != null/*  && GetEventObject() */)
							{
								((VmlBaseElement) _element).OnSelect(_eventobj, _element);
							}
							break;
						}

						case "selectionchange":
						{
							if (((VmlBaseElement) _element).SelectionChange != null/*  && GetEventObject() */)
							{
								((VmlBaseElement) _element).OnSelectionChange(_eventobj, _element);
							}
							break;
						}

						case "stop":
						{
							if (((VmlBaseElement) _element).Stop != null/*  && GetEventObject() */)
							{
								((VmlBaseElement) _element).OnStop(_eventobj, _element);
							}
							break;
						}

						case "reset":
						{
							break;
						}

						case "submit":
						{
							break;
						}
					}
				}

				#endregion

			}
            
			
			public EventSink(IElement element)
			{
				this._element = element;                
			}

			/// <summary>
			/// Connects the specified control and its underlying element to the event sink.
			/// </summary>
			public void Connect()
			{
				this._baseElement = ((VmlBaseElement) this._element).GetBaseElement();
				Guid guid = typeof(Interop.IHTMLGenericEvents).GUID;
				HtmlGenericEvent genericEvents = new HtmlGenericEvent(_element);
				this._eventSinkCookie = new ConnectionPointCookie(this._baseElement, genericEvents, guid, false);
			}

			public void Disconnect()
			{
				if (this._eventSinkCookie != null)
				{
					this._eventSinkCookie.Disconnect();
					this._eventSinkCookie = null;
				}
				this._element = null;
			}

		}

		public event DocumentEventHandler LoseCapture;
        /// <summary>
        /// Invokes the element's event.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="srcElement"></param>
        internal protected void OnLoseCapture(Interop.IHTMLEventObj e, IElement srcElement)
        {
            if (LoseCapture != null)
            {
                LoseCapture(this, new DocumentEventArgs(e, srcElement));
            }
        }

        /// <summary>
        /// Fired if the user clicks on the element in design mode.
        /// </summary>
        public event DocumentEventHandler Click;
        /// <summary>
        /// Invokes the element's event.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="srcElement"></param>
        internal protected void OnClick(Interop.IHTMLEventObj e, IElement srcElement)
        {
            if (Click != null)
            {
                Click(this, new DocumentEventArgs(e, srcElement));
            }
        }

        /// <summary>
        /// Fired if the user double clicks on the element in design mode.
        /// </summary>
        public event DocumentEventHandler DblClick;
        /// <summary>
        /// Invokes the element's event.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="srcElement"></param>
        internal protected void OnDblClick(Interop.IHTMLEventObj e, IElement srcElement)
        {
            if (DblClick != null)
            {
                DblClick(this, new DocumentEventArgs(e, srcElement));
            }
        }

        /// <summary>
        /// Fired if the user starts dragging the in design mode.
        /// </summary>
        public event DocumentEventHandler DragStart;
        /// <summary>
        /// Invokes the element's event.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="srcElement"></param>
        internal protected void OnDragStart(Interop.IHTMLEventObj e, IElement srcElement)
        {
            if (DragStart != null)
            {
                DragStart(this, new DocumentEventArgs(e, srcElement));
            }
        }

        /// <summary>
        /// Fired if the elemens receives the focus.
        /// </summary>
        public new event DocumentEventHandler Focus;
        /// <summary>
        /// Invokes the element's event.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="srcElement"></param>
        internal protected void OnFocus(Interop.IHTMLEventObj e, IElement srcElement)
        {
            if (Focus != null)
            {
                Focus(this, new DocumentEventArgs(e, srcElement));
            }
        }

        /// <summary>
        /// Fired if the elemens receives the focus.
        /// </summary>
        public event DocumentEventHandler Drop;
        /// <summary>
        /// Invokes the element's event.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="srcElement"></param>
        internal protected void OnDrop(Interop.IHTMLEventObj e, IElement srcElement)
        {
            if (Drop != null)
            {
                Drop(this, new DocumentEventArgs(e, srcElement));
            }
        }

        /// <summary>
        /// Fired if the elemens receives the focus.
        /// </summary>
        public event DocumentEventHandler Blur;
        /// <summary>
        /// Invokes the element's event.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="srcElement"></param>
        internal protected void OnBlur(Interop.IHTMLEventObj e, IElement srcElement)
        {
            if (Blur != null)
            {
                Blur(this, new DocumentEventArgs(e, srcElement));
            }
        }

        /// <summary>
        /// Fired if the elemens receives the focus.
        /// </summary>
        public event DocumentEventHandler DragOver;
        /// <summary>
        /// Invokes the element's event.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="srcElement"></param>
        internal protected void OnDragOver(Interop.IHTMLEventObj e, IElement srcElement)
        {
            if (DragOver != null)
            {
                DragOver(this, new DocumentEventArgs(e, srcElement));
            }
        }

        /// <summary>
        /// Fired if the elemens receives the focus.
        /// </summary>
        public event DocumentEventHandler DragEnter;
        /// <summary>
        /// Invokes the element's event.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="srcElement"></param>
        internal protected void OnDragEnter(Interop.IHTMLEventObj e, IElement srcElement)
        {
            if (DragEnter != null)
            {
                DragEnter(this, new DocumentEventArgs(e, srcElement));
            }
        }

        /// <summary>
        /// Fired if the elemens receives the focus.
        /// </summary>
        public event DocumentEventHandler DragLeave;
        /// <summary>
        /// Invokes the element's event.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="srcElement"></param>
        internal protected void OnDragLeave(Interop.IHTMLEventObj e, IElement srcElement)
        {
            if (DragLeave != null)
            {
                DragLeave(this, new DocumentEventArgs(e, srcElement));
            }
        }

        /// <summary>
        /// Fired if the user hits a key down on the element in design mode.
        /// </summary>
        public event DocumentEventHandler KeyDown;
        /// <summary>
        /// Invokes the element's event.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="srcElement"></param>
        internal protected void OnKeyDown(Interop.IHTMLEventObj e, IElement srcElement)
        {
            if (KeyDown != null)
            {
                KeyDown(this, new DocumentEventArgs(e, srcElement));
            }
        }

        /// <summary>
        /// Fired if the user pressed a key element in design mode.
        /// </summary>
        public event DocumentEventHandler KeyPress;
        /// <summary>
        /// Invokes the element's event.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="srcElement"></param>
        internal protected void OnKeyPress(Interop.IHTMLEventObj e, IElement srcElement)
        {
            if (KeyPress != null)
            {
                KeyPress(this, new DocumentEventArgs(e, srcElement));
            }
        }

        /// <summary>
        /// Fired if the user hits and releases a key on the element in design mode.
        /// </summary>
        public event DocumentEventHandler KeyUp;
        /// <summary>
        /// Invokes the element's event.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="srcElement"></param>
        internal protected void OnKeyUp(Interop.IHTMLEventObj e, IElement srcElement)
        {
            if (KeyUp != null)
            {
                KeyUp(this, new DocumentEventArgs(e, srcElement));
            }
        }
        /// <summary>
        /// Fired if the user clicks a mouse button on the element in design mode.
        /// </summary>
        public event DocumentEventHandler MouseDown;
        /// <summary>
        /// Invokes the element's event.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="srcElement"></param>
        internal protected void OnMouseDown(Interop.IHTMLEventObj e, IElement srcElement)
        {
            if (MouseDown != null)
            {
                MouseDown(this, new DocumentEventArgs(e, srcElement));
            }
        }		/// <summary>
        /// Sets or removes an event handler function that fires when the user begins to change the dimensions of the object.
        /// </summary>
        /// <remarks>
        /// This event handler will not suppress the embedded resizing behavior.
        /// </remarks>
        public event DocumentEventHandler ResizeStart;
        /// <summary>
        /// Invokes the element's event.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="srcElement"></param>
        internal protected void OnResizeStart(Interop.IHTMLEventObj e, IElement srcElement)
        {
            if (ResizeStart != null)
            {
                ResizeStart(this, new DocumentEventArgs(e, srcElement));
            }
        }

        /// <summary>
        /// Sets or removes an event handler function that fires when the user has finished changing the dimensions of the object.
        /// </summary>
        /// <remarks>
        /// This event handler will not suppress the embedded resizing behavior.
        /// </remarks>
        public event DocumentEventHandler ResizeEnd;
        /// <summary>
        /// Invokes the element's event.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="srcElement"></param>
        internal protected void OnResizeEnd(Interop.IHTMLEventObj e, IElement srcElement)
        {
            if (ResizeEnd != null)
            {
                ResizeEnd(this, new DocumentEventArgs(e, srcElement));
            }
        }
        /// <summary>
        /// Sets or removes an event handler function that fires when the user moves the mouse pointer into the object.
        /// </summary>
        /// <remarks>
        /// Unlike the OnMouseOver event, the OnMouseEnter event does not bubble. In other words, the OnMouseEnter 
        /// event does not fire when the user moves the mouse pointer over elements contained by the object, 
        /// whereas <see cref="OnMouseOver">OnMouseOver</see> does fire. 
        /// </remarks>
        public event DocumentEventHandler MouseEnter;
        /// <summary>
        /// Invokes the element's event.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="srcElement"></param>
        internal protected void OnMouseEnter(Interop.IHTMLEventObj e, IElement srcElement)
        {
            if (MouseEnter != null)
            {
                MouseEnter(this, new DocumentEventArgs(e, srcElement));
            }
        }

        /// <summary>
        /// Sets or retrieves a pointer to the event handler function that fires, when the user moves the mouse pointer outside 
        /// the boundaries of the object.</summary>
        /// <remarks>Use in correspondence to OnMouseEnter.</remarks>
        public event DocumentEventHandler MouseLeave;
        /// <summary>
        /// Invokes the element's event.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="srcElement"></param>
        internal protected void OnMouseLeave(Interop.IHTMLEventObj e, IElement srcElement)
        {
            if (MouseLeave != null)
            {
                MouseLeave(this, new DocumentEventArgs(e, srcElement));
            }
        }

        /// <summary>
        /// Fired if the user moves the mouse over the element area in design mode.
        /// </summary>
        public event DocumentEventHandler MouseMove;
        /// <summary>
        /// Invokes the element's event.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="srcElement"></param>
        internal protected void OnMouseMove(Interop.IHTMLEventObj e, IElement srcElement)
        {
            if (MouseMove != null)
            {
                MouseMove(this, new DocumentEventArgs(e, srcElement));
            }
        }

        /// <summary>
        /// Fired if the user mouse has left the element area with the mouse in design mode.
        /// </summary>
        /// <example>
        /// To use this event to show the content of a link the mouse pointer is over, use this code:
        /// <code>
        /// ArrayList anchors = this.htmlEditor2.DocumentProperties.GetElementCollection("A") as ArrayList;
        /// if (anchors != null)
        /// {
        ///    foreach (AnchorElement a in anchors)
        ///    {
        ///        a.OnMouseOver -= new Events.DocumentEventHandler(a_OnMouseOver);
        ///        a.OnMouseOver += new Events.DocumentEventHandler(a_OnMouseOver);
        ///        a.OnMouseOut -= new Events.DocumentEventHandler(a_OnMouseOut);
        ///        a.OnMouseOut += new Events.DocumentEventHandler(a_OnMouseOut);
        ///    }
        /// }</code>
        /// Place this code in the <see cref="GuruComponents.Netrix.HtmlEditor.ReadyStateComplete">ReadyStateComplete</see> event.
        /// The handler are now able to do something with the element.
        /// <code>
        ///private void a_OnMouseOut(GuruComponents.Netrix.Events.DocumentEventArgs e)
        ///{
        ///  AnchorElement a = e.SrcElement as AnchorElement;
        ///  if (a != null)
        ///  {
        ///     this.statusBarPanelLinks.Text = "";
        ///  }
        ///}</code>
        /// <code>
        ///private void a_OnMouseOver(GuruComponents.Netrix.Events.DocumentEventArgs e)
        ///{
        ///   AnchorElement a = e.SrcElement as AnchorElement;
        ///   if (a != null)
        ///   {
        ///      this.statusBarPanelLinks.Text = a.href;
        ///   }
        ///}</code>
        /// </example>
        public event DocumentEventHandler MouseOut;
        /// <summary>
        /// Invokes the element's event.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="srcElement"></param>
        internal protected void OnMouseOut(Interop.IHTMLEventObj e, IElement srcElement)
        {
            if (MouseOut != null)
            {
                MouseOut(this, new DocumentEventArgs(e, srcElement));
            }
        }

        /// <summary>
        /// Fired if the user has entered the element area with the mouse in design mode.
        /// </summary>
        /// <example>
        /// To use this event to show the content of a link the mouse pointer is over, use this code:
        /// <code>
        /// ArrayList anchors = this.htmlEditor2.DocumentProperties.GetElementCollection("A") as ArrayList;
        /// if (anchors != null)
        /// {
        ///    foreach (AnchorElement a in anchors)
        ///    {
        ///        a.OnMouseOver -= new Events.DocumentEventHandler(a_OnMouseOver);
        ///        a.OnMouseOver += new Events.DocumentEventHandler(a_OnMouseOver);
        ///        a.OnMouseOut -= new Events.DocumentEventHandler(a_OnMouseOut);
        ///        a.OnMouseOut += new Events.DocumentEventHandler(a_OnMouseOut);
        ///    }
        /// }</code>
        /// Place this code in the <see cref="GuruComponents.Netrix.HtmlEditor.ReadyStateComplete">ReadyStateComplete</see> event.
        /// The handler are now able to do something with the element.
        /// <code>
        ///private void a_OnMouseOut(GuruComponents.Netrix.Events.DocumentEventArgs e)
        ///{
        ///  AnchorElement a = e.SrcElement as AnchorElement;
        ///  if (a != null)
        ///  {
        ///     this.statusBarPanelLinks.Text = "";
        ///  }
        ///}</code>
        /// <code>
        ///private void a_OnMouseOver(GuruComponents.Netrix.Events.DocumentEventArgs e)
        ///{
        ///   AnchorElement a = e.SrcElement as AnchorElement;
        ///   if (a != null)
        ///   {
        ///      this.statusBarPanelLinks.Text = a.href;
        ///   }
        ///}</code>
        /// </example>
        public event DocumentEventHandler MouseOver;
        /// <summary>
        /// Invokes the element's event.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="srcElement"></param>
        internal protected void OnMouseOver(Interop.IHTMLEventObj e, IElement srcElement)
        {
            if (MouseOver != null)
            {
                MouseOver(this, new DocumentEventArgs(e, srcElement));
            }
        }

        /// <summary>
        /// Fired if the user releases the mouse button over the element area in design mode.
        /// </summary>
        public event DocumentEventHandler MouseUp;
        /// <summary>
        /// Invokes the element's event.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="srcElement"></param>
        internal protected void OnMouseUp(Interop.IHTMLEventObj e, IElement srcElement)
        {
            if (MouseUp != null)
            {
                MouseUp(this, new DocumentEventArgs(e, srcElement));
            }
        }

        /// <summary>
        /// Fired if the user starts selecting the element area in design mode.
        /// </summary>
        public event DocumentEventHandler SelectStart;

        /// <summary>
        /// Invokes the element's event.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="srcElement"></param>
        internal protected void OnSelectStart(Interop.IHTMLEventObj e, IElement srcElement)
        {
            if (SelectStart != null)
            {
                SelectStart(this, new DocumentEventArgs(e, srcElement));
            }
        }

        public event DocumentEventHandler LayoutComplete;

        /// <summary>
        /// Invokes the element's event.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="srcElement"></param>
        internal protected void OnLayoutComplete(Interop.IHTMLEventObj e, IElement srcElement)
        {
            if (LayoutComplete != null)
            {
                LayoutComplete(this, new DocumentEventArgs(e, srcElement));
            }
        }

        public new event DocumentEventHandler Load;

        /// <summary>
        /// Invokes the element's event.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="srcElement"></param>
        internal protected void OnLoad(Interop.IHTMLEventObj e, IElement srcElement)
        {
            if (Load != null)
            {
                Load(this, new DocumentEventArgs(e, srcElement));
            }
        }

        public event DocumentEventHandler MouseWheel;

        /// <summary>
        /// Invokes the element's event.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="srcElement"></param>
        internal protected void OnMouseWheel(Interop.IHTMLEventObj e, IElement srcElement)
        {
            if (MouseWheel != null)
            {
                MouseWheel(this, new DocumentEventArgs(e, srcElement));
            }
        }


        public event DocumentEventHandler MoveEnd;

        /// <summary>
        /// Invokes the element's event.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="srcElement"></param>
        internal protected void OnMoveEnd(Interop.IHTMLEventObj e, IElement srcElement)
        {
            if (MoveEnd != null)
            {
                MoveEnd(this, new DocumentEventArgs(e, srcElement));
            }
        }


        public event DocumentEventHandler MoveStart;

        /// <summary>
        /// Invokes the element's event.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="srcElement"></param>
        internal protected void OnMoveStart(Interop.IHTMLEventObj e, IElement srcElement)
        {
            if (MoveStart != null)
            {
                MoveStart(this, new DocumentEventArgs(e, srcElement));
            }
        }

        public event DocumentEventHandler Activate;

        /// <summary>
        /// Invokes the element's event.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="srcElement"></param>
        internal protected void OnActivate(Interop.IHTMLEventObj e, IElement srcElement)
        {
            if (Activate != null)
            {
                Activate(this, new DocumentEventArgs(e, srcElement));
            }
        }

        public event DocumentEventHandler BeforeActivate;

        /// <summary>
        /// Invokes the element's event.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="srcElement"></param>
        internal protected void OnBeforeActivate(Interop.IHTMLEventObj e, IElement srcElement)
        {
            if (BeforeActivate != null)
            {
                BeforeActivate(this, new DocumentEventArgs(e, srcElement));
            }
        }

        public event DocumentEventHandler BeforeCopy;

        /// <summary>
        /// Invokes the element's event.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="srcElement"></param>
        internal protected void OnBeforeCopy(Interop.IHTMLEventObj e, IElement srcElement)
        {
            if (BeforeCopy != null)
            {
                BeforeCopy(this, new DocumentEventArgs(e, srcElement));
            }
        }

        public event DocumentEventHandler BeforeCut;

        /// <summary>
        /// Invokes the element's event.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="srcElement"></param>
        internal protected void OnBeforeCut(Interop.IHTMLEventObj e, IElement srcElement)
        {
            if (BeforeCut != null)
            {
                BeforeCut(this, new DocumentEventArgs(e, srcElement));
            }
        }

        public event DocumentEventHandler BeforePaste;

        /// <summary>
        /// Invokes the element's event.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="srcElement"></param>
        internal protected void OnBeforePaste(Interop.IHTMLEventObj e, IElement srcElement)
        {
            if (BeforePaste != null)
            {
                BeforePaste(this, new DocumentEventArgs(e, srcElement));
            }
        }


        public event DocumentEventHandler ContextMenu;

        /// <summary>
        /// Invokes the element's event.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="srcElement"></param>
        internal protected void OnContextMenu(Interop.IHTMLEventObj e, IElement srcElement)
        {
            if (ContextMenu != null)
            {
                ContextMenu(this, new DocumentEventArgs(e, srcElement));
            }
        }

        public event DocumentEventHandler Copy;

        /// <summary>
        /// Invokes the element's event.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="srcElement"></param>
        internal protected void OnCopy(Interop.IHTMLEventObj e, IElement srcElement)
        {
            if (Copy != null)
            {
                Copy(this, new DocumentEventArgs(e, srcElement));
            }
        }

        public event DocumentEventHandler Cut;

        /// <summary>
        /// Invokes the element's event.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="srcElement"></param>
        internal protected void OnCut(Interop.IHTMLEventObj e, IElement srcElement)
        {
            if (Cut != null)
            {
                Cut(this, new DocumentEventArgs(e, srcElement));
            }
        }

        public event DocumentEventHandler Deactivate;

        internal protected void OnDeactivate(Interop.IHTMLEventObj e, IElement srcElement)
        {
            if (Deactivate != null)
            {
                Deactivate(this, new DocumentEventArgs(e, srcElement));
            }
        }

        public event DocumentEventHandler Drag;

        /// <summary>
        /// Invokes the element's event.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="srcElement"></param>
        internal protected void OnDrag(Interop.IHTMLEventObj e, IElement srcElement)
        {
            if (Drag != null)
            {
                Drag(this, new DocumentEventArgs(e, srcElement));
            }
        }

        public event DocumentEventHandler DragEnd;

        /// <summary>
        /// Invokes the element's event.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="srcElement"></param>
        internal protected void OnDragEnd(Interop.IHTMLEventObj e, IElement srcElement)
        {
            if (DragEnd != null)
            {
                DragEnd(this, new DocumentEventArgs(e, srcElement));
            }
        }

        public event DocumentEventHandler FocusIn;

        /// <summary>
        /// Invokes the element's event.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="srcElement"></param>
        internal protected void OnFocusIn(Interop.IHTMLEventObj e, IElement srcElement)
        {
            if (FocusIn != null)
            {
                FocusIn(this, new DocumentEventArgs(e, srcElement));
            }
        }

        public event DocumentEventHandler FocusOut;

        /// <summary>
        /// Invokes the element's event.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="srcElement"></param>
        internal protected void OnFocusOut(Interop.IHTMLEventObj e, IElement srcElement)
        {
            if (FocusOut != null)
            {
                FocusOut(this, new DocumentEventArgs(e, srcElement));
            }
        }

        public event DocumentEventHandler FilterChange;

        /// <summary>
        /// Invokes the element's event.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="srcElement"></param>
        internal protected void OnFilterChange(Interop.IHTMLEventObj e, IElement srcElement)
        {
            if (FilterChange != null)
            {
                FilterChange(this, new DocumentEventArgs(e, srcElement));
            }
        }

        public event DocumentEventHandler Abort;

        /// <summary>
        /// Invokes the element's event.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="srcElement"></param>
        internal protected void OnAbort(Interop.IHTMLEventObj e, IElement srcElement)
        {
            if (Abort != null)
            {
                Abort(this, new DocumentEventArgs(e, srcElement));
            }
        }


        public event DocumentEventHandler Change;

        /// <summary>
        /// Invokes the element's event.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="srcElement"></param>
        internal protected void OnChange(Interop.IHTMLEventObj e, IElement srcElement)
        {
            if (Change != null)
            {
                Change(this, new DocumentEventArgs(e, srcElement));
            }
        }

        public event DocumentEventHandler Select;

        /// <summary>
        /// Invokes the element's event.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="srcElement"></param>
        internal protected void OnSelect(Interop.IHTMLEventObj e, IElement srcElement)
        {
            if (Select != null)
            {
                Select(this, new DocumentEventArgs(e, srcElement));
            }
        }

        public event DocumentEventHandler SelectionChange;

        /// <summary>
        /// Invokes the element's event.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="srcElement"></param>
        internal protected void OnSelectionChange(Interop.IHTMLEventObj e, IElement srcElement)
        {
            if (SelectionChange != null)
            {
                SelectionChange(this, new DocumentEventArgs(e, srcElement));
            }
        }

        public event DocumentEventHandler Stop;

        /// <summary>
        /// Invokes the element's event.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="srcElement"></param>
        internal protected void OnStop(Interop.IHTMLEventObj e, IElement srcElement)
        {
            if (Stop != null)
            {
                Stop(this, new DocumentEventArgs(e, srcElement));
            }
        }

        public event DocumentEventHandler BeforeDeactivate;

        /// <summary>
        /// Invokes the element's event.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="srcElement"></param>
        internal protected void OnBeforeDeactivate(Interop.IHTMLEventObj e, IElement srcElement)
        {
            if (BeforeDeactivate != null)
            {
                BeforeDeactivate(this, new DocumentEventArgs(e, srcElement));
            }
        }

        public event DocumentEventHandler ControlSelect;

        internal protected void OnControlSelect(Interop.IHTMLEventObj e, IElement srcElement)
        {
            if (ControlSelect != null)
            {
                ControlSelect(this, new DocumentEventArgs(e, srcElement));
            }
        }

        public event DocumentEventHandler EditFocus;

        /// <summary>
        /// Invokes the element's event.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="srcElement"></param>
        internal protected void OnEditFocus(Interop.IHTMLEventObj e, IElement srcElement)
        {
            if (EditFocus != null)
            {
                EditFocus(this, new DocumentEventArgs(e, srcElement));
            }
        }

        public event DocumentEventHandler Error;

        /// <summary>
        /// Invokes the element's event.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="srcElement"></param>
        internal protected void OnError(Interop.IHTMLEventObj e, IElement srcElement)
        {
            if (Error != null)
            {
                Error(this, new DocumentEventArgs(e, srcElement));
            }
        }

        public event DocumentEventHandler Move;

        /// <summary>
        /// Invokes the element's event.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="srcElement"></param>
        internal protected void OnMove(Interop.IHTMLEventObj e, IElement srcElement)
        {
            if (Move != null)
            {
                Move(this, new DocumentEventArgs(e, srcElement));
            }
        }

        public event DocumentEventHandler Paste;

        /// <summary>
        /// Invokes the element's event.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="srcElement"></param>
        internal protected void OnPaste(Interop.IHTMLEventObj e, IElement srcElement)
        {
            if (Paste != null)
            {
                Paste(this, new DocumentEventArgs(e, srcElement));
            }
        }

        public event DocumentEventHandler PropertyChange;

        /// <summary>
        /// Invokes the element's event.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="srcElement"></param>
        internal protected void OnPropertyChange(Interop.IHTMLEventObj e, IElement srcElement)
        {
            if (PropertyChange != null)
            {
                PropertyChange(this, new DocumentEventArgs(e, srcElement));
            }
        }

        public event DocumentEventHandler Resize;

        /// <summary>
        /// Invokes the element's event.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="srcElement"></param>
        internal protected void OnResize(Interop.IHTMLEventObj e, IElement srcElement)
        {
            if (Resize != null)
            {
                Resize(this, new DocumentEventArgs(e, srcElement));
            }
        }
        /// <summary>
        /// Fires if scrolled
        /// </summary>
        public event DocumentEventHandler Scroll;

        /// <summary>
        /// Invokes the element's event.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="srcElement"></param>
        internal protected void OnScroll(Interop.IHTMLEventObj e, IElement srcElement)
        {
            if (Scroll != null)
            {
                Scroll(this, new DocumentEventArgs(e, srcElement));
            }
        }
        /// <summary>
        /// Not used.
        /// </summary>
        public event DocumentEventHandler Paged;

        /// <summary>
        /// Invokes the element's event.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="srcElement"></param>
        internal protected void OnPage(Interop.IHTMLEventObj e, IElement srcElement)
        {
            if (Paged != null)
            {
                Paged(this, new DocumentEventArgs(e, srcElement));
            }
        }

        # endregion
                
        #region IComponent Members

        public new event EventHandler Disposed;

        private ISite site;

        public new ISite Site
        {
            get
            {
                return site;
            }
            set
            {
                site = value;
            }
        }

        #endregion

        #region IDisposable Members

        public new void Dispose()
        {
            _eventSink.Disconnect();
            _eventSink = null;
            if (Disposed != null)
            {
                Disposed(this, EventArgs.Empty);
            }
        }

        #endregion


        #region IElement Members


        public string Alias
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public string ElementName
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        #endregion

        #region IElement Members


        public bool IsAbsolutePositioned
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public bool IsTextEdit
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }
        
        public System.Web.UI.CssStyleCollection Style
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
        #endregion
    }
}
