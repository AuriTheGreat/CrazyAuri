using CrazyAuriLibrary.Models.Moves.MoveTypes;
using CrazyAuriLibrary.Models.Pieces;
using CrazyAuriLibrary.Models.Util;
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

        public override List<Move> GetMoves(Board board, BoardTableSet boardTableSet)
        {
            List<Move> result = new List<Move>();

            CheckMove(board, result, (1, 2), boardTableSet);
            CheckMove(board, result, (1, -2), boardTableSet);
            CheckMove(board, result, (-1, 2), boardTableSet);
            CheckMove(board, result, (-1, -2), boardTableSet);
            CheckMove(board, result, (2, 1), boardTableSet);
            CheckMove(board, result, (2, -1), boardTableSet);
            CheckMove(board, result, (-2, 1), boardTableSet);
            CheckMove(board, result, (-2, -1), boardTableSet);

            return result;
        }

        public override List<Move> GetCheckMoves(Board board, BoardTableSet boardTableSet)
        {
            List<Move> result = new List<Move>();

            CheckMoveIfCheck(board, result, (1, 2), boardTableSet);
            CheckMoveIfCheck(board, result, (1, -2), boardTableSet);
            CheckMoveIfCheck(board, result, (-1, 2), boardTableSet);
            CheckMoveIfCheck(board, result, (-1, -2), boardTableSet);
            CheckMoveIfCheck(board, result, (2, 1), boardTableSet);
            CheckMoveIfCheck(board, result, (2, -1), boardTableSet);
            CheckMoveIfCheck(board, result, (-2, 1), boardTableSet);
            CheckMoveIfCheck(board, result, (-2, -1), boardTableSet);

            return result;
        }

        private void CheckMoveIfCheck(Board board, List<Move> result, (short, short) direction, BoardTableSet boardTableSet)
        {
            int x = location.Item1 + direction.Item1;
            int y = location.Item2 + direction.Item2;

            if (x >= 8 || y >= 8 || x <= -1 || y <= -1)
                return;

            var checkRays = boardTableSet.checkRays;
            var squareAttackerDefenderCounts = boardTableSet.squareAttackerDefenderCounts;

            squareAttackerDefenderCounts[x, y] += 1;

            if (checkRays[x, y] == true)
            {
                CheckMove(board, result, direction, boardTableSet);
            }

        }

        private void CheckMove(Board board, List<Move> result, (short, short) direction, BoardTableSet boardTableSet)
        {
            int x = location.Item1 + direction.Item1;
            int y = location.Item2 + direction.Item2;

            var pinRays = boardTableSet.pinRays;
            var squareAttackerDefenderCounts = boardTableSet.squareAttackerDefenderCounts;

            short isPinned = pinRays[location.Item1, location.Item2];

            if (x >= 8 || y >= 8 || x <= -1 || y <= -1)
                return;

            squareAttackerDefenderCounts[x, y] += 1;

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

        public override void GetAttacks(Board board, BoardTableSet boardTableSet)
        {
            CheckAttackDirection(board, (1, 2), boardTableSet);
            CheckAttackDirection(board, (1, -2), boardTableSet);
            CheckAttackDirection(board, (-1, 2), boardTableSet);
            CheckAttackDirection(board, (-1, -2), boardTableSet);
            CheckAttackDirection(board, (2, 1), boardTableSet);
            CheckAttackDirection(board, (2, -1), boardTableSet);
            CheckAttackDirection(board, (-2, 1), boardTableSet);
            CheckAttackDirection(board, (-2, -1), boardTableSet);
        }

        private void CheckAttackDirection(Board board, (short, short) direction, BoardTableSet boardTableSet)
        {
            int x = location.Item1 + direction.Item1;
            int y = location.Item2 + direction.Item2;

            if (x >= 8 || y >= 8 || x <= -1 || y <= -1)
                return;

            var attackedSquares = boardTableSet.attackedSquares;
            var checkRays = boardTableSet.checkRays;
            var squareAttackerDefenderCounts = boardTableSet.squareAttackerDefenderCounts;

            attackedSquares[x, y] += 1;
            squareAttackerDefenderCounts[x, y] -= 1;
            boardTableSet.replaceSquareLowestAttackerPiece((x, y), this.ToString());
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
