using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using WordPuzzles;

namespace WordPuzzlesTest
{
    [TestFixture]
    public class LettersAndArrowsPuzzleTest
    {

        [TestFixture]
        public class StringConstructor
        {
            [Test]
            [TestCase(3, "abc")]
            [TestCase(3, "abcdefg")]
            [TestCase(4, "abcdefghi")]
            [TestCase(4, "abcdefghijkl")] //12
            [TestCase(5, "abcdefghijklm")] //13
            [TestCase(5, "abcdefghijklmnopqr")] //18
            [TestCase(6, "abcdefghijklmnopqrs")] //19
            [TestCase(6, "abcdefghijklmnopqrstuvwx")] //24
            [TestCase(7, "abcdefghijklmnopqrstuvwxy")] //25
            public void SetsAppropriateSize(int expectedSize, string solution)
            {
                LettersAndArrowsPuzzle puzzle = new LettersAndArrowsPuzzle(solution);
                Assert.AreEqual(expectedSize, puzzle.Size);
            }

            [Test]
            public void TooLong_ThrowsException()
            {
                Assert.Throws<ArgumentException>(()=> new LettersAndArrowsPuzzle("abcdefghijklmnopqrstuvwxyz123456"));
            }

            [Test]
            [TestCase("abc")]
            [TestCase("abcdef")]
            [TestCase("abcdefg")]
            [TestCase("abcdefghijkl")]
            [TestCase("abcdefghijklm")]
            [TestCase("abcdefghijklmnopqrst")]
            [TestCase("abcdefghijklmnopqrstu")]
            public void PopulatesExpectedLetters(string solution)
            {
                LettersAndArrowsPuzzle puzzle = new LettersAndArrowsPuzzle(solution);
                List<char> expectedLetters = new List<char>();
                foreach (char letter in solution.ToUpper())
                {
                    expectedLetters.Add(letter);
                }

                List<char> actualLetters = new List<char>();
                for (int row = 0; row < puzzle.Size; row++)
                {
                    for (int column = 0; column < puzzle.Size; column++)
                    {
                        char letterToAdd = puzzle.GetCellAtCoordinates(row, column).Letter;
                        if (letterToAdd != ' ')
                        {
                            actualLetters.Add(letterToAdd);
                        }
                    }
                }
                CollectionAssert.IsSubsetOf(expectedLetters, actualLetters);
            }

            [Test]
            public void OHIO_RowsAsWords_CreatesWords()
            {
                LettersAndArrowsPuzzle puzzle = new LettersAndArrowsPuzzle("ohio", true);
                WordRepository repository = new WordRepository();
                Assert.AreEqual(5, puzzle.Size);
                StringBuilder builder = new StringBuilder();
                for (int row = 0; row < 5; row++)
                {
                    builder.Clear();
                    for (int column = 0; column < 5; column++)
                    {
                        builder.Append(puzzle.GetCellAtCoordinates(row, column).Letter);
                    }

                    string wordCandidate = builder.ToString().ToLower();
                    Assert.IsTrue(repository.IsAWord(wordCandidate), $"Expected '{wordCandidate}' to be a word");

                }

                Console.WriteLine(puzzle.FormatHtmlForGoogle());

            }

            [Test]
            public void OverwriteSize_HasThatSize()
            {
                LettersAndArrowsPuzzle puzzle = new LettersAndArrowsPuzzle("ohio", false, 8);
                Assert.AreEqual(8, puzzle.Size);
            }
        }

