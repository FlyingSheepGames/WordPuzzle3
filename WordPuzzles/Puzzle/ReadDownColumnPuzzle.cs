using System;
using System.Collections.Generic;
using System.Text;
using WordPuzzles.Puzzle.Legacy;
using WordPuzzles.Utility;

namespace WordPuzzles.Puzzle
{
    public class ReadDownColumnPuzzle : IPuzzle
    {
        // ReSharper disable once UnusedMember.Global
        public bool IsReadDownColumnPuzzle = true;
        private string _solution;
        public int Size => 6;
        private Random _random;
        public int ZeroBasedIndexOfSolution = 2;
        private readonly HtmlGenerator _generator = new HtmlGenerator();
        private char _specialCharacter;

        public WordRepository Repository { get; } = new WordRepository() {ExcludeAdvancedWords = true};

        public int NumberOfWordsToInclude { get; set; } = 3;

        public string Solution
        {
            get => _solution;
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    _solution = value.ToLower();
                    AcceptablePatterns = CalculateAcceptablePatterns(_solution);
                }
            }
        }

        private List<string> CalculateAcceptablePatterns(string solution)
        {
            List<string> calculateAcceptablePatterns = new List<string>();
            calculateAcceptablePatterns.AddRange(CompleteListOfPatterns);
            List<string> patternsToRemove = new List<string>();
            if (solution.Contains("q"))
            {
                foreach (string pattern in calculateAcceptablePatterns)
                {
                    if (!IncludeForQ.Contains(pattern))
                    {
                        patternsToRemove.Add(pattern);
                    }
                }
            }

            if (solution.Contains("j"))
            {
                foreach (string pattern in calculateAcceptablePatterns)
                {
                    if (!IncludeForJ.Contains(pattern))
                    {
                        patternsToRemove.Add(pattern);
                    }
                }
            }

            if (solution.Contains("x"))
            {
                foreach (string pattern in calculateAcceptablePatterns)
                {
                    if (!IncludeForX.Contains(pattern))
                    {
                        patternsToRemove.Add(pattern);
                    }
                }
            }

            if (solution.Contains("z"))
            {
                foreach (string pattern in calculateAcceptablePatterns)
                {
                    if (!IncludeForZ.Contains(pattern))
                    {
                        patternsToRemove.Add(pattern);
                    }
                }
            }

            if (solution.Contains("v"))
            {
                patternsToRemove.AddRange(ExcludeForV);
            }

            if (solution.Contains("u"))
            {
                patternsToRemove.AddRange(ExcludeForU);
            }

            if (solution.Contains("f"))
            {
                patternsToRemove.AddRange(ExcludeForF);
            }

            if (solution.Contains("y"))
            {
                patternsToRemove.AddRange(ExcludeForY);
            }

            if (solution.Contains("k"))
            {
                patternsToRemove.AddRange(ExcludeForK);
            }

            foreach (string patternToRemove in patternsToRemove)
            {
                calculateAcceptablePatterns.Remove(patternToRemove);
            }
            return calculateAcceptablePatterns;
        }

        public List<string> IncludeForQ = new List<string>()
        {
    "1___",
    "1____",
	"_1___",
	//"__1__",
	"1_____",
	"_1____",
	"__1___",
	"___1__",
	"1______",
	"_1_____",
	//"__1____",
	"___1___",
	"____1__",
	"1_______", 
	"_1______",
	"__1_____",
	"___1____",
	//"_____1__",
    "1_________",
    "_1________",
    "___1______",
        };

        public List<string> IncludeForJ = new List<string>()
        {
            "1__",
            "1___",
            "1____",
            "__1__",
            "1_____",
            "__1___",
            //"___1__",
            "1______",
            //"__1____",
            "___1___",
            "1_______",
            "__1_____",
            "___1____",
            "____1___",
            //"_____1__",
            "1_________",
            "__1_______",
            "___1______",
            "______1___",

        };

        public List<string> IncludeForX = new List<string>()
        {
	"__1",
	"__1_",
	//"1____",
	"_1___",
	"__1__",
	"___1_",
	"____1",
	"_1____",
	"__1___",
	"___1__",
	"_____1",
	//"1______",
	"_1_____",
	"__1____",
	//"___1___",
	"____1__",
	"_____1_",
	"___1____",
	"____1___",
	//"_____1__",
	"1_________", 
	"__1_______",
	"___1______", 
	"______1___",
	"_______1__",
        };

        public List<string> IncludeForZ = new List<string>()
        {
	"1__",
	"__1",
	"1___",
	"__1_",
	"1____",
	"_1___",
	"__1__",
	"___1_",
	"____1",
	"1_____",
	"__1___",
	"___1__",
	"____1_",
	"_____1",
	"1______",
	"__1____",
	"___1___",
	"____1__",
	"_____1_",
	"__1_____",
	"_____1__",
	"______1_",
	"_____1____",
	//"_______1__",
        };

        public List<string> ExcludeForV = new List<string>()
        {
            "____1",
            "_____1",
        };

        public List<string> ExcludeForU = new List<string>()
        {
            "_____1",
        };

        public List<string> ExcludeForF = new List<string>()
        {
            "_1_____",
            "_1______",
        };

        public List<string> ExcludeForY = new List<string>()
        {
            "_____1_",
            "1_________",
        };

        public List<string> ExcludeForK = new List<string>()
        {
            "1_________",
        };

        public List<string> CompleteListOfPatterns = new List<string>()
        {
            "1__",
            "_1_",
            "__1",
            "1___",
            "__1_",
            "1____",
            "_1___",
            "__1__",
            "___1_",
            "____1",
            "1_____",
            "_1____",
            "__1___",
            "___1__",
            "____1_",
            "_____1",
            "1______",
            "_1_____",
            "__1____",
            "___1___",
            "____1__",
            "_____1_",
            "1_______",
            "_1______",
            "__1_____",
            "___1____",
            "____1___",
            "_____1__",
            "______1_",
            "1_________",
            "__1_______",
            "___1______",
            "____1_____",
            "_____1____",
            "______1___",
            "_______1__",
        };

        public void PopulateWords()
        {
            for (var index = 0; index < Solution.Length; index++)
            {
                var letterToPlace = Solution[index];
                if (!char.IsLetter(letterToPlace))
                {
                    continue;
                }


                var wordCandidates = GetWordCandidatesForIndex(index);

                StringBuilder selectedWordCandidates = new StringBuilder();

                for (int includedWordCount = 0; includedWordCount < NumberOfWordsToInclude; includedWordCount++)
                {
                    selectedWordCandidates.Append(wordCandidates[Random1.Next(wordCandidates.Count)]);
                    if (includedWordCount != (NumberOfWordsToInclude - 1))
                    {
                        selectedWordCandidates.Append(", ");
                    }
                }

                SetWordAtIndex(selectedWordCandidates.ToString(), index);
            }
        }

        public void SetWordAtIndex(string wordToSet, int index)
        {
            if (index >= 0 && Words.Count > index)
            {
                Words[index] = wordToSet;
            }
            else
            {
                Words.Add(wordToSet);
            }
        }

        public void SetClueAtIndex(string clueToSet, int index)
        {
            if (index >= 0 && Clues.Count > index)
            {
                Clues[index] = clueToSet;
            }
            else
            {
                Clues.Add(clueToSet);
            }
        }
        public List<string> GetWordCandidatesForIndex(int index)
        {
            var patternToMatch = CreatePatternToMatch(Solution[index]);
            var wordCandidates = GetWordCandidates(patternToMatch);
            return wordCandidates;
        }

        private string CreatePatternToMatch(char letterToPlace)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append('_', ZeroBasedIndexOfSolution);
            builder.Append(letterToPlace);
            builder.Append('_', (Size - (ZeroBasedIndexOfSolution + 1)));

            string patternToMatch = builder.ToString();
            return patternToMatch;
        }

        private List<string> GetWordCandidates(string patternToMatch)
        {
            List<string> allWordCandidates = new List<string>();

            foreach (var pattern in InsertSpecialCharacterInPattern(patternToMatch))
            {
                allWordCandidates.AddRange(Repository.WordsMatchingPattern(pattern));
            }

            if (allWordCandidates.Count == 0)// if there aren't any matches, exclude the special character.
            {
                allWordCandidates.AddRange(Repository.WordsMatchingPattern(patternToMatch)); 
            }
            return allWordCandidates;
        }

        internal List<string> InsertSpecialCharacterInPattern(string patternToMatch)
        {
            List<string> patterns = new List<string>();
            for (int i = 0; i < patternToMatch.Length; i++)
            {
                if (patternToMatch[i] != '_') continue;
                string patternWithSpecialCharacter =
                    patternToMatch.Substring(0, i) + SpecialCharacter + patternToMatch.Substring(i +1);
                patterns.Add(patternWithSpecialCharacter);
            }

            return patterns;
        }

        public string FormatHtmlForGoogle(bool includeSolution = false, bool isFragment = false)
        {
            StringBuilder builder = new StringBuilder();
            if (!isFragment)
            {
                _generator.AppendHtmlHeader(builder);
            }
            
            builder.AppendLine("<!--StartFragment-->");
            var ordinalOfColumnWithSolution = GetOrdinalOfColumnWithSolution();
            if (char.IsLetter(SpecialCharacter) && !string.IsNullOrWhiteSpace(ReasonForSpecialCharacter))
            {
                builder.AppendLine(
                    $"{ReasonForSpecialCharacter}, all {SpecialCharacter.ToString().ToUpperInvariant()}s have been revealed.");
            }
            builder.AppendLine($"Fill in the clues below, and then read the solution down the {ordinalOfColumnWithSolution} column. ");
            builder.AppendLine(@"<table border=""1"">");
            for (var index = 0; index < Words.Count; index++)
            {
                string word = Words[index];
                builder.AppendLine(@"<tr>");
                string currentClue = $@"Clue for {word}";
                if (!string.IsNullOrWhiteSpace(Clues[index]))
                {
                    currentClue = Clues[index];
                }
                builder.AppendLine(@"    <td width=""250"">" + currentClue + @"</td>");
                for (int i = 0; i < Size; i++)
                {
                    string style = "normal";
                    if (i == ZeroBasedIndexOfSolution)
                    {
                        style = "bold";
                    }

                    string letterToDisplay = "&nbsp;";
                    if (includeSolution || (char.ToUpperInvariant(SpecialCharacter) == char.ToUpperInvariant(word[i])) )
                    {
                        letterToDisplay = word[i].ToString().ToUpperInvariant();
                        style += " centered";
                    }
                    builder.AppendLine($@"    <td class=""{style}"" width=""30"">{letterToDisplay}</td>");
                }

                builder.AppendLine(@"</tr>");
            }

            builder.AppendLine("</table>");
            builder.Append(@"Solution: ");
            if (includeSolution)
            {
                builder.AppendLine($"<u>{Solution.ToUpperInvariant()}</u>");
            }
            else
            {
                foreach (char character in Solution)
                {
                    if (char.IsLetter(character))
                    {
                        builder.Append("_ ");
                        continue;
                    }

                    if (character == ' ')
                    {
                        builder.Append("&nbsp;&nbsp;&nbsp;");
                        continue;
                    }

                    builder.Append(character);
                }
            }

            builder.AppendLine();
            builder.AppendLine("<!--EndFragment-->");
            if (!isFragment)
            {
                _generator.AppendHtmlFooter(builder);
            }

            return builder.ToString();
        }

        internal string GetOrdinalOfColumnWithSolution()
        {
            if (ZeroBasedIndexOfSolution == (Size - 1))
            {
                return "last";
            }
            switch (ZeroBasedIndexOfSolution)
            {
                case 0: return "first";
                case 1: return "second";
                case 2: return "third";
                case 3: return "fourth";
                case 4: return "fifth";
                case 5: return "sixth";
            }
            throw new Exception("Unexpected ZeroBasedIndexOfSolution.");
        }


        public string Description => $"Read Down Column puzzle {Solution}";
        public WordPuzzleType Type { get; } = WordPuzzleType.ReadDownColumn;

        public List<string> GetClues()
        {
            return Clues;
        }

        public void ReplaceClue(string clueToReplace, string newClue)
        {
            for (var index = 0; index < Clues.Count; index++)
            {
                var clue = Clues[index];
                if (clue == clueToReplace)
                {
                    Clues[index] = newClue;
                }
            }
        }

        public List<string> Clues = new List<string>();

        public char SpecialCharacter
        {
            get => _specialCharacter;
            set => _specialCharacter = value.ToString().ToLowerInvariant()[0];
        }

        public int RandomSeed { get; set; } = 0;
        public Random Random1
        {
            get
            {
                if (_random == null)
                {
                    if (RandomSeed == 0)
                    {
                        _random = new Random();
                    }
                    else
                    {
                        _random = new Random(RandomSeed);
                    }
                }
                return _random;
            }
        }

        public string ReasonForSpecialCharacter { get; set; }
        public List<string> AcceptablePatterns { get; set; }

        public List<string> Words = new List<string>();
    }
}