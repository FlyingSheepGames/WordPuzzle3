using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using WordPuzzles;
// ReSharper disable UnusedMember.Local
// A lot of methods are only called on an ad-hoc basis. 
//TODO: Clean this up and offer a menu of actions. 

namespace WeeklyThemeGenerator
{
    class Program
    {
        private static readonly WordRepository WordRepository = new WordRepository();
        // ReSharper disable once InconsistentNaming
        private static readonly string BASE_DIRECTORY = ConfigurationManager.AppSettings["BaseDirectory"]; //@"E:\utilities\WordSquare\data\";
        private static readonly WordSquareHistory History = new WordSquareHistory();

        // ReSharper disable once UnusedMember.Local
        private static readonly string[] Themes = {
            "alcohol", "anatomy",   "atari game", "artist", "astronomy", "automobile", "appliance",
            "animal", "animal noise", "animalZoo", "animalFarm", "pet", 
            "berry", "beverage",  "bird", "bodies_of_water", "breakfast_food", "buildingMaterial", "bug", 
            "candy", "chemical element", "Christmas", "clothing", "color", "country", "criminal", "crime", "cosmetic", 
            "dessert", "disney movie", "dog", "day", 
            "emotion",
            "family", "fantasy", "fish", "flower", "food", "fruit", "furniture", 
            "gem", "geographicFeature", "genre", "game", 
            "herb", "hobby", 
            "injury", 
            "job", 
            "medical",  "music", "music instrument", "musicGenre", "moodNegative", "moodPositive", "month", "metal", 
            "nut",
            "pasta", "place", "playground", "pokemon", "personDescription", "passage", 
            "religion", "rock", "royalty", "room", 

            "scientist", "soup", "sport", "spring", "state", "Summer", "supernatural creatures", 
            "tree", "Thanksgiving",
            "US city", "US President", "universitySubject", 
            "Valentine's Day", "vegetable", "venue", 
            "weather", 
            "zoo_animals",
        };
        //consider adding: "crime", "military rank",
        [STAThread]
        static void Main()
        {
            //CreateTweetsForMonth("April 2019");

            LoadWordSquareHistory();

            

            Console.WriteLine("Enter a theme.");
            string theme = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(theme))
            {
                // ReSharper disable once InconsistentNaming
                string DIRECTORY = $"{ConfigurationManager.AppSettings["BaseDirectory"]}PuzzlesSets\\";
                string fileName = DIRECTORY + $"{theme.Replace("#", "")}.xml";
                if (!Directory.Exists(DIRECTORY))
                {
                    Directory.CreateDirectory(DIRECTORY);
                }

                WeekOfPuzzles weekOfPuzzles = null;
                if (File.Exists(fileName))
                {
                    Console.WriteLine("A file exists with this theme. Press 'y' to load it.");
                    var userInput = Console.ReadKey();
                    if (userInput.KeyChar == 'y')
                    {
                        weekOfPuzzles = new WeekOfPuzzles();
                        weekOfPuzzles.Deserialize(fileName);
                        Console.Clear();
                        Console.WriteLine("Week of puzzles successfully loaded.");
                        if (weekOfPuzzles.MondayOfWeekPosted == DateTime.MinValue)
                        {
                            Console.WriteLine("Unable to determine the week these puzzles will be posted.");
                        }
                            else
                        {
                            Console.WriteLine($"These puzzles are scheduled to be posted on the week of {weekOfPuzzles.MondayOfWeekPosted}");
                            var daysAway = (weekOfPuzzles.MondayOfWeekPosted - DateTime.Now).Days;
                            if (0 <= daysAway)
                            {
                                Console.WriteLine($" which is {daysAway} days away.");
                            }
                            else
                            {
                                Console.WriteLine($" which happened {-daysAway} days ago.");
                            }
                        }
                        Console.WriteLine("Press any key to continue.");
                        Console.ReadKey();

                        Console.Clear();
                        InteractivelyRemoveUnwantedPuzzles(weekOfPuzzles);
                    }
                }

                if (weekOfPuzzles == null)
                {
                    weekOfPuzzles = new WeekOfPuzzles();
                }

                if (weekOfPuzzles.MondayOfWeekPosted == DateTime.MinValue)
                {
                    weekOfPuzzles.MondayOfWeekPosted = InteractiveGetMondayOfWeekPosted();
                    
                }

                bool tryAgain = true;

                while (tryAgain)
                {
                    Console.Clear();
                    if (weekOfPuzzles.WednesdayALittleAlliteration == null)
                    {
                        weekOfPuzzles.WednesdayALittleAlliteration = PromptForWednesdayPuzzle(theme);
                        if (weekOfPuzzles.WednesdayALittleAlliteration != null)
                        {
                            Console.WriteLine("Which of the words in the solution is the theme word?");
                            weekOfPuzzles.SelectedWords[2] = Console.ReadLine();
                        }
                    }
                    weekOfPuzzles = GetWeekOfPuzzles(theme, weekOfPuzzles);
                    weekOfPuzzles.Serialize(fileName);

                    Console.WriteLine("Week of puzzles has been selected.");
                    string weekOfWords = string.Join("\t", weekOfPuzzles.SelectedWords);
                    Console.WriteLine(weekOfWords);

                    ConsoleKeyInfo userInputTryAgain = new ConsoleKeyInfo();
                    while (userInputTryAgain.Key != ConsoleKey.C)
                    {
                        Clipboard.SetText(weekOfWords);
                        Console.WriteLine(
                            "Copied to clipboard. Press 'c' to continue, or anything else to copy it again.");
                        userInputTryAgain = Console.ReadKey();
                    }

                    Console.WriteLine(
                        "Copied to clipboard. Press 'r' to do this again, or anything else to continue;");
                    userInputTryAgain = Console.ReadKey();
                    if (userInputTryAgain.KeyChar == 'r')
                    {
                        continue;
                    }

                    tryAgain = false;
                }


                GenerateTweetsForSelectedPuzzles(weekOfPuzzles);
            }
        }

        private static ALittleAlliteration PromptForWednesdayPuzzle(string theme)
        {
            Console.Clear();
            Console.WriteLine("Press 'y' if Wednesday's clue is illustrated.");
            var key = Console.ReadKey();
            if (key.Key != ConsoleKey.Y)
            {
                return null;
            }
            Console.WriteLine("What's the solution?");
            string solution = Console.ReadLine();

            return new ALittleAlliteration()
            {
                Clue = "Illustrated by Sara Beauvais", 
                Solution = solution, 
                Theme = theme
            };
        }

