using DungeonCrawler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Game
{
    public class ArmorStatus
    {
        public string Name { get; set; }
        public double Amount { get; set; }
    }

    public class Armor : Item

    {
        public int Value { get; set; }
        public double FireResistance { get; set; } = 0;
        public double FrostResistance { get; set; } = 0;
        public double ThunderResistance { get; set; } = 0;
        public int HealthBonus { get; set; } = 0;
        public int ManaBonus { get; set; } = 0;

        public bool Equipped { get; set; } = false;
        public ArmorType Type { get; set; }

        public enum ArmorType
        {
            HEAD,
            CHEST,
            HANDS,
            BOOTS,
            RING
        }

        public List<ArmorStatus> GetStats()
        {
            List<ArmorStatus> stats = new List<ArmorStatus>();
            ArmorStatus stat = new ArmorStatus();
            stat.Name = "FIRE_RESISTANCE";
            stat.Amount = FireResistance;
            stats.Add(stat);

            stat = new ArmorStatus();
            stat.Name = "FROST_RESISTANCE";
            stat.Amount = FrostResistance;
            stats.Add(stat);

            stat = new ArmorStatus();
            stat.Name = "THUNDER_RESISTANCE";
            stat.Amount = ThunderResistance;
            stats.Add(stat);

            stat = new ArmorStatus();
            stat.Name = "HEALTH_BONUS";
            stat.Amount = HealthBonus;
            stats.Add(stat);

            stat = new ArmorStatus();
            stat.Name = "MANA_BONUS";
            stat.Amount = ManaBonus;
            stats.Add(stat);

            return stats;
        }
    }
}
