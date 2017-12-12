//using System;
//using System.Collections;
//using System.IO;
//using System.Net;
//using System.Text.Encodings.Web;
//using System.Text.RegularExpressions;

//namespace RoboUtil.utils
//{
//    public class WebUtil
//    {
//        /// <summary>
//        ///
//        /// </summary>
//        /// <param name="strURL"></param>
//        /// <param name="isTextHtml"></param>
//        /// <returns></returns>
//        public static String GetHtmlPage(string strURL, bool isTextHtml)
//        {
//            HttpWebRequest httpRequest = BuildRequestWithHeader(strURL);
//            return WebUtil.GetHtmlPage(httpRequest, isTextHtml);
//        }

//        /// <summary>
//        ///
//        /// </summary>
//        /// <param name="strURL"></param>
//        /// <param name="isTextHtml"></param>
//        /// <returns></returns>
//        public static String GetHtmlPage(Uri strURL, bool isTextHtml)
//        {
//            HttpWebRequest httpRequest = BuildRequestWithHeader(strURL);
//            return WebUtil.GetHtmlPage(httpRequest, isTextHtml);
//        }

//        /// <summary>
//        ///
//        /// </summary>
//        /// <param name="strURL"></param>
//        /// <param name="proxy"></param>
//        /// <returns></returns>
//        public static String GetHtmlPage(Uri strURL, bool isTextHtml, WebProxy proxy)
//        {
//            HttpWebRequest httpRequest = BuildRequestWithHeader(strURL);
//            //GlobalProxySelection.Select = proxy;
//            httpRequest.Proxy = proxy;

//            return WebUtil.GetHtmlPage(httpRequest, isTextHtml);
//        }

//        /// <summary>
//        ///
//        /// </summary>
//        /// <param name="request"></param>
//        /// <returns></returns>
//        public static string GetHtmlPage(HttpWebRequest request, bool isTextHtml)
//        {
//            HttpWebResponse response = null;
//            string dataString = null;
//            try
//            {
//                response = (HttpWebResponse)request.GetResponse();

//                if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Found)
//                {
//                    if (isTextHtml)
//                    {
//                        if (response.ContentType.Contains("text/html"))
//                        {
//                            Stream s = (Stream)response.GetResponseStream();
//                            StreamReader readStream = new StreamReader(s);
//                            dataString = readStream.ReadToEnd();
//                            s.Close();
//                            readStream.Close();
//                        }
//                        else
//                        {
//                            dataString = response.ContentType.ToString();
//                        }
//                    }
//                    else
//                    {
//                        Stream s = (Stream)response.GetResponseStream();
//                        StreamReader readStream = new StreamReader(s);
//                        dataString = readStream.ReadToEnd();
//                        s.Close();
//                        readStream.Close();
//                    }
//                }
//                else
//                {
//                    dataString = response.StatusCode.ToString();
//                    Console.WriteLine("HttpStatusCode not OK : " + request.Address.ToString());
//                }
//                Console.WriteLine("GetHtmlPage: " + request.RequestUri + " Response:" + response.StatusCode.ToString());
//            }
//            catch (Exception e)
//            {
//                Console.WriteLine("Hata: " + request.Address.ToString());
//                throw e;
//            }
//            finally
//            {
//                if (response != null)
//                {
//                    response.Close();
//                }
//            }

//            return dataString;
//        }

//        public static StaticResponse GetStaticResponse(string strURL, bool isTextHtml)
//        {
//            HttpWebRequest httpRequest = BuildRequestWithHeader(strURL);
//            return WebUtil.GetStaticResponse(httpRequest, isTextHtml);
//        }

//        public static StaticResponse GetStaticResponse(HttpWebRequest request, bool isTextHtml)
//        {
//            HttpWebResponse response = null;
//            StaticResponse staticResponse = null;
//            try
//            {
//                response = (HttpWebResponse)request.GetResponse();
//                staticResponse = new StaticResponse(response);
//                Console.WriteLine("GetStaticResponse: " + request.RequestUri + " Response:" + staticResponse.StatusCode.ToString());

