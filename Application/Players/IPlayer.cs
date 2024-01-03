using CrazyAuri.Models;
using Myra.Graphics2D.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyAuriApplication.Players
{
    public interface IPlayer
    {
        public void TileActionsWhenCicked(Board board, BoardScreen screen, ImageButton tile);
        public void ReserveTileActionsWhenCicked(Board board, BoardScreen screen, string identifier);
        public void MakeMove(Board board, BoardScreen screen);
        public string InstructionMessage();
    }
}
