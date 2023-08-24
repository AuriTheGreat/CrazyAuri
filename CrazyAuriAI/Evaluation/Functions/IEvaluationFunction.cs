using CrazyAuri.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyAuriAI.Evaluation.Functions
{
    public interface IEvaluationFunction
    {
        public double GetEvaluation(Board board);

    }
}
