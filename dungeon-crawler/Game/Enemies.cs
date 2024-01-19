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
                    zombieEntity.AttackDamage = 0.02;
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
                    batEntity.MaxHealth = 15;
                    batEntity.CurrentHealth = 15;
                    batEntity.AttackDamage = 0.01;
                    batEntity.Name = "BAT";
                    batEntity.XP = 10;
                    enemy = batEntity;
                    break;
                case ("SKELETON"):
                    Entity skeletonEntity = new Entity(Sprites.GetSprite("SKELETON"));
                    skeletonEntity.LoadContent(ContentManager);
                    skeletonEntity.State = Action.IdleSouth1;
                    skeletonEntity.MaxHealth = 30;
                    skeletonEntity.CurrentHealth = 30;
                    skeletonEntity.AttackDamage = 0.08;
                    skeletonEntity.Name = "SKELETON";
                    skeletonEntity.XP = 10;
                    //skeletonEntity.SpellCaster = true;
                    //skeletonEntity.SpellID = 1;
                    //skeletonEntity.CurrentMana = 1000;
                    skeletonEntity.FireResistance = 50;
                    enemy = skeletonEntity;
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
