﻿using System.Collections.Generic;
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
using System.Diagnostics;
using Demo.Game;
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

        public static SpriteFont Font;

        public EscapeMenu escapeMenu;
        public static SaveMenu saveMenu;
        public static LoadMenu loadMenu;

        public static Inventory ItemInventory;
        public static Inventory SpellInventory;
        public static Inventory ItemShopInventory;
        public static Inventory SpellShopInventory;
        public static Inventory SellShopInventory;

        public static Shop shops;
        PlayerStatus PlayerStatus;

        // Stores list of global items.
        public static Items Items { get; set; }
        // Stores a list of teleporters from imported maps.
        public static List<Teleporter> Teleporters;

        private GameWindow window;
        public static Scene SelectedScene { get; set; }
        public static Level SelectedLevel { get; set; }
        public static Map SelectedMap { get; set; }
        public static DialogBox DialogBox;
        public static bool TransitionState = false;

        // Store the player's collision state to pass to different scenes.
        IBox playerCollision;

        public static bool Reloaded = false;

        List<Level> levelList;
        Level newLevel;

        /// <summary>
        /// In-Game message parameters.
        /// </summary>
        public static string Message = "";
        public static bool MessageEnabled = false;
        private static int messageFrameCount = 0;


        /// <summary>
        ///  Flag for disabling other menus if in dialog.
        /// </summary>
        public static bool InDialog = false;

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
            Player.MaxMana = 100;
            Player.CurrentMana = 100;
            Player.MaxStamina = 75;
            Player.CurrentStamina = 75;
            Player.AttackDamage = 3.5;
            Player.Gold = 200;
            Player.Level = 1;
            Player.XP = 0;
            Player.XPRemaining = 250;
            ItemInventory = new Inventory(Content);
            SpellInventory = new Inventory(Content);
            base.Initialize();
        }

        // Store a list of scenes to switch to.
        public enum Scene
        {
            ESCAPE_MENU,
            SAVE_MENU,
            LOAD_MENU,
            LOADING_SCREEN,
            INVENTORY,
            PLAINS_1,
            PLAINS_2,
            PLAINS_3,
            PLAINS_4,
            PLAINS_5,
            FIRELANDS_1,
            CASTLE
        }

        public void LoadLevel(string levelName)
        {
            Level newLevel = levelList.Find(level => level.GetLevelName() == levelName);
            if (Player.EnemyList.Count > 0)
            {
                Player.EnemyList.Clear();
                newLevel.GetEnemyAI().Clear();
            }
            newLevel = new Level();
            newLevel.SetMap(new Map(Content, "Content/maps/" + levelName + ".tmx"));
            newLevel.SetScene(new Plains_1());
            newLevel.SetLevelName(levelName);
            newLevel.LoadContent(Content);
            newLevel.GetScene().LoadScene();
            levelList.Remove(levelList.Find(level => level.GetLevelName() == levelName));
            levelList.Add(newLevel);
            SelectedLevel = newLevel;
        }

        protected override void LoadContent()
        {
            Teleporters = new List<Teleporter>();
            levelList = new List<Level>();
            Items = new Items();
            Font = Content.Load<SpriteFont>(@"interface\font");

            newLevel = new Level();
            newLevel.SetMap(new Map(Content, "Content/maps/CASTLE.tmx"));
            newLevel.SetScene(new Castle());
            newLevel.SetLevelName("CASTLE");
            newLevel.LoadContent(Content);
            newLevel.GetScene().LoadScene();
            levelList.Add(newLevel);

            newLevel = new Level();
            newLevel.SetMap(new Map(Content, "Content/maps/PLAINS_1.tmx"));

            newLevel = new Level();
            newLevel.SetMap(new Map(Content, "Content/maps/PLAINS_2.tmx"));

            newLevel = new Level();
            newLevel.SetMap(new Map(Content, "Content/maps/PLAINS_3.tmx"));

            newLevel = new Level();
            newLevel.SetMap(new Map(Content, "Content/maps/PLAINS_4.tmx"));

            newLevel = new Level();
            newLevel.SetMap(new Map(Content, "Content/maps/PLAINS_5.tmx"));

            newLevel = new Level();
            newLevel.SetMap(new Map(Content, "Content/maps/FIRELANDS_1.tmx"));

            Player.LoadContent(Content);
            Player.Sprite = new AnimatedSprite(Player.playerAnimation);
            Player.State = Action.IdleSouth1;

            escapeMenu = new EscapeMenu(game, window, Content);
            saveMenu = new SaveMenu(game, window, Content);
            loadMenu = new LoadMenu(game, window, Content);

            string[] items = { "Continue", "Save", "Load", "Quit" };
            escapeMenu.SetMenuItems(items);

            ItemInventory.MenuTitle = "Items";
            ItemInventory.InventoryType = "PLAYER_INVENTORY";

            SpellInventory.MenuTitle = "Spells";
            SpellInventory.InventoryType = "spells";

            ItemShopInventory = new Inventory(Content);
            ItemShopInventory.InventoryType = "ITEM_SHOP";
            ItemShopInventory.MenuTitle = "Item Shop";

            SellShopInventory = new Inventory(Content);
            SellShopInventory.InventoryType = "SELL_SHOP";
            SellShopInventory.MenuTitle = "Sell";


            // Health potion
            ItemShopInventory.Contents.Add(Items.GetItemById(3));
            // Mana potion
            ItemShopInventory.Contents.Add(Items.GetItemById(4));
            // Homing Crystal
            ItemShopInventory.Contents.Add(Items.GetItemById(11));
           
            SpellShopInventory = new Inventory(Content);
            SpellShopInventory.InventoryType = "SPELL_SHOP";
            SpellShopInventory.MenuTitle = "Spell Shop";

            // TODO: Better organize shop population.
            // Fireball spell
            SpellShopInventory.Contents.Add(Items.GetItemById(7));
            // Frostbolt spell
            SpellShopInventory.Contents.Add(Items.GetItemById(8));
            // Thunderbolt spell
            SpellShopInventory.Contents.Add(Items.GetItemById(9));
            // Heal spell
            SpellShopInventory.Contents.Add(Items.GetItemById(10));
            // Flame Shield Spell
            SpellShopInventory.Contents.Add(Items.GetItemById(13));

            shops = new Shop();
            shops.Add(ItemShopInventory);
            shops.Add(SpellShopInventory);
            shops.Add(SellShopInventory);

            PlayerStatus = new PlayerStatus(Content);
            DialogBox = new DialogBox(game, Font);

            SelectedScene = Scene.CASTLE;
            base.LoadContent();
        }

        int transitionFrames = 0;
        int pauseAfterDeathFrames = 0;
        public static bool NewGame = false;
        public override void Update(GameTime gameTime)
        {
            // If save was loaded, create transition effects, assign the player's saved scene and position.
            if (Reloaded || NewGame)
            {
                UnloadContent();
                LoadContent();
                TransitionState = true;
                SelectedScene = Scene.CASTLE;
                SelectedMap = levelList.Find(map => map.GetLevelName() == "CASTLE").GetMap();
                FadeInMap("CASTLE");
                Player.State = Action.IdleSouth1;
                Player.InMenu = false;
                Player.Position = levelList.Find(level => level.GetLevelName() == Scene.CASTLE.ToString()).GetStartingPosition();
                Reloaded = false;
                NewGame = false;
            }
            // Handle Teleportation
            foreach (Teleporter teleporter in Teleporters)
            {
                if (teleporter.Enabled)
                {
                    if (Player.BoundingBox.Intersects(teleporter.GetRectangle()))
                    {
                        TransitionState = true;

                        switch(teleporter.GetDestinationMap())
                        {
                            case ("RANDOM_PLAIN"):
                                GetRandomLevel("PLAINS");
                                break;
                            case ("RANDOM_FIRELANDS"):
                                GetRandomLevel("FIRELANDS");
                                break;
                            default:
                                SelectedLevel = levelList.Find(l => l.GetLevelName() == "CASTLE");
                                SelectedScene = (Scene)Enum.Parse(typeof(Scene), "CASTLE");
                                Player.Position = SelectedLevel.GetStartingPosition();
                                break;
                        }
                    }
                }
            }

            switch (SelectedScene)
            {
                case Scene.ESCAPE_MENU:
                    escapeMenu.Update(gameTime);
                    break;
                case Scene.SAVE_MENU:
                    saveMenu.Update(gameTime);
                    break;
                case Scene.LOAD_MENU:
                    loadMenu.Update(gameTime);
                    break;
            }

            // Scene switching.
            if (SelectedScene == Scene.LOAD_MENU)
            {
                Player.Position = levelList.Find(map => map.GetLevelName() == Scene.CASTLE.ToString()).GetStartingPosition();
                playerCollision = levelList.Find(level => level.GetLevelName() == "CASTLE").GetMap().GetCollisionWorld();
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
            if (pauseAfterDeathFrames > 100)
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

                FadeInMap("CASTLE");
                SelectedScene = Scene.LOAD_MENU;
                Player.Position = new Vector2(335f, 150f);
                pauseAfterDeathFrames = 0;
                Player.Dead = false;
                Player.CurrentHealth = 100;
                Player.State = Action.IdleSouth1;
            }

            // Create a short pause after transitioning to next level.
            transitionFrames++;

            if (transitionFrames > 100)
            {
                transitionFrames = 0;
                TransitionState = false;
            }

            KeyBoardNewState = Keyboard.GetState();

            HandleDialog();

            DialogBox.Update();
            ItemInventory.Update(gameTime);
            SpellInventory.Update(gameTime);

            shops.ListenForInput(KeyBoardNewState, KeyBoardOldState);
            shops.Update(gameTime);

            Player.Update(gameTime);
            PlayerStatus.Update(gameTime);

            // Handle player's collision.
            playerCollision.Move(Player.Position.X, Player.Position.Y, (collision) => CollisionResponses.Slide);

            Camera.Zoom = 3;

            if (!InDialog && !TransitionState && !Player.Dead)
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

        public void GetRandomLevel(string levelType)
        {
            FadeInMap("CASTLE");
            Random randomLevel = new Random();
            int levelNum = randomLevel.Next(1, 6);
            string levelName = levelType + "_" + 1;
            LoadLevel(levelName);
            SelectedScene = (Scene)Enum.Parse(typeof(Scene), levelName);
            SelectedLevel = levelList.Find(l => l.GetLevelName() == levelName);
            Player.Position = SelectedLevel.GetStartingPosition();
            FadeInMap(SelectedLevel.GetLevelName());
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
            if (SelectedScene == Scene.ESCAPE_MENU)
            {
                escapeMenu.Draw(spriteBatch);
            }
            // Save menu.
            else if (SelectedScene == Scene.SAVE_MENU)
            {
                saveMenu.Draw(spriteBatch);
            }
            // Load menu.
            else if (SelectedScene == Scene.LOAD_MENU)
            {
                if (!Reloaded)
                {
                    loadMenu.Draw(spriteBatch);
                }
            }
            else if (SelectedScene == Scene.LOADING_SCREEN)
            {
                Vector2 loadingStatus = new Vector2(Player.Position.X - 20, Player.Position.Y - 20);
                spriteBatch.DrawString(Font, "Loading..", loadingStatus, Color.White);
            }
            else
            {
                Vector2 playerHealthPosition = new Vector2(Player.Position.X - 170, Player.Position.Y - 110);

                Player.Draw(spriteBatch);
                Player.DrawHUD(spriteBatch, playerHealthPosition, true);

                //int health = (int)Player.CurrentHealth;
                //Vector2 healthStatus = new Vector2(playerHealthPosition.X + 10, playerHealthPosition.Y);
                //spriteBatch.DrawString(Font, health.ToString() + " / 100", healthStatus, Color.White);

                DialogBox.Draw(spriteBatch);
                ItemInventory.Draw(spriteBatch);
                SpellInventory.Draw(spriteBatch);
                shops.Draw(spriteBatch);
                PlayerStatus.Draw(spriteBatch);
            }

            if (Player.Dead)
            {
                spriteBatch.DrawString(Init.Font, "YOU DIED", new Vector2(Init.Player.Position.X - 20, Init.Player.Position.Y - 50), Color.Red);
            }

            if (MessageEnabled)
            {
                ShowMessage(Message, spriteBatch);
            }

            shops.Draw(spriteBatch);
            spriteBatch.End();
            base.Draw(gameTime);
        }

        public static void HandleDialog()
        {
            if (KeyBoardNewState.IsKeyDown(Keys.E) && KeyBoardOldState.IsKeyUp(Keys.E) && DialogBox.StartDialog)
            {
                DialogBox.Position = new Vector2(Player.Position.X - 125, Player.Position.Y + 50);
                DialogBox.Show();
            }

            // Disable other menus if in dialog.
            if (DialogBox.IsActive())
            {
                InDialog = true;
            }
            else
            {
                InDialog = false;
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
            TransitionState = true;
            SelectedScene = Scene.LOADING_SCREEN;
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