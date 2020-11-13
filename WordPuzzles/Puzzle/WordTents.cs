using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mime;
using System.Text;
using WordPuzzles.Utility;

namespace WordPuzzles.Puzzle
{
    public class WordTents
    {
        private Random _randomNumberGenerator;
        private const string INSTRUCTIONS =
@"
This puzzle is part paper craft, part word puzzle. Cut out each set of letters below, and fold them in half to make tents. If done correctly, the tent will have letters on the back and on the front, and none of the letters will be upside down. 
<p>Cut out all the squares that have a picture of scissors. These blank spaces will allow you to see the letter on the tent underneath. 
<p>If you flip each tent so the correct side is in the front, and then nest the tents on top of each other in the correct order, you will reveal a four-word phrase. If you don’t do these things properly, you’ll just get nonsense. 
<p>Don’t mix tents from different puzzles. Use the puzzle # to keep them separate. 
<p>
";
        public char FakeLetterOverrideForTests { get; set; }
        public Pattern PatternOverrideForTests { get; set; }
        public List<string> Words { get; set; }
        public string TentConfiguration { get; set; }

        public class Pattern
        {
            public PatternInstruction Top { get; set; }
            public PatternInstruction Middle { get; set; }
            public PatternInstruction Bottom { get; set; }
        }

        public enum PatternInstruction
        {
            Unknown, 
            RealLetter,
            FakeLetter,
            CutSpace
        }

        public void MakeTents()
        {
            TopTent = new Tent();
            MiddleTent = new Tent();
            BottomTent = new Tent();

            for (var wordIndex = 0; wordIndex < Words.Count; wordIndex++)
            {
                string word = Words[wordIndex];
                for (var letterIndex = 0; letterIndex < word.Length; letterIndex++)
                {
                    char realLetter = word[letterIndex];
                    Pattern currentPattern = RandomlyDeterminePattern();
                    char fakeLetter = RandomlyDetermineFakeLetter();
                    if (2 <= wordIndex)
                    {
                        fakeLetter = FlipLetter(fakeLetter);
                    }
                    TopTent.ApplyPattern(wordIndex, letterIndex, realLetter, fakeLetter, currentPattern.Top);
                    MiddleTent.ApplyPattern(wordIndex, letterIndex, realLetter, fakeLetter, currentPattern.Middle);
                    BottomTent.ApplyPattern(wordIndex, letterIndex, realLetter, fakeLetter, currentPattern.Bottom);
                }
            }

            if (string.IsNullOrWhiteSpace(TentConfiguration))
            {
                TentConfiguration = GenerateRandomTentConfiguration();
            }
        }

        private string GenerateRandomTentConfiguration()
        {
            string tentConfiguration;
            int someRandomNumber = RandomNumberGenerator.Next(6);
            switch (someRandomNumber)
            {
                case 0: tentConfiguration = "bmt"; //eliminate TMB as a possibility.
                    break;
                case 1:
                    tentConfiguration = "tbm";
                    break;
                case 2:
                    tentConfiguration = "mtb";
                    break;
                case 3:
                    tentConfiguration = "mbt";
                    break;
                case 4:
                    tentConfiguration = "btm";
                    break;
                case 5:
                    tentConfiguration = "bmt";
                    break;
                default: 
                    throw new NotImplementedException("Unexpected case");
            }
            StringBuilder builder = new StringBuilder();
            foreach (var letter in tentConfiguration)
            {
                var letterToAppend = letter;
                if (0 == RandomNumberGenerator.Next(2))
                {
                    letterToAppend = char.ToUpperInvariant(letter);
                }

                builder.Append(letterToAppend);
            }

            return builder.ToString();
        }

        private char RandomlyDetermineFakeLetter()
        {
            if (FakeLetterOverrideForTests != char.MinValue) return FakeLetterOverrideForTests;
            return (char) ('a' + RandomNumberGenerator.Next(26));
        }


