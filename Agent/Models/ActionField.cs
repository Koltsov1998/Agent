using System;
using System.Text;
using System.Windows;
using Point = Agent.Others.Point;

namespace Agent.Models
{
    public class ActionField : DependencyObject
    {
        public ActionField(string[] fieldPrototype)
        {
            Height = fieldPrototype.Length;
            Width = fieldPrototype[1].Length;
            FieldNodes = new NodeType[Height, Width];

            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    var nodeLetter = fieldPrototype[i][j];
                    var nodeType = NodeType.Gross;
                    switch (nodeLetter)
                    {
                        case ' ':
                            nodeType = NodeType.Gross;
                            break;
                        case 'A':
                            nodeType = NodeType.Agent;
                            break;
                        case 'F':
                            nodeType = NodeType.Star;
                            break;
                        case 'C':
                            nodeType = NodeType.Cookie;
                            break;
                        case '#':
                            nodeType = NodeType.Rock;
                            break;
                    }

                    this.FieldNodes[i, j] = nodeType;
                }
            }
        }


        public ActionField(int height, int width)
        {
            Height = height;
            Width = width;
            FieldNodes = new NodeType[height, width];
        }

        public ActionField(int height, int width, int cookiesCount)
        {
            Height = height;
            Width = width;
            FieldNodes = new NodeType[height, width];

            InitializeFieldRandomly(cookiesCount);
        }

        public int Height { get; private set; }

        public int Width { get; private set; }

        public NodeType[,] FieldNodes { get; private set; }

        public event FieldNodesChanged FieldNodesChangedEvent;

        public void UpdateFieldNodeType(Point point, NodeType newType)
        {
            FieldNodes[point.X, point.Y] = newType;
            FieldNodesChangedEvent?.Invoke(this);
        }

        private void InitializeFieldRandomly(int cookiesCount = 3)
        {
            var random = new Random();

            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    FieldNodes[i, j] =
                        random.Next(4) == 0 ? NodeType.Rock : NodeType.Gross;
                }
            }

            PutObjectsOnSomeNodes(NodeType.Star, 1);
            PutObjectsOnSomeNodes(NodeType.Cookie, cookiesCount);
            PutObjectsOnSomeNodes(NodeType.Agent, 1);
        }

        Random r = new Random();

        private void PutObjectsOnSomeNodes(NodeType objectType, int objectsLeft)
        {
            if (objectsLeft == 0)
            {
                return;
            }
            
            int randomX = r.Next(Width);
            int randomY = r.Next(Height);
            if (FieldNodes[randomX, randomY] == NodeType.Gross)
            {
                FieldNodes[randomX, randomY] = objectType;
                PutObjectsOnSomeNodes(objectType, objectsLeft - 1);
            }
            else
            {
                PutObjectsOnSomeNodes(objectType, objectsLeft);
            }
        }

        public override string ToString()
        {
            var result = new StringBuilder();
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    var node = FieldNodes[i, j];

                    char nodeLetter = ' ';
                    switch (node)
                    {
                        case NodeType.Agent:
                            nodeLetter = 'A';
                            break;
                        case NodeType.Cookie:
                            nodeLetter = 'C';
                            break;
                        case NodeType.Star:
                            nodeLetter = 'F';
                            break;
                        case NodeType.Rock:
                            nodeLetter = '#';
                            break;
                        case NodeType.Gross:
                            nodeLetter = ' ';
                            break;
                    }

                    result.Append(nodeLetter);
                }

                result.Append("\n");
            }

            return result.ToString();
        }

        public string StringRepresentation => this.ToString();
    }


    public delegate void FieldNodesChanged(ActionField af);
}