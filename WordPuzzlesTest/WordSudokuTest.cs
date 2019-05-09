using System;
using System.Collections.Generic;
using NUnit.Framework;
using WordPuzzles;

namespace WordPuzzlesTest
{
    [TestFixture]
    public class WordSudokuTest
    {
        [TestFixture]
        public class Constructor
        {
            [Test]
            public void WithDuplicateLetters_Throws()
            {
                Assert.Throws<Exception>(() => new WordSudoku("aaa"));
            }

            [Test]
            public void GeneratesGrid()
            {
                WordSudoku sudoku = new WordSudoku("shoe");
                List<string> UniqueLines = new List<string>();
                foreach (string line in sudoku.Grid)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        StringAssert.Contains(i.ToString(), line); //expect to see each number in each line exactly once.
                    }
                    Assert.IsFalse(WordSudoku.ContainsDuplicateLetters(line));
                    Assert.IsFalse(UniqueLines.Contains(line), "Expected each line to be unique.");
                    UniqueLines.Add(line);
                }
            }

        }

        [TestFixture]
        public class FormatForGoogle
        {
            [Test]
            public void Full_ReturnsExpectedString()
            {
                WordSudoku sudoku = new WordSudoku("shoe");
                sudoku.Grid[0] = "0123";
                sudoku.Grid[1] = "3012";
                sudoku.Grid[2] = "2301";
                sudoku.Grid[3] = "1230";

                const string EXPECTED_TEXT = 
@"S	H	O	E
E	S	H	O
O	E	S	H
H	O	E	S
";
                Assert.AreEqual(EXPECTED_TEXT, sudoku.FormatForGoogle(false));
            }

            [Test]
            public void Partial_ReturnsExpectedString()
            {
                WordSudoku sudoku = new WordSudoku("shoe");
                sudoku.Grid[0] = "0123";
                sudoku.Grid[1] = "3012";
                sudoku.Grid[2] = "2301";
                sudoku.Grid[3] = "1230";

                sudoku.RefreshPartialGrid();

                
                sudoku.PartialGrid[0] = "____";
                sudoku.PartialGrid[1] = "3_1_";
                sudoku.PartialGrid[2] = "__01";
                sudoku.PartialGrid[3] = "_23_";
                
                const string EXPECTED_TEXT =
@" 	 	 	 
E	 	H	 
 	 	S	H
 	O	E	 
";
                Assert.AreEqual(EXPECTED_TEXT, sudoku.FormatForGoogle());

            }

        }

        [TestFixture]
        public class FormatHtmlForGoogle
        {
            [Test]
            public void Full_ReturnsExpectedString()
            {
                WordSudoku sudoku = new WordSudoku("shoe");
                sudoku.Grid[0] = "0123";
                sudoku.Grid[1] = "3012";
                sudoku.Grid[2] = "2301";
                sudoku.Grid[3] = "1230";

                const string EXPECTED_HTML =
                    @"<html>
<body>
<table>
<!--StartFragment-->
	<tr>
		<td>S</td>
		<td>H</td>
		<td>O</td>
		<td>E</td>
	</tr>
	<tr>
		<td>E</td>
		<td>S</td>
		<td>H</td>
		<td>O</td>
	</tr>
	<tr>
		<td>O</td>
		<td>E</td>
		<td>S</td>
		<td>H</td>
	</tr>
	<tr>
		<td>H</td>
		<td>O</td>
		<td>E</td>
		<td>S</td>
	</tr>
<!--EndFragment-->
</table>
</body>
</html>
";
                Assert.AreEqual(EXPECTED_HTML, sudoku.FormatHtmlForGoogle(false));
            }

            [Test]
            public void Partial_ReturnsExpectedString()
            {
                WordSudoku sudoku = new WordSudoku("shoe");
                sudoku.Grid[0] = "0123";
                sudoku.Grid[1] = "3012";
                sudoku.Grid[2] = "2301";
                sudoku.Grid[3] = "1230";

                sudoku.RefreshPartialGrid();


                sudoku.PartialGrid[0] = "____";
                sudoku.PartialGrid[1] = "3_1_";
                sudoku.PartialGrid[2] = "__01";
                sudoku.PartialGrid[3] = "_23_";

                const string EXPECTED_HTML =
                    @"<html>
<body>
<table>
<!--StartFragment-->
	<tr>
		<td> </td>
		<td> </td>
		<td> </td>
		<td> </td>
	</tr>
	<tr>
		<td>E</td>
		<td> </td>
		<td>H</td>
		<td> </td>
	</tr>
	<tr>
		<td> </td>
		<td> </td>
		<td>S</td>
		<td>H</td>
	</tr>
	<tr>
		<td> </td>
		<td>O</td>
		<td>E</td>
		<td> </td>
	</tr>
<!--EndFragment-->
</table>
</body>
</html>
";
                Assert.AreEqual(EXPECTED_HTML, sudoku.FormatHtmlForGoogle());

            }

        }

        [TestFixture]
        public class IsUniquelyDetermined
        {
            [Test]
            public void Example_BottomLeftCell_IsNotUniquelyDetermined()
            {
                WordSudoku sudoku = new WordSudoku("shoe");
                sudoku.Grid[0] = "0123";
                sudoku.Grid[1] = "3012";
                sudoku.Grid[2] = "2301";
                sudoku.Grid[3] = "1230";

                sudoku.RefreshPartialGrid();


                sudoku.PartialGrid[0] = "____";
                sudoku.PartialGrid[1] = "3_1_";
                sudoku.PartialGrid[2] = "__01";
                sudoku.PartialGrid[3] = "_23_";

                Assert.IsFalse(sudoku.IsUniquelyDetermined(3, 0, sudoku.PartialGrid));
            }
        }

        [TestFixture]
        public class SetBlank
        {
            [Test]
            public void Example_BottomLeftCell_IsNotUniquelyDetermined()
            {
                WordSudoku sudoku = new WordSudoku("shoe");
                sudoku.Grid[0] = "0123";
                sudoku.Grid[1] = "3012";
                sudoku.Grid[2] = "2301";
                sudoku.Grid[3] = "1230";

                sudoku.RefreshPartialGrid();


                sudoku.PartialGrid[0] = "____";
                sudoku.PartialGrid[1] = "3012";
                sudoku.PartialGrid[2] = "2301";
                sudoku.PartialGrid[3] = "1230";

                sudoku.SetBlank(sudoku.PartialGrid, 3, 0);
                Assert.AreEqual("_230", sudoku.PartialGrid[3]);

                sudoku.SetBlank(sudoku.PartialGrid, 2, 3);
                Assert.AreEqual("230_", sudoku.PartialGrid[2]);

            }
        }

        [TestFixture]
        public class CreateGrid
        {
            [Test]
            public void AllSizes_HaveDefaultGrid()
            {
                for (int i = 2; i < 9; i++)
                {
                    var substring = "abcdefghij".Substring(i);
                    Console.WriteLine($"Creating puzzle of length {substring.Length}");
                    var puzzle =  new WordSudoku(substring, 1);
                }
            }
        }
    }
}