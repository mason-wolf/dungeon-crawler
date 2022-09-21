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
            //skeletonTexture = content.Load<Texture2D>(@"spritesheets\Skeleton");
            //skeletonAtlas = TextureAtlas.Create(skeletonTexture, 24, 24);
            //skeletonAnimation = new SpriteSheetAnimationFactory(skeletonAtlas);
            //float skeletonAnimationSpeed = .3f;
            //skeletonAnimation.Add("idleSouth1", new SpriteSheetAnimationData(new[] { 1 }, skeletonAnimationSpeed, isLooping: true));
            //skeletonAnimation.Add("walkSouthPattern1", new SpriteSheetAnimationData(new[] { 1, 2, 3, 2 }, skeletonAnimationSpeed, isLooping: true));
            //skeletonAnimation.Add("attackSouthPattern1", new SpriteSheetAnimationData(new[] { 1, 2, 3, 2 }, skeletonAnimationSpeed, isLooping: true));
            //skeletonAnimation.Add("walkWestPattern1", new SpriteSheetAnimationData(new[] { 1, 2, 3, 2 }, skeletonAnimationSpeed, isLooping: true));
            //skeletonAnimation.Add("attackWestPattern1", new SpriteSheetAnimationData(new[] { 1, 2, 3, 2 }, skeletonAnimationSpeed, isLooping: true));
            //skeletonAnimation.Add("idleWest1", new SpriteSheetAnimationData(new[] { 1 }));
            //skeletonAnimation.Add("walkEastPattern1", new SpriteSheetAnimationData(new[] { 1, 2, 3, 2 }, skeletonAnimationSpeed, isLooping: true));
            //skeletonAnimation.Add("attackEastPattern1", new SpriteSheetAnimationData(new[] { 1, 2, 3, 2 }, skeletonAnimationSpeed, isLooping: true));
            //skeletonAnimation.Add("idleEast1", new SpriteSheetAnimationData(new[] { 1 }));
            //skeletonAnimation.Add("walkNorthPattern1", new SpriteSheetAnimationData(new[] { 1, 2, 3, 2 }, skeletonAnimationSpeed, isLooping: true));
            //skeletonAnimation.Add("attackNorthPattern1", new SpriteSheetAnimationData(new[] { 1, 2, 3, 2 }, skeletonAnimationSpeed, isLooping: true));
            //skeletonAnimation.Add("idleNorth1", new SpriteSheetAnimationData(new[] { 1 }));
            //skeletonAnimation.Add("dead", new SpriteSheetAnimationData(new[] { 4 }, .2f, isLooping: false));

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

            SpriteList.Add(fireBall1Icon);
            SpriteList.Add(icebolt1Icon);
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
