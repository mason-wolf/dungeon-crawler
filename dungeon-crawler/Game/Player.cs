using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using DungeonCrawler.Scenes;
using MonoGame.Extended.TextureAtlases;
using MonoGame.Extended.Animations.SpriteSheets;
using Microsoft.Xna.Framework.Content;
using MonoGame.Extended;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.Particles;
using MonoGame.Extended.SceneGraphs;
using MonoGame.Extended.ViewportAdapters;
using Microsoft.Xna.Framework.Audio;
using System.Timers;
using Humper;
using DungeonCrawler.Interface;
using DungeonCrawler.Engine;
using Humper.Responses;
using Demo.Interface;
using Demo.Game;
using static Demo.Game.Spell;

namespace DungeonCrawler
{
    public class Player : Entity
    {
        private Texture2D playerTexture;
        private TextureAtlas playerAtlas;
        public SpriteSheetAnimationFactory playerAnimation;
        MouseState oldMouseState;
        MouseState newMouseState;
   
        public List<Entity> EnemyList { get; set; }
        public static List<Item> InventoryList = new List<Item>();
        public static bool PressedContinue = false;
        public static bool IsAttacking = false;
        public static bool ActionButtonPressed = false;
        public static Vector2 MotionVector;
        public static string CurrentLevel { get; set; }

        public static Item SelectedItem { get; set; }
        public int EnemiesKilled { get; set; }
        public int Gold { get; set; }
        public static Entity PlayerWeapon;
        public bool InMenu = false;
        // Store currently running scene to revert back after exiting escape menu.
        Init.Scene currentScene = Init.SelectedScene;
        Random random = new Random();
        List<SoundEffect> soundEffects;

        float MovementSpeed = 1.2f;

        public new void LoadContent(ContentManager content)
        {
            playerTexture = content.Load<Texture2D>(@"spritesheets\mage");
            playerAtlas = TextureAtlas.Create(playerTexture, 24, 24);
            playerAnimation = new SpriteSheetAnimationFactory(playerAtlas);
            Sprites sprites = new Sprites();
            sprites.LoadContent(content);

            //swordWeapon = new Entity(Sprites.swordAnimation);
            //swordWeapon.State = Action.AttackEastPattern1;

            //bowWeapon = new Entity(Sprites.bowAnimation);
            //bowWeapon.State = Action.IdleSouth1;
            //PlayerWeapon = swordWeapon;

            //arrow = new AnimatedSprite(Sprites.arrowAnimation);
            float animationSpeed = .15f;
            float attackSpeed = 0.05f;
            playerAnimation.Add("idleSouth1", new SpriteSheetAnimationData(new[] { 0 }));
            playerAnimation.Add("walkSouthPattern1", new SpriteSheetAnimationData(new[] { 1, 2 }, animationSpeed, isLooping: true));
            playerAnimation.Add("attackSouthPattern1", new SpriteSheetAnimationData(new[] { 12, 12, 12 }, attackSpeed, isLooping: true));
            playerAnimation.Add("walkWestPattern1", new SpriteSheetAnimationData(new[] { 5, 6 }, animationSpeed, isLooping: true));
            playerAnimation.Add("attackWestPattern1", new SpriteSheetAnimationData(new[] { 14, 14, 14}, attackSpeed, isLooping: true));
            playerAnimation.Add("idleWest1", new SpriteSheetAnimationData(new[] { 5 }));
            playerAnimation.Add("walkEastPattern1", new SpriteSheetAnimationData(new[] {3, 4 }, animationSpeed, isLooping: true));
            playerAnimation.Add("attackEastPattern1", new SpriteSheetAnimationData(new[] { 13, 13, 13 }, attackSpeed, isLooping: true));
            playerAnimation.Add("idleEast1", new SpriteSheetAnimationData(new[] { 3 }));
            playerAnimation.Add("walkNorthPattern1", new SpriteSheetAnimationData(new[] { 8, 9 }, animationSpeed, isLooping: true));
            playerAnimation.Add("attackNorthPattern1", new SpriteSheetAnimationData(new[] { 15, 15, 15 }, attackSpeed, isLooping: true));
            playerAnimation.Add("idleNorth1", new SpriteSheetAnimationData(new[] { 7 }));
            playerAnimation.Add("dead", new SpriteSheetAnimationData(new[] { 10 }, 0.08f, isLooping: false));
            StatusBarTexture = content.Load<Texture2D>(@"interface\statusbar");
            HealthBarTexture = content.Load<Texture2D>(@"interface\healthbar");
            StaminaBarTexture = content.Load<Texture2D>(@"interface\staminabar");
            ManaBarTexture = content.Load<Texture2D>(@"interface\MANA_BAR");
            soundEffects = new List<SoundEffect>();
         //   soundEffects.Add(content.Load<SoundEffect>(@"sounds\sword-swing"));
         //   soundEffects.Add(content.Load<SoundEffect>(@"sounds\bow-shoot"));
        }


