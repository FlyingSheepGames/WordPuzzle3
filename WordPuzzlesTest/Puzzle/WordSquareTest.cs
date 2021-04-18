using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using WordPuzzles.Puzzle;

namespace WordPuzzlesTest.Puzzle
{
    [TestFixture]
    public class WordSquareTest
    {
        internal static WordSquare CreateShoePuzzle()
        {
            WordSquare square = new WordSquare("____");
            square.SetWordAtIndex("shoe", 0);
            square.SetWordAtIndex("heal", 1);
            square.SetWordAtIndex("oaks", 2);
            square.SetWordAtIndex("else", 3);
            square.Clues[0] = "Something you wear on your foot.";
            square.Clues[1] = "Recover from an illness.";
            square.Clues[2] = "Trees that grow from acorns.";
            square.Clues[3] = "Clue for else.";
            return square;
        }

        [TestFixture]
        public class Constructor
        {
            [Test]
            public void Taste_ReturnsExpectedResults()
            {
                WordSquare square = new WordSquare("taste");
                Assert.AreEqual(
                    @"t____
_a___
__s__
___t_
____e", square.ToString());
            }
        }

        [TestFixture]
        public class SetWordAtIndex
        {
            [Test]
            public void Taste_0_ReturnsExpectedResults()
            {
                WordSquare square = new WordSquare("taste");
                square.SetWordAtIndex("there", 0);
                Assert.AreEqual(
                    @"there
ha___
e_s__
r__t_
e___e", square.ToString());
            }

            [Test]
            public void Happy_1_ReturnsExpectedResults()
            {
                WordSquare square = new WordSquare("taste");
                square.SetWordAtIndex("there", 0);
                Assert.AreEqual(
                    @"there
ha___
e_s__
r__t_
e___e", square.ToString());
                square.SetWordAtIndex("happy", 1);
                Assert.AreEqual(
                    @"there
happy
eps__
rp_t_
ey__e", square.ToString());
            }
        }

        [Test]
        // ReSharper disable IdentifierTypo
        public void epszq_2_ReturnsExpectedResults()
            // ReSharper restore IdentifierTypo
        {
            WordSquare square = new WordSquare("taste");

            square.SetWordAtIndex("there", 0);
            Assert.AreEqual(
                @"there
ha___
e_s__
r__t_
e___e", square.ToString());

            var results = square.GetWordCandidates(0);
            Assert.LessOrEqual(0, results.Count, "Expected at least one word in first row.");

            square.SetWordAtIndex("happy", 1);
            Assert.AreEqual(
                @"there
happy
eps__
rp_t_
ey__e", square.ToString());

            int indexToSet = 2;


            // ReSharper disable StringLiteralTypo
            square.SetWordAtIndex("epszq", indexToSet);
            Assert.AreEqual(
                @"there
happy
epszq
rpzt_
eyq_e", square.ToString());
            // ReSharper restore StringLiteralTypo

            Assert.IsFalse(square.IsLastLineAWord(), "Last line is not a word.");
        }

        [Test]
        // ReSharper disable IdentifierTypo
        public void rpztj_3_ReturnsExpectedResults()
            // ReSharper restore IdentifierTypo
        {
            WordSquare square = new WordSquare("taste");
            square.SetWordAtIndex("there", 0);
            Assert.AreEqual(
                @"there
ha___
e_s__
r__t_
e___e", square.ToString());
            square.SetWordAtIndex("happy", 1);
            Assert.AreEqual(
                @"there
happy
eps__
rp_t_
ey__e", square.ToString());

            int indexToSet = 2;

            // ReSharper disable StringLiteralTypo
            square.SetWordAtIndex("epszq", indexToSet);
            Assert.AreEqual(
                @"there
happy
epszq
rpzt_
eyq_e", square.ToString());

            square.SetWordAtIndex("rpztj", 3);
            Assert.AreEqual(
                @"there
happy
epszq
rpztj
eyqje", square.ToString());
            // ReSharper restore StringLiteralTypo

        }
    }


    [TestFixture]
    public class CopyConstructor
    {
        [Test]
        public void CreatesDeepCopy()
        {
            WordSquare original = new WordSquare("_____");
            original.SetWordAtIndex("alpha", 0);
            WordSquare copy = new WordSquare(original);
            original.SetWordAtIndex("beta2", 1);
            copy.SetWordAtIndex("delta", 0);
        }
    }

    [TestFixture]
    public class GetTweet
    {
        [Test]
        public void Default_ProducesExpectedText()
        {
            WordSquare square = new WordSquare("_____");
            const string EXPECTED_STRING =
                @"#MagicWordSquare

_****
*****
*****
*****
*****

Top word is part of this week's theme!
Remaining (unordered) clues:





#HowToPlay: https://t.co/rSa0rUCvRC
";
            Assert.AreEqual(EXPECTED_STRING, square.GetTweet());
        }

        [Test]
        public void Example_ProducesExpectedText()
        {
            WordSquare square = new WordSquare("_____");
            square.SetWordAtIndex("guppy", 0);
            square.SetWordAtIndex("uvula", 1);
            int indexToSet = 2;

            square.SetWordAtIndex("purer", indexToSet);
            square.SetWordAtIndex("plead", 3);
            square.SetWordAtIndex("yards", 4);
            square.Clues[1] = "Thing that hangs above your throat";
            square.Clues[2] = "Less tainted";
            square.Clues[3] = "Beg";
            square.Clues[4] = "Measurements of distance";

            const string EXPECTED_STRING =
                @"#MagicWordSquare

G****
*****
*****
*****
*****

Top word is part of this week's theme!
Remaining (unordered) clues:
Beg
Less tainted
Measurements of distance
Thing that hangs above your throat

#HowToPlay: https://t.co/rSa0rUCvRC
";
            Assert.AreEqual(EXPECTED_STRING, square.GetTweet());
        }

        [Test]
        public void NoHashtagTheme_ProducesExpectedText()
        {
            WordSquare square = new WordSquare("_____");
            square.SetWordAtIndex("guppy", 0);
            square.SetWordAtIndex("uvula", 1);
            int indexToSet = 2;

            square.SetWordAtIndex("purer", indexToSet);
            square.SetWordAtIndex("plead", 3);
            square.SetWordAtIndex("yards", 4);
            square.Clues[1] = "Thing that hangs above your throat";
            square.Clues[2] = "Less tainted";
            square.Clues[3] = "Beg";
            square.Clues[4] = "Measurements of distance";
            square.Theme = "NoHashtag";
            const string EXPECTED_STRING =
                @"#MagicWordSquare

G****
*****
*****
*****
*****

Top word is part of this week's theme!
Remaining (unordered) clues:
Beg
Less tainted
Measurements of distance
Thing that hangs above your throat

#HowToPlay: https://t.co/rSa0rUCvRC
";
            Assert.AreEqual(EXPECTED_STRING, square.GetTweet());
        }

        [Test]
        public void HashtagTheme_ProducesExpectedText()
        {
            WordSquare square = new WordSquare("_____");
            square.SetWordAtIndex("guppy", 0);
            square.SetWordAtIndex("uvula", 1);
            int indexToSet = 2;

            square.SetWordAtIndex("purer", indexToSet);
            square.SetWordAtIndex("plead", 3);
            square.SetWordAtIndex("yards", 4);
            square.Clues[1] = "Thing that hangs above your throat";
            square.Clues[2] = "Less tainted";
            square.Clues[3] = "Beg";
            square.Clues[4] = "Measurements of distance";
            square.Theme = "#ThemeWeek";
            const string EXPECTED_STRING =
                @"#ThemeWeek
#MagicWordSquare

G****
*****
*****
*****
*****

Top word is part of this week's theme!
Remaining (unordered) clues:
Beg
Less tainted
Measurements of distance
Thing that hangs above your throat

#HowToPlay: https://t.co/rSa0rUCvRC
";
            Assert.AreEqual(EXPECTED_STRING, square.GetTweet());
        }

    }

    [TestFixture]
    public class ReadAllWordSquaresFromFile
    {
        [Test]
        [Ignore("Takes more than 3 seconds.")]
        public void CanReadSize5File()
        {
            var results = WordSquare.ReadAllWordSquaresFromFile(@"data\magic.txt");
            Assert.AreEqual(5, results.Count, "Expected to read 5 word squares.");
        }

        [Test]
        //[Ignore("Takes more than 3 seconds.")] //Covers a lot of the class.
        public void CanReadSize4File()
        {
            var results = WordSquare.ReadAllWordSquaresFromFile(@"data\area.txt", 4);
            Assert.AreEqual(5, results.Count, "Expected to read 5 word squares.");
        }
    }

    [TestFixture]
    public class FormatForGoogle
    {
        [Test]
        public void SHOE_ReturnsExpectedResult()
        {
            WordSquare square = new WordSquare("____");
            square.SetWordAtIndex("shoe", 0);
            square.SetWordAtIndex("heal", 1);
            square.SetWordAtIndex("oaks", 2);
            square.SetWordAtIndex("else", 3);
            square.Clues[0] = "Something you wear on your foot.";
            square.Clues[1] = "Recover from an illness.";
            square.Clues[2] = "Trees that grow from acorns.";
            square.Clues[3] = "Clue for else.";

            const string EXPECTED_TEXT =
                "Something you wear on your foot.\t\t\t\t\r\nRecover from an illness.\t\t\t\t\r\nTrees that grow from acorns.\t\t\t\t\r\nClue for else.\t\t\t\t\r\n";
            Assert.AreEqual(EXPECTED_TEXT, square.FormatForGoogle());
        }
    }

    [TestFixture]
    public class FormatHtmlForGoogle
    {
        [Test]
        public void SHOE_ReturnsExpectedResult()
        {
            const string HTML_DIRECTORY = @"html\WordSquares\";
            string SOURCE_DIRECTORY = ConfigurationManager.AppSettings["SourceDirectory"] + "WordSquares";

            var square = WordSquareTest.CreateShoePuzzle();

            string generatedHtml = square.FormatHtmlForGoogle();

            File.WriteAllText(HTML_DIRECTORY + "actualExample1.html", generatedHtml);
            var expectedLines = File.ReadAllLines(HTML_DIRECTORY + "expectedExample1.html");
            var actualLines = File.ReadAllLines(HTML_DIRECTORY + "actualExample1.html");
            bool anyLinesDifferent = false;
            for (var index = 0; index < expectedLines.Length; index++)
            {
                string expectedLine = expectedLines[index];
                string actualLine = "End of file already reached.";
                if (index >= 0 && actualLines.Length > index)
                {
                    actualLine = actualLines[index];
                }

                if (expectedLine != actualLine)
                {
                    anyLinesDifferent = true;
                    Console.WriteLine($"Expected Line {index}:{expectedLine}");
                    Console.WriteLine($"  Actual Line {index}:{expectedLine}");
                }
            }

            if (anyLinesDifferent)
            {
                Console.WriteLine("Updating source file. Will show up as a difference in source control.");
                File.WriteAllLines(SOURCE_DIRECTORY + @"\expectedExample1.html", actualLines);
            }
            Assert.IsFalse(anyLinesDifferent, "Didn't expect any lines to be different.");

        }

    }

    [TestFixture]
    public class IsLastLineAWord
    {
        [Test]
        [Ignore("Takes more than 3 seconds.")]
        public void Example_ReturnsTrue()
        {
            WordSquare square = new WordSquare();
            Assert.IsFalse(square.IsLastLineAWord());
            square.SetWordAtIndex("words", 4);
            Assert.IsTrue(square.IsLastLineAWord());
        }
    }

    [TestFixture]
    public class GetClues
    {
        [Test]
        public void ReturnsExpectedResults()
        {
            var puzzle = WordSquareTest.CreateShoePuzzle();
            CollectionAssert.AreEqual(new List<string>()
                {
                    "Something you wear on your foot.",
                    "Recover from an illness.",
                    "Trees that grow from acorns.",
                    "Clue for else.",
                }, puzzle.GetClues());
        }
    }

    [TestFixture]
    public class ReplaceClue
    {
        [Test]
        public void ReturnsExpectedResults()
        {
            var puzzle = WordSquareTest.CreateShoePuzzle();
            puzzle.ReplaceClue("Clue for else.", "updated clue");
            CollectionAssert.AreEqual(
                new List<string>()
                {
                    "Something you wear on your foot.",
                    "Recover from an illness.",
                    "Trees that grow from acorns.",
                    "updated clue",
                    
                },
                puzzle.GetClues());
        }
    }

    [TestFixture]
    public class GenerateJsonFileForMonty
    {
        [Test]
        public void Puzzle_4_2_Generates_ExpectedFile()
        {
            string expectedSerializedPuzzle = File.ReadAllText(@"data\json\puzzle03.json");
            JObject expectedJObject = JObject.Parse(expectedSerializedPuzzle);

            PuzzlePyramid pyramid = JsonConvert.DeserializeObject<PuzzlePyramid>(File.ReadAllText(@"C:\utilities\WordSquare\data\basic\pyramids\4-2.json"));

            WordSquare puzzle = pyramid.PuzzleC as WordSquare;
            JObject actualJObject = puzzle.GenerateJsonFileForMonty("Puzzle C");
            Assert.AreEqual((string)expectedJObject["name"], (string)actualJObject["name"], "Unexpected value for name");
            Assert.AreEqual((string)expectedJObject["type"], (string)actualJObject["type"], "Unexpected value for type");
            Assert.AreEqual((string)expectedJObject["directions"], (string)actualJObject["directions"], "Unexpected value for directions");
            Assert.AreEqual((string)expectedJObject["final_answer"], (string)actualJObject["final_answer"], "Unexpected value for final_answer");

            Assert.AreEqual((string)expectedJObject["first_letter"], (string)actualJObject["first_letter"], "Unexpected value for first_letter");

            AssertArraysMatch(expectedJObject, actualJObject, "clues");


        }

        private static void AssertArraysMatch(JObject expectedJObject, JObject actualJObject, string arrayName)
        {
            var token = actualJObject[arrayName];
            if (token == null)
            {
                Assert.Fail("Actual list for " + arrayName + " was null.");
                return;
            }

            var actualList = token.ToList();
            var expectedList = expectedJObject[arrayName].ToList();
            CollectionAssert.AreEqual(expectedList, actualList,
                "Unexpected value for " + arrayName);
        }
    }

}


