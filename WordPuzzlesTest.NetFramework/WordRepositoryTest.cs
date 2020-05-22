using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using NUnit.Framework;
using WordPuzzles;

namespace WordPuzzlesTest
{
    [TestFixture]
    public class WordRepositoryTest
    {

        [TestFixture]
        public class WordsStartingWith
        {
            [Test]
            public void TA_ReturnsExpectedList()
            {
                WordRepository repository  = new WordRepository();
                List<string> words = repository.WordsStartingWith("ta");
                for (int i = 0; i < 10; i++)
                {
                    Console.WriteLine(words[i]);
                }
                Assert.LessOrEqual(50, words.Count);
            }

            [Test]
            public void TA_SixLetters_ReturnsExpectedList()
            {
                WordRepository repository = new WordRepository();
                List<string> words = repository.WordsStartingWith("ta", 6);
                for (int i = 0; i < 10; i++)
                {
                    Console.WriteLine(words[i]);
                }
                Assert.AreEqual(11, words.Count);
            }

            [Test]
            public void TA_FourLetters_ReturnsExpectedList()
            {
                WordRepository repository = new WordRepository();
                List<string> words = repository.WordsStartingWith("ta", 4);
                for (int i = 0; i < 10; i++)
                {
                    Console.WriteLine(words[i]);
                }
                Assert.AreEqual(20, words.Count);
            }

            [Test]
            public void ON_ThreeLetters_ReturnsONE()
            {
                WordRepository repository = new WordRepository();
                List<string> words = repository.WordsStartingWith("on", 3);
                bool foundOne = false;
                foreach (var word in words)
                {
                    if (word == "one")
                    {
                        foundOne = true;
                    }
                }
                Assert.IsTrue(foundOne, "expected to fine 'one' as a word.");
            }
        }

        [TestFixture]
        public class GetRandomWord
        {
            [Test]
            public void ReturnsWordOfExpectedLength()
            {
                WordRepository repository = new WordRepository();
                string randomWord = repository.GetRandomWord();
                Debug.WriteLine(randomWord);
                Assert.AreEqual(5, randomWord.Length, $"Unexpected Random word '{randomWord}'");
            }

            [Test]
            public void WordLengthSix_ReturnsWordOfExpectedLength()
            {
                WordRepository repository = new WordRepository();
                string randomWord = repository.GetRandomWord(6);
                Debug.WriteLine(randomWord);
                Assert.AreEqual(6, randomWord.Length, $"Unexpected Random word '{randomWord}'");
            }


            [Test]
            public void WordLengthFour_ReturnsWordOfExpectedLength()
            {
                WordRepository repository = new WordRepository();
                string randomWord = repository.GetRandomWord(4);
                Debug.WriteLine(randomWord);
                Assert.AreEqual(4, randomWord.Length, $"Unexpected Random word '{randomWord}'");
            }
        }

        [TestFixture]
        public class WriteToDisk
        {
            [Test]
            public void EmptyExample_WritesFile()
            {
                const string FILENAME_FOR_THIS_TEST = "testFile_emptyExample.xml";
                WordRepository.WriteToDisk(new List<Clue>(), FILENAME_FOR_THIS_TEST);
                Assert.AreEqual(@"<?xml version=""1.0"" encoding=""us-ascii""?>
<ArrayOfClue xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" />", File.ReadAllText(FILENAME_FOR_THIS_TEST));
            }

            [Test]
            public void SingleExample_WritesFile()
            {
                const string FILENAME_FOR_THIS_TEST = "testFile_singleExample.xml";
                var clues = new List<Clue>() {new Clue() {Hint = "Hint", Word = "word"}};

                WordRepository.WriteToDisk(clues, FILENAME_FOR_THIS_TEST);
                Assert.AreEqual(@"<?xml version=""1.0"" encoding=""us-ascii""?>
<ArrayOfClue xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
  <Clue>
    <Word>word</Word>
    <Hint>Hint</Hint>
  </Clue>
</ArrayOfClue>", File.ReadAllText(FILENAME_FOR_THIS_TEST));
            }
        }

        [TestFixture]
        public class GetRelatedWordsForTheme
        {
            [Test]
            public void Alcohol_ReturnsExpectedList()
            {
                WordRepository repository = new WordRepository() {IgnoreCache = false};
                List<string> relatedWords = repository.GetRelatedWordsForTheme("#AlcoholWeek");
                Assert.LessOrEqual(27, relatedWords.Count, "Should be at least 27 items.");
            }

            [Test]
            public void MissingWord_DoesNotThrow()
            {
                WordRepository wordRepository = new WordRepository() ;
                wordRepository.GetRelatedWordsForTheme("#VegetableWeek");
            }

        }

