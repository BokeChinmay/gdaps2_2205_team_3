using Microsoft.Xna.Framework;
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
        public Vector2 playerPos;
        KeyboardState kbState = new KeyboardState();
        
        public Player(int health, int moveSpeed, Rectangle collision, Vector2 playerPos) : base(health, moveSpeed, collision)
        {
            this.playerPos = playerPos;
        }

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
    }
}
