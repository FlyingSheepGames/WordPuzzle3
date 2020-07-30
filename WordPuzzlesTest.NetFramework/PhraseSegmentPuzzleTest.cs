using System;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.Text;
using WordPuzzles;

namespace WordPuzzlesTest
{
    [TestFixture]
    public class PhraseSegmentPuzzleTest
    {
        [TestFixture]
        public class PlacePhrase
        {
            [Test]
            public void ExampleOne_SetsExpectedProperties()
            {
                const string AUTHOR = "Chip";
                const string PHRASE = "The fat cat sat on the bat.";
                PhraseSegmentPuzzle puzzle = new PhraseSegmentPuzzle
                {
                    Phrase = PHRASE, 
                    Author = AUTHOR
                };

                puzzle.PlacePhrase();
                Assert.AreEqual(32, puzzle.CompleteLength, "Unexpected Complete Length");
                Assert.AreEqual(1, puzzle.SpacesBeforeAuthor, "Unexpected SpacesBeforeAuthor");
                Assert.AreEqual(PHRASE + " " + AUTHOR, puzzle.CompletePhrase, "Unexpected CompletePhrase");

            }

            [Test]
            public void ExampleTwo_SetsExpectedProperties()
            {
                const string AUTHOR = "Chip";
                const string PHRASE = "The fat cat sat in the bath.";
                PhraseSegmentPuzzle puzzle = new PhraseSegmentPuzzle
                {
                    Phrase = PHRASE,
                    Author = AUTHOR
                };

                puzzle.PlacePhrase();
                Assert.AreEqual(36, puzzle.CompleteLength, "Unexpected Complete Length");
                Assert.AreEqual(4, puzzle.SpacesBeforeAuthor, "Unexpected SpacesBeforeAuthor");
                Assert.AreEqual(PHRASE + "    " + AUTHOR, puzzle.CompletePhrase, "Unexpected CompletePhrase");

            }

            [Test]
            public void LongPhrase_CreatesSubPuzzles()
            {
                const string AUTHOR = "Malala Yousafzai";
                const string PHRASE = "In some parts of the world, students are going to school every day. It's their normal life. But in other part of the world, we are starving for education... it's like a precious gift. It's like a diamond.";
                PhraseSegmentPuzzle puzzle = new PhraseSegmentPuzzle
                {
                    Phrase = PHRASE,
                    Author = AUTHOR
                };
                puzzle.PlacePhrase();

                Assert.AreEqual(3, puzzle.SubPuzzles.Count);
                StringBuilder piecePhraseTogetherAgain = new StringBuilder();
                for (var index = 0; index < puzzle.SubPuzzles.Count; index++)
                {
                    var subPuzzle = puzzle.SubPuzzles[index];
                    if (index == 2)
                    {
                        Assert.AreEqual(AUTHOR, puzzle.SubPuzzles[2].Author);
                    }
                    else
                    {
                        Assert.AreEqual("", subPuzzle.Author);
                    }

                    piecePhraseTogetherAgain.Append(subPuzzle.Phrase);
                    piecePhraseTogetherAgain.Append(" ");
                }

                Assert.AreEqual(PHRASE + " ", piecePhraseTogetherAgain.ToString());

            }
        }

        [TestFixture]
        public class CalculateBlockSizes
        {
            [Test]
            public void ExampleZeroModFour_ReturnsExpectedSizes()
            {
                PhraseSegmentPuzzle puzzle = new PhraseSegmentPuzzle();
                List<int> sizes = puzzle.CalculateBlockSizes(8);
                Assert.AreEqual(2, sizes.Count, "Expected 2 integers");

                Assert.AreEqual(4, sizes[0], "Unexpected first integer");
                Assert.AreEqual(4, sizes[1], "Unexpected second integer");
            }

            [Test]
            public void ExampleOneModFour_ReturnsExpectedSizes()
            {
                PhraseSegmentPuzzle puzzle = new PhraseSegmentPuzzle();
                List<int> sizes = puzzle.CalculateBlockSizes(9);
                Assert.AreEqual(2, sizes.Count, "Expected 2 integers");

                Assert.AreEqual(4, sizes[0], "Unexpected first integer");
                Assert.AreEqual(5, sizes[1], "Unexpected second integer");
            }

