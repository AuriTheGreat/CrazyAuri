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
using Myra.Graphics2D.Brushes;

public class StartMenu : GameScreen
{
    private new Game1 Game => (Game1)base.Game;
    public StartMenu(Game1 game) : base(game) { }

    private Desktop _desktop;

    public override void LoadContent()
    {
        base.LoadContent();

        MyraEnvironment.Game = Game;
        var panel = new Panel();

        var backgroundTexture = new Image();
        backgroundTexture.Background = new SolidBrush("#041652");
        backgroundTexture.Height = 800;
        backgroundTexture.Width = 100;
        panel.Widgets.Add(backgroundTexture);

        var positionedText = new Label();
        positionedText.HorizontalAlignment = HorizontalAlignment.Center;
        positionedText.Text = "CrazyAuri";
        positionedText.Font = ((DynamicSpriteFont)positionedText.Font).FontSystem.GetFont(80);
        positionedText.Top = 0;
        panel.Widgets.Add(positionedText);

        var startButton = new TextButton();
        startButton.Background = new SolidBrush("#041652");
        startButton.OverBackground = new SolidBrush("#051b66");
        startButton.Text = "Start";
        startButton.HorizontalAlignment = HorizontalAlignment.Center;
        startButton.Top = 100;
        startButton.Width = 200;
        startButton.Height = 80;
        panel.Widgets.Add(startButton);
        startButton.TouchDown += (s, a) =>
        {
            Game.LoadGameSettingsScreen();
            //Exit();
        };

        var exitButton = new TextButton();
        exitButton.Background = new SolidBrush("#041652");
        exitButton.OverBackground = new SolidBrush("#051b66");
        exitButton.Text = "Exit";
        exitButton.HorizontalAlignment = HorizontalAlignment.Center;
        exitButton.Top = 200;
        exitButton.Width = 200;
        exitButton.Height = 80;
        panel.Widgets.Add(exitButton);
        exitButton.TouchDown += (s, a) =>
        {
            Game.Exit();
        };

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