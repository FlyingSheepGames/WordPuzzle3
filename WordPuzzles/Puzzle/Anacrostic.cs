﻿using System;
using System.Collections.Generic;
using System.Text;
using WordPuzzles.Utility;

namespace WordPuzzles.Puzzle
{
    public class Anacrostic : IPuzzle
    {
        // ReSharper disable once UnusedMember.Global
        public bool IsAnacrostic = true;
        public InnerAnacrosticPuzzle Puzzle = new InnerAnacrosticPuzzle();
        public WordRepository Repository = new WordRepository();
        private readonly int[] _remainingLetters = new int[26];
        internal HtmlGenerator HtmlGenerator = new HtmlGenerator();

        public List<string> WordsFoundSoFar = new List<string>();
        public string OriginalPhrase;
        public string EncodedPhrase;
        public Anacrostic(string phrase)
        {
            SortLettersByReverseFrequency(phrase);
        }

        public int LineLength { get; set; }

        private void SortLettersByReverseFrequency(string phrase)
        {
            OriginalPhrase = phrase.ToLower();
            foreach (string word in phrase.Split(new [] { " "}, StringSplitOptions.RemoveEmptyEntries))
            {
                IgnoreWord(word);
            }
            EncodedPhrase = phrase.ToLower();
            LineLength = CalculateLineLength(phrase.Length);

            StringBuilder encodedPhraseForGoogle = new StringBuilder();
            int lineLengthSoFar = 0;
            int maxLineLength = 10;
            foreach (char letter in phrase)
            {
                lineLengthSoFar++;
                encodedPhraseForGoogle.Append(letter);
                if ((maxLineLength < lineLengthSoFar) && (letter == ' '))
                {
                    encodedPhraseForGoogle.Append("\r\n");
                    lineLengthSoFar = 0;
                }
                else
                {
                    encodedPhraseForGoogle.Append("\t");
                }
            }

            EncodedPhrase = encodedPhraseForGoogle.ToString();
            for (int i = 0; i < 26; i++)
            {
                _remainingLetters[i] = 0;
            }

            string lowercasePhrase = phrase.ToLower();
            foreach (char letter in lowercasePhrase)
            {
                var alphabetRank = GetAlphabetRank(letter);
                if (0 <= alphabetRank && alphabetRank < 26)
                {
                    _remainingLetters[alphabetRank]++;
                }
            }
        }

        private int CalculateLineLength(int phraseLength)
        {
            if (phraseLength < 15) return phraseLength;

            for (int lineLengthToConsider = 14; 7 < lineLengthToConsider; lineLengthToConsider--)
            {
                if (phraseLength % lineLengthToConsider == 0) return lineLengthToConsider;
                if (phraseLength % lineLengthToConsider == (lineLengthToConsider - 1)) return lineLengthToConsider;
                if (phraseLength % lineLengthToConsider == (lineLengthToConsider - 2)) return lineLengthToConsider;
            }

            return 10;
        }

        private static int GetAlphabetRank(char letter)
        {
            int alphabetRank = letter - 'a';
            return alphabetRank;
        }

        public static List<char> LettersInReverseFrequency = new List<char> {'z', 'q', 'x', 'j', 'k', 'v', 'b', 'p', 'y', 'g', 'f', 'w', 'm', 'u', 'c', 'l', 'd', 'r', 'h', 's', 'n', 'i', 'o', 'a', 't', 'e'};

        public int Remaining(char letter)
        {
            var alphabetRank = GetAlphabetRank(letter);
            if (0 <= alphabetRank && alphabetRank < 26)
            {
                return _remainingLetters[alphabetRank];
            }
            else return 0;
        }

