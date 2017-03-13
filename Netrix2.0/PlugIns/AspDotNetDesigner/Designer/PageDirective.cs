using System;
using System.Text.RegularExpressions;
using System.Web.RegularExpressions;

namespace GuruComponents.Netrix.Designer
{

    /// <summary>
    /// A class that handels the @Page directive
    /// </summary>
    public class PageDirective : Directive
    {
        /// <summary>
        /// Returns always 'Page'.
        /// </summary>
        public override string DirectiveName
        {
            get
            {
                return "Page";
            }
        }


        /// <summary>
        /// Create a directive object from string.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"/>
        /// <param name="cc">Regex</param>
        /// <returns>Directive object, or throws an exception if format is invalid.</returns>
        internal static Directive GetDirectiveFromString(CaptureCollection cc)
        {
            if (cc.Count > 0)
            {
                return new PageDirective(cc);
            }
            else
            {
                throw new ArgumentOutOfRangeException("text", "Page Directive string has not the expected format.");
            }
        }

        /// <summary>
        /// ctor
        /// </summary>
        protected PageDirective()
        {
        }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="cc"></param>
        public PageDirective(CaptureCollection cc)
        {
            foreach (Capture c in cc)
            {
                string[] pairs = c.Value.Split(new String[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
                if (pairs.Length != 2) continue;
                string val = pairs[1];
                if (val.StartsWith(@"""")) val = val.Substring(1);
                if (val.EndsWith(@"""")) val = val.Substring(0, val.Length - 1);
                val = val.Trim();
                string key = pairs[0].Trim().ToLower();
                base.Dictionary.Add(key, val);
            }
        }

    }
}