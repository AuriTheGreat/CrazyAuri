using CrazyAuri.Models;
using CrazyAuriAI.Evaluation.Functions;
using CrazyAuriAI.SearchAlgorithms.MinimaxSearch;
using CrazyAuriLibrary.Models.Moves.MoveTypes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyAuriAI.SearchAlgorithms.MonteCarloSearch
{
    public class CheckmateFinder
    {
        public Stopwatch stopwatch;
        public double searchLimitInSeconds = 3;
        public CheckmateFinder()
        {
            stopwatch = new Stopwatch();
        }
        public (string, double) runSearch(Node node, int depth)
        {
            stopwatch.Start();
            if (node.IsLeaf())
                node.ExpandNode();
            double value = double.MinValue;
            string bestmove = "";
            foreach (var child in node.childpositions)
            {
                var newvalue = -runRecursiveSearch(child, depth);
                if (newvalue > value)
                {
                    bestmove = child.move.ToString();
                    value = newvalue;
                    if (value > 0)
                        break;
                }
            }
            return (bestmove, value);
        }

        private double runRecursiveSearch(Node node, int depth)
        {
            var color = node.board.CurrentColor;
            var winner = node.board.GetWinner();
            double value = 0;

            if (depth == 0 || winner != "0")
            {
                if (winner == "w")
                {
                    value = double.MaxValue;
                }
                if (winner == "b")
                {
                    value = double.MinValue;
                }
            }
            else
            {
                if (node.IsLeaf())
                    node.ExpandNode();
                value = double.MinValue;
                foreach (var child in node.childpositions)
                {
                    value = Math.Max(value, -runRecursiveSearch(child, depth - 1));
                }
            }

            if (Math.Abs(value) > 1000)
            {
                if (value < -1000 && node.originalColor == true && node.board.CurrentColor==node.originalColor)
                    node.minimaxValue = double.MaxValue;
                else if (value < -1000 && node.originalColor == true && node.board.CurrentColor != node.originalColor)
                    node.minimaxValue = double.MinValue;
                else if (value > 1000 && node.originalColor == false && node.board.CurrentColor == node.originalColor)
                    node.minimaxValue = double.MaxValue;
                else if (value > 1000 && node.originalColor == false && node.board.CurrentColor != node.originalColor)
                    node.minimaxValue = double.MinValue;
            }
            node.searchedMinimaxDepth = depth;

            return value;
        }
    }
}
