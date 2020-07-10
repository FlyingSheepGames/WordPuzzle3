using NUnit.Framework;
using System.Collections.Generic;
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

                CollectionAssert.AreEquivalent(new[] {"The", "cat", "on", "t", "at."}, 
                    firstBlock.Fragments, "Unexpected set of Fragments");

                var secondBlock = blocks[1];

                Assert.AreEqual(4, secondBlock.Width, "Unexpected Width for secondBlock");
                Assert.AreEqual(4, secondBlock.Lines.Count, "Unexpected Number of Lines for secondBlock");

                Assert.AreEqual("fat ", secondBlock.Lines[0], "Unexpected first line in secondBlock");
                Assert.AreEqual("sat ", secondBlock.Lines[1], "Unexpected second line in secondBlock");
                Assert.AreEqual("he b", secondBlock.Lines[2], "Unexpected third line in secondBlock");
                Assert.AreEqual("Chip", secondBlock.Lines[3], "Unexpected fourth line in secondBlock");

                CollectionAssert.AreEquivalent(new[] { "fat", "sat", "he", "b", "Chip" },
                    secondBlock.Fragments, "Unexpected set of Fragments");

            }
        }
    }
}