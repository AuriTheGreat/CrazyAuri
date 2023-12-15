using CrazyAuri.Models;
using CrazyAuriAI.Evaluation.Functions;
using CrazyAuriAI.SearchAlgorithms.MinimaxSearch;
using CrazyAuriLibrary.Models.Moves.MoveTypes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyAuriAI.SearchAlgorithms.MonteCarloSearch
{
    public class MCMinimax
    {
        public MainEvaluationFunction evaluationFunction;
        public MCMinimax()
        {
            evaluationFunction = new MainEvaluationFunction();
        }

        public void runSearch(Node node, int depth)
        {
            NegaMaxWithTransposition(node, depth, double.MinValue, double.MaxValue);
        }

        public double NegaMaxWithTransposition(Node node, int depth, double alpha, double beta) // Alpha beta minimax
        {
            var currentboard = node.board;
            var color = node.board.CurrentColor;
            double value = 0;

            if (depth == 0 || currentboard.GetWinner() != "0")
            {
                value=(color == true ? -1 : 1) * evaluationFunction.GetEvaluation(currentboard);
            }
            else {
                if (node.IsLeaf())
                    node.ExpandNode();
                value = double.MinValue;
                foreach (var child in node.childpositions)
                {
                    if(child.move is CaptureMove)
                    {
                        if (node.board.GetAttackingPieceDifferenceOnSquare(child.move.endsquare) < 0)
                            continue;
                    }
                    value = Math.Max(value, -NegaMaxWithTransposition(child, depth - 1, -beta, -alpha));
                    if (value > alpha)
                    {
                        alpha = value;
                    }
                    if (alpha >= beta)
                    {
                        break;
                    }
                }
            }
            if (color == node.originalColor)
                node.minimaxValue = value;
            else if (color != node.originalColor)
                node.minimaxValue = -value;
            node.searchedMinimaxDepth = depth;

            return value;
        }
    }
}
