using DungeonCrawler.Engine;

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
using System.Diagnostics;
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
        SceneLogic scene;
        public List<MapObject> MapObjects { get; set; }
        private Vector2 startingPosition { get; set; }

        private bool LeveledUp { get; set; } = false;
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

        public SceneLogic GetScene()
        {
            return scene;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="content">ContentManager instance</param>
        /// <param name="mapObject">Map object to assign sprite, position and ID to.</param>
        /// <param name="spriteName">Name of sprite</param>
        public void AddNpc(ContentManager content, MapObject mapObject, string spriteName)
        {
            Entity entity = new Entity(Sprites.GetSprite(spriteName));
            entity.LoadContent(content);
            entity.State = Action.IdleSouth1;
            entity.Position = mapObject.GetPosition();
            entity.Name = spriteName;
            IBox entityCollidable = map.GetWorld().Create(entity.Position.X, entity.Position.Y, 16, 16);
            mapObject.SetCollisionBox(entityCollidable);
            if (mapObject.GetCustomProperties().Count > 0)
            {
                entity.ID = mapObject.GetCustomProperties()[0].ToString();
            }
            NPCList.Add(entity);
        }

        public static Entity GetNpcByID(string npcId)
        {
            Entity npcByName = null;
            foreach (Entity npc in NPCList)
            {
                if (npc.ID == npcId)
                {
                    npcByName = npc;
                }
            }

            return npcByName;
        }

        public static Entity GetNpcByName(string npcName)
        {
            Entity npcByName = null;
            foreach (Entity npc in NPCList)
            {
                if (npc.Name == npcName)
                {
                    npcByName = npc;
                }
            }

            return npcByName;
        }

        public override void LoadContent(ContentManager content)
        {

            foreach (MapObject mapObject in MapObjects)
            {
                switch (mapObject.GetName())
                {
                    case ("SKELETON"):
                        Entity skeletonEntity = new Entity(Sprites.GetSprite("SKELETON"));
                        skeletonEntity.LoadContent(content);
                        skeletonEntity.State = Action.IdleEast1;
                        skeletonEntity.MaxHealth = 30;
                        skeletonEntity.CurrentHealth = 30;
                        skeletonEntity.AttackDamage = 0.05;
                        skeletonEntity.Position = mapObject.GetPosition();
                        skeletonEntity.Name = "SKELETON";
                        skeletonEntity.XP = 10;
                        skeletonEntity.SpellCaster = true;
                        skeletonEntity.CurrentMana = 1000;
                        enemyList.Add(skeletonEntity);
                        break;
                    case ("BAT"):
                        Entity batEntity = new Entity(Sprites.GetSprite("BAT"));
                        batEntity.LoadContent(content);
                        batEntity.State = Action.IdleEast1;
                        batEntity.MaxHealth = 30;
                        batEntity.CurrentHealth = 30;
                        batEntity.AttackDamage = 0.03;
                        batEntity.Position = mapObject.GetPosition();
                        batEntity.Name = "BAT";
                        batEntity.XP = 10;
                        enemyList.Add(batEntity);
                        break;
                    case ("ZOMBIE"):
                        Entity zombieEntity = new Entity(Sprites.GetSprite("ZOMBIE"));
                        zombieEntity.LoadContent(content);
                        zombieEntity.State = Action.IdleEast1;
                        zombieEntity.MaxHealth = 30;
                        zombieEntity.CurrentHealth = 30;
                        zombieEntity.AttackDamage = 0.03;
                        zombieEntity.Position = mapObject.GetPosition();
                        zombieEntity.Name = "ZOMBIE";
                        zombieEntity.XP = 10;
                        enemyList.Add(zombieEntity);
                        break;
                    case ("BLUE_SLIME"):
                        Entity blueSlimeEntity = new Entity(Sprites.GetSprite("BLUE_SLIME"));
                        blueSlimeEntity.LoadContent(content);
                        blueSlimeEntity.State = Action.IdleEast1;
                        blueSlimeEntity.MaxHealth = 25;
                        blueSlimeEntity.CurrentHealth = 25;
                        blueSlimeEntity.AttackDamage = 0.06;
                        blueSlimeEntity.Position = mapObject.GetPosition();
                        blueSlimeEntity.Name = "BLUE_SLIME";
                        blueSlimeEntity.XP = 10;
                        enemyList.Add(blueSlimeEntity);
                        break;
                    case ("FIRE_MAGE"):
                        AddNpc(content, mapObject, "FIRE_MAGE");
                        break;
                    case ("FROST_MAGE"):
                        AddNpc(content, mapObject, "FROST_MAGE");
                        break;
                    case ("GREEN_MAGE"):
                        AddNpc(content, mapObject, "GREEN_MAGE");
                        break;
                    case ("GUARD"):
                        AddNpc(content, mapObject, "GUARD");
                        break;
                    case ("ITEM_MERCHANT"):
                        AddNpc(content, mapObject, "ITEM_MERCHANT");
                        break;
                    case ("NOVICE_MAGE"):
                        AddNpc(content, mapObject, "NOVICE_MAGE");
                        break;
                    case ("THUNDER_MAGE"):
                        AddNpc(content, mapObject, "THUNDER_MAGE");
                        break;
                    case ("WHITE_MAGE"):
                        AddNpc(content, mapObject, "WHITE_MAGE");
                        break;
                    case ("GREEN_PORTAL"):
                        AnimatedSprite greenPortalSprite = new AnimatedSprite(Sprites.GetSprite("GREEN_PORTAL"));
                        greenPortalSprite.Play("idle");
                        greenPortalSprite.Position = mapObject.GetPosition();
                        mapObject.SetSprite(greenPortalSprite);
                        break;
                    case ("RED_PORTAL"):
                        AnimatedSprite redPortalSprite = new AnimatedSprite(Sprites.GetSprite("RED_PORTAL"));
                        redPortalSprite.Play("idle");
                        redPortalSprite.Position = mapObject.GetPosition();
                        mapObject.SetSprite(redPortalSprite);
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
                        IBox deskCollidable = map.GetWorld().Create(deskSprite.Position.X, deskSprite.Position.Y - 5, 12, 10);
                        mapObject.SetCollisionBox(deskCollidable);
                        break;
                    case ("BED"):
                        AnimatedSprite bedSprite = new AnimatedSprite(Sprites.GetSprite("BED"));
                        bedSprite.Play("idle");
                        bedSprite.Position = mapObject.GetPosition();
                        IBox bedCollidable = map.GetWorld().Create(bedSprite.Position.X, bedSprite.Position.Y - 10, 16, 24);
                        mapObject.SetCollisionBox(bedCollidable);
                        mapObject.SetSprite(bedSprite);
                        break;
                    case ("CHEST"):
                        // Randomly populate a chest.
                        Random randomChest = new Random();
                        int chance = randomChest.Next(1, 4);
                        if (chance == 2)
                        {
                            AnimatedSprite chestSprite = new AnimatedSprite(Sprites.GetSprite("CHEST"));
                            chestSprite.Play("Unopened");
                            chestSprite.Position = mapObject.GetPosition();
                            IBox chestCollidable = map.GetWorld().Create(chestSprite.Position.X, chestSprite.Position.Y - 10, 16, 24);
                            mapObject.SetCollisionBox(chestCollidable);
                            mapObject.SetSprite(chestSprite);
                        }
                        break;
                    case ("BOOKSHELF"):
                        AnimatedSprite bookshelfSprite = new AnimatedSprite(Sprites.GetSprite("BOOKSHELF"));
                        bookshelfSprite.Play("idle");
                        bookshelfSprite.Position = mapObject.GetPosition();
                        mapObject.SetSprite(bookshelfSprite);
                        IBox bookshelfCollidable = map.GetWorld().Create(bookshelfSprite.Position.X, bookshelfSprite.Position.Y, 16, 16);
                        mapObject.SetCollisionBox(bookshelfCollidable);
                        break;
                    case ("POT"):
                        AnimatedSprite potSprite = new AnimatedSprite(Sprites.GetSprite("POT"));
                        potSprite.Play("idle");
                        potSprite.Position = mapObject.GetPosition();
                        mapObject.SetSprite(potSprite);
                        IBox potSpriteCollidable = map.GetWorld().Create(potSprite.Position.X, potSprite.Position.Y, 16, 16);
                        mapObject.SetCollisionBox(potSpriteCollidable);
                        break;
                    case ("SHOPSHELF_1"):
                        AnimatedSprite shopShelfSprite = new AnimatedSprite(Sprites.GetSprite("SHOPSHELF_1"));
                        shopShelfSprite.Play("idle");
                        shopShelfSprite.Position = mapObject.GetPosition();
                        mapObject.SetSprite(shopShelfSprite);
                        IBox shopShelfCollidable = map.GetWorld().Create(shopShelfSprite.Position.X, shopShelfSprite.Position.Y, 16, 16);
                        mapObject.SetCollisionBox(shopShelfCollidable);
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

            // Add water tiles
            if (map.GetWaterTiles().Count > 0)
            {
                foreach (Tile waterTile in map.GetWaterTiles())
                {
                    if (waterTile.TileID != 0)
                    {
                        AnimatedSprite waterSprite = new AnimatedSprite(Sprites.GetSprite("WATER"));
                        waterSprite.Play("idle");
                        waterSprite.Position = new Vector2(waterTile.Position.X + 8, waterTile.Position.Y + 8);
                        IBox waterCollidable = map.GetWorld().Create(waterSprite.Position.X, waterSprite.Position.Y - 10, 16, 24);
                        MapObject water = new MapObject();
                        water.SetCollisionBox(waterCollidable);
                        water.SetSprite(waterSprite);
                        MapObjects.Add(water);
                    }
                }
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
                    Init.Player.EnemiesKilled += 1;
                    Init.Player.XP += enemy.XP;

                    if (Init.Player.XP >= Init.Player.XPRemaining)
                    {
                        Init.Player.Level += 1;
                        Init.Message = "You reached Level " + Init.Player.Level + "!";
                        Init.Player.MaxHealth += 5;
                        Init.Player.MaxMana += 2;
                        Init.Player.XPRemaining = Init.Player.XPRemaining + (Init.Player.XPRemaining * .50);
                        Init.Player.XP = 0;
                        LeveledUp = true;
                    }
                    enemy.State = Action.Dead;
                    enemy.Dead = true;
                    MapObject gold = new MapObject();
                    AnimatedSprite goldSprite = new AnimatedSprite(Sprites.GetSprite("GOLD"));
                    gold.SetSprite(goldSprite);
                    gold.SetPosition(new Vector2(enemy.Position.X + 15, enemy.Position.Y));
                    gold.CreateBoundingBox();
                    goldSprite.Position = gold.GetPosition();
                    gold.SetName("GOLD");
                    MapObjects.Add(gold);

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

            // Handle the player destroying objects.
            foreach (MapObject mapObject in MapObjects)
            {
                mapObject.Update(gameTime);
                if (mapObject.GetName() == "GOLD" && Init.Player.BoundingBox.Intersects(mapObject.GetBoundingBox()))
                {
                    mapObject.PickUpItem();
                    Random randomGold = new Random();
                    int goldLoot = randomGold.Next(1, 6);
                    Init.Player.Gold += goldLoot;
                    Init.Message = "You looted " + goldLoot + " gold.";
                    Init.MessageEnabled = true;
                }
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

        private Stopwatch stopWatch = new Stopwatch();
        public override void Draw(SpriteBatch spriteBatch)
        {

            if (LeveledUp)
            {
                stopWatch.Start();

                if (stopWatch.ElapsedMilliseconds <= 4000)
                {
                    spriteBatch.DrawString(Init.Font, "You reached level " + Init.Player.Level + "!", new Vector2(Init.Player.Position.X - 50, Init.Player.Position.Y - 50), Color.Yellow);
                }
                else
                {
                    stopWatch.Stop();
                    LeveledUp = false;
                }
            }
            foreach (Entity enemy in enemyList)
            {
                enemy.Draw(spriteBatch);
                Vector2 AIHealthPosition = new Vector2(enemy.Position.X - 8, enemy.Position.Y - 20);
                enemy.DrawHUD(spriteBatch, AIHealthPosition, false);
            }
            // Randomly place loot in some objects.
            Random random = new Random();

            foreach (MapObject mapObject in MapObjects)
            {
                //Item item = new Item();

                //if (objectsPopulated == false)
                //{
                //    int lootChance = random.Next(1, 4);

                //    switch (lootChance)
                //    {
                //        case (1):
                //         //   item.ItemTexture = Sprites.chickenTexture;
                //            item.Name = "Chicken";
                //            item.Width = 16;
                //            item.Height = 16;
                //            break;
                //        case (2):
                //          //  item.ItemTexture = arrowsSprite;
                //            item.Name = "Arrow";
                //            item.Width = 13;
                //            item.Height = 19;
                //            break;
                //    }

                //    if (item != null)
                //    {
                //        mapObject.SetContainedItem(item);
                //    }
                //}

                if (!mapObject.ItemPickedUp())
                {
                    mapObject.Draw(spriteBatch);
                }
            }
        }
    }
}
