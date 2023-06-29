using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyAuri.Models.Pieces
{
    public class Rook : Piece
    {
        public Rook(bool color)
        {
            this.color = color;

        }

        public override string ToString()
        {
            if (color == true)
            {
                return "r";
            }
            else
            {
                return "R";
            }
        }

        public bool color { get; set; }
    }
}
