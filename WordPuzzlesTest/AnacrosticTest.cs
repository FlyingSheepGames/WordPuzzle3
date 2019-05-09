using System;
using System.Text;
using NUnit.Framework;
using WordPuzzles;

namespace WordSquareGeneratorTest
{
    [TestFixture]
    public class AnacrosticTest
    {
        [TestFixture]
        public class Constructor
        {
            [Test]
            public void CreatesListOfLetters()
            {
                Anacrostic anacrostic = new Anacrostic("az");
                Assert.AreEqual(1, anacrostic.Remaining('a'));
                Assert.AreEqual(1, anacrostic.Remaining('z'));
                for (char letter = 'b'; letter < 'z'; letter++)
                {
                    Assert.AreEqual(0, anacrostic.Remaining(letter));
                }
            }

            [Test]
            public void Multiples()
            {
                Anacrostic anacrostic = new Anacrostic("aaz");
                Assert.AreEqual(2, anacrostic.Remaining('a'));
                Assert.AreEqual(1, anacrostic.Remaining('z'));
                for (char letter = 'b'; letter < 'z'; letter++)
                {
                    Assert.AreEqual(0, anacrostic.Remaining(letter));
                }
            }

        }

        [TestFixture]
        public class FindNextWord
        {
            [Test]
            public void WHICH_FindsWord()
            {
                Anacrostic  anacrostic = new Anacrostic("whic h");
                Assert.AreEqual("which", anacrostic.FindNextWord());
            }

            [Test]
            public void WillNotFindWordWithRepeatedLetters_IfNotEnoughLettersRemain()
            {
                Anacrostic anacrostic = new Anacrostic("pule");
                Assert.AreEqual(null, anacrostic.FindNextWord());
            }

            [Test]
            public void SimpleExample_FindsKEEPS()
            {
                Anacrostic anacrostic = new Anacrostic("kbpmmurrhhstttttteeee");
                anacrostic.WordsFoundSoFar.Add("there");
                Assert.AreEqual("keeps", anacrostic.FindNextWord());
            }

            [Test]
            public void FourLetterExample_FindsPEEK()
            {
                Anacrostic anacrostic = new Anacrostic("keep");
                Assert.AreEqual("peek", anacrostic.FindNextWord());
            }

            [Test]
            public void IgnoresWords()
            {
                Anacrostic anacrostic = new Anacrostic("keep");
                anacrostic.IgnoreWord("keep");
                Assert.AreEqual("peek", anacrostic.FindNextWord());
            }

            [Test]
            public void FindsThreeLetterWords()
            {
                Anacrostic anacrostic = new Anacrostic("m om");
                Assert.AreEqual("mom", anacrostic.FindNextWord());
            }

            [Test]
            public void FindsLINT()
            {
                Anacrostic anacrostic = new Anacrostic("lin t");
                Assert.AreEqual("lint", anacrostic.FindNextWord());
            }

            [Test]
            [TestCase("on")]
            [TestCase("onx")]
            public void NonWords_FindsNothing(string phrase)
            {
                Anacrostic anacrostic = new Anacrostic(phrase);
                Assert.IsNull(anacrostic.FindNextWord());
            }
        }

        [TestFixture]
        public class RemoveWord
        {
            [Test]
            public void WHICH_FindsWord()
            {
                Anacrostic anacrostic = new Anacrostic("which");
                anacrostic.RemoveWord("which");
                for (char letter = 'a'; letter <= 'z'; letter++)
                {
                    Assert.AreEqual(0, anacrostic.Remaining(letter));
                }
            }
        }

        [TestFixture]
        public class RemainingLetters
        {
            [Test]
            public void WHICHZ_HasZLeft()
            {
                Anacrostic anacrostic = new Anacrostic("whichz");
                anacrostic.RemoveWord("which");
                Assert.AreEqual("z", anacrostic.RemainingLetters());
            }
        }

        [TestFixture]
        public class WordsWithNumberedBlanks
        {
            [Test]
            public void WHICHZ_HasZLeft()
            {
                Anacrostic anacrostic = new Anacrostic("whichz");
                anacrostic.FindNextWord();
                anacrostic.RemoveWord("which");
                Assert.AreEqual("z", anacrostic.RemainingLetters());
                Assert.AreEqual(
@"which A1 A2 A3 A4 A5 
z B6 
", anacrostic.WordsWithNumberedBlanks());
            }
        }

