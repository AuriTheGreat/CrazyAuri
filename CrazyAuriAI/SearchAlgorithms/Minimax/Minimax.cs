using CrazyAuri.Models;
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
        private Dictionary<string, double> piecevalues = new Dictionary<string, double>()
        {
            { "p", 134 },
            { "n", 235 },
            { "b", 300 },
            { "r", 314 },
            { "q", 604 },
        };

        private Dictionary<string, double> reservepiecevalues = new Dictionary<string, double>()
        {
            { "p", 149 },
            { "n", 296 },
            { "b", 281 },
            { "r", 353 },
            { "q", 563 },
        };

        private Dictionary<string, double[,]> tilepiecevalues = new Dictionary<string, double[,]>() {
                { "p", new double[8, 8]
                    {
                        { 0,  0,   0,   0,   0,   0,  0,  0},
                        {50, 50,  50,  50,  50,  50, 50, 50},
                        {10, 10,  20,  30,  30,  20, 10, 10},
                        { 5,  5,  10,  25,  25,  10,  5,  5},
                        { 0,  0,   0,  20,  20,   0,  0,  0},
                        { 5, -5, -10,   0,   0, -10, -5,  0},
                        { 5, 10,  10, -20, -20,  10, 10,  5},
                        { 0,  0,  0,    0,   0,   0,  0,  0}
                    }
                },
                { "n", new double[8, 8]
                    {
                        {-50, -40, -30, -30, -30, -30, -40, -50},
                        {-40, -20,   0,   0,   0,   0, -20, -40},
                        {-30,   0,  10,  15,  15,  10,   0, -30},
                        {-30,   5,  15,  20,  20,  15,   5, -30},
                        {-30,   0,  15,  20,  20,  15,   0, -30},
                        {-30,   5,  10,  15,  15,  10,   5, -30},
                        {-40, -20,   0,   5,   5,   0, -20, -40},
                        {-50, -40, -30, -30, -30, -30, -40, -50}
                    }
                },
                { "b", new double[8, 8]
                    {
                        {-20, -10, -10, -10, -10, -10, -10, -20} ,
                        {-10,   0,   0,   0,   0,   0,   0, -10},
                        {-10,   0,   5,  10,  10,   5,   0, -10},
                        {-10,   5,   5,  10,  10,   5,   5, -10},
                        {-10,   0,  10,  10,  10,  10,   0, -10},
                        {-10,  10,  10,  10,  10,  10,  10, -10},
                        {-10,   5,   0,   0,   0,   0,   5, -10},
                        {-20, -10, -10, -10, -10, -10, -10, -20}
                    }
                },
                { "r", new double[8, 8]
                    {
                        { 0,  0,  0,  0,  0,  0,  0,  0},
                        { 5, 10, 10, 10, 10, 10, 10,  5},
                        {-5,  0,  0,  0,  0,  0,  0, -5},
                        {-5,  0,  0,  0,  0,  0,  0, -5},
                        {-5,  0,  0,  0,  0,  0,  0, -5},
                        {-5,  0,  0,  0,  0,  0,  0, -5},
                        {-5,  0,  0,  0,  0,  0,  0, -5},
                        { 0,  0,  0,  5,  5,  0,  0,  0}
                    }
                },
                { "q", new double[8, 8]
                    {
                        {-20, -10, -10,  -5,  -5, -10, -10, -20} ,
                        {-10,   0,   0,   0,   0,   0,   0, -10},
                        {-10,   0,   5,   5,   5,   5,   0, -10},
                        { -5,   0,   5,   5,   5,   5,   0,  -5},
                        {  0,   0,   5,   5,   5,   5,   0,  -5},
                        {-10,   5,   5,   5,   5,   5,   0, -10},
                        {-10,   0,   5,   0,   0,   0,   0, -10},
                        {-20, -10, -10,  -5,  -5, -10, -10, -20}
                    }
                },
                { "k", new double[8, 8]
                    {
                        {-30, -40, -40, -50, -50, -40, -40, -30},
                        {-30, -40, -40, -50, -50, -40, -40, -30},
                        {-30, -40, -40, -50, -50, -40, -40, -30},
                        {-30, -40, -40, -50, -50, -40, -40, -30},
                        {-20, -30, -30, -40, -40, -30, -30, -20},
                        {-10, -20, -20, -20, -20, -20, -20, -10},
                        { 20,  20,   0,   0,   0,   0,  20,  20},
                        { 20,  30,  10,   0,   0,  10,  30,  20},
                    }
                }
            };

        public TranspositionTable transpositionTable;
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
                return ("", (color == true ? -1 : 1) * EvaluationFunction(currentboard));
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
            var comparer = new MoveComparer(board, transpositionTable, tilepiecevalues);
            Moves.Sort(comparer);
        }

        private double EvaluationFunction(Board board)
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
