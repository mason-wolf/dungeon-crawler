﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DungeonCrawler.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Sprites;

namespace DungeonCrawler.Interface
{
    class Inventory : Scene
    {
        Texture2D inventoryTexture;
        Texture2D selectedItemTexture;
        Rectangle inventoryInterface;
        SpriteFont inventoryFont;
        KeyboardState oldKeyboardState;
        KeyboardState newKeyboardState;
        AnimatedSprite dynamiteSprite;
        Entity explosionEntity;
        public static int SelectedItem { get; set; }
        public static List<Item> itemList = Player.InventoryList;
        public static bool InventoryOpen { get; set; }
        public static bool SavedGameLoaded { get; set; }
        Vector2 Position { get; set; }
        public static int TotalItems { get; set; }
        public static int TotalChickens { get; set; }
        public static int TotalArrows { get; set; }
        public static int TotalDynamite { get; set;  }

        public static int TotalKeys { get; set; }
        public int itemSlot;
        public bool dynamiteUsed;
        public enum Items
        {
            Chicken,
            Dynamite
        }

        public Inventory(ContentManager content)
        {
            LoadContent(content);
        }

        public override void LoadContent(ContentManager content)
        {
            inventoryTexture = new Texture2D(Game1.graphics.GraphicsDevice, 1, 1);
            inventoryFont = content.Load<SpriteFont>(@"interface\font");
            selectedItemTexture = new Texture2D(Game1.graphics.GraphicsDevice, 1, 1);
            Color selectedItemTextureColor = new Color(1f, 1f, 1f, 1f);
            Color interfaceColor = new Color(0f, 0f, 0f, 0.8f);
            inventoryTexture.SetData(new[] { interfaceColor });
            selectedItemTexture.SetData(new[] { selectedItemTextureColor });
            InventoryOpen = false;

            TotalDynamite = 0;
            TotalKeys = 1;

            if (!SavedGameLoaded)
            {
                TotalChickens = 12;
                TotalArrows = 25;
            }

            GenerateGrid();
            dynamiteSprite = new AnimatedSprite(Sprites.dynamiteAnimation);
            explosionEntity = new Entity(Sprites.explosionAnimation);
            explosionEntity.LoadContent(content);
        }

        public override void Update(GameTime gameTime)
        {
            explosionEntity.Update(gameTime);
            // Store & remember the selected item.
            if (InventoryOpen)
            {
                newKeyboardState = Keyboard.GetState();
            }

            Position = new Vector2(Init.Player.Position.X - 150, Init.Player.Position.Y - 90);
            inventoryInterface = new Rectangle((int)Position.X, (int)Position.Y, 300, 200);

            // Handle item selection in inventory menu: move right.
            if (newKeyboardState.IsKeyDown(Keys.D) && oldKeyboardState.IsKeyUp(Keys.D))
            {
                if (SelectedItem != itemList.Count)
                {
                    SelectedItem++;
                }
            }

            // Move Left
            if (newKeyboardState.IsKeyDown(Keys.A) && oldKeyboardState.IsKeyUp(Keys.A))
            {
                if (SelectedItem != 0)
                {
                    SelectedItem--;
                }
            }

            // Move Down
            if (newKeyboardState.IsKeyDown(Keys.S) && oldKeyboardState.IsKeyUp(Keys.S))
            {
                MoveToNextRow();
            }

            //Move Up
            if (newKeyboardState.IsKeyDown(Keys.W) && oldKeyboardState.IsKeyUp(Keys.W))
            {
                MoveToPreviousRow();
            }

            //Move Up
            if (newKeyboardState.IsKeyDown(Keys.E) && oldKeyboardState.IsKeyUp(Keys.E))
            {
                itemUsed = true;
            }

            // Reset selection if reached the end.
            if (SelectedItem == 32)
            {
                SelectedItem = 0;
            }

            if (!InventoryOpen)
            {
                frames = 0;
            }

            if (InventoryOpen)
            {
                oldKeyboardState = newKeyboardState;
                newKeyboardState = Keyboard.GetState();
            }

            dynamiteSprite.Update(gameTime);
        }

        Vector2 dynamitePosition;
        int dynamiteTimer = 0;
        int explosionTimer = 0;

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (InventoryOpen)
            {
                spriteBatch.Draw(inventoryTexture, null, inventoryInterface);
                spriteBatch.DrawString(inventoryFont, "Items", new Vector2(Position.X + 25, Position.Y + 10), Color.White, 0, new Vector2(0, 0), 1.5f, SpriteEffects.None, 0);
                DrawSelectedItem(spriteBatch);
            }

