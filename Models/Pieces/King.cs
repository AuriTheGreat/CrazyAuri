using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyAuri.Models.Pieces
{
    public class King : Piece
    {
        public King(bool color)
        {
            this.color = color;

        }

        public override string ToString()
        {
            if (color == true)
            {
                return "k";
            }
            else
            {
                return "K";
            }
        }

        public bool color { get; set; }
    }
}
