using System;
using System.Collections.Generic;
using NUnit.Framework;
using WordPuzzles;

namespace WordPuzzlesTest
{
    [TestFixture]
    public class WordnikUtilityTest
    {
        [TestFixture]
        public class FindPotentialThemes
        {
            [Test]
            public void BLUE_ReturnsExpectedCollection()
            {
                WordnikUtility utility = new WordnikUtility();
                List<PotentialTheme> results = utility.FindPotentialThemes("blue");
                foreach (var theme in results)
                {
                    Console.WriteLine($"{theme.Name}, {theme.Count}");
                }
                Assert.AreEqual(13, results.Count);
            }

        }

        [TestFixture]
        public class FindWordsInList
        {
            [Test]
            public void LiterallyOfFiguratively_ReturnsExpectedResults()
            {
                WordnikUtility utility = new WordnikUtility();
                List<string> results = utility.FindWordsInList("literally-or-figuratively");
                Assert.AreEqual(4, results.Count);
                Assert.AreEqual("blue", results[0]);
                Assert.AreEqual("malodorous", results[1]);
                Assert.AreEqual("niche", results[2]);
                Assert.AreEqual("pluck", results[3]);
            }
        }

    }
}