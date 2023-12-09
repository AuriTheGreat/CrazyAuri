using CrazyAuri.Models;
using CrazyAuriLibrary.Models.Moves.MoveTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace CrazyAuriAI.SearchAlgorithms.MinimaxSearch
{
    public class MoveComparer : IComparer<Move>
    {
        private TranspositionTable transpositionTable;
        private Board board;
        private Dictionary<string, double[,]> tilepiecevalues;
        public MoveComparer(Board board, TranspositionTable transpositionTable, Dictionary<string, double[,]> tilepiecevalues) : base()
        {
            this.board = board;
            this.transpositionTable = transpositionTable;
            this.tilepiecevalues = tilepiecevalues;
        }

        public int Compare(Move x, Move y)
        {
            var result = CheckIfHashMove(x, y);
            if (result != 0)
                return result;

            result = SortByMoveType(x, y);
            if (result != 0)
                return result;

            result = PrioritiseBiggerCaptures(x, y);
            if (result != 0)
                return result;

            result = PrioritiseBiggerPieces(x, y);
            if (result != 0)
                return result;

            result = SortByPieceValueTables(x, y);
            if (result != 0)
                return result;

            return 0;
        }

        private int CheckIfHashMove(Move x, Move y)
        {
            var transpositionentry = transpositionTable.GetEntry(board.GetPositionHash(), 0);
            if (transpositionentry != null)
            {
                if (transpositionentry.move == x.ToString())
                    return -1;
                else if (transpositionentry.move == y.ToString())
                    return 1;
            }
            return 0;
        }

        private int SortByMoveType(Move x, Move y)
        {
            if (x is PromotionCaptureMove && y is not PromotionCaptureMove)
                return -1;
            if (y is PromotionCaptureMove && x is not PromotionCaptureMove)
                return 1;
            if (x is PromotionMove && y is not PromotionMove)
                return -1;
            if (y is PromotionMove && x is not PromotionMove)
                return 1;
            if (x is CaptureMove && y is not CaptureMove)
                return -1;
            if (y is CaptureMove && x is not CaptureMove)
                return 1;
            if (x is CastlingMove && y is not CastlingMove)
                return -1;
            if (y is CastlingMove && x is not CastlingMove)
                return 1;

            return 0;
        }

        private int PrioritiseBiggerCaptures(Move x, Move y)
        {
            if (x is PromotionCaptureMove && y is PromotionCaptureMove)
            {
                var xpiece = board.GetPieceOnSquare(x.endsquare).acronym;
                var ypiece = board.GetPieceOnSquare(y.endsquare).acronym;
                var result = PieceComparison(xpiece, ypiece);
                if (result == 0)
                {
                    xpiece = board.GetPieceOnSquare(x.startsquare).acronym;
                    ypiece = board.GetPieceOnSquare(y.startsquare).acronym;
                    result = PieceComparison(xpiece, ypiece);
                    return -result;
                }
                return result;
            }

            if (x is CaptureMove && y is CaptureMove)
            {
                if (x is PromotionMove || y is PromotionMove)
                    return 0;
                var xpiece = board.GetPieceOnSquare(x.endsquare).acronym;
                var ypiece = board.GetPieceOnSquare(y.endsquare).acronym;
                var result = PieceComparison(xpiece, ypiece);
                if (result == 0)
                {
                    xpiece = board.GetPieceOnSquare(x.startsquare).acronym;
                    ypiece = board.GetPieceOnSquare(y.startsquare).acronym;
                    result = PieceComparison(xpiece, ypiece);
                    return -result;
                }
                return result;
            }

            return 0;
        }

        private int PrioritiseBiggerPieces(Move x, Move y)
        {
            var xpiece = "";
            var ypiece = "";
            if (x is CrazyhouseMove)
                xpiece = ((CrazyhouseMove) x).placedPiece;
            else
                xpiece = x.piece.acronym;

            if (y is CrazyhouseMove)
                ypiece = ((CrazyhouseMove) y).placedPiece;
            else
                ypiece = y.piece.acronym;

            return PieceComparison(xpiece, ypiece);
        }

        private int SortByPieceValueTables(Move x, Move y)
        {
            var xpiecemovevalue = Double.MinValue;
            var ypiecemovevalue = Double.MinValue;
            if (x is CrazyhouseMove)
                xpiecemovevalue = GetPieceSquareValue(((CrazyhouseMove)x).placedPiece.ToLower(), x.endsquare, board.CurrentColor);
            else
                xpiecemovevalue = GetPieceSquareValue(x.piece.acronym, x.endsquare, board.CurrentColor) - GetPieceSquareValue(x.piece.acronym, x.startsquare, x.piece.color);

            if (y is CrazyhouseMove)
                ypiecemovevalue = GetPieceSquareValue(((CrazyhouseMove)y).placedPiece.ToLower(), y.endsquare, board.CurrentColor);
            else
                ypiecemovevalue = GetPieceSquareValue(y.piece.acronym, y.endsquare, board.CurrentColor) - GetPieceSquareValue(y.piece.acronym, y.startsquare, board.CurrentColor);

            if (xpiecemovevalue > ypiecemovevalue)
                return -1;
            if (xpiecemovevalue < ypiecemovevalue)
                return 1;

            return 0;
        }

        private int PieceComparison(string xpiece, string ypiece)
        {
            if (xpiece == ypiece)
                return 0;
            if (xpiece == "q")
                return -1;
            if (ypiece == "q")
                return 1;
            if (xpiece == "r")
                return -1;
            if (ypiece == "r")
                return 1;
            if (xpiece == "b")
                return -1;
            if (ypiece == "b")
                return 1;
            if (xpiece == "n")
                return -1;
            if (ypiece == "n")
                return 1;
            if (xpiece == "p")
                return -1;
            if (ypiece == "p")
                return 1;
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
