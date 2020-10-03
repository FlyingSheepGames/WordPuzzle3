using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using WordPuzzles.Utility;

namespace WordPuzzles.Puzzle
{
    public class WordSearchMoreOrLess
    {
        private Dictionary<char, List<TakeTwoClue>> dictionaryOfClues;
        private bool alreadyLoaded;
        private Random _randomNumberGenerator;
        public List<HiddenWordInGrid> HiddenWords = new List<HiddenWordInGrid>();
        private int _size;
        public int RandomGeneratorSeed { get; set; } = 0;

        public Dictionary<char, List<TakeTwoClue>> DictionaryOfClues
        {
            get
            {
                if (!alreadyLoaded)
                {
                    string serializedClues =
                        File.ReadAllText(
                            @"C:\Users\Chip\Source\Repos\WordPuzzle3\WordPuzzlesTest\data\AddALetter\dictionary.json");
                    dictionaryOfClues =
                        Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<char, List<TakeTwoClue>>>(
                            serializedClues);
                    alreadyLoaded = true;
                }
                return dictionaryOfClues;
            }
        }

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

        public List<string> Grid { get; set; }

        public int Size
        {
            get => _size;
            set
            {
                _size = value;
                InitializeGrid();
            }
        }


        public void ProcessLetter(char letterToAdd)
        {
            List<TakeTwoClue> clues = DictionaryOfClues[letterToAdd];
            clues.Shuffle(RandomNumberGenerator);
            bool addLetter = 0 == RandomNumberGenerator.Next(1);
            bool wasAbleToPlace = false;

            HiddenWordInGrid hiddenWord =  null;
            foreach (var clue in clues)
            {
                hiddenWord = new HiddenWordInGrid(clue, addLetter);
                wasAbleToPlace = PlaceWordInGrid(hiddenWord);
                if (wasAbleToPlace) break;
            }

            if (wasAbleToPlace)
            {
                HiddenWords.Add(hiddenWord);
            }
        }

        public void InitializeGrid()
        {
            Grid = new List<string>();
            string item = new string('_', Size);
            for (int i = 0; i < Size; i++)
            {
                Grid.Add(item);
            }
        }
        private bool PlaceWordInGrid(HiddenWordInGrid hiddenWord)
        {
            int selectedX = -1;
            int selectedY = -1;
            CardinalDirection selectedDirection = CardinalDirection.Unknown;
            var canPlaceAllLetters = WordToHide(hiddenWord, out selectedX, out selectedY, out selectedDirection);

            if (!canPlaceAllLetters)
            {
                return false;
            }

            PlaceStringInGrid(hiddenWord.HiddenWord, selectedX, selectedY, selectedDirection );
            hiddenWord.XCoordinate = selectedX;
            hiddenWord.YCoordinate = selectedY;
            hiddenWord.Direction = selectedDirection;
            return true;
        }

