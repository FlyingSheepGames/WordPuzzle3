using System;
using System.Collections.Generic;
using System.Text;
using WordPuzzles.Utility;

namespace WordPuzzles.Puzzle
{
    public class PhraseSegmentPuzzle :IPuzzle
    {
        // ReSharper disable once UnusedMember.Global
        public bool IsPhraseSegmentPuzzle = true;//Used when deserializing.
        public string Phrase { get; set; }
        public string Author { get; set; } = "Anonymous";

        public DateTime AuthorBirthday { get; set; }

        public int CompleteLength { get; set; }

        private Random _randomNumberGenerator;


        public int RandomSeed;
        
        public void PlacePhrase()
        {
            if (CompletePhrase.Length < 100)
            {
                PlaceShortPhrase();
            }
            else
            {
                SubPuzzles = new List<PhraseSegmentPuzzle>();
                var list = BreakLongPhraseIntoSubPhrases();
                for (var index = 0; index < list.Count; index++)
                {
                    var subPhrase = list[index];
                    string author = "";
                    if (index == list.Count - 1)
                    {
                        author = Author;//only append author to the last one.
                    }

                    var subPuzzle = new PhraseSegmentPuzzle()
                    {
                        Phrase = subPhrase,
                        Author = author, 
                        RandomSeed = RandomSeed
                    };
                    subPuzzle.PlacePhrase();
                    SubPuzzles.Add(subPuzzle);
                }
            }
        }

        private void PlaceShortPhrase()
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
        private readonly HtmlGenerator _htmlGenerator = new HtmlGenerator();
        public List<PhraseSegmentPuzzle> SubPuzzles;

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
            var blockToReturn = new Block {Width = nextWidthToTake, Lines = new List<string>()};
            int singleLineLength = completePhrase.Length / 4;
            blockToReturn.Fragments = new List<string>();
            for (int lineIndex = 0; lineIndex < 4; lineIndex++)
            {
                var subString = GetSubString(completePhrase, lineLengthSoFar, nextWidthToTake, lineIndex, singleLineLength);
                blockToReturn.Lines.Add(subString);
                AddFragments(blockToReturn, subString);
            }
            blockToReturn.Fragments.Shuffle(RandomNumberGenerator);
            return blockToReturn;
        }

        internal static void AddFragments(Block blockToReturn, string subString)
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
                    else
                    {
                        AddExtractedFragment(blockToReturn, fragmentToAdd);
                        fragmentToAdd.Clear();
                    }
                }
                AddExtractedFragment(blockToReturn, fragmentToAdd);
            }

        }

        private static void AddExtractedFragment(Block blockToReturn, StringBuilder fragmentToAdd)
        {
            if (fragmentToAdd.Length == 0) return;
            blockToReturn.Fragments.Add(fragmentToAdd.ToString().ToUpperInvariant());
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

            builder.AppendLine(GetInstructions());

            if (SubPuzzles == null)
            {
                AppendPuzzleTable(includeSolution, builder);
            }
            else
            {
                foreach (var subPuzzle in SubPuzzles)
                {
                    subPuzzle.AppendPuzzleTable(includeSolution, builder);
                }
            }

            if (!isFragment)
            {
                _htmlGenerator.AppendHtmlFooter(builder);
            }

            return builder.ToString();
        }

        private string GetInstructions()
        {
            string instructions = $@"Oh no! The letters have fallen out of the grid below in consecutive clumps. 
Return the letters into the grid above them. The author of the quote appears at the end. ";
            if (AuthorBirthday != default)
            {
                instructions += $@"Note that the source of this quote was born on {AuthorBirthday.ToLongDateString()}.";
            }
            return instructions;
        }

        private void AppendPuzzleTable(bool includeSolution, StringBuilder builder)
        {
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
        }

        public string Description => $"PhraseSegmentPuzzle for phrase {Phrase} ";
        public List<string> GetClues()
        {
            throw new NotImplementedException();
        }

        public void ReplaceClue(string clueToReplace, string newClue)
        {
            throw new NotImplementedException();
        }

        public Random RandomNumberGenerator
        {
            get
            {
                if (_randomNumberGenerator == null)
                {
                    if (RandomSeed == 0)
                    {
                        _randomNumberGenerator = new Random();
                    }
                    else
                    {
                        _randomNumberGenerator = new Random(RandomSeed);
                    }
                }
                return _randomNumberGenerator;
            }
        }

        public List<string> BreakLongPhraseIntoSubPhrases()
        {
            var subPhrases = new List<string>();
            StringBuilder builder = new StringBuilder();
            int cumulativeTotal = 0;
            foreach (var word in Phrase.Split(' '))
            {
                cumulativeTotal += word.Length + 1;
                int phraseLengthLimit = CalculatePhraseLengthLimit(CompletePhrase.Length);
                if (phraseLengthLimit < cumulativeTotal)
                {
                    //add this.
                    string subPhraseToAdd = builder.ToString();
                    if (subPhraseToAdd.EndsWith(" "))
                    {
                        subPhraseToAdd = subPhraseToAdd.Substring(0, subPhraseToAdd.Length - 1);
                    }
                    subPhrases.Add(subPhraseToAdd);

                    builder.Clear();
                    cumulativeTotal = word.Length + 1;
                }

                builder.Append(word);
                builder.Append(' ');
            }
            //add last one
            //add this.
            string lastPhraseToAdd = builder.ToString();
            if (lastPhraseToAdd.EndsWith(" "))
            {
                lastPhraseToAdd = lastPhraseToAdd.Substring(0, lastPhraseToAdd.Length - 1);
            }
            subPhrases.Add(lastPhraseToAdd);

            return subPhrases;
        }

        private int CalculatePhraseLengthLimit(int phraseLength)
        {
            int numberOfDesiredPhrases = (phraseLength / 100) + 1;
            return phraseLength / numberOfDesiredPhrases;
        }
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
                fragmentsLine.AppendLine(@"</td>");
            }

        }

        private string DetermineClassAttribute(int lineIndex, int characterIndex, int lastLineIndex, bool blackOutCell, bool greyOutCell)
        {
            List<string> cssAttributes = new List<string> {"centered"};
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