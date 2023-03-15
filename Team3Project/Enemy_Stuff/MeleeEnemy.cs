using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Team3Project.Enemy_Stuff
{
    internal class MeleeEnemy : Enemy, IDamageable
    {
        //Fields
        int health;
        int moveSpeed;
        Rectangle collision;
        //PLACEHOLDER PLAYER POSITION VECTOR
        Vector2 playerPos;

        public MeleeEnemy(int health, int moveSpeed, Rectangle collision) : base(health, moveSpeed, collision)
        {

        }

        protected override void Move()
        {
            //Moves directly toward player
            //Calculate unit vector
            Vector2 displacement = playerPos - new Vector2(collision.X, collision.Y);
            double xDisplacement = Math.Pow(playerPos.X - collision.X, 2);
            double yDisplacement = Math.Pow(playerPos.Y - collision.Y, 2);
            float distance = (float)Math.Sqrt(xDisplacement + yDisplacement);
            Vector2 unitVector = displacement / distance;

            //Multiply by speed and move
            collision.X += (int)unitVector.X * moveSpeed;
            collision.Y += (int)unitVector.Y * moveSpeed;
        }

        public void TakeDamage(int amount)
        {

        }
    }
}