            [Test]
            public void ExampleTwoModFour_ReturnsExpectedSizes()
            {
                PhraseSegmentPuzzle puzzle = new PhraseSegmentPuzzle();
                List<int> sizes = puzzle.CalculateBlockSizes(10);
                Assert.AreEqual(3, sizes.Count, "Expected 3 integers");

                Assert.AreEqual(4, sizes[0], "Unexpected first integer");
                Assert.AreEqual(3, sizes[1], "Unexpected second integer");
                Assert.AreEqual(3, sizes[2], "Unexpected third integer");
            }

            [Test]
            public void ExampleThreeModFour_ReturnsExpectedSizes()
            {
                PhraseSegmentPuzzle puzzle = new PhraseSegmentPuzzle();
                List<int> sizes = puzzle.CalculateBlockSizes(11);
                Assert.AreEqual(3, sizes.Count, "Expected 3 integers");

                Assert.AreEqual(4, sizes[0], "Unexpected first integer");
                Assert.AreEqual(4, sizes[1], "Unexpected second integer");
                Assert.AreEqual(3, sizes[2], "Unexpected third integer");
            }

            [Test]
            public void ExampleOneModFourButBigger_ReturnsExpectedSizes()
            {
                PhraseSegmentPuzzle puzzle = new PhraseSegmentPuzzle();
                List<int> sizes = puzzle.CalculateBlockSizes(13);
                Assert.AreEqual(3, sizes.Count, "Expected 3 integers");

                Assert.AreEqual(4, sizes[0], "Unexpected first integer");
                Assert.AreEqual(5, sizes[1], "Unexpected second integer");
                Assert.AreEqual(4, sizes[2], "Unexpected third integer");
            }

            [Test]
            public void GreaterThanFourteen_EqualsSumOfSizes()
            {
                PhraseSegmentPuzzle puzzle = new PhraseSegmentPuzzle();
                for (int lineLength = 14; lineLength < 101; lineLength++)
                {
                    int sum = 0;
                    foreach (int blockSize in puzzle.CalculateBlockSizes(lineLength))
                    {
                        sum += blockSize;
                    }
                    Assert.AreEqual(lineLength, sum, "Expected sum of block sizes to equal line length.");
                }
            }
        }

        [TestFixture]
        public class BreakPhraseIntoBlocks
        {

            [Test]
            public void ExampleOne_CreatesExpectedBlocks()
            {
                const string AUTHOR = "Chip";
                const string PHRASE = "The fat cat sat on the bat.";

                PhraseSegmentPuzzle puzzle = new PhraseSegmentPuzzle();

                List<Block> blocks = puzzle.BreakPhraseIntoBlocks(PHRASE + " " + AUTHOR, new List<int> {4, 4});

                Assert.AreEqual(2, blocks.Count, "Expected two blocks.");
                var firstBlock = blocks[0];

                Assert.AreEqual(4, firstBlock.Width, "Unexpected Width for firstBlock");
                Assert.AreEqual(4, firstBlock.Lines.Count, "Unexpected Number of Lines for FirstBlock");

                Assert.AreEqual("The ", firstBlock.Lines[0], "Unexpected first line in firstBlock");
                Assert.AreEqual("cat ", firstBlock.Lines[1], "Unexpected second line in firstBlock");
                Assert.AreEqual("on t", firstBlock.Lines[2], "Unexpected third line in firstBlock");
                Assert.AreEqual("at. ", firstBlock.Lines[3], "Unexpected fourth line in firstBlock");

                CollectionAssert.AreEquivalent(new[] {"THE", "CAT", "ON", "T", "AT"}, 
                    firstBlock.Fragments, "Unexpected set of Fragments");

                var secondBlock = blocks[1];

                Assert.AreEqual(4, secondBlock.Width, "Unexpected Width for secondBlock");
                Assert.AreEqual(4, secondBlock.Lines.Count, "Unexpected Number of Lines for secondBlock");

                Assert.AreEqual("fat ", secondBlock.Lines[0], "Unexpected first line in secondBlock");
                Assert.AreEqual("sat ", secondBlock.Lines[1], "Unexpected second line in secondBlock");
                Assert.AreEqual("he b", secondBlock.Lines[2], "Unexpected third line in secondBlock");
                Assert.AreEqual("Chip", secondBlock.Lines[3], "Unexpected fourth line in secondBlock");

                CollectionAssert.AreEquivalent(new[] { "FAT", "SAT", "HE", "B", "CHIP" },
                    secondBlock.Fragments, "Unexpected set of Fragments");

            }
        }

