using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using Team3Project.Player_Stuff;
using Team3Project.Stage_Stuff;

namespace Team3Project
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SpriteEffects _spriteEffects;
        private Texture2D mainCharacter;
        private Player playerEntity;
        private Item items;
        private Texture2D speedBoost;
        private StageObjectManager stageObjectManager;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            stageObjectManager = new StageObjectManager();
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _graphics.PreferredBackBufferWidth = 1500;  // Window Width
            _graphics.PreferredBackBufferHeight = 900;   // Window Height
            _graphics.ApplyChanges();


            base.Initialize();
        }

        protected override void LoadContent()
        {
            // TODO: use this.Content to load your game content here
            mainCharacter = this.Content.Load<Texture2D>("Meo");
            playerEntity = new Player(100, 5, new Rectangle(10, 10, 32, 32), mainCharacter);

            stageObjectManager.LoadContent(this.Content, _graphics);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            playerEntity.Move();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            playerEntity.Draw(_spriteBatch, SpriteEffects.None);
            stageObjectManager.Draw(_spriteBatch);
            //items.Draw(_spriteBatch, SpriteEffects.None);
            
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}