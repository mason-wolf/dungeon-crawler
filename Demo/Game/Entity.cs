﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Animations;
using MonoGame.Extended.Animations.SpriteSheets;
using MonoGame.Extended.Collisions;
using MonoGame.Extended.Shapes;
using MonoGame.Extended.Sprites;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using DungeonCrawler.Scenes;
using Humper;
using Humper.Responses;
using DungeonCrawler.Engine;
using Microsoft.Xna.Framework.Audio;
using DungeonCrawler.Interface;

namespace DungeonCrawler
{
    public enum Action
    {
        IdleSouth1,
        IdleSouth2,
        WalkSouthPattern1,
        WalkSouthPattern2,
        AttackSouthPattern1,
        AttackSouthPattern2,
        WalkWestPattern1,
        WalkWestPattern2,
        AttackWestPattern1,
        AttackWestPattern2,
        IdleWest1,
        IdleWest2,
        WalkEastPattern1,
        WalkEastPattern2,
        AttackEastPattern1,
        AttackEastPattern2,
        IdleEast1,
        IdleEast2,
        WalkNorthPattern1,
        WalkNorthPattern2,
        AttackNorthPattern1,
        AttackNorthPattern2,
        IdleNorth1,
        IdleNorth2,
        Dead
    }

    /// <summary>
    /// Entity class for player, enemy or NPC.
    /// </summary>
    public class Entity : IUpdate
    {
        public AnimatedSprite Sprite;

        private Action state;
        public RectangleF BoundingBox => Sprite.BoundingRectangle;

        public Vector2 Position
        {
            get { return Sprite.Position; }
            set { Sprite.Position = value; }
        }

        // Create textures to display health.
        public Texture2D StatusBarTexture;
        public Texture2D HealthBarTexture;
        public Texture2D StaminaBarTexture;

        public string ID { get; set; }
        public double MaxHealth { get; set; } = 0;
        public double CurrentHealth { get; set; } = 0;

        public double MaxStamina { get; set; } = 0;

        public double CurrentStamina { get; set; } = 0;
        public double AttackDamage { get; set; } = 0;
        public bool Dead { get; set; } = false;
        public bool Aggroed { get; set; } = false;
        public string Name { get; set; }
        public PathFinder PathFinder { get; set; }
        public int WayPointIndex;
        public bool ReachedDestination;

        Entity projectile;
        Vector2 projectilePosition;
        Vector2 projectileStartingPosition;
        bool targetHit = false;
        string projectileDirection = "";
        RectangleF projectileBoundingBox;

        public void LoadContent(ContentManager content)
        {
            StatusBarTexture = content.Load<Texture2D>(@"interface\statusbar");
            HealthBarTexture = content.Load<Texture2D>(@"interface\healthbar");
            StaminaBarTexture = content.Load<Texture2D>(@"interface\staminabar");
        }

        // Create standard animation states for the entity.
        public Action State
        {
            get { return state; }

             set
            {
                if (state != value)
                {
                    state = value;

                    switch (state)
                    {
                        case Action.IdleSouth1:
                            Sprite.Play("idleSouth1");
                            break;
                        case Action.IdleSouth2:
                            Sprite.Play("idleSouth2");
                            break;
                        case Action.WalkSouthPattern1:
                            Sprite.Play("walkSouthPattern1", () => State = Action.IdleSouth1);
                            break;
                        case Action.WalkSouthPattern2:
                            Sprite.Play("walkSouthPattern2", () => State = Action.IdleSouth2);
                            break;
                        case Action.AttackSouthPattern1:
                            Sprite.Play("attackSouthPattern1", () => State = Action.IdleSouth1);
                            break;
                        case Action.AttackSouthPattern2:
                            Sprite.Play("attackSouthPattern2", () => State = Action.IdleSouth2);
                            break;
                        case Action.WalkWestPattern1:
                            Sprite.Play("walkWestPattern1", () => State = Action.IdleWest1);
                            break;
                        case Action.WalkWestPattern2:
                            Sprite.Play("walkWestPattern2", () => State = Action.IdleWest2);
                            break;
                        case Action.AttackWestPattern1:
                            Sprite.Play("attackWestPattern1", () => State = Action.IdleWest1);
                            break;
                        case Action.AttackWestPattern2:
                            Sprite.Play("attackWestPattern2", () => State = Action.IdleWest2);
                            break;
                        case Action.IdleWest1:
                            Sprite.Play("idleWest1");
                            break;
                        case Action.IdleWest2:
                            Sprite.Play("idleWest2");
                            break;
                        case Action.WalkEastPattern1:
                            Sprite.Play("walkEastPattern1", () => State = Action.IdleEast1);
                            break;
                        case Action.WalkEastPattern2:
                            Sprite.Play("walkEastPattern2", () => State = Action.IdleEast2);
                            break;
                        case Action.AttackEastPattern1:
                            Sprite.Play("attackEastPattern1", () => State = Action.IdleEast1);
                            break;
                        case Action.AttackEastPattern2:
                            Sprite.Play("attackEastPattern2", () => State = Action.IdleEast2);
                            break;
                        case Action.IdleEast1:
                            Sprite.Play("idleEast1");
                            break;
                        case Action.IdleEast2:
                            Sprite.Play("idleEast2");
                            break;
                        case Action.WalkNorthPattern1:
                            Sprite.Play("walkNorthPattern1", () => State = Action.IdleNorth1);
                            break;
                        case Action.WalkNorthPattern2:
                            Sprite.Play("walkNorthPattern2", () => State = Action.IdleNorth2);
                            break;
                        case Action.AttackNorthPattern1:
                            Sprite.Play("attackNorthPattern1", () => State = Action.IdleNorth1);
                            break;
                        case Action.AttackNorthPattern2:
                            Sprite.Play("attackNorthPattern2", () => State = Action.IdleNorth2);
                            break;
                        case Action.IdleNorth1:
                            Sprite.Play("idleNorth1");
                            break;
                        case Action.IdleNorth2:
                            Sprite.Play("idleNorth2");
                            break;
                        case Action.Dead:
                            Sprite.Play("dead");
                            break;
                    }
                }
            }
        }
  
