using System;
using System.Collections.Generic;
using System.Text;

namespace WordPuzzles
{

    public class LettersAndArrowsPuzzle : IPuzzle
    {
        private Random _randomNumberGenerator;
        public int Size { get; set; }
        private readonly Dictionary<string, LetterAndArrowCell> _grid = new Dictionary<string, LetterAndArrowCell>();
        private HtmlGenerator _htmlGenerator = new HtmlGenerator();

        public LettersAndArrowsPuzzle(int size)
        {
            Size = size;
            InitializeGrid();
            _clues = new string[size];
        }

        public WordRepository Repository;

        private void InitializeGrid()
        {
            for (int rowIndex = 0; rowIndex < Size; rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < Size; columnIndex++)
                {
                    _grid.Add(string.Concat(rowIndex, columnIndex), LetterAndArrowCell.EmptyCell);
                }
            }
        }

        public LettersAndArrowsPuzzle(string solution, bool rowsMustFormWords = false, int sizeOverride = 0, int randomSeed = 0)
        {
            RandomSeed = randomSeed;
            RowsMustFormWords = rowsMustFormWords;
            if (RowsMustFormWords)
            {
                Size = 4;
                Repository = new WordRepository() { ExludeAdvancedWords = true };
            }
            else
            {
                Size = CalculateSizeBasedOnSolutionLength(solution.Length);
                Repository = new WordRepository() {ExludeAdvancedWords = true};
            }

            if (sizeOverride != 0)
            {
                Size = sizeOverride;
            }
            InitializeGrid();
            _clues = new string[Size];
            PlaceSolution(solution);
            FillEmptyCells();
        }

        public bool RowsMustFormWords { get; set; }
        public int RandomSeed { get; set; }

        internal Random RandomNumberGenerator
        {
            get
            {
                if (_randomNumberGenerator == null)
                {
                    if (RandomSeed == 0)
                    {
                        _randomNumberGenerator = new Random();
                    }
                    else
                    {
                        _randomNumberGenerator = new Random(RandomSeed);
                    }
                }
                return _randomNumberGenerator;
            }
        }

        private int CalculateSizeBasedOnSolutionLength(int solutionLength)
        {
            if (solutionLength < 8) return 3;
            if (solutionLength < 13) return 4;
            if (solutionLength < 19) return 5;
            if (solutionLength < 25) return 6;
            if (solutionLength < 32) return 7;
            throw new ArgumentException($"Solution is too long at {solutionLength} characters. Must be less than 32 characters.");
        }

        public LetterAndArrowCell GetCellAtCoordinates(int row, int column)
        {
            return _grid[string.Concat(row, column)];
        }

        public List<int> GetAvailableHorizontalCells(int row, int column, char letterToInsert = '_')
        {
            var availableHorizontalCells = new List<int>();
            int horizontalOffset = 0 - column; //the lowest possible offset is "column" steps to the left (negative)
            for (int offset = 0; offset < Size; offset++)
            {
                int columnToCheck = column + horizontalOffset;
                if (columnToCheck != column)
                {
                    if (Equals(GetCellAtCoordinates(row, columnToCheck), LetterAndArrowCell.EmptyCell))
                    {
                        if (!RowsMustFormWords)
                        {
                            availableHorizontalCells.Add(horizontalOffset);
                        }
                        else //check to make sure that this letter can go in this spot to make a word.
                        {
                            StringBuilder wordPattern = new StringBuilder();
                            for (int columnOfNewGrid = 0; columnOfNewGrid < Size; columnOfNewGrid++)
                            {
                                if (columnOfNewGrid == columnToCheck)
                                {
                                    wordPattern.Append(letterToInsert);
                                }
                                else
                                {
                                    var letter = GetCellAtCoordinates(row, columnOfNewGrid).Letter;
                                    if (letter == ' ')
                                    {
                                        letter = '_';
                                    }
                                    wordPattern.Append(letter);
                                }
                            }

                            var pattern = wordPattern.ToString().ToLower();
                            var wordsMatchingPattern = Repository.WordsMatchingPattern(pattern);

                            if (0 < wordsMatchingPattern.Count)
                            {
                                Console.WriteLine($"At least one word (e.g. {wordsMatchingPattern[0]}) matches {pattern}, so {horizontalOffset} is valid in row {row}.");
                                availableHorizontalCells.Add(horizontalOffset);
                            }
                            else
                            {
                                Console.WriteLine($"No words match {pattern}, skipping {horizontalOffset} in row {row}.");
                            }
                        }
                    }
                }
                horizontalOffset++;
            }

            return availableHorizontalCells;
        }

