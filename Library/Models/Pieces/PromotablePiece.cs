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
    public abstract class PromotablePiece : Piece
    {
        public bool promoted = false;

        protected PromotablePiece(bool color, (int, int) location) : base(color, location)
        {
        }
    }
}
