using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using NUnit.Framework;
using WordPuzzles.Puzzle;

namespace WordPuzzlesTest.Puzzle
{
    [TestFixture]
    public class WordSearchMoreOrLessTest
    {
        [TestFixture]
        public class ProcessLetter
        {
            [Test]
            public void AddsToHiddenWordList()
            {

                var wordSearch = new WordSearchMoreOrLess {RandomGeneratorSeed = 42, Size = 5};
                Assert.AreEqual(0, wordSearch.HiddenWords.Count, "Expected no hidden words before processing a letter.");

                wordSearch.ProcessLetter('a');
                Assert.AreEqual(1, wordSearch.HiddenWords.Count, "Expected one hidden word before processing a letter.");
                var firstHiddenWord = wordSearch.HiddenWords[0];
                Assert.AreEqual('a', firstHiddenWord.LetterAddedOrRemoved, "Expected A as first letter removed.");
                Assert.AreEqual("pint", firstHiddenWord.HiddenWord, "Expected hidden word");
                Assert.AreEqual("paint", firstHiddenWord.DisplayedWord, "Expected displayed word");
                Assert.AreEqual(2, firstHiddenWord.XCoordinate, "Unexpected XCoordinate ");
                Assert.AreEqual(3, firstHiddenWord.YCoordinate, "Unexpected XCoordinate ");
                Assert.AreEqual(CardinalDirection.North, firstHiddenWord.Direction, "Unexpected XCoordinate ");
                List<string> expectedGrid = new List<string>()
                {
                    "__t__",
                    "__n__",
                    "__i__",
                    "__p__",
                    "_____",
                };
                Assert.AreEqual(expectedGrid,wordSearch.Grid);

                wordSearch.ProcessLetter('r');
                Assert.AreEqual(2, wordSearch.HiddenWords.Count, "Expected one hidden word before processing a letter.");
                var secondHiddenWord = wordSearch.HiddenWords[1];
                Assert.AreEqual('r', secondHiddenWord.LetterAddedOrRemoved, "Expected A as first letter removed.");
                Assert.AreEqual("pat", secondHiddenWord.HiddenWord, "Expected hidden word");
                Assert.AreEqual("part", secondHiddenWord.DisplayedWord, "Expected displayed word");
                Assert.AreEqual(2, secondHiddenWord.XCoordinate, "Unexpected XCoordinate ");
                Assert.AreEqual(4, secondHiddenWord.YCoordinate, "Unexpected YCoordinate ");
                Assert.AreEqual(CardinalDirection.NorthEast, secondHiddenWord.Direction, "Unexpected Direction ");
                expectedGrid = new List<string>()
                {
                    "__t__",
                    "__n__",
                    "__i_t",
                    "__pa_",
                    "__p__",
                };
                Assert.AreEqual(expectedGrid, wordSearch.Grid);

                wordSearch.ProcessLetter('t');
                Assert.AreEqual(3, wordSearch.HiddenWords.Count, "Expected one hidden word before processing a letter.");
                var thirdHiddenWord = wordSearch.HiddenWords[2];
                Assert.AreEqual('t', thirdHiddenWord.LetterAddedOrRemoved, "Expected A as first letter removed.");
                Assert.AreEqual("sank", thirdHiddenWord.HiddenWord, "Expected hidden word");
                Assert.AreEqual("stank", thirdHiddenWord.DisplayedWord, "Expected displayed word");
                Assert.AreEqual(1, thirdHiddenWord.XCoordinate, "Unexpected XCoordinate ");
                Assert.AreEqual(1, thirdHiddenWord.YCoordinate, "Unexpected YCoordinate ");
                Assert.AreEqual(CardinalDirection.South, thirdHiddenWord.Direction, "Unexpected Direction ");
                expectedGrid = new List<string>()
                {
                    "__t__",
                    "_sn__",
                    "_ai_t",
                    "_npa_",
                    "_kp__",
                };
                Assert.AreEqual(expectedGrid, wordSearch.Grid);

                wordSearch.ProcessLetter('s');
                Assert.AreEqual(4, wordSearch.HiddenWords.Count, "Expected one hidden word before processing a letter.");
                var fourthHiddenWord = wordSearch.HiddenWords[3];
                Assert.AreEqual('s', fourthHiddenWord.LetterAddedOrRemoved, "Expected A as first letter removed.");
                Assert.AreEqual("muter", fourthHiddenWord.HiddenWord, "Expected hidden word");
                Assert.AreEqual("muster", fourthHiddenWord.DisplayedWord, "Expected displayed word");
                Assert.AreEqual(4, fourthHiddenWord.XCoordinate, "Unexpected XCoordinate ");
                Assert.AreEqual(4, fourthHiddenWord.YCoordinate, "Unexpected YCoordinate ");
                Assert.AreEqual(CardinalDirection.North, fourthHiddenWord.Direction, "Unexpected Direction ");
                expectedGrid = new List<string>()
                {
                    "__t_r",
                    "_sn_e",
                    "_ai_t",
                    "_npau",
                    "_kp_m",
                };
                Assert.AreEqual(expectedGrid, wordSearch.Grid);

            }

        }

