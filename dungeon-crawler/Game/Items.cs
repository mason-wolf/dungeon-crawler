using DungeonCrawler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Game
{
    class Items
    {
        public static List<Item> ItemList = new List<Item>();

        public Items()
        {
            Item item = new Item();
            item.ItemTexture = Sprites.GetTexture("HEALTH_POTION_ICON");
            item.Name = "HEALTH POTION";
            item.Description = "Restores some health.";
            item.ID = 3;
            item.Price = 5;
            ItemList.Add(item);

            item = new Item();
            item.ItemTexture = Sprites.GetTexture("FIREBALL_1_ICON");
            item.Name = "FIREBALL";
            item.ID = 1;
            item.Description = "Shoots a flame.";
            ItemList.Add(item);

            item = new Item();
            item.ItemTexture = Sprites.GetTexture("ICEBOLT_1_ICON");
            item.Name = "ICEBOLT";
            item.ID = 2;
            item.Description = "Casts a bolt of ice.";
            ItemList.Add(item);
        }

        public static Item GetItemById(int id)
        {
            Item foundItem = null;

            foreach(Item item in ItemList)
            {
                if (item.ID == id)
                {
                    foundItem = item;
                }
            }

            return foundItem;
        }
    }
}
