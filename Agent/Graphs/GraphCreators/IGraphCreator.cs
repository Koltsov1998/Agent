using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agent.Algorythms;
using Agent.Graphs;
using Agent.Models;

namespace Agent.GraphCreators
{
    public interface IGraphCreator
    {
        GraphNode GenerateGraph(ActionField actionField);
    }
}
