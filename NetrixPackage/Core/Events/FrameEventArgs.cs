using System;
using GuruComponents.Netrix.WebEditing.Documents;

namespace GuruComponents.Netrix.Events
{

    /// <summary>
    /// This class is used to build the event args for the frame activated, frame ready state complete and
    /// frame context menu event.
    /// </summary>
    /// <remarks>
    /// <para>
    /// These events
    /// are fired if the user switches with the mouse from one active (and editable) frame to another.
    /// The final frame will fire the event and the event args allow access to the frame window object.
    /// </para>
    /// <para>
    /// The object <see cref="IFrameWindow">FrameWindow</see> is a shallow copy clone of the original object. Thus, it can be used to change
    /// the properties of the underlying frame but delete/dispose operation will not reach the original object.
    /// </para>
    /// </remarks>
	public class FrameEventArgs : EventArgs
	{

        private IFrameWindow fw;
        
        /// <summary>
        /// The constructor used to build the frame event arguments.
        /// </summary>
        /// <param name="fw"></param>
		public FrameEventArgs(IFrameWindow fw)
		{
            this.fw = fw;
		}

        /// <summary>
        /// Gets the frame window from which the event was fired.
        /// </summary>
        /// <remarks>
        /// See <see cref="IFrameWindow"/> for
        /// more information about dealing with frames.
        /// </remarks>
        public IFrameWindow FrameWindow
        {
            get
            {
                return fw;
            }
        }
	}
}