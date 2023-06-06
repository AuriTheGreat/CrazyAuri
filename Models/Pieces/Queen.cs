using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyAuri.Models.Pieces
{
    public class Queen : Piece
    {
        public Queen(bool color)
        {
            this.color = color;

        }

        public override string ToString()
        {
            if (color == true)
            {
                return "q";
            }
            else
            {
                return "Q";
            }
        }

        public bool color { get; set; }
    }
}
