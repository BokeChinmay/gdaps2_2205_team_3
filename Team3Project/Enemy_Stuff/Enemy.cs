using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Team3Project.Enemy_Stuff
{
    //Enum for possible vulnerability states
    enum VulnerabilityState
    {
        Vulnerable,
        Invincible
    }

    internal abstract class Enemy : Entity
    {
        //Fields
        int health;
        int moveSpeed;
        Rectangle collision;

        public Enemy(int health, int moveSpeed, Rectangle collision) : base(health, moveSpeed, collision)
        {
            this.health = health;
            this.moveSpeed = moveSpeed;
            this.collision = collision;
        }

        //Update method
        public abstract void Update(Rectangle playerCollision, List<Projectile> projectileList);

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
            collision.X += (int)(unitVector.X * moveSpeed);
            collision.Y += (int)(unitVector.Y * moveSpeed);
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

        //Name: Direction to player
        //Purpose: Gives unit vector in the direction from the enemy to the player
        //Params: Vector containing X and Y positions of the player
        public Vector2 DirectionToPlayer(Vector2 playerPos)
        {
            //Calculate unit vector
            Vector2 displacement = playerPos - new Vector2(collision.X, collision.Y);
            float distance = DistanceFromPlayer(playerPos);
            Vector2 unitVector = displacement / distance;
            return unitVector;
        }

        //Name: Take Damage
        //Purpose: Changes health by damage amount
        //Params: Damage amount
        public void TakeDamage(int amount)
        {
            health -= amount;
        }
    }
}
