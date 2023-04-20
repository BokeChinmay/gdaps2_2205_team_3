using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

//Name: Bullet
//Purpose: A class for bullet projectiles that move in a straight line.
//Restrictions: Inherits from Projectile

namespace Team3Project
{
    internal class Bullet : Projectile
    {
        // A vector for the projectile's direction
        private Vector2 direction;
        

        public Bullet(int speed, float xDirection, float yDirection, Rectangle collision, int damage, Texture2D texture, bool friendly) : base(1, speed, collision, texture, friendly)
        {
            direction = new Vector2(xDirection * speed, yDirection * speed);
            this.damage = damage;
        }

        /// <summary>
        /// Moves in the direction of the direction vector
        /// </summary>
        public void Move()
        {
            //Update position by direction vector
            collision.X += (int)direction.X;
            collision.Y += (int)direction.Y;

            //There's a method in StageObjectManager now that checks this based on collision boxes

            //Check if the direction the bullet is moving in is blocked. If so, set active to false
            if (direction.X > 0 && rightBlocked)
            {
                active = false;
            }
            if (direction.X < 0 && leftBlocked)
            {
                active = false;
            }
            if (direction.Y > 0 && bottomBlocked)
            {
                active = false;
            }
            if (direction.Y < 0 && topBlocked)
            {
                active = false;
            }
        }

        public override void Update()
        {
            if (Friendly)
            {
                foreach (Enemy_Stuff.Enemy enemy in LevelManager.EnemyList)
                {
                    if (collision.Intersects(enemy.Collision))
                    {
                        active = false;
                    }
                }
            }
            Move();
        }

    }
}