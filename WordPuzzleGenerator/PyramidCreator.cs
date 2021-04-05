using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using WordPuzzles.Puzzle;
using WordPuzzles.Puzzle.Legacy;
using WordPuzzles.Utility;

namespace WordPuzzleGenerator
{
    internal class PyramidCreator 
    {

        public PyramidCreator()
        {
        }

        public void RunInPyramidMode()
        {

            PuzzlePyramid puzzlePyramid = new PuzzlePyramid();
            // First, we select a start date
            puzzlePyramid.StartDate = new DateTime(2021, 4, 30);
            string fileNameForJson =
                $@"{Program.BASE_DIRECTORY}\pyramids\{puzzlePyramid.StartDate.Month}-{puzzlePyramid.StartDate.Day}.json";

            string fileNameForHtml =fileNameForJson.Replace(".json", ".html");
            string fileNameWithSolutionsForHtml = fileNameForJson.Replace(".json", "-Solutions.html");
            if (File.Exists(fileNameForJson))
            {
                puzzlePyramid = JsonConvert.DeserializeObject<PuzzlePyramid>(File.ReadAllText(fileNameForJson));
                Console.WriteLine($"Loaded pyramid from file {fileNameForJson}. Press any key to continue.");
                Console.ReadKey();
            }
            InitializeSortedListOfPuzzleTypes(puzzlePyramid);

            InteractivelyRemoveExistingPuzzles(puzzlePyramid);
            // Then we find someone quotable that was born within a week after the start date
            if (puzzlePyramid.SelectedPerson == null)
            {
                if (SelectPersonToQuote(puzzlePyramid)) return;
            }

            //  Do the following 3 times
            List<string> wordsToReplace;
            if (puzzlePyramid.WordsToReplace == null)
            {
                wordsToReplace = SelectWordsToReplace(puzzlePyramid);
                puzzlePyramid.WordsToReplace = wordsToReplace;
            }
            else
            {
                wordsToReplace = puzzlePyramid.WordsToReplace;
            }

            //      Create a puzzle (with clues) with that word as the solution
            if (puzzlePyramid.PuzzleJ == null)
            {
                 //CreatePuzzleJAsMultipleCluesPuzzle(wordsToReplace[0], puzzlePyramid);
                 Console.WriteLine($"Let's create puzzle J. The solution will be '{wordsToReplace[0]}' from the original quote.");
                 puzzlePyramid.PuzzleJ = CreatePuzzleAsType(wordsToReplace[0], PuzzlesWithoutClues);
            }

            //puzzle J relies on three sub-puzzles (A, B, C)
            if (puzzlePyramid.PuzzleJ != null)
            {
                if (puzzlePyramid.PuzzleA == null)
                {
                    //CreatePuzzleJ(wordsToReplace, puzzlePyramid);
                    var clueToReplace = SelectClueToReplace(puzzlePyramid.PuzzleJ);
                    if (!string.IsNullOrWhiteSpace(clueToReplace))
                    {
                        puzzlePyramid.PuzzleA = CreatePuzzleAsType(clueToReplace);
                        if (puzzlePyramid.PuzzleA != null)
                        {
                            puzzlePyramid.PuzzleJ.ReplaceClue(clueToReplace, "(solve puzzle A)");
                        }
                    }
                }

                if (puzzlePyramid.PuzzleB == null)
                {
                    var clueToReplace = SelectClueToReplace(puzzlePyramid.PuzzleJ);
                    if (!string.IsNullOrWhiteSpace(clueToReplace))
                    {
                        puzzlePyramid.PuzzleB = CreatePuzzleAsType(clueToReplace);
                        if (puzzlePyramid.PuzzleB != null)
                        {
                            puzzlePyramid.PuzzleJ.ReplaceClue(clueToReplace, "(solve puzzle B)");
                        }
                    }
                }

                if (puzzlePyramid.PuzzleC == null)
                {
                    var clueToReplace = SelectClueToReplace(puzzlePyramid.PuzzleJ);
                    if (!string.IsNullOrWhiteSpace(clueToReplace))
                    {
                        puzzlePyramid.PuzzleC = CreatePuzzleAsType(clueToReplace);
                        if (puzzlePyramid.PuzzleC != null)
                        {
                            puzzlePyramid.PuzzleJ.ReplaceClue(clueToReplace, "(solve puzzle C)");
                        }
                    }
                }

            }

            if (puzzlePyramid.PuzzleK == null)
            {
                Console.WriteLine($"Let's create puzzle K. The solution will be '{wordsToReplace[1]}' from the original quote.");
                puzzlePyramid.PuzzleK = CreatePuzzleAsType(wordsToReplace[1], PuzzlesWithoutClues);
            }

            // To Find the word K, solve a puzzle with missing clues D, E, and F
            if (puzzlePyramid.PuzzleK != null)
            {
                if (puzzlePyramid.PuzzleD == null)
                {
                    var clueToReplace = SelectClueToReplace(puzzlePyramid.PuzzleK);
                    if (!string.IsNullOrWhiteSpace(clueToReplace))
                    {
                        puzzlePyramid.PuzzleD = CreatePuzzleAsType(clueToReplace);
                        if (puzzlePyramid.PuzzleD != null)
                        {
                            puzzlePyramid.PuzzleK.ReplaceClue(clueToReplace, "(solve puzzle D)");
                        }
                    }
                }

                if (puzzlePyramid.PuzzleE == null)
                {
                    var clueToReplace = SelectClueToReplace(puzzlePyramid.PuzzleK);
                    if (!string.IsNullOrWhiteSpace(clueToReplace))
                    {
                        puzzlePyramid.PuzzleE = CreatePuzzleAsType(clueToReplace);
                        if (puzzlePyramid.PuzzleE != null)
                        {
                            puzzlePyramid.PuzzleK.ReplaceClue(clueToReplace, "(solve puzzle E)");
                        }
                    }
                }

                if (puzzlePyramid.PuzzleF == null)
                {
                    var clueToReplace = SelectClueToReplace(puzzlePyramid.PuzzleK);
                    if (!string.IsNullOrWhiteSpace(clueToReplace))
                    {
                        puzzlePyramid.PuzzleF = CreatePuzzleAsType(clueToReplace);
                        if (puzzlePyramid.PuzzleF != null)
                        {
                            puzzlePyramid.PuzzleK.ReplaceClue(clueToReplace, "(solve puzzle F)");
                        }
                    }
                }

            }

            // To find the word L, solve the puzzle with missing clues G, H, and I. 
            if (puzzlePyramid.PuzzleL == null)
            {
                Console.WriteLine($"Let's create puzzle L. The solution will be '{wordsToReplace[2]}' from the original quote.");
                puzzlePyramid.PuzzleL = CreatePuzzleAsType(wordsToReplace[2], PuzzlesWithoutClues);
            }

            if (puzzlePyramid.PuzzleL != null)
            {
                if (puzzlePyramid.PuzzleG == null)
                {
                    var clueToReplace = SelectClueToReplace(puzzlePyramid.PuzzleL);
                    if (!string.IsNullOrWhiteSpace(clueToReplace))
                    {
                        puzzlePyramid.PuzzleG = CreatePuzzleAsType(clueToReplace);
                        if (puzzlePyramid.PuzzleG != null)
                        {
                            puzzlePyramid.PuzzleL.ReplaceClue(clueToReplace, "(solve puzzle G)");
                        }
                    }
                }

                if (puzzlePyramid.PuzzleH == null)
                {
                    var clueToReplace = SelectClueToReplace(puzzlePyramid.PuzzleL);
                    if (!string.IsNullOrWhiteSpace(clueToReplace))
                    {
                        puzzlePyramid.PuzzleH = CreatePuzzleAsType(clueToReplace);
                        if (puzzlePyramid.PuzzleH != null)
                        {
                            puzzlePyramid.PuzzleL.ReplaceClue(clueToReplace, "(solve puzzle H)");
                        }
                    }
                }

                if (puzzlePyramid.PuzzleI == null)
                {
                    var clueToReplace = SelectClueToReplace(puzzlePyramid.PuzzleL);
                    if (!string.IsNullOrWhiteSpace(clueToReplace))
                    {
                        puzzlePyramid.PuzzleI = CreatePuzzleAsType(clueToReplace);
                        if (puzzlePyramid.PuzzleI != null)
                        {
                            puzzlePyramid.PuzzleL.ReplaceClue(clueToReplace, "(solve puzzle I)");
                        }
                    }
                }

            }

            GenerateHtmlFile(puzzlePyramid, fileNameForHtml);
            GenerateHtmlFile(puzzlePyramid, fileNameWithSolutionsForHtml, true);

            WritePyramidToDisk(puzzlePyramid, fileNameForJson);

            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();

        }

