using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using Team3Project.Enemy_Stuff;
using Team3Project.Player_Stuff;
using Team3Project.Stage_Stuff;

namespace Team3Project
{
    enum GameState
    {
        Menu,
        GamePlaying,
        Demoted,
        GameOver,
        Controls,
        Pause
    }

    public delegate void newLevelDelegate(int level);
    public delegate void itemDropDelegate(Vector2 pos, bool stageClear);

    public class Game1 : Game
    {
        // Graphics fields
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SpriteEffects _spriteEffects;
        private SpriteFont menuFont;

        // Fields related to states
        private GameState _gameState;
        private KeyboardState kbState;
        private KeyboardState prevKbState;
        private MouseState mouseState;
        private MouseState prevMouseState;
        private Random rng;
        
        // Fields for textures for the player
        private Texture2D mainCharacter;
        private Player playerEntity;
        private Texture2D playerMeleeTexture;
        private Texture2D playerBulletTexture;

        // Fields for items
        private Texture2D damageBoost;
        private Texture2D speedBoost;
        private Texture2D extraLife;
        private Texture2D healthPickup;
        private Item items;

        // Field for the stage object manager
        private StageObjectManager stageObjectManager;

        // Fields for enemies
        private Texture2D enemyAsset;
        private List<Enemy> enemyEntities;
        private Texture2D meleeEnemy;
        private Texture2D rangedEnemy;
        private Texture2D enemyBullet;

        private Texture2D smallHBBack;
        private Texture2D smallHBFront;
        private Texture2D largeHBBack;
        private Texture2D largeHBFront;

        // Fields for the UI
        private Texture2D gameTitle;
        private Texture2D titleOption1;
        private Texture2D titleOption2;
        private Texture2D controls;
        private Texture2D pause;
        private Texture2D demoted;
        private Texture2D gameOver;
        private int titleOption;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            stageObjectManager = new StageObjectManager();
            enemyEntities = new List<Enemy>();
            _gameState = GameState.Menu;
            titleOption = 1;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _graphics.PreferredBackBufferWidth = 1500;  // Window Width
            _graphics.PreferredBackBufferHeight = 900;   // Window Height
            _graphics.ApplyChanges();

            rng = new Random();
            LevelManager.Initialize();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // TODO: use this.Content to load your game content here
            
            // Loading player textures
            mainCharacter = this.Content.Load<Texture2D>("Meo");
            playerMeleeTexture = this.Content.Load<Texture2D>("MeleeAttack");
            playerBulletTexture = this.Content.Load<Texture2D>("PlayerBullet");
            playerEntity = new Player(3, 3, 3, 5, 20, new Rectangle(734, 864, 32, 32), mainCharacter, playerMeleeTexture, playerBulletTexture);

            // Subscribing the central GameOver method to the player's relevant event
            playerEntity.gameOver += GameOver;
            playerEntity.lostLife += Demotion;

            // Loading item textures
            damageBoost = this.Content.Load<Texture2D>("DamageUp");
            speedBoost = this.Content.Load<Texture2D>("SpeedBoost");
            extraLife = this.Content.Load<Texture2D>("Full_Life_v2");
            healthPickup = this.Content.Load<Texture2D>("Health_Pickup");

            // Loading enemy textures, and passing them into the level manager
            meleeEnemy = this.Content.Load<Texture2D>("MeleeEnemySpritesheet");
            rangedEnemy = this.Content.Load<Texture2D>("RangedEnemySpritesheet");
            enemyBullet = this.Content.Load<Texture2D>("EnemyBullet");

            smallHBBack = this.Content.Load<Texture2D>("Small_Healthbar_Back");
            smallHBFront = this.Content.Load<Texture2D>("Small_Healthbar_Front");
            largeHBBack = this.Content.Load<Texture2D>("Big_Healthbar_Back");
            largeHBFront = this.Content.Load<Texture2D>("Big_Healthbar_Front");
            LevelManager.SetUpTextures(
                meleeEnemy, rangedEnemy, enemyBullet, 
                damageBoost, speedBoost, extraLife, healthPickup, 
                smallHBBack, smallHBFront, largeHBBack, largeHBFront);

