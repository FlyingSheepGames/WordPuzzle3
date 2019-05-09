using System.Collections.Generic;
using NUnit.Framework;
using WordPuzzles;

namespace WordSquareGeneratorTest
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
        public class SetFirstLine
        {
            [Test]
            public void Taste_ReturnsExpectedResults()
            {
                WordSquare square = new WordSquare("taste");
                square.SetFirstLine("there");
                Assert.AreEqual(
                    @"there
ha___
e_s__
r__t_
e___e", square.ToString());
            }
        }

        [TestFixture]
        public class SetSecondLine
        {
            [Test]
            public void Taste_ReturnsExpectedResults()
            {
                WordSquare square = new WordSquare("taste");
                square.SetFirstLine("there");
                Assert.AreEqual(
                    @"there
ha___
e_s__
r__t_
e___e", square.ToString());
                square.SetSecondLine("happy");
                Assert.AreEqual(
                    @"there
happy
eps__
rp_t_
ey__e", square.ToString());
            }
        }

        [TestFixture]
        public class SetThirdLine
        {
            [Test]
            public void Taste_ReturnsExpectedResults()
            {
                WordSquare square = new WordSquare("taste");
                square.SetFirstLine("there");
                Assert.AreEqual(
                    @"there
ha___
e_s__
r__t_
e___e", square.ToString());
                square.SetSecondLine("happy");
                Assert.AreEqual(
                    @"there
happy
eps__
rp_t_
ey__e", square.ToString());

                square.SetThirdLine("epszq");
                Assert.AreEqual(
                    @"there
happy
epszq
rpzt_
eyq_e", square.ToString());
            }

        }

        [TestFixture]
        public class SetFourthLine
        {
            [Test]
            public void Taste_ReturnsExpectedResults()
            {
                WordSquare square = new WordSquare("taste");
                square.SetFirstLine("there");
                Assert.AreEqual(
                    @"there
ha___
e_s__
r__t_
e___e", square.ToString());
                square.SetSecondLine("happy");
                Assert.AreEqual(
                    @"there
happy
eps__
rp_t_
ey__e", square.ToString());

                square.SetThirdLine("epszq");
                Assert.AreEqual(
                    @"there
happy
epszq
rpzt_
eyq_e", square.ToString());

                square.SetFourthLine("rpztj");
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
                original.SetFirstLine("alpha");
                WordSquare copy = new WordSquare(original);
                original.SetSecondLine("beta2");
                copy.SetFirstLine("delta");

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
                square.SetFirstLine("guppy");
                square.SetSecondLine("uvula");
                square.SetThirdLine("purer");
                square.SetFourthLine("plead");
                square.SetFifthLine("yards");
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
                square.SetFirstLine("guppy");
                square.SetSecondLine("uvula");
                square.SetThirdLine("purer");
                square.SetFourthLine("plead");
                square.SetFifthLine("yards");
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
                square.SetFirstLine("guppy");
                square.SetSecondLine("uvula");
                square.SetThirdLine("purer");
                square.SetFourthLine("plead");
                square.SetFifthLine("yards");
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
                var results = WordSquare.ReadAllWordSquaresFromFile(@"data\magic.txt", 5);
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

    }

}
