using System;
using System.Collections.Generic;
using System.IO;
using WordPuzzles.Utility;

namespace WordPuzzles.Puzzle
{
    public class WordSearchMoreOrLess
    {
        private Dictionary<char, List<TakeTwoClue>> dictionaryOfClues;
        private bool alreadyLoaded;
        private Random _randomNumberGenerator;
        public List<HiddenWordInGrid> HiddenWords = new List<HiddenWordInGrid>();
        public int RandomGeneratorSeed { get; set; } = 0;

        public Dictionary<char, List<TakeTwoClue>> DictionaryOfClues
        {
            get
            {
                if (!alreadyLoaded)
                {
                    string serializedClues =
                        File.ReadAllText(
                            @"C:\Users\Chip\Source\Repos\WordPuzzle3\WordPuzzlesTest\data\AddALetter\dictionary.json");
                    dictionaryOfClues =
                        Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<char, List<TakeTwoClue>>>(
                            serializedClues);
                    alreadyLoaded = true;
                }
                return dictionaryOfClues;
            }
        }

        public Random RandomNumberGenerator
        {
            get
            {
                if (_randomNumberGenerator == null)
                {
                    _randomNumberGenerator = new Random(RandomGeneratorSeed);
                }
                return _randomNumberGenerator;
            }
        }


        public void ProcessLetter(char letterToAdd)
        {
            List<TakeTwoClue> clues = DictionaryOfClues[letterToAdd];
            clues.Shuffle(RandomNumberGenerator);
            bool addLetter = 0 == RandomNumberGenerator.Next(1);
            HiddenWords.Add(new HiddenWordInGrid(clues[0], addLetter));
        }
    }

    public class HiddenWordInGrid
    {
        public string DisplayedWord;
        public string HiddenWord;
        public char LetterAddedOrRemoved;

        public HiddenWordInGrid(TakeTwoClue clue, bool addLetter)
        {
            LetterAddedOrRemoved = clue.LetterRemoved;
            if (addLetter)
            {
                DisplayedWord = clue.ShorterWord;
                HiddenWord = clue.LongerWord;
            }
            else
            {
                DisplayedWord = clue.LongerWord;
                HiddenWord = clue.ShorterWord;
            }
        }
    }
}