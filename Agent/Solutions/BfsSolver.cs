using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly BfsGraphCreator _graphCreator;

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
        }

        private List<GraphNode> FindWayToObject(GraphNode graph, NodeType objectType)
        {
            Queue<GraphNode> q = new Queue<GraphNode>();
            if (graph.ChildNodes == null || graph.ChildNodes.Count == 0)
            {
                throw new Exception("source graph node has no childs");
            }

            foreach (var graphChildNode in graph.ChildNodes)
            {
                q.Enqueue(graphChildNode);
            }

            while (q.Count > 0)
            {

            }
            {
                
            }
        }
    }
}
