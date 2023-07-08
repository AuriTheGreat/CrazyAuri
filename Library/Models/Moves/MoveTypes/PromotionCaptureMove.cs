using CrazyAuri.Models;
using CrazyAuri.Models.Pieces;
using CrazyAuriLibrary.Models.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyAuriLibrary.Models.Moves.MoveTypes
{
    public class PromotionCaptureMove : PromotionMove
    {
        public PromotionCaptureMove(Piece piece, (int, int) startsquare, (int, int) endsquare, string promotionpiece) : base(piece, startsquare, endsquare, promotionpiece)
        {
        }

        public override void MakeMove(Board board)
        {
            HandleCapture(board);
            base.MakeMove(board);

        }
    }
}
