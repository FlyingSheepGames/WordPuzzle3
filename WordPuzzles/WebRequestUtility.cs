using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;

namespace WordPuzzles
{
    public class WebRequestUtility
    {
        static readonly List<string> FailedRequests = new List<string>();

        // ReSharper disable once InconsistentNaming
        private static readonly string BASE_DIRECTORY = ConfigurationManager.AppSettings["BaseDirectory"]; //@"E:\utilities\WordSquare\data\";

        private static bool _bggTooManyRequests;
        public static string ReadHtmlPageFromUrl(string url, bool ignoreCache = false)
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

            if (url.ToLower().Contains("/boardgamegeek.com/"))
            {
                if (_bggTooManyRequests)
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
                    if (url.ToLower().Contains("/boardgamegeek.com/"))
                    {
                        _bggTooManyRequests = true;
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

        private static string GetCachedResult(string url)
        {
            string fileName = CreateFileName(url);
            if (File.Exists(fileName))
            {
                return File.ReadAllText(fileName);
            }
            return null;
        }

        private static void CacheSuccessfulRequest(string url, string htmlPage)
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

        public static string CreateFileName(string url)
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

        public static string DownloadBggImage(string imageUrl, int bggId)
        {
            if (string.IsNullOrWhiteSpace(imageUrl)) return null;

            if (!Directory.Exists(BASE_DIRECTORY + @"CachedRequests\BggImages\"))
            {
                Directory.CreateDirectory(BASE_DIRECTORY + @"CachedRequests\BggImages\");
            }

            string fileName = BASE_DIRECTORY + @"CachedRequests\BggImages\" + bggId + ".jpg";

            if (File.Exists(fileName)) return fileName;

            WebClient client = new WebClient();
            client.DownloadFile(imageUrl, fileName);
            return fileName;
        }
    }
}
