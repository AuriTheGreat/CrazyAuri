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
            int squareAttackerDifference = oldboard.GetAttackingPieceDifferenceOnSquare(move.endsquare);

            if (move is not CrazyhouseMove)
            {
                if (newboard.inCheck())
                    result += 60;

                if (move.piece.acronym == "k")
                    result -= 30;
                if (move is CastlingMove)
                    result += 80;

                // Moves pieces that are attacked by lower pieces
                string squareLowestAttackerPiece = oldboard.GetSquareLowestAttackerPiece(move.startsquare);
                if (squareLowestAttackerPiece != null)
                {
                    result += Math.Max(GetPieceValue(oldboard.GetPieceOnSquare(move.startsquare).acronym)
                    - GetPieceValue(squareLowestAttackerPiece), 0);
                }

                // Does not place pieces where lower piece can capture them
                squareLowestAttackerPiece = oldboard.GetSquareLowestAttackerPiece(move.endsquare);
                if (squareLowestAttackerPiece != null)
                {
                    result -= Math.Max(GetPieceValue(oldboard.GetPieceOnSquare(move.startsquare).acronym)
                    - GetPieceValue(squareLowestAttackerPiece), 0);
                }

                // Moves hanging pieces away from threat.
                if (oldboard.GetAttackingPieceDifferenceOnSquare(move.startsquare) < 0)
                    if (squareAttackerDifference >= 0)
                        result += 80;

                // Tries to place pieces on better squares
                result += GetPieceSquareValue(move.piece.acronym, move.endsquare, oldboard.CurrentColor)
                    - GetPieceSquareValue(move.piece.acronym, move.startsquare, oldboard.CurrentColor) * 2;

                if ((move is CaptureMove && move is not PromotionMove) || move is PromotionCaptureMove)
                {
                    squareAttackerDifference -= 1;
                    // Capture bonus
                    result += 20;

                    var startSquarePieceValue = GetPieceValue(oldboard.GetPieceOnSquare(move.startsquare).acronym);
                    var endSquarePieceValue = GetPieceValue(oldboard.GetPieceOnSquare(move.endsquare).acronym);

                    if (squareAttackerDifference >= 0)
                        // Tries to capture if there are too few defenders.
                        result += endSquarePieceValue * 3;
                    else
                        // Does not consider capture if piece has too many protectors
                        if (endSquarePieceValue >= startSquarePieceValue)
                            // Captures if enemy piece has higher value than ours
                            result += (endSquarePieceValue - startSquarePieceValue) * 3;
                        else
                            result += overProtectedSquarePenalty(squareAttackerDifference, newboard.inCheck());
                }
                else
                {
                    result += overProtectedSquarePenalty(squareAttackerDifference, newboard.inCheck());
                }

                if (move is PromotionMove)
                {
                    // Encourages promotion moves
                    result += 130;
                    if (move is PromotionCaptureMove)
                        result += 150;
                }
            }
            else
            {
                CrazyhouseMove crazyhouseMove = (CrazyhouseMove)move;
                // Tries to block checks
                if (oldboard.inCheck())
                    result += 30;

                // Tries to place pieces from reserve on good squares
                result += GetPieceSquareValue(crazyhouseMove.placedPiece, move.endsquare, oldboard.CurrentColor);
                var reserveevaluation = reservepiecevalues[crazyhouseMove.placedPiece];
                var mainevaluation = piecevalues[crazyhouseMove.placedPiece];
                result += (mainevaluation - reserveevaluation);

                // Does not place pieces on overprotected squares
                result += overProtectedSquarePenalty(squareAttackerDifference, newboard.inCheck());

                // Does not place pieces where lower piece can capture them
                string squareLowestAttackerPiece = oldboard.GetSquareLowestAttackerPiece(move.endsquare);
                if (squareLowestAttackerPiece != null)
                {
                    result -= Math.Max(GetPieceValue(crazyhouseMove.placedPiece)
                    - GetPieceValue(squareLowestAttackerPiece), 0);
                }

                // Encourages to place pieces where it's a safe check
                if (squareAttackerDifference > 0 && newboard.inCheck())
                    result += 600;
                else if (newboard.inCheck())
                    result += 30;
            }

            return result;
        }

        private double overProtectedSquarePenalty(int squareAttackerDifference, bool isCheck)
        {
            // Avoids squares, where piece would be instantly captured by an opponent
            if (squareAttackerDifference < 0)
            {
                if (isCheck)
                    return -250;
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