        /// <summary>
        /// Method to check status of the player. Death, status effects, etc.
        /// </summary>
        /// <param name="gameTime"></param>
        public void CheckPlayerStatus(GameTime gameTime)
        {
            if (CurrentHealth <= 0)
            {
                Dead = true;
                Sprite.Play("dead");
            }
        }

        // Handle attacking and movement animations.
        private Vector2 oldPosition;
        public void HandleInput(GameTime gameTime, Entity player, IBox playerCollisionBox, KeyboardState newState, KeyboardState oldState)
        {
            MotionVector = new Vector2(playerCollisionBox.X, playerCollisionBox.Y);
            //  ActionButtonPressed = false;

            // If action button 'F' is pressed.
            if (newState.IsKeyDown(Keys.E) && oldState.IsKeyUp(Keys.E))
            {
                ActionButtonPressed = true;
            }

            //// If run button 'Shift' is held down.
            //if (newState.IsKeyDown(Keys.LeftShift) && CurrentStamina >= 0)
            //{
            //    speed = 2.2f;
            //    CurrentStamina -= .4;
            //}
            //else if (newState.IsKeyDown(Keys.LeftShift) && currentScene <= 0)
            //{
            //    speed = 1.4f; 
            //}
            //else
            //{
            //    speed = 1.4f;
            //    if (CurrentStamina <= MaxStamina)
            //    {
            //        CurrentStamina += 0.5;
            //    }
            //}

            newMouseState = Mouse.GetState();

            // Handle escape menu.
            if (newState.IsKeyDown(Keys.Escape) && oldState.IsKeyUp(Keys.Escape) || PressedContinue == true)
            {
                // Exit the menu if Escape is pressed or if player pressed continue.
                if (InMenu)
                {
                    Init.SelectedScene = currentScene;
                    InMenu = false;
                    PressedContinue = false;
                    Init.Player.Position = oldPosition;
                }
                else
                {
                    oldPosition = Init.Player.Position;
                    // Open the menu if escape is pressed.
                    currentScene = Init.SelectedScene;
                    // Store the current level to save progress later.
                    CurrentLevel = currentScene.ToString();
                    Init.SelectedScene = Init.Scene.ESCAPE_MENU;

                    InMenu = true;
                    SaveMenu.GameSaved = false;
                }
            }

            // Handle item inventory
            if (newState.IsKeyDown(Keys.I) && oldState.IsKeyUp(Keys.I) && !Shop.InventoriesOpen() && !Init.SpellInventory.InventoryOpen && !PlayerStatus.StatusOpen)
            {
                if (Init.ItemInventory.InventoryOpen)
                {
                    Init.ItemInventory.InventoryOpen = false;
                }
                else
                {
                    Init.ItemInventory.InventoryOpen = true;
                }
            }

            // Handle spell inventory
            if (newState.IsKeyDown(Keys.Tab) && oldState.IsKeyUp(Keys.Tab) && !Shop.InventoriesOpen() && !Init.ItemInventory.InventoryOpen && !PlayerStatus.StatusOpen)
            {
                if (Init.SpellInventory.InventoryOpen)
                {
                    Init.SpellInventory.InventoryOpen = false;
                }
                else
                {
                    Init.SpellInventory.InventoryOpen = true;
                }
            }

            // Handle status screen
            if (newState.IsKeyDown(Keys.P) && oldState.IsKeyUp(Keys.P) && !Shop.InventoriesOpen() && !Init.ItemInventory.InventoryOpen && !Init.SpellInventory.InventoryOpen)
            {
                if (PlayerStatus.StatusOpen)
                {
                    PlayerStatus.StatusOpen = false;
                    InMenu = false;
                }
                else
                {
                    PlayerStatus.StatusOpen = true;
                    InMenu = true;
                }
            }

            // Equip sword if 1 is pressed.
            if (newState.IsKeyDown(Keys.D1) && oldState.IsKeyDown(Keys.D1))
            {
                //EquipedWeapon = "Sword";
                //PlayerWeapon = swordWeapon;
                switch (State)
                {
                    case (Action.IdleEast2):
                        State = Action.IdleEast1;
                        break;
                    case (Action.IdleWest2):
                        State = Action.IdleWest1;
                        break;
                    case (Action.IdleNorth2):
                        State = Action.IdleNorth1;
                        break;
                    case (Action.IdleSouth2):
                        State = Action.IdleSouth1;
                        break;
                }
            }

            //// Equip bow if 2 is pressed.
            //if (newState.IsKeyDown(Keys.D2) && oldState.IsKeyDown(Keys.D2))
            //{
            //    EquipedWeapon = "Bow";
            //    PlayerWeapon = bowWeapon;
            //}

            // Handle Movement
            if (!InMenu && !Init.ItemInventory.InventoryOpen && !Init.SpellInventory.InventoryOpen && !Init.ItemShopInventory.InventoryOpen)
            {
                // Attacking south
                if (newMouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released && player.State == Action.WalkSouthPattern1 ||
                    newMouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released && player.State == Action.IdleSouth1 ||
                    newMouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released && player.State == Action.IdleSouth2 ||
                    newMouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released && player.State == Action.WalkSouthPattern2)
                {
                    if (SelectedItem != null)
                    {
                        player.State = Action.AttackSouthPattern1;
                        CastSpell(SpellDirection.SOUTH, SelectedItem.ID);
                    }
                }

                // Attacking West
                else if (newMouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released && player.State == Action.WalkWestPattern1 ||
                    newMouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released && player.State == Action.IdleWest1 ||
                    newMouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released && player.State == Action.IdleWest2 ||
                    newMouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released && player.State == Action.WalkWestPattern2)
                {
                    if (SelectedItem != null)
                    {
                        player.State = Action.AttackWestPattern1;
                        CastSpell(SpellDirection.WEST, SelectedItem.ID);
                    }
                }


                // Attacking East
                else if (newMouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released && player.State == Action.WalkEastPattern1 ||
                    newMouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released && player.State == Action.IdleEast1 ||
                    newMouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released && player.State == Action.WalkEastPattern2 ||
                    newMouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released && player.State == Action.IdleEast2 && IsAttacking)
                {
                    if (SelectedItem != null)
                    {
                        player.State = Action.AttackEastPattern1;
                        CastSpell(SpellDirection.EAST, SelectedItem.ID);
                    }
                }

                // Attacking North
                else if (newMouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released && player.State == Action.WalkNorthPattern1 ||
                    newMouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released && player.State == Action.IdleNorth1 ||
                    newMouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released && player.State == Action.WalkNorthPattern2 ||
                    newMouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released && player.State == Action.IdleNorth2)
                {
                    if (SelectedItem != null)
                    {
                        player.State = Action.AttackNorthPattern1;
                        CastSpell(SpellDirection.NORTH, SelectedItem.ID);
                    }
                }
                else
                {
                    if (newState.IsKeyDown(Keys.W) && player.State != Action.AttackNorthPattern1)
                    {
                        // Walk east if W and D are pressed
                        if (newState.IsKeyDown(Keys.W) && newState.IsKeyDown(Keys.D))
                        {
                            MotionVector.Y -= MovementSpeed;
                            player.Position = MotionVector;
                            player.State = Action.WalkEastPattern1;
                        }
                        // Walk west if W and A are pressed.
                        else if (newState.IsKeyDown(Keys.W) && newState.IsKeyDown(Keys.A))
                        {
                            MotionVector.Y -= MovementSpeed;
                            player.Position = MotionVector;
                            player.State = Action.WalkWestPattern1;
                        }
                        else
                        {
                            // Walk north.
                            MotionVector.Y -= MovementSpeed;
                            player.Position = MotionVector;
                            player.State = Action.WalkNorthPattern1;
                        }
                    }

                    if (newState.IsKeyDown(Keys.S) && player.State != Action.AttackSouthPattern1)
                    {
                        // Walk east if S and D are pressed.
                        if (newState.IsKeyDown(Keys.S) && newState.IsKeyDown(Keys.D))
                        {
                            MotionVector.Y += MovementSpeed;
                            player.Position = MotionVector;
                            player.State = Action.WalkEastPattern1;
                        }

                        // Walk west if S and A are pressed.
                        else if (newState.IsKeyDown(Keys.S) && newState.IsKeyDown(Keys.A))
                        {
                            MotionVector.Y += MovementSpeed;
                            player.Position = MotionVector;
                            player.State = Action.WalkWestPattern1;
                        }
                        else
                        {
                            // Walk south
                            MotionVector.Y += MovementSpeed;
                            player.Position = MotionVector;
                            player.State = Action.WalkSouthPattern1;
                        }
                    }

                    // Walk east
                    if (newState.IsKeyDown(Keys.D) && player.State != Action.AttackEastPattern1)
                    {
                        MotionVector.X += MovementSpeed;
                        player.Position = MotionVector;
                        player.State = Action.WalkEastPattern1;
                    }

                    // Walk west
                    if (newState.IsKeyDown(Keys.A) && player.State != Action.AttackWestPattern1)
                    {
                        MotionVector.X -= MovementSpeed;
                        player.Position = MotionVector;
                        player.State = Action.WalkWestPattern1;
                    }
                }

                switch(player.State)
                {
                    case (Action.AttackEastPattern1):
                        IsAttacking = true;
                        break;
                    case (Action.AttackEastPattern2):
                        IsAttacking = true;
                        break;
                    case (Action.AttackNorthPattern1):
                        IsAttacking = true;
                        break;
                    case (Action.AttackNorthPattern2):
                        IsAttacking = true;
                        break;
                    case (Action.AttackSouthPattern1):
                        IsAttacking = true;
                        break;
                    case (Action.AttackSouthPattern2):
                        IsAttacking = true;
                        break;
                    case (Action.AttackWestPattern1):
                        IsAttacking = true;
                        break;
                    case (Action.AttackWestPattern2):
                        IsAttacking = true;
                        break;
                    default:
                        IsAttacking = false;
                        break;
                }
            }
            oldMouseState = newMouseState;
        }