        private void InteractivelyRemoveExistingPuzzles(PuzzlePyramid puzzlePyramid)
        {
            bool atLeastOnePuzzleExists = false;
            bool readyToExit = false;

            while (!readyToExit)
            {
                Program.ClearConsoleInputAndOutput();
                Console.WriteLine("Press a letter to delete the associated puzzle.");
                if (puzzlePyramid.PuzzleA != null)
                {
                    Console.WriteLine($"A: {puzzlePyramid.PuzzleA.Description}");
                    atLeastOnePuzzleExists = true;
                }

                if (puzzlePyramid.PuzzleB != null)
                {
                    Console.WriteLine($"B: {puzzlePyramid.PuzzleB.Description}");
                    atLeastOnePuzzleExists = true;
                }

                if (puzzlePyramid.PuzzleC != null)
                {
                    Console.WriteLine($"C: {puzzlePyramid.PuzzleC.Description}");
                    atLeastOnePuzzleExists = true;
                }

                if (puzzlePyramid.PuzzleD != null)
                {
                    Console.WriteLine($"D: {puzzlePyramid.PuzzleD.Description}");
                    atLeastOnePuzzleExists = true;
                }

                if (puzzlePyramid.PuzzleE != null)
                {
                    Console.WriteLine($"E: {puzzlePyramid.PuzzleE.Description}");
                    atLeastOnePuzzleExists = true;
                }

                if (puzzlePyramid.PuzzleF != null)
                {
                    Console.WriteLine($"F: {puzzlePyramid.PuzzleF.Description}");
                    atLeastOnePuzzleExists = true;
                }

                if (puzzlePyramid.PuzzleG != null)
                {
                    Console.WriteLine($"G: {puzzlePyramid.PuzzleG.Description}");
                    atLeastOnePuzzleExists = true;
                }

                if (puzzlePyramid.PuzzleH != null)
                {
                    Console.WriteLine($"H: {puzzlePyramid.PuzzleH.Description}");
                    atLeastOnePuzzleExists = true;
                }

                if (puzzlePyramid.PuzzleI != null)
                {
                    Console.WriteLine($"I: {puzzlePyramid.PuzzleI.Description}");
                    atLeastOnePuzzleExists = true;
                }

                if (puzzlePyramid.PuzzleJ != null)
                {
                    Console.WriteLine($"J: {puzzlePyramid.PuzzleJ.Description}");
                    atLeastOnePuzzleExists = true;
                }

                if (puzzlePyramid.PuzzleK != null)
                {
                    Console.WriteLine($"K: {puzzlePyramid.PuzzleK.Description}");
                    atLeastOnePuzzleExists = true;
                }

                if (puzzlePyramid.PuzzleL != null)
                {
                    Console.WriteLine($"L: {puzzlePyramid.PuzzleL.Description}");
                    atLeastOnePuzzleExists = true;
                }

                if (!atLeastOnePuzzleExists)
                {
                    Program.ClearConsoleInputAndOutput();
                    return;
                }

                Console.WriteLine("Press the letter of the puzzle to remove, press enter to skip.");
                Console.Write(">>");
                var keyPress = Console.ReadKey();
                switch (keyPress.Key)
                {
                    case ConsoleKey.A:
                        CountOfPuzzleTypes[puzzlePyramid.PuzzleA.Type]--;
                        puzzlePyramid.PuzzleA = null;
                        break;
                    case ConsoleKey.B:
                        CountOfPuzzleTypes[puzzlePyramid.PuzzleB.Type]--;
                        puzzlePyramid.PuzzleB = null;
                        break;
                    case ConsoleKey.C:
                        CountOfPuzzleTypes[puzzlePyramid.PuzzleC.Type]--;
                        puzzlePyramid.PuzzleC = null;
                        break;
                    case ConsoleKey.D:
                        CountOfPuzzleTypes[puzzlePyramid.PuzzleD.Type]--;
                        puzzlePyramid.PuzzleD = null;
                        break;
                    case ConsoleKey.E:
                        CountOfPuzzleTypes[puzzlePyramid.PuzzleE.Type]--;
                        puzzlePyramid.PuzzleE = null;
                        break;
                    case ConsoleKey.F:
                        CountOfPuzzleTypes[puzzlePyramid.PuzzleF.Type]--;
                        puzzlePyramid.PuzzleF = null;
                        break;
                    case ConsoleKey.G:
                        CountOfPuzzleTypes[puzzlePyramid.PuzzleG.Type]--;
                        puzzlePyramid.PuzzleG = null;
                        break;
                    case ConsoleKey.H:
                        CountOfPuzzleTypes[puzzlePyramid.PuzzleH.Type]--;
                        puzzlePyramid.PuzzleH = null;
                        break;
                    case ConsoleKey.I:
                        CountOfPuzzleTypes[puzzlePyramid.PuzzleI.Type]--;
                        puzzlePyramid.PuzzleI = null;
                        break;
                    case ConsoleKey.J:
                        CountOfPuzzleTypes[puzzlePyramid.PuzzleJ.Type]--;
                        puzzlePyramid.PuzzleJ = null;
                        break;
                    case ConsoleKey.K:
                        CountOfPuzzleTypes[puzzlePyramid.PuzzleK.Type]--;
                        puzzlePyramid.PuzzleK = null;
                        break;
                    case ConsoleKey.L:
                        CountOfPuzzleTypes[puzzlePyramid.PuzzleL.Type]--;
                        puzzlePyramid.PuzzleL = null;
                        break;
                    default: //not a valid entry? Ready to leave;
                        readyToExit = true;
                        break;
                }
            }

        }

