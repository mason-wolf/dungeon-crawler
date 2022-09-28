using DungeonCrawler;
using DungeonCrawler.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Game
{
    public class Equipment
    {
        private List<Armor> armorList = new List<Armor>();
        public Armor Head { get; set; }
        public Armor Chest { get; set; }
        public Armor Hands { get; set; }
        public Armor Boots { get; set; }
        public Armor Ring { get; set; }

        public Equipment()
        {
            Head = new Armor();
            Chest = new Armor();
            Hands = new Armor();
            Boots = new Armor();
            Ring = new Armor();
        }

        public void Unequip(Armor armor)
        {
            switch (armor.Type)
            {
                case (Armor.ArmorType.BOOTS):
                    Boots = new Armor();
                    break;
                case (Armor.ArmorType.HEAD):
                    Head = new Armor();
                    break;
                case (Armor.ArmorType.HANDS):
                    Hands = new Armor();
                    break;
                case (Armor.ArmorType.RING):
                    Ring = new Armor();
                    break;
                case (Armor.ArmorType.CHEST):
                    Chest = new Armor();
                    break;
            }
            armor.Equipped = false;
        }

        public Dictionary<string, double> GetBonuses()
        {
            Dictionary<string, double> bonuses = new Dictionary<string, double>();

            armorList.Clear();
            armorList.Add(Head);
            armorList.Add(Chest);
            armorList.Add(Hands);
            armorList.Add(Boots);
            armorList.Add(Ring);

            double frostResistance = 0;
            double fireResistance = 0;
            double thunderResistance = 0;
            double healthBonus = 0;
            double manaBonus = 0;

            foreach (Armor armor in armorList)
            {
                frostResistance += armor.FrostResistance;
                fireResistance += armor.FireResistance;
                thunderResistance += armor.ThunderResistance;
                healthBonus += armor.HealthBonus;
                manaBonus += armor.ManaBonus;
            }

            bonuses.Add("FROST_RESISTANCE", frostResistance);
            bonuses.Add("FIRE_RESISTANCE", fireResistance);
            bonuses.Add("THUNDER_RESISTANCE", thunderResistance);
            bonuses.Add("HEALTH_BONUS", healthBonus);
            bonuses.Add("MANA_BONUS", manaBonus);

            return bonuses;
        }
    }
}
