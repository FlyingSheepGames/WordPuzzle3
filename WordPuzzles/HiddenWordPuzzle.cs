using System;
using System.Collections.Generic;
using System.Text;

namespace WordPuzzles
{
    public class HiddenWordPuzzle
    {
        public List<string> Sentences = new List<string>();
        public string Solution; 

        Random _random;
        public int RandomSeed { get; set; }

        internal WordRepository repository = new WordRepository() {ExludeAdvancedWords = true};
        Random Random
        {
            get
            {
                if (_random is null)
                {
                    if (RandomSeed != 0)
                    {
                        _random = new Random(RandomSeed);
                    }
                    else
                    {
                        _random = new Random();
                    }
                }
                return _random;
            }
        }

        public List<string> HideWord(string wordToHide)
        {
            List<string> puzzlePhrase = new List<string>();

            bool foundFirstWord = false;
            foreach (int whereToBreakWord in GenerateWordBreaks(wordToHide.Length))
            {
                foundFirstWord = false;
                var firstPartOfWord = wordToHide.Substring(0, whereToBreakWord);
                string lastPartOfWord = wordToHide.Substring(whereToBreakWord);

                StringBuilder firstPatternBuilder = new StringBuilder();
                bool foundSecondWord = false;

                for (int numberOfBlanksToPrepend = 1;
                    numberOfBlanksToPrepend < 7 - whereToBreakWord;
                    numberOfBlanksToPrepend++)
                {
                    if (numberOfBlanksToPrepend + whereToBreakWord < 3)
                    {
                        continue;
                    }

                    firstPatternBuilder.Clear();
                    firstPatternBuilder.Append('_', numberOfBlanksToPrepend);
                    firstPatternBuilder.Append(firstPartOfWord);
                    var firstPattern = firstPatternBuilder.ToString();
                    Console.WriteLine(
                        $"Looking for first word match {firstPattern} with {numberOfBlanksToPrepend} blanks.");
                    var firstMatchingWords = repository.WordsMatchingPattern(firstPattern);
                    if (0 == firstMatchingWords.Count)
                    {
                        //skip to next word. 
                        continue;
                    }

                    puzzlePhrase.Add(firstMatchingWords[Random.Next(firstMatchingWords.Count)]);
                    foundFirstWord = true;
                    break;
                }

                if (!foundFirstWord)
                {
                    continue;
                }
                foundSecondWord = false;
                var minimumNumberOfBlanks = 3 - lastPartOfWord.Length;
                if (minimumNumberOfBlanks < 1)
                {
                    minimumNumberOfBlanks = 1;
                }

                for (int numberOfBlanksToAdd = minimumNumberOfBlanks;
                    numberOfBlanksToAdd < 7 - (lastPartOfWord.Length);
                    numberOfBlanksToAdd++)
                {

                    StringBuilder secondPatternBuilder = new StringBuilder();
                    secondPatternBuilder.Append(lastPartOfWord);

                    secondPatternBuilder.Append('_', numberOfBlanksToAdd);
                    var secondPattern = secondPatternBuilder.ToString();
                    Console.WriteLine(
                        $"Looking for second word using pattern {secondPattern}, adding {numberOfBlanksToAdd} blanks");
                    var secondMatchingWords = repository.WordsMatchingPattern(secondPattern);
                    if (0 == secondMatchingWords.Count)
                    {
                        continue;
                    }

                    puzzlePhrase.Add(secondMatchingWords[Random.Next(secondMatchingWords.Count)]);
                    foundSecondWord = true;
                    break;
                }

                if (foundSecondWord)
                {
                    break;
                }
                puzzlePhrase.Clear();
            }

            return puzzlePhrase;
        }

        internal IEnumerable<int> GenerateWordBreaks(int length)
        {
            var orderedList = new List<int>();
            for (int i = 1; i < length; i++)
            {
                orderedList.Add(i);
            }

            var randomList = new List<int>();
            for (int randomIndex = 1; randomIndex < length; randomIndex++)
            {
                var indexToRemove = Random.Next(orderedList.Count);
                randomList.Add(orderedList[indexToRemove]);
                orderedList.RemoveAt(indexToRemove);
            }

            return randomList;
        }

        public string FormatPuzzleAsText()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("One word in each sentence below is hidden elsewhere in the sentence.");
            builder.AppendLine("Find the word, and then write the first letter of that word into the blanks below.");
            int sentenceCounter = 1;
            foreach (string sentence in Sentences)
            {
                builder.AppendLine($"{sentenceCounter++}. {sentence}");
            }

            builder.Append("Solution: ");
            foreach (var letter in Solution)
            {
                if (char.IsLetter(letter))
                {
                    builder.Append("_ ");
                }
                else
                {
                    builder.Append(letter);
                }
            }

            builder.AppendLine();
            return builder.ToString();
        }
    }
}