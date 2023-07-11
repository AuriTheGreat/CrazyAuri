using CrazyAuri.Models;
using CrazyAuriLibrary.Models.Moves.MoveTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyAuriConsole.Players
{
    public class HumanPlayer : IPlayer
    {
        public void MakeMove(Board board)
        {
            var possiblemoves = board.GetAllMoves();
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

        private void PrintMoves(List<Move> possiblemoves)
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
