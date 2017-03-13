using System;
using GuruComponents.Netrix.HtmlFormatting.Elements;

namespace Comzept.Genesis.NetRix.WebEditing.Formatting.Elements
{
    sealed class OLTagInfo : TagInfo
    {

		public OLTagInfo () : base  ("ol", FormattingFlags.None, WhiteSpaceType.NotSignificant, WhiteSpaceType.NotSignificant, ElementType.Block)
		{
			return;
		} 

        public override bool CanContainTag(ITagInfo info)
        {
            if (info.Type == ElementType.Any | info.Type == ElementType.Inline)
            {
                return true;
            }
            if (info.TagName.ToLower().Equals("li"))
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
