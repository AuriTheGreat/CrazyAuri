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
    public class Knight : Piece
    {
        public Knight(bool color, (int, int) location) : base(color, location)
        {
            acronym = "n";
        }

        public override List<Move> GetMoves(Board board, short[,] attackedSquares, bool[,] pinRays, bool[,] checkRays)
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

        private void CheckMove(Board board, List<Move> result, (short, short) direction, bool[,] pinRays)
        {
            int x = location.Item1 + direction.Item1;
            int y = location.Item2 + direction.Item2;

            bool isPinned = false;
            if (pinRays[location.Item1, location.Item2] == true)
                isPinned = true;

            if (x >= 8 || y >= 8 || x <= -1 || y <= -1)
                return;

            if (isPinned == true)
            {
                if (pinRays[x, y] == false)
                    return;
            }

            var piece = board.GetPieceOnSquare((x, y));
            if (piece != null)
            {
                if (piece.color != this.color)
                {
                    result.Add(new Move(this, location, (x, y)));
                }
                else
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

        public override void GetAttacks(Board board, short[,] attackedSquares, bool[,] pinRays, bool[,] checkRays)
        {
            CheckAttackDirection((1, 2), attackedSquares, checkRays);
            CheckAttackDirection((1, -2), attackedSquares, checkRays);
            CheckAttackDirection((-1, 2), attackedSquares, checkRays);
            CheckAttackDirection((-1, -2), attackedSquares, checkRays);
            CheckAttackDirection((2, 1), attackedSquares, checkRays);
            CheckAttackDirection((2, -1), attackedSquares, checkRays);
            CheckAttackDirection((-2, 1), attackedSquares, checkRays);
            CheckAttackDirection((-2, -1), attackedSquares, checkRays);
        }

        private void CheckAttackDirection((short, short) direction, short[,] attackedSquares, bool[,] checkRays)
        {
            int x = location.Item1 + direction.Item1;
            int y = location.Item2 + direction.Item2;

            if (x >= 8 || y >= 8 || x <= -1 || y <= -1)
                return;
            attackedSquares[x, y] += 1;
        }
    }
}
