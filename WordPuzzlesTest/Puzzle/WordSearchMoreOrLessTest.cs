using System.Collections.Generic;
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
                Assert.AreEqual("paint", firstHiddenWord.HiddenWord, "Expected hidden word");
                Assert.AreEqual("pint", firstHiddenWord.DisplayedWord, "Expected displayed word");
                Assert.AreEqual(2, firstHiddenWord.XCoordinate, "Unexpected XCoordinate ");
                Assert.AreEqual(0, firstHiddenWord.YCoordinate, "Unexpected XCoordinate ");
                Assert.AreEqual(CardinalDirection.South, firstHiddenWord.Direction, "Unexpected XCoordinate ");
                List<string> expectedGrid = new List<string>()
                {
                    "__p__",
                    "__a__",
                    "__i__",
                    "__n__",
                    "__t__",
                };
                Assert.AreEqual(expectedGrid,wordSearch.Grid);

                wordSearch.ProcessLetter('r');
                Assert.AreEqual(2, wordSearch.HiddenWords.Count, "Expected one hidden word before processing a letter.");
                var secondHiddenWord = wordSearch.HiddenWords[1];
                Assert.AreEqual('r', secondHiddenWord.LetterAddedOrRemoved, "Expected A as first letter removed.");
                Assert.AreEqual("horse", secondHiddenWord.HiddenWord, "Expected hidden word");
                Assert.AreEqual("hose", secondHiddenWord.DisplayedWord, "Expected displayed word");
                Assert.AreEqual(1, secondHiddenWord.XCoordinate, "Unexpected XCoordinate ");
                Assert.AreEqual(4, secondHiddenWord.YCoordinate, "Unexpected XCoordinate ");
                Assert.AreEqual(CardinalDirection.North, secondHiddenWord.Direction, "Unexpected XCoordinate ");
                expectedGrid = new List<string>()
                {
                    "_ep__",
                    "_sa__",
                    "_ri__",
                    "_on__",
                    "_ht__",
                };
                Assert.AreEqual(expectedGrid, wordSearch.Grid);

                wordSearch.ProcessLetter('t');
                Assert.AreEqual(3, wordSearch.HiddenWords.Count, "Expected one hidden word before processing a letter.");
                var thirdHiddenWord = wordSearch.HiddenWords[2];
                Assert.AreEqual('t', thirdHiddenWord.LetterAddedOrRemoved, "Expected A as first letter removed.");
                Assert.AreEqual("stew", thirdHiddenWord.HiddenWord, "Expected hidden word");
                Assert.AreEqual("sew", thirdHiddenWord.DisplayedWord, "Expected displayed word");
                Assert.AreEqual(0, thirdHiddenWord.XCoordinate, "Unexpected XCoordinate ");
                Assert.AreEqual(0, thirdHiddenWord.YCoordinate, "Unexpected XCoordinate ");
                Assert.AreEqual(CardinalDirection.South, thirdHiddenWord.Direction, "Unexpected XCoordinate ");
                expectedGrid = new List<string>()
                {
                    "sep__",
                    "tsa__",
                    "eri__",
                    "won__",
                    "_ht__",
                };
                Assert.AreEqual(expectedGrid, wordSearch.Grid);

                wordSearch.ProcessLetter('s');
                Assert.AreEqual(4, wordSearch.HiddenWords.Count, "Expected one hidden word before processing a letter.");
                var fourthHiddenWord = wordSearch.HiddenWords[3];
                Assert.AreEqual('s', fourthHiddenWord.LetterAddedOrRemoved, "Expected A as first letter removed.");
                Assert.AreEqual("seat", fourthHiddenWord.HiddenWord, "Expected hidden word");
                Assert.AreEqual("eat", fourthHiddenWord.DisplayedWord, "Expected displayed word");
                Assert.AreEqual(3, fourthHiddenWord.XCoordinate, "Unexpected XCoordinate ");
                Assert.AreEqual(1, fourthHiddenWord.YCoordinate, "Unexpected XCoordinate ");
                Assert.AreEqual(CardinalDirection.South, fourthHiddenWord.Direction, "Unexpected XCoordinate ");
                expectedGrid = new List<string>()
                {
                    "sep__",
                    "tsas_",
                    "erie_",
                    "wona_",
                    "_htt_",
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
                Assert.AreEqual(expectedDirections, wordSearch.GetPossibleDirections(x, y, length));
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
                Assert.AreEqual(expectedDirections, wordSearch.GetPossibleDirections(x, y, length));
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
                Assert.AreEqual(expectedDirections, wordSearch.GetPossibleDirections(x, y, length));
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
                Assert.AreEqual(expectedDirections, wordSearch.GetPossibleDirections(x, y, length));
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
                Assert.AreEqual(expectedDirections, wordSearch.GetPossibleDirections(x, y, length));
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
                Assert.AreEqual(expectedDirections, wordSearch.GetPossibleDirections(x, y, length));
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
                Assert.AreEqual(expectedDirections, wordSearch.GetPossibleDirections(x, y, length));
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
                Assert.AreEqual(expectedDirections, wordSearch.GetPossibleDirections(x, y, length));
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
                Assert.AreEqual(expectedDirections, wordSearch.GetPossibleDirections(x, y, length));
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

    }


}