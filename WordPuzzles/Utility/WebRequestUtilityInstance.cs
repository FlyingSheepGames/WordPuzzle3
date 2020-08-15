using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;

namespace WordPuzzles.Utility
{
    public class WebRequestUtilityInstance
    {
        // ReSharper disable IdentifierTypo
        private const string BOARDGAMEGEEK_COM = "/boardgamegeek.com/";
        // ReSharper restore IdentifierTypo
        internal readonly List<string> FailedRequests = new List<string>();

        // ReSharper disable once InconsistentNaming
        private readonly string BASE_DIRECTORY = ConfigurationManager.AppSettings["BaseDirectory"]; //@"E:\utilities\WordSquare\data\";

        internal bool BggTooManyRequests;
        public string ReadHtmlPageFromUrl(string url, bool ignoreCache = false)
        {
            if (FailedRequests.Contains(url))
            {
                return null;
            }
            if (!ignoreCache)
            {
                string cachedPage = GetCachedResult(url);
                if (!string.IsNullOrWhiteSpace(cachedPage))
                {
                    return cachedPage;
                }
            }

            // ReSharper disable StringLiteralTypo
            if (url.ToLower().Contains(BOARDGAMEGEEK_COM))
                // ReSharper restore StringLiteralTypo
            {
                if (BggTooManyRequests)
                {
                    return null;
                }
            }

            WebRequest request = WebRequest.Create(url);
            try
            {
                var response = request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                if (responseStream != null)
                {
                    StreamReader reader = new StreamReader(responseStream);
                    string htmlPage = reader.ReadToEnd();
                    CacheSuccessfulRequest(url, htmlPage);
                    return htmlPage;
                }
            }
            catch (WebException exception)
            {
                if (exception.Message.Contains("(429) Too Many Requests."))
                {
                    if (url.ToLower().Contains(BOARDGAMEGEEK_COM))
                    {
                        BggTooManyRequests = true;
                    }

                    if (!FailedRequests.Contains(url))
                    {
                        Debug.WriteLine(string.Format($"Too many requests. Request was {url} \n Message {exception.Message}"));
                        FailedRequests.Add(url);
                    }
                }
                else
                {
                    throw;
                }
            }
            return null;
        }

        private string GetCachedResult(string url)
        {
            string fileName = CreateFileName(url);
            if (File.Exists(fileName))
            {
                return File.ReadAllText(fileName);
            }
            return null;
        }

        private void CacheSuccessfulRequest(string url, string htmlPage)
        {
            if (!Directory.Exists(BASE_DIRECTORY + @"CachedRequests\"))
            {
                Directory.CreateDirectory(BASE_DIRECTORY + @"CachedRequests\");
            }
            string fileName = CreateFileName(url);
            try
            {
                File.WriteAllText(fileName, htmlPage);
            }
            catch (ArgumentException exception)
            {
                Debug.WriteLine(string.Format($"Could not create file '{fileName}'. Exception was {exception.Message}"));

            }
        }

        public string CreateFileName(string url)
        {
            StringBuilder builder = new StringBuilder(url);
            builder = builder.Replace("/", "");
            builder = builder.Replace("\\", "");
            builder = builder.Replace(":", "");
            builder = builder.Replace("+", "");
            builder = builder.Replace("=", "");
            builder = builder.Replace("&", "");
            builder = builder.Replace("?", "");
            builder = builder.Replace("%", "");
            builder = builder.Replace("*", "");
            builder = builder.Replace("~", "");

            return BASE_DIRECTORY + @"CachedRequests\" + builder;
        }
    }
}