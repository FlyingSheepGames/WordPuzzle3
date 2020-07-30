using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using WordPuzzles.Utility;

namespace WordPuzzles.Puzzle.Legacy
{
    public class WordHexagon
    {
        readonly WordRepository _repository = new WordRepository();
        public List<string> UniqueWords = new List<string>();
        public bool Verbose = true;
        public int Size;

        public WordHexagon() : this(3)
        {
        }

        public WordHexagon(int size = 3)
        {
            Size = size;
            switch (Size)
            {
                case 3:
                    Lines = new[] { "___", "____", "_____", "____", "___" };
                    break;
                case 4:
                    Lines = new[] { "____", "_____", "______", "___*___", "______", "_____", "____" };
                    break;
                default:
                    throw new ArgumentException($"Size {size} is not supported.");
            }

            NumberOfLines = Size * 2 -1;
        }

        public string[] Lines { get; set; }

        public WordHexagon(WordHexagon original)
        {
            Verbose = original.Verbose;
            foreach (string word in original.UniqueWords)
            {
                UniqueWords.Add(word);
            }
            Size = original.Size;
            NumberOfLines = original.NumberOfLines;
            Lines = new string[NumberOfLines];
            for (int index = 0; index < NumberOfLines; index++)
            {
                Lines[index] = original.Lines[index];
            }

            _repository = original._repository;
            _xmlSerializer = original._xmlSerializer;
        }

        private readonly XmlSerializer _xmlSerializer = new XmlSerializer(typeof(WordHexagon));
        public readonly int NumberOfLines;

        public void Serialize(string fileName)
        {
            using (TextWriter writer = new StreamWriter(fileName))
            {
                _xmlSerializer.Serialize(writer, this);
            }
        }

        public void Deserialize(string fileName)
        {
            using (TextReader reader = new StreamReader(fileName))
            {
                WordHexagon deserializedPuzzle = _xmlSerializer.Deserialize(reader) as WordHexagon;
                if (deserializedPuzzle != null)
                {
                    foreach (string word in deserializedPuzzle.UniqueWords)
                    {
                        UniqueWords.Add(word);
                    }
                    Lines = new string[5];
                    for (int index = 0; index < 5; index++)
                    {
                        Lines[index] = deserializedPuzzle.Lines[index];
                    }
                }
            }
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            for (int lineIndex = 0; lineIndex < (NumberOfLines); lineIndex++)
            {
                int numberOfLeadingSpaces = Math.Abs(Size - (lineIndex +1));
                builder.Append(new string (' ', numberOfLeadingSpaces));
                builder.Append(string.Join(" ", Lines[lineIndex].ToCharArray()).ToUpper());
                builder.AppendLine();
            }
            builder.AppendLine($"Unique words: {string.Join(", ", UniqueWords)}");

            return builder.ToString();
        }

        public List<string> FindDiagonalLineAtIndex(int index)
        {
            string missingWordPattern = CalculateDiagonalWordPattern(index);

            return FindUniqueWordsMatchingPattern(missingWordPattern);
        }