//                if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Found)
//                {
//                    if (isTextHtml)
//                    {
//                        if (response.ContentType.Contains("text/html"))
//                        {
//                            Stream s = (Stream)response.GetResponseStream();
//                            StreamReader readStream = new StreamReader(s);
//                            staticResponse.ResponseString = readStream.ReadToEnd();
//                            s.Close();
//                            readStream.Close();
//                        }
//                        else
//                        {
//                            staticResponse.ResponseString = null;
//                        }
//                    }
//                    else
//                    {
//                        Stream s = (Stream)response.GetResponseStream();
//                        StreamReader readStream = new StreamReader(s);
//                        staticResponse.ResponseString = readStream.ReadToEnd();
//                        s.Close();
//                        readStream.Close();
//                    }
//                }
//                else
//                {
//                    staticResponse.ResponseString = null;
//                    Console.WriteLine("HttpStatusCode not OK or Found : " + request.Address.ToString());
//                }
//            }
//            catch (Exception e)
//            {
//                Console.WriteLine("Response Error: " + request.Address.ToString() + " " + e.Message);
//                Console.WriteLine("Response Error: " + request.Address.ToString() + " " + e.Message, e);
//                if (response != null)
//                {
//                    staticResponse = new StaticResponse(response.StatusCode);
//                }
//                else
//                {
//                    staticResponse = new StaticResponse(HttpStatusCode.NotFound);
//                }
//                //throw;
//            }
//            finally
//            {
//                if (response != null)
//                {
//                    response.Close();
//                }
//            }
//            return staticResponse;
//        }

//        public class StaticResponse
//        {
//            private string _ContentType;

//            private HttpStatusCode _StatusCode;

//            private string _ResponseString;

//            public StaticResponse(HttpStatusCode statuscode)
//            {
//                _StatusCode = statuscode;
//            }

//            public StaticResponse(HttpWebResponse response)
//            {
//                _StatusCode = response.StatusCode;
//                _ContentType = response.ContentType;
//            }

//            public string ResponseString
//            {
//                get { return _ResponseString; }
//                set { _ResponseString = value; }
//            }

//            public HttpStatusCode StatusCode
//            {
//                get { return _StatusCode; }
//                set { _StatusCode = value; }
//            }

//            public string ContentType
//            {
//                get { return _ContentType; }
//                set { _ContentType = value; }
//            }
//        }

//        /// <summary>
//        ///
//        /// </summary>
//        /// <param name="strURL"></param>
//        /// <returns></returns>
//        public static HttpWebResponse GetResponse(string strURL)
//        {
//            HttpWebRequest httpRequest = BuildRequestWithHeader(strURL);
//            HttpWebResponse response = GetResponse(httpRequest);
//            return response;
//        }

//        /// <summary>
//        ///
//        /// </summary>
//        /// <param name="request"></param>
//        /// <param name="isTextHtml"></param>
//        /// <returns></returns>
//        public static HttpWebResponse GetResponse(HttpWebRequest request)
//        {
//            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
//            return response;
//        }

//        /// <summary>
//        ///
//        /// </summary>
//        /// <param name="strURL"></param>
//        /// <param name="proxy"></param>
//        /// <returns></returns>
//        public static HttpWebRequest BuildRequestWithHeader(Uri strURL, WebProxy proxy)
//        {
//            HttpWebRequest httpRequest = BuildRequestWithHeader(strURL);
//            httpRequest.Proxy = proxy;
//            return httpRequest;
//        }

//        /// <summary>
//        /// Genarate request, BuildRequestWithHeader with object url
//        /// </summary>
//        /// <param name="strURL"></param>
//        /// <returns></returns>
//        public static HttpWebRequest BuildRequestWithHeader(Uri strURL)
//        {
//            HttpWebRequest httpRequest = (HttpWebRequest)System.Net.HttpWebRequest.Create(strURL);
//            BuildRequestWithHeader(httpRequest);

//            return httpRequest;
//        }

//        /// <summary>
//        /// BuildRequestWithHeader with string url
//        /// </summary>
//        /// <param name="strURL"></param>
//        /// <returns></returns>
//        public static HttpWebRequest BuildRequestWithHeader(string strURL)
//        {
//            HttpWebRequest httpRequest = (HttpWebRequest)System.Net.HttpWebRequest.Create(strURL);
//            BuildRequestWithHeader(httpRequest);
//            return httpRequest;
//        }

