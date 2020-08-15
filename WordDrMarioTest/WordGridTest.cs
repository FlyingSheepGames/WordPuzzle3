using NUnit.Framework;
using WordDrMario;

namespace WordDrMarioTest
{
    [TestFixture]
    public class WordGridTest
    {
        [TestFixture]
        public class Constructor
        {
            [Test]
            public void CreatesExpectedObject()
            {
                WordGrid grid = new WordGrid(6);
                Assert.AreEqual(6, grid.Lines.Length);
                foreach (string line in grid.Lines)
                {
                    Assert.AreEqual("      ", line);
                }
            }
        }

        [TestFixture]
        public class DropLetter
        {
            [Test]
            public void EmptyGrid_PlacesLetterAsExpected()
            {
                WordGrid grid = new WordGrid(6);
                var ableToDropLetter = grid.DropLetter('A', 0);
                Assert.AreEqual(true, ableToDropLetter);
                Assert.AreEqual("      ", grid.Lines[0]);
                Assert.AreEqual("      ", grid.Lines[1]);
                Assert.AreEqual("      ", grid.Lines[2]);
                Assert.AreEqual("      ", grid.Lines[3]);
                Assert.AreEqual("      ", grid.Lines[4]);
                Assert.AreEqual("A     ", grid.Lines[5]);
            }

            [Test]
            public void FillColumn_PlacesLetterAsExpected()
            {
                WordGrid grid = new WordGrid(6);
                var ableToDropLetter = grid.DropLetter('A', 0);
                Assert.AreEqual(true, ableToDropLetter);
                Assert.AreEqual("      ", grid.Lines[0]);
                Assert.AreEqual("      ", grid.Lines[1]);
                Assert.AreEqual("      ", grid.Lines[2]);
                Assert.AreEqual("      ", grid.Lines[3]);
                Assert.AreEqual("      ", grid.Lines[4]);
                Assert.AreEqual("A     ", grid.Lines[5]);

                ableToDropLetter = grid.DropLetter('B', 0);
                Assert.AreEqual(true, ableToDropLetter);
                Assert.AreEqual("      ", grid.Lines[0]);
                Assert.AreEqual("      ", grid.Lines[1]);
                Assert.AreEqual("      ", grid.Lines[2]);
                Assert.AreEqual("      ", grid.Lines[3]);
                Assert.AreEqual("B     ", grid.Lines[4]);
                Assert.AreEqual("A     ", grid.Lines[5]);

                ableToDropLetter = grid.DropLetter('C', 0);
                Assert.AreEqual(true, ableToDropLetter);
                Assert.AreEqual("      ", grid.Lines[0]);
                Assert.AreEqual("      ", grid.Lines[1]);
                Assert.AreEqual("      ", grid.Lines[2]);
                Assert.AreEqual("C     ", grid.Lines[3]);
                Assert.AreEqual("B     ", grid.Lines[4]);
                Assert.AreEqual("A     ", grid.Lines[5]);

                ableToDropLetter = grid.DropLetter('D', 0);
                Assert.AreEqual(true, ableToDropLetter);
                Assert.AreEqual("      ", grid.Lines[0]);
                Assert.AreEqual("      ", grid.Lines[1]);
                Assert.AreEqual("D     ", grid.Lines[2]);
                Assert.AreEqual("C     ", grid.Lines[3]);
                Assert.AreEqual("B     ", grid.Lines[4]);
                Assert.AreEqual("A     ", grid.Lines[5]);

                ableToDropLetter = grid.DropLetter('E', 0);
                Assert.AreEqual(true, ableToDropLetter);
                Assert.AreEqual("      ", grid.Lines[0]);
                Assert.AreEqual("E     ", grid.Lines[1]);
                Assert.AreEqual("D     ", grid.Lines[2]);
                Assert.AreEqual("C     ", grid.Lines[3]);
                Assert.AreEqual("B     ", grid.Lines[4]);
                Assert.AreEqual("A     ", grid.Lines[5]);

                ableToDropLetter = grid.DropLetter('F', 0);
                Assert.AreEqual(true, ableToDropLetter);
                Assert.AreEqual("F     ", grid.Lines[0]);
                Assert.AreEqual("E     ", grid.Lines[1]);
                Assert.AreEqual("D     ", grid.Lines[2]);
                Assert.AreEqual("C     ", grid.Lines[3]);
                Assert.AreEqual("B     ", grid.Lines[4]);
                Assert.AreEqual("A     ", grid.Lines[5]);

                ableToDropLetter = grid.DropLetter('G', 0);
                Assert.AreEqual(false, ableToDropLetter);
                Assert.AreEqual("F     ", grid.Lines[0]);
                Assert.AreEqual("E     ", grid.Lines[1]);
                Assert.AreEqual("D     ", grid.Lines[2]);
                Assert.AreEqual("C     ", grid.Lines[3]);
                Assert.AreEqual("B     ", grid.Lines[4]);
                Assert.AreEqual("A     ", grid.Lines[5]);

            }

        }

        [TestFixture]
        public class FindHorizontalWords
        {
            [Test]
            [Ignore("Takes more than 3 seconds.")]
            public void SET_FindsExpectedWord()
            {
                WordGrid grid = new WordGrid(6);
                grid.Lines[5] = "SET   ";

                Assert.IsTrue(grid.FindHorizontalWords());

                Assert.AreEqual(1, grid.FoundWords.Count);

            }

