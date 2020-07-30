using System.Collections.Generic;
using System.Text;
using WordPuzzles.Utility;

namespace WordPuzzles.Puzzle.Legacy
{
    public class RelatedWordsPuzzle
    {
        WordRepository repository = new WordRepository() {ExludeAdvancedWords = true};
        public bool Shuffle = true;
        public string Solution;
        public void PlaceSolution(string theme, string hiddenWord)
        {
            Solution = hiddenWord;
            List<string> themeWords = repository.GetRelatedWordsForTheme(theme);
            if (Shuffle)
            {
                themeWords.Shuffle();
            }
            foreach (char letter in hiddenWord)
            {
                foreach (string puzzleCandidate in themeWords)
                {
                    if (!puzzleCandidate.Contains(letter.ToString())) continue;
                    if (puzzleCandidate.Contains(hiddenWord)) continue; //Don't include any part of the hidden word.
                    if (Words.Contains(puzzleCandidate)) continue; //don't repeat any words.
                    Words.Add(puzzleCandidate);
                    break;
                }
            }

        }

        public List<string> Words = new List<string>();

        public string FormatHtmlForGoogle()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine(@"<html>");
            builder.AppendLine(@"<body>");
            builder.AppendLine(@"<!--StartFragment-->");
            builder.AppendLine(@"Construct a word that fits in the same category as the words below by taking one letter from each word, in order.<br>");
            foreach (string word in Words)
            {
                builder.AppendLine($"{word.ToUpper()}<br>");
            }
            builder.Append(@"Solution: ");
            for (int i =0; i < Solution.Length; i++)
            {
                builder.Append("_ ");
            }
            builder.AppendLine();
            builder.AppendLine(@"<!--EndFragment-->");
            builder.AppendLine(@"</body>");
            builder.AppendLine(@"</html>");
            return builder.ToString();
        }
    }
}