        public List<WordPuzzleType> PuzzlesWithoutClues = new List<WordPuzzleType>()
        {
            WordPuzzleType.WordSearchMoreOrLess, 
            WordPuzzleType.PhraseSegmentPuzzle, 
            WordPuzzleType.WordSquare, //Technically, has some clues, but not many, if it's a single word.
        };

        private void GenerateHtmlFile(PuzzlePyramid puzzlePyramid, string fileNameForHtml, bool includeSolution = false)
        {
            string html = puzzlePyramid.FormatHtmlForGoogle(includeSolution, false);
            File.WriteAllText(fileNameForHtml, html);
        }

        private static void WritePyramidToDisk(PuzzlePyramid puzzlePyramid, string fileName)
        {
            string serializedPyramid = JsonConvert.SerializeObject(puzzlePyramid);
            Program.ClearConsoleInputAndOutput();
            Console.WriteLine(serializedPyramid);
            File.WriteAllText(fileName, serializedPyramid);
            Console.WriteLine($"Wrote pyramid to {fileName}");
        }

        private string SelectClueToReplace(IPuzzle puzzle)
        {
            Program.ClearConsoleInputAndOutput();
            string clueToUse = null;

            while (string.IsNullOrWhiteSpace(clueToUse))
            {
                Console.WriteLine($"This puzzle ({puzzle.Description}) has the following clues. Select one to replace (or hit ENTER to quit):");
                var clues = puzzle.GetClues();
                for (var index = 0; index < clues.Count; index++)
                {
                    string clue = clues[index];
                    Console.WriteLine($"{index}: {clue}");
                }

                string userInput = Console.ReadLine();
                int selectedIndex;
                if (int.TryParse(userInput, out selectedIndex))
                {
                    clueToUse = clues[selectedIndex];
                }
                else
                {
                    return null;
                }
            }
            return clueToUse;
        }

