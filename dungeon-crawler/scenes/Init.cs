using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.ViewportAdapters;
using MonoGame.Extended.Tiled.Renderers;
using DungeonCrawler.Scenes;
using MonoGame.Extended.TextureAtlases;
using MonoGame.Extended.Animations.SpriteSheets;
using MonoGame.Extended.Collisions;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using System;
using DungeonCrawler.Engine;
using Humper;
using Humper.Responses;
using RoyT.AStar;
using MonoGame.Extended.Sprites;
using Microsoft.Xna.Framework.Content;
using DungeonCrawler.Interface;
using Demo.Interface;
using System.Diagnostics;
using Demo.Game;

namespace DungeonCrawler.Scenes
{
    class Init : SceneManager
    {
        public static ViewportAdapter viewPortAdapter;
        // Monitor keyboard states.
        public static KeyboardState KeyBoardOldState;
        public static KeyboardState KeyBoardNewState;
        public static Player Player;
        public static Camera2D Camera;

        public static SpriteFont Font;

        public EscapeMenu escapeMenu;
        public static SaveMenu saveMenu;
        public static LoadMenu loadMenu;

        public static Inventory ItemInventory;
        public static Inventory SpellInventory;
        public static Inventory ShopInventory;

        public static Items Items { get; set; }
        // Stores a list of teleporters from imported maps.
        public static List<Teleporter> Teleporters;

        private GameWindow window;
        public static Scene SelectedScene { get; set; }
        public static Map SelectedMap { get; set; }
        public static DialogBox dialogBox;
        public static bool TransitionState = false;

        // Store the player's collision state to pass to different scenes.
        IBox playerCollision;

        public static bool Reloaded = false;

        List<Level> levelList;
        Level newLevel;

        public static string Message = "";
        public static bool MessageEnabled = false;
        static int messageFrameCount = 0;

        public Init(Game game, GameWindow window) : base(game)
        {
            this.window = window;
            viewPortAdapter = new BoxingViewportAdapter(window, GraphicsDevice, 1080, 720);
            Camera = new Camera2D(viewPortAdapter);
            Player = new Player();
            Player.LoadContent(Content);
            Player.Sprite = new AnimatedSprite(Player.playerAnimation);
            Player.State = Action.IdleSouth1;
            Player.MaxHealth = 100;
            Player.CurrentHealth = 100;
            Player.MaxStamina = 75;
            Player.CurrentStamina = 75;
            Player.AttackDamage = 3.5;
            Player.Gold = 50;
            base.Initialize();
        }

        // Store a list of scenes to switch to.
        public enum Scene
        {
            EscapeMenu,
            SaveMenu,
            LoadMenu,
            Inventory,
            Level_1,
            Castle
        }

        protected override void LoadContent()
        {
            Teleporters = new List<Teleporter>();
            levelList = new List<Level>();
            Items = new Items();
            newLevel = new Level();
            newLevel.SetMap(new Map(Content, "Content/maps/castle.tmx"));
            newLevel.SetScene(new Level_1());
            newLevel.SetLevelName("Castle");
            newLevel.LoadContent(Content);
            levelList.Add(newLevel);

            newLevel = new Level();
            newLevel.SetMap(new Map(Content, "Content/maps/level_1.tmx"));
            newLevel.SetScene(new Level_1A());
            newLevel.SetLevelName("Level_1");
            newLevel.LoadContent(Content);
            levelList.Add(newLevel);


            Font = Content.Load<SpriteFont>(@"interface\font");
            Player.LoadContent(Content);
            Player.Sprite = new AnimatedSprite(Player.playerAnimation);
            Player.State = Action.IdleSouth1;

            escapeMenu = new EscapeMenu(game, window, Content);
            saveMenu = new SaveMenu(game, window, Content);
            loadMenu = new LoadMenu(game, window, Content);

            string[] items = { "Continue", "Save", "Load", "Quit" };
            escapeMenu.SetMenuItems(items);

            ItemInventory = new Inventory(Content);
            ItemInventory.MenuTitle = "Items";
            ItemInventory.InventoryType = "inventory";

            SpellInventory = new Inventory(Content);
            SpellInventory.MenuTitle = "Spells";
            SpellInventory.InventoryType = "spells";

            ShopInventory = new Inventory(Content);
            ShopInventory.InventoryType = "shop";
            ShopInventory.MenuTitle = "Shop";

            Item fireballSpell = new Item();
            fireballSpell.ItemTexture = Sprites.GetTexture("FIREBALL_1_ICON");
            fireballSpell.Name = "FIREBALL";
            fireballSpell.ID = 1;
            fireballSpell.Description = "Shoots a flame.";

            Item iceBoltSpell = new Item();
            iceBoltSpell.ItemTexture = Sprites.GetTexture("ICEBOLT_1_ICON");
            iceBoltSpell.Name = "ICEBOLT";
            iceBoltSpell.ID = 2;
            iceBoltSpell.Description = "Casts a bolt of ice.";

            Item healthPotion = new Item();
            healthPotion.ItemTexture = Sprites.GetTexture("HEALTH_POTION_ICON");
            healthPotion.Name = "HEALTH POTION";
            healthPotion.Description = "Restores some health.";
            healthPotion.ID = 3;
            healthPotion.Price = 5;

            Item manaPotion = new Item();
            manaPotion.ItemTexture = Sprites.GetTexture("MANA_POTION_ICON");
            manaPotion.Name = "MANA POTION";
            manaPotion.Description = "Restores some mana.";
            manaPotion.Price = 5;

            SpellInventory.Contents.Add(fireballSpell);
            SpellInventory.Contents.Add(iceBoltSpell);
            ShopInventory.Contents.Add(healthPotion);
            ShopInventory.Contents.Add(manaPotion);

            dialogBox = new DialogBox(game, Font);

            SelectedScene = Scene.Castle;
            base.LoadContent();
        }

