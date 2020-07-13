using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WordPuzzles
{
    public class PhraseSegmentPuzzle :IPuzzle
    {
        public string Phrase { get; set; }
        public string Author { get; set; }
        public int CompleteLength { get; set; }

        public void PlacePhrase()
        {
            CompleteLength = Phrase.Length + Author.Length + 1;

            int distanceToNextMultipleOfFour = (CompleteLength % 4);
            if (distanceToNextMultipleOfFour == 0)
            {
                SpacesBeforeAuthor = 1;
            }
            else
            {
                SpacesBeforeAuthor = 5 - distanceToNextMultipleOfFour;
            }
            CompleteLength = Phrase.Length + Author.Length + SpacesBeforeAuthor;
            LineLength = CompleteLength / 4;
            Blocks = BreakPhraseIntoBlocks(CompletePhrase, CalculateBlockSizes(LineLength));
        }

        public int LineLength { get; set; }

        internal List<int> CalculateBlockSizes(int lineLength)
        {
            var blockSizes = new List<int>();
            int numberOfBlocks = lineLength / 4;
            for (int i = 0; i < numberOfBlocks; i++)
            {
                blockSizes.Add(4);
            }
            switch (lineLength% 4)
            {
                case 0:
                    break;
                case 1: //make second one size 5
                    blockSizes[1] = 5;
                    break;
                case 2: //make second one size 3, and add a 3 at the end.
                    blockSizes[1] = 3;
                    blockSizes.Add(3);
                    break;
                case 3:
                    blockSizes.Add(3);
                    break;
            }
            return blockSizes;
        }

        public List<Block> Blocks = new List<Block>();
        private HtmlGenerator _htmlGenerator = new HtmlGenerator();

        public int SpacesBeforeAuthor { get; set; }

        public string CompletePhrase
        {
            get
            {
                return Phrase + new string(' ', SpacesBeforeAuthor) + Author;
            }
        }

        public List<Block> BreakPhraseIntoBlocks(string completePhrase, List<int> blockWidths)
        {
            var blocksToReturn = new List<Block>();
            int lineLengthSoFar = 0;
            foreach (int blockWidth in blockWidths)
            {
                blocksToReturn.Add(CreateBlock(completePhrase, lineLengthSoFar, blockWidth));
                lineLengthSoFar += blockWidth;
            }
            return blocksToReturn;
        }

        private Block CreateBlock(string completePhrase, int lineLengthSoFar, int nextWidthToTake)
        {
            var blockToReturn = new Block();
            blockToReturn.Width = nextWidthToTake;
            blockToReturn.Lines = new List<string>();
            int singleLineLength = completePhrase.Length / 4;
            blockToReturn.Fragments = new List<string>();
            for (int lineIndex = 0; lineIndex < 4; lineIndex++)
            {
                var subString = GetSubString(completePhrase, lineLengthSoFar, nextWidthToTake, lineIndex, singleLineLength);
                blockToReturn.Lines.Add(subString);
                AddFragments(blockToReturn, subString);
            }
            return blockToReturn;
        }

        private static void AddFragments(Block blockToReturn, string subString)
        {
            var fragmentsWithoutSpaces  = subString.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
            foreach (var fragment in fragmentsWithoutSpaces)
            {
                StringBuilder fragmentToAdd = new StringBuilder();
                foreach (char character in fragment)
                {
                    if (char.IsLetter(character))
                    {
                        fragmentToAdd.Append( character);
                    }
                }
                blockToReturn.Fragments.Add(fragmentToAdd.ToString().ToUpperInvariant());
            }
        }

        internal string GetSubString(string completePhrase, int lineLengthSoFar, int nextWidthToTake, int lineIndex,
            int singleLineLength)
        {
            int startIndex = lineLengthSoFar;
            startIndex += lineIndex * singleLineLength;
            return completePhrase.Substring(startIndex, nextWidthToTake);
        }

        public string FormatHtmlForGoogle(bool includeSolution = false, bool isFragment = false)
        {
            StringBuilder builder = new StringBuilder();
            if (!isFragment)
            {
                _htmlGenerator.AppendHtmlHeader(builder);
            }

            builder.AppendLine("<table>");
            List<StringBuilder> htmlLineBuilders = new List<StringBuilder>();

            for (int i = 0; i < 5; i++)
            {
                htmlLineBuilders.Add(new StringBuilder());
            }

            foreach (var lineBuilder in htmlLineBuilders)
            {
                lineBuilder.AppendLine("<tr>");
            }
            foreach (var block in Blocks)
            {
                block.WriteToLineBuilders(htmlLineBuilders, includeSolution);
            }
            foreach (var lineBuilder in htmlLineBuilders)
            {
                lineBuilder.AppendLine("</tr>");
            }

            foreach (var lineBuilder in htmlLineBuilders)
            {
                builder.AppendLine(lineBuilder.ToString());
            }
            builder.AppendLine("</table>");
            if (!isFragment)
            {
                _htmlGenerator.AppendHtmlFooter(builder);
            }

            return builder.ToString();
        }

        public string Description => $"PhraseSegmentPuzzle for phrase {this.Phrase} ";
    }

    public class Block
    {
        public int Width { get; set; }
        public List<string> Lines { get; set; }
        public List<string> Fragments { get; set; }

        public void WriteToLineBuilders(List<StringBuilder> htmlLineBuilders, bool includeSolution)
        {

            for (var lineIndex = 0; lineIndex < Lines.Count; lineIndex++)
            {
                var line = Lines[lineIndex];
                for (var characterIndex = 0; characterIndex < line.Length; characterIndex++)
                {

                    char character = line[characterIndex];
                    var classAttribute = DetermineClassAttribute(lineIndex, characterIndex, 
                        line.Length - 1, 
                        character == ' ', 
                        (!char.IsLetter(character))
                        );

                    htmlLineBuilders[lineIndex].Append($@"<td width=""30"" {classAttribute}>");
                    if (includeSolution || (!char.IsLetter(character)))
                    {
                        htmlLineBuilders[lineIndex].Append(character);
                    }
                    htmlLineBuilders[lineIndex].Append("</td>");
                }

                htmlLineBuilders[lineIndex].AppendLine();
            }

            if (!includeSolution)
            {
                StringBuilder fragmentsLine = htmlLineBuilders[4];
                fragmentsLine.AppendLine($@"<td class=""normal"" colspan=""{Width}"">");
                fragmentsLine.AppendLine("<ul>");
                foreach (var fragment in Fragments)
                {
                    fragmentsLine.AppendLine($"<li>{fragment.ToUpperInvariant()}");
                }

                fragmentsLine.AppendLine("</ul>");
                fragmentsLine.AppendLine($@"</td>");
            }

        }

        private string DetermineClassAttribute(int lineIndex, int characterIndex, int lastLineIndex, bool blackOutCell, bool greyOutCell)
        {
            List<string> cssAttributes = new List<string>();
            cssAttributes.Add("centered");
            if (blackOutCell)
            {
                cssAttributes.Add("black");
            }
            else
            {
                if (greyOutCell)
                {
                    cssAttributes.Add("grey");
                }
            }

            string positionAttribute = "normal";

            if (lineIndex == 0)
            {
                positionAttribute += "-top";
            }

            if (lineIndex == (Lines.Count - 1))
            {
                positionAttribute += "-bottom";
            }

            if (characterIndex == 0)
            {
                positionAttribute += "-left";
            }

            if (characterIndex == (lastLineIndex))
            {
                positionAttribute += "-right";
            }
            cssAttributes.Add(positionAttribute);
            string classAttributeToReturn = $@"class=""{string.Join(" ", cssAttributes)}"" ";
            return classAttributeToReturn;
        }
    }
}