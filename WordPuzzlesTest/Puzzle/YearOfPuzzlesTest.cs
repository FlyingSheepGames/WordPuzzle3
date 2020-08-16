using System;
using NUnit.Framework;
using WordPuzzles;
using WordPuzzles.Puzzle;

namespace WordPuzzlesTest.Puzzle
{
    [TestFixture]
    public class YearOfPuzzlesTest
    {
        [TestFixture]
        public class Serialization
        {
            [Test]
            public void SinglePuzzle_SerializeAndDeserializeToFile()
            {
                YearOfPuzzles puzzles = new YearOfPuzzles();
                DateTime januaryFirst = new DateTime(2021, 1, 1);
                puzzles.Add(new WordSquare(), januaryFirst);

                string fileName = DateTime.Now.Ticks + ".json";
                puzzles.Serialize(fileName);

                YearOfPuzzles deserializedPuzzles = new YearOfPuzzles();
                deserializedPuzzles.Deserialize(fileName);

                Assert.AreEqual(1, deserializedPuzzles.Count);
                IPuzzle retrievedPuzzle = deserializedPuzzles.Retrieve(januaryFirst);
                Assert.IsInstanceOf<WordSquare>(retrievedPuzzle);

            }
        }

        [TestFixture]
        public class Add
        {
            [Test]
            public void AddingOnSameDate_ThrowsException()
            {
                YearOfPuzzles puzzles = new YearOfPuzzles();
                puzzles.Add(new WordSquare(), new DateTime(2021, 1, 1));
                var thrownException = Assert.Throws<ArgumentException>( ()=> puzzles.Add(new WordSquare(), new DateTime(2021, 1, 1) ));
                Assert.AreEqual("There's already a puzzle for that date. Delete it first.", thrownException.Message);
            }
        }
    }
}