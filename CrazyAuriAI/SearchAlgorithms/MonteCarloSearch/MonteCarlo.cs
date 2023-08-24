using CrazyAuri.Models;
using CrazyAuriAI.Evaluation.Functions;
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
        private Random random = new Random();
        private IEvaluationFunction evaluationfunction = new MainEvaluationFunction();
        public string MonteCarloSearch(Board board, double time)
        {
            var position = new Node(board);
            position.ExpandNode();

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            while (stopwatch.Elapsed.TotalSeconds < time)
            {
                var currentposition = position;
                while (!currentposition.IsLeaf())
                {
                    currentposition=SelectLeaf(currentposition);
                }
                currentposition.ExpandNode();
                currentposition = SelectLeaf(currentposition);
                var result = Simulate(board);
                while (currentposition.HasParent())
                {
                    currentposition.Update(result);
                    currentposition = currentposition.parent;
                }
            }
            stopwatch.Stop();

            return SelectLeaf(position).move.ToString(); // should probably select one where score is divided by visit count
        }

        public double getUCBscore(Node node)
        {
            if (node.visits == 0)
                return Double.MaxValue;
            var parentnode = node;
            if (parentnode.parent != null)
                parentnode = node.parent;
            var parentvisits = parentnode.visits;
            if (parentvisits == 0)
                parentvisits = 1;

            var c1 = 1.2; 
            var c2 = 3;

            return (node.score / node.visits) + c1 * Math.Sqrt(Math.Log(parentvisits) / node.visits); // default MCTS

            var localevaluation = evaluationfunction.GetEvaluation(node.board);
            if (node.board.CurrentColor == true)
                localevaluation *= -1; // UCL formula needs evaluation from the perspective of the playing color

            return (node.score / node.visits) + c1 * Math.Sqrt(Math.Log(parentvisits) / node.visits) + c2 * (localevaluation/Double.MaxValue); // MCTS with heuristic evaluation
        }

        private Node SelectLeaf(Node node)
        {
            double maxU = -1;
            foreach (var i in node.childpositions)
            {
                if (getUCBscore(i) > maxU)
                {
                    maxU = getUCBscore(i);
                    node = i;
                }
            }
            return node;
        }

        private double Simulate(Board board)
        {
            double localscore = 0;
            int depth = 3;
            bool done = false;
            var newboard = new Board(board.ToString(), board.FormerPositions);
            while (done == false && depth > 0)
            {
                depth -= 1;
                var moves = newboard.GetAllMoves();
                newboard.MakeMove(moves[random.Next(moves.Count)]);
                if (newboard.GetWinner() != "0")
                {
                    if (newboard.GetWinner() == "w")
                        localscore = 1;
                    else if (newboard.GetWinner() == "b")
                        localscore = -1;
                    done = true;
                }
                else if (depth == 0)
                {
                    localscore = evaluationfunction.GetEvaluation(newboard);
                    if (localscore > 0)
                        localscore /= double.MaxValue;
                    else if (localscore < 0)
                        localscore /= (-1 * double.MinValue);
                }
            }
            return localscore;
        }
    }
}