        public Entity()
        {
        }

        /// <summary>
        /// Creates a new entity. Can be an NPC, object or enemy. Has health, stamina, basic movement patterns and damage thresholds.
        /// </summary>
        /// <param name="animations">Animated sprite for entity</param>
        public Entity(SpriteSheetAnimationFactory animations)
        {
            Sprite = new AnimatedSprite(animations);
        }

        // Method to make an entity follow a path of waypoints.
        public void FollowPath(GameTime gameTime, Entity entity, List<Vector2> DestinationWaypoint, float Speed)
        {
            if (DestinationWaypoint.Count > 0)
            {
                if (!ReachedDestination)
                {
                    Vector2 Direction = DestinationWaypoint[WayPointIndex] - entity.Position;
                    Direction.Normalize();
                    Double angle = Math.Atan2(Direction.X, Direction.Y);
                    double rotation = (float)(angle * (180 / Math.PI));

                    if (rotation < -179 || rotation == 180)
                    {
                        entity.State = Action.WalkNorthPattern1;
                    }
                    else if (rotation >= 89 && rotation < 180)
                    {
                        entity.State = Action.WalkEastPattern1;
                    }
                    else if (rotation <= -90 && rotation > -179)
                    {

                        entity.State = Action.WalkWestPattern1;
                    }
                    else if (rotation >= 0 && rotation <= 1)
                    {
                        entity.State = Action.WalkSouthPattern1;
                    }

                    if (rotation < -0 && rotation > -90)
                    {
                        entity.State = Action.WalkSouthPattern1;
                    }

                    float Distance = Vector2.Distance(entity.Position, DestinationWaypoint[WayPointIndex]);

                    if (Distance > Direction.Length())
                        entity.Position += Direction * (float)(Speed * gameTime.ElapsedGameTime.TotalMilliseconds);
                    else
                    {
                        if (WayPointIndex >= DestinationWaypoint.Count - 1)
                        {
                            entity.Position += Direction;
                            ReachedDestination = true;
                        }
                            if(WayPointIndex < 3)
                            WayPointIndex++;
                    }
                }
            }
        }


        public void Attack(Entity target)
        {

            Vector2 currentPosition = Position;

            float distance = Vector2.Distance(Position, target.Position);

            if (distance < 20 && CurrentHealth > 0)
            {

                Vector2 destination = Position - target.Position;
                destination.Normalize();
                Double angle = Math.Atan2(destination.X, destination.Y);
                double direction = Math.Ceiling(angle);

                if (direction == -3 || direction == 4 || direction == -2)
                {
                    State = Action.AttackSouthPattern1;
                    target.CurrentHealth -= AttackDamage;
                }

                if (direction == -1)
                {
                    State = Action.AttackEastPattern1;
                    target.CurrentHealth -= AttackDamage;
                }

                if (direction == 0 || direction == 1)
                {
                    State = Action.AttackNorthPattern1;
                    target.CurrentHealth -= AttackDamage;
                }

                if (direction == 2 || direction == 3)
                {
                    State = Action.AttackWestPattern1;
                    target.CurrentHealth -= AttackDamage;
                }
            }
        }