        [TestFixture]
        public class IsSingleSyllable
        {
            [Test]
            [TestCase("beer")]
            [TestCase("beers")]
            [TestCase("gin")]
            [TestCase("wine")]
            [TestCase("ale")]
            [TestCase("stout")]

            [TestCase("mead")]
            [TestCase("sake")]
            [TestCase("port")]
            [TestCase("rum")]
            public void Examples_ReturnTrue(string singleSyllableWord)
            {
                WordRepository repository = new WordRepository();
                Assert.IsTrue(repository.IsSingleSyllable(singleSyllableWord), $"{singleSyllableWord} is a single syllable word.");
            }

            [Test]
            [TestCase("beer", "b", "r")]
            [TestCase("beers", "b", "rs")]
            [TestCase("gin", "g", "n")]
            [TestCase("wine", "w", "n")]
            [TestCase("ale", null, "l")]
            [TestCase("stout", "st", "t")]

            [TestCase("mead", "m", "d")]
            [TestCase("sake", "s", "k")]
            [TestCase("port", "p", "rt")]
            [TestCase("rum", "r", "m")]
            [TestCase("lee", "l", null)]
            public void Examples_ReturnExpectedStartAndEndLetters(string singleSyllableWord,
                string expectedStartConsonant, string expectedEndConsonant)
            {
                WordRepository repository = new WordRepository();
                string actualStartConsonant;
                string actualEndConsonant; 
                Assert.IsTrue(repository.IsSingleSyllable(singleSyllableWord, out actualStartConsonant, out actualEndConsonant), $"{singleSyllableWord} is a single syllable word.");
                Assert.AreEqual(expectedStartConsonant, actualStartConsonant, "Unexpected end consonant");
                Assert.AreEqual(expectedEndConsonant, actualEndConsonant, "Unexpected end consonant" );

            }

            [Test]
            [TestCase("cider")]
            [TestCase("liquor")]
            [TestCase("tequila")]
            [TestCase("vodka")]
            [TestCase("whiskey")]
            [TestCase("whisky")]
            [TestCase("cocktail")]
            [TestCase("porter")]
            [TestCase("lager")]

            [TestCase("pilsener")]
            [TestCase("sherry")]
            [TestCase("vermouth")]
            [TestCase("sangria")]
            [TestCase("champagne")]
            [TestCase("absinthe")]
            [TestCase("brandy")]
            [TestCase("cognac")]
            [TestCase("Inlet")]
            [TestCase("envy")]
            [TestCase("cookie")]
            public void Counterexamples_ReturnFalse(string multipleSyllableWord)
            {
                WordRepository repository = new WordRepository();
                Assert.IsFalse(repository.IsSingleSyllable(multipleSyllableWord), $"{multipleSyllableWord} is a multiple syllable word.");
            }

        }

