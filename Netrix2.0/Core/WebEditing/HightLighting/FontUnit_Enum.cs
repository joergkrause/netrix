using System;

namespace GuruComponents.Netrix.WebEditing 
{

    /// <summary>
    /// This enumeration returns the names and values of the possible
    /// values used for the "size" attribute of "font" tag.
    /// </summary>
    /// <remarks>
    /// If a dialog
    /// applies to CSS "font-size" and HTML font tag with size attribute
    /// you must think about the relation between the named parameters.
    /// The x-small in CSS does not correspond exactly to the value 2 in 
    /// HTML, whereas HTML parameter "7" has no direct counterpart in CSS.
    /// <para>
    /// A full description about the implementation details can be found
    /// here: 
    /// <link>http://style.cleverchimp.com/font_size_intervals/altintervals.html</link>.
    /// </para>
    /// </remarks>
    public enum FontUnit : int
    {
        /// <summary>
        /// Remove size attribute
        /// </summary>
        Empty   = 0,
        /// <summary>
        /// Set the size attribute to the value 1.
        /// </summary>
        XXSmall = 1,
        /// <summary>
        /// Set the size attribute to the value 2.
        /// </summary>
        XSmall  = 2,
        /// <summary>
        /// Set the size attribute to the value 3.
        /// </summary>
        Small   = 3,
        /// <summary>
        /// Set the size attribute to the value 4.
        /// </summary>
        Medium  = 4,
        /// <summary>
        /// Set the size attribute to the value 5.
        /// </summary>
        Large   = 5, 
        /// <summary>
        /// Set the size attribute to the value 6.
        /// </summary>
        XLarge  = 6,
        /// <summary>
        /// Set the size attribute to the value 7.
        /// </summary>
        XXLarge = 7,
        /// <summary>
        /// Set the size attribute to the value -1.
        /// </summary>
        Smaller = 100,
        /// <summary>
        /// Set the size attribute to the value +1.
        /// </summary>
        Larger  = 101
    }
}