        public string FindNextWord()
        {
            foreach (char letterToUseNext in LettersInReverseFrequency)
            {
                Dictionary<char, int> lettersReservedToUse = new Dictionary<char, int>();
                if (0 < Remaining(letterToUseNext))
                {
                    List<string> wordsStartingWith = new List<string>();
                    wordsStartingWith.AddRange(Repository.WordsStartingWith(letterToUseNext.ToString(), 6));
                    // ReSharper disable once RedundantArgumentDefaultValue
                    wordsStartingWith.AddRange(Repository.WordsStartingWith(letterToUseNext.ToString(), 5));
                    wordsStartingWith.AddRange(Repository.WordsStartingWith(letterToUseNext.ToString(), 4));
                    wordsStartingWith.AddRange(Repository.WordsStartingWith(letterToUseNext.ToString(), 3));
                    foreach (string word in wordsStartingWith)
                    {

                        for (char currentCharacter = 'a'; currentCharacter <= 'z'; currentCharacter++)
                        {
                            lettersReservedToUse[currentCharacter] = 0;
                        }

                        bool haveLettersForWord = true;
                        foreach (char letterInWordCandidate in word)
                        {
                            lettersReservedToUse[letterInWordCandidate]++;
                            if (Remaining(letterInWordCandidate) < lettersReservedToUse[letterInWordCandidate])
                            {
                                haveLettersForWord = false;
                            }
                            else
                            {
                            }
                        }

                        if (haveLettersForWord)
                        {
                            if (!WordsFoundSoFar.Contains(word)) //no repeats
                            {
                                if (!_ignoredWords.Contains(word))
                                {
                                    return word;
                                }
                            }
                        }
                    }
                }
            }

            return null;
        }

        private string _wordsWithNumberedBlanks = "";
        public string WordsWithNumberedBlanks()
        {
            HandleLeftoverLetters();

            return _wordsWithNumberedBlanks;
        }

        private void HandleLeftoverLetters()
        {
            string leftoverLetters = RemainingLetters();
            if (!string.IsNullOrWhiteSpace(leftoverLetters))
            {
                RemoveWord(leftoverLetters);
            }
        }

        public int LettersAssignedSoFar = 1;
        private readonly List<string> _ignoredWords = new List<string>();

        public void RemoveWord(string word)
        {
            WordsFoundSoFar.Add(word);

            UpdateWordsWithNumberedBlanks(word);

            foreach (char letterInWordCandidate in word)
            {
                _remainingLetters[GetAlphabetRank(letterInWordCandidate)]--;
            }
            Puzzle.AddWordToClues(word);
        }

        private void UpdateWordsWithNumberedBlanks(string word)
        {
            char wordIndexAsLetter = (char) (WordsFoundSoFar.Count + 64);
            _wordsWithNumberedBlanks += word + " ";
            foreach (char letterInWordCandidate in word)
            {
                string letterCode = wordIndexAsLetter.ToString() + LettersAssignedSoFar++;
                ReplaceFirstLetterWithLetterCodeInPhrase(letterInWordCandidate, letterCode);

                _wordsWithNumberedBlanks += letterCode + " ";
            }

            _wordsWithNumberedBlanks += Environment.NewLine;
        }

        private void ReplaceFirstLetterWithLetterCodeInPhrase(char letterInWordCandidate, string letterCode)
        {
            StringBuilder updatedEncodedPhrase = new StringBuilder();

            bool alreadyReplaced = false;

            foreach (char letter in EncodedPhrase)
            {
                if (letter == letterInWordCandidate)
                {
                    if (alreadyReplaced)
                    {
                        updatedEncodedPhrase.Append(letter);
                    }
                    else
                    {
                        updatedEncodedPhrase.Append(letterCode);
                        alreadyReplaced = true;
                    }
                }
                else
                {
                    updatedEncodedPhrase.Append(letter);
                }
            }

            EncodedPhrase = updatedEncodedPhrase.ToString();

        }

        public string RemainingLetters()
        {
            StringBuilder builder = new StringBuilder();
            foreach (char letter in LettersInReverseFrequency)
            {
                for (int numberRemaining = 0;
                    numberRemaining < _remainingLetters[GetAlphabetRank(letter)];
                    numberRemaining++)
                {
                    builder.Append(letter);
                }
            }

            return builder.ToString();
        }

        public string WordsFormattedForGoogleDocs()
        {
            int lettersAssignedSoFar = 1;
            HandleLeftoverLetters();

            PreparePuzzle();

            StringBuilder formattedResult = new StringBuilder();
            char currentLetter = 'A';
            StringBuilder topLine = new StringBuilder();
            StringBuilder middleLine = new StringBuilder();
            StringBuilder bottomLine = new StringBuilder();
            foreach (string word in Puzzle.Clues)
            {
                //formattedResult.Append($"clue for {word}\t");
                topLine.Append($"clue for {word}");
                foreach (char letter in word.ToUpper())
                {
                    topLine.Append("\t");
                    middleLine.Append(letter + "\t");
                    bottomLine.Append($"{currentLetter}{lettersAssignedSoFar++}\t");
                }


                topLine.Append("\t");
                middleLine.Append("\t");
                bottomLine.Append("\t");

                currentLetter++;

                if (currentLetter % 2 == 1)
                {
                    formattedResult.AppendLine(topLine.ToString());
                    formattedResult.AppendLine(middleLine.ToString());
                    formattedResult.AppendLine(bottomLine.ToString());
                    formattedResult.AppendLine();
                    topLine.Clear();
                    middleLine.Clear();
                    bottomLine.Clear();
                }
            }

            if (0 < topLine.Length)
            {
                formattedResult.AppendLine(topLine.ToString());
                formattedResult.AppendLine(middleLine.ToString());
                formattedResult.AppendLine(bottomLine.ToString());
                formattedResult.AppendLine();
            }
            return formattedResult.ToString();
        }

