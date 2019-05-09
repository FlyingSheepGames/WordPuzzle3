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
                HiddenWordPuzzle puzzle = new HiddenWordPuzzle();
                puzzle.RandomSeed = 1;

                List<string> words = puzzle.HideWord("test");
                Console.WriteLine(string.Join(',', words));
                Assert.AreEqual("mate", words[0]);
                Assert.AreEqual("stem", words[1]);
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
    }
}