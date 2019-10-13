using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agent.Graphs;
using Agent.Models;
using Agent.Others;

namespace Agent.Solutions
{
    public interface ISolutionProvider
    {
        Route Solve(ActionField actionField);
    }
}
