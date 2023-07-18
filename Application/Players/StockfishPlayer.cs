using CrazyAuri.Models;
using CrazyAuriAI;
using CrazyAuriAI.Bots;
using Myra.Graphics2D.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyAuriApplication.Players
{
    public class StockfishPlayer : IPlayer
    {
        public StockfishBot bot = new StockfishBot();

        public void MakeMove(Board board, BoardScreen screen)
        {
            string botmove = bot.GetMove(board);
            if (board.MakeMove(botmove) == false)
            {
                throw new InvalidDataException("Illegal move " + botmove + " " + board.ToString());
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
