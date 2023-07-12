using CrazyAuri.Models;
using CrazyAuriLibrary.Models.Moves.MoveTypes;
using CrazyAuriLibrary.Models.Pieces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace CrazyAuriAI.Models
{
    public class Bot : IBot
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

        public string GetMove(Board board)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            (string, double) result = NegaMax(board, 4, double.MinValue, double.MaxValue, board.CurrentColor);
            stopwatch.Stop();
            var move = board.GetAllMoves()[0].ToString();
            if (result.Item1 != "")
                move = result.Item1;
            var evaluation = result.Item2.ToString();
            if (result.Item2 > 100000000)
                evaluation = "Winning";
            else if (result.Item2 < -100000000)
                evaluation = "Hopeless";
            var color = "White";
            if (board.CurrentColor == true)
                color = "Black";

            Console.WriteLine(color + " move chosen: " + move + " (" + evaluation + ") After " + double.Round(stopwatch.Elapsed.TotalSeconds,2) + "s.");
            return move;
        }

        private (string, double) NegaMax(Board currentboard, int depth, double alpha, double beta, bool color) // Alpha beta minimax
        {
            if (depth == 0 || currentboard.GetWinner()!="0")
            {
                return ("", (color==true ? -1 : 1) * EvaluationFunction(currentboard));
            }
            var childNodes = currentboard.GetAllMoves();
            // sort moves here
            double value = double.MinValue;
            Move bestmove = null;
            foreach (var child in childNodes)
            {
                var newboard = new Board(currentboard.ToString(), currentboard.FormerPositions);
                newboard.MakeMove(child);
                var (returnedmove, newvalue) = NegaMax(newboard, depth - 1, -beta, -alpha, !color);
                value = double.MaxNumber(value, -newvalue);
                if (value > alpha)
                {
                    alpha = value;
                    bestmove = child;
                }
                if (alpha > beta)
                {
                    break;
                }
            }

            var bestmovestring = "";

            if (bestmove != null)
                bestmovestring = bestmove.ToString();

            return (bestmovestring, value);

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
