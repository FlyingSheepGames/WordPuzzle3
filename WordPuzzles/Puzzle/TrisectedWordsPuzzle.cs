using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Newtonsoft.Json;
using WordPuzzles.Utility;

namespace WordPuzzles.Puzzle
{
    public class TrisectedWordsPuzzle :IPuzzle
    {
        // ReSharper disable once UnusedMember.Global
        public bool IsTrisectedWordsPuzzle = true;
        private HtmlGenerator _htmlGenerator = new HtmlGenerator();

        [JsonIgnore]
        public WordRepository Repository = new WordRepository();
        public string Solution { get; set; }

        public List<TrisectedWord> FindWordsContainingLetters(char firstLetter, char secondLetter, char thirdLetter)
        {

            List<TrisectedWord> foundWords = new List<TrisectedWord>();

            foreach (PatternAndTemplate patternTemplate in GetPatternTemplates())
            {
                string pattern = patternTemplate.Pattern;

                pattern = pattern.Replace('1', firstLetter);
                pattern = pattern.Replace('2', secondLetter);
                pattern = pattern.Replace('3', thirdLetter);
                if (Verbose)
                {
                    Console.WriteLine(pattern);
                }
                foreach (string foundWord in Repository.WordsMatchingPattern(pattern))
                {
                    foundWords.Add(new TrisectedWord()
                    {
                        Word =  foundWord, 
                        Pattern = pattern,
                        PatternAsEnumArray = patternTemplate.PatternAsEnumArray,
                    });
                }
            }

            return foundWords;
        }

        public bool Verbose { get; set; } = false;

        private IEnumerable<PatternAndTemplate> GetPatternTemplates()
        {
            List<PatternAndTemplate> patternTemplates = new List<PatternAndTemplate>();
            
            StringBuilder patternBuilder = new StringBuilder();

            foreach (SectionSubPattern[] sectionSubPattern in GetAllSectionSubPatterns())
            {
                patternBuilder.Clear();
                for (int numberToSubstitue = 1; numberToSubstitue <= 3; numberToSubstitue++)
                {
                    switch (sectionSubPattern[numberToSubstitue-1])
                    {
                        case SectionSubPattern.FIRST_OF_TWO_LETTERS:
                            patternBuilder.Append($"{numberToSubstitue}_");
                            break;
                        case SectionSubPattern.SECOND_OF_TWO_LETTERS:
                            patternBuilder.Append($"_{numberToSubstitue}");
                            break;
                        case SectionSubPattern.FIRST_OF_THREE_LETTERS:
                            patternBuilder.Append($"{numberToSubstitue}__");
                            break;
                        case SectionSubPattern.SECOND_OF_THREE_LETTERS:
                            patternBuilder.Append($"_{numberToSubstitue}_");
                            break;
                        case SectionSubPattern.THIRD_OF_THREE_LETTERS:
                            patternBuilder.Append($"__{numberToSubstitue}");
                            break;
                    }
                }

                patternTemplates.Add( new PatternAndTemplate()
                {
                    Pattern  = patternBuilder.ToString(),
                    PatternAsEnumArray = sectionSubPattern,
                });
            }

            return patternTemplates;
        }

        private IEnumerable<SectionSubPattern[]> GetAllSectionSubPatterns()
        {
            var allSectionSubPatterns = new List<SectionSubPattern[]>();
            var sectionSubPattern = new SectionSubPattern[3];

            foreach (SectionSubPattern firstSubPattern in Enum.GetValues(typeof(SectionSubPattern)))
            foreach (SectionSubPattern secondSubPattern in Enum.GetValues(typeof(SectionSubPattern)))
            foreach (SectionSubPattern thirdSubPattern in Enum.GetValues(typeof(SectionSubPattern)))
            {
                if (!IsAcceptableCombination(firstSubPattern, secondSubPattern, thirdSubPattern)) continue;
                sectionSubPattern[0] = firstSubPattern;
                sectionSubPattern[1] = secondSubPattern;
                sectionSubPattern[2] = thirdSubPattern;
                allSectionSubPatterns.Add(sectionSubPattern.Clone() as SectionSubPattern[]);
            }

            return allSectionSubPatterns;
        }

        private bool IsAcceptableCombination(SectionSubPattern firstSubPattern, SectionSubPattern secondSubPattern, SectionSubPattern thirdSubPattern)
        {
            if (firstSubPattern == SectionSubPattern.UNDEFINED) return false;
            if (secondSubPattern == SectionSubPattern.UNDEFINED) return false;
            if (thirdSubPattern == SectionSubPattern.UNDEFINED) return false;

            int wordLength = CalculateLength(firstSubPattern) + CalculateLength(secondSubPattern) +
                             CalculateLength(thirdSubPattern);
            if (new List<int> {7, 8}.Contains(wordLength)) return true;
            return false;
        }

