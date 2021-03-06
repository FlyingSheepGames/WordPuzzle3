﻿using System;
using System.Collections.Generic;
using System.Text;
using WordPuzzles.Utility;

namespace WordPuzzles.Puzzle.Legacy
{
    public class HiddenWordPuzzle
    {
        public List<string> Sentences = new List<string>();
        public string Solution; 

        Random _random;
        public int RandomSeed { get; set; }

        internal WordRepository Repository = new WordRepository() {ExcludeAdvancedWords = true};
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
            return HideWordReplacement(wordToHide);
        }

        public List<string> HideWordReplacement(string wordToHide)
        {
            var splitableStringOptions = GenerateAllSplitableStrings(wordToHide);
            if (splitableStringOptions.Count == 0)
            {
                return new List<string>();
            }

            string randomlySelectedSplitableString = splitableStringOptions[Random.Next(splitableStringOptions.Count)];

            var wordsContainingHiddenWord = CreateSpecificExampleFromSplitableString(randomlySelectedSplitableString);

            return wordsContainingHiddenWord;
        }

        public List<string> CreateSpecificExampleFromSplitableString(string randomlySelectedSplitableString)
        {
            var wordsContainingHiddenWord = new List<string>();
            //Verbose
            //Console.WriteLine(string.Join(Environment.NewLine, splitableStringOptions));
            string[] tokens = randomlySelectedSplitableString.Split('.');

            //Handle first token
            string firstToken = tokens[0];
            List<string> wordsThatEndWithFirstToken = FindAllWordsThatEndWith(firstToken);
            //Because of the Verify ... method called before, I expect this to be a non-empty list.
            string randomlySelectedFirstWord = wordsThatEndWithFirstToken[Random.Next(wordsThatEndWithFirstToken.Count)];

            wordsContainingHiddenWord.Add(randomlySelectedFirstWord);

            //Handle middle tokens...
            for (int middleWordIndex = 1; middleWordIndex < tokens.Length - 1; middleWordIndex++)
            {
                wordsContainingHiddenWord.Add(tokens[middleWordIndex]);
            }

            //Handle last token.
            string lastToken = tokens[tokens.Length - 1];
            List<string> wordsThatStartWithLastToken = FindAllWordsThatStartWith(lastToken);
            //Because of the Verify ... method called before, I expect this to be a non-empty list.
            string randomlySelectedLastWord =
                wordsThatStartWithLastToken[Random.Next(wordsThatStartWithLastToken.Count)];
            wordsContainingHiddenWord.Add(randomlySelectedLastWord);
            return wordsContainingHiddenWord;
        }

        internal List<string> FindAllWordsThatStartWith(string lastToken)
        {
            var allWordsThatStartWith = new List<string>();
            for (int numberOfBlanks = 1; numberOfBlanks < 7 - lastToken.Length; numberOfBlanks++)
            {
                StringBuilder patternBuilder = new StringBuilder();
                patternBuilder.Append(lastToken);
                patternBuilder.Append('_', numberOfBlanks);
                List<string> wordsMatchingPattern = Repository.WordsMatchingPattern(patternBuilder.ToString());
                //Console.WriteLine($"{wordsMatchingPattern.Count} words match '{patternBuilder}'");
                allWordsThatStartWith.AddRange(wordsMatchingPattern);
            }
            return allWordsThatStartWith;
        }

