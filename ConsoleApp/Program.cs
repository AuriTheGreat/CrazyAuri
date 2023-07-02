using CrazyAuri;
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
            var ChessInstance = new ChessGameInstance("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR/ w KQkq - 0 1");
            ChessInstance.PrintBoard();
            ChessInstance.PrintAllMoves();
            ChessInstance.MakeMove("e2e4");
            ChessInstance.PrintBoard();
            ChessInstance.PrintAllMoves();
        }
    }
}
