using System.Collections.Generic;

namespace PegSolitaire
{
    public class MoveTracker
    {
        public bool FreshClone { get; set; }
        public List<Move> Moves { get; set; } = new List<Move>();

        public MoveTracker Clone()
        {
            var mt = new MoveTracker();
            mt.Moves.AddRange(this.Moves);
            mt.FreshClone = true;

            return mt;
        }
    }
}