        int transitionFrames = 0;
        int pauseAfterDeathFrames = 0;
        public override void Update(GameTime gameTime)
        {
            // If save was loaded, create transition effects, assign the player's saved scene and position.
            if (Reloaded)
            {
                TransitionState = true;
                SelectedScene = Scene.Castle;
                SelectedMap = levelList.Find(map => map.GetLevelName() == "Castle").GetMap();
                FadeInMap("Castle");
                Player.State = Action.IdleSouth1;
                Player.InMenu = false;
                Player.Position = levelList.Find(level => level.GetLevelName() == Scene.Castle.ToString()).GetStartingPosition();
                Reloaded = false;
            }
            // Handle Teleportation
            foreach (Teleporter teleporter in Teleporters)
            {
                if (teleporter.Enabled)
                {
                    if (Player.BoundingBox.Intersects(teleporter.GetRectangle()))
                    {
                        if (Player.EnemyList.Count > 0)
                        {
                            Player.EnemyList.Clear();

                            foreach (Level level in levelList)
                            {
                                level.GetEnemyAI().Clear();
                            }
                        }

                        TransitionState = true;
                        LoadContent();
                        FadeInMap(teleporter.GetDestinationMap());
                        SelectedScene = (Init.Scene)Enum.Parse(typeof(Init.Scene), teleporter.GetDestinationMap());
                        Player.Position = teleporter.GetTargetPosition();
                    }
                }
            }

            switch (SelectedScene)
            {
                case Scene.EscapeMenu:
                    escapeMenu.Update(gameTime);
                    break;
                case Scene.SaveMenu:
                    saveMenu.Update(gameTime);
                    break;
                case Scene.LoadMenu:
                    loadMenu.Update(gameTime);
                    break;
            }

            // Scene switching.
            if (SelectedScene == Scene.LoadMenu)
            {
                Player.Position = levelList.Find(map => map.GetLevelName() == Scene.Castle.ToString()).GetStartingPosition();
                playerCollision = levelList.Find(level => level.GetLevelName() == "Castle").GetMap().GetCollisionWorld();
            }
            else
            {
                // Find the selected scene.
                foreach (Level level in levelList)
                {
                    if (level.GetLevelName() == SelectedScene.ToString())
                    {
                        playerCollision = level.GetMap().GetCollisionWorld();
                        SelectedMap = level.GetMap();
                        level.SetPlayerStartPosition(Player);
                        ToggleTeleporters(SelectedMap.GetMapName());
                        Player.EnemyList = level.GetEnemyList();
                        level.Update(gameTime);
                    }
                }
            }

            // Create a pause if the player dies.
            if (Player.Dead)
            {
                pauseAfterDeathFrames++;
            }

            // If the player dies, reload and restart from beginning.
            if (pauseAfterDeathFrames > 200)
            {
                if (Player.EnemyList.Count > 0)
                {
                    Player.EnemyList.Clear();
                    foreach (Level level in levelList)
                    {
                        level.GetEnemyAI().Clear();
                    }
                }

                levelList.Clear();
                TransitionState = true;
                Content.Unload();
                LoadContent();

                FadeInMap("Castle");
                SelectedScene = Scene.Castle;
                Player.Position = new Vector2(335f, 150f);
                pauseAfterDeathFrames = 0;
                Player.Dead = false;
                Player.CurrentHealth = 100;
                Player.State = Action.IdleSouth1;
            }

            // Create a short pause after transitioning to next level.
            transitionFrames++;

            if (transitionFrames > 70)
            {
                transitionFrames = 0;
                TransitionState = false;
            }

            KeyBoardNewState = Keyboard.GetState();

            HandleDialog();

            dialogBox.Update();
            ItemInventory.Update(gameTime);
            SpellInventory.Update(gameTime);
            ShopInventory.Update(gameTime);

            Player.Update(gameTime);

            // Handle player's collision.
            playerCollision.Move(Player.Position.X, Player.Position.Y, (collision) => CollisionResponses.Slide);

            Camera.Zoom = 3;

            if (!inDialog && !TransitionState && !Player.Dead)
            {
                Player.HandleInput(gameTime, Player, playerCollision, KeyBoardNewState, KeyBoardOldState);
                Player.CheckPlayerStatus(gameTime);
            }

            Camera.LookAt(Player.Position);
            KeyBoardOldState = KeyBoardNewState;
            KeyBoardNewState = Keyboard.GetState();
            escapeMenu.Position = new Vector2(Player.Position.X, Player.Position.Y - 125);
            saveMenu.Position = new Vector2(Player.Position.X, Player.Position.Y - 125);
            loadMenu.Position = new Vector2(Player.Position.X, Player.Position.Y - 125);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: Camera.GetViewMatrix());

