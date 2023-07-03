using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrazyAuri.Models.Pieces;
using CrazyAuriLibrary.Models.Moves;
using CrazyAuriLibrary.Models.Pieces;

namespace CrazyAuri.Models
{
    public class Board
    {
        public Piece[,] array = new Piece[8, 8];
        public List<Piece> WhitePieces = new List<Piece>();
        public List<Piece> BlackPieces = new List<Piece>();
        public Piece WhiteKing;
        public Piece BlackKing;

        public bool CurrentColor = false;
        public bool CanWhiteCastleQueenside = true;
        public bool CanWhiteCastleKingside = true;
        public bool CanBlackCastleQueenside = true;
        public bool CanWhiteCastleingside = true;
        public (int, int) EnPassantSquare = (-1, -1);
        public ushort HalfMoveClock = 0;
        public ushort FullMoveClock = 1;

        public ushort WhiteCrazyHousePawns = 0;
        public ushort WhiteCrazyHouseKnights = 0;
        public ushort WhiteCrazyHouseBishops = 0;
        public ushort WhiteCrazyHouseRooks = 0;
        public ushort WhiteCrazyHouseQueens = 0;

        public ushort BlackCrazyHousePawns = 0;
        public ushort BlackCrazyHouseKnights = 0;
        public ushort BlackCrazyHouseBishops = 0;
        public ushort BlackCrazyHouseRooks = 0;
        public ushort BlackCrazyHouseQueens = 0;



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
            Piece newpiece = new Pawn(true, (-1, -1));
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

        public Piece GetPieceOnSquare((int, int) location)
        {
            int x = location.Item1;
            int y = location.Item2;
            if (array[x, y] != null)
            {
                    return array[x, y];
            }
            return null;
        }

        public List<Move> GetAllMoves()
        {
            LegalMoves.Clear();

            var ourPieces = BlackPieces;
            var ourKing = BlackKing;
            var enemyPieces = WhitePieces;
            var enemyKing = WhiteKing;

            if (CurrentColor == false)
            {
                ourPieces = WhitePieces;
                ourKing = WhiteKing;
                enemyPieces = BlackPieces;
                enemyKing = BlackKing;
            }

            short[,] attackedSquares = new short[8, 8];
            bool[,] pinRays = new bool[8, 8];

            foreach (var i in enemyPieces)
            {
                i.GetAttacks(this, attackedSquares, pinRays);
            }

            for (int i=0; i<8; i++)
            {
                for (int j=0; j<8; j++)
                {
                    Console.Write(attackedSquares[i, j]);
                    Console.Write(" ");
                }
                Console.WriteLine();

            }

            Console.WriteLine();

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Console.Write(pinRays[i, j] ? 1 : 0);
                    Console.Write(" ");
                }
                Console.WriteLine();

            }

            return GetAllMovesStandard(ourPieces, ourKing);
        }

        private List<Move> GetAllMovesStandard(List<Piece> pieces, Piece king)
        {
            var result = new List<Move>();

            foreach (var i in pieces)
            {
                foreach (var j in i.GetMoves(this))
                {
                    LegalMoves.Add(j.ToString(), j);
                    result.Add(j);
                }
            }

            return result;
        }

        private List<Move> GetAllMovesInCheck(List<Piece> pieces, Piece king)
        {
            var result = new List<Move>();

            return result;
        }

        private List<Move> GetAllMovesInDoubleCheck(List<Piece> pieces, Piece king)
        {
            var result = new List<Move>();

            return result;
        }

        public bool MakeMove(string move)
        {
            if (LegalMoves.ContainsKey(move))
            {
                Move moveobject = LegalMoves[move];

                HalfMoveClock += 1;
                if (CurrentColor == true)
                    FullMoveClock += 1;
                CurrentColor = !CurrentColor;

                moveobject.MakeMove(this);

                return true;
            }
            return false;
        }

    }
}
