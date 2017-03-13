using GuruComponents.Netrix.WebEditing.Elements;

namespace GuruComponents.Netrix.Events
{
    /// <summary>
    /// Event used to handle the displaying of property information within the PropertyGrid or similar environments.
    /// </summary>
    /// <seealso cref="PropertyDisplayEventArgs"/>
    /// <param name="sender">Snder of data, ususally an element object, derived from <see cref="IElement"/>.</param>
    /// <param name="e">Display event arguments object.</param>
    public delegate void PropertyDisplayHandler(object sender, PropertyDisplayEventArgs e);
}