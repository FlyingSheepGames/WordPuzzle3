using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using WordPuzzles.Puzzle;
using WordPuzzles.Utility;

namespace WordPuzzlesTest.Puzzle
{
    [TestFixture]
    public class TrisectedWordsTest
    {
        [TestFixture]
        public class UnrelatedTests
        {
            [Test]
            public void AreThereEnoughSevenAndEightLetterWords()
            {
                WordRepository wordRepository = new WordRepository() {};

                int countOfSevenLetterWords = wordRepository.WordsMatchingPattern("_______").Count;
                int countOfEightLetterWords = wordRepository.WordsMatchingPattern("________").Count;
                Console.WriteLine($"There are {countOfSevenLetterWords} seven-letter words and {countOfEightLetterWords} eight-letter words");
                Assert.LessOrEqual(80, countOfSevenLetterWords, "Expected at least 80 seven-letter words.");
                Assert.LessOrEqual(80, countOfEightLetterWords, "Expected at least 80 eight-letter words.");
            }
        }

        [TestFixture]
        public class FindWordsContainingLetters
        {
            [Test]
            public void THE_FindsAtLeastOneWord()
            {
                TrisectedWordsPuzzle trisectedWordsPuzzle = new TrisectedWordsPuzzle();
                var foundWords = trisectedWordsPuzzle.FindWordsContainingLetters('t', 'h', 'e');
                foreach (var word in foundWords)
                {
                    Console.WriteLine(word.Word);
                }
                Assert.LessOrEqual(1, foundWords.Count, "Found at least one word.");
                var firstWord = foundWords[0];
                Assert.AreEqual("torched", firstWord.Word, "Expected first word to be torched ");
                Assert.AreEqual("t___he_", firstWord.Pattern, "Unexpected pattern.");
                Assert.AreEqual(SectionSubPattern.FIRST_OF_TWO_LETTERS, firstWord.PatternAsEnumArray[0], "Unexpected PatternAsEnumArray[0]");
                Assert.AreEqual(SectionSubPattern.THIRD_OF_THREE_LETTERS, firstWord.PatternAsEnumArray[1], "Unexpected PatternAsEnumArray[1]");
                Assert.AreEqual(SectionSubPattern.FIRST_OF_TWO_LETTERS, firstWord.PatternAsEnumArray[2], "Unexpected PatternAsEnumArray[2]");
            }
        }

        [TestFixture]
        public class GetNextWordCandidates
        {
            [Test]
            public void THE_ReturnsExpectedResults()
            {
                TrisectedWordsPuzzle trisectedWordsPuzzle = new TrisectedWordsPuzzle();
                trisectedWordsPuzzle.Solution = "The";
                var foundWords = trisectedWordsPuzzle.GetNextWordCandidates();
                foreach (var word in foundWords)
                {
                    Console.WriteLine(word.Word);
                }
                Assert.LessOrEqual(1, foundWords.Count, "Found at least one word.");
                var firstWord = foundWords[0];
                Assert.AreEqual("torched", firstWord.Word, "Expected first word to be torched ");
                Assert.AreEqual("t___he_", firstWord.Pattern, "Unexpected pattern.");
                Assert.AreEqual(SectionSubPattern.FIRST_OF_TWO_LETTERS, firstWord.PatternAsEnumArray[0], "Unexpected PatternAsEnumArray[0]");
                Assert.AreEqual(SectionSubPattern.THIRD_OF_THREE_LETTERS, firstWord.PatternAsEnumArray[1], "Unexpected PatternAsEnumArray[1]");
                Assert.AreEqual(SectionSubPattern.FIRST_OF_TWO_LETTERS, firstWord.PatternAsEnumArray[2], "Unexpected PatternAsEnumArray[2]");

                Assert.IsNull(trisectedWordsPuzzle.GetNextWordCandidates(), "We should be at the end of the solution now.");
            }

            [Test]
            public void THE_IgnoresSpacesAndSymbols()
            {
                TrisectedWordsPuzzle trisectedWordsPuzzle = new TrisectedWordsPuzzle();
                trisectedWordsPuzzle.Solution = "T  h . $#^&*e %&*($#";
                var foundWords = trisectedWordsPuzzle.GetNextWordCandidates();
                foreach (var word in foundWords)
                {
                    Console.WriteLine(word.Word);
                }
                Assert.LessOrEqual(1, foundWords.Count, "Found at least one word.");
                var firstWord = foundWords[0];
                Assert.AreEqual("torched", firstWord.Word, "Expected first word to be torched ");
                Assert.AreEqual("t___he_", firstWord.Pattern, "Unexpected pattern.");
                Assert.AreEqual(SectionSubPattern.FIRST_OF_TWO_LETTERS, firstWord.PatternAsEnumArray[0], "Unexpected PatternAsEnumArray[0]");
                Assert.AreEqual(SectionSubPattern.THIRD_OF_THREE_LETTERS, firstWord.PatternAsEnumArray[1], "Unexpected PatternAsEnumArray[1]");
                Assert.AreEqual(SectionSubPattern.FIRST_OF_TWO_LETTERS, firstWord.PatternAsEnumArray[2], "Unexpected PatternAsEnumArray[2]");

                Assert.IsNull(trisectedWordsPuzzle.GetNextWordCandidates(), "We should be at the end of the solution now.");
            }

