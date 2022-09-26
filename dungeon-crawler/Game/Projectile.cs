using DungeonCrawler;
using MonoGame.Extended.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Game
{
    public class Projectile : Entity
    {
        new public int ID { get; set; }
        public string Direction { get; set; }
        public bool TargetHit { get; set; }
        public RectangleF HitBox { get; set; }
        public int Damage { get; set; }
    }
}
