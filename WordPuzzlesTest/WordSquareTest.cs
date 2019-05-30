using System.Collections.Generic;
using NUnit.Framework;
using WordPuzzles;

namespace WordPuzzlesTest
{
    [TestFixture]
    public class WordSquareTest
    {
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
        public void epszq_2_ReturnsExpectedResults()
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

            square.SetWordAtIndex("epszq", indexToSet);
            Assert.AreEqual(
                @"there
happy
epszq
rpzt_
eyq_e", square.ToString());
        }

        [Test]
        public void rpztj_3_ReturnsExpectedResults()
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

        }
    }

    [TestFixture]
    public class GetFirstWordCandidates
    {
        [Test]
        public void Taste_ReturnsExpectedNumberOfCandidates()
        {
            WordSquare square = new WordSquare("taste");
            List<string> result = square.GetFirstWordCandidates();
            Assert.LessOrEqual(300, result.Count);
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
        public void CanReadSize5File()
        {
            var results = WordSquare.ReadAllWordSquaresFromFile(@"data\magic.txt");
            Assert.AreEqual(5, results.Count, "Expected to read 5 word squares.");
        }

        [Test]
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
            WordSquare square = new WordSquare("____");
            square.SetWordAtIndex("shoe", 0);
            square.SetWordAtIndex("heal", 1);
            square.SetWordAtIndex("oaks", 2);
            square.SetWordAtIndex("else", 3);
            square.Clues[0] = "Something you wear on your foot.";
            square.Clues[1] = "Recover from an illness.";
            square.Clues[2] = "Trees that grow from acorns.";
            square.Clues[3] = "Clue for else.";

            const string EXPECTED_HTML =
                @"<html>
<body>
<!--StartFragment-->
Fill in the grid using the letters provided so that no letter appears twice in the same row or column.
<table border=""1"">
	<tr>
		<td>Something you wear on your foot.</td>
		<td> </td>
		<td> </td>
		<td> </td>
		<td> </td>
	</tr>
	<tr>
		<td>Recover from an illness.</td>
		<td> </td>
		<td> </td>
		<td> </td>
		<td> </td>
	</tr>
	<tr>
		<td>Trees that grow from acorns.</td>
		<td> </td>
		<td> </td>
		<td> </td>
		<td> </td>
	</tr>
	<tr>
		<td>Clue for else.</td>
		<td> </td>
		<td> </td>
		<td> </td>
		<td> </td>
	</tr>
</table>
<!--EndFragment-->
</body>
</html>
";
            Assert.AreEqual(EXPECTED_HTML, square.FormatHtmlForGoogle());
        }
    }

    [TestFixture]
    public class IsLastLineAWord
    {
        [Test]
        public void Example_ReturnsTrue()
        {
            WordSquare square = new WordSquare();
            Assert.IsFalse(square.IsLastLineAWord());
            square.SetWordAtIndex("words", 4);
            Assert.IsTrue(square.IsLastLineAWord());
        }
    }

    [TestFixture]
    public class GetFileNameFor
    {
        [Test]
        public void Example_ReturnsExpectedString()
        {
            Assert.AreEqual("wordsquares\\example.txt", WordSquare.GetFileNameFor("example"));
        }
    }
}