        private void CreatePuzzleJAsMultipleCluesPuzzle(string solution, PuzzlePyramid puzzlePyramid)
        {
            var createdPuzzle = Program.InteractivelyGenerateSelectedPuzzleType(WordPuzzleType.MultipleClues ,
                solution.Length, solution, null);
            if (createdPuzzle != null)
            {
                puzzlePyramid.PuzzleJ = createdPuzzle;
            }
        }

        private void CreatePuzzleAAsWordSearch(string solution, PuzzlePyramid puzzlePyramid)
        {
            var createdPuzzle = Program.InteractivelyGenerateSelectedPuzzleType(WordPuzzleType.WordSearchMoreOrLess,
                solution.Length, solution, null);
            if (createdPuzzle != null)
            {
                puzzlePyramid.PuzzleA = createdPuzzle;
            }
        }

        private void CreatePuzzleBAsLettersAndArrows(string solution, PuzzlePyramid puzzlePyramid)
        {
            var createdPuzzle = Program.InteractivelyGenerateSelectedPuzzleType(WordPuzzleType.LettersAndArrows,
                solution.Length, solution, null);
            if (createdPuzzle != null)
            {
                puzzlePyramid.PuzzleB = createdPuzzle;
            }
        }

        private static IPuzzle CreatePuzzleAsType(string solution, List<WordPuzzleType> puzzleTypesToSkip = null)
        {
            var typeOfPuzzleToCreate = InteractivelySelectPuzzleType(solution, puzzleTypesToSkip);
            if (typeOfPuzzleToCreate == WordPuzzleType.Undefined)
            {
                return null;
            }
            var createdPuzzle = Program.InteractivelyGenerateSelectedPuzzleType(typeOfPuzzleToCreate,
                solution.Length, solution, null);
            if (createdPuzzle != null)
            {
                MovePuzzleTypeToEndOfList(typeOfPuzzleToCreate);
            }
            return createdPuzzle;
        }

