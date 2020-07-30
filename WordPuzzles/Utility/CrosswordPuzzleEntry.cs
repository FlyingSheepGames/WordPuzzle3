namespace WordPuzzles.Utility
{
    public class CrosswordPuzzleEntry
    {
        public string Word { get; set; }
        public bool IsCellNumbered { get; set; }
        public int ClueNumber { get; set; }
        public int IndexInSingleString { get; set; }
        public CrosswordDirection Direction { get; set; }

        public object SortNumber => (IndexInSingleString * 10) + (int) Direction;
    }
}