using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json;
using WordPuzzles.Puzzle;
using WordPuzzles.Utility;

namespace WordPuzzleGenerator
{
    public class StatisticsCalculator
    {
        static readonly WordRepository WordRepository = new WordRepository() { ExcludeAdvancedWords = true };

        /*
 * Discoveries:
 * 1. J is almost always the first letter of a three letter word containing it.
 * 2. IOU are almost always the middle letter of words in which they appear.
 * 3. X is almost always the last letter of any three letter word containing it. 
 *
 */
        private static void CalculateStatisticsForThreeLetterWords()
        {
            int[,] letterFrequency = new int[26, 3];
            for (int letter = 'a' - 'a'; letter < 'z' - 'a'; letter++)
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
                int startOfTen = (startCount * 10) / totalCount;
                int middleOfTen = (middleCount * 10) / totalCount;
                int endOfTen = (endCount * 10) / totalCount;
                middleOfTen = 10 - (startOfTen + endOfTen);
                Console.Write($"{(char)(letter + 'A')}\t");//{startOfTen}\t{middleOfTen}\t{endOfTen}");

                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(new string('#', startOfTen));
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
                Console.WriteLine($"{(char)(i + 'a')} has {cluesForThisLetter} clue pairs.");
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

        /* being picky
         a has 32 clue pairs.
b has 17 clue pairs.
c has 29 clue pairs.
d has 15 clue pairs.
e has 56 clue pairs.
f has 16 clue pairs.
g has 20 clue pairs.
h has 35 clue pairs.
i has 32 clue pairs.
j has 5 clue pairs.
k has 26 clue pairs.
l has 89 clue pairs.
m has 33 clue pairs.
n has 56 clue pairs.
o has 33 clue pairs.
p has 35 clue pairs.
r has 107 clue pairs.
s has 54 clue pairs.
t has 48 clue pairs.
u has 29 clue pairs.
v has 15 clue pairs.
w has 23 clue pairs.
x has 4 clue pairs.
y has 8 clue pairs.
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
            var takeOneCluesAsList = new List<TakeTwoClue>();
            var takeOneCluesAsDictionary = new Dictionary<char, List<TakeTwoClue>>();
            bool bePicky = false;
            for (char letterToPlace = 'a'; letterToPlace <= 'z'; letterToPlace++)
            {
                bePicky = false;
                takeOneCluesAsDictionary.Add(letterToPlace, new List<TakeTwoClue>());
                for (int wordLength = 4; wordLength < 9; wordLength++)
                {
                    for (int firstLetterIndex = 0; firstLetterIndex < wordLength; firstLetterIndex++)
                    {
                        if (bePicky)
                        {
                            if (firstLetterIndex == 0 || firstLetterIndex == (wordLength - 1))
                            {
                                continue;//Skip the ones where the letter is at the start or end of the word. 
                            }
                        }
                        var placeLetterAtTheseIndicies = new List<int>() { firstLetterIndex };
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
                                    if (5 < countPerLetterRemoved[letterToPlace - 'a'])
                                    {
                                        bePicky = true;
                                    }
                                    var clueToAdd = new TakeTwoClue()
                                    {
                                        LongerWord = longerWord,
                                        ShorterWord = shorterWord,
                                        LetterRemoved = letterToPlace
                                    };
                                    takeOneCluesAsList.Add(clueToAdd);
                                    takeOneCluesAsDictionary[letterToPlace].Add(clueToAdd);
                                    Console.WriteLine($"Found a pair: {longerWord} and {shorterWord}.");
                                    //Console.ReadKey();
                                }
                            }
                        }
                    }
                }
            }
            Console.WriteLine($"Found {takeOneCluesAsList.Count} clues.");
            for (int i = 0; i < 26; i++)
            {
                int cluesForThisLetter = countPerLetterRemoved[i];
                if (cluesForThisLetter == 0) continue;
                Console.WriteLine($"{(char)(i + 'a')} has {cluesForThisLetter} clue pairs.");
            }

            var serializedDictionary = JsonConvert.SerializeObject(takeOneCluesAsDictionary);
            File.WriteAllText(@"C:\Users\Chip\Source\Repos\WordPuzzle3\WordPuzzlesTest\data\AddALetter\dictionary.json", serializedDictionary);

            Console.ReadKey();
            return takeOneCluesAsList;
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

        private static void ListWordsThatCanBeShifted()
        {
            WordRepository repository = new WordRepository() { ExcludeAdvancedWords = false };
            StringBuilder completeList = new StringBuilder();
            foreach (string pattern in new[] { "___", "____", "_____", "______" })
            {
                foreach (string word in repository.WordsMatchingPattern(pattern))
                {
                    bool reachedShiftLimit = false;
                    for (int shift = 1; shift < 26; shift++)
                    {
                        StringBuilder shiftedWord = new StringBuilder();
                        foreach (char letter in word)
                        {
                            char shiftedLetter = (char)(letter + shift);
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
            WordRepository repositoryWithAdvancedWords = new WordRepository() { ExcludeAdvancedWords = false };
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

    }
}