using DungeonCrawler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Game
{
    public class Loot
    {
        public int Gold { get; set; } = 0;
        public Armor Armor { get; set; }
    }

    public class LootGenerator
    {
        public Loot GenerateLoot()
        {
            int goldOrArmor = 0;
            Random random = new Random();
            goldOrArmor = random.Next(1, 3);

            Loot loot = new Loot();

            switch(goldOrArmor)
            {
                case (1):
                    loot.Gold = random.Next(1, 50);
                    break;
                case (2):
                    loot.Armor = GenerateArmor();
                    break;
            }

            return loot;
        }

        private Armor GenerateArmor()
        {
            Armor armor = new Armor();
            Random random = new Random();
            armor.ID = random.Next(500, 99999);
            int randomArmor = random.Next(1, 6);
            armor.Price = random.Next(1, 100);

            switch(randomArmor)
            {
                // Head
                case (1):
                    armor.Type = Armor.ArmorType.HEAD;
                    GenerateProperty(armor);
                    armor.Name = "HAT OF " + GetPrimaryStat(armor);
                    armor.ItemTexture = Sprites.GetTexture("HAT_1_ICON");
                    break;
                // Chest
                case (2):
                    armor.Type = Armor.ArmorType.CHEST;
                    GenerateProperty(armor);
                    armor.Name = "ROBES OF " + GetPrimaryStat(armor);
                    armor.ItemTexture = Sprites.GetTexture("ROBE_1_ICON");
                    break;
                // Hands
                case (3):
                    armor.Type = Armor.ArmorType.HANDS;
                    GenerateProperty(armor);
                    armor.Name = "GLOVES OF " + GetPrimaryStat(armor);
                    armor.ItemTexture = Sprites.GetTexture("GLOVES_1_ICON");
                    break;
                // Boots
                case (4):
                    armor.Type = Armor.ArmorType.BOOTS;
                    GenerateProperty(armor);
                    armor.Name = "BOOTS OF " + GetPrimaryStat(armor);
                    armor.ItemTexture = Sprites.GetTexture("BOOTS_1_ICON");
                    break;
                // Ring
                case (5):
                    armor.Type = Armor.ArmorType.RING;
                    GenerateProperty(armor);
                    armor.Name = "RING OF " + GetPrimaryStat(armor);
                    armor.ItemTexture = Sprites.GetTexture("RING_1_ICON");
                    break;
            }

            return armor;
        }

        /// <summary>
        /// Generates a random property to assign to an armor piece.
        /// </summary>
        /// <param name="armor">Armor to apply properties</param>
        private void GenerateProperty(Armor armor)
        {
            Random random = new Random();
            int randomProperty = random.Next(1, 6);
            int propertyAmount = random.Next(1, 26);
            switch (randomProperty)
            {
                // Fire
                case (1):
                    armor.FireResistance = propertyAmount;
                    armor.Description = "+" + armor.FireResistance + " FIRE RESISTANCE";
                    break;
                // Frost
                case (2):
                    armor.FrostResistance = propertyAmount;
                    armor.Description = "+" + armor.FrostResistance + " FROST RESISTANCE";
                    break;
                // Thunder
                case (3):
                    armor.ThunderResistance = propertyAmount;
                    armor.Description = "+" + armor.ThunderResistance + " THUNDER RESISTANCE";
                    break;
                // Health
                case (4):
                    armor.HealthBonus = propertyAmount;
                    armor.Description = "+" + armor.HealthBonus + " HEALTH";
                    break;
                // Mana
                case (5):
                    armor.ManaBonus = propertyAmount;
                    armor.Description = "+" + armor.ManaBonus + " MANA";
                    break;
            }
        }

        private string GetPrimaryStat(Armor armor)
        {
            List<ArmorStatus> stats = armor.GetStats();

            ArmorStatus primaryStat = new ArmorStatus();

            foreach(ArmorStatus stat in stats)
            {
                if (stat.Amount > 0)
                {
                    primaryStat = stat;
                }
            }

            return primaryStat.Name.Split('_').First();
        }
    }
}
