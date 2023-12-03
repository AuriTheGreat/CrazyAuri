using CrazyAuri;
using CrazyAuri.Models;
using CrazyAuriConsole.Players;
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
            IPlayer WhitePlayer = new BotPlayer();
            IPlayer BlackPlayer = new StockfishPlayer();

            while (true)
            {
                Console.WriteLine(board);
                board.PrintBoard();
                var winner = board.GetWinner();
                if (winner != "0")
                {
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
                    break;
                }

                if (board.CurrentColor == true)
                {
                    BlackPlayer.MakeMove(board);
                }
                else
                {
                    WhitePlayer.MakeMove(board);
                }
            }
        }
    }
}
