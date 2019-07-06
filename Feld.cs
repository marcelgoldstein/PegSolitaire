using System;

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

    public void Print()
    {
        Console.WriteLine("_____________________________");
        Console.WriteLine();
        for (int y = 0; y < this.size; y++)
        {
            for (int x = 0; x < this.size; x++)
            {
                if (this.field[x, y] == null)
                    Console.Write("   ");
                else
                    Console.Write($"[{((this.field[x, y] == true) ? "x" : " ")}]");
            }
            Console.WriteLine();
        }
        Console.WriteLine("_____________________________");
    }

    public bool CanMove(int x, int y, Directions direction)
    {
        if (this.field[x, y] == null)
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

    public void Move(int x, int y, Directions direction)
    {
        if (this.CanMove(x, y, direction))
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
    }
}