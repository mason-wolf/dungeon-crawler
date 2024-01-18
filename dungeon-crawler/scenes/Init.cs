using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;
using System;
using Humper;
using Humper.Responses;
using MonoGame.Extended.Sprites;
using Microsoft.Xna.Framework.Content;
using DungeonCrawler.Interface;
using Demo.Game;
using Demo.Interface;
using Demo.Scenes;

namespace DungeonCrawler.Scenes
{
    class Init : SceneManager
    {
        public static ViewportAdapter ViewportAdapter;
        // Monitor keyboard states.
        public static KeyboardState KeyBoardOldState;
        public static KeyboardState KeyBoardNewState;
        public static Player Player;
        public static Camera2D Camera;

        public static SpriteFont Font;

        public EscapeMenu EscapeMenu;
        public static SaveMenu SaveMenu;
        public static LoadMenu LoadMenu;

        public static Inventory ItemInventory;
        public static Inventory SpellInventory;
        public static Inventory ItemShopInventory;
        public static Inventory SpellShopInventory;
        public static Inventory SellShopInventory;

        public static Shop Shops;
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

        public static List<Level> LevelList;
        Level newLevel;

        /// <summary>
        /// In-Game message parameters.
        /// </summary>
        public static string Message = "";
        public static bool MessageEnabled = false;
        private static int MessageFrameCount = 0;

        int transitionFrames = 0;
        int pauseAfterDeathFrames = 0;
        public static bool NewGame = false;

        /// <summary>
        ///  Flag for disabling other menus if in dialog.
        /// </summary>
        public static bool InDialog = false;

        public static Texture2D _whiteTexture;
        public Init(Game game, GameWindow window) : base(game)
        {
            this.window = window;
            ViewportAdapter = new BoxingViewportAdapter(window, GraphicsDevice, 1080, 720);
            Camera = new Camera2D(ViewportAdapter);
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
            Player.SpellPower = 1;
            Player.XP = 0;
            Player.XPRemaining = 250;
            Enemies.Load(Content);
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
            FIRELANDS_2,
            FIRELANDS_3,
            FIRELANDS_4,
            FIRELANDS_5,
            FROSTLANDS_1,
            FROSTLANDS_2,
            FROSTLANDS_3,
            FROSTLANDS_4,
            FROSTLANDS_5,
            THUNDERLANDS_1,
            THUNDERLANDS_2,
            THUNDERLANDS_3,
            THUNDERLANDS_4,
            THUNDERLANDS_5,
            CASTLE
        }

        public void LoadLevel(string levelName)
        {
            Level newLevel = LevelList.Find(level => level.GetLevelName() == levelName);
            if (Player.EnemyList.Count > 0)
            {
                Player.EnemyList.Clear();
                newLevel.GetEnemyAI().Clear();
            }
            newLevel = new Level();
            newLevel.SetMap(new Map(Content, "Content/maps/" + levelName + ".tmx"));
            switch(levelName.Split('_')[0])
            {
                case ("FIRELANDS"):
                    newLevel.SetScene(new Firelands());
                    break;
                case ("PLAINS"):
                    newLevel.SetScene(new Plains());
                    break;
                case ("FROSTLANDS"):
                    newLevel.SetScene(new Frostlands());
                    break;
                case ("THUNDERLANDS"):
                    newLevel.SetScene(new Thunderlands());
                    break;
            }
            newLevel.SetLevelName(levelName);
            newLevel.LoadContent(Content);
            newLevel.GetScene().LoadScene();
            LevelList.Remove(LevelList.Find(level => level.GetLevelName() == levelName));
            LevelList.Add(newLevel);
            SelectedLevel = newLevel;
        }

