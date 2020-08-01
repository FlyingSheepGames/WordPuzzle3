using System;
using System.Collections.Generic;
using NUnit.Framework;
using WordPuzzles.Puzzle.Legacy;

namespace WordPuzzlesTest.NetFramework.Puzzle.Legacy
{
    [TestFixture]
    public class WordLadderTest
    {
        [TestFixture]
        public class Constructor
        {
            [Test]
            public void SetsSolution()
            {
                WordLadder ladder = new WordLadder("bed");
                Assert.AreEqual("bed", ladder.Solution);
                Assert.AreEqual(3, ladder.Size);
            }
        }

        [TestFixture]
        public class FindNextWordsInChain
        {
            [Test]
            public void BED_2_FindsExpectedWords()
            {
                WordLadder ladder = new WordLadder("bed");
                List<string> nextWordCandidates = ladder.FindNextWordsInChain("bed", 2);
                foreach (string nextWord in nextWordCandidates)
                {
                    Console.WriteLine(nextWord);
                }
                Assert.AreEqual(3, nextWordCandidates.Count);
            }
        }

        [TestFixture]
        public class FormatHtmlForGoogle
        {
            [Test]
            public void GeneratesExpectedHtml()
            {
                WordLadder ladder = new WordLadder("plant");
                ladder.AddToChain("plant", "Green living thing");
                ladder.AddToChain("slant", "At an angle");
                ladder.AddToChain("scant", "Not very many");

                const string EXPECTED_HTML =
                    @"<html>
<body>
<!--StartFragment-->
Change one letter in each word to get the next word.
<table border=""1"">
	<tr>
		<td>Not very many</td>
		<td> </td>
		<td> </td>
		<td>3</td>
		<td> </td>
		<td> </td>
	</tr>
	<tr>
		<td>At an angle</td>
		<td> </td>
		<td>2</td>
		<td> </td>
		<td> </td>
		<td> </td>
	</tr>
	<tr>
		<td>Green living thing</td>
		<td>1</td>
		<td> </td>
		<td> </td>
		<td> </td>
		<td> </td>
	</tr>
</table>
Copy the letters into the solution below, using the numbers as a guide.
<table border=""1"">
	<tr>
		<td>Solution</td>
		<td> </td>
		<td> </td>
		<td> </td>
		<td> </td>
		<td> </td>
	</tr>
	<tr>
		<td> </td>
		<td>1</td>
		<td>2</td>
		<td>3</td>
		<td>4</td>
		<td>5</td>
	</tr>
</table>
<!--EndFragment-->
</body>
</html>
";
                Assert.AreEqual(EXPECTED_HTML, ladder.FormatHtmlForGoogle());
            }

        }

        [TestFixture]
        public class DisplayChain
        {
            [Test]
            public void ReturnsExpectedResult()
            {
                WordLadder ladder = new WordLadder("a");
                ladder.AddToChain("a", "First Clue");
                Assert.AreEqual("First Clue = a\r\n", ladder.DisplayChain());
            }
        }

        [TestFixture]
        public class AlreadyContains
        {
            [Test]
            public void RepeatWord_ReturnsTrue()
            {
                WordLadder ladder = new WordLadder("a");
                ladder.AddToChain("a", "First Clue");
                Assert.IsTrue(ladder.AlreadyContains("a"));
                Assert.IsFalse(ladder.AlreadyContains("b"));
            }
        }

        [TestFixture]
        public class AddToChain
        {
            [Test]
            public void MarksOffLetterFromSolution()
            {
                WordLadder ladder = new WordLadder("solution");
                ladder.AddToChain("eyes", "What you see with");
                var wordAndClue = ladder.Chain[0];
                Assert.AreEqual('s', wordAndClue.SolutionLetter);
                Assert.AreEqual(3, wordAndClue.SolutionLetterIndexInWord);
                Assert.AreEqual(0, wordAndClue.SolutionLetterIndexInSolution);
            }

            [Test]
            public void MarksOffLetterFromSolution_FirstTimeOnly()
            {
                WordLadder ladder = new WordLadder("solution");
                ladder.AddToChain("eyes", "What you see with");
                var firstWordAndClue = ladder.Chain[0];
                Assert.AreEqual('s', firstWordAndClue.SolutionLetter);
                Assert.AreEqual(3, firstWordAndClue.SolutionLetterIndexInWord);
                Assert.AreEqual(0, firstWordAndClue.SolutionLetterIndexInSolution);
                ladder.AddToChain("eyes", "What you see with");
                var secondWordAndClue = ladder.Chain[1];
                Assert.AreEqual(char.MinValue, secondWordAndClue.SolutionLetter);
                Assert.AreEqual(-1, secondWordAndClue.SolutionLetterIndexInWord);
                Assert.AreEqual(-1, secondWordAndClue.SolutionLetterIndexInSolution);
            }

            [Test]
            public void MultipleMatchingLetters_OnlyUsesFirstOne()
            {
                WordLadder ladder = new WordLadder("solution");
                ladder.AddToChain("salt", "Spice");
                var wordAndClue = ladder.Chain[0];
                Assert.AreEqual('s', wordAndClue.SolutionLetter);
                Assert.AreEqual(0, wordAndClue.SolutionLetterIndexInWord);
                Assert.AreEqual(0, wordAndClue.SolutionLetterIndexInSolution);
            }

        }

        [TestFixture]
        public class AllLettersPlaced
        {
            [Test]
            public void False_UntilAllLettersPlaced()
            {
                WordLadder ladder = new WordLadder("test");
                Assert.IsFalse(ladder.AllLettersPlaced);
                ladder.AddToChain("tea", "Matches first T");
                Assert.IsFalse(ladder.AllLettersPlaced);
                ladder.AddToChain("tea", "Matches E");
                Assert.IsFalse(ladder.AllLettersPlaced);
                ladder.AddToChain("teas", "Matches S");
                Assert.IsFalse(ladder.AllLettersPlaced);
                ladder.AddToChain("tea", "Matches last T");
                Assert.IsTrue(ladder.AllLettersPlaced);
            }
        }

        [TestFixture]
        public class RemainingUnplacedLetters
        {
            [Test]
            public void False_UntilAllLettersPlaced()
            {
                WordLadder ladder = new WordLadder("test");
                Assert.AreEqual("test", ladder.RemainingUnplacedLetters);
                ladder.AddToChain("tea", "Matches first T");
                Assert.AreEqual("est", ladder.RemainingUnplacedLetters);
                ladder.AddToChain("tea", "Matches E");
                Assert.AreEqual("st", ladder.RemainingUnplacedLetters);
                ladder.AddToChain("teas", "Matches S");
                Assert.AreEqual("t", ladder.RemainingUnplacedLetters);
                ladder.AddToChain("tea", "Matches last T");
                Assert.AreEqual("", ladder.RemainingUnplacedLetters);
            }
        }
    }
}