        private void PreparePuzzle()
        {
            if (string.IsNullOrWhiteSpace(Puzzle.PhraseAsString))
            {
                HandleLeftoverLetters();
                Puzzle.PhraseAsString = OriginalPhrase;
                Puzzle.PlaceLetters();
            }
        }

        public void IgnoreWord(string wordToIgnore)
        {
            if (_ignoredWords.Contains(wordToIgnore)) return;
            _ignoredWords.Add(wordToIgnore);

        }

        public string FormatHtmlForGoogle(bool includeSolution = false, bool isFragment = false)
        {
            PreparePuzzle();
            StringBuilder builder = new StringBuilder();
            if (!isFragment)
            {
                HtmlGenerator.AppendHtmlHeader(builder);
            }

            builder.AppendLine("<!--StartFragment-->");

            builder.AppendLine("Fill in the blanks below based on the clues. ");

            AppendCluesTable(builder, includeSolution);


            builder.AppendLine("<p/>Then copy the letters to the grid below, using the numbers as a guide. ");
            AppendEncodedMessageTable(builder, includeSolution);


            builder.AppendLine("<!--EndFragment-->");
            if (!isFragment)
            {
                HtmlGenerator.AppendHtmlFooter(builder);
            }

            return builder.ToString();
        }

        public string Description => "Anacrostic: " + OriginalPhrase;

        private void AppendEncodedMessageTable(StringBuilder builder, bool includeSolution = false)
        {
            builder.AppendLine(@"<table border=""1"">");
            StringBuilder topLine = new StringBuilder();
            topLine.AppendLine("<tr>");
            StringBuilder middleLine = new StringBuilder();
            middleLine.AppendLine("<tr>");
            StringBuilder bottomLine = new StringBuilder();
            bottomLine.AppendLine("<tr>");


            string[] enumeratedCellValues = EnumerateCellValues();
            int phraseIndex = 0; //Can get out of sync with enumerated Cell Values.
            foreach (var cellValue in enumeratedCellValues)
            {
                char letterInSolution = ' ';

                if ( phraseIndex < OriginalPhrase.Length)
                {
                    letterInSolution = OriginalPhrase[phraseIndex];
                    phraseIndex += 1;
                }


                if (cellValue.Contains("\r\n"))
                {
                    string[] splitTokens = cellValue.Split(new[] {"\r\n"}, StringSplitOptions.None);
                    string lastCellValueInThisRow = splitTokens[0];
                    string firstCellValueInNextRow = splitTokens[1];

                    ProcessCellValue(topLine, middleLine, bottomLine, lastCellValueInThisRow, 
                        includeSolution ? letterInSolution: ' ');

                    ProcessLineReturn(topLine, builder);
                    ProcessLineReturn(middleLine, builder);
                    ProcessLineReturn(bottomLine, builder);

                    if (!string.IsNullOrWhiteSpace(firstCellValueInNextRow))
                    {
                        letterInSolution = ' ';
                        if (phraseIndex < OriginalPhrase.Length)
                        {
                            letterInSolution = OriginalPhrase[phraseIndex];
                            phraseIndex += 1;
                        }
                        ProcessCellValue(topLine, middleLine, bottomLine, firstCellValueInNextRow,
                            includeSolution ? letterInSolution : ' ');
                    }
                }
                else
                {
                    ProcessCellValue(topLine, middleLine, bottomLine, cellValue,
                        includeSolution ? letterInSolution : ' ');
                }
            }

            if (topLine.ToString() != "<tr>\r\n")//There's something left in the last line to be closed off.
            {
                topLine.AppendLine("</tr>");
                middleLine.AppendLine("</tr>");

                builder.Append(topLine);
                builder.Append(middleLine);
            }

            builder.AppendLine(@"</table>");

        }

