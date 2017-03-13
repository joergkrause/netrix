using System;
using System.Runtime.InteropServices;

using GuruComponents.Netrix.ComInterop;

namespace GuruComponents.Netrix.Networking
{
    /// <summary>
    /// Create COM classes. Internally only.
    /// </summary>
	public class ClassFactory: Interop.IClassFactory 
	{
		private const int CLASS_E_NOAGGREGATION = unchecked((int) 0x80040110);
		private const int E_NOINTERFACE = unchecked((int) 0x80004002);
		private readonly Guid IID_IInternetProtocol		= new Guid("{79EAC9E4-BAF9-11CE-8C82-00AA004BA90B}");
		private readonly Guid IID_IInternetProtocolInfo = new Guid("{79EAC9EC-BAF9-11CE-8C82-00AA004BA90B}");
		private readonly Guid IID_IUnknown				= new Guid("{00000000-0000-0000-C000-000000000046}");
        private readonly Guid IID_InternetSession		= new Guid("{79eac9e7-baf9-11ce-8c82-00aa004ba90b}");

		
		IFactory _currentFactory;
        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="factory"></param>
		public ClassFactory(IProtocolFactory factory) 
		{
			_currentFactory=factory;
		}
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="factory"></param>
        public ClassFactory(ISessionFactory factory)
        {
            _currentFactory = factory;
        }
        /// <summary>
        /// Create an instance
        /// </summary>
        /// <param name="pUnkOuter"></param>
        /// <param name="riid"></param>
        /// <param name="ppvObject"></param>
		public void CreateInstance(IntPtr pUnkOuter, ref Guid riid, out IntPtr ppvObject) 
		{
		    if (riid == IID_IInternetProtocol || riid == IID_IUnknown)
			{
				object ctrl = ((IProtocolFactory)_currentFactory).GetIInternetProtocol();
				ppvObject = Marshal.GetComInterfaceForObject(ctrl,typeof(Interop.IInternetProtocol));
			} 
			else if (riid == IID_IInternetProtocolInfo)
			{
				HttpProtocolInfo ctrl = new HttpProtocolInfo();
				ppvObject = Marshal.GetComInterfaceForObject(ctrl,typeof(Interop.IInternetProtocolInfo));
			} 
			else if (riid == IID_InternetSession)
			{
				InternetSession ctrl = new InternetSession();
				ppvObject = Marshal.GetComInterfaceForObject(ctrl,typeof(Interop.IInternetSession));
			} 
			else
			{
				ppvObject = IntPtr.Zero;
				Marshal.ThrowExceptionForHR(E_NOINTERFACE);
			}
		}

        /// <summary>
        /// Not implemented.
        /// </summary>
        /// <param name="fLock"></param>
		public void LockServer(bool fLock) 
		{
		} 
	} 

}
