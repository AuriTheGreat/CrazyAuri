using CrazyAuriLibrary.Models.Moves.MoveTypes;
using CrazyAuriLibrary.Models.Pieces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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

        public override List<Move> GetMoves(Board board, short[,] attackedSquares, short[,] pinRays)
        {
            List<Move> result = new List<Move>();

            CheckMove(board, result, attackedSquares, (1, 1));
            CheckMove(board, result, attackedSquares, (1, 0));
            CheckMove(board, result, attackedSquares, (1, -1));
            CheckMove(board, result, attackedSquares, (0, 1));
            CheckMove(board, result, attackedSquares, (0, -1));
            CheckMove(board, result, attackedSquares, (-1, 1));
            CheckMove(board, result, attackedSquares, (-1, 0));
            CheckMove(board, result, attackedSquares, (-1, -1));


            return result;
        }

        public override List<Move> GetCheckMoves(Board board, short[,] attackedSquares, short[,] pinRays, bool[,] checkRays)
        {
            return GetMoves(board, attackedSquares, pinRays);
        }

        private void CheckMove(Board board, List<Move> result, short[,] attackedSquares, (short, short) direction)
        {
            int x = location.Item1 + direction.Item1;
            int y = location.Item2 + direction.Item2;

            if (x == 8 || y == 8 || x == -1 || y == -1)
                return;

            if (attackedSquares[x, y] == 0)
            {
                var piece = board.GetPieceOnSquare((x, y));
                if (piece != null)
                {
                    if (piece.color != this.color)
                    {
                        result.Add(new CaptureMove(this, location, (x, y)));
                    }
                }
                else
                {
                    result.Add(new Move(this, location, (x, y)));
                }
            }
        }

        public override void MakeMove(Board board, Move move)
        {
            if (color == true)
            {
                board.CanBlackCastleKingside = false;
                board.CanBlackCastleQueenside = false;
            }
            else
            {
                board.CanWhiteCastleKingside = false;
                board.CanWhiteCastleQueenside = false;
            }
        }

        public override void GetAttacks(Board board, short[,] attackedSquares, short[,] pinRays, bool[,] checkRays)
        {
            CheckAttackDirection((1, 1), attackedSquares);
            CheckAttackDirection((1, 0), attackedSquares);
            CheckAttackDirection((1, -1), attackedSquares);
            CheckAttackDirection((0, 1), attackedSquares);
            CheckAttackDirection((0, -1), attackedSquares);
            CheckAttackDirection((-1, 1), attackedSquares);
            CheckAttackDirection((-1, 0), attackedSquares);
            CheckAttackDirection((-1, -1), attackedSquares);
        }

        private void CheckAttackDirection((short, short) direction, short[,] attackedSquares)
        {
            int x = location.Item1 + direction.Item1;
            int y = location.Item2 + direction.Item2;

            if (x == 8 || y == 8 || x == -1 || y == -1)
                return;
            attackedSquares[x, y] += 1;
        }
    }
}
