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
using NUnit.Framework.Constraints;

namespace Agent.Solutions
{
    public class BfsSolver : ISolutionProvider
    {
        private readonly BfsGraphCreator _graphCreator = new BfsGraphCreator();

        public Solution Solve(ActionField actionField)
        {
            Node agentNode = actionField.Nodes.Single(n => n.NodeType == NodeType.Agent);
            int cookiesCount = actionField.Nodes.Count(n => n.NodeType == NodeType.Cookie);
            
            var graph = _graphCreator.GenerateGraph(actionField, agentNode.Point);
            var route = new Route();
            route.AppendNode(agentNode);

            for (int i = 0; i < cookiesCount; i++)
            {
                var routeToCookie = FindWayToObject(graph, NodeType.Cookie);
                route = Route.ConcatRoutes(route, routeToCookie);
                graph = _graphCreator.GenerateGraph(actionField, route.LastNode.Point);
            }

            var routeToStar = FindWayToObject(graph, NodeType.Star);
            route = Route.ConcatRoutes(route, routeToStar);

            return new Solution
            {
                Route = route,
                Graph = graph,
            };
        }

        public Solution FindWay(ActionField actionField, NodeType objectType)
        {
            Node agentNode = actionField.Nodes.Single(n => n.NodeType == NodeType.Agent);

            var graph = _graphCreator.GenerateGraph(actionField, agentNode.Point);
            var routeToObject = FindWayToObject(graph, objectType);
            return new Solution
            {
                Route = routeToObject,
                Graph = graph
            };
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
                    if(currentNodeChildNode.Node.NodeType != objectType)
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
            var queue = new Stack<Node>();

            while (currentNode != startObject)
            {
                queue.Push(currentNode.Node);
                currentNode = currentNode.ParentNode;
            }
            queue.Push(startObject.Node);

            var result = new Route();

            while (queue.Count > 0)
            {
                result.AppendNode(queue.Pop());
            }

            return result;
        }
    }
}
