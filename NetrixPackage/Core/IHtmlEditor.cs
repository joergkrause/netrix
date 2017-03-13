using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.Designer;
using GuruComponents.Netrix.Events;
using GuruComponents.Netrix.PlugIns;
using GuruComponents.Netrix.WebEditing.Behaviors;
using GuruComponents.Netrix.WebEditing.Documents;
using GuruComponents.Netrix.WebEditing.Elements;
using GuruComponents.Netrix.HtmlFormatting;
using GuruComponents.Netrix.HtmlFormatting.Elements;
using GuruComponents.Netrix.WebEditing.Glyphs;
using GuruComponents.Netrix.WebEditing.HighLighting;
using GuruComponents.Netrix.WebEditing.UndoRedo;
using Control=System.Web.UI.Control;
using System.Collections.Generic;

namespace GuruComponents.Netrix
{

    /// <summary>
    /// The editor control with the basic functionality. </summary>
    /// This is the main control.
    /// <remarks>
    /// <para>
    /// The main assembly has a satellite contract attribute to unlink the version relation between
    /// the main and the satellite assemblies. You should set all new satellites always to "1.0.0.0" to
    /// make the resource manager operate properly, even if the version of the main assembly is
    /// much higher.
    /// </para>
    /// </remarks>
    /// <example>
    /// To instantiate the control just call the constructor:
/// <code>
/// using GuruComponents.Netrix;
/// 
/// HtmlEditor htmlEditor1 = new HtmlEditor();
/// // do some stuff with the editor here</code>
    /// A basic application to implement a HTML editor solution must provide these functions:
    /// <list type="list">
    ///     <item><term>An instance of the component.</term></item>
    ///     <item><term>The <see cref="GuruComponents.Netrix.IHtmlEditor.ReadyStateComplete">ReadyStateComplete</see> event attached to a handler.</term></item>
    ///     <item><term>A handler which sets the control into design mode on request.</term></item>
    /// </list>
    /// After having this the user can put the control into design mode and the application can look for the
    /// ready state complete event. It is important that no function is called before this event was fired.
    /// <para>
    /// Take a look at the various methods and properties to see more examples of how to use it.
    /// </para>
    /// </example>
    public interface IHtmlEditor 
    {

        /// <summary>
        /// Whether or elements which could be influenced by TAB key should get TAB sequences in design mode.
        /// </summary>
        /// <remarks>
        /// This applies to:
        /// <list type="bullet">
        ///     <item>LI</item>
        /// </list>
        /// </remarks>
        bool HandleTAB
        {
            get;
            set;
        }

        /// <summary>
        /// Disables the ability to select text if control is in browse mode.
        /// </summary>
        /// <remarks>
        /// This applies only if the content is loaded after setting the property.
        /// <para>
        /// Note: This features is available as of NetRix Pro 1.6. It's not available in Advanced or Standard.
        /// </para>
        /// </remarks>
        bool NoTextSelection
        {
            get;
            set;
        }

        /// <summary>
        /// Allows computer users to employ IME reconversion while browsing Web pages.
        /// </summary>
        /// <remarks>
        /// During initialization, the host can set this flag to enable Input Method Editor (IME) reconversion, allowing computer users to employ IME reconversion while browsing Web pages. An input method editor is a program that allows users to enter complex characters and symbols, such as Japanese Kanji characters, using a standard keyboard.
        /// <para>
        /// Note: This features is available as of NetRix Pro 1.6. It's not available in Advanced or Standard.
        /// </para>
        /// </remarks>
        bool ImeReconversion
        {
            get;
            set;
        }

        /// <summary>
        /// Default formatter used if no external formatter is being provided.
        /// </summary>
        IHtmlFormatterOptions HtmlFormatterOptions
        {
            get;
            set;
        }

        /// <summary>
        /// Turns on thread safe access to properties and methods.
        /// </summary>
        /// <remarks>
        /// By default the component isn't thread safe. However, the burdensome Invoke technique required to invoke
        /// cross thread calls is implemented internally and can be turned on or off using this property.
        /// The drawback is that permanent access in a threadsafe manner could result in the performance flaw. In
        /// case nobody needs cross thread access it's recommended to let this property set to <c>false</c>.
        /// </remarks>
        bool ThreadSafety { get; set; }
 
        /// <summary>
        /// Executes the specified command in MSHTML directly.
        /// </summary>
        /// <param name="command">The command. Not all commands are supported in all situations.</param>
        void Exec(Interop.IDM command);
      
        /// <summary>
        /// Executes the specified command in MSHTML with the specified arguments.
        /// </summary>
        /// <param name="command">The command. Not all commands are supported in all situations.</param>
        /// <param name="argument">The argument. Supported types are string, bool, int and short.</param>
        void Exec(Interop.IDM command, object argument);

        /// <summary>
        /// If set to true all loaded data with textual base type will be treated as HTML.
        /// </summary>
        bool ForceMimeType { get; set; }

        /// <summary>
        /// Processes the TAB key if possible instead of moving focus to next control.
        /// </summary>
        bool AcceptsTab { get; set; }

        /// <summary>
        /// Whether select a complete word on double click.
        /// </summary>
        bool AutoWordSelection { get; set; }

        /// <summary>
        /// Plugins can call this method to register itself as callback for certain functions.
        /// </summary>
        /// <param name="plugin">A reference to the plugin object.</param>
        void RegisterPlugIn(IPlugIn plugin);

        /// <summary>
        /// Gives direct access to the list of registered plug-ins.
        /// </summary>
        List<IPlugIn> RegisteredPlugIns { get; }

        /// <summary>
        /// Exchanges the behavior of Enter key. If active, Enter inserts BR instead of P.
        /// </summary>
        /// <remarks>
        /// This method allows the editor to behave differently if the user hits the Enter key.
        /// By default, enter inserts a new paragraph. If this property is <c>true</c>, the Enter key will
        /// insert a break (BR tag). The opposite insertion method, Shft-Enter, will insert a paragraph.
        /// The type of paragraph depends on the the current <see cref="BlockDefault"/> setting.
        /// </remarks>
        /// <seealso cref="BlockDefault"/>
        bool TransposeEnterBehavior
        {
            get;
            set;
        }

		/// <summary>
		/// Turns off the hatched border and handles around a site selectable element when the element has "edit focus" in design mode; that is, when the text or contents of the element can be edited.
		/// </summary>
		/// <value><c>True</c> to active the property. Defaults to <c>False</c>.</value>
		bool DisableEditFocus { get; set; }

        /// <summary>
        /// Return the native document instance.
        /// </summary>
        /// <param name="BaseDocument"></param>
        /// <returns></returns>
        Interop.IHTMLDocument2 GetActiveDocument(bool BaseDocument);

        /// <summary>
        /// Returns the factory that controls the relation between native and exposes element objects.
        /// </summary>
		IElementFactory GenericElementFactory { get; }

        /// <summary>
        /// Get the current element which is in the scope for next editing operation. 
        /// </summary>
        /// <remarks>
        /// Caller can cast the object to the right IElement derived type.
        /// <para>
        /// If it's more important to get the current element, which has fired the last event or
        /// is set by <see cref="HtmlElementChanged"/> event, it's better to use the <see cref="GetCurrentElement"/>
        /// method. Under normal circumstances both method return the same element, but during key or mouse
        /// operations in nested element hierarchies there might be a difference between currency and scope.
        /// </para>
        /// </remarks>
        /// <returns>The element object</returns>
        IElement GetCurrentScopeElement();
        
        /// <summary>
        /// Gets the current element.
        /// </summary>
        /// <remarks>
        /// This method returns the current element, as it is set by <see cref="HtmlElementChanged"/> event.
        /// If the element wasn't recognized for some reason the method returns the Body Element
        /// </remarks>
        /// <seealso cref="GetCurrentScopeElement"/>
        /// <returns>The current element object.</returns>
        IElement GetCurrentElement();

        /// <summary>
		/// Searches the element tree upwards to detect the next element the current element is in.
		/// </summary>
		/// <remarks>
		/// Search stops and fails if body is reached ==> element is not in document tree.
		/// Search returns the first occurence of element, therefore the innermost element of a nested
		/// structure is recognized.
		/// </remarks>
		/// <param name="element">IElement element or null if not found.</param>
		/// <param name="tagName">The tag name which the methods searches for.</param>
		/// <returns>Returns the element, if found.</returns>
        Control GetParentFromHierarchy(IElement element, string tagName);
      
        /// <summary>
        /// The current Assembly version.
        /// </summary>                   
        /// <remarks>
        /// This property returns the current assembly version to inform during design session about the assembly loaded.
        /// </remarks>
        string Version { get; }

        /// <summary>
        /// Returns the current serial number prefix the control was licensed with.
        /// </summary>
        /// <remarks>
        /// This gives the developer access to the type of control and license the control
        /// was compiled with during run time. The prefix contains information about the
        /// control type and kind of license in the following format:
        /// <para>
        /// XXTCCTK / 7 digits, numeric
        /// </para>
        /// <para>
        /// The digits have the following meaning:
        /// <list type="bullet">
        /// <item>XX: always 33 (Code for the Genesis Series at Guru Components)</item>
        /// <item>T: 9 for full version, 0 for trial</item>
        /// <item>CC: always 44 (Code for the NetRix Control and Component Series)</item>
        /// <item>K: Control key (1=Full, 2=Light, 3=ColorPicker, 4=FontPicker, 5=StylEditor, 6=UnitEditor)</item>
        /// </list>
        /// The host application can look for the specific value and operate differently depending
        /// on the used control type and license. For example, you can produce a demo version of your
        /// product with the trial license of NetRix which expires exactly one day before the NetRix
        /// license expires to avoid the license exception thrown from the license manager. 
        /// </para>
        /// Note: The prefix does not contain secret information about the key or your serial number.
        /// If an user retrieves the prefix from the control he cannot activate or retrieve your license.
        /// </remarks>
        string SerialNumberPrefix { get; }

        /// <summary>
        /// Registers a namespace.
        /// </summary>
        /// <remarks>
        /// Registered Namespaces will call the behaviorfactory for that namespace.
        /// The behaviorfactory uses a callback delegate to find the drawing method to draw and resolve
        /// the element and, additionally, the ViewLink content used to display the element. Both, drawing 
        /// and embedding, must implemented separatly by the host application.
        /// </remarks>
        /// <param name="alias">The alias of the namespace, like "asp" for ASP.NET controls.</param>
        /// <param name="namespaceName">URN of namespace</param>
        /// <param name="behavior">The type of behavior the namespace manager uses to resolve elements for rendering. Must inherit from IBaseBehavior.</param>
        void RegisterNamespace(string alias, string namespaceName, Type behavior);

        /// <summary>
        /// Registers a namespace.
        /// </summary>
        /// <remarks>
        /// Registered Namespaces will call the behaviorfactory for that namespace.
        /// The behaviorfactory uses a callback delegate to find the drawing method to draw and resolve
        /// the element and, additionally, the ViewLink content used to display the element. Both, drawing 
        /// and embedding, must implemented separatly by the host application.
        /// </remarks>
        /// <param name="alias">The alias of the namespace, like "asp" for ASP.NET controls.</param>
        /// <param name="behavior">The type of behavior the namespace manager uses to resolve elements for rendering. Must inherit from IBaseBehavior.</param>
        void RegisterNamespace(string alias, Type behavior);

        /// <summary>
        /// Returns the list of registered namespaces as dictionary with alias:ns pairs.
        /// </summary>
        /// <returns>A list of registered namespaces as dictionary with alias:ns pairs</returns>
        IDictionary GetRegisteredNamespaces();

        /// <summary>
        /// Registers an element for internal processing.
        /// </summary>
        /// <remarks>
        /// The type class must have a public constructor which must accept exact one parameter of
        /// type <see cref="GuruComponents.Netrix.ComInterop.Interop.IHTMLElement">IHTMLElement</see>.
        /// Once the element is registered, the control tries to get the constructor, instantiate an
        /// object and return the object to the caller. Some of the events the control fires use these
        /// generic object creation to expose native element objects to the host application.
        /// <para>
        /// To obtain the <see cref="GuruComponents.Netrix.HtmlFormatting.Elements.ITagInfo">ITagInfo</see>
        /// object instantiate it using the <see cref="GuruComponents.Netrix.HtmlFormatting.Elements.ITagInfo">ITagInfo</see> class or
        /// any class which implements <see cref="GuruComponents.Netrix.HtmlFormatting.Elements.ITagInfo">ITagInfo</see>.
        /// When creating an <see cref="GuruComponents.Netrix.HtmlFormatting.Elements.ITagInfo">ITagInfo</see> object for
        /// elements which resides in a specific namespace, one must include the namespace alias which
        /// registered whithin the control. The namespace must have been registered before the first element is added.
        /// </para>
        /// <para>
        /// The type the element object is created from must implement <see cref="GuruComponents.Netrix.WebEditing.Elements.IElement">IElement</see>.
        /// </para>
        /// <seealso cref="RegisterNamespace(string,Type)"/>
        /// <seealso cref="IElement"/>
        /// <seealso cref="GuruComponents.Netrix.WebEditing.Elements.IElement"/>
        /// <seealso cref="GuruComponents.Netrix.HtmlFormatting.Elements.ITagInfo"/>
        /// </remarks>
        /// <param name="tagInfo">The formatting information.</param>
        /// <param name="elementType">The type of element.</param>
        void RegisterElement(
            ITagInfo tagInfo,
            Type elementType);

        /// <summary>
        /// Add a command which allows Extenders to provide callbacks.
        /// </summary>
        /// <param name="wrapper">The CommandWrapper objects defines the ID and the callback handler.</param>
        void AddCommand(CommandWrapper wrapper);

        /// <summary>
        /// Sends a command to the editor.
        /// </summary>
        /// <remarks>
        /// Extender and plug-ins can register there private commands by adding them using <see cref="AddCommand"/>.
        /// The host application can then call these commands by using this method. 
        /// </remarks>
        /// <param name="id"></param>
        void InvokeCommand(CommandID id);

        /// <summary>
        /// Returns the service provider to provide common .NET services to plug-ins.
        /// </summary>
        NetrixServiceProvider ServiceProvider { get; }
        
        /// <summary>
        /// The HideCaret function removes the caret from the screen. 
        /// </summary>
        /// <remarks>
        /// Hiding a caret does not destroy its current shape or invalidate the insertion point.
        /// </remarks>
        void HideCaret();
        
        /// <summary>
        /// The ShowCaret function makes the caret visible on the screen at the caret's current position.
        /// </summary>
        /// <remarks>
        /// When the caret becomes visible, it begins flashing automatically. 
        /// ShowCaret shows the caret only if the specified window owns the caret, the caret has a shape, and the caret has not been hidden two or more times in a row. If one or more of these conditions is not met, ShowCaret does nothing and returns <c>false</c>.
        /// <para>
        /// Hiding is cumulative. If your application calls HideCaret five times in a row, it must also call ShowCaret five times before the caret reappears.
        /// </para>            
        /// <para>
        /// The system provides one caret per queue. A window should create a caret only when it has the keyboard focus or is active. The window should destroy the caret before losing the keyboard focus or becoming inactive.
        /// In NetRix this is the default behavior and there is no need to manipulate the caret directly but a different behavior is required.
        /// </para>
        /// </remarks>
        void ShowCaret();

