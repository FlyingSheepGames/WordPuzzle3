using System.Collections.Generic;
using NUnit.Framework;
using WordPuzzles;

namespace WordPuzzlesTest
{
    [TestFixture]
    public class VowelMovementTest
    {
        [TestFixture]
        public class FindPuzzle
        {
            [Test]
            public void R_D_FindsExpectedPuzzle()
            {
                VowelMovement puzzle = new VowelMovement();
                List<VowelMovement> vowelMovements = puzzle.FindPuzzle("r", "d");
                Assert.IsNotEmpty(vowelMovements);
                puzzle = vowelMovements[0];
                Assert.AreEqual("It's * to just * away in your * sports car after you * my refridgerator.", puzzle.Clue);
                Assert.AreEqual("It's RUDE to just RIDE away in your RED sports car after you RAID my refridgerator.", puzzle.Solution);
            }

            [Test]
            public void D_N_ExcludesAlreadyPostedClue()
            {
                VowelMovement puzzle = new VowelMovement();
                List<VowelMovement> vowelMovements = puzzle.FindPuzzle("d", "n");
                bool foundPuzzleThatShouldHaveBeenExcluded = false;
                foreach (var foundPuzzle in vowelMovements)
                {
                    if (foundPuzzle.Solution == "A DANE hid in the DEN by the sand DUNE in Anholt.")
                    {
                        foundPuzzleThatShouldHaveBeenExcluded = true;
                        break;
                    }
                }
                Assert.IsFalse(foundPuzzleThatShouldHaveBeenExcluded);
            }

            [Test]
            public void M_N_ExcludesClueWithDate()
            {
                VowelMovement puzzle = new VowelMovement();
                List<VowelMovement> vowelMovements = puzzle.FindPuzzle("m", "n");
                bool foundPuzzleThatShouldHaveBeenExcluded = false;
                foreach (var foundPuzzle in vowelMovements)
                {
                    if (foundPuzzle.Solution == "I saw a science fiction film about the first MAN to MINE the MOON.")
                    {
                        foundPuzzleThatShouldHaveBeenExcluded = true;
                        break;
                    }
                }
                Assert.IsFalse(foundPuzzleThatShouldHaveBeenExcluded);
            }

            [Test]
            public void L_K_FindsExpectedPuzzle()
            {
                VowelMovement puzzle = new VowelMovement();
                List<VowelMovement> vowelMovements = puzzle.FindPuzzle("l", "k");
                bool foundPuzzleThatShouldHaveBeenExcluded = false;
                foreach (var foundPuzzle in vowelMovements)
                {
                    if (foundPuzzle.Solution == "I LIKE the LOOK of the sun reflecting off the frozen LAKE.")
                    {
                        foundPuzzleThatShouldHaveBeenExcluded = true;
                        break;
                    }
                }
                Assert.IsTrue(foundPuzzleThatShouldHaveBeenExcluded);
            }

            //I knew someone was trying to KILL me when I found a lump of COAL in my KALE, so I had to CALL the cops.

            [Test]
            public void C_L_FindsExpectedPuzzle()
            {
                VowelMovement puzzle = new VowelMovement();
                List<VowelMovement> vowelMovements = puzzle.FindPuzzle("c", "l");
                bool foundPuzzleThatShouldHaveBeenExcluded = false;
                foreach (var foundPuzzle in vowelMovements)
                {
                    if (foundPuzzle.Solution == "I knew someone was trying to KILL me when I found a lump of COAL in my KALE, so I had to CALL the cops.")
                    {
                        foundPuzzleThatShouldHaveBeenExcluded = true;
                        break;
                    }
                }
                Assert.IsTrue(foundPuzzleThatShouldHaveBeenExcluded);
            }

            [Test]
            public void K_L_FindsExpectedPuzzle()
            {
                VowelMovement puzzle = new VowelMovement();
                List<VowelMovement> vowelMovements = puzzle.FindPuzzle("k", "l");
                bool foundPuzzleThatShouldHaveBeenExcluded = false;
                foreach (var foundPuzzle in vowelMovements)
                {
                    if (foundPuzzle.Solution == "I knew someone was trying to KILL me when I found a lump of COAL in my KALE, so I had to CALL the cops.")
                    {
                        foundPuzzleThatShouldHaveBeenExcluded = true;
                        break;
                    }
                }
                Assert.IsTrue(foundPuzzleThatShouldHaveBeenExcluded);
            }

