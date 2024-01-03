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

    private Label colorLabel;

    private Label instructionLabel;

    public override void LoadContent()
    {
        base.LoadContent();

        MyraEnvironment.Game = Game;

        var panel = new Panel();
        panel.Height = 800;
        panel.Width = 1200;
        panel.Background = new SolidBrush("#6495ed");

        var backgroundTexture = new Image();
        backgroundTexture.Background = new SolidBrush("#041652");
        backgroundTexture.Height = 30;
        backgroundTexture.Width = 1200;
        panel.Widgets.Add(backgroundTexture);

        backgroundTexture = new Image();
        backgroundTexture.Background = new SolidBrush("#041652");
        backgroundTexture.Height = 800;
        backgroundTexture.Width = 70;
        backgroundTexture.Top = 0;
        backgroundTexture.Left = 635;
        panel.Widgets.Add(backgroundTexture);

        backgroundTexture = new Image();
        backgroundTexture.Background = new SolidBrush("#041652");
        backgroundTexture.Top = 670;
        backgroundTexture.Height = 800;
        backgroundTexture.Width = 1200;
        panel.Widgets.Add(backgroundTexture);

        board = new Board(Settings.StartBoard);
        drawboard = new DrawBoard(board, this);
        panel.Widgets.Add(drawboard.reservegrid);
        panel.Widgets.Add(drawboard.boardgrid);
        panel.Widgets.Add(drawboard.movehistoryview);
        panel.Widgets.Add(drawboard.fenfield);

        colorLabel = new Label();
        FontSystem MainFontSystem = ((DynamicSpriteFont)colorLabel.Font).FontSystem;
        colorLabel.Background = new SolidBrush("#041652");
        colorLabel.Left = 720;
        colorLabel.Top = 45;
        colorLabel.Width = 285;
        colorLabel.Height = 35;
        colorLabel.Wrap = true;
        colorLabel.TextAlign = FontStashSharp.RichText.TextHorizontalAlignment.Center;
        colorLabel.Font = MainFontSystem.GetFont(30);
        panel.Widgets.Add(colorLabel);

        timeLabel = new Label();
        timeLabel.Background = new SolidBrush("#041652");
        timeLabel.Left = 720;
        timeLabel.Top = 90;
        timeLabel.Width = 285;
        timeLabel.Height = 35;
        timeLabel.TextAlign = FontStashSharp.RichText.TextHorizontalAlignment.Center;
        timeLabel.Font = MainFontSystem.GetFont(30);
        panel.Widgets.Add(timeLabel);

        instructionLabel = new Label();
        instructionLabel.Background = new SolidBrush("#041652");
        instructionLabel.Left = 720;
        instructionLabel.Top = 135;
        instructionLabel.Width = 285;
        instructionLabel.Height = 35;
        instructionLabel.TextAlign = FontStashSharp.RichText.TextHorizontalAlignment.Center;
        instructionLabel.Font = MainFontSystem.GetFont(30);
        panel.Widgets.Add(instructionLabel);



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
                timeLabel.Text = double.Round(stopwatch.Elapsed.TotalSeconds, 1) + "s.";
                colorLabel.Text = "Black to move";
            }
            else
            {
                Player = WhitePlayer;
                timeLabel.Text = double.Round(stopwatch.Elapsed.TotalSeconds, 1) + "s.";
                colorLabel.Text = "White to move";
            }

            instructionLabel.Text = Player.InstructionMessage();

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