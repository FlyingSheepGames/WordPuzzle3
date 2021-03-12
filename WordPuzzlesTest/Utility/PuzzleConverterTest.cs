using System.Collections.Generic;
using NUnit.Framework;
using WordPuzzles.Puzzle;
using Newtonsoft.Json;

namespace WordPuzzlesTest.Utility
{
    [TestFixture]
    public class PuzzleConverterTest
    {
        [TestFixture]
        public class SerializeAndDeserializeAreOpposites
        {
            [Test]
            public void WordSquare_Default_ReturnsExpectedObject()
            {
                WordSquare puzzleToSerialize = new WordSquare();

                string serializedPuzzle = JsonConvert.SerializeObject(puzzleToSerialize);
                Assert.LessOrEqual(serializedPuzzle.Length, 1000, "Expected less than a thousand characters.");

                WordSquare deserializedPuzzle = JsonConvert.DeserializeObject<WordSquare>(serializedPuzzle);

                Assert.AreEqual(puzzleToSerialize.Clues, deserializedPuzzle.Clues, "Unexpected difference in Clues");
                Assert.AreEqual(puzzleToSerialize.Description, deserializedPuzzle.Description, "Unexpected difference in Description");
                Assert.AreEqual(puzzleToSerialize.Lines, deserializedPuzzle.Lines, "Unexpected difference in Lines");
                Assert.AreEqual(puzzleToSerialize.Size, deserializedPuzzle.Size, "Unexpected difference in Size");
                Assert.AreEqual(puzzleToSerialize.Theme, deserializedPuzzle.Theme, "Unexpected difference in Theme");

                Assert.AreEqual(puzzleToSerialize.FormatHtmlForGoogle(true, true), 
                    deserializedPuzzle.FormatHtmlForGoogle(true, true), "Unexpected differences in generated HTML");
            }

            [Test]
            public void WordSquare_Modified_ReturnsExpectedObject()
            {
                WordSquare puzzleToSerialize = new WordSquare("test");
                puzzleToSerialize.SetWordAtIndex("test", 0);
                puzzleToSerialize.Clues[0] = "first clue";

                string serializedPuzzle = JsonConvert.SerializeObject(puzzleToSerialize);
                Assert.LessOrEqual(serializedPuzzle.Length, 1000, "Expected less than a thousand characters.");

                WordSquare deserializedPuzzle = JsonConvert.DeserializeObject<WordSquare>(serializedPuzzle);

                Assert.AreEqual(puzzleToSerialize.Clues, deserializedPuzzle.Clues, "Unexpected difference in Clues");
                Assert.AreEqual(puzzleToSerialize.Description, deserializedPuzzle.Description, "Unexpected difference in Description");
                Assert.AreEqual(puzzleToSerialize.Lines, deserializedPuzzle.Lines, "Unexpected difference in Lines");
                Assert.AreEqual(puzzleToSerialize.Size, deserializedPuzzle.Size, "Unexpected difference in Size");
                Assert.AreEqual(puzzleToSerialize.Theme, deserializedPuzzle.Theme, "Unexpected difference in Theme");

                Assert.AreEqual(puzzleToSerialize.FormatHtmlForGoogle(true, true),
                    deserializedPuzzle.FormatHtmlForGoogle(true, true), "Unexpected differences in generated HTML");

            }

            [Test]
            public void MultipleClues_Default_ReturnsExpectedObject()
            {
                MultipleCluesPuzzle puzzleToSerialize = new MultipleCluesPuzzle();
                string serializedPuzzle = JsonConvert.SerializeObject(puzzleToSerialize);
                Assert.LessOrEqual(serializedPuzzle.Length, 1000, "Expected less than a thousand characters.");

                MultipleCluesPuzzle deserializedPuzzle = JsonConvert.DeserializeObject<MultipleCluesPuzzle>(serializedPuzzle);
                Assert.AreEqual(puzzleToSerialize.Solution, deserializedPuzzle.Solution, "Unexpected difference in Solution");
                Assert.AreEqual(puzzleToSerialize.Description, deserializedPuzzle.Description, "Unexpected difference in Description");
                Assert.AreEqual(puzzleToSerialize.WordsWithClues, deserializedPuzzle.WordsWithClues, "Unexpected difference in WordsWithClues");

                Assert.AreEqual(puzzleToSerialize.FormatHtmlForGoogle(true, true),
                    deserializedPuzzle.FormatHtmlForGoogle(true, true), "Unexpected differences in generated HTML");

            }

