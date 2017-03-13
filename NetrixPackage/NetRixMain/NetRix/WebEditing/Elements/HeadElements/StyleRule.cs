using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Web.UI.WebControls;
using GuruComponents.Netrix.WebEditing;
using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.WebEditing.Documents;
using GuruComponents.Netrix.WebEditing.Styles;
using GuruComponents.Netrix.WebEditing.Elements;

namespace GuruComponents.Netrix.WebEditing.Styles
{
    /// <summary>
    /// The purpose of this class is to hold one rule definition. This class implements 
    /// <see cref="GuruComponents.Netrix.WebEditing.Styles.IStyleRule">IStyleRule</see> interface.
    /// </summary>
    /// <remarks>
    /// This class supports the NetRix infrastructure. It is used to create new style rules using the
    /// integrated UI.
    /// </remarks>
    public class StyleRule : GuruComponents.Netrix.WebEditing.Styles.IStyleRule, System.ComponentModel.ICustomTypeDescriptor
    {

        private string selectorname = String.Empty;

        [Browsable(false)]
        private string SelectorName
        {
            get { return selectorname; }
            set 
            { 
                selectorname = value; 
                //nativeRule.SetSelectorText(value); // not supported!
            }
        }
        private StyleRuleType selectorType = StyleRuleType.Standard;
        private Interop.IHTMLRuleStyle styleRule;
        Interop.IHTMLStyleSheetRule nativeRule;

        /// <summary>
        /// Gets or sets the type of selector.
        /// </summary>
        /// <remarks>
        /// See <see cref="GuruComponents.Netrix.WebEditing.Elements.StyleRuleType">StyleRuleType</see> for a list
        /// of possible rule types. The rule type will be detected by checking the first character:
        /// <list type="bullet">
        /// <item><term>.</term><description>Class</description></item>
        /// <item><term>#</term><description>Id</description></item>
        /// <item><term>@</term><description>Pseudo class</description></item>
        /// <item><term>HTML tag name</term><description>Behavior of that tag</description></item>
        /// </list>
        /// Changing the type will change the prefix character of the whole name if necessary.
        /// </remarks>
        [DescriptionAttribute("")]
        [DefaultValueAttribute("")]        
        [RefreshProperties(RefreshProperties.Repaint)]
        [Browsable(true)]
        [TypeConverter(typeof(GuruComponents.Netrix.UserInterface.TypeConverters.UITypeConverterDropList))]
        [GuruComponents.Netrix.UserInterface.TypeEditors.DisplayName()]
        public GuruComponents.Netrix.WebEditing.Elements.StyleRuleType SelectorType
        {
            set
            {
                selectorType = value;
                string st = (SelectorName == null) ? String.Empty : SelectorName;
                if (st.Length > 0)
                {
                    if (st[0] == '.' || st[0] == '#' || st[0] == '@')
                    {
                        st = st.Substring(1);
                    }
                    switch (selectorType)
                    {
                        case StyleRuleType.Class:
                            if (!st.StartsWith("."))
                            {
                                st = String.Concat(".", st);
                            }
                            break;
                        case StyleRuleType.ID:
                            if (!st.StartsWith("#"))
                            {
                                st = String.Concat("#", st);
                            }
                            break;
                        case StyleRuleType.Pseudo:
                            if (!st.StartsWith("@"))
                            {
                                st = String.Concat("@", st);
                            }
                            break;
                        case StyleRuleType.Global:
                            st = "*";
                            break;
                    }
                }
                SelectorName = st;
            }
      
            get
            {
                string dummy = RuleName;
                return selectorType;
            } 
      
        }

        /// <summary>
		/// Gets or sets the name of the rule, including the preceding type sign.
		/// </summary>
        /// <remarks>
        /// The name includes the prefix character as written in the style definition.
        /// If the value is set the 
        /// <see cref="GuruComponents.Netrix.WebEditing.Styles.StyleRule.SelectorType">SelectorType</see> will be
        /// set accordingly.
        /// </remarks>
        [DescriptionAttribute("")]
        [DefaultValueAttribute("")]
        [TypeConverterAttribute(typeof(string))]
        [RefreshProperties(RefreshProperties.Repaint)]
        [EditorAttribute(
             typeof(GuruComponents.Netrix.UserInterface.TypeEditors.UITypeEditorString),
             typeof(System.Drawing.Design.UITypeEditor))]
        [GuruComponents.Netrix.UserInterface.TypeEditors.DisplayName()]
        [ReadOnly(true)]
        public string RuleName
        {
            get
            {   
                string st = (SelectorName == null) ? String.Empty : SelectorName;
                if (st.Length > 0)
                {                    
                    switch (st[0])
                    {
                        case '.':
                            selectorType = StyleRuleType.Class;
                            break;
                        case '#':
                            selectorType = StyleRuleType.ID;
                            break;
                        case '@':
                            selectorType = StyleRuleType.Pseudo;
                            break;
                        case '*':
                            st = "*";
                            selectorType = StyleRuleType.Global;
                            break;
                        default:
                            selectorType = StyleRuleType.Standard;
                            break;
                    }
                }
                return st;
            }

            set
            {
                SelectorName = value;
            }
        }

		/// <summary>
		/// Gives access to the style definition.
		/// </summary>
		[Browsable(false)]
        public IStyle StyleDefinition 
        {
            get
            {
                return new CssStyle(styleRule) as IStyle;
            }
        }

