using System;
using System.Collections.Generic;
using System.Linq;

namespace WordPuzzles
{
    public class Player
    {
        public Player(Dictionary<int, string> playerRow, PuzzleType[] puzzleTypeIndicies)
        {
            const int TWITTER_HANDLE_INDEX = 0;
            const int TOTAL_SCORE_INDEX = 2;

            if (!playerRow.ContainsKey(TWITTER_HANDLE_INDEX)) return;
            if (!playerRow.ContainsKey(TOTAL_SCORE_INDEX)) return;

            string twitterHandle = playerRow[TWITTER_HANDLE_INDEX];
            if (new[] {"Date", "Type", "Tweet URL"}.Contains(twitterHandle))
            {
                return;
            }
            TwitterHandle = twitterHandle;

            int totalScore;
            if (Int32.TryParse(playerRow[TOTAL_SCORE_INDEX], out totalScore))
            {
                TotalScore = totalScore;
            }

            int columnIndex = 3;
            foreach (PuzzleType puzzleType in puzzleTypeIndicies)
            {
                if (playerRow.ContainsKey(columnIndex))
                {
                    int currentPuzzleScore;
                    if (Int32.TryParse(playerRow[columnIndex], out currentPuzzleScore))
                    {
                        switch (puzzleType)
                        {
                            case PuzzleType.A_LITTLE_ALLITERATION:
                                ALittleAlliterationScore += currentPuzzleScore;
                                break;
                            case PuzzleType.MAGIC_WORD_SQUARE:
                                MagicWordSquareScore += currentPuzzleScore;
                                break;
                            case PuzzleType.VOWEL_MOVEMENT:
                                VowelMovementScore += currentPuzzleScore;
                                break;
                            default: 
                                throw new Exception($"Unexpected case {puzzleType} at column {columnIndex}.");
                        }
                    }
                }
                columnIndex++;
            }

            if (TotalScore != ALittleAlliterationScore + MagicWordSquareScore + VowelMovementScore)
            {
                throw new Exception($"Expected total score {TotalScore} to equal the sum of ALittleAlliterationScore ({ALittleAlliterationScore}), MagicWordSquareScore ({MagicWordSquareScore}) and VowelMovementScore ({VowelMovementScore}). ");
            }
        }
    

        public string TwitterHandle { get; set; }
        public int TotalScore { get; set; }
        public int ALittleAlliterationScore { get; set; }
        public int MagicWordSquareScore { get; set; }
        public int VowelMovementScore { get; set; }
        public bool AlreadyMentioned { get; set; }

        public static int SortByTotalScore(Player p, Player q)
        {
            return q.TotalScore - p.TotalScore;
        }

        public static int SortByALittleAlliterationScore(Player p, Player q)
        {
            return q.ALittleAlliterationScore - p.ALittleAlliterationScore;
        }

        public static int SortByMagicWordSquareScore(Player p, Player q)
        {
            return q.MagicWordSquareScore - p.MagicWordSquareScore;
        }
        public static int SortByVowelMovementScore(Player p, Player q)
        {
            return q.VowelMovementScore - p.VowelMovementScore;
        }

    }
}