        private static WordSearchMoreOrLess sharedWordSearch = null;
        private static WordSearchMoreOrLess GetWordSearchMoreOrLess()
        {
            if (sharedWordSearch == null)
            {
                sharedWordSearch = new WordSearchMoreOrLess();
                sharedWordSearch.RandomGeneratorSeed = 42;
                sharedWordSearch.Size = 5;
            }

            return sharedWordSearch;
        }

        [TestFixture]
        public class FindStringInGrid
        {

            [Test]
            public void Simple_North_FindsExpectedWord()
            {
                WordSearchMoreOrLess wordSearch = GetWordSearchMoreOrLess();
                wordSearch.Size = 5;
                wordSearch.Grid = new List<string>()
                {
                    "__T__",
                    "__A__",
                    "__C__",
                    "_____",
                    "_____",
                };
                Assert.AreEqual("CAT", wordSearch.FindStringInGrid(2, 2, CardinalDirection.North, 3));
            }

            [Test]
            public void Simple_NorthEast_FindsExpectedWord()
            {
                WordSearchMoreOrLess wordSearch = GetWordSearchMoreOrLess();
                wordSearch.Size = 5;
                wordSearch.Grid = new List<string>()
                {
                    "____T",
                    "___A_",
                    "__C__",
                    "_____",
                    "_____",
                };
                Assert.AreEqual("CAT", wordSearch.FindStringInGrid(2, 2, CardinalDirection.NorthEast, 3));
            }

            [Test]
            public void Simple_East_FindsExpectedWord()
            {
                WordSearchMoreOrLess wordSearch = GetWordSearchMoreOrLess();
                wordSearch.Size = 4;
                wordSearch.Grid = new List<string>()
                {
                    "ISLE",
                    "____",
                    "____",
                    "____",
                };
                Assert.AreEqual("ISLE", wordSearch.FindStringInGrid(0, 0, CardinalDirection.East, 4) );
            }

            [Test]
            public void Simple_SouthEast_FindsExpectedWord()
            {
                WordSearchMoreOrLess wordSearch = GetWordSearchMoreOrLess();
                wordSearch.Size = 5;
                wordSearch.Grid = new List<string>()
                {
                    "_____",
                    "_____",
                    "__C__",
                    "___A_",
                    "____T",
                };
                Assert.AreEqual("CAT", wordSearch.FindStringInGrid(2, 2, CardinalDirection.SouthEast, 3));
            }