            // Loading UI and menu elements
            menuFont = this.Content.Load<SpriteFont>("MenuFont");
            gameTitle = this.Content.Load<Texture2D>("MEOWCH_logo");
            titleOption1 = this.Content.Load<Texture2D>("Main_Menu_1");
            titleOption2 = this.Content.Load<Texture2D>("Main_Menu_2");
            controls = this.Content.Load<Texture2D>("Controls_v2");
            pause = this.Content.Load<Texture2D>("Pause_Menu");
            gameOver = this.Content.Load<Texture2D>("Game_Over");
            demoted = this.Content.Load<Texture2D>("Demoted");

            _spriteBatch = new SpriteBatch(GraphicsDevice);

            if (rng.Next(0, 1) == 0)
            {
                items = new Item(1, 0,
                                new Rectangle(GraphicsDeviceManager.DefaultBackBufferWidth, GraphicsDeviceManager.DefaultBackBufferHeight, 20, 20),
                                damageBoost, ItemType.DamageBoost);
            }
            else
            {
                items = new Item(1, 0,
                                new Rectangle(GraphicsDeviceManager.DefaultBackBufferWidth, GraphicsDeviceManager.DefaultBackBufferHeight, 20, 20),
                                speedBoost, ItemType.SpeedBoost);
            }

            // Loading the stage object manager's content, and subscribing the central load new
            // level method to its elevator's relevant event
            stageObjectManager.LoadContent(this.Content, _graphics);
            stageObjectManager.Elevator.NewLevel += LoadNewLevel;
        }

        /// <summary>
        /// File IO Saving
        /// </summary>
        public void SaveData()
        {
            StreamWriter streamWriter = null;
            try
            {
                streamWriter = new StreamWriter("../../../savedData.txt");
                streamWriter.WriteLine("Data present");
                streamWriter.WriteLine(playerEntity.Health);
                streamWriter.WriteLine(playerEntity.Lives);
                streamWriter.WriteLine(playerEntity.MoveSpeed);
                streamWriter.WriteLine(playerEntity.Level);
                streamWriter.WriteLine(playerEntity.ProjectileDamage);
                streamWriter.Close();
            }
            catch(Exception ex) {}
        }

        /// <summary>
        /// File IO Loading
        /// </summary>
        public void LoadData()
        {
            StreamReader streamReader = null;
            try
            {
                streamReader = new StreamReader("../../../savedData.txt");
                if (streamReader.ReadLine() != "empty")
                {
                    int playerHealth = int.Parse(streamReader.ReadLine());
                    int playerLives = int.Parse(streamReader.ReadLine());
                    int playerMoveSpeed = int.Parse(streamReader.ReadLine());
                    int playerLevel = int.Parse(streamReader.ReadLine());
                    int playerDamage = int.Parse(streamReader.ReadLine());

                    playerEntity.Health = playerHealth;
                    playerEntity.Lives = playerLives;
                    playerEntity.MoveSpeed = playerMoveSpeed;
                    playerEntity.Level = playerLevel;
                    playerEntity.Checkpoint = (10 * (playerLevel / 10)) + 1;
                    playerEntity.ProjectileDamage = playerDamage;
                }
                streamReader.Close();
            }
            catch (Exception ex) { }
        }

