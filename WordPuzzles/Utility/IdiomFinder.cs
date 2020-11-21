using System;
using System.Collections;
using System.Collections.Generic;
using WordPuzzles.Puzzle;

namespace WordPuzzles.Utility
{
    public class IdiomFinder
    {
        public List<string> FindIdioms()
        {
            var idiomsFound = new List<string>();
            string htmlFromPage =
                new WebRequestUtilityInstance().ReadHtmlPageFromUrl(@"https://7esl.com/english-idioms/");
            foreach (var phrase in htmlFromPage.Split(new[] {"<strong>"}, StringSplitOptions.RemoveEmptyEntries))
            {
                int indexOfEndTag = phrase.IndexOf("</strong>", StringComparison.InvariantCultureIgnoreCase);
                if (indexOfEndTag < 0) continue;
                string idiom = phrase.Substring(0, indexOfEndTag);
                if (idiom.ToLowerInvariant().Contains("idiom")) continue;//There are no idioms with the word "idiom".
                idiom = idiom.Replace("<em>", "");
                idiom = idiom.Replace("</em>", "");

                idiomsFound.Add(idiom);
            }
            return idiomsFound;
        }

        public Dictionary<string, Clue> ExtractCluesFromIdiom(string idiom)
        {
            var extractedClues = new Dictionary<string, Clue>();
            foreach(var word in idiom.Split(' '))
            {
                var wordLength = word.Length;
                if (wordLength < 4) continue;
                var lowercaseWord = word.ToLowerInvariant();
                if (extractedClues.ContainsKey(lowercaseWord)) continue;
                string clueText = idiom.Replace(word, new string('_', wordLength));
                string clueToAdd = lowercaseWord;
                extractedClues.Add(clueToAdd, new Clue()
                {
                    ClueSource = ClueSource.ClueSourceIdiom, 
                    ClueText = clueText
                });
            }
            return extractedClues;
        }
    }
}