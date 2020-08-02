using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace WordPuzzles.Utility
{
    public class ClueRepository
    {
        public int CountOfWordWithClues => _clues.Count;

        private readonly Dictionary<string, List<Clue>> _clues = new Dictionary<string, List<Clue>>();

        public void AddClue(string word, string clue, ClueSource source=ClueSource.ClueSourceUnknown)
        {
            string canonicalWord = word.ToUpperInvariant();
            List<Clue> currentCluesForWord = null;
            if (_clues.ContainsKey(canonicalWord))
            {
                currentCluesForWord = _clues[canonicalWord];
            }

            if (currentCluesForWord == null)
            {
                currentCluesForWord = new List<Clue>();
            }

            if (! AlreadyContainsClue(currentCluesForWord, clue))
            {
                currentCluesForWord.Add( new Clue()
                {
                    ClueText =  clue, 
                    ClueSource = source,
                });
            }

            _clues[canonicalWord] = currentCluesForWord;
        }

        private bool AlreadyContainsClue(List<Clue> existingClues, string clueToAdd)
        {
            foreach (var clue in existingClues)
            {
                if (clue.ClueText.ToUpperInvariant() == clueToAdd.ToUpperInvariant())
                {
                    return true;
                }
            }

            return false;
        }

        public List<Clue> GetCluesForWord(string word)
        {
            string canonicalWord = word.ToUpperInvariant();
            if (_clues.ContainsKey(canonicalWord))
            {
                return _clues[canonicalWord];
            }
            return new List<Clue>();
        }

        public void WriteToDisk(string fileLocation)
        {
            File.WriteAllText(fileLocation, JsonConvert.SerializeObject(_clues));
        }

        // ReSharper disable StringLiteralTypo
        public void ReadFromDisk(string fileLocation = @"C:\Users\Chip\Source\Repos\WordPuzzle3\WordPuzzlesTest\data\PUZ\allclues.json")
            // ReSharper restore StringLiteralTypo
        {
            var cluesToAdd = JsonConvert.DeserializeObject<Dictionary<string, List<Clue>>>(File.ReadAllText(fileLocation));
            foreach (var clueKey in cluesToAdd.Keys)
            {
                var newClues = cluesToAdd[clueKey];
                foreach (var clue in newClues)
                {
                    AddClue(clueKey, clue.ClueText, clue.ClueSource);
                }
            }
        }
        public void ImportStackOverflowFormatFile(string fileLocation)
        {
            DataFromStackOverflowParser parser = new DataFromStackOverflowParser();
            var cluesToAdd = parser.ReadCluesFromFile(fileLocation);
            foreach (var clueKey in cluesToAdd.Keys)
            {
                var newClues = cluesToAdd[clueKey];
                foreach (var clue in newClues)
                {
                    AddClue(clueKey, clue.ClueText, clue.ClueSource);
                }
            }
        }

    }
}