        private Pattern RandomlyDeterminePattern()
        {
            if (PatternOverrideForTests != null) return PatternOverrideForTests;
            int randomNumber = RandomNumberGenerator.Next(8);
            switch (randomNumber)
            {
                case 0: 
                    return new Pattern()
                    {
                        Top= PatternInstruction.RealLetter, 
                        Middle = PatternInstruction.FakeLetter, 
                        Bottom = PatternInstruction.CutSpace,
                    };
                case 1:
                    return new Pattern()
                    {
                        Top = PatternInstruction.RealLetter,
                        Middle = PatternInstruction.CutSpace,
                        Bottom = PatternInstruction.FakeLetter,
                    };
                case 2:
                    return new Pattern()
                    {
                        Top = PatternInstruction.CutSpace,
                        Middle = PatternInstruction.RealLetter,
                        Bottom = PatternInstruction.FakeLetter,
                    };
                case 3:
                    return new Pattern()
                    {
                        Top = PatternInstruction.CutSpace,
                        Middle = PatternInstruction.RealLetter,
                        Bottom = PatternInstruction.CutSpace,
                    };
                case 4:
                    return new Pattern()
                    {
                        Top = PatternInstruction.CutSpace,
                        Middle = PatternInstruction.CutSpace,
                        Bottom = PatternInstruction.RealLetter,
                    };
                case 5:
                    return new Pattern()
                    {
                        Top = PatternInstruction.CutSpace,
                        Middle = PatternInstruction.CutSpace,
                        Bottom = PatternInstruction.RealLetter,
                    };
                case 6:
                    return new Pattern()
                    {
                        Top = PatternInstruction.RealLetter,
                        Middle = PatternInstruction.CutSpace,
                        Bottom = PatternInstruction.CutSpace,
                    };
                case 7:
                    return new Pattern()
                    {
                        Top = PatternInstruction.RealLetter,
                        Middle = PatternInstruction.FakeLetter,
                        Bottom = PatternInstruction.CutSpace,
                    };
                default:
                    throw new NotImplementedException($"Unexpected random number: {randomNumber}");
            }
        }

        public Tent BottomTent { get; set; }

        public Tent MiddleTent { get; set; }

        public Tent TopTent { get; set; }
        public int RandomGeneratorSeed { get; set; }

        public Random RandomNumberGenerator
        {
            get
            {
                if (_randomNumberGenerator == null)
                {
                    _randomNumberGenerator = new Random(RandomGeneratorSeed);
                }
                return _randomNumberGenerator;
            }
        }

        public const char CUT_SYMBOL = '_';

        public class Tent
        {
            public List<string> WordsOnTent = new List<string> { "____", "____", "____", "____"};

            public Tent()
            {
                
            }
            public Tent(Tent originalTent)
            {
                WordsOnTent = new List<string>(originalTent.WordsOnTent);
            }

            public void ApplyPattern(int wordIndex, int letterIndex, char realLetter, char fakeLetter,
                PatternInstruction instruction)
            {
                switch (instruction)
                {
                    case PatternInstruction.RealLetter:
                        WordsOnTent[wordIndex] = ReplaceLetterAtIndex(WordsOnTent[wordIndex], letterIndex, realLetter);
                        break;
                    case PatternInstruction.FakeLetter:
                        WordsOnTent[wordIndex] = ReplaceLetterAtIndex(WordsOnTent[wordIndex], letterIndex, fakeLetter);
                        break;
                    case PatternInstruction.CutSpace:
                        WordsOnTent[wordIndex] = ReplaceLetterAtIndex(WordsOnTent[wordIndex], letterIndex, CUT_SYMBOL);
                        break;

                }
            }


            //Start here with a test
            internal string ReplaceLetterAtIndex(string word, int letterIndex, char letterToPlace)
            {
                StringBuilder builder = new StringBuilder();
                builder.Append(word.Substring(0, letterIndex));
                builder.Append(letterToPlace);
                builder.Append(word.Substring(letterIndex + 1));
                return builder.ToString();
            }

