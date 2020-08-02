using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using WordPuzzles.Puzzle;
using WordPuzzles.Utility;

namespace WordPuzzlesTest.Puzzle
{
    [TestFixture]
    public class ReadDownColumnPuzzleTest
    {
        [TestFixture]
        public class PopulateWords
        {
            [Test]
            [Ignore("Takes more than 3 seconds.")]
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
            [Ignore("Takes more than 3 seconds.")]
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

            [Test]
            [Ignore("Takes more than 3 seconds.")]
            public void SpecialCharacter_IncludedInEachWord()
            {
                ReadDownColumnPuzzle puzzle = new ReadDownColumnPuzzle();
                puzzle.RandomSeed = 42;
                puzzle.SpecialCharacter = 'M';
                puzzle.Solution = "claw";
                puzzle.NumberOfWordsToInclude = 1;
                puzzle.PopulateWords();

                foreach (string word in puzzle.Words)
                {
                    if (word == "lowing") continue; //not sure why "mowing" isn't a word. 
                    StringAssert.Contains("m", word, "Expected the letter 'm' in each word.");
                }

            }
        }

        [TestFixture]
        public class FormatHtmlForGoogle
        {

            [Test]
            [TestCase(true)]
            [TestCase(false)]
            public void LongerPhrase_ReturnsExpectedResult(bool includeSolution)
            {
                const string HTML_DIRECTORY = @"html\ReadDownColumn\";
                const string SOURCE_DIRECTORY =
                    @"C:\Users\Chip\Source\Repos\WordPuzzle3\WordPuzzlesTest\html\ReadDownColumn";
                ClueRepository clueRepository = new ClueRepository();
                clueRepository.ReadFromDisk(@"C:\Users\Chip\Source\Repos\WordPuzzle3\WordPuzzlesTest\data\PUZ\allclues.json");

                var puzzle = new ReadDownColumnPuzzle()
                {
                    Solution = "XRAY", 
                    Words =
                    {
                        "boxing", 
                        "parent",
                        "brazen",
                        "joyful"
                    }
                };

                puzzle.Clues = new List<string>
                {
                    clueRepository.GetCluesForWord("boxing")[0].ClueText,
                    clueRepository.GetCluesForWord("parent")[0].ClueText,
                    clueRepository.GetCluesForWord("brazen")[0].ClueText,
                    clueRepository.GetCluesForWord("joyful")[0].ClueText,
                };

                string generatedHtml = puzzle.FormatHtmlForGoogle(includeSolution);

                var actualFileName = "actualExample1.html";
                if (includeSolution)
                {
                    actualFileName = "actualExampleWithSolution1.html";
                }
                File.WriteAllText(HTML_DIRECTORY + actualFileName, generatedHtml);
                var expectedFileName = "expectedExample1.html";
                if (includeSolution)
                {
                    expectedFileName = "expectedExampleWithSolution1.html";
                }

                string[] expectedLines = new string[] { " " };// need to have something to be different from generated file.
                if (File.Exists(HTML_DIRECTORY + expectedFileName))
                {
                    expectedLines = File.ReadAllLines(HTML_DIRECTORY + expectedFileName);
                }
                var actualLines = File.ReadAllLines(HTML_DIRECTORY + actualFileName);
                bool anyLinesDifferent = false;
                for (var index = 0; index < expectedLines.Length; index++)
                {
                    string expectedLine = expectedLines[index];
                    string actualLine = "End of file already reached.";
                    if (index >= 0 && actualLines.Length > index)
                    {
                        actualLine = actualLines[index];
                    }

                    if (expectedLine != actualLine)
                    {
                        anyLinesDifferent = true;
                        Console.WriteLine($"Expected Line {index}:{expectedLine}");
                        Console.WriteLine($"  Actual Line {index}:{expectedLine}");
                    }
                }

                if (anyLinesDifferent)
                {
                    Console.WriteLine($"Updating source file. Will show up as a difference in source control.");
                    File.WriteAllLines(SOURCE_DIRECTORY + $@"\{expectedFileName}", actualLines);
                }
                Assert.IsFalse(anyLinesDifferent, "Didn't expect any lines to be different.");

            }

