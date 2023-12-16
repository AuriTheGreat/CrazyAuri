using CrazyAuriLibrary.Models.Moves.MoveTypes;
using CrazyAuriLibrary.Models.Pieces;
using CrazyAuriLibrary.Models.Util;
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

        public override List<Move> GetMoves(Board board, BoardTableSet boardTableSet)
        {
            List<Move> result = new List<Move>();
            result.AddRange(CheckDirection(board, (1, 0), boardTableSet));
            result.AddRange(CheckDirection(board, (-1, 0), boardTableSet));
            result.AddRange(CheckDirection(board, (0, 1), boardTableSet));
            result.AddRange(CheckDirection(board, (0, -1), boardTableSet));
            return result;
        }

        public override List<Move> GetCheckMoves(Board board, BoardTableSet boardTableSet)
        {
            List<Move> result = new List<Move>();
            result.AddRange(CheckDirectionIfCheck(board, (1, 0), boardTableSet));
            result.AddRange(CheckDirectionIfCheck(board, (-1, 0), boardTableSet));
            result.AddRange(CheckDirectionIfCheck(board, (0, 1), boardTableSet));
            result.AddRange(CheckDirectionIfCheck(board, (0, -1), boardTableSet));
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

        public override void GetAttacks(Board board, BoardTableSet boardTableSet)
        {
            CheckAttackDirection(board, (1, 0), boardTableSet);
            CheckAttackDirection(board, (-1, 0), boardTableSet);
            CheckAttackDirection(board, (0, 1), boardTableSet);
            CheckAttackDirection(board, (0, -1), boardTableSet);
        }
    }
}