using System;

namespace GuruComponents.Netrix.HtmlFormatting.Elements
{
    sealed class PTagInfo : TagInfo
    {

#if RN
		public PTagInfo () : base  ("p", FormattingFlags.None, WhiteSpaceType.CarryThrough, WhiteSpaceType.CarryThrough, ElementType.Block)
		{
			return;
		} 
#else
        public PTagInfo()
            : base("p", FormattingFlags.None, WhiteSpaceType.NotSignificant, WhiteSpaceType.NotSignificant, ElementType.Block)
        {
            return;
        }
#endif

        public override bool CanContainTag(ITagInfo info)
        {
            if (info.Type == ElementType.Any | info.Type == ElementType.Inline | info.Type == ElementType.Block) //info.TagName.ToLower().Equals("table") | info.TagName.ToLower().Equals("hr"))) /* JOERG: != 0 */
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

}
