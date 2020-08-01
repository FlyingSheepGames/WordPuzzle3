using System;
using System.Collections.Generic;
using NUnit.Framework;
using WordPuzzles.Puzzle.Legacy;

namespace WordPuzzlesTest.NetFramework.Puzzle.Legacy
{
    [TestFixture]
    public class BuildingBlocksPuzzleTest
    {
        [TestFixture]
        public class PlaceSolution
        {
            [Test]
            public void GeneratesListOf6LetterWords()
            {
                BuildingBlocksPuzzle puzzle = new BuildingBlocksPuzzle();
                puzzle.RandomSeed = 1;
                const string SOLUTION = "word";
                puzzle.PlaceSolution(SOLUTION);
                Assert.AreEqual(2, puzzle.ColumnContainingSolution);//Third column should spell out the word.
                Assert.AreEqual(4, puzzle.Words.Count, "Expected four words.");
                for (var index = 0; index < SOLUTION.Length; index++)
                {
                    char letter = SOLUTION[index];
                    Console.WriteLine(puzzle.Words[index]);
                    Assert.AreEqual(letter, puzzle.Words[index][puzzle.ColumnContainingSolution], $"Expected letter #{puzzle.ColumnContainingSolution} word #{index} ('{puzzle.Words[index]}') to be letter '{letter}'");
                }
                //Words are "coward", "grouse", "streak", and "module". 
            }

            [Test]
            public void CreatesExpectedBlocks()
            {
                BuildingBlocksPuzzle puzzle = new BuildingBlocksPuzzle();
                puzzle.RandomSeed = 1;
                const string SOLUTION = "word";
                puzzle.PlaceSolution(SOLUTION);

                CollectionAssert.AreEqual(new List<string>()
                {
                    "ak",
                    "du",
                    "le",
                    "ou",
                    "rd",
                    "re",
                    "se",
                    "wa",
                }, puzzle.Blocks );
                
                //Words are "coward", "grouse", "streak", and "module". 
            }

        }

        [TestFixture]
        public class FormatHtmlForGoogle
        {

            [Test]
            public void ReturnsExpectedString()
            {
                BuildingBlocksPuzzle puzzle = new BuildingBlocksPuzzle();
                puzzle.RandomSeed = 1;
                const string SOLUTION = "word";
                puzzle.PlaceSolution(SOLUTION);
                const string EXPECTED_HTML =
                    @"<html>
<body>
<!--StartFragment-->
Using the letter pairs listed below, complete the words in the grid. 
To get the solution, read down one of the columns. 
<table border=""1"">
<tr>
    <td>CO</td>
    <td> </td>
    <td> </td>
</tr>
<tr>
    <td>GR</td>
    <td> </td>
    <td> </td>
</tr>
<tr>
    <td>ST</td>
    <td> </td>
    <td> </td>
</tr>
<tr>
    <td>MO</td>
    <td> </td>
    <td> </td>
</tr>
</table>
Available blocks: <br>
AK<br>
DU<br>
LE<br>
OU<br>
RD<br>
RE<br>
SE<br>
WA<br>
Solution: _ _ _ _ 
<!--EndFragment-->
</body>
</html>
";
            Assert.AreEqual(EXPECTED_HTML, puzzle.FormatHtmlForGoogle());
            }
        }
    }
}