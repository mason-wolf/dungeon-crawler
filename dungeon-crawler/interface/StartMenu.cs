﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using DungeonCrawler.Scenes;
using DungeonCrawler.Interface;
using Microsoft.Xna.Framework.Input;

namespace DungeonCrawler
{
    class StartMenu : SceneManager
    {
        public MainMenu buttonMenu;
        SpriteFont spriteFont;
        Texture2D background;
        Texture2D buttonImage;
        GameWindow window;


        public StartMenu(Game game, GameWindow window)
            : base(game)
        {
            LoadContent();

            this.window = window;
            this.game = game;

            string[] items = { "New Game", "Load", "Quit" };

            buttonMenu = new MainMenu(game, window, spriteFont, buttonImage, background);
            buttonMenu.SetMenuItems(items);          
            Components.Add(buttonMenu);
            buttonMenu.Show();
        }

        public int SelectedIndex
        {
            get { return buttonMenu.SelectedIndex; }
        }

        protected override void LoadContent()
        {
            background = Content.Load<Texture2D>(@"interface\titlescreen");
            buttonImage = Content.Load<Texture2D>(@"interface\menu");
            spriteFont = Content.Load<SpriteFont>(@"interface\font");
            base.LoadContent();
        }

        bool gameStart = false;

        public override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();

            if (!gameStart)
            {
                // New Game
                if (keyboardState.IsKeyDown(Keys.E) && SelectedIndex == 0)
                {
                    gameStart = true;
                    buttonMenu.Hide();
                    UnloadContent();
                    Init init = new Init(game, window);
                    Components.Add(init);
                    init.Show();
                }

                // Load Game
                if (keyboardState.IsKeyDown(Keys.E) && SelectedIndex == 1)
                {
                    gameStart = true;
                    buttonMenu.Hide();
                    UnloadContent();
                    Init init = new Init(game, window);
                    Components.Add(init);
                    init.Show();
                    // Pause player movement because they're in the menu.
                    Init.Player.InMenu = true;
                    Init.SelectedScene = Init.Scene.LoadMenu;
                }
            }

            base.Update(gameTime);
        }

        public override void Show()
        {
            buttonMenu.Position = new Vector2((Game.Window.ClientBounds.Width - buttonMenu.Width) / 2, 450);
            base.Show();
        }

        public override void Hide()
        {
            base.Hide();
        }
    }
}