        # region Culture Management

        /// <summary>
        /// Sets or gets the current Culture.
        /// </summary>
        /// <remarks>
        /// Setting a different culture will load the language from
        /// NetRix.Localization.dll satellite DLL, which should contain the strings and images for that
        /// culture. If the culture does not exists there the default culture "en-US" will load.
        /// Once the resource is loaded, we store it in a static field to make it accessible to all
        /// classes which does not derive from HtmlEditor.
        /// </remarks>
        string CurrentCulture { get; }

        # endregion

        # region Internally used Editor functions

		/// <summary>
		/// If set to <c>true</c> the focus will not set on load.
		/// </summary>
		/// <remarks>
		/// The default behavior of the focus will set the focus on the control and
		/// activates the caret after loading a document or HTML. This property suppresses
		/// this behavior if set.
		/// <para>
		/// It is important to set (or let) this property to <c>false</c>, if content is loaded, which
		/// has client side scripts in it or which has forms where the user can enter values. If the
		/// document cannot receive the focus, the user cannot enter anything, the content is totally
		/// blocked. 
		/// </para>
		/// The intention of this property is to allow the subsequential re-loading of the content using
		/// <see cref="LoadHtml(string)"/> whereas the user enters data into another control on the form. If, under
		/// any circumstances, the user is allows to enter anything or copy anything from the control, the
		/// property must turned off (set to <c>false</c> again.
		/// <para>
		/// This property was introduced in the second edition of the 1016 Build, June 8, 2004. The normal
		/// behavior was not changed if the property is not set explicitly and the control runs in design
		/// mode. In browse mode the control runs with getting the focus, unless this property is
		/// explicitly set.
		/// </para>
		/// </remarks>
		bool StopFocusOnLoad
		{
			set;
			get;
		}

		/// <summary>
		/// Fired before the snap rect method is applied during resize and move. Cancallable.
		/// </summary>
		event BeforeSnapRectEventHandler BeforeSnapRect;
		
		/// <summary>
		/// Fired after the snap rect method is applied during resize and move.
		/// </summary>
		event AfterSnapRectEventHandler AfterSnapRect;

		/// <summary>
		/// Fired before a new document will loaded from stream or file.
		/// </summary>
		event LoadEventHandler Loading;
		/// <summary>
		/// Fired after the new document is succesfully loaded from stream or file.
		/// </summary>
		/// <remarks>
		/// The event is fired immediataley after the base stream is loaded and and the protocol handler 
		/// is successfully attached. Loading a document is a asynchronous process. If the document contains
		/// several object (like images) they are loaded after the base document is ready. This may take a
		/// while. The final document cannot be used before the ReadyStatecomplete event is fired.
		/// </remarks>
		event LoadEventHandler Loaded;
		/// <summary>
		/// Fired before the document will save into stream.
		/// </summary>
		event SaveEventHandler Saving;
		/// <summary>
		/// Fired after the document is successfully saved into stream.
		/// </summary>
		event SaveEventHandler Saved;
		/// <summary>
		/// Fired if a new document has been succesfully created.
		/// </summary>
		event CreatedEventHandler DocumentCreated;
		/// <summary>
		/// Fired if a new element has been successfully created.
		/// </summary>
		event CreatedEventHandler ElementCreated;

		/// <summary>
		/// Switch element events globally on or off.
		/// </summary>
		/// <remarks>
		/// By default the event manager receives all events for all elements.
		/// </remarks>
		/// <returns>An instance of the event manager, which provides access to setting properties.</returns>
		EventManager EventManager { get; }

        /// <summary>
        /// Access to event binging events.
        /// </summary>
        IEventBinding EventBinding { get; }

        /// <summary>
        /// Binding to the user interface service.
        /// </summary>
        [Browsable(false)]
        IUIService UIBinding { get; }

        /// <summary>
        /// This methods returns one of the inner types not available in the Core assembly.
        /// </summary>
        /// <remarks>
        /// This method is used by plug-ins to get access to internal types.
        /// </remarks>
        /// <param name="typeName">The full qualified name.</param>
        /// <returns>The Type, if the name was found and is declared in 'GuruComponents.Netrix.Editor.dll'.</returns>
        Type ReflectInnerType(string typeName);

        /// <summary>
        /// Overrides the mouse pointer.
        /// </summary>
        /// <param name="Hide"></param>
        void SetMousePointer(bool Hide);

        /// <summary>
        /// Gets the current status of any (theoretical) available command.
        /// </summary>
        /// <remarks>
        /// This is a very fast method to check the current state of the control and
        /// the currently available commands, used to update toolbars and menus. 
        /// </remarks>
        /// <param name="command">Any command from the <see cref="GuruComponents.Netrix.HtmlCommand">HtmlCommand</see> enumeration.</param>
        /// <returns>A value that indicates the command is available or not or currently toggled on.</returns>
        HtmlCommandInfo CommandStatus(HtmlCommand command);
        
        /// <summary>
        /// Creates a new element.
        /// </summary>
        /// <remarks>
        /// This method does no add the element to the DOM, it makes it available for later placement.
        /// </remarks>
        /// <param name="tagName"></param>
        /// <returns></returns>
        IElement CreateElement(string tagName);

        # endregion
  
        #region +++++ Block: Common Control Properties

        # region Glyphs

        /// <summary>
        /// Access the glyph module.
        /// </summary>
		BuildGlyphs Glyphs { get; }
        
        # endregion Glyphs

        # region TableFormatting

        //GuruComponents.Netrix.WebEditing.ITableFormatter TableFormatter { get; }

        # endregion

        # region Highlighting, SpellChecker and Word selection

        /// <summary>
        /// Return the currently used TextSelector.
        /// </summary>
        /// <remarks>
        /// The <see cref="GuruComponents.Netrix.WebEditing.HighLighting.ITextSelector">ITextSelector</see> class handles selection 
        /// specifically for text, whereas HtmlSelection provides element based methods.
        /// </remarks>
        ITextSelector TextSelector { get; }

        /// <summary>
        /// This property returns the number of words in the document.
        /// </summary>
        /// <remarks>
        /// If the document is not in design mode the property will return 0. The method is time consuming.
        /// It is not recommended to call this property frequently, e.g. in mouse move or key press handlers.
        /// </remarks>
        /// <remarks>
        /// The method works in design mode only.
        /// </remarks>
        /// <example>
        /// Assuming MenuItem4 is a way the user can reach this method, the application can display the number of words
        /// using this method:<code>
///Private Sub MenuItem4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem4.Click
///  MessageBox.Show(Me, "This document contains approximatly " + Me.HtmlEditor1.WordCount.ToString() + " Words", 
///                      "Word Count", 
///                      MessageBoxButtons.OK, 
///                      MessageBoxIcon.Information)
///End Sub</code>
        /// </example>
        int WordCount { get; }

        # endregion

        #region Grid

        /// <summary>
        /// Access the grid module.
        /// </summary>
		IHtmlGrid Grid { get; }

        # endregion

        # region Styles and Behavior

        /// <summary>
        /// if activated the control respects hidden styles even in design mode.
        /// </summary>
        /// <remarks>
        /// The property defaults to <c>false</c>. Normally the designer shows hidden
        /// elements, to make editing possible. To see a more real preview of the page
        /// it's possible to change the option.
        /// <para>
        /// Application developers: This option should be set by user action, because
        /// a permanent setting will prevent the user from selecting hidden elements.
        /// </para>
        /// </remarks>
        bool RespectVisibility { get; set; }

        /// <summary>
        /// Show borders around elements if they don't have own borders.
        /// </summary>
        /// <remarks>
        /// This makes invisible tables and blind images
        /// visible at design time. If the table designer is used this option should not be activated.
        /// </remarks>
        bool BordersVisible { get; set; }

        /// <summary>
        /// This property disables linked stylesheets temporarily in design view.
        /// </summary>
        /// <remarks>
        /// It does not remove the styles or link tags nor it has any effect on the final document.
        /// </remarks>
        /// <example>
        /// Use the following code to switch the style sheet on and off:
        /// <code>
        /// this.htmlEditor1.LinkedStylesheetsEnabled = e.Button.Pushed;
        /// </code>
        /// </example>
        bool LinkedStylesheetsEnabled { get; set; }
        
        /// <summary>
        /// Enables or disables the automatic detection of URL and eMail addresses during text editing.
        /// </summary>
        bool AutoUrlModeEnabled { get; set; }

        # endregion

        # region 2D Positioning

        /// <summary>
        /// Causes the editor to update an element's appearance continuously during a resizing or moving operation, rather than updating only at the completion of the move or resize.
        /// </summary>
        /// <remarks>
        /// When this feature is off, an element's new position or size is indicated by a dashed rectangle until the mouse button is released.
        /// <seealso cref="AbsolutePositioningEnabled">AbsolutePositioningEnabled</seealso>
        /// <seealso cref="Grid">Grid</seealso>
        /// <seealso cref="MultipleSelectionEnabled">MultipleSelectionEnabled</seealso>
        /// </remarks>
        bool LiveResize { get; set; }

        /// <summary>
        /// Indicates if multiple selection is enabled in the editor.</summary>
        /// <remarks>
        /// Implicitly places MSHTML into multiple selection mode if set to <c>true</c>.
        /// This selection mode applies only if the control is in absolute positioning mode. To enter this mode
        /// just set the property 
        /// <see cref="AbsolutePositioningEnabled">AbsolutePositioningEnabled</see>.
        /// Internally the control uses style sheets to enable HTML beeing absolute positioned.
        /// <seealso cref="AbsolutePositioningEnabled">AbsolutePositioningEnabled</seealso>
        /// <seealso cref="Grid">Grid</seealso>
        /// </remarks>
        bool MultipleSelectionEnabled { get; set; }

        /// <summary>
        /// Enables or disables absolute position for the entire editor.
        /// </summary>
        /// <remarks>
        /// Internally the control uses style sheets to enable HTML beeing absolute positioned. After moving
        /// positionable elements (IMG, TABLE, DIV, ...) the style "position:absolute" will be applied. If a 
        /// position could be retrieved the "top" and "left" style attributes are set. Switching this property off
        /// will not remove the position information nor move the elements. After an element is first time moved
        /// with the mouse with AbsolutePositioningEnabled is off, the style "position:absolute" wil be removed.
        /// The position information will still left in the style attribute. If the property will be activated later,
        /// the style "position:absolute" is added again and the elements jump back to there previous position after
        /// the mouse touches them.
        /// <seealso cref="MultipleSelectionEnabled">MultipleSelectionEnabled</seealso>
        /// <seealso cref="Grid">Grid</seealso>
        /// </remarks>
        bool AbsolutePositioningEnabled { get; set; }

        # endregion

        # region General Document Related Commands and Properties


        /// <summary>
        /// Maintains a selection even when the control loses focus.
        /// </summary>
        /// <remarks>
        /// This property is useful for applications that require multiple control instances to interact with each other. 
        /// One example is an e-mail application that allows users to select from a list of e-mails in one control instance and 
        /// to see the actual e-mail in a second instance.
        /// </remarks>       
        bool KeepSelection { get; set; }

        /// <summary>
        /// The current selection in the editor.
        /// </summary> 
        /// <remarks>
        /// See <see cref="GuruComponents.Netrix.ISelection"/> for more information.
        /// </remarks>
        ISelection Selection { get; }

        /// <summary>
        /// The text formatting element of the editor.
        /// </summary>
        /// <remarks>
        /// See <see cref="GuruComponents.Netrix.ITextFormatting"/> for more information.
        /// </remarks>
        ITextFormatting TextFormatting { get; }

        /// <summary>
        /// The text formatting element of the editor that supports CSS inline styles.
        /// </summary>
        /// <remarks>
        /// See <see cref="GuruComponents.Netrix.ITextFormatting"/> for more information.
        /// </remarks>
        ITextFormatting CssTextFormatting { get; }

        /// <summary>
        /// Indicates whether the editor is in design mode.</summary>
        /// <remarks>
        /// Also places MSHTML into design mode if set to <c>true</c>.
        /// </remarks> 
        bool DesignModeEnabled { get; set; }

        /// <summary>
        /// Returns the document object for doing insertions.
        /// </summary>
        /// <remarks>
        /// If the property was called the first time and there was no document loaded the control
        /// creates a new empty document with default properties. 
        /// </remarks>
        IDocument Document { get; }

        /// <summary>
        /// The structure of the document.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        IDocumentStructure DocumentStructure { get; }

        /// <summary>
        /// Retrieves object which allows global access to the whole document window.
        /// </summary>
        IHtmlWindow Window { get; }

        # endregion

        # region Frames

        /// <summary>
        /// Gets <c>true</c> if the current document is a frame document.
        /// </summary>
        /// <remarks>
        /// The purpose of this property is to detect the frame state of the document and switch 
        /// appropriate UI elements on or off. Internally the property simply checks the number 
        /// of frames. Therefore the property may return <c>false</c>, even if the document is
        /// a frame document with just zero frames. Developery of application which use frames
        /// extensivly may double check the content using <see cref="GetXmlDocument()"/> method and
        /// XPath to see the real content of the base document.
        /// </remarks>
        bool IsFrameDocument { get; }

        /// <summary>
        /// Returns an object containing all Frame information.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Works only if the base document of a frameset.
        /// If the document contains a body this getter is still active and usable, but the framecollection is null.
        /// After retrieving successfully the frame object it can used to manipulate or create framesets.
        /// If the current document is a standard document and a new frame structure is build, the content will
        /// depraved and replaced by a new empty frameset.
        /// </para>
        /// <para>
        /// The documentation of <see cref="GuruComponents.Netrix.WebEditing.Documents.IFrameSet"/> for more information about manipulation framesets.
        /// </para>
        /// </remarks>
        IFrameSet HtmlFrames { get; }

        # region Printing

        /// <summary>
        /// Shows the internal Page Setup Dialog.
        /// </summary>
        void PrintPageSetup();

        /// <summary>
        /// Shows the print preview and let the user optionally print.
        /// </summary>
        /// <remarks>
        /// To use the print feature the control must be able to reload the current content internally. 
        /// Therefore the host application must provide a path or URL to the LoadXXX methods before 
        /// a printing process can start. Without a valid path the print command is ignored and the
        /// preview will display a blank page.
        /// </remarks>
        void PrintWithPreview();

        /// <summary>
        /// Prints a document using a customized preview from the given 
        /// print template.
        /// </summary>
        /// <remarks>
        /// To use the print feature the control must be able to reload the current content internally. 
        /// Therefore the host application must provide a path or URL to the LoadXXX methods before 
        /// a printing process can start. Without a valid path the print command is ignored and the
        /// preview will display a blank page.
        /// </remarks>
        /// <param name="PathToPrintTemplate">The path to the HTML code creating the print template.</param>
        void PrintWithPreview(string PathToPrintTemplate);

