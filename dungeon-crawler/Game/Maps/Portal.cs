using DungeonCrawler;
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
        int maxSpawn = 100;
        int maxHealth = 1;
        int currentHealth = 1;
        private Vector2 position;
        private readonly AnimatedSprite portalSprite;
        Map map;
        private string levelType;
        private IDictionary<string, string[]> enemyTypes = new Dictionary<string, string[]>();

        List<Entity> enemyList;
        ContentManager contentManager;

        public string ID;
        public MapObject MapObject;
        /// <summary>
        /// If destroyed, the portal will stop spawning enemies.
        /// </summary>
        public bool Destroyed = false;
        public bool Enabled = true;
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
            this.map = map;
            this.enemyList = enemyList;
            this.contentManager = contentManager;
            MapObject = mapObject;
            portalSprite = new AnimatedSprite(Sprites.GetSprite("PORTAL"));
            portalSprite.Play("idleSouth1");
            portalSprite.Position = mapObject.GetPosition();
            mapObject.SetSprite(portalSprite);
            portalCollidable = map.GetWorld().Create(portalSprite.Position.X, portalSprite.Position.Y, 32, 32);
            mapObject.SetCollisionBox(portalCollidable);
            portalEntity = new Entity(Sprites.GetSprite("PORTAL"));
            portalEntity.ID = mapObject.GetId().ToString();
            ID = portalEntity.ID;
            position = mapObject.GetPosition();
            portalEntity.LoadContent(contentManager);
            portalEntity.State = DungeonCrawler.Action.IdleSouth1;
            portalEntity.Movable = false;
            portalEntity.MaxHealth = maxHealth;
            portalEntity.CurrentHealth = currentHealth;
            portalEntity.Position = mapObject.GetPosition();
            portalEntity.Name = "PORTAL";
            spawnRate = 60;
            enemyTypes.Add("PLAINS", new string[] { "BAT", "ZOMBIE", "BLUE_SLIME" });
            enemyTypes.Add("FIRELANDS", new string[] { 
                "SKELETON", "FIRE_BAT", 
                "COBRA", "FIRE_GOLEM", 
                "GREEN_DRAKE", "KIMODO",
                "CINDER_DWARF", "CINDER_BEETLE"
            });

            if (map.GetMapName().Contains("PLAINS"))
            {
                levelType = "PLAINS";
            }
            else if (map.GetMapName().Contains("FIRELANDS"))
            {
                levelType = "FIRELANDS";
            }
        }

        public void Respawn()
        {
            Destroyed = false;
            portalEntity.MaxHealth = maxHealth;
            portalEntity.CurrentHealth = currentHealth;
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

        private Entity GetRandomSpawn(String[] enemies)
        {
            Random random = new Random();
            int randomNum = random.Next(0, enemies.Length);
            int spawnChance = random.Next(0, 100);
            Entity randomEnemy = Enemies.GetEnemyByName(enemies[randomNum]);

            while (spawnChance >= randomEnemy.SpawnRate)
            {
                randomNum = random.Next(0, enemies.Length);
                randomEnemy = Enemies.GetEnemyByName(enemies[randomNum]);
                spawnChance = random.Next(0, 100);
            }

            randomEnemy.Position = new Vector2(MapObject.GetPosition().X, MapObject.GetPosition().Y + 20);
            randomEnemy.MaxHealth += (int)((Level.Difficulty * 0.25) * 100);
            randomEnemy.CurrentHealth += (int)((Level.Difficulty * 0.25) * 100);
            randomEnemy.AttackDamage *= (1 + (Level.Difficulty * 0.25));

            return randomEnemy;
        }


        public override void Update(GameTime gameTime)
        {
            frames++;
            if (frames == spawnRate && enemyList.Count() < maxSpawn && !Destroyed && Enabled && !Level.NextLevel)
            {
                String[] enemies = enemyTypes["PLAINS"];
                switch (levelType)
                {
                    case ("PLAINS"):
                        enemies = enemyTypes["PLAINS"];
                        break;
                    case ("FIRELANDS"):
                        enemies = enemyTypes["FIRELANDS"];
                        break;
                }

                Entity randomEnemy = GetRandomSpawn(enemies);
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



