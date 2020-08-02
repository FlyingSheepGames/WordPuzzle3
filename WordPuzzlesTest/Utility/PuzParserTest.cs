using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using NUnit.Framework;
using WordPuzzles.Utility;

namespace WordPuzzlesTest.Utility
{
    [TestFixture]
    public class PuzParserTest
    {
        [TestFixture]
        public class ParseFile
        {
            [Test]
            public void ExampleFile_ReturnsExpectedCollection()
            {
                PuzParser parser = new PuzParser();

                ClueRepository results = parser.ParseFile(@"data\uc200525.puz");
                Assert.IsNotNull(results);
                Assert.AreEqual(75, results.CountOfWordWithClues);
            }

            [Test]
            [TestCase(2, "RIND", "Orange coat?")]
            [TestCase(3, "APART", "Not together")]
            [TestCase(4, "WOMB", "Sonogram target")]
            [TestCase(5, "CANOE", "Lake rental")]
            [TestCase(6, "COGITATE", "THINK")]
            public void OtherExampleFile_ReturnsExpectedCollection(int fileNumber, string expectedWord,
                string expectedClue)
            {
                PuzParser parser = new PuzParser();

                ClueRepository results = parser.ParseFile($@"data\example{fileNumber}.puz");
                Assert.IsNotNull(results);
                Assert.LessOrEqual(74, results.CountOfWordWithClues);
                Assert.AreEqual(expectedClue, results.GetCluesForWord(expectedWord)[0].ClueText,
                    $"Expected clue for {expectedWord}");

            }

        }

        [TestFixture]
        public class ParseWordsFromGridString
        {
            [Test]
            public void SixBySix_ReturnsExpectedClues()
            {
                string grid = "" +
                              "POEMS." +
                              "ONTAP." +
                              "MECCA." +
                              ".SHAR." +
                              "......" +
                              "......";
                PuzParser parser = new PuzParser();
                var results = parser.ParseWordsFromGridString(grid);

                Assert.AreEqual(9, results.Count, "Expected 10 crossword entries");

                Assert.AreEqual("POEMS", results[0].Word);
                Assert.AreEqual(true, results[0].IsCellNumbered, $"Unexpected value for IsCellNumbered for word POEMS");
                Assert.AreEqual(0, results[0].IndexInSingleString,
                    "Unexpected value for IndexInSingleString for word POEMS");
                Assert.AreEqual(CrosswordDirection.ACROSS, results[0].Direction,
                    $"Unexpected value for Direction for word POEMS");
                Assert.AreEqual(1, results[0].ClueNumber, $"Unexpected value for ClueNumber for word POEMS");

                var entryPom = results[1];
                Assert.AreEqual("POM", entryPom.Word);
                Assert.AreEqual(true, entryPom.IsCellNumbered, $"Unexpected value for IsCellNumbered for word POM");
                Assert.AreEqual(0, entryPom.IndexInSingleString,
                    "Unexpected value for IndexInSingleString for word POM");
                Assert.AreEqual(CrosswordDirection.DOWN, entryPom.Direction,
                    $"Unexpected value for Direction for word POM");

                var entryOnes = results[2];
                Assert.AreEqual("ONES", entryOnes.Word);
                Assert.AreEqual(true, entryOnes.IsCellNumbered, $"Unexpected value for IsCellNumbered for word ONES");
                Assert.AreEqual(1, entryOnes.IndexInSingleString,
                    "Unexpected value for IndexInSingleString for word ONES");
                Assert.AreEqual(CrosswordDirection.DOWN, entryOnes.Direction,
                    $"Unexpected value for Direction for word ONES");


                var entryEtch = results[3];
                Assert.AreEqual("ETCH", entryEtch.Word);
                Assert.AreEqual(true, entryEtch.IsCellNumbered, $"Unexpected value for IsCellNumbered for word ETCH");
                Assert.AreEqual(2, entryEtch.IndexInSingleString,
                    "Unexpected value for IndexInSingleString for word ETCH");
                Assert.AreEqual(CrosswordDirection.DOWN, entryEtch.Direction,
                    $"Unexpected value for Direction for word ETCH");

                var entryMaca = results[4];
                Assert.AreEqual("MACA", entryMaca.Word);
                Assert.AreEqual(true, entryMaca.IsCellNumbered, $"Unexpected value for IsCellNumbered for word MACA");
                Assert.AreEqual(3, entryMaca.IndexInSingleString,
                    "Unexpected value for IndexInSingleString for word MACA");
                Assert.AreEqual(CrosswordDirection.DOWN, entryMaca.Direction,
                    $"Unexpected value for Direction for word MACA");

                var entrySpar = results[5];
                Assert.AreEqual("SPAR", entrySpar.Word);
                Assert.AreEqual(true, entrySpar.IsCellNumbered, $"Unexpected value for IsCellNumbered for word SPAR");
                Assert.AreEqual(4, entrySpar.IndexInSingleString,
                    "Unexpected value for IndexInSingleString for word SPAR");
                Assert.AreEqual(CrosswordDirection.DOWN, entrySpar.Direction,
                    $"Unexpected value for Direction for word SPAR");

                var entryOnTap = results[6];
                Assert.AreEqual("ONTAP", entryOnTap.Word);
                Assert.AreEqual(true, entryOnTap.IsCellNumbered, $"Unexpected value for IsCellNumbered for word ONTAP");
                Assert.AreEqual(6, entryOnTap.IndexInSingleString,
                    "Unexpected value for IndexInSingleString for word ONTAP");
                Assert.AreEqual(CrosswordDirection.ACROSS, entryOnTap.Direction,
                    $"Unexpected value for Direction for word POEMS");

                var entryMecca = results[7];
                Assert.AreEqual("MECCA", entryMecca.Word);
                Assert.AreEqual(true, entryMecca.IsCellNumbered, $"Unexpected value for IsCellNumbered for word MECCA");
                Assert.AreEqual(12, entryMecca.IndexInSingleString,
                    "Unexpected value for IndexInSingleString for word MECCA");
                Assert.AreEqual(CrosswordDirection.ACROSS, entryMecca.Direction,
                    $"Unexpected value for Direction for word MECCA");

                var entryShar = results[8];
                Assert.AreEqual("SHAR", entryShar.Word);
                Assert.AreEqual(true, entryShar.IsCellNumbered, $"Unexpected value for IsCellNumbered for word SHAR");
                Assert.AreEqual(19, entryShar.IndexInSingleString,
                    "Unexpected value for IndexInSingleString for word SHAR");
                Assert.AreEqual(CrosswordDirection.ACROSS, entryShar.Direction,
                    $"Unexpected value for Direction for word SHAR");
            }

