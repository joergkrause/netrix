using System;
using System.Text;
using System.Net;
using System.Collections.Specialized;
using System.IO;
using System.Net.Cache;



namespace GuruComponents.Netrix.Networking
{
    /// <summary>
    /// Basic HTTP loader class.
    /// </summary>
    internal class HttpBaseClass
    {

        private string UserName;
        private string UserPwd;
        private string ProxyUserName;
        private string ProxyUserPwd;
        private string ProxyServer;
        private ICredentials proxyCredentials;
        private int ProxyPort;
        private string Request;
        private HtmlEditor editor;

        /// <summary>
        /// Creates a new instance of the HTTP loader class.
        /// </summary>
        /// <param name="HttpUserName">User name, required only if authentication is necessary.</param>
        /// <param name="HttpUserPwd">Password, required only if authentication is necessary.</param>
        /// <param name="HttpProxyServer">Proxy server, optional. Provide empty string if none is required.</param>
        /// <param name="HttpProxyPort">Port the proxy is listening to.</param>
        /// <param name="HttpRequest">Current request data.</param>
        /// <param name="proxyCredentials"></param>
        /// <param name="editor">Editor the HTTP loader belongs to.</param>
        public HttpBaseClass(string HttpUserName, string HttpUserPwd, string HttpProxyServer, int HttpProxyPort, ICredentials proxyCredentials, string HttpRequest, HtmlEditor editor)
        {
            this.UserName = HttpUserName;
            this.UserPwd = HttpUserPwd;
            this.ProxyServer = HttpProxyServer;
            this.ProxyPort = HttpProxyPort;
            this.proxyCredentials = proxyCredentials;
            if (proxyCredentials is NetworkCredential)
            {
                this.ProxyUserPwd = ((NetworkCredential)proxyCredentials).Password;
                this.ProxyUserName = ((NetworkCredential)proxyCredentials).UserName;
            }
            this.Request = HttpRequest;
            this.editor = editor;
        }

        /// <summary>
        /// This method creates secure/non secure web
        /// request based on the parameters passed.
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="collHeader">This parameter of type
        ///    NameValueCollection may contain any extra header
        ///    elements to be included in this request      </param>
        /// <param name="RequestMethod">Value can POST OR GET</param>
        /// <param name="NwCred">In case of secure request this would be true</param>
        /// <returns></returns>
        public virtual HttpWebRequest CreateWebRequest(string baseUri, string uri, NameValueCollection collHeader, string RequestMethod, bool NwCred)
        {
            HttpWebRequest webrequest = null;
            Uri relUri = null;
			if (Uri.IsWellFormedUriString(uri, UriKind.Absolute) || baseUri == null)
            {
                relUri = new Uri(uri);
                webrequest = (HttpWebRequest)WebRequest.Create(uri);
            }
            if (Uri.IsWellFormedUriString(uri, UriKind.Relative) && Uri.IsWellFormedUriString(baseUri, UriKind.Absolute))
            {
                relUri = new Uri(new Uri(baseUri), uri);
                webrequest = (HttpWebRequest)WebRequest.Create(relUri);
            }
            if (webrequest == null) return null;
            webrequest.KeepAlive = false;
            webrequest.Method = RequestMethod;

            int iCount = collHeader.Count;
            string key;
            string keyvalue;

            for (int i = 0; i < iCount; i++)
            {
                key = collHeader.Keys[i];
                keyvalue = collHeader[i];
                webrequest.Headers.Add(key, keyvalue);
            }

            // webrequest.ContentType = "text/html";
            //"application/x-www-form-urlencoded";

            if (!String.IsNullOrEmpty(ProxyServer))
            {
                webrequest.Proxy = new WebProxy(ProxyServer, ProxyPort);
                webrequest.Proxy.Credentials = new NetworkCredential(ProxyUserName, ProxyUserPwd);
            }
            webrequest.AllowAutoRedirect = false;

            if (NwCred)
            {
                CredentialCache wrCache = new CredentialCache();
                wrCache.Add(relUri, "Basic", new NetworkCredential(UserName, UserPwd));
                webrequest.Credentials = wrCache;
            }
            //Remove collection elements
            collHeader.Clear();
            return webrequest;
        }//End of secure CreateWebRequest

