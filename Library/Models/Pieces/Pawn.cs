using CrazyAuriLibrary.Models.Moves.MoveTypes;
using CrazyAuriLibrary.Models.Pieces;
using CrazyAuriLibrary.Models.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Threading.Tasks;

namespace CrazyAuri.Models.Pieces
{
    public class Pawn : Piece
    {
        public Pawn(bool color, (int, int) location) : base(color, location)
        {
            acronym = "p";
        }

        public override List<Move> GetMoves(Board board, BoardTableSet boardTableSet)
        {
            int x = location.Item1;
            int y = location.Item2;

            int colorint = color == true ? 1 : -1; // determines which direction to move the pawns in
            List<Move> result = new List<Move>();

            var pinRays = boardTableSet.pinRays;
            var squareAttackerDefenderCounts = boardTableSet.squareAttackerDefenderCounts;

            short isPinned = pinRays[x, y];

            var checkedsquare = (x + colorint * 1, y);

            var piece = board.GetPieceOnSquare(checkedsquare);
            if (piece == null)
            {
                if ((isPinned>0 && pinRays[checkedsquare.Item1, checkedsquare.Item2] == isPinned) || isPinned==0)
                {
                    if ((x == 1 && color == false) || (x == 6 && color == true))
                    {
                        result.Add(new PromotionMove(this, location, checkedsquare, "q"));
                        result.Add(new PromotionMove(this, location, checkedsquare, "r"));
                        result.Add(new PromotionMove(this, location, checkedsquare, "b"));
                        result.Add(new PromotionMove(this, location, checkedsquare, "n"));
                    }
                    else
                    {
                        result.Add(new Move(this, location, checkedsquare));
                        if ((x == 1 && color == true) || (x == 6 && color == false))
                        {
                            checkedsquare = (x + colorint * 2, y);
                            piece = board.GetPieceOnSquare(checkedsquare);
                            if (piece == null)
                            {
                                if ((isPinned > 0 && pinRays[checkedsquare.Item1, checkedsquare.Item2] == isPinned) || isPinned == 0)
                                    result.Add(new DoublePawnMove(this, location, checkedsquare));
                            }
                        }
                    }
                }
            }

            if (y < 7)
            {
                checkedsquare = (x + colorint * 1, y + 1);
                squareAttackerDefenderCounts[checkedsquare.Item1, checkedsquare.Item2] += 1;
                if ((isPinned > 0 && pinRays[checkedsquare.Item1, checkedsquare.Item2] == isPinned) || isPinned == 0)
                {
                    piece = board.GetPieceOnSquare(checkedsquare);
                    if (piece != null && piece.color != this.color)
                    {
                        if ((x == 1 && color == false) || (x == 6 && color == true))
                        {
                            result.Add(new PromotionCaptureMove(this, location, checkedsquare, "q"));
                            result.Add(new PromotionCaptureMove(this, location, checkedsquare, "r"));
                            result.Add(new PromotionCaptureMove(this, location, checkedsquare, "b"));
                            result.Add(new PromotionCaptureMove(this, location, checkedsquare, "n"));
                        }
                        else
                        {
                            result.Add(new CaptureMove(this, location, checkedsquare));
                        }
                    }
                }
            }

            if (y > 0)
            {
                checkedsquare = (x + colorint * 1, y - 1);
                squareAttackerDefenderCounts[checkedsquare.Item1, checkedsquare.Item2] += 1;
                if ((isPinned > 0 && pinRays[checkedsquare.Item1, checkedsquare.Item2] == isPinned) || isPinned == 0)
                {
                    piece = board.GetPieceOnSquare(checkedsquare);
                    if (piece != null && piece.color!=this.color)
                    {
                        if ((x == 1 && color == false) || (x == 6 && color == true))
                        {
                            result.Add(new PromotionCaptureMove(this, location, checkedsquare, "q"));
                            result.Add(new PromotionCaptureMove(this, location, checkedsquare, "r"));
                            result.Add(new PromotionCaptureMove(this, location, checkedsquare, "b"));
                            result.Add(new PromotionCaptureMove(this, location, checkedsquare, "n"));
                        }
                        else
                        {
                            result.Add(new CaptureMove(this, location, checkedsquare));
                        }
                    }
                }
            }

            if(board.EnPassantSquare!=(-1, -1))
            {
                result.AddRange(GetEnPassantMoves(board, boardTableSet));
            }

            return result;

        }

