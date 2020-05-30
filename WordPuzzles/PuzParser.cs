using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace WordPuzzles
{
    public class PuzParser
    {
        public ClueRepository ParseFile(string fileToParse)
        {
            var newClues = new ClueRepository();
            //ParseAllLinesTemp(fileToParse);
            var allText = File.ReadAllText(fileToParse);
            int tokenCount = 0;
            string gridAsString;

            var tokensSplitByNilCharacters = allText.Split(new char[] {(char) 0}, StringSplitOptions.RemoveEmptyEntries);
            int indexOfGrid = 0;
            gridAsString = ExtractGridAsStringFromTokens(tokensSplitByNilCharacters, out indexOfGrid);

            List<string> clues = new List<string>();
            for (var index = 0;
                index < tokensSplitByNilCharacters.Length;
                index++)
            {
                string token = tokensSplitByNilCharacters[index];
                //Console.WriteLine($"{tokenCount++}: {token}");
                //if (3 < token.Length)
                {
                    bool isAWord = true;
                    bool isAClue = true;
                    bool hasAtLeastOneLetter = false;
                    foreach (char character in token)
                    {
                        if (!char.IsLetter(character))
                        {
                            isAWord = false;
                        }
                        else
                        {
                            hasAtLeastOneLetter = true;
                        }

                        if (!char.IsUpper(character))
                        {
                            isAWord = false;
                        }
                    }

                    if (!hasAtLeastOneLetter)
                    {
                        //isAClue = false;
                    }

                    if (isAWord)
                    {
                        //isAClue = false;
                        newClues.AddClue(token, "");
                        //Console.WriteLine($"Adding '{token}' as a word");
                    }

                    if (isAClue)
                    {
                        //Console.WriteLine($"'{token}' is a clue.");
                        if (indexOfGrid + 3 < index)
                        {
                            clues.Add(token);
                        }
                    }
                }
            }

            var listOfClues = ParseWordsFromGridString(gridAsString);
            for (var index = 0; index < listOfClues.Count; index++)
            {
                var clue = listOfClues[index];
                string clueText = "No more clues";
                if (index >= 0 && clues.Count > index)
                {
                    clueText = clues[index];
                }
                else
                {
                    throw new Exception("No more clues");
                }
                newClues.AddClue(clue.Word, clueText, ClueSource.CLUE_SOURCE_CROSSWORD);
                Console.WriteLine($"{clue.Word}: {clueText}");
            }

            return newClues;
        }

        private static string ExtractGridAsStringFromTokens(string[] tokensSplitByNilCharacters, out int indexOfGrid)
        {
            string gridAsString = null;
            bool isGrid = false; 
            int indexToTry = 12;
            while (!isGrid)
            {
                gridAsString = tokensSplitByNilCharacters[indexToTry];
                int indexOfDash = gridAsString.IndexOf('-');
                if (0 < indexOfDash)
                {
                    gridAsString = gridAsString.Substring(0, indexOfDash);
                }

                isGrid = true;
                foreach (char letter in gridAsString)
                {
                    if (letter == '.') continue;
                    if (letter == '-') continue;
                    if (char.IsUpper(letter)) continue;
                    isGrid = false;
                    break;
                }

                if (gridAsString.Length < 25)
                {
                    isGrid = false;
                }
                indexToTry--;
                if (indexToTry < 0)
                {
                    throw new Exception("Unable to find grid. Sorry.");
                }

            }

            indexOfGrid = indexToTry;
            return gridAsString;
        }

        public List<CrosswordPuzzleEntry> ParseWordsFromGridString(string grid)
        {
            var wordsFromGrid = new List<CrosswordPuzzleEntry>();
            int dimension; //assume all grids are squares. If that's not the case, replace with width/height
            dimension = (int) Math.Sqrt(grid.Length);
            char[][] letterGrid = new char[dimension][];
            string[] letterGridAsStrings = new string[dimension];

            StringBuilder currentAcrossWord = new StringBuilder();
            PopulateGrid(grid, dimension, letterGrid, currentAcrossWord, wordsFromGrid, letterGridAsStrings);

            ExtractAcrossWordsFromGrid(grid, dimension, letterGrid, currentAcrossWord, wordsFromGrid, letterGridAsStrings);
            ExtractDownWordsFromGrid(grid, dimension, wordsFromGrid, letterGridAsStrings);

            wordsFromGrid = NumberWordsFromGrid(wordsFromGrid);


            return wordsFromGrid;
            
        }

        internal List<CrosswordPuzzleEntry> NumberWordsFromGrid(List<CrosswordPuzzleEntry> wordsFromGrid)
        {
            int wordCount = 0;
            var sortedList = wordsFromGrid.OrderBy(word => word.SortNumber).ToList();
            int lastIndex = int.MinValue;
            foreach (var word in sortedList)
            {
                if (lastIndex != word.IndexInSingleString)
                {
                    lastIndex = word.IndexInSingleString;
                    wordCount++;
                }
                word.ClueNumber = wordCount;
            }
            return sortedList;
        }

        private void ExtractDownWordsFromGrid(string grid, int dimension, List<CrosswordPuzzleEntry> wordsFromGrid,
            string[] letterGridAsStrings)
        {
            StringBuilder currentDownWord = new StringBuilder();
            bool isCurrentWordNumbered = false;
            int indexForCurrentWord = 0;
            for (int x = 0; x < dimension; x++)
            {
                for (int y = 0; y < dimension; y++)
                {
                    int indexForCurrentLetter = CalculateIndexFromCoordinates(dimension, x, y);
                    var letterInCrosswordClue = grid[indexForCurrentLetter];
                    if (letterInCrosswordClue == '.')
                    {
                        AddWordAndResetBuilder(currentDownWord, wordsFromGrid, x, y, isCurrentWordNumbered, 
                            indexForCurrentWord, CrosswordDirection.DOWN);
                    }
                    else
                    {
                        if (0 == currentDownWord.Length)
                        {
                            isCurrentWordNumbered = DetermineIfCellIsNumbered(x, y, letterGridAsStrings);
                            indexForCurrentWord = indexForCurrentLetter;
                        }
                        currentDownWord.Append(letterInCrosswordClue);
                    }
                }

                if (0 < currentDownWord.Length)
                {
                    AddWordAndResetBuilder(currentDownWord, wordsFromGrid, x, 0, isCurrentWordNumbered, 
                        indexForCurrentWord, CrosswordDirection.DOWN);
                }
            }
        }

        private static void PopulateGrid(string grid, int dimension, char[][] letterGrid,
            StringBuilder currentAcrossWord, List<CrosswordPuzzleEntry> wordsFromGrid, string[] letterGridAsStrings)
        {
            StringBuilder currentRowBuilder = new StringBuilder();
            for (int newY = 0; newY < dimension; newY++)
            {
                letterGrid[newY] = new char[dimension];
                letterGridAsStrings[newY] = "";
                for (int newX = 0; newX < dimension; newX++)
                {
                    var letterInCrosswordClue = grid[CalculateIndexFromCoordinates(dimension, newX, newY)];
                    letterGrid[newY][newX] = letterInCrosswordClue;
                    currentRowBuilder.Append(letterInCrosswordClue);
                }

                letterGridAsStrings[newY] = currentRowBuilder.ToString();
                currentRowBuilder.Clear();
            }

        }

        private void ExtractAcrossWordsFromGrid(string grid, int dimension, char[][] letterGrid,
    StringBuilder currentAcrossWord, List<CrosswordPuzzleEntry> wordsFromGrid, string[] letterGridAsStrings)
        {
            bool isCurrentWordNumbered = false;
            int startingIndexForNextWordToBeAdded = 0;
            for (int newY = 0; newY < dimension; newY++)
            {
                for (int newX = 0; newX < dimension; newX++)
                {
                    var currentIndexInSingleString = CalculateIndexFromCoordinates(dimension, newX, newY);
                    var letterInCrosswordClue = grid[currentIndexInSingleString];
                    if (letterInCrosswordClue == '.')
                    {
                        AddWordAndResetBuilder(currentAcrossWord, wordsFromGrid, newY, newX,
                            isCurrentWordNumbered, startingIndexForNextWordToBeAdded, CrosswordDirection.ACROSS);
                    }
                    else
                    {
                        if (0 == currentAcrossWord.Length)
                        {
                            isCurrentWordNumbered = DetermineIfCellIsNumbered(newX, newY, letterGridAsStrings);
                            startingIndexForNextWordToBeAdded = currentIndexInSingleString;
                        }
                        currentAcrossWord.Append(letterInCrosswordClue);
                    }

                }

                AddWordAndResetBuilder(currentAcrossWord, wordsFromGrid, newY, dimension, 
                    isCurrentWordNumbered, startingIndexForNextWordToBeAdded, CrosswordDirection.ACROSS);
            }
        }

        private static int CalculateIndexFromCoordinates(int dimension, int x, int y)
        {
            return (dimension * y) + x;
        }

        private static void AddWordAndResetBuilder(StringBuilder wordToAdd, List<CrosswordPuzzleEntry> wordsFromGrid,
            int x = 0, int y = 0, bool isCurrentWordNumbered = false, int currentIndexInSingleString =0, CrosswordDirection direction = CrosswordDirection.UNKNOWN)
        {
            if (0 < wordToAdd.Length)
            {
                wordsFromGrid.Add(
                    new CrosswordPuzzleEntry()
                    {
                        Word = wordToAdd.ToString(), 
                        IsCellNumbered = isCurrentWordNumbered, 
                        IndexInSingleString = currentIndexInSingleString, 
                        Direction = direction

                    });
                wordToAdd.Clear();
            }
        }

        internal bool DetermineIfCellIsNumbered(int x, int y, string[] letterGridAsStrings)
        {
            //This might always return true....
            if (x == 0) return true;
            if (y == 0) return true;
            //TODO: If cell immediately above is . return true
            //TODO: If cell immediately to the left is . return true
            string currentRow = letterGridAsStrings[y];
            if (currentRow[x - 1] == '.') return true; 

            return false;
        }
    }
}