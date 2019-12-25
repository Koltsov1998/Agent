using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crosses.Ai
{
    public class Bot
    {
        public Coordinate GetTurn(FieldNodes field)
        {
            return field.Where(node => node.State == NodeState.None)
                .Select(node => (node.GetMetrica(NodeState.Zero) + 0.5 * node.GetMetrica(NodeState.Cross), node))
                .OrderByDescending(p => p.Item1).First().node.Point;
        }
    }

    public class MovesGraph
    {
        private readonly Coordinate _coordinateMove;
        private const int width = 5;
        private const int depth = 2;

        public int MoveMetrica;
        public Coordinate MoveCoordinate;

        private MovesGraph(Coordinate coordinateMove)
        {
            _coordinateMove = coordinateMove;
        }

        private List<MovesGraph> AfterMoves = new List<MovesGraph>();

        public void Generate(int generationWave = 0)
        {

        }
    }
}