            [Test]
            public void Simple_South_FindsExpectedWord()
            {
                WordSearchMoreOrLess wordSearch = GetWordSearchMoreOrLess();
                wordSearch.Size = 5;
                wordSearch.Grid = new List<string>()
                {
                    "_____",
                    "_____",
                    "__C__",
                    "__A__",
                    "__T__",
                };
                Assert.AreEqual("CAT", wordSearch.FindStringInGrid(2, 2, CardinalDirection.South, 3));
            }

            [Test]
            public void Simple_SouthWest_FindsExpectedWord()
            {
                WordSearchMoreOrLess wordSearch = GetWordSearchMoreOrLess();
                wordSearch.Size = 5;
                wordSearch.Grid = new List<string>()
                {
                    "_____",
                    "_____",
                    "__C__",
                    "_A___",
                    "T____",
                };
                Assert.AreEqual("CAT", wordSearch.FindStringInGrid(2, 2, CardinalDirection.SouthWest, 3));
            }

            [Test]
            public void Simple_West_FindsExpectedWord()
            {
                WordSearchMoreOrLess wordSearch = GetWordSearchMoreOrLess();
                wordSearch.Size = 5;
                wordSearch.Grid = new List<string>()
                {
                    "_____",
                    "_____",
                    "TAC__",
                    "_____",
                    "_____",
                };
                Assert.AreEqual("CAT", wordSearch.FindStringInGrid(2, 2, CardinalDirection.West, 3));
            }
            [Test]
            public void Simple_NorthWest_FindsExpectedWord()
            {
                WordSearchMoreOrLess wordSearch = GetWordSearchMoreOrLess();
                wordSearch.Size = 5;
                wordSearch.Grid = new List<string>()
                {
                    "T____",
                    "_A___",
                    "__C__",
                    "_____",
                    "_____",
                };
                Assert.AreEqual("CAT", wordSearch.FindStringInGrid(2, 2, CardinalDirection.NorthWest, 3));
            }
        }

        [TestFixture]
        public class PlaceStringInGrid
        {
            private List<string> EMPTY_FIVE_GRID = new List<string>(){
                "_____",
                "_____",
                "_____",
                "_____",
                "_____",
            };

            [Test]
            public void North_CreatesExpectedGrid()
            {
                WordSearchMoreOrLess wordSearch = new WordSearchMoreOrLess() { RandomGeneratorSeed = 42 };
                wordSearch.Size = 5;
                var expectedGrid = new List<string>()
                {
                    "__T__",
                    "__A__",
                    "__C__",
                    "_____",
                    "_____",
                };
                wordSearch.Grid = new List<string>( EMPTY_FIVE_GRID);
                wordSearch.PlaceStringInGrid("CAT", 2, 2, CardinalDirection.North);
                Assert.AreEqual(expectedGrid, wordSearch.Grid, "Unexpected grid.");
            }


            [Test]
            public void Simple_NorthEast_FindsExpectedWord()
            {
                WordSearchMoreOrLess wordSearch = new WordSearchMoreOrLess() { RandomGeneratorSeed = 42 };
                wordSearch.Size = 5;
                var expectedGrid = new List<string>()
                {
                    "____T",
                    "___A_",
                    "__C__",
                    "_____",
                    "_____",
                };
                wordSearch.Grid = new List<string>(EMPTY_FIVE_GRID);
                wordSearch.PlaceStringInGrid("CAT", 2, 2, CardinalDirection.NorthEast);

                Assert.AreEqual(expectedGrid, wordSearch.Grid);
            }

            [Test]
            public void Simple_East_FindsExpectedWord()
            {
                WordSearchMoreOrLess wordSearch = new WordSearchMoreOrLess() { RandomGeneratorSeed = 42 };
                wordSearch.Size = 5;
                var expectedGrid = new List<string>()
                {
                    "_____",
                    "_____",
                    "__CAT",
                    "_____",
                    "_____",
                };
                wordSearch.Grid = new List<string>(EMPTY_FIVE_GRID);
                wordSearch.PlaceStringInGrid("CAT", 2, 2, CardinalDirection.East);

                Assert.AreEqual(expectedGrid, wordSearch.Grid);
            }