        /// <summary>
        /// Print the current document to the current printer without any user interaction.
        /// </summary>
        /// <remarks>
        /// To use the print feature the control must be able to reload the current content internally. 
        /// Therefore the host application must provide a path or URL to the LoadXXX methods before 
        /// a printing process can start. Without a valid path the print command is ignored and the
        /// method will print a blank page.
        /// </remarks>
        void PrintImmediately();

        # endregion

        # endregion

        /// <summary>
        /// This method sends an IDM command directly to MSHTML.
        /// </summary>
        /// <remarks>
        /// The call is protected by a try/catch block. The 
        /// method does not throw an exception on error and does always return nothing. This method is just for
        /// experimental environments and should not used in production environments.
        /// <para>
        /// This is method is subject to be changed or removed in later versions without further notice.
        /// </para>
        /// <list type="table">
        /// <listheader>
        ///     <term>IDM constant</term><term>Numeric Value</term>
        /// </listheader>
        /// <item>
        ///     <description></description>
        ///     <description></description>
        /// </item>
        /// <item><description>Interop.IDM.UNKNOWN                 </description><description>0</description></item>
        /// <item><description>Interop.IDM.ALIGNBOTTOM             </description><description>1</description></item>
        /// <item><description>Interop.IDM.ALIGNHORIZONTALCENTERS  </description><description>2</description></item>
        /// <item><description>Interop.IDM.ALIGNLEFT               </description><description>3</description></item>
        /// <item><description>Interop.IDM.ALIGNRIGHT              </description><description>4</description></item>
        /// <item><description>Interop.IDM.ALIGNTOGRID             </description><description>5</description></item>
        /// <item><description>Interop.IDM.ALIGNTOP                </description><description>6</description></item>
        /// <item><description>Interop.IDM.ALIGNVERTICALCENTERS    </description><description>7</description></item>
        /// <item><description>Interop.IDM.ARRANGEBOTTOM           </description><description>8</description></item>
        /// <item><description>Interop.IDM.ARRANGERIGHT            </description><description>9</description></item>
        /// <item><description>Interop.IDM.BRINGFORWARD            </description><description>10</description></item>
        /// <item><description>Interop.IDM.BRINGTOFRONT            </description><description>11</description></item>
        /// <item><description>Interop.IDM.CENTERHORIZONTALLY      </description><description>12</description></item>
        /// <item><description>Interop.IDM.CENTERVERTICALLY        </description><description>13</description></item>
        /// <item><description>Interop.IDM.CODE                    </description><description>14</description></item>
        /// <item><description>Interop.IDM.DELETE                  </description><description>17</description></item>
        /// <item><description>Interop.IDM.FONTNAME                </description><description>18</description></item>
        /// <item><description>Interop.IDM.FONTSIZE                </description><description>19</description></item>
        /// <item><description>Interop.IDM.GROUP                   </description><description>20</description></item>
        /// <item><description>Interop.IDM.HORIZSPACECONCATENATE   </description><description>21</description></item>
        /// <item><description>Interop.IDM.HORIZSPACEDECREASE      </description><description>22</description></item>
        /// <item><description>Interop.IDM.HORIZSPACEINCREASE      </description><description>23</description></item>
        /// <item><description>Interop.IDM.HORIZSPACEMAKEEQUAL     </description><description>24</description></item>
        /// <item><description>Interop.IDM.INSERTOBJECT            </description><description>25</description></item>
        /// <item><description>Interop.IDM.MULTILEVELREDO          </description><description>30</description></item>
        /// <item><description>Interop.IDM.SENDBACKWARD            </description><description>32</description></item>
        /// <item><description>Interop.IDM.SENDTOBACK              </description><description>33</description></item>
        /// <item><description>Interop.IDM.SHOWTABLE               </description><description>34</description></item>
        /// <item><description>Interop.IDM.SIZETOCONTROL           </description><description>35</description></item>
        /// <item><description>Interop.IDM.SIZETOCONTROLHEIGHT     </description><description>36</description></item>
        /// <item><description>Interop.IDM.SIZETOCONTROLWIDTH      </description><description>37</description></item>
        /// <item><description>Interop.IDM.SIZETOFIT               </description><description>38</description></item>
        /// <item><description>Interop.IDM.SIZETOGRID              </description><description>39</description></item>
        /// <item><description>Interop.IDM.SNAPTOGRID              </description><description>40</description></item>
        /// <item><description>Interop.IDM.TABORDER                </description><description>41</description></item>
        /// <item><description>Interop.IDM.TOOLBOX                 </description><description>42</description></item>
        /// <item><description>Interop.IDM.MULTILEVELUNDO          </description><description>44</description></item>
        /// <item><description>Interop.IDM.UNGROUP                 </description><description>45</description></item>
        /// <item><description>Interop.IDM.VERTSPACECONCATENATE    </description><description>46</description></item>
        /// <item><description>Interop.IDM.VERTSPACEDECREASE       </description><description>47</description></item>
        /// <item><description>Interop.IDM.VERTSPACEINCREASE       </description><description>48</description></item>
        /// <item><description>Interop.IDM.VERTSPACEMAKEEQUAL      </description><description>49</description></item>
        /// <item><description>Interop.IDM.JUSTIFYFULL             </description><description>50</description></item>
        /// <item><description>Interop.IDM.BACKCOLOR               </description><description>51</description></item>
        /// <item><description>Interop.IDM.BOLD                    </description><description>52</description></item>
        /// <item><description>Interop.IDM.BORDERCOLOR             </description><description>53</description></item>
        /// <item><description>Interop.IDM.FLAT                    </description><description>54</description></item>
        /// <item><description>Interop.IDM.FORECOLOR               </description><description>55</description></item>
        /// <item><description>Interop.IDM.ITALIC                  </description><description>56</description></item>
        /// <item><description>Interop.IDM.JUSTIFYCENTER           </description><description>57</description></item>
        /// <item><description>Interop.IDM.JUSTIFYGENERAL          </description><description>58</description></item>
        /// <item><description>Interop.IDM.JUSTIFYLEFT             </description><description>59</description></item>
        /// <item><description>Interop.IDM.JUSTIFYRIGHT            </description><description>60</description></item>
        /// <item><description>Interop.IDM.RAISED                  </description><description>61</description></item>
        /// <item><description>Interop.IDM.SUNKEN                  </description><description>62</description></item>
        /// <item><description>Interop.IDM.UNDERLINE               </description><description>63</description></item>
        /// <item><description>Interop.IDM.CHISELED                </description><description>64</description></item>
        /// <item><description>Interop.IDM.ETCHED                  </description><description>65</description></item>
        /// <item><description>Interop.IDM.SHADOWED                </description><description>66</description></item>
        /// <item><description>Interop.IDM.FIND                    </description><description>67</description></item>
        /// <item><description>Interop.IDM.SHOWGRID                </description><description>69</description></item>
        /// <item><description>Interop.IDM.OBJECTVERBLIST0         </description><description>72</description></item>
        /// <item><description>Interop.IDM.OBJECTVERBLIST1         </description><description>73</description></item>
        /// <item><description>Interop.IDM.OBJECTVERBLIST2         </description><description>74</description></item>
        /// <item><description>Interop.IDM.OBJECTVERBLIST3         </description><description>75</description></item>
        /// <item><description>Interop.IDM.OBJECTVERBLIST4         </description><description>76</description></item>
        /// <item><description>Interop.IDM.OBJECTVERBLIST5         </description><description>77</description></item>
        /// <item><description>Interop.IDM.OBJECTVERBLIST6         </description><description>78</description></item>
        /// <item><description>Interop.IDM.OBJECTVERBLIST7         </description><description>79</description></item>
        /// <item><description>Interop.IDM.OBJECTVERBLIST8         </description><description>80</description></item>
        /// <item><description>Interop.IDM.OBJECTVERBLIST9         </description><description>81</description></item>
        /// <item><description>Interop.IDM.OBJECTVERBLISTLAST </description><description> Interop.IDM.OBJECTVERBLIST9</description></item>
        /// <item><description>Interop.IDM.CONVERTOBJECT       </description><description>    82</description></item>
        /// <item><description>Interop.IDM.CUSTOMCONTROL       </description><description>    83</description></item>
        /// <item><description>Interop.IDM.CUSTOMIZEITEM       </description><description>    84</description></item>
        /// <item><description>Interop.IDM.RENAME              </description><description>    85</description></item>
        /// <item><description>Interop.IDM.IMPORT              </description><description>    86</description></item>
        /// <item><description>Interop.IDM.NEWPAGE             </description><description>    87</description></item>
        /// <item><description>Interop.IDM.MOVE                </description><description>    88</description></item>
        /// <item><description>Interop.IDM.CANCEL              </description><description>    89</description></item>
        /// <item><description>Interop.IDM.FONT                </description><description>    90</description></item>
        /// <item><description>Interop.IDM.STRIKETHROUGH       </description><description>    91</description></item>
        /// <item><description>Interop.IDM.DELETEWORD          </description><description>    92</description></item>
        /// <item><description>Interop.IDM.EXECPRINT           </description><description>    93</description></item>
        /// <item><description>Interop.IDM.JUSTIFYNONE         </description><description>    94</description></item>
        /// <item><description>Interop.IDM.TRISTATEBOLD        </description><description>    95</description></item>
        /// <item><description>Interop.IDM.TRISTATEITALIC      </description><description>    96</description></item>
        /// <item><description>Interop.IDM.TRISTATEUNDERLINE   </description><description>    97</description></item>
        /// <item><description>Interop.IDM.FOLLOW_ANCHOR        </description><description>   2008</description></item>
        /// <item><description>Interop.IDM.INSINPUTIMAGE         </description><description>  2114</description></item>
        /// <item><description>Interop.IDM.INSINPUTBUTTON        </description><description>  2115</description></item>
        /// <item><description>Interop.IDM.INSINPUTRESET         </description><description>  2116</description></item>
        /// <item><description>Interop.IDM.INSINPUTSUBMIT        </description><description>  2117</description></item>
        /// <item><description>Interop.IDM.INSINPUTUPLOAD        </description><description>  2118</description></item>
        /// <item><description>Interop.IDM.INSFIELDSET           </description><description>  2119</description></item>
        /// <item><description>Interop.IDM.PASTEINSERT          </description><description>   2120</description></item>
        /// <item><description>Interop.IDM.REPLACE              </description><description>   2121</description></item>
        /// <item><description>Interop.IDM.EDITSOURCE           </description><description>   2122</description></item>
        /// <item><description>Interop.IDM.BOOKMARK             </description><description>   2123</description></item>
        /// <item><description>Interop.IDM.HYPERLINK            </description><description>   2124</description></item>
        /// <item><description>Interop.IDM.UNLINK               </description><description>   2125</description></item>
        /// <item><description>Interop.IDM.BROWSEMODE           </description><description>   2126</description></item>
        /// <item><description>Interop.IDM.EDITMODE             </description><description>   2127</description></item>
        /// <item><description>Interop.IDM.UNBOOKMARK           </description><description>   2128</description></item>
        /// <item><description>Interop.IDM.TOOLBARS             </description><description>   2130</description></item>
        /// <item><description>Interop.IDM.STATUSBAR            </description><description>   2131</description></item>
        /// <item><description>Interop.IDM.FORMATMARK           </description><description>   2132</description></item>
        /// <item><description>Interop.IDM.TEXTONLY             </description><description>   2133</description></item>
        /// <item><description>Interop.IDM.OPTIONS              </description><description>   2135</description></item>
        /// <item><description>Interop.IDM.FOLLOWLINKC          </description><description>   2136</description></item>
        /// <item><description>Interop.IDM.FOLLOWLINKN          </description><description>   2137</description></item>
        /// <item><description>Interop.IDM.VIEWSOURCE           </description><description>   2139</description></item>
        /// <item><description>Interop.IDM.ZOOMPOPUP            </description><description>   2140</description></item>
        /// <item><description>Interop.IDM.BASELINEFONT1       </description><description>    2141</description></item>
        /// <item><description>Interop.IDM.BASELINEFONT2       </description><description>    2142</description></item>
        /// <item><description>Interop.IDM.BASELINEFONT3       </description><description>    2143</description></item>
        /// <item><description>Interop.IDM.BASELINEFONT4       </description><description>    2144</description></item>
        /// <item><description>Interop.IDM.BASELINEFONT5       </description><description>    2145</description></item>
        /// <item><description>Interop.IDM.HORIZONTALLINE      </description><description>    2150</description></item>
        /// <item><description>Interop.IDM.LINEBREAKNORMAL     </description><description>    2151</description></item>
        /// <item><description>Interop.IDM.LINEBREAKLEFT       </description><description>    2152</description></item>
        /// <item><description>Interop.IDM.LINEBREAKRIGHT      </description><description>    2153</description></item>
        /// <item><description>Interop.IDM.LINEBREAKBOTH       </description><description>    2154</description></item>
        /// <item><description>Interop.IDM.NONBREAK            </description><description>    2155</description></item>
        /// <item><description>Interop.IDM.SPECIALCHAR         </description><description>    2156</description></item>
        /// <item><description>Interop.IDM.HTMLSOURCE          </description><description>    2157</description></item>
        /// <item><description>Interop.IDM.IFRAME              </description><description>    2158</description></item>
        /// <item><description>Interop.IDM.HTMLCONTAIN         </description><description>    2159</description></item>
        /// <item><description>Interop.IDM.TEXTBOX             </description><description>    2161</description></item>
        /// <item><description>Interop.IDM.TEXTAREA            </description><description>    2162</description></item>
        /// <item><description>Interop.IDM.CHECKBOX            </description><description>    2163</description></item>
        /// <item><description>Interop.IDM.RADIOBUTTON         </description><description>    2164</description></item>
        /// <item><description>Interop.IDM.DROPDOWNBOX         </description><description>    2165</description></item>
        /// <item><description>Interop.IDM.LISTBOX             </description><description>    2166</description></item>
        /// <item><description>Interop.IDM.BUTTON              </description><description>    2167</description></item>
        /// <item><description>Interop.IDM.IMAGE               </description><description>    2168</description></item>
        /// <item><description>Interop.IDM.OBJECT              </description><description>    2169</description></item>
        /// <item><description>Interop.IDM.1D                  </description><description>    2170</description></item>
        /// <item><description>Interop.IDM.IMAGEMAP            </description><description>    2171</description></item>
        /// <item><description>Interop.IDM.FILE                </description><description>    2172</description></item>
        /// <item><description>Interop.IDM.COMMENT             </description><description>    2173</description></item>
        /// <item><description>Interop.IDM.SCRIPT              </description><description>    2174</description></item>
        /// <item><description>Interop.IDM.JAVAAPPLET          </description><description>    2175</description></item>
        /// <item><description>Interop.IDM.PLUGIN              </description><description>    2176</description></item>
        /// <item><description>Interop.IDM.PAGEBREAK           </description><description>    2177</description></item>
        /// <item><description>Interop.IDM.HTMLAREA            </description><description>    2178</description></item>
        /// <item><description>Interop.IDM.PARAGRAPH           </description><description>    2180</description></item>
        /// <item><description>Interop.IDM.FORM                </description><description>    2181</description></item>
        /// <item><description>Interop.IDM.MARQUEE             </description><description>    2182</description></item>
        /// <item><description>Interop.IDM.LIST                </description><description>    2183</description></item>
        /// <item><description>Interop.IDM.ORDERLIST           </description><description>    2184</description></item>
        /// <item><description>Interop.IDM.UNORDERLIST         </description><description>    2185</description></item>
        /// <item><description>Interop.IDM.INDENT              </description><description>    2186</description></item>
        /// <item><description>Interop.IDM.OUTDENT             </description><description>    2187</description></item>
        /// <item><description>Interop.IDM.PREFORMATTED        </description><description>    2188</description></item>
        /// <item><description>Interop.IDM.ADDRESS             </description><description>    2189</description></item>
        /// <item><description>Interop.IDM.BLINK               </description><description>    2190</description></item>
        /// <item><description>Interop.IDM.DIV                 </description><description>    2191</description></item>
        /// <item><description>Interop.IDM.TABLEINSERT         </description><description>    2200</description></item>
        /// <item><description>Interop.IDM.RCINSERT            </description><description>    2201</description></item>
        /// <item><description>Interop.IDM.CELLINSERT          </description><description>    2202</description></item>
        /// <item><description>Interop.IDM.CAPTIONINSERT       </description><description>    2203</description></item>
        /// <item><description>Interop.IDM.CELLMERGE           </description><description>    2204</description></item>
        /// <item><description>Interop.IDM.CELLSPLIT           </description><description>    2205</description></item>
        /// <item><description>Interop.IDM.CELLSELECT          </description><description>    2206</description></item>
        /// <item><description>Interop.IDM.ROWSELECT           </description><description>    2207</description></item>
        /// <item><description>Interop.IDM.COLUMNSELECT        </description><description>    2208</description></item>
        /// <item><description>Interop.IDM.TABLESELECT         </description><description>    2209</description></item>
        /// <item><description>Interop.IDM.TABLEPROPERTIES     </description><description>    2210</description></item>
        /// <item><description>Interop.IDM.CELLPROPERTIES      </description><description>    2211</description></item>
        /// <item><description>Interop.IDM.ROWINSERT           </description><description>    2212</description></item>
        /// <item><description>Interop.IDM.COLUMNINSERT        </description><description>    2213</description></item>
        /// <item><description>Interop.IDM.HELP_CONTENT         </description><description>   2220</description></item>
        /// <item><description>Interop.IDM.HELP_ABOUT           </description><description>   2221</description></item>
        /// <item><description>Interop.IDM.HELP_README          </description><description>   2222</description></item>
        /// <item><description>Interop.IDM.REMOVEFORMAT          </description><description>  2230</description></item>
        /// <item><description>Interop.IDM.PAGEINFO             </description><description>   2231</description></item>
        /// <item><description>Interop.IDM.TELETYPE             </description><description>   2232</description></item>
        /// <item><description>Interop.IDM.GETBLOCKFMTS          </description><description>  2233</description></item>
        /// <item><description>Interop.IDM.BLOCKFMT              </description><description>  2234</description></item>
        /// <item><description>Interop.IDM.SHOWHIDE_CODE         </description><description>  2235</description></item>
        /// <item><description>Interop.IDM.TABLE                 </description><description>  2236</description></item>
        /// <item><description>Interop.IDM.COPYFORMAT             </description><description> 2237</description></item>
        /// <item><description>Interop.IDM.PASTEFORMAT         </description><description>    2238</description></item>
        /// <item><description>Interop.IDM.GOTO                 </description><description>   2239</description></item>
        /// <item><description>Interop.IDM.CHANGEFONT            </description><description>  2240</description></item>
        /// <item><description>Interop.IDM.CHANGEFONTSIZE        </description><description>  2241</description></item>
        /// <item><description>Interop.IDM.CHANGECASE            </description><description>  2246</description></item>
        /// <item><description>Interop.IDM.SHOWSPECIALCHAR       </description><description>  2249</description></item>
        /// <item><description>Interop.IDM.SUBSCRIPT             </description><description>  2247</description></item>
        /// <item><description>Interop.IDM.SUPERSCRIPT           </description><description>  2248</description></item>
        /// <item><description>Interop.IDM.CENTERALIGNPARA       </description><description>  2250</description></item>
        /// <item><description>Interop.IDM.LEFTALIGNPARA         </description><description>  2251</description></item>
        /// <item><description>Interop.IDM.RIGHTALIGNPARA        </description><description>  2252</description></item>
        /// <item><description>Interop.IDM.REMOVEPARAFORMAT      </description><description>  2253</description></item>
        /// <item><description>Interop.IDM.APPLYNORMAL           </description><description>  2254</description></item>
        /// <item><description>Interop.IDM.APPLYHEADING1         </description><description>  2255</description></item>
        /// <item><description>Interop.IDM.APPLYHEADING2         </description><description>  2256</description></item>
        /// <item><description>Interop.IDM.APPLYHEADING3         </description><description>  2257</description></item>
        /// <item><description>Interop.IDM.DOCPROPERTIES         </description><description>  2260</description></item>
        /// <item><description>Interop.IDM.ADDFAVORITES          </description><description>  2261</description></item>
        /// <item><description>Interop.IDM.COPYSHORTCUT          </description><description>  2262</description></item>
        /// <item><description>Interop.IDM.SAVEBACKGROUND        </description><description>  2263</description></item>
        /// <item><description>Interop.IDM.SETWALLPAPER          </description><description>  2264</description></item>
        /// <item><description>Interop.IDM.COPYBACKGROUND        </description><description>  2265</description></item>
        /// <item><description>Interop.IDM.CREATESHORTCUT        </description><description>  2266</description></item>
        /// <item><description>Interop.IDM.PAGE                  </description><description>  2267</description></item>
        /// <item><description>Interop.IDM.SAVETARGET            </description><description>  2268</description></item>
        /// <item><description>Interop.IDM.SHOWPICTURE           </description><description>  2269</description></item>
        /// <item><description>Interop.IDM.SAVEPICTURE           </description><description>  2270</description></item>
        /// <item><description>Interop.IDM.DYNSRCPLAY            </description><description>  2271</description></item>
        /// <item><description>Interop.IDM.DYNSRCSTOP            </description><description>  2272</description></item>
        /// <item><description>Interop.IDM.PRINTTARGET           </description><description>  2273</description></item>
        /// <item><description>Interop.IDM.IMGARTPLAY            </description><description>  2274</description></item>
        /// <item><description>Interop.IDM.IMGARTSTOP            </description><description>  2275</description></item>
        /// <item><description>Interop.IDM.IMGARTREWIND          </description><description>  2276</description></item>
        /// <item><description>Interop.IDM.PRINTQUERYJOBSPENDING </description><description>  2277</description></item>
        /// <item><description>Interop.IDM.SETDESKTOPITEM        </description><description>  2278</description></item>
        /// <item><description>Interop.IDM.CONTEXTMENU           </description><description>  2280</description></item>
        /// <item><description>Interop.IDM.GOBACKWARD            </description><description>  2282</description></item>
        /// <item><description>Interop.IDM.GOFORWARD             </description><description>  2283</description></item>
        /// <item><description>Interop.IDM.PRESTOP               </description><description>  2284</description></item>
        /// <item><description>Interop.IDM.MP_MYPICS             </description><description>  2287</description></item>
        /// <item><description>Interop.IDM.MP_EMAILPICTURE       </description><description>  2288</description></item>
        /// <item><description>Interop.IDM.MP_PRINTPICTURE       </description><description>  2289</description></item>
        /// <item><description>Interop.IDM.CREATELINK           </description><description>   2290</description></item>
        /// <item><description>Interop.IDM.COPYCONTENT          </description><description>   2291</description></item>
        /// <item><description>Interop.IDM.LANGUAGE             </description><description>   2292</description></item>
        /// <item><description>Interop.IDM.GETPRINTTEMPLATE    </description><description>    2295</description></item>
        /// <item><description>Interop.IDM.SETPRINTTEMPLATE    </description><description>    2296</description></item>
        /// <item><description>Interop.IDM.TEMPLATE_PAGESETUP  </description><description>    2298</description></item>
        /// <item><description>Interop.IDM.REFRESH              </description><description>   2300</description></item>
        /// <item><description>Interop.IDM.STOPDOWNLOAD         </description><description>   2301</description></item>
        /// <item><description>Interop.IDM.ENABLE_INTERACTION    </description><description>  2302</description></item>
        /// <item><description>Interop.IDM.LAUNCHDEBUGGER         </description><description> 2310</description></item>
        /// <item><description>Interop.IDM.BREAKATNEXT            </description><description> 2311</description></item>
        /// <item><description>Interop.IDM.INSINPUTHIDDEN        </description><description>  2312</description></item>
        /// <item><description>Interop.IDM.INSINPUTPASSWORD      </description><description>  2313</description></item>
        /// <item><description>Interop.IDM.OVERWRITE          </description><description>     2314</description></item>
        /// <item><description>Interop.IDM.PARSECOMPLETE       </description><description>    2315</description></item>
        /// <item><description>Interop.IDM.HTMLEDITMODE       </description><description>     2316</description></item>
        /// <item><description>Interop.IDM.REGISTRYREFRESH      </description><description>   2317</description></item>
        /// <item><description>Interop.IDM.COMPOSESETTINGS      </description><description>   2318</description></item>
        /// <item><description>Interop.IDM.SHOWALLTAGS           </description><description>  2327</description></item>
        /// <item><description>Interop.IDM.SHOWALIGNEDSITETAGS   </description><description>  2321</description></item>
        /// <item><description>Interop.IDM.SHOWSCRIPTTAGS        </description><description>  2322</description></item>
        /// <item><description>Interop.IDM.SHOWSTYLETAGS         </description><description>  2323</description></item>
        /// <item><description>Interop.IDM.SHOWCOMMENTTAGS       </description><description>  2324</description></item>
        /// <item><description>Interop.IDM.SHOWAREATAGS          </description><description>  2325</description></item>
        /// <item><description>Interop.IDM.SHOWUNKNOWNTAGS       </description><description>  2326</description></item>
        /// <item><description>Interop.IDM.SHOWMISCTAGS          </description><description>  2320</description></item>
        /// <item><description>Interop.IDM.SHOWZEROBORDERATDESIGNTIME  </description><description>       2328</description></item>
        /// <item><description>Interop.IDM.AUTODETECT         </description><description>     2329</description></item>
        /// <item><description>Interop.IDM.SCRIPTDEBUGGER     </description><description>     2330</description></item>
        /// <item><description>Interop.IDM.GETBYTESDOWNLOADED  </description><description>    2331</description></item>
        /// <item><description>Interop.IDM.NOACTIVATENORMALOLECONTROLS   </description><description>     2332</description></item>
        /// <item><description>Interop.IDM.NOACTIVATEDESIGNTIMECONTROLS  </description><description>     2333</description></item>
        /// <item><description>Interop.IDM.NOACTIVATEJAVAAPPLETS          </description><description>    2334</description></item>
        /// <item><description>Interop.IDM.NOFIXUPURLSONPASTE              </description><description>   2335</description></item>
        /// <item><description>Interop.IDM.EMPTYGLYPHTABLE   </description><description>      2336</description></item>
        /// <item><description>Interop.IDM.ADDTOGLYPHTABLE   </description><description>      2337</description></item>
        /// <item><description>Interop.IDM.REMOVEFROMGLYPHTABLE </description><description>   2338</description></item>
        /// <item><description>Interop.IDM.REPLACEGLYPHCONTENTS  </description><description>  2339</description></item>
        /// <item><description>Interop.IDM.SHOWWBRTAGS            </description><description> 2340</description></item>
        /// <item><description>Interop.IDM.PERSISTSTREAMSYNC      </description><description> 2341</description></item>
        /// <item><description>Interop.IDM.SETDIRTY              </description><description>  2342</description></item>
        /// <item><description>Interop.IDM.RUNURLSCRIPT       </description><description>     2343</description></item>
        /// <item><description>Interop.IDM.ZOOMRATIO          </description><description>     2344</description></item>
        /// <item><description>Interop.IDM.GETZOOMNUMERATOR    </description><description>    2345</description></item>
        /// <item><description>Interop.IDM.GETZOOMDENOMINATOR   </description><description>   2346</description></item>
        /// <item><description>Interop.IDM.DIRLTR                </description><description>  2350</description></item>
        /// <item><description>Interop.IDM.DIRRTL               </description><description>   2351</description></item>
        /// <item><description>Interop.IDM.BLOCKDIRLTR          </description><description>   2352</description></item>
        /// <item><description>Interop.IDM.BLOCKDIRRTL          </description><description>   2353</description></item>
        /// <item><description>Interop.IDM.INLINEDIRLTR         </description><description>   2354</description></item>
        /// <item><description>Interop.IDM.INLINEDIRRTL         </description><description>   2355</description></item>
        /// <item><description>Interop.IDM.ISTRUSTEDDLG     </description><description>       2356</description></item>
        /// <item><description>Interop.IDM.INSERTSPAN        </description><description>      2357</description></item>
        /// <item><description>Interop.IDM.LOCALIZEEDITOR     </description><description>     2358</description></item>
        /// <item><description>Interop.IDM.SAVEPRETRANSFORMSOURCE </description><description> 2370</description></item>
        /// <item><description>Interop.IDM.VIEWPRETRANSFORMSOURCE </description><description> 2371</description></item>
        /// <item><description>Interop.IDM.SCROLL_HERE            </description><description> 2380</description></item>
        /// <item><description>Interop.IDM.SCROLL_TOP             </description><description> 2381</description></item>
        /// <item><description>Interop.IDM.SCROLL_BOTTOM          </description><description> 2382</description></item>
        /// <item><description>Interop.IDM.SCROLL_PAGEUP          </description><description> 2383</description></item>
        /// <item><description>Interop.IDM.SCROLL_PAGEDOWN        </description><description> 2384</description></item>
        /// <item><description>Interop.IDM.SCROLL_UP              </description><description> 2385</description></item>
        /// <item><description>Interop.IDM.SCROLL_DOWN            </description><description> 2386</description></item>
        /// <item><description>Interop.IDM.SCROLL_LEFTEDGE        </description><description> 2387</description></item>
        /// <item><description>Interop.IDM.SCROLL_RIGHTEDGE       </description><description> 2388</description></item>
        /// <item><description>Interop.IDM.SCROLL_PAGELEFT        </description><description> 2389</description></item>
        /// <item><description>Interop.IDM.SCROLL_PAGERIGHT       </description><description> 2390</description></item>
        /// <item><description>Interop.IDM.SCROLL_LEFT            </description><description> 2391</description></item>
        /// <item><description>Interop.IDM.SCROLL_RIGHT           </description><description> 2392</description></item>
        /// <item><description>Interop.IDM.MULTIPLESELECTION      </description><description> 2393</description></item>
        /// <item><description>Interop.IDM.TWOD_POSITION            </description><description> 2394</description></item>
        /// <item><description>Interop.IDM.TWOD_ELEMENT             </description><description> 2395</description></item>
        /// <item><description>Interop.IDM.ONED_ELEMENT             </description><description> 2396</description></item>
        /// <item><description>Interop.IDM.ABSOLUTE_POSITION      </description><description> 2397</description></item>
        /// <item><description>Interop.IDM.LIVERESIZE             </description><description> 2398</description></item>
        /// <item><description>Interop.IDM.ATOMICSELECTION	</description><description>		2399</description></item>
        /// <item><description>Interop.IDM.AUTOURLDETECT_MODE  </description><description>    2400</description></item>
        /// <item><description>Interop.IDM.IE50_PASTE          </description><description>    2401</description></item>
        /// <item><description>Interop.IDM.IE50_PASTE_MODE      </description><description>   2402</description></item>
        /// <item><description>Interop.IDM.DISABLE_EDITFOCUS_UI   </description><description> 2404</description></item>
        /// <item><description>Interop.IDM.RESPECTVISIBILITY_INDESIGN </description><description> 2405</description></item>
        /// <item><description>Interop.IDM.CSSEDITING_LEVEL         </description><description>   2406</description></item>
        /// <item><description>Interop.IDM.UI_OUTDENT                </description><description>  2407</description></item>
        /// <item><description>Interop.IDM.UPDATEPAGESTATUS           </description><description> 2408</description></item>
        /// <item><description>Interop.IDM.IME_ENABLE_RECONVERSION	</description><description>	2409</description></item>
        /// <item><description>Interop.IDM.KEEPSELECTION			</description><description>	2410</description></item>
        /// <item><description>Interop.IDM.UNLOADDOCUMENT             </description><description> 2411</description></item>
        /// <item><description>Interop.IDM.OVERRIDE_CURSOR            </description><description> 2420</description></item>
        /// <item><description>Interop.IDM.PEERHITTESTSAMEINEDIT      </description><description> 2423</description></item>
        /// <item><description>Interop.IDM.TRUSTAPPCACHE              </description><description> 2425</description></item>
        /// <item><description>Interop.IDM.BACKGROUNDIMAGECACHE       </description><description> 2430</description></item>
        /// <item><description>Interop.IDM.DEFAULTBLOCK           </description><description>     6046</description></item>
        /// <item><description>Interop.IDM.MIMECSET__FIRST__       </description><description>    3609</description></item>
        /// <item><description>Interop.IDM.MIMECSET__LAST__         </description><description>   3699</description></item>
        /// <item><description>Interop.IDM.MENUEXT_FIRST__   </description><description>    3700</description></item>
        /// <item><description>Interop.IDM.MENUEXT_LAST__     </description><description>   3732</description></item>
        /// <item><description>Interop.IDM.MENUEXT_COUNT       </description><description>  3733</description></item>
        /// <item><description>Interop.IDM.OPEN                </description><description>    2000</description></item>
        /// <item><description>Interop.IDM.NEW                 </description><description>    2001</description></item>
        /// <item><description>Interop.IDM.SAVE                </description><description>    70</description></item>
        /// <item><description>Interop.IDM.SAVEAS              </description><description>    71</description></item>
        /// <item><description>Interop.IDM.SAVECOPYAS          </description><description>    2002</description></item>
        /// <item><description>Interop.IDM.PRINTPREVIEW        </description><description>    2003</description></item>
        /// <item><description>Interop.IDM.SHOWPRINT           </description><description>    2010</description></item>
        /// <item><description>Interop.IDM.SHOWPAGESETUP       </description><description>    2011</description></item>
        /// <item><description>Interop.IDM.PRINT               </description><description>    27</description></item>
        /// <item><description>Interop.IDM.PAGESETUP           </description><description>    2004</description></item>
        /// <item><description>Interop.IDM.SPELL               </description><description>    2005</description></item>
        /// <item><description>Interop.IDM.PASTESPECIAL        </description><description>    2006</description></item>
        /// <item><description>Interop.IDM.CLEARSELECTION      </description><description>    2007</description></item>
        /// <item><description>Interop.IDM.PROPERTIES          </description><description>    28</description></item>
        /// <item><description>Interop.IDM.REDO                </description><description>    29</description></item>
        /// <item><description>Interop.IDM.UNDO                </description><description>    43</description></item>
        /// <item><description>Interop.IDM.SELECTALL           </description><description>    31</description></item>
        /// <item><description>Interop.IDM.ZOOMPERCENT         </description><description>    50</description></item>
        /// <item><description>Interop.IDM.GETZOOM             </description><description>    68</description></item>
        /// <item><description>Interop.IDM.STOP                </description><description>    2138</description></item>
        /// <item><description>Interop.IDM.COPY                </description><description>    15</description></item>
        /// <item><description>Interop.IDM.CUT                 </description><description>    16</description></item>
        /// <item><description>Interop.IDM.PASTE               </description><description>    26</description></item>
        /// <item><description>Interop.IDM.PERSISTDEFAULTVALUES </description><description> 7100</description></item>
        /// <item><description>Interop.IDM.PROTECTMETATAGS </description><description>  7101</description></item>
        /// <item><description>Interop.IDM.PRESERVEUNDOALWAYS </description><description> 6049</description></item>
        /// </list>
        /// Note: The appereance in this list does not mean that a particular command works either in NetRix or
        /// MSHTML. Additionally, some commands expect parameters which are currently not supported by this
        /// method.
        /// </remarks>
        /// <param name="command">The IDM command ID sent to the control.</param>
        void SendIDMCommand(int command);

