using System;
using NUnit.Framework;
using WordPuzzles;

namespace WordPuzzlesTest
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
}