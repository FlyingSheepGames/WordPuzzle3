using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using NUnit.Framework;
using WordPuzzles.Puzzle;

namespace WordPuzzlesTest.Puzzle
{
    [TestFixture]
    public class WordTentsTest
    {
        [TestFixture]
        public class GetLettersToDisplay
        {
            private List<string> InitialTentWords = new List<string>() { "aaaa", "bbbb", "CCCC", "DDDD" };
            private const string BACK_OF_CUT_SPACE= "__";
            private const string BACK_OF_REAL_LETTER = "DC";
            private const string BACK_OF_FAKE_LETTER = "FF";
            private const string FRONT_OF_REAL_LETTER = "ab";
            private const string FRONT_OF_FAKE_LETTER = "ff";
            private const string FRONT_OF_CUT_SPACE = "__";

            [Test]
            public void DefaultPattern_DefaultTentConfiguration_ReturnsExpectedLetters()
            {
                WordTents tents = new WordTents();
                tents.PatternOverrideForTests = new WordTents.Pattern()
                {
                    Top = WordTents.PatternInstruction.RealLetter,
                    Middle = WordTents.PatternInstruction.FakeLetter,
                    Bottom = WordTents.PatternInstruction.CutSpace,
                };
                tents.FakeLetterOverrideForTests = 'f';
                tents.TentConfiguration = "tmb";
                tents.Words = InitialTentWords;
                tents.MakeTents();
                var lettersToDisplay = tents.GetLettersToDisplay();
                Assert.AreEqual(8, lettersToDisplay.Count, "Expected 8 lines of text");
                for (int index = 0; index < 4; index++)
                {
                    Assert.AreEqual(BACK_OF_REAL_LETTER.ToUpperInvariant() + 
                                    BACK_OF_FAKE_LETTER.ToUpperInvariant() + 
                                    BACK_OF_CUT_SPACE.ToUpperInvariant(), 
                        lettersToDisplay[index], "One of the first four lines was incorrect.");
                }
                for (int index = 4; index < 8; index++)
                {
                    Assert.AreEqual(FRONT_OF_REAL_LETTER.ToLowerInvariant() + 
                                    FRONT_OF_FAKE_LETTER.ToLowerInvariant() + 
                                    FRONT_OF_CUT_SPACE.ToLowerInvariant(), 
                        lettersToDisplay[index], "One of the first four lines was incorrect.");
                }
            }

            //scramble the tent order
            [Test]
            public void DefaultPattern_TentConfiguration_tbm_ReturnsExpectedLetters()
            {
                WordTents tents = new WordTents();
                tents.PatternOverrideForTests = new WordTents.Pattern()
                {
                    Top = WordTents.PatternInstruction.RealLetter,
                    Middle = WordTents.PatternInstruction.FakeLetter,
                    Bottom = WordTents.PatternInstruction.CutSpace,
                };
                tents.FakeLetterOverrideForTests = 'f';
                tents.TentConfiguration = "tbm";
                tents.Words = InitialTentWords;
                tents.MakeTents();
                var lettersToDisplay = tents.GetLettersToDisplay();
                Assert.AreEqual(8, lettersToDisplay.Count, "Expected 8 lines of text");
                for (int index = 0; index < 4; index++)
                {
                    Assert.AreEqual(
                        BACK_OF_REAL_LETTER.ToUpperInvariant() +
                        BACK_OF_CUT_SPACE.ToUpperInvariant() +
                        BACK_OF_FAKE_LETTER.ToUpperInvariant() +
                        ""
                        ,
                        lettersToDisplay[index], "One of the first four lines was incorrect.");
                }

                for (int index = 4; index < 8; index++)
                {
                    Assert.AreEqual(
                        FRONT_OF_REAL_LETTER.ToLowerInvariant() +
                        FRONT_OF_CUT_SPACE.ToLowerInvariant() +
                        FRONT_OF_FAKE_LETTER.ToLowerInvariant() +
                        "",
                        lettersToDisplay[index], "One of the first four lines was incorrect.");
                }
            }

