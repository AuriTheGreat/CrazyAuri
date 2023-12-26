﻿using CrazyAuri.Models;
using CrazyAuriAI.Evaluation.Functions;
using CrazyAuriAI.Evaluation.PieceEvaluationSets;
using CrazyAuriAI.SearchAlgorithms.MinimaxSearch;
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
    public class MonteCarloLegacy
    {
        private Random random = new Random();
        private IEvaluationFunction evaluationfunction = new MainEvaluationFunction();
        private MoveEvaluationFunction moveevaluationfunction = new MoveEvaluationFunction();
        private CheckmateFinder checkmatefinder;
        private MCMinimax minimax = new MCMinimax();
        private Node position;

        private short p = -1; // probability Minimax will be chosen over Monte Carlo


        private Object nodeChildPositionsLock = new Object();
        public MonteCarloLegacy (Board board)
        {
            position = new Node(board);
            checkmatefinder = new CheckmateFinder();
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
                        position.ExpandNode();
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
            checkmatefinder.runSearch(position, 1); // checks if any of nodes end with checkmate
            var threadcount = 1;
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
                lock (nodeChildPositionsLock)
                    currentposition.ExpandNode();
                if (random.NextDouble() < p)
                {
                    if (position.searchedMinimaxDepth < 2)
                    {
                        checkmatefinder.runSearch(position, 2);
                    }
                }
                else
                {
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
        }

        public bool continueRunningCheck(Node position, Stopwatch stopwatch, double time)
        {
            if (stopwatch.Elapsed.TotalSeconds < 60)
            {
                if (stopwatch.Elapsed.TotalSeconds < time)
                    return true;
                if (position.visits < 1200)
                    return true;
                return false;
            }
            else
            {
                return false;
            }
            //if (position.MostVisitedChildEqualsMostEvaluated())
            //    return false;
        }

        public double getUCBscore(Node node)
        {
            if (Math.Abs(node.minimaxValue) > 10000)
                return node.minimaxValue;
            if (node.visits == 0)
                return Double.MaxValue;
            double localevaluation = 0;
            var parentnode = node;
            if (node.parent != null)
            {
                parentnode = node.parent;
                if (!node.evaluated)
                {
                    foreach(var i in parentnode.childpositions)
                    {
                        if (Math.Abs(node.minimaxValue) > 10000)
                            continue;
                        localevaluation =moveevaluationfunction.GetEvaluation(parentnode.board, i.board, i.move);
                        i.localevaluation = localevaluation;
                        i.evaluated = true;
                        if(i.localevaluation<parentnode.lowestchildlocalevaluation)
                            parentnode.lowestchildlocalevaluation = localevaluation;
                    }
                }
                localevaluation = (parentnode.lowestchildlocalevaluation/4)+node.localevaluation;
            }

            var parentvisits = parentnode.visits;

            var c1 = 0.7;
            var c2 = 0.01;

            if (node.killerHeuristic.bestMove==node.move.ToString())
                c1 *=2;

            //return (node.scoreratio) + c1 * Math.Sqrt(Math.Log(parentvisits) / node.visits); // default MCTS

            // Minimax usage
            //var minimaxEvaluationWeight =  (Math.Min(Math.Max(node.minimaxValue - node.parent.minimaxValue, -300),300)+300)/600;

            // MCTS with heuristic evaluation
            return localevaluation / node.visits;
        }

        private Node SelectLeaf(Node node)
        {
            double maxU = Double.MinValue;
            Node returnedNode = node.childpositions.Count==0 ? node : node.childpositions[0];
            lock (nodeChildPositionsLock)
            {
                foreach (var i in node.childpositions)
                {
                    var UCBscore = getUCBscore(i);
                    if (UCBscore > maxU)
                    {
                        maxU = UCBscore;
                        returnedNode = i;
                    }
                }
            }
            return returnedNode;
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
            var startevaluation = evaluationfunction.GetEvaluation(newboard);
            var startingcolor = newboard.CurrentColor;
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
                    var localscore = Math.Min(10000, Math.Max(-10000, localevaluation));
                    if (board.CurrentColor == false)
                        localscore /= 10000;
                    else
                        localscore /= (-1 * 10000);

                    evaluationscore = localscore;

                }
            }
            return new SimulationResult(matingscore, evaluationscore, isdraw);
        }

        private string selectNextSimulationMove(Board board)
        {
            var moves = board.GetAllMoves();
            try
            {
                var evaluationsum = 0.0;
                var positionevaluations = new List<double>() { 0 };
                var evaluatedmoves = new List<Move>();
                foreach (var move in moves)
                {
                    var newboard = new Board(board.ToString(), board.FormerPositions);
                    newboard.MakeMove(move);
                    var localevaluation = Math.Max(0, moveevaluationfunction.GetEvaluation(board, newboard, move));
                    if (localevaluation <= -1000)
                        continue;
                    else if (localevaluation >= 1000)
                        return move.ToString();
                    evaluationsum += localevaluation;
                    positionevaluations.Add(localevaluation + positionevaluations.Last());
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
            }
            catch { }
            return moves[random.Next(moves.Count)].ToString(); // chooses random move if none was chosen for some reason
        }
    }
}
