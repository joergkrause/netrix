using System;
using System.Text.RegularExpressions;
using System.Web.RegularExpressions;

namespace GuruComponents.Netrix.Designer
{

    /// <summary>
    /// A class that handels the @Control directive.
    /// </summary>
    public sealed class ControlDirective : PageDirective
    {
        /// <summary>
        /// Name of directive. Always returns "Control".
        /// </summary>
        public override string DirectiveName
        {
            get
            {
                return "Control";
            }
        }

    }
}