            [Test]
            public void Simple_SouthEast_FindsExpectedWord()
            {
                WordSearchMoreOrLess wordSearch = new WordSearchMoreOrLess() { RandomGeneratorSeed = 42 };
                wordSearch.Size = 5;
                var expectedGrid = new List<string>()
                {
                    "_____",
                    "_____",
                    "__C__",
                    "___A_",
                    "____T",
                };
                wordSearch.Grid = new List<string>( EMPTY_FIVE_GRID);
                wordSearch.PlaceStringInGrid("CAT", 2, 2, CardinalDirection.SouthEast);
                Assert.AreEqual(expectedGrid, wordSearch.Grid);
            }

            [Test]
            public void Simple_South_FindsExpectedWord()
            {

                WordSearchMoreOrLess wordSearch = new WordSearchMoreOrLess() { RandomGeneratorSeed = 42 };
                wordSearch.Size = 5;
                var expectedGrid = new List<string>()
                {
                    "_____",
                    "_____",
                    "__C__",
                    "__A__",
                    "__T__",
                };
                wordSearch.Grid = new List<string>(EMPTY_FIVE_GRID);
                wordSearch.PlaceStringInGrid("CAT",2, 2, CardinalDirection.South);
                Assert.AreEqual(expectedGrid, wordSearch.Grid);
            }

            [Test]
            public void Simple_SouthWest_FindsExpectedWord()
            {
                WordSearchMoreOrLess wordSearch = new WordSearchMoreOrLess() { RandomGeneratorSeed = 42 };
                wordSearch.Size = 5;
                var expectedGrid = new List<string>()
                {
                    "_____",
                    "_____",
                    "__C__",
                    "_A___",
                    "T____",
                };
                wordSearch.Grid = new List<string>(EMPTY_FIVE_GRID);
                wordSearch.PlaceStringInGrid("CAT",2, 2, CardinalDirection.SouthWest);
                Assert.AreEqual(expectedGrid, wordSearch.Grid);
            }

            [Test]
            public void Simple_West_FindsExpectedWord()
            {
                WordSearchMoreOrLess wordSearch = new WordSearchMoreOrLess() { RandomGeneratorSeed = 42 };
                wordSearch.Size = 5;
                var expectedGrid = new List<string>()
                {
                    "_____",
                    "_____",
                    "TAC__",
                    "_____",
                    "_____",
                };
                wordSearch.Grid = new List<string>(EMPTY_FIVE_GRID);
                wordSearch.PlaceStringInGrid("CAT", 2, 2, CardinalDirection.West);
                Assert.AreEqual(expectedGrid, wordSearch.Grid);
            }
            [Test]
            public void Simple_NorthWest_FindsExpectedWord()
            {
                WordSearchMoreOrLess wordSearch = new WordSearchMoreOrLess() { RandomGeneratorSeed = 42 };
                wordSearch.Size = 5;
                var expectedGrid = new List<string>()
                {
                    "T____",
                    "_A___",
                    "__C__",
                    "_____",
                    "_____",
                };
                wordSearch.Grid = new List<string>(EMPTY_FIVE_GRID);
                wordSearch.PlaceStringInGrid("CAT", 2, 2, CardinalDirection.NorthWest);
                Assert.AreEqual(expectedGrid, wordSearch.Grid);
            }

        }

        [TestFixture]
        public class GetPossibleDirections
        {
            [Test]
            public void Center_ReturnsExpectedDirections()
            {
                WordSearchMoreOrLess wordSearch = GetWordSearchMoreOrLess();
                List<CardinalDirection> expectedDirections = new List<CardinalDirection>()
                {
                    CardinalDirection.North, 
                    CardinalDirection.NorthEast, 
                    CardinalDirection.East, 
                    CardinalDirection.SouthEast, 
                    CardinalDirection.South, 
                    CardinalDirection.SouthWest, 
                    CardinalDirection.West, 
                    CardinalDirection.NorthWest
                };

                var x = 2;
                var y = 2;
                var length = 3;
                CollectionAssert.AreEquivalent(expectedDirections, wordSearch.GetPossibleDirections(x, y, length));
            }