        private static void CreateNewWordSpreadsheet()
        {
            StringBuilder rowsToAdd = new StringBuilder();
            int wordCount = 0;
            List<string> wordsAdded = new List<string>();
            foreach (string pattern in new[] { "___", "____", "_____", "______" })
            {
                foreach (string word in WordRepository.WordsMatchingPattern(pattern))
                {
                    var category = WordRepository.CategorizeWord(word);
                    if (category == WordCategory.NotAWord)
                    {
                        Console.WriteLine();
                        Console.WriteLine($"{word} is not a word.");
                        continue;
                    }

                    if (wordsAdded.Contains(word.ToLower()))
                    {
                        continue;
                    }

                    wordsAdded.Add(word.ToLower());
                    Console.Write(".");

                    string hint = WordRepository.FindClueFor(word);
                    string databaseRow = string.Join("\t", word, category.ToString(), word.Length.ToString(), hint);
                    rowsToAdd.AppendLine(databaseRow);
                    wordCount++;
                }
            }
            char lastKeyPressed = ' ';
            while (lastKeyPressed != 'c')
            {
                Console.WriteLine($"Copied {wordCount} words to the clipboard. Press 'c' to continue, anything else to copy again.");
                Clipboard.SetText(rowsToAdd.ToString());
                var key = Console.ReadKey();
                lastKeyPressed = key.KeyChar;
            }


        }

        private static void CreateWordDoughnuts()
        {
            Console.WriteLine("Enter the first (3) letters for searching for word doughnuts.");
            string firstThreeLetters = Console.ReadLine();

            StringBuilder patternBuilder = new StringBuilder(firstThreeLetters);
            while (patternBuilder.Length < 6)
            {
                patternBuilder.Append('_');
            }

            foreach (string sixLetterWord in WordRepository.WordsMatchingPattern(patternBuilder.ToString()))
            {
                CreateLargeWordHexagon(sixLetterWord);
            }
        }

        private static void CreateWordHexagon(string initialWord)
        {
            List<WordHexagon> completedHexagons = new List<WordHexagon>();

            WordHexagon stage0Hexagon = new WordHexagon() {Verbose = false};
            if (!stage0Hexagon.SetHorizontalLineAtIndex(2, initialWord))
            {
                Console.WriteLine("Could not create stage 0.");
                return;
            }

            //Placed so far
            //    _ _ _
            //   _ _ _ _
            //  * * * * * 
            //   _ _ _ _
            //    _ _ _ 
            foreach (string diagonalFiveCandidate in stage0Hexagon.FindDiagonalLineAtIndex(2))
            {
                WordHexagon stage1Hexagon = new WordHexagon(stage0Hexagon);
                if (!stage1Hexagon.SetDiagonalLineAtIndex(2, diagonalFiveCandidate))
                {
                    //Console.WriteLine($"Could not place {diagonalFiveCandidate} in hexagon.");
                    continue;
                }
                //Placed so far
                //    * _ _
                //   _ * _ _
                //  * * * * * 
                //   _ _ * _
                //    _ _ * 
                foreach (string diagonalFourCandidate in stage1Hexagon.FindDiagonalLineAtIndex(1))
                {
                    WordHexagon stage2Hexagon = new WordHexagon(stage1Hexagon);
                    if (!stage2Hexagon.SetDiagonalLineAtIndex(1, diagonalFourCandidate))
                    {
                        //Console.WriteLine($"Could not place {diagonalFourCandidate} in hexagon.");
                        continue;
                    }
                    //Placed so far
                    //    * _ _
                    //   * * _ _
                    //  * * * * * 
                    //   _ * * _
                    //    _ * * 

                    //Now let's just finish it off horizontally.
                    foreach (string horizontalThreeTopCandidate in stage2Hexagon.FindHorizontalLineAtIndex(0))
                    {
                        WordHexagon stage3Hexagon = new WordHexagon(stage2Hexagon);
                        if (!stage3Hexagon.SetHorizontalLineAtIndex(0, horizontalThreeTopCandidate))
                        {
                            //Console.WriteLine($"Could not place {horizontalThreeTopCandidate} in hexagon.");
                            continue;
                        }
                        foreach (string horizontalFourTopCandidate in stage3Hexagon.FindHorizontalLineAtIndex(1))
                        {
                            WordHexagon stage4Hexagon = new WordHexagon(stage3Hexagon);
                            if (!stage4Hexagon.SetHorizontalLineAtIndex(1, horizontalFourTopCandidate))
                            {
                                //Console.WriteLine($"Could not place {horizontalFourTopCandidate} in hexagon.");
                                continue;
                            }
                            foreach (string horizontalFourBottomCandidate in stage4Hexagon.FindHorizontalLineAtIndex(3))
                            {
                                WordHexagon stage5Hexagon = new WordHexagon(stage4Hexagon);
                                if (!stage5Hexagon.SetHorizontalLineAtIndex(3, horizontalFourBottomCandidate))
                                {
                                    //Console.WriteLine($"Could not place {horizontalFourBottomCandidate} in hexagon.");
                                    continue;
                                }
                                foreach (string horizontalThreeBottomCandidate in stage5Hexagon.FindHorizontalLineAtIndex(4))
                                {
                                    WordHexagon stage6Hexagon = new WordHexagon(stage5Hexagon);
                                    if (!stage6Hexagon.SetHorizontalLineAtIndex(4, horizontalThreeBottomCandidate))
                                    {
                                        //Console.WriteLine($"Could not place {horizontalThreeBottomCandidate} in hexagon.");
                                        Console.Write(".");
                                        continue;
                                    }
                                    completedHexagons.Add(stage6Hexagon);
                                    Console.WriteLine();
                                    Console.WriteLine(stage6Hexagon);
                                    Console.WriteLine();
                                    //Console.ReadKey();
                                }
                            }

                        }
                    }
                }
            }

            XmlSerializer serializer = new XmlSerializer(typeof(List<WordHexagon>));
            string fileName = $"{BASE_DIRECTORY}WordHexagons\\{initialWord}.xml";
            if (!Directory.Exists($"{BASE_DIRECTORY}WordHexagons"))
            {
                Directory.CreateDirectory($"{BASE_DIRECTORY}WordHexagons");
            }
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
            using (TextWriter writer = new StreamWriter(fileName, true))
            {
                serializer.Serialize(writer, completedHexagons);
            }
        }

