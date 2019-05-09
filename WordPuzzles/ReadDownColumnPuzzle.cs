using System;
using System.Collections.Generic;
using System.Text;

namespace WordPuzzles
{
    public class ReadDownColumnPuzzle
    {
        private string _solution;
        public List<string> Words = new List<string>();
        public int Size => 6;
        private readonly Random random = new Random();
        public WordRepository Repository => new WordRepository() {ExludeAdvancedWords = true};
        public int NumberOfWordsToInclude => 3;

        public string Solution
        {
            get { return _solution; }
            set { _solution = value.ToLower(); }
        }

        public void PopulateWords()
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < Solution.Length; i++)
            {
                char letterToPlace = Solution[i];
                if (!char.IsLetter(letterToPlace))
                {
                    continue;
                }
                builder.Clear();
                builder.Append('_', 2);
                builder.Append(letterToPlace);
                builder.Append('_', (Size - 3));

                var wordCandidates = Repository.WordsMatchingPattern(builder.ToString());
                StringBuilder selectedWordCanidates = new StringBuilder();
                for (int includedWordCount = 0; includedWordCount < NumberOfWordsToInclude; includedWordCount++)
                {
                    selectedWordCanidates.Append(wordCandidates[random.Next(wordCandidates.Count)]);
                    if (includedWordCount != (NumberOfWordsToInclude - 1))
                    {
                        selectedWordCanidates.Append(", ");
                    }
                }

                Words.Add(selectedWordCanidates.ToString());
            }
        }

        public string FormatHtmlForGoogle()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("<html>");
            builder.AppendLine("<body>");
            builder.AppendLine("<!--StartFragment-->");
            builder.AppendLine("Fill in the clues below, and then read the solution down the third column. ");
            builder.AppendLine(@"<table border=""1"">");
            foreach (string word in Words)
            {
                builder.AppendLine(@"<tr>");
                builder.AppendLine($@"    <td>Clue for {word}</td>");
                for (int i = 0; i < Size; i++)
                {
                    builder.AppendLine(@"    <td> </td>");
                }

                builder.AppendLine(@"</tr>");
            }
            builder.AppendLine("</table>");
            builder.Append(@"Solution: ");
            foreach (char character in Solution)
            {
                if (char.IsLetter(character))
                {
                    builder.Append("_ ");
                    continue;
                }

                if (character == ' ')
                {
                    builder.Append("&nbsp;&nbsp;&nbsp;");
                    continue;
                }

                builder.Append(character);
            }

            builder.AppendLine();
            builder.AppendLine("<!--EndFragment-->");
            builder.AppendLine("</body>");
            builder.AppendLine("</html>");

            return builder.ToString();
        }
    }
}