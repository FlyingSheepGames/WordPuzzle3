using System;
using System.Collections.Generic;
using System.Text;

namespace WordPuzzles
{
    public class Anacrostic
    {
        public Puzzle Puzzle = new Puzzle();
        public WordRepository Repository = new WordRepository();
        private readonly int[] _remainingLetters = new int[26];

        public List<string> WordsFoundSoFar = new List<string>();
        private string _originalPhrase;
        public string EncodedPhrase;
        public Anacrostic(string phrase)
        {
            SortLettersByReverseFrequency(phrase);
        }

        public string EncodedPhraseForGoogle;

        public string GetEncodedPhraseForGoogle()
        {
            PreparePuzzle();
            StringBuilder phraseEncodedForGoogle = new StringBuilder();
            for (var index = 0; index < Puzzle.Phrase.Count; index++)
            {
                var letter = Puzzle.Phrase[index];
                phraseEncodedForGoogle.Append(letter);
                if (index % LineLength == LineLength-1)
                {
                    phraseEncodedForGoogle.Append("\r\n");
                }
                else
                {
                    phraseEncodedForGoogle.Append("\t");
                }
            }
            Console.WriteLine(phraseEncodedForGoogle.ToString());
            Console.WriteLine(EncodedPhraseForGoogle);
            return phraseEncodedForGoogle.ToString();
        }
        public int LineLength { get; set; }

        private void SortLettersByReverseFrequency(string phrase)
        {
            if (56 < phrase.Length)
            {
                //throw new Exception("Phrases of length 57 or more are not supported (line length doesn't fit in the 8 - 14 range without 3 or more black spaces in the final row).");
            }
            _originalPhrase = phrase.ToLower();
            foreach (string word in phrase.Split(new [] { " "}, StringSplitOptions.RemoveEmptyEntries))
            {
                IgnoreWord(word);
            }
            EncodedPhrase = phrase.ToLower();
            LineLength = CalculateLineLength(phrase.Length);

            StringBuilder encodedPhraseForGoogle = new StringBuilder();
            int currentCount = 0;
            foreach (char letter in phrase)
            {
                encodedPhraseForGoogle.Append(letter);
                currentCount++;
                if (currentCount % LineLength == 0)
                {
                    encodedPhraseForGoogle.Append("\r\n");
                }
                else
                {
                    encodedPhraseForGoogle.Append("\t");
                }
            }

            EncodedPhraseForGoogle = encodedPhraseForGoogle.ToString();

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

            int lineLengthThatGeneratesTwoBlackSpaces = 0;
            for (int lineLengthToConsider = 14; 7 < lineLengthToConsider; lineLengthToConsider--)
            {
                if (phraseLength % lineLengthToConsider == 0) return lineLengthToConsider;
                if (phraseLength % lineLengthToConsider == (lineLengthToConsider - 1)) return lineLengthToConsider;
                if (phraseLength % lineLengthToConsider == (lineLengthToConsider - 2)) return lineLengthToConsider;
            }

            if (lineLengthThatGeneratesTwoBlackSpaces == 0)
            {
                //throw new Exception("Was expecting line length to be set by now.");
                //default to 10. 
                lineLengthThatGeneratesTwoBlackSpaces = 10;
            }

            return lineLengthThatGeneratesTwoBlackSpaces;
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
            StringBuilder updatedEncodedPhraseforGoogle = new StringBuilder();

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

            alreadyReplaced = false;
            foreach (char letter in EncodedPhraseForGoogle)
            {
                if (letter == letterInWordCandidate)
                {
                    if (alreadyReplaced)
                    {
                        updatedEncodedPhraseforGoogle.Append(letter);
                    }
                    else
                    {
                        updatedEncodedPhraseforGoogle.Append(letterCode);
                        alreadyReplaced = true;
                    }
                }
                else
                {
                    updatedEncodedPhraseforGoogle.Append(letter);
                }
            }

            EncodedPhrase = updatedEncodedPhrase.ToString();
            EncodedPhraseForGoogle = updatedEncodedPhraseforGoogle.ToString();
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
                Puzzle.PhraseAsString = _originalPhrase;
                Puzzle.PlaceLetters();
            }
        }

        public void IgnoreWord(string wordToIgnore)
        {
            if (_ignoredWords.Contains(wordToIgnore)) return;
            _ignoredWords.Add(wordToIgnore);

        }

