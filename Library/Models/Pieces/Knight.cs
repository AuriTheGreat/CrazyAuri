using CrazyAuriLibrary.Models.Moves.MoveTypes;
using CrazyAuriLibrary.Models.Pieces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CrazyAuri.Models.Pieces
{
    public class Knight : PromotablePiece
    {
        public Knight(bool color, (int, int) location) : base(color, location)
        {
            acronym = "n";
        }

        public override List<Move> GetMoves(Board board, short[,] attackedSquares, short[,] pinRays)
        {
            List<Move> result = new List<Move>();

            CheckMove(board, result, (1, 2), pinRays);
            CheckMove(board, result, (1, -2), pinRays);
            CheckMove(board, result, (-1, 2), pinRays);
            CheckMove(board, result, (-1, -2), pinRays);
            CheckMove(board, result, (2, 1), pinRays);
            CheckMove(board, result, (2, -1), pinRays);
            CheckMove(board, result, (-2, 1), pinRays);
            CheckMove(board, result, (-2, -1), pinRays);

            return result;
        }

        public override List<Move> GetCheckMoves(Board board, short[,] attackedSquares, short[,] pinRays, bool[,] checkRays)
        {
            List<Move> result = new List<Move>();

            CheckMoveIfCheck(board, result, (1, 2), pinRays, checkRays);
            CheckMoveIfCheck(board, result, (1, -2), pinRays, checkRays);
            CheckMoveIfCheck(board, result, (-1, 2), pinRays, checkRays);
            CheckMoveIfCheck(board, result, (-1, -2), pinRays, checkRays);
            CheckMoveIfCheck(board, result, (2, 1), pinRays, checkRays);
            CheckMoveIfCheck(board, result, (2, -1), pinRays, checkRays);
            CheckMoveIfCheck(board, result, (-2, 1), pinRays, checkRays);
            CheckMoveIfCheck(board, result, (-2, -1), pinRays, checkRays);

            return result;
        }

        private void CheckMoveIfCheck(Board board, List<Move> result, (short, short) direction, short[,] pinRays, bool[,] checkRays)
        {
            int x = location.Item1 + direction.Item1;
            int y = location.Item2 + direction.Item2;

            if (x >= 8 || y >= 8 || x <= -1 || y <= -1)
                return;

            if (checkRays[x, y] == true)
            {
                CheckMove(board, result, direction, pinRays);
            }

        }

        private void CheckMove(Board board, List<Move> result, (short, short) direction, short[,] pinRays)
        {
            int x = location.Item1 + direction.Item1;
            int y = location.Item2 + direction.Item2;

            short isPinned = pinRays[location.Item1, location.Item2];

            if (x >= 8 || y >= 8 || x <= -1 || y <= -1)
                return;

            if (isPinned > 0)
            {
                if (pinRays[x, y] != isPinned)
                    return;
            }

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

        public override void MakeMove(Board board, Move move)
        {
            
        }

        public override void GetAttacks(Board board, short[,] attackedSquares, short[,] pinRays, bool[,] checkRays)
        {
            CheckAttackDirection(board, (1, 2), attackedSquares, checkRays);
            CheckAttackDirection(board, (1, -2), attackedSquares, checkRays);
            CheckAttackDirection(board, (-1, 2), attackedSquares, checkRays);
            CheckAttackDirection(board, (-1, -2), attackedSquares, checkRays);
            CheckAttackDirection(board, (2, 1), attackedSquares, checkRays);
            CheckAttackDirection(board, (2, -1), attackedSquares, checkRays);
            CheckAttackDirection(board, (-2, 1), attackedSquares, checkRays);
            CheckAttackDirection(board, (-2, -1), attackedSquares, checkRays);
        }

        private void CheckAttackDirection(Board board, (short, short) direction, short[,] attackedSquares, bool[,] checkRays)
        {
            int x = location.Item1 + direction.Item1;
            int y = location.Item2 + direction.Item2;

            if (x >= 8 || y >= 8 || x <= -1 || y <= -1)
                return;
            attackedSquares[x, y] += 1;
            var piece = board.GetPieceOnSquare((x, y));
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
