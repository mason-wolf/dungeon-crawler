using DungeonCrawler;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

namespace Demo.Game
{
    public static class Enemies
    {

        public static ContentManager ContentManager;
        /// <summary>
        /// Returns an enemy by name.
        /// </summary>
        /// <param name="enemyName"></param>
        /// <returns></returns>
        public static Entity GetEnemyByName(string enemyName)
        {
            Entity enemy = new Entity();
            switch (enemyName)
            {
                case ("ZOMBIE"):
                    Entity zombieEntity = new Entity(Sprites.GetSprite("ZOMBIE"));
                    zombieEntity.LoadContent(ContentManager);
                    zombieEntity.State = Action.IdleSouth1;
                    zombieEntity.MaxHealth = 20;
                    zombieEntity.CurrentHealth = 20;
                    zombieEntity.AttackDamage = 0.01;
                    zombieEntity.Name = "ZOMBIE";
                    zombieEntity.XP = 10;
                    enemy = zombieEntity;
                    break;
                case ("BLUE_SLIME"):
                    Entity blueSlimeEntity = new Entity(Sprites.GetSprite("BLUE_SLIME"));
                    blueSlimeEntity.LoadContent(ContentManager);
                    blueSlimeEntity.State = Action.IdleEast1;
                    blueSlimeEntity.MaxHealth = 25;
                    blueSlimeEntity.CurrentHealth = 25;
                    blueSlimeEntity.AttackDamage = 0.08;
                    blueSlimeEntity.Name = "BLUE_SLIME";
                    blueSlimeEntity.XP = 10;
                    enemy = blueSlimeEntity;
                    break;
                case ("BAT"):
                    Entity batEntity = new Entity(Sprites.GetSprite("BAT"));
                    batEntity.LoadContent(ContentManager);
                    batEntity.State = Action.IdleEast1;
                    batEntity.MaxHealth = 30;
                    batEntity.CurrentHealth = 30;
                    batEntity.AttackDamage = 0.06;
                    batEntity.Name = "BAT";
                    batEntity.XP = 10;
                    enemy = batEntity;
                    break;
            }

            return enemy;
        }

        public static void Load(ContentManager contentManager)
        {
            ContentManager = contentManager;
        }
    }
}
