using MonoGame.Extended.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Game
{
    public class Spell
    {
        public int ID { get; set; }
        public AnimatedSprite Sprite { get; set; }
        
        public Projectile Projectile { get; set; }

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