        private int CalculateLength(SectionSubPattern firstSubPattern)
        {
            switch (firstSubPattern)
            {
                case SectionSubPattern.FIRST_OF_TWO_LETTERS:
                case SectionSubPattern.SECOND_OF_TWO_LETTERS:
                    return 2;
                case SectionSubPattern.FIRST_OF_THREE_LETTERS:
                case SectionSubPattern.SECOND_OF_THREE_LETTERS:
                case SectionSubPattern.THIRD_OF_THREE_LETTERS:
                    return 3;
            }

            throw new InvalidEnumArgumentException();
        }

        public List<TrisectedWord> GetNextWordCandidates()
        {
            char[] nextLetters = new char[3];
            nextLetters[0] = '_';
            nextLetters[1] = '_';
            nextLetters[2] = '_';
            int lettersFoundSoFar = 0;
            int newCurrentIndexIfAllThreeAreUsed = 0;
            int newCurrentIndexIfFirstTwoAreUsed = 0;
            int newCurrentIndexIfOnlyOneIsUsed = 0;

            for (int currentIndex = CurrentIndex; currentIndex < Solution.Length; currentIndex++)
            {
                newCurrentIndexIfAllThreeAreUsed = currentIndex;
                if (lettersFoundSoFar < 2)
                {
                    newCurrentIndexIfFirstTwoAreUsed = currentIndex;
                }
                if (lettersFoundSoFar < 1)
                {
                    newCurrentIndexIfOnlyOneIsUsed = currentIndex;
                }
                char currentCharacter = Solution[currentIndex];
                if (char.IsLetter(currentCharacter))
                {
                    nextLetters[lettersFoundSoFar++] = char.ToLowerInvariant(currentCharacter);
                    if (3 <= lettersFoundSoFar) break;
                }
            }

            if (lettersFoundSoFar == 0) return null;
            //Best case - can use all three letters.
            var findWordsContainingLetters = FindWordsContainingLetters(nextLetters[0], nextLetters[1], nextLetters[2]);
            if (0 < findWordsContainingLetters.Count)
            {
                CurrentIndex = newCurrentIndexIfAllThreeAreUsed + 1;
                return findWordsContainingLetters;
            }
            //Otherwise, let's try to use two of the letters. 
            findWordsContainingLetters.AddRange(FindWordsContainingLetters(nextLetters[0], nextLetters[1], '_'));
            findWordsContainingLetters.AddRange(FindWordsContainingLetters(nextLetters[0], '_', nextLetters[1]));
            findWordsContainingLetters.AddRange(FindWordsContainingLetters('_', nextLetters[0], nextLetters[1]));
            if (0 < findWordsContainingLetters.Count)
            {
                CurrentIndex = newCurrentIndexIfFirstTwoAreUsed + 1;
                return findWordsContainingLetters;
            }
            
            //If we can't use two letters (e.g. 'z' and 'z'), just use one. 
            findWordsContainingLetters.AddRange(FindWordsContainingLetters('_', nextLetters[0], '_'));
            findWordsContainingLetters.AddRange(FindWordsContainingLetters(nextLetters[0], '_', '_'));
            findWordsContainingLetters.AddRange(FindWordsContainingLetters('_', '_', nextLetters[0]));
            if (0 < findWordsContainingLetters.Count)
            {
                CurrentIndex = newCurrentIndexIfOnlyOneIsUsed + 1;
                return findWordsContainingLetters;
            }
            throw new Exception($"Can't find any words with letter {nextLetters[0]}.");
        }

        public int CurrentIndex { get; set; } = 0;

        public void AddClue(TrisectedWord wordWithClue)
        {
            if (string.IsNullOrWhiteSpace(wordWithClue.Clue))
            {
                wordWithClue.Clue = "Missing clue.";
            }
            Clues.Add(wordWithClue);
        }

        public IList<TrisectedWord> Clues { get; set; } = new List<TrisectedWord>();

