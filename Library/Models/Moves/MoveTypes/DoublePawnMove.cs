using CrazyAuri.Models;
using CrazyAuriLibrary.Models.Pieces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyAuriLibrary.Models.Moves.MoveTypes
{
    public class DoublePawnMove : Move
    {
        public DoublePawnMove(Piece piece, (int, int) startsquare, (int, int) endsquare) : base(piece, startsquare, endsquare)
        {
        }

        public override void MakeMove(Board board)
        {
            int colorint = piece.color == true ? 1 : -1;
            board.EnPassantSquare = (this.endsquare.Item1 - colorint, this.endsquare.Item2);

            base.MakeMove(board);
        }

    }
}
