﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
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
            [TestCase(true)]
            [TestCase(false)]
            public void LongerPhrase_ReturnsExpectedResult(bool includeSolution)
            {
                const string HTML_DIRECTORY = @"html\ReadDownColumn\";
                const string SOURCE_DIRECTORY =
                    @"C:\Users\Chip\Source\Repos\WordPuzzle3\WordPuzzlesTest.NetFramework\html\ReadDownColumn";

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
                    "Clue for boxing",
                    "Clue for parent",
                    "Clue for brazen",
                    "Clue for joyful"
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

        }
    }
}