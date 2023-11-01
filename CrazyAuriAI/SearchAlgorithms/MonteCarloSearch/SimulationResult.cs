using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyAuriAI.SearchAlgorithms.MonteCarloSearch
{
    public class SimulationResult
    {
        public int matingScore;
        public double evaluationScore;
        public bool isDraw;
        public SimulationResult(int matingScore, double evaluationScore, bool isDraw)
        {
            this.matingScore = matingScore;
            this.evaluationScore = evaluationScore;
            this.isDraw = isDraw;
        }
    }
}
