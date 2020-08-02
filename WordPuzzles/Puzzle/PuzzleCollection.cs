using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace WordPuzzles.Puzzle
{
    public class PuzzleCollection : IEnumerable<IPuzzle>
    {
        List<IPuzzle> _puzzles = new List<IPuzzle>();

        public long PuzzleCount => _puzzles.Count;
        public string Name { get; set; }

        public void AddPuzzle(IPuzzle puzzleToAdd)
        {
            if (puzzleToAdd != null) _puzzles.Add(puzzleToAdd);
        }

        public IPuzzle RetrievePuzzleAtIndex(int i)
        {
            if (i < _puzzles.Count)
            {
                return _puzzles[i];
            }
            throw new ArgumentOutOfRangeException(nameof(i), "There aren't that many puzzles in the collection.");
        }

        public void Serialize(string fileName)
        {
            string serializedCollection = JsonConvert.SerializeObject(_puzzles);
            File.WriteAllText(fileName, serializedCollection);
        }

        public void Deserialize(string fileName)
        {
            string serializedCollection = File.ReadAllText(fileName);
            _puzzles = JsonConvert.DeserializeObject<List<IPuzzle>>(serializedCollection);
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
            return _puzzles.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void RemovePuzzleAtIndex(int indexToRemove)
        {
            if (indexToRemove >= 0 && _puzzles.Count > indexToRemove)
            {
                _puzzles.RemoveAt(indexToRemove);
            }
        }
    }
}