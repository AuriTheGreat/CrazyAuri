using CrazyAuriLibrary.Models.Moves.MoveTypes;
using CrazyAuriLibrary.Models.Pieces;
using CrazyAuriLibrary.Models.Util;
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

        public override List<Move> GetMoves(Board board, BoardTableSet boardTableSet)
        {
            List<Move> result = new List<Move>();
            result.AddRange(CheckDirection(board, (1, 1), boardTableSet));
            result.AddRange(CheckDirection(board, (1, -1), boardTableSet));
            result.AddRange(CheckDirection(board, (-1, 1), boardTableSet));
            result.AddRange(CheckDirection(board, (-1, -1), boardTableSet));
            return result;
        }

        public override List<Move> GetCheckMoves(Board board, BoardTableSet boardTableSet)
        {
            List<Move> result = new List<Move>();
            result.AddRange(CheckDirectionIfCheck(board, (1, 1), boardTableSet));
            result.AddRange(CheckDirectionIfCheck(board, (1, -1), boardTableSet));
            result.AddRange(CheckDirectionIfCheck(board, (-1, 1), boardTableSet));
            result.AddRange(CheckDirectionIfCheck(board, (-1, -1), boardTableSet));
            return result;
        }

        public override void MakeMove(Board board, Move move)
        {
            
        }

        public override void GetAttacks(Board board, BoardTableSet boardTableSet)
        {
            CheckAttackDirection(board, (1, 1), boardTableSet);
            CheckAttackDirection(board, (1, -1), boardTableSet);
            CheckAttackDirection(board, (-1, 1), boardTableSet);
            CheckAttackDirection(board, (-1, -1), boardTableSet);
        }

    }
}
