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

        // Fields relating to stage objects
        private Texture2D bufferTexture;
        private VisualBuffer leftBuffer;
        private VisualBuffer rightBuffer;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
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
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            mainCharacter = this.Content.Load<Texture2D>("Meo");
            playerEntity = new Player(100, 5, new Rectangle(200, 10, 32, 32), mainCharacter);

            bufferTexture = this.Content.Load<Texture2D>("Buffer");
            leftBuffer = new VisualBuffer(0, bufferTexture);
            rightBuffer = new VisualBuffer(_graphics.PreferredBackBufferWidth - bufferTexture.Width, bufferTexture);

            speedBoost = this.Content.Load<Texture2D>("SpeedBoost");
            items = new Item(new Rectangle(300, 200, 32, 32), speedBoost, 0, 1);
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
            GraphicsDevice.Clear(Color.DarkGray);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            leftBuffer.Draw(_spriteBatch, SpriteEffects.None);
            rightBuffer.Draw(_spriteBatch, SpriteEffects.None);
            playerEntity.Draw(_spriteBatch, SpriteEffects.None);
            //items.Draw(_spriteBatch, SpriteEffects.None);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}