        private List<string> FindAllWordsThatEndWith(string firstToken)
        {
            var allWordsThatEndWith = new List<string>();
            for (int numberOfBlanks = 1; numberOfBlanks < 7 - firstToken.Length; numberOfBlanks++)
            {
                StringBuilder patternBuilder = new StringBuilder();
                patternBuilder.Append('_', numberOfBlanks);
                patternBuilder.Append(firstToken);
                allWordsThatEndWith.AddRange(Repository.WordsMatchingPattern(patternBuilder.ToString()));
            }
            return allWordsThatEndWith;
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

        public List<string> GenerateAllSplitableStrings(string wordToSplit)
        {
            var splitableStrings = new List<string>();

            for (int lengthOfFirstToken = 1; lengthOfFirstToken < wordToSplit.Length; lengthOfFirstToken++)
            {
                StringBuilder splitableStringBuilder = new StringBuilder();
                splitableStringBuilder.Append(wordToSplit.Substring(0, lengthOfFirstToken));
                splitableStringBuilder.Append(".");
                var remainingLetters = wordToSplit.Substring(lengthOfFirstToken);
                //Console.WriteLine($"Called with {wordToSplit}, {remainingLetters}, {splitableStringBuilder.ToString()}, {lengthOfAllPreviousTokens}, {string.Join( " - ", splitableStrings)}");
                foreach (string word in FindWordsAtTheStartOfThisString(remainingLetters))
                {
                    StringBuilder builder = new StringBuilder(splitableStringBuilder.ToString());
                    builder.Append(word);
                    builder.Append(".");
                    int lengthOfAllPreviousTokens = builder.ToString().Replace(".", "").Length;
                    if (lengthOfAllPreviousTokens < wordToSplit.Length)
                    {
                        var remainingLettersAfterWordRemoved = wordToSplit.Substring(lengthOfAllPreviousTokens);
                        ProcessRemainingLetters(wordToSplit, remainingLettersAfterWordRemoved, builder, splitableStrings);

                        builder.Append(remainingLettersAfterWordRemoved);
                        string splitableStringCandidate1 = builder.ToString();
                        if (VerifySplitableStringCandidate(splitableStringCandidate1))
                        {
                            splitableStrings.Add(splitableStringCandidate1);
                        }
                    }
                }

                splitableStringBuilder.Append(remainingLetters);

                string splitableStringCandidate = splitableStringBuilder.ToString();
                if (VerifySplitableStringCandidate(splitableStringCandidate))
                {
                    splitableStrings.Add(splitableStringCandidate);
                }
            }

            return splitableStrings;
        }

        internal bool VerifySplitableStringCandidate(string splitableStringCandidate)
        {
            //Verify that there is a word that will end with the first token, and there is a word that will start with the last token.
            string[] tokens = splitableStringCandidate.Split('.');
            string firstToken = tokens[0];
            bool firstTokenValid = false;
            for (int numberOfBlanks = 1; numberOfBlanks < 7 - firstToken.Length; numberOfBlanks++)
            {
                StringBuilder patternBuilder = new StringBuilder();
                patternBuilder.Append('_', numberOfBlanks);
                patternBuilder.Append(firstToken);
                if (0 < Repository.WordsMatchingPattern(patternBuilder.ToString()).Count)
                {
                    firstTokenValid = true;
                    break;
                }
            }

            if (!firstTokenValid)
            {
                //Console.WriteLine($"No word ends with {firstToken}.");
                return false;
            }

            bool lastTokenValid = false;
            string lastToken = tokens[tokens.Length - 1];
            for (int numberOfBlanks = 1; numberOfBlanks < 7 - lastToken.Length; numberOfBlanks++)
            {
                StringBuilder patternBuilder = new StringBuilder();
                patternBuilder.Append(lastToken);
                patternBuilder.Append('_', numberOfBlanks);
                if (0 < Repository.WordsMatchingPattern(patternBuilder.ToString()).Count)
                {
                    lastTokenValid = true;
                    break;
                }
            }

            if (!lastTokenValid)
            {
                //Console.WriteLine($"No word starts with {lastToken}.");
            }
            return lastTokenValid;
        }

        internal void ProcessRemainingLetters(string wordToSplit, string remainingLetters, StringBuilder splitableStringBuilder, List<string> splitableStrings)
        {
            //Console.WriteLine($"Called with {wordToSplit}, {remainingLetters}, {splitableStringBuilder.ToString()}, {lengthOfAllPreviousTokens}, {string.Join( " - ", splitableStrings)}");
            foreach (string word in FindWordsAtTheStartOfThisString(remainingLetters))
            {
                StringBuilder builder = new StringBuilder(splitableStringBuilder.ToString());
                builder.Append(word);
                builder.Append(".");
                int lengthOfAllPreviousTokens = builder.ToString().Replace(".", "").Length;
                if (lengthOfAllPreviousTokens < wordToSplit.Length)
                {
                    var remainingLettersAfterWordRemoved = wordToSplit.Substring(lengthOfAllPreviousTokens);
                    ProcessRemainingLetters(wordToSplit, remainingLettersAfterWordRemoved, builder, splitableStrings);

                    builder.Append(remainingLettersAfterWordRemoved);
                    string splitableStringCandidate = builder.ToString();
                    if (VerifySplitableStringCandidate(splitableStringCandidate))
                    {
                        splitableStrings.Add(splitableStringCandidate);
                    }
                }
            }
        }

        internal IEnumerable<string> FindWordsAtTheStartOfThisString(string remainingLetters)
        {
            List<string> prefixWords = new List<string>();
            for (int lengthOfWord = 1; lengthOfWord < remainingLetters.Length; lengthOfWord++)
            {
                string wordCandidate = remainingLetters.Substring(0, lengthOfWord);
                if (Repository.IsAWord(wordCandidate))
                {
                    prefixWords.Add(wordCandidate);
                }
            }
            return prefixWords;
        }
    }
}