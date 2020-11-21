using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web;

namespace WordPuzzles.Utility
{
    public class BirthdayFinder
    {
        private const int MAX_QUOTES = 20;
        private const string BRAINY_QUOTE_DOMAIN = @"https://www.brainyquote.com";

        public List<Person> FindPeopleForDate(int month, int date)
        {
            string monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month).ToLower();
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

                int year;
                if (!GetNameAndUrlFromFragment(pageFragment, out var currentName, out var quotesUrl, out year))
                {
                    continue;
                }
                //Console.WriteLine(quotesUrl);

                findPeopleForDate.Add(
                    new Person()
                    {
                        Name = currentName,
                        Year = year,
                        Quotes = LoadQuotesFromUrl(quotesUrl),
                    });
            }

            findPeopleForDate.Sort(SortPeopleByYear);
            return findPeopleForDate;
        }

        private int SortPeopleByYear(Person x, Person y)
        {
            return x.Year - y.Year;
        }

        internal bool GetNameAndUrlFromFragment(string pageFragment, out string currentName,
            out string quotesUrl, out int year)
        {
            currentName = null;
            quotesUrl = null;
            year = 0;
            var htmlSeperators = new[] {"<td>", "\">", "</"};
            var parsedTokens = pageFragment.Split(htmlSeperators, StringSplitOptions.RemoveEmptyEntries);
            for (var index = 0; index < parsedTokens.Length; index++)
            {
                var fragment = parsedTokens[index];
                switch (index)
                {
                    case 0:
                        quotesUrl = BRAINY_QUOTE_DOMAIN + fragment;
                        break;
                    case 1:
                        currentName = fragment;
                        break;
                    case 6:
                        int.TryParse(fragment, out year);
                        break;
                }
            }

            //quotesUrl = BRAINY_QUOTE_DOMAIN + workingFragment.Substring(0, indexOfAnchorTagEnd);
            return true;
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
                int indexOfAnchorCloseTag = pageFragment.IndexOf("</a>", StringComparison.Ordinal);
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
        public int Year { get; set; }
    }
}