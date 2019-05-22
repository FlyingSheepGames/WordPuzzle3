using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WordPuzzles;

namespace WordPuzzleGenerator
{
    class Program
    {
        // ReSharper disable once InconsistentNaming
        private static readonly string BASE_DIRECTORY = ConfigurationManager.AppSettings["BaseDirectory"]; //@"E:\utilities\WordSquare\data\";
        static readonly WordRepository WordRepository = new WordRepository() {ExludeAdvancedWords = true};
        static readonly WordSquareHistory History = new WordSquareHistory(); //Todo: populate
        static readonly AnagramFinder AnagramFinder = new AnagramFinder() {Repository = WordRepository};
        static readonly Random RandomNumberGenerator = new Random();
        [STAThread]
        static void Main()
        {
            //TODO: Add this to the menu. Get words that match pattern.
            //GetWordsForReadDownColumnPuzzle();

            Console.WriteLine("Enter the word or phrase you'd like to create a puzzle for.");
            string solution = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(solution))
            {
                return;
            }
            int solutionLength = solution.Length;
            WordPuzzleType userPuzzleSelection = WordPuzzleType.WordSquare; //Doesn't matter what it is as long as it isn't 0.

            Dictionary<WordPuzzleType, bool> availablePuzzleTypes = CalculateAvailablePuzzleTypes(solution);

            while (userPuzzleSelection != 0)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine($"Which type of puzzle would you like to create for '{solution}'?");
                Console.WriteLine("0. None. Quit to word patterns.");
                Console.ForegroundColor = availablePuzzleTypes[WordPuzzleType.WordSquare] ? ConsoleColor.Gray : ConsoleColor.DarkMagenta;
                Console.WriteLine("1. Word Square");
                Console.ForegroundColor = availablePuzzleTypes[WordPuzzleType.Sudoku] ? ConsoleColor.Gray : ConsoleColor.DarkMagenta;
                Console.WriteLine("2. Sudoku");
                Console.ForegroundColor = availablePuzzleTypes[WordPuzzleType.Anacrostic] ? ConsoleColor.Gray : ConsoleColor.DarkMagenta;
                Console.WriteLine("3. Anacrostic");
                Console.ForegroundColor = availablePuzzleTypes[WordPuzzleType.WordLadder] ? ConsoleColor.Gray : ConsoleColor.DarkMagenta;
                Console.WriteLine("4. Word Ladder");
                Console.ForegroundColor = availablePuzzleTypes[WordPuzzleType.LettersAndArrows] ? ConsoleColor.Gray : ConsoleColor.DarkMagenta;
                Console.WriteLine("5. Letters and Arrows");
                Console.ForegroundColor = availablePuzzleTypes[WordPuzzleType.ReadDownColumn] ? ConsoleColor.Gray : ConsoleColor.DarkMagenta;
                Console.WriteLine("6. Read Down Column");
                Console.ForegroundColor = availablePuzzleTypes[WordPuzzleType.HiddenWords] ? ConsoleColor.Gray : ConsoleColor.DarkMagenta;
                Console.WriteLine("7. Hidden Words");

                var userPuzzleSelectionInput = Console.ReadKey();
                if (Enum.TryParse(userPuzzleSelectionInput.KeyChar.ToString(), out userPuzzleSelection))
                {

                }
                else
                {
                    userPuzzleSelection = 0;
                }

