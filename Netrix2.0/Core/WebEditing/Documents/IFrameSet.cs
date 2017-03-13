using System;
using System.Globalization;
using System.Drawing;
using System.IO;
using System.Text;
using System.Xml;
using System.Runtime.InteropServices;
using System.Collections;
using GuruComponents.Netrix;
using GuruComponents.Netrix.Events;
using GuruComponents.Netrix.HtmlFormatting;
using GuruComponents.Netrix.ComInterop;
using GuruComponents.Netrix.WebEditing.Behaviors;
using GuruComponents.Netrix.WebEditing.Elements;

namespace GuruComponents.Netrix.WebEditing.Documents
{
	/// <summary>
	/// IFrameSet represents a complete framed master document.
	/// </summary>
	/// <remarks>
	/// This is the document containing
	/// one or more FrameSet tags. It returns a collection of FrameWindow objects which contain the
	/// complete frames, including all content and site management. It also contains a collection
	/// of HtmlFrameSet objects which represent frame documents which contain frames themself. This
	/// is a recursive strategy so there is no limit in handling nested frames.
	/// </remarks>
	public interface IFrameSet
	{

        /// <summary>
        /// This method creates a collection of all frames in the document, regardless in which frameset
        /// they are placed.
        /// </summary>
        /// <remarks>
        /// The collection contains <see cref="IFrameWindow">IFrameWindow</see> objects.
        /// </remarks>
        void ActivateFrames();

        /// <summary>
        /// Removes the attached frame behaviors and deactivate the frame editor. Clears the frame collection. The document
        /// will still remains in memory and can be edited using standard editing features.
        /// </summary>
        void DeactivateFrames();

        /// <summary>
        /// Gets the collection of FrameWindow objects. Each entry represents one 
        /// frame in the current frameset. Return null if there are no frames.
        /// </summary>
        ICollection FrameCollection { get; }

        /// <summary>
        /// Gets the collection of Framesets. This list contains documents which are part
        /// of a framed document and contain a additional frame definition. Each document
        /// is represented by a HtmlFrameSet object which contains FrameWindow object and so on,
        /// in recursively manner.
        /// </summary>
        ICollection FrameSetCollection { get; }

        /// <summary>
        /// Returs the active frame.
        /// </summary>
        /// <returns></returns>
        IFrameWindow GetActiveFrame();

        /// <summary>
        /// The number of frames of the current set. Frames in subsets are not counted.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Returns the structure of the frame definition document as <see cref="XmlDocument"/> object.
        /// </summary>
        /// <remarks>
        /// This is not an interactive
        /// access to the underlying structure and therefore changes in the DOM of the returned object are not reflected in the
        /// document. To change the structure using XML it is recommended to read <see cref="XmlDocument"/> object, change the content
        /// and reload the document using the <see cref="IHtmlEditor.LoadUrl"/> method.
        /// </remarks>
        /// <returns>The formatted document</returns>
        XmlDocument GetFrameStructure();

//        /// <summary>
//        /// This event is fired when the user right clicks the surface and no other
//        /// handler handles this click.
//        /// </summary>
//        /// <remarks>
//        /// It is normally used to present a customized
//        /// context men. It is up to the host application to create and show the 
//        /// menu. The event informs the application only to do so, it does not perform
//        /// any internal action.
//        /// </remarks>
//        event ShowContextMenuEventHandler OnShowContextMenu;
//
//        /// <summary>
//        /// After loading a new frameset each frame will handled as seperate document.</summary><remarks> Each
//        /// document fires a ready state complete event after completing the presentation. 
//        /// It is not necessary to use this event, because all commands will delayed until
//        /// the document is ready. On the other hand this event is helpful to attach additional
//        /// binary behaviors of other static configuration to specific frames. Such external
//        /// actions are not recognized by the command delayer and must not applied to a
//        /// document during load time.
//        /// </remarks>
//        event EventHandler OnReadyStateComplete;

        /// <summary>
        /// This event is fired when the user first activates a frame to begin editing
        /// there.</summary><remarks>The event is internally used to set the <see cref="ActivateFrames"/> method,
        /// which is an alternative way but polling the current frame.
        /// <para>
        /// It is recommended to to use the event instead of polling the frame state.
        /// </para>
        /// </remarks> 
        event FrameActivatedEventHandler FrameActivated;



    }
}
