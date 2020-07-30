using System;
using System.Collections.Generic;

namespace WordPuzzles.Utility
{
    public class MonthlyScoreKeeper
    {
        readonly GoogleSheet _sheet = new GoogleSheet() {
            GoogleSheetKey = "1KiWsFIPCVmst--r57KEjDT-F5yuoeu-Ug9pqesuWyWA" ,
            IgnoreCache = true

        };
        public List<Player> GetPlayers(string month = null)
        {
            var players = new List<Player>();
            var results = _sheet.ExecuteQuery("SELECT *", month);
            var puzzleTypeIndicies = GetPuzzleTypeIndiciesFromTypeRowInSheet(results[1]);

            foreach (var playerRow in results)
            {
                Player playerToAdd = new Player(playerRow, puzzleTypeIndicies);
                if (!string.IsNullOrWhiteSpace(playerToAdd.TwitterHandle))
                {
                    players.Add(playerToAdd);
                }
            }

            return players;
        }

        private static PuzzleType[] GetPuzzleTypeIndiciesFromTypeRowInSheet(Dictionary<int, string> dictionary)
        {
            List<PuzzleType> listOfPuzzleTypes = new List<PuzzleType>();

            int currentColumnIndex = 3;
            while(dictionary.ContainsKey(currentColumnIndex))
            {
                string puzzleTypeAsString = dictionary[currentColumnIndex];

                listOfPuzzleTypes.Add(GetPuzzleTypeFromString(puzzleTypeAsString));
                currentColumnIndex++;
            }

            return listOfPuzzleTypes.ToArray();
        }

        private static PuzzleType GetPuzzleTypeFromString(string puzzleTypeAsString)
        {
            int puzzleTypeAsInt;
            if (int.TryParse(puzzleTypeAsString, out puzzleTypeAsInt))
            {
                return (PuzzleType) puzzleTypeAsInt;
            }

            throw new ArgumentException($"Unexpected puzzle type {puzzleTypeAsString}", nameof(puzzleTypeAsString));
        }
    }

    public enum PuzzleType
    {
        UNKNOWN = 0, 
        MAGIC_WORD_SQUARE = 1, 
        VOWEL_MOVEMENT = 2, 
        A_LITTLE_ALLITERATION = 3,

    }
}