using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;

namespace GuruComponents.Netrix.UserInterface.ColorPicker
{
    internal class ColorWellHelper
    {

        private ColorWellHelper()
        {
        }

        /// <summary>
        /// Generate an array of ColorWellInfo from the supplied array of Color.
        /// </summary>
        /// <param name="customColors"></param>
        /// <param name="colorSortOrder"></param>
        /// <returns></returns>
        public static ColorWellInfo[] GetCustomColorWells(List<Color> customColors, ColorSortOrder colorSortOrder)
        {
            int nColors = customColors.Count;
            ColorWellInfo[] colorWells = new ColorWellInfo[nColors];
            for (int i = 0; i < customColors.Count; i++)
            {
                colorWells[i] = new ColorWellInfo((Color)customColors[i], i);
            }
            SortColorWells(colorWells, colorSortOrder);
            return colorWells;
        }

        /// <summary>
        /// This method return an array of colorWells that belong to the desired ColorSet and 
        /// that have been sorted in the desired ColorSortOrder.
        /// </summary>
        /// <param name="colorSet">The color palette to be generated.</param>
        /// <param name="colorSortOrder">The order the generated palette should be sorted.</param>
        /// <returns></returns>
        public static ColorWellInfo[] GetColorWells(ColorSet colorSet, ColorSortOrder colorSortOrder)
        {
            // get array of desired colorWells and sort
            // Could have sort order enum/property
            Array knownColors = Enum.GetValues(typeof(System.Drawing.KnownColor));

            int nColors = 0;

            // How many colors are there?
            switch (colorSet)
            {
                case ColorSet.Web:
                case ColorSet.Continues:
                    nColors = 216;
                    break;
                case ColorSet.System:
                    foreach (KnownColor k in knownColors)
                    {
                        Color c = Color.FromKnownColor(k);
                        if (c.IsSystemColor && (c.A > 0))
                        {
                            nColors++;
                        }
                    }
                    break;
            }

            ColorWellInfo[] colorWells = new ColorWellInfo[nColors];

            int index = 0;
            int r = 0, g = 0, b = 0, i = 0;
            // Get the colors
            switch (colorSet)
            {
                case ColorSet.Continues:
                    int[] ContR = new int[] { 0xCC, 0x66, 0x00, 0xFF, 0x99, 0x33 };
                    int[] ContG = new int[] { 0xFF, 0xCC, 0x99, 0x66, 0x33, 0x00, 0x00, 0x33, 0x66, 0x99, 0xCC, 0xFF };
                    int[] ContB = new int[] { 0xFF, 0xCC, 0x99, 0x66, 0x33, 0x00, 0x00, 0x33, 0x66, 0x99, 0xCC, 0xFF, 0xFF, 0xCC, 0x99, 0x66, 0x33, 0x00 };
                    for (int y = 0; y < 12; y++)
                    {
                        g = ContG[y];
                        for (int x = 0; x < 18; x++)
                        {
                            r = ContR[(x / 6) + ((y < 6) ? 0 : 3)];
                            b = ContB[x];
                            colorWells[i++] = new ColorWellInfo(Color.FromArgb(r, g, b), i);
                        }
                    }
                    break;
                case ColorSet.Web:
                    int[] WebSafe = new int[] { 0x00, 0x33, 0x66, 0x99, 0xCC, 0xFF };
                    for (int y = 0; y < 12; y++)
                    {
                        b = WebSafe[y % 6];
                        for (int x = 0; x < 18; x++)
                        {
                            g = WebSafe[x % 6];
                            r = (y < 6) ? WebSafe[x / 6] : WebSafe[(x / 6) + 3];
                            colorWells[i++] = new ColorWellInfo(Color.FromArgb(r, g, b), i);
                        }
                    }
                    break;
                case ColorSet.System:
                    foreach (KnownColor k in knownColors)
                    {
                        Color c = Color.FromKnownColor(k);

                        if (c.IsSystemColor && (c.A > 0))
                        {
                            colorWells[index] = new ColorWellInfo(c, index);
                            index++;
                        }
                    }
                    break;
            }
            SortColorWells(colorWells, colorSortOrder);
            return colorWells;
        }

        /// <summary>
        /// Sort the supplied colorWells according to the required sort order.
        /// </summary>
        /// <param name="colorWells"></param>
        /// <param name="colorSortOrder"></param>
        public static void SortColorWells(ColorWellInfo[] colorWells, ColorSortOrder colorSortOrder)
        {
            // Sort them
            switch (colorSortOrder)
            {
                case ColorSortOrder.Brightness:
                    Array.Sort(colorWells, ColorComparer.CompareColorBrightness());
                    break;
                case ColorSortOrder.Distance:
                    Array.Sort(colorWells, ColorComparer.CompareColorDistance());
                    break;
                case ColorSortOrder.Continues:
                    Array.Sort(colorWells, ColorComparer.CompareUnsorted());
                    break;
                case ColorSortOrder.Name:
                    Array.Sort(colorWells, ColorComparer.CompareColorName());
                    break;
                case ColorSortOrder.Saturation:
                    Array.Sort(colorWells, ColorComparer.CompareColorSaturation());
                    break;
                case ColorSortOrder.Unsorted:
                    Array.Sort(colorWells, ColorComparer.CompareUnsorted());
                    break;
            }
        }


        /// <summary>
        /// Draws the ColorWell on the Graphics surface.
        /// </summary>
        /// <remarks>
        /// This method draws the ColorWell as either enabled or disabled.
        /// It also indicates the currently selected color and the color
        /// that is ready to chosed (picked) by the mouse or keyboard.
        /// </remarks>
        /// <param name="g">Graphics object.</param>
        /// <param name="enabled">Enable appearance</param>
        /// <param name="selected">Selected appearance.</param>
        /// <param name="pickColor">Color used.</param>
        /// <param name="cwi">ColorWellInfo object.</param>
        public static void DrawColorWell(System.Drawing.Graphics g, bool enabled, bool selected, bool pickColor, ColorWellInfo cwi)
        {
            Rectangle r = cwi.ColorPosition;

            if (!enabled)
            {
                r.Inflate(-SystemInformation.BorderSize.Width, -SystemInformation.BorderSize.Height);
                ControlPaint.DrawBorder3D(g, r, Border3DStyle.Flat);
                r.Inflate(-SystemInformation.BorderSize.Width, -SystemInformation.BorderSize.Height);
                g.FillRectangle(SystemBrushes.Control, r);
            }
            else
            {
                SolidBrush br = new SolidBrush(cwi.Color);

                if (pickColor)
                {
                    ControlPaint.DrawBorder3D(g, r, Border3DStyle.Sunken);
                    r.Inflate(-SystemInformation.Border3DSize.Width, -SystemInformation.Border3DSize.Height);
                    g.FillRectangle(br, r);
                }
                else
                {
                    if (selected)
                    {
                        ControlPaint.DrawBorder3D(g, r, Border3DStyle.Raised);
                        r.Inflate(-SystemInformation.Border3DSize.Width, -SystemInformation.Border3DSize.Height);
                        g.FillRectangle(br, r);
                    }
                    else
                    {
                        g.FillRectangle(SystemBrushes.Control, r);
                        r.Inflate(-SystemInformation.BorderSize.Width, -SystemInformation.BorderSize.Height);
                        ControlPaint.DrawBorder3D(g, r, Border3DStyle.Flat);
                        r.Inflate(-SystemInformation.BorderSize.Width, -SystemInformation.BorderSize.Height);
                        g.FillRectangle(br, r);
                    }
                }

                br.Dispose();
                br = null;
            }
        }

    }
}