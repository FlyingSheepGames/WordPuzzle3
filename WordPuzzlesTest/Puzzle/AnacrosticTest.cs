using System;
using System.IO;
using System.Text;
using NUnit.Framework;
using WordPuzzles.Puzzle;
using WordPuzzles.Utility;

namespace WordPuzzlesTest.Puzzle
{
    [TestFixture]
    public class AnacrosticTest
    {
        [TestFixture]
        public class Constructor
        {
            [Test]
            public void CreatesListOfLetters()
            {
                Anacrostic anacrostic = new Anacrostic("az");
                Assert.AreEqual(1, anacrostic.Remaining('a'));
                Assert.AreEqual(1, anacrostic.Remaining('z'));
                for (char letter = 'b'; letter < 'z'; letter++)
                {
                    Assert.AreEqual(0, anacrostic.Remaining(letter));
                }
            }

            [Test]
            public void Multiples()
            {
                Anacrostic anacrostic = new Anacrostic("aaz");
                Assert.AreEqual(2, anacrostic.Remaining('a'));
                Assert.AreEqual(1, anacrostic.Remaining('z'));
                for (char letter = 'b'; letter < 'z'; letter++)
                {
                    Assert.AreEqual(0, anacrostic.Remaining(letter));
                }
            }

        }

        [TestFixture]
        public class FindNextWord
        {
            [Test]
            [Ignore("Takes more than 3 seconds.")]
            public void WHICH_FindsWord()
            {
                Anacrostic  anacrostic = new Anacrostic("whic h");
                Assert.AreEqual("which", anacrostic.FindNextWord());
            }

            [Test]
            [Ignore("Takes more than 3 seconds.")]
            public void WillNotFindWordWithRepeatedLetters_IfNotEnoughLettersRemain()
            {
                Anacrostic anacrostic = new Anacrostic("pule");
                Assert.AreEqual(null, anacrostic.FindNextWord());
            }

            [Test]
            [Ignore("Takes more than 3 seconds.")]
            public void SimpleExample_FindsKEEPS()
            {
                Anacrostic anacrostic = new Anacrostic("kbpmmurrhhstttttteeee");
                anacrostic.WordsFoundSoFar.Add("there");
                Assert.AreEqual("keeps", anacrostic.FindNextWord());
            }

            [Test]
            [Ignore("Takes more than 3 seconds.")]
            public void FourLetterExample_FindsPEEK()
            {
                Anacrostic anacrostic = new Anacrostic("keep");
                Assert.AreEqual("peek", anacrostic.FindNextWord());
            }

            [Test]
            [Ignore("Takes more than 3 seconds.")]
            public void IgnoresWords()
            {
                Anacrostic anacrostic = new Anacrostic("keep");
                anacrostic.IgnoreWord("keep");
                Assert.AreEqual("peek", anacrostic.FindNextWord());
            }

            [Test]
            [Ignore("Takes more than 3 seconds.")]
            public void FindsThreeLetterWords()
            {
                Anacrostic anacrostic = new Anacrostic("m om");
                Assert.AreEqual("mom", anacrostic.FindNextWord());
            }

            [Test]
            [Ignore("Takes more than 3 seconds.")]
            public void FindsLint()
            {
                Anacrostic anacrostic = new Anacrostic("lin t");
                Assert.AreEqual("lint", anacrostic.FindNextWord());
            }

            [Test]
            [TestCase("on")]
            [TestCase("onx")]
            [Ignore("Takes more than 3 seconds.")]
            public void NonWords_FindsNothing(string phrase)
            {
                Anacrostic anacrostic = new Anacrostic(phrase);
                Assert.IsNull(anacrostic.FindNextWord());
            }
        }

        [TestFixture]
        public class RemoveWord
        {
            [Test]
            public void WHICH_FindsWord()
            {
                Anacrostic anacrostic = new Anacrostic("which");
                anacrostic.RemoveWord("which");
                for (char letter = 'a'; letter <= 'z'; letter++)
                {
                    Assert.AreEqual(0, anacrostic.Remaining(letter));
                }
            }
        }

        [TestFixture]
        public class RemainingLetters
        {
            [Test]
            public void WHICHZ_HasZLeft()
            {
                Anacrostic anacrostic = new Anacrostic("whichz");
                anacrostic.RemoveWord("which");
                Assert.AreEqual("z", anacrostic.RemainingLetters());
            }
        }

        [TestFixture]
        public class WordsWithNumberedBlanks
        {
            [Test]
            [Ignore("Takes more than 3 seconds.")]
            public void WHICHZ_HasZLeft()
            {
                Anacrostic anacrostic = new Anacrostic("whichz");
                anacrostic.FindNextWord();
                anacrostic.RemoveWord("which");
                Assert.AreEqual("z", anacrostic.RemainingLetters());
                Assert.AreEqual(
@"which A1 A2 A3 A4 A5 
z B6 
", anacrostic.WordsWithNumberedBlanks());
            }
        }

