namespace WordPuzzles
{
    public interface IPuzzle
    {
        string FormatHtmlForGoogle(bool includeSolution = false, bool  isFragment = false);
    }
}