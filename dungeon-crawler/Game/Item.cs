using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonCrawler
{
    public class Item
    {
        public int ID { get; set; }
        public Texture2D ItemTexture { get; set; }
        public Rectangle ItemRectangle { get; set; }
        public int Column { get; set; }
        public int Row { get; set; }
        public int Index { get; set; }
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public int Quantity { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Price { get; set; }
        public bool Useable { get; set; } = true;
        public int ActionBarSlot { get; set; }
    }
}
