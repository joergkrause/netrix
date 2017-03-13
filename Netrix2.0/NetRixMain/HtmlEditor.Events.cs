using System;
using System.ComponentModel;
using System.Windows.Forms;
using GuruComponents.Netrix.Events;
using GuruComponents.Netrix.WebEditing.UndoRedo;

namespace GuruComponents.Netrix
{

    public partial class HtmlEditor
    {

        # region public events

        /// <summary>
        /// Fired if the user clicked any of the toolstrip or menustrip tools.
        /// </summary>
        [Category("NetRix UI Events"), Description("Fired if the user clicked any of the toolstrip or menustrip tools.")]
        public event ToolClickedCancelEventHandler ToolItemClicked;
        /// <summary>
        /// Fired before the control navigates to a new hyperlink.
        /// </summary>
        [Category("NetRix Events"), Description("Fired before the control navigates to a new hyperlink.")]
        public event BeforeNavigateEventHandler BeforeNavigate;
        /// <summary>
        /// Fired if mouse leaves the control during drag operation.
        /// </summary>
        [Category("NetRix Events"), Description("Fired if mouse leaves the control during drag operation.")]
        public new event EventHandler DragLeave;
        /// <summary>
        /// The event is fired after a drop operation.
        /// </summary>
        [Category("NetRix Events"), Description("The event is fired after a drop operation.")]
        public new event DragEventHandler DragDrop;
        /// <summary>
        /// This event is fired during the drag operation.
        /// </summary>
        /// <remarks>
        /// This is fired multiple times based
        /// on the mouse move event. Handler methods should not perform time consuming tasks.
        /// </remarks>
        [Category("NetRix Events"), Description("This event is fired during the drag operation.")]
        public new event DragEventHandler DragOver;
        /// <summary>
        /// This is fired on drag enter.
        /// </summary>
        /// <remarks>
        /// This should normally fire when the mouse enters the editors surface
        /// with left mouse button down. The mouse cursor will change before(!) the event is fired
        /// depending on the attached drag data.
        /// </remarks>
        [Category("NetRix Events"), Description("This is fired on drag enter, normally when the mouse enters the editors surface with left mouse button down.")]
        public new event DragEventHandler DragEnter;
        /// <summary>
        /// Fired before a shortcut is processed internally.
        /// </summary>
        /// <remarks>
        /// The purpose of this event is to handle shortcuts elsewhere and disable the internal handling
        /// specifically for some keys. 
        /// </remarks>
        [Category("NetRix Events"), Description("Fired before the control navigates to a new hyperlink.")]
        public event BeforeShortcutEventHandler BeforeShortcut;

        /// <summary>
        /// Notifies that a new unit has been added to the current manager.
        /// </summary>
        /// <remarks>
        /// The event argument's sender parameter is the current <see cref="IUndoStack">IUndoStack</see> object. One can cast the parameter to 
        /// <see cref="IUndoStack">IUndoStack</see> and use the various methods and properties
        /// to determine what happens during the last operation.
        /// </remarks>
        [Category("NetRix Events"), Description("Notifies that a new unit has been added to the current manager.")]
        public event EventHandler<UndoEventArgs> NextOperationAdded;

        /////// <summary>
        /////// Will fire after the undo list has been changed.
        /////// </summary>
        //[Category("NetRix Events"), Description("Will fire after the undo list has been changed.")]
        //public event EventHandler UndoListChanged;

        /// <summary>
        /// Fired before a new document will loaded from stream or file.
        /// </summary>
        [Category("NetRix Events"), Description("Fired before a new document will loaded from stream.")]
        public event LoadEventHandler Loading;
        /// <summary>
        /// Fired after the new document is succesfully loaded from stream or file.
        /// </summary>
        /// <remarks>
        /// The event is fired immediataley after the base stream is loaded and and the protocol handler 
        /// is successfully attached. Loading a document is a asynchronous process. If the document contains
        /// several object (like images) they are loaded after the base document is ready. This may take a
        /// while. The final document cannot be used before the ReadyStatecomplete event is fired.
        /// </remarks>
        [Category("NetRix Events"), Description("Fired after the new document is succesfully loaded from stream.")]
        public event LoadEventHandler Loaded;
        /// <summary>
        /// Fired before the document will save into stream.
        /// </summary>
        [Category("NetRix Events"), Description("Fired before the document will save into stream.")]
        public event SaveEventHandler Saving;
        /// <summary>
        /// Fired after the document is succesfully saved into stream.
        /// </summary>
        [Category("NetRix Events"), Description("Fired after the document is successfully saved into stream.")]
        public event SaveEventHandler Saved;
        /// <summary>
        /// Fired if a new document was successfully created.
        /// </summary>
        [Category("NetRix Events"), Description("Fired if a new document was successfully created.")]
        public event CreatedEventHandler DocumentCreated;
        /// <summary>
        /// Fired if a new document was successfully created.
        /// </summary>
        [Category("NetRix Events"), Description("Fired if a new element was successfully created.")]
        public event CreatedEventHandler ElementCreated;

