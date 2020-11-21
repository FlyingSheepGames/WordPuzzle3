using System;
using System.Collections.Generic;
using NUnit.Framework;
using WordPuzzles.Utility;

namespace WordPuzzlesTest.Utility
{
    [TestFixture]
    public class IdiomFinderTest
    {
        [TestFixture]
        public class FindIdioms
        {

            [Test]
            public void IncludesZipOnesLip()
            {
                IdiomFinder finder = new IdiomFinder();
                var listOfIdioms = finder.FindIdioms();
                foreach (var idiom in listOfIdioms)
                {
                    Console.WriteLine(idiom);
                }
                Assert.Contains("Zip One’s Lip", listOfIdioms, "Expected to find Zip One's Lip" );
            }
        }

        [TestFixture]
        public class ExtractCluesFromIdiom
        {
            [Test]
            public void AColdFish_ReturnsExpectedClues()
            {
                IdiomFinder finder = new IdiomFinder();
                Dictionary<string, Clue> extractedClues = finder.ExtractCluesFromIdiom("A Cold Fish");
                Assert.AreEqual(2, extractedClues.Count, "Expected 2 clues");
                Assert.IsTrue(extractedClues.ContainsKey("cold"), "Expected a clue for cold.");
                Clue firstClue = extractedClues["cold"];
                Assert.AreEqual("A ____ Fish", firstClue.ClueText);
                Assert.AreEqual(ClueSource.ClueSourceIdiom, firstClue.ClueSource);

                Assert.IsTrue(extractedClues.ContainsKey("fish"), "Expected a clue for fish.");
                Clue secondClue = extractedClues["fish"];
                Assert.AreEqual("A Cold ____", secondClue.ClueText);
                Assert.AreEqual(ClueSource.ClueSourceIdiom, secondClue.ClueSource);
            }
        }
    }
}