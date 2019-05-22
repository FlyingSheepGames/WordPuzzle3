using System;
using System.Collections.Generic;
using NUnit.Framework;
using WordPuzzles;

namespace WordPuzzlesTest
{
    [TestFixture]
    public class ALittleAlliterationTest
    {
        [TestFixture]
        public class FindPuzzle
        {
            [Test]
            public void BIL_ReturnsExpectedObject()
            {
                ALittleAlliteration aLittleAlliteration = new ALittleAlliteration();

                List<ALittleAlliteration> aLittleAlliterations = aLittleAlliteration.FindPuzzle("bil");

                Assert.IsNotEmpty( aLittleAlliterations);
                aLittleAlliteration = aLittleAlliterations[0];
                Assert.AreEqual("billionaire's bile bill",  aLittleAlliteration.Solution);
                Assert.AreEqual("reciept for rich person's bodily fluid", aLittleAlliteration.Clue);
            }

            [Test]
            public void Uppercase_BIL_ReturnsExpectedObject()
            {
                ALittleAlliteration aLittleAlliteration = new ALittleAlliteration();

                List<ALittleAlliteration> aLittleAlliterations = aLittleAlliteration.FindPuzzle("BIL");

                Assert.IsNotEmpty(aLittleAlliterations);
                aLittleAlliteration = aLittleAlliterations[0];
                Assert.AreEqual("billionaire's bile bill", aLittleAlliteration.Solution);
                Assert.AreEqual("reciept for rich person's bodily fluid", aLittleAlliteration.Clue);
            }


            [Test]
            public void BLI_ExcludesClueWithTwitterUrl()
            {
                ALittleAlliteration aLittleAlliteration = new ALittleAlliteration();

                List<ALittleAlliteration> aLittleAlliterations = aLittleAlliteration.FindPuzzle("bli");

                bool foundPuzzleThatShouldHaveBeenExcluded = false;
                foreach (var puzzle in aLittleAlliterations)
                {
                    if (puzzle.Solution == "Blitz blinding bliss")
                    {
                        foundPuzzleThatShouldHaveBeenExcluded = true;
                    }
                }
                Assert.IsFalse(foundPuzzleThatShouldHaveBeenExcluded);
            }

            [Test]
            public void BOO_ExcludesClueWithDate()
            {
                ALittleAlliteration aLittleAlliteration = new ALittleAlliteration();

                List<ALittleAlliteration> aLittleAlliterations = aLittleAlliteration.FindPuzzle("boo");

                bool foundPuzzleThatShouldHaveBeenExcluded = false;
                foreach (var puzzle in aLittleAlliterations)
                {
                    if (puzzle.Solution == "boost boo books")
                    {
                        foundPuzzleThatShouldHaveBeenExcluded = true;
                    }
                }
                Assert.IsFalse(foundPuzzleThatShouldHaveBeenExcluded);
            }

            [Test]
            public void SEA_DoesNotThrow()
            {
                ALittleAlliteration aLittleAlliteration = new ALittleAlliteration();
                aLittleAlliteration.FindPuzzle("sea");
            }

        }

        [TestFixture]
        public class GetTweet
        {
            [Test]
            public void Default_GeneratesExpectedText()
            {
                ALittleAlliteration aLittleAlliteration = new ALittleAlliteration();
                const string EXPECTED_STRING = @"

#HowToPlay: https://t.co/rSa0rUCvRC
";
                Assert.AreEqual(EXPECTED_STRING, aLittleAlliteration.GetTweet());
            }

            [Test]
            public void Example_GeneratesExpectedText()
            {
                ALittleAlliteration aLittleAlliteration = new ALittleAlliteration();
                aLittleAlliteration.Clue = "What color was the emergency vehicle that was unexpectedly attacked?";
                aLittleAlliteration.Solution = "Ambush Amber Ambulance";
                const string EXPECTED_STRING = @"What color was the emergency vehicle that was unexpectedly attacked?

#HowToPlay: https://t.co/rSa0rUCvRC
";
                Assert.AreEqual(EXPECTED_STRING, aLittleAlliteration.GetTweet());
            }

