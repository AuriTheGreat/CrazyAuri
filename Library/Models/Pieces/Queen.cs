﻿using CrazyAuriLibrary.Models;
using CrazyAuriLibrary.Models.Pieces;
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
        public Queen(bool color, (int, int) location) : base(color, location)
        {
            acronym = "q";
        }

        public override List<Move> GetMoves(Board board)
        {
            throw new NotImplementedException();
        }

        public override void MakeMove(Board board, Move move)
        {
            throw new NotImplementedException();
        }
    }
}
