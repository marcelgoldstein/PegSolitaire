using System;

namespace PegSolitaire
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var f = new Field(7);
            f.Print();
            
            f.Move(1, 3, Directions.Right);
            f.Print();
            f.Move(2, 5, Directions.Up);
            f.Print();
            f.Move(4, 4, Directions.Left);
            f.Print();
            f.Move(3, 2, Directions.Down);
            f.Print();
        }
    }
}
