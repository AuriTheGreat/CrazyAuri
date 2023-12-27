using CrazyAuri.Models;
using CrazyAuriAI.Evaluation.Functions;
using CrazyAuriLibrary.Models.Moves.MoveTypes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyAuriAI.SearchAlgorithms.MinimaxSearch
{
    public class Minimax
    {
        public TranspositionTable transpositionTable;
        public MainEvaluationFunction evaluationFunction;
        public int exactTriggered = 0;
        public int lowerTriggered = 0;
        public int upperTriggered = 0;
        public int totalPositions = 0;
        public Minimax()
        {
            transpositionTable = new TranspositionTable();
            evaluationFunction = new MainEvaluationFunction();
        }

        public (string, double) NegaMax(Board currentboard, int depth, double alpha, double beta, bool color) // Alpha beta minimax
        {
            (string, double) result = ("", Double.MinValue);
            for (int i=2; i<=depth; i++)
            {
                result = NegaMaxWithTransposition(currentboard, i, alpha, beta);
            }
            File.AppendAllText("transpositionResult.txt", currentboard.FullMoveClock + ", " 
                + totalPositions + ", " + exactTriggered + ", " + lowerTriggered + 
                ", " + upperTriggered + '\n');
            return result;
        }

        public (string, double) NegaMaxWithTransposition(Board currentboard, int depth, double alpha, double beta) // Alpha beta minimax
        {
            var color = currentboard.CurrentColor;
            var alphaOrig = alpha;
            var transpositionentry = transpositionTable.GetEntry(currentboard.GetPositionHash(), depth);
            totalPositions += 1;
            if (currentboard.GetWinner() != "0")
            {
                return ("", (color == true ? -1 : 1) * evaluationFunction.GetEvaluation(currentboard));
            }
            if (transpositionentry != null)
            {
                if (transpositionentry.flag == "EXACT")
                {
                    exactTriggered += 1;
                    return (transpositionentry.move, transpositionentry.value);
                }
                else if (transpositionentry.flag == "LOWERBOUND")
                {
                    lowerTriggered += 1;
                    alpha = Math.Max(alpha, transpositionentry.value);
                }
                else if (transpositionentry.flag == "UPPERBOUND")
                {
                    upperTriggered += 1;
                    beta = Math.Min(beta, transpositionentry.value);
                }

                if (alpha >= beta)
                    return (transpositionentry.move, transpositionentry.value);
            }

            if (depth == 0)
            {
                var evaluationValue = (color == true ? -1 : 1) * evaluationFunction.GetEvaluation(currentboard);
                transpositionTable.AddEntry(currentboard.GetPositionHash(), "", evaluationValue, depth, "EXACT");
                return ("", evaluationValue);
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