            [Test]
            public void BR_K_FindsExpectedPuzzle()
            {
                VowelMovement puzzle = new VowelMovement();
                List<VowelMovement> vowelMovements = puzzle.FindPuzzle("Br", "k");
                bool foundExpectedPuzzle = false;
                foreach (var foundPuzzle in vowelMovements)
                {
                    if (foundPuzzle.Solution == "As the storm BROKE over the BROOK, I had to BRAKE before my car hydroplaned out of control.")
                    {
                        foundExpectedPuzzle = true;
                        break;
                    }
                }
                Assert.IsTrue(foundExpectedPuzzle, "Did not find expected puzzle.");
            }

            [Test]
            public void Null_L_FindsExpectedPuzzle()
            {
                VowelMovement puzzle = new VowelMovement();
                List<VowelMovement> vowelMovements = puzzle.FindPuzzle(null, "l");
                bool foundExpectedPuzzle = false;
                foreach (var foundPuzzle in vowelMovements)
                {
                    if (foundPuzzle.Solution == "What was the name of the OWL who felt ILL after eating an EEL fried in OIL and drinking ALL the ALE? [AL]")
                    {
                        foundExpectedPuzzle = true;
                        break;
                    }
                }
                Assert.IsTrue(foundExpectedPuzzle, "Did not find expected puzzle.");
            }

            [Test]
            public void Q_T_FindsExpectedPuzzle()
            {
                VowelMovement puzzle = new VowelMovement();
                List<VowelMovement> vowelMovements = puzzle.FindPuzzle("q", "t");
                bool foundExpectedPuzzle = false;
                foreach (var foundPuzzle in vowelMovements)
                {
                    if (foundPuzzle.Solution == "The reporter got QUITE a QUOTE from the mayor, who QUIT the next day.")
                    {
                        foundExpectedPuzzle = true;
                        break;
                    }
                }
                Assert.IsTrue(foundExpectedPuzzle, "Did not find expected puzzle.");
            }

            [Test]
            public void Br_Null_FindsExpectedPuzzle()
            {
                VowelMovement puzzle = new VowelMovement();
                List<VowelMovement> vowelMovements = puzzle.FindPuzzle("br", null);
                bool foundExpectedPuzzle = false;
                foreach (var foundPuzzle in vowelMovements)
                {
                    if (foundPuzzle.Solution == "The day started poorly - She singed off a BROW trying to BREW coffee, and couldn't find her favorite BRA.")
                    {
                        foundExpectedPuzzle = true;
                        break;
                    }
                }
                Assert.IsTrue(foundExpectedPuzzle, "Did not find expected puzzle.");
            }

            //During a JOB interview, they said, "I like the cut of your JIB", and gave me a playful JAB.

            [Test]
            public void J_B_FindsExpectedPuzzle()
            {
                VowelMovement puzzle = new VowelMovement();
                List<VowelMovement> vowelMovements = puzzle.FindPuzzle("j", "b");
                bool foundExpectedPuzzle = false;
                foreach (var foundPuzzle in vowelMovements)
                {
                    if (foundPuzzle.Solution == @"During a JOB interview, they said, ""I like the cut of your JIB"", and gave me a playful JAB.")
                    {
                        foundExpectedPuzzle = true;
                        break;
                    }
                }
                Assert.IsTrue(foundExpectedPuzzle, "Did not find expected puzzle.");
            }


