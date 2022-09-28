﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Storage;
using System.Collections.Specialized;
using DungeonCrawler.Scenes;
using MonoGame.Extended.ViewportAdapters;
using MonoGame.Extended;

namespace DungeonCrawler.Interface
{
    public class EscapeMenu : Scene
    {
        public SpriteFont spriteFont;
        public Texture2D menuItem;
        public Texture2D menuItemSelected;
        public SpriteBatch spriteBatch;
        Color normalColor = Color.LightGreen;
        Color selectedColor = Color.White;
        int selectedIndex = 0;
        Game game;
        public Vector2 textPosition;
        private StringCollection menuItems = new StringCollection();
        public KeyboardState oldState = Keyboard.GetState();
        public KeyboardState newState;
        ContentManager content;
        GameWindow window;

        public EscapeMenu(Game game, GameWindow window, ContentManager content)
        {
            this.game = game;
            this.content = content;
            this.window = window;
            spriteBatch = (SpriteBatch)game.Services.GetService(typeof(SpriteBatch));
            LoadContent(content);
        }

        public int Width { get; private set; }
        public int Height { get; private set; }

        public int SelectedIndex
        {
            get { return selectedIndex; }
            set
            {
                selectedIndex = (int)MathHelper.Clamp(value, 0, menuItems.Count - 1);
            }
        }

        public Vector2 Position { get; set; } = new Vector2();

        public void SetMenuItems(string[] items)
        {
            menuItems.Clear();
            menuItems.AddRange(items);
            CalculateBounds();
        }

        private void CalculateBounds()
        {
            Width = menuItem.Width;
            Height = 0;
            foreach (string item in menuItems)
            {
                Height += 5;
                Height += menuItem.Height;
            }
        }

        public override void LoadContent(ContentManager content)
        {
            menuItem = content.Load<Texture2D>(@"interface\menu");
            menuItemSelected = content.Load<Texture2D>(@"interface\menuItemSelected");
            spriteFont = content.Load<SpriteFont>(@"interface\font");
        }

        public override void Update(GameTime gameTime)
        {
            newState = Keyboard.GetState();

            if (newState.IsKeyDown(Keys.S) && oldState.IsKeyUp(Keys.S))
            {
                SelectedIndex++;
            }

            if (newState.IsKeyDown(Keys.W) && oldState.IsKeyUp(Keys.W))
            {
                SelectedIndex--;
            }

            if (SelectedIndex == 0 && newState.IsKeyDown(Keys.E))
            {
                // Sends a signal to player object that player selected 'Continue' option.
                Player.PressedContinue = true;
            }

            // Save Menu
            if (SelectedIndex == 1 && newState.IsKeyDown(Keys.E))
            {
                Init.OpenSaveMenu(game, window, content);
                Init.SelectedScene = Init.Scene.SAVE_MENU;
            }

            // Load Menu
            if (SelectedIndex == 2 && newState.IsKeyDown(Keys.E))
            {
                Init.OpenLoadMenu(game, window, content);
                Init.SelectedScene = Init.Scene.LOAD_MENU;
            }

            if (SelectedIndex == 3 && newState.IsKeyDown(Keys.E))
            {
                Vector2 menuPosition = new Vector2(0, 0);
                Position = menuPosition;
                StartMenu.GameRestart = true;
            }

            oldState = newState;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            textPosition = Position;
            Rectangle buttonRectangle = new Rectangle((int)Position.X - 20, (int)Position.Y + 75, menuItem.Width, menuItem.Height);
            Color color;

            for (int i = 0; i < menuItems.Count; i++)
            {
                if (i == selectedIndex)
                {
                    color = normalColor;
                }
                else
                {
                    color = selectedColor;
                }

                spriteBatch.Draw(menuItem, buttonRectangle, Color.White);
                textPosition = new Vector2(buttonRectangle.X + (menuItem.Width / 2), buttonRectangle.Y + (menuItem.Height / 2));
                Vector2 textSize = spriteFont.MeasureString(menuItems[i]);
                textPosition.X -= textSize.X / 2;
                textPosition.Y -= spriteFont.LineSpacing / 3;
                spriteBatch.DrawString(spriteFont, menuItems[i], textPosition, color);
                buttonRectangle.Y += menuItem.Height;
                buttonRectangle.Y += 5;
            }
        }
    }
}