        protected override void LoadContent()
        {
            Teleporters = new List<Teleporter>();
            LevelList = new List<Level>();
            Items = new Items();
            Font = Content.Load<SpriteFont>(@"interface\font");

            newLevel = new Level();
            newLevel.SetMap(new Map(Content, "Content/maps/CASTLE.tmx"));
            newLevel.SetScene(new Castle());
            newLevel.SetLevelName("CASTLE");
            newLevel.LoadContent(Content);
            newLevel.GetScene().LoadScene();
            LevelList.Add(newLevel);

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

            newLevel = new Level();
            newLevel.SetMap(new Map(Content, "Content/maps/FIRELANDS_2.tmx"));

            newLevel = new Level();
            newLevel.SetMap(new Map(Content, "Content/maps/FIRELANDS_3.tmx"));

            newLevel = new Level();
            newLevel.SetMap(new Map(Content, "Content/maps/FIRELANDS_4.tmx"));

            newLevel = new Level();
            newLevel.SetMap(new Map(Content, "Content/maps/FIRELANDS_5.tmx"));

            newLevel = new Level();
            newLevel.SetMap(new Map(Content, "Content/maps/FROSTLANDS_1.tmx"));

            newLevel = new Level();
            newLevel.SetMap(new Map(Content, "Content/maps/FROSTLANDS_2.tmx"));

            newLevel = new Level();
            newLevel.SetMap(new Map(Content, "Content/maps/FROSTLANDS_3.tmx"));

            newLevel = new Level();
            newLevel.SetMap(new Map(Content, "Content/maps/FROSTLANDS_4.tmx"));

            newLevel = new Level();
            newLevel.SetMap(new Map(Content, "Content/maps/FROSTLANDS_5.tmx"));

            newLevel = new Level();
            newLevel.SetMap(new Map(Content, "Content/maps/THUNDERLANDS_1.tmx"));

            newLevel = new Level();
            newLevel.SetMap(new Map(Content, "Content/maps/THUNDERLANDS_2.tmx"));

            newLevel = new Level();
            newLevel.SetMap(new Map(Content, "Content/maps/THUNDERLANDS_3.tmx"));

            newLevel = new Level();
            newLevel.SetMap(new Map(Content, "Content/maps/THUNDERLANDS_4.tmx"));

            newLevel = new Level();
            newLevel.SetMap(new Map(Content, "Content/maps/THUNDERLANDS_5.tmx"));

            Player.LoadContent(Content);
            Player.Sprite = new AnimatedSprite(Player.playerAnimation);
            Player.State = Action.IdleSouth1;

            EscapeMenu = new EscapeMenu(game, window, Content);
            SaveMenu = new SaveMenu(game, window, Content);
            LoadMenu = new LoadMenu(game, window, Content);

            string[] items = { "Continue", "Save", "Load", "Quit" };
            EscapeMenu.SetMenuItems(items);

            ItemInventory.MenuTitle = "Items";
            ItemInventory.InventoryType = "PLAYER_INVENTORY";
          //  ItemInventory.Contents.Add(Items.GetItemById(14));
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
            Item manaPotion = Items.GetItemById(4);
            ItemShopInventory.Contents.Add(Items.GetItemById(4));
            // Homing Crystal
            ItemShopInventory.Contents.Add(Items.GetItemById(11));
            // Spell Power upgrade 
            ItemShopInventory.Contents.Add(Items.GetItemById(62));

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

            Shops = new Shop();
            Shops.Add(ItemShopInventory);
            Shops.Add(SpellShopInventory);
            Shops.Add(SellShopInventory);

            PlayerStatus = new PlayerStatus(Content);
            DialogBox = new DialogBox(game, Font);

            SelectedScene = Scene.CASTLE;

            _whiteTexture = new Texture2D(GraphicsDevice, 1, 1);
            _whiteTexture.SetData(new[] { Color.White });

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            // If save was loaded, create transition effects, assign the player's saved scene and position.
            if (Reloaded || NewGame)
            {
                UnloadContent();
                LoadContent();
                TransitionState = true;
                SelectedScene = Scene.CASTLE;
                SelectedMap = LevelList.Find(map => map.GetLevelName() == "CASTLE").GetMap();
                FadeInMap("CASTLE");
                Player.State = Action.IdleSouth1;
                Player.InMenu = false;
                Player.Position = LevelList.Find(level => level.GetLevelName() == Scene.CASTLE.ToString()).GetStartingPosition();
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
                            case ("RANDOM_FROSTLANDS"):
                                GetRandomLevel("FROSTLANDS");
                                break;
                            case ("RANDOM_THUNDERLANDS"):
                                GetRandomLevel("THUNDERLANDS");
                                break;
                            case ("RANDOM_ALL"):
                                Random randomLevel = new Random();
                                int levelType = randomLevel.Next(1, 5);
                                string level = "";
                                switch(levelType)
                                {
                                    case (1):
                                        level = "PLAINS";
                                        break;
                                    case (2):
                                        level = "FIRELANDS";
                                        break;
                                    case (3):
                                        level = "FROSTLANDS";
                                        break;
                                    case (4):
                                        level = "THUNDERLANDS";
                                        break;
                                }

                                GetRandomLevel(level);
                                break;
                            default:
                                SelectedLevel = LevelList.Find(l => l.GetLevelName() == "CASTLE");
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
                    EscapeMenu.Update(gameTime);
                    break;
                case Scene.SAVE_MENU:
                    SaveMenu.Update(gameTime);
                    break;
                case Scene.LOAD_MENU:
                    LoadMenu.Update(gameTime);
                    break;
            }

            // Scene switching.
            if (SelectedScene == Scene.LOAD_MENU)
            {
                Player.Position = LevelList.Find(map => map.GetLevelName() == Scene.CASTLE.ToString()).GetStartingPosition();
                playerCollision = LevelList.Find(level => level.GetLevelName() == "CASTLE").GetMap().GetCollisionWorld();
            }
            else
            {
                // Find the selected scene.
                foreach (Level level in LevelList)
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
                    foreach (Level level in LevelList)
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

            Shops.ListenForInput(KeyBoardNewState, KeyBoardOldState);
            Shops.Update(gameTime);

            Player.Update(gameTime);
            PlayerStatus.Update(gameTime);

            // Handle player's collision.
            playerCollision.Move(Player.Position.X, Player.Position.Y, (collision) => CollisionResponses.Slide);

            Camera.Zoom = 3f;

            if (!InDialog && !TransitionState && !Player.Dead)
            {
                Player.HandleInput(gameTime, Player, playerCollision, KeyBoardNewState, KeyBoardOldState);
                Player.CheckPlayerStatus(gameTime);
            }

            Camera.LookAt(Player.Position);
            KeyBoardOldState = KeyBoardNewState;
            KeyBoardNewState = Keyboard.GetState();
            EscapeMenu.Position = new Vector2(Player.Position.X, Player.Position.Y - 125);
            SaveMenu.Position = new Vector2(Player.Position.X, Player.Position.Y - 125);
            LoadMenu.Position = new Vector2(Player.Position.X, Player.Position.Y - 125);

            base.Update(gameTime);
        }

        /// <summary>
        /// Generate random level based on level type.
        /// </summary>
        /// <param name="levelType">PLAINS, FIRELANDS, FROSTLANDS, THUNDERLANDS</param>
        public void GetRandomLevel(string levelType)
        {
            FadeInMap("CASTLE");
            Random randomLevel = new Random();
            int levelNum = randomLevel.Next(1, 6);
            //string levelName = levelType + "_" + levelNum;
            string levelName = "PLAINS_1";
            LoadLevel(levelName);
            SelectedScene = (Scene)Enum.Parse(typeof(Scene), levelName);
            SelectedLevel = LevelList.Find(l => l.GetLevelName() == levelName);
            SelectedMap = SelectedLevel.GetMap();
            Player.Position = SelectedLevel.GetStartingPosition();
            FadeInMap(SelectedLevel.GetLevelName());
        }
        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: Camera.GetViewMatrix());

