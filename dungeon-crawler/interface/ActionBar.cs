using DungeonCrawler;
using DungeonCrawler.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Interface
{
    public class ActionBar : Scene
    {
        private Texture2D _actionBarTexture;

        public override void Draw(SpriteBatch spriteBatch)
        {
            Vector2 position = new Vector2(Init.Player.Position.X - 170, Init.Player.Position.Y - 110);
            spriteBatch.Draw(_actionBarTexture, new Vector2(position.X + 135, position.Y + 190), new Rectangle(0, 0, 80, 200), Color.White);
            foreach (Item item in Init.SpellInventory.Contents)
            {
                // Draw action bar items.
                if (item.ActionBarSlot != 0)
                {
                    switch(item.ActionBarSlot)
                    {
                        case (1):
                            spriteBatch.Draw(item.ItemTexture, new Vector2(position.X + 135, position.Y + 198), new Rectangle(0, 0, 16, 16), Color.White);
                            Vector2 slot1 = new Vector2(position.X + 150, position.Y + 198);
                            spriteBatch.DrawString(Init.Font, item.ActionBarSlot.ToString(), slot1, Color.White, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0f);
                            break;
                        case (2):
                            spriteBatch.Draw(item.ItemTexture, new Vector2(position.X + 155, position.Y + 198), new Rectangle(0, 0, 16, 16), Color.White);
                            Vector2 slot2 = new Vector2(position.X + 170, position.Y + 198);
                            spriteBatch.DrawString(Init.Font, item.ActionBarSlot.ToString(), slot2, Color.White, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0f);
                            break;
                        case (3):
                            spriteBatch.Draw(item.ItemTexture, new Vector2(position.X + 175, position.Y + 198), new Rectangle(0, 0, 16, 16), Color.White);
                            Vector2 slot3 = new Vector2(position.X + 190, position.Y + 198);
                            spriteBatch.DrawString(Init.Font, item.ActionBarSlot.ToString(), slot3, Color.White, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0f);
                            break;
                        case (4):
                            spriteBatch.Draw(item.ItemTexture, new Vector2(position.X + 195, position.Y + 198), new Rectangle(0, 0, 16, 16), Color.White);
                            Vector2 slot4 = new Vector2(position.X + 210, position.Y + 198);
                            spriteBatch.DrawString(Init.Font, item.ActionBarSlot.ToString(), slot4, Color.White, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0f);
                            break;
                    }
                }
            }
        }

        public override void LoadContent(ContentManager content)
        {
            _actionBarTexture = content.Load<Texture2D>(@"interface\actionbar");
        }

        public override void Update(GameTime gameTime)
        {
        }
    }
}
