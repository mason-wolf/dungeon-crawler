﻿using Demo.Game;
using DungeonCrawler.Engine;
using DungeonCrawler.Interface;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonCrawler.Scenes
{
    class LoadMenu : EscapeMenu
    {
        Game game;
        GameWindow window;
        ContentManager content;
        int selectedIndex = 0;
        int[] savedSlots = new int[3];
        Texture2D floppy;
        Texture2D transitionTexture;
        bool fadeIn = false;
        bool hasFaded = false;
        public Color transitionColor;
        new int SelectedIndex
        {
            get { return selectedIndex; }
            set
            {
                selectedIndex = (int)MathHelper.Clamp(value, 0, 2);
            }
        }

        public LoadMenu(Game game, GameWindow window, ContentManager content) : base(game, window, content)
        {
            this.game = game;
            this.window = window;
            this.content = content;
            spriteBatch = (SpriteBatch)game.Services.GetService(typeof(SpriteBatch));
            floppy = content.Load<Texture2D>(@"interface\floppy");
            LoadContent(content);
            CheckSaves();
            transitionTexture = new Texture2D(Game1.graphics.GraphicsDevice, 1, 1);
            transitionTexture.SetData(new Color[] { Color.Black });
            transitionColor = new Color(0, 0, 0, 0);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            textPosition = Position;
            Rectangle buttonRectangle = new Rectangle((int)Position.X - 50, (int)Position.Y + 75, 125, 25);
            Rectangle floppyRectangle = new Rectangle((int)Position.X - 50, (int)Position.Y + 75, 32, 32);
            Color color = Color.White;
            Vector2 titlePosition = new Vector2(Position.X - 50, Position.Y + 55);
            spriteBatch.DrawString(spriteFont, "Load Game", titlePosition, color);

            Rectangle[] saveSlots = new Rectangle[3];

            // Draw three save slots.
            for (int i = 0; i < 3; i++)
            {
                if (i == SelectedIndex)
                {
                    spriteBatch.Draw(menuItemSelected, buttonRectangle, Color.White);
                }
                else
                {
                    spriteBatch.Draw(menuItem, buttonRectangle, Color.White);
                }

                textPosition = new Vector2(buttonRectangle.X + (menuItem.Width / 2), buttonRectangle.Y + (menuItem.Height / 2) + 5);
                Vector2 textSize = spriteFont.MeasureString("File");
                textPosition.X -= textSize.X / 2;
                textPosition.Y -= spriteFont.LineSpacing / 3;
                spriteBatch.DrawString(spriteFont, "File " + (i + 1), textPosition, color);
                saveSlots[i] = buttonRectangle;
                saveSlots[i].Width = 16;
                saveSlots[i].Height = 16;
                saveSlots[i].X = buttonRectangle.X + 100;
                saveSlots[i].Y = buttonRectangle.Y + 5;
                buttonRectangle.Y += 25;
                buttonRectangle.Y += 5;
            }

            // Draw the saved slots for the files found.
            for (int i = 0; i < savedSlots.Length; i++)
            {
                if (savedSlots[i] == 1)
                {
                    spriteBatch.Draw(floppy, saveSlots[i], Color.White);
                }
            }

            if (fadeIn == true)
            {
                spriteBatch.Draw(transitionTexture, new Rectangle(0, 0, 1080, 1800), transitionColor);
            }
        }

        /// <summary>
        /// Checks if saves exists.
        /// </summary>
        void CheckSaves()
        {
            for (int i = 0; i < savedSlots.Length; i++)
            {
                if (File.Exists("Save_" + (i + 1) + ".txt"))
                {
                    savedSlots[i] = 1;
                }
            }
        }

        int selectedSave = 0;
        public override void Update(GameTime gameTime)
        {
            if (fadeIn && hasFaded == false)
            {
                transitionColor.A += 15;
                transitionColor.B += 15;
                transitionColor.G += 15;

                if (transitionColor.A >= 255)
                {
                    hasFaded = true;
                }
            }

            KeyboardState newState = Keyboard.GetState();

            if (newState.IsKeyDown(Keys.S) && oldState.IsKeyUp(Keys.S))
            {
                SelectedIndex++;
            }

            if (newState.IsKeyDown(Keys.W) && oldState.IsKeyUp(Keys.W))
            {
                SelectedIndex--;
            }
            
            // Save Slot 1
            if (SelectedIndex == 0 && newState.IsKeyDown(Keys.E) && oldState.IsKeyUp(Keys.E))
            {
                fadeIn = true;
                selectedSave = 1;
            }

            // Save Slot 2
            if (SelectedIndex == 1 && newState.IsKeyDown(Keys.E) && oldState.IsKeyUp(Keys.E))
            {
                fadeIn = true;
                selectedSave = 2;
            }

            // Save Slot 3
            if (SelectedIndex == 2 && newState.IsKeyDown(Keys.E) && oldState.IsKeyUp(Keys.E))
            {
                fadeIn = true;
                selectedSave = 3;
            }

            if (hasFaded)
            {
                Init.Reload();
                LoadGame(selectedSave);
            }
            oldState = newState;
        }


        /// <summary>
        /// Load saved game from file.
        /// </summary>
        /// <param name="saveSlot"></param>
        void LoadGame(int saveSlot)
        {
            int lineCount = 0;
            using (StreamReader streamReader = new StreamReader("Save_" + saveSlot + ".txt"))
            {
                string line;
                Init.ItemInventory.ItemCount = 0;
                Init.ItemInventory.Contents.Clear();
                Init.ItemInventory.ArmorList.Clear();
                Init.SpellInventory.Contents.Clear();
                Random randomID = new Random();
                while ((line = streamReader.ReadLine()) != null)
                {
                    lineCount++;
                    string value = line.Split('=').First();
                    switch(value)
                    {
                        case ("PLAYER_HEALTH"):
                            value = line.Split('=').Last();
                            Init.Player.CurrentHealth = (int)Math.Round(Double.Parse(value));
                            break;
                        case ("PLAYER_MAX_HEALTH"):
                            value = line.Split('=').Last();
                            Init.Player.MaxHealth = (int)Math.Round(Double.Parse(value));
                            break;
                        case ("PLAYER_LEVEL"):
                            value = line.Split('=').Last();
                            Init.Player.Level = Int32.Parse(value);
                            break;
                        case ("PLAYER_SPELLPOWER"):
                            value = line.Split('=').Last();
                            Init.Player.SpellPower = Int32.Parse(value);
                            break;
                        case ("PLAYER_XP"):
                            value = line.Split('=').Last();
                            Init.Player.XP = (int)Math.Round(Double.Parse(value));
                            break;
                        case ("PLAYER_XP_REMAINING"):
                            value = line.Split('=').Last();
                            Init.Player.XPRemaining = (int)Math.Round(Double.Parse(value));
                            break;
                        case ("PLAYER_MANA"):
                            value = line.Split('=').Last();
                            Init.Player.CurrentMana = (int)Math.Round(Double.Parse(value));
                            break;
                        case ("ENEMIES_KILLED"):
                            value = line.Split('=').Last();
                            Init.Player.EnemiesKilled = Int32.Parse(value);
                            break;
                        case ("PLAYER_GOLD"):
                            value = line.Split('=').Last();
                            Init.Player.Gold = Int32.Parse(value);
                            break;
                        case ("INVENTORY_ITEM"):
                            value = line.Split('=').Last();
                            string[] tempItem = value.Split(',');
                            int quantity = Int32.Parse(tempItem[1]);
                            int itemId = Int32.Parse(tempItem[2]);
                            Item item = new Item();
                            quantity += 1;
                            item.ID = itemId;
                            item = Items.GetItemById(item.ID);
                            for (var i = 0; i < quantity; i++)
                            {
                                Init.ItemInventory.AddItem(item);
                            }
                            break;
                        case ("ARMOR_ITEM"):
                            value = line.Split('=').Last();
                            string[] tempArmor = value.Split(',');
                            Armor armor = new Armor();
                            armor.ID = randomID.Next(501, 9999);
                            armor.Name = tempArmor[0];
                            armor.Description = tempArmor[1];
                            armor.FireResistance = Int32.Parse(tempArmor[2]);
                            armor.FrostResistance = Int32.Parse(tempArmor[3]);
                            armor.ThunderResistance = Int32.Parse(tempArmor[4]);
                            armor.HealthBonus = Int32.Parse(tempArmor[5]);
                            armor.ManaBonus = Int32.Parse(tempArmor[6]);
                            armor.Type = (Armor.ArmorType)Enum.Parse(typeof(Armor.ArmorType), tempArmor[7]);
                            armor.Equipped = bool.Parse(tempArmor[8]);
                            armor.Price = Int32.Parse(tempArmor[9]);
                            switch (armor.Type)
                            {
                                case (Armor.ArmorType.BOOTS):
                                    armor.ItemTexture = Sprites.GetTexture("BOOTS_1_ICON");
                                    break;
                                case (Armor.ArmorType.CHEST):
                                    armor.ItemTexture = Sprites.GetTexture("ROBE_1_ICON");
                                    break;
                                case (Armor.ArmorType.HANDS):
                                    armor.ItemTexture = Sprites.GetTexture("GLOVES_1_ICON");
                                    break;
                                case (Armor.ArmorType.RING):
                                    armor.ItemTexture = Sprites.GetTexture("RING_1_ICON");
                                    break;
                                case (Armor.ArmorType.HEAD):
                                    armor.ItemTexture = Sprites.GetTexture("HAT_1_ICON");
                                    break;
                            }

                            Init.ItemInventory.AddArmor(armor);

                            if (armor.Equipped)
                            {
                                switch (armor.Type)
                                {
                                    case (Armor.ArmorType.BOOTS):
                                        Init.Player.Equipment.Unequip(Init.Player.Equipment.Boots);
                                        Init.Player.Equipment.Boots = armor;
                                        armor.Equipped = true;
                                        break;
                                    case (Armor.ArmorType.HEAD):
                                        Init.Player.Equipment.Unequip(Init.Player.Equipment.Head);
                                        Init.Player.Equipment.Head = armor;
                                        armor.Equipped = true;
                                        break;
                                    case (Armor.ArmorType.HANDS):
                                        Init.Player.Equipment.Unequip(Init.Player.Equipment.Hands);
                                        Init.Player.Equipment.Hands = armor;
                                        armor.Equipped = true;
                                        break;
                                    case (Armor.ArmorType.RING):
                                        Init.Player.Equipment.Unequip(Init.Player.Equipment.Ring);
                                        Init.Player.Equipment.Ring = armor;
                                        armor.Equipped = true;
                                        break;
                                    case (Armor.ArmorType.CHEST):
                                        Init.Player.Equipment.Unequip(Init.Player.Equipment.Chest);
                                        Init.Player.Equipment.Chest = armor;
                                        armor.Equipped = true;
                                        break;
                                }
                            }

                            Init.Player.ApplyArmorStats();
                            break;
                        case ("SPELL"):
                            value = line.Split('=').Last();
                            string[] tempSpell = value.Split(',');
                            int spellId = Int32.Parse(tempSpell[1]);
                            Item spell = Items.GetItemById(spellId);
                            spell.Name = tempSpell[0];
                            Init.SpellInventory.Contents.Add(spell);
                            Player.SelectedItem = Items.GetItemById(1);
                            break;
                    }
                }
            }

            Init.ItemInventory.ItemList.Clear();
            // If save is empty, reset player's stats.
            if (lineCount == 0)
            {
                Init.Player.MaxHealth = 100;
                Init.Player.MaxMana = 100;
                Init.Player.CurrentMana = 100;
                Init.Player.CurrentHealth = 100;
                Init.Player.Gold = 0;
                Init.Player.EnemiesKilled = 0;
                Init.SpellInventory.Contents.Clear();
                Init.ItemInventory.Contents.Clear();
            }
        }
    }
}
