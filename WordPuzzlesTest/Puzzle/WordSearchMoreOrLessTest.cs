using NUnit.Framework;
using WordPuzzles.Puzzle;

namespace WordPuzzlesTest.Puzzle
{
    [TestFixture]
    public class WordSearchMoreOrLessTest
    {
        [TestFixture]
        public class ProcessLetter
        {
            [Test]
            public void AddsToHiddenWordList()
            {
                WordSearchMoreOrLess wordSearch = new WordSearchMoreOrLess();
                wordSearch.RandomGeneratorSeed = 42;
                Assert.AreEqual(0, wordSearch.HiddenWords.Count, "Expected no hidden words before processing a letter.");
                wordSearch.ProcessLetter('a');
                Assert.AreEqual(1, wordSearch.HiddenWords.Count, "Expected one hidden word before processing a letter.");
                var firstHiddenWord = wordSearch.HiddenWords[0];
                Assert.AreEqual('a', firstHiddenWord.LetterAddedOrRemoved, "Expected A as first letter removed.");
                Assert.AreEqual("aisle", firstHiddenWord.HiddenWord, "Expected hidden word");
                Assert.AreEqual("isle", firstHiddenWord.DisplayedWord, "Expected displayed word");

            }
        }
    }
}