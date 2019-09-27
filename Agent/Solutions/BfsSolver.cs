using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Agent.Graphs;
using Agent.Graphs.GraphCreators;
using Agent.Models;
using Agent.Others;

namespace Agent.Solutions
{
    public class BfsSolver : ISolutionProvider
    {
        private readonly BfsGraphCreator _graphCreator = new BfsGraphCreator();

        public List<GraphNode> Solve(ActionField actionField)
        {
            Point agentPoint = new Point(1, 1);
            for (int i = 0; i < actionField.Width; i++)
                for (int j = 0; j < actionField.Height; j++)
                {
                    if (actionField.FieldNodes[i, j] == NodeType.Agent)
                    {
                        agentPoint = new Point(i, j);
                    }
                }

            var graph = _graphCreator.GenerateGraph(actionField, agentPoint);
            return FindWayToObject(graph, NodeType.Cookie);
        }

        private List<GraphNode> FindWayToObject(GraphNode startNode, NodeType objectType)
        {
            var visited = new HashSet<GraphNode>();
            if (startNode.ChildNodes == null || startNode.ChildNodes.Count == 0)
            {
                throw new Exception("source graph node has no childs");
            }

            Queue<GraphNode> q = new Queue<GraphNode>();

            foreach (var graphChildNode in startNode.ChildNodes)
            {
                q.Enqueue(graphChildNode);
            }

            while (q.Count > 0)
            {
                var currentNode = q.Dequeue();
                if (visited.Contains(currentNode))
                {
                    continue;
                }

                foreach (var currentNodeChildNode in currentNode.ChildNodes)
                {
                    if(currentNodeChildNode.NodeType != objectType)
                    {
                        q.Enqueue(currentNodeChildNode);
                    }
                    else
                    {
                        return GetBackRoute(currentNodeChildNode, startNode);
                    }
                }
            }

            throw new Exception("Object not found");
        }

        private List<GraphNode> GetBackRoute(GraphNode foundObject, GraphNode startObject)
        {
            var currentNode = foundObject;
            var result = new List<GraphNode>();

            while (currentNode != startObject)
            {
                result.Add(currentNode);
                currentNode = currentNode.ParentNode;
            }
            result.Add(startObject);
            return result;
        }
    }
}
