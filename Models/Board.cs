using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrazyAuri.Models.Pieces;

namespace CrazyAuri.Models
{
    public class Board
    {
        private Piece[,] array = new Piece[8, 8];
        public List<Piece> WhitePieces;
        public List<Piece> BlackPieces;
        public Piece WhiteKing;
        public Piece BlackKing;
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
            for (int i = 0; i < FEN.Length; i++)
            {
                if (BoardIsBeingRead==true)
                {
                    switch (FEN[i])
                    {
                        case 'p':
                            array[(int)Math.Floor((decimal)currenttile / 8), currenttile % 8] = new Pawn(true);
                            currenttile += 1;
                            break;
                        case 'P':
                            array[(int)Math.Floor((decimal)currenttile / 8), currenttile % 8] = new Pawn(false);
                            currenttile += 1;
                            break;
                        case 'r':
                            array[(int)Math.Floor((decimal)currenttile / 8), currenttile % 8]= new Rook(true);
                            currenttile += 1;
                            break;
                        case 'R':
                            array[(int)Math.Floor((decimal)currenttile / 8), currenttile % 8] = new Rook(false);
                            currenttile += 1;
                            break;
                        case 'b':
                            array[(int)Math.Floor((decimal)currenttile / 8), currenttile % 8] = new Bishop(true);
                            currenttile += 1;
                            break;
                        case 'B':
                            array[(int)Math.Floor((decimal)currenttile / 8), currenttile % 8] = new Bishop(false);
                            currenttile += 1;
                            break;
                        case 'n':
                            array[(int)Math.Floor((decimal)currenttile / 8), currenttile % 8] = new Knight(true);
                            currenttile += 1;
                            break;
                        case 'N':
                            array[(int)Math.Floor((decimal)currenttile / 8), currenttile % 8] = new Knight(false);
                            currenttile += 1;
                            break;
                        case 'q':
                            array[(int)Math.Floor((decimal)currenttile / 8), currenttile % 8] = new Queen(true);
                            currenttile += 1;
                            break;
                        case 'Q':
                            array[(int)Math.Floor((decimal)currenttile / 8), currenttile % 8] = new Queen(false);
                            currenttile += 1;
                            break;
                        case 'k':
                            array[(int)Math.Floor((decimal)currenttile / 8), currenttile % 8] = new King(true);
                            currenttile += 1;
                            break;
                        case 'K':
                            array[(int)Math.Floor((decimal)currenttile / 8), currenttile % 8] = new King(false);
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
    }
}