            [Test]
            public void OneStepNorth_ReturnsExpectedDirections()
            {
                WordSearchMoreOrLess wordSearch = GetWordSearchMoreOrLess();
                List<CardinalDirection> expectedDirections = new List<CardinalDirection>()
                {
                    CardinalDirection.East,
                    CardinalDirection.SouthEast,
                    CardinalDirection.South,
                    CardinalDirection.SouthWest,
                    CardinalDirection.West,
                };

                var x = 2;
                var y = 1;
                var length = 3;
                CollectionAssert.AreEquivalent(expectedDirections, wordSearch.GetPossibleDirections(x, y, length));
            }

            [Test]
            public void OneStepSouth_ReturnsExpectedDirections()
            {
                WordSearchMoreOrLess wordSearch = GetWordSearchMoreOrLess();
                List<CardinalDirection> expectedDirections = new List<CardinalDirection>()
                {
                    CardinalDirection.North,
                    CardinalDirection.NorthEast,
                    CardinalDirection.East,
                    CardinalDirection.West,
                    CardinalDirection.NorthWest
                };

                var x = 2;
                var y = 3;
                var length = 3;
                CollectionAssert.AreEquivalent(expectedDirections, wordSearch.GetPossibleDirections(x, y, length));
            }

            [Test]
            public void OneStepWest_ReturnsExpectedDirections()
            {
                WordSearchMoreOrLess wordSearch = GetWordSearchMoreOrLess();
                List<CardinalDirection> expectedDirections = new List<CardinalDirection>()
                {
                    CardinalDirection.North,
                    CardinalDirection.NorthEast,
                    CardinalDirection.East,
                    CardinalDirection.SouthEast,
                    CardinalDirection.South,
                };

                var x = 1;
                var y = 2;
                var length = 3;
                CollectionAssert.AreEquivalent(expectedDirections, wordSearch.GetPossibleDirections(x, y, length));
            }

            [Test]
            public void OneStepEast_ReturnsExpectedDirections()
            {
                WordSearchMoreOrLess wordSearch = GetWordSearchMoreOrLess();
                List<CardinalDirection> expectedDirections = new List<CardinalDirection>()
                {
                    CardinalDirection.North,
                    CardinalDirection.South,
                    CardinalDirection.SouthWest,
                    CardinalDirection.West,
                    CardinalDirection.NorthWest
                };

                var x = 3;
                var y = 2;
                var length = 3;
                CollectionAssert.AreEquivalent(expectedDirections, wordSearch.GetPossibleDirections(x, y, length));
            }

            [Test]
            public void OneStepNorthEast_ReturnsExpectedDirections()
            {
                WordSearchMoreOrLess wordSearch = GetWordSearchMoreOrLess();
                List<CardinalDirection> expectedDirections = new List<CardinalDirection>()
                {
                    CardinalDirection.South,
                    CardinalDirection.SouthWest,
                    CardinalDirection.West,
                };

                var x = 3;
                var y = 1;
                var length = 3;
                CollectionAssert.AreEquivalent(expectedDirections, wordSearch.GetPossibleDirections(x, y, length));
            }

            [Test]
            public void OneStepSouthEast_ReturnsExpectedDirections()
            {
                WordSearchMoreOrLess wordSearch = GetWordSearchMoreOrLess();
                List<CardinalDirection> expectedDirections = new List<CardinalDirection>()
                {
                    CardinalDirection.North,
                    CardinalDirection.West,
                    CardinalDirection.NorthWest
                };

                var x = 3;
                var y = 3;
                var length = 3;
                CollectionAssert.AreEquivalent(expectedDirections, wordSearch.GetPossibleDirections(x, y, length));
            }

