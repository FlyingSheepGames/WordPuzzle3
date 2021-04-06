using System;
using System.Collections.Generic;
using System.Text;
using WordPuzzles.Puzzle.Legacy;
using WordPuzzles.Utility;

namespace WordPuzzleGenerator
{
    internal class ReadDownResearcher
    {
        WordRepository _repository = new WordRepository() ;
        public void Research()
        {
            var allPatterns = new List<string>();
            StringBuilder builder = new StringBuilder();
            for (int length = 3; length <= 10; length++)
            {
                if (length == 9) continue;
                allPatterns = GetAllPatternsByLength(length, builder);
                Console.WriteLine($"{allPatterns.Count} patterns of length {length}.");
                foreach (string pattern in allPatterns)
                {
                    for (char letter = 'a'; letter <= 'z'; letter++)
                    {
                        int countOfHits = CountWordsMatchingPattern(pattern.Replace('1', letter));
                        if (countOfHits < 6)
                        {
                            Console.WriteLine($"{letter} has {countOfHits} for {pattern}");
                        }
                    }
                }
                //Console.WriteLine("Press any key to continue");
                //Console.ReadKey();

            }

        }

        private static List<string> GetAllPatternsByLength(int length, StringBuilder builder)
        {
            List<string> allPatterns = new List<string>();
            for (int indexToReplace = 0; indexToReplace < length; indexToReplace++)
            {
                builder.Append('_', indexToReplace);
                builder.Append('1');
                builder.Append('_', (length - indexToReplace) - 1);
                allPatterns.Add(builder.ToString());
                builder.Clear();
            }

            return allPatterns;
        }

        private int CountWordsMatchingPattern(string pattern)
        {
            return _repository.WordsMatchingPattern(pattern).Count;
        }
    }
}