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
                    // 10% spawn rate.
                    blueSlimeEntity.SpawnRate = 10;
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
                case ("FIRE_BAT"):
                    Entity fireBatEntity = new Entity(Sprites.GetSprite("FIRE_BAT"));
                    fireBatEntity.LoadContent(ContentManager);
                    fireBatEntity.State = Action.IdleEast1;
                    fireBatEntity.MaxHealth = 50;
                    fireBatEntity.CurrentHealth = 50;
                    fireBatEntity.AttackDamage = 0.12;
                    fireBatEntity.Name = "FIRE_BAT";
                    fireBatEntity.FireResistance = 90;
                    fireBatEntity.XP = 10;
                    enemy = fireBatEntity;
                    break;
                case ("GREEN_SNAKE"):
                    Entity greenSnakeEntity = new Entity(Sprites.GetSprite("GREEN_SNAKE"));
                    greenSnakeEntity.LoadContent(ContentManager);
                    greenSnakeEntity.State = Action.IdleSouth1;
                    greenSnakeEntity.MaxHealth = 50;
                    greenSnakeEntity.CurrentHealth = 50;
                    greenSnakeEntity.AttackDamage = 0.12;
                    greenSnakeEntity.FireResistance = 50;
                    greenSnakeEntity.FrostResistance = 0;
                    greenSnakeEntity.Name = "GREEN_SNAKE";
                    greenSnakeEntity.XP = 20;
                    enemy = greenSnakeEntity;
                    break;
                case ("FROST_ELEMENTAL"):
                    Entity frostElementalEntity = new Entity(Sprites.GetSprite("FROST_ELEMENTAL"));
                    frostElementalEntity.LoadContent(ContentManager);
                    frostElementalEntity.State = Action.IdleWest1;
                    frostElementalEntity.MaxHealth = 50;
                    frostElementalEntity.CurrentHealth = 50;
                    frostElementalEntity.AttackDamage = .15;
                    frostElementalEntity.FireResistance = 0;
                    frostElementalEntity.FrostResistance = 99;
                    frostElementalEntity.ThunderResistance = 99;
                    frostElementalEntity.Name = "FROST_ELEMENTAL";
                    frostElementalEntity.XP = 25;
                    //frostElementalEntity.SpellCaster = true;
                    //frostElementalEntity.SpellID = 2;
                    //frostElementalEntity.MaxMana = 100;
                    //frostElementalEntity.CurrentMana = 100;
                    enemy = frostElementalEntity;
                    break;
                case ("FIRE_GOLEM"):
                    Entity fireGolem = new Entity(Sprites.GetSprite("FIRE_GOLEM"));
                    fireGolem.LoadContent(ContentManager);
                    fireGolem.MaxHealth = 65;
                    fireGolem.CurrentHealth = 60;
                    fireGolem.AttackDamage = .15;
                    fireGolem.FireResistance = 70;
                    fireGolem.FrostResistance = 0;
                    fireGolem.Name = "FIRE_GOLEM";
                    fireGolem.XP = 35;
                    fireGolem.SpawnRate = 40;
                    enemy = fireGolem;
                    break;
                case ("GREEN_DRAKE"):
                    Entity greenDrake = new Entity(Sprites.GetSprite("GREEN_DRAKE"));
                    greenDrake.LoadContent(ContentManager);
                    greenDrake.MaxHealth = 100;
                    greenDrake.CurrentHealth = 100;
                    greenDrake.AttackDamage = .30;
                    greenDrake.FireResistance = 100;
                    greenDrake.FrostResistance = 0;
                    greenDrake.Name = "GREEN_DRAKE";
                    greenDrake.XP = 50;
                    greenDrake.SpawnRate = 50;
                    enemy = greenDrake;
                    break;
                case ("COBRA"):
                    Entity cobra = new Entity(Sprites.GetSprite("COBRA"));
                    cobra.LoadContent(ContentManager);
                    cobra.MaxHealth = 50;
                    cobra.CurrentHealth = 50;
                    cobra.AttackDamage = 0.12;
                    cobra.FireResistance = 50;
                    cobra.FrostResistance = 0;
                    cobra.Name = "COBRA";
                    cobra.XP = 20;
                    cobra.SpawnRate = 20;
                    enemy = cobra;
                    break;
                case ("CINDER_DWARF"):
                    Entity cinderDwarf = new Entity(Sprites.GetSprite("CINDER_DWARF"));
                    cinderDwarf.LoadContent(ContentManager);
                    cinderDwarf.MaxHealth = 50;
                    cinderDwarf.CurrentHealth = 50;
                    cinderDwarf.AttackDamage = 0.12;
                    cinderDwarf.FireResistance = 50;
                    cinderDwarf.FrostResistance = 0;
                    cinderDwarf.Name = "CINDER_DWARF";
                    cinderDwarf.XP = 20;
                    cinderDwarf.SpawnRate = 30;
                    enemy = cinderDwarf;
                    break;
                case ("KIMODO"):
                    Entity kimodo = new Entity(Sprites.GetSprite("KIMODO"));
                    kimodo.LoadContent(ContentManager);
                    kimodo.MaxHealth = 50;
                    kimodo.CurrentHealth = 50;
                    kimodo.AttackDamage = 0.12;
                    kimodo.FireResistance = 50;
                    kimodo.FrostResistance = 0;
                    kimodo.Name = "KIMODO";
                    kimodo.XP = 20;
                    kimodo.SpawnRate = 30;
                    enemy = kimodo;
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
