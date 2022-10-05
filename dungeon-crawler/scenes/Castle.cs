
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
using Demo.Game;
using System.Diagnostics;
using Demo.Interface;

namespace DungeonCrawler.Scenes
{
    public class Castle : SceneLogic
    {
        Song levelThemeSong;
        List<Entity> NPCList = new List<Entity>();


        public Castle()
        {
        }
        public override List<MapObject> MapObjects { get; set; }
        public override ContentManager ContentManager { get; set;  }
        public override Map Map { get; set; }

        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (Entity npc in NPCList)
            {
                npc.Draw(spriteBatch);
            }
        }

        public override void LoadContent(ContentManager content)
        {
          //  levelThemeSong = content.Load<Song>(@"music\level_1");
            //     MediaPlayer.IsRepeating = true;
            //    MediaPlayer.Play(levelThemeSong);
        }

        public override void LoadScene()
        {
            int[] npcIDs = { 
                15, 16, 17, 18, 19, 20, 21, 22, 
                23, 24, 25, 26, 27, 28, 29, 30, 
                31, 32, 33, 34, 35, 36, 37
            };

            foreach(int npcId in npcIDs)
            {
                Entity npc = Level.GetNpcByID(npcId.ToString());

                switch(npc.ID)
                {
                    case ("18"):
                        npc.Sprite.Play("idleWest1");
                        break;
                    case ("33"):
                        npc.Sprite.Play("idleEast1");
                        break;
                    default:
                        npc.Sprite.Play("idleSouth1");
                        break;
                }
                NPCList.Add(npc);
            }
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
                        Loot loot = lootGenerator.GenerateLoot();

                        if (loot.Gold > 0)
                        {
                            Init.Player.Gold += loot.Gold;
                            Init.Message = "You obtained " + loot.Gold + " gold.";
                        }
                        else
                        {
                            Init.Message = "You obtained " + loot.Armor.Name + ".";
                            Init.ItemInventory.AddArmor(loot.Armor);
                        }

                        Init.MessageEnabled = true;
                        Player.ActionButtonPressed = false;
                        mapObject.Interact();
                        mapObject.GetSprite().Play("Opened");
                    }
                }
            }

            foreach (Entity npc in NPCList)
            {
                npc.Update(gameTime);
                if (npc == null)
                {
                    Console.WriteLine("NPC not found. Are you missing an NPC Name or ID?");
                }
                if (Init.Player.BoundingBox.Intersects(npc.BoundingBox))
                {
                    string message = "";
                    switch (npc.ID)
                    {
                        case ("15"):
                            message = 
                                "\nI am the warden of flame." +
                                "\nYou'd be wise to heed my teachings.";
                            Init.DialogBox.Text = message;
                            Init.DialogBox.StartDialog = true;
                            break;
                        case ("16"):
                            message =
                                "\nSome foes are weaker to fire than others." +
                                "\nConserve your flame powers when facing them.";
                            Init.DialogBox.Text = message;
                            Init.DialogBox.StartDialog = true;
                            break;
                        case ("17"):
                            message =
                                "\nYou can purchase new spells from the bookkeeper." +
                                 "\nHe's in the library.";
                            Init.DialogBox.Text = message;
                            Init.DialogBox.StartDialog = true;
                            break;
                        case ("18"):
                            Init.shops.Open(Shop.ShopType.ITEM_SHOP);
                            break;
                        case ("19"):
                            message =
                                "\nLooking for the exit?" +
                                 "\nIt's been magically sealed off.";
                            Init.DialogBox.Text = message;
                            Init.DialogBox.StartDialog = true;
                            break;
                        case ("21"):
                            message =
                                "\nThe portal room is ahead." +
                                 "\nThe archmage will tell you more about them.";
                            Init.DialogBox.Text = message;
                            Init.DialogBox.StartDialog = true;
                            break;
                        case ("22"):
                            message =
                                "\nYou've killed " + Init.Player.EnemiesKilled + " foes. Nice.";
                            Init.DialogBox.Text = message;
                            Init.DialogBox.StartDialog = true;
                            break;
                        case ("23"):
                            message =
                                "\nI am the warden of frost." +
                                "\nFoes and environment alike may be manipulated with ice.";
                            Init.DialogBox.Text = message;
                            Init.DialogBox.StartDialog = true;
                            break;
                        case ("24"):
                            message =
                                "\nShh..I'm trying to study.";
                            Init.DialogBox.Text = message;
                            Init.DialogBox.StartDialog = true;
                            break;
                        case ("25"):
                            message =
                                "\nIf your frost powers are strong enough.." +
                                "\nYou could freeze bodies of water.";
                            Init.DialogBox.Text = message;
                            Init.DialogBox.StartDialog = true;
                            break;
                        case ("26"):
                            message =
                                "\nSome foes react differently to ice magic." +
                                "\nI've learned it's just trial and error.";
                            Init.DialogBox.Text = message;
                            Init.DialogBox.StartDialog = true;
                            break;
                        case ("27"):
                            message =
                                "\nI am the warden of thunder." +
                                "\nBehold the power of the mighty bolt.";
                            Init.DialogBox.Text = message;
                            Init.DialogBox.StartDialog = true;
                            break;
                        case ("28"):
                            message =
                                "\nThunder is my favorite element.";
                            Init.DialogBox.Text = message;
                            Init.DialogBox.StartDialog = true;
                            break;
                        case ("29"):
                            message =
                                "\nI can't seem to stay awake.";
                            Init.DialogBox.Text = message;
                            Init.DialogBox.StartDialog = true;
                            break;
                        case ("30"):
                            Init.shops.Open(Shop.ShopType.SPELL_SHOP);
                            break;
                        case ("31"):
                            message =
                                "\nI am the warden of life." +
                                "\nLet my wisdom replenish you in a time of need.";
                            Init.DialogBox.Text = message;
                            Init.DialogBox.StartDialog = true;
                            break;
                        case ("32"):
                            message =
                                "\nSpells aren't taught to you for free." +
                                "\nThe academy wishes us to earn them through practice." +
                                "\nAnd of course, with tuition.";
                            Init.DialogBox.Text = message;
                            Init.DialogBox.StartDialog = true;
                            break;
                        case ("33"):
                            message =
                                "\nThese books seem quite expensive.";
                            Init.DialogBox.Text = message;
                            Init.DialogBox.StartDialog = true;
                            break;
                        case ("34"):
                            message =
                                "\nInvesting in some life magic will save you from using healing potions.";
                            Init.DialogBox.Text = message;
                            Init.DialogBox.StartDialog = true;
                            break;
                        case ("37"):
                                foreach (Inventory inventory in Shop.Inventories) { 
                                    if (inventory.InventoryType == "SELL_SHOP")
                                    {
                                        inventory.Contents = Init.ItemInventory.Contents;
                                        inventory.ArmorList = Init.ItemInventory.ArmorList;
                                    }
                                }

                                Init.shops.Open(Shop.ShopType.SELL_SHOP);
                            break;
                    }
                }
            }
        }
    }
}