        [TestFixture]
        public class WordsFormattedForGoogleDocs
        {
            [Test]
            [Ignore("Takes more than 3 seconds.")]
            public void WHICHZ_ReturnsExpectedResults()
            {
                Anacrostic anacrostic = new Anacrostic("whichz");
                anacrostic.FindNextWord();
                anacrostic.RemoveWord("which");
                Assert.AreEqual("z", anacrostic.RemainingLetters());
                Assert.AreEqual(
                    @"clue for which						clue for z		
W	H	I	C	H		Z		
A1	A2	A3	A4	A5		B6		

", anacrostic.WordsFormattedForGoogleDocs());
            }
            [Test]
            [Ignore("Takes more than 3 seconds.")]
            public void ThreeWords_ReturnsExpectedResults()
            {
                Anacrostic anacrostic = new Anacrostic("whichzonepretty");
                anacrostic.FindNextWord();
                anacrostic.RemoveWord("which");
                anacrostic.RemoveWord("zone");
                anacrostic.RemoveWord("pretty");
                Assert.AreEqual("", anacrostic.RemainingLetters());
                Assert.AreEqual(
                    @"clue for which						clue for zone					
W	H	I	C	H		Z	O	N	E		
A1	A2	A3	A4	A5		B6	B7	B8	B9		

clue for pretty							
P	R	E	T	T	Y		
C10	C11	C12	C13	C14	C15		

", anacrostic.WordsFormattedForGoogleDocs());
            }

        }

        [TestFixture]
        public class FormatHtmlForGoogle
        {

            [Test]
            [TestCase(true)]
            [TestCase(false)]
            public void LongerPhrase_ReturnsExpectedResult(bool includeSolution)
            {
                const string HTML_DIRECTORY = @"html\Anacrostics\";
                const string SOURCE_DIRECTORY =
                    @"C:\Users\Chip\Source\Repos\WordPuzzle3\WordPuzzlesTest\html\Anacrostics";

                Anacrostic anacrostic = new Anacrostic("this longer phrase has at least twenty characters");

                anacrostic.RemoveWord("place");
                anacrostic.RemoveWord("years");

                anacrostic.RemoveWord("great");
                anacrostic.RemoveWord("which");

                anacrostic.RemoveWord("rates");
                anacrostic.RemoveWord("later");

                anacrostic.RemoveWord("hosts");
                anacrostic.RemoveWord("hats");

                foreach (var clue in anacrostic.Puzzle.Clues)
                {
                    string currentWord;
                    StringBuilder builder = new StringBuilder();
                    foreach (var letter in clue.Letters)
                    {
                        builder.Append(letter.ActualLetter);
                    }

                    currentWord = builder.ToString();
                    switch (currentWord)
                    {
                        case "place":
                            clue.CustomizedClue = "Ace is the ___";
                            break;
                        default:
                            Console.WriteLine($"No clue yet for {currentWord}");
                            break;
                    }
                }
                Assert.AreEqual("nnt", anacrostic.RemainingLetters());
                CollectionAssert.AreEquivalent(anacrostic.EnumerateCellValues(), anacrostic.EnumerateCellValuesReplacement());
                string generatedHtml = anacrostic.FormatHtmlForGoogle(includeSolution);

                var actualFileName = "actualExample1.html";
                if (includeSolution)
                {
                    actualFileName = "actualExampleWithSolution1.html";
                }
                File.WriteAllText(HTML_DIRECTORY + actualFileName, generatedHtml);
                var expectedFileName = "expectedExample1.html";
                if (includeSolution)
                {
                    expectedFileName = "expectedExampleWithSolution1.html";
                }

                string[] expectedLines = new[] { " "};// need to have something to be different from generated file.
                if (File.Exists(HTML_DIRECTORY + expectedFileName))
                {
                    expectedLines = File.ReadAllLines(HTML_DIRECTORY + expectedFileName);
                }
                var actualLines = File.ReadAllLines(HTML_DIRECTORY + actualFileName);
                bool anyLinesDifferent = false;
                for (var index = 0; index < expectedLines.Length; index++)
                {
                    string expectedLine = expectedLines[index];
                    string actualLine = "End of file already reached.";
                    if (index >= 0 && actualLines.Length > index)
                    {
                        actualLine = actualLines[index];
                    }

                    if (expectedLine != actualLine)
                    {
                        anyLinesDifferent = true;
                        Console.WriteLine($"Expected Line {index}:{expectedLine}");
                        Console.WriteLine($"  Actual Line {index}:{expectedLine}");
                    }
                }

                if (anyLinesDifferent)
                {
                    Console.WriteLine("Updating source file. Will show up as a difference in source control.");
                    File.WriteAllLines(SOURCE_DIRECTORY + $@"\{expectedFileName}", actualLines);
                }
                Assert.IsFalse(anyLinesDifferent, "Didn't expect any lines to be different.");

            }

            [Test]
            [TestCase(true)]
            [TestCase(false)]
            public void RatchetAndClank_ReturnsExpectedResult(bool includeSolution)
            {
                const string HTML_DIRECTORY = @"html\Anacrostics\";
                const string SOURCE_DIRECTORY =
                    @"C:\Users\Chip\Source\Repos\WordPuzzle3\WordPuzzlesTest\html\Anacrostics";

                Anacrostic anacrostic = new Anacrostic("Ratchet and Clank");

                anacrostic.RemoveWord("talk");
                anacrostic.RemoveWord("ran");

                anacrostic.RemoveWord("catch");
                anacrostic.RemoveWord("end");

                foreach (var clue in anacrostic.Puzzle.Clues)
                {
                    string currentWord;
                    StringBuilder builder = new StringBuilder();
                    foreach (var letter in clue.Letters)
                    {
                        builder.Append(letter.ActualLetter);
                    }

                    currentWord = builder.ToString();
                    switch (currentWord)
                    {
                        case "talk":
                            clue.CustomizedClue = "Speak";
                            break;
                        case "ran":
                            clue.CustomizedClue = "Competed in a foot race";
                            break;
                        case "catch":
                            clue.CustomizedClue = "I'll throw the ball, and you _____ it.";
                            break;
                        case "end":
                            clue.CustomizedClue = "The last two words in most stories are 'The ___'.";
                            break;
                        default:
                            Console.WriteLine($"No clue yet for {currentWord}");
                            break;
                    }
                }
                Assert.AreEqual("", anacrostic.RemainingLetters());

                string generatedHtml = anacrostic.FormatHtmlForGoogle(includeSolution);
                CollectionAssert.AreEquivalent(anacrostic.EnumerateCellValues(), anacrostic.EnumerateCellValuesReplacement());

                var actualFileName = "actualExample2.html";
                if (includeSolution)
                {
                    actualFileName = "actualExampleWithSolution2.html";
                }
                File.WriteAllText(HTML_DIRECTORY + actualFileName, generatedHtml);
                var expectedFileName = "expectedExample2.html";
                if (includeSolution)
                {
                    expectedFileName = "expectedExampleWithSolution2.html";
                }

                string[] expectedLines = new[] { " " };// need to have something to be different from generated file.
                if (File.Exists(HTML_DIRECTORY + expectedFileName))
                {
                    expectedLines = File.ReadAllLines(HTML_DIRECTORY + expectedFileName);
                }
                var actualLines = File.ReadAllLines(HTML_DIRECTORY + actualFileName);
                bool anyLinesDifferent = false;
                for (var index = 0; index < expectedLines.Length; index++)
                {
                    string expectedLine = expectedLines[index];
                    string actualLine = "End of file already reached.";
                    if (index >= 0 && actualLines.Length > index)
                    {
                        actualLine = actualLines[index];
                    }

                    if (expectedLine != actualLine)
                    {
                        anyLinesDifferent = true;
                        Console.WriteLine($"Expected Line {index}:{expectedLine}");
                        Console.WriteLine($"  Actual Line {index}:{expectedLine}");
                    }
                }

                if (anyLinesDifferent)
                {
                    Console.WriteLine("Updating source file. Will show up as a difference in source control.");
                    File.WriteAllLines(SOURCE_DIRECTORY + $@"\{expectedFileName}", actualLines);
                }
                Assert.IsFalse(anyLinesDifferent, "Didn't expect any lines to be different.");

            }

        }

        [TestFixture]
        public class EncodedPhrase
        {
            [Test]
            [Ignore("Takes more than 3 seconds.")]
            public void WHICHZ_ReturnsExpectedResults()
            {
                Anacrostic anacrostic = new Anacrostic("whichz");
                anacrostic.FindNextWord();
                anacrostic.RemoveWord("which");
                Assert.AreEqual("z", anacrostic.RemainingLetters());
                anacrostic.WordsFormattedForGoogleDocs();
                Assert.AreEqual(
                    @"A1A2A3A4A5B6", anacrostic.EncodedPhrase);
                Assert.AreEqual(
                    "A1\tA5\tA3\tA4\tA2\tB6\t", anacrostic.GetEncodedPhraseForGoogle());

            }

            [Test]
            public void LongerPhrase_ReturnsExpectedResults()
            {
                Anacrostic anacrostic = new Anacrostic("this longer phrase has at least twenty characters");

                anacrostic.RemoveWord("place");
                anacrostic.RemoveWord("years");
                anacrostic.RemoveWord("great");
                anacrostic.RemoveWord("which");
                anacrostic.RemoveWord("rates");
                anacrostic.RemoveWord("later");
                anacrostic.RemoveWord("hosts");
                anacrostic.RemoveWord("hats");

                Assert.AreEqual("nnt", anacrostic.RemainingLetters());
                anacrostic.WordsFormattedForGoogleDocs();
                Assert.AreEqual(
@"C15	D17	D18	B10	 	A2	G32	I40	C11	A5	B9	 
A1	D20	C12	A3	E25	B7	 	G31	B8	G33	 
C14	E23	 	F26	C13	E22	G35	F28	 	G34	D16	E24	I41	H38	B6	 
A4	H36	F27	E21	H37	D19	I42	F29	F30	H39	", 
                    anacrostic.EncodedPhrase);

       

            }

        }

        [TestFixture]
        public class LineLengthProperty
        {
            [Test]
            [TestCase(24, 13)]
            [TestCase(45, 9)]
            [TestCase(63, 13)]
            [TestCase(65, 13)]
            public void SpecificPuzzleLength_SetsCorrectLineLength(int puzzleLength, int expectedLineLength)
            {
                string puzzlePhrase = new string('a', puzzleLength);
                Anacrostic anacrostic = new Anacrostic(puzzlePhrase);
                Assert.AreEqual(expectedLineLength, anacrostic.LineLength, $"Unexpected Line Length for puzzle phrase of length {puzzleLength}");
            }

            [Test]
            [Ignore("takes too long")]
            public void First56Numbers_SetsCorrectLineLength()
            {
                for (int puzzleLength = 1; puzzleLength <= 56; puzzleLength++)
                {
                    string puzzlePhrase = new string('a', puzzleLength);
                    Anacrostic anacrostic = new Anacrostic(puzzlePhrase);
                    int actualLineLength = anacrostic.LineLength;
                    if (actualLineLength == puzzleLength) continue;
                    Assert.LessOrEqual(actualLineLength, 14, "Line length should be 14 or less.");
                    Assert.LessOrEqual(8, actualLineLength, "Line length should be 9 or more.");
                }
            }
        }

        [TestFixture]
        public class WhichWordsSupportAllLetters
        {
            //There was at least word for each 5-letter in position 1
            //There was at least word for each 6-letter in position 2
            [Test]
            [Ignore("Takes more than 3 seconds.")]
            public void AllWords()
            {
                WordRepository repository = new WordRepository() {ExcludeAdvancedWords = true};
                for (int wordSize = 3; wordSize < 7; wordSize++)
                {
                    for (int index = 0; index < wordSize; index++)
                    {
                        bool atLeastOneWordForEachLetter = true;

                        foreach (char letter in Anacrostic.LettersInReverseFrequency)
                        {
                            StringBuilder builder = new StringBuilder();
                            builder.Append('_', index);
                            builder.Append(letter);
                            builder.Append('_', wordSize - (index + 1));

                            Assert.AreEqual(wordSize, builder.Length);
                            if (repository.WordsMatchingPattern(builder.ToString()).Count == 0)
                            {
                                Console.WriteLine(
                                    $"No {wordSize}-letter words match {builder} (letter in {index} position)");
                                atLeastOneWordForEachLetter = false;
                                break;
                            }
                        }

                        if (atLeastOneWordForEachLetter)
                        {
                            Console.WriteLine(
                                $"There was at least word for each {wordSize}-letter in position {index}");
                        }
                    }
                }

            }

            [Test]
            [Ignore("Takes more than 3 seconds.")]
            public void GenerateCodeForWordLengthStartingWithDictionary()
            {
                WordRepository repository = new WordRepository() { ExcludeAdvancedWords = true };
                for (char initialLetter = 'a'; initialLetter <= 'z'; initialLetter++)
                {
                    StringBuilder wordLengthsThatStartWithThisLetter = new StringBuilder();
                    for (int wordSize = 3; wordSize < 7; wordSize++)
                    {
                        StringBuilder patternBuilder = new StringBuilder();
                        patternBuilder.Append(initialLetter);
                        patternBuilder.Append('_', wordSize - 1);
                        string pattern = patternBuilder.ToString();
                        if (0 < repository.WordsMatchingPattern(pattern).Count)
                        {
                            wordLengthsThatStartWithThisLetter.Append(wordSize);
                        }
                    }
                    Console.WriteLine($"wordLengthStartingWithLetter.Add('{initialLetter}', \"{wordLengthsThatStartWithThisLetter}\");");
                }

            }
        }

    }
}