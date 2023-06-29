using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyAuri.Models.Pieces
{
    public class Pawn : Piece
    {
        public Pawn(bool color)
        {
            this.color = color;

        }

        public override string ToString()
        {
            if (color == true)
            {
                return "p";
            }
            else
            {
                return "P";
            }
        }

        public bool color { get; set; }
    }
}
