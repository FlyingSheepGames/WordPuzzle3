using System;
using System.Collections.Generic;

namespace WordPuzzles
{
    public class PhraseSegmentPuzzle
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
            foreach (int blockSize in CalculateBlockSizes(LineLength))
            {
                Blocks.Add(new Block()
                {
                    Width = blockSize
                });

            }
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
                blockToReturn.Fragments.AddRange(subString.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries));
            }
            return blockToReturn;
        }

        internal string GetSubString(string completePhrase, int lineLengthSoFar, int nextWidthToTake, int lineIndex,
            int singleLineLength)
        {
            int startIndex = lineLengthSoFar;
            startIndex += lineIndex * singleLineLength;
            return completePhrase.Substring(startIndex, nextWidthToTake);
        }

    }

    public class Block
    {
        public int Width { get; set; }
        public List<string> Lines { get; set; }
        public List<string> Fragments { get; set; }
    }
}