            // Draw the selected screen.
            foreach (Level level in LevelList)
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
                EscapeMenu.Draw(spriteBatch);
            }
            // Save menu.
            else if (SelectedScene == Scene.SAVE_MENU)
            {
                SaveMenu.Draw(spriteBatch);
            }
            // Load menu.
            else if (SelectedScene == Scene.LOAD_MENU)
            {
                if (!Reloaded)
                {
                    LoadMenu.Draw(spriteBatch);
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
                Player.DrawActionBar(spriteBatch);
                //int health = (int)Player.CurrentHealth;
                //Vector2 healthStatus = new Vector2(playerHealthPosition.X + 10, playerHealthPosition.Y);
                //spriteBatch.DrawString(Font, health.ToString() + " / 100", healthStatus, Color.White);

                DialogBox.Draw(spriteBatch);
                ItemInventory.Draw(spriteBatch);
                SpellInventory.Draw(spriteBatch);
                Shops.Draw(spriteBatch);
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

            Shops.Draw(spriteBatch);
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
        public static void FadeInMap(string mapName)
        {
            foreach (Level level in LevelList)
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
            SaveMenu = new SaveMenu(game, window, content);
        }

        public static void OpenLoadMenu(Game game, GameWindow window, ContentManager content)
        {
            LoadMenu = new LoadMenu(game, window, content);
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
            if (MessageFrameCount < 120)
            {
                spriteBatch.DrawString(Init.Font, message, new Vector2(Init.Player.Position.X - 165, Init.Player.Position.Y + 105), Color.White);
                MessageFrameCount++;
            }
            else
            {
                MessageEnabled = false;
                MessageFrameCount = 0;
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