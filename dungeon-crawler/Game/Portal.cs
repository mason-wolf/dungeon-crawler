﻿using DungeonCrawler;
using Humper;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Game
{
    public class Portal : Scene
    {
        IBox portalCollidable;
        Entity portalEntity;
        int frames = 0;
        int spawnRate;
        private string levelType;
        private IDictionary<string, string[]> enemyTypes = new Dictionary<string, string[]>();

        List<Entity> enemyList;
        ContentManager contentManager;

        public string ID;
        public MapObject MapObject;
        public bool Destroyed = false;
        /// <summary>
        /// Creates a portal on a map.
        /// </summary>
        /// <param name="map">Map to add portal to.</param>
        /// <param name="mapObject">Map object associated with portal.</param>
        /// <param name="contentManager">Content manager instance.</param>
        public Portal(
            Map map, 
            MapObject mapObject, 
            ContentManager contentManager,
            List<Entity> enemyList)
        {
            this.enemyList = enemyList;
            this.contentManager = contentManager;
            MapObject = mapObject;
            AnimatedSprite portalSprite = new AnimatedSprite(Sprites.GetSprite("PORTAL"));
            portalSprite.Play("idleSouth1");
            portalSprite.Position = mapObject.GetPosition();
            mapObject.SetSprite(portalSprite);
            portalCollidable = map.GetWorld().Create(portalSprite.Position.X, portalSprite.Position.Y, 32, 32);
            mapObject.SetCollisionBox(portalCollidable);
            portalEntity = new Entity(Sprites.GetSprite("PORTAL"));
            portalEntity.ID = mapObject.GetId().ToString();
            ID = portalEntity.ID;
            portalEntity.LoadContent(contentManager);
            portalEntity.State = DungeonCrawler.Action.IdleSouth1;
            portalEntity.Movable = false;
            portalEntity.MaxHealth = 250;
            portalEntity.CurrentHealth = 250;
            portalEntity.Position = mapObject.GetPosition();
            portalEntity.Name = "PORTAL";
            spawnRate = 100;
            enemyTypes.Add("PLAINS", new string[] { "BAT", "ZOMBIE" });

            if (map.GetMapName().Contains("PLAINS"))
            {
                levelType = "PLAINS";
            }
        }

        /// <summary>
        /// Returns entity associated with this portal.
        /// </summary>
        /// <returns>Entity</returns>
        public Entity GetEntity()
        {
            return portalEntity;
        }

        public IBox GetCollisionBoundaries()
        {
            return this.portalCollidable;
        }

        public override void LoadContent(ContentManager content)
        {
            throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {
            frames++;
            if (frames == spawnRate && enemyList.Count() < 100 && !Destroyed)
            {
                String[] enemies = enemyTypes["PLAINS"];
                Random random = new Random();
                int randomNum = random.Next(0, enemies.Length);
                Entity randomEnemy = Enemies.GetEnemyByName(enemies[randomNum]);
                randomEnemy.Position = new Vector2(MapObject.GetPosition().X, MapObject.GetPosition().Y + 20);
                enemyList.Add(randomEnemy);
            }

            if (frames == spawnRate)
            {
                frames = 0;
            }
            // If the enemy dies, remove from the list so the portal
            // can keep spawning.
            Entity enemyToRemove = null;
            foreach(Entity enemy in enemyList)
            {
                if (enemy.Dead)
                {
                    enemyToRemove = enemy;
                }
            }

            if (enemyToRemove != null)
            {
                enemyList.Remove(enemyToRemove);
                enemyToRemove = null;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            throw new NotImplementedException();
        }
    }
}



