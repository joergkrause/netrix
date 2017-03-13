using System;
using GuruComponents.Netrix.ComInterop;

namespace GuruComponents.Netrix.Networking
{
    /// <summary>
    /// TODO: Add comment.
    /// </summary>
    public class InternetSession : Interop.IInternetSession
    {


        /// <summary>
        /// TODO: Add comment.
        /// </summary>
        public InternetSession(IHtmlEditor editor)
        { 

        }

        /// <summary>
        /// TODO: Add comment.
        /// </summary>
        public InternetSession()
        { 

        }


        #region IInternetSession Members

        /// <summary>
        /// TODO: Add comment.
        /// </summary>
        public int RegisterNameSpace(Interop.IClassFactory classFactory, ref Guid rclsid, string pwzProtocol, int cPatterns, string ppwzPatterns, int dwReserved)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        /// <summary>
        /// TODO: Add comment.
        /// </summary>
        public int UnregisterNameSpace(Interop.IClassFactory classFactory, string pszProtocol)
        {
            throw new Exception("The method or operation is not implemented.");
        }


        #endregion

        #region IInternetSession Members


        /// <summary>
        /// TODO: Add comment.
        /// </summary>
        public int RegisterMimeFilter(Interop.IClassFactory pCF, ref Guid rclsid, string pwzType)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        /// <summary>
        /// TODO: Add comment.
        /// </summary>
        public int UnregisterMimeFilter(Interop.IClassFactory pCF, string pwzType)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        /// <summary>
        /// TODO: Add comment.
        /// </summary>
        public int CreateBinding(IntPtr pBC, string szUrl, Interop.IUnknown pUnkOuter, Interop.IUnknown ppUnk, Interop.IInternetProtocol ppOInetProt, int dwOption)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        /// <summary>
        /// TODO: Add comment.
        /// </summary>
        public int SetSessionOption(int dwOption, object pBuffer, int dwBufferLength, int dwReserved)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        /// <summary>
        /// TODO: Add comment.
        /// </summary>
        public int GetSessionOption(int dwOption, object pBuffer, int pdwBufferLength, int dwReserved)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}
