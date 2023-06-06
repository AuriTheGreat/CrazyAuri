using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyAuri.Models.Pieces
{
    public class Knight : Piece
    {
        public Knight(bool color)
        {
            this.color = color;

        }

        public override string ToString()
        {
            if (color == true)
            {
                return "n";
            }
            else
            {
                return "N";
            }
        }

        public bool color { get; set; }
    }
}