            public void Flip()
            {
                string originalFirstWord = new string(WordsOnTent[0].ToCharArray());
                string originalSecondWord = new string(WordsOnTent[1].ToCharArray());
                string originalThirdWord =  new string(WordsOnTent[2].ToCharArray());
                string originalFourthWord = new string(WordsOnTent[3].ToCharArray());
                WordsOnTent[0] = FlipString(originalThirdWord);
                WordsOnTent[1] = FlipString(originalFourthWord);
                WordsOnTent[2] = FlipString(originalFirstWord);
                WordsOnTent[3] = FlipString(originalSecondWord);
            }
        }

        public List<string> GetLettersToDisplay()
        {
            var lettersToDisplay = new List<string>();
            
            var firstTent = GetCloneOfTent(TentConfiguration[0]);
            if (char.IsUpper(TentConfiguration[0]))
            {
                firstTent.Flip();
            }

            var secondTent = GetCloneOfTent(TentConfiguration[1]);
            if (char.IsUpper(TentConfiguration[1]))
            {
                secondTent.Flip();
            }
            
            var thirdTent = GetCloneOfTent(TentConfiguration[2]);
            if (char.IsUpper(TentConfiguration[2]))
            {
                thirdTent.Flip();
            }

            for (int i = 3; 0 <= i; i--)
            {
                StringBuilder lineBuilder = new StringBuilder();

                AppendLettersFromTentBack(firstTent, i, lineBuilder);
                AppendLettersFromTentBack(secondTent, i, lineBuilder);
                AppendLettersFromTentBack(thirdTent, i, lineBuilder);

                lettersToDisplay.Add(lineBuilder.ToString());
            }

            for (int i = 0; i < 4; i++)
            {
                StringBuilder lineBuilder = new StringBuilder();
                AppendLettersFromTentFront(firstTent, i, lineBuilder);
                AppendLettersFromTentFront(secondTent, i, lineBuilder);
                AppendLettersFromTentFront(thirdTent, i, lineBuilder);

                lettersToDisplay.Add(lineBuilder.ToString());
            }

            return lettersToDisplay;
        }

        private static void AppendLettersFromTentFront(Tent firstTent, int i, StringBuilder lineBuilder)
        {
            var leftLetterOnFirstTent = firstTent.WordsOnTent[0][i] ;
            var rightLetterOnFirstTent = firstTent.WordsOnTent[1][i];
            lineBuilder.Append(leftLetterOnFirstTent);
            lineBuilder.Append(rightLetterOnFirstTent);
        }

        private static void AppendLettersFromTentBack(Tent tent, int i, StringBuilder lineBuilder)
        {
            var leftLetterFirstTent = tent.WordsOnTent[3][i];
            var rightLetterFirstTent = tent.WordsOnTent[2][i];
            lineBuilder.Append(leftLetterFirstTent);
            lineBuilder.Append(rightLetterFirstTent);
        }

        private static char FlipLetter(char leftLetterFirstTent)
        {
            if (char.IsUpper(leftLetterFirstTent))
            {
                return char.ToLowerInvariant(leftLetterFirstTent);
            }
            return char.ToUpperInvariant(leftLetterFirstTent);
        }

        private static string FlipString(string stringToFlip)
        {
            StringBuilder builder= new StringBuilder();
            foreach (char character in stringToFlip)
            {
                builder.Append(FlipLetter(character));
            }

            return builder.ToString();
        }

        private Tent GetCloneOfTent(char letter)
        {
            switch (letter.ToString().ToLowerInvariant())
            {
                case "t": return new Tent(TopTent);
                case "m": return new Tent(MiddleTent);
                case "b": return new Tent(BottomTent);
                default: throw new ArgumentException($"Unexpected letter {letter}");
            }
        }

