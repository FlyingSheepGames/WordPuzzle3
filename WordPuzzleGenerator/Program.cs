using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WordPuzzles;
using WordPuzzles.Puzzle;
using WordPuzzles.Puzzle.Legacy;
using WordPuzzles.Utility;

namespace WordPuzzleGenerator
{
    class Program
    {
        private const ConsoleColor ERROR_CONSOLE_COLOR = ConsoleColor.DarkRed;

        // ReSharper disable once InconsistentNaming
        private static readonly string BASE_DIRECTORY = ConfigurationManager.AppSettings["BaseDirectory"]; //@"E:\utilities\WordSquare\data\";
        static readonly WordRepository WordRepository = new WordRepository() {ExcludeAdvancedWords = true};
        static readonly WordSquareHistory History = new WordSquareHistory(); //Todo: populate
        static readonly AnagramFinder AnagramFinder = new AnagramFinder() {Repository = WordRepository};
        static readonly Random RandomNumberGenerator = new Random();
        static readonly ClueRepository ClueRepository = new ClueRepository();
        private static readonly StringBuilder PuzzleBuilder = new StringBuilder();
        private static readonly StringBuilder SolutionBuilder = new StringBuilder();
        private static DateTime TIME_TO_QUIT = DateTime.MaxValue;

        [STAThread]
        static void Main()
        {

            //CalculateStatisticsForThreeLetterWords();

            ClueRepository.ReadFromDisk(@"C:\Users\Chip\Source\Repos\WordPuzzle3\WordPuzzlesTest\data\PUZ\allclues.json");


            //FindAllTakeOneClues();
            //FindAllTakeTwoClues();
            //LoadSevenLetterWords();
            //FindWordsThatMakeDigits();

            //InteractiveHideTheseWords(new List<string>() {"winter", "spring", "fall", "autumn"});
            //ListWordsThatCanBeShifted();
            /*
            ListWordsThatCanPrependALetter("a");
            Console.ReadKey();
            ListWordsThatCanPrependALetter("i");
            Console.ReadKey();
            */
            ProgramMode programMode = ProgramMode.UNDEFINED;
            programMode = ProgramMode.YEAR; //TODO: Delete this line to let the user choose. 
            while (programMode == ProgramMode.UNDEFINED)
            {
                programMode = InteractiveGetProgramMode();
            }

            if (programMode == ProgramMode.COLLECTION)
            {
                RunInCollectionMode();
            }

            if (programMode == ProgramMode.PATTERN_MATCH)
            {
                InterativelyCreatesWordsWithSelectedPattern();
            }

            if (programMode == ProgramMode.YEAR)
            {
                RunInYearMode();
            }

        }

        private static void RunInCollectionMode()
        {
            PuzzleCollection collection = InteractiveSelectPuzzleCollection();
            if (0 < collection.PuzzleCount)
            {
                InteractiveDeletePuzzles(collection);
            }

            HtmlGenerator htmlGenerator = new HtmlGenerator();

            htmlGenerator.AppendHtmlHeader(PuzzleBuilder);
            htmlGenerator.AppendHtmlHeader(SolutionBuilder);

            string solution = "Placeholder that is not empty";
            while (!string.IsNullOrWhiteSpace(solution))
            {
                Console.Clear();
                Console.WriteLine("Enter the word or phrase you'd like to create a puzzle for.");

                solution = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(solution))
                {
                    break;
                }

                int solutionLength = solution.Length;

                List<string> solutionThemes = InteractiveFindThemesForWord(solution);
                WordPuzzleType
                    userPuzzleSelection =
                        WordPuzzleType.WordSquare; //Doesn't matter what it is as long as it isn't 0.
                Dictionary<WordPuzzleType, bool> availablePuzzleTypes =
                    CalculateAvailablePuzzleTypes(solution, solutionThemes);

                while (userPuzzleSelection != 0)
                {
                    userPuzzleSelection = DisplayMenuOfAvailablePuzzles(solution, availablePuzzleTypes);
                    IPuzzle createdPuzzle = InteractivelyGenerateSelectedPuzzleType(userPuzzleSelection,
                        solutionLength, solution,
                        solutionThemes);
                    AddPuzzleToCollection(createdPuzzle, collection, PuzzleBuilder, SolutionBuilder);
                }
            }


            ClueRepository.WriteToDisk(
                @"C:\Users\Chip\Source\Repos\WordPuzzle3\WordPuzzlesTest\data\PUZ\allclues.json");

            htmlGenerator.AppendHtmlFooter(PuzzleBuilder);
            htmlGenerator.AppendHtmlFooter(SolutionBuilder);
            long ticks = DateTime.Now.Ticks;
            File.WriteAllText($"{ticks}_puzzles.html", PuzzleBuilder.ToString());
            File.WriteAllText($"{ticks}_solutions.html", SolutionBuilder.ToString());

            SavePuzzleCollection(collection);
        }

