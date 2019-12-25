using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Crosses.Ai;

namespace Crosses
{
    public class FieldNodes : IEnumerable<Node>
    {
        public int Width { get; }
        public int Height { get; }
        private FieldNodesEnumerator _enumerator;

        public FieldNodes(int width, int height)
        {
            Width = width;
            Height = height;
            var nodes = new Node[height, width];
            _enumerator = new FieldNodesEnumerator(nodes);
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    nodes[i, j] = new Node(j, i, this);
                }
            }
        }

        public Node GetNode(int x, int y)
        {
            return _enumerator.GetNode(x, y);
        }

        public Node GetNode(Coordinate point)
        {
            return _enumerator.GetNode(point.X, point.Y);
        }

        public IEnumerator<Node> GetEnumerator()
        {
            return _enumerator;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class FieldNodesEnumerator : IEnumerator<Node>
    {
        private Node[,] _nodes;
        private int _i;
        private int _j;

        public Node GetNode(int x, int y)
        {
            if (x >= 0 && x < _nodes.GetLength(0) && y >= 0 && y < _nodes.GetLength(1))
            {
                return _nodes[y, x];
            }

            return null;
        }

        public FieldNodesEnumerator(Node[,] nodes)
        {
            _nodes = nodes;
            _i = 0;
            _j = 0;
            Current = _nodes[_i, _j];
        }

        public void Dispose()
        {
            Console.WriteLine("Enumerator disposed");
        }

        public bool MoveNext()
        {
            if (_i == _nodes.GetLength(0))
            {
                Reset();
                return false;
            }

            Current = _nodes[_i, _j];
            _j++;
            if (_j == _nodes.GetLength(1))
            {
                _j = 0;
                _i++;
            }

            return true;
        }

        public void Reset()
        {
            _i = 0;
            _j = 0;
        }

        public Node Current { get; set; }

        object IEnumerator.Current => Current;
    }

    public class Node
    {
        private readonly FieldNodes _nodes;
        public Coordinate Point { get; private set; }
        public NodeState State { set; get; }

        public Node(int x, int y, FieldNodes nodes)
        {
            _nodes = nodes;
            Point = new Coordinate(x, y);
            State = NodeState.None;
        }

        public int GetMetrica(NodeState state)
        {
            int metricaRight = 0;
            int metricaTop = 0;
            int metricaTopRight = 0;
            int metricaLeft = 0;
            int metricaTopLeft = 0;
            int metricaDown = 0;
            int metricaDownLeft = 0;
            int metricaDownRight = 0;

            int radius = _nodes.Width > 4 ? 5 : _nodes.Width;
            for (int i = 1; i < radius; i++)
            {
                metricaRight += Metrica(1, 0);
                metricaTop += Metrica(0, -1);
                metricaTopRight += Metrica(1, -1);
                metricaLeft += Metrica(-1, 0);
                metricaTopLeft += Metrica(-1, -1);
                metricaDown += Metrica(0, 1);
                metricaDownLeft += Metrica(-1, 1);
                metricaDownRight += Metrica(1, 1);
            }

            return new int[]
            {
                metricaRight,
                metricaTop,
                metricaTopRight,
                metricaLeft,
                metricaTopLeft,
                metricaDown,
                metricaDownLeft,
                metricaDownRight,
            }.Max();

            int Metrica(int dx, int dy)
            {
                var node = _nodes.GetNode(Point.X + dx, Point.Y + dy);
                if (node == null)
                {
                    return 0;
                }

                if (node.State == state)
                {
                    return 10;
                }

                return 0;
            }
        }
    }

    public enum NodeState
    {
        None, 
        Cross,
        Zero
    }

    public class Game
    {
        private FieldNodes _nodes;
        private Bot _bot;

        public Game(Size fieldSize)
        {
            _bot = new Bot();
            FieldSize = fieldSize;
            MyTurn = true;
            _nodes = new FieldNodes(fieldSize.Width, fieldSize.Height);

        }

        public bool MyTurn { set; get; }
        public Size FieldSize { get; }

        public void MakeTurn(Coordinate coordinate)
        {
            _nodes.GetNode(coordinate).State = NodeState.Cross;
        }

        public Coordinate WaitForEnemyTurn()
        {
            var coordinate = _bot.GetTurn(this._nodes);
            _nodes.GetNode(coordinate).State = NodeState.Zero;
            return coordinate;
        }

        public bool GameEnded()
        {

            return false;
        }
    }

    public class Coordinate
    {
        public int X { get; }
        public int Y { get; }

        public Coordinate(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