        private static void CreateLargeWordHexagon(string initialWord)
        {
            List<WordHexagon> completedHexagons = new List<WordHexagon>();

            WordHexagon stage0Hexagon = new WordHexagon(4) { Verbose = false };
            if (!stage0Hexagon.SetHorizontalLineAtIndex(2, initialWord))
            {
                Console.WriteLine("Could not create stage 0.");
                return;
            }

            //Placed so far
            //    _ _ _ _
            //   _ _ _ _ _
            //  * * * * * *
            // _ _ _ * _ _ _
            //  _ _ _ _ _ _ 
            //   _ _ _ _ _
            //    _ _ _ _
            bool foundFirstStage1 = false;
            bool foundFirstStage2 = false;
            bool foundFirstStage3 = false;
            bool foundFirstStage4 = false;
            bool foundFirstStage5 = false;
            bool foundFirstStage6 = false;
            bool foundFirstStage7 = false;
            bool foundFirstStage8 = false;

            var diagonalLineCandidates = stage0Hexagon.FindDiagonalLineAtIndex(5);
            Console.WriteLine($"Considering {diagonalLineCandidates.Count} candidates for first diagonal line.");
            foreach (string diagonalFiveCandidate in diagonalLineCandidates)
            {
                WordHexagon stage1Hexagon = new WordHexagon(stage0Hexagon);
                if (!foundFirstStage1)
                {
                    Console.WriteLine(stage1Hexagon);
                    foundFirstStage1 = true;
                }
                if (!stage1Hexagon.SetDiagonalLineAtIndex(5, diagonalFiveCandidate))
                {
                    Console.WriteLine($"Could not place {diagonalFiveCandidate} in hexagon.");
                    continue;
                }

                //Placed so far
                //    _ * _ _
                //   _ _ * _ _
                //  * * * * * *
                // _ _ _ * * _ _
                //  _ _ _ _ * _ 
                //   _ _ _ _ *
                //    _ _ _ _
                foreach (string diagonalFourCandidate in stage1Hexagon.FindDiagonalLineAtIndex(2))
                {
                    WordHexagon stage2Hexagon = new WordHexagon(stage1Hexagon);
                    if (!stage2Hexagon.SetDiagonalLineAtIndex(2, diagonalFourCandidate))
                    {
                        //Console.WriteLine($"Could not place {diagonalFourCandidate} in hexagon.");
                        continue;
                    }
                    if (!foundFirstStage2)
                    {
                        Console.WriteLine(stage2Hexagon);
                        foundFirstStage2 = true;
                    }
                    //Placed so far
                    //    _ * _ _
                    //   * _ * _ _
                    //  * * * * * *
                    // _ _ * * * _ _
                    //  _ _ * _ * _ 
                    //   _ _ * _ *
                    //    _ _ * _


                    //Now let's just finish it off horizontally.
                    foreach (string horizontalThreeTopCandidate in stage2Hexagon.FindHorizontalLineAtIndex(0))
                    {
                        WordHexagon stage3Hexagon = new WordHexagon(stage2Hexagon);
                        if (!stage3Hexagon.SetHorizontalLineAtIndex(0, horizontalThreeTopCandidate))
                        {
                            //Console.WriteLine($"Could not place {horizontalThreeTopCandidate} in hexagon.");
                            continue;
                        }
                        if (!foundFirstStage3)
                        {
                            Console.WriteLine(stage3Hexagon);
                            foundFirstStage3 = true;
                        }
                        foreach (string horizontalFourTopCandidate in stage3Hexagon.FindHorizontalLineAtIndex(1))
                        {
                            WordHexagon stage4Hexagon = new WordHexagon(stage3Hexagon);
                            if (!stage4Hexagon.SetHorizontalLineAtIndex(1, horizontalFourTopCandidate))
                            {
                                //Console.WriteLine($"Could not place {horizontalFourTopCandidate} in hexagon.");
                                continue;
                            }
                            if (!foundFirstStage4)
                            {
                                Console.WriteLine(stage4Hexagon);
                                foundFirstStage4 = true;
                            }
                            foreach (string horizontalFourBottomCandidate in stage4Hexagon.FindHorizontalLineAtIndex(3))
                            {
                                WordHexagon stage5Hexagon = new WordHexagon(stage4Hexagon);
                                if (!stage5Hexagon.SetHorizontalLineAtIndex(3, horizontalFourBottomCandidate))
                                {
                                    //Console.WriteLine($"Could not place {horizontalFourBottomCandidate} in hexagon.");
                                    continue;
                                }
                                if (!foundFirstStage5)
                                {
                                    Console.WriteLine(stage5Hexagon);
                                    foundFirstStage5 = true;
                                }
                                foreach (string horizontalThreeBottomCandidate in stage5Hexagon.FindHorizontalLineAtIndex(4))
                                {
                                    WordHexagon stage6Hexagon = new WordHexagon(stage5Hexagon);
                                    if (!stage6Hexagon.SetHorizontalLineAtIndex(4, horizontalThreeBottomCandidate))
                                    {
                                        //Console.WriteLine($"Could not place {horizontalThreeBottomCandidate} in hexagon.");
                                        //Console.Write(".");
                                        continue;
                                    }
                                    if (!foundFirstStage6)
                                    {
                                        Console.WriteLine(stage6Hexagon);
                                        foundFirstStage6 = true;
                                    }

                                    foreach (string horizontalFourthLineCandidate in stage6Hexagon
                                        .FindHorizontalLineAtIndex(5))
                                    {
                                        WordHexagon stage7Hexagon = new WordHexagon(stage6Hexagon);
                                        if (!stage7Hexagon.SetHorizontalLineAtIndex(5, horizontalFourthLineCandidate))
                                        {
                                            //Console.WriteLine($"Could not place {horizontalThreeBottomCandidate} in hexagon.");
                                            //Console.Write(".");
                                            continue;
                                        }
                                        if (!foundFirstStage7)
                                        {
                                            Console.WriteLine(stage7Hexagon);
                                            foundFirstStage7 = true;
                                        }

                                        foreach (string horizontalFifthLineCandidate in stage7Hexagon
                                            .FindHorizontalLineAtIndex(6))
                                        {
                                            WordHexagon stage8Hexagon = new WordHexagon(stage7Hexagon);
                                            if (!stage8Hexagon.SetHorizontalLineAtIndex(6,
                                                horizontalFifthLineCandidate))
                                            {
                                                //Console.WriteLine($"Could not place {horizontalThreeBottomCandidate} in hexagon.");
                                                Console.Write("*");
                                                continue;
                                            }
                                            if (!foundFirstStage8)
                                            {
                                                Console.WriteLine(stage8Hexagon);
                                                foundFirstStage8 = true;
                                            }

                                            foreach (string horizontalSixthLineCandidate in stage8Hexagon
                                                .FindHorizontalLineAtIndex(7))
                                            {
                                                WordHexagon stage9Hexagon = new WordHexagon(stage8Hexagon);
                                                if (!stage9Hexagon.SetHorizontalLineAtIndex(7,
                                                    horizontalSixthLineCandidate))
                                                {
                                                    //Console.WriteLine($"Could not place {horizontalThreeBottomCandidate} in hexagon.");
                                                    Console.Write(".");
                                                    continue;
                                                }

                                                completedHexagons.Add(stage9Hexagon);
                                                Console.WriteLine();
                                                Console.WriteLine(stage9Hexagon);
                                                Console.WriteLine();
                                                //Console.ReadKey();
                                            }
                                        }
                                    }
                                }
                            }

                        }
                    }
                }
            }

            if (0 < completedHexagons.Count)
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<WordHexagon>));
                const string FOLDER_NAME = "WordDoughnuts";
                string fileName = $"{BASE_DIRECTORY}{FOLDER_NAME}\\{initialWord}.xml";
                if (!Directory.Exists($"{BASE_DIRECTORY}{FOLDER_NAME}"))
                {
                    Directory.CreateDirectory($"{BASE_DIRECTORY}{FOLDER_NAME}");
                }

                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }

                using (TextWriter writer = new StreamWriter(fileName, true))
                {
                    serializer.Serialize(writer, completedHexagons);
                }
            }
        }

        private static void LoadWordSquareHistory()
        {
            Console.WriteLine($"Reading files in { BASE_DIRECTORY + @"\PuzzlesSets" }");
            foreach (string file in Directory.EnumerateFiles(BASE_DIRECTORY + @"\PuzzlesSets", "*.xml"))
            {
                WeekOfPuzzles weekOfPuzzles = new WeekOfPuzzles();
                weekOfPuzzles.Deserialize(file);
                //Console.WriteLine($"Deserializing file {file}");
                if (weekOfPuzzles.MondayOfWeekPosted != DateTime.MinValue)
                {
                    //Console.WriteLine($"for date {weekOfPuzzles.MondayOfWeekPosted}");
                    if (weekOfPuzzles.MondayWordSquare != null)
                    {
                        //Console.WriteLine($"adding puzzle {weekOfPuzzles.MondayWordSquare}");
                        History.AddWordSquare(weekOfPuzzles.MondayWordSquare, weekOfPuzzles.MondayOfWeekPosted);
                    }
                    else
                    {
                        //Console.WriteLine("But the Monday word puzzle was null.");
                    }
                }
                else
                {
                    //Console.WriteLine("But the posting date was not specified.");
                }
            }
        }

        private static DateTime InteractiveGetMondayOfWeekPosted()
        {
            DateTime closestFutureMonday = DateTime.Now;
            while (closestFutureMonday.DayOfWeek != DayOfWeek.Monday)
            {
                closestFutureMonday = closestFutureMonday.AddDays(1);
            }
            var oneWeekLater = closestFutureMonday.AddDays(7);
            Console.WriteLine("When should these puzzles be posted?");
            Console.WriteLine($"A: Next Monday {closestFutureMonday}");
            Console.WriteLine($"B: A week later {oneWeekLater}");
            Console.WriteLine($"C: Enter my own time");
            Console.WriteLine($"D: Ask me later.");

            var key = Console.ReadKey();
            switch (key.KeyChar.ToString().ToLower())
            {
                case "a":
                    return closestFutureMonday;
                case "b":
                    return oneWeekLater;
                case "d":
                    return DateTime.MinValue;
            }

            var userInputDate = DateTime.MinValue;
            while (userInputDate == DateTime.MinValue)
            {
                Console.WriteLine("Enter a date please.");
                var userEnteredString = Console.ReadLine();
                if (DateTime.TryParse(userEnteredString, out userInputDate))
                {
                    return userInputDate;
                }
                else
                {
                    Console.WriteLine("Unable to interpret input. Try again, please.");
                }


            }

            return DateTime.Now;
        }

        private static void CreateTweetsForMonth(string month)
        {
            MonthlyScoreKeeper keeper = new MonthlyScoreKeeper();
            List<Player> players = keeper.GetPlayers(month);

            ConsoleKeyInfo userInput = new ConsoleKeyInfo();

            var tweetMessage = CreateTotalScoreTweet(players);
            Console.WriteLine(tweetMessage);
            while (userInput.Key != ConsoleKey.C)
            {
                Clipboard.SetText(tweetMessage);
                Console.WriteLine(
                    "Total Score Tweet copied to clipboard. Press 'c' to continue, anything else to copy it again.");
                userInput = Console.ReadKey();
            }

            Console.Clear();

            tweetMessage = CreateALittleAlliterationScoreTweet(players);
            Console.WriteLine(tweetMessage);

            userInput = new ConsoleKeyInfo();
            while (userInput.Key != ConsoleKey.C)
            {

                Clipboard.SetText(tweetMessage);
                Console.WriteLine("A Little Alliteration Score Tweet copied to clipboard. Press 'c' to continue, anything else to copy it again.");
                userInput = Console.ReadKey();
            }
            Console.Clear();

            tweetMessage = CreateVowelMovementScoreTweet(players);
            Console.WriteLine(tweetMessage);

            userInput = new ConsoleKeyInfo();
            while (userInput.Key != ConsoleKey.C)
            {

                Clipboard.SetText(tweetMessage);
                Console.WriteLine("Vowel Movement Score Tweet copied to clipboard. Press 'c' to continue, anything else to copy it again.");
                userInput = Console.ReadKey();
            }

            Console.Clear();

            tweetMessage = CreateMagicWordSquareScoreTweet(players);
            Console.WriteLine(tweetMessage);
            userInput = new ConsoleKeyInfo();
            while (userInput.Key != ConsoleKey.C)
            {
                Clipboard.SetText(tweetMessage);
                Console.WriteLine("Magic Word Square Score Tweet copied to clipboard. Press 'c' to continue, anything else to copy it again.");
                userInput = Console.ReadKey();
            }

            Console.Clear();

            tweetMessage = CreateBalancedScoreTweet(players);
            Console.WriteLine(tweetMessage);
            userInput = new ConsoleKeyInfo();
            while (userInput.Key != ConsoleKey.C)
            {
                Clipboard.SetText(tweetMessage);
                Console.WriteLine("Balanced Score Tweet copied to clipboard. Press 'c' to continue, anything else to copy it again.");
                userInput = Console.ReadKey();
            }

            Console.Clear();
        }

        private static string CreateTotalScoreTweet(List<Player> players)
        {
            StringBuilder builder = new StringBuilder();

            players.Sort(Player.SortByTotalScore);
            builder.AppendLine("Top players by total score:");
            int playersMentionedSoFar = 0;
            int lastScore = 0;
            foreach (var player in players)
            {
                if (player.AlreadyMentioned) continue;
                if (4 < playersMentionedSoFar)
                {
                    if (player.TotalScore != lastScore) break;
                }

                lastScore = player.TotalScore;
                builder.AppendLine($"@{player.TwitterHandle} ({lastScore})");
                player.AlreadyMentioned = true;
                playersMentionedSoFar++;
            }

            string tweetMessage = builder.ToString();
            return tweetMessage;
        }

        private static string CreateALittleAlliterationScoreTweet(List<Player> players)
        {
            StringBuilder builder = new StringBuilder();

            players.Sort(Player.SortByALittleAlliterationScore);
            builder.AppendLine("Top A Little Alliteration players:");
            int playersMentionedSoFar = 0;
            int lastScore = 0;
            foreach (var player in players)
            {
                if (player.AlreadyMentioned) continue;
                if (4 < playersMentionedSoFar)
                {
                    if (player.ALittleAlliterationScore != lastScore) break;
                }

                lastScore = player.ALittleAlliterationScore;
                if (lastScore < 3) break;
                builder.AppendLine($"@{player.TwitterHandle} ({lastScore})");
                player.AlreadyMentioned = true;
                playersMentionedSoFar++;
            }

            string tweetMessage = builder.ToString();
            return tweetMessage;
        }

        private static string CreateVowelMovementScoreTweet(List<Player> players)
        {
            StringBuilder builder = new StringBuilder();

            players.Sort(Player.SortByVowelMovementScore);
            builder.AppendLine("Top Vowel Movement players:");
            int playersMentionedSoFar = 0;
            int lastScore = 0;
            foreach (var player in players)
            {
                if (player.AlreadyMentioned) continue;
                if (4 < playersMentionedSoFar)
                {
                    if (player.VowelMovementScore != lastScore) break;
                }

                lastScore = player.VowelMovementScore;
                if (lastScore < 3) break;
                builder.AppendLine($"@{player.TwitterHandle} ({lastScore})");
                player.AlreadyMentioned = true;
                playersMentionedSoFar++;
            }

            string tweetMessage = builder.ToString();
            return tweetMessage;
        }

        private static string CreateMagicWordSquareScoreTweet(List<Player> players)
        {
            StringBuilder builder = new StringBuilder();

            players.Sort(Player.SortByMagicWordSquareScore);
            builder.AppendLine("Top Magic Word Square players:");
            int playersMentionedSoFar = 0;
            int lastScore = 0;
            foreach (var player in players)
            {
                if (player.AlreadyMentioned) continue;
                if (4 < playersMentionedSoFar)
                {
                    if (player.MagicWordSquareScore != lastScore) break;
                }

                lastScore = player.MagicWordSquareScore;
                if (lastScore < 3) break;
                builder.AppendLine($"@{player.TwitterHandle} ({lastScore})");
                player.AlreadyMentioned = true;
                playersMentionedSoFar++;
            }

            string tweetMessage = builder.ToString();
            return tweetMessage;
        }

        private static string CreateBalancedScoreTweet(List<Player> players)
        {
            StringBuilder builder = new StringBuilder();

            players.Sort(Player.SortByMagicWordSquareScore);
            builder.AppendLine("Players who solved exactly the same number of each type of puzzle:");
            int playersMentionedSoFar = 0;
            int lastScore = 0;
            foreach (var player in players)
            {
                if (player.AlreadyMentioned) continue;
                if (player.ALittleAlliterationScore != player.VowelMovementScore ||
                    player.VowelMovementScore != player.MagicWordSquareScore)
                {
                    continue; //Only want players who solved the same number of every puzzle.
                }
                if (4 < playersMentionedSoFar)
                {
                    if (player.MagicWordSquareScore != lastScore ||
                        player.ALittleAlliterationScore != lastScore ||
                        player.VowelMovementScore != lastScore ) break;
                }

                lastScore = player.MagicWordSquareScore;
                if (lastScore == 0) break;
                builder.AppendLine($"@{player.TwitterHandle} ({lastScore})");
                player.AlreadyMentioned = true;
                playersMentionedSoFar++;
            }

            string tweetMessage = builder.ToString();
            return tweetMessage;
        }

        private static void InteractiveCreateAlphabetSoup(string solution)
        {
            AlphabetSoup puzzle = new AlphabetSoup();
            puzzle.GeneratePuzzle(solution);
            int currentIndex = 0;
            while (currentIndex < 26)
            {
                Console.WriteLine();
                for (int index = 0; index < 9; index++)
                {
                    if (26 <= index + currentIndex)
                    {
                        break;
                    }

                    string relatedWord = puzzle.HiddenWords[index + currentIndex];
                    Console.WriteLine($"{index}:{relatedWord} ({currentIndex+index}/26)");
                }

                Console.WriteLine("Enter the number of the first one to skip, or enter 9 to keep them all.");
                var userSelectedIndexAsKey = Console.ReadKey();
                int userSelectedIndexAsInt;
                if (int.TryParse(userSelectedIndexAsKey.KeyChar.ToString(), out userSelectedIndexAsInt))
                {
                    currentIndex += userSelectedIndexAsInt;
                    if (currentIndex < 26)
                    {
                        puzzle.GenerateLineAtIndex(currentIndex);
                    }
                }

            }

            Console.Clear();
            puzzle.ScrambleLines();
            var puzzleFormattedForGoogle = puzzle.FormatForGoogle();
            Console.WriteLine(puzzleFormattedForGoogle);

            var userInput = new ConsoleKeyInfo();
            while (userInput.Key != ConsoleKey.C)
            {

                Clipboard.SetText(puzzleFormattedForGoogle);

                Console.WriteLine("Puzzle copied to clipboard. Press 'c' to continue, anything else to copy it again.");
                userInput = Console.ReadKey();
            }
        }

        private static void InteractivelyRemoveUnwantedPuzzles(WeekOfPuzzles weekOfPuzzles)
        {
            if (weekOfPuzzles.MondayWordSquare == null)
            {
                Console.WriteLine("No Monday puzzle yet.");
            }
            else
            {
                Console.WriteLine(weekOfPuzzles.MondayWordSquare.GetTweet());
                Console.WriteLine("Found the above for Monday's word square. Press 'd' to delete, any other key to continue.");
                var userInputToDeleteMonday = Console.ReadKey();
                if (userInputToDeleteMonday.KeyChar == 'd')
                {
                    weekOfPuzzles.MondayWordSquare = null;
                    weekOfPuzzles.SelectedWords[0] = "";
                    Console.WriteLine("Deleted! Press any key to continue;");
                    Console.ReadKey();
                }

                Console.Clear();
            }

            if (weekOfPuzzles.TuesdayVowelMovement == null)
            {
                Console.WriteLine("No Tuesday puzzle yet.");
            }
            else
            {
                Console.WriteLine(weekOfPuzzles.TuesdayVowelMovement.GetTweet());
                Console.WriteLine("Found the above for Tuesday's vowel movement puzzle. Press 'd' to delete, any other key to continue.");
                var userInputToDeleteTuesday = Console.ReadKey();
                if (userInputToDeleteTuesday.KeyChar == 'd')
                {
                    weekOfPuzzles.TuesdayVowelMovement = null;
                    weekOfPuzzles.SelectedWords[1] = "";
                    Console.WriteLine("Deleted! Press any key to continue;");
                    Console.ReadKey();
                }

                Console.Clear();
            }


            if (weekOfPuzzles.WednesdayALittleAlliteration == null)
            {
                Console.WriteLine("No Wednesday puzzle yet.");
            }
            else
            {
                Console.WriteLine(weekOfPuzzles.WednesdayALittleAlliteration.GetTweet());
                Console.WriteLine("Found the above for Wednesday's vowel movement puzzle. Press 'd' to delete, any other key to continue.");
                var userInputToDeleteWednesday = Console.ReadKey();
                if (userInputToDeleteWednesday.KeyChar == 'd')
                {
                    weekOfPuzzles.WednesdayALittleAlliteration = null;
                    weekOfPuzzles.SelectedWords[2] = "";
                    Console.WriteLine("Deleted! Press any key to continue;");
                    Console.ReadKey();
                }

                Console.Clear();
            }


            if (weekOfPuzzles.ThursdayVowelMovement == null)
            {
                Console.WriteLine("No Thursday puzzle yet.");
            }
            else
            {
                Console.WriteLine(weekOfPuzzles.ThursdayVowelMovement.GetTweet());
                Console.WriteLine("Found the above for Thursday's a little alliteration puzzle. Press 'd' to delete, any other key to continue.");
                var userInputToDeleteThursday = Console.ReadKey();
                if (userInputToDeleteThursday.KeyChar == 'd')
                {
                    weekOfPuzzles.ThursdayVowelMovement = null;
                    weekOfPuzzles.SelectedWords[3] = "";
                    Console.WriteLine("Deleted! Press any key to continue;");
                    Console.ReadKey();
                }

                Console.Clear();
            }

            if (weekOfPuzzles.FridayALittleAlliteration == null)
            {
                Console.WriteLine("No Friday puzzle yet.");
            }
            else
            {
                Console.WriteLine(weekOfPuzzles.FridayALittleAlliteration.GetTweet());
                Console.WriteLine("Found the above for Friday's a little alliteration puzzle. Press 'd' to delete, any other key to continue.");
                var userInputToDeleteFriday = Console.ReadKey();
                if (userInputToDeleteFriday.KeyChar == 'd')
                {
                    weekOfPuzzles.FridayALittleAlliteration = null;
                    weekOfPuzzles.SelectedWords[4] = "";
                    Console.WriteLine("Deleted! Press any key to continue;");
                    Console.ReadKey();
                }

                Console.Clear();
            }

        }


        private static void CreateMagicWordSquare(string magicWordSquareTopLine)
        {
            if (!string.IsNullOrWhiteSpace(magicWordSquareTopLine))
            {
                var magicWordSquare = InteractiveFindWordSquare(magicWordSquareTopLine);

                if (magicWordSquare != null)
                {
                    string magicWordTweet = magicWordSquare.GetTweet();
                    Console.Clear();
                    Console.WriteLine(magicWordTweet);

                    var userInput = new ConsoleKeyInfo();
                    while (userInput.Key != ConsoleKey.C)
                    {
                        Clipboard.SetText(magicWordTweet);
                        Console.WriteLine("Copied to clipboard. Press 'c' to continue, or anything else to copy again.");
                        userInput = Console.ReadKey();
                    }
                }
            }
        }

        private static void GenerateTweetsForSelectedPuzzles(WeekOfPuzzles weekOfPuzzles)
        {
            ConsoleKeyInfo userInput;
            
            if (weekOfPuzzles.MondayWordSquare == null)
            {
                Console.WriteLine("No Magic word square selected for Monday.");
                Console.ReadKey();
            }
            else
            {
                string mondayTweet = weekOfPuzzles.MondayWordSquare.GetTweet();
                Console.WriteLine("Magic word square for Monday:");
                Console.WriteLine(mondayTweet);

                userInput = new ConsoleKeyInfo();
                while (userInput.Key != ConsoleKey.C)
                {
                    Clipboard.SetText(mondayTweet);
                    Console.WriteLine("Copied to clipboard. Press 'c' to continue or anything else to copy it again.");
                    userInput = Console.ReadKey();
                }

                Console.Clear();

            }

            if (weekOfPuzzles.TuesdayVowelMovement == null)
            {
                Console.WriteLine("No Vowel Movement selected for Tuesday.");
                Console.ReadKey();
            }
            else
            {
                string tuesdayTweet = weekOfPuzzles.TuesdayVowelMovement.GetTweet();
                Console.WriteLine("Vowel Movement for Tuesday:");
                Console.WriteLine(tuesdayTweet);
                userInput = new ConsoleKeyInfo();
                while (userInput.Key != ConsoleKey.C)
                {
                    Clipboard.SetText(tuesdayTweet);
                    Console.WriteLine("Copied to clipboard. Press 'c' to continue, anything else to copy again.");
                    userInput = Console.ReadKey();
                }
                Console.Clear();
            }

            if (weekOfPuzzles.WednesdayALittleAlliteration == null)
            {
                Console.WriteLine("No A Little Alliteration selected for Wednesday.");
                Console.ReadKey();
            }
            else
            {
                string wednesdayTweet = weekOfPuzzles.WednesdayALittleAlliteration.GetTweet();
                Console.WriteLine("A Little Alliteration for Wednesday:");
                Console.WriteLine(wednesdayTweet);

                userInput = new ConsoleKeyInfo();
                while (userInput.Key != ConsoleKey.C)
                {

                    Clipboard.SetText(wednesdayTweet);
                    Console.WriteLine("Copied to clipboard. Press 'c' to continue, anything else to copy again.");
                    userInput = Console.ReadKey();
                }
                Console.Clear();
            }

            if (weekOfPuzzles.ThursdayVowelMovement == null)
            {
                Console.WriteLine("No Vowel Movement selected for Thursday.");
                Console.ReadKey();
            }
            else
            {
                string thursdayTweet = weekOfPuzzles.ThursdayVowelMovement.GetTweet();
                Console.WriteLine("Vowel Movement for Thursday:");
                Console.WriteLine(thursdayTweet);

                userInput = new ConsoleKeyInfo();
                while (userInput.Key != ConsoleKey.C)
                {
                    Clipboard.SetText(thursdayTweet);
                    Console.WriteLine("Copied to clipboard. Press 'c' to continue, anything else to copy again.");
                    userInput = Console.ReadKey();
                }
                Console.Clear();
            }

            if (weekOfPuzzles.FridayALittleAlliteration == null)
            {
                Console.WriteLine("No A Little Alliteration selected for Friday.");
                Console.ReadKey();
            }
            else
            {
                string fridayTweet = weekOfPuzzles.FridayALittleAlliteration.GetTweet();
                Console.WriteLine("A Little Alliteration for Friday:");
                Console.WriteLine(fridayTweet);

                userInput = new ConsoleKeyInfo();
                while (userInput.Key != ConsoleKey.C)
                {
                    Clipboard.SetText(fridayTweet);
                    Console.WriteLine("Copied to clipboard. Press 'c' to continue, anything else to copy again.");
                    userInput = Console.ReadKey();
                }
                Console.Clear();
            }
        }

        private static WeekOfPuzzles GetWeekOfPuzzles(string theme, WeekOfPuzzles weekOfPuzzles = null )
        {
            if (weekOfPuzzles == null)
            {
                weekOfPuzzles = new WeekOfPuzzles();
            }
            List<string> threeLetterCombinationsAlreadyTried = new List<string>();

            Console.WriteLine($"Finding words related to {theme}");
            List<string> relatedWordsForTheme = WordRepository.GetRelatedWordsForTheme(theme);
            int index = 1;
            foreach (string relatedWord in relatedWordsForTheme)
            {
                Console.Clear();
                Console.WriteLine($"Looking for puzzles for {relatedWord}, which is word {index++}/{relatedWordsForTheme.Count}");

                //Then, find or create a word square.
                if (relatedWord.Length == 5 && weekOfPuzzles.MondayWordSquare == null)
                {
                    Console.WriteLine($"Starting to create a word square.");
                    weekOfPuzzles.MondayWordSquare = InteractiveFindWordSquare(relatedWord);
                    if (weekOfPuzzles.MondayWordSquare != null)
                    {
                        weekOfPuzzles.MondayWordSquare.Theme = theme;
                        weekOfPuzzles.SelectedWords[0] = relatedWord;
                        continue; //don't use this word for anything else.
                    }
                }

                //Then, find the Vowel Movements clues.
                if (weekOfPuzzles.ThursdayVowelMovement == null)
                {
                    Console.WriteLine($"Starting to create a vowel movement puzzle.");
                    string startConsonant;
                    string endConsonant;
                    if (WordRepository.IsSingleSyllable(relatedWord, out startConsonant, out endConsonant))
                    {
                        if (weekOfPuzzles.TuesdayVowelMovement == null)
                        {
                            weekOfPuzzles.TuesdayVowelMovement =
                                InteractiveFindVowelMovementClue(theme, relatedWord, startConsonant, endConsonant);
                            if (weekOfPuzzles.TuesdayVowelMovement != null)
                            {
                                weekOfPuzzles.SelectedWords[1] = relatedWord;
                                continue; //don't use this word for anything else.
                            }
                        }
                        else
                        {
                            weekOfPuzzles.ThursdayVowelMovement =
                                InteractiveFindVowelMovementClue(theme, relatedWord, startConsonant, endConsonant);
                            if (weekOfPuzzles.ThursdayVowelMovement != null)
                            {
                                weekOfPuzzles.SelectedWords[3] = relatedWord;
                                continue; //don't use this word for anything else.
                            }
                        }
                    }
                }

                //Finally, find the A Little Alliteration clues.
                if (weekOfPuzzles.FridayALittleAlliteration == null)
                {
                    Console.WriteLine($"Starting to create an A Little Alliteration Puzzle.");
                    string firstThreeLetters = relatedWord.Substring(0, 3);
                    if (!threeLetterCombinationsAlreadyTried.Contains(firstThreeLetters))
                    {
                        threeLetterCombinationsAlreadyTried.Add(firstThreeLetters);
                        if (weekOfPuzzles.WednesdayALittleAlliteration == null)
                        {
                            weekOfPuzzles.WednesdayALittleAlliteration =
                                InteractiveFindALittleAlliteration(theme, relatedWord, firstThreeLetters);
                            if (weekOfPuzzles.WednesdayALittleAlliteration != null)
                            {
                                weekOfPuzzles.SelectedWords[2] = relatedWord;
                            }
                        }
                        else
                        {
                            weekOfPuzzles.FridayALittleAlliteration =
                                InteractiveFindALittleAlliteration(theme, relatedWord, firstThreeLetters);
                            if (weekOfPuzzles.FridayALittleAlliteration != null)
                            {
                                weekOfPuzzles.SelectedWords[4] = relatedWord;
                            }
                        }
                        // ReSharper disable once RedundantJumpStatement
                        //Currently redundant, but will be used if another type of puzzle is added later.
                        continue; // Don't use this for anything else.
                    }
                }
            }

            return weekOfPuzzles;
        }

        private static ALittleAlliteration InteractiveFindALittleAlliteration(string theme, string relatedWord, string firstThreeLetters)
        {
            Console.WriteLine(
                $"Attempting to find A Little Alliteration puzzle for single syllable word {relatedWord} ...");
            ALittleAlliteration puzzleToReturn = new ALittleAlliteration();
            List<ALittleAlliteration> aLittleAlliterations = puzzleToReturn.FindPuzzle(firstThreeLetters);
            Console.WriteLine($"Found puzzle");
            for (int i = 0; i < aLittleAlliterations.Count; i++)
            {
                if (10 < i) break;
                ALittleAlliteration puzzle = aLittleAlliterations[i];
                Console.WriteLine($"{i}:{puzzle.Solution} = {puzzle.Clue}");
            }

            //Also write out related words
            int suggestedWordsCount;
            List<string> suggestedWords =
                puzzleToReturn.FindWordsThatStartWith(firstThreeLetters, out suggestedWordsCount);
            StringBuilder suggestedWordsGrid = new StringBuilder();
            int index = 0;
            foreach (string suggestedWord in suggestedWords)
            {
                suggestedWordsGrid.Append(suggestedWord);
                if (index < 4)
                {
                    if (suggestedWord.Length < 8)
                    {
                        suggestedWordsGrid.Append('\t');
                    }

                    suggestedWordsGrid.Append('\t');
                    index++;
                }
                else
                {
                    suggestedWordsGrid.Append(Environment.NewLine);
                    index = 0;
                }
            }

            Console.WriteLine($"Here are {suggestedWordsCount} words that will help if you want to create your own.");
            Console.WriteLine(suggestedWordsGrid);
            Console.WriteLine(
                $"Enter the number for the one you'd like to use or 'n' for none. Or create your own (solution = clue).");
            string userResponse = Console.ReadLine();
            int userSelectedIndex;
            if (int.TryParse(userResponse, out userSelectedIndex))
            {
                if (userSelectedIndex < aLittleAlliterations.Count)
                {
                    puzzleToReturn = aLittleAlliterations[userSelectedIndex];
                    puzzleToReturn.Theme = theme;
                    return puzzleToReturn;
                }
            }

            if (string.IsNullOrWhiteSpace(userResponse))
            {
                return null;
            }

            if (userResponse.ToLower() == "n")
            {
                return null;
            }

            ALittleAlliteration userCreatedPuzzle = new ALittleAlliteration(userResponse)
            {
                Theme = theme,
            };
            bool readyToContinue = false;
            while (!readyToContinue)
            {
                Clipboard.SetText(userCreatedPuzzle.GoogleSheetRow);
                Console.WriteLine(
                    "This new A Little Alliteration puzzle has been copied to the clipboard. Please paste it into the Google Sheet, and press 'c' to continue.");
                var key = Console.ReadKey();
                if (key.KeyChar == 'c')
                {
                    readyToContinue = true;
                }
            }

            return userCreatedPuzzle;
        }

        private static VowelMovement InteractiveFindVowelMovementClue(string theme, string relatedWord, string startConsonant, string endConsonant)
        {
            Console.WriteLine($"Attempting to find Vowel Movements puzzle for single syllable word {relatedWord} ...");
            VowelMovement puzzleToReturn = new VowelMovement();
            List<VowelMovement> vowelMovements = puzzleToReturn.FindPuzzle(startConsonant, endConsonant);
            if (0 < vowelMovements.Count)
            {
                Console.WriteLine($"Found puzzles");
                for (int i = 0; i < vowelMovements.Count; i++)
                {
                    Console.WriteLine($"{i}: {vowelMovements[i].Solution}");
                }

                Console.WriteLine($"Enter the number for the one you'd like to use or 'n' for none. Or create your own.");
                string userResponse = Console.ReadLine();
                int userSelectedIndex;
                if (int.TryParse(userResponse, out userSelectedIndex))
                {
                    if (userSelectedIndex < vowelMovements.Count)
                    {
                        var puzzleToUse = vowelMovements[userSelectedIndex];
                        puzzleToUse.Theme = theme;
                        return puzzleToUse;
                    }
                }

                if (string.IsNullOrWhiteSpace(userResponse))
                {
                    return null;
                }
                if (userResponse.ToLower() == "n")
                {
                    return null;
                }
                else
                {
                    var userEnteredPuzzle = new VowelMovement(userResponse)
                    {
                        Theme = theme, 
                        InitialConsonant =  startConsonant,
                        FinalConsonant =  endConsonant,
                    } ;
                    bool readyToContinue = false;
                    while (!readyToContinue)
                    {
                        Clipboard.SetText(userEnteredPuzzle.GoogleSheetRow);
                        Console.WriteLine(
                            "This new Vowel Movement puzzle has been copied to the clipboard. Please paste it into the Google Sheet, and press 'c' to continue.");
                        var key = Console.ReadKey();
                        if (key.KeyChar == 'c')
                        {
                            readyToContinue = true;
                        }
                    }

                    Console.WriteLine();
                    return userEnteredPuzzle;
                }
            }
            else
            {
                Console.WriteLine($"Unable to find a Vowel Movements puzzle for {startConsonant} - {endConsonant}.");
                return null;
            }
        }

        private static WordSquare InteractiveFindWordSquare(string relatedWord)
        {
            string fileWithMagicWordSquares = WordSquare.GetFileNameFor(relatedWord);
            if (!File.Exists(fileWithMagicWordSquares))
            {
                GenerateWordSquaresOfAnySize(relatedWord);
            }


            List<WordSquare> availableWordSquares = ReadAllWordSquaresFromFile(fileWithMagicWordSquares);
            availableWordSquares.Sort( (p,q) => History.CalculateScore(q) - History.CalculateScore(p));
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
                    for (int currentLineIndex = 1; currentLineIndex < 5; currentLineIndex++)
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
                        break;
                    }

                }
            }
            return selectedSquare;
        }

        private static List<WordSquare> ReadAllWordSquaresFromFile(string fileWithMagicWordSquares)
        {
            var allWordSquaresFromFile = new List<WordSquare>();
            string[] lines = File.ReadAllLines(fileWithMagicWordSquares);
            int lineCount = lines.Length;
            int availableSquareCount = lineCount / 6;
            for (int squareIndex = 0; squareIndex < availableSquareCount; squareIndex++)
            {
                WordSquare squareToAdd;

                string firstLine = lines[(squareIndex * 6) + 1];
                string secondLine = lines[(squareIndex * 6) + 2];
                string thirdLine = lines[(squareIndex * 6) + 3];
                string fourthLine = lines[(squareIndex * 6) + 4];
                string fifthLine = lines[(squareIndex * 6) + 5];

                {
                    squareToAdd = new WordSquare("");
                    squareToAdd.SetFirstLine(firstLine);
                    squareToAdd.SetSecondLine(secondLine);
                    squareToAdd.SetThirdLine(thirdLine);
                    squareToAdd.SetFourthLine(fourthLine);
                    squareToAdd.SetFifthLine(fifthLine);
                }
                allWordSquaresFromFile.Add(squareToAdd);
            }


            return allWordSquaresFromFile;
        }

        private static void GenerateWordSquaresOfAnySize(string firstWordCandidate)
        {
            WordSquare square = new WordSquare(new string('_', firstWordCandidate.Length));
            square.Repository = WordRepository;
            int[] wordsConsiderByLevel = new[] { 0, 0, 0, 0, 0, 0, };

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

            if (5 == linesPopulatedSoFar)
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

    }


    static class Shuffler
    {
        private static readonly Random RandomNumberGenerator = new Random();

        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = RandomNumberGenerator.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

    }

}
