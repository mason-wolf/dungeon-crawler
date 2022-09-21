using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Animations.SpriteSheets;
using MonoGame.Extended.TextureAtlases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Game
{
    class SpriteLoader
    {
        public string Name { get; set; }
        private Texture2D Texture;
        private TextureAtlas Atlas;
        private SpriteSheetAnimationFactory AnimationFactory;

        public SpriteSheetAnimationFactory GetAnimation()
        {
            return AnimationFactory;
        }

        public Texture2D GetTexture()
        {
            return Texture;
        }

        /// <summary>
        /// Creates a new spritesheet animation.
        /// </summary>
        /// <param name="content">ContentManager instance</param>
        /// <param name="name">Name of animation</param>
        /// <param name="path">Path of animation</param>
        /// <param name="width">Frame Width</param>
        /// <param name="height">Frame Height</param>
        public SpriteLoader(ContentManager content, string name, string path, int width, int height)
        {
            Name = name;
            Texture = content.Load<Texture2D>(path);
            Atlas = TextureAtlas.Create(Texture, width, height);
            AnimationFactory = new SpriteSheetAnimationFactory(Atlas);
        }

        public void AddAnimation(string animationName, int[] frames, float animationSpeed, bool isLooping)
        {
            AnimationFactory.Add(animationName, new SpriteSheetAnimationData(frames, animationSpeed, isLooping));
        }
    }
}
