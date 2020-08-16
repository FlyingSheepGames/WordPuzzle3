using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using WordPuzzles.Puzzle;

namespace WordPuzzles
{
    public class YearOfPuzzles
    {
        Dictionary<DateTime, IPuzzle> _dictionary = new Dictionary<DateTime, IPuzzle>();

        public double Count
        {
            get
            {
                return _dictionary.Count;
            }
        }

        public void Add(IPuzzle puzzle, DateTime dateTime)
        {
            if (_dictionary.ContainsKey(dateTime))
            {
                throw new ArgumentException("There's already a puzzle for that date. Delete it first.");
            }
            _dictionary.Add(dateTime, puzzle);
        }

        public void Serialize(string fileName)
        {
            string serializedCollection = JsonConvert.SerializeObject(_dictionary);
            File.WriteAllText(fileName, serializedCollection);
        }

        public void Deserialize(string fileName)
        {
            string serializedCollection = File.ReadAllText(fileName);
            _dictionary = JsonConvert.DeserializeObject<Dictionary<DateTime, IPuzzle>>(serializedCollection);
        }

        public IPuzzle Retrieve(DateTime dateToRetrieve)
        {
            return _dictionary[dateToRetrieve];
        }
    }
}