using CrazyAuriLibrary.Models.Moves;
using CrazyAuriLibrary.Models.Pieces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyAuri.Models.Pieces
{
    public class King : Piece
    {
        public King(bool color, (int, int) location ) : base(color, location) 
        { 
            acronym = "k";
        }

        public override List<Move> GetMoves(Board board)
        {
            List<Move> result = new List<Move>();



            return result;
        }

        public override void MakeMove(Board board, Move move)
        {
            
        }

        public override void GetAttacks(Board board, short[,] attackedSquares, bool[,] pinRays)
        {

        }
    }
}
