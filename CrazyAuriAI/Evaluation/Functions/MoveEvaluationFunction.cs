using CrazyAuri.Models;
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
        public readonly Dictionary<string, double> piecevalues = new Dictionary<string, double>()
        {
            { "p", 134 },
            { "n", 235 },
            { "b", 300 },
            { "r", 314 },
            { "q", 604 },
        };

        public readonly Dictionary<string, double> reservepiecevalues = new Dictionary<string, double>()
        {
            { "p", 149 },
            { "n", 296 },
            { "b", 281 },
            { "r", 353 },
            { "q", 563 },
        };

        public readonly Dictionary<string, double[,]> tilepiecevalues = new Dictionary<string, double[,]>() {
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

        public double GetEvaluation(Board oldboard, Board newboard, Move move)
        {
            double result = 0;

            if (newboard.inCheck())
                result += 50;

            if (!(move is CrazyhouseMove))
            {
                result += Math.Max(GetPieceSquareValue(move.piece.acronym, move.endsquare, oldboard.CurrentColor)
                    - GetPieceSquareValue(move.piece.acronym, move.startsquare, oldboard.CurrentColor), 0);
            }

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
