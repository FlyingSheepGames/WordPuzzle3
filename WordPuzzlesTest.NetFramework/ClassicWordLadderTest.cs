using System;
using System.Collections.Generic;
using NUnit.Framework;
using WordPuzzles;

namespace WordPuzzlesTest
{
    [TestFixture]
    public class ClassicWordLadderTest
    {
        [TestFixture]
        public class Constructor
        {
            [Test]
            public void SetsSolution()
            {
                ClassicWordLadder ladder = new ClassicWordLadder("bed", "Where you sleep.");
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
                ClassicWordLadder ladder = new ClassicWordLadder("bed", "Where you sleep.");
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
                ClassicWordLadder ladder = new ClassicWordLadder("plant", "Green living thing");
                ladder.AddToChain("slant", "At an angle");
                ladder.AddToChain("scant", "Not very many");

                const string EXPECTED_HTML =
                    @"<html>
<body>
<table>
<!--StartFragment-->
	<tr>
		<td>Not very many</td>
		<td> </td>
		<td> </td>
		<td> </td>
		<td> </td>
		<td> </td>
	</tr>
	<tr>
		<td>At an angle</td>
		<td> </td>
		<td> </td>
		<td> </td>
		<td> </td>
		<td> </td>
	</tr>
	<tr>
		<td>Green living thing</td>
		<td> </td>
		<td> </td>
		<td> </td>
		<td> </td>
		<td> </td>
	</tr>
<!--EndFragment-->
</table>
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
                ClassicWordLadder ladder = new ClassicWordLadder("a", "First Clue");
                Assert.AreEqual("First Clue = a\r\n", ladder.DisplayChain());
            }
        }

        [TestFixture]
        public class AlreadyContains
        {
            [Test]
            public void RepeatWord_ReturnsTrue()
            {
                ClassicWordLadder ladder = new ClassicWordLadder("a", "First Clue");
                Assert.IsTrue(ladder.AlreadyContains("a"));
                Assert.IsFalse(ladder.AlreadyContains("b"));
            }
        }
    }
}