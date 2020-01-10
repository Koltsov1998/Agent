using System;
using System.Collections;
using System.Collections.Generic;
using Agent.Others;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agent.Models
{
    public class FieldNodes : IEnumerable<Node>
    {
        private FieldNodesEnumerator _enumerator;

        public FieldNodes(int width, int height)
        {
            var nodes = new Node[height, width];
            _enumerator = new FieldNodesEnumerator(nodes);
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    nodes[i, j] = new Node(j, i);
                }
            }
        }

        public Node GetNode(int x, int y)
        {
            return _enumerator.GetNode(x, y);
        }

        public Node GetNode(Point point)
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
            return _nodes[y, x];
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
        public Point Point { get; private set; }
        public NodeType NodeType { set; get; }

        public Node(int x, int y)
        {
            Point = new Point(x, y);
        }

        public Node Copy()
        {
            return new Node(this.Point.X, this.Point.Y)
            {
                NodeType = this.NodeType
            };
        }
    }

}
