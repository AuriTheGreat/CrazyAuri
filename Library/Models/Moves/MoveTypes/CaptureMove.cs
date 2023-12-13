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
    public class CaptureMove : Move
    {
        public CaptureMove(Piece piece, (int, int) startsquare, (int, int) endsquare) : base(piece, startsquare, endsquare)
        {
        }

        public override void MakeMove(Board board)
        {
            board.HalfMoveClock = 0;

            HandleCapture(board);

            base.MakeMove(board);
        }

        protected void HandleCapture(Board board)
        {
            var endpiece = board.GetPieceOnSquare(endsquare);
            if (endpiece is PromotablePiece)
            {
                AddToCrazyHouseReserve(board, (PromotablePiece)endpiece);
            }
            else
            {
                AddToCrazyHouseReserve(board, endpiece);
            }

            if (endpiece.acronym.ToLower() == "r")
            {
                if (endpiece.location == (0, 0))
                    board.CanBlackCastleQueenside = false;
                if (endpiece.location == (0, 7))
                    board.CanBlackCastleKingside = false;
                if (endpiece.location == (7, 0))
                    board.CanWhiteCastleQueenside = false;
                if (endpiece.location == (7, 7))
                    board.CanWhiteCastleKingside = false;
            }

            if (board.CurrentColor == false)
            {
                board.WhitePieces.Remove(endpiece);
            }
            else
            {
                board.BlackPieces.Remove(endpiece);
            }
        }

        private void AddToCrazyHouseReserve(Board board, PromotablePiece piece)
        {
            if (piece.promoted == true)
            {
                if (board.CurrentColor == false)
                {
                    board.BlackCrazyHousePawns += 1;
                }
                else
                {
                    board.WhiteCrazyHousePawns += 1;
                }
            }
            else
            {
                AddToCrazyHouseReserve(board, (Piece) piece);
            }
        }
        private void AddToCrazyHouseReserve(Board board, Piece piece)
        {
            switch (piece.ToString())
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
        }

    }
}
