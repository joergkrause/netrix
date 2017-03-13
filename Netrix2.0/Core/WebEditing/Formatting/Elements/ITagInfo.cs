using System;

namespace GuruComponents.Netrix.HtmlFormatting.Elements
{
	/// <summary>
	/// This class is used to store information about the kind of element.
	/// </summary>
	/// <remarks>
	/// It is used internally to
	/// control the formatting behavior. Externally it is used to provide such information for plug-in modules.
	/// </remarks>
    public interface ITagInfo
    {
        /// <summary>
        /// Type
        /// </summary>
        ElementType Type { get; }
        /// <summary>
        /// Flags
        /// </summary>
        FormattingFlags Flags { get; }
        /// <summary>
        /// Following WhiteSpace Type
        /// </summary>
        WhiteSpaceType FollowingWhiteSpaceType { get; }
        /// <summary>
        /// Inner WhiteSpace Type
        /// </summary>
        WhiteSpaceType InnerWhiteSpaceType{ get; }
        /// <summary>
        /// Is comment
        /// </summary>
        bool IsComment{ get; }
        /// <summary>
        /// Is inline
        /// </summary>
        bool IsInline{ get; }
        /// <summary>
        /// Is XML
        /// </summary>
        bool IsXml{ get; }
        /// <summary>
        /// Has no end tag
        /// </summary>
        bool NoEndTag{ get; }
        /// <summary>
        /// Cannot indent
        /// </summary>
        bool NoIndent{ get; }
        /// <summary>
        /// Preserve content
        /// </summary>
        bool PreserveContent{ get; }
        /// <summary>
        /// Tag's name
        /// </summary>
        string TagName{ get; }
        /// <summary>
        /// Can have other tags in it.
        /// </summary>
        /// <param name="info">Which one?</param>
        /// <returns></returns>
        bool CanContainTag(ITagInfo info);
    }

}
