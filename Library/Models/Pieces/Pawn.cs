using CrazyAuriLibrary.Models;
using CrazyAuriLibrary.Models.Pieces;
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
        public Pawn(bool color, (int, int) location) : base(color, location)
        {
            acronym = "p";
        }

        public override List<Move> GetMoves(Board board)
        {
            int x = location.Item1;
            int y = location.Item2;
            int colorint = color == true ? 1 : -1; // determines which direction to move the pawns in
            List<Move> result = new List<Move>();

            var piece = board.GetPieceOnSquare((x + colorint * 1, y));
            if (piece == null)
            {
                result.Add(new Move(this, location, (x + colorint * 1, y)));
                if ((x==1 && color==true) || (x==6  && color==false))
                {
                    piece = board.GetPieceOnSquare((x + colorint * 2, y));
                    if (piece == null)
                    {
                        result.Add(new Move(this, location, (x + colorint * 2, y)));
                    }
                }
            }

            return result;

        }

        public override void MakeMove(Board board, Move move)
        {
            
        }
    }
}
