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

public class BoardScreen : GameScreen
{
    private new Game1 Game => (Game1)base.Game;
    public BoardScreen(Game1 game) : base(game) { }

    private Desktop _desktop;

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

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                var boardTile = new ImageButton();
                boardTile.Background= new TextureRegion(Content.Load<Texture2D>("gfx/wQ"));
                boardTile.OverBackground = null;
                boardTile.VerticalAlignment = VerticalAlignment.Stretch;
                boardTile.HorizontalAlignment = HorizontalAlignment.Stretch;
                boardTile.GridRow = i;
                boardTile.GridColumn = j;
                grid.Widgets.Add(boardTile);
                boardTile.TouchDown += (s, a) =>
                {
                    Game.Exit();
                };
            }
        }

        panel.Widgets.Add(grid);


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