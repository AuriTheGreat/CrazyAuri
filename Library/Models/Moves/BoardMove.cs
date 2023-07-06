﻿using CrazyAuri.Models;
using CrazyAuriLibrary.Models.Moves.MoveTypes;
using CrazyAuriLibrary.Models.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyAuriLibrary.Models.Moves
{
    public class BoardMove
    {
        private Board board = new Board();
  
        private short[,] attackedSquares = new short[8, 8];
        private short[,] pinRays = new short[8, 8];
        private bool[,] checkRays = new bool[8, 8];

        private Dictionary<string, Move> LegalMovesDictionary = new Dictionary<string, Move>();

        public BoardMove(Board board)
        {
            this.board = board;
        }

        public List<Move> GetAllMoves()
        {
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
                i.GetAttacks(board, attackedSquares, pinRays, checkRays);
            }

            //for (int i = 0; i < 8; i++)
            //{
            //    for (int j = 0; j < 8; j++)
            //    {
            //        Console.Write(attackedSquares[i, j]);
            //        Console.Write(" ");
            //    }
            //    Console.WriteLine();

            //}

            //Console.WriteLine();

            //for (int i = 0; i < 8; i++)
            //{
            //    for (int j = 0; j < 8; j++)
            //    {
            //        Console.Write(pinRays[i, j]);
            //        Console.Write(" ");
            //    }
            //    Console.WriteLine();

            //}

            //Console.WriteLine();

            //for (int i = 0; i < 8; i++)
            //{
            //    for (int j = 0; j < 8; j++)
            //    {
            //        Console.Write(checkRays[i, j] ? 1 : 0);
            //        Console.Write(" ");
            //    }
            //    Console.WriteLine();
            //}

            if (attackedSquares[ourKing.location.Item1, ourKing.location.Item2]==0)
            {
                return GetAllMovesStandard(ourPieces);
            }
            else if (attackedSquares[ourKing.location.Item1, ourKing.location.Item2] == 1)
            {
                return GetAllMovesInCheck(ourPieces);
            }
            else
            {
                return GetAllMovesInDoubleCheck(ourKing);
            }
        }

        private List<Move> GetAllMovesStandard(List<Piece> pieces)
        {
            var result = new List<Move>();

            foreach (var i in pieces)
            {
                foreach (var j in i.GetMoves(board, attackedSquares, pinRays))
                {
                    LegalMovesDictionary.Add(j.ToString(), j);
                    result.Add(j);
                }
            }

            return result;
        }

        private List<Move> GetAllMovesInCheck(List<Piece> pieces)
        {
            var result = new List<Move>();

            foreach (var i in pieces)
            {
                foreach (var j in i.GetCheckMoves(board, attackedSquares, pinRays, checkRays))
                {
                    LegalMovesDictionary.Add(j.ToString(), j);
                    result.Add(j);
                }
            }

            return result;
        }

        private List<Move> GetAllMovesInDoubleCheck(Piece king)
        {
            var result = king.GetMoves(board, attackedSquares, pinRays);

            foreach (var j in result)
            {
                LegalMovesDictionary.Add(j.ToString(), j);
            }
            return result;
        }

        public bool MakeMove(string move)
        {
            if (LegalMovesDictionary.Count == 0)
                GetAllMoves();
            if (LegalMovesDictionary.ContainsKey(move))
            {
                Move moveobject = LegalMovesDictionary[move];

                board.EnPassantSquare = (-1, -1);
                board.HalfMoveClock += 1;
                if (board.CurrentColor == true)
                    board.FullMoveClock += 1;
                board.CurrentColor = !board.CurrentColor;

                moveobject.MakeMove(board);

                LegalMovesDictionary.Clear();
                return true;
            }
            return false;
        }


    }
}
