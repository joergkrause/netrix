using System;
using System.Collections;
using System.Drawing;
using System.Text.RegularExpressions;

namespace GuruComponents.Netrix.UserInterface.StyleParser
{
	/// <summary>
	/// This is the StyleParser. It parses a stylesheet and creates the style objects from
	/// the found strings.
	/// </summary>
	public class Parser
	{

		private static readonly Regex StyleSelectorRegex = new Regex(
			"\\G(\\s*" +               // any leading spaces
            "(?(selectors){\\s*)" +    // if selector was already matched, 
                                       // match a { and spaces
			"(?<selectors>[^{]+?)" +   // match stylename - chars up to the {
			"\\s*{\\s*" +              // spaces, then the colon, then more spaces
			"(?<csstext>[^}]+)" +      // now match styleval till closing bracket
			")*\\s*}?\\s*",            // match a trailing } and trailing spaces
			RegexOptions.Singleline | 
			RegexOptions.Multiline);
	
		private static readonly Regex StyleAttribRegex = new Regex(
			"\\G(\\s*" +                // any leading spaces
			"(?(stylename);\\s*)" +     // if stylename was already matched, 
										// match a semicolon and spaces
			"(?<stylename>[^:]+?)" +    // match stylename - chars up to the semicolon
			"\\s*:\\s*" +               // spaces, then the colon, then more spaces
			"(?<styleval>[^;]+?)" +     // now match styleval
			")*\\s*;?\\s*$",            // match a trailing semicolon and trailing spaces
			RegexOptions.Singleline | 
			RegexOptions.Multiline);

        private static readonly Regex rxGroups = new Regex("(.*)\\s+(.*)\\s+(.*)");

		private StyleObject so;
        private ArrayList styles;

		/// <summary>
		/// Parses the given string and fires the selector event handler.
		/// </summary>
		/// <param name="main">Main class</param>
		/// <param name="source">Styles to parse</param>
		public void ParseStylesheet(CssParser main, string source)
		{
			if (source != null)
			{
                styles = new ArrayList();
				Match match;
				if ((match = StyleSelectorRegex.Match(source, 0)).Success) 
				{
					do
					{
						CaptureCollection selectors = match.Groups["selectors"].Captures;
						CaptureCollection cssText = match.Groups["csstext"].Captures;

						for (int i = 0; i < selectors.Count; i++) 
						{
							so = new StyleObject();
							GetStyleObject(cssText[i].ToString());	// create StyleObject from { content }
							String sn = selectors[i].ToString();
							SelectorType st = GetSelectorType(selectors[i].ToString());
//							// remove leading signs from names
//							switch (st)
//							{
//								case SelectorType.CLASS_SELECTOR:
//								case SelectorType.ID_SELECTOR:
//									sn = sn.Substring(1);
//									break;
//							}
							so.SelectorName = sn;
							so.SelectorType = st;
                            styles.Add(so);
							main.OnSelectorReady(so);
						}
						match = match.NextMatch();
					} while (match.Success);
				}
			}
		}

        internal IList ParsedStyles
        {
            get { return styles; }
        }

		private string ReplaceComments(string source)
		{
			return source;
		}

		private void GetStyleObject(string cssText)
		{
			if (cssText != null)
			{
				Match match;
				if ((match = StyleAttribRegex.Match(cssText, 0)).Success) 
				{
					CaptureCollection stylenames = match.Groups["stylename"].Captures;
					CaptureCollection stylevalues = match.Groups["styleval"].Captures;

					for (int i = 0; i < stylenames.Count; i++) 
					{
						String styleName = stylenames[i].ToString();
						String styleValue = stylevalues[i].ToString();
						StyleType st = GetStyleType(styleName, styleValue);
						// do not add style already exists (ignore furthermore ones)
						if (so.ContainsKey(styleName)) continue;
						switch (st)
						{
							case StyleType.Color:
								so.Add(styleName, GetStyleColor(styleName, styleValue));
								break;
							case StyleType.List:
								so.Add(styleName, GetStyleList(styleName, styleValue));
								break;
							case StyleType.Property:
								so.Add(styleName, GetStyleProperty(styleName, styleValue));
								break;
							case StyleType.Unit:
								so.Add(styleName, GetStyleUnit(styleName, styleValue));
								break;
							case StyleType.Group:
								so.Add(styleName, GetStyleGroup(styleName, styleValue));
								break;
						}
					}
				}
			} // end if
		} // end method

