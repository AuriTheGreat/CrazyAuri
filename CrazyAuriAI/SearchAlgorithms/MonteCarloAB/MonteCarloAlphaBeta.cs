using CrazyAuri.Models;
using CrazyAuriAI.Evaluation.Functions;
using CrazyAuriAI.SearchAlgorithms.Minimax;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyAuriAI.SearchAlgorithms.MonteCarloAB
{
    public class MonteCarloAlphaBeta
    {
        Stopwatch stopwatch = new Stopwatch();
        Random random = new Random();
        IEvaluationFunction evaluationFunction = new MainEvaluationFunction();
        //CrazyAuriAI.SearchAlgorithms.Minimax.Minimax minimax = new CrazyAuriAI.SearchAlgorithms.Minimax.Minimax();
        private short p = 0;
        private int depth = 0;

        public string MCTSAlphaBeta(Board currentboard)
        {
            stopwatch.Start();

            var position = new PositionState(currentboard);
            position.depth = 4;

            while (stopwatch.Elapsed.TotalSeconds < 3)
            {
                if (random.NextDouble() < p)
                    MCTSRollout(position);
                else
                    alphaBetaRollout(position);
            }
            stopwatch.Stop();

            return BestMove(position);

        }

        private double MCTSRollout(PositionState p)
        {
            return 0;
        }

        private double alphaBetaRollout(PositionState p)
        {
            p.childPositions.Clear();
            if (p.board.GetWinner() == "0" && p.depth > 0)
            {
                var defaultalpha = double.MinValue;
                var defaultbeta = double.MinValue;
                if (p.board.CurrentColor == true)
                {
                    defaultalpha = double.MaxValue;
                    defaultbeta = double.MaxValue;
                }

                foreach (var i in p.board.GetAllMoves())
                {
                    var newposition = new PositionState(p.board.Copy(), i);
                    p.childPositions.Add(newposition);
                    newposition.board.MakeMove(i);
                    var value = evaluationFunction.GetEvaluation(newposition.board);
                    if (p.board.CurrentColor == true)
                        newposition.alpha = value;
                    else
                        newposition.beta = value;
                    newposition.alpha = Math.Max(p.alpha, newposition.alpha);
                    newposition.beta = Math.Min(p.beta, newposition.beta);
                    newposition.depth = p.depth - 1;
                    if (p.board.CurrentColor == true)
                    {
                        defaultalpha = Math.Min(defaultalpha, newposition.alpha);
                        defaultbeta = Math.Min(defaultbeta, newposition.beta);
                    }
                    else
                    {
                        defaultalpha = Math.Max(defaultalpha, newposition.alpha);
                        defaultbeta = Math.Max(defaultbeta, newposition.beta);
                    }
                }
                p.alpha = defaultalpha;
                p.beta = defaultbeta;
                PositionState? nextstate=null;
                foreach(var i in p.childPositions)
                {
                    if (i.alpha < i.beta)
                    {
                        nextstate = i;
                        break;
                    }
                }
                var result = alphaBetaRollout(nextstate);
                return result;
            }
            else
            {
                var evaluation = evaluationFunction.GetEvaluation(p.board);
                p.alpha = evaluation;
                p.beta = evaluation;

                return evaluation;
            }
        }

        public string BestMove(PositionState position)
        {
            double bestscore = 0;
            string bestmove = "";
            if (position.board.CurrentColor == true)
                bestscore = double.MaxValue;
            else
                bestscore = double.MinValue;

            foreach (var i in position.childPositions)
            {
                if (position.board.CurrentColor == true) 
                {
                    if (i.alpha < bestscore)
                    {
                        bestscore = i.alpha;
                        bestmove = i.move.ToString();
                    }
                }
                else
                {
                    if (i.beta > bestscore)
                    {
                        bestscore = i.beta;
                        bestmove = i.move.ToString();
                    }
                }
            }
            return bestmove;
        }
    }
}
