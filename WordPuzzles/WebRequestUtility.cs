using System.Net;
using System.Text;

namespace WordPuzzles
{
    public class WebRequestUtility
    {
        private static readonly WebRequestUtilityInstance Instance = new WebRequestUtilityInstance();

        internal static bool BggTooManyRequests;
        public static string ReadHtmlPageFromUrl(string url, bool ignoreCache = false)
        {
            return Instance.ReadHtmlPageFromUrl(url, ignoreCache);
        }

    }
}
