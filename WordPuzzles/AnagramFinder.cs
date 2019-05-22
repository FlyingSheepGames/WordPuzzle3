using System;
using System.Collections.Generic;
using System.Linq;

namespace WordPuzzles
{
    public class AnagramFinder
    {
        public WordRepository Repository = null;
        public AnagramFinder()
        {
            MinimumWordLength = 4;
        }
        public List<string> FindAnagram(string letters)
        {
            string url = string.Format($@"https://new.wordsmith.org/anagram/anagram.cgi?anagram={letters}&t=500&a=n");

            string response = WebRequestUtility.ReadHtmlPageFromUrl(url);
            return ParseResponse(response);
        }

        internal List<string> ParseResponse(string response)
        {
            var list = new List<string>();
            int indexOfDisplayingAll = response.IndexOf(@"Displaying all", StringComparison.Ordinal);
            if (indexOfDisplayingAll < 0) return list;
            string remainingResponse = response.Substring(indexOfDisplayingAll + @"Displaying all".Length + 1);
            int indexOfScript = remainingResponse.IndexOf(@"<script>", StringComparison.Ordinal);
            remainingResponse = remainingResponse.Substring(0, indexOfScript);
            foreach (string line in remainingResponse.Split( new[] {"<br>"}, StringSplitOptions.RemoveEmptyEntries))
            {
                string currentLine = line.Trim();
                if (currentLine.StartsWith("<")) continue;
                Console.WriteLine(currentLine);
                foreach (string word in currentLine.Split(new []{" "}, StringSplitOptions.RemoveEmptyEntries))
                {
                    if (word.Length < MinimumWordLength) continue;
                    string lowerCaseWord = word.ToLowerInvariant();
                    if (!list.Contains(lowerCaseWord))
                    {
                        bool includeWord = true;
                        if (Repository != null)
                        {
                            if (lowerCaseWord.Length < 7) //Can only validate words of length 3, 4, 5, 6
                            {
                                includeWord = Repository.IsAWord(lowerCaseWord);
                            }
                        }

                        if (includeWord)
                        {
                            list.Add(lowerCaseWord);
                        }

                    }
                }
            }


            return list.OrderByDescending(o=> o.Length).ToList();
        }

        public int MinimumWordLength { get; set; }
    }
}