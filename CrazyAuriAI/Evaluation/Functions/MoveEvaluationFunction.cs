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
            bool isCheck = newboard.inCheck();
            int squareAttackerDifference = oldboard.GetAttackingPieceDifferenceOnSquare(move.endsquare)-1;

            if (isCheck)
                result += 60;

            if (move is not CrazyhouseMove)
            {
                // Moves hanging pieces away from threat.
                if (oldboard.GetAttackingPieceDifferenceOnSquare(move.startsquare) < 0)
                    if (squareAttackerDifference >= 0)
                        result += 80;

                // Tries to place pieces on better squares
                result += GetPieceSquareValue(move.piece.acronym, move.endsquare, oldboard.CurrentColor)
                    - GetPieceSquareValue(move.piece.acronym, move.startsquare, oldboard.CurrentColor) * 2;
            }

            if (move is CaptureMove && move is not CrazyhouseMove && move is not PromotionMove)
            {
                // Capture bonus
                result += 20;

                // Tries to capture if there are too few defenders.
                var startSquarePieceValue = GetPieceValue(oldboard.GetPieceOnSquare(move.startsquare).acronym);
                var endSquarePieceValue = GetPieceValue(oldboard.GetPieceOnSquare(move.endsquare).acronym);

                if (squareAttackerDifference >= 0)
                    result += endSquarePieceValue*3;
                else if (endSquarePieceValue > startSquarePieceValue) // Captures if enemy piece has higher value than ours
                    result += (endSquarePieceValue - startSquarePieceValue)*5;
                else
                    result += overProtectedSquarePenalty(squareAttackerDifference, isCheck);
            }
            else
            {
                result += overProtectedSquarePenalty(squareAttackerDifference, isCheck);
            }

            if (move is CrazyhouseMove)
            {
                // Tries to place pieces from reserve on good squares
                result += GetPieceSquareValue(((CrazyhouseMove) move).placedPiece, move.endsquare, oldboard.CurrentColor)/20;
                var reserveevaluation = reservepiecevalues[((CrazyhouseMove)move).placedPiece];
                var mainevaluation = piecevalues[((CrazyhouseMove)move).placedPiece];
                result += (mainevaluation - reserveevaluation)/18;
                if (isCheck)
                    result+= 10;
            }

            if (move is PromotionMove)
            {
                // Encourages promotion moves
                result += 130;
                if (move is PromotionCaptureMove)
                    result += 150;
            }

            return Math.Max(result,0);
        }

        private double overProtectedSquarePenalty(int squareAttackerDifference, bool isCheck)
        {
            // Avoids squares, where piece would be instantly captured by an opponent
            if (squareAttackerDifference < 0)
            {
                if (isCheck)
                    return -80;
                else
                    return -550;
            }
            return 0;
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
