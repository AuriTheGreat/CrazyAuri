using CrazyAuri.Models;
using CrazyAuri.Models.Pieces;
using CrazyAuriLibrary.Models.Moves.MoveTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyAuriLibrary.Models.Pieces
{
    public abstract class DirectionalPiece : PromotablePiece
    {
        protected DirectionalPiece(bool color, (int, int) location) : base(color, location)
        {
        }

        protected List<Move> CheckDirectionIfCheck(Board board, (short, short) direction, short[,] pinRays, bool[,] checkRays)
        {
            List<Move> result = new List<Move>();
            foreach(var i in CheckDirection(board, direction, pinRays))
            {
                if (checkRays[i.endsquare.Item1, i.endsquare.Item2] == true)
                {
                    result.Add(i);
                }
            }
            return result;
        }

        protected List<Move> CheckDirection(Board board, (short, short) direction, short[,] pinRays)
        {
            List<Move> result = new List<Move>();
            int x = location.Item1;
            int y = location.Item2;

            int newx = x;
            int newy = y;

            short directionx = direction.Item1;
            short directiony = direction.Item2;

            short isPinned = pinRays[x, y];

            for (short i = 0; i < 8; i++)
            {
                newx += directionx;
                newy += directiony;
                if (newx == 8 || newy == 8 || newx == -1 || newy == -1)
                    break;
                if (isPinned > 0)
                {
                    if (pinRays[newx, newy] != isPinned)
                        break;
                }
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

        protected void CheckAttackDirection(Board board, (short, short) direction, short[,] attackedSquares, short[,] pinRays, bool[,] checkRays)
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
                if (checkingAttacks == true)
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
                        else if ((color == true && piece == board.WhiteKing) || (color == false && piece == board.BlackKing))
                        {
                            // This is not a pin, this is a check, marking it on checkboard
                            var checkrayx = newx;
                            var checkrayy = newy;
                            for (short j = 0; j < 8; j++)
                            {
                                checkrayx -= directionx;
                                checkrayy -= directiony;
                                checkRays[checkrayx, checkrayy] = true;
                                if (checkrayx == x && checkrayy == y)
                                {
                                    break;
                                }
                            }
                            // Continue checking for attacked squares behind the king - king cannot escape into check.
                            checkingAttacks = true;

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
                            //This is a pin, marking it on pinboard.
                            int pieceposition = y * 8 + x + 1;
                            for (short j = 0; j < 8; j++)
                            {
                                newx -= directionx;
                                newy -= directiony;
                                pinRays[newx, newy] = (short)pieceposition;
                                if (newx == x && newy == y)
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
