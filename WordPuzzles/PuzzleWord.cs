using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace WordPuzzles
{

    public class Puzzle
    {
        public List<PuzzleWord> Clues = new List<PuzzleWord>();
        public List<PuzzleLetter> Phrase = new List<PuzzleLetter>();
        public string PhraseAsString;
        int _wordCount = 1;
        int _letterCount = 1;
        private int _lettersInPhrase;

        [JsonIgnore]
        public int LettersInPhrase
        {
            get
            {
                if (_lettersInPhrase == 0)
                {
                    _lettersInPhrase = CalculateLettersInPhrase();
                }
                return _lettersInPhrase;
            }

        }

        private int CalculateLettersInPhrase()
        {
            int letterCount = 0;
            foreach (char letter in PhraseAsString)
            {
                if (char.IsLetter(letter))
                {
                    letterCount++;
                }
            }

            return letterCount;
        }

        public void AddWordToClues(string word)
        {
            Clues.Add(new PuzzleWord(word, _letterCount, (char) ('A' + _wordCount - 1)));
            _wordCount++;
            _letterCount += word.Length;
        }

        public void PlaceUniqueLetters()
        {
            foreach (char letter in PhraseAsString)
            {
                if (char.IsPunctuation(letter))
                {
                    Phrase.Add(new PuzzleLetter() {ActualLetter = letter});
                    continue;
                }
                if (letter == ' ')
                {
                    Phrase.Add(PuzzleLetter.BlankSpace);
                    continue;
                }


                var matchingLetter = FindMatchingLetter(letter);

                if (matchingLetter != null)
                {
                    Phrase.Add(matchingLetter);
                    matchingLetter.AlreadyPlaced = true;
                }
                else
                {
                    Phrase.Add(null);
                }
            }
        }

        private PuzzleLetter FindMatchingLetter(char letter)
        {
            var foundLetters = FindMatchingLetters(letter);
            if (1 < foundLetters.Count) return null;
            return foundLetters[0];
        }

        private List<PuzzleLetter> FindMatchingLetters(char letter, string clueIndiciesToExclude = "")
        {
            PuzzleLetter letterFromIndiciesToExclude = null;
            List<PuzzleLetter> matchingLetters = new List<PuzzleLetter>();
            foreach (PuzzleWord clue in Clues)
            {
                foreach (PuzzleLetter clueLetter in clue.Letters)
                {
                    if (clueLetter.ActualLetter == letter)
                    {
                        if (clueLetter.AlreadyPlaced) continue;
                        if (clueIndiciesToExclude.Contains(clueLetter.AlphabeticIndex))
                        {
                            letterFromIndiciesToExclude = clueLetter;
                            continue;
                        }

                        matchingLetters.Add(clueLetter);
                    }
                }
            }

            if (matchingLetters.Count == 0)
            {
                if (letterFromIndiciesToExclude != null)
                {
                    matchingLetters.Add(letterFromIndiciesToExclude);
                }
            }
            return matchingLetters;
        }

        public List<PuzzleLetter> CalculateOptions(int phraseIndex, string clueIndiciesToExclude = "")
        {
            char letterToFind = PhraseAsString[phraseIndex];

            return FindMatchingLetters(letterToFind, clueIndiciesToExclude);
        }

        public bool PlaceForcedLetters(out int maxNumberOfOptions, int thresholdForNumberOfOptions = 1)
        {
            maxNumberOfOptions = 0;
            bool placedAtLeastOneLetter = false;
            Dictionary<int, string> missingLetters = new Dictionary<int, string>(); //int = index in phrase that's missing; string = Alphabetic Index of clue words already used.
            List<int> missingIndicies = new List<int>();
            int phraseIndex = 0;
            string clueIndiciesToExclude = "";
            string[] lettersToExcludeIndexByWord = {
                "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "",
                "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "",

            };
            int currentWordIndex = 0;
            foreach (PuzzleLetter currentLetter in Phrase)
            {
                if (currentLetter is null)
                {
                    missingLetters.Add(phraseIndex, "");
                    missingIndicies.Add(phraseIndex);
                }
                else
                {
                    if (currentLetter.ActualLetter == ' ') //reached the end of the word.
                    {
                        foreach (int missingIndex in missingIndicies)
                        {
                            missingLetters[missingIndex] = clueIndiciesToExclude;
                        }

                        if (lettersToExcludeIndexByWord.Length <= currentWordIndex)
                        {
                            throw new Exception("Make the lettersToExcludeIndexByWord array longer, please.");
                        }
                        lettersToExcludeIndexByWord[currentWordIndex] = clueIndiciesToExclude;
                        //reset for next word.
                        missingIndicies.Clear();
                        clueIndiciesToExclude = "";
                        currentWordIndex++;
                    }
                    else
                    {
                        clueIndiciesToExclude += currentLetter.AlphabeticIndex;
                    }
                }
                phraseIndex++;
            }
            foreach (int missingIndex in missingIndicies)
            {
                missingLetters[missingIndex] = clueIndiciesToExclude;
            }
            lettersToExcludeIndexByWord[currentWordIndex] = clueIndiciesToExclude;

            foreach (int missingIndex in missingLetters.Keys)
            {
                int wordIndexForLetterIndex = CalculateWordIndexFromLetterIndex(missingIndex);
                if (missingIndex == 1)
                {
                    Console.WriteLine($"index {missingIndex}, skip letters {lettersToExcludeIndexByWord[wordIndexForLetterIndex]}");
                }

                if (lettersToExcludeIndexByWord[wordIndexForLetterIndex] != missingLetters[missingIndex])
                {
                    //Console.WriteLine($"Uh oh. {lettersToExcludeIndexByWord[wordIndexForLetterIndex]} != {MissingLetters[missingIndex]}");
                }

                //var options = CalculateOptions(missingIndex, MissingLetters[missingIndex]);
                var options = CalculateOptions(missingIndex, lettersToExcludeIndexByWord[wordIndexForLetterIndex]);

                if (options.Count == 0)
                {
                    throw new Exception("Zero options found!");
                }
                if (options.Count <= thresholdForNumberOfOptions)
                {
                    Phrase[missingIndex] = options[0];
                    lettersToExcludeIndexByWord[wordIndexForLetterIndex] += options[0].AlphabeticIndex;
                    options[0].AlreadyPlaced = true;
                    placedAtLeastOneLetter = true;
                }

                if (maxNumberOfOptions < options.Count)
                {
                    maxNumberOfOptions = options.Count;
                }
            }

            return placedAtLeastOneLetter;
        }

        private int CalculateWordIndexFromLetterIndex(int letterIndex)
        {
            string subPhrase = PhraseAsString.Substring(0, letterIndex);
            int wordIndex = 0;
            foreach (char letter in subPhrase)
            {
                if (letter == ' ') wordIndex++;
            }

            return wordIndex;
        }

        public void PlaceLetters()
        {
            PlaceUniqueLetters();
            const int STACK_LIMIT = 20;
            for (int loopCounter = 0; loopCounter < STACK_LIMIT; loopCounter++)
            {
                int maxNumberOfOptions;
                if (!PlaceForcedLetters(out maxNumberOfOptions))
                {
                    //Console.WriteLine(countOfRemainingNulls);
                    for ( int threshold = 2; threshold <= maxNumberOfOptions; threshold++)
                    {
                        //Console.WriteLine($"Didn't place any letters. Will take first of {threshold}/ {maxNumberOfOptions} options when available");
                        if (PlaceForcedLetters(out maxNumberOfOptions, threshold))
                        {
                            break;
                        }

                        if (threshold == maxNumberOfOptions - 1)
                        {

                        }
                    }
                }

                if (loopCounter == STACK_LIMIT -1)
                {
                    Console.WriteLine("hit stack limit");
                }
            }

            foreach (var letter in Phrase)
            {
                if (letter is null)
                {
                    Console.Write("*");
                }
                else
                {
                    Console.Write(letter.ActualLetter);
                }
            }

            Console.WriteLine();
            foreach (var letter in Phrase)
            {
                if (letter is null)
                {
                    Console.Write("*");
                }
                else
                {
                    Console.Write(letter.AlphabeticIndex);
                }
            }

        }
    }

    public class PuzzleWord
    {
        public List<PuzzleLetter> Letters = new List<PuzzleLetter>();
        private readonly string _originalWord;

        public PuzzleWord(string word, int startingIndex, char wordIndex)
        {
            _originalWord = word;
            foreach (char character in word)
            {
                Letters.Add(
                    new PuzzleLetter()
                    {
                        ActualLetter = character, 
                        AlphabeticIndex = wordIndex, 
                        NumericIndex = startingIndex++,
                    });
            }
        }

        public static implicit operator string(PuzzleWord v) { return v._originalWord; }

        public string CustomizedClue { get; set; }
    }

    public class PuzzleLetter
    {
        public char AlphabeticIndex;
        public int NumericIndex;
        public char ActualLetter;

        public static PuzzleLetter BlankSpace => new PuzzleLetter() {ActualLetter = ' ', AlphabeticIndex = ' ', AlreadyPlaced = true};
        public bool AlreadyPlaced;

        public override string ToString()
        {
            if (ActualLetter == ' ') return "";

            return AlphabeticIndex + NumericIndex.ToString();
        }
    }
}
