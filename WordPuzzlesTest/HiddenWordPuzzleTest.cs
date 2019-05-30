using System;
using System.Collections.Generic;
using NUnit.Framework;
using WordPuzzles;

namespace WordPuzzlesTest
{
    [TestFixture]
    public class HiddenWordPuzzleTest
    {
        [TestFixture]
        public class HideWord
        {
            [Test]
            public void TEST_GeneratesExpectedWords()
            {
                HiddenWordPuzzle puzzle = new HiddenWordPuzzle {RandomSeed = 1};

                List<string> words = puzzle.HideWord("test");
                Console.WriteLine(string.Join(',', words));
                Assert.AreEqual("fate", words[0]);
                Assert.AreEqual("stirs", words[1]);
                Assert.AreEqual(2, words.Count);
            }


            [Test]
            public void SOCK_GeneratesNoPuzzles()
            {
                HiddenWordPuzzle puzzle = new HiddenWordPuzzle();
                //puzzle.RandomSeed = 1;

                List<string> words = puzzle.HideWord("sock");
                Console.WriteLine(string.Join(',', words));
                Assert.AreEqual(0, words.Count);
            }

        }

        [TestFixture]
        public class GenerateWordBreaks
        {
            [Test]
            public void CreatesExpectedLists()
            {
                HiddenWordPuzzle puzzle = new HiddenWordPuzzle();

                CollectionAssert.AreEquivalent(new[] { 1, 2 }, puzzle.GenerateWordBreaks(3));
                CollectionAssert.AreEquivalent(new[] {1, 2, 3}, puzzle.GenerateWordBreaks(4));
                CollectionAssert.AreEquivalent(new[] { 1, 2, 3, 4 }, puzzle.GenerateWordBreaks(5));
                CollectionAssert.AreEquivalent(new[] { 1, 2, 3, 4, 5 }, puzzle.GenerateWordBreaks(6));
            }
        }

        [TestFixture]
        public class GenerateAllSplitableStrings
        {
            [Test]
            public void TEST_ReturnsExpectedSplitableStrings()
            {
                HiddenWordPuzzle puzzle = new HiddenWordPuzzle();
                List<string> results = puzzle.GenerateAllSplitableStrings("test");
                List<string> expectedStrings = new List<string> {
                    //"t.est",  //no word starts with est.
                    "te.st",
                    "tes.t" 
                };
                CollectionAssert.AreEquivalent(expectedStrings, results);
            }

            [Test]
            public void LONE_ReturnsExpectedSplitableStrings()
            {
                HiddenWordPuzzle puzzle = new HiddenWordPuzzle();
                List<string> results = puzzle.GenerateAllSplitableStrings("lone");
                List<string> expectedStrings = new List<string> { "l.one", "lo.ne", "lon.e", //Single dot
                    "l.on.e" //double dot
                };
                CollectionAssert.AreEquivalent(expectedStrings, results);
            }

            [Test]
            public void ONION_ReturnsExpectedSplitableStrings()
            {
                HiddenWordPuzzle puzzle = new HiddenWordPuzzle();
                List<string> results = puzzle.GenerateAllSplitableStrings("onion");
                List<string> expectedStrings = new List<string>
                {
                    //"o.nion",//No words start with "nion"
                    "on.ion", 
                    //"oni.on", //No word starts with 'oni'
                    //"onio.n", //
                    "on.i.on",  //double dot.
                };
                CollectionAssert.AreEquivalent(expectedStrings, results);
            }

            [Test]
            public void SOCK_NoWordsStartWithCK_ReturnsExpectedResults()
            {
                HiddenWordPuzzle puzzle = new HiddenWordPuzzle();
                List<string> results = puzzle.GenerateAllSplitableStrings("sock");
                List<string> expectedStrings = new List<string>
                {
                };
                CollectionAssert.AreEquivalent(expectedStrings, results);
            }

        }

        [TestFixture]
        public class FindAllWordsThatStartWith
        {
            [Test]
            public void SIS_ReturnsNonEmptyList()
            {
                HiddenWordPuzzle puzzle = new HiddenWordPuzzle();
                var results =  puzzle.FindAllWordsThatStartWith("sis");
                Assert.Less(0, results.Count, "Expected at least one word to be returned.");

            }
        }

        [TestFixture]
        public class VerifySplitableStringCandidate
        {
            [Test]
            public void O_A_SIS_ReturnsExpectedValue()
            {
                HiddenWordPuzzle puzzle = new HiddenWordPuzzle();
                Assert.IsTrue(puzzle.VerifySplitableStringCandidate("o.a.sis")); //Initially false because no words start with 'sis'. 
            }
        }

        [TestFixture]
        public class CreateSpecificExampleFromSplitableString
        {
            [Test]
            public void ThreeDots_ReturnsExpectedValue()
            {
                HiddenWordPuzzle puzzle = new HiddenWordPuzzle() {RandomSeed =  1};
                Assert.AreEqual(new List<string> {"halo", "a", "sister"}, puzzle.CreateSpecificExampleFromSplitableString("o.a.sis"));
            }
        }

        [TestFixture]
        public class FormatPuzzleAsText
        {
            [Test]
            public void SingleSentence_ReturnsExpectedString()
            {
                const string EXPECTED_TEXT =
@"One word in each sentence below is hidden elsewhere in the sentence.
Find the word, and then write the first letter of that word into the blanks below.
1. Example Sentence
Solution: _ _ .
";
                HiddenWordPuzzle puzzle = new HiddenWordPuzzle() {RandomSeed = 1};
                puzzle.Solution = "ca.";
                puzzle.Sentences.Add("Example Sentence");
                Assert.AreEqual(EXPECTED_TEXT, puzzle.FormatPuzzleAsText());
            }
        }
    }
}