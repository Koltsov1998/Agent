using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agent.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class EnumerationTests
    {
        [TestMethod]
        public void TestEnumeration()
        {
            ActionField af = new ActionField(1, 1);
            af.Nodes = new FieldNodes(3, 3);
            foreach (var fieldNode in af.Nodes)
            {
                fieldNode.NodeType = NodeType.Gross;
            }

            var nodesList = af.Nodes.ToList();
        }
    }
}
