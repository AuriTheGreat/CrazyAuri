using CrazyAuri.Models;
using CrazyAuriAI.SearchAlgorithms.MonteCarloAB;
using CrazyAuriAI.SearchAlgorithms.MonteCarloSearch;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyAuriAI.Bots
{
    public class MCABBot : IBot
    {
        public string GetMove(Board board)
        {
            Stopwatch stopwatch = new Stopwatch();
            MonteCarloAlphaBeta alg = new MonteCarloAlphaBeta();
            stopwatch.Start();
            string result = alg.MCTSAlphaBeta(board);
            stopwatch.Stop();
            var move = result;
            //var move = board.GetAllMoves()[0].ToString();
            //if (result != "")
            //    move = result;
            var color = "White";
            if (board.CurrentColor == true)
                color = "Black";

            Console.WriteLine(color + " move chosen: " + move + " After " + double.Round(stopwatch.Elapsed.TotalSeconds, 2) + "s.");
            return move;
        }
    }
}
