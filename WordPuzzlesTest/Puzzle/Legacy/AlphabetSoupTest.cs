using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using WordPuzzles.Puzzle.Legacy;
using WordPuzzles.Utility;

namespace WordPuzzlesTest.Puzzle.Legacy
{
    [TestFixture]
    public class AlphabetSoupTest
    {
        static readonly WordRepository Repository = new WordRepository() {IgnoreCache = false};
        [TestFixture]
        public class GenerateSingleLine
        {

            [Test]
            public void Output_FitsExpectedFormat()
            {
                AlphabetSoup puzzle = new AlphabetSoup(Repository);
                string line = puzzle.GenerateSingleLine('a', 'b');
                Assert.AreEqual(7, line.Length, "Expected 7 letters.");

                AssertExactlyOneSubstringIsAWord(line);
            }

            [Test]
            public void ExampleThatStartsInSecondPosition()
            {
                AlphabetSoup puzzle = new AlphabetSoup(Repository);
                puzzle.RandomSeed = 1;
                Assert.AreEqual("allayam", puzzle.GenerateSingleLine('a', 'a'));
                Assert.AreEqual("alabout", puzzle.GenerateSingleLine('a', 'b'));
            }

        }

        [TestFixture]
        public class GeneratePuzzle
        {
            [Test]
            [Ignore("Takes too long")]
            public void CreatesExpectedPuzzle()
            {
                AlphabetSoup puzzle = new AlphabetSoup(Repository);
                const string SOLUTION = "greenredyellowpurpleorange";
                List<string> lines = puzzle.GeneratePuzzle(SOLUTION);
                Assert.AreEqual(26, lines.Count);
                StringBuilder builder = new StringBuilder();
                foreach (string currentLine in lines)
                {
                    string tabDelimitedLine = string.Join( '\t'.ToString(), currentLine.ToCharArray());
                    builder.AppendLine(tabDelimitedLine);
                }

                var formattedPuzzle = builder.ToString();
                Console.WriteLine(formattedPuzzle);

                for (int index = 0; index < 26; index++)
                {
                    string line = lines[index];
                    string hiddenWord = AssertExactlyOneSubstringIsAWord(line);
                    Console.WriteLine($"{line} {hiddenWord.ToUpper()}");
                    Assert.AreEqual(SOLUTION[index], line[0], "Expected first letter in the line to match the solution.");
                    Assert.AreEqual((char) 97 + index, line[3], "Expected the alphabet to be in the middle.");
                }

            }
        }

        [TestFixture]
        public class HideWordInLine
        {
            [Test]
            [Ignore("Takes too long to run.")]
            public void SimpleTest_PutsWordInExpectedPosition()
            {
                AlphabetSoup puzzle = new AlphabetSoup(Repository);
                string line = puzzle.HideWordInLine("stuck", StartPosition.FirstPosition);
                Assert.AreEqual(7, line.Length, "Expected 7 letters.");
                Assert.AreEqual("stuck", line.Substring(0, 5), "Expected word in first position");
                AssertExactlyOneSubstringIsAWord(line);

                line = puzzle.HideWordInLine("stuck", StartPosition.SecondPosition);
                Assert.AreEqual(7, line.Length, "Expected 7 letters.");
                Assert.AreEqual("stuck", line.Substring(1, 5), "Expected word in second position");
                AssertExactlyOneSubstringIsAWord(line);

                line = puzzle.HideWordInLine("stuck", StartPosition.ThirdPosition);
                Assert.AreEqual(7, line.Length, "Expected 7 letters.");
                Assert.AreEqual("stuck", line.Substring(2, 5), "Expected word in third position");
                AssertExactlyOneSubstringIsAWord(line);
            }

            [Test]
            [Ignore("Takes too long to run")]
            [TestCase(StartPosition.FirstPosition)]
            [TestCase(StartPosition.SecondPosition)]
            [TestCase(StartPosition.ThirdPosition)]
            public void CallTwice_GeneratesDifferentLines(StartPosition selectedPosition)
            {
                AlphabetSoup puzzle = new AlphabetSoup(Repository);
                string firstLine = puzzle.HideWordInLine("stuck", selectedPosition);
                Assert.AreEqual(7, firstLine.Length, "Expected 7 letters.");
                AssertExactlyOneSubstringIsAWord(firstLine);

                string secondLine = puzzle.HideWordInLine("stuck", selectedPosition);
                Assert.AreEqual(7, secondLine.Length, "Expected 7 letters.");
                AssertExactlyOneSubstringIsAWord(secondLine);

                Assert.AreNotEqual(firstLine, secondLine);
            }
        }