        /// <summary>
        /// This method retreives redirected URL from
        /// response header and also passes back
        /// any cookie (if there is any)
        /// </summary>
        /// <param name="webresponse"></param>
        /// <param name="Cookie"></param>
        /// <returns></returns>
        public virtual string GetRedirectURL(HttpWebResponse webresponse, ref string Cookie)
        {
            string uri = "";

            WebHeaderCollection headers = webresponse.Headers;

            if ((webresponse.StatusCode == HttpStatusCode.Found) ||
              (webresponse.StatusCode == HttpStatusCode.Redirect) ||
              (webresponse.StatusCode == HttpStatusCode.Moved) ||
              (webresponse.StatusCode == HttpStatusCode.MovedPermanently))
            {
                // Get redirected uri
                uri = headers["Location"];
#if !DOTNET20
                if (Uri.IsWellFormedUriString(uri, UriKind.Absolute))
                {
                    uri = uri.Trim();
                }
                else
                {
                    uri = webresponse.ResponseUri.AbsoluteUri;
                }
#else
				if (uri.StartsWith("http:"))
				{
					uri = uri.Trim();
				}
				else
				{
					uri = webresponse.ResponseUri.AbsoluteUri;
				}
#endif
            }

            //Check for any cookies
            if (headers["Set-Cookie"] != null)
            {
                Cookie = headers["Set-Cookie"];
            }
            //                string StartURI = "http:/";
            //                if (uri.Length > 0 && uri.StartsWith(StartURI)==false)
            //                {
            //                      uri = StartURI + uri;
            //                }
            return uri;
        }//End of GetRedirectURL method


        /// <summary>
        /// Get the response as binary data.
        /// </summary>
        /// <param name="baseUri">URI requested.</param>
        /// <param name="Cookie">Cookie data, if any.</param>
        /// <param name="RequestMethod">The method used, like GET or POST.</param>
        /// <param name="NwCred">Tells the method that this is a secure request.</param>
        /// <returns></returns>
        public virtual byte[] GetFinalResponse(string baseUri, string Cookie, string RequestMethod, bool NwCred, int timeOut)
        {
            NameValueCollection collHeader = new NameValueCollection();

            if (Cookie.Length > 0)
            {
                collHeader.Add("Cookie", Cookie);
            }

            HttpWebRequest webrequest = CreateWebRequest(null, baseUri, collHeader, RequestMethod, NwCred);            
            if (webrequest == null) return null;
#if !DOTNET20
            webrequest.CachePolicy = new RequestCachePolicy(RequestCacheLevel.CacheIfAvailable);
#endif
            BuildReqStream(ref webrequest);

            HttpWebResponse webresponse;

            webrequest.UserAgent = editor.UserAgent;

            webrequest.KeepAlive = true; //.Proxy.c.Add("Proxy-Connection", "Keep Alive");
            webrequest.Timeout = timeOut;
            webresponse = (HttpWebResponse)webrequest.GetResponse();

            if (webresponse.StatusCode == HttpStatusCode.Found)
            {
				string realUri = webresponse.Headers[HttpResponseHeader.Location];
                webrequest = CreateWebRequest(baseUri, realUri, collHeader, RequestMethod, NwCred);
                if (webrequest == null) return null;
				webrequest.CachePolicy = new RequestCachePolicy(RequestCacheLevel.CacheIfAvailable);
                BuildReqStream(ref webrequest);
                webresponse = (HttpWebResponse)webrequest.GetResponse();
            }

            Encoding enc = editor.Encoding;
            StreamReader loResponseStream = new StreamReader(webresponse.GetResponseStream(), enc);

            string Response = loResponseStream.ReadToEnd();

            loResponseStream.Close();
            webresponse.Close();

            return enc.GetBytes(Response);
        }

        private void BuildReqStream(ref HttpWebRequest webrequest)
        //This method build the request stream for WebRequest
        {
            byte[] bytes = Encoding.ASCII.GetBytes(Request);
            webrequest.ContentLength = bytes.Length;
            if (bytes.Length > 0)
            {
                Stream oStreamOut = webrequest.GetRequestStream();
                oStreamOut.Write(bytes, 0, bytes.Length);
                oStreamOut.Close();
            }
        }
    }
}