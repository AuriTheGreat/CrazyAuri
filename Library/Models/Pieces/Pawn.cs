using CrazyAuriLibrary.Models.Moves.MoveTypes;
using CrazyAuriLibrary.Models.Pieces;
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

        public override List<Move> GetMoves(Board board, short[,] squareAttackerDefenderCounts, short[,] attackedSquares, short[,] pinRays)
        {
            int x = location.Item1;
            int y = location.Item2;

            int colorint = color == true ? 1 : -1; // determines which direction to move the pawns in
            List<Move> result = new List<Move>();

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
                result.AddRange(GetEnPassantMoves(board, squareAttackerDefenderCounts, pinRays));
            }

            return result;

        }

        public override List<Move> GetCheckMoves(Board board, short[,] squareAttackerDefenderCounts, short[,] attackedSquares, short[,] pinRays, bool[,] checkRays)
        {
            List<Move> result = new List<Move>();
            foreach (var i in GetMoves(board, attackedSquares, squareAttackerDefenderCounts, pinRays))
            {
                if (checkRays[i.endsquare.Item1, i.endsquare.Item2] == true)
                {
                    result.Add(i);
                }
            }
            return result;
        }

        private List<Move> GetEnPassantMoves(Board board, short[,] squareAttackerDefenderCounts, short[,] pinRays)
        {
            int x = location.Item1;
            int y = location.Item2;
            int colorint = color == true ? 1 : -1; // determines which direction to move the pawns in
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

        public override void GetAttacks(Board board, short[,] squareAttackerDefenderCounts, short[,] attackedSquares, short[,] pinRays, bool[,] checkRays)
        {
            int x = location.Item1;
            int y = location.Item2;
            int colorint = color == true ? 1 : -1; // determines which direction to move the pawns in

            if (y < 7)
            {
                attackedSquares[x + colorint * 1, y + 1] += 1;
                squareAttackerDefenderCounts[x + colorint * 1, y + 1] -= 1;
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
