using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agent.Models;
using Agent.Others;

namespace Agent.Graphs
{
    public class GraphNode
    {
        public GraphNode ParentNode { set; get; }

        public List<GraphNode> ChildNodes { get; set; }

        public Node Node { private set; get; }

        public GraphNode(Node node, GraphNode parentNode)
        {
            Node = node;
            ParentNode = parentNode;
        }
    }
}
