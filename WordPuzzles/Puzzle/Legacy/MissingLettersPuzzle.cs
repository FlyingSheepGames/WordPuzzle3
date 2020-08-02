using System.Collections.Generic;
using System.Text;
using WordPuzzles.Utility;

namespace WordPuzzles.Puzzle.Legacy
{
    public class MissingLettersPuzzle
    {
        readonly WordRepository _repository = new WordRepository() {ExludeAdvancedWords = true};
        public List<string> Words = new List<string>();
        private string _solution;
        private readonly List<string> _decoyWords = new List<string>();
        public bool Shuffle = true;
        public List<string> FindWordsContainingLetters(string wordToHide)
        {
            List<string> wordsContainingHiddenWord = new List<string>();
            StringBuilder builder = new StringBuilder();
            for (int wordLength = wordToHide.Length; wordLength < 7; wordLength++)
            {
                for (int startingIndex = 0; startingIndex < wordLength - wordToHide.Length; startingIndex++)
                {
                    builder.Clear();
                    builder.Append('_', startingIndex);
                    builder.Append(wordToHide);
                    builder.Append('_', wordLength - (wordToHide.Length + startingIndex));
                    string pattern = builder.ToString();
                    //Console.WriteLine(pattern);
                    wordsContainingHiddenWord.AddRange(_repository.WordsMatchingPattern(pattern));
                }
            }
            return wordsContainingHiddenWord;
        }

        public void PlaceSolution(string solution)
        {
            _solution = solution;
            string pattern = new string('_', _solution.Length);
            var decoyCandidates = _repository.WordsMatchingPattern(pattern);
            if (Shuffle) { decoyCandidates.Shuffle();}


            var wordsContainingLetters = FindWordsContainingLetters(_solution);
            if (3 < wordsContainingLetters.Count)
            {
                if (Shuffle) { wordsContainingLetters.Shuffle(); }
                Words.Add(wordsContainingLetters[0]);
                Words.Add(wordsContainingLetters[1]);
                Words.Add(wordsContainingLetters[2]);
            }

            foreach (string candidate in decoyCandidates)
            {
                if (candidate == _solution) continue;
                if (_decoyWords.Contains(candidate)) continue;

                var wordsContainingLettersForCandidate = FindWordsContainingLetters(candidate);
                if (2 < wordsContainingLettersForCandidate.Count)
                {
                    _decoyWords.Add(candidate);
                    if (Shuffle) { wordsContainingLettersForCandidate.Shuffle();}
                    Words.Add(wordsContainingLettersForCandidate[0]);
                    Words.Add(wordsContainingLettersForCandidate[1]);
                }

                if (6 < Words.Count) break;
            }
            if (Shuffle) { Words.Shuffle();}
        }

        public string FormatHtmlForGoogle()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine(@"<html>");
builder.AppendLine(@"<body>");
            builder.AppendLine(@"<!--StartFragment-->");
            builder.AppendLine($@"Fill in the blanks below with {_solution.Length} letter words. The word that you use three times is the solution to the puzzle.<br>");
            string emptyPattern = new string('_', _solution.Length);
            foreach (string word in Words)
            {
                string maskedWord = word.Replace(_solution, emptyPattern);
                if (!maskedWord.Contains("_"))
                {
                    maskedWord = word.Replace(_decoyWords[0], emptyPattern);
                }

                if (!maskedWord.Contains("_"))
                {
                    maskedWord = word.Replace(_decoyWords[1], emptyPattern);
                }
                
                builder.AppendLine($"{maskedWord.ToUpper()}<br>");
            }

            builder.Append(@"Solution: ");
            for (int i = 0; i < _solution.Length; i++)
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