            [Test]
            public void OneStepSouthWest_ReturnsExpectedDirections()
            {
                WordSearchMoreOrLess wordSearch = GetWordSearchMoreOrLess();
                List<CardinalDirection> expectedDirections = new List<CardinalDirection>()
                {
                    CardinalDirection.North,
                    CardinalDirection.NorthEast,
                    CardinalDirection.East,
                };

                var x = 1;
                var y = 3;
                var length = 3;
                CollectionAssert.AreEquivalent(expectedDirections, wordSearch.GetPossibleDirections(x, y, length));
            }

            [Test]
            public void OneStepNorthWest_ReturnsExpectedDirections()
            {
                WordSearchMoreOrLess wordSearch = GetWordSearchMoreOrLess();
                List<CardinalDirection> expectedDirections = new List<CardinalDirection>()
                {
                    CardinalDirection.East,
                    CardinalDirection.SouthEast,
                    CardinalDirection.South,
                };

                var x = 1;
                var y = 1;
                var length = 3;
                CollectionAssert.AreEquivalent(expectedDirections, wordSearch.GetPossibleDirections(x, y, length));
            }

        }


        [TestFixture]
        public class FillInRemainingGrid
        {
            [Test]
            public void CreatesExpectedGrid()
            {
                WordSearchMoreOrLess wordSearch = new WordSearchMoreOrLess();
                wordSearch.RandomGeneratorSeed = 42;
                wordSearch.Size = 5;
                wordSearch.Grid = new List<string>()
                {
                    "sep__",
                    "tsas_",
                    "erie_",
                    "wona_",
                    "_htt_",
                };
                var expectedGrid = new List<string>()
                {
                    "sepdd",
                    "tsasn",
                    "eriee",
                    "wonag",
                    "rhtts",
                };
                wordSearch.FillInRemainingGrid();
                Assert.AreEqual(expectedGrid, wordSearch.Grid);
            }
        }


        [TestFixture]
        public class FindForbiddenWords
        {
            [Test]
            public void ReturnsPoop()
            {
                WordSearchMoreOrLess wordSearchWithForbiddenWords = new WordSearchMoreOrLess();
                wordSearchWithForbiddenWords.Size = 4;
                wordSearchWithForbiddenWords.RandomGeneratorSeed = 42;

                wordSearchWithForbiddenWords.PlaceStringInGrid("poop", 0, 0, CardinalDirection.East);

                List<HiddenWordInGrid> forbiddenWords = wordSearchWithForbiddenWords.FindForbiddenWords();
                Assert.AreEqual(2, forbiddenWords.Count, "Found a forbidden word");
                var firstForbiddenWord = forbiddenWords[0];
                Assert.AreEqual("poop", firstForbiddenWord.HiddenWord);
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
                const string HTML_DIRECTORY = @"html\WordSearchMoreOrLess\";
                 string SOURCE_DIRECTORY = ConfigurationManager.AppSettings["SourceDirectory"] + "WordSearchMoreOrLess";

                var puzzle = new WordSearchMoreOrLess();
                puzzle.RandomGeneratorSeed = 42;
                puzzle.Size = 6;
                puzzle.SetSolution("pens");
                puzzle.FillInRemainingGrid();
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

            [Test]
            [TestCase(true)]
            [TestCase(false)]
            public void Phrase_ReturnsExpectedResult(bool includeSolution)
            {
                const string HTML_DIRECTORY = @"html\WordSearchMoreOrLess\";
                string SOURCE_DIRECTORY = ConfigurationManager.AppSettings["SourceDirectory"] + "WordSearchMoreOrLess";

                var puzzle = new WordSearchMoreOrLess();
                puzzle.RandomGeneratorSeed = 42;
                puzzle.Size = 10;
                puzzle.SetSolution("multiple word solution");
                puzzle.FillInRemainingGrid();
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
    }


}