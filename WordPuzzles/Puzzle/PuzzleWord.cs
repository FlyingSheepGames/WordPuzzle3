using System.Collections.Generic;

namespace WordPuzzles.Puzzle
{
    public class PuzzleWord
    {
        public List<PuzzleLetter> Letters = new List<PuzzleLetter>();
        private readonly string _originalWord;

        public PuzzleWord(string word, int startingIndex, char wordIndex)
        {
            if (word == null) return; //TODO: Figure out why this would be when deserializing. Use WelcomeHomeEve as an example.
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
