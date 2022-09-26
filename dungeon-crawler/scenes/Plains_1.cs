
using DungeonCrawler.Engine;
using DungeonCrawler;
using Humper;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoGame.Extended.Sprites;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;
using Microsoft.Xna.Framework.Input;
using DungeonCrawler.Interface;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace DungeonCrawler.Scenes
{
    public class Plains_1 : SceneLogic
    {
        public override List<MapObject> MapObjects { get; set; }
        public override ContentManager ContentManager { get; set; }
        public override Map Map { get; set; }

        public override void Draw(SpriteBatch spriteBatch)
        {
        }

        public override void LoadContent(ContentManager content)
        {
        }

        public override void LoadScene()
        {

        }

        public override void Update(GameTime gameTime)
        {
            foreach (MapObject mapObject in MapObjects)
            {
                if (Init.Player.BoundingBox.Intersects(mapObject.GetBoundingBox()) && Player.ActionButtonPressed && mapObject.GetName() == "CHEST")
                {
                    if (!mapObject.Interacted())
                    {
                        mapObject.GetSprite().Play("Opened");
                        Random randomGold = new Random();
                        int gold = randomGold.Next(1, 50);
                        Init.Player.Gold += gold;
                        Init.Message = "You obtained " + gold + " gold.";
                        Init.MessageEnabled = true;
                        mapObject.Interact();
                    }
                }
            }

        }
    }
}