            if (dynamiteUsed && dynamiteTimer < 120)
            {
                dynamiteSprite.Play("burning");
                dynamiteSprite.Position = dynamitePosition;
                dynamiteSprite.Draw(spriteBatch);
                dynamiteTimer++;
            }
            else
            {
                if (dynamiteTimer >= 120)
                {
                    dynamiteUsed = false;
                    explosionEntity.Position = dynamitePosition;
                    explosionEntity.Sprite.Play("explosion");
                    explosionEntity.Draw(spriteBatch);

                    foreach(MapObject mapObject in Init.SelectedMap.GetMapObjects())
                    {
                        if (mapObject.GetName() == "Rock" && explosionEntity.BoundingBox.Intersects(mapObject.GetBoundingBox()) && !mapObject.IsDestroyed())
                        {
                            mapObject.GetSprite().Play("broken");
                            mapObject.Destroy();
                            Init.SelectedMap.GetWorld().Remove(mapObject.GetCollisionBox());
                        }
                    }
                    if (explosionTimer < 35)
                    {
                        explosionTimer++;
                    }
                    else
                    {
                        explosionEntity.Sprite.Play("idle");
                        dynamiteTimer = 0;
                        explosionTimer = 0;
                    }
                }
                else
                {
                    dynamitePosition = Init.Player.Position;
                }

            }
        }

        /// <summary>
        /// Adds an item to the player's inventory.
        /// </summary>
        /// <param name="item">Item Name</param>
        public void AddToInventory(Items item)
        {

            TotalItems++;
            switch (item)
            {
                case Items.Chicken:

                    Item chicken = new Item();
                    chicken.ItemTexture = Sprites.chickenTexture;

                    itemSlot = AssignSlot(chicken);

                    if (itemSlot < TotalItems)
                    {
                        itemList[itemSlot].ItemTexture = chicken.ItemTexture;
                        itemList[itemSlot].Name = "Chicken";
                        itemList[itemSlot].Description = "Restores health.";
                        itemList[itemSlot].Quantity = TotalChickens;
                    }

                    itemSlot = 0;
                    break;
                case Items.Dynamite: 

                    Item dynamite = new Item();
                    dynamite.ItemTexture = Sprites.singleDynamiteTexture;

                    itemSlot = AssignSlot(dynamite);

                    if (itemSlot < TotalItems)
                    {
                        itemList[itemSlot].ItemTexture = dynamite.ItemTexture;
                        itemList[itemSlot].Name = "Dynamite";
                        itemList[itemSlot].Description = "Blows stuff up.";
                        itemList[itemSlot].Quantity = TotalDynamite;
                    }

                    itemSlot = 0;
                    break;
            }


        }

        /// <summary>
        /// Assigns an inventory slot to an item. If the item is a duplicate, then assign it's current slot.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int AssignSlot(Item item)
        {
            int slotNumber = 0;

            if (!IsDuplicateItem(item))
            {
                slotNumber = GetEmptySlot();
            }
            else
            {
                slotNumber = duplicateItemSlot;
            }

            return slotNumber;
        }
        /// <summary>
        /// Gets the next empty slot in the inventory, if inventory is empty the first slot is 0.
        /// </summary>
        /// <returns></returns>
        public int GetEmptySlot()
        {
            int slot = 0;
            int emptySlots = 0;


            // Count number of items in inventory.
            foreach (Item item in itemList)
            {
                if (item.ItemTexture == null)
                {
                    emptySlots++;
                }
            }

            // If inventory is empty, assign the item to the first slot.
            if (emptySlots == itemList.Count)
            {
                slot = 0;
            }
            else
            {
                // Find the next open slot.
                foreach (Item item in itemList)
                {
                    if (item.ItemTexture != null)
                    {
                        slot++;
                    }
                }
            }

            return slot;
        }

        int duplicateItemSlot = 0;
        /// <summary>
        /// Checks to see if the item already exists in the inventory. 
        /// </summary>
        /// <param name="itemToAdd"></param>
        /// <returns></returns>
        public bool IsDuplicateItem(Item itemToAdd)
        {
            bool isDuplicate = false;

            foreach (Item item in itemList)
            {
                if (itemToAdd.ItemTexture == item.ItemTexture)
                {
                    isDuplicate = true;
                    duplicateItemSlot = item.Index;
                }
            }
            return isDuplicate;
        }

        bool itemUsed = false;

