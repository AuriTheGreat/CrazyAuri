using CrazyAuri.Models;
using CrazyAuriLibrary.Models.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyAuriLibrary.Models.Moves.MoveTypes
{
    public class CastlingMove : Move
    {
        public CastlingMove(Piece piece, (int, int) startsquare, (int, int) endsquare) : base(piece, startsquare, endsquare)
        {
        }

        public override void MakeMove(Board board)
        {
            if (startsquare==(0,4)) //e8
            {
                board.CanBlackCastleKingside = false;
                board.CanBlackCastleQueenside = false;
                if (endsquare==(0,2)) //a8
                {
                    board.array[0, 3] = board.array[0, 0];
                    board.array[0, 2] = board.array[0, 4];
                    board.array[0, 3].location = (0, 3);
                    board.array[0, 2].location = (0, 2);
                    board.array[0, 0] = null;
                }
                else // h8
                {
                    board.array[0, 5] = board.array[0, 7];
                    board.array[0, 6] = board.array[0, 4];
                    board.array[0, 5].location = (0, 5);
                    board.array[0, 6].location = (0, 6);
                    board.array[0, 7] = null;
                }
                board.array[0, 4] = null;
            }
            else
            {
                if (board.WhiteKing.location.Item1 == 6)
                {
                    //
                }
                board.CanWhiteCastleKingside = false;
                board.CanWhiteCastleQueenside = false;
                if (endsquare == (7, 2)) //a1
                {
                    board.array[7, 3] = board.array[7, 0];
                    board.array[7, 2] = board.array[7, 4];
                    board.array[7, 3].location = (7, 3);
                    board.array[7, 2].location = (7, 2);
                    board.array[7, 0] = null;
                }
                else // h1
                {
                    board.array[7, 5] = board.array[7, 7];
                    board.array[7, 6] = board.array[7, 4];
                    board.array[7, 5].location = (7, 5);
                    board.array[7, 6].location = (7, 6);
                    board.array[7, 7] = null;
                }
                board.array[7, 4] = null;
            }
        }
    }
}
