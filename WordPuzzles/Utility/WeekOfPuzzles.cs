using System;
using System.IO;
using System.Xml.Serialization;
using WordPuzzles.Puzzle;
using WordPuzzles.Puzzle.Legacy;

namespace WordPuzzles.Utility
{
    public class WeekOfPuzzles
    {
        public WordSquare MondayWordSquare;
        public VowelMovement TuesdayVowelMovement;
        public ALittleAlliteration WednesdayALittleAlliteration;
        public VowelMovement ThursdayVowelMovement;
        public ALittleAlliteration FridayALittleAlliteration;

        public string Theme { get; set; }

        [XmlElement(DataType = "date")]
        public DateTime MondayOfWeekPosted { get; set; }

        public string[] SelectedWords = { "", "", "", "", "" };
        private readonly XmlSerializer _xmlSerializer = new XmlSerializer(typeof(WeekOfPuzzles));

        public void Serialize(string fileName)
        {
            using (TextWriter writer = new StreamWriter(fileName))
            {
                _xmlSerializer.Serialize(writer, this);
            }
        }

        public void Deserialize(string fileName)
        {
            using (TextReader reader = new StreamReader(fileName))
            {
                WeekOfPuzzles deserializedPuzzles = _xmlSerializer.Deserialize(reader) as WeekOfPuzzles;
                if (deserializedPuzzles != null)
                {
                    MondayWordSquare = deserializedPuzzles.MondayWordSquare;
                    TuesdayVowelMovement = deserializedPuzzles.TuesdayVowelMovement;
                    WednesdayALittleAlliteration = deserializedPuzzles.WednesdayALittleAlliteration;
                    ThursdayVowelMovement = deserializedPuzzles.ThursdayVowelMovement;
                    FridayALittleAlliteration = deserializedPuzzles.FridayALittleAlliteration;
                    SelectedWords = deserializedPuzzles.SelectedWords;
                    Theme = deserializedPuzzles.Theme;
                    MondayOfWeekPosted = deserializedPuzzles.MondayOfWeekPosted;
                }
            }
        }

    }
}