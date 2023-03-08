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
        private Vector2 playerLoc;
        private KeyboardState kbState;

        //Constants for Source Rectangle
        const int PlayerRectOffsetY = 8;
        const int PlayerRectHeight = 20;
        public Player(int health, int moveSpeed) : base(health, moveSpeed)
        {

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

        protected override bool Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(playerTexture, 
                             playerLoc, 
                             new Rectangle(
                                 0, 
                                 )
        }
    }
}
