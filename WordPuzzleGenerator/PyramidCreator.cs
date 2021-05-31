using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WordPuzzles.Puzzle;
using WordPuzzles.Puzzle.Legacy;
using WordPuzzles.Utility;

namespace WordPuzzleGenerator 
{
    internal class PyramidCreator 
    {
        private const int MONTH = 7;
        private const int DAY = 16;

        private const string SOLVE_PUZZLE_A = "(solve puzzle A)";
        private const string SOLVE_PUZZLE_B = "(solve puzzle B)";
        private const string SOLVE_PUZZLE_C = "(solve puzzle C)";
        private const string SOLVE_PUZZLE_D = "(solve puzzle D)";
        private const string SOLVE_PUZZLE_E = "(solve puzzle E)";
        private const string SOLVE_PUZZLE_F = "(solve puzzle F)";
        private const string SOLVE_PUZZLE_G = "(solve puzzle G)";
        private const string SOLVE_PUZZLE_H = "(solve puzzle H)";
        private const string SOLVE_PUZZLE_I = "(solve puzzle I)";

        public PyramidCreator()
        {
        }

        public void RunInPyramidMode()
        {

            PuzzlePyramid puzzlePyramid = new PuzzlePyramid();
            // First, we select a start date
            puzzlePyramid.StartDate = new DateTime(2021, MONTH, DAY, 
                _random.Next(1, 10), _random.Next(10, 60), 0);
            if (puzzlePyramid.StartDate.DayOfWeek != DayOfWeek.Friday)
            {
                throw new Exception("Date should be a friday. ");
            }
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
                            puzzlePyramid.PuzzleJ.ReplaceClue(clueToReplace, SOLVE_PUZZLE_A);
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
                            puzzlePyramid.PuzzleJ.ReplaceClue(clueToReplace, SOLVE_PUZZLE_B);
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
                            puzzlePyramid.PuzzleJ.ReplaceClue(clueToReplace, SOLVE_PUZZLE_C);
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
                            puzzlePyramid.PuzzleK.ReplaceClue(clueToReplace, SOLVE_PUZZLE_D);
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
                            puzzlePyramid.PuzzleK.ReplaceClue(clueToReplace, SOLVE_PUZZLE_E);
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
                            puzzlePyramid.PuzzleK.ReplaceClue(clueToReplace, SOLVE_PUZZLE_F);
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
                            puzzlePyramid.PuzzleL.ReplaceClue(clueToReplace, SOLVE_PUZZLE_G);
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
                            puzzlePyramid.PuzzleL.ReplaceClue(clueToReplace, SOLVE_PUZZLE_H);
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
                            puzzlePyramid.PuzzleL.ReplaceClue(clueToReplace, SOLVE_PUZZLE_I);
                        }
                    }
                }

            }

            GenerateHtmlFile(puzzlePyramid, fileNameForHtml);
            GenerateHtmlFile(puzzlePyramid, fileNameWithSolutionsForHtml, true);

            WritePyramidToDisk(puzzlePyramid, fileNameForJson);
            PopulateWebFiles(puzzlePyramid);

            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();

        }

        private static string CalculateFtpDirectory(PuzzlePyramid puzzlePyramid)
        {
            string ftpDirectory2 =
                $@"pptest/v2/{puzzlePyramid.StartDate.Month}-{puzzlePyramid.StartDate.Day}";
            if ((puzzlePyramid.StartDate.Hour != 0) && (puzzlePyramid.StartDate.Minute != 0))
            {
                ftpDirectory2 += $"-{puzzlePyramid.StartDate.Hour}{puzzlePyramid.StartDate.Minute}";
            }

            return ftpDirectory2;
        }

        private static string CalculateDirectoryNameForWebFiles(PuzzlePyramid puzzlePyramid)
        {
            string directoryNameForWebFiles2 =
                $@"{Program.BASE_DIRECTORY}\pyramids\{puzzlePyramid.StartDate.Month}-{puzzlePyramid.StartDate.Day}";
            if ((puzzlePyramid.StartDate.Hour != 0) && (puzzlePyramid.StartDate.Minute != 0))
            {
                directoryNameForWebFiles2 += $"-{puzzlePyramid.StartDate.Hour}{puzzlePyramid.StartDate.Minute}";
            }

            return directoryNameForWebFiles2;
        }


        private void PopulateWebFiles(PuzzlePyramid puzzlePyramid)
        {
            string directory = CalculateDirectoryNameForWebFiles(puzzlePyramid);
            string remoteDirectory = CalculateFtpDirectory(puzzlePyramid);
            FtpHelper ftpHelper = new FtpHelper();
            ftpHelper.ReadCredentialsFromFile();
            ftpHelper.CreateDirectory(remoteDirectory);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            //Create json file
            JObject puzzles = new JObject();
            JObject metaData = new JObject();
            metaData["header"] =
                $"Puzzle Pyramid: {puzzlePyramid.StartDate.ToShortDateString()} - {puzzlePyramid.StartDate.AddDays(6).ToShortDateString()}";
            metaData["subheader"] = "By Chip Beauvais and Chester Monty © 2021";
            metaData["quote"] = puzzlePyramid.SelectedQuoteWithReplacedWords;
            puzzles["puzzle_metadata"] = metaData;
            var puzzleArray = new JArray();
            char currentCharacter = 'A';
            foreach (var puzzle in new IPuzzle[]
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
                    puzzleArray.Add(puzzle.GenerateJsonFileForMonty($"Puzzle {currentCharacter++}"));
                }
            }
            puzzles["puzzles"] = puzzleArray;

            File.WriteAllText($@"{directory}\puzzle_data.js", $"const puzzle_data = {puzzles}");
            ftpHelper.UploadFile($@"{directory}\puzzle_data.js", $@"{remoteDirectory}/puzzle_data.js");

            //Copy other files. 
            string relativePathOfSourceFiles = @"..\..\..\PuzzlePyramids";
            foreach (string path in Directory.GetFiles(relativePathOfSourceFiles))
            {
                Console.WriteLine(path);
                string fileName = Path.GetFileName(path);
                if (fileName == "puzzle_data.js") continue;
                if (fileName == "package-lock.json") continue;
                File.Copy(path, $@"{directory}\{fileName}", true);
                ftpHelper.UploadFile($@"{directory}\{fileName}", $@"{remoteDirectory}/{fileName}", fileName.ToLowerInvariant().Contains("png"));
            }
            Console.WriteLine("Those are files  in a parent directory. Hit any key to continue. ");
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

                DisplayCountOfPuzzleTypes();


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
                        puzzlePyramid.PuzzleJ.ReplaceClue(SOLVE_PUZZLE_A, puzzlePyramid.PuzzleA.Solution);
                        puzzlePyramid.PuzzleA = null;
                        break;
                    case ConsoleKey.B:
                        CountOfPuzzleTypes[puzzlePyramid.PuzzleB.Type]--;
                        puzzlePyramid.PuzzleJ.ReplaceClue(SOLVE_PUZZLE_B, puzzlePyramid.PuzzleB.Solution);
                        puzzlePyramid.PuzzleB = null;
                        break;
                    case ConsoleKey.C:
                        CountOfPuzzleTypes[puzzlePyramid.PuzzleC.Type]--;
                        puzzlePyramid.PuzzleJ.ReplaceClue(SOLVE_PUZZLE_C, puzzlePyramid.PuzzleC.Solution);
                        puzzlePyramid.PuzzleC = null;
                        break;

                    case ConsoleKey.D:
                        CountOfPuzzleTypes[puzzlePyramid.PuzzleD.Type]--;
                        puzzlePyramid.PuzzleK.ReplaceClue(SOLVE_PUZZLE_D, puzzlePyramid.PuzzleD.Solution);
                        puzzlePyramid.PuzzleD = null;
                        break;
                    case ConsoleKey.E:
                        CountOfPuzzleTypes[puzzlePyramid.PuzzleE.Type]--;
                        puzzlePyramid.PuzzleK.ReplaceClue(SOLVE_PUZZLE_E, puzzlePyramid.PuzzleE.Solution);
                        puzzlePyramid.PuzzleE = null;
                        break;
                    case ConsoleKey.F:
                        CountOfPuzzleTypes[puzzlePyramid.PuzzleF.Type]--;
                        puzzlePyramid.PuzzleK.ReplaceClue(SOLVE_PUZZLE_F, puzzlePyramid.PuzzleF.Solution);
                        puzzlePyramid.PuzzleF = null;
                        break;

                    case ConsoleKey.G:
                        CountOfPuzzleTypes[puzzlePyramid.PuzzleG.Type]--;
                        puzzlePyramid.PuzzleL.ReplaceClue(SOLVE_PUZZLE_G, puzzlePyramid.PuzzleG.Solution);
                        puzzlePyramid.PuzzleG = null;
                        break;
                    case ConsoleKey.H:
                        CountOfPuzzleTypes[puzzlePyramid.PuzzleH.Type]--;
                        puzzlePyramid.PuzzleL.ReplaceClue(SOLVE_PUZZLE_H, puzzlePyramid.PuzzleH.Solution);
                        puzzlePyramid.PuzzleH = null;
                        break;
                    case ConsoleKey.I:
                        CountOfPuzzleTypes[puzzlePyramid.PuzzleI.Type]--;
                        puzzlePyramid.PuzzleL.ReplaceClue(SOLVE_PUZZLE_I, puzzlePyramid.PuzzleI.Solution);
                        puzzlePyramid.PuzzleI = null;
                        break;
                    case ConsoleKey.J:
                        CountOfPuzzleTypes[puzzlePyramid.PuzzleJ.Type]--;
                        puzzlePyramid.PuzzleJ = null;
                        if (puzzlePyramid.PuzzleA != null)
                        {
                            CountOfPuzzleTypes[puzzlePyramid.PuzzleA.Type]--;
                            puzzlePyramid.PuzzleA = null;
                        }
                        if (puzzlePyramid.PuzzleB != null)
                        {
                            CountOfPuzzleTypes[puzzlePyramid.PuzzleB.Type]--;
                            puzzlePyramid.PuzzleB = null;
                        }
                        if (puzzlePyramid.PuzzleC != null)
                        {
                            CountOfPuzzleTypes[puzzlePyramid.PuzzleC.Type]--;
                            puzzlePyramid.PuzzleC = null;
                        }

                        break;
                    case ConsoleKey.K:
                        CountOfPuzzleTypes[puzzlePyramid.PuzzleK.Type]--;
                        puzzlePyramid.PuzzleK = null;
                        if (puzzlePyramid.PuzzleD != null)
                        {
                            CountOfPuzzleTypes[puzzlePyramid.PuzzleD.Type]--;
                            puzzlePyramid.PuzzleD = null;
                        }
                        if (puzzlePyramid.PuzzleE != null)
                        {
                            CountOfPuzzleTypes[puzzlePyramid.PuzzleE.Type]--;
                            puzzlePyramid.PuzzleE = null;
                        }
                        if (puzzlePyramid.PuzzleF != null)
                        {
                            CountOfPuzzleTypes[puzzlePyramid.PuzzleF.Type]--;
                            puzzlePyramid.PuzzleF = null;
                        }
                        break;
                    case ConsoleKey.L:
                        CountOfPuzzleTypes[puzzlePyramid.PuzzleL.Type]--;
                        puzzlePyramid.PuzzleL = null;
                        if (puzzlePyramid.PuzzleG != null)
                        {
                            CountOfPuzzleTypes[puzzlePyramid.PuzzleG.Type]--;
                            puzzlePyramid.PuzzleG = null;
                        }
                        if (puzzlePyramid.PuzzleH != null)
                        {
                            CountOfPuzzleTypes[puzzlePyramid.PuzzleH.Type]--;
                            puzzlePyramid.PuzzleH = null;
                        }
                        if (puzzlePyramid.PuzzleI != null)
                        {
                            CountOfPuzzleTypes[puzzlePyramid.PuzzleI.Type]--;
                            puzzlePyramid.PuzzleI = null;
                        }
                        break;
                    default: //not a valid entry? Ready to leave;
                        readyToExit = true;
                        break;
                }
            }

        }

        private void DisplayCountOfPuzzleTypes()
        {
            Console.WriteLine();
            List<string> puzzleTypeCounts = new List<string>();

            foreach (var key in CountOfPuzzleTypes.Keys)
            {
                puzzleTypeCounts.Add($"{CountOfPuzzleTypes[key]} {key}");
            }
            puzzleTypeCounts.Sort();
            foreach (string puzzleTypeSummary in puzzleTypeCounts)
            {
                Console.WriteLine(puzzleTypeSummary);
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
                    int score = CalculateClueRating(clue);
                    if (score == 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Gray;
                    }
                    Console.WriteLine($"{index}: {clue} {score}");
                    Console.ForegroundColor = ConsoleColor.Gray;
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

        private int CalculateClueRating(string solutionCandidate, List<WordPuzzleType> puzzleTypesToSkip = null)
        {
            int rating = 0;

            if (solutionCandidate.Contains("_")) return 0;
            if ((new List<string> {SOLVE_PUZZLE_A, SOLVE_PUZZLE_B, SOLVE_PUZZLE_C, SOLVE_PUZZLE_D, SOLVE_PUZZLE_E, SOLVE_PUZZLE_F, SOLVE_PUZZLE_G, SOLVE_PUZZLE_H, SOLVE_PUZZLE_I}).Contains(solutionCandidate)) return 0;

            if (puzzleTypesToSkip == null)
            {
                puzzleTypesToSkip = new List<WordPuzzleType>();
            }
            var iPuzzleTypes = Program.CalculateAvailableIPuzzleTypes(solutionCandidate);
            List<WordPuzzleType> availableTypes = new List<WordPuzzleType>();
            foreach (var key in iPuzzleTypes.Keys)
            {
                if (puzzleTypesToSkip.Contains(key)) continue;
                if (iPuzzleTypes[key])
                {
                    availableTypes.Add(key);
                }
            }

            foreach (var type in availableTypes)
            {
                if (!CountOfPuzzleTypes.ContainsKey(type)) continue;
                if (CountOfPuzzleTypes[type] == 0)
                {
                    rating += 10;
                }
                if (CountOfPuzzleTypes[type] == 1)
                {
                    rating += 3;
                }
                if (CountOfPuzzleTypes[type] == 2)
                {
                    rating += 1;
                }
            }
            return rating;
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
                    Console.WriteLine($"{index}: {puzzleType} {new string('*', CountOfPuzzleTypes[puzzleType])} (~{EstimateClueCount(puzzleType, solution)} clues)");
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

        private static int EstimateClueCount(WordPuzzleType puzzleType, string solution)
        {
            int lettersInSolution = 0;
            foreach (var character in solution)
            {
                if (char.IsLetter(character))
                {
                    lettersInSolution++;
                }
            }

            switch (puzzleType)
            {
                case WordPuzzleType.MultipleClues:
                    return (int) (lettersInSolution * 2.5);
                case WordPuzzleType.Anacrostic:
                    return (int) lettersInSolution / 3;
                case WordPuzzleType.LettersAndArrows:
                    return (int) (lettersInSolution / 2) + 1;
                case WordPuzzleType.ReadDownColumn:
                    return lettersInSolution;
                case WordPuzzleType.TrisectedWords:
                    return (int) (lettersInSolution / 2.5);
                case WordPuzzleType.WordSearchMoreOrLess:
                    return lettersInSolution;
                case WordPuzzleType.WordSquare:
                    return lettersInSolution - 1;
                default:
                    throw new ArgumentOutOfRangeException(nameof(puzzleType), "Unexpected puzzle type.");
            }
        }

        public static WordPuzzleType[] SortedListOfPuzzleTypes { get; set; } 
        public static Dictionary<WordPuzzleType, int> CountOfPuzzleTypes = new Dictionary<WordPuzzleType, int>();
        private Random _random = new Random();

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

        public void ReprocessExistingFiles()
        {
            foreach (string fileNameForJson in Directory.EnumerateFiles(Program.BASE_DIRECTORY + @"/pyramids/", "*.json"))
            {
                var puzzlePyramid = JsonConvert.DeserializeObject<PuzzlePyramid>(File.ReadAllText(fileNameForJson));
                Console.WriteLine($"Loaded pyramid from file {fileNameForJson}.");
                PopulateWebFiles(puzzlePyramid);
            }
            Console.WriteLine("Done. Press any key to exit.");
            Console.ReadKey();
        }
    }
}