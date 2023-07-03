using CrazyAuriLibrary.Models.Moves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyAuri.Models.Pieces
{
    public interface IPiece
    {
        bool color { get; set; } // white is false, black is true

        public List<Move> GetMoves(Board board);
        public void MakeMove(Board board, Move move);
        public void GetAttacks(Board board, short[,] attackedSquares, bool[,] pinRays);

    }
}
