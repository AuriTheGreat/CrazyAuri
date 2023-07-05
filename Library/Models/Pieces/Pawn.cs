using CrazyAuriLibrary.Models.Moves.MoveTypes;
using CrazyAuriLibrary.Models.Pieces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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

        public override List<Move> GetMoves(Board board, short[,] attackedSquares, bool[,] pinRays, bool[,] checkRays)
        {
            if ((location.Item1 == 1 && color == false) || (location.Item1 == 6 && color == true))
            {
                return GetPromotionMoves(board);
            }
            int x = location.Item1;
            int y = location.Item2;

            int colorint = color == true ? 1 : -1; // determines which direction to move the pawns in
            List<Move> result = new List<Move>();

            bool isPinned = false;
            if (pinRays[x, y] == true)
                isPinned = true;

            var piece = board.GetPieceOnSquare((x + colorint * 1, y));
            if (piece == null)
            {
                if ((isPinned == true && pinRays[x + colorint * 1, y] == true) || isPinned == false)
                {
                    result.Add(new Move(this, location, (x + colorint * 1, y)));
                    if ((x == 1 && color == true) || (x == 6 && color == false))
                    {
                        piece = board.GetPieceOnSquare((x + colorint * 2, y));
                        if (piece == null)
                        {
                            if ((isPinned = true && pinRays[x + colorint * 2, y] == true) || isPinned == false)
                                result.Add(new Move(this, location, (x + colorint * 2, y)));
                        }
                    }
                }
            }

            if (y < 7)
            {
                if ((isPinned == true && pinRays[x + colorint * 1, y+1] == true) || isPinned == false)
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
                if ((isPinned == true && pinRays[x + colorint * 1, y-1] == true) || isPinned == false)
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
            board.EnPassantSquare = move.endsquare;
        }

        public override void GetAttacks(Board board, short[,] attackedSquares, bool[,] pinRays, bool[,] checkRays)
        {
            int x = location.Item1;
            int y = location.Item2;
            int colorint = color == true ? 1 : -1; // determines which direction to move the pawns in

            if (y < 7)
            {
                attackedSquares[x + colorint * 1, y + 1] += 1;
            }

            if (y > 0)
            {
                attackedSquares[x + colorint * 1, y - 1] += 1;
            }
        }
    }
}