        public string FormatHtmlForGoogle(bool includeSolution = false, bool isFragment = false)
        {
            StringBuilder builder = new StringBuilder();
            HtmlGenerator generator = new HtmlGenerator();
            if (!isFragment)
            {
                generator.AppendHtmlHeader(builder);
            }

            builder.AppendLine(INSTRUCTIONS);
            builder.AppendLine(
                $"<br />1. Cut out each tent. <br/>{CreateImageTagForJpg(@"C:\Users\Chip\Documents\WordPuzzle3\WordPuzzlesTest\html\WordTents\images\instructions-a.jpg", 266, 183)}");
            builder.AppendLine(
                $"<br />2. Cut out squares marked with scissors. <br/>{CreateImageTagForJpg(@"C:\Users\Chip\Documents\WordPuzzle3\WordPuzzlesTest\html\WordTents\images\instructions-b.jpg", 291, 213)}");
            builder.AppendLine(
                $"<br />3. Fold each tent at the center line. <br/>{CreateImageTagForJpg(@"C:\Users\Chip\Documents\WordPuzzle3\WordPuzzlesTest\html\WordTents\images\instructions-c.jpg", 244, 101)}");

            AppendTentTable(builder);

            if (includeSolution)
            {
                AppendSolution(builder);
            }
            if (!isFragment)
            {
                generator.AppendHtmlFooter(builder);
            }

            return builder.ToString();
        }

        private void AppendSolution(StringBuilder builder)
        {
            Solution solution = new Solution(TentConfiguration);

            builder.AppendLine($"<br/>Solution code: ({TentConfiguration})");

            builder.AppendLine(  $"<br/>First, put the tent with the {solution.BottomSymbol} symbol on the bottom, with the symbol facing       {solution.BottomFacing} you.");
            builder.AppendLine(   $"<br/>Then, put the tent with the {solution.MiddleSymbol} symbol on top of that tent, with the symbol facing {solution.MiddleFacing} you.");
            builder.AppendLine($"<br/>Finally, put the tent with the {solution.TopSymbol} symbol on top, with the symbol facing                 {solution.TopFacing} you.");
        }

        private void AppendTentTable(StringBuilder builder)
        {
            StringBuilder firstTentBuilder = new StringBuilder();
            StringBuilder secondTentBuilder = new StringBuilder();
            StringBuilder thirdTentBuilder = new StringBuilder();

            StartTentBuilder(firstTentBuilder);
            StartTentBuilder(secondTentBuilder);
            StartTentBuilder(thirdTentBuilder);

            var linesToDisplay = GetLettersToDisplay();
            for (var index = 0; index < linesToDisplay.Count; index++)
            {
                string line = linesToDisplay[index];
                if (index == 4) //middle mark 
                {
                    const string FOLD_LINE = "<tr><td><hr></td><td><hr></td><td><hr></td></tr>";
                    firstTentBuilder.AppendLine(FOLD_LINE);
                    secondTentBuilder.AppendLine(FOLD_LINE);
                    thirdTentBuilder.AppendLine(FOLD_LINE);
                }

                string middleTextForFirstTent = null;
                string middleTextForSecondTent = null;
                string middleTextForThirdTent = null;

                if (index == 0) //first line
                {
                    middleTextForFirstTent =  $"<center>{CreateImageTagForJpg(@"C:\Users\Chip\Documents\WordPuzzle3\WordPuzzlesTest\html\WordTents\images\square.jpg")  }</center>"; 
                    middleTextForSecondTent = $"<center>{CreateImageTagForJpg(@"C:\Users\Chip\Documents\WordPuzzle3\WordPuzzlesTest\html\WordTents\images\triangle.jpg")}</center>";
                    middleTextForThirdTent =  $"<center>{CreateImageTagForJpg(@"C:\Users\Chip\Documents\WordPuzzle3\WordPuzzlesTest\html\WordTents\images\circle.jpg")  }</center>";
                }

                if (index == 7) //last line
                {
                    middleTextForFirstTent = "puzzle#: <br/>" + CalculateUniqueCode();
                    middleTextForSecondTent = "puzzle#: <br/>" + CalculateUniqueCode();
                    middleTextForThirdTent = "puzzle#: <br/>" + CalculateUniqueCode();
                }
                AppendTwoLetters(firstTentBuilder, line[0], line[1], middleTextForFirstTent);
                AppendTwoLetters(secondTentBuilder, line[2], line[3], middleTextForSecondTent);
                AppendTwoLetters(thirdTentBuilder, line[4], line[5], middleTextForThirdTent);
            }

            EndTentBuilder(firstTentBuilder);
            EndTentBuilder(secondTentBuilder);
            EndTentBuilder(thirdTentBuilder);

            builder.AppendLine(firstTentBuilder.ToString());
            builder.AppendLine(secondTentBuilder.ToString());
            builder.AppendLine(thirdTentBuilder.ToString());

        }

