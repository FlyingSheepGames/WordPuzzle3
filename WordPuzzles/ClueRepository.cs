using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;

namespace WordPuzzles
{
    public class ClueRepository
    {
        public int ClueCount => Clues.Count;

        private Dictionary<string, List<string>> Clues = new Dictionary<string, List<string>>();

        public void AddClue(string word, string clue)
        {
            string canonicalWord = word.ToUpperInvariant();
            List<string> currentCluesForWord = null;
            if (Clues.ContainsKey(canonicalWord))
            {
                currentCluesForWord = Clues[canonicalWord];
            }

            if (currentCluesForWord == null)
            {
                currentCluesForWord = new List<string>();
            }

            if (!currentCluesForWord.Contains(clue))
            {
                currentCluesForWord.Add(clue);
            }

            Clues[canonicalWord] = currentCluesForWord;
        }

        public List<string> GetCluesForWord(string word)
        {
            string canonicalWord = word.ToUpperInvariant();
            if (Clues.ContainsKey(canonicalWord))
            {
                return Clues[canonicalWord];
            }
            return new List<string>();
        }

        public void WriteToDisk(string fileLocation)
        {
            File.WriteAllText(fileLocation, JsonConvert.SerializeObject(Clues));
        }

        public void ReadFromDisk(string fileLocation)
        {
            Clues = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(File.ReadAllText(fileLocation));
        }
    }
}
