using System;
using System.Collections;
using GuruComponents.Netrix.ComInterop;

namespace GuruComponents.Netrix.Networking
{
	/// <summary>
	/// Allows registeríng new asynchronous pluggable protocol implementations.
	/// </summary>
	/// <remarks>
	/// The base component currently supports file, http and https. With a new APP one can
	/// overriden any of these to get non-default download behavior or write additional
	/// protocols to achieve a specific behavior.
	/// <para>
	/// Basically, pluggable protocols act as a redirection to different download behavior.
	/// This means, any tag which forces the download of a resource will have a protocol, like
	/// file: or http:. The handler is responsible for the download procedure. Overriding 
	/// existing protocols or inventing new ones is a good idea to simplify complex applications.
	/// When you, for instance, want to load webpages from an internal webserver, but this 
	/// webserver does not handle http weill or completely, the a new protocol like "light:"
	/// could be used. After replacing links like &lt;a href="light:page3.html"&gt; and 
	/// registering the protocol any resource request will call the protocols handler. 
	/// Instead of loading from file the handler may load from database, string or whatever.
	/// </para>
	/// Pluggable protocols will work in browse mode (non-design) only.
	/// <para>
	/// See also the Guide For Advanced Developers, section APP.
	/// </para>
	/// </remarks>
	public class InternetSessionRegistry
	{

        static Hashtable _currentProtocolHandler=new Hashtable();
        static Hashtable _currentMimeHandler=new Hashtable();
		static Interop.IInternetSession internetSession;

		/// <summary>
		/// Register a new APP for the application.
		/// </summary>
		/// <param name="protocol">the protocol (for instance "http")</param>
		/// <param name="factory">the IProtocolFactory used to build the protocol handler</param>
		public static void Register(string protocol, IProtocolFactory factory)
		{	
			Guid guid=Guid.Empty;
			//internetSession.RegisterNameSpace(new ClassFactory(), ref guid, "http", 0,null, 0);

			if(_currentProtocolHandler[protocol]!=null)
			{
				Unregister(protocol);
			}
			ClassFactory f = new ClassFactory(factory);
			_currentProtocolHandler[protocol] = f;
			internetSession.RegisterNameSpace(f, ref guid, protocol, 0, null, 0);
		}

        /// <summary>
		/// Register a new Mime handler for the application.
		/// </summary>
		/// <param name="mime">Mime type being used.</param>
		/// <param name="forProtocol">the protocol (for instance "http")</param>
		/// <param name="factory">the IProtocolFactory used to build the protocol handler</param>
        public static void RegisterMime(string mime, string forProtocol, ISessionFactory factory)
        {
            Guid guid = Guid.Empty;
            
            if (_currentMimeHandler[mime] != null)
            {
                UnregisterMime(mime);
            }
            //ClassFactory f = (ClassFactory) _currentProtocolHandler[forProtocol];
            ClassFactory f = new ClassFactory(factory);
            _currentMimeHandler[mime] = f;
            internetSession.RegisterMimeFilter(f, ref guid, mime);
        }

        /// <summary>
        /// Number of currently registered handlers.
        /// </summary>
        public static int HandlersCount
        {
            get
            {
                return (_currentProtocolHandler == null) ? -1 : _currentProtocolHandler.Count;
            }
        }

        /// <summary>
        /// Number of currently registered mime handlers.
        /// </summary>
        public static int MimeCount
        {
            get
            {
                return (_currentMimeHandler == null) ? -1 : _currentMimeHandler.Count;
            }
        }

		static InternetSessionRegistry()
		{
			Win32.CoInternetGetSession(0, out internetSession, 0);
		}

		/// <summary>
		/// Unregister all registered protocols.
		/// </summary>
		public static void UnregisterAll()
		{
			int currentProtocolHandlerKeys = _currentProtocolHandler.Keys.Count;
			string[] handler = new string[currentProtocolHandlerKeys];
			_currentProtocolHandler.Keys.CopyTo(handler, 0);
			for(int i = 0; i < currentProtocolHandlerKeys; i++)
			{
				Unregister(handler[i]);
			}
		}

		/// <summary>
		/// Unregister a previously registered protocol.
		/// </summary>
		/// <param name="protocol"></param>
		public static void Unregister(string protocol)
		{
			if(_currentProtocolHandler[protocol]==null) return;
			internetSession.UnregisterNameSpace((Interop.IClassFactory)_currentProtocolHandler[protocol], protocol);
			_currentProtocolHandler.Remove(protocol);
		}

        /// <summary>
        /// TODO: Add comment.
        /// </summary>
        public static void UnregisterMime(string protocol)
        {
            if (_currentMimeHandler[protocol] == null) return;
            internetSession.UnregisterMimeFilter((Interop.IClassFactory)_currentMimeHandler[protocol], protocol);
            _currentMimeHandler.Remove(protocol);
        }
	}
}