        # region HTML Element

        /// <summary>
        /// Returns an array of element objects which contains all element with the given tag name. 
        /// </summary>
        /// <remarks>
        /// If no such elements the method will return null.
        /// </remarks>
        /// <param name="tagName">The name of the specific tag which the method should search.</param>
        /// <returns>The <see cref="ElementCollection"/> of <see cref="IElement"/> objects or null, if no elements found.</returns>
        ElementCollection GetElementsByTagName(string tagName);

        /// <summary>
        /// Returns the element with the given Id as native element object.
        /// </summary>
        /// <param name="Id">The ID value which the method should search for.</param>
        /// <returns>The element, if found or <c>null</c>, if there is no such element.</returns>
        IElement GetElementById(string Id);

        /// <summary>
        /// Returns all element of the current page.
        /// </summary>
        /// <returns>The collection of all elements.</returns>
        ElementCollection GetAllElements();        

        /// <summary>
        /// Returns the current BodyElement object from the document.
        /// </summary>
        /// <returns>BodyElement, if any or null if no body is present or document is not ready.</returns>
        IElement GetBodyElement();

        # endregion

        /// <summary>
        /// Gets the vertical scroll position.
        /// </summary>
        /// <remarks>
        /// The vertical scroll position is the distance from the document top
        /// to the current visible top border, measured in pixel. If the scrollbar is
        /// at the top position, this value is 0.
        /// </remarks>
        int VerticalScrollPosition { get; }

