using CrazyAuri.Models.Pieces;
using CrazyAuri.Models;
using CrazyAuriLibrary.Models.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyAuriLibrary.Models.Moves.MoveTypes
{
    public class CrazyhouseMove : Move
    {
        public string placedPiece;

        public CrazyhouseMove((int, int) endsquare, string placedPiece) : base(null, (-1, -1), endsquare)
        {
            this.placedPiece = placedPiece;
        }

        public override string ToString()
        {
            int x = endsquare.Item1;
            int y = endsquare.Item2;
            return placedPiece.ToUpper() + "@" + char.ConvertFromUtf32(y + 97) + (8 - x).ToString();
        }

        public override void MakeMove(Board board)
        {
            bool piececolor = !board.CurrentColor;
            Piece newpiece = new Queen(!board.CurrentColor, endsquare);

            switch (placedPiece)
            {
                case "q":
                    if (piececolor == false)
                        board.WhiteCrazyHouseQueens -= 1;
                    else
                        board.BlackCrazyHouseQueens -= 1;
                    break;
                case "r":
                    newpiece = new Rook(!board.CurrentColor, endsquare);
                    if (piececolor == false)
                        board.WhiteCrazyHouseRooks -= 1;
                    else
                        board.BlackCrazyHouseRooks -= 1;
                    break;
                case "b":
                    newpiece = new Bishop(!board.CurrentColor, endsquare);
                    if (piececolor == false)
                        board.WhiteCrazyHouseBishops -= 1;
                    else
                        board.BlackCrazyHouseBishops -= 1;
                    break;
                case "n":
                    newpiece = new Knight(!board.CurrentColor, endsquare);
                    if (piececolor == false)
                        board.WhiteCrazyHouseKnights -= 1;
                    else
                        board.BlackCrazyHouseKnights -= 1;
                    break;
                case "p":
                    newpiece = new Pawn(!board.CurrentColor, endsquare);
                    if (piececolor == false)
                        board.WhiteCrazyHousePawns -= 1;
                    else
                        board.BlackCrazyHousePawns -= 1;
                    break;
            }

            if (piececolor == false)
            {
                board.WhitePieces.Add(newpiece);
            }
            else
            {
                board.BlackPieces.Add(newpiece);
            }

            if (placedPiece == "p")
                board.HalfMoveClock = 0;

            board.array[endsquare.Item1, endsquare.Item2] = newpiece;
        }

    }
}