		private StyleType GetStyleType(string StyleName, string cssAttribute)
		{
			// TODO: return fixed type for elements which have only one type of style, then check for others
			switch (StyleName.ToLower())
			{
				case "background-image":
				case "list-style-image":
					return StyleType.Property;
				case "border-color":
				case "border-left-color":
				case "border-right-color":
				case "border-top-color":
				case "border-bottom-color":
                case "color":
                case "background-color":
					return StyleType.Color;
				default:
					cssAttribute = cssAttribute.Trim();
					if (cssAttribute.IndexOf(',') != -1)
					{
						// comma sperated is a list
						return StyleType.List;
					} 
					else if (cssAttribute.IndexOf(' ') != -1)
					{
						// contains a whitespace, maybe list or group
						if (Util.ExtractNumberString(cssAttribute) != String.Empty)
						{
							// contains a number, so it must be a group
							return StyleType.Group;
						} 
						else 
						{
							return StyleType.List;
						}
					} 
					else 
					{
						// property, color or unit; first, check for color ( #XXXXXX or rgb(0,0,0) )
						if (cssAttribute.IndexOf('#') == 0
                            ||
                            cssAttribute.StartsWith("rgb")
                            )
						{
							return StyleType.Color;
						}
						Color c = Color.FromName(cssAttribute);
						if (c.A == 0 && c.R == 0 && c.G == 0 && c.B == 0)
						{
							// no color, must be property or unit
							if (Util.ExtractNumberString(cssAttribute) == String.Empty)
							{
								// no number, so it must be a property
								return StyleType.Property;
							} 
							else 
							{
								// with number it can be a unit or a simple number
								if (cssAttribute.Length > Util.ExtractNumberString(cssAttribute).Length)
								{
									// more than a number, must be a unit
									return StyleType.Unit;                            
								} 
								else 
								{
									// only the number, use property to avoid searching for units
									return StyleType.Property;
								}
							}
						} 
						else 
						{
							// known color name
							return StyleType.Color;
						}
					}
			}
		}

		private StyleColor GetStyleColor(string styleName, string val)
		{
			StyleColor sc = new StyleColor();
            Regex rx = new Regex(@"rgb(\s*\d{1,3}\s*,\s*\d{1,3}\s*,\s*\d{1,3}\s*)");
            Match m;
            if ((m = rx.Match(val)).Success)
            {
                val = String.Format("{0:X2}{1:X2}:{2:X2}", m.Groups[1], m.Groups[2], m.Groups[3]);
            }
			sc.Name = val;
			return sc;
		}

		private StyleList GetStyleList(string styleName, string val)
		{
			StyleList sl = new StyleList();
			string[] arr = val.Split(new char[] {';', ','});
			foreach(string element in arr)
			{
				if (element.Trim().Length > 0)
				{
					sl.Add(element);
				}
			}
			return sl;
		}

		private StyleProperty GetStyleProperty(string styleName, string val)
		{
			StyleProperty sp;
			if (styleName.ToLower().IndexOf("image") > 0 && val.Trim().ToLower().IndexOf("url") == 0)
			{
				// remove url() from val; internally we store only the real URL
				Regex rx = new Regex(@"url\s*\(\s*(.*)\s*\)");
				Match m;
				if ((m = rx.Match(val)).Success)
				{
					sp = new StyleProperty(m.Groups[1].Value);
				} 
				else 
				{
					sp = new StyleProperty(val);
				}
			} 
			else 
			{
				sp = new StyleProperty(val);
			}
			return sp;
		}

		private StyleUnit GetStyleUnit(string styleName, string val)
		{
			string numString = Util.ExtractNumberString(val);
			float num = Single.Parse(numString);
			string unit = val.Substring(numString.Length);
			StyleUnit su = new StyleUnit(num, unit);
			return su;
		}

		private StyleGroup GetStyleGroup(string styleName, string val)
		{
			StyleGroup sg = new StyleGroup();
			Match m;
			if ((m = rxGroups.Match(val)).Success)
			{
				for (int i = 0; i < m.Groups.Count; i++)
				{
					StyleType st = GetStyleType(styleName, m.Groups[i].Value);
					switch (st)
					{
						case StyleType.Color:
                            sg.Color = GetStyleColor(styleName, m.Groups[i].Value);
							break;
						case StyleType.Property:
                            // in groups properties will always appear as lists
                            sg.List = GetStyleList(styleName, m.Groups[i].Value);
                            break;
						case StyleType.Unit:
                            sg.Unit = GetStyleUnit(styleName, m.Groups[i].Value);
							break;
					}
				}
			}
			return sg;
		}

		private SelectorType GetSelectorType(string selector)
		{
			selector = selector.Trim().ToLower();
			if (selector[0] == '#')
			{
				return SelectorType.ID_SELECTOR;
			}
            if (selector[0] == '.')
			{
				return SelectorType.CLASS_SELECTOR;
			}
			if (selector.IndexOf('>') != -1)
			{
				return SelectorType.CHILD_SELECTOR;
			}
			if (selector.IndexOf(',') != -1)
			{
				return SelectorType.ANY_NODE_SELECTOR;
			}
			if (selector.IndexOf("a:") != -1)
			{
				return SelectorType.PSEUDO_CLASS_SELECTOR;
			}
			if (selector.IndexOf(':') != -1)
			{
				return SelectorType.PSEUDO_ELEMENT_SELECTOR;
			}
            if (selector.IndexOf('[') > 0)
            {
                return SelectorType.ATTRIBUTE_SELECTOR;
            }
            if (selector.IndexOf(' ') > 0)
			{
				return SelectorType.DESCENDANT_SELECTOR;
			}
			return SelectorType.ELEMENT_NODE_SELECTOR;
		}
	}
}
