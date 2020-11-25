using System;
using System.Collections.Generic;
using WordPuzzles.Utility;

namespace WordPuzzles.Puzzle
{
    public class PuzzlePyramid
    {
        private List<Person> _peopleBornInRange;
        public DateTime StartDate { get; set; }

        public List<Person> PeopleBornInRange
        {
            get
            {
                if (_peopleBornInRange == null)
                {
                    _peopleBornInRange = FindPeopleBornInRange(StartDate, StartDate.AddDays(7));
                }
                return _peopleBornInRange;
            }

        }

        public Person SelectedPerson { get; set; }
        public string SelectedQuote { get; set; }
        public string SelectedQuoteWithReplacedWords { get; set; }
        public IPuzzle PuzzleJ { get; set; }
        public List<string> WordsToReplace { get; set; }

        internal List<Person> FindPeopleBornInRange(DateTime startDate, DateTime endDate)
        {
            int daysToSearch = (endDate - startDate).Days;
            if (daysToSearch <= 0)
            {
                throw new ArgumentException("End date must be after Start Date.");
            }

            int MAX_DAYS_TO_SEARCH= 15;
            if (MAX_DAYS_TO_SEARCH < daysToSearch)
            {
                throw new ArgumentException($"End date is more than {daysToSearch} away from start date. Choose a smaller time interval.");
            }

            List<Person> peopleBornInRange = new List<Person>();
            BirthdayFinder birthdayFinder = new BirthdayFinder();
            for (int dayIndex = 0; dayIndex < daysToSearch; dayIndex++)
            {
                DateTime dayToSearch = startDate.AddDays(dayIndex);
                peopleBornInRange.AddRange(birthdayFinder.FindPeopleForDate(dayToSearch.Month, dayToSearch.Day));
            }
            peopleBornInRange.Sort(BirthdayFinder.SortPeopleByYear);
            return peopleBornInRange;
        }

    }
}