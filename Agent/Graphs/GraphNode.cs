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

        public Point Point { private set; get; }

        public NodeType NodeType { get; }

        public GraphNode(Point point, GraphNode parentNode, NodeType nodeType)
        {
            Point = point;
            ParentNode = parentNode;
            NodeType = nodeType;
        }
    }
}