        public List<int> GetAvailableVerticalCells(int row, int column, char letterToInsert = '_')
        {
            var availableVerticalCells = new List<int>();
            int verticalOffset = 0 - row; //At most it can be "row" steps up (negative). 
            for (int offset = 0; offset < Size; offset++)
            {
                int rowToCheck = row + verticalOffset;
                if (rowToCheck != row)
                {
                    if (Equals(GetCellAtCoordinates(rowToCheck, column), LetterAndArrowCell.EmptyCell))
                    {
                        if (!RowsMustFormWords)
                        {
                            availableVerticalCells.Add(verticalOffset);
                        }
                        else //check to make sure that this letter can go in this spot to make a word.
                        {
                            StringBuilder wordPattern = new StringBuilder();
                            for (int columnOfNewGrid = 0; columnOfNewGrid < Size; columnOfNewGrid++)
                            {
                                if (columnOfNewGrid == column)
                                {
                                    wordPattern.Append(letterToInsert);
                                }
                                else
                                {
                                    var letter = GetCellAtCoordinates(rowToCheck, columnOfNewGrid).Letter;
                                    if (letter == ' ')
                                    {
                                        letter = '_';
                                    }
                                    wordPattern.Append(letter);
                                }
                            }

                            var pattern = wordPattern.ToString().ToLower();
                            if (0 < Repository.WordsMatchingPattern(pattern).Count)
                            {
                                availableVerticalCells.Add(verticalOffset);
                            }
                        }
                    }
                }
                verticalOffset++;
            }

            return availableVerticalCells;
        }

        public void SetCellAtCoordinates(int row, int column, LetterAndArrowCell cell)
        {
            _grid[string.Concat(row, column)] = cell;
            if (!RowsVisited.Contains(row))
            {
                RowsVisited.Add(row);
            }
        }

        public void PlaceSolution(string solution)
        {
            Solution = solution;
            int currentRow = 0;
            int currentColumn = 0;

            var solutionLength = solution.Length;
            for (var index = 0; index < solutionLength; index++)
            {
                var nextRowOffset = 0;
                var nextColumnOffset = 0;
                var nextDirection = Direction.Undefined;
                char letter = solution.ToUpper()[index];
                char nextLetter = '_';
                if (solution.Length > index + 1)
                {
                    nextLetter = solution.ToLower()[index + 1];
                }
                if (index + 1 != solutionLength) //if we're not at the last letter...
                {
                    //Put a placeholder (without direction, but with the letter), so we can figure out if the next letter will fit. 
                    SetCellAtCoordinates(currentRow, currentColumn, new LetterAndArrowCell() {Letter = letter});

                        //Figure out if we're going horizontal or vertical.
                    var verticalOptions = GetAvailableVerticalCells(currentRow, currentColumn, nextLetter);
                    var horizontalOptions = GetAvailableHorizontalCells(currentRow, currentColumn, nextLetter);

                    var verticalOptionsCount = verticalOptions.Count;

                    var horizontalOptionsCount = horizontalOptions.Count;
                    if (verticalOptionsCount + horizontalOptionsCount == 0)
                    {
                        //TODO Come up with a cleaner solution to this, or prevent it from occurring completely.
                        Console.WriteLine("About to hit exception.");
                        Console.WriteLine(FormatHtmlForGoogle());
                    }
                    if (verticalOptionsCount < horizontalOptionsCount) //only go horizontal if there are fewer vertical options.
                    {
                        nextColumnOffset = horizontalOptions[RandomNumberGenerator.Next(horizontalOptionsCount)];
                        if (0 < nextColumnOffset)
                        {
                            nextDirection = Direction.Right;
                        }
                        else
                        {
                            nextDirection = Direction.Left;
                        }
                    }
                    else
                    {
                        int selectedVerticalOption = SelectVerticalOptionToMaximizeRowsVisited(verticalOptions, currentRow);
                        nextRowOffset = verticalOptions[selectedVerticalOption];
                        if (0 < nextRowOffset)
                        {
                            nextDirection = Direction.Down;
                        }
                        else
                        {
                            nextDirection = Direction.Up;
                        }
                    }

                }
                SetCellAtCoordinates(currentRow, currentColumn, new LetterAndArrowCell() {
                    Letter = letter,
                    Direction = nextDirection,
                    Number = nextRowOffset + nextColumnOffset //One of these will be zero. It may be negative.
                });
                currentRow += nextRowOffset;
                currentColumn += nextColumnOffset;
            }
        }

