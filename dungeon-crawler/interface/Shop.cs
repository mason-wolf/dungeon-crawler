using DungeonCrawler;
using DungeonCrawler.Interface;
using DungeonCrawler.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Interface
{
    public class Shop : Scene
    {
        public static List<Inventory> Inventories = new List<Inventory>();
        private KeyboardState newKeyState;
        private KeyboardState oldKeyState;
        public static bool ShopOpen = false;
        private Inventory selectedShop;

        public enum ShopType
        {
            ITEM_SHOP,
            SPELL_SHOP,
            SELL_SHOP
        }

        public void Add(Inventory inventory)
        {
            Inventories.Add(inventory);
        }

        public void Open(ShopType shopType)
        {
            ShopOpen = true;
            switch (shopType)
            {
                case (ShopType.ITEM_SHOP):
                    selectedShop = Inventories.Find(inventory => inventory.InventoryType == ShopType.ITEM_SHOP.ToString());
                    break;
                case (ShopType.SPELL_SHOP):
                    selectedShop = Inventories.Find(inventory => inventory.InventoryType == ShopType.SPELL_SHOP.ToString());
                    break;
                case (ShopType.SELL_SHOP):
                    selectedShop = Inventories.Find(Inventory => Inventory.InventoryType == ShopType.SELL_SHOP.ToString());
                    break;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (Inventory inventory in Inventories)
            {
                inventory.Draw(spriteBatch);
            }

        }

        public override void LoadContent(ContentManager content)
        {
            throw new NotImplementedException();
        }

        public void ListenForInput(KeyboardState newState, KeyboardState oldState)
        {
            newKeyState = newState;
            oldKeyState = oldState;
        }

        public override void Update(GameTime gameTime)
        {
            foreach(Inventory inventory in Inventories)
            {
                inventory.Update(gameTime);
            }

            if (newKeyState.IsKeyDown(Keys.E) && oldKeyState.IsKeyUp(Keys.E) 
                && ShopOpen 
                && !Init.ItemInventory.InventoryOpen 
                && !Init.SpellInventory.InventoryOpen
                && !Init.Player.InMenu
                )
            {
                Init.Player.InMenu = true;
                selectedShop.InventoryOpen = true;
            }

            if (newKeyState.IsKeyDown(Keys.Q) && oldKeyState.IsKeyUp(Keys.Q) && ShopOpen)
            {
                if (selectedShop.InventoryOpen)
                {
                    Init.Player.InMenu = false;
                    selectedShop.InventoryOpen = false;
                    ShopOpen = false;
                }
            }

        }

        public static bool InventoriesOpen()
        {
            bool inventoriesOpen = false;

            foreach (Inventory inventory in Inventories)
            {
                if (inventory.InventoryOpen)
                {
                    inventoriesOpen = true;
                }
            }

            return inventoriesOpen;
        }
    }
}
