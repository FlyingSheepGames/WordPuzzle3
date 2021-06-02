using System;
using System.Collections.Generic;
using WordPuzzles.Utility;

namespace WordPuzzleGenerator
{
    public class PatternMatcher
    {
        static readonly WordRepository WordRepository = new WordRepository() { ExcludeAdvancedWords = true };

        private static void FindWordsMatchingPattern(string pattern)
        {
            var canidates = WordRepository.WordsMatchingPattern(new string('_', pattern.Length));
            Console.WriteLine($"There are {canidates.Count} words to consider.");
            foreach (var canidate in canidates)
            {
                if (DoesWordMatchPattern(canidate, pattern))
                {
                    Console.WriteLine();
                    Console.WriteLine(canidate);
                }
                else
                {
                    Console.Write('.');
                }

            }
            Console.WriteLine("We've looked at all the words.");
            Console.ReadKey();
        }

        private static bool DoesWordMatchPattern(string canidate, string pattern)
        {
            for (var index = 0; index < canidate.Length; index++)
            {
                var letter = canidate[index];
                if (letter == 'y') continue;
                switch (pattern[index])
                {
                    case 'v':
                        if (!IsVowel(letter)) return false;
                        break;
                    case 'c':
                        if (IsVowel(letter)) return false;
                        break;
                    default:
                        throw new Exception("not c, not v");
                }
            }

            return true;
        }

        private static bool IsVowel(char letter)
        {
            if (new List<char> { 'a', 'e', 'i', 'o', 'u', 'y' }.Contains(letter))
            {
                return true;
            }

            return false;
        }

    }
}