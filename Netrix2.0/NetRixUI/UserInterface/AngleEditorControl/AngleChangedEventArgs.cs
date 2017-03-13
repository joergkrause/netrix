using System;
using System.Collections.Generic;
using System.Text;

namespace GuruComponents.Netrix.UserInterface
{
    /// <summary>
    /// Event arguments for AngleChanged event.
    /// </summary>
    public class AngleChangedEventArgs : EventArgs
    {
        private int angle;

        /// <summary>
        /// Ctor, internally used.
        /// </summary>
        /// <param name="angle"></param>
        public AngleChangedEventArgs(int angle)
        {
            this.angle = angle;
        }

        /// <summary>
        /// Current angle. Set to change immediately (does not refire event).
        /// </summary>
        public int Angle
        {
            get { return angle; }
            set { angle = value; }
        }


    }
}
