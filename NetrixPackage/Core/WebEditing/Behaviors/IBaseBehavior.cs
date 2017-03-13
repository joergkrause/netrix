using System;
using GuruComponents.Netrix.ComInterop;

namespace GuruComponents.Netrix.WebEditing.Behaviors
{
    /// <summary>
    /// This interface provides the base structure of all internal and external binary behavior classes.
    /// </summary>
    /// <remarks>
    /// It is not recommended to use this interface directly. A better way to make use of this feature is the
    /// abstract base class <see cref="GuruComponents.Netrix.WebEditing.Behaviors.BaseBehavior">BaseBehavior</see>.
    /// </remarks>
    public interface IBaseBehavior : Interop.IHTMLPainter, Interop.IElementBehavior //, Interop.IElementBehaviorFactory
    {
        /// <summary>
        /// Unique name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Associated control.
        /// </summary>
        /// <param name="editor"></param>
        /// <returns></returns>
        System.Web.UI.Control GetElement(IHtmlEditor editor);

		//Interop.IElementBehavior GetBehavior(IHtmlEditor editor, Interop.IHTMLElement element);

	}
}
