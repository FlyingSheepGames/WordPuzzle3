using System.Collections.Generic;
using System.Text;
using WordPuzzles.Puzzle.Legacy;
using WordPuzzles.Utility;

namespace WordPuzzles.Puzzle
{
    public class HiddenRelatedWordsPuzzle : IPuzzle
    {
        // ReSharper disable once UnusedMember.Global
        public bool IsHiddenRelatedWordsPuzzle = true; //For deserialization
        public List<HiddenWord> HiddenWords = new List<HiddenWord>();
        private HtmlGenerator _htmlGenerator = new HtmlGenerator();
        private const string INSTRUCTIONS = @"Find the word hiding in each of the sentences below. 
The hidden word will be made of consecutive letters in two or more adjacent words";

        public int CombinedKeyIndex
        {
            get
            {
                int maxKeyIndexSoFar = -1;
                foreach (var word in HiddenWords)
                {
                    if (maxKeyIndexSoFar < word.KeyIndex)
                    {
                        maxKeyIndexSoFar = word.KeyIndex;
                    }
                }

                return maxKeyIndexSoFar;
            }
        }

        public int CombinedLength
        {
            get
            {
                int maxLettersAfterIndex = -1;
                foreach (var word in HiddenWords)
                {
                    if (maxLettersAfterIndex < word.LettersAfterIndex)
                    {
                        maxLettersAfterIndex = word.LettersAfterIndex;
                    }
                }
                return maxLettersAfterIndex + CombinedKeyIndex + 1;
            }
        }

        public void AddWord(HiddenWord hiddenWordToAdd)
        {
            HiddenWords.Add(hiddenWordToAdd);
        }

        public string FormatHtmlForGoogle(bool includeSolution = false, bool isFragment = false)
        {
            var builder = new StringBuilder();
            _htmlGenerator = new HtmlGenerator();

            if (!isFragment)
            {
                _htmlGenerator.AppendHtmlHeader(builder);
            }

            builder.AppendLine(INSTRUCTIONS);
            StringBuilder tableBuilder = new StringBuilder();
            StringBuilder clueBuilder = new StringBuilder();
            tableBuilder.AppendLine("<table>");

            clueBuilder.AppendLine("<ol>");
            for (var index = 0; index < HiddenWords.Count; index++)
            {
                var currentWord = HiddenWords[index];
                tableBuilder.AppendLine("<tr>");
                tableBuilder.AppendLine($@"<td class=""normal centered"" width=""30""> {index+1} </td>");
                AppendHiddenWord(tableBuilder, currentWord, includeSolution);
                clueBuilder.AppendLine($"<li>{currentWord.SentenceHidingWord}");
            }

            tableBuilder.AppendLine("</table>");
            clueBuilder.AppendLine("</ol>");
            builder.Append(clueBuilder);
            builder.AppendLine("<br />");
            builder.Append(tableBuilder);
            if (!isFragment)
            {
                _htmlGenerator.AppendHtmlFooter(builder);
            }
            return builder.ToString();
        }

        private void AppendHiddenWord(StringBuilder builder, HiddenWord currentWord, bool includeSolution)
        {
            //Replaced by OL of sentences
            //builder.AppendLine($@"<td width=""250"" class=""normal"">{currentWord.SentenceHidingWord}</td>");
            int numberOfPrecedingEmptyCells = CombinedKeyIndex - currentWord.KeyIndex;
            string uppercaseCurrentWord = currentWord.Word.ToUpperInvariant();
            for (int index = 0; index < CombinedLength; index++)
            {
                if (index < numberOfPrecedingEmptyCells) // cells before word
                {
                    AppendCell(builder, @"hollow", @"&nbsp;");
                    continue;
                }
                if ((CombinedKeyIndex + currentWord.LettersAfterIndex) < index) //cells after word
                {
                    AppendCell(builder, @"hollow", @"&nbsp;");
                    continue;
                }

                string letterInWord = "&nbsp;";
                if (includeSolution)
                {
                    letterInWord = uppercaseCurrentWord[index - numberOfPrecedingEmptyCells].ToString();
                }

                string classAttributes = "normal centered";
                if (index == CombinedKeyIndex)
                {
                    classAttributes = "bold centered";
                }
                AppendCell(builder, classAttributes, letterInWord);

            }
            builder.AppendLine("</tr>");

        }

        private void AppendCell(StringBuilder builder, string classAttributes, string letterInWord)
        {
            builder.AppendLine(@"<td class=""" + classAttributes + @""" width=""30"">" + letterInWord + @"</td>");
        }

        public string Description => $"Hidden Related Words puzzle for {Solution}";
        public WordPuzzleType Type { get; } = WordPuzzleType.HiddenRelatedWords;

        public List<string> GetClues()
        {
            throw new System.NotImplementedException();
        }

        public void ReplaceClue(string clueToReplace, string newClue)
        {
            throw new System.NotImplementedException();
        }

        public string Solution { get; set; }
    }

    public class HiddenWord
    {
        public string Word { get; set; }
        public int KeyIndex { get; set; }

        public int LettersAfterIndex
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Word)) return -1;
                return Word.Length - (KeyIndex + 1);
            }
        }

        public string SentenceHidingWord { get; set; }
    }
}