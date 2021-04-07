using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using NUnit.Framework;
using NUnit.Framework.Internal.Execution;
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
            //[Ignore("Takes more than 3 seconds.")] //needed for coverage
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
                var puzzleSolution = "catsAndDogs".ToLowerInvariant();

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
                ReadDownColumnPuzzle puzzle = new ReadDownColumnPuzzle
                {
                    RandomSeed = 42, SpecialCharacter = 'M', Solution = "claw", NumberOfWordsToInclude = 1
                };
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
                string SOURCE_DIRECTORY = ConfigurationManager.AppSettings["SourceDirectory"] + "ReadDownColumn";
                ClueRepository clueRepository = new ClueRepository();
                clueRepository.ReadFromDisk();

                var puzzle = CreateXRayPuzzle(clueRepository);


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

                    if (expectedLine != actualLine)
                    {
                        anyLinesDifferent = true;
                        Console.WriteLine($"Expected Line {index}:{expectedLine}");
                        Console.WriteLine($"  Actual Line {index}:{expectedLine}");
                    }
                }

                if (anyLinesDifferent)
                {
                    Console.WriteLine("Updating source file. Will show up as a difference in source control.");
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
                string SOURCE_DIRECTORY = ConfigurationManager.AppSettings["SourceDirectory"] + "ReadDownColumn";
                ClueRepository clueRepository = new ClueRepository();
                clueRepository.ReadFromDisk();

                var puzzle = new ReadDownColumnPuzzle
                {
                    Solution = "XRAY",
                    Words = {"boxing", "parent", "brazen", "joyful"},
                    SpecialCharacter = 'E',
                    Clues = new List<string>
                    {
                        clueRepository.GetCluesForWord("boxing")[0].ClueText,
                        clueRepository.GetCluesForWord("parent")[0].ClueText,
                        clueRepository.GetCluesForWord("brazen")[0].ClueText,
                        clueRepository.GetCluesForWord("joyful")[0].ClueText,
                    }
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

                    if (expectedLine != actualLine)
                    {
                        anyLinesDifferent = true;
                        Console.WriteLine($"Expected Line {index}:{expectedLine}");
                        Console.WriteLine($"  Actual Line {index}:{expectedLine}");
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

        private static ReadDownColumnPuzzle CreateXRayPuzzle(ClueRepository clueRepository = null)
        {
            if (clueRepository == null)
            {
                clueRepository = new ClueRepository();
                clueRepository.ReadFromDisk();
            }

            var puzzle = new ReadDownColumnPuzzle
            {
                Solution = "XRAY",
                Words = { "boxing", "parent", "brazen", "joyful" },
                Clues = new List<string>
                {
                    clueRepository.GetCluesForWord("boxing")[0].ClueText,
                    clueRepository.GetCluesForWord("parent")[0].ClueText,
                    clueRepository.GetCluesForWord("brazen")[0].ClueText,
                    clueRepository.GetCluesForWord("joyful")[0].ClueText,
                }
            };
            return puzzle;
        }

        [TestFixture]
        public class InsertSpecialCharacterInPattern
        {
            [Test]
            public void ReturnsExpectedResults()
            {
                ReadDownColumnPuzzle puzzle = new ReadDownColumnPuzzle {SpecialCharacter = 'm', RandomSeed = 42};

                var results = puzzle.InsertSpecialCharacterInPattern("__x___");
                Assert.AreEqual("m_x___", results[0]);
                Assert.AreEqual("_mx___", results[1]);
                Assert.AreEqual("__xm__", results[2]);
                Assert.AreEqual("__x_m_", results[3]);
                Assert.AreEqual("__x__m", results[4]);
            }
        }

        [TestFixture]
        public class GetOrdinalOfColumnWithSolution
        {
            [Test]
            public void ReturnsExpectedWord()
            {
                ReadDownColumnPuzzle puzzle = new ReadDownColumnPuzzle();
                puzzle.ZeroBasedIndexOfSolution = 0;
                Assert.AreEqual("first", puzzle.GetOrdinalOfColumnWithSolution());
                puzzle.ZeroBasedIndexOfSolution = 1;
                Assert.AreEqual("second", puzzle.GetOrdinalOfColumnWithSolution());
                puzzle.ZeroBasedIndexOfSolution = 2;
                Assert.AreEqual("third", puzzle.GetOrdinalOfColumnWithSolution());
                puzzle.ZeroBasedIndexOfSolution = 3;
                Assert.AreEqual("fourth", puzzle.GetOrdinalOfColumnWithSolution());
                puzzle.ZeroBasedIndexOfSolution = 4;
                Assert.AreEqual("fifth", puzzle.GetOrdinalOfColumnWithSolution());
                puzzle.ZeroBasedIndexOfSolution = 5;
                Assert.AreEqual("last", puzzle.GetOrdinalOfColumnWithSolution());

            }
        }

        [TestFixture]
        public class GetClues
        {
            [Test]
            public void ReturnsExpectedResults()
            {
                var puzzle = CreateXRayPuzzle();
                CollectionAssert.AreEqual(new List<string>()
                    {
                        "Fighting with the fists",
                        "Michelle or Barack, for Malia",
                        "Face with defiance or impudence",
                        "Full of or producing joy",
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
                var puzzle = CreateXRayPuzzle();
                puzzle.ReplaceClue("Face with defiance or impudence", "updated clue");
                CollectionAssert.AreEqual(
                    new List<string>()
                    {
                        "Fighting with the fists",
                        "Michelle or Barack, for Malia",
                        "updated clue",
                        "Full of or producing joy",
                    },
                    puzzle.GetClues());
            }
        }

        [TestFixture]
        public class AcceptablePatterns
        {
            [Test]
            public void CommonLetters_ReturnsExpectedSet()
            {
                ReadDownColumnPuzzle puzzle = new ReadDownColumnPuzzle();
                puzzle.Solution = "abcdeghilmnoprstw";
                //2.If Q, J, X, Z, remove those patterns NOT in the IncludeFor<Letter> list
                //3.If V, U, F, Y, K, remove selected patterns in the ExcludeFor<Letter> list
                Assert.AreEqual(36, puzzle.AcceptablePatterns.Count);
                var puzzleRepository = new WordRepository();
                foreach (string pattern in puzzle.AcceptablePatterns)
                {
                    foreach (char letter in puzzle.Solution)
                    {
                        var patternWithLetter = pattern.Replace('1', letter);
                        Assert.Less(0, puzzleRepository.WordsMatchingPattern(patternWithLetter).Count, $"{patternWithLetter} should have had at least one match.");
                    }
                }
            }

            [Test]
            [TestCase('q', 17)]
            [TestCase('j', 16)]
            [TestCase('x', 21)]
            [TestCase('z', 23)]
            public void RareLetters_ReturnsExpectedSet(char rareLetter, int expectedNumberOfPatterns)
            {
                ReadDownColumnPuzzle puzzle = new ReadDownColumnPuzzle();
                puzzle.Solution = "abcdeghilmnoprstw" + rareLetter;
                //2.If Q, J, X, Z, remove those patterns NOT in the IncludeFor<Letter> list
                //3.If V, U, F, Y, K, remove selected patterns in the ExcludeFor<Letter> list
                var puzzleRepository = new WordRepository();
                foreach (string pattern in puzzle.AcceptablePatterns)
                {
                    foreach (char letter in puzzle.Solution)
                    {
                        var patternWithLetter = pattern.Replace('1', letter);
                        Assert.Less(0, puzzleRepository.WordsMatchingPattern(patternWithLetter).Count, $"{patternWithLetter} should have had at least one match.");
                    }
                }
                Assert.AreEqual(expectedNumberOfPatterns, puzzle.AcceptablePatterns.Count);
            }


            [Test]
            [TestCase('v', 34)]
            [TestCase('f', 34)]
            [TestCase('y', 34)]
            [TestCase('u', 35)]
            [TestCase('k', 35)]
            public void UncommonLetters_ReturnsExpectedSet(char uncommonLetter, int expectedNumberOfPatterns)
            {
                ReadDownColumnPuzzle puzzle = new ReadDownColumnPuzzle();
                puzzle.Solution = "abcdeghilmnoprstw" + uncommonLetter;
                //2.If Q, J, X, Z, remove those patterns NOT in the IncludeFor<Letter> list
                //3.If V, U, F, Y, K, remove selected patterns in the ExcludeFor<Letter> list
                var puzzleRepository = new WordRepository();
                foreach (string pattern in puzzle.AcceptablePatterns)
                {
                    foreach (char letter in puzzle.Solution)
                    {
                        var patternWithLetter = pattern.Replace('1', letter);
                        Assert.Less(0, puzzleRepository.WordsMatchingPattern(patternWithLetter).Count, $"{patternWithLetter} should have had at least one match.");
                    }
                }
                Assert.AreEqual(expectedNumberOfPatterns, puzzle.AcceptablePatterns.Count);
            }

            [Test]
            public void FullAlphabet_ReturnsExpectedSet()
            {
                ReadDownColumnPuzzle puzzle = new ReadDownColumnPuzzle();
                puzzle.Solution = "abcdeghilmnoprstw" + "qjxz" + "vufyk";
                //2.If Q, J, X, Z, remove those patterns NOT in the IncludeFor<Letter> list
                //3.If V, U, F, Y, K, remove selected patterns in the ExcludeFor<Letter> list
                var puzzleRepository = new WordRepository();
                foreach (string pattern in puzzle.AcceptablePatterns)
                {
                    foreach (char letter in puzzle.Solution)
                    {
                        var patternWithLetter = pattern.Replace('1', letter);
                        Assert.Less(0, puzzleRepository.WordsMatchingPattern(patternWithLetter).Count, $"{patternWithLetter} should have had at least one match.");
                        if (0 == puzzle.Repository.WordsMatchingPattern(patternWithLetter).Count)
                        {
                            Console.WriteLine($"{patternWithLetter} had zero hits");
                        }
                    }
                }
                Assert.AreEqual(1, puzzle.AcceptablePatterns.Count);
            }

        }

        [TestFixture]
        public class RandomizePattern
        {
            [Test]
            public void CommonLetters_ResetsPattern()
            {
                ReadDownColumnPuzzle puzzle = new ReadDownColumnPuzzle() {RandomSeed = 43};
                puzzle.Solution = "abcd"; //all common letters.

                Assert.Less(1, puzzle.AcceptablePatterns.Count, "Should be more than 1 acceptable pattern");

                puzzle.RandomizePattern();
                Assert.AreEqual("_1___", puzzle.SelectedPattern);
                Assert.AreEqual(5, puzzle.Size);
                Assert.AreEqual(1, puzzle.ZeroBasedIndexOfSolution);
            }
        }

    }
}
