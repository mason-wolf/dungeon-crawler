﻿using DungeonCrawler;
using DungeonCrawler.Scenes;
using Humper;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.TextureAtlases;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace DungeonCrawler.Engine
{
    public class MapRenderer
    {
        Texture2D mapTexture;
        TextureAtlas mapAtlas;
        List<Layer> layers;
        List<Tile> firstLayer;
        List<Tile> secondLayer;
        List<MapObject> mapObjects = new List<MapObject>();

        World world;

        int tileWidth = 16;
        int tileHeight = 16;

        public int mapWidth;
        public int mapHeight;

        public int Width()
        {
            return mapWidth;
        }

        public int Height()
        {
            return mapHeight;
        }

        public World GetWorld()
        {
            return world;
        }

        public List<Tile> GetCollisionLayer()
        {
            return secondLayer;
        }

        public List<MapObject> GetMapObjects()
        {
            return mapObjects;
        }

        public void LoadMap(ContentManager content, string filePath)
        {

            layers = new List<Layer>();
  
            // Load map from file.
            using (XmlReader reader = XmlReader.Create(filePath))
            {
                while (reader.Read())
                {
                    if (reader.LocalName == "map")
                    {
                        if (reader.GetAttribute("width") != null && reader.GetAttribute("height") != null)
                        {
                            mapWidth = Int32.Parse(reader.GetAttribute("width"));
                            mapHeight = Int32.Parse(reader.GetAttribute("height"));
                        }
                    }
     
                    // Grab the tileset.
                    if (reader.LocalName == "tileset")
                    {
                        string tilesetFilePath = "tilesets/" + Path.GetFileNameWithoutExtension(reader.GetAttribute("source"));
                        mapTexture = content.Load<Texture2D>(tilesetFilePath);
                        mapAtlas = TextureAtlas.Create(mapTexture, 16, 16);
                    }

                    // Get the the tiles in each layer and add to layers list.
                    if (reader.NodeType == XmlNodeType.Element && reader.LocalName == "layer")
                    {
                        Layer newLayer = new Layer();
                        newLayer.Name = reader.GetAttribute("name");
                        reader.ReadToFollowing("data");
                        string[] tiles = reader.ReadElementContentAsString().Split(',');               
                        newLayer.AddTiles(tiles);
                        layers.Add(newLayer);
                    }

                    int objectId = 0;

                    // Get the object layer.
                    if (reader.NodeType == XmlNodeType.Element && reader.LocalName == "object")
                    {
                        objectId = Int32.Parse(reader.GetAttribute("id"));
                        string objectName = reader.GetAttribute("name");
                        string objectType = reader.GetAttribute("type");
                        float objectPositionX = float.Parse(reader.GetAttribute("x"));
                        float objectPositionY = float.Parse(reader.GetAttribute("y"));

                        MapObject newObject = new MapObject(objectId, objectName, objectType, new Vector2(objectPositionX, objectPositionY));
                        mapObjects.Add(newObject);

                        // Add custom properties
                        List<string> customProperties = new List<string>(0);

                        XmlReader subTree = reader.ReadSubtree();
                        while (subTree.Read())
                        {
                            if (subTree.Name == "property" && subTree.GetAttribute("value") != string.Empty)
                            {
                                customProperties.Add(subTree.GetAttribute("value"));
                            }
                        }
                        mapObjects.Where(mapObject => mapObject.GetId() == objectId).FirstOrDefault().SetCustomProperties(customProperties);
                    }
                }
                reader.Close();
            }


            firstLayer = new List<Tile>();
            secondLayer = new List<Tile>();

            int tileRowCount = mapWidth;
            int tileColumnCount = mapHeight;

            // Create map dimensions.
            for (int rowIndex = 0; rowIndex < tileRowCount; rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < tileColumnCount; columnIndex++)
                {
                    firstLayer.Add(new Tile(new Vector2(columnIndex * tileWidth, rowIndex * tileHeight)));
                    secondLayer.Add(new Tile(new Vector2(columnIndex * tileWidth, rowIndex * tileHeight)));
                }
            }

 
            int count = 0;

            // Assign tile ID for each tile in first layer (walkable).
            foreach (Tile tile in firstLayer)
            {
                tile.TileID = layers[0].Tiles[count];
                count++;
            }

            int secondCount = 0;

            // Get the second layer (collidable).
            foreach (Tile tile in secondLayer)
            {
                tile.TileID = layers[1].Tiles[secondCount];
                secondCount++;
            }        
        }

        public void Draw(SpriteBatch spriteBatch)
        {

            foreach (Tile tile in firstLayer)
            {
                if (tile.TileID != 0)
                {
                    TextureRegion2D region = mapAtlas.GetRegion(tile.TileID - 1);
                    Rectangle sourceRectangle = region.Bounds;
                    Vector2 position = new Vector2((int)Math.Round(tile.Position.X), (int)Math.Round(tile.Position.Y));
                    Rectangle destinationRectangle = new Rectangle((int)position.X, (int)position.Y, region.Width, region.Height);
                    spriteBatch.Draw(region.Texture, destinationRectangle, sourceRectangle, Color.White);
                }
            }

            foreach (Tile tile in secondLayer)
            {
                if (tile.TileID != 0)
                {
                    TextureRegion2D region = mapAtlas.GetRegion(tile.TileID - 1);
                    Rectangle sourceRectangle = region.Bounds;
                    Rectangle destinationRectangle = new Rectangle((int)tile.Position.X, (int)tile.Position.Y, region.Width, region.Height);
                    spriteBatch.Draw(region.Texture, destinationRectangle, sourceRectangle, Color.White);
                }
            }
        }

        public IBox GenerateCollisionWorld()
        {
            world = new World(mapWidth * 16, mapHeight * 16);
            IBox collisionBox = world.Create(0, 0, 16, 16);
            // Find the tiles in the collision layer and add them to the collision world.
            foreach (Tile tile in GetCollisionLayer())
            {
                if (tile.TileID != 0)
                {
                    world.Create(tile.Position.X + 8, tile.Position.Y + 2, 16, 16);
                    tile.Rectangle = new Rectangle((int)tile.Position.X, (int)tile.Position.Y, 16, 16);
                }
            }

            return collisionBox;
        }

        public void SortSprites(SpriteBatch spriteBatch, Entity playerEntity, List<Entity> enemyList)
        {
            foreach (Entity e in enemyList)
            {
                Vector2 AIHealthPosition = new Vector2(e.Position.X - 8, e.Position.Y - 20);

                Vector2 destination = playerEntity.Position - e.Position;
                destination.Normalize();
                Double angle = Math.Atan2(destination.X, destination.Y);
                double direction = Math.Ceiling(angle);


                if (direction == -3 || direction == 4 || direction == -2)
                {
                    playerEntity.Draw(spriteBatch);
                    e.Draw(spriteBatch); ;
                }
                else if (direction == 0 || direction == 1)
                {
                    e.Draw(spriteBatch);
                    playerEntity.Draw(spriteBatch);
                }
                else if (e.CurrentHealth <= 0)
                {
                    e.Draw(spriteBatch);
                    playerEntity.Draw(spriteBatch);
                }
                else
                {
                    playerEntity.Draw(spriteBatch);
                    e.Draw(spriteBatch);
                }

                e.DrawHUD(spriteBatch, AIHealthPosition, false);
            }
            }
        }  
}