            [Test]
            [Ignore("Takes more than 3 seconds.")]
            public void Repeated_DoesNotAddAdditionalWords()
            {
                WordGrid grid = new WordGrid(6);
                grid.Lines[5] = "SET   ";

                Assert.IsTrue(grid.FindHorizontalWords());

                Assert.AreEqual(1, grid.FoundWords.Count);

                Assert.IsTrue(grid.FindHorizontalWords());

                Assert.AreEqual(1, grid.FoundWords.Count);

            }

            [Test]
            [Ignore("Takes more than 3 seconds.")]
            public void RightAligned_SET_FindsExpectedWord()
            {
                WordGrid grid = new WordGrid(6);
                grid.Lines[5] = "   SET";

                Assert.IsTrue(grid.FindHorizontalWords());

                Assert.AreEqual(1, grid.FoundWords.Count);
            }

            [Test]
            [Ignore("Takes more than 3 seconds.")]
            public void AND_SET_FindsExpectedWord()
            {
                WordGrid grid = new WordGrid(6);
                grid.Lines[4] = "   AND";
                grid.Lines[5] = "   SET";

                Assert.IsTrue(grid.FindHorizontalWords());

                Assert.AreEqual(2, grid.FoundWords.Count);
            }

        }


        [TestFixture]
        public class FindVerticalWords
        {
            [Test]
            [Ignore("Takes more than 3 seconds.")]
            public void SET_FindsExpectedWord()
            {
                WordGrid grid = new WordGrid(6);
                grid.Lines[3] = "S     ";
                grid.Lines[4] = "E     ";
                grid.Lines[5] = "T     ";

                Assert.IsTrue(grid.FindVerticalWords());

                Assert.AreEqual(1, grid.FoundWords.Count);
            }
        }

        [TestFixture]
        public class CalculateScore
        {
            [Test]
            public void EmptyGrid_Returns0()
            {
                WordGrid grid = new WordGrid(6);
                Assert.AreEqual(0, grid.CalculateScore());
            }

            [Test]
            [Ignore("Takes more than 3 seconds.")]
            public void SET_Returns3()
            {
                WordGrid grid = new WordGrid(6);
                grid.Lines[3] = "S     ";
                grid.Lines[4] = "E     ";
                grid.Lines[5] = "T     ";

                Assert.IsTrue(grid.FindVerticalWords());

                Assert.AreEqual(3, grid.CalculateScore());
            }

            [Test]
            [Ignore("Takes more than 3 seconds.")]

            public void SET_And_TEST_Returns8()
            {
                WordGrid grid = new WordGrid(6);
                grid.Lines[3] = "S     ";
                grid.Lines[4] = "E     ";
                grid.Lines[5] = "TEST  ";

                Assert.IsTrue(grid.FindVerticalWords());
                Assert.IsTrue(grid.FindHorizontalWords());

                Assert.AreEqual(8, grid.CalculateScore());
            }

        }

        [TestFixture]
        public class DropAllLetters
        {
            [Test]
            public void SingleDiagonalLine_DropsSuspendedLetters()
            {
                WordGrid gridWithSuspendedLetters = new WordGrid(6);

                gridWithSuspendedLetters.Lines[0] = "a     ";
                gridWithSuspendedLetters.Lines[1] = " b    ";
                gridWithSuspendedLetters.Lines[2] = "  c   ";
                gridWithSuspendedLetters.Lines[3] = "   d  ";
                gridWithSuspendedLetters.Lines[4] = "    e ";
                gridWithSuspendedLetters.Lines[5] = "     f";

                gridWithSuspendedLetters.DropAllLetters();

                for (int emptyIndex = 0; emptyIndex < 5; emptyIndex++)
                {
                    Assert.AreEqual("      ", gridWithSuspendedLetters.Lines[emptyIndex]);
                }
                Assert.AreEqual("abcdef", gridWithSuspendedLetters.Lines[5]);
            }

            [Test]
            public void TwoDiagonalLines_DropsSuspendedLetters()
            {
                WordGrid gridWithSuspendedLetters = new WordGrid(6);

                gridWithSuspendedLetters.Lines[0] = "a    l";
                gridWithSuspendedLetters.Lines[1] = " b  k ";
                gridWithSuspendedLetters.Lines[2] = "  cj  ";
                gridWithSuspendedLetters.Lines[3] = "  id  ";
                gridWithSuspendedLetters.Lines[4] = " h  e ";
                gridWithSuspendedLetters.Lines[5] = "g    f";

                gridWithSuspendedLetters.DropAllLetters();

                for (int emptyIndex = 0; emptyIndex < 4; emptyIndex++)
                {
                    Assert.AreEqual("      ", gridWithSuspendedLetters.Lines[emptyIndex]);
                }
                Assert.AreEqual("abcjkl", gridWithSuspendedLetters.Lines[4]);
                Assert.AreEqual("ghidef", gridWithSuspendedLetters.Lines[5]);
            }

        }

        [TestFixture]
        public class DeleteFoundWords
        {
            [Test]
            [Ignore("Takes more than 3 seconds.")]
            public void ClearsAsExpected()
            {
                WordGrid grid = new WordGrid(6);

                grid.Lines[5] = "setx  ";

                Assert.IsTrue(grid.FindHorizontalWords());

                grid.DeleteFoundWords();

                Assert.AreEqual(0, grid.FoundWords.Count);
                Assert.AreEqual("   x  ", grid.Lines[5]);
            }
        }
    }
}