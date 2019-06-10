using System;
using System.Collections.Generic;

namespace WordPuzzles
{
    public class WordnikUtility
    {
        public List<PotentialTheme> FindPotentialThemes(string word)
        {
            List<PotentialTheme> themes = new List<PotentialTheme>();
            string html = WebRequestUtility.ReadHtmlPageFromUrl($"https://www.wordnik.com/fragments/lists/{word}");
            int indexOfStartOfOtherLists = html.IndexOf(@"<ul class=""other_lists"">");
            if (indexOfStartOfOtherLists < 0) return null;
            string remainingHtml = html.Substring(indexOfStartOfOtherLists);
            foreach (string listFragment in remainingHtml.Split(new string[] { @"<a href=""/lists/" }, StringSplitOptions.RemoveEmptyEntries))
            {
                int indexOfQuote = listFragment.IndexOf('"');
                string listName = listFragment.Substring(0, indexOfQuote);
                int indexOfCount = listFragment.IndexOf(@"<span class=""count"">", StringComparison.Ordinal);
                if (indexOfCount < 0)
                {
                    //Console.WriteLine(listFragment);
                    continue;
                }
                string countAsString = listFragment.Substring(indexOfCount + @"<span class=""count"">".Length);
                int indexOfSpace = countAsString.IndexOf(" ", StringComparison.Ordinal);
                countAsString = countAsString.Substring(0, indexOfSpace);
                int count;
                if (int.TryParse(countAsString, out count))
                {
                    if (count <= 300)
                    {
                        themes.Add(new PotentialTheme() { Count = count, Name = listName });
                    }
                }
            }

            themes.Sort((a, b) => a.Count - b.Count);
            return themes;
        }

        public List<string> FindWordsInList(string listName)
        {
            List<string> words = new List<string>();
            string html = WebRequestUtility.ReadHtmlPageFromUrl($"https://www.wordnik.com/lists/{listName}");
            foreach (string fragment in html.Split(new[] {@"<li class=""word""><a href=""/words/"},
                StringSplitOptions.RemoveEmptyEntries))
            {
                int indexOfQuote = fragment.IndexOf('"');
                string wordToInclude = fragment.Substring(0, indexOfQuote);
                if (wordToInclude.Contains(" ")) continue;
                if (wordToInclude.Contains("-")) continue;
                words.Add(wordToInclude);
            }

            return words;
        }
    }

    public class PotentialTheme
    {
        public string Name;
        public int Count;
    }
}