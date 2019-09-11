using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace NationalActTest
{
    public class HttpHelper
    {
        /// <summary>
        /// httpget
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="timeOut">超时时间 秒</param>
        /// <param name="headers">请求头</param>
        /// <returns></returns>
        public static async Task<string> HttpGet(string url, int timeOut = 3, Dictionary<string, string> headers = null)
        {
            var httpClient = new HttpClient();
            try
            {
                httpClient.Timeout = TimeSpan.FromSeconds(timeOut);

                if (headers != null)
                {
                    foreach (KeyValuePair<string, string> k in headers)
                        httpClient.DefaultRequestHeaders.Add(k.Key, k.Value);
                }

                var response = await httpClient.GetAsync(url);
                var buffer = await response.Content.ReadAsStreamAsync();

                string result;
                StreamReader reader;
                Encoding code = Encoding.GetEncoding(response.Content.Headers.ContentType.CharSet);

                if (response.Content.Headers.ContentEncoding.ToString().Contains("gzip"))
                {
                    reader = new StreamReader(new GZipStream(buffer, CompressionMode.Decompress), code);
                }
                else
                {
                    reader = new StreamReader(buffer, code);
                }
                result = await reader.ReadToEndAsync();
                return result;
            }
            catch
            {
                return "";
            }
            finally
            {
                httpClient.Dispose();
            }
        }

        /// <summary>
        /// httppost
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="timeOut">超时时间 秒</param>
        /// <param name="postData">post数据 a=1&b=2</param>
        /// <param name="headers">请求头</param>
        /// <returns></returns>
        public static async Task<string> HttpPost(string url, int timeOut = 3, string postData = "",
            Dictionary<string, string> headers = null)
        {
            var httpClient = new HttpClient();
            try
            {
                httpClient.Timeout = TimeSpan.FromSeconds(timeOut);

                if (headers != null)
                {
                    foreach (KeyValuePair<string, string> k in headers)
                        httpClient.DefaultRequestHeaders.Add(k.Key, k.Value);
                }

                HttpContent content = new StringContent(postData);
                content.Headers.ContentType =
                    new System.Net.Http.Headers.MediaTypeHeaderValue("application/x-www-form-urlencoded");

                var response = await httpClient.PostAsync(url, content);
                var buffer = await response.Content.ReadAsStreamAsync();

                string result;
                StreamReader reader;
                Encoding code = Encoding.GetEncoding(response.Content.Headers.ContentType.CharSet);

                if (response.Content.Headers.ContentEncoding.ToString().Contains("gzip"))
                {
                    reader = new StreamReader(new GZipStream(buffer, CompressionMode.Decompress), code);
                }
                else
                {
                    reader = new StreamReader(buffer, code);
                }
                result = await reader.ReadToEndAsync();
                return result;
            }
            catch
            {
                return "";
            }
            finally
            {
                httpClient.Dispose();
            }
        }

        /// <summary>
        /// 同步get post
        /// </summary>
        /// <param name="url"></param>
        /// <param name="requestMethod">get or post</param>
        /// <param name="postData">post 数据</param>
        /// <param name="headers"></param>
        /// <param name="timeout"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public static string RequestHttpData(string url, string requestMethod, string postData = "",
            Dictionary<string, string> headers = null, int timeout = 2000, string contentType = "")
        {
            try
            {
                var request = WebRequest.Create(url) as HttpWebRequest;
                if (request == null)
                {
                    return "";
                }
                request.Timeout = timeout;
                request.Method = requestMethod;
                request.KeepAlive = true;
                request.AllowAutoRedirect = false;
                if (string.IsNullOrEmpty(contentType))
                    request.ContentType = "application/x-www-form-urlencoded";
                else
                    request.ContentType = contentType;

                if (headers != null)
                {
                    foreach (KeyValuePair<string, string> k in headers)
                        request.Headers.Add(k.Key, k.Value);
                }

                if (!string.IsNullOrEmpty(postData))
                {
                    byte[] postdatabtyes = Encoding.UTF8.GetBytes(postData);
                    request.ContentLength = postdatabtyes.Length;
                    Stream requeststream = request.GetRequestStream();
                    requeststream.Write(postdatabtyes, 0, postdatabtyes.Length);
                    requeststream.Close();
                }
                else
                {
                    request.ContentLength = 0;
                }

                string result;
                using (var response = request.GetResponse() as HttpWebResponse)
                {
                    if (response == null)
                    {
                        return "";
                    }
                    StreamReader reader;
                    if (response.ContentEncoding.Equals("gzip", StringComparison.InvariantCultureIgnoreCase))
                    {
                        reader = new StreamReader(new GZipStream(response.GetResponseStream(), CompressionMode.Decompress),
                            Encoding.UTF8);
                    }
                    else
                    {
                        reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                    }
                    result = reader.ReadToEnd();
                    reader.Close();
                    reader.Dispose();
                }



                return result;
            }
            catch (Exception ex)
            {

                return "";
            }

        }



    }
}