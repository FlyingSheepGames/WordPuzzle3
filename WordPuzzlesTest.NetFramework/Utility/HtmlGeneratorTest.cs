using System.IO;
using NUnit.Framework;
using WordPuzzles.Utility;

namespace WordPuzzlesTest.NetFramework.Utility
{
    [TestFixture]
    public class HtmlGeneratorTest
    {
        [TestFixture]
        public class CreateComment
        {
            [Test]
            public void GeneratesExpectedComment()
            {
                HtmlGenerator generator = new HtmlGenerator();
                WordPuzzles.Puzzle.Puzzle puzzle = new WordPuzzles.Puzzle.Puzzle {PhraseAsString = "max peel"};
                puzzle.AddWordToClues("example");
                puzzle.PlaceLetters();
                generator.Puzzle = puzzle;

                string expectedComment =
@"/*
E 0 <-> 12
X 1 <-> 9
A 2 <-> 8
M 3 <-> 7
P 4 <-> 10
L 5 <-> 13
E 6 <-> 11

-----
M 7 <-> 3
A 8 <-> 2
X 9 <-> 1
 
P 10 <-> 4
E 11 <-> 6
E 12 <-> 0
L 13 <-> 5
*/
";
                Assert.AreEqual(expectedComment, generator.CreateComment());

            }
        }

        [TestFixture]
        public class CreateIndexMapDefinition
        {
            [Test]
            public void ReturnsExpectedString()
            {
                HtmlGenerator generator = new HtmlGenerator();
                WordPuzzles.Puzzle.Puzzle puzzle = new WordPuzzles.Puzzle.Puzzle {PhraseAsString = "max peel"};
                puzzle.AddWordToClues("example");
                puzzle.PlaceLetters();
                generator.Puzzle = puzzle;

                const string EXPECTED_STRING =
@"var indexMap = [12, 9, 8, 7, 10, 13, 11, 3, 2, 1, 4, 6, 0, 5];";

                Assert.AreEqual(EXPECTED_STRING, generator.CreateIndexMapDefinition());
            }

            [Test]
            public void WithPunctuation_ReturnsExpectedString()
            {
                WordPuzzles.Puzzle.Puzzle puzzle = new WordPuzzles.Puzzle.Puzzle {PhraseAsString = "i'm x."};
                puzzle.AddWordToClues("mix");

                puzzle.PlaceLetters();

                HtmlGenerator htmlGenerator = new HtmlGenerator {Puzzle = puzzle};
                const string EXPECTED_STRING =
                    @"var indexMap = [4, 3, 5, 1, 0, 2];";

                Assert.AreEqual(EXPECTED_STRING, htmlGenerator.CreateIndexMapDefinition());

            }
        }

        [TestFixture]
        public class CreateTableRowForWord
        {
            [Test]
            public void ReturnsExpectedResult()
            {
                HtmlGenerator generator = new HtmlGenerator();
                WordPuzzles.Puzzle.Puzzle puzzle = new WordPuzzles.Puzzle.Puzzle {PhraseAsString = "max peel"};
                puzzle.AddWordToClues("example");
                puzzle.PlaceLetters();
                generator.Puzzle = puzzle;

                const string EXPECTED_HTML =
@"<tr>
	<td>
	Clue for example:
	</td>

	<td>
	<input type=""text"" size=""1"" maxlength=""1"" id=""letter0"" onFocus=""colorMeAndMyMatch(0,'yellow');"" onBlur=""colorMeAndMyMatch(0,'white');""
	/><input type=""text"" size=""1"" maxlength=""1"" id=""letter1"" onFocus=""colorMeAndMyMatch(1,'yellow');"" onBlur=""colorMeAndMyMatch(1,'white');""
	/><input type=""text"" size=""1"" maxlength=""1"" id=""letter2"" onFocus=""colorMeAndMyMatch(2,'yellow');"" onBlur=""colorMeAndMyMatch(2,'white');""
	/><input type=""text"" size=""1"" maxlength=""1"" id=""letter3"" onFocus=""colorMeAndMyMatch(3,'yellow');"" onBlur=""colorMeAndMyMatch(3,'white');""
	/><input type=""text"" size=""1"" maxlength=""1"" id=""letter4"" onFocus=""colorMeAndMyMatch(4,'yellow');"" onBlur=""colorMeAndMyMatch(4,'white');""
	/><input type=""text"" size=""1"" maxlength=""1"" id=""letter5"" onFocus=""colorMeAndMyMatch(5,'yellow');"" onBlur=""colorMeAndMyMatch(5,'white');""
	/><input type=""text"" size=""1"" maxlength=""1"" id=""letter6"" onFocus=""colorMeAndMyMatch(6,'yellow');"" onBlur=""colorMeAndMyMatch(6,'white');""
	/>
	</td>
</tr>
";
                Assert.AreEqual(EXPECTED_HTML, generator.CreateTableRowForWord(puzzle.Clues[0], 0));
            }