        /// <summary>
        /// File IO data clearing (for use mostly when the player loses)
        /// </summary>
        public void ClearData()
        {
            StreamWriter streamWriter = null;
            try
            {
                streamWriter = new StreamWriter("../../../savedData.txt");
                streamWriter.WriteLine("empty");
                streamWriter.Close();
            }
            catch (Exception ex) { }
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            // Getting current keyboard and mouse states
            mouseState = Mouse.GetState();
            kbState = Keyboard.GetState();
            
            // OVERARCHING FSM
            
            // Updating when the game is being played
            if (_gameState == GameState.GamePlaying)
            {
                if (playerEntity.Active)
                {
                    stageObjectManager.Update(LevelManager.EnemyList, playerEntity);
                    playerEntity.Update(kbState);

                    _spriteBatch.Begin();
                    playerEntity.MeleeAttack(mouseState, prevMouseState, kbState, _spriteBatch);
                    playerEntity.RangedAttack(mouseState, prevMouseState, kbState, _spriteBatch);
                    _spriteBatch.End();

                    LevelManager.Update(playerEntity, gameTime);                  

                    stageObjectManager.Elevator.PlayerEnters(playerEntity);

                    //items.CheckCollision(playerEntity);

                    // Allowing the game to be paused
                    if (kbState.IsKeyUp(Keys.P) && prevKbState.IsKeyDown(Keys.P))
                    {
                        _gameState = GameState.Pause;
                    }
                }
                else if (playerEntity.Lives >= 1)
                {
                    _gameState = GameState.Demoted;
                }
                else
                {
                    _gameState = GameState.GameOver;
                }
            }
            // Updating while in the main menu
            else if (_gameState == GameState.Menu)
            {
                // Allowing the gameplay state to be started
                if ((kbState.IsKeyUp(Keys.Space) && prevKbState.IsKeyDown(Keys.Space)) && titleOption == 1)
                {
                    _gameState = GameState.GamePlaying;
                    stageObjectManager.GenerateLevel(playerEntity.Level);
                    LoadData();
                    LevelManager.LoadNewLevel(stageObjectManager.ObstructiveStageObjects, playerEntity.Level);
                }

                // Allowing the controls display state to be started
                if ((kbState.IsKeyUp(Keys.Space) && prevKbState.IsKeyDown(Keys.Space)) && titleOption == 2)
                {
                    _gameState = GameState.Controls;
                }

                // Allowing the main menu to be navigated
                if (((kbState.IsKeyUp(Keys.Down) && prevKbState.IsKeyDown(Keys.Down)) || 
                    (kbState.IsKeyUp(Keys.S) && prevKbState.IsKeyDown(Keys.S))) && titleOption == 1)
                {
                    titleOption = 2;
                }

                if (((kbState.IsKeyUp(Keys.Up) && prevKbState.IsKeyDown(Keys.Up)) ||
                    (kbState.IsKeyUp(Keys.W) && prevKbState.IsKeyDown(Keys.W))) && titleOption == 2)
                {
                    titleOption = 1;
                }

                // Testing command for checking data clearing
                if (kbState.IsKeyUp(Keys.NumPad1) && prevKbState.IsKeyDown(Keys.NumPad1))
                {
                    ClearData();
                }

                // Testing command for checking the boss stage
                if (kbState.IsKeyUp(Keys.NumPad2) && prevKbState.IsKeyDown(Keys.NumPad2))
                {
                    playerEntity.Level = 10;
                    stageObjectManager.CurrentLevel = 10;
                }
            }
            // Updating while in the controls display
            else if (_gameState == GameState.Controls)
            {
                // Allowing returning to the main menu
                if (kbState.IsKeyUp(Keys.R) && prevKbState.IsKeyDown(Keys.R))
                {
                    _gameState = GameState.Menu;
                }
            }
            // Updating while in the pause menu
            else if (_gameState == GameState.Pause)
            {
                // Returning to the game
                if (kbState.IsKeyUp(Keys.R) && prevKbState.IsKeyDown(Keys.R))
                {
                    _gameState = GameState.GamePlaying;
                }

                // Returning to the menu
                if (kbState.IsKeyUp(Keys.Q) && prevKbState.IsKeyDown(Keys.Q))
                {
                    _gameState = GameState.Menu;
                }

                // Saving current progress
                if (kbState.IsKeyUp(Keys.S) && prevKbState.IsKeyDown(Keys.S))
                {
                    SaveData();
                }
            }
            // Updating while in the game over screen
            else if (_gameState == GameState.GameOver)
            {
                // Returning to the menu
                if (kbState.IsKeyUp(Keys.R) && prevKbState.IsKeyDown(Keys.R))
                {
                    _gameState = GameState.Menu;
                }
            }
            // Updating while in the demotion screen
            else if (_gameState == GameState.Demoted)
            {
                if (playerEntity.Lives == 1)
                {
                    _gameState = GameState.GameOver;
                }

                // Returning to the menu
                if (kbState.IsKeyUp(Keys.C) && prevKbState.IsKeyDown(Keys.C))
                {
                    Demotion();
                    _gameState = GameState.GamePlaying;
                }
            }

            // Getting previous keyboard and mouse states
            prevMouseState = mouseState;
            prevKbState = kbState;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            // Drawing the gameplay state
            if (_gameState == GameState.GamePlaying)
            {
                GraphicsDevice.Clear(Color.DarkGray);

                _spriteBatch.Begin();

                stageObjectManager.Draw1(_spriteBatch);
                playerEntity.Draw(_spriteBatch, SpriteEffects.None);
                //items.Draw(_spriteBatch, SpriteEffects.None);

                LevelManager.Draw(_spriteBatch, SpriteEffects.None, menuFont);
                stageObjectManager.Draw2(_spriteBatch);
                LevelManager.DrawItemDictionary(_spriteBatch, SpriteEffects.None, damageBoost, speedBoost, menuFont);
            }
            // Drawing the main menu
            else if (_gameState == GameState.Menu)
            {
                GraphicsDevice.Clear(Color.Black);
                
                _spriteBatch.Begin();

                _spriteBatch.Draw(gameTitle, new Rectangle(120, 200, 1280, 250), Color.White);

                // Checking what option the player is on
                if (titleOption == 1)
                {
                    _spriteBatch.Draw(titleOption1, new Rectangle(350, 600, 800, 200), Color.White);
                }
                else if (titleOption == 2) 
                {
                    _spriteBatch.Draw(titleOption2, new Rectangle(350, 600, 800, 200), Color.White);
                }
            }
            // Drawing the controls display
            else if (_gameState == GameState.Controls)
            {
                GraphicsDevice.Clear(Color.Black);

                _spriteBatch.Begin();

                _spriteBatch.Draw(controls, new Rectangle(273, 274, 954, 378), Color.White);
            }
            // Drawing the pause menu
            else if (_gameState == GameState.Pause)
            {
                GraphicsDevice.Clear(Color.Black);

                _spriteBatch.Begin();

                _spriteBatch.Draw(controls, new Rectangle(273, 200, 954, 378), Color.White);
                _spriteBatch.Draw(pause, new Rectangle(365, 578, 770, 130), Color.White);
            }
            // Drawing the game over screen
            else if (_gameState == GameState.GameOver)
            {
                GraphicsDevice.Clear(Color.Black);

                _spriteBatch.Begin();

                _spriteBatch.Draw(gameOver, new Rectangle(150, 250, 1200, 400), Color.White);
            }
            // Drawing the demotion screen
            else if (_gameState == GameState.Demoted)
            {
                GraphicsDevice.Clear(Color.Black);

                _spriteBatch.Begin();

                _spriteBatch.Draw(demoted, new Rectangle(140, 230, 1220, 440), Color.White);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        protected void Demotion()
        {
            playerEntity.Demoted();
            LevelManager.LoadNewLevel(stageObjectManager.ObstructiveStageObjects, playerEntity.Level);
        }

        /// <summary>
        /// A group of actions to occur when the player dies
        /// </summary>
        protected void GameOver()
        {
            _gameState = GameState.GameOver;
            ClearData();

            playerEntity.Health = 3;
            playerEntity.Lives = 3;
            playerEntity.MoveSpeed = 5;
            playerEntity.Level = 1;
            playerEntity.ProjectileDamage = 20;
        }

        /// <summary>
        /// A group of actions to occur when a new level is generated
        /// </summary>
        protected void LoadNewLevel(int level)
        {
            playerEntity.nextLevel();
            LevelManager.LoadNewLevel(stageObjectManager.ObstructiveStageObjects, playerEntity.Level);
        }
    }
}