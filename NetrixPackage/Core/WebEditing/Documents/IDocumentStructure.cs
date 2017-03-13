using System;
using System.IO;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Text;
using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.WebEditing.Elements;
using GuruComponents.Netrix.Events;

namespace GuruComponents.Netrix.WebEditing.Documents
{
    /// <summary>
    /// This interface represents a HTML document and its various settings.
    /// </summary> 
    /// <remarks>
    /// Especially it contains information from the header block, like title, meta tags and linked stylesheets.
    /// The class allows access to script blocks, and frame definitions, too.
    /// The common properties can be changed through the PropertyGrid, optionally, some
    /// others are informative, like nimber of images, frames or script blocks.
    /// </remarks>
    public interface IDocumentStructure
    {

        # region Events

        /// <summary>
        /// Fires when the user presses the F1 key while the browser is the active window.
        /// </summary>
         event DocumentEventHandler Help;
        /// <summary>
        /// Fires when the user clicks the left mouse button on the object. 
        /// </summary>
         event DocumentEventHandler Click;
        /// <summary>
        /// Fires when the user double-clicks the object.
        /// </summary>
         event DocumentEventHandler DblClick;
        /// <summary>
        /// Fires when the user presses a key.
        /// </summary>
         event DocumentEventHandler KeyDown;
        /// <summary>
        /// Fires when the user releases a key.
        /// </summary>
         event DocumentEventHandler KeyUp;
        /// <summary>
        /// Fires when the user presses an alphanumeric key.
        /// </summary>
         event DocumentEventHandler KeyPress;
        /// <summary>
        /// Fires when the user clicks the object with either mouse button.
        /// </summary>
         event DocumentEventHandler MouseDown;
        /// <summary>
        /// Fires when the user moves the mouse over the object.
        /// </summary>
         event DocumentEventHandler MouseMove;
        /// <summary>
        /// Fires when the user releases a mouse button while the mouse is over the object.
        /// </summary>
         event DocumentEventHandler MouseUp;
        /// <summary>
        /// Fires when the user moves the mouse pointer outside the boundaries of the object.
        /// </summary>
         event DocumentEventHandler MouseOut;
        /// <summary>
        /// Fires when the user moves the mouse pointer into the object.
        /// </summary>
         event DocumentEventHandler MouseOver;
        /// <summary>
        /// Fires when the state of the object has changed.
        /// </summary>
         event DocumentEventHandler ReadystateChange;
        /// <summary>
        /// Fires on the source object when the user starts to drag a text selection or selected object.
        /// </summary>
         event DocumentEventHandler DragStart;
        /// <summary>
        /// Fires when the object is being selected.
        /// </summary>
         event DocumentEventHandler SelectStart;
        /// <summary>
        /// Fires when the user clicks the right mouse button in the client area, opening the context menu.
        /// </summary>
         event DocumentEventHandler ContextMenu;
        /// <summary>
        /// Fires when a property changes on the object.
        /// </summary>
         event DocumentEventHandler PropertyChange;
        /// <summary>
        /// Fires before an object contained in an editable element enters a UI-activated state or when an editable container object is control selected.
        /// </summary>
         event DocumentEventHandler BeforeEditFocus;
        /// <summary>
        /// Fires when the selection state of a document changes.
        /// </summary>
         event DocumentEventHandler SelectionChange;
        /// <summary>
        /// Fires when the user is about to make a control selection of the object.
        /// </summary>
         event DocumentEventHandler ControlSelect;
        /// <summary>
        /// Fires when the wheel button is rotated.
        /// </summary>
         event DocumentEventHandler MouseWheel;
        /// <summary>
        /// Fires for an element just prior to setting focus on that element.
        /// </summary>
         event DocumentEventHandler FocusIn;
        /// <summary>
        /// Fires for the current element with focus immediately after moving focus to another element.
        /// </summary>
         event DocumentEventHandler FocusOut;
        /// <summary>
        /// Fires when the object is set as the active element.
        /// </summary>
         event DocumentEventHandler Activate;
        /// <summary>
        /// Fires when the activeElement is changed from the current object to another object in the parent document.
        /// </summary>
         event DocumentEventHandler Deactivate;
        /// <summary>
        /// Fires immediately before the object is set as the active element.
        /// </summary>
         event DocumentEventHandler BeforeActivate;
        /// <summary>
        /// Fires immediately before the activeElement is changed from the current object to another object in the parent document.
        /// </summary>
         event DocumentEventHandler BeforeDeactivate;


        # endregion Events

