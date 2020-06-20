using Newtonsoft.Json;

namespace WordPuzzles
{
    [JsonConverter(typeof(PuzzleConverter))]
    public interface IPuzzle
    {
        string FormatHtmlForGoogle(bool includeSolution = false, bool  isFragment = false);
    }
}