using System;
using System.Collections.Generic;
using System.Text;
using WordPuzzles.Utility;

namespace WordPuzzles.Puzzle
{
    public class MultipleCluesPuzzle : IPuzzle
    {

        // ReSharper disable once UnusedMember.Global
        public bool IsMultipleCluesPuzzle = true;
        private Random _randomNumberGenerator;
        public List<WordWithClues> WordsWithClues = new List<WordWithClues>();
        internal int NextClueOrder = 1;
        private string INSTRUCTIONS = @"
Each word below matches at least two numbered clues. The number next to each word is the total of all of the clues to that word. <p>
For example, if clues 1 and 4 describe the same word, that word will appear next to the number 5<p>
After you have filled in all the words, read down the second column to get the solution.<p>";

        public WordRepository Repository { get; set; } = new WordRepository();

        public int RandomGeneratorSeed { get; set; } = 0;
        public Random RandomNumberGenerator
        {
            get
            {
                if (_randomNumberGenerator == null)
                {
                    _randomNumberGenerator = new Random(RandomGeneratorSeed);
                }
                return _randomNumberGenerator;
            }
        }

        public List<string> GetCandidatesForLetter(char secondLetter)
        {
            var candidatesForLetter = new List<string>();
            StringBuilder builder = new StringBuilder();
            for (int wordLength = 2; wordLength <= 7; wordLength++)
            {
                builder.Clear();
                builder.Append('_');
                builder.Append(secondLetter);
                builder.Append('_', wordLength - 2);
                candidatesForLetter.AddRange(
                    Repository.WordsMatchingPattern(builder.ToString()));

            }
            return candidatesForLetter;
        }

        public void AddWordWithClues(string wordToAdd, List<string> clues)
        {
            WordWithClues wordWithClues = new WordWithClues()
            {
                WordText = wordToAdd,
                Clues = new List<OrderedClue>(),
            };
            foreach (var clue in clues)
            {
                wordWithClues.Clues.Add(new OrderedClue()
                {
                    ClueText = clue, 
                    ClueOrder = NextClueOrder++,
                });   
            }
            WordsWithClues.Add(wordWithClues);
        }

        public void ReorderClues(int stackCounter = 0)
        {
            if (3 < stackCounter)
            {
                throw new Exception("Tried to reorder clues at least 3 times.");
            }
            //Swap the order of a clue from two different words. Do this 5 times. 
            if (WordsWithClues.Count < 3) return;

            for (int swapCount = 0; swapCount < 6; swapCount++)
            {
                int firstWordIndex = RandomNumberGenerator.Next(WordsWithClues.Count);
                int secondWordIndex = RandomNumberGenerator.Next(WordsWithClues.Count);
                if (firstWordIndex == secondWordIndex) continue;
                var firstWord = WordsWithClues[firstWordIndex];
                var secondWord = WordsWithClues[secondWordIndex];
                firstWord.Clues.Shuffle(RandomNumberGenerator);
                secondWord.Clues.Shuffle(RandomNumberGenerator);

                int firstWordClueOrder = firstWord.Clues[0].ClueOrder;
                firstWord.Clues[0].ClueOrder = secondWord.Clues[0].ClueOrder;
                secondWord.Clues[0].ClueOrder = firstWordClueOrder;
            }
            //a few checks
            List<int> uniqueSums = new List<int>();
            bool allSumsUnique = true;
            foreach (var word in WordsWithClues)
            {
                if (uniqueSums.Contains(word.SumOfClueOrders))
                {
                    allSumsUnique = false;
                    break;
                }
                uniqueSums.Add(word.SumOfClueOrders);
            }

            if (!allSumsUnique)
            {
                ReorderClues(stackCounter+1);
            }
        }

        public string FormatHtmlForGoogle(bool includeSolution = false, bool isFragment = false)
        {
            HtmlGenerator generator = new HtmlGenerator();
            var builder = new StringBuilder();
            if (!isFragment)
            {
                generator.AppendHtmlHeader(builder);
            }

            builder.AppendLine("<p><h2>Instructions</h2>");
            builder.AppendLine(INSTRUCTIONS);
            //List words
            builder.AppendLine("<p><h2>List of clues</h2>");
            builder.AppendLine("<table>");
            DisplayListOfClues(includeSolution, builder);
            builder.AppendLine("</table>");
            //Show puzzle. 
            builder.AppendLine("<p><h2>List of words</h2>");
            builder.AppendLine("<table>");
            DisplayListOfWords(includeSolution, builder);
            builder.AppendLine("</table>");

            if (!isFragment)
            {
                generator.AppendHtmlFooter(builder);
            }
            return builder.ToString();
        }

        private void DisplayListOfWords(bool includeSolution, StringBuilder builder)
        {
            foreach (var word in WordsWithClues)
            {
                builder.Append("<tr>");
                builder.Append($@"  <td width=""30"">{word.SumOfClueOrders}</td>");
                for (var index = 0; index < word.WordText.ToUpperInvariant().Length; index++)
                {
                    char character = word.WordText.ToUpperInvariant()[index];
                    builder.Append($@"   <td class=""");
                    if (index == 1)
                    {
                        builder.Append($@"bold");
                    }
                    else
                    {
                        builder.Append($@"normal");
                    }

                    builder.Append($@""" width=""30"" >");
                    if (includeSolution)
                    {
                        builder.Append($@"{character}");
                    }

                    builder.AppendLine($@"</td>");
                }

                builder.AppendLine("</tr>");
            }
        }

        private void DisplayListOfClues(bool includeSolution, StringBuilder builder)
        {
            string[] cluesInOrder = new string[NextClueOrder];
            foreach (var word in WordsWithClues)
            {
                foreach (var clue in word.Clues)
                {
                    cluesInOrder[clue.ClueOrder] = clue.ClueText;
                }
            }

            for (int i = 1; i < NextClueOrder; i++)
            {
                builder.AppendLine($@"  <tr><td width=""30"">{i}.</td><td width=""250"">{cluesInOrder[i]}</td></tr>");
            }
        }

        public string Description => $"Multiple Clues Puzzle with {Solution}";
        public string Solution { get; set; }

    }

    public class OrderedClue
    {
        public string ClueText { get; set; }
        public int ClueOrder { get; set; }
    }

    public class WordWithClues
    {
        public string WordText { get; set; }
        public List<OrderedClue> Clues { get; set; }
        public int SumOfClueOrders
        {
            get
            {
                int sum = 0;
                foreach (var clue in Clues)
                {
                    sum += clue.ClueOrder;
                }

                return sum;
            }
        }
    }
}