using System.Collections.Generic;
using System.Text;

namespace WordPuzzles
{
    public class WordLadder
    {
        readonly WordRepository _repository = new WordRepository() {ExludeAdvancedWords = true};
        public string Solution { get; }
        public int Size { get; set; }

        public bool AllLettersPlaced
        {
            get
            {
                foreach (bool currentLetterPlaced in SolutionLettersAlreadyPlaced)
                {
                    if (!currentLetterPlaced) return false;
                }
                return true;
            }
        }

        public string RemainingUnplacedLetters
        {
            get
            {
                StringBuilder builder = new StringBuilder();
                for (int index = 0; index < Solution.Length; index++)
                {
                    if (SolutionLettersAlreadyPlaced[index]) continue;
                    builder.Append(Solution[index]);
                }
                return builder.ToString();
            }
        }

        public List<WordAndClue> Chain = new List<WordAndClue>();
        private bool[] SolutionLettersAlreadyPlaced;

        public WordLadder(string solution)
        {
            Solution = solution;
            Size = solution.Length;
            SolutionLettersAlreadyPlaced = new bool[Size];
        }

        public List<string> FindNextWordsInChain(string previousWord, int indexToReplace)
        {
            string patternToMatch = previousWord.Substring(0, indexToReplace) + "_" + previousWord.Substring(indexToReplace+1);

            List<string> wordsMatchingPattern = _repository.WordsMatchingPattern(patternToMatch);
            wordsMatchingPattern.Remove(previousWord);
            return wordsMatchingPattern;
        }

        public string DisplayChain()
        {
            StringBuilder builder = new StringBuilder();
            Chain.Reverse();
            foreach (var wordAndClue in Chain)
            {
                builder.AppendLine($"{wordAndClue.Clue} = {wordAndClue.Word}");
            }
            //reset 
            Chain.Reverse();

            return builder.ToString();
        }

        public string FormatHtmlForGoogle()
        {
            Chain.Reverse();
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("<html>");
            builder.AppendLine(@"<body>");
            builder.AppendLine("<!--StartFragment-->");
            builder.AppendLine(@"<table border=""1"">");
            foreach (var entry in Chain)
            {
                builder.AppendLine("\t<tr>");
                builder.AppendLine($"\t\t<td>{entry.Clue}</td>");
                for (int indexInWord = 0; indexInWord < entry.Word.Length; indexInWord++)
                {
                    if (indexInWord != entry.SolutionLetterIndexInWord)
                    {
                        builder.AppendLine("\t\t<td> </td>");
                    }
                    else
                    {
                        builder.AppendLine($"\t\t<td>{entry.SolutionLetterIndexInSolution +1}</td>");
                    }
                }
                builder.AppendLine("\t</tr>");
            }

            builder.AppendLine("</table>");
            builder.AppendLine(@"<table border=""1"">");
            builder.AppendLine("\t<tr>");
            builder.AppendLine($"\t\t<td>Solution</td>");
            for (int i = 0; i < Solution.Length; i++)
            {
                builder.AppendLine($"\t\t<td> </td>");
            }
            builder.AppendLine("\t</tr>");
            builder.AppendLine("\t<tr>");
            builder.AppendLine($"\t\t<td> </td>");
            for (int i = 0; i < Solution.Length; i++)
            {
                builder.AppendLine($"\t\t<td>{i+1}</td>");
            }
            builder.AppendLine("\t</tr>");
            builder.AppendLine("</table>");
            builder.AppendLine("<!--EndFragment-->");
            builder.AppendLine("</body>");
            builder.AppendLine("</html>");

            //reset 
            Chain.Reverse();
            return builder.ToString();
        }

        public bool AlreadyContains(string foundWord)
        {
            foreach (var entry in Chain)
            {
                if (entry.Word == foundWord)
                {
                    return true;
                }
            }

            return false;
        }

        public void AddToChain(string word, string clue)
        {
            var wordAndClue = new WordAndClue() {Word = word, Clue = clue};
            for (var indexOfLetterInSolution = 0; indexOfLetterInSolution < Solution.Length; indexOfLetterInSolution++)
            {
                char letter = Solution[indexOfLetterInSolution];
                if (SolutionLettersAlreadyPlaced[indexOfLetterInSolution]) continue;
                int indexOfLetterInWord = word.IndexOf(letter);
                if (indexOfLetterInWord < 0) continue;

                wordAndClue.SolutionLetter = letter;
                wordAndClue.SolutionLetterIndexInWord = indexOfLetterInWord;
                wordAndClue.SolutionLetterIndexInSolution = indexOfLetterInSolution;
                SolutionLettersAlreadyPlaced[indexOfLetterInSolution] = true;
                break; //Only place one letter.
            }

            Chain.Add(wordAndClue);
        }
    }

    public class WordAndClue
    {
        public string Word;
        public string Clue;
        public char SolutionLetter = char.MinValue;

        public int SolutionLetterIndexInWord = -1;
        public int SolutionLetterIndexInSolution = -1;
    }
}