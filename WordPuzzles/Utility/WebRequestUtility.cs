namespace WordPuzzles.Utility
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