        /// <summary>
        /// Gets the horizontal scroll position. 
        /// </summary>
        /// <remarks>
        /// The horizontal scroll position is the distance from the document left
        /// to the current visible left border, measured in pixel. If the scrollbar is
        /// at the left position, this value is 0.
        /// </remarks>
        int HorizontalScrollPosition { get; }


        /// <summary>
        /// Gets/Sets if the control can receive the focus.
        /// </summary>
        /// <remarks>
        /// The purpose of the property is to temporarily hook of the control from the
        /// TAB list of controls.
        /// </remarks>
        bool ActivationEnabled { get; set; }

        /// <summary>
        /// Indicates if the control is ready for use.
        /// </summary>
        /// <remarks>
        /// <para>
        /// If the document was a frameset, the complete handling is done in HtmlFrameSet class. This
        /// class can set the IsReady property exclusivly to true, if the frame document is ready.
        /// This assures that subsequent calls / events used from the base classes, like <see cref="GuruComponents.Netrix.IHtmlEditor.Selection"/>
        /// work properly as they normally throw exceptions if methods are called for an unready document.
        /// </para>
        /// </remarks>
        bool IsReady { get; }

        /// <summary>
        /// Indicates if the contents of the editor have been modified.
        /// </summary>
        /// <remarks>
        /// This flag is set to <c>false</c> after a document or document fragment wss loaded. It becomes
        /// <c>true</c> after the first change in the documents structure or content. Moving the caret or
        /// clicking objects without moving them wil not set the flag to dirty state.
        /// <para>
        /// The flag will be reset on each save operation. This is done by calling the RawHtml content by
        /// any of the methods 
        /// <see cref="GuruComponents.Netrix.IHtmlEditor.SaveHtml">SaveHtml</see> or 
        /// <see cref="GuruComponents.Netrix.IHtmlEditor.SaveFormattedHtml(Stream)">SaveFormattedHtml</see>. Whereas the
        /// SaveXXX methods really write content into a stream the 
        /// <see cref="GetRawHtml()">GetRawHtml</see> or
        /// <see cref="GetFormattedHtml(IHtmlFormatterOptions)">GetFormattedHtml</see>, 
        /// method just returns the content to the caller. Both method are overloaded and provide a second
        /// parameter to force resetting the dirty flag. If this is done the host application should assure that the
        /// content is really saved, to keep the flag and the UI synchronized.        
        /// </para>
        /// </remarks>
        bool IsDirty  { get; }

        /// <summary>
        /// Activates or deactivates the internal shortcut keys.
        /// </summary>
        /// <remarks>
        /// If the property is on (default), the following keys are static assigned:
        /// <list type="table">
        /// <listheader>
        /// <item>Key</item><item>Description</item><item>Design Mode Only</item>
        /// </listheader>
        /// <item>Ctrl-A</item><item>Select all</item><item>-</item>
        /// <item>Ctrl-C</item><item>Copy selected text</item><item>-</item>
        /// <item>Ctrl-P</item><item>Print immediately using internal dialogs.</item><item>-</item>
        /// <item>Ctrl-U</item><item>Underline</item><item>+</item>
        /// <item>Ctrl-B</item><item>Bold</item><item>+</item>
        /// <item>Ctrl-I</item><item>Italic</item><item>+</item>
        /// <item>Ctrl-L</item><item>Align Left (Block only)</item><item>+</item>
        /// <item>Ctrl-R</item><item>Align Right (Block only)</item><item>+</item>
        /// <item>Ctrl-J</item><item>Justify (Multiline Block only)</item><item>+</item>
        /// <item>Ctrl-V</item><item>Paste</item><item>+</item>
        /// <item>Ctrl-X</item><item>Cut</item><item>+</item>
        /// <item>Ctrl-Y</item><item>Redo</item><item>+</item>
        /// <item>Ctrl-Z</item><item>Undo</item><item>+</item>
        /// </list>
        /// The property can be turned on and off during run time and will become active
        /// immediately. 
        /// <para>
        /// To strip out specific keys you can handle the 
        /// <see cref="GuruComponents.Netrix.IHtmlEditor.BeforeShortcut">BeforeShortcut</see> event
        /// and set the <c>Cancel</c> property to true to disable the internal key handling. The same
        /// event can be used to assign more Shortcut keys in your application, because the event
        /// is fired on every kex in combination with the Ctrl (Control) key, even if the key stroke
        /// is not handled elsewhere.
        /// </para>
        /// </remarks>
        bool InternalShortcutKeys { get; set; }

        /// <summary>
        /// The encoding of the document.
        /// </summary>
        /// <remarks>
        /// The encoding can be changed during design time to change the document encoding with next save operation.
        /// </remarks>
        /// <example>
        /// Changing the encoding is very easy. The following line will set the encoding for all following save operations to UTF-8:
        /// <code>
        /// this.htmlEditor1.Encoding = System.Text.Encoding.UTF8;
        /// </code>
        /// If you load a text from a string with non-Ascii characters the automatic encoding recognition needs
        /// to knwo which culture should be used. Therefore it is important to set the CurrentCulture property of
        /// the current thread correctly. Even if the most systems run correctly it is recommended to set the 
        /// current culture in all environments to a specific value.
        /// </example>
        Encoding Encoding { get; set; }

        /// <summary>
        /// Does not execute any script until fully activated. Browse Mode only.
        /// </summary>
        bool ScriptEnabled { get; set; } 

        /// <summary>
        /// Enables or disables scrollbars.
        /// </summary>
        bool ScrollBarsEnabled { get; set; } 

        /// <summary>
        /// Gets the full decoded path to the file this document is load locally from.
        /// </summary>
        /// <remarks>
        /// This can be an URL, but under most circumstances this may be a Path. Any monikers of type "file://" are removed.
        /// </remarks>
        /// <returns></returns>
        string GetFullPathUrl();

        # region Control Design and Behavior

        /// <summary>
        /// Show component borders in 3D style.
        /// </summary>
        /// <remarks>
        /// The default is true (3D style used). Changing this property requieres re-load.
        /// </remarks>
        [DefaultValue(false), Browsable(true), Category("NetRix Component"), Description("Show component borders in 3D style. The default is true (3D style used).")]
        bool Border3d { get; set; }

        /// <summary>
        /// Controls the way the component renders themeable controls.
        /// </summary>
        /// <remarks>
        /// Buttons, Select boxes and other form elements can be rendered during
        /// design time using XP themes, if the application runs on XP. If the designer
        /// is used to design pages which run later on other operating systems,
        /// it is recommended to switch theming support of.
        /// <para>
        /// The default value is <c>true</c> (Theming is on).
        /// </para>
        /// </remarks>
        [Browsable(true), DefaultValue(true)]
        [Category("NetRix Component"), Description("Controls the way the component renders themeable controls.")]
        bool XPTheming { get; set; }

        /// <summary>
        /// Sets how paragraphs will be inserted, DIV or P tags.
        /// </summary>
        /// <remarks>
        /// <seealso cref="GuruComponents.Netrix.BlockDefaultType"/>
        /// </remarks>
        [Browsable(true), DefaultValue(BlockDefaultType.P)]
        [Category("NetRix Component"), Description("Sets how paragraphs will be inserted, DIV or P tags.")]
        BlockDefaultType BlockDefault { get; set; }