        /// <summary>
        /// Notifies the host that the asynchronous loader is about to load a new resource (image, css file, ...).
        /// </summary>
        /// <remarks>
        /// The host may stop loading or change the URL to any other local or remote resource which should be loaded
        /// instead. The event arguments contain information about MIME type and file name.
        /// </remarks>
        [Category("NetRix Events"), Description("Notifies that a new unit has been added to the current manager.")]
        public event BeforeResourceLoadEventHandler BeforeResourceLoad;

        /// <summary>
        /// Fired if a find call reached the end of the document.
        /// </summary>
        /// <remarks>
        /// It doesn't matter whether the previous find, which causes the event to be fired, has successfully found
        /// any text. Generally, ignoring the event will wrap the find process and start at the beginning of the document
        /// again. Alternatively the Parameter <c>stopAtEnd</c> of second overload of the <see cref="Find(string,bool,bool,bool)"/> method
        /// can be used to stop. The event will always fire, even if the process stops automatically then.
        /// </remarks>
        [Category("NetRix Events"), Description("Fired if a find call reached the end of the document.")]
        public event EventHandler FindHasReachedEnd;

        /// <summary>
        /// Fired after the replace routine has found a word.
        /// </summary>
        /// <remarks>
        /// The Cancel property allows the host application to stop replacing interactively.
        /// </remarks>
        [Category("NetRix Events"), Description("Fired if a find call reached the end of the document.")]
        public event BeforeReplaceEventHandler BeforeReplace;

        /// <summary>
        /// Fired when PropertyGrid first requests the property (attribute) descriptions.
        /// </summary>
        /// <remarks>
        /// This event can be used to filter the list of properties in the Grid form each HTML object
        /// to reduce the number of attributes the user can edit. This is an "per object" request which
        /// is cached internally.
        /// </remarks>
        /// <seealso cref="PropertyDisplayRequest"/>
        /// <seealso cref="EventFilterRequest"/>
        [Category("NetRix Events")]
        public event PropertyFilterHandler PropertyFilterRequest;

        /// <summary>
        /// Fired after the complete property description for an element is ready and the PropertyGrid is up to invoke.
        /// </summary>
        /// <remarks>
        /// The purpose of this event is to change the content and behavior of any property contained in
        /// the PropertyDescriptorCollection, as well as adding properties dynamically to build a property bag.
        /// </remarks>
        /// <seealso cref="PropertyFilterRequest"/>
        /// <seealso cref="EventDisplayRequest"/>
        [Category("NetRix Events")]
        public event PropertyDisplayHandler PropertyDisplayRequest;

        /// <summary>
        /// Fired when PropertyGrid first requests the event (attribute) descriptions.
        /// </summary>
        /// <remarks>
        /// This event can be used to filter the list of properties in the Grid form each HTML object
        /// to reduce the number of attributes the user can edit. This is an "per object" request which
        /// is cached internally.
        /// </remarks>
        /// <seealso cref="PropertyFilterRequest"/>
        /// <seealso cref="EventDisplayRequest"/>
        [Category("NetRix Events")]
        public event EventFilterHandler EventFilterRequest;

        /// <summary>
        /// Fired after the complete event description for an element is ready and the PropertyGrid is up to invoke.
        /// </summary>
        /// <remarks>
        /// The purpose of this event is to change the content and behavior of any property contained in
        /// the EventDescriptorCollection, as well as adding events dynamically to build a property bag.
        /// </remarks>
        /// <seealso cref="PropertyDisplayRequest"/>
        /// <seealso cref="EventFilterRequest"/>
        [Category("NetRix Events")]
        public event EventDisplayHandler EventDisplayRequest;

        /// <summary>
        /// Fired if the host application should update their UI (toolbars, enable/disable menu items).
        /// </summary>
        /// <remarks>
        /// The event is fired on all actions that the internal processing or the user might invoke. 
        /// This allows the host application to set their toolbars or menu items according to the current caret position or mouse move. 
        /// See method <see cref="GetCommandInfo"/> in conjunction 
        /// with <see cref="GuruComponents.Netrix.WebEditing.Elements.Element.EffectiveStyle">EffectiveStyle</see> is the best way 
        /// to retrieve the state at the current caret or mouse pointer position.
        /// <para>
        /// The element delivered by the event argument (<see cref="UpdateUIEventArgs"/>) is the one the has caused the last event.
        /// If, for example, the mouse is moving over an H1 element, than this element is the one delivered by the UpdateUI event,
        /// which is fired when the mouse enters the element's borders. The reason is, that you have now the opportunity to update
        /// your UI according to the actions that might be available for this very element. The <see cref="CurrentScopeElement"/>
        /// property returns the element following the insertion point (shown by the caret) and treated as the editor's current 'scope'.
        /// This is not neccesseraly the same as the one delivered by <see cref="UpdateUI"/> event.
        /// </para>
        /// </remarks>
        [Category("NetRix Events"), Description("Fired if the host application should update their UI.")]
        public event UpdateUIHandler UpdateUI; 

