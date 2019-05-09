using System;
using System.Collections.Generic;
using System.Text;
using WordPuzzles;

namespace WordDrMario
{
    internal class WordGrid
    {
        readonly WordRepository repository = new WordRepository();
        internal string[] Lines;
        private readonly int Width;
        private readonly int Height = 6;

        public WordGrid(int width)
        {
            Width = width;
            Lines = new string[Height];
            for (int i = 0; i < Height; i++)
            {
                Lines[i] = new string(' ', Width);
            }

            FoundWords = new List<WordLocation>();
            Verbose = false;
        }

        public List<WordLocation> FoundWords { get; set; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            foreach (string line in Lines)
            {
                builder.Append("|");
                builder.Append(line);
                builder.Append("|");
                builder.AppendLine();
            }

            builder.Append("_");
            builder.Append('_', Lines[0].Length);
            builder.Append("_");
            builder.AppendLine();

            return builder.ToString();
        }


        public bool DropLetter(char letter, int columnIndex)
        {
            int rowToPlaceLetter = Height - 1; //assume it goes to the bottom.

            for (int rowIndex = 0; rowIndex < Height; rowIndex++)
            {
                if (Lines[rowIndex][columnIndex] != ' ')
                {
                    rowToPlaceLetter = rowIndex - 1;
                    break;
                }
            }

            if (rowToPlaceLetter < 0)
            {
                return false;
            }

            Lines[rowToPlaceLetter] = SetCharacterAtIndex(Lines[rowToPlaceLetter], columnIndex, letter);
            return true;
        }

        private string SetCharacterAtIndex(string originalString, int index, char charater)
        {
            char[] array = originalString.ToCharArray();
            array[index] = charater;
            return new string(array);
        }

        public bool FindHorizontalWords()
        {
            bool foundAtLeastOneWord = false;
            for (int row = 0; row < Height; row++)
            {
                for (int column = 0; column <= Width - 3; column++)
                {
                    for (int length = Width - column; 3 <= length; length--)
                    {
                        var wordCandidate = Lines[row].Substring(column, length).ToLower();
                        if (repository.IsAWord(wordCandidate))
                        {
                            if (Verbose)
                            {
                                Console.WriteLine($"{row} {column} {length} {wordCandidate} is a word.");
                            }

                            WordLocation locationToAdd = new WordLocation()
                            {
                                Column = column,
                                Row = row,
                                Length = length,
                                Word = wordCandidate
                            };
                            if (!IsAlreadyIncluded(FoundWords, locationToAdd))
                            {
                                FoundWords.Add(locationToAdd);
                            }

                            foundAtLeastOneWord = true;
                            break; //don't look for substrings in this column. 
                        }

                        if (Verbose)
                        {
                            Console.WriteLine($"{row} {column} {length} {wordCandidate} is NOT a word.");
                        }
                    }
                }
            }

            return foundAtLeastOneWord;
        }

        private bool IsAlreadyIncluded(List<WordLocation> foundWords, WordLocation locationToAdd)
        {
            foreach (var location in foundWords)
            {
                if (location.Word == locationToAdd.Word &
                    location.Column == locationToAdd.Column &
                    location.Row == locationToAdd.Row)
                {
                    return true;
                }
            }

            return false;
        }

        public bool Verbose { get; set; }

        public void WriteToConsole(int scoringAnimationFrame = 0)
        {
            string[] INSTRUCTIONS = new[]
            {
                "CONTROLS",
                "^ Up arrow     : Rotate letter pair.",
                "<- Left arrow  : Move letter pair left.",
                "-> Right arrow : Move letter pair right.",
                "V Down arrow   : Drop letter pair. (also spacebar)",
                "S Score        : Green (1 pt), Dark green (3 pt)"
            };

            for (int row = 0; row < Height; row++)
            {
                Console.Write("|");
                for (int column = 0; column < Width; column++)
                {
                    Console.ForegroundColor = ConsoleColor.Gray;
                    bool isPartOfHorizontalWord = false;
                    bool isPartOfVericalWord = false;
                    foreach (var wordPosition in FoundWords)
                    {
                        if (wordPosition.Horizontal)
                        {
                            if (wordPosition.Row == row)
                            {
                                if (wordPosition.Column <= column &&
                                    column < wordPosition.Column + wordPosition.Length)
                                {
                                    isPartOfHorizontalWord = true;
                                }
                            }

                        }
                        else
                        {
                            if (wordPosition.Column == column)
                            {
                                if (wordPosition.Row <= row &&
                                    row < wordPosition.Row + wordPosition.Length)
                                {
                                    isPartOfVericalWord = true;
                                }
                            }
                        }
                    }

                    if (isPartOfVericalWord || isPartOfHorizontalWord)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                    }

                    if (isPartOfVericalWord && isPartOfHorizontalWord)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                    }

                    char letterToDisplay = Lines[row][column];
                    if (isPartOfVericalWord || isPartOfHorizontalWord)
                    {
                        switch (scoringAnimationFrame)
                        {
                            case 1:
                                letterToDisplay = '*';
                                break;
                            case 2:
                                letterToDisplay = '.';
                                break;
                            case 3:
                                letterToDisplay = ' ';
                                break;
                        }
                    }

                    Console.Write(letterToDisplay);
                }

                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write("|");
                Console.Write("\t");
                Console.Write(INSTRUCTIONS[row]);
                Console.WriteLine();
            }

