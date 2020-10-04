using System;
using System.Collections.Generic;
using System.Text;
using WordPuzzles.Utility;

namespace WordPuzzles.Puzzle
{
    public class MultipleCluesPuzzle
    {
        private Random _randomNumberGenerator;
        public List<WordWithClues> WordsWithClues = new List<WordWithClues>();
        internal int NextClueOrder = 1;

        public WordRepository Repository { get; set; } = new WordRepository();

        public int RandomGeneratorSeed { get; set; } = 0;
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

        public List<string> GetCandidatesForLetter(char secondLetter)
        {
            var candidatesForLetter = new List<string>();
            StringBuilder builder = new StringBuilder();
            for (int wordLength = 2; wordLength <= 7; wordLength++)
            {
                builder.Clear();
                builder.Append('_');
                builder.Append(secondLetter);
                builder.Append('_', wordLength - 2);
                candidatesForLetter.AddRange(
                    Repository.WordsMatchingPattern(builder.ToString()));

            }
            return candidatesForLetter;
        }

        public void AddWordWithClues(string wordToAdd, List<string> clues)
        {
            WordWithClues wordWithClues = new WordWithClues()
            {
                WordText = wordToAdd,
                Clues = new List<OrderedClue>(),
            };
            foreach (var clue in clues)
            {
                wordWithClues.Clues.Add(new OrderedClue()
                {
                    ClueText = clue, 
                    ClueOrder = NextClueOrder++,
                });   
            }
            WordsWithClues.Add(wordWithClues);
        }

        public void ReorderClues(int stackCounter = 0)
        {
            if (3 < stackCounter)
            {
                throw new Exception("Tried to reorder clues at least 3 times.");
            }
            //Swap the order of a clue from two different words. Do this 5 times. 
            if (WordsWithClues.Count < 3) return;

            for (int swapCount = 0; swapCount < 6; swapCount++)
            {
                int firstWordIndex = RandomNumberGenerator.Next(WordsWithClues.Count);
                int secondWordIndex = RandomNumberGenerator.Next(WordsWithClues.Count);
                if (firstWordIndex == secondWordIndex) continue;
                var firstWord = WordsWithClues[firstWordIndex];
                var secondWord = WordsWithClues[secondWordIndex];
                firstWord.Clues.Shuffle(RandomNumberGenerator);
                secondWord.Clues.Shuffle(RandomNumberGenerator);

                int firstWordClueOrder = firstWord.Clues[0].ClueOrder;
                firstWord.Clues[0].ClueOrder = secondWord.Clues[0].ClueOrder;
                secondWord.Clues[0].ClueOrder = firstWordClueOrder;
            }
            //a few checks
            List<int> uniqueSums = new List<int>();
            bool allSumsUnique = true;
            foreach (var word in WordsWithClues)
            {
                if (uniqueSums.Contains(word.SumOfClueOrders))
                {
                    allSumsUnique = false;
                    break;
                }
                uniqueSums.Add(word.SumOfClueOrders);
            }

            if (!allSumsUnique)
            {
                ReorderClues(stackCounter+1);
            }
        }
    }

    public class OrderedClue
    {
        public string ClueText { get; set; }
        public int ClueOrder { get; set; }
    }

    public class WordWithClues
    {
        public string WordText { get; set; }
        public List<OrderedClue> Clues { get; set; }
        public int SumOfClueOrders
        {
            get
            {
                int sum = 0;
                foreach (var clue in Clues)
                {
                    sum += clue.ClueOrder;
                }

                return sum;
            }
        }
    }
}