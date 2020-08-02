using System;
using NUnit.Framework;
using WordPuzzles.Puzzle;

namespace WordPuzzlesTest.Puzzle
{
    [TestFixture]
    public class PuzzleCollectionTest
    {
        [TestFixture]
        public class AddPuzzle
        {
            [Test]
            public void RetainsTypeOfAddedPuzzle()
            {
                PuzzleCollection collection = new PuzzleCollection();
                collection.AddPuzzle(new WordSquare());

                IPuzzle actualPuzzle = collection.RetrievePuzzleAtIndex(0);
                Assert.IsInstanceOf<WordSquare>(actualPuzzle, "Expected a Word Square as first puzzle");
            }
        }

        [TestFixture]
        public class RetrievePuzzleAtIndex
        {
            [Test]
            [Ignore("Takes more than 3 seconds.")]
            public void MultiplePuzzles_ReturnsCorrectPuzzle()
            {
                PuzzleCollection collection = new PuzzleCollection();
                collection.AddPuzzle(new WordSquare() {Theme = "First Theme"});
                collection.AddPuzzle(new WordSquare() { Theme = "Second Theme" });
                collection.AddPuzzle(new Anacrostic("solution phrase") );

                var firstPuzzle = collection.RetrievePuzzleAtIndex(0);
                var secondPuzzle = collection.RetrievePuzzleAtIndex(1);
                var thirdPuzzle = collection.RetrievePuzzleAtIndex(2);

                Assert.IsInstanceOf<WordSquare>(firstPuzzle, "Expected first puzzle to be a word square.");
                Assert.IsInstanceOf<WordSquare>(secondPuzzle, "Expected second puzzle to be a word square.");
                Assert.IsInstanceOf<Anacrostic>(thirdPuzzle, "Expected third puzzle to be an anacrostic.");
            }
        }

        [TestFixture]
        public class Serialize
        {
            [Test]
            public void Serialize_And_Deserialize_Are_Opposites()
            {
                PuzzleCollection collection = new PuzzleCollection();
                collection.AddPuzzle(new WordSquare());
                string fileName = $"SingleTest{DateTime.Now.Ticks}.json";

                collection.Serialize(fileName);

                PuzzleCollection deserializedCollection = new PuzzleCollection();
                deserializedCollection.Deserialize(fileName);

                Assert.AreEqual(1, deserializedCollection.PuzzleCount);
                Assert.IsInstanceOf<WordSquare>(deserializedCollection.RetrievePuzzleAtIndex(0));

            }

            [Test]
            [Ignore("Takes more than 3 seconds.")]
            public void MultiplePuzzles_ReturnsCorrectPuzzle()
            {
                string fileName = $"MultipleTest{DateTime.Now.Ticks}.json";
                PuzzleCollection collection = new PuzzleCollection();
                collection.AddPuzzle(new WordSquare() { Theme = "First Theme" });
                collection.AddPuzzle(new WordSquare() { Theme = "Second Theme" });
                collection.AddPuzzle(new Anacrostic("solution phrase"));

                collection.Serialize(fileName);

                PuzzleCollection deserializedCollection = new PuzzleCollection();

                deserializedCollection.Deserialize(fileName);
                var firstPuzzle = deserializedCollection.RetrievePuzzleAtIndex(0);
                var secondPuzzle = deserializedCollection.RetrievePuzzleAtIndex(1);
                var thirdPuzzle = deserializedCollection.RetrievePuzzleAtIndex(2);

                Assert.IsInstanceOf<WordSquare>(firstPuzzle, "Expected first puzzle to be a word square.");
                Assert.IsInstanceOf<WordSquare>(secondPuzzle, "Expected second puzzle to be a word square.");
                Assert.IsInstanceOf<Anacrostic>(thirdPuzzle, "Expected third puzzle to be an anacrostic.");
            }
        }
    }

    [TestFixture]
    public class ParseNameFromFileName
    {
        [Test]
        public void Example_ReturnsExpectedName()
        {
            PuzzleCollection collection = new PuzzleCollection();
            Assert.AreEqual("FirstCollection", collection.ParseNameFromFileName(@"C:\utilities\WordSquare\data\basic\collections\FirstCollection.json"));
        }
    }

    [TestFixture]
    public class RemovePuzzleAtIndex
    {
        [Test]
        public void RemovesPuzzle()
        {
            PuzzleCollection collection = new PuzzleCollection();
            collection.AddPuzzle(new WordSquare());
            Assert.AreEqual(1, collection.PuzzleCount, "Expected to have one puzzle before removing it.");

            collection.RemovePuzzleAtIndex(0);
            Assert.AreEqual(0, collection.PuzzleCount, "Expected to have no puzzles left.");
        }

        [Test]
        [Ignore("Takes more than 3 seconds.")]
        public void RemovesExpectedPuzzle()
        {
            PuzzleCollection collection = new PuzzleCollection();
            collection.AddPuzzle(new WordSquare());
            collection.AddPuzzle(new Anacrostic("phrase"));
            collection.AddPuzzle(new LettersAndArrowsPuzzle("phrase"));

            collection.RemovePuzzleAtIndex(1);

            Assert.AreEqual(2, collection.PuzzleCount, "There should be two puzzles left.");
            Assert.IsInstanceOf<WordSquare>(collection.RetrievePuzzleAtIndex(0));
            Assert.IsInstanceOf<LettersAndArrowsPuzzle>(collection.RetrievePuzzleAtIndex(1));

        }
    }
}