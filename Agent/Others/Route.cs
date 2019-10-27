using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agent.Models;

namespace Agent.Others
{
    public class Route
    {
        public List<Node> Nodes { private set; get; }

        public Route()
        {
            Nodes = new List<Node>();
        }

        private Route(List<Node> nodes)
        {
            Nodes = nodes;
        }

        public static Route FromNodesList(List<Node> _pointList)
        {
            return new Route(_pointList);
        }

        public void AppendNode(Node newNode)
        {
            if (Nodes.Count > 0 && !newNode.Point.Near(Nodes.Last().Point))
            {
                throw new Exception($"Point #{newNode} cannot be appended");
            }

            Nodes.Add(newNode);
        }

        public static Route ConcatRoutes(Route route1, Route route2)
        {
            List<Node> newRoutePoints = route1.Nodes;
            if (route1.Nodes.Count > 0 && !route1.Nodes.Last().Equals(route2.Nodes.First()))
            {
                throw new Exception($"Routes can't be concatenated.\nRoute 1 lasts at {route1.Nodes.Last()}\nRoute 2 starts at {route2.Nodes.First()}");
            }

            var route2Points = route2.Nodes;
            route2Points.RemoveAt(0);
            newRoutePoints.AddRange(route2Points);

            return new Route(newRoutePoints);
        }

        public override bool Equals(object obj)
        {
            var other = obj as Route;

            if (other.Nodes.Count != this.Nodes.Count)
            {
                return false;
            }

            for (int i = 0; i < this.Nodes.Count; i++)
            {
                if (!Nodes[i].Equals(other.Nodes[i]))
                {
                    return false;
                }
            }

            return true;
        }

        public Node LastNode => Nodes.Last();
    }
}
