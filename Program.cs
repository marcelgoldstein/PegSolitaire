using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PegSolitaire
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var f = new Field(7);

            var cts = new CancellationTokenSource();
            cts.CancelAfter(TimeSpan.FromSeconds(600));
            
            var mts = new List<MoveTracker>();

            Task.Run(async () =>
            {
                mts = await f.ProbePossibleMoves(cts.Token);

            }).GetAwaiter().GetResult();;

            // check if no dups in results
            if (mts.GroupBy(a => a.Moves.Last()).Count() != mts.Count)
                throw new Exception("Some probing paths resulted in the same moves, this should not happen!");

            var bestMts = mts.Where(a => a.Moves.Count == mts.Max(b => b.Moves.Count)).ToList();

            var superBestMts = bestMts.Where(a => a.Moves.Last().GetTargetPosition() == (3, 3)).ToList();

            Console.WriteLine($"Found {bestMts.Count} bestMoveTrackers.");
            Console.WriteLine($"Found {superBestMts.Count} superBestMoveTrackers.");
        }
    }
}