            [Test]
            [TestCase("b")]
            [TestCase("bl")]
            [TestCase("br")]
            [TestCase("c")]
            [TestCase("k")]
            [TestCase("ch")]
            [TestCase("cl")]
            [TestCase("cr")]
            [TestCase("d")]
            [TestCase("f")]
            [TestCase("g")]
            [TestCase("gl")]
            [TestCase("gr")]
            [TestCase("h")]
            [TestCase("l")]
            [TestCase("m")]
            [TestCase("n")]
            [TestCase("kn")]
            [TestCase("p")]
            [TestCase("pl")]
            [TestCase("pr")]
            [TestCase("r")]
            [TestCase("s")]
            [TestCase("sp")]
            [TestCase("st")]
            [TestCase("t")]
            [TestCase("w")]
            public void EndingWith_D_FindsAtLeastOnePuzzle(string startConsonant)
            {
                VowelMovement puzzle = new VowelMovement();
                {
                    List<VowelMovement> vowelMovements = puzzle.FindPuzzle(startConsonant, "d");
                    Assert.LessOrEqual(1, vowelMovements.Count,
                        $"Expected at least one puzzle starting with '{startConsonant}' and ending with 'd'.");
                }
            }

            [TestCase("h")]
            [TestCase("w")]
            [TestCase("y")]
            [TestCase("c")]
            [TestCase("k")]
            [TestCase("d")]
            [TestCase("l")]
            [TestCase("ld")]
            [TestCase("n")]
            [TestCase("nd")]
            [TestCase("nt")]
            [TestCase("p")]
            [TestCase("r")]
            [TestCase("rs")]
            [TestCase("s")]
            [TestCase("st")]
            [TestCase("t")]
            [TestCase("x")]
            [TestCase("z")]
            public void StartingWith_M_FindsAtLeastOnePuzzle(string endConsonant)
            {
                VowelMovement puzzle = new VowelMovement();
                {
                    List<VowelMovement> vowelMovements = puzzle.FindPuzzle("m", endConsonant);
                    Assert.LessOrEqual(1, vowelMovements.Count,
                        $"Expected at least one puzzle starting with 'm' and ending with '{endConsonant}'.");
                }
            }

        }

        [TestFixture]
        public class GetTweet
        {
            [Test]
            public void Default_ReturnsExpectedString()
            {
                VowelMovement puzzle = new VowelMovement();
                const string EXPECTED_TWEET =
@"

#HowToPlay: https://t.co/rSa0rUCvRC
";
                Assert.AreEqual(EXPECTED_TWEET, puzzle.GetTweet());
            }

            [Test]
            public void Example_ReturnsExpectedString()
            {
                VowelMovement puzzle = new VowelMovement
                {
                    Clue = "Each morning I have * * muffins with a cup of weak * because it's good for my *."
                };
                const string EXPECTED_TWEET =
                    @"Each morning I have * * muffins with a cup of weak * because it's good for my *.

#HowToPlay: https://t.co/rSa0rUCvRC
";
                Assert.AreEqual(EXPECTED_TWEET, puzzle.GetTweet());
            }

            [Test]
            public void HashtagTheme_ReturnsExpectedString()
            {
                VowelMovement puzzle = new VowelMovement
                {
                    Clue = "Each morning I have * * muffins with a cup of weak * because it's good for my *.",
                    Theme = "#ThemeWeek"
                };
                const string EXPECTED_TWEET =
                    @"#ThemeWeek
Each morning I have * * muffins with a cup of weak * because it's good for my *.

#HowToPlay: https://t.co/rSa0rUCvRC
";
                Assert.AreEqual(EXPECTED_TWEET, puzzle.GetTweet());
            }

            [Test]
            public void NoHashtagTheme_ReturnsExpectedString()
            {
                VowelMovement puzzle = new VowelMovement
                {
                    Clue = "Each morning I have * * muffins with a cup of weak * because it's good for my *.",
                    Theme = "NoHashtag"
                };
                const string EXPECTED_TWEET =
                    @"Each morning I have * * muffins with a cup of weak * because it's good for my *.

#HowToPlay: https://t.co/rSa0rUCvRC
";
                Assert.AreEqual(EXPECTED_TWEET, puzzle.GetTweet());
            }

        }

        [TestFixture]
        public class ReplaceAllCapsWordsWithAsterisks
        {
            [Test]
            public void Example_ReturnsExpectedString()
            {
                VowelMovement vowelMovement = new VowelMovement();
                Assert.AreEqual("Each morning I have * * muffins with a cup of weak * because it's good for my *.", vowelMovement.ReplaceAllCapsWordsWithAsterisks("Each morning I have BROWN BRAN muffins with a cup of weak BRINE because it's good for my BRAIN."));
            }
        }


    }

}