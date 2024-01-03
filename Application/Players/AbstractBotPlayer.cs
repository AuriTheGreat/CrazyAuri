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
    abstract public class AbstractBotPlayer : IPlayer
    {
        public string InstructionMessage()
        {
            return "Please wait";
        }
        public abstract void MakeMove(Board board, BoardScreen screen);
        public abstract void ReserveTileActionsWhenCicked(Board board, BoardScreen screen, string identifier);
        public abstract void TileActionsWhenCicked(Board board, BoardScreen screen, ImageButton tile);
    }
}
