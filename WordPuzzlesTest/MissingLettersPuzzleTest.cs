using System;
using System.Collections.Generic;
using NUnit.Framework;
using WordPuzzles;

namespace WordPuzzlesTest
{
    [TestFixture]
    public class MissingLettersPuzzleTest
    {
        [TestFixture]
        public class FindWordsContainingLetters
        {
            [Test]
            public void BAD_ReturnsExpectedResults()
            {
                MissingLettersPuzzle puzzle = new MissingLettersPuzzle();
                List<string> results = puzzle.FindWordsContainingLetters("bad");
                Console.WriteLine(string.Join(Environment.NewLine, results));
                Assert.AreEqual(3, results.Count);
            }

            [Test]
            public void HE_ReturnsExpectedResults()
            {
                MissingLettersPuzzle puzzle = new MissingLettersPuzzle();
                List<string> results = puzzle.FindWordsContainingLetters("he");
                Console.WriteLine(string.Join(Environment.NewLine, results));
                Assert.AreEqual(89, results.Count);
            }

        }

        [TestFixture]
        public class PlaceSolution
        {
            [Test]
            public void CreatesExpectedWords()
            {
                MissingLettersPuzzle puzzle = new MissingLettersPuzzle() {Shuffle = false};

                puzzle.PlaceSolution("he");
                Assert.AreEqual(7, puzzle.Words.Count);
                Assert.AreEqual("hem", puzzle.Words[0]);
                Assert.AreEqual("hen", puzzle.Words[1]);
                Assert.AreEqual("hey", puzzle.Words[2]);
                Assert.AreEqual("add", puzzle.Words[3]);
                Assert.AreEqual("adds", puzzle.Words[4]);
                Assert.AreEqual("amid", puzzle.Words[5]);
                Assert.AreEqual("camp", puzzle.Words[6]);
            }
        }

        [TestFixture]
        public class FormatHtmlForGoogle
        {
            [Test]
            public void HE_CreatesExpectedHtml()
            {
                const string EXPECTED_HTML =
                    @"<html>
<body>
<!--StartFragment-->
Fill in the blanks below with 2 letter words. The word that you use three times is the solution to the puzzle.<br>
__M<br>
__N<br>
__Y<br>
__D<br>
__DS<br>
__ID<br>
C__P<br>
Solution: _ _ 
<!--EndFragment-->
</body>
</html>
";
                MissingLettersPuzzle puzzle = new MissingLettersPuzzle() { Shuffle = false };
                puzzle.PlaceSolution("he");
                Assert.AreEqual(EXPECTED_HTML, puzzle.FormatHtmlForGoogle());
            }
        }
    }
}