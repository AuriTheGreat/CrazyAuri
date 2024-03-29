﻿using CrazyAuri.Models;
using CrazyAuriLibrary.Models.Moves.MoveTypes;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using Myra.Graphics2D.Brushes;
using Myra.Graphics2D.TextureAtlases;
using Myra.Graphics2D.UI;
using Myra.Graphics2D.UI.Styles;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyAuriApplication.Screens.BoardScreenFiles
{
    public class DrawBoard
    {
        public Grid boardgrid = new Grid();
        public Grid reservegrid = new Grid();
        public TextBox movehistoryview = new TextBox();
        public TextBox fenfield = new TextBox();
        private VerticalMenu movenumbermenu = new VerticalMenu();
        private VerticalMenu whitemovemenu = new VerticalMenu();
        private VerticalMenu blackmovemenu = new VerticalMenu();
        public ImageButton[,] boardTiles = new ImageButton[8, 8];
        private Label[] reserveTileTexts = new Label[10];
        private Board board;
        private BoardScreen screen;

        public DrawBoard(Board board, BoardScreen screen)
        {
            boardgrid.Background = new TextureRegion(screen.Content.Load<Texture2D>("gfx/Board"));
            boardgrid.Height = 600;
            boardgrid.Width = 600;
            boardgrid.Top = 50;
            boardgrid.Left = 20;
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
                    boardgrid.Widgets.Add(boardTile);
                    boardTile.TouchDown += (s, a) =>
                    {
                        if (board.CurrentColor == true)
                        {
                            screen.BlackPlayer.TileActionsWhenCicked(board, screen, boardTiles[boardTile.GridRow, boardTile.GridColumn]);
                        }
                        else
                        {
                            screen.WhitePlayer.TileActionsWhenCicked(board, screen, boardTiles[boardTile.GridRow, boardTile.GridColumn]);
                        }
                    };
                }
            }
            movehistoryview.Background = new SolidBrush("#041652");
            movehistoryview.Top = 50;
            movehistoryview.Left = 1020;
            movehistoryview.Height = 600;
            movehistoryview.Width = 160;
            movehistoryview.Wrap = true;
            movehistoryview.TextChanged += (s, a) =>
            {
                movehistoryview.Text = generateMoveHistory();
            };

            fenfield.Background = null;
            fenfield.Text = "";
            fenfield.Left = 20;
            fenfield.Height = 20;
            fenfield.Width = 760;
            fenfield.TextChanged += (s, a) =>
            {
                fenfield.Text = board.GetFEN();
            };

            DrawReservePart(board, screen);
            Update();
        }

        private void DrawReservePart(Board board, BoardScreen screen)
        {
            reservegrid.Background = new SolidBrush("#6495ed");
            reservegrid.Height = 600;
            reservegrid.Width = 60;
            reservegrid.Top = 50;
            reservegrid.Left = 640;


            AddNewReserveTile(board, screen, "bP");
            AddNewReserveTile(board, screen, "bN");
            AddNewReserveTile(board, screen, "bB");
            AddNewReserveTile(board, screen, "bR");
            AddNewReserveTile(board, screen, "bQ");
            AddNewReserveTile(board, screen, "wQ");
            AddNewReserveTile(board, screen, "wR");
            AddNewReserveTile(board, screen, "wB");
            AddNewReserveTile(board, screen, "wN");
            AddNewReserveTile(board, screen, "wP");

        }

        private void AddNewReserveTile(Board board, BoardScreen screen, string identifier)
        {
            var boardTile = new ImageButton();
            boardTile.Background = new TextureRegion(screen.Content.Load<Texture2D>("gfx/" + identifier));
            boardTile.OverBackground = null;
            boardTile.PressedBackground = new TextureRegion(screen.Content.Load<Texture2D>("gfx/tilePress"));
            boardTile.OverImage = new TextureRegion(screen.Content.Load<Texture2D>("gfx/hoverDot"));
            boardTile.GridRow = reservegrid.Widgets.Count() / 2;
            reservegrid.Widgets.Add(boardTile);
            boardTile.TouchDown += (s, a) =>
            {
                if (board.CurrentColor == true)
                {
                    screen.BlackPlayer.ReserveTileActionsWhenCicked(board, screen, identifier);
                }
                else
                {
                    screen.WhitePlayer.ReserveTileActionsWhenCicked(board, screen, identifier);
                }
            };
            var positionedText = new Label();
            positionedText.Background = new Myra.Graphics2D.Brushes.SolidBrush("#041652");
            positionedText.Text = "0";
            positionedText.GridRow = boardTile.GridRow;
            positionedText.HorizontalAlignment = HorizontalAlignment.Right;
            positionedText.VerticalAlignment = VerticalAlignment.Bottom;
            reserveTileTexts[positionedText.GridRow] = positionedText;
            reservegrid.Widgets.Add(positionedText);

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
            screen.stopwatch = new Stopwatch();
            screen.stopwatch.Start();

            if (board.lastmovemade != null)
            {
                if (board.CurrentColor == false)
                {
                    var moveButton = new MenuItem();
                    moveButton.Text = board.lastmovemade.ToString();
                    blackmovemenu.Items.Add(moveButton);
                }
                else
                {
                    var moveButton = new MenuItem();
                    moveButton.Text = (movenumbermenu.Items.Count + 1).ToString() + ".";
                    movenumbermenu.Items.Add(moveButton);

                    moveButton = new MenuItem();
                    moveButton.Text = board.lastmovemade.ToString();
                    whitemovemenu.Items.Add(moveButton);
                }


            }

            Clear();

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (board.array[i, j] == null)
                        continue;
                    switch (board.array[i, j].ToString())
                    {
                        case "p":
                            boardTiles[i, j].Background = new TextureRegion(screen.Content.Load<Texture2D>("gfx/bP"));
                            break;
                        case "P":
                            boardTiles[i, j].Background = new TextureRegion(screen.Content.Load<Texture2D>("gfx/wP"));
                            break;
                        case "r":
                            boardTiles[i, j].Background = new TextureRegion(screen.Content.Load<Texture2D>("gfx/bR"));
                            break;
                        case "R":
                            boardTiles[i, j].Background = new TextureRegion(screen.Content.Load<Texture2D>("gfx/wR"));
                            break;
                        case "b":
                            boardTiles[i, j].Background = new TextureRegion(screen.Content.Load<Texture2D>("gfx/bB"));
                            break;
                        case "B":
                            boardTiles[i, j].Background = new TextureRegion(screen.Content.Load<Texture2D>("gfx/wB"));
                            break;
                        case "n":
                            boardTiles[i, j].Background = new TextureRegion(screen.Content.Load<Texture2D>("gfx/bN"));
                            break;
                        case "N":
                            boardTiles[i, j].Background = new TextureRegion(screen.Content.Load<Texture2D>("gfx/wN"));
                            break;
                        case "q":
                            boardTiles[i, j].Background = new TextureRegion(screen.Content.Load<Texture2D>("gfx/bQ"));
                            break;
                        case "Q":
                            boardTiles[i, j].Background = new TextureRegion(screen.Content.Load<Texture2D>("gfx/wQ"));
                            break;
                        case "k":
                            boardTiles[i, j].Background = new TextureRegion(screen.Content.Load<Texture2D>("gfx/bK"));
                            break;
                        case "K":
                            boardTiles[i, j].Background = new TextureRegion(screen.Content.Load<Texture2D>("gfx/wK"));
                            break;
                    }
                }
            }
            fenfield.Text = board.GetFEN();
            movehistoryview.Text = generateMoveHistory();
            reserveTileTexts[0].Text = board.BlackCrazyHousePawns.ToString();
            reserveTileTexts[1].Text = board.BlackCrazyHouseKnights.ToString();
            reserveTileTexts[2].Text = board.BlackCrazyHouseBishops.ToString();
            reserveTileTexts[3].Text = board.BlackCrazyHouseRooks.ToString();
            reserveTileTexts[4].Text = board.BlackCrazyHouseQueens.ToString();
            reserveTileTexts[5].Text = board.WhiteCrazyHouseQueens.ToString();
            reserveTileTexts[6].Text = board.WhiteCrazyHouseRooks.ToString();
            reserveTileTexts[7].Text = board.WhiteCrazyHouseBishops.ToString();
            reserveTileTexts[8].Text = board.WhiteCrazyHouseKnights.ToString();
            reserveTileTexts[9].Text = board.WhiteCrazyHousePawns.ToString();
        }

        private string generateMoveHistory()
        {
            StringBuilder sb = new StringBuilder("", 10000);
            for (int i = 0; i<board.movehistory.Count; i++)
            {
                if (i % 2 == 0)
                {
                    if (i != 0)
                    {
                        sb.Append('\n');
                    }

                    sb.Append(i / 2 + 1 + ". ");
                }
                else
                {
                    sb.Append(", ");
                }
                sb.Append(board.movehistory[i]);
            }
            return sb.ToString();
        }

    }
}
