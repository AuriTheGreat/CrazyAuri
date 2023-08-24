using CrazyAuri.Models;
using CrazyAuriAI;
using CrazyAuriAI.Models;
using Myra.Graphics2D.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyAuriApplication.Players
{
    public class BotPlayer : IPlayer
    {
        public MonteCarloBot bot = new MonteCarloBot();

        public void MakeMove(Board board, BoardScreen screen)
        {
            if (board.MakeMove(bot.GetMove(board)) == false)
            {
                board.MakeMove(board.GetAllMoves()[0]);
            }
            screen.drawboard.Update();
        }

        public void ReserveTileActionsWhenCicked(Board board, BoardScreen screen, string identifier)
        {
        }

        public void TileActionsWhenCicked(Board board, BoardScreen screen, ImageButton tile)
        {
        }
    }
}