            [Test]
            public void DefaultPattern_TentConfiguration_btm_ReturnsExpectedLetters()
            {
                WordTents tents = new WordTents();
                tents.PatternOverrideForTests = new WordTents.Pattern()
                {
                    Top = WordTents.PatternInstruction.RealLetter,
                    Middle = WordTents.PatternInstruction.FakeLetter,
                    Bottom = WordTents.PatternInstruction.CutSpace,
                };
                tents.FakeLetterOverrideForTests = 'f';
                tents.TentConfiguration = "btm";
                tents.Words = InitialTentWords;
                tents.MakeTents();
                var lettersToDisplay = tents.GetLettersToDisplay();
                Assert.AreEqual(8, lettersToDisplay.Count, "Expected 8 lines of text");
                for (int index = 0; index < 4; index++)
                {
                    Assert.AreEqual(
                        BACK_OF_CUT_SPACE.ToUpperInvariant() +
                        BACK_OF_REAL_LETTER.ToUpperInvariant() +
                        BACK_OF_FAKE_LETTER.ToUpperInvariant() +
                        ""
                        ,
                        lettersToDisplay[index], "One of the first four lines was incorrect.");
                }

                for (int index = 4; index < 8; index++)
                {
                    Assert.AreEqual(
                        FRONT_OF_CUT_SPACE.ToLowerInvariant() +
                        FRONT_OF_REAL_LETTER.ToLowerInvariant() +
                        FRONT_OF_FAKE_LETTER.ToLowerInvariant() +
                        "",
                        lettersToDisplay[index], "One of the first four lines was incorrect.");
                }
            }

            [Test]
            public void DefaultPattern_TentConfiguration_bmt_ReturnsExpectedLetters()
            {
                WordTents tents = new WordTents();
                tents.PatternOverrideForTests = new WordTents.Pattern()
                {
                    Top = WordTents.PatternInstruction.RealLetter,
                    Middle = WordTents.PatternInstruction.FakeLetter,
                    Bottom = WordTents.PatternInstruction.CutSpace,
                };
                tents.FakeLetterOverrideForTests = 'f';
                tents.TentConfiguration = "bmt";
                tents.Words = InitialTentWords;
                tents.MakeTents();
                var lettersToDisplay = tents.GetLettersToDisplay();
                Assert.AreEqual(8, lettersToDisplay.Count, "Expected 8 lines of text");
                for (int index = 0; index < 4; index++)
                {
                    Assert.AreEqual(
                        BACK_OF_CUT_SPACE.ToUpperInvariant() +
                        BACK_OF_FAKE_LETTER.ToUpperInvariant() +
                        BACK_OF_REAL_LETTER.ToUpperInvariant() +
                        ""
                        ,
                        lettersToDisplay[index], "One of the first four lines was incorrect.");
                }

                for (int index = 4; index < 8; index++)
                {
                    Assert.AreEqual(
                        FRONT_OF_CUT_SPACE.ToLowerInvariant() +
                        FRONT_OF_FAKE_LETTER.ToLowerInvariant() +
                        FRONT_OF_REAL_LETTER.ToLowerInvariant() +
                        "",
                        lettersToDisplay[index], "One of the first four lines was incorrect.");
                }
            }


            [Test]
            public void DefaultPattern_TentConfiguration_mbt_ReturnsExpectedLetters()
            {
                WordTents tents = new WordTents();
                tents.PatternOverrideForTests = new WordTents.Pattern()
                {
                    Top = WordTents.PatternInstruction.RealLetter,
                    Middle = WordTents.PatternInstruction.FakeLetter,
                    Bottom = WordTents.PatternInstruction.CutSpace,
                };
                tents.FakeLetterOverrideForTests = 'f';
                tents.TentConfiguration = "mbt";
                tents.Words = InitialTentWords;
                tents.MakeTents();
                var lettersToDisplay = tents.GetLettersToDisplay();
                Assert.AreEqual(8, lettersToDisplay.Count, "Expected 8 lines of text");
                for (int index = 0; index < 4; index++)
                {
                    Assert.AreEqual(
                        BACK_OF_FAKE_LETTER.ToUpperInvariant() +
                        BACK_OF_CUT_SPACE.ToUpperInvariant() +
                        BACK_OF_REAL_LETTER.ToUpperInvariant() +
                        ""
                        ,
                        lettersToDisplay[index], "One of the first four lines was incorrect.");
                }

                for (int index = 4; index < 8; index++)
                {
                    Assert.AreEqual(
                        FRONT_OF_FAKE_LETTER.ToLowerInvariant() +
                        FRONT_OF_CUT_SPACE.ToLowerInvariant() +
                        FRONT_OF_REAL_LETTER.ToLowerInvariant() +
                        "",
                        lettersToDisplay[index], "One of the first four lines was incorrect.");
                }
            }

