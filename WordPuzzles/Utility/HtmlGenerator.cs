using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using WordPuzzles.Puzzle;

namespace WordPuzzles.Utility
{
    public class HtmlGenerator
    {
        public InnerAnacrosticPuzzle Puzzle;

        public Dictionary<int, int> IndexMap; 

        public string CreateComment()
        {
            if (IndexMap == null)
            {
                PopulateIndexMap();
            }

            StringBuilder builder = new StringBuilder();
            builder.AppendLine("/*");
            int letterIndex = 0;
            foreach (PuzzleWord word in Puzzle.Clues)
            {
                foreach (PuzzleLetter letter in word.Letters)
                {
                    builder.Append(letter.ActualLetter.ToString().ToUpperInvariant());
                    builder.Append($" {letterIndex} <->");
                    //if (letterIndex == 0)
                    {
                        int matchingLetterIndex = IndexMap[letterIndex]; 
                        builder.Append($" {matchingLetterIndex}");
                    }
                    builder.AppendLine();
                    letterIndex++;
                }
                builder.AppendLine("");
            }
            builder.AppendLine("-----");
            foreach (char letter in Puzzle.PhraseAsString)
            {
                {
                    if (letter == ' ')
                    {
                        builder.AppendLine(" ");
                    }
                    else
                    {
                        builder.Append(letter.ToString().ToUpperInvariant());
                        builder.Append($" {letterIndex} <->");
                        {
                            int matchingLetterIndex = IndexMap[letterIndex];
                            builder.Append($" {matchingLetterIndex}");
                        }
                        builder.AppendLine();
                        letterIndex++;
                    }
                }
            }

            builder.AppendLine("*/");
            return builder.ToString();
        }

        private void PopulateIndexMap()
        {
            int letterIndex = 0;
            foreach (PuzzleWord word in Puzzle.Clues)
            {
                foreach (PuzzleLetter letter in word.Letters)
                {
                    FindMatchingLetterIndex(letter, letterIndex++);
                }
            }
        }

        private void FindMatchingLetterIndex(PuzzleLetter letterToMatch, int letterIndex)
        {
            if (IndexMap != null)
            {
                if (IndexMap.ContainsKey(letterIndex))
                {
                    return;
                }
            }

            int puzzleLength = Puzzle.PhraseAsString.Length;
            int puzzleLengthWithoutBlanks = puzzleLength;
            int currentOffset = 0;
            int offsetOfMatch = 0;
            foreach (var letter in Puzzle.Phrase)
            {
                if (letter.ActualLetter == ' ' || Char.IsPunctuation(letter.ActualLetter))
                {
                    puzzleLengthWithoutBlanks--;
                }
                else
                {
                    if (letter == letterToMatch)
                    {
                        offsetOfMatch = currentOffset;
                    }
                    currentOffset++;
                }

            }

            var matchingLetterIndex = puzzleLengthWithoutBlanks + offsetOfMatch;
            if (IndexMap == null)
            {
                IndexMap = new Dictionary<int, int>(matchingLetterIndex*2);
            }

            IndexMap[letterIndex] = matchingLetterIndex;
            IndexMap[matchingLetterIndex] = letterIndex;
        }

        public string CreateIndexMapDefinition()
        {
            if (IndexMap == null)
            {
                PopulateIndexMap();
            }
            StringBuilder builder = new StringBuilder();
            builder.Append("var indexMap = [");
            int currentIndex = 0;
            while (IndexMap.ContainsKey(currentIndex))
            {
                builder.Append(IndexMap[currentIndex]);
                builder.Append(", ");
                currentIndex++;
            }

            builder.Remove(builder.Length - 2, 2);//remove last ", "
            builder.Append("];");

        return builder.ToString();
        }

        public string CreateTableRowForWord(PuzzleWord puzzleClue, int startingIndex)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("<tr>");
            builder.AppendLine("\t<td>");

