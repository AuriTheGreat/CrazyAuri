using CrazyAuri.Models;
using CrazyAuriAI.Evaluation.Functions;
using CrazyAuriLibrary.Models.Moves.MoveTypes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyAuriAI.SearchAlgorithms.Minimax
{
    public class Minimax
    {
        public TranspositionTable transpositionTable;
        public MainEvaluationFunction evaluationFunction;
        public Minimax()
        {
            transpositionTable = new TranspositionTable();
        }

        public (string, double) NegaMax(Board currentboard, int depth, double alpha, double beta, bool color) // Alpha beta minimax
        {
            (string, double) result = ("", Double.MinValue);
            for (int i=2; i<=depth; i++)
            {
                result = NegaMaxWithTransposition(currentboard, i, alpha, beta);
            }
            return result;
        }

        public (string, double) NegaMaxWithTransposition(Board currentboard, int depth, double alpha, double beta) // Alpha beta minimax
        {
            var color = currentboard.CurrentColor;
            var alphaOrig = alpha;
            var transpositionentry = transpositionTable.GetEntry(currentboard.GetPositionHash(), depth);
            if (transpositionentry != null)
            {
                if (transpositionentry.flag == "EXACT")
                    return (transpositionentry.move, transpositionentry.value);
                else if (transpositionentry.flag == "LOWERBOUND")
                    alpha = Math.Max(alpha, transpositionentry.value);
                else if (transpositionentry.flag == "UPPERBOUND")
                    beta = Math.Min(beta, transpositionentry.value);

                if (alpha >= beta)
                    return (transpositionentry.move, transpositionentry.value);
            }

            if (depth == 0 || currentboard.GetWinner() != "0")
            {
                return ("", (color == true ? -1 : 1) * evaluationFunction.GetEvaluation(currentboard));
            }

            var childNodes = currentboard.GetAllMoves();
            SortMoves(currentboard, childNodes);
            double value = double.MinValue;
            Move bestmove = null;
            foreach (var child in childNodes)
            {
                var newboard = currentboard.Copy();
                newboard.MakeMove(child);
                var (returnedmove, newvalue) = NegaMaxWithTransposition(newboard, depth - 1, -beta, -alpha);
                value = double.MaxNumber(value, -newvalue);
                if (value > alpha)
                {
                    alpha = value;
                    bestmove = child;
                }
                if (alpha >= beta)
                {
                    break;
                }
            }

            var bestmovestring = "";

            if (bestmove != null)
                bestmovestring = bestmove.ToString();

            if (value <= alphaOrig)
                transpositionTable.AddEntry(currentboard.GetPositionHash(), bestmovestring, value, depth, "UPPERBOUND");
            else if (value >= beta)
                transpositionTable.AddEntry(currentboard.GetPositionHash(), bestmovestring, value, depth, "LOWERBOUND");
            else
                transpositionTable.AddEntry(currentboard.GetPositionHash(), bestmovestring, value, depth, "EXACT");

            return (bestmovestring, value);

        }

        private void SortMoves(Board board, List<Move> Moves)
        {
            var comparer = new MoveComparer(board, transpositionTable, evaluationFunction.tilepiecevalues);
            Moves.Sort(comparer);
        }
    }
}
