using System;
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
using Demo.Game;
using static Demo.Game.Spell;
using System.Diagnostics;

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
        public Texture2D NPCStatusBarTexture;
        public Texture2D HealthBarTexture;
        public Texture2D StaminaBarTexture;
        public Texture2D ManaBarTexture;
        public Texture2D ActionBarTexture;

        public string ID { get; set; }
        public double MaxHealth { get; set; } = 0;
        public double CurrentHealth { get; set; } = 0;

        public double HealthBonus { get; set; }
        public double ManaBonus { get; set; }
        public double MaxStamina { get; set; } = 0;
        public double CurrentStamina { get; set; } = 0;

        public double MaxMana { get; set; } = 0;
        public double CurrentMana { get; set; } = 0;
        public float Distance { get; set; }
        public double AttackDamage { get; set; } = 0;
        public bool Dead { get; set; } = false;
        public bool Aggroed { get; set; } = false;
        public string Name { get; set; }
        public double XP { get; set;  }
        public int Level { get; set; }
        public double XPRemaining { get; set; }
        public PathFinder PathFinder { get; set; }
        public Spell Buff { get; set; }

        public Equipment Equipment { get; set; } = new Equipment();

        public double FrostResistance { get; set; }
        public double FireResistance { get; set; }
        public double ThunderResistance { get; set; }

        public int WayPointIndex;

        public bool SpellCaster { get; set; } = false;
        public bool Movable { get; set; } = true;
        public int SpellID { get; set; } = 0;

        public int SpellPower { get; set; } = 0;
        public bool ReachedDestination;

        public bool Moving;

        // Controls the spawn rate of the entity by %.
        public int SpawnRate { get; set; } = 100;

        public List<Projectile> Projectiles = new List<Projectile>();

        public void LoadContent(ContentManager content)
        {
            StatusBarTexture = content.Load<Texture2D>(@"interface\statusbar");
            NPCStatusBarTexture = content.Load<Texture2D>(@"interface\npc-statusbar");
            HealthBarTexture = content.Load<Texture2D>(@"interface\healthbar");
            StaminaBarTexture = content.Load<Texture2D>(@"interface\staminabar");
            ManaBarTexture = content.Load<Texture2D>(@"interface\MANA_BAR");
            ActionBarTexture = content.Load<Texture2D>(@"interface\ACTIONBAR");
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
            if (DestinationWaypoint.Count > 0 && WayPointIndex < DestinationWaypoint.Count && Movable)
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
                    else if (rotation < -0 && rotation > -90)
                    {
                        entity.State = Action.WalkSouthPattern1;
                    }
                    else
                    {
                        entity.State = Action.WalkSouthPattern1;
                    }

                    float Distance = Vector2.Distance(entity.Position, DestinationWaypoint[WayPointIndex]);
 
                    if (Distance > Direction.Length())
                    {
                        entity.Position += Direction * (float)(Speed * 20f);
                    }
                    else
                    {
                        if (WayPointIndex >= DestinationWaypoint.Count)
                        {
                            entity.Position += Direction;
                            ReachedDestination = true;
                            entity.Moving = false;
                        }
                            WayPointIndex++;
                    }
                }
            }
        }

        Stopwatch attackTimer = new Stopwatch();
        public void Attack(Entity target)
        {
            float distance = Vector2.Distance(Position, target.Position);

            Vector2 destination = Position - target.Position;
            destination.Normalize();
            Double angle = Math.Atan2(destination.X, destination.Y);
            double direction = Math.Ceiling(angle);
            attackTimer.Start();

            // Cast spell at target
            if (SpellCaster && distance < 200 & CurrentHealth > 0 && attackTimer.ElapsedMilliseconds > 1500 && Movable)
            {
                if (direction == -3 || direction == 4 || direction == -2)
                {
                    CastSpell(SpellDirection.SOUTH, SpellID);
                }

                if (direction == -1)
                {
                    CastSpell(SpellDirection.EAST, SpellID);
                }

                if (direction == 0 || direction == 1)
                {
                    CastSpell(SpellDirection.NORTH, SpellID);
                }

                if (direction == 2 || direction == SpellID)
                {
                    CastSpell(SpellDirection.WEST, SpellID);
                }
                attackTimer.Restart();
            }
            else
            {
                // Attack target physically
                if (distance < 20 && CurrentHealth > 0)
                {
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
        }

        Stopwatch stopWatch = new Stopwatch();
        public void CastSpell(SpellDirection direction, int spellId)
        {
            Spell spell = (Spell)Items.GetItemById(spellId);

            if (spell != null && spell.ManaCost <= CurrentMana)
            {
                spell.Direction = direction;
                switch (spell.ID)
                {
                    case (1):
                        spell.Sprite = new AnimatedSprite(Sprites.GetSprite("FIREBALL_1"));
                        ShootProjectile(spell, default);
                        break;
                    // Spread Spell
                    case (2):
                        // Place the spell a little further from the player.
                        int xOffset = 30;
                        int yOffset = 15;

                        //Positions starting from left to right.
                        List<Vector2> spellSpreadLocations = new List<Vector2>();
                        Vector2 pos1 = new Vector2(Position.X - xOffset, Position.Y + yOffset);
                        spellSpreadLocations.Add(pos1);
                        Vector2 pos2 = new Vector2(Position.X - (xOffset / 2), Position.Y + yOffset);
                        spellSpreadLocations.Add(pos2);
                        Vector2 pos3 = new Vector2(Position.X + (xOffset / 2), Position.Y + yOffset);
                        spellSpreadLocations.Add(pos3);
                        Vector2 pos4 = new Vector2(Position.X + xOffset, Position.Y + yOffset);
                        spellSpreadLocations.Add(pos4);

                        List<Spell> spreadSpells = new List<Spell>();
                        foreach (Vector2 position in spellSpreadLocations)
                        {
                            // Generate copies of the spell to cast.
                            Spell spreadSpell = Copy(spell);
                            spreadSpell.Position = position;
                            spreadSpell.Sprite = new AnimatedSprite(Sprites.GetSprite("ICEBOLT_1"));
                            spreadSpells.Add(spreadSpell);
                        }
                        CastSpreadSpell(spreadSpells);
                        break;
                    case (5):
                        spell.Sprite = new AnimatedSprite(Sprites.GetSprite("THUNDERBOLT_1"));
                        ShootProjectile(spell, default);
                        break;
                    case (6):
                        stopWatch.Restart();
                        Buff = spell;
                        Buff.Sprite = new AnimatedSprite(Sprites.GetSprite("HEAL_1"));
                        Buff.Sprite.Position = Position;
                        Buff.Duration = 800;
                        Buff.Sprite.Play("idle");
                        RestoreHealth(15);
                        break;
                    case (12):
                        stopWatch.Restart();
                        Buff = spell;
                        Buff.Sprite = new AnimatedSprite(Sprites.GetSprite("FLAME_SHIELD"));
                       break;
                }

                CurrentMana -= spell.ManaCost;
            }
        }

        /// <summary>
        /// Casts a spread spell. Spread spells handle projectile
        /// positions differently than single projectiles. 
        /// </summary>
        /// <param name="spells">List of spells to cast.</param>
        public void CastSpreadSpell(List<Spell> spells)
        {
            foreach(Spell spell in spells)
            {
                Projectile projectile = CreateProjectile(spell);
                projectile.Sprite = spell.Sprite;
                switch (State)
                {
                    // Positions are normal for north & south directions.
                    // Need custom positions for east and west.
                    case (Action.AttackEastPattern1):
                        spells[0].Position = new Vector2(spells[0].Position.X + 55, spells[0].Position.Y + 25);
                        spells[1].Position = new Vector2(spells[1].Position.X + 20, spells[1].Position.Y + 5);
                        spells[2].Position = new Vector2(spells[2].Position.X + 4, spells[2].Position.Y + -5);
                        spells[3].Position = new Vector2(spells[3].Position.X - 1, spells[3].Position.Y + -8);
                        break;
                    case (Action.AttackWestPattern1):
                        spells[0].Position = new Vector2(spells[0].Position.X - 1, spells[0].Position.Y - 25);
                        spells[1].Position = new Vector2(spells[1].Position.X - 8, spells[1].Position.Y - 5);
                        spells[2].Position = new Vector2(spells[2].Position.X - 15, spells[2].Position.Y + 5);
                        spells[3].Position = new Vector2(spells[3].Position.X - 15, spells[3].Position.Y - -8);
                        break;
                }
                projectile.Position = spell.Position;
                projectile.Direction = spell.Direction.ToString();
                projectile.TargetHit = false;
                QueueProjectile(projectile);
            }
        }

        /// <summary>
        /// Applies equipment bonuses to armor slots.
        /// </summary>
        public void ApplyArmorStats()
        {
            Dictionary<string, double> bonuses = Equipment.GetBonuses();
            FrostResistance = 0;
            FrostResistance = 0;
            FireResistance = 0;
            ThunderResistance = 0;

            FrostResistance = bonuses["FROST_RESISTANCE"];
            FireResistance = bonuses["FIRE_RESISTANCE"];
            ThunderResistance = bonuses["THUNDER_RESISTANCE"];

            HealthBonus = 0;
            HealthBonus += bonuses["HEALTH_BONUS"];

            ManaBonus = 0;
            ManaBonus += bonuses["MANA_BONUS"];
        }

        /// <summary>
        /// Creates a projectile from a spell.
        /// </summary>
        /// <param name="spell">Spell to make a projectile with.</param>
        /// <returns></returns>
        public Projectile CreateProjectile(Spell spell)
        {
            Projectile projectile = new Projectile();
            Spell spellToCast = new Spell();
            // Copy the spell data. Could use IClonable but this
            // will work for now.
            spellToCast.ID = spell.ID;
            spellToCast.Sprite = spell.Sprite;
            spellToCast.Damage = (spell.Damage * SpellPower) / 10;
            spellToCast.BoundingBox = spell.BoundingBox;
            spellToCast.Direction = spell.Direction;
            spellToCast.Element = spell.Element;
            projectile.ID = spell.ID;
            projectile.Spell = spellToCast;
            return projectile;
        }

        /// <summary>
        /// Shoots a projectile.
        /// </summary>
        /// <param name="spell">Name of spell</param>
        /// <param name="startingPosition">Projectile position</param>
        public void ShootProjectile(Spell spell, Vector2 projectilePosition)
        {
            Projectile projectile = CreateProjectile(spell);
            // Thunderbolt spell requires two separate spritesheets. 
            // Spell ID is 5 so change sprite depending on direction.
            // TODO: Decouple custom spell logic in ShootProjectile() method on Entity class.
            if (projectile.ID == 5 && spell.Direction == SpellDirection.NORTH || projectile.ID == 5 && spell.Direction == SpellDirection.SOUTH)
            {
                projectile.Sprite = new AnimatedSprite(Sprites.GetSprite("THUNDERBOLT_1_NORTH_SOUTH"));
            }
            else
            {
                projectile.Sprite = spell.Sprite;
            }

            switch (State)
            {
                case (Action.AttackEastPattern1):
                    projectilePosition = new Vector2(projectilePosition.X + 15, projectilePosition.Y + 5);
                    break;
                case (Action.AttackWestPattern1):
                    projectilePosition = new Vector2(projectilePosition.X - 15, projectilePosition.Y + 5);
                    break;
                case (Action.AttackNorthPattern1):
                    projectilePosition = new Vector2(projectilePosition.X, projectilePosition.Y - 15);
                    break;
                case (Action.AttackSouthPattern1):
                    projectilePosition = new Vector2(projectilePosition.X, projectilePosition.Y + 15);
                    break;
            }
            projectile.Position = projectilePosition;
            projectile.Direction = spell.Direction.ToString();
            projectile.TargetHit = false;
            QueueProjectile(projectile);
        }

        /// <summary>
        /// Adds projectile to the list to be fired.
        /// Throttles the list for performance.
        /// </summary>
        /// <param name="projectile"></param>
        public void QueueProjectile(Projectile projectile)
        {
            Projectiles.Add(projectile);

            if (Projectiles.Count > 50)
            {
                Projectiles.Clear();
            }
        }

        /// <summary>
        /// Checks to see if the projectile intersects any of the collision tiles on the current map.
        /// </summary>
        /// <param name="projectile"></param>
        /// <returns></returns>
        bool ProjectileCollision(Projectile projectile)
        {
            bool collided = false;
            Vector2 offsetPosition = new Vector2(projectile.Position.X + 5, projectile.Position.Y + 5);
            projectile.HitBox = new RectangleF(offsetPosition, new SizeF(2, 2));
            foreach (Tile tile in Init.SelectedMap.GetCollisionTiles())
            {
                if (projectile.HitBox.Intersects(tile.Rectangle))
                {
                    collided = true;
                    // Toggle false to show when projectile hits obstacle.
                    projectile.TargetHit = true;
                }
            }
            return collided;
        }

        /// <summary>
        /// Restores health.
        /// </summary>
        /// <param name="healAmount">Amount to heal</param>
        public void RestoreHealth(double healAmount)
        {
            double healthToHeal = (MaxHealth + HealthBonus) - CurrentHealth;
            if (healthToHeal >= healAmount)
            {
                CurrentHealth += healAmount;
            }
            else
            {
                CurrentHealth = (MaxHealth + HealthBonus);
            }
        }

        /// <summary>
        /// Restores mana.
        /// </summary>
        /// <param name="manaAmount">Amount to restore</param>
        public void RestoreMana(double manaAmount)
        {
            double manaToHeal = (MaxMana + ManaBonus) - CurrentMana;

            if (manaToHeal >= manaAmount)
            {
                CurrentMana += manaAmount;
            }
            else
            {
                CurrentMana = (MaxMana + ManaBonus);
            }
        }
    
        public void Update(GameTime gameTime)
        {
            if (Buff != null)
            {
                if (stopWatch.ElapsedMilliseconds < Buff.Duration)
                {
                    // Flame shield
                    if (Buff.ID == 12 && State != Action.Dead)
                    {
                        float time = stopWatch.ElapsedMilliseconds / 75;
                        float speed = MathHelper.PiOver4;
                        float radius = 25.0f;
                        Buff.BoundingBox = new RectangleF(Buff.Sprite.Position.X, Buff.Sprite.Position.Y, 16, 16);
                        Vector2 origin = Position;
                        Buff.Sprite.Position =
                            new Vector2((float)Math.Cos(time * speed) * radius + origin.X,
                            (float)Math.Sin(time * speed) * radius + origin.Y
                            );
                    }
                    else
                    {
                        Buff.Sprite.Position = Position;
                        Buff.Sprite.Update(gameTime);
                    }
                }
                else
                {
                    Buff = null;
                    stopWatch.Stop();
                }
            }

            if (Init.Player.EnemyList != null)
            {
                foreach (Entity enemy in Init.Player.EnemyList)
                {
                    if (Buff != null && Buff.BoundingBox.Intersects(enemy.BoundingBox) && enemy.state != Action.Dead)
                    {
                        enemy.CurrentHealth -= Buff.Damage;
                        enemy.Aggroed = true;
                    }
                }
            }

            foreach (Projectile projectile in Projectiles)
            {
                if (projectile != null && !ProjectileCollision(projectile))
                {
                    int speed = 4;

                    if (SpellCaster)
                    {
                        speed = 1;
                    }

                    if (projectile.Direction == "NORTH")
                    {
                        projectile.State = Action.IdleNorth1;
                        float y = projectile.Position.Y;
                        y -= speed;
                        Vector2 newPosition = new Vector2(projectile.Position.X, y);
                        projectile.Position = newPosition;
                    }

                    if (projectile.Direction == "SOUTH")
                    {
                        projectile.State = Action.AttackSouthPattern1;
                        float y = projectile.Position.Y;
                        y += speed;
                        Vector2 newPosition = new Vector2(projectile.Position.X, y);
                        projectile.Position = newPosition;
                    }

                    if (projectile.Direction == "EAST")
                    {
                        ProjectileCollision(projectile);
                        projectile.State = Action.AttackEastPattern1;
                        float x = projectile.Position.X;
                        x += speed;
                        Vector2 newPosition = new Vector2(x, projectile.Position.Y);
                        projectile.Position = newPosition;
                    }

                    if (projectile.Direction == "WEST")
                    {
                        projectile.State = Action.IdleWest1;
                        float x = projectile.Position.X;
                        x -= speed;
                        Vector2 newPosition = new Vector2(x, projectile.Position.Y);
                        projectile.Position = newPosition;
                    }

                    if (Init.Player.EnemyList != null)
                    {
                        foreach (Entity entity in Init.Player.EnemyList)
                        {
                            // If player casted projectile, damage enemy.
                            if (projectile.BoundingBox.Intersects(entity.BoundingBox) && projectile.TargetHit == false && entity.state != Action.Dead && !SpellCaster)
                            {
                                entity.CurrentHealth -= projectile.Spell.DamageResistance(entity);
                                entity.Aggroed = true;
                                projectile.TargetHit = true;
                            }
                        }

                        // If enemy casted projectile, damage player.
                        if (SpellCaster && projectile.BoundingBox.Intersects(Init.Player.BoundingBox) && !projectile.TargetHit)
                        {
                            Init.Player.CurrentHealth -= projectile.Spell.DamageResistance(Init.Player);
                            projectile.TargetHit = true;
                        }
                        projectile.Update(gameTime);
                    }
                }
            }
            Sprite.Update(gameTime);
        }

  
        public void Draw(SpriteBatch spriteBatch)
        {

            if (Movable)
            {
                spriteBatch.Draw(Sprite);
            }

            if (Buff != null)
            {
                Buff.Sprite.Draw(spriteBatch);
            }

            foreach (Projectile projectile in Projectiles)
            {
                if (projectile != null && projectile.TargetHit == false)
                {
                    spriteBatch.Draw(projectile.Sprite);
                }
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
                    Vector2 healthPosition = new Vector2(position.X, Position.Y - 108);
                    spriteBatch.Draw(StatusBarTexture, position, new Rectangle(0, 0, (Convert.ToInt32(MaxHealth) + Convert.ToInt32(HealthBonus)), 6), Color.White);
                    spriteBatch.Draw(HealthBarTexture, healthPosition, new Rectangle(0, 100, Convert.ToInt32(CurrentHealth), 4), Color.White);

                    // Draw mana bar
                    spriteBatch.Draw(StatusBarTexture, new Vector2(position.X + 1, Position.Y - 104), new Rectangle(0, 0, (Convert.ToInt32(MaxMana) + Convert.ToInt32(ManaBonus)), 6), Color.White);
                    spriteBatch.Draw(ManaBarTexture, new Vector2(healthPosition.X, healthPosition.Y + 6), new Rectangle(0, 100, Convert.ToInt32(CurrentMana), 4), Color.White);

                    // Draw xp bar
                    //spriteBatch.Draw(StatusBarTexture, new Vector2(position.X, Position.Y - 118), new Rectangle(0, 0, 300, 6), Color.White);
                    //spriteBatch.Draw(ManaBarTexture, new Vector2(position.X, Position.Y - 118), new Rectangle(0, 100, Convert.ToInt32((XP * 100) / 300), 4), Color.White);

              //      spriteBatch.Draw(ActionBarTexture, new Vector2(position.X + 125, Position.Y + 85), new Rectangle(0, 0, 100, 200), Color.White);


                }
            }
            else
            {
                // Draw NPC Status and Health Bars
                if (CurrentHealth > 0)
                {
                  //  spriteBatch.DrawString(Init.Font, EnemyAI.enemiesInRange.IndexOf(this).ToString(), new Vector2(position.X, position.Y + 20), Color.Red);
                    spriteBatch.Draw(NPCStatusBarTexture, position, new Rectangle(0, 0, 15, 1), Color.Black);
                    spriteBatch.Draw(StaminaBarTexture, position, new Rectangle(0, 0, (int)((CurrentHealth / MaxHealth) * 15), 1 ), Color.White);
                }
            }
        }

        public void OnCollision(CollisionInfo collisionInfo)
        {
            throw new NotImplementedException();
        }
    }
}