            [Test]
            public void WithPunctuation_ReturnsExpectedString()
            {
                WordPuzzles.Puzzle.Puzzle puzzle = new WordPuzzles.Puzzle.Puzzle {PhraseAsString = "i'm x."};
                puzzle.AddWordToClues("mix");

                puzzle.PlaceLetters();

                HtmlGenerator htmlGenerator = new HtmlGenerator {Puzzle = puzzle};
                const string EXPECTED_HTML =
                    @"<tr>
	<td>
	Clue for mix:
	</td>

	<td>
	<input type=""text"" size=""1"" maxlength=""1"" id=""letter0"" onFocus=""colorMeAndMyMatch(0,'yellow');"" onBlur=""colorMeAndMyMatch(0,'white');""
	/><input type=""text"" size=""1"" maxlength=""1"" id=""letter1"" onFocus=""colorMeAndMyMatch(1,'yellow');"" onBlur=""colorMeAndMyMatch(1,'white');""
	/><input type=""text"" size=""1"" maxlength=""1"" id=""letter2"" onFocus=""colorMeAndMyMatch(2,'yellow');"" onBlur=""colorMeAndMyMatch(2,'white');""
	/>
	</td>
</tr>
";
                Assert.AreEqual(EXPECTED_HTML, htmlGenerator.CreateTableRowForWord(puzzle.Clues[0], 0));
            }

            [Test]
            public void WithCustomClue_ReturnsExpectedString()
            {
                WordPuzzles.Puzzle.Puzzle puzzle = new WordPuzzles.Puzzle.Puzzle {PhraseAsString = "i'm x."};
                puzzle.AddWordToClues("mix");
                puzzle.Clues[0].CustomizedClue = "Customized Clue";
                puzzle.PlaceLetters();

                HtmlGenerator htmlGenerator = new HtmlGenerator {Puzzle = puzzle};
                const string EXPECTED_HTML =
                    @"<tr>
	<td>
	Customized Clue:
	</td>

	<td>
	<input type=""text"" size=""1"" maxlength=""1"" id=""letter0"" onFocus=""colorMeAndMyMatch(0,'yellow');"" onBlur=""colorMeAndMyMatch(0,'white');""
	/><input type=""text"" size=""1"" maxlength=""1"" id=""letter1"" onFocus=""colorMeAndMyMatch(1,'yellow');"" onBlur=""colorMeAndMyMatch(1,'white');""
	/><input type=""text"" size=""1"" maxlength=""1"" id=""letter2"" onFocus=""colorMeAndMyMatch(2,'yellow');"" onBlur=""colorMeAndMyMatch(2,'white');""
	/>
	</td>
</tr>
";
                Assert.AreEqual(EXPECTED_HTML, htmlGenerator.CreateTableRowForWord(puzzle.Clues[0], 0));

            }
        }

