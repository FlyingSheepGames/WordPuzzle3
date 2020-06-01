using System.Xml.Serialization;

namespace WordPuzzles
{
    public class NewClue
    {
        public string ClueText { get; set; }
        public ClueSource ClueSource { get; set; }
    }
}