            [Test]
            public void HashtagTheme_GeneratesExpectedText()
            {
                ALittleAlliteration aLittleAlliteration = new ALittleAlliteration();
                aLittleAlliteration.Clue = "What color was the emergency vehicle that was unexpectedly attacked?";
                aLittleAlliteration.Solution = "Ambush Amber Ambulance";
                aLittleAlliteration.Theme = "#ThemeWeek";
                const string EXPECTED_STRING = @"#ThemeWeek
What color was the emergency vehicle that was unexpectedly attacked?

#HowToPlay: https://t.co/rSa0rUCvRC
";
                Assert.AreEqual(EXPECTED_STRING, aLittleAlliteration.GetTweet());
            }

            [Test]
            public void NoHashtagTheme_GeneratesExpectedText()
            {
                ALittleAlliteration aLittleAlliteration = new ALittleAlliteration();
                aLittleAlliteration.Clue = "What color was the emergency vehicle that was unexpectedly attacked?";
                aLittleAlliteration.Solution = "Ambush Amber Ambulance";
                aLittleAlliteration.Theme = "NoHashtag";
                const string EXPECTED_STRING = @"What color was the emergency vehicle that was unexpectedly attacked?

#HowToPlay: https://t.co/rSa0rUCvRC
";
                Assert.AreEqual(EXPECTED_STRING, aLittleAlliteration.GetTweet());
            }

        }

        [TestFixture]
        public class Constructor
        {
            [Test]
            public void ParsesUserInputCorrectly()
            {
                ALittleAlliteration puzzle = new ALittleAlliteration("Solution  =  Clue");
                Assert.AreEqual("Solution", puzzle.Solution);
                Assert.AreEqual("Clue", puzzle.Clue);
            }
        }

        [TestFixture]
        public class GetCluesForTheme
        {
            [Test]
            public void BIRD_IncludesExpectedPuzzle()
            {
                List<ALittleAlliteration> results = ALittleAlliteration.GetCluesForTheme("bird");
                bool foundExpectedPuzzle = false;
                foreach (var puzzle in results)
                {
                    if (puzzle.Solution == "avid aviatrix's aviary")
                    {
                        foundExpectedPuzzle = true;
                        break;
                    }
                }
                Assert.IsTrue(foundExpectedPuzzle, "Did not find expected puzzle.");
            }

            [Test]
            public void MUSIC_SeasonOne_IncludesExpectedPuzzle()
            {
                List<ALittleAlliteration> results = ALittleAlliteration.GetCluesForTheme("music", 1);
                bool foundExpectedPuzzle = false;
                foreach (var puzzle in results)
                {
                    if (puzzle.Solution == "abbreviate ABBA's abbot")
                    {
                        foundExpectedPuzzle = true;
                        break;
                    }
                }
                Assert.IsTrue(foundExpectedPuzzle, "Did not find expected puzzle.");
            }
        }

        [TestFixture]
        public class FindWordsThatStartWith
        {
            [Test]
            public void NoWords_ReturnsEmptySet()
            {
                ALittleAlliteration alliteration = new ALittleAlliteration();
                Assert.AreEqual(0, alliteration.FindWordsThatStartWith("aaa", out _).Count);
            }

            [Test]
            public void ART_ReturnsARTICLE()
            {
                ALittleAlliteration alliteration = new ALittleAlliteration();
                var words = alliteration.FindWordsThatStartWith("art", out _);
                Assert.Contains("article", words);
                foreach (string word in words)
                {
                    Console.WriteLine(word);
                }
                Assert.IsFalse(words.Contains("arterial"), "Expected 'arterial' to be excluded as a copy of artery.");
            }

