using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agent.Algorythms;
using Agent.Graphs;
using Agent.Models;
using Agent.Others;

namespace Agent.GraphCreators
{
    public class DfsGraphCreator : IGraphCreator
    {
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
            AppendNextNode(sourceNode, visited, actionField);
            return sourceNode;
        }

        private void AppendNextNode(GraphNode node, bool[,] visited, ActionField field)
        {
            Point currentPoint = node.Point;
            visited[currentPoint.X, currentPoint.Y] = true;
            node.ChildNodes = new List<GraphNode>();
            // right
            if (currentPoint.X + 1 < visited.GetLength(0) && !visited[currentPoint.X + 1, currentPoint.Y] && field.FieldNodes[currentPoint.X + 1, currentPoint.Y] != NodeType.Rock)
            {
                var rightNode = new GraphNode(new Point(currentPoint.X + 1, currentPoint.Y), node, field.FieldNodes[currentPoint.X + 1, currentPoint.Y]);
                node.ChildNodes.Add(rightNode);
                AppendNextNode(rightNode, visited, field);
            }
            // up
            if (currentPoint.Y > 0 && !visited[currentPoint.X, currentPoint.Y - 1] && field.FieldNodes[currentPoint.X, currentPoint.Y - 1] != NodeType.Rock)
            {
                var rightNode = new GraphNode(new Point(currentPoint.X, currentPoint.Y - 1), node, field.FieldNodes[currentPoint.X, currentPoint.Y - 1]);
                node.ChildNodes.Add(rightNode);
                AppendNextNode(rightNode, visited, field);
            }
            // left
            if (currentPoint.X > 0 && !visited[currentPoint.X - 1, currentPoint.Y] && field.FieldNodes[currentPoint.X - 1, currentPoint.Y] != NodeType.Rock)
            {
                var rightNode = new GraphNode(new Point(currentPoint.X - 1, currentPoint.Y), node, field.FieldNodes[currentPoint.X - 1, currentPoint.Y]);
                node.ChildNodes.Add(rightNode);
                AppendNextNode(rightNode, visited, field);
            }
            // down
            if (currentPoint.Y + 1 < visited.GetLength(1) && !visited[currentPoint.X, currentPoint.Y + 1] && field.FieldNodes[currentPoint.X, currentPoint.Y + 1] != NodeType.Rock)
            {
                var rightNode = new GraphNode(new Point(currentPoint.X, currentPoint.Y + 1), node, field.FieldNodes[currentPoint.X, currentPoint.Y + 1]);
                node.ChildNodes.Add(rightNode);
                AppendNextNode(rightNode, visited, field);
            }
        }
    }
}
