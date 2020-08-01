using System.Collections.Generic;
using NUnit.Framework;
using WordPuzzles.Puzzle;

namespace WordPuzzlesTest.NetFramework.Puzzle
{
    [TestFixture]
    public class PuzzleWordTest
    {
        [TestFixture]
        public class Constructor
        {
            [Test]
            public void SingleWord_HasExpectedLetters()
            {
                PuzzleWord word = new PuzzleWord("as", 1, 'A');
                PuzzleLetter firstLetter = word.Letters[0];
                Assert.AreEqual('a', firstLetter.ActualLetter);
                Assert.AreEqual(1, firstLetter.NumericIndex);
                Assert.AreEqual('A', firstLetter.AlphabeticIndex);

                PuzzleLetter secondLetter = word.Letters[1];
                Assert.AreEqual('s', secondLetter.ActualLetter);
                Assert.AreEqual(2, secondLetter.NumericIndex);
                Assert.AreEqual('A', secondLetter.AlphabeticIndex);

            }
        }
    }

    [TestFixture]
    public class PuzzleTest
    {
        [TestFixture]
        public class Constructor
        {
            [Test]
            public void MultipleWords_HaveExpectedLetters()
            {
                WordPuzzles.Puzzle.Puzzle puzzle = new WordPuzzles.Puzzle.Puzzle();
                puzzle.AddWordToClues("as");
                puzzle.AddWordToClues("is");
                PuzzleWord firstWord = puzzle.Clues[0];
                PuzzleLetter firstLetter = firstWord.Letters[0];
                Assert.AreEqual('a', firstLetter.ActualLetter);
                Assert.AreEqual(1, firstLetter.NumericIndex);
                Assert.AreEqual('A', firstLetter.AlphabeticIndex);

                PuzzleLetter secondLetter = firstWord.Letters[1];
                Assert.AreEqual('s', secondLetter.ActualLetter);
                Assert.AreEqual(2, secondLetter.NumericIndex);
                Assert.AreEqual('A', secondLetter.AlphabeticIndex);

                PuzzleWord secondWord = puzzle.Clues[1];
                PuzzleLetter firstLetterInSecondWord = secondWord.Letters[0];
                Assert.AreEqual('i', firstLetterInSecondWord.ActualLetter);
                Assert.AreEqual(3, firstLetterInSecondWord.NumericIndex);
                Assert.AreEqual('B', firstLetterInSecondWord.AlphabeticIndex);

                PuzzleLetter secondLetterInSecondWord = secondWord.Letters[1];
                Assert.AreEqual('s', secondLetterInSecondWord.ActualLetter);
                Assert.AreEqual(4, secondLetterInSecondWord.NumericIndex);
                Assert.AreEqual('B', secondLetterInSecondWord.AlphabeticIndex);
            }
        }

        [TestFixture]
        public class PlaceUniqueLetters
        {
            [Test]
            public void PlacesRequiredLetters()
            {
                WordPuzzles.Puzzle.Puzzle puzzle = new WordPuzzles.Puzzle.Puzzle();
                puzzle.AddWordToClues("as");
                puzzle.AddWordToClues("is");

                puzzle.PhraseAsString = "as is";
                puzzle.PlaceUniqueLetters();
                Assert.AreEqual(5, puzzle.Phrase.Count);

                Assert.AreEqual('a', puzzle.Phrase[0].ActualLetter);
                Assert.AreEqual('A', puzzle.Phrase[0].AlphabeticIndex);
                Assert.AreEqual(1, puzzle.Phrase[0].NumericIndex);

                Assert.IsNull(puzzle.Phrase[1]);
                Assert.AreEqual(' ',  puzzle.Phrase[2].ActualLetter);

                Assert.AreEqual('i', puzzle.Phrase[3].ActualLetter);
                Assert.AreEqual('B', puzzle.Phrase[3].AlphabeticIndex);
                Assert.AreEqual(3, puzzle.Phrase[3].NumericIndex);

                Assert.IsNull(puzzle.Phrase[4]);
            }

            [Test]
            public void HandlesPunctuation()
            {
                WordPuzzles.Puzzle.Puzzle puzzleWithPunctuation = new WordPuzzles.Puzzle.Puzzle {PhraseAsString = "i'm x"};

                puzzleWithPunctuation.AddWordToClues("mix");

                puzzleWithPunctuation.PlaceUniqueLetters();

                

            }
        }

        [TestFixture]
        public class CalculateOptions
        {

            [Test]
            public void AsIsExample_FindsNothingForAlreadyPlacedFirstLetter()
            {
                WordPuzzles.Puzzle.Puzzle puzzle = new WordPuzzles.Puzzle.Puzzle();
                puzzle.AddWordToClues("as");
                puzzle.AddWordToClues("is");

                puzzle.PhraseAsString = "as is";
                puzzle.PlaceUniqueLetters();

                List<PuzzleLetter> options = puzzle.CalculateOptions(0);

                Assert.AreEqual(0, options.Count);
            }

