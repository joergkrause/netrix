using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Web.UI.WebControls;
using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.UserInterface.TypeEditors;
using DisplayNameAttribute=GuruComponents.Netrix.UserInterface.TypeEditors.DisplayNameAttribute;
using TE=GuruComponents.Netrix.UserInterface.TypeEditors;


namespace GuruComponents.Netrix.WebEditing.Elements
{
    
    /// <summary>
    /// This class represents the &lt;form&gt; element.
    /// </summary>
    /// <remarks>
    /// <para>
    /// FORM indicates the beginning of a form. All other form tags go inside 
    /// &lt;FORM ...&gt;. In its simplest use, &lt;FORM ...&gt; can be used without any attributes.
    /// Most forms require either the 
    /// <see cref="GuruComponents.Netrix.WebEditing.Elements.FormElement.action">action</see> or 
    /// <see cref="GuruComponents.Netrix.WebEditing.Elements.FormElement.Name">Name</see> attributes to do anything meaningful.
    /// </para>
    /// Classes directly or indirectly inherited from 
    /// <see cref="GuruComponents.Netrix.WebEditing.Elements.Element">Element</see> are not intended to be instantiated
    /// directly by the host application. Use the various insertion or creation methods in the base classes
    /// instead. The return values are of type <see cref="GuruComponents.Netrix.WebEditing.Elements.IElement">IElement</see>
    /// and can be casted into the element just created.
    /// <para>
    /// Examples of how to create and modify elements with the native element classes can be found in the 
    /// description of other element classes.
    /// </para>
    /// </remarks>
    public sealed class FormElement : StyledElement
    {

        /// <summary>
        /// ACTION gives the URL of the CGI program which will process this form.
        /// </summary>

        [DefaultValue("")]
        [CategoryAttribute("Element Behavior")]
        [DescriptionAttribute("")]
        [EditorAttribute(
             typeof(UITypeEditorUrl),
             typeof(UITypeEditor))]
        [DisplayNameAttribute()]

        public string action
        {
            set
            {
                this.SetStringAttribute ("action", this.GetRelativeUrl(value));
                return;
            } 
            get
            {
                return this.GetRelativeUrl (this.GetStringAttribute ("action"));
            }       
        }

        /// <summary>
        /// In most cases you will not need to use this attribute at all. 
        /// </summary>
        /// <remarks>
        /// The default value (i.e. if you don't use this attribute at all) is "application/x-www-form-urlencoded", 
        /// which is sufficient for almost any kind of form data. 
        /// <para>
        /// The one exception is if you want to do file uploads. In that case you should use "multipart/form-data". 
        /// Because many people use this attribute for file upload only the definition here sets the string 
        /// "multipart/form-data" as the default value. This changes the behavior of the property grid.
        /// </para>
        /// <para>
        /// The allowed values for this attribute are the following ones:
        /// <list>
        ///     <item>"application/x-www-form-urlencoded" (default from the HTML 4.01 viewpoint)</item>
        ///     <item>"multipart/form-data" (default in the NetRix UI)</item>
        ///     <item>"text/plain"</item>
        /// </list>
        /// </para>
        /// </remarks>
        /// <seealso cref="GuruComponents.Netrix.WebEditing.Elements.EncodingType"/>

        [DescriptionAttribute("")]
        [DefaultValueAttribute(EncodingType.UrlEncoded)]
        [CategoryAttribute("Element Behavior")]
        [DisplayName()]

        public EncodingType encType
        {
            get
            {
                if (!base.GetStringAttribute("encoding").Equals("multipart/form-data"))
                {
                    return EncodingType.UrlEncoded;
                }
                else
                {
                    return EncodingType.Multipart;
                }
            }

            set
            {
                base.SetStringAttribute("encoding", (value != EncodingType.Multipart) ? String.Empty : "multipart/form-data");
            }
        }

        /// <summary>
        /// METHOD specifies the method of transferring the form data to the web server.
        /// </summary>
        /// <remarks>
        ///  METHOD can be either GET or POST. Each method has its advantages and disadvantages. If the method is not set
        ///  the browser assumes GET. But from the viewpoint of the developer the method POST is the preferred standard.
        ///  Therefore the NetRix UI (not supported by Light version) will assume the POST is the default value. It is still
        ///  recommended to set this attribute explicitly for all standard HTML pages. 
        ///  <para>
        ///  Some environments will recreate this attribute automatically (just like ASP.NET does). If the editor creates
        ///  pages for such server driven applications there is often no need to set the attribute.
        ///  </para>
        /// </remarks>

        [DescriptionAttribute("")]
        [CategoryAttribute("Element Behavior")]
        [DefaultValueAttribute(FormMethod.Post)]
        [DisplayName()]

        public FormMethod method
        {
            get
            {
                return (FormMethod)base.GetEnumAttribute("method", FormMethod.Get);
            }

            set
            {
                base.SetEnumAttribute("method", value, FormMethod.Get);
            }
        }