            [Test]
            public void TWO_WORDS_ReturnsExpectedResults()
            {
                TrisectedWordsPuzzle trisectedWordsPuzzle = new TrisectedWordsPuzzle() {Verbose = true};
                trisectedWordsPuzzle.Solution = "Two words.";
                var foundWords = trisectedWordsPuzzle.GetNextWordCandidates();
                foreach (var word in foundWords)
                {
                    Console.WriteLine($"First word { word.Word}");
                }
                Assert.LessOrEqual(1, foundWords.Count, "Found at least one word.");
                var firstWord = foundWords[0];
                Assert.AreEqual("tawnier", firstWord.Word, "Expected first word to be tawnier ");
                Assert.AreEqual("t_w____", firstWord.Pattern, "Unexpected pattern.");
                Assert.AreEqual(SectionSubPattern.FIRST_OF_TWO_LETTERS, firstWord.PatternAsEnumArray[0], "Unexpected PatternAsEnumArray[0]");
                Assert.AreEqual(SectionSubPattern.FIRST_OF_TWO_LETTERS, firstWord.PatternAsEnumArray[1], "Unexpected PatternAsEnumArray[1]");
                Assert.AreEqual(SectionSubPattern.FIRST_OF_THREE_LETTERS, firstWord.PatternAsEnumArray[2], "Unexpected PatternAsEnumArray[2]");


                foundWords = trisectedWordsPuzzle.GetNextWordCandidates();
                foreach (var word in foundWords)
                {
                    Console.WriteLine($"Second word { word.Word}");
                }
                Assert.LessOrEqual(1, foundWords.Count, "Found at least one word.");
                var secondWord = foundWords[0];
                Assert.AreEqual("outwear", secondWord.Word, "Expected second word to be tawnier ");
                Assert.AreEqual("o__w___", secondWord.Pattern, "Unexpected pattern.");
                Assert.AreEqual(SectionSubPattern.FIRST_OF_TWO_LETTERS,   secondWord.PatternAsEnumArray[0], "Unexpected PatternAsEnumArray[0]");
                Assert.AreEqual(SectionSubPattern.SECOND_OF_TWO_LETTERS,   secondWord.PatternAsEnumArray[1], "Unexpected PatternAsEnumArray[1]");
                Assert.AreEqual(SectionSubPattern.FIRST_OF_THREE_LETTERS, secondWord.PatternAsEnumArray[2], "Unexpected PatternAsEnumArray[2]");

                foundWords = trisectedWordsPuzzle.GetNextWordCandidates();
                foreach (var word in foundWords)
                {
                    Console.WriteLine($"Third word { word.Word}");
                }
                Assert.LessOrEqual(1, foundWords.Count, "Found at least one word.");
                var thirdWord = foundWords[0];
                Assert.AreEqual("overbid", thirdWord.Word, "Expected third word to be overbid ");
                Assert.AreEqual("o__r__d", thirdWord.Pattern, "Unexpected pattern.");
                Assert.AreEqual(SectionSubPattern.FIRST_OF_TWO_LETTERS, thirdWord.PatternAsEnumArray[0], "Unexpected PatternAsEnumArray[0]");
                Assert.AreEqual(SectionSubPattern.SECOND_OF_TWO_LETTERS, thirdWord.PatternAsEnumArray[1], "Unexpected PatternAsEnumArray[1]");
                Assert.AreEqual(SectionSubPattern.THIRD_OF_THREE_LETTERS, thirdWord.PatternAsEnumArray[2], "Unexpected PatternAsEnumArray[2]");

                foundWords = trisectedWordsPuzzle.GetNextWordCandidates();
                foreach (var word in foundWords)
                {
                    Console.WriteLine($"Fourth word { word.Word}");
                }
                Assert.LessOrEqual(1, foundWords.Count, "Found at least one word.");
                var fourthWord = foundWords[0];
                Assert.AreEqual("salient", fourthWord.Word, "Expected third word to be salient ");
                Assert.AreEqual("s______", fourthWord.Pattern, "Unexpected pattern.");
                Assert.AreEqual(SectionSubPattern.FIRST_OF_TWO_LETTERS, fourthWord.PatternAsEnumArray[0], "Unexpected PatternAsEnumArray[0]");
                Assert.AreEqual(SectionSubPattern.FIRST_OF_TWO_LETTERS, fourthWord.PatternAsEnumArray[1], "Unexpected PatternAsEnumArray[1]");
                Assert.AreEqual(SectionSubPattern.FIRST_OF_THREE_LETTERS, fourthWord.PatternAsEnumArray[2], "Unexpected PatternAsEnumArray[2]");

                Assert.IsNull(trisectedWordsPuzzle.GetNextWordCandidates(), "We should be at the end of the solution now.");
            }
        }

