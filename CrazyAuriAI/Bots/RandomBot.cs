using CrazyAuri.Models;
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
    public class RandomBot : IBot
    {
        public string GetMove(Board board)
        {
            Random random = new Random();
            var moves = board.GetAllMoves();
            var move = moves[random.Next(moves.Count)].ToString();
            var color = "White";
            if (board.CurrentColor == true)
                color = "Black";

            Console.WriteLine(color + " move chosen: " + move + " (Random)");
            return move;
        }
    }
}
