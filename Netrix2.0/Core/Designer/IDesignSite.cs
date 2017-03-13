using System.ComponentModel;
using System.Web.UI;

namespace GuruComponents.Netrix.Designer
{

    /// <summary>
    /// Basic interface for sited components.
    /// </summary>
    /// <remarks>Used to site native element objects to support the designer environment.</remarks>
    public interface IDesignSite : ISite
    {

        /// <summary>
        /// TODO: Add comment.
        /// </summary>
        void SetComponent(IComponent component);

        /// <summary>
        /// TODO: Add comment.
        /// </summary>
        void SetElement(Control element);

        /// <summary>
        /// TODO: Add comment.
        /// </summary>
        void SetName(string name);

    }

}