        private int SelectVerticalOptionToMaximizeRowsVisited(List<int> verticalOptions, int currentRow)
        {
            List<int> offsetIndiciesThatVisitANewRow = new List<int>();
            for (var index = 0; index < verticalOptions.Count; index++)
            {
                int offset = verticalOptions[index];
                if (!HaveAlreadyVisitedRow(currentRow + offset))
                {
                    offsetIndiciesThatVisitANewRow.Add(index);
                }
            }

            if (offsetIndiciesThatVisitANewRow.Count == 0) //none of the options will increase the number of rows visited. Pick a random one.
            {
                return RandomNumberGenerator.Next(verticalOptions.Count);
            }
            else
            {
                //randomly select among the ones that will visit a new row.
                return offsetIndiciesThatVisitANewRow[RandomNumberGenerator.Next(offsetIndiciesThatVisitANewRow.Count)]; 
            }
        }

        private bool HaveAlreadyVisitedRow(int row)
        {
            return RowsVisited.Contains(row);
        }

        public List<int> RowsVisited = new List<int>();
        private string Solution = "";
        private string[] _clues;
        public string FormatHtmlForGoogle(bool includeSolution = false, bool isFragment = false)
        {
            StringBuilder builder = new StringBuilder();
            if (!isFragment)
            {
                _htmlGenerator.AppendHtmlHeader(builder);
            }

            builder.AppendLine("<!--StartFragment-->");
            builder.AppendLine(@"Fill in the words below (one letter per box) based on the clues. ");
            builder.AppendLine("Starting in the top left box, follow the direction (e.g. three spaces to the right) to find the next letter. ");
            builder.AppendLine(@"<table border=""1"">");
            for (int row = 0; row < Size; row++)
            {
                builder.AppendLine("<tr>");
                StringBuilder wordRow = new StringBuilder();
                for (int column = 0; column < Size; column++)
                {
                    wordRow.Append(GetCellAtCoordinates(row, column).Letter);
                }

                string clueForThisRow = $@"Clue for {wordRow}";
                if (!string.IsNullOrWhiteSpace(_clues[row]))
                {
                    clueForThisRow = _clues[row];
                }
                builder.AppendLine($@"    <td width=""250"">" + clueForThisRow + $@"</td>");

                for (int column = 0; column < Size; column++)
                {
                    builder.AppendLine($@"    <td width=""20""><sup>{GetCellAtCoordinates(row, column)}</sup><br/>&nbsp;</td>");
                }
                builder.AppendLine("</tr>");
            }
            builder.AppendLine("</table>");
            builder.Append("<h2>");
            builder.Append("Solution: ");
            for (var index = 0; index < Solution.Length; index++)
            {
                builder.Append("_ ");
            }
            builder.Append("</h2>");

            builder.AppendLine();
            builder.AppendLine("<!--EndFragment-->");
            if (!isFragment)
            {
                _htmlGenerator.AppendHtmlFooter(builder);
            }

            return builder.ToString();
        }

        public string Description => "Letters and Arrows: " + Solution;

