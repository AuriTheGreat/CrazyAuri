using CrazyAuri.Models;
using CrazyAuriAI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyAuriConsole.Players
{
    public class BotPlayer : IPlayer
    {
        public MainBot bot = new MainBot();
        public void MakeMove(Board board)
        {
            if (board.MakeMove(bot.GetMove(board)) == false)
            {
                board.MakeMove(board.GetAllMoves()[0]);
            }
        }
    }
}
