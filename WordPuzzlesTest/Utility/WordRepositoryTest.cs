using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using NUnit.Framework;
using WordPuzzles.Utility;

namespace WordPuzzlesTest.Utility
{
    [TestFixture]
    public class WordRepositoryTest
    {

        [TestFixture]
        public class WordsStartingWith
        {
            [Test]
            [Ignore("Takes more than 3 seconds.")]
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
            [Ignore("Takes more than 3 seconds.")]
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
            [Ignore("Takes more than 3 seconds.")]
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
            [Ignore("Takes more than 3 seconds.")]
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
            [Ignore("Takes more than 3 seconds.")]
            public void ReturnsWordOfExpectedLength()
            {
                WordRepository repository = new WordRepository();
                string randomWord = repository.GetRandomWord();
                Debug.WriteLine(randomWord);
                Assert.AreEqual(5, randomWord.Length, $"Unexpected Random word '{randomWord}'");
            }

            [Test]
            [Ignore("Takes more than 3 seconds.")]
            public void WordLengthSix_ReturnsWordOfExpectedLength()
            {
                WordRepository repository = new WordRepository();
                string randomWord = repository.GetRandomWord(6);
                Debug.WriteLine(randomWord);
                Assert.AreEqual(6, randomWord.Length, $"Unexpected Random word '{randomWord}'");
            }


            [Test]
            [Ignore("Takes more than 3 seconds.")]
            public void WordLengthFour_ReturnsWordOfExpectedLength()
            {
                WordRepository repository = new WordRepository();
                string randomWord = repository.GetRandomWord(4);
                Debug.WriteLine(randomWord);
                Assert.AreEqual(4, randomWord.Length, $"Unexpected Random word '{randomWord}'");
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
            [Ignore("Takes more than 3 seconds.")]
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
            [Ignore("Takes more than 3 seconds.")]
            public void PopulatesExpectedLists()
            {
                WordRepository wordRepository = new WordRepository();
                wordRepository.LoadAllWords();
                Assert.IsTrue(wordRepository.IsAWord("zooms")); 
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
            [Ignore("Takes more than 3 seconds.")]
            public void NET_ReturnsTrue()
            {
                WordRepository repository = new WordRepository();
                Assert.IsTrue(repository.IsAWord("net"));
            }

            [Test]
            [Ignore("Takes more than 3 seconds.")]
            public void OnlySingleLetterWordsAreAAndI()
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
            [Ignore("Takes more than 3 seconds.")]
            public void TO_IsAWord()
            {
                WordRepository repository = new WordRepository();
                Assert.IsTrue(repository.IsAWord("to"));
            }

            [Test]
            [Ignore("Takes more than 3 seconds.")]
            public void OT_IsNotAWord()
            {
                WordRepository repository = new WordRepository();
                Assert.IsFalse(repository.IsAWord("ot"));
            }

            [Test]
            [Ignore("Takes more than 3 seconds.")]
            public void SISTER_IsAWord()
            {
                WordRepository repository = new WordRepository() {ExcludeAdvancedWords = false};
                Assert.IsTrue(repository.IsAWord("sister"));
            }

            [Test]
            [Ignore("Takes more than 3 seconds.")]
            public void ZESTFUL_IsAWord()
            {
                WordRepository repository = new WordRepository() {ExcludeAdvancedWords = true};
                Assert.IsTrue(repository.IsAWord("zestful"));
            }

            [Test]
            [Ignore("Takes more than 3 seconds.")]
            public void OUTBREAK_IsAWord()
            {
                WordRepository repository = new WordRepository() { ExcludeAdvancedWords = true, IgnoreCache = true};
                Assert.IsTrue(repository.IsAWord("outbreak"));
            }

            [Test]
            [Ignore("Takes more than 3 seconds.")]
            public void TRAMPOLINE_IsAWord()
            {
                WordRepository repository = new WordRepository() { ExcludeAdvancedWords = true };
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
            // ReSharper disable IdentifierTypo
            public void ZZZZ_IsNotAWord()
                // ReSharper restore IdentifierTypo
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
            [Ignore("Takes more than 3 seconds.")]
            public void OnlyBasic_Excludes_ASP()
            {
                WordRepository repository = new WordRepository()
                {
                    ExcludeAdvancedWords = true
                };
                Assert.IsFalse(repository.IsAWord("asp"));

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


