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
            float skeletonAnimationSpeed = .3f;
            skeletonSprite.AddAnimation("idleSouth1", new[] { 1 }, skeletonAnimationSpeed, true);
            skeletonSprite.AddAnimation("walkSouthPattern1", new[] { 1, 2, 3, 2 }, skeletonAnimationSpeed, true);
            skeletonSprite.AddAnimation("attackSouthPattern1", new[] { 1, 2, 3, 2 }, skeletonAnimationSpeed, true);
            skeletonSprite.AddAnimation("walkWestPattern1", new[] { 1, 2, 3, 2 }, skeletonAnimationSpeed, true);
            skeletonSprite.AddAnimation("attackWestPattern1", new[] { 1, 2, 3, 2 }, skeletonAnimationSpeed, true);
            skeletonSprite.AddAnimation("idleWest1", new[] { 1 }, skeletonAnimationSpeed, false);
            skeletonSprite.AddAnimation("walkEastPattern1", new[] { 1, 2, 3, 2 }, skeletonAnimationSpeed, true);
            skeletonSprite.AddAnimation("attackEastPattern1", new[] { 1, 2, 3, 2 }, skeletonAnimationSpeed, true);
            skeletonSprite.AddAnimation("idleEast1", new[] { 1 }, skeletonAnimationSpeed, false);
            skeletonSprite.AddAnimation("walkNorthPattern1", new[] { 1, 2, 3, 2 }, skeletonAnimationSpeed, true);
            skeletonSprite.AddAnimation("attackNorthPattern1", new[] { 1, 2, 3, 2 }, skeletonAnimationSpeed, true);
            skeletonSprite.AddAnimation("idleNorth1", new[] { 1 }, skeletonAnimationSpeed, false);
            skeletonSprite.AddAnimation("dead", new[] { 4 }, .2f, isLooping: false);
            SpriteList.Add(skeletonSprite);

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

            // Zombie
            SpriteLoader zombieSprite = new SpriteLoader(content, "ZOMBIE", @"spritesheets\ZOMBIE", 32, 32);
            float zombieAnimationSpeed = .3f;
            zombieSprite.AddAnimation("idleSouth1", new[] { 0 }, zombieAnimationSpeed, true);
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

            // Item Merchant
            SpriteLoader itemMerchantSprite = new SpriteLoader(content, "ITEM_MERCHANT", @"spritesheets\ITEM_MERCHANT", 24, 24);
            itemMerchantSprite.AddAnimation("idleSouth1", new[] { 3 }, 0.9f, false);
            SpriteList.Add(itemMerchantSprite);

            // Novice Mage
            SpriteLoader noviceMageSprite = new SpriteLoader(content, "NOVICE_MAGE", @"spritesheets\NOVICE_MAGE", 24, 24);
            noviceMageSprite.AddAnimation("idleSouth1", new[] { 0 }, 0.9f, false);
            noviceMageSprite.AddAnimation("idleNorth1", new[] { 7 }, 2f, true);
            SpriteList.Add(noviceMageSprite);

            // Guard
            SpriteLoader guardSprite = new SpriteLoader(content, "GUARD", @"spritesheets\knight", 24, 24);
            guardSprite.AddAnimation("idleSouth1", new[] { 0 }, 0.9f, false);
            SpriteList.Add(guardSprite);

            // Green Portal
            SpriteLoader greenPortalSprite = new SpriteLoader(content, "GREEN_PORTAL", @"spritesheets\green_portal", 32, 32);
            greenPortalSprite.AddAnimation("idle", new[] { 0, 1, 2, 3, 4 }, 0.09f, true);
            SpriteList.Add(greenPortalSprite);

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

            // Chest
            SpriteLoader chestSprite = new SpriteLoader(content, "CHEST", @"objects\CHEST", 32, 32);
            chestSprite.AddAnimation("Unopened", new[] { 0 }, 1f, false);
            chestSprite.AddAnimation("Opened", new[] { 1 }, 1f, false);
            SpriteList.Add(chestSprite);
            

            // Fireball 1 Icon
            SpriteLoader fireBall1Icon = new SpriteLoader(content, "FIREBALL_1_ICON", @"items\FIREBALL_1_ICON", 16, 16);

            // Icebolt 1 Icon
            SpriteLoader icebolt1Icon = new SpriteLoader(content, "ICEBOLT_1_ICON", @"items\ICEBOLT_1_ICON", 16, 16);

            // Health Potion Icon
            SpriteLoader healthPotionIcon = new SpriteLoader(content, "HEALTH_POTION_ICON", @"items\HEALTH_POTION_ICON", 16, 16);

            // Mana Potion Icon
            SpriteLoader manaPotionIcon = new SpriteLoader(content, "MANA_POTION_ICON", @"items\MANA_POTION_ICON", 16, 16);

            // Gold Icon
            SpriteLoader goldIcon = new SpriteLoader(content, "GOLD_ICON", @"items\GOLD_ICON", 16, 16);

            // Gold Loot
            SpriteLoader goldSprite = new SpriteLoader(content, "GOLD", @"items\GOLD", 16, 16);
            goldSprite.AddAnimation("idle", new[] { 0 }, 0.06f, true);
            SpriteList.Add(goldSprite);


            SpriteLoader bedSprite = new SpriteLoader(content, "BED", @"objects\BED", 24, 32);
            bedSprite.AddAnimation("idle", new[] { 0 }, 1f, true);

            SpriteList.Add(fireBall1Icon);
            SpriteList.Add(icebolt1Icon);
            SpriteList.Add(healthPotionIcon);
            SpriteList.Add(goldIcon);
            SpriteList.Add(manaPotionIcon);
            SpriteList.Add(bedSprite);

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
