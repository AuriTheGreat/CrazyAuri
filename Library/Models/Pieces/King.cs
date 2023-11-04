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

        public override List<Move> GetMoves(Board board, short[,] squareAttackerDefenderCounts, short[,] attackedSquares, short[,] pinRays)
        {
            List<Move> result = new List<Move>();

            CheckMove(board, result, squareAttackerDefenderCounts, attackedSquares, (1, 1));
            CheckMove(board, result, squareAttackerDefenderCounts, attackedSquares, (1, 0));
            CheckMove(board, result, squareAttackerDefenderCounts, attackedSquares, (1, -1));
            CheckMove(board, result, squareAttackerDefenderCounts, attackedSquares, (0, 1));
            CheckMove(board, result, squareAttackerDefenderCounts, attackedSquares, (0, -1));
            CheckMove(board, result, squareAttackerDefenderCounts, attackedSquares, (-1, 1));
            CheckMove(board, result, squareAttackerDefenderCounts, attackedSquares, (-1, 0));
            CheckMove(board, result, squareAttackerDefenderCounts, attackedSquares, (-1, -1));

            result.AddRange(GetCastlingMoves(board, attackedSquares));


            return result;
        }

        public override List<Move> GetCheckMoves(Board board, short[,] squareAttackerDefenderCounts, short[,] attackedSquares, short[,] pinRays, bool[,] checkRays)
        {
            return GetMoves(board, squareAttackerDefenderCounts, attackedSquares, pinRays);
        }

        private void CheckMove(Board board, List<Move> result, short[,] squareAttackerDefenderCounts, short[,] attackedSquares, (short, short) direction)
        {
            int x = location.Item1 + direction.Item1;
            int y = location.Item2 + direction.Item2;

            if (x == 8 || y == 8 || x == -1 || y == -1)
                return;

            squareAttackerDefenderCounts[x, y] += 1;

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

        private List<Move> GetCastlingMoves(Board board, short[,] attackedSquares)
        {
            List<Move> result = new List<Move>();

            if (attackedSquares[location.Item1, location.Item2] > 0)
                return result;

            if (this == board.WhiteKing)
            {
                //e1g1
                var neededsquares = new List<(int, int)>() { (7, 5), (7,6) };
                if (board.CanWhiteCastleKingside==true && CheckIfPossibleToCastle(board, attackedSquares, neededsquares) == true)
                {
                    result.Add(new CastlingMove(this, location, (7, 6)));
                }
                neededsquares= new List<(int, int)>() { (7, 2), (7, 3) };
                if (board.CanWhiteCastleQueenside == true && board.GetPieceOnSquare((7, 1)) == null && CheckIfPossibleToCastle(board, attackedSquares, neededsquares) == true)
                {
                    result.Add(new CastlingMove(this, location, (7, 2)));
                }

            }
            else
            {
                var neededsquares = new List<(int, int)>() { (0, 5), (0, 6) };
                if (board.CanBlackCastleKingside == true && CheckIfPossibleToCastle(board, attackedSquares, neededsquares) == true)
                {
                    result.Add(new CastlingMove(this, location, (0, 6)));
                }
                neededsquares = new List<(int, int)>() { (0, 2), (0, 3) };
                if (board.CanBlackCastleQueenside == true && board.GetPieceOnSquare((0,1)) == null && CheckIfPossibleToCastle(board, attackedSquares, neededsquares) == true)
                {
                    result.Add(new CastlingMove(this, location, (0, 2)));
                }
            }

            return result;

        }

        private Boolean CheckIfPossibleToCastle(Board board, short[,] attackedSquares, List<(int, int)> neededSquares)
        {
            foreach (var square in neededSquares)
            {
                if (board.GetPieceOnSquare(square)!=null || attackedSquares[square.Item1, square.Item2] > 0)
                    return false;
            }

            return true;

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

        public override void GetAttacks(Board board, short[,] squareAttackerDefenderCounts, short[,] attackedSquares, short[,] pinRays, bool[,] checkRays)
        {
            CheckAttackDirection((1, 1), squareAttackerDefenderCounts, attackedSquares);
            CheckAttackDirection((1, 0), squareAttackerDefenderCounts, attackedSquares);
            CheckAttackDirection((1, -1), squareAttackerDefenderCounts, attackedSquares);
            CheckAttackDirection((0, 1), squareAttackerDefenderCounts, attackedSquares);
            CheckAttackDirection((0, -1), squareAttackerDefenderCounts, attackedSquares);
            CheckAttackDirection((-1, 1), squareAttackerDefenderCounts, attackedSquares);
            CheckAttackDirection((-1, 0), squareAttackerDefenderCounts, attackedSquares);
            CheckAttackDirection((-1, -1), squareAttackerDefenderCounts, attackedSquares);
        }

        private void CheckAttackDirection((short, short) direction, short[,] squareAttackerDefenderCounts, short[,] attackedSquares)
        {
            int x = location.Item1 + direction.Item1;
            int y = location.Item2 + direction.Item2;

            if (x == 8 || y == 8 || x == -1 || y == -1)
                return;
            attackedSquares[x, y] += 1;
            squareAttackerDefenderCounts[x, y] -= 1;
        }
    }
}
