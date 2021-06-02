using System;
using System.Collections.Generic;
using NUnit.Framework;
using WordPuzzles.Puzzle;

namespace WordPuzzlesTest.Puzzle
{
    [TestFixture]
    public class PuzzlePyramidTest
    {
        [TestFixture]
        public class FindPeopleBornInRange
        {

            [Test]
            [Timeout(600000)]
            public void Jan1_Includes_Jack_Hanna()
            {
                PuzzlePyramid pyramid = new PuzzlePyramid();

                var results = pyramid.FindPeopleBornInRange(new DateTime(2021, 1, 1), new DateTime(2021, 1, 3));
                bool foundJackHanna = false;
                foreach (var person in results)
                {
                    Console.WriteLine(person.Name);
                    if (person.Name == "Jack Hanna")
                    {
                        foundJackHanna = true;
                        break;
                    }
                }
                Assert.IsTrue(foundJackHanna, "Expected to find Jack Hanna.");
            }
            [Test]
            [Timeout(600000)]
            public void Jan1_Ordered_ByYear()
            {
                PuzzlePyramid pyramid = new PuzzlePyramid();

                var results = pyramid.FindPeopleBornInRange(new DateTime(2021, 1, 1), new DateTime(2021, 1, 3));

                int currentYear = 0;
                foreach (var person in results)
                {
                    Console.WriteLine(person.Year);
                    if (currentYear < person.Year)
                    {
                        currentYear = person.Year;
                    }
                    Assert.LessOrEqual(currentYear, person.Year, "Expected years to be increasing.");
                }

            }

        }

        [TestFixture]
        public class ReplaceWordsWithMarkers
        {

            //Suspicion is the cancer of friendship.
            //Replace lower case "suspicion, cancer, friendship
            [Test]
            public void ReplaceFirstWord()
            {
                string result = PuzzlePyramid.ReplaceWordsWithMarkers("Suspicion is the cancer of friendship.",
                    new List<string>() {"suspicion", "cancer", "friendship"});
                Assert.AreEqual("(solve puzzle J) is the (solve puzzle K) of (solve puzzle L).", result);
            }
            [Test]
            public void ReplaceSecondWord()
            {
                string result = PuzzlePyramid.ReplaceWordsWithMarkers("Deep suspicion is the cancer of friendship.",
                    new List<string>() { "suspicion", "cancer", "friendship" });
                Assert.AreEqual("Deep (solve puzzle J) is the (solve puzzle K) of (solve puzzle L).", result);
            }

        }
    }
}