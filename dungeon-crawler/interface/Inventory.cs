using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo.Game;
using DungeonCrawler.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Sprites;

namespace DungeonCrawler.Interface
{
    public class Inventory : Scene
    {
        Texture2D inventoryTexture;
        Texture2D selectedItemTexture;
        Rectangle inventoryInterface;
        SpriteFont inventoryFont;
        KeyboardState oldKeyboardState;
        KeyboardState newKeyboardState;

        public int SelectedItem { get; set; }
        public List<Item> ItemList = Player.InventoryList;
        public List<Armor> ArmorList = new List<Armor>();

        public bool InventoryOpen { get; set; }
        public string MenuTitle { get; set; }
        public string InventoryType { get; set; }
        Vector2 Position { get; set; }
        public int TotalItems { get; set; }
        public static int TotalKeys { get; set; }

        public int itemSlot;

        public bool InventoryFull { get; set; }

        public Inventory(ContentManager content)
        {
            LoadContent(content);
        }


        public List<Item> Contents = new List<Item>();

        // Store a mock event for selecting items in the menu.
        // Otherwise items will sell upon entering the menu.
        private int actionCount = 0;

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
            SelectedItem = 0;
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
                if (SelectedItem != ItemList.Count)
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
                if (actionCount == 0)
                {
                    actionCount++;
                }
                else
                {
                    itemUsed = true;
                }
            }

            // Reset selection if reached the end.
            if (SelectedItem == 32)
            {
                SelectedItem = 0;
            }

            if (!InventoryOpen)
            {
                frames = 0;
                actionCount = 0;
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
                spriteBatch.DrawString(inventoryFont, MenuTitle, new Vector2(Position.X + 25, Position.Y), Color.White, 0, new Vector2(0, 0), 1.5f, SpriteEffects.None, 0);
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
            foreach (Item item in ItemList)
            {
                if (emptySlots < 32)
                {
                    if (item.ItemTexture == null)
                    {
                        emptySlots++;
                    }
                }
            }

            // If inventory is empty, assign the item to the first slot.
            if (emptySlots == ItemList.Count)
            {
                slot = 0;
            }
            else
            {
                // Find the next open slot.
                foreach (Item item in ItemList)
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

            foreach (Item item in ItemList)
            {
                if (itemToAdd.ID == item.ID)
                {
                    isDuplicate = true;
                    duplicateItemSlot = item.Index;
                }
            }
            return isDuplicate;
        }

        bool itemUsed = false;

        public void GenerateGrid()
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
                    ItemList.Add(newItem);
                    x += 31;
                }