        private long CalculateUniqueCode()
        {
            long total = 0;
            foreach (string word in Words)
            {
                foreach (char letter in word)
                {
                    total += letter;
                }
            }
            return total;
        }

        private void EndTentBuilder(StringBuilder builder)
        {
            builder.AppendLine("</table>");
        }

        private void AppendTwoLetters(StringBuilder builder, char leftLetter, char rightLetter, string middleText = "&nbsp;")
        {
            builder.AppendLine("<tr>");
            builder.AppendLine($@"   <td width=""55"">{ImageTagForLetter(leftLetter)}</td>");
            builder.AppendLine($@"   <td width=""80"">{middleText}</td>");
            builder.AppendLine($@"   <td width=""55"">{ImageTagForLetter(rightLetter)}</td>");
            builder.AppendLine("</tr>");

        }

        private string ImageTagForLetter(char letter)
        {
            string imagePath;
            if (letter == CUT_SYMBOL)
            {
                imagePath =
                    $@"C:\Users\Chip\Documents\WordPuzzle3\WordPuzzlesTest\html\WordTents\images\cut.jpg";
            }
            else
            {
                if (char.IsUpper(letter))
                {
                    imagePath =
                        $@"C:\Users\Chip\Documents\WordPuzzle3\WordPuzzlesTest\html\WordTents\images\flip-{char.ToLowerInvariant(letter)}.jpg";
                }
                else
                {
                    imagePath =
                        $@"C:\Users\Chip\Documents\WordPuzzle3\WordPuzzlesTest\html\WordTents\images\{letter}.jpg";
                }
            }

            return CreateImageTagForJpg(imagePath);

            //return $@"<img width=""50"" src=""images\{letter}.jpg"">";
        }

        private string CreateImageTagForJpg(string imagePath, int width = 50, int height = 50)
        {
            return $@"<img width=""" + width + $@""" height=""" + height+ $@""" src=""data:image/jpg;base64,{base64StringFromFile(imagePath)}"">";
        }

        private string base64StringFromFile(string imgpath)
        {
            var bytes = File.ReadAllBytes(imgpath);
            return Convert.ToBase64String(bytes);
        }
        private void StartTentBuilder(StringBuilder builder)
        {
            builder.AppendLine(@"<table border=""1"">");
        }
    }

    internal class Solution
    {
        public Solution(string tentConfiguration)
        {
            //TODO: Constructor is a lousy place for this logic. Create a separate method instead.
            InitializeSymbolBasedOnLetter(tentConfiguration[0], "Square");
            InitializeSymbolBasedOnLetter(tentConfiguration[1], "Triangle");
            InitializeSymbolBasedOnLetter(tentConfiguration[2], "Circle");
        }

        private void InitializeSymbolBasedOnLetter(char configurationLetter, string symbolName)
        {
            string symbolFacing;
            if (char.IsUpper(configurationLetter))
            {
                symbolFacing = "towards";
            }
            else
            {
                symbolFacing = "away from";
            }

            switch (char.ToLower(configurationLetter))
            {
                case 'b':
                    BottomSymbol = symbolName;
                    BottomFacing = symbolFacing;
                    break;
                case 't':
                    TopSymbol = symbolName;
                    TopFacing = symbolFacing;
                    break;
                case 'm':
                    MiddleSymbol = symbolName;
                    MiddleFacing = symbolFacing;
                    break;
                default: throw new ArgumentException("Unexpected tent configuration value");
            }
        }

        public string BottomSymbol { get; set; }
        public string MiddleSymbol { get; set; }
        public string TopSymbol { get; set; }
        public string BottomFacing { get; set; }
        public string MiddleFacing { get; set; }
        public string TopFacing { get; set; }
    }
}