        internal string CalculateDiagonalWordPattern(int index)
        {
            StringBuilder missingWordPatternBuilder = new StringBuilder();
            string missingWordPattern = null;
            switch (Size)
            {
                case 3:
                    switch (index)
                    {
                        case 0:
                            missingWordPatternBuilder.Append(Lines[2][0]);
                            missingWordPatternBuilder.Append(Lines[3][0]);
                            missingWordPatternBuilder.Append(Lines[4][0]);
                            missingWordPattern = missingWordPatternBuilder.ToString();
                            break;

                        case 1:
                            missingWordPatternBuilder.Append(Lines[1][0]);
                            missingWordPatternBuilder.Append(Lines[2][1]);
                            missingWordPatternBuilder.Append(Lines[3][1]);
                            missingWordPatternBuilder.Append(Lines[4][1]);
                            missingWordPattern = missingWordPatternBuilder.ToString();
                            break;

                        case 2:
                            missingWordPatternBuilder.Append(Lines[0][0]);
                            missingWordPatternBuilder.Append(Lines[1][1]);
                            missingWordPatternBuilder.Append(Lines[2][2]);
                            missingWordPatternBuilder.Append(Lines[3][2]);
                            missingWordPatternBuilder.Append(Lines[4][2]);
                            missingWordPattern = missingWordPatternBuilder.ToString();
                            break;

                        case 3:
                            missingWordPatternBuilder.Append(Lines[0][1]);
                            missingWordPatternBuilder.Append(Lines[1][2]);
                            missingWordPatternBuilder.Append(Lines[2][3]);
                            missingWordPatternBuilder.Append(Lines[3][3]);
                            missingWordPattern = missingWordPatternBuilder.ToString();
                            break;

                        case 4:
                            missingWordPatternBuilder.Append(Lines[0][2]);
                            missingWordPatternBuilder.Append(Lines[1][3]);
                            missingWordPatternBuilder.Append(Lines[2][4]);
                            missingWordPattern = missingWordPatternBuilder.ToString();
                            break;

                        default:
                            throw new ArgumentException($"Unimplemented index {index} for size {Size}");
                    }
                    break;
                case 4:
                    switch (index)
                    {
                        case 0:
                            missingWordPatternBuilder.Append(Lines[3][0]);
                            missingWordPatternBuilder.Append(Lines[4][0]);
                            missingWordPatternBuilder.Append(Lines[5][0]);
                            missingWordPatternBuilder.Append(Lines[6][0]);
                            missingWordPattern = missingWordPatternBuilder.ToString();
                            break;

                        case 1:
                            missingWordPatternBuilder.Append(Lines[2][0]);
                            missingWordPatternBuilder.Append(Lines[3][1]);
                            missingWordPatternBuilder.Append(Lines[4][1]);
                            missingWordPatternBuilder.Append(Lines[5][1]);
                            missingWordPatternBuilder.Append(Lines[6][1]);
                            missingWordPattern = missingWordPatternBuilder.ToString();
                            break;

                        case 2:
                            missingWordPatternBuilder.Append(Lines[1][0]);
                            missingWordPatternBuilder.Append(Lines[2][1]);
                            missingWordPatternBuilder.Append(Lines[3][2]);
                            missingWordPatternBuilder.Append(Lines[4][2]);
                            missingWordPatternBuilder.Append(Lines[5][2]);
                            missingWordPatternBuilder.Append(Lines[6][2]);
                            missingWordPattern = missingWordPatternBuilder.ToString();
                            break;

                        case 3:
                            missingWordPatternBuilder.Append(Lines[0][0]);
                            missingWordPatternBuilder.Append(Lines[1][1]);
                            missingWordPatternBuilder.Append(Lines[2][2]);
                            missingWordPattern = missingWordPatternBuilder.ToString();
                            break;

                        case 4:
                            missingWordPatternBuilder.Append(Lines[4][3]);
                            missingWordPatternBuilder.Append(Lines[5][3]);
                            missingWordPatternBuilder.Append(Lines[6][3]);
                            missingWordPattern = missingWordPatternBuilder.ToString();
                            break;

                        case 5:
                            missingWordPatternBuilder.Append(Lines[0][1]);
                            missingWordPatternBuilder.Append(Lines[1][2]);
                            missingWordPatternBuilder.Append(Lines[2][3]);
                            missingWordPatternBuilder.Append(Lines[3][4]);
                            missingWordPatternBuilder.Append(Lines[4][4]);
                            missingWordPatternBuilder.Append(Lines[5][4]);
                            missingWordPattern = missingWordPatternBuilder.ToString();
                            break;

                        case 6:
                            missingWordPatternBuilder.Append(Lines[0][2]);
                            missingWordPatternBuilder.Append(Lines[1][3]);
                            missingWordPatternBuilder.Append(Lines[2][4]);
                            missingWordPatternBuilder.Append(Lines[3][5]);
                            missingWordPatternBuilder.Append(Lines[4][5]);
                            missingWordPattern = missingWordPatternBuilder.ToString();
                            break;

                        case 7:
                            missingWordPatternBuilder.Append(Lines[0][3]);
                            missingWordPatternBuilder.Append(Lines[1][4]);
                            missingWordPatternBuilder.Append(Lines[2][5]);
                            missingWordPatternBuilder.Append(Lines[3][6]);
                            missingWordPattern = missingWordPatternBuilder.ToString();
                            break;

                        default:
                            throw new ArgumentException($"Unimplemented index {index} for size {Size}");
                    }
                    break;
            }

            return missingWordPattern;
        }

