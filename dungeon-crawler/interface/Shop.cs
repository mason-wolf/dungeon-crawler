using DungeonCrawler;
using DungeonCrawler.Interface;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Interface
{
    class Shop : Inventory
    {
        public Shop(ContentManager content) : base(content)
        {
        }

        public override void Update(GameTime gameTime)
        {
            if (Player.SelectedItem != null)
            {
                Console.WriteLine("hello!");
            }
        }
    }
}