        /// <summary>
        /// Set the behavior of links in browse mode.
        /// </summary>
        /// <remarks>
        /// If this property is set to <c>true</c> an the user clicks a link in browse mode the new page
        /// will load internally and replace the current page. If set to <c>false</c> the link will start
        /// a new instance of Internet Explorer (or the systems default browser) and opens the link 
        /// externally. Any further navigation operations are no longer under the control of NetRix.
        /// <para>
        /// This option is not recognized in design mode.
        /// </para>
        /// </remarks>
        [DefaultValue(true), Browsable(true), Category("NetRix Component"), Description("Allow to navigate in design mode without calling external browser.")]
        bool AllowInPlaceNavigation { get; set; }

        /// <summary>
        /// Gets or sets a value to determine full document mode.
        /// </summary>
        /// <remarks>
        /// The full document mode forces the basic structure of the loaded HTML document to 
        /// this one:
        /// <code>
        /// &lt;html&gt;
        /// &lt;body&gt;
        ///   &lt;!-- your HTML fragment goes here --&gt;
        /// &lt;/body&gt;
        /// &lt;/html&gt;
        /// </code>
        /// To avoid this behavior just set this property to <c>false</c> and reload the document.
        /// <para>
        /// Changing this property during an editing session will not be recognized.
        /// </para>
        /// </remarks>
        [Browsable(false), DefaultValue(true)]
        bool IsFullDocumentMode { get; set; } 

        # endregion

        # region Editor Commands

        /// <summary>
        /// Indicates if the current selection can be copied.
        /// </summary>
        [Browsable(false)]
        bool CanCopy { get; }

        /// <summary>
        /// Indicates if the current selection can be cut.
        /// </summary>
        [Browsable(false)]
        bool CanCut { get; }

        /// <summary>
        /// Indicates if the current selection can be pasted to.
        /// </summary>
        [Browsable(false)]
        bool CanPaste { get; }

        /// <summary>
        /// Indicates if the current selection can be deleted.
        /// </summary>
        [Browsable(false)]
        bool CanDelete { get; }

        /// <summary>
        /// Indicates if the editor can redo.
        /// </summary>
        [Browsable(false)]
        bool CanRedo { get; }

        /// <summary>
        /// Indicates if the editor can undo.
        /// </summary>
        [Browsable(false)]
        bool CanUndo { get; }
        
        /// <summary>
        /// Copy the current selection.
        /// </summary>
        /// <remarks>
        /// The command does nothing if the <see cref="CanCopy"/> property returns false.
        /// </remarks>
        void Copy();

        /// <summary>
        /// Cut the current selection
        /// </summary>
        /// <remarks>
        /// The command does nothing if the <see cref="CanCut"/> property returns false.
        /// </remarks>
        void Cut();

        /// <summary>
        /// Cut the current selection.
        /// </summary>
        void Paste();

        /// <summary>
        /// Delete the current selection.
        /// </summary>
        void Delete();

        /// <summary>
        /// Shows flat or normal themed scrollbars
        /// </summary>
        [Browsable(true), DefaultValue(false)]
        [Category("NetRix Component"), Description("Shows flat or normal themed scrollbars")]
        bool FlatScrollBars { get; set; } 
        
        # endregion

        #endregion

        #region +++++ Block: Find and Replace

        /// <summary>
        /// Searches and replaces a string.
        /// </summary>
        /// <param name="searchString">String to search for</param>
        /// <param name="replaceString">String to replace with</param>
        /// <param name="matchCase">true if case sensitive</param>
        /// <param name="wholeWord">true if only whole words (search string matches word boundaries) found</param>
        /// <param name="searchUp">true if search upwards, false otherwise</param>
        /// <returns>Number of replaces</returns>
        int Replace(string searchString, string replaceString, bool matchCase, bool wholeWord, bool searchUp);

        /// <summary>
        /// Searches and replaces a string.
        /// </summary>
        /// <param name="searchString">String to search for</param>
        /// <param name="replaceString">String to replace with</param>
        /// <param name="matchCase">true if case sensitive</param>
        /// <param name="wholeWord">true if only whole words (search string matches word boundaries) found</param>
        /// <param name="searchUp">true if search upwards, false otherwise</param>
        /// <param name="maxReplacements">Maximum number of replacments.</param>
        /// <returns>Number of replaces</returns>
        int Replace(string searchString, string replaceString, bool matchCase, bool wholeWord, bool searchUp, int maxReplacements);

        /// <summary>
        /// Replaces the current selection with a new string.
        /// </summary>
        /// <remarks>The string can have any length. An empty string is allowed.
        /// The method will do nothing if there is no valid text selection. 
        /// <para>
        /// If the selection contains tags or block elements they will be replaced, too. If this results in invalid HTML 
        /// the method can fire an exception or produce unpredictable results. It is strongly recommended to read
        /// <see cref="ISelection.Text"/> and <see cref="ISelection.Html"/> to
        /// see the current content of the selection and start replacement only if applicable.
        /// </para>
        /// </remarks>
        /// <param name="replaceString">The string which replaces the current selection.</param>
        void ReplaceSelection(string replaceString);

        /// <summary>Replaces the next occurence of the give string.</summary>
        /// <remarks>
        /// This method does not fire any events. After the replacement takes place the method ends. There is no information
        /// whether the replacement was successful. The purpose of this method is to interact with <see cref="Find"/>.
        /// </remarks>
        /// <param name="searchString">String to search for</param>
        /// <param name="replaceString">String to replace with</param>
        /// <param name="matchCase">true if case sensitive</param>
        /// <param name="wholeWord">true if only whole words (search string matches word boundaries) found</param>
        /// <param name="backwards">true if search upwards, false otherwise</param>
        /// <returns>Number of replaces</returns>
        void ReplaceNext(string searchString, string replaceString, bool matchCase, bool wholeWord, bool backwards);

        /// <summary>
        /// Searches for a text string in the whole document.
        /// </summary>
        /// <remarks>
        /// Searches document for a given string and, if the string was found, returns true. The search stops at the first 
        /// hit and starts from the last stop. Subsequent calls jump from hit to hit. The document will automatically 
        /// scroll to the marked position.
        /// </remarks>
        /// <example>
        /// The following code assumes that you have a dialog created which provides the following properties:
        /// <list type="bullet">
        /// <item>
        ///     <term>WithReplace</term>
        ///     <description>A boolean value (supported by a checkbox) which turn replacement on.</description>
        /// </item>
        /// <item>
        ///     <term>Search</term>
        ///     <description>A string value (supported by a textbox) which returns the phrase we're searching for.</description>
        /// </item>
        /// <item>
        ///     <term>Replace</term>
        ///     <description>A string value (supported by a textbox) which returns the characters we use for replacement.</description>
        /// </item>
        /// <item>
        ///     <term>Match</term>
        ///     <description>A boolean value (supported by a checkbox) which indicates that the search is case sensitive.</description>
        /// </item>
        /// <item>
        ///     <term>Word</term>
        ///     <description>A boolean value (supported by a checkbox) which delimites the search to whole words.</description>
        /// </item>
        /// <item>
        ///     <term>Up</term>
        ///     <description>A boolean value (supported by a checkbox). The search runs upwards if turned on.</description>
        /// </item>
        /// </list>
        /// <code>
        /// if (SearchReplaceDialog.ShowDialog() == DialogResult.OK)
        /// {        
        ///     if (SearchReplaceDialog.WithReplace)
        ///     {
        ///         int c = this.htmlEditor1.Replace(SearchReplaceDialog.Search, SearchReplaceDialog.Replace, SearchReplaceDialog.Match, SearchReplaceDialog.Word, SearchReplaceDialog.Up);
        ///         MessageBox.Show(c.ToString() + " strings found and replaced");
        ///     } 
        ///     else 
        ///     {
        ///         bool r = this.htmlEditor1.Find(SearchReplaceDialog.Search, SearchReplaceDialog.Match, SearchReplaceDialog.Word, SearchReplaceDialog.Up);
        ///         if (!r)
        ///         {
        ///             MessageBox.Show("String '"+SearchReplaceDialog.Search+"' not found");
        ///         }
        ///     }
        /// }
        /// </code>
        /// </example>
        /// <param name="searchString">A string to search for.</param>
        /// <param name="matchCase">Must be set to <c>true</c> if match case is required.</param>
        /// <param name="wholeWord">Must set to <c>true</c> if only whole words should be found.</param>
        /// <param name="searchUp">Must set to <c>true</c> to search backwards.</param>
        /// <returns>Returns <c>true</c> if something was found, <c>false</c> otherwise.</returns>
        bool Find(string searchString, bool matchCase, bool wholeWord, bool searchUp);

        #endregion

        # region +++++ Block: Load, Save and Authentication

        /// <summary>
        /// Gets the url of the document contained in the control.
        /// </summary>
        /// <remarks>
        /// If there is no path defined, the property returns <c>null</c>.
        /// </remarks>
        string Url { get; set; }

        /// <summary>
        /// Returns the currently used temporary file, if any.
        /// </summary>
        string TempFile { get; }

        /// <summary>
        /// Reset the dirty state flag to inform the control that the current state is
        /// used as unchanged state.</summary>
        /// <remarks>This may result in removing the Undo history under some circumstances.
        /// </remarks>
        void ClearDirtyState();

        /// <summary>
        /// Sets username and password four silent authentication on protected sites.
        /// </summary>
        /// <remarks>
        /// If these parameters are set the control tries to authenticate using these
        /// values if a site requests authentication. This in fact suppresses the 
        /// typical authentication dialog. After changing the values it is necessary to
        /// call LoadHtml or LoadUrl again.
        /// </remarks>
        /// <param name="userName">The username the control should send on request.</param>
        /// <param name="passWord">The password the control should send on request.</param>
        void SetAuthentication(string userName, string passWord);

        /// <summary>
        /// User Agent string used to identify the control when browsing the web.
        /// </summary>
        string UserAgent
        {
            get;
            set;
        }

        /// <overloads>There are two overloads of this method.</overloads>
        /// <summary>
        /// This method resets the current credentials applied to the control.
        /// </summary>
        /// <remarks>
        /// After successfull authentication to a secure site the control will reuse the 
        /// credentials on any further access. To avoid this and to re-force the authentication,
        /// a call to this method is applicable. Calling this method will not reset the
        /// username and password set with <see cref="IHtmlEditor.SetAuthentication">SetAuthentication</see>.
        /// </remarks>
        void ClearAuthenticationCache();

        /// <summary>
        /// This method resets the current credentials applied to the control and remove the username and password.
        /// </summary>
        /// <remarks>
        /// After successfull authentication to a secure site the control will reuse the 
        /// credentials on any further access. To avoid this and to re-force the authentication,
        /// a call to this method is applicable. Calling this method will clear the
        /// username and password set with <see cref="IHtmlEditor.SetAuthentication">SetAuthentication</see>.
        /// Further calls to a protected site will always fail if no new username and password beeing set before.
        /// </remarks>
        void ClearAuthenticationCache(bool reset);

        /// <overloads>This method has two overloads.</overloads>
        /// <summary>
        /// Retrieves the HTML contained in control to a string and return it.
        /// </summary>
        /// <remarks>
        /// The content is unchanged "as is" the MSHTML control provides it. 
        /// <para>
        /// This method returns always the content of the base document. In case of a framed document this is the
        /// document containing the frameset definitions. The host application must use the HtmlFrameSet and 
        /// related objects to call the save methods there to save the content of each frame document separatly.
        /// </para>
        /// </remarks>
        /// <returns>The HTML in the control</returns>
        string GetRawHtml();

        /// <summary>
        /// Retrieves the HTML contained in control to a string and return it.
        /// </summary>
        /// <remarks>
        /// The content is unchanged "as is" the MSHTML control provides it. 
        /// <para>
        /// This method returns always the content of the base document. In case of a framed document this is the
        /// document containing the frameset definitions. The host application must use the HtmlFrameSet and 
        /// related objects to call the save methods there to save the content of each frame document separatly.
        /// </para>
        /// </remarks>
        /// <param name="ClearDirty">If set to true the call of this method will reset the <see cref="IsDirty">IsDirty</see> flag.</param>
        /// <returns>The HTML in the control</returns>
        string GetRawHtml(bool ClearDirty);

        /// <summary>
        /// Retrieves the HTML contained in control to a string and return it.
        /// </summary>
        /// <remarks>
        /// The content is unchanged "as is" the MSHTML control provides it. 
        /// <para>
        /// This method returns always the content of the base document. In case of a framed document this is the
        /// document containing the frameset definitions. The host application must use the HtmlFrameSet and 
        /// related objects to call the save methods there to save the content of each frame document separatly.
        /// </para>
        /// </remarks>
        /// <param name="ClearDirty">If set to true the call of this method will reset the <see cref="IsDirty">IsDirty</see> flag.</param>
        /// <param name="fromActiveFrame">If set to <c>true</c>, the currently active frame is the source, otherwise the frame definition document. This parameter is ignored, if the document has no frames.</param>
        /// <returns>The HTML in the control</returns>
        string GetRawHtml(bool ClearDirty, bool fromActiveFrame);


        /// <summary>
        /// Returns a well formatted representation of the current document.
        /// </summary>
        /// <remarks>Uses the default formatter, see <see cref="HtmlFormatterOptions"/></remarks>
        /// <exception cref="System.ArgumentException">Thrown if either the indent size or max line length is less than 0.</exception>
        /// <returns>Returns the formatted string.</returns>
        string GetFormattedHtml();

        /// <summary>
        /// Returns a well formatted representation of the current document.
        /// </summary>
        /// <param name="fo"><see cref="IHtmlFormatterOptions"/> for formatting this streams content.</param>
        /// <returns></returns>
        string GetFormattedHtml(IHtmlFormatterOptions fo);

        /// <summary>
        /// Returns a well formatted representation of the current document.
        /// </summary>
        /// <param name="fo"><see cref="IHtmlFormatterOptions"/> for formatting this streams content.</param>
        /// <returns></returns>
        /// <param name="ClearDirty">Resets the dirty flag or prevent this.</param>
        string GetFormattedHtml(IHtmlFormatterOptions fo, bool ClearDirty);
        
        /// <summary>
        /// Returns a well formatted representation of the current document.
        /// </summary>
        /// <param name="fo">See <see cref="IHtmlFormatterOptions"/> for formatting this streams content.</param>
        /// <param name="ClearDirty">If set to true the call of this method will reset the <see cref="IsDirty">IsDirty</see> flag.</param>
        /// <returns></returns>
        /// <param name="fromActiveFrame"></param>
        string GetFormattedHtml(IHtmlFormatterOptions fo, bool ClearDirty, bool fromActiveFrame);

        /// <summary>
        /// Saves the content as a file to the location the content was previously loaded from.
        /// </summary>
        /// <remarks>
        /// The method checks whether the file already exists and tries to create the file if it does not exist. The name of
        /// the <see cref="Url"/> method is used as a file name. If the path does not point to a filesystem with write 
        /// permissions an exception occurs.
        /// </remarks>
        void SaveFile();

