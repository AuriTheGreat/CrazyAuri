using CrazyAuriApplication.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyAuriApplication.Models
{
    public static class Settings
    {
        public static IPlayer WhitePlayer = new HumanPlayer();

        public static IPlayer BlackPlayer = new MinimaxPlayer();

        public static string StartBoard = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR/ w KQkq - 0 1";

    }
}