            [Test]
            public void MultipleClues_Custom_ReturnsExpectedObject()
            {
                MultipleCluesPuzzle puzzleToSerialize = new MultipleCluesPuzzle();
                puzzleToSerialize.Solution = "test";
                puzzleToSerialize.AddWordWithClues("test", new List<string>() {"clue one", "clue two"});
                puzzleToSerialize.AddWordWithClues("testa", new List<string>() { "clue one", "clue two" });
                puzzleToSerialize.AddWordWithClues("testb", new List<string>() { "clue one", "clue two" });
                puzzleToSerialize.AddWordWithClues("testc", new List<string>() { "clue one", "clue two" });
                string serializedPuzzle = JsonConvert.SerializeObject(puzzleToSerialize);
                Assert.LessOrEqual(serializedPuzzle.Length, 1000, "Expected less than a thousand characters.");

                MultipleCluesPuzzle deserializedPuzzle = JsonConvert.DeserializeObject<MultipleCluesPuzzle>(serializedPuzzle);
                Assert.AreEqual(puzzleToSerialize.Solution, deserializedPuzzle.Solution, "Unexpected difference in Solution");
                Assert.AreEqual(puzzleToSerialize.Description, deserializedPuzzle.Description, "Unexpected difference in Description");
                Assert.AreEqual(puzzleToSerialize.WordsWithClues[0].Clues[0].ClueOrder, deserializedPuzzle.WordsWithClues[0].Clues[0].ClueOrder, "Unexpected difference in WordsWithClues");
                Assert.AreEqual(puzzleToSerialize.WordsWithClues[0].Clues[0].ClueText, deserializedPuzzle.WordsWithClues[0].Clues[0].ClueText, "Unexpected difference in WordsWithClues");

                string originalHtml = puzzleToSerialize.FormatHtmlForGoogle(true, true);
                string deserializedHtml = deserializedPuzzle.FormatHtmlForGoogle(true, true);
                Assert.AreEqual(originalHtml,  deserializedHtml, "Unexpected differences in generated HTML");
            }

            [Test]
            public void WordSearchMoreOrLess_Custom_ReturnsExpectedObject()
            {
                WordSearchMoreOrLess puzzleToSerialize = new WordSearchMoreOrLess();
                puzzleToSerialize.Size = 4;
                puzzleToSerialize.SetSolution("solution");
                string serializedPuzzle = JsonConvert.SerializeObject(puzzleToSerialize);
                Assert.LessOrEqual(serializedPuzzle.Length, 2000, "Expected less than 2 thousand characters.");

                WordSearchMoreOrLess deserializedPuzzle = JsonConvert.DeserializeObject<WordSearchMoreOrLess>(serializedPuzzle);
                Assert.AreEqual(puzzleToSerialize.Solution, deserializedPuzzle.Solution, "Unexpected difference in Solution");
                Assert.AreEqual(puzzleToSerialize.Description, deserializedPuzzle.Description, "Unexpected difference in Description");
                Assert.AreEqual(puzzleToSerialize.DictionaryOfClues.Count, deserializedPuzzle.DictionaryOfClues.Count, "Unexpected difference in DictionaryOfClues");
                Assert.AreEqual(puzzleToSerialize.Grid, deserializedPuzzle.Grid, "Unexpected difference in Grid");
                Assert.AreEqual(puzzleToSerialize.Size, deserializedPuzzle.Size, "Unexpected difference in Size");
                
                string originalHtml = puzzleToSerialize.FormatHtmlForGoogle(true, true);
                string deserializedHtml = deserializedPuzzle.FormatHtmlForGoogle(true, true);
                Assert.AreEqual(originalHtml, deserializedHtml, "Unexpected differences in generated HTML");

            }

