using System.Collections.Generic;

namespace WordPuzzles
{
    public class HiddenRelatedWordsPuzzle
    {
        List<HiddenWord> hiddenWords = new List<HiddenWord>();

        public int CombinedKeyIndex
        {
            get
            {
                int maxKeyIndexSoFar = -1;
                foreach (var word in hiddenWords)
                {
                    if (maxKeyIndexSoFar < word.KeyIndex)
                    {
                        maxKeyIndexSoFar = word.KeyIndex;
                    }
                }

                return maxKeyIndexSoFar;
            }
        }

        public int CombinedLength
        {
            get
            {
                int maxLettersAfterIndex = -1;
                foreach (var word in hiddenWords)
                {
                    if (maxLettersAfterIndex < word.LettersAfterIndex)
                    {
                        maxLettersAfterIndex = word.LettersAfterIndex;
                    }
                }

                return maxLettersAfterIndex + CombinedKeyIndex + 1;
            }
        }

        public void AddWord(HiddenWord hiddenWordToAdd)
        {
            hiddenWords.Add(hiddenWordToAdd);
        }
    }

    public class HiddenWord
    {
        public string Word { get; set; }
        public int KeyIndex { get; set; }

        public int LettersAfterIndex
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Word)) return -1;
                return Word.Length - (KeyIndex + 1);
            }
        }

        public string SentenceHidingWord { get; set; }
    }
}