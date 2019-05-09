using System.Collections.Generic;
using System.IO;

namespace WordSquareGenerator
{
    public class WordRepository
    {
        private List<string> words = new List<string>();
        public List<string> LoadAllWords()
        {
            words.AddRange(File.ReadAllLines(@"E:\utilities\WordSquare\data\sgb-words.txt"));
            return words;
        }

        public List<string> WordsStartingWith(string startingCharacters)
        {
            if (0 == words.Count)
            {
                LoadAllWords();
            }
            List<string> matchingWords = new List<string>();
            foreach (string word in words)
            {
                if (word.StartsWith(startingCharacters))
                {
                    matchingWords.Add(word);
                }
            }

            return matchingWords;
        }

        public bool IsAWord(string wordCandidate)
        {
            if (0 == words.Count)
            {
                LoadAllWords();
            }

            return words.Contains(wordCandidate);
        }
    }
}