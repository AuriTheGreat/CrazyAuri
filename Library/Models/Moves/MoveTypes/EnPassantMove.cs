using CrazyAuri.Models;
using CrazyAuriLibrary.Models.Pieces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyAuriLibrary.Models.Moves.MoveTypes
{
    public class EnPassantMove : Move
    {
        public EnPassantMove(Piece piece, (int, int) startsquare, (int, int) endsquare) : base(piece, startsquare, endsquare)
        {
        }

        public override void MakeMove(Board board)
        {
            board.HalfMoveClock = 0;
            int colorint = piece.color == true ? 1 : -1;

            var endpiece = board.GetPieceOnSquare((endsquare.Item1 - colorint, endsquare.Item2));

            board.array[endpiece.location.Item1, endpiece.location.Item2] = null;

            if (board.CurrentColor == false)
            {
                board.WhitePieces.Remove(endpiece);
                board.BlackCrazyHousePawns += 1;
            }
            else
            {
                board.BlackPieces.Remove(endpiece);
                board.WhiteCrazyHousePawns += 1;
            }

            base.MakeMove(board);
        }

    }
}
