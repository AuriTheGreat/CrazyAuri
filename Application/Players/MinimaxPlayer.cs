using CrazyAuri.Models;
using CrazyAuriAI;
using CrazyAuriAI.Bots;
using Myra.Graphics2D.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyAuriApplication.Players
{
    public class MinimaxPlayer : AbstractBotPlayer
    {
        public IBot bot = new MinimaxBot();

        public override void MakeMove(Board board, BoardScreen screen)
        {
            if (board.MakeMove(bot.GetMove(board)) == false)
            {
                board.MakeMove(board.GetAllMoves()[0]);
            }
            screen.drawboard.Update();
        }

        public override void ReserveTileActionsWhenCicked(Board board, BoardScreen screen, string identifier)
        {
        }

        public override void TileActionsWhenCicked(Board board, BoardScreen screen, ImageButton tile)
        {
        }
    }
}
