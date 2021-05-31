namespace WordPuzzleGenerator
{
    internal enum ProgramMode
    {
        UNDEFINED = 0, 
        COLLECTION = 1, //Work with a collection of puzzles
        YEAR = 2, //Work with a year of puzzles
        PATTERN_MATCH = 3, //Just get words that match a given pattern

        PUZZLE_PYRAMID = 4, //Create a pyramid of puzzles (12 in all)
        READ_DOWN_COLUMNS_RESEARCH,
        PUZZLE_PYRAMID_RERUN
    }
}