        void GenerateGrid()
        {
            int x = (int)Position.X + 25;
            int y = (int)Position.Y + 30;

            Rectangle itemSlot;

            // Generate and populate grid items.
            for (int i = 0; i < 4; ++i)
            {
                for (int j = 0; j < 8; ++j)
                {
                    // Create a new rectangle with the coordinates for the item.
                    itemSlot = new Rectangle(x, y, 32, 32);
                    Item newItem = new Item();
                    // Create a new item and assign the rectangle data.
                    newItem.ItemRectangle = itemSlot;
                    newItem.Column = i;
                    newItem.Row = j;
                    newItem.Name = "";
                    newItem.Description = "";
                    // Add the rectangle data to the inventory list.
                    itemList.Add(newItem);
                    x += 31;
                }

                x = (int)Position.X + 25;
                y += 31;
            }

            // Assign each item an index.
            for (int i = 0; i < itemList.Count; i++)
            {
                itemList[i].Index = i;
            }

            TotalItems = 0;

            for (int i = 0; i < TotalChickens; i++)
            {
                AddToInventory(Items.Chicken);
            }

            for (int i = 0; i < TotalDynamite; i++)
            {
                AddToInventory(Items.Dynamite);
            }
            if (itemUsed)
            {
                if (itemList[SelectedItem].Name == "Chicken")
                {
                    TotalChickens -= 1;
                    if (Init.Player.CurrentHealth < Init.Player.MaxHealth)
                    {
                        Init.Player.CurrentHealth += 25;
                    }
                }
                if (itemList[SelectedItem].Name == "Dynamite")
                {
                    TotalDynamite -= 1;
                    InventoryOpen = false;
                    dynamiteUsed = true;
                }
            }
        }

        // Create a delay before drawing to allow time for positioning to update correctly.
        int frames = 0;
        public void DrawSelectedItem(SpriteBatch spriteBatch)
        {
            GenerateGrid();
            itemUsed = false;

            frames++;

            if (frames > 10)
            {
                for (int i = 0; i < itemList.Count; i++)
                {
                    if (i < 32 && itemList[i].ItemTexture != null)
                    {
                        // Draw the item texture on the inventory slot.
                        spriteBatch.Draw(itemList[i].ItemTexture, new Rectangle(itemList[i].ItemRectangle.X, itemList[i].ItemRectangle.Y, 32, 32), Color.White);

                        // Draw the item name and description.
                        spriteBatch.DrawString(inventoryFont, itemList[SelectedItem].Name, new Vector2(Position.X + 25, Position.Y + 175), Color.LightGreen, 0, new Vector2(0, 0), 1f, SpriteEffects.None, 0);
                        spriteBatch.DrawString(inventoryFont, itemList[i].Quantity.ToString(), new Vector2(itemList[i].ItemRectangle.X + 25, itemList[i].ItemRectangle.Y + 22), Color.White, 0, new Vector2(0, 0), 1f, SpriteEffects.None, 0);
                        spriteBatch.DrawString(inventoryFont, itemList[SelectedItem].Description, new Vector2(Position.X + 25, Position.Y + 185), Color.White, 0, new Vector2(0, 0), .7f, SpriteEffects.None, 0);
                    }
                }
                int itemPositionX = itemList[SelectedItem].ItemRectangle.X;
                int itemPositionY = itemList[SelectedItem].ItemRectangle.Y;
                int itemWidth = itemList[SelectedItem].ItemRectangle.Width;
                int itemHeight = itemList[SelectedItem].ItemRectangle.Height;

                spriteBatch.Draw(selectedItemTexture, new Rectangle(itemPositionX, itemPositionY, 1, itemHeight + 1), Color.White);
                spriteBatch.Draw(selectedItemTexture, new Rectangle(itemPositionX, itemPositionY, itemWidth + 1, 1), Color.White);
                spriteBatch.Draw(selectedItemTexture, new Rectangle(itemPositionX + itemWidth, itemPositionY, 1, itemHeight + 1), Color.White);
                spriteBatch.Draw(selectedItemTexture, new Rectangle(itemPositionX, itemPositionY + itemHeight, itemWidth + 1, 1), Color.White);
            }

            if (itemList.Count > 200)
            {
                itemList.Clear();
            }
        }

        /// <summary>
        /// Move to next row on grid.
        /// </summary>
        public static void MoveToNextRow()
        {
            bool found = false;

            foreach (Item item in itemList)
            {
                if (item.Column == itemList[SelectedItem].Column + 1
                    && item.Row == itemList[SelectedItem].Row
                    && found == false)
                {
                    found = true;

                    if (item.Index <= itemList.Count)
                    {
                        SelectedItem = item.Index;
                    }
                }
            }
        }

        /// <summary>
        /// Move to previous row on grid.
        /// </summary>
        public static void MoveToPreviousRow()
        {
            bool found = false;

            foreach (Item item in itemList)
            {
                if (item.Column == itemList[SelectedItem].Column - 1
                     && item.Row == itemList[SelectedItem].Row
                     && found == false)
                {
                    found = true;

                    if (item.Index <= itemList.Count)
                    {
                        SelectedItem = item.Index;
                    }
                }
            }
        }
    }
}