                switch (userPuzzleSelection)
                {

                    case WordPuzzleType.WordSquare:
                        if (3 < solutionLength && solutionLength < 7)
                        {
                            Console.Clear();
                            Console.WriteLine("Creating a word square for you.");
                            InteractiveFindWordSquare(solution);
                            Console.WriteLine("Done. Press a key to continue.");
                            Console.ReadKey();
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
                            InteractiveGenerateAnacrostic(new AnacrosticParameterSet
                            {
                                Phrase = solution,
                                WordsToUse = new List<string>() { "dr", "nor"}
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
                        if (3 < solutionLength && solutionLength < 30)
                        {
                            Console.Clear();
                            Console.WriteLine("Creating a letters and arrows puzzle for you.");
                            InteractiveFindLettersAndArrowsPuzzle(solution);
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
                            InteractiveFindReadDownColumnPuzzle(solution);
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
                    case WordPuzzleType.HiddenWords:
                        if (!solution.ToLower().Contains('x'))
                        {
                            Console.Clear();
                            Console.WriteLine("Creating a Hidden Word puzzle for you.");
                            InteractiveCreateHiddenWordPuzzle(solution);
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
                }
            }

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

        private static Dictionary<WordPuzzleType, bool> CalculateAvailablePuzzleTypes(string solution)
        {
            int solutionLength = solution.Length;
            var availablePuzzleTypes = new Dictionary<WordPuzzleType, bool>();

            availablePuzzleTypes.Add(WordPuzzleType.WordSquare, (3 < solutionLength && solutionLength < 7));
            availablePuzzleTypes.Add(WordPuzzleType.Sudoku, !WordSudoku.ContainsDuplicateLetters(solution));
            availablePuzzleTypes.Add(WordPuzzleType.Anacrostic, (7 < solutionLength && solutionLength < 57));
            availablePuzzleTypes.Add(WordPuzzleType.WordLadder, (2 < solutionLength && solutionLength < 7));
            availablePuzzleTypes.Add(WordPuzzleType.LettersAndArrows, (3 < solutionLength && solutionLength < 30));
            availablePuzzleTypes.Add(WordPuzzleType.ReadDownColumn, (3 < solutionLength && solutionLength < 30) && (!solution.Contains('h')));
            availablePuzzleTypes.Add(WordPuzzleType.HiddenWords, (!solution.ToLower().Contains('x')));

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
                    int blanksToAdd = RandomNumberGenerator.Next(2, 5);
                    if (letter == 'q') //must be at least 4 letters long.
                    {
                        blanksToAdd = RandomNumberGenerator.Next(3, 5);
                    }

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
                    var phrase = puzzle.HideWord(hiddenWordCandidate);
                    if (phrase.Count == 2)
                    {
                        Console.WriteLine(
                            $"Write a sentence that hides '{string.Join(",", phrase)}' and '{hiddenWordCandidate}'. Or just hit enter to create another one for this letter.");
                        string sentence = Console.ReadLine();
                        if (!string.IsNullOrEmpty(sentence))
                        {
                            foundSentence = true;
                            puzzle.Sentences.Add(sentence);
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Unable to hide word {hiddenWordCandidate}");
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

        private static void InteractiveFindReadDownColumnPuzzle(string solution)
        {
            ReadDownColumnPuzzle puzzle = new ReadDownColumnPuzzle();
            puzzle.Solution = solution;
            puzzle.PopulateWords();

            char lastKeyPressed = 'z';
            while (lastKeyPressed != 'c')
            {
                Clipboard.SetData(DataFormats.Html, puzzle.FormatHtmlForGoogle());
                Console.WriteLine(
                    "Read Down Column puzzle has been copied to the clipboard. Press 'c' to continue, or anything else to copy it again.");
                lastKeyPressed = Console.ReadKey().KeyChar;
            }
        }


        private static void InteractiveFindLettersAndArrowsPuzzle(string solution)
        {
            LettersAndArrowsPuzzle puzzle = new LettersAndArrowsPuzzle(solution, true);
            char lastKeyPressed = 'z';
            while (lastKeyPressed != 'c')
            {
                Clipboard.SetData(DataFormats.Html, puzzle.FormatHtmlForGoogle());
                Console.WriteLine(
                    "Letters and Arrows puzzle has been copied to the clipboard. Press 'c' to continue, or anything else to copy it again.");
                lastKeyPressed = Console.ReadKey().KeyChar;
            }

        }

        private static void InteractiveFindWordLadder(string solution)
        {
            Console.WriteLine($"Enter a clue for {solution.ToUpper()}, or press 'z' to skip.");
            string initialClue = Console.ReadLine();
            if (initialClue == null)
            {
                return;
            }
            if ("z" == initialClue.ToLower())
            {
                return;
            }
            WordLadder ladder = new WordLadder(solution, initialClue);

            char lastKeyPressed = 'c';
            while ('c' == lastKeyPressed)
            {
                int candidateIndex = 0;
                var indexToReplace = RandomNumberGenerator.Next(0, ladder.Size);
                int wordsAddedSoFar = ladder.Chain.Count;
                List<string> nextWordCandidates = new List<string>();

                for (int offset = 0; offset < ladder.Size; offset++)
                {
                    indexToReplace = (indexToReplace + offset) % ladder.Size;
                    var findNextWordsInChain = ladder.FindNextWordsInChain(ladder.Chain[wordsAddedSoFar - 1].Word,
                        indexToReplace);
                    foreach (string foundWord in findNextWordsInChain)
                    {
                        if (!ladder.AlreadyContains(foundWord))
                        {
                            nextWordCandidates.Add(foundWord);
                        }
                    }
                    if (8 < nextWordCandidates.Count) break;
                }

                foreach (string nextWordCandidate in nextWordCandidates)
                {
                    Console.WriteLine($"{candidateIndex} {nextWordCandidate}");
                    candidateIndex++;
                    if (8 < candidateIndex) break;
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
                        ladder.Chain.Add(new WordAndClue() {Word = selectedWord, Clue = clue});
                        Console.WriteLine("Press 'c' to add another word to the chain, or any other letter to wrap up. ");
                        lastKeyPressed = Console.ReadKey().KeyChar;
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
                Clipboard.SetText(formatForGoogle);
                Clipboard.SetData(DataFormats.Html, sudoku.FormatHtmlForGoogle());
                userInput = Console.ReadKey();
            }

        }

        private static void InteractiveFindWordSquare(string relatedWord)
        {
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
                Console.WriteLine($"Or enter 'z' to skip to the next word.");

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
                    Console.WriteLine();
                    string[] currentLines = selectedSquare.Lines;
                    for (int currentLineIndex = 0; currentLineIndex < selectedSquare.Size; currentLineIndex++)
                    {
                        string currentLine = currentLines[currentLineIndex];
                        string previousClue = WordRepository.FindClueFor(currentLine);

                        Console.WriteLine($"Enter a clue for {currentLine} (or enter 0 to choose another square) [{previousClue}]:");
                        string suggestedClue = Console.ReadLine();
                        if (suggestedClue == "0")
                        {
                            selectedSquare = null;
                            break;
                        }

                        if (!string.IsNullOrWhiteSpace(previousClue))
                        {
                            if (string.IsNullOrWhiteSpace(suggestedClue))
                            {
                                suggestedClue = previousClue;
                            }
                        }
                        else
                        {
                            var userInput = new ConsoleKeyInfo();
                            while (userInput.Key != ConsoleKey.C)
                            {

                                Console.WriteLine(
                                    "New clue has been copied to the clipboard. Please paste it into the words spreadsheet and replace the existing row for that word. Press 'c' to continue, or anything else to copy it again.");
                                Clipboard.SetText($"{currentLine}\t{currentLine.Length}\t{suggestedClue}");
                                userInput = Console.ReadKey();
                            }

                            WordRepository.AddClue(currentLine, suggestedClue);
                            WordRepository.SaveClues();
                        }
                        selectedSquare.Clues[currentLineIndex] = suggestedClue;
                    }

                    if (selectedSquare != null)
                    {
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
            }
        }

        private static void GenerateWordSquaresOfAnySize(string firstWordCandidate)
        {
            WordSquare square = new WordSquare(new string('_', firstWordCandidate.Length) );
            square.Repository = WordRepository;

            int[] wordsConsiderByLevel = { 0, 0, 0, 0, 0, 0, };

            if (!Directory.Exists(BASE_DIRECTORY + $@"wordsquares\"))
            {
                Directory.CreateDirectory(BASE_DIRECTORY + $@"wordsquares\");
            }


            {
                using (StreamWriter writer = new StreamWriter(
                    new FileStream(string.Format(BASE_DIRECTORY + $@"wordsquares\{firstWordCandidate}.txt"), FileMode.OpenOrCreate)))
                {
                    wordsConsiderByLevel[0]++;
                    WordSquare alpha = new WordSquare(square);
                    alpha.SetFirstLine(firstWordCandidate);
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

        private static void InteractiveGenerateAnacrostic(AnacrosticParameterSet parameterSet)
        {
            List<string> wordsAlreadyUsed = new List<string>();
            bool boolAddedAtLeastOneClue = false;

            int selectedIndex;
            bool readyToProceed = false;
            Anacrostic anacrostic = null;

            while (!readyToProceed)
            {
                anacrostic = CreateAnacrosticFromPuzzleSet(parameterSet, wordsAlreadyUsed);

                Console.WriteLine(@"Which words should we remove? 
Press 0 to continue to the next step.");
                string commaDelimitedResponse = Console.ReadLine();
                if (commaDelimitedResponse == null)
                {
                    Console.WriteLine($"I don't know what you mean.");
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
                        Console.WriteLine($"I don't know what you mean by {response} .");
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
                return;
            }
            string wordsFormattedForGoogleDocs = anacrostic.WordsFormattedForGoogleDocs();
            Console.WriteLine(wordsFormattedForGoogleDocs); //was WordsWithNumberedBlanks()
            //Console.ReadKey();
            string encodedPhraseForGoogle = anacrostic.GetEncodedPhraseForGoogle();
            Console.WriteLine(encodedPhraseForGoogle);
            Console.WriteLine("Puzzle copied to clipboard. Press any key to continue");//todo loop
            Clipboard.SetData(DataFormats.Html, anacrostic.GetFormattedHtmlForGoogle());
            Console.ReadKey();

            wordsAlreadyUsed.AddRange(anacrostic.WordsFoundSoFar);


            foreach (PuzzleWord clue in anacrostic.Puzzle.Clues)
            {
                string clueAsString = clue;
                string previouslyUsedClue = WordRepository.FindClueFor(clueAsString);
                if (string.IsNullOrWhiteSpace(previouslyUsedClue))
                {
                    Console.WriteLine($"Enter customized clue for {clueAsString}:");
                    string userEnteredHint = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(userEnteredHint))
                    {
                        clue.CustomizedClue = userEnteredHint;
                        WordRepository.AddClue(clueAsString, userEnteredHint);
                        boolAddedAtLeastOneClue = true;
                    }
                }
                else
                {
                    clue.CustomizedClue = previouslyUsedClue;
                }
            }

            //Generate Html File
            HtmlGenerator generator = new HtmlGenerator
            {
                Puzzle = anacrostic.Puzzle,
                TwitterUrl = parameterSet.TwitterUrl
            };

            generator.GenerateHtmlFile(BASE_DIRECTORY + $@"anacrostics\puzzle_{parameterSet.TweetId}.html", false);

            if (boolAddedAtLeastOneClue)
            {
                WordRepository.SaveClues();
            }
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
}
