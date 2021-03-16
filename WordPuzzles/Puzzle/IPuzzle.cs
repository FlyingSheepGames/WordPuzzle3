using System.Collections.Generic;
using Newtonsoft.Json;
using WordPuzzles.Utility;

namespace WordPuzzles.Puzzle
{
    [JsonConverter(typeof(PuzzleConverter))]
    public interface IPuzzle
    {
        string FormatHtmlForGoogle(bool includeSolution = false, bool  isFragment = false);
        string Description { get;  }
        List<string> GetClues();
        void ReplaceClue(string clueToReplace, string newClue);
    }
}