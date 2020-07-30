using System;
using System.Collections.Generic;
using System.Text;
using WordPuzzles.Utility;

namespace WordPuzzles.Puzzle.Legacy
{
    public class BuildingBlocksPuzzle
    {
        WordRepository repository = new WordRepository() {ExludeAdvancedWords = true};
        public int RandomSeed { get; set; }
        public int ColumnContainingSolution { get; set; }

        public Random RandomNumberGenerator
        {
            get
            {
                if (random == null)
                {
                    if (RandomSeed != 0)
                    {
                        random = new Random(RandomSeed);
                    }
                    else
                    {
                        random = new Random();
                    }
                }
                return random;
            }
        }

        public List<string> Words  = new List<string>();

        private Random random; 

        public void PlaceSolution(string solution)
        {
            ColumnContainingSolution = RandomNumberGenerator.Next(2, 5); //Pick a random column (skip the first two). Zero-based
            Solution = solution;
            foreach (char letter in Solution)
            {
                StringBuilder patternBuilder = new StringBuilder();
                patternBuilder.Append('_', ColumnContainingSolution);
                patternBuilder.Append(letter);
                patternBuilder.Append('_', 5 - ColumnContainingSolution);

                var candidatesToAdd = repository.WordsMatchingPattern(patternBuilder.ToString());
                string wordToAdd = candidatesToAdd[RandomNumberGenerator.Next(candidatesToAdd.Count)];
                Words.Add(wordToAdd);
                Blocks.Add(wordToAdd.Substring(2, 2));
                Blocks.Add(wordToAdd.Substring(4, 2));
            }
            Blocks.Sort();
        }

        public string Solution { get; set; }
        public List<string> Blocks = new List<string>();

        public string FormatHtmlForGoogle()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine(@"<html>");
            builder.AppendLine(@"<body>");
            builder.AppendLine(@"<!--StartFragment-->");
            builder.AppendLine(@"Using the letter pairs listed below, complete the words in the grid. ");
            builder.AppendLine(@"To get the solution, read down one of the columns. ");
            builder.AppendLine(@"<table border=""1"">");
            foreach (string word in Words)
            {
                builder.AppendLine(@"<tr>");
                builder.AppendLine($"    <td>{word.Substring(0, 2).ToUpper()}</td>");
                builder.AppendLine(@"    <td> </td>");
                builder.AppendLine(@"    <td> </td>");
                builder.AppendLine(@"</tr>");
            }

            builder.AppendLine(@"</table>");
            builder.AppendLine(@"Available blocks: <br>");
            foreach (string block in Blocks)
            {
                builder.AppendLine($"{block.ToUpper()}<br>");
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