        [TestFixture]
        public class GetCellAtCoordinates
        {
            [Test]
            public void Empty4x4_CreatesExpectedObject()
            {
                LettersAndArrowsPuzzle puzzle = new LettersAndArrowsPuzzle(4);
                Assert.AreEqual(4, puzzle.Size);
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        Assert.AreEqual(LetterAndArrowCell.EmptyCell, puzzle.GetCellAtCoordinates(i, j));
                    }
                }
            }
        }

        [TestFixture]
        public class GetAvailableHorizontalCells
        {
            [Test]
            public void Empty4x4_FromOrigin_ReturnsCorrectNumbers()
            {
                LettersAndArrowsPuzzle puzzle = new LettersAndArrowsPuzzle(4);

                List<int> expectedNumbers = new List<int>() {1, 2, 3};

                CollectionAssert.AreEquivalent(expectedNumbers, puzzle.GetAvailableHorizontalCells(0, 0));
            }

            [Test]
            public void Empty5x5_FromCenter_ReturnsCorrectNumbers()
            {
                LettersAndArrowsPuzzle puzzle = new LettersAndArrowsPuzzle(5);

                List<int> expectedNumbers = new List<int>() { -2, -1, 1, 2 };

                CollectionAssert.AreEquivalent(expectedNumbers, puzzle.GetAvailableHorizontalCells(2, 2));
            }

            [Test]
            public void NonEmpty5x5_FromCenter_ReturnsCorrectNumbers()
            {
                LettersAndArrowsPuzzle puzzle = new LettersAndArrowsPuzzle(5);

                puzzle.SetCellAtCoordinates(2, 0, new LetterAndArrowCell() {Letter = 'A'});
                List<int> expectedNumbers = new List<int>() { -1, 1, 2 };

                CollectionAssert.AreEquivalent(expectedNumbers, puzzle.GetAvailableHorizontalCells(2, 2));
            }
        }

        [TestFixture]
        public class PlaceSolution
        {
            [Test]
            public void SingleLetter_PlacesExpectedCell()
            {
                const int SIZE = 3;
                LettersAndArrowsPuzzle puzzle = new LettersAndArrowsPuzzle(SIZE);

                puzzle.PlaceSolution("a");

                var populatedCell = puzzle.GetCellAtCoordinates(0, 0);
                Assert.AreEqual('A', populatedCell.Letter);
                Assert.AreEqual(0, populatedCell.Number);
                Assert.AreEqual(Direction.Undefined, populatedCell.Direction);

                for (int row = 0; row < SIZE; row++)
                {
                    for (int column = 0; column < SIZE; column++)
                    {
                        if (row == 0 && column == 0) continue; //all other cells should be empty.
                        Assert.AreEqual(LetterAndArrowCell.EmptyCell, puzzle.GetCellAtCoordinates(row, column));
                    }
                }
            }

            [Test]
            public void TwoLetters_PlacesExpectedCells()
            {
                const int SIZE = 3;
                LettersAndArrowsPuzzle puzzle = new LettersAndArrowsPuzzle(SIZE);

                puzzle.PlaceSolution("ab");

                var cellWithA = puzzle.GetCellAtCoordinates(0, 0);
                Assert.AreEqual('A', cellWithA.Letter);
                int offset = cellWithA.Number;
                Assert.AreNotEqual(0, offset);
                Direction directionForB = cellWithA.Direction;
                Assert.AreNotEqual(Direction.Undefined, directionForB);
                int expectedRowForB = 0;
                int expectedColumnForB = 0;

                switch (directionForB)
                {
                    case Direction.Down:
                        expectedRowForB += offset;
                        break;
                    case Direction.Right:   //always goes down, so this isn't necessary.
                        expectedColumnForB += offset;
                        break;
                    case Direction.Up:  
                        throw new Exception("Starting at 0, 0, the next direction should not be up.");
                    case Direction.Left:
                        throw new Exception("Starting at 0, 0, next direction should not be left.");
                }

                for (int row = 0; row < SIZE; row++)
                {
                    for (int column = 0; column < SIZE; column++)
                    {
                        if (row == 0 && column == 0) continue; //all other cells should be empty.
                        if (row == expectedRowForB && column == expectedColumnForB)
                        {
                            LetterAndArrowCell actualCellWhereBShouldBe = puzzle.GetCellAtCoordinates(row, column);
                            Assert.AreEqual('B', actualCellWhereBShouldBe.Letter);
                            Assert.AreEqual(0, actualCellWhereBShouldBe.Number);
                            Assert.AreEqual(Direction.Undefined, actualCellWhereBShouldBe.Direction);
                        }
                        else
                        {
                            Assert.AreEqual(LetterAndArrowCell.EmptyCell, puzzle.GetCellAtCoordinates(row, column));
                        }
                    }
                }
            }

            [Test]
            public void ABCD_ReturnsExpectedGrid()
            {
                LettersAndArrowsPuzzle puzzle = new LettersAndArrowsPuzzle(2);
                puzzle.PlaceSolution("abcd");

                /*
                 * A D
                 * B C
                 */
                LetterAndArrowCell actualA = puzzle.GetCellAtCoordinates(0, 0);
                Assert.AreEqual('A', actualA.Letter);
                Assert.AreEqual(1, actualA.Number);
                Assert.AreEqual(Direction.Down, actualA.Direction);

                LetterAndArrowCell actualB = puzzle.GetCellAtCoordinates(1, 0);
                Assert.AreEqual('B', actualB.Letter);
                Assert.AreEqual(1, actualB.Number);
                Assert.AreEqual(Direction.Right, actualB.Direction);

                LetterAndArrowCell actualC = puzzle.GetCellAtCoordinates(1, 1);
                Assert.AreEqual('C', actualC.Letter);
                Assert.AreEqual(-1, actualC.Number);
                Assert.AreEqual(Direction.Up, actualC.Direction);

                LetterAndArrowCell actualD = puzzle.GetCellAtCoordinates(0, 1);
                Assert.AreEqual('D', actualD.Letter);
                Assert.AreEqual(0, actualD.Number);
                Assert.AreEqual(Direction.Undefined, actualD.Direction);
            }

            [Test]
            public void FiveLetters_VisitsAllRows()
            {
                LettersAndArrowsPuzzle puzzle = new LettersAndArrowsPuzzle(3) {RandomSeed = 1};
                puzzle.PlaceSolution("12345");
                Console.WriteLine(puzzle.FormatHtmlForGoogle());
                //The third box (in the center) should go down, not up. 
                Assert.AreEqual(Direction.Down, puzzle.GetCellAtCoordinates(1, 1).Direction);
            }
        }

        [TestFixture]
        public class FormatHtmlForGoogle
        {
            [Test]
            public void Empty2x2_ReturnsExpectedString()
            {
                LettersAndArrowsPuzzle puzzle = new LettersAndArrowsPuzzle(2);

                const string EXPECTED_HTML =
@"<html>
<body>
<!--StartFragment-->
Starting in the top left box, follow the direction (e.g. three spaces to the right) to find the next letter. 
<table border=""1"">
<tr>
    <td> </td>
    <td> </td>
</tr>
<tr>
    <td> </td>
    <td> </td>
</tr>
</table>
<!--EndFragment-->
</body>
</html>
";
                string actualHtml = puzzle.FormatHtmlForGoogle();
                Assert.AreEqual(EXPECTED_HTML, actualHtml);
            }

            [Test]
            public void SingleLetter_ReturnsExpectedString()
            {
                LettersAndArrowsPuzzle puzzle = new LettersAndArrowsPuzzle(2);
                puzzle.PlaceSolution("a");
                const string EXPECTED_HTML =
                    @"<html>
<body>
<!--StartFragment-->
Starting in the top left box, follow the direction (e.g. three spaces to the right) to find the next letter. 
<table border=""1"">
<tr>
    <td>A</td>
    <td> </td>
</tr>
<tr>
    <td> </td>
    <td> </td>
</tr>
</table>
<!--EndFragment-->
</body>
</html>
";
                string actualHtml = puzzle.FormatHtmlForGoogle();
                Assert.AreEqual(EXPECTED_HTML, actualHtml);
            }

            [Test]
            public void FourLetters_ReturnsExpectedString()
            {
                LettersAndArrowsPuzzle puzzle = new LettersAndArrowsPuzzle(2);
                puzzle.PlaceSolution("abcd");
                const string EXPECTED_HTML_OPTION_ONE =
                    @"<html>
<body>
<!--StartFragment-->
Starting in the top left box, follow the direction (e.g. three spaces to the right) to find the next letter. 
<table border=""1"">
<tr>
    <td>A 1↓</td>
    <td>D</td>
</tr>
<tr>
    <td>B 1→</td>
    <td>C 1↑</td>
</tr>
</table>
<!--EndFragment-->
</body>
</html>
";

                string actualHtml = puzzle.FormatHtmlForGoogle();

                Assert.AreEqual(EXPECTED_HTML_OPTION_ONE, actualHtml);

            }

        }

        [TestFixture]
        public class FillEmptyCells
        {
            [Test]
            public void LeavesNoEmptyCells()
            {
                LettersAndArrowsPuzzle puzzle = new LettersAndArrowsPuzzle(5);
                puzzle.FillEmptyCells();

                for (int row = 0; row < 5; row++)
                {
                    for (int column = 0; column < 5; column++)
                    {
                        var cell = puzzle.GetCellAtCoordinates(row, column);
                        Assert.AreNotEqual(' ', cell.Letter);
                        Assert.AreNotEqual(0, cell.Number);
                        Assert.AreNotEqual(Direction.Undefined, cell.Direction);
                    }
                }
            }
        }
    }
}