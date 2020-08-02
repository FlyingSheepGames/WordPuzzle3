using System;
using System.Collections.Generic;
using NUnit.Framework;
using WordPuzzles.Puzzle.Legacy;
using WordPuzzles.Utility;

namespace WordPuzzlesTest.Puzzle.Legacy
{
    [TestFixture]
    public class MissingLettersPuzzleTest
    {
        [TestFixture]
        public class FindWordsContainingLetters
        {
            [Test]
            [Ignore("Takes more than 3 seconds.")]
            public void BAD_ReturnsExpectedResults()
            {
                MissingLettersPuzzle puzzle = new MissingLettersPuzzle();
                List<string> results = puzzle.FindWordsContainingLetters("bad");
                Console.WriteLine(string.Join(Environment.NewLine, results));
                Assert.AreEqual(3, results.Count);
            }

            [Test]
            [Ignore("Takes more than 3 seconds.")]
            public void HE_ReturnsExpectedResults()
            {
                MissingLettersPuzzle puzzle = new MissingLettersPuzzle();
                List<string> results = puzzle.FindWordsContainingLetters("he");
                Console.WriteLine(string.Join(Environment.NewLine, results));
                Assert.AreEqual(89, results.Count);
            }

            /*
ace has 11 words with these letters.
age has 15 words with these letters.
air has 10 words with these letters.
ale has 15 words with these letters.
and has 13 words with these letters.
ape has 12 words with these letters.
art has 11 words with these letters.
bar has 13 words with these letters.
bee has 10 words with these letters.
bus has 13 words with these letters.
car has 21 words with these letters.
chi has 14 words with these letters.
din has 10 words with these letters.
eat has 12 words with these letters.
eve has 11 words with these letters.
for has 19 words with these letters.
hum has 10 words with these letters.
ink has 11 words with these letters.
oar has 10 words with these letters.
one has 14 words with these letters.
owe has 16 words with these letters.
par has 24 words with these letters.
pea has 11 words with these letters.
pin has 20 words with these letters.
rat has 12 words with these letters.
see has 10 words with these letters.
she has 13 words with these letters.
sin has 11 words with these letters.
ski has 13 words with these letters.
tea has 11 words with these letters.
tin has 13 words with these letters.
use has 14 words with these letters.
war has 23 words with these letters.
win has 22 words with these letters.
             */
            [Test]
            [Ignore("Exploratory test")]
            public void WhichThreeLetterWordsHaveMoreThan10Options()
            {
                MissingLettersPuzzle puzzle = new MissingLettersPuzzle();
                WordRepository repository = new WordRepository() {ExludeAdvancedWords = true};
                foreach (string word in repository.WordsMatchingPattern("___"))
                {
                    int count = puzzle.FindWordsContainingLetters(word).Count;
                    if (9 < count)
                    {
                        Console.WriteLine($"{word} has {count} words with these letters.");
                    }
                }
            }
        }

        [TestFixture]
        public class PlaceSolution
        {
            [Test]
            [Ignore("Takes more than 3 seconds.")]
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
            [Ignore("Takes more than 3 seconds.")]
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