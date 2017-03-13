using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using GuruComponents.Netrix.ComInterop;
using ComTypes = GuruComponents.Netrix.ComInterop;

namespace GuruComponents.Netrix.Networking
{
    /// <summary>
    /// This is the implementation of the <see cref="Interop.IOleClientSite"/> interface.
    /// Additionally the internal authentication was implemented later.
    /// </summary>
    internal class ClientSite : Interop.IAuthenticate, Interop.IHttpSecurity // Interop.IOleClientSite, 
    {

        string userName;
        string passWord;

        internal ClientSite()
        {
        }

        internal ClientSite(string un, string pw)
        {
            userName = un;
            passWord = pw;
        }

        [DispId(-5512)]
        internal static int Idispatch_Invoke_Handler()
        {
            return 
                DispId.DLCTL_NO_SCRIPTS | 
                DispId.DLCTL_NO_JAVA | 
                DispId.DLCTL_NO_DLACTIVEXCTLS |
                DispId.DLCTL_NO_RUNACTIVEXCTLS | 
                DispId.DLCTL_DOWNLOADONLY |
                DispId.DLCTL_SILENT | 
                DispId.DLCTL_DLIMAGES | 0;
        }

        //# region IOleClientSite Implementation 

        //public int SaveObject()
        //{
        //    Debug.WriteLine("SaveObject", "ClientSite");
        //    return Interop.S_OK;
        //}

        //public int GetMoniker(int dwAssign, int dwWhichMoniker, out Object ppmk)
        //{  
        //    Debug.WriteLine("GetMoniker", "ClientSite");
        //    //ppmk = new ClientMoniker();
        //    ppmk = null;
        //    return Interop.E_NOTIMPL; // Interop.S_OK
        //}

        //public int GetContainer(out Interop.IOleContainer ppContainer)
        //{
        //    Debug.WriteLine("GetConteiner", "ClientSite");
        //    ppContainer = null;
        //    return Interop.E_NOINTERFACE;
        //}

        //public int ShowObject()
        //{
        //    return Interop.S_OK;
        //}

        //public int OnShowWindow(int fShow)
        //{
        //    return Interop.S_OK;
        //}

        //public int RequestNewObjectLayout()
        //{
        //    return Interop.S_OK;
        //}

        //# endregion

        #region IAuthenticate Member

        public void Authenticate(IntPtr phwnd, ref string pszUsername, ref string pszPassword)
        {
            pszUsername = userName;
            pszPassword = passWord;
        }

        #endregion

        #region IHttpSecurity Member

        public int GetWindow(ref Guid refGuid, ref IntPtr phWnd)
        {
            return Interop.S_OK;
        }

        public int OnSecurityProblem(int dwProblem)
        {            
            return Interop.S_OK;
        }

        #endregion

    }
}