        private bool WordToHide(HiddenWordInGrid hiddenWord, out int selectedX, out int selectedY,
            out CardinalDirection selectedDirection)
        {
             selectedX = -1;
             selectedY = -1;
             selectedDirection = CardinalDirection.Unknown;
            List<int> XCandidates = new List<int>();
            List<int> YCandidates = new List<int>();
            for (int i = 0; i < Size; i++)
            {
                XCandidates.Add(i);
                YCandidates.Add(i);
            }

            XCandidates.Shuffle(RandomNumberGenerator);
            YCandidates.Shuffle(RandomNumberGenerator);
            string wordToHide = hiddenWord.HiddenWord;

            bool canPlaceAllLetters = false;
            foreach (int xToTry in XCandidates)
            {
                foreach (int yToTry in YCandidates)
                {
                    var length = wordToHide.Length;
                    foreach (CardinalDirection directionToTry in GetPossibleDirections(xToTry, yToTry,
                        length))
                    {
                        string existingLetters = FindStringInGrid(xToTry, yToTry, directionToTry, length);

                        canPlaceAllLetters = true;
                        for (int letterIndexToCompare = 0; letterIndexToCompare < length; letterIndexToCompare++)
                        {
                            var existingLetter = existingLetters[letterIndexToCompare];
                            if (existingLetter == '_') continue;
                            if (existingLetter == wordToHide[letterIndexToCompare]) continue;
                            canPlaceAllLetters = false;
                            break;
                        }

                        if (canPlaceAllLetters)
                        {
                            selectedX = xToTry;
                            selectedY = yToTry;
                            selectedDirection = directionToTry;
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public string FindStringInGrid(int x, int y, CardinalDirection direction, int length)
        {
            StringBuilder builder = new StringBuilder();
            int nextX = x;
            int nextY = y;
            for (int currentIndex = 0; currentIndex < length; currentIndex++)
            {
                char letterToAdd = GetLetterAtCoordinates(nextX, nextY);
                builder.Append(letterToAdd);
                nextX = CalculateNextX(direction,  nextX);
                nextY = CalculateNextY(direction, nextY);
            }

            return builder.ToString();
        }

        private int CalculateNextX(CardinalDirection direction, int nextX)
        {
            if (new[]
            {
                CardinalDirection.East, 
                CardinalDirection.NorthEast, 
                CardinalDirection.SouthEast,
            }.Contains(direction))
            {
                return nextX + 1;
            }

            if (new[]
            {
                CardinalDirection.West,
                CardinalDirection.NorthWest,
                CardinalDirection.SouthWest,
            }.Contains(direction))
            {
                return nextX -1;
            }

            return nextX;
        }

        private int CalculateNextY(CardinalDirection direction, int nextY)
        {
            if (new[]
            {
                CardinalDirection.South,
                CardinalDirection.SouthEast,
                CardinalDirection.SouthWest,
            }.Contains(direction))
            {
                return nextY + 1;
            }

            if (new[]
            {
                CardinalDirection.North,
                CardinalDirection.NorthEast,
                CardinalDirection.NorthWest,
            }.Contains(direction))
            {
                return nextY - 1;
            }

            return nextY;
        }

        private char GetLetterAtCoordinates(int x, int y)
        {
            return Grid[y][x];
        }

        private void PlaceLetterAtCoordinates(char letter, int x, int y)
        {
            StringBuilder builder = new StringBuilder();
            string lineToReplace = Grid[y];
            if (lineToReplace.Length > 0)
            {
                builder.Append(lineToReplace.Substring(0, x));
            }
            builder.Append(letter);
            if (0 <= x + 1 && x + 1 < lineToReplace.Length)
            {
                builder.Append(lineToReplace.Substring(x + 1));
            }

            Grid[y] = builder.ToString();
        }

        public void PlaceStringInGrid(string wordToPlace, int x, int y, CardinalDirection direction)
        {
            int nextX = x;
            int nextY = y;
            foreach(char letterToAdd in wordToPlace)
            {
                char alreadyPlacedLetter = GetLetterAtCoordinates(nextX, nextY);
                if (alreadyPlacedLetter == letterToAdd ||
                    alreadyPlacedLetter == '_')
                {
                    PlaceLetterAtCoordinates(letterToAdd, nextX, nextY);
                }
                else
                {
                    throw new Exception($"The letter {alreadyPlacedLetter} was already placed at {nextX}, {nextY}, so I can't place {letterToAdd} there.");
                }
                nextX = CalculateNextX(direction, nextX);
                nextY = CalculateNextY(direction, nextY);
            }
        }

        public List<CardinalDirection> GetPossibleDirections(int x, int y, int length)
        {
            var possibleDirections = new List<CardinalDirection>()
            {
                CardinalDirection.North,
                CardinalDirection.NorthEast,
                CardinalDirection.East,
                CardinalDirection.SouthEast,
                CardinalDirection.South,
                CardinalDirection.SouthWest,
                CardinalDirection.West,
                CardinalDirection.NorthWest
            };

            if ((y - (length -1)) < 0)
            {
                possibleDirections.Remove(CardinalDirection.NorthWest);
                possibleDirections.Remove(CardinalDirection.North);
                possibleDirections.Remove(CardinalDirection.NorthEast);
            }

            if (Size < y + length )
            {
                possibleDirections.Remove(CardinalDirection.SouthWest);
                possibleDirections.Remove(CardinalDirection.South);
                possibleDirections.Remove(CardinalDirection.SouthEast);
            }

            if ((x - (length - 1)) < 0)
            {
                possibleDirections.Remove(CardinalDirection.NorthWest);
                possibleDirections.Remove(CardinalDirection.West);
                possibleDirections.Remove(CardinalDirection.SouthWest);
            }

            if (Size < x + length)
            {
                possibleDirections.Remove(CardinalDirection.NorthEast);
                possibleDirections.Remove(CardinalDirection.East);
                possibleDirections.Remove(CardinalDirection.SouthEast);
            }

            return possibleDirections;
        }
    }

    public class HiddenWordInGrid
    {
        public string DisplayedWord;
        public string HiddenWord;
        public char LetterAddedOrRemoved;

        public HiddenWordInGrid(TakeTwoClue clue, bool addLetter)
        {
            LetterAddedOrRemoved = clue.LetterRemoved;
            if (addLetter)
            {
                DisplayedWord = clue.ShorterWord;
                HiddenWord = clue.LongerWord;
            }
            else
            {
                DisplayedWord = clue.LongerWord;
                HiddenWord = clue.ShorterWord;
            }
        }

        public int XCoordinate { get; set; }
        public int YCoordinate { get; set; }
        public CardinalDirection Direction { get; set; }
    }   
}