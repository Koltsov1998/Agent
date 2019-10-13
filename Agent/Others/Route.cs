using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agent.Others
{
    public class Route
    {
        public List<Point> Points { private set; get; }

        public Route()
        {
            Points = new List<Point>();
        }

        private Route(List<Point> points)
        {
            Points = points;
        }

        public static Route FromPointsList(List<Point> _pointList)
        {
            return new Route(_pointList);
        }

        public void AppendNode(Point newPoint)
        {
            if (Points.Count > 0 && !newPoint.Near(Points.Last()))
            {
                throw new Exception($"Point #{newPoint} cannot be appended");
            }

            Points.Add(newPoint);
        }

        public static Route ConcatRoutes(Route route1, Route route2)
        {
            List<Point> newRoutePoints = route1.Points;
            if (route1.Points.Count > 0 && !route1.Points.Last().Equals(route2.Points.First()))
            {
                throw new Exception($"Routes can't be concatenated.\nRoute 1 lasts at {route1.Points.Last()}\nRoute 2 starts at {route2.Points.First()}");
            }

            var route2Points = route2.Points;
            route2Points.RemoveAt(0);
            newRoutePoints.AddRange(route2Points);

            return new Route(newRoutePoints);
        }

        public override bool Equals(object obj)
        {
            var other = obj as Route;

            if (other.Points.Count != this.Points.Count)
            {
                return false;
            }

            for (int i = 0; i < this.Points.Count; i++)
            {
                if (!Points[i].Equals(other.Points[i]))
                {
                    return false;
                }
            }

            return true;
        }

        public Point LastPoint => Points.Last();
    }
}