            string puzzleClueCustomizedClue = puzzleClue.CustomizedClue;
            if (String.IsNullOrWhiteSpace(puzzleClueCustomizedClue))
            {
                puzzleClueCustomizedClue = $"Clue for {(string) puzzleClue}";
            }
            builder.AppendLine($"\t{puzzleClueCustomizedClue}:");

            builder.AppendLine("\t</td>");
            builder.AppendLine();
            builder.AppendLine("\t<td>");
            builder.Append("\t");
            foreach (var unused in puzzleClue.Letters)
            {
                builder.AppendLine($@"<input type=""text"" size=""1"" maxlength=""1"" id=""letter{startingIndex}"" onFocus=""colorMeAndMyMatch({startingIndex},'yellow');"" onBlur=""colorMeAndMyMatch({startingIndex},'white');""");
                startingIndex++;
                builder.Append("\t/>");
            }
            builder.AppendLine();
            builder.AppendLine("\t</td>");
            builder.AppendLine("</tr>");

            return builder.ToString();
        }

        public string CreateTableRowForPhrase()
        {
            int startingIndex = Puzzle.LettersInPhrase;
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("<tr>");
            builder.AppendLine("\t<td colspan=\"2\">");
            builder.Append("\t");
            string phrase = Puzzle.PhraseAsString;
            int letterIndex = startingIndex;
            int lettersSinceLastLineBreak = 0;
            bool lastCharacterWasALetter = false;
            for (int currentOffset = 0; currentOffset < Puzzle.PhraseAsString.Length; currentOffset++)
            {

                if (phrase[currentOffset] == ' ')
                {
                    if (15 < lettersSinceLastLineBreak)
                    {
                        builder.AppendLine();
                        builder.AppendLine("\t<br />&nbsp;<br />");
                        builder.Append("\t");
                        lettersSinceLastLineBreak = 0;
                    }
                    else
                    {
                        builder.AppendLine();
                        builder.AppendLine("\t&nbsp;");
                        builder.AppendLine("\t&nbsp;");
                        builder.Append("\t");
                    }
                }
                else
                {
                    if (Char.IsPunctuation(phrase[currentOffset]))
                    {
                        builder.AppendLine();
                        builder.AppendLine($"\t{phrase[currentOffset]}");
                        builder.Append("\t");
                        lastCharacterWasALetter = false;
                    }
                    else
                    {
                        builder.AppendLine(
                            $@"<input type=""text"" size=""1"" maxlength=""1"" id=""letter{letterIndex}"" onFocus=""colorMeAndMyMatch({letterIndex},'yellow');"" onBlur=""colorMeAndMyMatch({letterIndex},'white');"" ");
                        letterIndex++;
                        builder.Append("\t/>");
                        lastCharacterWasALetter = true;
                    }
                }

                lettersSinceLastLineBreak++;
            }

            if (lastCharacterWasALetter)
            {
                builder.Remove(builder.Length - 2, 2);
                builder.AppendLine("/>");
            }
            else
            {
                builder.Remove(builder.Length - 1, 1);
            }
            builder.AppendLine("\t</td>");
            builder.AppendLine("</tr>");
            return builder.ToString();
        }

