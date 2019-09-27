using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agent.Models;
using Agent.Others;

namespace Agent.Graphs.GraphCreators
{
    public class BfsGraphCreator
    {
        private Queue<Point> q;

        public GraphNode GenerateGraph(ActionField actionField)
        {
            Point sourcePoint = new Point(1, 1);
            for (int i = 0; i < actionField.Width; i++)
                for (int j = 0; j < actionField.Height; j++)
                {
                    if (actionField.FieldNodes[i, j] == NodeType.Agent)
                    {
                        sourcePoint = new Point(i, j);
                    }
                }

            var visited = new bool[actionField.Width, actionField.Height];
            visited[sourcePoint.X, sourcePoint.Y] = true;

            GraphNode sourceNode = new GraphNode(sourcePoint, null, NodeType.Agent);
            StartAppendingNodes(sourceNode, visited, actionField);
            return sourceNode;
        }

        public GraphNode GenerateGraph(ActionField actionField, Point startPoint)
        {
            var visited = new bool[actionField.Width, actionField.Height];
            visited[startPoint.X, startPoint.Y] = true;

            GraphNode sourceNode = new GraphNode(startPoint, null, actionField.FieldNodes.Get(startPoint));
            StartAppendingNodes(sourceNode, visited, actionField);
            return sourceNode;
        }

        private void StartAppendingNodes(GraphNode node, bool[,] visited, ActionField field)
        {
            Queue<GraphNode> q = new Queue<GraphNode>();
            q.Enqueue(node);

            while (q.Count > 0)
            {
                GraphNode currentNode = q.Dequeue();
                currentNode.ChildNodes = new List<GraphNode>();

                // right
                CheckNode(currentNode, 1, 0);

                // up
                CheckNode(currentNode, 0, -1);

                // left
                CheckNode(currentNode, -1, 0);

                // down
                CheckNode(currentNode, 0, 1);
            }

            void CheckNode(GraphNode currNode, int dx, int dy)
            {
                if (dx * dy != 0)
                {
                    throw new Exception("Route can't be diagonal'");
                }

                var newX = currNode.Point.X + dx;
                var newY = currNode.Point.Y + dy;

                if (newX >= 0 && newX < field.Width && newY >= 0 && newY < field.Height && !visited[newX, newY] && field.FieldNodes[newX, newY] != NodeType.Rock)
                {
                    var newNode = new GraphNode(new Point(newX, newY), currNode, field.FieldNodes[newX, newY]);
                    currNode.ChildNodes.Add(newNode);
                    visited[newX, newY] = true;
                    q.Enqueue(newNode);
                }
            }
        }
    }

    public static class VisitedArrayExctensios
    {
        public static bool Get(this bool[,] array, Point point)
        {
            return array[point.X, point.Y];
        }

        public static NodeType Get(this NodeType[,] array, Point point)
        {
            return array[point.X, point.Y];
        }
    }
}
