﻿using DungeonCrawler;
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
            Item healthPotion = new Item();
            healthPotion.ItemTexture = Sprites.GetTexture("HEALTH_POTION_ICON");
            healthPotion.Name = "HEALTH POTION";
            healthPotion.Description = "Restores some health.";
            healthPotion.ID = 3;
            healthPotion.Price = 100;
            ItemList.Add(healthPotion);

            Item manaPotion = new Item();
            manaPotion.ItemTexture = Sprites.GetTexture("MANA_POTION_ICON");
            manaPotion.Name = "MANA POTION";
            manaPotion.Description = "Restores some mana.";
            manaPotion.ID = 4;
            manaPotion.Price = 100;
            ItemList.Add(manaPotion);

            Item fireball1 = new Item();
            fireball1.ItemTexture = Sprites.GetTexture("FIREBALL_1_ICON");
            fireball1.Name = "FIREBALL";
            fireball1.ID = 1;
            fireball1.Description = "Shoots a flame.";
            ItemList.Add(fireball1);

            Item icebolt1 = new Item();
            icebolt1.ItemTexture = Sprites.GetTexture("ICEBOLT_1_ICON");
            icebolt1.Name = "ICEBOLT";
            icebolt1.ID = 2;
            icebolt1.Description = "Casts a bolt of ice.";
            ItemList.Add(icebolt1);
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