        [TestFixture]
        public class WordsFormattedForGoogleDocs
        {
            [Test]
            public void WHICHZ_ReturnsExpectedResults()
            {
                Anacrostic anacrostic = new Anacrostic("whichz");
                anacrostic.FindNextWord();
                anacrostic.RemoveWord("which");
                Assert.AreEqual("z", anacrostic.RemainingLetters());
                Assert.AreEqual(
                    @"clue for which						clue for z		
W	H	I	C	H		Z		
A1	A2	A3	A4	A5		B6		

", anacrostic.WordsFormattedForGoogleDocs());
            }
            [Test]
            public void ThreeWords_ReturnsExpectedResults()
            {
                Anacrostic anacrostic = new Anacrostic("whichzonepretty");
                anacrostic.FindNextWord();
                anacrostic.RemoveWord("which");
                anacrostic.RemoveWord("zone");
                anacrostic.RemoveWord("pretty");
                Assert.AreEqual("", anacrostic.RemainingLetters());
                Assert.AreEqual(
                    @"clue for which						clue for zone					
W	H	I	C	H		Z	O	N	E		
A1	A2	A3	A4	A5		B6	B7	B8	B9		

clue for pretty							
P	R	E	T	T	Y		
C10	C11	C12	C13	C14	C15		

", anacrostic.WordsFormattedForGoogleDocs());
            }

        }

        [TestFixture]
        public class GetFormattedHtmlForGoogle
        {
            [Test]
            public void WHICHZ_ReturnsExpectedResults()
            {
                Anacrostic anacrostic = new Anacrostic("whichz");
                anacrostic.FindNextWord();
                anacrostic.RemoveWord("which");
                Assert.AreEqual("z", anacrostic.RemainingLetters());
                
                const string EXPECTED_HTML =
@"<html>
<body>
<!--StartFragment-->
Fill in the blanks below based on the clues. 
<table border=""1"">
<tr>
    <td colspan=""5"">Clue for which</td>
    <td> </td>
    <td colspan=""1"">Clue for z</td>
</tr>
<tr>
    <td>W</td>
    <td>H</td>
    <td>I</td>
    <td>C</td>
    <td>H</td>
    <td> </td>
    <td>Z</td>
</tr>
<tr>
    <td>A1</td>
    <td>A2</td>
    <td>A3</td>
    <td>A4</td>
    <td>A5</td>
    <td> </td>
    <td>B6</td>
</tr>
</table>
Then copy the letters to the grid below, using the numbers as a guide. 
<table border=""1"">
<tr>
    <td> </td>
    <td> </td>
    <td> </td>
    <td> </td>
    <td> </td>
    <td> </td>
</tr>
<tr>
    <td>A1</td>
    <td>A2</td>
    <td>A3</td>
    <td>A4</td>
    <td>A5</td>
    <td>B6</td>
</tr>
</table>
<!--EndFragment-->
</body>
</html>
";
                Assert.AreEqual(
                    EXPECTED_HTML, anacrostic.GetFormattedHtmlForGoogle());
            }

            [Test]
            public void LongerPhrase_ReturnsExpectedResults()
            {
                Anacrostic anacrostic = new Anacrostic("this longer phrase has at least twenty characters");

                anacrostic.RemoveWord("place");
                anacrostic.RemoveWord("years");

                anacrostic.RemoveWord("great");
                anacrostic.RemoveWord("which");

                anacrostic.RemoveWord("rates");
                anacrostic.RemoveWord("later");

                anacrostic.RemoveWord("hosts");
                anacrostic.RemoveWord("hats");

                Assert.AreEqual("nnt", anacrostic.RemainingLetters());
                const string EXPECTED_HTML =
                    @"<html>
<body>
<!--StartFragment-->
Fill in the blanks below based on the clues. 
<table border=""1"">
<tr>
    <td colspan=""5"">Clue for place</td>
    <td> </td>
    <td colspan=""5"">Clue for years</td>
</tr>
<tr>
    <td>P</td>
    <td>L</td>
    <td>A</td>
    <td>C</td>
    <td>E</td>
    <td> </td>
    <td>Y</td>
    <td>E</td>
    <td>A</td>
    <td>R</td>
    <td>S</td>
</tr>
<tr>
    <td>A1</td>
    <td>A2</td>
    <td>A3</td>
    <td>A4</td>
    <td>A5</td>
    <td> </td>
    <td>B6</td>
    <td>B7</td>
    <td>B8</td>
    <td>B9</td>
    <td>B10</td>
</tr>
<tr>
    <td colspan=""5"">Clue for great</td>
    <td> </td>
    <td colspan=""5"">Clue for which</td>
</tr>
<tr>
    <td>G</td>
    <td>R</td>
    <td>E</td>
    <td>A</td>
    <td>T</td>
    <td> </td>
    <td>W</td>
    <td>H</td>
    <td>I</td>
    <td>C</td>
    <td>H</td>
</tr>
<tr>
    <td>C11</td>
    <td>C12</td>
    <td>C13</td>
    <td>C14</td>
    <td>C15</td>
    <td> </td>
    <td>D16</td>
    <td>D17</td>
    <td>D18</td>
    <td>D19</td>
    <td>D20</td>
</tr>
<tr>
    <td colspan=""5"">Clue for rates</td>
    <td> </td>
    <td colspan=""5"">Clue for later</td>
</tr>
<tr>
    <td>R</td>
    <td>A</td>
    <td>T</td>
    <td>E</td>
    <td>S</td>
    <td> </td>
    <td>L</td>
    <td>A</td>
    <td>T</td>
    <td>E</td>
    <td>R</td>
</tr>
<tr>
    <td>E21</td>
    <td>E22</td>
    <td>E23</td>
    <td>E24</td>
    <td>E25</td>
    <td> </td>
    <td>F26</td>
    <td>F27</td>
    <td>F28</td>
    <td>F29</td>
    <td>F30</td>
</tr>
<tr>
    <td colspan=""5"">Clue for hosts</td>
    <td> </td>
    <td colspan=""4"">Clue for hats</td>
</tr>
<tr>
    <td>H</td>
    <td>O</td>
    <td>S</td>
    <td>T</td>
    <td>S</td>
    <td> </td>
    <td>H</td>
    <td>A</td>
    <td>T</td>
    <td>S</td>
</tr>
<tr>
    <td>G31</td>
    <td>G32</td>
    <td>G33</td>
    <td>G34</td>
    <td>G35</td>
    <td> </td>
    <td>H36</td>
    <td>H37</td>
    <td>H38</td>
    <td>H39</td>
</tr>
<tr>
    <td colspan=""3"">Clue for nnt</td>
    <td> </td>
</tr>
<tr>
    <td>N</td>
    <td>N</td>
    <td>T</td>
    <td> </td>
</tr>
<tr>
    <td>I40</td>
    <td>I41</td>
    <td>I42</td>
    <td> </td>
</tr>
</table>
Then copy the letters to the grid below, using the numbers as a guide. 
<table border=""1"">
<tr>
    <td> </td>
    <td> </td>
    <td> </td>
    <td> </td>
    <td> </td>
    <td> </td>
    <td> </td>
    <td> </td>
    <td> </td>
    <td> </td>
</tr>
<tr>
    <td>C15</td>
    <td>D17</td>
    <td>D18</td>
    <td>B10</td>
    <td> </td>
    <td>A2</td>
    <td>G32</td>
    <td>I40</td>
    <td>C11</td>
    <td>A5</td>
</tr>
<tr>
    <td> </td>
    <td> </td>
    <td> </td>
    <td> </td>
    <td> </td>
    <td> </td>
    <td> </td>
    <td> </td>
    <td> </td>
    <td> </td>
</tr>
<tr>
    <td>B9</td>
    <td> </td>
    <td>A1</td>
    <td>D20</td>
    <td>C12</td>
    <td>A3</td>
    <td>E25</td>
    <td>B7</td>
    <td> </td>
    <td>G31</td>
</tr>
<tr>
    <td> </td>
    <td> </td>
    <td> </td>
    <td> </td>
    <td> </td>
    <td> </td>
    <td> </td>
    <td> </td>
    <td> </td>
    <td> </td>
</tr>
<tr>
    <td>B8</td>
    <td>G33</td>
    <td> </td>
    <td>C14</td>
    <td>E23</td>
    <td> </td>
    <td>F26</td>
    <td>C13</td>
    <td>E22</td>
    <td>G35</td>
</tr>
<tr>
    <td> </td>
    <td> </td>
    <td> </td>
    <td> </td>
    <td> </td>
    <td> </td>
    <td> </td>
    <td> </td>
    <td> </td>
    <td> </td>
</tr>
<tr>
    <td>F28</td>
    <td> </td>
    <td>G34</td>
    <td>D16</td>
    <td>E24</td>
    <td>I41</td>
    <td>H38</td>
    <td>B6</td>
    <td> </td>
    <td>A4</td>
</tr>
<tr>
    <td> </td>
    <td> </td>
    <td> </td>
    <td> </td>
    <td> </td>
    <td> </td>
    <td> </td>
    <td> </td>
    <td> </td>
    <td> </td>
</tr>
<tr>
    <td>H36</td>
    <td>F27</td>
    <td>E21</td>
    <td>H37</td>
    <td>D19</td>
    <td>I42</td>
    <td>F29</td>
    <td>F30</td>
    <td>H39</td>
    <td></td>
</tr>
</table>
<!--EndFragment-->
</body>
</html>
";
                string formattedHtmlForGoogle = anacrostic.GetFormattedHtmlForGoogle();
                Console.WriteLine();
                Console.WriteLine(anacrostic.EncodedPhrase);
                Console.WriteLine();
                Console.WriteLine(anacrostic.EncodedPhraseForGoogle);
                Assert.AreEqual(10, anacrostic.LineLength);
                Assert.AreEqual(
                    EXPECTED_HTML, formattedHtmlForGoogle);

            }
        }

        [TestFixture]
        public class EncodedPhrase
        {
            [Test]
            public void WHICHZ_ReturnsExpectedResults()
            {
                Anacrostic anacrostic = new Anacrostic("whichz");
                anacrostic.FindNextWord();
                anacrostic.RemoveWord("which");
                Assert.AreEqual("z", anacrostic.RemainingLetters());
                anacrostic.WordsFormattedForGoogleDocs();
                Assert.AreEqual(
                    @"A1A2A3A4A5B6", anacrostic.EncodedPhrase);
                Assert.AreEqual(
                    "A1\tA2\tA3\tA4\tA5\tB6\r\n", anacrostic.EncodedPhraseForGoogle);
                Assert.AreEqual(
                    "A1\tA5\tA3\tA4\tA2\tB6\r\n", anacrostic.GetEncodedPhraseForGoogle());

            }

            [Test]
            public void LongerPhrase_ReturnsExpectedResults()
            {
                Anacrostic anacrostic = new Anacrostic("this longer phrase has at least twenty characters");

                anacrostic.RemoveWord("place");
                anacrostic.RemoveWord("years");
                anacrostic.RemoveWord("great");
                anacrostic.RemoveWord("which");
                anacrostic.RemoveWord("rates");
                anacrostic.RemoveWord("later");
                anacrostic.RemoveWord("hosts");
                anacrostic.RemoveWord("hats");

                Assert.AreEqual("nnt", anacrostic.RemainingLetters());
                anacrostic.WordsFormattedForGoogleDocs();
                Assert.AreEqual(
                    @"C15D17D18B10 A2G32I40C11A5B9 A1D20C12A3E25B7 G31B8G33 C14E23 F26C13E22G35F28 G34D16E24I41H38B6 A4H36F27E21H37D19I42F29F30H39", anacrostic.EncodedPhrase);
                Assert.AreEqual(
@"C15	D17	D18	B10	 	A2	G32	I40	C11	A5
B9	 	A1	D20	C12	A3	E25	B7	 	G31
B8	G33	 	C14	E23	 	F26	C13	E22	G35
F28	 	G34	D16	E24	I41	H38	B6	 	A4
H36	F27	E21	H37	D19	I42	F29	F30	H39	", anacrostic.EncodedPhraseForGoogle);

                Assert.AreEqual(
@"H38	G31	D18	B10		A2	G32	I40	C11	B7
E21		A1	D17	B9	E22	G33	C13		D20
A3	G35		F27	I42		F26	A5	C14	H39
E23		C15	D16	E24	I41	F28	B6		A4
H36	H37	C12	B8	D19	G34	F29	F30	E25	", anacrostic.GetEncodedPhraseForGoogle());


            }

        }

        [TestFixture]
        public class LineLengthProperty
        {
            [Test]
            [TestCase(24, 13)]
            [TestCase(45, 9)]
            [TestCase(63, 13)]
            [TestCase(65, 13)]
            public void SpecificPuzzleLength_SetsCorrectLineLength(int puzzleLength, int expectedLineLength)
            {
                string puzzlePhrase = new string('a', puzzleLength);
                Anacrostic anacrostic = new Anacrostic(puzzlePhrase);
                Assert.AreEqual(expectedLineLength, anacrostic.LineLength, $"Unexpected Line Length for puzzle phrase of length {puzzleLength}");
            }

            [Test]
            public void First56Numbers_SetsCorrectLineLength()
            {
                for (int puzzleLength = 1; puzzleLength <= 56; puzzleLength++)
                {
                    string puzzlePhrase = new string('a', puzzleLength);
                    Anacrostic anacrostic = new Anacrostic(puzzlePhrase);
                    int actualLineLength = anacrostic.LineLength;
                    if (actualLineLength == puzzleLength) continue;
                    Assert.LessOrEqual(actualLineLength, 14, "Line length should be 14 or less.");
                    Assert.LessOrEqual(8, actualLineLength, "Line length should be 9 or more.");
                }
            }
        }

        [TestFixture]
        public class WhichWordsSupportAllLetters
        {
            //There was at least word for each 5-letter in position 1
            //There was at least word for each 6-letter in position 2
            [Test]
            public void AllWords()
            {
                WordRepository repository = new WordRepository() {ExludeAdvancedWords = true};
                for (int wordSize = 3; wordSize < 7; wordSize++)
                {
                    for (int index = 0; index < wordSize; index++)
                    {
                        bool atLeastOneWordForEachLetter = true;

                        foreach (char letter in Anacrostic.LettersInReverseFrequency)
                        {
                            StringBuilder builder = new StringBuilder();
                            builder.Append('_', index);
                            builder.Append(letter);
                            builder.Append('_', wordSize - (index + 1));

                            Assert.AreEqual(wordSize, builder.Length);
                            if (repository.WordsMatchingPattern(builder.ToString()).Count == 0)
                            {
                                Console.WriteLine(
                                    $"No {wordSize}-letter words match {builder} (letter in {index} position)");
                                atLeastOneWordForEachLetter = false;
                                break;
                            }
                        }

                        if (atLeastOneWordForEachLetter)
                        {
                            Console.WriteLine(
                                $"There was at least word for each {wordSize}-letter in position {index}");
                        }
                    }
                }

            }

            [Test]
            public void GenerateCodeForWordLengthStartingWithDictionary()
            {
                WordRepository repository = new WordRepository() { ExludeAdvancedWords = true };
                for (char initialLetter = 'a'; initialLetter <= 'z'; initialLetter++)
                {
                    StringBuilder wordLengthsThatStartWithThisLetter = new StringBuilder();
                    for (int wordSize = 3; wordSize < 7; wordSize++)
                    {
                        StringBuilder patternBuilder = new StringBuilder();
                        patternBuilder.Append(initialLetter);
                        patternBuilder.Append('_', wordSize - 1);
                        string pattern = patternBuilder.ToString();
                        if (0 < repository.WordsMatchingPattern(pattern).Count)
                        {
                            wordLengthsThatStartWithThisLetter.Append(wordSize);
                        }
                    }
                    Console.WriteLine($"wordLengthStartingWithLetter.Add('{initialLetter}', \"{wordLengthsThatStartWithThisLetter}\");");
                }

            }
        }

    }
}