using System;
using System.Collections.Generic;
using NUnit.Framework;
using WordPuzzles;

namespace WordPuzzlesTest
{
    [TestFixture]
    public class ReadDownColumnPuzzleTest
    {
        [TestFixture]
        public class PopulateWords
        {
            [Test]
            public void CreatesExpectedPuzzle()
            {
                ReadDownColumnPuzzle puzzle = new ReadDownColumnPuzzle {Solution = "cat"};

                puzzle.PopulateWords();

                int currentIndex = 0;
                foreach (string word in puzzle.Words)
                {
                    Console.WriteLine(puzzle.Words[currentIndex]);
                    Assert.AreEqual(puzzle.Solution[currentIndex], word[2], $"Expected character at index {currentIndex}");
                    currentIndex++;
                }
            }

            [Test]
            public void SpacesInPhrase_CreatesExpectedPuzzle()
            {
                ReadDownColumnPuzzle puzzle = new ReadDownColumnPuzzle {Solution = "cats and dogs"};
                var puzzleSolution = "catsanddogs";

                puzzle.PopulateWords();

                int currentIndex = 0;
                foreach (string word in puzzle.Words)
                {
                    Console.WriteLine(puzzle.Words[currentIndex]);
                    Assert.AreEqual(puzzleSolution[currentIndex], word[2], $"Expected character at index {currentIndex}");
                    currentIndex++;
                }
            }
        }

        [TestFixture]
        public class FormatHtmlForGoogle
        {
            [Test]
            public void CAT_ReturnsExpectedResult()
            {
                ReadDownColumnPuzzle puzzle = new ReadDownColumnPuzzle
                {
                    Solution = "cat",
                    Words = new List<string>() {"pacing", "shaved", "metric"}
                };

                const string EXPECTED_HTML =
@"<html>
<body>
<!--StartFragment-->
Fill in the clues below, and then read the solution down the third column. 
<table border=""1"">
<tr>
    <td>Clue for pacing</td>
    <td> </td>
    <td> </td>
    <td> </td>
    <td> </td>
    <td> </td>
    <td> </td>
</tr>
<tr>
    <td>Clue for shaved</td>
    <td> </td>
    <td> </td>
    <td> </td>
    <td> </td>
    <td> </td>
    <td> </td>
</tr>
<tr>
    <td>Clue for metric</td>
    <td> </td>
    <td> </td>
    <td> </td>
    <td> </td>
    <td> </td>
    <td> </td>
</tr>
</table>
Solution: _ _ _ 
<!--EndFragment-->
</body>
</html>
";

                string actualResult = puzzle.FormatHtmlForGoogle();
                Console.WriteLine(actualResult);
                Assert.AreEqual(EXPECTED_HTML, actualResult);
            }
            [Test]
            public void XRAY_ReturnsExpectedResult()
            {
                ReadDownColumnPuzzle puzzle = new ReadDownColumnPuzzle {Solution = "x-ray"};
                puzzle.PopulateWords();
                puzzle.Words = new List<string>() { "boxing", "parent", "frayed", "joyful" };

                const string EXPECTED_HTML =
                    @"<html>
<body>
<!--StartFragment-->
Fill in the clues below, and then read the solution down the third column. 
<table border=""1"">
<tr>
    <td>Clue for boxing</td>
    <td> </td>
    <td> </td>
    <td> </td>
    <td> </td>
    <td> </td>
    <td> </td>
</tr>
<tr>
    <td>Clue for parent</td>
    <td> </td>
    <td> </td>
    <td> </td>
    <td> </td>
    <td> </td>
    <td> </td>
</tr>
<tr>
    <td>Clue for frayed</td>
    <td> </td>
    <td> </td>
    <td> </td>
    <td> </td>
    <td> </td>
    <td> </td>
</tr>
<tr>
    <td>Clue for joyful</td>
    <td> </td>
    <td> </td>
    <td> </td>
    <td> </td>
    <td> </td>
    <td> </td>
</tr>
</table>
Solution: _ -_ _ _ 
<!--EndFragment-->
</body>
</html>
";

                string actualResult = puzzle.FormatHtmlForGoogle();
                Console.WriteLine(actualResult);
                Assert.AreEqual(EXPECTED_HTML, actualResult);
            }
        }

    }
}