﻿using Microsoft.Xna.Framework;
using Myra.Graphics2D.UI;
using Myra;
using MonoGame.Extended.Screens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrazyAuriApplication.Models;
using CrazyAuriApplication.Players;
using FontStashSharp;
using Myra.Graphics2D.Brushes;

namespace CrazyAuriApplication.Screens
{
    public class GameSettingsScreen : GameScreen
    {
        private new Game1 Game => (Game1)base.Game;
        public GameSettingsScreen(Game1 game) : base(game) { }

        private Desktop _desktop;

        public override void LoadContent()
        {
            base.LoadContent();

            MyraEnvironment.Game = Game;

            var panel = new Panel();

            var backgroundTexture = new Image();
            backgroundTexture.Background = new SolidBrush("#041652");
            backgroundTexture.Top = 670;
            backgroundTexture.Height = 800;
            backgroundTexture.Width = 1200;
            panel.Widgets.Add(backgroundTexture);

            PlayerSelection(panel);

            var startButton = new TextButton();
            startButton.Background = new SolidBrush("#041652");
            startButton.OverBackground = new SolidBrush("#051b66");
            startButton.Text = "Play";
            startButton.Left = 980;
            startButton.Top = 580;
            startButton.Width = 200;
            startButton.Height = 80;
            panel.Widgets.Add(startButton);
            startButton.TouchDown += (s, a) =>
            {
                Game.LoadBoardScreen();
                //Exit();
            };

            var returnButton = new TextButton();
            returnButton.Background = new SolidBrush("#041652");
            returnButton.OverBackground = new SolidBrush("#051b66");
            returnButton.Text = "Return";
            returnButton.Left = 20;
            returnButton.Top = 580;
            returnButton.Width = 200;
            returnButton.Height = 80;
            panel.Widgets.Add(returnButton);
            returnButton.TouchDown += (s, a) =>
            {
                Game.LoadMainMenu();
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

        private void PlayerSelection(Panel panel)
        {
            //White Player Radio
            var chooseWhitePlayer = new VerticalStackPanel();
            chooseWhitePlayer.Top = 100;
            chooseWhitePlayer.Left = 50;
            var newButton = new RadioButton();
            FontSystem MainFontSystem = ((DynamicSpriteFont)newButton.Font).FontSystem;
            newButton.Text = "Human";
            newButton.Font = MainFontSystem.GetFont(32);
            newButton.TouchDown += (s, a) =>
            {
                Settings.WhitePlayer = new HumanPlayer();
            };
            chooseWhitePlayer.Widgets.Add(newButton);
            newButton.IsPressed = true;
            newButton = new RadioButton();
            newButton.Text = "Minimax AI";
            newButton.Font = MainFontSystem.GetFont(32);
            newButton.TouchDown += (s, a) =>
            {
                Settings.WhitePlayer = new MinimaxPlayer();
            };
            chooseWhitePlayer.Widgets.Add(newButton);
            newButton = new RadioButton();
            newButton.Text = "Monte-Carlo AI";
            newButton.Font = MainFontSystem.GetFont(32);
            newButton.TouchDown += (s, a) =>
            {
                Settings.WhitePlayer = new MonteCarloPlayer();
            };
            chooseWhitePlayer.Widgets.Add(newButton);
            newButton = new RadioButton();
            newButton.Text = "Stockfish";
            newButton.Font = MainFontSystem.GetFont(32);
            newButton.TouchDown += (s, a) =>
            {
                Settings.WhitePlayer = new StockfishPlayer();
            };
            chooseWhitePlayer.Widgets.Add(newButton);
            panel.Widgets.Add(chooseWhitePlayer);

            //Black Player Radio
            var chooseBlackPlayer = new VerticalStackPanel();
            chooseBlackPlayer.Top = 300;
            chooseBlackPlayer.Left = 50;
            newButton = new RadioButton();
            newButton.Text = "Human";
            newButton.Font = MainFontSystem.GetFont(32);
            newButton.TouchDown += (s, a) =>
            {
                Settings.BlackPlayer = new HumanPlayer();
            };
            chooseBlackPlayer.Widgets.Add(newButton);
            newButton = new RadioButton();
            newButton.Text = "Minimax AI";
            newButton.Font = MainFontSystem.GetFont(32);
            newButton.TouchDown += (s, a) =>
            {
                Settings.BlackPlayer = new MinimaxPlayer();
            };
            chooseBlackPlayer.Widgets.Add(newButton);
            newButton.IsPressed = true;
            newButton = new RadioButton();
            newButton.Text = "Monte-Carlo AI";
            newButton.Font = MainFontSystem.GetFont(32);
            newButton.TouchDown += (s, a) =>
            {
                Settings.BlackPlayer = new MonteCarloPlayer();
            };
            chooseBlackPlayer.Widgets.Add(newButton);
            newButton = new RadioButton();
            newButton.Text = "Stockfish";
            newButton.Font = MainFontSystem.GetFont(32);
            newButton.TouchDown += (s, a) =>
            {
                Settings.BlackPlayer = new StockfishPlayer();
            };
            chooseBlackPlayer.Widgets.Add(newButton);
            panel.Widgets.Add(chooseBlackPlayer);

            //Labels
            var positionedText = new Label();
            positionedText.HorizontalAlignment = HorizontalAlignment.Center;
            positionedText.Text = "Game Settings";
            positionedText.Font = ((DynamicSpriteFont)positionedText.Font).FontSystem.GetFont(40);
            positionedText.Top = 0;
            panel.Widgets.Add(positionedText);


            positionedText = new Label();
            positionedText.Text = "Choose the White Player:";
            positionedText.Font = ((DynamicSpriteFont)positionedText.Font).FontSystem.GetFont(32);
            positionedText.Top = 60;
            positionedText.Left = 50;
            panel.Widgets.Add(positionedText);


            positionedText = new Label();
            positionedText.Text = "Choose the Black Player:";
            positionedText.Font = ((DynamicSpriteFont)positionedText.Font).FontSystem.GetFont(32);
            positionedText.Top = 260;
            positionedText.Left = 50;
            panel.Widgets.Add(positionedText);

        }
    }
}