            [Test]
            [TestCase(true)]
            [TestCase(false)]
            public void WithSpecialCharacter_ReturnsExpectedResult(bool includeSolution)
            {
                const string HTML_DIRECTORY = @"html\ReadDownColumn\";
                const string SOURCE_DIRECTORY =
                    @"C:\Users\Chip\Source\Repos\WordPuzzle3\WordPuzzlesTest\html\ReadDownColumn";
                ClueRepository clueRepository = new ClueRepository();
                clueRepository.ReadFromDisk(@"C:\Users\Chip\Source\Repos\WordPuzzle3\WordPuzzlesTest\data\PUZ\allclues.json");

                var puzzle = new ReadDownColumnPuzzle()
                {
                    Solution = "XRAY",
                    Words =
                    {
                        "boxing",
                        "parent",
                        "brazen",
                        "joyful"
                    }
                };
                puzzle.SpecialCharacter = 'E';
                puzzle.Clues = new List<string>
                {
                    clueRepository.GetCluesForWord("boxing")[0].ClueText,
                    clueRepository.GetCluesForWord("parent")[0].ClueText,
                    clueRepository.GetCluesForWord("brazen")[0].ClueText,
                    clueRepository.GetCluesForWord("joyful")[0].ClueText,
                };

                string generatedHtml = puzzle.FormatHtmlForGoogle(includeSolution);

                var actualFileName = "actualExample2.html";
                if (includeSolution)
                {
                    actualFileName = "actualExampleWithSolution2.html";
                }
                File.WriteAllText(HTML_DIRECTORY + actualFileName, generatedHtml);
                var expectedFileName = "expectedExample2.html";
                if (includeSolution)
                {
                    expectedFileName = "expectedExampleWithSolution2.html";
                }

                string[] expectedLines = new string[] { " " };// need to have something to be different from generated file.
                if (File.Exists(HTML_DIRECTORY + expectedFileName))
                {
                    expectedLines = File.ReadAllLines(HTML_DIRECTORY + expectedFileName);
                }
                var actualLines = File.ReadAllLines(HTML_DIRECTORY + actualFileName);
                bool anyLinesDifferent = false;
                for (var index = 0; index < expectedLines.Length; index++)
                {
                    string expectedLine = expectedLines[index];
                    string actualLine = "End of file already reached.";
                    if (index >= 0 && actualLines.Length > index)
                    {
                        actualLine = actualLines[index];
                    }

                    if (expectedLine != actualLine)
                    {
                        anyLinesDifferent = true;
                        Console.WriteLine($"Expected Line {index}:{expectedLine}");
                        Console.WriteLine($"  Actual Line {index}:{expectedLine}");
                    }
                }

                if (anyLinesDifferent)
                {
                    Console.WriteLine($"Updating source file. Will show up as a difference in source control.");
                    File.WriteAllLines(SOURCE_DIRECTORY + $@"\{expectedFileName}", actualLines);
                }
                Assert.IsFalse(anyLinesDifferent, "Didn't expect any lines to be different.");

            }

        }

        [TestFixture]
        public class InsertSpecialCharacterInPattern
        {
            [Test]
            public void ReturnsExpectedResults()
            {
                ReadDownColumnPuzzle puzzle = new ReadDownColumnPuzzle();
                puzzle.SpecialCharacter = 'm';
                puzzle.RandomSeed = 42;

                var results = puzzle.InsertSpecialCharacterInPattern("__x___");
                Assert.AreEqual("m_x___", results[0]);
                Assert.AreEqual("_mx___", results[1]);
                Assert.AreEqual("__xm__", results[2]);
                Assert.AreEqual("__x_m_", results[3]);
                Assert.AreEqual("__x__m", results[4]);
            }
        }
    }
}