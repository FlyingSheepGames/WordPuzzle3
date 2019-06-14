using System;
using System.Collections.Generic;
using System.Text;

namespace WordPuzzles
{
    public class MissingLettersPuzzle
    {
        WordRepository repository = new WordRepository() {ExludeAdvancedWords = true};
        public List<string> Words = new List<string>();
        private string Solution;
        private List<string> DecoyWords = new List<string>();
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
                    wordsContainingHiddenWord.AddRange(repository.WordsMatchingPattern(pattern));
                }
            }
            return wordsContainingHiddenWord;
        }

        public void PlaceSolution(string solution)
        {
            Solution = solution;
            string pattern = new string('_', Solution.Length);
            var decoyCandidates = repository.WordsMatchingPattern(pattern);
            if (Shuffle) { decoyCandidates.Shuffle();}


            var wordsContainingLetters = FindWordsContainingLetters(Solution);
            if (3 < wordsContainingLetters.Count)
            {
                if (Shuffle) { wordsContainingLetters.Shuffle(); }
                Words.Add(wordsContainingLetters[0]);
                Words.Add(wordsContainingLetters[1]);
                Words.Add(wordsContainingLetters[2]);
            }

            foreach (string candidate in decoyCandidates)
            {
                if (candidate == Solution) continue;
                if (DecoyWords.Contains(candidate)) continue;

                var wordsContainingLettersForCandidate = FindWordsContainingLetters(candidate);
                if (2 < wordsContainingLettersForCandidate.Count)
                {
                    DecoyWords.Add(candidate);
                    if (Shuffle) { wordsContainingLettersForCandidate.Shuffle();}
                    Words.Add(wordsContainingLettersForCandidate[0]);
                    Words.Add(wordsContainingLettersForCandidate[1]);
                }

                if (6 < Words.Count) break;
            }
            if (Shuffle) { Words.Shuffle();}
        }

        private void AddWords(string substring, int count)
        {
            var wordsContainingLetters = FindWordsContainingLetters(substring);
            if (Shuffle) { wordsContainingLetters.Shuffle();}
            for (int i = 0; i < count; i++)
            {
                Words.Add(wordsContainingLetters[i]);
            }
        }

        public string FormatHtmlForGoogle()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine(@"<html>");
builder.AppendLine(@"<body>");
            builder.AppendLine(@"<!--StartFragment-->");
            builder.AppendLine($@"Fill in the blanks below with {Solution.Length} letter words. The word that you use three times is the solution to the puzzle.<br>");
            string emptyPattern = new string('_', Solution.Length);
            foreach (string word in Words)
            {
                string maskedWord = word.Replace(Solution, emptyPattern);
                if (!maskedWord.Contains("_"))
                {
                    maskedWord = word.Replace(DecoyWords[0], emptyPattern);
                }

                if (!maskedWord.Contains("_"))
                {
                    maskedWord = word.Replace(DecoyWords[1], emptyPattern);
                }
                
                builder.AppendLine($"{maskedWord.ToUpper()}<br>");
            }

            builder.Append(@"Solution: ");
            for (int i = 0; i < Solution.Length; i++)
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