        private static void RunInYearMode()
        {
            YearOfPuzzles yearOfPuzzles = new YearOfPuzzles();
            //Load year
            string pathToYearOfPuzzlesFile = LoadExistingPuzzles(yearOfPuzzles);

            bool continueMainLoop = true;

            while (continueMainLoop)
            {
                //Select Date to work on
                DateTime userSelectedDate = DateTime.MinValue;
                while (userSelectedDate == DateTime.MinValue)
                {
                    userSelectedDate = InteractiveGetDateToWorkOn(yearOfPuzzles.NextOpenDate());
                }

                if (userSelectedDate == TIME_TO_QUIT)
                {
                    return;
                }

                string solution = "Placeholder that is not empty";
                while (!string.IsNullOrWhiteSpace(solution))
                {
                    ClearConsoleInputAndOutput();
                    Console.WriteLine($"What is the solution for the puzzle on {userSelectedDate.ToLongDateString()}?");
                    Console.WriteLine("Or enter '111' to create a quotation puzzle for this date.");
                    Console.WriteLine("Or hit enter to quit. ");

                    solution = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(solution))
                    {
                        break;
                    }

                    if (solution == "111")
                    {
                        var puzzle = InteractiveGetQuotationPuzzleForDate(userSelectedDate);
                        if (puzzle != null)
                        {
                            yearOfPuzzles.Add(puzzle, userSelectedDate);
                        }
                        yearOfPuzzles.Serialize(pathToYearOfPuzzlesFile);
                        break;
                    }

                    int solutionLength = solution.Length;

                    WordPuzzleType
                        userPuzzleSelection =
                            WordPuzzleType.WordSquare; //Doesn't matter what it is as long as it isn't 0.
                    Dictionary<WordPuzzleType, bool> availablePuzzleTypes =
                        CalculateAvailableIPuzzleTypes(solution);

                    while (userPuzzleSelection != 0)
                    {
                        userPuzzleSelection = DisplayMenuOfAvailablePuzzles(solution, availablePuzzleTypes);
                        IPuzzle createdPuzzle = InteractivelyGenerateSelectedPuzzleType(userPuzzleSelection,
                            solutionLength, solution,
                            null);
                        if (createdPuzzle != null)
                        {
                            yearOfPuzzles.Add(createdPuzzle, userSelectedDate);
                            userPuzzleSelection = 0; //reset solution to jump out of loop.
                        }

                    }
                }


                ClueRepository.WriteToDisk(
                    @"C:\Users\Chip\Source\Repos\WordPuzzle3\WordPuzzlesTest\data\PUZ\allclues.json");

                //todo: persist year
                yearOfPuzzles.Serialize(pathToYearOfPuzzlesFile);

                ClearConsoleInputAndOutput();
                Console.WriteLine("Files updated. Press 'z' to quit, any other key to continue with the next date.");
                var keyPressed = Console.ReadKey();
                continueMainLoop = (keyPressed.KeyChar != 'z');
            }
            //todo: use HTML Generator if prompted by user.

        }

        private static DateTime InteractiveGetDateToWorkOn(DateTime defaultDate)
        {
            ClearConsoleInputAndOutput();
            Console.WriteLine("Please enter a date to work with. ");
            Console.WriteLine($"Just hit enter to work with {defaultDate} ");
            Console.WriteLine($"Enter 'z' to quit. "); //can actually enter anything to quit. 

            string userInput = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(userInput))
            {
                return defaultDate;
            }

            DateTime userEntereDateTime;

            if (DateTime.TryParse(userInput, out userEntereDateTime))
            {
                return userEntereDateTime;
            }

            return TIME_TO_QUIT;
        }

        private static string LoadExistingPuzzles(YearOfPuzzles yearOfPuzzles)
        {
            string directory = Path.Combine(BASE_DIRECTORY, "year");
            Directory.CreateDirectory(directory);
            string yearPath = $@"{directory}\2021.json";
            if (File.Exists(yearPath))
            {
                yearOfPuzzles.Deserialize(yearPath);
            }

            return yearPath;
        }

        private static ProgramMode InteractiveGetProgramMode()
        {
            ClearConsoleInputAndOutput();
            Console.WriteLine("What would you like to do? ");
            Console.WriteLine($"{ProgramMode.COLLECTION}: Work with a collection of puzzles. ");
            Console.WriteLine($"{ProgramMode.YEAR}: Work with a year of puzzles. ");
            Console.WriteLine($"{ProgramMode.PATTERN_MATCH}: Match some arbitrary patterns. ");
            string userInputAsString =  Console.ReadLine();
            int userInput;
            if (int.TryParse(userInputAsString, out userInput))
            {
                return (ProgramMode) userInput;
            }

            return ProgramMode.UNDEFINED;
        }
        /*
         * Discoveries:
         * 1. J is almost always the first letter of a three letter word containing it.
         * 2. IOU are almost always the middle letter of words in which they appear.
         * 3. X is almost always the last letter of any three letter word containing it. 
         *
         */

        private static void CalculateStatisticsForThreeLetterWords()
        {
            int[,] letterFrequency = new int[26,3];
            for (int letter = 'a' -'a'; letter < 'z'-'a'; letter++)
            {
                for (int wordIndex = 0; wordIndex < 3; wordIndex++)
                {
                    letterFrequency[letter, wordIndex] = 0;
                }
            }

            foreach (string word in WordRepository.WordsMatchingPattern("___"))
            {
                for (var indexOfLetterInWord = 0; indexOfLetterInWord < word.ToLowerInvariant().Length; indexOfLetterInWord++)
                {
                    char letter = word.ToLowerInvariant()[indexOfLetterInWord];
                    letterFrequency[letter - 'a', indexOfLetterInWord]++;
                }
            }
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("LETTER\tSTART\tMIDDLE\tEND");
            for (int letter = 'a' - 'a'; letter < 'z' - 'a'; letter++)
            {
                int startCount = letterFrequency[letter, 0];
                int middleCount = letterFrequency[letter, 1];
                int endCount = letterFrequency[letter, 2];
                int totalCount = startCount + middleCount + endCount;
                if (totalCount == 0) continue;
                int startOfTen = (startCount*10)/totalCount;
                int middleOfTen = (middleCount * 10) / totalCount;
                int endOfTen = (endCount * 10) / totalCount;
                middleOfTen = 10 - (startOfTen + endOfTen);
                Console.Write($"{(char) (letter + 'A')}\t");//{startOfTen}\t{middleOfTen}\t{endOfTen}");

                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write( new string('#', startOfTen));
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write(new string('#', middleOfTen));
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(new string('#', endOfTen));

                Console.ForegroundColor = ConsoleColor.Gray;

                Console.WriteLine();
                for (int wordIndex = 0; wordIndex < 3; wordIndex++)
                {
                    letterFrequency[letter, wordIndex] = 0;
                }
            }

            Console.ReadKey();
        }

        /*
         *Results
         * Found 99 clues.
a has 1 clue pairs.
d has 8 clue pairs.
e has 5 clue pairs.
f has 2 clue pairs.
g has 4 clue pairs.
h has 1 clue pairs.
i has 1 clue pairs.
l has 12 clue pairs.
m has 1 clue pairs.
n has 1 clue pairs.
o has 2 clue pairs.
p has 3 clue pairs.
r has 6 clue pairs.
s has 48 clue pairs.
t has 3 clue pairs.
v has 1 clue pairs.

         *
         */
        private static List<TakeTwoClue> FindAllTakeTwoClues()
        {
            return new List<TakeTwoClue>();
            StringBuilder pattern = new StringBuilder();
            long wordsConsidered = 0;
            int[] countPerLetterRemoved = new int[26];
            for (int i = 0; i < 26; i++)
            {
                countPerLetterRemoved[i] = 0;
            }
            var findAllTakeTwoClues = new List<TakeTwoClue>();
            for (char letterToPlace = 'a'; letterToPlace <= 'z'; letterToPlace++)
            {
                for (int wordLength = 4; wordLength < 9; wordLength++)
                {
                    for (int firstLetterIndex = 0; firstLetterIndex < wordLength - 1; firstLetterIndex++)
                    {
                        for (int secondLetterIndex = firstLetterIndex + 1;
                            secondLetterIndex < wordLength;
                            secondLetterIndex++)
                        {
                            var placeLetterAtTheseIndicies = new List<int>() { firstLetterIndex, secondLetterIndex };
                            pattern.Clear();
                            for (int patternIndex = 0; patternIndex < wordLength; patternIndex++)
                            {
                                char letterToAppend = '_';
                                if (placeLetterAtTheseIndicies.Contains(patternIndex))
                                {
                                    letterToAppend = letterToPlace;
                                }

                                pattern.Append(letterToAppend);
                            }

                            string patternAsString = pattern.ToString();
                            foreach (var longerWord in WordRepository.WordsMatchingPattern(patternAsString))
                            {
                                wordsConsidered++;
                                if (wordsConsidered % 50 == 0)
                                {
                                    Console.WriteLine($"Considered {wordsConsidered} words so far.");
                                }
                                string shorterWord = longerWord.Replace(letterToPlace.ToString(), "");
                                if (WordRepository.IsAWord(shorterWord))
                                {
                                    if (shorterWord.Length + 2 == longerWord.Length) //exactly two letters were removed
                                    {
                                        countPerLetterRemoved[letterToPlace - 'a']++;
                                        findAllTakeTwoClues.Add(
                                            new TakeTwoClue()
                                            {
                                                LongerWord = longerWord, 
                                                ShorterWord = shorterWord, 
                                                LetterRemoved = letterToPlace
                                            });
                                        Console.WriteLine($"Found a pair: {longerWord} and {shorterWord}.");
                                        //Console.ReadKey();
                                    }
                                }
                            }
                        }
                    }
                }
            }
            Console.WriteLine($"Found {findAllTakeTwoClues.Count} clues.");
            for (int i = 0; i < 26; i++)
            {
                int cluesForThisLetter = countPerLetterRemoved[i];
                if (cluesForThisLetter == 0) continue;
                Console.WriteLine($"{(char) (i+'a')} has {cluesForThisLetter} clue pairs.");
            }
            Console.ReadKey();
            return findAllTakeTwoClues;
        }

        /*
         * Take One clues
         * Found 2222 clues.
a has 56 clue pairs.
b has 52 clue pairs.
c has 61 clue pairs.
d has 212 clue pairs.
e has 103 clue pairs.
f has 40 clue pairs.
g has 47 clue pairs.
h has 52 clue pairs.
i has 35 clue pairs.
j has 5 clue pairs.
k has 40 clue pairs.
l has 111 clue pairs.
m has 49 clue pairs.
n has 77 clue pairs.
o has 36 clue pairs.
p has 74 clue pairs.
r has 160 clue pairs.
s has 749 clue pairs.
t has 102 clue pairs.
u has 30 clue pairs.
v has 15 clue pairs.
w has 39 clue pairs.
x has 4 clue pairs.
y has 68 clue pairs.
z has 5 clue pairs.

         */
        private static List<TakeTwoClue> FindAllTakeOneClues()
        {
            StringBuilder pattern = new StringBuilder();
            long wordsConsidered = 0;
            int[] countPerLetterRemoved = new int[26];
            for (int i = 0; i < 26; i++)
            {
                countPerLetterRemoved[i] = 0;
            }
            var findAllTakeTwoClues = new List<TakeTwoClue>();
            for (char letterToPlace = 'a'; letterToPlace <= 'z'; letterToPlace++)
            {
                for (int wordLength = 4; wordLength < 9; wordLength++)
                {
                    for (int firstLetterIndex = 0; firstLetterIndex < wordLength; firstLetterIndex++)
                    {
                        var placeLetterAtTheseIndicies = new List<int>() {firstLetterIndex};
                        pattern.Clear();
                        for (int patternIndex = 0; patternIndex < wordLength; patternIndex++)
                        {
                            char letterToAppend = '_';
                            if (placeLetterAtTheseIndicies.Contains(patternIndex))
                            {
                                letterToAppend = letterToPlace;
                            }

                            pattern.Append(letterToAppend);
                        }

                        string patternAsString = pattern.ToString();
                        foreach (var longerWord in WordRepository.WordsMatchingPattern(patternAsString))
                        {
                            wordsConsidered++;
                            if (wordsConsidered % 50 == 0)
                            {
                                Console.WriteLine($"Considered {wordsConsidered} words so far.");
                            }

                            string shorterWord = longerWord.Replace(letterToPlace.ToString(), "");
                            if (WordRepository.IsAWord(shorterWord))
                            {
                                if (shorterWord.Length + 1 == longerWord.Length) //exactly two letters were removed
                                {
                                    countPerLetterRemoved[letterToPlace - 'a']++;
                                    findAllTakeTwoClues.Add(
                                        new TakeTwoClue()
                                        {
                                            LongerWord = longerWord,
                                            ShorterWord = shorterWord,
                                            LetterRemoved = letterToPlace
                                        });
                                    Console.WriteLine($"Found a pair: {longerWord} and {shorterWord}.");
                                    //Console.ReadKey();
                                }
                            }
                        }
                    }
                }
            }
            Console.WriteLine($"Found {findAllTakeTwoClues.Count} clues.");
            for (int i = 0; i < 26; i++)
            {
                int cluesForThisLetter = countPerLetterRemoved[i];
                if (cluesForThisLetter == 0) continue;
                Console.WriteLine($"{(char)(i + 'a')} has {cluesForThisLetter} clue pairs.");
            }
            Console.ReadKey();
            return findAllTakeTwoClues;
        }

        private static void AddPuzzleToCollection(IPuzzle puzzleToAdd, PuzzleCollection collection, StringBuilder puzzleBuilder, StringBuilder solutionBuilder)
        {
            if (puzzleToAdd == null) return;
            collection.AddPuzzle(puzzleToAdd);
            PuzzleBuilder.Append(puzzleToAdd.FormatHtmlForGoogle(false, true));
            SolutionBuilder.Append(puzzleToAdd.FormatHtmlForGoogle(true, true));
        }

        private static PhraseSegmentPuzzle InteractiveGetQuotationPuzzleForDate(DateTime date)
        {
            Console.Clear();
            Console.WriteLine("Finding people born on this day.");
            BirthdayFinder finder = new BirthdayFinder();
            var results = finder.FindPeopleForDate(date.Month, date.Day);
            ClearConsoleInputAndOutput();
            foreach (var person in results)
            {
                ClearConsoleInputAndOutput();
                Console.WriteLine($"{person.Name} was born on {date.Month}/{date.Day}");
                var quotesCount = person.Quotes.Count;
                var maxQuotesToShow = 7;
                if (maxQuotesToShow < quotesCount)
                {
                    quotesCount = maxQuotesToShow;
                }
                for (var index = 0; index < quotesCount; index++)
                {
                    var quote = person.Quotes[index];
                    Console.WriteLine($"{index}: {quote}");
                }
                Console.WriteLine("Either enter the number of the quote to use, or press enter to go to the next person born this day (if there is one).");
                string userInputAsString = Console.ReadLine();
                int selectedIndex;
                if (int.TryParse(userInputAsString, out selectedIndex))
                {
                    PhraseSegmentPuzzle puzzle = new PhraseSegmentPuzzle()
                    {
                        Author = person.Name,
                        Phrase = person.Quotes[selectedIndex],
                    };
                    puzzle.PlacePhrase();
                    
                    return puzzle;

                }
            }

            return null;
        }

        private static void InteractiveDeletePuzzles(PuzzleCollection collection)
        {
            int selectedIndex = 0;
            while (selectedIndex != int.MinValue)
            {
                Console.Clear();
                Console.WriteLine("Select an index to delete, or just hit enter to skip this step.");
                for (var index = 0; index < collection.ToList().Count; index++)
                {
                    IPuzzle puzzle = collection.ToList()[index];
                    Console.WriteLine($"{index}: {puzzle.Description}");
                }

                string userInput = Console.ReadLine();
                if (int.TryParse(userInput, out selectedIndex))
                {
                    collection.RemovePuzzleAtIndex(selectedIndex);
                }
                else
                {
                    selectedIndex = int.MinValue;
                }
            }

        }

        private static void SavePuzzleCollection(PuzzleCollection collection)
        {
            if (collection.PuzzleCount == 0) return;
            if (string.IsNullOrWhiteSpace(collection.Name)) return;
            string directory = Path.Combine(BASE_DIRECTORY, "collections");
            collection.Serialize($@"{directory}\{collection.Name}.json");
        }

        private static PuzzleCollection InteractiveSelectPuzzleCollection()
        {
            Console.Clear();
            PuzzleCollection selectedCollection = new PuzzleCollection();
            string directory = Path.Combine(BASE_DIRECTORY, "collections");
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            var enumerateFiles = Directory.EnumerateFiles(directory, "*.json", SearchOption.TopDirectoryOnly).ToArray();
            Console.WriteLine("Please select one of the following options.");

            for (var i = 0; i < enumerateFiles.Length; i++)
            {
                string fileName = enumerateFiles[i];
                Console.WriteLine($"{i}:{fileName}");
            }

            Console.WriteLine("To create a new collection, enter a name.");
            string userInput = Console.ReadLine();
            int selectedIndex = 0;
            if (int.TryParse(userInput, out selectedIndex))
            {
                selectedCollection.Deserialize(enumerateFiles[selectedIndex]);
            }
            else
            {
                selectedCollection.Name = userInput;
            }
            return selectedCollection;
        }

        private static WordPuzzleType DisplayMenuOfAvailablePuzzles(string solution,
            Dictionary<WordPuzzleType, bool> availablePuzzleTypes)
        {
            WordPuzzleType userPuzzleSelection;

            ClearConsoleInputAndOutput();

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine($"Which type of puzzle would you like to create for '{solution.ToUpperInvariant()}'?");
            Console.WriteLine("0. None. Enter the next word or phrase.");
            Console.ForegroundColor = availablePuzzleTypes[WordPuzzleType.WordSquare]
                ? ConsoleColor.Gray
                : ERROR_CONSOLE_COLOR;
            Console.WriteLine("1. * Word Square");
            if (availablePuzzleTypes.ContainsKey(WordPuzzleType.Sudoku))
            {
                Console.ForegroundColor = availablePuzzleTypes[WordPuzzleType.Sudoku]
                    ? ConsoleColor.Gray
                    : ERROR_CONSOLE_COLOR;
                Console.WriteLine("2. Sudoku");
            }

            Console.ForegroundColor = availablePuzzleTypes[WordPuzzleType.Anacrostic]
                ? ConsoleColor.Gray
                : ERROR_CONSOLE_COLOR;
            Console.WriteLine("3. * Anacrostic");
            if (availablePuzzleTypes.ContainsKey(WordPuzzleType.WordLadder))
            {
                Console.ForegroundColor = availablePuzzleTypes[WordPuzzleType.WordLadder]
                    ? ConsoleColor.Gray
                    : ERROR_CONSOLE_COLOR;
                Console.WriteLine("4. Word Ladder");
            }

            Console.ForegroundColor =
                availablePuzzleTypes[WordPuzzleType.LettersAndArrows] ? ConsoleColor.Gray : ERROR_CONSOLE_COLOR;
            Console.WriteLine("5. * Letters and Arrows");
            Console.ForegroundColor =
                availablePuzzleTypes[WordPuzzleType.ReadDownColumn] ? ConsoleColor.Gray : ERROR_CONSOLE_COLOR;
            Console.WriteLine("6. * Read Down Column");
            Console.ForegroundColor =
                availablePuzzleTypes[WordPuzzleType.HiddenRelatedWords] ? ConsoleColor.Gray : ERROR_CONSOLE_COLOR;
            Console.WriteLine("7. * Hidden Related Words");
            if (availablePuzzleTypes.ContainsKey(WordPuzzleType.BuildingBlocks))
            {
                Console.ForegroundColor =
                    availablePuzzleTypes[WordPuzzleType.BuildingBlocks] ? ConsoleColor.Gray : ERROR_CONSOLE_COLOR;
                Console.WriteLine("8. Building Blocks");
            }

            if (availablePuzzleTypes.ContainsKey(WordPuzzleType.RelatedWords))
            {
                Console.ForegroundColor =
                    availablePuzzleTypes[WordPuzzleType.RelatedWords] ? ConsoleColor.Gray : ERROR_CONSOLE_COLOR;
                Console.WriteLine("9. Related Words");
            }

            if (availablePuzzleTypes.ContainsKey(WordPuzzleType.MissingLetters))
            {
                Console.ForegroundColor =
                    availablePuzzleTypes[WordPuzzleType.MissingLetters] ? ConsoleColor.Gray : ERROR_CONSOLE_COLOR;
                Console.WriteLine("Q. Missing Letters");
            }

            if (availablePuzzleTypes.ContainsKey(WordPuzzleType.PuzzleForDate))
            {
                Console.ForegroundColor =
                    availablePuzzleTypes[WordPuzzleType.PuzzleForDate] ? ConsoleColor.Gray : ERROR_CONSOLE_COLOR;
                Console.WriteLine("W. Puzzle For Date");
            }

            Console.ForegroundColor = ConsoleColor.Gray;

            var userPuzzleSelectionInput = Console.ReadKey();
            var userPuzzleSelectionString = userPuzzleSelectionInput.KeyChar.ToString();
            if (userPuzzleSelectionString == "q")
            {
                userPuzzleSelectionString = "10";
            }
            if (userPuzzleSelectionString == "w")
            {
                userPuzzleSelectionString = "11";
            }

            if (Enum.TryParse(userPuzzleSelectionString, out userPuzzleSelection))
            {
            }
            else
            {
                userPuzzleSelection = 0;
            }

            return userPuzzleSelection;
        }

        private static void ClearConsoleInputAndOutput()
        {
            Console.Clear();
            while (Console.KeyAvailable)
            {
                Console.ReadKey();
            }
        }

        private static void InterativelyCreatesWordsWithSelectedPattern()
        {
            string wordPattern = "test";
            while (!string.IsNullOrWhiteSpace(wordPattern))
            {
                Console.Clear();

                int counter = 0;
                foreach (string word in WordRepository.WordsMatchingPattern(wordPattern))
                {
                    if (counter++ % 5 == 0)
                    {
                        Console.WriteLine();
                    }

                    Console.Write(word);
                    Console.Write('\t');
                }

                Console.WriteLine();
                Console.WriteLine("Enter a pattern (use underscores for missing letters) or just hit enter to exit:");
                wordPattern = Console.ReadLine();
            }
        }

        private static IPuzzle InteractivelyGenerateSelectedPuzzleType(WordPuzzleType userPuzzleSelection,
            int solutionLength,
            string solution, List<string> solutionThemes)
        {
            IPuzzle generatedPuzzle = null;
            switch (userPuzzleSelection)
            {
                case WordPuzzleType.WordSquare:
                    if (3 < solutionLength && solutionLength < 7)
                    {
                        generatedPuzzle = InteractiveFindWordSquare(solution);
                        
                        PuzzleBuilder.Append(generatedPuzzle?.FormatHtmlForGoogle(false, true));
                        SolutionBuilder.Append(generatedPuzzle?.FormatHtmlForGoogle(true, true));
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine(
                            $"{solution} is not the right length for a Magic Word Square. Press anything to continue.");
                        Console.ReadKey();
                    }

                    break;

                case WordPuzzleType.Sudoku:
                    if (WordSudoku.ContainsDuplicateLetters(solution))
                    {
                        Console.Clear();
                        Console.WriteLine(
                            $"{solution} has duplicate letters, so cannot be used for Sudoku. Press anything to continue.");
                        Console.ReadKey();
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("Creating a word sudoku for you.");
                        InteractiveGenerateWordSudoku(solution);
                        Console.WriteLine("Done. Press a key to continue.");
                        Console.ReadKey();
                    }

                    break;
                case WordPuzzleType.Anacrostic:
                    if (7 < solutionLength && solutionLength < 57)
                    {
                        Console.Clear();
                        Console.WriteLine("Creating an anacrostic for you.");
                        generatedPuzzle = InteractiveGenerateAnacrostic(new AnacrosticParameterSet
                        {
                            Phrase = solution,
                            WordsToUse = new List<string>() { }
                        });
                        Console.WriteLine("Done. Press a key to continue.");
                        Console.ReadKey();
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine($"{solution} is not the right length for an anacrostic.");
                        Console.ReadKey();
                    }

                    break;
                case WordPuzzleType.WordLadder:
                    if (2 < solutionLength && solutionLength < 7)
                    {
                        Console.Clear();
                        Console.WriteLine("Creating a word ladder for you.");
                        InteractiveFindWordLadder(solution);
                        Console.WriteLine("Done. Press a key to continue.");
                        Console.ReadKey();
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine(
                            $"{solution} is not the right length for a Word Ladder. Press anything to continue.");
                        Console.ReadKey();
                    }

                    break;

                case WordPuzzleType.LettersAndArrows:
                    if (7 < solutionLength && solutionLength < 30)
                    {
                        Console.Clear();
                        Console.WriteLine("Creating a letters and arrows puzzle for you.");
                        generatedPuzzle = InteractiveFindLettersAndArrowsPuzzle(solution);
                        Console.WriteLine("Done. Press a key to continue.");
                        Console.ReadKey();
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine(
                            $"{solution} is not the right length for a Letters and Arrows puzzle. Press anything to continue.");
                        Console.ReadKey();
                    }

                    break;

                case WordPuzzleType.ReadDownColumn:
                    if (3 < solutionLength && solutionLength < 30)
                    {
                        Console.Clear();
                        Console.WriteLine("Creating a read down column puzzle for you.");
                        generatedPuzzle = InteractiveFindReadDownColumnPuzzle(solution);
                        Console.WriteLine("Done. Press a key to continue.");
                        Console.ReadKey();
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine(
                            $"{solution} is not the right length for a Read Down Column puzzle. Press anything to continue.");
                        Console.ReadKey();
                    }

                    break;
                case WordPuzzleType.HiddenRelatedWords:
                    if (!solution.ToLower().Contains('x'))
                    {
                        Console.Clear();
                        Console.WriteLine("Creating a Hidden Related Word puzzle for you.");
                        generatedPuzzle = InteractiveCreateHiddenRelatedWordPuzzle(solution);
                        Console.WriteLine("Done. Press a key to continue.");
                        Console.ReadKey();
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine(
                            $"{solution} contains the letter X, so I can't create a hidden word puzzle. Press anything to continue.");
                        Console.ReadKey();
                    }

                    break;
                case WordPuzzleType.BuildingBlocks:
                    if (!solution.Contains(' '))
                    {
                        Console.Clear();
                        Console.WriteLine("Creating a Building Blocks puzzle for you.");
                        InteractiveCreateBuildingBlocksPuzzle(solution);
                        Console.WriteLine("Done. Press a key to continue.");
                        Console.ReadKey();
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine(
                            $"{solution} contains a space, so I can't create a building blocks puzzle. Press anything to continue.");
                        Console.ReadKey();
                    }

                    break;

                case WordPuzzleType.RelatedWords:
                    if (0 < solutionThemes.Count)
                    {
                        Console.Clear();
                        Console.WriteLine("Creating a Related Words puzzle for you.");
                        InteractiveCreateRelatedWordsPuzzle(solution, solutionThemes);
                        Console.WriteLine("Done. Press a key to continue.");
                        Console.ReadKey();
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine(
                            $"{solution} doesn't have any themes, so I can't create a related words puzzle. Press anything to continue.");
                        Console.ReadKey();
                    }

                    break;

                case WordPuzzleType.MissingLetters:
                {
                    Console.Clear();
                    Console.WriteLine("Creating a Missing Letters puzzle for you.");
                    InteractiveCreateMissingLettersPuzzle(solution);
                    Console.WriteLine("Done. Press a key to continue.");
                    Console.ReadKey();
                }
                    break;
                case WordPuzzleType.PuzzleForDate:
                    generatedPuzzle = InteractiveGetQuotationPuzzleForDate(DateTime.Now);
                    break;
            }

            return generatedPuzzle;
        }

        private static void LoadSevenLetterWords()
        {
            StringBuilder rowsToAdd = new StringBuilder();
            foreach (string word in File.ReadAllLines(@"C:\Users\cbeauvai\source\repos\WordPuzzle3\WordPuzzlesTest\data\10LetterWords.txt"))
            {
                if (word.Length == 10)
                {
                    var category = WordRepository.CategorizeWord(word);
                    string newWordRow = $"{word}\t{category}\t{word.Length}";
                    rowsToAdd.AppendLine(newWordRow);
                    Console.WriteLine(newWordRow);
                }
            }
            Clipboard.SetText(rowsToAdd.ToString());
            Console.WriteLine("Results copied to clipboard. Press a key to continue.");
            Console.ReadKey();
        }

        private static void FindWordsThatMakeDigits()
        {
            Console.WriteLine("Words that fit into the pattern of the digit 0:");
            for (char repeatedLetter = 'a'; repeatedLetter <= 'z'; repeatedLetter++)
            {
                string pattern = $"{repeatedLetter}_____{repeatedLetter}";
                Console.WriteLine(pattern);
                foreach (string word in WordRepository.WordsMatchingPattern(pattern))
                {
                    Console.WriteLine(word);
                }
            }
            Console.WriteLine();


            Console.WriteLine("Words that fit into the pattern of the digit 1:");
            Console.WriteLine("Any three letter word.");
            Console.WriteLine();



            Console.WriteLine("Words that fit into the pattern of the digit 2:");
            Console.WriteLine("Any six letter word.");
            Console.WriteLine();

            Console.WriteLine("Words that fit into the pattern of the digit 3:");
            for (char repeatedLetter = 'a'; repeatedLetter <= 'z'; repeatedLetter++)
            {
                string pattern = $"__{repeatedLetter}_{repeatedLetter}__";
                foreach (string word in WordRepository.WordsMatchingPattern(pattern))
                {
                    Console.WriteLine(word);
                }
            }
            Console.WriteLine();

            Console.WriteLine("Words that fit into the pattern of the digit 4:");
            foreach (string sixLetterWord in WordRepository.WordsMatchingPattern("______"))
            {
                if (sixLetterWord[2] == sixLetterWord[4])
                {
                    Console.WriteLine(sixLetterWord);
                }
            }
            Console.WriteLine("none");
 

            Console.WriteLine("Words that fit into the pattern of the digit 5:");
            Console.WriteLine("Any six letter word.");
            Console.WriteLine();

            Console.WriteLine("Words that fit into the pattern of the digit 6:");
            for (char repeatedLetter = 'a'; repeatedLetter <= 'z'; repeatedLetter++)
            {
                string pattern = $"__{repeatedLetter}___{repeatedLetter}";
                foreach (string word in WordRepository.WordsMatchingPattern(pattern))
                {
                    Console.WriteLine(word);
                }
            }

            Console.WriteLine("Words that fit into the pattern of the digit 7:");
            Console.WriteLine("Any four letter word.");
            Console.WriteLine();

            Console.WriteLine("Words that fit into the pattern of the digit 8:");
            Console.WriteLine("Nine letters not supported yet.");

            Console.WriteLine("Words that fit into the pattern of the digit 9:");
            for (char repeatedLetter = 'a'; repeatedLetter <= 'z'; repeatedLetter++)
            {
                string pattern = $"___{repeatedLetter}_{repeatedLetter}_";
                foreach (string word in WordRepository.WordsMatchingPattern(pattern))
                {
                    Console.WriteLine(word);
                }
            }

            Console.ReadKey();
        }

        private static void InteractiveCreateMissingLettersPuzzle(string solution)
        {
            MissingLettersPuzzle puzzle = new MissingLettersPuzzle();
            puzzle.PlaceSolution(solution);
            char lastKeyPressed = 'z';
            while (lastKeyPressed != 'c')
            {
                string puzzleAsHtml = puzzle.FormatHtmlForGoogle();
                foreach (string word in puzzle.Words)
                {
                    Console.WriteLine(word);
                }
                Clipboard.SetData(DataFormats.Html, puzzleAsHtml);

                Console.WriteLine(
                    "Puzzle copied to clipboard. Press 'c' to continue, or anything else to copy it again.");
                lastKeyPressed = Console.ReadKey().KeyChar;
            }


        }

        private static void InteractiveCreateRelatedWordsPuzzle(string solution, List<string> solutionThemes)
        {
            string selectedTheme = solutionThemes[0];
            if (1 < solutionThemes.Count)
            {
                Console.WriteLine($"Select a theme for {solution.ToUpper()}");
                int counter = 0;
                foreach (string theme in solutionThemes)
                {
                    Console.WriteLine($"{counter++}. {theme}");
                }

                Console.WriteLine($"Or enter {counter} to exit.");

                string userInput = Console.ReadLine();
                int selectedIndex;
                if (!int.TryParse(userInput, out selectedIndex)) return;
                if (solutionThemes.Count <= selectedIndex) return;
                selectedTheme = solutionThemes[selectedIndex];
            }

            RelatedWordsPuzzle puzzle = new RelatedWordsPuzzle();
            puzzle.PlaceSolution(selectedTheme, solution);

            char lastKeyPressed = 'z';
            while (lastKeyPressed != 'c')
            {
                string puzzleAsHtml = puzzle.FormatHtmlForGoogle();
                foreach (string word in puzzle.Words)
                {
                    Console.WriteLine(word);
                }
                Clipboard.SetData(DataFormats.Html, puzzleAsHtml);

                Console.WriteLine(
                    "Puzzle copied to clipboard. Press 'c' to continue, or anything else to copy it again.");
                lastKeyPressed = Console.ReadKey().KeyChar;
            }

        }

        private static List<string> InteractiveFindThemesForWord(string word)
        {
            List<string> themes = new List<string>();

            Console.WriteLine($"Loading themes for {word} ");
            //Find existing themes
            themes.AddRange(WordRepository.FindThemesForWord(word));
            Console.WriteLine($"Found pre-existing themes: {string.Join(", ", themes )} ");



            //Find new ones.
            Console.WriteLine($"Finding new themes for {word} ");
            WordnikUtility utility = new WordnikUtility();
            bool readyToProceed = false;
            var potentialThemes = utility.FindPotentialThemes(word);
            while (!readyToProceed)
            {
                int counter = 0;
                if (potentialThemes.Count == 0)
                {
                    Console.WriteLine("No potential themes found.");
                    break;
                }
                foreach (var potentialTheme in potentialThemes)
                {
                    Console.WriteLine($"{counter++} {potentialTheme.Name} ({potentialTheme.Count} entries)");
                    readyToProceed = false;
                }

                Console.WriteLine("Which potential theme do you want to explore?");
                string userSelection = Console.ReadLine();
                int userSelectedIndex;
                StringBuilder rowsToCopy = new StringBuilder();
                if (int.TryParse(userSelection, out userSelectedIndex))
                {
                    var selectedTheme = potentialThemes[userSelectedIndex];
                    string selectedThemeName = selectedTheme.Name;
                    List<string> words = utility.FindWordsInList(selectedThemeName);
                    Console.WriteLine($"Words in {selectedThemeName}:");
                    foreach (string wordInNewTheme in words)
                    {
                        Console.WriteLine($"{wordInNewTheme}");
                        rowsToCopy.AppendLine($"{selectedThemeName}\t{wordInNewTheme}\t{wordInNewTheme.Length}");
                    }

                    Clipboard.SetText(rowsToCopy.ToString());
                    Console.WriteLine("New theme copied to clipboard. Paste into Google sheet.");
                    themes.Add(selectedThemeName);
                }
                else
                {
                    break;
                }
                Console.WriteLine("Press 'p' to proceed to the next step, or anything else to do this again.");
                var userKey = Console.ReadKey();
                if (userKey.Key == ConsoleKey.P)
                {
                    readyToProceed = true;
                }
            }

            return themes;
        }

        private static void InteractiveCreateBuildingBlocksPuzzle(string solution)
        {
            BuildingBlocksPuzzle puzzle = new BuildingBlocksPuzzle();
            puzzle.PlaceSolution(solution);

            char lastKeyPressed = 'z';
            while (lastKeyPressed != 'c')
            {
                string puzzleAsHtml = puzzle.FormatHtmlForGoogle();
                Console.WriteLine("Solution includes these words:");
                foreach (string word in puzzle.Words)
                {
                    Console.WriteLine(word);
                }
                Clipboard.SetData(DataFormats.Html, puzzleAsHtml);

                Console.WriteLine(
                    "Puzzle copied to clipboard. Press 'c' to continue, or anything else to copy it again.");
                lastKeyPressed = Console.ReadKey().KeyChar;
            }
        }


        private static void ListWordsThatCanBeShifted()
        {
            WordRepository repository = new WordRepository() {ExcludeAdvancedWords = false};
            StringBuilder completeList = new StringBuilder();
            foreach (string pattern in new[] {"___", "____", "_____", "______"})
            {
                foreach (string word in repository.WordsMatchingPattern(pattern))
                {
                    bool reachedShiftLimit = false;
                    for (int shift = 1; shift < 26; shift++)
                    {
                        StringBuilder shiftedWord = new StringBuilder();
                        foreach (char letter in word)
                        {
                            char shiftedLetter = (char) (letter + shift);
                            if ('z' < shiftedLetter)
                            {
                                reachedShiftLimit = true;
                                break;
                            }

                            shiftedWord.Append(shiftedLetter);
                        }

                        if (reachedShiftLimit) break;
                        if (repository.IsAWord(shiftedWord.ToString()))
                        {
                            Console.WriteLine($"{word}\t{shift}\t{shiftedWord}");
                            completeList.AppendLine($"{word}\t{shift}\t{shiftedWord}");
                        }
                    }
                }
            }

            Clipboard.SetText(completeList.ToString());
        }

        private static void ListWordsThatCanPrependALetter(string initialLetter)
        {
            WordRepository repositoryWithAdvancedWords = new WordRepository() {ExcludeAdvancedWords = false};
            for (int length = 3; length < 7; length++)
            {
                StringBuilder patternBuilder = new StringBuilder();
                patternBuilder.Append(initialLetter);
                patternBuilder.Append('_', length - 1);
                foreach (string word in repositoryWithAdvancedWords.WordsMatchingPattern(patternBuilder.ToString()))
                {
                    string subword = word.Substring(1);
                    if (repositoryWithAdvancedWords.IsAWord(subword))
                    {
                        Console.WriteLine(subword);
                    }
                }
            }
        }

        // ReSharper disable once UnusedMember.Local
        //TODO: Add "get words that match pattern to main menu.
        private static void GetWordsForReadDownColumnPuzzle()
        {
            int counter = 0;
            foreach (string word in WordRepository.WordsMatchingPattern("__o___"))
            {
                Console.WriteLine(word);
                if (counter++ % 20 == 0) Console.ReadKey();
            }
        }

        private static Dictionary<WordPuzzleType, bool> CalculateAvailablePuzzleTypes(string solution,
            List<string> solutionThemes)
        {
            int solutionLength = solution.Length;
            var availablePuzzleTypes = new Dictionary<WordPuzzleType, bool>();

            MissingLettersPuzzle puzzle = new MissingLettersPuzzle();

            availablePuzzleTypes.Add(WordPuzzleType.WordSquare, (3 < solutionLength && solutionLength < 7));
            availablePuzzleTypes.Add(WordPuzzleType.Sudoku, !WordSudoku.ContainsDuplicateLetters(solution));
            availablePuzzleTypes.Add(WordPuzzleType.Anacrostic, (7 < solutionLength && solutionLength < 57));
            availablePuzzleTypes.Add(WordPuzzleType.WordLadder, (2 < solutionLength && solutionLength < 7));
            availablePuzzleTypes.Add(WordPuzzleType.LettersAndArrows, (7 < solutionLength && solutionLength < 30));
            availablePuzzleTypes.Add(WordPuzzleType.ReadDownColumn, (3 < solutionLength && solutionLength < 30) && (!solution.Contains('h')));
            availablePuzzleTypes.Add(WordPuzzleType.HiddenRelatedWords, (!solution.ToLower().Contains('x')));
            availablePuzzleTypes.Add(WordPuzzleType.BuildingBlocks, (!solution.Contains(' ')));//TODO: Support phrases as well as single words.
            availablePuzzleTypes.Add(WordPuzzleType.RelatedWords, (0 < solutionThemes.Count));//Require that the word has at least one theme.
            availablePuzzleTypes.Add(WordPuzzleType.MissingLetters, ( 10 < puzzle.FindWordsContainingLetters(solution).Count));//There must be at least 10 words containing the solution as a substring.
            availablePuzzleTypes.Add(WordPuzzleType.PuzzleForDate, true);

            return availablePuzzleTypes;
        }

        private static Dictionary<WordPuzzleType, bool> CalculateAvailableIPuzzleTypes(string solution)
        {
            int solutionLength = solution.Length;
            var availablePuzzleTypes = new Dictionary<WordPuzzleType, bool>();


            availablePuzzleTypes.Add(WordPuzzleType.WordSquare, (3 < solutionLength && solutionLength < 7));
            //availablePuzzleTypes.Add(WordPuzzleType.Sudoku, !WordSudoku.ContainsDuplicateLetters(solution));
            availablePuzzleTypes.Add(WordPuzzleType.Anacrostic, (7 < solutionLength && solutionLength < 57));
            //availablePuzzleTypes.Add(WordPuzzleType.WordLadder, (2 < solutionLength && solutionLength < 7));
            availablePuzzleTypes.Add(WordPuzzleType.LettersAndArrows, (7 < solutionLength && solutionLength < 30));
            availablePuzzleTypes.Add(WordPuzzleType.ReadDownColumn, (3 < solutionLength && solutionLength < 30) && (!solution.Contains('h')));
            availablePuzzleTypes.Add(WordPuzzleType.HiddenRelatedWords, (!solution.ToLower().Contains('x')));
            //availablePuzzleTypes.Add(WordPuzzleType.BuildingBlocks, (!solution.Contains(' ')));//TODO: Support phrases as well as single words.
            //availablePuzzleTypes.Add(WordPuzzleType.RelatedWords, (0 < solutionThemes.Count));//Require that the word has at least one theme.
            //availablePuzzleTypes.Add(WordPuzzleType.MissingLetters, (10 < puzzle.FindWordsContainingLetters(solution).Count));//There must be at least 10 words containing the solution as a substring.
            //availablePuzzleTypes.Add(WordPuzzleType.PuzzleForDate, true);

            return availablePuzzleTypes;
        }

        private static void InteractiveCreateHiddenWordPuzzle(string solution)
        {
            HiddenWordPuzzle puzzle = new HiddenWordPuzzle() {Solution = solution};

            foreach (char letter in solution.ToLower())
            {
                bool foundSentence = false;
                while (!foundSentence)
                {
                    Console.Clear();
                    int blanksToAdd = RandomNumberGenerator.Next(2, 5);
                    if (letter == 'q') //must be at least 4 letters long.
                    {
                        blanksToAdd = RandomNumberGenerator.Next(3, 5);
                    }

                    //blanksToAdd = 5;//Let's just try hiding the longest words. 

                    StringBuilder patternBuilder = new StringBuilder();
                    patternBuilder.Append(letter);
                    patternBuilder.Append('_', blanksToAdd);
                    var pattern = patternBuilder.ToString();
                    var hiddenWordCandidates = WordRepository.WordsMatchingPattern(pattern);
                    if (0 == hiddenWordCandidates.Count)
                    {
                        throw new Exception($"No words found for pattern {pattern}");
                    }

                    string hiddenWordCandidate = hiddenWordCandidates[RandomNumberGenerator.Next(hiddenWordCandidates.Count)];
                    foreach (string splitableString in puzzle.GenerateAllSplitableStrings(hiddenWordCandidate))
                    {
                        List<string> phraseHidingWord =
                            puzzle.CreateSpecificExampleFromSplitableString(splitableString);
                        Console.WriteLine($"({splitableString}) : {string.Join(" ", phraseHidingWord)}      {hiddenWordCandidate.ToUpper()}.");
                    }
                    Console.WriteLine("Or just hit enter to create another one for this letter.");
                    string sentence = Console.ReadLine();
                    if (!string.IsNullOrEmpty(sentence))
                    {
                        foundSentence = true;
                        puzzle.Sentences.Add(sentence);
                    }

                }
            }
            Console.Clear();
            Console.WriteLine("Created hidden word puzzle!");
            char lastKeyPressed = 'z';
            while (lastKeyPressed != 'c')
            {
                string puzzleAsString = puzzle.FormatPuzzleAsText();
                Console.WriteLine(puzzleAsString);
                Clipboard.SetText(puzzleAsString);

                Console.WriteLine(
                    "Puzzle copied to clipboard. Press 'c' to continue, or anything else to copy it again.");
                lastKeyPressed = Console.ReadKey().KeyChar;
            }

        }

        private static HiddenRelatedWordsPuzzle InteractiveCreateHiddenRelatedWordPuzzle(string solution)
        {
            HiddenRelatedWordsPuzzle puzzle = new HiddenRelatedWordsPuzzle() {Solution = solution};
            HiddenWordPuzzle legacyPuzzle = new HiddenWordPuzzle() { Solution = solution };

            foreach (char letter in solution.ToLower())
            {
                bool foundSentence = false;
                while (!foundSentence)
                {
                    Console.Clear();
                    var hiddenWordCandidates = GetHiddenWordCandidates(letter);

                    string hiddenWordCandidate = hiddenWordCandidates[RandomNumberGenerator.Next(hiddenWordCandidates.Count)];
                    var splitableStrings = legacyPuzzle.GenerateAllSplitableStrings(hiddenWordCandidate);
                    if (splitableStrings.Count == 0)
                    {
                        continue;
                    }
                    foreach (string splitableString in splitableStrings)
                    {
                        List<string> phraseHidingWord =
                            legacyPuzzle.CreateSpecificExampleFromSplitableString(splitableString);
                        Console.WriteLine($"({splitableString}) : {string.Join(" ", phraseHidingWord)}      {hiddenWordCandidate.ToUpper()}.");
                    }
                    Console.WriteLine("Or just hit enter to create another one for this letter.");
                    string sentence = Console.ReadLine();
                    if (!string.IsNullOrEmpty(sentence))
                    {
                        if (!hiddenWordCandidate.Contains(letter))
                        {
                            Console.WriteLine($"{hiddenWordCandidate} does not contain {letter}! Try again.");
                        }
                        else
                        {
                            foundSentence = true;
                            legacyPuzzle.Sentences.Add(sentence);
                            int keyIndex = hiddenWordCandidate.IndexOf(letter);

                            puzzle.AddWord(
                                new HiddenWord()
                                {
                                    Word = hiddenWordCandidate,
                                    KeyIndex = keyIndex,
                                    SentenceHidingWord = sentence,
                                });
                        }
                    }

                }
            }
            Console.Clear();
            Console.WriteLine("Created hidden word puzzle!");
            char lastKeyPressed = 'z';
            while (lastKeyPressed != 'c')
            {
                string puzzleAsString = legacyPuzzle.FormatPuzzleAsText();
                Console.WriteLine(puzzleAsString);
                Clipboard.SetText(puzzleAsString);

                Console.WriteLine(
                    "Puzzle copied to clipboard. Press 'c' to continue, or anything else to copy it again.");
                lastKeyPressed = Console.ReadKey().KeyChar;
            }

            return puzzle;
        }

        private static List<string> GetHiddenWordCandidates(char letter)
        {
            int blanksToAdd = RandomNumberGenerator.Next(2, 5);
            if (letter == 'q') //must be at least 4 letters long.
            {
                blanksToAdd = RandomNumberGenerator.Next(3, 5);
            }

            //blanksToAdd = 5;//Let's just try hiding the longest words. 

            StringBuilder patternBuilder = new StringBuilder();
            patternBuilder.Append(letter);
            patternBuilder.Append('_', blanksToAdd);
            var pattern = patternBuilder.ToString();
            var hiddenWordCandidates = WordRepository.WordsMatchingPattern(pattern);
            int wordLength = RandomNumberGenerator.Next(4, 6);
            var randomIndex = RandomNumberGenerator.Next(0, wordLength - 1);
            hiddenWordCandidates = WordRepository.WordsWithCharacterAtIndex(letter, randomIndex, wordLength);
            if (0 == hiddenWordCandidates.Count)
            {
                throw new Exception($"No words found with {letter} in index {randomIndex} with length {wordLength}");
            }

            return hiddenWordCandidates;
        }

        private static void InteractiveHideTheseWords(List<string> wordsToHide)
        {
            wordsToHide.Shuffle();
            HiddenWordPuzzle puzzle = new HiddenWordPuzzle() {  };
            puzzle.Solution = "";
            foreach (string hiddenWordCandidate in wordsToHide)
            {
                bool foundSentence = false;
                while (!foundSentence)
                {

                    foreach (string splitableString in puzzle.GenerateAllSplitableStrings(hiddenWordCandidate))
                    {
                        List<string> phraseHidingWord =
                            puzzle.CreateSpecificExampleFromSplitableString(splitableString);
                        Console.WriteLine($"({splitableString}) : {string.Join(" ", phraseHidingWord)}      {hiddenWordCandidate.ToUpper()}.");
                    }
                    Console.WriteLine("Or just hit enter to create another one for this letter.");
                    string sentence = Console.ReadLine();
                    if (!string.IsNullOrEmpty(sentence))
                    {
                        foundSentence = true;
                        puzzle.Sentences.Add(sentence);
                    }

                }
            }
            Console.Clear();
            Console.WriteLine("Created hidden word puzzle!");
            char lastKeyPressed = 'z';
            while (lastKeyPressed != 'c')
            {
                string puzzleAsString = puzzle.FormatPuzzleAsText();
                Console.WriteLine(puzzleAsString);
                Clipboard.SetText(puzzleAsString);

                Console.WriteLine(
                    "Puzzle copied to clipboard. Press 'c' to continue, or anything else to copy it again.");
                lastKeyPressed = Console.ReadKey().KeyChar;
            }

        }

        private static IPuzzle InteractiveFindReadDownColumnPuzzle(string solution)
        {
            char specialCharacter;
            string reasonForSpecialCharacter;
            InteractiveGetSpecialCharacterAndReason(out specialCharacter, out reasonForSpecialCharacter);

            ReadDownColumnPuzzle puzzle = new ReadDownColumnPuzzle();
            puzzle.Solution = solution;
            if (char.IsLetter(specialCharacter) && !string.IsNullOrWhiteSpace(reasonForSpecialCharacter))
            {
                puzzle.SpecialCharacter = specialCharacter;
                puzzle.ReasonForSpecialCharacter = reasonForSpecialCharacter;
            }
            for (var index = 0; index < solution.Length; index++)
            {
                char letter = solution[index];
                if (!char.IsLetter(letter)) continue;
                Console.Clear();
                Console.WriteLine($"Pick (or enter) a word for the {index}-indexed letter in the solution:{letter.ToString().ToUpperInvariant()}");
                var list = puzzle.GetWordCandidatesForIndex(index);
                list.Shuffle();

                var numberOfOptionsToDisplay = list.Count;
                if (10 < numberOfOptionsToDisplay)
                {
                    numberOfOptionsToDisplay = 10;
                }
                for (var i = 0; i < numberOfOptionsToDisplay; i++)
                {
                    var word = list[i];
                    Console.WriteLine($"{i}: {word}");
                }

                var userInput = Console.ReadLine();
                int userSelectedIndex;
                string userSelectedWord; 
                if (int.TryParse(userInput, out userSelectedIndex))
                {
                    userSelectedWord = list[userSelectedIndex];
                }
                else
                {
                    userSelectedWord = userInput;
                }

                if (string.IsNullOrWhiteSpace(userSelectedWord))
                {
                    return null; //null. 
                }
                puzzle.SetWordAtIndex(userSelectedWord, index);
                string clue = InteractiveGetClueForWord(userSelectedWord);
                if (!string.IsNullOrWhiteSpace(clue))
                {
                    puzzle.SetClueAtIndex(clue, index);
                }
            }

            char lastKeyPressed = 'z';
            while (lastKeyPressed != 'c')
            {
                ClearConsoleInputAndOutput();
                for (var index = 0; index < puzzle.Words.Count; index++)
                {
                    string word = puzzle.Words[index];
                    string clue = puzzle.Clues[index];
                    Console.WriteLine($"{word.ToUpperInvariant()} ({clue})");
                }

//                Clipboard.SetData(DataFormats.Html, puzzle.FormatHtmlForGoogle());
                Console.WriteLine(
                    "Read Down Column puzzle has been copied to the clipboard. Press 'c' to continue, or anything else to copy it again.");
                lastKeyPressed = Console.ReadKey().KeyChar;
            }

            return puzzle;
        }

        private static void InteractiveGetSpecialCharacterAndReason(out char specialCharacter, out string reasonForSpecialCharacter)
        {
            reasonForSpecialCharacter = null;
            ClearConsoleInputAndOutput();
            Console.WriteLine("If there is a special character for this date, press the key of that character (otherwise, hit the space bar.)");
            var keyPressed = Console.ReadKey();
            specialCharacter = keyPressed.KeyChar;
            if (!char.IsLetter(specialCharacter)) return;
            Console.WriteLine("Ok, what is the reason this letter is special? (e.g. 'Because it is the second of May' the rest of the sentence will be added. ):");
            reasonForSpecialCharacter = Console.ReadLine();

        }


        private static IPuzzle InteractiveFindLettersAndArrowsPuzzle(string solution)
        {
            LettersAndArrowsPuzzle puzzle = null;
            string formatHtmlForGoogle = null;
            for (int size = 4; size < 7; size++)
            {
                try
                {
                    puzzle = new LettersAndArrowsPuzzle(solution, true, size);
                    InteractiveSetCluesForLettersAndArrowsPuzzle(puzzle);
                    formatHtmlForGoogle = puzzle.FormatHtmlForGoogle();
                    break;
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Unable to create a puzzle of size {size} ({e.Message}). Will increase size and try again. ");
                }
            }

            if (formatHtmlForGoogle == null) return null;
            char lastKeyPressed = 'z';
            while (lastKeyPressed != 'c')
            {
                Clipboard.SetData(DataFormats.Html, formatHtmlForGoogle);
                Console.WriteLine(
                    "Letters and Arrows puzzle has been copied to the clipboard. Press 'c' to continue, or anything else to copy it again.");
                lastKeyPressed = Console.ReadKey().KeyChar;
            }

            return puzzle;
        }

        private static void InteractiveSetCluesForLettersAndArrowsPuzzle(LettersAndArrowsPuzzle puzzle)
        {
            List<string> wordsInGrid = puzzle.GetWords();
            for (int i = 0; i < puzzle.Size; i++)
            {
                string currentWord = wordsInGrid[i];
                List<Clue> suggestedClues = ClueRepository.GetCluesForWord(currentWord);
                Console.Clear();
                if (0 < suggestedClues.Count)
                {
                    Console.WriteLine($"Input or select a clue for {currentWord}");
                    for (var index = 0; index < suggestedClues.Count; index++)
                    {
                        var clue = suggestedClues[index];
                        Console.WriteLine($"{index}: {clue.ClueText} ({clue.ClueSource})");
                    }

                }
                else
                {
                    Console.WriteLine($"Input a clue for {currentWord}");
                }

                int selectedIndex;
                string userInput = Console.ReadLine();
                string clueToUse;

                if (int.TryParse(userInput, out selectedIndex))
                {
                    clueToUse = suggestedClues[selectedIndex].ClueText;
                }
                else //Use all input as the clue
                {
                    clueToUse = userInput;
                    if (!string.IsNullOrWhiteSpace(clueToUse))
                    {
                        ClueRepository.AddClue(currentWord, clueToUse, ClueSource.ClueSourceChip);
                    }
                }
                puzzle.SetClueForRowIndex(i, clueToUse);
            }
        }

        private static void InteractiveFindWordLadder(string solution)
        {
            string suggestedWord = WordRepository.GetRandomWord();

            Console.WriteLine($"Enter a first word, or hit enter to use {suggestedWord}.");
            string initialWord = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(initialWord))
            {
                initialWord = suggestedWord;
            }

            Console.WriteLine($"Enter a clue for {initialWord}.");
            string initialClue = Console.ReadLine();
            if (initialClue == null)
            {
                return;
            }

            WordLadder ladder = new WordLadder(solution);
            ladder.AddToChain(initialWord, initialClue);

            char lastKeyPressed = 'c';
            while ('c' == lastKeyPressed)
            {
                int candidateIndex = 0;
                int wordsAddedSoFar = ladder.Chain.Count;
                List<string> nextWordCandidates = new List<string>();
                List<string> candidateNotes = new List<string>();

                for (int indexToReplace = 0; indexToReplace < initialWord.Length; indexToReplace++)
                {
                    var findNextWordsInChain = ladder.FindNextWordsInChain(ladder.Chain[wordsAddedSoFar - 1].Word,
                        indexToReplace);
                    foreach (string foundWord in findNextWordsInChain)
                    {
                        if (ladder.AlreadyContains(foundWord)) continue;
                        if (nextWordCandidates.Contains(foundWord)) continue;
                        if (ladder.AllLettersPlaced)
                        {
                            nextWordCandidates.Add(foundWord);
                            candidateNotes.Add("(all letters placed)");
                            continue;
                        }

                        bool foundWordContainsUnplacedLetter = false;
                        bool foundWordCanTakeUnplacedLetter = false;
                        foreach(char letter in ladder.RemainingUnplacedLetters)
                        {
                            if (foundWord.Contains(letter.ToString()))
                            {
                                foundWordContainsUnplacedLetter = true;
                                candidateNotes.Add($"(contains {letter.ToString().ToUpperInvariant()})");
                                break;
                            }

                            for (int index = 0; index < foundWord.Length; index++)
                            {
                                string futureWord = ReplaceCharacterAtIndex(foundWord, index, letter);
                                if (WordRepository.IsAWord(futureWord))
                                {
                                    foundWordCanTakeUnplacedLetter = true; //This word doesn't help place a missing letter, but the next word might.
                                    candidateNotes.Add($"--> {futureWord}");
                                }
                            }
                        }

                        if (foundWordContainsUnplacedLetter || foundWordCanTakeUnplacedLetter)
                        {
                            nextWordCandidates.Add(foundWord);
                            continue;
                        }

                    }

                    if (8 < nextWordCandidates.Count) break;
                }
                //nextWordCandidates.Shuffle();// only if we can shuffle the notes at the same time. 

                for (var index = 0; index < nextWordCandidates.Count; index++)
                {
                    string nextWordCandidate = nextWordCandidates[index];
                    Console.WriteLine($"{candidateIndex} {nextWordCandidate} {candidateNotes[index]}");
                    candidateIndex++;
                    if (8 < candidateIndex) break;
                }

                if (ladder.AllLettersPlaced)
                {
                    Console.WriteLine("All letters have been placed.");
                }
                else
                {
                    Console.WriteLine($"You still need to place letters: {ladder.RemainingUnplacedLetters}.");
                }
                Console.WriteLine($"Select the next word in the chain, or {candidateIndex} to exit.");
                char selectedWordIndexAsChar = Console.ReadKey().KeyChar;
                int selectedWordIndex;
                if (int.TryParse(selectedWordIndexAsChar.ToString(), out selectedWordIndex))
                {
                    if (selectedWordIndex < candidateIndex)
                    {
                        string selectedWord = nextWordCandidates[selectedWordIndex];
                        Console.WriteLine($"Enter a clue for {selectedWord.ToUpper()}.");
                        string clue = Console.ReadLine();
                        ladder.AddToChain(selectedWord, clue);
                    }
                    else
                    {
                        lastKeyPressed = 'z';//exit loop.
                    }
                }
                else
                {
                    Console.WriteLine("Unable to parse selected Word Index.");
                }
            }

            Console.WriteLine("Here's your ladder!");
            Console.WriteLine(ladder.DisplayChain());
            lastKeyPressed = 'z';
            while (lastKeyPressed != 'c')
            {

                Console.WriteLine(
                    "Ladder has been copied to the clipboard. Press 'c' to continue, or anything else to copy it again.");
                Clipboard.SetData(DataFormats.Html, ladder.FormatHtmlForGoogle());
                lastKeyPressed = Console.ReadKey().KeyChar;
            }
        }

        private static string ReplaceCharacterAtIndex(string word, int indexToReplace, char letter) //TODO: I feel like we have this method somewhere already.
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < word.Length; i++)
            {
                if (i == indexToReplace)
                {
                    builder.Append(letter);
                }
                else
                {
                    builder.Append(word[i]);
                }
            }

            return builder.ToString();
        }

        private static void InteractiveGenerateWordSudoku(string solution)
        {

            WordSudoku sudoku;

            try
            {
                sudoku = new WordSudoku(solution);
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Hit exception '{exception.Message}' trying to create a sudoku puzzle. Sorry.");
                return;
            }

            string formatForGoogle = sudoku.FormatForGoogle();

            Console.WriteLine(formatForGoogle);

            var userInput = new ConsoleKeyInfo();
            while (userInput.Key != ConsoleKey.C)
            {

                Console.WriteLine(
                    "Sudoku has been copied to the clipboard. Press 'c' to continue, or anything else to copy it again.");
                //Clipboard.SetText(formatForGoogle);
                Clipboard.SetData(DataFormats.Html, sudoku.FormatHtmlForGoogle());
                userInput = Console.ReadKey();
            }

        }

        private static IPuzzle InteractiveFindWordSquare(string relatedWord)
        {
            Console.Clear();
            Console.WriteLine("Creating a word square for you.");

            string fileWithMagicWordSquares = WordSquare.GetFileNameFor(relatedWord);
            if (!File.Exists(fileWithMagicWordSquares))
            {
                GenerateWordSquaresOfAnySize(relatedWord);
            }


            List<WordSquare> availableWordSquares = WordSquare.ReadAllWordSquaresFromFile(fileWithMagicWordSquares, relatedWord.Length);
            availableWordSquares.Sort((p, q) => History.CalculateScore(q) - History.CalculateScore(p));
            int availableSquareCount = availableWordSquares.Count;
            WordSquare selectedSquare = null;
            int squareIndex = 0;
            foreach (var availableWordSquare in availableWordSquares)
            {
                squareIndex++;
                Console.WriteLine($"0: accept this square. {squareIndex} / {availableSquareCount}");
                Console.WriteLine("Or enter 'z' to skip to the next word.");

                Console.WriteLine(availableWordSquare);
                Console.WriteLine($"Score: {History.CalculateScore(availableWordSquare)}");

                var userReaction = Console.ReadKey();
                if (userReaction.KeyChar == 'z')
                {
                    break;
                }
                if (userReaction.KeyChar == '0')
                {
                    selectedSquare = availableWordSquare;
                }

                if (selectedSquare != null)//populate clues
                {
                    string[] currentLines = selectedSquare.Lines;
                    for (int currentLineIndex = 0; currentLineIndex < selectedSquare.Size; currentLineIndex++)
                    {
                        string currentLine = currentLines[currentLineIndex];
                        selectedSquare.Clues[currentLineIndex] = InteractiveGetClueForWord(currentLine);
                    }

                    string squareFormattedForGoogle = selectedSquare.FormatForGoogle();
                    var userInput = new ConsoleKeyInfo();
                    while (userInput.Key != ConsoleKey.C)
                    {

                        Console.WriteLine(
                            "Word Square has been copied to the clipboard. Press 'c' to continue, or anything else to copy it again.");
                        Clipboard.SetText(squareFormattedForGoogle);
                        Clipboard.SetData(DataFormats.Html, selectedSquare.FormatHtmlForGoogle());
                        userInput = Console.ReadKey();
                    }

                    break;
                }
            }

            Console.WriteLine("Done. Press a key to continue.");
            Console.ReadKey();

            return selectedSquare;
        }

        private static void GenerateWordSquaresOfAnySize(string firstWordCandidate)
        {
            WordSquare square = new WordSquare(new string('_', firstWordCandidate.Length) );
            square.Repository = WordRepository;

            int[] wordsConsiderByLevel = { 0, 0, 0, 0, 0, 0, };

            if (!Directory.Exists(BASE_DIRECTORY + @"wordsquares\"))
            {
                Directory.CreateDirectory(BASE_DIRECTORY + @"wordsquares\");
            }


            {
                using (StreamWriter writer = new StreamWriter(
                    new FileStream(string.Format(BASE_DIRECTORY + $@"wordsquares\{firstWordCandidate}.txt"), FileMode.OpenOrCreate)))
                {
                    wordsConsiderByLevel[0]++;
                    WordSquare alpha = new WordSquare(square);
                    alpha.SetWordAtIndex(firstWordCandidate, 0);
                    FindNextLine(alpha, 1, writer, wordsConsiderByLevel);
                    writer.Flush();

                    Console.WriteLine();
                    Console.WriteLine(
                        $"Considered {wordsConsiderByLevel[0]} first words, {wordsConsiderByLevel[1]} second words, {wordsConsiderByLevel[2]} third words, {wordsConsiderByLevel[3]} fourth words, and {wordsConsiderByLevel[4]} fifth words",
                        "some number of");
                }

                //Console.ReadKey();
            }
        }

        private static void FindNextLine(WordSquare squareSoFar, int linesPopulatedSoFar, StreamWriter writer, int[] wordsConsideredSoFar)
        {
            if (2 <= linesPopulatedSoFar)
            {
                if (0 == squareSoFar.GetWordCandidates(squareSoFar.Size - 1).Count) //Check last line first
                {
                    //Console.WriteLine($"No words start with {squareSoFar.LastLine}, skipping");
                    return;
                }
            }
            if (4 == linesPopulatedSoFar)
            {
                Console.WriteLine("Calculating fifth (+) line ...");
            }

            if (1 < linesPopulatedSoFar)
            {
                Console.Write(linesPopulatedSoFar);
            }
            if ((squareSoFar.Size - 1) == linesPopulatedSoFar)
            {
                Console.WriteLine("Calculating last line for ");
                Console.WriteLine(squareSoFar);
            }

            foreach (string wordCandidate in squareSoFar.GetWordCandidates(linesPopulatedSoFar))
            {
                wordsConsideredSoFar[linesPopulatedSoFar]++;
                WordSquare epsilon = new WordSquare(squareSoFar);
                epsilon.SetWordAtIndex(wordCandidate, linesPopulatedSoFar);

                int nextLine = linesPopulatedSoFar + 1;
                if (nextLine < squareSoFar.Size)
                {
                    FindNextLine(epsilon, nextLine, writer, wordsConsideredSoFar);
                }
                else
                {
                    if (epsilon.IsLastLineAWord())
                    {
                        Console.WriteLine();
                        Console.WriteLine(epsilon.ToString());

                        writer.WriteLine();
                        writer.WriteLine(epsilon.ToString());
                    }
                }
            }
        }

        private static IPuzzle InteractiveGenerateAnacrostic(AnacrosticParameterSet parameterSet)
        {
            List<string> wordsAlreadyUsed = new List<string>();

            int selectedIndex;
            bool readyToProceed = false;
            Anacrostic anacrostic = null;

            while (!readyToProceed)
            {
                anacrostic = CreateAnacrosticFromPuzzleSet(parameterSet, wordsAlreadyUsed);

                Console.WriteLine(@"
To remove a word from the list, enter its number.
To add a new word, type it below. (use commas for multiples)
Press 0 to continue to the next step.");
                string commaDelimitedResponse = Console.ReadLine();
                if (commaDelimitedResponse == null)
                {
                    Console.WriteLine("I don't know what you mean.");
                    continue;
                }
                foreach (string response in commaDelimitedResponse.Split( new[] { ","}, StringSplitOptions.RemoveEmptyEntries))
                {
                    if (int.TryParse(response, out selectedIndex))
                    {
                        if (selectedIndex == 0)
                        {
                            readyToProceed = true;
                        }
                        else
                        {
                            selectedIndex = selectedIndex - 1;
                            if (selectedIndex < anacrostic.WordsFoundSoFar.Count)
                            {
                                string wordToRemove = anacrostic.WordsFoundSoFar[selectedIndex];
                                parameterSet.WordsToIgnore.Add(wordToRemove);
                                parameterSet.WordsToUse.Remove(wordToRemove);
                                readyToProceed = false;
                                parameterSet.Serialize();
                            }
                        }

                    }
                    else
                    {
                        parameterSet.WordsToUse.Add(response);
                        readyToProceed = false;
                    }
                }
            }
            //Second step - find anagrams of remaining letters
            readyToProceed = false;
            while (!readyToProceed)
            {
                anacrostic = CreateAnacrosticFromPuzzleSet(parameterSet, wordsAlreadyUsed);
                Console.WriteLine(@"Which word should we anagram with the remaining letters? 
Press 0 to continue to the next step.");
                string response = Console.ReadLine();

                if (int.TryParse(response, out selectedIndex))
                {
                    if (selectedIndex == 0)
                    {
                        readyToProceed = true;
                    }
                    else
                    {
                        selectedIndex = selectedIndex - 1;
                        if (selectedIndex < anacrostic.WordsFoundSoFar.Count)
                        {
                            string wordToAnagram = anacrostic.WordsFoundSoFar[selectedIndex];
                            string remainingLetters = anacrostic.RemainingLetters();
                            Console.WriteLine($"Generating anagrams using {wordToAnagram} + {remainingLetters} ");
                            var anagrams = AnagramFinder.FindAnagram(wordToAnagram + remainingLetters);
                            int anagramIndex = 1;
                            if (0 < anagrams.Count)
                            {
                                foreach (string anagramSuggestion in anagrams)
                                {
                                    Console.WriteLine($"{anagramIndex++}: {anagramSuggestion}");
                                    if (15 < anagramIndex) break;
                                }

                                Console.Write(@"Which anagram(s) should we use? 
Enter 0 for none.");
                                string responseForAnagramSelection = Console.ReadLine();
                                if (responseForAnagramSelection == null)
                                {
                                    responseForAnagramSelection = "0";
                                }
                                if (responseForAnagramSelection != "0")
                                {
                                    var selectedAnagramIndexAsStrings = responseForAnagramSelection.Split(
                                        new[]  {","},
                                        StringSplitOptions.RemoveEmptyEntries);

                                    foreach (string selectedAnagramIndexAsString in selectedAnagramIndexAsStrings)
                                    {
                                        int selectedAnagramIndex;
                                        if (int.TryParse(selectedAnagramIndexAsString, out selectedAnagramIndex))
                                        {
                                            if (parameterSet.WordsToUse.Contains(wordToAnagram))
                                            {
                                                parameterSet.WordsToUse.Remove(wordToAnagram);
                                            }
                                            parameterSet.WordsToUse.Add(anagrams[selectedAnagramIndex - 1]);
                                            parameterSet.Serialize();
                                        }
                                        else
                                        {
                                            Console.WriteLine(
                                                $"I didn't understand what you meant by '{selectedAnagramIndexAsString}'");
                                        }
                                    }
                                }
                            }
                            else
                            {
                                Console.Write(@"No anagrams found.");
                            }
                        }
                    }

                }
                else
                {
                    Console.WriteLine($"I don't know what you mean by {response} .");
                }
            }

            if (0 < anacrostic.RemainingLetters().Length)
            {
                Console.WriteLine("There are letters remaining. Press any key to exit.");
                Console.ReadKey();
                return null;
            }

            foreach (PuzzleWord puzzleWord in anacrostic.Puzzle.Clues)
            {
                puzzleWord.CustomizedClue = InteractiveGetClueForWord(puzzleWord);
            }

            string wordsFormattedForGoogleDocs = anacrostic.WordsFormattedForGoogleDocs();
            //Console.WriteLine(wordsFormattedForGoogleDocs); //was WordsWithNumberedBlanks()
            //Console.ReadKey();

            //Console.WriteLine(encodedPhraseForGoogle);
            ConsoleKeyInfo lastkeyPressed = new ConsoleKeyInfo();

            while (lastkeyPressed.KeyChar != 'c')
            {
                Console.WriteLine("Puzzle copied to clipboard. Press 'c' to continue or any other key to copy it again."); //todo loop
                Clipboard.SetData(DataFormats.Html, anacrostic.FormatHtmlForGoogle());

                lastkeyPressed = Console.ReadKey();
            }

            wordsAlreadyUsed.AddRange(anacrostic.WordsFoundSoFar);




            //Generate Html File
            HtmlGenerator generator = new HtmlGenerator
            {
                Puzzle = anacrostic.Puzzle,
                TwitterUrl = parameterSet.TwitterUrl
            };

            generator.GenerateHtmlFile(BASE_DIRECTORY + $@"anacrostics\puzzle_{parameterSet.TweetId}.html", false);
            return anacrostic;
        }

        private static string InteractiveGetClueForWord(string currentWord)
        {
            Console.Clear();
            Console.WriteLine(
                $"Enter customized clue for {currentWord.ToUpperInvariant()} or selected index from the following:");
            var clues = ClueRepository.GetCluesForWord(currentWord);
            for (var index = 0; index < clues.Count; index++)
            {
                var clue = clues[index];
                Console.WriteLine($"{index}: {clue.ClueText} ({clue.ClueSource})");
            }

            string userEnteredHint = Console.ReadLine();
            int selectedClueIndex;
            if (int.TryParse(userEnteredHint, out selectedClueIndex))
            {
                userEnteredHint = clues[selectedClueIndex].ClueText;
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(userEnteredHint))
                {
                    ClueRepository.AddClue(currentWord, userEnteredHint, ClueSource.ClueSourceChip);
                }
            }

            return userEnteredHint;
        }

        private static Anacrostic CreateAnacrosticFromPuzzleSet(AnacrosticParameterSet parameterSet, List<string> wordsAlreadyUsed)
        {
            Anacrostic anacrostic = new Anacrostic(parameterSet.Phrase);
            anacrostic.Repository = WordRepository;
            anacrostic.IgnoreWord("zeroes");
            anacrostic.IgnoreWord("zeros");
            foreach (string wordAlreadyUsed in wordsAlreadyUsed)
            {
                anacrostic.IgnoreWord(wordAlreadyUsed);
            }

            foreach (string wordToIgnore in parameterSet.WordsToIgnore)
            {
                anacrostic.IgnoreWord(wordToIgnore);
            }

            int index = 1;
            foreach (string predeterminedWord in parameterSet.WordsToUse)
            {
                Console.WriteLine($"* {index++}: " + predeterminedWord);
                anacrostic.RemoveWord(predeterminedWord);
            }

            StringBuilder listOfWords = new StringBuilder();
            string nextword = anacrostic.FindNextWord();
            while (nextword != null)
            {
                listOfWords.AppendLine($"{index++}: {nextword}");
                anacrostic.RemoveWord(nextword);
                nextword = anacrostic.FindNextWord();
            }

            listOfWords.AppendLine($"remaining:{anacrostic.RemainingLetters()}");

            Console.WriteLine(listOfWords.ToString());
            return anacrostic;
        }
    }

    internal enum ProgramMode
    {
        UNDEFINED = 0, 
        COLLECTION = 1, //Work with a collection of puzzles
        YEAR = 2, //Work with a year of puzzles
        PATTERN_MATCH = 3, //Just get words that match a given pattern

    }

    internal class TakeTwoClue
    {
        public string LongerWord;
        public string ShorterWord;
        public char LetterRemoved;
    }
}
