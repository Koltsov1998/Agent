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

        private void StartAppendingNodes(GraphNode node, bool[,] visited, ActionField field)
        {
            Queue<GraphNode> q = new Queue<GraphNode>();
            q.Enqueue(node);

            while (q.Count > 0)
            {
                GraphNode currentNode = q.Dequeue();
                currentNode.ChildNodes = new List<GraphNode>();
                Point currentPoint = currentNode.Point;

                // right
                var rightPoint = new Point(currentPoint.X + 1, currentPoint.Y);
                if (currentPoint.X + 1 < visited.GetLength(0) && !visited.Get(rightPoint) && field.FieldNodes.Get(rightPoint) != NodeType.Rock)
                {
                    var rightNode = new GraphNode(rightPoint, node, field.FieldNodes[currentPoint.X + 1, currentPoint.Y]);
                    currentNode.ChildNodes.Add(rightNode);
                    visited[currentPoint.X + 1, currentPoint.Y] = true;
                    q.Enqueue(rightNode);
                }
                // up
                var upPoint = new Point(currentPoint.X, currentPoint.Y - 1);
                if (currentPoint.Y > 0 && !visited.Get(upPoint) && field.FieldNodes.Get(upPoint) != NodeType.Rock)
                {
                    var upNode = new GraphNode(upPoint, node, field.FieldNodes.Get(upPoint));
                    currentNode.ChildNodes.Add(upNode);
                    visited[currentPoint.X, currentPoint.Y - 1] = true;
                    q.Enqueue(upNode);
                }
                // left
                var leftPoint = new Point(currentPoint.X - 1, currentPoint.Y);
                if (currentPoint.X > 0 && !visited.Get(leftPoint) && field.FieldNodes.Get(leftPoint) != NodeType.Rock)
                {
                    var rightNode = new GraphNode(leftPoint, node, field.FieldNodes.Get(leftPoint));
                    currentNode.ChildNodes.Add(rightNode);
                    visited[currentPoint.X - 1, currentPoint.Y] = true;
                    q.Enqueue(rightNode);
                }
                // down
                var downPoint = new Point(currentPoint.X, currentPoint.Y + 1);
                if (currentPoint.Y + 1 < visited.GetLength(1) && !visited.Get(downPoint) && field.FieldNodes.Get(downPoint) != NodeType.Rock)
                {
                    var downNode = new GraphNode(new Point(currentPoint.X, currentPoint.Y + 1), node, field.FieldNodes[currentPoint.X, currentPoint.Y + 1]);
                    currentNode.ChildNodes.Add(downNode);
                    visited[currentPoint.X, currentPoint.Y + 1] = true;
                    q.Enqueue(downNode);
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