        public string GetFormattedHtmlForGoogle()
        {
            PreparePuzzle();
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("<html>");
            builder.AppendLine("<body>");
            builder.AppendLine("<!--StartFragment-->");

            builder.AppendLine("Fill in the blanks below based on the clues. ");

            AppendCluesTable(builder);


            builder.AppendLine("Then copy the letters to the grid below, using the numbers as a guide. ");
            AppendEncodedMessageTable(builder);


            builder.AppendLine("<!--EndFragment-->");
            builder.AppendLine("</body>");
            builder.AppendLine("</html>");
            return builder.ToString();
        }

        private void AppendEncodedMessageTable(StringBuilder builder)
        {
            builder.AppendLine(@"<table border=""1"">");
            StringBuilder topLine = new StringBuilder();
            topLine.AppendLine("<tr>");
            StringBuilder bottomLine = new StringBuilder();
            bottomLine.AppendLine("<tr>");

            string[] enumeratedCellValues = EncodedPhraseForGoogle.Split('\t');
            foreach(string cellValue in enumeratedCellValues)
            {
                if (cellValue.Contains("\r\n"))
                {
                    string[] splitTokens = cellValue.Split(new []{"\r\n"}, StringSplitOptions.None);
                    string lastCellValueInThisRow = splitTokens[0];
                    string firstCellValueInNextRow = splitTokens[1];
                    ProcessCellValue(topLine, bottomLine, lastCellValueInThisRow);

                    ProcessLineReturn(topLine, builder);
                    ProcessLineReturn(bottomLine, builder);

                    if (!string.IsNullOrWhiteSpace(firstCellValueInNextRow))
                    {
                        ProcessCellValue(topLine, bottomLine, firstCellValueInNextRow);
                    }
                }
                else
                {
                    ProcessCellValue(topLine, bottomLine, cellValue);
                }
            }

            if (topLine.ToString() != "<tr>\r\n")//There's something left in the last line to be closed off.
            {
                topLine.AppendLine("</tr>");
                bottomLine.AppendLine("</tr>");

                builder.Append(topLine);
                builder.Append(bottomLine);
            }

            builder.AppendLine(@"</table>");

        }

        private void ProcessLineReturn(StringBuilder line, StringBuilder builder)
        {
            line.AppendLine("</tr>");

            builder.Append(line);
            line.Clear();
            line.AppendLine("<tr>");
        }

        private static void ProcessCellValue(StringBuilder topLine, StringBuilder bottomLine, string cellValue)
        {
            topLine.AppendLine("    <td> </td>");
            bottomLine.AppendLine($"    <td>{cellValue}</td>");
        }

        private void AppendCluesTable(StringBuilder builder)
        {
            builder.AppendLine(@"<table border=""1"">");
            StringBuilder topLine = new StringBuilder();
            topLine.AppendLine("<tr>");
            StringBuilder middleLine = new StringBuilder();
            middleLine.AppendLine("<tr>");
            StringBuilder bottomLine = new StringBuilder();
            bottomLine.AppendLine("<tr>");

            char currentLetter = 'A';
            int lettersAssignedSoFar = 1;
            foreach (string word in Puzzle.Clues)
            {
                topLine.AppendLine($@"    <td colspan=""{word.Length}"">Clue for {word}</td>");
                foreach (char letter in word.ToUpper())
                {
                    middleLine.AppendLine($"    <td>{letter}</td>");
                    bottomLine.AppendLine($"    <td>{currentLetter}{lettersAssignedSoFar++}</td>");
                }

                currentLetter++;

                if (currentLetter % 2 == 1)
                {
                    topLine.AppendLine("</tr>");
                    middleLine.AppendLine("</tr>");
                    bottomLine.AppendLine("</tr>");

                    builder.Append(topLine);
                    builder.Append(middleLine);
                    builder.Append(bottomLine);

                    topLine.Clear();
                    topLine.AppendLine("<tr>");
                    middleLine.Clear();
                    middleLine.AppendLine("<tr>");
                    bottomLine.Clear();
                    bottomLine.AppendLine("<tr>");
                }
                else
                {
                    topLine.AppendLine(@"    <td> </td>"); //space between words.
                    middleLine.AppendLine(@"    <td> </td>"); //space between words.
                    bottomLine.AppendLine(@"    <td> </td>"); //space between words.
                }
            }

            if (currentLetter % 2 == 0)
            {
                topLine.AppendLine("</tr>");
                middleLine.AppendLine("</tr>");
                bottomLine.AppendLine("</tr>");
                builder.Append(topLine);
                builder.Append(middleLine);
                builder.Append(bottomLine);
            }
            builder.AppendLine("</table>");
        }
    }
}