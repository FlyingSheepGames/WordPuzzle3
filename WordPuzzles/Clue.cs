using System.Xml.Serialization;

namespace WordPuzzles
{
    public class Clue
    {
        public string ClueText { get; set; }
        public ClueSource ClueSource { get; set; }
    }
}