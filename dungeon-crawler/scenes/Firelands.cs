using Demo.Game;
using DungeonCrawler;
using DungeonCrawler.Engine;
using DungeonCrawler.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Scenes
{
    public class Firelands : SceneLogic
    {
        public override Map Map { get; set; }
        public override ContentManager ContentManager { get; set; }
        public override List<MapObject> MapObjects { get; set; }

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
                        LootGenerator lootGenerator = new LootGenerator();
                        lootGenerator.Level = LootGenerator.LevelType.FIRELANDS;
                        Loot loot = lootGenerator.GenerateLoot();

                        if (loot.Gold > 0)
                        {
                            Init.Player.Gold += loot.Gold;
                            Init.Message = "You obtained " + loot.Gold + " gold.";
                        }
                        else if (loot.Item != null)
                        {
                            Init.ItemInventory.AddItem(loot.Item);
                            Init.Message = "You obtained a " + loot.Item.Name;
                        }
                        else
                        {
                            Init.Message = "You obtained " + loot.Armor.Name + ".";
                            Init.ItemInventory.AddArmor(loot.Armor);
                        }

                        Init.MessageEnabled = true;
                        Player.ActionButtonPressed = false;
                        if (mapObject.GetSprite() != null)
                        {
                            mapObject.Interact();
                            mapObject.GetSprite().Play("Opened");
                        }
                    }
                }
            }
        }
    }
}
