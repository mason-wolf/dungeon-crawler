
using DungeonCrawler.Engine;
using DungeonCrawler;
using Humper;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoGame.Extended.Sprites;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;
using Microsoft.Xna.Framework.Input;
using DungeonCrawler.Interface;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace DungeonCrawler.Scenes
{
    public class Castle : SceneLogic
    {
        Song levelThemeSong;
        List<Entity> NPCList = new List<Entity>();
        public Castle()
        {

        }
        public override List<MapObject> MapObjects { get; set; }
        public override ContentManager ContentManager { get; set;  }
        public override Map Map { get; set; }

        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (Entity npc in NPCList)
            {
                npc.Draw(spriteBatch);
            }
        }

        public override void LoadContent(ContentManager content)
        {
            levelThemeSong = content.Load<Song>(@"music\level_1");
            //     MediaPlayer.IsRepeating = true;
            //    MediaPlayer.Play(levelThemeSong);
        }

        public override void LoadScene()
        {
            Entity fireMage = Level.GetNpcByName("FIRE_MAGE");
            NPCList.Add(fireMage);
        }

        public override void Update(GameTime gameTime)
        {
            foreach (Entity npc in NPCList)
            {
                if (Init.Player.BoundingBox.Intersects(npc.BoundingBox))
                {
                    Init.dialogBox.Text =
                       "\nI am the fire professor." +
                       "\nWelcome to the academy.";
                    Init.startDialog = true;
                    Init.HandleDialog();
                }
                else
                {
                    Init.startDialog = false;
                }
            }

        }
    }
}
