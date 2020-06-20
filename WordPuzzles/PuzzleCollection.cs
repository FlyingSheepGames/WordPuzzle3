using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace WordPuzzles
{
    public class PuzzleCollection
    {
        List<IPuzzle> puzzles = new List<IPuzzle>();

        public long PuzzleCount => puzzles.Count;

        public void AddPuzzle(IPuzzle puzzleToAdd)
        {
            puzzles.Add(puzzleToAdd);
        }

        public IPuzzle RetrievePuzzleAtIndex(int i)
        {
            if (i < puzzles.Count)
            {
                return puzzles[i];
            }
            throw new ArgumentOutOfRangeException(nameof(i), "There aren't that many puzzles in the collection.");
        }

        public void Serialize(string fileName)
        {
            string serializedCollection = JsonConvert.SerializeObject(puzzles);
            File.WriteAllText(fileName, serializedCollection);
        }

        public void Deserialize(string fileName)
        {
            string serializedCollection = File.ReadAllText(fileName);
            puzzles = JsonConvert.DeserializeObject<List<IPuzzle>>(serializedCollection);
        }
    }
}