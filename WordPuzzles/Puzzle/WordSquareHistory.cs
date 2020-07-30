using System;
using System.Collections.Generic;

namespace WordPuzzles.Puzzle
{
    public class WordSquareHistory
    {
        public void AddWordSquare(WordSquare wordSquare, DateTime dateTweeted)
        {
            foreach (string word in wordSquare.Lines)
            {
                int daysSinceLastUse = (DateTime.Now - dateTweeted).Days;
                if (!DaysSinceLastUse.ContainsKey(word))
                {
                    DaysSinceLastUse.Add(word, daysSinceLastUse);
                }
                else
                {
                    int previousDaysSinceLastUse = DaysSinceLastUse[word];
                    if (daysSinceLastUse < previousDaysSinceLastUse)
                    {
                        DaysSinceLastUse[word] = daysSinceLastUse;
                    }
                }
            }
        }

        public Dictionary<string, int> DaysSinceLastUse { get; set; } = new Dictionary<string, int>();

        public int CalculateScore(string word)
        {
            if (DaysSinceLastUse.ContainsKey(word))
            {
                return DaysSinceLastUse[word];
            }

            return 60;
        }

        public int CalculateScore(WordSquare wordSquare)
        {
            int total = 0;
            foreach (string usedWord in wordSquare.Lines)
            {
                total += CalculateScore(usedWord);
            }

            return total;
        }

    }
}