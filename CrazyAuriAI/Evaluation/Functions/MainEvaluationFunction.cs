using CrazyAuri.Models;
using CrazyAuriAI.Evaluation.PieceEvaluationSets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyAuriAI.Evaluation.Functions
{
    public class MainEvaluationFunction : IEvaluationFunction
    {
        public readonly Dictionary<string, double> piecevalues = PieceEvaluationSet.piecevalues;
        public readonly Dictionary<string, double> reservepiecevalues = PieceEvaluationSet.reservepiecevalues;
        public readonly Dictionary<string, double[,]> tilepiecevalues = PieceEvaluationSet.tilepiecevalues;

        public double GetEvaluation(Board board)
        {
            if (board.GetWinner() != "0")
            {
                if (board.GetWinner() == "w")
                {
                    return double.MaxValue;
                }
                else if (board.GetWinner() == "b")
                {
                    return double.MinValue;
                }
                else
                {
                    return 0;
                }
            }

            double result = 0;

            foreach (var i in board.BlackPieces)
            {
                result -= GetPieceValue(i.ToString());
                result -= GetPieceSquareValue(i.ToString(), i.location, i.color);
            }

            foreach (var i in board.WhitePieces)
            {
                result += GetPieceValue(i.ToString().ToLower());
                result += GetPieceSquareValue(i.ToString().ToLower(), i.location, i.color);
            }

            result += GetReservePieceEvaluation(board);

            return result;
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

        private double GetReservePieceEvaluation(Board board)
        {
            double evaluation = 0;

            evaluation += board.WhiteCrazyHousePawns * reservepiecevalues["p"];
            evaluation += board.WhiteCrazyHouseKnights * reservepiecevalues["n"];
            evaluation += board.WhiteCrazyHouseBishops * reservepiecevalues["b"];
            evaluation += board.WhiteCrazyHouseRooks * reservepiecevalues["r"];
            evaluation += board.WhiteCrazyHouseQueens * reservepiecevalues["q"];
            evaluation -= board.BlackCrazyHousePawns * reservepiecevalues["p"];
            evaluation -= board.BlackCrazyHouseKnights * reservepiecevalues["n"];
            evaluation -= board.BlackCrazyHouseBishops * reservepiecevalues["b"];
            evaluation -= board.BlackCrazyHouseRooks * reservepiecevalues["r"];
            evaluation -= board.BlackCrazyHouseQueens * reservepiecevalues["q"];

            return evaluation;
        }

    }
}
