using CrazyAuri.Models;
using CrazyAuriAI.Evaluation.Functions;
using CrazyAuriLibrary.Models.Moves.MoveTypes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CrazyAuriAI.SearchAlgorithms.MonteCarloSearch
{
    public class MonteCarlo
    {
        private Random random = new Random();
        private IEvaluationFunction evaluationfunction = new MainEvaluationFunction();
        private MoveEvaluationFunction moveevaluationfunction = new MoveEvaluationFunction();
        private CrazyAuriAI.SearchAlgorithms.Minimax.Minimax minimax = new CrazyAuriAI.SearchAlgorithms.Minimax.Minimax();
        private Node position;
        public MonteCarlo (Board board)
        {
            position = new Node(board);
        }

        public void UpdateParent(string move)
        {
            if (position.childpositions.Count > 0)
            {
                foreach (var i in position.childpositions)
                {
                    if (i.board.lastmovemade.ToString() == move)
                    {
                        position = i;
                        position.parent = null;
                        return;
                    }
                }
                throw new Exception("No child position found.");
            }

        }

        public void UpdateParent(Board board)
        {
            try
            {
                UpdateParent(board.lastmovemade.ToString());
            }
            catch (Exception e)
            {
                position = new Node(board);
            }
        }

        public (string, double) MonteCarloSearch(double time)
        {
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
                var result = Simulate(position.board);
                while (currentposition.HasParent())
                {
                    currentposition.Update(result);
                    currentposition = currentposition.parent;
                }
                currentposition.Update(result);
            }
            stopwatch.Stop();

            double bestratio = double.MinValue;
            double bestvisits = 0;
            var bestposition = position;

            foreach (var i in position.childpositions)
            {
                if (i.visits > bestvisits)
                {
                    bestratio = i.scoreratio;
                    bestvisits = i.visits;
                    bestposition = i;
                }
                else if (i.visits == bestvisits)
                    if (i.scoreratio > bestratio)
                    {
                        bestratio = i.scoreratio;
                        bestposition = i;
                    }
            }

            return (bestposition.move.ToString(), position.scoreratio);
        }

        public double getUCBscore(Node node)
        {
            double localevaluation = 0;
            if (node.visits == 0)
                return Double.MaxValue;
            var parentnode = node;
            if (parentnode.parent != null)
            {
                parentnode = node.parent;
                localevaluation = moveevaluationfunction.GetEvaluation(parentnode.board, node.board, node.move);
            }
            var parentvisits = parentnode.visits;

            var c1 = 4; 
            var c2 = 0.01;

            //return (node.scoreratio) + c1 * Math.Sqrt(Math.Log(parentvisits) / node.visits); // default MCTS

            return (node.score / node.visits) + c1 * Math.Sqrt(Math.Log(parentvisits) / node.visits) + c2 * (localevaluation); // MCTS with heuristic evaluation
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
            int depth = 6;
            bool done = false;
            var newboard = new Board(board.ToString(), board.FormerPositions);
            while (done == false && depth > 0)
            {
                depth -= 1;
                var nextmove = selectNextSimulationMove(newboard);
                newboard.MakeMove(nextmove);
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
                    var localevaluation = evaluationfunction.GetEvaluation(newboard);
                    /*
                    localscore = Math.Min(3000, Math.Max(-3000, localevaluation));
                    if (board.CurrentColor == false)
                        localscore /= 3000;
                    else
                        localscore /= (-1 * 3000);
                    */

                    if (newboard.CurrentColor == true)
                        localevaluation *= -1; // Evaluation from the perspective of the playing color
                    if (localevaluation > 500)
                        localscore = -1;
                    else if (localevaluation < -500)
                        localscore = 1;

                }
            }
            return localscore;
        }

        private string selectNextSimulationMove(Board board)
        {
            var moves = board.GetAllMoves();
            var evaluationsum = 0.0;
            var positionevaluations = new List<double>() { 0 };
            var evaluatedmoves = new List<Move>();
            foreach (var move in moves)
            {
                var newboard = new Board(board.ToString(), board.FormerPositions);
                newboard.MakeMove(move);
                var localevaluation = moveevaluationfunction.GetEvaluation(board, newboard, move);
                if (localevaluation <= -1000)
                    continue;
                else if (localevaluation >= 1000)
                    return move.ToString();
                evaluationsum += localevaluation;
                positionevaluations.Add(localevaluation+positionevaluations.Last());
                evaluatedmoves.Add(move);
            }
            /*
            var result = new Dictionary<string, double>();
            for (int j = 0; j < 1000; j++)
            {
                var correctvalue = evaluationsum * random.NextDouble();
                var lowerbound = 0.0;
                var higherbound = 0.0;
                for (int i = 1; i < positionevaluations.Count; i++)
                {
                    lowerbound = positionevaluations[i - 1];
                    higherbound = positionevaluations[i];
                    if (lowerbound <= correctvalue && correctvalue <= higherbound)
                    {
                        //Console.WriteLine(lowerbound);
                        //Console.WriteLine(correctvalue);
                        //Console.WriteLine(higherbound);
                        if(!result.ContainsKey(evaluatedmoves[i - 1].ToString()))
                            result[evaluatedmoves[i - 1].ToString()] = 0;
                        result[evaluatedmoves[i - 1].ToString()] += 1;
                    }
                }
            }
            foreach (var entry in result.Keys)
                Console.WriteLine(entry + ": " +result[entry].ToString());
            throw new Exception("Hello");
            */

            var correctvalue = evaluationsum * random.NextDouble();
                var lowerbound = 0.0;
                var higherbound = 0.0;
            for (int i = 1; i < positionevaluations.Count; i++)
            {
                lowerbound = positionevaluations[i - 1];
                higherbound = positionevaluations[i];
                if (lowerbound <= correctvalue && correctvalue <= higherbound)
                {
                    return evaluatedmoves[i - 1].ToString();
                }
            }
            return null;
        }
    }
}
