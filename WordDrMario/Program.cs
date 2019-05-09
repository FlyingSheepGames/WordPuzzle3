using System;
using WordPuzzles;

namespace WordDrMario
{
    class Program
    {
        static void Main()
        {
            const int BOARD_WIDTH = 6;
            Console.CursorVisible = false;

            int score = 0;

            WordRepository repository = new WordRepository() { IgnoreCache = true }; //load up the latest words once.
            repository.LoadAllWords();

            WordGrid grid = new WordGrid(BOARD_WIDTH);
            grid.FindHorizontalWords(); //won't find anything, but will initialize WordRepository. 


            string nextLetterPair = "ET";
            int nextLetterPairIndex = BOARD_WIDTH / 2;
            bool nextLetterPairHorizontal = true;

            bool quit = false;
            bool stillAlive = true;

            while (!quit)
            {
                
                Console.Clear();
                DisplayScreen(grid, nextLetterPair, nextLetterPairIndex, nextLetterPairHorizontal, stillAlive, score);

                bool readyForNextStep = false;
                while (!readyForNextStep)
                {
                    var key = Console.ReadKey(true);
                    if (!stillAlive)
                    {
                        quit = true;
                        break;
                    }
                    switch (key.Key)
                    {
                        case ConsoleKey.Q:
                            quit = true;
                            readyForNextStep = true;
                            break;
                        case ConsoleKey.RightArrow:
                            int rightLimit;
                            if (nextLetterPairHorizontal)
                            {
                                rightLimit = BOARD_WIDTH - 2;
                            }
                            else
                            {
                                rightLimit = BOARD_WIDTH - 1;
                            }

                            if (nextLetterPairIndex < rightLimit)
                                {
                                    nextLetterPairIndex++;
                                }

                            readyForNextStep = true;
                            break;
                        case ConsoleKey.LeftArrow:
                            if (0 < nextLetterPairIndex)
                            {
                                nextLetterPairIndex--;
                            }
                            readyForNextStep = true;
                            break;
                        case ConsoleKey.DownArrow:
                        case ConsoleKey.Spacebar:
                            if (nextLetterPairHorizontal)
                            {
                                if (!grid.DropLetter(nextLetterPair[0], nextLetterPairIndex))
                                {
                                    stillAlive = false;
                                }

                                if (!grid.DropLetter(nextLetterPair[1], nextLetterPairIndex + 1))
                                {
                                    stillAlive = false;
                                }
                            }
                            else
                            {
                                if (!grid.DropLetter(nextLetterPair[1], nextLetterPairIndex))
                                {
                                    stillAlive = false;
                                }

                                if (!grid.DropLetter(nextLetterPair[0], nextLetterPairIndex))
                                {
                                    stillAlive = false;
                                }
                            }

                            grid.FindHorizontalWords();
                            grid.FindVerticalWords();
                            
                            nextLetterPair = GenerateNextLetterPair();
                            nextLetterPairIndex = BOARD_WIDTH / 2;
                            nextLetterPairHorizontal = true;
                            readyForNextStep = true;
                            break;
                        case ConsoleKey.UpArrow: //Spin
                            nextLetterPairHorizontal = !nextLetterPairHorizontal;
                            if (nextLetterPairHorizontal)
                            {
                                nextLetterPair = string.Concat(nextLetterPair[1], nextLetterPair[0]);
                                if (BOARD_WIDTH -2 < nextLetterPairIndex)
                                {
                                    nextLetterPairIndex = BOARD_WIDTH -2;
                                }
                            }
                            readyForNextStep = true;
                            break;
                        case ConsoleKey.S: //Score
                            for (int animationFrame = 0; animationFrame < 3; animationFrame++)
                            {
                                Console.Clear();
                                DisplayScreen(grid, nextLetterPair, nextLetterPairIndex, nextLetterPairHorizontal, true, score, animationFrame);
                                new System.Threading.ManualResetEvent(false).WaitOne(800);
                                //Thread.Sleep(1000);
                            }
                            score += grid.CalculateScore();

                            grid.DeleteFoundWords();
                            Console.Clear();
                            DisplayScreen(grid, nextLetterPair, nextLetterPairIndex, nextLetterPairHorizontal, true, score);
                            new System.Threading.ManualResetEvent(false).WaitOne(800);

                            grid.DropAllLetters();
                            grid.FindHorizontalWords();
                            grid.FindVerticalWords();
                            Console.Clear();
                            DisplayScreen(grid, nextLetterPair, nextLetterPairIndex, nextLetterPairHorizontal, true, score);
                            new System.Threading.ManualResetEvent(false).WaitOne(800);


                            readyForNextStep = true;
                            break;
                    }
                }
            }
        }

        private static string GenerateNextLetterPair()
        {
            Random random = new Random();
            string[] letterPairs = {"ET", "AN", "TH", "HE", "AN", "IN", "ER", "ND", "RE", "ED", "ES", "OU", "TO"};
            return letterPairs[random.Next(letterPairs.Length)];
        }

        private static void DisplayScreen(WordGrid grid, string nextLetterPair, int nextLetterPairIndex, bool nextLetterPairHorizontal, bool stillAlive, int score, int scoringAnimationFrame = 0)
        {
            if (!stillAlive)
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }

            if (nextLetterPairHorizontal)
            {
                Console.WriteLine();
                Console.Write(new string(' ', nextLetterPairIndex + 1));
                Console.Write(nextLetterPair);
                Console.WriteLine();
            }
            else
            {
                Console.Write(new string(' ', nextLetterPairIndex + 1));
                Console.WriteLine(nextLetterPair[0]);
                Console.Write(new string(' ', nextLetterPairIndex + 1));
                Console.WriteLine(nextLetterPair[1]);
            }

            grid.WriteToConsole(scoringAnimationFrame);
            //Console.WriteLine(grid);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine();
            if (!stillAlive)
            {
                Console.WriteLine($"{score:D3} You've lost. Press any key to exit.");
            }
            else
            {
                Console.WriteLine($"{score:D3} ");
            }
        }
    }
}
