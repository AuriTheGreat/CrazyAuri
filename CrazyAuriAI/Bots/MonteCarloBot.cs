﻿using CrazyAuri.Models;
using CrazyAuriAI.SearchAlgorithms;
using CrazyAuriAI.SearchAlgorithms.MonteCarloSearch;
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
    public class MonteCarloBot : IBot
    {
        public string GetMove(Board board)
        {
            Stopwatch stopwatch = new Stopwatch();
            MonteCarlo montecarlo = new MonteCarlo();
            stopwatch.Start();
            (string, double) result = montecarlo.MonteCarloSearch(board, 3);
            stopwatch.Stop();
            var move = result.Item1;
            var evaluation = result.Item2.ToString();
            //var move = board.GetAllMoves()[0].ToString();
            //if (result != "")
            //    move = result;
            var color = "White";
            if (board.CurrentColor == true)
                color = "Black";

            Console.WriteLine(color + " move chosen: " + move + " (" + evaluation + ") After " + double.Round(stopwatch.Elapsed.TotalSeconds,2) + "s.");
            return move;
        }
    }
}
