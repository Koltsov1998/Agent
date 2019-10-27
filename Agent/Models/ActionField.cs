using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Windows;
using Agent.Models.Exceprions;
using Point = Agent.Others.Point;

namespace Agent.Models
{
    public class ActionField : DependencyObject
    {
        public ActionField(string[] fieldPrototype)
        {
            Height = fieldPrototype.Length;
            Width = fieldPrototype[1].Length;
            Nodes = new FieldNodes(Width, Height);

            foreach (var fieldNode in Nodes)
            {
                var nodeLetter = fieldPrototype[fieldNode.Point.Y][fieldNode.Point.X];
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

                fieldNode.NodeType = nodeType;
            }
        }


        public ActionField(int height, int width)
        {
            Height = height;
            Width = width;
            Nodes = new FieldNodes(width, height);
        }

        public ActionField(int height, int width, int cookiesCount)
        {
            Height = height;
            Width = width;
            Nodes = new FieldNodes(width, height);
            InitializeFieldRandomly(cookiesCount);
        }

        public int Height { get; private set; }

        public int Width { get; private set; }

        public event FieldNodesChanged FieldNodesChangedEvent;

        public FieldNodes Nodes;

        public void UpdateFieldNodeType(Point point, NodeType newType)
        {
            Nodes.GetNode(point).NodeType = newType;
            FieldNodesChangedEvent?.Invoke(this);
        }

        private void InitializeFieldRandomly(int cookiesCount = 3)
        {
            var random = new Random();

            if (Nodes.Count() < cookiesCount + 2)
            {
                throw new NotEnouphNodesException();
            }

            foreach (var fieldNode in Nodes)
            {
                fieldNode.NodeType =
                    random.Next(4) == 0 ? NodeType.Rock : NodeType.Gross;
            }

            Nodes.Where(n => n.NodeType == NodeType.Gross)
                .Random(1)
                .Apply(n => n.NodeType = NodeType.Star);
            Nodes.Where(n => n.NodeType == NodeType.Gross)
                .Random(cookiesCount)
                .Apply(n => n.NodeType = NodeType.Cookie);
            Nodes.Where(n => n.NodeType == NodeType.Gross)
                .Random(1)
                .Apply(n => n.NodeType = NodeType.Agent);
        }

        public override string ToString()
        {
            var result = new StringBuilder();
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    var node = Nodes.GetNode(j, i).NodeType;

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

    public static class EnumerableExctensions
    {
        public static IEnumerable<T> Random<T>(this IEnumerable<T> collection, int count)
        {
            Random r = new Random();
            var arr = collection.ToList();
            for (int i = 0; i < count; i++)
            {
                var randomElement = arr[r.Next(arr.Count)];
                yield return randomElement;
                arr.Remove(randomElement);
            }
        }

        public static void Apply(this IEnumerable<Node> collection, Action<Node> action)
        {
            foreach (var c in collection)
            {
                action(c);
            }
        }
    }
}