        /// <summary>
        /// The style which this rule defines.
        /// </summary>
        /// <remarks>
        /// This is the style text and must follow the CSS rules. The property does not check
        /// the syntax if the value is written. 
        /// </remarks>
        [DescriptionAttribute("")]
        [DefaultValueAttribute("")]
        [TypeConverterAttribute(typeof(string))]
        [RefreshProperties(RefreshProperties.Repaint)]
        [EditorAttribute(
             typeof(GuruComponents.Netrix.UserInterface.TypeEditors.UITypeEditorStyleStyle),
             typeof(System.Drawing.Design.UITypeEditor))]
        [GuruComponents.Netrix.UserInterface.TypeEditors.DisplayName()]
        public string style
        {
            get
            {
                return (styleRule.cssText == null) ? String.Empty : styleRule.cssText;
            }

            set
            {
                try
                {
                    styleRule.cssText = value;                    
                }
                catch
                {
                    // ignore wrong styles 
                }
            }
        }

        /// <summary>
        /// The string representation of the rule name.
        /// </summary>
        /// <returns>The value includes the prefix character as written in the style definition.</returns>
        public override string ToString()
        {
            string st = String.Empty;
            switch (selectorType)
            {
                case StyleRuleType.Class:
                    st = "[CLASS] ";
                    break;
                case StyleRuleType.ID:
                    st = "[ID] ";
                    break;
                case StyleRuleType.Pseudo:
                    st = "[@RULE] ";
                    break;
                default:
                    st = "[BASE] ";
                    break;
            }
            return String.Concat(st, this.RuleName);
        }

        /////<overloads/>
        ///// <summary>
        ///// Creates a new rule and adds them to the given style element.
        ///// </summary>
        //public StyleRule()
        //{

        //}

		internal void InitializeRule(StyleElement styleElement, string ruleName, string rule)
		{
			((Interop.IHTMLStyleElement) styleElement.GetBaseElement()).styleSheet.AddRule(ruleName, rule, 0);
			// if succeeded, synchronize content
			Interop.IHTMLStyleSheetRulesCollection rules = ((Interop.IHTMLStyleElement) styleElement.GetBaseElement()).styleSheet.GetRules() as Interop.IHTMLStyleSheetRulesCollection;
			if (rules != null)
			{
				styleRule = (Interop.IHTMLRuleStyle) rules.Item(0).GetStyle();
				RuleName = ruleName;
			}
		}

        /// <summary>
        /// Creates a new rule and adds them to the given style element.
        /// </summary>
        /// <param name="styleElement"></param>
        /// <param name="ruleName"></param>
        public StyleRule(StyleElement styleElement, string ruleName) : this(styleElement, ruleName, " " /* cannot add empty string, so we add a space */)
        {
        }

        /// <summary>
        /// Creates a new rule and adds them to the given style element.
        /// </summary>
        /// <param name="styleElement">The style element the new rule will be added to.</param>
        /// <param name="ruleName">The name of the rule, including preceding signs (. for class, # for ID).</param>
        /// <param name="rule">The rule content, e.g. "font-family: Verdana" (without quotes and braces).</param>
        public StyleRule(StyleElement styleElement, string ruleName, string rule)
        {
            int insertedRule = ((Interop.IHTMLStyleElement) styleElement.GetBaseElement()).styleSheet.AddRule(ruleName, " ", 0);
            // if succeeded, synchronize content
            Interop.IHTMLStyleSheetRulesCollection rules = ((Interop.IHTMLStyleElement) styleElement.GetBaseElement()).styleSheet.GetRules() as Interop.IHTMLStyleSheetRulesCollection;
            if (rules == null)
            {
                throw new ArgumentException("Cannot create style rule");
            }
            if (insertedRule >= 0)
            {
                styleRule = (Interop.IHTMLRuleStyle)rules.Item(insertedRule).GetStyle();
                RuleName = ruleName;
            }
            else
            {
                for (int i = 0; i < rules.GetLength(); i++)
                {
                    Interop.IHTMLStyleSheetRule rulestyle = rules.Item(i);
                    if (ruleName.Equals(rulestyle.GetSelectorText()))
                    {
                        styleRule = rulestyle.GetStyle();
                        styleRule.cssText = rule;
                        RuleName = ruleName;
                        break;
                    }
                }
            }
        }

        internal StyleRule(Interop.IHTMLStyleSheetRule rule)
        {
            nativeRule = rule;
            RuleName = rule.GetSelectorText();
            styleRule = rule.GetStyle();
        }
 
        #region ICustomTypeDescriptor Member

        PropertyDescriptorCollection pdc = null;

        PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties(Attribute[] filter) 
        { 
            if (pdc == null)
            {
                PropertyDescriptorCollection baseProps = TypeDescriptor.GetProperties(GetType(), filter); 
                // notice we use the type here so we don't recurse 
                PropertyDescriptor[] newProps = new PropertyDescriptor[baseProps.Count]; 
                for (int i = 0; i < baseProps.Count; i++) 
                { 
                    newProps[i] = new GuruComponents.Netrix.UserInterface.TypeEditors.CustomPropertyDescriptor(baseProps[i], filter); 
                } 
                // probably wanna cache this... 
                pdc = new PropertyDescriptorCollection(newProps); 
            }
            return pdc;
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
 
        EventDescriptorCollection ICustomTypeDescriptor.GetEvents(System.Attribute[] attributes) 
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
 
        object ICustomTypeDescriptor.GetEditor(System.Type editorBaseType) 
        { 
            return TypeDescriptor.GetEditor(this, editorBaseType, true); 
        } 
 
        object ICustomTypeDescriptor.GetPropertyOwner(System.ComponentModel.PropertyDescriptor pd) 
        { 
            return this; 
        } 
 
        #endregion
    }
}