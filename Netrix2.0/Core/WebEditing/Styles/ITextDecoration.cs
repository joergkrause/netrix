using System;
namespace GuruComponents.Netrix.WebEditing.Styles
{
    /// <summary>
    /// Text decoration style definition.
    /// </summary>
    public interface ITextDecoration
    {

        /// <summary>
        /// Sets or retrieves a value that indicates whether the text in the object 
        /// has blink, line-through, overline, or underline decorations.
        /// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.IStyle">IStyle</see> 
		/// class for more details.
		/// </remarks>
		/// <seealso cref="GuruComponents.Netrix.WebEditing.Styles.IStyle"/>
		string textDecoration{ get; set;}    
        
		/// <summary>
        /// Sets or retrieves a Boolean value that indicates whether the object's 
        /// IHTMLStyle::textDecoration property has a value of "blink." 
        /// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.IStyle">IStyle</see> 
		/// class for more details.
		/// </remarks>
		/// <seealso cref="GuruComponents.Netrix.WebEditing.Styles.IStyle"/>
		bool textDecorationBlink{ get; set;}  
        
		/// <summary>
        /// Sets or retrieves a Boolean value indicating whether the text in the object has a 
        /// line drawn through it.  
        /// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.IStyle">IStyle</see> 
		/// class for more details.
		/// </remarks>
		/// <seealso cref="GuruComponents.Netrix.WebEditing.Styles.IStyle"/>
		bool textDecorationLineThrough{ get; set;}  
        
		/// <summary>
        /// Sets or retrieves the Boolean value indicating whether the IHTMLStyle::textDecoration 
        /// property for the object has been set to none.  
        /// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.IStyle">IStyle</see> 
		/// class for more details.
		/// </remarks>
		/// <seealso cref="GuruComponents.Netrix.WebEditing.Styles.IStyle"/>
		bool textDecorationNone{ get; set;}  
        
		/// <summary>
        /// Sets or retrieves a Boolean value indicating whether the text in the object 
        /// has a line drawn over it.  
        /// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.IStyle">IStyle</see> 
		/// class for more details.
		/// </remarks>
		/// <seealso cref="GuruComponents.Netrix.WebEditing.Styles.IStyle"/>
		bool textDecorationOverline{ get; set;}  
        
		/// <summary>
        /// Sets or retrieves whether the text in the object is underlined.  
        /// </summary>
		/// <remarks>
		/// See <see cref="GuruComponents.Netrix.WebEditing.Styles.IStyle">IStyle</see> 
		/// class for more details.
		/// </remarks>
		/// <seealso cref="GuruComponents.Netrix.WebEditing.Styles.IStyle"/>
		bool textDecorationUnderline{ get; set;} 
    }
}
