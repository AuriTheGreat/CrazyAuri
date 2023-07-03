using CrazyAuriLibrary.Models.Moves;
using CrazyAuriLibrary.Models.Pieces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyAuri.Models.Pieces
{
    public class Bishop : Piece
    {
        public Bishop(bool color, (int, int) location) : base(color, location)
        {
            acronym = "b";
        }

        public override List<Move> GetMoves(Board board)
        {
            List<Move> result = new List<Move>();
            int x = location.Item1;
            int y = location.Item2;

            int newx = x;
            int newy = y;

            result.AddRange(CheckDirection(board, (1, 1)));
            result.AddRange(CheckDirection(board, (1, -1)));
            result.AddRange(CheckDirection(board, (-1, 1)));
            result.AddRange(CheckDirection(board, (-1, -1)));
            return result;
        }

        private List<Move> CheckDirection(Board board, (short, short) direction)
        {
            List<Move> result = new List<Move>();
            int x = location.Item1;
            int y = location.Item2;

            int newx = x;
            int newy = y;

            short directionx = direction.Item1;
            short directiony = direction.Item2;

            for (short i = 0; i < 8; i++)
            {
                newx += directionx;
                newy += directiony;
                if (newx == 8 || newy == 8 || newx==-1 || newy ==-1)
                    break;
                var piece = board.GetPieceOnSquare((newx, newy));
                if (piece == null)
                {
                    result.Add(new Move(this, location, (newx, newy)));
                }
                else
                {
                    if (piece.color != this.color)
                    {
                        result.Add(new CaptureMove(this, location, (newx, newy)));
                    }
                    break;
                }
            }
            return result;
        }

        public override void MakeMove(Board board, Move move)
        {
            
        }

        public override void GetAttacks(Board board, short[,] attackedSquares, bool[,] pinRays)
        {
            CheckAttackDirection(board, (1, 1), attackedSquares, pinRays);
            CheckAttackDirection(board, (1, -1), attackedSquares, pinRays);
            CheckAttackDirection(board, (-1, 1), attackedSquares, pinRays);
            CheckAttackDirection(board, (-1, -1), attackedSquares, pinRays);
        }

        private void CheckAttackDirection(Board board, (short, short) direction, short[,] attackedSquares, bool[,] pinRays)
        {
            int x = location.Item1;
            int y = location.Item2;

            int newx = x;
            int newy = y;

            short directionx = direction.Item1;
            short directiony = direction.Item2;

            bool checkingAttacks = true;

            for (short i = 0; i < 8; i++)
            {
                newx += directionx;
                newy += directiony;
                if (newx == 8 || newy == 8 || newx == -1 || newy == -1)
                    break;
                if (checkingAttacks==true)
                {
                    attackedSquares[newx, newy] += 1;
                    var piece = board.GetPieceOnSquare((newx, newy));
                    if (piece != null)
                    {
                        checkingAttacks = false;
                        if (color == piece.color)
                        {
                            //Our own piece is blocking the pathway - not a pin.
                            break;
                        }
                        else if ((color == true && piece==board.WhiteKing) || (color == false && piece == board.BlackKing))
                        {
                            // This is not a pin, this is a check.
                            break;
                        }
                    }
                }
                else // Will continue looking for a pin
                {
                    var piece = board.GetPieceOnSquare((newx, newy));
                    if (piece != null)
                    {
                        if ((color == true && piece == board.WhiteKing) || (color == false && piece == board.BlackKing))
                        {
                            //This is a pin. Time to mark the pin on pinboard.
                            for (short j = 0; j < 8; j++)
                            {
                                newx -= directionx;
                                newy -= directiony;
                                pinRays[newx, newy] = true;
                                if (newx == x)
                                {
                                    break;
                                }
                            }
                        }
                        break;
                    }
                }
            }
        }
    }
}