        [TestFixture]
        public class AddClue
        {
            [Test]
            public void TWO_WORDS_ReturnsExpectedResults()
            {
                TrisectedWordsPuzzle trisectedWordsPuzzle = new TrisectedWordsPuzzle() { Verbose = true };
                trisectedWordsPuzzle.Solution = "Two words.";
                var foundWords = trisectedWordsPuzzle.GetNextWordCandidates();
                Assert.LessOrEqual(1, foundWords.Count, "Found at least one word.");
                var firstWord = foundWords[0];
                Assert.AreEqual("tawnier", firstWord.Word, "Expected first word to be tawnier ");
                firstWord.Clue = "More tawny.";
                trisectedWordsPuzzle.AddClue(firstWord);

                foundWords = trisectedWordsPuzzle.GetNextWordCandidates();
                Assert.LessOrEqual(1, foundWords.Count, "Found at least one word.");
                var secondWord = foundWords[0];
                Assert.AreEqual("outwear", secondWord.Word, "Expected second word to be tawnier ");
                secondWord.Clue = "Use clothing until it's no longer needed.";
                trisectedWordsPuzzle.AddClue(secondWord);

                foundWords = trisectedWordsPuzzle.GetNextWordCandidates();
                Assert.LessOrEqual(1, foundWords.Count, "Found at least one word.");
                var thirdWord = foundWords[0];
                Assert.AreEqual("overbid", thirdWord.Word, "Expected third word to be overbid ");
                thirdWord.Clue = "Pay too much on EBay";
                trisectedWordsPuzzle.AddClue(thirdWord);

                foundWords = trisectedWordsPuzzle.GetNextWordCandidates();
                Assert.LessOrEqual(1, foundWords.Count, "Found at least one word.");
                var fourthWord = foundWords[0];
                Assert.AreEqual("salient", fourthWord.Word, "Expected third word to be salient ");
                fourthWord.Clue = "Relevant";
                trisectedWordsPuzzle.AddClue(fourthWord);

                Assert.IsNull(trisectedWordsPuzzle.GetNextWordCandidates(), "We should be at the end of the solution now.");

                Assert.AreEqual(4, trisectedWordsPuzzle.Clues.Count, "Expected 4 clues.");
            }
        }

        [TestFixture]
        public class CalculateWordSections
        {

            [Test]
            public void TWO_WORDS_CreatesExpectedProperty()
            {
                //arrange
                TrisectedWordsPuzzle trisectedWordsPuzzle = new TrisectedWordsPuzzle() {  };
                trisectedWordsPuzzle.Solution = "Two words.";

                var foundWords = trisectedWordsPuzzle.GetNextWordCandidates();
                var firstWord = foundWords[0];
                firstWord.Clue = "More tawny.";
                trisectedWordsPuzzle.AddClue(firstWord);

                foundWords = trisectedWordsPuzzle.GetNextWordCandidates();
                var secondWord = foundWords[0];
                secondWord.Clue = "Use clothing until it's no longer needed.";
                trisectedWordsPuzzle.AddClue(secondWord);

                foundWords = trisectedWordsPuzzle.GetNextWordCandidates();
                var thirdWord = foundWords[0];
                thirdWord.Clue = "Pay too much on EBay";
                trisectedWordsPuzzle.AddClue(thirdWord);

                foundWords = trisectedWordsPuzzle.GetNextWordCandidates();
                var fourthWord = foundWords[0];
                fourthWord.Clue = "Relevant";
                trisectedWordsPuzzle.AddClue(fourthWord);

                //act
                trisectedWordsPuzzle.CalculateWordSections();

                //assert
                Assert.IsNull(trisectedWordsPuzzle.GetNextWordCandidates(), "We should be at the end of the solution now.");

                Assert.AreEqual(4, trisectedWordsPuzzle.Clues.Count, "Expected 4 clues.");
                foreach (var section in trisectedWordsPuzzle.WordSections)
                {
                    Console.WriteLine(section);
                }
                Assert.AreEqual(12, trisectedWordsPuzzle.WordSections.Count, "Expected 12 word sections.");
                CollectionAssert.AreEquivalent(new List<string>
                    { 
                        "ta", "wn", "ier", 
                        "ou", "tw", "ear", 
                        "ov", "er", "bid", 
                        "sa", "li", "ent"
                    }, trisectedWordsPuzzle.WordSections);
            }
        }
    }
}