        /// <summary>
        /// Get a collection of elements from the given tag name.
        /// </summary>
        /// <remarks>
        /// <para>
        /// For element type related operations this method retrieves a collection (ArrayList) of 
        /// elements of a given tag name. This method is commonly used to add DHTML events to specific
        /// elements, and add wizards or dialogs to - for example - a double click.
        /// </para>
        /// <para>
        /// The objects in the ArrayList are derived from IElement interface. They normally have their
        /// native base type, e.g. if one retrieves the collection of "A" tags the objects are from type
        /// AnchorElement. It is not recommended to change the expected base type. Even if
        /// it is working, it fails under some circumstances due to the different types of properties the
        /// elements provide. Returning the interface instead of a native class is a simplification to
        /// avoid the need of many getter methods for each type of element.
        /// </para>
        /// </remarks>
        /// <param name="tagName">Tag name to be retrieved</param>
        /// <returns>The method return null if no such elements in the document.</returns>
        ICollection GetElementCollection(string tagName);

        /// <summary>
        /// Get the collection of script blocks.
        /// </summary>
        /// <remarks>Supports PropertyGrid by applying a simple script editor (not supported
        /// in NetRix Light version).</remarks>
        ICollectionBase ScriptBlocks { get; set; }

        /// <summary>
        /// Get the current collection of META tags.</summary>
        /// <remarks>
        /// <para> The getter is called by the propertygrid via reflection
        /// to determine the META tags and use events to propagate new values.
        /// The setter is only to use from the host application and set the complete collection at once. To 
        /// add one value only just call the getter and then the Add() method.</para>
        /// <para>To delete the whole list simply assign null: <c>MetaCollection = null;</c>. This will <i>not</i> 
        /// destroy the object internally nor clear any resources, it will just call the <c>Clear</c> method.</para>
        /// </remarks>
        /// <example>
        /// The following example retrieves two TextBox controls to add a new META tag. It assumes that the
        /// current collection was previously retrieved by calling <c>this.MetaElements = this.HtmlEditor.DocumentStructure.MetaTags;</c>.
        /// <code>
        /// private void buttonAdd_Click(object sender, System.EventArgs e)
        /// {
        ///   if (this.textBoxName.Text.Length == 0 || this.textBoxContent.Text.Length == 0)
        ///   {
        ///       MessageBox.Show("Cannot add empty fields");
        ///   } 
        ///   else 
        ///   {
        ///       MetaElement mE = new MetaElement();
        ///       mE.name = this.textBoxName.Text;
        ///       mE.content = this.textBoxContent.Text;
        ///       this.MetaElements.Add(mE);
        ///   }
        /// }
        /// </code>
        /// </example>
        ICollectionBase NamedMetaTags { get; set; }

        /// <summary>
        /// Get the current collection of HTTP-EQUIV META tags.
        /// </summary>
        /// <remarks>
        /// <para> The getter is called by the propertygrid via reflection
        /// to determine the META tags and use events to propagate new values. HTTP-EQUIV meta tags are modifiers for
        /// HTTP headers. The are often used to implement auto refresh or overwrite the default settings for charset or MIME type.
        /// The setter is only to use from the host application and set the complete collection at once. To 
        /// add one value only just call the getter and then the Add() method.</para>
        /// <para>To delete the whole list simply assign null: <c>MetaCollection = null;</c>. This will <i>not</i> 
        /// destroy the object internally nor clear any resources, it will just call the <c>Clear</c> method.</para>
        /// <para>
        /// The MetaElement object is the same as used for named META tags. The both attributes used here
        /// (name="" and http-equiv="") are programmed in a toggle manner. This means that if the attribute name is set
        /// from hsot app the http-equiv parameter will set to String.Empty and vice versa.
        /// </para>
        /// </remarks>
        /// <example>
        /// The following example retrieves two TextBox controls to add a new META tag. It assumes that the
        /// current collection was previously retrieved by calling <c>this.MetaElements = this.HtmlEditor.DocumentStructure.MetaTags;</c>.
        /// <code>
        /// private void buttonAdd_Click(object sender, System.EventArgs e)
        /// {
        ///   if (this.textBoxName.Text.Length == 0 || this.textBoxContent.Text.Length == 0)
        ///   {
        ///       MessageBox.Show("Cannot add empty fields");
        ///   } 
        ///   else 
        ///   {
        ///       MetaElement mE = new MetaElement();
        ///       mE.httpEquiv = this.textBoxName.Text;
        ///       mE.content = this.textBoxContent.Text;
        ///       this.MetaElements.Add(mE);
        ///   }
        /// }
        /// </code>
        /// </example>
        ICollectionBase HttpEquivMetaTags { get; set; }

        /// <summary>
        /// This getter retrieves the collection of embedded stylesheets. It detects the style tags in the head section.
        /// </summary>
        ICollectionBase EmbeddedStylesheets { get; set; }

