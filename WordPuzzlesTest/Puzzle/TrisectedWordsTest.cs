using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
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

            [Test]
            [TestCase("zz")]
            [TestCase("z z")]
            [TestCase(" z z ")]
            [TestCase(".z...z.")]
            public void ZZ_ReturnsExpectedResults(string solutionWithTwoZs)
            {
                TrisectedWordsPuzzle puzzle = new TrisectedWordsPuzzle();
                puzzle.Repository = new WordRepository() {ExcludeAdvancedWords = true};

                puzzle.Solution = solutionWithTwoZs;

                var results = puzzle.GetNextWordCandidates();
                Assert.Less(0, results.Count, "Expected at least one result.");
                var firstResultForFirstZ = results[0];
                Assert.AreEqual("haziest", firstResultForFirstZ.Word, "Unexpected word");
                Assert.AreEqual("__z____", firstResultForFirstZ.Pattern, "Unexpected pattern");

                 results = puzzle.GetNextWordCandidates();
                Assert.Less(0, results.Count, "Expected at least one result.");
                var firstResultForSecondZ = results[0];
                Assert.AreEqual("zestful", firstResultForSecondZ.Word, "Unexpected word");
                Assert.AreEqual("z______", firstResultForSecondZ.Pattern, "Unexpected pattern");

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


        [TestFixture]
        public class FormatHtmlForGoogle
        {
            [Test]
            [TestCase(true)]
            [TestCase(false)]
            public void TWO_WORDS_ReturnsExpectedResult(bool includeSolution)
            {
                const string HTML_DIRECTORY = @"html\TrisectedWordsPuzzle\";
                string SOURCE_DIRECTORY = ConfigurationManager.AppSettings["SourceDirectory"] + "TrisectedWordsPuzzle";

                TrisectedWordsPuzzle puzzle = new TrisectedWordsPuzzle() { };
                puzzle.Solution = "Two words.";

                var foundWords = puzzle.GetNextWordCandidates();
                var firstWord = foundWords[0];
                firstWord.Clue = "More tawny.";
                puzzle.AddClue(firstWord);

                foundWords = puzzle.GetNextWordCandidates();
                var secondWord = foundWords[0];
                secondWord.Clue = "Use clothing until it's no longer needed.";
                puzzle.AddClue(secondWord);

                foundWords = puzzle.GetNextWordCandidates();
                var thirdWord = foundWords[0];
                thirdWord.Clue = "Pay too much on EBay";
                puzzle.AddClue(thirdWord);

                foundWords = puzzle.GetNextWordCandidates();
                var fourthWord = foundWords[0];
                fourthWord.Clue = "Relevant";
                puzzle.AddClue(fourthWord);


                string generatedHtml = puzzle.FormatHtmlForGoogle(includeSolution);

                var actualFileName = "actualExample1.html";
                if (includeSolution)
                {
                    actualFileName = "actualExampleWithSolution1.html";
                }
                File.WriteAllText(HTML_DIRECTORY + actualFileName, generatedHtml);
                var expectedFileName = "expectedExample1.html";
                if (includeSolution)
                {
                    expectedFileName = "expectedExampleWithSolution1.html";
                }

                string[] expectedLines = new[] { " " };// need to have something to be different from generated file.
                if (File.Exists(HTML_DIRECTORY + expectedFileName))
                {
                    expectedLines = File.ReadAllLines(HTML_DIRECTORY + expectedFileName);
                }
                var actualLines = File.ReadAllLines(HTML_DIRECTORY + actualFileName);
                bool anyLinesDifferent = false;
                for (var index = 0; index < expectedLines.Length; index++)
                {
                    string expectedLine = expectedLines[index];
                    string actualLine = "End of file already reached.";
                    if (index >= 0 && actualLines.Length > index)
                    {
                        actualLine = actualLines[index];
                    }

                    if (!expectedLine.Equals(actualLine, StringComparison.InvariantCultureIgnoreCase))
                    {
                        anyLinesDifferent = true;
                        Console.WriteLine($"Expected Line {index}:{expectedLine}");
                        Console.WriteLine($"  Actual Line {index}:{actualLine}");
                    }
                }

                if (anyLinesDifferent)
                {
                    Console.WriteLine("Updating source file. Will show up as a difference in source control.");
                    File.WriteAllLines(SOURCE_DIRECTORY + $@"\{expectedFileName}", actualLines);
                }
                Assert.IsFalse(anyLinesDifferent, "Didn't expect any lines to be different.");

            }

            [Test]
            [TestCase(true)]
            [TestCase(false)]
            public void EightLetterClue_ReturnsExpectedResult(bool includeSolution)
            {
                const string HTML_DIRECTORY = @"html\TrisectedWordsPuzzle\";
                string SOURCE_DIRECTORY = ConfigurationManager.AppSettings["SourceDirectory"] + "TrisectedWordsPuzzle";

                TrisectedWordsPuzzle puzzle = new TrisectedWordsPuzzle() { };
                puzzle.Solution = "Two words.";

                var foundWords = puzzle.GetNextWordCandidates();
                var firstWord = foundWords[0];
                firstWord.Clue = "More tawny.";
                puzzle.AddClue(firstWord);

                foundWords = puzzle.GetNextWordCandidates();
                var secondWord = foundWords[0];
                secondWord.Clue = "Use clothing until it's no longer needed.";
                puzzle.AddClue(secondWord);

                foundWords = puzzle.GetNextWordCandidates();
                var thirdWord = foundWords[0];
                thirdWord.Clue = "Pay too much on EBay";
                puzzle.AddClue(thirdWord);

                foundWords = puzzle.GetNextWordCandidates();
                var fourthWord = foundWords[1420];
                fourthWord.Clue = "Using a sword";
                puzzle.AddClue(fourthWord);


                string generatedHtml = puzzle.FormatHtmlForGoogle(includeSolution);

                var actualFileName = "actualExample2.html";
                if (includeSolution)
                {
                    actualFileName = "actualExampleWithSolution2.html";
                }
                File.WriteAllText(HTML_DIRECTORY + actualFileName, generatedHtml);
                var expectedFileName = "expectedExample2.html";
                if (includeSolution)
                {
                    expectedFileName = "expectedExampleWithSolution2.html";
                }

                string[] expectedLines = new[] { " " };// need to have something to be different from generated file.
                if (File.Exists(HTML_DIRECTORY + expectedFileName))
                {
                    expectedLines = File.ReadAllLines(HTML_DIRECTORY + expectedFileName);
                }
                var actualLines = File.ReadAllLines(HTML_DIRECTORY + actualFileName);
                bool anyLinesDifferent = false;
                for (var index = 0; index < expectedLines.Length; index++)
                {
                    string expectedLine = expectedLines[index];
                    string actualLine = "End of file already reached.";
                    if (index >= 0 && actualLines.Length > index)
                    {
                        actualLine = actualLines[index];
                    }

                    if (!expectedLine.Equals(actualLine, StringComparison.InvariantCultureIgnoreCase))
                    {
                        anyLinesDifferent = true;
                        Console.WriteLine($"Expected Line {index}:{expectedLine}");
                        Console.WriteLine($"  Actual Line {index}:{actualLine}");
                    }
                }

                if (anyLinesDifferent)
                {
                    Console.WriteLine("Updating source file. Will show up as a difference in source control.");
                    File.WriteAllLines(SOURCE_DIRECTORY + $@"\{expectedFileName}", actualLines);
                }
                Assert.IsFalse(anyLinesDifferent, "Didn't expect any lines to be different.");

            }

        }

    }
}
