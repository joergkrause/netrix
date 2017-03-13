using System;
using System.Collections.Generic;
using System.Text;

namespace GuruComponents.Netrix
{
    /// <summary>
    /// Allows service based components to add the editor instance where the service is hosted.
    /// </summary>
    public interface IEditorInstanceService
    {
        /// <summary>
        /// Returns the editor the designer refers to.
        /// </summary>
        IHtmlEditor EditorInstance
        {
            get;
        }
    }
}
