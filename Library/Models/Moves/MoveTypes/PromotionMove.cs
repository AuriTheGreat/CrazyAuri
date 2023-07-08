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
    public class PromotionMove : CaptureMove
    {
        string promotionPiece;

        public PromotionMove(Piece piece, (int, int) startsquare, (int, int) endsquare, string promotionpiece) : base(piece, startsquare, endsquare)
        {
            this.promotionPiece = promotionpiece;
        }

        public override string ToString()
        {
            return GetMoveName() + promotionPiece;
        }

        public override void MakeMove(Board board)
        {
            int x1 = startsquare.Item1;
            int x2 = endsquare.Item1;
            int y1 = startsquare.Item2;
            int y2 = endsquare.Item2;

            Piece newpiece= new Queen(piece.color, (x2, y2));

            switch (promotionPiece)
            {
                case "r":
                    newpiece = new Rook(piece.color, (x2, y2));
                    break;
                case "b":
                    newpiece = new Bishop(piece.color, (x2, y2));
                    break;
                case "n":
                    newpiece = new Knight(piece.color, (x2, y2));
                    break;
            }

            var startpiece = board.GetPieceOnSquare(startsquare);
            if (board.CurrentColor == true)
            {
                board.WhitePieces.Remove(startpiece);
                board.WhitePieces.Add(newpiece);
            }
            else
            {
                board.BlackPieces.Remove(startpiece);
                board.BlackPieces.Add(newpiece);
            }

            board.HalfMoveClock = 0;

            board.array[x2, y2] = newpiece;
            board.array[x1, y1] = null;
        }
    }
}
