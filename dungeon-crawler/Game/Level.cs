﻿using DungeonCrawler.Engine;

using DungeonCrawler.Scenes;
using Humper;
using Humper.Responses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using MonoGame.Extended.Shapes;
using MonoGame.Extended.Sprites;
using RoyT.AStar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonCrawler
{
    public class Level : Scene
    {
        EnemyAI enemyAI;
        private List<Entity> enemyList = new List<Entity>();
        public static List<Entity> NPCList = new List<Entity>();
        RoyT.AStar.Grid collisionGrid;
        Map map;
        string levelName;
        List<SoundEffect> soundEffects;
        bool objectsPopulated = false;
        SceneLogic scene;
        public List<MapObject> MapObjects { get; set; }
        private Vector2 startingPosition { get; set; }
        public Vector2 GetStartingPosition()
        {
            return startingPosition;
        }
        private Boolean startPositionSet = false;
        /// <summary>
        /// Sets the tiled map for this level.
        /// </summary>
        /// <param name="map">Map(filepath)</param>
        /// <returns></returns>
        public Level SetMap(Map map)
        {
            this.map = map;
            MapObjects = map.GetMapObjects();
            return this;
        }

        /// <summary>
        /// Sets the scene logic for this level. 
        /// </summary>
        /// <param name="levelScene"></param>
        /// <returns></returns>
        public void SetScene(SceneLogic levelScene)
        {
            map.SetScene(levelScene);
            map.LoadScene();
            scene = levelScene;
        }

        public Level SetLevelName(string levelName)
        {
            this.levelName = levelName;
            return this;
        }

        public string GetLevelName()
        {
            return levelName;
        }

        public void SetPlayerStartPosition(Entity player)
        {
            if (!startPositionSet)
            {
                player.Position = startingPosition;
                startPositionSet = true;
            }
        }
        /// <summary>
        /// Returns the tiled map for this level.
        /// </summary>
        /// <returns></returns>
        public Map GetMap()
        {
            return map;
        }

        /// <summary>
        /// Returns the EnemyAI object for this level. 
        /// EnemyAI stores the collision grid, list of enemies for this level and player object.
        /// </summary>
        /// <returns></returns>
        public EnemyAI GetEnemyAI()
        {
            return enemyAI;
        }

        /// <summary>
        /// Returns the list of enemies that exist on this level.
        /// </summary>
        /// <returns></returns>
        public List<Entity> GetEnemyList()
        {
            return enemyList;
        }
        public override void LoadContent(ContentManager content)
        {

            foreach (MapObject mapObject in MapObjects)
            {
                switch (mapObject.GetName())
                {
                    case ("Skeleton"):
                        Entity skeletonEntity = new Entity(Sprites.GetSprite("SELETON"));
                        skeletonEntity.LoadContent(content);
                        skeletonEntity.State = Action.IdleEast1;
                        skeletonEntity.MaxHealth = 15;
                        skeletonEntity.CurrentHealth = 15;
                        skeletonEntity.AttackDamage = 0.05;
                        skeletonEntity.Position = mapObject.GetPosition();
                        skeletonEntity.Name = "Skeleton";
                        enemyList.Add(skeletonEntity);
                        break;
                    case ("FIRE_MAGE"):
                        Entity fireProfessorEntity = new Entity(Sprites.GetSprite("FIRE_MAGE"));
                        fireProfessorEntity.LoadContent(content);
                        fireProfessorEntity.State = Action.IdleSouth1;
                        fireProfessorEntity.Position = mapObject.GetPosition();
                        NPCList.Add(fireProfessorEntity);
                        break;
                    case ("GREEN_PORTAL"):
                        AnimatedSprite greenPortalSprite = new AnimatedSprite(Sprites.GetSprite("GREEN_PORTAL"));
                        greenPortalSprite.Play("idle");
                        greenPortalSprite.Position = mapObject.GetPosition();
                        mapObject.SetSprite(greenPortalSprite);
                        break;
                    case ("TORCH"):
                        AnimatedSprite torchSprite = new AnimatedSprite(Sprites.GetSprite("TORCH"));
                        torchSprite.Play("BURNING");
                        torchSprite.Position = mapObject.GetPosition();
                        mapObject.SetSprite(torchSprite);
                        break;
                    case ("DESK"):
                        AnimatedSprite deskSprite = new AnimatedSprite(Sprites.GetSprite("DESK"));
                        deskSprite.Play("idle");
                        deskSprite.Position = mapObject.GetPosition();
                        mapObject.SetSprite(deskSprite);
                        IBox deskCollidable = map.GetWorld().Create(deskSprite.Position.X, deskSprite.Position.Y, 16, 16);
                        mapObject.SetCollisionBox(deskCollidable);
                        break;
                    case ("BOOKSHELF"):
                        AnimatedSprite bookshelfSprite = new AnimatedSprite(Sprites.GetSprite("BOOKSHELF"));
                        bookshelfSprite.Play("idle");
                        bookshelfSprite.Position = mapObject.GetPosition();
                        mapObject.SetSprite(bookshelfSprite);
                        IBox bookshelfCollidable = map.GetWorld().Create(bookshelfSprite.Position.X, bookshelfSprite.Position.Y, 16, 16);
                        mapObject.SetCollisionBox(bookshelfCollidable);
                        break;
                    case ("START"):
                        // Map object with the name "start", specifies the starting position.
                        startingPosition = mapObject.GetPosition();
                        break;
                }

                scene.Map = map;
                scene.MapObjects = MapObjects;
                scene.ContentManager = content;
                collisionGrid = map.GenerateAStarGrid();
                enemyAI = new EnemyAI(collisionGrid, enemyList, Init.Player);
                soundEffects = new List<SoundEffect>();
                soundEffects.Add(content.Load<SoundEffect>(@"sounds\destroyed-barrel"));
                soundEffects.Add(content.Load<SoundEffect>(@"sounds\dead-bat"));
                soundEffects.Add(content.Load<SoundEffect>(@"sounds\dead-skeleton"));
               // arrowsSprite = content.Load<Texture2D>(@"objects\arrows");
            }
        }

        public override void Update(GameTime gameTime)
        {

            if (scene != null)
            {
                scene.MapObjects = MapObjects;
                scene.Update(gameTime);
            }

            if (enemyAI != null)
            {
                enemyAI.Update(gameTime);
            }

            foreach (Entity enemy in enemyList)
            {
                enemy.Update(gameTime);

                // If enemy dies
                if (enemy.CurrentHealth <= 0 && enemy.Dead == false)
                {
                    enemy.State = Action.Dead;
                    enemy.Dead = true;

                    switch (enemy.Name)
                    {
                        case ("Bat"):
                            soundEffects[1].Play();
                            break;
                        case ("Skeleton"):
                            soundEffects[2].Play();
                            break;
                    }
                }
            }

            foreach (MapObject mapObject in MapObjects)
            {
                mapObject.Update(gameTime);
            }

            // Handle the player destroying objects.
            foreach (MapObject mapObject in MapObjects)
            {
                //RectangleF offset = new Rectangle((int)Init.Player.Position.X + 1, (int)Init.Player.Position.Y, 32, 24);
                //if (Player.PlayerWeapon.BoundingBox.Intersects(mapObject.GetBoundingBox()) && Player.IsAttacking && mapObject.GetName() == "BARREL")
                //{
                //    if (!mapObject.IsDestroyed())
                //    {
                //        mapObject.GetSprite().Play("broken");
                //        mapObject.Destroy();
                //        soundEffects[0].Play();
                //        map.GetWorld().Remove(mapObject.GetCollisionBox());
                //    }
                //}
            }

            if (map != null)
            {
                map.Update(gameTime);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (Entity enemy in enemyList)
            {
                enemy.Draw(spriteBatch);
                Vector2 AIHealthPosition = new Vector2(enemy.Position.X - 8, enemy.Position.Y - 20);
                enemy.DrawHUD(spriteBatch, AIHealthPosition, false);
            }
            
            foreach (Entity npc in NPCList)
            {
                npc.Draw(spriteBatch);
            }

            // Randomly place loot in some objects.
            Random random = new Random();

            foreach (MapObject mapObject in MapObjects)
            {
                Item item = new Item();

                if (objectsPopulated == false)
                {
                    int lootChance = random.Next(1, 4);

                    switch (lootChance)
                    {
                        case (1):
                         //   item.ItemTexture = Sprites.chickenTexture;
                            item.Name = "Chicken";
                            item.Width = 16;
                            item.Height = 16;
                            break;
                        case (2):
                          //  item.ItemTexture = arrowsSprite;
                            item.Name = "Arrow";
                            item.Width = 13;
                            item.Height = 19;
                            break;
                    }

                    if (item != null)
                    {
                        mapObject.SetContainedItem(item);
                    }
                }
                mapObject.Draw(spriteBatch);
            }

            objectsPopulated = true;
        }
    }
}
