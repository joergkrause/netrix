using System;
using System.Xml;
#pragma warning disable 1591
namespace GuruComponents.Netrix.XmlDesigner.Edx
{

	/// <summary>
	/// Little object defining a match inside a container.
	/// </summary>
	public struct Match
	{

		public string tag;
		public string edxtemplate;
		public string uiname;
		public XmlNode oTemplate;
	
		public Match(string sTag, string sTemp, string sUI, XmlNode oTemp )
		{
			this.tag = sTag;
			this.edxtemplate = sTemp;
			this.uiname = sUI;
			this.oTemplate = oTemp;
		}
	}
}
