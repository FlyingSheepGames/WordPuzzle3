using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using NUnit.Framework;
using WordPuzzles.Puzzle;

namespace WordPuzzlesTest.Puzzle
{
    [TestFixture]
    public class MultipleCluesPuzzleTest
    {
        [TestFixture]
        public class GetCandidatesForLetter
        {
            [Test]
            public void C_ReturnsExpectedCollection()
            {
                MultipleCluesPuzzle puzzle = new MultipleCluesPuzzle();
                puzzle.RandomGeneratorSeed = 42;

                List<string> candidates = puzzle.GetCandidatesForLetter('c');
                Assert.Less(10, candidates.Count, "Expected at least 10 candidates.");
                foreach (var candidate in candidates)
                {
                    //Console.WriteLine(candidate);
                    Assert.AreEqual('c', candidate[1], "Expected second letter to be 'c'.");
                }

                candidates = puzzle.GetCandidatesForLetter('a');
                Assert.Less(10, candidates.Count, "Expected at least 10 candidates.");
                foreach (var candidate in candidates)
                {
                    Console.WriteLine(candidate);
                    Assert.AreEqual('a', candidate[1], "Expected second letter to be 'a'.");
                }

                candidates = puzzle.GetCandidatesForLetter('t');
                Assert.Less(10, candidates.Count, "Expected at least 10 candidates.");
                foreach (var candidate in candidates)
                {
                    Console.WriteLine(candidate);
                    Assert.AreEqual('t', candidate[1], "Expected second letter to be 't'.");
                }
            }
        }

        [TestFixture]
        public class AddWordWithClues
        {
            [Test]
            public void AddsWords()
            {
                MultipleCluesPuzzle puzzle = new MultipleCluesPuzzle();
                puzzle.RandomGeneratorSeed = 42;

                puzzle.AddWordWithClues("ace", new List<string>
                {
                    "Do well on a test",
                    "Might be the high card?",
                    "Fighter pilot"
                });

                Assert.AreEqual(4, puzzle.NextClueOrder, "Next clue should be 4.");
                Assert.AreEqual("ace", puzzle.WordsWithClues[0].WordText);
                Assert.AreEqual(6, puzzle.WordsWithClues[0].SumOfClueOrders);

                puzzle.AddWordWithClues("hanger", new List<string>()
                {
                    "Word after cliff", 
                    "Something often found in closets", 
                    "Executioner?"
                });

                Assert.AreEqual(7, puzzle.NextClueOrder, "Next clue should be 7.");
                Assert.AreEqual("hanger", puzzle.WordsWithClues[1].WordText);
                Assert.AreEqual(15, puzzle.WordsWithClues[1].SumOfClueOrders);

                puzzle.AddWordWithClues("atom", new List<string>()
                {
                    "Anagram of a watery castle defense",
                    "Basic chemical element"
                });

                Assert.AreEqual(9, puzzle.NextClueOrder, "Next clue should be 9.");
                Assert.AreEqual("atom", puzzle.WordsWithClues[2].WordText);
                Assert.AreEqual(15, puzzle.WordsWithClues[2].SumOfClueOrders);
            }
        }

        [TestFixture]
        public class ReorderClues
        {
            [Test]
            public void ChangesSums()
            {
                MultipleCluesPuzzle puzzle = new MultipleCluesPuzzle();
                puzzle.RandomGeneratorSeed = 42;

                puzzle.AddWordWithClues("ace", new List<string>
                {
                    "Do well on a test",
                    "Might be the high card?",
                    "Fighter pilot"
                });

                puzzle.AddWordWithClues("hanger", new List<string>()
                {
                    "Word after cliff",
                    "Something often found in closets",
                    "Executioner?"
                });


                puzzle.AddWordWithClues("atom", new List<string>()
                {
                    "Anagram of a watery castle defense",
                    "Basic chemical element"
                });
                Assert.AreEqual(6, puzzle.WordsWithClues[0].SumOfClueOrders);
                Assert.AreEqual(15, puzzle.WordsWithClues[1].SumOfClueOrders);
                Assert.AreEqual(15, puzzle.WordsWithClues[2].SumOfClueOrders);

                puzzle.ReorderClues();
                int firstSum = puzzle.WordsWithClues[0].SumOfClueOrders;
                int secondSum = puzzle.WordsWithClues[1].SumOfClueOrders;
                int thirdSum = puzzle.WordsWithClues[2].SumOfClueOrders;

                Assert.AreNotEqual(6, firstSum, "Expected first sum to be different");
                Assert.AreNotEqual(15, secondSum, "Expected second sum to be different");
                Assert.AreNotEqual(15, thirdSum, "Expected third sum to be different");

                Assert.AreNotEqual(firstSum, secondSum, "Expected first != second");
                Assert.AreNotEqual(firstSum, thirdSum, "Expected first != third");
                Assert.AreNotEqual(thirdSum, secondSum, "Expected third != second");
                Assert.AreEqual(36, firstSum + secondSum + thirdSum);


            }
        }


