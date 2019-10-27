using System;
using System.Collections.Generic;
using Agent.Models;
using Agent.Others;
using Agent.Solutions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class SolutionTests
    {
        [TestMethod]
        public void RouteConcatenating()
        {
            List<Point> points1 = new List<Point>
            {
                new Point(1, 0),
                new Point(2, 0),
            };
            List<Point> points2 = new List<Point>
            {
                new Point(2, 0),
                new Point(2, 1),
            };
            List<Point> resultRoutePoints = new List<Point>
            {
                new Point(1, 0),
                new Point(2, 0),
                new Point(2, 1),
            };
            //var testRoute = Route.FromNodesList(resultRoutePoints);


            //var route1 = Route.FromNodesList(points1);
            //var route2 = Route.FromNodesList(points2);

            //var concatenatedRoute = Route.ConcatRoutes(route1, route2);
            //Assert.AreEqual(testRoute, concatenatedRoute);
        }

        [TestMethod]
        public void TestSolution()
        {
            var af = CreateActionField();
            var solver = new BfsSolver();
            var solutionRoute = solver.Solve(af);
        }

        private ActionField CreateActionField()
        {
            string[] fieldPrototype = new[]
            {
                "F# C",
                "    ",
                "A #C",
                " C##",
            };
            ActionField af = new ActionField(fieldPrototype);

            return af;
        }
    }
}

//"    ",
//"  C#",
//"  C ",
//"CFA ",