        private List<string> FindUniqueWordsMatchingPattern(string missingWordPattern)
        {
            if (!string.IsNullOrWhiteSpace(missingWordPattern))
            {
                List<string> findDiagonalLineAtIndex = new List<string>();
                foreach (string word in _repository.WordsMatchingPattern(missingWordPattern))
                {
                    var wordInLowercase = word.ToLower();
                    if (UniqueWords.Contains(wordInLowercase)) continue;
                    if (UniqueWords.Contains(wordInLowercase + "s")) continue; //Exclude singular versions of existing words.
                    if (wordInLowercase.EndsWith("s"))
                    {
                        if (UniqueWords.Contains(wordInLowercase.Substring(0, wordInLowercase.Length - 1))) //exlude plural versions of existing words.
                        {
                            continue;
                        }
                    }
                    findDiagonalLineAtIndex.Add(wordInLowercase);
                }

                return findDiagonalLineAtIndex;
            }

            return new List<string>();
        }

        public bool SetDiagonalLineAtIndex(int index, string wordToPlace)
        {
            if (UniqueWords.Contains(wordToPlace.ToLower()))
            {
                throw new ArgumentException($"{wordToPlace} has already been placed", nameof(wordToPlace));
            }
            UniqueWords.Add(wordToPlace.ToLower());

            string[] originalLines = {Lines[0], Lines[1], Lines[2], Lines[3], Lines[4]};

            switch (Size)
            {
                case 3:
                    switch (index)
                    {

                        case 0:
                            Lines[2] = SetCharacterAtIndex(Lines[2], 0, wordToPlace[0]);
                            Lines[3] = SetCharacterAtIndex(Lines[3], 0, wordToPlace[1]);
                            Lines[4] = SetCharacterAtIndex(Lines[4], 0, wordToPlace[2]);
                            break;

                        case 1:
                            Lines[1] = SetCharacterAtIndex(Lines[1], 0, wordToPlace[0]);
                            Lines[2] = SetCharacterAtIndex(Lines[2], 1, wordToPlace[1]);
                            Lines[3] = SetCharacterAtIndex(Lines[3], 1, wordToPlace[2]);
                            Lines[4] = SetCharacterAtIndex(Lines[4], 1, wordToPlace[3]);
                            break;

                        case 2:
                            Lines[0] = SetCharacterAtIndex(Lines[0], 0, wordToPlace[0]);
                            Lines[1] = SetCharacterAtIndex(Lines[1], 1, wordToPlace[1]);
                            Lines[2] = SetCharacterAtIndex(Lines[2], 2, wordToPlace[2]);
                            Lines[3] = SetCharacterAtIndex(Lines[3], 2, wordToPlace[3]);
                            Lines[4] = SetCharacterAtIndex(Lines[4], 2, wordToPlace[4]);
                            break;

                        case 3:
                            Lines[0] = SetCharacterAtIndex(Lines[0], 1, wordToPlace[0]);
                            Lines[1] = SetCharacterAtIndex(Lines[1], 2, wordToPlace[1]);
                            Lines[2] = SetCharacterAtIndex(Lines[2], 3, wordToPlace[2]);
                            Lines[3] = SetCharacterAtIndex(Lines[3], 3, wordToPlace[3]);
                            break;

                        case 4:
                            Lines[0] = SetCharacterAtIndex(Lines[0], 2, wordToPlace[0]);
                            Lines[1] = SetCharacterAtIndex(Lines[1], 3, wordToPlace[1]);
                            Lines[2] = SetCharacterAtIndex(Lines[2], 4, wordToPlace[2]);
                            break;

                        default:
                            throw new ArgumentException($"Unimplemented index {index}");
                    }
                    break;
                case 4:
                    switch (index)
                    {

                        case 0:
                            Lines[3] = SetCharacterAtIndex(Lines[3], 0, wordToPlace[0]);
                            Lines[4] = SetCharacterAtIndex(Lines[4], 0, wordToPlace[1]);
                            Lines[5] = SetCharacterAtIndex(Lines[5], 0, wordToPlace[2]);
                            Lines[6] = SetCharacterAtIndex(Lines[6], 0, wordToPlace[3]);
                            break;

                        case 1:
                            Lines[2] = SetCharacterAtIndex(Lines[2], 0, wordToPlace[0]);
                            Lines[3] = SetCharacterAtIndex(Lines[3], 1, wordToPlace[1]);
                            Lines[4] = SetCharacterAtIndex(Lines[4], 1, wordToPlace[2]);
                            Lines[5] = SetCharacterAtIndex(Lines[5], 1, wordToPlace[3]);
                            Lines[6] = SetCharacterAtIndex(Lines[6], 1, wordToPlace[4]);
                            break;

                        case 2:
                            Lines[1] = SetCharacterAtIndex(Lines[1], 0, wordToPlace[0]);
                            Lines[2] = SetCharacterAtIndex(Lines[2], 1, wordToPlace[1]);
                            Lines[3] = SetCharacterAtIndex(Lines[3], 2, wordToPlace[2]);
                            Lines[4] = SetCharacterAtIndex(Lines[4], 2, wordToPlace[3]);
                            Lines[5] = SetCharacterAtIndex(Lines[5], 2, wordToPlace[4]);
                            Lines[6] = SetCharacterAtIndex(Lines[6], 2, wordToPlace[5]);
                            break;

                        case 3:
                            Lines[0] = SetCharacterAtIndex(Lines[0], 0, wordToPlace[0]);
                            Lines[1] = SetCharacterAtIndex(Lines[1], 1, wordToPlace[1]);
                            Lines[2] = SetCharacterAtIndex(Lines[2], 2, wordToPlace[2]);
                            break;

                        case 4:
                            Lines[4] = SetCharacterAtIndex(Lines[4], 3, wordToPlace[0]);
                            Lines[5] = SetCharacterAtIndex(Lines[5], 3, wordToPlace[1]);
                            Lines[6] = SetCharacterAtIndex(Lines[6], 3, wordToPlace[2]);
                            break;

                        case 5:
                            Lines[0] = SetCharacterAtIndex(Lines[0], 1, wordToPlace[0]);
                            Lines[1] = SetCharacterAtIndex(Lines[1], 2, wordToPlace[1]);
                            Lines[2] = SetCharacterAtIndex(Lines[2], 3, wordToPlace[2]);
                            Lines[3] = SetCharacterAtIndex(Lines[3], 4, wordToPlace[3]);
                            Lines[4] = SetCharacterAtIndex(Lines[4], 4, wordToPlace[4]);
                            Lines[5] = SetCharacterAtIndex(Lines[5], 4, wordToPlace[5]);
                            break;

                        case 6:
                            Lines[0] = SetCharacterAtIndex(Lines[0], 2, wordToPlace[0]);
                            Lines[1] = SetCharacterAtIndex(Lines[1], 3, wordToPlace[1]);
                            Lines[2] = SetCharacterAtIndex(Lines[2], 4, wordToPlace[2]);
                            Lines[3] = SetCharacterAtIndex(Lines[3], 5, wordToPlace[3]);
                            Lines[4] = SetCharacterAtIndex(Lines[4], 5, wordToPlace[4]);
                            break;

                        case 7:
                            Lines[0] = SetCharacterAtIndex(Lines[0], 3, wordToPlace[0]);
                            Lines[1] = SetCharacterAtIndex(Lines[1], 4, wordToPlace[1]);
                            Lines[2] = SetCharacterAtIndex(Lines[2], 5, wordToPlace[2]);
                            Lines[3] = SetCharacterAtIndex(Lines[3], 6, wordToPlace[3]);
                            break;

                        default:
                            throw new ArgumentException($"Unimplemented index {index}");
                    }
                    break;
            }

            if (!ValidateHorizontalLines())
            {
                if (Verbose)
                {
                    Console.WriteLine("Unable to validate horizontal lines. Rolling back.");
                }
                Lines[0] = originalLines[0];
                Lines[1] = originalLines[1];
                Lines[2] = originalLines[2];
                Lines[3] = originalLines[3];
                Lines[4] = originalLines[4];
                UniqueWords.Remove(wordToPlace.ToLower());
                return false;
            }

            return true;
        }

