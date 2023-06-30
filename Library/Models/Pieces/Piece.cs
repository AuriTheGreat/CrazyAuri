using CrazyAuri.Models;
using CrazyAuri.Models.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyAuriLibrary.Models.Pieces
{
    public abstract class Piece : IPiece
    {
        public string acronym = "s";
        public (int, int) location = (-1, -1);
        public bool color { get; set; }
        public Piece(bool color, (int, int) location)
        {
            this.color = color;
            this.location = location;
        }

        public override string ToString()
        {
            if (color == true)
            {
                return acronym;
            }
            else
            {
                return acronym.ToUpper();
            }
        }
        public abstract List<Move> GetMoves(Board board);
        public abstract void MakeMove(Board board, Move move);
    }
}
