using System;
using System.Text;
using System.Collections.Specialized;
using System.Net;
using GuruComponents.Netrix.Events;

namespace GuruComponents.Netrix.Networking
{
    /// <summary>
    /// Receives a request and send the response.
    /// </summary>
    internal class HttpRequestResponse
    {
        private string URI;
        private string Request;
        private string UserName;
        private string UserPwd;
        private string proxyServer;
        private string userAgent = "Mozilla/4.0 (compatible; NetRix)";
        private int proxyPort;
        private ICredentials credentials;
        private string RequestMethod = "GET";

        /// <summary>
        /// Constructor of class, set request data and request URI.
        /// </summary>
        /// <param name="pRequest">The request data.</param>
        /// <param name="pURI">The request URI.</param>
        public HttpRequestResponse(string pRequest, string pURI)
        {
            Request = pRequest;
            URI = pURI;
        }

        public string HttpUserName
        {
            get
            {
                return UserName;
            }
            set
            {
                UserName = value;
            }
        }

        public string HttpUserPassword
        {
            get
            {
                return UserPwd;
            }
            set
            {
                UserPwd = value;
            }
        }

        public string UserAgent
        {
            get
            {
                return userAgent;
            }
            set
            {
                userAgent = value;
            }
        }

        public ICredentials ProxyCredentials
        {
            get
            {
                return credentials;
            }
            set
            {
                credentials = value;
            }
        }

        public string ProxyServer
        {
            get
            {
                return proxyServer;
            }
            set
            {
                proxyServer = value;
            }
        }

        public int ProxyPort
        {
            get
            {
                return proxyPort;
            }
            set
            {
                proxyPort = value;
            }
        }

        /// <summary>
        /// This public interface receives the request and send the response of type byte array
        /// </summary>
        /// <returns></returns>
        public byte[] SendRequest(HtmlEditor Editor, int timeout)        
        {
            byte[] FinalResponse = new byte[0];
            string Cookie = "";

            NameValueCollection collHeader = new NameValueCollection();

            HttpWebResponse webresponse = null;
            HttpWebRequest webrequest = null;

            HttpBaseClass BaseHttp = new HttpBaseClass(UserName, UserPwd, proxyServer, proxyPort, ProxyCredentials, Request, Editor);
            try
            {
                webrequest = BaseHttp.CreateWebRequest(null, URI, collHeader, RequestMethod, true);
				webrequest.CachePolicy = new System.Net.Cache.RequestCachePolicy(System.Net.Cache.RequestCacheLevel.CacheIfAvailable);
                webrequest.UserAgent = UserAgent;
                webrequest.Timeout = timeout;
                webresponse = (HttpWebResponse) webrequest.GetResponse();
                
                string ReUri = BaseHttp.GetRedirectURL(webresponse, ref Cookie);
                //Check if there is any redirected URI.
                //webresponse.Close();
                ReUri = ReUri.Trim();
                if (ReUri.Length == 0) //No redirection URI
                {
                    ReUri = URI;
                }

                Encoding enc = Encoding.Default;
                System.IO.StreamReader loResponseStream = new System.IO.StreamReader(webresponse.GetResponseStream(), enc);

                string Response = loResponseStream.ReadToEnd();

                loResponseStream.Close();
                webresponse.Close();

                return enc.GetBytes(Response);


                //FinalResponse = BaseHttp.GetFinalResponse(ReUri, Cookie, RequestMethod, true, timeOut);

            }//End of Try Block

            catch (WebException e)
            {
                InvokeWebError(webrequest, e);
            }
            catch (System.Exception e)
            {
               throw e;
            }
            finally
            {
                BaseHttp = null;
            }
            return FinalResponse;
        } //End of SendRequestTo method


        internal event EventHandler<WebExceptionEventArgs> WebError;

        private void InvokeWebError(HttpWebRequest webrequest, WebException e)
        {
            if (WebError != null)
            {
                WebError(webrequest, new WebExceptionEventArgs(e, webrequest.RequestUri.AbsoluteUri));
            }
        }


    }//End of RequestResponse Class
}