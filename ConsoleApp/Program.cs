using CrazyAuri;
using CrazyAuri.Models;
using CrazyAuriLibrary.Models.Moves;
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
