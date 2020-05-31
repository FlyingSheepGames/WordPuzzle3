using System.Collections.Generic;
using NUnit.Framework;
using WordPuzzles;

namespace WordPuzzlesTest
{
    [TestFixture]
    public class DataFromStackOverflowParserTest
    {
        [TestFixture]
        public class ReadCluesFromFile
        {
            [Test]
            public void Example_IncludesExpectedClues()
            {
                DataFromStackOverflowParser parser = new DataFromStackOverflowParser();
                Dictionary<string, List<NewClue>> retrievedClues =
                    parser.ReadCluesFromFile(@"data\dataFromSO\example.json");

                Assert.Less(0, retrievedClues.Count, "Unexpected number of clues.");
                List<NewClue> actualCluesForA = retrievedClues["A"];
                Assert.AreEqual("The 1st letter of the Roman alphabet", actualCluesForA[0].ClueText,
                    "Expected specific clue for A");

                List<NewClue> actualCluesForAbactinal = retrievedClues["ABACTINAL"];
                Assert.AreEqual("Opposite of actinal", actualCluesForAbactinal[1].ClueText,
                    "Expected specific clue for ABACTINAL");

                List<NewClue> actualCluesForAardvark = retrievedClues["AARDVARK"];
                Assert.AreEqual("Synonyms include Orycteropus afer, Ant bear, and Anteater", actualCluesForAardvark[1].ClueText, 
                    "Expected Synonym clue for Aardvark");

            }
        }
    }
}
