using System;

namespace GuruComponents.Netrix.XmlDesigner.Edx
{

    /// <summary>
    /// Holds one option from options attribute.
    /// </summary>
	public class Option 
	{

        private string name;

        /// <summary>
        /// Option's name.
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        private string val;

        /// <summary>
        /// Option's value.
        /// </summary>
        public string Value
        {
            get { return val; }
            set { val = value; }
        }

		/// <summary>
		/// Constructor for micro-option class.
		/// </summary>
		/// <param name="n"></param>
		/// <param name="v"></param>
		public Option(string n, string v)
		{
			this.name = n;
			this.val = v;
		}
	}

}
