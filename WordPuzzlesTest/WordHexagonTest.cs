using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using WordPuzzles;

namespace WordPuzzlesTest
{
    [TestFixture]
    public class WordHexagonTest
    {
        [TestFixture]
        public class SetHorizontalLineAtIndex
        {
            [Test]
            public void NIGHT_CreatesExpectedPuzzle()
            {
                WordHexagon hexagon = new WordHexagon();
                hexagon.SetHorizontalLineAtIndex(2, "night");

                Assert.AreEqual("___", hexagon.Lines[0]);
                Assert.AreEqual("____", hexagon.Lines[1]);
                Assert.AreEqual("night", hexagon.Lines[2]);
                Assert.AreEqual("____", hexagon.Lines[3]);
                Assert.AreEqual("___", hexagon.Lines[4]);
            }

            [Test]
            public void CreatesExpectedUniqueWords()
            {
                WordHexagon hexagon = new WordHexagon();
                hexagon.SetHorizontalLineAtIndex(0, "war");
                hexagon.SetHorizontalLineAtIndex(1, "taco");
                hexagon.SetHorizontalLineAtIndex(2, "night");
                hexagon.SetHorizontalLineAtIndex(3, "aloe");
                hexagon.SetHorizontalLineAtIndex(4, "pen");

                Console.WriteLine(hexagon);
                List<string> ExpectedWords = new List<string>();
                ExpectedWords.Add("war");
                ExpectedWords.Add("taco");
                ExpectedWords.Add("night");
                ExpectedWords.Add("aloe");
                ExpectedWords.Add("pen");

                ExpectedWords.Add("wagon");
                ExpectedWords.Add("tile");
                ExpectedWords.Add("rot");
                ExpectedWords.Add("ache");
                ExpectedWords.Add("nap");
                CollectionAssert.AreEquivalent(ExpectedWords, hexagon.UniqueWords);
            }
        }

        [TestFixture]
        public class FindDiagonalLineAtIndex
        {
            [Test]
            public void NIGHT_FindsExpectedLines()
            {
                WordHexagon hexagon = new WordHexagon();
                hexagon.SetHorizontalLineAtIndex(2, "night");

                List<string> results = hexagon.FindDiagonalLineAtIndex(2);
                Assert.AreEqual("aegis", results[0]);
            }

        }

        [TestFixture]
        public class SetDiagonalLineAtIndex
        {
            [Test]
            public void NIGHT_ANGEL_Index_2_CreatesExpectedPuzzle()
            {
                WordHexagon hexagon = new WordHexagon();
                hexagon.SetHorizontalLineAtIndex(2, "night");

                hexagon.SetDiagonalLineAtIndex(2, "angel");

                Assert.AreEqual("a__", hexagon.Lines[0]);
                Assert.AreEqual("_n__", hexagon.Lines[1]);
                Assert.AreEqual("night", hexagon.Lines[2]);
                Assert.AreEqual("__e_", hexagon.Lines[3]);
                Assert.AreEqual("__l", hexagon.Lines[4]);
            }

            [Test]
            public void CAT_Index_0_CreatesExpectedPuzzle()
            {
                WordHexagon hexagon = new WordHexagon();

                hexagon.SetDiagonalLineAtIndex(0, "cat");

                Assert.AreEqual("___", hexagon.Lines[0]);
                Assert.AreEqual("____", hexagon.Lines[1]);
                Assert.AreEqual("c____", hexagon.Lines[2]);
                Assert.AreEqual("a___", hexagon.Lines[3]);
                Assert.AreEqual("t__", hexagon.Lines[4]);
            }

            [Test]
            public void TURN_Index_1_CreatesExpectedPuzzle()
            {
                WordHexagon hexagon = new WordHexagon();

                hexagon.SetDiagonalLineAtIndex(1, "turn");

                Assert.AreEqual("___", hexagon.Lines[0]);
                Assert.AreEqual("t___", hexagon.Lines[1]);
                Assert.AreEqual("_u___", hexagon.Lines[2]);
                Assert.AreEqual("_r__", hexagon.Lines[3]);
                Assert.AreEqual("_n_", hexagon.Lines[4]);
            }

