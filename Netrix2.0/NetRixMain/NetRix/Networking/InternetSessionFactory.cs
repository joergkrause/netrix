using GuruComponents.Netrix.ComInterop;

namespace GuruComponents.Netrix.Networking
{

	public class InternetSessionFactory : ISessionFactory
	{

		private IHtmlEditor editor;
		public static InternetSession _client;

		public InternetSessionFactory(IHtmlEditor editor)
		{
			this.editor = editor;
			_client = new InternetSession(editor);
		}

		#region ISessionFactory Members
		/// <summary>
		/// Gets ISessionFactory interface.
		/// </summary>
		/// <remarks>
		/// IInternetSession is the main interface exposed by an asynchronous pluggable protocol
		/// to handle private mime types.
		/// </remarks>
		/// <returns>Returns IInternetSession handler.</returns>
		public Interop.IInternetSession GetIInternetSession()
		{
			return _client;
		}
		#endregion


	}
}
