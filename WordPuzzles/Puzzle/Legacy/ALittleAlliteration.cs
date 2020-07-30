using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using WordPuzzles.Utility;

namespace WordPuzzles.Puzzle.Legacy
{
    public class ALittleAlliteration
    {
        private static readonly string A_LITTLE_ALLITERATION_GOOGLE_SHEET = "1J5j8fRBUVT66OjpflMe4K_Q-blicYiYQ2YZ6MbdaLQU";
        private const int MAX_WORDS_TO_RETURN = 50;

        private static readonly string A_LITTLE_ALLITERATION_SEASON_ONE_GOOGLE_SHEET =
            "1DKQ4a_H_JIE15NMXpnedW1dNe5ZqJzCMBXjFyk2IZJo";
        public ALittleAlliteration(): this("") { }

        public ALittleAlliteration(string userResponse)
        {
            if (userResponse == null)
            {
                return;
            }

            int indexOfEqualsSign = userResponse.IndexOf("=", StringComparison.Ordinal);
            if (indexOfEqualsSign < 0)
            {
                return;
            }

            Solution = userResponse.Substring(0, indexOfEqualsSign).Trim();
            Clue = userResponse.Substring(indexOfEqualsSign +1).Trim();
        }

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        private ALittleAlliteration(Dictionary<int, string> dictionary, int season = 0)
        {
            Solution = dictionary[1];

            int CLUE_INDEX = 6;
            int TWITTER_URL_INDEX = 8;
            int DATE_POSTED_INDEX = 10;

            if (season == 1)
            {
                CLUE_INDEX = 7;
                TWITTER_URL_INDEX = 9;
                DATE_POSTED_INDEX = 11;
            }
            if (dictionary.ContainsKey(CLUE_INDEX))
            {
                Clue = dictionary[CLUE_INDEX];
            }


            if (dictionary.ContainsKey(TWITTER_URL_INDEX))
            {
                TwitterUrl = dictionary[TWITTER_URL_INDEX];
            }

            if (dictionary.ContainsKey(DATE_POSTED_INDEX))
            {
                DatePosted = dictionary[DATE_POSTED_INDEX];
            }
        }


        public string DatePosted { get; set; }

        public string TwitterUrl { get; set; }

        public string Clue { get; set; }
        public string Solution { get; set; }

        public string GoogleSheetRow =>
            string.Join("\t", "", Solution, "", "", Solution.Substring(0, 3), "", Clue, Theme, "scheduled", "", "");

        public string Theme { get; set; }


        public List<ALittleAlliteration> FindPuzzle(string firstThreeLetters)
        {
            GoogleSheet sheet = new GoogleSheet() {GoogleSheetKey = A_LITTLE_ALLITERATION_GOOGLE_SHEET };

            string firstThreeLetters1 = firstThreeLetters.ToLower();

            List<ALittleAlliteration> results = new List<ALittleAlliteration>();
            foreach (var dictionary in sheet.ExecuteQuery($"SELECT * WHERE E = '{firstThreeLetters1}'" ))
            {
                var aLittleAlliteration = new ALittleAlliteration(dictionary);
                if (string.IsNullOrWhiteSpace(aLittleAlliteration.TwitterUrl) &&
                    string.IsNullOrWhiteSpace(aLittleAlliteration.DatePosted) &&
                    !string.IsNullOrWhiteSpace(aLittleAlliteration.Clue))
                {
                    results.Add(aLittleAlliteration);
                }
            }
            return results;
        }

        public string GetTweet()
        {
            StringBuilder tweetBuilder = new StringBuilder();
            if (Theme != null && Theme.StartsWith("#"))
            {
                tweetBuilder.AppendLine(Theme);
            }
            tweetBuilder.AppendLine(Clue);
            tweetBuilder.AppendLine();
            tweetBuilder.AppendLine(@"#HowToPlay: https://t.co/rSa0rUCvRC");
            return tweetBuilder.ToString();
        }

        public static List<ALittleAlliteration> GetCluesForTheme(string theme, int season = 0)
        {
            List<ALittleAlliteration> matchingPuzzles = new List<ALittleAlliteration>();

            GoogleSheet sheet = new GoogleSheet() {GoogleSheetKey = A_LITTLE_ALLITERATION_GOOGLE_SHEET};
            string query = $"SELECT * WHERE H LIKE '%{theme}%'";
            if (season == 1)
            {
                sheet.GoogleSheetKey = A_LITTLE_ALLITERATION_SEASON_ONE_GOOGLE_SHEET;
                query = $"SELECT * WHERE I LIKE '%{theme}%'";
            }


            foreach (var result in sheet.ExecuteQuery(query))
            {
                ALittleAlliteration aLittleAlliteration = new ALittleAlliteration(result, season);
                if (!string.IsNullOrWhiteSpace(aLittleAlliteration.Clue))
                {
                    matchingPuzzles.Add(aLittleAlliteration);
                }
            }

            return matchingPuzzles;
    }

        public List<string> FindWordsThatStartWith(string initialLetters, out int totalWordsFound)
        {
            totalWordsFound = 0;

            string content = WebRequestUtility.ReadHtmlPageFromUrl(string.Format(
                @"https://www.morewords.com/most-common-starting-with/{0}",
                initialLetters));

            var wordsFound = ParseContentOldFormat(content, out totalWordsFound);
            if (wordsFound == null || totalWordsFound == 0)
            {
                wordsFound = ParseContentNewFormat(content, out totalWordsFound);
            }
            wordsFound?.Sort();
            return wordsFound;
        }