            [Test]
            public void ExampleFromFile_ReturnsExpectedClues()
            {
                PuzParser parser = new PuzParser();
                var results = parser.ParseWordsFromGridString(
                    "POEMS.RANG.DATA" +
                    "ONTAP.AVIA.EPIC" +
                    "MECCA.RAPS.BOTH" +
                    ".SHARPEISHARPIE" +
                    "...ORACLE.BALL." +
                    "APB.ECO.YSL.ELI" +
                    "BRANDEIS.PENCAP" +
                    "BOTS.CNOTE.ETTA" +
                    "OTTAWA.BRANDIES" +
                    "TEL.ARM.EKE.CDS" +
                    ".MEAD.ALEUTS..." +
                    "APPLEIDAPPLIED." +
                    "NOLA.NEMO.ORDER" +
                    "TRAM.CUPS.SENSE" +
                    "ZENO.APSE.SNAKY");
                Assert.AreEqual(74, results.Count, "Expected 34 results");
                bool foundPOEMS = false; //first across
                bool foundSNAKY = false; //last across

                bool foundPOM = false; //first down
                bool foundREY = false; //last down

                foreach (var result in results)
                {
                    if (result.Word == "POEMS")
                    {
                        foundPOEMS = true;
                        Assert.AreEqual(0, result.IndexInSingleString, "Unexpected IndexInSingleString for POEMS");
                        Assert.AreEqual(1, result.ClueNumber, "Unexpected IndexInSingleString for POEMS");
                        Assert.AreEqual(true, result.IsCellNumbered, "Unexpected IndexInSingleString for POEMS");
                        Assert.AreEqual(CrosswordDirection.ACROSS, result.Direction,
                            "Unexpected IndexInSingleString for POEMS");
                    }

                    if (result.Word == "SNAKY") foundSNAKY = true;

                    if (result.Word == "POM") foundPOM = true;
                    if (result.Word == "REY") foundREY = true;
                }

                Assert.AreEqual(true, foundPOEMS, "Found POEMS");
                Assert.AreEqual(true, foundSNAKY, "Found SNAKY");

                Assert.AreEqual(true, foundPOM, "Found POM");
                Assert.AreEqual(true, foundREY, "Found REY");

            }
        }

        [TestFixture]
        public class DetermineIfCellIsNumbered
        {

            [Test]
            public void TopRow_ReturnsTrue()
            {
                PuzParser parser = new PuzParser();
                string[] letterGridAsStrings =
                {
                    "POEMS.",
                    "ONTAP.",
                    "MECCA.",
                    ".SHAR.",
                    "......",
                    "......",
                };
                for (int x = 0; x < 6; x++)
                {
                    Assert.IsTrue(parser.DetermineIfCellIsNumbered(x, 0, letterGridAsStrings));
                }
            }
        }