            [Test]
            public void TURN_Index_3_CreatesExpectedPuzzle()
            {
                WordHexagon hexagon = new WordHexagon();

                hexagon.SetDiagonalLineAtIndex(3, "turn");

                Assert.AreEqual("_t_", hexagon.Lines[0]);
                Assert.AreEqual("__u_", hexagon.Lines[1]);
                Assert.AreEqual("___r_", hexagon.Lines[2]);
                Assert.AreEqual("___n", hexagon.Lines[3]);
                Assert.AreEqual("___", hexagon.Lines[4]);
            }

            [Test]
            public void CAT_Index_4_CreatesExpectedPuzzle()
            {
                WordHexagon hexagon = new WordHexagon();

                hexagon.SetDiagonalLineAtIndex(4, "cat");

                Assert.AreEqual("__c", hexagon.Lines[0]);
                Assert.AreEqual("___a", hexagon.Lines[1]);
                Assert.AreEqual("____t", hexagon.Lines[2]);
                Assert.AreEqual("____", hexagon.Lines[3]);
                Assert.AreEqual("___", hexagon.Lines[4]);
            }

            [Test]
            public void CreatesExpectedUniqueWords()
            {
                WordHexagon hexagon = new WordHexagon();
                hexagon.SetDiagonalLineAtIndex(0, "nib");
                hexagon.SetDiagonalLineAtIndex(1, "visa");
                hexagon.SetDiagonalLineAtIndex(2, "bigly");
                hexagon.SetDiagonalLineAtIndex(3, "ache");
                hexagon.SetDiagonalLineAtIndex(4, "net");

                Console.WriteLine(hexagon);
                List<string> ExpectedWords = new List<string>();
                ExpectedWords.Add("ban");
                ExpectedWords.Add("vice");
                ExpectedWords.Add("night");
                ExpectedWords.Add("isle");
                ExpectedWords.Add("bay");

                ExpectedWords.Add("nib");
                ExpectedWords.Add("visa");
                ExpectedWords.Add("bigly");
                ExpectedWords.Add("ache");
                ExpectedWords.Add("net");
                CollectionAssert.AreEquivalent(ExpectedWords, hexagon.UniqueWords);
            }

        }

        [TestFixture]
        public class FindHorizontalLineAtIndex
        {
            [Test]
            public void NIGHT_ANGEL_FindsExpectedLines()
            {
                WordHexagon hexagon = new WordHexagon();
                hexagon.SetHorizontalLineAtIndex(2, "night");
                hexagon.SetDiagonalLineAtIndex(2, "angel");

                List<string> results = hexagon.FindHorizontalLineAtIndex(1);
                Assert.AreEqual("ante", results[0]);
            }

            [Test]
            public void NIGHT_ANGEL_ANTE_FindsExpectedLines()
            {
                WordHexagon hexagon = new WordHexagon();
                hexagon.SetHorizontalLineAtIndex(2, "night");
                hexagon.SetDiagonalLineAtIndex(2, "angel");
                hexagon.SetHorizontalLineAtIndex(1, "ante");

                List<string> results = hexagon.FindHorizontalLineAtIndex(3);
                Assert.AreEqual("ages", results[0]);
            }

        }

        [TestFixture]
        public class ToStringDescription
        {
            [Test]
            public void ReturnsExpectedString()
            {
                WordHexagon hexagon = new WordHexagon();
                hexagon.SetHorizontalLineAtIndex(2, "night");
                hexagon.SetDiagonalLineAtIndex(2, "angel");
                hexagon.SetHorizontalLineAtIndex(1, "ante");

                const string EXPECTED_STRING = 
@"  A _ _
 A N T E
N I G H T
 _ _ E _
  _ _ L
Unique words: night, angel, ante
";
                Assert.AreEqual(EXPECTED_STRING, hexagon.ToString());
            }

        }

