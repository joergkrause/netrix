using System;
using System.Collections;
using System.Text;

namespace GuruComponents.Netrix.UserInterface.StyleParser
{
	/// <summary>
	/// StyleObject contains a complete list of styles assigned to one selector.
	/// </summary>
	/// <remarks>
	/// The class uses
	/// the case insensitive version of a hastable to recognize the style names in a more robust way.
	/// </remarks>
    [Serializable()]
    public class StyleObject
	{
		private string _prop;
		private string _name;
		private SelectorType _type;

        private Hashtable baseTable;

        /// <summary>
        /// The constructor used to create a new style object.
        /// </summary>
        /// <remarks>
        /// This class uses a case insensitive <see cref="System.Collections.Hashtable">Hashtable</see> to 
        /// simplify the access to the keys.
        /// </remarks>
		public StyleObject()
		{
            baseTable = System.Collections.Specialized.CollectionsUtil.CreateCaseInsensitiveHashtable(5);
		}

        /// <summary>
        /// String representation of styles.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (DictionaryEntry de in baseTable)
            {
                sb.AppendFormat("{0}:{1};", de.Key, de.Value); // ToString is overriden!
            }
            return sb.ToString();
        }

        /// <summary>
        /// Checks if the style name already exists.
        /// </summary>
        /// <param name="Key">The style name.</param>
        /// <returns>Returns <c>true</c> on success.</returns>
        public bool ContainsKey(object Key)
        {
            return baseTable.ContainsKey(Key);
        }

        /// <summary>
        /// The name of the selector (rule).
        /// </summary>
		public string SelectorName
		{
			get
			{	
				return _name;
			}
			set
			{
				_name = value;
			}
		}
        /// <summary>
        /// The type of the selector (rule).
        /// </summary>
		public SelectorType SelectorType
		{
			get
			{	
				return _type;
			}
			set
			{
				_type = value;
			}
		}
        /// <summary>
        /// Adds a property to the collection.
        /// </summary>
        /// <param name="property">The name of the property.</param>
        /// <param name="o">The property value.</param>
		public void Add (string property, StyleProperty o)
		{
			baseTable.Add(property, o);
		}
        /// <summary>
        /// Adds a color to the collection.
        /// </summary>
        /// <param name="property">The name of the property.</param>
        /// <param name="o">The color value.</param>
        public void Add (string property, StyleColor o)
		{
			baseTable.Add(property, o);
		}
        /// <summary>
        /// Adds a unit to the collection.
        /// </summary>
        /// <param name="property">The name of the property.</param>
        /// <param name="o">The unit value.</param>
        public void Add (string property, StyleUnit o)
		{
			baseTable.Add(property, o);
		}
        /// <summary>
        /// Adds a list to the collection.
        /// </summary>
        /// <param name="property">The name of the property.</param>
        /// <param name="o">The list value.</param>
        public void Add (string property, StyleList o)
		{
			baseTable.Add(property, o);
		}
        /// <summary>
        /// Adds a group to the collection.
        /// </summary>
        /// <param name="property">The name of the property.</param>
        /// <param name="o">The group value.</param>
        public void Add (string property, StyleGroup o)
        {
            baseTable.Add(property, o);
        }

        /// <summary>
        /// Add a new style object.
        /// </summary>
        /// <param name="o">The value.</param>
		public void Add(object o)
		{
			if (_prop == null)
			{
				throw new ArgumentException("No property given to set new style object");
			} 
			else 
			{
				baseTable.Add(_prop, o);
			}
		}
        /// <summary>
        /// Sets a default property which the next value is added to.
        /// </summary>
		public string Property
		{
			set
			{
				_prop = value;
			}
		}
        /// <summary>
        /// The collection of style rules.
        /// </summary>
		public Hashtable Styles
		{
			get
			{	
				return baseTable;
			}
			set
			{
                baseTable = value;
			}
		}
	}
}
