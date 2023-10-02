using DungeonCrawler;
using Humper;
using Microsoft.Xna.Framework.Content;
using MonoGame.Extended.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Game
{
    public class Portal
    {
        public string id;
        IBox portalCollidable;
        Entity portalEntity;
        public MapObject MapObject;
        /// <summary>
        /// Creates a portal on a map.
        /// </summary>
        /// <param name="map">Map to add portal to.</param>
        /// <param name="mapObject">Map object associated with portal.</param>
        /// <param name="contentManager">Content manager instance.</param>
        public Portal(Map map, MapObject mapObject, ContentManager contentManager)
        {
            this.MapObject = mapObject;
            AnimatedSprite portalSprite = new AnimatedSprite(Sprites.GetSprite("PORTAL"));
            portalSprite.Play("idleSouth1");
            portalSprite.Position = mapObject.GetPosition();
            mapObject.SetSprite(portalSprite);
            portalCollidable = map.GetWorld().Create(portalSprite.Position.X, portalSprite.Position.Y, 32, 32);
            mapObject.SetCollisionBox(portalCollidable);
            portalEntity = new Entity(Sprites.GetSprite("PORTAL"));
            portalEntity.ID = mapObject.GetId().ToString();
            id = portalEntity.ID;
            portalEntity.LoadContent(contentManager);
            portalEntity.State = DungeonCrawler.Action.IdleSouth1;
            portalEntity.Movable = false;
            portalEntity.MaxHealth = 10;
            portalEntity.CurrentHealth = 10;
            portalEntity.Position = mapObject.GetPosition();
            portalEntity.Name = "PORTAL";
        }

        /// <summary>
        /// Returns entity associated with this portal.
        /// </summary>
        /// <returns>Entity</returns>
        public Entity GetEntity()
        {
            return portalEntity;
        }

        public IBox GetCollisionBoundaries()
        {
            return this.portalCollidable;
        }
    }
}


