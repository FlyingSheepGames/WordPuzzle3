using System;
using System.Collections.Generic;
using System.Text;

namespace WordPuzzles
{
    public class WordSudoku
    {
        public readonly string Solution;
        public string[] Grid;
        public string[] PartialGrid;
        private Random _random;

        public WordSudoku(string solution, int seed = 0)
        {
            RandomSeed = seed;
            string lowercaseSolution = solution.ToLower();
            var containsDuplicateLetters = ContainsDuplicateLetters(lowercaseSolution);
            if (containsDuplicateLetters)
            {
                throw new Exception("Solution cannot contain duplicate letters.");
            }
            Solution = lowercaseSolution;
            Grid = CreateGrid(solution.Length);
            RefreshPartialGrid();
        }

        public int RandomSeed;
        public Random Random1
        {
            get
            {
                if (_random == null)
                {
                    if (RandomSeed != 0)
                    {
                        _random = new Random();
                    }
                    else
                    {
                        _random = new Random(RandomSeed);
                    }
                }
                return _random;
            }
            set => _random = value;
        }

        internal void RefreshPartialGrid()
        {
            PartialGrid = CreatePartialGrid(Grid);
        }

        private string[] CreatePartialGrid(string[] grid)
        {
            string[] partialGridInProgress = new string[grid.Length];
            partialGridInProgress[0] = new string('_', grid.Length);
            for (var index = 1; index < grid.Length; index++)
            {
                partialGridInProgress[index] = grid[index];
            }
            //randomly block out some letters. 
            for (int randomCounter = 0; randomCounter < 12; randomCounter++)
            {
                int row = Random1.Next(1, Solution.Length);
                int column = Random1.Next(0, Solution.Length);
                var isUniquelyDetermined = IsUniquelyDetermined(row, column, partialGridInProgress);

                if (isUniquelyDetermined)
                {
                    SetBlank(partialGridInProgress, row, column);
                }
            }

            return partialGridInProgress;
        }

        internal bool IsUniquelyDetermined(int row, int column, string[] partialGridInProgress)
        {
            bool isUniquelyDetermined = false;
            var results = CalculateAvailableNumbersForRowAndColumn(column, partialGridInProgress.Length, partialGridInProgress[row],
                partialGridInProgress);
            if (0 == results.Count) //only one possibility for this cell, so we can blank it out.
            {
                isUniquelyDetermined = true;
            }

            return isUniquelyDetermined;
        }

        internal void SetBlank(string[] partialGridInProgress, int row, int column)
        {
            StringBuilder updatedRow = new StringBuilder();
            string currentRow = partialGridInProgress[row];
            for (var index = 0; index < currentRow.Length; index++)
            {
                char currentCharacter = currentRow[index];
                if (column == index)
                {
                    updatedRow.Append("_");
                }
                else
                {
                    updatedRow.Append(currentCharacter);
                }
            }
            partialGridInProgress[row] = updatedRow.ToString();
        }

        internal string[] CreateGrid(int length, int stackDepth = 0)
        {

            if (5 < stackDepth)//hardcoded example
            {
                if (2 == length)
                {
                    return new[]
                    {
                        "01",
                        "10",
                    };
                }

                if (3 == length)
                {
                    return new[]
                    {
                        "012",
                        "120",
                        "201",
                    };
                }

                if (4 == length)
                {
                    return new[]
                    {
                        "0123",
                        "1302",
                        "3210",
                        "2013"
                    }; 
                }
                if (5 == length)
                {
                    return new[]
                    {
                        "01234",
                        "10423",
                        "34012",
                        "42301",
                        "23140",
                    };
                }

                if (6 == length)
                {
                    return new[]
                    {
                        "012345",
                        "431052",
                        "105234",
                        "240513",
                        "523401",
                        "354120",
                    };
                }

                if (7 == length)
                {
                    return new[]
                    {
                        "0123456",
                        "2365140",
                        "3041625",
                        "6452013",
                        "4530261",
                        "5216304",
                        "1604532"
                    };
                }

                if (8 == length)
                {
                    return new[]
                    {
                        "01234567",
                        "23467105",
                        "16705243",
                        "47013652",
                        "70352416",
                        "65170324",
                        "52641730",
                        "34526071"
                    };
                }

            }

            string[] gridInProgress = new string[length];
            for (int i = 0; i < length; i++)
            {
                gridInProgress[i] = new string('_', length);
            }
            //first line = 0123...
            StringBuilder currentLine = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                currentLine.Append(i);
            }

