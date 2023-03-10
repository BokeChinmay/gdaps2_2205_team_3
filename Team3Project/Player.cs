using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Team3Project
{
    internal class Player : Entity, IDamageable
    {
        //Fields
        private Texture2D playerTexture;
        private Vector2 playerPos;
        private KeyboardState kbState = new KeyboardState();

        //Consts
        const int PlayerOffsetX = 4;
        const int PlayerOffsetY = 9;
        const int PlayerRectHeight = 26;
        const int PlayerRectWidth = 32;

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="health"></param>
        /// <param name="moveSpeed"></param>
        /// <param name="collision"></param>
        /// <param name="playerPos"></param>
        /// <param name="playerTexture"></param>
        public Player(int health, int moveSpeed, Rectangle collision, Texture2D playerTexture) : base(health, moveSpeed, collision)
        {
            this.playerPos = playerPos;
            this.playerTexture = playerTexture;
        }

        /// <summary>
        /// Makes the character move.
        /// </summary>
        protected override void Move()
        {
            kbState = Keyboard.GetState();

            if (kbState.IsKeyDown(Keys.W))
            {
                playerPos.X--;
            }

            if (kbState.IsKeyDown(Keys.S))
            {
                playerPos.X++;
            }

            if (kbState.IsKeyDown(Keys.D))
            {
                playerPos.Y++;
            }

            if (kbState.IsKeyDown(Keys.W))
            {
                playerPos.Y--;
            }
        }

        public void TakeDamage(int amount)
        {

        }

        /// <summary>
        /// Draw Method.
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="flipSprite"></param>
        /// <returns></returns>
        public void Draw(SpriteBatch spriteBatch, SpriteEffects flipSprite)
        {
            spriteBatch.Draw(playerTexture,
                             playerPos,
                             new Rectangle(
                                 PlayerOffsetX,
                                 PlayerOffsetY,
                                 PlayerRectWidth,
                                 PlayerRectHeight),
                             Color.White,
                             0,
                             Vector2.Zero,
                             1.0f,
                             flipSprite,
                             0
                             );
        }
    }
}