            [Test]
            public void AsIsExample_FindsOptionsForSecondLetter()
            {
                WordPuzzles.Puzzle.Puzzle puzzle = new WordPuzzles.Puzzle.Puzzle();
                puzzle.AddWordToClues("as");
                puzzle.AddWordToClues("is");

                puzzle.PhraseAsString = "as is";
                puzzle.PlaceUniqueLetters();

                List<PuzzleLetter> options = puzzle.CalculateOptions(1);

                Assert.AreEqual(2, options.Count);
                Assert.AreEqual('s', options[0].ActualLetter);
                Assert.AreEqual('s', options[1].ActualLetter);
            }

            [Test]
            public void AsIsExample_ExcludingWordAlreadyPlaced_FindsSingleOptionForSecondLetter()
            {
                WordPuzzles.Puzzle.Puzzle puzzle = new WordPuzzles.Puzzle.Puzzle();
                puzzle.AddWordToClues("as");
                puzzle.AddWordToClues("is");

                puzzle.PhraseAsString = "as is";
                puzzle.PlaceUniqueLetters();

                List<PuzzleLetter> options = puzzle.CalculateOptions(1, "A"); //Skip letters from first clue word A, because the first letter is already from that word.

                Assert.AreEqual(1, options.Count);
                Assert.AreEqual('s', options[0].ActualLetter);
                Assert.AreEqual('B', options[0].AlphabeticIndex);
                Assert.AreEqual(4, options[0].NumericIndex);
            }

        }

        [TestFixture]
        public class PlaceForcedLetters
        {
            [Test]
            public void AsIsExample_PlacesRemainingLetters()
            {
                WordPuzzles.Puzzle.Puzzle puzzle = new WordPuzzles.Puzzle.Puzzle();
                puzzle.AddWordToClues("as");
                puzzle.AddWordToClues("is");

                puzzle.PhraseAsString = "as is";
                puzzle.PlaceUniqueLetters();

                puzzle.PlaceForcedLetters(out _);

                Assert.AreEqual('a', puzzle.Phrase[0].ActualLetter);
                Assert.AreEqual('s', puzzle.Phrase[1].ActualLetter);
                Assert.AreEqual(' ', puzzle.Phrase[2].ActualLetter);
                Assert.AreEqual('i', puzzle.Phrase[3].ActualLetter);
                Assert.AreEqual('s', puzzle.Phrase[4].ActualLetter);

            }
        }

        [TestFixture]
        public class PlaceLetters
        {
            [Test]
            public void FirstExample_PlacesAllLetters()
            {
                WordPuzzles.Puzzle.Puzzle puzzle = new WordPuzzles.Puzzle.Puzzle();
                puzzle.AddWordToClues("agility");
                puzzle.AddWordToClues("quite");
                puzzle.AddWordToClues("tethers");
                puzzle.AddWordToClues("vapor");
                puzzle.AddWordToClues("chew");
                puzzle.AddWordToClues("chives");
                puzzle.AddWordToClues("xenon");
                puzzle.AddWordToClues("place");
                puzzle.AddWordToClues("while");
                puzzle.AddWordToClues("mists");

                const string PHRASE = "which vowel appears in this clever question exactly eight times";
                puzzle.PhraseAsString = PHRASE;

                puzzle.PlaceLetters();
                int index = 0;
                foreach (char letter in PHRASE)
                {
                    Assert.AreEqual(letter, puzzle.Phrase[index].ActualLetter, $"unexpected letter at index {index}");
                    Assert.AreEqual(true, puzzle.Phrase[index].AlreadyPlaced, $"index {index} was not placed");
                    index++;
                }

            }

            [Test]
            public void SecondExample_PlacesAllLetters()
            {
                WordPuzzles.Puzzle.Puzzle puzzle = new WordPuzzles.Puzzle.Puzzle();
                puzzle.AddWordToClues("donated");
                puzzle.AddWordToClues("two");
                puzzle.AddWordToClues("monster");
                puzzle.AddWordToClues("heal");
                puzzle.AddWordToClues("closet");
                puzzle.AddWordToClues("great");
                puzzle.AddWordToClues("first");
                puzzle.AddWordToClues("water");
                puzzle.AddWordToClues("death");
                puzzle.AddWordToClues("dated");

                const string PHRASE = "add this letter to the start of deal and mage to create new words";
                puzzle.PhraseAsString = PHRASE;

                puzzle.PlaceLetters();
                int index = 0;
                foreach (char letter in PHRASE)
                {
                    Assert.AreEqual(letter, puzzle.Phrase[index].ActualLetter, $"unexpected letter at index {index}");
                    Assert.AreEqual(true, puzzle.Phrase[index].AlreadyPlaced, $"index {index} was not placed");
                    index++;
                }
            }

            [Test]
            public void ExampleWithPunctuation()
            {
                WordPuzzles.Puzzle.Puzzle puzzle = new WordPuzzles.Puzzle.Puzzle {PhraseAsString = "i'm x."};
                puzzle.AddWordToClues("mix");

                puzzle.PlaceLetters();
                Assert.AreEqual('i', puzzle.Phrase[0].ActualLetter);
                Assert.AreEqual('\'', puzzle.Phrase[1].ActualLetter);
                Assert.AreEqual('m', puzzle.Phrase[2].ActualLetter);
                Assert.AreEqual(' ', puzzle.Phrase[3].ActualLetter);
                Assert.AreEqual('x', puzzle.Phrase[4].ActualLetter);
                Assert.AreEqual('.', puzzle.Phrase[5].ActualLetter);

            }

        }
    }

}