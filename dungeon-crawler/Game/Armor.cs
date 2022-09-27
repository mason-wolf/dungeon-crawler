using DungeonCrawler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Game
{
    public class Armor : Item
    {
        public int Value { get; set; }
        public double FireResistance { get; set; } = 0;
        public double FrostResistance { get; set; } = 0;
        public double ThunderResistance { get; set; } = 0;
        public int HealthBonus { get; set; } = 0;
        public int ManaBonus { get; set; } = 0;

        public enum ArmorType
        {
            HEAD,
            CHEST,
            HANDS,
            BOOTS,
            RING
        }
    }
}
