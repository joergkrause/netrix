using System;
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
    /// The &lt;BGSOUND&gt; tag allows a sound file (WAV, AU or MID) to be played when a page is opened.
    /// </summary>
    /// <remarks>
    /// Sometimes you will visit a web site and suddenly your speakers will start spitting out sounds.  I don't know about you, but the first thing that I do when this happens is curse silently. Then I take a quick glance (a second or two) and decide if I really need to be visiting this web site. One of two things will happen at this point. I will find a button to turn off the sound and stay or I will surf to another site. 
    /// Every new webmaster seems to do it at least once. Add background sound to their web site. And, you know, there are times when it is appropriate. A musicians site, for example, might be the right place. Or a site all about MIDI files might also have a background sound. I ran across a MIDI site once that played a random, different MIDI each time a page was loaded. That was cool, but only because that was what the site was about.
    /// </remarks>
    public sealed class BgSoundElement : Element
    {

        /// <summary>
        /// SRC provides the name of the sound.
        /// </summary>
        /// <remarks>The name of the sound file to be played.</remarks>

        [Category("Element Behavior")]
        [DefaultValueAttribute("")]
        [DescriptionAttribute("")]
        [EditorAttribute(
             typeof(UITypeEditorUrl),
             typeof(UITypeEditor))]
        [DisplayNameAttribute()]

        public string src
        {
            set
            {
                this.SetStringAttribute ("src", this.GetRelativeUrl(value));
                return;
            } 
            get
            {
                return this.GetRelativeUrl (this.GetStringAttribute ("src"));
            }  
        }

        /// <summary>
        /// The number of times the sound is played.
        /// </summary>
        /// <remarks>
        /// A value of -1 or INFINITE will cause the sound to be played forever.
        /// <para>To support the value INFINITE the properties datatype is <c>string</c>. The number must anyway given as
        /// a numeric expression. Otherwise an exception is thrown. If the access of this property uses the propertygrid,
        /// the exception will handled within the propertygrid and a messagebox appears instead.</para>
        /// </remarks>
        /// <exception cref="System.ArgumentException">Thrown if the value does not represent a number or the string 'INFINITE'.</exception>

        [CategoryAttribute("Element Behavior")]
        [DefaultValueAttribute("")]
        [DescriptionAttribute("")]
        [EditorAttribute(
             typeof(UITypeEditorString),
             typeof(UITypeEditor))]
        [DisplayName()]

        public string loop
        {
            get
            {
                return base.GetStringAttribute("loop");
            }

            set
            {
                if (!value.ToLower().Equals("infinite"))
                {
                    int i;
                    if (!Int32.TryParse(value, out i))
                    {
                        throw new ArgumentException("Loop value must be either 'infinite' or an integer value.", "loop");
                    }
                }
                base.SetStringAttribute("loop", value);
            }
        }

        /// <summary>
        /// AUTOSTART let the sound played immediately.
        /// </summary>
        /// <remarks>Value is TRUE to being playing the music immediately upon page load. TRUE is the default value.</remarks>

        [CategoryAttribute("Element Behavior")]
        [DefaultValue(true)]
        [Description("")]
        [TypeConverter(typeof(UITypeConverterDropList))]
        [DisplayName()]

        public bool autostart
        {
            get
            {
                return this.GetBooleanAttribute ("autostart");
            } 
      
            set
            {
                this.SetBooleanAttribute ("autostart", value);
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
        public BgSoundElement(IHtmlEditor editor)
            : base("bgsound", editor)
        {
        }

        internal BgSoundElement (Interop.IHTMLElement peer, IHtmlEditor editor) : base (peer, editor)
        {
        } 
    }
}