                x = (int)Position.X + 25;
                y += 31;
            }


            // Assign each item an index.
            for (int i = 0; i < ItemList.Count; i++)
            {
                ItemList[i].Index = i;
            }

            TotalItems = 0;

            foreach (Item item in Contents)
            {
                AssignItem(item);
            }

            if (itemUsed)
            {
                if (InventoryType == "SELL_SHOP")
                {
                    Item itemToSell = null;
                    foreach (Item item in Init.ItemInventory.Contents)
                    {
                        if (item.ID == ItemList[SelectedItem].ID)
                        {
                            itemToSell = item;
                        }
                    }

                    if (itemToSell != null)
                    {
                        Init.ItemInventory.ItemCount--;
                        if (itemToSell.Quantity > 1)
                        {
                            itemToSell.Quantity -= 1;
                        }
                        else
                        {
                            Init.ItemInventory.Contents.Remove(itemToSell);
                            Init.SpellInventory.Contents.Remove(itemToSell);
                        }

                        // Unequip armor if it's sold.
                        Armor armorToRemove = null;
                        foreach (Armor armor in Init.ItemInventory.ArmorList)
                        {
                            if (armor.ID == itemToSell.ID)
                            {
                                armorToRemove = armor;
                            }
                        }

                        if (armorToRemove != null)
                        {
                            Init.Player.Equipment.Unequip(armorToRemove);
                            Init.Player.ApplyArmorStats();
                            Init.ItemInventory.ArmorList.Remove(armorToRemove);
                            Init.SpellInventory.ArmorList.Remove(armorToRemove);
                        }
                        Init.Player.Gold += ItemList[SelectedItem].Price;
                    }
                }
                else if (InventoryType == "SPELL_SHOP" || InventoryType == "ITEM_SHOP")
                {
                    if (ItemList[SelectedItem].Price > Init.Player.Gold)
                    {
                        Init.Message = "You don't have enough gold.";
                        Init.MessageEnabled = true;
                    }
                    else
                    {
                        Init.Player.Gold -= ItemList[SelectedItem].Price;
                        AddItem(ItemList[SelectedItem]);
                    }
                }
                else if (InventoryType == "PLAYER_INVENTORY")
                {
                    Item foundItem = null;

                    // Equipping armor.
                    foreach (Armor armor in ArmorList)
                    {
                        if (armor.ID == ItemList[SelectedItem].ID)
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
                            Init.Player.ApplyArmorStats();
                            Init.Message = armor.Name + " equipped.";
                            Init.MessageEnabled = true;
                        }
                    }

                    // Using an item.
                    foreach (Item item in Init.ItemInventory.Contents)
                    {
                        if (item.ID == ItemList[SelectedItem].ID && item.ID < 500)
                        {
                            switch (item.ID)
                            {
                                // Health Potion
                                case (3):
                                    Init.Player.RestoreHealth(50);
                                    break;
                                // Mana Potion
                                case (4):
                                    Init.Player.RestoreMana(50);
                                    break;
                                case (11):
                                    Init.SelectedMap.FadeIn();
                                    Init.ItemInventory.InventoryOpen = false;
                                    Init.TransitionState = true;
                                    Init.SelectedLevel = Init.levelList.Find(l => l.GetLevelName() == "CASTLE");
                                    Init.SelectedScene = (Init.Scene)Enum.Parse(typeof(Init.Scene), "CASTLE");
                                    Init.Player.Position = Init.SelectedLevel.GetStartingPosition();
                                    break;
                                default:
                                    Item spell = Items.GetItemById(item.ID);
                                    Init.Player.LearnSpell(spell);
                                    break;
                            }
                            item.Quantity -= 1;
                            AssignItem(item);
                            foundItem = item;
                        }
                    }

                    if (foundItem != null && foundItem.Quantity <= -1)
                    {
                        Init.ItemInventory.ItemCount--;
                        Init.ItemInventory.Contents.Remove(foundItem);
                    }
                }
                else if (InventoryType == "spells")
                {
                    Init.Message = ItemList[SelectedItem].Name;
                    Init.MessageEnabled = true;
                    Player.SelectedItem = ItemList[SelectedItem];
                }
            }
        }

        public void AddArmor(Armor newArmor)
        {
            ArmorList.Add(newArmor);
            AddItem(newArmor);
        }

        public int ItemCount = 0;
        public void AddItem(Item newItem)
        {
            bool exists = false;
            TotalItems++;

            foreach (Item item in Init.ItemInventory.Contents)
            {
                if (item.ID == newItem.ID)
                {
                    item.Quantity += 1;
                    AssignItem(item);
                    exists = true;
                }
            }

            if (!exists)
            {
                if (ItemCount > 31)
                {
                    Init.Message = "Inventory full.";
                    Init.MessageEnabled = true;
                }
                else
                {
                    ItemCount++;
                    Init.ItemInventory.Contents.Add(newItem);
                }
            }

        }
        /// <summary>
        /// Assigns an item to a slot on the inventory screen.
        /// </summary>
        /// <param name="item">Item Name</param>
        public void AssignItem(Item item)
        {
            itemSlot = AssignSlot(item);
            if (itemSlot < 32)
            {
                ItemList[itemSlot].ItemTexture = item.ItemTexture;
                ItemList[itemSlot].Name = item.Name;
                ItemList[itemSlot].Description = item.Description;
                ItemList[itemSlot].ID = item.ID;
                ItemList[itemSlot].Price = item.Price;
                ItemList[itemSlot].Quantity = item.Quantity;
            }
        }

        // Create a delay before drawing to allow time for positioning to update correctly.
        int frames = 0;
        public void DrawItems(SpriteBatch spriteBatch)
        {
            GenerateGrid();
            itemUsed = false;

            frames++;
            // Player Gold
            spriteBatch.Draw(Sprites.GetTexture("GOLD_ICON"), new Vector2(Position.X + 275, Position.Y - 25));
            spriteBatch.DrawString(inventoryFont, Init.Player.Gold.ToString(), new Vector2(Position.X + 295, Position.Y - 20), Color.White, 0, new Vector2(0, 0), .7f, SpriteEffects.None, 0);

            // Quit button
            spriteBatch.DrawString(inventoryFont, "Q - quit", new Vector2(Position.X + 295, Position.Y + 200), Color.White, 0, new Vector2(0, 0), .7f, SpriteEffects.None, 0);
            if (frames > 20)
            {
                for (int i = 0; i < ItemList.Count; i++)
                {
                    if (ItemList[i].ItemTexture != null)
                    {
                        // Draw the item texture on the inventory slot.
                        spriteBatch.Draw(ItemList[i].ItemTexture, new Rectangle(ItemList[i].ItemRectangle.X, ItemList[i].ItemRectangle.Y, 32, 32), Color.White);

                        Armor equippedArmor = ArmorList.Find(armor => armor.ID == ItemList[i].ID);
                        if (equippedArmor != null && equippedArmor.Equipped)
                        {

                            spriteBatch.DrawString(inventoryFont, "E", new Vector2(ItemList[i].ItemRectangle.X, ItemList[i].ItemRectangle.Y), Color.LightGreen);
                        }
                        // Draw the item name and description.
                        spriteBatch.DrawString(inventoryFont, ItemList[SelectedItem].Name, new Vector2(Position.X + 25, Position.Y + 175), Color.LightGreen, 0, new Vector2(0, 0), 1f, SpriteEffects.None, 0);

                        // Draw player's inventory
                        if (ItemList[i].Quantity > 0 && InventoryType != "ITEM_SHOP" && InventoryType != "SPELL_SHOP")
                        {
                            spriteBatch.DrawString(inventoryFont, ItemList[i].Quantity.ToString(), new Vector2(ItemList[i].ItemRectangle.X + 25, ItemList[i].ItemRectangle.Y + 22), Color.White, 0, new Vector2(0, 0), 1f, SpriteEffects.None, 0);
                        }

                        spriteBatch.DrawString(inventoryFont, ItemList[SelectedItem].Description, new Vector2(Position.X + 25, Position.Y + 185), Color.White, 0, new Vector2(0, 0), .7f, SpriteEffects.None, 0);


                        // Item price
                        spriteBatch.Draw(Sprites.GetTexture("GOLD_ICON"), new Vector2(Position.X + 250, Position.Y + 180));
                        spriteBatch.DrawString(inventoryFont, ItemList[SelectedItem].Price.ToString(), new Vector2(Position.X + 270, Position.Y + 185), Color.White, 0, new Vector2(0, 0), .7f, SpriteEffects.None, 0);

                    }
                }
                int itemPositionX = ItemList[SelectedItem].ItemRectangle.X;
                int itemPositionY = ItemList[SelectedItem].ItemRectangle.Y;
                int itemWidth = ItemList[SelectedItem].ItemRectangle.Width;
                int itemHeight = ItemList[SelectedItem].ItemRectangle.Height;

                spriteBatch.Draw(selectedItemTexture, new Rectangle(itemPositionX, itemPositionY, 1, itemHeight + 1), Color.White);
                spriteBatch.Draw(selectedItemTexture, new Rectangle(itemPositionX, itemPositionY, itemWidth + 1, 1), Color.White);
                spriteBatch.Draw(selectedItemTexture, new Rectangle(itemPositionX + itemWidth, itemPositionY, 1, itemHeight + 1), Color.White);
                spriteBatch.Draw(selectedItemTexture, new Rectangle(itemPositionX, itemPositionY + itemHeight, itemWidth + 1, 1), Color.White);
            }

            if (ItemList.Count > 200)
            {
                ItemList.Clear();
            }
        }

        /// <summary>
        /// Move to next row on grid.
        /// </summary>
        public void MoveToNextRow()
        {
            bool found = false;

            foreach (Item item in ItemList)
            {
                if (item.Column == ItemList[SelectedItem].Column + 1
                    && item.Row == ItemList[SelectedItem].Row
                    && found == false)
                {
                    found = true;

                    if (item.Index <= ItemList.Count)
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

            foreach (Item item in ItemList)
            {
                if (item.Column == ItemList[SelectedItem].Column - 1
                     && item.Row == ItemList[SelectedItem].Row
                     && found == false)
                {
                    found = true;

                    if (item.Index <= ItemList.Count)
                    {
                        SelectedItem = item.Index;
                    }
                }
            }
        }
    }
}
