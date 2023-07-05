using CrazyAuri.Models;
using CrazyAuri.Models.Pieces;
using CrazyAuriLibrary.Models.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace CrazyAuriLibrary.Models.Moves.MoveTypes
{
    public class Move
    {
        public Piece piece;
        public (int, int) startsquare;
        public (int, int) endsquare;

        public Move(Piece piece, (int, int) startsquare, (int, int) endsquare)
        {
            this.piece = piece;
            this.startsquare = startsquare;
            this.endsquare = endsquare;

        }

        public override string ToString()
        {
            return GetMoveName();
        }

        public string GetMoveName()
        {
            int x1 = startsquare.Item1;
            int x2 = endsquare.Item1;
            int y1 = startsquare.Item2;
            int y2 = endsquare.Item2;

            return char.ConvertFromUtf32(y1 + 97) + (8 - x1).ToString() + char.ConvertFromUtf32(y2 + 97) + (8 - x2).ToString();
        }

        public virtual void MakeMove(Board board)
        {
            int x1 = startsquare.Item1;
            int x2 = endsquare.Item1;
            int y1 = startsquare.Item2;
            int y2 = endsquare.Item2;

            board.array[x2, y2] = board.array[x1, y1];
            board.array[x2, y2].location = (x2, y2);
            board.array[x1, y1] = null;

            piece.MakeMove(board, this);

        }


    }
}