            [Test]
            public void DefaultPattern_TentConfiguration_mtb_ReturnsExpectedLetters()
            {
                WordTents tents = new WordTents();
                tents.PatternOverrideForTests = new WordTents.Pattern()
                {
                    Top = WordTents.PatternInstruction.RealLetter,
                    Middle = WordTents.PatternInstruction.FakeLetter,
                    Bottom = WordTents.PatternInstruction.CutSpace,
                };
                tents.FakeLetterOverrideForTests = 'f';
                tents.TentConfiguration = "mtb";
                tents.Words = InitialTentWords;
                tents.MakeTents();
                var lettersToDisplay = tents.GetLettersToDisplay();
                Assert.AreEqual(8, lettersToDisplay.Count, "Expected 8 lines of text");
                for (int index = 0; index < 4; index++)
                {
                    Assert.AreEqual(
                        BACK_OF_FAKE_LETTER.ToUpperInvariant() +
                        BACK_OF_REAL_LETTER.ToUpperInvariant() +
                        BACK_OF_CUT_SPACE.ToUpperInvariant() +
                        ""
                        ,
                        lettersToDisplay[index], "One of the first four lines was incorrect.");
                }

                for (int index = 4; index < 8; index++)
                {
                    Assert.AreEqual(
                        FRONT_OF_FAKE_LETTER.ToLowerInvariant() +
                        FRONT_OF_REAL_LETTER.ToLowerInvariant() +
                        FRONT_OF_CUT_SPACE.ToLowerInvariant() +
                        "",
                        lettersToDisplay[index], "One of the first four lines was incorrect.");
                }
            }


            [Test]
            public void DefaultPattern_TentConfiguration_Mtb_ReturnsExpectedLetters()
            {
                WordTents tents = new WordTents();
                tents.PatternOverrideForTests = new WordTents.Pattern()
                {
                    Top = WordTents.PatternInstruction.RealLetter,
                    Middle = WordTents.PatternInstruction.FakeLetter,
                    Bottom = WordTents.PatternInstruction.CutSpace,
                };
                tents.FakeLetterOverrideForTests = 'f';
                tents.TentConfiguration = "Mtb";
                tents.Words = InitialTentWords;
                tents.MakeTents();
                var lettersToDisplay = tents.GetLettersToDisplay();
                Assert.AreEqual(8, lettersToDisplay.Count, "Expected 8 lines of text");
                for (int index = 0; index < 4; index++)
                {
                    Assert.AreEqual(
                        Flip(FRONT_OF_FAKE_LETTER) +
                        BACK_OF_REAL_LETTER.ToUpperInvariant() +
                        BACK_OF_CUT_SPACE.ToUpperInvariant() +
                        ""
                        ,
                        lettersToDisplay[index], "One of the first four lines was incorrect.");
                }

                for (int index = 4; index < 8; index++)
                {
                    Assert.AreEqual(
                        Flip(BACK_OF_FAKE_LETTER) +
                        FRONT_OF_REAL_LETTER.ToLowerInvariant() +
                        FRONT_OF_CUT_SPACE.ToLowerInvariant() +
                        "",
                        lettersToDisplay[index], "One of the first four lines was incorrect.");
                }
            }

