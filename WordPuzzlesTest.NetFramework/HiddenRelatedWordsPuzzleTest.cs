using System;
using NUnit.Framework;
using WordPuzzles;

namespace WordPuzzlesTest
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