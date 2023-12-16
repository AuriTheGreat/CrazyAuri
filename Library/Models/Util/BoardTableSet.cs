using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyAuriLibrary.Models.Util
{
    public class BoardTableSet
    {
        public short[,] attackedSquares;
        public short[,] pinRays;
        public bool[,] checkRays;
        public short[,] squareAttackerDefenderCounts;
        public string[,] squareLowestAttackerPiece;

        public BoardTableSet()
        {
            squareAttackerDefenderCounts = new short[8, 8];
            squareLowestAttackerPiece = new string[8, 8];
            attackedSquares = new short[8, 8];
            pinRays = new short[8, 8];
            checkRays = new bool[8, 8];
        }

        public void replaceSquareLowestAttackerPiece((int, int) square, string piece)
        {
            List<string> pieceFromLowest = new List<string>{ "p", "n", "b", "r", "q", "k"};
            var currentPiece = squareLowestAttackerPiece[square.Item1, square.Item2];
            foreach (var i in pieceFromLowest)
            {
                if (piece == i)
                {
                    squareLowestAttackerPiece[square.Item1, square.Item2] = i;
                    break;
                }
                if (currentPiece == i)
                {
                    break;
                }
            }
        }
    }
}
