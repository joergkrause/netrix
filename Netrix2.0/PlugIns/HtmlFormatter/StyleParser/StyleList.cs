using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace GuruComponents.Netrix.UserInterface.StyleParser
{
	/// <summary>
	/// StyleList contains one or more string values, e.g. "Arial Verdana.."
	/// </summary>
    [Serializable()]
    public class StyleList : StringCollection 
	{
	    /// <summary>
        /// Gets the joined list (string list) of the list elements. Read only.
        /// </summary>
		public string JoinedList
		{
			get
			{
				string[] list = new string[base.Count];
				base.CopyTo(list, 0);
				return String.Join(",", list);
			}
		}

        /// <summary>
        /// String representation of list.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return JoinedList;
        }

        /// <summary>
        /// The styles as fixed lenght array.
        /// </summary>
        /// <returns>The styles as fixed lenght array.</returns>
        public string[] ToArray()
        {
            string[] arr = new string[base.Count];
            base.CopyTo(arr, 0);
            return arr;
        }

	}
}