        private static void MovePuzzleTypeToEndOfList(WordPuzzleType typeOfPuzzleToMoveToTheEnd)
        {
            List<WordPuzzleType> replacementList = new List<WordPuzzleType>();
            foreach (var puzzleType in SortedListOfPuzzleTypes)
            {
                if (puzzleType != typeOfPuzzleToMoveToTheEnd)
                {
                    replacementList.Add(puzzleType);
                }
            }
            replacementList.Add(typeOfPuzzleToMoveToTheEnd);
            SortedListOfPuzzleTypes = replacementList.ToArray();

            if (CountOfPuzzleTypes.ContainsKey(typeOfPuzzleToMoveToTheEnd))
            {
                CountOfPuzzleTypes[typeOfPuzzleToMoveToTheEnd]++;
            }
            else
            {
                CountOfPuzzleTypes[typeOfPuzzleToMoveToTheEnd] = 1;
            }
        }

        private static void InitializeSortedListOfPuzzleTypes(PuzzlePyramid puzzlePyramid)
        {
            List<WordPuzzleType> newList = new List<WordPuzzleType>();
            newList.Add(WordPuzzleType.MultipleClues);
            CountOfPuzzleTypes.Add(WordPuzzleType.MultipleClues, 0);
            newList.Add(WordPuzzleType.WordSquare);
            CountOfPuzzleTypes.Add(WordPuzzleType.WordSquare, 0);
            newList.Add(WordPuzzleType.TrisectedWords);
            CountOfPuzzleTypes.Add(WordPuzzleType.TrisectedWords, 0);
            newList.Add(WordPuzzleType.Anacrostic);
            CountOfPuzzleTypes.Add(WordPuzzleType.Anacrostic, 0);
            newList.Add(WordPuzzleType.LettersAndArrows);
            CountOfPuzzleTypes.Add(WordPuzzleType.LettersAndArrows, 0);
            newList.Add(WordPuzzleType.ReadDownColumn);
            CountOfPuzzleTypes.Add(WordPuzzleType.ReadDownColumn, 0);
            newList.Add(WordPuzzleType.WordSearchMoreOrLess);
            CountOfPuzzleTypes.Add(WordPuzzleType.WordSearchMoreOrLess, 0);
            newList.Shuffle();

            SortedListOfPuzzleTypes = newList.ToArray();
            //let's move existing types to the bottom. 
            foreach (IPuzzle puzzle in new List<IPuzzle>()
            {
                puzzlePyramid.PuzzleA,
                puzzlePyramid.PuzzleB,
                puzzlePyramid.PuzzleC,
                puzzlePyramid.PuzzleD,
                puzzlePyramid.PuzzleE,
                puzzlePyramid.PuzzleF,
                puzzlePyramid.PuzzleG,
                puzzlePyramid.PuzzleH,
                puzzlePyramid.PuzzleI,
                puzzlePyramid.PuzzleJ,
                puzzlePyramid.PuzzleK,
                puzzlePyramid.PuzzleL,

            })
            {
                if (puzzle != null)
                {
                    MovePuzzleTypeToEndOfList(puzzle.Type);
                }
            }

        }