            for (int column = 0; column < Width + 2; column++)
            {
                Console.Write('-');
            }

        }

        public int CalculateScore()
        {
            int score = 0;
            for (int row = 0; row < Height; row++)
            {
                for (int column = 0; column < Width; column++)
                {
                    bool isPartOfHorizontalWord = false;
                    bool isPartOfVericalWord = false;
                    foreach (var wordPosition in FoundWords)
                    {
                        if (wordPosition.Horizontal)
                        {
                            if (wordPosition.Row == row)
                            {
                                if (wordPosition.Column <= column &&
                                    column < wordPosition.Column + wordPosition.Length)
                                {
                                    isPartOfHorizontalWord = true;
                                }
                            }

                        }
                        else
                        {
                            if (wordPosition.Column == column)
                            {
                                if (wordPosition.Row <= row &&
                                    row < wordPosition.Row + wordPosition.Length)
                                {
                                    isPartOfVericalWord = true;
                                }
                            }
                        }
                    }

                    if (isPartOfVericalWord || isPartOfHorizontalWord)
                    {
                        score++;
                    }

                    if (isPartOfVericalWord && isPartOfHorizontalWord)
                    {
                        score += 2; //overlapping letters are worth 3 (1 + 2)
                    }

                }
            }

            return score;
        }

        public bool? FindVerticalWords()
        {
            bool foundAtLeastOneWord = false;
            for (int columnIndex = 0; columnIndex < Width; columnIndex++)
            {
                StringBuilder columnBuilder = new StringBuilder();
                for (int row = 0; row < Height; row++)
                {
                    columnBuilder.Append(Lines[row][columnIndex]);
                }

                string column = columnBuilder.ToString().ToLower();
                //Does this string contains a word? 
                for (int length = 6; 3 <= length; length--)
                {
                    for (int row = 0; row <= Height - 3; row++)
                    {
                        if (6 < row + length) break;
                        string wordCandidate = column.Substring(row, length);
                        if (repository.IsAWord(wordCandidate))
                        {
                            WordLocation locationToAdd = new WordLocation()
                            {
                                Column = columnIndex,
                                Horizontal = false,
                                Length = length,
                                Row = row,
                                Word = wordCandidate
                            };
                            if (!IsAlreadyIncluded(FoundWords, locationToAdd))
                            {
                                FoundWords.Add(locationToAdd);
                            }

                            foundAtLeastOneWord = true;
                            break; //don't look for a shorter word.
                        }

                        if (Verbose)
                        {
                            Console.WriteLine($"{wordCandidate} is not a word.");
                        }
                    }

                }

            }

            return foundAtLeastOneWord;
        }

        public void DropAllLetters()
        {
            string[] LettersToDrop = new string[Width];
            for (int columnIndex = 0; columnIndex < Width; columnIndex++)
            {
                //start from the bottom. 
                List<char> LettersToDropList = new List<char>();

                for (int rowIndex = Height - 1; 0 <= rowIndex; rowIndex--)
                {
                    LettersToDropList.Add(Lines[rowIndex][columnIndex]);
                }

                LettersToDrop[columnIndex] = string.Join("", LettersToDropList);
            }

            Clear();
            for (int columnIndex = 0; columnIndex < Width; columnIndex++)
            {
                foreach (char letter in LettersToDrop[columnIndex])
                {
                    if (letter == ' ') continue;
                    DropLetter(letter, columnIndex);
                }
            }

        }

        public void Clear()
        {
            for (var index = 0; index < Lines.Length; index++)
            {
                Lines[index] = new string(' ', Width);
            }

            FoundWords = new List<WordLocation>();
        }

        public void DeleteFoundWords()
        {
            for (int row = 0; row < Height; row++)
            {
                for (int column = 0; column < Width; column++)
                {
                    bool isPartOfHorizontalWord = false;
                    bool isPartOfVericalWord = false;
                    foreach (var wordPosition in FoundWords)
                    {
                        if (wordPosition.Horizontal)
                        {
                            if (wordPosition.Row == row)
                            {
                                if (wordPosition.Column <= column &&
                                    column < wordPosition.Column + wordPosition.Length)
                                {
                                    isPartOfHorizontalWord = true;
                                }
                            }

                        }
                        else
                        {
                            if (wordPosition.Column == column)
                            {
                                if (wordPosition.Row <= row &&
                                    row < wordPosition.Row + wordPosition.Length)
                                {
                                    isPartOfVericalWord = true;
                                }
                            }
                        }
                    }

                    if (isPartOfVericalWord || isPartOfHorizontalWord)
                    {
                        Lines[row] = SetCharacterAtIndex(Lines[row], column, ' ');
                    }
                }
            }
            FoundWords = new List<WordLocation>();
        }
    }

    internal class WordLocation
    {
        public int Column;
        public int Row;
        public int Length;
        public string Word;
        public bool Horizontal = true;
    }
}