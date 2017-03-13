using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace GuruComponents.Netrix.UserInterface.ColorPicker
{
    internal class ColorComparer
    {

        private static DistanceComparer dc;

        private static NameComparer nc;

        private static SaturationComparer sc;

        private static HueComparer hc;

        private static BrightnessComparer bc;

        private static UnsortedComparer uc;

        static ColorComparer()
        {
            dc = new DistanceComparer();

            nc = new NameComparer();

            sc = new SaturationComparer();

            hc = new HueComparer();

            bc = new BrightnessComparer();

            uc = new UnsortedComparer();
        }

        /// <summary>
        /// Returns an new instance of a class used to sort the color table.
        /// </summary>
        /// <returns>IComparer</returns>
        public static IComparer CompareColorDistance()
        {
            return dc;
        }

        /// <summary>
        /// Returns an new instance of a class used to sort the color table.
        /// </summary>
        /// <returns>IComparer</returns>
        public static IComparer CompareColorName()
        {
            return nc;
        }

        /// <summary>
        /// Returns an new instance of a class used to sort the color table.
        /// </summary>
        /// <returns>IComparer</returns>
        public static IComparer CompareColorSaturation()
        {
            return sc;
        }

        /// <summary>
        /// Returns an new instance of a class used to sort the color table.
        /// </summary>
        /// <returns>IComparer</returns>
        public static IComparer CompareColorHue()
        {
            return hc;
        }

        /// <summary>
        /// Returns an new instance of a class used to sort the color table.
        /// </summary>
        /// <returns>IComparer</returns>
        public static IComparer CompareColorBrightness()
        {
            return bc;
        }

        /// <summary>
        /// Returns an new instance of a class used to sort the color table.
        /// </summary>
        /// <returns>IComparer</returns>
        public static IComparer CompareUnsorted()
        {
            return uc;
        }

        private class DistanceComparer : IComparer
        {
            public int Compare(object a, object b)
            {
                ColorWellInfo _a = (ColorWellInfo)a;
                ColorWellInfo _b = (ColorWellInfo)b;

                return _a.Distance.CompareTo(_b.Distance);
            }
        }

        private class NameComparer : IComparer
        {
            public int Compare(object a, object b)
            {
                ColorWellInfo _a = (ColorWellInfo)a;
                ColorWellInfo _b = (ColorWellInfo)b;

                return _a.Color.Name.CompareTo(_b.Color.Name);
            }
        }

        private class SaturationComparer : IComparer
        {
            public int Compare(object a, object b)
            {
                ColorWellInfo _a = (ColorWellInfo)a;
                ColorWellInfo _b = (ColorWellInfo)b;

                return _a.Color.GetSaturation().CompareTo(_b.Color.GetSaturation());
            }
        }

        private class HueComparer : IComparer
        {
            public int Compare(object a, object b)
            {
                ColorWellInfo _a = (ColorWellInfo)a;
                ColorWellInfo _b = (ColorWellInfo)b;

                return _a.Color.GetHue().CompareTo(_b.Color.GetHue());
            }
        }

        private class BrightnessComparer : IComparer
        {
            public int Compare(object a, object b)
            {
                ColorWellInfo _a = (ColorWellInfo)a;
                ColorWellInfo _b = (ColorWellInfo)b;

                return _a.Color.GetBrightness().CompareTo(_b.Color.GetBrightness());
            }
        }

        private class UnsortedComparer : IComparer
        {
            public int Compare(object a, object b)
            {
                ColorWellInfo _a = (ColorWellInfo)a;
                ColorWellInfo _b = (ColorWellInfo)b;
                return _a.UnsortedIndex.CompareTo(_b.UnsortedIndex);
            }
        }

    }
}