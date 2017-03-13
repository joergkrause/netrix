using System;
using GuruComponents.Netrix.ComInterop;
using System.Runtime.InteropServices;

# pragma warning disable 0618

namespace GuruComponents.Netrix.Networking
{
	/// <summary>
	/// Controls the zone management the control is running in if activated.
	/// </summary>
	internal sealed class SecurityManager : Interop.IInternetSecurityManager
	{
		public SecurityManager()
		{
		}

        public int GetSecurityId(string pwszUrl, byte[] pbSecurityId, ref uint pcbSecurityId, uint dwReserved)
        {
            return Interop.INET_E_DEFAULT_ACTION;
        }
        public int GetSecuritySite(out Interop.IInternetSecurityMgrSite mgr)
        {
            mgr = null;
            return Interop.E_NOTIMPL;
        }
        public int GetZoneMappings(Interop.URLZONE dwZone, out UCOMIEnumString ppenumString, uint dwFlags)
        {
            ppenumString = null;
            return Interop.INET_E_DEFAULT_ACTION;
        }
        public int MapUrlToZone(string pwszUrl, out Interop.URLZONE pdwZone, uint dwFlags)
        {
            pdwZone = Interop.URLZONE.INTERNET;
            System.Diagnostics.Debug.WriteLine(pwszUrl, "MapUrlToZone");
            if (pwszUrl == "http://www.comzept.de")
            {
                pdwZone = Interop.URLZONE.LOCAL_MACHINE;
                //return Interop.S_OK;
            }
            //return Interop.S_OK;
            return Interop.INET_E_DEFAULT_ACTION;
        }
        public int ProcessUrlAction(string pwszUrl, uint dwAction, ref byte pPolicy,
            uint cbPolicy, byte pContext, uint cbContext, uint dwFlags, uint dwReserved)
        {
            System.Diagnostics.Debug.WriteLine(pwszUrl, "ProcessUrlAction");
            return Interop.S_OK;
            //pPolicy = (byte)Interop.URLPOLICY.ALLOW;
            //if ((Interop.URLACTIONS)dwAction == Interop.URLACTIONS.SCRIPT_RUN)
            //{
            //    pPolicy = (byte)Interop.URLPOLICY.ALLOW;
            //    return Interop.S_OK;
            //}
            //return Interop.INET_E_DEFAULT_ACTION;
        }
        public int QueryCustomPolicy(string pwszUrl, ref Guid guidKey, ref byte ppPolicy, 
            ref Interop.URLPOLICY pcbPolicy, ref byte pContext, uint cbContext, uint dwReserved)
        {
            return Interop.INET_E_DEFAULT_ACTION;
        }
        public int SetSecuritySite(Interop.IInternetSecurityMgrSite pSite)
        {
            return Interop.INET_E_DEFAULT_ACTION;
        }
        public int SetZoneMapping(uint dwZone, string lpszPattern, Interop.SZM_FLAG dwFlags)
        {
           return Interop.INET_E_DEFAULT_ACTION;
        }

	}
}