        private static WordPuzzleType InteractivelySelectPuzzleType(string solution,
            List<WordPuzzleType> puzzleTypesToSkip)
        {
            if (puzzleTypesToSkip == null)
            {
                puzzleTypesToSkip = new List<WordPuzzleType>();
            }
            WordPuzzleType selectedPuzzleType = WordPuzzleType.Undefined;
            var iPuzzleTypes = Program.CalculateAvailableIPuzzleTypes(solution);
            List<WordPuzzleType> availableTypes = new List<WordPuzzleType>();
            foreach (var key in iPuzzleTypes.Keys)
            {
                if (puzzleTypesToSkip.Contains(key)) continue;
                if (iPuzzleTypes[key])
                {
                    availableTypes.Add(key);
                }
            }

            Console.WriteLine($"Select puzzle type for {solution}, just hit the number, don't press enter.");
            for (var index = 0; (index < SortedListOfPuzzleTypes.Length) && index < 10 ; index++)
            {
                WordPuzzleType puzzleType = SortedListOfPuzzleTypes[index];
                if (availableTypes.Contains(puzzleType))
                {
                    Console.WriteLine($"{index}: {puzzleType} {new string('*', CountOfPuzzleTypes[puzzleType])}");
                }
            }
            Console.Write(">>");

            var userInput = Console.ReadKey();
            int userInputIndex;
            if (int.TryParse(userInput.KeyChar.ToString(), out userInputIndex))
            {
                 selectedPuzzleType = SortedListOfPuzzleTypes[userInputIndex];
            }

            Program.ClearConsoleInputAndOutput();
            return selectedPuzzleType;
        }

        public static WordPuzzleType[] SortedListOfPuzzleTypes { get; set; } 
        public static Dictionary<WordPuzzleType, int> CountOfPuzzleTypes = new Dictionary<WordPuzzleType, int>();

        private void CreatePuzzleJ(List<string> wordsToReplace, PuzzlePyramid puzzlePyramid)
        {
            string solutionToPuzzleJ = wordsToReplace[0];
            var availablePuzzleTypes = Program.CalculateAvailableIPuzzleTypes(solutionToPuzzleJ);
            IPuzzle createdPuzzle = null;

            while (createdPuzzle == null)
            {
                var userPuzzleSelection = Program.DisplayMenuOfAvailablePuzzles(solutionToPuzzleJ, availablePuzzleTypes);
                createdPuzzle = Program.InteractivelyGenerateSelectedPuzzleType(userPuzzleSelection,
                    solutionToPuzzleJ.Length, solutionToPuzzleJ, null);
                if (createdPuzzle != null)
                {
                    puzzlePyramid.PuzzleJ = createdPuzzle;
                }
            }
        }

        internal static bool SelectPersonToQuote(PuzzlePyramid puzzlePyramid)
        {
            Person selectedPerson = null;
            int countOfPeopleBornInThisTimeRange = puzzlePyramid.PeopleBornInRange.Count;
            for (var index = 0; index < countOfPeopleBornInThisTimeRange; index++)
            {
                Person person = puzzlePyramid.PeopleBornInRange[index];
                Program.ClearConsoleInputAndOutput();
                Console.WriteLine(
                    $"{index}/{countOfPeopleBornInThisTimeRange}: {person.Name} ({person.Year}) has {person.Quotes.Count} quotes.");
                Console.WriteLine($"{person.Quotes[0]}");
                Console.WriteLine($"Press 'y' to select this person, any other key to go to the next person.");

                var userInput = Console.ReadKey();
                if (userInput.Key == ConsoleKey.Y)
                {
                    selectedPerson = person;
                    Program.ClearConsoleInputAndOutput();
                    int numberOfQuotesToDisplay = 10;
                    if (selectedPerson.Quotes.Count < numberOfQuotesToDisplay)
                    {
                        numberOfQuotesToDisplay = selectedPerson.Quotes.Count;
                    }

                    for (int quoteIndex = 0; quoteIndex < numberOfQuotesToDisplay; quoteIndex++)
                    {
                        Console.WriteLine($"{quoteIndex}: {selectedPerson.Quotes[quoteIndex]}");
                    }

                    Console.WriteLine(
                        "Which quote would you like to use? Press the number, or any other key to go to the next person.");
                    var userInputQuoteSelection = Console.ReadKey();
                    int quoteIndexSelected;
                    // Then we pick a quote from that person to be the ultimate solution in the puzzle. 
                    if (Int32.TryParse(userInputQuoteSelection.KeyChar.ToString(), out quoteIndexSelected))
                    {
                        string selectedQuote = selectedPerson.Quotes[quoteIndexSelected];
                        puzzlePyramid.SelectedPerson = selectedPerson;
                        puzzlePyramid.SelectedQuote = selectedQuote;
                        break;
                    }
                    else
                    {
                        selectedPerson = null;
                    }
                }
            }

            if (selectedPerson == null)
            {
                Program.ClearConsoleInputAndOutput();
                Console.WriteLine("Sorry you weren't able to find anyone you liked. Press any key to exit.");
                Console.ReadKey();
                return true;
            }

            return false;
        }