//        /// <summary>
//        ///
//        /// </summary>
//        /// <param name="httpRequest"></param>
//        private static void BuildRequestWithHeader(HttpWebRequest httpRequest)
//        {
//            httpRequest.Method = "GET";
//            httpRequest.Accept = "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/x-shockwave-flash, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, */*";
//            httpRequest.Headers.Add("Accept-Language", "en-US");
//            httpRequest.Headers.Add("Accept-Encoding", "gzip,deflate");
//            httpRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; .NET CLR 1.0.3705; .NET CLR 1.1.4322)";
//            httpRequest.KeepAlive = true;
//            httpRequest.Headers.Add("Cache-Control", "no-cache");
//            httpRequest.ProtocolVersion = HttpVersion.Version10;
//            httpRequest.Referer = "http://www.google.com/";
//            httpRequest.AllowAutoRedirect = false;
//            httpRequest.AllowWriteStreamBuffering = false;
//            httpRequest.MaximumAutomaticRedirections = 4;
//            //httpRequest.Headers.Add("Proxy-Connection", "Keep-Alive");
//            //httpRequest.Headers.Add("Host", "Amiga");
//            //httpRequest.Headers.Add("UA-CPU", "x86");
//        }

//        /// <summary>
//        ///
//        /// </summary>
//        /// <param name="uri"></param>
//        /// <returns></returns>
//        public static DateTime GetPageLastModified(Uri uri)
//        {
//            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uri);
//            request.Credentials = CredentialCache.DefaultCredentials;
//            request.Method = "HEAD";
//            HttpWebResponse response = null;
//            try
//            {
//                response = (HttpWebResponse)request.GetResponse();
//                return response.LastModified;
//            }
//            finally
//            {
//                if (response != null)
//                    response.Close();
//            }
//        }

//        /// <summary>
//        /// Get Fixed links according weburl, from html
//        /// </summary>
//        /// <param name="html"></param>
//        /// <param name="url"></param>
//        /// <returns></returns>
//        public static ArrayList GetLinksFromHtml(string html, Uri baseUrl)
//        {
//            ArrayList links = GetLinksFromHtml(html);

//            ArrayList normalizedLinks = new ArrayList();
//            foreach (string varUrl in links)
//            {
//                if (!varUrl.StartsWith("http://", System.StringComparison.CurrentCultureIgnoreCase))
//                {
//                    if (varUrl.StartsWith("/"))
//                    {
//                        normalizedLinks.Add(new Uri("http://" + baseUrl.DnsSafeHost + varUrl));
//                    }
//                    else
//                    {
//                        normalizedLinks.Add(new Uri("http://" + baseUrl.DnsSafeHost + "/" + varUrl));
//                    }
//                }
//                else
//                {
//                    normalizedLinks.Add(new Uri(varUrl));
//                }
//            }
//            ArrayList filterLinks = new ArrayList();
//            foreach (Uri varFiter in normalizedLinks)
//            {
//                if (varFiter.ToString().Contains(baseUrl.DnsSafeHost))
//                {
//                    filterLinks.Add(varFiter);
//                }
//            }

//            return filterLinks;
//        }

//        /// <summary>
//        /// Get Raw links from html
//        /// </summary>
//        /// <param name="html"></param>
//        /// <returns></returns>
//        public static ArrayList GetLinksFromHtml(string html)
//        {
//            ArrayList links = new ArrayList();
//            Match m = RegExUtil.GetMatchRegEx(RegExUtil.URL_EXTRACTOR, html);
//            while (m.Success)
//            {
//                m = m.NextMatch();
//                links.Add(m.Groups[1].ToString());
//            }

//            return links;
//        }

//        /// <summary>
//        /// get page page image according src
//        /// </summary>
//        /// <param name="html"></param>
//        /// <returns></returns>
//        public static ArrayList GetPageImages(string html)
//        {
//            ArrayList ls = new ArrayList();

//            Match m = RegExUtil.GetMatchRegEx(RegExUtil.SRC_EXTRACTOR, html);
//            while (m.Success)
//            {
//                m = m.NextMatch();
//                ls.Add(m.Groups[1].ToString());
//            }
//            return ls;
//        }

//        public static ArrayList GetPageImages(string html, Uri baseUrl)
//        {
//            ArrayList links = GetPageImages(html);

