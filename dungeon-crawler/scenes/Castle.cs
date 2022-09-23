
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
            int[] npcIDs = { 15, 16, 17 };

            foreach(int npcId in npcIDs)
            {
                Entity npc = Level.GetNpcByID(npcId.ToString());
                NPCList.Add(npc);
            }

            foreach(Entity npc in NPCList)
            {
                Console.WriteLine(npc.ID + " " + npc.Name + " " + npc.Position);
            }
        }

        public override void Update(GameTime gameTime)
        {
            foreach (Entity npc in NPCList)
            {
                if (Init.Player.BoundingBox.Intersects(npc.BoundingBox))
                {
                    string message = "";
                    switch (npc.ID)
                    {
                        case ("15"):
                            message = 
                                "\nI am the warden of flame." +
                                "\nYou'd be wise to heed my teachings.";
                            break;
                        case ("16"):
                            message =
                                "\nSome foes are weaker to fire than others." +
                                "\nConserve your flame powers when facing them.";
                            break;
                        case ("17"):
                            message =
                                "\nYou can purchase new spells from the bookkeeper." +
                                 "\nHe's in the library.";
                            break;
                    }

                    Init.DialogBox.Text = message;
                    Init.DialogBox.StartDialog = true;
                    Init.HandleDialog();
                }
 
            }

        }
    }
}
