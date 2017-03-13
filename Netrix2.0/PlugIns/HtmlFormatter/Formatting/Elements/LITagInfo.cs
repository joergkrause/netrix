using System;

namespace GuruComponents.Netrix.HtmlFormatting.Elements
{
    sealed class LITagInfo : TagInfo
    {
		public LITagInfo () : base  ("li", FormattingFlags.None, WhiteSpaceType.NotSignificant, WhiteSpaceType.CarryThrough)
		{
			return;
		} 

        public override bool CanContainTag(ITagInfo info)
        {
            if (info.Type == ElementType.Any)
            {
                return true;
            }
            if ((info.Type == ElementType.Inline | info.Type == ElementType.Block)) /* JOERG:  != 0 */
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
