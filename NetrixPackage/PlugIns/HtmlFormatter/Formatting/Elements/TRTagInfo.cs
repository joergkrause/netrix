using System;

namespace GuruComponents.Netrix.HtmlFormatting.Elements
{
    sealed class TRTagInfo : TagInfo
    {

		public TRTagInfo () : base  ("tr", FormattingFlags.None, WhiteSpaceType.NotSignificant, WhiteSpaceType.NotSignificant, ElementType.Any)
		{
			return;
		} 

        public override bool CanContainTag(ITagInfo info)
        {
            if (info.Type == ElementType.Any)
            {
                return true;
            }
            if ((info.TagName.ToLower().Equals("th") | info.TagName.ToLower().Equals("td")))
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