        /// <summary>
        /// Fired if an exception occured during loading resources from web.
        /// </summary>
        [Category("NetRix Events"), Description("Fired if an exception occured during loading resources from web.")]
        public event WebExceptionEventHandler WebException;

        /// <summary>
        /// Fired before the snap rect method is applied during resize and move. Cancallable.
        /// </summary>
        [Category("NetRix Events"), Description("Fired before the snap rect method is applied during resize and move. Cancallable.")]
        public event BeforeSnapRectEventHandler BeforeSnapRect;

        /// <summary>
        /// Fired after the snap rect method is applied during resize and move.
        /// </summary>
        [Category("NetRix Events"), Description("Fired after the snap rect method is applied during resize and move.")]
        public event AfterSnapRectEventHandler AfterSnapRect;


        /// <summary>
        /// Latest event after all internal processing has taken place.
        /// </summary>
        [Category("NetRix Events"), Description("Latest event after all internal processing has taken place.")]
        public event PostEditorEventHandler PostEditorEvent;

        /// <summary>
        /// Fired if the control becomes Dirty the first time.
        /// </summary>
        /// <remarks>
        /// The event is fired if the Dirty flag changes from "not dirty" to "dirty". If the
        /// dirty flag is reset after this and becomes dirty later the event is fired again.
        /// </remarks>
        [Browsable(true), Category("NetRix Events"), Description("Fired if the control becomes Dirty the first time.")]
        public event ContentModifiedHandler ContentModified;

        /// <summary>
        /// Fired if the document changes by user interaction or programmatic access to properties.
        /// </summary>
        [Browsable(true), Category("NetRix Events"), Description("Fired if the document changes by user interaction or programmatic access to properties.")]
        public event EventHandler ContentChanged;

        /// <summary>
        /// Fired if user tries to show a contextmenu with right click.
        /// </summary>
        /// <remarks>
        /// Host app should provide a contextmenu shown after receiving the event (right mouse click). An alternative
        /// way is the usage of the <see cref="ContextMenu">ContextMenu</see> 
        /// as well as <see cref="GuruComponents.Netrix.HtmlEditor.ContextMenuStrip">ContextMenuStrip</see> property.
        /// You must assure that not both option used in the same application.
        /// <para>
        /// <see cref="GuruComponents.Netrix.HtmlEditor.ContextMenuStrip">ContextMenuStrip</see> is not available for
        /// version compiled exclusively for .NET 1.1 or prior.
        /// </para>
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
        [Category("NetRix Events")]
        public event ShowContextMenuEventHandler ShowContextMenu;

        /// <summary>
        /// Fired if any mouse operation happens. 
        /// </summary>
        /// <remarks>Returns element information and coordinates in event args.</remarks>
        /// <seealso cref="HtmlKeyOperation"/>
        [Category("NetRix Events")]
        public event HtmlMouseEventHandler HtmlMouseOperation;

        /// <summary>
        /// Fired if any key operation happens.
        /// </summary>
        /// <remarks>
        /// Returns element information and pressed key status.
        /// </remarks>
        /// <seealso cref="HtmlMouseOperation"/>
        [Category("NetRix Events")]
        public event HtmlKeyEventHandler HtmlKeyOperation;

        /// <summary>
        /// Fired if a key or mouse operation has changed the current element.
        /// </summary>
        /// <remarks>
        /// Subsequent clicks or keystrokes within the same element does not fire the event again. 
        /// See <see cref="HtmlEditorSite"/> for an usage scenario.
        /// </remarks>
        /// <seealso cref="HtmlEditorSite"/>
        /// <seealso cref="HtmlKeyOperation"/>
        /// <seealso cref="HtmlMouseOperation"/>
        [Category("NetRix Events")]
        public event HtmlElementChangedHandler HtmlElementChanged;


        /// <summary>
        /// Fired if a frame becomes active in a framed document.
        /// </summary>
        /// <remarks>
        /// This event is only available if the current document has frames and is in design mode.
        /// </remarks>
        [Browsable(true), Category("NetRix Events"), Description("Fired if an frame becomes active in a framed document.")]
        public event FrameActivatedEventHandler FrameActivated;

        /// <summary>
        /// Fired if ready state switches to complete after loading/reloading a document.
        /// </summary>
        [Category("NetRix Events"), Description("Fired if ready state switches to complete after loading/reloading a document")]
        public event EventHandler ReadyStateComplete;

        /// <summary>
        /// Fired if the control changes the state of the document.
        /// </summary>
        /// <remarks>
        /// It is recommended to use the <see cref="ReadyStateComplete"/>
        /// event instead of checking ReadyStateChanged for the complete state.
        /// </remarks>
        [Category("NetRix Events")]
        [Description("Fired if the control changes the state of the document.")]
        public event ReadyStateChangedHandler ReadyStateChanged;

        # endregion public events
    }
}