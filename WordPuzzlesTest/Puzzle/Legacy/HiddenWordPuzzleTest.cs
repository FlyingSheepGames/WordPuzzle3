using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using WordPuzzles.Puzzle.Legacy;

namespace WordPuzzlesTest.Puzzle.Legacy
{
    [TestFixture]
    public class HiddenWordPuzzleTest
    {
        [TestFixture]
        public class HideWord
        {
            [Test]
            [Ignore("Takes more than 3 seconds.")]
            public void TEST_GeneratesExpectedWords()
            {
                HiddenWordPuzzle puzzle = new HiddenWordPuzzle {RandomSeed = 1};

                List<string> words = puzzle.HideWord("test");
                Console.WriteLine(string.Join(','.ToString(), words));
                Assert.AreEqual("fate", words[0]);
                Assert.AreEqual("stirs", words[1]);
                Assert.AreEqual(2, words.Count);
            }


            [Test]
            [Ignore("Takes more than 3 seconds.")]
            public void SOCK_GeneratesNoPuzzles()
            {
                HiddenWordPuzzle puzzle = new HiddenWordPuzzle();
                //puzzle.RandomSeed = 1;

                List<string> words = puzzle.HideWord("sock");
                Console.WriteLine(string.Join(','.ToString(), words));
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
            [Ignore("Takes more than 3 seconds.")]
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
            [Ignore("Takes more than 3 seconds.")]
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
            [Ignore("Takes more than 3 seconds.")]
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
            [Ignore("Takes more than 3 seconds.")]
            public void DOMAIN_ReturnsExpectedSplitableStrings()
            {
                HiddenWordPuzzle puzzle = new HiddenWordPuzzle();
                List<string> results = puzzle.GenerateAllSplitableStrings("domain");
                List<string> expectedStrings = new List<string>
                {
                    "do.main",  //one dot
                    "dom.a.in", //two dots
                    "dom.a.i.n"//three dots
                };
                CollectionAssert.AreEquivalent(expectedStrings, results);
            }

            [Test]
            [Ignore("Takes more than 3 seconds.")]
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
            [Ignore("Takes more than 3 seconds.")]
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
            [Ignore("Takes more than 3 seconds.")]
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
            [Ignore("Takes more than 3 seconds.")]
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

        [TestFixture]
        public class FindWordsAtTheStartOfThisString
        {


            [Test]
            [Ignore("Takes more than 3 seconds.")]
            public void AIN_IncludesA()
            {
                HiddenWordPuzzle puzzle = new HiddenWordPuzzle();
                puzzle.Solution = "domain";
                var results = puzzle.FindWordsAtTheStartOfThisString("ain");
                CollectionAssert.Contains(results, "a");
            }
        }

        [TestFixture]
        public class ProcessRemainingLetters
        {
            [Test]
           [Ignore("Takes more than 3 seconds.")]

            public void DOMAIN_ReturnsExpectedResult()
            {
                HiddenWordPuzzle puzzle = new HiddenWordPuzzle();
                puzzle.Solution = "domain";
                var splitableStrings = new List<string>();
                puzzle.ProcessRemainingLetters("domain", "ain", new StringBuilder("dom."), splitableStrings);
                CollectionAssert.Contains(splitableStrings, "dom.a.i.n")
                ;
            }
        }
    }
}