            [Test]
            public void CHI_ReturnsExpectedResults()
            {
                ALittleAlliteration alliteration = new ALittleAlliteration();
                var words = alliteration.FindWordsThatStartWith("chi", out _);
                Console.WriteLine("--------------");
                foreach (string word in words)
                {
                    Console.WriteLine(word);
                }
                Assert.IsTrue(words.Contains("child"), "Expected 'child' to be included.");

                Assert.IsFalse(words.Contains("chips"), "Expected 'chips' to be excluded.");
                Assert.IsFalse(words.Contains("chil"), "Expected 'chil' to be excluded.");
                Assert.IsFalse(words.Contains("childish"), "Expected 'childish' to be excluded.");
                Assert.IsFalse(words.Contains("chives"), "Expected 'chives' to be excluded.");

            }
        }

        [TestFixture]
        public class AddIfDistinct
        {
            [Test]
            public void DuplicateWord_DoesNotAdd()
            {
                var currentWords = new List<string>() { "any" };
                ALittleAlliteration.AddIfDistinct("any", currentWords);
                Assert.AreEqual(1, currentWords.Count);

            }
            [Test]
            public void DoubleEndingExample()
            {
                const string ROOT_WORD = "chip";
                const string DERRIVED_WORD = "chipped";

                var currentWords = new List<string>() {ROOT_WORD};
                ALittleAlliteration.AddIfDistinct(DERRIVED_WORD, currentWords);
                Assert.AreEqual(1, currentWords. Count);
                Assert.AreEqual(ROOT_WORD, currentWords[0]);

                currentWords = new List<string>() { DERRIVED_WORD };
                ALittleAlliteration.AddIfDistinct(ROOT_WORD, currentWords);
                Assert.AreEqual(1, currentWords.Count);
                Assert.AreEqual(ROOT_WORD, currentWords[0]);

            }

            [Test]
            public void EndingWithY_RemovesIES()
            {
                const string ROOT_WORD = "artery";
                const string DERRIVED_WORD = "arteries";

                var currentWords = new List<string>() { ROOT_WORD };
                ALittleAlliteration.AddIfDistinct(DERRIVED_WORD, currentWords);
                Assert.AreEqual(1, currentWords.Count);
                Assert.AreEqual(ROOT_WORD, currentWords[0]);

                currentWords = new List<string>() { DERRIVED_WORD };
                ALittleAlliteration.AddIfDistinct(ROOT_WORD, currentWords);
                Assert.AreEqual(1, currentWords.Count);
                Assert.AreEqual(ROOT_WORD, currentWords[0]);
            }

            [Test]
            public void EndingWithY_RemovesIAL()
            {
                const string ROOT_WORD = "artery";
                const string DERRIVED_WORD = "arterial";

                var currentWords = new List<string>() { ROOT_WORD };
                ALittleAlliteration.AddIfDistinct(DERRIVED_WORD, currentWords);
                Assert.AreEqual(1, currentWords.Count);
                Assert.AreEqual(ROOT_WORD, currentWords[0]);

                currentWords = new List<string>() { DERRIVED_WORD };
                ALittleAlliteration.AddIfDistinct(ROOT_WORD, currentWords);
                Assert.AreEqual(1, currentWords.Count);
                Assert.AreEqual(ROOT_WORD, currentWords[0]);
            }

            [Test]
            public void Removes_Ful_Ending()
            {
                const string ROOT_WORD = "art";
                const string DERRIVED_WORD = "artful";

                var currentWords = new List<string>() { ROOT_WORD };
                ALittleAlliteration.AddIfDistinct(DERRIVED_WORD, currentWords);
                Assert.AreEqual(1, currentWords.Count);
                Assert.AreEqual(ROOT_WORD, currentWords[0]);

                currentWords = new List<string>() { DERRIVED_WORD };
                ALittleAlliteration.AddIfDistinct(ROOT_WORD, currentWords);
                Assert.AreEqual(1, currentWords.Count);
                Assert.AreEqual(ROOT_WORD, currentWords[0]);
            }


        }
    }
}