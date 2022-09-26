using DungeonCrawler;
using MonoGame.Extended.Shapes;
using MonoGame.Extended.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Game
{
    public class Spell : Item
    {
        public AnimatedSprite Sprite { get; set; }
        public double Damage { get; set; }
        public int Heal { get; set; }

        public double ManaCost { get; set; }
        public Projectile Projectile { get; set; }
        public RectangleF BoundingBox { get; set; }
        public int Duration { get; set; }
        public SpellDirection Direction { get; set; }
        public enum SpellDirection
        {
            NORTH,
            EAST,
            SOUTH,
            WEST
        }
    }
}