        internal string[] EnumerateCellValues()
        {
            return EnumerateCellValuesReplacement();
        }
        internal string[] EnumerateCellValuesReplacement()
        {
            return EncodedPhrase.Split('\t');
        }
        private void ProcessLineReturn(StringBuilder line, StringBuilder builder)
        {
            line.AppendLine("</tr>");

            builder.Append(line);
            line.Clear();
            line.AppendLine("<tr>");
        }

        private static void ProcessCellValue(StringBuilder topLine, StringBuilder middleLine, StringBuilder bottomLine, string cellValue,
            char letterInSolution = ' ')
        {
            var classAttribute = @"class=""normal centered""";
            if (string.IsNullOrWhiteSpace(cellValue))
            {
                classAttribute = @"class=""hollow""";
            }

            string letterInSolutionAsString = @"&nbsp;";
            if (letterInSolution != ' ')
            {
                letterInSolutionAsString = letterInSolution.ToString().ToUpperInvariant();
            }
            topLine.AppendLine(@"    <td width=""30"" " + classAttribute + @">" + letterInSolutionAsString + @"</td>");
            middleLine.AppendLine(@"    <td width=""30"" " + classAttribute + $@">{cellValue}</td>");
            bottomLine.AppendLine(@"    <td width=""30"" " + @"class=""open""" + @">&nbsp;</td>");
        }

        private void AppendCluesTable(StringBuilder builder, bool showSolution = false)
        {
            builder.AppendLine(@"<table border=""1"">");
            StringBuilder topLine = new StringBuilder();
            topLine.AppendLine("<tr>");
            StringBuilder secondLine = new StringBuilder();
            secondLine.AppendLine("<tr>");
            StringBuilder thirdLine = new StringBuilder();
            thirdLine.AppendLine("<tr>");
            StringBuilder fourthLine = new StringBuilder();
            fourthLine.AppendLine("<tr>");

            char currentLetter = 'A';
            int lettersAssignedSoFar = 1;
            foreach (PuzzleWord wordAsClue in Puzzle.Clues)
            {
                string word = wordAsClue;
                string currentClue = $@"Clue for {word}";
                if (!string.IsNullOrWhiteSpace(wordAsClue.CustomizedClue))
                {
                    currentClue = wordAsClue.CustomizedClue;
                }
                topLine.AppendLine($@"    <td colspan=""{word.Length}"" class=""open""><br/>" + currentClue + @"</td>");
                foreach (char letter in word.ToUpper())
                {
                    secondLine.Append(@"    <td width=""30"" class=""normal centered"">");
                    if (showSolution)
                    {
                        secondLine.Append(letter);
                    }
                    else
                    {
                        secondLine.Append(@"&nbsp;");
                    }
                    secondLine.AppendLine(@"</td>");
                    thirdLine.AppendLine($@"    <td width=""30"" class=""normal centered"">{currentLetter}{lettersAssignedSoFar++}</td>");
                    fourthLine.AppendLine(@"    <td width=""30"" class=""open""></td>");
                }

                currentLetter++;

                if (currentLetter % 2 == 1)
                {
                    CloseAndAppendAllLines(builder, topLine, secondLine, thirdLine, fourthLine);

                    topLine.Clear();
                    topLine.AppendLine("<tr>");
                    secondLine.Clear();
                    secondLine.AppendLine("<tr>");
                    thirdLine.Clear();
                    thirdLine.AppendLine("<tr>");
                    fourthLine.Clear();
                    fourthLine.AppendLine("<tr>");
                }
                else
                {
                    topLine.AppendLine(@"    <td width=""30"" class=""open"">&nbsp;</td>"); 
                    secondLine.AppendLine(@"    <td width=""30"" class=""hollow"">&nbsp;</td>"); 
                    thirdLine.AppendLine(@"    <td width=""30"" class=""hollow"">&nbsp;</td>");
                }
            }

            if (currentLetter % 2 == 0)
            {
                CloseAndAppendAllLines(builder, topLine, secondLine, thirdLine, fourthLine);
            }
            builder.AppendLine("</table>");
        }

        private static void CloseAndAppendAllLines(StringBuilder builder, StringBuilder topLine, StringBuilder secondLine,
            StringBuilder thirdLine, StringBuilder fourthLine)
        {
            topLine.AppendLine("</tr>");
            secondLine.AppendLine("</tr>");
            thirdLine.AppendLine("</tr>");
            fourthLine.AppendLine("</tr>");

            builder.Append(topLine);
            builder.Append(secondLine);
            builder.Append(thirdLine);
            builder.Append(fourthLine);
        }
    }
}