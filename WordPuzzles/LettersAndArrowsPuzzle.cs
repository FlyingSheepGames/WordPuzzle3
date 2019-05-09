using System;
using System.Collections.Generic;
using System.Text;

namespace WordPuzzles
{

    public class LettersAndArrowsPuzzle
    {
        internal readonly Random RandomNumberGenerator = new Random();
        public int Size { get; set; }
        private readonly Dictionary<string, LetterAndArrowCell> _grid = new Dictionary<string, LetterAndArrowCell>();
        public LettersAndArrowsPuzzle(int size)
        {
            Size = size;
            InitializeGrid();
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

        public LettersAndArrowsPuzzle(string solution, bool rowsMustFormWords = false, int sizeOverride = 0)
        {
            RowsMustFormWords = rowsMustFormWords;
            if (RowsMustFormWords)
            {
                Size = 5;
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
            PlaceSolution(solution);
            FillEmptyCells();
        }

        public bool RowsMustFormWords { get; set; }

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
        }

        public void PlaceSolution(string solution)
        {
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
                        Console.WriteLine("About to hit exception.");
                        Console.WriteLine(FormatHtmlForGoogle());
                    }
                    if (verticalOptionsCount < horizontalOptionsCount) //go horizontal, unless there are more vertical options.
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
                        nextRowOffset = verticalOptions[RandomNumberGenerator.Next(verticalOptionsCount)];
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

        public string FormatHtmlForGoogle()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("<html>");
            builder.AppendLine("<body>");
            builder.AppendLine("<!--StartFragment-->");
            builder.AppendLine("Starting in the top left box, follow the direction (e.g. three spaces to the right) to find the next letter. ");
            builder.AppendLine(@"<table border=""1"">");
            for (int row = 0; row < Size; row++)
            {
                builder.AppendLine("<tr>");
                for (int column = 0; column < Size; column++)
                {
                    builder.AppendLine($"    <td>{GetCellAtCoordinates(row, column)}</td>");
                }
                builder.AppendLine("</tr>");
            }
            builder.AppendLine("</table>");
            builder.AppendLine("<!--EndFragment-->");
            builder.AppendLine("</body>");

            builder.AppendLine("</html>");
            return builder.ToString();
        }

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
    }

    public class LetterAndArrowCell
    {
        public char Letter;
        public int Number;
        public Direction Direction;

        public static LetterAndArrowCell EmptyCell => new LetterAndArrowCell() {Letter = ' ', Direction = Direction.Undefined, Number = 0};

        public override bool Equals(object obj)
        {
            LetterAndArrowCell cell = obj as LetterAndArrowCell;
            if (cell is null) return false;
            return Equals(cell);
        }

        protected bool Equals(LetterAndArrowCell other)
        {
            return Letter == other.Letter && Number == other.Number && Direction == other.Direction;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Letter.GetHashCode();
                hashCode = (hashCode * 397) ^ Number;
                hashCode = (hashCode * 397) ^ (int) Direction;
                return hashCode;
            }
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(Letter);
            if (0 != Number)
            {
                builder.Append(" ");
                builder.Append(Math.Abs(Number));
            }

            switch (Direction)
            {
                case Direction.Down:
                    builder.Append("↓");
                    break;
                case Direction.Up:
                    builder.Append("↑");
                    break;
                case Direction.Right:
                    builder.Append("→");
                    break;
                case Direction.Left:
                    builder.Append("←");
                    break;
            }
            return builder.ToString();
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