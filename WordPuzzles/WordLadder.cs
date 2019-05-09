using System.Collections.Generic;
using System.Text;

namespace WordPuzzles
{
    public class WordLadder
    {
        readonly WordRepository repository = new WordRepository() {ExludeAdvancedWords = true};
        public string Solution { get; }
        public int Size { get; set; }

        public List<WordAndClue> Chain = new List<WordAndClue>();

        public WordLadder(string solution, string clue)
        {
            Solution = solution;
            Size = solution.Length;
            Chain.Add(new WordAndClue() {Word = solution, Clue = clue});
        }

        public List<string> FindNextWordsInChain(string previousWord, int indexToReplace)
        {
            string patternToMatch = previousWord.Substring(0, indexToReplace) + "_" + previousWord.Substring(indexToReplace+1);

            List<string> wordsMatchingPattern = repository.WordsMatchingPattern(patternToMatch);
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
            builder.AppendLine("<body>");
            builder.AppendLine("<table>");
            builder.AppendLine("<!--StartFragment-->");
            foreach (var entry in Chain)
            {
                builder.AppendLine("\t<tr>");
                builder.AppendLine($"\t\t<td>{entry.Clue}</td>");
                for (int i = 0; i < Size; i++)
                {
                    builder.AppendLine($"\t\t<td> </td>");
                }
                builder.AppendLine("\t</tr>");
            }

            builder.AppendLine("<!--EndFragment-->");
            builder.AppendLine("</table>");
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
    }

    public class WordAndClue
    {
        public string Word;
        public string Clue;
    }
}