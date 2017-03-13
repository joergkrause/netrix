using System.Collections;
using System.Drawing;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Threading;
using System.Collections.Generic;
using GuruComponents.Netrix.UserInterface.ColorPicker;

namespace GuruComponents.Netrix.UserInterface
{
	/// <summary>
	/// This class controls the resource manager of all UI resources. 
	/// </summary>
	/// <remarks>
    /// The class contains static methods and properties to give other consumers direct access to the recource
    /// management. Resources are global and apply to all instances of NetRix HtmlEditor.
    /// </remarks>
	public class ResourceManager
	{

        internal enum GridFilterType
        {
            Standard,
            Events,
            Document
        }

        /// <summary>
        /// Used to determine the type of attribute names used in a propertygrid.
        /// </summary>
        public enum GridLanguageType
        {
            /// <summary>
            /// The standard names (HTML) are used.
            /// </summary>
            Standard,
            /// <summary>
            /// The localized names (from satellite assemblies) are used.
            /// </summary>
            Localized
        }

        /// <summary>
        /// Controls the language for properties in the grid. Set by Toolbar.
        /// </summary>
        private static GridLanguageType GridLanguage;
        /// <summary>
        /// Controls the filter for properties in the grid. Set by Toolbar.
        /// </summary>
        private static GridFilterType GridFilter;
        /// <summary>
        /// A list of colors which is used to fill the custom tab of each color selector control.
        /// </summary>
        public static List<Color> _customColors = null;
        /// <summary>
        /// The internal store of the resource manager.
        /// </summary>
		private static System.Resources.ResourceManager _resources = null;
        /// <summary>
        /// The current culture
        /// </summary>
		private static CultureInfo currentCulture = Thread.CurrentThread.CurrentUICulture;
        /// <summary>
        /// The basename for the satellite assemblies namespace.
        /// </summary>
        private static string baseName = "GuruComponents.Netrix.UserInterface.Resources.Resx.Resource"; // const

        internal static object HtmlDocument;

        /// <summary>
        /// Used to force the initialization from other assemblies. If not used the initialization will start before
        /// the first call of GetString has finished.
        /// </summary>
		private static void Initialize()
		{
			if (_resources == null)
			{
				_resources = Resources;
			}
        }

        /// <summary>
        /// Initialize the resource manager with associated document property.
        /// </summary>
        /// <param name="htmlDocument">A relation to the active HTML document.</param>
        public static void Initialize(object htmlDocument)
        {
            Initialize();
            HtmlDocument = htmlDocument;            
        }

        /// <summary>
        /// Initialize the resource manager with associated document property.
        /// </summary>
        /// <param name="htmlDocument">A relation to the active HTML document.</param>
        /// <param name="Culture">The culture which is actually beeing set. Should be the current UI culture.</param>
        public static void Initialize(object htmlDocument, string Culture)
        {
            Initialize();
            HtmlDocument = htmlDocument;
            currentCulture = new CultureInfo(Culture);
        }

		/// <summary>
		/// Initialize the resource manager with associated document property.
		/// </summary>
        /// <param name="htmlDocument">A relation to the active HTML document.</param>
        /// <param name="Culture">The culture which is actually beeing set. Should be the current UI culture.</param>
        /// <param name="CustomColorsArray">The array of custom colors.</param>		
        public static void Initialize(object htmlDocument, string Culture, List<Color> CustomColorsArray)
		{
			Initialize();
			HtmlDocument = htmlDocument;
            currentCulture = new CultureInfo(Culture);
            _customColors = CustomColorsArray;
		}

        /// <summary>
        /// Set the filter for the property descriptor.
        /// </summary>
        /// <param name="type"></param>
        internal static void SetGridFilter(GridFilterType type)
        {
            GridFilter = type;
        }

        /// <summary>
        /// Get the filter the descriptor should use. Called from PropertyDescriptor.
        /// </summary>
        /// <returns></returns>
        internal static GridFilterType GetGridFilter()
        {
            return GridFilter;
        }

        /// <summary>
        /// Set the language for the property descriptor.
        /// </summary>
        /// <param name="type"></param>
        public static void SetGridLanguage(GridLanguageType type)
        {
            GridLanguage = type;
        }

        /// <summary>
        /// Get the language the descriptor should use. 
        /// </summary>
        /// <remarks>Called from PropertyDescriptor.</remarks>
        /// <returns>Type of Language for the PropertyGrid.</returns>
        public static GridLanguageType GetGridLanguage()
        {
            return GridLanguage;
        }

        /// <summary>
        /// Sets the current culture form the host application.
        /// </summary>
		public static CultureInfo CurrentCulture
		{
			set
			{
				currentCulture = value;
			}
		}

        /// <summary>
        /// Retrieves a string from the embedded resource or the satellites, if any. 
        /// </summary>
        /// <remarks>Uses the two
        /// predefines sets is the current culture is ether english or german.</remarks>
        /// <param name="res"></param>
        /// <returns></returns>
        public static string GetString(string res)
        {
            // faster access for standard languages through preloaded sets
            try
            {
                // use standard fallback to recognize other cultures
                return Resources.GetString(res, currentCulture);
            }
            catch
            {
                return res;
            }
        }

        /// <summary>
        /// Gets the resource manager. 
        /// </summary>
        /// <remarks>If no resource manager is defined the property will create one.</remarks>
		public static System.Resources.ResourceManager Resources
		{
			get
			{
                if (_resources == null)
                {                    
                    _resources = new System.Resources.ResourceManager(baseName, Assembly.GetExecutingAssembly());
                    _resources.IgnoreCase = true;
                }
				return _resources;
			}
		}

        /// <summary>
        /// Gets or sets the custom colors which uses the ColorPicker control to set the custom tab.
        /// </summary>
        public static List<Color> CustomColors
		{
			get
			{
				if (_customColors == null)
				{
					_customColors = ColorPanel.DefaultCustomColors();
				}
				return _customColors;
			}
			set
			{
				_customColors = value;
			}
		}

        private static bool colorPickerButtonSystemVisible = true;

        /// <summary>
        /// Makes the 'system' color button systemwide visible or invisible.
        /// </summary>
        public static bool ColorPickerButtonSystemVisible
        {
            get { return colorPickerButtonSystemVisible; }
            set { colorPickerButtonSystemVisible = value; }
        }

	}
}
