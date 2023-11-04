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
    public class Bishop : DirectionalPiece
    {
        public Bishop(bool color, (int, int) location) : base(color, location)
        {
            acronym = "b";
        }

        public override List<Move> GetMoves(Board board, short[,] squareAttackerDefenderCounts, short[,] attackedSquares, short[,] pinRays)
        {
            List<Move> result = new List<Move>();
            result.AddRange(CheckDirection(board, (1, 1), squareAttackerDefenderCounts, pinRays));
            result.AddRange(CheckDirection(board, (1, -1), squareAttackerDefenderCounts, pinRays));
            result.AddRange(CheckDirection(board, (-1, 1), squareAttackerDefenderCounts, pinRays));
            result.AddRange(CheckDirection(board, (-1, -1), squareAttackerDefenderCounts, pinRays));
            return result;
        }

        public override List<Move> GetCheckMoves(Board board, short[,] squareAttackerDefenderCounts, short[,] attackedSquares, short[,] pinRays, bool[,] checkRays)
        {
            List<Move> result = new List<Move>();
            result.AddRange(CheckDirectionIfCheck(board, (1, 1), squareAttackerDefenderCounts, pinRays, checkRays));
            result.AddRange(CheckDirectionIfCheck(board, (1, -1), squareAttackerDefenderCounts, pinRays, checkRays));
            result.AddRange(CheckDirectionIfCheck(board, (-1, 1), squareAttackerDefenderCounts, pinRays, checkRays));
            result.AddRange(CheckDirectionIfCheck(board, (-1, -1), squareAttackerDefenderCounts, pinRays, checkRays));
            return result;
        }

        public override void MakeMove(Board board, Move move)
        {
            
        }

        public override void GetAttacks(Board board, short[,] squareAttackerDefenderCounts, short[,] attackedSquares, short[,] pinRays, bool[,] checkRays)
        {
            CheckAttackDirection(board, (1, 1), squareAttackerDefenderCounts, attackedSquares, pinRays, checkRays);
            CheckAttackDirection(board, (1, -1), squareAttackerDefenderCounts, attackedSquares, pinRays, checkRays);
            CheckAttackDirection(board, (-1, 1), squareAttackerDefenderCounts, attackedSquares, pinRays, checkRays);
            CheckAttackDirection(board, (-1, -1), squareAttackerDefenderCounts, attackedSquares, pinRays, checkRays);
        }

    }
}
