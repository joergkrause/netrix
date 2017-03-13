using System;
using GuruComponents.Netrix.ComInterop;

namespace GuruComponents.Netrix.Events
{
    /// <summary>
    /// Event arguments for post editor events.
    /// </summary>
	public class PostEditorEventArgs : EventArgs
	{
		Interop.IHTMLEventObj e;
        EventObject eo;

        /// <summary>
        /// Internally used ctor. DO NOT USE IN USER CODE.
        /// </summary>
        /// <param name="e"></param>
		public PostEditorEventArgs(Interop.IHTMLEventObj e)
		{
			this.e = e;
		}

        /// <summary>
        /// Returns the native event object.
        /// </summary>
		public EventObject EventObject
		{
			get
			{
                if (eo == null)
                {
                    eo = new EventObject(e);
                }
				return eo;
			}
		}

	}
}
