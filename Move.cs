using System;
using System.Collections.Generic;

namespace PegSolitaire
{
    public class Move : IEquatable<Move>
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Directions Direction { get; set; }
        public int Number { get; set; }
        public Move PreviousMove { get; set; }

        public Move()
        {

        }

        #region IEquatable
        public override bool Equals(object obj)
        {
            return this.Equals(obj as Move);
        }

        public bool Equals(Move other)
        {
            return other != null &&
                   this.X == other.X &&
                   this.Y == other.Y &&
                   this.Direction == other.Direction &&
                   this.Number == other.Number &&
                   EqualityComparer<Move>.Default.Equals(this.PreviousMove, other.PreviousMove);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.X, this.Y, this.Direction, this.PreviousMove);
        }

        public static bool operator ==(Move left, Move right)
        {
            return EqualityComparer<Move>.Default.Equals(left, right);
        }

        public static bool operator !=(Move left, Move right)
        {
            return !(left == right);
        }
        #endregion IEquatable

        public (int X, int Y) GetTargetPosition()
        {
            switch (this.Direction)
            {
                case Directions.Left:
                    return (this.X - 2, this.Y);
                case Directions.Up:
                    return (this.X, this.Y - 2);
                case Directions.Right:
                    return (this.X + 2, this.Y);
                case Directions.Down:
                    return (this.X, this.Y + 2);
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}