using System.Collections.Generic;
using Newtonsoft.Json;
using WordPuzzles.Puzzle.Legacy;
using WordPuzzles.Utility;

namespace WordPuzzles.Puzzle
{
    [JsonConverter(typeof(PuzzleConverter))]
    public interface IPuzzle
    {
        string FormatHtmlForGoogle(bool includeSolution = false, bool  isFragment = false);
        string Description { get;  }
        WordPuzzleType Type { get; }
        string Solution { get; }
        List<string> GetClues();
        void ReplaceClue(string clueToReplace, string newClue);
    }
}