            [Test]
            public void DefaultPattern_TentConfiguration_MTB_ReturnsExpectedLetters()
            {
                WordTents tents = new WordTents();
                tents.PatternOverrideForTests = new WordTents.Pattern()
                {
                    Top = WordTents.PatternInstruction.RealLetter,
                    Middle = WordTents.PatternInstruction.FakeLetter,
                    Bottom = WordTents.PatternInstruction.CutSpace,
                };
                tents.FakeLetterOverrideForTests = 'f';
                tents.TentConfiguration = "MTB";
                tents.Words = InitialTentWords;
                tents.MakeTents();
                var lettersToDisplay = tents.GetLettersToDisplay();
                Assert.AreEqual(8, lettersToDisplay.Count, "Expected 8 lines of text");
                for (int index = 0; index < 4; index++)
                {
                    Assert.AreEqual(
                        Flip(FRONT_OF_FAKE_LETTER) +
                        Flip(FRONT_OF_REAL_LETTER) +
                        Flip(FRONT_OF_CUT_SPACE) +
                        ""
                        ,
                        lettersToDisplay[index], "One of the first four lines was incorrect.");
                }

                for (int index = 4; index < 8; index++)
                {
                    Assert.AreEqual(
                        Flip(BACK_OF_FAKE_LETTER) +
                        Flip(BACK_OF_REAL_LETTER) +
                        Flip(BACK_OF_CUT_SPACE) +
                        "",
                        lettersToDisplay[index], "One of the first four lines was incorrect.");
                }
            }

            private string Flip(string letters)
            {
                letters = new string( letters.Reverse().ToArray());
                if (char.IsLower(letters[0]))
                {
                    return letters.ToUpperInvariant();
                }

                return letters.ToLowerInvariant();
            }

            //START HERE scramble the patterns
            [Test]
            public void Pattern_RCF_DefaultTentConfiguration_ReturnsExpectedLetters()
            {
                WordTents tents = new WordTents();
                tents.PatternOverrideForTests = new WordTents.Pattern()
                {
                    Top = WordTents.PatternInstruction.RealLetter,
                    Middle = WordTents.PatternInstruction.CutSpace,
                    Bottom = WordTents.PatternInstruction.FakeLetter,
                };
                tents.FakeLetterOverrideForTests = 'f';
                tents.TentConfiguration = "tmb";
                tents.Words = InitialTentWords;
                tents.MakeTents();
                var lettersToDisplay = tents.GetLettersToDisplay();
                Assert.AreEqual(8, lettersToDisplay.Count, "Expected 8 lines of text");
                for (int index = 0; index < 4; index++)
                {
                    Assert.AreEqual(
                        BACK_OF_REAL_LETTER.ToUpperInvariant() +
                        BACK_OF_CUT_SPACE.ToUpperInvariant() +
                        BACK_OF_FAKE_LETTER.ToUpperInvariant() + 
                        "",
                        lettersToDisplay[index], "One of the first four lines was incorrect.");
                }

                for (int index = 4; index < 8; index++)
                {
                    Assert.AreEqual(
                        FRONT_OF_REAL_LETTER.ToLowerInvariant() +
                        FRONT_OF_CUT_SPACE.ToLowerInvariant() +
                        FRONT_OF_FAKE_LETTER.ToLowerInvariant() +
                        "",
                        lettersToDisplay[index], "One of the first four lines was incorrect.");
                }
            }

            [Test]
            public void Pattern_CFR_DefaultTentConfiguration_ReturnsExpectedLetters()
            {
                WordTents tents = new WordTents();
                tents.PatternOverrideForTests = new WordTents.Pattern()
                {
                    Top = WordTents.PatternInstruction.CutSpace,
                    Middle = WordTents.PatternInstruction.FakeLetter,
                    Bottom = WordTents.PatternInstruction.RealLetter,
                };
                tents.FakeLetterOverrideForTests = 'f';
                tents.TentConfiguration = "tmb";
                tents.Words = InitialTentWords;
                tents.MakeTents();
                var lettersToDisplay = tents.GetLettersToDisplay();
                Assert.AreEqual(8, lettersToDisplay.Count, "Expected 8 lines of text");
                for (int index = 0; index < 4; index++)
                {
                    Assert.AreEqual(
                        BACK_OF_CUT_SPACE.ToUpperInvariant() +
                        BACK_OF_FAKE_LETTER.ToUpperInvariant() +
                        BACK_OF_REAL_LETTER.ToUpperInvariant() +
                        "",
                        lettersToDisplay[index], "One of the first four lines was incorrect.");
                }

                for (int index = 4; index < 8; index++)
                {
                    Assert.AreEqual(
                        FRONT_OF_CUT_SPACE.ToLowerInvariant() +
                        FRONT_OF_FAKE_LETTER.ToLowerInvariant() +
                        FRONT_OF_REAL_LETTER.ToLowerInvariant() +
                        "",
                        lettersToDisplay[index], "One of the first four lines was incorrect.");
                }
            }

        }

