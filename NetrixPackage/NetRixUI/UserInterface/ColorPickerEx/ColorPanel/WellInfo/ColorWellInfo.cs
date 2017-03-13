using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;

namespace GuruComponents.Netrix.UserInterface.ColorPicker
{
    /// <summary>
    /// This class is used to hold the information about each color in the collection. The enhances the
    /// color store with sorting capabilites.
    /// </summary>
    internal class ColorWellInfo
    {
        private int unsorted_index;
        private string name;

        public int UnsortedIndex
        {
            get { return unsorted_index; }
        }
        private long distance;

        public long Distance
        {
            get { return distance; }
        }
        public readonly System.Drawing.Color Color;
        
        private Rectangle colorPosition;

        public Rectangle ColorPosition
        {
            get { return colorPosition; }
            set { colorPosition = value; }
        }

        public string ColorName
        {
            get
            {
                return name;
            }
        }

        public ColorWellInfo(Color color, int unsorted_index)
        {
            this.Color = color;
            distance = color.R * color.R + color.B * color.B + color.G * color.G;
            this.unsorted_index = unsorted_index;
            Color cn = Color.FromArgb(0, this.Color);
            if (cn.IsNamedColor || cn.IsKnownColor || cn.IsSystemColor)
            {
                name = cn.Name;
            }
            else
            {
                name = String.Format("#{0:X2}{1:X2}{2:X2}", cn.R, cn.G, cn.B);
            }
        }

    }
}
