using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32.SafeHandles;

namespace WordPuzzles
{
    public class AlphabetSoup
    {
        WordRepository repository;
        static Random random = new Random();

        public string[] Lines = new String [26];
        public string[] HiddenWords = new String[26];
        public string Solution;

        public AlphabetSoup(WordRepository repositoryToUse)
        {
            repository = repositoryToUse;
        }

        public AlphabetSoup()
        {
            repository = new WordRepository();
        }

        public string GenerateSingleLine(char firstLetter, char middleLetter)
        {
            string hiddenWord;
            return GenerateSingleLine(firstLetter, middleLetter, out hiddenWord);
        }

        public string GenerateSingleLine(char firstLetter, char middleLetter, out string hiddenWord)
        {
            StartPosition selectedPosition = StartPosition.FirstPosition;
            List<string> possibleWords = new List<string>();
            switch (random.Next(3))
            {
                case 0: selectedPosition = StartPosition.FirstPosition;
                    foreach (string word in repository.WordsWithCharacterAtIndex(middleLetter, 3, 5))
                    {
                        if (word[0] == firstLetter)
                        {
                            possibleWords.Add(word);
                        }
                    }

                    if (0 < possibleWords.Count)
                    {
                        break;
                    }
                    //otherwise, let's use second position instead.
                    goto case 1;
                case 1:
                    selectedPosition = StartPosition.SecondPosition;
                    possibleWords = repository.WordsWithCharacterAtIndex(middleLetter, 2, 5);
                    break;
                case 2:
                    selectedPosition = StartPosition.ThirdPosition;
                    possibleWords = repository.WordsWithCharacterAtIndex(middleLetter, 1, 5);
                    break;
            }

            if (possibleWords.Count <= 0)
            {
                throw new Exception($"There are no words that start with {firstLetter} and have the letter {middleLetter} at position {selectedPosition}");
                //Todo: try a different position.
            }
            hiddenWord = possibleWords[random.Next(possibleWords.Count)];

            return HideWordInLine(hiddenWord, selectedPosition, firstLetter); 
        }

        internal string HideWordInLine(string hiddenWord, StartPosition selectedPosition, char firstCharacter = ' ')
        {
            string line = null;
            if (firstCharacter == ' ')
            {
                firstCharacter = (char) (97 + random.Next(26));
            }
            char secondExcessCharacter = (char)(97 + random.Next(26));

            bool tryAgain = true;
            while (tryAgain)
            {
                tryAgain = false;
                line = HideWordInLine(hiddenWord, selectedPosition, firstCharacter, secondExcessCharacter);
                string substringInFirstPosition = line.Substring(0, 5);
                string substringInSecondPosition = line.Substring(1, 5);
                string substringInThirdPosition = line.Substring(2, 5);

                switch (selectedPosition)
                {
                    case StartPosition.FirstPosition:
                        if (repository.IsAWord(substringInSecondPosition))
                        {
                            tryAgain = true;
                        }
                        if (repository.IsAWord(substringInThirdPosition))
                        {
                            tryAgain = true;
                        }
                        break;
                    case StartPosition.SecondPosition:
                        if (repository.IsAWord(substringInFirstPosition))
                        {
                            tryAgain = true;
                        }
                        if (repository.IsAWord(substringInThirdPosition))
                        {
                            tryAgain = true;
                        }
                        break;
                    case StartPosition.ThirdPosition:
                        if (repository.IsAWord(substringInFirstPosition))
                        {
                            tryAgain = true;
                        }
                        if (repository.IsAWord(substringInSecondPosition))
                        {
                            tryAgain = true;
                        }
                        break;
                }
                //Disallow 6 letter words. Also, the entire line cannot be a 7 letter word.
                if (repository.IsAWord(line.Substring(0, 6)) ||
                    repository.IsAWord(line.Substring(1, 6)) ||
                    repository.IsAWord(line))
                {
                    tryAgain = true;
                }
            }



            return line;
        }

        private static string HideWordInLine(string hiddenWord, StartPosition selectedPosition, char firstExcessCharacter,
            char secondExcessCharacter)
        {
            string line;
            StringBuilder builder = new StringBuilder();
            switch (selectedPosition)
            {
                case StartPosition.FirstPosition:
                    builder.Append(hiddenWord);
                    builder.Append(firstExcessCharacter);
                    builder.Append(secondExcessCharacter);
                    break;
                case StartPosition.SecondPosition:
                    builder.Append(firstExcessCharacter);
                    builder.Append(hiddenWord);
                    builder.Append(secondExcessCharacter);
                    break;
                case StartPosition.ThirdPosition:
                    builder.Append(firstExcessCharacter);
                    builder.Append(secondExcessCharacter);
                    builder.Append(hiddenWord);
                    break;
                default:
                    throw new Exception("Unexpected selected position");
            }

            return builder.ToString();
        }

        public List<string> GeneratePuzzle(string solution)
        {
            if (solution.Length != 26)
            {
                throw new ArgumentException("Solution needs to be exactly 26 characters", nameof(solution));
            }
            Solution = solution;
            Lines = new string[26];
            HiddenWords = new string[26];

            for (int index = 0; index < 26; index++)
            {
                GenerateLineAtIndex(index);
            }

            return new List<string>(Lines);
        }

        public void GenerateLineAtIndex(int index)
        {
            string hiddenWord = null;
            Lines[index] = GenerateSingleLine(Solution[index], (char) (97 + index), out hiddenWord);
            HiddenWords[index] = hiddenWord;
        }

        public string FormatForGoogle()
        {
            StringBuilder builder = new StringBuilder();
            foreach (string line in Lines)
            {
                for (int characterIndex = 0; characterIndex < 7; characterIndex++)
                {
                    if (characterIndex != 3)
                    {
                        builder.Append(line.Substring(characterIndex, 1).ToUpper());
                    }
                    else
                    {
                        builder.Append(" ");
                    }

                    builder.Append("\t");
                }

                builder.AppendLine();
            }
            return builder.ToString();
        }

        public void ScrambleLines()
        {
            int[] newIndicies = new int[26];

            string[] originalLines = new string[26];
            string[] originalHiddenWords = new string[26];

            for (int i = 0; i < 26; i++)
            {
                newIndicies[i] = i;
                originalLines[i] = Lines[i];
                originalHiddenWords[i] = HiddenWords[i];
            }
            newIndicies.Shuffle();


            for (int i = 0; i < 26; i++)
            {
                Lines[i] = originalLines[newIndicies[i]];
                HiddenWords[i] = originalHiddenWords[newIndicies[i]];
            }

        }
    }

    public enum StartPosition 
    {
        FirstPosition = 0,  //STUCK**
        SecondPosition = 1, //*STUCK*
        ThirdPosition = 2,  //**STUCK
    }

    static class Shuffler
    {
        private static Random rng = new Random();

        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

    }

}