        [TestFixture]
        public class Serialization
        {
            [Test]
            public void NoAssertions()
            {
                WordHexagon hexagon = new WordHexagon();
                hexagon.SetHorizontalLineAtIndex(0, "ban");
                hexagon.SetHorizontalLineAtIndex(1, "vice");
                hexagon.SetHorizontalLineAtIndex(2, "night");
                hexagon.SetHorizontalLineAtIndex(3, "isle");
                hexagon.SetHorizontalLineAtIndex(4, "bay");

                const string fileName = "test.xml";
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }
                hexagon.Serialize(fileName);
            }
        }
        [TestFixture]
        public class CalculateDiagonalWordPattern
        {
            [Test]
            public void ReturnsExpectedResults()
            {
                WordHexagon hexagon = new WordHexagon();
                hexagon.Lines[0] = "abc";
                hexagon.Lines[1] = "defg";
                hexagon.Lines[2] = "hijkl";
                hexagon.Lines[3] = "mnop";
                hexagon.Lines[4] = "qrs";

                Assert.AreEqual("hmq", hexagon.CalculateDiagonalWordPattern(0));
                Assert.AreEqual("dinr", hexagon.CalculateDiagonalWordPattern(1));
                Assert.AreEqual("aejos", hexagon.CalculateDiagonalWordPattern(2));
                Assert.AreEqual("bfkp", hexagon.CalculateDiagonalWordPattern(3));
                Assert.AreEqual("cgl", hexagon.CalculateDiagonalWordPattern(4));

            }
        }

    }

    [TestFixture]
    public class WordHexagonSizeFourTest
    {
        [TestFixture]
        public class SetHorizontalLineAtIndex
        {
            [Test]
            public void KNIGHT_CreatesExpectedPuzzle()
            {
                WordHexagon hexagon = new WordHexagon(4);
                hexagon.SetHorizontalLineAtIndex(2, "knight");

                Assert.AreEqual("____", hexagon.Lines[0]);
                Assert.AreEqual("_____", hexagon.Lines[1]);
                Assert.AreEqual("knight", hexagon.Lines[2]);
                Assert.AreEqual("___*___", hexagon.Lines[3]);
                Assert.AreEqual("______", hexagon.Lines[4]);
                Assert.AreEqual("_____", hexagon.Lines[5]);
                Assert.AreEqual("____", hexagon.Lines[6]);
            }

            [Test]
            public void CAT_CreatesExpectedPuzzle()
            {
                WordHexagon hexagon = new WordHexagon(4);
                hexagon.SetHorizontalLineAtIndex(3, "cat");

                Assert.AreEqual("____", hexagon.Lines[0]);
                Assert.AreEqual("_____", hexagon.Lines[1]);
                Assert.AreEqual("______", hexagon.Lines[2]);
                Assert.AreEqual("cat*___", hexagon.Lines[3]);
                Assert.AreEqual("______", hexagon.Lines[4]);
                Assert.AreEqual("_____", hexagon.Lines[5]);
                Assert.AreEqual("____", hexagon.Lines[6]);
            }

            [Test]
            public void DOG_CreatesExpectedPuzzle()
            {
                WordHexagon hexagon = new WordHexagon(4);
                hexagon.SetHorizontalLineAtIndex(4, "dog");

                Assert.AreEqual("____", hexagon.Lines[0]);
                Assert.AreEqual("_____", hexagon.Lines[1]);
                Assert.AreEqual("______", hexagon.Lines[2]);
                Assert.AreEqual("___*dog", hexagon.Lines[3]);
                Assert.AreEqual("______", hexagon.Lines[4]);
                Assert.AreEqual("_____", hexagon.Lines[5]);
                Assert.AreEqual("____", hexagon.Lines[6]);
            }

            [Test]
            public void KNIGHT_Index5_CreatesExpectedPuzzle()
            {
                WordHexagon hexagon = new WordHexagon(4);
                hexagon.SetHorizontalLineAtIndex(5, "knight");

                Assert.AreEqual("____", hexagon.Lines[0]);
                Assert.AreEqual("_____", hexagon.Lines[1]);
                Assert.AreEqual("______", hexagon.Lines[2]);
                Assert.AreEqual("___*___", hexagon.Lines[3]);
                Assert.AreEqual("knight", hexagon.Lines[4]);
                Assert.AreEqual("_____", hexagon.Lines[5]);
                Assert.AreEqual("____", hexagon.Lines[6]);
            }

            [Test]
            public void NIGHT_Index6_CreatesExpectedPuzzle()
            {
                WordHexagon hexagon = new WordHexagon(4);
                hexagon.SetHorizontalLineAtIndex(6, "night");

                Assert.AreEqual("____", hexagon.Lines[0]);
                Assert.AreEqual("_____", hexagon.Lines[1]);
                Assert.AreEqual("______", hexagon.Lines[2]);
                Assert.AreEqual("___*___", hexagon.Lines[3]);
                Assert.AreEqual("______", hexagon.Lines[4]);
                Assert.AreEqual("night", hexagon.Lines[5]);
                Assert.AreEqual("____", hexagon.Lines[6]);
            }

            [Test]
            public void NIGHT_Index7_CreatesExpectedPuzzle()
            {
                WordHexagon hexagon = new WordHexagon(4);
                hexagon.SetHorizontalLineAtIndex(7, "dawn");

                Assert.AreEqual("____", hexagon.Lines[0]);
                Assert.AreEqual("_____", hexagon.Lines[1]);
                Assert.AreEqual("______", hexagon.Lines[2]);
                Assert.AreEqual("___*___", hexagon.Lines[3]);
                Assert.AreEqual("______", hexagon.Lines[4]);
                Assert.AreEqual("_____", hexagon.Lines[5]);
                Assert.AreEqual("dawn", hexagon.Lines[6]);
            }


            /* TODO: Revisit when we have a fully populated example.
            [Test]
            public void CreatesExpectedUniqueWords()
            {
                WordHexagon hexagon = new WordHexagon();
                hexagon.SetHorizontalLineAtIndex(0, "war");
                hexagon.SetHorizontalLineAtIndex(1, "taco");
                hexagon.SetHorizontalLineAtIndex(2, "night");
                hexagon.SetHorizontalLineAtIndex(3, "aloe");
                hexagon.SetHorizontalLineAtIndex(4, "pen");

                Console.WriteLine(hexagon);
                List<string> ExpectedWords = new List<string>();
                ExpectedWords.Add("war");
                ExpectedWords.Add("taco");
                ExpectedWords.Add("night");
                ExpectedWords.Add("aloe");
                ExpectedWords.Add("pen");

                ExpectedWords.Add("wagon");
                ExpectedWords.Add("tile");
                ExpectedWords.Add("rot");
                ExpectedWords.Add("ache");
                ExpectedWords.Add("nap");
                CollectionAssert.AreEquivalent(ExpectedWords, hexagon.UniqueWords);
            }
            */
        }

        [TestFixture]
        public class FindDiagonalLineAtIndex
        {
            [Test]
            public void KNIGHT_FindsExpectedLines()
            {
                WordHexagon hexagon = new WordHexagon(4);
                hexagon.SetHorizontalLineAtIndex(2, "knight");

                List<string> results = hexagon.FindDiagonalLineAtIndex(1);
                //should start with K
                Assert.AreEqual("kabob", results[0]);
                foreach (var result in results)
                {
                    StringAssert.StartsWith("k", result, $"Expected {result} to start with k.");
                }
            }

            [Test]
            public void ExcludesPluralVersionsOfWords()
            {
                WordHexagon hexagon = new WordHexagon(4);
                hexagon.SetHorizontalLineAtIndex(0, "rock");

                var candidates = hexagon.FindDiagonalLineAtIndex(1);
                bool foundRocks = false;
                foreach (string candidate in candidates)
                {
                    if (candidate == "rocks")
                    {
                        foundRocks = true;
                    }
                }
                Assert.IsFalse(foundRocks, "Should not have found ROCKS because ROCK is already in the puzzle.");
            }

            [Test]
            public void ExcludesSingleVersionsOfWords()
            {
                WordHexagon hexagon = new WordHexagon(4);
                hexagon.SetHorizontalLineAtIndex(1, "rocks");

                var candidates = hexagon.FindDiagonalLineAtIndex(0);
                bool foundRock = false;
                foreach (string candidate in candidates)
                {
                    if (candidate == "rock")
                    {
                        foundRock = true;
                    }
                }
                Assert.IsFalse(foundRock, "Should not have found ROCK because ROCKS is already in the puzzle.");
            }

        }

        [TestFixture]
        public class SetDiagonalLineAtIndex
        {

            [Test]
            public void CATS_Index_0_CreatesExpectedPuzzle()
            {
                WordHexagon hexagon = new WordHexagon(4);

                hexagon.SetDiagonalLineAtIndex(0, "cats");

                Assert.AreEqual("____", hexagon.Lines[0]);
                Assert.AreEqual("_____", hexagon.Lines[1]);
                Assert.AreEqual("______", hexagon.Lines[2]);
                Assert.AreEqual("c__*___", hexagon.Lines[3]);
                Assert.AreEqual("a_____", hexagon.Lines[4]);
                Assert.AreEqual("t____", hexagon.Lines[5]);
                Assert.AreEqual("s___", hexagon.Lines[6]);
            }

            [Test]
            public void TURNS_Index_1_CreatesExpectedPuzzle()
            {
                WordHexagon hexagon = new WordHexagon(4);

                hexagon.SetDiagonalLineAtIndex(1, "turns");

                Assert.AreEqual(   "____", hexagon.Lines[0]);
                Assert.AreEqual(  "_____", hexagon.Lines[1]);
                Assert.AreEqual( "t_____", hexagon.Lines[2]);
                Assert.AreEqual("_u_*___", hexagon.Lines[3]);
                Assert.AreEqual( "_r____", hexagon.Lines[4]);
                Assert.AreEqual(  "_n___", hexagon.Lines[5]);
                Assert.AreEqual(   "_s__", hexagon.Lines[6]);
            }

            [Test]
            public void TURNIP_Index_2_CreatesExpectedPuzzle()
            {
                WordHexagon hexagon = new WordHexagon(4);

                hexagon.SetDiagonalLineAtIndex(2, "turnip");

                Assert.AreEqual(   "____", hexagon.Lines[0]);
                Assert.AreEqual(  "t____", hexagon.Lines[1]);
                Assert.AreEqual( "_u____", hexagon.Lines[2]);
                Assert.AreEqual("__r*___", hexagon.Lines[3]);
                Assert.AreEqual( "__n___", hexagon.Lines[4]);
                Assert.AreEqual(  "__i__", hexagon.Lines[5]);
                Assert.AreEqual(   "__p_", hexagon.Lines[6]);
            }

            [Test]
            public void CAT_Index_3_CreatesExpectedPuzzle()
            {
                WordHexagon hexagon = new WordHexagon(4);

                hexagon.SetDiagonalLineAtIndex(3, "cat");

                Assert.AreEqual("c___", hexagon.Lines[0]);
                Assert.AreEqual("_a___", hexagon.Lines[1]);
                Assert.AreEqual("__t___", hexagon.Lines[2]);
                Assert.AreEqual("___*___", hexagon.Lines[3]);
                Assert.AreEqual("______", hexagon.Lines[4]);
                Assert.AreEqual("_____", hexagon.Lines[5]);
                Assert.AreEqual("____", hexagon.Lines[6]);
            }


            [Test]
            public void DOG_Index_4_CreatesExpectedPuzzle()
            {
                WordHexagon hexagon = new WordHexagon(4);

                hexagon.SetDiagonalLineAtIndex(4, "dog");

                Assert.AreEqual("____", hexagon.Lines[0]);
                Assert.AreEqual("_____", hexagon.Lines[1]);
                Assert.AreEqual("______", hexagon.Lines[2]);
                Assert.AreEqual("___*___", hexagon.Lines[3]);
                Assert.AreEqual("___d__", hexagon.Lines[4]);
                Assert.AreEqual("___o_", hexagon.Lines[5]);
                Assert.AreEqual("___g", hexagon.Lines[6]);
            }

            [Test]
            public void TURNIP_Index_5_CreatesExpectedPuzzle()
            {
                WordHexagon hexagon = new WordHexagon(4);

                hexagon.SetDiagonalLineAtIndex(5, "turnip");

                Assert.AreEqual("_t__", hexagon.Lines[0]);
                Assert.AreEqual("__u__", hexagon.Lines[1]);
                Assert.AreEqual("___r__", hexagon.Lines[2]);
                Assert.AreEqual("___*n__", hexagon.Lines[3]);
                Assert.AreEqual("____i_", hexagon.Lines[4]);
                Assert.AreEqual("____p", hexagon.Lines[5]);
                Assert.AreEqual("____", hexagon.Lines[6]);
            }

            [Test]
            public void NIGHT_Index_6_CreatesExpectedPuzzle()
            {
                WordHexagon hexagon = new WordHexagon(4);

                hexagon.SetDiagonalLineAtIndex(6, "night");

                Assert.AreEqual("__n_", hexagon.Lines[0]);
                Assert.AreEqual("___i_", hexagon.Lines[1]);
                Assert.AreEqual("____g_", hexagon.Lines[2]);
                Assert.AreEqual("___*_h_", hexagon.Lines[3]);
                Assert.AreEqual("_____t", hexagon.Lines[4]);
                Assert.AreEqual("_____", hexagon.Lines[5]);
                Assert.AreEqual("____", hexagon.Lines[6]);
            }

            [Test]
            public void DAWN_Index_7_CreatesExpectedPuzzle()
            {
                WordHexagon hexagon = new WordHexagon(4);

                hexagon.SetDiagonalLineAtIndex(7, "dawn");

                Assert.AreEqual("___d", hexagon.Lines[0]);
                Assert.AreEqual("____a", hexagon.Lines[1]);
                Assert.AreEqual("_____w", hexagon.Lines[2]);
                Assert.AreEqual("___*__n", hexagon.Lines[3]);
                Assert.AreEqual("______", hexagon.Lines[4]);
                Assert.AreEqual("_____", hexagon.Lines[5]);
                Assert.AreEqual("____", hexagon.Lines[6]);
            }

            [Test]
            public void KNIGHTS_ARGENT_Example()
            {
                WordHexagon hexagon = new WordHexagon(4);
                hexagon.SetHorizontalLineAtIndex(2, "knight");
                Assert.IsTrue(hexagon.SetDiagonalLineAtIndex(5, "argent"));

            }

            /* TODO: Get full example.
            [Test]
            public void CreatesExpectedUniqueWords()
            {
                WordHexagon hexagon = new WordHexagon();
                hexagon.SetDiagonalLineAtIndex(0, "nib");
                hexagon.SetDiagonalLineAtIndex(1, "visa");
                hexagon.SetDiagonalLineAtIndex(2, "bigly");
                hexagon.SetDiagonalLineAtIndex(3, "ache");
                hexagon.SetDiagonalLineAtIndex(4, "net");

                Console.WriteLine(hexagon);
                List<string> ExpectedWords = new List<string>();
                ExpectedWords.Add("ban");
                ExpectedWords.Add("vice");
                ExpectedWords.Add("night");
                ExpectedWords.Add("isle");
                ExpectedWords.Add("bay");

                ExpectedWords.Add("nib");
                ExpectedWords.Add("visa");
                ExpectedWords.Add("bigly");
                ExpectedWords.Add("ache");
                ExpectedWords.Add("net");
                CollectionAssert.AreEquivalent(ExpectedWords, hexagon.UniqueWords);
            }
            */
        }

        [TestFixture]
        public class FindHorizontalLineAtIndex
        {
            [Test]
            public void KNIGHT_KABOB_FindsExpectedLines()
            {
                WordHexagon hexagon = new WordHexagon(4);
                hexagon.SetHorizontalLineAtIndex(2, "knight");
                hexagon.SetDiagonalLineAtIndex(1, "kabob");

                List<string> results = hexagon.FindHorizontalLineAtIndex(3);
                Assert.AreEqual("baa", results[0]);
                foreach (var result in results)
                {
                    Assert.AreEqual('a', result[1], $"Expected 'a' as the middle character in {result}");
                }
            }

            [Test]
            public void KNIGHT_KABOO_ASHED_FindsExpectedLines()
            {
                WordHexagon hexagon = new WordHexagon(4);
                hexagon.SetHorizontalLineAtIndex(2, "knight");
                hexagon.SetDiagonalLineAtIndex(1, "kaboo");
                hexagon.SetDiagonalLineAtIndex(6, "ashed");

                List<string> results = hexagon.FindHorizontalLineAtIndex(0);
                foreach (var result in results)
                {
                    Assert.AreEqual('a', result[2], $"Expected 'a' as the third character in {result}");
                }
                Assert.AreEqual("agar", results[0]);

                results = hexagon.FindHorizontalLineAtIndex(1);
                foreach (var result in results)
                {
                    Assert.AreEqual('s', result[3], $"Expected 's' as the fourth character in {result}");
                }
                Assert.AreEqual("abase", results[0]);

                //index 2 is KNIGHT, already placed.
                //index 3 tested above.

                results = hexagon.FindHorizontalLineAtIndex(4);
                foreach (var result in results)
                {
                    Assert.AreEqual('e', result[1], $"Expected 'e' as the middle character in {result}");
                }
                Assert.AreEqual("bed", results[0]);

                results = hexagon.FindHorizontalLineAtIndex(5);
                foreach (var result in results)
                {
                    Assert.AreEqual('b', result[1], $"Expected 'b' as the second character in {result}");
                    Assert.AreEqual('d', result[5], $"Expected 'd' as the last character in {result}");
                }
                Assert.AreEqual("abound", results[0]);

                results = hexagon.FindHorizontalLineAtIndex(6);
                foreach (var result in results)
                {
                    Assert.AreEqual('o', result[1], $"Expected 'o' as the second character in {result}");
                }
                Assert.AreEqual("aorta", results[0]);

                results = hexagon.FindHorizontalLineAtIndex(7);
                foreach (var result in results)
                {
                    Assert.AreEqual('o', result[1], $"Expected 'b' as the second character in {result}");
                }
                Assert.AreEqual("boar", results[0]);
            }

        }

        [TestFixture]
        public class ToStringDescription
        {
            [Test]
            public void ReturnsExpectedString()
            {
                WordHexagon hexagon = new WordHexagon(4);
                hexagon.SetHorizontalLineAtIndex(2, "knight");
                hexagon.SetDiagonalLineAtIndex(1, "kaboo");
                hexagon.SetDiagonalLineAtIndex(6, "ashed");

                const string EXPECTED_STRING =
@"   _ _ A _
  _ _ _ S _
 K N I G H T
_ A _ * _ E _
 _ B _ _ _ D
  _ O _ _ _
   _ O _ _
Unique words: knight, kaboo, ashed
";
                Assert.AreEqual(EXPECTED_STRING, hexagon.ToString());
            }

        }

        [TestFixture]
        public class Serialization
        {
            [Test]
            public void NoAssertions()
            {
                WordHexagon hexagon = new WordHexagon();
                hexagon.SetHorizontalLineAtIndex(0, "ban");
                hexagon.SetHorizontalLineAtIndex(1, "vice");
                hexagon.SetHorizontalLineAtIndex(2, "night");
                hexagon.SetHorizontalLineAtIndex(3, "isle");
                hexagon.SetHorizontalLineAtIndex(4, "bay");

                const string fileName = "test.xml";
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }
                hexagon.Serialize(fileName);
            }
        }
        [TestFixture]
        public class CalculateDiagonalWordPattern
        {
            [Test]
            public void ReturnsExpectedResults()
            {
                WordHexagon hexagon = new WordHexagon(4);
                hexagon.Lines[0] = "abcd";
                hexagon.Lines[1] = "efghi";
                hexagon.Lines[2] = "jklmno";
                hexagon.Lines[3] = "p9q*rst";
                hexagon.Lines[4] = "uvwxyz";
                hexagon.Lines[5] = "01234";
                hexagon.Lines[6] = "5678";

                Assert.AreEqual("pu05", hexagon.CalculateDiagonalWordPattern(0));
                Assert.AreEqual("j9v16", hexagon.CalculateDiagonalWordPattern(1));
                Assert.AreEqual("ekqw27", hexagon.CalculateDiagonalWordPattern(2));
                Assert.AreEqual("afl", hexagon.CalculateDiagonalWordPattern(3));
                Assert.AreEqual("x38", hexagon.CalculateDiagonalWordPattern(4));
                Assert.AreEqual("bgmry4", hexagon.CalculateDiagonalWordPattern(5));
                Assert.AreEqual("chnsz", hexagon.CalculateDiagonalWordPattern(6));
                Assert.AreEqual("diot", hexagon.CalculateDiagonalWordPattern(7));

            }
        }

        [TestFixture]
        public class CopyConstructor
        {
            [Test]
            public void CreatesSameDescription()
            {
                WordHexagon hexagon = new WordHexagon(4);
                hexagon.SetHorizontalLineAtIndex(2, "knight");
                hexagon.SetDiagonalLineAtIndex(1, "kaboo");
                hexagon.SetDiagonalLineAtIndex(6, "ashed");

                WordHexagon hexagonCopy = new WordHexagon(hexagon);
                Assert.AreEqual(hexagon.ToString(), hexagonCopy.ToString());
            }
        }

        [TestFixture]
        public class ValidateHorizontalLines
        {
            [Test]
            [TestCase(0, "kale")]
            [TestCase(1, "salad")]
            [TestCase(2, "knight")]
            [TestCase(3, "cat*dog")]
            [TestCase(4, "knight")]
            [TestCase(5, "salad")]
            [TestCase(6, "kale")]
            public void SingleValidLine_ReturnsTrue(int index, string line)
            {
                WordHexagon hexagon = new WordHexagon(4);
                Assert.IsTrue(hexagon.ValidateHorizontalLines());

                hexagon.Lines[index] = line;
                Assert.IsTrue(hexagon.ValidateHorizontalLines(), $"After setting {line} at index {index}, it should be valid.");
            }

            [Test]
            [TestCase(0, "xale")]
            [TestCase(1, "xalad")]
            [TestCase(2, "xnight")]
            [TestCase(3, "xat*dog")]
            [TestCase(4, "xnight")]
            [TestCase(5, "xalad")]
            [TestCase(6, "xale")]
            public void SingleInvalidLine_ReturnsFalse(int index, string line)
            {
                WordHexagon hexagon = new WordHexagon(4);
                Assert.IsTrue(hexagon.ValidateHorizontalLines());

                hexagon.Lines[index] = line;
                Assert.IsFalse(hexagon.ValidateHorizontalLines(), $"After setting {line} at index {index}, it should be invalid.");
            }

        }

        [TestFixture]
        public class ValidateDiagonalLines
        {
            [Test]
            [TestCase(0, "kale")]
            [TestCase(1, "salad")]
            [TestCase(2, "knight")]
            [TestCase(3, "cat")]
            [TestCase(4, "dog")]
            [TestCase(5, "knight")]
            [TestCase(6, "salad")]
            [TestCase(7, "kale")]
            public void SingleValidLine_ReturnsTrue(int index, string word)
            {
                WordHexagon hexagon = new WordHexagon(4);
                Assert.IsTrue(hexagon.ValidateDiagonalLines());

                hexagon.SetDiagonalLineAtIndex(index, word);
                Assert.IsTrue(hexagon.ValidateDiagonalLines(), $"After setting {word} at index {index}, it should be valid.");
            }

            [Test]
            [TestCase(0, "xale")]
            [TestCase(1, "xalad")]
            [TestCase(2, "xnight")]
            [TestCase(3, "xat")]
            [TestCase(4, "xog")]
            [TestCase(5, "xnight")]
            [TestCase(6, "xalad")]
            [TestCase(7, "xale")]
            public void SingleInvalidLine_ReturnsFalse(int index, string word)
            {
                WordHexagon hexagon = new WordHexagon(4);
                Assert.IsTrue(hexagon.ValidateHorizontalLines());

                hexagon.SetDiagonalLineAtIndex(index, word);
                Assert.IsFalse(hexagon.ValidateDiagonalLines(), $"After setting {word} at index {index}, it should be invalid.");
            }

        }

    }

}