        private static string AssertExactlyOneSubstringIsAWord(string line)
        {
            string hiddenWord;
            var firstPositionSubstring = line.Substring(0, 5);
            var secondPositionSubstring = line.Substring(1, 5);
            var thirdPositionSubstring = line.Substring(2, 5);

            var isThereAWordInFirstPosition = Repository.IsAWord(firstPositionSubstring);
            var isThereAWordInSecondPosition = Repository.IsAWord(secondPositionSubstring);
            var isThereAWordInThirdPosition = Repository.IsAWord(thirdPositionSubstring);
            //Expect exactly one of these to be true. 
            if (isThereAWordInFirstPosition)
            {
                hiddenWord = firstPositionSubstring;
                Assert.IsFalse(isThereAWordInSecondPosition,
                    $"'{line}' Word in first position, so second position should not be a word.");
                Assert.IsFalse(isThereAWordInThirdPosition, $"'{line}' Word in first position, so third position should not be a word.");
            }
            else
            {
                if (isThereAWordInSecondPosition)
                {
                    hiddenWord = secondPositionSubstring;
                    Assert.IsFalse(isThereAWordInThirdPosition,
                        $"'{line}' Word found in second position, so third position should not be a word.");
                }
                else
                {
                    hiddenWord = thirdPositionSubstring;
                    Assert.IsTrue(isThereAWordInThirdPosition,
                        $"'{line}' Word not found in first or second position, so third position should be a word.");
                }
            }
            //Also expect no 6 or 7 letter words. 
            string firstPositionSixLetterString = line.Substring(0, 6);
            string secondPositionSixLetterString = line.Substring(1, 6);
            Assert.IsFalse(Repository.IsAWord(firstPositionSixLetterString), $"{firstPositionSixLetterString} is a 6-letter word starting at the first letter, and shouldn't be.");
            Assert.IsFalse(Repository.IsAWord(secondPositionSixLetterString), $"{secondPositionSixLetterString} is a 6-letter word starting at the second letter, and shouldn't be.");

            if (line.Length < 7) //We don't support words of length 7 or more yet.
            {
                Assert.IsFalse(Repository.IsAWord(line), $"The entire line {line} is a seven letter word, and shouldn't be. ");
            }

            return hiddenWord;
        }
    }

    [TestFixture]
    public class Constructor
    {
        [Test]
        public void Default_UsesDefaultRepository()
        {
            AlphabetSoup puzzle = new AlphabetSoup();
            Assert.IsNotNull(puzzle);
        }
    }

    [TestFixture]
    public class GenerateLineAtIndex
    {
        [Test]
        [Ignore("Takes too long to run")]
        public void PopulatesExpectedLine()
        {
            AlphabetSoup puzzle = new AlphabetSoup();
            puzzle.Solution = "abcdefghijklmnopqrstuvwxyz";
            puzzle.GenerateLineAtIndex(0);
            Assert.AreEqual('a', puzzle.Lines[0][3], "Fourth character of this line should be 'a'");
        }
    }

    [TestFixture]
    public class ScrambleLines
    {
        [Test]
        [Ignore("Takes more than 3 seconds.")]
        public void MovesSingleLine()
        {
            AlphabetSoup puzzle = new AlphabetSoup();
            puzzle.Solution = "abcdefghijklmnopqrstuvwxyz";
            puzzle.GenerateLineAtIndex(0);
            puzzle.ScrambleLines();
            bool foundSingleNonNullLine = false;
            foreach (string line in puzzle.Lines)
            {
                Console.WriteLine(line);
                if (foundSingleNonNullLine) //All other lines should be null.
                {
                    Assert.IsNull(line);
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        foundSingleNonNullLine = true;
                    }
                }
            }
        }
    }

}