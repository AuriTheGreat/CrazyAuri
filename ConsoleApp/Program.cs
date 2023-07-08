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
            //Board board = new Board("rnbk1b1r/pp3ppp/2p5/4q1B1/4n3/8/PPP2PPP/2KR1BNR b - - 1 10"); // double check test

            while (true)
            {
                Console.WriteLine(board);
                board.PrintBoard();
                var possiblemoves = board.GetAllMoves();
                if (possiblemoves.Count == 0)
                {
                    var winner = board.GetWinner();
                    if (winner == "w")
                    {
                        Console.WriteLine("White has won the game!");
                    }
                    else if (winner == "b")
                    {
                        Console.WriteLine("Black has won the game!");
                    }
                    else if (winner == "s")
                    {
                        Console.WriteLine("Stalemate!");
                    }
                    else if (winner == "r")
                    {
                        Console.WriteLine("Draw due to repetition!");
                    }
                    else if (winner == "50")
                    {
                        Console.WriteLine("Draw due to 50 move rule!");
                    }
                }

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
