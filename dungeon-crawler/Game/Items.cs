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
            Item healthPotion = new Item();
            healthPotion.ItemTexture = Sprites.GetTexture("HEALTH_POTION_ICON");
            healthPotion.Name = "HEALTH POTION";
            healthPotion.Description = "Restores some health.";
            healthPotion.ID = 3;
            healthPotion.Price = 50;
            ItemList.Add(healthPotion);

            Item manaPotion = new Item();
            manaPotion.ItemTexture = Sprites.GetTexture("MANA_POTION_ICON");
            manaPotion.Name = "MANA POTION";
            manaPotion.Description = "Restores some mana.";
            manaPotion.ID = 4;
            manaPotion.Price = 50;
            ItemList.Add(manaPotion);

            Spell fireball1 = new Spell();
            fireball1.ItemTexture = Sprites.GetTexture("FIREBALL_1_ICON");
            fireball1.Name = "FIREBALL";
            fireball1.ID = 1;
            fireball1.Damage = 5;
            fireball1.ManaCost = .5;
            fireball1.Description = "Shoots a flame.";
            fireball1.Element = Spell.SpellElement.FIRE;
            ItemList.Add(fireball1);

            Item fireballBook = new Item();
            fireballBook.ItemTexture = Sprites.GetTexture("BOOK");
            fireballBook.Name = "FIREBALL";
            fireballBook.ID = 7;
            fireballBook.Price = 100;
            fireballBook.Description = "";
            ItemList.Add(fireballBook);

            Item iceboltBook = new Item();
            iceboltBook.ItemTexture = Sprites.GetTexture("BOOK");
            iceboltBook.Name = "ICEBOLT";
            iceboltBook.ID = 8;
            iceboltBook.Price = 100;
            iceboltBook.Description = "";
            ItemList.Add(iceboltBook);

            Item thunderboltBook = new Item();
            thunderboltBook.ItemTexture = Sprites.GetTexture("BOOK");
            thunderboltBook.Name = "THUNDERBOLT";
            thunderboltBook.ID = 9;
            thunderboltBook.Price = 200;
            thunderboltBook.Description = "";
            ItemList.Add(thunderboltBook);

            Item healBook = new Item();
            healBook.ItemTexture = Sprites.GetTexture("BOOK");
            healBook.Name = "HEAL";
            healBook.ID = 10;
            healBook.Price = 200;
            healBook.Description = "";
            ItemList.Add(healBook);

            Item flameShieldBook = new Item();
            flameShieldBook.ItemTexture = Sprites.GetTexture("BOOK");
            flameShieldBook.Name = "FLAME SHIELD";
            flameShieldBook.ID = 13;
            flameShieldBook.Price = 400;
            flameShieldBook.Description = "";
            ItemList.Add(flameShieldBook);

            Spell icebolt1 = new Spell();
            icebolt1.ItemTexture = Sprites.GetTexture("ICEBOLT_1_ICON");
            icebolt1.Name = "ICEBOLT";
            icebolt1.ID = 2;
            icebolt1.Damage = 5;
            icebolt1.ManaCost = .5;
            icebolt1.Element = Spell.SpellElement.FROST;
            icebolt1.Description = "Casts a bolt of ice.";
            ItemList.Add(icebolt1);

            Spell thunderBolt1 = new Spell();
            thunderBolt1.ItemTexture = Sprites.GetTexture("THUNDERBOLT_1_ICON");
            thunderBolt1.Name = "THUNDERBOLT";
            thunderBolt1.ID = 5;
            thunderBolt1.Damage = 10;
            thunderBolt1.ManaCost = 1;
            icebolt1.Element = Spell.SpellElement.THUNDER;
            thunderBolt1.Description = "Shoots a thunder bolt.";
            ItemList.Add(thunderBolt1);

            Spell heal1 = new Spell();
            heal1.ItemTexture = Sprites.GetTexture("HEAL_1_ICON");
            heal1.Name = "HEAL";
            heal1.ID = 6;
            heal1.ManaCost = 5;
            heal1.Element = Spell.SpellElement.HOLY;
            heal1.Description = "Restores some health.";
            ItemList.Add(heal1);

            Item homingCrystal = new Item();
            homingCrystal.ItemTexture = Sprites.GetTexture("HOMING_CRYSTAL");
            homingCrystal.Name = "HOMING CRYSTAL";
            homingCrystal.ID = 11;
            homingCrystal.Description = "Sends the user home when rubbed. It's shiny.";
            homingCrystal.Price = 50;
            ItemList.Add(homingCrystal);

            Spell flameShield = new Spell();
            flameShield.ItemTexture = Sprites.GetTexture("FLAME_SHIELD_ICON");
            flameShield.Name = "FLAME SHIELD";
            flameShield.ID = 12;
            flameShield.Description = "Casts a shield of flame for 10 seconds.";
            flameShield.ManaCost = 15;
            flameShield.Damage = 2;
            flameShield.Element = Spell.SpellElement.FIRE;
            flameShield.Duration = 10000; 
            ItemList.Add(flameShield);

            Item fireShard = new Item();
            fireShard.ItemTexture = Sprites.GetTexture("FIRE_SHARD");
            fireShard.Name = "FIRE SHARD";
            fireShard.ID = 14;
            fireShard.Description = "Contains the power of flame.";
            fireShard.Price = 100;
            fireShard.Useable = false;
            ItemList.Add(fireShard);

            Item frostShard = new Item();
            frostShard.ItemTexture = Sprites.GetTexture("FROST_SHARD");
            frostShard.Name = "FROST SHARD";
            frostShard.ID = 15;
            frostShard.Description = "Contains the power of frost.";
            frostShard.Price = 100;
            frostShard.Useable = false;
            ItemList.Add(frostShard);

            Item thunderShard = new Item();
            thunderShard.ItemTexture = Sprites.GetTexture("THUNDER_SHARD");
            thunderShard.Name = "THUNDER SHARD";
            thunderShard.ID = 16;
            thunderShard.Description = "Contains the power of thunder.";
            thunderShard.Price = 100;
            thunderShard.Useable = false;
            ItemList.Add(thunderShard);
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
