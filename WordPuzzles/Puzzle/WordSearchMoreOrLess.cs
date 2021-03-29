using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using WordPuzzles.Puzzle.Legacy;
using WordPuzzles.Utility;

namespace WordPuzzles.Puzzle
{
    public class WordSearchMoreOrLess :IPuzzle
    {
        // ReSharper disable once UnusedMember.Global
        public bool IsWordSearchMoreOrLess = true;

        private Dictionary<char, List<TakeTwoClue>> dictionaryOfClues;
        private bool alreadyLoaded;
        private Random _randomNumberGenerator;
        public List<HiddenWordInGrid> HiddenWords = new List<HiddenWordInGrid>();
        private int _size;
        private Dictionary<int, List<string>> forbiddenWordsIndexedByLength;
        private const string INSTRUCTIONS = @"
This is a word search, more or less. <p>
Each word listed below is almost (give or take one letter) hidden in the grid. <p>
For example if HEAR is given as a word, you might find HER in the grid, or you might find HEART. <p>
The word may be placed in the grid horizontally, veritically, diagonally, and may be read backwards. It will not wrap around the edge of grid.<p>
Write the letter that was added (or removed) next to the word, and then read down the column of letters to solve the puzzle.<p>
";
        public int RandomGeneratorSeed { get; set; } = 0;
        private List<CardinalDirection> preferredDirections;
        [JsonIgnore]
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
                if (Grid?.Count != value)
                {
                    InitializeGrid();
                }
            }
        }
        [JsonIgnore]
        public Dictionary<int, List<string>> ForbiddenWordsIndexedByLength
        {
            get
            {
                if (forbiddenWordsIndexedByLength == null)
                {
                    forbiddenWordsIndexedByLength = InitializeListOfForbiddenWords();
                }
                return forbiddenWordsIndexedByLength;
            }

        }

        private Dictionary<int, List<string>> InitializeListOfForbiddenWords()
        {
            var forbiddenWords = new Dictionary<int, List<string>>();
            AddForbiddenWord(forbiddenWords, "poop");
            AddForbiddenWord(forbiddenWords, "fuck");
            AddForbiddenWord(forbiddenWords, "cunt");
            AddForbiddenWord(forbiddenWords, "pussy");
            return forbiddenWords;
        }

        private void AddForbiddenWord(Dictionary<int, List<string>> forbiddenWords, string wordToAdd)
        {
            int wordLength = wordToAdd.Length;
            if (!forbiddenWords.ContainsKey(wordLength))
            {
                forbiddenWords[wordLength] = new List<string>();
            }
            forbiddenWords[wordLength].Add(wordToAdd);
        }


        public void ProcessLetter(char letterToAdd)
        {
            List<TakeTwoClue> clues = DictionaryOfClues[letterToAdd];
            clues.Shuffle(RandomNumberGenerator);
            bool addLetter = 0 == RandomNumberGenerator.Next(2);
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
            var canPlaceAllLetters = DetermineWhereToHideWord(hiddenWord, out selectedX, out selectedY, out selectedDirection);

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

        private bool DetermineWhereToHideWord(HiddenWordInGrid hiddenWordObject, out int selectedX, out int selectedY,
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
            string wordToHide = hiddenWordObject.HiddenWord;

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

            MoveDirectionToEndOfList(direction);
        }

        private void MoveDirectionToEndOfList(CardinalDirection directionToBeMadeLast)
        {
            var replacementDirections = new List<CardinalDirection>();
            foreach (var direction in PreferredDirections)
            {
                if (direction == directionToBeMadeLast) continue;
                replacementDirections.Add(direction);
            }
            replacementDirections.Add(directionToBeMadeLast);
            PreferredDirections = replacementDirections;
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

            possibleDirections = OrderPossibleDirectionsPerPreferences(possibleDirections);
            return possibleDirections;
        }

        private List<CardinalDirection> OrderPossibleDirectionsPerPreferences(
            List<CardinalDirection> possibleDirections)
        {
            var orderedPreferences = new List<CardinalDirection>();
            foreach (var direction in PreferredDirections)
            {
                if (possibleDirections.Contains(direction))
                {
                    orderedPreferences.Add(direction);
                }
            }
            return orderedPreferences;
        }

        public void FillInRemainingGrid()
        {
            for (int x = 0; x < Size; x++)
            {
                for (int y = 0; y < Size; y++)
                {
                    if (GetLetterAtCoordinates(x, y) == '_')
                    {
                        char randomLetter = (char) ('a' + RandomNumberGenerator.Next(26));
                        PlaceLetterAtCoordinates(randomLetter, x, y);
                    }
                }
            }

        }

        public List<HiddenWordInGrid> FindForbiddenWords()
        {
            var forbiddenWordsFound = new List<HiddenWordInGrid>();
            foreach (int currentLength in ForbiddenWordsIndexedByLength.Keys)
            {
                List<string> forbiddenWordsOfCurrentLength = ForbiddenWordsIndexedByLength[currentLength];
                for (int x = 0; x < Size; x++)
                {
                    for (int y = 0; y < Size; y++)
                    {
                        foreach (var directionToCheck in GetPossibleDirections(x, y, currentLength))
                        {
                            var wordToCheck = FindStringInGrid(x, y, directionToCheck, currentLength);
                            if (forbiddenWordsOfCurrentLength.Contains(wordToCheck))
                            {
                                forbiddenWordsFound.Add(
                                    new HiddenWordInGrid()
                                    {
                                        Direction = directionToCheck, 
                                        XCoordinate =  x, 
                                        YCoordinate = y, 
                                        HiddenWord = wordToCheck,

                                    });
                            }
                        }
                    }
                }
            }
            return forbiddenWordsFound;
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
            builder.AppendLine("<p><h2>Word List</h2>");
            builder.AppendLine("<table>");
            foreach (var word in HiddenWords)
            {
                builder.Append(@"<tr><td class=""normal"" width=""30"">");
                if (includeSolution)
                {
                    builder.Append(word.LetterAddedOrRemoved.ToString().ToUpperInvariant());
                }
                builder.Append(@"</td><td class=""open"" width=""250"">");
                builder.Append(word.DisplayedWord.ToUpperInvariant());
                builder.AppendLine("</td></tr>");
            }
            builder.AppendLine("</table>");
            //Show puzzle. 
            builder.AppendLine("<p><h2>Grid of Letters</h2>");
            builder.AppendLine("<table>");
            foreach (string line in Grid)
            {
                builder.Append("<tr>");
                foreach (char character in line.ToUpperInvariant())
                {
                    builder.AppendLine($@"   <td class=""open"" width=""30"" >{character}</td>");
                }
                builder.AppendLine("</tr>");
            }
            builder.AppendLine("</table>");

            generator.AppendSolution(builder, Solution, includeSolution);

            if (!isFragment)
            {
                generator.AppendHtmlFooter(builder);
            }
            return builder.ToString();

        }

        public string Description => $"Word Search More Or Less with solution {Solution}";
        public WordPuzzleType Type { get; } = WordPuzzleType.WordSearchMoreOrLess;

        public List<string> GetClues()
        {
            throw new NotImplementedException();
        }

        public void ReplaceClue(string clueToReplace, string newClue)
        {
            throw new NotImplementedException();
        }

        public string Solution { get; set; }

        public List<CardinalDirection> PreferredDirections
        {
            get
            {
                if (preferredDirections == null)
                {
                    preferredDirections = InitializePreferredDirections();
                }
                return preferredDirections;
            }
            set => preferredDirections = value;
        }

        private List<CardinalDirection> InitializePreferredDirections()
        {
            var directions = new List<CardinalDirection>()
            {
                CardinalDirection.North, 
                CardinalDirection.NorthEast, 
                CardinalDirection.East, 
                CardinalDirection.SouthEast,
                CardinalDirection.South, 
                CardinalDirection.SouthWest,
                CardinalDirection.West, 
                CardinalDirection.NorthWest,
            };
            directions.Shuffle(RandomNumberGenerator);
            return directions;
        }

        public void SetSolution(string solution)
        {
            Solution = solution;
            foreach (char letter in solution.ToLowerInvariant())
            {
                if (!char.IsLetter(letter)) continue;
                ProcessLetter(letter);
            }
        }
    }

    public class HiddenWordInGrid
    {
        public string DisplayedWord;
        public string HiddenWord;
        public char LetterAddedOrRemoved;

        public HiddenWordInGrid()
        {
            
        }
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