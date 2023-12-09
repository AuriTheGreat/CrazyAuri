using CrazyAuri.Models;
using CrazyAuriAI.SearchAlgorithms.MinimaxSearch;
using CrazyAuriLibrary.Models.Moves.MoveTypes;
using CrazyAuriLibrary.Models.Pieces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace CrazyAuriAI.Bots
{
    public class MinimaxBot : IBot
    {
        public string GetMove(Board board)
        {
            Stopwatch stopwatch = new Stopwatch();
            Minimax negamax = new Minimax();
            stopwatch.Start();
            (string, double) result = negamax.NegaMax(board, 4, double.MinValue, double.MaxValue, board.CurrentColor);
            stopwatch.Stop();
            var move = board.GetAllMoves()[0].ToString();
            var evaluation = result.Item2.ToString();
            if (result.Item1 != "")
            {
                move = result.Item1;
                if (result.Item2 > 100000000)
                    evaluation = "Winning";
                else if (result.Item2 < -100000000)
                    evaluation = "Hopeless";
            }
            else
                evaluation = "Random";
            var color = "White";
            if (board.CurrentColor == true)
                color = "Black";

            Console.WriteLine(color + " move chosen: " + move + " (" + evaluation + ") After " + double.Round(stopwatch.Elapsed.TotalSeconds,2) + "s.");
            return move;
        }
    }
}
