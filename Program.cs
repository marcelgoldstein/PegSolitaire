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
            f.Move(2, 5, Directions.Up);
            f.Move(4, 4, Directions.Left);
            f.Move(3, 2, Directions.Down);
            f.RevertMove(3, 2, Directions.Down);
            f.RevertMove(4, 4, Directions.Left);
            f.RevertMove(2, 5, Directions.Up);
            f.RevertMove(1, 3, Directions.Right);


            f.Print();


        }
    }
}