        public override List<Move> GetCheckMoves(Board board, BoardTableSet boardTableSet)
        {
            List<Move> result = new List<Move>();
            var checkRays = boardTableSet.checkRays;
            foreach (var i in GetMoves(board, boardTableSet))
            {
                if (checkRays[i.endsquare.Item1, i.endsquare.Item2] == true)
                {
                    result.Add(i);
                }
            }
            return result;
        }

        private List<Move> GetEnPassantMoves(Board board, BoardTableSet boardTableSet)
        {
            int x = location.Item1;
            int y = location.Item2;
            int colorint = color == true ? 1 : -1; // determines which direction to move the pawns in
            var pinRays = boardTableSet.pinRays;
            short isPinned = pinRays[x, y];
            List<Move> result = new List<Move>();
            if (board.EnPassantSquare == (x + colorint, y + 1))
            {
                if ((isPinned > 0 && pinRays[x + colorint * 1, y + 1] == isPinned) || isPinned == 0)
                {
                    result.Add(new EnPassantMove(this, location, (x + colorint, y + 1)));
                }
            }
            else if (board.EnPassantSquare == (x + colorint, y - 1))
            {
                if ((isPinned > 0 && pinRays[x + colorint * 1, y - 1] == isPinned) || isPinned == 0)
                {
                    result.Add(new EnPassantMove(this, location, (x + colorint, y - 1)));
                }
            }

            return result;
        }

        private List<Move> GetPromotionMoves(Board board)
        {
            int x = location.Item1;
            int y = location.Item2;
            int colorint = color == true ? 1 : -1; // determines which direction to move the pawns in
            List<Move> result = new List<Move>();

            return result;
        }

        public override void MakeMove(Board board, Move move)
        {
            board.HalfMoveClock = 0;
        }

        public override void GetAttacks(Board board, BoardTableSet boardTableSet)
        {
            int x = location.Item1;
            int y = location.Item2;
            int colorint = color == true ? 1 : -1; // determines which direction to move the pawns in

            var attackedSquares = boardTableSet.attackedSquares;
            var checkRays = boardTableSet.checkRays;
            var squareAttackerDefenderCounts = boardTableSet.squareAttackerDefenderCounts;

            if (y < 7)
            {
                attackedSquares[x + colorint * 1, y + 1] += 1;
                squareAttackerDefenderCounts[x + colorint * 1, y + 1] -= 1;
                boardTableSet.replaceSquareLowestAttackerPiece((x + colorint * 1, y + 1), this.ToString());
                var piece = board.GetPieceOnSquare((x + colorint * 1, y + 1));
                if (piece != null)
                {
                    if ((color == true && piece == board.WhiteKing) || (color == false && piece == board.BlackKing))
                    {
                        checkRays[location.Item1, location.Item2] = true;
                    }
                }
            }

            if (y > 0)
            {
                attackedSquares[x + colorint * 1, y - 1] += 1;
                squareAttackerDefenderCounts[x + colorint * 1, y - 1] -= 1;
                boardTableSet.replaceSquareLowestAttackerPiece((x + colorint * 1, y - 1), this.ToString());
                var piece = board.GetPieceOnSquare((x + colorint * 1, y - 1));
                if (piece != null)
                {
                    if ((color == true && piece == board.WhiteKing) || (color == false && piece == board.BlackKing))
                    {
                        checkRays[location.Item1, location.Item2] = true;
                    }
                }
            }
        }
    }
}
