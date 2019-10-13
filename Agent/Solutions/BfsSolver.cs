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

        public Route Solve(ActionField actionField)
        {
            Point agentPoint = new Point(1, 1);
            int cookiesCount = 0;
            for (int i = 0; i < actionField.Width; i++)
            {
                for (int j = 0; j < actionField.Height; j++)
                {
                    switch (actionField.FieldNodes[i, j])
                    {
                        case NodeType.Agent:
                        {
                            agentPoint = new Point(i, j);
                            break;
                        }
                        case NodeType.Cookie:
                        {
                            cookiesCount++;
                            break;
                        }
                    }
                }
            }
            var graph = _graphCreator.GenerateGraph(actionField, agentPoint);
            var result = new Route();
            result.AppendNode(agentPoint);

            for (int i = 0; i < cookiesCount; i++)
            {
                var routeToCookie = FindWayToObject(graph, NodeType.Cookie);
                result = Route.ConcatRoutes(result, routeToCookie);
                graph = _graphCreator.GenerateGraph(actionField, result.LastPoint);
            }

            var routeToStar = FindWayToObject(graph, NodeType.Star);
            result = Route.ConcatRoutes(result, routeToStar);

            return result;
        }

        private Route FindWayToObject(GraphNode startNode, NodeType objectType)
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

        private Route GetBackRoute(GraphNode foundObject, GraphNode startObject)
        {
            var currentNode = foundObject;
            var queue = new Stack<Point>();

            while (currentNode != startObject)
            {
                queue.Push(currentNode.Point);
                currentNode = currentNode.ParentNode;
            }
            queue.Push(startObject.Point);

            var result = new Route();

            while (queue.Count > 0)
            {
                result.AppendNode(queue.Pop());
            }

            return result;
        }
    }
}
