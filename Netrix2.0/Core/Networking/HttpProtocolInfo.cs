using System;
using GuruComponents.Netrix.ComInterop;

namespace GuruComponents.Netrix.Networking
{
    /// <summary>
    /// HTTP protocol info handler.
    /// </summary>
	public class HttpProtocolInfo : Interop.IInternetProtocolInfo
	{

		#region IInternetProtocolInfo Members

        /// <summary>
        /// Parse URL
        /// </summary>
        /// <param name="pwzUrl"></param>
        /// <param name="parseAction"></param>
        /// <param name="dwParseFlags"></param>
        /// <param name="pwzResult"></param>
        /// <param name="cchResult"></param>
        /// <param name="pcchResult"></param>
        /// <param name="dwReserved"></param>
		public void ParseUrl(string pwzUrl, Interop.PARSEACTION parseAction, UInt32 dwParseFlags, IntPtr pwzResult, UInt32 cchResult, out UInt32 pcchResult, UInt32 dwReserved)
		{
			pcchResult = 0;
			//   throw new NotImplementedException();
		}

        /// <summary>
        /// Combine URL
        /// </summary>
        /// <param name="pwzBaseUrl"></param>
        /// <param name="pwzRelativeUrl"></param>
        /// <param name="dwCombineFlags"></param>
        /// <param name="pwzResult"></param>
        /// <param name="cchResult"></param>
        /// <param name="pcchResult"></param>
        /// <param name="dwReserved"></param>
		public void CombineUrl(string pwzBaseUrl, string pwzRelativeUrl, UInt32 dwCombineFlags, IntPtr pwzResult, UInt32 cchResult, out UInt32 pcchResult, UInt32 dwReserved)
		{
			//    throw new NotImplementedException();
			pcchResult = 0;
		}

        /// <summary>
        /// Compare URL
        /// </summary>
        /// <param name="pwzUrl1"></param>
        /// <param name="pwzUrl2"></param>
        /// <param name="dwCompareFlags"></param>
		public void CompareUrl(string pwzUrl1, string pwzUrl2, UInt32 dwCompareFlags)
		{
			//    throw new NotImplementedException();
		}

        /// <summary>
        /// Query URL Info
        /// </summary>
        /// <param name="pwzUrl"></param>
        /// <param name="queryOption"></param>
        /// <param name="dwQueryFlags"></param>
        /// <param name="pBuffer"></param>
        /// <param name="cbBuffer"></param>
        /// <param name="cbBuf"></param>
        /// <param name="dwReserved"></param>
		public void QueryInfo(string pwzUrl, Interop.QUERYOPTION queryOption, UInt32 dwQueryFlags, IntPtr pBuffer, UInt32 cbBuffer, ref UInt32 cbBuf, UInt32 dwReserved)
		{
			//   throw new NotImplementedException();
		}

		#endregion
	}

}
