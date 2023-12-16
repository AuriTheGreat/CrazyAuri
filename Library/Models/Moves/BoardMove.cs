using CrazyAuri.Models;
using CrazyAuriLibrary.Models.Moves.MoveTypes;
using CrazyAuriLibrary.Models.Pieces;
using CrazyAuriLibrary.Models.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyAuriLibrary.Models.Moves
{
    public class BoardMove
    {
        public bool inCheck = false;
        private bool movesHaveBeenChecked=false;

        private Board board;

        private BoardTableSet boardTableSet;

        private List <Move> LegalMoves = new List <Move>();
        private Dictionary<string, Move> LegalMovesDictionary = new Dictionary<string, Move>();

        public BoardMove(Board board)
        {
            this.board = board;
        }

        public List<Move> GetAllMoves()
        {
            if (movesHaveBeenChecked == true)
            {
                return LegalMoves;
            }
            movesHaveBeenChecked = true;

            this.boardTableSet = new BoardTableSet();
            var attackedSquares = boardTableSet.attackedSquares;

            var ourPieces = board.BlackPieces;
            var ourKing = board.BlackKing;
            var enemyPieces = board.WhitePieces;
            var enemyKing = board.WhiteKing;

            if (board.CurrentColor == false)
            {
                ourPieces = board.WhitePieces;
                ourKing = board.WhiteKing;
                enemyPieces = board.BlackPieces;
                enemyKing = board.BlackKing;
            }

            foreach (var i in enemyPieces)
            {
                i.GetAttacks(board, boardTableSet);
            }

            if (attackedSquares[ourKing.location.Item1, ourKing.location.Item2]==0)
            {
                LegalMoves=GetAllMovesStandard(ourPieces);
                this.inCheck = false;
            }
            else if (attackedSquares[ourKing.location.Item1, ourKing.location.Item2] == 1)
            {
                LegalMoves = GetAllMovesInCheck(ourPieces);
                this.inCheck = true;
            }
            else
            {
                LegalMoves = GetAllMovesInDoubleCheck(ourKing);
                this.inCheck = true;
            }
            return LegalMoves;
        }

        private List<Move> GetAllMovesStandard(List<Piece> pieces)
        {
            var result = new List<Move>();

            foreach (var i in pieces)
            {
                foreach (var j in i.GetMoves(board, boardTableSet))
                {
                    LegalMovesDictionary.Add(j.ToString(), j);
                    result.Add(j);
                }
            }

            result.AddRange(GetCrazyhouseMoves());

            return result;
        }

        private List<Move> GetAllMovesInCheck(List<Piece> pieces)
        {
            var result = new List<Move>();
            var checkRays = boardTableSet.checkRays;

            foreach (var i in pieces)
            {
                foreach (var j in i.GetCheckMoves(board, boardTableSet))
                {
                    LegalMovesDictionary.Add(j.ToString(), j);
                    result.Add(j);
                }
            }

            result.AddRange(GetCrazyhouseMoves(checkRays));

            return result;
        }

        private List<Move> GetAllMovesInDoubleCheck(Piece king)
        {
            var result = king.GetMoves(board, boardTableSet);

            foreach (var j in result)
            {
                LegalMovesDictionary.Add(j.ToString(), j);
            }
            return result;
        }

        private List<Move> GetCrazyhouseMoves()
        {
            var result = new List<Move>();
            var possiblepieces = GetPossibleCrazyhousePieces();
            if (possiblepieces.Count == 0)
                return result;
            for (int i=0; i<8; i++)
            {
                for (int j=0; j<8; j++)
                {
                    if (board.array[i, j] == null)
                    {
                        foreach(var piece in possiblepieces)
                        {
                            if (piece != "p" || (i != 0 && i != 7))
                            {
                                var newmove = new CrazyhouseMove((i, j), piece);
                                result.Add(newmove);
                                LegalMovesDictionary.Add(newmove.ToString(), newmove);
                            }
                        }
                    }
                }
            }
            return result;
        }

        private List<Move> GetCrazyhouseMoves(bool[,] checkrays)
        {
            var result = new List<Move>();
            var possiblepieces = GetPossibleCrazyhousePieces();
            if (possiblepieces.Count == 0)
                return result;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (checkrays[i,j] == true && board.array[i, j] == null)
                    {
                        foreach (var piece in possiblepieces)
                        {
                            if (piece != "p" || (i != 0 && i != 7))
                            {
                                var newmove = new CrazyhouseMove((i, j), piece);
                                result.Add(newmove);
                                LegalMovesDictionary.Add(newmove.ToString(), newmove);
                            }
                        }
                    }
                }
            }
            return result;
        }

        private List<string> GetPossibleCrazyhousePieces()
        {
            var result = new List<string>();
            if (board.CurrentColor == true)
            {
                if (board.BlackCrazyHousePawns>0)
                    result.Add("p");
                if (board.BlackCrazyHouseKnights > 0)
                    result.Add("n");
                if (board.BlackCrazyHouseBishops > 0)
                    result.Add("b");
                if (board.BlackCrazyHouseRooks > 0)
                    result.Add("r");
                if (board.BlackCrazyHouseQueens > 0)
                    result.Add("q");
            }
            else
            {
                if (board.WhiteCrazyHousePawns > 0)
                    result.Add("p");
                if (board.WhiteCrazyHouseKnights > 0)
                    result.Add("n");
                if (board.WhiteCrazyHouseBishops > 0)
                    result.Add("b");
                if (board.WhiteCrazyHouseRooks > 0)
                    result.Add("r");
                if (board.WhiteCrazyHouseQueens > 0)
                    result.Add("q");
            }


            return result;
        }

        public bool MakeMove(Move move)
        {
            board.lastmovemade = move;
            board.EnPassantSquare = (-1, -1);
            board.HalfMoveClock += 1;
            if (board.CurrentColor == true)
                board.FullMoveClock += 1;
            board.CurrentColor = !board.CurrentColor;

            move.MakeMove(board);

            board.movehistory.Add(move.ToString());
            LegalMovesDictionary.Clear();
            LegalMoves.Clear();
            movesHaveBeenChecked = false;
            return true;
        }

        public bool MakeMove(string move)
        {
            if (LegalMovesDictionary.Count == 0)
                GetAllMoves();
            if (LegalMovesDictionary.ContainsKey(move))
            {
                Move moveobject = LegalMovesDictionary[move];
                return MakeMove(moveobject);
            }
            return false;
        }

        public string GetWinner()
        {
            string result = "0";
            if (GetAllMoves().Count == 0)
            {
                if (board.CurrentColor == false && boardTableSet.attackedSquares[board.WhiteKing.location.Item1, board.WhiteKing.location.Item2] > 0)
                {
                    return "b";
                }
                else if (board.CurrentColor == true && boardTableSet.attackedSquares[board.BlackKing.location.Item1, board.BlackKing.location.Item2] > 0)
                {
                    return "w";
                }
                else
                {
                    return "s";
                }
            }
            if (board.HalfMoveClock>50)
            {
                return "50";
            }
            if (DrawByRepetitionCheck() == true)
            {
                return "r";
            }
            return result;
        }

        private bool DrawByRepetitionCheck()
        {
            string currentposition = board.GetPositionHash();
            if (board.FormerPositions[currentposition] > 2)
            {
                return true;
            }

            return false;
        }

        public List<Move>GetAllPieceMoves((int, int) location)
        {
            if (LegalMovesDictionary.Count == 0)
                GetAllMoves();

            var result = new List<Move>();

            foreach(var i in LegalMoves)
            {
                if (i.startsquare == location)
                    result.Add(i);
            }

            return result;
        }

        public short GetAttackingPieceDifferenceOnSquare((int, int) location)
        {
            int x = location.Item1;
            int y = location.Item2;
            var squareAttackerDefenderCounts = boardTableSet.squareAttackerDefenderCounts;
            return squareAttackerDefenderCounts[x, y];
        }

        public string GetSquareLowestAttackerPiece((int, int) location)
        {
            int x = location.Item1;
            int y = location.Item2;
            var squareLowestAttackerPiece = boardTableSet.squareLowestAttackerPiece;
            return squareLowestAttackerPiece[x, y];
        }

    }
}
