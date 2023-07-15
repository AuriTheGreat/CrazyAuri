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
using CrazyAuriApplication.Models;
using CrazyAuriApplication.Players;
using System.Threading;

public class BoardScreen : GameScreen
{
    private new Game1 Game => (Game1)base.Game;
    public BoardScreen(Game1 game) : base(game) { }

    private Desktop _desktop;

    private Board board;

    public DrawBoard drawboard;

    public IPlayer WhitePlayer = new HumanPlayer();

    public IPlayer BlackPlayer = new BotPlayer();

    private Thread MoveGetter = new Thread(() => { }); 

    public override void LoadContent()
    {
        base.LoadContent();

        MyraEnvironment.Game = Game;

        var panel = new Panel();
        panel.Height = 1200;
        panel.Width = 800;

        board = new Board("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR/ w KQkq - 0 1");
        drawboard = new DrawBoard(board, this);
        panel.Widgets.Add(drawboard.reservegrid);
        panel.Widgets.Add(drawboard.boardgrid);


        // Add it to the desktop
        _desktop = new Desktop();
        _desktop.Root = panel;
    }

    public override void Update(GameTime gameTime)
    {
        if (MoveGetter.IsAlive == false)
        {
            if (board.CurrentColor == true)
            {
                MoveGetter = new Thread(() =>
                {
                    Thread.CurrentThread.IsBackground = true;
                    BlackPlayer.MakeMove(board, this);
                });
                MoveGetter.Start();
            }
            else
            {
                MoveGetter = new Thread(() =>
                {
                    Thread.CurrentThread.IsBackground = true;
                    WhitePlayer.MakeMove(board, this);
                });
                MoveGetter.Start();
            }
        }
    }

    public override void Draw(GameTime gameTime)
    {
        _desktop.Render();
    }

}