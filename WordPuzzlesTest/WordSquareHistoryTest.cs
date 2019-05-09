using System;
using NUnit.Framework;
using WordPuzzles;

namespace WordPuzzlesTest
{
    [TestFixture]
    public class WordSquareHistoryTest
    {
        [TestFixture]
        public class AddWordSquare
        {
            [Test]
            public void PopulatesDictionary()
            {
                WordSquareHistory history = new WordSquareHistory();
                var wordSquare = new WordSquare();
                wordSquare.SetWordAtIndex("thank", 0);
                wordSquare.SetWordAtIndex("human", 1);
                wordSquare.SetWordAtIndex("amuse", 2);
                wordSquare.SetWordAtIndex("nasal", 3);
                wordSquare.SetWordAtIndex("knelt", 4);

                history.AddWordSquare(wordSquare, DateTime.Now.AddDays(-1) );
                Assert.AreEqual(5, history.DaysSinceLastUse.Count);

                foreach (string expectedWord in new[] {"thank", "human", "amuse", "nasal", "knelt"})
                {
                    Assert.IsTrue(history.DaysSinceLastUse.ContainsKey(expectedWord));
                    Assert.AreEqual(1, history.DaysSinceLastUse[expectedWord]);
                }
            }
        }

        [TestFixture]
        public class CalculateScore
        {
            [Test]
            public void Example_ReturnsExpectedValue()
            {
                WordSquareHistory history = new WordSquareHistory();
                var wordSquare = new WordSquare();
                wordSquare.SetWordAtIndex("thank", 0);
                wordSquare.SetWordAtIndex("human", 1);
                wordSquare.SetWordAtIndex("amuse", 2);
                wordSquare.SetWordAtIndex("nasal", 3);
                wordSquare.SetWordAtIndex("knelt", 4);

                history.AddWordSquare(wordSquare, DateTime.Now.AddDays(-5));
                Assert.AreEqual(5, history.DaysSinceLastUse.Count);

                foreach (string expectedWord in new[] { "thank", "human", "amuse", "nasal", "knelt" })
                {
                    Assert.AreEqual(5, history.CalculateScore(expectedWord));
                }
                Assert.AreEqual(60, history.CalculateScore("word not in the history"));
            }
        }
    }
}