using CrazyAuriLibrary.Models.Moves.MoveTypes;
using CrazyAuriLibrary.Models.Pieces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CrazyAuri.Models.Pieces
{
    public class Rook : DirectionalPiece
    {
        public Rook(bool color, (int, int) location) : base(color, location)
        {
            acronym = "r";
        }

        public override List<Move> GetMoves(Board board, short[,] squareAttackerDefenderCounts, short[,] attackedSquares, short[,] pinRays)
        {
            List<Move> result = new List<Move>();
            result.AddRange(CheckDirection(board, (1, 0), squareAttackerDefenderCounts, pinRays));
            result.AddRange(CheckDirection(board, (-1, 0), squareAttackerDefenderCounts, pinRays));
            result.AddRange(CheckDirection(board, (0, 1), squareAttackerDefenderCounts, pinRays));
            result.AddRange(CheckDirection(board, (0, -1), squareAttackerDefenderCounts, pinRays));
            return result;
        }

        public override List<Move> GetCheckMoves(Board board, short[,] squareAttackerDefenderCounts, short[,] attackedSquares, short[,] pinRays, bool[,] checkRays)
        {
            List<Move> result = new List<Move>();
            result.AddRange(CheckDirectionIfCheck(board, (1, 0), squareAttackerDefenderCounts, pinRays, checkRays));
            result.AddRange(CheckDirectionIfCheck(board, (-1, 0), squareAttackerDefenderCounts, pinRays, checkRays));
            result.AddRange(CheckDirectionIfCheck(board, (0, 1), squareAttackerDefenderCounts, pinRays, checkRays));
            result.AddRange(CheckDirectionIfCheck(board, (0, -1), squareAttackerDefenderCounts, pinRays, checkRays));
            return result;
        }

        public override void MakeMove(Board board, Move move)
        {
            if (color == true)
            {
                if (move.startsquare == (0, 0))
                {
                    board.CanBlackCastleQueenside = false;
                }
                else if (move.startsquare == (0, 7))
                {
                    board.CanBlackCastleKingside = false;
                }
            }
            else
            {
                if (move.startsquare == (7, 0))
                {
                    board.CanWhiteCastleQueenside = false;
                }
                else if (move.startsquare == (7, 7))
                {
                    board.CanWhiteCastleKingside = false;
                }
            }
            
        }

        public override void GetAttacks(Board board, short[,] squareAttackerDefenderCounts, short[,] attackedSquares, short[,] pinRays, bool[,] checkRays)
        {
            CheckAttackDirection(board, (1, 0), squareAttackerDefenderCounts, attackedSquares, pinRays, checkRays);
            CheckAttackDirection(board, (-1, 0), squareAttackerDefenderCounts,  attackedSquares, pinRays, checkRays);
            CheckAttackDirection(board, (0, 1), squareAttackerDefenderCounts,  attackedSquares, pinRays, checkRays);
            CheckAttackDirection(board, (0, -1), squareAttackerDefenderCounts, attackedSquares, pinRays, checkRays);
        }
    }
}