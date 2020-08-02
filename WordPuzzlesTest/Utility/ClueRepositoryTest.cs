using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using WordPuzzles.Utility;

namespace WordPuzzlesTest.Utility
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
                Assert.AreEqual(0, repository.CountOfWordWithClues, "Repository should start out empty");
                repository.AddClue("ONES", "Singletons");

                Assert.AreEqual(1, repository.CountOfWordWithClues, "Repository should have one clue now.");
            }

            [Test]
            public void SupportsMultipleClues()
            {
                ClueRepository repository = new ClueRepository();
                Assert.AreEqual(0, repository.CountOfWordWithClues, "Repository should start out empty");
                repository.AddClue("ONES", "Singletons");
                repository.AddClue("ONES", "Smallest denomination of folding money.");

                Assert.AreEqual(1, repository.CountOfWordWithClues, "Repository should have one clue now.");
            }

        }

        [TestFixture]
        public class GetCluesForWord
        {
            [Test]
            public void NoClues_ReturnsEmptyList()
            {
                ClueRepository repository = new ClueRepository();
                List<Clue> clues=  repository.GetCluesForWord("Clueless"); 
                Assert.AreEqual(0, clues.Count);
            }

            [Test]
            public void SupportsMultipleClues()
            {
                ClueRepository repository = new ClueRepository();
                Assert.AreEqual(0, repository.CountOfWordWithClues, "Repository should start out empty");
                repository.AddClue("ONES", "Singletons");
                repository.AddClue("ONES", "Smallest denomination of folding money.");
                List<Clue> clues = repository.GetCluesForWord("ONES");
                Assert.AreEqual(2, clues.Count);
            }

            [Test]
            public void IsCaseInsensitive()
            {
                ClueRepository repository = new ClueRepository();
                Assert.AreEqual(0, repository.CountOfWordWithClues, "Repository should start out empty");
                repository.AddClue("ones", "Singletons");
                repository.AddClue("OnEs", "Smallest denomination of folding money.");
                List<Clue> clues = repository.GetCluesForWord("Ones");
                Assert.AreEqual(2, clues.Count);
            }

            [Test]
            [TestCase("ones")]
            [TestCase("ions")]
            [TestCase("hope")]
            [TestCase("fawn")]
            [TestCase("badger")]
            [TestCase("stolen")]
            [TestCase("bronze")]
            [TestCase("models")]
            [TestCase("silent")]
            [TestCase("french")]
            [TestCase("sequin")]
            [TestCase("bounce")]
            [TestCase("theory")]
            [TestCase("poster")]
            [TestCase("anthem")]


            [TestCase("simple")]
            [TestCase("shaped")]
            [TestCase("magnet")]
            [TestCase("paired")]
            [TestCase("wicked")]
            [TestCase("faking")]
            [TestCase("weight")]
            [TestCase("eating")]
            [Ignore("Takes more than 3 seconds.")]

            public void AllWords_ContainsSpecificWords(string word)
            {
                const string ALL_WORDS_FILE =
                    @"C:\Users\Chip\Source\Repos\WordPuzzle3\WordPuzzlesTest\data\PUZ\allclues.json";
                ClueRepository clues = new ClueRepository();
                clues.ReadFromDisk(ALL_WORDS_FILE);
                var cluesForWord = clues.GetCluesForWord(word);
                foreach (var clueForWord in cluesForWord)
                {
                    Console.WriteLine($"{word.ToUpperInvariant()}: {clueForWord.ClueText}");
                }

                Assert.Less(0, clues.GetCluesForWord(word).Count,
                    $"Expected at least one clue for {word.ToUpperInvariant()}");

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

                repository.AddClue("Lego", "A small colorful brick.", ClueSource.CLUE_SOURCE_CHIP);

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
            public void MultipleClues_PopulatesClueDictionary()
            {
                ClueRepository repository = new ClueRepository();

                repository.ReadFromDisk(@"data\expected_nonempty_clue_repository.json");

                Assert.AreEqual(2, repository.CountOfWordWithClues);
                List<Clue> clues = repository.GetCluesForWord("ONES");
                Assert.AreEqual(2, clues.Count);
                List<Clue> cluesForLego = repository.GetCluesForWord("Lego");
                Assert.AreEqual(1, cluesForLego.Count);
                Assert.AreEqual("A small colorful brick.", cluesForLego[0].ClueText);
                Assert.AreEqual(ClueSource.CLUE_SOURCE_CHIP, cluesForLego[0].ClueSource);
            }
        }
    }
}