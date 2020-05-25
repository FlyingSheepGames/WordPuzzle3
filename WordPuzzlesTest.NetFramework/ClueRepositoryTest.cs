using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using WordPuzzles;

namespace WordPuzzlesTest
{
    [TestFixture]
    public class ClueRepositoryTest
    {
        [TestFixture]
        public class AddClue
        {
            [Test]
            public void IncrementsClueCount()
            {
                ClueRepository repository = new ClueRepository();
                Assert.AreEqual(0, repository.ClueCount, "Repository should start out empty");
                repository.AddClue("ONES", "Singletons");

                Assert.AreEqual(1, repository.ClueCount, "Repository should have one clue now.");
            }

            [Test]
            public void SupportsMultipleClues()
            {
                ClueRepository repository = new ClueRepository();
                Assert.AreEqual(0, repository.ClueCount, "Repository should start out empty");
                repository.AddClue("ONES", "Singletons");
                repository.AddClue("ONES", "Smallest denomination of folding money.");

                Assert.AreEqual(1, repository.ClueCount, "Repository should have one clue now.");
            }

        }

        [TestFixture]
        public class GetCluesForWord
        {
            [Test]
            public void NoClues_ReturnsEmptyList()
            {
                ClueRepository repository = new ClueRepository();
                List<string> clues=  repository.GetCluesForWord("Clueless"); 
                Assert.AreEqual(0, clues.Count);
            }

            [Test]
            public void SupportsMultipleClues()
            {
                ClueRepository repository = new ClueRepository();
                Assert.AreEqual(0, repository.ClueCount, "Repository should start out empty");
                repository.AddClue("ONES", "Singletons");
                repository.AddClue("ONES", "Smallest denomination of folding money.");
                List<string> clues = repository.GetCluesForWord("ONES");
                Assert.AreEqual(2, clues.Count);
            }

            [Test]
            public void IsCaseInsensitive()
            {
                ClueRepository repository = new ClueRepository();
                Assert.AreEqual(0, repository.ClueCount, "Repository should start out empty");
                repository.AddClue("ones", "Singletons");
                repository.AddClue("OnEs", "Smallest denomination of folding money.");
                List<string> clues = repository.GetCluesForWord("Ones");
                Assert.AreEqual(2, clues.Count);
            }

        }

        [TestFixture]
        public class WriteToDisk
        {
            [Test]
            public void EmptyRepository_CreatesExpectedFile()
            {
                ClueRepository repository = new ClueRepository();
                repository.WriteToDisk(@"data\actual_empty_clue_repository.json");

                var expectedLines= File.ReadAllLines(@"data\expected_empty_clue_repository.json");
                var actualLines = File.ReadAllLines(@"data\actual_empty_clue_repository.json");
                for (var index = 0; index < expectedLines.Length; index++)
                {
                    string expectedLine = expectedLines[index];
                    string actualLine = actualLines[index];
                    Assert.AreEqual(expectedLine, actualLine, $"Unexpected line at index {index}");
                }
            }
            [Test]
            public void MultipleClues_CreatesExpectedFile()
            {
                ClueRepository repository = new ClueRepository();
                repository.AddClue("ONES", "Singletons");
                repository.AddClue("ONES", "Smallest denomination of folding money.");

                repository.AddClue("Lego", "A small colorful brick.");

                repository.WriteToDisk(@"data\actual_nonempty_clue_repository.json");

                var expectedLines = File.ReadAllLines(@"data\expected_nonempty_clue_repository.json");
                var actualLines = File.ReadAllLines(@"data\actual_nonempty_clue_repository.json");
                for (var index = 0; index < expectedLines.Length; index++)
                {
                    string expectedLine = expectedLines[index];
                    string actualLine = actualLines[index];
                    Assert.AreEqual(expectedLine, actualLine, $"Unexpected line at index {index}");
                }
            }
        }

        [TestFixture]
        public class ReadFromDisk
        {
            [Test]
            public void MultipleClues_PopulatesClueDictionry()
            {
                ClueRepository repository = new ClueRepository();

                repository.ReadFromDisk(@"data\expected_nonempty_clue_repository.json");

                Assert.AreEqual(2, repository.ClueCount);
                List<string> clues = repository.GetCluesForWord("ONES");
                Assert.AreEqual(2, clues.Count);
                List<string> cluesForLego = repository.GetCluesForWord("Lego");
                Assert.AreEqual(1, cluesForLego.Count);

            }
        }
    }
}