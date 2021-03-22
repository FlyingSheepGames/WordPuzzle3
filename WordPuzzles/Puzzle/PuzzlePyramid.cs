using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using WordPuzzles.Utility;

namespace WordPuzzles.Puzzle
{
    public class PuzzlePyramid
    {
        private List<Person> _peopleBornInRange;
        public DateTime StartDate { get; set; }

        [JsonIgnore]
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
        public IPuzzle PuzzleA { get; set; }
        public IPuzzle PuzzleB { get; set; }
        public IPuzzle PuzzleC { get; set; }
        public IPuzzle PuzzleD { get; set; }
        public IPuzzle PuzzleE { get; set; }
        public IPuzzle PuzzleF { get; set; }
        public IPuzzle PuzzleG { get; set; }
        public IPuzzle PuzzleH { get; set; }
        public IPuzzle PuzzleI { get; set; }
        public IPuzzle PuzzleJ { get; set; }
        public IPuzzle PuzzleK { get; set; }
        public IPuzzle PuzzleL { get; set; }

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


        public static string ReplaceWordsWithMarkers(string selectedQuote, List<string> wordsToReplace)
        {
            var replaceWordsWithMarkers = selectedQuote;
            if (replaceWordsWithMarkers != null)
            {
                replaceWordsWithMarkers = replaceWordsWithMarkers.Replace(wordsToReplace[0], "(solve puzzle J)");
                replaceWordsWithMarkers = replaceWordsWithMarkers.Replace(wordsToReplace[1], "(solve puzzle K)");
                replaceWordsWithMarkers = replaceWordsWithMarkers.Replace(wordsToReplace[2], "(solve puzzle L)");
            }
            return replaceWordsWithMarkers;
        }
        public string FormatHtmlForGoogle(bool includeSolution = false, bool isFragment = false)
        {
            HtmlGenerator _generator = new HtmlGenerator();


            StringBuilder builder = new StringBuilder();
            _generator.AppendHtmlHeader(builder);

            builder.AppendLine($@"<h1>Puzzle Pyramid for { StartDate.ToShortDateString()} </h1>");
            builder.AppendLine($@"""{ReplaceWordsWithMarkers(SelectedQuote, WordsToReplace)}""");

            if (SelectedPerson != null)
            {
                builder.AppendLine($@"<P>The above quote is attributed to {SelectedPerson.Name}, who was born on {SelectedPerson.Month}/{SelectedPerson.Day}/{SelectedPerson.Year}.");
            }

            builder.AppendLine($@"<P>This is a puzzle pyramid. ");
            builder.AppendLine($@"<br>Solve puzzles A, B, and C to get the clues you need to solve puzzle J.");
            builder.AppendLine($@"<br>Solve puzzles D, E, and F to get the clues you need to solve puzzle K.");
            builder.AppendLine($@"<br>Solve puzzles G, H, and I to get the clues you need to solve puzzle L.");
            builder.AppendLine($@"<br>Finally, solve puzzles J, K, and L to find the missing words in the quote above.");

            _generator.AppendPageBreak(builder);

            if (PuzzleA != null)
            {
                builder.AppendLine($@"<h2>Puzzle A </h2>");
                builder.AppendLine(PuzzleA.FormatHtmlForGoogle(false, true));
                _generator.AppendPageBreak(builder);
            }

            if (PuzzleB != null)
            {
                builder.AppendLine($@"<h2>Puzzle B </h2>");
                builder.AppendLine(PuzzleB.FormatHtmlForGoogle(false, true));
                _generator.AppendPageBreak(builder);
            }
            if (PuzzleC != null)
            {
                builder.AppendLine($@"<h2>Puzzle C </h2>");
                builder.AppendLine(PuzzleC.FormatHtmlForGoogle(false, true));
                _generator.AppendPageBreak(builder);
            }
            if (PuzzleD != null)
            {
                builder.AppendLine($@"<h2>Puzzle D </h2>");
                builder.AppendLine(PuzzleD.FormatHtmlForGoogle(false, true));
                _generator.AppendPageBreak(builder);
            }
            if (PuzzleE != null)
            {
                builder.AppendLine($@"<h2>Puzzle E </h2>");
                builder.AppendLine(PuzzleE.FormatHtmlForGoogle(false, true));
                _generator.AppendPageBreak(builder);
            }
            if (PuzzleF != null)
            {
                builder.AppendLine($@"<h2>Puzzle F </h2>");
                builder.AppendLine(PuzzleF.FormatHtmlForGoogle(false, true));
                _generator.AppendPageBreak(builder);
            }
            if (PuzzleG != null)
            {
                builder.AppendLine($@"<h2>Puzzle G </h2>");
                builder.AppendLine(PuzzleG.FormatHtmlForGoogle(false, true));
                _generator.AppendPageBreak(builder);
            }
            if (PuzzleH != null)
            {
                builder.AppendLine($@"<h2>Puzzle H </h2>");
                builder.AppendLine(PuzzleH.FormatHtmlForGoogle(false, true));
                _generator.AppendPageBreak(builder);
            }
            if (PuzzleI != null)
            {
                builder.AppendLine($@"<h2>Puzzle I </h2>");
                builder.AppendLine(PuzzleI.FormatHtmlForGoogle(false, true));
                _generator.AppendPageBreak(builder);
            }
            if (PuzzleJ != null)
            {
                builder.AppendLine($@"<h2>Puzzle J </h2>");
                builder.AppendLine(PuzzleJ.FormatHtmlForGoogle(false, true));
                _generator.AppendPageBreak(builder);
            }
            if (PuzzleK != null)
            {
                builder.AppendLine($@"<h2>Puzzle K </h2>");
                builder.AppendLine(PuzzleK.FormatHtmlForGoogle(false, true));
                _generator.AppendPageBreak(builder);
            }
            if (PuzzleL != null)
            {
                builder.AppendLine($@"<h2>Puzzle L </h2>");
                builder.AppendLine(PuzzleL.FormatHtmlForGoogle(false, true));
                _generator.AppendPageBreak(builder);
            }

            _generator.AppendHtmlFooter(builder);

            return builder.ToString();
        }
    }
}