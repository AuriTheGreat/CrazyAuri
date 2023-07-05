using CrazyAuri;
using CrazyAuri.Models;
using CrazyAuriLibrary.Models.Moves.MoveTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyAuri
{
    class CrazyAuri
    {
        static public void Main(string[] args)
        {
            Board board = new Board("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR/ w KQkq - 0 1");
            //Board board = new Board("rn1qk1nr/pppppppp/8/7b/5q1b/5BQ1/PPPP1K1P/RN3BNR w HAkq - 0 1"); // pin test
            while (true)
            {
                board.PrintBoard();
                var possiblemoves = board.GetAllMoves();
                if (possiblemoves.Count == 0)
                    break;
                while (true)
                {
                    PrintMoves(possiblemoves);
                    Console.WriteLine("Enter your move:");
                    string move = Console.ReadLine();
                    if (board.MakeMove(move) == true)
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Move is illegal!");
                    }
                }
            }
            
        }

        private static void PrintMoves(List<Move> possiblemoves)
        {
            foreach (var i in possiblemoves)
            {
                Console.Write(i);
                Console.Write(" ");
            }
            Console.WriteLine();
        }
    }
}
