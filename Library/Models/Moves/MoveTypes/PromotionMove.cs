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
    public class PromotionMove : Move
    {
        string promotionPiece;

        public PromotionMove(Piece piece, (int, int) startsquare, (int, int) endsquare, string promotionpiece) : base(piece, startsquare, endsquare)
        {
            this.promotionPiece = promotionpiece;
        }

        public override void MakeMove(Board board)
        {
            board.HalfMoveClock = 0;
            var endpiece = board.GetPieceOnSquare(endsquare);
            switch (endpiece.ToString())
            {
                case "p":
                    board.WhiteCrazyHousePawns += 1;
                    break;
                case "n":
                    board.WhiteCrazyHouseKnights += 1;
                    break;
                case "b":
                    board.WhiteCrazyHouseBishops += 1;
                    break;
                case "r":
                    board.WhiteCrazyHouseRooks += 1;
                    break;
                case "q":
                    board.WhiteCrazyHouseQueens += 1;
                    break;
                case "P":
                    board.BlackCrazyHousePawns += 1;
                    break;
                case "N":
                    board.BlackCrazyHouseKnights += 1;
                    break;
                case "B":
                    board.BlackCrazyHouseBishops += 1;
                    break;
                case "R":
                    board.BlackCrazyHouseRooks += 1;
                    break;
                case "Q":
                    board.BlackCrazyHouseQueens += 1;
                    break;
            }

            if (board.CurrentColor == false)
            {
                board.WhitePieces.Remove(endpiece);
            }
            else
            {
                board.BlackPieces.Remove(endpiece);
            }

            base.MakeMove(board);
        }
    }
}