        [TestFixture]
        public class FormatHtmlForGoogle
        {

            [Test]
            [TestCase(true)]
            [TestCase(false)]
            public void ExamplePuzzle_ReturnsExpectedResult(bool includeSolution)
            {
                const string HTML_DIRECTORY = @"html\PhraseSegment\";
                const string SOURCE_DIRECTORY =
                    @"C:\Users\Chip\Source\Repos\WordPuzzle3\WordPuzzlesTest.NetFramework\html\PhraseSegment";

                const string AUTHOR = "Chip";
                const string PHRASE = "The fat cat sat on the bat.";
                PhraseSegmentPuzzle puzzle = new PhraseSegmentPuzzle
                {
                    Phrase = PHRASE,
                    Author = AUTHOR
                };
                puzzle.RandomSeed = 42;
                puzzle.PlacePhrase();

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

                string[] expectedLines = new string[] { "  " };// need to have something to be different from generated file.
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

            [Test]
            [TestCase(true)]
            [TestCase(false)]
            public void LongPuzzle_ReturnsExpectedResult(bool includeSolution)
            {
                const string HTML_DIRECTORY = @"html\PhraseSegment\";
                const string SOURCE_DIRECTORY =
                    @"C:\Users\Chip\Source\Repos\WordPuzzle3\WordPuzzlesTest.NetFramework\html\PhraseSegment";

                const string AUTHOR = "Malala Yousafzai";
                const string PHRASE = "In some parts of the world, students are going to school every day. It's their normal life. But in other part of the world, we are starving for education... it's like a precious gift. It's like a diamond.";
                PhraseSegmentPuzzle puzzle = new PhraseSegmentPuzzle
                {
                    Phrase = PHRASE,
                    Author = AUTHOR
                };
                puzzle.RandomSeed = 42;

                puzzle.PlacePhrase();

                string generatedHtml = puzzle.FormatHtmlForGoogle(includeSolution);

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

                string[] expectedLines = new string[] { "  " };// need to have something to be different from generated file.
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

        [TestFixture]
        public class BreakLongCompletePhraseIntoSubPhrases
        {
            [Test]
            public void Example_CreatesExpectedSubPhrases()
            {

                const string AUTHOR = "Malala Yousafzai";
                const string PHRASE = "In some parts of the world, students are going to school every day. It's their normal life. But in other part of the world, we are starving for education... it's like a precious gift. It's like a diamond.";
                PhraseSegmentPuzzle puzzle = new PhraseSegmentPuzzle
                {
                    Phrase = PHRASE,
                    Author = AUTHOR
                };
                List<string> subPhrases = puzzle.BreakLongPhraseIntoSubPhrases();

                Assert.AreEqual(3, subPhrases.Count);
                foreach (var subPhrase in subPhrases)
                {
                    Console.WriteLine($"{subPhrase.Length} : {subPhrase}");
                    Assert.LessOrEqual(subPhrase.Length, 100, "Expected 100 characters or less.");
                }
            }
        }

        [TestFixture]
        public class AddFragments
        {
            [Test]
            public void HandlesApostrophesCorrectly()
            {
                Block newBlock = new Block()
                {
                    Fragments =  new List<string>()
                };
                PhraseSegmentPuzzle.AddFragments(newBlock, "lot's and lot's of a'pos'tr'o'phe's.");
                foreach (string fragment in newBlock.Fragments)
                {
                    Console.WriteLine(fragment);
                    Assert.AreNotEqual("LOTS", fragment, "Did not expect LOTS as a fragment");
                }

            }
        }
    }
}