        public string GenerateHtmlFile(string testPuzzleFilename, bool includeComment = true)
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine("<html>");
            builder.AppendLine("<head>");
            builder.AppendLine(@"<style> 
input {
    width: 20;
}
</style>");
            builder.AppendLine();
            builder.AppendLine(@"<script>

window.addEventListener('input', function (e) {

var currentIndex = Number(e.target.id.substring(6))
var nextIndex = currentIndex + 1;
var uppercaseLetter =  e.target.value.toUpperCase();
e.target.value = uppercaseLetter;

var matchingLetterElementId = ""letter"" + indexMap[currentIndex];
var matchingLetterElement = document.getElementById(matchingLetterElementId);
if (matchingLetterElement)
	{
	matchingLetterElement.value=uppercaseLetter;
	}

var nextIndexElementId = ""letter"" + nextIndex;

var nextElement = document.getElementById(nextIndexElementId);
if (nextElement)
	{
	nextElement.focus();
    nextElement.setSelectionRange(0,1);
	}
else {
e.target.blur();
}
}, false);

function colorMeAndMyMatch(currentIndex, color) {

    var letterElementId = ""letter"" + currentIndex;
    var letterElement = document.getElementById(letterElementId);
    letterElement.style.backgroundColor = color;

    var matchingLetterElementId = ""letter"" + indexMap[currentIndex];
    var matchingLetterElement = document.getElementById(matchingLetterElementId);
    matchingLetterElement.style.backgroundColor = color;

}
</script>
");
            builder.AppendLine("<script>");
            if (includeComment)
            {
                builder.Append(CreateComment());
            }

            builder.AppendLine(CreateIndexMapDefinition());
            builder.AppendLine();
            builder.AppendLine("</script>");
            builder.AppendLine();
            builder.AppendLine(@"</head>

<body>
<form>

<table border=""0"">");

            int startingIndex = 0;
            foreach (PuzzleWord word in Puzzle.Clues)
            {
                builder.Append(CreateTableRowForWord(word, startingIndex));
                startingIndex += word.Letters.Count;

            }

            builder.Append(CreateTableRowForPhrase());
            builder.AppendLine(@"</table>");

            builder.Append(@"</form>
</body>
");
            if (!String.IsNullOrWhiteSpace(TwitterUrl))
            {
                builder.AppendLine(
                    $@"Click here to confirm the <a href=""{TwitterUrl}"">solution</a> (spoiler warning).");
            }

            builder.Append(@"</html>");

            File.WriteAllText(testPuzzleFilename, builder.ToString());

            return builder.ToString();
        }

        public string TwitterUrl { get; set; }

        public void AppendHtmlFooter(StringBuilder builder)
        {
            builder.AppendLine("</body>");
            builder.AppendLine("</html>");
        }

        public void AppendHtmlHeader(StringBuilder builder)
        {
            builder.AppendLine("<html>");
            builder.AppendLine(@"
<head>
<style type=""text / css"">
        table td, table th {
            padding: 0
        }
       .bold {
            border-right-style: solid;
            border-bottom-color: #000000;
            border-top-width: 2.2pt;
            border-right-width: 2.2pt;
            border-left-color: #000000;
            vertical-align: top;
            border-right-color: #000000;
            border-left-width: 2.2pt;
            border-top-style: solid;
            border-left-style: solid;
            border-bottom-width: 2.2pt;
            border-top-color: #000000;
            border-bottom-style: solid
        }
        .normal {
            border-right-style: solid;
            border-bottom-color: #000000;
            border-top-width: 1pt;
            border-right-width: 1pt;
            border-left-color: #000000;
            vertical-align: top;
            border-right-color: #000000;
            border-left-width: 1pt;
            border-top-style: solid;
            border-left-style: solid;
            border-bottom-width: 1pt;
            border-top-color: #000000;
            border-bottom-style: solid
        }
        .normal-top {
            border-right-style: solid;
            border-bottom-color: #000000;
            border-top-width: 3pt;
            border-right-width: 1pt;
            border-left-color: #000000;
            vertical-align: top;
            border-right-color: #000000;
            border-left-width: 1pt;
            border-top-style: solid;
            border-left-style: solid;
            border-bottom-width: 1pt;
            border-top-color: #000000;
            border-bottom-style: solid
        }
        .normal-top-left {
            border-right-style: solid;
            border-bottom-color: #000000;
            border-top-width: 3pt;
            border-right-width: 1pt;
            border-left-color: #000000;
            vertical-align: top;
            border-right-color: #000000;
            border-left-width: 3pt;
            border-top-style: solid;
            border-left-style: solid;
            border-bottom-width: 1pt;
            border-top-color: #000000;
            border-bottom-style: solid
        }
        .normal-top-right {
            border-right-style: solid;
            border-bottom-color: #000000;
            border-top-width: 3pt;
            border-right-width: 3pt;
            border-left-color: #000000;
            vertical-align: top;
            border-right-color: #000000;
            border-left-width: 1pt;
            border-top-style: solid;
            border-left-style: solid;
            border-bottom-width: 1pt;
            border-top-color: #000000;
            border-bottom-style: solid
        }
        .normal-bottom {
            border-right-style: solid;
            border-bottom-color: #000000;
            border-top-width: 1pt;
            border-right-width: 1pt;
            border-left-color: #000000;
            vertical-align: top;
            border-right-color: #000000;
            border-left-width: 1pt;
            border-top-style: solid;
            border-left-style: solid;
            border-bottom-width: 3pt;
            border-top-color: #000000;
            border-bottom-style: solid
        }
        .normal-bottom-left {
            border-right-style: solid;
            border-bottom-color: #000000;
            border-top-width: 1pt;
            border-right-width: 1pt;
            border-left-color: #000000;
            vertical-align: top;
            border-right-color: #000000;
            border-left-width: 3pt;
            border-top-style: solid;
            border-left-style: solid;
            border-bottom-width: 3pt;
            border-top-color: #000000;
            border-bottom-style: solid
        }
        .normal-bottom-right {
            border-right-style: solid;
            border-bottom-color: #000000;
            border-top-width: 1pt;
            border-right-width: 3pt;
            border-left-color: #000000;
            vertical-align: top;
            border-right-color: #000000;
            border-left-width: 1pt;
            border-top-style: solid;
            border-left-style: solid;
            border-bottom-width: 3pt;
            border-top-color: #000000;
            border-bottom-style: solid
        }
        .normal-left {
            border-right-style: solid;
            border-bottom-color: #000000;
            border-top-width: 1pt;
            border-right-width: 1pt;
            border-left-color: #000000;
            vertical-align: top;
            border-right-color: #000000;
            border-left-width: 3pt;
            border-top-style: solid;
            border-left-style: solid;
            border-bottom-width: 1pt;
            border-top-color: #000000;
            border-bottom-style: solid
        }
        .normal-right {
            border-right-style: solid;
            border-bottom-color: #000000;
            border-top-width: 1pt;
            border-right-width: 3pt;
            border-left-color: #000000;
            vertical-align: top;
            border-right-color: #000000;
            border-left-width: 1pt;
            border-top-style: solid;
            border-left-style: solid;
            border-bottom-width: 1pt;
            border-top-color: #000000;
            border-bottom-style: solid
        }
        .hollow {
            border-right-style: solid;
            border-bottom-color: #000000;
            border-top-width: 0pt;
            border-left-color: #000000;
            vertical-align: top;
            border-right-color: #000000;
            border-top-style: solid;
            border-left-style: solid;
            border-bottom-width: 0pt;
            border-top-color: #000000;
            border-bottom-style: solid
        }

        .open {
            border-right-style: solid;
            border-bottom-color: #000000;
            border-top-width: 0pt;
            border-right-width: 0pt;
            border-left-color: #000000;
            vertical-align: top;
            border-right-color: #000000;
            border-left-width: 0pt;
            border-top-style: solid;
            border-left-style: solid;
            border-bottom-width: 0pt;
            border-top-color: #000000;
            border-bottom-style: solid
        }
    	.centered {
	        text-align: center
	    }
    	.black {
	        background-color:#000000
	    }
    	.grey {
	        background-color:#AAAAAA
	    }

</style>
</head>");
            builder.AppendLine("<body>");
        }

        public void AppendSolution(StringBuilder builder, string solutionToAppend, bool includeSolution)
        {
            builder.AppendLine("<p><h2>Solution</h2>");
            foreach (char character in solutionToAppend)
            {
                if (Char.IsLetter(character))
                {
                    if (includeSolution)
                    {
                        builder.Append("<u>");
                        builder.Append(Char.ToUpperInvariant(character));
                        builder.Append("</u> ");
                    }
                    else
                    {
                        builder.Append("_ ");
                    }
                }
                else
                {
                    if (Char.IsWhiteSpace(character))
                    {
                        builder.Append("<br>");
                    }
                    else
                    {
                        builder.Append(character);
                    }
                }
            }
        }
    }
}
