using CrazyAuri.Models;
using CrazyAuriLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace CrazyAuri
{
    public class ChessGameInstance
    {
        private Board board = new Board();
        public ChessGameInstance(string FEN)
        {
            board = new Board(FEN);

        }

        public void PrintBoard()
        {
            board.PrintBoard();
        }

        public void PrintAllMoves()
        {
            foreach (var i in board.GetAllMoves())
            {
                Console.Write(i);
                Console.Write(" ");
            }
            Console.WriteLine();
        }

        public void MakeMove(string move)
        {
            board.MakeMove(move);
        }
    }
}