            [Test]
            public void Anacrostic_Default_ReturnsExpectedObject()
            {
                Anacrostic puzzleToSerialize = new Anacrostic("default phrase");
                string serializedPuzzle = JsonConvert.SerializeObject(puzzleToSerialize);
                Assert.LessOrEqual(serializedPuzzle.Length, 1000, "Expected less than a thousand characters.");

                Anacrostic deserializedPuzzle = JsonConvert.DeserializeObject<Anacrostic>(serializedPuzzle);
                Assert.AreEqual(puzzleToSerialize.LineLength, deserializedPuzzle.LineLength, "Unexpected difference in Solution");
                Assert.AreEqual(puzzleToSerialize.Description, deserializedPuzzle.Description, "Unexpected difference in Description");
                Assert.AreEqual(puzzleToSerialize.EncodedPhrase, deserializedPuzzle.EncodedPhrase, "Unexpected difference in EncodedPhrase");
                Assert.AreEqual(puzzleToSerialize.OriginalPhrase, deserializedPuzzle.OriginalPhrase, "Unexpected difference in OriginalPhrase");

                Assert.AreEqual(puzzleToSerialize.Puzzle.PhraseAsString, deserializedPuzzle.Puzzle.PhraseAsString, "Unexpected difference in Puzzle.PhraseAsString");
                Assert.AreEqual(puzzleToSerialize.Puzzle.Clues, deserializedPuzzle.Puzzle.Clues, "Unexpected difference in Puzzle.Clues");
                Assert.AreEqual(puzzleToSerialize.Puzzle.Phrase, deserializedPuzzle.Puzzle.Phrase, "Unexpected difference in Puzzle.Phrase");


                string originalHtml = puzzleToSerialize.FormatHtmlForGoogle(true, true);
                string deserializedHtml = deserializedPuzzle.FormatHtmlForGoogle(true, true);
                Assert.AreEqual(originalHtml, deserializedHtml, "Unexpected differences in generated HTML");
            }

            [Test]
            public void ReadDownColumn_Default_ReturnsExpectedObject()
            {
                ReadDownColumnPuzzle puzzleToSerialize = new ReadDownColumnPuzzle();
                puzzleToSerialize.Solution = "test";
                string serializedPuzzle = JsonConvert.SerializeObject(puzzleToSerialize);
                Assert.LessOrEqual(serializedPuzzle.Length, 1000, "Expected less than a thousand characters.");

                ReadDownColumnPuzzle deserializedPuzzle = JsonConvert.DeserializeObject<ReadDownColumnPuzzle>(serializedPuzzle);
                Assert.AreEqual(puzzleToSerialize.Description, deserializedPuzzle.Description, "Unexpected difference in Description");

                string originalHtml = puzzleToSerialize.FormatHtmlForGoogle(true, true);
                string deserializedHtml = deserializedPuzzle.FormatHtmlForGoogle(true, true);
                Assert.AreEqual(originalHtml, deserializedHtml, "Unexpected differences in generated HTML");
            }

            [Test]
            public void ReadDownColumn_Custom_ReturnsExpectedObject()
            {
                ReadDownColumnPuzzle puzzleToSerialize = new ReadDownColumnPuzzle();
                puzzleToSerialize.Solution = "test";
                puzzleToSerialize.PopulateWords();
                puzzleToSerialize.SetClueAtIndex("clue 1", 0);
                puzzleToSerialize.SetClueAtIndex("clue 2", 1);
                puzzleToSerialize.SetClueAtIndex("clue 3", 2);
                puzzleToSerialize.SetClueAtIndex("clue 4", 3);

                string serializedPuzzle = JsonConvert.SerializeObject(puzzleToSerialize);
                Assert.LessOrEqual(serializedPuzzle.Length, 1000, "Expected less than a thousand characters.");

                ReadDownColumnPuzzle deserializedPuzzle = JsonConvert.DeserializeObject<ReadDownColumnPuzzle>(serializedPuzzle);
                Assert.AreEqual(puzzleToSerialize.Description, deserializedPuzzle.Description, "Unexpected difference in Description");

                string originalHtml = puzzleToSerialize.FormatHtmlForGoogle(true, true);
                string deserializedHtml = deserializedPuzzle.FormatHtmlForGoogle(true, true);
                Assert.AreEqual(originalHtml, deserializedHtml, "Unexpected differences in generated HTML");
            }

