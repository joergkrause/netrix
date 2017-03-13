using System.ComponentModel;
using System.Drawing.Design;
using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.UserInterface.TypeConverters;
using GuruComponents.Netrix.UserInterface.TypeEditors;
using DisplayNameAttribute=GuruComponents.Netrix.UserInterface.TypeEditors.DisplayNameAttribute;
using TE=GuruComponents.Netrix.UserInterface.TypeEditors;


namespace GuruComponents.Netrix.WebEditing.Elements
{
    
    /// <summary>
    /// Defines a caption for a table.
    /// </summary>
    /// <remarks>
    /// This element must appear straight after the opening table tag and used only once. 
    /// To add a caption to an existing table use the TableDesigner PlugIn's AddCaption command, 
    /// that can be found in the TableFormatter class. Each table
    /// object can return the TableFormatter instance.
    /// <seealso cref="GuruComponents.Netrix.WebEditing.Elements.TableElement">TableElement</seealso>
    /// <seealso cref="GuruComponents.Netrix.WebEditing.Elements.CaptionAlignment">CaptionAlignment</seealso>
    /// </remarks>
    public sealed class CaptionElement : StyledElement
    {

        # region Public Properties

        /// <summary>
        /// Gets or sets the content of the caption.
        /// </summary>
        /// <remarks>
        /// If this property is not set the caption will be invisible. The property does not accept HTML.
        /// Tags are transformed into text, e.g. are stripped out.
        /// </remarks>

        [Description("")]
        [DefaultValueAttribute("")]
        [CategoryAttribute("Element Content")]
        [EditorAttribute(
             typeof(UITypeEditorString),
             typeof(UITypeEditor))]
        [DisplayNameAttribute()]

        public string Content
        {
            get
            {
                return base.InnerText;
            } 
            set
            {
                base.InnerText = value;
            } 
      
        }

        /// <summary>
        /// The alignment of the caption. Depreciated.
        /// </summary>
        /// <remarks>
        /// This property is used for compatibility only. It is recommended to format 
        /// captions with styles (CSS) instead.
        /// </remarks>
        [DescriptionAttribute("")]
        [DefaultValueAttribute(CaptionAlignment.Top)]
        [CategoryAttribute("Element Layout")]
        [TypeConverter(typeof(UITypeConverterDropList))]
        [DisplayName()]
        public CaptionAlignment align
        {
            get
            {
                return (CaptionAlignment) this.GetEnumAttribute ("align", (CaptionAlignment) 0);
            } 
            set
            {
                this.SetEnumAttribute ("align", value, (CaptionAlignment) 0);
            } 
      
        }

        # endregion

        /// <include file='DocumentorIncludes.xml' path='//WebEditing/Elements[@name="PublicElementConstructor"]'/>
        public CaptionElement() : base("caption", null)
        {        
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
        public CaptionElement(IHtmlEditor editor)
            : base("caption", editor)
        {
        }

        internal CaptionElement(Interop.IHTMLElement peer, IHtmlEditor editor) : base (peer, editor)
        {
        }
    }

}