using System;

namespace GuruComponents.Netrix.HtmlFormatting.Elements
{
    sealed class TDTagInfo : TagInfo
    {
		
		public TDTagInfo () : base  ("td", FormattingFlags.None, WhiteSpaceType.NotSignificant, WhiteSpaceType.NotSignificant, ElementType.Other)
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
