using System;
using WordPuzzles.Utility;

namespace WordPuzzleGenerator
{
    public class IdiomParser
    {
        static readonly ClueRepository ClueRepository = new ClueRepository();

        private static void AddIdiomClues()
        {
            IdiomFinder idiomFinder = new IdiomFinder();
            int counter = 0;
            foreach (var idiom in idiomFinder.FindIdioms())
            {
                var cluesToAdd = idiomFinder.ExtractCluesFromIdiom(idiom);
                foreach (var wordWithNewClue in cluesToAdd.Keys)
                {
                    Console.WriteLine($"{counter} {wordWithNewClue}: {cluesToAdd[wordWithNewClue].ClueText}");
                    ClueRepository.AddClue(wordWithNewClue, cluesToAdd[wordWithNewClue].ClueText, ClueSource.ClueSourceIdiom);
                    counter++;
                    if (counter % 500 == 0)
                    {
                        Console.WriteLine("Press any key to continue.");
                        Console.ReadKey();
                    }
                }
            }
            Console.WriteLine("Done. Press any key to continue and save.");
            Console.ReadKey();
            ClueRepository.WriteToDisk();
            Console.WriteLine("Saved");
            Console.ReadKey();
        }

        private static void ListFourLetterIdioms()
        {
            IdiomFinder idiomFinder = new IdiomFinder();
            int counter = 0;
            foreach (var idiom in idiomFinder.FindIdioms())
            {
                bool allFourLetterWords = true;
                foreach (var word in idiom.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries))
                {
                    if (word.Length != 4)
                    {
                        allFourLetterWords = false;
                        break;
                    }
                }

                if (allFourLetterWords)
                {
                    Console.WriteLine();
                    Console.WriteLine(idiom);
                    Console.ReadKey();
                }
                else
                {
                    Console.Write(".");
                }
            }
            Console.WriteLine("Done. Press any key to continue.");
            Console.ReadKey();
        }

    }
}