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
    /// This class represents the &lt;a&gt; element.
    /// </summary>
    /// <remarks>
    /// <para>
    /// </para>
    /// Classes directly or indirectly inherited from 
    /// <see cref="GuruComponents.Netrix.WebEditing.Elements.Element"/> are not intended to be instantiated
    /// directly by the host application. Use the various insertion or creation methods in the base classes
    /// instead. The return values are of type <see cref="GuruComponents.Netrix.WebEditing.Elements.IElement"/>
    /// and can be casted into the element just created.
    /// <para>
    /// Examples of how to create and modify elements with the native element classes can be found in the 
    /// description of other element classes.
    /// </para>
    /// </remarks>
    /// <example>
    /// The following code will set up double click event handlers for all anchor tags in the document:
    /// <code>
    /// ArrayList anchors = this.HtmlEditor.DocumentStructure.GetElementCollection("A") as ArrayList;
    /// if (anchors != null)
    /// {
    ///   foreach (AnchorElement a in anchors)
    ///   {
    ///     a.OnDblClick += new GuruComponents.NetrixEvents.DocumentEventHandler(a_OnDblClick);
    ///   }
    /// }
    /// </code>
    /// Remember, that this is a one-time action. If the user removes an anchor and inserts another one later, the event 
    /// handler will no longer be active. You can control this by adding the event handler after creating a new element; 
    /// the following code inserts a new hyperlink, using the current selection as viewable content and presets to properties. 
    /// Then the double click event will be tied with the event handler.
    /// <code>
    /// AnchorElement a = (AnchorElement) this.HtmlEditor.Document.InsertHyperlink();
    /// a.href = "http://www.comzept.de";
    /// a.target = "_blank";
    /// a.OnDblClick += new GuruComponents.NetrixEvents.DocumentEventHandler(a_OnDblClick);
    /// </code>
    /// The handler should be the same as in the previous example. The DEMO application uses the following code:
    /// <code>
    /// private void a_OnDblClick(GuruComponents.NetrixEvents.DocumentEventArgs e)
    /// {
    ///   if (HyperlinkWizardDlg.ShowDialog() == DialogResult.OK)
    ///   {
    ///     AnchorElement a = (AnchorElement) e.SrcElement;
    ///     a.href = String.Concat(HyperlinkWizardDlg.Protocol,"//",HyperlinkWizardDlg.Url);
    ///     a.target = HyperlinkWizardDlg.Target;
    ///   }
    /// }
    /// </code>
    /// HyperlinkWizardDlg is the dialog, which appears, if the user double clicks an anchor to change the behavior in a more user friendly and individual way than the PropertyGrid.
    /// </example>
    public sealed class AnchorElement : SelectableElement
    {

        /// <summary>
        /// HREF indicates the URL being linked to.
        /// </summary>
        /// <remarks>HREF makes the anchor into a link.</remarks>

        [Category("Element Behavior")]
        [DefaultValueAttribute("")]
        [DescriptionAttribute("")]
        [EditorAttribute(
             typeof(UITypeEditorUrl),
             typeof(UITypeEditor))]
        [DisplayNameAttribute()]

        public string href
        {
            set
            {
                try
                {
                    System.Uri uri;
                    if (System.Uri.TryCreate(value, System.UriKind.Absolute, out uri))
                    {
                        this.SetStringAttribute("href", uri.AbsoluteUri);
                        return;
                    }
                }
                catch
                {
                }
                this.SetStringAttribute ("href", this.GetRelativeUrl(value));
                return;
            } 
            get
            {
                string href = this.GetStringAttribute("href");
                try
                {
                    System.Uri uri;
                    if (System.Uri.TryCreate(href, System.UriKind.Absolute, out uri))
                    {
                        return uri.AbsoluteUri;
                    }
                }
                catch
                {
                }
                return this.GetRelativeUrl(href);

            }  
        }

		/// <summary>
		/// NAME gives the anchor a name.
		/// </summary>
		/// <remarks>Other links target the anchor using that name. This allows you to link to specific places within the page.</remarks>

        [CategoryAttribute("Element Behavior")]
		[DefaultValueAttribute("")]
		[DescriptionAttribute("")]
        [EditorAttribute(
             typeof(UITypeEditorString),
             typeof(UITypeEditor))]
        [DisplayName()]

        public string name
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


        [CategoryAttribute("JavaScript Events")]
        [DefaultValueAttribute("")]
        [DescriptionAttribute("Event raised when a mouse click begins over this object")]
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


        [CategoryAttribute("JavaScript Events")]
        [DescriptionAttribute("")]
        [DefaultValueAttribute("")]
        [EditorAttribute(
             typeof(UITypeEditorString),
             typeof(UITypeEditor))]
        [DisplayName()]

        [ScriptingVisible()] public string ScriptOnMouseUp
        {
            get
            {
                return base.GetStringAttribute("onmouseup");
            }

            set
            {
                base.SetStringAttribute("onmouseup", value);
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
        public AnchorElement(IHtmlEditor editor)
            : base("a", editor)
        {
        }

        internal AnchorElement(Interop.IHTMLElement peer, IHtmlEditor editor) : base (peer, editor)
        {
        }
    }

}
