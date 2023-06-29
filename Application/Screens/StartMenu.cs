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
        var positionedText = new Label();
        positionedText.Width = 600;
        positionedText.Height = 400;
        positionedText.Text = "Positioned Text";
        positionedText.Left = 50;
        positionedText.Top = 100;
        panel.Widgets.Add(positionedText);

        var startButton = new TextButton();
        startButton.Text = "Start";
        startButton.HorizontalAlignment = HorizontalAlignment.Center;
        startButton.Top = 100;
        startButton.Width = 200;
        startButton.Height = 80;
        panel.Widgets.Add(startButton);
        startButton.TouchDown += (s, a) =>
        {
            Game.LoadBoardScreen();
            //Exit();
        };

        var exitButton = new TextButton();
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