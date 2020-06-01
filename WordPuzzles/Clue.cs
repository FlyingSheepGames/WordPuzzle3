using System;

namespace WordPuzzles
{
    [Serializable]
    public class Clue
    {
        public string Word { get; set; }
        public string Hint { get; set; }
    }
}