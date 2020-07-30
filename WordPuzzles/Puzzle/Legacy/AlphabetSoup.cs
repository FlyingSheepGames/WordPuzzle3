using System;
using System.Collections.Generic;
using System.Text;
using WordPuzzles.Utility;

namespace WordPuzzles.Puzzle.Legacy
{
    public class AlphabetSoup
    {
        readonly WordRepository _repository;
        Random _randomNumberGenerator;

        public string[] Lines = new string [26];
        public string[] HiddenWords = new string[26];
        public string Solution;

        public AlphabetSoup(WordRepository repositoryToUse)
        {
            _repository = repositoryToUse;
        }

        public AlphabetSoup() : this (new WordRepository())
        {
        }

        public int RandomSeed = 0;

        public  Random RandomNumberGenerator
        {
            get
            {
                if (_randomNumberGenerator == null)
                {
                    if (RandomSeed == 0)
                    {
                        _randomNumberGenerator = new Random();
                    }
                    else
                    {
                        _randomNumberGenerator = new Random(RandomSeed);
                    }
                }
                return _randomNumberGenerator;
            }
        }

        public string GenerateSingleLine(char firstLetter, char middleLetter)
        {
            return GenerateSingleLine(firstLetter, middleLetter, out _);
        }

        public string GenerateSingleLine(char firstLetter, char middleLetter, out string hiddenWord)
        {
            StartPosition selectedPosition = StartPosition.FirstPosition;

            switch (RandomNumberGenerator.Next(3))
            {
                case 0: selectedPosition = StartPosition.FirstPosition;
                    break;
                case 1:
                    selectedPosition = StartPosition.SecondPosition;
                    break;
                case 2:
                    selectedPosition = StartPosition.ThirdPosition;
                    break;
            }

            return GenerateSingleLine(firstLetter, middleLetter, out hiddenWord, selectedPosition);
        }

        private string GenerateSingleLine(char firstLetter, char middleLetter, out string hiddenWord,
            StartPosition selectedPosition)
        {
            StringBuilder patternBuilder = new StringBuilder();
            List<string> possibleWords;
            switch (selectedPosition)
            {
                case StartPosition.FirstPosition:
                    patternBuilder.Append(firstLetter);
                    patternBuilder.Append("__");
                    patternBuilder.Append(middleLetter);
                    patternBuilder.Append("_");
                    break;
                case StartPosition.SecondPosition:
                    patternBuilder.Append("__");
                    patternBuilder.Append(middleLetter);
                    patternBuilder.Append("__");
                    break;
                case StartPosition.ThirdPosition:
                    patternBuilder.Append("_");
                    patternBuilder.Append(middleLetter);
                    patternBuilder.Append("___");
                    break;
            }

            possibleWords = _repository.WordsMatchingPattern(patternBuilder.ToString());
            if (possibleWords.Count <= 0)
            {
                throw new Exception(
                    $"There are no words that start with {firstLetter} and have the letter {middleLetter} at position {selectedPosition}");
                //Todo: try a different position.
            }

            hiddenWord = possibleWords[RandomNumberGenerator.Next(possibleWords.Count)];
            return HideWordInLine(hiddenWord, selectedPosition, firstLetter);
        }

        internal string HideWordInLine(string hiddenWord, StartPosition selectedPosition, char firstCharacter = ' ')
        {
            string line = null;
            if (firstCharacter == ' ')
            {
                firstCharacter = (char) (97 + RandomNumberGenerator.Next(26));
            }
            char secondExcessCharacter = (char)(97 + RandomNumberGenerator.Next(26));

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
                        if (_repository.IsAWord(substringInSecondPosition))
                        {
                            tryAgain = true;
                        }
                        if (_repository.IsAWord(substringInThirdPosition))
                        {
                            tryAgain = true;
                        }
                        break;
                    case StartPosition.SecondPosition:
                        if (_repository.IsAWord(substringInFirstPosition))
                        {
                            tryAgain = true;
                        }
                        if (_repository.IsAWord(substringInThirdPosition))
                        {
                            tryAgain = true;
                        }
                        break;
                    case StartPosition.ThirdPosition:
                        if (_repository.IsAWord(substringInFirstPosition))
                        {
                            tryAgain = true;
                        }
                        if (_repository.IsAWord(substringInSecondPosition))
                        {
                            tryAgain = true;
                        }
                        break;
                }
                //Disallow 6 letter words. 
                //TODO: Also, the entire line cannot be a 7 letter word, but the repository doesn't support 7 letter words yet. 
                if (_repository.IsAWord(line.Substring(0, 6)) ||
                    _repository.IsAWord(line.Substring(1, 6)) )
                {
                    tryAgain = true;
                }
            }



            return line;
        }

        private static string HideWordInLine(string hiddenWord, StartPosition selectedPosition, char firstExcessCharacter,
            char secondExcessCharacter)
        {
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
            string hiddenWord;
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
        Undefined
    }
}