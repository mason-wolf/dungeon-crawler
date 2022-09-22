using System;
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

        public int SelectedItem { get; set; }
        public List<Item> itemList = Player.InventoryList;
        public bool InventoryOpen { get; set; }
        public string MenuTitle { get; set; }
        public string InventoryType { get; set; }
        Vector2 Position { get; set; }
        public int TotalItems { get; set; }
        public static int TotalKeys { get; set; }

        public int itemSlot;


        public Inventory(ContentManager content)
        {
            LoadContent(content);
        }


        public List<Item> Contents = new List<Item>();

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
            GenerateGrid();
        }

        public override void Update(GameTime gameTime)
        {
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
            if (newKeyboardState.IsKeyDown(Keys.A) && oldKeyboardState.IsKeyUp(Keys.A) && InventoryOpen)
            {
                if (SelectedItem != 0)
                {
                    SelectedItem--;
                }
            }

            // Move Down
            if (newKeyboardState.IsKeyDown(Keys.S) && oldKeyboardState.IsKeyUp(Keys.S) && InventoryOpen)
            {
                MoveToNextRow();
            }

            // Move Up
            if (newKeyboardState.IsKeyDown(Keys.W) && oldKeyboardState.IsKeyUp(Keys.W) && InventoryOpen)
            {
                MoveToPreviousRow();
            }

            // Use Item
            if (newKeyboardState.IsKeyDown(Keys.E) && oldKeyboardState.IsKeyUp(Keys.E) && InventoryOpen)
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

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (InventoryOpen)
            {
                spriteBatch.Draw(inventoryTexture, null, inventoryInterface);
                spriteBatch.DrawString(inventoryFont, MenuTitle, new Vector2(Position.X + 25, Position.Y + 10), Color.White, 0, new Vector2(0, 0), 1.5f, SpriteEffects.None, 0);
                DrawItems(spriteBatch);
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

            foreach (Item item in Contents)
            {
                AssignItem(item);
            }

            if (itemUsed)
            {
                if (InventoryType == "shop")
                {
                    if (itemList[SelectedItem].Price > Init.Player.Gold)
                    {
                        Init.Message = "You don't have enough gold.";
                        Init.MessageEnabled = true;
                    }
                    else
                    {
                        Init.Player.Gold -= itemList[SelectedItem].Price;
                        bool exists = false;

                        foreach (Item item in Init.itemInventory.Contents)
                        {
                            if (item.ID == itemList[SelectedItem].ID)
                            {
                                item.Quantity += 1;
                                AssignItem(item);
                                exists = true;
                            }
                        }

                        if (!exists)
                        {
                            Init.itemInventory.Contents.Add(itemList[SelectedItem]);
                        } 
                    }
                }
                else if (InventoryType == "inventory")
                {
                    Item foundItem = null;

                    foreach (Item item in Init.itemInventory.Contents)
                    {
                        if (item.ID == itemList[SelectedItem].ID)
                        {
                            if (item.ID == 3)
                            {
                                Init.Player.CurrentHealth += 25;
                            }
                            item.Quantity -= 1;
                            AssignItem(item);
                            foundItem = item;
                        }
                    }

                    if (foundItem != null && foundItem.Quantity == -1)
                    {
                        Init.itemInventory.Contents.Remove(foundItem);
                    }
                }
                else if (InventoryType == "spells")
                {
                    Init.Message = itemList[SelectedItem].Name;
                    Init.MessageEnabled = true;
                    Player.SelectedItem = itemList[SelectedItem];
                }
            }
        }

        /// <summary>
        /// Assigns an item to a slot on the inventory screen.
        /// </summary>
        /// <param name="item">Item Name</param>
        public void AssignItem(Item item)
        {
            TotalItems++;
            itemSlot = AssignSlot(item);
            itemList[itemSlot].ItemTexture = item.ItemTexture;
            itemList[itemSlot].Name = item.Name;
            itemList[itemSlot].Description = item.Description;
            itemList[itemSlot].ID = item.ID;
            itemList[itemSlot].Price = item.Price;
            itemList[itemSlot].Quantity = item.Quantity;
        }

        // Create a delay before drawing to allow time for positioning to update correctly.
        int frames = 0;
        public void DrawItems(SpriteBatch spriteBatch)
        {
            GenerateGrid();
            itemUsed = false;

            frames++;
            
            if (frames > 20)
            {
                for (int i = 0; i < itemList.Count; i++)
                {
                    if (itemList[i].ItemTexture != null)
                    {
                        // Draw the item texture on the inventory slot.
                        spriteBatch.Draw(itemList[i].ItemTexture, new Rectangle(itemList[i].ItemRectangle.X, itemList[i].ItemRectangle.Y, 32, 32), Color.White);

                        // Draw the item name and description.
                        spriteBatch.DrawString(inventoryFont, itemList[SelectedItem].Name, new Vector2(Position.X + 25, Position.Y + 175), Color.LightGreen, 0, new Vector2(0, 0), 1f, SpriteEffects.None, 0);

                        if (itemList[i].Quantity != 0 && InventoryType != "shop" && itemList[i].Quantity != -1)
                        {
                            spriteBatch.DrawString(inventoryFont, itemList[i].Quantity.ToString(), new Vector2(itemList[i].ItemRectangle.X + 25, itemList[i].ItemRectangle.Y + 22), Color.White, 0, new Vector2(0, 0), 1f, SpriteEffects.None, 0);
                        }
       
                        spriteBatch.DrawString(inventoryFont, itemList[SelectedItem].Description, new Vector2(Position.X + 25, Position.Y + 185), Color.White, 0, new Vector2(0, 0), .7f, SpriteEffects.None, 0);

                        if (InventoryType == "shop" && itemList[SelectedItem].Price != 0)
                        {
                            // Item price
                            spriteBatch.Draw(Sprites.GetTexture("GOLD_ICON"), new Vector2(Position.X + 250, Position.Y + 180));
                            spriteBatch.DrawString(inventoryFont, itemList[SelectedItem].Price.ToString(), new Vector2(Position.X + 270, Position.Y + 185), Color.White, 0, new Vector2(0, 0), .7f, SpriteEffects.None, 0);

                            // Player Gold
                            spriteBatch.Draw(Sprites.GetTexture("GOLD_ICON"), new Vector2(Position.X + 275, Position.Y - 25));
                            spriteBatch.DrawString(inventoryFont, Init.Player.Gold.ToString(), new Vector2(Position.X + 295, Position.Y - 20), Color.White, 0, new Vector2(0, 0), .7f, SpriteEffects.None, 0);
                        }
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
        public void MoveToNextRow()
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
        public void MoveToPreviousRow()
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
