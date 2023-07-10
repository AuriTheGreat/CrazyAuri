using CrazyAuri.Models;
using Microsoft.Xna.Framework.Graphics;
using Myra.Graphics2D.TextureAtlases;
using Myra.Graphics2D.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyAuriApplication.Models
{
    public class DrawBoard
    {
        public Grid grid = new Grid();
        private ImageButton[,] boardTiles = new ImageButton[8, 8];
        private Board board;
        private BoardScreen screen;
        private ImageButton clickedSquare = null;

        public DrawBoard(Board board, BoardScreen screen)
        {
            this.board = board;
            this.screen = screen;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    var boardTile = new ImageButton();
                    boardTiles[i, j] = boardTile;
                    boardTile.Background = null;
                    boardTile.OverBackground = null;
                    boardTile.PressedBackground = new TextureRegion(screen.Content.Load<Texture2D>("gfx/tilePress"));
                    boardTile.OverImage = new TextureRegion(screen.Content.Load<Texture2D>("gfx/hoverDot"));
                    boardTile.VerticalAlignment = VerticalAlignment.Stretch;
                    boardTile.HorizontalAlignment = HorizontalAlignment.Stretch;
                    boardTile.GridRow = i;
                    boardTile.GridColumn = j;
                    grid.Widgets.Add(boardTile);
                    boardTile.TouchDown += (s, a) =>
                    {
                        TileActionsWhenCicked(boardTiles[boardTile.GridRow, boardTile.GridColumn]);
                    };
                }
            }
            Update();
        }

        public void Clear()
        {
            foreach (var boardTile in boardTiles)
            {
                boardTile.Background = null;
                boardTile.Image = null;
            }
        }


        public void Update()
        {
            var FEN = board.ToString();
            var separatedFEN = FEN.Split(" ");
            int currenttile = 0;
            bool BoardIsBeingRead = true;

            Clear();

            for (int i = 0; i < board.ToString().Length; i++)
            {
                if (BoardIsBeingRead == true)
                {
                    int x = (int)Math.Floor((decimal)currenttile / 8);
                    int y = currenttile % 8;
                    if (x < 8)
                    {
                        switch (FEN[i])
                        {
                            case 'p':
                                boardTiles[x, y].Background = new TextureRegion(screen.Content.Load<Texture2D>("gfx/bP"));
                                currenttile += 1;
                                break;
                            case 'P':
                                boardTiles[x, y].Background = new TextureRegion(screen.Content.Load<Texture2D>("gfx/wP"));
                                currenttile += 1;
                                break;
                            case 'r':
                                boardTiles[x, y].Background = new TextureRegion(screen.Content.Load<Texture2D>("gfx/bR"));
                                currenttile += 1;
                                break;
                            case 'R':
                                boardTiles[x, y].Background = new TextureRegion(screen.Content.Load<Texture2D>("gfx/wR"));
                                currenttile += 1;
                                break;
                            case 'b':
                                boardTiles[x, y].Background = new TextureRegion(screen.Content.Load<Texture2D>("gfx/bB"));
                                currenttile += 1;
                                break;
                            case 'B':
                                boardTiles[x, y].Background = new TextureRegion(screen.Content.Load<Texture2D>("gfx/wB"));
                                currenttile += 1;
                                break;
                            case 'n':
                                boardTiles[x, y].Background = new TextureRegion(screen.Content.Load<Texture2D>("gfx/bN"));
                                currenttile += 1;
                                break;
                            case 'N':
                                boardTiles[x, y].Background = new TextureRegion(screen.Content.Load<Texture2D>("gfx/wN"));
                                currenttile += 1;
                                break;
                            case 'q':
                                boardTiles[x, y].Background = new TextureRegion(screen.Content.Load<Texture2D>("gfx/bQ"));
                                currenttile += 1;
                                break;
                            case 'Q':
                                boardTiles[x, y].Background = new TextureRegion(screen.Content.Load<Texture2D>("gfx/wQ"));
                                currenttile += 1;
                                break;
                            case 'k':
                                boardTiles[x, y].Background = new TextureRegion(screen.Content.Load<Texture2D>("gfx/bK"));
                                currenttile += 1;
                                break;
                            case 'K':
                                boardTiles[x, y].Background = new TextureRegion(screen.Content.Load<Texture2D>("gfx/wK"));
                                currenttile += 1;
                                break;
                            case '/':
                                break;
                            case ' ':
                                BoardIsBeingRead = false;
                                break;
                            default:
                                currenttile += int.Parse(FEN[i].ToString());
                                break;
                        }

                    }

                }
            }
        }

        public void TileActionsWhenCicked(ImageButton tile)
        {
            var hoverImage = new TextureRegion(screen.Content.Load<Texture2D>("gfx/hoverDot"));
            var targettedSquaresImage = new TextureRegion(screen.Content.Load<Texture2D>("gfx/overImage"));

            if (clickedSquare!=null) // Executes if it's a move
            {
                foreach (var i in board.GetAllPieceMoves((clickedSquare.GridRow, clickedSquare.GridColumn)))
                {
                    if(i.endsquare==(tile.GridRow, tile.GridColumn))
                    {
                        board.MakeMove(i.ToString());
                        board.GetAllMoves();
                        Update();
                        foreach (var j in boardTiles)
                            j.OverImage = hoverImage;
                        clickedSquare = null;
                        return;
                    }
                }
            }
            clickedSquare = tile;
            //Clears board
            foreach (var i in boardTiles)
            {
                i.Image = null;
                i.OverImage = hoverImage;
            }
            //Adds new pictures
            foreach (var i in board.GetAllPieceMoves((tile.GridRow, tile.GridColumn)))
            {
                boardTiles[i.endsquare.Item1, i.endsquare.Item2].Image = targettedSquaresImage;
                boardTiles[i.endsquare.Item1, i.endsquare.Item2].OverImage = targettedSquaresImage;
            }
        }
    }
}
