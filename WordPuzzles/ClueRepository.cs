using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;

namespace WordPuzzles
{
    public class ClueRepository
    {
        public int CountOfWordWithClues => Clues.Count;

        private Dictionary<string, List<Clue>> Clues = new Dictionary<string, List<Clue>>();

        public void AddClue(string word, string clue, ClueSource source=ClueSource.CLUE_SOURCE_UNKNOWN)
        {
            string canonicalWord = word.ToUpperInvariant();
            List<Clue> currentCluesForWord = null;
            if (Clues.ContainsKey(canonicalWord))
            {
                currentCluesForWord = Clues[canonicalWord];
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

            Clues[canonicalWord] = currentCluesForWord;
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
            if (Clues.ContainsKey(canonicalWord))
            {
                return Clues[canonicalWord];
            }
            return new List<Clue>();
        }

        public void WriteToDisk(string fileLocation)
        {
            File.WriteAllText(fileLocation, JsonConvert.SerializeObject(Clues));
        }

        public void ReadFromDisk(string fileLocation)
        {
            var CluesToAdd = JsonConvert.DeserializeObject<Dictionary<string, List<Clue>>>(File.ReadAllText(fileLocation));
            foreach (var clueKey in CluesToAdd.Keys)
            {
                var newClues = CluesToAdd[clueKey];
                foreach (var clue in newClues)
                {
                    AddClue(clueKey, clue.ClueText, clue.ClueSource);
                }
            }
        }
        public void ImportStackOverflowFormatFile(string fileLocation)
        {
            DataFromStackOverflowParser parser = new DataFromStackOverflowParser();
            var CluesToAdd = parser.ReadCluesFromFile(fileLocation);
            foreach (var clueKey in CluesToAdd.Keys)
            {
                var newClues = CluesToAdd[clueKey];
                foreach (var clue in newClues)
                {
                    AddClue(clueKey, clue.ClueText, clue.ClueSource);
                }
            }
        }

    }
}
