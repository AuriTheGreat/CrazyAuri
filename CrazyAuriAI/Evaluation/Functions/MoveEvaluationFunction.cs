using CrazyAuri.Models;
using CrazyAuriAI.Evaluation.PieceEvaluationSets;
using CrazyAuriLibrary.Models.Moves.MoveTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyAuriAI.Evaluation.Functions
{
    public class MoveEvaluationFunction
    {
        public readonly Dictionary<string, double> piecevalues = PieceEvaluationSet.piecevalues;
        public readonly Dictionary<string, double> reservepiecevalues = PieceEvaluationSet.reservepiecevalues;
        public readonly Dictionary<string, double[,]> tilepiecevalues = PieceEvaluationSet.tilepiecevalues;

        public double GetEvaluation(Board oldboard, Board newboard, Move move)
        {
            double result = 0;

            if (newboard.inCheck())
                result += 50;

            if (move is not CrazyhouseMove)
            {
                result += GetPieceSquareValue(move.piece.acronym, move.endsquare, oldboard.CurrentColor)
                    - GetPieceSquareValue(move.piece.acronym, move.startsquare, oldboard.CurrentColor) * 2;
            }

            if (move is CaptureMove && move is not CrazyhouseMove && move is not PromotionMove)
            {
                result += 20 -
                    GetPieceValue(oldboard.GetPieceOnSquare(move.startsquare).acronym) / 30 +
                    GetPieceValue(oldboard.GetPieceOnSquare(move.endsquare).acronym) / 5;
            }

            if (move is CrazyhouseMove)
            {
                result += GetPieceSquareValue(((CrazyhouseMove) move).placedPiece, move.endsquare, oldboard.CurrentColor)/20;
                var reserveevaluation = reservepiecevalues[((CrazyhouseMove)move).placedPiece];
                var mainevaluation = piecevalues[((CrazyhouseMove)move).placedPiece];
                result += (mainevaluation - reserveevaluation)/20;
            }

            if (move is PromotionMove)
            {
                result += 50;
                if (move is PromotionCaptureMove)
                    result += 50;
            }

            return Math.Max(result,0);
        }

        private double GetPieceValue(string piece)
        {
            if (piecevalues.ContainsKey(piece))
                return piecevalues[piece];
            return 0;
        }

        private double GetPieceSquareValue(string piece, (int, int) location, bool color)
        {
            if (tilepiecevalues.ContainsKey(piece))
            {
                if (color == true)
                {
                    return tilepiecevalues[piece][7 - location.Item1, location.Item2];
                }
                return tilepiecevalues[piece][location.Item1, location.Item2];
            }
            return 0;
        }
    }
}
