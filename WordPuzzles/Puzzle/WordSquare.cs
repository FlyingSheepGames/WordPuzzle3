using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using WordPuzzles.Puzzle.Legacy;
using WordPuzzles.Utility;

namespace WordPuzzles.Puzzle
{
    public class WordSquare : IPuzzle
    {
        // ReSharper disable once UnusedMember.Global
        public bool IsWordSquare = true;
        public WordRepository Repository { get; set; } = new WordRepository();
        // ReSharper disable once InconsistentNaming
        private static readonly string BASE_DIRECTORY = ConfigurationManager.AppSettings["BaseDirectory"]; //@"E:\utilities\WordSquare\data\";
        private readonly HtmlGenerator _htmlGenerator = new HtmlGenerator();
        public string[] Lines ;
        public int Size;

        public WordSquare() :this("")
        {
        }
        public WordSquare(string diagonalWord)
        {
            if (String.IsNullOrWhiteSpace(diagonalWord))
            {
                diagonalWord = "_____";
            }
            Size = diagonalWord.Length;
            Lines = new string[Size];
            Clues = new string[Size];
            for (int lineIndex = 0; lineIndex < Size; lineIndex++)
            {
                Lines[lineIndex] = String.Concat(new string('_', lineIndex), diagonalWord[lineIndex], new string('_', Size - (lineIndex+ 1)));
                Clues[lineIndex] = "";
            }

        }


        public WordSquare(WordSquare source)
        {
            Size = source.Size;
            Lines = new string[Size];
            Array.Copy(source.Lines, Lines, source.Lines.Length);
            Repository = source.Repository;
        }

        public override string ToString()
        {
            return String.Join(Environment.NewLine, Lines); 
        }


        public void SetWordAtIndex(string wordToSet, int indexToSet)
        {
            for (int lineIndex = 0; lineIndex < Lines.Length; lineIndex++)
            {
                Lines[lineIndex] = String.Concat(Lines[lineIndex].Substring(0, indexToSet), wordToSet[lineIndex],
                    Lines[lineIndex].Substring(indexToSet + 1));
            }

            Lines[indexToSet] = wordToSet;
        }

        public List<string> GetWordCandidates(int index)
        {
            return Repository.WordsStartingWith(Lines[index].Trim('_'), Size);
        }


        public bool IsLastLineAWord()
        {
            string lastLine = Lines[Lines.Length -1];
            return Repository.IsAWord(lastLine);
        }

        public static string GetFileNameFor(string relatedWord)
        {
            // ReSharper disable StringLiteralTypo
            return BASE_DIRECTORY + $@"wordsquares\{relatedWord}.txt";
            // ReSharper restore StringLiteralTypo
        }

        public string GetTweet()
        {
            StringBuilder tweetBuilder = new StringBuilder();
            if (Theme != null && Theme.StartsWith("#"))
            {
                tweetBuilder.AppendLine(Theme);
            }
            tweetBuilder.AppendLine("#MagicWordSquare");
            tweetBuilder.AppendLine();
            tweetBuilder.Append(Lines[0][0].ToString().ToUpper());
            tweetBuilder.AppendLine("****");
            tweetBuilder.AppendLine("*****");
            tweetBuilder.AppendLine("*****");
            tweetBuilder.AppendLine("*****");
            tweetBuilder.AppendLine("*****");
            tweetBuilder.AppendLine();
            tweetBuilder.AppendLine("Top word is part of this week's theme!");
            tweetBuilder.AppendLine("Remaining (unordered) clues:");

            List<string> listOfClues = new List<string> {Clues[1], Clues[2], Clues[3], Clues[4]};
            //skip top clue [0]

            listOfClues.Sort();

            foreach (string clue in listOfClues)
            {
                tweetBuilder.AppendLine(clue);
            }

            tweetBuilder.AppendLine();
            tweetBuilder.AppendLine("#HowToPlay: https://t.co/rSa0rUCvRC");
            return tweetBuilder.ToString();
        }

        public string[] Clues { get; set; }
        public string Theme { get; set; }

        public static List<WordSquare> ReadAllWordSquaresFromFile(string fileWithMagicWordSquares, int size = 5)
        {
            var allWordSquaresFromFile = new List<WordSquare>();
            string[] lines = File.ReadAllLines(fileWithMagicWordSquares);
            int lineCount = lines.Length;
            int linesInFilePerSquare = size +1;
            int availableSquareCount = lineCount / linesInFilePerSquare;
            for (int squareIndex = 0; squareIndex < availableSquareCount; squareIndex++)
            {
                var squareToAdd = new WordSquare(new string('_', size));
                for (int offset = 0; offset < size; offset++)
                {
                    string lineToSet = lines[(squareIndex * linesInFilePerSquare) + 1 + offset];
                    squareToAdd.SetWordAtIndex(lineToSet, offset);
                }
                allWordSquaresFromFile.Add(squareToAdd);
            }


            return allWordSquaresFromFile;
        }

        public string FormatForGoogle()
        {
            string tabs = new string('\t', Size);
            StringBuilder builder = new StringBuilder();
            foreach (string clue in Clues)
            {
                builder.Append(clue);
                builder.Append(tabs);
                builder.AppendLine();
            }
            return builder.ToString();
        }

        public string FormatHtmlForGoogle(bool includeSolution = false, bool isFragment = false)
        {
            StringBuilder builder = new StringBuilder();
            if (!isFragment)
            {
                _htmlGenerator.AppendHtmlHeader(builder);
            }


            builder.AppendLine("<!--StartFragment-->");
            builder.AppendLine(
                @"Use the clues below to fill in the grid. Each horizontal word also appears vertically (in the same order).");
            builder.AppendLine(
                @"Then read the solution to the puzzle from the highlighted squares.");
            builder.AppendLine(@"<table border=""1"">");
            for (var rowIndex = 0; rowIndex < Clues.Length; rowIndex++)
            {
                var clue = Clues[rowIndex];
                if (rowIndex == 0)
                {
                    clue = "&nbsp;";
                }
                builder.AppendLine("\t<tr>");
                builder.AppendLine($@"		<td width=""250"">{clue}</td>");
                for (int i = 0; i < Size; i++)
                {
                    string letterInWord = @"&nbsp;";
                    string style = @"class=""normal""";
                    if (includeSolution)
                    {
                        letterInWord = Lines[rowIndex][i].ToString().ToUpperInvariant();
                    }
                    if (rowIndex == 0)
                    {
                        style = @"class=""bold""";
                        if (i == 0)
                        {
                            letterInWord = Lines[rowIndex][i].ToString().ToUpperInvariant();
                        }
                    }

                    builder.AppendLine(@"		<td width=""20"" " + style + ">" + letterInWord + @"</td>");
                }

                builder.AppendLine("\t</tr>");
            }

            builder.AppendLine("</table>");
            builder.AppendLine("<!--EndFragment-->");
            if (!isFragment)
            {
                builder.AppendLine("</body>");
                builder.AppendLine("</html>");
            }

            //reset 
            return builder.ToString();
        }

        public string Description
        {
            get
            {
                string line = "No first line.";
                if (Lines.Length > 0)
                {
                    line = Lines[0];
                }
                return "Word Square: " + line;
            }
        }

        public WordPuzzleType Type { get; } = WordPuzzleType.WordSquare;

        public List<string> GetClues()
        {
            return Clues.ToList();
        }

        public void ReplaceClue(string clueToReplace, string newClue)
        {
            for (var index = 0; index < Clues.Length; index++)
            {
                var clue = Clues[index];
                if (clue == clueToReplace)
                {
                    Clues[index] = newClue;
                }
            }
        }
    }
}