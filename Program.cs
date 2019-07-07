using System;
using System.Linq;

namespace PegSolitaire
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var f = new Field(7);

            var mts = f.ProbePossibleMoves();

            var bestMts = mts.Where(a => a.Moves.Count == mts.Max(b => b.Moves.Count)).ToList();

            // todo: print best restult/s
            var superBestMts = bestMts.Where(a => a.Moves.Last().GetTargetPosition() == (3, 3)).ToList();

            Console.WriteLine($"Found {bestMts.Count} bestMoveTrackers.");
            Console.WriteLine($"Found {superBestMts.Count} superBestMoveTrackers.");
        }
    }
}