            [Test]
            public void PhraseSegmentPuzzle_Default_ReturnsExpectedObject()
            {
                PhraseSegmentPuzzle puzzleToSerialize = new PhraseSegmentPuzzle();

                string serializedPuzzle = JsonConvert.SerializeObject(puzzleToSerialize);
                Assert.LessOrEqual(serializedPuzzle.Length, 1000, "Expected less than a thousand characters.");

                PhraseSegmentPuzzle deserializedPuzzle = JsonConvert.DeserializeObject<PhraseSegmentPuzzle>(serializedPuzzle);
                Assert.AreEqual(puzzleToSerialize.Description, deserializedPuzzle.Description, "Unexpected difference in Description");

                string originalHtml = puzzleToSerialize.FormatHtmlForGoogle(true, true);
                string deserializedHtml = deserializedPuzzle.FormatHtmlForGoogle(true, true);
                Assert.AreEqual(originalHtml, deserializedHtml, "Unexpected differences in generated HTML");
            }

            [Test]
            public void PhraseSegmentPuzzle_Custom_ReturnsExpectedObject()
            {
                PhraseSegmentPuzzle puzzleToSerialize = new PhraseSegmentPuzzle();
                puzzleToSerialize.Phrase = "This phrase is the solution to this puzzle.";
                puzzleToSerialize.PlacePhrase();

                string serializedPuzzle = JsonConvert.SerializeObject(puzzleToSerialize);
                Assert.LessOrEqual(serializedPuzzle.Length, 1000, "Expected less than a thousand characters.");

                PhraseSegmentPuzzle deserializedPuzzle = JsonConvert.DeserializeObject<PhraseSegmentPuzzle>(serializedPuzzle);
                Assert.AreEqual(puzzleToSerialize.Description, deserializedPuzzle.Description, "Unexpected difference in Description");

                string originalHtml = puzzleToSerialize.FormatHtmlForGoogle(true, true);
                string deserializedHtml = deserializedPuzzle.FormatHtmlForGoogle(true, true);
                Assert.AreEqual(originalHtml, deserializedHtml, "Unexpected differences in generated HTML");
            }

            [Test]
            public void HiddenRelatedWordsPuzzle_Default_ReturnsExpectedObject()
            {
                HiddenRelatedWordsPuzzle puzzleToSerialize = new HiddenRelatedWordsPuzzle();

                string serializedPuzzle = JsonConvert.SerializeObject(puzzleToSerialize);
                Assert.LessOrEqual(serializedPuzzle.Length, 1000, "Expected less than a thousand characters.");

                HiddenRelatedWordsPuzzle deserializedPuzzle = JsonConvert.DeserializeObject<HiddenRelatedWordsPuzzle>(serializedPuzzle);
                Assert.AreEqual(puzzleToSerialize.Description, deserializedPuzzle.Description, "Unexpected difference in Description");

                string originalHtml = puzzleToSerialize.FormatHtmlForGoogle(true, true);
                string deserializedHtml = deserializedPuzzle.FormatHtmlForGoogle(true, true);
                Assert.AreEqual(originalHtml, deserializedHtml, "Unexpected differences in generated HTML");
            }


