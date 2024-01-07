using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Myra;
using Myra.Graphics2D.UI;
using System.IO;
using MonoGame.Extended.Screens;
using MonoGame.Extended.Screens.Transitions;
using MonoGame.Extended;
using CrazyAuriApplication.Screens;

namespace CrazyAuriApplication
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private readonly ScreenManager _screenManager;
        public SpriteBatch SpriteBatch;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _screenManager = new ScreenManager();
            Components.Add(_screenManager);
        }

        protected override void Initialize()
        {
            _graphics.IsFullScreen = false;
            _graphics.PreferredBackBufferWidth = 1200;
            _graphics.PreferredBackBufferHeight = 800;
            _graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            LoadStartMenu();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            base.Draw(gameTime);
        }

        public void LoadStartMenu()
        {
            _screenManager.LoadScreen(new StartMenu(this));
        }

        public void LoadStartMenuWithFade()
        {
            _screenManager.LoadScreen(new StartMenu(this), new FadeTransition(GraphicsDevice, Color.Black));
        }

        public void LoadMainMenu()
        {
            _screenManager.LoadScreen(new StartMenu(this), new FadeTransition(GraphicsDevice, Color.Black));
        }

        public void LoadGameSettingsScreen()
        {
            _screenManager.LoadScreen(new GameSettingsScreen(this), new FadeTransition(GraphicsDevice, Color.Black));
        }

        public void LoadBoardScreen()
        {
            _screenManager.LoadScreen(new BoardScreen(this), new FadeTransition(GraphicsDevice, Color.Black));
        }
    }
}