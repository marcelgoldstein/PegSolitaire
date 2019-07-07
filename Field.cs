using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PegSolitaire
{
    public enum Directions
    {
        Left,
        Up,
        Right,
        Down,
    }

    public class Field
    {
        private int size;
        private int center;
        private int margin;

        // Hält den Zustand des Feldes. null => nicht betretbar; false => leerstehend; true => gefüllt
        private bool?[,] field;

        public Field(int size)
        {
            if (size % 2 != 1)
            {
                throw new ArgumentException("The size must be an uneven number.");
            }

            this.size = size;

            this.ResetField();
        }

        public void ResetField()
        {
            this.field = new bool?[this.size, this.size];

            // erst mal alle mit true füllen
            for (int y = 0; y < this.size; y++)
            {
                for (int x = 0; x < this.size; x++)
                {
                    this.field[x, y] = true;
                }
            }

            this.center = (int)this.size / 2;
            this.margin = (this.size - center) / 2;

            // die ränder auf null setzen
            // links oben
            for (int y = 0; y < margin; y++)
            {
                for (int x = 0; x < margin; x++)
                {
                    this.field[x, y] = null;
                }
            }

            // rechts oben
            for (int y = 0; y < margin; y++)
            {
                for (int x = this.size - margin; x < this.size; x++)
                {
                    this.field[x, y] = null;
                }
            }

            // links unten
            for (int y = this.size - margin; y < this.size; y++)
            {
                for (int x = 0; x < margin; x++)
                {
                    this.field[x, y] = null;
                }
            }

            // rechts unten
            for (int y = this.size - margin; y < this.size; y++)
            {
                for (int x = this.size - margin; x < this.size; x++)
                {
                    this.field[x, y] = null;
                }
            }

            // die mitte auf null setzen
            this.field[center, center] = false;
        }

        public void Print(Move highlightMove = null)
        {
            if (Environment.GetCommandLineArgs().Select(a => a.ToUpper()).Contains("-print".ToUpper()) == false)
                return;

            (int X, int Y) startPos = default;
            (int X, int Y) jumpedPos = default;
            (int X, int Y) targetPos = default;
            if (highlightMove != null)
            {
                startPos = (highlightMove.X, highlightMove.Y);
                switch (highlightMove.Direction)
                {
                    case Directions.Left:
                        jumpedPos = (highlightMove.X - 1, highlightMove.Y);
                        targetPos = (highlightMove.X - 2, highlightMove.Y);
                        break;
                    case Directions.Up:
                        jumpedPos = (highlightMove.X, highlightMove.Y - 1);
                        targetPos = (highlightMove.X, highlightMove.Y - 2);
                        break;
                    case Directions.Right:
                        jumpedPos = (highlightMove.X + 1, highlightMove.Y);
                        targetPos = (highlightMove.X + 2, highlightMove.Y);
                        break;
                    case Directions.Down:
                        jumpedPos = (highlightMove.X, highlightMove.Y + 1);
                        targetPos = (highlightMove.X, highlightMove.Y + 2);
                        break;
                }
            }

            Console.WriteLine("_____________________________");
            Console.WriteLine();
            for (int y = 0; y < this.size; y++)
            {
                for (int x = 0; x < this.size; x++)
                {
                    if (x == startPos.X && y == startPos.Y)
                        Console.ForegroundColor = ConsoleColor.Red;
                    else if (x == jumpedPos.X && y == jumpedPos.Y)
                        Console.ForegroundColor = ConsoleColor.Red;
                    else if (x == targetPos.X && y == targetPos.Y)
                        Console.ForegroundColor = ConsoleColor.Green;

                    if (this.field[x, y] == null)
                        Console.Write("   ");
                    else
                        Console.Write($"[{((this.field[x, y] == true) ? "x" : " ")}]");

                    Console.ResetColor();
                }
                Console.WriteLine();
            }
            Console.WriteLine("_____________________________");
        }

        public bool CanMove(int x, int y, Directions direction)
        {
            if (this.field[x, y] != true)
                return false; // starting position is not set

            switch (direction)
            {
                case Directions.Left:
                    if (x < 2) // out of bound
                        return false;
                    else if ((y < this.margin || y > (this.size - 1) - this.margin) && x < (this.margin + 2))
                        return false; // out of bound
                    else if (this.field[x - 1, y] == true && this.field[x - 2, y] == false)
                        return true; // possible move
                    else
                        return false;
                case Directions.Up:
                    if (y < 2) // out of bound
                        return false;
                    else if ((x < this.margin || x > (this.size - 1) - this.margin) && y < (this.margin + 2))
                        return false; // out of bound
                    else if (this.field[x, y - 1] == true && this.field[x, y - 2] == false)
                        return true; // possible move
                    else
                        return false;
                case Directions.Right:
                    if (x > (this.size - 1) - 2) // out of bound
                        return false;
                    else if ((y < this.margin || y > (this.size - 1) - this.margin) && x > ((this.size - 1) - this.margin - 2))
                        return false; // out of bound
                    else if (this.field[x + 1, y] == true && this.field[x + 2, y] == false)
                        return true; // possible move
                    else
                        return false;
                case Directions.Down:
                    if (y > (this.size - 1) - 2) // out of bound
                        return false;
                    else if ((x < this.margin || x > (this.size - 1) - this.margin) && y > ((this.size - 1) - this.margin - 2))
                        return false; // out of bound
                    else if (this.field[x, y + 1] == true && this.field[x, y + 2] == false)
                        return true; // possible move
                    else
                        return false;
                default:
                    throw new InvalidOperationException();
            }
        }

        public void MakeMove(int x, int y, Directions direction)
        {
            this.field[x, y] = false; // clear starting position

            switch (direction)
            {
                case Directions.Left:
                    this.field[x - 2, y] = true; // set target position
                    this.field[x - 1, y] = false; // clear jumped position
                    break;
                case Directions.Up:
                    this.field[x, y - 2] = true; // set target position
                    this.field[x, y - 1] = false; // clear jumped position
                    break;
                case Directions.Right:
                    this.field[x + 2, y] = true; // set target position
                    this.field[x + 1, y] = false; // clear jumped position
                    break;
                case Directions.Down:
                    this.field[x, y + 2] = true; // set target position
                    this.field[x, y + 1] = false; // clear jumped position
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }

        public void RevertMove(int x, int y, Directions direction)
        {
            this.field[x, y] = true; // un-clear starting position

            switch (direction)
            {
                case Directions.Left:
                    this.field[x - 2, y] = false; // un-set target position
                    this.field[x - 1, y] = true; // un-clear jumped position
                    break;
                case Directions.Up:
                    this.field[x, y - 2] = false; // un-set target position
                    this.field[x, y - 1] = true; // un-clear jumped position
                    break;
                case Directions.Right:
                    this.field[x + 2, y] = false; // un-set target position
                    this.field[x + 1, y] = true; // un-clear jumped position
                    break;
                case Directions.Down:
                    this.field[x, y + 2] = false; // un-set target position
                    this.field[x, y + 1] = true; // un-clear jumped position
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }

        public List<MoveTracker> ProbePossibleMoves()
        {
            var mts = new List<MoveTracker>();
            var mt = new MoveTracker();
            mts.Add(mt);

            while (true)
            {
                if (this.MakeFirstPossibleMove(mts, mt) == false)
                {
                    if (mt.Moves.Any() == false)
                    {
                        mts.Remove(mt);
                        break; // no moves left to probe
                    }

                    this.Print();

                    var lastValidMove = mt.Moves.LastOrDefault();
                    if (mt.FreshClone == false)
                    {
                        mt = mt.Clone();
                        mts.Add(mt);
                        mt.Moves.Remove(lastValidMove);
                    }
                    this.RevertMove(lastValidMove.X, lastValidMove.Y, lastValidMove.Direction); // revert last move on current field
                }
                else
                {
                    var lastValidMove = mt.Moves.Last();
                    this.Print(lastValidMove);
                }
            }

            return mts;
        }

        public bool MakeFirstPossibleMove(List<MoveTracker> mts, MoveTracker mt)
        {
            for (int y = 0; y < this.size; y++)
            {
                for (int x = 0; x < this.size; x++)
                {
                    foreach (Directions direction in Enum.GetValues(typeof(Directions)))
                    {
                        var m = new Move { X = x, Y = y, Direction = direction, PreviousMove = mt.Moves.LastOrDefault(), Number = mt.Moves.Count + 1 };
                        if (this.CanMove(x, y, direction) && mts.Any(a => a.Moves.Any(b => b == m)) == false)
                        {
                            mt.Moves.Add(m);
                            this.MakeMove(x, y, direction);
                            return true;
                        }
                    }
                }
            }

            return false;
        }
    }
}