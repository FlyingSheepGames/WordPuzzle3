using System;
using System.Text;
// ReSharper disable NonReadonlyMemberInGetHashCode
//TODO: Make this class immutable. 
namespace WordPuzzles
{
    public class LetterAndArrowCell
    {
        public char Letter;
        public int Number;
        public Direction Direction;

        public static LetterAndArrowCell EmptyCell => new LetterAndArrowCell() {Letter = ' ', Direction = Direction.Undefined, Number = 0};

        public override bool Equals(object obj)
        {
            LetterAndArrowCell cell = obj as LetterAndArrowCell;
            if (cell is null) return false;
            return Equals(cell);
        }

        protected bool Equals(LetterAndArrowCell other)
        {
            return Letter == other.Letter && Number == other.Number && Direction == other.Direction;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                // TODO: Make this class immutable.
                var hashCode = Letter.GetHashCode();
                hashCode = (hashCode * 397) ^ Number;
                hashCode = (hashCode * 397) ^ (int) Direction;
                return hashCode;
            }
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            //builder.Append(Letter);
            if (0 != Number)
            {
                builder.Append(" ");
                builder.Append(Math.Abs(Number));
            }

            switch (Direction)
            {
                case Direction.Down:
                    builder.Append("↓");
                    break;
                case Direction.Up:
                    builder.Append("↑");
                    break;
                case Direction.Right:
                    builder.Append("→");
                    break;
                case Direction.Left:
                    builder.Append("←");
                    break;
            }
            return builder.ToString();
        }
    }
}