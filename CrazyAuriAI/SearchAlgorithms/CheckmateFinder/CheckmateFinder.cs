using CrazyAuri.Models;
using CrazyAuriAI.Evaluation.Functions;
using CrazyAuriAI.SearchAlgorithms.Minimax;
using CrazyAuriLibrary.Models.Moves.MoveTypes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyAuriAI.SearchAlgorithms
{
    public class CheckmateFinder
    {
        public Stopwatch stopwatch;
        public Double searchLimitInSeconds = 3;
        public CheckmateFinder()
        {
            stopwatch = new Stopwatch();
        }
        public (string, double) runSearch(Board currentboard, int depth)
        {
            stopwatch.Start();
            var childNodes = currentboard.GetAllMoves();
            double value = double.MinValue;
            string bestmove = "";
            foreach (var child in childNodes)
            {
                var newboard = currentboard.Copy();
                newboard.MakeMove(child);
                var newvalue = -runRecursiveSearch(newboard, depth);
                if (newvalue > value)
                {
                    bestmove = child.ToString();
                    value = newvalue;
                    if (value > 0)
                        break;
                }
            }
            return (bestmove, value);
        }

        private double runRecursiveSearch(Board currentboard, int depth)
        {
            var color = currentboard.CurrentColor;
            var winner = currentboard.GetWinner();

            if (depth == 0 || winner!="0" || stopwatch.Elapsed.TotalSeconds>searchLimitInSeconds)
            {
                Double winnerevaluation = 0;
                if (winner == "w")
                    winnerevaluation = Double.MaxValue;
                if (winner == "b")
                    winnerevaluation = Double.MinValue;
                winnerevaluation *= color == true ? -1 : 1;

                return winnerevaluation;
            }

            var childNodes = currentboard.GetAllMoves();
            double value = double.MinValue;
            foreach (var child in childNodes)
            {
                var newboard = currentboard.Copy();
                newboard.MakeMove(child);
                value = Math.Max(value, -runRecursiveSearch(newboard, depth - 1));
            }

            return value;
        }
    }
}