        internal static List<string> SelectWordsToReplace(PuzzlePyramid puzzlePyramid)
        {
            List<string> wordsInQuote = new List<string>(
                puzzlePyramid.SelectedQuote.Split(new[] {" ", ",", "."}, StringSplitOptions.RemoveEmptyEntries));
            wordsInQuote = RemoveDuplicatesAndShortWords(wordsInQuote);
            List<string> wordsToReplace = new List<string>();
            while (wordsToReplace.Count < 3)
            {
                //      Select a word from the quote 
                int wordsToDisplay = 10;
                if (wordsInQuote.Count < wordsToDisplay)
                {
                    wordsToDisplay = wordsInQuote.Count;
                }

                Program.ClearConsoleInputAndOutput();
                DisplayQuoteWithRemovedWords(puzzlePyramid.SelectedQuote, wordsToReplace);
                Console.WriteLine($"Words already replaced: {String.Join(",", wordsToReplace)}");
                for (int wordToRemoveIndex = 0; wordToRemoveIndex < wordsToDisplay; wordToRemoveIndex++)
                {
                    string currentWord = wordsInQuote[wordToRemoveIndex];
                    if (wordsToReplace.Contains(currentWord.ToLowerInvariant()))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                    }
                    else
                    {
                        if (currentWord.Length < 4)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Gray;
                        }
                    }

                    Console.WriteLine($"{wordToRemoveIndex}: {currentWord}");
                }

                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("Press a key indicating the next word that should be hidden (or un-hidden). Hit ENTER to skip.");
                Console.Write(">>");
                bool wordSelected = false;
                while (!wordSelected)
                {
                    var userInputWordToHide = Console.ReadKey();
                    int indexOfWordToHide;
                    if (Int32.TryParse(userInputWordToHide.KeyChar.ToString(), out indexOfWordToHide))
                    {
                        string selectedWord = wordsInQuote[indexOfWordToHide].ToLowerInvariant();
                        if (wordsToReplace.Contains(selectedWord))
                        {
                            wordsToReplace.Remove(selectedWord);
                        }
                        else
                        {
                            wordsToReplace.Add(selectedWord);
                        }

                        wordSelected = true;
                    }
                    else
                    {
                        Console.WriteLine("Didn't understand that input. Try again?");
                    }
                }
            }

            string selectedQuoteWithReplacedWords =
                PuzzlePyramid.ReplaceWordsWithMarkers(puzzlePyramid.SelectedQuote, wordsToReplace);
            Program.ClearConsoleInputAndOutput();
            Console.WriteLine(selectedQuoteWithReplacedWords);
            puzzlePyramid.SelectedQuoteWithReplacedWords = selectedQuoteWithReplacedWords;
            return wordsToReplace;
        }

        private static List<string> RemoveDuplicatesAndShortWords(List<string> wordsInQuote)
        {
            List<string> removeDuplicates = new List<string>();
            foreach (var word in wordsInQuote)
            {
                string currentWord = word.ToLowerInvariant();
                if (currentWord.Length < 3) continue;
                if (removeDuplicates.Contains(currentWord)) continue;
                removeDuplicates.Add(currentWord);

            }
            return removeDuplicates;
        }

        private static void DisplayQuoteWithRemovedWords(string selectedQuote, List<string> wordsToReplace)
        {
            foreach (string word in selectedQuote.Split(new[] {" "}, StringSplitOptions.RemoveEmptyEntries))
            {
                string currentWordWithoutPunctuation = RemovePunctuation(word);
                if (wordsToReplace.Contains(currentWordWithoutPunctuation))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
                Console.Write(word);
                Console.Write(" ");
            }
            Console.WriteLine();
        }

        private static string RemovePunctuation(string word)
        {
            StringBuilder builder = new StringBuilder();
            foreach (char letter in word)
            {
                if (Char.IsLetter(letter))
                {
                    builder.Append(Char.ToLowerInvariant(letter));
                }
            }
            return builder.ToString();
        }
    }
}