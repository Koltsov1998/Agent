using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Agent.Models;

namespace Agent.Algorythms
{
    public interface IAlgorythm
    {
        List<Point> Solve(ActionField actionField);
    }
}
