using System;
using System.Drawing;

namespace GuruComponents.Netrix.HelpLine.Events
{
	/// <summary>
	/// HelplineMovedEventArgs used on HelpLineMoved event. </summary><remarks>This informs
	/// the handler about the final position the helpline has reached.
	/// </remarks>
	public class HelplineMovedEventArgs : EventArgs
	{

        private Point _fp;

        /// <summary>
        /// The Point where the final position of the helpline is.
        /// </summary>
		public Point FinalPosition
        {
            get
            {
                return _fp;
            }
        }
        /// <summary>
        /// The X coordinate of the Point.
        /// </summary>
        public int X
        {
            get
            {
                return _fp.X;
            }
        }
        /// <summary>
        /// The Y coordinate of the Point.
        /// </summary>
		public int Y
        {
            get
            {
                return _fp.Y;
            }
        }

		internal HelplineMovedEventArgs(Point xy)
		{
			_fp = xy;
		}
	}
}
