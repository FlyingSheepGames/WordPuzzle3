using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web;

namespace WordPuzzles
{
    public class BirthdayFinder
    {
        private const int MAX_QUOTES = 20;

        public List<Person> FindPeopleForDate(int month, int date)
        {
            string monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month).ToLower();
            const string BRAINY_QUOTE_DOMAIN = @"https://www.brainyquote.com";
            string url = BRAINY_QUOTE_DOMAIN + $@"/birthdays/{monthName}_{date}";
            string page = WebRequestUtility.ReadHtmlPageFromUrl(url);
            bool alreadySkippedFirstOne = false;
            var findPeopleForDate = new List<Person>();

            foreach (var pageFragment in page.Split(new[] {@"<td><a href="""}, StringSplitOptions.RemoveEmptyEntries))
            {
                if (!alreadySkippedFirstOne)
                {
                    alreadySkippedFirstOne = true;
                    continue;
                }
                int indexOfAnchorCloseTag = pageFragment.IndexOf("</a>");
                if (indexOfAnchorCloseTag < 0) continue;
                string workingFragment = pageFragment.Substring(0, indexOfAnchorCloseTag);
                int indexOfAnchorTagEnd = workingFragment.IndexOf(@""">");
                if (indexOfAnchorTagEnd < 0) continue;
                string currentName = workingFragment.Substring(indexOfAnchorTagEnd + 2);
                //Console.WriteLine(workingFragment);

                string quotesUrl = BRAINY_QUOTE_DOMAIN + workingFragment.Substring(0, indexOfAnchorTagEnd);
                //Console.WriteLine(quotesUrl);

                findPeopleForDate.Add(
                    new Person()
                    {
                        Name = currentName,
                        Quotes = LoadQuotesFromUrl(quotesUrl),
                    });
            }

            return findPeopleForDate;
        }

        private List<string> LoadQuotesFromUrl(string quotesUrl)
        {
            List<string> quotesFromUrl = new List<string>();
            string htmlFromQuotesPage = WebRequestUtility.ReadHtmlPageFromUrl(quotesUrl);
            bool alreadySkippedFirstEntry = false;
            foreach (string pageFragment in htmlFromQuotesPage.Split(new[] {"title=\"view quote\">"},
                StringSplitOptions.RemoveEmptyEntries))
            {
                if (!alreadySkippedFirstEntry)
                {
                    alreadySkippedFirstEntry = true;
                    continue;
                }
                int indexOfAnchorCloseTag = pageFragment.IndexOf("</a>");
                if (indexOfAnchorCloseTag < 0) continue;
                string quoteAsString = pageFragment.Substring(0, indexOfAnchorCloseTag);
                quoteAsString = HttpUtility.HtmlDecode(quoteAsString);
                //Console.WriteLine(HttpUtility.HtmlDecode(quoteAsString));
                if (MAX_QUOTES < quotesFromUrl.Count) break;
                quotesFromUrl.Add(quoteAsString);
            }
            return quotesFromUrl;
        }
    }

    public class Person
    {
        public string Name { get; set; }
        public List<string> Quotes { get; set; }
    }
}