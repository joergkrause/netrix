using System;
using GuruComponents.Netrix.WebEditing.Elements;
using GuruComponents.Netrix.HtmlFormatting;

namespace GuruComponents.Netrix.WebEditing.Documents
{
    /// <summary>
    /// This class represents a single frame in the frameset.
    /// </summary>
    /// <remarks>
    /// Its objects are stored together with
    /// the related Site to handle the events.  
    /// </remarks>
    public interface IFrameWindow
    {

        /// <summary>
        /// Deactivate the frame designer and remove all attached behaviors.
        /// </summary>
        void DeactivateDesigner();

        /// <summary>
        /// Add a behavior to this frame.
        /// </summary>
        /// <remarks>
        /// Binary behaviors are permanent drawings on the surface. Multiple
        /// behaviors are drawn in the order they are added.
        /// </remarks>
        /// <param name="behavior">The behavior that is used to change the frame area.</param>
        void AddBehavior(GuruComponents.Netrix.WebEditing.Behaviors.IBaseBehavior behavior);

        /// <summary>
        /// Remove a previously set binary behavior. See <see cref="AddBehavior"/>.
        /// </summary>
        /// <param name="behavior"></param>
        void RemoveBehavior(GuruComponents.Netrix.WebEditing.Behaviors.IBaseBehavior behavior);

        /// <summary>
        /// Removes all attached behaviors assigned to that frame.
        /// </summary>
        void RemoveAllBehaviors();

        /// <summary>
        /// Make the content editable in the editor. Gets the current editable state.
        /// </summary>
        bool ContentEditable { get; set; }

        /// <summary>
        /// Gets true if content was changed snce last save operation.
        /// </summary>
        bool IsDirty { get; }

        /// <summary>
        /// Sets or gets the current encoding for that frame.
        /// </summary>
        /// <remarks>
        /// It is not necessary to set
        /// this property as it defaults to the global Encoding of the main document.
        /// </remarks>
        System.Text.Encoding Encoding { get; set; }

        /// <summary>
        /// Save the raw content into the file the frame was loaded from. Overwrites.
        /// </summary>
        void SaveRawContent();

        /// <summary>
        /// Returns the full path to the document based on the relative path and the current position
        /// in file system.
        /// </summary>
        /// <remarks>
        /// If the Source is a HTTP URL this method returns the Url as is.
        /// The internally used URI format with file:// moniker is removed before returning.
        /// </remarks>
        /// <returns>Full path in file format, leading monikers are removed, URL coding is decoded.</returns>
        string GetFullPathUrl();

        /// <summary>
        /// Save the formatted content into the file the frame was loaded from.
        /// </summary>
        /// <remarks>
        /// Overwrites an existing file.
        /// </remarks>
        /// <param name="fo">Formatter Options.</param>
        void SaveFormattedContent(IHtmlFormatterOptions fo);

        /// <summary>
        /// Returns the raw content.
        /// </summary>
        /// <remarks>
        /// Usable if the host application has its own save method.
        /// </remarks>
        /// <returns>String with raw content</returns>
        string GetRawContent();
            
        /// <summary>
        /// Returns a well formatted representation of the frame content.
        /// </summary>
        /// <param name="fo">Formatter Options</param>
        /// <returns>String of well formatted and XHTML compatible content</returns>
        string GetFormattedContent(IHtmlFormatterOptions fo);

        /// <summary>
        /// Returns a string with the outer html of the body.
        /// </summary>
        /// <remarks>
        /// This is for further investigation only,
        /// not for saving, as it does not contain the full content.
        /// </remarks>
        /// <returns>String which contains the content of the frame.</returns>
        string GetBodyContent();

        /// <summary>
        /// The name of the frame as it is in the name attribute of the frame definition.
        /// </summary>
        string FrameName { get; set; }

        /// <summary>
        /// The frame src attribute, mostly the filename and a relative path. URL format.
        /// </summary>
        string FrameSrc { get; set; }

        /// <summary>
        /// The native frame element object. Used for PropertyGrid to set various parameters.
        /// </summary>
        IElement NativeFrameElement { get; }
    }

}
