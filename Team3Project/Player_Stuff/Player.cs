using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Team3Project.Player_Stuff
{
    internal class Player : Entity, IDamageable
    {
        //Fields
        private Texture2D playerTexture;
        private KeyboardState kbState;
        private Random rng = new Random();

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
        /// <param name="playerTexture"></param>
        public Player(int health, int moveSpeed, Rectangle collision, Texture2D playerTexture) 
            : base(health, moveSpeed, collision)
        {
            this.playerTexture = playerTexture;
        }

        /// <summary>
        /// Makes the character move.
        /// </summary>
        public void Move(KeyboardState kbState)
        {

            if (kbState.IsKeyDown(Keys.A) && !leftBlocked)
            {
                collision.X-= moveSpeed;
            }

            if (kbState.IsKeyDown(Keys.D) && !rightBlocked)
            {
                collision.X+= moveSpeed;
            }

            if (kbState.IsKeyDown(Keys.S) && !bottomBlocked)
            {
                collision.Y+= moveSpeed;
            }

            if (kbState.IsKeyDown(Keys.W) && !topBlocked)
            {
                collision.Y-= moveSpeed;
            }
        }

        /// <summary>
        /// Method to check the collision between two objects.
        /// </summary>
        /// <param name="check"></param>
        /// <returns></returns>
        public void CheckCollision(Entity check)
        {
            if (check.Collision.Intersects(collision) )
            {
                TakeDamage(1);
            }
            else if(check is Item && check.Collision.Intersects(collision))
            {
                if (rng.Next(1) == 0)
                {
                    moveSpeed++;
                }

                else if (rng.Next(1) == 1)
                {
                    health++;
                }

            }
        }

        /// <summary>
        /// Method that simulates taking damage and sets the entity to inactive if the health reaches zero
        /// </summary>
        /// <param name="damage"></param>
        /// <param name="entity"></param>
        public void TakeDamage(int damage)
        {
            Health = Health - damage;

            if (Health == 0)
            {
                Active = false;
            }
        }

        /// <summary>
        /// Draw Method.
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="flipSprite"></param>
        /// <returns></returns>
        public override void Draw(SpriteBatch spriteBatch, SpriteEffects flipSprite)
        {
            spriteBatch.Draw(playerTexture,
                             collision,
                             new Rectangle(
                                 PlayerOffsetX,
                                 PlayerOffsetY,
                                 PlayerRectWidth,
                                 PlayerRectHeight),
                             Color.White,
                             0,
                             Vector2.Zero,
                             flipSprite,
                             0
                             );
        }

        public override void Update()
        {
            if(Health < 0)
            {
                Active = false;
            }

            // When adding attack capabilities to the player, make left click shoot and right click melee
        }
    }
}