        [DescriptionAttribute("")]
        [DefaultValueAttribute("")]
        [CategoryAttribute("JavaScript Events")]
        [EditorAttribute(
             typeof(UITypeEditorString),
             typeof(UITypeEditor))]
        [DisplayName()]

        [ScriptingVisible()] public string ScriptOnKeyPress
        {
            get
            {
                return base.GetStringAttribute("onKeyPress");
            }

            set
            {
                base.SetStringAttribute("onKeyPress", value);
            }
        }

		/// <summary>
		/// NAME gives a name to the form.</summary>
		/// <remarks>
		/// This is most useful in scripting, where you frequently need to refer to the form in order to refer to the element within the form.
		/// </remarks>

        [DefaultValueAttribute("")]
		[CategoryAttribute("Element Behavior")]
		[DescriptionAttribute("")]
        [EditorAttribute(
             typeof(UITypeEditorString),
             typeof(UITypeEditor))]
        [DisplayName("name_Form")]

        public string Name
		{
			get
			{
				return base.GetStringAttribute("name");
			}

			set
			{
				base.SetStringAttribute("name", value);
			}
		}


        [DefaultValueAttribute("")]
        [CategoryAttribute("JavaScript Events")]
        [DescriptionAttribute("")]
        [EditorAttribute(
             typeof(UITypeEditorString),
             typeof(UITypeEditor))]        
        [DisplayName()]

        [ScriptingVisible()] public string ScriptOnMouseDown
        {
            get
            {
                return base.GetStringAttribute("onMouseDown");
            }

            set
            {
                base.SetStringAttribute("onMouseDown", value);
            }
        }


        [DescriptionAttribute("")]
        [DefaultValueAttribute("")]
        [CategoryAttribute("JavaScript Events")]
        [EditorAttribute(
             typeof(UITypeEditorString),
             typeof(UITypeEditor))]
        [DisplayName()]

        [ScriptingVisible()] public string ScriptOnMouseUp
        {
            get
            {
                return base.GetStringAttribute("onMouseUp");
            }

            set
            {
                base.SetStringAttribute("onMouseUp", value);
            }
        }

        /// <summary>
        /// onReset runs a script when the user resets the form.
        /// </summary>
        /// <remarks>
        /// If onReset returns false, the reset is cancelled. Often when people hit reset they don't really mean to reset all their typing, they just hit it accidentally. onReset gives them a chance to cancel the action.
        /// </remarks>

        [CategoryAttribute("JavaScript Events")]
        [DefaultValueAttribute("")]
        [DescriptionAttribute("")]
        [EditorAttribute(
             typeof(UITypeEditorString),
             typeof(UITypeEditor))]
        [DisplayName()]

        [ScriptingVisible()] public string ScriptOnReset
        {
            get
            {
                return base.GetStringAttribute("onReset");
            }

            set
            {
                base.SetStringAttribute("onReset", value);
            }
        }

        /// <summary>
        /// onSubmit is a scripting event that occurs when the user attempts to submit the form to the CGI. 
        /// </summary>
        /// <remarks>
        /// onSubmit can be used to do some error checking on the form data, and to cancel the submit if an error is found.
        /// </remarks>

        [CategoryAttribute("JavaScript Events")]
        [DescriptionAttribute("")]
        [DefaultValueAttribute("")]
        [EditorAttribute(
             typeof(UITypeEditorString),
             typeof(UITypeEditor))]
        [DisplayName()]

        [ScriptingVisible()] public string ScriptOnSubmit
        {
            get
            {
                return base.GetStringAttribute("onSubmit");
            }

            set
            {
                base.SetStringAttribute("onSubmit", value);
            }
        }

        /// <include file='DocumentorIncludes.xml' path='WebEditing/Elements[@name="TargetAttribute"]'/>
        [CategoryAttribute("Element Behavior")]
        [TypeConverterAttribute(typeof(TargetConverter))]
        [DescriptionAttribute("")]
        [DefaultValueAttribute("")]
        [DisplayName()]

        public string target
        {
            get
            {
                return base.GetStringAttribute("target");
            }

            set
            {
                base.SetStringAttribute("target", value);
            }
        }

                /// <summary>
        /// Creates the specified element.
        /// </summary>
        /// <remarks>
        /// The element is being created and attached to the current document, but nevertheless not visible,
        /// until it's being placed anywhere within the DOM. To attach an element it's possible to either
        /// use the <see cref="ElementDom"/> property of any other already placed element and refer to this
        /// DOM or use the body element (<see cref="HtmlEditor.GetBodyElement"/>) and add the element there. Also, in 
        /// case of user interactive solutions, it's possible to add an element near the current caret 
        /// position, using <see cref="HtmlEditor.CreateElementAtCaret(string)"/> method.
        /// <para>
        /// Note: Invisible elements do neither appear in the DOM nor do they get saved.
        /// </para>
        /// </remarks>
        /// <param name="editor">The editor this element belongs to.</param>
        public FormElement(IHtmlEditor editor)
            : base(@"form", editor)
        {
        }


        internal FormElement(Interop.IHTMLElement peer, IHtmlEditor editor) : base (peer, editor)
        {
        }
    }

}