        [TestFixture]
        public class ReplaceLetterAtIndex
        {
            [Test]
            public void SimpleExample_ReturnsExpectedResults()
            {
                WordTents.Tent tent = new WordTents.Tent();
                Assert.AreEqual("abFd", tent.ReplaceLetterAtIndex("abcd", 2, 'F'));

            }
        }
    }

    [TestFixture]
    public class WordTentTest
    {
        [TestFixture]
        public class Flip
        {
            [Test]
            public void WithCopyConstructor_LeavesOriginalObjectUnchanged()
            {
                WordTents.Tent originalTent = new WordTents.Tent();
                List<string> originalWords = new List<string>() {"abcd", "efgh", "IJKL", "MNOP"};
                List<string> flippedWords = new List<string>() { "ijkl", "mnop", "ABCD", "EFGH" };

                originalTent.WordsOnTent = originalWords;
                var flippedTent = new WordTents.Tent(originalTent);
                flippedTent.Flip();
                
                Assert.AreEqual(originalWords, originalTent.WordsOnTent, "Expected original words to be unchanged");
                Assert.AreEqual(flippedWords, flippedTent.WordsOnTent, "Expected flipped words.");

            }
        }

        [TestFixture]
        public class FormatHtmlForGoogle
        {


            [Test]
            [TestCase(true, "funk", "rock", "PUNK", "DUCK")] //Funk rock punk duck  @WhoWhatWhyCast
            [TestCase(true, "cats", "have", "DONE", "WELL")] //CATS HAVE DONE WELL  @@Rorschach_Ink
            [TestCase(true, "time", "will", "HEAL", "PAIN")] //TIME WILL HEAL PAIN  @whatseplaying
            [TestCase(true, "most", "cars", "TURN", "LEFT")] //MOST CARS TURN LEFT  @whatseplaying
            /*
Home Away from Home
Just Take Your Time
They Play With Fire
Wear Mask Over Nose

THEY SAID ZERO RATS	Jusiv_
FISH FOOD TACO TIME	Jusiv_
THIS CLUE WILL HELP	whatseplaying

             */

            public void WithSpecialCharacter_ReturnsExpectedResult(bool includeSolution, string firstWord, string secondWord, string thirdWord, string fourthWord)
            {
                const string HTML_DIRECTORY = @"html\WordTents\";
                string SOURCE_DIRECTORY = ConfigurationManager.AppSettings["SourceDirectory"] + "WordTents";

                var puzzleRandomGeneratorSeed = 42 + firstWord[0] + firstWord[1];

                var puzzle = new WordTents();

                puzzle.RandomGeneratorSeed = puzzleRandomGeneratorSeed;
                puzzle.Words = new List<string>{firstWord, secondWord, thirdWord.ToUpperInvariant(), fourthWord.ToUpperInvariant()};

                puzzle.MakeTents();

                string generatedHtml = puzzle.FormatHtmlForGoogle(includeSolution);

                var actualFileName = $"actualExample_{firstWord}.html";
                if (includeSolution)
                {
                    actualFileName = $"actualExampleWithSolution_{firstWord}.html";
                }
                File.WriteAllText(HTML_DIRECTORY + actualFileName, generatedHtml);
                var expectedFileName = $"expectedExample_{firstWord}.html";
                if (includeSolution)
                {
                    expectedFileName = $"expectedExampleWithSolution_{firstWord}.html";
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

                    if (!expectedLine.Equals(actualLine, StringComparison.InvariantCultureIgnoreCase))
                    {
                        anyLinesDifferent = true;
                        Console.WriteLine($"Expected Line {index}:{expectedLine}");
                        Console.WriteLine($"  Actual Line {index}:{actualLine}");
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
    }
}