        public void CalculateWordSections()
        {
            foreach (var currentWord in Clues)
            {
                int currentIndexInWord = 0;
                int wordLength = 0;
                foreach (var patternEnum in currentWord.PatternAsEnumArray)
                {
                    switch (patternEnum)
                    {
                        case SectionSubPattern.FIRST_OF_TWO_LETTERS:
                        case SectionSubPattern.SECOND_OF_TWO_LETTERS:
                            wordLength = 2;
                            break;
                        case SectionSubPattern.FIRST_OF_THREE_LETTERS:
                        case SectionSubPattern.SECOND_OF_THREE_LETTERS:
                        case SectionSubPattern.THIRD_OF_THREE_LETTERS:
                            wordLength = 3;
                            break;
                    }

                    var currentWordSection = currentWord.Word.Substring(currentIndexInWord, wordLength);
                    currentIndexInWord += wordLength;
                    WordSections.Add(currentWordSection);
                }
            }
        }

        public List<string> WordSections { get; set; } = new List<string>();
        public string FormatHtmlForGoogle(bool includeSolution = false, bool isFragment = false)
        {
            var builder = new StringBuilder();
            if (!isFragment)
            {
                _htmlGenerator.AppendHtmlHeader(builder);
            }

            AppendInstructions(builder);

            AppendMissingWords(builder, includeSolution);

            AppendFragments(builder);
            if (string.IsNullOrWhiteSpace(Solution))
            {
                Solution = "";
            }
            _htmlGenerator.AppendSolution(builder, Solution, includeSolution);

            if (!isFragment)
            {
                _htmlGenerator.AppendHtmlFooter(builder);
            }
            return builder.ToString();
        }

        private void AppendFragments(StringBuilder builder)
        {
            if (WordSections.Count == 0 ) CalculateWordSections();

            List<string> TwoLetterFragments = new List<string>();
            List<string> ThreeLetterFragments = new List<string>();
            foreach (string fragment in WordSections)
            {
                if (fragment.Length == 2)
                {
                    TwoLetterFragments.Add(fragment.ToUpperInvariant());
                    continue;
                }

                if (fragment.Length == 3)
                {
                    ThreeLetterFragments.Add(fragment.ToUpperInvariant());
                    continue;
                }
                throw new Exception($"Unexpected fragment length for {fragment}");
            }
            TwoLetterFragments.Sort();
            ThreeLetterFragments.Sort();
            builder.AppendLine(@"<H2>Fragments</H2>");
            builder.AppendLine("<p />");
            builder.AppendLine($@"Length 2: {string.Join(", ", TwoLetterFragments)}");
            builder.AppendLine("<p />");
            builder.AppendLine($@"Length 3: {string.Join(", ", ThreeLetterFragments)}");

        }

        private void AppendMissingWords(StringBuilder builder, bool includeSolution)
        {
            builder.AppendLine("<H2>Missing Words</H2>");
            builder.AppendLine("<p />");
            builder.AppendLine(@"<table border=""1"">");
            foreach (var clue in Clues)
            {
                builder.AppendLine(@"<tr>");
                builder.AppendLine($@"<td width=""175"">{clue.Clue}</td>");
                for (var index = 0; index < clue.Word.Length; index++)
                {
                    var letter = clue.Word[index];
                    string letterToDisplay = "&nbsp;";
                    if (includeSolution)
                    {
                        letterToDisplay = letter.ToString().ToUpperInvariant();
                    }

                    var classAttribute = @"grey";
                    if (clue.Pattern[index] == '_')
                    {
                        classAttribute = @"normal";
                    }
                    builder.AppendLine($@"<td width=""30"" class=""{classAttribute}"" >{letterToDisplay}</td > ");
                }

                builder.AppendLine(@"</tr>");
            }

            builder.AppendLine(@"</table>");

        }

        private void AppendInstructions(StringBuilder builder)
        {
            builder.AppendLine("<H2>Instructions</H2>");
            builder.AppendLine("<p />");
            builder.AppendLine("Construct the missing words by using the 2 or 3 three letter fragments below.");
            builder.AppendLine("Each fragment will be used once.");
            builder.AppendLine("<p />");
            builder.AppendLine("Move the letters (in order) from the shaded boxes to the solution below.");
            builder.AppendLine("<p />");
        }

        public string Description => $"Trisected Word puzzle for {Solution}.";
    }

    public class PatternAndTemplate 
    {
        public string Pattern { get; set; }
        public SectionSubPattern[] PatternAsEnumArray { get; set; }
    }

    public class TrisectedWord
    {
        public string Word { get; set; }
        public string Pattern { get; set; }
        public SectionSubPattern[] PatternAsEnumArray { get; set; }
        public string Clue { get; set; }
    }

    public enum SectionSubPattern
    {
        UNDEFINED,
        FIRST_OF_TWO_LETTERS,
        FIRST_OF_THREE_LETTERS,
        SECOND_OF_TWO_LETTERS,
        SECOND_OF_THREE_LETTERS,
        THIRD_OF_THREE_LETTERS
    }
}