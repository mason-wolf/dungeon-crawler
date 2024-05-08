using Demo.Game;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Animations.SpriteSheets;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.TextureAtlases;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace DungeonCrawler
{
    class Sprites
    {
        private static List<SpriteLoader> SpriteList;

        public void LoadContent(ContentManager content)
        {
            SpriteList = new List<SpriteLoader>();

            // Skeleton
            SpriteLoader skeletonSprite = new SpriteLoader(content, "SKELETON", @"spritesheets\Skeleton", 24, 24);
            float skeletonAnimationSpeed = .5f;
            skeletonSprite.AddAnimation("idleSouth1", new[] { 0 }, skeletonAnimationSpeed, true);
            skeletonSprite.AddAnimation("walkSouthPattern1", new[] { 0, 1, 2}, skeletonAnimationSpeed, true);
            skeletonSprite.AddAnimation("attackSouthPattern1", new[] { 0, 1, 2 }, skeletonAnimationSpeed, true);
            skeletonSprite.AddAnimation("walkWestPattern1", new[] { 0, 1, 2}, skeletonAnimationSpeed, true);
            skeletonSprite.AddAnimation("attackWestPattern1", new[] { 0, 1, 2 }, skeletonAnimationSpeed, true);
            skeletonSprite.AddAnimation("idleWest1", new[] { 1 }, skeletonAnimationSpeed, false);
            skeletonSprite.AddAnimation("walkEastPattern1", new[] { 0, 1, 2 }, skeletonAnimationSpeed, true);
            skeletonSprite.AddAnimation("attackEastPattern1", new[] { 0, 1, 2 }, skeletonAnimationSpeed, true);
            skeletonSprite.AddAnimation("idleEast1", new[] { 1 }, skeletonAnimationSpeed, false);
            skeletonSprite.AddAnimation("walkNorthPattern1", new[] { 0, 1, 2}, skeletonAnimationSpeed, true);
            skeletonSprite.AddAnimation("attackNorthPattern1", new[] { 0, 1, 2 }, skeletonAnimationSpeed, true);
            skeletonSprite.AddAnimation("idleNorth1", new[] { 1 }, skeletonAnimationSpeed, false);
            skeletonSprite.AddAnimation("dead", new[] { 3 }, .2f, isLooping: false);
            SpriteList.Add(skeletonSprite);

            // Frost Skeleton
            SpriteLoader frostSkeletonSprite = new SpriteLoader(content, "FROST_SKELETON", @"spritesheets\FROST_SKELETON", 24, 24);
            float frostSkeletonAnimationSpeed = .5f;
            frostSkeletonSprite.AddAnimation("idleSouth1", new[] { 0 }, frostSkeletonAnimationSpeed, true);
            frostSkeletonSprite.AddAnimation("walkSouthPattern1", new[] { 0, 1, 2 }, frostSkeletonAnimationSpeed, true);
            frostSkeletonSprite.AddAnimation("attackSouthPattern1", new[] { 0, 1, 2 }, frostSkeletonAnimationSpeed, true);
            frostSkeletonSprite.AddAnimation("walkWestPattern1", new[] { 0, 1, 2 }, frostSkeletonAnimationSpeed, true);
            frostSkeletonSprite.AddAnimation("attackWestPattern1", new[] { 0, 1, 2 }, frostSkeletonAnimationSpeed, true);
            frostSkeletonSprite.AddAnimation("idleWest1", new[] { 1 }, frostSkeletonAnimationSpeed, false);
            frostSkeletonSprite.AddAnimation("walkEastPattern1", new[] { 0, 1, 2 }, frostSkeletonAnimationSpeed, true);
            frostSkeletonSprite.AddAnimation("attackEastPattern1", new[] { 0, 1, 2 }, frostSkeletonAnimationSpeed, true);
            frostSkeletonSprite.AddAnimation("idleEast1", new[] { 1 }, frostSkeletonAnimationSpeed, false);
            frostSkeletonSprite.AddAnimation("walkNorthPattern1", new[] { 0, 1, 2 }, frostSkeletonAnimationSpeed, true);
            frostSkeletonSprite.AddAnimation("attackNorthPattern1", new[] { 0, 1, 2 }, frostSkeletonAnimationSpeed, true);
            frostSkeletonSprite.AddAnimation("idleNorth1", new[] { 1 }, frostSkeletonAnimationSpeed, false);
            frostSkeletonSprite.AddAnimation("dead", new[] { 3 }, .2f, isLooping: false);
            SpriteList.Add(frostSkeletonSprite);

            // Bat
            SpriteLoader batSprite = new SpriteLoader(content, "BAT", @"spritesheets\Bat", 32, 32);
            float batAnimationSpeed = .3f;
            batSprite.AddAnimation("idleSouth1", new[] { 0, 1, 2 }, batAnimationSpeed, true);
            batSprite.AddAnimation("walkSouthPattern1", new[] { 0, 1, 2 }, batAnimationSpeed, true);
            batSprite.AddAnimation("attackSouthPattern1", new[] { 0, 1, 2 }, batAnimationSpeed, true);
            batSprite.AddAnimation("walkWestPattern1", new[] { 0, 1, 2 }, batAnimationSpeed, true);
            batSprite.AddAnimation("attackWestPattern1", new[] { 0, 1, 2 }, batAnimationSpeed, true);
            batSprite.AddAnimation("idleWest1", new[] { 0, 1, 2 }, batAnimationSpeed, true);
            batSprite.AddAnimation("walkEastPattern1", new[] { 0, 1, 2 }, batAnimationSpeed, true);
            batSprite.AddAnimation("attackEastPattern1", new[] { 0, 1, 2 }, batAnimationSpeed, true);
            batSprite.AddAnimation("idleEast1", new[] { 0, 1, 2 }, batAnimationSpeed, true);
            batSprite.AddAnimation("walkNorthPattern1", new[] { 0, 1, 2 }, batAnimationSpeed, true);
            batSprite.AddAnimation("attackNorthPattern1", new[] { 0, 1, 2 }, batAnimationSpeed, true);
            batSprite.AddAnimation("idleNorth1", new[] { 0, 1, 2 }, batAnimationSpeed, true);
            batSprite.AddAnimation("dead", new[] { 3 }, batAnimationSpeed, true);
            SpriteList.Add(batSprite);

            // Fire Bat
            SpriteLoader fireBatSprite = new SpriteLoader(content, "FIRE_BAT", @"spritesheets\FIRE_BAT", 32, 32);
            float fireBatAnimationSpeed = .3f;
            fireBatSprite.AddAnimation("idleSouth1", new[] { 0, 1, 2 }, fireBatAnimationSpeed, true);
            fireBatSprite.AddAnimation("walkSouthPattern1", new[] { 0, 1, 2 }, fireBatAnimationSpeed, true);
            fireBatSprite.AddAnimation("attackSouthPattern1", new[] { 0, 1, 2 }, fireBatAnimationSpeed, true);
            fireBatSprite.AddAnimation("walkWestPattern1", new[] { 0, 1, 2 }, fireBatAnimationSpeed, true);
            fireBatSprite.AddAnimation("attackWestPattern1", new[] { 0, 1, 2 }, fireBatAnimationSpeed, true);
            fireBatSprite.AddAnimation("idleWest1", new[] { 0, 1, 2 }, fireBatAnimationSpeed, true);
            fireBatSprite.AddAnimation("walkEastPattern1", new[] { 0, 1, 2 }, fireBatAnimationSpeed, true);
            fireBatSprite.AddAnimation("attackEastPattern1", new[] { 0, 1, 2 }, fireBatAnimationSpeed, true);
            fireBatSprite.AddAnimation("idleEast1", new[] { 0, 1, 2 }, fireBatAnimationSpeed, true);
            fireBatSprite.AddAnimation("walkNorthPattern1", new[] { 0, 1, 2 }, fireBatAnimationSpeed, true);
            fireBatSprite.AddAnimation("attackNorthPattern1", new[] { 0, 1, 2 }, fireBatAnimationSpeed, true);
            fireBatSprite.AddAnimation("idleNorth1", new[] { 0, 1, 2 }, fireBatAnimationSpeed, true);
            fireBatSprite.AddAnimation("dead", new[] { 3 }, fireBatAnimationSpeed, true);
            SpriteList.Add(fireBatSprite);

            // Zombie
            SpriteLoader zombieSprite = new SpriteLoader(content, "ZOMBIE", @"spritesheets\ZOMBIE", 32, 32);
            float zombieAnimationSpeed = .3f;
            zombieSprite.AddAnimation("idleSouth1", new[] { 0 }, zombieAnimationSpeed, false);
            zombieSprite.AddAnimation("walkSouthPattern1", new[] { 0, 1, 2 }, zombieAnimationSpeed, true);
            zombieSprite.AddAnimation("attackSouthPattern1", new[] { 0, 1, 2 }, zombieAnimationSpeed, true);
            zombieSprite.AddAnimation("walkWestPattern1", new[] { 0, 1, 2 }, zombieAnimationSpeed, true);
            zombieSprite.AddAnimation("attackWestPattern1", new[] { 0, 1, 2 }, zombieAnimationSpeed, true);
            zombieSprite.AddAnimation("idleWest1", new[] { 0, 1, 2 }, zombieAnimationSpeed, true);
            zombieSprite.AddAnimation("walkEastPattern1", new[] { 0, 1, 2 }, zombieAnimationSpeed, true);
            zombieSprite.AddAnimation("attackEastPattern1", new[] { 0, 1, 2 }, zombieAnimationSpeed, true);
            zombieSprite.AddAnimation("idleEast1", new[] { 0, 1, 2 }, zombieAnimationSpeed, true);
            zombieSprite.AddAnimation("walkNorthPattern1", new[] { 0, 1, 2 }, zombieAnimationSpeed, true);
            zombieSprite.AddAnimation("attackNorthPattern1", new[] { 0, 1, 2 }, zombieAnimationSpeed, true);
            zombieSprite.AddAnimation("idleNorth1", new[] { 0, 1, 2 }, zombieAnimationSpeed, true);
            zombieSprite.AddAnimation("dead", new[] { 3 }, zombieAnimationSpeed, true);
            SpriteList.Add(zombieSprite);

            // Blue Slime
            SpriteLoader blueSlimeSprite = new SpriteLoader(content, "BLUE_SLIME", @"spritesheets\BLUE_SLIME", 32, 32);
            float blueSlimeSpeed = .3f;
            blueSlimeSprite.AddAnimation("idleSouth1", new[] { 0 }, blueSlimeSpeed, true);
            blueSlimeSprite.AddAnimation("walkSouthPattern1", new[] { 0, 1, 2 }, blueSlimeSpeed, true);
            blueSlimeSprite.AddAnimation("attackSouthPattern1", new[] { 0, 1, 2 }, blueSlimeSpeed, true);
            blueSlimeSprite.AddAnimation("walkWestPattern1", new[] { 0, 1, 2 }, blueSlimeSpeed, true);
            blueSlimeSprite.AddAnimation("attackWestPattern1", new[] { 0, 1, 2 }, blueSlimeSpeed, true);
            blueSlimeSprite.AddAnimation("idleWest1", new[] { 0, 1, 2 }, blueSlimeSpeed, true);
            blueSlimeSprite.AddAnimation("walkEastPattern1", new[] { 0, 1, 2 }, blueSlimeSpeed, true);
            blueSlimeSprite.AddAnimation("attackEastPattern1", new[] { 0, 1, 2 }, blueSlimeSpeed, true);
            blueSlimeSprite.AddAnimation("idleEast1", new[] { 0, 1, 2 }, blueSlimeSpeed, true);
            blueSlimeSprite.AddAnimation("walkNorthPattern1", new[] { 0, 1, 2 }, blueSlimeSpeed, true);
            blueSlimeSprite.AddAnimation("attackNorthPattern1", new[] { 0, 1, 2 }, blueSlimeSpeed, true);
            blueSlimeSprite.AddAnimation("idleNorth1", new[] { 0, 1, 2 }, blueSlimeSpeed, true);
            blueSlimeSprite.AddAnimation("dead", new[] { 3 }, blueSlimeSpeed, true);
            SpriteList.Add(blueSlimeSprite);

            // Yellow Slime
            SpriteLoader yellowSlimeSprite = new SpriteLoader(content, "YELLOW_SLIME", @"spritesheets\YELLOW_SLIME", 32, 32);
            float yellowSlimeSpeed = .3f;
            yellowSlimeSprite.AddAnimation("idleSouth1", new[] { 0 }, yellowSlimeSpeed, true);
            yellowSlimeSprite.AddAnimation("walkSouthPattern1", new[] { 0, 1, 2 }, yellowSlimeSpeed, true);
            yellowSlimeSprite.AddAnimation("attackSouthPattern1", new[] { 0, 1, 2 }, yellowSlimeSpeed, true);
            yellowSlimeSprite.AddAnimation("walkWestPattern1", new[] { 0, 1, 2 }, yellowSlimeSpeed, true);
            yellowSlimeSprite.AddAnimation("attackWestPattern1", new[] { 0, 1, 2 }, yellowSlimeSpeed, true);
            yellowSlimeSprite.AddAnimation("idleWest1", new[] { 0, 1, 2 }, yellowSlimeSpeed, true);
            yellowSlimeSprite.AddAnimation("walkEastPattern1", new[] { 0, 1, 2 }, yellowSlimeSpeed, true);
            yellowSlimeSprite.AddAnimation("attackEastPattern1", new[] { 0, 1, 2 }, yellowSlimeSpeed, true);
            yellowSlimeSprite.AddAnimation("idleEast1", new[] { 0, 1, 2 }, yellowSlimeSpeed, true);
            yellowSlimeSprite.AddAnimation("walkNorthPattern1", new[] { 0, 1, 2 }, yellowSlimeSpeed, true);
            yellowSlimeSprite.AddAnimation("attackNorthPattern1", new[] { 0, 1, 2 }, yellowSlimeSpeed, true);
            yellowSlimeSprite.AddAnimation("idleNorth1", new[] { 0, 1, 2 }, yellowSlimeSpeed, true);
            yellowSlimeSprite.AddAnimation("dead", new[] { 3 }, yellowSlimeSpeed, true);
            SpriteList.Add(yellowSlimeSprite);

            // Green Ghost
            SpriteLoader greenGhostSprite = new SpriteLoader(content, "GREEN_GHOST", @"spritesheets\GREEN_GHOST", 24, 24);
            float greenGhostSpeed = .6f;
            greenGhostSprite.AddAnimation("idleSouth1", new[] { 0 }, greenGhostSpeed, true);
            greenGhostSprite.AddAnimation("walkSouthPattern1", new[] { 0, 1, 2 }, greenGhostSpeed, true);
            greenGhostSprite.AddAnimation("attackSouthPattern1", new[] { 0, 1, 2 }, greenGhostSpeed, true);
            greenGhostSprite.AddAnimation("walkWestPattern1", new[] { 0, 1, 2 }, greenGhostSpeed, true);
            greenGhostSprite.AddAnimation("attackWestPattern1", new[] { 0, 1, 2 }, greenGhostSpeed, true);
            greenGhostSprite.AddAnimation("idleWest1", new[] { 0, 1, 2 }, greenGhostSpeed, true);
            greenGhostSprite.AddAnimation("walkEastPattern1", new[] { 0, 1, 2 }, greenGhostSpeed, true);
            greenGhostSprite.AddAnimation("attackEastPattern1", new[] { 0, 1, 2 }, greenGhostSpeed, true);
            greenGhostSprite.AddAnimation("idleEast1", new[] { 0, 1, 2 }, greenGhostSpeed, true);
            greenGhostSprite.AddAnimation("walkNorthPattern1", new[] { 0, 1, 2 }, greenGhostSpeed, true);
            greenGhostSprite.AddAnimation("attackNorthPattern1", new[] { 0, 1, 2 }, greenGhostSpeed, true);
            greenGhostSprite.AddAnimation("idleNorth1", new[] { 0, 1, 2 }, greenGhostSpeed, true);
            greenGhostSprite.AddAnimation("dead", new[] { 3 }, greenGhostSpeed, true);
            SpriteList.Add(greenGhostSprite);

            // Green Snake
            SpriteLoader greenSnakeSprite = new SpriteLoader(content, "GREEN_SNAKE", @"spritesheets\GREEN_SNAKE", 24, 24);
            float greenSnakeSpeed = .6f;
            greenSnakeSprite.AddAnimation("idleSouth1", new[] { 0 }, greenGhostSpeed, false);
            greenSnakeSprite.AddAnimation("walkSouthPattern1", new[] { 0, 1, 2 }, greenSnakeSpeed, true);
            greenSnakeSprite.AddAnimation("attackSouthPattern1", new[] { 0, 1, 2 }, greenSnakeSpeed, true);
            greenSnakeSprite.AddAnimation("walkWestPattern1", new[] { 0, 1, 2 }, greenSnakeSpeed, true);
            greenSnakeSprite.AddAnimation("attackWestPattern1", new[] { 0, 1, 2 }, greenSnakeSpeed, true);
            greenSnakeSprite.AddAnimation("idleWest1", new[] { 0, 1, 2 }, greenSnakeSpeed, true);
            greenSnakeSprite.AddAnimation("walkEastPattern1", new[] { 0, 1, 2 }, greenSnakeSpeed, true);
            greenSnakeSprite.AddAnimation("attackEastPattern1", new[] { 0, 1, 2 }, greenSnakeSpeed, true);
            greenSnakeSprite.AddAnimation("idleEast1", new[] { 0, 1, 2 }, greenSnakeSpeed, true);
            greenSnakeSprite.AddAnimation("walkNorthPattern1", new[] { 0, 1, 2 }, greenSnakeSpeed, true);
            greenSnakeSprite.AddAnimation("attackNorthPattern1", new[] { 0, 1, 2 }, greenSnakeSpeed, true);
            greenSnakeSprite.AddAnimation("idleNorth1", new[] { 0, 1, 2 }, greenSnakeSpeed, true);
            greenSnakeSprite.AddAnimation("dead", new[] { 3 }, greenSnakeSpeed, true);
            SpriteList.Add(greenSnakeSprite);

            // Troll
            SpriteLoader trollSprite = new SpriteLoader(content, "TROLL", @"spritesheets\TROLL", 16, 16);
            float trollSpeed = .6f;
            trollSprite.AddAnimation("idleSouth1", new[] { 0 }, trollSpeed, false);
            trollSprite.AddAnimation("walkSouthPattern1", new[] { 0, 1, 2 }, trollSpeed, true);
            trollSprite.AddAnimation("attackSouthPattern1", new[] { 0, 1, 2 }, trollSpeed, true);
            trollSprite.AddAnimation("walkWestPattern1", new[] { 0, 1, 2 }, trollSpeed, true);
            trollSprite.AddAnimation("attackWestPattern1", new[] { 0, 1, 2 }, trollSpeed, true);
            trollSprite.AddAnimation("idleWest1", new[] { 0, 1, 2 }, trollSpeed, true);
            trollSprite.AddAnimation("walkEastPattern1", new[] { 0, 1, 2 }, trollSpeed, true);
            trollSprite.AddAnimation("attackEastPattern1", new[] { 0, 1, 2 }, trollSpeed, true);
            trollSprite.AddAnimation("idleEast1", new[] { 0, 1, 2 }, trollSpeed, true);
            trollSprite.AddAnimation("walkNorthPattern1", new[] { 0, 1, 2 }, trollSpeed, true);
            trollSprite.AddAnimation("attackNorthPattern1", new[] { 0, 1, 2 }, trollSpeed, true);
            trollSprite.AddAnimation("idleNorth1", new[] { 0, 1, 2 }, trollSpeed, true);
            trollSprite.AddAnimation("dead", new[] { 3 }, trollSpeed, true);
            SpriteList.Add(trollSprite);

            // Frost Elemental
            SpriteLoader frostElementalSprite = new SpriteLoader(content, "FROST_ELEMENTAL", @"spritesheets\FROST_ELEMENTAL", 16, 16);
            float frostElementalSpeed = 0.6f;
            frostElementalSprite.AddAnimation("idleSouth1", new[] { 0, 1, 2 }, frostElementalSpeed, true);
            frostElementalSprite.AddAnimation("walkSouthPattern1", new[] { 0, 1, 2 }, frostElementalSpeed, true);
            frostElementalSprite.AddAnimation("attackSouthPattern1", new[] { 0, 1, 2 }, frostElementalSpeed, true);
            frostElementalSprite.AddAnimation("walkWestPattern1", new[] { 0, 1, 2 }, frostElementalSpeed, true);
            frostElementalSprite.AddAnimation("attackWestPattern1", new[] { 0, 1, 2 }, frostElementalSpeed, true);
            frostElementalSprite.AddAnimation("idleWest1", new[] { 0, 1, 2 }, frostElementalSpeed, true);
            frostElementalSprite.AddAnimation("walkEastPattern1", new[] { 0, 1, 2 }, frostElementalSpeed, true);
            frostElementalSprite.AddAnimation("attackEastPattern1", new[] { 0, 1, 2 }, frostElementalSpeed, true);
            frostElementalSprite.AddAnimation("idleEast1", new[] { 0, 1, 2 }, frostElementalSpeed, true);
            frostElementalSprite.AddAnimation("walkNorthPattern1", new[] { 0, 1, 2 }, frostElementalSpeed, true);
            frostElementalSprite.AddAnimation("attackNorthPattern1", new[] { 0, 1, 2 }, frostElementalSpeed, true);
            frostElementalSprite.AddAnimation("idleNorth1", new[] { 0, 1, 2 }, frostElementalSpeed, true);
            frostElementalSprite.AddAnimation("dead", new[] { 3 }, frostElementalSpeed, true);
            SpriteList.Add(frostElementalSprite);

            // Crab
            SpriteLoader crabSprite = new SpriteLoader(content, "CRAB", @"spritesheets\CRAB", 32, 32);
            float crabSpeed = 0.6f;
            crabSprite.AddAnimation("idleSouth1", new[] { 0, 1, 2 }, crabSpeed, false);
            crabSprite.AddAnimation("walkSouthPattern1", new[] { 0, 1, 2 }, crabSpeed, true);
            crabSprite.AddAnimation("attackSouthPattern1", new[] { 0, 1, 2 }, crabSpeed, true);
            crabSprite.AddAnimation("walkWestPattern1", new[] { 0, 1, 2 }, crabSpeed, true);
            crabSprite.AddAnimation("attackWestPattern1", new[] { 0, 1, 2 }, crabSpeed, true);
            crabSprite.AddAnimation("idleWest1", new[] { 0, 1, 2 }, crabSpeed, true);
            crabSprite.AddAnimation("walkEastPattern1", new[] { 0, 1, 2 }, crabSpeed, true);
            crabSprite.AddAnimation("attackEastPattern1", new[] { 0, 1, 2 }, crabSpeed, true);
            crabSprite.AddAnimation("idleEast1", new[] { 0, 1, 2 }, crabSpeed, true);
            crabSprite.AddAnimation("walkNorthPattern1", new[] { 0, 1, 2 }, crabSpeed, true);
            crabSprite.AddAnimation("attackNorthPattern1", new[] { 0, 1, 2 }, crabSpeed, true);
            crabSprite.AddAnimation("idleNorth1", new[] { 0, 1, 2 }, crabSpeed, true);
            crabSprite.AddAnimation("dead", new[] { 3 }, crabSpeed, true);
            SpriteList.Add(crabSprite);

            // Fire Golem
            SpriteLoader fireGolem = new SpriteLoader(content, "FIRE_GOLEM", @"spritesheets\fire_golem", 32, 32);
            float fireGolemSpeed = 0.6f;
            fireGolem.AddAnimation("idleSouth1", new[] { 0, 1, 2, 3 }, fireGolemSpeed, false);
            fireGolem.AddAnimation("walkSouthPattern1", new[] { 0, 1, 2, 3 }, fireGolemSpeed, true);
            fireGolem.AddAnimation("attackSouthPattern1", new[] { 0, 1, 2, 3 }, fireGolemSpeed, true);
            fireGolem.AddAnimation("walkWestPattern1", new[] { 0, 1, 2, 3 }, fireGolemSpeed, true);
            fireGolem.AddAnimation("attackWestPattern1", new[] { 0, 1, 2, 3 }, fireGolemSpeed, true);
            fireGolem.AddAnimation("idleWest1", new[] { 0, 1, 2, 3 }, fireGolemSpeed, true);
            fireGolem.AddAnimation("walkEastPattern1", new[] { 0, 1, 2, 3 }, fireGolemSpeed, true);
            fireGolem.AddAnimation("attackEastPattern1", new[] { 0, 1, 2, 3 }, fireGolemSpeed, true);
            fireGolem.AddAnimation("idleEast1", new[] { 0, 1, 2, 3 }, fireGolemSpeed, true);
            fireGolem.AddAnimation("walkNorthPattern1", new[] { 0, 1, 2, 3 }, fireGolemSpeed, true);
            fireGolem.AddAnimation("attackNorthPattern1", new[] { 0, 1, 2, 3 }, fireGolemSpeed, true);
            fireGolem.AddAnimation("idleNorth1", new[] { 0, 1, 2, 3 }, fireGolemSpeed, true);
            fireGolem.AddAnimation("dead", new[] { 4 }, fireGolemSpeed, true);
            SpriteList.Add(fireGolem);

            // Green Drake
            SpriteLoader greenDrake = new SpriteLoader(content, "GREEN_DRAKE", @"spritesheets\green_drake", 32, 32);
            float greenDrakeSpeed = 0.6f;
            greenDrake.AddAnimation("idleSouth1", new[] { 0, 1, 2 }, greenDrakeSpeed, false);
            greenDrake.AddAnimation("walkSouthPattern1", new[] { 0, 1, 2 }, greenDrakeSpeed, true);
            greenDrake.AddAnimation("attackSouthPattern1", new[] { 0, 1, 2 }, greenDrakeSpeed, true);
            greenDrake.AddAnimation("walkWestPattern1", new[] { 5 }, greenDrakeSpeed, true);
            greenDrake.AddAnimation("attackWestPattern1", new[] { 5 }, greenDrakeSpeed, true);
            greenDrake.AddAnimation("idleWest1", new[] { 5 }, greenDrakeSpeed, true);
            greenDrake.AddAnimation("walkEastPattern1", new[] { 3 }, greenDrakeSpeed, true);
            greenDrake.AddAnimation("attackEastPattern1", new[] { 3 }, greenDrakeSpeed, true);
            greenDrake.AddAnimation("idleEast1", new[] { 3 }, greenDrakeSpeed, true);
            greenDrake.AddAnimation("walkNorthPattern1", new[] { 0, 1, 2 }, greenDrakeSpeed, true);
            greenDrake.AddAnimation("attackNorthPattern1", new[] { 0, 1, 2 }, greenDrakeSpeed, true);
            greenDrake.AddAnimation("idleNorth1", new[] { 0, 1, 2 }, greenDrakeSpeed, true);
            greenDrake.AddAnimation("dead", new[] { 4 }, greenDrakeSpeed, true);
            SpriteList.Add(greenDrake);

            // Cobra
            SpriteLoader cobra = new SpriteLoader(content, "COBRA", @"spritesheets\cobra", 32, 32);
            float cobraSpeed = 0.6f;
            cobra.AddAnimation("idleSouth1", new[] { 0, 1, 2 }, cobraSpeed, false);
            cobra.AddAnimation("walkSouthPattern1", new[] { 0, 1, 2 }, cobraSpeed, true);
            cobra.AddAnimation("attackSouthPattern1", new[] { 0, 1, 2 }, cobraSpeed, true);
            cobra.AddAnimation("walkWestPattern1", new[] { 4, 5, 7 }, cobraSpeed, true);
            cobra.AddAnimation("attackWestPattern1", new[] { 4, 5, 7 }, cobraSpeed, true);
            cobra.AddAnimation("idleWest1", new[] { 4 }, cobraSpeed, true);
            cobra.AddAnimation("walkEastPattern1", new[] { 0, 1, 2 }, cobraSpeed, true);
            cobra.AddAnimation("attackEastPattern1", new[] { 0, 1, 2 }, cobraSpeed, true);
            cobra.AddAnimation("idleEast1", new[] { 0 }, cobraSpeed, true);
            cobra.AddAnimation("walkNorthPattern1", new[] { 0, 1, 2 }, cobraSpeed, true);
            cobra.AddAnimation("attackNorthPattern1", new[] { 0, 1, 2 }, cobraSpeed, true);
            cobra.AddAnimation("idleNorth1", new[] { 0, 1, 2 }, cobraSpeed, true);
            cobra.AddAnimation("dead", new[] { 4 }, cobraSpeed, true);
            SpriteList.Add(cobra);


            // Torch
            SpriteLoader torchSprite = new SpriteLoader(content, "TORCH", @"objects\torch", 32, 32);
            torchSprite.AddAnimation("BURNING", new[] { 0, 1, 2 }, 0.09f, true);
            SpriteList.Add(torchSprite);

            // Desk
            SpriteLoader deskSprite = new SpriteLoader(content, "DESK", @"objects\desk", 24, 24);
            deskSprite.AddAnimation("idle", new[] { 0 }, 0.09f, false);
            SpriteList.Add(deskSprite);

            // Fire Mage
            SpriteLoader fireProfessorSprite = new SpriteLoader(content, "FIRE_MAGE", @"spritesheets\fire_mage", 24, 24);
            fireProfessorSprite.AddAnimation("idleSouth1", new[] { 0 }, 0.9f, false);
            SpriteList.Add(fireProfessorSprite);

            // Frost Mage
            SpriteLoader frostProfessorSprite = new SpriteLoader(content, "FROST_MAGE", @"spritesheets\FROST_MAGE", 24, 24);
            frostProfessorSprite.AddAnimation("idleSouth1", new[] { 0 }, 0.9f, false);
            SpriteList.Add(frostProfessorSprite);

            // Thunder Mage
            SpriteLoader thunderProfessorSprite = new SpriteLoader(content, "THUNDER_MAGE", @"spritesheets\THUNDER_MAGE", 24, 24);
            thunderProfessorSprite.AddAnimation("idleSouth1", new[] { 0 }, 0.9f, false);
            SpriteList.Add(thunderProfessorSprite);

            // Green Mage
            SpriteLoader librarianSprite = new SpriteLoader(content, "GREEN_MAGE", @"spritesheets\GREEN_MAGE", 24, 24);
            librarianSprite.AddAnimation("idleSouth1", new[] { 3 }, 0.9f, true);
            SpriteList.Add(librarianSprite);

            // White Mage
            SpriteLoader whiteMageSprite = new SpriteLoader(content, "WHITE_MAGE", @"spritesheets\WHITE_MAGE", 24, 24);
            whiteMageSprite.AddAnimation("idleSouth1", new[] { 0 }, 0.9f, true);
            SpriteList.Add(whiteMageSprite);

            // Item Merchant
            SpriteLoader itemMerchantSprite = new SpriteLoader(content, "ITEM_MERCHANT", @"spritesheets\ITEM_MERCHANT", 24, 24);
            itemMerchantSprite.AddAnimation("idleSouth1", new[] { 3 }, 0.9f, false);
            itemMerchantSprite.AddAnimation("idleWest1", new[] { 5 }, 0.9f, false);
            SpriteList.Add(itemMerchantSprite);

            // Novice Mage
            SpriteLoader noviceMageSprite = new SpriteLoader(content, "NOVICE_MAGE", @"spritesheets\NOVICE_MAGE", 24, 24);
            noviceMageSprite.AddAnimation("idleSouth1", new[] { 0 }, 0.9f, false);
            noviceMageSprite.AddAnimation("idleEast1", new[] { 3 }, 2f, true);
            noviceMageSprite.AddAnimation("idleNorth1", new[] { 7 }, 2f, true);
            SpriteList.Add(noviceMageSprite);

            // Guard
            SpriteLoader guardSprite = new SpriteLoader(content, "GUARD", @"spritesheets\knight", 24, 24);
            guardSprite.AddAnimation("idleSouth1", new[] { 0 }, 0.9f, false);
            SpriteList.Add(guardSprite);

            // Portal
            SpriteLoader portalSprite = new SpriteLoader(content, "PORTAL", @"spritesheets\portal", 32, 32);
            portalSprite.AddAnimation("idleSouth1", new[] { 0, 1, 2, 3 }, 0.09f, true);
            SpriteList.Add(portalSprite);

            // Green Portal
            SpriteLoader greenPortalSprite = new SpriteLoader(content, "GREEN_PORTAL", @"spritesheets\green_portal", 32, 32);
            greenPortalSprite.AddAnimation("idle", new[] { 0, 1, 2, 3, 4 }, 0.09f, true);
            SpriteList.Add(greenPortalSprite);

            // Red Portal
            SpriteLoader redPortalSprite = new SpriteLoader(content, "RED_PORTAL", @"spritesheets\RED_PORTAL", 32, 32);
            redPortalSprite.AddAnimation("idle", new[] { 0, 1, 2, 3, 4 }, 0.09f, true);
            SpriteList.Add(redPortalSprite);

            // Blue Portal
            SpriteLoader bluePortalSprite = new SpriteLoader(content, "BLUE_PORTAL", @"spritesheets\BLUE_PORTAL", 32, 32);
            bluePortalSprite.AddAnimation("idle", new[] { 0, 1, 2, 3, 4 }, 0.09f, true);
            SpriteList.Add(bluePortalSprite);

            // Yellow Portal
            SpriteLoader yellowPortalSprite = new SpriteLoader(content, "YELLOW_PORTAL", @"spritesheets\YELLOW_PORTAL", 32, 32);
            yellowPortalSprite.AddAnimation("idle", new[] { 0, 1, 2, 3, 4 }, 0.09f, true);
            SpriteList.Add(yellowPortalSprite);

            // Yellow Portal
            SpriteLoader purplePortalSprite = new SpriteLoader(content, "PURPLE_PORTAL", @"spritesheets\PURPLE_PORTAL", 32, 32);
            purplePortalSprite.AddAnimation("idle", new[] { 0, 1, 2, 3, 4 }, 0.09f, true);
            SpriteList.Add(purplePortalSprite);

            // Bookshelf
            SpriteLoader bookshelfSprite = new SpriteLoader(content, "BOOKSHELF", @"objects\bookshelf", 24, 24);
            bookshelfSprite.AddAnimation("idle", new[] { 0 }, 0.09f, true);
            SpriteList.Add(bookshelfSprite);

            // Shopshelf
            SpriteLoader shopShelfSprite = new SpriteLoader(content, "SHOPSHELF_1", @"objects\SHOP_SHELF_1", 24, 24);
            shopShelfSprite.AddAnimation("idle", new[] { 0 }, 0.09f, true);
            SpriteList.Add(shopShelfSprite);

            // Water
            SpriteLoader waterSprite = new SpriteLoader(content, "WATER", @"objects\WATER", 16, 16);
            waterSprite.AddAnimation("idle", new[] { 0, 1, 2, 3 }, .5f, true);
            SpriteList.Add(waterSprite);

            // Pot
            SpriteLoader potSprite = new SpriteLoader(content, "POT", @"objects\POT", 16, 16);
            potSprite.AddAnimation("idle", new[] { 0 }, .5f, true);
            SpriteList.Add(potSprite);

            // Fireball 1
            SpriteLoader fireball1Sprite = new SpriteLoader(content, "FIREBALL_1", @"spritesheets\FIREBALL_1", 16, 16);
            fireball1Sprite.AddAnimation("attackEastPattern1", new[] { 0, 1, 2, 3 }, 0.06f, true);
            fireball1Sprite.AddAnimation("idleEast1", new[] { 0, 1, 2, 3 }, 0.06f, true);
            fireball1Sprite.AddAnimation("attackSouthPattern1", new[] { 4, 5, 6, 7 }, 0.06f, true);
            fireball1Sprite.AddAnimation("idleSouth1", new[] { 4, 5, 6, 7 }, 0.06f, true);
            fireball1Sprite.AddAnimation("attackWestPattern1", new[] { 8, 9, 10 ,11}, 0.06f, true);
            fireball1Sprite.AddAnimation("idleWest1", new[] { 8, 9, 10, 11 }, 0.06f, true);
            fireball1Sprite.AddAnimation("attackNorthPattern1", new[] { 12, 13, 14, 15 }, 0.06f, true);
            fireball1Sprite.AddAnimation("idleNorth1", new[] { 12, 13, 14, 15}, 0.06f, true);
            SpriteList.Add(fireball1Sprite);

            // Icebolt 1
            SpriteLoader icebolt1Sprite = new SpriteLoader(content, "ICEBOLT_1", @"spritesheets\ICEBOLT_1", 16, 16);
            icebolt1Sprite.AddAnimation("attackEastPattern1", new[] { 0, 1, 2, 3 }, 0.06f, true);
            icebolt1Sprite.AddAnimation("idleEast1", new[] { 0, 1, 2, 3 }, 0.06f, true);
            icebolt1Sprite.AddAnimation("attackSouthPattern1", new[] { 4, 5, 6, 7 }, 0.06f, true);
            icebolt1Sprite.AddAnimation("idleSouth1", new[] { 4, 5, 6, 7 }, 0.06f, true);
            icebolt1Sprite.AddAnimation("attackWestPattern1", new[] { 8, 9, 10, 11 }, 0.06f, true);
            icebolt1Sprite.AddAnimation("idleWest1", new[] { 8, 9, 10, 11 }, 0.06f, true);
            icebolt1Sprite.AddAnimation("attackNorthPattern1", new[] { 12, 13, 14, 15 }, 0.06f, true);
            icebolt1Sprite.AddAnimation("idleNorth1", new[] { 12, 13, 14, 15 }, 0.06f, true);
            SpriteList.Add(icebolt1Sprite);

            // Thunderbolt 1 East/West
            SpriteLoader thunderBolt1EastWestSprite = new SpriteLoader(content, "THUNDERBOLT_1", @"spritesheets\THUNDERBOLT_1_EAST_WEST", 64, 16);
            thunderBolt1EastWestSprite.AddAnimation("attackEastPattern1", new[] { 0, 1 }, 0.06f, true);
            thunderBolt1EastWestSprite.AddAnimation("idleEast1", new[] { 0, 1 }, 0.06f, true);
            thunderBolt1EastWestSprite.AddAnimation("idleWest1", new[] { 0, 1 }, 0.06f, true);
            SpriteList.Add(thunderBolt1EastWestSprite);

            // Thunderbolt 1 North/South
            SpriteLoader thunderBolt1NorthSouthSprite = new SpriteLoader(content, "THUNDERBOLT_1_NORTH_SOUTH", @"spritesheets\THUNDERBOLT_1_NORTH_SOUTH", 16, 64);
            thunderBolt1NorthSouthSprite.AddAnimation("attackNorthPattern1", new[] { 0, 1 }, 0.06f, true);
            thunderBolt1NorthSouthSprite.AddAnimation("idleNorth1", new[] { 0, 1 }, 0.06f, true);
            thunderBolt1NorthSouthSprite.AddAnimation("attackSouthPattern1", new[] { 0, 1 }, 0.06f, true);
            thunderBolt1NorthSouthSprite.AddAnimation("idleSouth1", new[] { 0, 1 }, 0.06f, true);
            SpriteList.Add(thunderBolt1NorthSouthSprite);

            // Heal 1
            SpriteLoader heal1Sprite = new SpriteLoader(content, "HEAL_1", @"spritesheets\HEAL_1", 16, 16);
            heal1Sprite.AddAnimation("idle", new[] { 0, 1, 2, 3, 2, 1, 0, 1, 2 }, .09f, false);
            SpriteList.Add(heal1Sprite);

            // Chest
            SpriteLoader chestSprite = new SpriteLoader(content, "CHEST", @"objects\CHEST", 32, 32);
            chestSprite.AddAnimation("Unopened", new[] { 0 }, 1f, false);
            chestSprite.AddAnimation("Opened", new[] { 1 }, 1f, false);
            SpriteList.Add(chestSprite);

            // Flame Shield
            SpriteLoader flameShieldSprite = new SpriteLoader(content, "FLAME_SHIELD", @"spritesheets\FLAME_SHIELD", 16, 16);
            SpriteList.Add(flameShieldSprite);

            // Fireball 1 Icon
            SpriteLoader fireBall1Icon = new SpriteLoader(content, "FIREBALL_1_ICON", @"items\FIREBALL_1_ICON", 16, 16);

            // Icebolt 1 Icon
            SpriteLoader icebolt1Icon = new SpriteLoader(content, "ICEBOLT_1_ICON", @"items\ICEBOLT_1_ICON", 16, 16);

            // Thunderbolt 1 Icon
            SpriteLoader thunderbolt1Icon = new SpriteLoader(content, "THUNDERBOLT_1_ICON", @"items\THUNDERBOLT_1_ICON", 16, 16);

            // Flame shield Icon
            SpriteLoader flameShieldIcon = new SpriteLoader(content, "FLAME_SHIELD_ICON", @"items\FLAME_SHIELD_ICON", 16, 16);

            // Heal 1 Icon
            SpriteLoader heal1Icon = new SpriteLoader(content, "HEAL_1_ICON", @"items\HEAL_1_ICON", 16, 16);

            // Health Potion Icon
            SpriteLoader healthPotionIcon = new SpriteLoader(content, "HEALTH_POTION_ICON", @"items\HEALTH_POTION_ICON", 16, 16);

            // Mana Potion Icon
            SpriteLoader manaPotionIcon = new SpriteLoader(content, "MANA_POTION_ICON", @"items\MANA_POTION_ICON", 16, 16);

            // Elixir Potion Icon
            SpriteLoader elixirPotionIcon = new SpriteLoader(content, "ELIXIR_POTION_ICON", @"items\ELIXIR_POTION_ICON", 16, 16);
            SpriteList.Add(elixirPotionIcon);

            // Gold Icon
            SpriteLoader goldIcon = new SpriteLoader(content, "GOLD_ICON", @"items\GOLD_ICON", 16, 16);

            // Gold Loot
            SpriteLoader goldSprite = new SpriteLoader(content, "GOLD", @"items\GOLD", 16, 16);
            goldSprite.AddAnimation("idle", new[] { 0 }, 0.06f, true);
            SpriteList.Add(goldSprite);

            // Fire Shard Icon
            SpriteLoader fireShardSprite = new SpriteLoader(content, "FIRE_SHARD", @"items\FIRE_SHARD_ICON", 16, 16);
            SpriteList.Add(fireShardSprite);

            // Frost Shard Icon
            SpriteLoader frostShardSprite = new SpriteLoader(content, "FROST_SHARD", @"items\FROST_SHARD_ICON", 16, 16);
            SpriteList.Add(frostShardSprite);

            // Thunder Shard Icon 
            SpriteLoader thunderShardSprite = new SpriteLoader(content, "THUNDER_SHARD", @"items\THUNDER_SHARD_ICON", 16, 16);
            SpriteList.Add(thunderShardSprite);

            // Book Icon
            SpriteLoader bookSprite = new SpriteLoader(content, "BOOK", @"items\BOOK", 16, 16);
            bookSprite.AddAnimation("idle", new[] { 0 }, 0.5f, false);
            SpriteList.Add(bookSprite);

            // Spell Upgrade Icon
            SpriteLoader spellUpgrade = new SpriteLoader(content, "SPELL_POWER_UPGRADE", @"items\SPELL_POWER_UPGRADE", 16, 16);
            spellUpgrade.AddAnimation("idle", new[] { 0 }, 0.5f, false);
            SpriteList.Add(spellUpgrade);

            // Bed
            SpriteLoader bedSprite = new SpriteLoader(content, "BED", @"objects\BED", 24, 32);
            bedSprite.AddAnimation("idle", new[] { 0 }, 1f, true);

            // Homing Crystal
            SpriteLoader homingCrystalSprite = new SpriteLoader(content, "HOMING_CRYSTAL", @"items\HOMING_CRYSTAL", 16, 16);
            homingCrystalSprite.AddAnimation("idle", new[] { 0 }, 1f, true);

            // Hat 1 Icon
            SpriteLoader hat1Icon = new SpriteLoader(content, "HAT_1_ICON", @"items\HAT_1_ICON", 16, 16);
            SpriteList.Add(hat1Icon);

            // Robe 1 Icon
            SpriteLoader robe1Icon = new SpriteLoader(content, "ROBE_1_ICON", @"items\ROBE_1_ICON", 16, 16);
            SpriteList.Add(robe1Icon);

            // Gloves 1 ICON
            SpriteLoader gloves1Icon = new SpriteLoader(content, "GLOVES_1_ICON", @"items\GLOVES_1_ICON", 16, 16);
            SpriteList.Add(gloves1Icon);

            // Boots 1 Icon
            SpriteLoader boots1Icon = new SpriteLoader(content, "BOOTS_1_ICON", @"items\BOOTS_1_ICON", 16, 16);
            SpriteList.Add(boots1Icon);

            // Ring 1 Icon
            SpriteLoader ring1Icon = new SpriteLoader(content, "RING_1_ICON", @"items\RING_1_ICON", 16, 16);
            SpriteList.Add(ring1Icon);

            SpriteList.Add(homingCrystalSprite);
            SpriteList.Add(fireBall1Icon);
            SpriteList.Add(icebolt1Icon);
            SpriteList.Add(thunderbolt1Icon);
            SpriteList.Add(healthPotionIcon);
            SpriteList.Add(goldIcon);
            SpriteList.Add(manaPotionIcon);
            SpriteList.Add(bedSprite);
            SpriteList.Add(heal1Icon);
            SpriteList.Add(flameShieldIcon);

        }

        public static Texture2D GetTexture(string spriteName)
        {
            Texture2D texture = null;
            foreach (SpriteLoader sprite in SpriteList)
            {
                if (sprite.Name == spriteName)
                {
                    texture = sprite.GetTexture();
                }
            }

            return texture;
        }

        public static SpriteSheetAnimationFactory GetSprite(string spriteName)
        {
            SpriteSheetAnimationFactory foundSprite = null;
            foreach (SpriteLoader sprite in SpriteList)
            {
                if (sprite.Name == spriteName)
                {
                    foundSprite = sprite.GetAnimation();
                }
            }

            if (foundSprite == null)
            {
                Console.WriteLine("Sprite " + spriteName + " was not found.");
            }
            return foundSprite;
        }
    }
}