            gridInProgress[0] = currentLine.ToString();
            currentLine.Clear();

            //second line can start with anything except 0 (or 1, so first column won't = top row). 
            //int nextNumber = random.Next(2, length);
            //currentLine.Append(nextNumber);

            //for each row, for each column
            for (int row = 1; row < length; row++)
            {
                currentLine.Clear();
                for (int column = 0; column < length; column++)
                {
                    List<int> availableNumbers = CalculateAvailableNumbersForRowAndColumn(column, length, currentLine.ToString(), gridInProgress);
                    if (0 == availableNumbers.Count)
                    {
                        if (5 < stackDepth)
                        {
                            throw new Exception("Reached stack limit.");
                        }
                        else
                        {
                            return CreateGrid(length, stackDepth + 1);
                        }
                    }

                    int nextNumber = availableNumbers[Random1.Next(availableNumbers.Count)];
                    currentLine.Append(nextNumber);
                }
                gridInProgress[row] = currentLine.ToString();
            }

            return gridInProgress;
        }

        private List<int> CalculateAvailableNumbersForRowAndColumn(int column, int length, string lineSoFar,
            string[] gridInProgress)
        {
            bool[] isAvailable = new bool[length];
            
            //start with everything available. 
            for (int i = 0; i < length; i++)
            {
                isAvailable[i] = true;
            }

            //eliminate already placed in row. 
            foreach (char character in lineSoFar)
            {
                int ineligibleNumber;
                if (int.TryParse(character.ToString(), out ineligibleNumber))
                {
                    isAvailable[ineligibleNumber] = false;
                }
            }
            //eliminate already placed in column.
            foreach (string line in gridInProgress)
            {
                char characterInColumn = line[column];
                int ineligibleNumber;
                if (int.TryParse(characterInColumn.ToString(), out ineligibleNumber))
                {
                    isAvailable[ineligibleNumber] = false;
                }
            }
            List<int> availableNumbers = new List<int>();
            for (int i = 0; i < length; i++)
            {
                if (isAvailable[i])
                {
                    availableNumbers.Add(i);
                }
            }

            return availableNumbers;
        }

        public static bool ContainsDuplicateLetters(string solution)
        {
            string lowercaseSolution = solution.ToLower();
            List<char> uniqueCharacters = new List<char>();
            bool containsDuplicateLetters = false;
            foreach (char character in lowercaseSolution)
            {
                if (uniqueCharacters.Contains(character))
                {
                    containsDuplicateLetters = true;
                }
                else
                {
                    uniqueCharacters.Add(character);
                }
            }

            return containsDuplicateLetters;
        }

        public string FormatForGoogle(bool usePartialGrid = true)
        {
            StringBuilder builder = new StringBuilder();
            string[] gridToUse = Grid;
            if (usePartialGrid)
            {
                gridToUse = PartialGrid;
            }
            foreach (string line in gridToUse)
            {
                foreach (char character in line)
                {
                    if (character == '_')
                    {
                        builder.Append(" ");
                    }
                    else
                    {
                        int indexOfSolution = int.Parse(character.ToString());
                        builder.Append(Solution[indexOfSolution].ToString().ToUpper());
                    }

                    builder.Append("\t");
                }

                builder.Remove(builder.Length - 1, 1); //remove last tab
                builder.AppendLine();
            }

            return builder.ToString();
        }

        public string FormatHtmlForGoogle(bool usePartialGrid = true)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("<html>");
            builder.AppendLine("<body>");
            builder.AppendLine("<table>");
            builder.AppendLine("<!--StartFragment-->");
            string[] gridToUse = Grid;
            if (usePartialGrid)
            {
                gridToUse = PartialGrid;
            }
            foreach (string line in gridToUse)
            {
                builder.AppendLine("\t<tr>");
                foreach (char character in line.ToUpper())
                {
                    builder.Append("\t\t<td>");
                    if (character == '_')
                    {
                        builder.Append(" ");
                    }
                    else
                    {
                        int indexOfSolution = int.Parse(character.ToString());
                        string characterToPrint = Solution[indexOfSolution].ToString().ToUpper();

                        builder.Append(characterToPrint);
                    }

                    builder.Append("</td>");
                    builder.AppendLine();
                }
                builder.AppendLine("\t</tr>");
            }
            builder.AppendLine("<!--EndFragment-->");
            builder.AppendLine("</table>");
            builder.AppendLine("</body>");
            builder.AppendLine("</html>");

            return builder.ToString();
        }
    }
}