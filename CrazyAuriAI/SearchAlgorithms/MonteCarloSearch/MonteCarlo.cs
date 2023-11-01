using CrazyAuri.Models;
using CrazyAuriAI.Evaluation.Functions;
using CrazyAuriAI.Evaluation.PieceEvaluationSets;
using CrazyAuriAI.SearchAlgorithms.Minimax;
using CrazyAuriLibrary.Models.Moves.MoveTypes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
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

        private Object nodeChildPositionsLock = new Object();
        public MonteCarlo (Board board)
        {
            position = new Node(board);
            position.ExpandNode();
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
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            var threadcount = 7;
            Parallel.For(0, threadcount, i =>
            {
                runSearchOnThread(position, stopwatch, time);
            });
            while (continueRunningCheck(position, stopwatch, time)) { }
            position.childpositions.Sort((p, q) => p.visits.CompareTo(q.visits)); //debugging purposes
            position.childpositions.Reverse();
            stopwatch.Stop();

            return (position.mostvisitedchild.move.ToString(), position.mostvisitedchild.evaluationscoreratio);
        }

        public void runSearchOnThread(Node position, Stopwatch stopwatch, double time)
        {
            while (continueRunningCheck(position, stopwatch, time))
            {
                var currentposition = position;
                while (!currentposition.IsLeaf())
                {
                    currentposition = SelectLeaf(currentposition);
                }
                if (currentposition.IsLeaf())
                {
                    lock (nodeChildPositionsLock)
                        currentposition.ExpandNode();
                }
                currentposition = SelectLeaf(currentposition);
                var result = Simulate(position.board);
                while (currentposition.HasParent())
                {
                    currentposition.Update(result);
                    currentposition = currentposition.parent;
                }
                currentposition.Update(result);
            }
        }

        public bool continueRunningCheck(Node position, Stopwatch stopwatch, double time)
        {
            if (stopwatch.Elapsed.TotalSeconds < time)
                return true;
            if (position.visits < 1200)
                return true;
            //if (stopwatch.Elapsed.TotalSeconds > 20)
            //    return false;
            //if (position.MostVisitedChildEqualsMostEvaluated())
            //    return false;
            return false;
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

            // MCTS with heuristic evaluation
            return node.evaluationscoreratio + node.matingscoreratio 
                + c1 * Math.Sqrt(Math.Log(parentvisits) / node.visits) 
                + c2 * (localevaluation);
        }

        private Node SelectLeaf(Node node)
        {
            double maxU = -1;
            lock (nodeChildPositionsLock)
            {
                foreach (var i in node.childpositions)
                {
                    if (getUCBscore(i) > maxU)
                    {
                        maxU = getUCBscore(i);
                        node = i;
                    }
                }
            }
            return node;
        }

        private Object boardLock = new Object();

        private SimulationResult Simulate(Board board)
        {
            double evaluationscore = 0;
            int matingscore = 0;
            bool isdraw = false;
            int depth = 6;
            bool done = false;
            Board newboard;
            lock (boardLock)
            {
                newboard = new Board(board.ToString(), board.FormerPositions);
            }
            while (done == false && depth > 0)
            {
                depth -= 1;
                var nextmove = selectNextSimulationMove(newboard);
                newboard.MakeMove(nextmove);
                if (newboard.GetWinner() != "0")
                {
                    if (newboard.GetWinner() == "w")
                        matingscore = 1;
                    else if (newboard.GetWinner() == "b")
                        matingscore = -1;
                    else
                        isdraw = true;
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
                        evaluationscore = -1;
                    else if (localevaluation < -500)
                        evaluationscore = 1;

                }
            }
            return new SimulationResult(matingscore, evaluationscore, isdraw);
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
