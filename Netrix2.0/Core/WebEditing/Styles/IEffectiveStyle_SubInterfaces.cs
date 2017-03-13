using System;

namespace GuruComponents.Netrix.WebEditing.Styles
{
    /// <summary>
    /// Holds one background style definition.
    /// </summary>
    public interface IBackgroundStyles
    {
        /// <summary>
        /// Attachment name (usually, an URL).
        /// </summary>
        string Attachment
        {
            get;
        }
        /// <summary>
        /// Color
        /// </summary>
        System.Drawing.Color Color
        {
            get;
        }
        /// <summary>
        /// Image
        /// </summary>
        string Image
        {
            get;
        }
        /// <summary>
        /// Repeat information.
        /// </summary>
        BackgroundRepeat Repeat
        {
            get;
        }

        /// <summary>
        /// Retrieves the x-coordinate and y-coordinate of the backgroundPosition property.  
        /// </summary>
        IBackgroundPosition Position
        { 
            get;
        }  

    }

    /// <summary>
    /// Holds one background position definition.
    /// </summary>
    public interface IBackgroundPosition
    {

        /// <summary>
        /// X position
        /// </summary>
        System.Web.UI.WebControls.Unit X
        {
            get;
        }
        /// <summary>
        /// Y position
        /// </summary>
        System.Web.UI.WebControls.Unit Y
        {
            get;
        }        

    }

    /// <summary>
    /// Holds one border property definition.
    /// </summary>
    public interface IBorderEffective
    {
        /// <summary>
        /// Left border property definition.
        /// </summary>
        IBorderProperties Left
        {
            get;
        }

        /// <summary>
        /// Right border property definition.
        /// </summary>
        IBorderProperties Right
        {
            get;
        }

        /// <summary>
        /// Top border property definition.
        /// </summary>
        IBorderProperties Top
        {
            get;
        }

        /// <summary>
        /// Bottom border property definition.
        /// </summary>
        IBorderProperties Bottom
        {
            get;
        }

    }

    /// <summary>
    /// Holds the style clip definition. 
    /// </summary>
    /// <remarks>
    /// The clip is a rectangle, which is defined using a left, right, top and bottom value. 
    /// </remarks>
    public interface IClipEffective
    {
        /// <summary>
        /// Clip on left side.
        /// </summary>
        string Left
        {
            get;
        }
        /// <summary>
        /// Clip on top side.
        /// </summary>
        string Top
        {
            get;
        }
        /// <summary>
        /// Clip on right side.
        /// </summary>        
        string Right
        {
            get;
        }
        /// <summary>
        /// Clip on bottom side.
        /// </summary>    
        string Bottom
        {
            get;
        }

    }

    /// <summary>
    /// Holds a font definition.
    /// </summary>
    /// <remarks>
    /// The font is defined using Font Family, Size, Style, Variant and Weight.
    /// </remarks>
    public interface IFontEffective
    {
        /// <summary>
        /// Font family
        /// </summary>
        string FontFamily
        {
            get;
        }
        /// <summary>
        /// Font size
        /// </summary>
        System.Web.UI.WebControls.Unit FontSize
        {
            get;
        }
        /// <summary>
        /// Style
        /// </summary>
        string FontStyle
        {
            get;
        }
        /// <summary>
        /// Variant
        /// </summary>
        string FontVariant
        {
            get;
        }
        /// <summary>
        /// Weight
        /// </summary>
        string FontWeight
        {
            get;
        }

    }

    /// <summary>
    /// Used to handle properties build with four values.
    /// </summary>
    /// <remarks>
    /// This class is used to hold the values for left, right, top, and bottom used by the
    /// effective style definition.
    /// </remarks>
    public interface IFourProperties
    {
        /// <summary>
        /// Left property.
        /// </summary>
        string Left
        {
            get;
        }
        /// <summary>
        /// Right property.
        /// </summary>
        string Right
        {
            get;
        }
        /// <summary>
        /// Top property.
        /// </summary>
        string Top
        {
            get;
        }
        /// <summary>
        /// Bottom property.
        /// </summary>
        string Bottom
        {
            get;
        }

    }

    /// <summary>
    /// Border properties.
    /// </summary>
    public interface IBorderProperties
    {
        /// <summary>
        /// Color
        /// </summary>
        System.Drawing.Color BorderColor
        {
            get;
        }
        /// <summary>
        /// Style
        /// </summary>
        System.Web.UI.WebControls.BorderStyle BorderStyle
        {
            get;
        }
        /// <summary>
        /// Width
        /// </summary>
        string BorderWidth
        {
            get;
        }

    }


}
