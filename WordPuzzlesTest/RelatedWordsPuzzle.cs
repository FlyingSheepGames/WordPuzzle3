using System;
using NUnit.Framework;
using WordPuzzles;

namespace WordPuzzlesTest
{
    [TestFixture]
    public class RelatedWordsPuzzleTest
    {
        [TestFixture]
        public class PlaceSolution
        {
            [Test]
            public void BLUE_CreatesExpectedPuzzle()
            {
                RelatedWordsPuzzle puzzle = new RelatedWordsPuzzle();
                puzzle.PlaceSolution("color", "blue");
                Console.WriteLine(string.Join(Environment.NewLine, puzzle.Words));
                Assert.AreEqual(4, puzzle.Words.Count);
                Assert.IsTrue(puzzle.Words[0].Contains("b"));
                Assert.IsTrue(puzzle.Words[1].Contains("l"));
                Assert.IsTrue(puzzle.Words[2].Contains("u"));
                Assert.IsTrue(puzzle.Words[3].Contains("e"));
            }
        }

        [TestFixture]
        public class FormatHtmlForGoogle
        {
            [Test]
            public void BLUE_CreatesExpectedHtml()
            {
                const string EXPECTED_HTML =
                    @"<html>
<body>
<!--StartFragment-->
Construct a word that fits in the same category as the words below by taking one letter from each word, in order.<br>
AMBER<br>
GOLD<br>
MAUVE<br>
RED<br>
Solution: _ _ _ _ 
<!--EndFragment-->
</body>
</html>
";
                RelatedWordsPuzzle puzzle = new RelatedWordsPuzzle() {Shuffle = false};
                puzzle.PlaceSolution("color", "blue");
                Assert.AreEqual(EXPECTED_HTML, puzzle.FormatHtmlForGoogle());
            }
        }

    }
}