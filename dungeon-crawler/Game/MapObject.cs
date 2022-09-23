﻿
using DungeonCrawler.Interface;
using DungeonCrawler.Scenes;
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

namespace DungeonCrawler
{
    public class MapObject : Scene
    {
        int id;
        string name;
        string type;
        GameTime gameTime;
        Vector2 position;
        Rectangle objectBoundingBox;
        AnimatedSprite animatedSprite;
        Item containedItem;
        Rectangle containedItemBoundingBox;
        bool destroyed = false;
        bool itemPickedUp = false;
        List<string> customProperties;

        IBox collisionBox;

        public MapObject(int objectId, string objectName, string objectType, Vector2 position)
        {
            this.id = objectId;
            this.name = objectName;
            this.type = objectType;
            this.position = position;
            this.objectBoundingBox = new Rectangle((int)position.X - 5, (int)position.Y - 5, 10, 10);
        }

        public MapObject() { }
        public int GetId()
        {
            return id;
        }
        public string GetName()
        {
            return name;
        }

        public string GetObjectType()
        {
            return type;
        }

        public Vector2 GetPosition()
        {
            return position;
        }

        public Rectangle GetBoundingBox()
        {
            return objectBoundingBox;
        }

        public void SetSprite(AnimatedSprite animatedSprite)
        {
            this.animatedSprite = animatedSprite;
        }

        public AnimatedSprite GetSprite()
        {
            return animatedSprite;
        }

        public void SetId(int id)
        {
            this.id = id;
        }

        public void SetContainedItem(Item item)
        {
            this.containedItem = item;
        }

        public void SetCollisionBox(IBox collisionBox)
        {
            this.collisionBox = collisionBox;
        }

        public void SetCustomProperties(List<string> customProperties)
        {
            this.customProperties = customProperties;
        }

        public List<string> GetCustomProperties()
        {
            return customProperties;
        }

        public IBox GetCollisionBox()
        {
            return collisionBox;
        }

        public bool IsDestroyed()
        {
            return destroyed;
        }

        public void PickUpItem()
        {
            itemPickedUp = true;
        }
        public bool ItemPickedUp()
        {
            return itemPickedUp;
        }

        public void Destroy()
        {
            destroyed = true;
        }

        public override void LoadContent(ContentManager content)
        {
            throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {
            this.gameTime = gameTime;

            if (animatedSprite != null)
            {
                animatedSprite.Update(gameTime);
            }

            // If the player picks up an item, add it to the inventory.
            if (containedItem != null && Init.Player.BoundingBox.Intersects(containedItemBoundingBox) && !itemPickedUp)
            {
                switch (containedItem.Name)
                {
                    //case "Chicken":
                    //    Inventory.TotalChickens = Inventory.TotalChickens + 1;
                    //    break;
                    //case "Arrow":
                    //    Inventory.TotalArrows = Inventory.TotalArrows + 5;
                    //    break;
                }

                itemPickedUp = true;
            }
        }

        bool itemDrawn = false;

        public override void Draw(SpriteBatch spriteBatch)
        {

            if (animatedSprite != null)
            {
                animatedSprite.Draw(spriteBatch);
            }

            // If the object is destroyed, drop an item. (Draw and create a bounding box).
            if (destroyed && !itemPickedUp)
            {
                itemDrawn = true;

                int x = 0;
                int y = 0;

                // Spawn the item to the left or right of the container depending on the direction the player intersects.
                if (position.X < Init.Player.Position.X && itemDrawn)
                {
                    x = (int)position.X - 5;
                    y = (int)position.Y;
                }
                else if (position.X > Init.Player.Position.X && itemDrawn)
                {
                    x = (int)position.X + 3;
                    y = (int)position.Y;
                }

                if (containedItem.ItemTexture != null)
                {
                    spriteBatch.Draw(containedItem.ItemTexture, new Rectangle(x, y, containedItem.Width, containedItem.Height), Color.White);
                }
                containedItemBoundingBox = new Rectangle(x, y, 1, 1);
            }
        }
    }
}
