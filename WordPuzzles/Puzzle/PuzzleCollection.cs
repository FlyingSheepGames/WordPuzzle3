using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace WordPuzzles.Puzzle
{
    public class PuzzleCollection : IEnumerable<IPuzzle>
    {
        List<IPuzzle> puzzles = new List<IPuzzle>();

        public long PuzzleCount => puzzles.Count;
        public string Name { get; set; }

        public void AddPuzzle(IPuzzle puzzleToAdd)
        {
            if (puzzleToAdd != null) puzzles.Add(puzzleToAdd);
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
            Name = ParseNameFromFileName(fileName);
            
        }

        internal string ParseNameFromFileName(string fileName)
        {
            int indexOfLastSlash = fileName.LastIndexOf('\\');
            string name = "";
            if (0 < indexOfLastSlash)
            {
                name = fileName.Substring(indexOfLastSlash +1);
            }

            int indexOfDot = name.IndexOf('.');
            if (0 < indexOfDot)
            {
                name = name.Substring(0, indexOfDot);
            }
            return name;
        }

        public IEnumerator<IPuzzle> GetEnumerator()
        {
            return puzzles.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void RemovePuzzleAtIndex(int indexToRemove)
        {
            if (indexToRemove >= 0 && puzzles.Count > indexToRemove)
            {
                puzzles.RemoveAt(indexToRemove);
            }
        }
    }
}