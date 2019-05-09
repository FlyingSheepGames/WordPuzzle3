﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text;

namespace WordPuzzles
{
    public class WordSquare
    {
        public WordRepository Repository { get; set; } = new WordRepository();
        private static string BASE_DIRECTORY = ConfigurationManager.AppSettings["BaseDirectory"]; //@"E:\utilities\WordSquare\data\";

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

        public string LastLine => Lines[Size - 1];

        public override string ToString()
        {
            return String.Join(Environment.NewLine, Lines); 
        }



        public void SetFirstLine(string firstLineWord)
        {
            SetWordAtIndex(firstLineWord, 0);
            return;
            /*
            lines[0] = firstLineWord;
            lines[1] = firstLineWord[1] + lines[1].Substring(1);

            lines[2] = firstLineWord[2] + lines[2].Substring(1);

            lines[3] = firstLineWord[3] + lines[3].Substring(1);
            
            lines[4] = firstLineWord[4] + lines[4].Substring(1);
            */
        }

        public void SetSecondLine(string secondLineWord)
        {
            SetWordAtIndex(secondLineWord, 1);
            return;
            /*
            lines[1] = secondLineWord;
            lines[2] = string.Concat(lines[2][0], secondLineWord[2], lines[2].Substring(2));
            lines[3] = string.Concat(lines[3][0], secondLineWord[3], lines[3].Substring(2));
            lines[4] = string.Concat(lines[4][0], secondLineWord[4], lines[4].Substring(2));
            */
        }

        public void SetThirdLine(string thirdLineWord)
        {
            int indexToSet = 2;

            SetWordAtIndex(thirdLineWord, indexToSet);
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

        public void SetFourthLine(string fourthLineWord)
        {
            SetWordAtIndex(fourthLineWord, 3);
            return;
            /*
            lines[3] = fourthLineWord;
            lines[4] = string.Concat(lines[4].Substring(0, 3), fourthLineWord[4], lines[4].Substring(4));
            */
        }

        public List<string> GetFirstWordCandidates()
        {
            return GetWordCandidates(0);
        }

        public List<string> GetWordCandidates(int index)
        {
            return Repository.WordsStartingWith(Lines[index].Trim('_'), Size);
        }

        public IEnumerable<string> GetSecondWordCandidates()
        {
            return GetWordCandidates(1);
        }

        public IEnumerable<string> GetThirdWordCandidates()
        {
            return GetWordCandidates(2);
        }

        public IEnumerable<string> GetFourthWordCandidates()
        {
            return GetWordCandidates(3);
        }

        public IEnumerable<string> GetFifthWordCandidates()
        {
            return GetWordCandidates(4);
        }

        public void SetFifthLine(string fifthWordCandidate)
        {
            SetWordAtIndex(fifthWordCandidate, 4);
            return;
            //lines[4] = fifthWordCandidate;
        }

        public bool IsLastLineAWord()
        {
            string lastLine = Lines[Lines.Length -1];
            return Repository.IsAWord(lastLine);
        }

        public static string GetFileNameFor(string relatedWord)
        {
            return BASE_DIRECTORY + $@"wordsquares\{relatedWord}.txt";
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

            List<string> listOfClues = new List<string>();
            //skip top clue [0]
            listOfClues.Add(Clues[1]);
            listOfClues.Add(Clues[2]);
            listOfClues.Add(Clues[3]);
            listOfClues.Add(Clues[4]);

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

        public string FormatHtmlForGoogle()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("<html>");
            builder.AppendLine("<body>");
            builder.AppendLine("<!--StartFragment-->");
            builder.AppendLine(
                @"Fill in the grid using the letters provided so that no letter appears twice in the same row or column.");
            builder.AppendLine(@"<table border=""1"">");
            foreach (var clue in Clues)
            {
                builder.AppendLine("\t<tr>");
                builder.AppendLine($"\t\t<td>{clue}</td>");
                for (int i = 0; i < Size; i++)
                {
                    builder.AppendLine($"\t\t<td> </td>");
                }
                builder.AppendLine("\t</tr>");
            }

            builder.AppendLine("</table>");
            builder.AppendLine("<!--EndFragment-->");
            builder.AppendLine("</body>");
            builder.AppendLine("</html>");

            //reset 
            return builder.ToString();
        }
    }
}