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
            int numberOfGamesToPlay = 10;
            double whitePlayerResult = 0;
            double blackPlayerResult = 0;

            for (int i = 0; i < numberOfGamesToPlay; i++)
            {
                Board board = new Board("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR/ w KQkq - 0 1");
                IPlayer WhitePlayer = new MinimaxPlayer();
                IPlayer BlackPlayer = new RandomPlayer();
                if (WhitePlayer is HumanPlayer || BlackPlayer is HumanPlayer)
                    numberOfGamesToPlay = 1;
                if ((i + 1) % 2 == 0)
                    (WhitePlayer, BlackPlayer) = (BlackPlayer, WhitePlayer);
                while (true)
                {
                    Console.WriteLine(board);
                    board.PrintBoard();
                    var winner = board.GetWinner();
                    if (winner != "0")
                    {
                        double matchWinner = 0.5;
                        if (winner == "w")
                        {
                            Console.WriteLine("White has won the game!");
                            matchWinner = 1;
                        }
                        else if (winner == "b")
                        {
                            Console.WriteLine("Black has won the game!");
                            matchWinner = 0;
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
                        if ((i + 1) % 2 != 0)
                        {
                            whitePlayerResult += matchWinner;
                            blackPlayerResult += 1-matchWinner;
                        }
                        else
                        {
                            blackPlayerResult += matchWinner;
                            whitePlayerResult += 1 - matchWinner;
                        }
                        File.AppendAllText("result.txt", DateTime.Now + " " +
                            String.Join(", ", board.movehistory) + " "  + matchWinner + "-" + 
                            (1-matchWinner).ToString() + '\n');
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

                Console.WriteLine("Result: " + whitePlayerResult + "-" + blackPlayerResult);

            }
        }
    }
}