        [TestFixture]
        public class FormatHtmlForGoogle
        {


            [Test]
            [TestCase(true)]
            [TestCase(false)]
            public void WithSpecialCharacter_ReturnsExpectedResult(bool includeSolution)
            {
                const string HTML_DIRECTORY = @"html\MultipleCluesPuzzle\";
                string SOURCE_DIRECTORY = ConfigurationManager.AppSettings["SourceDirectory"] + "MultipleCluesPuzzle";

                var puzzle = CreateMultipleCluesPuzzle();

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

                string[] expectedLines = new[] { " " };// need to have something to be different from generated file.
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

                    if (!expectedLine.Equals(actualLine, StringComparison.InvariantCultureIgnoreCase))
                    {
                        anyLinesDifferent = true;
                        Console.WriteLine($"Expected Line {index}:{expectedLine}");
                        Console.WriteLine($"  Actual Line {index}:{actualLine}");
                    }
                }

                if (anyLinesDifferent)
                {
                    Console.WriteLine("Updating source file. Will show up as a difference in source control.");
                    File.WriteAllLines(SOURCE_DIRECTORY + $@"\{expectedFileName}", actualLines);
                }
                Assert.IsFalse(anyLinesDifferent, "Didn't expect any lines to be different.");

            }

        }

        private static MultipleCluesPuzzle CreateMultipleCluesPuzzle()
        {
            var puzzle = new MultipleCluesPuzzle();
            puzzle.RandomGeneratorSeed = 42;
            puzzle.AddWordWithClues("ace", new List<string>
            {
                "Do well on a test",
                "Might be the high card?",
                "Fighter pilot"
            });

            puzzle.AddWordWithClues("hanger", new List<string>()
            {
                "Word after cliff",
                "Something often found in closets",
                "Executioner?"
            });


            puzzle.AddWordWithClues("atom", new List<string>()
            {
                "Anagram of a watery castle defense",
                "Basic chemical element"
            });
            puzzle.ReorderClues();
            return puzzle;
        }

        [TestFixture]
        public class GetClues
        {
            [Test]
            public void ReturnsExpectedResults()
            {
                var puzzle = CreateMultipleCluesPuzzle();
                CollectionAssert.AreEqual(new List<string>()
                    {
                        "Do well on a test",
                        "Might be the high card?",
                        "Fighter pilot",
                        "Word after cliff",
                        "Something often found in closets",
                        "Executioner?",
                        "Anagram of a watery castle defense",
                        "Basic chemical element",
                    },
                    puzzle.GetClues());
            }
        }

        [TestFixture]
        public class ReplaceClue
        {
            [Test]
            public void ReturnsExpectedResults()
            {
                var puzzle = CreateMultipleCluesPuzzle();
                puzzle.ReplaceClue("Fighter pilot", "updated clue");
                CollectionAssert.AreEqual(
                    new List<string>()
                    {
                        "Do well on a test",
                        "Might be the high card?",
                        "updated clue",
                        "Word after cliff",
                        "Something often found in closets",
                        "Executioner?",
                        "Anagram of a watery castle defense",
                        "Basic chemical element",

                    },
                    puzzle.GetClues());
            }
        }

    }
}