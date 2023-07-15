using CrazyAuri.Models;
using CrazyAuriLibrary.Models.Moves.MoveTypes;
using Microsoft.Xna.Framework.Graphics;
using Myra.Graphics2D.TextureAtlases;
using Myra.Graphics2D.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyAuriApplication.Players
{
    public class HumanPlayer : IPlayer
    {
        private ImageButton clickedSquare = null;
        private string selectedReservePiece = null;

        public void TileActionsWhenCicked(Board board, BoardScreen screen, ImageButton tile)
        {
            var hoverImage = new TextureRegion(screen.Content.Load<Texture2D>("gfx/hoverDot"));
            var targettedSquaresImage = new TextureRegion(screen.Content.Load<Texture2D>("gfx/overImage"));

            if (clickedSquare != null) // Executes if it's a move
            {
                foreach (var i in board.GetAllPieceMoves((clickedSquare.GridRow, clickedSquare.GridColumn)))
                {
                    if (i.endsquare == (tile.GridRow, tile.GridColumn))
                    {
                        board.MakeMove(i.ToString());
                        board.GetAllMoves();
                        screen.drawboard.Update();
                        foreach (var j in screen.drawboard.boardTiles)
                            j.OverImage = hoverImage;
                        clickedSquare = null;
                        selectedReservePiece = null;
                        return;
                    }
                }
            }
            else
            {
                if (selectedReservePiece != null)
                {
                    board.MakeMove(selectedReservePiece.ToUpper() + "@" + board.ConvertLocationToTileName((tile.GridRow, tile.GridColumn)));
                    screen.drawboard.Update();
                }
            }
            selectedReservePiece = null;
            clickedSquare = tile;
            //Clears board
            foreach (var i in screen.drawboard.boardTiles)
            {
                i.Image = null;
                i.OverImage = hoverImage;
            }
            //Adds new pictures
            foreach (var i in board.GetAllPieceMoves((tile.GridRow, tile.GridColumn)))
            {
                screen.drawboard.boardTiles[i.endsquare.Item1, i.endsquare.Item2].Image = targettedSquaresImage;
                screen.drawboard.boardTiles[i.endsquare.Item1, i.endsquare.Item2].OverImage = targettedSquaresImage;
            }
        }
        public void ReserveTileActionsWhenCicked(Board board, BoardScreen screen, string identifier)
        {
            var hoverImage = new TextureRegion(screen.Content.Load<Texture2D>("gfx/hoverDot"));
            var targettedSquaresImage = new TextureRegion(screen.Content.Load<Texture2D>("gfx/overImage"));

            //Clears board
            foreach (var i in screen.drawboard.boardTiles)
            {
                i.Image = null;
                i.OverImage = hoverImage;
            }
            if ((identifier[0].ToString() == "b" && board.CurrentColor == false) || (identifier[0].ToString() == "w" && board.CurrentColor == true))
                return;
            //Adds new pictures
            clickedSquare = null;
            selectedReservePiece = identifier[1].ToString().ToLower();
            foreach (var i in board.GetAllMoves())
            {
                if (i is CrazyhouseMove)
                {
                    CrazyhouseMove move = (CrazyhouseMove)i;
                    if (move.placedPiece == selectedReservePiece)
                    {
                        screen.drawboard.boardTiles[i.endsquare.Item1, i.endsquare.Item2].Image = targettedSquaresImage;
                        screen.drawboard.boardTiles[i.endsquare.Item1, i.endsquare.Item2].OverImage = targettedSquaresImage;
                    }
                }
            }
        }

        public void MakeMove(Board board, BoardScreen screen)
        {
        }

    }
}
