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
        GameOver
    }

    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SpriteEffects _spriteEffects;

        private SpriteFont menuFont;
        private GameState _gameState;
        private KeyboardState kbState;
        private KeyboardState prevKbState;
        private Random rng;
        
        private Texture2D mainCharacter;
        private Player playerEntity;
        private Texture2D playerMeleeTexture;
        private Texture2D playerBulletTexture;


        private Texture2D damageBoost;
        private Texture2D speedBoost;
        private Item items;
        
        private Texture2D enemyAsset;
        private List<Enemy> enemyEntities;
        
        private StageObjectManager stageObjectManager;

        private Texture2D meleeEnemy;
        private Texture2D rangedEnemy;
        private Texture2D enemyBullet;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            stageObjectManager = new StageObjectManager();
            enemyEntities = new List<Enemy>();
            _gameState = GameState.Menu;
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
            mainCharacter = this.Content.Load<Texture2D>("Meo");
            playerMeleeTexture = this.Content.Load<Texture2D>("MeleeAttack");
            playerBulletTexture = this.Content.Load<Texture2D>("PlayerBullet");
            playerEntity = new Player(100, 5, new Rectangle(185, 864, 32, 32), mainCharacter, playerMeleeTexture, playerBulletTexture);

            damageBoost = this.Content.Load<Texture2D>("DamageUp");
            speedBoost = this.Content.Load<Texture2D>("SpeedBoost");


            // meleeEnemy = this.Content.Load<Texture2D>("MeleeEnemySheet");
            meleeEnemy = this.Content.Load<Texture2D>("MeleeEnemySpritesheet");
            rangedEnemy = this.Content.Load<Texture2D>("RangedEnemySpritesheet");
            enemyBullet = this.Content.Load<Texture2D>("EnemyBullet");
            LevelManager.SetUpLevel(meleeEnemy, rangedEnemy, enemyBullet);

            menuFont = this.Content.Load<SpriteFont>("MenuFont");

            _spriteBatch = new SpriteBatch(GraphicsDevice);

            if(rng.Next(0,1) == 0)
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

            stageObjectManager.LoadContent(this.Content, _graphics);

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
                streamWriter.WriteLine(playerEntity.Health);
                streamWriter.WriteLine(playerEntity.MoveSpeed);
                streamWriter.WriteLine(playerEntity.Collision.X + ", " + playerEntity.Collision.Y);
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
                int playerHealth = int.Parse(streamReader.ReadLine());
                int playerMoveSpeed = int.Parse(streamReader.ReadLine());
                //--------------------------------
                // Temporary code adjusting player move speed for testing
                playerMoveSpeed = 5;
                //--------------------------------
                string[] playerPositions = streamReader.ReadLine().Split(',');
                int playerPosX = int.Parse(playerPositions[0]);
                int playerPosY = int.Parse(playerPositions[1]);
                playerEntity = new Player(playerHealth, playerMoveSpeed, new Rectangle(playerPosX, playerPosY, 32, 32), mainCharacter, playerBulletTexture);

                
            }
            catch (Exception ex) { }
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            kbState = Keyboard.GetState();
            
            if (_gameState == GameState.GamePlaying)
            {
                if (playerEntity.Active)
                {
                    stageObjectManager.Update(LevelManager.EnemyList, playerEntity);
                    playerEntity.Move(kbState);
                    playerEntity.RangedAttack(kbState, _spriteBatch);

                    LevelManager.Update(playerEntity.Collision, gameTime);
                }
                else
                {
                    _gameState = GameState.GameOver;
                }
            }
            else if (_gameState == GameState.Menu)
            {
                if (kbState.IsKeyUp(Keys.Space) && prevKbState.IsKeyDown(Keys.Space))
                {
                    _gameState = GameState.GamePlaying;
                    stageObjectManager.GenerateLevel();
                }
            }

            prevKbState = kbState;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkGray);
            
            _spriteBatch.Begin();

            if (_gameState == GameState.GamePlaying)
            {
                stageObjectManager.Draw(_spriteBatch);
                playerEntity.Draw(_spriteBatch, SpriteEffects.None);
                //items.Draw(_spriteBatch, SpriteEffects.None);

                LevelManager.Draw(_spriteBatch, SpriteEffects.None);
            }
            else if (_gameState == GameState.Menu)
            {
                _spriteBatch.DrawString(menuFont, "Meowch", 
                    new Vector2(_graphics.PreferredBackBufferWidth/2 - 100, 
                    _graphics.PreferredBackBufferHeight/2 - 24), Color.Green);
                _spriteBatch.DrawString(menuFont, "Use WASD to move", 
                    new Vector2(_graphics.PreferredBackBufferWidth/2 - 175, 
                    _graphics.PreferredBackBufferHeight/2 + 36), Color.Green);
                _spriteBatch.DrawString(menuFont, "Press space to begin",
                    new Vector2(_graphics.PreferredBackBufferWidth / 2 - 180,
                    _graphics.PreferredBackBufferHeight / 2 + 72), Color.Green);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}