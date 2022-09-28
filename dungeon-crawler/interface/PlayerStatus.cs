﻿using DungeonCrawler;
using DungeonCrawler.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Interface
{
    public class PlayerStatus : Scene
    {
        private Texture2D statusTexture;
        private Rectangle statusInterface;
        private SpriteFont font;
        private Vector2 position;

        public static bool StatusOpen { get; set; } = false;

        public PlayerStatus(ContentManager content)
        {
            LoadContent(content);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (StatusOpen)
            {
                spriteBatch.Draw(statusTexture, null, statusInterface);
                spriteBatch.DrawString(font, "Status", new Vector2(position.X + 25, position.Y), Color.White, 0, new Vector2(0, 0), 1.25f, SpriteEffects.None, 0);
                spriteBatch.DrawString(font, "Level " + Init.Player.Level, new Vector2(position.X + 25, position.Y + 25), Color.White, 0, new Vector2(0, 0), .8f, SpriteEffects.None, 0);
                spriteBatch.DrawString(font, "XP: " + Init.Player.XP + "/" + Init.Player.XPRemaining, new Vector2(position.X + 25, position.Y + 35), Color.White, 0, new Vector2(0, 0), .8f, SpriteEffects.None, 0);
                spriteBatch.DrawString(font, "HP " + Math.Round(Init.Player.CurrentHealth).ToString() + "/" + (Init.Player.MaxHealth + Init.Player.HealthBonus).ToString(), new Vector2(position.X + 25, position.Y + 45), Color.Green, 0, new Vector2(0, 0), .8f, SpriteEffects.None, 0);
                spriteBatch.DrawString(font, "MP " + Math.Round(Init.Player.CurrentMana).ToString() + "/" + (Init.Player.MaxMana + Init.Player.ManaBonus).ToString(), new Vector2(position.X + 25, position.Y + 55), Color.Blue, 0, new Vector2(0, 0), .8f, SpriteEffects.None, 0);
                spriteBatch.DrawString(font, "Gold " + Init.Player.Gold.ToString(), new Vector2(position.X + 25, position.Y + 65), Color.Yellow, 0, new Vector2(0, 0), .8f, SpriteEffects.None, 0);
                spriteBatch.DrawString(font, "Spells Learned " + Init.SpellInventory.Contents.Count().ToString(), new Vector2(position.X + 25, position.Y + 75), Color.White, 0, new Vector2(0, 0), .8f, SpriteEffects.None, 0);
                spriteBatch.DrawString(font, "Enemies Defeated " + Init.Player.EnemiesKilled.ToString(), new Vector2(position.X + 25, position.Y + 85), Color.White, 0, new Vector2(0, 0), .8f, SpriteEffects.None, 0);

                spriteBatch.DrawString(font, "Fire Resistance          " + Init.Player.FireResistance, new Vector2(position.X + 25, position.Y + 105), Color.White, 0, new Vector2(0, 0), .8f, SpriteEffects.None, 0);
                spriteBatch.DrawString(font, "Frost Resistance       " + Init.Player.FrostResistance, new Vector2(position.X + 25, position.Y + 115), Color.White, 0, new Vector2(0, 0), .8f, SpriteEffects.None, 0);
                spriteBatch.DrawString(font, "Thunder Resistance   " + Init.Player.ThunderResistance, new Vector2(position.X + 25, position.Y + 125), Color.White, 0, new Vector2(0, 0), .8f, SpriteEffects.None, 0);

                spriteBatch.DrawString(font, "Head", new Vector2(position.X + 180, position.Y + 25), Color.Cyan, 0, new Vector2(0, 0), .8f, SpriteEffects.None, 0);
                spriteBatch.DrawString(font, Init.Player.Equipment.Head.Name, new Vector2(position.X + 180, position.Y + 35), Color.White, 0, new Vector2(0, 0), .8f, SpriteEffects.None, 0);
                spriteBatch.DrawString(font, Init.Player.Equipment.Head.Description, new Vector2(position.X + 180, position.Y + 45), Color.White, 0, new Vector2(0, 0), .8f, SpriteEffects.None, 0);

                spriteBatch.DrawString(font, "Chest", new Vector2(position.X + 180, position.Y + 55), Color.Cyan, 0, new Vector2(0, 0), .8f, SpriteEffects.None, 0);
                spriteBatch.DrawString(font, Init.Player.Equipment.Chest.Name, new Vector2(position.X + 180, position.Y + 65), Color.White, 0, new Vector2(0, 0), .8f, SpriteEffects.None, 0);
                spriteBatch.DrawString(font, Init.Player.Equipment.Chest.Description, new Vector2(position.X + 180, position.Y + 75), Color.White, 0, new Vector2(0, 0), .8f, SpriteEffects.None, 0);

                spriteBatch.DrawString(font, "Hands", new Vector2(position.X + 180, position.Y + 85), Color.Cyan, 0, new Vector2(0, 0), .8f, SpriteEffects.None, 0);
                spriteBatch.DrawString(font, Init.Player.Equipment.Hands.Name, new Vector2(position.X + 180, position.Y + 95), Color.White, 0, new Vector2(0, 0), .8f, SpriteEffects.None, 0);
                spriteBatch.DrawString(font, Init.Player.Equipment.Hands.Description, new Vector2(position.X + 180, position.Y + 105), Color.White, 0, new Vector2(0, 0), .8f, SpriteEffects.None, 0);


                spriteBatch.DrawString(font, "Boots", new Vector2(position.X + 180, position.Y + 115), Color.Cyan, 0, new Vector2(0, 0), .8f, SpriteEffects.None, 0);
                spriteBatch.DrawString(font, Init.Player.Equipment.Boots.Name, new Vector2(position.X + 180, position.Y + 125), Color.White, 0, new Vector2(0, 0), .8f, SpriteEffects.None, 0);
                spriteBatch.DrawString(font, Init.Player.Equipment.Boots.Description, new Vector2(position.X + 180, position.Y + 135), Color.White, 0, new Vector2(0, 0), .8f, SpriteEffects.None, 0);

                spriteBatch.DrawString(font, "Ring", new Vector2(position.X + 180, position.Y + 145), Color.Cyan, 0, new Vector2(0, 0), .8f, SpriteEffects.None, 0);
                spriteBatch.DrawString(font, Init.Player.Equipment.Ring.Name, new Vector2(position.X + 180, position.Y + 155), Color.White, 0, new Vector2(0, 0), .8f, SpriteEffects.None, 0);
                spriteBatch.DrawString(font, Init.Player.Equipment.Ring.Description, new Vector2(position.X + 180, position.Y + 165), Color.White, 0, new Vector2(0, 0), .8f, SpriteEffects.None, 0);
            }
        }

        public override void LoadContent(ContentManager content)
        {
            statusTexture = new Texture2D(Game1.graphics.GraphicsDevice, 1, 1);
            font = content.Load<SpriteFont>(@"interface\font");
            Color interfaceColor = new Color(0f, 0f, 0f, 0.8f);
            statusTexture.SetData(new[] { interfaceColor });
        }

        public override void Update(GameTime gameTime)
        {
            position = new Vector2(Init.Player.Position.X - 175, Init.Player.Position.Y - 90);
            statusInterface = new Rectangle((int)position.X, (int)position.Y, 350, 200);
        }
    }
}