        [TestFixture]
        public class CreateTableRowForPhrase
        {
            [Test]
            public void ReturnsExpectedResult()
            {
                HtmlGenerator generator = new HtmlGenerator();
                WordPuzzles.Puzzle.Puzzle puzzle = new WordPuzzles.Puzzle.Puzzle {PhraseAsString = "max peel"};
                puzzle.AddWordToClues("example");
                puzzle.PlaceLetters();
                generator.Puzzle = puzzle;

                const string EXPECTED_HTML =
@"<tr>
	<td colspan=""2"">
	<input type=""text"" size=""1"" maxlength=""1"" id=""letter7"" onFocus=""colorMeAndMyMatch(7,'yellow');"" onBlur=""colorMeAndMyMatch(7,'white');"" 
	/><input type=""text"" size=""1"" maxlength=""1"" id=""letter8"" onFocus=""colorMeAndMyMatch(8,'yellow');"" onBlur=""colorMeAndMyMatch(8,'white');"" 
	/><input type=""text"" size=""1"" maxlength=""1"" id=""letter9"" onFocus=""colorMeAndMyMatch(9,'yellow');"" onBlur=""colorMeAndMyMatch(9,'white');"" 
	/>
	&nbsp;
	&nbsp;
	<input type=""text"" size=""1"" maxlength=""1"" id=""letter10"" onFocus=""colorMeAndMyMatch(10,'yellow');"" onBlur=""colorMeAndMyMatch(10,'white');"" 
	/><input type=""text"" size=""1"" maxlength=""1"" id=""letter11"" onFocus=""colorMeAndMyMatch(11,'yellow');"" onBlur=""colorMeAndMyMatch(11,'white');"" 
	/><input type=""text"" size=""1"" maxlength=""1"" id=""letter12"" onFocus=""colorMeAndMyMatch(12,'yellow');"" onBlur=""colorMeAndMyMatch(12,'white');"" 
	/><input type=""text"" size=""1"" maxlength=""1"" id=""letter13"" onFocus=""colorMeAndMyMatch(13,'yellow');"" onBlur=""colorMeAndMyMatch(13,'white');"" 
	/>
	</td>
</tr>
";
                Assert.AreEqual(EXPECTED_HTML, generator.CreateTableRowForPhrase());
            }


            [Test]
            public void WithPunctuation_ReturnsExpectedString()
            {
                WordPuzzles.Puzzle.Puzzle puzzle = new WordPuzzles.Puzzle.Puzzle {PhraseAsString = "i'm x."};
                puzzle.AddWordToClues("mix");

                puzzle.PlaceLetters();

                HtmlGenerator htmlGenerator = new HtmlGenerator {Puzzle = puzzle};
                const string EXPECTED_HTML =
                    @"<tr>
	<td colspan=""2"">
	<input type=""text"" size=""1"" maxlength=""1"" id=""letter3"" onFocus=""colorMeAndMyMatch(3,'yellow');"" onBlur=""colorMeAndMyMatch(3,'white');"" 
	/>
	'
	<input type=""text"" size=""1"" maxlength=""1"" id=""letter4"" onFocus=""colorMeAndMyMatch(4,'yellow');"" onBlur=""colorMeAndMyMatch(4,'white');"" 
	/>
	&nbsp;
	&nbsp;
	<input type=""text"" size=""1"" maxlength=""1"" id=""letter5"" onFocus=""colorMeAndMyMatch(5,'yellow');"" onBlur=""colorMeAndMyMatch(5,'white');"" 
	/>
	.
	</td>
</tr>
";
                Assert.AreEqual(EXPECTED_HTML, htmlGenerator.CreateTableRowForPhrase());
            }

        }


        [TestFixture]
        public class GenerateHtmlFile
        {
            [Test]
            public void CreatedExpectedFile()
            {
                HtmlGenerator generator = new HtmlGenerator();
                WordPuzzles.Puzzle.Puzzle puzzle = new WordPuzzles.Puzzle.Puzzle {PhraseAsString = "max peel"};
                puzzle.AddWordToClues("example");
                puzzle.PlaceLetters();
                generator.Puzzle = puzzle;

                const string TEST_PUZZLE_FILENAME = "test_puzzle.html";

                string actualHtml = generator.GenerateHtmlFile(TEST_PUZZLE_FILENAME);

                Assert.AreEqual(File.ReadAllText(@"data\puzzle.html"), actualHtml);
                FileAssert.AreEqual(@"data\puzzle.html", TEST_PUZZLE_FILENAME);

            }

            [Test]
            public void WithoutKey_CreatedExpectedFile()
            {
                HtmlGenerator generator = new HtmlGenerator();
                WordPuzzles.Puzzle.Puzzle puzzle = new WordPuzzles.Puzzle.Puzzle {PhraseAsString = "max peel"};
                puzzle.AddWordToClues("example");
                puzzle.PlaceLetters();
                generator.Puzzle = puzzle;

                const string TEST_PUZZLE_FILENAME = "test_puzzle_withoutkey.html";
                const string EXPECTED_RESULTS_FILENAME = @"data\puzzle_withoutkey.html";

                string actualHtml = generator.GenerateHtmlFile(TEST_PUZZLE_FILENAME, false);


                Assert.AreEqual(File.ReadAllText(EXPECTED_RESULTS_FILENAME), actualHtml);
                FileAssert.AreEqual(EXPECTED_RESULTS_FILENAME, TEST_PUZZLE_FILENAME);

            }

        }

    }


}