        /// <summary>
        /// Save the current content into a file.
        /// </summary>
        /// <remarks>
        /// This method saves the "raw" content which consists of unformatted HTML. 
        /// This method requires Write access to the file. The file will be created if it doesn't exists.
        /// <para>
        /// The method checks whether the file already exists and tries to create the file if it does not exist. The name of
        /// the <see cref="Url"/> method is used as a file name. If the path does not point to a filesystem with write 
        /// permissions an exception occurs.
        /// </para>
        /// </remarks>
        /// <param name="fileName">The filename optionally including the full path.</param>
        void SaveFile(string fileName);

        /// <summary>
        /// Saves the current "raw" content into a stream. The stream is not closed and remains open.
        /// </summary>
        /// <param name="stream">The stream the content is written into.</param>
        void SaveFile(Stream stream);

        /// <summary>
        /// Saves the HTML contained in the control to a stream.
        /// </summary>
        /// <remarks>
        /// Throws an <exception cref="ArgumentNullException"/> if the parameter is <c>null</c>.
        /// Uses the global Encoding property for document encoding.
        /// </remarks>
        /// <param name="stream"></param>
        void SaveHtml(Stream stream);

        /// <summary>
        /// Saves the HTML contained in the control to a stream using the default formatter.
        /// </summary>
        /// <seealso cref="HtmlFormatterOptions"/>
        /// <param name="stream"></param>
        void SaveFormattedHtml(Stream stream);

        /// <summary>
        /// Saves the HTML contained in the control to a stream.
        /// </summary>
        /// <remarks>
        /// Throws an <exception cref="ArgumentNullException"/> if the parameter is <c>null</c>.
        /// Uses the global Encoding property for document encoding.
        /// Another method uses the <see cref="GetRawHtml()">GetRawHtml</see> or <see cref="GetFormattedHtml(IHtmlFormatterOptions)">GetFormattedHtml</see> 
        /// methods, which return content of the editor as string. It is up to the host application to use the common .NET file classes to save the content.
        /// The class <see cref="IHtmlFormatterOptions">HtmlFormatterOptions</see> provides a way to control the beautifier and formatter. The various properties are:
        /// <list type="bullet">
        ///     <item>IndentChar</item>
        ///     <item>IndentSize</item>
        ///     <item>MaxLineLength</item>
        ///     <item>AsXhtml</item>
        ///     <item>ElementCasing</item>
        ///     <item>AttributeCasing</item>
        /// </list>
        /// <para>
        /// Note: Both save methods throw an <see cref="System.ArgumentNullException">ArgumentNullException</see> if the stream parameter is null. If it is possible that the stream is null under some circumstances, we should put a try/catch block around the call.
        /// </para>
        /// <seealso cref="HtmlFormatterOptions">HtmlFormatterOptions</seealso>
        /// </remarks>       
        /// <exception cref="System.ArgumentNullException">Fired if stream parameter is <c>null</c>.</exception>
        /// <example>
        /// Assuming a menu exists and a menu item fires a click event, the following event handler will save the content of the base document to file in well formatted XHTML compatible format.
        /// <code>
        /// private void menuItem_Click(object sender, System.EventArgs e)
        /// {
        ///     // Build Formatting Options in variable fo here
        ///     if (this.saveFileDialog1.ShowDialog() == DialogResult.OK)
        ///     {
        ///         string fileName = this.saveFileDialog1.FileName;
        ///         FileStream fs = new FileStream(fileName, FileMode.Create);
        ///         this.htmlEditor1.SaveFormattedHtml(fs, fo);
        ///         fs.Close();
        ///     }
        /// }
        /// </code>
        /// </example> 
        /// <param name="stream"></param>
        /// <param name="fo"> for formatting this streams content.</param>
        void SaveFormattedHtml(Stream stream, IHtmlFormatterOptions fo);

        /// <overloads>This method has three overloads.</overloads>
        /// <summary>
        /// Saves the previously loaded content as MHTML stream.
        /// </summary>
        /// <remarks>
        /// If the file does not exist the method will create it. If the file exists the method will overwrite
        /// it. 
        /// <para>
        /// The main idea of the MHTML standard is that you send a HTML document, together with in-line graphics, 
        /// applets, etc., and also other linked documents if you so wish, in a MIME multipart/related body part.
        /// The content of the HTML part is coded in quoted printable format. The images and binary objects are
        /// coded in Base64, always chunked in segments to 76 characters per line.
        /// </para>
        /// <para>
        /// The IE can save and view MHTML. The preferred extension is MHT. NetRix works similiar to the basic
        /// MHTML save routine, but does not use the internal interfaces. 
        /// </para>
        /// <para>
        /// <b>Attention:</b> The method returns the content as it if was loaded the first time. ANY CHANGE DURING DESIGNTIME IS
        /// NOT RECOGNIZED. To save the current content the host application MUST save and reload the content AND
        /// wait for the next ReadyStateComplete event before this method can work properly.
        /// </para>
        /// <para>
        /// <b>Know  Issues:</b> This method is EXPERIMENTAL. It does not save frames properly and it does not handle
        /// local absolute paths correctly.
        /// </para>
        /// </remarks>
        /// <param name="fileName">The full path the content is saved into.</param>
        void SaveMht(string fileName);

        /// <summary>
        /// Saves the previously loaded content as MHTML stream.
        /// </summary>
        /// <remarks>
        /// If the file does not exist the method will create it. If the file exists the method will overwrite
        /// it. 
        /// <para>
        /// The main idea of the MHTML standard is that you send a HTML document, together with in-line graphics, 
        /// applets, etc., and also other linked documents if you so wish, in a MIME multipart/related body part.
        /// The content of the HTML part is coded in quoted printable format. The images and binary objects are
        /// coded in Base64, always chunked in segments to 76 characters per line.
        /// </para>
        /// <para>
        /// The IE can save and view MHTML. The preferred extension is MHT. NetRix works similiar to the basic
        /// MHTML save routine, but does not use the internal interfaces. 
        /// </para>
        /// <para>
        /// The method fails if the document is not ready yet.
        /// </para>
        /// <para>
        /// <b>Attention:</b> The method returns the content as it if was loaded the first time. ANY CHANGE DURING DESIGNTIME IS
        /// NOT RECOGNIZED. To save the current content the host application MUST save and reload the content AND
        /// wait for the next ReadyStateComplete event before this method can work properly.
        /// </para>
        /// <para>
        /// <b>Know  Issues:</b> This method is EXPERIMENTAL. It does not save frames properly and it does not handle
        /// local absolute paths correctly.
        /// </para>
        /// </remarks>
        /// <param name="fileStream">A stream (primarily a FileStream) to which the content is saved.</param>
        void SaveMht(Stream fileStream);

        /// <summary>
        /// Saves the previously loaded content as MHTML string.
        /// </summary>
        /// <remarks>
        /// Be aware that the string contains the page and all embedded resources as one block of coded data.
        /// This is approximatly 33% more than the sum of all file sizes (due to the Base64 coding). A normal
        /// page can produce a huge string and the processing may take a while.
        /// <para>
        /// The main idea of the MHTML standard is that you send a HTML document, together with in-line graphics, 
        /// applets, etc., and also other linked documents if you so wish, in a MIME multipart/related body part.
        /// The content of the HTML part is coded in quoted printable format. The images and binary objects are
        /// coded in Base64, always chunked in segments to 76 characters per line.
        /// </para>
        /// <para>
        /// The IE can save and view MHTML. The preferred extension is MHT. NetRix works similiar to the basic
        /// MHTML save routine, but does not use the internal interfaces. 
        /// </para>
        /// <para>
        /// <b>Attention:</b> The method returns the content as it if was loaded the first time. ANY CHANGE DURING DESIGNTIME IS
        /// NOT RECOGNIZED. To save the current content the host application MUST save and reload the content AND
        /// wait for the next ReadyStateComplete event before this method can work properly.
        /// </para>
        /// <para>
        /// <b>Know  Issues:</b> This method is EXPERIMENTAL. It does not save frames properly and it does not handle
        /// local absolute paths correctly.
        /// </para>
        /// </remarks>
        /// <returns>The string to which the content was written or <c>null</c>, if there is no content or the document is not ready.</returns>
        string SaveMht();

        /// <summary>
        /// Forces the file loader to prepare the content as MHT which could be save later.
        /// </summary>
        /// <remarks>
        /// </remarks>
        bool CanBuildMht { get; set; }

        /// <summary>
        /// Checks whether or not we have MHT loaded and available for save to string or file.
        /// </summary>
        bool CanSaveMht { get; }

        /// <summary>
        /// Add a external designer to the designer chain. 
        /// </summary>
        /// <remarks>
        /// This is done by the various PlugIns the control accepts.
        /// </remarks>
        /// <param name="designer">The object being attached must implement IHTMLEditDesigner directly or indirectly.</param>
        void AddEditDesigner(object designer);

		/// <summary>
		/// Returns the DOM (document object model) of the current loaded document.
		/// </summary>
		/// <remarks>
		/// The property may return <c>null</c> (<c>Nothing</c> in Visual Basic) if the document is 
		/// either not ready or doesn't contain a HTML element.
		/// <para>
		/// After first call the reference to the object is being cached, so a direct and extensive access
		/// using this property does not cause a performance flaw. Any LoadXXX method will reset cache
		/// and the first subsequent call will recreate the new document structure. Changes to the
		/// document are reflected synchrounously.
		/// </para>
		/// <seealso cref="IElementDom">ElementDom interface</seealso>.
		/// </remarks>
		IElementDom DocumentDom { get; }

        /// <overloads/>
        /// <summary>
        /// Returns the current document as <see cref="System.Xml.XmlDocument"/> formatted object.
        /// </summary>
        /// <remarks>
        /// This method should be
        /// covered by a try/catch structure because it rethrows internal exceptions occuring during the parse process.
        /// </remarks>
        /// <returns></returns>
        XmlDocument GetXmlDocument();

        /// <overloads/>
        /// <summary>
        /// Returns the current document as <see cref="System.Xml.XmlDocument"/> formatted object.
        /// </summary>
        /// <remarks>
        /// This method should be
        /// covered by a try/catch structure because it rethrows internal exceptions occuring during the parse process.
        /// </remarks>
        /// <param name="AddXmlDeclaration">
        /// If <c>true</c> the module tries to add the &lt;?xml version="1.0" ?&gt; declaration in front of the document.
        /// </param>
        /// <returns></returns>
        XmlDocument GetXmlDocument(bool AddXmlDeclaration);

        /// <summary>
        /// Returns the current document as <see cref="System.Xml.XmlDocument"/> formatted object.
        /// </summary>
        /// <remarks>
        /// This method should be
        /// covered by a try/catch structure because it rethrows internal exceptions occuring during the parse process.
        /// </remarks>
        /// <param name="AddXmlDeclaration">
        /// If <c>true</c> the module tries to add the &lt;?xml version="1.0" ?&gt; declaration in front of the document.
        /// </param>
        /// <param name="fromActiveFrame"><c>True</c> if content is read from active frame, otherwise the content of the frame definition file is read.</param>
        /// <returns></returns>
        XmlDocument GetXmlDocument(bool AddXmlDeclaration, bool fromActiveFrame);
        
        /// <summary>
        /// This method replaces the content of the whole designer.
        /// </summary>
        /// <remarks>
        /// The relation to the previously loaded URL or filename is left untouched. The content is also
        /// beeing rewritten to the file and reloaded from that location. This assures that the paths to 
        /// relative embedded objects are still valid.
        /// <seealso cref="ReLoadHtml">ReLoadHtml</seealso>
        /// </remarks>
        /// <param name="content">Content which replaces the current content</param>
        void ReLoadHtml(string content);

        /// <overloads>This method has to overloads.</overloads>
        /// <summary>
        /// This method replaces the content of the whole designer.
        /// </summary>
        /// <remarks>
        /// This is the preferred methos to refresh the
        /// content. If the content was externally changed, it is necessary to save the content before the reload
        /// takes place. If saving is not done, the old content will replace all changes.
        /// <para>
        /// It is up to the host application to decide where to save the content. If the parameter <c>saveContent</c>
        /// is set to <c>true</c>, the component will save the content internally and overwrite the original file without
        /// further questions.
        /// </para>
        /// </remarks>
        /// <exception cref="System.IO.FileNotFoundException">Thrown if the path is not valid or not a file.</exception>
        /// <param name="content">Content which replaces the current content</param>
        /// <param name="saveContent">If <c>true</c>, the component will save the the current content to 
        /// file and reload from file then. If <c>false</c> the component will load the last not saved content from 
        /// the related URL. This means, that any unsaved content will be lost. There is no UNDO on this action available.</param>
        void ReLoadUrl(string content, bool saveContent);

        /// <summary>
        /// This method simply refreshes the content to rebuild all relations, like linked stylesheets.
        /// </summary>
        /// <remarks>
        /// A call to this method can force the save operation on request. 
        /// </remarks>
        void ReLoadUrl(bool SaveContent);

        /// <summary>
        /// Loads HTML content from a URL or local path.
        /// </summary>
        /// <remarks>
        /// This is the preferred load method. It can take HTML content from the following sources:
        /// <list type="bullet">
        ///     <item>A locally stored file</item>
        ///     <item>An URL, with the leading "http://" (required)</item>
        /// </list>
        /// The method will fire the <see cref="Loading">Loading</see> before
        /// the load process starts, but not when the URL or filename was not accepted. This can be used to check
        /// the processing, e.g. if the event does not fire the URL is wrong or the filename was not found. 
        /// If the processing is done the <see cref="Loaded">Loaded</see> event
        /// will fired. Remember that the next editing command MUST wait until the
        /// <see cref="ReadyStateComplete">ReadyStateComplete</see> was fired, which
        /// takes a bit more time used by the control to render and display of the content.
        /// <para>
        /// Use <see cref="LoadHtml(string)">LoadHtml</see> to load memory based strings only.
        /// </para>
        /// <para>
        /// If MSHTML has not yet been created, the loading is postponed until MSHTML has been created.
        /// The method uses internally IMoniker to set the current base path to resolve relative paths
        /// in src and href attributes without changing the parameters.
        /// </para>
        /// </remarks>
        /// <example>
        /// The following example shows how to load a file into NetRix using a menu entry event handler. The
        /// code assumes that a main menu extists with a entry <c>menuItem</c> in it and a event handler
        /// <c>menuItem_Click</c> attached to the Click event.
        /// <code>
        /// private void menuItem_Click(object sender, System.EventArgs e)
        /// {
        ///    if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
        ///    {
        ///       string fileName = this.openFileDialog1.FileName;
        ///       if (File.Exists(fileName))
        ///       {
        ///          this.htmlEditor1.LoadUrl(this.openFileDialog1.FileName);
        ///       }
        ///    }
        /// }
        /// </code>
        /// If is recommended to check whether the file exists. However, the method will not throw an exception
        /// but ignores the command, which makes it impossible to inform the user that the action fails. 
        /// <para>
        /// From build 1015 on the control accepts SSL protected connections too.
        /// </para>
        /// </example>
        /// <param name="url">URL (http://any.domain.com/file.html) or file (C:\whatever\path\file.html) to load from.</param>
        void LoadUrl(string url);