        /// <summary>
        /// Shoots a projectile.
        /// </summary>
        /// <param name="sprite">AnimatedSprite of the projectile.</param>
        /// <param name="direction">Direction (North, South, East, West)</param>
        public void ShootProjectile(AnimatedSprite sprite, string direction)
        {
            projectile = new Entity();
            projectile.Sprite = sprite;
            projectilePosition = new Vector2(Init.Player.Position.X, Init.Player.Position.Y);
            projectile.Position = projectilePosition;
            projectileStartingPosition = projectilePosition;
            projectile.MaxHealth = 10;
            projectile.CurrentHealth = 10;
            this.projectileDirection = direction;
            targetHit = false;
        }

        /// <summary>
        /// Checks to see if the projectile intersects any of the collision tiles on the current map.
        /// </summary>
        /// <param name="projectile"></param>
        /// <returns></returns>
        bool ProjectileCollision(Entity projectile)
        {
            bool collided = false;
            Vector2 offsetPosition = new Vector2(projectilePosition.X + 5, projectilePosition.Y + 5);
            projectileBoundingBox = new RectangleF(offsetPosition, new SizeF(2, 2));
            foreach (Tile tile in Init.SelectedMap.GetCollisionTiles())
            {
                if (projectileBoundingBox.Intersects(tile.Rectangle))
                {
                    collided = true;
                }
            }
            return collided;
        }

        public void Update(GameTime gameTime)
        {
            if (projectile != null && !ProjectileCollision(projectile))
            {
                int speed = 7;

                if (projectileDirection == "north")
                {
                    projectile.State = Action.IdleNorth1;
                    projectilePosition.Y -= speed;
                    projectile.Position = projectilePosition;
                }

                if (projectileDirection == "south")
                {
                    projectile.State = Action.AttackSouthPattern1;
                    projectilePosition.Y += speed;
                    projectile.Position = projectilePosition;
                }

                if (projectileDirection == "east")
                {
                    ProjectileCollision(projectile);
                    projectile.State = Action.IdleEast1;
                    projectilePosition.X += speed;
                    projectile.Position = projectilePosition;
                }

                if (projectileDirection == "west")
                {
                    projectile.State = Action.IdleWest1;
                    projectilePosition.X -= speed;
                    projectile.Position = projectilePosition;
                }

                if (Init.Player.EnemyList != null)
                {
                    foreach (Entity entity in Init.Player.EnemyList)
                    {
                        if (projectile.BoundingBox.Intersects(entity.BoundingBox) && targetHit == false && entity.state != Action.Dead)
                        {
                            entity.CurrentHealth -= 4;
                            entity.Aggroed = true;
                            targetHit = true;
                        }
                    }

                    projectile.Update(gameTime);
                }
            }

            Sprite.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Sprite);

            if (projectile != null && targetHit == false)
            {
                spriteBatch.Draw(projectile.Sprite);
            }
        }

        /// <summary>
        /// Draws HUD depending on entity type.
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch</param>
        /// <param name="position">Entity's position</param>
        /// <param name="isPlayer">Flag for player</param>

        public void DrawHUD(SpriteBatch spriteBatch, Vector2 position, bool isPlayer)
        {
            if (isPlayer)
            {
                if (CurrentHealth > 0)
                {
                    // Draw health bar
                    spriteBatch.Draw(StatusBarTexture, position, new Rectangle(0, 0, Convert.ToInt32(MaxHealth), 4), Color.Black);
                    spriteBatch.Draw(HealthBarTexture, position, new Rectangle(10, 10, Convert.ToInt32(CurrentHealth), 4), Color.White);
                    // Draw stamina bar
                    Vector2 staminaPosition = new Vector2(position.X, position.Y + 6);
                    spriteBatch.Draw(StatusBarTexture, staminaPosition, new Rectangle(0, 0, Convert.ToInt32(MaxStamina), 4), Color.Black);
                    spriteBatch.Draw(StaminaBarTexture, staminaPosition, new Rectangle(10, 10, Convert.ToInt32(CurrentStamina), 4), Color.White);
                }
            }
            else
            {
                if (CurrentHealth > 0)
                {
                    spriteBatch.Draw(StatusBarTexture, position, new Rectangle(0, 0, Convert.ToInt32(MaxHealth), 2), Color.Black);
                    spriteBatch.Draw(StaminaBarTexture, position, new Rectangle(10, 10, Convert.ToInt32(CurrentHealth), 2), Color.White);
                }
            }
        }

        public void OnCollision(CollisionInfo collisionInfo)
        {
            throw new NotImplementedException();
        }
    }
}