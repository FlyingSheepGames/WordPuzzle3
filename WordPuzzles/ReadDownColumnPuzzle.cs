using System;
using System.Collections.Generic;
using System.Text;

namespace WordPuzzles
{
    public class ReadDownColumnPuzzle :IPuzzle
    {
        private string _solution;
        public List<string> Words = new List<string>();
        public int Size => 6;
        private readonly Random _random = new Random();
        public int ZeroBasedIndexOfSolution = 2;
        private HtmlGenerator _generator = new HtmlGenerator();
        public WordRepository Repository => new WordRepository() {ExludeAdvancedWords = true};
        public int NumberOfWordsToInclude => 3;

        public string Solution
        {
            get => _solution;
            set => _solution = value.ToLower();
        }

        public void PopulateWords()
        {
            StringBuilder builder = new StringBuilder();
            foreach (var letterToPlace in Solution)
            {
                if (!char.IsLetter(letterToPlace))
                {
                    continue;
                }
                builder.Clear();
                builder.Append('_', ZeroBasedIndexOfSolution);
                builder.Append(letterToPlace);
                builder.Append('_', (Size - (ZeroBasedIndexOfSolution +1)));

                var wordCandidates = Repository.WordsMatchingPattern(builder.ToString());
                StringBuilder selectedWordCanidates = new StringBuilder();
                for (int includedWordCount = 0; includedWordCount < NumberOfWordsToInclude; includedWordCount++)
                {
                    selectedWordCanidates.Append(wordCandidates[_random.Next(wordCandidates.Count)]);
                    if (includedWordCount != (NumberOfWordsToInclude - 1))
                    {
                        selectedWordCanidates.Append(", ");
                    }
                }

                Words.Add(selectedWordCanidates.ToString());
            }
        }

        public string FormatHtmlForGoogle(bool includeSolution = false, bool isFragment = false)
        {
            StringBuilder builder = new StringBuilder();
            if (!isFragment)
            {
                _generator.AppendHtmlHeader(builder);
            }
            
            builder.AppendLine("<!--StartFragment-->");
            var ordinalOfColumnWithSolution = GetOrdinalOfColumnWithSolution();

            builder.AppendLine($"Fill in the clues below, and then read the solution down the {ordinalOfColumnWithSolution} column. ");
            builder.AppendLine(@"<table border=""1"">");
            for (var index = 0; index < Words.Count; index++)
            {
                string word = Words[index];
                builder.AppendLine(@"<tr>");
                string currentClue = $@"Clue for {word}";
                if (!string.IsNullOrWhiteSpace(Clues[index]))
                {
                    currentClue = Clues[index];
                }
                builder.AppendLine($@"    <td width=""250"">" + currentClue + $@"</td>");
                for (int i = 0; i < Size; i++)
                {
                    string style = "normal";
                    if (i == ZeroBasedIndexOfSolution)
                    {
                        style = "bold";
                    }

                    string letterToDisplay = "&nbsp;";
                    if (includeSolution)
                    {
                        letterToDisplay = word[i].ToString().ToUpperInvariant();
                        style += " centered";
                    }
                    builder.AppendLine($@"    <td class=""{style}"" width=""30"">{letterToDisplay}</td>");
                }

                builder.AppendLine(@"</tr>");
            }

            builder.AppendLine("</table>");
            builder.Append(@"Solution: ");
            if (includeSolution)
            {
                builder.AppendLine($"<u>{Solution.ToUpperInvariant()}</u>");
            }
            else
            {
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
            }

            builder.AppendLine();
            builder.AppendLine("<!--EndFragment-->");
            if (!isFragment)
            {
                _generator.AppendHtmlFooter(builder);
            }

            return builder.ToString();
        }

        private string GetOrdinalOfColumnWithSolution()
        {
            if (ZeroBasedIndexOfSolution == (Size - 1))
            {
                return "last";
            }
            switch (ZeroBasedIndexOfSolution)
            {
                case 0: return "first";
                case 1: return "second";
                case 2: return "third";
                case 3: return "fourth";
                case 4: return "fifth";
                case 5: return "sixth";
            }
            throw new Exception("Unexpected ZeroBasedIndexOfSolution.");
        }


        public string Description => $"Read Down Column puzzle {Solution}";
        public List<string> Clues { get; set; }
    }
}