        /// <summary>
        /// Detects if an entity has intersected an object on the collision layer.
        /// </summary>
        /// <param name="entity">Entity that has been added to the collision layer.</param>
        /// <returns>True = Intersected, False = No Intersection</returns>
        public bool IntersectsCollidable(Entity entity)
        {
            bool intersected = false;
            foreach (Tile tile in Init.SelectedMap.GetCollisionTiles())
            {
                if (entity.BoundingBox.Intersects(tile.Rectangle))
                {
                    intersected = true;
                }
            }

            return intersected;
        }

        public Vector2 CorrectIntersection(Entity entity)
        {
            foreach (Tile tile in Init.SelectedMap.GetCollisionTiles())
            {
                if (entity.BoundingBox.Intersects(tile.Rectangle))
                {
                    Rectangle entityRect = new Rectangle((int)entity.BoundingBox.X, (int)entity.BoundingBox.Y, 16, 16);
                    Vector2 intDepth = IntersectionDepth(entityRect, tile.Rectangle);
                    return intDepth;
                }
            }
            return new Vector2(entity.Position.X, entity.Position.Y);
        }

        private Vector2 IntersectionDepth(Rectangle rectA, Rectangle rectB)
        {
            // Calculate half sizes.
            float halfWidthA = rectA.Width / 2.0f;
            float halfHeightA = rectA.Height / 2.0f;
            float halfWidthB = rectB.Width / 2.0f;
            float halfHeightB = rectB.Height / 2.0f;

            // Calculate centers.
            Vector2 centerA = new Vector2(rectA.Left + halfWidthA, rectA.Top + halfHeightA);
            Vector2 centerB = new Vector2(rectB.Left + halfWidthB, rectB.Top + halfHeightB);

            // Calculate current and minimum-non-intersecting distances between centers.
            float distanceX = centerA.X - centerB.X;
            float distanceY = centerA.Y - centerB.Y;
            float minDistanceX = halfWidthA + halfWidthB;
            float minDistanceY = halfHeightA + halfHeightB;

            // If we are not intersecting at all, return (0, 0).
            if (Math.Abs(distanceX) >= minDistanceX || Math.Abs(distanceY) >= minDistanceY)
                return Vector2.Zero;

            // Calculate and return intersection depths.
            float depthX = distanceX > 0 ? minDistanceX - distanceX : -minDistanceX - distanceX;
            float depthY = distanceY > 0 ? minDistanceY - distanceY : -minDistanceY - distanceY;
            return new Vector2(depthX, depthY);
        }

        public void LearnSpell(Item spellBook)
        {
            if (spellBook != null)
            {
                if (Init.SpellInventory.Contents.Find(spell => spell.Name == spellBook.Name && spell.ID != spellBook.ID) != null)
                {
                    Init.Message = "You already know this spell.";
                    Init.MessageEnabled = true;
                }
                else
                {
                    Init.SpellInventory.Contents.Add(Items.ItemList.Find(spell => spell.Name == spellBook.Name && spell.ID != spellBook.ID));
                    Init.Message = "You learned the " + spellBook.Name + " spell!";
                    Init.MessageEnabled = true;
                }
            }
        }
    }

}