        /// <summary>
        /// This getter retrieves the collection of linked stylesheets.
        /// </summary>
        /// <remarks>
        /// It detects the stylesheets in link tags in a tolerant
        /// way, ether by checking the type attribute for "text/css" or the url in href attribute ends with ".css".
        /// </remarks>
        ICollectionBase LinkedStylesheets { get; set; }

        /// <summary>
        /// Sets or gets the document title.
        /// </summary>
        string Title { get; set; }

        /// <summary>
        /// Gets the current documents encoding.
        /// </summary>
        [Browsable(false)]
        System.Text.Encoding Encoding { get; }

        /// <summary>
        /// Gets the character set used by the document.
        /// </summary>
        string charset { get; }

        /// <summary>
        /// Gets the URL or local path where the document comes from.
        /// </summary>
        string URL { get; }

        /// <summary>
        /// Returns the current BASE element. 
        /// </summary>
        /// <returns>Return the BaseElement object, if any, or <c>null</c> (<c>Nothing</c> in Visual Basic) if there is no such element.</returns>
        IElement GetBase();
        
        /// <summary>
        /// Returns the current BGSOUND element. 
        /// </summary>
        /// <returns>Return the BgSoundElement object, if any, or <c>null</c> (<c>Nothing</c> in Visual Basic) if there is no such element.</returns>
        IElement GetBgSound();

        /// <overloads/>
        /// <summary>
        /// Inserts a BGSOUND tag in the head section.
        /// </summary>
        /// <remarks>
        /// The default value for LOOPS is set to -1, the default for AUTOSTART is set to TRUE.
        /// </remarks>
        /// <param name="src">The name of the sound file to be played.</param>
        /// <returns>Returns the native element object.</returns>
        IElement SetBgSound(string src);

        /// <summary>
        /// Inserts a BGSOUND tag in the head section.
        /// </summary>
        /// <remarks>
        /// The default for AUTOSTART is set to TRUE.
        /// </remarks>
        /// <param name="src">The name of the sound file to be played.</param>
        /// <param name="loop"></param>
        /// <returns>Returns the native element object.</returns>
        IElement SetBgSound(string src, int loop);

        /// <summary>
        /// Inserts a BGSOUND tag in the head section.
        /// </summary>
        /// <remarks>
        /// All parameters can be set directly.
        /// </remarks>
        /// <param name="src">The name of the sound file to be played.</param>
        /// <param name="loop"></param>
        /// <param name="autostart"></param>
        /// <returns>Returns the native element object.</returns>
        IElement SetBgSound(string src, int loop, bool autostart);

        /// <overloads/>
        /// <summary>
        /// Inserts a BASE element without target information.
        /// </summary>
        /// <param name="href">Set the default address for hypertext links.</param>
        /// <returns>Returns the native element object.</returns>
        IElement SetBase(string href);

        /// <summary>
        /// Inserts a BASE element.
        /// </summary>
        /// <param name="href">Set the default address for hypertext links.</param>
        /// <param name="target">Sets the default window for linked documents. Can be <c>null</c> (<c>Nothing</c> in Visual Basic) to avoid setting this attribute.</param>
        /// <returns>Returns the native element object.</returns>
        IElement SetBase(string href, string target);

        /// <summary>
        /// Returns the base path of the document without filename.
        /// </summary>
        /// <remarks>
        /// Used to build relative paths
        /// (links) from this document to subdocs, like script blocks, stylesheets etc.
        /// If the document is in browse mode and URL returns a URI beginning with "http:", we
        /// return nothing here (String.Empty).
        /// </remarks>
        string BasePath { get; }
        /// <summary>
        /// The time it takes to load the document with a 56K modem connection. This value does
        /// not calculate the embedded elements, images and so on.
        /// </summary>
        string LoadTime56K { get; }
        /// <summary>
        /// The file size in bytes.
        /// </summary>
        long FileSize { get; }
        /// <summary>
        /// Time and date when the file was last modified.
        /// </summary>
        string FileLastModified { get; }
        /// <summary>
        /// The number of frames defined in this document. Does not recognize IFrame tags.
        /// </summary>
        int NumberOfFrames { get; }
        /// <summary>
        /// The number of styles defined in this document. Does not recognize inline style tags.
        /// </summary>
        int NumberOfStylesheets { get; }

        /// <summary>
        /// The number of embedded images.
        /// </summary>
        int NumberOfImages { get; }

        /// <summary>
        /// The number of script blocks.
        /// </summary>
        int NumberOfScriptBlocks { get; }

        /// <summary>
        /// The total number of meta tags.
        /// </summary>
        int NumberOfMetaTags { get; }

        /// <summary>
        /// Overridden to help designers displaying the object to the user.
        /// </summary>
        /// <returns></returns>
        string ToString();

    }
}