            [Test]
            public void HiddenRelatedWordsPuzzle_Custom_ReturnsExpectedObject()
            {
                HiddenRelatedWordsPuzzle puzzleToSerialize = new HiddenRelatedWordsPuzzle();
                puzzleToSerialize.Solution = "test";
                puzzleToSerialize.AddWord(new HiddenWord()
                {
                    KeyIndex = 0, 
                    SentenceHidingWord = "I did not estimate accurately.",
                    Word = "test"
                });

                string serializedPuzzle = JsonConvert.SerializeObject(puzzleToSerialize);
                Assert.LessOrEqual(serializedPuzzle.Length, 1000, "Expected less than a thousand characters.");

                HiddenRelatedWordsPuzzle deserializedPuzzle = JsonConvert.DeserializeObject<HiddenRelatedWordsPuzzle>(serializedPuzzle);
                Assert.AreEqual(puzzleToSerialize.Description, deserializedPuzzle.Description, "Unexpected difference in Description");

                Assert.AreEqual(puzzleToSerialize.CombinedKeyIndex, deserializedPuzzle.CombinedKeyIndex, "Unexpected difference in CombinedKeyIndex");
                Assert.AreEqual(puzzleToSerialize.CombinedLength, deserializedPuzzle.CombinedLength, "Unexpected difference in CombinedLength");


                string originalHtml = puzzleToSerialize.FormatHtmlForGoogle(true, true);
                string deserializedHtml = deserializedPuzzle.FormatHtmlForGoogle(true, true);
                Assert.AreEqual(originalHtml, deserializedHtml, "Unexpected differences in generated HTML");
            }

            [Test]
            public void LettersAndArrowsPuzzle_Default_ReturnsExpectedObject()
            {
                LettersAndArrowsPuzzle puzzleToSerialize = new LettersAndArrowsPuzzle(5);

                string serializedPuzzle = JsonConvert.SerializeObject(puzzleToSerialize);
                Assert.LessOrEqual(serializedPuzzle.Length, 2000, "Expected less than 2 thousand characters.");

                LettersAndArrowsPuzzle deserializedPuzzle = JsonConvert.DeserializeObject<LettersAndArrowsPuzzle>(serializedPuzzle);
                Assert.AreEqual(puzzleToSerialize.Description, deserializedPuzzle.Description, "Unexpected difference in Description");

                string originalHtml = puzzleToSerialize.FormatHtmlForGoogle(true, true);
                string deserializedHtml = deserializedPuzzle.FormatHtmlForGoogle(true, true);
                Assert.AreEqual(originalHtml, deserializedHtml, "Unexpected differences in generated HTML");
            }

            [Test]
            public void LettersAndArrowsPuzzle_Custom_ReturnsExpectedObject()
            {
                LettersAndArrowsPuzzle puzzleToSerialize = new LettersAndArrowsPuzzle(5);
                puzzleToSerialize.PlaceSolution("test");
                puzzleToSerialize.SetClueForRowIndex(0, "custom clue");

                string serializedPuzzle = JsonConvert.SerializeObject(puzzleToSerialize);
                Assert.LessOrEqual(serializedPuzzle.Length, 2000, "Expected less than 2 thousand characters.");

                LettersAndArrowsPuzzle deserializedPuzzle = JsonConvert.DeserializeObject<LettersAndArrowsPuzzle>(serializedPuzzle);
                Assert.AreEqual(puzzleToSerialize.Description, deserializedPuzzle.Description, "Unexpected difference in Description");

                string originalHtml = puzzleToSerialize.FormatHtmlForGoogle(true, true);
                string deserializedHtml = deserializedPuzzle.FormatHtmlForGoogle(true, true);
                Assert.AreEqual(originalHtml, deserializedHtml, "Unexpected differences in generated HTML");
            }


            [Test]
            public void TrisectedWordsPuzzle_Default_ReturnsExpectedObject()
            {
                TrisectedWordsPuzzle puzzleToSerialize = new TrisectedWordsPuzzle();

                string serializedPuzzle = JsonConvert.SerializeObject(puzzleToSerialize);
                Assert.LessOrEqual(serializedPuzzle.Length, 2000, "Expected less than 2 thousand characters.");

                TrisectedWordsPuzzle deserializedPuzzle = JsonConvert.DeserializeObject<TrisectedWordsPuzzle>(serializedPuzzle);
                Assert.AreEqual(puzzleToSerialize.Description, deserializedPuzzle.Description, "Unexpected difference in Description");

                string originalHtml = puzzleToSerialize.FormatHtmlForGoogle(true, true);
                string deserializedHtml = deserializedPuzzle.FormatHtmlForGoogle(true, true);
                Assert.AreEqual(originalHtml, deserializedHtml, "Unexpected differences in generated HTML");
            }
        }
    }
}