        /// <summary>
        /// Loads a local file into the control.
        /// </summary>
        /// <remarks>
        /// This method is simply a wrapper for <see cref="LoadUrl"/>. It checks the existence of the file on the local system
        /// and loads it if possible. 
        /// </remarks>
        /// <exception cref="FileNotFoundException">Fires this exception if the file was not found.</exception>
        /// <param name="fileName"></param>
        void LoadFile(string fileName);

        /// <summary>
        /// Load a DOCX document into the editor. EXPERIMENTAL.
        /// </summary>
        /// <remarks>
        /// The document is read, unpacked, transformed and loaded as string into HTML. The control exposes and writes content back as HTML.
        /// <para>
        /// Some features are not properly supported. EXPERIMENTAL.
        /// </para>
        /// </remarks>
        /// <param name="filePath"></param>
        void LoadDocx(string filePath);

        /// <overloads>This method has three overloads.</overloads>
        /// <summary>
        /// Loads HTML content from a stream into this control.
        /// </summary>
        /// <remarks>
        /// It is recommended to use a <see cref="System.IO.FileStream">FileStream</see> for that procedure, because
        /// internally the name is extracted from the file name of the underlying stream. Otherwise any stream is
        /// possible which can be used with the <see cref="System.IO.StreamReader">StreamReader</see> class.
        /// The method will fire the <see cref="Loading">Loading</see> before
        /// the load process starts, but not when the URL or filename was not accepted. This can be used to check
        /// the processing, e.g. if the event does not fire the URL is wrong or the filename was not found. 
        /// If the processing is done the <see cref="Loaded">Loaded</see> event
        /// will fired. Remember that the next editing command MUST wait until the
        /// <see cref="ReadyStateComplete">ReadyStateComplete</see> was fired, which
        /// takes a bit more time used by the control to render and display of the content.
        /// <para>
        /// If MSHTML has not yet been created, the loading is postponed until MSHTML has been created.
        /// The method uses internally IMoniker to set the current base path to resolve relative paths
        /// in src and href attributes without changing the parameters.
        /// </para>
        /// </remarks>
        /// <param name="stream"></param>
        void LoadHtml(FileStream stream);

        /// <summary>
        /// Loads HTML content from a string into this control.
        /// </summary>
        /// <remarks>
        /// If previously used stream or file load process was used the existing URL (e.g. the filename) remains.
        /// If in a previous operation was no name set the default name "localhost" was set. 
        /// The method will fire the <see cref="Loaded">Loaded</see> before
        /// the load process starts, but not when the URL or filename was not accepted. This can be used to check
        /// the processing, e.g. if the event does not fire the URL is wrong or the filename was not found. 
        /// If the processing is done the <see cref="Loading">Loading</see> event
        /// will fired. Remember that the next editing command MUST wait until the
        /// <see cref="ReadyStateComplete">ReadyStateComplete</see> was fired, which
        /// takes a bit more time used by the control to render and display of the content.
        /// <para>
        /// If MSHTML has not yet been created, the loading is postponed until MSHTML has been created.
        /// The method uses internally IMoniker to set the current base path to resolve relative paths
        /// in src and href attributes without changing the parameters.
        /// </para>
        /// <para>This method will set the control into non file based mode, as there is no filename provided.</para>
        /// </remarks>
        /// <param name="content"></param>
        void LoadHtml(string content);

        /// <summary>
        /// Determines that the document is loaded from and saved into a file.
        /// </summary>
        /// <remarks>
        /// This property is set automatically if the document is loaded from an URL or file
        /// using the <see cref="LoadUrl"/> methods. If the <see cref="LoadHtml(string)"/> methods are
        /// used, the host application can determine how the load/save process works. To
        /// work with files the property is set to <c>true</c>. This is the default value.
        /// To work without any file relation, the property should set to <c>false</c>, before
        /// issuing a <see cref="LoadHtml(string)"/> method to load content.
        /// </remarks>
        bool IsFileBasedDocument
        { get;
            set;
        }

        /// <summary>
        /// This property changes the content of the body. 
        /// </summary>
        /// <remarks>Its purpose is to support simple databindings.</remarks>
        /// <seealso cref="OuterHtml"/>
        [Browsable(false), DefaultValue("")]
        string InnerHtml { get; set; }

        /// <summary>
        /// This property returns the document's content in raw (unformatted) format.
        /// </summary>
        /// <seealso cref="GetRawHtml()"/>
        /// <seealso cref="InnerHtml"/>
        [Browsable(false), DefaultValue("")]
        string OuterHtml
        {
            get;
        }


        /// <summary>
        /// Creates a new document.
        /// </summary>
        /// <remarks>
        /// Used to reset the control into a neutral state.
        /// </remarks>
        void NewDocument();

        /// <summary>
        /// Insert Text at Caret position.
        /// </summary>
        /// <remarks>
        /// This method will always insert any characters als Text, tag definitions appear
        /// as converted text, e.g. &lt;span&gt; is inserted as &amp;lt;span&amp;gt;.
        /// </remarks>
        /// <param name="text">Text that has to be inserted.</param>
        void InsertTextAtCaret(string text);

        /// <summary>
        /// Inserts a generic created element at the current caret position.
        /// </summary>
        /// <remarks>
        /// This method internally calls <see cref="CreateElement"/> to add the element to the documents DOM.
        /// After that, the element is inserted. The method may fail if the element cannot be inserted at caret position.
        /// The control will not accept invalid HTML. In case of error an internal exception is thrown and catched. The
        /// method will return <c>null</c> (<c>Nothing</c> in VB.NET) if no success.
        /// </remarks>
        /// <param name="tagName">The name of tag to be inserted. Use "span" to insert &lt;span&gt;, for instance.</param>
        /// <returns>Returns <c>True</c> on success, <c>False</c> otherwise.</returns>
        IElement CreateElementAtCaret(string tagName);

        /// <summary>
        /// Insert the element at the current caret position.
        /// </summary>
        /// <remarks>
        /// See implementation for details.
        /// <seealso cref="CreateElementAtCaret">CreateElementAtCaret</seealso>
        /// </remarks>
        /// <param name="element">The element which has to be inserted, returns the new element.</param>
        /// <returns>Returns <c>True</c> on success or <c>False</c> if the insertion fails.</returns>
        bool InsertElementAtCaret(IElement element);

        /// <summary>
        /// Gets or sets the proxy used to support download beyond a proxy.
        /// </summary>
        /// <remarks>
        /// Set this property to <c>null</c> if no proxy is required. If the internal loader is being used the 
        /// proxy settings of IE or operating system are bypassed. Restrictive security settings may make 
        /// the operation fail, though.
        /// </remarks>
        WebProxy Proxy { get; set; }

        /// <summary>
        /// Fired if an exception occured during loading resources from web.
        /// </summary>
        event WebExceptionEventHandler WebException;


        # endregion

        # region +++++ Block: UNDO

        /// <summary>
        /// Undo the last action.
        /// </summary>
        /// <remarks>
        /// The command does nothing if the <see cref="CanUndo"/> property returns false.
        /// </remarks>
        void Undo();

        /// <summary>
        /// Redo the last operation or - in case of a batched operation - the whole batch.
        /// </summary>
        /// <remarks>
        /// The command does nothing if the <see cref="CanRedo"/> property returns false.
        /// </remarks>
        void Redo();

        /// <summary>
        /// Notifies that a new unit has been added to the current manager.
        /// </summary>
        /// <remarks>
        /// The object returned a sender is the current <see cref="IUndoStack">IUndoStack</see> object. One can cast the parameter to 
        /// <see cref="IUndoStack">IUndoStack</see> and use the various methods and properties
        /// to determine what happens during the last operation.
        /// </remarks>
        event EventHandler<UndoEventArgs> NextOperationAdded;

        /// <summary>
        /// Fired if ready state switches to complete after loading/reloading a document.
        /// </summary>
        event EventHandler ReadyStateComplete;

        /// <summary>
        /// Fired if the control changes the state of the document.
        /// </summary>
        /// <remarks>
        /// It is recommended to use the <see cref="ReadyStateComplete"/>
        /// event instead of checking ReadyStateChanged for the complete state.
        /// </remarks>
        event ReadyStateChangedHandler ReadyStateChanged;

        /// <summary>
        /// Fired if the control becomes Dirty the first time.
        /// </summary>
        /// <remarks>
        /// The event is fired if the Dirty flag changes from "not dirty" to "dirty". If the
        /// dirty flag is reset after this and becomes dirty later the event is fired again.
        /// </remarks>
        event ContentModifiedHandler ContentModified;

        /// <summary>
        /// Fired if the document changes by user interaction or programmatic access to properties.
        /// </summary>
        event EventHandler ContentChanged;

        ///// <summary>
        ///// Informs the host that the undo list has been changed.
        ///// </summary>
        //event EventHandler UndoListChanged;

        /// <summary>
        /// This event informs the host that the UI (toolbar, menu) is supposed to re-render.
        /// </summary>
        event UpdateUIHandler UpdateUI;

        /// <summary>
        /// Fired before a shortcut is processed internally.
        /// </summary>
        /// <remarks>
        /// The purpose of this event is to handle shortcuts elsewhere and disable the internal handling
        /// specifically for some keys. 
        /// </remarks>
        event BeforeShortcutEventHandler BeforeShortcut;

        /// <summary>
        /// Fired is PropertyGrid firstly requests the property (attribute) descriptions.
        /// </summary>
        /// <remarks>
        /// This event can be used to filter the list of properties in the Grid form each HTML object
        /// to reduce the number of attributes the user can edit. This is an "per object" request which
        /// is cached internally.
        /// </remarks>
        event PropertyFilterHandler PropertyFilterRequest;

		/// <summary>
		/// Fired after the complete property description for an element is ready and the PropertyGrid is up to invoke.
		/// </summary>
		/// <remarks>
		/// The purpose of this event is to change the content and behavior of any property contained in
		/// the PropertyDescriptorCollection, as well as adding properties dynamically to build a property bag.
		/// </remarks>
		event PropertyDisplayHandler PropertyDisplayRequest;

        /// <summary>
        /// Latest event after all internal processing has taken place.
        /// </summary>
        event PostEditorEventHandler PostEditorEvent;

        /// <summary>
        /// Fired if user tries to show a contextmenu with right click.
        /// </summary>
        /// <remarks>
        /// Host app should provide a contextmenu shown after receiving the event (right mouse click). An alternative
        /// way is the usage of the <see cref="ContextMenu">ContextMenu</see> property.
        /// You must assure that not both option used in the same application.
        /// </remarks>
        /// <example>
        /// The event is used in that way:
        /// <code>
        /// this.htmlEditor1.ShowContextMenu += new GuruComponents.NetrixEvents.ShowContextMenuEventHandler(htmlEditor1_ShowContextMenu);
        /// </code>
        /// The event handler is responsible for the context menus themselves:
        /// <code>
        /// private void htmlEditor1_ShowContextMenu(object sender, GuruComponents.NetrixEvents.ShowContextMenuEventArgs e)
        /// {
        ///     this.contextMenu1.Show(this.htmlEditor1, e.Location);
        ///     EditTagDialog.Location = this.PointToScreen(e.Location);
        /// }
        /// </code>
        /// The field <c>contextMenu1</c> is a .NET context menu, which can define or configure on the fly if necessary. 
        /// </example>
        event ShowContextMenuEventHandler ShowContextMenu;

        /// <summary>
        /// Fired if any mouse operation happens. Returns element information and coordinates in event args
        /// </summary>
        event HtmlMouseEventHandler HtmlMouseOperation;

        /// <summary>
        /// Fired if any key operation happens.
        /// </summary>
        /// <remarks>
        /// Returns element information and pressed key status
        /// </remarks>
        event HtmlKeyEventHandler HtmlKeyOperation;

        /// <summary>
        /// Fired if a key or mouse operation has changed the current element.
        /// </summary>
        /// <remarks>
        /// Subsequent clicks or keystrokes within
        /// the same element does not fire the event again. 
        /// </remarks>
        event HtmlElementChangedHandler HtmlElementChanged;


        /// <summary>
        /// Gets a new, opened undo manager. 
        /// </summary>
        /// <remarks>
        /// Until the <see cref="IUndoStack.Close">Close</see> method is called all
        /// following operations become part of one single undo step.
        /// </remarks>
        /// <param name="Name">Provide a specific name to distinguish this manager instance from other parallel opened ones.</param>
        /// <returns>The undo stack, that contains all current undo steps the undo manager currently helds.</returns>
        IUndoStack GetUndoManager(string Name);

        /// <summary>
        /// Gets the default internal undo manager.
        /// </summary>
        /// <remarks>This is the same call as <see cref="GetUndoManager(string)"/>.</remarks>
        /// <returns>The undo stack, that contains all current undo steps the undo manager currently helds.</returns>
        IUndoStack GetUndoManager();

        /// <summary>
        /// Gets a new, opened redo manager. 
        /// </summary>
        /// <remarks>
        /// Until the <see cref="IUndoStack.Close">Close</see> method is called all
        /// following operations become part of one single redo step.
        /// <para>
        /// The host application should assure that only operations become part of the redo stack which can be 
        /// reliable "redone" in any situation. It is not recommended to use this in conjunction with table operations.
        /// </para>
        /// </remarks>
        /// <param name="Name">Provide a specific name to distinguish this manager instance from other parallel opened ones.</param>
        /// <returns>Returns an object which provides the method and properties to control the redo manager.</returns>
        IUndoStack GetRedoManager(string Name);       

        # endregion

        /// <summary>
        /// Provides access for sited components.
        /// </summary>
        /// <remarks>
        /// See implementing class for further usage explanation.
        /// </remarks>
        /// <seealso cref="IDesignerHost"/>
        /// <seealso cref="IDesignSite"/>
        /// <seealso cref="IMenuCommandService"/>
        /// <seealso cref="IUIService"/>
        /// <seealso cref="HtmlElementChangedEventArgs"/>
        /// <seealso cref="HtmlElementChanged"/>
        IDesignSite HtmlEditorSite
        {
            get;
            set;
        }

        /// <summary>
        /// Set the editor window zoom level temporarily to given value.
        /// </summary>
        /// <remarks>
        /// This setting does not persist. Reloading document removes zoom level and sets document to 100%.
        /// </remarks>
        /// <seealso cref="IDocument.Zoom"/>
        /// <seealso cref="GetZoom"/>
        /// <param name="ratio">Value for zoom, 1 equals 100%. Set 0.5 for 50% or 2.0 for 200%.</param>
        void Zoom(decimal ratio);     

        /// <summary>
        /// Returns the current temporariry Zoom level.
        /// </summary>
        /// <seealso cref="IDocument.Zoom"/>
        /// <seealso cref="Zoom"/>
        /// <returns>Zoom value, return 1 for 100%, 0.5 for 50% or 2.0 for 200%.</returns>
        decimal GetZoom();

    }
}