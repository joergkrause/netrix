namespace GuruComponents.Netrix.UserInterface.FontPicker
{	
    /// <summary>
    /// Tells the FontListBox what Font list to load. The user list is filled by
    /// the client application.
    /// </summary>
    public enum FontFamilyType
    {
        /// <summary>
        /// System font
        /// </summary>
        System,
        /// <summary>
        /// Web related fonts (IE fonts).
        /// </summary>
        Web,
        /// <summary>
        /// Generic fonts, like "Sans Serif".
        /// </summary>
        Generic,
        /// <summary>
        /// User defined list.
        /// </summary>
        User
    }

    /// <summary>
    /// Tells the FontListBox how to show the fonts (with or without Sample).
    /// </summary>
    public enum ListBoxType
    {
        /// <summary>
        /// Display the names.
        /// </summary>
        FontName,
        /// <summary>
        /// Display the sample string only.
        /// </summary>
        FontSample,
        /// <summary>
        /// Display both, name and sample (recommended).
        /// </summary>
        FontNameAndSample
    }
}