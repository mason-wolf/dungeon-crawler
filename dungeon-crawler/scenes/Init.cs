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
        // Store the maps.
        public static Map StartingAreaMap;
        //public static Map Level_1Map;
        //public static Map Level_1AMap;
        //public static Map Level_1BMap;
        public static SpriteFont Font;

        public EscapeMenu escapeMenu;
        public static SaveMenu saveMenu;
        public static LoadMenu loadMenu;

        public static Inventory itemInventory;
        public static Inventory spellInventory;
        public static Inventory shopInventory;

        // Stores a list of teleporters from imported maps.
        public static List<Teleporter> teleporterList;

        private GameWindow window;
        public static Scene SelectedScene { get; set; }
        public static Map SelectedMap { get; set; }
        public static Vector2 SavedGamePosition;
        public static string SavedGameLocation;
        public static DialogBox dialogBox;
        public static bool TransitionState = false;

        // Store the player's collision state to pass to different scenes.
        IBox playerCollision;

        public static bool Reloaded = false;

        List<Level> levelList = new List<Level>();
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
            StartingArea,
            Level_1,
            Castle
        }

        protected override void LoadContent()
        {
            teleporterList = new List<Teleporter>();
            StartingAreaMap = new Map(Content, "Content/maps/castle.tmx");
            Sprites sprites = new Sprites();
            sprites.LoadContent(Content);

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

            itemInventory = new Inventory(Content);
            itemInventory.MenuTitle = "Items";
            itemInventory.InventoryType = "inventory";

            spellInventory = new Inventory(Content);
            spellInventory.MenuTitle = "Spells";
            spellInventory.InventoryType = "spells";

            shopInventory = new Inventory(Content);
            shopInventory.InventoryType = "shop";
            shopInventory.MenuTitle = "Shop";

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

            spellInventory.Contents.Add(fireballSpell);
            spellInventory.Contents.Add(iceBoltSpell);
            shopInventory.Contents.Add(healthPotion);
            shopInventory.Contents.Add(manaPotion);

            dialogBox = new DialogBox(game, Font);


            SelectedScene = Scene.Castle;
            Player.Position = new Vector2(335f, 900f);
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
                SelectedScene = (Init.Scene)Enum.Parse(typeof(Init.Scene), SavedGameLocation);

                if(SelectedScene.ToString() == "StartingArea")
                {
                    FadeInMap("StartingArea");
                    Player.Position = new Vector2(335f, 150f);
                }
                else
                {
                    foreach (Level level in levelList)
                    {
                        if (level.GetLevelName() == SelectedScene.ToString())
                        {
                            FadeInMap(level.GetMap().GetMapName());
                        }
                    }
                }

                Player.Position = SavedGamePosition;
                Player.MotionVector = SavedGamePosition;
                Reloaded = false;
            }

            // Handle Teleportation
            foreach(Teleporter teleporter in teleporterList)
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

                        levelList.Clear();
                        Content.Unload();
                        LoadContent();
                        TransitionState = true;
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
            if (SelectedScene == Scene.StartingArea)
            {
                playerCollision = StartingAreaMap.GetCollisionWorld();
                StartingAreaMap.Update(gameTime);
                SelectedMap = StartingAreaMap;
                ToggleTeleporters(SelectedMap.GetMapName());
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

                FadeInMap("StartingArea");
                SelectedScene = Scene.StartingArea;
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
            itemInventory.Update(gameTime);
            spellInventory.Update(gameTime);
            shopInventory.Update(gameTime);

            Player.Update(gameTime);
          //  OffsetWeaponPosition();
        //    Player.PlayerWeapon.Update(gameTime);

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

            if (SelectedScene == Scene.StartingArea)
            {
                StartingAreaMap.Draw(spriteBatch);
            }
            else
            {
                foreach (Level level in levelList)
                {
                    if (level.GetLevelName() == SelectedScene.ToString())
                    {
                        level.GetMap().Draw(spriteBatch);
                        level.Draw(spriteBatch);
                    }
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
             //   Player.PlayerWeapon.Draw(spriteBatch);
                Player.DrawHUD(spriteBatch, playerHealthPosition, true);

                //int health = (int)Player.CurrentHealth;
                //Vector2 healthStatus = new Vector2(playerHealthPosition.X + 10, playerHealthPosition.Y);
                //spriteBatch.DrawString(Font, health.ToString() + " / 100", healthStatus, Color.White);

                dialogBox.Draw(spriteBatch);
                itemInventory.Draw(spriteBatch);
                spellInventory.Draw(spriteBatch);
                shopInventory.Draw(spriteBatch);

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
            if (mapName == "StartingArea")
            {
                StartingAreaMap.FadeIn();
                if (StartingAreaMap.HasFaded())
                {
                    StartingAreaMap.HasFaded(false);
                    StartingAreaMap.SetTransitionColor(new Color(255, 255, 255, 255));
                }
            }
            else
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
            foreach (Teleporter teleporter in teleporterList)
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
        /// <summary>
        /// Offsets weapon position relative to player, handy
        /// when creating different weapons with varying sizes and
        /// hand placement.
        /// </summary>
        public void OffsetWeaponPosition()
        {
            if (Player.PlayerWeapon.State == Action.AttackWestPattern1)
            {
                Player.PlayerWeapon.Position = new Vector2(Player.Position.X - 3, Player.Position.Y);
            }
            else if (Player.PlayerWeapon.State == Action.AttackEastPattern1)
            {
                Player.PlayerWeapon.Position = new Vector2(Player.Position.X + 2, Player.Position.Y);
            }
            else
            {
                Player.PlayerWeapon.Position = Player.Position;
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