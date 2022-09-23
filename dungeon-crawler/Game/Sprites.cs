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
            fireProfessorSprite.AddAnimation("idle", new[] { 0 }, 0.9f, false);
            SpriteList.Add(fireProfessorSprite);

            // Green Portal
            SpriteLoader greenPortalSprite = new SpriteLoader(content, "GREEN_PORTAL", @"spritesheets\green_portal", 32, 32);
            greenPortalSprite.AddAnimation("idle", new[] { 0, 1, 2, 3, 4 }, 0.09f, true);
            SpriteList.Add(greenPortalSprite);

            // Bookshelf
            SpriteLoader bookshelfSprite = new SpriteLoader(content, "BOOKSHELF", @"objects\bookshelf", 24, 24);
            bookshelfSprite.AddAnimation("idle", new[] { 0 }, 0.09f, true);
            SpriteList.Add(bookshelfSprite);

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

            // Bed
            //SpriteLoader bookshelfSprite = new SpriteLoader(content, "BOOKSHELF", @"objects\bookshelf", 24, 24);
            //bookshelfSprite.AddAnimation("idle", new[] { 0 }, 0.09f, true);
            //SpriteList.Add(bookshelfSprite);
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

            return foundSprite;
        }
    }
}
