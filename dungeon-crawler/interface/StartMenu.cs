using System;
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
        public MainMenu mainMenu;
        SpriteFont spriteFont;
        Texture2D background;
        Texture2D buttonImage;
        GameWindow window;
        KeyboardState newKeyboardState;
        KeyboardState oldKeyboardState;
        Init init;
        bool gameStart = false;
        public static bool GameRestart = false;
        public StartMenu(Game game, GameWindow window)
            : base(game)
        {
            LoadContent();

            this.window = window;
            this.game = game;

            string[] items = { "New Game", "Load", "Quit" };

            mainMenu = new MainMenu(game, window, spriteFont, buttonImage, background);
            mainMenu.SetMenuItems(items);          
            Components.Add(mainMenu);
            mainMenu.Show();
        }

        public int SelectedIndex
        {
            get { return mainMenu.SelectedIndex; }
        }

        protected override void LoadContent()
        {
            background = Content.Load<Texture2D>(@"interface\titlescreen");
            buttonImage = Content.Load<Texture2D>(@"interface\menu");
            spriteFont = Content.Load<SpriteFont>(@"interface\font");
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            oldKeyboardState = newKeyboardState;
            newKeyboardState = Keyboard.GetState();

            if (!gameStart)
            {
                // New Game
                if (newKeyboardState.IsKeyDown(Keys.E) && oldKeyboardState.IsKeyUp(Keys.E) && SelectedIndex == 0)
                {
                    gameStart = true;
                    mainMenu.Hide();
                    UnloadContent();
                    init = new Init(game, window);
                    Components.Clear();
                    Components.Add(init);
                    init.Show();
                    Init.Player.InMenu = true;
                    Init.NewGame = true;
                   // Init.SelectedScene = Init.Scene.LOADING_SCREEN;
                }

                // Load Game
                if (newKeyboardState.IsKeyDown(Keys.E) && oldKeyboardState.IsKeyUp(Keys.E) && SelectedIndex == 1)
                {
                    gameStart = true;
                    mainMenu.Hide();
                    UnloadContent();
                    init = new Init(game, window);
                    Components.Clear();
                    Components.Add(init);
                    init.Show();
                    // Pause player movement because they're in the menu.
                    Init.Player.InMenu = true;
                    Init.SelectedScene = Init.Scene.LOAD_MENU;
                }

                // Quit game
                if (newKeyboardState.IsKeyDown(Keys.E) && oldKeyboardState.IsKeyUp(Keys.E) && SelectedIndex == 2)
                {
                    game.Exit();
                }

            }

            if (GameRestart)
            {

                init.Hide();
                UnloadContent();
                Components.Clear();
                LoadContent();

                string[] items = { "New Game", "Load", "Quit" };
                mainMenu = new MainMenu(game, window, spriteFont, buttonImage, background);
                mainMenu.SetMenuItems(items);
                Components.Add(mainMenu);

                mainMenu.Show();
                GameRestart = false;
                gameStart = false;
            }
            base.Update(gameTime);
        }

        public override void Show()
        {
        //    mainMenu.Position = new Vector2(mainMenu.Position.X - 200, mainMenu.Position.Y);
            base.Show();
        }

        public override void Hide()
        {
            base.Hide();
        }
    }
}