        private string SetCharacterAtIndex(string originalString, int index, char charater)
        {
            char[] array = originalString.ToCharArray();
            array[index] = charater;
            return new string(array);
        }

        public List<string> FindHorizontalLineAtIndex(int index)
        {
            string wordPattern = null;
            if (index < Lines.Length)
            {
                wordPattern = Lines[index];
            }

            if (4 == Size)
            {
                if (index == 3) //three letters before *
                {
                    if (wordPattern is null)
                    {
                        throw new Exception($"Null word pattern at index {index} .");
                    }
                    wordPattern = wordPattern.Substring(0, 3);
                }

                if (index == 4) //three letters after *
                {
                    wordPattern = Lines[3];
                    wordPattern = wordPattern.Substring(4);
                }

                if (4 < index) //Index is off by one after the two words in the center.
                {
                    wordPattern = Lines[index - 1];
                }
            }

            return FindUniqueWordsMatchingPattern(wordPattern);

        }

        public bool SetHorizontalLineAtIndex(int index, string wordToPlace)
        {
            if (UniqueWords.Contains(wordToPlace.ToLower()))
            {
                throw new ArgumentException($"{wordToPlace} has already been placed", nameof(wordToPlace));
            }
            UniqueWords.Add(wordToPlace.ToLower());
            string originalWord = null;
            if (index < Lines.Length)
            {
                originalWord = Lines[index]; //In case we need to rollback.
            }

            string originalThirdLine = Lines[3];

            if (4 == Size)
            {
                switch (index)
                {
                    case 3:
                        Lines[3] = SetCharacterAtIndex(Lines[3], 0, wordToPlace[0]);
                        Lines[3] = SetCharacterAtIndex(Lines[3], 1, wordToPlace[1]);
                        Lines[3] = SetCharacterAtIndex(Lines[3], 2, wordToPlace[2]);
                        break;
                    case 4:
                        Lines[3] = SetCharacterAtIndex(Lines[3], 4, wordToPlace[0]);
                        Lines[3] = SetCharacterAtIndex(Lines[3], 5, wordToPlace[1]);
                        Lines[3] = SetCharacterAtIndex(Lines[3], 6, wordToPlace[2]);
                        break;
                    default:
                        if (4 < index)
                        {
                            index = index - 1; //off by one to account for two words in center row.
                        }
                        originalWord = Lines[index]; //In case we need to rollback.
                        Lines[index] = wordToPlace;
                        break;
                }
            }
            else
            {
                if (index < Lines.Length)
                {
                    Lines[index] = wordToPlace;
                }
            }

            if (ValidateDiagonalLines()) return true;

            if (Verbose)
            {
                Console.WriteLine("Unable to validate diagonal lines. Rolling back.");
            }
            if (index < Lines.Length)
            {
                Lines[index] = originalWord;
            }
            Lines[3] = originalThirdLine;

            UniqueWords.Remove(wordToPlace.ToLower());
            return false;

        }