        [TestFixture]
        public class NumberWordsFromGrid
        {
            [Test]
            public void SortThreeWords()
            {
                PuzParser parser = new PuzParser();
                var wordsToSort = new List<CrosswordPuzzleEntry>();
                wordsToSort.Add(new CrosswordPuzzleEntry()
                {
                    IndexInSingleString = 1,
                    Direction = CrosswordDirection.ACROSS,
                    Word = "third"
                });
                wordsToSort.Add(new CrosswordPuzzleEntry()
                {
                    IndexInSingleString = 0,
                    Direction = CrosswordDirection.DOWN,
                    Word = "second"
                });
                wordsToSort.Add(new CrosswordPuzzleEntry()
                {
                    IndexInSingleString = 0,
                    Direction = CrosswordDirection.ACROSS,
                    Word = "first"
                });

                var actualSortedWords = parser.NumberWordsFromGrid(wordsToSort);
                Assert.AreEqual("first", actualSortedWords[0].Word);
                Assert.AreEqual(1, actualSortedWords[0].ClueNumber);
                Assert.AreEqual("second", actualSortedWords[1].Word);
                Assert.AreEqual(1, actualSortedWords[1].ClueNumber);
                Assert.AreEqual("third", actualSortedWords[2].Word);
                Assert.AreEqual(2, actualSortedWords[2].ClueNumber);
            }
        }

        [Test]
        [Timeout(600000)]
        [Ignore("Only run manually to get more puzzles.")]
        public void DownloadPuzFiles_NoAssertions()
        {
            int year = 18; // up to 20
            for (int month = 1; month < 13; month++)
            {
                for (int day = 1; day < 32; day++)
                {
                    var url = $"http://herbach.dnsalias.com/uc/uc{year:00}{month:00}{day:00}.puz";
                    try
                    {
                        string content = WebRequestUtility.ReadHtmlPageFromUrl(url);
                        Console.WriteLine($"{content.Length} : {url}");
                    }
                    catch (WebException exception)
                    {
                        Console.WriteLine($"Exception: {url} {exception.Message}");
                    }
                }
            }
        }

        [Test]
        [Timeout(600000)]
        [Ignore("Run manually")]
        public void ProcessAllPuzzlesInDirectory_NoAssertions()
        {
            PuzParser parser = new PuzParser();
            const string DIRECTORY_WITH_PUZ_FILES =
                @"C:\Users\Chip\Source\Repos\WordPuzzle3\WordPuzzlesTest\data\PUZ";
            foreach (string file in Directory.EnumerateFiles(DIRECTORY_WITH_PUZ_FILES, "*.puz"))
            {
                Console.WriteLine($"Attempting {file}");
                var results = parser.ParseFile(file);
                Console.WriteLine($"{results.CountOfWordWithClues}: {file}");
                var fileWithClues = file.Replace(".puz", ".json");
                results.WriteToDisk(fileWithClues);
                Assert.LessOrEqual(70, results.CountOfWordWithClues, $"Expected at least 70 clues.");
            }
        }

        [Test]
        [Ignore("Run Manually")]
        public void CollectAllTheClues_NoAssertions()
        {
            const string DIRECTORY_WITH_PUZ_FILES =
                @"C:\Users\Chip\Source\Repos\WordPuzzle3\WordPuzzlesTest\data\PUZ";

            PuzParser parser = new PuzParser();
            ClueRepository clues = new ClueRepository();
            clues.ReadFromDisk(DIRECTORY_WITH_PUZ_FILES + @"\allclues.json");
            foreach (string file in Directory.EnumerateFiles(DIRECTORY_WITH_PUZ_FILES, "*.json"))
            {
                Console.WriteLine($"Adding {file}, words so far {clues.CountOfWordWithClues}.");
                clues.ReadFromDisk(file);
            }
            clues.WriteToDisk(DIRECTORY_WITH_PUZ_FILES + @"\allclues.json");
        }

        [Test]
        [Timeout(6000000)]
        [Ignore("Run Manually")]
        public void ImportAllStackOverflowFiles_NoAssertions()
        {
            const string DIRECTORY_WITH_PUZ_FILES =
                @"C:\Users\Chip\Source\Repos\WordPuzzle3\WordPuzzlesTest\data\PUZ";
            const string DIRECTORY_WITH_SO_FILES =
                @"C:\Users\Chip\Source\Repos\WordPuzzle3\WordPuzzlesTest\data\dataFromSO";

            PuzParser parser = new PuzParser();
            ClueRepository clues = new ClueRepository();
            clues.ReadFromDisk(DIRECTORY_WITH_PUZ_FILES + @"\allclues.json");
            foreach (string file in Directory.EnumerateFiles(DIRECTORY_WITH_SO_FILES, "*.json"))
            {
                Console.WriteLine($"Adding {file}, words so far {clues.CountOfWordWithClues}.");
                clues.ImportStackOverflowFormatFile(file);
            }
            clues.WriteToDisk(DIRECTORY_WITH_PUZ_FILES + @"\allclues.json");
        }

    }
}