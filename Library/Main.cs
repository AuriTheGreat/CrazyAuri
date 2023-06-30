using CrazyAuri.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyAuri
{
    public static class CrazyAuriLibrary
    {
        public static void FindBestMove(string FEN)
        {
            Board board = new Board(FEN);
            board.PrintBoard();
            foreach (var i in board.WhitePieces)
            {
                foreach (var j in i.GetMoves(board))
                {
                    Console.Write(j);
                }
            }
        }
    }
}
