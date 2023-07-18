using CrazyAuri.Models;
using CrazyAuriAI;
using CrazyAuriAI.Bots;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyAuriConsole.Players
{
    public class StockfishPlayer : IPlayer
    {
        public StockfishBot bot = new StockfishBot();
        public void MakeMove(Board board)
        {
            string botmove = bot.GetMove(board);
            if (board.MakeMove(botmove) == false)
            {
                throw new InvalidDataException("Illegal move " + botmove + " " + board.ToString());
            }
        }
    }
}