//            ArrayList normalizedLinks = new ArrayList();
//            foreach (string varUrl in links)
//            {
//                if (!varUrl.StartsWith("http://", System.StringComparison.CurrentCultureIgnoreCase))
//                {
//                    if (varUrl.StartsWith("/"))
//                    {
//                        normalizedLinks.Add(new Uri("http://" + baseUrl.DnsSafeHost + varUrl));
//                    }
//                    else
//                    {
//                        normalizedLinks.Add(new Uri("http://" + baseUrl.DnsSafeHost + "/" + varUrl));
//                    }
//                }
//                else
//                {
//                    normalizedLinks.Add(new Uri(varUrl));
//                }
//            }
//            ArrayList filterLinks = new ArrayList();
//            foreach (Uri varFiter in normalizedLinks)
//            {
//                if (varFiter.ToString().Contains(baseUrl.DnsSafeHost))
//                {
//                    filterLinks.Add(varFiter);
//                }
//            }

//            return filterLinks;
//        }

//        /// <summary>
//        ///
//        /// </summary>
//        /// <param name="dataString"></param>
//        /// <param name="pageUrl"></param>
//        /// <param name="folder"></param>
//        public static void SaveHtmlResponseToFolder(string dataString, string pageUrl, string folder)
//        {
//            Uri url = new Uri(pageUrl);
//            string absolute = url.PathAndQuery.ToString();
//            absolute = absolute.Replace('/', '\\');
//            try
//            {
//                if (url.LocalPath.Equals("/"))
//                {
//                    absolute = "default";
//                }
               
//                FileUtil.Create(folder + UrlEncoder.Default.Encode(url.ToString()) + ".htm", dataString);
//            }
//            catch (Exception e)
//            {
//                Console.WriteLine(e.StackTrace.ToString());
//            }
//        }

//        /// <summary>
//        ///
//        /// </summary>
//        /// <param name="link"></param>
//        /// <returns></returns>
//        public static HttpStatusCode GetResponseCode(string link)
//        {
//            HttpStatusCode resultCode = HttpStatusCode.NotFound;
//            HttpWebResponse httpResponse = null;
//            HttpWebRequest httpRequest;

//            try
//            {
//                httpRequest = WebUtil.BuildRequestWithHeader(new Uri(link));
//                httpResponse = (HttpWebResponse)httpRequest.GetResponse();
//                resultCode = httpResponse.StatusCode;
//            }
//            catch (Exception e)
//            {
//                Console.WriteLine(e.Message, e);
//            }
//            finally
//            {
//                if (httpResponse != null)
//                {
//                    httpResponse.Close();
//                }
//            }
//            return resultCode;
//        }

//        /// <summary>
//        ///
//        /// </summary>
//        /// <param name="link"></param>
//        /// <returns></returns>
//        public static bool isBrokenLink(string link)
//        {
//            if (GetResponseCode(link).Equals(HttpStatusCode.NotFound))
//            {
//                return true;
//            }
//            else
//            {
//                return false;
//            }
//        }

//        /// <summary>
//        ///
//        /// </summary>
//        /// <param name="link"></param>
//        /// <returns></returns>
//        public static bool isBrokenDonwloadLink(string link)
//        {
//            bool result = false;
//            HttpWebResponse httpResponse = null;
//            HttpWebRequest httpRequest;

//            try
//            {
//                httpRequest = WebUtil.BuildRequestWithHeader(new Uri(link));
//                httpResponse = (HttpWebResponse)httpRequest.GetResponse();
//                if (httpResponse.StatusCode == HttpStatusCode.OK)
//                {
//                    StreamReader sr = new StreamReader(httpResponse.GetResponseStream());
//                    sr.Close();
//                    if (httpResponse.ContentType.Contains("application"))
//                    {
//                        result = false;
//                    }
//                    else
//                    {
//                        result = true;
//                    }
//                }

//                else
//                {
//                    result = true;
//                }
//            }
//            catch (Exception e)
//            {
//                Console.WriteLine(e.Message, e);
//                result = true;
//            }
//            finally
//            {
//                if (httpResponse != null)
//                {
//                    httpResponse.Close();
//                }
//            }

//            return result;
//        }
//    }
//}
