using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrazyAuri.Models.Pieces;
using CrazyAuriLibrary.Models;

namespace CrazyAuri.Models
{
    public class Board
    {
        public IPiece[,] array = new IPiece[8, 8];
        public List<IPiece> WhitePieces = new List<IPiece>();
        public List<IPiece> BlackPieces = new List<IPiece>();
        public IPiece WhiteKing;
        public IPiece BlackKing;

        public bool CurrentColor = false;
        public bool CanWhiteCastleQueenside = true;
        public bool CanWhiteCastleKingside = true;
        public bool CanBlackCastleQueenside = true;
        public bool CanWhiteCastleingside = true;
        public (int, int) EnPassantSquare = (-1, -1);
        public int HalfMoveClock = 0;
        public int FullMoveClock = 1;

        private Dictionary<string, Move> LegalMoves = new Dictionary<string, Move>();
        public Board()
        {
            InitialiseBoard("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR/ w KQkq - 0 1");

        }

        public Board(string FEN)
        {
            InitialiseBoard(FEN);
        }

        protected void InitialiseBoard(string FEN)
        {
            int currenttile = 0;
            bool BoardIsBeingRead = true;
            IPiece newpiece = new Pawn(true, (-1, -1));
            for (int i = 0; i < FEN.Length; i++)
            {
                if (BoardIsBeingRead==true)
                {
                    int x = (int)Math.Floor((decimal)currenttile / 8);
                    int y = currenttile % 8;
                    switch (FEN[i])
                    {
                        case 'p':
                            newpiece = new Pawn(true, (x, y));
                            array[x, y] = newpiece;
                            BlackPieces.Add(newpiece);
                            currenttile += 1;
                            break;
                        case 'P':
                            newpiece = new Pawn(false, (x, y));
                            array[x, y] = newpiece;
                            WhitePieces.Add(newpiece);
                            currenttile += 1;
                            break;
                        case 'r':
                            newpiece = new Rook(true, (x, y));
                            array[x, y] = newpiece;
                            BlackPieces.Add(newpiece);
                            currenttile += 1;
                            break;
                        case 'R':
                            newpiece = new Rook(false, (x, y));
                            array[x, y] = newpiece;
                            WhitePieces.Add(newpiece);
                            currenttile += 1;
                            break;
                        case 'b':
                            newpiece = new Bishop(true, (x, y));
                            array[x, y] = newpiece;
                            BlackPieces.Add(newpiece);
                            currenttile += 1;
                            break;
                        case 'B':
                            newpiece = new Bishop(false, (x, y));
                            array[x, y] = newpiece;
                            WhitePieces.Add(newpiece);
                            currenttile += 1;
                            break;
                        case 'n':
                            newpiece = new Knight(true, (x, y));
                            array[x, y] = newpiece;
                            BlackPieces.Add(newpiece);
                            currenttile += 1;
                            break;
                        case 'N':
                            newpiece = new Knight(false, (x, y));
                            array[x, y] = newpiece;
                            WhitePieces.Add(newpiece);
                            currenttile += 1;
                            break;
                        case 'q':
                            newpiece = new Queen(true, (x, y));
                            array[x, y] = newpiece;
                            BlackPieces.Add(newpiece);
                            currenttile += 1;
                            break;
                        case 'Q':
                            newpiece = new Queen(false, (x, y));
                            array[x, y] = newpiece;
                            WhitePieces.Add(newpiece);
                            currenttile += 1;
                            break;
                        case 'k':
                            newpiece = new King(true, (x, y));
                            array[x, y] = newpiece;
                            BlackPieces.Add(newpiece);
                            BlackKing = newpiece;
                            currenttile += 1;
                            break;
                        case 'K':
                            newpiece = new King(false, (x, y));
                            array[x, y] = newpiece;
                            WhitePieces.Add(newpiece);
                            WhiteKing = newpiece;
                            currenttile += 1;
                            break;
                        case '/':
                            break;
                        case ' ':
                            BoardIsBeingRead=false;
                            break;
                        default:
                            currenttile += int.Parse(FEN[i].ToString());
                            break;
                    }
                }
                else
                {
                    break;
                }
            }

        }

        public override string ToString()
        {
            return PrintFEN();
        }

        public void PrintBoard()
        {
            Console.WriteLine("+---+---+---+---+---+---+---+---+");
            for (int i=0; i<8; i++)
            {
                Console.Write("| ");
                for (int j = 0; j < 8; j++)
                {
                    Console.Write(array[i, j]!=null ? array[i, j] : " ");
                    Console.Write(" | ");
                }
                Console.WriteLine("");
                Console.WriteLine("+---+---+---+---+---+---+---+---+");
            }
        }

        public string PrintFEN()
        {
            return base.ToString();
        }

        public IPiece GetPieceOnSquare((int, int) location)
        {
            int x = location.Item1;
            int y = location.Item2;
            if (array[x, y] != null)
            {
                return array[x,y];
            }
            return null;
        }

        public List<Move> GetAllMoves()
        {
            LegalMoves.Clear();
            List<Move> result = new List<Move>();

            if (CurrentColor == false)
            {
                foreach (var i in WhitePieces)
                {
                    foreach (var j in i.GetMoves(this))
                    {
                        LegalMoves.Add(j.ToString(), j);
                        result.Add(j);
                    }
                }
            }
            else
            {
                foreach (var i in BlackPieces)
                {
                    foreach (var j in i.GetMoves(this))
                    {
                        LegalMoves.Add(j.ToString(), j);
                        result.Add(j);
                    }
                }
            }

            return result;
        }

        public void MakeMove(string move)
        {
            Move moveobject = LegalMoves[move];
            moveobject.MakeMove(this);
        }

    }
}
