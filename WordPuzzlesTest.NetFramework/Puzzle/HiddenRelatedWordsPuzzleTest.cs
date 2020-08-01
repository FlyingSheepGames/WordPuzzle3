using System;
using System.IO;
using NUnit.Framework;
using WordPuzzles.Puzzle;

namespace WordPuzzlesTest.Puzzle
{
    [TestFixture]
    public class HiddenRelatedWordsPuzzleTest
    {
        [TestFixture]
        public class AddWord
        {
            [Test]
            public void CalculatesCombinedIndexAndLength()
            {

                HiddenRelatedWordsPuzzle puzzle = new HiddenRelatedWordsPuzzle();
                puzzle.AddWord(new HiddenWord()
                {
                    Word = "holder",
                    KeyIndex = 3,
                    SentenceHidingWord = "You'll have to hold ermine and skunks in this job."
                });

                Assert.AreEqual(3, puzzle.CombinedKeyIndex, "Unexpected Combined Key Index");
                Assert.AreEqual(6, puzzle.CombinedLength, "Unexpected Combined Length");

                puzzle.AddWord(new HiddenWord()
                {
                    Word = "table",
                    KeyIndex = 4,
                    SentenceHidingWord = "He wasn't able to keep his foxes straight."
                });
                Assert.AreEqual(4, puzzle.CombinedKeyIndex, "Unexpected Combined Key Index");
                Assert.AreEqual(7, puzzle.CombinedLength, "Unexpected Combined Length");

                puzzle.AddWord(new HiddenWord()
                {
                    Word = "stand",
                    KeyIndex = 0,
                    SentenceHidingWord = "I lived somewhere between east and west."
                });
                Assert.AreEqual(4, puzzle.CombinedKeyIndex, "Unexpected Combined Key Index");
                Assert.AreEqual(9, puzzle.CombinedLength, "Unexpected Combined Length");

                puzzle.AddWord(new HiddenWord()
                {
                    Word = "rack",
                    KeyIndex = 3,
                    SentenceHidingWord = "You'd better acknowledge my superiority in this matter."
                });
                Assert.AreEqual(4, puzzle.CombinedKeyIndex, "Unexpected Combined Key Index");
                Assert.AreEqual(9, puzzle.CombinedLength, "Unexpected Combined Length");
            }
        }
        [TestFixture]
        public class FormatHtmlForGoogle
        {


            [Test]
            [TestCase(true)]
            [TestCase(false)]
            public void WithSpecialCharacter_ReturnsExpectedResult(bool includeSolution)
            {
                const string HTML_DIRECTORY = @"html\HiddenRelatedWords\";
                const string SOURCE_DIRECTORY =
                    @"C:\Users\Chip\Source\Repos\WordPuzzle3\WordPuzzlesTest.NetFramework\html\HiddenRelatedWords";

                HiddenRelatedWordsPuzzle puzzle = new HiddenRelatedWordsPuzzle();
                puzzle.AddWord(new HiddenWord()
                {
                    Word = "holder",
                    KeyIndex = 3,
                    SentenceHidingWord = "You'll have to hold ermine and skunks in this job."
                });

                puzzle.AddWord(new HiddenWord()
                {
                    Word = "table",
                    KeyIndex = 4,
                    SentenceHidingWord = "He wasn't able to keep his foxes straight."
                });
                
                puzzle.AddWord(new HiddenWord()
                {
                    Word = "stand",
                    KeyIndex = 0,
                    SentenceHidingWord = "I lived somewhere between east and west."
                });

                puzzle.AddWord(new HiddenWord()
                {
                    Word = "rack",
                    KeyIndex = 3,
                    SentenceHidingWord = "You'd better acknowledge my superiority in this matter."
                });

                string generatedHtml = puzzle.FormatHtmlForGoogle(includeSolution);

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

                string[] expectedLines = new string[] { " " };// need to have something to be different from generated file.
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
                    Console.WriteLine($"Updating source file. Will show up as a difference in source control.");
                    File.WriteAllLines(SOURCE_DIRECTORY + $@"\{expectedFileName}", actualLines);
                }
                Assert.IsFalse(anyLinesDifferent, "Didn't expect any lines to be different.");

            }

        }

    }

    [TestFixture]
    public class HiddenWordTest
    {
        [TestFixture]
        public class Constructor
        {
            [Test]
            public void CalculatesLettersAfterIndex()
            {
                var hiddenWord = new HiddenWord()
                {
                    Word = "holder",
                    KeyIndex = 3,
                };
                Assert.AreEqual(2, hiddenWord.LettersAfterIndex);
            }

        }
    }
}