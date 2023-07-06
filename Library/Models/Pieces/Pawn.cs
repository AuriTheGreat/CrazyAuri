using CrazyAuriLibrary.Models.Moves.MoveTypes;
using CrazyAuriLibrary.Models.Pieces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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

        public override List<Move> GetMoves(Board board, short[,] attackedSquares, short[,] pinRays)
        {
            if ((location.Item1 == 1 && color == false) || (location.Item1 == 6 && color == true))
            {
                return GetPromotionMoves(board);
            }
            int x = location.Item1;
            int y = location.Item2;

            int colorint = color == true ? 1 : -1; // determines which direction to move the pawns in
            List<Move> result = new List<Move>();

            short isPinned = pinRays[x, y];

            var piece = board.GetPieceOnSquare((x + colorint * 1, y));
            if (piece == null)
            {
                if ((isPinned>0 && pinRays[x + colorint * 1, y] == isPinned) || isPinned==0)
                {
                    result.Add(new Move(this, location, (x + colorint * 1, y)));
                    if ((x == 1 && color == true) || (x == 6 && color == false))
                    {
                        piece = board.GetPieceOnSquare((x + colorint * 2, y));
                        if (piece == null)
                        {
                            if ((isPinned > 0 && pinRays[x + colorint * 2, y] == isPinned) || isPinned == 0)
                                result.Add(new Move(this, location, (x + colorint * 2, y)));
                        }
                    }
                }
            }

            if (y < 7)
            {
                if ((isPinned > 0 && pinRays[x + colorint * 1, y+1] == isPinned) || isPinned == 0)
                {
                    piece = board.GetPieceOnSquare((x + colorint * 1, y + 1));
                    if (piece != null)
                    {
                        result.Add(new CaptureMove(this, location, (x + colorint * 1, y + 1)));
                    }
                }
            }

            if (y > 0)
            {
                if ((isPinned >0 && pinRays[x + colorint * 1, y-1] == isPinned) || isPinned == 0)
                {
                    piece = board.GetPieceOnSquare((x + colorint * 1, y - 1));
                    if (piece != null)
                    {
                        result.Add(new CaptureMove(this, location, (x + colorint * 1, y - 1)));
                    }
                }
            }

            return result;

        }

        public override List<Move> GetCheckMoves(Board board, short[,] attackedSquares, short[,] pinRays, bool[,] checkRays)
        {
            List<Move> result = new List<Move>();
            foreach (var i in GetMoves(board, attackedSquares, pinRays))
            {
                if (checkRays[i.endsquare.Item1, i.endsquare.Item2] == true)
                {
                    result.Add(i);
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
            board.EnPassantSquare = move.endsquare;
        }

        public override void GetAttacks(Board board, short[,] attackedSquares, short[,] pinRays, bool[,] checkRays)
        {
            int x = location.Item1;
            int y = location.Item2;
            int colorint = color == true ? 1 : -1; // determines which direction to move the pawns in

            if (y < 7)
            {
                attackedSquares[x + colorint * 1, y + 1] += 1;
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
