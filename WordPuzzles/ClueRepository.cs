using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;

namespace WordPuzzles
{
    public class ClueRepository
    {
        public int CountOfWordWithClues => Clues.Count;

        private Dictionary<string, List<NewClue>> Clues = new Dictionary<string, List<NewClue>>();

        public void AddClue(string word, string clue, ClueSource source=ClueSource.CLUE_SOURCE_UNKNOWN)
        {
            string canonicalWord = word.ToUpperInvariant();
            List<NewClue> currentCluesForWord = null;
            if (Clues.ContainsKey(canonicalWord))
            {
                currentCluesForWord = Clues[canonicalWord];
            }

            if (currentCluesForWord == null)
            {
                currentCluesForWord = new List<NewClue>();
            }

            if (! AlreadyContainsClue(currentCluesForWord, clue))
            {
                currentCluesForWord.Add( new NewClue()
                {
                    ClueText =  clue, 
                    ClueSource = source,
                });
            }

            Clues[canonicalWord] = currentCluesForWord;
        }

        private bool AlreadyContainsClue(List<NewClue> existingClues, string clueToAdd)
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

        public List<NewClue> GetCluesForWord(string word)
        {
            string canonicalWord = word.ToUpperInvariant();
            if (Clues.ContainsKey(canonicalWord))
            {
                return Clues[canonicalWord];
            }
            return new List<NewClue>();
        }

        public void WriteToDisk(string fileLocation)
        {
            File.WriteAllText(fileLocation, JsonConvert.SerializeObject(Clues));
        }

        public void ReadFromDisk(string fileLocation)
        {
            var CluesToAdd = JsonConvert.DeserializeObject<Dictionary<string, List<NewClue>>>(File.ReadAllText(fileLocation));
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

    public class NewClue
    {
        public string ClueText { get; set; }
        public ClueSource ClueSource { get; set; }
    }

    public enum ClueSource
    {
        CLUE_SOURCE_UNKNOWN = 0,
        CLUE_SOURCE_CHIP = 1,
        CLUE_SOURCE_CROSSWORD = 2,
    }
}
