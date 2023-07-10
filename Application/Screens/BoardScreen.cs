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

public class BoardScreen : GameScreen
{
    private new Game1 Game => (Game1)base.Game;
    public BoardScreen(Game1 game) : base(game) { }

    private Desktop _desktop;

    private Board board;

    public ImageButton[,] boardTiles = new ImageButton[8, 8];

    public override void LoadContent()
    {
        base.LoadContent();

        MyraEnvironment.Game = Game;

        var panel = new Panel();
        panel.Height = 600;
        panel.Width = 600;
        panel.Top = 50;
        panel.Left = 20;

        var grid = new Grid();
        grid.Background = new TextureRegion(Content.Load<Texture2D>("gfx/Board"));

        panel.Widgets.Add(grid);

        board = new Board("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR/ w KQkq - 0 1");
        var drawboard = new DrawBoard(board, this);
        panel.Widgets.Add(drawboard.grid);

        //drawBoard(board.ToString());



        // Add it to the desktop
        _desktop = new Desktop();
        _desktop.Root = panel;
    }

    public override void Update(GameTime gameTime)
    {
        
    }

    public override void Draw(GameTime gameTime)
    {
        _desktop.Render();
    }

}