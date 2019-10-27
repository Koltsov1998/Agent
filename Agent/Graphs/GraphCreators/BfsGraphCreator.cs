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
            var startNode = actionField.Nodes.Single(n => n.NodeType == NodeType.Agent);
            sourcePoint = startNode.Point;

            var visited = new bool[actionField.Width, actionField.Height];
            visited[sourcePoint.X, sourcePoint.Y] = true;

            GraphNode sourceNode = new GraphNode(actionField.Nodes.GetNode(sourcePoint), null);
            StartAppendingNodes(sourceNode, visited, actionField);
            return sourceNode;
        }

        public GraphNode GenerateGraph(ActionField actionField, Point startPoint)
        {
            var visited = new bool[actionField.Width, actionField.Height];
            visited[startPoint.X, startPoint.Y] = true;

            GraphNode sourceNode = new GraphNode(actionField.Nodes.GetNode(startPoint), null);
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

                var newX = currNode.Node.Point.X + dx;
                var newY = currNode.Node.Point.Y + dy;

                if (newX >= 0 && newX < field.Width && newY >= 0 && newY < field.Height && !visited[newX, newY] && field.Nodes.GetNode(newX, newY).NodeType != NodeType.Rock)
                {
                    var newNode = new GraphNode(field.Nodes.GetNode(newX, newY), currNode);
                    currNode.ChildNodes.Add(newNode);
                    visited[newX, newY] = true;
                    q.Enqueue(newNode);
                }
            }
        }
    }
}
