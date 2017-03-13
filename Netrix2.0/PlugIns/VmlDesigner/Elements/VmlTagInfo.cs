using System;

using GuruComponents.Netrix.HtmlFormatting.Elements;

namespace GuruComponents.Netrix.VmlDesigner.Elements
{
	/// <summary>
	/// This class is used to store information about the kind of element.
	/// </summary>
	/// <remarks>
	/// It is used internally to control the formatting behavior. Externally it is used to provide such information for plug-in modules.
	/// </remarks>
    public class VmlTagInfo : ITagInfo
    {
        private string _tagName;
        private WhiteSpaceType _inner;
        private WhiteSpaceType _following;
        private FormattingFlags _flags;
        private ElementType _type;

        /// <summary>
        /// The type of element controls the generel formatting behavior.
        /// </summary>
        public ElementType Type
        {
            get
            {
                return _type;
            }
        }

        /// <summary>
        /// Determines how to format this kind of element.
        /// </summary>
        public FormattingFlags Flags
        {
            get
            {
                return _flags;
            }
        }
        /// <summary>
        /// Determines how significant are whitespaces following this element.
        /// </summary>
        public WhiteSpaceType FollowingWhiteSpaceType
        {
            get
            {
                return _following;
            }
        }
        /// <summary>
        /// Determines how significant are whitespaces within this element.
        /// </summary>
        public WhiteSpaceType InnerWhiteSpaceType
        {
            get
            {
                return _inner;
            }
        }
        /// <summary>
        /// The element has to treated as comment.
        /// </summary>
        public bool IsComment
        {
            get
            {
                return (_flags & FormattingFlags.Comment) == 0 == false;
            }
        }
        /// <summary>
        /// The element is an inline element.
        /// </summary>
        public bool IsInline
        {
            get
            {
                return (_flags & FormattingFlags.Inline) == 0 == false;
            }
        }
        /// <summary>
        /// The element has to be treated as pure XML.
        /// </summary>
        public bool IsXml
        {
            get
            {
                return (_flags & FormattingFlags.Xml) == 0 == false;
            }
        }
        /// <summary>
        /// The element does not has an end tag, e.g. it is not a container.
        /// </summary>
        public bool NoEndTag
        {
            get
            {
                return (_flags & FormattingFlags.NoEndTag) == 0 == false;
            }
        }
        /// <summary>
        /// The element does not being indented, even if it starts at a new line.
        /// </summary>
        public bool NoIndent
        {
            get
            {
                if ((_flags & FormattingFlags.NoIndent) == 0)
                {
                    return NoEndTag;
                }
                else
                {
                    return true;
                }
            }
        }
        /// <summary>
        /// The element is supposed to preserve its content.
        /// </summary>
        public bool PreserveContent
        {
            get
            {
                return (_flags & FormattingFlags.PreserveContent) == 0 == false;
            }
        }
        /// <summary>
        /// The tag name of the element.
        /// </summary>
        public string TagName
        {
            get
            {
                return String.Concat("v", ":", _tagName);
            }
        }

        /// <overloads/>
        /// <summary>
        /// Creates a new tag info with basic parameters. 
        /// </summary>
        /// <param name="tagName"></param>
        /// <param name="flags"></param>
        public VmlTagInfo(string tagName, FormattingFlags flags) : this(tagName, flags, WhiteSpaceType.CarryThrough, WhiteSpaceType.CarryThrough, ElementType.Other)
        {
        }
        /// <summary>
        /// Creates a new tag info with basic parameters. 
        /// </summary>
        /// <param name="tagName"></param>
        /// <param name="flags"></param>
        /// <param name="type"></param>
        public VmlTagInfo(string tagName, FormattingFlags flags, ElementType type) : this(tagName, flags, WhiteSpaceType.CarryThrough, WhiteSpaceType.CarryThrough, type)
        {
        }
        /// <summary>
        /// Creates a new tag info with basic parameters. 
        /// </summary>
        /// <param name="tagName"></param>
        /// <param name="flags"></param>
        /// <param name="innerWhiteSpace"></param>
        /// <param name="followingWhiteSpace"></param>
        public VmlTagInfo(string tagName, FormattingFlags flags, WhiteSpaceType innerWhiteSpace, WhiteSpaceType followingWhiteSpace) : this(tagName, flags, innerWhiteSpace, followingWhiteSpace, ElementType.Other)
        {
        }
        /// <summary>
        /// Creates a new tag info with basic parameters. 
        /// </summary>
        /// <param name="tagName"></param>
        /// <param name="flags"></param>
        /// <param name="innerWhiteSpace"></param>
        /// <param name="followingWhiteSpace"></param>
        /// <param name="type"></param>
        public VmlTagInfo(string tagName, FormattingFlags flags, WhiteSpaceType innerWhiteSpace, WhiteSpaceType followingWhiteSpace, ElementType type)
        {
            _tagName = tagName;
            _inner = innerWhiteSpace;
            _following = followingWhiteSpace;
            _flags = flags;
            _type = type;
        }
        /// <summary>
        /// Creates a new tag info with basic parameters. 
        /// </summary>
        /// <param name="newTagName"></param>
        /// <param name="info"></param>
        public VmlTagInfo(string newTagName, ITagInfo info)
        {
            _tagName = newTagName;
            _inner = info.InnerWhiteSpaceType;
            _following = info.FollowingWhiteSpaceType;
            _flags = info.Flags;
            _type = info.Type;
        }
        /// <summary>
        /// Informs that a element can carry another one.
        /// </summary>
        /// <remarks>
        /// This method always returns true. Classes derive from <see cref="VmlTagInfo"/> may
        /// overwrite it to control the behavior of complex elements.
        /// </remarks>
        /// <param name="info"></param>
        /// <returns></returns>
        public virtual bool CanContainTag(ITagInfo info)
        {
            return true;
        }
    }

}
