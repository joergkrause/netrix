using System;
using GuruComponents.Netrix.ComInterop;

namespace GuruComponents.Netrix.WebEditing.Behaviors
{
    /// <summary>
    /// Event arguments for BehaviorNotify event.
    /// </summary>
	public class BehaviorNotifyEventArgs : EventArgs
	{
		private BehaviorNotifyType type;
		private Interop.IElementBehaviorSite site;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="t"></param>
        /// <param name="site"></param>
		public BehaviorNotifyEventArgs(BehaviorNotifyType t, Interop.IElementBehaviorSite site)
		{
			this.site = site;
			this.type = t;
		}
		
		/// <summary>
		/// Returns the event type which causes the notification.
		/// </summary>
		public BehaviorNotifyType NotifyType
		{
			get { return type; }
		}

		/// <summary>
		/// The underyling COM element object to which this behavior is currently attached.
		/// </summary>
		public Interop.IHTMLElement AttachedElement
		{
			get
			{
				return site.GetElement();
			}
		}

	}
}