            // Draw the selected screen.
            foreach (Level level in levelList)
            {
                if (level.GetLevelName() == SelectedScene.ToString())
                {
                    level.GetMap().Draw(spriteBatch);
                    level.Draw(spriteBatch);
                }
            }

            // Escape menu.
            if (SelectedScene == Scene.EscapeMenu)
            {
                escapeMenu.Draw(spriteBatch);
            }
            // Save menu.
            else if (SelectedScene == Scene.SaveMenu)
            {
                saveMenu.Draw(spriteBatch);
            }
            // Load menu.
            else if (SelectedScene == Scene.LoadMenu)
            {
                if (!Reloaded)
                {
                    loadMenu.Draw(spriteBatch);
                }
            }
            else
            {
                Vector2 playerHealthPosition = new Vector2(Player.Position.X - 170, Player.Position.Y - 110);

                Player.Draw(spriteBatch);
                Player.DrawHUD(spriteBatch, playerHealthPosition, true);

                //int health = (int)Player.CurrentHealth;
                //Vector2 healthStatus = new Vector2(playerHealthPosition.X + 10, playerHealthPosition.Y);
                //spriteBatch.DrawString(Font, health.ToString() + " / 100", healthStatus, Color.White);

                dialogBox.Draw(spriteBatch);
                ItemInventory.Draw(spriteBatch);
                SpellInventory.Draw(spriteBatch);
                ShopInventory.Draw(spriteBatch);

            }

            if (Player.Dead)
            {
                spriteBatch.DrawString(Init.Font, "YOU DIED", new Vector2(Init.Player.Position.X - 20, Init.Player.Position.Y - 50), Color.Red);
            }

            if (MessageEnabled)
            {
                ShowMessage(Message, spriteBatch);
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }

        static bool inDialog = false;
        public static bool startDialog = false;
        public static void HandleDialog()
        {
            if (KeyBoardNewState.IsKeyDown(Keys.E) && KeyBoardOldState.IsKeyUp(Keys.E) && startDialog)
            {
                dialogBox.Position = new Vector2(Player.Position.X - 125, Player.Position.Y + 50);
                dialogBox.Show();
            }

            if (dialogBox.IsActive())
            {
                inDialog = true;
            }
            else
            {
                inDialog = false;
            }
        }
        /// <summary>
        /// Creates a transition effect on the map.
        /// </summary>
        /// <param name="map">Map to fade in.</param>
        public void FadeInMap(string mapName)
        {
            foreach (Level level in levelList)
            {
                if (level.GetLevelName() == mapName)
                {
                    // Fade in the screen
                    level.GetMap().FadeIn();

                    // The effect occured once, so set the trigger back to false and reset visibility color.
                    if (level.GetMap().HasFaded())
                    {
                        level.GetMap().HasFaded(false);
                        level.GetMap().SetTransitionColor(new Color(255, 255, 255, 255));
                    }
                }
            }
        }

        public static void OpenSaveMenu(Game game, GameWindow window, ContentManager content)
        {
            saveMenu = new SaveMenu(game, window, content);
        }

        public static void OpenLoadMenu(Game game, GameWindow window, ContentManager content)
        {
            loadMenu = new LoadMenu(game, window, content);
        }
        public override void Show()
        {
            base.Show();
            Enabled = true;
            Visible = true;
        }

        /// <summary>
        /// Enables/Disables teleporters depending on which scene is selected.
        /// </summary>
        /// <param name="selectedScene">Which scene to enable teleporters</param>
        public void ToggleTeleporters(string selectedMap)
        {
            foreach (Teleporter teleporter in Teleporters)
            {
                if (teleporter.GetSourceMap() == selectedMap)
                {
                    teleporter.Enabled = true;
                }
                else
                {
                    teleporter.Enabled = false;
                }
            }
        }

        public static void ShowMessage(string message, SpriteBatch spriteBatch)
        {
            if (messageFrameCount < 120)
            {
                spriteBatch.DrawString(Init.Font, message, new Vector2(Init.Player.Position.X - 165, Init.Player.Position.Y + 105), Color.White);
                messageFrameCount++;
            }
            else
            {
                MessageEnabled = false;
                messageFrameCount = 0;
            }
        }

        public static void Reload()
        {
            Reloaded = true;
        }
        public override void Hide()
        {
            base.Hide();
            Enabled = false;
            Visible = false;
        }


    }


}