using CrazyAuriApplication;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Screens;
using FontStashSharp;
using Microsoft.Xna.Framework.Input;
using Myra;
using Myra.Graphics2D.UI;
using System.IO;
using MonoGame.Extended.Screens.Transitions;
using MonoGame.Extended;
using Myra.Graphics2D.TextureAtlases;
using Myra.Graphics2D.Brushes;
using CrazyAuri.Models;
using System.Collections.Generic;
using CrazyAuri.Models.Pieces;
using System;
using CrazyAuriApplication.Players;
using System.Threading;
using System.Diagnostics;
using CrazyAuriApplication.Screens.BoardScreenFiles;
using CrazyAuriApplication.Models;

public class BoardScreen : GameScreen
{
    private new Game1 Game => (Game1)base.Game;
    public BoardScreen(Game1 game) : base(game) { }

    private Desktop _desktop;

    private Board board;

    public DrawBoard drawboard;

    public IPlayer WhitePlayer = Settings.WhitePlayer;

    public IPlayer BlackPlayer = Settings.BlackPlayer;

    private Thread MoveGetter = new Thread(() => { });

    public Stopwatch stopwatch = new Stopwatch();

    private Label timeLabel;

    public override void LoadContent()
    {
        base.LoadContent();

        MyraEnvironment.Game = Game;

        var panel = new Panel();
        panel.Height = 800;
        panel.Width = 1200;

        board = new Board(Settings.StartBoard);
        drawboard = new DrawBoard(board, this);
        panel.Widgets.Add(drawboard.reservegrid);
        panel.Widgets.Add(drawboard.boardgrid);

        var scrollviewer = new ScrollViewer();
        scrollviewer.Height = 600;
        scrollviewer.Width = 250;
        scrollviewer.Left = 950;
        scrollviewer.Content = drawboard.movehistoryview;
        panel.Widgets.Add(scrollviewer);

        timeLabel = new Label();
        timeLabel.Left = 20;
        panel.Widgets.Add(timeLabel);


        // Add it to the desktop
        _desktop = new Desktop();
        _desktop.Root = panel;
    }

    public override void Update(GameTime gameTime)
    {
        double.Round(stopwatch.Elapsed.TotalSeconds, 2);

        IPlayer Player;
        if (board.GetWinner() == "0")
        {
            if (board.CurrentColor == true)
            {
                Player = BlackPlayer;
                timeLabel.Text = "Black has been considering the move for " + double.Round(stopwatch.Elapsed.TotalSeconds, 1) + "s.";
            }
            else
            {
                Player = WhitePlayer;
                timeLabel.Text = "White has been considering the move for " + double.Round(stopwatch.Elapsed.TotalSeconds, 1) + "s.";
            }

            if (MoveGetter.IsAlive == false)
            {
                MoveGetter = new Thread(() =>
                {
                    Thread.CurrentThread.IsBackground = true;
                    if (board.GetWinner() == "0")
                    {
                        Player.MakeMove(board, this);
                    }
                });
                MoveGetter.Start();
            }
        }
        else
        {
            if (board.GetWinner()=="w")
                timeLabel.Text = "White wins!";
            else if (board.GetWinner() == "b")
                timeLabel.Text = "Black wins!";
            else if (board.GetWinner() == "s")
                timeLabel.Text = "Stalemate!";
            else if (board.GetWinner() == "50")
                timeLabel.Text = "Draw by 50 move rule!";
            else if (board.GetWinner() == "r")
                timeLabel.Text = "Draw by repetition!";
            else
                timeLabel.Text = "Game is over!";
        }
    }

    public override void Draw(GameTime gameTime)
    {
        _desktop.Render();
    }

}