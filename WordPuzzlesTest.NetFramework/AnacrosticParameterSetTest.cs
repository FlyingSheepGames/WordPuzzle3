using System;
using System.Configuration;
using System.IO;
using NUnit.Framework;
using WordPuzzles;
using WordPuzzles.Puzzle;

namespace WordPuzzlesTest
{
    [TestFixture]
    public class AnacrosticParameterSetTest
    {

        [TestFixture]
        public class Serialize
        {
            [Test]
            public void NewObject_GeneratesExpectedFile()
            {
                AnacrosticParameterSet set = new AnacrosticParameterSet {TweetId = 0};
                set.Serialize();
                
                string EXPECTED_FILE_PATH = ConfigurationManager.AppSettings["BaseDirectory"] +  @"anacrostics\parameter_set_0.xml";
                FileAssert.Exists(EXPECTED_FILE_PATH);
                var readAllText = File.ReadAllText(EXPECTED_FILE_PATH);
                Console.WriteLine(readAllText);
                Assert.AreEqual(@"<?xml version=""1.0""?>
<AnacrosticParameterSet xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
  <WordsToUse />
  <WordsToIgnore />
  <TweetId>0</TweetId>
</AnacrosticParameterSet>", readAllText);
            }
        }

        [TestFixture]
        public class Deserialize
        {
            [Test]
            public void SetsExpectedFields()
            {
                AnacrosticParameterSet setWithFields = new AnacrosticParameterSet {TweetId = 1};
                setWithFields.WordsToUse.Add("wordToUse");
                setWithFields.WordsToIgnore.Add("wordToIgnore");

                setWithFields.Serialize();
                setWithFields.WordsToUse.Clear();
                setWithFields.WordsToIgnore.Clear();
                //act   
                setWithFields.Deserialize();

                Assert.Contains("wordToUse", setWithFields.WordsToUse);
                Assert.Contains("wordToIgnore", setWithFields.WordsToIgnore);
            }
        }

    }
}