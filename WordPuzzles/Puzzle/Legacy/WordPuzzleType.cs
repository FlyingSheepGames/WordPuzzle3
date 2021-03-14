namespace WordPuzzles.Puzzle.Legacy
{
    public enum WordPuzzleType
    {
        // ReSharper disable UnusedMember.Global
        Undefined,
        // ReSharper restore UnusedMember.Global
        WordSquare, 
        Sudoku, 
        Anacrostic, 
        WordLadder,
        LettersAndArrows,
        ReadDownColumn,
        HiddenRelatedWords,
        BuildingBlocks,
        RelatedWords,
        MissingLetters,
        PuzzleForDate,
        WordSearchMoreOrLess,
        MultipleClues,
        TrisectedWords,
        // ReSharper disable InconsistentNaming
        MAX_VALUE,
        // ReSharper restore InconsistentNaming

    }
}