        internal bool ValidateDiagonalLines()
        {
            int numberOfIndiciesToCheck = 5;
            if (4 == Size)
            {
                numberOfIndiciesToCheck = 8;
            }
            for (int diagonalIndex = 0; diagonalIndex < numberOfIndiciesToCheck; diagonalIndex++)
            {
                string patternToCheck = CalculateDiagonalWordPattern(diagonalIndex);
                if (Verbose)
                {
                    Console.WriteLine($"Checking pattern '{patternToCheck}' for index {diagonalIndex}.");
                }

                if (!ValidatePattern(patternToCheck)) return false;
            }
            return true;
        }

        internal bool ValidateHorizontalLines()
        {
            for (int horizontalIndex = 0; horizontalIndex < NumberOfLines; horizontalIndex++)
            {
                string patternToCheck = Lines[horizontalIndex];
                if (Verbose)
                {
                    Console.WriteLine($"Checking pattern '{patternToCheck}' for index {horizontalIndex}.");
                }
                if (!ValidatePattern(patternToCheck)) return false;
            }
            return true;
        }

        private bool ValidatePattern(string patternToCheck)
        {
            if (patternToCheck.Contains("*"))
            {
                foreach (string subword in patternToCheck.Split(new [] {"*"}, StringSplitOptions.RemoveEmptyEntries))
                {
                    if (!ValidatePattern(subword))
                    {
                        return false;
                    }
                }
                return true;
            }
            switch (patternToCheck.Split(new[] {'_'}, StringSplitOptions.None).Length)
            {
                case 1:
                    if (!_repository.IsAWord(patternToCheck))
                    {
                        if (Verbose)
                        {
                            Console.WriteLine("No blanks, but not a word. Rejecting ");
                        }
                        return false;
                    }
                    if (!UniqueWords.Contains(patternToCheck.ToLower()))
                    {
                        UniqueWords.Add(patternToCheck.ToLower());
                    }
                    break;
                case 2:
                    if (0 == _repository.WordsMatchingPattern(patternToCheck).Count)
                    {
                        if (Verbose)
                        {
                            Console.WriteLine("One blank, but no words fit this pattern. Rejecting ");
                        }
                        return false;
                    }

                    break;
            }

            return true;
        }
    }
}