        [TestFixture]
        public class RemoveWordFromFile
        {
            [Test]
            [Repeat(3)]
            public void RemovesExpectedWord()
            {
                //arrange
                string fileNameForThisTest = $@"c:\temp\testFile_{Thread.CurrentThread.ManagedThreadId}.txt";
                using (var stream = File.CreateText(fileNameForThisTest))
                {
                    stream.WriteLine("first");
                    stream.WriteLine("second");
                    stream.WriteLine("third");

                    stream.Flush();
                }

                var initialText = File.ReadAllText(fileNameForThisTest);
                Assert.AreEqual(@"first
second
third
", initialText);

                //act
                WordRepository repository = new WordRepository();
                repository.RemoveWordFromFile("second", fileNameForThisTest);

                //assert
                var modifiedText = File.ReadAllText(fileNameForThisTest);
                Assert.AreEqual(@"first
third
", modifiedText);

            }
        }

        [TestFixture]
        public class LoadAllWords
        {
            [Test]
            public void PopulatesExpectedLists()
            {
                WordRepository wordRepository = new WordRepository();
                wordRepository.LoadAllWords();
                Assert.IsTrue(wordRepository.IsAWord("zooms")); 
            }

            [Test]
            public void PopulatesClues()
            {
                WordRepository wordRepository = new WordRepository();
                wordRepository.LoadAllWords();
                Assert.AreEqual("Rising agent", wordRepository.FindClueFor("yeast")); 
            }

        }

        [TestFixture]
        public class WordsWithCharacterAtIndex
        {
            [Test]
            public void J_AtIndex3()
            {
                WordRepository repository = new WordRepository() {IgnoreCache = false};
                foreach (string word in repository.WordsWithCharacterAtIndex('j', 3, 5))
                {
                    Console.WriteLine(word);
                }
            }
        }

        [TestFixture]
        public class WordsMatchingPattern
        {
            [Test]
            public void CenterG_IncludesNight()
            {
                WordRepository repository = new WordRepository() {IgnoreCache = false};
                bool containsNight = false;
                foreach (string result in repository.WordsMatchingPattern("__g__"))
                {
                    Console.WriteLine(result);
                    if (result == "night")
                    {
                        containsNight = true;
                    }
                }

                Assert.IsTrue(containsNight, "Expected 'night' to be returned.");
            }
        }


        [TestFixture]
        public class IsAWord
        {
            [Test]
            public void NET_ReturnsTrue()
            {
                WordRepository repository = new WordRepository();
                Assert.IsTrue(repository.IsAWord("net"));
            }

            [Test]
            public void OnlySingleLetterWordsAreAandI()
            {
                WordRepository repository = new WordRepository();
                Assert.IsTrue(repository.IsAWord("a"));
                for (char letter = 'b'; letter <= 'z'; letter++)
                {
                    if (letter == 'i')
                    {
                        Assert.IsTrue(repository.IsAWord(letter.ToString()));
                    }
                    else
                    {
                        Assert.IsFalse(repository.IsAWord(letter.ToString()));
                    }
                }
            }

            [Test]
            public void TO_IsAWord()
            {
                WordRepository repository = new WordRepository();
                Assert.IsTrue(repository.IsAWord("to"));
            }

            [Test]
            public void OT_IsNotAWord()
            {
                WordRepository repository = new WordRepository();
                Assert.IsFalse(repository.IsAWord("ot"));
            }

            [Test]
            public void SISTER_IsAWord()
            {
                WordRepository repository = new WordRepository() {ExludeAdvancedWords = false};
                Assert.IsTrue(repository.IsAWord("sister"));
            }

            [Test]
            public void ZESTFUL_IsAWord()
            {
                WordRepository repository = new WordRepository() {ExludeAdvancedWords = true};
                Assert.IsTrue(repository.IsAWord("zestful"));
            }

            [Test]
            public void OUTBREAK_IsAWord()
            {
                WordRepository repository = new WordRepository() { ExludeAdvancedWords = true, IgnoreCache = true};
                Assert.IsTrue(repository.IsAWord("outbreak"));
            }

            [Test]
            public void TRAMPOLINE_IsAWord()
            {
                WordRepository repository = new WordRepository() { ExludeAdvancedWords = true };
                Assert.IsTrue(repository.IsAWord("trampoline"));
            }

        }

        [TestFixture]
        public class CategorizeWord
        {
            [Test]
            public void CAT_IsABasicWord()
            {
                WordRepository repository = new WordRepository();
                Assert.AreEqual(WordCategory.BasicWord, repository.CategorizeWord("cat"));
            }

            [Test]
            public void ABASE_IsAnAdvancedWord()
            {
                WordRepository repository = new WordRepository();
                Assert.AreEqual(WordCategory.AdvancedWord, repository.CategorizeWord("abase"));

            }

            [Test]
            public void ZZZZ_IsNotAWord()
            {
                WordRepository repository = new WordRepository();
                Assert.AreEqual(WordCategory.NotAWord, repository.CategorizeWord("zzzz"));

            }

            [Test]
            public void AMINE_IsAnAdvancedWord()
            {
                WordRepository repository = new WordRepository();
                Assert.AreEqual(WordCategory.AdvancedWord, repository.CategorizeWord("amine"));

            }

        }

        [TestFixture]
        public class CategoriesToInclude
        {
            [Test]
            public void OnlyBasic_Excludes_ASP()
            {
                WordRepository repository = new WordRepository()
                {
                    ExludeAdvancedWords = true
                };
                Assert.IsFalse(repository.IsAWord("asp"));

            }
        }

        [TestFixture]
        public class FindClueFor
        {
            [Test]
            public void LoadsWordsFirst()
            {
                WordRepository repository = new  WordRepository();
                Assert.IsFalse(repository._alreadyLoaded);
                repository.FindClueFor("ask");
                Assert.IsTrue(repository._alreadyLoaded);
            }
        }

        [TestFixture]
        public class FindThemesForWord
        {
            [Test]
            public void BLUE_ReturnsExpectedResults()
            {
                WordRepository repository = new WordRepository();
                List<string> results = repository.FindThemesForWord("blue");
                Console.WriteLine(string.Join(Environment.NewLine, results));
                Assert.AreEqual(2, results.Count);
            }
        }

    }
}


