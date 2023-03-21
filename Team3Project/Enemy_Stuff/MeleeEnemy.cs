using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Team3Project.Enemy_Stuff
{
    internal class MeleeEnemy : Enemy, IDamageable
    {
        //****Note: removed unnecessary code because I apparently don't understand how classes work

        public MeleeEnemy(int health, int moveSpeed, Rectangle collision) : base(health, moveSpeed, collision)
        {
            
        }

        public override void Move()
        {
            
        }

        //Name: MoveTowardPos
        //Purpose: Moves entity one speed unit directly toward a Vector
        //Params: Vector containing X and Y positions of the target position.
        public void MoveTowardPos(Vector2 targetPos)
        {
            //Calculate unit vector
            Vector2 displacement = targetPos - new Vector2(collision.X, collision.Y);
            float distance = DistanceFromPlayer(targetPos);
            Vector2 unitVector = displacement / distance;

            //Multiply by speed and move
            collision.X += (int)unitVector.X * moveSpeed;
            collision.Y += (int)unitVector.Y * moveSpeed;
        }

        //Name: DistanceFromPlayer
        //Purpose: Determines an enemy's distance from the player
        //Params: Vector containing X and Y positions of the player
        public float DistanceFromPlayer(Vector2 playerPos)
        {
            //Find displacement vector
            Vector2 displacement = playerPos - new Vector2(collision.X, collision.Y);

            //Determine distance using distance formula
            float distance = (float)Math.Sqrt(Math.Pow(displacement.X, 2) + Math.Pow(displacement.Y, 2));
            return distance;
        }

        public void TakeDamage(int amount)
        {
            health -= amount;
        }

        public override void Update() 
        {
            //Check if player position is within aggro range

            //If more than attack radius away from player, move directly toward player

            //If within attack radius, initiate attack
            throw new NotImplementedException();
        }

        public override void Draw(SpriteBatch spriteBatch, SpriteEffects spriteEffects)
        {
            throw new NotImplementedException();
        }
    }
}