        public void FillEmptyCells()
        {
            for (int row = 0; row < Size; row++)
            {
                char[] wordForRow = new char[Size];
                if (!RowsMustFormWords)
                {
                    for (int randomIndex = 0; randomIndex < Size; randomIndex++)
                    {
                        wordForRow[randomIndex] = (char)RandomNumberGenerator.Next('A', 'Z');
                    }
                }
                else
                {
                    StringBuilder patternBuilder = new StringBuilder();
                    for (int wordIndex = 0; wordIndex < Size; wordIndex++)
                    {
                        char letter = GetCellAtCoordinates(row, wordIndex).Letter;
                        if (letter == ' ')
                        {
                            letter = '_';
                        }

                        patternBuilder.Append(letter);
                    }

                    string pattern = patternBuilder.ToString().ToLower();
                    var wordsMatchingPattern = Repository.WordsMatchingPattern(pattern);
                    if (wordsMatchingPattern.Count == 0)
                    {
                        //This should not happen
                        Console.WriteLine($"No words match {pattern} in row {row}.");
                        Console.WriteLine(FormatHtmlForGoogle());
                    }
                    string wordForRowAsString = wordsMatchingPattern[RandomNumberGenerator.Next(wordsMatchingPattern.Count)];
                    wordForRow = wordForRowAsString.ToUpper().ToCharArray();
                }

                for (int column = 0; column < Size; column++)
                {
                    if (Equals(GetCellAtCoordinates(row, column), LetterAndArrowCell.EmptyCell))
                    {
                        char randomLetter = wordForRow[column]; // (char) random.Next( (int) 'A',  (int) 'Z');
                        int spacesBelowMe = (Size - row) - 1;
                        int spacesAboveMe = Size - (spacesBelowMe + 1);
                        int spacesToTheRight = (Size - column) - 1;
                        int spacesToTheLeft = Size - (spacesToTheRight + 1);

                        Direction randomDirection = Direction.Undefined;
                        int randomNumber = 0;
                        switch (RandomNumberGenerator.Next(4))
                        {
                            case 0: //go down if you can, otherwise go up.
                                if (0 < spacesBelowMe)
                                {
                                    randomDirection = Direction.Down;
                                    randomNumber = RandomNumberGenerator.Next(1, spacesBelowMe);
                                }
                                else
                                {
                                    randomDirection = Direction.Up;
                                    randomNumber = RandomNumberGenerator.Next(1, spacesAboveMe);
                                }
                                break;
                            case 1: //go up if you can, otherwise go down.
                                if (0 < spacesAboveMe)
                                {
                                    randomDirection = Direction.Up;
                                    randomNumber = RandomNumberGenerator.Next(1, spacesAboveMe);
                                }
                                else
                                {
                                    randomDirection = Direction.Down;
                                    randomNumber = RandomNumberGenerator.Next(1, spacesBelowMe);
                                }
                                break;
                            case 2: //go right if you can, otherwise go left.
                                if (0 < spacesToTheRight)
                                {
                                    randomDirection = Direction.Right;
                                    randomNumber = RandomNumberGenerator.Next(1, spacesToTheRight);
                                }
                                else
                                {
                                    randomDirection = Direction.Left;
                                    randomNumber = RandomNumberGenerator.Next(1, spacesToTheLeft);
                                }
                                break;
                            case 3: //go left if you can, otherwise go right.
                                if (0 < spacesToTheLeft)
                                {
                                    randomDirection = Direction.Left;
                                    randomNumber = RandomNumberGenerator.Next(1, spacesToTheLeft);
                                }
                                else
                                {
                                    randomDirection = Direction.Right;
                                    randomNumber = RandomNumberGenerator.Next(1, spacesToTheRight);
                                }
                                break;
                        }
                        SetCellAtCoordinates(row, column, new LetterAndArrowCell()
                        {
                            Letter = randomLetter,
                            Direction = randomDirection, 
                            Number = randomNumber,

                        });
                    }
                }
            }
        }

        public List<string> GetWords()
        {
            var wordsToReturn = new List<string>();
            StringBuilder builder = new StringBuilder();
            for (int rowIndex = 0; rowIndex < Size; rowIndex++)
            {
                builder.Clear();
                for (int columnIndex = 0; columnIndex < Size; columnIndex++)
                {
                    builder.Append(GetCellAtCoordinates(rowIndex, columnIndex).Letter);
                }
                wordsToReturn.Add(builder.ToString());
            }
            return wordsToReturn;
        }

        public void SetClueForRowIndex(int rowIndex, string clue)
        {
            _clues[rowIndex] = clue;
        }


    }

    public enum Direction
    {
        Undefined,
        Up, 
        Right, 
        Down, 
        Left
    }
}