        public List<string> ParseContentOldFormat(string content, out int wordCount)
        {
            wordCount = 0;
            if (string.IsNullOrEmpty(content)) return null;
            if (content.Contains(@"No words starting with")) return null;
            int startOfUncommonWords = content.IndexOf(@"less common words", StringComparison.Ordinal);
            if (-1 < startOfUncommonWords)
            {
                content = content.Substring(0, startOfUncommonWords);
            }

            string lastTextBeforeList = @"per million words</p>";
            int startOfList = content.IndexOf(lastTextBeforeList, StringComparison.Ordinal);
            if (startOfList <= -1)
            {
                lastTextBeforeList = @"The words occuring most frequently are shown first.</p>";
                startOfList = content.IndexOf(lastTextBeforeList,
                    StringComparison.CurrentCulture); //Try to parse the new format of the page.
            }
            if (-1 < startOfList)
            {
                content = content.Substring(startOfList + lastTextBeforeList.Length);
            }
            List<string> wordsFound = new List<string>();
            var lines = content.Split(new[] { @"<br />" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in lines)
            {
                if (line.StartsWith(@"<a href=""/word/"))
                {
                    var lineWithoutOpeningAnchorTag = line.Substring(@"<a href=""/word/".Length);
                    int endOfWord = lineWithoutOpeningAnchorTag.IndexOf('/');
                    string word = lineWithoutOpeningAnchorTag.Substring(0, endOfWord);
                    //Console.WriteLine(word);
                    if (wordCount < MAX_WORDS_TO_RETURN)
                    {
                        AddIfDistinct(word, wordsFound);
                    }
                    wordCount++;

                }
            }
            return wordsFound;
        }

        public List<string> ParseContentNewFormat(string content, out int wordCount)
        {
            wordCount = 0;
            if (string.IsNullOrEmpty(content)) return null;
            if (content.Contains(@"No words starting with")) return null;
            int startOfUncommonWords = content.IndexOf(@"less common words", StringComparison.Ordinal);
            if (-1 < startOfUncommonWords)
            {
                content = content.Substring(0, startOfUncommonWords);
            }

            string lastTextBeforeList = @"The words occuring most frequently are shown first.</p>";
            int startOfList = content.IndexOf(lastTextBeforeList, StringComparison.Ordinal);
            if (-1 < startOfList)
            {
                content = content.Substring(startOfList + lastTextBeforeList.Length);
            }
            List<string> wordsFound = new List<string>();
            var lines = content.Split(new[] { @"<p>" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in lines)
            {
                var trimmedLine = line.Trim();
                if (trimmedLine.StartsWith(@"<a href=""https://www.morewords.com/word/"))
                {
                    var lineWithoutOpeningAnchorTag = trimmedLine.Substring(@"<a href=""https://www.morewords.com/word/".Length);
                    int endOfWord = lineWithoutOpeningAnchorTag.IndexOf('"');
                    string word = lineWithoutOpeningAnchorTag.Substring(0, endOfWord);
                    //Console.WriteLine(word);
                    if (wordCount < MAX_WORDS_TO_RETURN)
                    {
                        AddIfDistinct(word, wordsFound);
                    }
                    wordCount++;

                }
            }
            return wordsFound;
        }

        public static void AddIfDistinct(string wordToAdd, List<string> currentWords)
        {
            if (currentWords.Contains(wordToAdd))
            {
                return;
            }
            var commonEndings = new[] {"s", "y", "ish", "d", "ly", "less", "ed", "ing", "ful", "ally"};
            var commonEndingWithDoubledLetters = new[] { "ed", "ing" };
            var commonEndingsWithY = new[] {"ies", "ied", "ial"};
            var commonEndingsWithE = new[] {"ing", "ion"};

            foreach (string commonEnding in commonEndings)
            {
                if (wordToAdd.EndsWith(commonEnding) )
                {
                    if (currentWords.Contains(wordToAdd.Substring(0, wordToAdd.Length - commonEnding.Length)))
                    {
                        return;
                    }
                }
            }
            //Sometimes consonants are doubled before adding 'ed' or 'ing. Like chip -> chipping, chipped. 
            foreach (string commonEndingWithDoubledLetter in commonEndingWithDoubledLetters)
            {
                if (wordToAdd.EndsWith(commonEndingWithDoubledLetter))
                {
                    if (currentWords.Contains(wordToAdd.Substring(0, wordToAdd.Length - (commonEndingWithDoubledLetter.Length +1))))
                    {
                        return;
                    }
                }
            }
            // carry -> carried; artery -> arteries
            foreach (string commonEndingWithY in commonEndingsWithY)
            {
                if (wordToAdd.EndsWith(commonEndingWithY))
                {
                    string rootWord = wordToAdd.Substring(0, wordToAdd.Length - commonEndingWithY.Length) + 'y';
                    if (currentWords.Contains(rootWord))
                    {
                        return;
                    }
                }
            }

            foreach (string commonEndingWithE in commonEndingsWithE)
            {
                if (wordToAdd.EndsWith(commonEndingWithE))
                {
                    string rootWord = wordToAdd.Substring(0, wordToAdd.Length - commonEndingWithE.Length) + 'e';
                    if (currentWords.Contains(rootWord))
                    {
                        return;
                    }
                }
            }

            currentWords.Add(wordToAdd);
            foreach (string ending in commonEndings)
            {
                currentWords.Remove(wordToAdd + ending);
            }

            foreach (string ending in commonEndingWithDoubledLetters)
            {
                currentWords.Remove(wordToAdd + wordToAdd[wordToAdd.Length-1] + ending);
            }

            foreach (string ending in commonEndingsWithY)
            {
                currentWords.Remove(wordToAdd.Substring(0, wordToAdd.Length - 1) + ending);
            }

            foreach (string ending in commonEndingsWithE)
            {
                currentWords.Remove(wordToAdd.Substring(0, wordToAdd.Length - 1) + ending);
            }

        }

    }
}