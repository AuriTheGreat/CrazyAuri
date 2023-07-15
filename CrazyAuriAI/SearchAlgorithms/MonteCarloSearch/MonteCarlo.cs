using CrazyAuri.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyAuriAI.SearchAlgorithms.MonteCarloSearch
{
    public class MonteCarlo
    {
        public string MonteCarloSearch(Board board, double time)
        {
            var position = new Node(board, null, false);

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            while (stopwatch.Elapsed.TotalSeconds < time)
            {
                position.explore();
            }
            stopwatch.Stop();

            var nextboard = position.next();
            return position.findTheBoardMove(nextboard).ToString();
        }


    }
}
